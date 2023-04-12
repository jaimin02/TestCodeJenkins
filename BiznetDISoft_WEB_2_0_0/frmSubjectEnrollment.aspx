<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="frmSubjectEnrollment.aspx.vb" 
Inherits="frmSubjectEnrollment"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" Runat="Server">

<script type="text/javascript" src="Script/AutoComplete.js"></script>
 <script type="text/javascript" src="Script/General.js"></script>
 <script type="text/javascript" src="Script/Gridview.js"></script>
<script type="text/javascript" language="javascript">

function ClientPopulated(sender, e)
{

    ProjectClientShowing('AutoCompleteExtender1',$get('<%= txtProject.ClientId %>'));
}
            
function OnSelected(sender,e)
{
    ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'),document.getElementById('<%= btnSetProject.ClientId %>'));
} 
  
function ClientPopulatedSubject(sender, e)
{
    SubjectClientShowing('AutoCompleteExtender2',$get('<%= txtSubject.ClientId %>'));
}
            
function OnSelectedSubject(sender,e)
{
    SubjectOnItemSelected(e.get_value(), $get('<%= txtSubject.clientid %>'),
                $get('<%= HSubjectId.clientid %>'),document.getElementById('<%= btnSubject.ClientId %>'));
}
        
function Validation()
{
    if (document.getElementById('<%= HProjectId.ClientId %>').value.toString().trim().length <= 0)
    {
        msgalert('Please Enter Project !');
        return false;
    }   
    return true;
}

</script>

<TABLE cellPadding=0><TBODY>

<TR>
    <td class="Label" nowrap="nowrap">
        Project Name/Request Id:
    </td>
    <TD noWrap class="Label" style="text-align: left">
 <asp:TextBox id="txtproject" runat="server" CssClass="textBox" Width="622px" TabIndex="1">
 </asp:TextBox><asp:Button style="DISPLAY: none" id="btnSetProject" onclick="btnSetProject_Click" runat="server" Text=" Project"></asp:Button>
 <asp:HiddenField id="HProjectId" runat="server"></asp:HiddenField>
 <cc1:AutoCompleteExtender id="AutoCompleteExtender1" runat="server" UseContextKey="True" 
 TargetControlID="txtProject" ServicePath="AutoComplete.asmx" ServiceMethod="GetMyProjectCompletionList" 
 OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1" 
 CompletionListItemCssClass="autocomplete_listitem" 
 CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" 
 CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1">
                    </cc1:AutoCompleteExtender>
</TD></TR>
    <tr>
        <td class="Label" nowrap="nowrap" style="text-align: right">
            Search Subject:</td>
        <td style="height: 20px; width: 125px;" align="left">
            <asp:TextBox ID="txtSubject" runat="server" CssClass="textBox" TabIndex="2" Width="480px"></asp:TextBox>
            <asp:Button
                ID="btnSubject" runat="server" Style="display: none" Text="Subject" /><asp:HiddenField
                    ID="HSubjectId" runat="server" />
            <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" BehaviorID="AutoCompleteExtender2"
                CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListItemCssClass="autocomplete_listitem" 
                MinimumPrefixLength="1" OnClientItemSelected="OnSelectedSubject" OnClientShowing="ClientPopulatedSubject"
                ServiceMethod="GetSubjectCompletionList_NotRejected" ServicePath="AutoComplete.asmx" TargetControlID="txtSubject"
                UseContextKey="True">
            </cc1:AutoCompleteExtender>
               
            </td>
    </tr>
                
<tr>
    <td class="Label" style="height: 21px">
    </td>
    <TD style="height: 21px; text-align: left" class="Label"><asp:Button id="btnAdd" runat="server" 
    Text="Add New Subject" CssClass="btn btnnew" OnClientClick="return Validation();" Width="107px"></asp:Button> <asp:Button id="btnExit" runat="server" Text="Exit" CssClass="btn btnexit" CausesValidation="False" onclientclick="return msgconfirmalert('Are you sure you want to Exit?',this);"></asp:Button></TD>

<TD style="height: 21px"></TD>
</tr>
    <tr>
        <td class="Label" style="height: 21px">
        </td>
        <td class="Label" style="height: 21px; text-align: left">
        </td>
        <td style="height: 21px">
        </td>
    </tr>
</TBODY></TABLE>

<asp:UpdatePanel id="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline"><ContentTemplate>
<asp:GridView id="gvSubject" runat="server" SkinID="grdViewSml" ShowFooter="True" PageSize="25" AllowPaging="True" AutoGenerateColumns="False" OnRowCommand="gvSubject_RowCommand"><Columns>
<%--<asp:BoundField DataFormatString="number" HeaderText="#">
<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

<ItemStyle HorizontalAlign="Right"></ItemStyle>
</asp:BoundField>--%>
<asp:BoundField DataField="vRandomizationNo" HeaderText="Rendomization No"></asp:BoundField>
<asp:BoundField DataField="vSubjectId" HeaderText="SubjectId">
<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

<ItemStyle HorizontalAlign="Left" wrap="false"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="vInitials" HeaderText="Initials">
<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:BoundField>
<asp:TemplateField HeaderText="SubjectName"><ItemTemplate>
<asp:HyperLink ID="hlnkFile" runat="server" Text='<%# Eval("FullName") %>' style="WHITE-SPACE:Nowrap" >
</asp:HyperLink>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="dEnrollmentDate" HeaderText="Enrollment Date"></asp:BoundField>
<asp:BoundField DataField="iLastPeriod" HeaderText="Last Visit"></asp:BoundField>
<asp:BoundField DataField="vWorkSpaceId" HeaderText="WorkSpaceId">
<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:BoundField>
<asp:TemplateField HeaderText="Assignment"><ItemTemplate>
<asp:LinkButton id="lnkAssign" text="Assign" runat="server"  OnClientClick="return msgconfirmalert('Are You Sure You want to Assign Subject?',this);"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="iMySubjectNo" HeaderText="MySubjectNo"></asp:BoundField>
</Columns>
</asp:GridView> 
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

