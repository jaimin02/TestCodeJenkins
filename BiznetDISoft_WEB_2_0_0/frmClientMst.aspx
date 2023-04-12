<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmClientMst.aspx.vb" Inherits="frmClientMst" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <%--added by ketan--%>
    <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }
    </style>
    <%--ended by ketan--%>
    <script type="text/javascript" src="Script/scrollablegrid.js"></script>

    <div runat="server">
        <asp:GridView runat="server" ID="gvExport" AutoGenerateColumns="true" Style="display: none"
            HeaderStyle-BackColor="#1560a1" HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" Header-style-font-size="10px !importnat" HeaderStyle-Font-Names="Verdana, Arial, Helvetica, sans-serif"
            HeaderStyle-Font-Size=" 0.9em"
            HeaderStyle-HorizontalAlign="Center" CellPadding="3" CellSpacing="0"
            RowStyle-Font-Names=" Verdana, Arial, Helvetica, sans-serif" RowStyle-Font-Size="10pt"
            RowStyle-BackColor="#84c8e6" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#191970" RowStyle-ForeColor="#191970">
        </asp:GridView>
    </div>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellpadding="5px" style="width: 100%;">
                <tr>
                    <td>
                        <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                            <legend class="LegendText" style="color: Black; font-size: 12px">
                                <img id="img2" alt="Client Details" src="images/panelcollapse.png"
                                    onclick="Display(this,'divClientDetail');" runat="server" style="margin-right: 2px;" />Client Details</legend>
                            <div id="divClientDetail">
                                <table width="98%">
                                    <tr>
                                        <td class="Label" style="text-align: right; width: 15%;">Client Name* :
                                        </td>
                                        <td style="text-align: left; width: 20%;">
                                            <asp:TextBox ID="txtClientName" runat="server" CssClass="textBox" Width="97%" MaxLength="100" />
                                        </td>
                                        <td class="Label" nowrap="nowrap" style="text-align: right; width: 11%;">Project Manager* :
                                        </td>
                                        <td align="left" style="text-align: left; width: 20%;">
                                            <asp:DropDownList ID="ddlProjectMngr" runat="server" CssClass="dropDownList" Width="98%"
                                                AutoPostBack="false">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="Label" style="text-align: right; width: 7%;">Remarks :
                                        </td>
                                        <td style="text-align: left; width: 27%;">
                                            <asp:TextBox ID="txtremark" runat="server" CssClass="textBox" Width="70%" Height="50px" TextMode="MultiLine"
                                                onKeyUp="CheckTextLength(this,250)" onChange="CheckTextLength(this,250)" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6" style="text-align: center;">
                                            <asp:Button ID="btnSave" runat="server" Text="Save" ToolTip="Save" CssClass="btn btnsave"
                                                OnClientClick="return ValidateClient();" />
                                            <asp:Button ID="btnCancel" OnClick="btnCancel_Click" OnClientClick=" ResetControl(); return true; " runat="server" Text="Cancel"
                                                ToolTip="Cancel" CssClass="btn btncancel" CausesValidation="False" />
                                            <asp:Button ID="btnClose" runat="server" Text="Exit" ToolTip="Exit" CssClass="btn btnclose"
                                                OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" CausesValidation="False" />
                                            <asp:Button ID="btnExcel" runat="server" ToolTip="Export To Excel"
                                                CssClass="btn btnexcel"/>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </td>
                </tr>
            </table>
            <table style="width: 100%;" cellpadding="5px">
                <tbody>
                    <tr>
                        <td>
                            <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img1" alt="Sponsor Data" src="images/panelcollapse.png"
                                        onclick="Display(this,'divSponsorData');" runat="server" style="margin-right: 2px;" />Client Data</legend>
                                <div id="divSponsorData">
                                    <table style="margin: auto; width: 80%;">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvclient" runat="server" Style="width: auto; margin: auto; display: none;"
                                                    OnRowCommand="gvclient_RowCommand" OnRowDataBound="gvclient_RowDataBound" AutoGenerateColumns="False"
                                                    ShowFooter="false">
                                                    <Columns>
                                                        <asp:BoundField DataFormatString="number" HeaderText="Sr. No">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vClientCode" HeaderText="ClientCode">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vClientName" HeaderText="Client Name">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ProjectMangerWithProfile" HeaderText="Project Manager">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vRemark" HeaderText="Remarks">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="iUserId" HeaderText="UserId">
                                                            <ItemStyle HorizontalAlign="Left" />
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

                                                        <asp:TemplateField HeaderText="Add Contacts">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="lnkAdd" runat="server" ImageUrl="~/Images/addcontacts.png" ToolTip="Add Contacts" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Export">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <img src="images/Export.gif" onclick="ExportToExcel(this.id);" id='<%#  Eval("vClientCode") %>' />
                                                                    <%--<asp:ImageButton ID="imgExport"  runat="server" ImageUrl="images/Export.gif" ToolTip="Export To Excel " />--%>
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

            <div>
                <table>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpClientAuditTrail" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <div id="dvClientMstAudiTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
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
                                                                <table id="tblClientMstAudit" class="tblAudit" width="100%" style="background-color: aliceblue;"></table>
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
            <cc1:ModalPopupExtender ID="MPE_CleintMstHistory" runat="server" PopupControlID="dvClientMstAudiTrail" BehaviorID="MPE_CleintMstHistory"
                PopupDragHandleControlID="LbldRandomizationDtl" BackgroundCssClass="modalBackground" CancelControlID="imgClientAuditTrail"
                TargetControlID="btn3">
            </cc1:ModalPopupExtender>
            <div>
                <table>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpLocationMstAuditTrail" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <div id="divExportClient" runat="server" class="centerModalPopup" style="display: none; width: 30%;">
                                        <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                            <tbody>
                                                <tr align="center">
                                                    <td style="width: 100%; height: 34px">
                                                        <asp:RadioButtonList ID="rblst" runat="server" RepeatDirection="vertical">
                                                            <asp:ListItem Value="S">Export Client with Contacts</asp:ListItem>
                                                            <asp:ListItem Value="P">Export Client without Contacts</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr align="center">
                                                    <td style="width: 295px; height: 21px">
                                                        <asp:Button ID="btnDivExp" runat="Server" CssClass="btn btnexcel" Text="" ToolTip="Export" />
                                                        <asp:Button ID="btnDivExit" runat="Server" CssClass="btn btnexit" Width="71px" Text="Exit"
                                                            ToolTip="Exit" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </div>
            <cc1:ModalPopupExtender ID="MPE_Export" runat="server" PopupControlID="divExportClient" BehaviorID="MPE_Export"
                BackgroundCssClass="modalBackground" CancelControlID="btnDivExit"
                TargetControlID="btnExcel">
            </cc1:ModalPopupExtender>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnCancel" />
            <asp:PostBackTrigger ControlID="btnExcel" />
            <asp:PostBackTrigger ControlID="btnDivExp" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:HiddenField runat="server" ID="hdnClientCode" />
    <asp:Button ID="btnExportToExcel" runat="server" Style="display: none;" />


    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <%--<script type="text/javascript" src="Script/jquery-1.4.3.min.js"></script>--%>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>

    <script type="text/javascript" language="javascript">

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

        function ShowAlert(msg) {
            //msgalert(msg);
            //window.location.href = "frmClientMst.aspx?mode=1";
            alertdooperation(msg, 1, "frmClientMst.aspx?mode=1");
        }

        function ValidateClient() {
            var e = document.getElementById("<%=ddlProjectMngr.ClientID%>");
            var ddlval = e.options[e.selectedIndex].text;
            if (document.getElementById("<%=txtClientName.ClientID%>").value.trim() == "") {
                msgalert("Please enter Client name !");
                document.getElementById("<%=txtClientName.ClientID%>").value = ""
                document.getElementById("<%=txtClientName.ClientID%>").focus();
                return false;
            }
            if (ddlval == "Select Project Manager") {
                msgalert("Please select Project manager !");
                return false;
            }
            if (document.getElementById("<%=btnSave.ClientID%>").value.trim() == "Update") {
                if (document.getElementById("<%=txtremark.ClientID%>").value.trim() == "") {
                    msgalert("Please enter Remarks !");
                    return false;
                }
            }
            return true;
        }

        function UIgvClient() {
            $('#<%= gvclient.ClientID%>').removeAttr('style', 'display:block');
            oTab = $('#<%= gvclient.ClientID%>').prepend($('<thead>').append($('#<%= gvclient.ClientID%> tr:first'))).dataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": false,
                "sScrollY": "250px",
                "sScrollX": "100%",
                "bScrollCollapse": true,
                "bDestroy": true,
                aLengthMenu: [
                    [10, 25, 50, 100, -1],
                    [10, 25, 50, 100, "All"]
                ],
            });
            return false;
        }
        function ResetControl() {
            document.getElementById("ctl00_CPHLAMBDA_txtClientName").value = "";
            document.getElementById("ctl00_CPHLAMBDA_txtremark").value = "";
            document.getElementById("ctl00_CPHLAMBDA_ddlProjectMngr").selectedIndex = "0";

            return false;

        }
        function CheckTextLength(text, long) {
            var maxlength = new Number(long); // Change number to your max length.
            if (text.value.length > maxlength) {
                text.value = text.value.substring(0, maxlength);
                msgalert(" Only " + long + " characters allowed");
            }
        }
        function AudtiTrail(e) {
            var vclientcode = $("#" + e.id).attr("vclientcode");

            if (vclientcode != "") {
                $.ajax({
                    type: "post",
                    url: "frmClientMst.aspx/AuditTrail",
                    data: '{"vclientcode":"' + vclientcode + '"}',
                    contentType: "application/json; charset=utf-8",
                    datatype: JSON,
                    async: false,
                    success: function (data) {
                        $('#tblClientMstAudit').attr("IsTable", "has");
                        var aaDataSet = [];
                        var range = null;

                        if (data.d != "" && data.d != null) {
                            data = JSON.parse(data.d);
                            for (var Row = 0; Row < data.length; Row++) {
                                var InDataSet = [];
                                InDataSet.push(data[Row].SrNo, data[Row].ClientName, data[Row].ProjectManager, data[Row].Remarks, data[Row].ModifyBy, data[Row].ModifyOn);
                                aaDataSet.push(InDataSet);
                            }

                        }
                        if ($("#tblClientMstAudit").children().length > 0) {
                            $("#tblClientMstAudit").dataTable().fnDestroy();
                        }

                        oTable = $('#tblClientMstAudit').prepend($('<thead>').append($('#tblClientMstAudit tr:first'))).dataTable({

                            "bJQueryUI": true,
                            "sPaginationType": "full_numbers",
                            "bLengthChange": true,
                            "iDisplayLength": 10,
                            "bProcessing": true,
                            "bSort": false,
                            "aaData": aaDataSet,
                            aLengthMenu: [
                                    [10, 25, 50, 100, -1],
                                    [10, 25, 50, 100, "All"]
                            ],
                            "aoColumns": [
                                {
                                    "sTitle": "Sr. No",
                                },
                                { "sTitle": "Client Name" },
                                 { "sTitle": "Project Manager" },
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
            $("#<%= hdnClientCode.ClientID %>").val(id);
            var btn = document.getElementById('<%= btnExportToExcel.ClientId()%>')
            btn.click();
        }

    </script>

</asp:Content>
