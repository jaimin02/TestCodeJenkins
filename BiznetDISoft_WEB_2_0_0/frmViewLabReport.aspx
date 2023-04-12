<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="frmViewLabReport.aspx.vb" Inherits="frmViewLabReport"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" Runat="Server">
<script type="text/javascript" src="Script/AutoComplete.js"></script>
<script type="text/javascript" src="Script/General.js"></script>
<script type="text/javascript">
     
            
       

        function ClientPopulated(sender, e)
        {
            SubjectClientShowing('AutoCompleteExtender1',$get('<%= txtSubject.ClientId %>'));
        }
            
        function OnSelected(sender,e)
        {
            SubjectOnItemSelected(e.get_value(), $get('<%= txtSubject.clientid %>'),
                $get('<%= HSubjectId.clientid %>'),document.getElementById('<%= btnSubject.ClientId %>'));
        }
        
     

</script>   


<table border="0" cellspacing="0" style="border-collapse: collapse" bordercolor="#111111"
  width="998" id="AutoNumber1" cellpadding="0">
<tr>
   <td align="left" style="white-space: nowrap; width: 100%; height: 110px;">
     <asp:Label ID="lblSuject" runat="server" SkinID="lblDisplay" Text="Subject: " CssClass="Label"></asp:Label>
     <asp:TextBox ID="txtSubject" runat="server" CssClass="textBox" TabIndex="2" Width="480px"></asp:TextBox>
     <asp:Button ID="btnSubject" runat="server" Style="display: none" Text="Subject" />
       &nbsp;
     <asp:HiddenField ID="HSubjectId" runat="server" />
     <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
       MinimumPrefixLength="1" OnClientItemSelected="OnSelected" OnClientShowing="ClientPopulated"
       ServiceMethod="GetSubjectCompletionList_NotRejected" ServicePath="AutoComplete.asmx" TargetControlID="txtSubject"
       UseContextKey="True">
     </cc1:AutoCompleteExtender>
      <asp:RadioButtonList ID="rblScreeningDate" runat="server" AutoPostBack="True" RepeatColumns="3"
         RepeatDirection="Horizontal" CssClass="Label"></asp:RadioButtonList>
                                                                        
     </td> 
     </tr>
     </table> 
   <TABLE align=center><TBODY><TR><TD align=left colSpan=2><asp:GridView id="GV_LabReport" runat="server" Width="768px" SkinID="grdViewSml" OnPageIndexChanging="GV_LabReport_PageIndexChanging" AllowPaging="True" PageSize="25" AutoGenerateColumns="False" OnRowDataBound="GV_LabReport_RowDataBound"><Columns>
<asp:BoundField HeaderText="#"></asp:BoundField>
<asp:BoundField DataField="vSubjectId" HeaderText="Subject Id"></asp:BoundField>
<asp:BoundField DataField="FullName" HeaderText="FullName"></asp:BoundField>
<asp:BoundField DataField="vSampleId" HeaderText="Sample Id"></asp:BoundField>
<asp:BoundField DataField="vMedExDesc" HeaderText="MedExDesc"></asp:BoundField>
<asp:BoundField DataField="vMedexResult" HeaderText="Medex Result"></asp:BoundField>
<asp:BoundField DataField="cNormalflag" HeaderText="Normal flag"></asp:BoundField>
<asp:BoundField DataField="cAbnormalflag" HeaderText="Abnormal flag"></asp:BoundField>
<asp:BoundField DataField="cClinicallySignflag" HeaderText="Clinically Signflag"></asp:BoundField>
<asp:BoundField DataField="vGeneralRemark" HeaderText="General Remark"></asp:BoundField>
<asp:BoundField DataField="vPlanOfAction" HeaderText="Plan Of Action"></asp:BoundField>

 </Columns>
</asp:GridView> </TD></TR></TBODY></TABLE>
</asp:Content>

