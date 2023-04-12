<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmDrugRegionMatrix.aspx.vb" Inherits="frmDrugRegionMatrix"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="1">
        <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="TabPanel1">
            <ContentTemplate>
                <table style="width: 100%; margin-bottom: 1%" cellpadding="5px">
                    <tr>
                        <td class="Label" style="width: 40%; text-align: right;">
                            Drug Name :
                        </td>
                        <td style="text-align: left;">
                            <select id="SlcDrugEdit" runat="server" class="dropDownList" style="width: 35%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;" class="Label">
                            Region Name :
                        </td>
                        <td style="text-align: left;">
                            <select id="SlcRegionEdit" runat="server" class="dropDownList" style="width: 35%" />
                            <asp:Button ID="BtnGo" runat="server" OnClientClick="return Validation();" CssClass="btn btngo"
                                Text="" ToolTip="GO"/>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="GV_Detail" runat="server" AutoGenerateColumns="False" SkinID="grdViewAutoSizeMax" style="width:60%; margin:auto;"
                    AllowPaging="True" PageSize="10" ShowFooter="true">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:Label ID="LblSrNo" runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="vDrugRegionCode" HeaderText="DrugRegion Code" />
                        <asp:BoundField DataField="vDrugCode" HeaderText="Drug Code" />
                        <asp:BoundField DataField="vDrugName" HeaderText="Drug Name" />
                        <asp:BoundField DataField="vRegionCode" HeaderText="Region Code" />
                        <asp:BoundField DataField="vRegionName" HeaderText="Region Name" />
                        <asp:BoundField DataField="vRLDDetails" HeaderText="RLD Details" />
                        <asp:BoundField DataField="vManufacturer" HeaderText="Manufacturer" />
                        <asp:BoundField DataField="cActiveFlag" HeaderText="Active">
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
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
                                <asp:ImageButton ID="ImbDelete" runat="server" ImageUrl="~/Images/i_delete.gif" 
                                    OnClientClick="return msgconfirmalert('Are you sure you want to Delete?',this);" ToolTip="Delete" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Button ID="Button1" runat="server" CssClass="button" Text="Add >>" ToolTip="Add" />
            </ContentTemplate>
            <HeaderTemplate>
                Edit Delete
            </HeaderTemplate>
        </cc1:TabPanel>
        <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="TabPanel2">
            <ContentTemplate>
                <table align="center" width="100%" cellpadding="5px">
                    <tr>
                        <td align="right" class="Label" style="width: 40%">
                            Drug Name* :
                        </td>
                        <%--<td align="left" style="height: 21px">--%>
                        <td style="text-align: left;">
                            <select id="SlcDrug" runat="server" class="dropDownList" style="width: 30%">
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;" class="Label">
                            Region* :
                        </td>
                        <td style="text-align: left;">
                            <select id="SlcRegion" runat="server" class="dropDownList" style="width: 30%">
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;" class="Label">
                            Enter RLD* :
                        </td>
                        <td style="text-align: left;">
                            <textarea id="TxtRLD" runat="server" style="width: 29%" rows="2" cols="0" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;" class="Label">
                            Marketing Authorization Holder :
                        </td>
                        <td style="text-align: left;">
                            <textarea id="txtMfg" runat="server" style="width: 29%" rows="2" cols="0" />
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td style="text-align: right;" class="Label">
                            Active :
                        </td>
                        <td style="text-align: left;">
                            <asp:CheckBox ID="ChkActive" runat="server" Checked="True" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;" colspan="2">
                            <asp:Button ID="BtnSave" runat="server" CssClass="btn btnsave" Text="Save" ToolTip="Save"
                                OnClientClick="return ValidationToSave();" />
                            <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" Text="Cancel" ToolTip="Cancel" />
                            <asp:Button ID="BtnExit" runat="server" CssClass="btn btnexit" Text="Exit" ToolTip="Exit"
                                OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <HeaderTemplate>
                Add
            </HeaderTemplate>
        </cc1:TabPanel>
    </cc1:TabContainer>
    <table align="center" style="margin-top: 2%; width: 100%">
        <tr>
            <td style="width: 50%; text-align: center;">
                <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/frmDrugMst.aspx?Mode=1">Back</asp:HyperLink>
            </td>
            <td style="text-align: center;">
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/frmDrugAnalytesDetail.aspx?mode=1">Drug Analytes Detail</asp:HyperLink>
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">
        function ValidationToSave() {
            if (document.getElementById('<%=SlcDrug.clientid %>').selectedIndex == 0) {
                document.getElementById('<%=SlcDrug.clientid %>').focus();
                 msgalert('Please Select Drug');
                return false;
            }
            else if (document.getElementById('<%=SlcRegion.clientid %>').selectedIndex == 0) {
                msgalert('Please Select Region');
                document.getElementById('<%=SlcRegion.clientid %>').focus();
                return false;
            }
            else if (document.getElementById('<%=TxtRLD.ClientID%>').value.toString().replace(/^\s*/, '').replace(/\s*$/, '').length <= 0) {
                document.getElementById('<%=TxtRLD.ClientID%>').value = '';
                document.getElementById('<%=TxtRLD.ClientID%>').focus();
                msgalert('Please Enter RLD');
                return false;
            }

            return true;
        }

        function Validation() {
            if (document.getElementById('<%=SlcDrugEdit.clientid %>').selectedIndex == 0) {
                document.getElementById('<%=SlcDrugEdit.clientid %>').focus();
                msgalert('Please Select Drug');
                return false;
            }

            else if (document.getElementById('<%=SlcRegionEdit.clientid %>').selectedIndex == 0) {
                msgalert('Please Select Region');
                document.getElementById('<%=SlcRegionEdit.clientid %>').focus();
                return false;
            }
            //       function ShowAlert(msg) {
            //                alert(msg);
            //                //            window.location.href = "frmDrugRegionMatrix.aspx?mode=1";
            //            }

            return true;
        }
        
        
    </script>

</asp:Content>
