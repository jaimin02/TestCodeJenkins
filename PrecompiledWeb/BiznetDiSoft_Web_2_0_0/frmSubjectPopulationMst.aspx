<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmSubjectPopulationMst, App_Web_5xoe1jh1" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }
    </style>
    <script type="text/javascript" src="Script/scrollablegrid.js"></script>


    <div id="Div1"  runat="server">
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
                                <img id="img2" alt="Population Details" src="images/panelcollapse.png"
                                    onclick="Display(this,'divLocationDetail');" runat="server" style="margin-right: 2px;" />Population Details</legend>
                            <div id="divLocationDetail">
                                <table width="98%">
                                    <tr>
                                        <td class="Label" style="text-align: right; width:20%;">Population Name* :
                                        </td>
                                        <td style="text-align: left; width:30%;">
                                            <asp:TextBox ID="txtPopulationName" runat="server" CssClass="textBox" Width="100%" MaxLength="100" />
                                        </td>
                                        <td class="Label" nowrap="nowrap" style="text-align: right; width:7%;">Active :
                                        </td>
                                        <td style="text-align: left; width:3%;" class="Label">
                                            <asp:CheckBox ID="ChkActive" runat="server" Checked="true" />
                                        </td>
                                        <td class="Label" style="text-align: right; width:7%;">Remarks :
                                        </td>
                                        <td style="text-align: left; width:33%;">
                                            <asp:TextBox ID="txtremark" runat="server" CssClass="textBox" Width="75%" Height="50px" TextMode="MultiLine"
                                                onKeyUp="CheckTextLength(this,250)" onChange="CheckTextLength(this,250)" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center; vertical-align: middle;" colspan="6">
                                            <table width=" 100%">
                                                <tbody>
                                                    <tr>
                                                        <td colspan="2" align="center">
                                                            <div style="display: none; left: 172px; width: 273px; position: absolute; top: 510px; height: 89px"
                                                                id="divExportClient" class="DIVSTYLE2" runat="server">
                                                                <table width="100%">
                                                                    <tbody>
                                                                        <tr align="center">
                                                                            <td style="width: 100%; height: 34px">
                                                                                <%--<asp:RadioButtonList ID="rblst" runat="server" RepeatDirection="vertical">
                                                                    <asp:ListItem Value="S">Export Client with Contacts</asp:ListItem>
                                                                    <asp:ListItem Value="P">Export Client without Contacts</asp:ListItem>
                                                                </asp:RadioButtonList>--%>
                                                                            </td>
                                                                        </tr>
                                                                        <tr align="center">
                                                                            <td style="width: 295px; height: 21px">
                                                                                <asp:Button ID="btnDivExp" runat="Server" CssClass="btn btnexcel" Text="Export" ToolTip="Export" />
                                                                                <asp:Button ID="btnDivExit" runat="Server" CssClass="btn btnexit"  Text="Exit"
                                                                                    ToolTip="Exit" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="text-align: center;">
                                                            <asp:Button ID="btnSave" runat="server" Text="Save" ToolTip="Save" CssClass="btn btnsave"
                                                                OnClientClick="return ValidateClient();" />
                                                            <asp:Button ID="btnCancel" OnClick="btnCancel_Click" OnClientClick=" ResetControl(); return true; " runat="server" Text="Cancel"
                                                                ToolTip="Cancel" CssClass="btn btncancel" CausesValidation="False" />
                                                            <asp:Button ID="btnClose" runat="server" Text="Exit" ToolTip="Exit" CssClass="btn btnclose"
                                                                OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" CausesValidation="False" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>

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
                                    <img id="img1" alt="Population Data" src="images/panelcollapse.png"
                                        onclick="Display(this,'divLocationData');" runat="server" style="margin-right: 2px;" />Population Data</legend>
                                <div id="divLocationData">
                                    <table style="margin: auto; width: 80%;">
                                        <tr>
                                            <td colspan="2">
                                                <asp:GridView ID="gvPopulation" runat="server" Style="width: auto; margin: auto; display: none;"
                                                    OnRowCommand="gvPopulation_RowCommand" OnRowDataBound="gvPopulation_RowDataBound" AutoGenerateColumns="False"
                                                    ShowFooter="false">
                                                    <Columns>
                                                        <asp:BoundField DataFormatString="number" HeaderText="Sr. No">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="nPopulationId" HeaderText="PopulationId">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vPopulationName" HeaderText="Population Name">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="cActiveFlag" HeaderText="Active">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vRemarks" HeaderText="Remarks">
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
                                                        <asp:TemplateField HeaderText="Export">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <img src="images/Export.gif" onclick="ExportToExcel(this.id);" id='<%# Eval("nPopulationId")%>' />
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
                            <asp:UpdatePanel ID="UpClienAuditTrail" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <div id="dvClientMstAudiTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
                                        <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                            <tr>
                                                <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 97%;">
                                                    <asp:Label ID="lblRamge" runat="server" Text="Audit Trail Information" ></asp:Label>
                                                </td>
                                                <td style="width: 3%">
                                                    <img id="imgPopulationAuditTrail" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
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
                                                                <table id="tblPopulationMstAudit" class="tblAudit" width="100%" style="background-color: aliceblue;"></table>
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
            <cc1:ModalPopupExtender ID="MPE_PopulationMstHistory" runat="server" PopupControlID="dvClientMstAudiTrail" BehaviorID="MPE_PopulationMstHistory"
                PopupDragHandleControlID="LbldRandomizationDtl" BackgroundCssClass="modalBackground" CancelControlID="imgPopulationAuditTrail"
                TargetControlID="btn3">
            </cc1:ModalPopupExtender>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnCancel" />
            <asp:PostBackTrigger ControlID="btnDivExp" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:HiddenField runat="server" ID="hdnPopulationId" />
    <asp:Button ID="btnExportToExcel" runat="server" Style="display: none;" />
    

    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    
    <script type="text/javascript" language="javascript">

        function HidePopulationDetails() {
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
            //alert(msg);
            //window.location.href = "frmSubjectPopulationMst.aspx?mode=1";
            alertdooperation(msg, 1, "frmSubjectPopulationMst.aspx?mode=1");
        }
        function ValidateClient() {
            if (document.getElementById('<%=txtPopulationName.ClientID%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=txtPopulationName.ClientID%>').value = '';
                document.getElementById('<%=txtPopulationName.ClientID%>').focus();
                msgalert('Please Enter Population Name !');
                return false;
            }
            if (document.getElementById("<%=btnSave.ClientID%>").value.trim() == "Update") {
                if (document.getElementById("<%=txtremark.ClientID%>").value.trim() == "") {
                    msgalert("Please Enter Remarks !");
                    return false;
                }
            }
            return true;
        }

        function UIgvPopulation() {
            $('#<%= gvPopulation.ClientID%>').removeAttr('style', 'display:block');
            oTab = $('#<%= gvPopulation.ClientID%>').prepend($('<thead>').append($('#<%= gvPopulation.ClientID%> tr:first'))).dataTable({
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
        function ResetControl() {
            document.getElementById("ctl00_CPHLAMBDA_txtClientName").value = "";
            document.getElementById("ctl00_CPHLAMBDA_txtremark").value = "";
            document.getElementById("ctl00_CPHLAMBDA_ddlProjectMngr").selectedIndex = "0";

            return false;

        }
        function CheckTextLength(text, long) {
            var maxlength = new Number(long); 
            if (text.value.length > maxlength) {
                text.value = text.value.substring(0, maxlength);
                msgalert(" Only " + long + " characters allowed");
            }
        }
        function AudtiTrail(e) {
            var nPopulationId = $("#" + e.id).attr("nPopulationId");

            if (nPopulationId != "") {
                $.ajax({
                    type: "post",
                    url: "frmSubjectPopulationMst.aspx/AuditTrail",
                    data: '{"nPopulationId":"' + nPopulationId + '"}',
                    contentType: "application/json; charset=utf-8",
                    datatype: JSON,
                    async: false,
                    success: function (data) {
                        $('#tblPopulationMstAudit').attr("IsTable", "has");
                        var aaDataSet = [];
                        var range = null;

                        if (data.d != "" && data.d != null) {
                            data = JSON.parse(data.d);
                            for (var Row = 0; Row < data.length; Row++) {
                                var InDataSet = [];
                                InDataSet.push(data[Row].SrNo, data[Row].PopulationName, data[Row].Active, data[Row].Remarks, data[Row].ModifyBy, data[Row].ModifyOn);
                                aaDataSet.push(InDataSet);
                            }

                        }
                        if ($("#tblPopulationMstAudit").children().length > 0) {
                            $("#tblPopulationMstAudit").dataTable().fnDestroy();
                        }
                        oTable = $('#tblPopulationMstAudit').prepend($('<thead>').append($('#tblPopulationMstAudit tr:first'))).dataTable({
                            "bJQueryUI": true,
                            "sPaginationType": "full_numbers",
                            "bLengthChange": true,
                            "iDisplayLength": 10,
                            "bProcessing": true,
                            "bSort": false,
                            "aaData": aaDataSet,
                            "aoColumns": [
                                {
                                    "sTitle": "Sr. No",
                                },
                                { "sTitle": "PopulationName" },
                                 { "sTitle": "Active" },
                                { "sTitle": "Remarks" },
                                { "sTitle": "Modify By" },
                                { "sTitle": "Modify On" },

                            ],
                            "aoColumnDefs": [
                                        { 'bSortable': false, 'aTargets': [0] }
                            ],
                            "oLanguage": {
                                "sEmptyTable": "No Record Found",
                            },
                            aLengthMenu: [
                                [10, 25, 50, 100, -1],
                                [10, 25, 50, 100, "All"]
                            ],

                        });
                        oTable.fnAdjustColumnSizing();
                        $('.DataTables_sort_wrapper').click;
                        $find('MPE_PopulationMstHistory').show();
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
            $("#<%= hdnPopulationId.ClientID%>").val(id);
            var btn = document.getElementById('<%= btnExportToExcel.ClientId()%>')
            btn.click();
        }

    </script>

</asp:Content>