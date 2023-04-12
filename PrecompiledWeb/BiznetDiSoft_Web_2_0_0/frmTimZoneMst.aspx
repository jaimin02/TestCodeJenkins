<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmTimZoneMst, App_Web_xjkmyygy" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <style type="text/css">
        .ui-timepicker-wrapper {
            overflow-y: auto;
            height: 150px;
            width: 6.5em;
            background: #fff;
            border: 1px solid #ddd;
            -webkit-box-shadow: 0 5px 10px rgba(0,0,0,0.2);
            -moz-box-shadow: 0 5px 10px rgba(0,0,0,0.2);
            box-shadow: 0 5px 10px rgba(0,0,0,0.2);
            outline: none;
            z-index: 10001;
            margin: 0;
        }

        .ui-dialog .ui-dialog-content {
            width: auto;
            min-height: 50px;
            max-height: 155px;
            overflow: scroll;
            text-align: left;
        }

        .ui-timepicker-wrapper.ui-timepicker-with-duration {
            width: 11em;
        }

        .ui-timepicker-list {
            margin: 0;
            padding: 0;
            list-style: none;
        }

        .ui-timepicker-duration {
            margin-left: 5px;
            color: #888;
        }

        .ui-timepicker-list:hover .ui-timepicker-duration {
            color: #888;
        }

        .ui-timepicker-list li {
            padding: 3px 0 3px 5px;
            cursor: pointer;
            white-space: nowrap;
            color: #000;
            list-style: none;
            margin: 0;
            font-size: 15px;
        }

        .ui-timepicker-list:hover .ui-timepicker-selected {
            background: #fff;
            color: #000;
        }

        li.ui-timepicker-selected, .ui-timepicker-list li:hover, .ui-timepicker-list .ui-timepicker-selected:hover {
            background: #1980EC;
            color: #fff;
        }

            li.ui-timepicker-selected .ui-timepicker-duration, .ui-timepicker-list li:hover .ui-timepicker-duration {
                color: #ccc;
            }

        .ui-timepicker-list li.ui-timepicker-disabled, .ui-timepicker-list li.ui-timepicker-disabled:hover, .ui-timepicker-list li.ui-timepicker-selected.ui-timepicker-disabled {
            color: #888;
            cursor: default;
        }

            .ui-timepicker-list li.ui-timepicker-disabled:hover, .ui-timepicker-list li.ui-timepicker-selected.ui-timepicker-disabled {
                background: #f2f2f2;
            }

        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

        /*#ctl00_CPHLAMBDA_GV_UserType_wrapper {
            margin: 0px 235px;
        }*/

        .ajax__calendar_container {
            z-index: 1;
        }
    </style>

    <script type="text/javascript" src="script/jquery-1.7.min.js"></script>

    <script src="Script/jquery.timepicker.js" type="text/javascript"></script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellpadding="5px" style="width: 100%;">
                <tr>
                    <td>
                        <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                            <legend class="LegendText" style="color: Black; font-size: 12px">
                                <img id="img2" alt="Time Zone Details" src="images/panelcollapse.png"
                                    onclick="Display(this,'divTimeZoneDetail');" runat="server" style="margin-right: 2px;" />Time Zone Details</legend>
                            <div id="divTimeZoneDetail">
                                <table width="98%">
                <tr>
                    <td class="Label" style="text-align: right; vertical-align: middle; width: 26%;">Select TimeZone* :
                    </td>
                    <td style="text-align: left; width: 25%;">
                        <asp:DropDownList ID="ddlTimezone" CssClass="dropDownList" runat="server" Width="85%">
                        </asp:DropDownList>
                    </td>
                    <td class="Label" style="text-align: right; vertical-align: middle; width: 20%;">Select Location* :
                    </td>
                    <td style="text-align: left; width: 29%;">
                        <asp:DropDownList ID="ddlLocation" CssClass="dropDownList" runat="server" Width="78%">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="Label" style="text-align: right; vertical-align: middle; width: 26%;">Effective Timezone Start From* :
                    </td>
                    <td style="text-align: left; width: 25%;">
                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="textBox" Style="width: 43%;"
                            onKeyDown="return Validate(event);" />
                        <cc1:CalendarExtender ID="calanderToDate" TargetControlID="txtStartDate" runat="server"
                            Format="dd-MMM-yyyy" Animated="true">
                        </cc1:CalendarExtender>&nbsp;
                        <asp:TextBox ID="txtStarttime" runat="server" CssClass="textBox timepicker" Style="width: 25%;"
                            onKeyDown="return Validate(event);" />
                        <span class="Label">(EST) </span>
                    </td>

                    <td class="Label" style="text-align: right; vertical-align: middle; width: 20%;">
                        <span class="Label">To* : </span>
                    </td>

                    <td style="text-align: left; width: 29%;">
                         <asp:TextBox ID="txtEndDate" runat="server" CssClass="textBox" Style="width: 43%;"
                            onKeyDown="return Validate(event);" />
                        <cc1:CalendarExtender ID="calanderFromDate" TargetControlID="txtEndDate" runat="server"
                            Format="dd-MMM-yyyy">
                        </cc1:CalendarExtender>&nbsp;
                        <asp:TextBox ID="txtEndTime" runat="server" CssClass="textBox timepicker" Style="width: 20%;"
                            onKeyDown="return Validate(event);" />
                        <span class="Label">(EST)</span>
                    </td>
                </tr>
                <tr>
                    <td nowrap="nowrap" colspan="4" style="text-align: center; vertical-align: top;">
                        <asp:Button ID="btnSave" OnClientClick="return Validation();" runat="server" CssClass="btn btnsave"
                            Text="Save" ToolTip="Save" />
                        <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="btn btncancel"
                            Text="Cancel" ToolTip="Cancel" />
                        <asp:Button ID="btnclose" runat="server" CausesValidation="False" CssClass="btn btnclose"
                            Text="Exit" OnClientClick="return msgconfirmalert('Are You Sure You Want To Exit?',this);"
                            ToolTip="Exit" />
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
                                    <img id="img1" alt="Time Zone Data" src="images/panelcollapse.png"
                                        onclick="Display(this,'divTimeZoneData');" runat="server" style="margin-right: 2px;" />Time Zone Data</legend>
                                <div id="divTimeZoneData">
                                    <table style="margin: auto; width: 80%;">
                                        <tr>
                                            <td style="display: none;" id="tdFieldSetBox">
                                                <fieldset class="FieldSetBox" style="width: 20%; float: right; position: relative;">
                                                    <legend class="LegendText" style="color: Black">Legend</legend>
                                                    <table class="LabelText">
                                                        <tbody>
                                                            <tr>
                                                                <td>
                                                                    <div style="width: 20px; height: 20px; float: left; background-color: rgb(255, 189, 121); font-weight: bold;">
                                                                    </div>
                                                                </td>
                                                                <td>Delete TimeZone
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:GridView ID="gvTimeZone" TabIndex="6" runat="server" AutoGenerateColumns="False" Style="width: 50%; display: none" >
                                                    <Columns>
                                                        <asp:BoundField HeaderText="#">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vTimeZoneName" HeaderText="TimeZone Name" />
                                                        <asp:BoundField DataField="vLocationName" HeaderText="Location" />
                                                        <asp:BoundField DataField="vTimeZoneOffset" HeaderText="Time Zone Offset" />
                                                        <asp:BoundField DataField="dDaylightStart" HeaderText="Effective Start Date(IST)" DataFormatString="{0:dd-MMM-yyyy HH:mm tt}" />
                                                        <asp:BoundField DataField="dDaylightEnd" HeaderText="Effective End Date(IST)" DataFormatString="{0:dd-MMM-yyyy HH:mm tt}" />
                                                        <asp:BoundField DataField="dDaylightStartEST" HeaderText="Effective Start Date(EST)" DataFormatString="{0:dd-MMM-yyyy HH:mm tt}" />
                                                        <asp:BoundField DataField="dDaylightEndEST" HeaderText="Effective End Date(EST)" DataFormatString="{0:dd-MMM-yyyy HH:mm tt}" />
                                                        <asp:BoundField DataField="nTimeZoneMstNo" HeaderText="TimeZoneMstNo" />
                                                        <asp:BoundField DataField="cStatusIndi" HeaderText="cStatusIndi">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField SortExpression="status" HeaderText="Delete">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="lnkDelete" runat="server" ImageUrl="~/images/i_delete.gif" ToolTip="Delete"
                                                                   OnClientClick='<%# Eval("nTimeZoneMstNo", "return ShowModalPopup({0})")%>' />
                                                                 <%--<img src="images/i_delete.gif" alt="Delete" onclick="ShowModalPopup(this.id);" id='<%#  Eval("nTimeZoneMstNo") %>' />--%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Audit Trail">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="lnkAudit" runat="server" ImageUrl="~/Images/audit.png" ToolTip="Audit Trial" OnClientClick="AuditTrail(this); return false;" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Export">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <img src="images/Export.gif" onclick="ExportToExcel(this.id);" id='<%#  Eval("nTimeZoneMstNo") %>' />
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
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnclose" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>


    <div>
        <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpTimeZoneAuditTrail" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div id="dvUomMstAuditTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
                                <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                    <tr>
                                        <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 97%;">
                                            <asp:Label ID="lblRamge" runat="server" Text="Audit Trail Information"></asp:Label>
                                        </td>
                                        <td style="width: 3%">
                                            <img id="imgtimeZoneAuditTrail" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
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
                                                        <table id="tblUomMstAudit" class="tblAudit" width="100%" style="background-color: aliceblue; text-align: left"></table>
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
    <cc1:ModalPopupExtender ID="MPE_TimeZoneMstHistory" runat="server" PopupControlID="dvUomMstAuditTrail" BehaviorID="MPE_TimeZoneMstHistory"
        PopupDragHandleControlID="LbldRandomizationDtl" BackgroundCssClass="modalBackground" CancelControlID="imgtimeZoneAuditTrail"
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
                    <%--<asp:Button ID="btnRemarksUpdate" runat="server" Text="Update" CssClass="ButtonText"
                        Width="64px" Style="font-size: 12px !important;" TabIndex="56" OnClientClick="return UpdateData();" />--%>
                    <asp:Button ID="btnRemarksUpdate" runat="server" Text="Save" CssClass="btn btnsave"
                        Style="font-size: 12px !important;" OnClick="btnRemarksUpdate_Click" OnClientClick="return ValidateInactiveRemarks();" />
                    <asp:Button ID="btnRemarksCancel" runat="server" Text="Cancel" CssClass="btn btncancel"
                         Style="font-size: 12px !important;" />

                </td>
            </tr>
        </table>
    </div>



    <asp:HiddenField runat="server" ID="hdnTimeZoneNo" />

    <asp:Button ID="btnExportToExcel" runat="server" Style="display: none;" CssClass="btn btnexcel" />

    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/scrollablegrid.js"></script>

    <script type="text/javascript" language="javascript">

        function HideTimeZoneDetails() {
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

        function UIgvTimeZone() {
            $('#<%= gvTimeZone.ClientID%>').removeAttr('style', 'display:block');
            $('#tdFieldSetBox').removeAttr('style', 'display:block');

            oTab = $('#<%= gvTimeZone.ClientID%>').prepend($('<thead>').append($('#<%= gvTimeZone.ClientID%> tr:first'))).dataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": 5,
                "bProcessing": true,
                "bSort": false,
                "sScrollY": "250px",
                "sScrollX": "100%",
                "bScrollCollapse": true,
                aLengthMenu: [
                    [5, 10, 25, 50, 100, -1],
                    [5, 10, 25, 50, 100, "All"]
                ],
            });
            return false;
        }
        function pageLoad() {
            $('.timepicker').timepicker({ 'scrollDefaultNow': true });
        }
        function ValidateInactiveRemarks() {
            if (document.getElementById("<%=txtRemarks_delete.ClientID%>").value.trim() == "") {
                msgalert("Please Enter Remarks !");
                return false;
            }
            return true;
        }
        function Validation() {
            if (document.getElementById('<%=ddlTimezone.clientid %>').selectedIndex == 0) {
                msgalert('Please Select TimeZone !');
                return false;
            }
            else if (document.getElementById('<%=ddlLocation.clientid %>').selectedIndex == 0) {
                msgalert('Please Select Country !');
                return false;
            }

            else if (document.getElementById('<%=txtStartDate.ClientId %>').value.toString().trim() == "") {
                msgalert('Please Enter Effective Start Date !');
                document.getElementById('<%=txtStartDate.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtStarttime.ClientId %>').value.toString().trim() == "") {
                msgalert('Please Enter Effective Start Time !');
                document.getElementById('<%=txtStarttime.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtEndDate.ClientId %>').value.toString().trim() == "") {
                msgalert('Please Enter Effective From Date !');
                document.getElementById('<%=txtEndDate.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtEndTime.ClientId %>').value.toString().trim() == "") {
                msgalert('Please Enter Effective From Date !');
                document.getElementById('<%=txtEndTime.ClientID%>').focus();
                return false;
            }
            else {
                var startDate = new Date(document.getElementById('<%=txtStartDate.ClientId %>').value.toString().trim());
                var fromDate = new Date(document.getElementById('<%=txtEndDate.ClientId %>').value.toString().trim());
                if (startDate > fromDate) {
                    msgalert("You can not enter from date greater than to date !");
                    return false;
                }
                if (startDate == fromDate) {
                    msgalert("You can not enter from date equal to date !");
                    return false;
                }
                return true;
            }

    return true;
}

function Validate(evt) {
    var browser = navigator.appName;
    var charCode = 0;
    if (browser == 'Microsoft Internet Explorer')
        charCode = evt.keyCode;
    else
        charCode = evt.which;


    if (evt.shiftKey == true)
    { return false; }

    if ((charCode == 190) || (charCode == 110) || (charCode == 20) || (charCode == 9) || (charCode == 46) || (charCode == 8))
    { return false; }

    if ((charCode < 48 || charCode > 57) && (charCode < 96 || charCode > 105))
    { return false; }

    return false;

}

function AuditTrail(e) {
    debugger;
    var nTimeZoneMstNo = e.attributes.nTimeZoneMstNo.value

    if (nTimeZoneMstNo != "") {
        $.ajax({
            type: "post",
            url: "frmTimZoneMst.aspx/AuditTrail",
            data: '{"nTimeZoneMstNo":"' + nTimeZoneMstNo + '"}',
            contentType: "application/json; charset=utf-8",
            datatype: JSON,
            async: false,
            success: function (data) {
                $('#tblUomMstAudit').attr("IsTable", "has");
                var aaDataSet = [];
                var range = null;

                data = JSON.parse(data.d);
                for (var Row = 0; Row < data.length; Row++) {
                    var InDataSet = [];
                    InDataSet.push(data[Row].SrNo, data[Row].TimeZoneName, data[Row].LocationName, data[Row].Remarks, data[Row].ModifyBy, data[Row].ModifyOn);

                    aaDataSet.push(InDataSet);
                }

                if ($("#tblUomMstAudit").children().length > 0) {
                    $("#tblUomMstAudit").dataTable().fnDestroy();
                }
                oTable = $('#tblUomMstAudit').prepend($('<thead>').append($('#tblUomMstAudit tr:first'))).dataTable({



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

                         { "sTitle": "TimeZoneName" },
                         { "sTitle": "LocationName" },
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
                $find('MPE_TimeZoneMstHistory').show();

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


function ShowModalPopup(id) {
    $("#<%= hdnTimeZoneNo.ClientID %>").val(id);
    msgConfirmDeleteAlert(null, "Are You Sure You Want To Delete TimeZone?", function (isConfirmed) {
        if (isConfirmed) {
            $('#<%= txtRemarks_delete.ClientID%>').val('');
            $find('mdlRemarks').show();
            return true;
        } else {
            return false;
        }
    });
    return false;
}

function ExportToExcel(id) {
    $("#<%= hdnTimeZoneNo.ClientID %>").val(id);
    var btn = document.getElementById('<%= btnExportToExcel.ClientId()%>')
    btn.click();
}


    </script>

</asp:Content>
