<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmClientContactMatrix.aspx.vb" Inherits="frmClientContactMatrix"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%" cellpadding="5px">
                <tbody>
                    <tr>
                        <td class="Label" style="width: 25%; text-align: right; vertical-align: top;">
                            Client Name* :
                        </td>
                        <td style="width: 20%; text-align: left; vertical-align: top;">
                            <asp:DropDownList ID="DDLClient" runat="server" CssClass="dropDownList" Width="100%"
                                OnSelectedIndexChanged="DDLClient_SelectedIndexChanged" AutoPostBack="True" />
                        </td>
                    </tr>
                    <tr>
                        <td class="Label" style="text-align: right; vertical-align: top;">
                            Contact Person Name* :
                        </td>
                        <td style="text-align: left; vertical-align: top;">
                            <asp:TextBox ID="txtContactPerson" TabIndex="1" runat="server" CssClass="textBox"
                                Width="100%" MaxLength="200" />
                        </td>
                        <td class="Label" style="width: 18%; vertical-align: top; text-align: right;">
                            Designation :
                        </td>
                        <td style="text-align: left; vertical-align: top;">
                            <asp:TextBox ID="txtDesignation" TabIndex="2" runat="server" CssClass="textBox" Width="40%"
                                MaxLength="200" />
                        </td>
                    </tr>
                    <tr>
                        <td class="Label" style="text-align: right; vertical-align: top;">
                            Address1 :
                        </td>
                        <td style="text-align: left; vertical-align: top;">
                            <asp:TextBox ID="txtAddr1" TabIndex="3" runat="server" CssClass="textBox" Width="100%"
                                TextMode="MultiLine" MaxLength="200" />
                        </td>
                        <td class="Label" style="text-align: right; vertical-align: top;">
                            Telephone No. :
                        </td>
                        <td style="text-align: left; vertical-align: top;">
                            <asp:TextBox ID="txtTelNo" TabIndex="7" runat="server" CssClass="textBox" Width="30%"
                                MaxLength="20" />
                        </td>
                    </tr>
                    <tr>
                        <td class="Label" style="text-align: right; vertical-align: top;">
                            Address2 :
                        </td>
                        <td style="text-align: left; vertical-align: top;">
                            <asp:TextBox ID="txtAddr2" TabIndex="4" runat="server" CssClass="textBox" Width="100%"
                                TextMode="MultiLine" MaxLength="200" />
                        </td>
                        <td class="Label" style="text-align: right; vertical-align: top;">
                            Ext. No. :
                        </td>
                        <td style="text-align: left; vertical-align: top;">
                            <asp:TextBox ID="txtExtNo" TabIndex="8" MaxLength="20" runat="server" CssClass="textBox"
                                Width="15%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="Label" style="text-align: right; vertical-align: top;">
                            Address3 :
                        </td>
                        <td style="text-align: left; vertical-align: top;">
                            <asp:TextBox ID="txtAddr3" TabIndex="5" runat="server" CssClass="textBox" Width="100%"
                                TextMode="MultiLine" MaxLength="200" />
                        </td>
                        <td class="Label" style="text-align: right; vertical-align: top;">
                            Fax No. :
                        </td>
                        <td style="text-align: left; vertical-align: top;">
                            <asp:TextBox ID="txtFaxNo" TabIndex="9" runat="server" CssClass="textBox" Width="30%"
                                MaxLength="20" />
                        </td>
                    </tr>
                    <tr>
                        <td class="Label" style="text-align: right; vertical-align: top;">
                            Email-Id :
                        </td>
                        <td style="text-align: left; vertical-align: top;">
                            <asp:TextBox ID="txtEmailId" TabIndex="6" runat="server" CssClass="textBox" Width="100%"
                                MaxLength="100" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Width="305px"
                                ErrorMessage="Please Enter Email Id in Correct Format." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                SetFocusOnError="True" ControlToValidate="txtEmailId" Display="Dynamic" ValidationGroup="B">
                                </asp:RegularExpressionValidator>
                        </td>
                        <td class="Label" style="text-align: right; vertical-align: top;">
                            Active :
                        </td>
                        <td style="text-align: left; vertical-align: top;">
                            <asp:CheckBox ID="ChkActive" TabIndex="10" runat="server" Checked="True" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="text-align: center; vertical-align: top;">
                            <asp:Button ID="BtnAdd" TabIndex="11" runat="server" Text="Save" ToolTip="Save" CssClass="btn btnsave"
                                OnClientClick="return Validation();" />
                            <asp:Button ID="btnCancel" TabIndex="12" OnClick="btnCancel_Click" runat="server"
                                Text="Cancel" ToolTip="Cancel" CssClass="btn btncancel" CausesValidation="False" />
                            <asp:Button ID="btnClose" TabIndex="13" runat="server" Text="Exit" ToolTip="Exit"
                                CssClass="btn btnclose" CausesValidation="False" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <br />
    <table style="width: 100%">
        <tr>
            <td style="width: 100%">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvclient" runat="server" SkinID="grdViewAutoSizeMax" style="width:65%; margin:auto;" OnRowDeleting="gvclient_RowDeleting"
                            OnRowCommand="gvclient_RowCommand" OnRowDataBound="gvclient_RowDataBound" AutoGenerateColumns="False"
                            OnRowCreated="gvclient_RowCreated">
                            <Columns>
                                <asp:BoundField DataFormatString="number" HeaderText="#">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="vClientContactCode" HeaderText="Client Contact Code">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="vClientCode" HeaderText="Client Code">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="vClientName" HeaderText="ClientName">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="vContactName" HeaderText="Contact Name">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="vDesignation" HeaderText="Designation">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="vAddress1" HeaderText="Address 1">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="vAddress2" HeaderText="Address 2">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="vAddress3" HeaderText="Address 3">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="vTelephoneNo" HeaderText="Telephone No">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="vFaxNo" HeaderText="Fax No" />
                                <asp:BoundField DataField="vExtNo" HeaderText="Ext No">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="vEmailId" HeaderText="Email Id">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="dModifyOn" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Modify On"
                                    HtmlEncode="False">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="cActiveflag" HeaderText="Active" />
                                <asp:TemplateField HeaderText="Edit" SortExpression="status">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="lnkEdit" runat="server" ImageUrl="~/images/Edit2.gif" ToolTip="Edit" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete" Visible="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImbADelete" runat="server" ToolTip="Delete" ImageUrl="~/Images/i_delete.gif" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="BtnAdd" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="DDLClient" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">

        function Validation() {
            if (document.getElementById('<%=DDLClient.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Client Name !');
                return false;
            }
           else if  (document.getElementById('<%=txtContactPerson.ClientID%>').value.toString().replace(/^\s*/, '').replace(/\s*$/, '').length <= 0) {
                msgalert('Please Enter Contact Person Name !');
                document.getElementById('<%=txtContactPerson.ClientID%>').value = '';
                document.getElementById('<%=txtContactPerson.ClientID%>').focus();
                return false;
            }
        }
        function ShowAlert(msg) {
            //alert(msg);
            //window.location.href = "frmClientContactMatrix.aspx?mode=1";
            alertdooperation(msg, 1, "frmClientContactMatrix.aspx?mode=1");
        }
       
    </script>

</asp:Content>
