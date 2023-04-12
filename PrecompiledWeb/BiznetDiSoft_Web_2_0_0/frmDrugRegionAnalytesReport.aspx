<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmDrugRegionAnalytesReport, App_Web_22suyskz" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/General.js"></script>

    <script type="text/javascript" language="javascript"> 

function ValidationForDrug() 
{
        var chklst = document.getElementById('<%=chklstDrug.clientid%>');
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
            msgalert('Please Select Atleast One Drug !');
            return false;
        }
        
    return true;            
}

    </script>

    <table width="98%">
        <tr>
            <td class="Label" valign="top" align="right" style="width: 35%; text-align: right;
                padding-right: 5px;"> 
                Select Drug :
            </td>
            <td style="width: 65%; text-align: left;">
                <asp:CheckBox ID="chkSelectAll" Text="Select All" runat="server" Style="padding-left: 5px;" />
                <div id="dvChkListDrug" runat="server" style="border-right: gray thin solid; border-top: gray thin solid;
                    overflow-y: scroll; border-left: gray thin solid; width: 212px; border-bottom: gray thin solid;
                    height: 100px; text-align: left;">
                    <asp:CheckBoxList ID="chklstDrug" runat="server" CssClass="checkboxlist" Font-Names="Verdana"
                        Font-Size="XX-Small" ForeColor="Black" Height="37px" Width="260px">
                    </asp:CheckBoxList>
                </div>
                <br />
                <asp:Button ID="btnViewReport" OnClientClick="return ValidationForDrug();" runat="server"
                    CssClass="btn btnnew" Text=" View Report " Font-Bold="True" />
                <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" Text="Cancel" 
                    Font-Bold="True" />
                <asp:Button ID="btnExit" runat="server" CssClass="btn btnexit" Text="Exit"
                    Font-Bold="True" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this); " />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <br />
                <asp:Label ID="lblReport" runat="server"></asp:Label><br />
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:Button ID="btnExportToExcel" runat="server" CssClass="btn btnexcel" 
                     Font-Bold="True" />
            </td>
        </tr>
    </table>
</asp:Content>
