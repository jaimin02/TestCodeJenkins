<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmEditUserRights, App_Web_22suyskz" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" Runat="Server">


    <script language="javascript">
function SelectAll(CheckBoxControl) 
{
        if (CheckBoxControl.checked == true) 
        {
            var i;
            for (i=0; i < document.forms[0].elements.length; i++) 
            {
           
                if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf('GV_UserStage') > -1)) 
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
                if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf('GV_UserStage') > -1)) 
                {
                            document.forms[0].elements[i].checked = false;
                }
                
            }
        }
}
    </script>

<table>
<tr>
<td>
    <table style="width: 681px">
        <tr>
            <td align="right" class="Label">
                Operation Type :&nbsp;</td>
            <td align="left"><asp:RadioButton ID="RbAdd" runat="server" Text="Add/Delete" AutoPostBack="True" GroupName="A" ValidationGroup="a" />
                <asp:RadioButton ID="RbEdit" runat="server" Text="Edit Stage" AutoPostBack="True" GroupName="A" ValidationGroup="a" />
                <asp:RadioButton ID="RbDelete" runat="server" Text="Delete" AutoPostBack="True" GroupName="A" ValidationGroup="a" Visible="False" /></td>
        </tr>
    </table>
</td>
</tr>

<tr>
<td align="left">
    <br />
<asp:UpdatePanel id="Up_General" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <contenttemplate>
<DIV style="WIDTH: 100px" id="DivAdd" runat="server"><TABLE align=center __designer:dtid="1407374883553281"><TBODY><TR __designer:dtid="1407374883553282"><TD align="center"  __designer:dtid="1407374883553283"><TABLE style="WIDTH: 319px" __designer:dtid="1407374883553284"><TBODY><TR __designer:dtid="1407374883553285"><TD __designer:dtid="1407374883553286">User Group :</TD><TD __designer:dtid="1407374883553287"><asp:DropDownList id="DDLUserGroup" runat="server" CssClass="dropDownList" __designer:dtid="1407374883553288" AutoPostBack="True" __designer:wfdid="w175">
                </asp:DropDownList></TD></TR></TBODY></TABLE><INPUT id="HdfTempName" type=hidden runat="server" __designer:dtid="1407374883553289" /> <INPUT id="HdfNodeName" type=hidden runat="server" __designer:dtid="1407374883553290" /></TD></TR><TR __designer:dtid="1407374883553291"><TD align="center" __designer:dtid="1407374883553292"><asp:UpdatePanel id="Up_UserStages" runat="server" __designer:dtid="1407374883553293" UpdateMode="Conditional" RenderMode="Inline" __designer:wfdid="w176"><ContentTemplate __designer:dtid="1407374883553294">
<TABLE><TBODY><TR><TD>Users</TD><TD>Stages</TD></TR><TR><TD><DIV style="BORDER-RIGHT: gray thin solid; BORDER-TOP: gray thin solid; OVERFLOW-Y: scroll; BORDER-LEFT: gray thin solid; WIDTH: 170px; BORDER-BOTTOM: gray thin solid; HEIGHT: 80px" id="Div1"><asp:CheckBoxList id="chklstUser" runat="server" ForeColor="Black" Font-Size="XX-Small" Font-Names="Verdana" CssClass="checkboxlist" Width="150" __designer:wfdid="w177" Font-Name="Verdana" Height="37px"></asp:CheckBoxList></DIV></TD><TD><DIV style="BORDER-RIGHT: gray thin solid; BORDER-TOP: gray thin solid; OVERFLOW-Y: scroll; BORDER-LEFT: gray thin solid; WIDTH: 170px; BORDER-BOTTOM: gray thin solid; HEIGHT: 80px" id="Div2"><asp:CheckBoxList id="chklstStages" runat="server" ForeColor="Black" Font-Size="XX-Small" Font-Names="Verdana" CssClass="checkboxlist" Width="150" __designer:wfdid="w178" Font-Name="Verdana" Height="37px">
                    </asp:CheckBoxList></DIV></TD><TD><asp:Button id="BtnAdd" runat="server" Text="Add" CssClass="btn btnnew"  __designer:dtid="1407374883553301" __designer:wfdid="w179"></asp:Button></TD></TR></TBODY></TABLE>
</ContentTemplate>
<Triggers __designer:dtid="1407374883553295">
<asp:AsyncPostBackTrigger ControlID="DDLUserGroup" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="RbAdd" EventName="CheckedChanged"></asp:AsyncPostBackTrigger>
</Triggers>
</asp:UpdatePanel></TD></TR><TR __designer:dtid="1407374883553296"><TD align=left __designer:dtid="1407374883553297"><TABLE style="WIDTH: 347px" __designer:dtid="1407374883553298"><TBODY><TR __designer:dtid="1407374883553302"><TD align="center"  __designer:dtid="1407374883553303"><asp:UpdatePanel id="UpdatePanel1" runat="server" __designer:dtid="1407374883553304" UpdateMode="Conditional" RenderMode="Inline" __designer:wfdid="w180"><ContentTemplate __designer:dtid="1407374883553305">
<asp:GridView id="GV_UserStage" runat="server" SkinID="grdView" __designer:wfdid="w181" OnRowCommand="GV_UserStage_RowCommand" OnRowDeleting="GV_UserStage_RowDeleting" OnRowDataBound="GV_UserStage_RowDataBound" AutoGenerateColumns="False"><Columns>
<asp:TemplateField HeaderText="Delete"><ItemTemplate>
<asp:CheckBox runat="Server" id="CHKDelete"></asp:CheckBox>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="vTemplateId" HeaderText="TemplateId"></asp:BoundField>
<asp:BoundField DataField="vTemplateDesc" HeaderText="TemplateName"></asp:BoundField>
<asp:BoundField DataField="iNodeId" HeaderText="NodeId"></asp:BoundField>
<asp:BoundField DataField="vNodeDisplayName" HeaderText="NodeName"></asp:BoundField>
<asp:BoundField DataField="iUserId" HeaderText="UserId"></asp:BoundField>
<asp:BoundField DataField="vUserName" HeaderText="UserName"></asp:BoundField>
<asp:BoundField DataField="iStageId" HeaderText="StageId"></asp:BoundField>
<asp:BoundField DataField="vStageDesc" HeaderText="StageName"></asp:BoundField>
</Columns>
</asp:GridView> <asp:Button id="Button1" onclick="Button1_Click" runat="server" Text="Delete" CssClass="button" __designer:dtid="1407374883553309" __designer:wfdid="w182" onclientclick="return confirm('Are You sure You want to DELETE?')"></asp:Button> <asp:Button id="btnSave" runat="server" Text="Save" CssClass="btn btnsave" __designer:dtid="1407374883553309" __designer:wfdid="w183"></asp:Button> 
</ContentTemplate>
<Triggers __designer:dtid="1407374883553306">
<asp:AsyncPostBackTrigger ControlID="BtnAdd" EventName="Click"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="RbAdd" EventName="CheckedChanged"></asp:AsyncPostBackTrigger>
</Triggers>
</asp:UpdatePanel></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></DIV><DIV style="WIDTH: 100px" id="DivEdit" runat="server"><asp:UpdatePanel id="UpdatePanel3" runat="server" UpdateMode="Conditional" RenderMode="Inline" __designer:wfdid="w184"><ContentTemplate>
<asp:GridView id="GV_UserStage_Edit" runat="server" SkinID="grdView" __designer:wfdid="w185" OnRowCommand="GV_UserStage_Edit_RowCommand" OnRowDataBound="GV_UserStage_Edit_RowDataBound" AutoGenerateColumns="False" OnRowCreated="GV_UserStage_Edit_RowCreated" OnRowUpdating="GV_UserStage_Edit_RowUpdating" OnRowEditing="GV_UserStage_Edit_RowEditing"><Columns>
<asp:BoundField DataField="vTemplateId" HeaderText="TemplateId"></asp:BoundField>
<asp:BoundField DataField="vTemplateDesc" HeaderText="TemplateName"></asp:BoundField>
<asp:BoundField DataField="iNodeId" HeaderText="NodeId"></asp:BoundField>
<asp:BoundField DataField="vNodeDisplayName" HeaderText="NodeName"></asp:BoundField>
<asp:BoundField DataField="iUserId" HeaderText="UserId"></asp:BoundField>
<asp:BoundField DataField="vUserName" HeaderText="UserName"></asp:BoundField>
<asp:BoundField DataField="iStageId" HeaderText="StageId"></asp:BoundField>
<asp:BoundField DataField="vStageDesc" HeaderText="StageName"></asp:BoundField>
<asp:TemplateField HeaderText="New Stage"><ItemTemplate>
<asp:DropDownList id="DDLStages" runat="server" Width="132px" CssClass="dropDownList" __designer:wfdid="w2"></asp:DropDownList>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Edit"><ItemTemplate>
<asp:LinkButton id="LnkEdit" runat="server" __designer:wfdid="w85">Edit</asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView> <asp:Button id="BtnUpdate" onclick="BtnUpdate_Click" runat="server" CssClass="btn btnsave" __designer:wfdid="w186" text="Update"></asp:Button> 
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="RbEdit" EventName="CheckedChanged"></asp:AsyncPostBackTrigger>
</Triggers>
</asp:UpdatePanel> </DIV>
</contenttemplate>
    <triggers>
<asp:AsyncPostBackTrigger ControlID="RbAdd" EventName="CheckedChanged"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="RbDelete" EventName="CheckedChanged"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="RbEdit" EventName="CheckedChanged"></asp:AsyncPostBackTrigger>
</triggers>
        
    </asp:UpdatePanel>
</td>
</tr>
    <tr>
        <td align="center">
            <asp:Button ID="BtnExit" runat="server" CssClass="btn btnback" Text="" /></td>
    </tr>
</table>

    
</asp:Content>

