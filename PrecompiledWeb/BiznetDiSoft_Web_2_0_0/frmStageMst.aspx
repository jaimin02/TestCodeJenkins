<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmStageMst, App_Web_22suyskz" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

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
        /*#ctl00_CPHLAMBDA_gvstage_wrapper {
            margin: 0px 235px;
        }*/
    </style>

    <table cellpadding="5px" style="width: 100%;">
        <tr>
            <td>
                <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                    <legend class="LegendText" style="color: Black; font-size: 12px">
                        <img id="img2" alt="Stage Details" src="images/panelcollapse.png"
                            onclick="Display(this,'divStageDetail');" runat="server" style="margin-right: 2px;" />Stage Details</legend>
                    <div id="divStageDetail">
                        <table width="98%">
                            <tr>
                                <td class="Label" style="width: 16%; text-align: right;">Name* :
                                </td>
                                <td style="text-align: left; width: 20%;">
                                    <asp:TextBox ID="txtName" runat="server" CssClass="textBox" Width="99%" MaxLength="50" />
                                </td>
                                <td class="Label" nowrap="noWrap" style="width: 10%; text-align: right;">Sequence No* :
                                </td>
                                <td style="text-align: left; width: 13%;" class="Label ">
                                    <asp:DropDownList ID="ddlseqno" runat="server" CssClass="dropDownList" Width="88%">
                                        <asp:ListItem>0</asp:ListItem>
                                        <asp:ListItem>1</asp:ListItem>
                                        <asp:ListItem>2</asp:ListItem>
                                        <asp:ListItem>3</asp:ListItem>
                                        <asp:ListItem>4</asp:ListItem>
                                        <asp:ListItem>5</asp:ListItem>
                                        <asp:ListItem>6</asp:ListItem>
                                        <asp:ListItem>7</asp:ListItem>
                                        <asp:ListItem>8</asp:ListItem>
                                        <asp:ListItem>9</asp:ListItem>
                                        <asp:ListItem>10</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td class="Label" style="width: 7%; text-align: right">Remarks* :
                                </td>
                                <td style="text-align: left; width: 25%;">
                                    <asp:TextBox ID="txtRemark" runat="server" Rows="3" Columns="20" CssClass="textBox" Style="width: 55%; height: auto;" TextMode="MultiLine" MaxLength="500" />
                                </td>
                            </tr>
                            <tr>
                                <%--<td align="left" class="Label" valign="top">
                    </td>--%>
                                <td nowrap="nowrap" valign="top" class="Label" colspan="6" style="text-align: center;">
                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btnsave" Text="Save" ToolTip="Save"
                                        OnClientClick=" return Validation()" />
                                    <asp:Button ID="btnExportToExcelGrid" runat="Server" Font-Size="Smaller" CssClass="btn btnexcel"
                                        ToolTip="Export To Excel" />
                                    <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="btn btncancel"
                                        OnClick="btnCancel_Click" Text="Cancel" ToolTip="Cancel" />
                                    <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="btn btnclose"
                                        Text="Exit" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);"
                                        ToolTip="Exit" />
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
                                    <img id="img1" alt="Stage Data" src="images/panelcollapse.png"
                                        onclick="Display(this,'divStageData');" runat="server" style="margin-right: 2px;" />Stage Data</legend>
                                <div id="divStageData">
                                    <table style="margin: auto; width: 80%;">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvstage" runat="server" Style="display: none; width: 60%; margin: auto;" AutoGenerateColumns="False"
                                                    OnRowCreated="gvstage_RowCreated" OnPageIndexChanging="gvstage_PageIndexChanging"
                                                    OnRowDataBound="gvstage_RowDataBound" OnRowCommand="gvstage_RowCommand">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Sr. No">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" Font-Underline="False" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vStageDesc" HeaderText="Stage Name">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="iSequenceNo" HeaderText="Sequence No">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd-MMM-yyyy}" DataField="dModifyOn"
                                                            HeaderText="Modify On">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField SortExpression="status" HeaderText="Edit">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="lnkEdit" runat="server" ImageUrl="~/images/Edit2.gif" ToolTip="Edit" />
                                                                <%--<asp:LinkButton ID="lnkEdit" Text="Edit" runat="server"></asp:LinkButton>--%>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="iStageId" HeaderText="Stage Id">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
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
                                                                    <image src="images/Export.gif" onclick="ExportToExcel(this.id);" id='<%# Eval("iStageId")%>' />
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
        </Triggers>
    </asp:UpdatePanel>
    <div>
        <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpStageMstAuditTrail" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div id="dvStageMstAuditTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
                                <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                    <tr>
                                        <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 97%;">
                                            <asp:Label ID="lblRamge" runat="server" Text="Audit Trail Information"></asp:Label>
                                        </td>
                                        <td style="width: 3%">
                                            <img id="imgStageMstAuditTrail" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
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
                                                        <table id="tblStageMstAudit" class="tblAudit" width="100%" style="background-color: aliceblue;"></table>
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

    <cc1:ModalPopupExtender ID="MPE_StageMstHistory" runat="server" PopupControlID="dvStageMstAuditTrail" BehaviorID="MPE_StageMstHistory"
        PopupDragHandleControlID="LbldRandomizationDtl" BackgroundCssClass="modalBackground" CancelControlID="imgStageMstAuditTrail"
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
    <asp:HiddenField runat="server" ID="hdnStageId" />
    <asp:Button ID="btnExportToExcel" runat="server" Style="display: none;" />
    <script type="text/javascript" language="javascript">

        function HideStageDetails() {
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
            var iStageId = $("#" + e.id).attr("iStageId");

            if (iStageId != "") {
                $.ajax({
                    type: "post",
                    url: "frmStageMst.aspx/AuditTrail",
                    data: '{"iStageId":"' + iStageId + '"}',
                    contentType: "application/json; charset=utf-8",
                    datatype: JSON,
                    async: false,
                    success: function (data) {
                        $('#tblStageMstAudit').attr("IsTable", "has");
                        var aaDataSet = [];
                        var range = null;

                        if (data.d != "" && data.d != null) {
                            data = JSON.parse(data.d);
                            for (var Row = 0; Row < data.length; Row++) {
                                var InDataSet = [];
                                InDataSet.push(data[Row].SrNo, data[Row].StageDesc, data[Row].SequenceNo, data[Row].Remark, data[Row].ModifyBy, data[Row].ModifyOn);
                                aaDataSet.push(InDataSet);
                            }
                        }
                        if ($("#tblStageMstAudit").children().length > 0) {
                            $("#tblStageMstAudit").dataTable().fnDestroy();
                        }
                        oTable = $('#tblStageMstAudit').prepend($('<thead>').append($('#tblStageMstAudit tr:first'))).dataTable({

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
                                   { "sTitle": "Stage Name" },
                                   { "sTitle": "Sequence No" },
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
                        $find('MPE_StageMstHistory').show();
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
            $("#<%= hdnStageId.ClientID()%>").val(id);
            var btn = document.getElementById('<%= btnExportToExcel.ClientId()%>')
            btn.click();
        }
        function UIgvstage() {
            $('#<%= gvstage.ClientID%>').removeAttr('style', 'display:block');
            oTab = $('#<%= gvstage.ClientID%>').prepend($('<thead>').append($('#<%= gvstage.ClientID%> tr:first'))).dataTable({
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
            if (document.getElementById('<%=txtName.ClientID%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=txtName.ClientID%>').value = ''
                msgalert('Please Enter Stage Name !');
                document.getElementById('<%=txtName.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=ddlseqno.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Sequence No !');
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
           // alert(msg);
            //window.location.href = "frmStageMst.aspx?mode=1 ";
            alertdooperation(msg, 1, "frmStageMst.aspx?mode=1");
        }

    </script>

</asp:Content>
