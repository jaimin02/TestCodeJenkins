<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmSourceComments, App_Web_vq2225em" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" Runat="Server">

<script type="text/javascript" src="Script/AutoComplete.js"></script>

<script type="text/javascript" language ="javascript" src ="Script/popcalendar.js"></script>

<script type="text/javascript" language ="javascript" src ="Script/General.js"></script>
<script type="text/javascript" language ="javascript" src ="Script/Validation.js"></script>

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

    function Validation()
    {
       var chklst = document.getElementById('<%=chkAll.clientid%>');
       if ((document.getElementById('<%=txtProject.clientid%>').value == '' || document.getElementById('<%=HProjectId.clientid%>').value == '') && (!(chklst.checked)))
        {
           msgalert('Please Select Project or Select All !');
            return false; 
        }
          else if (document.getElementById('<%=txtFromDate.ClientID%>').value.toString().trim().length <= 0 ||document.getElementById('<%=txtToDate.ClientID%>').value.toString().trim().length <= 0)
        {
              msgalert('Please Enter Date !');
          document.getElementById('<%=txtFromDate.ClientID%>').focus();
          return false;
        }
          else if(CompareDate(document.getElementById('<%=txtFromDate.ClientID%>').value,document.getElementById('<%=txtToDate.ClientID%>').value)!= true)
        { 
          return false;
          
         }
       return true;
    }

</script>

    <table style="width: 100%">
        <tr>
            <td align="center" style="width: 100%">
                <table align="left" style="width: 100%">

                    <tr>
                        <td class="Label" style="width: 30%; text-align: right" align="left">
                            Project:
                            <br />
                        </td>
                        <td class="Label" style="width: 50%; text-align: left; white-space: nowrap;">
                            <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="622px"></asp:TextBox><asp:CheckBox
                                ID="chkAll" runat="server" Text="All" /><asp:Button
                                ID="btnSetProject" runat="server" OnClick="btnSetProject_Click" Style="display: none"
                                Text=" Project" /><asp:HiddenField ID="HProjectId" runat="server" />
                            <cc1:autocompleteextender id="AutoCompleteExtender1" runat="server" behaviorid="AutoCompleteExtender1"
                                completionlistcssclass="autocomplete_list" completionlisthighlighteditemcssclass="autocomplete_highlighted_listitem"
                                completionlistitemcssclass="autocomplete_listitem" minimumprefixlength="1" onclientitemselected="OnSelected"
                                onclientshowing="ClientPopulated" ServiceMethod="GetProjectCompletionListWithOutSponser" servicepath="AutoComplete.asmx"
                                targetcontrolid="txtProject" usecontextkey="True">

                </cc1:autocompleteextender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%; text-align: right" align="left">
                            <strong class="Label">QA Date : </strong>
                        </td>
                        <td class="Label" style="width: 50%; text-align: left">
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="textBox"
                                Width="100px"></asp:TextBox>
                            <img id="ImgFromDate" alt="Select From Date" onclick="popUpCalendar(this,ctl00_CPHLAMBDA_txtFromDate,'dd-mmm-yy');"
                                src="images/Calendar_scheduleHS.png" />
                            To :
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="textBox"
                                TabIndex="1" Width="100px"></asp:TextBox>
                            <img id="ImgToDate" alt="Select To Date" onclick="popUpCalendar(this,ctl00_CPHLAMBDA_txtToDate,'dd-mmm-yy');"
                                src="images/Calendar_scheduleHS.png" />&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 30%; text-align: right; height: 21px;" align="left">
                        </td>
                        <td class="Label" style="width: 50%; text-align: left; height: 21px;">
                            <asp:Button ID="btnGenerate" runat="server" CssClass="btn btnsave" Text="Generate Report" OnClientClick="return Validation();" />
                            <asp:Button ID="btnExit" runat="server" CssClass="btn btnexit" Text="Exit" onclientclick="return msgconfirmalert('Are you sure you want to Exit?',this);" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>



