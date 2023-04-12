<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmDrugScanReport, App_Web_2mzu20n4" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    
    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    
    <script type="text/javascript">
        function ClientPopulated(sender, e)
        {
            ProjectClientShowing('AutoCompleteExtender1',$get('<%= txtProject.ClientId %>'));
        }
            
        function OnSelected(sender,e)
        {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
                $get('<%= HProjectId.clientid %>'),document.getElementById('<%= btnSetProject.ClientId %>'));
        }
    </script>
    
    
    <table width="90%" cellpadding="0px">
        <tr>
            <td nowrap align="left">
                <strong>DRUG SCAN RESULT FORM</strong>
                <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
            </td>
        </tr>
        
        <tr>
            <td>
                <table cellpadding="5">
                    <tbody>
                        <tr>
                            <td style="white-space: nowrap" class="Label" valign="top" align="left">
                                Project* :</td>
                            <td style="white-space: nowrap" valign="top" align="left">
                                <asp:TextBox ID="txtProject" runat="server" Width="271px"></asp:TextBox>
                                <asp:Button Style="display: none" ID="btnSetProject" runat="server" Text=" Click">
                                </asp:Button><br />
                                <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" OnClientShowing="ClientPopulated"
                                        OnClientItemSelected="OnSelected" UseContextKey="True" TargetControlID="txtProject"
                                        ServiceMethod="GetProjectCompletionListWithOutSponser"  ServicePath="AutoComplete.asmx"
                                        BehaviorID="AutoCompleteExtender1" MinimumPrefixLength="1">
                                    </cc1:AutoCompleteExtender>
                            </td>
                            <td style="white-space: nowrap" class="Label" valign="top" align="left">
                                Period* :</td>
                            <td valign="top" align="left" width="100%">
                                <asp:DropDownList ID="ddlPeriod" runat="server" Width="198px" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td class="Label" align="left">
                                Date :</td>
                            <td valign="top" nowrap align="left">
                                <asp:TextBox ID="txtDrugScanDate" runat="server" CssClass="textbox" Width="163px"
                                    Enabled="False"></asp:TextBox><img style="display: none" id="imgDate" onclick="popUpCalendar(this,ctl00_CPHLAMBDA_txtdrugscanrpt,'dd-mmm-yyyy');"
                                        alt="Select date of Enrollment Reporting" src="images/calendar.gif" align="absMiddle"
                                        runat="server" /></td>
                        </tr>
                        <tr>
                        </tr>
                        <tr>
                            <td style="white-space: nowrap" class="Label" valign="top" align="left">
                                Tested For :</td>
                            <td valign="top" align="left">
                                <asp:TextBox ID="txtTestedFor" runat="server" CssClass="textbox" Width="213px" Height="49px"
                                    TextMode="MultiLine">AMP,BAR,BZD,COC,MOR,THC</asp:TextBox></td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <asp:GridView ID="gvwSubjectDrugScan" runat="server" SkinID="grdViewAutoSizeMax" AutoGenerateColumns="False"
        OnRowCreated="gvclient_RowCreated" AllowPaging="True" OnPageIndexChanging="gvclient_PageIndexChanging"
        OnRowDataBound="gvwSubjectDrugScan_RowDataBound" OnRowEditing="gvwSubjectDrugScan_RowEditing"
        OnRowCancelingEdit="gvwSubjectDrugScan_RowCancelingEdit" OnRowUpdating="gvwSubjectDrugScan_RowUpdating">
        <Columns>
            <asp:BoundField DataFormatString="number" HeaderText="#" ReadOnly="True">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </asp:BoundField>
            <asp:TemplateField HeaderText="Subject Id">
                <EditItemTemplate>
                    <asp:Label ID="lblSubjectId" runat="server" Text='<%# eval("vSubjectId") %>'></asp:Label>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblSubjectId" runat="server" Text='<%# eval("vSubjectId") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="FullName" HeaderText="Name" ReadOnly="True">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                <ItemStyle HorizontalAlign="Left"></ItemStyle>
            </asp:BoundField>
            <asp:TemplateField HeaderText="Time">
                <EditItemTemplate>
                    <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtStartTime"
                        BehaviorID="MaskedEditExtender1" ClearTextOnInvalid="True" ErrorTooltipEnabled="True"
                        MaskType="Time" Mask="99:99" UserTimeFormat="TwentyFourHour">
                    </cc1:MaskedEditExtender>
                    <asp:TextBox ID="txtStartTime" runat="server" Text='<%# Bind("dStartDate", "{0:HH:mm}") %>'
                        CssClass="textBox" Width="49px"></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblStartTime" runat="server" Text='<%# Eval("dStartDate", "{0:dd-MMM-yyyy HH:mm}") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle VerticalAlign="Middle"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Positive, If any">
                <EditItemTemplate>
                    <asp:CheckBoxList ID="cblPositive" runat="server" CellPadding="1" RepeatColumns="3">
                    </asp:CheckBoxList>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblPositive" runat="server" Text='<%# Eval("vPositiveTestCodes") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle VerticalAlign="Middle"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Repeat Details">
                <EditItemTemplate>
                    <asp:TextBox ID="txtRepeatDetails" runat="server" Text='<%# Bind("vRepeatAnalysisDetails") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblRepeatDetails" runat="server" Text='<%# Eval("vRepeatAnalysisDetails") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle VerticalAlign="Middle"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Final result">
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlFinalResult" runat="server" CssClass="dropDownList" Width="52px">
                        <asp:ListItem Value="Y">Yes</asp:ListItem>
                        <asp:ListItem Value="N">No</asp:ListItem>
                    </asp:DropDownList>&nbsp;
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblFinalResult" runat="server" Text='<%# Eval("cFinalResult") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle VerticalAlign="Middle"></ItemStyle>
            </asp:TemplateField>
            <asp:CommandField ShowEditButton="True"></asp:CommandField>
        </Columns>
    </asp:GridView>
    <table cellpadding="5" width="95%">
        <tbody>
            <tr>
                <td style="width: 100%" align="left" class="Label">
                    Remarks:
                    <br />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:TextBox ID="txtAllRemarks" runat="server" CssClass="textBox" Width="82%" Height="205px"
                        TextMode="MultiLine"></asp:TextBox>
                    <br />
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Button ID="btnRemarks" OnClick="btnRemarks_Click" runat="server" Text=" Save Remarks "
                        CssClass="btn btnnew"></asp:Button>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
