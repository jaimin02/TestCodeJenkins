<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmAddLocationMst, App_Web_l40sj1d0" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/scrollablegrid.js"></script>
    <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }
        /*#ctl00_CPHLAMBDA_gvlocation_wrapper {
            margin: 0px 235px;
        }*/
    </style>
    
            <table cellpadding="5px" style="width: 100%;">
                <tr>
                    <td>
                        <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                            <legend class="LegendText" style="color: Black; font-size: 12px">
                                <img id="img2" alt="Location Details" src="images/panelcollapse.png"
                                    onclick="Display(this,'divLocationDetail');" runat="server" style="margin-right: 2px;" />Location Details</legend>
                            <div id="divLocationDetail">
                                <table width="98%">
                                    <tr>
                                        <td class="Label" valign="top" style="width: 28%; white-space: nowrap; text-align: right;"
                                            nowrap="noWrap">Location Name* :
                                        </td>
                                        <td style="text-align: left; width: 25%; vertical-align: top;" class="Label" nowrap="noWrap">
                                            <asp:TextBox ID="txtlocationname" runat="server" CssClass="textBox" TabIndex="0"
                                                Width="70%" MaxLength="50" />
                                            <asp:Label ID="LblErrorLocation" runat="server" Text="" CssClass="ErrorCode " Font-Bold="true" Width="5%" />
                                        </td>
                                        <td class="Label" style="white-space: nowrap; text-align: right; width: 12%;" valign="top">Initial* :
                                        </td>
                                        <td style="text-align: left; vertical-align: top;" class="Label" nowrap="noWrap">
                                            <asp:TextBox ID="txtInitial" runat="server" CssClass="textBox" TabIndex="1"
                                                onblur="return CheckLocationInitial();" MaxLength="2" Width="45%" />
                                            <asp:Label ID="LblErrorInitial" runat="server" Text="" CssClass="ErrorCode " Font-Bold="true" Width="5%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Label" valign="top" style="width: 28%; text-align: right;">Country Code* :
                                        </td>
                                        <td style="text-align: left; vertical-align: top; width: 25%;" class="Label" nowrap="noWrap">
                                            <asp:TextBox ID="txtcountrycode" Width="70%" runat="server" CssClass="textBox" MaxLength="3" />
                                        </td>
                                        <td class="Label" valign="top" style="text-align: right; width: 12%;">Type :
                                        </td>
                                        <td style="text-align: left; vertical-align: top;" class="Label" nowrap="noWrap">
                                            <asp:RadioButton runat="server" ID="RbIsLocation" Checked="true" Text="Location"
                                                GroupName="GrpLocation" />
                                            <asp:RadioButton runat="server" ID="RbIsNotLocation" Text="Site" GroupName="GrpLocation" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Label" style="text-align: right; width: 28%;">Select TimeZone* :
                                        </td>
                                        <td style="text-align: left; width: 25%;">
                                            <asp:DropDownList ID="ddlTimezone" CssClass="dropDownList" Width="71%" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="Label" style="text-align: right; width: 12%;">Remarks* :
                                        </td>
                                        <td style="text-align: left">
                                            <asp:TextBox ID="txtRemark" runat="server" Rows="3" Columns="20" CssClass="textBox" Style="height: auto; width: 45%;" TextMode="MultiLine" MaxLength="500" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Label" nowrap="nowrap" colspan="4" style="text-align: center; vertical-align: top;">
                                            <asp:Button ID="btnSave" OnClientClick="return Validation();" runat="server" CssClass="btn btnsave"
                                                Text="Save" TabIndex="3" ToolTip="Save" />
                                            <asp:Button ID="btnExportToExcelGrid" runat="Server" Font-Size="Smaller" CssClass="btn btnexcel"  ToolTip="Export To Excel" />
                                            <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="btn btncancel"
                                                OnClick="btnCancel_Click" Text="Cancel" TabIndex="4" ToolTip="Cancel" />
                                            <asp:Button ID="btnclose" runat="server" CausesValidation="False" CssClass="btn btnclose"
                                                Text="Exit" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);"
                                                TabIndex="5" ToolTip="Exit" />
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
                                    <img id="img1" alt="Location Data" src="images/panelcollapse.png"
                                        onclick="Display(this,'divLocationData');" runat="server" style="margin-right: 2px;" />Location Data</legend>
                                <div id="divLocationData">
                                    <table style="margin: auto; width: 80%;">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvlocation" TabIndex="6" runat="server" Style="display: none; width: 60%; margin: auto;" OnPageIndexChanging="gvlocation_PageIndexChanging"
                                                    AutoGenerateColumns="False" OnRowCommand="gvlocation_RowCommand"
                                                    OnRowDataBound="gvlocation_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="#">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vLocationName" HeaderText="Location Name" />
                                                        <asp:BoundField DataField="vLocationInitiate" HeaderText="Initial" />
                                                        <asp:BoundField DataField="cLocationType" HeaderText="Type" />
                                                        <asp:BoundField DataField="vCountryCode" HeaderText="Country Code" />
                                                        <asp:BoundField DataField="vTimeZoneName" HeaderText="TimeZone Name" />
                                                        <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd-MMM-yyyy}" DataField="dModifyOn"
                                                            Visible="False" HeaderText="Modify On" />
                                                        <asp:TemplateField SortExpression="status" HeaderText="Edit">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="lnkEdit" runat="server" ImageUrl="~/images/Edit2.gif" ToolTip="Edit" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="vLocationCode" HeaderText="LocationCode" />
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
                                                                    <img src="images/Export.gif" onclick="ExportToExcel(this.id);" id='<%# Eval("vLocationCode")%>' />
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
            <%--<asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnclose" EventName="Click" />--%>
        </Triggers>
    </asp:UpdatePanel>
    <div>
        <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpLocationMstAuditTrail" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div id="dvLocationMstAuditTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
                                <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                    <tr>
                                        <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 97%;">
                                            <asp:Label ID="lblRamge" runat="server" Text="Audit Trail Information"></asp:Label>
                                        </td>
                                        <td style="width: 3%">
                                            <img id="imgLocationMstAuditTrail" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
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
                                                        <table id="tblLocationMstAudit" class="tblAudit" width="100%" style="background-color: aliceblue;"></table>
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
    <cc1:ModalPopupExtender ID="MPE_LocationMstHistory" runat="server" PopupControlID="dvLocationMstAuditTrail" BehaviorID="MPE_LocationMstHistory"
        PopupDragHandleControlID="LbldRandomizationDtl" BackgroundCssClass="modalBackground" CancelControlID="imgLocationMstAuditTrail"
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
    <asp:HiddenField runat="server" ID="hdnLocationCode" />
    <asp:Button ID="btnExportToExcel" runat="server" Style="display: none;" />
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <script type="text/javascript" language="javascript">

        function HideLocationDetails() {
            $('#<%= img2.ClientID%>').click();
        }

        function Display(control, target) {
            debugger;
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
            var vLocationCode = $("#" + e.id).attr("vLocationCode");

            if (vLocationCode != "") {
                $.ajax({
                    type: "post",
                    url: "frmLocationMst.aspx/AuditTrail",
                    data: '{"vLocationCode":"' + vLocationCode + '"}',
                    contentType: "application/json; charset=utf-8",
                    datatype: JSON,
                    async: false,
                    success: function (data) {
                        $('#tblLocationMstAudit').attr("IsTable", "has");
                        var aaDataSet = [];
                        var range = null;

                        if (data.d != "" && data.d != null) {
                            data = JSON.parse(data.d);
                            for (var Row = 0; Row < data.length; Row++) {
                                var InDataSet = [];
                                InDataSet.push(data[Row].SrNo, data[Row].LocationName, data[Row].LocationInitiate, data[Row].LocationType,
                                               data[Row].vCountryCode, data[Row].TimeZoneName, data[Row].Remark, data[Row].ModifyBy, data[Row].ModifyOn);
                                aaDataSet.push(InDataSet);
                            }
                        }
                        if ($("#tblLocationMstAudit").children().length > 0) {
                            $("#tblLocationMstAudit").dataTable().fnDestroy();
                        }
                        oTable = $('#tblLocationMstAudit').prepend($('<thead>').append($('#tblLocationMstAudit tr:first'))).dataTable({

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
                                   { "sTitle": "Location Name" },
                                   { "sTitle": "Initial" },
                                   { "sTitle": "Type" },
                                   { "sTitle": "Country Code" },
                                   { "sTitle": "Time Zone Name" },
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
                        $find('MPE_LocationMstHistory').show();
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
            $("#<%= hdnLocationCode.ClientID()%>").val(id);
            var btn = document.getElementById('<%= btnExportToExcel.ClientId()%>')
            btn.click();
        }
        function UIgvlocation() {
            $('#<%= gvlocation.ClientID%>').removeAttr('style', 'display:block');
            oTab = $('#<%= gvlocation.ClientID%>').prepend($('<thead>').append($('#<%= gvlocation.ClientID%> tr:first'))).dataTable({
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

            if (document.getElementById('<%=txtlocationname.ClientID%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=txtlocationname.ClientID%>').value = '';
                msgalert('Please Enter Location Name !');
                document.getElementById('<%=txtlocationname.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtInitial.ClientID%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=txtInitial.ClientID%>').value = '';
                msgalert('Please Enter Initial !');
                document.getElementById('<%=txtInitial.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=ddlTimezone.clientid %>').selectedIndex == 0) {
                msgalert('Please Select TimeZone !');
                return false;
            }


            else if (document.getElementById('<%=txtcountrycode.ClientID%>').value.toString().trim().length > 3 ||
          document.getElementById('<%=txtcountrycode.ClientID%>').value.toString().trim().length < 3) {


                document.getElementById('<%=txtcountrycode.ClientID%>').value = '';

                msgalert('CountryCode must be of 3 characters !');
                document.getElementById('<%=txtcountrycode.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtInitial.ClientID%>').value.toString().trim().length > 2 ||
            document.getElementById('<%=txtInitial.ClientID%>').value.toString().trim().length < 2) {
                document.getElementById('<%=txtInitial.ClientID%>').value = '';
                msgalert('Initial must be of 2 characters !');
                document.getElementById('<%=txtInitial.ClientID%>').focus();
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

function setinitial() {
    var location = document.getElementById('<%= txtlocationname.clientid %>').value.toString().trim();
    var initial;
    initial = location.toString().substring(0, 2).toUpperCase();
    document.getElementById('<%= txtinitial.clientid %>').value = initial;
            return;
        }

        function CheckLocationInitial() {
            document.getElementById('<%= LblErrorLocation.clientId %>').innerHTML = "";
            document.getElementById('<%= LblErrorInitial.clientId %>').innerHTML = "";
            setinitial();
            document.getElementById("txtcountrycode.ClientId").focus();
            var Choice = document.getElementById("Choice").value;
            var LocationCode = 0;
            var Initial = 0;
            var location = 0;

            if (document.getElementById("vLocationCode") != null) {
                LocationCode = document.getElementById("vLocationCode").value;
            }
            if (document.getElementById('<%= txtInitial.clientId %>') != null) {
                Initial = document.getElementById('<%= txtInitial.clientId %>').value;
            }
            if (document.getElementById('<%= txtlocationname.clientId %>') != null) {
                location = document.getElementById('<%= txtlocationname.clientId %>').value;
            }
            PageMethods.CheckDuplicateLocationInitial
    (
        Choice,
        Initial,
        location,
        LocationCode,

        function (Result) {

            if (Result.length > 0) {
                if (Result == "Location") {
                    document.getElementById('<%= LblErrorLocation.clientId %>').innerHTML = "The Location name already exists.";
                    document.getElementById('<%= txtlocationname.clientId %>').focus()
                    return false;
                }
                else if (Result == "Initial") {
                    document.getElementById('<%= LblErrorInitial.clientId %>').innerHTML = "The Initial already exists.";
                    document.getElementById('<%= txtInitial.clientId %>').focus()
                    return false;
                }
        }
            if (document.getElementById('<%= txtInitial.clientId %>').value == '' && document.getElementById('<%= txtlocationname.clientId %>').value != '') {
                setinitial();
            }
        },
        function (eerror) {
            msgalert(eerror);
        }
    );

            return true;
        }

        function ShowAlert(msg) {
            //alert(msg);
            //window.location.href = "frmLocationMst.aspx?mode=1";
            alertdooperation(msg, 1, "frmLocationMst.aspx?mode=1");
        }
    </script>

</asp:Content>
