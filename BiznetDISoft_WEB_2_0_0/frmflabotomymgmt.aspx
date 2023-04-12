<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="frmflabotomymgmt.aspx.vb" Inherits="frmflabotomymgmt"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" Runat="Server">
<%--<script type="text/javascript" src="Script/AutoComplete.js"></script>  
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

</script>
    <table style="width: 321px">
        <tr>
            <td class="Label" style="width: 35%" align="right">
            Project: 
            </td>
            <td align="left">
                <asp:TextBox id="txtproject" runat="server" CssClass="textBox" Width="448px"></asp:TextBox>
                <asp:Button style="DISPLAY: none" id="btnSetProject" onclick="btnSetProject_Click" runat="server" Text=" Project"></asp:Button>
                <asp:HiddenField id="HProjectId" runat="server">
                </asp:HiddenField><cc1:AutoCompleteExtender id="AutoCompleteExtender1" runat="server" UseContextKey="True" TargetControlID="txtProject" ServicePath="AutoComplete.asmx" ServiceMethod="GetProjectCompletionListWithOutSponser" OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1" CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1">
                    </cc1:AutoCompleteExtender>
            </td>
        </tr>
        <tr>
            <td class="Label" style="width: 35%" align="right">
            Period: 
            </td>
            <td align="left">
                <asp:DropDownList id="ddlPeriod" runat="server" CssClass="dropDownList" Width="192px" OnSelectedIndexChanged="ddlPeriod_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="Label" style="width: 35%" align="right">
            Activity: 
            </td>
            <td align="left">
                <asp:DropDownList id="ddlActivity" runat="server" CssClass="dropDownList" Width="454px"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 35%" align="right" class="Label">
            BarCode: 
            </td>
            <td class="Label" align="left">
                <asp:TextBox ID="txtScan" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td align="right" class="Label" style="width: 35%">
                Collected By:
            </td>
            <td align="left" class="Label">
                <asp:DropDownList id="ddlCollectedBy" runat="server" CssClass="dropDownList" Width="262px" OnSelectedIndexChanged="ddlPeriod_SelectedIndexChanged" AutoPostBack="True">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td align="right" class="Label" style="width: 35%">
            </td>
            <td align="left" class="Label">
                &nbsp;<asp:Button ID="btnSearch" runat="server" CssClass="button" Text="Search" />
                <asp:Button ID="btnExit" runat="server" CssClass="button" Text="Exit" /></td>
        </tr>
        <tr>
            <td colspan="2">
                </td>
        </tr>
        <tr>
            <td colspan="2" class="Label">
                </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Panel ID="pnlGrid" runat="server" Height="300px" ScrollBars="Auto" Width="100%">
                    <asp:GridView ID="gvwSubjectSample" runat="server" AutoGenerateColumns="False" SkinID="grdViewSml"
                        Width="100%" PageSize="25">
                        <Columns>
                            <asp:BoundField DataField="nSampleTypeDetailNo" HeaderText="nSampleTypeDetailNo" />
                            <asp:BoundField DataField="vSampleBarCode" HeaderText="Sample" />
                            <asp:BoundField DataField="vSubjectId" HeaderText="SubjectId" />
                            <asp:BoundField DataField="iMySubjectNo" HeaderText="SubjectNo" />
                            <asp:BoundField DataField="FullName" HeaderText="Subject Name" />
                            <asp:BoundField DataField="vWorkSpaceDesc" HeaderText="Project" />
                            <asp:BoundField DataField="vNodeDisplayName" HeaderText="Activity" />
                            <asp:BoundField DataField="iPeriod" HeaderText="Period" />
                            <asp:BoundField DataField="dCollectionDateTime" HeaderText="Collection Time" />
                        </Columns>
                    </asp:GridView>
                    </asp:Panel>
                    <asp:Label ID="lblCount" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                </td>
        </tr>
    </table>--%>
</asp:Content>

