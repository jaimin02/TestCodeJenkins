<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="frmPifMst.aspx.vb" Inherits="frmPifMst"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" Runat="Server">
<script type="text/javascript" src="Script/popcalendar.js"></script>
<script type="text/javascript">
 function ShowDiv(e, nameDiv)
        {
            var ev = e || window.event
            var dv = document.getElementById(nameDiv);
            if ( dv != null || dv != 'undefined')
            {
                var posY = e.clientY + document.body.scrollTop
			                    + document.documentElement.scrollTop;
                
                dv.style.display = 'block';
                dv.style.top = posY + 15;
                dv.focus();
                return false;
            }
        }

</script>
    
<TABLE>
    <TBODY><TR><TD align=left><STRONG>Personal Information Form&nbsp;</STRONG> 
<HR style="BACKGROUND-IMAGE: none; COLOR: #ffcc66; BACKGROUND-COLOR: #ffcc66" />
&nbsp;</TD></TR><TR><TD align=left>
<TABLE style="WIDTH: 890px"  align=center>
    <TBODY>
        <TR>
            <TD style="WIDTH: 134px; HEIGHT: 20px" align=left>Project No : </TD><TD style="WIDTH: 252px; HEIGHT: 20px" align=left><asp:DropDownList id="ddlprojectno" runat="server" Width="142px"></asp:DropDownList></TD><TD style="WIDTH: 120px; HEIGHT: 20px" align=left>Ref. SOP No:</TD><TD style="HEIGHT: 20px" align=left><asp:Label id="Label2" runat="server" Text="CPMA-02-07"></asp:Label></TD></TR><TR><TD style="WIDTH: 134px" align=left>Subject Id No:</TD><TD style="WIDTH: 252px" align=left><asp:TextBox id="txtsubjectidno" runat="server" CssClass="textBox" Width="140px"></asp:TextBox></TD><TD style="WIDTH: 120px" align=left>Form No:</TD><TD align=left><asp:Label id="Label3" runat="server" Text="5"></asp:Label></TD></TR><TR style="FONT-WEIGHT: bold"><TD style="WIDTH: 134px" align=left>Subject Name:</TD><TD style="WIDTH: 252px" align=left><asp:TextBox id="txtsubjectname" runat="server" CssClass="textBox" Width="140px"></asp:TextBox></TD><TD style="WIDTH: 120px" id="TdUploadLbl" align=left runat="server">Page No:</TD><TD id="TdUpload" align=left runat="server"><asp:Label id="Label4" runat="server" Text="01 to 04"></asp:Label></TD></TR></TBODY></TABLE></TD></TR><TR><TD align=left>Personal Information 
<HR style="BACKGROUND-IMAGE: none; COLOR: #ffcc66; BACKGROUND-COLOR: #ffcc66" />
&nbsp;</TD></TR><TR><TD align=left>
<TABLE style="WIDTH: 890px" align=center>
    <TBODY>
        <TR>
            <TD style="WIDTH: 166px; HEIGHT: 20px" align=left>First Name:</TD>
            <TD style="WIDTH: 252px; HEIGHT: 20px" align=left><asp:TextBox id="txtfirstname" runat="server" CssClass="textBox" Width="140px"></asp:TextBox></TD>
            <TD style="WIDTH: 373px" align=left colSpan=2 rowSpan=12>&nbsp;<asp:Image id="Image1" runat="server" Width="214px" Height="273px" Visible="False"></asp:Image> <BR /><BR /></TD></TR>
        <TR><TD style="WIDTH: 166px; HEIGHT: 23px" align=left>Middle Name:</TD>
            <TD style="WIDTH: 252px; HEIGHT: 23px" align=left>
                <asp:TextBox id="txtmiddlename" runat="server" CssClass="textBox" Width="140px"></asp:TextBox></TD></TR>
        <TR><TD style="WIDTH: 166px" align=left>Last Name:</TD>
            <TD style="WIDTH: 252px" align=left><asp:TextBox id="txtlastname" runat="server" CssClass="textBox" Width="140px"></asp:TextBox></TD></TR>
        <TR><TD style="WIDTH: 166px" align=left>Initials:</TD>
            <TD style="WIDTH: 252px" align=left><asp:TextBox id="txtInitials" runat="server" CssClass="textBox" Width="140px"></asp:TextBox></TD></TR>
        <TR><TD style="WIDTH: 166px" align=left>Date Of Birth:</TD>
            <TD style="WIDTH: 252px" align=left><asp:TextBox id="txtdob" runat="server" CssClass="textBox" Width="140px"></asp:TextBox> 
                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtdob"  Format="dd-MMM-yyyy"></cc1:CalendarExtender>
            </TD></TR>
        <TR><TD style="WIDTH: 166px; HEIGHT: 25px" align=left>Date of Enrollment Reporting:</TD>
            <TD style="WIDTH: 252px; HEIGHT: 25px" align=left>
                <asp:TextBox id="txtdoer" runat="server" Width="135px" Height="14px"></asp:TextBox>&nbsp; 
                <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtdoer"  Format="dd-MMM-yyyy"></cc1:CalendarExtender>

            </TD></TR>
        <TR>
            <TD style="WIDTH: 166px; HEIGHT: 25px" align=left>Sex:</TD>
            <TD style="WIDTH: 252px; HEIGHT: 25px" align=left>&nbsp; &nbsp;&nbsp;&nbsp; <asp:RadioButtonList id="rblsex" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Value="M">Male</asp:ListItem>
                <asp:ListItem Value="F">Female</asp:ListItem>
            </asp:RadioButtonList> <asp:LinkButton id="lnkfemaledt" runat="server">if Female Click Here!!</asp:LinkButton></TD></TR>
        <TR style="COLOR: #000000"><TD style="WIDTH: 166px; HEIGHT: 18px" align=left>Martial Status:</TD>
            <TD style="WIDTH: 252px; HEIGHT: 18px" align=left>&nbsp; &nbsp;&nbsp;&nbsp; <asp:RadioButtonList id="rblMartialStatus" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Value="S">Single</asp:ListItem>
                <asp:ListItem Value="M">Married</asp:ListItem>
            </asp:RadioButtonList></TD></TR>
        <TR>
            <TD style="WIDTH: 166px; HEIGHT: 25px" align=left>Food Habit:</TD>
            <TD style="WIDTH: 252px" align=left rowSpan=3><asp:CheckBoxList id="chfooddetail" runat="server"><asp:ListItem>Vegetarian</asp:ListItem>
<asp:ListItem>Non_vegetarian</asp:ListItem>
<asp:ListItem>Eggetarian</asp:ListItem>
</asp:CheckBoxList></TD></TR><TR><TD style="WIDTH: 166px; HEIGHT: 25px" align=left>&nbsp;</TD></TR><TR><TD style="WIDTH: 166px; HEIGHT: 25px" align=left></TD></TR><TR><TD style="WIDTH: 166px" align=left>Blood Group:</TD><TD style="WIDTH: 252px" align=left><asp:DropDownList id="ddlbloodgroup" runat="server" Width="58px">
        <asp:ListItem>A+Ve</asp:ListItem>
        <asp:ListItem>A-Ve</asp:ListItem>
        <asp:ListItem>B-Ve</asp:ListItem>
        <asp:ListItem>B+Ve</asp:ListItem>
        <asp:ListItem>AB-Ve</asp:ListItem>
        <asp:ListItem>Ab +Ve</asp:ListItem>
        <asp:ListItem>O-Ve</asp:ListItem>
        <asp:ListItem>O+Ve</asp:ListItem>
    </asp:DropDownList></TD></TR><TR><TD style="WIDTH: 166px" align=left>Education Qualification:</TD><TD style="WIDTH: 252px" align=left><asp:TextBox id="txteducationquali" runat="server" CssClass="textBox" Width="140px"></asp:TextBox></TD><TD style="WIDTH: 373px" align=left colSpan=2 rowSpan=1><asp:Button id="Button1" runat="server" Text="Capture Photo" Width="101px"></asp:Button></TD></TR><TR><TD style="WIDTH: 166px; HEIGHT: 21px" align=left>ICF Required In Lang.</TD><TD style="WIDTH: 252px; HEIGHT: 21px" align=left><asp:DropDownList id="ddlicflanguage" runat="server" Width="146px">
              
            </asp:DropDownList></TD>
     <TD style="WIDTH: 373px; HEIGHT: 21px" align=left colSpan=2 rowSpan=1></TD></TR><TR><TD style="WIDTH: 166px" align=left>Occupation:</TD><TD style="WIDTH: 252px" align=left><asp:TextBox id="txtoccupation" runat="server" CssClass="textBox" Width="140px"></asp:TextBox></TD><TD style="WIDTH: 373px" align=left colSpan=2 rowSpan=1></TD></TR><TR><TD style="WIDTH: 166px" align=left>Photocopy Of Proof To be Attached:</TD><TD style="WIDTH: 252px" align=left>&nbsp;<asp:CheckBoxList id="cblProofOfAge" runat="server">
                <asp:ListItem Value="SLC">School Leaving Certificate</asp:ListItem>
                <asp:ListItem Value="BC">Birth Certificate</asp:ListItem>
                <asp:ListItem Value="DL">Driving License</asp:ListItem>
                <asp:ListItem Value="EC">Election Card</asp:ListItem>
                <asp:ListItem Value="IIC">International I-Card</asp:ListItem>
                <asp:ListItem Value="RC">Ration Card</asp:ListItem>
                <asp:ListItem Value="O">Others</asp:ListItem>
            </asp:CheckBoxList></TD><TD style="WIDTH: 373px" align=left colSpan=2 rowSpan=1></TD></TR></TBODY></TABLE></TD></TR>
            
            <TR>
                <TD style="HEIGHT: 56px" align=left><STRONG>Bmi Detail</STRONG> 
                    <HR style="BACKGROUND-IMAGE: none; COLOR: #ffcc66; BACKGROUND-COLOR: #ffcc66" />
                    &nbsp;</TD></TR>
            <TR><TD align=left>
                <TABLE style="WIDTH: 890px" align=center><TBODY>
                    <TR>
                        <TD style="WIDTH: 24px; HEIGHT: 20px" align=left>Height:</TD>
                        <TD style="WIDTH: 252px; HEIGHT: 20px" align=left><asp:TextBox id="txtheight" runat="server" CssClass="textBox" Width="140px"></asp:TextBox></TD></TR>
            <TR><TD style="WIDTH: 24px; HEIGHT: 24px" align=left>Weight:</TD>
                <TD style="WIDTH: 252px; HEIGHT: 24px" align=left><asp:TextBox id="txtweight" runat="server" CssClass="textBox" Width="140px"></asp:TextBox></TD></TR>
            <TR>
                <TD style="WIDTH: 24px" align=left>BMI:</TD>
                <TD style="WIDTH: 252px" align=left><asp:TextBox id="txtbmi" runat="server" CssClass="textBox" Width="140px"></asp:TextBox></TD></TR></TBODY></TABLE></TD></TR>
            <TR><TD style="HEIGHT: 95px" align=left>
                <TABLE style="WIDTH: 890px" align=center>
                    <TBODY><TR><TD style="WIDTH: 145px" align=left rowSpan=1>Smoking/Chewing/Alcohol History:</TD>
                        <TD style="HEIGHT: 21px" align=left colSpan=3><asp:RadioButtonList id="rblhabitdetail" runat="server"><asp:ListItem Value="01">Cigaretts</asp:ListItem>
<asp:ListItem Value="02">Bidi</asp:ListItem>
<asp:ListItem Value="03">Guthka</asp:ListItem>
<asp:ListItem Value="04">Supari -Tobacco</asp:ListItem>
<asp:ListItem Value="05">Alchohol</asp:ListItem>
<asp:ListItem Value="09">Others</asp:ListItem>
</asp:RadioButtonList></TD></TR>
<TR>
    <TD style="WIDTH: 145px" align=left rowSpan=1>Consumption Detail:</TD>
    <TD style="WIDTH: 92px; HEIGHT: 21px" align=left><asp:TextBox id="txtconsumption" runat="server"></asp:TextBox></TD>
    <TD style="WIDTH: 165px; HEIGHT: 21px" id="Td3" align=left runat="server">If Previous,Stopped Since:</TD>
    <TD style="HEIGHT: 21px" id="Td4" align=left runat="server"><asp:TextBox id="txthabithistory" runat="server"></asp:TextBox> 
        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txthabithistory"  Format="dd-MMM-yyyy">
        </cc1:CalendarExtender>

    </TD></TR></TBODY></TABLE>
    </TD></TR>
    
    <TR><TD style="HEIGHT: 56px" align=left><STRONG>Residential/Office Address</STRONG> 
<HR style="BACKGROUND-IMAGE: none; COLOR: #ffcc66; BACKGROUND-COLOR: #ffcc66" />
&nbsp;</TD></TR>
<TR>
    <TD style="HEIGHT: 149px" align=left>
        <TABLE style="WIDTH: 890px" align=center><TBODY>
            <TR>
                <TD style="WIDTH: 134px; HEIGHT: 20px" align=left>Local Address(1):</TD>
                <TD style="WIDTH: 167px; HEIGHT: 20px" align=left><asp:TextBox id="txtlocaladds1" runat="server" CssClass="textBox" Width="246px" TextMode="MultiLine"></asp:TextBox></TD>
                <TD style="WIDTH: 120px; HEIGHT: 20px" align=left>Local Tel1 No:</TD>
                <TD style="HEIGHT: 20px" align=left><asp:TextBox id="txtLocaltel1no" runat="server" CssClass="textBox" Width="140px"></asp:TextBox></TD>
            </TR>
            <TR>
                <TD style="WIDTH: 134px" align=left>Local Address(2):</TD>
                <TD style="WIDTH: 167px" align=left><asp:TextBox id="txtlocaladd2" runat="server" CssClass="textBox" Width="246px" TextMode="MultiLine"></asp:TextBox></TD>
                <TD style="WIDTH: 120px" align=left>Local Tel2 No:</TD>
                <TD align=left><asp:TextBox id="txtLocaltel2no" runat="server" CssClass="textBox" Width="140px"></asp:TextBox>
                </TD></TR>
            <TR><TD style="WIDTH: 134px; height: 34px;" align=left>Permanent Address:</TD>
                <TD style="WIDTH: 167px; height: 34px;" align=left><asp:TextBox id="txtPermanentAdds" runat="server" CssClass="textBox" Width="246px" TextMode="MultiLine"></asp:TextBox></TD>
                <TD style="WIDTH: 120px; height: 34px;" id="Td5" align=left runat="server">Permanent Tel No:</TD>
                <TD id="Td6" align=left runat="server" style="height: 34px"><asp:TextBox id="txtpertelno" runat="server" CssClass="textBox" Width="140px"></asp:TextBox></TD></TR>
            <TR>
                <TD style="WIDTH: 134px" align=left>Office Address:</TD>
                <TD style="WIDTH: 167px" align=left><asp:TextBox id="txtOfficeAddress" runat="server" CssClass="textBox" Width="246px" TextMode="MultiLine"></asp:TextBox></TD>
                <TD style="WIDTH: 120px" id="Td7" align=left runat="server">Office Tel No:</TD>
                <TD id="Td8" align=left runat="server"><asp:TextBox id="txtOfficetelno" runat="server" CssClass="textBox" Width="140px"></asp:TextBox></TD>
            </TR>
            
            </TBODY></TABLE>
                
                
                </TD></TR>
                
                <TR>
                <TD style="HEIGHT: 56px" align=left><STRONG>Contact Person With Address</STRONG> 
<HR style="BACKGROUND-IMAGE: none; COLOR: #ffcc66; BACKGROUND-COLOR: #ffcc66" />
&nbsp;</TD></TR>
    <TR></TR>
    <TR><TD align=left>
        <TABLE style="WIDTH: 890px" align=center><TBODY>
            <TR>
                <TD style="WIDTH: 134px; HEIGHT: 20px" align=left>Contact Person Name:</TD>
                <TD style="WIDTH: 59px; HEIGHT: 20px" align=left><asp:TextBox id="txtconper" runat="server" CssClass="textBox" Width="140px" Height="13px"></asp:TextBox></TD>
                <TD style="WIDTH: 120px; HEIGHT: 20px" align=left>Contact Person Tel1 No:</TD>
                <TD style="HEIGHT: 20px" align=left><asp:TextBox id="txtconpertel1" runat="server" CssClass="textBox" Width="140px" Height="13px"></asp:TextBox></TD>
            </TR>
            <TR><TD style="WIDTH: 134px; HEIGHT: 20px" align=left>Contact Person Adds.(1):</TD>
                <TD style="WIDTH: 59px; HEIGHT: 20px" align=left><asp:TextBox id="txtconperadds1" runat="server" CssClass="textBox" Width="250px" Height="32px" TextMode="MultiLine"></asp:TextBox></TD>
                <TD style="WIDTH: 120px; HEIGHT: 20px" align=left>Contact Person Tel2 No:</TD>
                <TD style="HEIGHT: 20px" align=left><asp:TextBox id="txtconpertel2" runat="server" CssClass="textBox" Width="140px"></asp:TextBox></TD>
            </TR>
            <TR>
                <TD style="WIDTH: 134px; HEIGHT: 20px" align=left>Contact Person&nbsp; Adds(2):</TD>
                <TD style="WIDTH: 59px; HEIGHT: 20px" align=left><asp:TextBox id="txtconperadds2" runat="server" CssClass="textBox" Width="250px" Height="31px" TextMode="MultiLine"></asp:TextBox></TD>
                <TD style="WIDTH: 120px; HEIGHT: 20px" align=left></TD>
                <TD style="HEIGHT: 20px" align=left></TD>
            </TR>
            <TR>
                <TD style="WIDTH: 134px; HEIGHT: 20px" align=left></TD>
                <TD style="WIDTH: 59px; HEIGHT: 20px" align=left></TD>
                <TD style="WIDTH: 120px; HEIGHT: 20px" align=left></TD>
                <TD style="HEIGHT: 20px" align=left></TD>
            </TR>
            
            </TBODY></TABLE>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; <asp:Button id="btnsave" runat="server" Text="Save" Width="81px" CssClass="btn btnsave"></asp:Button></TD></TR><DIV></DIV></TBODY></TABLE>


</asp:Content>

