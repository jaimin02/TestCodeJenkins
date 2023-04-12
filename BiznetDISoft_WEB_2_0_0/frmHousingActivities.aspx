<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmHousingActivities.aspx.vb" Inherits="frmHousingActivities" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/Validation.js"></script>

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <table cellpadding="5px" width="100%">
        <tr>
            <td style="text-align: center;" colspan="2">
                <asp:RadioButtonList ID="rbtSelection" runat="server" RepeatDirection="Horizontal"
                    AutoPostBack="True" Style="margin: auto">
                    <asp:ListItem Selected="True" Value="00">Normal Design</asp:ListItem>
                    <asp:ListItem Value="01">MultiDose Design</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td style="text-align: right; width: 40%;" class="Label" nowrap="nowrap">
                Project Name/Project No./Request ID* :
            </td>
            <td style="text-align: left;" nowrap="nowrap">
                <span style="font-weight: normal">
                    <asp:TextBox ID="txtProject" runat="server" CssClass="textBox" Width="45%" />
                </span>
                <asp:Button ID="btnSetProject" runat="server" Style="display: none" Text=" Project" />
                <asp:HiddenField ID="HProjectId" runat="server" />
                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                    CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                    CompletionListElementID="pnlProjectlist" CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                    OnClientShowing="ClientPopulated" ServiceMethod="GetMyProjectCompletionList"
                    ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True">
                </cc1:AutoCompleteExtender>
                <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 300px; overflow: auto;
                            overflow-x: hidden" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td class="Label" style="text-align: left;">
                <div id="VersionDtl" runat="server" style="display: none;">
                    Version :<asp:Label runat="server" ID="VersionNo" Style="padding-right: 10px"></asp:Label>Version
                    Date :<asp:Label ID="VersionDate" Style="padding-right: 10px;" runat="server"></asp:Label>
                    Status:<img src="images/UnFreeze.jpg" id="ImageLockUnlock" runat="server" alt="Lock" />
                </div>
            </td>
        </tr>
        <tr>
            <td class="Label" nowrap="nowrap" style="text-align: right;">
                Parent Activity* :
            </td>
            <td style="text-align: left;" nowrap="nowrap">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlParentAct" TabIndex="1" runat="server" CssClass="dropDownList"
                            Width="45%" AutoPostBack="True" OnSelectedIndexChanged="ddlParentAct_SelectedIndexChanged">
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
                        <%--<asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />--%>
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="BtnAdd" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnExit" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td class="Label" nowrap="nowrap" style="text-align: right;">
                Reference Activity* :
            </td>
            <td style="text-align: left;" class="Label" nowrap="nowrap">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlRefAct" TabIndex="3" runat="server" CssClass="dropDownList"
                            Width="45%">
                        </asp:DropDownList>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" ToolTip="Search" CssClass="btn btnnew" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
                        <%--<asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />--%>
                        <asp:AsyncPostBackTrigger ControlID="BtnAdd" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnExit" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr runat="server" id="tractivitygroup">
            <td class="Label" nowrap="nowrap" style="text-align: right;">
                Activity Group* :
            </td>
            <td style="text-align: left;" nowrap="nowrap">
                <asp:DropDownList ID="ddlActivityGroup2" runat="server" AutoPostBack="True" CssClass="dropDownList"
                    TabIndex="2" Width="35%">
                </asp:DropDownList>
            </td>
        </tr>
        <tr runat="server" id="tractivity">
            <td class="Label" nowrap="nowrap" style="text-align: right;">
                Activity* :
            </td>
            <td style="text-align: left;" nowrap="nowrap">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlAct" TabIndex="3" runat="server" CssClass="dropDownList"
                            Width="35%">
                        </asp:DropDownList>
                        <span class="Label">Referance Time Interval:</span>
                        <asp:TextBox onblur="return ValidTimeInterval(this);" ID="txtRefTimeInterval" TabIndex="4"
                            runat="server" CssClass="textBox" Width="5%">1</asp:TextBox><span class="Label "> Hours</span>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlActivityGroup2" EventName="SelectedIndexChanged">
                        </asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr runat="server" id="trrepetitions">
            <td class="Label" nowrap="nowrap" style="text-align: right;">
                No. of Repetitions* :
            </td>
            <td style="text-align: left;" nowrap="nowrap">
                <asp:TextBox ID="txtRepeatation" runat="server" CssClass="textBox" TabIndex="4" Width="5%">1</asp:TextBox>
                <span class="Label">Deviation Time:</span>
                <asp:TextBox onblur="return ValidTimeInterval(this);" ID="txtDevTime" runat="server"
                    CssClass="textBox" Width="5%">1</asp:TextBox>
                <span class="Label">Minutes</span>
            </td>
        </tr>
        <tr>
            <%-- <td align="left" class="Label" nowrap="nowrap">
            </td>--%>
            <td colspan="2" style="text-align: center;" nowrap="nowrap">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:HiddenField ID="HfPeriod" runat="server" />
                        <asp:HiddenField ID="HfMaxNodeId" runat="server" />
                        <asp:HiddenField ID="HfMaxNodeIndex" runat="server" />
                        <asp:Button ID="BtnAdd" OnClientClick="return Validation('ADD');" TabIndex="5" runat="server"
                            Text="ADD" ToolTip="Add" CssClass="btn btnnew" />
                    </ContentTemplate>
                    <Triggers>
                        <%--<asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />--%>
                        <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td nowrap="nowrap" colspan="2">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div style="left: 110px; width: 296px; top: 220px; height: 108px" id="divSearch"
                            class="DIVSTYLE2" runat="server" visible="false">
                            <asp:Panel ID="pnlSearch" runat="server" Width="100%" Visible="false" Style="text-align: left;
                                margin-bottom: 10%">
                                <asp:CheckBox ID="chkAllSearch" TabIndex="6" runat="server" Text="All Periods" CssClass="Label"
                                    AutoPostBack="True" CausesValidation="True" ValidationGroup="a" OnCheckedChanged="chkAllSearch_CheckedChanged" />
                                <br />
                                <asp:CheckBox ID="ChkPeriodSearch" TabIndex="7" runat="server" Text="Period" CssClass="Label"
                                    AutoPostBack="True" CausesValidation="True" ValidationGroup="a" OnCheckedChanged="ChkPeriodSearch_CheckedChanged"
                                    Checked="True"></asp:CheckBox>
                                <asp:DropDownList ID="ddlPeriodSearch" TabIndex="8" runat="server" CssClass="dropDownList"
                                    Width="78px" />
                                <br />
                            </asp:Panel>
                            <asp:Button ID="BtnSearchDiv" OnClick="BtnSearchDiv_Click" runat="server" Text="Search"
                                ToolTip="Search" CssClass="btn btnnew" />
                            <asp:Button ID="btnExit" OnClick="btnExit_Click" runat="server" Text="Cancel" ToolTip="Cancel"
                                CssClass="btn btnexit" /></div>
                        <br />
                        <table width="100%">
                            <tbody>
                                <tr>
                                    <td style="height: 50%">
                                        <asp:ImageButton ID="ImgBtnUp" runat="server" Width="30px" ImageUrl="~/images/Up.jpg" />
                                    </td>
                                    <td rowspan="2">
                                        <asp:GridView ID="GV_Housing" runat="server" SkinID="grdView" OnRowUpdating="GV_Housing_RowUpdating"
                                            OnRowCommand="GV_Housing_RowCommand" AutoGenerateColumns="False" OnRowDataBound="GV_Housing_RowDataBound"
                                            OnRowDeleting="GV_Housing_RowDeleting" OnRowCreated="GV_Housing_RowCreated">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChkMove" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:BoundField HeaderText="Sr. No." />--%>
                                                <asp:BoundField HeaderText="Sr. No.">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Description">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDesc" runat="server" Text='<% # Eval("vNodeDisplayName") %>'
                                                            CssClass="textBox" Width="285px" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="iNodeIndex" HeaderText="NodeIndex" />
                                                <asp:BoundField DataField="iNodeId" HeaderText="NodeId" />
                                                <asp:BoundField DataField="cIsPreDose" HeaderText="Is PreDose" />
                                                <asp:TemplateField HeaderText="Ref. Time">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRefTime" runat="server" Text='<% # Eval("nRefTime") %>' CssClass="textBox"
                                                            Width="58px" onblur="return ValidTimeInterval(this);"></asp:TextBox>Hours
                                                    </ItemTemplate>
                                                    <ItemStyle VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Dev. Time">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDevTime" runat="server" Text='<% # Eval("nDeviationTime") %>'
                                                            CssClass="textBox" Width="58px" onblur="return ValidTimeInterval(this);"></asp:TextBox>min
                                                        <asp:LinkButton ID="lnkUpdateTime" runat="server" Text="Update" ToolTip="Update"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgbtnDelete" ToolTip="Delete" runat="server" ImageUrl="~/Images/i_delete.gif" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Is Predose">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkIsPredose" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:HiddenField ID="HFGV_NodeId" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 50%">
                                        <asp:ImageButton ID="ImgBtnDown" runat="server" Width="30px" ToolTip="Down" ImageUrl="~/images/Down.jpg"
                                            Height="50%" />
                                    </td>
                                </tr>
                                <tr>
                                    <caption>
                                        <td colspan="2" rowspan="1" style="text-align: center;">
                                            <asp:Button ID="btnSchedule" runat="server" CssClass="btn btnnew" OnClick="btnSchedule_Click"
                                                Text="Schedule" ToolTip="Schedule" Visible="False" />
                                        </td>
                                    </caption>
                                </tr>
                            </tbody>
                        </table>
                        <br />
                        <asp:CheckBox ID="chkAll" TabIndex="6" runat="server" Text="Attach to all Periods"
                            CssClass="Label" AutoPostBack="True" CausesValidation="True" ValidationGroup="a"
                            OnCheckedChanged="chkAll_CheckedChanged" />
                        <asp:CheckBox ID="ChkPeriod" TabIndex="7" runat="server" Text="Period" CssClass="Label"
                            AutoPostBack="True" CausesValidation="True" ValidationGroup="a" OnCheckedChanged="ChkPeriod_CheckedChanged"
                            Checked="True" />
                        <asp:DropDownList ID="ddlPeriod" TabIndex="8" runat="server" CssClass="dropDownList"
                            Width="78px" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="BtnAdd" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ImgBtnUp" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ImgBtnDown" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <%--<asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />--%>
                        <asp:AsyncPostBackTrigger ControlID="ddlParentAct" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td style="text-align: center;" colspan="2">
                <asp:Button ID="BtnSave" OnClientClick="return Validation('SAVE');" runat="server"
                    CssClass="btn btnsave" Text="Save" ToolTip="Save" TabIndex="9" />
                <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" Text="Cancel" ToolTip="Cancel"
                    TabIndex="10" />
                <asp:Button ID="btnExit1" runat="server" CssClass="btn btnexit" Text="Exit" ToolTip="Exit"
                    OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
            </td>
        </tr>
    </table>

    <script type="text/javascript">
        function Validation(type) {
            if (document.getElementById('<%= HProjectId.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Project !');
                document.getElementById('<%= txtProject.ClientId %>').focus();
                document.getElementById('<%= txtProject.ClientId %>').value = '';
                return false;
            }

            else if (document.getElementById('<%= ddlParentAct.ClientId %>').selectedIndex == 0) {
                msgalert('Please select Parent Activity !');
                document.getElementById('<%= ddlParentAct.ClientId %>').focus();
                return false;
            }

            else if (document.getElementById('<%= ddlRefAct.ClientId %>').selectedIndex == 0) {
                msgalert('Please select referance Activity !');
                document.getElementById('<%= ddlRefAct.ClientId %>').focus();
                return false;
            }

            else if (type.toUpperCase() == 'ADD' && document.getElementById('<%= ddlActivityGroup2.ClientId %>').selectedIndex == 0) {
                msgalert('Please select Activity Group !');
                document.getElementById('<%= ddlActivityGroup2.ClientId %>').focus();
                return false;
            }

            else if (type.toUpperCase() == 'ADD' && document.getElementById('<%= ddlAct.ClientId %>').selectedIndex == 0) {
                msgalert('Please select Activity !');
                document.getElementById('<%= ddlAct.ClientId %>').focus();
                return false;
            }
            else if (type.toUpperCase() == 'SAVE') {

                var housingGrid = document.getElementById('<%= GV_Housing.ClientId %>');

                if (housingGrid == null || typeof (housingGrid) == 'undefined') {
                    msgalert('Please Add Housing Activity !');
                    document.getElementById('<%= BtnAdd.ClientId %>').focus();
                    return false;
                }
                else {

                    document.getElementById('<%= btnsave.ClientId %>').style.display = 'none';

                }

            }

            return true;

        }

        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        function IsNumericAndNotZero(txt) {
            var result = CheckInteger(txt.value);

            if (result) {
                if (txt.value == 0) {
                    msgalert("Please Enter Repetations More Than Zero !");
                    txt.focus();
                    txt.value = "1";
                    return false;
                }
            }
            else {
                msgalert("Please Enter Repetations More Than Zero !");
                txt.focus();
                txt.value = "1";
                return false;
            }

            return true;
        }

        function ValidTimeInterval(txt) {
            var ttime;
            var tottime
            var arrtime;
            var hh;
            var mm;

            ttime = txt.value;
            if (!(CheckDecimalOrBlank(txt.value))) {
                msgalert("Please Enter Valid Interval !");
                txt.focus();
                txt.value = "1.00";
                return true;
            }

            if (ttime.indexOf('.') > -1) {
                arrtime = ttime.split('.');

                if (arrtime.length > 2) {
                    msgalert("Please Enter Valid Interval !");
                    txt.focus();
                    txt.value = "1.00";
                    return true;
                }

                if (arrtime[1].length < 2) {
                    arrtime[1] = arrtime[1] + '0';
                }

                tottime = arrtime[0] * 60;

                if (arrtime[1] > 59) {
                    tottime = tottime + parseInt(arrtime[1].substring(0, 2));
                    hh = Math.floor(tottime / 60);
                    mm = Math.floor(tottime % 60);

                    if (mm.toString().length < 2) {
                        mm = '0' + mm;
                    }

                }
                else {
                    hh = arrtime[0];
                    mm = arrtime[1];
                }

                txt.value = hh + '.' + mm;
                return true;
            }
            else {
                txt.value = txt.value + '.' + '00'
            }
            return true;

        }

        //for hide fields Added By Mrunal Parekh on 12-Jan-2012
        //        function Hidefields()
        //        {
        //            document.getElementById('ctl00_CPHLAMBDA_tractivitygroup').style.display='none';
        //            document.getElementById('ctl00_CPHLAMBDA_tractivity').style.display='none';
        //            document.getElementById('ctl00_CPHLAMBDA_trrepetitions').style.display='none';
        //            document.getElementById('ctl00_CPHLAMBDA_BtnSave').style.display='none';
        //                                  
        //        }

        function Showfields(type) {

            document.getElementById('ctl00_CPHLAMBDA_tractivitygroup').style.display = '';
            document.getElementById('ctl00_CPHLAMBDA_tractivity').style.display = '';
            document.getElementById('ctl00_CPHLAMBDA_trrepetitions').style.display = '';
            document.getElementById('ctl00_CPHLAMBDA_BtnSave').style.display = '';
            document.getElementById('<%=BtnAdd.ClientId %>').style.display = ''; 
            document.getElementById('<%=chkAll.ClientId %>').style.display = ''; 
            document.getElementById('<%=ChkPeriod.ClientId %>').style.display = '';  

            if (type == 'H') {

                document.getElementById('ctl00_CPHLAMBDA_tractivitygroup').style.display = 'none';
                document.getElementById('ctl00_CPHLAMBDA_tractivity').style.display = 'none';
                document.getElementById('ctl00_CPHLAMBDA_trrepetitions').style.display = 'none';
                document.getElementById('ctl00_CPHLAMBDA_BtnSave').style.display = 'none';
                document.getElementById('<%=BtnAdd.ClientId %>').style.display = 'none'; 
                document.getElementById('<%=chkAll.ClientId %>').style.display = 'none'; 
                document.getElementById('<%=ChkPeriod.ClientId %>').style.display = 'none';   
                
            }


        }

        //        function ShowAlert()
        //        {
        //        
        //            Alert_box = alert("HOUSING ACTIVITY SAVED SUCCESFULLY."); 
        //            window.document.location.href = "frmHousingActivities.aspx?mode=1&Saved=Y"
        //             
        //        }
        
    </script>

</asp:Content>
