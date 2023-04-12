<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmSTP.aspx.vb" Inherits="frmSTP"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script language="javascript" type="text/javascript" src="Script/Validation.js"></script>

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" src="Script/General.js"></script>

    <script language="javascript" type="text/javascript">
function CheckCityVal()
        {
                    
         if(document.getElementById('<%=ddlFromstate.ClientID%>').selectedIndex<1)
            {  
               alert("Please Select Place State");
               document.getElementById('<%=ddlFromstate.ClientID%>').focus();
               return false;
            }           
         else if(document.getElementById('<%=ddlPlaceCity.ClientID%>').selectedIndex<1)
            {  
               alert("Please Select Place City");
               document.getElementById('<%=ddlPlaceCity.ClientID%>').focus();
               return false;
            }       
         else if(document.getElementById('<%=HProjectId.ClientID%>').value=="")
            {  
               alert("Please Select Project");
               document.getElementById('<%=txtProject.ClientID%>').focus();
               return false;
            }       
         else if(document.getElementById('<%=txtSiteName.ClientID%>').value.toString().trim().length <= 0)
            {  
                document.getElementById('<%=txtSiteName.ClientID%>').value = '';
               alert("Please Enter Site Name");
               document.getElementById('<%=txtSiteName.ClientID%>').focus();
               return false;
            }       
         else
          {
          return true;
          }   
        }
        
function ClientPopulated(sender, e)
      {
                ProjectClientShowing('AutoCompleteExtender1',$get('<%= txtProject.ClientId %>'));
      }
            
function OnSelected(sender,e)
      {
        ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'),document.getElementById('<%= btnSetProject.ClientId %>'));
            
      }           
    
function ddlCitySelected()
    {
        var txt = document.getElementById('<%= txtSiteName.ClientId %>');
        var txtPorject = document.getElementById('<%= txtProject.clientid %>');
        var ddlCity = document.getElementById('<%= ddlPlaceCity.ClientId %>');
        
        txt.value = ddlCity.options[ddlCity.selectedIndex].text + ' - ' + txtPorject.value;
        return true;
    }
    
function ddlStateSelected()
    {
        document.getElementById('<%= ddlPlaceCity.ClientId %>').focus();
        return true;
    }
    </script>

    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="WIDTH: 100%; HEIGHT: 100%"><TBODY><TR><TD style="WIDTH: 100%"><TABLE style="WIDTH: 100%; HEIGHT: 100%"><TBODY><TR><TD style="WIDTH: 25%" align=left><STRONG class="Label">Project Name/Request Id :&nbsp;</STRONG></TD><TD style="WIDTH: 100%; WHITE-SPACE: nowrap" align=left><asp:TextBox id="txtproject" tabIndex=1 runat="server" CssClass="textBox" Width="362px" __designer:wfdid="w77"></asp:TextBox><asp:HiddenField id="HProjectId" runat="server" __designer:wfdid="w78"></asp:HiddenField><cc1:AutoCompleteExtender id="AutoCompleteExtender1" runat="server" __designer:wfdid="w79" UseContextKey="True" TargetControlID="txtProject" ServicePath="AutoComplete.asmx" ServiceMethod="GetProjectCompletionListWithOutSponser" OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1" CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1">
                            </cc1:AutoCompleteExtender><asp:Button style="DISPLAY: none" id="btnSetProject" runat="server" Text=" Project" __designer:wfdid="w80"></asp:Button></TD></TR><TR><TD align=left colSpan=2>
<HR style="BACKGROUND-IMAGE: none; WIDTH: 100%; COLOR: #ffcc66; BACKGROUND-COLOR: #ffcc66" />
&nbsp;</TD></TR><TR><TD style="WIDTH: 25%" class="Label" align=left>Investigator Name :</TD><TD style="WIDTH: 100%; WHITE-SPACE: nowrap" align=left><asp:TextBox id="txtInvestigator" tabIndex=1 runat="server" CssClass="textBox" Width="249px" __designer:wfdid="w81"></asp:TextBox> </TD></TR><TR><TD style="WIDTH: 25%" class="Label" align=left>Mobile No. :</TD><TD style="WIDTH: 100%; WHITE-SPACE: nowrap" align=left><asp:TextBox id="txtMobile" tabIndex=2 runat="server" CssClass="textBox" Width="249px" __designer:wfdid="w82"></asp:TextBox> </TD></TR><TR><TD style="WIDTH: 25%" class="Label" align=left>E-mail Address :</TD><TD style="WIDTH: 100%; WHITE-SPACE: nowrap" align=left><asp:TextBox id="txtEmail" tabIndex=3 runat="server" CssClass="textBox" Width="249px" __designer:wfdid="w83"></asp:TextBox> <asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" Width="360px" __designer:wfdid="w84" ValidationGroup="B" Display="Dynamic" ControlToValidate="txtEmail" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ErrorMessage="Please Enter Email Id in Correct Format."></asp:RegularExpressionValidator></TD></TR><TR><TD style="WIDTH: 25%" class="Label" align=left>State :</TD><TD style="WIDTH: 100%; WHITE-SPACE: nowrap" align=left><asp:DropDownList id="ddlFromstate" tabIndex=4 runat="server" CssClass="dropDownList" Width="253px" __designer:wfdid="w85" OnSelectedIndexChanged="ddlFromstate_SelectedIndexChanged" AutoPostBack="True">
                                        </asp:DropDownList> </TD></TR><TR><TD style="WIDTH: 25%" class="Label" align=left>City :</TD><TD style="WIDTH: 100%; WHITE-SPACE: nowrap" align=left><asp:DropDownList id="ddlPlaceCity" tabIndex=5 runat="server" CssClass="dropDownList" Width="253px" __designer:wfdid="w86" onchange="return ddlCitySelected();">
                                        </asp:DropDownList> </TD></TR><TR><TD style="WIDTH: 25%" class="Label" align=left>Site Name :&nbsp;</TD><TD style="WIDTH: 100%; WHITE-SPACE: nowrap" align=left><asp:TextBox id="txtSiteName" tabIndex=6 runat="server" CssClass="textBox" Width="393px" __designer:wfdid="w87"></asp:TextBox> </TD></TR><TR><TD style="WIDTH: 25%" class="Label" align=left>Site Addr1 : </TD><TD style="WIDTH: 100%; WHITE-SPACE: nowrap" align=left><asp:TextBox id="txtSiteAddr1" tabIndex=7 runat="server" CssClass="textBox" Width="249px" __designer:wfdid="w88"></asp:TextBox> </TD></TR><TR><TD style="WIDTH: 25%" class="Label" align=left>Site Addr2 : </TD><TD style="WIDTH: 100%; WHITE-SPACE: nowrap" align=left><asp:TextBox id="txtSiteAddr2" tabIndex=8 runat="server" CssClass="textBox" Width="249px" __designer:wfdid="w89"></asp:TextBox> </TD></TR><TR><TD style="WIDTH: 25%" class="Label" align=left>Site Addr3 : </TD><TD style="WIDTH: 100%; WHITE-SPACE: nowrap" align=left><asp:TextBox id="txtSiteAddr3" tabIndex=9 runat="server" CssClass="textBox" Width="249px" __designer:wfdid="w90"></asp:TextBox> </TD></TR><TR><TD style="WIDTH: 25%" class="Label" align=left>TelePhone No. : </TD><TD style="WIDTH: 100%; WHITE-SPACE: nowrap" align=left><asp:TextBox id="TxtTelePhone" tabIndex=10 runat="server" CssClass="textBox" Width="249px" __designer:wfdid="w91"></asp:TextBox> </TD></TR><TR><TD style="WIDTH: 25%" class="Label" align=left>Fax No. : </TD><TD style="WIDTH: 100%; WHITE-SPACE: nowrap" align=left><asp:TextBox id="TxtFax" tabIndex=11 runat="server" CssClass="textBox" Width="249px" __designer:wfdid="w92"></asp:TextBox> </TD></TR><TR><TD style="WIDTH: 25%" class="Label" align=left>Active Flag : </TD><TD style="WIDTH: 100%; WHITE-SPACE: nowrap" class="Label" align=left><asp:CheckBox id="ChkActive" tabIndex=12 runat="server" Width="109px" __designer:wfdid="w93" Checked="true"></asp:CheckBox> </TD></TR><TR><TD style="WIDTH: 25%" align=left></TD><TD style="WIDTH: 100%; WHITE-SPACE: nowrap" align=left><asp:Button id="btnAdd" tabIndex=13 runat="server" Font-Bold="True" Text="Add" CssClass="btn btnnew" Width="80px" __designer:wfdid="w94" ValidationGroup="A" OnClientClick="return CheckCityVal();"></asp:Button> <asp:Button id="btnCancel" tabIndex=14 onclick="btnCancel_Click" runat="server" Font-Bold="True" Text="Cancel" CssClass="btn btncancel" __designer:wfdid="w95"></asp:Button>&nbsp;<asp:Button id="btnExit" tabIndex=15 onclick="btnExit_Click" runat="server" Font-Bold="True" Text="Exit" CssClass="btn btnexit" __designer:wfdid="w96" onclientclick="return confirm('Are You sure You want to EXIT?')" ></asp:Button> </TD></TR></TBODY></TABLE></TD></TR><TR><TD style="WIDTH: 100%"><asp:GridView id="GVSiteDesc" runat="server" Font-Size="Small" Width="432px" SkinID="grdViewAutoSizeMax" OnPageIndexChanging="GVSiteDesc_PageIndexChanging" AllowPaging="True" PageSize="25" OnRowCommand="GVSiteDesc_RowCommand" BorderColor="Green" AutoGenerateColumns="False" BorderWidth="1px" OnRowEditing="GVSiteDesc_RowEditing"><Columns>
<asp:BoundField DataField="nSTPNo" HeaderText="nSTPNo"></asp:BoundField>
<asp:BoundField DataField="nStateNo" HeaderText="nStateNo"></asp:BoundField>
<asp:BoundField DataField="nCityNo" HeaderText="nCityNo"></asp:BoundField>
<asp:BoundField DataField="vInvestigatorName" HeaderText="Investigator Name"></asp:BoundField>
<asp:BoundField DataField="vMobNo" HeaderText="Mobile No."></asp:BoundField>
<asp:BoundField DataField="vEmail" HeaderText="Email"></asp:BoundField>
<asp:BoundField DataField="vSiteName" HeaderText="Site Name"></asp:BoundField>
<asp:BoundField DataField="vCityName" HeaderText="City Name"></asp:BoundField>
<asp:BoundField DataField="vSiteAddr1" HeaderText="Site Addr1"></asp:BoundField>
<asp:BoundField DataField="vSiteAddr2" HeaderText="Site Addr2"></asp:BoundField>
<asp:BoundField DataField="vSiteAddr3" HeaderText="Site Addr3"></asp:BoundField>
<asp:BoundField DataField="vTelePhoneNo" HeaderText="Telephone No."></asp:BoundField>
<asp:BoundField DataField="vFaxNo" HeaderText="Fax No."></asp:BoundField>
<asp:BoundField DataField="cActiveFlag" HeaderText="Active"></asp:BoundField>
<asp:TemplateField HeaderText="edit"><ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit" runat="server">Edit</asp:LinkButton>
                                                    
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView></TD></TR></TBODY></TABLE>&nbsp; 
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>
