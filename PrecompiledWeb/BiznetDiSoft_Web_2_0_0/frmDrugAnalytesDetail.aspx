<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmDrugAnalyseDetail, App_Web_22suyskz" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <table style="width: 100%;">
        <tr>
            <td colspan="2" style="width: 100%;">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="1">
                            <cc1:TabPanel runat="server" ID="TabPanel1" HeaderText="TabPanel1">
                                <ContentTemplate>
                                    <table style="width: 100%; margin-bottom: 2%" cellpadding="5px">
                                        <tbody>
                                            <tr>
                                                <td class="Label" style="width: 40%; text-align: right;">
                                                    Drug Name :
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:DropDownList ID="SlcDrugEdit" runat="server" CssClass="dropDownList" AutoPostBack="True"
                                                        Width="35%" />
                                                    <asp:Button ID="BtnGo" runat="server" Text="" ToolTip="Go" CssClass="btn btngo"
                                                        Visible="False" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <asp:GridView ID="GV_Detail" runat="server" SkinID="grdViewAutoSizeMax" style="width:60%; margin:auto;" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:TemplateField HeaderText="#">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblSrNo" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="vDrugAnalytesCode" HeaderText="DrugAnalytes Code" />
                                            <asp:BoundField DataField="vDrugCode" HeaderText="Drug Code" />
                                            <asp:BoundField DataField="vDrugName" HeaderText="Drug Name" />
                                            <asp:BoundField DataField="vAnalytes" HeaderText="Analytes" />
                                            <asp:BoundField DataField="vLinearityRange" HeaderText="LinearityRange" />
                                            <asp:BoundField DataField="vAnalysisMethod" HeaderText="AnalysisMethod" />
                                            <asp:TemplateField HeaderText="Edit">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImbEdit" runat="server" ImageUrl="~/images/Edit2.gif" ToolTip="Edit" />
                                                    <%--<asp:ImageButton ID="ImbUpdate" runat="server" ImageUrl="~/images/save.gif" Width="16px" />
                                <asp:ImageButton ID="ImbCancel" runat="server" ImageUrl="~/images/cancel.gif" />--%>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImbDelete" runat="server" ImageUrl="~/Images/i_delete.gif" OnClientClick="return msgconfirmalert('Are you sure you want to Delete?',this);" 
                                                        ToolTip="Delete" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                    <asp:Button ID="Button1" runat="server" Text="Add >>" ToolTip="Add" CssClass="btn btnadd" />
                                </ContentTemplate>
                                <HeaderTemplate>
                                    Edit Delete
                                </HeaderTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" ID="TabPanel2" HeaderText="TabPanel2">
                                <ContentTemplate>
                                    <table style="text-align: center; width: 100%;" cellpadding="5px">
                                        <tbody>
                                            <tr>
                                                <td style="width: 40%; text-align: right;" class="Label">
                                                    Drug Name* :
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:DropDownList ID="SlcDrug" runat="server" CssClass="dropDownList" Width="30%" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Label" style="text-align: right;">
                                                    Analytes* :
                                                </td>
                                                <td style="text-align: left;">
                                                    <textarea style="width: 29%" id="Txtanalytes" runat="server" rows="2" cols="0" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Label" style="text-align: right;">
                                                    LinearityRange :
                                                </td>
                                                <td style="text-align: left;">
                                                    <textarea style="width: 29%;" id="TxtLinearrange" runat="server" rows="2" cols="0"  />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Label" style="text-align: right;">
                                                    AnalysisMethod
                                                </td>
                                                <td style="text-align: left;">
                                                    <textarea style="width: 29%" id="txtAnalysismethod" runat="server" cols="0" rows="2" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Label" style="text-align: right;">
                                                    Comments :
                                                </td>
                                                <td style="text-align: left;">
                                                    <textarea style="width: 29%" id="TxtComments" runat="server" cols="0" rows="2" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: center" colspan="2">
                                                    <asp:Button ID="BtnSave" runat="server" Text="Save" ToolTip="Save" CssClass="btn btnsave"
                                                        OnClientClick="return ValidationToSave();" />
                                                    <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Text="Cancel"
                                                        ToolTip="Cancel" CssClass="btn btncancel" />
                                                    <asp:Button ID="BtnExit" runat="server" Text="Exit" ToolTip=" Exit" CssClass="btn btnexit"
                                                        OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </ContentTemplate>
                                <HeaderTemplate>
                                    Add</HeaderTemplate>
                            </cc1:TabPanel>
                        </cc1:TabContainer><br />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <table align="center" style="margin-top: 2%; width: 100%">
        <tr>
            <td align="center" style="width: 50%;">
                <asp:HyperLink ID="hlnkBack" runat="server" NavigateUrl="~/frmDrugRegionMatrix.aspx?Mode=1">Back</asp:HyperLink>
            </td>
            <td align="center">
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/frmDrugRegionPKMatrix.aspx?Mode=1">Drug Region Analytes PK Detail</asp:HyperLink>
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">
        function ValidationToSave() {
            if (document.getElementById('<%=SlcDrug.clientid %>').selectedIndex == 0) {
                msgalert('Please Select Drug');
                return false;
            }

            else if (document.getElementById('<%=Txtanalytes.clientid %>').value.toString().replace(/^\s*/, '').replace(/\s*$/, '').length <= 0) {
                document.getElementById('<%=Txtanalytes.clientid %>').value = ''
                document.getElementById('<%=Txtanalytes.clientid %>').focus();
                msgalert('Please Enter Analytes');
                return false;
            }

            return true;
        }

    </script>

</asp:Content>
