<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmAddTemplateMst.aspx.vb" Inherits="frmAddTemplateMst" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="Script/scrollablegrid.js"></script>
    <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

        /*#ctl00_CPHLAMBDA_Gvtemplate_wrapper {
            margin: 0px 235px;
        }*/
    </style>
    <table style="width: 100%; margin-top: 2%;" cellpadding="5px">
        <tr>
            <td>
                <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                    <legend class="LegendText" style="color: Black; font-size: 12px">
                        <img id="img2" alt="Template Structure Details" src="images/panelcollapse.png"
                            onclick="Display(this,'divTemplateDetail');" runat="server" style="margin-right: 2px;" />Template Structure Details</legend>
                    <div id="divTemplateDetail">
                        <table width="98%">
                            <tr>
                                <td class="Label" style="text-align: right; width: 15%;">Template Type* :
                                </td>
                                <td style="text-align: left; width: 20%;">
                                    <asp:DropDownList ID="ddlTemplateType" runat="server" CssClass="dropDownList" Width="100%" />
                                </td>
                                <td class="Label" style="text-align: right; width: 10%;">Project Type* :
                                </td>
                                <td style="text-align: left; width: 20%;">
                                    <asp:DropDownList ID="ddlProjectType" runat="server" CssClass="dropDownList" Width="100%" />
                                </td>
                                <td class="Label" style="text-align: right; width: 10%;">Template Name* :
                                </td>
                                <td style="text-align: left; width: 25%;">
                                    <asp:TextBox ID="txtTemplateName" runat="server" CssClass="textBox" Width="80%" MaxLength="100" />
                                </td>
                            </tr>
                            <tr>
                                <td nowrap="nowrap" class="Label" colspan="6" style="text-align: center; vertical-align: top;">
                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btnsave" Text="Save" ToolTip="Save"
                                        OnClientClick="return  Validation()" />
                                    <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="btn btncancel"
                                        Text="Cancel" ToolTip="Cancel" />
                                    <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="btn btnclose"
                                        Text="Exit" ToolTip="Exit" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </fieldset>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table style="width: 100%;" cellpadding="5px">
                <tbody>
                    <tr>
                        <td>
                            <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img1" alt="Template Structure Data" src="images/panelcollapse.png"
                                        onclick="Display(this,'divTemplateData');" runat="server" style="margin-right: 2px;" />Template Structure Data</legend>
                                <div id="divTemplateData">
                                    <table style="margin: auto; width: 80%;">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="Gvtemplate" runat="server" Style="width: 60%; margin: auto;" OnPageIndexChanging="Gvtemplate_PageIndexChanging"
                                                    OnRowCreated="Gvtemplate_RowCreated" AutoGenerateColumns="False"
                                                    OnRowDataBound="Gvtemplate_RowDataBound" OnRowCommand="Gvtemplate_RowCommand"
                                                    OnRowEditing="Gvtemplate_RowEditing" OnRowCancelingEdit="Gvtemplate_RowCancelingEdit">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="#">
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vTemplateDesc" HeaderText="Template Name">
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vProjectTypeName" HeaderText="Project Type" />
                                                        <asp:BoundField DataField="vTemplateTypeName" HeaderText="Template Type Name">
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="dModifyOn" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Modify On"
                                                            HtmlEncode="False">
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="cStatusIndi" HeaderText="Status">
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Add/Edit/Delete Activity" SortExpression="status">
                                                            <ItemTemplate>
                                                                <a href="frmtreenodeMst.aspx?vTemplateId=<%# DataBinder.Eval(Container.Dataitem,"vTemplateId") %>&TemplateName=<%# Eval("vTemplateDesc") %>">
                                                                    <img src="Images/activityaction.png" alt="Add/Edit/Delete Activity" title="Add/Edit/Delete Activity" style="border: none" />
                                                                </a>
                                                            </ItemTemplate>

                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"
                                                                Wrap="False" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Copy Template" SortExpression="status">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgCopy" runat="server" ToolTip="Copy" ImageUrl="~/images/copy.png" />
                                                                <asp:TextBox ID="txtNewName" runat="server" CssClass="textBox" />
                                                                <asp:ImageButton ID="ImgSave" runat="server" ToolTip="Save" ImageUrl="~/images/save.gif" />
                                                                <asp:ImageButton ID="ImgCancel" runat="server" ToolTip="Cancel" ImageUrl="~/images/Cancel.gif" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Edit" SortExpression="status">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgEdit" runat="server" ToolTip="Edit" ImageUrl="~/images/Edit2.gif" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="vTemplateId" HeaderText="Template Id">
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Default Doc. UserRights">
                                                            <ItemTemplate>
                                                                <a href='frmDefaultDocumentUserRights.aspx?mode=1&TemplateId=<%# DataBinder.Eval(Container.Dataitem,"vTemplateId") %>&TemplateName=<%# Eval("vTemplateDesc") %>'>
                                                                    <%--Default Doc. UserRights--%>
                                                                    <img src="Images/userrights.png" alt="Default Doc. UserRights" title="Default Doc. UserRights" style="border: none" />
                                                                </a>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
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
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="Gvtemplate" EventName="RowCommand" />
        </Triggers>
    </asp:UpdatePanel>

    <script type="text/javascript" language="javascript">

        function HideTemplateDetails() {
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

        function UIGvtemplate() {
            $('#<%= Gvtemplate.ClientID%>').removeAttr('style', 'display:block');
            oTab = $('#<%= Gvtemplate.ClientID%>').prepend($('<thead>').append($('#<%= Gvtemplate.ClientID%> tr:first'))).dataTable({
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
        function Validation() {
            if (document.getElementById('<%=ddlTemplateType.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Template Type !');
                return false;
            }
            else if (document.getElementById('<%=ddlProjectType.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Project Type !');
                return false;
            }
            else if (document.getElementById('<%=txtTemplateName.ClientID%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=txtTemplateName.ClientID%>').value = '';
                msgalert('Please Enter Template Name !');
                document.getElementById('<%=txtTemplateName.ClientID%>').focus();
                return false;
            }
    return true;
}
function ShowAlert(msg) {
    //alert(msg);
    //window.location.href = "frmAddTemplateMst.aspx?mode=1";
    alertdooperation(msg, 1, "frmAddTemplateMst.aspx?mode=1");
}


    </script>

</asp:Content>
