<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmSubjectSelectionCriteria.aspx.vb" Inherits="frmSubjectSelectionCriteria" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript">
        function ClientPopulated(sender, e)
        {
            ProjectClientShowing('AutoCompleteExtender1',$get('<%= txtProject.ClientId %>') );
        }
                
        function OnSelected(sender,e)
        {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
                $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>') );
        }
    </script>

    <table cellpadding="0px" width="95%">
        <tr>
            <td width="98%">
                <div class="popUpDivNoTop" style="display: block; width: 100%; left: 35px; top: 255px;"
                    visible="false" id="divIncExlGrid" runat="server">
                    <table width="100%">
                        <tbody>
                            <tr>
                                <td align="center" width="100%">
                                    <table cellpadding="2px" width="100%">
                                        <tr>
                                            <td nowrap="nowrap" align="left">
                                                <strong>Subject ID:
                                                    <asp:Label ID="lblSubjectId" runat="server" Width="210px"></asp:Label>
                                                </strong>
                                            </td>
                                            <td nowrap="nowrap" align="left">
                                                <strong>Name:
                                                    <asp:Label ID="lblFullName" runat="server" Width="210px"></asp:Label>
                                                </strong>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="left" width="100%" colspan="3">
                                    <table cellpadding="3px" width="100%">
                                        <tr>
                                            <td align="left" nowrap="nowrap" width="100%">
                                                <strong>Inclusion Criteria</strong>
                                                <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:GridView ID="gvwInclusion" runat="server" AutoGenerateColumns="False" SkinID="grdViewAutoSizeMax">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="#" />
                                                        <asp:BoundField DataField="vProtocolWorkspaceCriterienId" />
                                                        <asp:BoundField DataField="vCriterienDescription" HeaderText="Description" />
                                                        <asp:TemplateField HeaderText="Results">
                                                            <ItemTemplate>
                                                                <asp:RadioButtonList ID="rdoLstInclusionResult" runat="server" RepeatDirection="Horizontal"
                                                                    SelectedValue='<%# Bind("cResultFlag") %>'>
                                                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                                    <asp:ListItem Value="N">No</asp:ListItem>
                                                                    <asp:ListItem Value="A">N/A</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Remarks">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtRemark" CssClass="textBox" runat="server" Text='<%# Bind("vResultRemark") %>'></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="left" width="100%" colspan="3">
                                    <table cellpadding="3px" width="100%">
                                        <tr>
                                            <td align="left" nowrap="nowrap" width="100%">
                                                <strong>Exclusion Criteria</strong>
                                                <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:GridView ID="gvwExclusion" runat="server" AutoGenerateColumns="False" SkinID="grdViewAutoSizeMax">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="#" />
                                                        <asp:BoundField DataField="vProtocolWorkspaceCriterienid" />
                                                        <asp:BoundField DataField="vCriterienDescription" HeaderText="Description" />
                                                        <asp:TemplateField HeaderText="Results">
                                                            <ItemTemplate>
                                                                <asp:RadioButtonList ID="rdoLstInclusionResult" runat="server" RepeatDirection="Horizontal"
                                                                    SelectedValue='<%# Bind("cResultFlag") %>'>
                                                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                                    <asp:ListItem Value="N">No</asp:ListItem>
                                                                    <asp:ListItem Value="A">N/A</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Remarks">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtRemark" CssClass="textBox" runat="server" Text='<%# Bind("vResultRemark") %>'></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table cellpadding="5px" width="100%">
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td nowrap="nowrap">
                                                Subject is:
                                            </td>
                                            <td align="left" valign="middle" nowrap="nowrap" width="40%">
                                                <asp:RadioButtonList ID="rdoLstResult" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Selected="True" Value="Y">Suitable</asp:ListItem>
                                                    <asp:ListItem Value="N">NOT Suitable</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td nowrap="nowrap">
                                                Remark
                                            </td>
                                            <td align="left" width="90%" nowrap="nowrap">
                                                <asp:TextBox ID="txtGenRemark" CssClass="textBox" TextMode="MultiLine" runat="server"
                                                    MaxLength="100" Height="53px" Width="332px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 90%" valign="top" align="center">
                                    <div id="div4" runat="server">
                                        <asp:Button ID="btnSaveInExlDtl" runat="server" Text=" Save " CssClass="btn btnsave" />
                                        <asp:Button ID="btnCloseDiv" runat="server" Text=" Close " CssClass="btn btnclose" /></div>
                                </td>
                            </tr>
                            <tr>
                                <td height="15px">
                                    &nbsp;
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <%-- THIS TR IS FOR LEAVING SPACE UNDER DIV--%>
                    
                </div>
            </td>
        </tr>
        <%--THIS IS FOR HEADING AND HR--%>
        <tr>
            <td align="left" nowrap="nowrap" width="90%" style="height: 48px">
                <strong>CRITERIA FOR SELECTION OF SUBJECTS</strong>
                <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
            </td>
        </tr>
        <tr>
            <td>
                <%--THIS IS FOR PROJECT SEARCH --%>
                <table cellpadding="3px" id="TABLE1" width="100%">
                    <tr>
                        <td class="Label" align="left" nowrap>
                            Project Search: &nbsp; <span>
                                <asp:TextBox ID="txtProject" CssClass="textBox" runat="server" Width="325px" TabIndex="1"></asp:TextBox>
                                <asp:HiddenField ID="HProjectId" runat="server" />
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" OnClientItemSelected="OnSelected"
                                    BehaviorID="AutoCompleteExtender1" OnClientShowing="ClientPopulated" TargetControlID="txtProject"
                                    MinimumPrefixLength="1" ServiceMethod="GetProjectCompletionListWithOutSponser" UseContextKey="True"
                                    ServicePath="AutoComplete.asmx" CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                    CompletionListItemCssClass="autocomplete_listitem">
                                </cc1:AutoCompleteExtender>
                                <asp:Button ID="btnSetProject" runat="server" Text=" Set Project" Style="display: none;" />
                            </span>
                        </td>
                        <td class="Label" width="33%" nowrap>
                            Period:&nbsp;&nbsp;<asp:DropDownList ID="ddlPeriod" runat="server" Width="90px" AutoPostBack="True"
                                CssClass="dropDownList" TabIndex="2">
                            </asp:DropDownList>
                        </td>
                        <td class="Label" nowrap>
                            Date: &nbsp;
                            <asp:TextBox ID="txtDate" runat="server" CssClass="textBox" ReadOnly="True" TabIndex="3"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <%--THIS IS FOR INCLUSION CRITERIA--%>
                <table cellpadding="3px" width="100%">
                    <tr>
                        <td align="left" nowrap="nowrap" width="100%">
                            <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gvwSubjectSelection" runat="server" AutoGenerateColumns="False"
                                SkinID="grdViewAutoSizeMax" TabIndex="4">
                                <Columns>
                                    <asp:BoundField HeaderText="#" />
                                    <asp:BoundField DataField="vSubjectId" HeaderText="Subject ID" />
                                    <asp:BoundField DataField="iMySubjectNo" HeaderText="Subject No">
                                        <ItemStyle HorizontalAlign="Right" Width="10px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FullName" HeaderText="Name">
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="cSelectedFlag" HeaderText="Selected">
                                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vRemark" HeaderText="Remarks">
                                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkBtnSubSelAdd" runat="server" CommandName="Add" OnClick="lnkBtnSubSelAdd_Click">Add</asp:LinkButton>
                                            <asp:LinkButton ID="lnkBtnSubSelEdit" runat="server" CommandName="Edit">Edit</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="min-height: 10px; height: 10px;">
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
