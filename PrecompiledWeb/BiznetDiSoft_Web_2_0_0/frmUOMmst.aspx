<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmUOMmst, App_Web_5xoe1jh1" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

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
    
    <table cellpadding="5px" style="width: 100%;">
        <tr>
            <td>
                <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                    <legend class="LegendText" style="color: Black; font-size: 12px">
                        <img id="img2" alt="UOM Details" src="images/panelcollapse.png"
                            onclick="Display(this,'divUOMDetail');" runat="server" style="margin-right: 2px;" />UOM Details</legend>
                    <div id="divUOMDetail">
                        <table width="98%">
        <tr>
            <td class="Label" style=" vertical-align: top; width:17%; text-align: right;">UOM Class* :
            </td>
            <td style="text-align: left; width:20%; vertical-align: top;" class="Label" nowrap="noWrap">
                <asp:TextBox ID="txtUOMClass" runat="server" CssClass="textBox" Width="99%" TabIndex="1"
                    MaxLength="25" />
            </td>
            <td class="Label" style="vertical-align: top; width:8%; text-align: right;">UOM Desc* :
            </td>
            <td style="text-align: left; vertical-align: top; width:20%;" class="Label" nowrap="noWrap">
                <asp:TextBox ID="txtUOMDesc" runat="server" Width="99%" CssClass="textBox" TabIndex="2"
                    MaxLength="50" />
            </td>
            <td class="Label" style="vertical-align: top; width:7%; text-align: right;">Remarks :
            </td>
            <td style="text-align: left; vertical-align: top; width:28%;" class="Label" nowrap="noWrap">
                <asp:TextBox ID="txtRemarks" runat="server" Width="70%" CssClass="textBox" TextMode="MultiLine" TabIndex="2"
                    MaxLength="50" />
            </td>
        </tr>
        <tr>
            <td style="text-align: center; vertical-align: top;" class="Label" nowrap="nowrap" colspan="6">
                <asp:Button ID="btnSave" OnClientClick="return Validation();" runat="server" CssClass="btn btnsave"
                    Text="Save" TabIndex="3" ToolTip="Save" />
                <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="btn btncancel"
                    OnClick="btnCancel_Click" Text="Cancel" TabIndex="5" ToolTip="Cancel" />
                <asp:Button ID="btnExit" runat="server" CausesValidation="False" CssClass="btn btnexit"
                    Text="Exit" ToolTip=" Exit" TabIndex="6" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
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
                                    <img id="img1" alt="UOM Data" src="images/panelcollapse.png"
                                        onclick="Display(this,'divUOMData');" runat="server" style="margin-right: 2px;" />UOM Data</legend>
                                <div id="divUOMData">
                                    <table style="margin: auto; width: 85%;">
                                        <tr>
                                            <td id="fldLegnedInActive" style="display: none">
                                                <fieldset class="FieldSetBox" style="width: 20%; float: right; margin-right: 0%; position: relative;">
                                                    <legend class="LegendText" style="color: Black">Legend</legend>
                                                    <table class="LabelText">
                                                        <tbody>
                                                            <tr>
                                                                <td>
                                                                    <div style="width: 20px; height: 20px; float: left; background-color: rgb(255, 189, 121); font-weight: bold;">
                                                                    </div>
                                                                </td>
                                                                <td>Delete UOM
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <div style="width: 100%; overflow: auto;">
                                                    <asp:GridView ID="gvwUOM" TabIndex="7" runat="server" Style="width: 50%; margin: auto; display: none" OnPageIndexChanging="gvwUOM_PageIndexChanging"
                                                        AutoGenerateColumns="False" OnRowCreated="gvwUOM_RowCreated"
                                                        OnRowCommand="gvwUOM_RowCommand" OnRowDataBound="gvwUOM_RowDataBound" OnRowEditing="gvwUOM_RowEditing"
                                                        OnRowDeleting="gvwUOM_RowDeleting">
                                                        <Columns>
                                                            <asp:BoundField HeaderText="#">
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="vUOMCode" HeaderText="UOM Code">
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="vUOMClass" HeaderText="UOM Class">
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="vUOMDesc" HeaderText="UOM Description">
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="cStatusIndi" HeaderText="cStatusIndi">
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:BoundField>

                                                            <asp:TemplateField SortExpression="status" HeaderText="Edit">
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="lnkEdit" runat="server" ImageUrl="~/images/Edit2.gif" ToolTip="Edit" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Delete">
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="lnkDelete" runat="server" ImageUrl="~/Images/i_delete.gif"
                                                                        ToolTip="Inactive" OnClientClick="return ShowModalPopup(this);"></asp:ImageButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField SortExpression="status" HeaderText="AuditTrail">
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="lnkAudit" runat="server" ImageUrl="~/Images/audit.png" ToolTip="Audit Trial" OnClientClick="AuditTrail(this); return false;" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Export">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <img src="images/Export.gif" onclick="ExportToExcel(this.id);" id='<%#  Eval("vUOMCode") %>' alt="" />
                                                                        <%--<asp:ImageButton ID="imgExport"  runat="server" ImageUrl="images/Export.gif" ToolTip="Export To Excel " />--%>
                                                                    </center>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
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
            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnExit" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <div>
        <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UomMst" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div id="dvUomMstAuditTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
                                <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                    <tr>
                                        <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 97%;">
                                            <asp:Label ID="lblRamge" runat="server" Text="Audit Trail Information"></asp:Label>
                                        </td>
                                        <td style="width: 3%">
                                            <img id="imgUomAuditTrail" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
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
                                                        <table id="tblUOMMstAudit" class="tblAudit" width="100%" style="background-color: aliceblue; text-align: left"></table>
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
    <cc1:ModalPopupExtender ID="MPE_UOMMstHistory" runat="server" PopupControlID="dvUomMstAuditTrail" BehaviorID="MPE_UOMMstHistory"
        PopupDragHandleControlID="LbldRandomizationDtl" BackgroundCssClass="modalBackground" CancelControlID="imgUomAuditTrail"
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
                        Width="300px" TabIndex="55"></asp:TextBox>
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
                    <asp:Button ID="btnRemarksCancel" runat="server" Text="Cancel" CssClass="btn btncancel"
                        Width="64px" Style="font-size: 12px !important;" TabIndex="56" />

                </td>
            </tr>
        </table>
    </div>



    <asp:HiddenField runat="server" ID="hdnUOMCode" />

    <asp:Button ID="btnExportToExcel" runat="server" Style="display: none;" />

    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/scrollablegrid.js"></script>

    <script type="text/javascript" language="javascript">

        function HideUOMDetails() {
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

        function UIgvwUOM() {
            $('#<%= gvwUOM.ClientID%>').removeAttr('style', 'display:block');
            $('#fldLegnedInActive').removeAttr('style', 'display:block');
            oTab = $('#<%= gvwUOM.ClientID%>').prepend($('<thead>').append($('#<%= gvwUOM.ClientID%> tr:first'))).dataTable({
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
            
            msgConfirmDeleteAlert(null, "Are You Sure You Want To Delete UOM Data ?", function (isConfirmed) {
                if (isConfirmed) {
                    __doPostBack(e.name, '');
                    $('#<%= txtRemarks_delete.ClientID%>').val('');
                    $find('mdlRemarks').show();
                    return true;
                } else {
                    return false;
                }
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
            if (document.getElementById('<%=txtUOMClass.ClientID%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=txtUOMClass.ClientID%>').value = '';
                msgalert('Please Enter UOM Class!');
                document.getElementById('<%=txtUOMClass.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtUOMDesc.ClientID%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=txtUOMDesc.ClientID%>').value = '';
                msgalert('Please Enter UOM Description!');
                document.getElementById('<%=txtUOMDesc.ClientID%>').focus();
                return false;
            }
        if (document.getElementById("<%=btnSave.ClientID%>").value.trim() == "Update") {
                if (document.getElementById("<%=txtRemarks.ClientID%>").value.trim() == "") {
                msgalert("Please Enter Remarks!");
                return false;
            }
        }

        return true;
    }


    function ShowAlert(msg) {
       // alert(msg);
        //window.location.href = "frmUOMmst.aspx?mode=1";
        alertdooperation(msg, 1, "frmUOMmst.aspx?mode=1");
    }
    function AuditTrail(e) {
        debugger;
        var vUomCode = $("#" + e.id).attr("vUomCode");


        if (vUomCode != "") {
            $.ajax({
                type: "post",
                url: "frmUOMmst.aspx/AuditTrail",
                data: '{"vUomCode":"' + vUomCode + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                success: function (data) {
                    $('#tblUOMMstAudit').attr("IsTable", "has");
                    var aaDataSet = [];
                    var range = null;

                    data = JSON.parse(data.d);
                    for (var Row = 0; Row < data.length; Row++) {
                        var InDataSet = [];
                        InDataSet.push(data[Row].SrNo, data[Row].UOMClass, data[Row].UOMDesc, data[Row].Remarks, data[Row].ModifyBy, data[Row].ModifyOn);
                        aaDataSet.push(InDataSet);
                    }

                    if ($("#tblUOMMstAudit").children().length > 0) {
                        $("#tblUOMMstAudit").dataTable().fnDestroy();
                    }
                    oTable = $('#tblUOMMstAudit').prepend($('<thead>').append($('#tblUOMMstAudit tr:first'))).dataTable({



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

                             { "sTitle": "UOM Class" },
                             { "sTitle": "UOMDesc" },
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
                    $find('MPE_UOMMstHistory').show();

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
        $("#<%= hdnUOMCode.ClientID %>").val(id);
        var btn = document.getElementById('<%= btnExportToExcel.ClientId()%>')
        btn.click();
    }

    </script>

</asp:Content>
