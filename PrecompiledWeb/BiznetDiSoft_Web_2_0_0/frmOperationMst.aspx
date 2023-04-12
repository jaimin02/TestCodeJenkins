<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmOperationMst, App_Web_22suyskz" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer; 
            * cursor: hand;
        }     
        /*#ctl00_CPHLAMBDA_GV_Operation_wrapper {
            margin: 0px 235px;
        }*/  
    </style>
    <script type="text/javascript" src="Script/scrollablegrid.js"></script>

    <asp:UpdatePanel ID="Up_View" runat="server">
        <ContentTemplate>
            <table style="width: 100%;" cellpadding="5px">
                <tbody>
                     <tr>
            <td>
                <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                    <legend class="LegendText" style="color: Black; font-size: 12px">
                        <img id="img2" alt="Operation Details" src="images/panelcollapse.png"
                            onclick="Display(this,'divOperationDetail');" runat="server" style="margin-right: 2px;" />Operation Details</legend>
                    <div id="divOperationDetail">
                        <table width="98%">
                    <tr>
                        <td  style ="text-align :center ;" colspan="4" class="Label">
                            <asp:RadioButtonList ID="rbtnlstApplicationType" runat="server" RepeatDirection="Horizontal"
                                AutoPostBack="True" style="margin :auto;">
                                <asp:ListItem Value="0">BizNET Web</asp:ListItem>
                                <asp:ListItem Value="1">BizNET Desktop</asp:ListItem>
                                <asp:ListItem Value="2">BizNET LIMS</asp:ListItem>
                                 <asp:ListItem Value="3">BioLyte</asp:ListItem> 
                                 <asp:ListItem Value="4"> IMP Track</asp:ListItem> 
                                <asp:ListItem Value="5">DI Soft</asp:ListItem> 
                                <asp:ListItem Value="6">SDTM</asp:ListItem> 
                                <asp:ListItem Value="7">OIMS</asp:ListItem> 
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="Label" style="text-align: right; width:27%;">
                            Operation Name* :
                        </td>
                        <td style="text-align: left; width:25%;">
                            <asp:TextBox ID="txtOpName" runat="server" CssClass="textBox" Width="90%" MaxLength="100" />
                        </td>
                        <td class="Label" style="text-align: right; width:13%;">
                            Operation Path* :
                        </td>
                        <td style="text-align: left; width:35%;">
                            <asp:TextBox ID="TxtOpPath" runat="server" CssClass="textBox" Width="65%" MaxLength="100" />
                        </td>
                    </tr>
                    <tr>
                        <td class="Label" style="text-align: right;">
                            Parent Operation Code :
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList ID="DDLParentOp" runat="server" CssClass="dropDownList" Width="40%" />
                        </td>
                        <td class="Label" style="text-align: right;">
                            Seq.No.* :
                        </td>
                        <td style="text-align: left;">
                            <input style="width: 65%" id="TxtSeq" class="textBox" type="text" runat="server"
                                maxlength="2" onblur="return Numeric();" />
                        </td>
                    </tr>
                    <tr>
                        <td style="white-space: nowrap; text-align: center;" colspan="4">
                            <asp:Button ID="BtnSave" runat="server" Text="Save" ToolTip="Save" CssClass="btn btnsave"
                                OnClientClick="return Validation();" />
                            <asp:Button ID="BtnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="btn btncancel" />
                            <asp:Button ID="BtnExit" runat="server" Text="Exit" ToolTip="Exit" CssClass="btn btnexit"
                                OnClientClick="return msgconfirmalert('Are You Sure You Want To Exit?',this)" />
                        </td>
                    </tr>
                             </table>
                    </div>
                </fieldset>
            </td>
        </tr>
                </tbody>
            </table>
            <table style="text-align: center; width: 100%; margin-top: 2%;">
                               <tbody>
                    <tr>
                        <td>
                            <fieldset class="FieldSetBox" style="display: block; width: 95.5%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img1" alt="Operation Data" src="images/panelcollapse.png"
                                        onclick="Display(this,'divOperationData');" runat="server" style="margin-right: 2px;" />Operation Data</legend>
                                <div id="divOperationData">
                                    <table style="margin: auto; width: 85%;">
                                        <tr>
                                            <td>
                                <asp:GridView ID="GV_Operation" runat="server" style="width:auto; margin:auto;"
                                     OnRowDataBound="GV_Operation_RowDataBound" OnRowCommand="GV_Operation_RowCommand" AutoGenerateColumns="False"
                                     OnRowCreated="GV_Operation_RowCreated" OnPageIndexChanging="GV_Operation_PageIndexChanging" OnRowEditing="GV_Operation_RowEditing">
                                    <Columns>
                                        <asp:BoundField HeaderText=" # ">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="vOperationCode" HeaderText="Operation Code">
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="vOperationName" HeaderText="Operation Name">
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="vOperationPath" HeaderText="Operation Path">
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="vParentOperationCode" HeaderText="Parent Operation Code">
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="iSeqNo" HeaderText="Seq.No.">
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:TemplateField SortExpression="status" HeaderText="Edit">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="lnkEdit" runat="server" ImageUrl="~/images/Edit2.gif" ToolTip="Edit" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                          </td>
                                        </tr>
                                    </table>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="rbtnlstApplicationType" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>

    <script type="text/javascript" language="javascript">

        function HideSponsorDetails() {
            $('#<%= img2.ClientID%>').click();
          }
        function Display(control, target) {
            if (control.src.toString().toUpperCase().search("EXPAND") != -1) {
                $("#" + target).slideToggle(600);
                control.src = "images/panelcollapse.png";
            }
            else {
                $("#" + target).slideToggle(600);
                control.src = "images/panelexpand.png";
            }
        }

        function Validation() {
            if (document.getElementById('<%=txtOpName.ClientID%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=txtOpName.ClientID%>').value = '';
                msgalert('Please Enter Operation Name');
                document.getElementById('<%=txtOpName.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=TxtOpPath.ClientID%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=TxtOpPath.ClientID%>').value = '';
                msgalert('Please Enter Operation Path');
                document.getElementById('<%=TxtOpPath.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=TxtSeq.ClientID%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=TxtSeq.ClientID%>').value = '';
                msgalert('Please Enter SeqNo.');
                document.getElementById('<%=TxtSeq.ClientID%>').focus();
                return false;
            }
            return true;
        }

        function Numeric() {
            var ValidChars = "0123456789";
            var Numeric = true;
            var Char;

            sText = document.getElementById('<%=TxtSeq.ClientID%>').value;
            for (i = 0; i < sText.length && Numeric == true; i++) {
                Char = sText.charAt(i);
                if (ValidChars.indexOf(Char) == -1) {
                    msgalert('Please Enter Numeric Value');
                    document.getElementById('<%=TxtSeq.ClientID%>').value = "";
                    Numeric = false;
                }
            }

            return Numeric;
        }
        function UIGV_Operation() {
            $('#<%= GV_Operation.ClientID%>').removeAttr('style', 'display:block');
            oTab = $('#<%= GV_Operation.ClientID%>').prepend($('<thead>').append($('#<%= GV_Operation.ClientID%> tr:first'))).dataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": false,
                "sScrollY": "250px",
                "sScrollX": "100%",
                "bScrollCollapse": true,
                 aLengthMenu: [
                     [10, 25, 50, 100, -1],
                     [10, 25, 50, 100, "All"]
                 ],
             });
             return false;
         }
       
    </script>

</asp:Content>
