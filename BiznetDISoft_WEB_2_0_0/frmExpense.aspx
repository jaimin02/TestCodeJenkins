<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmExpense.aspx.vb" Inherits="frmExpense"  %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/popcalendar.js"></script>

    <script type="text/javascript" src="Script/Validation.js">
    </script>

    <script type="text/javascript" language="javascript">

    function CheckTextAmount(txt)
    {
        if ( txt == null || typeof ( txt ) == 'undefined')
        {
            return;
        }
        else if( CheckDecimal(txt.value) == false)
        {
            txt.value = '';
            txt.focus();
            msgalert('Please Enter Amount In Proper Format !');
            return;            
        }
        
    }    

function ValidationForddlSite()
    {
        if (document.getElementById('<%=ddlSite.clientid %>').selectedIndex == 0)
        {
            msgalert('Select Site');
            return false;            
        }
        else if (document.getElementById('<%=ddlExpType.clientid %>').selectedIndex == 0)
        {
            msgalert('Select Expense Type !');
            return false;            
        }
        else if (document.getElementById('<%=txtExpAmount.clientid %>').value.toString().trim().length <= 0)
        {
            document.getElementById('<%=txtExpAmount.clientid %>').value = '';
            msgalert('Enter Expense Amount !');
            document.getElementById('<%=txtExpAmount.clientid %>').focus();
            return false;            
        }
        return true;
    }
    
    function ValidationForDate()
    {
        if (document.getElementById('<%=txtExpenseFromDate.clientid %>').value == '')
        {
            msgalert('Enter From Date !');
            return false;            
        }
        if (document.getElementById('<%=txtExpenseToDate.clientid %>').value == '')
        {
            msgalert('Enter To Date !');
            return false;            
        }
        return true;
    }
    
    </script>
    
    <asp:UpdatePanel id="UpdatePanel1" runat="server" >
        <contenttemplate>
<TABLE style="WIDTH: 100%" cellPadding=0><TBODY><TR><TD align=left colSpan=2></TD></TR><TR><TD style="WHITE-SPACE: nowrap" align=left colSpan=2></TD></TR><TR><TD style="WHITE-SPACE: nowrap" align=left colSpan=2></TD></TR><TR><TD style="WHITE-SPACE: nowrap" align=left colSpan=2><asp:GridView id="gvwExpenseAdded" runat="server" Width="100%" SkinID="grdViewAutoSizeMax" OnPageIndexChanging="gvwExpenseAdded_PageIndexChanging" PageSize="5" AllowPaging="True" ShowFooter="True" AutoGenerateColumns="False" __designer:wfdid="w127">
<FooterStyle HorizontalAlign="Left"></FooterStyle>
<Columns>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd-MMM-yyyy}" DataField="dFromDate" HeaderText="FromDate"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd-MMM-yyyy}" DataField="dToDate" HeaderText="Todate"></asp:BoundField>
<asp:BoundField DataField="iTotalExpAmt" HeaderText="TotalAmount"></asp:BoundField>
<asp:BoundField DataField="cApprovalFlag" HeaderText="ApprovalFlag"></asp:BoundField>
<asp:TemplateField HeaderText="Edit" ShowHeader="False"><ItemTemplate>
                            <asp:LinkButton ID="lnkbtnEdit" runat="server" CausesValidation="False" CommandName="Select"
                                Text="Edit"></asp:LinkButton>
                        
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView> </TD></TR><TR><TD style="WHITE-SPACE: nowrap" align=left colSpan=2></TD></TR><TR><TD style="WHITE-SPACE: nowrap; HEIGHT: 24px; TEXT-ALIGN: left" class="Label" align=center colSpan=2>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Expense&nbsp;Date :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;<asp:TextBox id="txtExpenseFromDate" runat="server" Width="100px" __designer:wfdid="w128" Enabled="False"></asp:TextBox> <IMG style="disabled: false" id="ImgExpenseFromDate" onclick="popUpCalendar(this,ctl00_CPHLAMBDA_txtExpenseFromDate,'dd-mmm-yyyy');" alt="Select From Date" src="images/Calendar_scheduleHS.png" />&nbsp; To :&nbsp;<asp:TextBox id="txtExpenseToDate" tabIndex=1 runat="server" Width="100px" __designer:wfdid="w129" Enabled="False"></asp:TextBox> <IMG style="disabled: false" id="ImgExpenseToDate" onclick="popUpCalendar(this,ctl00_CPHLAMBDA_txtExpenseToDate,'dd-mmm-yyyy');" alt="Select To Date" src="images/Calendar_scheduleHS.png" />&nbsp; <asp:Button id="btnSearch" tabIndex=2 onclick="btnSearch_Click" runat="server" Text="" CssClass="btn btngo" __designer:wfdid="w130" onclientclick="return ValidationForDate();"></asp:Button></TD></TR><TR><TD style="WHITE-SPACE: nowrap" align=left colSpan=2><%--<asp:Panel id="pnlExpense" runat="server" Width="125px" __designer:wfdid="w103">--%><%--<DIV style="WIDTH: 125px" id="pnlExpense" runat="server">--%><TABLE style="WIDTH: 100%" id="tblExpense" runat="server"><TBODY><TR><TD style="HEIGHT: 24px; TEXT-ALIGN: right "  class="Label">Site :</TD><TD style="HEIGHT: 24px; TEXT-ALIGN: left" class="Label"><asp:DropDownList id="ddlSite" tabIndex=3 runat="server" Width="154px" __designer:wfdid="w131"></asp:DropDownList></TD></TR><TR><TD style="TEXT-ALIGN: right" class="Label">Exp.Type :</TD><TD style="TEXT-ALIGN: left" class="Label"><asp:DropDownList id="ddlExpType" tabIndex=4 runat="server" Width="154px" __designer:wfdid="w132"></asp:DropDownList></TD></TR><TR><TD style="HEIGHT: 26px; TEXT-ALIGN: right" class="Label">Exp.Amount :</TD><TD style="HEIGHT: 26px; TEXT-ALIGN: left" class="Label"><asp:TextBox onblur="CheckTextAmount(this);" id="txtExpAmount" tabIndex=5 runat="server" Width="149px" __designer:wfdid="w133"></asp:TextBox>Rs.</TD></TR><TR><TD style="TEXT-ALIGN: right" class="Label">Remarks :</TD><TD style="TEXT-ALIGN: left" class="Label"><asp:TextBox id="txtRemarks" tabIndex=6 runat="server" CssClass="textbox" __designer:wfdid="w134"></asp:TextBox></TD></TR><TR><TD style="TEXT-ALIGN: right" class="Label">Ref.Detail :</TD><TD style="TEXT-ALIGN: left" class="Label"><asp:TextBox id="txtReferenceDetail" tabIndex=7 runat="server" __designer:wfdid="w135"></asp:TextBox></TD></TR><TR><TD style="HEIGHT: 17px; TEXT-ALIGN: right" class="Label">Ref. Attachment :</TD><TD style="WHITE-SPACE: nowrap; HEIGHT: 17px; TEXT-ALIGN: left" class="Label"><asp:FileUpload id="FlAttachment" tabIndex=8 runat="server" __designer:wfdid="w136"></asp:FileUpload></TD></TR><TR><TD style="HEIGHT: 35px; TEXT-ALIGN: right"></TD><TD style="HEIGHT: 35px" class="Label"><asp:Button id="btnAdd" tabIndex=9 runat="server" Text="Add" CssClass="btn btnnew" __designer:wfdid="w137" onclientclick="return ValidationForddlSite();">
    
    </asp:Button> <asp:Button id="btnCancel" onclick="btnCancel_Click" runat="server" Text="Cancel" CssClass="btn btncancel" __designer:wfdid="w38"></asp:Button> <asp:Button id="btnExit" onclick="btnExit_Click" runat="server" Text="Exit" CssClass="btn btnexit" __designer:wfdid="w39" onclientclick="return msgconfirmalert('Are you sure you want to Exit?',this);"></asp:Button></TD></TR></TBODY></TABLE><%--</DIV>--%><%--</asp:Panel> --%></TD></TR><TR><TD style="WHITE-SPACE: nowrap" align=left colSpan=2></TD></TR><TR><TD style="WHITE-SPACE: nowrap" align=left colSpan=2><asp:GridView id="gvwExpense" runat="server" Width="100%" SkinID="grdViewSml" ShowFooter="True" AutoGenerateColumns="False" __designer:wfdid="w138" OnRowCreated="gvwExpense_RowCreated">
<FooterStyle HorizontalAlign="Left"></FooterStyle>
<Columns>
<asp:BoundField DataField="nOtherExpHdrNo" HeaderText="nOtherExpHdrNo"></asp:BoundField>
<asp:BoundField DataField="nOtherExpDtlNo" HeaderText="nOtherExpDtlNo"></asp:BoundField>
<asp:BoundField DataField="dFromDate" HeaderText="FromDate"></asp:BoundField>
<asp:BoundField DataField="dToDate" HeaderText="ToDate"></asp:BoundField>
<asp:BoundField DataField="nSTPNo" HeaderText="SiteNo"></asp:BoundField>
<asp:BoundField DataField="vSiteName" HeaderText="Site"></asp:BoundField>
<asp:BoundField DataField="nOtherExpMstNo" HeaderText="ExpenseTypeNo"></asp:BoundField>
<asp:BoundField DataField="vOtherExpName" HeaderText="ExpenseType"></asp:BoundField>
<asp:BoundField DataField="iExpAmt" HeaderText="ExpenseAmount">
<FooterStyle HorizontalAlign="Left"></FooterStyle>
</asp:BoundField>
<asp:BoundField DataField="vRemarks" HeaderText="Remarks"></asp:BoundField>
<asp:BoundField DataField="vRefDetail" HeaderText="Ref.Detail"></asp:BoundField>
<asp:TemplateField HeaderText="ref. Attachment"><ItemTemplate>
<asp:HyperLink ID="hlnkFile" runat="server" Target="_blank" Text='<%# Eval("vAttachment") %>'></asp:HyperLink>
</ItemTemplate>
</asp:TemplateField>
<asp:CommandField ShowDeleteButton="True" HeaderText="Delete"></asp:CommandField>
</Columns>
</asp:GridView></TD></TR><TR><TD style="WHITE-SPACE: nowrap" align=left colSpan=2>&nbsp;</TD></TR><TR><TD style="WHITE-SPACE: nowrap" class="Label" align="center" colSpan=2><asp:Button id="btnSave" tabIndex=10 onclick="btnSave_Click" runat="server" Text="Save" CssClass="btn btnsave" __designer:wfdid="w139"></asp:Button></TD></TR></TBODY></TABLE>
</contenttemplate>
        <triggers>
<asp:PostBackTrigger ControlID="btnAdd"></asp:PostBackTrigger>
</triggers>
    </asp:UpdatePanel>
</asp:Content>
