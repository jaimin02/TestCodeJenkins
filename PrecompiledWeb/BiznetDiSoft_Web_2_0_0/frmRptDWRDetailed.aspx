<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmRptDWRDetailed, App_Web_mlepfeoz" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" Runat="Server">

<script type="text/javascript" src="Script/popcalendar.js"></script>
<script type="text/javascript" src="Script/General.js"></script>
<script type="text/javascript" src="Script/Validation.js"></script>

<script type="text/javascript" language="javascript">

    function ValidationForddlUser()
    {
        var chklst = document.getElementById('<%=cblUser.clientid%>');
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
            msgalert('Please Select atleast One User !');
            return false;
        }
        
        if (document.getElementById('<%=txtDWRFromDate.clientid%>').value == '')
        {
            msgalert('Please Enter From Date !');
            return false; 
        }
        if (document.getElementById('<%=txtDWRToDate.clientid%>').value == '')
        {
            msgalert('Please Enter To Date !');
            return false; 
        }
        if(CompareDate(document.getElementById('<%=txtDWRFromDate.ClientID%>').value,document.getElementById('<%=txtDWRToDate.ClientID%>').value)!= true)
        { 
          return false;
          
        }
      return true;
    }

</script>

   
<TABLE align="left"><TBODY><TR><TD style="TEXT-ALIGN: left">
    <strong class="Label">User :</strong></TD><TD style="TEXT-ALIGN: left">
        <asp:CheckBox ID="chkSelectAll" runat="server" Text="Select All" /><br />
        <asp:Panel ID="pnlUser" runat="server" ScrollBars="Auto" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" Height="100px">
            <asp:CheckBoxList ID="cblUser" runat="server" RepeatColumns="2" RepeatDirection="Horizontal">
            </asp:CheckBoxList></asp:Panel>
        </TD></TR>
    <tr>
        <td style="text-align: left">
        </td>
        <td style="text-align: left">
        </td>
    </tr>
    <TR><TD style="TEXT-ALIGN: left">
    <strong class="Label">From Date :</strong></TD><TD style="TEXT-ALIGN: left" class="Label">

        <asp:TextBox id="txtDWRFromDate" runat="server" Width="100px" CssClass="textBox" Enabled="true" ></asp:TextBox> 
        <cc1:CalendarExtender ID="CalExtFromDateForRptDWRDetail" runat="server" TargetControlID="txtDWRFromDate"  Format="dd-MMM-yyyy">
        </cc1:CalendarExtender>

        &nbsp;To:&nbsp;<asp:TextBox id="txtDWRToDate" tabIndex=1 runat="server" Width="100px" CssClass="textBox" Enabled="true"></asp:TextBox> 
        &nbsp;<cc1:CalendarExtender ID="CalExtToDateForRptDWRDetail" runat="server" TargetControlID="txtDWRToDate"  Format="dd-MMM-yyyy"></cc1:CalendarExtender>
    </TD></TR>

    <tr>
        <td style="text-align: left">
        </td>
        <td class="Label" style="text-align: left">
        </td>
    </tr>
    <TR><TD style="TEXT-ALIGN: left">
    <strong class="Label">With Summary :</strong></TD><TD style="TEXT-ALIGN: left"><asp:CheckBox id="chkWithSummary" runat="server" Text="Yes" CssClass="label"></asp:CheckBox></TD></TR>
    <tr>
        <td style="text-align: left">
        </td>
        <td style="text-align: left">
        </td>
    </tr>
    <TR><TD></TD><TD style="TEXT-ALIGN: left;"><asp:Button id="btnGo" onclick="btnGo_Click" runat="server" Text="" CssClass="btn btngo" onclientclick="return ValidationForddlUser();" Font-Bold="True"></asp:Button>
        <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" Text="Cancel" Font-Bold="True" />
        <asp:Button ID="btnExit" runat="server" CssClass="btn btnexit" Text="Exit" Font-Bold="True" onclientclick="return msgconfirmalert('Are you sure you want to Exit?',this); " /></TD></TR></TBODY></TABLE>
</asp:Content>