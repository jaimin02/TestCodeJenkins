<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmGenerateSamples.aspx.vb" Inherits="frmGenerateSamples"  %>


<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

<script type="text/javascript" src="Script/AutoComplete.js"></script>
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
    
    
function SelectAll(CheckBoxControl,Grid) 
    {
        if (CheckBoxControl.checked == true) 
        {
            var i;
            for (i=0; i < document.forms[0].elements.length; i++) 
            {           
                if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf(Grid) > -1)) 
                {
                   if  (document.forms[0].elements[i].disabled==false)
                       {
                          document.forms[0].elements[i].checked = true;
                       }            
                }                     
            }  
        }    
        else
        {
            var i;
            for (i=0; i < document.forms[0].elements.length; i++) 
            {
                if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf(Grid) > -1)) 
                {
                            document.forms[0].elements[i].checked = false;
                }    
            }
        }
    }

    function ValidationToSave(gv)
    {

            var gvwSubject = document.getElementById('<%= gvwSubject.ClientID %>');
            var gvwMedEx = document.getElementById('<%= gvwMedEx.ClientID %>');
            //alert(CheckOne('gvwSubject'));
            if ( gvwSubject == null || typeof ( gvwSubject ) == 'undefined')
            {
                return false;
            }
            else if( CheckOne(gvwSubject.id) == false)
            {
                msgalert('Select atleast one Subject !');
                return false;            
            }
            if ( gvwMedEx == null || typeof ( gvwMedEx ) == 'undefined')
            {
                return false;
            }
            else if( CheckOne(gvwMedEx.id) == false)
            {
                msgalert('Select atleast one MedEx !');
                return false;            
            }
        return true;
    }
    

    function ValidationToAdd()
    {
        if (document.getElementById('<%=ddlMedExGroup.clientid %>').selectedIndex == 0)
        {
            msgalert('Select MedExGroup !');
            return false;            
        }
        if (document.getElementById('<%=ddlMedEx.clientid %>').selectedIndex == 0)
        {
            msgalert('Select MedEx !');
            return false;            
        }
        return true;
    }
//    
    function ValidationToSearch()
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
<TABLE style="WIDTH: 100%" cellPadding=0><TBODY><TR><TD align=center colSpan=2><asp:RadioButtonList id="rblSelection" runat="server" Width="244px" __designer:wfdid="w121" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblSelection_SelectedIndexChanged" AutoPostBack="True"><asp:ListItem Value="00">Screening</asp:ListItem>
<asp:ListItem Value="01">Project Specific</asp:ListItem>
</asp:RadioButtonList></TD></TR><TR><TD align=center colSpan=2><asp:Panel id="pnlProjectSpecific" runat="server" Width="100%" __designer:wfdid="w122" BorderWidth="1px" BorderColor="White"><TABLE align=center><TBODY><TR><TD style="WHITE-SPACE: nowrap" align=right colSpan=1>Project Name/Request Id:</TD><TD style="WHITE-SPACE: nowrap; TEXT-ALIGN: left" colSpan=2><asp:TextBox id="txtproject" tabIndex=1 runat="server" CssClass="textBox" Width="327px" __designer:dtid="2814749767106571" __designer:wfdid="w123"></asp:TextBox><BR /><asp:Button style="DISPLAY: none" id="btnSetProject" onclick="btnSetProject_Click" runat="server" Text=" Project" __designer:dtid="2814749767106572" __designer:wfdid="w124"></asp:Button><asp:HiddenField id="HProjectId" runat="server" __designer:dtid="2814749767106573" __designer:wfdid="w125">


</asp:HiddenField> <cc1:AutoCompleteExtender id="AutoCompleteExtender1" runat="server" __designer:dtid="2814749767106574" __designer:wfdid="w126" UseContextKey="True" TargetControlID="txtProject" ServicePath="AutoComplete.asmx" ServiceMethod="GetProjectCompletionListWithOutSponser" OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1" CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1">
                </cc1:AutoCompleteExtender></TD></TR><TR><TD style="WHITE-SPACE: nowrap; TEXT-ALIGN: right" colSpan=1>Activity:</TD><TD style="WHITE-SPACE: nowrap; TEXT-ALIGN: left" colSpan=2><asp:DropDownList id="ddlActivity" tabIndex=2 runat="server" CssClass="dropDownList" Width="192px" __designer:wfdid="w127" OnSelectedIndexChanged="ddlActivity_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>&nbsp;&nbsp;</TD></TR><TR><TD style="WHITE-SPACE: nowrap; TEXT-ALIGN: right" colSpan=1>Period:</TD><TD style="WHITE-SPACE: nowrap; TEXT-ALIGN: left" colSpan=2><asp:DropDownList id="ddlPeriod" tabIndex=3 runat="server" CssClass="dropDownList" Width="192px" __designer:wfdid="w1" OnSelectedIndexChanged="ddlPeriod_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList></TD></TR><TR><TD style="WHITE-SPACE: nowrap; TEXT-ALIGN: right" colSpan=1>Node:</TD><TD style="WHITE-SPACE: nowrap; TEXT-ALIGN: left" colSpan=2><asp:DropDownList id="ddlNode" tabIndex=4 runat="server" CssClass="dropDownList" Width="192px" __designer:wfdid="w2" AutoPostBack="True"></asp:DropDownList></TD></TR><TR><TD style="WHITE-SPACE: nowrap" colSpan=1></TD><TD style="WHITE-SPACE: nowrap; TEXT-ALIGN: left" colSpan=2><asp:Button id="btnSearch" tabIndex=5 onclick="btnSearch_Click" runat="server" Text="Search" CssClass="btn btnnew" __designer:wfdid="w129" onClientClick="return ValidationToSearch();"></asp:Button></TD></TR><TR><TD style="WHITE-SPACE: nowrap" colSpan=1></TD><TD style="WHITE-SPACE: nowrap" colSpan=2></TD></TR></TBODY></TABLE></asp:Panel> </TD></TR><TR><TD align=center colSpan=2><asp:Panel id="pnlSubjectGrid" runat="server" Width="100%" __designer:wfdid="w1"><asp:GridView id="gvwSubject" tabIndex=6 runat="server" Width="100%" SkinID="grdViewAutoSizeMax" __designer:wfdid="w130" AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="gvwSubject_PageIndexChanging" PageSize="5"><Columns>
<asp:TemplateField HeaderText="All"><HeaderTemplate>
               <%--<asp:CheckBox id="chkSelectAll" runat="server" Text="All" 
                OnCheckedChanged= "SelectAll(this,'gvwSubject')">--%>
                <%--</asp:CheckBox>--%>
               
 <input id="chkSelectAllgvwSubject"  onclick="SelectAll(this,'gvwSubject')" type="checkbox">
    <asp:Label ID="Label1gvwSubject" runat="server" Text="All"></asp:Label>
                
</HeaderTemplate>
<ItemTemplate>
                <asp:CheckBox id="chkSelect_Sub" runat="server">
                </asp:CheckBox>
                
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="vSubjectID" HeaderText="Subject Id"></asp:BoundField>
<asp:BoundField DataField="FullName" HeaderText="Subject Name"></asp:BoundField>
</Columns>
</asp:GridView></asp:Panel>&nbsp;</TD></TR><TR><TD align=left colSpan=2></TD></TR><TR><TD align=left colSpan=2></TD></TR><TR><TD align=center colSpan=2><TABLE style="WIDTH: 311px" id="tblMedEx" runat="server"><TBODY><TR><TD align=right>MedexGroup:</TD><TD style="WIDTH: 199px" align=left><asp:DropDownList id="ddlMedexGroup" tabIndex=7 runat="server" CssClass="dropDownList" Width="192px" __designer:wfdid="w131" OnSelectedIndexChanged="ddlMedexGroup_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList> </TD></TR><TR><TD style="TEXT-ALIGN: right" align=right>Medex:</TD><TD style="WIDTH: 199px" align=left><asp:DropDownList id="ddlMedex" tabIndex=8 runat="server" CssClass="dropDownList" Width="192px" __designer:wfdid="w132"></asp:DropDownList> </TD></TR><TR><TD style="WIDTH: 94px; HEIGHT: 35px" align=right></TD><TD style="WIDTH: 199px; HEIGHT: 35px" align=left><asp:Button id="btnAddMedEx" tabIndex=9 onclick="btnAddMedEx_Click" runat="server" Text="Add" CssClass="btn btnnew" __designer:wfdid="w133" onClientClick="return ValidationToAdd();"></asp:Button></TD></TR></TBODY></TABLE></TD></TR><TR><TD align=center colSpan=2>&nbsp;<asp:Panel id="pnlMedExGrid" runat="server" Width="100%" __designer:wfdid="w2"><asp:GridView id="gvwMedEx" tabIndex=10 runat="server" Width="100%" SkinID="grdViewAutoSizeMax" __designer:wfdid="w134" PageSize="5" OnPageIndexChanging="gvwMedEx_PageIndexChanging" AllowPaging="True" AutoGenerateColumns="False" OnRowDeleting="gvwMedEx_RowDeleting" OnRowDataBound="gvwMedEx_RowDataBound" OnRowCommand="gvwMedEx_RowCommand"><Columns>
<asp:TemplateField HeaderText="All"><HeaderTemplate>
                <%--<asp:CheckBox id="chkSelectAll" runat="server" Text="All">
                </asp:CheckBox>--%>
<input id="chkSelectAllgvwMedEx" onclick="SelectAll(this,'gvwMedEx')" type="checkbox">
    <asp:Label ID="Label1gvwMedEx" runat="server" Text="All" Width="34px"></asp:Label>
    
</HeaderTemplate>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
<ItemTemplate>
<asp:CheckBox id="chkSelect_Med" runat="server" __designer:wfdid="w1"></asp:CheckBox> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="vMedExCode" HeaderText="Medex Id"></asp:BoundField>
<asp:BoundField DataField="vMedExDesc" HeaderText="Medex Description"></asp:BoundField>
<asp:BoundField DataField="vDefaultValue" HeaderText="Medex Default Value"></asp:BoundField>
<asp:TemplateField HeaderText="Delete"><ItemTemplate>
<asp:ImageButton id="imgDelete" align="Center" runat="server" __designer:wfdid="w3" ImageUrl="~/images/cancel.gif"></asp:ImageButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView></asp:Panel>&nbsp;</TD></TR><TR><TD align=center colSpan=2><asp:Button id="btnSave" tabIndex=11 onclick="btnSave_Click" runat="server" Text="Save" CssClass="btn btnsave" __designer:wfdid="w135" onclientclick="return ValidationToSave('<%= gvwSubject.ClientID %>');"></asp:Button></TD></TR><TR><TD align=center colSpan=2><asp:Button id="btnPrintBarcode" onclick="btnPrintBarcode_Click" runat="server" Text="Print Barcodes of Generated Samples" CssClass="btn btnnew" Width="345px" Visible="False" __designer:wfdid="w2"></asp:Button></TD></TR><TR><TD align=center colSpan=2>&nbsp; <asp:CheckBox id="chkBarcode" runat="server" Text="Generate Barcode" Width="170px" Visible="False" __designer:wfdid="w8"></asp:CheckBox> </TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>
