<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmFemaleDetail, App_Web_vq2225em" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
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
    <table>
        <tr>
            <td align="center" style="height: 56px">
                <strong>Personal Information Form(For Woman)</strong>
                <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                &nbsp;</td>
        </tr>
        <tr>
            <td align="left" style="height: 93px">
                <table style="width: 890px" align="center">
                    <tbody>
                        <tr>
                            <td style="width: 134px; height: 20px" align="right" class="Label">
                                Subject Id No :</td>
                            <td style="width: 252px; height: 20px" align="left">
                                <asp:TextBox ID="txtsubjectidno" runat="server" Width="140px" CssClass="textBox"></asp:TextBox></td>
                            <td style="width: 120px; height: 20px" align="right" class="Label">
                                Ref. SOP No :</td>
                            <td style="height: 20px" align="left">
                                <asp:Label ID="Label2" runat="server" Text="CPMA-02-07"></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="width: 134px" align="right" class="Label">
                                Subject Name :</td>
                            <td style="width: 252px" align="left">
                                <asp:TextBox ID="txtsubjectname" runat="server" CssClass="textBox" Width="140px"></asp:TextBox></td>
                            <td style="width: 120px" align="right" class="Label">
                                Form No :</td>
                            <td align="left">
                                <asp:Label ID="Label3" runat="server" Text="06"></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="width: 134px" align="right" class="Label">
                                Date Of Birth :</td>
                            <td style="width: 252px" align="left">
                                <asp:TextBox ID="txtdob" runat="server" CssClass="textBox" Width="140px"></asp:TextBox>
                                &nbsp;<cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtdob"  Format="dd-MMM-yyyy"></cc1:CalendarExtender>
                            </td>
                            <td style="width: 120px" id="TdUploadLbl" align="right" runat="server" class="Label">
                                Page No :</td>
                            <td id="TdUpload" align="left" runat="server">
                                <asp:Label ID="Label4" runat="server" Text="01 to 01"></asp:Label></td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <strong>Last Menstrual Period</strong>
                <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                &nbsp;<table style="width: 890px" align="center">
                    <tr>
                        <td style="width: 134px; height: 20px" align="right" class="Label">
                            Date :</td>
                        <td style="width: 252px; height: 20px" align="left">
                            <asp:TextBox ID="txtmendate" runat="server" CssClass="textBox" Width="140px"></asp:TextBox>&nbsp;
                            &nbsp;<cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtmendate"  Format="dd-MMM-yyyy"></cc1:CalendarExtender>
                        </td>
                        <td style="width: 120px; height: 20px" align="right" class="Label">
                            Cycle Length :</td>
                        <td style="height: 20px" align="left">
                            <asp:TextBox ID="txtcyclelength" runat="server" CssClass="textBox" Width="140px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 134px" align="right" class="Label">
                            Regular :</td>
                        <td style="width: 252px" align="left">
                            <asp:CheckBox ID="chkyes" runat="server" Text="Yes" />
                            <asp:CheckBox ID="chkno" runat="server" Text="No" /></td>
                        <td style="width: 120px" align="left">
                        </td>
                        <td align="left">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" style="height: 56px">
                <strong>Obstetric History</strong>
                <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                &nbsp;</td>
        </tr>
        <tr>
            <td align="left" style="height: 93px">
                <table style="width: 890px" align="center">
                    <tbody>
                        <tr>
                            <td style="width: 134px; height: 20px" align="right" class="Label">
                                Date of Last Delivery :</td>
                            <td style="width: 251px; height: 20px" align="left">
                                <asp:TextBox ID="txtdold" runat="server" CssClass="textBox" Width="140px"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtdold" Format="dd-MMM-yyyy"></cc1:CalendarExtender>
                            </td>
                            <td style="width: 120px; height: 20px" align="right" class="Label">
                                Gravida :</td>
                            <td style="height: 20px" align="left">
                                <asp:TextBox ID="txtgravida" runat="server" CssClass="textBox" Width="140px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 134px; height: 34px;" align="right" class="Label">
                                No.of Live Children :</td>
                            <td style="width: 251px; height: 34px;" align="left">
                                <asp:TextBox ID="txtnoofchildren" runat="server" CssClass="textBox" Width="140px"></asp:TextBox></td>
                            <td style="width: 120px; height: 34px;" align="right" class="Label">
                                Any Spontaneous Abortion/MTP :</td>
                            <td align="left" style="height: 34px">
                                <asp:TextBox ID="txtabortion" runat="server" CssClass="textBox" Width="140px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 134px; height: 18px;" align="right" class="Label">
                                All Children Healthy :</td>
                            <td style="width: 251px; height: 18px;" align="left">
                                &nbsp; &nbsp;
                                <asp:RadioButtonList ID="rblchildrenhealthy" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                    <asp:ListItem Value="N">No</asp:ListItem>
                                </asp:RadioButtonList></td>
                            <td style="width: 120px; height: 18px;" id="Td1" align="right" runat="server" class="Label">
                                Date Of Last Abortion :</td>
                            <td id="Td2" align="left" runat="server" style="height: 18px">
                                <asp:TextBox ID="txtdolabortion" runat="server" CssClass="textBox" Width="140px"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtdolabortion" Format="dd-MMM-yyyy"></cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 134px" class="Label">
                                Para :</td>
                            <td align="left" style="width: 251px">
                                <asp:TextBox ID="txtpara" runat="server" CssClass="textBox" Width="140px"></asp:TextBox></td>
                            <td id="Td3" runat="server" align="right" style="width: 120px" class="Label">
                                Lactating :</td>
                            <td id="Td4" runat="server" align="left">
                                <asp:TextBox ID="txtLactating" runat="server" CssClass="textBox" Width="140px"></asp:TextBox></td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" style="height: 56px">
                <strong>Family Planning Measures</strong>
                <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
            </td>
        </tr>
         <tr>
            <td align="left" style="height: 93px">
                <table style="width: 100%" align="center">
                    <tbody>
                        <tr>
                            <td style="width: 45%; height: 60px" align="right" class="Label">
                                Method Of Contraception :</td>
                            <td align="left" valign="top" colspan="2" style="width:55%; height: 60px">
                                &nbsp; &nbsp;
                                <asp:RadioButtonList ID="rblcontraception" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="P">Permanent</asp:ListItem>
                                    <asp:ListItem Value="T">Temporary</asp:ListItem>
                                </asp:RadioButtonList></td>
                           
                        </tr>
                        <tr>
                            <td style="width: 45%; height: 18px;" align="right" class="Label">
                                Incase Of Self-Temporary :</td>
                            <td style="width: 14%; height: 18px;" align="left">
                                Current
                            </td>
                            <td style="width: 41%; height: 18px;" align="left">
                                &nbsp;Past</td>
                           
                        </tr>
                        <tr>
                            <td style="width: 45%; height: 18px;" align="right" class="Label">
                                Barrier :</td>
                            <td align="left" rowspan="4" style="width: 14%">
                                <asp:RadioButtonList ID="rblcurrent" runat="server" Height="125px">
                                    <asp:ListItem Value="B">Barrier</asp:ListItem>
                                    <asp:ListItem Value="P">Pills</asp:ListItem>
                                    <asp:ListItem Value="R">Rhythm</asp:ListItem>
                                    <asp:ListItem Value="I">IUCD</asp:ListItem>
                                </asp:RadioButtonList></td>
                            <td runat="server" align="left" rowspan="4" style="width:41%">
                                <asp:RadioButtonList ID="rblpast" runat="server" Height ="125px">
                                    <asp:ListItem Value="B">Barrier</asp:ListItem>
                                    <asp:ListItem Value="P">Pills</asp:ListItem>
                                    <asp:ListItem Value="R">Rhythm</asp:ListItem>
                                    <asp:ListItem Value="I">IUCD</asp:ListItem>
                                </asp:RadioButtonList></td>
                          
                        </tr>
                        <tr>
                            <td align="right" style="width: 45%; height: 21px;" class="Label">
                                Pills :</td>
                          
                        </tr>
                        <tr>
                            <td align="right" style="width: 45%; height: 21px" class="Label">
                                Rhythm :</td>
                            
                        </tr>
                        <tr>
                            <td align="right" valign =top  style="width: 45%; height: 25px;" class="Label">
                                IUCD :</td>
                           
                        </tr>
                    </tbody>
                </table>
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<asp:Button
                    ID="Btnsavenback" runat="server" Text="Save & Back" CssClass="btn btnsave" /></td>
        </tr>
       
</asp:Content>
