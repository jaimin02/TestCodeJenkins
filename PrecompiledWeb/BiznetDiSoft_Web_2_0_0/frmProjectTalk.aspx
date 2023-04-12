<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmProjectTalk, App_Web_w1bzwbih" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
   
    <table width="100%">
        <tr>
            <td>
                <table style="width: 100%;" cellpadding="5px">
                    <tr>
                        <td style="text-align: right; width: 40%;" class="Label">
                            Poject No :
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="txtProjNo" runat="server" CssClass="textBox" Width="30%" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;" class="Label">
                            Activity Name :
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="txtActivityName" runat="server" CssClass="textBox" Width="30%" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;" class="Label">
                            Comment Title :
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="TxtTitle" runat="server" CssClass="textBox" Width="30%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;" class="Label">
                            Comment :
                        </td>
                        <td style="text-align: left;">
                            <textarea id="TxtAComment" runat="server" class="textBox" style="width: 30%;" rows="2"
                                cols="0"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;" class="Label">
                            Send To :
                        </td>
                        <td style="text-align: left;">
                            <div id="Div1" style="border-right: gray thin solid; border-top: gray thin solid;
                                overflow: auto; border-left: gray thin solid; width: 50%; border-bottom: gray thin solid;
                                height: 100px">
                                <asp:CheckBoxList ID="chklstUser" runat="server" CssClass="checkboxlist" Font-Name="Verdana"
                                    Font-Names="Verdana" Font-Size="XX-Small" RepeatColumns="3" ForeColor="Black"
                                    Style="width: 100%;">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;" colspan="2" nowrap="noWrap">
                            <asp:Button ID="btnSend" runat="server" CssClass="btn btnsave" Text="Send" ToolTip="Send" />
                            <asp:Button ID="BtnCancel" runat="server" CssClass="btn btncancel" Font-Bold="True" Text="Cancel"
                                ToolTip="Cancel" />
                            <asp:Button ID="BtnBack" runat="server" CssClass="btn btnback" Text="" ToolTip="Back" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div style="left: 197px; top: 430px" id="divMsg" class="DIVSTYLE2" runat="server"
                            visible="false">
                            <asp:Label ID="lblMsg" runat="server" CssClass="Label"></asp:Label>
                            <asp:Button ID="btnExit" OnClick="btnExit_Click" runat="server" Text="Exit" ToolTip="Exit"
                                CssClass="btn btnexit" Width="10%"></asp:Button>
                        </div>
                        <asp:Label ID="LblUnRead" runat="server" Text="UnRead Comments" CssClass="Label" />
                        <asp:GridView ID="GV_UnRead" runat="server" SkinID="grdView" OnRowCreated="GV_UnRead_RowCreated"
                            OnRowCommand="GV_UnRead_RowCommand" AllowSorting="True" AutoGenerateColumns="False">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <input id="Checkbox1" onclick="SelectAll(this,'GV_UnRead')" type="checkbox">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChkSelect" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="vTitle" HeaderText="Title" />
                                <asp:TemplateField HeaderText="Comment">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LnkMsg" runat="server">Read Comment</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="GivenBy" HeaderText="Given By" />
                                <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd-MMM-yyyy}" DataField="dSendDate"
                                    SortExpression="dSendDate" HeaderText="Given On" />
                                <asp:BoundField DataField="vSubject" HeaderText="Message" />
                                <asp:BoundField DataField="vCommentId" HeaderText="CommentId" />
                            </Columns>
                        </asp:GridView>
                        <asp:Button ID="btnRead" OnClick="btnRead_Click" runat="server" Text="Mark as Read"
                            ToolTip="Mark as Read" CssClass="btn btnnew" Width="12%" Enabled="False" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="LblRead" runat="server" Text="Read Comments" CssClass="Label"></asp:Label>
                        <asp:GridView ID="Gv_Read" runat="server" SkinID="grdView" OnRowCreated="Gv_Read_RowCreated"
                            AllowSorting="True" AutoGenerateColumns="False">
                            <Columns>
                                <asp:TemplateField HeaderText="SrNo.">
                                    <ItemTemplate>
                                        <asp:Label ID="LblSr" runat="server" CssClass="Label" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="vTitle" HeaderText="Title" />
                                <asp:TemplateField HeaderText="Comment">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LnkReadMsg" runat="server">Read Comment</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="GivenBy" HeaderText="Given By" />
                                <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd-MMM-yyyy}" DataField="dSendDate"
                                    SortExpression="dSendDate" HeaderText="Given On" />
                                <asp:BoundField DataField="vSubject" HeaderText="Message" />
                                <asp:BoundField DataField="vCommentId" HeaderText="CommentId" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnRead" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <br />
            </td>
        </tr>
    </table>
      <asp:HiddenField ID="hndLockStatus" runat="server" />

    <script type="text/javascript">

        function PageLoad() {

        }

        function ShowDiv(e, nameDiv, disp) {
            msgalert('in');
            var ev = e || window.event;
            var dv = document.getElementById('<%=divMsg.ClientID%>');

            if (dv != null || dv != 'undefined') {
                var posY = e.clientY + document.body.scrollTop
			                    + document.documentElement.scrollTop;

                dv.style.display = disp;
                dv.style.top = posY + 15;
                dv.focus();
                return true;
            }
            else {
                msgalert('Null');
            }
        }
        function SelectAll(CheckBoxControl, Grid) {
            if (CheckBoxControl.checked == true) {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {

                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf(Grid) > -1)) {
                        if (document.forms[0].elements[i].disabled == false) {
                            document.forms[0].elements[i].checked = true;
                        }


                    }


                }

            }

            else {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf(Grid) > -1)) {
                        document.forms[0].elements[i].checked = false;
                    }

                }
            }
        }
        //Add by shivani pandya for project lock

        function getParameterByName(name) {
            name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                results = regex.exec(location.search);
            return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        }

        function getData() {
            var WorkspaceID = getParameterByName('workspaceid');
            $.ajax({
                type: "post",
                url: "frmProjectTalk.aspx/LockImpact",
                data: '{"WorkspaceID":"' + WorkspaceID + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                dataType: "json",
                success: function (data) {
                    if (data.d == "L") {
                        $("#ctl00_CPHLAMBDA_btnSend").attr("Disabled", "Disabled");
                        $("#ctl00_CPHLAMBDA_btnRead").attr("Disabled", "Disabled");
                    }
                },
                failure: function (response) {
                    msgalert(response.d);
                },
                error: function (response) {
                    msgalert(response.d);
                }
            });
            return true;
        }
    </script>

</asp:Content>
