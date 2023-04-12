<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmResourceMst, App_Web_22suyskz" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="conResourceMst" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

        /*#ctl00_CPHLAMBDA_gvwResourceMst_wrapper {
            margin: 0px 235px;
        }*/
    </style>

    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td>
                <asp:UpdatePanel ID="upResourceMst" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table cellpadding="5" style="width: 100%;">
                            <tbody>
                                <tr>
                                    <td>
                                        <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                                            <legend class="LegendText" style="color: Black; font-size: 12px">
                                                <img id="img2" alt="Resource Details" src="images/panelcollapse.png"
                                                    onclick="Display(this,'divResourceDetail');" runat="server" style="margin-right: 2px;" />Resource Details</legend>
                                            <div id="divResourceDetail">
                                                <table width="98%">
                                                    <tr>
                                                        <td class="Label" style="width: 22%; text-align: right;">Select Location* :
                                                        </td>
                                                        <td style="text-align: left; width: 25%;">
                                                            <asp:DropDownList ID="ddlLocation" runat="server" CssClass="dropDownList" Width="76%" />
                                                        </td>
                                                        <td class="Label" style="text-align: right; width: 12%;">Select Department* :
                                                        </td>
                                                        <td style="text-align: left; width: 32%;">
                                                            <asp:DropDownList ID="ddlDept" runat="server" CssClass="dropDownList" Width="60%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Label" style="text-align: right; width: 22%;">Resource Name* :
                                                        </td>
                                                        <td style="text-align: left; width: 25%;">
                                                            <asp:TextBox ID="txtResourceName" runat="server" CssClass="textBox" MaxLength="255" Width="75%" />
                                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" __designer:wfdid="w17"
                                            ControlToValidate="txtResourceName" Display="None" ErrorMessage="Resource Name is required."
                                            SetFocusOnError="True">Resource Name is required.</asp:RequiredFieldValidator>--%>
                                                            <%--<cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" __designer:wfdid="w18"
                                            TargetControlID="RequiredFieldValidator1">
                                        </cc1:ValidatorCalloutExtender>--%>
                                                            <td class="Label" style="text-align: right; width: 12%;">Units Of Measurement :
                                                            </td>
                                                            <td style="text-align: left; width: 32%;">
                                                                <asp:TextBox ID="TxtUOM" runat="server" CssClass="textBox" MaxLength="10" Width="59%" />
                                                            </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Label" style="text-align: right; width: 22%;">Resource Capacity :
                                                        </td>
                                                        <td style="text-align: left; width: 25%;">
                                                            <asp:TextBox ID="TXTCapacity" runat="server" CssClass="textBox" MaxLength="9" Width="75%" />
                                                            <cc1:FilteredTextBoxExtender ID="txtValid" runat="server" ValidChars="0123456789"
                                                                TargetControlID="TXTCapacity" />
                                                        </td>
                                                        <td class="Label" style="width: 12%; text-align: right">Remarks* :
                                                        </td>
                                                        <td style="text-align: left; width: 32%;">
                                                            <asp:TextBox ID="txtRemark" runat="server" Rows="3" Columns="20" CssClass="textBox" Style="width: 59%; height: auto;" TextMode="MultiLine" MaxLength="500" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" colspan="4">
                                                            <asp:Button ID="btnSave" runat="server" Text=" Save " ToolTip="Save" CssClass="btn btnsave"
                                                                OnClientClick="return  Validation() " />
                                                            <asp:Button ID="btnExportToExcelGrid" runat="Server" Font-Size="Smaller" CssClass="btn btnexcel"  ToolTip="Export To Excel" />
                                                            <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Text="Cancel"
                                                                ToolTip="Cancel" CssClass="btn btncancel" CausesValidation="False" />
                                                            <asp:Button ID="btnClose" OnClick="btnClose_Click" runat="server" Text="Exit" CssClass="btn btnclose"
                                                                ToolTip="Exit" CausesValidation="False" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);
" />
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
                        <asp:AsyncPostBackTrigger ControlID="gvwResourceMst" EventName="RowCommand" />
                        <asp:PostBackTrigger ControlID="btnExportToExcelGrid" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <table cellpadding="5" style="width: 100%; margin-top: 1%;">
        <tr>
            <td>
                <asp:UpdatePanel ID="upGVWResourceMst" runat="server" RenderMode="Inline">
                    <ContentTemplate>
                        <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                            <legend class="LegendText" style="color: Black; font-size: 12px">
                                <img id="img1" alt="Resource Data" src="images/panelcollapse.png"
                                    onclick="Display(this,'divResourceData');" runat="server" style="margin-right: 2px;" />Resource Data</legend>
                            <div id="divResourceData">
                                <table style="margin: auto; width: 80%;">
                                    <tr>
                                        <td>
                                            <asp:GridView ID="gvwResourceMst" runat="server" Style="display: none; width: 60%; margin: auto;" OnPageIndexChanging="gvwResourceMst_PageIndexChanging"
                                                AutoGenerateColumns="False" OnRowCreated="gvwResourceMst_RowCreated"
                                                OnRowDataBound="gvwResourceMst_RowDataBound" OnRowCommand="gvwResourceMst_RowCommand">
                                                <Columns>
                                                    <asp:BoundField HeaderText="Sr. No">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" Font-Underline="False" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vLocationName" HeaderText="Location" />
                                                    <asp:BoundField DataField="vDeptName" HeaderText="Department" />
                                                    <asp:BoundField DataField="vResourceName" HeaderText="Resource" />
                                                    <asp:BoundField DataField="vUOM" HeaderText="Units of Measurement" />
                                                    <asp:BoundField DataField="nResourceCapacity" HeaderText="Capacity" />
                                                    <asp:TemplateField SortExpression="status" HeaderText="Edit">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="lnkEdit" runat="server" ImageUrl="~/images/Edit2.gif" ToolTip="Edit" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" Font-Underline="False" VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="vResourceCode" HeaderText="ResourceCode" />
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
                                                                <image src="images/Export.gif" onclick="ExportToExcel(this.id);" id='<%# Eval("vResourceCode")%>' />
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
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <div>
        <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpResourceMstAuditTrail" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div id="dvResourceMstAuditTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
                                <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                    <tr>
                                        <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 97%;">
                                            <asp:Label ID="lblRamge" runat="server" Text="Audit Trail Information"></asp:Label>
                                        </td>
                                        <td style="width: 3%">
                                            <img id="imgResourceMstAuditTrail" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
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
                                                        <table id="tblResourceMstAudit" class="tblAudit" width="100%" style="background-color: aliceblue;"></table>
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

    <cc1:ModalPopupExtender ID="MPE_ResourceMstHistory" runat="server" PopupControlID="dvResourceMstAuditTrail" BehaviorID="MPE_ResourceMstHistory"
        PopupDragHandleControlID="LbldRandomizationDtl" BackgroundCssClass="modalBackground" CancelControlID="imgResourceMstAuditTrail"
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
    <div id="Div1" runat="server">
        <asp:GridView runat="server" ID="gvExportToExcel" AutoGenerateColumns="true" Style="display: none"
            HeaderStyle-BackColor="#1560a1" HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" Header-style-font-size="10px !importnat" HeaderStyle-Font-Names="Verdana, Arial, Helvetica, sans-serif"
            HeaderStyle-Font-Size=" 0.9em"
            HeaderStyle-HorizontalAlign="Center" CellPadding="3" CellSpacing="0"
            RowStyle-Font-Names=" Verdana, Arial, Helvetica, sans-serif" RowStyle-Font-Size="10pt"
            RowStyle-BackColor="#84c8e6" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#191970" RowStyle-ForeColor="#191970">
        </asp:GridView>
    </div>
    <asp:HiddenField runat="server" ID="hdnResourceCode" />
    <asp:Button ID="btnExportToExcel" runat="server" Style="display: none;" />
    <script type="text/javascript" language="javascript">

        function HideResourceDetails() {
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
            var vResourceCode = $("#" + e.id).attr("vResourceCode");

            if (vResourceCode != "") {
                $.ajax({
                    type: "post",
                    url: "frmResourceMst.aspx/AuditTrail",
                    data: '{"vResourceCode":"' + vResourceCode + '"}',
                    contentType: "application/json; charset=utf-8",
                    datatype: JSON,
                    async: false,
                    success: function (data) {
                        $('#tblResourceMstAudit').attr("IsTable", "has");
                        var aaDataSet = [];
                        var range = null;

                        if (data.d != "" && data.d != null) {
                            data = JSON.parse(data.d);
                            for (var Row = 0; Row < data.length; Row++) {
                                var InDataSet = [];
                                InDataSet.push(data[Row].SrNo, data[Row].Location, data[Row].DeptName, data[Row].ResourceName, data[Row].UOM, data[Row].ResourceCapacity, data[Row].Remark, data[Row].ModifyBy, data[Row].ModifyOn);
                                aaDataSet.push(InDataSet);
                            }
                        }
                        if ($("#tblResourceMstAudit").children().length > 0) {
                            $("#tblResourceMstAudit").dataTable().fnDestroy();
                        }
                        oTable = $('#tblResourceMstAudit').prepend($('<thead>').append($('#tblResourceMstAudit tr:first'))).dataTable({

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
                                   { "sTitle": "Location" },
                                   { "sTitle": "Department" },
                                   { "sTitle": "Resource" },
                                   { "sTitle": "Unit of Measurement" },
                                   { "sTitle": "Capacity" },
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
                        $find('MPE_ResourceMstHistory').show();
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
        function UIgvwResourceMst() {
            $('#<%= gvwResourceMst.ClientID%>').removeAttr('style', 'display:block');
            oTab = $('#<%= gvwResourceMst.ClientID%>').prepend($('<thead>').append($('#<%= gvwResourceMst.ClientID%> tr:first'))).dataTable({
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
            $("#<%= hdnResourceCode.ClientID()%>").val(id);
            var btn = document.getElementById('<%= btnExportToExcel.ClientId()%>')
            btn.click();
        }
        function Validation() {

            if (document.getElementById('<%=ddlLocation.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Location !');
                return false;
            }
            if (document.getElementById('<%=ddlDept.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Department !');
                return false;
            }

            if (document.getElementById('<%=txtResourceName.ClientID%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=txtResourceName.ClientID%>').value = ''
                msgalert('Please Enter Resource Name !');
                document.getElementById('<%=txtResourceName.ClientID%>').focus();
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
           // alert(msg);
            //window.location.href = "frmResourceMst.aspx?mode=1";
            alertdooperation(msg, 1, "frmResourceMst.aspx?mode=1");
        }
    </script>

</asp:Content>
