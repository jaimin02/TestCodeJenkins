<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmDrugAnalytesPkDetail, App_Web_ybumpksz" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" Runat="Server">
<table>
<tr>
<td>
 <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline" UpdateMode="Conditional">
 <ContentTemplate>
<TABLE><TBODY><TR><TD style="WIDTH: 192px; HEIGHT: 21px" class="Label" align=right>Drug Name :</TD><TD style="HEIGHT: 21px" align=left><SELECT style="WIDTH: 220px" id="SlcDrug" class="dropDownList" runat="server"></SELECT> </TD></TR><TR><TD style="WIDTH: 192px" class="Label" align=right>Region :</TD><TD align=left><SELECT style="WIDTH: 220px" id="SlcRegion" class="dropDownList" runat="server"></SELECT> <asp:Button id="BtnGo" runat="server" Text="" CssClass="btn btngo" Width="36px" __designer:wfdid="w17"></asp:Button> <INPUT id="HdfDrugRegion" type=hidden runat="server" /></TD></TR><TR><TD align=left colSpan=2><asp:GridView id="GV_Detail" runat="server" SkinID="grdViewAutoSizeMax" __designer:wfdid="w18" AutoGenerateColumns="False">
                    <Columns>
                        <asp:TemplateField HeaderText="Sr No">
                            <ItemTemplate>
                                <asp:Label ID="LblSrNo" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="vDrugRegionPKCode" HeaderText="DrugRegion PK Code" />
                        <asp:BoundField DataField="vDrugRegionCode" HeaderText="DrugRegion Code" />
                        <asp:BoundField DataField="vDrugCode" HeaderText="Drug Code" />
                        <asp:BoundField DataField="vDrugName" HeaderText="Drug Name" />
                        <asp:BoundField DataField="vRegionCode" HeaderText="Region Code" />
                        <asp:BoundField DataField="vRegionName" HeaderText="Region Name" />
                        <asp:BoundField DataField="vRLDDetails" HeaderText="RLD Detail" />
                        <asp:BoundField DataField="iMetNo" HeaderText="MetNo" />
                        <asp:BoundField DataField="vMCode" HeaderText="MCode" />
                        <asp:BoundField DataField="vMValue" HeaderText="MValue" />
                        <asp:BoundField DataField="vMMaxValue" HeaderText="MMaxValue" />
                        <asp:BoundField DataField="vMHalfValue" HeaderText="MHalfValue" />
                        <asp:BoundField DataField="DrugRegionPKActive" HeaderText="Active" />
                        <asp:TemplateField HeaderText="Edit">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImbEdit" runat="server" ImageUrl="~/images/Edit2.gif" />
                         
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImbDelete" runat="server" ImageUrl="~/Images/i_delete.gif" OnClientClick="return msgconfirmalert('Are you sure you want to Delete?',this);"  />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView> </TD></TR><TR><TD style="WIDTH: 192px" align=left></TD><TD align=left><asp:Button id="BtnAdd" runat="server" Text="Add " CssClass="btn btnnew" __designer:wfdid="w19"></asp:Button> <asp:Button id="BtnExit" runat="server" Text="Exit" CssClass="btn btnexit" __designer:wfdid="w20" onclientclick="return msgconfirmalert('Are you sure you want to Exit?',this);"></asp:Button></TD></TR></TBODY></TABLE>
</ContentTemplate>
     <triggers>
<asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="BtnCancel" EventName="Click"></asp:AsyncPostBackTrigger>
</triggers>
     
 </asp:UpdatePanel>
 </td>
</tr>

    <tr>
        <td align="centre" colspan="2">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
<DIV id="DivAdd" runat="server"><TABLE width="100%"><TBODY><TR><TD class="Label" align=left>Code :</TD><TD align=left><asp:TextBox id="txtCode" runat="server" CssClass="textBox" Width="32px" MaxLength="2"></asp:TextBox></TD></TR><TR><TD class="Label" align=left>Value :</TD><TD align=left><asp:TextBox id="txtValue" runat="server" CssClass="textBox" MaxLength="50"></asp:TextBox></TD></TR><TR><TD style="HEIGHT: 28px" class="Label" align=left>Max Value :</TD><TD style="HEIGHT: 28px" align=left><asp:TextBox id="txtMaxValue" runat="server" CssClass="textBox" MaxLength="50"></asp:TextBox></TD></TR><TR><TD class="Label" align=left>Half Value :</TD><TD align=left><asp:TextBox id="txtHalfValue" runat="server" CssClass="textBox" MaxLength="50"></asp:TextBox></TD></TR><TR><TD class="Label" align=left>Active :</TD><TD align=left><asp:CheckBox id="ChkActive" runat="server" checked="true"></asp:CheckBox></TD></TR><TR><TD align=left></TD><TD align=left><asp:Button id="BtnSave" runat="server" Text="Save" CssClass="btn btnsave"></asp:Button>&nbsp; <asp:Button id="BtnCancel" runat="server" Text="Cancel" CssClass="btn" btncancel></asp:Button>&nbsp;&nbsp; </TD></TR></TBODY></TABLE></DIV>
</ContentTemplate>
            <Triggers>
<asp:AsyncPostBackTrigger ControlID="BtnAdd"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="BtnCancel"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="GV_Detail" EventName="RowCommand"></asp:AsyncPostBackTrigger>
</Triggers>
        </asp:UpdatePanel>
            &nbsp;
        </td>
    </tr>
</table>

</asp:Content>

