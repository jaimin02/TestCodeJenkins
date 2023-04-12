<%@ Page Language="VB" MasterPageFile="~/ECTDMasterPage.master" AutoEventWireup="false"
    CodeFile="frmCDMSProjectMedicalCondition.aspx.vb" Inherits="CDMS_frmCDMSProjectMedicalCondition" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <link rel="stylesheet" href="../App_Themes/CDMS.css" />

    <script src="../Script/General.js" language="javascript" type="text/javascript"></script>

    <script src="../Script/Validation.js" language="javascript" type="text/javascript"></script>

    <script src="../Script/jquery-1.9.1.js" type="text/javascript"></script>

    <script src="../Script/jquery-ui.js" type="text/javascript"></script>

    <script src="../Script/General.js" type="text/javascript"></script>

    <script src="../Script/AutoComplete.js" type="text/javascript"></script>

    <script src="../Script/jquery.dataTables.min.js" type="text/javascript"></script>

    <div style="text-align: Left; padding-bottom: 2%; padding-left: 9%;">
        <asp:Label ID="project" Text="Project :" CssClass="LabelText" runat="server" Style="margin-left: 28px;"></asp:Label>
        <asp:TextBox ID="txtproject" runat="server" Enabled="false" CssClass="TextBox" Width="40%"
            Style="margin-left: 14px;"></asp:TextBox>
        <asp:Button ID="btnSetProject" runat="server" Style="display: none" OnClientClick="return SetActivetab();" />
        <asp:HiddenField ID="HProjectId" runat="server" />
        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
            TargetControlID="txtProject" ServicePath="~/AutoComplete.asmx" OnClientShowing="ClientPopulated"
            OnClientItemSelected="OnSelected" MinimumPrefixLength="1" ServiceMethod="GetMyProjectCompletionList"
            CompletionListElementID="pnlSelectedDate" CompletionListItemCssClass="autocomplete_listitem"
            CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListCssClass="autocomplete_list"
            BehaviorID="AutoCompleteExtender1">
        </cc1:AutoCompleteExtender>
        <asp:Panel ID="pnlSelectedDate1" runat="server" Style="max-height: 150px; overflow: auto;
            overflow-x: hidden;" />
    </div>
    <div id="tabs" style="text-align: left; width: 99%;">
        <ul>
            <li onclick="fnRedirect();"><a href="#">
                <img alt="Study Information" src="images/Subject.png" style="padding-right: 8px;" />
                Study Information</a></li>
            <li onclick="return clickMedical();"><a href="#tabs-1">
                <img alt="Medical Condition" src="images/Medical Condition.png" style="padding-right: 8px;" />
                Medical Condition</a></li>
            <li onclick="return clickConco();"><a href="#tabs-2">
                <img alt="Conco. Medication" src="images/Medication.png" style="padding-right: 8px;" />
                Conco. Medication</a></li>
        </ul>
        <div id="tabs-1" align="left">
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td colspan="2">
                        <fieldset class="FieldSetBox" style="padding-top: 7px; width: 98%;">
                            <table cellspacing="2" width="100%">
                                <tr>
                                    <td class="LabelText" style="text-align: left; width: 10%;">
                                        Criteria :
                                    </td>
                                    <td class="LabelText" style="text-align: left; font-size: 11px !important;">
                                        <asp:RadioButtonList ID="rblMedi" runat="server" RepeatDirection="Horizontal" ToolTip="Inclusion Or Exclusion Criteria"
                                            AutoPostBack="true">
                                            <asp:ListItem Value="Inclusion" Text="Inclusion" />
                                            <asp:ListItem Value="Exclusion" Text="Exclusion" />
                                        </asp:RadioButtonList>
                                    </td>
                                    <td style="text-align: right;">
                                        <input type="button" id="btnAddMoreMedi" value="Add" title="Add Criteria For Medical Cond."
                                            class="btn btnadd" style="display: none;"
                                            onclick="return ValidationForMedi();" />
                                        <asp:Button ID="btnHistoryCondition" runat="server" Text="History" CssClass="btn btnadd" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr style="height: 8px;">
                    <td colspan="2">
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <fieldset class="FieldSetBox" style="padding-top: 7px; width: 98%;">
                            <legend class="LegendText" style="color: Black">Medical Condition</legend>
                            <table cellpadding="2" cellspacing="2" width="100%">
                                <tr>
                                    <td>
                                        <asp:HiddenField ID="hdnMedicalConditionNo" runat="server" />
                                        <asp:GridView ID="grdMedicalCondition" runat="server" AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:BoundField DataField="vType" HeaderText="Type">
                                                    <ItemStyle HorizontalAlign="Left" Width="16%" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vSubType" HeaderText="Sub Type">
                                                    <ItemStyle HorizontalAlign="Left" Width="17%" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vDescription" HeaderText="Description">
                                                    <ItemStyle HorizontalAlign="Left" Width="17%" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vSymptom" HeaderText="Symptom">
                                                    <ItemStyle HorizontalAlign="Left" Width="17%" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="dOnsetDate" HeaderText="Onset Date" DataFormatString="{0:dd-MMM-yyyy}"
                                                    HtmlEncode="false">
                                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="dResolutionDate" HeaderText="Resolution Date" DataFormatString="{0:dd-MMM-yyyy}"
                                                    HtmlEncode="false">
                                                    <ItemStyle HorizontalAlign="Left" Width="22%" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vCriteria" HeaderText="Criteria" HtmlEncode="false">
                                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Action">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgEdit" runat="server" AlternateText='<% #databinder.eval(container.dataitem,"nProjectDtlCSMSMedicalConditionNo") %>'
                                                            ImageUrl="~/CDMS/images/Edit_Small.png" ToolTip="Edit Medication" Style="cursor: pointer;"
                                                            onclick="fnEditMedicalCondition(this.alt);" />
                                                        <asp:Image ID="imgDelete" runat="server" AlternateText='<% #databinder.eval(container.dataitem,"nProjectDtlCSMSMedicalConditionNo") %>'
                                                            ImageUrl="~/CDMS/images/delete_small.png" ToolTip="Delete Medication" Style="padding-left: 3px;
                                                            cursor: pointer;" onclick="fnDeleteMedicalCondition(this.alt);" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <asp:HiddenField ID="hdnrblvalue" runat="server" />
                    </td>
                </tr>
                <tr style="height: 8px;">
                    <td colspan="2">
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <fieldset class="FieldSetBox" style="padding-top: 7px; width: 98%;">
                            <table cellpadding="2" cellspacing="2" width="100%">
                                <tr>
                                    <td class="LabelText" style="text-align: left; font-size: 12px !important;">
                                        <asp:Button ID="btnMedicalPrevious" runat="server" Text="Previous" CssClass="btn btnpre" />
                                    </td>
                                    <td class="LabelText" style="text-align: right; font-size: 12px !important;">
                                        <asp:Button ID="btnMedicalNext" runat="server" Text="Next" CssClass="btn btnnext" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="BtnMedi" runat="server" Style="display: none;" />
                        <cc1:ModalPopupExtender ID="mpMedicalCond" runat="server" PopupControlID="divMedicalCond"
                            BackgroundCssClass="modalBackground" BehaviorID="mpMedicalCond" CancelControlID="imgCancelmedi"
                            TargetControlID="BtnMedi">
                        </cc1:ModalPopupExtender>
                        <div id="divMedicalCond" runat="server" class="centerModalPopup" style="display: none;
                            overflow: auto; width: 80%; height: auto; max-height: 85%; min-height: auto">
                            <table cellpadding="2" cellspacing="2" width="100%">
                                <tr>
                                    <td class="LabelText" style="text-align: center !important; font-size: 12px !important;
                                        width: 97%;">
                                        <asp:Label ID="Hdrmedication" runat="server"></asp:Label>
                                    </td>
                                    <td style="width: 3%">
                                        <img id="imgCancelmedi" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div style="overflow: auto; width: 100%; max-height: 300px;">
                                            <asp:HiddenField ID="hdnSelectedCondition" runat="server" />
                                            <asp:GridView ID="grdAddMedicalCond" runat="server" AutoGenerateColumns="false">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select">
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkCodeList" runat="server" onclick="fnCheckBokSelectionMedical(this);" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="CodeMedicalConditions" HeaderText="ID Code">
                                                        <ItemStyle HorizontalAlign="Center" Width="12%" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Type" HeaderText="Type">
                                                        <ItemStyle HorizontalAlign="Left" Width="14%" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SubType" HeaderText="Sub Type">
                                                        <ItemStyle HorizontalAlign="Left" Width="14%" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Description" HeaderText="Description">
                                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Symptom" HeaderText="Symptom">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: center;">
                                        <asp:Button ID="btnAddMoreMediCond" runat="server" Text="Add" CssClass="ButtonText"
                                            Width="57px" Style="font-size: 12px !important;" ToolTip="Add Medical Condition Criteria For This Project" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr style="height: 8px;">
                    <td colspan="2">
                        <asp:Button ID="btnEdit" runat="server" Style="display: none;" />
                        <cc1:ModalPopupExtender ID="mpEditMedicalCond" runat="server" PopupControlID="divEditMedicalCond"
                            BackgroundCssClass="modalBackground" BehaviorID="mpEditMedicalCond" CancelControlID="imgEditClosePopup"
                            TargetControlID="btnEdit">
                        </cc1:ModalPopupExtender>
                        <div id="divEditMedicalCond" runat="server" class="centerModalPopup" style="display: none;
                            overflow: auto; width: 650px; height: 315px; max-height: 315px; min-height: 315px">
                            <table cellpadding="2" cellspacing="2" width="100%">
                                <tr>
                                    <td class="LabelText" style="text-align: center !important; font-size: 12px !important;
                                        width: 97%;">
                                        Edit Medical Condition
                                    </td>
                                    <td style="width: 3%">
                                        <img id="imgEditClosePopup" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <table cellpadding="2" cellspacing="2" width="100%" style="padding: 5px;">
                                            <tr>
                                                <td class="LabelText">
                                                    Type :
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtType" Enabled="false" runat="server" CssClass="TextBox" Width="210px" />
                                                </td>
                                                <td class="LabelText">
                                                    Sub Type :
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtSubType" Enabled="false" runat="server" CssClass="TextBox" Width="219px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="LabelText">
                                                    Description :
                                                </td>
                                                <td colspan="3" style="text-align: left">
                                                    <asp:TextBox ID="txtDescrption" Enabled="false" runat="server" CssClass="TextBox"
                                                        Width="541px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="LabelText">
                                                    Onset Date :
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtOnsetDate" runat="server" CssClass="TextBox"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="calOnsetDate" runat="server" TargetControlID="txtOnsetDate"
                                                        Format="dd-MMM-yyyy">
                                                    </cc1:CalendarExtender>
                                                </td>
                                                <td class="LabelText">
                                                    Resolution Date :
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtResolutionDate" runat="server" CssClass="TextBox"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="calResolutionDate" runat="server" TargetControlID="txtResolutionDate"
                                                        Format="dd-MMM-yyyy">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="LabelText">
                                                    Source :
                                                </td>
                                                <td class="LabelText" style="text-align: left;">
                                                    <asp:DropDownList ID="ddlSource" runat="server" Width="156px">
                                                        <asp:ListItem Text="Select Source" Value="0" />
                                                        <asp:ListItem Text="Both" Value="Both" />
                                                        <asp:ListItem Text="Medical/Surgical History" Value="Medical/Surgical History" />
                                                        <asp:ListItem Text="Physical Examination" Value="Physical Examination" />
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="LabelText">
                                                    Confimed :
                                                </td>
                                                <td class="LabelText" style="text-align: left;">
                                                    <asp:DropDownList ID="ddlConfirmed" runat="server" Width="110px">
                                                        <asp:ListItem Text="Yes" Value="Y" />
                                                        <asp:ListItem Text="No" Value="N" />
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="LabelText" style="vertical-align: top;">
                                                    Comments :
                                                </td>
                                                <td colspan="3" style="text-align: left;">
                                                    <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Rows="5" Height="60px"
                                                        CssClass="TextBox" Width="541px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <hr />
                                    </td>
                                </tr>
                                <tr style="height: 8px;">
                                    <td colspan="2">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: center;">
                                        <asp:Button ID="btnUpdateRecord" runat="server" Text="Update" CssClass="ButtonText"
                                            Width="65px" Style="font-size: 12px !important;" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ButtonText" Width="65px"
                                            Style="font-size: 12px !important;" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnConditionAudit" runat="server" Style="display: none;" />
            <cc1:ModalPopupExtender ID="mdlConditionAudit" runat="server" PopupControlID="divConditionAudit"
                BackgroundCssClass="modalBackground" BehaviorID="mdlConditionAudit" CancelControlID="imgCondiAuditClosePopup"
                TargetControlID="btnConditionAudit">
            </cc1:ModalPopupExtender>
            <div id="divConditionAudit" runat="server" class="centerModalPopup" style="display: none;
                overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto">
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td class="LabelText" style="text-align: center !important; font-size: 12px !important;
                            width: 97%;">
                            <asp:Label ID="lblCondiAudit" runat="server" />
                        </td>
                        <td style="width: 3%">
                            <img id="imgCondiAuditClosePopup" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table cellpadding="2" cellspacing="2" width="100%" style="padding: 5px;">
                                <tr>
                                    <td>
                                        <asp:GridView ID="grdConditionAudit" runat="server" AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:BoundField DataField="vType" HeaderText="Type">
                                                    <ItemStyle HorizontalAlign="Left" Width="14%"></ItemStyle>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vSubType" HeaderText="Sub Type">
                                                    <ItemStyle HorizontalAlign="Left" Width="14%"></ItemStyle>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vDescription" HeaderText="Description">
                                                    <ItemStyle HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vSymptom" HeaderText="Symptom">
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Status">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnStatus" runat="server" Value='<% #databinder.eval(container.dataitem,"cStatusIndi") %>' />
                                                        <asp:HiddenField ID="hdnIdCode" runat="server" Value='<% #databinder.eval(container.dataitem,"vIdCode") %>' />
                                                        <asp:Label ID="lblStatus" runat="server" Style="font-family: Calibri, sans-serif !important;
                                                            font-size: 12px !important;" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Audit">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgCondAudit" runat="server" ImageUrl="~/CDMS/images/audit.png"
                                                            ToolTip="Audit Trail" Style="cursor: pointer;" />
                                                        <asp:Label ID="lblConditionAuditMark" runat="server" Text="*" CssClass="LabelText"
                                                            Style="color: Red !important; font-size: 16px;" Visible="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px;">
                        </td>
                    </tr>
                </table>
            </div>
            <asp:Button ID="btnConditionRowAudit" runat="server" Style="display: none;" />
            <cc1:ModalPopupExtender ID="mdlConditionRowAudit" runat="server" PopupControlID="divConditionRowAudit"
                BackgroundCssClass="modalBackground" BehaviorID="mdlConditionRowAudit" CancelControlID="imgCondiRowAuditClose"
                TargetControlID="btnConditionRowAudit">
            </cc1:ModalPopupExtender>
            <div id="divConditionRowAudit" runat="server" class="centerModalPopup" style="display: none;
                overflow: auto; width: 95%; height: auto; max-height: 80%; min-height: auto">
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td class="LabelText" style="text-align: center !important; font-size: 12px !important;
                            width: 97%;">
                            <asp:Label ID="lblCondiRowAudit" runat="server" />
                        </td>
                        <td style="width: 3%">
                            <img id="imgCondiRowAuditClose" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table cellpadding="2" cellspacing="2" width="100%" style="padding: 5px;">
                                <tr>
                                    <td>
                                        <asp:GridView ID="grdConditionRowAudit" runat="server" AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:BoundField DataField="dOnsetDate" HeaderText="Onset Date" DataFormatString="{0:dd-MMM-yyyy}"
                                                    HtmlEncode="false">
                                                    <ItemStyle HorizontalAlign="Left" Width="13%"></ItemStyle>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="dResolutionDate" HeaderText="Resolution Date" DataFormatString="{0:dd-MMM-yyyy}"
                                                    HtmlEncode="false">
                                                    <ItemStyle HorizontalAlign="Left" Width="17%"></ItemStyle>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vSource" HeaderText="Source">
                                                    <ItemStyle HorizontalAlign="Left" Width="15%"></ItemStyle>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="cConfirmed" HeaderText="Confirmed">
                                                    <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vComments" HeaderText="Comments">
                                                    <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vRemarks" HeaderText="Reason">
                                                    <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="UserName" HeaderText="Modify By">
                                                    <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="dModifyOffSet" HeaderText="Modify On">
                                                    <ItemStyle HorizontalAlign="Left" Width="15%"></ItemStyle>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px;">
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:Button ID="btnmdlSave" runat="server" Style="display: none;" />
        <cc1:ModalPopupExtender ID="mdlSaveAlert" runat="server" PopupControlID="divSaveAlert"
            BackgroundCssClass="modalBackground" BehaviorID="mdlSaveAlert" TargetControlID="btnmdlSave">
        </cc1:ModalPopupExtender>
        <div id="divSaveAlert" runat="server" class="centerModalPopup" style="display: none;
            overflow: auto; width: 35%; height: auto; max-height: 25%; min-height: auto;">
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td id="SaveAlertHeader" class="LabelText" style="text-align: left !important; font-size: 12px !important;
                        width: 97%;">
                        Save Warning
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td id="SaveAlertMessage" class="LabelText" style="text-align: center !important;">
                        Are you sure you want to add selected Medical Condition?
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btnSaveAdd" runat="server" Text="Add" CssClass="ButtonText" Width="57px"
                            Style="font-size: 12px !important;" ToolTip="Add Medical Condition" />
                        <asp:Button ID="btnSaveCancel" runat="server" Text="Cancel" CssClass="ButtonText"
                            Width="66px" Style="font-size: 12px !important;" ToolTip="Cancel Medical Condition" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btnDelete" runat="server" Style="display: none;" />
        <cc1:ModalPopupExtender ID="mdlDeleteMedical" runat="server" PopupControlID="divDelete"
            BackgroundCssClass="modalBackground" BehaviorID="mdlDeleteMedical" CancelControlID="btnNo"
            TargetControlID="btnDelete">
        </cc1:ModalPopupExtender>
        <div id="divDelete" runat="server" class="centerModalPopup" style="display: none;
            overflow: auto; width: 35%; height: auto; max-height: 30%; min-height: auto;">
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td class="LabelText" style="text-align: left !important; font-size: 12px !important;
                        width: 97%;">
                        Delete Confirmation
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td class="LabelText" style="text-align: center !important;">
                        Are you sure you want to delete this medical condition?
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btnYes" runat="server" Text="Yes" ToolTip="Confirm To Delete Condition"
                            CssClass="ButtonText" Width="57px" Style="font-size: 12px !important;" />
                        <asp:Button ID="btnNo" runat="server" Text="No" ToolTip="Cancel To Return Back" CssClass="ButtonText"
                            Width="57px" Style="font-size: 12px !important;" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btnRemarks" runat="server" Style="display: none;" />
        <cc1:ModalPopupExtender ID="mdlRemarksMedical" runat="server" PopupControlID="divRemarks"
            BackgroundCssClass="modalBackground" BehaviorID="mdlRemarksMedical" CancelControlID="btnRemarksCancel"
            TargetControlID="btnRemarks">
        </cc1:ModalPopupExtender>
        <div id="divRemarks" runat="server" class="centerModalPopup" style="display: none;
            overflow: auto; width: 32%; height: auto; max-height: 50%; min-height: auto;">
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td class="LabelText" style="text-align: left !important; font-size: 12px !important;
                        width: 97%;">
                        Remarks
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td class="LabelText" style="text-align: left !important;">
                        Enter Remarks:
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left !important;">
                        <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="5" Height="60px"
                            CssClass="TextBox" Width="300px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btnRemarksDelete" runat="server" Text="Delete" CssClass="ButtonText"
                            Width="60px" Style="font-size: 12px !important;" />
                        <asp:Button ID="btnRemarksUpdate" runat="server" Text="Update" CssClass="ButtonText"
                            Width="60px" Style="font-size: 12px !important; display: none;" />
                        <asp:Button ID="btnRemarksCancel" runat="server" Text="Cancel" CssClass="ButtonText"
                            Width="62px" Style="font-size: 12px !important;" />
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnMedicalFillGrid" runat="server" Style="display: none;" />
        </div>
        <div id="tabs-2" align="left">
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td colspan="2">
                        <fieldset class="FieldSetBox" style="padding-top: 7px; width: 98%;">
                            <table cellpadding="2" cellspacing="2" width="100%">
                                <tr>
                                    <td class="LabelText" style="text-align: left; width: 10%;">
                                        Criteria :
                                    </td>
                                    <td class="LabelText" style="text-align: left; font-size: 11px !important;">
                                        <asp:RadioButtonList ID="rblconco" runat="server" RepeatDirection="Horizontal" ToolTip="Inclusion Or Exclusion Criteria"
                                            AutoPostBack="true">
                                            <asp:ListItem Value="Inclusion" Text="Inclusion" />
                                            <asp:ListItem Value="Exclusion" Text="Exclusion" />
                                        </asp:RadioButtonList>
                                    </td>
                                    <td style="text-align: right;">
                                        <input type="button" id="BtnAddmoreConco" value="Add" title="Add Conco. Medication Criteria"
                                            class="btn btnadd" style="display: none;"
                                            onclick="return ValidationForConco();" />
                                        <asp:Button ID="btnHistoryMedication" runat="server" Text="History" CssClass="btn btnadd" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr style="height: 8px;">
                    <td colspan="2">
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <fieldset class="FieldSetBox" style="padding-top: 7px; width: 98%;">
                            <legend class="LegendText" style="color: Black">Conco. Medication</legend>
                            <table cellpadding="2" cellspacing="2" width="100%">
                                <tr>
                                    <td>
                                        <asp:HiddenField ID="hdnConcoMedicationNo" runat="server" />
                                        <asp:GridView ID="grdConcoMedi" runat="server" AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:BoundField DataField="vIdCode" HeaderText="Code">
                                                    <ItemStyle HorizontalAlign="Left" Width="16%" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vClass" HeaderText="Class">
                                                    <ItemStyle HorizontalAlign="Left" Width="17%" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vDescription" HeaderText="Description">
                                                    <ItemStyle HorizontalAlign="Left" Width="14%" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vDosage" HeaderText="Dosage">
                                                    <ItemStyle HorizontalAlign="Left" Width="17%" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="dStartDate" HeaderText="Start Date" DataFormatString="{0:dd-MMM-yyyy}"
                                                    HtmlEncode="false">
                                                    <ItemStyle HorizontalAlign="Left" Width="12%" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="dEndDate" HeaderText="End Date" DataFormatString="{0:dd-MMM-yyyy}"
                                                    HtmlEncode="false">
                                                    <ItemStyle HorizontalAlign="Left" Width="12%" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vCriteria" HeaderText="Criteria" HtmlEncode="false">
                                                    <ItemStyle HorizontalAlign="Left" Width="12%" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Action">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgEdit" runat="server" AlternateText='<% #databinder.eval(container.dataitem,"nProjectDtlCDMSConcoMedicationNo") %>'
                                                            ImageUrl="~/CDMS/images/Edit2.gif" ToolTip="Edit Medication" Style="cursor: pointer;"
                                                            onclick="fnEditConditionConcoMedi(this.alt);" />
                                                        <asp:Image ID="imgDelete" runat="server" AlternateText='<% #databinder.eval(container.dataitem,"nProjectDtlCDMSConcoMedicationNo") %>'
                                                            ImageUrl="~/CDMS/images/i_delete.gif" ToolTip="Delete Medication" Style="padding-left: 3px;
                                                            cursor: pointer;" onclick="fnDeleteConditionConcoMedi(this.alt);" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr style="height: 8px;">
                    <td colspan="2">
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <fieldset class="FieldSetBox" style="padding-top: 7px; width: 98%;">
                            <table cellpadding="2" cellspacing="2" width="100%">
                                <tr>
                                    <td class="LabelText" style="text-align: left; font-size: 12px !important;">
                                        <asp:Button ID="btnConcoPrevious" runat="server" Text="Previous" CssClass="btn btnpre" />
                                    </td>
                                    <td class="LabelText" style="text-align: right; font-size: 12px !important;">
                                        <asp:Button ID="btnConcoNext" runat="server" Text="Next" CssClass="btn btnnext" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr style="height: 8px;">
                    <td colspan="2">
                        <asp:Button ID="btnEditConco" runat="server" Style="display: none;" />
                        <cc1:ModalPopupExtender ID="mpEditConcoMedi" runat="server" PopupControlID="divEditConcoMedi"
                            BackgroundCssClass="modalBackground" BehaviorID="mpEditConcoMedi" CancelControlID="imgEditClosePopupConco"
                            TargetControlID="btnEditConco">
                        </cc1:ModalPopupExtender>
                        <div id="divEditConcoMedi" runat="server" class="centerModalPopup" style="display: none;
                            overflow: auto; width: 650px; height: 290px; max-height: 290px; min-height: 290px">
                            <table cellpadding="2" cellspacing="2" width="100%">
                                <tr>
                                    <td class="LabelText" style="text-align: center !important; font-size: 12px !important;
                                        width: 97%;">
                                        Edit Conco. Medication
                                    </td>
                                    <td style="width: 3%">
                                        <img id="imgEditClosePopupConco" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <table cellpadding="2" cellspacing="2" width="100%" style="padding: 5px;">
                                            <tr>
                                                <td class="LabelText">
                                                    Code :
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtCodeConco" runat="server" CssClass="TextBox" Width="210px" />
                                                </td>
                                                <td class="LabelText">
                                                    Class :
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtClassConco" runat="server" CssClass="TextBox" Width="219px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="LabelText">
                                                    Start Date :
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtStartDateConco" runat="server" CssClass="TextBox"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="calStartDate" runat="server" TargetControlID="txtStartDateConco"
                                                        Format="dd-MMM-yyyy">
                                                    </cc1:CalendarExtender>
                                                </td>
                                                <td class="LabelText">
                                                    End Date :
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtEndDateConco" runat="server" CssClass="TextBox"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="calEndDate" runat="server" TargetControlID="txtEndDateConco"
                                                        Format="dd-MMM-yyyy">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="LabelText">
                                                    Dosage :
                                                </td>
                                                <td class="LabelText" style="text-align: left;">
                                                    <asp:TextBox ID="txtDosageConco" runat="server" CssClass="TextBox" Width="210px" />
                                                </td>
                                                <td class="LabelText">
                                                    Confimed :
                                                </td>
                                                <td class="LabelText" style="text-align: left;">
                                                    <asp:DropDownList ID="ddlConfirmedConco" runat="server" Width="110px">
                                                        <asp:ListItem Text="Yes" Value="Y" />
                                                        <asp:ListItem Text="No" Value="N" />
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="LabelText" style="vertical-align: top;">
                                                    Comments :
                                                </td>
                                                <td colspan="3" style="text-align: left;">
                                                    <asp:TextBox ID="txtCommentsConco" runat="server" TextMode="MultiLine" Rows="5" Height="60px"
                                                        CssClass="TextBox" Width="541px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <hr />
                                    </td>
                                </tr>
                                <tr style="height: 8px;">
                                    <td colspan="2">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: center;">
                                        <asp:Button ID="btnUpdateRecordConco" runat="server" Text="Update" CssClass="ButtonText"
                                            Width="65px" Style="font-size: 12px !important;" />
                                        <asp:Button ID="btnCancelConco" runat="server" Text="Cancel" CssClass="ButtonText"
                                            Width="65px" Style="font-size: 12px !important;" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <asp:Button ID="btnConcoMedi" runat="server" Style="display: none;" />
                        <cc1:ModalPopupExtender ID="mpConcoMedi" runat="server" PopupControlID="divConcoMedi"
                            BackgroundCssClass="modalBackground" BehaviorID="mpConcoMedi" CancelControlID="imgClosePopup"
                            TargetControlID="btnConcoMedi">
                        </cc1:ModalPopupExtender>
                        <div id="divConcoMedi" runat="server" class="centerModalPopup" style="display: none;
                            overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto">
                            <table cellpadding="2" cellspacing="2" width="100%">
                                <tr>
                                    <td class="LabelText" style="text-align: center !important; font-size: 12px !important;
                                        width: 97%;">
                                        <asp:Label ID="hdrConcomedic" runat="server" Visible="true"></asp:Label>
                                        <%--<asp:Label ID="hdrConcomedi" runat="server"></asp:Label>--%>
                                    </td>
                                    <td style="width: 3%">
                                        <img id="imgClosePopup" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <table cellpadding="2" cellspacing="2" width="100%" style="padding: 5px;">
                                            <tr>
                                                <td>
                                                    <asp:HiddenField ID="hdnSelectedConco" runat="server" />
                                                    <asp:GridView ID="grdAddConcoMedi" runat="server" AutoGenerateColumns="false">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Select">
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkCodeList" runat="server" onclick="fnCheckBokSelectionConco(this);" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="CodeClassOfMedication" HeaderText="Code">
                                                                <ItemStyle HorizontalAlign="Center" Width="12%" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Class" HeaderText="Class">
                                                                <ItemStyle HorizontalAlign="Left" Width="14%" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Description" HeaderText="Description">
                                                                <ItemStyle HorizontalAlign="Left" Width="14%" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: center;">
                                        <asp:Button ID="btnAddConcoMedi" runat="server" Text="Add" CssClass="ButtonText"
                                            Width="57px" Style="font-size: 12px !important;" ToolTip="Add Conco Medication Criteria For This Project" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnMedicationAudit" runat="server" Style="display: none;" />
            <cc1:ModalPopupExtender ID="mdlMedicationAudit" runat="server" PopupControlID="divMedicationAudit"
                BackgroundCssClass="modalBackground" BehaviorID="mdlMedicationAudit" CancelControlID="imgMediAuditClosePopup"
                TargetControlID="btnMedicationAudit">
            </cc1:ModalPopupExtender>
            <div id="divMedicationAudit" runat="server" class="centerModalPopup" style="display: none;
                overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto">
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td class="LabelText" style="text-align: center !important; font-size: 12px !important;
                            width: 97%;">
                            <asp:Label ID="lblMediAudit" runat="server" />
                        </td>
                        <td style="width: 3%">
                            <img id="imgMediAuditClosePopup" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table cellpadding="2" cellspacing="2" width="100%" style="padding: 5px;">
                                <tr>
                                    <td>
                                        <asp:GridView ID="grdMedicationAudit" runat="server" AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:BoundField DataField="vIdCode" HeaderText="Code">
                                                    <ItemStyle HorizontalAlign="Left" Width="14%"></ItemStyle>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vClass" HeaderText="Class">
                                                    <ItemStyle HorizontalAlign="Left" Width="14%"></ItemStyle>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vDescription" HeaderText="Description">
                                                    <ItemStyle HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Status">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnStatus" runat="server" Value='<% #databinder.eval(container.dataitem,"cStatusIndi") %>' />
                                                        <asp:HiddenField ID="hdnIdCode" runat="server" Value='<% #databinder.eval(container.dataitem,"vIdCode") %>' />
                                                        <asp:Label ID="lblStatus" runat="server" Style="font-family: Calibri, sans-serif !important;
                                                            font-size: 12px !important;" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Audit">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgMediAudit" runat="server" ImageUrl="~/CDMS/images/audit.png"
                                                            ToolTip="Audit Trail" Style="cursor: pointer;" />
                                                        <asp:Label ID="lblMedicationAuditMark" runat="server" Text="*" CssClass="LabelText"
                                                            Style="color: Red !important; font-size: 16px;" Visible="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px;">
                        </td>
                    </tr>
                </table>
            </div>
            <asp:Button ID="btnMedicationRowAudit" runat="server" Style="display: none;" />
            <cc1:ModalPopupExtender ID="mdlMedicationRowAudit" runat="server" PopupControlID="divMedicationRowAudit"
                BackgroundCssClass="modalBackground" BehaviorID="mdlMedicationRowAudit" CancelControlID="imgMediRowAuditClose"
                TargetControlID="btnMedicationRowAudit">
            </cc1:ModalPopupExtender>
            <div id="divMedicationRowAudit" runat="server" class="centerModalPopup" style="display: none;
                overflow: auto; width: 95%; height: auto; max-height: 80%; min-height: auto">
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td class="LabelText" style="text-align: center !important; font-size: 12px !important;
                            width: 97%;">
                            <asp:Label ID="lblMediRowAudit" runat="server" />
                        </td>
                        <td style="width: 3%">
                            <img id="imgMediRowAuditClose" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table cellpadding="2" cellspacing="2" width="100%" style="padding: 5px;">
                                <tr>
                                    <td>
                                        <asp:GridView ID="grdMedicationRowAudit" runat="server" AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:BoundField DataField="dStartDate" HeaderText="Start Date" DataFormatString="{0:dd-MMM-yyyy}"
                                                    HtmlEncode="false">
                                                    <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="dEndDate" HeaderText="End Date" DataFormatString="{0:dd-MMM-yyyy}"
                                                    HtmlEncode="false">
                                                    <ItemStyle HorizontalAlign="Left" Width="11%"></ItemStyle>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vDosage" HeaderText="Dosage">
                                                    <ItemStyle HorizontalAlign="Left" Width="15%"></ItemStyle>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="cConfirmed" HeaderText="Confirmed">
                                                    <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vComments" HeaderText="Comments">
                                                    <ItemStyle HorizontalAlign="Left" Width="15%"></ItemStyle>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vRemarks" HeaderText="Reason">
                                                    <ItemStyle HorizontalAlign="Left" Width="15%"></ItemStyle>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="UserName" HeaderText="Modify By">
                                                    <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="dModifyOffSet" HeaderText="Modify On">
                                                    <ItemStyle HorizontalAlign="Left" Width="15%"></ItemStyle>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px;">
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:Button ID="btnmdlSaveConco" runat="server" Style="display: none;" />
        <cc1:ModalPopupExtender ID="mdlSaveAlertConco" runat="server" PopupControlID="divSaveAlertConco"
            BackgroundCssClass="modalBackground" BehaviorID="mdlSaveAlertConco" TargetControlID="btnmdlSaveConco">
        </cc1:ModalPopupExtender>
        <div id="divSaveAlertConco" runat="server" class="centerModalPopup" style="display: none;
            overflow: auto; width: 35%; height: auto; max-height: 25%; min-height: auto;">
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td id="Td1" class="LabelText" style="text-align: left !important; font-size: 12px !important;
                        width: 97%;">
                        Save Warning
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td id="Td2" class="LabelText" style="text-align: center !important;">
                        Are you sure you want to add selected Conco. Medication?
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btnSaveAddConco" runat="server" Text="Add" CssClass="ButtonText"
                            Width="57px" Style="font-size: 12px !important;" ToolTip="Add Conco. Medication" />
                        <asp:Button ID="btnSaveCancelConco" runat="server" Text="Cancel" CssClass="ButtonText"
                            Width="66px" Style="font-size: 12px !important;" ToolTip="Cancel Conco. Medication" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btnDeleteConco" runat="server" Style="display: none;" />
        <cc1:ModalPopupExtender ID="mdlDeleteConco" runat="server" PopupControlID="divDeleteConco"
            BackgroundCssClass="modalBackground" BehaviorID="mdlDeleteConco" CancelControlID="btnNo"
            TargetControlID="btnDeleteConco">
        </cc1:ModalPopupExtender>
        <div id="divDeleteConco" runat="server" class="centerModalPopup" style="display: none;
            overflow: auto; width: 35%; height: auto; max-height: 25%; min-height: auto;">
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td id="AlertHeaderConco" class="LabelText" style="text-align: left !important; font-size: 12px !important;
                        width: 97%;">
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td id="AlertMessageConco" class="LabelText" style="text-align: center !important;">
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btnYesConco" runat="server" Text="Yes" CssClass="ButtonText" Width="57px"
                            Style="font-size: 12px !important; display: inline;" />
                        <asp:Button ID="btnNoConco" runat="server" Text="No" CssClass="ButtonText" Width="57px"
                            Style="font-size: 12px !important; display: inline;" />
                        <asp:Button ID="btnOkConco" runat="server" Text="Ok" CssClass="ButtonText" Width="57px"
                            Style="font-size: 12px !important; display: none;" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btnWarning" runat="server" Style="display: none;" />
        <cc1:ModalPopupExtender ID="mdlWarning" runat="server" PopupControlID="divWarning"
            BackgroundCssClass="modalBackground" BehaviorID="mdlWarning" TargetControlID="btnWarning">
        </cc1:ModalPopupExtender>
        <div id="divWarning" runat="server" class="centerModalPopup" style="display: none;
            overflow: auto; width: 35%; height: auto; max-height: 25%; min-height: auto;">
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td id="WarningHeader" class="LabelText" style="text-align: left !important; font-size: 12px !important;
                        width: 97%;">
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td id="WarningMessage" class="LabelText" style="text-align: center !important;">
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btnWarningOk" runat="server" Text="Ok" CssClass="ButtonText" Width="57px"
                            Style="font-size: 12px !important;" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btnRemarksconco" runat="server" Style="display: none;" />
        <cc1:ModalPopupExtender ID="mdlRemarksConco" runat="server" PopupControlID="divRemarksConco"
            BackgroundCssClass="modalBackground" BehaviorID="mdlRemarksConco" CancelControlID="btnRemarksCancel"
            TargetControlID="btnRemarksconco">
        </cc1:ModalPopupExtender>
        <div id="divRemarksConco" runat="server" class="centerModalPopup" style="display: none;
            overflow: auto; width: 32%; height: auto; max-height: 45%; min-height: auto;">
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td class="LabelText" style="text-align: left !important; font-size: 12px !important;
                        width: 97%;">
                        Remarks
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td class="LabelText" style="text-align: left !important;">
                        Enter Remarks:
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left !important;">
                        <asp:TextBox ID="txtRemarksConco" runat="server" TextMode="MultiLine" Rows="5" Height="60px"
                            CssClass="TextBox" Width="300px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btnRemarksDeleteConco" runat="server" Text="Delete" CssClass="ButtonText"
                            Width="60px" Style="font-size: 12px !important; display: inline;" />
                        <asp:Button ID="btnRemarksUpdateConco" runat="server" Text="Update" CssClass="ButtonText"
                            Width="60px" Style="font-size: 12px !important; display: none;" />
                        <asp:Button ID="btnRemarksCancelConco" runat="server" Text="Cancel" CssClass="ButtonText"
                            Width="62px" Style="font-size: 12px !important;" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btnConcoFillGrid" runat="server" Style="display: none;" />
        <asp:HiddenField ID="hdnActivetab" runat="server" />
    </div>

    <script type="text/javascript">
    
    function pageLoad() {
                
                var SWorkFlowStageId = '<%= Session(S_WorkFlowStageId).ToString() %>'; 
                if (SWorkFlowStageId == '0')
                   {
                     document.getElementById('BtnAddmoreConco').style.display = '';
                     document.getElementById('btnAddMoreMedi').style.display = '';
                   }
                
                $('#<%= grdAddMedicalCond.ClientID %>').prepend($('<thead>').append($('#<%= grdAddMedicalCond.ClientID %> tr:first'))).dataTable({
                "bStateSave": false,
                "bPaginate": true,
                "sPaginationType": "full_numbers",
                "bSort": true,
                "bDestory": true,
                "bRetrieve": true
                });
                 $('#<%= grdAddMedicalCond.ClientID %> tr:first').css('background-color','#3A87AD'); 
                 
                $('#<%= grdAddConcoMedi.ClientID %>').prepend($('<thead>').append($('#<%= grdAddConcoMedi.ClientID %> tr:first'))).dataTable({
                "bStateSave": false,
                "bPaginate": true,
                "sPaginationType": "full_numbers",
                "bSort": true,
                "bDestory": true,
                "bRetrieve": true
                });
                   $('#<%= grdAddConcoMedi.ClientID %> tr:first').css('background-color','#3A87AD'); 
                   
                 $('#<%= grdMedicalCondition.ClientID %>').prepend($('<thead>').append($('#<%= grdMedicalCondition.ClientID %> tr:first'))).dataTable({
                "bStateSave": false,
                "bPaginate": true,
                "sPaginationType": "full_numbers",
                "bSort": true,
                "bDestory": true,
                "bRetrieve": true
                });
                     $('#<%= grdMedicalCondition.ClientID %> tr:first').css('background-color','#3A87AD'); 
                   
                 $('#<%= grdConcoMedi.ClientID %>').prepend($('<thead>').append($('#<%= grdConcoMedi.ClientID %> tr:first'))).dataTable({
                "bStateSave": false,
                "bPaginate": true,
                "sPaginationType": "full_numbers",
                "bSort": true,
                "bDestory": true,
                "bRetrieve": true
                });
                      $('#<%= grdConcoMedi.ClientID %> tr:first').css('background-color','#3A87AD'); 
                    
                 $('#<%= grdConditionAudit.ClientID %>').prepend($('<thead>').append($('#<%= grdConditionAudit.ClientID %> tr:first'))).dataTable({
                    "bPaginate": true,
                    "sPaginationType": "full_numbers",
                    "bSort": true,
                    "bDestory": true,
                    "bRetrieve": true,
                     "aoColumns": [
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    { "bSortable": false }
                                 ]
                });
                
                     $('#<%= grdConditionAudit.ClientID %> tr:first').css('background-color','#3A87AD'); 
                
                $('#<%= grdConditionRowAudit.ClientID %>').prepend($('<thead>').append($('#<%= grdConditionRowAudit.ClientID %> tr:first'))).dataTable({
                    "bPaginate": true,
                    "sPaginationType": "full_numbers",
                    "bSort": true,
                    "bDestory": true,
                    "bRetrieve": true
                });
                     $('#<%= grdConditionRowAudit.ClientID %> tr:first').css('background-color','#3A87AD');   
                
                $('#<%= grdMedicationAudit.ClientID %>').prepend($('<thead>').append($('#<%= grdMedicationAudit.ClientID %> tr:first'))).dataTable({
                    "bPaginate": true,
                    "sPaginationType": "full_numbers",
                    "bSort": true,
                    "bDestory": true,
                    "bRetrieve": true,
                     "aoColumns": [
                                    null,
                                    null,
                                    null,
                                    null,
                                    { "bSortable": false }
                                 ]
                });
                     $('#<%= grdMedicationAudit.ClientID %> tr:first').css('background-color','#3A87AD'); 
                     
                $('#<%= grdMedicationRowAudit.ClientID %>').prepend($('<thead>').append($('#<%= grdMedicationRowAudit.ClientID %> tr:first'))).dataTable({
                    "bPaginate": true,
                    "sPaginationType": "full_numbers",
                    "bSort": true,
                    "bDestory": true,
                    "bRetrieve": true
                });
                     $('#<%= grdMedicationRowAudit.ClientID %> tr:first').css('background-color','#3A87AD'); 
        }
        
       
        
        
    $(function() 
     {
                $('.InnerTable').parent().attr('align','left')
                $('.InnerTable').parent().css('padding-left','4px');
                $( "#tabs" ).tabs().addClass( "ui-tabs-vertical ui-helper-clearfix" );
                $( "#tabs li" ).removeClass( "ui-corner-top" ).addClass( "ui-corner-left" );
                fnGetQueryString("tab") == "Medication" ?  $( "#tabs" ).tabs({active:2}) :  $( "#tabs" ).tabs({active:1});
                $('#ctl00_lblHeading').text('Study Information - Medical Condition');
                if(fnGetQueryString("tab") == "Medication" )
                {
                    $('#ctl00_lblHeading').text('Study Information - Conco. Medication');
                }
                $('#<%= btnAddMoreMediCond.ClientID %>').unbind('click').click(function(){ $find('mdlSaveAlert').show(); return false; });
                $('#<%= btnAddConcoMedi.ClientID %>').unbind('click').click(function(){ $find('mdlSaveAlertConco').show(); return false; });
                $('#<%= btnWarningOk.ClientID %>').unbind('click').click(function(){$find('mdlWarning').hide();  return false;});
                
                 $("[id$='_txtOnsetDate']").blur(function(){
                                                        if ($(this).val() != "" && !CheckDateLessThenToday($(this).val())) {
                                                            $(this).val('');
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Onset date should be less than current date.');
                                                            return false;
                                                        }
                                                        });
                                                        
                  
                  $("[id$='_txtResolutionDate']").blur(function(){
                                                        if ($(this).val() != "" && !CheckDateLessThenToday($(this).val())) {
                                                            $(this).val('');
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Resolution date should be less than current date.');
                                                            return false;
                                                        }
                                                        });
                                                        
                 $("[id$='_txtStartDateConco']").blur(function(){
                                                        if ($(this).val() != "" && !CheckDateLessThenToday($(this).val())) {
                                                            $(this).val('');
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Start date should be less than current date.');
                                                            return false;
                                                        }
                                                        });
                                                        
                  
                  $("[id$='_txtEndDateConco']").blur(function(){
                                                        if ($(this).val() != "" && !CheckDateLessThenToday($(this).val())) {
                                                            $(this).val('');
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('End date should be less than current date.');
                                                            return false;
                                                        }
                                                        });                                                    
                                                            
    });
    
    function fnCheckBokSelectionMedical(ctrl)
            {
               if($(ctrl).is(':checked'))
                    {
                       if($('#<%= hdnSelectedCondition.ClientId %>').val() == "")
                       {
                          $('#<%= hdnSelectedCondition.ClientId %>').val($($(ctrl).closest('tr').children('td')[1]).text() + ",");
                       }
                       else
                       {
                           var Data = $('#<%= hdnSelectedCondition.ClientId %>').val().split(',');
                           var found = jQuery.inArray($($(ctrl).closest('tr').children('td')[1]).text(), Data);
                           if (found >= 0) {
                                Data.splice(found, 1);
                            } else {
                                Data.push($($(ctrl).closest('tr').children('td')[1]).text());
                            }
                           var blkstr = $.map(Data, function(val,index) {
                                     return val;
                                }).join(",");
                           $('#<%= hdnSelectedCondition.ClientId %>').val(blkstr.replace(',,',','));
                       }
                 }
                 else
                 {
                     var Data = $('#<%= hdnSelectedCondition.ClientId %>').val().split(',');
                     var found = jQuery.inArray($($(ctrl).closest('tr').children('td')[1]).text(), Data);
                     if (found >= 0) {
                         Data.splice(found, 1);
                     } 
                     var blkstr = $.map(Data, function(val,index) {
                                     return val;
                                }).join(",");
                     $('#<%= hdnSelectedCondition.ClientId %>').val(blkstr.replace(',,',','));
                 }
            }
    
    function fnCheckBokSelectionConco(ctrl)
            {
               if($(ctrl).is(':checked'))
                    {
                       if($('#<%= hdnSelectedConco.ClientId %>').val() == "")
                       {
                          $('#<%= hdnSelectedConco.ClientId %>').val($($(ctrl).closest('tr').children('td')[1]).text() + ",");
                       }
                       else
                       {
                           var Data = $('#<%= hdnSelectedConco.ClientId %>').val().split(',');
                           var found = jQuery.inArray($($(ctrl).closest('tr').children('td')[1]).text(), Data);
                           if (found >= 0) {
                                Data.splice(found, 1);
                            } else {
                                Data.push($($(ctrl).closest('tr').children('td')[1]).text());
                            }
                           var blkstr = $.map(Data, function(val,index) {
                                     return val;
                                }).join(",");
                           $('#<%= hdnSelectedConco.ClientId %>').val(blkstr.replace(',,',','));
                       }
                 }
                 else
                 {
                     var Data = $('#<%= hdnSelectedConco.ClientId %>').val().split(',');
                     var found = jQuery.inArray($($(ctrl).closest('tr').children('td')[1]).text(), Data);
                     if (found >= 0) {
                         Data.splice(found, 1);
                     } 
                     var blkstr = $.map(Data, function(val,index) {
                                     return val;
                                }).join(",");
                     $('#<%= hdnSelectedConco.ClientId %>').val(blkstr.replace(',,',','));
                 }
            }
   
    function ClientPopulated(sender, e) {

            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
       }

     function OnSelected(sender, e) {

         ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
         $get('<%= HProjectId.clientid %>'),document.getElementById('<%= btnSetProject.ClientId %>'));
       }
    function ValidationForMedi()
    {
      if($('#ctl00_CPHLAMBDA_HProjectId').val()== 0)
       {
        msgalert("Please Select Project");
        $('#txtproject').val() ==" ";
        $('#txtproject').focus();
        return false ;  
       }
     else if ($('#ctl00_CPHLAMBDA_rblMedi input[type=radio]:checked').val() == undefined )
      {
        msgalert('Please Select Criteria To Add Medical Conditions');
        return false ;
      }
      document.getElementById('ctl00_CPHLAMBDA_hdnrblvalue').value = $('#ctl00_CPHLAMBDA_rblMedi input[type=radio]:checked ').next().text();
      $('#ctl00_CPHLAMBDA_Hdrmedication').html("Add New Medical Condition for"+ " " + $('#ctl00_CPHLAMBDA_rblMedi input[type=radio]:checked ').next().text()+ " " + "Criteria");
      $('#ctl00_CPHLAMBDA_grdAddMedicalCond input[type=checkbox]').attr('checked',false);
      $find('mpMedicalCond').show();
        
        return false ;
    }
    function ValidationForConco()
    {
       if($('#ctl00_CPHLAMBDA_HProjectId').val()== 0)
       {
        msgalert("Please Select Project");
        $('#txtproject').val() ==" ";
        $('#txtproject').focus();
        return false ;  
       }
       else if ($('#ctl00_CPHLAMBDA_rblconco input[type=radio]:checked').val() == undefined )
      {
        msgalert('Please Select Criteria To Add Conco. Medications');
        return false ;
      }
       document.getElementById('ctl00_CPHLAMBDA_hdnrblvalue').value = $('#ctl00_CPHLAMBDA_rblconco input[type=radio]:checked ').next().text();
       $('#ctl00_CPHLAMBDA_hdrConcomedic').html("Add New Conco. Medication for"+ " " + $('#ctl00_CPHLAMBDA_rblconco input[type=radio]:checked ').next().text()+ " " + "Criteria");
       $('#ctl00_CPHLAMBDA_grdAddConcoMedi input[type=checkbox]').attr('checked',false);
       $find('mpConcoMedi').show();
       
       return false ;
    } 
    
   function  OpenMedicalTab()
   {
      $( "#tabs" ).tabs({active:1});
   }
   function  OpenConcoTab()
   {
      $( "#tabs" ).tabs({active:2});
   }
    
   function fnRedirect()
   {
         window.location.href = "frmCDMSStudyInformation.aspx?Mode=1&WorkspaceId=" + $('#<%= HProjectId.ClientID %>').val();
   }
   
   function fnGetQueryString(key, default_)
   {
        if (default_ == null) default_ = "";
        key = key.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regex = new RegExp("[\\?&]" + key + "=([^&#]*)");
        var qs = regex.exec(window.location.href);
        if (qs == null)
              return default_;
        else
              return qs[1];
   }
   
   function fnEditConditionConcoMedi(ConcoMedicationNo)
            {
                $("[id$='_txtRemarksConco']").val('');
                var content = {};
                content.vWorkspaceID = $('#ctl00_CPHLAMBDA_HProjectId').val();
                content.ConcoMedicationNo = ConcoMedicationNo;
                $.ajax({
                           type: "POST",
                           url: "frmCDMSProjectMedicalCondition.aspx/EditConcoMedication",
                           data: JSON.stringify(content),          
                           contentType: "application/json; charset=utf-8",
                           dataType: "json",
                           success: function(data) {
                                        if(data.d != undefined || data.d != null)
                                        {
                                            var Data = JSON.parse(data.d);
                                            var cMONTHNAMES = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug','Sep', 'Oct', 'Nov', 'Dec']; 
                                            var txtStartDate = Data.PROJECTDTLCDMSCONCOMEDICATION[0].dStartDate;
                                            var txtEndDate = Data.PROJECTDTLCDMSCONCOMEDICATION[0].dEndDate;
//                                            if(txtStartDate != null)
//                                            {
//                                                var StartDateArry;
//                                                StartDateArry = txtStartDate.replace('T00:00:00','').split("-");
//                                                txtStartDate  = StartDateArry[2] + "-" +  cMONTHNAMES[StartDateArry[1] - 1] + "-" + StartDateArry[0];
//                                            }
//                                            if(txtEndDate != null)
//                                            {
//                                                var EndDateArry;
//                                                EndDateArry = txtEndDate.replace('T00:00:00','').split("-");
//                                                txtEndDate  = EndDateArry[2] + "-" +  cMONTHNAMES[EndDateArry[1] - 1] + "-" + EndDateArry[0];
//                                            }

                                            $('#<%= hdnConcoMedicationNo.ClientID %>').val(Data.PROJECTDTLCDMSCONCOMEDICATION[0].nProjectDtlCDMSConcoMedicationNo);
                                            $('#<%= txtCodeConco.ClientID %>').val(Data.PROJECTDTLCDMSCONCOMEDICATION[0].vIdCode);
                                            $('#<%= txtClassConco.ClientID %>').val(Data.PROJECTDTLCDMSCONCOMEDICATION[0].vClass);
                                            $('#<%= txtStartDateConco.ClientID %>').val(txtStartDate);
                                            $('#<%= txtEndDateConco.ClientID %>').val(txtEndDate);
                                            $('#<%= txtDosageConco.ClientID %>').val(Data.PROJECTDTLCDMSCONCOMEDICATION[0].vDosage);
                                            $('#<%= ddlConfirmedConco.ClientID %>').val(Data.PROJECTDTLCDMSCONCOMEDICATION[0].cConfirmed);
                                            $('#<%= txtCommentsConco.ClientID %>').val(Data.PROJECTDTLCDMSCONCOMEDICATION[0].vComments);
                                            $find('mpEditConcoMedi').show();
                                        }
                                    },
                           failure: function(error) {
                                       msgalert(error);
                                    }
                });
                
                  $('#<%= btnUpdateRecordConco.ClientID %>').click(function(){ 
                                                        $('#<%= btnRemarksDeleteConco.ClientID %>').css('display','none');
                                                        $('#<%= btnRemarksUpdateConco.ClientID %>').css('display','inline');                                       
                                                        $find('mdlRemarksConco').show();
                                                        return false;
                                                  });
                
                $('#<%= btnCancelConco.ClientID %>').click(function(){ 
                                                        //event.preventDefault(); 
                                                        $find('mpEditConcoMedi').hide();
                                                        return false;
                                                  });
            }
            
            
            
    function fnDeleteConditionConcoMedi(ConcoMedicationNo)
    {
        $("[id$='_txtRemarksConco']").val('');
        $('#AlertHeaderConco').text('Delete Confirmation');
        $('#AlertMessageConco').text('Are you sure you want to delete this Conco. Medication ?');
        $('#<%= btnYesConco.ClientID %>').css('display','inline');
        $('#<%= btnNoConco.ClientID %>').css('display','inline');
        $('#<%= btnOkConco.ClientID %>').css('display','none');
        $find('mdlDeleteConco').show();
        $('#<%= btnYesConco.ClientID %>').click(function(){ 
                                               //event.preventDefault();
                                                $('#AlertHeaderConco').text('');
                                                $('#AlertMessageConco').text('');
                                                $('#<%= btnYesConco.ClientID %>').css('display','inline');
                                                $('#<%= btnNoConco.ClientID %>').css('display','inline');
                                                $('#<%= btnOkConco.ClientID %>').css('display','none');
                                                $find('mdlDeleteConco').hide();
                                                $('#<%= btnRemarksDeleteConco.ClientID %>').css('display','inline');
                                                $('#<%= btnRemarksUpdateConco.ClientID %>').css('display','none')
                                                $find('mdlRemarksConco').show();
                                                return false;
                                          });
                                          
        $('#<%= btnRemarksDeleteConco.ClientID %>').click(function(){ 
                                                //event.preventDefault();
                                                var content = {};
                                                content.vWorkspaceID = $('#ctl00_CPHLAMBDA_HProjectId').val();
                                                content.ConcoMedicationNo = ConcoMedicationNo;
                                                content.Remarks =  $("[id$='_txtRemarksConco']").val().trim();
                                                 if($("[id$='_txtRemarksConco']").val().trim() == "")
                                                {
                                                        $find('mdlDeleteConco').show();
                                                        $find('mdlRemarksConco').hide();
                                                        $('#AlertHeaderConco').text('Remarks Warning');
                                                        $('#AlertMessageConco').text('Please enter remarks.');
                                                        $('#<%= btnYesConco.ClientID %>').css('display','none');
                                                        $('#<%= btnNoConco.ClientID %>').css('display','none');
                                                        $('#<%= btnOkConco.ClientID %>').css('display','inline');
                                                }
                                                else
                                                {
                                                        $.ajax({
                                                                  type: "POST",
                                                                  url: "frmCDMSProjectMedicalCondition.aspx/DeleteConcoMedication",
                                                                  data: JSON.stringify(content),          
                                                                  contentType: "application/json; charset=utf-8",
                                                                  dataType: "json",
                                                                  success: function(data) {
                                                                        if(data.d == "Success")
                                                                        {
                                                                            $find('mdlRemarksConco').hide();
                                                                            $('#<%= btnConcoFillGrid.ClientID %>').click();
                                                                        }
                                                                        else
                                                                        {
                                                                            msgalert(data.d);
                                                                        }
                                                                  },
                                                                   failure: function(error) {
                                                                            msgalert(error);
                                                                        }
                                                              });
                                                }
                                                return false;
                                          });                                                 
        
                                         
    }  
    
    function fnDeleteMedicalCondition(MedicalConditionNo)
            {
                $find('mdlDeleteMedical').show();
                $('#<%= btnYes.ClientID %>').click(function(){ 
                                                        //event.preventDefault();
                                                        $find('mdlDeleteMedical').hide();
                                                        $find('mdlRemarksMedical').show();
                                                        return false;
                                                  });
                                                  
                $('#<%= btnRemarksDelete.ClientID %>').click(function(){ 
                                                        //event.preventDefault();
                                                        var content = {};
                                                        content.vWokspaceId = $('#ctl00_CPHLAMBDA_HProjectId').val();
                                                        content.MedicalConditionNo = MedicalConditionNo;
                                                        content.Remarks =  $("[id$='_txtRemarks']").val();
                                                        $.ajax({
                                                                  type: "POST",
                                                                  url: "frmCDMSProjectMedicalCondition.aspx/DeleteMedicalConditionMedical",
                                                                  data: JSON.stringify(content),          
                                                                  contentType: "application/json; charset=utf-8",
                                                                  dataType: "json",
                                                                  success: function(data) {
                                                                        if(data.d == "Success")
                                                                        {
                                                                            $find('mdlRemarksMedical').hide();
                                                                            $('#<%= btnMedicalFillGrid.ClientID %>').click();
                                                                        }
                                                                        else
                                                                        {
                                                                            msgalert(data.d);
                                                                        }
                                                                  },
                                                                   failure: function(error) {
                                                                            msgalert(error);
                                                                        }
                                                              });
                                                       return false;

                                                  });                                                 
                
                                                 
            }
            
            function fnEditMedicalCondition(MedicalConditionNo)
            {
                var content = {};
                content.vWokspaceId = $('#ctl00_CPHLAMBDA_HProjectId').val();
                content.MedicalConditionNo = MedicalConditionNo;
                $.ajax({
                           type: "POST",
                           url: "frmCDMSProjectMedicalCondition.aspx/EditMedicalConditionMedical",
                           data: JSON.stringify(content),          
                           contentType: "application/json; charset=utf-8",
                           dataType: "json",
                           success: function(data) {
                                        if(data.d != undefined || data.d != null)
                                        {
                                            var Data = JSON.parse(data.d);
                                            var cMONTHNAMES = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug','Sep', 'Oct', 'Nov', 'Dec']; 
                                            var txtOnsetDate = Data.PROJECTDTLCDMSMEDICALCONDITION[0].dOnsetDate;
                                            var txtResolutionDate = Data.PROJECTDTLCDMSMEDICALCONDITION[0].dResolutionDate;
//                                            if(txtOnsetDate != null)
//                                            {
//                                                var OnsetDateArry;
//                                                OnsetDateArry = txtOnsetDate.replace('T00:00:00','').split("-");
//                                                txtOnsetDate  = OnsetDateArry[2] + "-" +  cMONTHNAMES[OnsetDateArry[1] - 1] + "-" + OnsetDateArry[0];
//                                            }
//                                            if(txtResolutionDate != null)
//                                            {
//                                                var ResolutionDateArry;
//                                                ResolutionDateArry = txtResolutionDate.replace('T00:00:00','').split("-");
//                                                txtResolutionDate  = ResolutionDateArry[2] + "-" +  cMONTHNAMES[ResolutionDateArry[1] - 1] + "-" + ResolutionDateArry[0];
//                                            }
                                            
                                            $('#<%= hdnMedicalConditionNo.ClientID %>').val(Data.PROJECTDTLCDMSMEDICALCONDITION[0].nProjectDtlCSMSMedicalConditionNo);
                                            $('#<%= txtType.ClientID %>').val(Data.PROJECTDTLCDMSMEDICALCONDITION[0].vType);
                                            $('#<%= txtSubType.ClientID %>').val(Data.PROJECTDTLCDMSMEDICALCONDITION[0].vSubType);
                                            $('#<%= txtDescrption.ClientID %>').val(Data.PROJECTDTLCDMSMEDICALCONDITION[0].vDescription);
                                            $('#<%= txtOnsetDate.ClientID %>').val(txtOnsetDate);
                                            $('#<%= txtResolutionDate.ClientID %>').val(txtResolutionDate);
                                            $('#<%= ddlSource.ClientID %>').val(Data.PROJECTDTLCDMSMEDICALCONDITION[0].vSource);
                                            $('#<%= ddlConfirmed.ClientID %>').val(Data.PROJECTDTLCDMSMEDICALCONDITION[0].cConfirmed);
                                            $('#<%= txtComments.ClientID %>').val(Data.PROJECTDTLCDMSMEDICALCONDITION[0].vComments);
                                            $find('mpEditMedicalCond').show();
                                        }
                                    },
                           failure: function(error) {
                                       msgalert(error);
                                    }
                });
                
                 $('#<%= btnUpdateRecord.ClientID %>').click(function(){ 
                                                        $('#<%= btnRemarksDelete.ClientID %>').css('display','none');
                                                        $('#<%= btnRemarksUpdate.ClientID %>').css('display','inline');                                       
                                                        $find('mdlRemarksMedical').show();
                                                        return false;
                                                  });
               
              
                $('#<%= btnCancel.ClientID %>').click(function(){ 
                                                        //event.preventDefault(); 
                                                        $find('mpEditMedicalCond').hide();
                                                        return false;
                                                  });
                
            }
       function SetActivetab()
       {
           document.getElementById('ctl00_CPHLAMBDA_hdnActivetab').value = ($("#tabs").tabs().data().uiTabs.active).text().trimLeft();
       }            
       function clickMedical()
       {
             window.location.href = "frmCDMSProjectMedicalCondition.aspx?Mode=1&WorkspaceId=" + $('#<%= HProjectId.ClientID %>').val() + "&tab=Medical";
           
       }
       function clickConco()
       {
             window.location.href = "frmCDMSProjectMedicalCondition.aspx?Mode=1&WorkspaceId=" + $('#<%= HProjectId.ClientID %>').val() + "&tab=Medication";
       }  
    //Added For UK-UNK-UKUK IN Calendar
  var inyear;
  function DateConvertForScreening(ParamDate,txtdate)
        {
    
         if (ParamDate.length == 0)
           {
               return true;
           }
           
         if (ParamDate.trim() != '') {
        
              var dt = ParamDate.trim().toUpperCase();
              var tempdt;
              if (dt.indexOf('UK') >= 0 || dt.indexOf('UNK') >= 0 || dt.indexOf('UKUK') >= 0) {

               if (dt.length < 8) {
                msgalert('Please Enter Date In DDMMYYYY Or dd-Mon-YYYY Format Only.'); 
                txtdate.value = "";
                txtdate.focus();
                return false;
                }
                var day;
                var month;
                var year;
                if (dt.indexOf('-') >= 0) {
                var arrDate = dt.split('-');
                day = arrDate[0];
                month = arrDate[1];
                year = arrDate[2];
                }
                else {
                day = dt.substr(0, 2);
                month = dt.substr(2, 2);
                year = dt.substr(4, 4);
                if (dt.indexOf('UNK') >= 0) {
                    month = dt.substr(2, 3);
                    year = dt.substr(5, 4);
                }
                if (dt.indexOf('UNK') == -1) {
                    month = dt.substr(2, 2);
                    year = dt.substr(4, 5);
                }
            }
               inyear = parseInt(year, 10);
               
            if (day.length > 2 && day.length != 0) {
                msgalert('Please Enter Date In DDMMYYYY Or dd-Mon-YYYY Format Only.');
                txtdate.value = "";
                txtdate.focus();
                return false;
            }
            if (month.length > 3 && month.length != 3) {
                msgalert('Please Enter Date In DDMMYYYY Or dd-Mon-YYYY Format Only.');
                txtdate.value = "";
                txtdate.focus();
                return false;
            }
            if (year.length > 4 && month.length != 4) {
                msgalert('Please Enter Date In DDMMYYYY Or dd-Mon-YYYY Format Only.');
                txtdate.value = "";
                txtdate.focus();
                return false;
            }
            if (day == 'UK') {
                tempdt = '01';
            }
            else {
                tempdt = day;
            }
            if (dt.indexOf('-') >= 0) {
                tempdt += '-';
            }
            if (month == 'UNK') {
                tempdt += '01';
            }
            else {
                tempdt += month;
            }
            if (dt.indexOf('-') >= 0) {
                tempdt += '-';
            }
            if (year == 'UKUK') {
                tempdt += '1800';
            }
            else {
                tempdt += year;
            }
            var chk = false;
            chk = DateConvert(tempdt, txtdate);
            if (chk == true) {
                if (isNaN(month)) {
                    txtdate.value = day + '-' + month + '-' + year;
                }
                else {
                    txtdate.value = day + '-' + cMONTHNAMES[month - 1] + '-' + year;
                }
                if (inyear < 1900)
               {
                    msgalert('You Can Not Add Date Which Is Less Than "01-Jan-1900"  ');
                    txtdate.value = "";
                    txtdate.focus();
                    return false ;
               }
                return true;
            }
            txtdate.value = "";
            txtdate.focus();
            return false;
        }
     }
            DateConvert(txtdate.value, txtdate);
             dt = txtdate.value;
             var Year = dt.substring(dt.lastIndexOf('-') + 1);
             inyear = parseInt(Year, 10);
             if (inyear < 1900)
              {
                    msgalert('You Can Not Add Date Which Is Less Than "01-Jan-1900"  ');
                    txtdate.value = "";
                    txtdate.focus();
                    return false ;
              }
            var fromdate=document.getElementById('ctl00_CPHLAMBDA_txtStartDateConco');
            var todate = document.getElementById('ctl00_CPHLAMBDA_txtEndDateConco');    
            if (fromdate.value != "" && todate.value != "" ){
            var gap=GetDateDifference(fromdate.value, todate.value);
              if (gap.Days  == 0 || gap.Days  < 0){
                 msgalert('End Date Must Not Be Less Than From Date');    
                 todate.value = "";
               }
          }  
     return true ;  
 }

    </script>

</asp:Content>
