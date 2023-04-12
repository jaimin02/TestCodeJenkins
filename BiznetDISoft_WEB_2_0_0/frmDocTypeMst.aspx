<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="frmDocTypeMst.aspx.vb" Inherits="frmAddDocTypeMst" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

        /*#ctl00_CPHLAMBDA_gvdoctypemst_wrapper {
            margin: 0px 235px;
        }*/
    </style>

    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>

    <script type="text/javascript" language="javascript">

        function HideDocTypeDetails() {
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
            if (document.getElementById('<%=txtDocTypeName.ClientID%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=txtDocTypeName.ClientID%>').value = '';
                msgalert('Please Enter Doc. Type Name !');
                document.getElementById('<%=txtDocTypeName.ClientID%>').focus();
        return false;
    }
    else if (document.getElementById('<%=ddlIndicator.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Indicator !');
                return false;
            }
            else if (document.getElementById('<%=txtReviewedDays.ClientID%>').value.toString().trim().length <= 0) {
        document.getElementById('<%=txtReviewedDays.ClientID%>').value = '';
        msgalert('Please Enter Reviewed Days !');
        document.getElementById('<%=txtReviewedDays.ClientID%>').focus();
        return false;
    }

    return true;
}

function UIgvdoctypemst() {
    $('#<%= gvdoctypemst.ClientID%>').removeAttr('style', 'display:block');
    oTab = $('#<%= gvdoctypemst.ClientID%>').prepend($('<thead>').append($('#<%= gvdoctypemst.ClientID%> tr:first'))).dataTable({
        "bJQueryUI": true,
        "sPaginationType": "full_numbers",
        "bLengthChange": true,
        "iDisplayLength": 10,
        "bProcessing": true,
        "bSort": false,
        aLengthMenu: [
            [10, 25, 50, 100, -1],
            [10, 25, 50, 100, "All"]
        ],
    });
    return false;
}

function Numeric() {
    var ValidChars = "0123456789";
    var Numeric = true;
    var Char;

    sText = document.getElementById('<%=txtReviewedDays.ClientID%>').value;
        for (i = 0; i < sText.length && Numeric == true; i++) {
            Char = sText.charAt(i);
            if (ValidChars.indexOf(Char) == -1) {
                msgalert('Please Enter Numeric value !');
                document.getElementById('<%=txtReviewedDays.ClientID%>').value = "";
           Numeric = false;
       }
   }
    return Numeric;
}
    </script>


    <table style="width: 100%; margin-bottom: 2%;" cellpadding="5px">
         <tr>
     <td>
         <fieldset class="FieldSetBox" style="display: block; width: 96%; margin:auto; text-align: left; border: #aaaaaa 1px solid;">
             <legend class="LegendText" style="color: Black; font-size: 12px">
                 <img id="img2" alt="Document type Details" src="images/panelcollapse.png"
                 onclick="Display(this,'divDocTypeDetail');" runat="server" style="margin-right: 2px;" />Document Type Details</legend>
                     <div id="divDocTypeDetail">
                         <table width="98%">
        <tr>
            <td class="Label" style="width: 29%; text-align: right;">Doc.Type Name* :</td>
            <td style="text-align: left; width: 25%;">
                <asp:TextBox ID="txtDocTypeName" runat="server" Width="60%" CssClass="textBox"></asp:TextBox></td>
       <td align="right" style="width:5%;" class="Label" valign="top">Indicator* :</td>
            <td align="left" valign="top" style="width:33%;">
                <asp:DropDownList ID="ddlIndicator" runat="server" Width="39%" CssClass="dropDownList">
                    <asp:ListItem Value="0">--Select Indicator--</asp:ListItem>
                    <asp:ListItem Value="S">Sample</asp:ListItem>
                    <asp:ListItem Value="T">Template</asp:ListItem>
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td align="right" style="width: 29%;" class="Label" valign="top">Reviewed Days* :</td>
            <td align="left" valign="top" style="width: 25%;">
                <asp:TextBox ID="txtReviewedDays" runat="server" CssClass="textBox" Width="60%"></asp:TextBox></td>
      <td align="right" style="width: 5%;" class="Label" valign="top">Remarks :</td>
            <td align="left" valign="top" style="width: 33%;">
                <asp:TextBox ID="txtremark" runat="server" Width="38%" CssClass="textBox"></asp:TextBox></td>
        </tr>
        <tr id="Tr1" visible="false" runat="server">
            <td align="right" class="Label" valign="top">Doc.TemplateDesc.* :</td>
            <td align="left" valign="top">
                <asp:DropDownList ID="ddldescription" runat="server" Width="200px" CssClass="dropDownList">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td align="center" colspan="4" nowrap="nowrap" valign="top" class="Label">
                <asp:Button ID="btnSave" runat="server" CssClass="btn btnsave" Text="Save" />
                <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="btn btncancel"
                    OnClick="btnCancel_Click" Text="Cancel" />
                <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="btn btnclose" Text="Exit" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" /></td>
        </tr>
                             </table>
                         </div>
             </fieldset>
         </td>
             </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
             <table style="width: 100%; margin-bottom: 2%;" cellpadding="5px">
         <tr>
     <td>
         <fieldset class="FieldSetBox" style="display: block; width: 96%; margin:auto; text-align: left; border: #aaaaaa 1px solid;">
             <legend class="LegendText" style="color: Black; font-size: 12px">
                 <img id="img1" alt="Document type Data" src="images/panelcollapse.png"
                 onclick="Display(this,'divDocTypeData');" runat="server" style="margin-right: 2px;" />Document Type Data</legend>
                     <div id="divDocTypeData">
                         <table style="margin:auto;width:80%">
                             <tr>
                                 <td>
            <asp:GridView ID="gvdoctypemst" runat="server" SkinID="grdViewAutoSizeMax" __designer:wfdid="w43" OnRowCommand="gvdoctypemst_RowCommand" OnRowDataBound="gvdoctypemst_RowDataBound" AutoGenerateColumns="False" OnRowCreated="gvdoctypemst_RowCreated" AllowPaging="True" PageSize="25" OnPageIndexChanging="gvdoctypemst_PageIndexChanging">
                <Columns>
                    <asp:BoundField HeaderText="#">
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>

                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="vDocTypeName" HeaderText="Doc.TypeName">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>

                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="vRemark" HeaderText="Remark">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>

                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="cDocTypeIndi" HeaderText="Indicator"></asp:BoundField>
                    <asp:BoundField DataField="iReviewWithinDays" HeaderText="Reviewed Days"></asp:BoundField>
                    <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd-MMM-yyyy}" DataField="dModifyOn" HeaderText="ModifyOn">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Edit" SortExpression="status">
                        <ItemTemplate>
                            <%--<asp:LinkButton ID="lnkEdit" Text="Edit" runat="server"></asp:LinkButton>--%>
                            <asp:ImageButton ID="lnkEdit" runat="server" ImageUrl="~/images/Edit2.gif" ToolTip="Edit" CommandName="Edit" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <%--<asp:TemplateField SortExpression="status" HeaderText="Edit"><ItemTemplate>
            <asp:LinkButton ID="lnkEdit" Text="Edit" runat="server"></asp:LinkButton>
      
</ItemTemplate>
</asp:TemplateField>--%>
                    <asp:BoundField DataField="vDocTypeCode" HeaderText="DocTypeCode"></asp:BoundField>
                </Columns>
            </asp:GridView>
                                     </td>
                             </tr>
                             </table>
                         </div>
             </fieldset>
         </td>
             </tr>
    </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click"></asp:AsyncPostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

