<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmDeptMst.aspx.vb" Inherits="frmDeptMst" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="conDeptMst" ContentPlaceHolderID="CPHLAMBDA" runat="Server">


    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="Script/Gridview.js"></script>
    <script type="text/javascript" src="Script/scrollablegrid.js"></script>
    <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

        /*#ctl00_CPHLAMBDA_gvwDeptMst_wrapper {
            margin: 0px 235px;
        }*/
    </style>
    <asp:UpdatePanel ID="upAddDeptMst" runat="server">
                    <ContentTemplate>
                         <table style="width: 100%;" cellpadding="5px">
                <tbody>
                    <tr>
                        <td>
                            <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img2" alt="Department Details" src="images/panelcollapse.png"
                                        onclick="Display(this,'divDepartmentDetail');" runat="server" style="margin-right: 2px;" />Department Details</legend>
                                <div id="divDepartmentDetail">
                                    <table style="margin: auto; width: 80%;">
                                        <tr>
                                    <td class="Label" style="width: 30%; text-align: right">Department Name* :
                                    </td>
                                    <td style="text-align: left; width: 25%;">
                                        <asp:TextBox ID="txtDeptName" runat="server" CssClass="textBox" Width="100%" MaxLength="50" />
                                    </td>
                                    <td class="Label" style="width: 8%; text-align: right">Remarks* :
                                    </td>
                                    <td style="text-align: left; width:37%;">
                                        <asp:TextBox ID="txtRemarks" runat="server" Rows="3" Columns="20" CssClass="textBox" Style="width: 60%; height: auto;" TextMode="MultiLine" MaxLength="500" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="white-space: nowrap; text-align: center" class="Label" colspan="4">
                                        <asp:Button ID="btnSave" runat="server" Text=" Save " CssClass="btn btnsave" ToolTip="Save"
                                            OnClientClick="return Validation() " />
                                        <asp:Button ID="btnExportToExcelGrid" runat="Server" Font-Size="Smaller" CssClass="btn btnexcel"  ToolTip="Export To Excel" />
                                        <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Text=" Cancel "
                                            CssClass="btn btncancel" ToolTip="Cancel" CausesValidation="False" />
                                        <asp:Button ID="btnClose" OnClick="btnClose_Click" runat="server" Text="Exit" CssClass="btn btnclose"
                                            ToolTip="Exit" CausesValidation="False" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />

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
    <asp:UpdatePanel ID="upGVWDeptMst" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                    <ContentTemplate>
                                             <table style="width: 100%;" cellpadding="5px">
                <tbody>
                    <tr>
                        <td>
                            <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img1" alt="Department Data" src="images/panelcollapse.png"
                                        onclick="Display(this,'divDepartmentData');" runat="server" style="margin-right: 2px;" />Department Data</legend>
                                <div id="divDepartmentData">
                                    <table style="margin: auto; width: 80%;">
                                        <tr>
                                            <td>
                                            <asp:GridView ID="gvwDeptMst" runat="server" Style="display: none; width: auto; margin: auto;" OnRowCommand="gvwDeptMst_RowCommand"
                                                AutoGenerateColumns="False" OnRowDataBound="gvwDeptMst_RowDataBound" OnPageIndexChanging="gvwDeptMst_PageIndexChanging">
                                                <Columns>
                                                    <asp:BoundField HeaderText="Sr. No">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" Font-Underline="False" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vDeptCode" HeaderText="Dept Code" />
                                                    <asp:BoundField DataField="vDeptName" HeaderText="Department">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vRemark" HeaderText="Remarks" />
                                                    <asp:TemplateField SortExpression="status" HeaderText="Edit">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="lnkEdit" runat="server" ImageUrl="~/images/Edit2.gif" ToolTip="Edit" />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" Font-Underline="False" VerticalAlign="Middle" />
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
                                                                <img src="images/Export.gif" onclick="ExportToExcel(this.id);" id='<%# Eval("vDeptCode")%>' />
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
                    <asp:UpdatePanel ID="UpDeptMstAuditTrail" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div id="dvDeptMstAuditTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
                                <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                    <tr>
                                        <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 97%;">
                                            <asp:Label ID="lblRamge" runat="server" Text="Audit Trail Information"></asp:Label>
                                        </td>
                                        <td style="width: 3%">
                                            <img id="imgDeptMstAuditTrail" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
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
                                                        <table id="tblDeptMstAudit" class="tblAudit" width="100%" style="background-color: aliceblue;"></table>
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

    <cc1:ModalPopupExtender ID="MPE_DeptMstHistory" runat="server" PopupControlID="dvDeptMstAuditTrail" BehaviorID="MPE_DeptMstHistory"
        PopupDragHandleControlID="LbldRandomizationDtl" BackgroundCssClass="modalBackground" CancelControlID="imgDeptMstAuditTrail"
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
        <div id="DivExportToExcel" runat="server">
            <asp:GridView runat="server" ID="gvExportToExcel" AutoGenerateColumns="true" Style="display: none"
                HeaderStyle-BackColor="#1560a1" HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" Header-style-font-size="10px !importnat" HeaderStyle-Font-Names="Verdana, Arial, Helvetica, sans-serif"
                HeaderStyle-Font-Size=" 0.9em"
                HeaderStyle-HorizontalAlign="Center" CellPadding="3" CellSpacing="0"
                RowStyle-Font-Names=" Verdana, Arial, Helvetica, sans-serif" RowStyle-Font-Size="10pt"
                RowStyle-BackColor="#84c8e6" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#191970" RowStyle-ForeColor="#191970">
            </asp:GridView>
        </div>
    </div>

    <asp:HiddenField runat="server" ID="hdnDeptCode" />
    <asp:Button ID="btnExportToExcel" runat="server" Style="display: none;" />
    <script type="text/javascript" language="javascript">

        function HideDeptDetails() {
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
            var vDeptCode = $("#" + e.id).attr("vDeptCode");

            if (vDeptCode != "") {
                $.ajax({
                    type: "post",
                    url: "frmDeptMst.aspx/AuditTrail",
                    data: '{"vDeptCode":"' + vDeptCode + '"}',
                    contentType: "application/json; charset=utf-8",
                    datatype: JSON,
                    async: false,
                    success: function (data) {
                        $('#tblDeptMstAudit').attr("IsTable", "has");
                        var aaDataSet = [];
                        var range = null;

                        if (data.d != "" && data.d != null) {
                            data = JSON.parse(data.d);
                            for (var Row = 0; Row < data.length; Row++) {
                                var InDataSet = [];
                                InDataSet.push(data[Row].SrNo, data[Row].DeptName, data[Row].Remark, data[Row].ModifyBy, data[Row].ModifyOn);
                                aaDataSet.push(InDataSet);
                            }
                        }
                        if ($("#tblDeptMstAudit").children().length > 0) {
                            $("#tblDeptMstAudit").dataTable().fnDestroy();
                        }
                        oTable = $('#tblDeptMstAudit').prepend($('<thead>').append($('#tblDeptMstAudit tr:first'))).dataTable({

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
                                   { "sTitle": "Department" },
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
                        $find('MPE_DeptMstHistory').show();
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
        function ExportToExcel(id) {
            $("#<%= hdnDeptCode.ClientID()%>").val(id);
            var btn = document.getElementById('<%= btnExportToExcel.ClientId()%>')
            btn.click();
        }
        function Validation() {
            if (document.getElementById('<%=txtDeptName.ClientID%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=txtDeptName.ClientID%>').value = '';
                msgalert('Please Enter Department Name !');
                document.getElementById('<%=txtDeptName.ClientID%>').focus();
                return false;
            }
            if (document.getElementById("<%=btnSave.ClientID%>").value.trim() == "Update") {

                if (document.getElementById("<%=txtRemarks.ClientID%>").value.trim() == "") {
                    msgalert("Please Enter Remarks !");
                    return false;
                }
            }
            return true;
        }

        function UIgvwDeptMst() {
            $('#<%= gvwDeptMst.ClientID%>').removeAttr('style', 'display:block');
            oTab = $('#<%= gvwDeptMst.ClientID%>').prepend($('<thead>').append($('#<%= gvwDeptMst.ClientID%> tr:first'))).dataTable({
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
        function ShowAlert(msg) {
           // alert(msg);
            // window.location.href = "frmDeptMst.aspx?mode=1";
            alertdooperation(msg, 1, "frmDeptMst.aspx?mode=1");
        }
    </script>

</asp:Content>
