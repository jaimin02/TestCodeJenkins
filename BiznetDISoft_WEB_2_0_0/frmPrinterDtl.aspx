<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmPrinterDtl.aspx.vb" Inherits="frmPrinterDtl" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

        .ui-dialog .ui-dialog-content {
            width: auto;
            min-height: 50px;
            max-height: 155px;
            overflow: scroll;
            text-align: left;
        }
    </style>
    
    <asp:UpdatePanel runat="server" ID="UpPnlPrinter">
        <ContentTemplate>
            <table cellpadding="5px" style="width: 100%;">
                <tr>
                    <td>
                        <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                            <legend class="LegendText" style="color: Black; font-size: 12px">
                                <img id="img2" alt="Printer Details" src="images/panelcollapse.png"
                                    onclick="Display(this,'divPrinterDetail');" runat="server" style="margin-right: 2px;" />Printer Details</legend>
                            <div id="divPrinterDetail">
                                <table width="98%">
                                    <tr>
                                        <td class="Label" nowrap="noWrap" style="width: 15%; text-align: right;">Location*:
                                        </td>
                                        <td style="text-align: left; width: 20%;">
                                            <asp:DropDownList ID="DdlListLocation" runat="server" CssClass="dropDownList " Width="99%" />
                                        </td>
                                        <td class="Label" nowrap="noWrap" style="width: 8%; text-align: right;">Printer Name*:
                                        </td>
                                        <td style="text-align: left; width: 20%;">
                                            <asp:TextBox ID="TxtPrinter" runat="server" Width="98%" CssClass="textBox" />
                                        </td>
                                        <td class="Label" nowrap="noWrap" style="width: 7%; text-align: right;">Remarks:
                                        </td>
                                        <td style="text-align: left; width: 27%;">
                                            <asp:TextBox ID="txtRemarks" runat="server" Width="64%" CssClass="textBox" TextMode="MultiLine" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Label" nowrap="nowrap" colspan="6" style="text-align: center; vertical-align: top;">
                                            <asp:Button ID="BtnSave" runat="server" Text="Save" ToolTip="Save" CssClass="btn btnsave"
                                                OnClientClick="return Validation();" />
                                            <asp:Button ID="btnExportToExcelGrid" runat="server" Text="" ToolTip="Export To Excel"
                                                CssClass="btn btnexcel"/>
                                            <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Text="Cancel"
                                                ToolTip="Cancel" CssClass="btn btncancel" />
                                            <asp:Button ID="BtnExit" runat="server" Text="Exit" ToolTip="Exit" CssClass="btn btnexit"
                                                OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <button id="btnRemarks" runat="server" style="display: none;" />
    <cc1:ModalPopupExtender ID="mdlRemarks" runat="server" PopupControlID="divRemarks"
        BackgroundCssClass="modalBackground" BehaviorID="mdlRemarks" CancelControlID="btnRemarksCancel"
        TargetControlID="btnRemarks">
    </cc1:ModalPopupExtender>
    <div id="divRemarks" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 32%; height: auto; max-height: 45%; min-height: auto;">
        <table cellpadding="2" cellspacing="2" width="100%">
            <tr>
                <td class="LabelText" style="text-align: left !important; font-size: 12px !important; width: 97%;"><b>Reason/Remark for Delete </b>
                </td>
            </tr>
            <tr>
                <td>
                    <hr />
                </td>
            </tr>
            <tr>
                <td class="LabelText" style="text-align: left !important;">Enter Remarks:
                </td>
            </tr>
            <tr>
                <td style="text-align: left !important;">
                    <asp:TextBox ID="txtRemarks_delete" runat="server" TextMode="MultiLine" Rows="5" Height="60px"
                        Width="300px" TabIndex="55" />
                </td>
            </tr>
            <tr>
                <td>
                    <hr />
                </td>
            </tr>
            <tr>
                <td style="text-align: center;">
                    <asp:Button ID="btnRemarksUpdate" runat="server" Text="Save" CssClass="btn btnsave"
                        Width="64px" Style="font-size: 12px !important;" TabIndex="56" OnClick="btnRemarksUpdate_Click" OnClientClick="return ValidateInactiveRemarks();" />
                    <asp:Button ID="btnRemarksCancel" runat="server" Text="Cancel" CssClass="btn btnexit"
                        Width="64px" Style="font-size: 12px !important;" TabIndex="56" />

                </td>
            </tr>
        </table>
    </div>


    <asp:UpdatePanel ID="Up_Grid" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table style="width: 100%;" cellpadding="5px">
                <tbody>
                    <tr>
                        <td>
                            <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img1" alt="Printer Data" src="images/panelcollapse.png"
                                        onclick="Display(this,'divPrinterData');" runat="server" style="margin-right: 2px;" />Printer Data</legend>
                                <div id="divPrinterData">
                                    <table style="margin: auto; width: 85%;">
                                        <tr>
                                            <td id="fldLegnedDelete" style="display: none">
                                                <fieldset class="FieldSetBox" style="width: 20%; float: right;">
                                                    <legend class="LegendText" style="color: Black">Legend</legend>
                                                    <table class="LabelText">
                                                        <tbody>
                                                            <tr>
                                                                <td>
                                                                    <div style="width: 20px; height: 20px; float: left; background-color: rgb(255, 189, 121); font-weight: bold;">
                                                                    </div>
                                                                </td>
                                                                <td>Inactive Printer
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:GridView ID="GV_Printer" runat="server" Style="width: 50%; margin: auto; display: none"
                                                    AutoGenerateColumns="False" OnRowCommand="GV_Printer_RowCommand" OnRowDataBound="GV_Printer_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="#">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="nPrinterId" HeaderText="Printer ID">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        
                                                        <asp:BoundField DataField="vPrinterName" HeaderText="Printer Name">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vLocationName" HeaderText="Location">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vRemarks" HeaderText="Remarks">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="cStatusIndi" HeaderText="cStatusIndi">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Edit" SortExpression="status">
                                                            <ItemTemplate>

                                                                <asp:ImageButton ID="lnkEdit" runat="server" ImageUrl="~/images/Edit2.gif" ToolTip="Edit" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Delete" SortExpression="status">
                                                            <ItemTemplate>

                                                                <asp:ImageButton ID="lnkDelete" runat="server" ImageUrl="~/Images/i_delete.gif"
                                                                    ToolTip="Inactive" OnClientClick="return ShowModalPopup(this);" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Audit Trail">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="lnkAudit" runat="server" ImageUrl="~/Images/audit.png" ToolTip="Audit Trial" OnClientClick="AuditTrail(this); return false;" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Export">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <img src="images/Export.gif" onclick="ExportToExcel(this.id);" id='<%#  Eval("nPrinterId") %>' />
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
            <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnExportToExcelGrid" />
        </Triggers>
    </asp:UpdatePanel>



    <div>
        <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpUserGroupAuditTrail" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div id="dvUserGroupMstAudiTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
                                <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                    <tr>
                                        <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 97%;">
                                            <asp:Label ID="lblRamge" runat="server" Text="Audit Trail Information"></asp:Label>
                                        </td>
                                        <td style="width: 3%">
                                            <img id="imgClientAuditTrail" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
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
                                                        <table id="tblPrinterAudit" class="tblAudit" width="100%" style="background-color: aliceblue; text-align: left"></table>
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
    <cc1:ModalPopupExtender ID="MPE_CleintMstHistory" runat="server" PopupControlID="dvUserGroupMstAudiTrail" BehaviorID="MPE_CleintMstHistory"
        PopupDragHandleControlID="LbldRandomizationDtl" BackgroundCssClass="modalBackground" CancelControlID="imgClientAuditTrail"
        TargetControlID="btn3">
    </cc1:ModalPopupExtender>
    <div id="DivExports" runat="server">
        <asp:GridView runat="server" ID="gvExport" AutoGenerateColumns="true" Style="display: none"
            HeaderStyle-BackColor="#1560a1" HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" HeaderStyle-Font-Names="Verdana, Arial, Helvetica, sans-serif"
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
    <asp:HiddenField runat="server" ID="hdnPrinterId" />
    <asp:Button ID="btnExportToExcel" runat="server" Style="display: none;" />


    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>

    <script type="text/javascript" language="javascript">
        function pageLoad() {
            UIgvPrinterDtl();
        }
        function HidePrinterDetail() {
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

        function UIgvPrinterDtl() {
            $('#<%= GV_Printer.ClientID%>').removeAttr('style', 'display:block');
            $('#fldLegnedDelete').removeAttr('style', 'display:block');
            oTab = $('#<%= GV_Printer.ClientID%>').prepend($('<thead>').append($('#<%= GV_Printer.ClientID%> tr:first'))).dataTable({
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

        function ShowModalPopup(e) {

            msgConfirmDeleteAlert(null, "Are You Sure You Want To Deactive Printer ?", function (isConfirmed) {
                if (isConfirmed) {
                    document.getElementById("<%=txtRemarks_delete.ClientID%>").value = "";
                    $find('mdlRemarks').show();
                    __doPostBack(e.name, '');
                    return true;
                };
            });
            return false;
        }
        function ValidateInactiveRemarks() {
            if (document.getElementById("<%=txtRemarks_delete.ClientID%>").value.trim() == "") {
                msgalert("Please Enter Remarks!");
                return false;
            }
            return true;
        }
        function Validation() {
            if (document.getElementById('<%=DdlListLocation.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Location!');
                return false;
            }
            else if (document.getElementById('<%=TxtPrinter.ClientID%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=TxtPrinter.ClientID%>').value == '';
                msgalert('Please Enter Printer Name!');
                document.getElementById('<%=TxtPrinter.ClientID%>').focus();
                return false;
            }
        if (document.getElementById("<%=btnSave.ClientID%>").value.trim() == "Update") {
                if (document.getElementById("<%=txtRemarks.ClientID%>").value.trim() == "") {
                msgalert("Please Enter Remarks!");
                return false;
            }
        }
    }
    function ShowAlert(msg) {
        msgalert(msg);
        window.location.href = "frmPrinterDtl.aspx?mode=1";
    }

    function AuditTrail(e) {
        debugger;
        var nPrinterId = e.attributes.nPrinterId.value

        if (nPrinterId != "") {
            $.ajax({
                type: "post",
                url: "frmPrinterDtl.aspx/AuditTrail",
                data: '{"nPrinterId":"' + nPrinterId + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                success: function (data) {
                    $('#tblPrinterAudit').attr("IsTable", "has");
                    var aaDataSet = [];
                    var range = null;

                    data = JSON.parse(data.d);
                    for (var Row = 0; Row < data.length; Row++) {
                        var InDataSet = [];
                        InDataSet.push(data[Row].SrNo, data[Row].Printer, data[Row].Location, data[Row].Remarks, data[Row].ModifyBy, data[Row].ModifyOn);
                        aaDataSet.push(InDataSet);
                    }
                    if ($("#tblPrinterAudit").children().length > 0) {
                        $("#tblPrinterAudit").dataTable().fnDestroy();
                    }
                    oTable = $('#tblPrinterAudit').prepend($('<thead>').append($('#tblPrinterAudit tr:first'))).dataTable({

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
                                "sTitle": "#",
                            },

                             { "sTitle": "Printer Name" },
                             { "sTitle": "Location Name" },
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
                    $find('MPE_CleintMstHistory').show();

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
        $("#<%= hdnPrinterId.ClientID%>").val(id);
        var btn = document.getElementById('<%= btnExportToExcel.ClientId()%>')
        btn.click();
    }


    </script>

</asp:Content>
