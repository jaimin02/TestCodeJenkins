<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmPKSampleCollection_NEW, App_Web_xjkmyygy" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <asp:UpdatePanel runat="server" ID="UpControls" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button runat="server" ID="btndefault" Style="display: none" OnClientClick="return PassTheBarcode();" />
            <table style="width: 321px">
                <tr>
                    <td class="Label" style="width: 35%; white-space: nowrap;" align="right">
                        Project:
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="622px" TabIndex="1"></asp:TextBox><asp:Button
                            Style="display: none" ID="btnSetProject" runat="server" Text=" Project"></asp:Button><asp:HiddenField
                                ID="HProjectId" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="HFLocalMachineTime" runat="server" />
                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                            TargetControlID="txtProject" ServicePath="AutoComplete.asmx" ServiceMethod="GetMyProjectCompletionList"
                            OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1"
                            CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                            CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1">
                        </cc1:AutoCompleteExtender>
                    </td>
                </tr>
                <tr id="VersionDtl" runat="server" style="display: none;">
                    <td>
                    </td>
                    <td style="width: 59px" class="Label" align="left">
                        Version :<asp:Label runat="server" ID="VersionNo" Style="padding-right: 10px"></asp:Label>Version
                        Date :<asp:Label ID="VersionDate" Style="padding-right: 10px;" runat="server"></asp:Label>
                        Status :<img src="images/Lock.jpg" id="ImageLockUnlock" runat="server" alt="Lock" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="Label" style="width: 35%; white-space: nowrap;" align="right">
                        Period:
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="dropDownList" Width="192px"
                            OnSelectedIndexChanged="ddlPeriod_SelectedIndexChanged" AutoPostBack="True" TabIndex="2">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="Label" style="width: 35%; white-space: nowrap; height: 21px;" align="right">
                        Activity:
                    </td>
                    <td align="left" style="height: 21px">
                        <asp:DropDownList ID="ddlActivity" runat="server" AutoPostBack="true" CssClass="dropDownList"
                            Width="588px" TabIndex="3">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="Label" style="width: 35%; white-space: nowrap;">
                        Collection By:
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlUser" runat="server" CssClass="dropDownList" Width="192px"
                            TabIndex="4">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="Label" style="width: 35%; white-space: nowrap; height: 21px;">
                        Deviation:
                    </td>
                    <td align="left" class="Label" style="height: 21px">
                        <u>+</u><asp:TextBox ID="txtDeviation" runat="server" Width="57px" CssClass="textBox"
                            onblur="return CheckDeviation();" TabIndex="5" Font-Bold="True" Font-Size="Large"></asp:TextBox>Minutes
                    </td>
                </tr>
                <tr>
                    <td style="width: 35%; white-space: nowrap;" align="right" class="Label">
                        <asp:HiddenField ID="hfTextChnaged" runat="server" />
                        &nbsp;
                    </td>
                    <td class="Label" align="left">
                        <asp:Button ID="btnSubjectMgmt" runat="server" CssClass="btn btnnew" Text="Subject Mgmt"
                            TabIndex="6" />
                        <%--OnClientClick="return funValidate();"--%>
                        <asp:Button ID="btnSearch" runat="server" CssClass="btn btnnew" Text="Search" TabIndex="7"
                            OnClientClick="return CheckBlankField();" />
                        <asp:Button ID="btnReplace" runat="server" CssClass="btn btnsave" Text="Replace" TabIndex="7"
                            OnClientClick="return CheckBlankField();" Visible="False" />
                        <asp:Button ID="btnExit" runat="server" CssClass="btn btnexit" Text="Exit" TabIndex="9"
                            OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
                    </td>
                </tr>
                <tr>
                    <td align="right" class="Label" style="width: 35%; white-space: nowrap;">
                        BarCode:
                    </td>
                    <td align="left" class="Label">
                        <asp:TextBox ID="txtScan" runat="server" TabIndex="8" AutoPostBack="true" CssClass="textBox"
                            Enabled="False" onFocusOut="getLocalMachineTime();"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="Label" style="width: 35%; height: 15px;">
                    </td>
                    <td align="left" class="Label" style="height: 15px">
                        <asp:Label ID="lblSampleId" runat="server" Text="Sample Id :"></asp:Label><asp:Label
                            ID="lblSample" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblSubjectId" runat="server" Text="Subject Id :"></asp:Label><asp:Label
                            ID="lblSubject" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblMySubjectNo" runat="server" Text="MySubject No :"></asp:Label><asp:Label
                            ID="lblMySubject" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="Label" style="height: 15px">
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="Hdn_FreezeStatus" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel runat="server" ID="UpGridSubjectSample" UpdateMode="Conditional">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <asp:Panel ID="pnlGrid" runat="server" Height="300px" ScrollBars="Auto" Width="100%">
                            <asp:GridView ID="gvwSubjectSample" runat="server" AutoGenerateColumns="False" SkinID="grdViewSml"
                                Width="100%" PageSize="25">
                                <Columns>
                                    <asp:BoundField DataField="nPKSampleId" HeaderText="nPKSampleId" />
                                    <asp:BoundField DataField="vPKSampleId" HeaderText="Sample Id" />
                                    <asp:BoundField DataField="iMySubjectNo" HeaderText="MySubjectNo" />
                                    <asp:BoundField DataField="vMySubjectNo" HeaderText="MySubjectNo" />
                                    <asp:BoundField DataField="vSubjectID" HeaderText="Subject ID">
                                        <ItemStyle Wrap="false" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vInitials" HeaderText="Subject Initials">
                                        <ItemStyle Wrap="false" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="iRefNodeId" HeaderText="Ref.NodeId" />
                                    <asp:BoundField DataField="nRefTime" HeaderText="Scheduled Time">
                                        <ItemStyle Wrap="false" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="dCollectionDateTime" HeaderText="Collection DateTime">
                                        <ItemStyle Wrap="false" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="iCollectionBy" HeaderText="Collection By" />
                                    <asp:BoundField DataField="vCollectionBy" HeaderText="Collection By" />
                                    <asp:BoundField DataField="nRefTime" HeaderText="Dosing Time" />
                                    <asp:BoundField DataField="vRemark" HeaderText="Replacement Remark" />
                                    <asp:BoundField DataField="AttendanceMysubNo" HeaderText="AttendanceMysubNo" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkReplace" Text="Replace" runat="server"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSearch" />
            <asp:AsyncPostBackTrigger ControlID="gvwSubjectSample" EventName="RowCommand" />
            <asp:AsyncPostBackTrigger ControlID="txtreplaceCode" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="btnReplaceOK" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdatePanel runat="server" ID="UpOtherControls" UpdateMode="Conditional">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <button id="btnDeviation" runat="server" style="display: none;" />
                        <cc1:ModalPopupExtender ID="MPEDeviation" runat="server" PopupControlID="divDeviation"
                            PopupDragHandleControlID="LblPopUpDeviation" BackgroundCssClass="modalBackground"
                            TargetControlID="btnDeviation" CancelControlID="ImgPopUpCloseDeviation" BehaviorID="MPEDeviation">
                        </cc1:ModalPopupExtender>
                        <div id="divDeviation" runat="server" class="popUpDivNoTop" style="left: 25px; width: 600px;
                            display: none; position: absolute; top: 901px; height: 150px">
                            <asp:Panel ID="PnlDeviation" runat="server" Visible="true">
                                <table cellpadding="5" style="width: 600px">
                                    <tr>
                                        <td align="left" class="Label" style="text-align: center; height: 22px;" valign="top">
                                            <asp:Label ID="lblDeviation" runat="server" class="Label" Visible="true" Text="Deviation Remarks"></asp:Label>
                                        </td>
                                        <td align="right">
                                            <img id="ImgPopUpCloseDeviation" alt="Close" src="images/Sqclose.gif" style="position: relative;
                                                float: right; right: 5px;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" class="Label" colspan="2" valign="top" style="text-align: left">
                                            <table style="width: 100%; text-align: left;">
                                                <tr>
                                                    <td style="text-align: right" nowrap="noWrap">
                                                        Select Remark
                                                    </td>
                                                    <td style="text-align: left" nowrap="noWrap">
                                                        <asp:DropDownList ID="ddlRemarks" runat="server" Width="225px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right" nowrap="noWrap">
                                                        Enter Remarks, If Other
                                                    </td>
                                                    <td style="text-align: left" nowrap="noWrap">
                                                        <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Width="219px" CssClass="textBox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td nowrap="nowrap" style="text-align: right">
                                                    </td>
                                                    <td nowrap="nowrap" style="text-align: left">
                                                        <asp:Button ID="btnOk" runat="server" OnClientClick="return CheckRemarks();" CssClass="btn btnnew"
                                                            TabIndex="11" Text="Ok" />
                                                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" Text="Cancel" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                        <button id="btnReplacement" runat="server" style="display: none;" />
                        <cc1:ModalPopupExtender ID="MPEReplacement" runat="server" PopupControlID="divReplacement"
                            PopupDragHandleControlID="LblPopUpReplacement" BackgroundCssClass="modalBackground"
                            TargetControlID="btnReplacement" CancelControlID="ImgPopUpCloseReplacement">
                        </cc1:ModalPopupExtender>
                        <div id="divReplacement" runat="server" class="popUpDivNoTop" style="left: 25px;
                            width: 600px; position: absolute; top: 901px; height: 150px; display: none;">
                            <asp:Panel ID="pnlReplace" runat="server" Visible="true">
                                <table cellpadding="5" style="width: 602px">
                                    <tr>
                                        <td align="left" class="Label" style="text-align: center; height: 22px;" valign="top">
                                            <asp:Label ID="lblReplacement" runat="server" class="Label" Visible="true" Text="Sample Replacement"></asp:Label>
                                        </td>
                                        <td align="right">
                                            <img id="ImgPopUpCloseReplacement" alt="Close" src="images/Sqclose.gif" style="position: relative;
                                                float: right; right: 5px;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" class="Label" colspan="2" valign="top" style="text-align: left;
                                            width: 599px;">
                                            <table style="width: 100%; text-align: left;">
                                                <tr>
                                                    <td nowrap="nowrap" style="text-align: right">
                                                        BarCode:
                                                    </td>
                                                    <td nowrap="nowrap" style="text-align: left">
                                                        <asp:TextBox ID="txtreplaceCode" runat="server" CssClass="textBox" TabIndex="8" AutoPostBack="True"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td nowrap="nowrap" style="text-align: right; height: 15px;">
                                                        Replace:
                                                    </td>
                                                    <td nowrap="nowrap" style="text-align: left; height: 15px;">
                                                        <asp:Label ID="lblReplaceCode" runat="server"></asp:Label>
                                                        With
                                                        <asp:Label ID="lbReplaceWith" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right; height: 44px;" nowrap="noWrap">
                                                        Remarks:
                                                    </td>
                                                    <td style="text-align: left; height: 44px;" nowrap="noWrap">
                                                        <asp:TextBox ID="txtReplaceRemark" runat="server" TextMode="MultiLine" Width="219px"
                                                            CssClass="textBox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td nowrap="nowrap" style="text-align: right">
                                                    </td>
                                                    <td nowrap="nowrap" style="text-align: left">
                                                        <asp:Button ID="btnReplaceOK" runat="server" OnClientClick="return CheckReplaceRemarks();"
                                                            CssClass="btn btnnew" TabIndex="11" Text="Ok" />
                                                        <asp:Button ID="btnReplaceCancel" runat="server" CssClass="btn btncancel" Text="Cancel" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <button id="btnMangament" runat="server" style="display: none;" />
                        <cc1:ModalPopupExtender ID="MPESubMgmt" runat="server" PopupControlID="divMedEx"
                            PopupDragHandleControlID="LblPopUpSubMgmt" BackgroundCssClass="modalBackground"
                            TargetControlID="btnMangament" CancelControlID="ImgPopUpClose" BehaviorID="MPESubMgmt">
                        </cc1:ModalPopupExtender>
                        <div id="divMedEx" runat="server" class="popUpDivNoTop" style="display: none; left: 521px;
                            width: 320px; position: absolute; top: 525px; height: 400px">
                            <asp:Panel ID="pnlMedEx" runat="server" Visible="true">
                                <table cellpadding="5" style="width: 300px">
                                    <tr>
                                        <td align="left" class="Label" style="text-align: center; height: 22px;" valign="top">
                                            <asp:Label ID="LblPopUpSubMgmt" runat="server" class="Label" Visible="true" Text="Subject Management"></asp:Label>
                                        </td>
                                        <td align="right">
                                            <img id="ImgPopUpClose" alt="Close" src="images/Sqclose.gif" style="position: relative;
                                                float: right; right: 5px;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" class="Label" colspan="2" valign="top">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td rowspan="2">
                                                        <asp:Panel ID="pnlMedExGrid" runat="server" Height="300px" ScrollBars="Auto" Width="300px"
                                                            Visible="true">
                                                            <asp:GridView ID="gvwSubjects" runat="server" AutoGenerateColumns="False" PageSize="5"
                                                                SkinID="grdViewSmlSize" TabIndex="10" Visible="true">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <input id="chkSelectAll" onclick="SelectAll(this,'gvwSubjects')" type="checkbox" />
                                                                            <asp:Label ID="Label1" runat="server" Text="All"></asp:Label>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="ChkMove" Onclick="CheckUncheckAll('gvwSubjects');" runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="vWorkSpaceId" HeaderText="vWorkSpaceId">
                                                                        <ItemStyle Wrap="False" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="iMySubjectNoNew" HeaderText="iMySubject No.">
                                                                        <ItemStyle Wrap="False" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vMySubjectNo" HeaderText="My Subject No.">
                                                                        <ItemStyle Wrap="False" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vSubjectId" HeaderText="Subject Id">
                                                                        <ItemStyle Wrap="False" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vInitials" HeaderText=" Subject Initials">
                                                                        <ItemStyle Wrap="False" />
                                                                    </asp:BoundField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" class="Label" colspan="2" style="text-align: center" valign="top">
                                            <asp:Button ID="btnSaveSubject" OnClientClick="return CheckAtleastOne('<%= gvwSubjects.ClientId %>');"
                                                runat="server" CssClass="btn btnsave" TabIndex="11" Text="Save" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="HsubjectId" runat="server" />
            <asp:HiddenField ID="HPendingNode" runat="server" />
            <asp:HiddenField ID="hremark" runat="server" />
        </ContentTemplate>
        <Triggers>
            <%--   <asp:AsyncPostBackTrigger ControlID="btnSubjectMgmt" />--%>
            <asp:AsyncPostBackTrigger ControlID="gvwSubjectSample" EventName="RowCommand" />
            <asp:AsyncPostBackTrigger ControlID="txtreplaceCode" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="btnReplaceOK" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnSubjectMgmt" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnSaveSubject" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Button ID="btnShow" runat="server" Text="Show Dialog" Style="display: none" />
    <cc1:ModalPopupExtender ID="MPEActivitySequence" runat="server" TargetControlID="btnShow"
        PopupControlID="DivPopSequence" BackgroundCssClass="modalBackground" PopupDragHandleControlID="DivPopSequence"
        BehaviorID="MPEId">
    </cc1:ModalPopupExtender>
    <div id="DivPopSequence" runat="server" style="position: relative; display: none;
        background-color: #c2ebfc; padding: 5px; width: 500px; height: inherit; border: dotted 1px gray;
        border: solid 3px Navy;">
        <div>
            <div>
                <table width="100%">
                    <tr>
                        <td align="left" class="Label" style="text-align: center; height: 22px;" valign="top">
                            <asp:Label runat="server" ID="lblHeading" Text="Activity Deviation" class="LabelBold" />
                        </td>
                    </tr>
                </table>
                <hr />
                <div style="width: 470px; left: 204px; top: 77px" id="divTV" runat="server">
                    <table>
                        <tbody>
                            <tr>
                                <td style="width: 452px" valign="top" align="left">
                                    <table width="100%">
                                        <tr>
                                            <td align="left">
                                                <div style="overflow: auto; max-height: 100px;">
                                                    <asp:Panel runat="server" ID="Deviation">
                                                        <asp:PlaceHolder ID="PlaceDeviation" runat="server"></asp:PlaceHolder>
                                                    </asp:Panel>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lbl" runat="server" Text="Do You Want To Continue? "></asp:Label>
                                                <asp:LinkButton ID="lbtnForSub" runat="server" Text="Sequence" Style="display: none;"
                                                    onmouseover="funOnMouseOver(this);"></asp:LinkButton>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <label id="btnPforStruct" onclick="open_ProjStruct();" onmouseover="funOnMouseOver(this);">
                                                    <u>Structure Management</u></label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 452px" valign="top" align="left">
                                                <div>
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="lblRemark" runat="server" Text="Remarks:  " Style="color: Navy; font-weight: bold;"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td align="left" valign="top">
                                                            <div>
                                                                <asp:Label ID="lblError" runat="server" Style="color: Red;"></asp:Label>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="center">
                                                <asp:Button ID="btn_ok" Text="Ok" CssClass="btn btnnew" runat="server" OnClientClick="return chk_Remark();" />
                                                <input type="button" id="btn_cancel" value="Cancel" class="btn btncancel" onclick="unCheckSelected();" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" src="Script/Gridview.js"></script>

    <script type="text/javascript" src="Script/General.js"></script>

    <script type="text/javascript" language="javascript">

        function ClientPopulated(sender, e)
        {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e)
        {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        function CheckBlankField()
        {
            if (document.getElementById('<%=txtDeviation.ClientId %>').value.trim() == '')
            {
                msgalert('Please Enter Deviation !');
                document.getElementById('<%=txtDeviation.ClientId %>').focus();
                return false;
            }
            return true;
        }

        function CheckDeviation()
        {
            var txt = document.getElementById('<%=txtDeviation.ClientId %>');
            var val = document.getElementById('<%=txtDeviation.ClientId %>').value.trim();
            if (txt == null || typeof (txt) == 'undefined')
            {
                return false;
            }
            else if (val == '')
            {
                msgalert("Please Enter a Deviation !");
                document.getElementById('<%=txtDeviation.ClientId %>').value = '';
                document.getElementById('<%=txtDeviation.ClientId %>').focus();
                return false;
            }
            else if (isNaN(val) == true)
            {
                msgalert("Please Enter a Numeric Value !");
                document.getElementById('<%=txtDeviation.ClientId %>').value = '';
                document.getElementById('<%=txtDeviation.ClientId %>').focus();
                return false;
            }
            return true;
        }

        
        function CheckRemarks()
        {
            var ddl = document.getElementById('<%=ddlRemarks.clientid %>');
            var txt = document.getElementById('<%=txtRemarks.clientid %>').value.trim();
            if (ddl.selectedIndex == 0 && txt == '')
            {
                msgalert('Either Select Or Enter Remarks !');
                return false;
            }
            $find('MPEDeviation').hide();
            return true;
        }

       
        function CheckAtleastOne(gv)
        {
            var gvwSubject = document.getElementById('<%= gvwSubjects.ClientID %>');

            if (CheckOne(gvwSubject.id) == false)
            {
                msgalert('Please Select Atleast One Subject !');
                return false;
            }
            return true;
        }

        function CheckReplaceRemarks()
        {
            var txt = document.getElementById('<%=txtReplaceRemark.clientid %>').value.trim();
            if (txt == '')
            {
                msgalert('Please enter Remarks !');
                document.getElementById('<%=txtReplaceRemark.clientid %>').focus();
                return false;
            }
        }

        function PassTheBarcode()
        {
            if (document.getElementById('<%=txtScan.clientid %>').value.length > 0)
            {
                __doPostBack('ctl00$CPHLAMBDA$txtScan', '');
            }
            return false;
        }
       
         function CheckSelected()
        {        
          
            var Gdv = document.getElementById('<%=gvwSubjects.ClientId %>');
            var str="";
            for(c=1; c < Gdv.rows.length - 1;c++)// -1 as gdv row count contains both header and footer
            {
                if (Gdv.rows[c].childNodes[0].childNodes[0].type == "checkbox" )
                {
                    if( Gdv.rows[c].childNodes[0].childNodes[0].checked == true)
                    {
                        if(str.toString ()=="")
                        {
                            str=str+Gdv.rows[c].childNodes[2].innerText;
                        }
                        else
                        {
                            str=str+","+Gdv.rows[c].childNodes[2].innerText;
                        }
                    }
                }
            }
            document.getElementById('<%=HsubjectId.ClientID %>').value=str.toString();
           
        }
           function CheckUncheckAll(Grid) {
            var Checkall = document.getElementById('chkSelectAll');
            var Gvd = document.getElementById('<%=gvwSubjects.ClientId %>');
            j = 0;
            for (i = 0; i < document.forms[0].elements.length; i++) {
                if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf(Grid) > -1)) {
                    j = j + 1;
                    if (document.forms[0].elements[i].checked == false) {
                        Checkall.checked = false;
                        break;
                    }
                    else if (document.forms[0].elements[i].checked == true) {
                        if (j == Gvd.rows.length - 2) {
                            Checkall.checked = true;
                        }
                    }
                }
            }
        }
         function SelectAll(CheckBoxControl, Grid) {
         var str="";
        var Gvd = document.getElementById('<%=gvwSubjects.ClientId %>');
            if (CheckBoxControl.checked == true) {
                var i;
                var Cell = 0;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf(Grid) > -1)) {
                        if (document.forms[0].elements[i].disabled == false) {
                            document.forms[0].elements[i].checked = true;
                            Cell +=1;
                            
                             if(str.toString ()=="")
                             {
                                    str=str+Gvd.lastChild.childNodes[Cell].children[2].innerText;
                             }
                             else
                             {
                                    str=str+","+Gvd.lastChild.childNodes[Cell].children[2].innerText;
                             }
                          
                        }
                    }
                }
                document.getElementById('<%=HsubjectId.ClientID %>').value=str.toString();
             
            }
            else {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf(Grid) > -1)) {
                        document.forms[0].elements[i].checked = false;
                    }
                }
            }
        }

     
         function chk_Remark()
       {
        var txtContent=document.getElementById('<%= txtContent.ClientID %>').value.toString().trim();
      
        if(txtContent=="")
        {             
             document.getElementById('<%=lblError.ClientID %>').innerHTML="Please Enter Remark";
             return false ;       
       }
       else
       {
            document.getElementById('<%=hremark.ClientId %>').value=txtContent.toString(); 
       }
       return true ;
       }
  
        function unCheckSelected()
        {
            var Gdv = document.getElementById('<%=gvwSubjects.ClientId %>');
            $find('MPEId').hide();
            $find('MPESubMgmt').show(); 
            for(c=1; c < Gdv.rows.length - 1;c++)// -1 as gdv row count contains both header and footer
            {
                if (Gdv.rows[c].childNodes[0].childNodes[0].type == "checkbox" )
                {
                    if( Gdv.rows[c].childNodes[0].childNodes[0].checked == true)
                    {
                        Gdv.rows[c].childNodes[0].childNodes[0].checked = false;
                    }
                }
            }
                     
        document.getElementById('<%=HsubjectId.ClientID %>').value="";

        }
        
        function checkActivity()
        {
            if(document.getElementById('<%=ddlActivity.ClientID %>').selectedIndex <= 0)
            {
                msgalert("Please select atleast one activity !");
                return false;
            }
            return true;
        }

     function open_ProjStruct()
     {
      window.open("frmEditWorkspaceNodeDetail.aspx?WorkSpaceId=" + document.getElementById('<%= HProjectId.ClientId%>').value);
     }
     function funOnMouseOver(id)
    {  

    id.style.cursor='pointer';
    }
 // CRFVersion
//    function funValidate()
//    {
//     if (document.getElementById ('<%= Hdn_FreezeStatus.clientid %>').value=="U")
//     {
//        alert ("This Project Is UnFreezed.Kindly Freeze this To do Data Entry");
//        return false; 
//     }
//     return true;
//    }
//    
//======================================================    
function getLocalMachineTime()
{
    var date=new Date () ;
    
   document.getElementById('<%=HFLocalMachineTime.ClientId %>').value=(date.getMonth()+1)+"/"+date.getDate()+"/"+date.getFullYear()+" "+date.getHours()+":"+date.getMinutes()+":"+date.getSeconds();         
}
    </script>

</asp:Content>
