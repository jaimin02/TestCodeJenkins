<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmSubjectLanguageMst.aspx.vb" Inherits="frmSubjectLanguageMst" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="Script/scrollablegrid.js"></script>
    <script type="" src="Script/General.js"></script>
    <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }
        /*#ctl00_CPHLAMBDA_gvwSubjectLanguageMst_wrapper {
            margin: 0px 235px;
        }*/
    </style>
    <asp:UpdatePanel ID="upAddDeptMst" runat="server">
        <ContentTemplate>
            <table cellpadding="5px" style="width: 100%;">
                <tbody>
                    <tr>
                        <td>
                            <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img2" alt="Language Details" src="images/panelcollapse.png"
                                        onclick="Display(this,'divLanguageDetail');" runat="server" style="margin-right: 2px;" />Language Details</legend>
                                <div id="divLanguageDetail">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 23%; text-align: right" class="Label">Language*:
                                            </td>
                                            <td style="width: 20%; text-align: left;">
                                                <asp:TextBox ID="txtLanguage" runat="server" CssClass="textBox" Width="100%" MaxLength="50" />
                                            </td>
                                            <td style="width: 9%; text-align: right" class="Label">Active:
                                            </td>
                                            <td style="width: 3%; text-align: left;" class="Label">
                                                <asp:CheckBox ID="ChkActive" runat="server" Checked="True" />
                                            </td>
                                            <td class="Label" style="width: 9%; text-align: right">Remarks* :
                                            </td>
                                            <td style="width: 36%; text-align: left">
                                                <asp:TextBox ID="txtRemark" runat="server" Rows="3" Columns="20" CssClass="textBox" Style="width: 51%; height: auto;" TextMode="MultiLine" MaxLength="500" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center;" colspan="6">
                                                <asp:Button ID="btnSave" runat="server" Text=" Save " CssClass="btn btnsave" ToolTip="Save"
                                                    OnClientClick="return Validation();" />
                                                <asp:Button ID="btnExportToExcelGrid" runat="Server" Font-Size="Smaller" CssClass="btn btnexcel" ToolTip="Export To Excel" />
                                                <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Text=" Cancel "
                                                    CausesValidation="False" CssClass="btn btncancel" ToolTip="Cancel" />
                                                <asp:Button ID="btnClose" OnClick="btnClose_Click" runat="server" Text="Exit" CssClass="btn btnclose"
                                                    CausesValidation="False" ToolTip="Exit" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
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
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upGVWDeptMst" runat="server">
        <ContentTemplate>
            <table style="width: 100%;" cellpadding="5px">
                <tbody>
                    <tr>
                        <td>
                            <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img1" alt="Language Data" src="images/panelcollapse.png"
                                        onclick="Display(this,'divLanguageData');" runat="server" style="margin-right: 2px;" />Language Data</legend>
                                <div id="divLanguageData">
                                    <table style="margin: auto; width: 80%;">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvwSubjectLanguageMst" runat="server" Style="display: none; width: 55%; margin: auto;" OnPageIndexChanging="gvwSubjectLanguageMst_PageIndexChanging"
                                                    OnRowDataBound="gvwSubjectLanguageMst_RowDataBound"
                                                    AutoGenerateColumns="False" OnRowCreated="gvwSubjectLanguageMst_RowCreated" OnRowCommand="gvwSubjectLanguageMst_RowCommand">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Sr. No">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" Font-Underline="False" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vLanguageId" HeaderText="LanguageId">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" Font-Underline="False" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vLanguageName" HeaderText="Language Name">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Center" Font-Underline="False" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="cActiveFlag" HeaderText="Active"></asp:BoundField>
                                                        <asp:TemplateField SortExpression="status" HeaderText="Edit">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" Font-Underline="False" VerticalAlign="Middle" />
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="lnkEdit" runat="server" ImageUrl="~/images/Edit2.gif" ToolTip="Edit" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Audit Trail">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="lnkAudit" runat="server" ImageUrl="~/Images/audit.png" ToolTip="Audit Trial" OnClientClick="AudtiTrail(this); return false;" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Export">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <img src="images/Export.gif" onclick="ExportToExcel(this.id);" id='<%# Eval("vLanguageId")%>' />
                                                                </center>
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
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnClose" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnExportToExcelGrid" />
        </Triggers>
    </asp:UpdatePanel>

    <div>
        <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpSubjectLanguageMstAuditTrail" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div id="dvSubjectLanguageMstAuditTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
                                <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                    <tr>
                                        <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 97%;">
                                            <asp:Label ID="lblRamge" runat="server" Text="Audit Trail Information"></asp:Label>
                                        </td>
                                        <td style="width: 3%">
                                            <img id="imgSubjectLanguageMstAuditTrail" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <table align="center" border="0" cellpadding="2" cellspacing="2" width="100%">
                                                <tr>

                                                    <td>
                                                        <table id="tblSubjectLanguageMstAudit" class="tblAudit" width="100%" style="background-color: aliceblue;"></table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <hr />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    <button id="btn3" runat="server" style="display: none;" />

    <cc1:ModalPopupExtender ID="MPE_SubjectLanguageMstHistory" runat="server" PopupControlID="dvSubjectLanguageMstAuditTrail" BehaviorID="MPE_SubjectLanguageMstHistory"
        PopupDragHandleControlID="LbldRandomizationDtl" BackgroundCssClass="modalBackground" CancelControlID="imgSubjectLanguageMstAuditTrail"
        TargetControlID="btn3">
    </cc1:ModalPopupExtender>

    <div id="DivExports" runat="server">
        <asp:GridView runat="server" ID="gvExport" AutoGenerateColumns="true" Style="display: none"
            HeaderStyle-BackColor="#1560a1" HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" Header-style-font-size="10px !importnat" HeaderStyle-Font-Names="Verdana, Arial, Helvetica, sans-serif"
            HeaderStyle-Font-Size=" 0.9em"
            HeaderStyle-HorizontalAlign="Center" CellPadding="3" CellSpacing="0"
            RowStyle-Font-Names=" Verdana, Arial, Helvetica, sans-serif" RowStyle-Font-Size="10pt"
            RowStyle-BackColor="#84c8e6" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#191970" RowStyle-ForeColor="#191970">
        </asp:GridView>
    </div>
    <div id="DivExportToExcel" runat="server">
        <asp:GridView runat="server" ID="gvExportToExcel" AutoGenerateColumns="true" Style="display: none"
            HeaderStyle-BackColor="#1560a1" HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" Header-style-font-size="10px !importnat" HeaderStyle-Font-Names="Verdana, Arial, Helvetica, sans-serif"
            HeaderStyle-Font-Size=" 0.9em"
            HeaderStyle-HorizontalAlign="Center" CellPadding="3" CellSpacing="0"
            RowStyle-Font-Names=" Verdana, Arial, Helvetica, sans-serif" RowStyle-Font-Size="10pt"
            RowStyle-BackColor="#84c8e6" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#191970" RowStyle-ForeColor="#191970">
        </asp:GridView>
    </div>
    <asp:HiddenField runat="server" ID="hdnLanguageId" />
    <asp:Button ID="btnExportToExcel" runat="server" Style="display: none;" />

    <script type="text/javascript">

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

        function AudtiTrail(e) {
            var vLanguageId = $("#" + e.id).attr("vLanguageId");

            if (vLanguageId != "") {
                $.ajax({
                    type: "post",
                    url: "frmSubjectLanguageMst.aspx/AuditTrail",
                    data: '{"vLanguageId":"' + vLanguageId + '"}',
                    contentType: "application/json; charset=utf-8",
                    datatype: JSON,
                    async: false,
                    success: function (data) {
                        $('#tblSubjectLanguageMstAudit').attr("IsTable", "has");
                        var aaDataSet = [];
                        var range = null;

                        if (data.d != "" && data.d != null) {
                            data = JSON.parse(data.d);
                            for (var Row = 0; Row < data.length; Row++) {
                                var InDataSet = [];
                                InDataSet.push(data[Row].SrNo, data[Row].LanguageName, data[Row].ActiveFlag, data[Row].Remark, data[Row].ModifyBy, data[Row].ModifyOn);
                                aaDataSet.push(InDataSet);
                            }
                        }
                        if ($("#tblSubjectLanguageMstAudit").children().length > 0) {
                            $("#tblSubjectLanguageMstAudit").dataTable().fnDestroy();
                        }
                        oTable = $('#tblSubjectLanguageMstAudit').prepend($('<thead>').append($('#tblSubjectLanguageMstAudit tr:first'))).dataTable({

                            "bJQueryUI": true,
                            "sPaginationType": "full_numbers",
                            "bLengthChange": true,
                            "iDisplayLength": 10,
                            "bProcessing": true,
                            "bSort": false,
                            "aaData": aaDataSet,
                            "autowidth": false,
                            aLengthMenu: [
                                [10, 25, 50, 100, -1],
                                [10, 25, 50, 100, "All"]
                            ],
                            "aoColumns": [
                                {
                                    "sTitle": "Sr. No",
                                },
                                   { "sTitle": "Language Name" },
                                   { "sTitle": "Active" },
                                   { "sTitle": "Remarks" },
                                   { "sTitle": "Modify By" },
                                   { "sTitle": "Modify On" },

                            ],
                            "aoColumnDefs": [
                                        { 'bSortable': false, 'aTargets': [0] }
                            ],
                            "oLanguage": {
                                "sEmptyTable": "No Record Found",
                            }

                        });
                        oTable.fnAdjustColumnSizing();
                        $('.DataTables_sort_wrapper').click;
                        $find('MPE_SubjectLanguageMstHistory').show();
                        $('.dataTables_filter input').addClass('textBox');
                    },
                    failure: function (response) {
                        msgalert(response.d);
                    },
                    error: function (response) {
                        msgalert(response.d);
                    }
                });
            }
            return false;

        }
        function UIgvwSubjectLanguageMst() {
            $('#<%= gvwSubjectLanguageMst.ClientID%>').removeAttr('style', 'display:block');
            oTab = $('#<%= gvwSubjectLanguageMst.ClientID%>').prepend($('<thead>').append($('#<%= gvwSubjectLanguageMst.ClientID%> tr:first'))).dataTable({
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
        function ExportToExcel(id) {
            $("#<%= hdnLanguageId.ClientID()%>").val(id);
            var btn = document.getElementById('<%= btnExportToExcel.ClientId()%>')
            btn.click();
        }
        function Validation() {
            if (document.getElementById('<%=txtLanguage.ClientID%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=txtLanguage.ClientID%>').value = '';
                document.getElementById('<%=txtLanguage.ClientID%>').focus();
                msgalert('Please Enter Language !');
                return false;
            }
            if (document.getElementById("<%=btnSave.ClientID%>").value.trim() == "Update") {

                if (document.getElementById("<%=txtRemark.ClientID%>").value.trim() == "") {
                    msgalert("Please Enter Remarks !");
                    return false;
                }
            }
            return true;
        }
        // for alert message added on 26-oct-2012
        function ShowAlert(msg) {
            //alert(msg);
            //window.location.href = "frmSubjectLanguageMst.aspx?mode=1";
            alertdooperation(msg, 1, "frmSubjectLanguageMst.aspx?mode=1");
        }


    </script>

</asp:Content>
