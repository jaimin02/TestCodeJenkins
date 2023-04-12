<%@ Page Language="VB" MasterPageFile="~/ECTDMasterPage.master" AutoEventWireup="false"
    CodeFile="frmCDMSStudyInformation.aspx.vb" Inherits="CDMS_frmCDMSStudyInformation"
    Title=":: CDMS - Study Information ::" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <link rel="stylesheet" href="../App_Themes/CDMS.css" />
    
    <script src="../Script/jquery-1.9.1.js" type="text/javascript"></script>

    <script src="../Script/jquery-ui.js" type="text/javascript"></script>

    <script src="../Script/General.js" type="text/javascript"></script>

    <script src="../Script/Validation.js" type="text/javascript"></script>

    <script src="../Script/jquery.dataTables.min.js" type="text/javascript"></script>

    <script src="../Script/AutoComplete.js" type="text/javascript"></script>

    
    <style type="text/css">
        .dataTables_wrapper .dataTable tbody td
        {
            padding: 0px 0px !important;
        }
        .innerTD
        {
            height: 30px;
            text-indent: 10px;
        }
        .EditControl {
            position:relative;
            top:5px;
        }
        .AuditControl {
            position:relative;
            top:5px;
        }
        .UpdateControl {
            position:relative;
            top:5px;
        }
    </style>
    <div style="text-align: Left; padding-bottom: 2%; padding-left: 9%;">
        <asp:Label ID="project" Text="Enter Project* :" CssClass="LabelText" runat="server"></asp:Label>
        <asp:TextBox ID="txtproject" runat="server" CssClass="TextBox" Width="40%" Style="margin-left: 5px;" TabIndex="1"></asp:TextBox>
        <asp:HiddenField ID="HProjectId" runat="server" />
        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
            TargetControlID="txtProject" ServicePath="~/AutoComplete.asmx" OnClientShowing="ClientPopulated"
            OnClientItemSelected="OnSelected" MinimumPrefixLength="1" ServiceMethod="GetMyProjectCompletionList"
            CompletionListElementID="pnlSelectedDate" CompletionListItemCssClass="autocomplete_listitem"
            CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListCssClass="autocomplete_list"
            BehaviorID="AutoCompleteExtender1">
        </cc1:AutoCompleteExtender>
        <asp:Panel ID="pnlSelectedDate" runat="server" Style="max-height: 150px; overflow: auto;
            overflow-x: hidden;" />
    </div>
    <div id="tabs" style="text-align: left; width: 99%">
        <ul>
            <li><a href="#tabs-0">
                <img alt="Study Information" src="images/Subject.png" style="padding-right: 8px;" />Study
                Information</a></li>
            <li onclick="return fnRedirect('Condition');"><a href="#tabs-1">
                <img alt="Medical Condition" src="images/Medical Condition.png" style="padding-right: 8px;" />
                Medical Condition</a></li>
            <li onclick="return fnRedirect('Medication');"><a href="#tabs-2">
                <img alt="Conco. Medication" src="images/Medication.png" style="padding-right: 8px;" />
                Conco. Medication</a></li>
        </ul>
        <div id="tabs-0" align="left">
            <asp:UpdatePanel ID="upSubject" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td colspan="2" style="height: 10px;">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset class="FieldSetBox" style="padding: 10px; width: 77%; float: left;">
                                    <legend class="LegendText" style="color: Black">General Information</legend>
                                    <table cellpadding="2" cellspacing="2">
                                        <tr>
                                            <td class="LabelText">
                                                Study :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStudy" Width="94%" runat="server" CssClass="TextBox" Enabled="false" />
                                            </td>
                                            <td class="LabelText">
                                                Study Type :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStudyType" Width="98%" runat="server" CssClass="TextBox" Enabled="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelText">
                                                Drug :
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtDrug" Width="99%" runat="server" CssClass="TextBox" Enabled="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelText">
                                                Start Date :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStartDate" runat="server" CssClass="TextBox EntryControl" onChange="DateConvertForScreening(this.value,this);"
                                                    ToolTip="Please enter date in dd-mm-yyyy format (e.g. '01012013'/'01-Jan-2013')" TabIndex="2"></asp:TextBox>
                                                <cc1:CalendarExtender ID="calStartDate" runat="server" TargetControlID="txtStartDate"
                                                    Format="dd-MMM-yyyy">
                                                </cc1:CalendarExtender>
                                                <asp:Image ID="imgEditStartDate" ToolTip="Edit Start Date" CssClass="EditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="2"/>
                                                <asp:Image ID="imgAuditStartDate" ToolTip="Audit Trail - Start Date" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="2"/>
                                            </td>
                                            <td class="LabelText">
                                                End Date :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEnddate" runat="server" CssClass="TextBox EntryControl" ToolTip="Please enter date in dd-mm-yyyy format (e.g. '01012013'/'01-Jan-2013')"
                                                    onChange="DateConvertForScreening(this.value,this);" TabIndex="3"/>
                                                <cc1:CalendarExtender ID="calEndDate" runat="server" TargetControlID="txtEnddate"
                                                    Format="dd-MMM-yyyy">
                                                </cc1:CalendarExtender>
                                                <asp:Image ID="imgEditEndDate" ToolTip="Edit End Date" CssClass="EditControl" runat="server"
                                                    ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="3"/>
                                                <asp:Image ID="imgAuditEndDate" ToolTip="Audit Trail - End Date" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="3"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelText" valign="top">
                                                Sponsor Name :
                                            </td>
                                            <td valign="top">
                                                <asp:TextBox ID="txtSponsorName" Width="94%" runat="server" Enabled="false" CssClass="TextBox" TabIndex="4"/>
                                            </td>
                                            <td class="LabelText" valign="top">
                                                Sponsor Address :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSponsorAddress" TextMode="MultiLine" Rows="5" Height="60px" Width="98%"
                                                    runat="server" CssClass="TextBox" Enabled="false" TabIndex="5"/>
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
                                <fieldset class="FieldSetBox" style="width: 19%; float: left;">
                                    <legend class="LegendText" style="color: Black">Study Quantities</legend>
                                    <table cellpadding="2" cellspacing="2">
                                        <tr>
                                            <td class="LabelText">
                                                Study Size :
                                            </td>
                                            <td class="LabelText" style="text-align: left;">
                                                <asp:TextBox ID="txtStudySize" runat="server" CssClass="TextBox EntryControl" Width="35px" TabIndex="6"/>
                                                <asp:Image ID="imgEditStudySize" ToolTip="Edit Study Size" CssClass="EditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="6"/>
                                                <asp:Image ID="imgAuditStudySize" ToolTip="Audit Trail - Study Size" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="6"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelText">
                                                Stand By :
                                            </td>
                                            <td class="LabelText" style="text-align: left;">
                                                <asp:TextBox ID="txtStandBy" runat="server" CssClass="TextBox EntryControl" Width="35px" TabIndex="7"/>
                                                <asp:Image ID="imgEditStandBy" ToolTip="Edit Stand By" CssClass="EditControl" runat="server"
                                                    ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="7"/>
                                                <asp:Image ID="imgAuditStandBy" ToolTip="Audit Trail - Stand Bu" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="7"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelText">
                                                Group :
                                            </td>
                                            <td class="LabelText" style="text-align: left;">
                                                <asp:TextBox ID="txtGroup" runat="server" CssClass="TextBox EntryControl" Width="35px" TabIndex="8"/>
                                                <asp:Image ID="imgEditGroup" ToolTip="Edit Group" CssClass="EditControl" runat="server"
                                                    ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="8"/>
                                                <asp:Image ID="imgAuditGroup" ToolTip="Audit Trail - Group" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="8"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelText">
                                                Periods :
                                            </td>
                                            <td class="LabelText" style="text-align: left;">
                                                <asp:TextBox ID="txtPeriods" runat="server" CssClass="TextBox EntryControl" Width="35px" TabIndex="9"/>
                                                <asp:Image ID="imgEditPeriods" ToolTip="Edit Periods" CssClass="EditControl" runat="server"
                                                    ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="9"/>
                                                <asp:Image ID="imgAuditPeriods" ToolTip="Audit Trail - Periods" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="9"/>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                                <fieldset class="FieldSetBox" style="padding: 10px; width: 73%; height: 90px; padding-top: 36px;">
                                    <legend class="LegendText" style="color: Black">Blood Volume / Washout Days</legend>
                                    <table cellpadding="2" cellspacing="2">
                                        <tr>
                                            <td class="LabelText">
                                                Required (ml) :
                                            </td>
                                            <td class="LabelText" style="text-align: left; width: 21%;">
                                                <asp:TextBox ID="txtRequired" runat="server" CssClass="TextBox EntryControl" Width="60px" TabIndex="10"/>
                                                <asp:Image ID="imgEditRequired" ToolTip="Edit Required" CssClass="EditControl" runat="server"
                                                    ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="10"/>
                                                <asp:Image ID="imgAuditRequired" ToolTip="Audit Trail - Required" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="10"/>
                                            </td>
                                            <td class="LabelText">
                                                No. Draws :
                                            </td>
                                            <td class="LabelText" style="text-align: left;">
                                                <asp:TextBox ID="txtNoDraws" runat="server" CssClass="TextBox EntryControl" Width="60px" TabIndex="11"/>
                                                <asp:Image ID="imgEditNoDraws" ToolTip="Edit No. Draws" CssClass="EditControl" runat="server"
                                                    ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="11"/>
                                                <asp:Image ID="imgAuditNoDraws" ToolTip="Audit Trail - No. Draws" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="11"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelText">
                                                Pre-study :
                                            </td>
                                            <td class="LabelText" style="text-align: left;">
                                                <asp:TextBox ID="txtPreStudy" runat="server" CssClass="TextBox EntryControl" Width="60px" TabIndex="12"/>
                                                <asp:Image ID="imgEditPreStudy" ToolTip="Edit Pre-study" CssClass="EditControl" runat="server"
                                                    ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="12"/>
                                                <asp:Image ID="imgAuditPreStudy" ToolTip="Audit Trail - Pre-study" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="12"/>
                                            </td>
                                            <td class="LabelText">
                                                Post-study :
                                            </td>
                                            <td class="LabelText" style="text-align: left;">
                                                <asp:TextBox ID="txtPostStudy" runat="server" CssClass="TextBox EntryControl" Width="60px" TabIndex="13"/>
                                                <asp:Image ID="imgEditPostStudy" ToolTip="Edit Post-study" CssClass="EditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="13"/>
                                                <asp:Image ID="imgAuditPostStudy" ToolTip="Audit Trail - Post-study" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="13"/>
                                            </td>
                                            <td class="LabelText">
                                                Post-dosage :
                                            </td>
                                            <td class="LabelText" style="text-align: left;">
                                                <asp:TextBox ID="txtPostDosage" runat="server" CssClass="TextBox EntryControl" Width="60px" TabIndex="14"/>
                                                <asp:Image ID="imgEditPostDosage" ToolTip="Edit Post-dosage" CssClass="EditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="14"/>
                                                <asp:Image ID="imgAuditPostDosage" ToolTip="Audit Trail - Post-dosage" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="14"/>
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
                                <fieldset class="FieldSetBox" style="padding: 10px; width: 95%;">
                                    <legend class="LegendText" style="color: Black">Measurements / Demographics</legend>
                                    <table cellpadding="2" cellspacing="2" width="100%">
                                        <tr>
                                            <td valign="top" style="width: 49%">
                                                <table cellpadding="2" cellspacing="2" width="100%">
                                                    <tr>
                                                        <td class="LabelText" style="width: 20%;">
                                                            Sex :
                                                        </td>
                                                        <td colspan="2" class="LabelText" style="text-align: left; padding-left: 4px;">
                                                            <asp:DropDownList ID="ddlSex" runat="server" Width="110px" CssClass="EntryControl" TabIndex="15">
                                                                <asp:ListItem Text="Male" Value="M"></asp:ListItem>
                                                                <asp:ListItem Text="Female" Value="F"></asp:ListItem>
                                                                <asp:ListItem Text="Both" Value="B"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:Image ID="imgEditSex" ToolTip="Edit Sex" CssClass="EditControl" runat="server"
                                                                ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="15"/>
                                                            <asp:Image ID="imgAuditSex" ToolTip="Audit Trail - Sex" CssClass="AuditControl" runat="server"
                                                                ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="15"/>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="LabelText" valign="top">
                                                            Race :
                                                        </td>
                                                        <td class="LabelText" style="text-align: left; font-weight: normal !important; width: 69%">
                                                            <fieldset class="FieldSetBox" style="height: 120px;">
                                                                <asp:CheckBoxList ID="chkRace" runat="server" CellPadding="2" CellSpacing="2" RepeatLayout="Table"
                                                                    RepeatDirection="Vertical" RepeatColumns="2" CssClass="RaceEntryControl" TabIndex="16">
                                                                    <asp:ListItem Text="Any Race" Value="Any Race" />
                                                                    <asp:ListItem Text="Asian/Oriental" Value="Asian/Oriental" />
                                                                    <asp:ListItem Text="Black" Value="Black" />
                                                                    <asp:ListItem Text="Caucasian" Value="Caucasian" />
                                                                    <asp:ListItem Text="Hispanic" Value="Hispanic" />
                                                                    <asp:ListItem Text="Mulatto" Value="Mulatto" />
                                                                    <asp:ListItem Text="Native" Value="Native" />
                                                                    <asp:ListItem Text="Verify at Screening" Value="Verify at Screening" />
                                                                </asp:CheckBoxList>
                                                            </fieldset>
                                                        </td>
                                                        <td valign="top" style="width: 10%">
                                                            <asp:Image ID="imgEditRace" ToolTip="Edit Race" CssClass="RaceEditControl" runat="server"
                                                                ImageUrl="~/CDMS/images/Edit_Small.png" ctype="Race" TabIndex="17"/>
                                                            <asp:Image ID="imgAuditRace" ToolTip="Audit Trail - Race" CssClass="RaceAuditControl"
                                                                runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" ctype="Race" TabIndex="17"/>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="LabelText">
                                                            Age Match To :
                                                        </td>
                                                        <td colspan="2" class="LabelText" style="text-align: left; padding-left: 4px;">
                                                            <asp:DropDownList ID="ddlAgeMatchTo" runat="server" Width="140px" CssClass="EntryControl" TabIndex="18">
                                                                <asp:ListItem Text="Start Date" Value="Start Date"></asp:ListItem>
                                                                <asp:ListItem Text="End Date" Value="End Date"></asp:ListItem>
                                                                <asp:ListItem Text="Start Date-End Date" Value="Start Date-End Date"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:Image ID="imgEditAgeMatchTo" ToolTip="Edit Age Match To" CssClass="EditControl"
                                                                runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="19"/>
                                                            <asp:Image ID="imgAuditAgeMatchTo" ToolTip="Audit Trail - Age Match To" CssClass="AuditControl"
                                                                runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="19"/>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td valign="top">
                                                <table cellpadding="2" cellspacing="2" width="100%">
                                                    <tr>
                                                        <td class="LabelText" style="width: 25%; padding-right: 10px; text-decoration: underline;">
                                                            Weight
                                                        </td>
                                                        <td class="LabelText" style="width: 18%; padding-left: 10px; text-align: left; text-decoration: underline;">
                                                            Minimum
                                                        </td>
                                                        <td class="LabelText" style="width: 18%; padding-left: 10px; text-align: left; text-decoration: underline;">
                                                            Maximum
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="LabelText" style="padding-right: 10px;">
                                                            Male (kg) :
                                                        </td>
                                                        <td class="LabelText" style="text-align: left;">
                                                            <asp:TextBox ID="txtMakeWeightMin" runat="server" CssClass="TextBox EntryControl"
                                                                Width="60px" TabIndex="20"/>
                                                            <asp:Image ID="imgEditMaleWeightMin" ToolTip="Edit Male Minimum Weight" CssClass="EditControl"
                                                                runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="20"/>
                                                            <asp:Image ID="imgAuditMaleWeightMin" ToolTip="Audit Trail - Male Minimum Weight"
                                                                CssClass="AuditControl" runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="20"/>
                                                        </td>
                                                        <td class="LabelText" style="text-align: left;">
                                                            <asp:TextBox ID="txtMaleWeightMax" runat="server" CssClass="TextBox EntryControl"
                                                                Width="60px" TabIndex="21"/>
                                                            <asp:Image ID="imgEditMaleWeightMax" ToolTip="Edit Male Maximum Weight" CssClass="EditControl"
                                                                runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="21"/>
                                                            <asp:Image ID="imgAuditMaleWeightMax" ToolTip="Audit Trail - Male Maximum Weight"
                                                                CssClass="AuditControl" runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="21"/>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="LabelText" style="padding-right: 10px;">
                                                            Female (kg) :
                                                        </td>
                                                        <td class="LabelText" style="text-align: left;">
                                                            <asp:TextBox ID="txtFemaleWeightMin" runat="server" CssClass="TextBox EntryControl"
                                                                Width="60px" TabIndex="22"/>
                                                            <asp:Image ID="imgEditFemaleWeightMin" ToolTip="Edit Female Minimum Weight" CssClass="EditControl"
                                                                runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="22"/>
                                                            <asp:Image ID="imgAuditFemaleWeightMin" ToolTip="Audit Trail - Female Minimum Weight"
                                                                CssClass="AuditControl" runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="22"/>
                                                        </td>
                                                        <td class="LabelText" style="text-align: left;">
                                                            <asp:TextBox ID="txtFemaleWeightMax" runat="server" CssClass="TextBox EntryControl"
                                                                Width="60px" TabIndex="23"/>
                                                            <asp:Image ID="imgEditFemaleWeightMax" ToolTip="Edit Female Maximum Weight" CssClass="EditControl"
                                                                runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="23"/>
                                                            <asp:Image ID="imgAuditFemaleWeightMax" ToolTip="Audit Trail - Female Maximum Weight"
                                                                CssClass="AuditControl" runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="23"/>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="LabelText" style="padding-right: 10px;">
                                                            BMI :
                                                        </td>
                                                        <td class="LabelText" style="text-align: left;">
                                                            <asp:TextBox ID="txtBMIMin" runat="server" CssClass="TextBox EntryControl" Width="60px" TabIndex="24"/>
                                                            <asp:Image ID="imgEditBMIMin" ToolTip="Edit Minimum BMI" CssClass="EditControl" runat="server"
                                                                ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="24"/>
                                                            <asp:Image ID="imgAuditBMIMin" ToolTip="Audit Trail - Minimum BMI" CssClass="AuditControl"
                                                                runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="24"/>
                                                        </td>
                                                        <td class="LabelText" style="text-align: left;">
                                                            <asp:TextBox ID="txtBMIMax" runat="server" CssClass="TextBox EntryControl" Width="60px" TabIndex="25"/>
                                                            <asp:Image ID="imgEditBMIMax" ToolTip="Edit Maximum BMI" CssClass="EditControl" runat="server"
                                                                ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="25"/>
                                                            <asp:Image ID="imgAuditBMIMax" ToolTip="Audit Trail - Maximum BMI" CssClass="AuditControl"
                                                                runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="25"/>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="LabelText" style="padding-right: 10px;">
                                                            Age (years) :
                                                        </td>
                                                        <td class="LabelText" style="text-align: left;">
                                                            <asp:TextBox ID="txtAgeMin" runat="server" CssClass="TextBox EntryControl" Width="60px" TabIndex="26"/>
                                                            <asp:Image ID="imgEditAgeMin" ToolTip="Edit Minimum Age" CssClass="EditControl" runat="server"
                                                                ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="26"/>
                                                            <asp:Image ID="imgAuditAgeMin" ToolTip="Audit Trail - Minimum Age" CssClass="AuditControl"
                                                                runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="26"/>
                                                        </td>
                                                        <td class="LabelText" style="text-align: left;">
                                                            <asp:TextBox ID="txtAgeMax" runat="server" CssClass="TextBox EntryControl" Width="60px" TabIndex="27"/>
                                                            <asp:Image ID="imgEditAgeMax" ToolTip="Edit Maximum Age" CssClass="EditControl" runat="server"
                                                                ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="27"/>
                                                            <asp:Image ID="imgAuditAgeMax" ToolTip="Audit Trail - Maximum Age" CssClass="AuditControl"
                                                                runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="27"/>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="LabelText" style="padding-right: 10px;">
                                                            Menstrual Cycle (days) :
                                                        </td>
                                                        <td class="LabelText" style="text-align: left;">
                                                            <asp:TextBox ID="txtMenustralCycleMin" runat="server" CssClass="TextBox EntryControl"
                                                                Width="60px" TabIndex="28"/>
                                                            <asp:Image ID="imgEditMenstraulCycleMin" ToolTip="Edit Minimum Menstrual Cycle" CssClass="EditControl"
                                                                runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="28"/>
                                                            <asp:Image ID="imgAuditMenstrualCycleMin" ToolTip="Audit Trail - Minimum Menstrual Cycle"
                                                                CssClass="AuditControl" runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="28"/>
                                                        </td>
                                                        <td class="LabelText" style="text-align: left;">
                                                            <asp:TextBox ID="txtMenstrualCycleMax" runat="server" CssClass="TextBox EntryControl"
                                                                Width="60px" TabIndex="29"/>
                                                            <asp:Image ID="imgEditMenstraulCycleMax" ToolTip="Edit Maximum Menstrual Cycle" CssClass="EditControl"
                                                                runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="29"/>
                                                            <asp:Image ID="imgAuditMenstraulCycleMax" ToolTip="Audit Trail - Maximum Menstrual Cycle"
                                                                CssClass="AuditControl" runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="29"/>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="LabelText" style="padding-right: 10px;">
                                                            Regular :
                                                        </td>
                                                        <td colspan="2" class="LabelText" style="text-align: left;">
                                                            <asp:DropDownList ID="ddlRegular" runat="server" Width="68px" CssClass="EntryControl" TabIndex="30">
                                                                <asp:ListItem Text="N/A" Value="N/A"></asp:ListItem>
                                                                <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:Image ID="imgEditRegular" ToolTip="Edit Regular" CssClass="EditControl" runat="server"
                                                                ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="30"/>
                                                            <asp:Image ID="imgAuditRegular" ToolTip="Audit Trail - Regular" CssClass="AuditControl"
                                                                runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="30"/>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <table cellpadding="2" cellspacing="2" width="100%">
                                                    <tr>
                                                        <td class="LabelText">
                                                            Availability :
                                                        </td>
                                                        <td class="LabelText" style="text-align: left;">
                                                            <asp:DropDownList ID="ddlAvailability" runat="server" Width="143px" CssClass="EntryControl" TabIndex="31">
                                                                <asp:ListItem Text="Any Availability" Value="Any Availability" />
                                                                <asp:ListItem Text="Includes all availability" Value="Includes all availability" />
                                                                <asp:ListItem Text="Monday to Friday" Value="Monday to Friday" />
                                                                <asp:ListItem Text="Friday to Monday" Value="Friday to Monday" />
                                                            </asp:DropDownList>
                                                            <asp:Image ID="imgEditAvailability" ToolTip="Edit Availability" CssClass="EditControl"
                                                                runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="31"/>
                                                            <asp:Image ID="imgAuditAvailability" ToolTip="Audit Trail - Availability" CssClass="AuditControl"
                                                                runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="31"/>
                                                        </td>
                                                        <td class="LabelText">
                                                            Regular Diet :
                                                        </td>
                                                        <td class="LabelText" style="text-align: left;">
                                                            <asp:DropDownList ID="ddlRegularDiet" runat="server" Width="110px" CssClass="EntryControl" TabIndex="32">
                                                                <asp:ListItem Text="N/A" Value="N/A" />
                                                                <asp:ListItem Text="Yes" Value="Y" />
                                                                <asp:ListItem Text="No" Value="N" />
                                                            </asp:DropDownList>
                                                            <asp:Image ID="imgEditRegularDiet" ToolTip="Edit Regular Diet" CssClass="EditControl"
                                                                runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="32"/>
                                                            <asp:Image ID="imgAuditRegularDiet" ToolTip="Audit Trail - Regular Diet" CssClass="AuditControl"
                                                                runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="32"/>
                                                        </td>
                                                        <td class="LabelText">
                                                            Swallow Pill :
                                                        </td>
                                                        <td class="LabelText" style="text-align: left;">
                                                            <asp:DropDownList ID="ddlSwallowPill" runat="server" Width="135px" CssClass="EntryControl" TabIndex="33">
                                                                <asp:ListItem Text="N/A" Value="N/A" />
                                                                <asp:ListItem Text="Yes" Value="Y" />
                                                                <asp:ListItem Text="No" Value="N" />
                                                            </asp:DropDownList>
                                                            <asp:Image ID="imgEditSwallow" ToolTip="Edit Swallow Pill" CssClass="EditControl"
                                                                runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="33"/>
                                                            <asp:Image ID="imgAuditSwallow" ToolTip="Audit Trail - Swallow Pill" CssClass="AuditControl"
                                                                runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="33"/>
                                                        </td>
                                                    </tr>
                                                </table>
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
                                <fieldset class="FieldSetBox" style="padding: 10px; width: 95%;">
                                    <legend class="LegendText" style="color: Black">General Consumption</legend>
                                    <table cellpadding="2" cellspacing="2" width="100%" style="padding: 5px;">
                                        <tr>
                                            <td class="LabelText">
                                                <asp:Image ID="imgEditConsumption" ToolTip="Edit General Consumption" cType="Consumption"
                                                    runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="34"/>
                                                <asp:ImageButton ID="imgAuditConsumption" ToolTip="Audit Trail - General Consumption"
                                                    cType="Consumption" runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="34"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="grdGeneralConmp" runat="server" AutoGenerateColumns="false">
                                                    <Columns>
                                                        <asp:BoundField DataField="nCDMSConsumptionNo" HeaderText="nCDMSConsumptionNo">
                                                            <ItemStyle HorizontalAlign="left" Width="20%"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vConsumptionType" HeaderText="Type">
                                                            <ItemStyle HorizontalAlign="left" Width="20%"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Status">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="14%"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hdnCDMSConsumptionNo" runat="server" Value='<%# databinder.eval(container.dataitem,"nCDMSConsumptionNo") %>' />
                                                                <asp:DropDownList ID="ddlConmpStatus" CssClass="LabelText EntryControl" runat="server" TabIndex="35">
                                                                    <asp:ListItem Text="N/A" Value="N/A" Selected="True" />
                                                                    <asp:ListItem Text="Never" Value="Never" />
                                                                    <asp:ListItem Text="Currently" Value="Currently" />
                                                                    <asp:ListItem Text="Stopped" Value="Stopped" />
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Min">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtConmpMin" runat="server" Width="40px" CssClass="TextBox ConEntryControl" TabIndex="35"/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Max">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtConmpMax" runat="server" Width="40px" CssClass="TextBox ConEntryControl" TabIndex="35"/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="vDescription" HeaderText="Description">
                                                            <ItemStyle HorizontalAlign="Left" Width="7%"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Frequency">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="12%"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:DropDownList CssClass="LabelText ConEntryControl" ID="ddlConmpFrequency" runat="server" TabIndex="35">
                                                                    <asp:ListItem Text="Yearly" Value="Yearly" />
                                                                    <asp:ListItem Text="Monthly" Value="Monthly" />
                                                                    <asp:ListItem Text="Weekly" Value="Weekly" />
                                                                    <asp:ListItem Text="Daily" Value="Daily" />
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Start Date">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="14%"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtStartDate" CssClass="TextBox ConEntryControl" runat="server"
                                                                    Width="80px" Style="font-size: 11px !Important;"   onChange="DateConvertForScreening(this.value,this);" ToolTip="Please enter date in dd-mm-yyyy format (e.g. '01012013'/'01-Jan-2013')" TabIndex="35"/>
                                                                <cc1:CalendarExtender ID="calStartDate" runat="server" TargetControlID="txtStartDate"
                                                                    Format="dd-MMM-yyyy">
                                                                </cc1:CalendarExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="End Date">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="14%"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtEndDate" CssClass="TextBox ConEntryControl"  Width="80px" Style="font-size: 11px !Important;" onChange="DateConvertForScreening(this.value,this);" ToolTip="Please enter date in dd-mm-yyyy format (e.g. '01012013'/'01-Jan-2013')" runat="server" TabIndex="35"/>
                                                                <cc1:CalendarExtender ID="calEndDate"
                                                                    runat="server" TargetControlID="txtEndDate" Format="dd-MMM-yyyy">
                                                                </cc1:CalendarExtender>
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
                            <td colspan="2" style="text-align: center;">
                                <asp:Button ID="btnSetProject" runat="server" Style="display: none" TabIndex="41"/>
                                <asp:Button ID="btnSaveStudy" runat="server" Text="Save" CssClass="btn btnsave" Width="66px"
                                    Style="font-size: 12px !important;" TabIndex="41"/>
                                <asp:Button ID="btnCancelStudy" runat="server" Text="Cancel" CssClass="btn btncancel"
                                    Width="66px" Style="font-size: 12px !important;" TabIndex="41"/>
                            </td>
                        </tr>
                    </table>
                    <asp:UpdatePanel ID="updAudit" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                        <ContentTemplate>
                            <asp:Button ID="btnFillAuditGrid" runat="server" Style="display: none;" TabIndex="42"/>
                            <asp:Button ID="btnConsAudit" runat="server" Style="display: none;" TabIndex="42"/>
                            <cc1:ModalPopupExtender ID="mdlConsAudit" runat="server" PopupControlID="divConsAudit"
                                BackgroundCssClass="modalBackground" BehaviorID="mdlConsAudit" CancelControlID="imgAuditClosePopup"
                                TargetControlID="btnConsAudit">
                            </cc1:ModalPopupExtender>
                            <div id="divConsAudit" runat="server" class="centerModalPopup" style="display: none;
                                overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto">
                                <table cellpadding="2" cellspacing="2" width="100%">
                                    <tr>
                                        <td class="LabelText" style="text-align: center !important; font-size: 12px !important;
                                            width: 97%;">
                                            Audit Trail - General Consumption
                                        </td>
                                        <td style="width: 3%">
                                            <img id="imgAuditClosePopup" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
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
                                                        <asp:GridView ID="grdAudit" runat="server" AutoGenerateColumns="false" style="width:100%;">
                                                            <Columns>
                                                                <asp:BoundField DataField="vConsumptionCode" HeaderText="Code">
                                                                    <ItemStyle HorizontalAlign="Left" Width="16%"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="vStatus" HeaderText="Status">
                                                                    <ItemStyle HorizontalAlign="center" Width="10%"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="nMin" HeaderText="Min">
                                                                    <ItemStyle HorizontalAlign="center" Width="10%"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="nMax" HeaderText="Max">
                                                                    <ItemStyle HorizontalAlign="center" Width="10%"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="vFrequency" HeaderText="Frequency">
                                                                    <ItemStyle HorizontalAlign="center" Width="10%"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="dStartDate" HeaderText="Start Date" DataFormatString="{0:dd-MMM-yyyy}"
                                                                    HtmlEncode="false">
                                                                    <ItemStyle HorizontalAlign="center" Width="10%"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="dEndDate" HeaderText="End Date" DataFormatString="{0:dd-MMM-yyyy}"
                                                                    HtmlEncode="false">
                                                                    <ItemStyle HorizontalAlign="center" Width="10%"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Audit">
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%"></ItemStyle>
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdnCode" runat="server" Value='<% #databinder.eval(container.dataitem,"vConsumptionCode") %>' />
                                                                        <asp:ImageButton ID="imgAudit" runat="server" ImageUrl="~/CDMS/images/Audit_Small.png"
                                                                            ToolTip="Audit Trail" Style="cursor: pointer;" TabIndex="43"/>
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
                            <asp:Button ID="btnRowAudit" runat="server" Style="display: none;" TabIndex="44"/>
                            <cc1:ModalPopupExtender ID="mdlRowAudit" runat="server" PopupControlID="divRowAudit"
                                BackgroundCssClass="modalBackground" BehaviorID="mdlRowAudit" CancelControlID="imgRowAuditClose"
                                TargetControlID="btnRowAudit">
                            </cc1:ModalPopupExtender>
                            <div id="divRowAudit" runat="server" class="centerModalPopup" style="display: none;
                                overflow: auto; width: 95%; height: auto; max-height: 80%; min-height: auto">
                                <table cellpadding="2" cellspacing="2" width="100%">
                                    <tr>
                                        <td class="LabelText" style="text-align: center !important; font-size: 12px !important;
                                            width: 97%;">
                                            Record Audit Trail - General Consumption
                                        </td>
                                        <td style="width: 3%">
                                            <img id="imgRowAuditClose" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
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
                                                        <asp:GridView ID="grdRowAudit" runat="server" AutoGenerateColumns="false">
                                                            <Columns>
                                                                <asp:BoundField DataField="vConsumptionCode" HeaderText="Code">
                                                                    <ItemStyle HorizontalAlign="Left" Width="13%"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="vStatus" HeaderText="Status">
                                                                    <ItemStyle HorizontalAlign="center" Width="8%"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="nMin" HeaderText="Min">
                                                                    <ItemStyle HorizontalAlign="center" Width="8%"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="nMax" HeaderText="Max">
                                                                    <ItemStyle HorizontalAlign="center" Width="8%"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="vFrequency" HeaderText="Frequency">
                                                                    <ItemStyle HorizontalAlign="center" Width="10%"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="dStartDate" HeaderText="Start Date" DataFormatString="{0:dd-MMM-yyyy}"
                                                                    HtmlEncode="false">
                                                                    <ItemStyle HorizontalAlign="center" Width="10%"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="dEndDate" HeaderText="End Date" DataFormatString="{0:dd-MMM-yyyy}"
                                                                    HtmlEncode="false">
                                                                    <ItemStyle HorizontalAlign="center" Width="10%"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="vRemarks" HeaderText="Reason">
                                                                    <ItemStyle HorizontalAlign="Left" Width="14%"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="UserName" HeaderText="Modify By">
                                                                    <ItemStyle HorizontalAlign="Left" Width="12%"></ItemStyle>
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
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnFillAuditGrid" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </ContentTemplate>
                <Triggers>
                    <%--<asp:AsyncPostBackTrigger ControlID="btnSaveStudy" EventName="Click" />--%>
                    <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelStudy" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <asp:Button ID="btnmdlSave" runat="server" Style="display: none;" TabIndex="45"/>
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
                        Are you sure you want to save Study Information?
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btnsave" Width="57px"
                            Style="font-size: 12px !important;" ToolTip="Save Subject" TabIndex="46"/>
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btncancel" Width="66px"
                            Style="font-size: 12px !important;" ToolTip="Cancel Subject" TabIndex="46"/>
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btnmdlCancel" runat="server" Style="display: none;" TabIndex="47"/>
        <cc1:ModalPopupExtender ID="mdlCancelAlert" runat="server" PopupControlID="divCancelAlert"
            BackgroundCssClass="modalBackground" BehaviorID="mdlCancelAlert" TargetControlID="btnmdlCancel">
        </cc1:ModalPopupExtender>
        <div id="divCancelAlert" runat="server" class="centerModalPopup" style="display: none;
            overflow: auto; width: 35%; height: auto; max-height: 25%; min-height: auto;">
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td class="LabelText" style="text-align: left !important; font-size: 12px !important;
                        width: 97%;">
                        Cancel Warning
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td class="LabelText" style="text-align: center !important;">
                        Are you sure you want to cancel?
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btnCancelCancel" runat="server" Text="Cancel" CssClass="btn btncancel" ToolTip="Cancel Subject" TabIndex="48"/>
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btnWarning" runat="server" Style="display: none;" TabIndex="49"/>
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
                        <asp:Button ID="btnWarningOk" runat="server" Text="Ok" CssClass="btn btnadd" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btnAlert" runat="server" Style="display: none;" TabIndex="50"/>
        <cc1:ModalPopupExtender ID="mdlAlert" runat="server" PopupControlID="divAlert" BackgroundCssClass="modalBackground"
            BehaviorID="mdlAlert" CancelControlID="btnNo" TargetControlID="btnAlert">
        </cc1:ModalPopupExtender>
        <div id="divAlert" runat="server" class="centerModalPopup" style="display: none;
            overflow: auto; width: 35%; height: auto; max-height: 25%; min-height: auto;">
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td id="AlertHeader" class="LabelText" style="text-align: left !important; font-size: 12px !important;
                        width: 97%;">
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td id="AlertMessage" class="LabelText" style="text-align: center !important;">
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btnYes" runat="server" Text="Yes" CssClass="ButtonText" Width="57px"
                            Style="font-size: 12px !important; display: inline;" TabIndex="51"/>
                        <asp:Button ID="btnNo" runat="server" Text="No" CssClass="ButtonText" Width="57px"
                            Style="font-size: 12px !important; display: inline;" TabIndex="51"/>
                        <asp:Button ID="btnOk" runat="server" Text="Ok" CssClass="ButtonText" Width="57px"
                            Style="font-size: 12px !important; display: none;" TabIndex="51"/>
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btnRemarks" runat="server" Style="display: none;" TabIndex="51"/>
        <cc1:ModalPopupExtender ID="mdlRemarks" runat="server" PopupControlID="divRemarks"
            BackgroundCssClass="modalBackground" BehaviorID="mdlRemarks" CancelControlID="btnRemarksCancel"
            TargetControlID="btnRemarks">
        </cc1:ModalPopupExtender>
        <div id="divRemarks" runat="server" class="centerModalPopup" style="display: none;
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
                        <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="5" Height="60px"
                            CssClass="TextBox" Width="300px" TabIndex="52"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btnRemarksUpdate" runat="server" Text="Update" CssClass="ButtonText"
                            Width="64px" Style="font-size: 12px !important;" TabIndex="53"/>
                        <asp:Button ID="btnRemarksCancel" runat="server" Text="Cancel" CssClass="ButtonText"
                            Width="64px" Style="font-size: 12px !important;" TabIndex="53"/>
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btnAudit" runat="server" Style="display: none;" TabIndex="54"/>
        <cc1:ModalPopupExtender ID="mdlAudit" runat="server" PopupControlID="divAudit" BackgroundCssClass="modalBackground"
            BehaviorID="mdlAudit" CancelControlID="imgClosePopup" TargetControlID="btnAudit">
        </cc1:ModalPopupExtender>
        <div id="divAudit" runat="server" class="centerModalPopup" style="display: none;
            overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto">
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td id="AuditHeader" class="LabelText" style="text-align: center !important; font-size: 12px !important;
                        width: 97%;">
                        Audit Trail
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
                                    <table id="tblAudit" width="100%">
                                    </table>
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
            </table>
        </div>
        <asp:Button ID="btnSaveAlert" runat="server" Style="display: none;" TabIndex="55"/>
        <cc1:ModalPopupExtender ID="mdlSaveRedirect" runat="server" PopupControlID="divSaveRedirect"
            BackgroundCssClass="modalBackground" BehaviorID="mdlSaveRedirect" TargetControlID="btnSaveAlert">
        </cc1:ModalPopupExtender>
        <div id="divSaveRedirect" runat="server" class="centerModalPopup" style="display: none;
            overflow: auto; width: 35%; height: auto; max-height: 25%; min-height: auto;">
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td class="LabelText" style="text-align: left !important; font-size: 12px !important;
                        width: 97%;">
                        Information
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td class="LabelText" style="text-align: center !important;">
                        Study Information saved successfully.
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btnOkAlert" runat="server" Text="Ok" CssClass="ButtonText" Width="57px"
                            Style="font-size: 12px !important;" TabIndex="56"/>
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <script type="text/javascript">
            var TableName="",ColumnName="",ChangedValue="",AuditTable,ClickedControl;
            $(function() {
                fnFunctionCall();
            });
            
            function fnFunctionCall()
            {
                $('.InnerTable').parent().attr('align','left')
                $('.InnerTable').parent().css('padding-left','4px');
                $( "#tabs" ).tabs().addClass( "ui-tabs-vertical ui-helper-clearfix" );
                $( "#tabs li" ).removeClass( "ui-corner-top" ).addClass( "ui-corner-left" );
                $( "#tabs" ).tabs({active:0});
                $('#tabs ul li a').click(function () {
                        location.href = this.href;
                });
                $('.ConEntryControl').each(function(){$(this).attr('disabled',true);});
                $('.EditControl').each(function(){this.style.display = "none";});
                $('.AuditControl').each(function(){this.style.display = "none";});
                $('.RaceEditControl').each(function(){this.style.display = "none";});
                $('.RaceAuditControl').each(function(){this.style.display = "none";});
                $('#<%= imgEditConsumption.ClientID %>')[0].style.display = "none";
                $('#<%= imgAuditConsumption.ClientID %>')[0].style.display = "none";
                $('#<%= btnCancelStudy.ClientID %>').unbind('click').click(function(){$find('mdlCancelAlert').show();  return false;});
                $('.EntryControl').each(function()
                                        {
                                            if($(this).attr('disabled') != undefined || $(this).attr('disabled') != null)
                                            {
                                                $(this).removeAttr('disabled');
                                            }
                                        });
                 $('.RaceEntryControl').find('tr td input').each(function()
                                        {
                                            if($(this).attr('disabled') != undefined || $(this).attr('disabled') != null)
                                            {
                                                $(this).removeAttr('disabled');
                                            }
                                        });

                 //if (fnGetQueryString("Mode") == 1 && fnGetQueryString("WorkspaceId") != undefined && fnGetQueryString("WorkspaceId") != "") {
                 //    fnAssignProperties();
                 //}
                 //else {
                 //    if ($("#ctl00_CPHLAMBDA_txtproject")[0].value != undefined && $("#ctl00_CPHLAMBDA_txtproject")[0].value != "") {
                 //        fnAssignProperties();
                 //    }
                //}

                if (fnGetQueryString("Mode") == 1 && fnGetQueryString("WorkspaceId") != undefined && fnGetQueryString("WorkspaceId") != "") {
                    fnAssignProperties();
                }

                $('#<%= btnWarningOk.ClientID %>').unbind('click').click(function(){$find('mdlWarning').hide();  return false;});
                $('#<%= btnCancelStudy.ClientID %>').unbind('click').click(function(){$find('mdlCancelAlert').show();  return false;});
                
                $('#<%= btnOk.ClientID %>').unbind('click').click(function(event){ 
                                                        event.preventDefault();
                                                        $find('mdlAlert').hide();
                                                        $find('mdlRemarks').show();
                                                        $('#AlertHeader').text('');
                                                        $('#AlertMessage').text('');
                                                        $('#<%= btnYes.ClientID %>').css('display','inline');
                                                        $('#<%= btnNo.ClientID %>').css('display','inline');
                                                        $('#<%= btnOk.ClientID %>').css('display','none');
                                                  });
                        
               
            
                fnConDataTable();
                fnControlBlur();
                fnEditField();
                fnUpdateField();
                fnSaveField();
                fnAuditTrail();
                fnStatusChange();
                fnValidateData();
            }
            
            function fnConDataTable()
            {
                 $('#<%= grdGeneralConmp.ClientID %>').prepend($('<thead>').append($('#<%= grdGeneralConmp.ClientID %> tr:first'))).dataTable({
                    "bPaginate": false,
                    "bInfo":false,
                    "bFilter":false,
                    "bSort": false,
                    "bDestory": true,
                    "bRetrieve": true,
                     "aoColumns": [
                                    { "sClass": "innerTD"},
                                    null,
                                    null,
                                    null,
                                    { "sClass": "innerTD"},
                                    null,
                                    null,
                                    null
                                 ]
                });
                $('#<%= grdGeneralConmp.ClientID %> tr:first').css('background-color','#3A87AD');
            }
            
            function fnAssignProperties()   
            {
                 var SWorkFlowStageId = '<%= Session(S_WorkFlowStageId).ToString() %>'; 
                 if (SWorkFlowStageId == '0')
                   {
                       $('.EditControl').each(function(){this.style.display = "inline";});
                       $('.AuditControl').each(function(){this.style.display = "inline";});
                       $('.RaceEditControl').each(function(){this.style.display = "inline";});
                       $('.RaceAuditControl').each(function(){this.style.display = "inline";});
                       $('#<%= imgEditConsumption.ClientID %>')[0].style.display = "inline";
                       $('#<%= imgAuditConsumption.ClientID %>')[0].style.display = "inline";
                   }
                 
                 $('.EntryControl').each(function(){$(this).attr('disabled','disabled');});
                 $('.RaceEntryControl').find('tr td input').each(function(){$(this).attr('disabled','disabled');});
                 
            }
            
           function ClientPopulated(sender, e)
           {
                ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
           }

           function OnSelected(sender, e)
           {
                ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
                $get('<%= HProjectId.clientid %>'),document.getElementById('<%= btnSetProject.ClientId %>'));
           }
            
           function fnStatusChange()
            {
                $("[id$='_ddlConmpStatus']").unbind('change').change(function(){
                    if($(this).val() != "Never" && $(this).val() != "N/A")
                    {
                        $($(this).parents('tr')[0]).find('.ConEntryControl').each(function(){
                            $(this).removeAttr('disabled');
                        });
                    }
                    else
                    {
                        $($(this).parents('tr')[0]).find('.ConEntryControl').each(function(){
                            $(this).attr('disabled',true);
                       });
                    }
                });
            }
            
            function fnApplyDataTable()
            {
                var SWorkFlowStageId = '<%= Session(S_WorkFlowStageId).ToString() %>'; 
                if (SWorkFlowStageId == '0')
                  {
                     $('.EditControl').each(function(){this.style.display = "inline";});
                     $('.AuditControl').each(function(){this.style.display = "inline";});
                     $('.RaceEditControl').each(function(){this.style.display = "inline";});
                     $('.RaceAuditControl').each(function(){this.style.display = "inline";});
                  } 
                
                $('.EntryControl').each(function(){$(this).attr('disabled','disabled');});
                $('.ConEntryControl').each(function(){$(this).attr('disabled','disabled');});
                $('.RaceEntryControl').find('tr td input').each(function(){$(this).attr('disabled','disabled');});
             
                $('#<%= grdAudit.ClientID %>').prepend($('<thead>').append($('#<%= grdAudit.ClientID %> tr:first'))).dataTable({
                    "bStateSave": false,
                    "bPaginate": true,
                    "sPaginationType": "full_numbers",
                    "bSort": true,
                    "bDestory": true,
                    "bRetrieve": true
                   
                });
                    $('#<%= grdAudit.ClientID %> tr:first').css('background-color','#3A87AD');
                
                $('#<%= grdRowAudit.ClientID %>').prepend($('<thead>').append($('#<%= grdRowAudit.ClientID %> tr:first'))).dataTable({
                    "bStateSave": false,
                    "bPaginate": true,
                    "sPaginationType": "full_numbers",
                    "bSort": true,
                    "bDestory": true,
                    "bRetrieve": true
                });
                    $('#<%= grdRowAudit.ClientID %> tr:first').css('background-color','#3A87AD');
            }
            
            function fnAuditTrail()
            {
                $('.AuditControl,.RaceAuditControl').unbind('click').click(function(){
                    $('#AuditHeader').text($(this).attr('title'));
                    var content = {};
                    if(fnGetQueryString('WorkspaceId') == "")
                    {
                        content.WorkspaceId = $('#<%= HProjectId.clientid %>').val() ;
                    } 
                    else
                    {
                        content.WorkspaceId = fnGetQueryString('WorkspaceId');
                    }
                    if($(this).hasClass('RaceAuditControl'))
                    {
                        content.ColumnName =  $(this).closest('tr').find('td table').attr('cName');
                    }
                    else
                    {
                        content.ColumnName = $(this.previousElementSibling.previousElementSibling).attr('cName');
                    }
                    
                    $.ajax({
                          type: "POST",
                          url: "frmCDMSStudyInformation.aspx/GetAuditTrailField",
                          data: JSON.stringify(content),          
                          contentType: "application/json; charset=utf-8",
                          dataType: "json",
                          success: function(data) {
                                   var aaDataSet = [];
                                   
                                   if(data.d != "")
                                   {
                                         data = JSON.parse(data.d);
                                         for (var Row = 0;Row < data.length;Row++)
                                         {
                                            var InDataSet = [];
                                            InDataSet.push(Row + 1 ,data[Row].vChangedValue,data[Row].vRemarks,data[Row].UserName,data[Row].dModifyOffSet);
                                            aaDataSet.push(InDataSet);
                                         }
                                         if($("#tblAudit").children().length > 0)
                                         {
                                            $("#tblAudit").dataTable().fnDestroy(); 
                                         }
                                         $('#tblAudit').prepend($('<thead>').append($('#tblAudit tr:first'))).dataTable({
                                                            "bPaginate": true,
                                                            "sPaginationType": "full_numbers",
                                                            "bSort": true,
                                                            "bDestory": true,
                                                            "bRetrieve": true ,
					                                        "aaData": aaDataSet,
					                                        "aoColumns": [
						                                        { "sTitle": "Sr. No." },
						                                        { "sTitle": "Value" },
						                                        { "sTitle": "Reason" },
						                                        { "sTitle": "Modify By"},
						                                        { "sTitle": "Modify On"}
					                                        ]
				                           });
				                           
                                          $find('mdlAudit').show();
                                   }
                                   else
                                   {
                                         // alert(data.d);
                                          $find('mdlWarning').show();
                                          //$('#WarningHeader').text('Warning');
                                          $('#WarningMessage').text('No Audit Trail found for this field.');
                                          return false;
                                   }
                          },
                          failure: function(error) {
                                    msgalert(error);
                          }
                     });
                });
            }
            
            function fnEditField()
            {
                $('.EditControl').unbind('click').click(function(){
                    $(this).attr('title',$(this).attr('title').replace('Edit','Update'));
                    $(this).attr('src',$(this).attr('src').replace('Edit.png','Update.png'));
                    $(this).attr('class','UpdateControl');
                    $(this.previousElementSibling).removeAttr('disabled');
                    fnUpdateField();
                });
                
               $('.RaceEditControl').unbind('click').click(function(){
                    $(this).attr('title',$(this).attr('title').replace('Edit','Update'));
                    $(this).attr('src',$(this).attr('src').replace('Edit.png','Update.png'));
                    $(this).attr('class','RaceUpdateControl');
                    $(this).closest('tr').find('td table tbody tr td input').removeAttr('disabled');
                    fnUpdateField();
                });
                                
                
               $('#<%= imgEditConsumption.ClientID %>').unbind('click').click(function(){
                    $(this).attr('title',$(this).attr('title').replace('Edit','Update'));
                    $(this).attr('src',$(this).attr('src').replace('Edit.png','Update.png'));
                    $(this).attr('class','UpdateConControl');
                    $($(this).parent().parent().parent()).find('.EntryControl').removeAttr('disabled');
                    $($(this).parent().parent().parent()).find("[id$='_ddlConmpStatus']").each(function(){
                           if($(this).val() != "Never" && $(this).val() != "N/A")
                           {
                                $($(this).parents('tr')[0]).find('.ConEntryControl').each(function(){
                                    $(this).removeAttr('disabled');
                                });
                           }
                           else
                           {
                                $($(this).parents('tr')[0]).find('.ConEntryControl').each(function(){
                                    $(this).attr('disabled',true);
                               });
                           }
                    });
                    fnUpdateField();
               });
            }
            function fnUpdateField()
            {
                 $('.UpdateControl').unbind('click').click(function(){
                      TableName = "StudyDtlCDMS";
                      ColumnName= $(this.previousElementSibling).attr('cName');
                      ChangedValue = $(this.previousElementSibling).val();
                      $("#<%= txtRemarks.ClientID %>").val('');
                      $find('mdlRemarks').show();
                      ClickedControl = $(this);
                });
                
                $('.RaceUpdateControl').unbind('click').click(function(){
                      var Value="";
                      TableName = "StudyDtlCDMS";
                      ColumnName= $(this).closest('tr').find('td table').attr('cName');
                      $(this).closest('tr').find('td table tbody tr td input:checked').each(function(){
                                Value += $(this).closest('td').find('label').text() + ",";
                      });
                      ChangedValue = Value.substring(0,Value.length - 1);
                      $("#<%= txtRemarks.ClientID %>").val('');
                      $find('mdlRemarks').show();
                      ClickedControl = $(this);
                });
                
                $('.UpdateConControl').unbind('click').click(function(){
                    $("#<%= txtRemarks.ClientID %>").val('');
                    $find('mdlRemarks').show();
                    ClickedControl = $(this);
               });
            }
            
            function fnSaveField()
            {
                $('#<%= btnRemarksUpdate.ClientID %>').unbind('click').click(function(){
                                                        event.preventDefault();
                                                        var content = {};
                                                        if(fnGetQueryString('WorkspaceId') == "")
                                                        {
                                                            content.WorkspaceId = $('#<%= HProjectId.clientid %>').val() ;
                                                        } 
                                                        else
                                                        {
                                                            content.WorkspaceId = fnGetQueryString('WorkspaceId');
                                                        }
                                                        content.ColumnName = ColumnName;
                                                        content.TableName = TableName;
                                                        content.ChangedValue = ChangedValue.trim();
                                                        content.Remarks =  $("#<%= txtRemarks.ClientID %>").val().trim();
                                                        content.JSONString = "";
                                                        if ($(ClickedControl).attr('ctype') == "Consumption")
                                                        {
                                                            var JSONObj = [];
                                                            var tblConsumption = $('#<%= grdGeneralConmp.ClientID %> tbody');
                                                            tblConsumption.children('tr .odd,.even').each(function(){
                                                               var Consumption={};
                                                               Consumption.nCDMSConsumptionNo = $($($($(this).children()[1]).children())[0]).val();
                                                               Consumption.vStatus = $($($($(this).children()[1]).children())[1]).val();
                                                               Consumption.nMin =  $($($(this).children()[2]).children()).val();
                                                               Consumption.nMax =  $($($(this).children()[3]).children()).val();
                                                               Consumption.vFrequency =  $($($(this).children()[5]).children()).val();
                                                               Consumption.dStartDate =  $($($(this).children()[6]).children()).val();
                                                               Consumption.dEndDate =  $($($(this).children()[7]).children()).val();
                                                               JSONObj.push(Consumption);
                                                            })
                                                            content.JSONString = JSON.stringify(JSONObj);
                                                        }
                                                        
                                                        if($("#<%= txtRemarks.ClientID %>").val().trim() == "")
                                                        {
                                                                $find('mdlAlert').show();
                                                                $find('mdlRemarks').hide();
                                                                $('#AlertHeader').text('Remarks Warning');
                                                                $('#AlertMessage').text('Please enter remarks.');
                                                                $('#<%= btnYes.ClientID %>').css('display','none');
                                                                $('#<%= btnNo.ClientID %>').css('display','none');
                                                                $('#<%= btnOk.ClientID %>').css('display','inline');
                                                        }
                                                        else
                                                        {
                                                            $.ajax({
                                                                      type: "POST",
                                                                      url: "frmCDMSStudyInformation.aspx/UpdateFieldValues",
                                                                      data: JSON.stringify(content),          
                                                                      contentType: "application/json; charset=utf-8",
                                                                      dataType: "json",
                                                                      success: function(data) {
                                                                            if(data.d)
                                                                            {
                                                                                $("#<%= txtRemarks.ClientID %>").val('');
                                                                                $find('mdlRemarks').hide();
                                                                                ClickedControl.attr('title',ClickedControl.attr('title').replace('Update','Edit'));
                                                                                ClickedControl.attr('src',ClickedControl.attr('src').replace('Update.png','Edit.png'));
                                                                                if ($(ClickedControl).attr('ctype') == "Consumption")
                                                                                {
                                                                                    $($(ClickedControl).parent().parent().parent()).find('.ConEntryControl').attr('disabled',true);
                                                                                    $($(ClickedControl).parent().parent().parent()).find('.EntryControl').attr('disabled',true);
                                                                                }
                                                                                else if($(ClickedControl).attr('ctype') == "Race")
                                                                                {
                                                                                    $('.RaceEntryControl').find('tr td input').each(function(){$(this).attr('disabled','disabled');});
                                                                                }
                                                                                else
                                                                                {
                                                                                    ClickedControl.attr('class','EditControl');
                                                                                    $(ClickedControl).prev().attr('disabled',true);
                                                                                }
                                                                                fnEditField();
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
                    
                });
            }
            
            function fnControlBlur()
            {
                    
//                    $("[id$='_txtStartDate']").blur(function(){
//                    debugger ;
//                                                        if(!DateConvert($(this).val(), $(this)))
//                                                        {   
//                                                            $(this).val('');
//                                                            return false;
//                                                        };
//                                                        });
//                    $("[id$='_txtEndDate']").blur(function(){
//                    debugger ;
//                                                        if(!DateConvert($(this).val(), $(this)))
//                                                        {
//                                                            $(this).val('');
//                                                            return false;
//                                                        }
//                                                        });
                    
                    $("[id$='_txtStartDate'].ConEntryControl").blur(function(){
                    
//                                                        if(!DateConvert($(this).val(), $(this)))
//                                                        {   
//                                                            $(this).val('');
//                                                            return false;
//                                                        };
                                                        
//                                                        if ($(this).val() != "" && !CheckDateLessThenToday($(this).val())) {
//                                                            $(this).val('');
//                                                            $find('mdlWarning').show();
//                                                            $('#WarningHeader').text('Warning');
//                                                            $('#WarningMessage').text('Start date should be less than current date.');
//                                                            return false;
//                                                        }
                                                        
                                                        });
                                                        
                    $("[id$='_txtEndDate'].ConEntryControl").blur(function(){
                    
//                                                        if(!DateConvert($(this).val(), $(this)))
//                                                        {
//                                                            $(this).val('');
//                                                            return false;
//                                                        }
                                                        
//                                                        if ($(this).val() != "" && !CheckDateLessThenToday($(this).val())) {
//                                                            $(this).val('');
//                                                            $find('mdlWarning').show();
//                                                            $('#WarningHeader').text('Warning');
//                                                            $('#WarningMessage').text('End date should be less than current date.');
//                                                            return false;
//                                                        }
                                                        });
  
                    
                    $("[id$='_txtMakeWeightMin']").blur(function(){
                                                        if($(this).val() != "")
                                                        {
                                                        if(!CheckDecimal($(this).val()) || $(this).val() < 0)
                                                        {
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Please enter valid Male minimum Weight in kilograms.');
                                                            $(this).val('');
                                                            $(this).focus();
                                                            return false;
                                                        }
                                                            $(this).val(parseFloat($(this).val()).toFixed(1));
                                                        return true ;
                                                        }
                                                        });
                                                        
                   $("[id$='_txtMaleWeightMax']").blur(function(){
                                                        if($(this).val() != "")
                                                        {
                                                        if(!CheckDecimal($(this).val()) || $(this).val() < 0)
                                                        {
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Please enter valid Male maximum Weight in kilograms.');
                                                            $(this).val('');
                                                            $(this).focus();
                                                            return false;
                                                        }
                                                            $(this).val(parseFloat($(this).val()).toFixed(1));
                                                        }
                                                        });
                   
                    $("[id$='_txtFemaleWeightMin']").blur(function(){
                                                        if($(this).val() != "")
                                                        {
                                                        if(!CheckDecimal($(this).val()) || $(this).val() < 0)
                                                        {
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Please enter valid Female minimum Weight in kilograms.');
                                                            $(this).val('');
                                                            $(this).focus();
                                                            return false;
                                                        }
                                                            $(this).val(parseFloat($(this).val()).toFixed(1));
                                                        }
                                                        });
                    $("[id$='_txtFemaleWeightMax']").blur(function(){
                                                        if($(this).val() != "")
                                                        {
                                                        if(!CheckDecimal($(this).val()) || $(this).val() < 0)
                                                        {
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Please enter valid Female maximum Weight in kilograms.');
                                                            $(this).val('');
                                                            $(this).focus();
                                                            return false;
                                                        }
                                                            $(this).val(parseFloat($(this).val()).toFixed(1));
                                                        }
                                                        });
                     $("[id$='_txtBMIMin']").blur(function(){
                      
                                                        if($(this).val() != "")
                                                        {
                                                        if(!CheckDecimal($(this).val()) || $(this).val() < 0)
                                                        {
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Please enter valid minimum BMI.');
                                                            $(this).val('');
                                                            $(this).focus();
                                                            return false;
                                                        }
                                                            $(this).val(parseFloat($(this).val()).toFixed(1));
                                                        }
                                                        });                                                                       
                      
                       $("[id$='_txtBMIMax']").blur(function(){
                                                        if($(this).val() != "")
                                                        {
                                                        if(!CheckDecimal($(this).val()) || $(this).val() < 0)
                                                        {
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Please enter valid maximum BMI.');
                                                            $(this).val('');
                                                            $(this).focus();
                                                            return false;
                                                        }
                                                            $(this).val(parseFloat($(this).val()).toFixed(1));
                                                        }
                                                        });      
                     $("[id$='_txtAgeMin']").blur(function(){
                                                        if($(this).val() != "")
                                                        {
                                                        if(!CheckDecimal($(this).val()) || $(this).val() < 0)
                                                        {
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Please enter valid minimum Age.');
                                                            $(this).val('');
                                                            $(this).focus();
                                                            return false;
                                                        }
                                                            $(this).val(parseFloat($(this).val()).toFixed(0));
                                                        }
                                                        });      
                       $("[id$='_txtAgeMax']").blur(function(){
                                                        if($(this).val() != "")
                                                        {
                                                        if(!CheckDecimal($(this).val()) || $(this).val() < 0)
                                                        {
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Please enter valid maximum Age.');
                                                            $(this).val('');
                                                            $(this).focus();
                                                            return false;
                                                        }
                                                            $(this).val(parseFloat($(this).val()).toFixed(0));
                                                        }
                                                        }); 
                      $("[id$='_txtMenustralCycleMin']").blur(function(){
                                                        if($(this).val() != "")
                                                        {
                                                        if(!CheckDecimal($(this).val()) || $(this).val() < 0)
                                                        {
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Please enter valid days in menstrual cycle.');
                                                            $(this).val('');
                                                            $(this).focus();
                                                            return false;
                                                        }
                                                            $(this).val(parseFloat($(this).val()).toFixed(0));
                                                        }
                                                        });      
                     $("[id$='_txtMenstrualCycleMax']").blur(function(){
                                                        if($(this).val() != "")
                                                        {
                                                        if(!CheckDecimal($(this).val()) || $(this).val() < 0)
                                                        {
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Please enter valid days in menstrual cycle.');
                                                            $(this).val('');
                                                            $(this).focus();
                                                            return false;
                                                        }
                                                            $(this).val(parseFloat($(this).val()).toFixed(0));
                                                        }
                                                        });
                      $("[id$='_txtConmpMin']").blur(function(){
                                                        if($(this).val() != "")
                                                        {
                                                        if(!CheckDecimal($(this).val()) || $(this).val() < 0)
                                                        {
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Please enter valid minimum Quantity.');
                                                            $(this).val('');
                                                            $(this).focus();
                                                            return false;
                                                        }
                                                        }
                                                        });
                      $("[id$='_txtConmpMax']").blur(function(){
                                                        if($(this).val() != "")
                                                        {
                                                        if(!CheckDecimal($(this).val()) || $(this).val() < 0)
                                                        {
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Please enter valid maximum Quantity.');
                                                            $(this).val('');
                                                            $(this).focus();
                                                            return false;
                                                        }
                                                        }
                                                        });
                     $("[id$='_txtNoDraws']").blur(function(){
                                                        if($(this).val() != "")
                                                        {
                                                        if(!CheckDecimal($(this).val()) || $(this).val() < 0)
                                                        {
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Please enter valid no. of Draws.');
                                                            $(this).val('');
                                                            $(this).focus();
                                                            return false;
                                                        }
                                                        }
                                                        });          
                   $("[id$='_txtRequired']").blur(function(){
                                                        if($(this).val() != "")
                                                        {
                                                        if(!CheckDecimal($(this).val()) || $(this).val() < 0)
                                                        {
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Please enter valid required blood in milliliters.');
                                                            $(this).val('');
                                                            $(this).focus();
                                                            return false;
                                                        }
                                                        }
                                                        });
                    $("[id$='_txtPreStudy']").blur(function(){
                                                        if($(this).val() != "")
                                                        {
                                                        if(!CheckDecimal($(this).val()) || $(this).val() < 0)
                                                        {
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Please enter valid days in Pre-study.');
                                                            $(this).val('');
                                                            $(this).focus();
                                                            return false;
                                                        }
                                                             $(this).val(parseFloat($(this).val()).toFixed(0));  
                                                        }
                                                        });          
                      $("[id$='_txtPostStudy']").blur(function(){
                                                        if($(this).val() != "")
                                                        {
                                                        if(!CheckDecimal($(this).val()) || $(this).val() < 0)
                                                        {
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Please enter valid days in Post-study.');
                                                            $(this).val('');
                                                            $(this).focus();
                                                            return false;
                                                        }
                                                             $(this).val(parseFloat($(this).val()).toFixed(0));
                                                        }
                                                        }); 
                    $("[id$='_txtPostDosage']").blur(function(){
                                                        if($(this).val() != "")
                                                        {
                                                        if(!CheckDecimal($(this).val()) || $(this).val() < 0)
                                                        {
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Please enter valid days in Post-dosage.');
                                                            $(this).val('');
                                                            $(this).focus();
                                                            return false;
                                                        }
                                                            $(this).val(parseFloat($(this).val()).toFixed(0));
                                                        }
                                                        });
                    $("[id$='_txtStudySize']").blur(function(){
                                                        if($(this).val() != "")
                                                        {
                                                        if(!CheckDecimal($(this).val()) || $(this).val() < 0)
                                                        {
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Please enter valid values in Study Size.');
                                                            $(this).val('');
                                                            $(this).focus();
                                                            return false;
                                                        }
                                                        }
                                                        });   
                   $("[id$='_txtStandBy']").blur(function(){
                                                        if($(this).val() != "")
                                                        {
                                                        if(!CheckDecimal($(this).val()) || $(this).val() < 0)
                                                        {
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Please enter valid values in Stand by subject.');
                                                            $(this).val('');
                                                            $(this).focus();
                                                            return false;
                                                        }
                                                        }
                                                        });   
                   $("[id$='_txtGroup']").blur(function(){
                                                        if($(this).val() != "")
                                                        {
                                                        if(!CheckDecimal($(this).val()) || $(this).val() < 0)
                                                        {
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Please enter valid values in Group.');
                                                            $(this).val('');
                                                            $(this).focus();
                                                            return false;
                                                        }
                                                        }
                                                        });
                  $("[id$='_txtPeriods']").blur(function(){
                                                        if($(this).val() != "")
                                                        {
                                                        if(!CheckDecimal($(this).val()) || $(this).val() < 0)
                                                        {
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Please enter valid values in Periods.');
                                                            $(this).val('');
                                                            $(this).focus();
                                                            return false;
                                                        }
                                                        }
                                                        });                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   
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
            
            function fnRedirect(str)
            {
                 window.location.href = "frmCDMSProjectMedicalCondition.aspx?Mode=1&WorkspaceId=" + $('#<%= HProjectId.ClientID %>').val() +"&tab=" + str;
            }
            
            $('#imgRowAuditClose').unbind('click').click(function(event){
                event.preventDefault();
                $('#<%= btnConsAudit.ClientID %>').click();
                $find('mdlRowAudit').hide();
           });
            
           function fnValidateData()
            {
                $('#<%= btnSaveStudy.ClientID %>').unbind('click').click(function(){
                     var WarningHeader="Warning",WarningMessage="";
                     $find('mdlSaveAlert').show();
                     return false;
                });
            }
            
             $('#imgRowAuditClose').unbind('click').click(function(event){
                //event.preventDefault();
                $('#<%= btnConsAudit.ClientID %>').click();
                $find('mdlRowAudit').hide();
                return false;
           });
           
           //Added For UK-UNK-UKUK IN Calendar
  var inyear;
  function DateConvertForScreening(ParamDate,txtdate)
        {
        debugger ;
         if (ParamDate.length == 0)
           {
               return true;
           }
           
         if (ParamDate.trim() != '') {
        
              var dt = ParamDate.trim().toUpperCase();
              var tempdt;
              if (dt.indexOf('UK') >= 0 || dt.indexOf('UNK') >= 0 || dt.indexOf('UKUK') >= 0) {

               if (dt.length < 8) {
                $find('mdlWarning').show();
                $('#WarningHeader').text('Warning');
                $('#WarningMessage').text('Please enter date in DDMMYYYY or dd-Mon-YYYY format only.');
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
                $find('mdlWarning').show();
                $('#WarningHeader').text('Warning');
                $('#WarningMessage').text('Please enter date in DDMMYYYY or dd-Mon-YYYY format only.');
                txtdate.value = "";
                txtdate.focus();
                return false;
            }
            if (month.length > 3 && month.length != 3) {
                $find('mdlWarning').show();
                $('#WarningHeader').text('Warning');
                $('#WarningMessage').text('Please enter date in DDMMYYYY or dd-Mon-YYYY format only.');
                txtdate.value = "";
                txtdate.focus();
                return false;
            }
            if (year.length > 4 && month.length != 4) {
                $find('mdlWarning').show();
                $('#WarningHeader').text('Warning');
                $('#WarningMessage').text('Please enter date in DDMMYYYY or dd-Mon-YYYY format only.');
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
                    $find('mdlWarning').show();
                    $('#WarningHeader').text('Warning');
                    $('#WarningMessage').text('You can not add date which is less than "01-Jan-1900" ');
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
                    $find('mdlWarning').show();
                    $('#WarningHeader').text('Warning');
                    $('#WarningMessage').text('You can not add date which is less than "01-Jan-1900" ');
                    txtdate.value = "";
                    txtdate.focus();
                    return false ;
              }
     return true ;  
 }
 
    </script>

</asp:Content>
