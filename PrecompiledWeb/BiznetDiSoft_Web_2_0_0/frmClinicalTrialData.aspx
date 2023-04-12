<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmClinicalTrialData, App_Web_vq2225em" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" Runat="Server">

<script language="javascript" type="text/javascript">
     
function Validation()
{
    if (document.getElementById('<%= ddlProjectGroup.ClientId %>').selectedIndex ==0)
    {
        msgalert('Please Select Project Group !');
        document.getElementById('<%= ddlProjectGroup.ClientId %>').focus();
        return false;
    }
    return true;
}                        
   
        
</script>

    <table>
        <tr>
            <td class="Label" style="width: 100%">
                Project Group:
                <asp:DropDownList ID="ddlProjectGroup" runat="server" CssClass="dropDownList" Width="334px">
                </asp:DropDownList>
                &nbsp;<asp:Button ID="btnSearch" runat="server" CssClass="btn btnnew" Text="Search" 
                             OnClientClick="return Validation();"/>
                <asp:Button ID="btnExit" runat="server" CssClass="btn btnexit" Text="Exit" onclientclick="return msgconfirmalert('Are you sure you want to Exit?',this);"/></td>
        </tr>
        <tr>
            <td style="width: 100%">
            </td>
        </tr>
        <tr>
            <td style="width: 100%">
                <asp:GridView ID="gvWorkspaceSubject" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    PageSize="25" ShowFooter="True" SkinID="grdViewAutoSizeMax" style="width:65%; margin:auto;">
                    <Columns>
                        <asp:BoundField DataFormatString="number" HeaderText="#"><ItemStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="vWorkspaceDesc" HeaderText="Sites">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="NoOfSubjects" HeaderText="Patient Details">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="vWorkSpaceId" HeaderText="WorkSpaceId">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Edit">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" runat="server">Edit</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td style="width: 100%">
            </td>
        </tr>
        <tr>
            <td style="width: 100%">
                <asp:GridView ID="gvSubjectVisit" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    PageSize="5" ShowFooter="True" SkinID="grdViewAutoSizeMax" style="width:70%; margin:auto;" Visible="False">
                    <Columns>
                        <asp:BoundField DataFormatString="number" HeaderText="#">
                            <ItemStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="iMySubjectNo" HeaderText="MySubjectNo">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="vInitial" HeaderText="Initial">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="vSubjectId" HeaderText="Subject Id">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="vSubject" HeaderText="Subject Name">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="dRegistrationDate" HeaderText="Registration Date">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="iVisits" HeaderText="Visits">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>

