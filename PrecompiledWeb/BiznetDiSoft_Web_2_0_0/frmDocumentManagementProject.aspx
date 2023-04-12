<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmDocumentManagementProject, App_Web_l40sj1d0" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" src="Script/General.js"></script>

    <script type="text/javascript" language="javascript">

       function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));

        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }
        function ClientPopulated1(sender, e) {

            ProjectClientShowing('AutoCompleteExtender2', $get('<%= txtProjNo.ClientId %>'));

        }

        function OnSelected1(sender, e) {

            ProjectOnItemSelected(e.get_value(), $get('<%= txtProjNo.clientid %>'),
            $get('<%= HProject.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }


        function ShowElement() {

            var elem = document.getElementById('<%= divact.ClientId %>');
            elem.style.display = 'block';
            SetCenter(elem.id);

        }

        function HideElement() {
            var elem = document.getElementById('<%= divact.ClientId %>');
            elem.style.display = 'none';

        }

        function IsAnyNodeChecked(treeViewID) {
            var inputsInTree = document.getElementById(treeViewID).getElementsByTagName("input");
            var count = 0;
            for (var i = 0; i < inputsInTree.length; i++) {
                var currentElement = inputsInTree[i];
                if (currentElement.type == "checkbox" && currentElement.checked) {
                    count += 1;
                }

                if (count > 1) {
                    msgalert('Please Select Only One Node');
                    currentElement.checked = false;
                    return true;
                }

            }
            if (count == 1) {
                return true;
            }
            //alert('Please Select Atleat One Node');
            //return false;
        }

        function RefreshPage() {
        }
        
    </script>

    <table style="width: 100%">
        <tr>
            <td align="center" style="width: 100%" class="Label">
                <strong class="Label">Project Name/Request Id :</strong>
                <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="362px"></asp:TextBox>&nbsp;
                <asp:Button ID="btnExit" runat="server" CssClass="btn btnexit" Text="Exit" />
                <asp:Button ID="btnSetProject" runat="server" OnClick="btnSetProject_Click" Style="display: none"
                    Text=" Project" />
                <asp:HiddenField ID="HProjectId" runat="server" />
                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                    CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                    CompletionListElementID="pnlProjectList" CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                    OnClientShowing="ClientPopulated" ServiceMethod="GetProjectCompletionListWithOutSponser"
                    ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True">
                </cc1:AutoCompleteExtender>
                <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 300px; overflow: auto;
                            overflow-x: hidden" />
                &nbsp;
            </td>
        </tr>
        <tr id="TR_TreeView" runat="server">
            <td align="left" style="width: 100%">
                <asp:TreeView ID="TVWorkspace" runat="server" BorderColor="DarkGreen" BorderStyle="None"
                    BorderWidth="1px" ShowCheckBoxes="All" ShowLines="True" Font-Bold="True" Width="100%">
                </asp:TreeView>
            </td>
        </tr>
    </table>
    <%--<asp:UpdatePanel ID="UPDivAct" runat ="server" RenderMode="Inline" UpdateMode="Conditional">
   <ContentTemplate>--%>
    <div style="left: 204px; width: 450px; top: 262px; height: 90px; display: none" id="divact"
        class="DIVSTYLE2" align="left" runat="server">
        <table width="100%">
            <div>
                <img id="Img1" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right;
                    right: 5px;" onclick="HideElement()" />
            </div>
            <tbody>
                <tr>
                    <td align="center" class="Label" valign="top">
                        <asp:Label ID="lblNode" runat="server" Width="380px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" class="Label" valign="top">
                        <asp:Panel ID="pnlMedExGrid" runat="server" Height="30px" ScrollBars="Auto" Width="350px">
                            <asp:GridView ID="gvw_MedExDetail" runat="server" SkinID="grdViewSmlSize" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField DataField="vMedExDesc" HeaderText="Attribute" />
                                    <asp:BoundField DataField="vDefaultValue" HeaderText="Default Value" />
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="center" class="Label" valign="top">
                    </td>
                </tr>
                <tr>
                    <td class="Label" valign="top" align="center">
                        <asp:Button ID="btnDocs" runat="server" Text="Docs" CssClass="btn btnnew"></asp:Button>
                        <asp:Button ID="btnSubDocs" runat="server" Text="SubDocs" CssClass="btn btnnew"></asp:Button>
                        <asp:Button ID="btnSubsDetail" runat="server" Text="SubsDetail" CssClass="btn btnnew">
                        </asp:Button>
                        <asp:Button ID="btnRelease" runat="server" Text="Release" CssClass="btn btnnew"></asp:Button>
                        <input type="button" runat="server" id="iRelease" />
                        <asp:Button ID="btnTalk" runat="server" CssClass="btn btnnew" Text="Talk" />
                        <%-- <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="button" OnClick="btnClose_Click">
                        </asp:Button>--%>
                        <%--        <asp:Button ID="btnGetdata" runat="server" style="display:none" OnClick="btnGetdata_Click" CssClass="button" Text="Getdata" />--%>
                    </td>
                </tr>
            </tbody>
        </table>
        <br />
    </div>
    <%--FOR POPUP OF RELEASE--%>
    <button id="Btn3" runat="server" style="display: none;" />
    <cc1:ModalPopupExtender ID="MdlRelease" runat="server" PopupControlID="DivPopUpWorkSummary"
        PopupDragHandleControlID="LblPopUpTitleWorkSummary" BackgroundCssClass="modalBackground"
        TargetControlID="iRelease" CancelControlID="ImgPopUpCloseWorkSummary">
    </cc1:ModalPopupExtender>
    <%--added by vishal--%>
    <div id="DivPopUpWorkSummary" runat="server" style="position: relative; display: none;
        background-color: #cee3ed; padding: 5px; width: 500px; height: inherit; border: dotted 1px gray;">
        <div>
            <div>
                <img id="ImgPopUpCloseWorkSummary" alt="Close" src="images/Sqclose.gif" style="position: relative;
                    float: right; right: 5px;" />
                <asp:Label ID="LblPopUpTitleWorkSummary" runat="server" class="Label" Visible="true"
                    Text="Release Document"></asp:Label>
            </div>
        </div>
        <table id="tdGeneral" runat="server">
            <tr>
                <td align="Center">
                    <table style="width: 100%" align="center">
                        <tr>
                            <td align="right" style="width: 120px;">
                                Activity Name :
                            </td>
                            <td>
                                <asp:TextBox ID="txtActivityName" ReadOnly="true" BackColor="AntiqueWhite" runat="server"
                                    CssClass="textBox" Width="246px" __designer:wfdid="w92"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 100px;">
                                Project No:
                            </td>
                            <td align="left" style="width: 100px;">
                                <asp:TextBox ID="txtProjNo" runat="server" CssClass="textBox" Width="246px" __designer:wfdid="w91">
                                </asp:TextBox>
                                <asp:HiddenField ID="HProject" runat="server" />
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" UseContextKey="True"
                                    TargetControlID="txtProjNo" ServicePath="AutoComplete.asmx" ServiceMethod="GetAllProjectList"
                                    OnClientShowing="ClientPopulated1" MinimumPrefixLength="1" CompletionListItemCssClass="autocomplete_listitem"
                                    CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListCssClass="autocomplete_list"
                                    BehaviorID="AutoCompleteExtender2">
                                </cc1:AutoCompleteExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                Remarks:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="textBox" Width="244px" __designer:wfdid="w112"
                                    TextMode="MultiLine" MaxLength="1023"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                No of Copies:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtNosofCopies" runat="server" CssClass="textBox" Width="30px" __designer:wfdid="w92"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="Right">
                                <asp:Button ID="BtnSave" runat="server" Text="Release" CssClass="btn btnsave" />
                            </td>
                            <td align="left">
                                <asp:Button ID="BtnCancel" runat="server" Text="Cancel" CssClass="btn btncancel" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
