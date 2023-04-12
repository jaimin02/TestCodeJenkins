<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmActivityGroupMst.aspx.vb" Inherits="frmActivityGroupMst" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

        /*#ctl00_CPHLAMBDA_GV_ActivityGroup_wrapper {
            margin: 0px 235px;
        }*/
    </style>
    <script type="text/javascript" src="Script/scrollablegrid.js"></script>
    <table style="margin: auto; width: 100%; padding-top: 10px;">
        <tr>
            <td>
                <fieldset class="FieldSetBox" style="display: block; width: 98%; text-align: left; border: #aaaaaa 1px solid;">
                    <legend class="LegendText" style="color: Black; font-size: 12px">
                        <img id="img2" alt="Activity Group Details" src="images/panelcollapse.png"
                            onclick="Display(this,'divActivityDetail');" runat="server" style="margin-right: 2px;" />Activity Group Details</legend>
                    <div id="divActivityDetail">
                        <table width="100%">
                            <tr>
                                <td class="Label" style="text-align: right; width: 15%;">Project Type* :
                                </td>
                                <td style="text-align: left; width: 20%;">
                                    <asp:DropDownList ID="ddlProjectType" runat="server" Style="width: 100%; height: auto;" CssClass="dropDownList" />
                                </td>
                                <td class="Label" style="text-align: right; width: 13%;">Activity Group Name* :
                                </td>
                                <td style="text-align: left; width: 20%;">
                                    <asp:TextBox ID="txtActivityGroupName" runat="server" CssClass="textBox" Width="99%" />
                                </td>
                                <td style="text-align: right;" class="Label width: 5%;">Remark* :
                                </td>
                                <td nowrap="nowrap" style="text-align: left; width: 23%;">
                                    <asp:TextBox ID="txtRemark" runat="server" Rows="3" Columns="20" CssClass="textBox" Style="width: auto; height: auto;" TextMode="MultiLine" MaxLength="500" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" style="text-align: center; padding-top: 10px;">
                                    <asp:Button ID="BtnSave" runat="server" CssClass="btn btnsave" Text="Save" OnClientClick="return Validation();"
                                        ToolTip="Save" />
                                    <asp:Button ID="btnExportToExcelGrid" runat="Server" Font-Size="Smaller" CssClass="btn btnexcel" ToolTip="Export To Excel" />
                                    <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" Text="Cancel"
                                        ToolTip="Cancel" Style="margin-left: 5px;" />
                                    <asp:Button ID="BtnExit" runat="server" CssClass="btn btnexit" Text="Exit" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);"
                                        ToolTip="Exit" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </fieldset>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="Up_View" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table style="padding-top: 15px; width: 100%;">
                <tbody style="width:100%;">
                    <tr>
                        <td style="width:100%;">
                            <fieldset class="FieldSetBox" style="display: block; width: 98%; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img1" alt="Activity Group Data" src="images/panelcollapse.png"
                                        onclick="Display(this,'divActivityData');" runat="server" style="margin-right: 2px;" />Activity Group Data</legend>
                                <div id="divActivityData" style="width:100%;">
                                    <table width="98%">
                                        <tr>
                                            <td style="width:100%;">
                                                <div style="height: 100%;margin:auto; width: 85%; overflow: 0 auto;" class="grid">
                                                    <asp:GridView ID="GV_ActivityGroup" runat="server" Style="display: none; width: 100%; margin: auto;" AutoGenerateColumns="False"
                                                        OnRowCommand="GV_ActivityGroup_RowCommand" OnRowDataBound="GV_ActivityGroup_RowDataBound"
                                                        OnPageIndexChanging="GV_ActivityGroup_PageIndexChanging">
                                                        <Columns>
                                                            <asp:BoundField HeaderText="Sr. No" />
                                                            <asp:BoundField DataField="vProjectTypeName" HeaderText="Project Type" />
                                                            <asp:BoundField DataField="vActivityGroupId" HeaderText="Activity Group Id" />
                                                            <asp:BoundField DataField="vActivityGroupName" HeaderText="Activity Group Name" />
                                                            <asp:TemplateField HeaderText="Edit" SortExpression="status">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="ImbEdit" runat="server" ImageUrl="~/images/Edit2.gif" ToolTip="Edit" />
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
                                                                        <img src="images/Export.gif" onclick="ExportToExcel(this.id);" id='<%# Eval("vActivityGroupId")%>' />
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
            <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <div>
        <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpActivityGroupMstAuditTrail" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div id="dvActivityGroupMstAudiTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
                                <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                    <tr>
                                        <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 97%;">
                                            <asp:Label ID="lblRamge" runat="server" Text="Audit Trail Information"></asp:Label>
                                        </td>
                                        <td style="width: 3%">
                                            <img id="imgActivityGroupMstAuditTrail" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
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
                                                        <table id="tblActivityGroupMstAudit" class="tblAudit" width="100%" style="background-color: aliceblue;"></table>
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

    <cc1:ModalPopupExtender ID="MPE_ActivityGroupMstHistory" runat="server" PopupControlID="dvActivityGroupMstAudiTrail" BehaviorID="MPE_ActivityGroupMstHistory"
        PopupDragHandleControlID="LbldRandomizationDtl" BackgroundCssClass="modalBackground" CancelControlID="imgActivityGroupMstAuditTrail"
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

    <asp:HiddenField runat="server" ID="hdnActivityGroupId" />
    <asp:Button ID="btnExportToExcel" runat="server" Style="display: none;" />


    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>

    <script type="text/javascript" language="javascript">

        function HideActivityGroupDetails() {
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
            var vActivityGroupId = $("#" + e.id).attr("vActivityGroupId");

            if (vActivityGroupId != "") {
                $.ajax({
                    type: "post",
                    url: "frmActivityGroupMst.aspx/AuditTrail",
                    data: '{"vActivityGroupId":"' + vActivityGroupId + '"}',
                    contentType: "application/json; charset=utf-8",
                    datatype: JSON,
                    async: false,
                    success: function (data) {
                        $('#tblActivityGroupMstAudit').attr("IsTable", "has");
                        var aaDataSet = [];
                        var range = null;

                        if (data.d != "" && data.d != null) {
                            data = JSON.parse(data.d);
                            for (var Row = 0; Row < data.length; Row++) {
                                var InDataSet = [];
                                InDataSet.push(data[Row].SrNo, data[Row].ProjectTypeName, data[Row].ActivityGroupName, data[Row].Remark, data[Row].ModifyBy, data[Row].ModifyOn);
                                aaDataSet.push(InDataSet);
                            }
                        }
                        if ($("#tblActivityGroupMstAudit").children().length > 0) {
                            $("#tblActivityGroupMstAudit").dataTable().fnDestroy();
                        }
                        oTable = $('#tblActivityGroupMstAudit').prepend($('<thead>').append($('#tblActivityGroupMstAudit tr:first'))).dataTable({

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
                             { "sTitle": "Project Type" },
                             { "sTitle": "Activity Group Name" },
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
                        $find('MPE_ActivityGroupMstHistory').show();
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

        function Validation() {
            if (document.getElementById('<%=ddlProjectType.clientid %>').selectedIndex == 0) {
                msgalert('Please Select Project Type !');
                return false;
            }
            else if (document.getElementById('<%=txtActivityGroupName.ClientID%>').value.toString().replace(/^\s*/, '').replace(/\s*$/, '').length <= 0) {
                document.getElementById('<%=txtActivityGroupName.ClientID%>').value = '';
                document.getElementById('<%=txtActivityGroupName.ClientID%>').focus();
                msgalert('Please Enter Activity Group Name !');
                return false;
            }
        if (document.getElementById("<%=btnSave.ClientID%>").value.trim() == "Update") {
                if (document.getElementById("<%=txtRemark.ClientID%>").value.trim() == "") {
                msgalert("Please Enter Remarks !");
                return false;
            }
        }
    }

    function UIGV_ActivityGroup() {
        $('#<%= GV_ActivityGroup.ClientID%>').removeAttr('style', 'display:block');
        oTab = $('#<%= GV_ActivityGroup.ClientID%>').prepend($('<thead>').append($('#<%= GV_ActivityGroup.ClientID%> tr:first'))).dataTable({
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
        //alert(msg);
        //window.location.href = "frmActivityGroupMst.aspx?mode=1";
        alertdooperation(msg, 1, "frmActivityGroupMst.aspx?mode=1");
    }
    function ExportToExcel(id) {
        $("#<%= hdnActivityGroupId.ClientID()%>").val(id);
            var btn = document.getElementById('<%= btnExportToExcel.ClientId()%>')
            btn.click();
        }

        // for fix gridview header aded on 22-nov-2011
        // function pageLoad() {
        // FreezeTableHeader($('#<%= GV_ActivityGroup.ClientID %>'), { height: 250, width: 900 });
        //}

    </script>
</asp:Content>
