<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmDeptStageMatrix.aspx.vb" Inherits="frmDeptStageMAtrix" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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

        /*#ctl00_CPHLAMBDA_gvdeptstage_wrapper {
            margin: 0px 235px;
        }*/
    </style>
    <table cellpadding="5px" style="width: 100%;">
        <tr>
            <td>
                <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                    <legend class="LegendText" style="color: Black; font-size: 12px">
                        <img id="img2" alt="Department Stage Details" src="images/panelcollapse.png"
                            onclick="Display(this,'divDepartmentStageDetail');" runat="server" style="margin-right: 2px;" />Department Stage Details</legend>
                    <div id="divDepartmentStageDetail">
                        <table width="98%">
                            <tr>
                                <td class="Label" valign="top" style="white-space: nowrap; text-align: right; width: 25%;">Deparment* :<br />
                                    <br />
                                    <br />
                                    <br />
                                    <br />
                                    <br />
                                    Remarks :
                                </td>
                                <td style="text-align: left; vertical-align: top; width: 25%;">
                                    <asp:DropDownList ID="ddldept" runat="server" CssClass="dropDownList" Width="60%" /><br />
                                    <br />
                                    <br />
                                    <br />
                                    <asp:TextBox ID="txtRemark" runat="server" Rows="3" Columns="20" CssClass="textBox" Style="width: 59%; height: auto;" TextMode="MultiLine" MaxLength="500" />
                                </td>
                                <td class="Label" valign="top" rowspan="1" style="text-align: right; width: 5%;">Stages* :
                                </td>
                                <td rowspan="1" style="text-align: left; vertical-align: top; width: 30%; height:30%;">
                                    <asp:DropDownList ID="ddlStages" runat="server" CssClass="dropDownList" Width="40%" />
                                    <div style="border-right: gray thin solid; border-top: gray thin solid; overflow-y: scroll; border-left: gray thin solid; width: 40%; border-bottom: gray thin solid; height: 100%; text-align: left"
                                        id="dvChkListStageMatrix" runat="server">
                                        <asp:CheckBoxList ID="Cblstages" runat="server" Height="1px" SkinID="chkDisplay"
                                            RepeatColumns="1" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap="nowrap" colspan="4" style="vertical-align: top; text-align: center;">
                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btnsave" Text="Save" ToolTip="Save" />
                                    <asp:Button ID="btnExportToExcelGrid" runat="Server" Font-Size="Smaller" CssClass="btn btnexcel" ToolTip="Export To Excel" />
                                    <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="btn btncancel"
                                        OnClick="btnCancel_Click" Text="Cancel" ToolTip="Cancel" />
                                    <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="btn btnclose"
                                        ToolTip="Exit" Text="Exit" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
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
                                    <img id="img1" alt="Department Stage Data" src="images/panelcollapse.png"
                                        onclick="Display(this,'divDepartmentStageData');" runat="server" style="margin-right: 2px;" />Department Stage Data</legend>
                                <div id="divDepartmentStageData">
                                    <table style="margin: auto; width: 80%;">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvdeptstage" runat="server" Style="width: 60%; margin: auto;" AutoGenerateColumns="False" OnRowCreated="gvdeptstage_RowCreated"
                                                    OnPageIndexChanging="gvdeptstage_PageIndexChanging" OnRowCommand="gvdeptstage_RowCommand" OnRowDataBound="gvdeptstage_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Sr. No">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vDeptName" HeaderText="Department Name">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vStageDesc" HeaderText="Stage Desc">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd-MMM-yyyy}" DataField="dModifyOn"
                                                            HeaderText="Modify On">
                                                            <ItemStyle HorizontalAlign="center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField SortExpression="status" HeaderText="Edit">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="lnkEdit" runat="server" ImageUrl="~/images/Edit2.gif" ToolTip="Edit" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="vDeptStageCode" HeaderText="DeptStageCode" />
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
                                                                    <img src="images/Export.gif" onclick="ExportToExcel(this.id);" id='<%# Eval("vDeptStageCode")%>' />
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
            <asp:AsyncPostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>
    <div>
        <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpDeptStageMatrixAuditTrail" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div id="dvDeptStageMatrixAuditTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
                                <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                    <tr>
                                        <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 97%;">
                                            <asp:Label ID="lblRamge" runat="server" Text="Audit Trail Information"></asp:Label>
                                        </td>
                                        <td style="width: 3%">
                                            <img id="imgDeptStageMatrixAuditTrail" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
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
                                                        <table id="tblDeptStageMatrixAudit" class="tblAudit" width="100%" style="background-color: aliceblue;"></table>
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

    <cc1:ModalPopupExtender ID="MPE_DeptStageMatrixHistory" runat="server" PopupControlID="dvDeptStageMatrixAuditTrail" BehaviorID="MPE_DeptStageMatrixHistory"
        PopupDragHandleControlID="LbldRandomizationDtl" BackgroundCssClass="modalBackground" CancelControlID="imgDeptStageMatrixAuditTrail"
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
    <asp:HiddenField runat="server" ID="hdnDeptStageCode" />
    <asp:Button ID="btnExportToExcel" runat="server" Style="display: none;" />
    <script type="text/javascript" language="javascript">

        function HideDeptStageDetails() {
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
            var vDeptStageCode = $("#" + e.id).attr("vDeptStageCode");

            if (vDeptStageCode != "") {
                $.ajax({
                    type: "post",
                    url: "frmDeptStageMatrix.aspx/AuditTrail",
                    data: '{"vDeptStageCode":"' + vDeptStageCode + '"}',
                    contentType: "application/json; charset=utf-8",
                    datatype: JSON,
                    async: false,
                    success: function (data) {
                        $('#tblDeptStageMatrixAudit').attr("IsTable", "has");
                        var aaDataSet = [];
                        var range = null;

                        if (data.d != "" && data.d != null) {
                            data = JSON.parse(data.d);
                            for (var Row = 0; Row < data.length; Row++) {
                                var InDataSet = [];
                                InDataSet.push(data[Row].SrNo, data[Row].DeptName, data[Row].StageDesc, data[Row].Remark, data[Row].ModifyBy, data[Row].ModifyOn);
                                aaDataSet.push(InDataSet);
                            }
                        }
                        if ($("#tblDeptStageMatrixAudit").children().length > 0) {
                            $("#tblDeptStageMatrixAudit").dataTable().fnDestroy();
                        }
                        oTable = $('#tblDeptStageMatrixAudit').prepend($('<thead>').append($('#tblDeptStageMatrixAudit tr:first'))).dataTable({

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
                                   { "sTitle": "Department Name" },
                                   { "sTitle": "Stage Desc" },
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
                        $find('MPE_DeptStageMatrixHistory').show();
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
        function UIgvdeptstage() {
            $('#<%= gvdeptstage.ClientID%>').removeAttr('style', 'display:block');
            oTab = $('#<%= gvdeptstage.ClientID%>').prepend($('<thead>').append($('#<%= gvdeptstage.ClientID%> tr:first'))).dataTable({
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
            if (document.getElementById('<%=ddldept.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Department!');
                return false;
            }
            var chklst = document.getElementById('<%=Cblstages.clientid%>');
            var chks;
            var result = false;
            var i;

            if (chklst != null && typeof (chklst) != 'undefined') {
                chks = chklst.getElementsByTagName('input');
                for (i = 0; i < chks.length; i++) {
                    if (chks[i].type.toUpperCase() == 'CHECKBOX' && chks[i].checked) {
                        result = true;
                        break;
                    }
                }
            }

            if (!result) {
                msgalert('Please Select Atleast One Stage!');
                return false;
            }
            if (document.getElementById("<%=btnSave.ClientID%>").value.trim() == "Update") {

                if (document.getElementById("<%=txtRemark.ClientID%>").value.trim() == "") {
                    msgalert("Please Enter Remarks!");
                    return false;
                }
            }
            return true;
        }
        function ExportToExcel(id) {
            $("#<%= hdnDeptStageCode.ClientID()%>").val(id);
            var btn = document.getElementById('<%= btnExportToExcel.ClientId()%>')
            btn.click();
        }
        function ValidationForEdit() {
            if (document.getElementById('<%=ddldept.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Department!');
                return false;
            }

            if (document.getElementById("<%=btnSave.ClientID%>").value.trim() == "Update") {

                if (document.getElementById("<%=txtRemark.ClientID%>").value.trim() == "") {
                    msgalert("Please Enter Remarks!");
                    return false;
                }
            }
            return true;
        }
        // for alert message added on 26-oct-2012
        function ShowAlert(msg) {
            //alert(msg);
            //window.location.href = "frmDeptStageMatrix.aspx?mode=1";
            alertdooperation(msg, 1, "frmDeptStageMatrix.aspx?mode=1");
        }


    </script>

</asp:Content>
