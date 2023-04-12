<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmWorkspaceUserRights, App_Web_5xoe1jh1" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="ConWorkspaceUserRights" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script language="javascript">
        function CheckListBox() {
            var chklstUser = document.getElementById('<%=chklstUser.clientid%>');
    var chksUser;
    var resultUser = false;
    var i;

    if (chklstUser != null && typeof (chklstUser) != 'undefined') {
        chksUser = chklstUser.getElementsByTagName('input');
        for (i = 0; i < chksUser.length; i++) {
            if (chksUser[i].type.toUpperCase() == 'CHECKBOX' && chksUser[i].checked) {
                resultUser = true;
                break;
            }
        }
    }
    if (!resultUser) {
        msgalert('Please Select Atleast One User !');
        return false;
    }

    var chklstStage = document.getElementById('<%=chklstStages.clientid%>');
        var chksStage;
        var resultStage = false;

        if (chklstStage != null && typeof (chklstStage) != 'undefined') {
            chksStage = chklstStage.getElementsByTagName('input');
            for (i = 0; i < chksStage.length; i++) {
                if (chksStage[i].type.toUpperCase() == 'CHECKBOX' && chksStage[i].checked) {
                    resultStage = true;
                    break;
                }
            }
        }
        if (!resultStage) {
            msgalert('Please Select Atleast One Stage !');
            return false;
        }
        return true;
    }
    </script>

    <table>
        <tr>
            <td>
                <table style="width: 681px">
                    <tr>
                        <td align="right" class="Label">Operation Type : &nbsp;</td>
                        <td align="left" class="Label">
                            <asp:RadioButton ID="RbAdd" runat="server" Text="Add/Delete" AutoPostBack="True"
                                GroupName="A" ValidationGroup="a" />
                            <asp:RadioButton ID="RbEdit" runat="server" Text="Edit Stage" AutoPostBack="True"
                                GroupName="A" ValidationGroup="a" />
                            <asp:RadioButton ID="RbDelete" runat="server" Text="Delete" AutoPostBack="True" GroupName="A"
                                ValidationGroup="a" Visible="False" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <br />
                <asp:UpdatePanel ID="Up_General" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div style="WIDTH: 100px" id="DivAdd" runat="server">
                            <table align="center">
                                <tbody>
                                    <tr>
                                        <td align="center">
                                            <table style="WIDTH: 319px">
                                                <tbody>
                                                    <tr>
                                                        <td class="Label">User Group :</td>
                                                        <td>
                                                            <asp:DropDownList ID="DDLUserGroup" runat="server" CssClass="dropDownList" AutoPostBack="True" __designer:wfdid="w196">
                                                            </asp:DropDownList></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                            <input id="HdfTempName" type="hidden" runat="server" />
                                            <input id="HdfNodeName" type="hidden" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:UpdatePanel ID="Up_UserStages" runat="server" UpdateMode="Conditional" RenderMode="Inline" __designer:wfdid="w197">
                                                <ContentTemplate>
                                                    <table>
                                                        <tbody>
                                                            <tr>
                                                                <td class="Label">Users</td>
                                                                <td class="Label">Stages</td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <div style="BORDER-RIGHT: gray thin solid; BORDER-TOP: gray thin solid; OVERFLOW-Y: scroll; BORDER-LEFT: gray thin solid; WIDTH: 170px; BORDER-BOTTOM: gray thin solid; HEIGHT: 80px" id="Div1">
                                                                        <asp:CheckBoxList ID="chklstUser" runat="server" ForeColor="Black" Font-Size="XX-Small" Font-Names="Verdana" CssClass="checkboxlist" Width="150" Font-Name="Verdana" Height="37px" __designer:wfdid="w198"></asp:CheckBoxList></div>
                                                                </td>
                                                                <td>
                                                                    <div style="BORDER-RIGHT: gray thin solid; BORDER-TOP: gray thin solid; OVERFLOW-Y: scroll; BORDER-LEFT: gray thin solid; WIDTH: 170px; BORDER-BOTTOM: gray thin solid; HEIGHT: 80px" id="Div2">
                                                                        <asp:CheckBoxList ID="chklstStages" runat="server" ForeColor="Black" Font-Size="XX-Small" Font-Names="Verdana" CssClass="checkboxlist" Width="150" Font-Name="Verdana" Height="37px" __designer:wfdid="w199">
                                                                        </asp:CheckBoxList>
                                                                    </div>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="BtnAdd" runat="server" Text="Add" CssClass="btn btnnew" Width="51px" __designer:wfdid="w200" OnClientClick="return CheckListBox();"></asp:Button></td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="DDLUserGroup" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                                                    <asp:AsyncPostBackTrigger ControlID="RbAdd" EventName="CheckedChanged"></asp:AsyncPostBackTrigger>
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <table style="WIDTH: 347px">
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline" __designer:wfdid="w201">
                                                                <ContentTemplate>
                                                                    <asp:GridView ID="GV_UserStage" runat="server" SkinID="grdView" AutoGenerateColumns="False" OnRowDataBound="GV_UserStage_RowDataBound" OnRowDeleting="GV_UserStage_RowDeleting" OnRowCommand="GV_UserStage_RowCommand" __designer:wfdid="w202">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="vWorkSpaceId" HeaderText="WorkSpaceId"></asp:BoundField>
                                                                            <asp:BoundField DataField="vWorkSpaceDesc" HeaderText="WorkSpaceName"></asp:BoundField>
                                                                            <asp:BoundField DataField="iNodeId" HeaderText="NodeId"></asp:BoundField>
                                                                            <asp:BoundField DataField="vNodeDisplayName" HeaderText="NodeName"></asp:BoundField>
                                                                            <asp:BoundField DataField="iUserId" HeaderText="UserId"></asp:BoundField>
                                                                            <asp:BoundField DataField="vUserName" HeaderText="UserName"></asp:BoundField>
                                                                            <asp:BoundField DataField="iStageId" HeaderText="StageId"></asp:BoundField>
                                                                            <asp:BoundField DataField="vStageDesc" HeaderText="StageName"></asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Delete">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="ImbMDelete" runat="server" ImageUrl="~/Images/i_delete.gif" OnClientClick="return msgconfirmalert('Are You sure You want to DELETE?',this)"></asp:ImageButton>

                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btnsave" __designer:wfdid="w203"></asp:Button>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="BtnAdd" EventName="Click"></asp:AsyncPostBackTrigger>
                                                                    <asp:AsyncPostBackTrigger ControlID="RbAdd" EventName="CheckedChanged"></asp:AsyncPostBackTrigger>
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div style="WIDTH: 100px" id="DivEdit" runat="server">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" RenderMode="Inline" __designer:wfdid="w204">
                                <ContentTemplate>
                                    <asp:GridView ID="GV_UserStage_Edit" runat="server" SkinID="grdView" AutoGenerateColumns="False" OnRowDataBound="GV_UserStage_Edit_RowDataBound" OnRowCommand="GV_UserStage_Edit_RowCommand" OnRowCreated="GV_UserStage_Edit_RowCreated" OnRowUpdating="GV_UserStage_Edit_RowUpdating" OnRowEditing="GV_UserStage_Edit_RowEditing" __designer:wfdid="w205">
                                        <Columns>
                                            <asp:BoundField DataField="vWorkSpaceId" HeaderText="WorkSpaceId"></asp:BoundField>
                                            <asp:BoundField DataField="vWorkSpaceDesc" HeaderText="WorkSpaceName"></asp:BoundField>
                                            <asp:BoundField DataField="iNodeId" HeaderText="NodeId"></asp:BoundField>
                                            <asp:BoundField DataField="vNodeDisplayName" HeaderText="NodeName"></asp:BoundField>
                                            <asp:BoundField DataField="iUserId" HeaderText="UserId"></asp:BoundField>
                                            <asp:BoundField DataField="vUserName" HeaderText="UserName"></asp:BoundField>
                                            <asp:BoundField DataField="iStageId" HeaderText="StageId"></asp:BoundField>
                                            <asp:BoundField DataField="vStageDesc" HeaderText="StageName"></asp:BoundField>
                                            <asp:TemplateField HeaderText="New Stage">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DDLStages" runat="server" Width="132px" CssClass="dropDownList"></asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Edit">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LnkEdit" runat="server">Edit</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:Button ID="BtnUpdate" OnClick="BtnUpdate_Click" runat="server" CssClass="btn btnsave" Text="Update" __designer:wfdid="w206"></asp:Button>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="RbEdit" EventName="CheckedChanged"></asp:AsyncPostBackTrigger>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="RbAdd" EventName="CheckedChanged"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="RbDelete" EventName="CheckedChanged"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="RbEdit" EventName="CheckedChanged"></asp:AsyncPostBackTrigger>
                    </Triggers>
                </asp:UpdatePanel>
                <asp:Button ID="BtnExit" runat="server" CssClass="btn btnback" Text="" /></td>
        </tr>
        <tr>
            <td align="center"></td>
        </tr>
    </table>
</asp:Content>
