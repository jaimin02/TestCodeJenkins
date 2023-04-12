<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" CodeFile="~/frmEditWorkspaceNodeDetail.aspx.vb"
    AutoEventWireup="false" Inherits="frmEditWorkspaceNodeDetail" EnableEventValidation="false"
    Theme="StyleBlue" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:content id="Content1" contentplaceholderid="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" src="Script/General.js"></script>

    <script type="text/javascript" src="Script/jquery.min.js"></script>

    <style type="text/css">
        a {
            font-weight: bold;
        }
    </style>

    <script type="text/javascript">

        function ShowTemplate(path) {
            window.open(path);
        }

        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        function ddlActSelected() {
            var txt = document.getElementById('<%= txtDisplayName.ClientId %>');
            var ddlAct = document.getElementById('<%= ddlAct.ClientId %>');

            txt.value = ddlAct.options[ddlAct.selectedIndex].text;
            return true;
        }
        function ddlActivitySelected() {
            var txt = document.getElementById('<%= txtActDispName.ClientId %>');
            var ddlAddActivity = document.getElementById('<%= ddlAddActivity.ClientId %>');

            txt.value = ddlAddActivity.options[ddlAddActivity.selectedIndex].text;
            return true;
        }
        function NodeChecked() {
            var inputsInTree = document.getElementById('<%= TVWorkspace.ClientId %>').getElementsByTagName("input");
            var count = 0;

            for (var i = 0; i < inputsInTree.length; i++) {
                var currentElement = inputsInTree[i];
                if (currentElement.type == "checkbox" && currentElement.checked) {
                    count += 1;
                }

                if (count > 1) {
                    msgalert('Please Select Only One Node !');
                    currentElement.checked = false;
                    return false;
                }

            }
            if (count == 1) {
                return true;
            }
            //return false;
        }

        function IsAnyNodeChecked(Operation) {

            if (document.getElementById('<%= Hdn_FreezeStatus.clientid %>').value == "F") {
                msgalert("This Project Is Freezed.Kindly UnFreeze This To Change Project Structure !");
                return false;
            }

            var inputsInTree = document.getElementById('<%= TVWorkspace.ClientId %>').getElementsByTagName("input");
            var count = 0;
            for (var i = 0; i < inputsInTree.length; i++) {
                var currentElement = inputsInTree[i];
                if (currentElement.type == "checkbox" && currentElement.checked) {
                    count += 1;
                }
            }
            if (count == 1) {
                if (Operation == 'D') {
                    return confirm('Are You Sure You Want To Delete Activity And All Its Sub-Activities?');
                }
                return true;
            }
            msgalert('Please Select Atleat One Node !');
            return false;
        }

        function validateActivity(obj) {
            var act = document.getElementById('<%=ddlAddActivity.ClientId%>');
            var actGrp = document.getElementById('<%=ddlActivityGroup.ClientId%>');
            var txtDisp = document.getElementById('<%=txtActDispName.ClientId%>');
            var period = document.getElementById('<%=ddlPeriod.ClientId%>');

            var actGrpIndex = actGrp.selectedIndex;
            var actIndex = act.selectedIndex;
            var txtDisp = txtDisp.value;
            var period = period.options[period.selectedIndex].text;


            if (actGrpIndex > 0) {
                if (actIndex > 0) {
                    var actVal = act.options[act.selectedIndex].text;
                    if (txtDisp != '') {
                        if (ConfirmPeriod(actVal, period)) {
                            return true;
                            obj.style.display = 'none';
                        }

                        else
                            return false;
                    }
                    else {
                        msgalert("Enter Activity Display Name !");
                        return false;
                    }
                }
                else {
                    msgalert("Select Activity !");
                    return false;
                }
            }
            else {
                msgalert("Select Activity Group !");
                return false;
            }
        }

        function validateEditActivity() {
            var act = document.getElementById('<%=ddlAct.ClientId%>');
            var actGrp = document.getElementById('<%=ddlActivityGroup2.ClientId%>');
            var txtDisp = document.getElementById('<%=txtDisplayName.ClientId%>');
            var period = document.getElementById('<%=ddlPeriodEdit.ClientId%>');

            var actGrpIndex = actGrp.selectedIndex;
            var actIndex = act.selectedIndex;
            var txtDisp = txtDisp.value;
            var period = period.options[period.selectedIndex].text;


            if (actGrpIndex > 0) {
                if (actIndex > 0) {
                    var actVal = act.options[act.selectedIndex].text;
                    if (txtDisp != '') {
                        if (ConfirmPeriod(actVal, period)) {
                            return true;
                            obj.style.display = 'none';
                        }

                        else
                            return false;
                    }
                    else {
                        msgalert("Enter Activity Display Name !");
                        return false;
                    }
                }
                else {
                    msgalert("Select Activity !");
                    return false;
                }
            }
            else {
                msgalert("Select Activity group !");
                return false;
            }
        }

        function ConfirmPeriod(actVal, period) {
            if (confirm("Are You Sure You Want To Add '" + actVal + "' Activity In Period : " + period + " ?")) {
                return true;
            }
            else
                return false;
        }

        function CloseTab() {
              self.close();
        }

    </script>

    <asp:UpdatePanel ID="UpProject" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="Hdn_FreezeStatus" runat="server" />
            <table style="width: 100%" id="tblProjectName" runat="server">
               
                 <tr>
                    <td class="Label" style="text-align: right; width: 25%;">
                        <strong class="Label">Project Name/Request Id :</strong>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="70%"></asp:TextBox>&nbsp<asp:Button
                            ID="btnExitPage" runat="server" CssClass="btn btnexit" Text="Exit" ToolTip="Exit"
                            OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
                        <asp:Button ID="btnSetProject" runat="server" OnClick="btnSetProject_Click" Style="display: none"
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
                    <td class="Label" colspan="2">
                        <div id="VersionDtl" runat="server" style="display: none; margin: auto;">
                            Version: <asp:Label runat="server" ID="VersionNo" Style="padding-right: 10px"></asp:Label>Version
                            Date: <asp:Label ID="VersionDate" Style="padding-right: 10px;" runat="server"></asp:Label>
                            Status: <img src="images/Freeze.jpg" id="ImageLockUnlock" runat="server" alt="Lock" />
                        </div>
                    </td>
                </tr>

            </table>

            <table style="width: 100%; display: block; overflow: hidden;" border="0" runat="server"
                id="tblNote" visible="false">
                <tr>
                    <td style="text-align: left;" class="Label">
                        Note : Click on activity to see attached template...
                    </td>
                </tr>
            </table>

            <table width="100%">
                <tr>
                    <td align="left" style="width: 100%">
                        <asp:TreeView ID="TVWorkspace" runat="server" BorderColor="DarkGreen" BorderStyle="None" NodeWrap="true"
                            BorderWidth="1px" ShowCheckBoxes="All" ShowLines="True" Font-Bold="True" Width="100%">
                        </asp:TreeView> 
                    </td> 
                </tr>
            </table>

            <div style="width: 100%;">

                <table style="width: 100%; text-align: center;">
                    <tr>
                        <td>
                           <asp:Button ID="btnuserrights" runat="server" Text="Default UserRights" ToolTip="Default UserRights"
                               CssClass="btn btnnew" Visible="False" />
                            <asp:Button ID="btnsetuserrights" runat="server" Text="Set UserRights" ToolTip="Set UserRights"
                                 CssClass="btn btnnew" Visible="False" />
                            <asp:Button ID="BtnBack" runat="server" Text="" ToolTip="Back"
                                CssClass="btn btnback" />
                        </td>
                    </tr>
                </table>

            </div>
            
            <button id="Btn3" runat="server" style="display: none;" />
            <cc1:ModalPopupExtender ID="mpeDialogAddActivity" runat="server" PopupControlID="DivPopUp"
                PopupDragHandleControlID="LblPopUpTitle" BackgroundCssClass="modalBackground"
                TargetControlID="Btn3" CancelControlID="ImgPopUp">
            </cc1:ModalPopupExtender>
            

            <div id="DivPopUp" runat="server" style="position: relative; display: none; background-color: #c2ebfc;
                padding: 5px; width: 60%; height: inherit; border: dotted 1px gray; border: solid 3px Navy;">

                <div style="width: 100%;">
                    <div style="width: 100%;">
                        <table width="100%">
                            <tr>
                                <td>
                                    <img id="ImgPopUp" alt="Close" src="images/Sqclose.gif" style="position: relative;
                                        float: right; right: 5px;" title="Close" />
                                </td>
                            </tr>
                        </table>
                        
                        <div style="width: 100%;" id="divTV" runat="server">
                            <table width="100%">
                                <tbody>
                                    
                                    <tr>
                                        <td>
                                            <div style="width: 100%;" id="div1" runat="server">
                                                <table width="100%">
                                                    <tbody>
                                                        <tr>
                                                            <td style="width: 40%; white-space: nowrap; text-align: right;" class="Label" valign="top">
                                                                Activity Group* :
                                                            </td>
                                                            <td valign="top" style="text-align: left;">
                                                                <asp:DropDownList ID="ddlActivityGroup" runat="server" CssClass="dropDownList" Width="65%"
                                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlActivityGroup_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="white-space: nowrap; text-align: right;" class="Label" valign="top">
                                                                Activity Name* :
                                                            </td>
                                                            <td valign="top" style="text-align: left;">
                                                                <asp:DropDownList ID="ddlAddActivity" runat="server" CssClass="dropDownList" Width="65%"
                                                                    AutoPostBack="True" onchange="return ddlActivitySelected();">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="white-space: nowrap; text-align: right;" class="Label" valign="top">
                                                                Display Name* :
                                                            </td>
                                                            <td valign="top" style="text-align: left; .">
                                                                <asp:TextBox ID="txtActDispName" CssClass="textBox" runat="server" Width="65%"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="white-space: nowrap; text-align: right;" class="Label" valign="top">
                                                                Period :
                                                            </td>
                                                            <td valign="top" style="text-align: left;">
                                                                <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="dropDownList" Width="45%">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="divBtn" style="width: 100%" runat="server">
                                                <table width="100%">
                                                    <tbody>
                                                        <tr>
                                                            <td style="text-align: center;" valign="top">
                                                              
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                            <asp:GridView ID="GVActivityName" runat="server" SkinID="grdViewSmlAutoSize" Width="100%"
                                                AutoGenerateColumns="False">
                                                <Columns>
                                                    <asp:BoundField DataField="vActivityId" HeaderText="ActivityId">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vActivityName" HeaderText="ActivityName">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vDeptCode" HeaderText="DeptCode">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="iStageId" HeaderText="StageId">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vStageDesc" HeaderText="StageDesc">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="iSequenceNo" HeaderText="SequenceNo">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        
                    </div>
                </div>

            </div>

            <button id="Button1" runat="server" style="display: none;" />
            <cc1:ModalPopupExtender ID="mpeDialogEditActivity" runat="server" PopupControlID="DivPopEdit"
                PopupDragHandleControlID="LblPopUpTitle" BackgroundCssClass="modalBackground"
                TargetControlID="Button1" CancelControlID="ImgCloseEdit">
            </cc1:ModalPopupExtender>

            <div id="DivPopEdit" runat="server" style="position: relative; display: none; background-color: #c2ebfc;
                padding: 5px; width: 620px; height: inherit; border: dotted 1px gray; border: solid 3px Navy;">
                <div>
                    <div>
                        <table width="100%">
                            <tr>
                                <td>
                                    <img id="ImgCloseEdit" alt="Close" src="images/Sqclose.gif" style="position: relative;
                                        float: right; right: 5px;" title="Close" />
                                </td>
                            </tr>
                        </table>
                        
                        <div style="width: 100%; height: inherit;" id="divact" runat="server">
                            
                            <table width="100%">
                                <tbody>
                                    <tr>
                                        <td style="white-space: nowrap; width: 40%; text-align: right;" class="Label" valign="top">
                                            Activity Group* :
                                        </td>
                                        <td valign="top" style="text-align: left;">
                                            <asp:DropDownList ID="ddlActivityGroup2" runat="server" CssClass="dropDownList" AutoPostBack="True"
                                                Width="65%">
                                            </asp:DropDownList>
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; text-align: right;" class="Label" valign="top">
                                            Select Activity* :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="ddlAct" runat="server" CssClass="dropDownList" Width="65%"
                                                onchange="return ddlActSelected();">
                                            </asp:DropDownList>
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; text-align: right;" class="Label" valign="top">
                                            Display Name* :
                                        </td>
                                        <td valign="top" style="text-align: left;">
                                            <asp:TextBox ID="txtDisplayName" runat="server" Width="65%" CssClass="textBox"></asp:TextBox>
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; text-align: right;" class="Label" valign="top">
                                            Period :
                                        </td>
                                        <td valign="top" style="text-align: left;">
                                            <asp:DropDownList ID="ddlPeriodEdit" runat="server" CssClass="dropDownList" Width="40%">
                                            </asp:DropDownList>
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align: center;">
                                            <asp:Button ID="btnOK" runat="server" Text="Attach" CssClass="btn btnsave" 
                                                ToolTip="Attach" OnClientClick="return validateEditActivity();" />
                                            <asp:Button ID="btnExit" OnClick="btnExit_Click" runat="server" Text="Exit" CssClass="btn btnexit"
                                                 ToolTip="Exit" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            
                        </div>
                        
                    </div>
                </div>
            </div>
           </ContentTemplate>
        
    </asp:UpdatePanel>
</asp:content>
