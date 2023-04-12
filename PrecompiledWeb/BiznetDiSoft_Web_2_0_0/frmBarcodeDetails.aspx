<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmBarcodeDetails, App_Web_qa4vhgvp" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" Runat="Server">

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    

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
  
  

<TABLE><TBODY><tr>
            <td class="Label" style="width: 25%; white-space: nowrap;" align="right">
            Project: 
            </td>
            <td align="left" style="width:75%" >
                <asp:TextBox id="txtproject" runat="server" CssClass="textBox" Width="622px" TabIndex="1"></asp:TextBox><asp:Button style="DISPLAY: none" id="btnSetProject" onclick="btnSetProject_Click" runat="server" Text=" Project"></asp:Button><asp:HiddenField id="HProjectId" runat="server">
                </asp:HiddenField><cc1:AutoCompleteExtender id="AutoCompleteExtender1" runat="server" UseContextKey="True" TargetControlID="txtProject" ServicePath="AutoComplete.asmx" ServiceMethod="GetMyProjectCompletionList" OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1" CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1">
                    </cc1:AutoCompleteExtender>
            </td>
        </tr>
        <tr>
            <td class="Label" style="width: 25%; white-space: nowrap;" align="right">
            Period: 
            </td>
            <td align="left" style="width:75%" >
                <asp:DropDownList id="ddlPeriod" runat="server" CssClass="dropDownList" Width="192px" OnSelectedIndexChanged="ddlPeriod_SelectedIndexChanged" AutoPostBack="True" TabIndex="2"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="Label" style="width: 25%; white-space: nowrap; height: 21px;" align="right">
            Activity: 
            </td>
            <td align="left" style=" width:75% ;height: 21px">
                <asp:DropDownList id="ddlActivity" runat="server" CssClass="dropDownList" Width="588px" OnSelectedIndexChanged="ddlPeriod_SelectedIndexChanged" AutoPostBack="True" TabIndex="3"></asp:DropDownList>
            </td>
 </tr> <tr><td colspan =2>
   <!--  nSampleTypeDetailNo,nSampleId,cSampleTypeCode,vSampleBarCode,iCollectionBy,CollectedBy,dCollectionDateTime,iModifyBy,dModifyOn,cStatusIndi,nSampleTypeMstNo,vSampleTypeDesc,cSize,vSampleId,vLocationCode,vSubjectID,vWorkSpaceId,vActivityId,iNodeId,iPeriod,iMySubjectNo,vLocationName,vActivityName,vNodeDisplayName,vWorkSpaceDesc,nRefTime,FullName,vProjectTypeCode -->           
 <asp:GridView ID="gvwBarcodeDtl" runat="server" OnPageIndexChanging="gvwBarcodeDtl_PageIndexChanging" AllowPaging="True"  AutoGenerateColumns="False"  SkinID="grdViewSml" Width="100%" PageSize="15">
                        <Columns>
                    
<asp:BoundField DataField="nSampleTypeDetailNo" HeaderText="nSampleTypeDetailNo" Visible =false ></asp:BoundField>
<asp:BoundField DataField="vSampleBarCode" HeaderText="Sample"></asp:BoundField>
<asp:BoundField DataField="vSampleTypeDesc" HeaderText="Sample Type"></asp:BoundField>
<asp:BoundField DataField="vLocationName" HeaderText="Location"></asp:BoundField>
<asp:BoundField DataField="vWorkspacedesc" HeaderText="Project"></asp:BoundField>
<asp:BoundField DataField="vSubjectID" HeaderText="SubjectId"></asp:BoundField>
<asp:BoundField DataField="FullName" HeaderText="Subject"></asp:BoundField>
<asp:BoundField DataField="vNodeDisplayName" HeaderText="Activity"></asp:BoundField>
<asp:BoundField DataField="dCollectionDateTime" HeaderText="Collection Date" HtmlEncode="False" ></asp:BoundField>
<asp:BoundField DataField="CollectedBy" HeaderText="Collected By"></asp:BoundField>
                
                </Columns> 
                </asp:GridView> 
                
                
                
                
                
                </td></tr></TBODY></TABLE>




</asp:Content>

