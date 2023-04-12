<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmDrugRegionPKMatrix.aspx.vb" Inherits="frmDrugRegionPKMatrix"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" language="javascript">

        function ValidationForGo() {
            if (document.getElementById('<%=SlcDrug.clientid %>').selectedIndex == 0) {
                msgalert('Please Select Drug !');
                return false;
            }
            if (document.getElementById('<%=SlcRegion.clientid %>').selectedIndex == 0) {
                msgalert('Please Select Region !');
                return false;
            }
            return true;
        }

        function ValidationForSave() {
            if (document.getElementById('<%=txtValue.ClientID %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter CMAX Value !');
                document.getElementById('<%=txtValue.ClientID %>').value = '';
                document.getElementById('<%=txtValue.ClientID %>').focus();
                return false;
            }
            if (document.getElementById('<%=txtMaxValue.ClientID %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter TMax Value !');
                document.getElementById('<%=txtMaxValue.ClientID %>').value = '';
                document.getElementById('<%=txtMaxValue.ClientID %>').focus();
                return false;
            }
            return true;
        }
        //        function navigate() {

        //            //            window.open("frmDrugAnalytesDetail.aspx?Mode=1")
        //            window.location.href = "frmDrugAnalytesDetail.aspx?mode=1";
        //        }
    
    </script>

    <table width="100%">
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table width="100%" cellpadding ="3px">
                            <tbody>
                                <tr>
                                    <td class="Label" style="text-align: right; width: 40%;">
                                        Drug Name* :
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="SlcDrug" runat="server" CssClass="dropDownList" AutoPostBack="True"
                                            OnSelectedIndexChanged="SlcDrug_SelectedIndexChanged" Width="40%">
                                        </asp:DropDownList>
                                        <select style="width: 40%" id="SlcDrug1" class="dropDownList" runat="server" visible="false">
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Label" style="text-align: right;">
                                        Region* :
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="SlcRegion" runat="server" CssClass="dropDownList" AutoPostBack="True"
                                            OnSelectedIndexChanged="SlcRegion_SelectedIndexChanged" Width="40%">
                                        </asp:DropDownList>
                                        <select style="display: none; width: 40%" id="SlcRegion1" class="dropDownList" runat="server">
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Label" style="text-align: right;">
                                        Analytes :
                                    </td>
                                    <td style="text-align: left">
                                        <select style="width: 40%" id="ddlAnalytes" class="dropDownList" runat="server" visible="true">
                                        </select>
                                        <asp:Button ID="BtnGo" runat="server" Text="" OnClientClick="return ValidationForGo();"
                                            CssClass="btn btngo" ToolTip="Go" Width="7%"></asp:Button>
                                        <input id="HdfDrugRegion" type="hidden" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: center;">
                                        <asp:GridView ID="GV_Detail" runat="server" SkinID="grdViewAutoSizeMax" style="width:60%; margin:auto;" OnPageIndexChanging="GV_Detail_PageIndexChanging"
                                            AllowPaging="True" PageSize="25" AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblSrNo" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="vDrugAnalytesCode" HeaderText="Drug Analytes Code" />
                                                <asp:BoundField DataField="vDrugRegionCode" HeaderText="DrugRegion Code" />
                                                <asp:BoundField DataField="vDrugCode" HeaderText="Drug Code" />
                                                <asp:BoundField DataField="vDrugName" HeaderText="Drug Name" />
                                                <asp:BoundField DataField="vRegionCode" HeaderText="Region Code" />
                                                <asp:BoundField DataField="vRegionName" HeaderText="Region Name" />
                                                <asp:BoundField DataField="vAnalytes" HeaderText="Analyte" />
                                                <asp:BoundField DataField="vRLDDetails" HeaderText="RLD Detail" />
                                                <asp:BoundField DataField="iMetNo" HeaderText="MetNo" />
                                                <asp:BoundField DataField="vMCode" HeaderText="MCode" />
                                                <asp:BoundField DataField="vMValue" HeaderText="CMaxValue" />
                                                <asp:BoundField DataField="vMMaxValue" HeaderText="TMaxValue" />
                                                <asp:BoundField DataField="vMHalfValue" HeaderText="T1/2Value" />
                                                <asp:BoundField DataField="DrugRegionPKActive" HeaderText="Active" />
                                                <asp:TemplateField HeaderText="Edit">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImbEdit" runat="server" ImageUrl="~/images/Edit2.gif" CommandName="Edit">
                                                        </asp:ImageButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImbDelete" runat="server" ImageUrl="~/Images/i_delete.gif" 
                                                            OnClientClick="return msgconfirmalert('Are you sure you want to Delete?',this);" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center;" colspan="2">
                                     <asp:HyperLink ID="hlnkBack" runat="server" Text="Back" ToolTip="Back To Previous Page"
                                            NavigateUrl="~/frmDrugAnalytesDetail.aspx?Mode=1" style=" padding-right :20px; ">
                                        </asp:HyperLink>
                                        <asp:Button ID="BtnAdd" runat="server" Text="Add " ToolTip="Add" CssClass="btn btnnew" />
                                        <asp:Button ID="BtnExit" runat="server" Text="Exit" ToolTip="Exit" CssClass="btn btnexit"
                                            OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
                                       
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="BtnCancel" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" style="width: 100%; height: 188px;">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="DivAdd" runat="server">
                            <table align="center">
                                <tbody>
                                    <tr>
                                        <td style="display: none" align="right">
                                            Code :
                                        </td>
                                        <td style="display: none" align="left">
                                            <asp:TextBox ID="txtCode" runat="server" CssClass="textBox" Width="32px" MaxLength="2"
                                               >N/A</asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap" class="Label" align="right">
                                            CMax Value :
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtValue" runat="server" CssClass="textBox" MaxLength="50"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap" class="Label" align="right">
                                            TMax Value :
                                        </td>
                                        <td style="height: 12px" align="left">
                                            <asp:TextBox ID="txtMaxValue" runat="server" CssClass="textBox" MaxLength="50" ></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap" class="Label" align="right">
                                            T <sub>1/2</sub>&nbsp;Value :
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtHalfValue" runat="server" CssClass="textBox" MaxLength="50" ></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="display: none" class="Label" align="right">
                                            Active :
                                        </td>
                                        <td style="display: none" align="left">
                                            <asp:CheckBox ID="ChkActive" runat="server"  Checked="true">
                                            </asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="2">
                                            <asp:Button ID="BtnSave" runat="server" Text="Save" ToolTip ="Save" CssClass="btn btnsave" 
                                                OnClientClick="return ValidationForSave();"></asp:Button>
                                            <asp:Button ID="BtnCancel" runat="server" Text="Cancel" ToolTip ="Cancel" CssClass="btn btncancel" >
                                            </asp:Button>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="BtnAdd" />
                        <asp:AsyncPostBackTrigger ControlID="BtnCancel" />
                        <asp:AsyncPostBackTrigger ControlID="GV_Detail" EventName="RowCommand" />
                        <asp:AsyncPostBackTrigger ControlID="BtnGo" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
           
            </td>
        </tr>
    </table>
</asp:Content>
