<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmDefaultDocumentUserRights, App_Web_xjkmyygy" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <asp:UpdatePanel ID="Up_General" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <table width="100%">
                <tbody>
                    <tr>
                        <td>
                            <table width="100%" cellpadding="3px">
                                <tbody>
                                    <tr>
                                        <td style="text-align: center;" colspan="2">
                                            <asp:Label ID="lblInformation" runat="server" CssClass="Label" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Label" style="text-align: right; width: 40%;">
                                            User Group :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="DDLUserGroup" runat="server" CssClass="dropDownList" AutoPostBack="True"
                                                Width="30%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Label" style="text-align: right;">
                                            User Type:
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="DdlUserType" runat="server" CssClass="dropDownList" Width="30%"
                                                AutoPostBack="True" OnSelectedIndexChanged="DdlUserType_SelectedIndexChanged" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Label" style="text-align: right;">
                                            Location:
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="DdlLocation" runat="server" CssClass="dropDownList" Width="30%"
                                                AutoPostBack="True" OnSelectedIndexChanged="DdlLocation_SelectedIndexChanged" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="Up_UserStages" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table width="100%">
                                                        <tbody>
                                                            <tr>
                                                                <td class="Label" style="width: 50%; text-align: right;">
                                                                    Users
                                                                </td>
                                                                <td class="Label" style="text-align: left;">
                                                                    Stages
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">
                                                                    <div style="border-right: gray thin solid; text-align: left; border-top: gray thin solid;
                                                                        overflow-y: scroll; border-left: gray thin solid; width: 35%; border-bottom: gray thin solid;
                                                                        height: 80px;" id="Div1" class="Div ">
                                                                        <asp:CheckBoxList ID="chklstUser" runat="server" ForeColor="Black" Font-Size="Small"
                                                                            Font-Names="Verdana" CssClass="checkboxlist" Font-Name="Verdana" />
                                                                    </div>
                                                                </td>
                                                                <td>
                                                                    <div style="border-right: gray thin solid; border-top: gray thin solid; overflow-y: scroll;
                                                                        border-left: gray thin solid; width: 35%; border-bottom: gray thin solid; height: 80px;
                                                                        text-align: left;" id="Div2" class="Div ">
                                                                        <asp:CheckBoxList ID="chklstStages" runat="server" ForeColor="Black" Font-Size="Small"
                                                                            Font-Names="Verdana" CssClass="checkboxlist" Font-Name="Verdana" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="DDLUserGroup" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center;" colspan="2">
                                            <asp:Button ID="btnSave" runat="server" Text="Save" ToolTip="Save" CssClass="btn btnsave"
                                                OnClientClick="return Validation();" />
                                            <asp:Button ID="BtnExit" runat="server" Text="" ToolTip="Back" CssClass="btn btnback" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="GV_UserStage_Edit" runat="server" SkinID="grdViewAutoSizeMax" style="width:60%; margin:auto;" AutoGenerateColumns="False"
                                        AllowPaging="true" PageSize="20" ShowFooter="true">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CHKDelete" runat="Server"></asp:CheckBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="nTemplateDefaultWorkflowUserId" HeaderText="TemplateDefaultWorkflowUserId">
                                            </asp:BoundField>
                                            <asp:BoundField DataField="vTemplateId" HeaderText="TemplateId"></asp:BoundField>
                                            <asp:BoundField DataField="vTemplateDesc" HeaderText="TemplateName"></asp:BoundField>
                                            <asp:BoundField DataField="iUserId" HeaderText="UserId"></asp:BoundField>
                                            <asp:BoundField DataField="vUserName" HeaderText="UserName"></asp:BoundField>
                                            <asp:BoundField DataField="vUserTypeName" HeaderText="Profile"></asp:BoundField>
                                            <asp:BoundField DataField="iStageId" HeaderText="StageId"></asp:BoundField>
                                            <asp:BoundField DataField="vStageDesc" HeaderText="StageName"></asp:BoundField>
                                            <asp:TemplateField HeaderText="New Stage">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DDLStages" runat="server" Width="132px" CssClass="dropDownList">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Edit" SortExpression="status">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgEdit" runat="server" ToolTip="Edit" ImageUrl="~/images/Edit2.gif">
                                                    </asp:ImageButton>
                                                    <asp:ImageButton ID="ImgSave" runat="server" ToolTip="Save" ImageUrl="~/images/save.gif">
                                                    </asp:ImageButton>
                                                    <asp:ImageButton ID="ImgCancel" runat="server" ToolTip="Cancel" ImageUrl="~/images/Cancel.gif">
                                                    </asp:ImageButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete" SortExpression="status">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgDelete" runat="server" ToolTip="Delete" ImageUrl="~/images/i_delete.gif"
                                                        OnClientClick="return confirm('Are You Sure You want To Delete?')"></asp:ImageButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False"></ItemStyle>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:Button ID="BtnDelete" OnClick="BtnDelete_Click" runat="server" Text="Delete"
                                        ToolTip="Delete" CssClass="btn btncancel" OnClientClick="return msgconfirmalert('Are you sure you want to Delete?',this);">
                                    </asp:Button>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click"></asp:AsyncPostBackTrigger>
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        function SelectAll(CheckBoxControl) {
            if (CheckBoxControl.checked == true) {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {

                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf('GV_UserStage') > -1)) {
                        if (document.forms[0].elements[i].disabled == false) {
                            document.forms[0].elements[i].checked = true;
                        }


                    }


                }

            }

            else {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf('GV_UserStage') > -1)) {
                        document.forms[0].elements[i].checked = false;
                    }

                }
            }
        }

        function Validation() {
            var chklstUser = document.getElementById('<%=chklstUser.clientid%>');
            var chklstStage = document.getElementById('<%=chklstStages.clientid%>');
            var chks;
            var result = false;
            var i;

            if (document.getElementById('<%=DDLUserGroup.clientid %>').selectedIndex == 0) {
                msgalert('Please Select UserGroup !');
                return false;
            }

            if (chklstUser != null && typeof (chklstUser) != 'undefined') {
                chks = chklstUser.getElementsByTagName('input');
                for (i = 0; i < chks.length; i++) {
                    if (chks[i].type.toUpperCase() == 'CHECKBOX' && chks[i].checked) {
                        result = true;
                        break;
                    }
                }
                if (!result) {
                    msgalert('Please Select Atleast One User !');
                    return false;
                }
            }

            if (chklstStage != null && typeof (chklstStage) != 'undefined') {
                result = false;
                chks = chklstStage.getElementsByTagName('input');
                for (i = 0; i < chks.length; i++) {
                    if (chks[i].type.toUpperCase() == 'CHECKBOX' && chks[i].checked) {
                        result = true;
                        break;
                    }
                }
                if (!result) {
                    msgalert('Please Select Atleast One Stage !');
                    return false;
                }
            }

        }
    </script>

</asp:Content>
