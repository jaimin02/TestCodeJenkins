<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmProtocolWorkSpaceDetails, App_Web_2mzu20n4" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript">

//window.onload = SetFocus;

function ClientPopulated(sender, e)
            {
                var txt = $find('AutoCompleteExtender1');
                var target = txt.get_completionList();
                var items = target.childNodes;
                var searchText = $get('<%= txtsearch.ClientId %>').value;
               
                for (var i=0; i < items.length; i++)
                {
                    var child = items[i];
                    var value = child._value;
                    var startIndex;
                    var len;
                    var strValue;
                    var tempValue;
                    
                    startIndex = value.toUpperCase().indexOf(searchText.toUpperCase());
                    len = searchText.length;
                   strValue = value.substring(0,startIndex);
                    
                    
                    
                        
                    strValue = strValue + '<b>' + value.substring(startIndex,parseInt(strValue.length) + parseInt(len)) + '</b>';
                    tempValue =strValue;
                    
                    tempValue = tempValue.replace('<b>','');
                    tempValue = tempValue.replace('</b>','');
           
                    
                    
                    strValue += value.substring(tempValue.length);
                    
                    strValue = strValue.replace('\'','');
                    //strValue = strValue.replace(/,/g,' ');
                    
                    child.style.width='100%';
                    child.innerHTML = strValue.split('#')[1];
                    
                }
            }
            
function OnSelected(sender,e)
            {
                var strvalue = e.get_value();
                strvalue = strvalue.replace('\'','');
               //strvalue = strvalue.replace(/,/g,' ');
                
               var arrstrvalue = strvalue.split('#');
               $get('<%= txtsearch.clientid %>').value = arrstrvalue[1];
               //$get('<%= txtsearch.clientid %>').value = strvalue;
               $get('<%= hworkspaceid.clientid %>').value = arrstrvalue[0];
               document.getElementById('<%= BtnGo.ClientID%>').click()
            }
            
            
            function SetFocus()
            {
                document.getElementById('<%= txtsearch.ClientId %>').focus();
            }
            
            function ValidateNumeric(txt)
            {
                var ValidChars = "0123456789-";
                var Numeric=true;
                var Char;    
                var sText = txt.value;
                
                for (i = 0; i < sText.length && Numeric == true; i++) 
                { 
                        Char = sText.charAt(i); 
                    if (ValidChars.indexOf(Char) == -1) 
                    {   
                        msgalert('Please Enter Numeric value and You can add "-"');
                        txt.value="";
                        txt.focus();
                        Numeric = false;
                    }
                }
            }
            

    </script>

    <table style="width: 100%">
        <tr>
            <td>
                <strong>Project Name :</strong>
                <asp:TextBox ID="txtsearch" runat="server" CssClass="textBox" Width="240px"></asp:TextBox>&nbsp;
                <asp:Button ID="BtnGo" runat="server" Width="33px" Style="display: none" CssClass="btn btngo"/>
                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                        CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                        CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                        OnClientShowing="ClientPopulated" ServiceMethod="GetProjectCompletionListWithOutSponser"
                        ServicePath="AutoComplete.asmx" TargetControlID="txtsearch" UseContextKey="True">
                    </cc1:AutoCompleteExtender>
                <asp:HiddenField ID="HWorkspaceId" runat="server" />
                <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66; width: 100%;" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="Divgeneral" runat="server">
                            <table style="width: 100%">
                                <tbody>
<TR><TD colSpan=2>
<TABLE cellPadding=2 width="100%"><TBODY>
<TR><TD align=left>Test Product :<asp:TextBox id="txtTestProduct" runat="server" CssClass="textBox" ></asp:TextBox></TD>
<TD align=left>Ref. Product : &nbsp; &nbsp; &nbsp; <asp:TextBox id="txtRefProduct" runat="server" CssClass="textBox" ></asp:TextBox></TD>
<TD align=left>Drug :<asp:TextBox id="txtDrug" runat="server" CssClass="textBox" __designer:wfdid="w118"></asp:TextBox></TD></TR><TR><TD align=left rowSpan=2>Strength : &nbsp; &nbsp; &nbsp; <asp:TextBox id="txtStrength" runat="server" CssClass="textBox" __designer:wfdid="w119"></asp:TextBox></TD><TD align=left rowSpan=2>Formulation Type :<asp:TextBox id="txtFormulation" runat="server" CssClass="textBox" __designer:wfdid="w120"></asp:TextBox></TD><TD rowSpan=2></TD></TR></TBODY></TABLE>
<HR style="BACKGROUND-IMAGE: none; COLOR: #ffcc66; BACKGROUND-COLOR: #ffcc66" />
<TABLE cellPadding=2 width="100%"><TBODY>
<TR><TD align=left><UL><LI>Housing :<asp:TextBox onblur="ValidateNumeric(this);" id="txtHousing" runat="server" CssClass="textBox" Width="50px" ></asp:TextBox> days. &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;</LI>
<LI>Water Qty. :<asp:TextBox onblur="ValidateNumeric(this);" id="txtWaterMin" runat="server" CssClass="textBox" Width="60px" __designer:wfdid="w122"></asp:TextBox>ml <SPAN style="TEXT-DECORATION: underline">+</SPAN> <asp:TextBox onblur="ValidateNumeric(this);" id="txtWaterMax" runat="server" CssClass="textBox" Width="60px" ></asp:TextBox>ml</LI>
<LI>Washout Period : Min <asp:TextBox onblur="ValidateNumeric(this);" id="txtWoshOutMin" runat="server" CssClass="textBox" Width="54px" __designer:wfdid="w124"></asp:TextBox> days, Max <asp:TextBox onblur="ValidateNumeric(this);"  id="txtWoshOutMax" runat="server" CssClass="textBox" Width="54px" ></asp:TextBox> days.</LI></UL></TD>
<TD align=left></TD></TR><TR><TD align=left></TD><TD align=left></TD></TR></TBODY></TABLE></TD></TR><TR><TD colSpan=2><TABLE style="BORDER-RIGHT: #ff9900 thin solid; BORDER-TOP: #ff9900 thin solid; BORDER-LEFT: #ff9900 thin solid; BORDER-BOTTOM: #ff9900 thin solid"><TBODY><TR>
<TD style="BORDER-RIGHT: #ff9933 thin solid; BACKGROUND-COLOR: #ffcc66" align=left><STRONG>Pre-dose Restriction</STRONG></TD><TD style="BACKGROUND-COLOR: #ffcc66" align=left><STRONG>Post-dose Restriction</STRONG></TD></TR><TR><TD style="BORDER-RIGHT: #ff9933 thin solid; BACKGROUND-IMAGE: none; BACKGROUND-COLOR: transparent" align=left><asp:GridView id="GV_Pre" runat="server" SkinID="grdViewSmlSize" __designer:wfdid="w126" AutoGenerateColumns="False" ShowHeader="False">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr. No.">
                                        <ItemTemplate>
                                            &nbsp;<asp:Label ID="LblPreSrNo" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Criterien Description">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPre" runat="server" CssClass="textBox"
                                                Text='<%# Eval("vProtocolCriterienDescription") %>' TextMode="MultiLine" Width="278px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView> </TD><TD style="BACKGROUND-IMAGE: none; BACKGROUND-COLOR: transparent" align=left><asp:GridView id="GV_Post" runat="server" SkinID="grdViewSmlSize"  AutoGenerateColumns="False" ShowHeader="False">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr. No">
                                        <ItemTemplate>
                                            &nbsp;<asp:Label ID="LblPostSrNo" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Criterien Description">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPost" runat="server" CssClass="textBox"
                                                Text='<%# Eval("vProtocolCriterienDescription") %>' TextMode="MultiLine" Width="278px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView> </TD></TR></TBODY></TABLE></TD></TR><TR><TD align=left colSpan=2><STRONG>Sample :<BR />
<HR style="BACKGROUND-IMAGE: none; COLOR: #ffcc66; BACKGROUND-COLOR: #ffcc66" />
</STRONG><TABLE><TBODY><TR><TD><UL><LI>Sampling starts <asp:TextBox onblur="ValidateNumeric(this);" id="txtSampleStarts" runat="server" CssClass="textBox" Width="54px" ></asp:TextBox>
 Hrs. after dosing</LI><LI>Sampling interval <asp:TextBox onblur="ValidateNumeric(this);"  id="txtSampleInterval" runat="server" CssClass="textBox" Width="54px" ></asp:TextBox> </LI>
 <LI>No. of Samples &nbsp; <asp:TextBox onblur="ValidateNumeric(this);" id="txtsampleNo" runat="server" CssClass="textBox" Width="54px" ></asp:TextBox></LI>
 <LI>Sample Qty <asp:TextBox id="txtSampleQty" onblur="ValidateNumeric(this);" runat="server" CssClass="textBox" Width="54px" __designer:wfdid="w131"></asp:TextBox>ml <SPAN style="TEXT-DECORATION: underline">+</SPAN> <asp:TextBox onblur="ValidateNumeric(this);" id="txtAdditionalSampleQty" runat="server" CssClass="textBox" Width="54px" __designer:wfdid="w132"></asp:TextBox>ml</LI></UL></TD></TR></TBODY></TABLE></TD></TR><TR><TD align=center colSpan=2><asp:Button id="BtnSave" runat="server" Text="Save" CssClass="btn btnsave" __designer:wfdid="w133"></asp:Button> <asp:Button id="BtnCancel" runat="server" Text="Cancel" CssClass="btn btncancel" __designer:wfdid="w134"></asp:Button></TD></TR></tbody>
                            </table>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="BtnGo" EventName="Click"></asp:AsyncPostBackTrigger>
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
