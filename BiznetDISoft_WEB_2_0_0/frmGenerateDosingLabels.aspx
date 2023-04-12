<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmGenerateDosingLabels.aspx.vb" Inherits="frmGenerateDosingLabels" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <%--<script type="text/javascript" src="Script/Gridview.js"></script>--%>

    <script type="text/javascript" src="Script/popcalendar.js"></script>

    <script src="Script/Validation.js" language="javascript" type="text/javascript"></script>

    <script src="Script/jquery.dataTables.min.js" type="text/javascript"></script>

    <script src="Script/jquery.dataTables.plugins.js" type="text/javascript"></script>

    <script src="Script/TableTools.min.js" type="text/javascript"></script>

    <style type="text/css">
        .ui-buttonset, .ui-button {
            margin-right: 0em !important;
        }

        .DTTT_button {
            padding: 3px 10px;
            border-left-width: 1px !important;
            border-right-width: 1px !important;
            margin-right: 3px !important;
        }

        .DTTT_container {
            float: left;
            margin-left: 13px;
        }
        /*.dataTables_wrapper {
            width:82% !important;
            margin:0px auto;
        }*/
        #ctl00_CPHLAMBDA_divDosingDetail {
            width: 1068px !important;
        }
    </style>



    <script type="text/javascript" src="Script/scrollablegrid.js"></script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" width="100%">
                <tbody>
                    <tr>
                        <td align="left">
                            <table cellpadding="5px" width="100%">
                                <tbody>
                                    <tr>
                                        <td>
                                            <table cellpadding="5px" width="83%" style="margin: auto;">
                                                <tbody>
                                                    <tr>
                                                        <td colspan="2">
                                                            <button id="Btn3" runat="server" style="display: none;" />
                                                            <cc1:ModalPopupExtender ID="MpeDialogRemarks" runat="server" BackgroundCssClass="modalBackground"
                                                                CancelControlID="ImgPopUp" PopupControlID="DivPopUp" TargetControlID="btn3">
                                                            </cc1:ModalPopupExtender>

                                                            <div class="modal-content modal-sm" id="DivPopUp" style="display:none;" runat="server">
                                                                <div class="modal-header">
                                                                    <img id="ImgPopUp" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px;" />
                                                                    <h2 style="font-size: x-large;">Reason for deleting Labels</h2>
                                                                </div>
                                                                <div class="modal-body">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td align="right">Remarks :
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="TxtRemarks" runat="server" Text="" TextMode="MultiLine" Style="margin: 0px; height: 61px; width: 312px;"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <div class="modal-footer">
                                                                    <asp:Button ID="BtnSaveRemarks" runat="server" CssClass="btn btnsave" OnClientClick="return ValidationforRemarks();" Text="Save" ToolTip="Save" />
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="pheader" runat="server">
                                                                <div id="divExpandable1" runat="server" style="font-weight: bold; font-size: 13px; float: none; color: white; background-color: #1560a1 !important; width: 100%; margin: auto;">
                                                                    <div>
                                                                        <asp:Image ID="imgArrows" runat="server" Style="float: right;" />
                                                                    </div>
                                                                    <div style="text-align: left; margin-left: 3%;">
                                                                        <asp:Label ID="Label1" runat="server" Text="Parameters">
                                                                        </asp:Label>

                                                                    </div>
                                                                </div>
                                                            </asp:Panel>

                                                            <asp:Panel ID="pnltable" runat="server" Style="">
                                                                <table cellpadding="5px" width="100%" style="margin: auto; border: 1px solid; border-color: Black; overflow-y: inherit">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td class="Label" style="width: 30%; text-align: right; white-space: nowrap" valign="top">Project Name/Project No* :
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" TabIndex="1" Width="80%"></asp:TextBox>
                                                                                <asp:Button ID="btnSetProject" runat="server" OnClick="btnSetProject_Click" Style="display: none"
                                                                                    Text=" Project" />
                                                                                <asp:HiddenField ID="HProjectId" runat="server" />
                                                                                <asp:HiddenField ID="HParentProject" runat="server" />
                                                                                <asp:HiddenField ID='HFirstGroupChild' runat="server" />
                                                                                <asp:HiddenField ID="HstarteiMySubjectNo" runat="server" />
                                                                                <asp:HiddenField ID="HstarteiMySubjectNoFromDosing" runat="server" />
                                                                                <asp:HiddenField ID="HSubjectToDelete" runat="server" />
                                                                                <asp:HiddenField ID="HIsTestSite" runat="server" />
                                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                                                                    CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                                                    CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                                                                    OnClientShowing="ClientPopulated" ServiceMethod="GetMyProjectCompletionList" CompletionListElementID="pnlProjectList"
                                                                                    ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True">
                                                                                </cc1:AutoCompleteExtender>
                                                                                <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 300px; overflow: auto; overflow-x: hidden" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="VersionDtl" class="Label" runat="server" style="text-align: center; display: none">
                                                                            <td colspan="2" style="text-align: center;">Version:<asp:Label ID="VersionNo" runat="server" Style="padding-right: 10px"></asp:Label>
                                                                                VersionDate:<asp:Label ID="VersionDate" runat="server" Style="padding-right: 10px"></asp:Label>
                                                                                Status:<img src="images/Freeze.jpg" id="ImageLockUnlock" runat="server" alt="Lock" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="text-align: right;" class="Label">Principal Investigator*:
                                                                            </td>
                                                                            <td align="left" style="white-space: nowrap">
                                                                                <asp:DropDownList ID="ddlPIName" runat="server" AutoPostBack="True" CssClass="dropDownList"
                                                                                    TabIndex="2" Width="61%">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="right" class="Label" style="white-space: nowrap" valign="top">Period* :
                                                                            </td>
                                                                            <td align="left" style="white-space: nowrap">
                                                                                <asp:DropDownList ID="ddlPeriod" runat="server" AutoPostBack="True" CssClass="dropDownList"
                                                                                    TabIndex="3" Width="61%">
                                                                                </asp:DropDownList>
                                                                                <asp:CheckBox ID="chkdoubleblinded" runat="server" onClick="chkDoubleblinded();"
                                                                                    Text="Double Blinded Study" TabIndex="4"></asp:CheckBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="right" class="Label" style="white-space: nowrap" valign="top">Product Type* :
                                                                            </td>
                                                                            <td align="left" style="white-space: nowrap">
                                                                                <asp:DropDownList ID="ddlProductType" runat="server" AutoPostBack="True" CssClass="dropDownList"
                                                                                    TabIndex="5" Width="61%">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="right" class="Label" style="white-space: nowrap" valign="top">Protocol No* :
                                                                            </td>
                                                                            <td align="left" style="white-space: nowrap">
                                                                                <asp:TextBox ID="txtprotocol" runat="server" CssClass="textBox" TabIndex="6" Width="60%"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="right" class="Label" style="white-space: nowrap" valign="top">Drug* :
                                                                            </td>
                                                                            <td align="left" style="white-space: nowrap">
                                                                                <asp:DropDownList ID="ddlDrug" runat="server" AutoPostBack="True" TabIndex="7" CssClass="dropDownList"
                                                                                    Width="61%">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="right" class="Label" style="white-space: nowrap" valign="top">Drug Name For Print* :
                                                                            </td>
                                                                            <td align="left" style="white-space: nowrap">
                                                                                <asp:TextBox ID="txtDrugNameForPrint" runat="server" CssClass="textBox" TabIndex="6" Width="60%" MaxLength="61"
                                                                                    onKeyUp="ValidateComments(this, 'sptxtDrugNameForPrint',60,'Drug Name For Print Must Not be More Than 60 Characters !');"></asp:TextBox>
                                                                                <span id='sptxtDrugNameForPrint' style='color: Red;'>60</span> Characters Left
                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td align="right" class="Label" style="white-space: nowrap" valign="top">Batch no/Lot no*:
                                                                            </td>
                                                                            <td align="left" style="white-space: nowrap">
                                                                                <asp:DropDownList ID="ddlBatchno" runat="server" AutoPostBack="True" TabIndex="8" CssClass="dropDownList"
                                                                                    Width="61%">
                                                                                </asp:DropDownList>
                                                                            </td>

                                                                        </tr>
                                                                        <%-- <tr>
                                                            <td align="right" class="Label" style="white-space: nowrap" valign="top">
                                                                	Lot no*:
                                                            </td>
                                                            <td align="left" style="white-space: nowrap">
                                                                <asp:TextBox ID="txtLotno" runat="server" MaxLength="20"  onBlur="MaxvalueinTextBox(this.id,20)"  CssClass="textBox" TabIndex="9" Width="60%"></asp:TextBox>
                                                            </td>
                                                        </tr>--%>
                                                                        <tr>
                                                                            <td align="right" class="Label" style="white-space: nowrap" valign="top">Strength* :
                                                                            </td>
                                                                            <td align="left" style="white-space: nowrap">
                                                                                <asp:TextBox ID="txtstrength" runat="server" CssClass="textBox" TabIndex="10" Width="60%"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="right" class="Label" style="white-space: nowrap" valign="top">Dosage Form &amp; Unit* :
                                                                            </td>
                                                                            <td align="left" style="white-space: nowrap">
                                                                                <asp:TextBox ID="txtDosage" runat="server" CssClass="textBox" TabIndex="11" Width="60%"></asp:TextBox>
                                                                            </td>
                                                                        </tr>


                                                                        <tr>
                                                                            <td align="right" class="Label" style="white-space: nowrap" valign="top">Expiry Date/Retest Date* :
                                                                            </td>
                                                                            <td align="left" style="white-space: nowrap">
                                                                                <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="textBox" onblur=" return ValidateExpiryDate(this.value,this);"
                                                                                    TabIndex="12" Width="60%"></asp:TextBox>
                                                                                (e.g. 01011984 or 01-Jan-1984)
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="right" class="Label" style="white-space: nowrap" valign="top">Special Instruction:
                                                                            </td>
                                                                            <td align="left" style="white-space: nowrap">
                                                                                <asp:TextBox ID="txtInstruction" runat="server" CssClass="textBox" TabIndex="13" Width="60%"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="right" class="Label" style="white-space: nowrap" valign="top">Storage condition*:
                                                                            </td>
                                                                            <td align="left" style="white-space: nowrap">
                                                                                <asp:TextBox ID="txtcondition" runat="server" CssClass="textBox" TabIndex="14" Width="60%"></asp:TextBox>
                                                                                <asp:CheckBox ID="chkMultidose" runat="server" Text="Multidose" AutoPostBack="true"
                                                                                    onClick="chkMultidose();" TabIndex="15"></asp:CheckBox>
                                                                                <asp:CheckBox ID="chkDeletebarcode" runat="server" onClick="chkDeletebarcode();" AutoPostBack="true"
                                                                                    Text="Regenerate Barcode Label" TabIndex="16"></asp:CheckBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="right" class="Label" style="white-space: nowrap" valign="top">Route Of Administration*:
                                                                            </td>
                                                                            <td align="left" style="white-space: nowrap">
                                                                                <asp:TextBox ID="txtRouteAdmin" runat="server" CssClass="textBox" TabIndex="17" MaxLength="100" Width="60%"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trBlankBarcodes" runat="server">
                                                                            <td align="right" class="Label" style="white-space: nowrap" valign="top">No. Of Blank Labels Per Product:
                                                                            </td>
                                                                            <td align="left" style="white-space: nowrap">
                                                                                <asp:TextBox ID="txtBlanks" runat="server" CssClass="textBox" onblur="validNumber(this);"
                                                                                    TabIndex="18" Text="0" Width="60%"></asp:TextBox>
                                                                            </td>
                                                                        </tr>


                                                                        <div>
                                                                            <tr id="trSubjectRange" style="display: none;" runat="server">
                                                                                <td align="right" class="Label" style="white-space: nowrap" valign="top">Give Subject Range*:
                                                                                </td>
                                                                                <td align="left" style="white-space: nowrap">
                                                                                    <asp:TextBox ID="txtSubjectInitial" runat="server" CssClass="textBox" onblur="return Numeric(this);"
                                                                                        TabIndex="19" Width="15%"></asp:TextBox>To
                                                                    <asp:TextBox ID="txtSubjectLast" runat="server" CssClass="textBox SubjectLast" onblur="return Numeric(this);"
                                                                        TabIndex="20" Width="15%"></asp:TextBox>(e.g. 1001 To 1030)
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trGenerateBlankIpLabel" runat="server" style="display: none">
                                                                                <td align="right" class="Label" style="white-space: nowrap" valign="top">Do You Want To Generate Blank Barcode:
                                                                                </td>
                                                                                <td>
                                                                                    <asp:CheckBox ID="chkGenerateLabel" runat="server" Text="Please Check" Width="30%"></asp:CheckBox>
                                                                                </td>
                                                                            </tr>
                                                                        </div>

                                                                        <tr id="trDispenseQtyPerSubject" runat="server">
                                                                            <td align="right" class="Label" style="white-space: nowrap" valign="top">Dispense Qty/Subject*:
                                                                            </td>
                                                                            <td align="left" style="white-space: nowrap">
                                                                                <asp:TextBox ID="txtDispenseQtyPerSubject" runat="server" CssClass="textBox" onblur="validNumber(this);"
                                                                                    TabIndex="21" Text="0" Width="60%"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="Label" style="white-space: nowrap; text-align: center;" colspan="2">
                                                                                <asp:Button ID="btnSave" runat="server" CausesValidation="True" CssClass="btn btnsave"
                                                                                    OnClientClick="return Validation();" TabIndex="21" ToolTip="Save & Print" Text="Save" />
                                                                                <asp:Button ID="btnCancel" runat="server" CausesValidation="True" CssClass="btn btncancel"
                                                                                    OnClick="btnCancel_Click" TabIndex="22" Text="Cancel" ToolTip="Cancel" />
                                                                                <asp:Button ID="btnCloseNew" runat="server" CausesValidation="True" CssClass="btn btnclose"
                                                                                    OnClick="btnCloseNew_Click" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this); "
                                                                                    TabIndex="23" Text="Exit" ToolTip="Exit"  />
                                                                                <asp:Button ID="BtnAuditTrail" runat="server" CausesValidation="True" CssClass="btn btnaudit"
                                                                                    OnClientClick="return ValidationForAuditbtn();" TabIndex="24"
                                                                                    ToolTip="Audit Trail" />
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </asp:Panel>
                                                            <cc1:CollapsiblePanelExtender ID="Collpase1" runat="server" TargetControlID="pnltable"
                                                                ExpandControlID="pheader" CollapseControlID="pheader" ExpandedImage="images/panelcollapse.png"
                                                                CollapsedImage="images/panelexpand.png" ImageControlID="imgArrows" AutoCollapse="false"
                                                                AutoExpand="false">
                                                            </cc1:CollapsiblePanelExtender>
                                                        </td>

                                                        <tr>
                                                            <td class="TDMandatory" colspan="2" style="white-space: nowrap; text-align: center;"
                                                                valign="top"></td>
                                                        </tr>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <button id="btn2" runat="server" style="display: none;" />
                            <cc1:ModalPopupExtender ID="MpeAudit" runat="server" BackgroundCssClass="modalBackground"
                                CancelControlID="Img1" PopupControlID="DivAudit" TargetControlID="btn2">
                            </cc1:ModalPopupExtender>
                            <div id="DivAudit" runat="server" style="position: relative; display: none; background-color: #cee3ed; padding: 5px; width: 840px; height: inherit; border: dotted 1px gray;"
                                class="centerModalPopup ">
                                <div>
                                    <h1>
                                        <div>
                                            <h1 class="header">
                                                <%--                                                <img id="Img1" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right;
                                                    right: 5px;" title="Close" />--%>
                                                <asp:ImageButton runat="server" ID="Img1" alt="Close" src="images/Sqclose.gif" Style="position: relative; float: right; right: 5px;"
                                                    title="Close" OnClientClick="return $('<%= DivAudit.ClientID %>').hide();" />
                                                <asp:Label ID="lblTitle" Text="Audit Trail" runat="server" class="LabelBold"></asp:Label>
                                            </h1>
                                        </div>
                                </div>
                                <table border="0" cellpadding="2" cellspacing="2" width="40%" style="margin: auto;">
                                    <tbody>
                                        <tr>
                                            <td align="center" colspan="2" style="height: 168px; width: 90%">
                                                <asp:Panel ID="pnlMedExGrid" runat="server" ScrollBars="Auto" Height="300px" Width="800px">
                                                    <asp:GridView ID="GVAudit" runat="server" AutoGenerateColumns="False" BorderColor="Peru"
                                                        Font-Size="Small" SkinID="grdViewAutoSizeMax" Width="92%">
                                                        <Columns>
                                                            <asp:BoundField DataField="nDosingDetailNo" HeaderText="DosingDetailNo" />
                                                            <asp:BoundField DataField="vDosingBarCode" HeaderText="Barcode">
                                                                <ItemStyle Width="20%" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="vProjectNo" HeaderText="Project No." />
                                                            <asp:BoundField DataField="vProtocolNo" HeaderText="Protocol No." />
                                                            <asp:BoundField DataField="vGenericName" HeaderText="Drug" />
                                                            <asp:BoundField DataField="iPeriod" HeaderText="Period" />
                                                            <asp:BoundField DataField="iMySubjectNo" HeaderText="Randomization Sub No">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="vSubjectId" HeaderText="Subject ID">
                                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="vInitials" HeaderText="Subject" />
                                                            <asp:BoundField DataField="vProductType" HeaderText="Type" />
                                                            <asp:BoundField DataField="vRandomizationcode" HeaderText="Randomization Code" />
                                                            <asp:BoundField DataField="iDayNo" HeaderText="Day No." />
                                                            <asp:BoundField DataField="iDoseNo" HeaderText="Dose No." />
                                                            <asp:BoundField DataField="vdosageform" HeaderText="Dosage Form" />
                                                            <asp:BoundField DataField="vStoragecondition" HeaderText="Storage Condition" />
                                                            <asp:BoundField DataField="vStrength" HeaderText="Strength" />
                                                            <asp:BoundField DataField="dExpiryDate_IST" HeaderText="Expiry Date" HtmlEncode="False"
                                                                DataFormatString="{0:dd-MMM-yyyy}">
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="vModifyBy" HeaderText="Modify By">
                                                                <ItemStyle Width="20%" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="dModifyOn" HeaderText="Modify On" HtmlEncode="False">
                                                                <ItemStyle Wrap="false" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="vRemarks" HeaderText="Remarks">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="PanelProjectSpecific" runat="server" Style="margin-top: 2%; display: none">
                                <div id="div1" runat="server" style="font-weight: bold; font-size: 13px; float: none; color: white; background-color: #1560a1; width: 80.8%; margin: 0px auto;">
                                    <div>
                                        <asp:Image ID="ImgProjectSpecific" runat="server" Style="float: right;" />
                                    </div>
                                    <div style="text-align: left; margin-left: 3%;">
                                        <asp:Label ID="lblprojecthdr" runat="server" Text="Tabular Data">
                                        </asp:Label>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnlgvmedexworkspadce" runat="server" Style="display: none">
                                <table cellpadding="5" style="margin: 0px auto; border: solid 1px; width: 79%;">
                                    <tbody>
                                        <tr>
                                            <td class="Label" style="text-align: center; display: none">
                                                <strong>Dosing Details
                                                <hr style="background-image: none; width: 100%; color: #ffcc66; background-color: #ffcc66" />
                                                </strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center;">
                                                <asp:Button ID="BtnPrint1" runat="server" Text="Print" ToolTip="Print" CssClass="btn btnnew"
                                                    Visible="false" OnClientClick="return GridchecksValidation('P');" />
                                                <asp:Button ID="BtnDelete1" runat="server" Text="Delete" ToolTip="Delete" CssClass="btn btnnew"
                                                    OnClientClick="return GridchecksValidation('D');" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div>
                                                    <div id="divnote" runat="server" style="display: none; text-align: right;">
                                                        <asp:Label runat="server" ID="lblnote" Text="Note : Red Color Indicate Replaced Barcode." Font-Names="verdana" Font-Size="X-Small" ForeColor="Red" Font-Bold="true" />
                                                    </div>
                                                    <div id="divDosingDetail" runat="server" style="display: none; width: 82% !important; margin: 0px auto;">

                                                        <asp:GridView ID="gvwDosingDetail" TabIndex="23" runat="server"
                                                            AutoGenerateColumns="False" Style="">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="BarCode">
                                                                    <HeaderTemplate>
                                                                        <input id="chkSelectAll" onclick="SelectAll(this, 'gvwDosingDetail')" type="checkbox" />
                                                                        <asp:Label ID="lblSelectAll" runat="server" Text="All"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelectBarCode" onclick="CheckUncheckAll('gvwDosingDetail')"
                                                                            runat="server"></asp:CheckBox>
                                                                        <%--<asp:LinkButton id="lnkPrint" runat="server" >Print</asp:LinkButton> --%>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="nDosingDetailNo" HeaderText="nDosingDetailNo" />
                                                                <asp:BoundField DataField="vDosingBarCode" HeaderText="BarCode" />
                                                                <asp:BoundField DataField="vProjectNo" HeaderText="Project No" />
                                                                <asp:BoundField DataField="vProtocolNo" HeaderText="Protocol No" />
                                                                <asp:BoundField DataField="vGenericName" HeaderText="Drug" />
                                                                <asp:BoundField DataField="iPeriod" HeaderText="Period" />
                                                                <asp:BoundField DataField="iMySubjectNo" HeaderText="Randomization Sub No">
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="vSubjectId" HeaderText="Subject ID">
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%" />
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="vInitials" HeaderText="Subject">
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="vProductType" HeaderText="Type" />
                                                                <asp:BoundField DataField="vRandomizationcode" HeaderText="Randomization code" />
                                                                <asp:BoundField DataField="iDayNo" HeaderText="Day No." />
                                                                <asp:BoundField DataField="iDoseNo" HeaderText="Dose No." />
                                                                <asp:BoundField DataField="vdosageform" HeaderText="Dosage Form" />
                                                                <asp:BoundField DataField="vStoragecondition" HeaderText="Storage Condition" />
                                                                <asp:BoundField DataField="vStrength" HeaderText="Strength" />
                                                                <asp:BoundField DataField="dExpiryDate" HeaderText="Expiry Date" HtmlEncode="False"
                                                                    DataFormatString="{0:dd-MMM-yyyy}">
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="vInstruction" HeaderText="Instruction" />
                                                                <asp:TemplateField HeaderText="Assign" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkAssing" runat="server">Assign</asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="dDosedOn" HeaderText="Dosing Time">
                                                                    <ItemStyle Wrap="false" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ReplaceSubject" HeaderText="Replaced Subject" />
                                                                <asp:BoundField DataField="vPIName" HeaderText="PI Name" />
                                                                <asp:BoundField DataField="vRouteAdmin" HeaderText="Route Admin" />
                                                                <asp:BoundField DataField="cReplaceFlag" HeaderText="Replace Flag" Visible="true" />
                                                                <asp:BoundField DataField="vBatchNo" HeaderText="Batch No" />
                                                                <asp:BoundField DataField="iDispenseQtyPerSubject" HeaderText="Dispense Qty/Subject" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                                <div style="left: 25px; width: 600px; position: absolute; top: 901px; height: 150px"
                                                    id="divReplacement" class="divStyleNoAbs" runat="server" visible="false">
                                                    <asp:Panel ID="pnlReplace" runat="server" Visible="False">
                                                        <table cellpadding="5" style="width: 600px">
                                                            <tr>
                                                                <td align="left" class="Label" colspan="2" style="text-align: center" valign="top">IP Lable Assignment
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center" class="Label" colspan="2" style="text-align: left" valign="top">
                                                                    <table style="width: 100%; text-align: left">
                                                                        <tr>
                                                                            <td nowrap="nowrap" style="text-align: left" colspan="2">
                                                                                <asp:Label ID="lblLabelId" runat="server"></asp:Label>
                                                                                Assigned To Subject:
                                                                            <asp:TextBox ID="txtreplaceCode" runat="server" CssClass="textBox" TabIndex="25"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td nowrap="nowrap" style="text-align: right">Remarks:
                                                                            </td>
                                                                            <td nowrap="nowrap" style="text-align: left">
                                                                                <asp:TextBox ID="txtAssignRemark" runat="server" CssClass="textBox" TextMode="MultiLine"
                                                                                    Width="219px"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <%-- <td nowrap="nowrap" style="text-align: right">
                                                                        </td>--%>
                                                                            <td nowrap="nowrap" style="text-align: center" colspan="2">
                                                                                <asp:Button ID="btnReplaceOK" runat="server" CssClass="btn btnnew" OnClientClick="return CheckReplaceRemarks();"
                                                                                    TabIndex="15" Text="Ok" ToolTip="Ok" />
                                                                                <asp:Button ID="btnReplaceCancel" runat="server" CssClass="btn btnnew" Text="Cancel"
                                                                                    ToolTip="Cancel" />
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
                                            <td style="text-align: center;">
                                                <asp:Button ID="BtnPrint2" runat="server" Text="Print" ToolTip="Print" CssClass="btn btnnew"
                                                    Visible="false" OnClientClick="return GridchecksValidation('P');"></asp:Button>
                                                <asp:Button ID="BtnDelete2" runat="server" Text="Delete" CssClass="btn btnnew" ToolTip="Delete"
                                                    OnClientClick="return GridchecksValidation('D');"></asp:Button>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </asp:Panel>

                            <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="pnlgvmedexworkspadce"
                                ExpandControlID="PanelProjectSpecific" CollapseControlID="PanelProjectSpecific"
                                ExpandedImage="images/panelcollapse.png" CollapsedImage="images/panelexpand.png"
                                ImageControlID="ImgProjectSpecific" AutoCollapse="false" AutoExpand="True">
                            </cc1:CollapsiblePanelExtender>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <button id="btnMultidose" runat="server" style="display: none;" CssClass="btn btnnew" />
                            <cc1:ModalPopupExtender ID="mpeMultidose" runat="server" PopupControlID="dialogModalmultidose"
                                PopupDragHandleControlID="LblMultidose" BackgroundCssClass="modalBackground"
                                TargetControlID="btnMultidose">
                            </cc1:ModalPopupExtender>
                            <div id="dialogModalmultidose" runat="server" class="centerModalPopup" style="position: relative; display: none; padding: 5px; width: 600px; height: inherit; border: dotted 1px gray;">
                                <div>
                                    <div>
                                        <%--<img id="dialogModalClose1" alt="Close" title="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px;" onclick ="return Uncheck();" />--%>
                                        <asp:Label ID="LblMultidose" runat="server" class="LabelBold" Text=" Multidose Study"></asp:Label>
                                    </div>
                                </div>
                                <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                    <tr>
                                        <td colspan="5" style="height: 5px;">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="text-align: right">Total No. of Days:
                                        </td>
                                        <td style="text-align: center">
                                            <asp:TextBox ID="txtTotaldays" runat="Server" Text="" CssClass="textBox" Width="50px"
                                                onblur="return Numeric(this);" MaxLength="3" Height="20px"></asp:TextBox>
                                        </td>
                                        <td style="width: 20px;"></td>
                                        <td align="right" style="text-align: right">Total No. of Doses:
                                        </td>
                                        <td style="text-align: center">
                                            <asp:TextBox ID="txtTotaldoses" runat="Server" Text="" CssClass="textBox" Width="50px"
                                                onblur="return Numeric(this);" AutoPostBack="true" MaxLength="3" Height="20px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" style="height: 5px;"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" style="height: 5px;" align="center">
                                            <div>
                                                <asp:GridView ID="grdMultidose" runat="server" ShowFooter="true" SkinID="grdViewSmlAutoSize"
                                                    AutoGenerateColumns="false" Width="60%">
                                                    <Columns>
                                                        <asp:BoundField DataField="Days" HeaderText="Days" />
                                                        <asp:TemplateField HeaderText="Doses">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtMultidose" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Doses") %>'
                                                                    onblur="return Numeric(this);"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" style="height: 5px;"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" align="center">
                                            <hr />
                                            <asp:Button ID="btnOk" runat="server" Text="Ok" ToolTip="Confirm" CssClass="btn btnnew"
                                                OnClientClick="return ValidationForMultidose();"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btnCancelMultidose" runat="server" CausesValidation="True" CssClass="btn btncancel"
                                                OnClick="btnCancelMultidose_Click" TabIndex="15" Text="Cancel" ToolTip="Cancel" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" style="height: 5px;"></td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <button id="btnDeletedLabels" runat="server" style="display: none;" CssClass="btn btncancel" />
                            <cc1:ModalPopupExtender ID="mpeDeletedLabels" runat="server" PopupControlID="dialogModalDeletedLabels"
                                PopupDragHandleControlID="LblDeletedLabels" BackgroundCssClass="modalBackground"
                                TargetControlID="btnDeletedLabels">
                            </cc1:ModalPopupExtender>
                            <div id="dialogModalDeletedLabels" runat="server" style="position: relative; display: none; background-color: #FFFFFF; padding: 5px; width: 600; height: inherit; border: dotted 1px gray;">
                                <div>
                                    <div>
                                        <%--<img id="dialogModalClose2" alt="Close" title="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px;" />--%>
                                        <asp:Label ID="LblDeletedLabels" runat="server" class="LabelBold" Text=" Generate Deleted Labels"></asp:Label>
                                    </div>
                                </div>
                                <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                    <tr>
                                        <td colspan="5" style="height: 5px;">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="text-align: right">Days:
                                        </td>
                                        <td style="text-align: center">
                                            <asp:TextBox ID="txtdays" runat="Server" Text="" CssClass="textBox" Width="50px"
                                                onblur="return Numeric(this);" MaxLength="3" Height="20px"></asp:TextBox>
                                        </td>
                                        <td style="width: 20px;"></td>
                                        <td align="right" style="text-align: right">Doses:
                                        </td>
                                        <td style="text-align: center">
                                            <asp:TextBox ID="txtdoses" runat="Server" Text="" CssClass="textBox" Width="50px"
                                                onblur="return Numeric(this);" MaxLength="3" Height="20px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" style="height: 5px;"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" style="height: 5px;"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" align="center">
                                            <asp:Button ID="btnOkDeletebarcode" runat="server" Text="Ok" ToolTip="Confirm" CssClass="btn btnnew"
                                                OnClientClick="return ValidationForDeletebarcode();"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btnCancelDeletedLabels" runat="server" CausesValidation="True" CssClass="btn btncancel"
                                                OnClick="btnCancelDeletedLabels_Click" TabIndex="15" Text="Cancel" ToolTip="Cancel" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" style="height: 5px;"></td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    &nbsp;
    <script type="text/javascript" language="javascript">

        $(document).ready(function () {
            var txtDrugNameForPrint = $get('<%= txtDrugNameForPrint.ClientID%>');
            if (txtDrugNameForPrint != null && txtDrugNameForPrint != 'undefined') {
                ValidateComments(txtDrugNameForPrint, 'sptxtDrugNameForPrint', 60);
            }
        });

        function pageLoad() {

            $(window).width() > 1180 ? wid = ($(window).width() - 94) + 'px' : wid = $(window).width() - 100 + 'px';
            $('#<%= divDosingDetail.ClientID%>').attr("style", "width:" + wid + ";")

            if ($('#<%= gvwDosingDetail.ClientID%>')) {
                otable = $('#<%= gvwDosingDetail.ClientID%>').prepend($('<thead>').append($('#<%= gvwDosingDetail.ClientID%> tr:first')))
                     .dataTable({
                         "sScrollY": '250px',
                         "sScrollX": '100%',
                         "bPaginate": false,
                         "bJQueryUI": true,
                         "bFooter": false,
                         "bHeader": false,
                         "bAutoWidth": false,
                         "bInfo": true,
                         "bSort": false,
                         "sDom": '<"H"frT>t<"F"i>',
                         "oLanguage": { "sSearch": "Search (Day No.)" },
                         "oTableTools": {
                             "aButtons": [
                                 "xls"
                             ],
                             "sSwfPath": "Script/swf/copy_cvs_xls_pdf.swf"
                         },
                     });


            }
            $('[id$="gvwDosingDetail_wrapper"]').find(".dataTables_filter").find(":text").unbind().bind("input", function (e) {
                var searchval = $(this).val();
                searchval = searchval.replace(/,/gi, "|")
                $('[id$="gvwDosingDetail"]').dataTable().fnFilter(searchval, 11, true, true)
            })

            //$('input[id*="chkSelectBarCode"]').attr("disabled", true);
        }

        ////// For Project    
        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
                    $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        //For Validation
        function Validation() {

            if (document.getElementById('<%= HProjectId.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Project !');
                document.getElementById('<%= txtProject.ClientId %>').focus();
                document.getElementById('<%= txtProject.ClientId %>').value = '';
                return false;
            }
            else if (document.getElementById('<%= ddlPIName.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select PI Name !');
                document.getElementById('<%= ddlPIName.ClientID%>').focus();
                document.getElementById('<%= chkMultidose.ClientId %>').checked = false;
                return false;

            }
            else if (document.getElementById('<%= ddlPeriod.ClientId %>').selectedIndex == 0) {
                msgalert('Please Select Period !');
                document.getElementById('<%= ddlPeriod.ClientId %>').focus();
                return false;

            }
            else if (document.getElementById('<%= ddlProductType.ClientId %>').selectedIndex == 0 && document.getElementById('<%= chkdoubleblinded.ClientId %>').checked == false) {
                msgalert('Please Select Product Type !');
                document.getElementById('<%= ddlProductType.ClientId %>').focus();
                return false;

            }
            else if (document.getElementById('<%= txtprotocol.ClientId %>').value.trim() == '') {
                msgalert('Please Enter Protocol No !');
                document.getElementById('<%= txtprotocol.ClientId %>').focus();
                return false;
            }
            else if (document.getElementById('<%= ddlDrug.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Drug Name !');
                document.getElementById('<%= ddlDrug.ClientID%>').focus();
                document.getElementById('<%= chkMultidose.ClientId %>').checked = false;
                return false;
            }
            else if (document.getElementById('<%= txtDrugNameForPrint.ClientID%>').value.trim() == '') {
                msgalert('Please Enter Drug Name For Print !');
                document.getElementById('<%= txtDrugNameForPrint.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%= ddlBatchno.ClientID%>').value.trim() == '' || document.getElementById('<%= ddlBatchno.ClientID%>').value.trim() == '0') {
                msgalert('Please Select Batch No/Lot No !');
                document.getElementById('<%= ddlBatchno.ClientID%>').focus();
                document.getElementById('<%= chkMultidose.ClientId %>').checked = false;
                return false;
            }
            else if (document.getElementById('<%= txtstrength.ClientId %>').value.trim() == '') {
                msgalert('Please Enter Strength !');
                document.getElementById('<%= txtstrength.ClientId %>').focus();
                return false;
            }
            else if (document.getElementById('<%= txtDosage.ClientId %>').value.trim() == '') {
                msgalert('Please Enter No of Dosage !');
                document.getElementById('<%= txtDosage.ClientId %>').focus();
                return false;
            }
            else if (document.getElementById('<%= txtExpiryDate.ClientId %>').value.trim() == '') {
                msgalert('Please Enter Expiry Date !');
                document.getElementById('<%= txtExpiryDate.ClientId %>').focus();
                return false;
            }

            else if (document.getElementById('<%= txtcondition.ClientId %>').value.trim() == '') {
                msgalert('Please Enter Storage Condition !');
                document.getElementById('<%= txtcondition.ClientId %>').focus();
                return false;
            }
            else if (document.getElementById('<%= txtRouteAdmin.ClientID%>').value.trim() == '') {
                msgalert('Please Enter Route Admin !');
                document.getElementById('<%= txtRouteAdmin.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%= txtSubjectInitial.ClientId %>').value.trim() == '') {
                msgalert('Please Enter Start Range !');
                document.getElementById('<%= txtRouteAdmin.ClientID%>').focus();
                return false;
            }

            else if (document.getElementById('<%= txtSubjectLast.ClientId %>').value.trim() == '') {
                msgalert('Please Enter To Range !');
                document.getElementById('<%= txtRouteAdmin.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%= txtDispenseQtyPerSubject.ClientID%>').value.trim() == '') {
                msgalert('Please Enter Dispense Qty/Subject !');
                document.getElementById('<%= txtRouteAdmin.ClientID%>').focus();
                return false;
            }

    return true;

}

function validNumber(txt) {

    var result = CheckInteger(txt.value);
    if (result == false) {
        txt.value = '1';
        //txt.focus();
        msgalert('Please Enter Numeric Value !');
    }
    if (txt.id == 'ctl00_CPHLAMBDA_txtDispenseQtyPerSubject') {
        if (CheckQTY()) {
            return true;
        }
        else {
            return false;
        }
    }


}
function runApp(which) {
    WshShell = new ActiveXObject("WScript.Shell");
    WshShell.Run(which, 1, false);
}

function printLbl(strrepBar, strrepDrug, strrepProject, strrepSub, strrepDose, strrepTest) {
    //var strrepBar='0000000010,0000000020,0000000030';
    var ArrstrBar = strrepBar.split(',*');
    var ArrstrDrug = strrepDrug.split(',*');
    var ArrstrProj = strrepProject.split(',*');
    var ArrstrSub = strrepSub.split(',*');
    var ArrstrDose = strrepDose.split(',*');
    var ArrstrTest = strrepTest.split(',*');

    var fso1 = new ActiveXObject("Scripting.FileSystemObject");

    if (fso1.FolderExists("C:\\Batch_File") == false) {
        msgalert('Please Create Folder With Name "Batch_File In C Drive" !');
        return;
        //fso1.CreateFolder("C:\\Batch_File");
    }
    for (var i = 0; i <= ArrstrBar.length - 1; i++) {

        StrToRepBar = ArrstrBar[i];
        StrToRepDrug = ArrstrDrug[i];
        StrToRepProject = ArrstrProj[i];
        StrToRepSub = 'Sub. No.:' + ArrstrSub[i];
        StrToRepDose = 'Dose:' + ArrstrDose[i];
        StrToRepTest = 'Product:' + ArrstrTest[i];

        //alert('a1:' + StrToRepBar);
        //alert('a2:' + StrToRepDrug);
        //alert('a3:' + StrToRepProject);
        //alert('a4:' + StrToRepSub);
        //alert('a5:' + StrToRepDose);
        //alert('a6:' + StrToRepTest);

        ReadFromFile(StrToRepBar, StrToRepDrug, StrToRepProject, StrToRepSub, StrToRepDose, StrToRepTest);

    }

    //ReadFromFile(strrepBar,strrepDrug,strrepProject,strrepSub,strrepDose,strrepTest);


}
function ReadFromFile(StrToRepBar, StrToRepDrug, StrToRepProject, StrToRepSub, StrToRepDose, StrToRepTest) {
    //alert('0');
    var strContent = ReadFileToString("C:\\Batch_File/IPLabel.PRN");
    var str;
    //alert('1');
    //alert(strContent);

    //            for(var i=0; i<=strContent.length -1; i++)
    //            {
    //            str=strContent.replace('Barcode',StrToRepBar);
    //            ReadFromFile(StrToRepBar);
    //            }


    str = strContent
    str = str.replace(/Barcode/g, StrToRepBar);
    //alert('b1:' + str);
    str = str.replace(/Proj/g, StrToRepProject);
    //alert('b2:' + str);
    str = str.replace(/Sub/g, StrToRepSub);
    //alert('b3:' + str);
    str = str.replace(/Dose/g, StrToRepDose);
    //alert('b4:' + str);
    str = str.replace(/Test/g, StrToRepTest);
    //alert('b5:' + str);


    if (StrToRepDrug.length > 37) {
        var StrToRepDrug1 = StrToRepDrug.substr(0, 37);
        str = str.replace(/Drug1/g, StrToRepDrug1);

        StrToRepDrug = StrToRepDrug.substr(37);

        if (StrToRepDrug.length > 37) {
            var StrToRepDrug2 = StrToRepDrug.substr(0, 37);
            str = str.replace(/Drug2/g, StrToRepDrug2);

            StrToRepDrug = StrToRepDrug.substr(37);

            if (StrToRepDrug.length > 37) {
                var StrToRepDrug3 = StrToRepDrug.substr(0, 36);
                str = str.replace(/Drug3/g, StrToRepDrug3);

            }
            else {
                var StrToRepDrug3 = StrToRepDrug.substr(0);
                str = str.replace(/Drug3/g, StrToRepDrug3);

            }
        }
        else {
            var StrToRepDrug2 = StrToRepDrug.substr(0);
            str = str.replace(/Drug2/g, StrToRepDrug2);
            str = str.replace(/Drug3/g, '');
        }


    }
    else {
        var StrToRepDrug1 = StrToRepDrug.substr(0);
        str = str.replace(/Drug1/g, StrToRepDrug1);
        str = str.replace(/Drug2/g, '');
        str = str.replace(/Drug3/g, '');
    }

    //alert('b6:' + str);

    var fso = new ActiveXObject("Scripting.FileSystemObject");
    fso.CreateTextFile("C:\\Batch_File/IPLabel_New.PRN", true); //overwriteFlag= true/false

    varFileObject = fso.OpenTextFile("C:\\Batch_File/IPLabel_New.PRN", 2, true, 0);

    varFileObject.write(str);
    varFileObject.close();

    //    document.Form1.HdnContent.value = strContent;
    //    document.Form1.submit();

    //Need to Uncomments for print barcode
    runApp('file://C:/Batch_File/DosingBarcodePrint.bat');

}

function ReadFileToString(strFileName) {
    var strContents;
    strContents = "";
    try {
        objFSO = new ActiveXObject("Scripting.FileSystemObject");
        if (objFSO.FileExists(strFileName)) {
            strContents = objFSO.OpenTextFile(strFileName, 1).ReadAll();
        }

        return strContents;
    }
    catch (err) {
        msgalert(err.description);
    }
} //end of function

function SelectAll(CheckBoxControl, Grid) {
    if (CheckBoxControl.checked == true)

        $('input[type="checkbox"]:not([disabled])', $('#<%= gvwDosingDetail.ClientID%>' + '_wrapper')).attr('checked', true);
    else
        $('input[type="checkbox"]:not([disabled])', $('#<%= gvwDosingDetail.ClientID%>' + '_wrapper')).attr('checked', false);
    //            if (CheckBoxControl.checked == true) {
    //                var i;
    //                for (i = 0; i < document.forms[0].elements.length; i++) {
    //                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf(Grid) > -1)) {
    //                        if (document.forms[0].elements[i].disabled == false) {
    //                            document.forms[0].elements[i].checked = true;
    //                        }
    //                    }
    //                }
    //            }
    //            else {
    //                var i;
    //                for (i = 0; i < document.forms[0].elements.length; i++) {
    //                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf(Grid) > -1)) {
    //                        document.forms[0].elements[i].checked = false;
    //                    }
    //                }
    //            }
}


function CheckUncheckAll(Grid) {
    var Checkall = document.getElementById('chkSelectAll');
    var Gvd = document.getElementById('<%=gvwDosingDetail.ClientId %>');
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

function CheckReplaceRemarks() {
    var txt = document.getElementById('<%=txtAssignRemark.clientid %>').value.trim();
    if (txt == '') {
        msgalert('Either Select Or Enter Remarks !');
        document.getElementById('<%=txtAssignRemark.clientid %>').focus();
        return false;
    }
    return true;
}


function GridchecksValidation(OpType) {

    var gvwDosingDetail = document.getElementById('<%= gvwDosingDetail.ClientID %>');
    if (gvwDosingDetail == null || typeof (gvwDosingDetail) == 'undefined') {
        return false;
    }
        //alert(CheckOne('gvwSample'));
    else if ((CheckOne(gvwDosingDetail.id) == false) && OpType == 'D') {
        msgalert('Select Atleast One IPLabel To Delete !');
        return false;
    }

    else if ((CheckOne(gvwDosingDetail.id) == false) && OpType == 'P') {
        msgalert('Select Atleast One IPLabel To Print !');
        return false;
    }

    return true;
}


function ValidationforRemarks() {
    if (document.getElementById('<%= TxtRemarks.ClientId %>').value.trim() == '') {
        msgalert('Please Enter Remarks !');
        document.getElementById('<%= TxtRemarks.ClientId %>').focus();
        return false;
    }
    return true;
}

function ValidateExpiryDate(ParamDate, txtdate) {


    if (txtdate.value != "") {
        if (!DateConvert(ParamDate, txtdate)) {

        }

        if (CheckDateLessThenToday(txtdate.value)) {
            txtdate.value = "";
            txtdate.focus();
            msgalert('Expiry Date Should Be Greater Than Current Date !');
            return false;
        }
    }
    return true
}

function ValidationForAuditbtn() {

    if (document.getElementById('<%= HProjectId.ClientId %>').value.toString().trim().length <= 0) {
        msgalert('Please Enter Project !');
        document.getElementById('<%= txtProject.ClientId %>').focus();
        document.getElementById('<%= txtProject.ClientId %>').value = '';
        return false;
    }
    else if (document.getElementById('<%= ddlPeriod.ClientId %>').selectedIndex == 0) {
        msgalert('Please Select Period !');
        document.getElementById('<%= ddlPeriod.ClientId %>').focus();
        return false;

    }
    return true;
}

function chkDoubleblinded() {


    if (document.getElementById('<%= chkdoubleblinded.ClientId %>').checked == true) {
        //                document.getElementById('<%= ddlProductType.ClientId %>').selectedindex = 0;
        //                document.getElementById('<%= ddlProductType.ClientId %>').disabled = true;
        document.getElementById('<%= txtBlanks.ClientId %>').disabled = true;

    }
    else if (document.getElementById('<%= chkdoubleblinded.ClientId %>').checked == false) {
        //                document.getElementById('<%= ddlProductType.ClientId %>').disabled = false;
        document.getElementById('<%= txtBlanks.ClientId %>').disabled = false;
    }
}

function validNumberForSubject(txt) {
    var initialSubjectno = txt.value
    var subjectcheck = $('#ctl00_CPHLAMBDA_HstarteiMySubjectNo').val()
    var subjectcheck1 = ('<%=HstarteiMySubjectNo.ClientID %>').Value
    var result = CheckInteger(txt.value);
    if (result == false) {
        txt.value = '1001';
        msgalert('Please Enter Numeric Value !');
    }
}

function ValidationForMultidose() {

    if ($get('<%= txtTotaldays.ClientID%>').value == '') {
        msgalert('Please Enter Total days !');
        $get('<%= txtTotaldays.ClientID%>').focus();
        return false;
    }
    else if ($get('<%= txtTotaldoses.ClientID%>').value == '') {
        msgalert('Please Enter Total doses !');
        $get('<%= txtTotaldoses.ClientID%>').focus();
        return false;
    }
    else if (parseInt($get('<%= txtTotaldays.ClientID%>').value) > parseInt($get('<%= txtTotaldoses.ClientID%>').value)) {
        msgalert('Days Cannot Be Greater Than Doses !');
        $get('<%= txtTotaldoses.ClientID%>').value = '';
        $get('<%= txtTotaldays.ClientID%>').focus();
        return false;
    }
    var Totdose = 0;
    var gvDrv = document.getElementById('ctl00_CPHLAMBDA_grdMultidose');
    for (i = 1; i < gvDrv.rows.length - 2; i++) {
        if (gvDrv.rows[i].cells[1].children[0].value == '') {
            msgalert('Doses cannot be blank.Please Enter doses !');
            return false;
        }
        Totdose += parseInt(gvDrv.rows[i].cells[1].children[0].value);
    }
    if (Totdose > parseInt($get('ctl00_CPHLAMBDA_txtTotaldoses').value) || Totdose < parseInt($get('ctl00_CPHLAMBDA_txtTotaldoses').value)) {
        msgalert('Entered Doses cannot be greater or less than the total number of doses !');
        return false;

    }
}
function ValidationForDeletebarcode() {

    if ($get('<%= txtdays.ClientID%>').value == '') {
        msgalert('Please Enter days !');
        $get('<%= txtdays.ClientID%>').focus();
        return false;
    }
    else if ($get('<%= txtdoses.ClientID%>').value == '') {
        msgalert('Please Enter doses !');
        $get('<%= txtdoses.ClientID%>').focus();
        return false;
    }
}
function chkMultidose() {
    if (document.getElementById('<%= HProjectId.ClientId %>').value.toString().trim().length <= 0) {
        msgalert('Please Enter Project !');
        document.getElementById('<%= txtProject.ClientId %>').focus();
        document.getElementById('<%= txtProject.ClientId %>').value = '';
        document.getElementById('<%= chkMultidose.ClientId %>').checked = false;
        return false;
    }
    else if (document.getElementById('<%= ddlPIName.ClientID%>').selectedIndex == 0) {
        msgalert('Please Select PI Name !');
        document.getElementById('<%= ddlPIName.ClientID%>').focus();
        document.getElementById('<%= chkMultidose.ClientId %>').checked = false;
        return false;

    }
    else if (document.getElementById('<%= ddlPeriod.ClientId %>').selectedIndex == 0) {
        msgalert('Please Select Period !');
        document.getElementById('<%= ddlPeriod.ClientId %>').focus();
        document.getElementById('<%= chkMultidose.ClientId %>').checked = false;
        return false;

    }

    else if (document.getElementById('<%= ddlProductType.ClientId %>').selectedIndex == 0 && document.getElementById('<%= chkdoubleblinded.ClientId %>').checked == false) {
        msgalert('Please Select ProductType !');
        document.getElementById('<%= ddlProductType.ClientId %>').focus();
        document.getElementById('<%= chkMultidose.ClientId %>').checked = false;
        return false;

    }

    else if (document.getElementById('<%= txtprotocol.ClientId %>').value.trim() == '') {
        msgalert('Please Enter Protocol No !');
        document.getElementById('<%= txtprotocol.ClientId %>').focus();
        document.getElementById('<%= chkMultidose.ClientId %>').checked = false;
        return false;
    }
    else if (document.getElementById('<%= ddlDrug.ClientID%>').selectedIndex == 0) {
        msgalert('Please Select Drug Name !');
        document.getElementById('<%= ddlDrug.ClientID%>').focus();
        document.getElementById('<%= chkMultidose.ClientId %>').checked = false;
        return false;

    }
    else if (document.getElementById('<%= txtDrugNameForPrint.ClientID%>').value.trim() == '') {
        msgalert('Please Enter Drug Name For Print !');
        document.getElementById('<%= txtDrugNameForPrint.ClientID%>').focus();

        return false;
    }
    else if (document.getElementById('<%= txtstrength.ClientId %>').value.trim() == '') {
        msgalert('Please Enter Strength !');
        document.getElementById('<%= txtstrength.ClientId %>').focus();
        document.getElementById('<%= chkMultidose.ClientId %>').checked = false;
        return false;
    }
    else if (document.getElementById('<%= txtExpiryDate.ClientId %>').value.trim() == '') {
        msgalert('Please Enter Expiry Date !');
        document.getElementById('<%= txtExpiryDate.ClientId %>').focus();
        document.getElementById('<%= chkMultidose.ClientId %>').checked = false;
        return false;
    }

    else if (document.getElementById('<%= txtcondition.ClientId %>').value.trim() == '') {
        msgalert('Please Enter Storage Condition !');
        document.getElementById('<%= txtcondition.ClientId %>').focus();
        document.getElementById('<%= chkMultidose.ClientId %>').checked = false;
        return false;
    }
}
function chkDeletebarcode() {
    if (document.getElementById('<%= HProjectId.ClientId %>').value.toString().trim().length <= 0) {
        msgalert('Please Enter Project !');
        document.getElementById('<%= txtProject.ClientId %>').focus();
        document.getElementById('<%= txtProject.ClientId %>').value = '';
        document.getElementById('<%= chkDeletebarcode.ClientId %>').checked = false;
        return false;
    }
    else if (document.getElementById('<%= ddlPIName.ClientID%>').selectedIndex == 0) {
        msgalert('Please Select PI Name !');
        document.getElementById('<%= ddlPIName.ClientID%>').focus();
        document.getElementById('<%= chkMultidose.ClientId %>').checked = false;
        return false;

    }
    else if (document.getElementById('<%= ddlPeriod.ClientId %>').selectedIndex == 0) {
        msgalert('Please Select Period !');
        document.getElementById('<%= ddlPeriod.ClientId %>').focus();
        document.getElementById('<%= chkDeletebarcode.ClientId %>').checked = false;
        return false;

    }

    else if (document.getElementById('<%= ddlProductType.ClientId %>').selectedIndex == 0 && document.getElementById('<%= chkdoubleblinded.ClientId %>').checked == false) {
        msgalert('Please Select ProductType !');
        document.getElementById('<%= ddlProductType.ClientId %>').focus();
        document.getElementById('<%= chkDeletebarcode.ClientId %>').checked = false;
        return false;

    }

    else if (document.getElementById('<%= txtprotocol.ClientId %>').value.trim() == '') {
        msgalert('Please Enter Protocol No !');
        document.getElementById('<%= txtprotocol.ClientId %>').focus();
        document.getElementById('<%= chkDeletebarcode.ClientId %>').checked = false;
        return false;
    }
    else if (document.getElementById('<%= ddlDrug.ClientID%>').selectedIndex == 0) {
        msgalert('Please Select Drug Name !');
        document.getElementById('<%= ddlDrug.ClientID%>').focus();
        document.getElementById('<%= chkMultidose.ClientId %>').checked = false;
        return false;
    }
    else if (document.getElementById('<%= txtDrugNameForPrint.ClientID%>').value.trim() == '') {
        msgalert('Please Enter Drug Name For Print !');
        document.getElementById('<%= txtDrugNameForPrint.ClientID%>').focus();

        return false;
    }
    else if (document.getElementById('<%= txtstrength.ClientId %>').value.trim() == '') {
        msgalert('Please Enter Strength !');
        document.getElementById('<%= txtstrength.ClientId %>').focus();
        document.getElementById('<%= chkDeletebarcode.ClientId %>').checked = false;
        return false;
    }
    else if (document.getElementById('<%= txtExpiryDate.ClientId %>').value.trim() == '') {
        msgalert('Please Enter Expiry Date !');
        document.getElementById('<%= txtExpiryDate.ClientId %>').focus();
        document.getElementById('<%= chkDeletebarcode.ClientId %>').checked = false;
        return false;
    }

    else if (document.getElementById('<%= txtcondition.ClientId %>').value.trim() == '') {
        msgalert('Please Enter Storage Condition !');
        document.getElementById('<%= txtcondition.ClientId %>').focus();
        document.getElementById('<%= chkDeletebarcode.ClientId %>').checked = false;
        return false;
    }
}
function Numeric(e) {
    var ValidChars = "0123456789";
    var Numeric = true;
    var Char;

    sText = e.value.trim();
    if (sText.trim() != '') {
        if (sText > 0) {
            for (i = 0; i < sText.length && Numeric == true; i++) {
                Char = sText.charAt(i);
                if (ValidChars.indexOf(Char) == -1) {
                    msgalert('Please Enter Value Greater than Zero !');
                    e.value = '';
                    //e.focus();
                    Numeric = false;
                }
            }
        }
        else {
            msgalert('Please Enter Value Greater than Zero !');
            e.value = '';
            e.focus();
            Numeric = false;
        }
    }
    if (e.id == 'ctl00_CPHLAMBDA_txtSubjectInitial' || e.id == 'ctl00_CPHLAMBDA_txtSubjectLast') {
        CheckQTY();
    }

    return Numeric;
}

function MaxvalueinTextBox(txt, maxSize) {
    cntField = document.getElementById(cntField);
    if (txt.value.length > maxSize) {
        txt.value = txt.value.substring(0, maxSize);
    }
}

function CheckQTY() {

    if (document.getElementById('<%= HProjectId.ClientId %>').value.toString().trim().length <= 0) {
        msgalert('Please Enter Project !');
        document.getElementById('<%= txtProject.ClientId %>').focus();
        document.getElementById('<%= txtProject.ClientId %>').value = '';
        document.getElementById('<%= txtDispenseQtyPerSubject.ClientID%>').value = '';
        return false;
    }
    else if (document.getElementById('<%= ddlDrug.ClientID%>').selectedIndex == 0) {
        msgalert('Please select Drug Name !');
        document.getElementById('<%= ddlDrug.ClientID%>').focus();
        document.getElementById('<%= chkMultidose.ClientId %>').checked = false;
        document.getElementById('<%= txtDispenseQtyPerSubject.ClientID%>').value = '';
        return false;
    }
    else if (document.getElementById('<%= txtDrugNameForPrint.ClientID%>').value.trim() == '') {
        msgalert('Please Enter Drug Name For Print !');
        document.getElementById('<%= txtDrugNameForPrint.ClientID%>').focus();

        return false;
    }
    else if (document.getElementById('<%= ddlBatchno.ClientID%>').selectedIndex == 0) {
        msgalert('Please Select Batch No/Lot No !');
        document.getElementById('<%= chkMultidose.ClientId %>').checked = false;
        document.getElementById('<%= ddlBatchno.ClientID%>').focus();
        document.getElementById('<%= txtDispenseQtyPerSubject.ClientID%>').value = '';
        return false;
    }
    else if (document.getElementById('<%= txtSubjectInitial.ClientId %>').value.trim() == '') {
        msgalert('Please Enter Start Range !');
        document.getElementById('<%= txtDispenseQtyPerSubject.ClientID%>').value = '';
        document.getElementById('<%= txtRouteAdmin.ClientID%>').focus();

        return false;
    }

    else if (document.getElementById('<%= txtSubjectLast.ClientId %>').value.trim() == '') {
        msgalert('Please Enter To Range !');
        document.getElementById('<%= txtDispenseQtyPerSubject.ClientID%>').value = '';
        document.getElementById('<%= txtRouteAdmin.ClientID%>').focus();

        return false;
    }

    if (GetRandomizationDetailCount() == false) {
        document.getElementById('<%= txtDispenseQtyPerSubject.ClientID%>').value = '';
        return false;

    }

    var QuantityCheck = {
        vWorkSpaceId: document.getElementById('<%= HProjectId.ClientId %>').value.toString().trim(),
        vProductNo: ',' + $('#ctl00_CPHLAMBDA_ddlDrug').val() + ',',
        nStudyProductBatchNo: ',' + $('#ctl00_CPHLAMBDA_ddlBatchno').val() + ',',
        vRefModule: "PD",
    }
    var ApiUrl_PMS = '<%=ConfigurationManager.AppSettings("ApiUrl_PMS").ToString()%>';
    var BaseUrl = ApiUrl_PMS;
    var GetPmsQuntitytyCheckProductName = {
        Url: BaseUrl + "PmsGeneral/QtyDetail",
        SuccessMethod: "SuccessMethod",
        Data: QuantityCheck
    }
    QuntityCheckMaster(GetPmsQuntitytyCheckProductName.Url, GetPmsQuntitytyCheckProductName.SuccessMethod, GetPmsQuntitytyCheckProductName.Data);
}


var RandomizationDetailRowCount = 0;
var DosingDetailCount = 0;

var QuntityCheckMaster = function (Url, SuccessMethod, Data) {
    $.ajax({
        url: Url,
        type: 'POST',
        data: Data,
        async: false,
        success: Success,
        error: function () {
            msgalert("Data Not Found !");
        }
    });
    function Success(response) {

        var rows = 0;
        if (document.getElementById('<%= gvwDosingDetail.ClientID%>') != null) {
            rows = otable.dataTable().fnGetNodes();
        }
        var SubjectLast = parseInt(document.getElementById('<%= txtSubjectLast.ClientId %>').value.trim());
        var SubjectInitial = parseInt(document.getElementById('<%= txtSubjectInitial.ClientID%>').value.trim());
        var DispenseQty = parseInt($("#ctl00_CPHLAMBDA_txtDispenseQtyPerSubject").val());
        var GenerateBlankIpLabel = parseInt($("#ctl00_CPHLAMBDA_txtBlanks").val()) * parseInt(DispenseQty);
        var GenerateDosingQty = DosingDetailCount;
        //DispenseQty = DispenseQty * ((SubjectLast - SubjectInitial) + 1);
        DispenseQty = DispenseQty * parseInt(RandomizationDetailRowCount);
        DispenseQty = parseInt(DispenseQty) + parseInt(GenerateBlankIpLabel);

        var MultiDoses = 0;
        if (document.getElementById('<%= chkMultidose.ClientId %>').checked == true) {
            if (($('[id$="txtTotaldoses"]').val() == undefined) || ($('[id$="txtTotaldoses"]').val().trim() == "")) {
                MultiDoses = 0;
            }
            else {
                MultiDoses = parseInt($('[id$="txtTotaldoses"]').val().trim())
            }
            MultiDoses = parseInt($("#ctl00_CPHLAMBDA_txtDispenseQtyPerSubject").val()) * MultiDoses;
            DispenseQty = MultiDoses;
        }

        var ProductType = $("#ctl00_CPHLAMBDA_ddlProductType option:selected").text().toString().trim();
        var DrugName = $("#ctl00_CPHLAMBDA_ddlDrug option:selected").text().toString().trim();
        var BatchName = $("#ctl00_CPHLAMBDA_ddlBatchno option:selected").text().toString().trim();

        if (SubjectLast < SubjectInitial) {
            msgalert('Invalid Subject Range !');
            document.getElementById('<%= txtSubjectLast.ClientId %>').value = "";
            document.getElementById('<%= txtSubjectInitial.ClientID%>').value = "";
            return false;
        }

        TotalQuantity = DispenseQty;
        if (isNaN(TotalQuantity)) { return false; }

        if (TotalQuantity == 0) {
            msgalert('Please Enter Valid Quantity !');
            $("#ctl00_CPHLAMBDA_txtDispenseQtyPerSubject").val('');
            return false;
        }
        if ((parseInt(response) - parseInt(GenerateDosingQty)) > 0) {
            if ((parseInt(response) - parseInt(GenerateDosingQty)) < parseInt(DispenseQty)) {
                $("#ctl00_CPHLAMBDA_txtDispenseQtyPerSubject").val('');
                msgalert('Current Stock Is ' + (parseInt(response) - parseInt(GenerateDosingQty)) + ' !');
                return false;
            }
        }
        else {
            $("#ctl00_CPHLAMBDA_txtDispenseQtyPerSubject").val('');
            msgalert('No Stock For Barcode Generate !');
            return false;
        }
        if (parseInt(response) < parseInt(TotalQuantity)) {
            $("#ctl00_CPHLAMBDA_txtDispenseQtyPerSubject").val('');
            msgalert('Current Stock Is ' + response + ' !');
            return false;
        }

    }
}

function GetRandomizationDetailCount() {

    var vWorkSpaceId = document.getElementById('<%= HProjectId.ClientId %>').value.toString().trim();
    var iPeriod = $('#ctl00_CPHLAMBDA_ddlPeriod').val();
    var vProductType = $("#ctl00_CPHLAMBDA_ddlProductType option:selected").text().toString().trim();
    var nProductNo = $("#ctl00_CPHLAMBDA_ddlDrug").val();
    var nBatchNo = $("#ctl00_CPHLAMBDA_ddlBatchno").val();
    var iMySubjectNoStart = parseInt(document.getElementById('<%= txtSubjectInitial.ClientID%>').value.trim());
    var iMySubjectNoEnd = parseInt(document.getElementById('<%= txtSubjectLast.ClientId %>').value.trim());

    try {
        $.ajax({
            type: "POST",
            url: "frmGenerateDosingLabels.aspx/GetRandomizationDetailCount",
            data: '{"vWorkSpaceId":"' + vWorkSpaceId + '", "iPeriod":"' + iPeriod + '", "vProductType":"' + vProductType + '" , "iMySubjectNoStart":"' + iMySubjectNoStart + '", "iMySubjectNoEnd":"' + iMySubjectNoEnd + '" , "nProductNo":"' + nProductNo + '" , "nBatchNo":"' + nBatchNo + '" }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.d != "null") {
                    RandomizationDetailRowCount = eval('(' + data.d + ')').Table[0].RandomizationDetailRowCount;
                    DosingDetailCount = eval('(' + data.d + ')').Table1[0].DosingDetailCount;
                    return true;
                }
                else {
                    RandomizationDetailRowCount = 0;
                    DosingDetailCount = 0;
                    return false;
                }
            }
        });
    }
    catch (err) {
        msgalert('GetRandomizationDetailCount function : ' + err.message);
        return false;

    }
}

    </script>
</asp:Content>
