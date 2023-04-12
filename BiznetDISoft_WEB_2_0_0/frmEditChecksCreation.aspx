<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmEditChecksCreation.aspx.vb" Inherits="frmEditChecksCreation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
       <script src="Script/popcalendar.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    
    <asp:UpdatePanel ID="upEditChecks" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset id="fldgen" class="FieldSetBox" style="width: 85%; margin: auto; margin-top: 20px;">
                <legend id="lblProject" runat="server" class="LegendText" style="font-weight: bold;
                    font-size: 12px; text-align: left;">
                    <img id="imgfldgen" alt="SubjectSpecific" src="images/expand.jpg" onclick="displayProjectInfo(this,'tblgen');"
                        runat="server" />
                    Project Details</legend>
                <div id="tblgen">
                    <table cellpadding="3" align="center" width="85%">
                        <tr>
                            <td class="Label" nowrap="nowrap" style="text-align: right; width: 30%;">
                                Project Name/Request Id* : &nbsp;
                            </td>
                            <td class="Label" style="text-align: left; width: 70%;">
                                <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="400px" TabIndex="1"></asp:TextBox><asp:Button
                                    Style="display: none" ID="btnSetProject" runat="server" Text=" Project"></asp:Button>
                                <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                    CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                    CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                    OnClientShowing="ClientPopulated" ServiceMethod="GetMyProjectCompletionList"
                                    ServicePath="AutoComplete.asmx" CompletionListElementID="pnlTemplate" TargetControlID="txtProject"
                                    UseContextKey="True">
                                </cc1:AutoCompleteExtender>
                                <asp:Panel ID="pnlTemplate" runat="server" Style="max-height: 200px; overflow: auto;
                                    overflow-x: hidden;" />
                            </td>
                        </tr>
                        <%--<tr id="VersionDtl" runat="server" style="display: none;" valign="top">
                    <td>
                    </td>
                    <td style="text-align: left; vertical-align: top;">
                        <span class="Label">Version : </span>
                        <asp:Label runat="server" ID="VersionNo" Style="font-weight: normal; font-size: 8pt;"></asp:Label>
                        <span class="Label" style="padding-left: 20px;">Version Date : </span>
                        <asp:Label ID="VersionDate" runat="server" Style="font-weight: normal; font-size: 8pt;"></asp:Label>
                        <span class="Label" style="padding-left: 20px;">Status: </span>
                        <img src="../images/Freeze.jpg" id="ImageLockUnlock" runat="server" alt="Lock" />
                    </td>
                </tr>--%>
                        <tr id="trPeriod" runat="server" style="display: table-row;">
                            <td class="Label" nowrap="nowrap" style="text-align: right; width: 30%;">
                                Period* : &nbsp;
                            </td>
                            <td align="left" style="text-align: left; width: 70%;">
                                <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="dropDownList" Width="110px"
                                    AutoPostBack="True" TabIndex="2" EnableViewState="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="trVisit" runat="server" style="display: table-row;">
                            <td class="Label" nowrap="nowrap" style="text-align: right; width: 30%;">
                                Visit/Parent Activity* : &nbsp;
                            </td>
                            <td align="left" style="text-align: left; width: 70%;">
                                <asp:DropDownList ID="ddlVisit" runat="server" CssClass="dropDownList" Width="300px"
                                    AutoPostBack="True" TabIndex="2" EnableViewState="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="Label" nowrap="nowrap" style="text-align: right; width: 30%">
                                Select Activity* : &nbsp;
                            </td>
                            <td class="Label" nowrap="nowrap" style="text-align: left; width: 70%">
                                <asp:DropDownList ID="ddlActivity" TabIndex="3" runat="server" CssClass="dropDownList"
                                    Width="305px" AutoPostBack="True" onChange="fnActivityChange();">
                                </asp:DropDownList>
                                <asp:HiddenField ID="hdnNodeID" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td style="text-align: left">
                                <asp:RadioButtonList CellPadding="0" CellSpacing="0" ID="rdbEditChecksType" CssClass="Label"
                                    runat="server" RepeatColumns="2" RepeatDirection="Horizontal" AutoPostBack="true"
                                    Enabled="false" Style="float: left; margin-right: 15px;">
                                    <asp:ListItem Selected="True" Text="Within Activity" Value="Within Activity"></asp:ListItem>
                                    <asp:ListItem Text="Cross Activity" Value="Cross Activity"></asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:HiddenField ID="hdnCrossNodes" runat="server" />
                                <asp:Button ID="btnViewEditChecks" TabIndex="4" runat="server" Text="View" CssClass="btn btnnew"
                                    OnClientClick="return fnViewClick();" />
                                <asp:Button ID="btnExportToExcel" Style="display: none;" runat="server"
                                    CssClass="btn btnexcel" OnClientClick="return fnViewClick();" />
                                <asp:Button ID="btnViewCancel" Visible="false" runat="server" Text="Create" CssClass="btn btncancel"  OnClientClick="return fnClear();" />
                            </td>
                        </tr>
                    </table>
                </div>
            </fieldset>
            <fieldset id="fldsFormula" runat="server"  style="width: 85%; margin: auto; margin-top: 10px;" class="FieldSetBox">
                <legend id="lblEdit" runat="server" class="LegendText" style="font-weight: bold;
                    font-size: 12px; text-align: left;">
                    <img id="imgFormula" alt="SubjectSpecific" src="images/expand.jpg" onclick="displayProjectInfo(this,'divFormula');"
                        runat="server" />
                    Edit Check Formula</legend>
                <table style="width: 100%; margin: auto;" id="tblFormula" runat="server">
                    <tr>
                        <td>
                            <div id="divFormula">
                                <table cellpadding="3" width="100%" style="margin-left: 10px;">
                                    <tr>
                                        <td style="height: 5px;" colspan="6">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; width: 17%" valign="top">
                                            <asp:Button ID="btnInsertAttribute" runat="server"  Text="Insert Attribute"
                                                CssClass="btn btnsave" TabIndex="5" Enabled="false"  ></asp:Button>
                                            <%--<asp:LinkButton ID="lnkDummy" runat="server"></asp:LinkButton>--%>
                                        </td>
                                        <td style="text-align: left; width: 17%;" valign="top">
                                            <asp:DropDownList ID="ddlOperator" runat="server" CssClass="dropDownList" Height="22"
                                                Width="120px" onChange="fnInsertOperator(this);" TabIndex="6" Enabled="false"  style="margin-top: 4%;">
                                                <asp:ListItem Text="Select Operator"></asp:ListItem>
                                                <asp:ListItem Text="&gt;"></asp:ListItem>
                                                <asp:ListItem Text="&lt;"></asp:ListItem>
                                                <asp:ListItem Text="&gt;="></asp:ListItem>
                                                <asp:ListItem Text="&lt;="></asp:ListItem>
                                                <asp:ListItem Text="="></asp:ListItem>
                                                <asp:ListItem Text="&lt;&gt;"></asp:ListItem>
                                                <%--   <asp:ListItem Text="+"></asp:ListItem>
                                            <asp:ListItem Text="*"></asp:ListItem>
                                            <asp:ListItem Text="-"></asp:ListItem>
                                            <asp:ListItem Text="/"></asp:ListItem>
                                            <asp:ListItem Text="("></asp:ListItem>
                                            <asp:ListItem Text=")"></asp:ListItem>--%>
                                                <asp:ListItem Text="Is NULL" Value="= '' "></asp:ListItem>
                                                <asp:ListItem Text="Is Not NULL" Value="!= '' "></asp:ListItem>
                                                <asp:ListItem Text="Between"></asp:ListItem>
                                                <asp:ListItem Text="Not Between"></asp:ListItem>
                                                <asp:ListItem Text="(" Value="(" ></asp:ListItem>
                                                <asp:ListItem Text=")" Value=")" ></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="Label" style="text-align: left; width: 7%;" runat="server" id="tdValues">
                                            Value :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Panel ID="pnlMedExAttributeValues" BorderWidth="1" BorderColor="Gray" runat="server"
                                                Width="100%" Height="50px" ScrollBars="Auto" Style="display: none;">
                                                <asp:RadioButtonList ID="rbtnlstMedExValues" TabIndex="7" Style="font-size: 8pt;"
                                                    runat="server" RepeatColumns="1">
                                                </asp:RadioButtonList>
                                            </asp:Panel>
                                            <asp:HiddenField ID="hdnMedExValues" runat="server" />
                                            <asp:TextBox ID="txtValue" TabIndex="7" runat="server" Style="display: inline;" Enabled="false"></asp:TextBox>
                                              <asp:TextBox ID="txtDate" TabIndex="7" runat="server" ReadOnly="true" Style="display: none;" onblur="standarddateBlur(this);" ></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalExtDate" runat="server" Format="dd-MMM-yyyy" 
                                                                            TargetControlID="txtDate" CssClass="MyCalendar">
                                                                        </cc1:CalendarExtender>
                                            <span id="spnAnd" style="display: none" class="Label">And</span>
                                            <asp:TextBox ID="txtValueBetween" TabIndex="7" runat="server" Style="display: none"></asp:TextBox>
                                            <%--<asp:ImageButton id="ImgDate" runat="server" style="display:none;" OnClientClick="popUpCalendar(this,ctl00_CPHLAMBDA_txtValue,'dd-mmm-yyyy');" alt="Select From Date" src="images/Calendar_scheduleHS.png" />--%>
                                          
                                            <asp:TextBox ID="txtDateBetween" TabIndex="7" runat="server" ReadOnly="true" Style="display: none;" onblur="standarddatebetweenBlur(this);" ></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy" 
                                                                            TargetControlID="txtDateBetween" CssClass="MyCalendar">
                                                                        </cc1:CalendarExtender>
                                            <asp:HiddenField ID="hdnMedExType" runat="server" />
                                            <asp:HiddenField ID="hdnDataType" runat="server" />
                                        </td>
                                        <td align="right"  valign="top">
                                            <asp:Button ID="btnOR" runat="server"  Text="OR" CssClass="btn btnnew"
                                                 OnClientClick="fnInsertOR();" TabIndex="8" Enabled="false" Style="margin-right: 1%;">
                                            </asp:Button>
                                            <asp:Button ID="btnAND" runat="server"  Text="And" CssClass="btn btnnew"
                                                 OnClientClick="fnInsertAND();" TabIndex="9" Enabled="false" Style="margin-right: 1%;">
                                            </asp:Button>
                                            <asp:Button ID="btnClear"  TabIndex="9" runat="server" Text="Clear All "
                                                 CssClass="btn btnnew" Style="float: right; margin-right: 4.5%;" OnClientClick="fnClear();" />
                                            <asp:Button ID="btnHdnMedExValues" runat="server" Text="And" CssClass="btn btnnew" Style="display: none;">
                                            </asp:Button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6" align="left">
                                            <asp:TextBox ID="txtFormula" runat="server" Rows="8" Width="98%" TextMode="MultiLine"
                                                Enabled="false" Style="overflow: auto; height: auto;"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <table width="100%">
                                                <tr>
                                                    <td class="Label" style="text-align: left; width: 13%" valign="top">
                                                        Discrepancy Message* :
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtMessage" runat="server" TabIndex="10" Rows="3" Width="98%" TextMode="MultiLine"></asp:TextBox>
                                                        <asp:HiddenField ID="hdnQueryValue" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <table width="100%">
                                                <tr>
                                                    <td class="Label" style="text-align: left; width: 12%" valign="top">
                                                    </td>
                                                    <td align="left">
                                                        <label id="lblSyntaxError" class="Label" style="text-transform: none !important;
                                                            font-size: 11.5px; color: Red; display: none;">
                                                        </label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr style="height: 5px;">
                        <td>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset id="fldsData" runat="server" class="FieldSetBox" style="width: 85%; margin: auto;
                margin-top: 10px;">
                <legend id="lblData" runat="server" class="LegendText" style="color: black; font-weight: bolder;
                    font-size: 12px; text-align: left;">
                    <img id="imgData" alt="SubjectSpecific" src="images/expand.jpg" onclick="displayProjectInfo(this,'divData');"
                        runat="server" />
                    Activities</legend>
                <div id="divData">
                    <table style="width: 100%; margin: auto;" id="tblAllActivity" runat="server">
                        <tr>
                            <td class="Label" align="left">
                                <asp:CheckBox ID="chkAllActivity" runat="server" Text="Attach To All Same Activities ?"
                                    AutoPostBack="true" TabIndex="11" />
                            </td>
                        </tr>
                        <tr style="height: 5px;">
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td runat="server" id="tdAllActivity">
                            </td>
                        </tr>
                    </table>
                </div>
            </fieldset>
            <table style="width: 85%; margin: auto;" id="tblButton" runat="server">
                <tr style="height: 10px;">
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <asp:Button ID="btnValidate" runat="server" Text="Validate" Visible="false" CssClass="btn btnnew"
                            OnClientClick="return ValidateSyntax();" />
                        <asp:Button ID="btnSave" TabIndex="12" runat="server" Text="Save" CssClass="btn btnsave"
                            OnClientClick="return fnSaveClick();" />
                        <asp:Button ID="btnUpdate" TabIndex="12" runat="server" Text="Update" Visible="false"
                            CssClass="btn btnsave" />
                        <asp:Button ID="btnCancel" TabIndex="13" runat="server" Text="Cancel" CssClass="btn btncancel" OnClientClick="fncancel()"/><%--onclientcliek added by --%>
                        <asp:HiddenField ID="hdnSaveNode" runat="server" />
                        <asp:HiddenField ID="hdnMedExCode" runat="server" />
                    </td>
                </tr>
            </table>
            <table width="90%" align="center" runat="server" id="tblData" runat="server">
                <tr style="height: 10px;">
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblNoData" runat="server" CssClass="Label" Text="No Edit Checks found for selected Activity !!!"
                            Visible="false" />
                        <asp:GridView ID="grdViewData" runat="server" SkinID="grdViewAutoSizeMax" AllowPaging="True"
                            PageSize="10" AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField HeaderText="#">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Period" DataField="iPeriod">
                                    <ItemStyle HorizontalAlign="Center" Width="8%" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Visit/Parent Activity" DataField="vParentNodeDisplayName">
                                    <ItemStyle HorizontalAlign="Center" Width="12%" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Activity" DataField="ActivityDisplayName">
                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Edit Check Formula" DataField="vQueryMessage">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="50%" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Discrepancy Message" DataField="vErrorMessage">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="top" Width="30%" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Type" DataField="cEditCheckType">
                                    <ItemStyle HorizontalAlign="Left" Width="13%" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="lnkEdit" runat="server" ImageUrl="~/images/Edit2.png" ToolTip="Edit Edit Check" />
                                        <asp:ImageButton ID="lnkDelete" runat="server" ImageUrl="~/Images/Delete.png" OnClientClick="return msgconfirmalert('Are you sure you want to delete it?',this)"
                                            ToolTip="Delete Edit Check" />
                                        <asp:ImageButton ID="lnkPreview" runat="server" ImageUrl="~/Images/Preview.png"
                                            ToolTip="View Edit Check" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="6%" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Edit Check No" DataField="nEditCheckNo" />
                                <asp:BoundField HeaderText="WorkspaceID" DataField="vWorkspaceId" />
                                <asp:BoundField HeaderText="Period" DataField="iPeriod" />
                                <asp:BoundField HeaderText="ActivityID" DataField="vActivityID" />
                                <asp:BoundField HeaderText="ParentNode" DataField="iParentNodeID" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            <cc1:ModalPopupExtender ID="mpDivAttribute" runat="server" TargetControlID="btnInsertAttribute"
                PopupControlID="divAttribute" CancelControlID="btnClose" BackgroundCssClass="modalBackground"
                BehaviorID="mpEdAttribute">
            </cc1:ModalPopupExtender>
            <div style="display: none; left: 391px; width: 95%; top: 528px; text-align: left"
                id="divAttribute" runat="server" class="centerModalPopup">
                <table width="100%" style="margin: 5px;">
                    <tr>
                        <td class="Label">
                            Attribute Selection
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 5px;">
                        </td>
                    </tr>
                    <tr>
                        <td id="tdselectedActivity" runat="server" class="Label">
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 5px;">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="99%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="font-weight: normal; font-size: 8pt;" align="left">
                                        Select Attribute then click Insert Or Double click on Attribute.
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 2px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <input id="txtlstAttributeSearch" type="text" style="width: 99.3%" tabindex="14" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 2px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:ListBox ID="lstAttribute" runat="server" TabIndex="15" Width="100%" Height="220px"
                                            Style="font-weight: normal;" onChange="fnSelectionInsertAttribute();" ondblclick="fnInsertAttributeDbClick();">
                                        </asp:ListBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <fieldset style="height: 90px; margin-top: 5px; width: 96%;">
                                <table width="100%" cellpadding="0" cellspacing="0" style="margin-top: 5px;">
                                    <tr>
                                        <td align="left" colspan="2" style="font-weight: bold; font-size: 9pt;">
                                            Your selected attribute has :
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="height: 10px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="font-size: 8pt; padding-right: 5px;">
                                            Type :
                                        </td>
                                        <td id="spnType" align="left" style="font-size: 8pt;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="height: 5px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="font-size: 8pt; padding-right: 5px;">
                                            Group Name :
                                        </td>
                                        <td id="spnGroup" align="left" style="font-size: 8pt;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="height: 5px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="font-size: 8pt; padding-right: 5px; width: 21%">
                                            Sub-Group Name :
                                        </td>
                                        <td id="spnSubGroup" align="left" style="font-size: 8pt; width: 79%">
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px;">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnInsert" runat="server" Text="Insert" TabIndex="16" CssClass="btn btnsave"
                                OnClientClick="fnInsertAttributeDbClick();" />
                            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btnclose" TabIndex="17" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnHdnMedExValues" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnUpdate" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnViewEditChecks" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="ddlPeriod" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlActivity" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="rdbEditChecksType" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="grdViewData" EventName="RowCommand" />
            <asp:PostBackTrigger ControlID="btnExportToExcel" />
        </Triggers>
    </asp:UpdatePanel>
     <asp:UpdateProgress ID="updateProgress" runat="server">
        <ProgressTemplate>
            <div id="Div1" runat="server" style=" display:none; position: absolute; top: 0px; left: 0px;  width: 1349px; height: 751px; background-color: rgba(0,0,0,0.6)">
                <div id="updateProgress" class="updateProgress" style="vertical-align: central !Important; position:relative !Important; left:550px;">
                    <div align="center" style="background-color:#ecf6fc  ! important; border:1px solid #227199  !Important; z-index :99999; position :relative !Important; ">
                        <table>
                            <tr>
                                <td style="height: 130px; ">
                                    <font class="updateText">Please Wait...</font>
                                </td>
                                <td style="height: 130px">
                                    <div title="Wait" class="updateImage">
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <script type="text/javascript">


        //var ConstQueryString = "(select CASE WHEN UPPER(ISNULL(vMedextYPE,'')) = 'STANDARDDATE' THEN ISNULL(RIGHT(vMedexResult,4),'') + ISNULL(substring(vMedexResult,3,2),'') + ISNULL(substring(vMedexResult,1,2),'') ELSE " +
        //                     " (case when isdate(ISNULL(vMedexResult,'')) = 1 then cast(ISNULL(vMedexResult,'') as smalldatetime)  else CASE WHEN ISNULL(vMedexResult,'') = '' THEN '--' ELSE ISNULL(vMedexResult,'') END end) END " +
        //                     " from CRFHdr inner Join CRFDtl on(CRFHDR.nCRFHdrNo=CRFDtl.nCRFHdrNo " +
		//	                 " And CRFDtl.cStatusIndi <> 'D' ) inner join CRFSubDtl on (CRFDtl.nCRFDtlNo=CRFSubDtl.nCRFDtlNo " +
		//	                 " and CRFSubDtl.cStatusIndi <> 'D' ) inner join(select nCRFDtlNo,vMedExCode,max(iTranNo) as MaxTranNo " +
		//	                 " from CRFSubDtl where CRFSubDtl.cStatusIndi <> 'D' group by nCRFDtlNo,vMedExCode)CRFSubDtlMax " +
		//	                 " on(CRFSubDtl.vMedExCode= CRFSubDtlMax.vMedExCode And CRFSubDtl.iTranNo= CRFSubDtlMax.MaxTranNo " +
		//	                 " And CRFSubDtl.nCRFDtlNo = CRFSubDtlMax.nCRFDtlNo) INNER JOIN WORKSPACEMST ON WORKSPACEMST.VWORKSPACEID = CRFHDR.VWORKSPACEID " +
        //                     " INNER JOIN MEDEXWORKSPACEHDR ON (( MEDEXWORKSPACEHDR.VWORKSPACEID = WORKSPACEMST.vParentWorkspaceId ) or (MEDEXWORKSPACEHDR.VWORKSPACEID = '@@')) AND MEDEXWORKSPACEHDR.iNodeId = CRFHDR.iNodeId " +
        //                     " INNER JOIN MEDEXWORKSPACEDTL ON MEDEXWORKSPACEDTL.nMedExWorkSpaceHdrNo =  MEDEXWORKSPACEHDR.nMedExWorkSpaceHdrNo  AND MEDEXWORKSPACEDTL.vMedExCode = CRFSubDtl.vMedExCode " +
        //                     " where CRFHdr.cStatusIndi <> 'D' And " +
		//	                 " CRFHdr.vWorkSpaceId= '@@' And CRFHdr.iNodeId in ([iNodeId]) And CRFDtl.vSubjectId= '##' " +
		//	                 " And CRFDtl.iRepeatNo= '++' And CRFSubDtl.vMedExCode='[vMedExCode]' )", 
                             
                             SelectedActivity = "", iFlag = 0,
			                 CrossFlag = false;
                             var DataTypeForInsertvalue  = ""
                                
        $(document).ready(function () {

            $('#ctl00_CPHLAMBDA_txtValue').blur(function () {
                var txtFormula = document.getElementById('ctl00_CPHLAMBDA_txtFormula');
                var hdnMedExType = document.getElementById('ctl00_CPHLAMBDA_hdnMedExType').value
                var hdnQueryValue = document.getElementById('ctl00_CPHLAMBDA_hdnQueryValue');
                if (this.value != "") {
                    txtFormula.value = IsNumeric(this.value.trim()) ? txtFormula.value + "  " + this.value.trim() : txtFormula.value + "  '" + this.value.trim() + "'"

                        hdnQueryValue.value = IsNumeric(this.value.trim()) ? hdnQueryValue.value + " " + this.value.trim() : hdnQueryValue.value + " '" + this.value.trim() + "'"
                }
                this.value = '';
            }).keypress(function (event) { if (event.which == 13) { $('#ctl00_CPHLAMBDA_txtValue').blur(); return false; }; });

           


            $('#ctl00_CPHLAMBDA_txtValueBetween').blur(function () {
                var txtFormula = document.getElementById('ctl00_CPHLAMBDA_txtFormula');
                var hdnQueryValue = document.getElementById('ctl00_CPHLAMBDA_hdnQueryValue');
                if (this.value != "") {
                    txtFormula.value = txtFormula.value + " And  '" + this.value.trim() + "'";
                    hdnQueryValue.value = hdnQueryValue.value + " And '" + this.value.trim() + "'";
                }
                this.value = '';
                document.getElementById('ctl00_CPHLAMBDA_txtValueBetween').style.display = "none";
                document.getElementById('spnAnd').style.display = 'none';
            });

            $('#txtlstAttributeSearch').keypress(function (event) {
                if (event.which == 13) {
                    return false;
                }
            });

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            function EndRequestHandler(sender, args) {
                iFlag = 0;
                if (CrossFlag == true) {
                    iFlag = 1;
                }
                $("#ctl00_CPHLAMBDA_lstAttribute").val("-1");

                fnListBoxSearch();

                $('#ctl00_CPHLAMBDA_txtValue').blur(function () {
                    var txtFormula = document.getElementById('ctl00_CPHLAMBDA_txtFormula');
                    var hdnQueryValue = document.getElementById('ctl00_CPHLAMBDA_hdnQueryValue');
                    
                    if (this.value != "") {
                   //     txtFormula.value = txtFormula.value + "  '" + this.value.trim() + "'";
                        //     hdnQueryValue.value = hdnQueryValue.value + " '" + this.value.trim() + "'";
                        if (DataTypeForInsertvalue.split(",")[0].trim() == "IN" || DataTypeForInsertvalue.split(",")[0].trim() == "NU") {
                            txtFormula.value = IsNumeric(this.value.trim()) ? txtFormula.value + "  " + this.value.trim() : txtFormula.value + "  '" + this.value.trim() + "'"

                            hdnQueryValue.value = IsNumeric(this.value.trim()) ? hdnQueryValue.value + " cast ( ' " + this.value.trim()  + "' as numeric (18,2) )" : hdnQueryValue.value + "cast( '" + this.value.trim() + "' as numeric (18,2) )"

                        } else {
                            txtFormula.value = IsNumeric(this.value.trim()) ? txtFormula.value + "  " + this.value.trim() : txtFormula.value + "  '" + this.value.trim() + "'"
                            hdnQueryValue.value = IsNumeric(this.value.trim()) ? hdnQueryValue.value + " " + this.value.trim() : hdnQueryValue.value + " '" + this.value.trim() + "'"
                        }
                        
                    }
                    this.value = '';
                }).keypress(function (event) { if (event.which == 13) { $('#ctl00_CPHLAMBDA_txtValue').blur(); return false; }; });

                $('#ctl00_CPHLAMBDA_txtValueBetween').blur(function () {
                    var txtFormula = document.getElementById('ctl00_CPHLAMBDA_txtFormula');
                    var hdnQueryValue = document.getElementById('ctl00_CPHLAMBDA_hdnQueryValue');
                    if (this.value != "") {
                        if (DataTypeForInsertvalue.split(",")[0].trim() == "IN" || DataTypeForInsertvalue.split(",")[0].trim() == "NU") {
                            txtFormula.value = txtFormula.value + " And '" + this.value.trim() + "'";
                            hdnQueryValue.value = hdnQueryValue.value + " And cast('" + this.value.trim() + "' as numeric (18,2) )";
                        }
                        else {

                            txtFormula.value = txtFormula.value + " And '" + this.value.trim() + "'";
                            hdnQueryValue.value = hdnQueryValue.value + " And '" + this.value.trim() + "'";
                        }
                    }
                    this.value = '';
                    document.getElementById('ctl00_CPHLAMBDA_txtValueBetween').style.display = "none";
                    document.getElementById('spnAnd').style.display = 'none';
                });

                $('#txtlstAttributeSearch').keypress(function (event) {
                    if (event.which == 13) {
                        return false;
                    }
                });

            }
        });

        function fnActivityChange() {
            //            if (iFlag == 0) {
            //                SelectedActivity = $('#<%= ddlActivity.ClientID %> option:selected').val();
            //            }
            //            iFlag = 1;
        }

        function IsNumeric(input) {
            var RE = /^-{0,1}\d*\.{0,1}\d+$/;
            return (RE.test(input));
        }

        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
            iFlag = 0;
        }

        function fnListBoxSearch() {
            var win;
            var keys = [];
            var values = [];



            function DoListBoxFilter(listBoxSelector, filter, keys, values) {
                var list = $(listBoxSelector);
                var selectBase = '<option value="{0}">{1}</option>';

                list.empty();
                for (i = 0; i < values.length; ++i) {
                    var value = values[i];

                    if (value == "" || value.toLowerCase().indexOf(filter.toLowerCase()) >= 0) {
                        var temp = '<option value="' + keys[i] + '">' + value + '</option>';
                        list.append(temp);
                    }
                }
            }

            var options = $('#ctl00_CPHLAMBDA_lstAttribute option');
            $.each(options, function (index, item) {
                keys.push(item.value);
                values.push(item.innerHTML);
            });
            $('#txtlstAttributeSearch').keyup(function () {
                var filter = $(this).val();
                DoListBoxFilter('#ctl00_CPHLAMBDA_lstAttribute', filter, keys, values);
            });

            var watermark = 'Enter text to Search Attribute...';
            $('#txtlstAttributeSearch').blur(function () {
                if ($(this).val().length == 0)
                    $(this).val(watermark).addClass('watermark');
            }).focus(function () {
                if ($(this).val() == watermark)
                    $(this).val('').removeClass('watermark');
            }).val(watermark).addClass('watermark');
        }

        function fnSelectionInsertAttribute() {
            var selectedItem = $("#ctl00_CPHLAMBDA_lstAttribute option:selected").val();
            document.getElementById('spnType').innerText = selectedItem.split('#')[2] != undefined ? selectedItem.split('#')[2] : '';
            document.getElementById('spnGroup').innerText = selectedItem.split('#')[5] != undefined ? selectedItem.split('#')[5] : '';
            document.getElementById('spnSubGroup').innerText = selectedItem.split('#')[4] != undefined ? selectedItem.split('#')[4] : '';
 
        }

        //Added By Vivek Patel
        function fncancel() {
            MedExCodeCount = 1
        }

        //Added By Vivek Patel
        var MedExCodeCount = 1

        function fnInsertAttributeDbClick() {

            //Added By Vivek Patel
            if (MedExCodeCount == 1) {
                var hdnMedExCode = null
                hdnMedExCode = $("#ctl00_CPHLAMBDA_lstAttribute option:selected").val();
                hdnMedExCode = hdnMedExCode.split("#")[0]
                MedExCodeCount = MedExCodeCount + 1;
                //var hdnMedExCodeValue = document.getElementById('ctl00_CPHLAMBDA_hdnMedExCode');
                document.getElementById('ctl00_CPHLAMBDA_hdnMedExCode').value = null;
                document.getElementById('ctl00_CPHLAMBDA_hdnMedExCode').value = hdnMedExCode;
            }

            var selectedItem1 = $("#ctl00_CPHLAMBDA_lstAttribute option:selected").val();
            var ConstQueryString
            var hdnMedExType = selectedItem1.split("#")[2]
            $("#hdnMedExType").val("hdnMedExType");

            var DataType = selectedItem1.split("#")[7]
            DataTypeForInsertvalue = selectedItem1.split("#")[7]
            $("#hdnDataType").val(DataType)

            if (hdnMedExType == "STANDARDDATE" || hdnMedExType == "DateTime" || hdnMedExType == "ASYNCDATETIME") {
                ConstQueryString = "(select CASE WHEN UPPER(ISNULL(vMedextYPE,'')) = 'STANDARDDATE'  THEN  CAST(ISNULL(RIGHT(vMedexResult,4),'') + ISNULL(substring(vMedexResult,3,2),'') + ISNULL(substring(vMedexResult,1,2),'') as Smalldatetime)  ELSE " +
                                " (case when isdate(ISNULL(vMedexResult,'')) = 1 then cast(ISNULL(vMedexResult,'') as smalldatetime)  else CASE WHEN ISNULL(vMedexResult,'') = '' THEN '' ELSE ISNULL(vMedexResult,'') END end) END " +
                                " from CRFHdr inner Join CRFDtl on(CRFHDR.nCRFHdrNo=CRFDtl.nCRFHdrNo " +
                                " And CRFDtl.cStatusIndi <> 'D' ) inner join CRFSubDtl on (CRFDtl.nCRFDtlNo=CRFSubDtl.nCRFDtlNo " +
                                " and CRFSubDtl.cStatusIndi <> 'D' ) inner join(select nCRFDtlNo,vMedExCode,max(iTranNo) as MaxTranNo " +
                                " from CRFSubDtl where CRFSubDtl.cStatusIndi <> 'D' group by nCRFDtlNo,vMedExCode)CRFSubDtlMax " +
                                " on(CRFSubDtl.vMedExCode= CRFSubDtlMax.vMedExCode And CRFSubDtl.iTranNo= CRFSubDtlMax.MaxTranNo " +
                                " And CRFSubDtl.nCRFDtlNo = CRFSubDtlMax.nCRFDtlNo) INNER JOIN WORKSPACEMST ON WORKSPACEMST.VWORKSPACEID = CRFHDR.VWORKSPACEID " +
                                " INNER JOIN MEDEXWORKSPACEHDR ON (( MEDEXWORKSPACEHDR.VWORKSPACEID = WORKSPACEMST.vParentWorkspaceId ) or (MEDEXWORKSPACEHDR.VWORKSPACEID = '@@')) AND MEDEXWORKSPACEHDR.iNodeId = CRFHDR.iNodeId " +
                                " INNER JOIN MEDEXWORKSPACEDTL ON MEDEXWORKSPACEDTL.nMedExWorkSpaceHdrNo =  MEDEXWORKSPACEHDR.nMedExWorkSpaceHdrNo  AND MEDEXWORKSPACEDTL.vMedExCode = CRFSubDtl.vMedExCode  AND  MEDEXWORKSPACEDTL.cActiveFlag  <> 'N'  " +
                                " INNER JOIN ActivityMst on  ActivityMst.vActivityId = MEDEXWORKSPACEHDR.vActivityId " +
                                " where CRFHdr.cStatusIndi <> 'D' And " +
                                " CRFHdr.vWorkSpaceId= '@@' And CRFHdr.iNodeId in ([iNodeId]) And CRFDtl.vSubjectId= '##' " +
                                " AND CRFDtl.iRepeatNo  = CASE  ActivityMst.cIsRepeatable  WHEN  'Y'  THEN  '++' Else 1 END  " +
                                " And CRFSubDtl.vMedExCode='[vMedExCode]' )";

            }
            else if (DataType.split(",")[0] == "NU" || DataType.split(",")[0] == "IN")
            {
                ConstQueryString = "(select  " +
                               " case when ISNUMERIC(ISNULL(vMedexResult,0)) > 0 then CASE WHEN ISNULL(vMedexResult,0) = 0 then 1 else cast(ISNULL(vMedexResult,0) as numeric (18,2)) End else  CASE WHEN ISNULL(vMedexResult,0) = 0 THEN 0 ELSE ISNULL(vMedexResult,0) END end " +
                               //"  case when ISNUMERIC(ISNULL(vMedexResult,0)) > 0 then cast(ISNULL(vMedexResult,0) as numeric (18,2))  else CASE WHEN ISNULL(vMedexResult,0) = 0 THEN 0 ELSE ISNULL(vMedexResult,0) END end " +
                               " from CRFHdr inner Join CRFDtl on(CRFHDR.nCRFHdrNo=CRFDtl.nCRFHdrNo " +
                               " And CRFDtl.cStatusIndi <> 'D' ) inner join CRFSubDtl on (CRFDtl.nCRFDtlNo=CRFSubDtl.nCRFDtlNo " +
                               " and CRFSubDtl.cStatusIndi <> 'D' ) inner join(select nCRFDtlNo,vMedExCode,max(iTranNo) as MaxTranNo " +
                               " from CRFSubDtl where CRFSubDtl.cStatusIndi <> 'D' group by nCRFDtlNo,vMedExCode)CRFSubDtlMax " +
                               " on(CRFSubDtl.vMedExCode= CRFSubDtlMax.vMedExCode And CRFSubDtl.iTranNo= CRFSubDtlMax.MaxTranNo " +
                               " And CRFSubDtl.nCRFDtlNo = CRFSubDtlMax.nCRFDtlNo) INNER JOIN WORKSPACEMST ON WORKSPACEMST.VWORKSPACEID = CRFHDR.VWORKSPACEID " +
                               " INNER JOIN MEDEXWORKSPACEHDR ON (( MEDEXWORKSPACEHDR.VWORKSPACEID = WORKSPACEMST.vParentWorkspaceId ) or (MEDEXWORKSPACEHDR.VWORKSPACEID = '@@')) AND MEDEXWORKSPACEHDR.iNodeId = CRFHDR.iNodeId " +
                               " INNER JOIN MEDEXWORKSPACEDTL ON MEDEXWORKSPACEDTL.nMedExWorkSpaceHdrNo =  MEDEXWORKSPACEHDR.nMedExWorkSpaceHdrNo  AND MEDEXWORKSPACEDTL.vMedExCode = CRFSubDtl.vMedExCode  AND  MEDEXWORKSPACEDTL.cActiveFlag  <> 'N'  " +
                               " INNER JOIN ActivityMst on  ActivityMst.vActivityId = MEDEXWORKSPACEHDR.vActivityId " +
                               " where CRFHdr.cStatusIndi <> 'D' And " +
                               " CRFHdr.vWorkSpaceId= '@@' And CRFHdr.iNodeId in ([iNodeId]) And CRFDtl.vSubjectId= '##' " +
                               " AND CRFDtl.iRepeatNo  = CASE  ActivityMst.cIsRepeatable  WHEN  'Y'  THEN  '++' Else 1 END  " +
                               " And CRFSubDtl.vMedExCode='[vMedExCode]' )";

            }
            else {
                ConstQueryString = "(select CASE WHEN UPPER(ISNULL(vMedextYPE,'')) = 'STANDARDDATE' THEN ISNULL(RIGHT(vMedexResult,4),'') + ISNULL(substring(vMedexResult,3,2),'') + ISNULL(substring(vMedexResult,1,2),'') ELSE " +
                             " (case when isdate(ISNULL(vMedexResult,'')) = 1 then cast(ISNULL(vMedexResult,'') as varchar(MAX))  else CASE WHEN ISNULL(vMedexResult,'') = '' THEN '' ELSE ISNULL(vMedexResult,'') END end) END " +
                             " from CRFHdr inner Join CRFDtl on(CRFHDR.nCRFHdrNo=CRFDtl.nCRFHdrNo " +
                             " And CRFDtl.cStatusIndi <> 'D' ) inner join CRFSubDtl on (CRFDtl.nCRFDtlNo=CRFSubDtl.nCRFDtlNo " +
                             " and CRFSubDtl.cStatusIndi <> 'D' ) inner join(select nCRFDtlNo,vMedExCode,max(iTranNo) as MaxTranNo " +
                             " from CRFSubDtl where CRFSubDtl.cStatusIndi <> 'D' group by nCRFDtlNo,vMedExCode)CRFSubDtlMax " +
                             " on(CRFSubDtl.vMedExCode= CRFSubDtlMax.vMedExCode And CRFSubDtl.iTranNo= CRFSubDtlMax.MaxTranNo " +
                             " And CRFSubDtl.nCRFDtlNo = CRFSubDtlMax.nCRFDtlNo) INNER JOIN WORKSPACEMST ON WORKSPACEMST.VWORKSPACEID = CRFHDR.VWORKSPACEID " +
                             " INNER JOIN MEDEXWORKSPACEHDR ON (( MEDEXWORKSPACEHDR.VWORKSPACEID = WORKSPACEMST.vParentWorkspaceId ) or (MEDEXWORKSPACEHDR.VWORKSPACEID = '@@')) AND MEDEXWORKSPACEHDR.iNodeId = CRFHDR.iNodeId " +
                             " INNER JOIN MEDEXWORKSPACEDTL ON MEDEXWORKSPACEDTL.nMedExWorkSpaceHdrNo =  MEDEXWORKSPACEHDR.nMedExWorkSpaceHdrNo  AND MEDEXWORKSPACEDTL.vMedExCode = CRFSubDtl.vMedExCode AND  MEDEXWORKSPACEDTL.cActiveFlag  <> 'N'   " +
                             " INNER JOIN ActivityMst on  ActivityMst.vActivityId = MEDEXWORKSPACEHDR.vActivityId " +
                             " where CRFHdr.cStatusIndi <> 'D' And " +
                             " CRFHdr.vWorkSpaceId= '@@' And CRFHdr.iNodeId in ([iNodeId]) And CRFDtl.vSubjectId= '##' " +
                             " AND CRFDtl.iRepeatNo  = CASE  ActivityMst.cIsRepeatable  WHEN  'Y'  THEN  '++' Else 1 END  " +
                             "And CRFSubDtl.vMedExCode='[vMedExCode]' )";
            }


            document.getElementById('ctl00_CPHLAMBDA_Div1').style.display = 'inline';
            var ConcatString = '';
            var selectedItem = $("#ctl00_CPHLAMBDA_lstAttribute option:selected").val();
            var selectedNodeItem = $("#ctl00_CPHLAMBDA_ddlActivity option:selected").val() + "#" + $("#ctl00_CPHLAMBDA_ddlPeriod option:selected").val();
            
            var txtFormula = document.getElementById('ctl00_CPHLAMBDA_txtFormula');
            var hdnNodeID = document.getElementById('ctl00_CPHLAMBDA_hdnNodeID');
            var hdnMedExType = document.getElementById('ctl00_CPHLAMBDA_hdnMedExType');
            var hdnCrossNodes = document.getElementById('ctl00_CPHLAMBDA_hdnCrossNodes');
            var hdnQueryValue = document.getElementById('ctl00_CPHLAMBDA_hdnQueryValue');
            hdnMedExType.value = selectedItem.split('#')[2].toUpperCase();
            
            var e = document.getElementById("ctl00_CPHLAMBDA_ddlActivity");
            var strUser = e.options[e.selectedIndex].text;

            txtFormula.value = txtFormula.value + "  [" + selectedItem.split('#')[1] + "] ";

            document.getElementById('ctl00_CPHLAMBDA_hdnMedExValues').value = selectedItem.split('#')[6];
            ConcatString = ConstQueryString.replace(/\[vWorkSpaceId]/g, document.getElementById('ctl00_CPHLAMBDA_HProjectId').value);
            ConcatString = ConcatString.replace(/\[iNodeId]/g, selectedNodeItem.split('#')[0]).replace(/\[vMedExCode]/g, selectedItem.split('#')[0]);
            hdnQueryValue.value = hdnQueryValue.value + " " + ConcatString;
            hdnCrossNodes.value = hdnCrossNodes.value + selectedNodeItem + ",";


            if (iFlag == 0) {
                SelectedActivity = $('#<%= ddlActivity.ClientID %> option:selected').val() + "#" + $('#<%= ddlPeriod.ClientID %> option:selected').val();
            }
            iFlag = 1;

            if (hdnNodeID.value == "") {
                hdnNodeID.value = selectedNodeItem;
            }
            if (selectedNodeItem != SelectedActivity) {
                $('#ctl00_CPHLAMBDA_rdbEditChecksType_1').attr('checked', true);
                $('#<%= chkAllActivity.ClientID %>').val("Attach To All Same Periods ?");
            }
            $('#ctl00_CPHLAMBDA_btnHdnMedExValues').click();
            $('#ctl00_CPHLAMBDA_btnClose').click();
            

        }

        function fnInsertOperator(ctrl) {

            var selectedItem = $("#ctl00_CPHLAMBDA_ddlOperator option:selected").val()
            var txtFormula = document.getElementById('ctl00_CPHLAMBDA_txtFormula');
            var hdnQueryValue = document.getElementById('ctl00_CPHLAMBDA_hdnQueryValue');
            txtFormula.value = txtFormula.value + "  " + $("#ctl00_CPHLAMBDA_ddlOperator option:selected").text();

            if ((DataTypeForInsertvalue.split(",")[0] == "NU" || DataTypeForInsertvalue.split(",")[0] == "IN") && (selectedItem == "!= '' ") ) {
                hdnQueryValue.value = hdnQueryValue.value + " " + " != 0";
            }
            else if ((DataTypeForInsertvalue.split(",")[0] == "NU" || DataTypeForInsertvalue.split(",")[0] == "IN") && (selectedItem == "= '' ")) {
                hdnQueryValue.value = hdnQueryValue.value + " " + " = 0";
            }
            else {
                hdnQueryValue.value = hdnQueryValue.value + " " + selectedItem;
            }
            ctrl.selectedIndex = 0;

            CrossFlag = true; iFlag = 1;

            if (selectedItem == 'Between' || selectedItem == 'Not Between') {
                document.getElementById('spnAnd').style.display = 'inline';
                if (document.getElementById('ctl00_CPHLAMBDA_txtValue').style.display == 'none') {
                    document.getElementById('ctl00_CPHLAMBDA_txtValueBetween').style.display = 'none';
                    document.getElementById('ctl00_CPHLAMBDA_txtDateBetween').style.display = 'inline';
                }
                else {
                    document.getElementById('ctl00_CPHLAMBDA_txtValueBetween').style.display = 'inline';
                    document.getElementById('ctl00_CPHLAMBDA_txtDateBetween').style.display = 'none';
                }
            }

        }

        function fnInsertAND() {
            var txtFormula = document.getElementById('ctl00_CPHLAMBDA_txtFormula');
            var hdnQueryValue = document.getElementById('ctl00_CPHLAMBDA_hdnQueryValue');
            txtFormula.value = txtFormula.value + "  " + ' And';
            hdnQueryValue.value = hdnQueryValue.value + " " + ' And';
            CrossFlag = true;
        }

        function fnInsertOR() {
            var txtFormula = document.getElementById('ctl00_CPHLAMBDA_txtFormula');
            var hdnQueryValue = document.getElementById('ctl00_CPHLAMBDA_hdnQueryValue');
            txtFormula.value = txtFormula.value + "  " + ' OR';
            hdnQueryValue.value = hdnQueryValue.value + " " + ' OR';
            CrossFlag = true;
        }

        function fnInsertRadioValue(ClickedValue) {
            var txtFormula = document.getElementById('ctl00_CPHLAMBDA_txtFormula');
            var hdnQueryValue = document.getElementById('ctl00_CPHLAMBDA_hdnQueryValue');
            txtFormula.value = txtFormula.value + "  '" + ClickedValue + "'";
            hdnQueryValue.value = hdnQueryValue.value + " '" + ClickedValue + "'";
        }

        function fnSelectAllPeriod(Ctrl) {
            $($(Ctrl).parents('table')[0]).find(':checkbox').attr('checked', Ctrl.checked);
        }

        function fnSaveClick() {
            var strhdnSaveNode = '';
            
            if ($('#<%= HProjectId.ClientId %>').val() === "") {
                msgalert("Please Enter Project Name !");
                return false;
            }
           else if ($('#ctl00_CPHLAMBDA_ddlPeriod option:selected').val().toLowerCase() === "select period") {
               msgalert("Please Select Period !");
                        return false;
            }
               
            else if ($('#ctl00_CPHLAMBDA_ddlVisit option:selected').val().toLowerCase() === "select visit/parent activity") {
                msgalert("Please Select Visit/Parent Activity !");
                        return false;
            }
            else if ($('#ctl00_CPHLAMBDA_ddlActivity option:selected').val().toLowerCase() == "select activity") {
                msgalert("Please Select Activity !");
                return false;
            }
            else if ($('#<%= txtFormula.ClientID %>').val() === "") {
                msgalert("Please Generate Edit Check Formula !");
                return false;
            }
            else if ($('#<%= txtMessage.ClientID %>').val() === "") {
                msgalert("Please Enter Discrepancy Message !");
                return false;
            }
            else if ($('#<%= hdnCrossNodes.ClientID %>').val() == "") {
                msgalert("Please Insert Attribute !");
                return false;
            }
            else if (!ValidateSyntax()) {
                msgalert("Please Enter Proper Formula !");
                return false;
            }
            else {
                var hdnSaveNode = $('#ctl00_CPHLAMBDA_hdnSaveNode');
                $("#ctl00_CPHLAMBDA_ddlPeriod option").each(function () {
                    if ($('#ctl00_CPHLAMBDA_Period_' + $(this).val())[0] != undefined) {
                        $('#ctl00_CPHLAMBDA_Period_' + $(this).val()).find(':checkbox:checked').parents('span').each(function () {
                            if ($(this).attr('nodeid') != undefined) {
                                strhdnSaveNode = strhdnSaveNode + $(this).attr('nodeid') + ',';
                            }
                        });
                    }
                });
                hdnSaveNode.val(strhdnSaveNode);
                iFlag = 0;
                CrossFlag = false; // added by prayag
                MedExCodeCount = 1 // Added By Vivek Patel
                return true;
            }
        }


        function ValidateSyntax() {
            var regExp = /([\w-\.''\]\[]+)(| {0,1000})(=|!=|>|<>|>=|<|<=|\+|\-|\*|\/)(| {0,1000})([\w-\.''\(\[\]\/\)]+)/;
            var ISNULLExp = /(Is NULL |Is NULL)(\()( {0,10000}[\w-\.''\]\[]+ {0,10000})(\))/;
            var ISNOTNULLExp = /(Is Not NULL  |Is Not NULL )(\()( {0,10000}[\w-\.''\]\[]+ {0,10000})(\))/;
            var Formula = $('#<%= txtFormula.ClientID %>').val();
            var lblSyntaxError = document.getElementById('lblSyntaxError');
            var ErrorMessage = '', boolFlag = true;
             
            if (Formula == '') {
                boolFlag = false;
                ErrorMessage = "Please enter formula...";

            }
            if (Formula.match('Is NULL ') == 'Is NULL ' && (Formula.match('And ') == null && Formula.match('OR ') == null) && !ISNULLExp.test(Formula)) {
                boolFlag = false;
                ErrorMessage = "Please enter proper formula...";
            }
            if (Formula.match('Is Not NULL ') == 'Is Not NULL ' && (Formula.match('And ') == null && Formula.match('OR ') == null) && !ISNOTNULLExp.test(Formula)) {
                boolFlag = false;
                ErrorMessage = "Please enter proper formula...";
            }
            if (Formula.match('Between ') == 'Between ') {
                return true;
            }
            //And Function
            if (/&\&/g.exec(Formula) == '&&') {
                for (var Cnt = 0; Cnt < Formula.split(/\&&/g).length; Cnt++) {
                    if (Formula.split(/\&&/g)[Cnt].match('ISBLANK') == 'ISBLANK' && !blankregExp.test(Formula)) {
                        boolFlag = false;
                        ErrorMessage = "Please enter proper formula...";
                    }
                    else if (Formula.split(/\&&/g)[Cnt].match('.CONTAINS') == '.CONTAINS' && !containregExp.test(Formula)) {
                        boolFlag = false;
                        ErrorMessage = "Please enter proper formula...";
                    }
                    else if (Formula.split(/\&&/g)[Cnt].match('.CONTAINS') != '.CONTAINS' && Formula.split(/\&&/g)[Cnt].match('ISBLANK') != 'ISBLANK' && !regExp.test(Formula.split(/\&&/g)[Cnt])) {
                        boolFlag = false;
                        ErrorMessage = "Please enter proper formula...";
                    }
                }
            }

            if (Formula.indexOf('NULL') < 0 && Formula.match('.CONTAINS') != '.CONTAINS' && Formula.match('ISBLANK') != 'ISBLANK' && !regExp.test(Formula)) {
                boolFlag = false;
                ErrorMessage = "Please enter proper formula...";
            }

            lblSyntaxError.innerText = '';
            lblSyntaxError.style.display = 'none';

            return boolFlag;
        }

        function fnViewClick() {
            if ($('#<%= btnViewEditChecks.ClientID %>').val() === 'View') {
                if ($('#<%= HProjectId.ClientId %>').val() === "") {
                    msgalert("Please Enter Project Name !");
                    return false;
                }

            }
            else {
                return true;
            }
        }

        function fnClear() {
            if (document.getElementById('ctl00_CPHLAMBDA_txtFormula') != undefined) {
                document.getElementById('ctl00_CPHLAMBDA_txtFormula').value = "";
            }
            document.getElementById('ctl00_CPHLAMBDA_hdnNodeID').value = "";
            document.getElementById('ctl00_CPHLAMBDA_hdnCrossNodes').value = "";
            if (document.getElementById('ctl00_CPHLAMBDA_hdnQueryValue') != undefined) {
                document.getElementById('ctl00_CPHLAMBDA_hdnQueryValue').value = "";
            }
            $('#ctl00_CPHLAMBDA_rdbEditChecksType_0').attr('checked', true);
            iFlag = 0;
            CrossFlag = false;
            MedExCodeCount = 1//Added By Vivek Patel
        }

        function fnSelectActivity(iActivityID) {
            $("#ctl00_CPHLAMBDA_ddlActivity option").each(function () {
                if ($(this).val().indexOf(iActivityID) >= 0) {
                    $(this).attr('selected', 'selected');
                }
            });
        }
        function displayProjectInfo(ele, parent) {
            if (ele.src.toString().toUpperCase().search("EXPAND") != -1) {
                $("#" + parent).slideToggle(400);
                ele.src = "images/collapse_blue.jpg";

            }
            else {
                $("#" + parent).slideToggle(400);
                ele.src = "images/expand.jpg";
            }
        }

        //$('#ctl00_CPHLAMBDA_txtDate').blur(function () {
          
        //}).keypress(function (event) { if (event.which == 13) { $('#ctl00_CPHLAMBDA_txtDate').blur(); return false; }; });

        function standarddateBlur(ele) {
            var txtFormula = document.getElementById('ctl00_CPHLAMBDA_txtFormula');
            var hdnMedExType = document.getElementById('ctl00_CPHLAMBDA_hdnMedExType').value

            var hdnQueryValue = document.getElementById('ctl00_CPHLAMBDA_hdnQueryValue');
            if (ele.value != "") {
                txtFormula.value = txtFormula.value + "  '" + ele.value.trim() + "'";
                if (hdnMedExType.toString().trim() == "STANDARDDATE" || hdnMedExType.toString().trim() == "DateTime" || hdnMedExType.toString().trim() == "ASYNCDATETIME") {
                    hdnQueryValue.value = hdnQueryValue.value + " cast( '" + ele.value.trim() + "' as smalldatetime)";
                }
                else {
                    hdnQueryValue.value = hdnQueryValue.value + " '" + ele.value.trim() + "'";
                }

            }
            ele.value = '';
        }

        function standarddatebetweenBlur(ele) {
            var txtFormula = document.getElementById('ctl00_CPHLAMBDA_txtFormula');
            var hdnQueryValue = document.getElementById('ctl00_CPHLAMBDA_hdnQueryValue');
            if (ele.value != "") {

                var ddl = $('#ddlOperator :selected').text();
                var hdnMedExType = document.getElementById('ctl00_CPHLAMBDA_hdnMedExType').value

                txtFormula.value = txtFormula.value + " And '" + ele.value.trim() + "'";
                if (hdnMedExType.toString().trim() == "STANDARDDATE" || hdnMedExType.toString().trim() == "DateTime" || hdnMedExType.toString().trim() == "ASYNCDATETIME") {
                    hdnQueryValue.value = hdnQueryValue.value + " AND  cast( '" + ele.value.trim() + "' as smalldatetime)";
                }
                else {
                    hdnQueryValue.value = hdnQueryValue.value + " AND  '" + ele.value.trim() + "'";
                }
            }
            ele.value = '';
            //document.getElementById('ctl00_CPHLAMBDA_txtDateBetween').style.display = "none";
            //document.getElementById('spnAnd').style.display = 'none';
        }



        function hide1() {
            document.getElementById('ctl00_CPHLAMBDA_Div1').style.display = 'none';
        }
        function CheckFormula() {
            var txtFormula = document.getElementById('ctl00_CPHLAMBDA_txtFormula');
            var FormulaValue = txtFormula.value;
            if (FormulaValue.toString().trim() != "") {
                var lastFiveChar = FormulaValue.substr(FormulaValue.length - 5);
                if (lastFiveChar == '  And' || lastFiveChar == '   OR' || lastFiveChar == 'nd  (' || lastFiveChar == 'OR  (' || (lastFiveChar.length == 2 && lastFiveChar == ' (')) {
                    $find("mpEdAttribute").show();
                    return false;
                }
                else {
                    msgalert('Please Enter Correct Formula !');
                    return false;

                }
            }
            else {
                $find("mpEdAttribute").show();
                return false;
            }
            return false;

        }
    </script>
</asp:Content>
