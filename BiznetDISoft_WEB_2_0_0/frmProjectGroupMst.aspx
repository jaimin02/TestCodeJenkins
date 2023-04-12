<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmProjectGroupMst.aspx.vb" Inherits="frmProjectGroupMst" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

        /*#ctl00_CPHLAMBDA_GV_ProjectGroup_wrapper {
            margin: 0px 235px;
        }*/
    </style>

    <table width="100%" cellpadding="5px">
        <tr>
            <td >
                <fieldset class="FieldSetBox" style="display: block; width: 98%; text-align: left; border: #aaaaaa 1px solid;">
                    <legend class="LegendText" style="color: Black; font-size: 12px">
                        <img id="img2" alt="Project Group Details" src="images/panelcollapse.png"
                            onclick="Display(this,'divProjectGroup');" runat="server" style="margin-right: 2px;" />Project Group Details</legend>
                    <div id="divProjectGroup">
                        <table width="100%">
                            <tr>
                                <td class="Label" style="width: 15%; text-align: right;">Project Type* :
                                </td>
                                <td style="text-align: left; width: 20%;">
                                    <asp:DropDownList ID="ddlProjectType" runat="server" CssClass="dropDownList" Width="100%" />
                                </td>
                                <td style="text-align: right; width: 15%;" class="Label">Project Group Description*:
                                </td>
                                <td nowrap="nowrap" style="text-align: left; width: 20%;">
                                    <asp:TextBox ID="txtProjectGroupDesc" runat="server" CssClass="textBox" Width="99%"
                                        MaxLength="250" />
                                </td>
                                <td style="text-align: right; width: 5%;" class="Label">Remark :
                                </td>
                                <td nowrap="nowrap" style="text-align: left; width: 25%;">
                                    <asp:TextBox ID="txtRemark" runat="server" CssClass="textBox" TextMode="MultiLine" Height="106%" Width="60%" MaxLength="500" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center; padding-top: 1%;" class="Label" colspan="6">
                                    <asp:Button ID="BtnSave" runat="server" CssClass="btn btnsave" Text="Save" ToolTip="Save"
                                        OnClientClick="return Validation();" />
                                    <asp:Button ID="btnExportToExcelGrid" runat="Server" Font-Size="Smaller" CssClass="btn btnexcel" ToolTip="Export To Excel" />
                                    <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" Text="Cancel" ToolTip="Cancel" />
                                    <asp:Button ID="BtnExit" runat="server" CssClass="btn btnexit" Text="Exit" ToolTip="Exit"
                                        OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
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
            <table style="width: 100%; margin-top: 2%;">
                <tbody style="width:100%;">
                    <tr>
                        <td style="width:100%;">
                            <fieldset class="FieldSetBox" style="display: block; width: 98%; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img1" alt="Project Group Data" src="images/panelcollapse.png"
                                        onclick="Display(this,'divProjectData');" runat="server" style="margin-right: 2px;" />Project Group Data</legend>
                                <div id="divProjectData" style="width:100%;">
                                    <table style="width:98%";>
                                        <tr>
                                            <td style="text-align: center; width:100%;"">
                                                <div style="height: 100%;margin:auto; width: 85%; overflow: 0 auto;" class="grid">
                                                <asp:GridView ID="GV_ProjectGroup" runat="server" Style="display: none; width: 100%; margin: auto;" AutoGenerateColumns="False" OnRowCommand="GV_ProjectGroup_RowCommand"
                                                    OnRowCreated="GV_ProjectGroup_RowCreated" OnRowDataBound="GV_ProjectGroup_RowDataBound"
                                                    OnPageIndexChanging="GV_ProjectGroup_PageIndexChanging" OnRowDeleting="GV_ProjectGroup_RowDeleting">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Sr. No">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="nProjectGroupNo" HeaderText="Project Group No." />
                                                        <asp:BoundField DataField="vProjectTypeCode" HeaderText="vProjectTypeCode" />
                                                        <asp:BoundField DataField="vProjectTypeName" HeaderText="Project Type" />
                                                        <asp:BoundField DataField="vProjectGroupDesc" HeaderText="Project Group" />
                                                        <asp:BoundField DataField="vRemark" HeaderText="Remarks" />
                                                        <asp:TemplateField HeaderText="Edit" SortExpression="status">
                                                            <ItemTemplate>
                                                                <%-- <asp:LinkButton ID="lnkEdit" Text="Edit" runat="server"></asp:LinkButton>--%>
                                                                <asp:ImageButton ID="lnkEdit" runat="server" ImageUrl="~/images/Edit2.gif" ToolTip="Edit" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Delete" SortExpression="status">
                                                            <ItemTemplate>
                                                                <%--<asp:LinkButton ID="lnkDelete" Text="Delete" runat="server" OnClientClick="return confirm('Are You sure You want to DELETE?')"></asp:LinkButton>--%>
                                                                <asp:ImageButton ID="lnkDelete" runat="server" ImageUrl="~/Images/i_delete.gif"
                                                                    OnClientClick="return msgconfirmalert('Are you sure you want to Delete?',this);" ToolTip="Delete" />
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
                                                                    <img src="images/Export.gif" onclick="ExportToExcel(this.id);" id='<%# Eval("nProjectGroupNo")%>' />
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
                    <asp:UpdatePanel ID="UpProjectGroupMstAuditTrail" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div id="dvProjectGroupMstAudiTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
                                <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                    <tr>
                                        <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 97%;">
                                            <asp:Label ID="lblRamge" runat="server" Text="Audit Trail Information"></asp:Label>
                                        </td>
                                        <td style="width: 3%">
                                            <img id="imgProjectGroupMstAuditTrail" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
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
                                                        <table id="tblProjectGroupMstAudit" class="tblAudit" width="100%" style="background-color: aliceblue;"></table>
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

    <cc1:ModalPopupExtender ID="MPE_ProjectGroupMstHistory" runat="server" PopupControlID="dvProjectGroupMstAudiTrail" BehaviorID="MPE_ProjectGroupMstHistory"
        PopupDragHandleControlID="LbldRandomizationDtl" BackgroundCssClass="modalBackground" CancelControlID="imgProjectGroupMstAuditTrail"
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
    <asp:HiddenField runat="server" ID="hdnProjectGroupNo" />
    <asp:Button ID="btnExportToExcel" runat="server" Style="display: none;" />

    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <script language="javascript" type="text/javascript">

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
            var nProjectGroupNo = $("#" + e.id).attr("nProjectGroupNo");

            if (nProjectGroupNo != "") {
                $.ajax({
                    type: "post",
                    url: "frmProjectGroupMst.aspx/AuditTrail",
                    data: '{"nProjectGroupNo":"' + nProjectGroupNo + '"}',
                    contentType: "application/json; charset=utf-8",
                    datatype: JSON,
                    async: false,
                    success: function (data) {
                        $('#tblProjectGroupMstAudit').attr("IsTable", "has");
                        var aaDataSet = [];
                        var range = null;

                        if (data.d != "" && data.d != null) {
                            data = JSON.parse(data.d);
                            for (var Row = 0; Row < data.length; Row++) {
                                var InDataSet = [];
                                InDataSet.push(data[Row].SrNo, data[Row].ProjectTypeName, data[Row].ProjectGroupDesc, data[Row].Remark, data[Row].ModifyBy, data[Row].ModifyOn);
                                aaDataSet.push(InDataSet);
                            }

                        }
                        if ($("#tblProjectGroupMstAudit").children().length > 0) {
                            $("#tblProjectGroupMstAudit").dataTable().fnDestroy();
                        }
                        oTable = $('#tblProjectGroupMstAudit').prepend($('<thead>').append($('#tblProjectGroupMstAudit tr:first'))).dataTable({

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
                                { "sTitle": "Project Group" },
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
                        $find('MPE_ProjectGroupMstHistory').show();
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
            else if (document.getElementById('<%=txtProjectGroupDesc.ClientID%>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Project Group Description !');
                document.getElementById('<%=txtProjectGroupDesc.ClientID%>').focus();
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

    function UIGV_ProjectGroup() {
        $('#<%= GV_ProjectGroup.ClientID%>').removeAttr('style', 'display:block');
        oTab = $('#<%= GV_ProjectGroup.ClientID%>').prepend($('<thead>').append($('#<%= GV_ProjectGroup.ClientID%> tr:first'))).dataTable({
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
        //msgalert(msg);
        //window.location.href = "frmProjectGroupMst.aspx?mode=1";
        alertdooperation(msg, 1, "frmProjectGroupMst.aspx?mode=1");
    }
    function ExportToExcel(id) {
        $("#<%= hdnProjectGroupNo.ClientID%>").val(id);
        var btn = document.getElementById('<%= btnExportToExcel.ClientId()%>')
        btn.click();
    }
    </script>

</asp:Content>
