<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmMedExSubGroupMst, App_Web_mlepfeoz" validaterequest="false" enableeventvalidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

        /*#ctl00_CPHLAMBDA_gvwMedExSubGroupMst_wrapper {
            margin: 0px 235px;
        }*/
    </style>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="width: 100%;" cellpadding="5px">
                <tbody>
                    <tr>
                        <td>
                            <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img2" alt="Attribute Sub Group Details" src="images/panelcollapse.png"
                                        onclick="Display(this,'divAttributeSubGroupDetail');" runat="server" style="margin-right: 2px;" />Attribute Sub Group Details</legend>
                                <div id="divAttributeSubGroupDetail">
                                    <table width="98%">
                                        <tr>
                                            <td style="white-space: nowrap; text-align: right; width: 29%;" class="Label">Attribute Sub Group* :
                                                <td style="text-align: left; width: 25%;">
                                                    <asp:TextBox ID="txtMedExSubGropuDesc" TabIndex="1" runat="server" CssClass="textBox"
                                                        Width="100%" MaxLength="250" />
                                                </td>
                                                <td style="white-space: nowrap; text-align: right; width: 16%;" class="Label">Variable Name :
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                    <asp:TextBox ID="txtCDISCValue" TabIndex="2" runat="server" CssClass="textBox" Width="100%"
                                                        MaxLength="10" />
                                                </td>
                                        </tr>
                                        <tr>
                                            <td style="white-space: nowrap; text-align: right; width: 26%;" class="Label">Other Value :
                                            </td>
                                            <td style="text-align: left; width: 25%;">
                                                <asp:TextBox ID="txtOtherValues" TabIndex="2" runat="server" CssClass="textBox" Width="100%"
                                                    MaxLength="100" />
                                            </td>
                                            <td style="text-align: right; width: 16%;" class="Label">Display Name* :
                                            </td>
                                            <td style="text-align: left; width: 25%;">
                                                <asp:TextBox ID="txtDisplayName" runat="server"  CssClass="textBox" Style="width: 100%; height: auto;"  MaxLength="500" />
                                            </td>
                                            
                                        </tr>
                                        <tr>

                                            <td style="text-align: right; width: 16%;" class="Label">Remark* :
                                            </td>
                                            <td nowrap="nowrap" style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="txtRemark" runat="server" Rows="3" Columns="20" CssClass="textBox" Style="width: 100%; height: auto;" TextMode="MultiLine" MaxLength="500" />
                                            </td>

                                            <td style="white-space: nowrap; text-align: right; display: none;" class="Label">ActiveFlag
                                            </td>
                                            <td style="text-align: left; display: none;">
                                                <asp:CheckBox ID="chkactive" TabIndex="3" runat="server" CssClass="checkboxlist "
                                                    Checked="True" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center;" class="Label" colspan="4">
                                                <asp:Button ID="BtnSave" TabIndex="3" OnClick="BtnSave_Click" runat="server" Text="Save"
                                                    ToolTip="Save" CssClass="btn btnsave" OnClientClick="return Validation();" />
                                                <asp:Button ID="btnExportToExcelGrid" runat="Server" Font-Size="Smaller" CssClass="btn btnexcel" ToolTip="Export To Excel" />
                                                <asp:Button ID="BtnExit" TabIndex="4" runat="server" Text="Exit" ToolTip=" Exit"
                                                    CssClass="btn btnexit" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);
" />
                                                <asp:Button ID="btncancel" TabIndex="6" OnClick="btncancel_Click" runat="server"
                                                    Text="Cancel" ToolTip=" Cancel" CssClass="btn btncancel" Visible="False" />

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
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table style="width: 100%; margin-bottom: 1%; display: none;">
                <tr>
                    <td style="width: 30%; white-space: nowrap; text-align: right;" class="Label">Search Attribute Sub-Group :
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtSubGroup" runat="server" CssClass="textBox" Width="55%" />
                        <asp:Button ID="btnSetSubGroup" runat="server" Style="display: none" />
                        <asp:HiddenField ID="HSubGroupId" runat="server" />
                        <cc1:AutoCompleteExtender ID="AutoCompleteExtenderForSubGroup" runat="server" UseContextKey="True"
                            TargetControlID="txtSubGroup" ServicePath="AutoComplete.asmx" ServiceMethod="GetAttributeSubGroup"
                            OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1"
                            CompletionListElementID="pnlAttributeSubGrpList" CompletionListItemCssClass="autocomplete_listitem"
                            CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListCssClass="autocomplete_list"
                            BehaviorID="AutoCompleteExtenderForSubGroup">
                        </cc1:AutoCompleteExtender>
                        <asp:Button runat="server" ID="BtnViewAll" Text="View All" ToolTip=" View All" CssClass="btn btnnew" />
                        <asp:Panel ID="pnlAttributeSubGrpList" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" />
                    </td>
                </tr>
            </table>
            <table style="width: 100%;" cellpadding="5px">
                <tbody>
                    <tr>
                        <td>
                            <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img1" alt="Attribute Sub Group Data" src="images/panelcollapse.png"
                                        onclick="Display(this,'divAttributeSubGroupData');" runat="server" style="margin-right: 2px;" />Attribute Sub Group Data</legend>
                                <div id="divAttributeSubGroupData">
                                    <table style="width: 80%; margin: auto">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvwMedExSubGroupMst" runat="server" Style="display: none; width: auto; margin: auto;"
                                                    AutoGenerateColumns="False" OnRowDataBound="gvmedexgrp_RowDataBound"
                                                    OnRowCommand="gvmedexgrp_RowCommand" OnPageIndexChanging="gvmedexgrp_PageIndexChanging"
                                                    OnRowEditing="gvmedexgrp_RowEditing" OnRowDeleting="gvmedexgrp_RowDeleting" TabIndex="7">
                                                    <Columns>
                                                        <asp:BoundField DataFormatString="number" HeaderText="Sr. No">
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vMedExSubGroupCode" HeaderText="Attribute SubGroup Code"></asp:BoundField>
                                                        <asp:BoundField DataField="vMedExSubGroupDesc" HeaderText="Attribute SubGroup Desc">
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vCDISCValue" HeaderText="Variable Name">
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vOtherValues" HeaderText="Other Value">
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vDisplayName" HeaderText="Display Name">
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Edit">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImbEdit" runat="server" TabIndex="6" ToolTip="Edit" ImageUrl="~/images/Edit2.gif" />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Delete">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImbDelete" runat="server" TabIndex="6" ToolTip="Delete" ImageUrl="~/Images/i_delete.gif"
                                                                    OnClientClick="return msgconfirmalert('Are you sure you want to Delete?',this);
" />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemStyle HorizontalAlign="Center" />
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
                                                                    <img src="images/Export.gif" onclick="ExportToExcel(this.id);" id='<%# Eval("vMedExSubGroupCode")%>' />
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
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnExportToExcelGrid" />
        </Triggers>
    </asp:UpdatePanel>
    <div>
        <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpMedExSubGroupMstAuditTrail" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div id="dvMedExSubGroupMstAuditTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
                                <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                    <tr>
                                        <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 97%;">
                                            <asp:Label ID="lblRamge" runat="server" Text="Audit Trail Information"></asp:Label>
                                        </td>
                                        <td style="width: 3%">
                                            <img id="imgMedExSubGroupMstAuditTrail" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
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
                                                        <table id="tblMedExSubGroupMstAudit" class="tblAudit" width="100%" style="background-color: aliceblue;"></table>
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

    <cc1:ModalPopupExtender ID="MPE_MedExSubGroupMstHistory" runat="server" PopupControlID="dvMedExSubGroupMstAuditTrail" BehaviorID="MPE_MedExSubGroupMstHistory"
        PopupDragHandleControlID="LbldRandomizationDtl" BackgroundCssClass="modalBackground" CancelControlID="imgMedExSubGroupMstAuditTrail"
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
    <asp:HiddenField runat="server" ID="hdnMedExSubGroupCode" />
    <asp:Button ID="btnExportToExcel" runat="server" Style="display: none;" />

    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>

    <script type="text/javascript" src="Script/AutoComplete.js" language="javascript"></script>

    <script type="text/javascript">



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

        function HideAttributeSubGroupDetails() {
            $('#<%= img2.ClientID%>').click();
        }

        function AudtiTrail(e) {
            var vMedExSubGroupCode = $("#" + e.id).attr("vMedExSubGroupCode");

            if (vMedExSubGroupCode != "") {
                $.ajax({
                    type: "post",
                    url: "frmMedExSubGroupMst.aspx/AuditTrail",
                    data: '{"vMedExSubGroupCode":"' + vMedExSubGroupCode + '"}',
                    contentType: "application/json; charset=utf-8",
                    datatype: JSON,
                    async: false,
                    success: function (data) {
                        $('#tblMedExSubGroupMstAudit').attr("IsTable", "has");
                        var aaDataSet = [];
                        var range = null;

                        if (data.d != "" && data.d != null) {
                            data = JSON.parse(data.d);
                            for (var Row = 0; Row < data.length; Row++) {
                                var InDataSet = [];
                                InDataSet.push(data[Row].SrNo, data[Row].MedExSubGroupDesc, data[Row].CDISCValue, data[Row].OtherValues,data[Row].DisplayName, data[Row].Remark, data[Row].ModifyBy, data[Row].ModifyOn);
                                aaDataSet.push(InDataSet);
                            }
                        }
                        if ($("#tblMedExSubGroupMstAudit").children().length > 0) {
                            $("#tblMedExSubGroupMstAudit").dataTable().fnDestroy();
                        }
                        oTable = $('#tblMedExSubGroupMstAudit').prepend($('<thead>').append($('#tblMedExSubGroupMstAudit tr:first'))).dataTable({

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
                                { "sTitle": "Attribute Sub Group Desc" },
                                   { "sTitle": "Variable Name" },
                                   { "sTitle": "Other Value" },
                                   { "sTitle": "Display Name" },
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
                        //oTable.fnAdjustColumnSizing();
                        //$('.DataTables_sort_wrapper').click;
                        $find('MPE_MedExSubGroupMstHistory').show();
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
            $("#<%= hdnMedExSubGroupCode.ClientID()%>").val(id);
            var btn = document.getElementById('<%= btnExportToExcel.ClientId()%>')
            btn.click();
        }

        function Validation() {
            if (document.getElementById('<%=txtMedExSubGropuDesc.ClientID%>').value.toString().replace(/^\s*/, '').replace(/\s*$/, '').length <= 0) {
                document.getElementById('<%=txtMedExSubGropuDesc.ClientID%>').value = '';
                document.getElementById('<%=txtMedExSubGropuDesc.ClientID%>').focus();
                msgalert('Please Enter Attribute Sub Group !');
                return false;
            }
            if (document.getElementById("<%=txtDisplayName.ClientID%>").value.trim() == "") {
                msgalert("Please Enter Display Name !");
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

        function UIgvwMedExSubGroupMst() {
            $('#<%= gvwMedExSubGroupMst.ClientID%>').removeAttr('style', 'display:block');
            oTab = $('#<%= gvwMedExSubGroupMst.ClientID%>').prepend($('<thead>').append($('#<%= gvwMedExSubGroupMst.ClientID%> tr:first'))).dataTable({
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

        function ClientPopulated(sender, e) {
            SubjectClientShowing('AutoCompleteExtenderForSubGroup', $get('<%= txtSubGroup.ClientId %>'));
        }

        function OnSelected(sender, e) {
            SubjectOnItemSelected(e.get_value(), $get('<%= txtSubGroup.clientid %>'),
         $get('<%= HSubGroupId.clientid %>'), document.getElementById('<%= btnSetSubGroup.ClientId %>'));
        }

    </script>

</asp:Content>
