<%@ page title="" language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmServiceMst, App_Web_5xoe1jh1" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

  

    <style type="text/css">
        .hide_column {
            display: none;
        }

        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

        .table th {
            border: 1px solid black;
            border-collapse: collapse;
        }
    </style>

    <table style="width: 100%;" cellpadding="5px">
        <tr>
            <td class="Label" style="width: 40%; text-align: right;">Service Name*:
            </td>
            <td style="text-align: left;">
                <asp:TextBox ID="txtServiceName" runat="server" CssClass="textBox" Width="40%" MaxLength="50" />
            </td>
        </tr>
        <tr>
            <td style="text-align: right;" class="Label">Service Remarks :
            </td>
            <td style="text-align: left;">
                <asp:TextBox ID="txtremarks" runat="server" CssClass="textBox" Width="40%" MaxLength="50" TextMode="MultiLine" />
            </td>
        </tr>
        <tr>
            <td style="text-align: center;" colspan="2">
                <asp:Button ID="BtnSave" runat="server" CssClass="btn btnsave" Text="Save" ToolTip="Save" OnClientClick="SaveData(); return false;" />
                <asp:Button ID="BtnCancel" runat="server" CssClass="btn btncancel" Text="Cancel" ToolTip="Cancel" OnClientClick="ClearField(); return false; " />
                <asp:Button ID="BtnExit" runat="server" CssClass="btn btnexit" Text="Exit" ToolTip="Exit"
                    OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" OnClick="BtnExit_Click" />
            </td>
        </tr>
    </table>
    <div id="divServiceView" style="margin: 2% 12%">
        <table id="tblServiceView" class="tblAudit" width="100%" style="background-color: #B0AFAF;"></table>
    </div>

    <button id="btnaudit" runat="server" style="display: none;" />
    <cc1:ModalPopupExtender ID="MPE_ServiceAuditTrial" runat="server" PopupControlID="dvServiceAudiTrail" BehaviorID="MPE_ServiceAuditTrial"
        PopupDragHandleControlID="LbldServiceDtl" BackgroundCssClass="modalBackground" CancelControlID="imgServiceAuditTrail"
        TargetControlID="btnaudit">
    </cc1:ModalPopupExtender>

    <div>
        <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpServiceAuditTrail" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div id="dvServiceAudiTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
                                <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                    <tr>
                                        <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 12px !important; width: 97%;">
                                            <asp:Label ID="lblRamge" runat="server" Text="Audit Trail"></asp:Label>
                                        </td>
                                        <td style="width: 3%">
                                            <img id="imgServiceAuditTrail" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
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
                                                        <table id="tblAuditTrail" class="tblAudit" width="100%" style="background-color: aliceblue;"></table>
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

    <asp:HiddenField ID="hdnServiceNo" runat="server" Value=""></asp:HiddenField>


    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
 
    

    <script type="text/javascript">

        window.onload = function () {
           
        }
   
        function pageLoad() {
            GetServiceData();
            $("#tblServiceView").DataTable();

           
        }

        function ClearField() {
            document.getElementById('<%=txtServiceName.ClientID%>').value = '';
            document.getElementById('<%=txtremarks.clientid%>').value = '';
            document.getElementById('<%=txtServiceName.ClientID%>').value = "";
            document.getElementById('<%=hdnServiceNo.ClientID%>').value = "";
            $("#ctl00_CPHLAMBDA_BtnSave").attr('value', 'Save');
            $("#ctl00_CPHLAMBDA_BtnSave").attr('title', 'Save');

            return false;
        }

        function SaveData() {
            var id = '<%= Me.Session(S_UserID)%>';
            var ServiceCode = document.getElementById('<%=hdnServiceNo.ClientID%>').value
            var Name = document.getElementById('<%=txtServiceName.ClientID%>').value
            var Remark = document.getElementById('<%=txtremarks.ClientID%>').value
            if (Name == "") {
                msgalert("Please Enter Service Name !");
                return false;
            }
            if (ServiceCode != "" && Remark == "") {
                msgalert("Please Enter Remark !");
                return false;
            }
            $.ajax({
                type: "POST",
                url: "frmservicemst.aspx/InsertData",
                data: '{"ID":"' + id + '", "ServiceName":"' + Name + '", "Remark":"' + Remark + '" , "ServiceCode":"' + ServiceCode + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d == "exist") {
                        msgalert("All ready Exist  Service Name !");
                        return false;
                    }
                    if (data.d == "add") {
                        msgalert("Service has been added successfully !");
                        document.getElementById('<%=txtServiceName.ClientID%>').value = "";
                        document.getElementById('<%=txtremarks.ClientID%>').value = "";
                        document.getElementById('<%=hdnServiceNo.ClientID%>').value = "";
                        GetServiceData();
                        return true;
                    }
                    if (data.d == "update") {
                        msgalert("Service has been updated successfully !");
                        document.getElementById('<%=txtServiceName.ClientID%>').value = "";
                        document.getElementById('<%=txtremarks.ClientID%>').value = "";
                        document.getElementById('<%=hdnServiceNo.ClientID%>').value = "";
                        $("#ctl00_CPHLAMBDA_BtnSave").attr('value', 'Save');
                        $("#ctl00_CPHLAMBDA_BtnSave").attr('title', 'Save');
                        GetServiceData();
                        return true;
                    }
                }
            });
            return false;
        }

        function GetServiceData() {

            var TotalActivity;
            var id = '<%= Me.Session(S_UserID)%>';
            $.ajax({
                type: "post",
                url: "frmservicemst.aspx/GetServiceData",
                data: '{"ID":"' + id + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                dataType: "json",
                success: function (data) {
                    $('#tblServiceView').attr("IsTable", "has");
                    var ActivityDataset = [];

                    if (data.d != "" && data.d != null) {
                        if (data.d.length > 2) {

                        }
                        data = JSON.parse(data.d)
                        TotalActivity = data.length;

                        for (var Row = 0; Row < data.length; Row++) {
                            var InDataset = [];
                            InDataset.push(data[Row].SrNo, data[Row].ServiceCode, data[Row].ServiceName, data[Row].Remark, data[Row].Edit, data[Row].Audit);
                            ActivityDataset.push(InDataset);
                        }

                        otable = $('#tblServiceView').dataTable({

                            "bJQueryUI": true,
                            "sPaginationType": "full_numbers",
                            "bLengthChange": true,
                            "iDisplayLength": 10,
                            "bProcessing": true,
                            "bSort": false,
                            "autoWidth": true,
                            "aaData": ActivityDataset,
                            "bInfo": true,
                            "bDestroy": true,

                            "fnCreatedRow": function (nRow, aData, iDataIndex) {
                                $('td:eq(4)', nRow).append("<input type='image' id='imgEdit_" + iDataIndex + "' name='imgEdit$" + iDataIndex + "' src='images/Edit2.gif' OnClick='return Edit(this); return false;' style='border-width:0px;'  ServiceCode='" + aData[1] + "', vServiceName='" + aData[2] + "' , vRemark='" + aData[3] + "'  >");
                                $('td:eq(5)', nRow).append("<input type='image' id='imgAudit_" + iDataIndex + "' name='imgAudit$" + iDataIndex + "' src='Images/audit.png' OnClick='return AuditTrial(this); return false;' style='border-width:0px;'  ServiceCode='" + aData[1] + "', vServiceName='" + aData[2] + "' , vRemark='" + aData[3] + "'  >");

                            },

                            "aoColumns": [
                                        { "sTitle": "#" },
                                        {
                                            "sTitle": "Service No",
                                            "sClass": "hide_column"
                                        },
                                        { "sTitle": "Service Name" },
                                        { "sTitle": "Remark" },
                                        { "sTitle": "Edit" },
                                        { "sTitle": "Audit Trail" },

                            ],
                            "oLanguage": {
                                "sEmptyTable": "No Record Found",
                            },

                            "columnDefs": [
                                       { "width": "1%", "targets": 1 },
                                        { "width": "99%", "targets": 2 }
                            ]

                        });
                    }
                    return false;
                },
                failure: function (response) {
                    msgalert(response.d);
                },
                error: function (response) {
                    msgalert(response.d);
                }
            });
            //oTable.fnAdjustColumnSizing();
            return false;
        }

        function Edit(e) {
            var serviceno = $("#" + e.id).attr("ServiceCode");
            document.getElementById('<%= hdnServiceNo.ClientID%>').value = serviceno;
            document.getElementById('<%=txtServiceName.ClientID%>').value = $("#" + e.id).attr("vservicename");
            // document.getElementById('<%=txtremarks.ClientID%>').value = $("#" + e.id).attr("vRemark");
            document.getElementById('<%=BtnSave.ClientID%>')
            $("#ctl00_CPHLAMBDA_BtnSave").attr('value', 'Update');
            $("#ctl00_CPHLAMBDA_BtnSave").attr('title', 'Update');
            return false;
        }

        function AuditTrial(e) {
            var vServiceCode = $("#" + e.id).attr("ServiceCode");
            if (vServiceCode != "") {
                $.ajax({
                    type: "post",
                    url: "frmservicemst.aspx/AuditTrail",
                    data: '{"vServiceCode":"' + vServiceCode + '"}',
                    contentType: "application/json; charset=utf-8",
                    datatype: JSON,
                    async: false,
                    success: function (data) {
                        $('#tblAuditTrail').attr("IsTable", "has");
                        var aaDataSet = [];
                        var range = null;

                        if (data.d != "" && data.d != null) {
                            data = JSON.parse(data.d);
                            for (var Row = 0; Row < data.length; Row++) {
                                var InDataSet = [];
                                InDataSet.push(data[Row].SrNo, data[Row].ServiceName, data[Row].Remarks, data[Row].ModifyBy, data[Row].ModifyOn);
                                aaDataSet.push(InDataSet);
                            }

                        }
                        if ($("#tblAuditTrail").children().length > 0) {
                            $("#tblAuditTrail").dataTable().fnDestroy();
                        }
                        oTable = $('#tblAuditTrail').prepend($('<thead>').append($('#tblAuditTrail tr:first'))).dataTable({

                            "bJQueryUI": true,
                            "sPaginationType": "full_numbers",
                            "bLengthChange": true,
                            "iDisplayLength": 10,
                            "bProcessing": true,
                            "bSort": false,

                            "aaData": aaDataSet,
                            "aoColumns": [
                                {
                                    "sTitle": "#",
                                },
                                { "sTitle": "Service Name" },
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
                        $find('MPE_ServiceAuditTrial').show();

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
    </script>


</asp:Content>

