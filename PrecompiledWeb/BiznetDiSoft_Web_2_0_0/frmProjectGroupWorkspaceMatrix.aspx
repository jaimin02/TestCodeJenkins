<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmProjectGroupWorkspaceMatrix, App_Web_w1bzwbih" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" Runat="Server">

<script language="javascript" type="text/javascript">
        function IsChecked()
        {
            if(document.getElementById('<%=ddlProjectGroup.ClientID%>').selectedIndex < 0)
            {

                msgalert("No Project Group Found!");
                return false;
            }
            if(document.getElementById('<%=ddlProjectGroup.ClientID%>').selectedIndex == 0)
            {
                msgalert("Please Select Project Group!");
                return false;
            }
            
            var chklst = document.getElementById('<%=chklstWorkspace.clientid%>');
            var chks;
            var result = false;
            var i;
            
            if ( chklst != null && typeof ( chklst ) != 'undefined')
            {
                chks = chklst.getElementsByTagName('input');
                for ( i=0; i< chks.length; i++)
                {
                    if ( chks[i].type.toUpperCase() == 'CHECKBOX' && chks[i].checked)
                    {
                        result =  true;
                        break;
                    }
                }
            }
            
            if ( !result)
            {
                msgalert('Please Select atleast One Project');
                return false;
            }
        
            return true;
        }
    </script>
    
    <hr class="hr" />
    
    <table border="0" cellpadding="0" cellspacing="0" width="700px">
        <tr style="padding:2px">
            <td align="left" class="Label" valign="top" width="16%">Project Group<span class="TDMandatory">&nbsp;*</span></td>
            <td align="left" class="Label" valign="top" width="42%">Projects<span class="TDMandatory">&nbsp;*</span></td>
            <td align="left" class="Label" valign="top" width="42%"></td>
        </tr>
        <tr style="padding:2px">
            <td align="left" class="Label" valign="top" width="16%">
                <asp:UpdatePanel id="UpProjectGroup" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <contenttemplate>
                        <asp:ListBox ID="ddlProjectGroup" runat="server" Rows="26" CssClass="dropDownList" AutoPostBack="True"></asp:ListBox>
                    </contenttemplate>
                    <triggers>                        
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click"></asp:AsyncPostBackTrigger>
                    </triggers>
                </asp:UpdatePanel>
            </td>
            <td align="left" class="Label" valign="top" width="42%">
                <asp:UpdatePanel id="UpTree" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <contenttemplate>
<DIV style="BORDER-RIGHT: gray thin solid; BORDER-TOP: gray thin solid; OVERFLOW-Y: auto; BORDER-LEFT: gray thin solid; WIDTH: auto; BORDER-BOTTOM: gray thin solid; HEIGHT: 340px" id="Div1" align=left><asp:CheckBoxList id="chklstWorkspace" runat="server" Width="475px" __designer:wfdid="w2" Height="10px"></asp:CheckBoxList></DIV>
</contenttemplate>
                    <triggers>
<asp:AsyncPostBackTrigger ControlID="ddlProjectGroup" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click"></asp:AsyncPostBackTrigger>
</triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr style="padding:10px 0px 0px 0px">
            <td colspan="3" width="100%" align="center" valign="middle" class="Label">
                <asp:Button ID="btnSave" runat="server" CssClass="btn btnsave" OnClientClick="javascript:return IsChecked();" Text="Save" />&nbsp;
                <asp:Button ID="btnexit" runat="server" Text="Exit" CssClass="btn btnexit" onclientclick="return msgconfirmalert('Are you sure you want to Exit?',this);" />  
            </td>
        </tr>
    </table>

</asp:Content>

