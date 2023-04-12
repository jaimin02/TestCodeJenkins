<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmProjectSelection, App_Web_ybumpksz" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" Runat="Server">

<script type="text/javascript" src="Script/AutoComplete.js"></script>
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

function ValidationToContinue()
    {
        if (document.getElementById('<%=HProjectId.clientid %>').value == '')
        {
            msgalert('Select Project !');
            return false;            
        }
        return true;
    }
    
</script>

<asp:UpdatePanel id="upGenerateSamples" runat="server" RenderMode="Inline" UpdateMode="Conditional">
    <contenttemplate>
<TABLE style="WIDTH: 482px"><TBODY><TR><TD style="WIDTH: 271px; WHITE-SPACE: nowrap; HEIGHT: 21px; TEXT-ALIGN: right" class="Label" vAlign=top noWrap align=left>Project Name/Request Id:&nbsp; </TD><TD vAlign=top align=left><asp:TextBox id="txtproject" tabIndex=1 runat="server" CssClass="textBox" Width="622px"></asp:TextBox><asp:Button style="DISPLAY: none" id="btnSetProject" onclick="btnSetProject_Click" runat="server" Text=" Project">
            </asp:Button><asp:HiddenField id="HProjectId" runat="server"></asp:HiddenField> <cc1:AutoCompleteExtender id="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1" CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected" OnClientShowing="ClientPopulated" ServiceMethod="GetMyProjectCompletionList" ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True">
                </cc1:AutoCompleteExtender> </TD></TR><TR><TD style="WIDTH: 271px; WHITE-SPACE: nowrap; HEIGHT: 21px; TEXT-ALIGN: right" class="Label" vAlign=top noWrap align=left></TD><TD vAlign=top align=left></TD></TR><TR><TD style="WHITE-SPACE: nowrap; TEXT-ALIGN: center" class="Label" vAlign=top align=left colSpan=2><asp:Label id="lblProjectSummary" runat="server" CssClass="Label" __designer:wfdid="w1"></asp:Label> </TD></TR><TR><TD style="WHITE-SPACE: nowrap; TEXT-ALIGN: center" class="Label" vAlign=top align=left colSpan=2><asp:Button id="btnContinue" onclientclick="return ValidationToContinue();" runat="server" Text="Continue" CssClass="btn btnnew" __designer:wfdid="w2" OnClick="btnContinue_Click"></asp:Button></TD></TR></TBODY></TABLE>
</contenttemplate>
</asp:UpdatePanel>
</asp:Content>

