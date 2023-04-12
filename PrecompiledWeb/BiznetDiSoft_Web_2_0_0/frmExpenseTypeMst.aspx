<%@ page title="" language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmExpenseTypeMst, App_Web_5xoe1jh1" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <center>
        <%-- /* Site ExpensePanel Starts */--%>
        <div>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:Panel runat="server" ID="SiteExpensePanel" Width="362px" Height="100px" CssClass="setsiteexpensepanel"
                        BorderColor="#000099" BorderStyle="Solid" BorderWidth="1px">
                        <table border="0" cellpadding="3" cellspacing="2">
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="LblExpenseType" Text="Expense Type"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="TxtExpenseType" Text="" Width="226px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    
                                </td>
                                <td style="text-align: left">
                                <asp:LinkButton runat="server" ID="LnkBtnSave" Text="Save" Font-Bold="True" Font-Underline="False"
                                        Width="75px" Height="18px" ForeColor="white" BorderColor="Black" CssClass="setbuttontotuneuptoleft"></asp:LinkButton>
                                        
                                    &nbsp;<asp:LinkButton runat="server" ID="LnkBtnCancel" Text="Cancel" Font-Bold="True" Font-Underline="False"
                                        Width="75px" Height="18px" ForeColor="white" CssClass="setbuttontotuneuptoright"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="LnkBtnCancel" />
                    <asp:PostBackTrigger ControlID="LnkBtnSave" />
                </Triggers>
            </asp:UpdatePanel>
            <br />
            <div>
                <asp:GridView runat="server" ID="GVW_ShowExpense" SkinID="grdViewAutoSizeMax" style="width:60%; margin:auto;" AutoGenerateColumns="False"
                    AllowPaging="True">
                    <Columns>
                        <asp:BoundField HeaderText="#">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            <HeaderStyle HorizontalAlign="right" VerticalAlign="Middle"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="nExpTypeId" HeaderText="Expense Id">
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="vExpType" HeaderText="Expense Type">
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                        </asp:BoundField>
                        <asp:TemplateField SortExpression="status" HeaderText="Edit" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="ImgEdit" ImageUrl="~/images/Edit2.gif" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="center"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="status" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="ImgDelete" ImageUrl="~\images\cancel.gif" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="center"></HeaderStyle>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </center>
</asp:Content>
