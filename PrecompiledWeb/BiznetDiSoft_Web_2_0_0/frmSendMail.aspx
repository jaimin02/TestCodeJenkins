<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmSendMail, App_Web_2mzu20n4" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" language="javascript">
     function closewindow()
       { 
            var parWin = window.opener;
            if (parWin != null && typeof (parWin) != 'undefined') 
            {
                parWin.RefreshPage();
                self.close();
            }
       }

    </script>

    <div>
        <asp:UpdatePanel ID="UpdatePanelSendMail" runat="server">
            <contenttemplate>
<asp:panel id="PnlMailSend" runat="server" BackColor="#E0ECFF" BorderStyle="Solid" BorderColor="#6694E3"><TABLE border=0><TBODY><TR align=center><TD align=center><TABLE><TBODY><TR><TD align=left>To : </TD><TD style="TEXT-ALIGN: left"><asp:textbox id="TxtTo" runat="server" Text="" Width="450px" BorderStyle="Solid" BorderColor="#6694E3" TextMode="MultiLine"></asp:textbox> </TD></TR><TR><TD align=left>Cc : </TD><TD style="TEXT-ALIGN: left"><asp:TextBox id="TxtCc" runat="server" Width="450px" BorderStyle="Solid" BorderColor="#6694E3"></asp:TextBox> </TD></TR><TR><TD align=left>Subject : </TD><TD style="TEXT-ALIGN: left"><asp:TextBox id="TxtSubject" runat="server" Width="450px" BorderStyle="Solid" BorderColor="#6694E3"></asp:TextBox> </TD></TR><TR><TD align=left>Remarks : </TD><TD style="TEXT-ALIGN: left"><asp:TextBox id="TxtBody" runat="server" Text="" Width="450px" BorderStyle="Solid" BorderColor="#6694E3" TextMode="MultiLine"></asp:TextBox> </TD></TR><TR><TD></TD><TD style="TEXT-ALIGN: left"><asp:Button id="BtnSend" runat="server" Text="Send" CssClass="btn btnsave"></asp:Button> <asp:button id="BtnCancel" runat="server" Text="Close" CssClass="btn btncancel" OnClientClick="return closewindow();"></asp:button> </TD></TR></TBODY></TABLE><TABLE><TBODY><TR><TD colSpan=2><asp:PlaceHolder id="PlcHolder" runat="server">
                            <asp:Panel  runat="server" ID="PnlDetailsToBeSend" BorderStyle="Solid" BorderColor="#6694E3"
                                ScrollBars="Vertical" BackColor="white">
                                <%= session("Body") %></asp:Panel>
                        </asp:PlaceHolder> </TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></asp:panel> 
</contenttemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
