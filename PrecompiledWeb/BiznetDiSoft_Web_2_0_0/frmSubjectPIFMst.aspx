<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmSubjectPIFMst, App_Web_22suyskz" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="txtOtherProof" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/popcalendar.js"></script>

    <script type="text/javascript" src="Script/General.js"></script>

    <script type="text/javascript" src="Script/Validation.js"></script>

    <script type="text/javascript" src="Script/AutoComplete.js" language="javascript"></script>

    <%--
    <asp:updatepanel id ="upsubjectpif" runat="server">  
    <contenttemplate>--%>
    <table width="100%">
        <tbody>
            <tr>
                <td>
                    <div style="display: none; left: 424px; width: 700px; top: 367px; height: 440px;
                        text-align: left" id="divQCDtl" class="DIVSTYLE2" runat="server">
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
                                    <td style="width: 30%; text-align: right;" class="Label">
                                        Remarks/Response :
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtQCRemarks" runat="server" CssClass="textBox" Width="229px" Height="46px"
                                            TextMode="MultiLine" MaxLength="1000"></asp:TextBox>
                                        <asp:Label ID="lblcnt" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;" class="Label">
                                        To :
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtToEmailId" runat="server" CssClass="textBox" Width="230px" MaxLength="500"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;" class="Label">
                                        CC :
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtCCEmailId" runat="server" CssClass="textBox" Width="231px" MaxLength="500"
                                            Height="37px" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center;" colspan="2">
                                        <%--<input id="BtnQCSave" runat="server" class="button" type="button" value="Save & Send" style="width: 91px" />--%><asp:Button
                                            ID="BtnQCSave" runat="server" Text="Save" CssClass="btn btnsave"
                                            ToolTip="Save" OnClientClick="return ValidationQC();" />
                                        <asp:Button  ID="BtnQCSaveSend" runat="server" Text="Save & Send"
                                            CssClass="btm btsave" ToolTip="Save & Send" OnClientClick="return ValidationQC();  " />
                                        <input id="btnExitQC" class="btn btnexit" onclick="QCDivShowHide('H');" type="button"
                                            value="Close" runat="server" title="Close"  />
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
                                    <asp:BoundField DataField="dQCGivenOn" HeaderText="QC Date" DataFormatString="{0:dd-MMM-yyyy}"
                                        HtmlEncode="False">
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vResponse" HeaderText="Response" />
                                    <asp:BoundField DataField="vResponseGivenBy" HeaderText="Response BY">
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="dResponseGivenOn" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Response Date"
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
                    <table style="width: 80%; margin: auto;">
                        <tbody>
                            <tr align="center">
                                <td style="text-align: left;">
                                    <strong>Personal Information Form</strong>
                                    <hr style="background-image: none; width: 100%; color: #ffcc66; background-color: #ffcc66"
                                        class="hr" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table style="width: 100%" id="TABLE1">
                                        <tbody>
                                            <tr>
                                                <td style="white-space: nowrap; text-align: right; width: 25%" class="Label" id="tdSubject"
                                                    runat="server">
                                                    Search Subject:
                                                </td>
                                                <td style="text-align: left; width: 44%">
                                                    <asp:TextBox ID="txtSubject" TabIndex="2" runat="server" CssClass="textBox" Width="100%" />
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:Button Style="display: none" ID="btnEdit" TabIndex="2" runat="server" Text=" Edit "
                                                        ToolTip="Edit" CssClass="btn btnsave" />
                                                    <asp:Button ID="BtnNew" runat="server" Text="New" ToolTip="New" CssClass="btn btnnew" />
                                                    <asp:Button ID="btnMSR" runat="server" Text="MSR" ToolTip=" Medical Screening Report"
                                                        CssClass="btn btnnew" OnClientClick="return ValidationMSR();" />
                                                    <input id="btnQC" class="btn btnnew" onclick="return QCDivShowHide('S');" type="button"
                                                        value="QC" title="QC Report" runat="server" />
                                                    <asp:Button ID="btnExit" runat="server" Text="Exit" ToolTip="Exit" CssClass="btn btnexit"
                                                        OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" ServicePath="AutoComplete.asmx"
                                                        OnClientShowing="ClientPopulated" CompletionSetCount="10" OnClientItemSelected="OnSelected"
                                                        UseContextKey="True" MinimumPrefixLength="1" ServiceMethod="GetSubjectCompletionList_NotRejected"
                                                        CompletionListElementID="pnlSubjectList" CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                        CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1"
                                                        TargetControlID="txtSubject">
                                                    </cc1:AutoCompleteExtender>
                                                    <asp:Panel ID="pnlSubjectList" runat="server" Style="max-height: 300px; overflow: auto;
                                                        overflow-x: hidden" />
                                                    <asp:HiddenField ID="HSubjectId" runat="server"></asp:HiddenField>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <table cellpadding="3px" width="80%" style="margin: auto;">
                        <tbody>
                            <tr>
                                <td style="text-align: left;">
                                    <cc1:TabContainer ID="tbConPIFMst" TabIndex="50" runat="server" Width="100%" AutoPostBack="True"
                                        ActiveTabIndex="0">
                                        <cc1:TabPanel runat="server" HeaderText="tbPnlPersonalInfo" ID="tbPnlPersonalInfo">
                                            <HeaderTemplate>
                                                Personal Information
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                <table align="center">
                                                    <tbody>
                                                        <tr>
                                                            <td style="width: 22%; text-align: left;" class="Label">
                                                                Last Name*:
                                                            </td>
                                                            <td style="text-align: left;" colspan="2">
                                                                <asp:TextBox ID="txtlastname" TabIndex="3" runat="server" CssClass="textBox" MaxLength="50"
                                                                    Width="35%" />
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="tabPanelOne"
                                                                    SetFocusOnError="True" Display="Dynamic" ErrorMessage="*" ControlToValidate="txtlastname">*</asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left;" class="Label">
                                                                First Name*:
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:TextBox ID="txtfirstname" TabIndex="4" runat="server" MaxLength="152" CssClass="textBox"
                                                                    Width="52%" />
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="tabPanelOne"
                                                                    SetFocusOnError="True" Display="Dynamic" ErrorMessage="*" ControlToValidate="txtfirstname">*</asp:RequiredFieldValidator>
                                                            </td>
                                                            <td style="text-align: left;" class="Label">
                                                                Project:<asp:DropDownList ID="ddlprojectno" TabIndex="30" runat="server" CssClass="dropDownList"
                                                                    Width="100%" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left;" class="Label">
                                                                Middle Name:
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:TextBox ID="txtmiddlename" MaxLength="50" TabIndex="5" runat="server" CssClass="textBox"
                                                                    Width="52%" />
                                                            </td>
                                                            <td style="width: 249px; text-align: center" class="Label" valign="top" align="right"
                                                                colspan="2" rowspan="15">
                                                                <br />
                                                                Photo<br />
                                                                <asp:Image ID="Image1" runat="server" Width="80%" Height="125px" ImageUrl="frmPIFImage.aspx">
                                                                </asp:Image>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left;" class="Label">
                                                                Initials:
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:TextBox ID="txtInitials" TabIndex="6" MaxLength="5" runat="server" CssClass="textBox"
                                                                    Width="52%" />
                                                                <asp:HiddenField ID="hfinitials" runat="server"></asp:HiddenField>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left;" class="Label">
                                                                Ref. Subject Code:
                                                            </td>
                                                            <td style="text-align: left; white-space: nowrap;">
                                                                <asp:TextBox ID="txtRefSubectCode" TabIndex="7" MaxLength="50" runat="server" CssClass="textBox"
                                                                    Width="52%" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left;" class="Label">
                                                                Height*:
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:TextBox onblur="CheckHeight(this);" ID="txtheight" MaxLength="5" TabIndex="8"
                                                                    runat="server" CssClass="textBox" Width="52%"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="tabPanelOne"
                                                                    SetFocusOnError="True" Display="Dynamic" ErrorMessage="*" ControlToValidate="txtheight">*</asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left;" class="Label">
                                                                Weight*:
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:TextBox ID="txtweight" TabIndex="9" MaxLength="5" runat="server" CssClass="textBox"
                                                                    Width="52%" />
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ValidationGroup="tabPanelOne"
                                                                    SetFocusOnError="True" Display="Dynamic" ErrorMessage="*" ControlToValidate="txtweight">*</asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left;" class="Label">
                                                                BMI:
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:TextBox ID="txtbmi" TabIndex="10" MaxLength="5" runat="server" CssClass="textBox"
                                                                    Width="52%" />
                                                                <asp:HiddenField ID="hfbmi" runat="server"></asp:HiddenField>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left;" class="Label">
                                                                Date Of Birth:
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:TextBox onblur="DateConvert_Age(this.value,this,'Y')" ID="txtdob" TabIndex="11"
                                                                    runat="server" CssClass="textBox" Width="43%" MaxLength="8000" />
                                                                <asp:Label ID="lbldbo" TabIndex="53" runat="server" Font-Size="Small" Text='(e.g."01012009"or "01-Jan-2008")' />
                                                                <asp:Button ID="btnCheckDuplicateSubject" Style="display: none;" runat="server" Text="Check Duplicate Subject"
                                                                    CssClass="btn btnnew" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left;" class="Label">
                                                                Date of Enrollment Reporting*:
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:TextBox onblur="DateConvert_Age(this.value,this,'Y')" ID="txtdoer" TabIndex="12"
                                                                    runat="server" CssClass="textBox" Width="43%" MaxLength="8000" Height="14px"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="tabPanelOne"
                                                                    SetFocusOnError="True" Display="Dynamic" ErrorMessage="*" ControlToValidate="txtdoer"
                                                                    ToolTip="Enrollment Date is required.">*</asp:RequiredFieldValidator>
                                                                <asp:Label ID="lblDateEnroll" TabIndex="52" runat="server" Font-Size="Small" Text='(e.g."01012009"or "01-Jan-2008")' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left;" class="Label">
                                                                Age:
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:TextBox Enabled="False" ID="txtAge" TabIndex="13" MaxLength="3" runat="server"
                                                                    CssClass="textBox" Width="43%"></asp:TextBox>
                                                                Years.
                                                                <asp:HiddenField ID="hfAge" runat="server"></asp:HiddenField>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left;" class="Label">
                                                                Sex:
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:RadioButtonList ID="rblsex" TabIndex="14" runat="server" AutoPostBack="True"
                                                                    RepeatDirection="Horizontal">
                                                                    <asp:ListItem Value="M" Selected="True">Male</asp:ListItem>
                                                                    <asp:ListItem Value="F">Female</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left;" class="Label">
                                                                Marital Status:
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:RadioButtonList ID="rblMartialStatus" TabIndex="15" runat="server" RepeatDirection="Horizontal">
                                                                    <asp:ListItem Value="S">Single</asp:ListItem>
                                                                    <asp:ListItem Value="M">Married</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left;" class="Label">
                                                                Food Habit:
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:RadioButtonList ID="rdoLstFoodHabit" TabIndex="16" runat="server">
                                                                    <asp:ListItem Selected="True">Vegetarian</asp:ListItem>
                                                                    <asp:ListItem>Non-Vegetarian</asp:ListItem>
                                                                    <asp:ListItem>Eggetarian</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left;" class="Label">
                                                                Blood Group:
                                                            </td>
                                                            <td style="text-align: left;" colspan="2">
                                                                <asp:DropDownList ID="ddlbloodgroup" TabIndex="17" runat="server" CssClass="dropDownList"
                                                                    Width="15%">
                                                                    <asp:ListItem></asp:ListItem>
                                                                    <asp:ListItem Value="A">A</asp:ListItem>
                                                                    <asp:ListItem Value="B">B</asp:ListItem>
                                                                    <asp:ListItem Value="AB">AB</asp:ListItem>
                                                                    <asp:ListItem Value="O">O</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="ddlRh" TabIndex="18" runat="server" CssClass="dropDownList"
                                                                    Width="15%">
                                                                    <asp:ListItem></asp:ListItem>
                                                                    <asp:ListItem Value="P">+Ve</asp:ListItem>
                                                                    <asp:ListItem Value="N">-Ve</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left;" class="Label">
                                                                Education Qualification:
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:TextBox ID="txteducationquali" MaxLength="50" TabIndex="19" runat="server" CssClass="textBox"
                                                                    Width="30%"></asp:TextBox>
                                                            </td>
                                                            <td style="width: 249px" align="left" colspan="2" rowspan="1">
                                                                <asp:Button ID="Button1" TabIndex="19" runat="server" Text="Capture Photo" CssClass="button"
                                                                    Width="101px" Visible="False"></asp:Button>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left;" class="Label">
                                                                ICF Required In Lang.
                                                            </td>
                                                            <td style="text-align: left;" colspan="2">
                                                                <asp:Panel ID="pnlSubject" runat="server" Width="40%" ScrollBars="Auto" BorderStyle="Solid"
                                                                    BorderWidth="1px">
                                                                    <asp:CheckBoxList ID="chklstIcfLanguage" runat="server" CssClass="checkboxlist" RepeatColumns="2"
                                                                        RepeatDirection="Horizontal">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left;" class="Label">
                                                                Occupation:
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:TextBox ID="txtoccupation" TabIndex="21" MaxLength="50" runat="server" CssClass="textBox"
                                                                    Width="52%" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left;" class="Label">
                                                                Photocopy Of Proof To be Attached:
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:CheckBoxList ID="cblProofOfAge" TabIndex="22" runat="server" onClick="check_Other();">
                                                                    <asp:ListItem Value="SLC">School Leaving Certificate</asp:ListItem>
                                                                    <asp:ListItem Value="BC">Birth Certificate</asp:ListItem>
                                                                    <asp:ListItem Value="DL">Driving License</asp:ListItem>
                                                                    <asp:ListItem Value="EC">Election Card</asp:ListItem>
                                                                    <asp:ListItem Value="IIC">Institutional I-Card</asp:ListItem>
                                                                    <asp:ListItem Value="RC">Ration Card</asp:ListItem>
                                                                    <asp:ListItem Value="PC">Pan Card</asp:ListItem>
                                                                    <asp:ListItem Value="AC">Aadhaar Card</asp:ListItem>
                                                                    <asp:ListItem Value="PP">Passport</asp:ListItem>
                                                                    <asp:ListItem Value="O">Others</asp:ListItem>
                                                                </asp:CheckBoxList>
                                                                If other,
                                                                <asp:TextBox ID="txtotherProf" TabIndex="23" runat="server" MaxLength="50" CssClass="textBox"
                                                                    Width="23%"></asp:TextBox>
                                                            </td>
                                                            <td colspan="2" rowspan="1">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left;" class="Label">
                                                                Smoking/Chewing/Alcohol History:
                                                            </td>
                                                            <td style="text-align: left; vertical-align: top;" colspan="4">
                                                                <asp:GridView ID="GVHabits" TabIndex="24" runat="server" SkinID="grdView_Small" AutoGenerateColumns="False"
                                                                    OnRowDataBound="GVHabits_RowDataBound" OnRowCreated="GVHabits_RowCreated">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="nSubjectHabitDetailsNo" HeaderText="SubjectHabitDetailsNo">
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="vSubjectId" HeaderText="SubjectId" />
                                                                        <asp:BoundField DataField="iScreenId" HeaderText="ScreenId" />
                                                                        <asp:BoundField DataField="vHabitId" HeaderText="HabitId" />
                                                                        <asp:BoundField DataField="vHabitDetails" HeaderText="Habit Details" />
                                                                        <asp:TemplateField HeaderText="Habit">
                                                                            <ItemTemplate>
                                                                                <asp:DropDownList TabIndex="24" ID="ddlHebitType" runat="server" CssClass="dropDownList"
                                                                                    SelectedValue='<%# Bind("cHabitFlag") %>' Width="92px">
                                                                                    <asp:ListItem Selected="True" Value="N">Never</asp:ListItem>
                                                                                    <asp:ListItem Value="C">Current</asp:ListItem>
                                                                                    <asp:ListItem Value="P">Previous</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Consumption Detail">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox TabIndex="24" ID="txtConsumption" runat="server" CssClass="textBox"
                                                                                    Text='<%# Bind("vRemarks") %>' Width="200px"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="If Previous, stopped since">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox TabIndex="24" ID="txtEndDate" Width="200px" runat="server" CssClass="textBox"
                                                                                    Text='<%# Bind("vHabitHistory") %>'></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="cHabitFlag" HeaderText="HabitFlag" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left;" class="Label">
                                                                Attachments:
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:DropDownList ID="ddlProofType" TabIndex="25" runat="server" CssClass="dropDownList">
                                                                    <asp:ListItem>--Select--</asp:ListItem>
                                                                    <asp:ListItem>School Leaving certificate</asp:ListItem>
                                                                    <asp:ListItem>Birth Certificate</asp:ListItem>
                                                                    <asp:ListItem>Driving License</asp:ListItem>
                                                                    <asp:ListItem>Election Card</asp:ListItem>
                                                                    <asp:ListItem>Institutional I-Card</asp:ListItem>
                                                                    <asp:ListItem>Ration Card</asp:ListItem>
                                                                    <asp:ListItem>Pan Card</asp:ListItem>
                                                                    <asp:ListItem>Aadhaar Card</asp:ListItem>
                                                                    <asp:ListItem>Passport</asp:ListItem>
                                                                    <asp:ListItem>Others</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:FileUpload ID="FlAttachment" TabIndex="26" runat="server"></asp:FileUpload>
                                                                <asp:Button ID="btnAttach" TabIndex="27" runat="server" Text="Attach" ToolTip="Attach Document"
                                                                    CssClass="btn btnnew" />
                                                                <br />
                                                                If other,
                                                                <asp:TextBox ID="txtAttach" TabIndex="28" runat="server" CssClass="textBox" Width="23%"></asp:TextBox>
                                                                <asp:GridView ID="GVSubjectProof" runat="server" SkinID="grdView_Small" AutoGenerateColumns="False">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="nSubjectProofNo" HeaderText="SubjectProofNo" />
                                                                        <asp:BoundField DataField="vSubjectId" HeaderText="SubjectId" />
                                                                        <asp:BoundField DataField="iTranNo" HeaderText="TranNo" />
                                                                        <asp:BoundField DataField="vProofType" HeaderText="ProofType" />
                                                                        <asp:BoundField DataField="vProofPath" HeaderText="ProofPath" />
                                                                        <asp:BoundField DataField="iModifyBy" HeaderText="ModifyBy" />
                                                                        <asp:BoundField DataField="dModifyOn" HeaderText="ModifyOn" />
                                                                        <asp:BoundField DataField="cStatusIndi" HeaderText="Status" />
                                                                        <asp:TemplateField HeaderText="Attachment">
                                                                            <ItemTemplate>
                                                                                <asp:HyperLink ID="hlnkFile" runat="server" Target="_blank" Text='<%# Eval("vProofPath") %>'>
                                                                                </asp:HyperLink>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Delete">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="imgbtnDelete" runat="server" ImageUrl="~/Images/i_delete.gif"
                                                                                    OnClientClick="return msgconfirmalert('Are You Sure You Want To Delete?',this)" ToolTip="Delete" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                            <td style="width: 249px" align="left" colspan="2" rowspan="1">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right;" colspan="5" rowspan="1">
                                                                <asp:Button ID="btnNext" TabIndex="29" runat="server" Text=" Next >>" ToolTip="Next "
                                                                    CssClass="btn btnnew" OnClientClick="return ValidateTab();" ValidationGroup="tabPanelOne" />
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                        <cc1:TabPanel runat="server" HeaderText="tbPnlFemalDetails" Enabled="False" ID="tbPnlFemaleDetails">
                                            <HeaderTemplate>
                                                For Female Only
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                <table cellpadding="3" width="100%">
                                                    <tbody>
                                                        <tr>
                                                            <td>
                                                                <strong>Last Menstrual Period</strong>
                                                                <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66; width: 100%;"
                                                                    class="hr" />
                                                                <table style="width: 100%">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td class="Label" style="text-align: left; width: 10%">
                                                                                Date:
                                                                            </td>
                                                                            <td style="text-align: left;">
                                                                                <asp:TextBox onblur="DateConvert_Age(this.value,this,'Y')" ID="txtmendate" TabIndex="1"
                                                                                    runat="server" CssClass="textBox" Width="35%" MaxLength="8000"></asp:TextBox>
                                                                                <img id="img4" onclick="popUpCalendar(this,ctl00_CPHLAMBDA_tbConPIFMst_tbPnlFemaleDetails_txtmendate,'dd-mmm-yyyy');"
                                                                                    alt="Select  Date " src="images/calendar.gif" style="vertical-align: middle;" />(e.g."01012009"or
                                                                                "01-Jan-2008")
                                                                            </td>
                                                                            <td style="text-align: left;" class="Label">
                                                                                Cycle Length:
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtcyclelength" TabIndex="2" MaxLength="4" runat="server" CssClass="textBox"
                                                                                    Width="90%" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="text-align: left;" class="Label">
                                                                                Regular:
                                                                            </td>
                                                                            <td style="text-align: left;" class="Label">
                                                                                <asp:RadioButtonList ID="rdoLstRegular" TabIndex="3" runat="server" RepeatDirection="Horizontal">
                                                                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                                                    <asp:ListItem Value="N">No</asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                            </td>
                                                                            <td style="text-align: left;" class="Label">
                                                                                Association with Pain:
                                                                            </td>
                                                                            <td style="text-align: left;">
                                                                                <asp:RadioButtonList ID="rblPain" TabIndex="4" runat="server" RepeatDirection="Horizontal">
                                                                                    <asp:ListItem Value="0">Painful</asp:ListItem>
                                                                                    <asp:ListItem Value="1">Painless</asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left;">
                                                                <strong>Obstetric History</strong>
                                                                <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66; width: 100%;"
                                                                    class="hr" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table style="width: 100%" align="center">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td class="Label" style="text-align: left; width: 20%;">
                                                                                Date of Last Delivery:
                                                                            </td>
                                                                            <td style="text-align: left; width: 43%" colspan="3">
                                                                                <asp:TextBox onblur="DateConvert_Age(this.value,this,'Y')" ID="txtdold" TabIndex="5"
                                                                                    runat="server" MaxLength="8000" CssClass="textBox" Width="20%"></asp:TextBox>
                                                                                <img id="Img5" onclick="popUpCalendar(this,ctl00_CPHLAMBDA_tbConPIFMst_tbPnlFemaleDetails_txtdold,'dd-mmm-yyyy');"
                                                                                    alt="Select  Date of Last Delivary" src="images/calendar.gif" style="vertical-align: middle;" />(e.g."01012009"or
                                                                                "01-Jan-2008")
                                                                            </td>
                                                                            <%--<td align="left">
                                                                            </td>
                                                                            <td align="left">
                                                                                &nbsp;
                                                                            </td>--%>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="Label" style="text-align: left;">
                                                                                Gravida:
                                                                            </td>
                                                                            <td style="text-align: left; width: 43%">
                                                                                <asp:TextBox ID="txtgravida" TabIndex="6" MaxLength="50" runat="server" CssClass="textBox"
                                                                                    Width="38%"></asp:TextBox>
                                                                            </td>
                                                                            <td class="Label" style="text-align: left;">
                                                                                Para:
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtpara" TabIndex="7" MaxLength="50" runat="server" CssClass="textBox"
                                                                                    Width="60%"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="Label" style="text-align: left;">
                                                                                No.of Live Children:
                                                                            </td>
                                                                            <td colspan="3" style="text-align: left;">
                                                                                <asp:TextBox ID="txtnoofchildren" TabIndex="8" runat="server" CssClass="textBox"
                                                                                    Width="20%" MaxLength="4"></asp:TextBox>
                                                                            </td>
                                                                            <%--  <td align="left">
                                                                            </td>
                                                                            <td align="left">
                                                                                &nbsp;
                                                                            </td>--%>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="Label" style="text-align: left;">
                                                                                No. of children died:
                                                                            </td>
                                                                            <td style="text-align: left;">
                                                                                <asp:TextBox ID="txtNoChildernDied" TabIndex="9" runat="server" CssClass="textBox"
                                                                                    Width="38%"></asp:TextBox>
                                                                            </td>
                                                                            <td id="Td1" class="Label" style="text-align: left;" runat="server">
                                                                                Remarks if children died:
                                                                            </td>
                                                                            <td id="Td17" style="text-align: left;" runat="server">
                                                                                <asp:TextBox ID="txtRemarkChDied" TabIndex="10" runat="server" CssClass="textBox"
                                                                                    Width="60%" MaxLength="4"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="Label" style="text-align: left;">
                                                                                All Children Healthy:
                                                                            </td>
                                                                            <td style="text-align: left;">
                                                                                <asp:RadioButtonList ID="rblchildrenhealthy" TabIndex="11" runat="server" RepeatDirection="Horizontal">
                                                                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                                                    <asp:ListItem Value="N">No</asp:ListItem>
                                                                                    <asp:ListItem Value="A">NA</asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                            </td>
                                                                            <td id="Td2" class="Label" style="text-align: left;" runat="server">
                                                                                Remarks:
                                                                            </td>
                                                                            <td style="text-align: left;" id="Td3" runat="server">
                                                                                <asp:TextBox ID="txtChildrenRemark" TabIndex="12" runat="server" CssClass="textBox"
                                                                                    Width="60%" MaxLength="500"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="Label" style="text-align: left;">
                                                                                Any Spontaneous Abortion/MTP:
                                                                            </td>
                                                                            <td style="text-align: left;">
                                                                                <asp:RadioButtonList ID="rblAbortion" TabIndex="15" runat="server" RepeatDirection="Horizontal">
                                                                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                                                    <asp:ListItem Value="N">No</asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                            </td>
                                                                            <td id="Td4" class="Label" style="text-align: left;" runat="server">
                                                                                Date Of Last Abortion:
                                                                            </td>
                                                                            <td id="Td5" style="text-align: left;" runat="server">
                                                                                <asp:TextBox onblur="DateConvert_Age(this.value,this,'Y')" ID="txtdolabortion" TabIndex="14"
                                                                                    runat="server" CssClass="textBox" Width="60%" MaxLength="8000"></asp:TextBox>
                                                                                <img id="Img2" onclick="popUpCalendar(this,ctl00_CPHLAMBDA_tbConPIFMst_tbPnlFemaleDetails_txtdolabortion,'dd-mmm-yyyy');"
                                                                                    alt="Select  Date of Last Delivary" src="images/calendar.gif" style="vertical-align: middle;" />(e.g."01012009"or
                                                                                "01-Jan-2008")
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="Label" style="text-align: left;">
                                                                                Lactating:
                                                                            </td>
                                                                            <td style="text-align: left;" colspan="2">
                                                                                <asp:RadioButtonList ID="RBLLactating" TabIndex="15" runat="server" RepeatDirection="Horizontal">
                                                                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                                                    <asp:ListItem Value="N">No</asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                            </td>
                                                                            <%--<td style="width: 120px; height: 21px" id="Td9" align="left" runat="server">
                                                                            </td>
                                                                            <td style="height: 21px" id="Td10" align="left" runat="server">
                                                                                &nbsp;
                                                                            </td>--%>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="Label" style="text-align: left;">
                                                                                Volunteer is in the child bearing age:
                                                                            </td>
                                                                            <td style="text-align: left;" colspan="3">
                                                                                <asp:RadioButtonList ID="rblVolunteer" TabIndex="15" runat="server" RepeatDirection="Horizontal">
                                                                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                                                    <asp:ListItem Value="N">No</asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                            </td>
                                                                            <%-- <td style="width: 120px; height: 21px" id="Td18" align="left" runat="server">
                                                                            </td>
                                                                            <td style="height: 21px" id="Td19" align="left" runat="server">
                                                                            </td>--%>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left;">
                                                                <strong>Family Planning Measures</strong>
                                                                <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66; width: 100%;"
                                                                    class="hr" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" align="left">
                                                                <table style="width: 100%" align="center">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td style="width: 20%; text-align: left;" class="Label">
                                                                                Family Planing Measures:
                                                                            </td>
                                                                            <td style="text-align: left;" colspan="2">
                                                                                <asp:RadioButtonList ID="rblcontraception" TabIndex="16" runat="server" RepeatDirection="Horizontal">
                                                                                    <asp:ListItem Value="P">Permanent Contraception</asp:ListItem>
                                                                                    <asp:ListItem Value="T">Temporary Contraception</asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                            </td>
                                                                            <%-- <td style="height: 20px" align="left">
                                                                            </td>--%>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="text-align: left;" class="Label">
                                                                                Details of Contraception:
                                                                            </td>
                                                                            <td style="text-align: left;" colspan="2">
                                                                                <asp:CheckBoxList ID="chkContraception" TabIndex="17" runat="server" RepeatDirection="Horizontal"
                                                                                    CellSpacing="1" CellPadding="1">
                                                                                    <asp:ListItem Value="B">Double Barrier</asp:ListItem>
                                                                                    <asp:ListItem Value="P">Pills</asp:ListItem>
                                                                                    <asp:ListItem Value="R">Rhythm</asp:ListItem>
                                                                                    <asp:ListItem Value="I">IUCD</asp:ListItem>
                                                                                </asp:CheckBoxList>
                                                                            </td>
                                                                            <%-- <td style="height: 18px" align="left">
                                                                            </td>--%>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="text-align: left;" class="Label">
                                                                                Remarks:
                                                                            </td>
                                                                            <td style="text-align: left;" colspan="3" rowspan="1">
                                                                                <asp:TextBox ID="txtOtherRemarks" TabIndex="19" runat="server" CssClass="textBox"
                                                                                    Width="35%" TextMode="MultiLine" MaxLength="500"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table style="width: 100%">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Button ID="btnPnl1Prev" TabIndex="21" runat="server" Text="<< Previous" ToolTip="Previous Page"
                                                                                    CssClass="btn btnnew" OnClientClick="return ValidateTab();" />
                                                                            </td>
                                                                            <td style="text-align: right;">
                                                                                <asp:Button ID="btnPnl1Next" TabIndex="20" runat="server" Text=" Next >>" ToolTip="Next Page "
                                                                                    CssClass="btn btnnew" OnClientClick="return ValidateTab();" ValidationGroup="tabPanelOne" />
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                        <cc1:TabPanel runat="server" HeaderText="TabPanel3" ID="tbPnlAddress">
                                            <HeaderTemplate>
                                                Contact Information
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                <table cellpadding="3" width="100%">
                                                    <tbody>
                                                        <tr>
                                                            <td class="Label" style="text-align: left; width: 20%;">
                                                                Local Address(1):
                                                            </td>
                                                            <td style="text-align: left; width: 40%;">
                                                                <asp:TextBox ID="txtlocaladds1" TabIndex="4" runat="server" CssClass="textBox" Width="60%"
                                                                    TextMode="MultiLine" MaxLength="250"></asp:TextBox>
                                                            </td>
                                                            <td class="Label" style="text-align: left; width: 18%;">
                                                                Local Tel1 No:
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:TextBox ID="txtLocaltel1no" MaxLength="50" TabIndex="5" runat="server" CssClass="textBox"
                                                                    Width="90%"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="Label" style="text-align: left;">
                                                                Local Address(2):
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:TextBox ID="txtlocaladd2" TabIndex="6" runat="server" CssClass="textBox" Width="60%"
                                                                    TextMode="MultiLine" MaxLength="250"></asp:TextBox>
                                                            </td>
                                                            <td class="Label" style="text-align: left;">
                                                                Local Tel2 No:
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:TextBox ID="txtLocaltel2no" MaxLength="50" TabIndex="7" runat="server" CssClass="textBox"
                                                                    Width="90%"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="Label" style="text-align: left;">
                                                                Permanent Address:
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:TextBox ID="txtPermanentAdds" TabIndex="8" runat="server" CssClass="textBox"
                                                                    Width="60%" TextMode="MultiLine" MaxLength="250">SAME AS LOCAL</asp:TextBox>
                                                            </td>
                                                            <td style="text-align: left;" id="Td6" class="Label" runat="server">
                                                                Permanent Tel No:
                                                            </td>
                                                            <td style="text-align: left;" id="Td7" runat="server">
                                                                <asp:TextBox ID="txtpertelno" MaxLength="50" TabIndex="9" runat="server" CssClass="textBox"
                                                                    Width="90%"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="Label" style="text-align: left;">
                                                                Place:
                                                            </td>
                                                            <td style="text-align: left" colspan="3">
                                                                <asp:TextBox ID="txtPlace" TabIndex="10" MaxLength="50" runat="server" CssClass="textBox"
                                                                    Width="30%"></asp:TextBox>
                                                            </td>
                                                            <%--<td style="height: 40px" class="Label" align="left" runat="server">
                                                            </td>
                                                            <td style="height: 40px" align="left" runat="server">
                                                            </td>--%>
                                                        </tr>
                                                        <tr>
                                                            <td class="Label" style="text-align: left;">
                                                                Office Address:
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:TextBox ID="txtOfficeAddress" TabIndex="11" runat="server" CssClass="textBox"
                                                                    Width="60%" TextMode="MultiLine" MaxLength="250"></asp:TextBox>
                                                            </td>
                                                            <td id="Td8" class="Label" runat="server" style="text-align: left;">
                                                                Office Tel No:
                                                            </td>
                                                            <td id="Td9" style="text-align: left;" runat="server">
                                                                <asp:TextBox ID="txtOfficetelno" MaxLength="50" TabIndex="12" runat="server" CssClass="textBox"
                                                                    Width="90%"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                                <strong>Contact Person:
                                                    <hr style="background-image: none; width: 100%; color: #ffcc66; background-color: #ffcc66" />
                                                </strong>&nbsp;<br />
                                                <table cellpadding="5" width="100%">
                                                    <tbody>
                                                        <tr>
                                                            <td class="Label" style="text-align: left; width: 20%;">
                                                                Contact Person Name(1):
                                                            </td>
                                                            <td style="text-align: left; width: 40%;">
                                                                <asp:TextBox ID="txtconper" MaxLength="100" TabIndex="13" runat="server" CssClass="textBox"
                                                                    Width="60%"></asp:TextBox>
                                                            </td>
                                                            <td style="text-align: left; width: 18%;" class="Label">
                                                                Contact Person Name(2):
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:TextBox ID="txtContactName2" TabIndex="16" MaxLength="100" runat="server" CssClass="textBox"
                                                                    Width="100%" Height="13px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left;" class="Label">
                                                                Contact Person Adds.(1):
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:TextBox ID="txtconperadds1" TabIndex="14" MaxLength="250" runat="server" CssClass="textBox"
                                                                    Width="60%" TextMode="MultiLine"></asp:TextBox>
                                                            </td>
                                                            <td style="text-align: left;" class="Label ">
                                                                Contact Person Adds.(2):
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:TextBox ID="txtconperadds2" TabIndex="17" MaxLength="250" runat="server" CssClass="textBox"
                                                                    Width="100%" TextMode="MultiLine"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left;" class="Label">
                                                                Contact Person Tel No(1):
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:TextBox ID="txtconpertel1" TabIndex="15" MaxLength="50" runat="server" CssClass="textBox"
                                                                    Width="60%"></asp:TextBox>
                                                            </td>
                                                            <td style="text-align: left;" class="Label">
                                                                Contact Person Tel No(2):
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:TextBox ID="txtconpertel2" TabIndex="18" runat="server" MaxLength="50" CssClass="textBox"
                                                                    Width="100%"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left;" class="Label">
                                                                Referred By:
                                                            </td>
                                                            <td style="text-align: left;" colspan="3">
                                                                <asp:TextBox ID="txtRefBy" TabIndex="19" MaxLength="50" runat="server" CssClass="textBox"
                                                                    Width="30%" Height="13px"></asp:TextBox>
                                                            </td>
                                                            <%--  <td style="width: 165px" align="left">
                                                            </td>
                                                            <td style="height: 20px" align="left">
                                                            </td>--%>
                                                        </tr>
                                                        <tr>
                                                            <td class="Label" align="left">
                                                            </td>
                                                            <td align="left" colspan="3" rowspan="1">
                                                                <asp:Label ID="lblLastRemark" runat="server" CssClass="Label"></asp:Label>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left;" class="Label">
                                                                Remarks:
                                                            </td>
                                                            <td align="left" colspan="3">
                                                                <asp:DropDownList ID="ddlRemark" runat="server" CssClass="dropDownList" Width="30%"
                                                                    onchange="return ddlRemarkSelected();">
                                                                    <asp:ListItem>SELECT REMARK</asp:ListItem>
                                                                    <asp:ListItem>1.PIF ENTRY</asp:ListItem>
                                                                    <asp:ListItem>2.BLOOD GROUP ENTRY</asp:ListItem>
                                                                    <asp:ListItem>3.CONTACT DETAILS ENTRY</asp:ListItem>
                                                                    <asp:ListItem>4.DOCUMENT ATTACHED</asp:ListItem>
                                                                    <asp:ListItem>5.QC CORRECTION</asp:ListItem>
                                                                    <asp:ListItem>6. OTHERS</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <br />
                                                                <asp:TextBox ID="txtRemark" TabIndex="20" runat="server" CssClass="textBox" Width="30%"
                                                                    Height="42px" TextMode="MultiLine" MaxLength="500"></asp:TextBox>
                                                            </td>
                                                            <td style="height: 20px" align="left">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left;">
                                                                <asp:Button ID="btnPnl3Prev" TabIndex="21" runat="server" Text="<< Previous" ToolTip="Previous Page"
                                                                    CssClass="btn btnnew" OnClientClick="return ValidateTab();"></asp:Button>
                                                            </td>
                                                            <td style="text-align: center;" colspan="3">
                                                                <asp:Button ID="btnsave" TabIndex="21" runat="server" Text="Save" ToolTip="Save"
                                                                    CssClass="btn btnsave" ></asp:Button>
                                                            </td>
                                                            <%-- <td style="width: 165px" align="left">
                                                            </td>
                                                            <td align="left">
                                                            </td>--%>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                    </cc1:TabContainer>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
    <%-- </ContentTemplate> 
 </asp:UpdatePanel>--%>

    <script type="text/javascript">

        var bmi = document.getElementById('<%= hfbmi.ClientID %>');
        var Age = document.getElementById('<%= hfAge.ClientID  %>');
        var Initails = document.getElementById('<%= hfinitials.ClientID  %>');
        var txtbmi = document.getElementById('<%= txtbmi.ClientID  %>');
        var txtAge = document.getElementById('<%= txtAge.ClientID  %>');
        var txtInitials = document.getElementById('<%= txtInitials.ClientID  %>');



        if (bmi != null || bmi != 'undefined') {
            txtbmi.value = bmi.value;
        }
        if (Age != null || Age != 'undefined') {
            txtAge.value = Age.value;
        }
        if (Initails != null || Initails != 'undefined') {
            txtInitials.value = Initails.value;

        }

  
    </script>

    <script type="text/javascript">

        function check_Other() {

            var chkOther = document.getElementById('<%= cblProofOfAge.ClientId %>');

            if (chkOther) {
                var chkOtherItemList = chkOther.getElementsByTagName('input');
                if (chkOtherItemList) {
                    if (chkOtherItemList.length > 0) {
                        for (var i = 0; i < chkOtherItemList.length; i++) {
                            var chkOtherItem = chkOtherItemList[i];
                            if (chkOtherItem)
                                if (chkOtherItem.type == 'checkbox')
                                if (i == 6)
                                if (chkOtherItem.checked) {
                                document.getElementById('<%=txtotherProf.ClientId %>').disabled = false;
                            }
                            if (!chkOtherItem.checked) {
                                document.getElementById('<%=txtotherProf.ClientId %>').value = '';
                                document.getElementById('<%=txtotherProf.ClientId %>').disabled = true;
                            }
                        }
                    }

                }
            }
        }

        function ValidateTab() {
            if (typeof (Page_ClientValidate) == 'function') {
                Page_ClientValidate();
            }
            return Page_IsValid;
        }

        function ClientPopulated(sender, e) {
            SubjectClientShowing('AutoCompleteExtender1', $get('<%= txtSubject.ClientId %>'));
        }
        function ClientPopulated1(sender, e) {
            SubjectClientShowing_OnlyID('AutoCompleteExtender1', $get('<%= txtSubject.ClientId %>'));
        }
        function OnSelected(sender, e) {
            debugger;
            SubjectOnItemSelected(e.get_value(), $get('<%= txtSubject.clientid %>'),
                $get('<%= HSubjectId.clientid %>'), document.getElementById('<%= btnEdit.ClientId %>'));
            if (window.location.search.substr(1).split('&')[0] == "mode=4") {
                window.location.href = "frmSubjectPIFMst.aspx?mode=4&SearchSubjectId=" + $get('<%= HSubjectId.clientid %>').value + "&SearchSubjectText=" + $get('<%= txtSubject.clientid %>').value;
            }
            else if (window.location.search.substr(1).split('&')[0] == "mode=11") {
                window.location.href = "frmSubjectPIFMst.aspx?mode=11&SearchSubjectId=" + $get('<%= HSubjectId.clientid %>').value + "&SearchSubjectText=" + $get('<%= txtSubject.clientid %>').value;
            }
            else {
                window.location.href = "frmSubjectPIFMst.aspx?SearchSubjectId=" + $get('<%= HSubjectId.clientid %>').value + "&SearchSubjectText=" + $get('<%= txtSubject.clientid %>').value;
            }
        }


        function CheckHeight(txtHeight) {
            var result = CheckDecimal(txtHeight.value);

            if (result == false) {
                msgalert('Please Enter Valid Height in Centimeters !');
                txtHeight.value = '';
                txtHeight.focus();
            }
        }

        function FillBMIValue(txtHeightID, txtWeightID, txtBMIID) {

            var txtHeight = document.getElementById(txtHeightID);
            var txtWeight = document.getElementById(txtWeightID);
            var txtBMI = document.getElementById(txtBMIID);
            var hfBMI = document.getElementById('<%= hfbmi.ClientID %>');

            //Again validate Height TextBox
            var result = CheckDecimal(txtHeight.value);
            if (result == false) {
                txtHeight.focus();
                return;
            }

            //Now Check Weight TextBox
            result = CheckDecimal(txtWeight.value);
            if (result == false) {
                msgalert('Please Enter Valid Weight In Kilogram !');
                txtWeight.value = '';
                txtWeight.focus();
                return;
            }



            var bmi = GetBMI(txtHeight.value, txtWeight.value);
            try {
                if ((bmi != null) && !isNaN(bmi)) {
                    bmi = parseFloat(bmi);
                    txtBMI.value = bmi;
                    hfBMI.value = bmi;
                    txtBMI.disabled = true;
                    document.getElementById('<%=txtdob.ClientID %>').focus();

                }
                else {
                    lblBMI.value = '0';
                    txtBMI.value = '0';
                }
            }
            catch (err) {
                msgalert(err.description);
            }
            txtBMI.disabled = true;
        }


        function SetAge(txt) {
            var txtDob = document.getElementById('<%=txtdob.ClientID %>');
            var hf = document.getElementById('<%=hfAge.ClientID %>');
            var dt = document.getElementById('<%=txtdoer.ClientID %>').value;

            if (txtDob.value.length > 8) {

                dob = GetDateDifference(txtDob.value, dt);
                srch = '';
                if ((dob.Days / 365) == 1) {
                    txt.value = (dob.Years + 1).toString();
                    hf.value = (dob.Years + 1).toString();
                    txt.disabled = true;
                }

                else {
                    txt.value = dob.Years.toString();
                    hf.value = dob.Years.toString();
                    txt.disabled = true;
                }
                if (txt.value < 17) {
                    msgalert('Age Is Less Than 17 yrs !');
                }
            }

        }

        function SetInitial(txt) {

            var txtlastname = document.getElementById('<%=txtlastname.ClientID %>');
            var txtmiddlename = document.getElementById('<%=txtmiddlename.ClientID %>');
            var txtfirstname = document.getElementById('<%=txtfirstname.ClientID %>');
            var txtInitial = document.getElementById(txt);
            txtInitial.disabled = false;
            var hfIni = document.getElementById('<%=hfinitials.ClientID %>');
            var strlastname = new String();
            strlastname = txtlastname.value;
            var strfirstname = new String();
            strfirstname = txtfirstname.value;
            var strmiddlename = new String();
            strmiddlename = txtmiddlename.value;
            //var initial= substring(txtlastname.value,0,1) + substring(txtfirstname.value,0,1) + substring(txtmiddlename.value,0,1);
            txtInitial.value = strlastname.substring(0, 1) + strfirstname.substring(0, 1) + strmiddlename.substring(0, 1);
            hfIni.value = strlastname.substring(0, 1) + strfirstname.substring(0, 1) + strmiddlename.substring(0, 1);
            document.getElementById('<%=txtInitials.ClientID %>').style.disable = true;
            txtInitial.disabled = true;
        }

        function DateConvert_Age(ParamDate, txtdate, CheckLess) {

            document.getElementById('<%=txtAge.ClientID %>').style.disable = false;
            if (txtdate.value != "") {
                if (!DateConvert(ParamDate, txtdate)) {
                    return false;
                }

                if (CheckLess = 'Y' && !CheckDateLessThenToday(txtdate.value)) {
                    txtdate.value = "";
                    txtdate.focus();
                    msgalert('Date should be less than Current Date !');
                    return false;
                }

                SetAge(document.getElementById('<%=txtAge.ClientID %>'));
                document.getElementById('<%=txtAge.ClientID %>').style.disable = true;

                var initial = document.getElementById('<%=txtInitials.ClientID %>');
                var dob = document.getElementById('<%=txtdob.ClientID %>');
                var btn = document.getElementById('<%=btnCheckDuplicateSubject.ClientID %>');
                if (initial.value != "" && dob.value != "") {
                    btn.click();
                }
            }
            return true;
        }
        function CheckDuplicateSubject() {
            var initial = document.getElementById('<%=txtInitials.ClientID %>');
            var txtdate = document.getElementById('<%=txtdob.ClientID %>');
            var btn = document.getElementById('<%=btnCheckDuplicateSubject.ClientID %>');
            if (initial.value != "" && txtdate.value != "") {
                btn.click();
            }
            return true;
        }

        //For Validation
        function Validation(type) {

            if (document.getElementById('<%= txtlastname.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Last Name !');
                return false;
            }
            else if (document.getElementById('<%= txtfirstname.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter First Name !');
                return false;
            }
            else if (document.getElementById('<%= txtheight.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Height !');
                return false;
            }
            else if (document.getElementById('<%= txtweight.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Weight !');
                return false;
            }
            else if (document.getElementById('<%= txtdoer.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Enrollment Date !');
                return false;
            }
            //Added for compulsory add remark while Edit else not compulsory on 24-Aug-2009
            if (type == 'EDIT') {
                if (document.getElementById('<%= txtRemark.ClientId %>').value.toString().trim().length <= 0) {
                    msgalert('Please Enter Remarks !');
                    return false;
                }
            }

            if (type == 'ADD') {
                document.getElementById('<%= btnsave.ClientId %>').style.display = 'none';
                //document.getElementById('<%= btnsave.ClientId %>').disabled = true;
            }

            return true;
        }


        function QCDivShowHide(Type) {
            if (document.getElementById('<%= HSubjectId.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Subject !');
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


        function ValidateRemarks(txt, cntField, maxSize) {
            cntField = document.getElementById(cntField);
            if (txt.value.length > maxSize) {
                txt.value = txt.value.substring(0, maxSize);
            }
            // otherwise, update 'characters left' counter
            else {
                cntField.innerHTML = maxSize - txt.value.length;
            }
        }

        function ValidationQC() {
            if (document.getElementById('<%= txtQCRemarks.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Remarks/Response !');
                document.getElementById('<%= txtQCRemarks.ClientId %>').focus();
                return false;
            }
            return true;
        }

        function ValidationMSR() {
            if (document.getElementById('<%= HSubjectId.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Subject !');
                document.getElementById('<%= txtSubject.ClientId %>').focus();
                document.getElementById('<%= txtSubject.ClientId %>').value = '';
                return false;
            }
            return true;
        }


        function validate(DD, TC, TP) {
            var ddlValue = DD.options[DD.selectedIndex].value;

            TC.disabled = false;
            TP.disabled = false;

            if (ddlValue == 'N') {
                TC.disabled = true;
                TP.disabled = true;
            }
            else if (ddlValue == 'C') {
                TC.disabled = false;
                TP.disabled = true;
            }
        }

        function ddlRemarkSelected() {
            var txt = document.getElementById('<%= txtRemark.ClientId %>');
            var ddlremark = document.getElementById('<%= ddlRemark.ClientId %>');

            txt.value = ddlremark.options[ddlremark.selectedIndex].text;
            return true;
        }

        //function save()
        //{
        //    alert(document.getElementById('<%= btnsave.ClientId %>').style.disable);
        //    document.getElementById('<%= btnsave.ClientId %>').style.disable = true;
        //     return true;
        //}

        function ShowAlert(msg) {
            msgalert(msg);
            window.location.href = "frmSubjectPIFMst.aspx";
        }
        
                 
    </script>

</asp:Content>
