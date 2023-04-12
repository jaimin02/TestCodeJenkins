<%@ Page Language="VB" MasterPageFile="~/ECTDMasterPage.master" CodeFile="frmSubjectPIFMst_New.aspx.vb" Inherits="frmSubjectPIFMst_New" AutoEventWireup="true" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <style type="text/css">
        .AuditControl, .EditControl  {
            cursor:pointer;
        }

    </style>
    <%--    <link type="text/css" rel="stylesheet" href="App_Themes/CDMS.css" />
    <link type="text/css" rel="Stylesheet" href="App_Themes/jquery.ui.theme_cal.css" />--%>
   
   <%-- <script src="Script/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="Script/jquery-2.1.4.js" type="text/javascript"></script>--%>
    
    <script src="Script/jquery-1.11.3.min.js"  type="text/javascript" ></script>
    <script src="Script/jquery-ui.js" type="text/javascript"></script>

    <script src="Script/General.js" type="text/javascript"></script>

    <%--<link href="App_Themes/StyleBlue/UI_Theme/jquery-ui.min.css" rel="stylesheet" />--%>

    <script src="Script/jquery.dataTables.min.js" type="text/javascript"></script>

    <script type="text/javascript" src="Script/AutoComplete.js" language="javascript"></script>

    <div style="text-align: center; overflow: auto; height: 500px; display:none;" id="HeaderLabel"
        runat="server" align="center">
        <table style="width: 100%" align="center" id="MainContentTable" runat="server">
            <tbody>
                <tr align="center">
                    <td align="center" width="100%" colspan="3">
                        <asp:UpdatePanel ID="UpPlaceHolder" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                            <ContentTemplate>
                                <asp:Panel ID="PnlPlaceMedex" runat="server" Width="100%" ScrollBars="Auto">
                                    <asp:PlaceHolder ID="PlaceMedEx" runat="server" EnableViewState="true"></asp:PlaceHolder>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>

    <div id="divHeaderPDF" style="display:none;" runat="server" align="left" enableviewstate="false">
        <table style="padding: 2px; margin: auto; border: solid 1px black; width: 91%; margin: 0PX 20px 20px 20px; font-family: Verdana;"
            align="left">
            <tr>
                <td>
                    <table>
                        <tr>
                            <td colspan="4" style="vertical-align: top;">
                                <table>
                                    <tr>

                                        <td style="vertical-align: top; font-size: large; font-weight: 900">Lambda Therapeutic Research
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; font-size: larger; font-weight: 700">
                                            <span id="spnBaBe" runat="server"></span>
                                        </td>
                                    </tr>
                                    <tr>

                                        <td style="vertical-align: top; font-size: medium; font-weight: 600; ">Personal Information Form
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; font-size: larger; font-weight: 700">
                                            <span id="Span1" runat="server"></span>
                                        </td>
                                    </tr>
                                   <tr>
                                        <td align="left" >
                                            <span id="SpnSubject" runat="server" enableviewstate="false">Volunteer Id: </span>
                                             <asp:Label ID="lblSubjectNo" runat="server" Text="_" EnableViewState="false"></asp:Label>
                                        </td>

                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <%-- <tr>
                            <td align="right">Project No:
                            </td>
                            <td style="border: thin solid #000000;">
                                <asp:Label ID="lblProjectNo" runat="server" Text="_" EnableViewState="false"></asp:Label>
                            </td>
                            <td id="tdSiteName" align="right" runat="server" enableviewstate="false">
                                <span id="SpnSite" runat="server" enableviewstate="false">Site Id: </span>
                            </td>
                            <td id="tdSiteId" runat="server" style="border: thin solid #000000;" enableviewstate="false">
                                <asp:Label ID="lblSiteNo" runat="server" Text="_" EnableViewState="false"></asp:Label>
                            </td>
                        </tr>
                        <tr id="trProtocol" runat="server" enableviewstate="false">
                            <td align="right">
                                <span id="Span1" runat="server" enableviewstate="false">Protocol No: </span>
                            </td>
                            <td style="border: thin solid #000000;">
                                <asp:Label ID="LblProtocol" runat="server" Text="_" EnableViewState="false"></asp:Label>
                            </td>
                        </tr>--%>
                        <%--<tr>
                            <td align="right">
                                <span id="SpnSubject" runat="server" enableviewstate="false">Volunteer Id: </span>
                            </td>
                            <td style="border: thin solid #000000;">
                                <asp:Label ID="lblSubjectNo" runat="server" Text="_" EnableViewState="false"></asp:Label>
                            </td>
                         <%--   <td align="right">
                                <span id="SpnSubjectInit" runat="server" enableviewstate="false">Subject Initials:
                                </span>
                            </td>--%>
                        <%-- <td style="border: thin solid #000000;">
                                <asp:Label ID="lblSubjectName" runat="server" Text="_" EnableViewState="false"></asp:Label>
                            </td>--%>
                        <%--</tr>--%>
                    </table>
                </td>
                <td valign="top">
                    <img alt="" runat="server" id="Img1" src="images/lambda_logo.jpg" enableviewstate="false" />
                </td>
            </tr>
        </table>
    </div>

    <img alt="" runat="server" id="ImgLogo" src="images/lambda_logo.jpg" style="display:none" enableviewstate="false" />
    <asp:UpdatePanel ID="UPPanelQc" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="display:none; left: 424px; width: 700px; top: 367px; height: 440px; text-align: left"
                id="divQCDtl" class="DIVSTYLE2" runat="server">
                <table width="100%">
                    <tbody>
                        <tr>
                            <td colspan="2" style="text-align: center;" class="Label ">
                                <asp:Label ID="Label1" Text="QC Comments" Font-Size="Larger" CssClass="Label " runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <%--  <td style="width: 14%" class="Label" align="left">
                                        </td>--%>
                            <td colspan="2" style="text-align: center;">
                                <asp:RadioButtonList ID="RBLQCFlag" runat="server" Visible="False" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="Y">Approve</asp:ListItem>
                                    <asp:ListItem Value="N">Reject</asp:ListItem>
                                    <asp:ListItem Selected="True" Value="F">Feedback</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="Label" align="left" colspan="2">
                                <asp:Label ID="lblResponse" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 30%; text-align: right;" class="Label">Remarks/Response :
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtQCRemarks" runat="server" CssClass="textBox" Width="229px" Height="46px"
                                    TextMode="MultiLine" MaxLength="1000"></asp:TextBox>
                                <asp:Label ID="lblcnt" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <%--<tr>
                    <td style="text-align: right;" class="Label">To :
                    </td>
                    <td style="text-align: left">
                        <asp:TextBox ID="txtToEmailId" runat="server" CssClass="textBox" Width="230px" MaxLength="500"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;" class="Label">CC :
                    </td>
                    <td style="text-align: left">
                        <asp:TextBox ID="txtCCEmailId" runat="server" CssClass="textBox" Width="231px" MaxLength="500"
                            Height="37px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>--%>
                        <tr>
                            <td style="text-align: center;" colspan="2">
                                <asp:Button Style="width: 12%" ID="BtnQCSave" runat="server" Text="Save" CssClass="btn btnsave"
                                    ToolTip="Save" OnClientClick="return ValidationQC();" OnClick="BtnQCSave_Click" />

                                <input type="button" id="btnExitQC" class="btn btnexit" onclick="QCDivShowHide('H');"
                                    value="Close" runat="server" title="Close" style="width: 12%;" />
                            </td>
                        </tr>
                    </tbody>
                </table>
                <strong style="text-align: center;">
                    <br />
                    QC Comments History </strong>
                <br />
                <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                <br />

                <asp:Panel ID="Panel1" runat="server" Width="100%" ScrollBars="Auto">
                    <asp:GridView ID="GVQCDtl" runat="server" Font-Size="Small" SkinID="grdViewSmlAutoSize"
                        BorderColor="Peru" AutoGenerateColumns="False" Width="100%">
                        <Columns>
                            <asp:BoundField DataField="nSubjectMasterQCNo" HeaderText="nSubjectMasterQCNo" />
                            <asp:BoundField DataField="vSubjectId" HeaderText="vSubjectId" />
                            <asp:BoundField DataField="iTranNo" HeaderText="Sr. No.">
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FullName" HeaderText="Subject">
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="vQCComment" HeaderText="QC Comments" />
                            <asp:BoundField DataField="cQCFlag" HeaderText="QC" />
                            <asp:BoundField DataField="vQCGivenBy" HeaderText="QC BY">
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                            <%--  <asp:TemplateField HeaderText="QC Date">
                                <ItemTemplate>
                                    <asp:Label  ID="qcdate" runat="server"  Text='<%# Eval("dQCGivenOn")%>' >
                                    </asp:Label> 
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:BoundField DataField="ActualTIME" HeaderText="QC Date" DataFormatString="{0:dd-MMM-yyyy HH:mm}"
                                HtmlEncode="False">
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="vResponse" HeaderText="Response" />
                            <asp:BoundField DataField="vResponseGivenBy" HeaderText="Response BY">
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ActualTIME2" DataFormatString="{0:dd-MMM-yyyy HH:mm}" HeaderText="Response Date"
                                HtmlEncode="False">
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Response">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkResponse" runat="server" Text="Response" ToolTip="Response" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UPPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button runat="server" ID="btndefault" Style="display:none" />
            <table style="width:60% !important;">
                <tr>
                    <td style="width: 100%;">
                        <fieldset id="fSubjectSearch" class="FieldSetBox" style="width: 97.5%;">
                            <legend class="LegendText" style="font-size: 14px !important;">Subject Search</legend>
                            <table style="width: 100%;">
                                <tr>
                                    <td style="text-align: right; width: 20%;" class="LabelText">Search Subject:
                                    </td>
                                    <td style="text-align: left; width: 50%">
                                        <asp:TextBox ID="txtSubject" TabIndex="2" runat="server" CssClass="textBox" Style="width: 100%;" onkeydown="return (event.keyCode!=13)" />
                                        <asp:Button Style="display:none" TabIndex="1" ID="btnEdit" runat="server" Text=" Edit "
                                            ToolTip="Edit" CssClass="btn btnsave" />
                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" ServicePath="AutoComplete.asmx"
                                            OnClientShowing="ClientPopulated" CompletionSetCount="10" OnClientItemSelected="OnSelected"
                                            UseContextKey="True" MinimumPrefixLength="1" ServiceMethod="GetSubjectCompletionList_NotRejected_BlockPeriod"
                                            CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                            CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1"
                                            TargetControlID="txtSubject">
                                        </cc1:AutoCompleteExtender>
                                        <asp:HiddenField ID="HSubjectId" runat="server"></asp:HiddenField>
                                    </td>
                                    <td style="text-align: right;" class="LabelText">
                                        <asp:Button ID="BtnNew" TabIndex="2" runat="server" Text="New" CssClass="btn btnmsr"
                                            OnClientClick="fnNew();" />
                                        <asp:Button ID="btnMSR" TabIndex="3" runat="server" Text="MSR" ToolTip=" Medical Screening Report"
                                            CssClass="btn btnadd" OnClientClick="return ValidationMSR();" />
                                        <input id="btnQC" class="btn btnnew" tabindex="4" onclick="return QCDivShowHide('S');"
                                            type="button" value="QC" title="QC Report" runat="server" />


                                        <asp:Button ID="btnExit" runat="server" Text="Exit" ToolTip="Exit" CssClass="btn btnclose"
                                            OnClientClick="return msgconfirmalert('Are You Sure You Want To Exit?',this);" />

                                        <asp:Button ID="btngenerate" runat="server" Text="" ToolTip="PDF Generate" CssClass="btn btnpdf" 
                                            OnClientClick="return validateForm();" />
                                        <asp:HiddenField ID="HFHeaderLabel" runat="server" />
                                        <asp:HiddenField ID="HFHeaderPDF" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <div style="width: 90%; margin: auto;">
                <div id="tabs" style="width: 95%; font-size: 12px !important;">

                    <ul>
                        <li><b><a href="#PersionalInfo">Personal Information</a></b></li>
                        <li style="display:none;" id="LiFemaleInfo"><b><a href="#FemaleInfo">Female Information</a></b></li>
                        <li><b><a href="#ContactInfo">Contact Information</a><b></li>
                    </ul>
                    <div id="PersionalInfo" style="width: 95%;">
                        <table style="width: 100%;">
                            <%--  <tr>
                                <td style="width: 100%;" colspan="2">
                                    <fieldset id="fSubjectSearch" class="FieldSetBox" style="width: 97.5%;">
                                        <legend class="LegendText" style="font-size: 14px !important;">Subject Search</legend>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="text-align: right; width: 11.5%;" class="LabelText">Search Subject:
                                                </td>
                                                <td style="text-align: left; width: 50%">
                                                    <asp:TextBox ID="txtSubject" TabIndex="2" runat="server" CssClass="textBox" Style="width: 100%;" onkeydown="return (event.keyCode!=13)" />
                                                    <asp:Button Style="display:none" TabIndex="1" ID="btnEdit" runat="server" Text=" Edit "
                                                        ToolTip="Edit" CssClass="button" />
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" ServicePath="AutoComplete.asmx"
                                                        OnClientShowing="ClientPopulated" CompletionSetCount="10" OnClientItemSelected="OnSelected"
                                                        UseContextKey="True" MinimumPrefixLength="1" ServiceMethod="GetSubjectCompletionList_NotRejected"
                                                        CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                        CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1"
                                                        TargetControlID="txtSubject">
                                                    </cc1:AutoCompleteExtender>
                                                    <asp:HiddenField ID="HSubjectId" runat="server"></asp:HiddenField>
                                                </td>
                                                <td style="text-align: right;" class="LabelText">
                                                    <asp:Button ID="BtnNew" TabIndex="2" runat="server" Text="New" CssClass="button"
                                                        OnClientClick="fnNew();" />
                                                    <asp:Button ID="btnMSR" TabIndex="3" runat="server" Text="MSR" ToolTip=" Medical Screening Report"
                                                        CssClass="button" OnClientClick="return ValidationMSR();" />
                                                    <input id="btnQC" class="button" tabindex="4" onclick="return QCDivShowHide('S');"
                                                        type="button" value="QC" title="QC Report" runat="server" />

                                                    
                                                    <asp:Button ID="btnExit" runat="server" Text="Exit" ToolTip="Exit" CssClass="button"
                                                        OnClientClick="return confirm('Are You Sure You Want To Exit?');" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>--%>
                            <tr>
                                <td style="width: 75%;">
                                    <fieldset id="fPersionalIfo" class="FieldSetBox" style="width: 97.5%;">
                                        <legend class="LegendText" style="font-size: 14px !important;">Personal Information</legend>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="text-align: right;" class="LabelText">Last Name*:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox ID="txtvSurName" onblur="SetInitial();" TabIndex="5" runat="server"
                                                        CssClass="EntryControl" MaxLength="50" onkeydown="return (event.keyCode!=13)" />
                                                    <asp:Image ID="imgEditvSurName" ToolTip="Edit" CssClass="EditControl" runat="server" cName="Intials" Style="display:none;"
                                                        ImageUrl="images/Edit_Small.png" />
                                                    <asp:Image ID="imgAuditvSurName" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ImageUrl="images/Audit_Small.png" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="tabPanelOne"
                                                        SetFocusOnError="True" Display="Dynamic" ErrorMessage="*" ControlToValidate="txtvSurName">*</asp:RequiredFieldValidator>
                                                </td>
                                                <td style="text-align: right;" class="LabelText">First Name*:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox ID="txtvFirstName" TabIndex="6" runat="server" onblur="SetInitial();"
                                                        MaxLength="152" CssClass="EntryControl" onkeydown="return (event.keyCode!=13)" />
                                                    <asp:Image ID="imgEditvFirstName" ToolTip="Edit" CssClass="EditControl" cName="Intials" Style="display:none;"
                                                        runat="server" ImageUrl="images/Edit_Small.png" />
                                                    <asp:Image ID="imgAuditvFirstName" ToolTip="Audit Trail" CssClass="AuditControl" Style="display:none;"
                                                        runat="server" ImageUrl="images/Audit_Small.png" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="tabPanelOne"
                                                        SetFocusOnError="True" Display="Dynamic" ErrorMessage="*" ControlToValidate="txtvFirstName">*</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" class="LabelText">Middle Name:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox ID="txtvMiddleName" onblur="SetInitial();" MaxLength="50" TabIndex="7"
                                                        runat="server" CssClass="EntryControl" onkeydown="return (event.keyCode!=13)" />
                                                    <asp:Image ID="imgEditvMiddleName" ToolTip="Edit" CssClass="EditControl" cName="Intials" runat="server" Style="display:none;"
                                                        ImageUrl="images/Edit_Small.png" />
                                                    <asp:Image ID="imgAuditvMiddleName" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ImageUrl="images/Audit_Small.png" />
                                                </td>
                                                <td style="text-align: right;" class="LabelText">Initials:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox ID="txtInitials" Enabled="false" MaxLength="5" runat="server" CssClass="EntryControl" ReadOnly="true" />
                                                    <asp:HiddenField ID="HFInitials" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" class="LabelText">Date Of Birth *:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox ID="txtdBirthDate" TabIndex="8" onchange="DateConvert_Age(this.value,this,'Y')" runat="server" CssClass="EntryControl"
                                                        Style="width: 100px;" onkeydown="return (event.keyCode!=13)" />
                                                    <asp:Image ID="imgEditdBirthDate" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                    <asp:Image ID="imgAuditdBirthDate" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ImageUrl="images/Audit_Small.png" />
                                                    <asp:Button ID="btnCheckDuplicateSubject" Style="display:none;" runat="server" Text="Check Duplicate Subject"
                                                        CssClass="btn btnew" />
                                                    <cc1:CalendarExtender ID="caltxtDBirthDate" runat="server" TargetControlID="txtdBirthDate"
                                                        Format="dd-MMM-yyyy">
                                                    </cc1:CalendarExtender>
                                                </td>
                                                <td style="text-align: right;" class="LabelText">Date of Enrollment *:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox ID="txtdEnrollmentDate" onchange="DateConvert_Age(this.value,this,'Y')" TabIndex="9" runat="server" CssClass="EntryControl"
                                                        Style="width: 100px;" onkeydown="return (event.keyCode!=13)"></asp:TextBox>
                                                    <asp:Image ID="imgEditdEnrollmentDate" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                    <asp:Image ID="imgAuditdEnrollmentDate" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ImageUrl="images/Audit_Small.png" />
                                                    <cc1:CalendarExtender ID="caldoer" runat="server" TargetControlID="txtdEnrollmentDate"
                                                        Format="dd-MMM-yyyy">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" class="LabelText">Ref. Subject Code:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox ID="txtvRefSubjectId" TabIndex="10" MaxLength="50" runat="server" CssClass="EntryControl" onkeydown="return (event.keyCode!=13)" />
                                                    <asp:Image ID="imgEditvRefSubjectId" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                    <asp:Image ID="imgAuditvRefSubjectId" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ImageUrl="images/Audit_Small.png" />
                                                </td>
                                                <td style="text-align: right;" class="LabelText">Age:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox Enabled="False" ID="txtAge" MaxLength="3" runat="server" Style="width: 30%;"></asp:TextBox>
                                                    <span class="LabelText">(Years)</span>
                                                    <asp:HiddenField ID="HFAge" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" class="LabelText">Sex:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:DropDownList ID="ddlcSex" CssClass="EntryControl" TabIndex="11" runat="server"
                                                        onchange="openFemaleInfo()">
                                                        <asp:ListItem Value="M" Selected="True">Male</asp:ListItem>
                                                        <asp:ListItem Value="F">Female</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Image ID="imgEditcSex" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                    <asp:Image ID="imgAuditcSex" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ImageUrl="images/Audit_Small.png" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                                <td style="width: 20%;">
                                    <fieldset id="fPhotograph" class="FieldSetBox">
                                        <legend class="LegendText">Photograph</legend>
                                        <table cellpadding="2"  width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Image ID="Image1" runat="server" Style="height: 144px;" ImageUrl="~/images/NotFound.gif"></asp:Image>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="width: 100%;">
                                    <fieldset id="fMesurement" class="FieldSetBox">
                                        <legend class="LegendText" style="font-size: 14px !important;">Measurements / Demographics</legend>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="width: 60%">
                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td style="text-align: right; width: 16%;" class="LabelText">Height(cm)*:
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:TextBox ID="txtnHeight" MaxLength="6" onblur="calcBMI();" TabIndex="12" Style="width: 60px;"
                                                                    runat="server" CssClass="EntryControl" onkeypress="return isNumberKey(event)" onkeydown="return (event.keyCode!=13)" />

                                                                <asp:Image ID="imgEditnHeight" ToolTip="Edit" CssClass="EditControl" cName="BMI" runat="server"
                                                                    ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                                <asp:Image ID="imgAuditnHeight" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                                    ImageUrl="images/Audit_Small.png" />
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="tabPanelOne"
                                                                    SetFocusOnError="True" Display="Dynamic" ErrorMessage="*" ControlToValidate="txtnHeight">*</asp:RequiredFieldValidator>
                                                                <%--<asp:RegularExpressionValidator ID="revheight" ControlToValidate="txtnHeight"  ValidationExpression="" 
                                                                --%></td>
                                                            <td style="text-align: right; width: 29%;" class="LabelText">BMI:
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:TextBox ID="txtbmi" TabIndex="10" MaxLength="5" Style="width: 60px;" runat="server"
                                                                    Enabled="false" onkeydown="return (event.keyCode!=13)" />(kg/m2)
                                                                <asp:HiddenField ID="HfBMI" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right;" class="LabelText">Weight(kg)*:
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:TextBox ID="txtnWeight" onBlur="calcBMI();" TabIndex="13" MaxLength="6" Style="width: 60px;"
                                                                    runat="server" CssClass="EntryControl" onkeypress="return isNumberKey(event)" onkeydown="return (event.keyCode!=13)" />

                                                                <asp:Image ID="imgEditnWeight" ToolTip="Edit" CssClass="EditControl" cName="BMI" runat="server" Style="display:none;"
                                                                    ImageUrl="images/Edit_Small.png" />
                                                                <asp:Image ID="imgAuditnWeight" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                                    ImageUrl="images/Audit_Small.png" />
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ValidationGroup="tabPanelOne"
                                                                    SetFocusOnError="True" Display="Dynamic" ErrorMessage="*" ControlToValidate="txtnWeight">*</asp:RequiredFieldValidator>
                                                            </td>
                                                            <td style="text-align: right;" class="LabelText">Marital Status:
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:DropDownList ID="ddlcMaritalStatus" CssClass="EntryControl" TabIndex="14" runat="server">
                                                                    <asp:ListItem Value="S">Single</asp:ListItem>
                                                                    <asp:ListItem Value="M">Married</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:Image ID="imgEditcMaritalStatus" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                                <asp:Image ID="imgAuditcMaritalStatus" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                                    ImageUrl="images/Audit_Small.png" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right;" class="LabelText">Food Habit:
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:DropDownList ID="ddlcFoodHabit" CssClass="EntryControl" TabIndex="15" runat="server">
                                                                    <asp:ListItem Selected="True">Vegetarian</asp:ListItem>
                                                                    <asp:ListItem>Non-Vegetarian</asp:ListItem>
                                                                    <asp:ListItem>Eggetarian</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:Image ID="imgEditcFoodHabit" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                                <asp:Image ID="imgAuditcFoodHabit" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                                    ImageUrl="images/Audit_Small.png" />
                                                            </td>
                                                            <td style="text-align: right;" class="LabelText">Blood Group:
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:DropDownList ID="ddlcBloodGroup" CssClass="EntryControl" TabIndex="16" Style="width: 50px;"
                                                                    runat="server">
                                                                    <asp:ListItem></asp:ListItem>
                                                                    <asp:ListItem Value="A">A</asp:ListItem>
                                                                    <asp:ListItem Value="B">B</asp:ListItem>
                                                                    <asp:ListItem Value="AB">AB</asp:ListItem>
                                                                    <asp:ListItem Value="O">O</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:HiddenField ID="hdncBloodGroup" runat="server" />
                                                                <asp:DropDownList ID="ddlcRh" TabIndex="17" CssClass="EntryControl" Style="width: 60px;"
                                                                    runat="server">
                                                                    <asp:ListItem></asp:ListItem>
                                                                    <asp:ListItem Value="P">+Ve</asp:ListItem>
                                                                    <asp:ListItem Value="N">-Ve</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:HiddenField ID="hdncRh" runat="server" />
                                                                <asp:Image ID="imgEditcBloodGroup" ToolTip="Edit" CssClass="EditControl" runat="server" ctype="BloodGroup"
                                                                    ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                                <asp:Image ID="imgAuditcBloodGroup" ToolTip="Audit Trail" CssClass="AuditControl" runat="server"
                                                                    ImageUrl="images/Audit_Small.png" Style="display:none;" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right;" class="LabelText">Occupation:
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:TextBox ID="txtvOccupation" Style="width: 150px;" TabIndex="18" MaxLength="50"
                                                                    runat="server" CssClass="EntryControl" onkeydown="return (event.keyCode!=13)" />
                                                                <asp:Image ID="imgEditvOccupation" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                                <asp:Image ID="imgAuditvOccupation" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                                    ImageUrl="images/Audit_Small.png" />
                                                            </td>
                                                            <td style="text-align: right;" class="LabelText">Educational Qualification:
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:TextBox ID="txtvEducationQualification" Style="width: 150px;" MaxLength="50" TabIndex="19"
                                                                    runat="server" CssClass="EntryControl" onkeydown="return (event.keyCode!=13)"></asp:TextBox>
                                                                <asp:Image ID="imgEditvEducationQualification" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                                <asp:Image ID="imgAuditvEducationQualification" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                                    ImageUrl="images/Audit_Small.png" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right;" class="LabelText">Remarks:
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:TextBox ID="txtvDemographicRemarks" Style="width: 150px;" TabIndex="18" MaxLength="500" TextMode="MultiLine"
                                                                    runat="server" CssClass="EntryControl" onblur="characterlimit(this)"   />
                                                                <asp:Image ID="imgEditvDemographicRemarks" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                                <asp:Image ID="imgAuditvDemographicRemarks" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                                    ImageUrl="images/Audit_Small.png" />
                                                            </td>
                                                            <td style="text-align: right;" class="LabelText">Population *:
                                                            </td>
                                                            <td style="text-align: left;">
                                                                 <asp:DropDownList ID="ddlvPopulation" TabIndex="17" CssClass="EntryControl" Style="width: 150px;" runat="server">
                                                                </asp:DropDownList>
                                                                <asp:Image ID="imgEditvPopulation" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                                <asp:Image ID="imgAuditvPopulation" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                                    ImageUrl="images/Audit_Small.png" />
                                                            </td>

                                                        </tr>
                                                          
                                                    </table>
                                                </td>
                                                <td style="width: 28%; vertical-align: top;">
                                                    <fieldset id="fICfLang" class="FieldSetBox">
                                                        <legend class="LegendText" style="font-size: 14px !important;">ICF Required In Language</legend>
                                                        <div style="text-align: right;">
                                                            <asp:Image ID="imgEditvICFLanguageCodeId" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                            <asp:Image ID="imgAuditvICFLanguageCodeId" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                                ImageUrl="images/Audit_Small.png" />
                                                        </div>
                                                        <div id="divSubjectLanguage" style="width: 100%; overflow: auto; height: 100px;">
                                                            <asp:CheckBoxList ID="chkvICFLanguageCodeId" TabIndex="20" runat="server" CssClass="checkboxlist ConControl"
                                                                RepeatColumns="3" RepeatDirection="Horizontal">
                                                            </asp:CheckBoxList>
                                                        </div>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="width: 100%;">
                                    <fieldset id="fphotoproof" class="FieldSetBox">
                                        <legend class="LegendText" style="font-size: 14px !important;">Proof To Be Attached</legend>
                                        <asp:UpdatePanel ID="upAttach" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td style="text-align: right;" class="LabelText" colspan="2">
                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <td style="text-align: right; width: 11%">Attachments:
                                                                    </td>
                                                                    <td style="text-align: left;">
                                                                        <asp:DropDownList ID="ddlProofType" TabIndex="21" onchange="funCheckOthers();" runat="server">
                                                                            <asp:ListItem>Select</asp:ListItem>
                                                                            <asp:ListItem>School Leaving certificate</asp:ListItem>
                                                                            <asp:ListItem>Birth Certificate</asp:ListItem>
                                                                            <asp:ListItem>Driving Licence</asp:ListItem>
                                                                            <asp:ListItem>Election Card</asp:ListItem>
                                                                            <asp:ListItem>Institutional I-Card</asp:ListItem>
                                                                            <asp:ListItem>Ration Card</asp:ListItem>
                                                                            <asp:ListItem>Pan Card</asp:ListItem>
                                                                            <asp:ListItem>Aadhaar Card</asp:ListItem>
                                                                            <asp:ListItem>Passport</asp:ListItem>
                                                                            <asp:ListItem>Others</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <span class="LabelText" id="ifOther" style="vertical-align: top; display:none;">If
                                                                            other:</span>
                                                                        <asp:TextBox ID="txtAttach" Style="vertical-align: top; display:none;"  onkeyup="characterforAttach(this)"  runat="server"></asp:TextBox>
                                                                        <asp:FileUpload ID="FlAttachment" Style="width: 175px; vertical-align: top;" runat="server"></asp:FileUpload>
                                                                        <asp:Button ID="btnAttach" TabIndex="22" Style="vertical-align: top;" runat="server"
                                                                            Text="Attach" OnClientClick="return funAttachDoc();"
                                                                            ToolTip="Attach Document" />
                                                                        <asp:HiddenField runat="server" ID="HDSubjectProofDetails" />
                                                                        <asp:Button ID="btnSaveProof" Text="SaveProof" runat="server" Style="display:none" />
                                                                        <asp:Button ID="btnBindProof" Text="BindProof" runat="server" Style="display:none" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: left; width: 30%;">


                                                            <fieldset id="fAttachment" style="width: 93.6%;" class="FieldSetBox">
                                                                <legend class="LegendText">Attachment Details</legend>
                                                                <div style="text-align: right;">
                                                                    <asp:Image ID="Image36" ToolTip="Edit" cName="AttachDoc" CssClass="EditControl" runat="server"
                                                                        ImageUrl="images/Edit_Small.png" />
                                                                    <%--<asp:ImageButton ID="Image36" ToolTip="Edit" cName="AttachDoc" CssClass="EditControl" runat="server"
                                                                        ImageUrl="images/Edit_Small.png" Enabled ="true" />--%>
                                                                    <asp:Image ID="Image37" ToolTip="Audit Trail" CssClass="AuditControl" runat="server"
                                                                        ImageUrl="images/Audit_Small.png" />
                                                                </div>

                                                                <div style="width: 90%; height: 277px; margin: auto; overflow: auto;">
                                                                    <asp:GridView ID="GVSubjectProof" runat="server" SkinID="grdViewSmlAutoSize" Style="width: 100%;"
                                                                        AutoGenerateColumns="False" CssClass="CoAttach">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="nSubjectProofNo" HeaderText="SubjectProofNo" />
                                                                            <asp:BoundField DataField="vSubjectId" HeaderText="SubjectId" />
                                                                            <asp:BoundField DataField="iTranNo" HeaderText="TranNo" />
                                                                            <asp:BoundField DataField="vProofType" HeaderText="ProofType" />
                                                                            <asp:BoundField DataField="vProofPath" HeaderText="ProofPath" />
                                                                            <asp:BoundField DataField="iModifyBy" HeaderText="ModifyBy" />
                                                                            <asp:BoundField DataField="cStatusIndi" HeaderText="Status" />
                                                                            <asp:TemplateField HeaderText="Attachment">
                                                                                <ItemTemplate>
                                                                                    <asp:HyperLink ID="hlnkFile" runat="server" Target="_blank" Text='<%# Eval("vProofPath") %>'>
                                                                                    </asp:HyperLink>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Delete">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="imgbtnDelete" runat="server" ImageUrl="~/Images/delete_small.png"
                                                                                        OnClientClick="return confirm('Are You Sure You Want To Delete?')" ToolTip="Delete"
                                                                                        CssClass="EntryControl" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </div>

                                                            </fieldset>

                                                        </td>
                                                        <td style="width: 65%;">
                                                            <fieldset id="fSmoking" class="FieldSetBox">
                                                                <legend class="LegendText" style="font-size: 14px !important;">Smoking/chewing/alcohol History:</legend>
                                                                <div style="text-align: right;">
                                                                    <asp:Image ID="Image38" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png"
                                                                        cType="Consumption" />
                                                                    <asp:Image ID="ImgUpdateHabits" ToolTip="Audit Trail" runat="server" CssClass="AuditControl"
                                                                        ImageUrl="images/Audit_Small.png" cType="Consumption" />
                                                                </div>
                                                                <div style="width: 98%; margin: auto; height: 260px; overflow: auto;">
                                                                    <asp:GridView ID="GVHabits" TabIndex="24" runat="server" AutoGenerateColumns="False"
                                                                        SkinID="grdViewSmlAutoSize" Style="width: 100%;">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="nSubjectHabitDetailsNo" HeaderText="SubjectHabitDetailsNo"></asp:BoundField>
                                                                            <asp:BoundField DataField="vSubjectId" HeaderText="SubjectId" />
                                                                            <asp:BoundField DataField="iScreenId" HeaderText="ScreenId" />
                                                                            <asp:BoundField DataField="vHabitId" HeaderText="HabitId" />
                                                                            <asp:BoundField DataField="vHabitDetails" HeaderText="Habit Details" />
                                                                            <asp:TemplateField HeaderText="Habit">
                                                                                <ItemTemplate>
                                                                                    <asp:DropDownList TabIndex="24" ID="ddlHebitType" runat="server" CssClass="LabelText EntryControl"
                                                                                        SelectedValue='<%# Bind("cHabitFlag") %>' Width="92px">
                                                                                        <asp:ListItem Selected="True" Value="N">Never</asp:ListItem>
                                                                                        <asp:ListItem Value="C">Current</asp:ListItem>
                                                                                        <asp:ListItem Value="P">Previous</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Consumption Detail">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox TabIndex="24" ID="txtConsumption" runat="server" CssClass="LabelText ConEntryControl"
                                                                                        Text='<%# Bind("vRemarks") %>' Width="200px" Style="text-align: left;" Enabled="false"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="If Previous, stopped since">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox TabIndex="24" ID="txtEndDate" Width="200px" runat="server" CssClass="LabelText ConEntryControl"
                                                                                        Text='<%# Bind("vHabitHistory") %>'
                                                                                        Style="text-align: left;" Enabled="false"></asp:TextBox>
                                                                                    <%--<cc1:CalendarExtender ID="calEndDate" runat="server" TargetControlID="txtEndDate"
                                                                                        Format="dd-MMM-yyyy">
                                                                                    </cc1:CalendarExtender>--%>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="cHabitFlag" HeaderText="HabitFlag" />
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </div>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:HiddenField ID="HFAttachstatus" runat="server" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btnAttach" />

                                                <asp:AsyncPostBackTrigger ControlID="btnSaveProof" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="BtnQCSave" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                        <%--        </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnAttach" />
                    </Triggers>
                </asp:UpdatePanel>--%>
                    </div>
                    <div id="FemaleInfo" style="width: 95%;">
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 35%">
                                    <fieldset id="fMensrualPeriod" class="FieldSetBox" style="width: 94.5%;">
                                        <legend class="LegendText" style="font-size: 14px !important;">Last Menstrual Period</legend>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="text-align: right;" class="LabelText">Date:
                                                </td>
                                                <td style="text-align: left; width: 50%">
                                                    <asp:TextBox Style="width: 100px;" ID="txtdLastMenstrualDate" onchange="DateConvert_Age(this.value,this,'Y')" TabIndex="23" runat="server" CssClass="EntryControl" onkeydown="return (event.keyCode!=13)"></asp:TextBox>
                                                    <asp:Image ID="imgEditdLastMenstrualDate" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                    <asp:Image ID="imgAuditdLastMenstrualDate" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none"
                                                        ImageUrl="images/Audit_Small.png" />

                                                    <cc1:CalendarExtender ID="cetxtdLastMenstrualDate" runat="server" TargetControlID="txtdLastMenstrualDate"
                                                        Format="dd-MMM-yyyy">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" class="LabelText">Cycle Length:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox ID="txtiLastMenstrualDays" TabIndex="2" MaxLength="24" runat="server" CssClass="EntryControl"
                                                        Style="width: 40px" onkeypress="return isNumberKey(event)" onkeydown="return (event.keyCode!=13)" />
                                                    <asp:Image ID="imgEditiLastMenstrualDays" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                    <asp:Image ID="imgAuditiLastMenstrualDays" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none"
                                                        ImageUrl="images/Audit_Small.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" class="LabelText">Regular:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:Image ID="imgEditcRegular" Style="vertical-align: sub; display:none;" ToolTip="Edit" CssClass="EditControl"
                                                        runat="server" ImageUrl="images/Edit_Small.png" />
                                                    <asp:Image ID="imgAuditcRegular" ToolTip="Audit Trail" CssClass="AuditControl" Style="vertical-align: sub; display:none;"
                                                        runat="server" ImageUrl="images/Audit_Small.png" />
                                                    <asp:RadioButtonList ID="rblcRegular" TabIndex="25" Style="float: left;" runat="server"
                                                        CssClass="ConControl" RepeatDirection="Horizontal">
                                                        <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                        <asp:ListItem Value="N">No</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" class="LabelText">Association with Pain:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:Image ID="imgEditcLastMenstrualIndi" Style="vertical-align: sub; display:none;" ToolTip="Edit" CssClass="EditControl"
                                                        runat="server" ImageUrl="images/Edit_Small.png" />
                                                    <asp:Image ID="imgAuditcLastMenstrualIndi" ToolTip="Audit Trail" CssClass="AuditControl" Style="vertical-align: sub; display:none;"
                                                        runat="server" ImageUrl="images/Audit_Small.png" />
                                                    <asp:RadioButtonList ID="rblcLastMenstrualIndi" CssClass="ConControl" Style="float: left;" TabIndex="26"
                                                        runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem Value="0">Painful</asp:ListItem>
                                                        <asp:ListItem Value="1">Painless</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                                <td style="width: 65%">
                                    <fieldset id="fFamilyPlanning" class="FieldSetBox">
                                        <legend class="LegendText" style="font-size: 14px !important;">Family Planning Measures</legend>
                                        <table style="width: 100%; margin-bottom: 1.5%;">
                                            <tr>
                                                <td style="text-align: right; width: 36%;" class="LabelText">Family Planning Measures:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:Image ID="imgEditcContraception" Style="vertical-align: sub; display:none;" ToolTip="Edit" CssClass="EditControl"
                                                        runat="server" ImageUrl="images/Edit_Small.png" />
                                                    <asp:Image ID="imgAuditcContraception" ToolTip="Audit Trail" CssClass="AuditControl" Style="vertical-align: sub; display:none;"
                                                        runat="server" ImageUrl="images/Audit_Small.png" />
                                                    <asp:RadioButtonList ID="rblcContraception" TabIndex="27" Style="float: left;" runat="server"
                                                        CssClass="ConControl" RepeatDirection="Horizontal">
                                                        <asp:ListItem Value="P">Permanent Contraception</asp:ListItem>
                                                        <asp:ListItem Value="T">Temporary Contraception</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" class="LabelText">Details of Contraception:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:Image ID="imgEditcBarrier" Style="vertical-align: sub; display:none" ToolTip="Edit" CssClass="EditControl"
                                                        runat="server" ImageUrl="images/Edit_Small.png" ctype="Contrapception" />
                                                    <asp:Image ID="imgAuditcBarrier" ToolTip="Audit Trail" CssClass="AuditControl" Style="vertical-align: sub; display:none;"
                                                        runat="server" ImageUrl="images/Audit_Small.png" ctype="Contrapception" />
                                                    <asp:CheckBoxList ID="chkcContraception" TabIndex="28" Style="float: left;" runat="server"
                                                        CssClass="ConControl" RepeatDirection="Horizontal">
                                                        <asp:ListItem Value="B">Double Barrier</asp:ListItem>
                                                        <asp:ListItem Value="P">Pills</asp:ListItem>
                                                        <asp:ListItem Value="R">Rhythm</asp:ListItem>
                                                        <asp:ListItem Value="I">IUCD</asp:ListItem>
                                                    </asp:CheckBoxList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" class="LabelText">Remarks:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox ID="txtvOtherRemark" class="EntryControl" TabIndex="29" runat="server" onblur="characterlimit(this)" 
                                                        TextMode="MultiLine" Style="width: 80%;"></asp:TextBox>
                                                    <asp:Image ID="imgEditvOtherRemark" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                    <asp:Image ID="imgAuditvOtherRemark" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ImageUrl="images/Audit_Small.png" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="width: 100%;">
                                    <fieldset id="fObstetricHistory" style="width: 97%;" class="FieldSetBox">
                                        <legend class="LegendText" style="font-size: 14px !important;">Obstetric History</legend>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="text-align: right; width: 17%" class="LabelText">Date of Last Delivery:
                                                </td>
                                                <td style="text-align: left; width: 25%;">
                                                    <asp:TextBox ID="txtdLastDelivaryDate" onchange="DateConvert_Age(this.value,this,'Y')" TabIndex="30" runat="server" Style="width: 100px;" onkeydown="return (event.keyCode!=13)" CssClass="EntryControl"></asp:TextBox>
                                                    <asp:Image ID="imgEditdLastDelivaryDate" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                    <asp:Image ID="imgAuditdLastDelivaryDate" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ImageUrl="images/Audit_Small.png" />
                                                    <cc1:CalendarExtender ID="cetxtdLastDelivaryDate" runat="server" TargetControlID="txtdLastDelivaryDate"
                                                        Format="dd-MMM-yyyy">
                                                    </cc1:CalendarExtender>

                                                </td>
                                                <td style="text-align: right; width: 18%" class="LabelText">Gravida:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox ID="txtvGravida" TabIndex="31" MaxLength="50" runat="server" CssClass="EntryControl" onkeydown="return (event.keyCode!=13)"></asp:TextBox>
                                                    <asp:Image ID="imgEditvGravida" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                    <asp:Image ID="imgAuditvGravida" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ImageUrl="images/Audit_Small.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" class="LabelText">No.of Live Children:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox ID="txtiNoOfChildren" Style="width: 40px" TabIndex="32" runat="server"
                                                        CssClass="EntryControl" onkeypress="return isNumberKey(event)" onkeydown="return (event.keyCode!=13)"></asp:TextBox>
                                                    <asp:Image ID="imgEditiNoOfChildren" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                    <asp:Image ID="imgAuditiNoOfChildren" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ImageUrl="images/Audit_Small.png" />
                                                </td>
                                                <td style="text-align: right;" class="LabelText">Para:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox ID="txtvPara" TabIndex="33" MaxLength="50" runat="server" CssClass="EntryControl" onkeydown="return (event.keyCode!=13)"></asp:TextBox>
                                                    <asp:Image ID="imgEditvPara" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                    <asp:Image ID="imgAuditvPara" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ImageUrl="images/Audit_Small.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" class="LabelText">No. of children died:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox ID="txtiNoOfChildrenDied" TabIndex="34" runat="server" CssClass="EntryControl" onkeydown="return (event.keyCode!=13)"
                                                        Style="width: 40px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                    <asp:Image ID="imgEditiNoOfChildrenDied" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                    <asp:Image ID="imgAuditiNoOfChildrenDied" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ImageUrl="images/Audit_Small.png" />
                                                </td>
                                                <td style="text-align: right;" class="LabelText">Remarks if children died:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox ID="txtvRemarkifDied" TabIndex="35" runat="server" CssClass="EntryControl" onkeydown="return (event.keyCode!=13)"></asp:TextBox>
                                                    <asp:Image ID="imgEditvRemarkifDied" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                    <asp:Image ID="imgAuditvRemarkifDied" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ImageUrl="images/Audit_Small.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" class="LabelText">All Children Healthy:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:Image ID="imgEditcChildrenHelath" Style="vertical-align: sub; display:none;" ToolTip="Edit" CssClass="EditControl"
                                                        runat="server" ImageUrl="images/Edit_Small.png" />
                                                    <asp:Image ID="imgAuditcChildrenHelath" ToolTip="Audit Trail" CssClass="AuditControl" Style="vertical-align: sub; display:none;"
                                                        runat="server" ImageUrl="images/Audit_Small.png" />
                                                    <asp:RadioButtonList ID="rblcChildrenHelath" CssClass="ConControl" Style="float: left;"
                                                        TabIndex="36" runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                        <asp:ListItem Value="N">No</asp:ListItem>
                                                        <asp:ListItem Value="A">NA</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td style="text-align: right;" class="LabelText">Remarks:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox ID="txtvchildrenHealthRemark" TabIndex="37" runat="server" CssClass="EntryControl"
                                                        MaxLength="500" onkeydown="return (event.keyCode!=13)"></asp:TextBox>
                                                    <asp:Image ID="imgEditvchildrenHealthRemark" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                    <asp:Image ID="imgAuditvchildrenHealthRemark" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ImageUrl="images/Audit_Small.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" class="LabelText">Any Spontaneous Abortion/MTP:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:Image ID="imgEditcAbortions" Style="vertical-align: sub; display:none;" ToolTip="Edit" CssClass="EditControl"
                                                        runat="server" ImageUrl="images/Edit_Small.png" />
                                                    <asp:Image ID="imgAuditcAbortions" ToolTip="Audit Trail" CssClass="AuditControl" Style="vertical-align: sub; display:none;"
                                                        runat="server" ImageUrl="images/Audit_Small.png" />
                                                    <asp:RadioButtonList ID="rblcAbortions" TabIndex="38" Style="float: left;" runat="server"
                                                        CssClass="ConControl" RepeatDirection="Horizontal">
                                                        <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                        <asp:ListItem Value="N">No</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td style="text-align: right;" class="LabelText">Date Of Last Abortion:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox class="EntryControl" onchange="DateConvert_Age(this.value,this,'Y')" ID="txtdAbortionDate" TabIndex="39" runat="server"
                                                        Style="width: 100px;" onkeydown="return (event.keyCode!=13)"></asp:TextBox>
                                                    <asp:Image ID="imgEditdAbortionDate" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                    <asp:Image ID="imgAuditdAbortionDate" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ImageUrl="images/Audit_Small.png" />

                                                    <cc1:CalendarExtender ID="cetxtdAbortionDate" runat="server" TargetControlID="txtdAbortionDate"
                                                        Format="dd-MMM-yyyy">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" class="LabelText">Lactating:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:Image ID="imgEditcLoctating" Style="vertical-align: sub; display:none;" ToolTip="Edit" CssClass="EditControl"
                                                        runat="server" ImageUrl="images/Edit_Small.png" />
                                                    <asp:Image ID="imgAuditcLoctating" ToolTip="Audit Trail" CssClass="AuditControl" Style="vertical-align: sub; display:none;"
                                                        runat="server" ImageUrl="images/Audit_Small.png" />
                                                    <asp:RadioButtonList ID="rblcLoctating" TabIndex="40" CssClass="ConControl" Style="float: left;"
                                                        runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                        <asp:ListItem Value="N">No</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td style="text-align: right;" class="LabelText">Volunteer is in the child bearing age:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:Image ID="imgEditcIsVolunteerinBearingAge" Style="vertical-align: sub; display:none;" ToolTip="Edit" CssClass="EditControl"
                                                        runat="server" ImageUrl="images/Edit_Small.png" />
                                                    <asp:Image ID="imgAuditcIsVolunteerinBearingAge" ToolTip="Audit Trail" CssClass="AuditControl" Style="vertical-align: sub; display:none;"
                                                        runat="server" ImageUrl="images/Audit_Small.png" />
                                                    <asp:RadioButtonList ID="rblcIsVolunteerinBearingAge" CssClass="ConControl" Style="float: left;"
                                                        TabIndex="41" runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                        <asp:ListItem Value="N">No</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="ContactInfo" style="width: 95%;">
                        <table style="width: 100%;">
                            <tr>
                                <td>
                                    <fieldset id="fSubjectAdd" class="FieldSetBox">
                                        <legend class="LegendText" style="font-size: 14px !important;">Subject Contact Detail</legend>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="text-align: right; width: 13%;" class="LabelText">Local Address(1):
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                    <asp:TextBox ID="txtvLocalAdd1" TextMode="MultiLine" TabIndex="42" class="EntryControl" runat="server" Style="width: 60%; text-align: left; font-size: 11px;"
                                                        MaxLength="250" onblur="characterlimit(this)"  >
                                                    </asp:TextBox>
                                                    <asp:Image ID="imgEditvLocalAdd1" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;"
                                                        ctype="localadd1" />
                                                    <asp:Image ID="imgAuditvLocalAdd1" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ctype="localadd1" ImageUrl="images/Audit_Small.png" />
                                                </td>
                                                <td style="text-align: right; width: 13%;" class="LabelText">Local Address(2):
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                    <asp:TextBox ID="txtvLocalAdd21" TabIndex="43" TextMode="MultiLine" class="EntryControl" runat="server" Style="width: 60%; font-size: 11px;"
                                                        MaxLength="250" onblur="characterlimit(this)"  ></asp:TextBox>
                                                    <asp:Image ID="imgEditvLocalAdd21" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;"
                                                        ctype="localAdd2" />
                                                    <asp:Image ID="imgAuditvLocalAdd21" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ctype="localAdd2" ImageUrl="images/Audit_Small.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" class="LabelText">Local Tel1 No:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox ID="txtvLocalTelephoneno1" CssClass="EntryControl" Style="width: 150px;"
                                                        TabIndex="44" runat="server" onkeydown="return (event.keyCode!=13)"></asp:TextBox>
                                                    <asp:Image ID="imgEditvLocalTelephoneno1" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                    <asp:Image ID="imgAuditvLocalTelephoneno1" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ImageUrl="images/Audit_Small.png" />
                                                </td>
                                                <td style="text-align: right;" class="LabelText">Local Tel2 No:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox ID="txtvLocalTelephoneno2" CssClass="EntryControl" Style="width: 150px;" onkeydown="return (event.keyCode!=13)"
                                                        TabIndex="45" runat="server"></asp:TextBox>
                                                    <asp:Image ID="imgEditvLocalTelephoneno2" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                    <asp:Image ID="imgAuditvLocalTelephoneno2" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ImageUrl="images/Audit_Small.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" class="LabelText">Permanent Address:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox ID="txtvPerAdd1" TabIndex="46" Style="width: 60%; font-size: 11px;"
                                                        runat="server" TextMode="MultiLine" onblur="characterlimit(this)"    class="EntryControl" MaxLength="250">SAME AS LOCAL</asp:TextBox>
                                                    <asp:Image ID="imgEditvPerAdd1" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;"
                                                        ctype="PerAdd" />
                                                    <asp:Image ID="imgAuditvPerAdd1" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ctype="PerAdd" ImageUrl="images/Audit_Small.png" />
                                                </td>
                                                <td style="text-align: right;" class="LabelText">Permanent Tel No:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox ID="txtvPerTelephoneno" MaxLength="15" TabIndex="47" runat="server" CssClass="EntryControl"
                                                        Style="width: 150px;" onkeydown="return (event.keyCode!=13)"></asp:TextBox>
                                                    <asp:Image ID="imgEditvPerTelephoneno" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                    <asp:Image ID="imgAuditvPerTelephoneno" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ImageUrl="images/Audit_Small.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" class="LabelText">Office Address:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox ID="txtvOfficeAddress" TabIndex="48" runat="server" TextMode="MultiLine"
                                                        Style="width: 60%; font-size: 11px;" MaxLength="250"  onblur="characterlimit(this)"  class="EntryControl"></asp:TextBox>
                                                    <asp:Image ID="imgEditvOfficeAddress" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                    <asp:Image ID="imgAuditvOfficeAddress" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ImageUrl="images/Audit_Small.png" />
                                                </td>
                                                <td style="text-align: right;" class="LabelText">Office Tel No:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox ID="txtvOfficeTelephoneno" MaxLength="50" TabIndex="49" runat="server" CssClass="EntryControl"
                                                        Style="width: 150px;" onkeydown="return (event.keyCode!=13)"></asp:TextBox>
                                                    <asp:Image ID="imgEditvOfficeTelephoneno" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                    <asp:Image ID="imgAuditvOfficeTelephoneno" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ImageUrl="images/Audit_Small.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td></td>
                                                <td style="text-align: right;" class="LabelText">Place *:
                                                </td>
                                                <td style="text-align: left;">
                                                    <%--<asp:TextBox ID="txtvPerCity" TabIndex="50" MaxLength="50" runat="server" CssClass="EntryControl"
                                                        Width="30%" onkeydown="return (event.keyCode!=13)"></asp:TextBox>--%>
                                                    <asp:DropDownList ID="ddlvPerCity" TabIndex="17" CssClass="EntryControl" Style="width: 150px;" runat="server"></asp:DropDownList>
                                                    <asp:Image ID="imgEditvPerCity" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                    <asp:Image ID="imgAuditvPerCity" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ImageUrl="images/Audit_Small.png" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <fieldset id="fContactPerson" class="FieldSetBox">
                                        <legend class="LegendText" style="font-size: 14px !important;">Contact Person Contact Detail</legend>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="text-align: right; width: 13%;" class="LabelText">Person Name(1):
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                    <asp:TextBox ID="txtvContactName1" MaxLength="100" TabIndex="51" runat="server" CssClass="EntryControl"
                                                        Width="60%" onkeydown="return (event.keyCode!=13)" onkeyPress="return (event.keyCode!=13)"> 
                                                    </asp:TextBox>
                                                    <asp:Image ID="imgEditvContactName1" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                    <asp:Image ID="imgAuditvContactName1" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ImageUrl="images/Audit_Small.png" />
                                                </td>
                                                <td style="text-align: right; width: 13%" class="LabelText">Person Name(2):
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                    <asp:TextBox ID="txtvContactName2" TabIndex="52" MaxLength="100" runat="server" CssClass="EntryControl" onkeydown="return (event.keyCode!=13)">
                                                    </asp:TextBox>
                                                    <asp:Image ID="imgEditvContactName2" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                    <asp:Image ID="imgAuditvContactName2" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ImageUrl="images/Audit_Small.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right; width: 13%;" class="LabelText">Person Adds.(1):
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                    <asp:TextBox ID="txtvContactAddress11" TabIndex="53" MaxLength="250" runat="server" Style="width: 60%; font-size: 11px;" onblur="characterlimit(this)"  
                                                        class="EntryControl" TextMode="MultiLine"></asp:TextBox>
                                                    <asp:Image ID="imgEditvContactAddress11" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                    <asp:Image ID="imgAuditvContactAddress11" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ImageUrl="images/Audit_Small.png" />
                                                </td>
                                                <td style="text-align: right;" class="LabelText">Person Adds.(2):
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                    <asp:TextBox ID="txtvContactAddress21" TabIndex="54" MaxLength="250" class="EntryControl" TextMode="MultiLine" onblur="characterlimit(this)"  
                                                        runat="server" Style="width: 60%; font-size: 11px;"></asp:TextBox>
                                                    <asp:Image ID="imgEditvContactAddress21" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                    <asp:Image ID="imgAuditvContactAddress21" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ImageUrl="images/Audit_Small.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right; width: 13%;" class="LabelText">Person Tel No(1):
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                    <asp:TextBox ID="txtvContactTelephoneno1" TabIndex="55" runat="server" CssClass="EntryControl"
                                                        Style="width: 150px;" onkeydown="return (event.keyCode!=13)"></asp:TextBox>
                                                    <asp:Image ID="imgEditvContactTelephoneno1" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                    <asp:Image ID="imgAuditvContactTelephoneno1" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ImageUrl="images/Audit_Small.png" />
                                                </td>
                                                <td style="text-align: right;" class="LabelText">Person Tel No(2):
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                    <asp:TextBox ID="txtvContactTelephoneno2" TabIndex="56" runat="server" CssClass="EntryControl"
                                                        Style="width: 150px;" onkeydown="return (event.keyCode!=13)"></asp:TextBox>
                                                    <asp:Image ID="imgEditvContactTelephoneno2" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit_Small.png" Style="display:none;" />
                                                    <asp:Image ID="imgAuditvContactTelephoneno2" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ImageUrl="images/Audit_Small.png" />
                                                </td>
                                            </tr>
                                            <tr >
                                                <td style="text-align: right;  display:none;  width: 13%;" class="LabelText">Referred By *:
                                                </td>
                                                <td style="text-align: left; width: 30%; display:none;">
                                                    <asp:TextBox ID="txtvReferredBy" Style="width: 150px;  " TabIndex="57" MaxLength="50" runat="server"
                                                        CssClass="EntryControl" onkeydown="return (event.keyCode!=13)"></asp:TextBox>
                                                    <asp:Image ID="imgEditvReferredBy" ToolTip="Edit" CssClass="EditControl" runat="server" Style="display:none;"
                                                        ImageUrl="images/Edit_Small.png" />
                                                    <asp:Image ID="ImgAuditvReferredBy" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ImageUrl="images/Audit_Small.png" />
                                                </td>
                                                <%--<td style="text-align: right;" class="LabelText"></td>
                                                <td style="text-align: left;"></td>--%>
                                                <td style="text-align: right; width: 13%;" class="LabelText">Remarks:
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                    <asp:TextBox ID="txtvRemarks" Style="width: 150px;" TabIndex="58" runat="server" onblur="characterlimit(this)"  
                                                        CssClass="EntryControl" TextMode="MultiLine"></asp:TextBox>
                                                    <asp:Image ID="imgEditvRemarks" ToolTip="Edit" CssClass="EditControl" runat="server" Style="display:none;"
                                                        ImageUrl="images/Edit_Small.png" />
                                                    <asp:Image ID="imgAuditvRemarks" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display:none;"
                                                        ImageUrl="images/Audit_Small.png" />
                                                </td>
                                            </tr>


                                            <%--<tr>
                                                <td style="text-align: right; width: 13%;" class="LabelText">Remarks:
                                                </td>
                                               
                                            </tr>--%>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <asp:Button ID="btnsave" runat="server" Text="Save" ToolTip="Save" CssClass="btn btnsave"
                    Width="10%" OnClientClick="return Validation('ADD');"></asp:Button>
                <asp:Button runat="server" Text="Cancel" ID="btnCancel" CssClass="btn btncancel" OnClick="btnCancel_Click" Width="10%" />
            </div>
            <asp:HiddenField ID="HFEdit" runat="server" />
            <asp:HiddenField ID="HFMode" runat="server" />
            <asp:HiddenField ID="HFAttach" runat="server" />
            <asp:HiddenField ID="HFRemarks" runat="server" />
            <asp:HiddenField ID="HFFileName" runat="server" />
            <asp:HiddenField ID="HFDelete" runat="server" />
            <asp:HiddenField ID="HFADD" runat="server" />
            <asp:HiddenField ID="HFDelete1" runat="server" />
            <asp:HiddenField ID="HFTimeZone" runat="server" />
            <asp:Button runat="server" ID="btnActualSave" Style="display:none" />
            </b></b>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnEdit" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="GVSubjectProof" EventName="RowCommand" />
            <asp:AsyncPostBackTrigger ControlID="btnsave" EventName="Click" />
            <asp:PostBackTrigger ControlID="btngenerate" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Button ID="btnAlert" runat="server" Style="display:none;" TabIndex="53" />
    <cc1:ModalPopupExtender ID="mdlAlert" runat="server" PopupControlID="divAlert" BackgroundCssClass="modalBackground"
        BehaviorID="mdlAlert" CancelControlID="btnNo" TargetControlID="btnAlert">
    </cc1:ModalPopupExtender>
    <div id="divAlert" runat="server" class="centerModalPopup" style="display:none; overflow: auto; width: 35%; height: auto; max-height: 25%; min-height: auto;">
        <table cellpadding="2" cellspacing="2" width="100%">
            <tr>
                <td id="AlertHeader" class="LabelText" style="text-align: left !important; font-size: 12px !important; width: 97%;"></td>
            </tr>
            <tr>
                <td>
                    <hr />
                </td>
            </tr>
            <tr>
                <td id="AlertMessage" class="LabelText" style="text-align: center !important;"></td>
            </tr>
            <tr>
                <td>
                    <hr />
                </td>
            </tr>
            <tr>
                <td style="text-align: center;">
                    <asp:Button ID="btnYes" runat="server" Text="Yes" CssClass="ButtonText" Width="57px"
                        Style="font-size: 12px !important; display: inline;" TabIndex="54" />
                    <asp:Button ID="btnNo" runat="server" Text="No" CssClass="ButtonText" Width="57px"
                        Style="font-size: 12px !important; display: inline;" TabIndex="54" />
                    <asp:Button ID="btnOk" runat="server" Text="Ok" CssClass="ButtonText" Width="57px"
                        Style="font-size: 12px !important; display:none;" TabIndex="54" />
                </td>
            </tr>
        </table>
    </div>
    <asp:Button ID="btnAudit" runat="server" Style="display:none;" TabIndex="55" />
    <cc1:ModalPopupExtender ID="MPEAudit" runat="server" PopupControlID="divAudit" BackgroundCssClass="modalBackground"
        BehaviorID="MPEAudit" CancelControlID="imgClose" TargetControlID="btnAudit">
    </cc1:ModalPopupExtender>
    <div id="divAudit" runat="server" class="centerModalPopup" style="display:none; overflow: auto; width: 60%; height: auto; max-height: 300px; min-height: auto;">
        <table cellpadding="2" cellspacing="2" width="100%">
            <tr>
                <td class="LabelText" style="text-align: center !important; font-size: 12px !important; width: 97%;">
                    <asp:Image ID="imgClose" runat="server" ImageUrl="~/images/Sqclose.gif" Style="float: right;" Title="Close"/>
                    <h3>Audit Trail</h3>
                    <hr />
                </td>
            </tr>
            <tr>
                <td>
                    <table id="tblAudit" width="100%">
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <hr />
                </td>
            </tr>
        </table>
    </div>
    <asp:Button runat="server" ID="btnHide" Style="display:none;" />
    <asp:Button ID="btnWarning" runat="server" Style="display:none;" TabIndex="51" />
    <cc1:ModalPopupExtender ID="mdlWarning" runat="server" PopupControlID="divWarning"
        BackgroundCssClass="modalBackground" BehaviorID="mdlWarning" TargetControlID="btnWarning">
    </cc1:ModalPopupExtender>
    <div id="divWarning" runat="server" class="centerModalPopup" style="display:none; overflow: auto; width: 35%; height: auto; max-height: 25%; min-height: auto; z-index: 9999999 !important;">
        <table cellpadding="2" cellspacing="2" width="100%">
            <tr>
                <td id="WarningHeader" class="LabelText" style="text-align: left !important; font-size: 12px !important; width: 97%;"></td>
            </tr>
            <tr>
                <td>
                    <hr />
                </td>
            </tr>
            <tr>
                <td id="WarningMessage" class="LabelText" style="text-align: center !important;"></td>
            </tr>
            <tr>
                <td>
                    <hr />
                </td>
            </tr>
            <tr>
                <td style="text-align: center;">
                    <asp:Button ID="btnWarningOk" runat="server" Text="Ok" CssClass="ButtonText" Width="57px"
                        Style="font-size: 12px !important;" TabIndex="52" />
                </td>
            </tr>
        </table>
    </div>
    <asp:Button ID="btnRemarks" runat="server" Style="display:none;" TabIndex="55" />
    <asp:Button ID="Button1" runat="server" Text="Cancel" CssClass="ButtonText"
        Width="64px" Style="font-size: 12px !important; display:none;" TabIndex="56" />
    <cc1:ModalPopupExtender ID="mdlRemarks" runat="server" PopupControlID="divRemarks"
        BackgroundCssClass="modalBackground" BehaviorID="mdlRemarks" CancelControlID="btnRemarksCancel"
        TargetControlID="btnRemarks">
    </cc1:ModalPopupExtender>
    <div id="divRemarks" runat="server" class="centerModalPopup" style="display:none; overflow: auto; width: 32%; height: auto; max-height: 45%; min-height: auto;">
        <table cellpadding="2" cellspacing="2" width="100%">
            <tr>
                <td class="LabelText" style="text-align: left !important; font-size: 12px !important; width: 97%;">Remarks
                </td>
            </tr>
            <tr>
                <td>
                    <hr />
                </td>
            </tr>
            <tr>
                <td class="LabelText" style="text-align: left !important;">Enter Remarks:
                </td>
            </tr>
            <tr>
                <td style="text-align: left !important;">
                    <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="5" Height="60px"
                        Width="300px" TabIndex="55" />
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
                        Width="64px" Style="font-size: 12px !important;" TabIndex="56" />
                    <asp:Button ID="btnRemarksCancel" runat="server" Text="Cancel" CssClass="btn btncancel"
                        Width="64px" Style="font-size: 12px !important;" TabIndex="56" />

                </td>
            </tr>
        </table>
    </div>


    <script type="text/javascript">
        $(document).ready(function () {
            if ((document.getElementById('<%=HFAttachstatus.ClientId %>').value != "Y") && (document.getElementById('<%=HFEdit.ClientId %>').value != "Y")) {
                // $('.EditControl').each(function () { this.style.display = "none"; });
                //  $('.AuditControl').each(function () { this.style.display = "none"; });
            }
            document.getElementById('<%=HFEdit.ClientId %>').value = "";
            if (document.getElementById('<%=HFAttachstatus.ClientId %>').value == "Y") {
                disableControl();
                $('#ctl00_CPHLAMBDA_Image36').attr('title', $('#ctl00_CPHLAMBDA_Image36').attr('title').replace('Edit', 'Update'));
                $('#ctl00_CPHLAMBDA_Image36').attr('src', $('#ctl00_CPHLAMBDA_Image36').attr('src').replace('Edit_Small.png', 'Update.png'));
                $('#ctl00_CPHLAMBDA_Image36').attr('class', 'UpdateControl');
                $('#ctl00_CPHLAMBDA_Image36'.previousElementSibling).removeAttr('disabled');
                $('#ctl00_CPHLAMBDA_Image37'.previousElementSibling).removeAttr('disabled');
                document.getElementById('<%=HFAttachstatus.ClientId %>').value = "";

            }
            var Edit = '<%=ViewState("Edit")%>'
            if (Edit == "Y" && document.getElementById('<%=HFMode.ClientID%>').value == "4") {
                AuditdisableControl();
            }
            var Mode = document.getElementById('<%=HFMode.ClientId %>').value

            if (Mode == "4") {
                document.getElementById('<%= Image36.ClientID%>').style.visibility = "hidden";
            }
        });


        function pageLoad() {


            $("#tabs").tabs();
            //$("#ctl00_CPHLAMBDA_txtDBirthDate").datepicker(
            // {
            //     changeMonth: true,
            //     changeYear: true,
            //     dateFormat: 'dd-M-yy',
            //     maxDate: 'today',
            //     onSelect: function (dateStr) {
            //         var date = $(this).datepicker('getDate');

            //         if (date) {
            //             date.setDate(date.getDate());
            //         }
            //         //alert(new Date(dateStr).getFullYear() + ":"+new Date(dateStr).getMonth() +":" + new Date(dateStr).getDate());
            //       //  $('#ctl00_CPHLAMBDA_txtdoer').datepicker('option', 'minDate', new Date(new Date(dateStr).getFullYear() + 17, new Date(dateStr).getMonth(), new Date(dateStr).getDate()));
            //         getAge();
            //     }
            // }

            //).val();
            //$("#ctl00_CPHLAMBDA_txtdoer").datepicker(
            // {
            //     changeMonth: true,
            //     changeYear: true,
            //     dateFormat: 'dd-M-yy',
            //     maxDate: 'today',
            //     onSelect: function (dateStr) {
            //         var date = $(this).datepicker('getDate');
            //         if (date) {
            //             date.setDate(date.getDate());
            //         }
            //         //$('#ctl00_CPHLAMBDA_txtDBirthDate').datepicker('option', 'maxDate', new Date(new Date(dateStr).getFullYear() - 17, new Date(dateStr).getMonth(), new Date(dateStr).getDate()));
            //     }
            // }

            //).val();

            //$("#ctl00_CPHLAMBDA_txtdLastMenstrualDate").datepicker(
            // {
            //     changeMonth: true,
            //     changeYear: true,
            //     dateFormat: 'dd-M-yy'
            // }).val();

            //$("#ctl00_CPHLAMBDA_txtdLastDelivaryDate").datepicker(
            // {
            //     changeMonth: true,
            //     changeYear: true,
            //     dateFormat: 'dd-M-yy'
            // }).val();

            //$("#ctl00_CPHLAMBDA_txtdAbortionDate").datepicker(
            // {
            //     changeMonth: true,
            //     changeYear: true,
            //     dateFormat: 'dd-M-yy'
            // }).val();


            // getAge();
            fnEditField();
            fnUpdateField();
            fnSaveField();
            //fnAuditTrail();
            //fnAppyDatatable();
            fnAuditField();
            //var rowscount = document.getElementByID('<%=GVSubjectProof.ClientID%>').rows.length;
            //alert(rowscount)

            // var mode =document.getElementById(<%=Request.QueryString("name")%>)
            //    if (mode == 4) {

            //}
            var totalRows = $("#<%=GVSubjectProof.ClientID %> tr").length;
            if (totalRows == 0) {
                document.getElementById('<%= Image36.ClientID%>').style.visibility = "hidden";
                //  document.getElementById('<%= Image37.ClientID%>').style.visibility = "hidden";
                //document.getElementById("myP").style.visibility = "hidden";
                $("ctl00_CPHLAMBDA_Image36").attr("style", "display:none")

            }

        }

        var Mode = document.getElementById('<%=HFMode.ClientId %>').value

        if (Mode == 4) {

            document.getElementById('<%= Image36.ClientID%>').style.visibility = "hidden";
            document.getElementById('<%= Image38.ClientID%>').style.visibility = "hidden";
            $("ctl00_CPHLAMBDA_Image36").attr("style", "display:none")
            $("ctl00_CPHLAMBDA_Image38").attr("style", "display:none")

        }

        $('#ctl00_CPHLAMBDA_btnOk').click(function () {
            $find('mdlAlert').Hide();
        });




        function disableControl() {

            //$('.EditControl').each(function () { this.style.display = "inline"; });
            //$('.AuditControl').each(function () { this.style.display = "inline"; });
            //$('.ConControl').each(function () {

            //    $(this).children().children().children().find('input').attr('disabled', true);
            //});
            //$('.EntryControl').each(function () { $(this).attr('disabled', true); });
            //$('.ConEntryControl').each(function () { $(this).attr('disabled', true); });
        }

        function AuditdisableControl() {

            // $('.AuditControl').each(function () { this.style.display = "inline"; });
            //  $('.EditControl').each(function () { this.style.display = "none"; });
            //$('.ConControl').each(function () {
            //    debugger;
            //    $(this).children().children().children().find('input').attr('disabled', true);
            //});
            //$('.EntryControl').each(function () { $(this).attr('disabled', true); });
            //$('.ConEntryControl').each(function () { $(this).attr('disabled', true); });
        }

        function enableControl() {

            //$('.EditControl').each(function () { this.style.display = "none"; });
            //$('.AuditControl').each(function () { this.style.display = "none"; });
            //$('.ConControl').each(function () { $(this).removeAttr('disabled'); });
            //$('.EntryControl').each(function () { $(this).removeAttr('disabled'); });
            //$('.ConEntryControl').each(function () { $(this).removeAttr('disabled'); });
        }

        function enableControlAttach() {

            $('.EditControl').each(function () { this.style.display = "none"; });
            $('.AuditControl').each(function () { this.style.display = "none"; });
        }

        function funAttachDoc() {
            document.getElementById('<%=Image36.ClientID%>').className = "UpdateControl"
            var na = document.getElementById('<%=Image36.ClientID%>')
            $(na).attr('src', $(na).attr('src').replace('Edit_Small.png', 'Update.png'));
            $(na).attr('title', $(na).attr('title').replace('Edit', 'Update'));
            return true;
        }
        function ClientPopulated(sender, e) {
            SubjectClientShowing('AutoCompleteExtender1', $get('<%= txtSubject.ClientId %>'));
        }
        function OnSelected(sender, e) {
            SubjectOnItemSelected(e.get_value(), $get('<%= txtSubject.clientid %>'),
            $get('<%= HSubjectId.clientid %>'), document.getElementById('<%= btnEdit.ClientId %>'));
            if (window.location.search.substr(1).split('&')[0] == "mode=4") {
                window.location.href = "frmSubjectPIFMst_New.aspx?mode=4&SearchSubjectId=" + $get('<%= HSubjectId.clientid %>').value + "&SearchSubjectText=" + $get('<%= txtSubject.clientid %>').value;
            }
            else if (window.location.search.substr(1).split('&')[0] == "mode=11") {
                window.location.href = "frmSubjectPIFMst_New.aspx?mode=11&SearchSubjectId=" + $get('<%= HSubjectId.clientid %>').value + "&SearchSubjectText=" + $get('<%= txtSubject.clientid %>').value;
            }
            else {
                window.location.href = "frmSubjectPIFMst_New.aspx?mode=1&SearchSubjectId=" + $get('<%= HSubjectId.clientid %>').value + "&SearchSubjectText=" + $get('<%= txtSubject.clientid %>').value;
            }
    }
    function fnNew() {
        if (window.location.search.substr(1).split('&')[0] == "mode=11") {
            window.location.href = "frmSubjectPIFMst_New.aspx?mode=11";
        }
        else {
            window.location.href = "frmSubjectPIFMst_New.aspx?mode=1";
        }
    }
    function openFemaleInfo() {
        if ($('#ctl00_CPHLAMBDA_ddlcSex').val() == "M") {
            document.getElementById('LiFemaleInfo').style.display = 'none';
        }
        else {
            document.getElementById('LiFemaleInfo').style.display = '';
        }
    }
    function funCheckOthers() {
        document.getElementById('<%=txtAttach.ClientId %>').style.display = 'none';
        document.getElementById('ifOther').style.display = 'none';
        if ($('#ctl00_CPHLAMBDA_ddlProofType').val() == "Others") {
            document.getElementById('<%=txtAttach.ClientId %>').style.display = '';
            document.getElementById('ifOther').style.display = '';
        }
    }
    function SetInitial() {
        var txtvSurName = document.getElementById('<%=txtvSurName.ClientID%>');
            var txtmiddlename = document.getElementById('<%=txtvMiddleName.ClientID %>');
            var txtvFirstName = document.getElementById('<%=txtvFirstName.ClientID %>');
            var txtInitial = document.getElementById('<%=txtInitials.ClientID %>');
            var strlastname = new String();
            strlastname = txtvSurName.value;
            var strfirstname = new String();
            strfirstname = txtvFirstName.value;
            var strmiddlename = new String();
            strmiddlename = txtmiddlename.value;
            strlastname = strlastname.trimLeft();
            strfirstname = strfirstname.trimLeft();
            strmiddlename = strmiddlename.trimLeft();
            document.getElementById('<%=txtvSurName.ClientID%>').value = strlastname
        document.getElementById('<%=txtvMiddleName.ClientID %>').value = strmiddlename
            document.getElementById('<%=txtvFirstName.ClientID %>').value = strfirstname
            txtInitial.value = strlastname.substring(0, 1) + strfirstname.substring(0, 1) + strmiddlename.substring(0, 1);
            document.getElementById('<%=HFInitials.ClientID %>').value = txtInitial.value;
    }

    function getAge() {

        var dateString = document.getElementById('<%=txtdBirthDate.ClientID %>');
        var Enrolldate = document.getElementById('<%=txtdEnrollmentDate.ClientID%>');
        if (dateString.value != "" && Enrolldate.value != "") {
            var today = new Date();
            var birthDate = new Date(dateString.value);
            var enrollDate = new Date(Enrolldate.value);
            var age = enrollDate.getFullYear() - birthDate.getFullYear();
            var m = enrollDate.getMonth() - birthDate.getMonth();
            if (m < 0 || (m === 0 && enrollDate.getDate() < birthDate.getDate())) {
                age--;
            }
            document.getElementById('<%=txtAge.ClientID %>').value = age;
            document.getElementById('<%=HFAge.ClientID %>').value = age;
            if (age < 18) {
                msgalert('Age should be more than 18 years');
                // document.getElementById('<%= txtdBirthDate.ClientId %>').focus();
            }
        }
        else {
            document.getElementById('<%=txtAge.ClientID %>').value = "";
            document.getElementById('<%=HFAge.ClientID %>').value = "";
            // document.getElementById(this.id).focus();
        }

        var Edit = '<%=ViewState("Edit")%>'
        if (Edit != 'Y') {
            enableControl();
        }
        else {

        }
    }


    function DateConvert_Age(ParamDate, txtdate, CheckLess) {
        //getAge();
        document.getElementById('<%=txtAge.ClientID %>').style.disable = false;
        if (txtdate.value != "") {
            if (!DateConvert(ParamDate, txtdate)) {
                return false;
            }

            if (CheckLess = 'Y' && !CheckDateLessThenToday(txtdate.value)) {
                //txtdate.value = "";
                // txtdate.focus();
                msgalert('Date should be previous or equal to current date.');
                return false;
            }

            //SetAge(document.getElementById('<%=txtAge.ClientID %>'));
            document.getElementById('<%=txtAge.ClientID %>').style.disable = true;

            var initial = document.getElementById('<%=txtInitials.ClientID %>');
            var dob = document.getElementById('<%=txtdBirthDate.ClientID %>');
            var btn = document.getElementById('<%=btnCheckDuplicateSubject.ClientID %>');
            if (initial.value != "" && dob.value != "") {
                // btn.click();
            }
        }
        getAge();
        validateHabit();
        return true;

    }

    function ValidationMSR() {
        if (document.getElementById('<%= HSubjectId.ClientId %>').value.toString().trim().length <= 0) {
            msgalert('Please Enter Subject');
            document.getElementById('<%= txtSubject.ClientId %>').focus();
            document.getElementById('<%= txtSubject.ClientId %>').value = '';
            return false;
        }
        return true;
    }

    function CheckDuplicateSubject() {
        var initial = document.getElementById('<%=txtInitials.ClientID %>');
        var txtdate = document.getElementById('<%=txtdBirthDate.ClientID %>');
        var btn = document.getElementById('<%=btnCheckDuplicateSubject.ClientID %>');
        if (initial.value != "" && txtdate.value != "") {
            btn.click();
        }
        return true;
    }

    function calcBMI() {
        if (document.getElementById('<%=txtnHeight.ClientID %>').value != '' && document.getElementById('<%=txtnWeight.ClientID %>').value != '') {

            var weight = parseFloat(document.getElementById('<%=txtnWeight.ClientID %>').value)
            var height = parseFloat(document.getElementById('<%=txtnHeight.ClientID %>').value)
            var Wei = weight.toFixed(1)
            var Hei = height.toFixed(1)

            document.getElementById('<%=txtnHeight.ClientID %>').value = Hei
            document.getElementById('<%=txtnWeight.ClientID%>').value = Wei

            document.getElementById('<%=txtbmi.ClientID %>').value = GetBMI(document.getElementById('<%=txtnHeight.ClientID %>').value, document.getElementById('<%=txtnWeight.ClientID%>').value)
            document.getElementById('<%=HfBMI.ClientID %>').value = document.getElementById('<%=txtbmi.ClientID %>').value;
            var BMI = document.getElementById('<%=HfBMI.ClientID%>');
            if (BMI.value < 18) {
                msgalert('Please Note That BMI Is Less Than 18.')
            }

        }

    }
    function validate(DD, TC, TP) {

        var ddlValue = DD.options[DD.selectedIndex].value;
        TC.disabled = false;
        TP.disabled = false;
        if (ddlValue == 'N') {
            TC.disabled = true;
            TP.disabled = true;
            document.getElementById(TC.id).value = ''
            document.getElementById(TP.id).value = ''
        }
        else if (ddlValue == 'C') {
            TC.disabled = false;
            TP.disabled = true;
            document.getElementById(TP.id).value = ''
        }
    }


    function validateHabit() {
        var ddl1 = $('#ctl00_CPHLAMBDA_GVHabits_ctl02_ddlHebitType option:selected').text();
        var ddl2 = $('#ctl00_CPHLAMBDA_GVHabits_ctl03_ddlHebitType option:selected').text();
        var ddl3 = $('#ctl00_CPHLAMBDA_GVHabits_ctl04_ddlHebitType option:selected').text();
        var ddl4 = $('#ctl00_CPHLAMBDA_GVHabits_ctl05_ddlHebitType option:selected').text();
        var ddl5 = $('#ctl00_CPHLAMBDA_GVHabits_ctl06_ddlHebitType option:selected').text();
        var ddl6 = $('#ctl00_CPHLAMBDA_GVHabits_ctl07_ddlHebitType option:selected').text();
        var ddl7 = $('#ctl00_CPHLAMBDA_GVHabits_ctl08_ddlHebitType option:selected').text();

        $('#ctl00_CPHLAMBDA_GVHabits_ctl02_ddlHebitType').prop('disabled', false);
        $('#ctl00_CPHLAMBDA_GVHabits_ctl03_ddlHebitType').prop('disabled', false);
        $('#ctl00_CPHLAMBDA_GVHabits_ctl04_ddlHebitType').prop('disabled', false);
        $('#ctl00_CPHLAMBDA_GVHabits_ctl05_ddlHebitType').prop('disabled', false);
        $('#ctl00_CPHLAMBDA_GVHabits_ctl06_ddlHebitType').prop('disabled', false);
        $('#ctl00_CPHLAMBDA_GVHabits_ctl07_ddlHebitType').prop('disabled', false);
        $('#ctl00_CPHLAMBDA_GVHabits_ctl08_ddlHebitType').prop('disabled', false);

        if (ddl1 == "Never") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl02_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl02_txtEndDate').prop('disabled', true);
        }
        if (ddl1 == "Current") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl02_txtConsumption').prop('disabled', false);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl02_txtEndDate').prop('disabled', true);
        }

        if (ddl1 == "Previous") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl02_txtConsumption').prop('disabled', false);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl02_txtEndDate').prop('disabled', false);
        }

        if (ddl2 == "Never") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl03_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl03_txtEndDate').prop('disabled', true);
        }

        if (ddl2 == "Current") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl03_txtConsumption').prop('disabled', false);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl03_txtEndDate').prop('disabled', true);
        }

        if (ddl2 == "Previous") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl03_txtConsumption').prop('disabled', false);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl03_txtEndDate').prop('disabled', false);
        }

        if (ddl3 == "Never") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl04_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl04_txtEndDate').prop('disabled', true);
        }

        if (ddl3 == "Current") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl04_txtConsumption').prop('disabled', false);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl04_txtEndDate').prop('disabled', true);
        }

        if (ddl3 == "Previous") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl04_txtConsumption').prop('disabled', false);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl04_txtEndDate').prop('disabled', false);
        }

        if (ddl4 == "Never") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl05_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl05_txtEndDate').prop('disabled', true);
        }

        if (ddl4 == "Current") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl05_txtConsumption').prop('disabled', false);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl05_txtEndDate').prop('disabled', true);
        }
        if (ddl4 == "Previous") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl05_txtConsumption').prop('disabled', false);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl05_txtEndDate').prop('disabled', false);
        }
        if (ddl5 == "Never") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl06_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl06_txtEndDate').prop('disabled', true);
        }
        if (ddl5 == "Current") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl06_txtConsumption').prop('disabled', false);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl06_txtEndDate').prop('disabled', true);
        }
        if (ddl5 == "Previous") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl06_txtConsumption').prop('disabled', false);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl06_txtEndDate').prop('disabled', false);
        }

        if (ddl6 == "Never") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl07_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl07_txtEndDate').prop('disabled', true);
        }

        if (ddl6 == "Current") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl07_txtConsumption').prop('disabled', false);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl07_txtEndDate').prop('disabled', true);
        }

        if (ddl6 == "Previous") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl07_txtConsumption').prop('disabled', false);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl07_txtEndDate').prop('disabled', false);
        }

        if (ddl7 == "Never") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl08_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl08_txtEndDate').prop('disabled', true);
        }

        if (ddl7 == "Never") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl08_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl08_txtEndDate').prop('disabled', true);
        }
        if (ddl7 == "Current") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl08_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl08_txtEndDate').prop('disabled', true);
        }

        if (ddl7 == "Previous") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl08_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl08_txtEndDate').prop('disabled', true);
        }
    }



    function validateHabitForViewMode() {
        var ddl1 = $('#ctl00_CPHLAMBDA_GVHabits_ctl02_ddlHebitType option:selected').text();
        var ddl2 = $('#ctl00_CPHLAMBDA_GVHabits_ctl03_ddlHebitType option:selected').text();
        var ddl3 = $('#ctl00_CPHLAMBDA_GVHabits_ctl04_ddlHebitType option:selected').text();
        var ddl4 = $('#ctl00_CPHLAMBDA_GVHabits_ctl05_ddlHebitType option:selected').text();
        var ddl5 = $('#ctl00_CPHLAMBDA_GVHabits_ctl06_ddlHebitType option:selected').text();
        var ddl6 = $('#ctl00_CPHLAMBDA_GVHabits_ctl07_ddlHebitType option:selected').text();
        var ddl7 = $('#ctl00_CPHLAMBDA_GVHabits_ctl08_ddlHebitType option:selected').text();

        $('#ctl00_CPHLAMBDA_GVHabits_ctl02_ddlHebitType').prop('disabled', true);
        $('#ctl00_CPHLAMBDA_GVHabits_ctl03_ddlHebitType').prop('disabled', true);
        $('#ctl00_CPHLAMBDA_GVHabits_ctl04_ddlHebitType').prop('disabled', true);
        $('#ctl00_CPHLAMBDA_GVHabits_ctl05_ddlHebitType').prop('disabled', true);
        $('#ctl00_CPHLAMBDA_GVHabits_ctl06_ddlHebitType').prop('disabled', true);
        $('#ctl00_CPHLAMBDA_GVHabits_ctl07_ddlHebitType').prop('disabled', true);
        $('#ctl00_CPHLAMBDA_GVHabits_ctl08_ddlHebitType').prop('disabled', true);

        if (ddl1 == "Never") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl02_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl02_txtEndDate').prop('disabled', true);
        }
        if (ddl1 == "Current") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl02_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl02_txtEndDate').prop('disabled', true);
        }

        if (ddl1 == "Previous") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl02_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl02_txtEndDate').prop('disabled', true);
        }

        if (ddl2 == "Never") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl03_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl03_txtEndDate').prop('disabled', true);
        }

        if (ddl2 == "Current") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl03_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl03_txtEndDate').prop('disabled', true);
        }

        if (ddl2 == "Previous") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl03_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl03_txtEndDate').prop('disabled', true);
        }

        if (ddl3 == "Never") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl04_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl04_txtEndDate').prop('disabled', true);
        }

        if (ddl3 == "Current") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl04_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl04_txtEndDate').prop('disabled', true);
        }

        if (ddl3 == "Previous") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl04_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl04_txtEndDate').prop('disabled', true);
        }

        if (ddl4 == "Never") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl05_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl05_txtEndDate').prop('disabled', true);
        }

        if (ddl4 == "Current") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl05_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl05_txtEndDate').prop('disabled', true);
        }
        if (ddl4 == "Previous") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl05_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl05_txtEndDate').prop('disabled', true);
        }
        if (ddl5 == "Never") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl06_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl06_txtEndDate').prop('disabled', true);
        }
        if (ddl5 == "Current") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl06_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl06_txtEndDate').prop('disabled', true);
        }
        if (ddl5 == "Previous") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl06_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl06_txtEndDate').prop('disabled', true);
        }

        if (ddl6 == "Never") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl07_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl07_txtEndDate').prop('disabled', true);
        }

        if (ddl6 == "Current") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl07_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl07_txtEndDate').prop('disabled', true);
        }

        if (ddl6 == "Previous") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl07_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl07_txtEndDate').prop('disabled', true);
        }

        if (ddl7 == "Never") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl08_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl08_txtEndDate').prop('disabled', true);
        }

        if (ddl7 == "Never") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl08_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl08_txtEndDate').prop('disabled', true);
        }
        if (ddl7 == "Current") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl08_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl08_txtEndDate').prop('disabled', true);
        }

        if (ddl7 == "Previous") {
            $('#ctl00_CPHLAMBDA_GVHabits_ctl08_txtConsumption').prop('disabled', true);
            $('#ctl00_CPHLAMBDA_GVHabits_ctl08_txtEndDate').prop('disabled', true);
        }
    }

    function fnEditAttachment(id) {

        var Val = document.getElementById(id);
        $(Val).attr('title', $(Val).attr('title').replace('Edit', 'Update'));
        $(Val).attr('src', $(Val).attr('src').replace('Edit_Small.png', 'Update.png'));
        $(Val).attr('class', 'UpdateControl');
        fnUpdateField();

    }

    function fnEditField() {

        $('.EditControl').unbind('click').click(function () {

            $(this).attr('title', $(this).attr('title').replace('Edit', 'Update'));
            $(this).attr('src', $(this).attr('src').replace('Edit_Small.png', 'Update.png'));
            $(this).attr('class', 'UpdateControl');
            if ($(this).attr('id') == '<%= Image38.ClientID %>') {
                $('#<%= GVHabits.ClientID %> .EntryControl').removeAttr('disabled');
            }
            if ($(this).attr('ctype') == 'BloodGroup') {
                $(this.previousElementSibling).removeAttr('disabled');
                $(this.previousElementSibling.previousElementSibling).removeAttr('disabled');
            }
            else {
                $(this.previousElementSibling).removeAttr('disabled');
                if ($(this).previousElementSibling == null) {
                    if ($(this).next().next('').find('input').length != 0) {
                        $(this).next().next('').find('input').removeAttr('disabled');
                    }
                    else {
                        if ($(this).attr('ctype') != 'Consumption') {
                            $(this).parent().next().find('input').removeAttr('disabled');
                        }
                    }
                }
            }

            if ($(this).attr('id') == '<%= Image36.ClientID %>') {
                $('#<%= GVSubjectProof.ClientID %> .EntryControl').removeAttr('disabled');
            }
            if ($(this).attr('id') == '<%= Image38.ClientID%>') {
                validateHabit();
            }
            fnUpdateField();
        });


    }
    function fnAuditField() {

        $('.AuditControl').unbind('click').click(function () {


            TableName = $(this.previousElementSibling.previousElementSibling).attr('tName');
            ColumnName = $(this.previousElementSibling.previousElementSibling).attr('cName');
            if (TableName == undefined && ColumnName == undefined) {
                TableName = $(this).parent().next().find('table').attr('tName')
                ColumnName = $(this).parent().next().find('table').attr('cName');

                if (TableName == undefined && ColumnName == undefined) {
                    TableName = $(this.nextElementSibling).attr('tname');
                    ColumnName = $(this.nextElementSibling).attr('cName');
                }
                if ($(this).attr('ctype') == 'Consumption') {
                    TableName = 'view_SUBJECTHABITDETAILS';
                    ColumnName = '*';
                }
            }

            if (this.id == 'ctl00_CPHLAMBDA_Image37') {
                TableName = "SubjectProofDetails";
                ColumnName = "cStatusIndi";
            }
            var obj = new Object();

            if (TableName == 'SubjectMaster') {
                if (ColumnName == "vICFLanguageCodeId") {
                    obj.query = " select SubjectLanguageMst.vLanguageName as  vChangedValue,  " +
                    " '' as  vRemarks, UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName, " +
                    " ISNULL( SubjectMasterHistory.dModifyOn,SubjectMaster.dModifyOn )AS dModifyOn " +
                    " from  SubjectMaster " +
                    "  inner join UserMst UserMst_subject on(SubjectMaster.iModifyBy = UserMst_subject.iUserId)  " +
                    " inner join UserTypeMst UserTypeMst_subject on (UserMst_subject.vUserTypeCode=UserTypeMst_subject.vUserTypeCode) " +
                    " left join  " +
                    " (select SubjectMasterHistory.vsubjectid,SubjectMasterHistory.itranno, " +
                    " SubjectMasterHistory.vupdateremarks, " +
                    " SubjectMasterHistory.iModifyBy,SubjectMasterHistory.dModifyOn, " +
                    "  SubjectMasterHistory.vICFLanguageCodeId " +
                    " from SubjectMasterHistory  " +
                    " inner join " +
                    " (select   " +
                    " max(itranno) as itranno,vsubjectid from SubjectMasterHistory " +
                    "  where  vColumnName IS NULL AND vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +
                    "  group by vsubjectid) SubjectMasterHistory_latest on " +
                    "   SubjectMasterHistory_latest.vsubjectid = SubjectMasterHistory.vsubjectid " +
                    "  and SubjectMasterHistory_latest.itranno = SubjectMasterHistory.itranno " +
                    "  ) SubjectMasterHistory  on " +
                    "  SubjectMasterHistory.vsubjectid = SubjectMaster.vsubjectid " +
                    " left join UserMst on(SubjectMasterHistory.iModifyBy = UserMst.iUserId)  " +
                    " left join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode) " +
                    " LEFT join SubjectLanguageMst " +
                    " on(charindex(','+SubjectLanguageMst.vLanguageId+',',+','+isnull(SubjectMasterHistory.vICFLanguageCodeId,SubjectMaster.vICFLanguageCodeId)+',')>0) " +
                    " where  SubjectMaster.vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "' " +
                               " UNION  " +

                        "select SubjectLanguageMst.vLanguageName as  vChangedValue, vUpdateRemarks as  vRemarks, UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName,SubjectMasterHistory.dModifyOn" +
                               " from SubjectMasterHistory" +
                               " inner join UserMst on(SubjectMasterHistory.iModifyBy = UserMst.iUserId)" +
                               " inner join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)" +
                                " LEFT join SubjectLanguageMst" +
                               " on(charindex(','+SubjectLanguageMst.vLanguageId+',',+','+isnull(SubjectMasterHistory.vICFLanguageCodeId,'')+',')>0)" +
                               " where vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "' " +
                               " AND vColumnName like  '" + "%" + ColumnName + "%" + "'" +
                               " order by dModifyon "
                }
                else if (ColumnName == "cRh" || ColumnName == "cBloodGroup") {
                    obj.query =
                                // "select cBloodGroup+''+case when cRh ='P' then '+Ve'  When cRh = 'N' then '-Ve' else NULL End as  vChangedValue, vUpdateRemarks as  vRemarks, UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName,SubjectMasterHistory.dModifyOn" +
                                // " from SubjectMasterHistory" +
                                // " inner join UserMst on(SubjectMasterHistory.iModifyBy = UserMst.iUserId)" +
                                // " inner join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)" +
                                // " where vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "' " +

                    "  select ISNULL(SubjectMasterHistory.cBloodGroup,'')+''+case when ISNULL(SubjectMasterHistory.cRh,'') ='P' then '+Ve'  When SubjectMasterHistory.cRh = 'N' then '-Ve' else NULL End as  vChangedValue,  " +
                    "  '' as  vRemarks,  " +
                    "  isnull(UserMst.vUserName, UserMst_subject.vusername) +'('+ isnull(UserTypeMst.vUserTypeName,UserTypeMst_subject.vUserTypeName) +')' as UserName, " +
                    "  isnull(SubjectMasterHistory.dModifyOn,'') as dModifyOn " +

                    "    from  SubjectMaster " +
                    "  inner join UserMst UserMst_subject on(SubjectMaster.iModifyBy = UserMst_subject.iUserId)  " +
                    " inner join UserTypeMst UserTypeMst_subject on (UserMst_subject.vUserTypeCode=UserTypeMst_subject.vUserTypeCode)  " +
                    " left join  " +
                    " (select SubjectMasterHistory.vsubjectid,SubjectMasterHistory.itranno, " +
                    " SubjectMasterHistory.cBloodGroup,SubjectMasterHistory.vupdateremarks, " +
                    " SubjectMasterHistory.cRh, " +
                    " SubjectMasterHistory.iModifyBy,SubjectMasterHistory.dModifyOn " +
                    " from SubjectMasterHistory " +
                    " inner join " +
                    " (select  " +
                    " max(itranno) as itranno,vsubjectid from SubjectMasterHistory " +
                    "  where  vColumnName IS NULL AND vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +
                    "  group by vsubjectid) SubjectMasterHistory_latest on " +
                    "   SubjectMasterHistory_latest.vsubjectid = SubjectMasterHistory.vsubjectid " +
                    "  and SubjectMasterHistory_latest.itranno = SubjectMasterHistory.itranno " +
                    "  ) SubjectMasterHistory  on " +
                    "  SubjectMasterHistory.vsubjectid = SubjectMaster.vsubjectid " +

                    " left join UserMst on(SubjectMasterHistory.iModifyBy = UserMst.iUserId)  " +
                    " left join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode) " +
                    " where  SubjectMaster.vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +

                                " UNION  " +
                        "select cBloodGroup+''+case when cRh ='P' then '+Ve' When cRh = 'N' then '-Ve' else '' End as  vChangedValue, vUpdateRemarks as  vRemarks, UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName,SubjectMasterHistory.dModifyOn" +
                                " from SubjectMasterHistory" +
                                " inner join UserMst on(SubjectMasterHistory.iModifyBy = UserMst.iUserId)" +
                                " inner join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)" +
                                " where vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "' and SubjectMasterHistory.vColumnName like  '" + "%" + ColumnName + "%" + "'" +
                                " order by dModifyon "
                }
                else if (ColumnName == "cSex") {
                    obj.query =
                            // "select case when SubjectMasterHistory." + ColumnName + " = 'M' then 'Male' else 'Female' End  as  vChangedValue, vUpdateRemarks as  vRemarks, UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName,SubjectMasterHistory.dModifyOn" +
                            // " from SubjectMasterHistory" +
                            // " inner join UserMst on(SubjectMasterHistory.iModifyBy = UserMst.iUserId)" +
                            // " inner join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)" +
                            // " where vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'and SubjectMasterHistory.iTranNo = 1 " +


                    " select case when ISNULL(SubjectMasterHistory." + ColumnName + ",'') = 'M' then 'Male' else 'Female' End  as  vChangedValue,  " +
                    " '' as  vRemarks, " +
                    " isnull(UserMst.vUserName, UserMst_subject.vusername) +'('+ isnull(UserTypeMst.vUserTypeName,UserTypeMst_subject.vUserTypeName) +')' as UserName, " +
                    " isnull(SubjectMasterHistory.dModifyOn,SubjectMaster.dmodifyon) as dModifyOn " +
                    " from  SubjectMaster " +
                    " inner join UserMst UserMst_subject on(SubjectMaster.iModifyBy = UserMst_subject.iUserId)  " +
                    " inner join UserTypeMst UserTypeMst_subject on (UserMst_subject.vUserTypeCode=UserTypeMst_subject.vUserTypeCode)  " +
                    " left join  " +
                    " (select SubjectMasterHistory.vsubjectid,SubjectMasterHistory.itranno, " +
                    " SubjectMasterHistory." + ColumnName + ",SubjectMasterHistory.vupdateremarks, " +
                    " SubjectMasterHistory.iModifyBy,SubjectMasterHistory.dModifyOn " +
                    " from SubjectMasterHistory " +
                    " inner join " +
                    " (select  " +
                    " max(itranno) as itranno,vsubjectid from SubjectMasterHistory " +
                    "  where  vColumnName IS NULL AND vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +
                    "  group by vsubjectid) SubjectMasterHistory_latest on " +
                    "   SubjectMasterHistory_latest.vsubjectid = SubjectMasterHistory.vsubjectid " +
                    "  and SubjectMasterHistory_latest.itranno = SubjectMasterHistory.itranno " +
                    "  ) SubjectMasterHistory  on " +
                    "  SubjectMasterHistory.vsubjectid = SubjectMaster.vsubjectid " +

                    " left join UserMst on(SubjectMasterHistory.iModifyBy = UserMst.iUserId)  " +
                    " left join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)  " +
                    " where  SubjectMaster.vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +
                            " UNION  " +

                        "select case when SubjectMasterHistory." + ColumnName + " = 'M' then 'Male' else 'Female' End  as  vChangedValue, vUpdateRemarks as  vRemarks, UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName,SubjectMasterHistory.dModifyOn" +
                            " from SubjectMasterHistory" +
                            " inner join UserMst on(SubjectMasterHistory.iModifyBy = UserMst.iUserId)" +
                            " inner join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)" +
                            " where vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'and SubjectMasterHistory.vColumnName like '" + "%" + ColumnName + "%" + "'" +
                            " order by dModifyon "
                }
                else {
                    obj.query =
                                  //"select SubjectMasterHistory." + ColumnName + " as  vChangedValue, vUpdateRemarks as  vRemarks, UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName,SubjectMasterHistory.dModifyOn" +
                                  //" from SubjectMasterHistory" +
                                  //" inner join UserMst on(SubjectMasterHistory.iModifyBy = UserMst.iUserId)" +
                                  //" inner join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)" +
                                  //" where vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "' and SubjectMasterHistory.iTranNo = 1 " +

                    " select " +
                    " ISNULL(SubjectMasterHistory." + ColumnName + ",'') as  vChangedValue , " +
                    " '' as  vRemarks, " +
                    " isnull(UserMst.vUserName, UserMst_subject.vusername) +'('+ isnull(UserTypeMst.vUserTypeName,UserTypeMst_subject.vUserTypeName) +')' as UserName, " +
                    " isnull(SubjectMasterHistory.dModifyOn,SubjectMaster.dmodifyon) as dModifyOn " +
                    " from  SubjectMaster " +
                    " inner join UserMst UserMst_subject on(SubjectMaster.iModifyBy = UserMst_subject.iUserId)  " +
                    " inner join UserTypeMst UserTypeMst_subject on (UserMst_subject.vUserTypeCode=UserTypeMst_subject.vUserTypeCode)  " +
                    " left join  " +
                    " (select SubjectMasterHistory.vsubjectid,SubjectMasterHistory.itranno, " +
                    " SubjectMasterHistory." + ColumnName + ",SubjectMasterHistory.vupdateremarks, " +
                    " SubjectMasterHistory.iModifyBy,SubjectMasterHistory.dModifyOn " +
                    " from SubjectMasterHistory " +
                    " inner join " +
                    " (select  " +
                    " max(itranno) as itranno,vsubjectid from SubjectMasterHistory " +
                    "  where  vColumnName IS NULL AND vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +
                    "  group by vsubjectid) SubjectMasterHistory_latest on " +
                    "   SubjectMasterHistory_latest.vsubjectid = SubjectMasterHistory.vsubjectid " +
                    "  and SubjectMasterHistory_latest.itranno = SubjectMasterHistory.itranno " +
                    "  ) SubjectMasterHistory  on " +
                    "  SubjectMasterHistory.vsubjectid = SubjectMaster.vsubjectid " +

                    " left join UserMst on(SubjectMasterHistory.iModifyBy = UserMst.iUserId)  " +
                    " left join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)  " +
                    " where  SubjectMaster.vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +

                 " union " +

          "select SubjectMasterHistory." + ColumnName + " as  vChangedValue, vUpdateRemarks as  vRemarks, UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName,SubjectMasterHistory.dModifyOn" +
                    " from SubjectMasterHistory" +
                    " inner join UserMst on(SubjectMasterHistory.iModifyBy = UserMst.iUserId)" +
                    " inner join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)" +
                    " where vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "' and SubjectMasterHistory.vColumnName like  '" + "%" + ColumnName + "%" + "'" +
                    " order by dModifyon "

                }
    }
            if (TableName == 'SubjectDetails') {

                if (ColumnName == "cMaritalStatus") {
                    obj.query =
                           // "select case when " + ColumnName + " = 'S' then 'Single' else 'Married' End  as  vChangedValue, vUpdateRemarks as  vRemarks, UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName,SubjectDetailsHistory.dModifyOn" +
                           // " from SubjectDetailsHistory" +
                           // " inner join UserMst on(SubjectDetailsHistory.iModifyBy = UserMst.iUserId)" +
                           // " inner join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)" +
                           // " where vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'and SubjectDetailsHistory.iTranNo = 1 " +
                    " select case when ISNULL(SUbjectDetailsHIstory." + ColumnName + ",'') = 'S' then 'Single' else 'Married' End  as  vChangedValue,  " +
                    " '' as  vRemarks, " +
                    "  isnull(UserMst.vUserName, UserMst_subject.vusername) +'('+ isnull(UserTypeMst.vUserTypeName,UserTypeMst_subject.vUserTypeName) +')' as UserName, " +
                    "  isnull(SubjectDetailsHistory.dModifyOn,SubjectDetails.dmodifyon) as dModifyOn " +
                    "    from  SubjectDetails " +
                    "  inner join UserMst UserMst_subject on(SubjectDetails.iModifyBy = UserMst_subject.iUserId)  " +
                    " inner join UserTypeMst UserTypeMst_subject on (UserMst_subject.vUserTypeCode=UserTypeMst_subject.vUserTypeCode)  " +
                    "INNER JOIN SubjectMaster ON SubjectMaster.vSubjectId =   SubjectDetails.vSubjectId" +
                    " left join  " +
                    " (select SubjectDetailsHistory.vsubjectid,SubjectDetailsHistory.itranno, " +
                    " SubjectDetailsHistory." + ColumnName + ",SubjectDetailsHistory.vupdateremarks, " +
                    " SubjectDetailsHistory.iModifyBy,SubjectDetailsHistory.dModifyOn " +
                    "  from SubjectDetailsHistory " +
                    "  inner join " +
                    "  (select  " +
                    "  max(itranno) as itranno,vsubjectid from SUbjectDetailsHIstory " +
                    "  where  vColumnName IS NULL AND vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +
                    "  group by vsubjectid)   " +
                    "  SubjectDetailsHistory_latest on " +
                    "   SubjectDetailsHistory_latest.vsubjectid = SUbjectDetailsHIstory.vsubjectid " +
                    "  and SubjectDetailsHistory_latest.itranno = SUbjectDetailsHIstory.itranno " +
                    "  ) SubjectDetailsHistory  on " +
                    "  SubjectDetailsHistory.vsubjectid = SubjectDetails.vsubjectid " +

                    "  left join  " +
                    " (select SubjectMasterHistory.vsubjectid,SubjectMasterHistory.itranno, " +
                    " SubjectMasterHistory.vupdateremarks, " +
                    " SubjectMasterHistory.iModifyBy,SubjectMasterHistory.dModifyOn " +
                    " from SubjectMasterHistory " +
                    " inner join " +
                    " (select " +
                    " max(itranno) as itranno,vsubjectid from SubjectMasterHistory " +
                    "  where  vColumnName IS NULL AND vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +
                    "  group by vsubjectid) SubjectMasterHistory_latest on " +
                    "   SubjectMasterHistory_latest.vsubjectid = SubjectMasterHistory.vsubjectid " +
                    "  and SubjectMasterHistory_latest.itranno = SubjectMasterHistory.itranno " +
                    "  ) SubjectMasterHistory  on " +
                    "  SubjectMasterHistory.vsubjectid = SubjectDetails.vsubjectid " +

                    " left join UserMst on(SubjectDetailsHistory.iModifyBy = UserMst.iUserId)  " +
                    " left join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)  " +
                    " where  SubjectDetails.vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +
                            " UNION  " +
                        "select case when " + ColumnName + " = 'S' then 'Single' else 'Married' End  as  vChangedValue, vUpdateRemarks as  vRemarks, UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName,SubjectDetailsHistory.dModifyOn" +
                            " from SubjectDetailsHistory" +
                            " inner join UserMst on(SubjectDetailsHistory.iModifyBy = UserMst.iUserId)" +
                            " inner join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)" +
                            " where vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'and SubjectDetailsHistory.vColumnName like '" + "%" + ColumnName + "%" + "'" +
                            " order by dModifyon "
                }
                else {
                    obj.query =
                         // "select " + ColumnName + " as  vChangedValue, vUpdateRemarks as  vRemarks, UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName,SubjectDetailsHistory.dModifyOn" +
                         // " from SubjectDetailsHistory" +
                         // " inner join UserMst on(SubjectDetailsHistory.iModifyBy = UserMst.iUserId)" +
                         // " inner join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)" +
                         // " where vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'and SubjectDetailsHistory.iTranNo = 1 " +

                         " select  SUbjectDetailsHIstory." + ColumnName + " as  vChangedValue, " +
                    " '' as  vRemarks, " +
                    "  isnull(UserMst.vUserName, UserMst_subject.vusername) +'('+ isnull(UserTypeMst.vUserTypeName,UserTypeMst_subject.vUserTypeName) +')' as UserName, " +
                    "  isnull(SubjectDetailsHistory.dModifyOn,SubjectDetails.dmodifyon) as dModifyOn " +
                    "    from  SubjectDetails " +
                    "  inner join UserMst UserMst_subject on(SubjectDetails.iModifyBy = UserMst_subject.iUserId)  " +
                    " inner join UserTypeMst UserTypeMst_subject on (UserMst_subject.vUserTypeCode=UserTypeMst_subject.vUserTypeCode)  " +
                    " INNER JOIN SubjectMaster ON SubjectMaster.vSubjectId =   SubjectDetails.vSubjectId " +
                    " left join  " +
                    " (select SubjectDetailsHistory.vsubjectid,SubjectDetailsHistory.itranno, " +
                    " SubjectDetailsHistory." + ColumnName + ",SubjectDetailsHistory.vupdateremarks, " +
                    " SubjectDetailsHistory.iModifyBy,SubjectDetailsHistory.dModifyOn " +
                    "  from SubjectDetailsHistory " +
                    "  inner join " +
                    "  (select  " +
                    "  max(itranno) as itranno,vsubjectid from SUbjectDetailsHIstory " +
                    "  where  vColumnName IS NULL AND vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +
                    "  group by vsubjectid)   " +
                    "  SubjectDetailsHistory_latest on " +
                    "   SubjectDetailsHistory_latest.vsubjectid = SUbjectDetailsHIstory.vsubjectid " +
                    "  and SubjectDetailsHistory_latest.itranno = SUbjectDetailsHIstory.itranno " +
                    "  ) SubjectDetailsHistory  on " +
                    "  SubjectDetailsHistory.vsubjectid = SubjectDetails.vsubjectid " +

                    "  left join  " +
                    " (select SubjectMasterHistory.vsubjectid,SubjectMasterHistory.itranno, " +
                    " SubjectMasterHistory.vupdateremarks, " +
                    " SubjectMasterHistory.iModifyBy,SubjectMasterHistory.dModifyOn " +
                    " from SubjectMasterHistory " +
                    " inner join " +
                    " (select " +
                    " max(itranno) as itranno,vsubjectid from SubjectMasterHistory " +
                    "  where  vColumnName IS NULL AND vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +
                    "  group by vsubjectid) SubjectMasterHistory_latest on " +
                    "   SubjectMasterHistory_latest.vsubjectid = SubjectMasterHistory.vsubjectid " +
                    "  and SubjectMasterHistory_latest.itranno = SubjectMasterHistory.itranno " +
                    "  ) SubjectMasterHistory  on " +
                    "  SubjectMasterHistory.vsubjectid = SubjectDetails.vsubjectid " +

                    " left join UserMst on(SubjectDetailsHistory.iModifyBy = UserMst.iUserId)  " +
                    " left join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)  " +
                    " where  SubjectDetails.vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +
                    " UNION  " +

                        "select " + ColumnName + " as  vChangedValue, vUpdateRemarks as  vRemarks, UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName,SubjectDetailsHistory.dModifyOn" +
                         " from SubjectDetailsHistory" +
                         " inner join UserMst on(SubjectDetailsHistory.iModifyBy = UserMst.iUserId)" +
                         " inner join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)" +
                         " where vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'and SubjectDetailsHistory.vColumnName like '" + "%" + ColumnName + "%" + "'" +
                         " order by dModifyon "
                }

            }
            if (TableName == 'SubjectContactDetails') {
                if (ColumnName == 'vLocalAdd1,vLocalAdd12,vLocalAdd13') {
                    obj.query =
                           //  "select (vLocalAdd1+','+vLocalAdd12+','+vLocalAdd13) as  vChangedValue, vUpdateRemarks as  vRemarks, UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName,SubjectContactDetailsHistory.dModifyOn" +
                           //  " from SubjectContactDetailsHistory" +
                           //  " inner join UserMst on(SubjectContactDetailsHistory.iModifyBy = UserMst.iUserId)" +
                           //  " inner join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)" +
                           //  " where vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "' and SubjectContactDetailsHistory.iTranNo = 1" +


                    "   select (ISNULL(SubjectContactDetailsHistory.vLocalAdd1,'')+','+ISNULL(SubjectContactDetailsHistory.vLocalAdd12,'')+','+ISNULL(SubjectContactDetailsHistory.vLocalAdd13,'')) as  vChangedValue,  " +
                    "   '' as  vRemarks,  " +
                    "   isnull(UserMst.vUserName, UserMst_subject.vusername) +'('+ isnull(UserTypeMst.vUserTypeName,UserTypeMst_subject.vUserTypeName) +')' as UserName, " +
                    "   ISNULL(SubjectContactDetailsHistory.dModifyOn,SubjectContactDetails.dModifyOn) as dModifyOn " +
                    "    from  SubjectContactDetails " +
                    "  inner join UserMst UserMst_subject on(SubjectContactDetails.iModifyBy = UserMst_subject.iUserId)  " +
                    " inner join UserTypeMst UserTypeMst_subject on (UserMst_subject.vUserTypeCode=UserTypeMst_subject.vUserTypeCode)  " +
                    " INNER JOIn SubjectMaster on SubjectMaster.vSUbjectId =  SubjectContactDetails.vSubjectId " +
                    " left join  " +
                    " (select SubjectContactDetailsHistory.vSubjectId,SubjectContactDetailsHistory.itranno, " +
                    " SubjectContactDetailsHistory.vLocalAdd1,SubjectContactDetailsHistory.vLocalAdd12,SubjectContactDetailsHistory.vLocalAdd13, " +
                    " SubjectContactDetailsHistory.vupdateremarks, " +
                    " SubjectContactDetailsHistory.iModifyBy,SubjectContactDetailsHistory.dModifyOn " +
                    "  from SubjectContactDetailsHistory " +
                    "  inner join " +
                    "  (select  " +
                    "  max(itranno) as itranno,vsubjectid from SubjectContactDetailsHistory " +
                    "  where  vColumnName IS NULL AND vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +
                    "  group by vsubjectid)   " +
                    "  SubjectContactDetailsHistory_latest on " +
                    "   SubjectContactDetailsHistory_latest.vsubjectid = SubjectContactDetailsHistory.vsubjectid " +
                    "  and SubjectContactDetailsHistory_latest.itranno = SubjectContactDetailsHistory.itranno " +
                    "  ) SubjectContactDetailsHistory  on " +
                    "  SubjectContactDetailsHistory.vSubjectId = SubjectContactDetails.vSubjectId " +

                    "  left join  " +
                    " (select SubjectMasterHistory.vsubjectid,SubjectMasterHistory.itranno, " +
                    " SubjectMasterHistory.vupdateremarks, " +
                    " SubjectMasterHistory.iModifyBy,SubjectMasterHistory.dModifyOn " +
                    " from SubjectMasterHistory " +
                    " inner join " +
                    " (select  " +
                    " max(itranno) as itranno,vsubjectid from SubjectMasterHistory " +
                    "  where  vColumnName IS NULL AND vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +
                    "  group by vsubjectid) SubjectMasterHistory_latest on " +
                    "   SubjectMasterHistory_latest.vsubjectid = SubjectMasterHistory.vsubjectid " +
                    "  and SubjectMasterHistory_latest.itranno = SubjectMasterHistory.itranno " +
                    "  ) SubjectMasterHistory  on " +
                    "  SubjectMasterHistory.vsubjectid = SubjectContactDetails.vsubjectid " +

                    " left join UserMst on(SubjectContactDetailsHistory.iModifyBy = UserMst.iUserId)  " +
                    " left join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)  " +
                    " where  SubjectContactDetails.vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +


                            " UNION    " +
                        "select (vLocalAdd1+','+vLocalAdd12+','+vLocalAdd13) as  vChangedValue, vUpdateRemarks as  vRemarks, UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName,SubjectContactDetailsHistory.dModifyOn" +
                            " from SubjectContactDetailsHistory" +
                            " inner join UserMst on(SubjectContactDetailsHistory.iModifyBy = UserMst.iUserId)" +
                            " inner join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)" +
                            " where vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "' and SubjectContactDetailsHistory.vColumnName like '" + "%" + ColumnName + "%" + "'" +
                            " order by dModifyon "

                }
                else if (ColumnName == 'vLocalAdd21,vLocalAdd22,vLocalAdd23') {
                    obj.query =
                            // "select (vLocalAdd21+','+vLocalAdd22+','+vLocalAdd23) as  vChangedValue, vUpdateRemarks as  vRemarks, UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName,SubjectContactDetailsHistory.dModifyOn" +
                            // " from SubjectContactDetailsHistory" +
                            // " inner join UserMst on(SubjectContactDetailsHistory.iModifyBy = UserMst.iUserId)" +
                            // " inner join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)" +
                            // " where vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "' and SubjectContactDetailsHistory.iTranNo = 1 " +
                            // " UNION ALL " +

                    "   select (ISNULL(SubjectContactDetailsHistory.vLocalAdd21,'')+','+ISNULL(SubjectContactDetailsHistory.vLocalAdd22,'')+','+ISNULL(SubjectContactDetailsHistory.vLocalAdd23,'')) as  vChangedValue,  " +
                    "   ''  as  vRemarks,  " +
                    "   isnull(UserMst.vUserName, UserMst_subject.vusername) +'('+ isnull(UserTypeMst.vUserTypeName,UserTypeMst_subject.vUserTypeName) +')' as UserName, " +
                    "   ISNULL(SubjectContactDetailsHistory.dModifyOn,SubjectContactDetails.dModifyOn) as dModifyOn " +
                    "    from  SubjectContactDetails " +
                    "  inner join UserMst UserMst_subject on(SubjectContactDetails.iModifyBy = UserMst_subject.iUserId)  " +
                    " inner join UserTypeMst UserTypeMst_subject on (UserMst_subject.vUserTypeCode=UserTypeMst_subject.vUserTypeCode)  " +
                    " INNER JOIn SubjectMaster on SubjectMaster.vSUbjectId =  SubjectContactDetails.vSubjectId " +
                    " left join  " +
                    " (select SubjectContactDetailsHistory.vSubjectId,SubjectContactDetailsHistory.itranno, " +
                    " SubjectContactDetailsHistory.vLocalAdd21,SubjectContactDetailsHistory.vLocalAdd22,SubjectContactDetailsHistory.vLocalAdd23, " +
                    " SubjectContactDetailsHistory.vupdateremarks, " +
                    " SubjectContactDetailsHistory.iModifyBy,SubjectContactDetailsHistory.dModifyOn " +
                    "  from SubjectContactDetailsHistory " +
                    "  inner join " +
                    "  (select  " +
                    "  max(itranno) as itranno,vsubjectid from SubjectContactDetailsHistory " +
                    "  where  vColumnName IS NULL AND vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +
                    "  group by vsubjectid)   " +
                    "  SubjectContactDetailsHistory_latest on " +
                    "   SubjectContactDetailsHistory_latest.vsubjectid = SubjectContactDetailsHistory.vsubjectid " +
                    "  and SubjectContactDetailsHistory_latest.itranno = SubjectContactDetailsHistory.itranno " +
                    "  ) SubjectContactDetailsHistory  on " +
                    "  SubjectContactDetailsHistory.vSubjectId = SubjectContactDetails.vSubjectId " +

                    "  left join  " +
                    " (select SubjectMasterHistory.vsubjectid,SubjectMasterHistory.itranno, " +
                    " SubjectMasterHistory.vupdateremarks, " +
                    " SubjectMasterHistory.iModifyBy,SubjectMasterHistory.dModifyOn " +
                    " from SubjectMasterHistory " +
                    " inner join " +
                    " (select  " +
                    " max(itranno) as itranno,vsubjectid from SubjectMasterHistory " +
                    "  where  vColumnName IS NULL AND vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +
                    "  group by vsubjectid) SubjectMasterHistory_latest on " +
                    "   SubjectMasterHistory_latest.vsubjectid = SubjectMasterHistory.vsubjectid " +
                    "  and SubjectMasterHistory_latest.itranno = SubjectMasterHistory.itranno " +
                    "  ) SubjectMasterHistory  on " +
                    "  SubjectMasterHistory.vsubjectid = SubjectContactDetails.vsubjectid " +

                    " left join UserMst on(SubjectContactDetailsHistory.iModifyBy = UserMst.iUserId)  " +
                    " left join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)  " +
                    " where  SubjectContactDetails.vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +

                            "UNION  " +

                        "select (vLocalAdd21+','+vLocalAdd22+','+vLocalAdd23) as  vChangedValue, vUpdateRemarks as  vRemarks, UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName,SubjectContactDetailsHistory.dModifyOn" +
                            " from SubjectContactDetailsHistory" +
                            " inner join UserMst on(SubjectContactDetailsHistory.iModifyBy = UserMst.iUserId)" +
                            " inner join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)" +
                            " where vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "' and SubjectContactDetailsHistory.vColumnName like '" + "%" + ColumnName + "%" + "'" +
                            " order by dModifyon "

                }
                else if (ColumnName == 'vPerAdd1,vPerAdd2,vPerAdd3') {
                    obj.query =
                            // "select (vPerAdd1+','+vPerAdd2+','+vPerAdd3) as  vChangedValue, vUpdateRemarks as  vRemarks, UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName,SubjectContactDetailsHistory.dModifyOn" +
                            // " from SubjectContactDetailsHistory" +
                            // " inner join UserMst on(SubjectContactDetailsHistory.iModifyBy = UserMst.iUserId)" +
                            // " inner join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)" +
                            // " where vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "' and SubjectContactDetailsHistory.iTranNo = 1 " +
                     "   select (ISNULL(SubjectContactDetailsHistory.vPerAdd1,'')+','+ISNULL(SubjectContactDetailsHistory.vPerAdd2,'')+','+ISNULL(SubjectContactDetailsHistory.vPerAdd3,'')) as  vChangedValue,  " +
                    "   '' as  vRemarks,  " +
                    "   isnull(UserMst.vUserName, UserMst_subject.vusername) +'('+ isnull(UserTypeMst.vUserTypeName,UserTypeMst_subject.vUserTypeName) +')' as UserName, " +
                    "   ISNULL(SubjectContactDetailsHistory.dModifyOn,SubjectContactDetails.dModifyOn) as dModifyOn " +
                    "    from  SubjectContactDetails " +
                    "  inner join UserMst UserMst_subject on(SubjectContactDetails.iModifyBy = UserMst_subject.iUserId)  " +
                    " inner join UserTypeMst UserTypeMst_subject on (UserMst_subject.vUserTypeCode=UserTypeMst_subject.vUserTypeCode)  " +
                    " INNER JOIn SubjectMaster on SubjectMaster.vSUbjectId =  SubjectContactDetails.vSubjectId " +
                    " left join  " +
                    " (select SubjectContactDetailsHistory.vSubjectId,SubjectContactDetailsHistory.itranno, " +
                    " SubjectContactDetailsHistory.vPerAdd1,SubjectContactDetailsHistory.vPerAdd2,SubjectContactDetailsHistory.vPerAdd3, " +
                    " SubjectContactDetailsHistory.vupdateremarks, " +
                    " SubjectContactDetailsHistory.iModifyBy,SubjectContactDetailsHistory.dModifyOn " +
                    "  from SubjectContactDetailsHistory " +
                    "  inner join " +
                    "  (select  " +
                    "  max(itranno) as itranno,vsubjectid from SubjectContactDetailsHistory " +
                    "  where  vColumnName IS NULL AND vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +
                    "  group by vsubjectid)   " +
                    "  SubjectContactDetailsHistory_latest on " +
                    "   SubjectContactDetailsHistory_latest.vsubjectid = SubjectContactDetailsHistory.vsubjectid " +
                    "  and SubjectContactDetailsHistory_latest.itranno = SubjectContactDetailsHistory.itranno " +
                    "  ) SubjectContactDetailsHistory  on " +
                    "  SubjectContactDetailsHistory.vSubjectId = SubjectContactDetails.vSubjectId " +

                    "  left join  " +
                    " (select SubjectMasterHistory.vsubjectid,SubjectMasterHistory.itranno, " +
                    " SubjectMasterHistory.vupdateremarks, " +
                    " SubjectMasterHistory.iModifyBy,SubjectMasterHistory.dModifyOn " +
                    " from SubjectMasterHistory " +
                    " inner join " +
                    " (select  " +
                    " max(itranno) as itranno,vsubjectid from SubjectMasterHistory " +
                    "  where  vColumnName IS NULL AND vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +
                    "  group by vsubjectid) SubjectMasterHistory_latest on " +
                    "   SubjectMasterHistory_latest.vsubjectid = SubjectMasterHistory.vsubjectid " +
                    "  and SubjectMasterHistory_latest.itranno = SubjectMasterHistory.itranno " +
                    "  ) SubjectMasterHistory  on " +
                    "  SubjectMasterHistory.vsubjectid = SubjectContactDetails.vsubjectid " +

                    " left join UserMst on(SubjectContactDetailsHistory.iModifyBy = UserMst.iUserId)  " +
                    " left join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)  " +
                    " where  SubjectContactDetails.vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +

                            "UNION  " +
                        "select (vPerAdd1+','+vPerAdd2+','+vPerAdd3) as  vChangedValue, vUpdateRemarks as  vRemarks, UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName,SubjectContactDetailsHistory.dModifyOn" +
                            " from SubjectContactDetailsHistory" +
                            " inner join UserMst on(SubjectContactDetailsHistory.iModifyBy = UserMst.iUserId)" +
                            " inner join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)" +
                            " where vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "' and SubjectContactDetailsHistory.vColumnName like '" + "%" + ColumnName + "%" + "'" +
                            " order by dModifyon "

                }
                else {
                    obj.query =
                             // "select " + ColumnName + " as  vChangedValue, vUpdateRemarks as  vRemarks, UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName,SubjectContactDetailsHistory.dModifyOn" +
                             // " from SubjectContactDetailsHistory" +
                             // " inner join UserMst on(SubjectContactDetailsHistory.iModifyBy = UserMst.iUserId)" +
                             // " inner join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)" +
                             // " where vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "' and SubjectContactDetailsHistory.iTranNo = 1 " +
                    "   select ISNULL(SubjectContactDetailsHistory." + ColumnName + ",'') as  vChangedValue,  " +
                    "   ''  as  vRemarks,  " +
                    "   isnull(UserMst.vUserName, UserMst_subject.vusername) +'('+ isnull(UserTypeMst.vUserTypeName,UserTypeMst_subject.vUserTypeName) +')' as UserName, " +
                    "   ISNULL(SubjectContactDetailsHistory.dModifyOn,SubjectContactDetails.dModifyOn) as dModifyOn " +

                    "    from  SubjectContactDetails " +
                    "  inner join UserMst UserMst_subject on(SubjectContactDetails.iModifyBy = UserMst_subject.iUserId)  " +
                    " inner join UserTypeMst UserTypeMst_subject on (UserMst_subject.vUserTypeCode=UserTypeMst_subject.vUserTypeCode)  " +
                    " INNER JOIn SubjectMaster on SubjectMaster.vSUbjectId =  SubjectContactDetails.vSubjectId " +
                    " left join  " +
                    " (select SubjectContactDetailsHistory.vSubjectId,SubjectContactDetailsHistory.itranno, " +
                    " SubjectContactDetailsHistory." + ColumnName + ", " +
                    " SubjectContactDetailsHistory.vupdateremarks, " +
                    " SubjectContactDetailsHistory.iModifyBy,SubjectContactDetailsHistory.dModifyOn " +
                    "  from SubjectContactDetailsHistory " +
                    "  inner join " +
                    "  (select  " +
                    "  max(itranno) as itranno,vsubjectid from SubjectContactDetailsHistory " +
                    "  where  vColumnName IS NULL AND vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +
                    "  group by vsubjectid)   " +
                    "  SubjectContactDetailsHistory_latest on " +
                    "   SubjectContactDetailsHistory_latest.vsubjectid = SubjectContactDetailsHistory.vsubjectid " +
                    "  and SubjectContactDetailsHistory_latest.itranno = SubjectContactDetailsHistory.itranno " +
                    "  ) SubjectContactDetailsHistory  on " +
                    "  SubjectContactDetailsHistory.vSubjectId = SubjectContactDetails.vSubjectId " +

                    "  left join  " +
                    " (select SubjectMasterHistory.vsubjectid,SubjectMasterHistory.itranno, " +
                    " SubjectMasterHistory.vupdateremarks, " +
                    " SubjectMasterHistory.iModifyBy,SubjectMasterHistory.dModifyOn " +
                    " from SubjectMasterHistory " +
                    " inner join " +
                    " (select  " +
                    " max(itranno) as itranno,vsubjectid from SubjectMasterHistory " +
                    "  where  vColumnName IS NULL AND vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +
                    "  group by vsubjectid) SubjectMasterHistory_latest on " +
                    "   SubjectMasterHistory_latest.vsubjectid = SubjectMasterHistory.vsubjectid " +
                    "  and SubjectMasterHistory_latest.itranno = SubjectMasterHistory.itranno " +
                    "  ) SubjectMasterHistory  on " +
                    "  SubjectMasterHistory.vsubjectid = SubjectContactDetails.vsubjectid  " +

                    " left join UserMst on(SubjectContactDetailsHistory.iModifyBy = UserMst.iUserId)  " +
                    " left join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)  " +
                    " where  SubjectContactDetails.vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +
                             " UNION " +

                        "select " + ColumnName + " as  vChangedValue, vUpdateRemarks as  vRemarks, UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName,SubjectContactDetailsHistory.dModifyOn" +
                             " from SubjectContactDetailsHistory" +
                             " inner join UserMst on(SubjectContactDetailsHistory.iModifyBy = UserMst.iUserId)" +
                             " inner join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)" +
                             " where vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "' and SubjectContactDetailsHistory.vColumnName like '" + "%" + ColumnName + "%" + "'" +
                             " order by dModifyon "
                }
    }
            if (TableName == 'SubjectProofDetails') {

                obj.query = "Select ISNULL(SubjectProofDetails.vProofType,SubjectMaster.vProofOfAge1) as vChangedValue , " +
                " ISNULL (SUbjectProofDetails.vUpdateRemarks,'') as  vRemarks, " +
                " isnull(UserMst.vUserName,UserMst_subject.vusername) +'('+ isnull(UserTypeMst.vUserTypeName,UserTypeMst_subject.vUserTypeName) +')' as UserName, " +
                " isnull(SubjectProofDetails.dModifyOn,SubjectMaster.dmodifyon) as dModifyOn  " +
                " from  SubjectProofDetails  " +
                " INNER join UserMst on(SubjectProofDetails.iModifyBy = UserMst.iUserId)  " +
                " INNER join UserTypeMst  on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode) " +
                " RIGHT JOIN SubjectMaster   " +
                " ON  SubjectMaster.vSubjectId = SubjectProofDetails.vSubjectId  " +
                " LEFT join  " +
                "       (  " +
                "       select SubjectMasterHistory.vsubjectid,SubjectMasterHistory.itranno,  " +
                "        SubjectMasterHistory.vProofOfAge1,SubjectMasterHistory.vupdateremarks,  " +
                "        SubjectMasterHistory.iModifyBy,SubjectMasterHistory.dModifyOn   " +
                " from SubjectMasterHistory    " +
                " inner join  " +
                " (select   " +
                " max(itranno) as itranno,vsubjectid from SubjectMasterHistory  " +
                " where  vColumnName IS NULL AND vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'    " +
                " group by vsubjectid) SubjectMasterHistory_latest on  " +
                " SubjectMasterHistory_latest.vsubjectid = SubjectMasterHistory.vsubjectid  " +
                " and SubjectMasterHistory_latest.itranno = SubjectMasterHistory.itranno    " +
                " ) SubjectMasterHistory  on  " +
                " SubjectMasterHistory.vsubjectid = SubjectMaster.vsubjectid    " +
                " Left join UserMst UserMst_subject on(SubjectMasterHistory.iModifyBy = UserMst_subject.iUserId)   " +
                " Left join UserTypeMst UserTypeMst_subject on (UserMst_subject.vUserTypeCode=UserTypeMst_subject.vUserTypeCode)   " +
                " where  SubjectMaster.vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "' " +
                "order by dModifyon  "





                // "Select vProofType as vChangedValue ,vUpdateRemarks as vRemarks , UM.vUserName as UserName,SPD.dModifyOn from SubjectProofDetails SPD" +
                // " INNER JOIN UserMst UM ON UM.iUserId = SPD.iModifyBy" +
                // " Where vSubjectId = '" + $('#<%= HSubjectId.clientid %>').val() + "' "
                //+ // 
                //" UNION " +
                //"Select NULL as vChangedValue , NULL as vRemarks , UM.vUserName as UserName,SubjectMaster.dModifyOn  From " +
                //"SubjectMaster " +
                //"INNER JOIN UserMst UM ON UM.iUserId = SubjectMaster.iModifyBy " +
                //" Where vSubjectId = '" + $('#<%= HSubjectId.clientid %>').val() + "' " +



            }
            if (TableName == 'SubjectFemaleDetails') {
                if (ColumnName == "cRegular" || ColumnName == "cChildrenHelath" || ColumnName == "cAbortions" || ColumnName == "cLoctating" || ColumnName == "cIsVolunteerinBearingAge") {
                    obj.query =
                              //  "select case when " + ColumnName + " ='N' then 'No' When " + ColumnName + " ='Y' then 'Yes' when " + ColumnName + " ='A' then 'NA' End as vChangedValue, vUpdateRemarks as  vRemarks, UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName,SubjectFemaleDetailsHistory.dModifyOn" +
                              //  " from SubjectFemaleDetailsHistory" +
                              //  " inner join UserMst on(SubjectFemaleDetailsHistory.iModifyBy = UserMst.iUserId)" +
                              //  " inner join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)" +
                              //  " where vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "' and SubjectFemaleDetailsHistory.iTranNo = 1 " +

                     " select case when ISNULL(SubjectFemaleDetailsHistory." + ColumnName + ",NULL) ='N' then 'No' When ISNULL(SubjectFemaleDetailsHistory." + ColumnName + ",NULL) ='Y' then 'Yes' when  " +
                    " ISNULL(SubjectFemaleDetailsHistory." + ColumnName + ",NULL) ='A' then 'NA' End as vChangedValue, " +
                    " ''  as  vRemarks, " +
                    " UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName, " +
                    " SubjectFemaleDetailsHistory.dModifyOn " +
                    "   from  SubjectFemaleDetails " +
                    " inner join UserMst UserMst_subject on(SubjectFemaleDetails.iModifyBy = UserMst_subject.iUserId)  " +
                    "inner join UserTypeMst UserTypeMst_subject on (UserMst_subject.vUserTypeCode=UserTypeMst_subject.vUserTypeCode)  " +
                    " INNER Join SubjectMaster On SubjectMaster.vSubjectId = SubjectFemaleDetails.vSubjectId " +
                    "left join  " +
                    "(select SubjectFemaleDetailsHistory.vSubjectId,SubjectFemaleDetailsHistory.itranno, " +
                    "SubjectFemaleDetailsHistory." + ColumnName + ", " +
                    "SubjectFemaleDetailsHistory.vupdateremarks, " +
                    "SubjectFemaleDetailsHistory.iModifyBy,SubjectFemaleDetailsHistory.dModifyOn " +
                    " from SubjectFemaleDetailsHistory " +
                    " inner join " +
                    " (select  " +
                    " max(itranno) as itranno,vsubjectid from SubjectFemaleDetailsHistory " +
                    " where ( vColumnName IS NULL OR vColumnName  ='') AND vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +
                    " group by vsubjectid)   " +
                    " SubjectFemaleDetailsHistory_latest on " +
                    "  SubjectFemaleDetailsHistory_latest.vsubjectid = SubjectFemaleDetailsHistory.vsubjectid " +
                    " and SubjectFemaleDetailsHistory_latest.itranno = SubjectFemaleDetailsHistory.itranno " +
                    " ) SubjectFemaleDetailsHistory  on " +
                    " SubjectFemaleDetailsHistory.vSubjectId = SubjectFemaleDetails.vSubjectId " +

                    " left join  " +
                    "(select SubjectMasterHistory.vsubjectid,SubjectMasterHistory.itranno, " +
                    "SubjectMasterHistory.vupdateremarks, " +
                    "SubjectMasterHistory.iModifyBy,SubjectMasterHistory.dModifyOn " +
                    "from SubjectMasterHistory " +
                    "inner join " +
                    "(select  " +
                    "max(itranno) as itranno,vsubjectid from SubjectMasterHistory " +
                    " where  vColumnName IS NULL AND vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +
                    " group by vsubjectid) SubjectMasterHistory_latest on " +
                    "  SubjectMasterHistory_latest.vsubjectid = SubjectMasterHistory.vsubjectid " +
                    " and SubjectMasterHistory_latest.itranno = SubjectMasterHistory.itranno " +
                    " ) SubjectMasterHistory  on " +
                    " SubjectMasterHistory.vsubjectid = SubjectFemaleDetails.vsubjectid " +

                    "left join UserMst on(SubjectFemaleDetailsHistory.iModifyBy = UserMst.iUserId)  " +
                    "left join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode) " +
                    "where  SubjectFemaleDetails.vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "' " +

                                " UNION  " +

                        "select case when " + ColumnName + " ='N' then 'No' When " + ColumnName + " ='Y' then 'Yes' when " + ColumnName + " ='A' then 'NA' End as vChangedValue, vUpdateRemarks as  vRemarks, UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName,SubjectFemaleDetailsHistory.dModifyOn" +
                                " from SubjectFemaleDetailsHistory" +
                                " inner join UserMst on(SubjectFemaleDetailsHistory.iModifyBy = UserMst.iUserId)" +
                                " inner join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)" +
                                " where vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "' and SubjectFemaleDetailsHistory.vColumnName like '" + "%" + ColumnName + "%" + "'" +
                                " order by dModifyon "
                }
                else if (ColumnName == "cLastMenstrualIndi") {
                    obj.query =

                                // "select case when " + ColumnName + " ='0' then 'Painful' When " + ColumnName + " = '1' then 'Painless' else NULL End as  vChangedValue, vUpdateRemarks as  vRemarks, UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName,SubjectFemaleDetailsHistory.dModifyOn" +
                                // " from SubjectFemaleDetailsHistory" +
                                // " inner join UserMst on(SubjectFemaleDetailsHistory.iModifyBy = UserMst.iUserId)" +
                                // " inner join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)" +
                                // " where vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "' and SubjectFemaleDetailsHistory.iTranNo = 1 " +

                                " select case when ISNULL(SubjectFemaleDetailsHistory." + ColumnName + ",NULL )='0' then 'Painful' When ISNULL(SubjectFemaleDetailsHistory." + ColumnName + ",NULL ) = '1' then 'Painless' else NULL End as  vChangedValue,  " +
                            " ''  as  vRemarks, " +
                            " UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName, " +
                    "  SubjectFemaleDetailsHistory.dModifyOn as dModifyOn " +
                    "  from  SubjectFemaleDetails " +
                    "  inner join UserMst UserMst_subject on(SubjectFemaleDetails.iModifyBy = UserMst_subject.iUserId) " +
                    " inner join UserTypeMst UserTypeMst_subject on (UserMst_subject.vUserTypeCode=UserTypeMst_subject.vUserTypeCode)  " +
                    " INNER Join SubjectMaster On SubjectMaster.vSubjectId = SubjectFemaleDetails.vSubjectId " +
                    " left join " +
                    " (select SubjectFemaleDetailsHistory.vSubjectId,SubjectFemaleDetailsHistory.itranno, " +
                    " SubjectFemaleDetailsHistory." + ColumnName + ", " +
                    " SubjectFemaleDetailsHistory.vupdateremarks, " +
                    " SubjectFemaleDetailsHistory.iModifyBy,SubjectFemaleDetailsHistory.dModifyOn " +
                    "  from SubjectFemaleDetailsHistory " +
                    "  inner join " +
                    "  (select  " +
                    "  max(itranno) as itranno,vsubjectid from SubjectFemaleDetailsHistory " +
                    "  where  ( vColumnName IS NULL OR vColumnName  ='') AND vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +
                    "  group by vsubjectid)   " +
                    "  SubjectFemaleDetailsHistory_latest on " +
                    "   SubjectFemaleDetailsHistory_latest.vsubjectid = SubjectFemaleDetailsHistory.vsubjectid " +
                    "  and SubjectFemaleDetailsHistory_latest.itranno = SubjectFemaleDetailsHistory.itranno " +
                    "  ) SubjectFemaleDetailsHistory  on " +
                    "  SubjectFemaleDetailsHistory.vSubjectId = SubjectFemaleDetails.vSubjectId " +

                    "  left join  " +
                    " (select SubjectMasterHistory.vsubjectid,SubjectMasterHistory.itranno, " +
                    " SubjectMasterHistory.vupdateremarks, " +
                    " SubjectMasterHistory.iModifyBy,SubjectMasterHistory.dModifyOn " +
                    " from SubjectMasterHistory " +
                    " inner join " +
                    " (select  " +
                    " max(itranno) as itranno,vsubjectid from SubjectMasterHistory " +
                    "  where  vColumnName IS NULL AND vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'   " +
                    "  group by vsubjectid) SubjectMasterHistory_latest on " +
                    "   SubjectMasterHistory_latest.vsubjectid = SubjectMasterHistory.vsubjectid " +
                    "  and SubjectMasterHistory_latest.itranno = SubjectMasterHistory.itranno " +
                    "  ) SubjectMasterHistory  on " +
                    "  SubjectMasterHistory.vsubjectid = SubjectFemaleDetails.vsubjectid  " +

                    " left join UserMst on(SubjectFemaleDetailsHistory.iModifyBy = UserMst.iUserId)  " +
                    " left join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)  " +
                    " where  SubjectFemaleDetails.vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +
                                 " UNION " +
                    "select case when " + ColumnName + " ='0' then 'Painful' When " + ColumnName + " = '1' then 'Painless' else NULL End as  vChangedValue, vUpdateRemarks as  vRemarks, UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName,SubjectFemaleDetailsHistory.dModifyOn" +
                            " from SubjectFemaleDetailsHistory" +
                            " inner join UserMst on(SubjectFemaleDetailsHistory.iModifyBy = UserMst.iUserId)" +
                            " inner join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)" +
                            " where vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "' and SubjectFemaleDetailsHistory.vColumnName like '" + "%" + ColumnName + "%" + "'" +
                            " order by dModifyon "
                }
                else if (ColumnName == "cContraception") {
                    obj.query =
                                // "select case when " + ColumnName + " ='P' then 'Permanent Contraception' When " + ColumnName + " = 'T'  Then 'Temporary Contraception' Else  NULL End as  vChangedValue, vUpdateRemarks as  vRemarks, UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName,SubjectFemaleDetailsHistory.dModifyOn" +
                                // " from SubjectFemaleDetailsHistory" +
                                // " inner join UserMst on(SubjectFemaleDetailsHistory.iModifyBy = UserMst.iUserId)" +
                                // " inner join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)" +
                                // " where vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "' and SubjectFemaleDetailsHistory.iTranNo = 1 " +



                    " select case when ISNULL(SubjectFemaleDetailsHistory." + ColumnName + " ,NULL)='P' then 'Permanent Contraception' When ISNULL(SubjectFemaleDetailsHistory." + ColumnName + " ,NULL) = 'T'  Then 'Temporary Contraception' Else  NULL End as  vChangedValue, " +
                    " ''  as  vRemarks, " +
                    " UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName, " +
                    " SubjectFemaleDetailsHistory.dModifyOn  as dModifyOn " +
                    "  from  SubjectFemaleDetails " +
                    "  inner join UserMst UserMst_subject on(SubjectFemaleDetails.iModifyBy = UserMst_subject.iUserId)  " +
                    " inner join UserTypeMst UserTypeMst_subject on (UserMst_subject.vUserTypeCode=UserTypeMst_subject.vUserTypeCode)  " +
                    " INNER Join SubjectMaster On SubjectMaster.vSubjectId = SubjectFemaleDetails.vSubjectId " +
                    " left join  " +
                    " (select SubjectFemaleDetailsHistory.vSubjectId,SubjectFemaleDetailsHistory.itranno, " +
                    " SubjectFemaleDetailsHistory." + ColumnName + ", " +
                    " SubjectFemaleDetailsHistory.vupdateremarks, " +
                    " SubjectFemaleDetailsHistory.iModifyBy,SubjectFemaleDetailsHistory.dModifyOn " +
                    "  from SubjectFemaleDetailsHistory " +
                    "  inner join " +
                    "  (select  " +
                    "  max(itranno) as itranno,vsubjectid from SubjectFemaleDetailsHistory " +
                    "  where ( vColumnName IS NULL OR vColumnName  ='') AND vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +
                    "  group by vsubjectid)  " +
                    "  SubjectFemaleDetailsHistory_latest on " +
                    "   SubjectFemaleDetailsHistory_latest.vsubjectid = SubjectFemaleDetailsHistory.vsubjectid " +
                    "  and SubjectFemaleDetailsHistory_latest.itranno = SubjectFemaleDetailsHistory.itranno " +
                    "  ) SubjectFemaleDetailsHistory  on " +
                    "  SubjectFemaleDetailsHistory.vSubjectId = SubjectFemaleDetails.vSubjectId " +

                    "  left join  " +
                    " (select SubjectMasterHistory.vsubjectid,SubjectMasterHistory.itranno, " +
                    " SubjectMasterHistory.vupdateremarks, " +
                    " SubjectMasterHistory.iModifyBy,SubjectMasterHistory.dModifyOn " +
                    " from SubjectMasterHistory " +
                    " inner join " +
                    " (select  " +
                    " max(itranno) as itranno,vsubjectid from SubjectMasterHistory " +
                    "  where  vColumnName IS NULL AND vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +
                    "  group by vsubjectid) SubjectMasterHistory_latest on " +
                    "   SubjectMasterHistory_latest.vsubjectid = SubjectMasterHistory.vsubjectid " +
                    "  and SubjectMasterHistory_latest.itranno = SubjectMasterHistory.itranno " +
                    "  ) SubjectMasterHistory  on " +
                    "  SubjectMasterHistory.vsubjectid = SubjectFemaleDetails.vsubjectid " +

                    " left join UserMst on(SubjectFemaleDetailsHistory.iModifyBy = UserMst.iUserId)  " +
                    " left join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)  " +
                    " where  SubjectFemaleDetails.vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +

                                 " UNION  " +

                        "select case when " + ColumnName + " ='P' then 'Permanent Contraception' When " + ColumnName + " = 'T'  Then 'Temporary Contraception' Else  NULL End as  vChangedValue, vUpdateRemarks as  vRemarks, UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName,SubjectFemaleDetailsHistory.dModifyOn" +
                                " from SubjectFemaleDetailsHistory" +
                                " inner join UserMst on(SubjectFemaleDetailsHistory.iModifyBy = UserMst.iUserId)" +
                                " inner join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)" +
                                " where vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "' and SubjectFemaleDetailsHistory.vColumnName  like '" + "%" + ColumnName + "%" + "'" +
                                " order by dModifyon "
                }
                else if (ColumnName == "cBarrier,cPills,cRhythm,cIUCD") {
                    obj.query =
                             //    "select (case when cBarrier='C' then 'DoubleBarrier' else ' ' End +' '+" +
                             //    " case when cPills ='C' then 'Pills' else ' ' end +' '+" +
                             //    " case when cRhythm='C' then 'Rhythm' else ' ' end +' '+" +
                             //    " case when cIUCD ='C' then 'IUCD' else ' ' end ) as  vChangedValue," +
                             //    " vUpdateRemarks as  vRemarks, UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName,SubjectFemaleDetailsHistory.dModifyOn" +
                             //    " from SubjectFemaleDetailsHistory" +
                             //    " inner join UserMst on(SubjectFemaleDetailsHistory.iModifyBy = UserMst.iUserId)" +
                             //    " inner join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)" +
                             //    " where vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "' and SubjectFemaleDetailsHistory.iTranNo = 1 " +
                             // " UNION ALL " +


                    " select (case when ISNULL(SubjectFemaleDetailsHistory.cBarrier,NULL) ='C' then 'DoubleBarrier' else ' ' End +' '+ " +
                    " case when ISNULL(SubjectFemaleDetailsHistory.cPills,NULL) ='C' then 'Pills' else ' ' end +' '+ " +
                    " case when ISNULL(SubjectFemaleDetailsHistory.cRhythm,NULL) ='C' then 'Rhythm' else ' ' end +' '+ " +
                    " case when ISNULL(SubjectFemaleDetailsHistory.cIUCD ,NULL) ='C' then 'IUCD' else ' ' end ) as  vChangedValue, " +
                    " '' as  vRemarks,  " +
                    " UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName, SubjectFemaleDetailsHistory.dModifyOn " +
                    " from  SubjectFemaleDetails    " +
                    " inner join UserMst UserMst_subject on(SubjectFemaleDetails.iModifyBy = UserMst_subject.iUserId)  " +
                    " INNER Join SubjectMaster On (SubjectMaster.vSubjectId = SubjectFemaleDetails.vSubjectId) " +
                    " inner join UserTypeMst UserTypeMst_subject on (UserMst_subject.vUserTypeCode=UserTypeMst_subject.vUserTypeCode)    " +
                    " left join  (select SubjectFemaleDetailsHistory.vSubjectId,SubjectFemaleDetailsHistory.itranno,  SubjectFemaleDetailsHistory.cBarrier,SubjectFemaleDetailsHistory.cPills , " +
                    "  SubjectFemaleDetailsHistory.cRhythm,SubjectFemaleDetailsHistory.cIUCD, " +
                    " SubjectFemaleDetailsHistory.vupdateremarks,  SubjectFemaleDetailsHistory.iModifyBy,SubjectFemaleDetailsHistory.dModifyOn   from SubjectFemaleDetailsHistory   " +
                    " inner join   (select    max(itranno) as itranno,vsubjectid from SubjectFemaleDetailsHistory   where ( vColumnName IS NULL OR vColumnName  = '') AND vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'    group by vsubjectid)    " +
                    " SubjectFemaleDetailsHistory_latest on    SubjectFemaleDetailsHistory_latest.vsubjectid = SubjectFemaleDetailsHistory.vsubjectid    " +
                    " and SubjectFemaleDetailsHistory_latest.itranno = SubjectFemaleDetailsHistory.itranno   ) SubjectFemaleDetailsHistory  on   " +
                    " SubjectFemaleDetailsHistory.vSubjectId = SubjectFemaleDetails.vSubjectId   left join   (select SubjectMasterHistory.vsubjectid,SubjectMasterHistory.itranno,   " +
                    " SubjectMasterHistory.vupdateremarks,  SubjectMasterHistory.iModifyBy,SubjectMasterHistory.dModifyOn   " +
                    " from SubjectMasterHistory  inner join  (select   max(itranno) as itranno,vsubjectid from SubjectMasterHistory   " +
                    " where  vColumnName IS NULL AND vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'     group by vsubjectid) SubjectMasterHistory_latest on  " +
                    " SubjectMasterHistory_latest.vsubjectid = SubjectMasterHistory.vsubjectid   and SubjectMasterHistory_latest.itranno = SubjectMasterHistory.itranno   )  " +
                    " SubjectMasterHistory  on   SubjectMasterHistory.vsubjectid = SubjectFemaleDetails.vsubjectid   " +
                    " left join UserMst on(SubjectFemaleDetailsHistory.iModifyBy = UserMst.iUserId)   left join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)    " +
                    " where  SubjectFemaleDetails.vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "' " +
                    " UNION " +

                     "select (case when cBarrier='C' then 'DoubleBarrier' else ' ' End +' '+" +
                             " case when cPills ='C' then 'Pills' else ' ' end +' '+" +
                             " case when cRhythm='C' then 'Rhythm' else ' ' end +' '+" +
                             " case when cIUCD ='C' then 'IUCD' else ' ' end ) as  vChangedValue," +
                             " vUpdateRemarks as  vRemarks, UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName,SubjectFemaleDetailsHistory.dModifyOn" +
                             " from SubjectFemaleDetailsHistory" +
                             " inner join UserMst on(SubjectFemaleDetailsHistory.iModifyBy = UserMst.iUserId)" +
                             " inner join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)" +
                             " where vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "' and SubjectFemaleDetailsHistory.vColumnName like '" + "%" + ColumnName + "%" + "'" +
                             " order by dModifyon "
                }
                else {
                    obj.query =
                                   //"select " + ColumnName + " as  vChangedValue, vUpdateRemarks as  vRemarks, UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName,SubjectFemaleDetailsHistory.dModifyOn" +
                                   //" from SubjectFemaleDetailsHistory" +
                                   //" inner join UserMst on(SubjectFemaleDetailsHistory.iModifyBy = UserMst.iUserId)" +
                                   //" inner join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)" +
                                   //" where vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "' and SubjectFemaleDetailsHistory.iTranNo = 1" +


                    " select ISNULL(SubjectFemaleDetailsHistory." + ColumnName + ",NULL)as  vChangedValue, " +
                    " ''  as  vRemarks, " +
                    " UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName, " +
                    " SubjectFemaleDetailsHistory.dModifyOn as dModifyOn " +
                    "  from  SubjectFemaleDetails " +
                    "  inner join UserMst UserMst_subject on(SubjectFemaleDetails.iModifyBy = UserMst_subject.iUserId)  " +
                    " inner join UserTypeMst UserTypeMst_subject on (UserMst_subject.vUserTypeCode=UserTypeMst_subject.vUserTypeCode)  " +
                    " INNER JOIN SubjectMaster on SubjectMaster.vSubjectId = SubjectFemaleDetails.vSubjectId " +
                    " left join  " +
                    " (select SubjectFemaleDetailsHistory.vSubjectId,SubjectFemaleDetailsHistory.itranno, " +
                    " SubjectFemaleDetailsHistory." + ColumnName + ", " +
                    " SubjectFemaleDetailsHistory.vupdateremarks, " +
                    " SubjectFemaleDetailsHistory.iModifyBy,SubjectFemaleDetailsHistory.dModifyOn " +
                    "  from SubjectFemaleDetailsHistory " +
                    "  inner join " +
                    "  (select  " +
                    "  max(itranno) as itranno,vsubjectid from SubjectFemaleDetailsHistory " +
                    "  where  ( vColumnName IS NULL OR vColumnName  = '' ) AND vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +
                    "  group by vsubjectid)  " +
                    "  SubjectFemaleDetailsHistory_latest on " +
                    "   SubjectFemaleDetailsHistory_latest.vsubjectid = SubjectFemaleDetailsHistory.vsubjectid " +
                    "  and SubjectFemaleDetailsHistory_latest.itranno = SubjectFemaleDetailsHistory.itranno " +
                    "  ) SubjectFemaleDetailsHistory  on " +
                    "  SubjectFemaleDetailsHistory.vSubjectId = SubjectFemaleDetails.vSubjectId " +

                    "  left join  " +
                    " (select SubjectMasterHistory.vsubjectid,SubjectMasterHistory.itranno, " +
                    " SubjectMasterHistory.vupdateremarks, " +
                    " SubjectMasterHistory.iModifyBy,SubjectMasterHistory.dModifyOn " +
                    " from SubjectMasterHistory " +
                    " inner join " +
                    " (select  " +
                    " max(itranno) as itranno,vsubjectid from SubjectMasterHistory " +
                    "  where  vColumnName IS NULL AND vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +
                    "  group by vsubjectid) SubjectMasterHistory_latest on " +
                    "   SubjectMasterHistory_latest.vsubjectid = SubjectMasterHistory.vsubjectid " +
                    "  and SubjectMasterHistory_latest.itranno = SubjectMasterHistory.itranno " +
                    "  ) SubjectMasterHistory  on " +
                    "  SubjectMasterHistory.vsubjectid = SubjectFemaleDetails.vsubjectid " +

                    " left join UserMst on(SubjectFemaleDetailsHistory.iModifyBy = UserMst.iUserId)  " +
                    " left join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)  " +
                    " where  SubjectFemaleDetails.vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +

                                " UNION   " +
                                "select " + ColumnName + " as  vChangedValue, vUpdateRemarks as  vRemarks, UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName,SubjectFemaleDetailsHistory.dModifyOn" +
                                " from SubjectFemaleDetailsHistory" +
                                " inner join UserMst on(SubjectFemaleDetailsHistory.iModifyBy = UserMst.iUserId)" +
                                " inner join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)" +
                                " where vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "' and SubjectFemaleDetailsHistory.vColumnName like '" + "%" + ColumnName + "%" + "'" +
                                " order by dModifyon "
                }
}
            if (TableName == 'view_SUBJECTHABITDETAILS') {
                obj.query =
                           //"select Habit,vHabitDetails,vRemarks,vHabitHistory,UserName,dModifyOn,vUpdateRemarks from view_SUBJECTHABITDETAILS" +
                           // " where vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "' order by dModifyOn "

                       " Select ISNULL(SubjectHabitDetailsHistory.vSubjectId ,'') as vSubjectId, " +
                  " ISNULL(SubjectHabitDetailsHistory.vHabitDetails,'') as vHabitDetails , " +
                  " case when ISNULL(SubjectHabitDetailsHistory.cHabitFlag ,'')='C' then 'Current'  " +
                 " when ISNULL(SubjectHabitDetailsHistory.cHabitFlag ,'') ='P' then 'Previous' " +
                 " else 'Never' End as Habit, " +
                 " ISNULL(SubjectHabitDetailsHistory.vRemarks ,'')  as vRemarks, " +
                 " ISNULL(SubjectHabitDetailsHistory.vHabitHistory ,'')  as vHabitHistory, " +
                 " UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName, " +
                 " ISNULL(SubjectHabitDetailsHistory.dModifyOn,'') as dModifyOn , " +
                 " isnull(SubjectHabitDetailsHistory.vUpdateRemarks,'')as  vUpdateRemarks    " +
                 "  from  SubjectHabitDetails  " +
                 "  LEFT join UserMst UserMst_subject on(SubjectHabitDetails.iModifyBy = UserMst_subject.iUserId)  " +
                   " LEFT join UserTypeMst UserTypeMst_subject on (UserMst_subject.vUserTypeCode=UserTypeMst_subject.vUserTypeCode)  " +
                    " INNER JOIN SubjectMaster on SubjectMaster.vSubjectId = SubjectHabitDetails.vSubjectId " +
                 " left join   " +
                 " (select SubjectHabitDetailsHistory.vSubjectId,SubjectHabitDetailsHistory.itranno, " +
                 " SubjectHabitDetailsHistory.vHabitDetails,SubjectHabitDetailsHistory.cHabitFlag, " +
                 " SubjectHabitDetailsHistory.vupdateremarks,SubjectHabitDetailsHistory.vRemarks,SubjectHabitDetailsHistory.vHabitHistory, " +
                 " SubjectHabitDetailsHistory.iModifyBy,SubjectHabitDetailsHistory.dModifyOn,SubjectHabitDetailsHistory.vHabitId " +
                 "  from SubjectHabitDetailsHistory " +
                 "  inner join " +
                 "   (select  " +
                 "   max(itranno) as itranno,vsubjectid from SubjectHabitDetailsHistory " +
                 "   where  vUpdateRemarks IS NULL AND vColumnName IS NULL AND vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +
                 "   group by vsubjectid)   " +
                 "   SubjectHabitDetailsHistory_latest on " +
                 "    SubjectHabitDetailsHistory_latest.vsubjectid = SubjectHabitDetailsHistory.vsubjectid " +
                 "   and SubjectHabitDetailsHistory_latest.itranno = SubjectHabitDetailsHistory.itranno " +
                 "  ) SubjectHabitDetailsHistory  on  " +
                 "  SubjectHabitDetailsHistory.vSubjectId  = SubjectHabitDetails.vSubjectId AND  " +
                 "  SubjectHabitDetailsHistory.vHabitId  = SubjectHabitDetails.vHabitId " +

                 " left join UserMst on(SubjectHabitDetailsHistory.iModifyBy = UserMst.iUserId)  " +
                    " left join UserTypeMst on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)  " +

                 " where  SubjectHabitDetails.vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "'  " +

                 " UNION  " +


              "     Select ISNULL(SubjectHabitDetailsHistory.vSubjectId ,'') as vSubjectId,           " +
              "    ISNULL(SubjectHabitDetailsHistory.vHabitDetails,'') as vHabitDetails ,             " +
              "    case when ISNULL(SubjectHabitDetailsHistory.cHabitFlag ,'')='C' then 'Current'     " +
              "     when ISNULL(SubjectHabitDetailsHistory.cHabitFlag ,'') ='P' then 'Previous'       " +
              "    else 'Never' End as Habit,                                                         " +
              "   ISNULL(SubjectHabitDetailsHistory.vRemarks ,'')  as vRemarks,                       " +
              "   ISNULL(SubjectHabitDetailsHistory.vHabitHistory ,NULL)  as vHabitHistory,           " +
              "    UserMst.vUserName+'('+UserTypeMst.vUserTypeName+')' as UserName,                   " +
              "   ISNULL(SubjectHabitDetailsHistory.dModifyOn,'') as dModifyOn ,                      " +
              "   isnull(SubjectHabitDetailsHistory.vUpdateRemarks,'')as  vUpdateRemarks              " +
              "   from  SubjectHabitDetailsHistory                                                    " +
              "   INNER join UserMst  on(SubjectHabitDetailsHistory.iModifyBy = UserMst.iUserId)      " +
              "   INNER join UserTypeMst  on (UserMst.vUserTypeCode=UserTypeMst.vUserTypeCode)        " +
                 " where  SubjectHabitDetailsHistory.vSubjectId='" + $('#<%= HSubjectId.clientid %>').val() + "' AND SubjectHabitDetailsHistory.vColumnName IS NOT NULL " +

                 " order by dModifyOn "
            }


            var JsonText = JSON.stringify(obj);
            $.ajax(
            {
                type: "POST",
                url: "WS_HelpDB_JSON.asmx/GetDataTableObjectBySqlQuery",
                data: JsonText,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    var aaDataSet = [];
                    var Column = [];

                    if (data.d != "" && data.d != null) {
                        data = JSON.parse(data.d);
                        if (TableName != 'view_SUBJECTHABITDETAILS') {

                            for (var Row = 0; Row < data.length; Row++) {
                                var InDataSet = [];
                                if (ColumnName == 'dBirthDate' || ColumnName == 'dEnrollmentDate' || ColumnName == 'dLastMenstrualDate' || ColumnName == 'dLastDelivaryDate' || ColumnName == 'dAbortionDate') {
                                    if (data[Row].vChangedValue != "") {
                                        data[Row].vChangedValue = convertDateToDDMMMYYY((data[Row].vChangedValue).toString())
                                    }
                                }
                                if (data[Row].vChangedValue == ',,' || endsWith(data[Row].vChangedValue, ",,")) {
                                    data[Row].vChangedValue = data[Row].vChangedValue.replace(",,", "")
                                }

                                // data[Row].vChangedValue.replace(",,",',')
                                if (data[Row].vChangedValue.search(',,')) {
                                    data[Row].vChangedValue = data[Row].vChangedValue.replace(",,", ",")
                                }
                                convertDateTo24Hour((data[Row].dModifyOn).toString())
                                InDataSet.push(Row + 1, data[Row].vChangedValue, data[Row].vRemarks, data[Row].UserName, convertDateTo24Hour((data[Row].dModifyOn).toString()));
                                aaDataSet.push(InDataSet);
                            }
                            Column = createColumn("Sr. No.#Value#Reason#Modify By#Modify On")
                        }
                        else {
                            for (var Row = 0; Row < data.length; Row++) {
                                var InDataSet = [];
                                //if (isDate())
                                var date = new Date(data[Row].dModifyOn)
                                InDataSet.push(Row + 1, data[Row].Habit, data[Row].vHabitDetails, data[Row].vRemarks, data[Row].vHabitHistory, data[Row].vUpdateRemarks, data[Row].UserName, convertDateTo24Hour((data[Row].dModifyOn).toString()));
                                aaDataSet.push(InDataSet);
                            }
                            Column = createColumn("Sr. No.#Habit#HabitDetails#Consumption Details#If Previous,stopped since#Reason#Modify By#Modify On")
                        }

                        if ($("#tblAudit").children().length > 0) {

                            $("#tblAudit").dataTable().fnDestroy();
                            $("#tblAudit").html("");
                        }
                        $('#tblAudit').prepend($('<thead>').append($('#tblAudit tr:first'))).dataTable({
                            //'bPaginate': false,
                            //'bInfo': false,
                            //'bFilter': true,
                            //"bSort": true,
                            //"bDestory": true,
                            //"bRetrieve": true,
                            //"bLengthChange": true,
                            //"iDisplayLength": 10,
                            "bJQueryUI": true,
                            "sPaginationType": "full_numbers",
                            "bLengthChange": true,
                            "iDisplayLength": 10,
                            "bProcessing": true,
                            "bSort": false,
                            aLengthMenu: [
                                [10, 25, 50, 100, -1],
                                [10, 25, 50, 100, "All"]
                            ],
                            "aaData": aaDataSet,
                            "aoColumns": Column
                        });
                        $('#tblAudit tr:first').css('background-color', '#3A87AD');
                        $('#tblAudit tr th').css('color', 'White');
                        $find('MPEAudit').show();

                    }
                    else {
                        $find('mdlWarning').show();
                        //$('#WarningHeader').text('Warning');
                        $('#WarningMessage').text('No Audit Trail found for this field.');
                        return false;
                    }
                    //Add by shivani
                    $("#tblAudit th").attr("class", "trHeader")
                },
                failure: function (error) {
                    msgalert(error);
                }
            });

        });
    }

    function createColumn(HeaderText) {
        var Column = new Array();
        for (var i = 0; i < HeaderText.split("#").length; i++) {
            var obj = new Object();
            obj.sTitle = HeaderText.split("#")[i].toString();
            Column.push(obj);
        }
        return Column;
    }
    function fnUpdateField() {

        $('.UpdateControl').unbind('click').click(function () {

            // if (this.id = "ctl00$CPHLAMBDA$Image8") {
            //  var Age = document.getElementById('<%=txtAge.ClientID%>').value;
            //document.getElementById("ctl00$CPHLAMBDA$txtAge").value
            //   if (parseInt(Age) < 17) {
            //       alert("Age Should be more than 17 years. ")
            //      $("#ctl00$CPHLAMBDA$divRemarks").hide();
            // document.getElementById('<%=divRemarks.ClientID%>').style.display = 'none';
            //   }
            // }


            TableName = $(this.previousElementSibling).attr('tName');
            ColumnName = $(this.previousElementSibling).attr('cName');
            ChangedValue = $(this.previousElementSibling).val();

            if ($(this).attr('ctype') == "Contrapception") {
                TableName = $(this).next().next().attr('tname');
                ColumnName = $(this).next().next().attr('cName');

                $(this).next().next().find('input').each(function () {
                    if (this.checked == true) {
                        if (ChangedValue != null) {
                            ChangedValue = ChangedValue + ',' + 'C'
                        }
                        else {
                            ChangedValue = 'C'
                        }
                    }
                    else {
                        if (ChangedValue != null) {
                            ChangedValue = ChangedValue + ',' + 'N'
                        }
                        else {
                            ChangedValue = 'N'
                        }
                    }
                });
            }

            if (TableName == undefined && ColumnName == undefined) {
                TableName = $(this).parent().next().find('table').attr('tName')
                ColumnName = $(this).parent().next().find('table').attr('cName');
                ChangedValue = $(this.parent).val();
                $(this).parent().next().find('input').each(function () {
                    if (this.checked == true) {
                        if (ChangedValue != null) {
                            ChangedValue = ChangedValue + ',' + $(this).parent().data('lng');
                        }
                        else {
                            ChangedValue = $(this).parent().data('lng');
                        }
                    }

                });
            }

            if (TableName == undefined && ColumnName == undefined) {
                TableName = $(this).next().next().attr('tname')
                ColumnName = $(this).next().next().attr('cName');
                $(this).next().next().find('input').each(function () {
                    if (this.checked == true) {
                        if (ChangedValue != null) {
                            if ($(this).parent().data('lng') != undefined) {
                                ChangedValue = ChangedValue + ',' + $(this).parent().data('lng');
                            }
                            else {
                                ChangedValue = ChangedValue + ',' + $(this).val();
                            }

                        }
                        else {
                            if ($(this).parent().data('lng') != undefined) {
                                ChangedValue = $(this).parent().data('lng');
                            }
                            else {
                                ChangedValue = $(this).val();
                            }

                        }
                    }

                });
            }


            //if (this.id = "ctl00_CPHLAMBDA_Image36") {
            //    TableName = "SubjectProofDetails";
            //    ColumnName = "cStatusIndi";

            //}
            $("#<%= txtRemarks.ClientID %>").val('');

            //if (!ValidationUpdate()) {
            //    return false;
            //}

            if (ColumnName == 'vSurName') {
                if (document.getElementById('<%= txtvSurName.ClientId %>').value.toString().trim().length <= 0) {
                    msgalert('Please Enter Last Name');
                    document.getElementById('<%= txtvSurName.ClientID%>').focus();
                    return false;
                }
            }

            if (ColumnName == 'vFirstName') {
                if (document.getElementById('<%= txtvFirstName.ClientId %>').value.toString().trim().length <= 0) {
                    msgalert('Please Enter First Name');
                    document.getElementById('<%= txtvFirstName.ClientId %>').focus();
                    return false;
                }
            }

            if (ColumnName == 'dBirthDate') {
                if (document.getElementById('<%= txtdBirthDate.ClientId %>').value.toString().trim().length <= 0) {
                    msgalert('Please Enter Date of Birth.');
                    document.getElementById('<%= txtdBirthDate.ClientID%>').focus();
                    return false;
                }
            }

            if (ColumnName == 'nHeight') {
                if (document.getElementById('<%= txtnHeight.ClientId %>').value.toString().trim().length <= 0) {
                    msgalert('Please Enter Height');
                    document.getElementById('<%= txtnHeight.ClientId %>').focus();
                    return false;
                }
            }

            if (ColumnName == 'nWeight') {
                if (document.getElementById('<%= txtnWeight.ClientID%>').value.toString().trim().length <= 0) {
                    msgalert('Please Enter Weight');
                    document.getElementById('<%= txtnWeight.ClientID%>').focus();
                    return false;
                }
            }

            if (ColumnName == 'dEnrollmentDate') {
                if (document.getElementById('<%= txtdEnrollmentDate.ClientId %>').value.toString().trim().length <= 0) {
                    msgalert('Please Enter Enrolment Date');
                    document.getElementById('<%= txtdEnrollmentDate.ClientID%>').focus();
                    return false;
                }
            }
            if (ColumnName == 'dLastMenstrualDate' || ColumnName == 'dLastDelivaryDate' || ColumnName == 'dAbortionDate') {

                if (ColumnName == 'dLastMenstrualDate') {
                    var bdate = $("#ctl00_CPHLAMBDA_txtdLastMenstrualDate").val()
                    bdate = new Date(bdate)
                    var today = new Date();
                    if (bdate > today) {
                        $find('mdlRemarks').hide();
                        //$("#ctl00_CPHLAMBDA_txtDBirthDate").val() = "";
                        msgalert("Date Should be less than current date.")
                        return false;
                    }
                }

                if (ColumnName == 'dLastDelivaryDate') {
                    var bdate = $("#ctl00_CPHLAMBDA_txtdLastDelivaryDate").val()
                    bdate = new Date(bdate)
                    var today = new Date();
                    if (bdate > today) {
                        $find('mdlRemarks').hide();
                        //$("#ctl00_CPHLAMBDA_txtDBirthDate").val() = "";
                        msgalert("Date Should be less than current date.")
                        return false;
                    }
                }
                if (ColumnName == 'dAbortionDate') {
                    var bdate = $("#ctl00_CPHLAMBDA_txtdAbortionDate").val()
                    bdate = new Date(bdate)
                    var today = new Date();
                    if (bdate > today) {
                        $find('mdlRemarks').hide();
                        //$("#ctl00_CPHLAMBDA_txtDBirthDate").val() = "";
                        msgalert("Date Should be less than current date.")
                        return false;
                    }
                }
            }
            if (ColumnName == 'dBirthDate' || ColumnName == 'dEnrollmentDate') {
                var today = new Date();
                if (ColumnName == 'dBirthDate') {
                    var bdate = $("#ctl00_CPHLAMBDA_txtDBirthDate").val()
                    bdate = new Date(bdate)
                    if (bdate > today) {
                        $find('mdlRemarks').hide();
                        //$("#ctl00_CPHLAMBDA_txtDBirthDate").val() = "";
                        msgalert("Date Should be less than current date.")
                        return false;
                    }
                }
                if (ColumnName == 'dEnrollmentDate') {
                    var Edate = $("#ctl00_CPHLAMBDA_txtdoer").val();
                    Edate = new Date(Edate)
                    if (Edate > today) {
                        $find('mdlRemarks').hide();
                        //$("#ctl00_CPHLAMBDA_txtdoer").val() = "";
                        msgalert("Date Should Be Less Than Current Date.")
                        return false;
                    }
                }
            }
            if (ColumnName == 'dBirthDate' || ColumnName == 'dEnrollmentDate') {
                if ($("#ctl00_CPHLAMBDA_txtAge").val() < 17) {
                    msgalert("Age Should Be More Than 18 Years.");
                    $find('mdlRemarks').hide();
                }
                else {
                    $find('mdlRemarks').show();
                }
            }
            else {
                $find('mdlRemarks').show();
            }




            ClickedControl = $(this);
        });




        $('.UpdateConControl').unbind('click').click(function () {
            $("#<%= txtRemarks.ClientID %>").val('');

            $find('mdlRemarks').show();

            ClickedControl = $(this);
        });
    }

    function fnSaveField() {
        try {


            $('#<%= btnRemarksUpdate.ClientID %>').unbind('click').click(function () {
                // var dele = document.getElementById('<%=HFDelete.ClientID %>').value
                $('#<%= btnRemarksUpdate.ClientID %>').hide()
                //event.preventDefault();
                // if ($(ClickedControl).attr('cName') == "AttachDoc") {
                //      funAttachDoc();
                //      document.getElementById('<%=btnSaveProof.ClientId %>').click();
                // return true;
                //   }
                var content = {};
                content.SubjectId = $('#<%= HSubjectId.clientid %>').val();
                if (!(TableName == undefined)) {
                    content.ColumnName = ColumnName;
                    content.TableName = TableName;
                    content.ChangedValue = ChangedValue;
                    if (ColumnName == 'vICFLanguageCodeId' && ChangedValue == undefined) {
                        content.ChangedValue = 'NULL'
                    }
                    content.Remarks = $("#<%= txtRemarks.ClientID %>").val().trim();
                    content.JSONString = "";
                }

                if ($(ClickedControl).attr('cName') == "Intials") {
                    content.ColumnName = content.ColumnName + ',' + "vInitials";
                    content.ChangedValue = content.ChangedValue + ',' + $('#<%= HFInitials.clientid %>').val();

                }


                if ($(ClickedControl).attr('cName') == "BMI") {
                    content.ColumnName = content.ColumnName + ',' + "nBMI";
                    // if ($('#<%= HfBMI.clientid %>').val() == "") {

                    // }
                    content.ChangedValue = content.ChangedValue + ',' + $('#ctl00_CPHLAMBDA_txtbmi').val();
                }
                if ($(ClickedControl).attr('ctype') == "BloodGroup") {

                    content.ColumnName = content.ColumnName + ',' + "cBloodGroup";
                    content.ChangedValue = content.ChangedValue + ',' + $('#ctl00_CPHLAMBDA_ddlcBloodGroup').val();
                }
                if ($(ClickedControl).attr('ctype') == "AttachDoc") {
                    content.TableName = "SubjectProofDetails";
                    content.ColumnName = ColumnName;

                }

                if (ColumnName == "vPopulation") {
                    content.ColumnName = ColumnName;
                    content.ChangedValue = $("#ctl00_CPHLAMBDA_ddlvPopulation option:selected").text();
                }
                if (ColumnName == "vPerCity") {
                    content.ColumnName = ColumnName;
                    content.ChangedValue = $("#ctl00_CPHLAMBDA_ddlvPerCity option:selected").text();
                }

                if (TableName == 'SubjectProofDetails') {

                    content.TableName = "SubjectProofDetails";
                    content.ColumnName = ColumnName + ',' + 'nSubjectProofNo' + ',iModifyBy'
                    content.ChangedValue = "D " + ',' + $('#ctl00_CPHLAMBDA_HDSubjectProofDetails').val() + ',' + '<%= Session(S_UserID)%>';

                }

                if ($(ClickedControl).attr('ctype') == "PerAdd" || $(ClickedControl).attr('ctype') == "localAdd2" || $(ClickedControl).attr('ctype') == "localadd1") {
                    var columns = ColumnName.split(',');
                    var add = '';
                    for (var i = 0; i < columns.length; i++) {

                        if (i <= 1) {
                            var colval = ChangedValue.substr(0, ChangedValue.indexOf(','))
                            ChangedValue = ChangedValue.replace(colval + ",", '')
                        }
                        else {
                            colval = ChangedValue;
                        }
                        if (add == '') {
                            add = colval;
                        }
                        else {
                            add = add + "#" + colval;
                        }
                    }
                    content.ChangedValue = add;
                    //content.Refcolumn = "TRUE";

                }

                if ($("#<%= txtRemarks.ClientID %>").val().trim() == "") {
                    msgalert("Please Enter Remarks.")
                    $('#<%= btnRemarksUpdate.ClientID %>').show();
                    return false;
                    //$find('mdlAlert').show();
                    //$find('mdlRemarks').hide();
                    //$('#AlertHeader').text('Remarks Warning');
                    //$('#AlertMessage').text('Please enter remarks.');
                    // $('#<%= btnYes.ClientID %>').css('display', 'none');
                    //$('#<%= btnNo.ClientID %>').css('display', 'none');
                    // $('#<%= btnOk.ClientID %>').css('display', 'inline');
                }

                if (content.TableName == "SubjectProofDetails") {
                    var Edit = '<%=ViewState("Edit")%>';
                    var dele = document.getElementById('<%=HFDelete.ClientID %>').value
                    var aadd = document.getElementById('<%=HFADD.ClientID %>').value
                    if (Edit == 'Y' && dele != 'Y' && aadd == 'Y') {
                        $find('mdlRemarks').hide();
                        document.getElementById('<%=HFADD.ClientID %>').value = ''
                        var hf = $("#<%= txtRemarks.ClientID %>").val().trim();
                        document.getElementById('<%= HFRemarks.ClientID()%>').value = hf
                        var btn = document.getElementById('<%= btnActualSave.ClientId()%>')
                        document.getElementById('<%=HFRemarks.ClientID%>').value = $("#<%= txtRemarks.ClientID %>").val().trim();
                        document.getElementById('<%=HFADD.ClientID %>').value = ''
                        btn.click();
                        msgalert("Value Updated Successfully");

                        return false;
                    }
                    var array = content.ChangedValue.split(',');
                    var dele = document.getElementById('<%=HFDelete.ClientID %>').value = ''
                    if (array[1] == "") {
                        $('#<%= btnRemarksUpdate.ClientID %>').show();

                        msgalert("Please Follow Sequence Properly.")
                        return false;
                    }
                }
                if ($(ClickedControl).attr('ctype') == "Consumption") {


                    content.ColumnName = '';
                    content.TableName = 'SUBJECTHABITDETAILS';
                    content.ChangedValue = '';
                    content.Remarks = $("#<%= txtRemarks.ClientID %>").val().trim();
                    var JSONObj = [];
                    var i = 0
                    $('.Habits').each(function () {

                        var Habits = {};
                        i = i + 1
                        var j = i + 1
                        var habittype = $('#ctl00_CPHLAMBDA_GVHabits_ctl0' + j.toString() + '_ddlHebitType option:selected')
                        var cHabitFlag = habittype.val()
                        Habits.nSubjectHabitDetailsNo = $(this).children()[0].innerHTML;
                        Habits.vSubjectId = $('#<%= HSubjectId.clientid %>').val();
                        Habits.vHabitId = '0' + i.toString()
                        // Habits.cHabitFlag = $(this).find('.EntryControl').val();
                        Habits.cHabitFlag = cHabitFlag
                        Habits.vHabitDetails = $(this).children()[2].innerHTML;
                        Habits.vHabitHistory = $($(this).children()[5]).find('input').val();
                        Habits.vRemarks = $($(this).children()[4]).find('input').val();
                        Habits.iModifyBy = '';
                        Habits.dModifyon = '';
                        Habits.cStatusIndi = 'E'
                        Habits.vUpdateRemarks = $("#<%= txtRemarks.ClientID %>").val().trim();
                        Habits.vColumnName = '--'
                        JSONObj.push(Habits);
                    });
                    content.JSONString = JSON.stringify(JSONObj);

                }
                if ($("#<%= txtRemarks.ClientID %>").val().trim() == "") {
                    msgalert("Please Enter Remarks.")
                    $('#<%= btnRemarksUpdate.ClientID %>').show();
                    //$find('mdlAlert').show();
                    //$find('mdlRemarks').hide();
                    //$('#AlertHeader').text('Remarks Warning');
                    //$('#AlertMessage').text('Please enter remarks.');
                    // $('#<%= btnYes.ClientID %>').css('display', 'none');
                    //$('#<%= btnNo.ClientID %>').css('display', 'none');
                    // $('#<%= btnOk.ClientID %>').css('display', 'inline');
                }
                else {
                    $.ajax({
                        type: "POST",
                        // url: "frmSubjectPIFMst_New.aspx/fnUpdateField",
                        url: "frmSubjectPIFMst_New.aspx/UpdateFieldValues",
                        data: JSON.stringify(content),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {

                            if (data.d) {
                                $("#<%= txtRemarks.ClientID %>").val('');
                                $find('mdlRemarks').hide();
                                ClickedControl.attr('title', ClickedControl.attr('title').replace('Update', 'Edit'));
                                ClickedControl.attr('src', ClickedControl.attr('src').replace('Update.png', 'Edit_Small.png'));
                                if ($(ClickedControl).attr('ctype') == "Contrapception") {
                                    $(ClickedControl).next().next().find('input').attr('disabled', true);
                                    ClickedControl.attr('class', 'EditControl');
                                }
                                else if ($(ClickedControl).attr('ctype') == "BloodGroup") {
                                    ClickedControl.attr('class', 'EditControl');
                                    $(ClickedControl).prev().attr('disabled', true);
                                    $(ClickedControl).prev().prev().attr('disabled', true);
                                }
                                else {
                                    ClickedControl.attr('class', 'EditControl');
                                    if ($(ClickedControl).prev().length == 0) {
                                        $(ClickedControl).next().next().find('input').attr('disabled', true);
                                    }
                                    else {
                                        $(ClickedControl).prev().attr('disabled', true);
                                    }
                                }
                                msgalert("Value updated successfully");


                                fnEditField();
                                if (content.TableName == "SubjectProofDetails") {
                                    var btn = document.getElementById('<%=btnBindProof.ClientID%>');
                                    btn.click();
                                    var url = window.location
                                    location.assign(url)
                                    document.getElementById('<%=HFDelete1.ClientID %>').value = ''

                                }
                                else {
                                    location.reload(true);
                                }


                            }
                            else {
                                msgalert('Error while Saving to database');
                                ClickedControl.attr('class', 'EditControl');
                            }
                        },
                        failure: function (error) {
                            msgalert(error);
                        }
                    });
                }
                return false;

            });
        }
        catch (exception) {
            msgalert("Value NOt Updated.")
        }
        finally {
            $('#<%= btnRemarksUpdate.ClientID %>').show();
        }
    }

    var inyear;
    function DateConvertForScreening(ParamDate, txtdate) {

        if (ParamDate.length == 0) {
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
                    if (inyear < 1900) {
                        msgalert('You Can Not Add Date Which Is Less Than "01-Jan-1900"  ');
                        txtdate.value = "";
                        txtdate.focus();
                        return false;
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
        if (inyear < 1900) {
            msgalert('You Can Not Add Date Which Is Less Than "01-Jan-1900"  ');
            txtdate.value = "";
            txtdate.focus();
            return false;
        }

        return true;
    }


    function QCDivShowHide(Type) {
        if (document.getElementById('<%= HSubjectId.ClientId %>').value.toString().trim().length <= 0) {
            msgalert('Please Enter Subject');
            document.getElementById('<%= txtSubject.ClientId %>').focus();
            document.getElementById('<%= txtSubject.ClientId %>').value = '';
            return false;
        }
        if (Type == 'S') {
            document.getElementById('<%= divQCDtl.ClientId %>').style.display = 'block';
            SetCenter('<%= divQCDtl.ClientId %>');
            return false;
        }
        else if (Type == 'H') {
            document.getElementById('<%= divQCDtl.ClientId %>').style.display = 'none';
            return false;
        }
        else if (Type == 'ST') {
            document.getElementById('<%= divQCDtl.ClientId %>').style.display = 'block';
            SetCenter('<%= divQCDtl.ClientId %>');
            return true;
        }
    return true;
}

function ValidateTab() {
    if (typeof (Page_ClientValidate) == 'function') {
        Page_ClientValidate();
    }
    return Page_IsValid;
}
function Validation(type) {
    var e = document.getElementById("ctl00_CPHLAMBDA_ddlvPopulation");
    var Population = e.options[e.selectedIndex].text;
    var p = document.getElementById("ctl00_CPHLAMBDA_ddlvPerCity");
    var place = p.options[p.selectedIndex].text;


    if (document.getElementById('<%= txtvSurName.ClientId %>').value.toString().trim().length <= 0) {
        document.getElementById('<%= txtvSurName.ClientId %>').focus();
        msgalert('Please Enter Last Name');
        return false;
    }
    else if (document.getElementById('<%= txtvFirstName.ClientId %>').value.toString().trim().length <= 0) {
        document.getElementById('<%= txtvFirstName.ClientId %>').focus();
        msgalert('Please Enter First Name');
        return false;
    }
    else if (document.getElementById('<%= txtdBirthDate.ClientId %>').value.toString().trim().length <= 0) {
        document.getElementById('<%= txtdBirthDate.ClientID%>').focus();
        msgalert('Please Enter Date of Birth.');
        return false;
    }
    else if (document.getElementById('<%= txtdEnrollmentDate.ClientId %>').value.toString().trim().length <= 0) {
        document.getElementById('<%= txtdEnrollmentDate.ClientId %>').focus();
        msgalert('Please Enter Enrolment Date');
        return false;
    }
    else if (document.getElementById('<%= txtnHeight.ClientID%>').value.toString().trim().length <= 0) {
        document.getElementById('<%= txtnHeight.ClientID%>').focus();
        msgalert('Please Enter Height');
        return false;
    }
    else if (document.getElementById('<%= txtnWeight.ClientID%>').value.toString().trim().length <= 0) {
        document.getElementById('<%= txtnWeight.ClientID%>').focus();
        msgalert('Please Enter Weight');
        return false;
    }


    else if (Population == '--Select Population --') {
        msgalert('Please Select Population');
        return false;
    }

    else if (place == '--Select Place --') {
        document.getElementById('<%= txtnWeight.ClientID%>').focus();
        msgalert('Please Select Place');
        return false;
    }
    // else if (document.getElementById('<%= txtvReferredBy.ClientID%>').value.toString().trim().length <= 0) {
    //     alert('Please Enter ReferredBy');
    //     return false;
    // }



    //Added for compulsory add remark while Edit else not compulsory on 24-Aug-2009
    if (type == 'EDIT') {
        if (document.getElementById('<%= txtRemarks.ClientId %>').value.toString().trim().length <= 0) {
            alert('Please Enter Remarks');
            return false;
        }
    }

    var Edit = '<%=ViewState("code")%>';
    if (Edit != 'Y') {
        enableControl();
    }

    return true;
}


function Validation12() {

    if (document.getElementById('<%= txtvSurName.ClientId %>').value.toString().trim().length <= 0) {
        document.getElementById('<%= txtvSurName.ClientId %>').focus();
        msgalert('Please Enter Last Name');
        return false;
    }
    else if (document.getElementById('<%= txtvFirstName.ClientId %>').value.toString().trim().length <= 0) {
        msgalert('Please Enter First Name');
        document.getElementById('<%= txtvFirstName.ClientId %>').focus();
        return false;
    }
    else if (document.getElementById('<%= txtdBirthDate.ClientId %>').value.toString().trim().length <= 0) {
        msgalert('Please Enter Date of Birth.');
        document.getElementById('<%= txtdBirthDate.ClientID%>').focus();
        return false;
    }
    else if (document.getElementById('<%= txtdEnrollmentDate.ClientId %>').value.toString().trim().length <= 0) {
        msgalert('Please Enter Enrolment Date');
        document.getElementById('<%= txtdEnrollmentDate.ClientId %>').focus();
        return false;
    }
    else if (document.getElementById('<%= txtnHeight.ClientID%>').value.toString().trim().length <= 0) {
        msgalert('Please Enter Height');
                document.getElementById('<%= txtnHeight.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%= txtnWeight.ClientID%>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Weight');
                document.getElementById('<%= txtnWeight.ClientID%>').focus();
                return false;
            }
    return true;
}
function ValidationUpdate() {

    if (document.getElementById('<%= txtvSurName.ClientID%>').value.toString().trim().length <= 0) {
        msgalert('Please Enter Last Name');
        document.getElementById('<%= txtvSurName.ClientID%>').focus();
        return false;
    }
    else if (document.getElementById('<%= txtvFirstName.ClientID%>').value.toString().trim().length <= 0) {
        msgalert('Please Enter First Name');
        document.getElementById('<%= txtvFirstName.ClientID%>').focus();
        return false;
    }
    else if (document.getElementById('<%= txtdBirthDate.ClientID%>').value.toString().trim().length <= 0) {
        msgalert('Please Enter Date of Birth.');
        document.getElementById('<%= txtdBirthDate.ClientID%>').focus();
        return false;
    }
    else if (document.getElementById('<%= txtdEnrollmentDate.ClientId %>').value.toString().trim().length <= 0) {
        msgalert('Please Enter Enrolment Date');
        document.getElementById('<%= txtdEnrollmentDate.ClientId %>').focus();
        return false;
    }
    else if (document.getElementById('<%= txtnHeight.ClientID%>').value.toString().trim().length <= 0) {
        msgalert('Please Enter Height');
        document.getElementById('<%= txtnHeight.ClientID%>').focus();
        return false;
    }
    else if (document.getElementById('<%= txtnWeight.ClientID%>').value.toString().trim().length <= 0) {
        msgalert('Please Enter Weight');
        document.getElementById('<%= txtnWeight.ClientID%>').focus();
        return false;
    }
    return true;
}

function ValidationQC() {
    if (document.getElementById('<%= txtQCRemarks.ClientId %>').value.toString().trim().length <= 0) {
        msgalert('Please Enter Remarks/Response');
        document.getElementById('<%= txtQCRemarks.ClientId %>').focus();
        return false;
    }
    // try 
    //{
    //  document.getElementById('<%= BtnQCSave.ClientId()%>').disabled = true;
    // var btn = document.getElementById('<%= BtnQCSave.ClientId()%>')
    // btn.click();
    //}
    // catch(err)
    // {  }
    // finally
    // {
    //   document.getElementById('<%= BtnQCSave.ClientId()%>').disabled = false;

    //}
    return true;
}
function convertDateTo24Hour(date) {
    var elem = date.split(' ');
    var stSplit = elem[1].split(":");// alert(stSplit);
    var stHour = stSplit[0];
    var stMin = stSplit[1];
    var stAmPm = elem[2];
    var newhr = 0;
    var ampm = '';
    var newtime = '';
    //alert("hour:"+stHour+"\nmin:"+stMin+"\nampm:"+stAmPm); //see current values

    if (stAmPm == 'PM') {
        if (stHour != 12) {
            stHour = stHour * 1 + 12;
        }

    } else if (stAmPm == 'AM' && stHour == '12') {
        stHour = stHour - 12;
    } else {
        stHour = stHour;
    }

    var date = elem[0].split('/')

    var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
    var mont = ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12']
    var i = 0;
    for (i; i <= months.length; i++) {
        if (mont[i] == date[0]) {
            break;
        }
    }

    var date12 = ['01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12', '13', '14', '15', '16', '17', '18', '19', '20', '21', '22', '23', '24', '25', '26', '27', '28', '29', '30', '31']
    var date13 = ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12', '13', '14', '15', '16', '17', '18', '19', '20', '21', '22', '23', '24', '25', '26', '27', '28', '29', '30', '31']
    var j = 0;
    for (j; j <= date13.length; j++) {
        if (date13[j] == date[1]) {
            break;
        }
    }


    var MasterDate = $("#ctl00_lblTime").text()
    var dateGMt = MasterDate.split('(')
    var offsate = dateGMt[1]
    var TimeZone = document.getElementById('<%= HFTimeZone.ClientId %>').value.toString()
    if (TimeZone == "India Standard Time") {
        TimeZone = " IST"
    }
    else {

        TimeZone = " EST"
    }
    return date12[j] + "-" + months[i] + "-" + date[2] + " " + stHour + ':' + stMin + TimeZone + "(" + offsate;
}


function convertDateToDDMMMYYY(date) {
    var elem = date.split(' ');
    var stSplit = elem[1].split(":");// alert(stSplit);
    var stHour = stSplit[0];
    var stMin = stSplit[1];
    var stAmPm = elem[2];
    var newhr = 0;
    var ampm = '';
    var newtime = '';
    //alert("hour:"+stHour+"\nmin:"+stMin+"\nampm:"+stAmPm); //see current values

    if (stAmPm == 'PM') {
        if (stHour != 12) {
            stHour = stHour * 1 + 12;
        }

    } else if (stAmPm == 'AM' && stHour == '12') {
        stHour = stHour - 12;
    } else {
        stHour = stHour;
    }

    var date = elem[0].split('/')

    var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
    var mont = ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12']
    var i = 0;
    for (i; i <= months.length; i++) {
        if (mont[i] == date[0]) {
            break;
        }
    }

    var date12 = ['01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12', '13', '14', '15', '16', '17', '18', '19', '20', '21', '22', '23', '24', '25', '26', '27', '28', '29', '30', '31']
    var date13 = ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12', '13', '14', '15', '16', '17', '18', '19', '20', '21', '22', '23', '24', '25', '26', '27', '28', '29', '30', '31']
    var j = 0;
    for (j; j <= date13.length; j++) {
        if (date13[j] == date[1]) {
            break;
        }
    }


    var MasterDate = $("#ctl00_lblTime").text()
    var dateGMt = MasterDate.split('(')
    var offsate = dateGMt[1]
    return date12[j] + "-" + months[i] + "-" + date[2]
    //"(" + offsate;
}

function DateConvert(ParamDate, txtdate) {

    if (ParamDate.length == 0) {

        return true;
    }
    if (ParamDate.length < 8) {
        txtdate.value = '';
        //txtdate.focus();
        msgalert("Please Enter Date in DDMMYYYY or dd-Mon-YYYY format only.");
        return false;
    }

    if (ParamDate.length > 8) {

        var Ret_Date = GetDateFromString(ParamDate);
        if ((Ret_Date == '' || isNaN(Ret_Date)) && ((ParamDate.indexOf('UK')) == -1 || (ParamDate.indexOf('UNK')) == -1 || (ParamDate.indexOf('UKUK')) == -1)) {
            // Ret_Date = new Date();
            // txtdate.value = (Ret_Date.getDate().toString().length < 2 ? '0' + Ret_Date.getDate().toString() : Ret_Date.getDate().toString())
            //       + '-' + cMONTHNAMES[Ret_Date.getMonth()] + '-' + Ret_Date.getFullYear().toString();
            txtdate.value = "";
            msgalert("Please Enter Date in DDMMYYYY or dd-Mon-YYYY format only.");
            return false;
        }
        return true;
    }

    var PDay = ParamDate.substr(0, 2);
    var PMonNo = ParamDate.substr(2, 2);
    var PYear = ParamDate.substr(4, 4);

    //alert(PDay);
    //alert(PMonNo);
    //alert(PYear);

    if (PDay > 31 || PMonNo > 12 || PDay < 1 || PMonNo < 1) {
        txtdate.value = '';
        txtdate.focus();
        msgalert("Please Enter Date in DDMMYYYY or dd-Mon-YYYY format only.");
        return false;
    }

    if (PYear < 1800) {
        txtdate.value = '';
        txtdate.focus();
        msgalert("Please Enter Valid Year.");
        return false;
    }

    var DMon = cMONTHNAMES[PMonNo - 1];

    ParamDate = PDay.toString() + '-' + PMonNo + '-' + PYear.toString();

    //alert(ParamDate);

    //alert(isDate(ParamDate));
    //if (!isDate(ParamDate)) {
    //    txtdate.value = '';
    //    txtdate.focus();
    //    //alert("Please Enter Proper Date in DDMMYYYY or dd-Mon-YYYY format only.");
    //    return false;
    //}
    txtdate.value = PDay.toString() + '-' + DMon + '-' + PYear.toString();

    return true;

}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode == 13) {
        return false;
    }
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        if (charCode != 46) {

            return false;
        }

    return true;
}

function isNumber(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode

    if (charCode == 13) {
        return false;
    }
    if (charCode == 43)
        return true
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;


    return true;
}
function isDate(dateStr) {
    var datePat = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
    var matchArray = dateStr.match(datePat); // is the format ok?

    if (matchArray == null) {
        msgalert("Please enter date as either dd/mm/yyyy or dd-mm-yyyy.");
        return false;
    }

    day = matchArray[1]; // p@rse date into variables
    month = matchArray[3];
    year = matchArray[5];

    //month = matchArray[3]; // p@rse date into variables
    //day = matchArray[1];
    //year = matchArray[5];

    if (month < 1 || month > 12) { // check month range
        msgalert("Month must be between 1 and 12.");
        return false;
    }

    if (day < 1 || day > 31) {
        msgalert("Day must be between 1 and 31.");
        return false;
    }

    if ((month == 4 || month == 6 || month == 9 || month == 11) && day == 31) {
        msgalert("Month " + month + " doesn`t have 31 days!")
        return false;
    }

    if (month == 2) { // check for february 29th
        var isleap = (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0));
        if (day > 29 || (day == 29 && !isleap)) {
            msgalert("February " + year + " doesn`t have " + day + " days!");
            return false;
        }
    }
    return true; // date is valid
}

function validatedate(inputText) {
    var dateformat = /^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/;
    // Match the date format through regular expression  
    if (inputText.value.match(dateformat)) {
        document.form1.text1.focus();
        //Test which seperator is used '/' or '-'  
        var opera1 = inputText.value.split('/');
        var opera2 = inputText.value.split('-');
        lopera1 = opera1.length;
        lopera2 = opera2.length;
        // Extract the string into month, date and year  
        if (lopera1 > 1) {
            var pdate = inputText.value.split('/');
        }
        else if (lopera2 > 1) {
            var pdate = inputText.value.split('-');
        }
        var dd = parseInt(pdate[0]);
        var mm = parseInt(pdate[1]);
        var yy = parseInt(pdate[2]);
        // Create list of days of a month [assume there is no leap year by default]  
        var ListofDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
        if (mm == 1 || mm > 2) {
            if (dd > ListofDays[mm - 1]) {
                msgalert('Invalid date format!');
                return false;
            }
        }
        if (mm == 2) {
            var lyear = false;
            if ((!(yy % 4) && yy % 100) || !(yy % 400)) {
                lyear = true;
            }
            if ((lyear == false) && (dd >= 29)) {
                msgalert('Invalid date format!');
                return false;
            }
            if ((lyear == true) && (dd > 29)) {
                msgalert('Invalid date format!');
                return false;
            }
        }
    }
    else {
        alert("Invalid date format!");
        document.form1.text1.focus();
        return false;
    }
}

function enableControlmode4() {

    $('.EditControl').each(function () { this.style.display = "none"; });
    $('.AuditControl').each(function () { this.style.display = "none"; });
}

function endsWith(str, suffix) {
    return str.indexOf(suffix, str.length - suffix.length) !== -1;
}
$("#ctl00_CPHLAMBDA_btnRemarksCancel").click(function () {
    //debugger;
    //$find('mdlRemarks').hide();
    // var btn = document.getElementById('<%=btnHide.ClientID%>');
    //btn.click();


});

        function ReloadPage1() {
            location.reload(false);
        }

        function Redirect() {
            var url = window.location
            location.assign(url)
        }

        function validateForm() {
            document.getElementById('<%= HFHeaderLabel.ClientId %>').value = document.getElementById('<%=HeaderLabel.ClientID%>').innerHTML;
            document.getElementById('<%= HFHeaderPDF.ClientId %>').value = document.getElementById('<%=divHeaderPDF.ClientId %>').innerHTML;
            return true;
        }

        function characterlimit(id) {

            var text = id.value
            var textLength = text.length;
            if (textLength > 150) {
                // if (id.value.length > 500) {
                $(id).val(text.substring(0, (150)));
                msgalert("Only 150 characters are allowed");
                return false;
            }
            else {
                return true;
                // $("#lblCharLeft").text((maxLength - textLength) + " characters left.");
            }

        }

        function characterforAttach(id) {

            var text = id.value
            var textLength = text.length;
            if (textLength > 50) {
                $(id).val(text.substring(0, (50)));
                msgalert("Only 50 characters are allowed.");
                return false;
            }
            else {
                return true;

            }

        }
        function AlertAndRedirect(URL, msg) {
            alertdooperation(msg, 1, URL);
        }

    </script>

</asp:Content>
