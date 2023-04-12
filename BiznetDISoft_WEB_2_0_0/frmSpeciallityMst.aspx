<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmSpeciallityMst.aspx.vb" Inherits="frmSpeciallityMst"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" language="javascript"></script>

    <script type="text/javascript" src="Script/Validation.js"></script>
  
    
 
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <contenttemplate>
<TABLE style="WIDTH: 80%; TEXT-ALIGN: left"><TBODY><TR><TD style="WHITE-SPACE: nowrap; HEIGHT: 22px" class="Label" align=left>Speciality Desc </TD><TD style="WIDTH: 50%; HEIGHT: 22px" align=left><asp:TextBox id="txtSpecDesc" tabIndex=1 runat="server" CssClass="textBox" Width="148px" MaxLength="100" __designer:wfdid="w107"></asp:TextBox> </TD></TR><TR><TD style="WIDTH: 25%; WHITE-SPACE: nowrap" class="Label" align=left>Speciality Details </TD><TD style="WIDTH: 25%" align=left><asp:TextBox id="txtSpecDtl" tabIndex=2 runat="server" CssClass="textBox" Width="148px" MaxLength="5" TextMode="MultiLine" __designer:wfdid="w108"></asp:TextBox> </TD></TR><TR><TD style="HEIGHT: 26px" align=right><asp:Button id="BtnSave" tabIndex=3 onclick="BtnSave_Click" runat="server" Text="Save" CssClass="btn btnsave" __designer:wfdid="w109"></asp:Button> </TD><TD style="WIDTH: 25%; HEIGHT: 26px" align=left><asp:Button id="BtnExit" tabIndex=4 onclick="BtnExit_Click" runat="server" Text="Exit" CssClass="btn btnexit" __designer:wfdid="w110"></asp:Button> </TD></TR><TR><TD style="WIDTH: 25%" align=right><asp:Button id="btnupdate" tabIndex=5 onclick="btnupdate_Click" runat="server" Text="Update" CssClass="btn btnsave" Visible="False" __designer:wfdid="w111"></asp:Button> </TD><TD style="WIDTH: 25%" align=left><asp:Button id="btncancel" tabIndex=6 onclick="btncancel_Click" runat="server" Text="Cancel" CssClass="btn btncancel" Visible="False" __designer:wfdid="w112"></asp:Button> </TD></TR><TR><TD colSpan=4><asp:GridView id="gvwSpecialityMst" runat="server" SkinID="grdViewSml" OnRowCommand="gvwSpecialityMst_RowCommand" OnRowDataBound="gvwSpecialityMst_RowDataBound" OnPageIndexChanging="gvwSpecialityMst_PageIndexChanging" AllowPaging="True" AutoGenerateColumns="False" PageSize="50" __designer:wfdid="w113">
                            
                            <Columns>
                                <asp:BoundField DataFormatString="number" HeaderText="#">
                                    <ItemStyle HorizontalAlign="Right" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField DataField="nSpecialityNo" HeaderText="Speciality No" />
                                
                                <asp:BoundField DataField="vSpecialityDesc" HeaderText="Speciality Desc" />
                                
                                <asp:BoundField DataField="vSpecialityDetails" HeaderText="Speciality Details" />
                                
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
                        </asp:GridView> </TD></TR></TBODY></TABLE>
</contenttemplate>
        <triggers>
<asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click"></asp:AsyncPostBackTrigger>
</triggers>
    </asp:UpdatePanel>
</asp:Content>
