<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmMedExDependency.aspx.vb" Inherits="frmMedExDependency" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="script/autocomplete.js"></script>

    <script src="Script/General.js" language="javascript" type="text/javascript"></script>

    <asp:UpdatePanel ID="upSubject" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <table width="90%" cellpadding="5" style="margin: auto;">
                <tr>
                    <td colspan="2">
                        <fieldset id="fldgen" class="FieldSetBox" style="width: 100%; float: left;">
                            <legend id="lblProject" runat="server" class="LegendText" style="color: black; font-weight: bold; font-size: 12px; text-align: left;">
                                <img id="imgfldgen" alt="SubjectSpecific" src="images/panelcollapse.png" onclick="displayProjectInfo(this,'tblgen');"
                                    runat="server" />
                                Project Details</legend>
                            <div id="tblgen">
                                <table width="100%" cellpadding="3">
                                    <tr>
                                        <td class="Label" style="text-align: right; vertical-align: middle; width: 15%;">Project Name* :
                                        </td>
                                        <td style="text-align: left; width: 30%">
                                            <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" MaxLength="50" Width="100%"
                                                TabIndex="1"></asp:TextBox>
                                            <asp:Button ID="btnSetProject" OnClientClick="getData(this);" runat="server" Style="display: none" />
                                            <asp:HiddenField ID="HProjectId" runat="server" />
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                                                TargetControlID="txtProject" ServicePath="AutoComplete.asmx" OnClientShowing="ClientPopulated"
                                                OnClientItemSelected="OnSelected" MinimumPrefixLength="1" ServiceMethod="GetMyProjectCompletionList"
                                                CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1"
                                                CompletionListElementID="pnlProjectDependency">
                                            </cc1:AutoCompleteExtender>
                                            <asp:Panel ID="pnlProjectDependency" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden;" />
                                        </td>
                                        <td class="Label" style="text-align: right; vertical-align: middle; width: 15%">
                                            <asp:Label ID="lblPeriod" runat="server"></asp:Label>
                                        </td>
                                        <td style="text-align: left; width: 35%">
                                            <asp:DropDownList ID="ddlPeriod" TabIndex="2" runat="server" Width="80%" AutoPostBack="true"
                                                Style="display: none;" CssClass="dropDownList" />
                                            <asp:DropDownList ID="ddlVisit" runat="server" TabIndex="2" Width="80%" AutoPostBack="true"
                                                Style="display: none;" CssClass="dropDownList" />
                                        </td>

                                    </tr>
                                    <tr>
                                        <%--<td class="Label" style="text-align: right; vertical-align: middle;">
                                            <asp:Label ID="lblPeriod" runat="server"></asp:Label>
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="ddlPeriod" TabIndex="2" runat="server" Width="30%" AutoPostBack="true"
                                                Style="display: none;" CssClass="dropDownList" />
                                            <asp:DropDownList ID="ddlVisit" runat="server" TabIndex="2" Width="46%" AutoPostBack="true"
                                                Style="display: none;" CssClass="dropDownList" />
                                        </td>--%>
                                    </tr>
                                    <tr>
                                        <td class="Label" style="text-align: right; vertical-align: middle;">Activity* :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="ddlActivity" TabIndex="3" CssClass="dropDownList" runat="server"
                                                Width="100%" AutoPostBack="true" />
                                        </td>

                                        <td class="Label" style="text-align: right; vertical-align: middle;">Dependancy Type :
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="RadioButtonList1" TabIndex="4" runat="server" RepeatDirection="Horizontal" Enabled="false"
                                                AutoPostBack="true">
                                                <asp:ListItem Text="Within" Selected="True" Value="A" Enabled="false"></asp:ListItem>
                                                <asp:ListItem Text="Cross" Value="F" Enabled="false"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>

                                    </tr>
                                    <tr id="trRbl" runat="server" style="display: none;">
                                        <td class="Label" style="text-align: right; vertical-align: middle;">Select Dependency* :
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rblDependency" TabIndex="4" runat="server" RepeatDirection="Horizontal"
                                                AutoPostBack="true">
                                                <asp:ListItem Text="Activity Dependency&nbsp;&nbsp;" Value="A"></asp:ListItem>
                                                <asp:ListItem Text="Attribute Dependency" Value="F"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr id="tr1" runat="server">
                                        <%-- <td class="Label" style="text-align: right; vertical-align: middle;">
                                            Dependancy Type
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="RadioButtonList1" TabIndex="4" runat="server" RepeatDirection="Horizontal"
                                                AutoPostBack="true">
                                                <asp:ListItem Text="Within" Value="A"></asp:ListItem>
                                                <asp:ListItem Text="Cross" Value="F"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>--%>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="Label" style="text-align: center; vertical-align: middle;">
                                            <asp:Button ID="btnGo" TabIndex="5" runat="server" CssClass="btn btngo" Text="" ToolTip="Get Dependency Details"
                                                OnClientClick="return fnvalidateGo();" />
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" TabIndex="6" ToolTip="Cancel"
                                                CssClass="btn btncancel" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td style="width: 50%;">
                        <fieldset id="fldsActivity" runat="server" class="FieldSetBox" style="width: 100%; float: left; display: none;">
                            <legend id="lblActivity" runat="server" class="LegendText" style="color: black; font-weight: bold; font-size: 12px; text-align: left;">
                                <img id="imgfldsActivity" alt="SubjectSpecific" src="images/panelcollapse.png" onclick="displayProjectInfo(this,'tblAct');"
                                    runat="server" />
                                Source Attribute Details </legend>
                            <div id="tblAct">
                                <table width="100%" cellpadding="3">
                                    <tr>
                                        <td class="Label" style="text-align: right; vertical-align: middle; width: 30%">Select Source Attribute* :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="ddlActAttribute" TabIndex="7" runat="server" Width="45%" AutoPostBack="true"
                                                CssClass="dropDownList">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trAct" runat="server" visible="false">
                                        <td class="Label" style="text-align: right; vertical-align: middle; width: 30%;">Attribute Value :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtAct" runat="server" Visible="false" TabIndex="8" CssClass="textBox"></asp:TextBox>
                                            <fieldset id="fldAct" runat="server" class="FieldSetBox" style="min-width: 50%; max-width: 95%; display: none;">
                                                <asp:CheckBoxList ID="ChkAct" runat="server" CssClass="checkboxlist" TabIndex="8"
                                                    RepeatColumns="3">
                                                </asp:CheckBoxList>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <%--   <tr>
                                        <td class="Label" style="text-align: right; vertical-align: middle;">
                                            Select Target Activity* :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="ddlActActivty" runat="server" Width="45%" TabIndex="9" CssClass="dropDownList">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td nowrap="nowrap" colspan="2" style="text-align: center; vertical-align: top;">
                                            <asp:Button ID="btnActSave" runat="server" CssClass="btn btnsave" Text="Save" ToolTip="Save Activity Dependency"
                                                OnClientClick="return fnvalidatesave();" TabIndex="10" />
                                            <asp:Button ID="btnActCancel" runat="server" CssClass="btn btncancel" TabIndex="11" Text="Cancel"
                                                ToolTip="Cancel Activity Dependency" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                        <fieldset id="fldsAttribute" runat="server" class="FieldSetBox" style="width: 100%; float: left; display: none;">
                            <legend id="lblAttribute" runat="server" class="LegendText" style="color: black; font-weight: bolder; font-size: 12px; text-align: left;">
                                <img id="imgfldsAttribute" alt="SubjectSpecific" src="images/panelcollapse.png" onclick="displayProjectInfo(this,'tblAtt');"
                                    runat="server" />
                                Attribute Dependency</legend>
                            <div id="tblAtt">
                                <table width="100%" cellpadding="3">
                                    <tr>
                                        <td class="Label" style="text-align: right; vertical-align: middle; width: 30%;">Select Source Attribute* :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="ddlAttAttribute" runat="server" AutoPostBack="true" TabIndex="7"
                                                CssClass="dropDownList" Width="45%">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trAtt" runat="server" visible="false">
                                        <td class="Label" style="text-align: right; vertical-align: middle; width: 30%;">Attribute Value :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtAtt" runat="server" Visible="false" CssClass="textBox" TabIndex="8"></asp:TextBox>
                                            <fieldset id="FldAtt" runat="server" class="FieldSetBox" style="min-width: 50%; max-width: 95%; display: none;">
                                                <asp:CheckBoxList ID="ChkAtt" runat="server" Visible="false" RepeatColumns="3" TabIndex="8">
                                                </asp:CheckBoxList>
                                            </fieldset>
                                            <asp:DropDownList ID="ddlDate" runat="server" Style="display: none;" class="standarddate" onChange="CheckStandardDate(this,'ctl00_CPHLAMBDA_ddlDate','ctl00_CPHLAMBDA_ddlMonth','ctl00_CPHLAMBDA_ddlYear')"></asp:DropDownList>
                                            <asp:DropDownList ID="ddlMonth" runat="server" Style="display: none;" class="standarddate" onChange="CheckStandardDate(this,'ctl00_CPHLAMBDA_ddlDate' ,'ctl00_CPHLAMBDA_ddlMonth','ctl00_CPHLAMBDA_ddlYear')"></asp:DropDownList>
                                            <asp:DropDownList ID="ddlYear" runat="server" Style="display: none;" class="standarddate" onChange="CheckStandardDate(this,'ctl00_CPHLAMBDA_ddlDate','ctl00_CPHLAMBDA_ddlMonth' ,'ctl00_CPHLAMBDA_ddlYear')"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <%-- <tr>
                                        <td class="Label" style="text-align: right; vertical-align: middle;">
                                            Select Target Attribute* :
                                        </td>
                                        <td style="text-align: left;">
                                            <fieldset id="FldTrgAttribute" runat="server" class="FieldSetBox" style="min-width: 50%;
                                                max-width: 95%; display: none;">
                                                <asp:CheckBoxList ID="ChkAttTrgAttribute" runat="server" RepeatColumns="3" TabIndex="9">
                                                </asp:CheckBoxList>
                                            </fieldset>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td nowrap="nowrap" colspan="2" style="text-align: center; vertical-align: top;">
                                            <asp:Button ID="btnAttSave" runat="server" Text="Save" ToolTip="Save Attribute Dependency"
                                                CssClass="btn btnsave " OnClientClick="return fnvalidatesave();" TabIndex="10" />
                                            <asp:Button ID="btnAttCancel" runat="server" CssClass="btn btncancel" Text="Cancel" TabIndex="11"
                                                ToolTip="Cancel Attribute Dependency" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </td>

                    <td style="float: left; width: 90%; margin-left: 50px;">
                        <fieldset id="fldsTargetActivity" runat="server" class="FieldSetBox" style="width: 100%; float: left; display: none;">
                            <legend id="Legend1" runat="server" class="LegendText" style="color: black; font-weight: bold; font-size: 12px; text-align: left;">
                                <img id="img1" alt="SubjectSpecific" src="images/panelcollapse.png" onclick="displayProjectInfo(this,'tblTarget');"
                                    runat="server" />
                                Target Attribute Details </legend>
                            <div id="tblTarget">
                                <table>
                                    <tr>
                                        <td class="Label" style="text-align: right; vertical-align: middle; width: 40%;">Select Target Visit:
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:DropDownList runat="server" ID="ddlTargetVisit" AutoPostBack="true"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Label" style="text-align: right; vertical-align: middle; width: 40%;">Select Target Activity
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:DropDownList runat="server" ID="ddlTargetActivity" AutoPostBack="true"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Label" runat="server" id="TrTrgAttribute" style="text-align: right; display: none; vertical-align: middle;">Select Target Attribute* :
                                        </td>
                                        <td style="text-align: left;">
                                            <fieldset id="FldTrgAttribute" runat="server" class="FieldSetBox" style="min-width: 50%; max-width: 95%; display: none;">
                                                <asp:CheckBoxList ID="ChkAttTrgAttribute" runat="server" RepeatColumns="3" TabIndex="9">
                                                </asp:CheckBoxList>
                                            </fieldset>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>

                    </td>
                </tr>
                <tr>
                    <td>
                        <fieldset id="fldAttachedActivities" runat="server" class="FieldSetBox" style="width: 100%; float: left; display: none;">
                            <legend id="Legend2" runat="server" class="LegendText" style="color: black; font-weight: bold; font-size: 12px; text-align: left;">
                                <img id="img2" alt="SubjectSpecific" src="images/panelcollapse.png" onclick="displayProjectInfo(this,'tblAttached');"
                                    runat="server" />
                                Activities </legend>
                            <div id="tblAttached">
                                <table style="width: 100%; margin: auto;" id="tblAllActivity" runat="server">
                                    <tr>
                                        <td class="Label" align="left">
                                            <asp:CheckBox ID="chkAllActivity" runat="server" Text="Attach To All Same Activities ?"
                                                AutoPostBack="true" TabIndex="11" />
                                        </td>
                                    </tr>
                                    <tr style="height: 5px;">
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td runat="server" id="tdAllActivity"></td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <fieldset id="fldGrid" runat="server" class="FieldSetBox" style="width: 100%; float: left; display: none;">
                            <legend id="lblgrid" runat="server" class="LegendText" style="color: black; font-weight: bold; font-size: 12px; text-align: left;">
                                <img id="imgExpand" alt="SubjectSpecific" src="images/panelexpand.png" onclick="displayProjectInfo(this,'divGrid');"
                                    runat="server" />
                                Dependency Details</legend>
                            <div id="divGrid">
                                <asp:Button ID="btnExport" CssClass="btn btnexcel" runat="server" TabIndex="12" Visible="false"
                                    ToolTip="Export Grid Data" Style="margin-bottom: 2%;" />
                                <asp:GridView ID="gvwDependency" runat="server" AutoGenerateColumns="false" SkinID="grdViewSmlAutoSize"
                                    Width="100%" ShowFooter="true" AllowPaging="true" PageSize="10" TabIndex="13">
                                    <Columns>
                                        <asp:BoundField DataFormatString="number" HeaderText="Sr. No.">
                                            <ItemStyle HorizontalAlign="Center" Width="3%" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="3%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="nMedExDependcyNo" HeaderText="MedExDependcy No">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SourceMedexDesc" HeaderText="Source Attribute">
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="true" Width="10%" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="true" Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TargetMedexDesc" HeaderText="Target Attribute">
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="true" Width="10%" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="true" Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="vMedExValue" HeaderText="Attribute Value">
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="14%" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="14%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SourceActivityDesc" HeaderText="Source Activity">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TargetActivityDesc" HeaderText="Target Activity">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="cDependencyType" HeaderText="Dependency Type">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="7%" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="7%" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="DependencyType" HeaderText="Type">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="7%" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="7%" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="iModifyBy" HeaderText="Modify By">
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="10%" Wrap="true" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" Wrap="true" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="dModifyOn" HeaderText="Modify On" HtmlEncode="false">
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="10%" Wrap="true" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" Wrap="true" />
                                        </asp:BoundField>
                                        <asp:TemplateField SortExpression="status" HeaderText="Delete">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="lnkDelete" runat="server" ImageUrl="~/images/Delete.png" ToolTip="Delete"
                                                    TabIndex="12" OnClientClick="return msgconfirmalert('Are You Sure You Want To Delete This Dependency?',this);" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="7%" Wrap="true" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="7%" Wrap="true" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:HiddenField ID="hndLockStatus" runat="server" />
    <asp:HiddenField ID="hdnSaveNode" runat="server" />

    <script type="text/javascript" src="script/general.js"></script>

    <script type="text/javascript" language="javascript">

        function pageLoad() {
            //fnaddtitle();
        }

        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }
        function fnvalidatesave() {

            var strhdnSaveNode = '';


            if (document.getElementById('<%=HProjectId.ClientID%>').value.toString().trim() == "") {
                 document.getElementById('<%=txtProject.ClientID%>').value = '';
                 msgalert('Please enter Project !');
                 document.getElementById('<%=txtProject.ClientID%>').focus();
                 return false;
             }
             else if (document.getElementById('<%=ddlPeriod.ClientID %>').style.display != 'none') {
                 if (document.getElementById('<%=ddlPeriod.ClientID %>').selectedIndex == 0) {
                      msgalert('Please select Period !');
                      document.getElementById('<%=ddlPeriod.ClientID%>').focus();
                     return false;
                 }
             }
             else if (document.getElementById('<%=ddlVisit.ClientID %>').style.display != 'none') {
                  if (document.getElementById('<%=ddlVisit.ClientID %>').selectedIndex == 0) {
                       msgalert('Please select Visit !');
                       document.getElementById('<%=ddlVisit.ClientID%>').focus();
                     return false;
                 }
             }
             else if (document.getElementById('<%=ddlActivity.ClientID %>').selectedIndex == 0) {
                   msgalert('Please select Activity !');
                   document.getElementById('<%=ddlActivity.ClientID%>').focus();
                 return false;
             }



    if ($('#<%=rblDependency.ClientID %> input:checked').val() == "A") {
                 if (document.getElementById('<%=ddlActAttribute.ClientID %>').selectedIndex == 0) {
                    msgalert('Please select Source Attribute !');
                    $('#<%=rblDependency.ClientID %>').focus();
                     return false;
                 }
                 if ($('#<%=ChkAct.ClientID %>').length == 1) {
                    if ($('#<%=ChkAct.ClientID %> input:checked').length == 0) {
                          msgalert('Please select Attribute value !');
                          $('#<%=ChkAct.ClientID %>').focus();
                         return false;
                     }
                 }
                 if ($('#<%=txtAct.ClientID %>').length == 1) {
                    if ($('#<%=txtAct.ClientID %>').val() == "") {
                          msgalert('Please enter Attribute value !');
                          $('#<%=txtAct.ClientID %>').focus();
                          return false;
                      }

                 }

                  if (document.getElementById('<%=ddlTargetVisit.ClientID %>').selectedIndex == 0) {
                    msgalert('Please select Target Visit !');
                    document.getElementById('<%=ddlTargetVisit.ClientID%>').focus();
                    return false;
                  }

                if (document.getElementById('<%=ddlTargetActivity.ClientID %>').selectedIndex == 0 || document.getElementById('<%=ddlTargetActivity.ClientID %>').selectedIndex == -1) {
                    msgalert('Please select Target Activity !');
                    document.getElementById('<%=ddlTargetActivity.ClientID%>').focus();
                     return false;
                 }

             }
             else if ($('#<%=rblDependency.ClientID %> input:checked').val() == "F") {
                if (document.getElementById('<%=ddlAttAttribute.ClientID %>').selectedIndex == 0) {
                      msgalert('Please select Source Attribute !');
                      $('#<%=rblDependency.ClientID %>').focus();
                     return false;
                 }
                 if ($('#<%=ChkAtt.ClientID %>').length == 1) {
                      if ($('#<%=ChkAtt.ClientID %> input:checked').length == 0) {
                          msgalert('Please select Attribute value !');
                          $('#<%=ChkAtt.ClientID %>').focus();
                          return false;
                      }
                  }
                  if ($('#<%=txtAtt.ClientID %>').length == 1) {
                      if ($('#<%=txtAtt.ClientID %>').val() == "") {
                          msgalert('Please Enter Attribute value !');
                          $('#<%=txtAtt.ClientID %>').focus();
                          return false;
                      }
                  }

                 if (document.getElementById('<%=ddlTargetVisit.ClientID %>').selectedIndex == 0) {
                     msgalert('Please select Target Visit !');
                     document.getElementById('<%=ddlTargetVisit.ClientID%>').focus();
                    return false;
                }

                if (document.getElementById('<%=ddlTargetActivity.ClientID %>').selectedIndex == 0 || document.getElementById('<%=ddlTargetActivity.ClientID %>').selectedIndex == -1) {
                     msgalert('Please select Target Activity !');
                     document.getElementById('<%=ddlTargetActivity.ClientID%>').focus();
                    return false;
                }


                  if ($('#<%=ChkAttTrgAttribute.ClientID %> input:checked').length == 0) {
                      msgalert('Please select Target Attribute !');
                      $('#<%=ChkAttTrgAttribute.ClientID %>').focus();
                     return false;
                 }
                 if ($get('<%=ddlDate.ClientId%>').style.display != 'none') {
                      var flg = true;
                      $('.standarddate').each(function () {
                          if ($(this).val() == '') {
                              flg = false;
                          }
                      });
                      if (flg == false)
                          msgalert('Please select Attribute value !');
                      return flg;
                  }

              }
          var hdnSaveNode = $('#ctl00_CPHLAMBDA_hdnSaveNode');
          if ($('#ctl00_CPHLAMBDA_Period_1')[0] != undefined) {
              $('#ctl00_CPHLAMBDA_Period_1').find(':checkbox:checked').parents('span').each(function () {
                  if ($(this).attr('nodeid') != undefined) {
                      strhdnSaveNode = strhdnSaveNode + $(this).attr('nodeid') + ',';
                  }
              });
          }
          hdnSaveNode.val(strhdnSaveNode);
          return true;
      }
      function fnvalidateGo() {
          if (document.getElementById('<%=HProjectId.ClientID%>').value.toString().trim() == "") {
             document.getElementById('<%=txtProject.ClientID%>').value = '';
             msgalert('Please Enter Project !');
             document.getElementById('<%=txtProject.ClientID%>').focus();
                   return false;
               }
               return true;
           }
           function displayProjectInfo(ele, parent) {
               if (ele.src.toString().toUpperCase().search("EXPAND") != -1) {
                   $("#" + parent).slideToggle(400);
                   ele.src = "images/panelcollapse.png";

               }
               else {
                   $("#" + parent).slideToggle(400);
                   ele.src = "images/panelexpand.png";
               }
           }

           //Add by shivani pandya for project lock
           function getData(e) {
               var WorkspaceID = $('input[id$=HProjectId]').val();
               $.ajax({
                   type: "post",
                   url: "frmMedExDependency.aspx/LockImpact",
                   data: '{"WorkspaceID":"' + WorkspaceID + '"}',
                   contentType: "application/json; charset=utf-8",
                   datatype: JSON,
                   async: false,
                   dataType: "json",
                   success: function (data) {
                       if (data.d == "L") {
                           msgalert("Project is locked !");
                           $("#<%=hndLockStatus.ClientID%>").val("Lock");
                      }
                      if (data.d == "U") {
                          $("#<%=hndLockStatus.ClientID%>").val("UnLock");
                   }
                  },
                  failure: function (response) {
                      msgalert(response.d);
                  },
                  error: function (response) {
                      msgalert(response.d);
                  }
              });
           return true;
       }

       function fnSelectAllPeriod(Ctrl) {
           $($(Ctrl).parents('table')[0]).find(':checkbox').attr('checked', Ctrl.checked);
       }


    </script>

</asp:Content>
