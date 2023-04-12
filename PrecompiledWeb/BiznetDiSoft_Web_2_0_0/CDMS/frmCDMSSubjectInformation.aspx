<%@ page language="VB" masterpagefile="~/ECTDMasterPage.master" autoeventwireup="false" inherits="CDMS_frmCDMSSubjectInformation, App_Web_4wz2dz2v" title=":: CDMS - Subject Information ::" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <link rel="stylesheet" href="../App_Themes/CDMS.css" />
    
    <script src="../Script/jquery-1.11.3.min.js"  type="text/javascript"></script>
    <script src="../Script/jquery-ui.js" type="text/javascript"></script>
    <script src="../Script/General.js" type="text/javascript"></script>
    <script src="../Script/jquery.dataTables.min.js" type="text/javascript"></script>
    <script src="../Script/Validation.js" type="text/javascript"></script>
    <script src="../Script/AutoComplete.js" type="text/javascript"></script>
    <script src="../Script/scrolltop.js" type="text/javascript"></script>

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
    </style>


    <div id="tabs" style="text-align: left; width: 99%">
        <ul>
            <li><a href="#tabs-0">
                <img alt="Subject Information" src="images/Subject.png" style="padding-right: 8px;" />Subject
                Information</a></li>
            <li onclick="fnRedirect('Condition');"><a href="#">
                <img alt="Medical Condition" src="images/Medical Condition.png" style="padding-right: 8px;" />Medical
                Condition</a></li>
            <li onclick="fnRedirect('Medication');"><a href="#">
                <img alt="Conco. Medication" src="images/Medication.png" style="padding-right: 8px;" />
                Conco. Medication</a></li>
            <li onclick="fnRedirect('StudyHistory');"><a href="#">
                <img alt="Study History" src="images/Studyhistory.png" style="padding-right: 8px;" />Study
                History</a></li>
        </ul>
        <div id="tabs-0" align="left">
            <asp:UpdatePanel ID="upSubject" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td colspan="2">
                                <fieldset class="FieldSetBox" style="width: 58%; height: 45px; float: left;" >
                                    <legend class="LegendText" style="color: Black">Search Subject</legend>
                                    <table cellpadding="2" cellspacing="2" width="100%">
                                        <tr>
                                            <td class="LabelText" style="text-align: left" colspan="2">
                                                <asp:TextBox ID="txtSearchSubject" runat="server" CssClass="TextBox" Style="width: 99%;" TabIndex="1"/>
                                                <asp:Button Style="display: none" ID="btnSearchSubject" runat="server" Text="Search Subject" />
                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" ServicePath="~/AutoComplete.asmx"
                                                    OnClientShowing="ClientPopulated" CompletionSetCount="10" OnClientItemSelected="OnSelected"
                                                    UseContextKey="True" MinimumPrefixLength="1" ServiceMethod="GetCDMSSubjectCompletionList"
                                                    CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                    CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1"
                                                    TargetControlID="txtSearchSubject">
                                                </cc1:AutoCompleteExtender>
                                                <asp:HiddenField ID="HSubjectId" runat="server"></asp:HiddenField>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                                <fieldset class="FieldSetBox" style="width: 22%; float: left;">
                                    <legend id="lblStatus" runat="server" class="LegendText" style="color: green; font-weight: bolder;
                                        font-size: 13px;">Status - Active</legend>
                                    <table cellpadding="2" cellspacing="2" width="100%">
                                        <tr>
                                            <td class="LabelText" style="text-align: center">
                                                <asp:DropDownList ID="ddlSubjectStatus" runat="server" Enabled="false" CssClass="EntryControl" TabIndex="2">
                                                    <asp:ListItem Text="Change Status" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Active" Value="AC"></asp:ListItem>
                                                    <asp:ListItem Text="Inactive" Value="IA"></asp:ListItem>
                                                    <asp:ListItem Text="On-Hold" Value="HO"></asp:ListItem>
                                                    <asp:ListItem Text="Forever Ineligible" Value="FI"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Image ID="imgEditSubjectStatus" ToolTip="Edit Subject Status" ctype="Status"
                                                    CssClass="EditControl" runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="2"/>
                                                <asp:Image ID="imgAuditSubjectStatus" ToolTip="Audit Trail - Subject Status" CssClass="AuditControl"
                                                    runat="server" ctype="Status" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="2"/>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                                <fieldset class="FieldSetBox" style="width: 7%; height: 45px;" id="fieldNew" runat="server">
                                    <legend class="LegendText" style="color: Black">New Entry</legend>
                                    <table cellpadding="2" cellspacing="2" width="100%">
                                        <tr>
                                            <td class="LabelText" style="text-align: left">
                                                <asp:Button ID="btnNewEntry" runat="server" Text="New" CssClass="btn btnnew" Width="60px"
                                                   ToolTip="New Entry" TabIndex="3"/>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="height: 10px;">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset class="FieldSetBox" style="padding: 10px; width: 77%; float: left; height: 132px;">
                                    <legend class="LegendText" style="color: Black">Personal Information</legend>
                                    <table cellpadding="2" cellspacing="2">
                                        <tr>
                                            <td class="LabelText">
                                                First Name* :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFirstName" runat="server" CssClass="TextBox EntryControl" TabIndex="4"/>
                                                <asp:Image ID="imgEditFirstName" ToolTip="Edit First Name" CssClass="EditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="4"/>
                                                <asp:Image ID="imgAuditFirstName" ToolTip="Audit Trail - First Name" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="4"/>
                                            </td>
                                            <td class="LabelText">
                                                Middle Name :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMiddleName" runat="server" CssClass="TextBox EntryControl" TabIndex="5"/>
                                                <asp:Image ID="imgEditMiddleName" ToolTip="Edit Middle Name" CssClass="EditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="5"/>
                                                <asp:Image ID="imgAuditMiddleName" ToolTip="Audit Trail - Middle Name" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="5"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelText">
                                                Last Name* :
                                            </td>
                                            <td width="37%">
                                                <asp:TextBox ID="txtLastName" runat="server" CssClass="TextBox EntryControl" TabIndex="6" />
                                                <asp:Image ID="imgEditLastName" ToolTip="Edit Last Name" CssClass="EditControl" runat="server"
                                                    ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="6"/>
                                                <asp:Image ID="imgAuditLastName" ToolTip="Audit Trail - Last Name" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="6"/>
                                            </td>
                                            <td class="LabelText">
                                                Initials :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtInitials" runat="server" CssClass="TextBox" Enabled="false" TabIndex="7"/>
                                                <asp:HiddenField ID="hdnInitials" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelText">
                                                Enrollment Date* :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEnrollmentDate" runat="server" ToolTip="Please enter date in dd-mm-yyyy format (e.g. '01012013'/'01-Jan-2013')"
                                                    CssClass="TextBox EntryControl" TabIndex="8"></asp:TextBox>
                                                <cc1:CalendarExtender ID="calEnrollmentDate" runat="server" TargetControlID="txtEnrollmentDate"
                                                    Format="dd-MMM-yyyy">
                                                </cc1:CalendarExtender>
                                                <asp:Image ID="imgEditEnrollmentDate" ToolTip="Edit Enrollment Date" CssClass="EditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Edit_Small.png"  TabIndex="8"/>
                                                <asp:Image ID="imgAuditEnrollmentDate" ToolTip="Audit Trail - Enrollment Date" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="8"/>
                                            </td>
                                            <td class="LabelText">
                                                Birthdate* :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBirthdate" runat="server" ToolTip="Please enter date in dd-mm-yyyy format (e.g. '01012013'/'01-Jan-2013')"
                                                    CssClass="TextBox EntryControl" TabIndex="9"/>
                                                <cc1:CalendarExtender ID="calBirthDate" runat="server" TargetControlID="txtBirthdate"
                                                    Format="dd-MMM-yyyy">
                                                </cc1:CalendarExtender>
                                                <asp:Image ID="imgEditBirthdate" ToolTip="Edit Birthdate" CssClass="EditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="9"/>
                                                <asp:Image ID="imgAuditBirthdate" ToolTip="Audit Trail - Birthdate" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="9"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelText">
                                                RSVP No :
                                            </td>
                                            <td valign="top">
                                                <asp:TextBox ID="txtRsvp" runat="server" CssClass="TextBox EntryControl" TabIndex="10"/>
                                                <asp:Image ID="imgEditRsvp" ToolTip="Edit RSVP Id" CssClass="EditControl" runat="server"
                                                    Style="vertical-align: top;" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="10"/>
                                                <asp:Image ID="imgAuditRsvp" ToolTip="Audit Trail - RSVP" CssClass="AuditControl"
                                                    runat="server" Style="vertical-align: top;" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="10"/>
                                            </td>
                                            <td class="LabelText">
                                                Age :
                                            </td>
                                            <td class="LabelText" style="text-align: left">
                                                <asp:TextBox ID="txtAge" runat="server" Enabled="false" Width="60px" CssClass="TextBox" TabIndex="11"></asp:TextBox>
                                                (years)
                                                <asp:HiddenField ID="hdnAge" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                                <fieldset class="FieldSetBox" style="width: 15%; height: 140px;">
                                    <legend class="LegendText" style="color: Black">Photograph</legend>
                                    <table cellpadding="2" cellspacing="2">
                                        <tr>
                                            <td style="text-align: center;">
                                                <asp:Image ID="imgSubjectPhoto" runat="server" ImageUrl="~/CDMS/images/NotFound.gif"
                                                    Width="110px" Height="110px" TabIndex="12"/>
                                                <asp:Label ID="lblSubjectPhoto" runat="server" Style="font-weight: bold; color: #3A7DC1;"></asp:Label>
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
                                    <legend class="LegendText" style="color: Black">Contact Information</legend>
                                    <table cellpadding="2" cellspacing="2">
                                        <tr>
                                            <td class="LabelText">
                                                Contact No. 1* :
                                            </td>
                                            <td width="38%" class="LabelText" style="text-align: left">
                                                <asp:DropDownList ID="ddlContact1Prefix" runat="server" CssClass="EntryControl" TabIndex="13">
                                                    <asp:ListItem Text="Primary" Value="Primary"></asp:ListItem>
                                                    <asp:ListItem Text="Work 1" Value="Work 1"></asp:ListItem>
                                                    <asp:ListItem Text="Work 2" Value="Work 2"></asp:ListItem>
                                                    <asp:ListItem Text="Cellular" Value="Cellular"></asp:ListItem>
                                                    <asp:ListItem Text="Pager" Value="Pager"></asp:ListItem>
                                                    <asp:ListItem Text="Cottage" Value="Cottage"></asp:ListItem>
                                                    <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Image ID="imgEditContact1Prefix" ToolTip="Edit Contact No. 1 Prefix" CssClass="EditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="13" />
                                                <asp:Image ID="imgAuditContact1Prefix" ToolTip="Audit Trail - Contact No. 1 Prefix"
                                                    CssClass="AuditControl" runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="13"/>
                                                <asp:TextBox ID="txtContactNo1" runat="server" CssClass="TextBox EntryControl" Width="109px" TabIndex="13"/>
                                                <asp:Image ID="imgEditContactNo1" ToolTip="Edit Contact No. 1" CssClass="EditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="13"/>
                                                <asp:Image ID="imgAuditContactNo1" ToolTip="Audit Trail - Contact No. 1" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="13"/>
                                            </td>
                                            <td class="LabelText">
                                                Email Address :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEmail" runat="server" CssClass="TextBox EntryControl" Width="210px" TabIndex="14"/>
                                                <asp:Image ID="imgEditEmail" ToolTip="Edit Email Address" CssClass="EditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="14"/>
                                                <asp:Image ID="imgAuditEmail" ToolTip="Audit Trail - Email Address" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="14"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelText">
                                                Contact No. 2 :
                                            </td>
                                            <td width="38%" class="LabelText" style="text-align: left">
                                                <asp:DropDownList ID="ddlContact2Prefix" runat="server" CssClass="EntryControl" TabIndex="15">
                                                    <asp:ListItem Text="Primary" Value="Primary"></asp:ListItem>
                                                    <asp:ListItem Text="Work 1" Value="Work 1"></asp:ListItem>
                                                    <asp:ListItem Text="Work 2" Value="Work 2"></asp:ListItem>
                                                    <asp:ListItem Text="Cellular" Value="Cellular"></asp:ListItem>
                                                    <asp:ListItem Text="Pager" Value="Pager"></asp:ListItem>
                                                    <asp:ListItem Text="Cottage" Value="Cottage"></asp:ListItem>
                                                    <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Image ID="imgEditContact2Prefix" ToolTip="Edit Contact No. 2 Prefix" CssClass="EditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="15"/>
                                                <asp:Image ID="imgAuditContact2Prefix" ToolTip="Audit Trail - Contact No. 2 Prefix"
                                                    CssClass="AuditControl" runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="15"/>
                                                <asp:TextBox ID="txtContactNo2" runat="server" CssClass="TextBox EntryControl" Width="109px" TabIndex="15"/>
                                                <asp:Image ID="imgEditContactNo2" ToolTip="Edit Contact No. 2" CssClass="EditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="15"/>
                                                <asp:Image ID="imgAuditContactNo2" ToolTip="Audit Trail - Contact No. 2" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="15"/>
                                            </td>
                                            <td class="LabelText">
                                                Place :
                                            </td>
                                            <td class="LabelText" style="text-align: left;">
                                                <asp:TextBox ID="txtPlace" runat="server" CssClass="TextBox EntryControl" Width="210px" TabIndex="16"/>
                                                <asp:Image ID="imgEditPlace" ToolTip="Edit Place" CssClass="EditControl" runat="server"
                                                    ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="16"/>
                                                <asp:Image ID="imgAuditPlace" ToolTip="Audit Trail - Place" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="16"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelText" valign="top">
                                                Address :
                                            </td>
                                            <td valign="top">
                                                <asp:TextBox ID="txtAddress" TextMode="MultiLine" Rows="5" Width="210px" Height="60px"
                                                    runat="server" CssClass="TextBox EntryControl" TabIndex="17"/>
                                                <asp:Image ID="imgEditAddress" ToolTip="Edit Address" CssClass="EditControl" Style="vertical-align: top;"
                                                    runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="17"/>
                                                <asp:Image ID="imgAuditAddress" ToolTip="Audit Trail - Address" CssClass="AuditControl"
                                                    Style="vertical-align: top;" runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="17"/>
                                            </td>
                                            <td class="LabelText" valign="top">
                                                Contact Comments :
                                            </td>
                                            <td valign="top">
                                                <asp:TextBox ID="txtContactComments" TextMode="MultiLine" runat="server" Rows="5"
                                                    Width="210px" Height="60px" CssClass="TextBox EntryControl" TabIndex="18"/>
                                                <asp:Image ID="imgEditContactComments" ToolTip="Edit Contact Comments" CssClass="EditControl"
                                                    runat="server" Style="vertical-align: top;" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="18"/>
                                                <asp:Image ID="imgAuditContactComments" ToolTip="Audit Trail - Contact Comments"
                                                    CssClass="AuditControl" runat="server" Style="vertical-align: top;" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="18"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="LabelText" style="text-align: left;">
                                                Permission to contact for future studies ?
                                                <asp:DropDownList ID="ddlContactFuture" runat="server" CssClass="EntryControl" TabIndex="19">
                                                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Image ID="imgEditContactFuture" ToolTip="Edit Permission" CssClass="EditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="19"/>
                                                <asp:Image ID="imgAuditContactFuture" ToolTip="Audit Trail - Permission" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="19"/>
                                            </td>
                                            <td class="LabelText">
                                                Recruiting Source :
                                            </td>
                                            <td class="LabelText" style="text-align: left;">
                                                <asp:DropDownList ID="ddlRecruitingSource" CssClass="EntryControl" runat="server"
                                                    Width=" 218px" onchange="return ddlRecruitingSourceSelectedIndexChange();" TabIndex="20"/>
                                                <asp:Image ID="imgEditRecruitingSource" ToolTip="Edit Recruiting Source" CssClass="EditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="20"/>
                                                <asp:Image ID="imgAuditRecruitingSource" ToolTip="Audit Trail - Recruiting Source"
                                                    CssClass="AuditControl" runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="20"/>
                                            </td>
                                        </tr>
                                        <tr id="trReference" style="display: none;" runat="server">
                                            <td class="LabelText">
                                                Ref. Subject :
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtreferSubject" runat="server" Width="93.5%" CssClass="TextBox EntryControl" TabIndex="21"/>
                                                <%--<asp:Button Style="display: none" ID="btnReferenceSubject" runat="server" />--%>
                                                <asp:Image ID="imgEdittxtreferSubject" ctype="RefSubject" ToolTip="Edit Reference Subject"
                                                    CssClass="EditControl" runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="21"/>
                                                <asp:Image ID="imgAudittxtreferSubject" ctype="RefSubject" ToolTip="Audit Trail - Reference Subject"
                                                    CssClass="AuditControl" runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="21"/>
                                                <input type="button" id="btnReferenceSubject" style="display: none;" TabIndex="21"/>
                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" ServicePath="~/AutoComplete.asmx"
                                                    OnClientShowing="ClientPopulatedReferenceSubject" CompletionSetCount="10" OnClientItemSelected="OnSelectedReferenceSubject"
                                                    UseContextKey="True" MinimumPrefixLength="1" ServiceMethod="GetCDMSSubjectCompletionList"
                                                    CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                    CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender2"
                                                    TargetControlID="txtreferSubject">
                                                </cc1:AutoCompleteExtender>
                                                <asp:HiddenField ID="hdnReferenceSubject" runat="server"></asp:HiddenField>
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
                                    <table cellpadding="2" cellspacing="2">
                                        <tr>
                                            <td class="LabelText">
                                                Sex :
                                            </td>
                                            <td class="LabelText" style="text-align: left;">
                                                <asp:DropDownList ID="ddlSex" runat="server" CssClass="EntryControl" TabIndex="22">
                                                    <asp:ListItem Text="Male" Value="M"></asp:ListItem>
                                                    <asp:ListItem Text="Female" Value="F"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Image ID="imgEditSex" ToolTip="Edit Sex" CssClass="EditControl" runat="server"
                                                    ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="22"/>
                                                <asp:Image ID="imgAuditSex" ToolTip="Audit Trail - Sex" CssClass="AuditControl" runat="server"
                                                    ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="22"/>
                                            </td>
                                            <td colspan="4">
                                                <table id="tdFemale" runat="server" style="display: none;" cellpadding="0" cellspacing="0"
                                                    width="100%">
                                                    <tr>
                                                        <td id="tdMenstrualLabel" class="LabelText" style="text-align: left;" width="105px">
                                                            Menstrual Cycle :
                                                        </td>
                                                        <td id="tdMenstrualText" class="LabelText" style="text-align: left;" width="155px">
                                                            <asp:TextBox ID="txtMenstrualCycle" runat="server" CssClass="TextBox EntryControl"
                                                                Width="50px" TabIndex="23"/>
                                                            days
                                                            <asp:Image ID="imgEditMenstrualCycle" ToolTip="Edit Messtrual Cycle" CssClass="EditControl"
                                                                runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="23"/>
                                                            <asp:Image ID="imgAuditMenstrualCycle" ToolTip="Audit Trail - Menstrual Cycle" CssClass="AuditControl"
                                                                runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="23"/>
                                                        </td>
                                                        <td id="tdRegularLabel" class="LabelText" style="text-align: left" width="57px">
                                                            Regular :
                                                        </td>
                                                        <td id="tdRegularText" class="LabelText" style="text-align: left;">
                                                            <asp:DropDownList ID="ddlRegular" runat="server" CssClass="EntryControl" TabIndex="23">
                                                                <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:Image ID="imgEditRegular" ToolTip="Edit Regular" CssClass="EditControl" runat="server"
                                                                ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="23"/>
                                                            <asp:Image ID="imgAuditRegular" ToolTip="Audit Trail - Regular" CssClass="AuditControl"
                                                                runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="23"/>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelText">
                                                Language :
                                            </td>
                                            <td class="LabelText" style="text-align: left;" width="22%">
                                                <asp:DropDownList ID="ddlLanguage" runat="server" Width="125px" CssClass="EntryControl" TabIndex="24">
                                                </asp:DropDownList>
                                                <asp:Image ID="imgEditLanguage" ToolTip="Edit Language" CssClass="EditControl" runat="server"
                                                    ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="24"/>
                                                <asp:Image ID="imgAuditLanguage" ToolTip="Audit Trail - Language" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="24"/>
                                            </td>
                                            <td class="LabelText">
                                                Race :
                                            </td>
                                            <td class="LabelText" style="text-align: left;" width="21%">
                                                <asp:DropDownList ID="ddlRace" runat="server" Width="110px" CssClass="EntryControl" TabIndex="25">
                                                    <asp:ListItem Text="Verify at Screening" Value="Verify at Screening" />
                                                    <asp:ListItem Text="Asian/Oriental" Value="Asian/Oriental" />
                                                    <asp:ListItem Text="Black" Value="Black" />
                                                    <asp:ListItem Text="Caucasian" Value="Caucasian" />
                                                    <asp:ListItem Text="Hispanic" Value="Hispanic" />
                                                    <asp:ListItem Text="Mulatto" Value="Mulatto" />
                                                    <asp:ListItem Text="Native" Value="Native" />
                                                </asp:DropDownList>
                                                <asp:Image ID="imgEditRace" ToolTip="Edit Race" CssClass="EditControl" runat="server"
                                                    ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="25"/>
                                                <asp:Image ID="imgAuditRace" ToolTip="Audit Trail - Race" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="25"/>
                                            </td>
                                            <td class="LabelText">
                                                Transportation :
                                            </td>
                                            <td class="LabelText">
                                                <asp:DropDownList ID="ddlTransportation" runat="server" Width="135px" CssClass="EntryControl" TabIndex="26">
                                                    <asp:ListItem Text="Public Transportation" Value="Public Transportation" />
                                                    <asp:ListItem Text="Car" Value="Car" />
                                                    <asp:ListItem Text="None" Value="None" />
                                                </asp:DropDownList>
                                                <asp:Image ID="imgEditTransportation" ToolTip="Edit Transportation" CssClass="EditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="26"/>
                                                <asp:Image ID="imgAuditTransportation" ToolTip="Audit Trail - Transportation" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="26"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelText">
                                                Availability :
                                            </td>
                                            <td class="LabelText" style="text-align: left;">
                                                <asp:DropDownList ID="ddlAvailability" runat="server" Width="125px" CssClass="EntryControl">
                                                    <asp:ListItem Text="Any Availability" Value="0" TabIndex="27"/>
                                                    <asp:ListItem Text="Includes all availability" Value="Includes all availability" />
                                                    <asp:ListItem Text="Monday to Friday" Value="Monday to Friday" />
                                                    <asp:ListItem Text="Friday to Monday" Value="Friday to Monday" />
                                                </asp:DropDownList>
                                                <asp:Image ID="imgEditAvailability" ToolTip="Edit Availability" CssClass="EditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="27"/>
                                                <asp:Image ID="imgAuditAvailability" ToolTip="Audit Trail - Availability" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="27"/>
                                            </td>
                                            <td class="LabelText">
                                                Regular Diet :
                                            </td>
                                            <td class="LabelText" style="text-align: left;">
                                                <asp:DropDownList ID="ddlRegularDiet" runat="server" Width="110px" CssClass="EntryControl" TabIndex="28">
                                                    <asp:ListItem Text="Yes" Value="Y" />
                                                    <asp:ListItem Text="No" Value="N" />
                                                </asp:DropDownList>
                                                <asp:Image ID="imgEditRegularDiet" ToolTip="Edit Regular Diet" CssClass="EditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="28"/>
                                                <asp:Image ID="imgAuditRegularDiet" ToolTip="Audit Trail - Regular Diet" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="28"/>
                                            </td>
                                            <td class="LabelText">
                                                Swallow Pill :
                                            </td>
                                            <td class="LabelText" style="text-align: left;">
                                                <asp:DropDownList ID="ddlSwallowPill" runat="server" Width="135px" CssClass="EntryControl" TabIndex="29">
                                                    <asp:ListItem Text="Yes" Value="Y" />
                                                    <asp:ListItem Text="No" Value="N" />
                                                </asp:DropDownList>
                                                <asp:Image ID="imgEditSwallow" ToolTip="Edit Swallow Pill" CssClass="EditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="29"/>
                                                <asp:Image ID="imgAuditSwallow" ToolTip="Audit Trail - Swallow Pill" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="29"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelText">
                                                Height (cm) :
                                            </td>
                                            <td class="LabelText" style="text-align: left;">
                                                <asp:TextBox ID="txtHeight" runat="server" CssClass="TextBox EntryControl" Width="60px" TabIndex="30"/>
                                                <asp:Image ID="imgEditHeight" ToolTip="Edit Height(cm)" CssClass="EditControl" runat="server"
                                                    ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="30"/>
                                                <asp:Image ID="imgAuditHeight" ToolTip="Audit Trail - Height(cm)" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="30"/>
                                            </td>
                                            <td class="LabelText">
                                                Weight (kg) :
                                            </td>
                                            <td class="LabelText" style="text-align: left;">
                                                <asp:TextBox ID="txtWeight" runat="server" CssClass="TextBox EntryControl" Width="60px" TabIndex="31"/>
                                                <asp:Image ID="imgEditWeight" ToolTip="Edit Weight(kg)" CssClass="EditControl" runat="server"
                                                    ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="31"/>
                                                <asp:Image ID="imgAuditWeight" ToolTip="Audit Trail - Weight(kg)" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="31"/>
                                            </td>
                                            <td class="LabelText">
                                                BMI :
                                            </td>
                                            <td class="LabelText" style="text-align: left;">
                                                <asp:TextBox ID="txtBMI" runat="server" CssClass="TextBox" Width="60px" Enabled="false" TabIndex="32"/>
                                                <asp:HiddenField ID="hdnBMI" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelText">
                                                Height (feet)* :
                                            </td>
                                            <td class="LabelText" style="text-align: left;">
                                                <asp:TextBox ID="txtHeightFeet" runat="server" CssClass="TextBox EntryControl" Width="60px" TabIndex="33"/>
                                                <asp:Image ID="imgEditHeightFeet" ToolTip="Edit Height(feet)" CssClass="EditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="33"/>
                                                <asp:Image ID="imgAuditHeightFeet" ToolTip="Audit Trail - Height(feet)" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="33"/>
                                            </td>
                                            <td class="LabelText">
                                                Weight (lb)* :
                                            </td>
                                            <td class="LabelText" style="text-align: left;">
                                                <asp:TextBox ID="txtWeightLb" runat="server" CssClass="TextBox EntryControl" Width="60px" TabIndex="34"/>
                                                <asp:Image ID="imgEditWeightLb" ToolTip="Edit Weight(lb)" CssClass="EditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="34"/>
                                                <asp:Image ID="imgAuditWeightLb" ToolTip="Audit Trail - Weight(lb)" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="34"/>
                                            </td>
                                            <td class="LabelText">
                                            </td>
                                            <td class="LabelText" style="text-align: left;">
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
                                                    runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="35"/>
                                                <asp:ImageButton ID="imgAuditConsumption" ToolTip="Audit Trail - General Consumption"
                                                    cType="Consumption" runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="35"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="grdGeneralConmp" runat="server" AutoGenerateColumns="false" TabIndex="36">
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
                                                                <asp:DropDownList ID="ddlConmpStatus" onchange="validateHabit()"  CssClass="LabelText EntryControl" runat="server" TabIndex="36">
                                                                    <asp:ListItem Text="Never" Value="Never" Selected="True" />
                                                                    <asp:ListItem Text="Currently" Value="Currently" />
                                                                    <asp:ListItem Text="Stopped" Value="Stopped" />
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Quantity">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtConmpQuantity" runat="server" Width="40px" CssClass="TextBox ConEntryControl" TabIndex="36"/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="vDescription" HeaderText="Description">
                                                            <ItemStyle HorizontalAlign="Left" Width="7%"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Frequency">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="12%"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:DropDownList CssClass="LabelText ConEntryControl" ID="ddlConmpFrequency" runat="server" TabIndex="36">
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
                                                                    Width="80px" Style="font-size: 11px !Important;" ToolTip="Please enter date in dd-mm-yyyy format (e.g. '01012013'/'01-Jan-2013')"
                                                                    onChange="DateConvertForScreening(this.value,this);" TabIndex="36"/>
                                                                <cc1:CalendarExtender ID="calStartDate" runat="server" TargetControlID="txtStartDate"
                                                                    Format="dd-MMM-yyyy">
                                                                </cc1:CalendarExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="End Date">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="14%"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtEndDate" CssClass="TextBox ConEntryControl" ToolTip="Please enter date in dd-mm-yyyy format (e.g. '01012013'/'01-Jan-2013')"
                                                                    runat="server" Style="font-size: 11px !Important;" Width="80px" onChange="DateConvertForScreening(this.value,this);" TabIndex="36"/>
                                                                <cc1:CalendarExtender ID="calEndDate" runat="server" TargetControlID="txtEndDate"
                                                                    Format="dd-MMM-yyyy">
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
                            <td valign="top" style="vertical-align: top; text-align: left" colspan="2">
                                <fieldset class="FieldSetBox" style="width: 95%; padding-top: 12px; height: 50px;">
                                    <legend class="LegendText" style="color: Black">Blood</legend>
                                    <table cellpadding="2" cellspacing="2">
                                        <tr>
                                            <td class="LabelText" style="width: 8%;">
                                                Available (ml) :
                                            </td>
                                            <td class="LabelText" style="text-align: left; width: 15%">
                                                <asp:TextBox ID="txtAvailableBlood" runat="server" CssClass="TextBox EntryControl"
                                                    Width="60px" TabIndex="37"/>
                                                <asp:Image ID="imgEditAvailableBlood" ToolTip="Edit Available" CssClass="EditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="37"/>
                                                <asp:Image ID="imgAuditAvailableBlood" ToolTip="Audit Trail - Available" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="37"/>
                                            </td>
                                            <td class="LabelText" style="width: 14%;">
                                                Wash Out Date :
                                            </td>
                                            <td style="text-align: left; width: 18%;">
                                                <asp:TextBox ID="txtWashoutDate" ToolTip="Please enter date in dd-mm-yyyy format (e.g. '01012013'/'01-Jan-2013')"
                                                    runat="server" CssClass="TextBox EntryControl" Width="80px" Style="font-size: 11px !Important;" TabIndex="38"/>
                                                <cc1:CalendarExtender ID="calWashoutDate" runat="server" TargetControlID="txtWashoutDate"
                                                    Format="dd-MMM-yyyy">
                                                </cc1:CalendarExtender>
                                                <asp:Image ID="imgEditWashoutDate" ToolTip="Edit Wash out Date" CssClass="EditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="38"/>
                                                <asp:Image ID="imgAuditWashoutDate" ToolTip="Audit Trail - Wash out Date" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="38"/>
                                            </td>
                                            <td class="LabelText" style="width: 6%;">
                                                Used (ml) :
                                            </td>
                                            <td class="LabelText" style="text-align: left; width: 14%;">
                                                <asp:TextBox ID="txtUsedBlood" runat="server" CssClass="TextBox EntryControl" Width="60px" TabIndex="39"/>
                                                <asp:Image ID="imgEditUsedBlood" ToolTip="Edit Used" CssClass="EditControl" runat="server"
                                                    ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="39"/>
                                                <asp:Image ID="imgAuditUsedBlood" ToolTip="Audit Trail - Used" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="39"/>
                                            </td>
                                            <td class="LabelText" style="width: 8%;">
                                                Last Study :
                                            </td>
                                            <td class="LabelText" style="text-align: left;">
                                                <asp:TextBox ID="txtLastStudy" runat="server" CssClass="TextBox EntryControl" Width="80px" TabIndex="40"/>
                                                <asp:Image ID="imgEditLastStudy" ToolTip="Edit Last Study" CssClass="EditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="40"/>
                                                <asp:Image ID="imgAuditLastStudy" ToolTip="Audit Trail - Last Study" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="40"/>
                                            </td>
                                        </tr>
                                        <%--<tr>
                                            <td class="LabelText">
                                                Used (ml) :
                                            </td>
                                            <td class="LabelText" style="text-align: left; width: 29%">
                                                <asp:TextBox ID="txtUsedBlood" runat="server" CssClass="TextBox EntryControl" Width="60px" />
                                                <asp:Image ID="imgEditUsedBlood" ToolTip="Edit Used" CssClass="EditControl" runat="server"
                                                    ImageUrl="~/CDMS/images/Edit_Small.png" />
                                                <asp:Image ID="imgAuditUsedBlood" ToolTip="Audit Trail - Used" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" />
                                            </td>
                                            <td class="LabelText">
                                                Last Study :
                                            </td>
                                            <td class="LabelText" style="text-align: left;">
                                                <asp:TextBox ID="txtLastStudy" runat="server" CssClass="TextBox EntryControl" Width="80px" />
                                                <asp:Image ID="imgEditLastStudy" ToolTip="Edit Last Study" CssClass="EditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" />
                                                <asp:Image ID="imgAuditLastStudy" ToolTip="Audit Trail - Last Study" CssClass="AuditControl"
                                                    runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" />
                                            </td>
                                        </tr>--%>
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
                                <fieldset class="FieldSetBox" style="width: 95%;">
                                    <legend class="LegendText" style="color: Black">Comments</legend>
                                    <table cellpadding="2" cellspacing="2" width="100%">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtComments" TextMode="MultiLine" Rows="5" Width="94%" Height="60px"
                                                    runat="server" CssClass="TextBox EntryControl" TabIndex="41"/>
                                                <asp:Image ID="imgEditComments" ToolTip="Edit Comments" CssClass="EditControl" Style="vertical-align: top;"
                                                    runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" TabIndex="41"/>
                                                <asp:Image ID="imgAuditComments" ToolTip="Audit Trail - Comments" CssClass="AuditControl"
                                                    Style="vertical-align: top;" runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" TabIndex="41"/>
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
                        <%--<tr>
                            <td colspan="2">
                                <fieldset class="FieldSetBox" style="width: 95%;">
                                    <legend class="LegendText" style="color: Black">Study History</legend>
                                    <table cellpadding="2" cellspacing="2" width="100%">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtStudyhistory" TextMode="MultiLine" Rows="5" Width="94%" Height="60px"
                                                    runat="server" CssClass="TextBox EntryControl" />
                                                <asp:Image ID="imgEditStudyhistory" ToolTip="Edit Study History" CssClass="EditControl"
                                                    Style="vertical-align: top;" runat="server" ImageUrl="~/CDMS/images/Edit_Small.png" />
                                                <asp:Image ID="imgAuditStudyhistory" ToolTip="Audit Trail - Study History" CssClass="AuditControl"
                                                    Style="vertical-align: top;" runat="server" ImageUrl="~/CDMS/images/Audit_Small.png" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>--%>
                        <tr style="height: 25px;">
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center;">
                                <asp:Button ID="btnSaveSubject" runat="server" Text="Save" CssClass="btn btnsave"
                                    Width="66px" Style="font-size: 12px !important;" ToolTip="Save Subject" TabIndex="42"/>
                                <asp:Button ID="btnCancelSubject" runat="server" Text="Cancel" CssClass="btn btncancel"
                                    Width="66px" Style="font-size: 12px !important;" ToolTip="Cancel Subject" TabIndex="42"/>
                            </td>
                        </tr>
                    </table>
                    <asp:UpdatePanel ID="updAudit" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                        <ContentTemplate>
                            <asp:Button ID="btnFillAuditGrid" runat="server" Style="display: none;" TabIndex="43"/>
                            <asp:Button ID="btnConsAudit" runat="server" Style="display: none;" TabIndex="43"/>
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
                                            <img id="imgAuditClosePopup" alt="Close" src="images/Close.png" onmouseover="this.style.cursor='pointer';" TabIndex="44"/>
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
                                                        <asp:GridView ID="grdAudit" runat="server" AutoGenerateColumns="false">
                                                            <Columns>
                                                                <asp:BoundField DataField="vConsumptionCode" HeaderText="Code">
                                                                    <ItemStyle HorizontalAlign="Left" Width="16%"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="vStatus" HeaderText="Status">
                                                                    <ItemStyle HorizontalAlign="center" Width="10%"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="nQuantity" HeaderText="Quantity">
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
                                                                            ToolTip="Audit Trail" Style="cursor: pointer;" TabIndex="45"/>
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
                            <asp:Button ID="btnRowAudit" runat="server" Style="display: none;" TabIndex="46"/>
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
                                            <img id="imgRowAuditClose" alt="Close" src="images/Close.png" onmouseover="this.style.cursor='pointer';" TabIndex="46"/>
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
                                                                <asp:BoundField DataField="nQuantity" HeaderText="Quantity">
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
                             <asp:HiddenField ID="hdnWorkSpaceID" runat="server" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnFillAuditGrid" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </ContentTemplate>
                <Triggers>
                    <%--<asp:AsyncPostBackTrigger ControlID="btnSaveSubject" EventName="Click" />--%>
                    <asp:AsyncPostBackTrigger ControlID="btnSearchSubject" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelSubject" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <asp:Button ID="btnmdlSave" runat="server" Style="display: none;" TabIndex="47"/>
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
                        Are you sure you want to save Subject Information?
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ButtonText" Width="57px"
                            Style="font-size: 12px !important;" ToolTip="Save Subject" TabIndex="48"/>
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ButtonText" Width="66px"
                            Style="font-size: 12px !important;" ToolTip="Cancel Subject" TabIndex="48"/>
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btnmdlCancel" runat="server" Style="display: none;" TabIndex="49"/>
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
                        <asp:Button ID="btnCancelCancel" runat="server" Text="Cancel" CssClass="btn btncancel" ToolTip="Cancel Subject" TabIndex="50"/>
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btnWarning" runat="server" Style="display: none;" TabIndex="51"/>
        <cc1:ModalPopupExtender ID="mdlWarning" runat="server" PopupControlID="divWarning"
            BackgroundCssClass="modalBackground" BehaviorID="mdlWarning" TargetControlID="btnWarning">
        </cc1:ModalPopupExtender>
        <div id="divWarning" runat="server" class="centerModalPopup" style="display: none;
            overflow: auto; width: 35%; height: auto; max-height: 25%; min-height: auto;
            z-index: 9999999 !important;">
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
                        <asp:Button ID="btnWarningOk" runat="server" Text="Ok" CssClass="btn btnadd" TabIndex="52"/>
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btnAlert" runat="server" Style="display: none;" TabIndex="53"/>
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
                            Style="font-size: 12px !important; display: inline;" TabIndex="54"/>
                        <asp:Button ID="btnNo" runat="server" Text="No" CssClass="ButtonText" Width="57px"
                            Style="font-size: 12px !important; display: inline;" TabIndex="54"/>
                        <asp:Button ID="btnOk" runat="server" Text="Ok" CssClass="ButtonText" Width="57px"
                            Style="font-size: 12px !important; display: none;" TabIndex="54"/>
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btnRemarks" runat="server" Style="display: none;" TabIndex="55"/>
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
                            CssClass="TextBox" Width="300px" TabIndex="55"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btnRemarksUpdate" runat="server" Text="Update" CssClass="btn btnupdate"
                            Width="64px" Style="font-size: 12px !important;" TabIndex="56" ToolTip="Update"/>
                        <asp:Button ID="btnRemarksCancel" runat="server" Text="Cancel" CssClass="btn btncancel"
                            Width="64px" Style="font-size: 12px !important;" TabIndex="56" ToolTip="Cancel"/>
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btnAudit" runat="server" Style="display: none;" TabIndex="57"/>
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
                        <img id="imgClosePopup" alt="Close" src="images/Close.png" onmouseover="this.style.cursor='pointer';" TabIndex="58"/>
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
        <asp:Button ID="btnSaveAlert" runat="server" Style="display: none;" TabIndex="59"/>
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
                        Subject Information saved successfully.
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btnOkAlert" runat="server" Text="Ok" CssClass="btn btnnew" Width="57px"
                            Style="font-size: 12px !important;" TabIndex="61"/>
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btnmdlStatus" runat="server" Style="display: none;" TabIndex="62"/>
        <cc1:ModalPopupExtender ID="mdlStatus" runat="server" PopupControlID="divStatus"
            BackgroundCssClass="modalBackground" BehaviorID="mdlStatus" CancelControlID="btnStatusCancel"
            TargetControlID="btnRemarks">
        </cc1:ModalPopupExtender>
        <div id="divStatus" runat="server" class="centerModalPopup" style="display: none;
            overflow: none; width: 36%; height: auto; max-height: 45%; min-height: auto;">
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td class="LabelText" style="text-align: left !important; font-size: 12px !important;
                        width: 97%;">
                        On-Hold Status
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <table style="width: 100%">
                            <tr>
                                <td class="LabelText" style="text-align: left !important;">
                                    Start Date:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtStatusStartDate" runat="server" CssClass="TextBox" Width="93px" TabIndex="63" ReadOnly="true"/>
                                    <cc1:CalendarExtender ID="calStatusStartDate" runat="server" TargetControlID="txtStatusStartDate"
                                        Format="dd-MMM-yyyy">
                                    </cc1:CalendarExtender>
                                </td>
                                <td class="LabelText" style="text-align: left !important;">
                                    End Date:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtStatusEndDate" runat="server" CssClass="TextBox" Width="93px" TabIndex="64" ReadOnly="true"/>
                                    <cc1:CalendarExtender ID="calStatusEndDate" runat="server" TargetControlID="txtStatusEndDate"
                                        Format="dd-MMM-yyyy">
                                    </cc1:CalendarExtender>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="LabelText" style="text-align: left !important;">
                        Remarks:
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left !important;">
                        <asp:TextBox ID="txtStatusRemarks" runat="server" TextMode="MultiLine" Rows="5" Height="60px"
                            CssClass="TextBox" Width="337px" TabIndex="65"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btnStatusUpdate" runat="server" Text="Update" CssClass="btn btnsave"
                            Width="64px" Style="font-size: 12px !important;" TabIndex="66" ToolTip="Update"/>
                        <asp:Button ID="btnStatusCancel" runat="server" Text="Cancel" CssClass="btn btncancel"
                            Width="64px" Style="font-size: 12px !important;" TabIndex="66" ToolTip="Cancel"/>
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="hdnweightkg" runat="server" />
        <asp:HiddenField ID="hdnweightlb" runat="server" />
        <asp:HiddenField ID="hdnheightcm" runat="server" />
        <asp:HiddenField ID="hdnheightft" runat="server" />
       
    </div>

    <script type="text/javascript">
            var TableName="",ColumnName="",ChangedValue="",AuditTable,ClickedControl;
            $(function() {
                fnFunctionCall();
            });
            
            $(function () {
                $("#tabs").tabs();
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
                $('#<%= imgEditConsumption.ClientID %>')[0].style.display = "none";
                $('#<%= imgAuditConsumption.ClientID %>')[0].style.display = "none";
                $('.EntryControl').each(function()
                                        {
                                            if($(this).attr('disabled') != undefined || $(this).attr('disabled') != null)
                                            {
                                                $(this).removeAttr('disabled');
                                            }
                                        });
                $('#<%= ddlSubjectStatus.ClientID %>').attr('disabled',true);    
                if(fnGetQueryString("Mode") == 2)
                {
                    fnAssignProperties();
                }
                $('#<%= btnWarningOk.ClientID %>').unbind('click').click(function(){$find('mdlWarning').hide();  return false;});
                $('#<%= btnCancelSubject.ClientID %>').unbind('click').click(function(){$find('mdlCancelAlert').show();  return false;});
                $('#<%= btnOk.ClientID %>').unbind('click').click(function(event){ 
                                                        //event.preventDefault();
                                                        $find('mdlAlert').hide();
                                                        $find('mdlRemarks').show();
                                                        $('#AlertHeader').text('');
                                                        $('#AlertMessage').text('');
                                                        $('#<%= btnYes.ClientID %>').css('display','inline');
                                                        $('#<%= btnNo.ClientID %>').css('display','inline');
                                                        $('#<%= btnOk.ClientID %>').css('display','none');
                                                        return false;
                                                  });
                        
               
            
                fnConDataTable();
                fnControlBlur();
                fnEditField();
                fnUpdateField();
                fnSaveField();
                fnAuditTrail();
                fnStatusChange();
                fnValidateData();
                fnChangeStatus();
            }
            
            function fnChangeStatus()
            {
              debugger ;
                $('#<%= ddlSubjectStatus.ClientId %>').change(function(){
                    if($(this).val() == 'HO')
                    {
                        $find('mdlStatus').show();
                    }
                });
                
                
                $('#<%= btnStatusUpdate.ClientId %>').unbind('click').click(function(){
                debugger ;
                     if($('#<%= txtStatusStartDate.ClientId %>').val() == "")
                       {
                           msgalert("Please Select Start Date");
                           $('#<%= txtStatusStartDate.ClientId %>').focus();
                           return false ;
                       }
                     else if($('#<%= txtStatusEndDate.ClientId %>').val() == "")
                       {
                          msgalert("Please Select End Date");
                          $('#<%= txtStatusEndDate.ClientId %>').focus();
                          return false ;
                       }
                     else if($('#<%= txtStatusRemarks.ClientId %>').val() == "")
                       {
                          msgalert("Please Enter Remarks");
                          $('#<%= txtStatusRemarks.ClientId %>').focus();
                          return false ;
                       }  
                     var data ={},content = {},JSONObj = [],SubjectId = '',status = {};
                     ClickedControl = $('#<%= imgEditSubjectStatus.ClientId %>');
                     if(fnGetQueryString('SubjectID') == "")
                     {
                         SubjectId = $('#<%= HSubjectId.clientid %>').val() ;
                     } 
                     else
                     {
                         SubjectId = fnGetQueryString('SubjectID');
                     }
                     content.vSubjectID = SubjectId;
                     content.vColumnName = 'cStatus';
                     content.vTableName = 'SubjectDtlCDMS';
                     content.vChangedValue = 'HO';
                     content.vRemarks =  $("#<%= txtStatusRemarks.ClientID %>").val().trim();
                     JSONObj.push(content);
                     content={};
                     content.vSubjectID = SubjectId;
                     content.vColumnName = 'dStartDate';
                     content.vTableName = 'SubjectDtlCDMS';
                     content.vChangedValue = $("#<%= txtStatusStartDate.ClientID %>").val().trim();
                     content.vRemarks =  $("#<%= txtStatusRemarks.ClientID %>").val().trim();
                     JSONObj.push(content);
                     content={};
                     content.vSubjectID = SubjectId;
                     content.vColumnName = 'dEndDate';
                     content.vTableName = 'SubjectDtlCDMS';
                     content.vChangedValue = $("#<%= txtStatusEndDate.ClientID %>").val().trim();
                     content.vRemarks =  $("#<%= txtStatusRemarks.ClientID %>").val().trim();
                     JSONObj.push(content);
                     status.workspaceid = $("#<%= hdnWorkSpaceID.ClientID %>").val().trim();
                     JSONObj.push(status);
                     
                     data.JSONString = JSON.stringify(JSONObj);
                     
                     if($("#<%= txtStatusRemarks.ClientID %>").val().trim() == "")
                     {
                           $find('mdlAlert').show();
                           $find('mdlStatus').hide();
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
                               url: "frmCDMSSubjectInformation.aspx/UpdateStatus",
                               data: JSON.stringify(data),          
                               contentType: "application/json; charset=utf-8",
                               dataType: "json",
                               success: function(data) {
                                        if(data.d)
                                        {
                                            $("#<%= txtStatusRemarks.ClientID %>").val('');
                                            $("#<%= txtStatusEndDate.ClientID %>").val('');
                                            $("#<%= txtStatusStartDate.ClientID %>").val('');
                                            ClickedControl.attr('title',ClickedControl.attr('title').replace('Update','Edit'));
                                            ClickedControl.attr('src',ClickedControl.attr('src').replace('Update.png','Edit.png'));
                                            $(ClickedControl).closest('fieldset').children('legend').text('Status - ' + $($(ClickedControl).closest('td').children('select').children('option:selected')).text());
                                            $($(ClickedControl).parent().parent().parent()).find('.EntryControl').attr('disabled',true);
                                            $(ClickedControl).attr('class','EditControl');
                                            $find('mdlStatus').hide();
                                            $($(ClickedControl).closest('td').children('select')).val("0");
                                            $(ClickedControl).closest('fieldset').children('legend').css('color','olive');
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
                       return false;
                });
            }
            
            function fnConDataTable()
            {
                 $('#<%= grdGeneralConmp.ClientID %>').prepend($('<thead>').append($('#<%= grdGeneralConmp.ClientID %> tr:first'))).dataTable({
                    "bPaginate": false,
                    "bInfo":false,
                    "bFilter":false,
                    "bSort": false,
                    "bDestory": true,
                     "aoColumns": [
                                    { "sClass": "innerTD"},
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
                      $('#<%= imgEditConsumption.ClientID %>')[0].style.display = "inline";
                      $('#<%= imgAuditConsumption.ClientID %>')[0].style.display = "inline";    
                  }
                 $('.EntryControl').each(function(){$(this).attr('disabled','disabled');});  
            }
            
            function ClientPopulated(sender, e)
            {
                    SubjectClientShowing('AutoCompleteExtender1', $get('<%= txtSearchSubject.ClientId %>'));
            }

            function OnSelected(sender, e)
            {
                    SubjectOnItemSelected(e.get_value(), $get('<%= txtSearchSubject.ClientId %>'),
                        $get('<%= HSubjectId.clientid %>'), document.getElementById('<%= btnSearchSubject.ClientId %>'));
            }
     
     
     
            function ClientPopulatedReferenceSubject(sender, e)
            {
                    SubjectClientShowing('AutoCompleteExtender2', $get('<%= txtreferSubject.ClientId %>'));
            }

            function OnSelectedReferenceSubject(sender, e)
            {
                    SubjectOnItemSelected(e.get_value(), $get('<%= txtreferSubject.ClientId %>'),
                        $get('<%= hdnReferenceSubject.clientid %>'),$("#btnReferenceSubject") );
            }
            function fnStatusChange()
            {
                $("[id$='_ddlConmpStatus']").unbind('change').change(function(){
                    if($(this).val() != "Never")
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
                  }
                    $('.EntryControl').each(function(){$(this).attr('disabled','disabled');});
                    $('.ConEntryControl').each(function(){$(this).attr('disabled','disabled');});               
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
            //debugger ;
                $('.AuditControl').unbind('click').click(function(){
                //debugger ;
                    $('#AuditHeader').text($(this).attr('title'));
                    var content = {};
                    if(fnGetQueryString('SubjectID') == "")
                    {
                        content.SubjectId = $('#<%= HSubjectId.clientid %>').val() ;
                    } 
                    else
                    {
                        content.SubjectId = fnGetQueryString('SubjectID');
                    }
                    content.ColumnName = $(this.previousElementSibling.previousElementSibling).attr('cName');
                    
                    $.ajax({
                          type: "POST",
                          url: "frmCDMSSubjectInformation.aspx/GetAuditTrailField",
                          data: JSON.stringify(content),          
                          contentType: "application/json; charset=utf-8",
                          dataType: "json",
                          success: function(data) {
                          //debugger ;
                                   var aaDataSet = [];
                                   
                                   if(data.d != "" && data.d != null)
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
                
               $('#<%= imgEditConsumption.ClientID %>').unbind('click').click(function(){
                    $(this).attr('title',$(this).attr('title').replace('Edit','Update'));
                    $(this).attr('src',$(this).attr('src').replace('Edit.png','Update.png'));
                    $(this).attr('class','UpdateConControl');
                    $($(this).parent().parent().parent()).find('.EntryControl').removeAttr('disabled');
                    $($(this).parent().parent().parent()).find("[id$='_ddlConmpStatus']").each(function(){
                           if($(this).val() != "Never")
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
                      TableName = $(this.previousElementSibling).attr('tName') == undefined ? "SubjectDtlCDMS" : $(this.previousElementSibling).attr('tName') ;
                      ColumnName= $(this.previousElementSibling).attr('cName');
                      ChangedValue = $(this.previousElementSibling).val();
                      if($(this).attr('ctype') == 'Status'){
                        if(ChangedValue == "0")
                        {
                            $find('mdlWarning').show();
                            $('#WarningHeader').text('Warning');
                            $('#WarningMessage').text('Please select status properly.');
                            return false;
                        }
                       }
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
               
                
                    $('#<%=btnRemarksUpdate.ClientID%>').attr('disabled', true);
                                                        //event.preventDefault();
                                                        var content = {};
                                                        if(fnGetQueryString('SubjectID') == "")
                                                        {
                                                            content.SubjectId = $('#<%= HSubjectId.clientid %>').val() ;
                                                        } 
                                                        else
                                                        {
                                                            content.SubjectId = fnGetQueryString('SubjectID');
                                                        }
                                                        content.workspaceid = $("#<%= hdnWorkSpaceID.ClientID %>").val().trim();
                                                        if (ColumnName == "vFirstName" || ColumnName == "vMiddleName" || ColumnName == "vSurName" || ColumnName == "dBirthdate" || ColumnName == "nWeight" || ColumnName == "nHeight" || ColumnName == "nWeightlb" || ColumnName == "nHeightfeet")
                                                           {
                                                              
                                                              var JSONObj = []; 
                                                              var RefValue={};
                                                              content.ColumnName = ColumnName;
                                                              content.TableName = TableName;
                                                              content.ChangedValue = ChangedValue.trim();
                                                              content.Remarks =  $("#<%= txtRemarks.ClientID %>").val().trim();
                                                              content.Refcolumn = "TRUE" 
                                                                if (ColumnName == "dBirthdate")
                                                                  {
                                                                       RefValue.TableName = TableName;
                                                                       RefValue.ColumnName = "nAge";
                                                                       RefValue.ChangedValue = $('#<%= txtAge.ClientID %>').val().trim()
                                                                       JSONObj.push(RefValue);
                                                                       content.JSONString = JSON.stringify(JSONObj);
                                                                  }
                                                                else if (ColumnName == "vFirstName" || ColumnName == "vMiddleName" || ColumnName == "vSurName")
                                                                  {
                                                                       RefValue.TableName = TableName;
                                                                       RefValue.ColumnName = "vInitials";
                                                                       RefValue.ChangedValue = $('#<%= txtInitials.ClientID %>').val().trim()
                                                                       JSONObj.push(RefValue);
                                                                       RefValue = {};
                                                                       RefValue.TableName = "SubjectDtlCDMS";
                                                                       RefValue.ColumnName = "vInitials";
                                                                       RefValue.ChangedValue = $('#<%= txtInitials.ClientID %>').val().trim()
                                                                       JSONObj.push(RefValue);
                                                                       content.JSONString = JSON.stringify(JSONObj);
                                                                  }
                                                                else if (ColumnName == "nWeight")
                                                                  {
                                                                       RefValue.TableName = TableName;
                                                                       RefValue.ColumnName = "nBMI";
                                                                       RefValue.ChangedValue = $('#<%= txtBMI.ClientID %>').val().trim()
                                                                       JSONObj.push(RefValue);
                                                                       RefValue = {};
                                                                       RefValue.TableName = TableName;
                                                                       RefValue.ColumnName = "nWeightlb";
                                                                       RefValue.ChangedValue = $('#<%= txtWeightLb.ClientID %>').val().trim()
                                                                       JSONObj.push(RefValue);
                                                                       content.JSONString = JSON.stringify(JSONObj);
                                                                  }
                                                               else if (ColumnName == "nHeight")
                                                                  {
                                                                       RefValue.TableName = TableName;
                                                                       RefValue.ColumnName = "nBMI";
                                                                       RefValue.ChangedValue = $('#<%= txtBMI.ClientID %>').val().trim()
                                                                       JSONObj.push(RefValue);
                                                                       RefValue = {};
                                                                       RefValue.TableName = TableName;
                                                                       RefValue.ColumnName = "nHeightfeet";
                                                                       RefValue.ChangedValue = $('#<%= txtHeightFeet.ClientID %>').val().trim()
                                                                       JSONObj.push(RefValue);
                                                                       content.JSONString = JSON.stringify(JSONObj);
                                                                  }
                                                               else if (ColumnName == "nWeightlb")
                                                                  {
                                                                       RefValue.TableName = TableName;
                                                                       RefValue.ColumnName = "nBMI";
                                                                       RefValue.ChangedValue = $('#<%= txtBMI.ClientID %>').val().trim()
                                                                       JSONObj.push(RefValue);
                                                                       RefValue = {};
                                                                       RefValue.TableName = TableName;
                                                                       RefValue.ColumnName = "nWeight";
                                                                       RefValue.ChangedValue = $('#<%= txtWeight.ClientID %>').val().trim()
                                                                       JSONObj.push(RefValue);
                                                                       content.JSONString = JSON.stringify(JSONObj);
                                                                  }
                                                               else if (ColumnName == "nHeightfeet")
                                                                  {
                                                                       RefValue.TableName = TableName;
                                                                       RefValue.ColumnName = "nBMI";
                                                                       RefValue.ChangedValue = $('#<%= txtBMI.ClientID %>').val().trim()
                                                                       JSONObj.push(RefValue);
                                                                       RefValue = {};
                                                                       RefValue.TableName = TableName;
                                                                       RefValue.ColumnName = "nHeight";
                                                                       RefValue.ChangedValue = $('#<%= txtHeight.ClientID %>').val().trim()
                                                                       JSONObj.push(RefValue);
                                                                       content.JSONString = JSON.stringify(JSONObj);
                                                                  }              
                                                           }
                                                        else 
                                                           {
                                                                 content.ColumnName = ColumnName;
                                                                 content.TableName = TableName;
                                                                 content.ChangedValue = ChangedValue.trim();
                                                                 content.Remarks =  $("#<%= txtRemarks.ClientID %>").val().trim();
                                                                 content.Refcolumn = ""
                                                                 content.JSONString = "";
                                                           }   
                                                        
                                                        if ($(ClickedControl).attr('ctype') == "Consumption")
                                                        {
                                                            
                                                            var JSONObj = [];
                                                            var tblConsumption = $('#<%= grdGeneralConmp.ClientID %> tbody');
                                                            tblConsumption.children('tr .odd,.even').each(function(){
                                                               var Consumption={};
                                                               Consumption.nCDMSConsumptionNo = $($($($(this).children()[1]).children())[0]).val();
                                                               Consumption.vStatus = $($($($(this).children()[1]).children())[1]).val();
                                                               Consumption.nQuantity =  $($($(this).children()[2]).children()).val();
                                                               Consumption.vFrequency =  $($($(this).children()[4]).children()).val();
                                                               Consumption.dStartDate =  $($($(this).children()[5]).children()).val();
                                                               Consumption.dEndDate =  $($($(this).children()[6]).children()).val();
                                                               JSONObj.push(Consumption);
                                                            })
                                                            content.JSONString = JSON.stringify(JSONObj);
                                                        }
                                                        else if ($(ClickedControl).attr('ctype') == "RefSubject")
                                                        {
                                                            content.ChangedValue =$(ClickedControl).parent().children('input[type=hidden]').val().trim();
                                                        }
                                                        
                                                        if($("#<%= txtRemarks.ClientID %>").val().trim() == "")
                                                        {
                                                                $find('mdlAlert').show();
                                                                $find('mdlRemarks').hide();
                                                                $('#AlertHeader').text('Remarks Warning');
                                                                $('#AlertMessage').text('Please enter remarks.');
                                                                $('#<%= btnYes.ClientID %>').css('display','none');
                                                                $('#<%= btnNo.ClientID %>').css('display','none');
                                                            $('#<%= btnOk.ClientID %>').css('display', 'inline');
                                                            $('#<%= btnRemarksUpdate.ClientID%>').attr('disabled', false);
                                                        }
                                                        else
                                                        {
                                                            $.ajax({
                                                                      type: "POST",
                                                                      url: "frmCDMSSubjectInformation.aspx/UpdateFieldValues",
                                                                      data: JSON.stringify(content),          
                                                                      contentType: "application/json; charset=utf-8",
                                                                      dataType: "json",
                                                                      success: function(data) {
                                                                            if(data.d)
                                                                            {
                                                                                ColumnName = "";TableName="";ChangedValue="";
                                                                                $("#<%= txtRemarks.ClientID %>").val('');
                                                                                $('#<%=btnRemarksUpdate.ClientID%>').attr('disabled', false);
                                                                                $find('mdlRemarks').hide();
                                                                                ClickedControl.attr('title',ClickedControl.attr('title').replace('Update','Edit'));
                                                                                ClickedControl.attr('src',ClickedControl.attr('src').replace('Update.png','Edit.png'));
                                                                                if ($(ClickedControl).attr('ctype') == "Consumption")
                                                                                {
                                                                                    $($(ClickedControl).parent().parent().parent()).find('.ConEntryControl').attr('disabled',true);
                                                                                    $($(ClickedControl).parent().parent().parent()).find('.EntryControl').attr('disabled',true);
                                                                                }
                                                                                else if($(ClickedControl).attr('ctype') == "Status")
                                                                                {
                                                                                //debugger ;
                                                                                    if ($($(ClickedControl).closest('td').children('select').children('option:selected')).val().toString() == "AC")
                                                                                        {
                                                                                            $(ClickedControl).closest('fieldset').children('legend').css('color','green');
                                                                                        }
                                                                                    else if ($($(ClickedControl).closest('td').children('select').children('option:selected')).val().toString() == "IA")
                                                                                        {
                                                                                            $(ClickedControl).closest('fieldset').children('legend').css('color','maroon');
                                                                                        }
                                                                                    else if ($($(ClickedControl).closest('td').children('select').children('option:selected')).val().toString() == "FI")
                                                                                        {
                                                                                            $(ClickedControl).closest('fieldset').children('legend').css('color','red');
                                                                                        }                 
                                                                                    $(ClickedControl).closest('fieldset').children('legend').text('Status - ' + $($(ClickedControl).closest('td').children('select').children('option:selected')).text());
                                                                                    $($(ClickedControl).parent().parent().parent()).find('.EntryControl').attr('disabled',true);
                                                                                    $(ClickedControl).attr('class','EditControl');
                                                                                    $($(ClickedControl).closest('td').children('select')).val("0");
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
                                                                                msgalert('Error while Saving to database');
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
            
            function fnControlBlur()
            {
                    $("[id$='_txtFirstName']").blur(function(){fnSetInitials();});
                    $("[id$='_txtMiddleName']").blur(function(){fnSetInitials();});
                    $("[id$='_txtLastName']").blur(function(){fnSetInitials();});
                    
                    $("[id$='_ddlSex']").change(function(){ 
                                                              $('#<%= tdFemale.ClientId %>').css('display','none');
                                                              if($(this).val() == "F")
                                                              {
                                                                    $('#<%= tdFemale.ClientId %>').css('display','block');
                                                              }
                                                          });
                                                          
                    $("[id$='_txtEnrollmentDate']").blur(function(){
                                                        if(!DateConvert($(this).val(), $(this)))
                                                        {   
                                                            $(this).val('');
                                                            return false;
                                                        }
                                                        if ($(this).val() != "" && !CheckDateLessThenToday($(this).val())) {
                                                            $(this).val('');
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Enrollment date should be less than current date.');
                                                            return false;
                                                        }
                                                        });
                                                        
                                                        
                                                        
//                    $("[id$='_txtWashoutDate']").blur(function(){
//                                                        if ($(this).val() != "" && !CheckDateLessThenToday($(this).val())) {
//                                                            $(this).val('');
//                                                            $find('mdlWarning').show();
//                                                            $('#WarningHeader').text('Warning');
//                                                            $('#WarningMessage').text('Washout date should be less than current date.');
//                                                            return false;
//                                                        }
//                                                        });
                    
//                     $("[id$='_txtStartDate']").blur(function(){
//                                                        if ($(this).val() != "" && !CheckDateLessThenToday($(this).val())) {
//                                                            $(this).val('');
//                                                            $find('mdlWarning').show();
//                                                            $('#WarningHeader').text('Warning');
//                                                            $('#WarningMessage').text('Start date should be less than current date.');
//                                                            return false;
//                                                        }
//                                                        });
//                     
//                      $("[id$='_txtEndDate']").blur(function(){
//                                                        if ($(this).val() != "" && !CheckDateLessThenToday($(this).val())) {
//                                                            $(this).val('');
//                                                            $find('mdlWarning').show();
//                                                            $('#WarningHeader').text('Warning');
//                                                            $('#WarningMessage').text('End date should be less than current date.');
//                                                            return false;
//                                                        }
//                                                        });
                    
                    $("[id$='_txtBirthdate']").blur(function(){
                                                        var txtbirth = document.getElementById('ctl00_CPHLAMBDA_txtBirthdate');
                                                        if(!DateConvert($(this).val(), txtbirth))
                                                        {
                                                            $(this).val('');
                                                            return false;
                                                        }
                                                        if ($(this).val() != "" && !CheckDateLessThenToday($(this).val())) {
                                                            $(this).val('');
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Birth date should be less than current date.');
                                                            return false;
                                                        }
                                                        
                                                        var txtAge = $('#<%= txtAge.ClientID %>');
                                                        if($(this).val().length > 8) {
                                                            //debugger ;
                                                            // Added by Jeet Patel on 15-Apr-2015 to Calculate age 
                                                            var fullDate = new Date()
                                                            var twoDigitMonth = ((fullDate.getMonth().length + 1) === 1) ? (fullDate.getMonth() + 1) : '0' + (fullDate.getMonth() + 1);
                                                            var currentDate = fullDate.getDate() + "-" + twoDigitMonth + "-" + fullDate.getFullYear();                                                    
                                                            var dob = GetDateDifference($(this).val(), currentDate);
                                                            // ====================================================
                                                            // var dob = GetDateDifference($(this).val(), $("[id$='_txtEnrollmentDate']").val()); As per issue Caluculate Birthdate with today date
                                                            if (dob.Days == NaN || dob.Years == NaN)
                                                            {
                                                                 msgalert('Please enter Valid Enrollment/Birth date.')
                                                                 return false ;
                                                            }
                                                            if ((dob.Days / 365) == 1) {
                                                                txtAge.val((dob.Years + 1).toString());
                                                            }
                                                            else {
                                                                txtAge.val(dob.Years.toString());
                                                            }
                                                            if (txtAge.val() < 18) {
                                                                 $(this).val('');
                                                                 $find('mdlWarning').show();
                                                                 $('#WarningHeader').text('Warning');
                                                                 $('#WarningMessage').text('Age of Subject can not be less than 18 years.');
                                                                  return false;
                                                            }
                                                        }
                                                        
                                                        });
                    
                   
                    $("[id$='_txtWeight']").blur(function(){
                                                        if($(this).val() != "")
                                                        {
                                                        if(!CheckDecimal($(this).val()) || $(this).val() < 0)
                                                        {
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Please enter valid weight in kilograms.');
                                                            $(this).val('');
                                                            $(this).focus();
                                                            return false;
                                                        }
                                                        if(document.getElementById('<%=hdnweightkg.ClientId %>').value != "")
                                                        {
                                                          var temp = document.getElementById('<%=hdnweightkg.ClientId %>').value; 
                                                          if (parseFloat(temp).toFixed(1) == $(this).val())
                                                          {
                                                              $(this).val("");
                                                              $(this).val(document.getElementById('<%=hdnweightkg.ClientId %>').value);
                                                              document.getElementById('<%=hdnweightkg.ClientId %>').value ="";
                                                          }
                                                        }
                                                        var weightlb = $(this).val() * 2.2046 * 100/100 ;
                                                        document.getElementById('<%=hdnweightlb.ClientId %>').value = weightlb;
                                                        
                                                        $("[id$='_txtWeightLb']").val(weightlb.toFixed(1));
                                                        var HeightValue = $("[id$='_txtHeight']").val() != '' ? $("[id$='_txtHeight']").val() : '0.00' ;
                                                        var WeightValue = $("[id$='_txtWeight']").val() != '' ? $("[id$='_txtWeight']").val() : '0.00';
                                                        var bmiValue = GetBMI(HeightValue, WeightValue); 
                                                        if ((bmiValue != null) && !isNaN(bmiValue) && (bmiValue != Infinity))
                                                        {  
                                                            bmiValue = parseFloat(bmiValue);
                                                            $("[id$='_txtBMI']").val(bmiValue.toFixed(1));
                                                        }
                                                        else
                                                        {
                                                             $("[id$='_txtBMI']").val(0);
                                                        }
                                                             $(this).val(parseFloat($(this).val()).toFixed(1));
                                                        }
                                                        });
                                                        
                    $("[id$='_txtHeight']").blur(function(){
                    //debugger ;
                                                        if($(this).val() != "")
                                                        {
                                                        if(!CheckDecimal($(this).val()) || $(this).val() < 0)
                                                        {
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Please enter valid height in centimeters.');
                                                            $(this).val('');
                                                            $(this).focus();
                                                            return false;
                                                        }
                                                        if(document.getElementById('<%=hdnheightcm.ClientId %>').value != "")
                                                        {
                                                          var temp = document.getElementById('<%=hdnheightcm.ClientId %>').value; 
                                                          if (parseFloat(temp).toString().indexOf('.') != -1)
                                                             {
                                                              if (parseFloat(temp).toString().substring (0,parseFloat(temp).toString().indexOf('.') + 3) == $(this).val())
                                                              {
                                                                  return false ;
//                                                                  $(this).val("");
//                                                                  $(this).val(document.getElementById('<%=hdnheightcm.ClientId %>').value);
                                                                  //document.getElementById('<%=hdnheightcm.ClientId %>').value ="";
                                                              }
                                                             }
                                                          else 
                                                             {
                                                              if (parseFloat(temp) == $(this).val())
                                                              {
                                                                  return false ;
//                                                                  $(this).val("");
//                                                                  $(this).val(document.getElementById('<%=hdnheightcm.ClientId %>').value);
                                                                  //document.getElementById('<%=hdnheightcm.ClientId %>').value ="";
                                                              }
                                                             }    
                                                        }
                                                        var heightft = Math.floor(($(this).val()/2.54)/12).toString() + "." + Math.round(($(this).val()/2.54)%12).toString() 
                                                        document.getElementById('<%=hdnheightft.ClientId %>').value = heightft;
                                                        if (heightft.toString().indexOf('.') != -1)
                                                        {
                                                           $("[id$='_txtHeightFeet']").val(heightft.toString().substring(0, heightft.toString().indexOf('.') + 3));
                                                        }
                                                        else 
                                                        {
                                                           $("[id$='_txtHeightFeet']").val(heightft);
                                                        }
                                                        var HeightValue = $("[id$='_txtHeight']").val() != '' ? $("[id$='_txtHeight']").val() : '0.00' ;
                                                        var WeightValue = $("[id$='_txtWeight']").val() != '' ? $("[id$='_txtWeight']").val() : '0.00';
                                                        var bmiValue = GetBMI(HeightValue, WeightValue); 
                                                        if ((bmiValue != null) && !isNaN(bmiValue) && (bmiValue != Infinity))
                                                        {  
                                                            bmiValue = parseFloat(bmiValue);
                                                            $("[id$='_txtBMI']").val(bmiValue.toFixed(1));
                                                        }
                                                        else
                                                        {
                                                             $("[id$='_txtBMI']").val(0);
                                                        }
                                                           if (parseFloat($(this).val()).toString().indexOf('.') != -1)
                                                            {
                                                              $(this).val($(this).val().toString().substring(0,$(this).val().toString().indexOf('.') + 3))
                                                              //$(this).val(parseFloat($(this).val()).toString().substring(0, parseFloat($(this).val()).toString().indexOf('.') + 3));
                                                            }
                                                           else 
                                                            {
                                                              $(this).val(parseFloat($(this).val()));
                                                            }
                                                        }
                                                        });
              $("[id$='_txtWeightLb']").blur(function(){
                                                        if($(this).val() != "")
                                                        {
                                                        if(!CheckDecimal($(this).val()) || $(this).val() < 0)
                                                        {
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Please enter valid weight in pound.');
                                                            $(this).val('');
                                                            $(this).focus();
                                                            return false;
                                                        }
                                                        if(document.getElementById('<%=hdnweightlb.ClientId %>').value != "")
                                                        {
                                                          var temp = document.getElementById('<%=hdnweightlb.ClientId %>').value; 
                                                          if (parseFloat(temp).toFixed(1) == $(this).val())
                                                          {
                                                              $(this).val("");
                                                              $(this).val(document.getElementById('<%=hdnweightlb.ClientId %>').value);
                                                              document.getElementById('<%=hdnweightlb.ClientId %>').value ="";
                                                          }
                                                        }
                                                        var weightkg = $(this).val() / 2.2046 * 100/100 ;
                                                        document.getElementById('<%=hdnweightkg.ClientId %>').value = weightkg;
                                                        $("[id$='_txtWeight']").val(weightkg.toFixed(1));
                                                        var HeightValue = $("[id$='_txtHeight']").val() != '' ? $("[id$='_txtHeight']").val() : '0.00' ;
                                                        var WeightValue = $("[id$='_txtWeight']").val() != '' ? $("[id$='_txtWeight']").val() : '0.00';
                                                        var bmiValue = GetBMI(HeightValue, WeightValue); 
                                                        if ((bmiValue != null) && !isNaN(bmiValue) && (bmiValue != Infinity))
                                                        {  
                                                            bmiValue = parseFloat(bmiValue);
                                                            $("[id$='_txtBMI']").val(bmiValue.toFixed(1));
                                                        }
                                                        else
                                                        {
                                                             $("[id$='_txtBMI']").val(0);
                                                        }
                                                             $(this).val(parseFloat($(this).val()).toFixed(1));
                                                        }
                                                        });
                                                        
                    $("[id$='_txtHeightFeet']").blur(function(){
                    //debugger ;
                                                        if($(this).val() != "")
                                                        {
                                                        if(!CheckDecimal($(this).val()) || $(this).val() < 0)
                                                        {
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Please enter valid height.');
                                                            $(this).val('');
                                                            $(this).focus();
                                                            return false;
                                                        }
                                                        if(document.getElementById('<%=hdnheightft.ClientId %>').value != "")
                                                        {
                                                          var temp = document.getElementById('<%=hdnheightft.ClientId %>').value ;
                                                          if (parseFloat(temp).toString().indexOf('.') != -1)
                                                             {
                                                              if (parseFloat(temp).toString().substring (0,parseFloat(temp).toString().indexOf('.') + 3) == $(this).val())
                                                              {
                                                                    return false ;
//                                                                  $(this).val("");
//                                                                  $(this).val(document.getElementById('<%=hdnheightft.ClientId %>').value);
                                                                  //document.getElementById('<%=hdnheightft.ClientId %>').value ="";
                                                              }
                                                             }
                                                          else 
                                                             {
                                                              if (parseFloat(temp) == $(this).val())
                                                              {
                                                                 return false ;
//                                                                  $(this).val("");
//                                                                  $(this).val(document.getElementById('<%=hdnheightft.ClientId %>').value);
                                                                 // document.getElementById('<%=hdnheightft.ClientId %>').value ="";
                                                              }
                                                             }    
                                                        }
                                                        if ($(this).val().indexOf('.') != -1)
                                                        {
                                                           var heightcm =($(this).val().substring(0, $(this).val().indexOf('.')) * 30.48) + ($(this).val().substring($(this).val().indexOf('.') + 1, $(this).val().indexOf('.') + 3) * 2.54)
                                                           document.getElementById('<%=hdnheightcm.ClientId %>').value = Math.round(heightcm);
                                                        }
                                                        else 
                                                        {
                                                           var heightcm = $(this).val() * 30.48;
                                                           document.getElementById('<%=hdnheightcm.ClientId %>').value =Math.round(heightcm);
                                                        }
                                                        if ( Math.round(heightcm).toString().indexOf('.') != -1)
                                                        {
                                                           $("[id$='_txtHeight']").val(heightcm.toString().substring(0, heightcm.toString().indexOf('.') + 3));
                                                        }
                                                        else 
                                                        {
                                                           $("[id$='_txtHeight']").val(Math.round(heightcm));
                                                        }
                                                        var HeightValue = $("[id$='_txtHeight']").val() != '' ? $("[id$='_txtHeight']").val() : '0.00' ;
                                                        var WeightValue = $("[id$='_txtWeight']").val() != '' ? $("[id$='_txtWeight']").val() : '0.00';
                                                        var bmiValue = GetBMI(HeightValue, WeightValue); 
                                                        if ((bmiValue != null) && !isNaN(bmiValue) && (bmiValue != Infinity))
                                                        {  
                                                            bmiValue = parseFloat(bmiValue);
                                                            $("[id$='_txtBMI']").val(bmiValue.toFixed(1));
                                                        }
                                                        else
                                                        {
                                                             $("[id$='_txtBMI']").val(0);
                                                        }
                                                           if (parseFloat($(this).val()).toString().indexOf('.') != -1)
                                                            {
                                                             $(this).val($(this).val().toString().substring(0,$(this).val().toString().indexOf('.') + 3))
                                                             // $(this).val(parseFloat($(this).val()).toString().substring(0, parseFloat($(this).val()).toString().indexOf('.') + 3));
                                                            }
                                                           else 
                                                            {
                                                              $(this).val(parseFloat($(this).val()));
                                                            }  
                                                        }
                                                        });
                                                        
                                                       
               $("[id$='_txtAvailableBlood']").blur(function(){
                                                        if($(this).val() != "")
                                                        {
                                                        if(!CheckDecimal($(this).val()) || $(this).val() < 0)
                                                        {
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Please enter valid Available Blood in milliliters.');
                                                            $(this).val('');
                                                            $(this).focus();
                                                            return false;
                                                        }
                                                        }
                                                        });
                                                        
                  $("[id$='_txtUsedBlood']").blur(function(){
                                                        if($(this).val() != "")
                                                        {
                                                        if(!CheckDecimal($(this).val()) || $(this).val() < 0)
                                                        {
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Please enter valid Used Blood in milliliters.');
                                                            $(this).val('');
                                                            $(this).focus();
                                                            return false;
                                                        }
                                                        }
                                                        });
                                                        
                   $("[id$='_txtConmpQuantity']").blur(function(){
                                                        if($(this).val() != "")
                                                        {
                                                        if(!CheckDecimal($(this).val()) || $(this).val() < 0)
                                                        {
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Please enter valid Quantity.');
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
            
            function fnSetInitials()
            {
                    $("[id$='_txtInitials']").val($("[id$='_txtFirstName']").val().substring(0,1) + 
                                                  $("[id$='_txtMiddleName']").val().substring(0,1) + 
                                                  $("[id$='_txtLastName']").val().substring(0,1));               
            }
            
            function fnRedirect(str)
            {
            //debugger ;
                if (str == "Condition")
                {
                     window.location.href = "frmCDMSMedicalCondition.aspx?Mode=2&SubjectID=" + $('#<%= HSubjectId.ClientID %>').val();
                }
                else if (str == "Medication")
                {
                     window.location.href = "frmCDMSConcoMedication.aspx?Mode=2&SubjectID=" + $('#<%= HSubjectId.ClientID %>').val();
                }
                else if (str == "StudyHistory")
                {
                     window.location.href = "frmCDMSStudyHistory.aspx?Mode=2&SubjectID=" + $('#<%= HSubjectId.ClientID %>').val();
                }
               
            }
            
            function fnValidateData()
            {
                $('#<%= btnSaveSubject.ClientID %>').unbind('click').click(function(){
                     var WarningHeader="Warning",WarningMessage="";
                     $('#<%= hdnInitials.ClientID %>').val($('#<%= txtInitials.ClientID %>').val());
                     $('#<%= hdnAge.ClientID %>').val($('#<%= txtAge.ClientID %>').val());
                     $('#<%= hdnBMI.ClientID %>').val($('#<%= txtBMI.ClientID %>').val());
                     if($('#<%= txtFirstName.ClientID %>').val().trim() == '')
                     {
                         $('#<%= txtFirstName.ClientID %>').focus();
                         msgalert("Please enter first name !");
                         return false
                           
                     }
                     else if($('#<%= txtLastName.ClientID %>').val().trim() == '')
                     {
                         $('#<%= txtLastName.ClientID %>').focus();
                         msgalert("Please enter last name !");
                         return false;
                     }
                     else if($('#<%= txtEnrollmentDate.ClientID %>').val().trim() == '')
                     {
                         $('#<%= txtEnrollmentDate.ClientID %>').focus();
                         msgalert("Please enter enrollment date.");
                         return false;
                     }
                     else if($('#<%= txtContactNo1.ClientID %>').val().trim() == '')
                     {
                         $('#<%= txtContactNo1.ClientID %>').focus();
                         msgalert("Please enter contact no. 1 !");
                         return false;
                     }
                     else if($('#<%= txtHeightFeet.ClientID %>').val().trim() == '')
                     {
                         $('#<%= txtHeightFeet.ClientID %>').focus();
                         msgalert("Please enter height in feet. !");
                         return false;
                     }
                     else if($('#<%= txtWeightLb.ClientID %>').val().trim() == '')
                     {
                         $('#<%= txtWeightLb.ClientID %>').focus();
                         msgalert("Please enter weight in pound !");
                         return false;
                     }
                     else if($('#<%= txtBirthdate.ClientID %>').val().trim() == '')
                     {
                         $('#<%= txtBirthdate.ClientID %>').focus();
                         msgalert("Please enter birthdate. !");
                         return false;
                     }
                     else if ($('#<%= txtAge.ClientID %>').val() < 17)
                     {
                         $('#<%= txtAge.ClientID %>').focus();
                         msgalert("Age of Subject can not be less than 17 years. !");
                         return false;
                     }

                    if (WarningMessage != "") {
                        $find('mdlWarning').show();
                        $('#WarningHeader').text(WarningHeader);
                        $('#WarningMessage').text(WarningMessage);
                        return false;
                    }

                    $find('mdlSaveAlert').show();

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
  function DateConvertForScreening(ParamDate,txtdate,mode)
        {
        
        var flag = false 
             if($(txtdate).attr('id') == '<%= txtStatusStartDate.ClientID %>' || $(txtdate).attr('id') == '<%= txtStatusEndDate.ClientID %>')
                 {
                   var flag = true  
                 }
             if (ParamDate.length == 0)
                 {
                   return true;
                 }
           
             if (ParamDate.trim() != '') 
                 {
            
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
                   
                if (day.length > 2 && day.length != 0) 
                {
                    $find('mdlWarning').show();
                    $('#WarningHeader').text('Warning');
                    $('#WarningMessage').text('Please enter date in DDMMYYYY or dd-Mon-YYYY format only.');
                    txtdate.value = "";
                    txtdate.focus();
                    return false;
                }
                if (month.length > 3 && month.length != 3) 
                {
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
                    if (flag == true)
                    {
                       
                       dateformatvalidator(mode);
//                       var currentDate=new Date();
//                       var date= currentDate .getDate() + "-" + cMONTHNAMES[currentDate .getMonth()] + "-" +currentDate .getFullYear();
//                       var difference = GetDateDifference(txtdate.value, date);
//                       if (difference.Days > 0)
//                        {
//                            $find('mdlWarning').show();
//                            $('#WarningHeader').text('Warning');
//                            $('#WarningMessage').text('You can not add date which is less than current date ');
//                            txtdate.value = "";
//                            txtdate.focus();
//                            return false ;
//                        }
                    }
                    else 
                    {
                       if (inyear < 1900)
                        {
                            $find('mdlWarning').show();
                            $('#WarningHeader').text('Warning');
                            $('#WarningMessage').text('You can not add date which is less than "01-Jan-1900" ');
                            txtdate.value = "";
                            txtdate.focus();
                            return false ;
                        }
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
//                 var currentDate=new Date();
//                 var date= currentDate .getDate() + "-" + cMONTHNAMES[currentDate .getMonth()] + "-" +currentDate .getFullYear();
//                 var difference = GetDateDifference(txtdate.value, date);
                 if (flag == true)
                   {
                      dateformatvalidator(mode);
//                       if (difference.Days > 0)
//                        {
//                            $find('mdlWarning').show();
//                            $('#WarningHeader').text('Warning');
//                            $('#WarningMessage').text('You can not add date which is less than current date ');
//                            txtdate.value = "";
//                            txtdate.focus();
//                            return false ;
//                        }
                    }
                    else 
                    {
                       if (inyear < 1900)
                        {
                            $find('mdlWarning').show();
                            $('#WarningHeader').text('Warning');
                            $('#WarningMessage').text('You can not add date which is less than "01-Jan-1900" ');
                            txtdate.value = "";
                            txtdate.focus();
                            return false ;
                        }
                    }
          return true ;  
   }
   
   
   function dateformatvalidator(mode){
        
          if (mode==1){
              var str ='ctl00_CPHLAMBDA_txtStatusStartDate';
                    }
         else if (mode==2) {
              var str ='ctl00_CPHLAMBDA_txtStatusEndDate';
             }
       if (document.getElementById('ctl00_CPHLAMBDA_txtStatusStartDate').value != ""&& document.getElementById('ctl00_CPHLAMBDA_txtStatusEndDate').value != "" ){
       var fromdate=document.getElementById('ctl00_CPHLAMBDA_txtStatusStartDate');
       var todate = document.getElementById('ctl00_CPHLAMBDA_txtStatusEndDate');
       var gap=GetDateDifference(fromdate.value, todate.value);
       if (gap.Days <= 0)
       {
           if (fromdate.value != todate.value)
           {
             msgalert('End Date Must Not Be Less Than Start Date !');
             document.getElementById(str).value = '';
             document.getElementById(str).focus();
             return false ;
            }
         }
       }
     }  
 
 function ddlRecruitingSourceSelectedIndexChange()
 {
    $('#<%= hdnReferenceSubject.ClientId %>').val('');
    if ($('#ctl00_CPHLAMBDA_ddlRecruitingSource option:selected').text() == "Word of Mouth")
    {
       $('#ctl00_CPHLAMBDA_trReference').css('display','');
    }
    else 
    {
      $('#ctl00_CPHLAMBDA_trReference').css('display','none');
    }    
 }
        function validateHabit() {
            var ddl1 = $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl02_ddlConmpStatus option:selected').text();
            var ddl2 = $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl03_ddlConmpStatus option:selected').text();
            var ddl3 = $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl04_ddlConmpStatus option:selected').text();
            var ddl4 = $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl05_ddlConmpStatus option:selected').text();
            var ddl5 = $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl06_ddlConmpStatus option:selected').text();
            var ddl6 = $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl07_ddlConmpStatus option:selected').text();
            var ddl7 = $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl08_ddlConmpStatus option:selected').text();
            var ddl8 = $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl09_ddlConmpStatus option:selected').text();
            var ddl9 = $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl10_ddlConmpStatus option:selected').text();

            $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl02_ddlConmpStatus').prop('disabled', false);
            $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl03_ddlConmpStatus').prop('disabled', false);
            $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl04_ddlConmpStatus').prop('disabled', false);
            $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl05_ddlConmpStatus').prop('disabled', false);
            $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl06_ddlConmpStatus').prop('disabled', false);
            $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl07_ddlConmpStatus').prop('disabled', false);
            $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl08_ddlConmpStatus').prop('disabled', false);
            $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl09_ddlConmpStatus').prop('disabled', false);
            $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl10_ddlConmpStatus').prop('disabled', false);

            if (ddl1 == "Never") {
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl02_txtConmpQuantity').prop('disabled', true);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl02_txtStartDate').prop('disabled', true);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl02_txtEndDate').prop('disabled', true);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl02_ddlConmpFrequency').prop('disabled', true);
            }
            if (ddl1 == "Currently") {
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl02_txtConmpQuantity').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl02_txtStartDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl02_txtEndDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl02_ddlConmpFrequency').prop('disabled', false);
            }

            if (ddl1 == "Stopped") {
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl02_txtConmpQuantity').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl02_txtStartDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl02_txtEndDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl02_ddlConmpFrequency').prop('disabled', false);
            }


            if (ddl2 == "Never") {
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl03_txtConmpQuantity').prop('disabled', true);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl03_txtStartDate').prop('disabled', true);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl03_txtEndDate').prop('disabled', true);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl03_ddlConmpFrequency').prop('disabled', true);
            }
            if (ddl2 == "Currently") {
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl03_txtConmpQuantity').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl03_txtStartDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl03_txtEndDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl03_ddlConmpFrequency').prop('disabled', false);
            }
            if (ddl2 == "Stopped") {
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl03_txtConmpQuantity').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl03_txtStartDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl03_txtEndDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl03_ddlConmpFrequency').prop('disabled', false);
            }

            if (ddl3 == "Never") {
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl04_txtConmpQuantity').prop('disabled', true);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl04_txtStartDate').prop('disabled', true);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl04_txtEndDate').prop('disabled', true);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl04_ddlConmpFrequency').prop('disabled', true);
            }
            if (ddl3 == "Currently") {
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl04_txtConmpQuantity').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl04_txtStartDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl04_txtEndDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl04_ddlConmpFrequency').prop('disabled', false);
            }

            if (ddl3 == "Stopped") {
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl04_txtConmpQuantity').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl04_txtStartDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl04_txtEndDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl04_ddlConmpFrequency').prop('disabled', false);
            }


            if (ddl4 == "Never") {
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl05_txtConmpQuantity').prop('disabled', true);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl05_txtStartDate').prop('disabled', true);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl05_txtEndDate').prop('disabled', true);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl05_ddlConmpFrequency').prop('disabled', true);
            }
            if (ddl4 == "Currently") {
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl05_txtConmpQuantity').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl05_txtStartDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl05_txtEndDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl05_ddlConmpFrequency').prop('disabled', false);
            }

            if (ddl4 == "Stopped") {
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl05_txtConmpQuantity').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl05_txtStartDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl05_txtEndDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl05_ddlConmpFrequency').prop('disabled', false);
            }



            if (ddl5 == "Never") {
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl06_txtConmpQuantity').prop('disabled', true);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl06_txtStartDate').prop('disabled', true);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl06_txtEndDate').prop('disabled', true);
                //$('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl06_ddlConmpFrequency').prop('disabled', true);
            }
            if (ddl5 == "Currently") {
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl06_txtConmpQuantity').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl06_txtStartDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl06_txtEndDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl06_ddlConmpFrequency').prop('disabled', false);
            }

            if (ddl5 == "Stopped") {
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl06_txtConmpQuantity').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl06_txtStartDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl06_txtEndDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl06_ddlConmpFrequency').prop('disabled', false);
            }


            if (ddl6 == "Never") {
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl07_txtConmpQuantity').prop('disabled', true);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl07_txtStartDate').prop('disabled', true);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl07_txtEndDate').prop('disabled', true);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl07_ddlConmpFrequency').prop('disabled', true);
            }
            if (ddl6 == "Currently") {
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl07_txtConmpQuantity').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl07_txtStartDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl07_txtEndDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl07_ddlConmpFrequency').prop('disabled', false);
            }

            if (ddl6 == "Stopped") {
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl07_txtConmpQuantity').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl07_txtStartDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl07_txtEndDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl07_ddlConmpFrequency').prop('disabled', false);
            }



            if (ddl7 == "Never") {
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl08_txtConmpQuantity').prop('disabled', true);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl08_txtStartDate').prop('disabled', true);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl08_txtEndDate').prop('disabled', true);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl08_ddlConmpFrequency').prop('disabled', true);
            }
            if (ddl7 == "Currently") {
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl08_txtConmpQuantity').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl08_txtStartDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl08_txtEndDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl08_ddlConmpFrequency').prop('disabled', false);
            }

            if (ddl7 == "Stopped") {
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl08_txtConmpQuantity').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl08_txtStartDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl08_txtEndDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl08_ddlConmpFrequency').prop('disabled', false);
            }



            if (ddl8 == "Never") {
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl09_txtConmpQuantity').prop('disabled', true);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl09_txtStartDate').prop('disabled', true);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl09_txtEndDate').prop('disabled', true);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl09_ddlConmpFrequency').prop('disabled', true);
            }
            if (ddl8 == "Currently") {
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl09_txtConmpQuantity').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl09_txtStartDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl09_txtEndDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl09_ddlConmpFrequency').prop('disabled', false);
            }


            if (ddl8 == "Stopped") {
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl09_txtConmpQuantity').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl09_txtStartDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl09_txtEndDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl09_ddlConmpFrequency').prop('disabled', false);
            }



            if (ddl9 == "Never") {
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl10_txtConmpQuantity').prop('disabled', true);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl10_txtStartDate').prop('disabled', true);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl10_txtEndDate').prop('disabled', true);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl10_ddlConmpFrequency').prop('disabled', true);
            }
            if (ddl9 == "Currently") {
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl10_txtConmpQuantity').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl10_txtStartDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl10_txtEndDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl10_ddlConmpFrequency').prop('disabled', false);
            }

            if (ddl9 == "Stopped") {
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl10_txtConmpQuantity').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl10_txtStartDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl10_txtEndDate').prop('disabled', false);
                $('#ctl00_CPHLAMBDA_grdGeneralConmp_ctl10_ddlConmpFrequency').prop('disabled', false);
            }
        }

    </script>

</asp:Content>
