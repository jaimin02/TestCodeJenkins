<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmtreenode, App_Web_mlepfeoz" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <style type="text/css">
        a:hover
        {
            color: Black;
            background-color: #DFE0FF;
        }
    </style>

    <script type="text/javascript" src="Script/General.js"></script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <button id="Btn4" runat="server" style="display: none;" />
            <cc1:ModalPopupExtender ID="mpeaddactivity" runat="server" PopupControlID="divTV"
                PopupDragHandleControlID="LblPopUpTitle" BackgroundCssClass="modalBackground"
                TargetControlID="Btn4" CancelControlID="ImgPopUp1">
            </cc1:ModalPopupExtender>
            <div id="divTV" runat="server" style="position: relative; display: none; background-color: #c2ebfc;
                padding: 5px; width: 60%; height: inherit; border: dotted 1px gray; border: solid 3px Navy;">
                <div style="width: 100%">
                    <table width="100%">
                        <tr>
                            <td style="text-align: center;">
                                <img id="ImgPopUp1" alt="Close" src="images/Sqclose.gif" style="position: relative;
                                    float: right; right: 5px;" title="Close" />
                            </td>
                        </tr>
                    </table>
                    <div style="width: 100%;">
                        <table width="100%">
                            <tbody>
                                <tr>
                                    <td>
                                        <div id="div1" runat="server" style="width: 100%; margin: auto;">
                                            <table width="100%">
                                                <tbody>
                                                    <tr>
                                                        <td style="width: 40%; white-space: nowrap; text-align: right;" class="Label" valign="top">
                                                            <asp:Label ID="Label1" runat="server" Text="Activity Group :"></asp:Label>
                                                        </td>
                                                        <td valign="top" style="text-align: left;">
                                                            <asp:DropDownList ID="ddlActivityGroup" runat="server" CssClass="dropDownList" Width="65%"
                                                                AutoPostBack="True" OnSelectedIndexChanged="ddlActivityGroup_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="white-space: nowrap; text-align: right;" class="Label" valign="top">
                                                            <strong class="Label">Actvity Name :</strong>
                                                        </td>
                                                        <td valign="top" style="text-align: left;">
                                                            <asp:DropDownList ID="ddlAddActivity" runat="server" CssClass="dropDownList" Width="65%"
                                                                AutoPostBack="True">
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
                                        <div id="divBtn" runat="server">
                                            <table width="100%">
                                                <tbody>
                                                    <tr>
                                                        <td style="margin: auto; text-align: center;" valign="top">
                                                            <asp:Button ID="btnAddLast" runat="server" Text="Add Last" ToolTip="Add Last" CssClass="btn btnnew"
                                                                />
                                                            <asp:Button ID="btnAddBefore" runat="server" Text="Add Before" ToolTip="Add Before"
                                                                CssClass="btn btnnew" />
                                                            <asp:Button ID="btnAddAfter" runat="server" Text="Add After" ToolTip="Add After"
                                                                CssClass="btn btnnew"  />
                                                            <asp:Button ID="btnclose" OnClick="btnclose_Click" runat="server" Text="Close" ToolTip="Close"
                                                                CssClass="btn btnclose" />
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
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAddLast" />
            <asp:PostBackTrigger ControlID="btnAddAfter" />
            <asp:PostBackTrigger ControlID="btnAddBefore" />
            <asp:AsyncPostBackTrigger ControlID="BtnAddActivity" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <table style="width: 100%; display: block; overflow: hidden;" border="0" runat="server"
        id="tblNote">
        <tr align="center">
            <td align="right" class="Label">
                Note : Click on activity to see attached template...
            </td>
        </tr>
    </table>
    <div style="text-align: left; width: 100%;">
        <asp:TreeView ID="TVTemplate" runat="server" BorderColor="DarkGreen" BorderStyle="Solid"
            BorderWidth="1px" ShowCheckBoxes="All" ShowLines="True" Font-Bold="True" Width="100%">
        </asp:TreeView>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>
                <button id="Btn3" runat="server" style="display: none;" />
                <cc1:ModalPopupExtender ID="mpeDialogAddActivity" runat="server" PopupControlID="divact"
                    PopupDragHandleControlID="LblPopUpTitle" BackgroundCssClass="modalBackground"
                    TargetControlID="Btn3" CancelControlID="ImgPopUp">
                </cc1:ModalPopupExtender>
                <div id="divact" runat="server" style="position: relative; display: none; background-color: #c2ebfc;
                    padding: 5px; width: 60%; height: inherit; border: dotted 1px gray; border: solid 3px Navy;">
                    <div style="width: 100%">
                        <table width="100%">
                            <tr>
                                <td>
                                    <img id="ImgPopUp" alt="Close" src="images/Sqclose.gif" style="position: relative;
                                        float: right; right: 5px;" title="Close" />
                                </td>
                            </tr>
                        </table>
                        <div style="width: 100%;">
                            <table width="100%">
                                <tbody>
                                    <tr>
                                        <td style="width: 40%; white-space: nowrap; text-align: right;" class="Label" valign="top">
                                            Activity Group:
                                        </td>
                                        <td valign="top" style="text-align: left;">
                                            <asp:DropDownList ID="ddlActivityGroup2" runat="server" CssClass="dropDownList" AutoPostBack="True"
                                                Width="65%">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; text-align: right;" valign="top" class="Label">
                                            <asp:Label ID="lblact" runat="server" Text="Select Activity:"></asp:Label>
                                        </td>
                                        <td valign="top" style="text-align: left;">
                                            <asp:DropDownList ID="ddlAct" runat="server" CssClass="dropDownList" Width="65%"
                                                onchange="return ddlActSelected();">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; text-align: right;" valign="middle" class="Label ">
                                            Display Name:
                                        </td>
                                        <td valign="top" style="text-align: left;">
                                            <asp:TextBox ID="txtDisplayName" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align: center;">
                                            <asp:Button ID="btnOK" runat="server" Text="Attach" ToolTip="Attach" CssClass="btn btnnew"
                                                Width="10%" Height="20px" />
                                            <asp:Button ID="btnExit" OnClick="btnExit_Click" runat="server" Text="Exit" ToolTip="Exit"
                                                CssClass="btn btnexit" Width="10%" Height="20px" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExit" />
                <asp:PostBackTrigger ControlID="btnOK" />
                <asp:AsyncPostBackTrigger ControlID="btneditact" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <table style="text-align: center; width: 100%; margin-top: 2%;">
            <tr>
                <td>
                    <asp:Button ID="BtnAddActivity" runat="server" Text="Add Activity" ToolTip="Add Activity"
                        OnClientClick="return CheckOneOnly();" CssClass="btn btnnew" Width="10%" />
                    <asp:Button ID="btneditact" runat="server" Text="Edit Activity" ToolTip="Edit Activity"
                        CssClass="btn btnnew" Width="10%" OnClientClick="return CheckOneOnly();" />
                    <asp:Button ID="btnDeleteActivity" runat="server" OnClientClick="return   CheckDelete();"
                        Text="Delete Activity" ToolTip="Delete Activity" CssClass="btn btnnew" Width="11%" />
                    <asp:Button ID="btnsetuserrights" runat="server" Text="Set UserRights" Width="10%"
                        CssClass="btn btnnew" Visible="False" />
                    <asp:Button ID="btnuserrights" runat="server" Text="Default UserRights" ToolTip="Default UserRights"
                        Width="10%" CssClass="btn btnnew" Visible="False" />
                    <asp:Button ID="BtnBack" runat="server" Text="" ToolTip="Back" Width="10%" CssClass="btn btnback" />
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript" language="javascript">



        function ddlActSelected() {
            var txt = document.getElementById('<%= txtDisplayName.ClientId %>');
            var ddlAct = document.getElementById('<%= ddlAct.ClientId %>');

            txt.value = ddlAct.options[ddlAct.selectedIndex].text;
            return true;
        }
        function ShowTemplate(path) {
            window.open(path);
        }
        function NoTemplate() {
            msgalert('No Templates Attached With This Activity !');
        }
        function CheckOneOnly() {

            var chkDup = document.getElementById('<%= TVTemplate.ClientId %>');
            var chks;
            var cntCheck = 0;

            if (chkDup != null && typeof (chkDup) != 'undefined') {
                chks = chkDup.getElementsByTagName('input');
                for (i = 0; i < chks.length; i++) {
                    if (chks[i].type.toUpperCase() == 'CHECKBOX' && chks[i].checked) {
                        cntCheck = cntCheck + 1;
                    }
                    if (cntCheck > 1) {
                        msgalert('Please Select Only One Activity !');
                        return false;
                    }
                }
                if (cntCheck == 0) {
                    msgalert('Please Select One Activity !');
                    return false;
                }
            }

            return true;
        }

        function CheckDelete() {

            var chkDup = document.getElementById('<%= TVTemplate.ClientId %>');
            var chks;
            var cntCheck = 0;

            if (chkDup != null && typeof (chkDup) != 'undefined') {
                chks = chkDup.getElementsByTagName('input');
                for (i = 0; i < chks.length; i++) {
                    if (chks[i].type.toUpperCase() == 'CHECKBOX' && chks[i].checked) {
                        cntCheck = cntCheck + 1;
                    }
                    //                    if (cntCheck > 1) {
                    //                        alert('Please Select Only One Activity');
                    //                        return false;
                    //                    }
                }
                if (cntCheck == 0) {
                    msgalert('Please Select Atleast One Activity To Delete !');
                    return false;
                }
            }

            return true;
        }


    </script>

</asp:Content>
