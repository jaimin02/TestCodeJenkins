<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmcountryMst, App_Web_qa4vhgvp" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" Runat="Server">


    <script type="text/javascript" language="javascript"></script>

    <script type="text/javascript" src="Script/Validation.js"></script>
    
    
    <script type="text/javascript">
        function Validation() {
            if (document.getElementById('<%=txtCountryName.ClientID%>').value == '') {
                msgalert('Please Enter Country Name !');
                return false;
            }
            else if (document.getElementById('<%=txtCountryCode.ClientID%>').value == '') {
                msgalert('Please Enter CountryCode !');
                return false;
            }
            else if (document.getElementById('<%=ddlRegion.ClientID%>').selectedIndex <= 0) {
                msgalert('Please Select Region !');
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="width: 80%; text-align: left">
                <tr>
                    <td align="right" class="Label" style="white-space: nowrap; height: 22px">
                        Country Name
                    </td>
                    <td align="left" style="width: 50%; height: 22px">
                        <asp:TextBox ID="txtCountryName" runat="server" CssClass="textBox" MaxLength="100"
                            TabIndex="1" Width="148px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="Label" style="width: 25%; white-space: nowrap">
                        Country Code
                    </td>
                    <td align="left" style="width: 25%">
                        <asp:TextBox ID="txtCountryCode" runat="server" CssClass="textBox" MaxLength="5"
                            TabIndex="2" Width="148px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="Label" style="width: 25%; white-space: nowrap">
                        Region Name
                    </td>
                    <td align="left" style="width: 25%">
                        <asp:DropDownList ID="ddlRegion" runat="server" CssClass="dropDownList" TabIndex="3"
                            Width="152px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="Label" style="display: none; width: 25%">
                        ActiveFlag
                    </td>
                    <td align="left" style="display: none; width: 25%">
                        <asp:CheckBox ID="chkactive" runat="server" Checked="True" CssClass="checkBoxList"
                            TabIndex="9" />
                    </td>
                </tr>
                <tr>
                    <td align="right" style="height: 26px">
                        <asp:Button ID="BtnSave" runat="server" CssClass="btn btnsave" OnClick="BtnSave_Click"
                            TabIndex="4" Text="Save" />
                    </td>
                    <td align="left" style="width: 25%; height: 26px">
                        <asp:Button ID="BtnExit" runat="server" CssClass="btn btnexit" OnClick="BtnExit_Click" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this); "
                            TabIndex="5" Text="Exit" />
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%">
                        <asp:Button ID="btnupdate" runat="server" CssClass="btn btnnew" OnClick="btnupdate_Click"
                            TabIndex="6" Text="Update" Visible="False" />
                    </td>
                    <td align="left" style="width: 25%">
                        &nbsp;<asp:Button ID="btncancel" runat="server" CssClass="btn btncancel" OnClick="btncancel_Click"
                            TabIndex="7" Text="Cancel" Visible="False" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:GridView ID="gvwCountryMst" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            OnPageIndexChanging="gvwCountryMst_PageIndexChanging" OnRowCommand="gvwCountryMst_RowCommand"
                            OnRowDataBound="gvwCountryMst_RowDataBound" OnRowDeleting="gvwCountryMst_RowDeleting"
                            PageSize="50" SkinID="grdViewSml">
                            <Columns>
                                <asp:BoundField DataFormatString="number" HeaderText="#">
                                    <ItemStyle HorizontalAlign="Right" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField DataField="vCountryId" HeaderText="Country No" />
                                <asp:BoundField DataField="vCountryCode" HeaderText="Contry Code" />
                                <asp:BoundField DataField="vCountryName" HeaderText="Contry Name" />
                                <asp:BoundField DataField="vRegionName" HeaderText="Region" />
                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImbEdit" runat="server" ImageUrl="~/images/Edit2.gif" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImbDelete" runat="server" ImageUrl="~/Images/i_delete.gif" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                       
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

