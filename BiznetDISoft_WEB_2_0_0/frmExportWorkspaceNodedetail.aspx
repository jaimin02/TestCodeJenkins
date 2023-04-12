<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmExportWorkspaceNodedetail.aspx.vb" Inherits="ExportWorkspaceNodedeatail" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" src="Script/General.js"></script>

    <script type="text/javascript">

        function ClientPopulated(sender, e) {

            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }





        //************************** Treeview Parent-Child check behaviour*****added on 13-11-09****************************//  

        function OnTreeClick(evt) {
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
            if (isChkBoxClick) {
                var parentTable = GetParentByTagName("table", src);
                var nxtSibling = parentTable.nextSibling;
                if (nxtSibling && nxtSibling.nodeType == 1)//check if nxt sibling is not null & is an element node
                {
                    if (nxtSibling.tagName.toLowerCase() == "div") //if node has children
                    {
                        //check or uncheck children at all levels
                        CheckUncheckChildren(parentTable.nextSibling, src.checked);
                    }
                }
                //check or uncheck parents at all levels
                // CheckUncheckParents(src, src.checked);
            }
        }

        function CheckUncheckChildren(childContainer, check) {
            var childChkBoxes = childContainer.getElementsByTagName("input");
            var childChkBoxCount = childChkBoxes.length;
            for (var i = 0; i < childChkBoxCount; i++) {
                childChkBoxes[i].checked = check;
            }
        }

        function CheckUncheckParents(srcChild, check) {
            var parentDiv = GetParentByTagName("div", srcChild);
            var parentNodeTable = parentDiv.previousSibling;

            if (parentNodeTable) {
                var checkUncheckSwitch;

                if (check) //checkbox checked
                {
                    var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild);
                    if (isAllSiblingsChecked)
                        checkUncheckSwitch = true;
                    else
                        return; //do not need to check parent if any child is not checked
                }
                else //checkbox unchecked
                {
                    checkUncheckSwitch = false;
                }

                var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
                if (inpElemsInParentTable.length > 0) {
                    var parentNodeChkBox = inpElemsInParentTable[0];
                    parentNodeChkBox.checked = checkUncheckSwitch;
                    //do the same recursively
                    CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
                }
            }
        }

        function AreAllSiblingsChecked(chkBox) {
            var parentDiv = GetParentByTagName("div", chkBox);
            var childCount = parentDiv.childNodes.length;
            for (var i = 0; i < childCount; i++) {
                if (parentDiv.childNodes[i].nodeType == 1) //check if the child node is an element node
                {
                    if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                        var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                        //if any of sibling nodes are not checked, return false
                        if (!prevChkBox.checked) {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        //utility function to get the container of an element by tagname
        function GetParentByTagName(parentTagName, childElementObj) {
            var parent = childElementObj.parentNode;
            while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
                parent = parent.parentNode;
            }
            return parent;
        }
  
    </script>

    <table style="width: 100%;">
        <tr>
            <td align="center" colspan="2" style="width: 100%" class="label">
                <strong class="Label">Project Name/Request Id :</strong>
                <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="622px"></asp:TextBox>
                <asp:Button ID="btnSetProject" runat="server" OnClick="btnSetProject_Click" Style="display: none;"
                    Text=" Project" />
                <asp:HiddenField ID="HProjectId" runat="server" />
                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                    CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                    CompletionListElementID="pnlProjectList" CompletionListItemCssClass="autocomplete_listitem"
                    MinimumPrefixLength="1" OnClientItemSelected="OnSelected" OnClientShowing="ClientPopulated"
                    ServiceMethod="GetMyProjectCompletionList" ServicePath="AutoComplete.asmx" TargetControlID="txtProject"
                    UseContextKey="True">
                </cc1:AutoCompleteExtender>
                <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 300px; overflow: auto;
                    overflow-x: hidden" />
            </td>
        </tr>
        <tr>
            <td align="left" colspan="2" style="width: 100%">
                <asp:TreeView ID="TVWorkspace" runat="server" BorderColor="DarkGreen" BorderStyle="None"
                    BorderWidth="1px" ShowCheckBoxes="All" ShowLines="True" Font-Bold="True" Width="100%">
                </asp:TreeView>
            </td>
        </tr>
        <tr align="center">
            <td align="right" style="width: 50%; height: 25px;">
                <asp:Button ID="BtnExport" runat="server" CssClass="btn btnexcel" />
            </td>
            <td align="left" style="width: 50%; height: 25px;">
                <asp:Button ID="btnExitPage" runat="server" CssClass="btn btnexit" Text="Exit" />
            </td>
            <%--<td align="left">
                <asp:Button ID="btnexportpdf" runat="server" CssClass="button" Text="Export To Pdf"
                    Height="23px" Width="116px" />
            </td>--%>
        </tr>
    </table>
</asp:Content>
