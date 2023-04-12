<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="frmModifySamples.aspx.vb" Inherits="frmModifySamples" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

<script type="text/javascript" src="Script/AutoComplete.js"></script>
<script type="text/javascript" src="Script/General.js"></script>
<script type="text/javascript" src="Script/Gridview.js"></script>
<script type="text/javascript" language="javascript">

function ClientPopulated(sender, e)
  {
                ProjectClientShowing('AutoCompleteExtender1',$get('<%=txtProject.ClientId%>'));
  }
            
 function OnSelected(sender,e)
  {
        ProjectOnItemSelected(e.get_value(), $get('<%=txtProject.clientid%>'),
            $get('<%=HProjectId.clientid%>'),document.getElementById('<%=btnSetProject.ClientId%>'));
  }               
    
function ValidationToAdd()
    {
        if (document.getElementById('<%=ddlMedExGroup.clientid %>').selectedIndex == 0)
        {
            msgalert('Select Attribute Group !');
            return false;            
        }
        if (document.getElementById('<%=ddlMedEx.clientid %>').selectedIndex == 0)
        {
            msgalert('Select Attribute !');
            return false;            
        }
        return true;
    }
        
function ValidationToSearch()
    {
        if (document.getElementById('<%=HProjectId.clientid %>').value == '')
        {
            msgalert('Select Project !');
            return false;            
        }
        else if (document.getElementById('<%=ddlActivity.clientid %>').selectedIndex == 0)
        {
            msgalert('Select Activity !');
            return false;            
        }
        else if (document.getElementById('<%=ddlPeriod.clientid %>').selectedIndex == 0)
        {
            msgalert('Select Period !');
            return false;            
        }
        else if (document.getElementById('<%=ddlNode.clientid %>').selectedIndex == 0)
        {
            msgalert('Select Node !');
            return false;            
        }
        return true;
    }
  
</script>

  <asp:UpdatePanel id="upGenerateSamples" runat="server" RenderMode="Inline" UpdateMode="Conditional">
    <contenttemplate>
<TABLE cellPadding=0><TBODY><TR><TD class="Label" align=center colSpan=2><asp:RadioButtonList id="rblSelection" runat="server" Width="244px" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rblSelection_SelectedIndexChanged"><asp:ListItem Value="00">Screening</asp:ListItem>
<asp:ListItem Value="01">Project Specific</asp:ListItem>
</asp:RadioButtonList></TD></TR><TR><TD align=left colSpan=2><asp:Panel id="pnlProjectSpecific" runat="server" Width="100%" BorderWidth="1px" BorderColor="White"><TABLE align=left><TBODY><TR><TD style="WHITE-SPACE: nowrap; TEXT-ALIGN: left" class="Label" align=left colSpan=1>Project Name/Request Id :</TD><TD style="WHITE-SPACE: nowrap; TEXT-ALIGN: left" align=left colSpan=2><asp:TextBox id="txtproject" tabIndex=1 runat="server" CssClass="textBox" Width="622px"></asp:TextBox><BR /><asp:Button style="DISPLAY: none" id="btnSetProject" onclick="btnSetProject_Click" runat="server" Text=" Project"></asp:Button><asp:HiddenField id="HProjectId" runat="server">


</asp:HiddenField> <cc1:AutoCompleteExtender id="AutoCompleteExtender1" runat="server" UseContextKey="True" TargetControlID="txtProject" ServicePath="AutoComplete.asmx" ServiceMethod="GetProjectCompletionListWithOutSponser" OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1" CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1">
                </cc1:AutoCompleteExtender></TD></TR><TR><TD style="WHITE-SPACE: nowrap; TEXT-ALIGN: right" class="Label" align=right colSpan=1>Activity :</TD><TD style="WHITE-SPACE: nowrap; TEXT-ALIGN: left" colSpan=2><asp:DropDownList id="ddlActivity" tabIndex=2 runat="server" CssClass="dropDownList" Width="192px" AutoPostBack="True" OnSelectedIndexChanged="ddlActivity_SelectedIndexChanged"></asp:DropDownList>&nbsp;&nbsp;</TD></TR><TR><TD style="WHITE-SPACE: nowrap; TEXT-ALIGN: right" class="Label" align=right colSpan=1>Period :</TD><TD style="WHITE-SPACE: nowrap; TEXT-ALIGN: left" colSpan=2><asp:DropDownList id="ddlPeriod" tabIndex=3 runat="server" CssClass="dropDownList" Width="192px" AutoPostBack="True" OnSelectedIndexChanged="ddlPeriod_SelectedIndexChanged"></asp:DropDownList></TD></TR><TR><TD style="WHITE-SPACE: nowrap; HEIGHT: 21px; TEXT-ALIGN: right" class="Label" align=right colSpan=1>Node :</TD><TD style="WHITE-SPACE: nowrap; HEIGHT: 21px; TEXT-ALIGN: left" colSpan=2><asp:DropDownList id="ddlNode" tabIndex=4 runat="server" CssClass="dropDownList" Width="192px"></asp:DropDownList></TD></TR></TBODY></TABLE></asp:Panel> </TD></TR><TR><TD align=center colSpan=2><asp:Button id="btnSearch" tabIndex=5 onclick="btnSearch_Click" runat="server" Text="Search" CssClass="btn btnnew" onClientClick="return ValidationToSearch();"></asp:Button> <asp:Button id="btnCancel" onclick="btnCancel_Click" runat="server" Text="Cancel" CssClass="btn btncancel" __designer:wfdid="w1"></asp:Button> <asp:Button id="btnExit" onclick="btnExit_Click" runat="server" Text="Exit" CssClass="btn btnexit" __designer:wfdid="w2"></asp:Button></TD></TR><TR><TD align=left colSpan=2></TD></TR><TR><TD align=left colSpan=2><asp:Panel id="pnlSubjectGrid" runat="server" Width="100%"><asp:GridView id="gvwSubject" tabIndex=6 runat="server" Width="100%" SkinID="grdViewAutoSizeMax" OnRowEditing="gvwSubject_RowEditing" OnRowDataBound="gvwSubject_RowDataBound" OnRowCommand="gvwSubject_RowCommand" AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="gvwSubject_PageIndexChanging" PageSize="25" OnRowCreated="gvwSubject_RowCreated"><Columns>
<asp:BoundField DataField="nSampleId" HeaderText="nSampleId"></asp:BoundField>
<asp:BoundField DataField="vSampleId" HeaderText="Sample Id"></asp:BoundField>
<asp:BoundField DataField="vSubjectID" HeaderText="Subject Id"></asp:BoundField>
<asp:BoundField DataField="FullName" HeaderText="Subject Name"></asp:BoundField>
<asp:BoundField DataField="dCollectionDateTime" HeaderText="Collection DateTime"></asp:BoundField>
<asp:BoundField DataField="iMySubjectNo" HeaderText="MySubjectNo"></asp:BoundField>
<asp:TemplateField HeaderText="View"><ItemTemplate>
<asp:LinkButton id="lnkView" runat="server" __designer:wfdid="w15">View</asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Edit"><ItemTemplate>
<asp:LinkButton id="lnkEdit" runat="server" __designer:wfdid="w16">Edit</asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView></asp:Panel>&nbsp;</TD></TR></TBODY></TABLE><DIV style="LEFT: 438px; WIDTH: 500px; POSITION: absolute; TOP: 608px; HEIGHT: 331px" id="divMedEx" class="divStyleNoAbs" runat="server" Visible="false" __designer:dtid="281474976710662"><asp:Panel id="pnlMedEx" runat="server" Visible="False" __designer:wfdid="w3"><TABLE cellPadding=5 __designer:dtid="281474976710663"><TBODY><TR __designer:dtid="281474976710664"><TD style="TEXT-ALIGN: center" class="Label" vAlign=top align=left colSpan=2 __designer:dtid="281474976710665">Attribute&nbsp;Details</TD></TR><TR __designer:dtid="281474976710666"><TD style="WIDTH: 179px; TEXT-ALIGN: right" class="Label" vAlign=top align=left __designer:dtid="281474976710667">Attribute Group :</TD><TD style="TEXT-ALIGN: left" vAlign=top align=left __designer:dtid="281474976710668"><asp:DropDownList id="ddlMedexGroup" tabIndex=7 runat="server" CssClass="dropDownList" Width="192px" __designer:dtid="281474976710669" AutoPostBack="True" OnSelectedIndexChanged="ddlMedexGroup_SelectedIndexChanged" __designer:wfdid="w4">
                    </asp:DropDownList></TD></TR><TR __designer:dtid="281474976710670"><TD style="WIDTH: 179px; TEXT-ALIGN: right" class="Label" vAlign=top align=left __designer:dtid="281474976710671">Attribute&nbsp;:</TD><TD style="TEXT-ALIGN: left" vAlign=top align=left __designer:dtid="281474976710672"><asp:DropDownList id="ddlMedex" tabIndex=8 runat="server" CssClass="dropDownList" Width="192px" __designer:dtid="281474976710673" __designer:wfdid="w5">
                    </asp:DropDownList></TD></TR><TR __designer:dtid="281474976710674"><TD style="WIDTH: 179px" class="Label" vAlign=top align=left __designer:dtid="281474976710675"></TD><TD style="TEXT-ALIGN: left" vAlign=top align=left __designer:dtid="281474976710676"><asp:Button id="btnAddMedEx" tabIndex=9 onclick="btnAddMedEx_Click" runat="server" Font-Size="8pt" Font-Bold="False" Text="Add" CssClass="btn btnnew" __designer:dtid="281474976710677" __designer:wfdid="w6" OnClientClick="return ValidationToAdd();"></asp:Button></TD></TR><TR __designer:dtid="281474976710678"><TD class="Label" vAlign=top align=center colSpan=2 __designer:dtid="281474976710679">&nbsp;<asp:Panel id="pnlMedExGrid" runat="server" Width="480px" __designer:dtid="281474976710680" __designer:wfdid="w7" ScrollBars="Auto" Height="150px"><asp:GridView id="gvwMedEx" tabIndex=10 runat="server" SkinID="grdViewSmlSize" __designer:dtid="281474976710681" __designer:wfdid="w8" OnRowDataBound="gvwMedEx_RowDataBound" OnRowCommand="gvwMedEx_RowCommand" AutoGenerateColumns="False" PageSize="5" OnRowDeleting="gvwMedEx_RowDeleting" OnRowCreated="gvwMedEx_RowCreated"><Columns __designer:dtid="281474976710682">
<asp:BoundField DataField="vMedExCode" HeaderText="Attribute Id" __designer:dtid="281474976710683"></asp:BoundField>
<asp:BoundField DataField="vMedExDesc" HeaderText="Attribute Description" __designer:dtid="281474976710684"></asp:BoundField>
<asp:TemplateField HeaderText="Delete" __designer:dtid="281474976710686"><ItemTemplate __designer:dtid="281474976710687">
                                    <asp:ImageButton __designer:dtid="281474976710688" ID="imgDelete" runat="server" align="Center" ImageUrl="~/images/cancel.gif"  />
                                
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView> </asp:Panel> </TD></TR><TR __designer:dtid="281474976710689"><TD style="TEXT-ALIGN: center" class="Label" vAlign=top align=center colSpan=2 __designer:dtid="281474976710690"><asp:Button id="btnSave" tabIndex=11 onclick="btnSave_Click" runat="server" Text="Save" CssClass="btn btnsave" __designer:dtid="281474976710691" __designer:wfdid="w9"></asp:Button> <asp:Button id="btnClose" onclick="btnClose_Click" runat="server" Text="Close" CssClass="btn btnclose" __designer:dtid="281474976710692" __designer:wfdid="w10"></asp:Button></TD></TR></TBODY></TABLE></asp:Panel> </DIV>
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>


