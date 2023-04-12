<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmTemplateTypeMst.aspx.vb" Inherits="frmTemplateTypeMst" %>

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

    <table style="width: 100%; margin-bottom: 2%;" cellpadding="5px">
        <tr>
            <td>
                <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                    <legend class="LegendText" style="color: Black; font-size: 12px">
                        <img id="img2" alt="Template Type Details" src="images/panelcollapse.png"
                            onclick="Display(this,'divTempTypeDetail');" runat="server" style="margin-right: 2px;" />Template Type Details</legend>
                    <div id="divTempTypeDetail">
                        <table width="98%">
                            <tr>
                                <td class="Label" style="white-space: nowrap; width: 26%; text-align: right;"
                                    nowrap="noWrap">Template Type Name* :
                                </td>
                                <td style="text-align: left; width: 28%;">
                                    <asp:TextBox ID="txtTemplateTypeName" runat="server" CssClass="textBox" Width="85%"
                                        MaxLength="100" />
                                </td>
                                <td class="Label" nowrap="noWrap" style="text-align: right; width: 7%;">Remarks* :
                                </td>
                                <td style="text-align: left; width: 37%;">
                                    <asp:TextBox ID="txtRemark" runat="server" Rows="3" Columns="20" CssClass="textBox" Style="width: 62%; height: auto;" TextMode="MultiLine" MaxLength="500" />
                                </td>
                            </tr>
                            <tr>
                                <td nowrap="nowrap" colspan="4" style="text-align: center;">
                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btnsave" Text="Save" ToolTip="Save"
                                        OnClientClick=" return  Validation();" />
                                    <asp:Button ID="btnExportToExcelGrid" runat="Server" Font-Size="Smaller" CssClass="btn btnexcel"  ToolTip="Export To Excel" />
                                    <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="btn btncancel"
                                        OnClick="btnCancel_Click" Text="Cancel" ToolTip="Cancel" />
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
             <table style="width: 100%;" cellpadding="5px">
                <tbody>
                    <tr>
                        <td>
                            <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img1" alt="Template Type Data" src="images/panelcollapse.png"
                                        onclick="Display(this,'divTempTypeData');" runat="server" style="margin-right: 2px;" />Template Type Data</legend>
                                <div id="divTempTypeData">
                                    <table style="margin: auto; width: 80%;">
                                        <tr>
                                            <td>
            <asp:GridView ID="gvtemplatetypemst" runat="server" Style="display: none; width: 60%; margin: auto;" AutoGenerateColumns="False"
                OnPageIndexChanging="gvtemplatetypemst_PageIndexChanging" OnRowCommand="gvtemplatetypemst_RowCommand" OnRowDataBound="gvtemplatetypemst_RowDataBound">
                <Columns>
                    <asp:BoundField HeaderText="Sr. No">
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField DataField="vTemplateTypeCode" HeaderText="TemplateType Code" />
                    <asp:BoundField DataField="vTemplateTypeName" HeaderText="Template Type Name">
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField DataField="vRemarks" HeaderText="Remarks">
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd-MMM-yyyy}" DataField="dModifyOn"
                        HeaderText="Modify On">
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Edit">
                        <ItemTemplate>
                            <asp:ImageButton ID="lnkEdit" runat="server" ImageUrl="~/images/Edit2.gif" ToolTip="Edit" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
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
                                <image src="images/Export.gif" onclick="ExportToExcel(this.id);" id='<%# Eval("vTemplateTypeCode")%>' />
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
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <div>
        <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpTemplateTypeMstAuditTrail" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div id="dvTemplateTypeMstAuditTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
                                <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                    <tr>
                                        <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 97%;">
                                            <asp:Label ID="lblRamge" runat="server" Text="Audit Trail Information"></asp:Label>
                                        </td>
                                        <td style="width: 3%">
                                            <img id="imgTemplateTypeMstAuditTrail" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
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
                                                        <table id="tblTemplateTypeMstAudit" class="tblAudit" width="100%" style="background-color: aliceblue;"></table>
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

    <cc1:ModalPopupExtender ID="MPE_TemplateTypeMstHistory" runat="server" PopupControlID="dvTemplateTypeMstAuditTrail" BehaviorID="MPE_TemplateTypeMstHistory"
        PopupDragHandleControlID="LbldRandomizationDtl" BackgroundCssClass="modalBackground" CancelControlID="imgTemplateTypeMstAuditTrail"
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

    <asp:HiddenField runat="server" ID="hdnTemplateTypeCode" />
    <asp:Button ID="btnExportToExcel" runat="server" Style="display: none;" />
    <script type="text/javascript" language="javascript">

        function HideTempTypeDetails() {
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

        function pageLoad() {
            UIgvtemplatetypemst();
        }
        function AudtiTrail(e) {
            var vTemplateTypeCode = $("#" + e.id).attr("vTemplateTypeCode");
            if (vTemplateTypeCode != "") {
                $.ajax({
                    type: "post",
                    url: "frmTemplateTypeMst.aspx/AuditTrail",
                    data: '{"vTemplateTypeCode":"' + vTemplateTypeCode + '"}',
                    contentType: "application/json; charset=utf-8",
                    datatype: JSON,
                    async: false,
                    success: function (data) {
                        $('#tblTemplateTypeMstAudit').attr("IsTable", "has");
                        var aaDataSet = [];
                        var range = null;

                        if (data.d != "" && data.d != null) {
                            data = JSON.parse(data.d);
                            for (var Row = 0; Row < data.length; Row++) {
                                var InDataSet = [];
                                InDataSet.push(data[Row].SrNo, data[Row].TemplateTypeName, data[Row].Remarks, data[Row].ModifyBy, data[Row].ModifyOn);
                                aaDataSet.push(InDataSet);
                            }
                        }
                        if ($("#tblTemplateTypeMstAudit").children().length > 0) {
                            $("#tblTemplateTypeMstAudit").dataTable().fnDestroy();
                            $("#tblTemplateTypeMstAudit").empty();
                        }
                        oTable = $('#tblTemplateTypeMstAudit').prepend($('<thead>').append($('#tblTemplateTypeMstAudit tr:first'))).dataTable({

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
                                    "sTitle": "Sr. No"
                                },
                                   { "sTitle": "Template Type Name" },
                                   { "sTitle": "Remarks" },
                                   { "sTitle": "Modify By" },
                                   { "sTitle": "Modify On" }

                            ],
                            "aoColumnDefs": [
                                        { 'bSortable': false, 'aTargets': [0] }
                            ],
                            "oLanguage": {
                                "sEmptyTable": "No Record Found"
                            }

                        });
                        oTable.fnAdjustColumnSizing();
                        $('.DataTables_sort_wrapper').click;
                        $find('MPE_TemplateTypeMstHistory').show();
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
            $("#<%= hdnTemplateTypeCode.ClientID()%>").val(id);
            var btn = document.getElementById('<%= btnExportToExcel.ClientId()%>')
            btn.click();
        }
        function UIgvtemplatetypemst() {
            $('#<%= gvtemplatetypemst.ClientID%>').removeAttr('style', 'display:block');
            oTab = $('#<%= gvtemplatetypemst.ClientID%>').prepend($('<thead>').append($('#<%= gvtemplatetypemst.ClientID%> tr:first'))).dataTable({
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
            if (document.getElementById('<%=txtTemplateTypeName.ClientID%>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Template Type Name !');
                document.getElementById('<%=txtTemplateTypeName.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtRemark.ClientID%>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Remarks !');
                document.getElementById('<%=txtRemark.ClientID%>').focus();
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
    function ShowAlert(msg) {
      //  alert(msg);
        // window.location.href = "frmTemplateTypeMst.aspx?mode=1";
        alertdooperation(msg, 1, "frmTemplateTypeMst.aspx?mode=1");
    }
    </script>

</asp:Content>
