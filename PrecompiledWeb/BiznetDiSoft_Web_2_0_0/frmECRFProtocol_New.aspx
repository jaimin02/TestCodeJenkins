<%@ page title="" language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmECRFProtocol_New, App_Web_w1bzwbih" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <script src="Script/jquery-ui.js" type="text/javascript"></script>
    <style type="text/css">
        .textBox {
            /*width: 100% !important;*/

        }
        .ui-multiselect {
            max-height: 35px;
            overflow: auto;
            overflow-x: hidden;
            white-space: nowrap;
        }

        .ui-multiselect-menu {
            width: 430px !important;
            font-size: 0.8em !important;
        }

            .ui-multiselect-menu span {
                vertical-align: top;
            }

        .ui-menu .ui-menu-item a {
            font-size: 11px !important;
            text-align: left !important;
        }

        .ui-multiselect-checkboxes li ul li {
            list-style: none !important;
            clear: both;
            font-size: 1.0em;
            padding-right: 3px;
        }

        .ui-multiselect-checkboxes ui-helper-reset {
            height: 200px;
            width: 500px;
            overflow: auto;
        }

        .ui-multiselect {
            border: 1px solid navy;
            max-width: 100% !important;
            max-height: 35px;
            overflow: auto;
            overflow-x: hidden;
            white-space: nowrap;
            width:auto !important;
        }
    </style>

    <div id="divMain" style="width: 100%;">

        <div id="divLeft" runat="server" style="width: 30%; float: left; padding-top: 3%; height: 560px;">

            <table style="width: 100%;">

                <tr>

                    <td>

                        <asp:UpdatePanel ID="upPeriod" runat="server" RenderMode="Inline" UpdateMode="Conditional">

                            <ContentTemplate>

                                <table style="width: 100%; border: 1px solid darkgray; border-radius: 5px;border-collapse:initial;" align="left" border="0" cellspacing="10px">

                                    <tr>
                                        <td style="text-align: center;" colspan="2">
                                            <asp:RadioButtonList ID="rbtSelection" runat="server" RepeatDirection="Horizontal"
                                                Style="margin: auto" AutoPostBack="true">
                                                <asp:ListItem Selected="True" Value="ParentActivity">Parent Activity</asp:ListItem>
                                                <asp:ListItem Value="ChildActivity">Child Activity</asp:ListItem>
                                                 <asp:ListItem Value="DefineWorkflow">CRF Review Flow</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>

                                    <tr id="trmultidose" runat="server">

                                        <td class="Label" style="white-space: nowrap;" colspan="2">

                                            <asp:UpdatePanel ID="upMultidose" runat="server" RenderMode="Inline" UpdateMode="Conditional">

                                                <ContentTemplate>
                                                    <asp:CheckBox ID="chkMultiDose" runat="server" Text="Multidose Design" CssClass="Label" AutoPostBack="true" />
                                                </ContentTemplate>

                                                <Triggers>

                                                    <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />

                                                </Triggers>

                                            </asp:UpdatePanel>

                                        </td>

                                    </tr>

                                    <tr>
                                        <td class="Label" style="white-space: nowrap; text-align: right; width: 10%">Project No./</br>Request ID* :
                                        </td>

                                        <td style="text-align: left; width: 90%;">
                                            <span style="font-weight: normal">
                                                <asp:TextBox ID="txtProject" runat="server" CssClass="textBox"  onkeydown="return (event.keyCode!=13)" Width="90%"/>
                                            </span>
                                            <asp:Button ID="btnSetProject" runat="server" Style="display: none" Text=" Project" />
                                            <asp:Button ID="btnreviewer" runat="server" Style="display: none" Text=" reviewer" />
                                            <asp:HiddenField ID="HProjectId" runat="server" />
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                                CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                CompletionListElementID="pnlProjectlist" CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                                OnClientShowing="ClientPopulated" ServiceMethod="GetMyProjectCompletionListParentOnly"
                                                ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True">
                                            </cc1:AutoCompleteExtender>
                                            <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" />

                                        </td>

                                    </tr>

                                    <tr id="trPeriod" runat="server">

                                        <td class="Label" style="white-space: nowrap; text-align: left; width: 10%">Periods :
                                        </td>

                                        <td style="text-align: left; width: 90%">

                                            <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="dropDownList" Width="20%" AutoPostBack="True" />

                                        </td>

                                    </tr>

                                    <tr>
                                        <td class="Label" colspan="2">
                                            <div id="VersionDtl" runat="server" style="display: none;">
                                                Version :<asp:Label runat="server" ID="VersionNo"></asp:Label>
                                                Version Date :<asp:Label ID="VersionDate" runat="server"></asp:Label>
                                                Status:<img src="images/UnFreeze.jpg" id="ImageLockUnlock" runat="server" alt="Lock" />
                                            </div>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td class="Label" colspan="2">
                                            <div id="Divlnk" runat="server" style="display: none;">
                                                <a id="Proc_struct" runat="server" href="" onclick="open_ProjStruct();" style="text-decoration: underline;">Goto Project Structure Management</a>
                                            </div>
                                        </td>
                                    </tr>
                                    
                                </table>

                            </ContentTemplate>

                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="chkMultiDose" EventName="CheckedChanged" />
                                <asp:AsyncPostBackTrigger ControlID="rbtSelection" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlPeriod" EventName="SelectedIndexChanged" />
                            </Triggers>

                        </asp:UpdatePanel>

                    </td>

                </tr>

                <tr>

                    <td>

                        <asp:UpdatePanel ID="UpParentActivity" runat="server" RenderMode="Inline" UpdateMode="Conditional">

                            <ContentTemplate>

                                <div id="divParentActivity" runat="server" style="width: 100%; display: none; vertical-align: top; float: none;" align="center">

                                    <table style="width: 100%; border: 1px solid darkgray; border-radius: 5px;border-collapse:initial;" align="left" border="0" cellspacing="12px">

                                        <tr>
                                            <td class="Label" style="white-space: nowrap; text-align: left; width: 14%">Parent Activity* :
                                            </td>
                                            <td style="text-align: right; width: 86%">

                                                <asp:DropDownList ID="ddlParentAct" runat="server" CssClass="dropDownList" Width="100%"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlParentAct_SelectedIndexChanged">
                                                </asp:DropDownList>

                                            </td>
                                        </tr>

                                        <tr id="trReferenceActivity" runat="server">
                                            <td class="Label" style="white-space: nowrap; text-align: left; width: 14%">Reference Activity :
                                            </td>
                                            <td style="text-align: right; width: 86%">
                                                <asp:DropDownList ID="ddlRefAct" runat="server" CssClass="dropDownList"
                                                    Width="100%" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td colspan="2" style="text-align: center">
                                                <asp:Button ID="btnSearch" runat="server" Text="Search" ToolTip="Search" CssClass="btn btnnew" OnClick="BtnSearch_Click" OnClientClick="return Validation('SEARCH');" />
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="btn btncancel" OnClientClick=" clearControls();" Visible="false" />
                                            </td>
                                        </tr>

                                    </table>

                                </div>

                            </ContentTemplate>

                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnRearrange" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="rbtSelection" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="chkMultiDose" EventName="CheckedChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlPeriod" EventName="SelectedIndexChanged" />
                            </Triggers>

                        </asp:UpdatePanel>

                    </td>

                </tr>

                <tr>

                    <td>

                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">

                            <ContentTemplate>

                                <div id="divActivityGroup" runat="server" style="width: 100%; vertical-align: top; float: none; display: block" align="center">

                                    <table style="width: 100%; border: 1px solid darkgray; border-radius: 5px;border-collapse:initial" align="left" border="0" cellspacing="12px">

                                        <tr runat="server" id="tractivitygroup">
                                            <td class="Label" style="white-space: nowrap; text-align: left; width: 14%">Activity Group* :
                                            </td>
                                            <td style="text-align: right; width: 86%">

                                                <asp:DropDownList ID="ddlActivityGroup" runat="server" AutoPostBack="True" CssClass="dropDownList"
                                                    Width="100%" OnSelectedIndexChanged="ddlActivityGroup_SelectedIndexChanged">
                                                </asp:DropDownList>

                                            </td>
                                        </tr>

                                        <tr runat="server" id="tractivity">
                                            <td class="Label" style="white-space: nowrap; text-align: left; width: 14%">Activity* :
                                            </td>

                                            <td style="text-align: left; width: 86%">

                                                <asp:UpdatePanel ID="UpActivity" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                                    <ContentTemplate>

                                                        <asp:DropDownList ID="ddlActivity" runat="server" CssClass="dropDownList"
                                                            Width="100%">
                                                        </asp:DropDownList>

                                                    </ContentTemplate>

                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlActivityGroup" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>

                                            </td>
                                        </tr>

                                        <tr>

                                            <td colspan="2">

                                                <div id="divActivity" style="width: 100%; vertical-align: top; float: right; display: block" align="center">

                                                    <table width="100%" cellpadding="5px">

                                                        <tr runat="server" id="trrepetitions">
                                                            <td class="Label" style="white-space: nowrap; text-align: left; width: 30%">No. of Repetitions* :
                                                            </td>
                                                            <td class="Label" style="white-space: nowrap; text-align: left; width: 70% !important">
                                                                <asp:TextBox ID="txtRepeatation" runat="server" CssClass="textBox" onkeypress="return isNumber(event)"
                                                                    onblur="return IsNotZero();" Style="width: 20% !important">1</asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr runat="server" id="trreferencetime" style="display: none;">
                                                            <td class="Label" style="white-space: nowrap; text-align: left; width: 30% !important">Referance Time Interval* :
                                    
                                                            </td>
                                                            <td class="Label" style="white-space: nowrap; text-align: left; width: 70% !important">
                                                                <asp:TextBox onblur="return ValidTimeInterval(this);" ID="txtRefTimeInterval"
                                                                    runat="server" CssClass="textBox" Style="width: 20% !important">1.00</asp:TextBox><span class="Label "> HR.MM </span>
                                                            </td>

                                                        </tr>

                                                        <tr runat="server" id="trDeviationTime" style="display: none;">
                                                            <td class="Label" style="text-align: left; white-space: nowrap; width: 30%">Deviation Time* :
                                                            </td>
                                                            <td class="Label" style="white-space: nowrap; text-align: left; width: 70%  !important">
                                                                <asp:TextBox onblur="return ValidTimeInterval(this);" ID="txtDevTime" runat="server"
                                                                    CssClass="textBox" Style="width: 20% !important">1</asp:TextBox>
                                                                <span class="Label">MIN.</span>
                                                            </td>
                                                        </tr>

                                                    </table>

                                                </div>

                                            </td>

                                        </tr>

                                        <tr>

                                            <td colspan="2" align="center">
                                                <asp:UpdatePanel ID="UpdatePanelAdd" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:HiddenField ID="HfPeriod" runat="server" />
                                                        <asp:HiddenField ID="HfMaxNodeId" runat="server" />
                                                        <asp:HiddenField ID="HfMaxNodeIndex" runat="server" />
                                                        <asp:Button ID="btnAdd" OnClientClick="return Validation('ADD');" runat="server"
                                                            Text="ADD" ToolTip="Add" CssClass="btn btnnew" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>

                                        </tr>

                                    </table>

                                </div>

                            </ContentTemplate>

                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
                                <asp:asyncpostbacktrigger controlid="btnsave" eventname="click" />
                                <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnRearrange" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="rbtSelection" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="chkMultiDose" EventName="CheckedChanged" />
                            </Triggers>

                        </asp:UpdatePanel>

                    </td>

                </tr>

            </table>

        </div>

        <asp:UpdatePanel ID="UPGrid" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="divRight" style="width: 70%; vertical-align: top; float: right; display: block; padding-top: 1%; height: 560px;" align="center">
                    <div runat="server" id="divButton" style="width: 100%; display: block;">

                        <asp:Button ID="btnRearrange" runat="server" CssClass="btn btnnew" Text="Rearrange"
                            ToolTip="Rearrange" Style="display: none;" />

                        <asp:Button ID="btnSave" OnClientClick="return Validation('SAVE');" runat="server"
                            CssClass="btn btnsave" Text="Save" ToolTip="Save" Style="display: none;" />

                        <asp:Button ID="btnSchedule" runat="server" CssClass="btn btnnew"
                            Text="Schedule" ToolTip="Schedule" Style="display: none;" />

                        <asp:Button ID="btnDelete" runat="server" CssClass="btn btncancel"
                            Text="Delete" ToolTip="Delete" Style="display: none;" OnClientClick="return confirm('Are You Sure You Want To Delete?')" />

                        <asp:Button ID="btnCancel1" runat="server" CssClass="btn btncancel" Text="Cancel"
                            ToolTip="Cancel" OnClientClick="return clearData();" />

                        <asp:Button ID="btnExit" runat="server" CssClass="btn btnexit" Text="Exit" ToolTip="Exit"
                            OnClientClick="return confirmExit();" />

                        <asp:Button ID="btnaudittrail" runat="server" CssClass="btn btnaudit"  ToolTip="Audit Trail" Style="display: none;" />

                    </div>

                    <div runat="server" id="divParent" style="padding-top: 1%; width: 95%; display: none;">

                        <asp:GridView ID="GV_ParentActivity" AutoGenerateColumns="false" runat="server" Width="100%">

                            <Columns>

                                <asp:TemplateField HeaderText="Edit/</br>Delete">
                                    <ItemTemplate>
                                        <input type="checkbox" id="chkEdit" name="chkEdit" runat="server" onchange="return EditRow_ParentActivity(this);" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />

                                </asp:TemplateField>

                                <asp:BoundField HeaderText="Sr.No">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:TemplateField HeaderText="Activity Description">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDesc" runat="server" classname="desc" Text='<% # Eval("vNodeDisplayName")%>'
                                            CssClass="textBox" Enabled="false" onblur="return CheckTextDesc(this)" MaxLength="1000" TextMode="MultiLine" ToolTip='<% # Eval("vNodeDisplayName")%>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="iNodeIndex" HeaderText="NodeIndex" />

                                <asp:BoundField DataField="iNodeId" HeaderText="NodeId" />

                                <asp:BoundField DataField="cIsPreDose" HeaderText="Is PreDose" />

                                <asp:TemplateField HeaderText="Ref.</br>Time">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtRefTime" runat="server" Text='<% # Eval("nRefTime") %>' CssClass="textBox" classname="refTime"
                                            Style="width: 60px ! important" onblur="return ValidTimeInterval(this);" Enabled="false" ToolTip='<% # Eval("nRefTime") %>'></asp:TextBox>HR.MM
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dev.</br>Time">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDevTime" runat="server" Text='<% # Eval("nDeviationTime") %>' classname="devTime"
                                            CssClass="textBox" Style="width: 30px !important" onblur="return ValidTimeInterval(this);" Enabled="false" ToolTip='<% # Eval("nDeviationTime")%>'></asp:TextBox>MIN.
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Pre-</br>Dose">
                                    <ItemTemplate>
                                        <input type="checkbox" id="chkIsPredose" name="chkEditChild" runat="server" disabled="disabled" />
                                        <%--<asp:CheckBox ID="chkIsPredose" runat="server" classname="isPredose" Enabled="false" />--%>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Day">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDay" runat="server" Style="text-align: center; width: 30px !important" Text='<% # Eval("iDayNo")%>' onkeypress="return isNumber(event)"
                                            CssClass="textBox" onblur="return CheckText(this)" Enabled="false" ToolTip='<% # Eval("iDayNo")%>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dose">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDose" runat="server" Style="text-align: center; width: 30px !important" Text='<% # Eval("iDoseNo")%>' onkeypress="return isNumber(event)"
                                            CssClass="textBox" Enabled="false" onblur="return CheckText(this)" ToolTip='<% # Eval("iDoseNo")%>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Domain">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDomainName" runat="server" Style="text-align: center; width: 50px ! important" classname="domain" Text='<% # Eval("vDomainName")%>'
                                            CssClass="textBox" Enabled="false" onblur="return CheckText(this)" ToolTip='<% # Eval("vDomainName")%>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Barcode">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblBarcode" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Re-</br>arrange">
                                    <ItemTemplate>
                                        <asp:Image ID="imgbtnRearrange" runat="server" ImageUrl="~/Images/Drag.png" ToolTip="Drag to Rearrange" CssClass="HandleParent" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnDelete" class="delete" ToolTip="Delete" runat="server" ImageUrl="~/Images/Delete.png" Visible="true" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="cStatusIndi" HeaderText="cStatus" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" />

                                <asp:TemplateField HeaderText="View">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgBtnPreview" runat="server" ImageUrl="~/Images/browse.png" ToolTip="Show Preview" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="ActivityID">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblActivity" Text='<% # Eval("vActivityID")%>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Period">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblPeriod" Text='<% # Eval("iPeriod")%>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>


                            </Columns>

                        </asp:GridView>

                    </div>

                    <div runat="server" id="divChild" style="padding-top: 1%; width: 95%; display: none;">

                        <asp:GridView ID="GV_ChildActivity" AutoGenerateColumns="false" runat="server">

                            <Columns>

                                <asp:TemplateField HeaderText="Edit/</br>Delete">
                                    <ItemTemplate>
                                        <input type="checkbox" id="chkChildEdit" name="chkEdit" runat="server" onchange="return EditChildRow(this);" />

                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>

                                <asp:BoundField HeaderText="Sr. No" ItemStyle-VerticalAlign="Middle">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:TemplateField HeaderText="Activity Description" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDesc" runat="server" classname="desc" Text='<%#Eval("vNodeDisplayName")%>'
                                            CssClass="textBox" Enabled="false" onblur="return CheckTextDesc(this)" MaxLength="1000" TextMode="MultiLine" ToolTip='<%#Eval("vNodeDisplayName")%>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="iNodeIndex" HeaderText="NodeIndex" />
                                <asp:BoundField DataField="iNodeId" HeaderText="NodeId" />
                                <asp:BoundField DataField="cIsPreDose" HeaderText="Is PreDose" />

                                <asp:TemplateField HeaderText="Ref.</br>Time">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtRefTime" runat="server" Text='<% # Eval("nRefTime")%>' CssClass="textBox"
                                            Style="width: 60px ! important" onblur="return ValidTimeInterval(this);" Enabled="false" ToolTip='<% # Eval("nRefTime")%>'></asp:TextBox>HR.MM
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dev.</br>Time">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDevTime" runat="server" Text='<% # Eval("nDeviationTime")%>'
                                            CssClass="textBox" Style="width: 30px ! important" onblur="return ValidTimeInterval(this);" Enabled="false" ToolTip='<% # Eval("nDeviationTime")%>'></asp:TextBox>MIN.
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Pre-</br>Dose">
                                    <ItemTemplate>
                                        <input type="checkbox" id="chkIsPredose" name="chkEditChild" runat="server" disabled="disabled" />

                                        <%--<asp:CheckBox ID="chkIsPredose" runat="server" classname="isPredose" Enabled="false" />--%>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Day">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDay" runat="server" Style="text-align: center; width: 30px !important" Text='<% # Eval("iDayNo")%>' onkeypress="return isNumber(event)"
                                            CssClass="textBox" Enabled="true" onblur="return CheckText(this)" ToolTip='<% # Eval("iDayNo")%>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dose">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDose" runat="server" Style="text-align: center; width: 30px !important" Text='<% # Eval("iDoseNo")%>' onkeypress="return isNumber(event)"
                                            CssClass="textBox" Enabled="true" onblur="return CheckText(this)" ToolTip='<% # Eval("iDoseNo")%>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Domain">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDomainName" runat="server" Style="text-align: center; width: 50px ! important" classname="domain" Text='<% # Eval("vDomainName")%>'
                                            CssClass="textBox" Enabled="false" onblur="return CheckText(this)" ToolTip='<% # Eval("vDomainName")%>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Barcode">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblBarcode" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnDelete" ToolTip="Delete" runat="server" ImageUrl="~/Images/Delete.png" Visible="true" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Re-</br>arrange">
                                    <ItemTemplate>
                                        <asp:Image ID="imgBtnRearrange" runat="server" ImageUrl="~/Images/Drag.png" ToolTip="Drag to Rearrange" CssClass="HandleChild" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="cStatusIndi" HeaderText="cStatus" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" />

                                <asp:TemplateField HeaderText="View">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgBtnPreview" runat="server" ImageUrl="~/Images/browse.png" ToolTip="Show Preview" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="ActivityID">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblActivity" Text='<% # Eval("vActivityID")%>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Period">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblPeriod" Text='<% # Eval("iPeriod")%>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>

                            </Columns>

                        </asp:GridView>

                    </div>

                    <asp:HiddenField ID="HFGV_NodeId" runat="server" />
                    <asp:HiddenField ID="HFGV_Sequence" runat="server" />

                    <div runat="server" id="divreviewer" style="padding-top: 1%; width: 95%; display: none;">
                        <asp:HiddenField ID="Hprofilelist" runat="server" />
                        <asp:HiddenField ID="hindependentprofile" runat="server" />
                        <asp:HiddenField ID="hdataentry" runat="server" />
                        <asp:GridView ID="gvreview" runat="server" AutoGenerateColumns="False">
                            <RowStyle HorizontalAlign="Center"></RowStyle>
                            <Columns>
                                
                                <asp:TemplateField HeaderText="Edit" >
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chkEdit" onchange="return EditReviewerRow(this);" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Reviewer Level" DataField="vReviewerlevel" />

                                <asp:TemplateField HeaderText="Profile Name">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlgvprofile" runat="server" Orientation="Horizontal" onchange="return Validprofile(this)">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="UserType Code">
                                    <ItemTemplate>
                                        <asp:Label ID="lblusertypecode" runat="server" Text='<%# Eval("vUserTypeCode") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Auth. Dialogue">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chkauthentication" CssClass="Edit"  onchange ="return EditCheckB(this);" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Authentication">
                                    <ItemTemplate>
                                        <asp:Label ID="lblauthentication" runat="server" Text='<%# Eval("cAuthenticationDialog") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField>
                                    <HeaderTemplate>
                                        Remarks
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtremarks" runat="server" classname="Remarks"
                                            TextMode="MultiLine" Style="float: center; width: 87%; text-align: left;" Text='<%#eval("vRemarks") %>'
                                            onChange="Check(this, 500);"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <%--<asp:TemplateField HeaderText="Save">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnsave" runat="server" ImageUrl="~/Images/Update.png" Style="display: none;" ToolTip="Save" OnClientClick="return UpdateRow(this);" CommandName="Save"
                                            Width="15px" Height="15px" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="ReviewWorkflowstageid">
                                    <ItemTemplate>
                                        <asp:TextBox ID="lbliReviewWorkflowStageId" runat="server" classname="Remarks"
                                            TextMode="MultiLine" Style="float: center; width: 87%; text-align: left;" Text='<%#eval("iReviewWorkflowStageId") %>' tag='<%#eval("iReviewWorkflowStageId") %>'> </asp:TextBox>
                                        <%--<asp:Label ID="" runat="server" Text='<%# Eval("iReviewWorkflowStageId") %>' tag='<%# Eval("iReviewWorkflowStageId") %>'></asp:Label>--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ActualWorkflowstageid">
                                    <ItemTemplate>
                                        <asp:Label ID="lbliActualWorkflowstageid" runat="server" Text='<%# Eval("iActualWorkflowstageid") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>

                        <div id="dvindependentreviewer" runat="server" style="padding-top: 1%;" align="left">
                            <table>
                                <tr>
                                    <td>Independent reviewer:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlindependentprofile" runat="server" CssClass="dropDownList" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <button id="btnRemarks" runat="server" style="display: none;" />
                <cc1:ModalPopupExtender ID="MPERemarks" runat="server" PopupControlID="divRemarks"
                    BackgroundCssClass="modalBackground" TargetControlID="btnRemarks" CancelControlID="imgReplacement" BehaviorID="MPERemarks">
                </cc1:ModalPopupExtender>

                <div class="modal-content" id="divRemarks" style="display:none;" runat="server">
                    <div class="modal-header">
                        <h2>Please Enter Remarks</h2>
                        <img id="imgReplacement" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px;bottom:45px;" title="Close" onclick="funHideMPEREmarks();" />
                    </div>
                    <div class="modal-body">
                        <table style="width: 100%; text-align: left;">
                            <tr>
                                <td style="text-align: right;" nowrap="noWrap">Remarks* :
                                </td>
                                <td style="text-align: left;" nowrap="noWrap">
                                    <asp:TextBox ID="txtRemarks" runat="Server" Text="" CssClass="textBox" Width="226px"
                                        TextMode="MultiLine" onKeyUp="CheckTextLength(this,500)" onChange="CheckTextLength(this,500)"
                                        Height="50px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left" colspan="2">
                                    <asp:Label ID="lblNote" runat="server" class="LabelBold" ForeColor="Red" Text=" Note:- Maximum 500 characters are allowed "></asp:Label>
                                </td>
                            </tr>
                            <%--<tr>
                                <td colspan="2" align="center">
                                    
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="height: 5px;"></td>
                            </tr>--%>
                        </table>
                    </div>

                    <div class="modal-header">
                        <asp:Button ID="btnSaveRemarks" runat="server" Text="Confirm" CssClass="btn btnsave" 
                                OnClientClick="return ValidationForRemark();" ToolTip="Confirm"></asp:Button>
                    </div>
                </div>

                <button id="btnaudit" runat="server" style="display: none;" />
                <cc1:ModalPopupExtender ID="MPEaudit" runat="server" PopupControlID="divaudit"
                    BackgroundCssClass="modalBackground" TargetControlID="btnaudit" BehaviorID="MPEaudit">
                </cc1:ModalPopupExtender>
                <div id="divaudit" runat="server" class="centerModalPopup" style="display: none; width: 80%; max-height: 250px;">
                    <div style="width: 100%">
                        <h1 class="header">
                            <label id="lblDocAction" class="LabelBold">
                                Audit Trail
                            </label>
                            <img id="ImgPopUpClose" alt="Close" title="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';"
                                onclick="funHideMPE();" />
                        </h1>
                    </div>
                    <div style="width: 100%; max-height: 200px; overflow: auto;">
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:GridView runat="server" ID="GV_Audit" AutoGenerateColumns="false" Width="100%"
                                        BorderStyle="Solid" BorderColor="#1560a1" BorderWidth="1" EmptyDataText="No Audit Trail">
                                        <RowStyle BackColor="#cee3ed" Font-Names="Verdana" VerticalAlign="Top" HorizontalAlign="left"
                                            Font-Size="Small" ForeColor="navy" />
                                        <EditRowStyle BackColor="#cee3ed" Font-Names="Verdana" Font-Size="Small" />
                                        <HeaderStyle Font-Underline="False" BackColor="#1560a1" Font-Names="Verdana" VerticalAlign="Top"
                                            Font-Size="Small" HorizontalAlign="Center" ForeColor="white" Font-Bold="True" />
                                        <FooterStyle BackColor="#1560a1" Font-Names="Verdana" Font-Size="Small" HorizontalAlign="Center"
                                            ForeColor="white" Font-Bold="True" />
                                        <Columns>
                                            <asp:BoundField DataField="vProjectNo" HeaderText="Project No">
                                                <ItemStyle HorizontalAlign="Center" Width="10" />
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="vReviewerlevel" HeaderText="Reviewer level">
                                                <ItemStyle HorizontalAlign="Center" Width="10" />
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="vUserTypeName" HeaderText="Profile">
                                                <ItemStyle HorizontalAlign="Center" Width="150" />
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="cAuthenticationDialog" HeaderText="Authentication Dialog">
                                                <ItemStyle HorizontalAlign="Center" Width="10" />
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="vRemarks" HeaderText="Remarks">
                                                <ItemStyle HorizontalAlign="Center" Width="10" />
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UserNameWithProfile" HeaderText="Modify By">
                                                <ItemStyle HorizontalAlign="Center" Width="10" />
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="dModifyOn_IST" DataFormatString="{0:dd'-'MMM'-'yyyy   HH:mm}"
                                                HeaderText="Modify On">
                                                <ItemStyle HorizontalAlign="Center" Width="150" />
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>

            </ContentTemplate>

            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnreviewer" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="GV_ParentActivity" EventName="RowCommand" />
                <asp:AsyncPostBackTrigger ControlID="GV_ChildActivity" EventName="RowCommand" />
                <asp:AsyncPostBackTrigger ControlID="GV_ParentActivity" EventName="RowDataBound" />
                <asp:AsyncPostBackTrigger ControlID="GV_ChildActivity" EventName="RowDataBound" />
                <asp:AsyncPostBackTrigger ControlID="chkMultiDose" EventName="CheckedChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlParentAct" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlRefAct" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="rbtSelection" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlPeriod" EventName="SelectedIndexChanged" />
            </Triggers>

        </asp:UpdatePanel>

    </div>
    <asp:HiddenField runat="server" ID="hdnCheckBox" />

    <script src="Script/jquery-2.1.0.min.js" type="text/javascript"></script>

    <script src="Script/jquery-ui-1.10.4.custom.min.js" type="text/javascript"></script>

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" src="Script/Gridview.js"></script>

    <script type="text/javascript" src="Script/General.js"></script>

    <script src="Script/jquery.dataTables.min.js" type="text/javascript"></script>

    <script src="Script/jquery.dataTables.plugins.js" type="text/javascript"></script>

    <script src="Script/FixedHeader.min.js" type="text/javascript"></script>

    <script src="Script/jquery.dragtable.min.js" type="text/javascript"></script>

    <script type="text/javascript" src="Script/Validation.js"></script>

     <script src="Script/jquery-ui.js" type="text/javascript"></script>

    <link rel="stylesheet" type="text/css" href="App_Themes/jquery.multiselect.css" />
    <link rel="Stylesheet" type="text/css" href="App_Themes/StyleBlue/UI_Theme/jquery-ui.css" />
    <script src="Script/jquery.multiselect.min.js" type="text/javascript"></script>


    <script type="text/javascript">
        var ArrSub = [];
        function pageLoad() {
            fnApplyDtAddParentActivity()
            fnApplyDtAddChildActivity()
            fnapplydtreviewerlevel();
            if ($("#<%= ddlindependentprofile.ClientID%>")) {
                fnApplySelectprofile();
                var hdataentrycheck = document.getElementById('<%= hdataentry.ClientID %>');
                if (hdataentrycheck.value != '') {
                    $('input[name$="multiselect_ctl00_CPHLAMBDA_ddlindependentprofile"]').attr('disabled', 'disabled');
                    $(".ui-multiselect-none").last().html("");
                    $(".ui-multiselect-all").last().html("");

                }

                //$("#ctl00_CPHLAMBDA_ddlRereviewActivity").next().css({ 'width': '60%' })
            }
        }
        function fnapplydtreviewerlevel() {

            if ($get('<%= gvreview.ClientID %>') != null && $get('<%= gvreview.ClientID %>_wrapper') == null) {
                var oTab = $('#<%= gvreview.ClientID %>').prepend($('<thead>').append($('#<%= gvreview.ClientID %> tr:first'))).dataTable({
                    "bJQueryUI": true,
                    "bStateSave": false,
                    "bDestory": true,
                    "bPaginate": false,
                    "bInfo": true,
                    "bFilter": false,
                    "bSort": false,
                    "bRetrieve": true,
                    "bFooter": false,
                    "bAutoWidth": true,
                    "sDom": '<"H"lfr>t<"F"p>'
                });
                //$('.dataTables_scrollHeadInner').removeAttr("style");
                //$('input[id*="chkEdit"]', $get('ctl00_CPHLAMBDA_gvreview')).removeAttr('checked');
            }
        }

        function fnApplySelectprofile() {
            //var chkdataentry = document.getElementById('<%= hdataentry.ClientID %>').value;
            $("#<%= ddlindependentprofile.ClientID%>").multiselect({
                noneSelectedText: "--Select Profile--",
                minWidth: "auto",
                click: function (event, ui) {

                    if (ui.checked == true)
                        ArrSub.push(ui.value);
                    else if (ui.checked == false) {
                        if ($.inArray(ui.value, ArrSub) >= 0)
                            ArrSub.splice(ArrSub.indexOf(ui.value), 1);
                    }

                },
                checkAll: function (event, ui) {

                    ArrSub = [];
                    for (var i = 0; i < event.target.options.length; i++) {
                        ArrSub.push($(event.target.options[i]).val());
                    }
                    $("#<%= ddlindependentprofile.ClientID%>").multiselect("refresh");
                    $("#<%= ddlindependentprofile.ClientID%>").multiselect("widget").find(':checkbox').click();

                },
                uncheckAll: function (event, ui) {

                    ArrSub = [];
                    $("#<%= ddlindependentprofile.ClientID%>").multiselect("refresh");

                }
            });

            $("#<%= ddlindependentprofile.ClientID%>").multiselect("refresh");
            var CheckedCheckBox = document.getElementById('<%= hindependentprofile.ClientID %>').value.replace("'", "");
            if (CheckedCheckBox != "") {
                ArrSub = [];
                CheckedCheckBox = CheckedCheckBox.split(',');
                for (i = 0; i <= CheckedCheckBox.length - 1; i++) {
                    $("#<%= ddlindependentprofile.ClientID %>").multiselect("widget").find(".ui-multiselect-checkboxes.ui-helper-reset").find("input[value='" + CheckedCheckBox[i] + "']").attr("checked", "checked");
                    ArrSub.push(CheckedCheckBox[i])
                }
                $('#<%= ddlindependentprofile.ClientID%>').multiselect("update");
            }
        }

        //function fnApplySelectprofile() {
        //    fnDeletePreviousMultiselect();
        //    $("#<%= ddlindependentprofile.ClientID%>").multiselect({
        //        noneSelectedText: "--Select profile--",
        //        click: function (event, ui) {
        //            if (ui.checked == true)
        //                profile.push("'" + ui.value + "'");
        //            else if (ui.checked == false) {
        //                if ($.inArray("'" + ui.value + "'", profile) >= 0)
        //                    profile.splice(profile.indexOf("'" + ui.value + "'"), 1)
        //            }
        //
        //            //if ($("input[name$='ddlindependentprofile']").length > 0) {
        //            //    //clearControls();
        //            //}
        //        },
        //        checkAll: function (event, ui) {
        //            profile = [];
        //            for (var i = 0; i < event.target.options.length; i++) {
        //                profile.push("'" + $(event.target.options[i]).val() + "'")
        //            }
        //            //if ($("input[name$='ddlindependentprofile']").length > 0) {
        //                //clearControls();
        //            //}
        //            //$("#<%= ddlindependentprofile.ClientID%>").multiselect("refresh");
        //            //$("#<%= ddlindependentprofile.ClientID%>").multiselect("widget").find(':checkbox').click();
        //
        //        },
        //        uncheckAll: function (event, ui) {
        //            profile = [];
        //            //$("#<%= ddlindependentprofile.ClientID%>").multiselect("refresh");
        //            //if ($("input[name$='ddlindependentprofile':checked]").length > 0) {
        //                //clearControls();
        //            //}
        //        }
        //    });
        //    $("#<%= ddlindependentprofile.ClientID%>").multiselect("refresh");
        //    var CheckedCheckBox = document.getElementById('<%= hindependentprofile.ClientID %>').value;
        //    if (CheckedCheckBox != "") {
        //
        //        CheckedCheckBox = CheckedCheckBox.split(',');
        //        for (i = 0; i <= CheckedCheckBox.length - 1; i++) {
        //            $("#<%= ddlindependentprofile.ClientID %>").multiselect("widget").find(".ui-multiselect-checkboxes.ui-helper-reset").find("input[value=" + CheckedCheckBox[i] + "]").attr("checked", "checked");
        //
        //        }
        //        $('#<%= ddlindependentprofile.ClientID%>').multiselect("update");
        //    }
        //}

        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientID%>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
             $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        function IsNotZero() {
            var result = document.getElementById('<%= txtRepeatation.ClientID %>').value

            if (result < 1) {
                msgalert("No. of Repetition can not be Zero !");
                document.getElementById('<%= txtRepeatation.ClientID %>').focus();
                document.getElementById('<%= txtRepeatation.ClientID %>').value = "1";
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

        function Validation(type) {
            if (document.getElementById('<%= HProjectId.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Project !');
                document.getElementById('<%= txtProject.ClientId %>').focus();
                document.getElementById('<%= txtProject.ClientId %>').value = '';
                return false;
            }

            else if (type.toUpperCase() == 'ADD' && document.getElementById('<%= ddlActivityGroup.ClientId %>').selectedIndex == 0) {
                msgalert('Please select Activity Group !');
                document.getElementById('<%= ddlActivityGroup.ClientId %>').focus();
                return false;
            }

            else if (type.toUpperCase() == 'ADD' && document.getElementById('<%= ddlActivity.ClientId %>').selectedIndex == 0) {
                msgalert('Please select Activity !');
                document.getElementById('<%= ddlActivity.ClientId %>').focus();
                return false;
            }
            else if (type.toUpperCase() == 'SEARCH' && document.getElementById('<%=ddlParentAct.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Parent Activity !');
                document.getElementById('<%= ddlParentAct.ClientId %>').focus();
                return false;
            }
            else if (type.toUpperCase() == 'ADD' && document.getElementById('<%=ddlParentAct.ClientID%>').selectedIndex == 0 && $('input:checked', '#<%=rbtSelection.ClientID%>')[0].value.toUpperCase() == "CHILDACTIVITY") {
                msgalert('Please select Parent Activity !');
                document.getElementById('<%= ddlParentAct.ClientId %>').focus();
                return false;
            }
            if ($('input:checked', '#<%=rbtSelection.ClientID%>')[0].value.toUpperCase() == "DEFINEWORKFLOW") {
                if (ArrSub.length == 0) {
                    msgalert('Please select independent profile !');
                    return false;
                }
                var reviewertable = document.getElementById('<%=  gvreview.ClientID %>');
                var rowcount = $($(reviewertable)[0]).children('tbody').children('tr');
                var boolcheckprofile = false;
                var arrayreviewer = [];
                for (var j = 0; j <= rowcount.length - 1; j++) {
                    var rowspecificcurrent = $($(reviewertable)[0]).children('tbody').children('tr')[j];
                    if (j != 0) {
                        var rowspecificprevious = $($(reviewertable)[0]).children('tbody').children('tr')[j - 1];
                        if ($('input[id$="chkEdit"]', rowspecificcurrent)[0].checked == true) {
                            $('[id$="lbliReviewWorkflowStageId"]', rowspecificprevious).each(function () {
                                if (this.value == '0') {
                                    msgalert('Please select previous reviewer profile !');
                                    boolcheckprofile = true;
                                    return false;
                                }
                            });
                            if (boolcheckprofile == true) {
                                $('input[id$="chkEdit"]', rowspecificcurrent)[0].checked = false;
                                return false;
                            }
                        }
                    }
                    var profileid = $('select[id$="ddlgvprofile"]', rowspecificcurrent)[0].id;
                    if ($get(profileid).options[$get(profileid).selectedIndex].value != '0000') {
                        arrayreviewer.push($get(profileid).options[$get(profileid).selectedIndex].value);
                    }
                }
                if (arrayreviewer.length > 1) {
                    for (var i = 0; i <= arrayreviewer.length - 1; i++) {
                        for (var j = 0; j <= arrayreviewer.length - 1; j++) {
                            if (i != j) {
                                if (arrayreviewer[i] == arrayreviewer[j]) {
                                    msgalert('You cannot select same profile at different reviewer stage !');
                                    boolcheckprofile = true;
                                    return false;
                                }
                            }
                        }
                    }
                }

                document.getElementById('<%= hindependentprofile.ClientID%>').value = ArrSub;
            }
            
        return true;
    }

function getParentSequence() {
    var i = 1;
    var j = 1;
    var sequencedata = [];
    var reviewertable = document.getElementById('<%=gvreview.ClientID %>');
    $('.SaveParent').each(function () {
        var ContentActivity = new Object();
        $(this).children("td:nth-child(2)").html(i)
        ContentActivity.iNodeId = $(this).children("td:nth-child(5)").html();
        ContentActivity.iNodeNo = i;
        sequencedata.push(ContentActivity);
        i = parseInt(i) + 1;
        var rowspecificprevious = $($(reviewertable)[0]).children('tbody').children('tr')[j - 1];
    });
    $('#<%= HFGV_Sequence.ClientId %>').val(JSON.stringify(sequencedata));
    $('#<%=btnRearrange.ClientID%>').css('display', 'inline');
    $('input[id$="chkEdit"]', '#ctl00_CPHLAMBDA_GV_ParentActivity').attr('disabled', 'disabled');

    return true;
}

function fnApplyDtAddParentActivity() {

    if ($get('<%= GV_ParentActivity.ClientID %>') != null && $get('<%= GV_ParentActivity.ClientID %>_wrapper') == null) {
        var oTab = $('#<%= GV_ParentActivity.ClientID %>').prepend($('<thead>').append($('#<%= GV_ParentActivity.ClientID %> tr:first'))).dataTable({
            "bJQueryUI": true,
            "sScrollY": '480px',
            //"sScrollX": "100%",
            //scrollCollapse: true,
            "bStateSave": false,
            "bDestory": true,
            "bPaginate": false,
            "bInfo": true,
            "bFilter": true,
            "bSort": false,
            "bRetrieve": true,
            "bFooter": false,
            "bAutoWidth": true,
            "sDom": '<"H"lfr>t<"F"p>'

        });
        $('.dataTables_scrollHeadInner').removeAttr("style");
        $('input[id*="chkEdit"]', $get('ctl00_CPHLAMBDA_GV_ParentActivity')).removeAttr('checked');
    }
    $('#<%= GV_ParentActivity.ClientId %> tbody').sortable({
        handle: ".HandleParent",
        activate: function (event, ui) {
            $('.ui-sortable-helper').removeAttr('style');
        },
        deactivate: function (event, ui) {
            getParentSequence();

        },
        stop: function (event, ui) {
            if (ui.item.children("th").length != 0 || ui.item.children("td:nth-child(2)").html() == "&nbsp;") {
                $(this).sortable("cancel");
            }
        }
    })
}

function fnApplyDtAddChildActivity() {

    if ($get('<%= GV_ChildActivity.ClientID %>') != null && $get('<%= GV_ChildActivity.ClientID %>_wrapper') == null) {
        $('#<%= GV_ChildActivity.ClientID %>').prepend($('<thead>').append($('#<%= GV_ChildActivity.ClientID %> tr:first'))).dataTable({
            "bJQueryUI": true,
            "sScrollY": '480px',

            "bStateSave": false,
            "bDestory": true,
            "bPaginate": false,
            "bInfo": true,
            "bFilter": true,
            "bSort": false,
            "bRetrieve": true,
            "bFooter": false,
            "bAutoWidth": false,
            "sDom": '<"H"lfr>t<"F"p>'

        });

        $('.dataTables_scrollHeadInner').removeAttr("style");
        $('input[id*="chkChildEdit"]', $get('ctl00_CPHLAMBDA_GV_ChildActivity')).removeAttr('checked');
    }
    $('#<%= GV_ChildActivity.ClientId %> tbody').sortable({
        handle: ".HandleChild",
        deactivate: function (event, ui) {
            getChildSequence();
        },
        stop: function (event, ui) {
            if (ui.item.children("th").length != 0 || ui.item.children("td:nth-child(2)").html() == "&nbsp;") {
                $(this).sortable("cancel");
            }
        }
    })
}

function getChildSequence() {
    var i = 1;
    var sequencedata = [];
    $('.SaveChild').each(function () {
        var ContentActivity = new Object();
        $(this).children("td:nth-child(2)").html(i)
        ContentActivity.iNodeId = $(this).children("td:nth-child(5)").html();
        ContentActivity.iNodeNo = i;
        sequencedata.push(ContentActivity);
        i = parseInt(i) + 1;
    });
    $('#<%= HFGV_Sequence.ClientId %>').val(JSON.stringify(sequencedata));
    $('#<%=btnRearrange.ClientID%>').css('display', 'inline');
    $('input[id$="chkChildEdit"]', '#ctl00_CPHLAMBDA_GV_ChildActivity').attr('disabled', 'disabled');
    $('#<%=btnSchedule.ClientID%>').css('display', 'none');
    return true;
}

function EditRow_ParentActivity(row) {

    if ($('input[id$="chkEdit"]:checked', '#<%=GV_ParentActivity.ClientID%>').length > 0) {
        $('#<%=btnDelete.ClientID%>, #<%=btnSave.ClientID%>').css('display', '');

        $('#<%=btnRearrange.ClientID%>').css('display', 'none');

        $('img[id$="imgbtnRearrange"], #<%=GV_ParentActivity.ClientID%>').removeClass("HandleParent");

    }
    else {
        $('#<%=btnDelete.ClientID%>, #<%=btnSave.ClientID%>').css('display', 'none');
        $('img[id$="imgbtnRearrange"], #<%=GV_ParentActivity.ClientID%>').addClass("HandleParent");
    }

    var rownode = row.parentNode.parentNode;
    if (row.checked == true) {

        $('textarea,input[type="text"],input[id*="chkIsPredose"]', rownode).removeAttr("disabled")
    }
    else {
        $('textarea,input[type="text"],input[id*="chkIsPredose"]', rownode).each(function () {
            this.value = this.defaultValue;
            $(this).attr('disabled', 'disabled');
        });
    }
    return false;
}

function CheckText(row) {
    if (row.value.trim() == "") {
        msgalert("Please enter value !");
        //ele.focus();
    }
    
    if (row.value.trim().length > 2) {
        msgalert("Domain Name Length is Minimum 2 Character Long !");
        row.value = "";
        return false;
    }
}


function CheckTextDesc(row) {
    if (row.value.trim() == "") {
        msgalert("Please enter value !");
        row.focus();
    }
    else {
        GenerateBarcode(row);
    }
}

function GenerateBarcode(row) {
    var TimePoint, Barcode, vnodename
    var rownode = row.parentNode.parentNode;
    TimePoint = row.value
    vnodename = TimePoint.split(" ")[0].toUpperCase().replace("HR", "");
    TimePoint = TimePoint.toUpperCase().replace(vnodename, "");

    if ($('span[id*="lblActivity"]', rownode).text() == '1153' || $('span[id*="lblActivity"]', rownode).text() == '2053' || $('span[id*="lblActivity"]', rownode).text() == '2483') {
        if (vnodename.toLowerCase() == "pre") {
            var nodearray = TimePoint.split("-")
            if (nodearray.length > 1) {
                if (nodearray(nodearray.length - 1).toString() != ")") {
                    Barcode = vnodename + " DOSE " + (nodearray.length - 1)
                }
                else {
                    Barcode = vnodename + " DOSE"
                }
            }
            else {
                Barcode = vnodename + " DOSE"
            }

        }
        else {
            var nodearray = TimePoint.split("-")
            if (nodearray.length > 1) {
                if ((nodearray.length - 1).toString() != ")") {
                    Barcode = vnodename + " HR " + (nodearray.length - 1)
                }
                else {
                    Barcode = vnodename + " HR"
                }
            }
            else {
                Barcode = vnodename + " HR "
            }

        }
        var BarcodeName = Barcode + "(xxxx)"
        if (BarcodeName.length > 17) {
            BarcodeName = BarcodeName.substr(0, 17)
        }

        $('span[id*="lblBarcode"]', rownode).text(BarcodeName)
        return true;
    }
    else
        return false;

}

function EditChildRow(row) {
    if ($('input[id$="chkChildEdit"]:checked', '#<%=GV_ChildActivity.ClientID%>').length > 0) {
        $('#<%=btnDelete.ClientID%>, #<%=btnSave.ClientID%>').css('display', '');
        $('#<%=btnRearrange.ClientID%>').css('display', 'none');
        $('#<%=btnSchedule.ClientID%>').css('display', 'none');
        $('img[id$="imgBtnRearrange"], #<%=GV_ChildActivity.ClientID%>').removeClass("HandleChild");

    }
    else {
        $('#<%=btnDelete.ClientID%>, #<%=btnSave.ClientID%>').css('display', 'none');
        $('img[id$="imgBtnRearrange"], #<%=GV_ChildActivity.ClientID%>').addClass("HandleChild");
        $('#<%=btnSchedule.ClientID%>').css('display', '');
    }


    var rownode = row.parentNode.parentNode;

    if (row.checked == true) {
        $('textarea,input[type="text"],input[id*="chkIsPredose"]', rownode).removeAttr("disabled")

    }
    else {

        $('textarea,input[type="text"],input[id*="chkIsPredose"]', rownode).each(function () {
            this.value = this.defaultValue;
            $(this).attr('disabled', 'disabled');
        });
    }
    return false;
}

function clearControls() {
    $('#ddlParentAct').index(0);
    $('#ddlRefAct').index(0);
    $("#btnRearrange").hide();
}

function clearData() {
    window.location.replace(window.location.href);
}

function confirmExit() {
    msgConfirmDeleteAlert(null, "Are you sure want to Exit ?", function (isConfirmed) {
        if (isConfirmed) {
            window.location.replace("frmMainPage.aspx");
            return true;
        } else {
            return false;
        }
    });
    return false;
}

function OpenWindow(Path) {
    window.open(Path);
    return false;
}

function open_ProjStruct() {
    window.open("frmEditWorkspaceNodeDetail.aspx?WorkSpaceId=" + document.getElementById('<%= HProjectId.ClientId%>').value, '_blank');

}

function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

function edited() {
    pageLoad();
    msgalert('Activity Edited Successfully !');
}

jQuery.browser = {};
(function () {
    jQuery.browser.msie = false;
    jQuery.browser.version = 0;
    if (navigator.userAgent.match(/MSIE ([0-9]+)\./)) {
        jQuery.browser.msie = true;
        jQuery.browser.version = RegExp.$1;
    }
})();
function EditReviewerRow(row) {
    var rownode = row.parentNode.parentNode;
    var rowposition = row.parentElement.parentElement.rowIndex;
    var reviewertable = document.getElementById('<%=  gvreview.ClientID %>');
            var boolcheckprofile = false;

            if ($('input[id$="chkEdit"]', row)[0].checked == true) {
                for (var j = 0; j <= rowposition - 2; j++) {
                    var rowspecific = $($(reviewertable)[0]).children('tbody').children('tr')[j];
                    $('[id$="lbliReviewWorkflowStageId"]', rowspecific).each(function () {
                        if (this.value == '0') {
                            msgalert('Please select previous reviewer profile !');
                            boolcheckprofile = true;
                            return false;
                        }
                    });
                    if (boolcheckprofile == true) {
                        $('input[id$="chkEdit"]', row)[0].checked = false;
                        return false;
                    }
                }
                $('select[id$="ddlgvprofile"]', rownode).removeAttr("disabled")
                $('input[id$="chkauthentication"]', rownode).removeAttr("disabled")
            }
            else {
                $('select[id$="ddlgvprofile"]', rownode).each(function () {
                    this.value = $(this).attr('tag');
                    $(this).attr('disabled', 'disabled');
                });
                $('input[id$="chkauthentication"]', rownode).each(function () {
                    this.checked = this.defaultChecked;
                    $(this).attr('disabled', 'disabled');
                });
                $('[id$="lbliReviewWorkflowStageId"]', rownode).each(function () {
                    this.value = $(this).attr('tag');
                    //this.textContent = $(this).attr('tag');
                });

            }
        }

        function Check(textBox, maxLength) {
            if (textBox.value.length > maxLength) {
                msgalert("Max characters allowed are " + maxLength);
                textBox.value = textBox.value.substr(0, maxLength);
            }
        }

        function Validprofile(row) {
            var rowposition = row.parentElement.parentElement.rowIndex;
            if (rowposition == 1) {
                if (row.value == '0000') {
                    msgalert('You cannot deselect first reviewer !');
                    row.value = $(row).attr('tag');
                    $('[id$="lbliReviewWorkflowStageId"]', rownode).each(function () {
                        //this.textContent = $(this).attr('tag');
                        this.value = $(this).attr('tag');
                    });
                    return false;
                }
            }
            var rownode = row.parentNode.parentNode;
            var profilelist = JSON.parse(document.getElementById('ctl00_CPHLAMBDA_Hprofilelist').value);
            var arrayworkflow = [];
            var grid = document.getElementById('<%= gvreview.ClientId%>');
            var iworkflow = -1;
            var profilecode = row.value;
            $('[id$="lbliReviewWorkflowStageId"]', grid).each(function () {
                arrayworkflow.push(this.value);
                //arrayworkflow.push(this.textContent);
            });
            if (arrayworkflow.indexOf('0') > -1) {
                arrayworkflow.splice(arrayworkflow.indexOf('0'), 1);
                //if (arrayworkflow.indexOf('0') == rowposition) {
                //    arrayworkflow.splice(arrayworkflow.indexOf('0'), 1);
                //}
                //else {
                //    arrayworkflow.splice(rowposition - 1, 1);
                //    arrayworkflow.splice(arrayworkflow.indexOf('0'), 1);
                //}
            }
            else { arrayworkflow.splice(rowposition - 1, 1); }

            $.each(profilelist, function (i, p) {

                if (profilelist[i]["vUserTypeCode"] == profilecode) {
                    iworkflow = profilelist[i]["iWorkflowStageId"];
                }
            });

            if (iworkflow > 0) {
                for (var i = 0; i <= arrayworkflow.length - 1; i++) {
                    if (arrayworkflow[i] == iworkflow) {
                        msgalert('This profile stage is already belong to another reviewer level !');
                        row.value = $(row).attr('tag');
                        $('[id$="lbliReviewWorkflowStageId"]', rownode).each(function () {
                            this.value = $(this).attr('tag');
                            //this.textContent = $(this).attr('tag');
                        });
                        return false;
                    }
                }
            }
            $('[id$="lbliReviewWorkflowStageId"]', rownode).each(function () {
                //this.textContent = iActualworkflow;
                this.value = iworkflow;
            });
        }
        function CheckTextLength(text, long) {
            var maxlength = new Number(long); // Change number to your max length.
            if (text.value.length > maxlength) {
                text.value = text.value.substring(0, maxlength);
                msgalert(" Only " + long + " characters allowed !");
            }
        }

        function ValidationForRemark() {
            if ($get('<%= txtRemarks.ClientID%>').value.trim() == '') {
                msgalert('Please Enter Remarks !');
                $get('<%= txtRemarks.ClientID%>').value = '';
                $get('<%= txtRemarks.ClientID%>').focus();
                return false;
            }
        }

        function funHideMPE() {
            $find('MPEaudit').hide();
            var reviewertable = document.getElementById('<%=  gvreview.ClientID %>');
            var rowcount = $($(reviewertable)[0]).children('tbody').children('tr');
            for (var j = 0; j <= rowcount.length - 1; j++) {
                var rowspecificcurrent = $($(reviewertable)[0]).children('tbody').children('tr')[j];
                if ($('input[id$="chkEdit"]', rowspecificcurrent)[0].checked == true) {
                    $('select[id$="ddlgvprofile"]', rowspecificcurrent).removeAttr("disabled")
                    $('input[id$="chkauthentication"]', rowspecificcurrent).removeAttr("disabled")
                }
            }
        }

        function funHideMPEREmarks() {
            $find('MPERemarks').hide();
            var reviewertable = document.getElementById('<%=  gvreview.ClientID %>');
            var rowcount = $($(reviewertable)[0]).children('tbody').children('tr');
            for (var j = 0; j <= rowcount.length - 1; j++) {
                var rowspecificcurrent = $($(reviewertable)[0]).children('tbody').children('tr')[j];
                if ($('input[id$="chkEdit"]', rowspecificcurrent)[0].checked == true) {
                    $('select[id$="ddlgvprofile"]', rowspecificcurrent).removeAttr("disabled")
                    $('input[id$="chkauthentication"]', rowspecificcurrent).removeAttr("disabled")
                }
            }
        }
        function EditCheckB(row) {
            var rownode = row.parentNode.parentNode;
            var rowposition = row.parentElement.parentElement.rowIndex;
            var reviewertable = document.getElementById('<%=  gvreview.ClientID %>');


            if ($('input[id$="chkauthentication"]', row)[0].checked == true) {
                $('input[id$="chkauthentication"]', row).attr('checked', 'checked');
            }
            else {
                $('input[id$="chkauthentication"]', row).removeAttr('checked');
            }
            var i = 0
            var value = ""
            for (i = 2; i < 5; i++) {
                if ($("#ctl00_CPHLAMBDA_gvreview_ctl0" +i+"_chkauthentication").is(':checked') == true) {
                    value += 'Y,'
                }
                else {
                    value += 'N,'
                }
            }
            $("#ctl00_CPHLAMBDA_hdnCheckBox").val(value)

        }

        function CheckBoxChecked() {
            var value = $("#ctl00_CPHLAMBDA_hdnCheckBox").val();
            var i = 0
            var j = -1
            for (i = 0; i < 8; i += 2) {
                j = j +1
                if (value[i] == "Y") {
                    var id = $('input[id$="chkauthentication"]')[j].id
                    $("#"+id).attr('checked', 'checked');
                }
                else {
                    var id = $('input[id$="chkauthentication"]')[j].id
                    $("#" + id).removeAttr('checked');
                }
            }
        }
        function CheckUnCheck(id) {
            var ChkId = id.id
            if ($('#' + ChkId).is(':checked')) {
                $("#" + ChkId).prop('checked', true);
            }
            else {
                ChkId.checked = false
                $("#" + ChkId).prop('checked', false);
            }
        }



        //function closepopup() {
        //    var reviewertable = document.getElementById('<%=  gvreview.ClientID %>');
        //    var rowcount = $($(reviewertable)[0]).children('tbody').children('tr');
        //    var boolcheckprofile = false;
        //    for (var j = 0; j <= rowcount - 1; j++) {
        //        var rowspecificcurrent = $($(reviewertable)[0]).children('tbody').children('tr')[j];
        //        if ($('input[id$="chkEdit"]', rowspecificcurrent)[0].checked == true) {
        //            $('select[id$="ddlgvprofile"]', rowspecificcurrent).removeAttr("disabled")
        //            $('input[id$="chkauthentication"]', rowspecificcurrent).removeAttr("disabled")
        //        }
        //    }
        //
        //    return false;
        //}
</script>
</asp:Content>

