<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="frmDocTypeTemplateMst.aspx.vb" Inherits="frmDocTypeTemplateMst" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>

    <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }
    </style>
    <script type="text/javascript">

        function HideDocTypeTempDetails() {
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

        function UIgvDocTypeTemplate() {
            $('#<%= gvDocTypeTemplate.ClientID%>').removeAttr('style', 'display:block');
            oTab = $('#<%= gvDocTypeTemplate.ClientID%>').prepend($('<thead>').append($('#<%= gvDocTypeTemplate.ClientID%> tr:first'))).dataTable({
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
        function Validation() {
            if (document.getElementById('<%=ddlDocType.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Document Type !');
                return false;
            }
            else if (document.getElementById('<%=ddlLocation.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Location Name !');
                return false;
            }
            else if (document.getElementById('<%=ddlDepartment.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Department !');
                return false;
            }
            else if (document.getElementById('<%=txtDocTemplateName.ClientID%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=txtDocTemplateName.ClientID%>').value = '';
                msgalert('Please Enter Doc Template Name !');
                document.getElementById('<%=txtDocTemplateName.ClientID%>').focus();
             return false;
         }
    return true;
}

function ShowElement(btnid, elemid) {

    var elem = document.getElementById(elemid);
    var btn = document.getElementById(btnid);
    var tl = GetElementTopLeft(btn);

    elem.style.display = 'block';
    SetCenter(elemid);
    //            elem.style.top = tl.PosY - 67; 
    //            elem.style.left= '150px';
}
function HideElement(elemId) {
    var elem = document.getElementById(elemId);
    elem.style.display = 'none';
}
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table>
                <tbody>
                    <tr>
                        <td style="WIDTH: 134px" align="left"></td>
                        <td style="WIDTH: 253px" align="left"></td>
                        <td style="WIDTH: 120px" align="left">
                            <input id="HdfFolder" type="hidden" name="HdfFolder" runat="server" /></td>
                        <td align="left">
                            <input id="HdfFileName" type="hidden" name="HdfFileName" runat="server" />
                            <input id="HdfTranNo" type="hidden" name="HdfTranNo" runat="server" />
                            <input id="HdfBaseFolder" type="hidden" name="HdfBaseFolder" runat="server" /></td>
                    </tr>
                </tbody>
            </table>
            <table style="width: 100%; margin-bottom: 2%;" cellpadding="5px">
                <tbody>
                    <tr>
                        <td>
                            <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img2" alt="Document type Template Details" src="images/panelcollapse.png"
                                        onclick="Display(this,'divDocTypeTempDetail');" runat="server" style="margin-right: 2px;" />Document Type Template Details</legend>
                                <div id="divDocTypeTempDetail">
                                    <table width="98%">
                                        <tr>
                                            <td class="Label" align="right" style="width: 27%">DocType* :</td>
                                            <td class="Label" align="left" style="width: 25%">
                                                <asp:DropDownList ID="ddlDocType" Width="89%" TabIndex="1" runat="server" CssClass="dropDownList" __designer:wfdid="w1"></asp:DropDownList>
                                            </td>
                                            <td class="Label" align="right" style="width: 15%">Location* :</td>
                                            <td class="Label" align="left" style="width: 33%">
                                                <asp:DropDownList ID="ddlLocation" TabIndex="2" Width="47%" runat="server" CssClass="dropDownList" __designer:wfdid="w2"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Label" align="right" style="width: 27%">Department* :</td>
                                            <td class="Label" align="left" style="width: 25%">
                                                <asp:DropDownList ID="ddlDepartment" Width="89%" runat="server" CssClass="dropDownList" __designer:wfdid="w3"></asp:DropDownList>
                                            </td>
                                            <td style="HEIGHT: 20px; width: 15%;" class="Label" align="right">Document Template Name* :&nbsp;</td>
                                            <td style="HEIGHT: 20px; width: 33%;" class="Label" align="left">
                                                <asp:TextBox ID="txtDocTemplateName" Width="46%" runat="server" CssClass="textBox" __designer:wfdid="w19"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Label" align="right" style="width: 27%">Document :</td>
                                            <td class="Label" align="left" style="width: 25%">
                                                <input id="flDocument" class="textBox" width="88%" type="file" name="FlUpload" runat="server" __designer:dtid="1970324836974601" />
                                            </td>
                                            <td style="DISPLAY: none; width: 15%;" class="Label" align="right">ActiveFlag</td>
                                            <td style="DISPLAY: none; WIDTH: 33%" align="left">
                                                <asp:CheckBox ID="chkactive" TabIndex="9" runat="server" CssClass="checkBoxList" __designer:wfdid="w4" Checked="True"></asp:CheckBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="WHITE-SPACE: nowrap" class="Label" align="center" colspan="4">
                                                <asp:Button ID="btnadd" OnClick="btnadd_Click1" runat="server" Text="Add" CssClass="btn btnnew"  Visible="False" __designer:wfdid="w5" Enabled="False"></asp:Button>
                                                <asp:Button ID="BtnGo" TabIndex="4" OnClick="BtnSave_Click" runat="server" Text="Save" CssClass="btn btnnew" __designer:wfdid="w6"></asp:Button>
                                                <asp:Button ID="BtnCancel" TabIndex="5" OnClick="BtnCancel_Click" runat="server" Text="Cancel" CssClass="btn btncancel" __designer:wfdid="w7"></asp:Button>
                                                <asp:Button ID="btnExit" OnClick="btnExit_Click1" runat="server" Text="Exit" CssClass="btn btnexit" __designer:wfdid="w8" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div style="DISPLAY: none; LEFT: 241px; WIDTH: 335px; POSITION: absolute; TOP: 164px; HEIGHT: 134px" id="divAdd" class="divStyleNoAbs" runat="server">
                <table style="WIDTH: 324px; HEIGHT: 102px" cellpadding="5">
                    <tbody>
                        <tr>
                            <td style="WIDTH: 62px" class="Label" valign="top" align="left">Version No:</td>
                            <td class="Label" valign="top" align="left">
                                <asp:TextBox ID="txtdivVersionNo" runat="server" CssClass="textBox" Width="93px" __designer:wfdid="w5"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 62px" class="Label" valign="top" align="left">TemplateName:</td>
                            <td class="Label" valign="top" align="left">
                                <asp:TextBox ID="txtdivTemplateName" runat="server" CssClass="textBox" Width="94px" __designer:wfdid="w6"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 62px" class="Label" valign="top" align="left">
                                <br />
                                Document:</td>
                            <td class="Label" valign="top" align="left">
                                <input style="WIDTH: 166px" id="AdFlUpload" class="textBox" type="file" name="FlUpload" runat="server" __designer:dtid="1970324836974601" />
                            </td>
                        </tr>
                        <tr>
                            <td class="Label" valign="top" align="left"></td>
                            <td class="Label" valign="top" align="left">
                                <asp:Button ID="btnDivAdd" OnClick="btnDivAdd_Click" runat="server" Text="Add" CssClass="btn btnadd" __designer:wfdid="w7"></asp:Button>
                                <asp:Button ID="btndivclose" OnClick="btnDivclose_Click" runat="server" Text="Close" CssClass="btn btnexit" __designer:wfdid="w8"></asp:Button>
                            </td>
                        </tr>
                    </tbody>
                </table>
                &nbsp;
            </div>
            <div style="DISPLAY: none; LEFT: 298px; WIDTH: 335px; POSITION: absolute; TOP: 10px; HEIGHT: 132px" id="DivEdit" class="divStyleNoAbs" runat="server">
                <table style="WIDTH: 314px; HEIGHT: 122px" cellpadding="5">
                    <tbody>
                        <tr>
                            <td class="Label" valign="top" align="left">VersionNo:</td>
                            <td class="Label" valign="top" align="left">
                                <asp:Label ID="lblEditVersion" runat="server" Text="Label" CssClass="Label" __designer:wfdid="w1"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="Label" valign="top" align="left">TemplateName:</td>
                            <td class="Label" valign="top" align="left">
                                <asp:TextBox ID="txtEditTemplateName" runat="server" CssClass="textBox" Width="94px" __designer:wfdid="w2"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="Label" valign="top" align="left">Document:</td>
                            <td class="Label" valign="top" align="left">
                                <input style="WIDTH: 166px" id="AdUpload2" class="textBox" type="file" name="FlUpload" runat="server" __designer:dtid="1970324836974601" />
                            </td>
                        </tr>
                        <tr>
                            <td class="Label" valign="top" align="left"></td>
                            <td class="Label" valign="top" align="left">
                                <asp:Button ID="btnDivupdate" TabIndex="3" OnClick="btnDivupdate_Click" runat="server" Text="Update" CssClass="btn btnupdate" __designer:wfdid="w3"></asp:Button>
                                <asp:Button ID="btnEditclose" TabIndex="4" OnClick="btnEditclose_Click" runat="server" Text="Close" CssClass="btn btnexit" __designer:wfdid="w4"></asp:Button>
                            </td>
                        </tr>
                    </tbody>
                </table>
                &nbsp;
            </div>
            <table style="width: 100%;" cellpadding="5px">
                <tbody>
                    <tr>
                        <td>
                            <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img1" alt="Document type Template Data" src="images/panelcollapse.png"
                                        onclick="Display(this,'divDocTypeTempData');" runat="server" style="margin-right: 2px;" />Document type Template Data</legend>
                                <div id="divDocTypeTempData">
                                    <table style="margin: auto; width: 80%;">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvDocTypeTemplate" runat="server"
                                                    OnRowDeleting="gvDocTypeTemplate_RowDeleting" OnRowEditing="gvDocTypeTemplate_RowEditing"
                                                    OnPageIndexChanging="gvDocTypeTemplate_PageIndexChanging" OnRowCommand="gvDocTypeTemplate_RowCommand"
                                                    OnRowDataBound="gvDocTypeTemplate_RowDataBound" AutoGenerateColumns="False">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="#">
                                                            <EditItemTemplate>
                                                                <asp:TextBox runat="server" ID="TextBox1"></asp:TextBox>
                                                            </EditItemTemplate>

                                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>

                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSrNo" runat="server" __designer:wfdid="w9"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="vDocTemplateId" HeaderText="DocTemplateId"></asp:BoundField>
                                                        <asp:BoundField DataField="vDocTemplateName" HeaderText="TemplateName"></asp:BoundField>
                                                        <asp:BoundField DataField="vVersionNo" HeaderText="VersionNo."></asp:BoundField>
                                                        <asp:BoundField DataField="vUserName" HeaderText="User"></asp:BoundField>
                                                        <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd-MMM-yy}" DataField="dModifyOn" HeaderText="Date">
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="FolderPath">
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="lnkFolderPath" runat="server" Text='<%# Eval("vDocTemplatePath") %>' __designer:wfdid="w70" Target="_blank" NavigateUrl='<%# Eval("vDocTemplatePath") %>'></asp:HyperLink>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Edit">
                                                            <ItemTemplate>
                                                                <%--<asp:LinkButton id="lnkEdit" onclick="lnlEdit_Click" runat="server" __designer:wfdid="w10">Edit</asp:LinkButton>--%>
                                                                <asp:ImageButton ID="lnkEdit" OnClick="lnlEdit_Click" runat="server" ImageUrl="~/images/Edit2.gif" ToolTip="Edit" />
                                                            </ItemTemplate>
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
            <br />
            &nbsp; 
        </ContentTemplate>
        <Triggers>
            <%--<asp:AsyncPostBackTrigger ControlID="BtnGo" EventName="Click"></asp:AsyncPostBackTrigger>--%>
            <asp:PostBackTrigger ControlID="btnDivAdd"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="btnDivupdate"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="flDocument"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="BtnGo"></asp:PostBackTrigger>

        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

