<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmMasterAuditTrail, App_Web_l40sj1d0" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" Runat="Server">
 <script type="text/javascript" src="Script/popcalendar.js"></script>
    <script type= "text/javascript" src="script/AutoComplete.js"></script>

<script type="text/javascript">


        function ValidateDate() {

            var returnValue = 0;

            var Master = document.getElementById('<%=ddlMasterName.ClientID%>');
            var CreatedBy = document.getElementById('<%=ddlcreatedby.ClientID%>');
            var ModifiedBy = document.getElementById('<%=ddlModifiedBy.ClientID%>');

            if (Master.selectedIndex <= 0) {
                msgalert('Please Select Master !');
                return false;
            }

            if (CreatedBy.selectedIndex <= 0) {
                msgalert('Please Select Created By !');
                return false;
            }

            if (ModifiedBy.selectedIndex <= 0) {
                msgalert('Please Select Last Modified By !');
                return false;
            }
         
            return true;

        }

 
    </script>
 
 <asp:UpdatePanel ID="Up_MasterAuditTrail" runat="server" UpdateMode="Conditional"
        RenderMode="Inline">
        <ContentTemplate>
<TABLE cellPadding=0><TBODY><TR><TD><TABLE cellPadding=5><TBODY><TR><TD style="TEXT-ALIGN: left" class="Label">Master Form: </TD><TD align=left colSpan=3><asp:DropDownList id="ddlMasterName" tabIndex=1 runat="server" CssClass="dropDownList" AutoPostBack="True">
                                        <asp:ListItem Value="0"> --Select Master-- </asp:ListItem>
                                        <asp:ListItem Value="1">DrugMst</asp:ListItem>
                                        <asp:ListItem Value="2">UomMst</asp:ListItem>
                                        <asp:ListItem Value="3">LocationMst</asp:ListItem>
                                        <asp:ListItem Value="4">DeptMst</asp:ListItem>
                                        <asp:ListItem Value="5">CountryMst</asp:ListItem>
                                        <asp:ListItem Value="6">SpecilityMst</asp:ListItem>
                                    </asp:DropDownList> </TD></TR><TR><TD style="TEXT-ALIGN: left" class="Label">Created By: </TD><TD style="WHITE-SPACE: nowrap; TEXT-ALIGN: left"><asp:DropDownList id="ddlCreatedBy" tabIndex=2 runat="server" CssClass="dropDownList" AutoPostBack="True">
                                    </asp:DropDownList> </TD><TD style="TEXT-ALIGN: right" class="Label">Created After: </TD><TD style="WHITE-SPACE: nowrap; TEXT-ALIGN: left"><asp:TextBox id="txtCreatedOn" tabIndex=3 runat="server" CssClass="textBox" Width="100px" Enabled="False"></asp:TextBox> <IMG id="Img1" onclick="popUpCalendar(this,ctl00_CPHLAMBDA_txtCreatedOn,'dd-mmm-yy');" alt="Select Created After" src="images/calendar.gif" align=absMiddle runat="server" /> </TD></TR><TR><TD style="TEXT-ALIGN: left" class="Label">Last Modified by: </TD><TD style="WHITE-SPACE: nowrap; TEXT-ALIGN: left"><asp:DropDownList id="ddlModifiedBy" tabIndex=4 runat="server" CssClass="dropDownList" AutoPostBack="True">
                                    </asp:DropDownList> </TD><TD style="TEXT-ALIGN: right" class="Label">Modified After: </TD><TD style="WHITE-SPACE: nowrap; TEXT-ALIGN: left"><asp:TextBox id="txtModifiedOn" tabIndex=5 runat="server" CssClass="textBox" Width="100px" Enabled="False"></asp:TextBox> <IMG id="Img3" onclick="popUpCalendar(this,ctl00_CPHLAMBDA_txtModifiedOn,'dd-mmm-yy');" alt="Select Modified After" src="images/calendar.gif" align=absMiddle runat="server" /> </TD></TR><TR><TD style="TEXT-ALIGN: left" class="Label" align=right><asp:Button id="btnViewReport" tabIndex=6 onclick="btnViewReport_Click" runat="server" Text="" CssClass="btn btngo"  OnClientClick="return ValidateDate();"></asp:Button> </TD><TD style="WHITE-SPACE: nowrap; TEXT-ALIGN: left"><asp:Button id="btncancel" tabIndex=7 onclick="btncancel_Click" runat="server" Text="Cancel" CssClass="btn btncancel"></asp:Button> </TD><TD style="TEXT-ALIGN: right" class="Label">&nbsp; </TD><TD style="WHITE-SPACE: nowrap; TEXT-ALIGN: left">&nbsp;</TD></TR><TR><TD style="TEXT-ALIGN: left" class="Label" align=left colSpan=4>&nbsp;</TD></TR></TBODY></TABLE><asp:GridView id="gvwMasterAudit" runat="server" SkinID="grdViewSml" PageSize="25" AutoGenerateColumns="True" PagerSettings-Position="TopAndBottom" AllowPaging="True" OnPageIndexChanging="gvwMasterAudit_PageIndexChanging" OnRowDataBound="gvwMasterAudit_RowDataBound">
                    <PagerSettings Position="TopAndBottom" />
                    <Columns>
                    </Columns>
                </asp:GridView> </TD></TR></TBODY></TABLE>
</ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnViewReport" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btncancel" EventName="Click" />
        </Triggers>
   </asp:UpdatePanel>

</asp:Content>

