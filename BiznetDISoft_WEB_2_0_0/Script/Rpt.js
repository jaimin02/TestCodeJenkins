var theme = 'energyblue';

$(document).ready(function () {
    onUpdating();
    $(".dropdown").attr('class', 'dropdown jqx-combobox-state-normal jqx-combobox-state-normal-energyblue jqx-rc-all jqx-rc-all-energyblue jqx-widget jqx-widget-energyblue jqx-widget-content jqx-widget-content-energyblue');
    //datetime picker
    $('.datePicker').datepicker({ dateFormat: 'd-M-yy', showAnim: 'drop', changeMonth: true, changeYear: true, maxDate: new Date() });
    $(".datePicker").datepicker('setDate', (new Date()));
    $('#ui-datepicker-div').hide();
    //bind the report combobox
    var d = new Date();
    var report = '';
    var title = '';
    var year = d.getFullYear();
    var source = new Array();

    for (i = 0; i < 10; i++) {

        switch (i) {
            case 0:
                report = 'StockReport.png';
                title = 'Stock Report';
                id = 0;
                break;
            case 1:
                report = 'PurchaseReport.png';
                title = 'Purchase Report';
                id = 1;
                break;
            case 2:
                report = 'DispenseReport.png';
                title = 'Dispense Report';
                id = 2;
                break;
            case 3:
                report = 'CollectionReport.png';
                title = 'Collection Report';
                id = 3;
                break;
            case 4:
                report = 'StockLedger.png';
                title = 'Stock Ledger';
                id = 4;
                break;
            case 5:
                report = '3cReport.png';
                title = '3c Report';
                id = 5;
                break;
            case 6:
                report = 'ProcedureReport.png';
                title = 'Procedure Report';
                id = 6;
                break;
            case 7:
                report = 'PaymentReport.png';
                title = 'Payment Report';
                id = 7;
                break;
            case 8:
                report = 'payment_ledger.png';
                title = 'Payment Ledger';
                id = 8;
                break;
            case 9:
                report = 'collection_ledger.png';
                title = 'Collection Ledger';
                id = 9;
                break;
        }
        var html = "<div style='padding: 0px; margin: 0px; float: left;'><img style='float: left; margin-top: 4px; margin-right: 15px;' src='Images/" + report + "'/>"
                + "<div>" + title + "</div><div>" + year.toString() + "</div></div></div>";
        source[i] = { html: html, title: title, id: id };

    }
    // Create a ComboBox
    $("#ddlReports").jqxComboBox({ source: source, selectedIndex: 0, width: '200', height: '20px', theme: theme, displayMember: "title", valueMember: "id" });

    //Show hide controls depending on the report selection
    $("#ddlReports").bind('select', function (event) {
        var source = new Array();
        if (event.args) {
            $("#grid").html("");
            var item = event.args.item;

            switch (item.value) {
                case 0:
                    document.getElementById('StockReport').style.display = '';
                    document.getElementById('PurchaseReport').style.display = 'none';
                    document.getElementById('DispenseReport').style.display = 'none';
                    document.getElementById('Date').style.display = 'none';
                    document.getElementById('StockLedger').style.display = 'none';
                    break;
                case 1:
                    document.getElementById('PurchaseReport').style.display = '';
                    document.getElementById('Date').style.display = '';
                    document.getElementById('StockReport').style.display = 'none';
                    document.getElementById('DispenseReport').style.display = 'none';
                    document.getElementById('StockLedger').style.display = 'none';
                    $('#pddlProduct').show();
                    $('#pddlProduct').prev().show();
                    break;
                case 2:
                    BindDispensingReportdropdownList("'D'");
                    document.getElementById('DispenseReport').style.display = '';
                    document.getElementById('Date').style.display = '';
                    document.getElementById('StockReport').style.display = 'none';
                    document.getElementById('PurchaseReport').style.display = 'none';
                    document.getElementById('divProduct').style.display = '';
                    document.getElementById('divPatient').style.display = '';
                    document.getElementById('divCompany').style.display = '';
                    document.getElementById('divProcedure').style.display = 'none';
                    document.getElementById('divCPatient').style.display = 'none';
                    document.getElementById('StockLedger').style.display = 'none';
                    $("#dddlCollectedType").hide();
                    $("#dddlCollectedType").prev().hide();
                    $("#cddlClinic").hide();
                    $("#cddlClinic").prev().hide();
                    break;
                case 3: case 9:
                    //  BindDispensingReportdropdownList();
                    BindLocationDropDownList();
                    document.getElementById('DispenseReport').style.display = '';
                    document.getElementById('Date').style.display = '';
                    document.getElementById('divCPatient').style.display = '';
                    document.getElementById('divProcedure').style.display = 'none';
                    document.getElementById('StockReport').style.display = 'none';
                    document.getElementById('PurchaseReport').style.display = 'none';
                    document.getElementById('divCompany').style.display = 'none';
                    document.getElementById('divProduct').style.display = 'none';
                    document.getElementById('divPatient').style.display = 'none';
                    document.getElementById('StockLedger').style.display = 'none';
                    $("#dddlCollectedType").show();
                    $("#dddlCollectedType").prev().show();
                    $("#cddlClinic").show();
                    $("#cddlClinic").prev().show();
                    document.getElementById('divProcedure').style.display = 'none';
                    break;
                case 4:
                    document.getElementById('StockLedger').style.display = '';
                    document.getElementById('Date').style.display = '';
                    document.getElementById('StockReport').style.display = 'none';
                    document.getElementById('PurchaseReport').style.display = 'none';
                    document.getElementById('DispenseReport').style.display = 'none';
                    document.getElementById('divProcedure').style.display = 'none';
                    break;
                case 5:
                    BindDispensingReportdropdownList((item.value == 5) ? "'A'" + "," + "'D'" : "'A'");
                    document.getElementById('DispenseReport').style.display = '';
                    document.getElementById('Date').style.display = '';
                    document.getElementById('divProcedure').style.display = 'none';
                    document.getElementById('divPatient').style.display = 'none';
                    document.getElementById('divCompany').style.display = 'none';
                    document.getElementById('divProduct').style.display = 'none';
                    document.getElementById('divCPatient').style.display = 'none';
                    document.getElementById('StockReport').style.display = 'none';
                    document.getElementById('PurchaseReport').style.display = 'none';
                    document.getElementById('StockLedger').style.display = 'none';
                    // $("#dddlCollectedType").hide(); nidhi
                    // $("#dddlCollectedType").prev().hide(); nidhi
                    $("#dddlCollectedType").show();
                    $("#dddlCollectedType").prev().show();
                    $("#cddlClinic").hide();
                    $("#cddlClinic").prev().hide();
                    break;
                case 6:
                    BindDispensingReportdropdownList((item.value == 5) ? "'A'" + "," + "'D'" : "'A'");
                    document.getElementById('DispenseReport').style.display = '';
                    document.getElementById('Date').style.display = '';
                    document.getElementById('divProcedure').style.display = '';
                    document.getElementById('divPatient').style.display = '';
                    document.getElementById('divCompany').style.display = 'none';
                    document.getElementById('divProduct').style.display = 'none';
                    document.getElementById('divCPatient').style.display = 'none';
                    document.getElementById('StockReport').style.display = 'none';
                    document.getElementById('PurchaseReport').style.display = 'none';
                    document.getElementById('StockLedger').style.display = 'none';
                    $("#dddlCollectedType").hide();
                    $("#dddlCollectedType").prev().hide();
                    $("#cddlClinic").hide();
                    $("#cddlClinic").prev().hide();
                    break;
                case 7: case 8:
                    document.getElementById('StockReport').style.display = 'none';
                    document.getElementById('PurchaseReport').style.display = '';
                    document.getElementById('DispenseReport').style.display = 'none';
                    document.getElementById('StockLedger').style.display = 'none';
                    document.getElementById('Date').style.display = '';
                    document.getElementById('divProcedure').style.display = '';
                    $('#pddlProduct').hide();
                    $('#pddlProduct').prev().hide();
                    break;
            }
        }
    });

    // IN 3c report purpose dropdown will only displayed if collection type=Appointment
    $("#dddlCollectedType").live("select", function (event) {
        if ($("#ddlReports").jqxComboBox('selectedIndex') == 5) {
            if ($("#dddlCollectedType").jqxComboBox('selectedIndex') == 1)
                document.getElementById('divProcedure').style.display = '';
            else
                document.getElementById('divProcedure').style.display = 'none';
        }
    });

    //----------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------
    //Created Date : 21-May-2012
    //Created By : Bharat patel
    //Reason: For Stock Report                     

    BindStockReportDropDownList();

    $("#sddlCompany").live('select', function (event) {
        var source = new Array();
        if (event.args) {
            var item = event.args.item;
            if (item) {
                BindStockReportProductDropdownList(item.value);
            }
        }
    });

    //----------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------


    //Created Date : 23-May-2012
    //Created By : Bharat patel
    //Reason: For Purchase Report                     

    BindPurchaseReportdropdownList();

    $("#pddlCompany").live('select', function (event) {

        var source = new Array();
        if (event.args) {
            var item = event.args.item;
            if (item) {
                BindPurchasereportAgentDropDownList(item.value);
            }
        }
    });

    $("#pddlAgent").live('select', function (event) {

        var source = new Array();
        if (event.args) {
            var item = event.args.item;
            if (item) {
                var Companyid = $("#pddlCompany").jqxComboBox('getSelectedItem').value;
                BindPurchaseReportProductdropDownList(Companyid, item.value);
            }
        }
    });

    //Created Date : 24-May-2012
    //Created By : Bharat patel
    //Reason: For Dispensing Report                     

    //BindDispensingReportdropdownList();
    BindCollectedTypeDropDownList();
    BindDispensingReportComapnydropdownList();
    BindCollectionReportdropdownList();

    $("#dddlPatient").live('select', function (event) {
        var Reports = $("#ddlReports").jqxComboBox('getSelectedItem');
        //var TranType = (Reports.value == "2") ? "'D'" : "'A'" + ',' + "'D'";

        var source = new Array();
        if (event.args) {
            var item = event.args.item;
            if (item) {
                if (Reports.value == "2")
                    BindDispensingReportComapnydropdownList(item.value);
                else
                    BindDispensingReportProcedureDropDownList(item.value);
            }
        }
    });
    $("#dddlCompany").live('select', function (event) {
        var PatientId = $("#dddlPatient").jqxComboBox('getSelectedItem').value;
        var source = new Array();
        if (event.args) {
            var item = event.args.item;
            if (item) {
                BindDispensingReportProductDropDownList(PatientId, item.value, "'D'");
            }
        }
    });



    //Created Date : 26-May-2012
    //Created By : Bharat patel
    //Reason: For Stock Ledger Report                     

    BindStockLedgerReportdropdownList();

    $("#lddlCompany").live('select', function (event) {
        var Reports = $("#ddlReports").jqxComboBox('getSelectedItem');

        var source = new Array();
        if (event.args) {
            var item = event.args.item;
            if (item) {
                BindStockLedgerComapnyDropDownList(item.value);
            }
        }
    });

    //change fromDate and toDate on range selector
    $("#divDate").live('select', function (event) {
        var today = new Date();
        var source = new Array();
        if (event.args) {
            var item = event.args.item;
            if (item) {
                if (item.value == 1)
                    $(".datePicker").datepicker('setDate', (new Date()));
                else if (item.value == 2) {
                    today.setDate(today.getDate() - 7);
                    $("#txtFromDate").datepicker('setDate', (new Date(today)));
                    $("#txtToDate").datepicker('setDate', (new Date()));
                }
                else if (item.value == 3) {
                    today.setMonth(today.getMonth() - 1);
                    $("#txtFromDate").datepicker('setDate', (new Date(today)));
                    $("#txtToDate").datepicker('setDate', (new Date()));
                }
                else if (item.value == 4) {
                    today.setMonth(today.getMonth() - 3);
                    $("#txtFromDate").datepicker('setDate', (new Date(today)));
                    $("#txtToDate").datepicker('setDate', (new Date()));
                }
                else if (item.value == 5) {
                    today.setMonth(today.getMonth() - 6);
                    $("#txtFromDate").datepicker('setDate', (new Date(today)));
                    $("#txtToDate").datepicker('setDate', (new Date()));
                }
                else if (item.value == 6) {
                    today.setYear(today.getFullYear() - 1);
                    $("#txtFromDate").datepicker('setDate', (new Date(today)));
                    $("#txtToDate").datepicker('setDate', (new Date()));
                }

            }
        }
    });

});


//For displaying Grid
function DisplayGrid() {

    var ReportsId = $("#ddlReports").jqxComboBox('getSelectedItem');
    switch (ReportsId.value) {
        case 0:
            DisplayStockReportGrid();
            break;
        case 1: case 7: case 8:
            DisplayPurchaseReportGrid();
            break;
        case 2: case 3: case 5: case 6: case 9:
            DisplayDispensingReportGrid();
            break;
        case 4:
            DisplayStockLedgerReportGrid();
            break;

    }
}

//For Stock Report
function BindStockReportDropDownList(data) {
    var source = new Array();

    $.ajax({
        type: "POST",
        url: "WebService.asmx/GetCompanyForStock",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: "{'ReportsId':'0','PatientId':'-1'}",
        error: function (ex) {
            console.log(ex);
        },
        success: function (data) {
            var data = jQuery.parseJSON(data.d);
            if (data != null) {
                $("#sddlCompany").jqxComboBox({ source: data, selectedIndex: 0, width: '200', height: '20px', theme: theme, displayMember: "vCompanyName", valueMember: "nCompanyId" });
                BindStockReportProductDropdownList("-1");
            }
            else {
                $("#sddlCompany").jqxComboBox('clear');
                onUpdated();
            }
        }
    });

}
function BindStockReportProductDropdownList(Id) {
    $.ajax({
        type: "POST",
        url: "WebService.asmx/GetProductCompanyWiseForStock",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: '{"CompanyId":"' + Id + '"}',
        error: function (ex) {
            console.log(ex);
        },
        success: function (data) {
            var data = jQuery.parseJSON(data.d);
            if (data != null) {
                $("#sddlProduct").jqxComboBox({ source: data, selectedIndex: 0, width: '200', height: '20px', theme: theme, displayMember: "vProductName", valueMember: "nProductId" });
            }
        }
    });
}

function DisplayStockReportGrid() {
    var Companyid = $("#sddlCompany").jqxComboBox('getSelectedItem');
    var ProductId = $("#sddlProduct").jqxComboBox('getSelectedItem');
    Companyid = (Companyid == null && Companyid == undefined) ? "-1" : Companyid.value;
    ProductId = (ProductId == null && Companyid == undefined) ? "-1" : ProductId.value;
    GetView_ProductBatchWiseStock(Companyid, ProductId);
}

function GetView_ProductBatchWiseStock(CompanyId, ProductId) {
    var content = new Object();
    content.CompanyId = CompanyId;
    content.ProductId = ProductId;

    var jsontext = JSON.stringify(content);

    $.ajax({
        type: "POST",
        url: "WebService.asmx/GetView_AllProductBatchWiseStock",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: jsontext,
        error: function (ex) {
            console.log(ex);
        },
        success: function (data) {
            var data = jQuery.parseJSON(data.d);
            console.log(data);
            if (data != null)
                BindStockReportGrid(data);
            else
                $("#grid").html("<div style='width:100%;text-align:center;font-weight:bold;font-size:14px'>Data not found</div>");
        },
        failure: function (errMsg) {
            $('#errorMessage').text(errMsg);  //errorMessage is id of the div
        }
    });
}

function BindStockReportGrid(result) {
    var dtData = new Array();
    var TotalCurrentStock = 0;
    var TotalStockvalue = 0;
    var TotPurchaseAmount = 0;
    var TotLendingAmount = 0;

    $.each(result, function (index) {
        dtData.push([
                              this.vProductCompany,
                              this.vProductName,
                              this.vBatch,
                              this.vExpiryDate,
                              intToFormat(this.iPendingQuantity),
                              intToFormat(parseFloat(this.nSellRate).toFixed(2)),
                              intToFormat(parseFloat(this.iPendingQuantity * this.nSellRate).toFixed(2)),
                              intToFormat(parseFloat(this.nPurchaseSellRate).toFixed(2)),
                              intToFormat(parseFloat(this.iPendingQuantity * this.nPurchaseSellRate).toFixed(2)),
                              intToFormat(parseFloat(this.nLendingAmt).toFixed(2)),
                              intToFormat(parseFloat(this.iPendingQuantity * this.nLendingAmt).toFixed(2))
                        ]);
        TotalCurrentStock += this.iPendingQuantity;
        TotalStockvalue += this.iPendingQuantity * this.nSellRate;
        TotPurchaseAmount += parseFloat(this.nPurchaseSellRate * this.iPendingQuantity);
        TotLendingAmount += parseFloat(this.nLendingAmt * this.iPendingQuantity);
    });
    var AvgPurchaseRate = ($("#sddlProduct").jqxComboBox('getSelectedItem').value != -1) ? 'Avg: ' + intToFormat(parseFloat(TotPurchaseAmount / TotalCurrentStock).toFixed(2)) : '';
    $('#grid').html('<table cellpadding="0" cellspacing="0" border="0" id="table"></table>');
    $('#table').append('<tfoot id="tFoot"><tr><th></th><th></th><th></th><th></th><th>' + intToFormat(TotalCurrentStock) + '</th><th></th><th>' + intToFormat(parseFloat(TotalStockvalue).toFixed(2)) + '</th><th>' + intToFormat(AvgPurchaseRate) + '</th><th>' + intToFormat(parseFloat(TotPurchaseAmount).toFixed(2)) + '</th><th></th><th>' + intToFormat(parseFloat(TotLendingAmount).toFixed(2)) + '</th></tr>');
    var oTable = $('#table').dataTable({

        "sPaginationType": "scrolling",
        "bPaginate": true,                   // To disable pagination.
        "bJQueryUI": false,
        "sScrollX": "100%",                     // For applying Horizontal scroll in grid.
        "sScrollXInner": "100%",                // Setting size of scroll that how much it is bigger.
        "bFilter": true,
        "sPaginationType": "full_numbers",      // For displaying next, prev, first, last in pagination.
        "bSort": true,                         // For enable sorting.
        "bSortClasses": false,
        "iDisplayLength": 10,
        "aLengthMenu": [[10, 15, 20, 25, -1], [10, 15, 20, 25, "All"]],
        "sDom": 'T<"clear">lfrtip',
        "oTableTools": {
            "aButtons": [
			                {
			                    "sExtends": "copy",
			                    "sButtonText": "Copy to all",
			                    "bFooter": true
			                },
                            {
                                "sExtends": "csv",
                                "sButtonText": "CSV",
                                "bFooter": true
                            },
                            {
                                "sExtends": "xls",
                                "sButtonText": "Excel",
                                "bFooter": true
                            },
			                {
			                    "sExtends": "pdf",
			                    "sPdfOrientation": "landscape",
			                    "bFooter": true,
			                    "sPdfMessage": "Title: Stock Report"
			                }

            //"print"
		                ],
            "sSwfPath": "Scripts/swf/copy_cvs_xls_pdf.swf"
        },
        'aaData': dtData,
        "aoColumns": [
                                { "sTitle": "Company Name", "sClass": "sleft", "sWid%th": "15" },
                                { "sTitle": "Product Name", "sWidth": "15%", "sClass": "sleft" },
                                { "sTitle": "Batch", "sWidth": "10%", "sClass": "sleft" },
                                { "sTitle": "Expiry Date", "sWidth": "10%" },
                                { "sTitle": "Current Stock", "sClass": "sright", "sWidth": "10%" },
                                { "sTitle": "Dispense Rate", "sClass": "sright", "sWidth": "10%" },
                                { "sTitle": "Stock Value", "sClass": "sright", "sWidth": "10%" },
                                { "sTitle": "Purchase Rate", "sClass": "sright", "sWidth": "10%" },
                                { "sTitle": "Purchase Amount", "sClass": "sright", "sWidth": "10%" },
                                { "sTitle": "Lending Rate", "sClass": "sright", "sWidth": "10%" },
                                { "sTitle": "Lending Amount", "sClass": "sright", "sWidth": "10%" }
                            ]
    });
    //new FixedColumns(oTable);
}

//For Purchase Report

function BindPurchaseReportdropdownList() {
    //bind comapny dropdown list
    $.ajax({
        type: "POST",
        url: "WebService.asmx/GetCompanyForPurchase",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: '{}',
        error: function (ex) {
            console.log(ex);
        },
        success: function (data) {
            var data = jQuery.parseJSON(data.d);
            if (data != null) {
                $("#pddlCompany").jqxComboBox({ source: data, selectedIndex: 0, width: '200', height: '20px', theme: theme, displayMember: "vCompanyName", valueMember: "nCompanyId" });
                BindPurchaseReportAgentDropDownList("-1");
            }
            else {
                $("#pddlCompany").jqxComboBox('clear');
                onUpdated();
            }

        }
    });

    //bind payment status dropdown list
    var source = new Array();
    var content = new Object();
    content.id = "'Y'" + ',' + "'N'";
    content.status = 'both';
    source.push(content);

    var content = new Object();
    content.id = "'Y'";
    content.status = 'Paid';
    source.push(content);

    var content = new Object();
    content.id = "'N'";
    content.status = 'Pending';
    source.push(content);

    $("#pddlStatus").jqxComboBox({ source: source, selectedIndex: 0, width: '200', height: '20px', theme: theme, displayMember: "status", valueMember: "id" });

    //bind range(current year) dropdown list
    var source = new Array();
    var content = new Object();
    content.value = "1";
    content.text = 'Daily';
    source.push(content);

    var content = new Object();
    content.value = "2";
    content.text = 'Weekly';
    source.push(content);

    var content = new Object();
    content.value = "3";
    content.text = 'Monthly';
    source.push(content);

    var content = new Object();
    content.value = "4";
    content.text = 'Quarterly';
    source.push(content);

    var content = new Object();
    content.value = "5";
    content.text = 'Half Yearly';
    source.push(content);

    var content = new Object();
    content.value = "6";
    content.text = 'Yearly';
    source.push(content);


    $("#divDate").jqxComboBox({ source: source, selectedIndex: 0, width: '200', height: '20px', theme: theme, displayMember: "text", valueMember: "value" });

}
function BindPurchaseReportAgentDropDownList(Id) {
    $.ajax({
        type: "POST",
        url: "WebService.asmx/GetAgentForPurchase",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: '{"CompanyId":"' + Id + '"}',
        error: function (ex) {
            console.log(ex);
        },
        success: function (data) {

            var data = jQuery.parseJSON(data.d);
            if (data != null) {
                $("#pddlAgent").jqxComboBox({ source: data, selectedIndex: 0, width: '200', height: '20px', theme: theme, displayMember: "vAgentName", valueMember: "nAgentId" });
                BindPurchaseReportProductdropDownList("-1", "-1");
            }
        }
    });

}
function BindPurchaseReportProductdropDownList(ComapnyId, AgentId) {
    var content = new Object();
    content.CompanyId = ComapnyId;
    content.AgentId = AgentId;

    var jsontext = JSON.stringify(content);

    $.ajax({
        type: "POST",
        url: "WebService.asmx/GetProductComapnyAgentWiseForPurchase",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: jsontext,
        error: function (ex) {
            console.log(ex);
        },
        success: function (data) {

            var data = jQuery.parseJSON(data.d);
            if (data != null) {
                $("#pddlProduct").jqxComboBox({ source: data, selectedIndex: 0, width: '200', height: '20px', theme: theme, displayMember: "vProductName", valueMember: "nProductId" });
            }
        }
    });

}
function DisplayPurchaseReportGrid() {

    var Reports = $("#ddlReports").jqxComboBox('getSelectedItem');
    var CompanyId = $("#pddlCompany").jqxComboBox('getSelectedItem');
    var AgentId = $("#pddlAgent").jqxComboBox('getSelectedItem');
    var ProductId = (Reports.value == "1") ? $("#pddlProduct").jqxComboBox('getSelectedItem') : null;
    var Status = $("#pddlStatus").jqxComboBox('getSelectedItem');
    CompanyId = (CompanyId == null && CompanyId == undefined) ? "-1" : CompanyId.value;
    AgentId = (AgentId == null && AgentId == undefined) ? "-1" : AgentId.value;
    ProductId = (ProductId == null && ProductId == undefined) ? "-1" : ProductId.value;
    Status = (Status == null) ? "-1" : Status.value;
    GetView_PendingPayments(Reports.value, CompanyId, AgentId, ProductId, Status);
}

function GetView_PendingPayments(ReportsId, CompanyId, AgentId, ProductId, Status) {
    var fromDate = $('#txtFromDate').datepicker('getDate');
    var toDate = $('#txtToDate').datepicker('getDate');
    var content = new Object();
    content.ReportsId = ReportsId;
    content.CompanyId = CompanyId;
    content.AgentId = AgentId;
    content.ProductId = ProductId;
    content.Status = Status;

    if (fromDate != null && fromDate != undefined) {
        var startMonth = (fromDate.getMonth().toString().length < 2) ? '0' + (fromDate.getMonth() + 1).toString() : fromDate.getMonth().toString();
        var startDay = (fromDate.getDate().toString().length < 2) ? '0' + fromDate.getDate().toString() : fromDate.getDate().toString();
        var startYear = fromDate.getFullYear().toString();
    }
    else
        fromDate = '';

    if (toDate != null && toDate != undefined) {
        var endMonth = (toDate.getMonth().toString().length < 2) ? '0' + (toDate.getMonth() + 1).toString() : toDate.getMonth().toString();
        var endDay = (toDate.getDate().toString().length < 2) ? '0' + toDate.getDate().toString() : toDate.getDate().toString();
        var endYear = toDate.getFullYear().toString();
    }
    else
        toDate = '';

    content.FromDate = parseInt(startYear + startMonth + startDay + "000000");
    content.ToDate = parseInt(endYear + endMonth + endDay + "999999");
    var jsontext = JSON.stringify(content);

    $.ajax({
        type: "POST",
        url: "WebService.asmx/GetView_PendingPayments",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: jsontext,
        error: function (ex) {
            console.log(ex);
        },
        success: function (data) {
            var data = jQuery.parseJSON(data.d);
            console.log(data);
            if (data != null)
                BindPurchaseReportGrid(data);
            else
                $("#grid").html("<div style='width:100%;text-align:center;font-weight:bold;font-size:14px'>Data not found</div>");
        },
        failure: function (errMsg) {
            $('#errorMessage').text(errMsg);  //errorMessage is id of the div
        }
    });
}

function BindPurchaseReportGrid(result) {
    var Reports = $("#ddlReports").jqxComboBox('getSelectedItem');
    var dtData = new Array();
    var columns = new Array();

    $('#grid').html('<table cellpadding="0" cellspacing="0" border="0" id="table"></table>');
    if (Reports.value == "1") {
        var TotalQuantity = 0;
        var TotalAmt = 0;
        var AvgPurchaseRate = 0;
        var TotLendingAmt = 0;

        $.each(result, function (index) {
            dtData.push([
                              this.vTranDate,
                              this.vAgentCompany,
                              this.vAgentName,
                              this.vAgentDocNo,
                              this.vProductName,
                              this.vBatch,
                              intToFormat(this.iQuantity),
                              intToFormat(parseFloat(this.nPurchaseSellRate).toFixed(2)),
                              intToFormat(parseFloat(this.nAmount).toFixed(2)),
                              intToFormat(parseFloat(this.nLendingAmt).toFixed(2)),
                              intToFormat(parseFloat(this.nLendingAmt * this.iQuantity).toFixed(2))
            // intToFormat(this.nPaidAmount),
            // intToFormat(this.nPendingAmount)
            //(this.cIsPaymentDone == "Y") ? "Paid" : "Pending"
                        ]);
            TotalQuantity += this.iQuantity;
            TotalAmt += this.nAmount;
            AvgPurchaseRate += this.nPurchaseSellRate;
            TotLendingAmt += this.nLendingAmt * this.iQuantity;
        });
        TotalQuantity = TotalQuantity;
        TotalAmt = parseFloat(TotalAmt).toFixed(2);
        TotLendingAmt = parseFloat(TotLendingAmt).toFixed(2);

        var obj = new Object();
        obj.sTitle = "Purchase Date";
        obj.sWidth = "8%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Company Name";
        obj.sClass = "sleft";
        obj.sWidth = "15%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Agent Name";
        obj.sClass = "sleft";
        obj.sWidth = "17%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Invoice No.";
        obj.sClass = "sleft";
        obj.sWidth = "8%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Product Name";
        obj.sClass = "sleft";
        obj.sWidth = "12%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Batch";
        obj.sClass = "sleft";
        obj.sWidth = "8%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Quantity";
        obj.sClass = "sright";
        obj.sWidth = "5%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Purchase Rate";
        obj.sClass = "sright";
        obj.sWidth = "5%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Total Amount";
        obj.sClass = "sright";
        obj.sWidth = "10%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Lending Rate";
        obj.sClass = "sright";
        obj.sWidth = "10%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Lending Amount";
        obj.sClass = "sright";
        obj.sWidth = "10%";
        columns.push(obj);

        //AvgPurchaseRate = ($("#pddlProduct").jqxComboBox('getSelectedItem').value != -1) ? intToFormat(parseFloat(AvgPurchaseRate / dtData.length).toFixed(2)) : '';
        AvgPurchaseRate = ($("#pddlProduct").jqxComboBox('getSelectedItem').value != -1) ? 'Avg: ' + intToFormat(parseFloat(TotalAmt / TotalQuantity).toFixed(2)) : '';

        $('#table').append('<tfoot id="tFoot"><tr><th></th><th></th><th></th><th></th><th></th><th></th><th>' + intToFormat(TotalQuantity) + '</th><th>' + AvgPurchaseRate + '</th><th>' + intToFormat(TotalAmt) + '</th><th></th><th>' + intToFormat(TotLendingAmt) + '</th></tr>');
    }
    else if (Reports.value == "7") {
        var TotalAmt = 0;
        var TotalPaidAmt = 0;
        var TotalPendingAmt = 0;
        var TotDiscountAmount = 0;

        $.each(result, function (index) {
            dtData.push([
                              this.vTranDate,
                              this.vAgentCompany,
                              this.vAgentName,
                              this.vAgentDocNo,
                              intToFormat(parseFloat(this.nTotalAmount).toFixed(2)),
                              intToFormat(parseFloat(this.nDiscountAmount).toFixed(2)),
                              intToFormat(parseFloat(this.nPaidAmount).toFixed(2)),
                              intToFormat(parseFloat(this.nPendingAmount).toFixed(2))
                        ]);
            TotalAmt += this.nTotalAmount;
            TotalPaidAmt += this.nPaidAmount;
            TotDiscountAmount += this.nDiscountAmount;
            TotalPendingAmt += this.nPendingAmount;
        });

        var obj = new Object();
        obj.sTitle = "Purchase Date";
        obj.sWidth = "10%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Company Name";
        obj.sClass = "sleft";
        obj.sWidth = "15%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Agent Name";
        obj.sClass = "sleft";
        obj.sWidth = "15%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Invoice No.";
        obj.sClass = "sleft";
        obj.sWidth = "15%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Total Amount";
        obj.sClass = "sright";
        obj.sWidth = "10%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Discount Amount";
        obj.sClass = "sright";
        obj.sWidth = "5%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Paid Amount";
        obj.sClass = "sright";
        obj.sWidth = "10%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Pending Amount";
        obj.sClass = "sright";
        obj.sWidth = "10%";
        columns.push(obj);

        $('#table').append('<tfoot id="tFoot"><tr><th></th><th></th><th></th><th></th><th>' + intToFormat(parseFloat(TotalAmt).toFixed(2)) + '</th><th>' + intToFormat(parseFloat(TotDiscountAmount).toFixed(2)) + '</th><th>' + intToFormat(parseFloat(TotalPaidAmt).toFixed(2)) + '</th><th>' + intToFormat(parseFloat(TotalPendingAmt).toFixed(2)) + '</th></tr>');
    }
    else {
        var TotalAmt = 0;
        var TotalPaidAmt = 0;
        var TotDiscountAmount = 0;
        var TotalCashAmt = 0;
        var TotalChequeAmt = 0;

        $.each(result, function (index) {
            dtData.push([
                              this.vTranDate,
                              this.vAgentDocNo,
                              intToFormat(parseFloat(this.nTotalAmount).toFixed(2)),
                              intToFormat(parseFloat(this.nDiscountAmount).toFixed(2)),
                              intToFormat(parseFloat(this.nPaidAmount).toFixed(2)),
                              intToFormat(parseFloat(this.nCashAmount).toFixed(2)),
                              intToFormat(parseFloat(this.nChequeAmount).toFixed(2)),
                              this.vChequeNo,
                              this.vBankName,

                        ]);
            TotalAmt += this.nTotalAmount;
            TotalPaidAmt += this.nPaidAmount;
            TotDiscountAmount += this.nDiscountAmount;
            TotalCashAmt += this.nCashAmount;
            TotalChequeAmt += this.nChequeAmount;
        });

        var obj = new Object();
        obj.sTitle = "Purchase Date";
        obj.sWidth = "10%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Invoice No.";
        obj.sClass = "sleft";
        obj.sWidth = "15%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Total Amount";
        obj.sClass = "sright";
        obj.sWidth = "10%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Discount";
        obj.sClass = "sright";
        obj.sWidth = "5%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Paid Amount";
        obj.sClass = "sright";
        obj.sWidth = "10%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Cash Amount";
        obj.sClass = "sright";
        obj.sWidth = "10%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Cheque Amount";
        obj.sClass = "sright";
        obj.sWidth = "10%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Cheque No";
        obj.sClass = "sleft";
        obj.sWidth = "10%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Bank Name";
        obj.sClass = "sleft";
        obj.sWidth = "10%";
        columns.push(obj);

        $('#table').append('<tfoot id="tFoot"><tr><th></th><th></th><th>' + intToFormat(parseFloat(TotalAmt).toFixed(2)) + '</th><th>' + intToFormat(parseFloat(TotDiscountAmount).toFixed(2)) + '</th><th>' + intToFormat(parseFloat(TotalPaidAmt).toFixed(2)) + '</th><th>' + intToFormat(parseFloat(TotalCashAmt).toFixed(2)) + '</th><th>' + intToFormat(parseFloat(TotalChequeAmt).toFixed(2)) + '</th><th></th><th></th></tr>');
    }


    var oTable = $('#table').dataTable({

        "sPaginationType": "scrolling",
        "bPaginate": true,                   // To disable pagination.
        "bJQueryUI": false,
        "sScrollX": "100%",                     // For applying Horizontal scroll in grid.
        "sScrollXInner": "100%",                // Setting size of scroll that how much it is bigger.
        //"iDisplayLength": 25,                   // Setting values of dropdown for pagination
        //"sScrollY": "300px",                    // For Vertical Scroll in grid.
        "bFilter": true,
        "sPaginationType": "full_numbers",      // For displaying next, prev, first, last in pagination.
        "bSort": true,                         // For enable sorting.
        "bSortClasses": false,
        "iDisplayLength": 10,
        "aLengthMenu": [[10, 15, 20, 25, -1], [10, 15, 20, 25, "All"]],
        //                "sScrollY": "380px",
        //                "bScrollCollapse": true,
        //                "bPaginate": true,
        //                "bJQueryUI": false,
        // "sDom": '<"H"lTfr>t<"F"ip>',
        "sDom": 'T<"clear">lfrtip',
        "oTableTools": {
            "aButtons": [
			                {
			                    "sExtends": "copy",
			                    "sButtonText": "Copy to all",
			                    "bFooter": true
			                },
                            {
                                "sExtends": "csv",
                                "sButtonText": "CSV",
                                "bFooter": true
                            },
                            {
                                "sExtends": "xls",
                                "sButtonText": "Excel",
                                "bFooter": true
                            },
			                {
			                    "sExtends": "pdf",
			                    "sPdfOrientation": "landscape",
			                    "bFooter": true,
			                    "sPdfMessage": "Title: Stock Report"
			                }

            //"print"
		                ],
            "sSwfPath": "Scripts/swf/copy_cvs_xls_pdf.swf"
        },
        'aaData': dtData,
        "aoColumns": columns
    });
    oTable.fnSort([[0, 'desc'], [1, 'asc']]);
}

//For Dispensing And Collection Report
function BindDispensingReportdropdownList(Type) {
    //bind comapny dropdown list
    var Reports = $("#ddlReports").jqxComboBox('getSelectedItem');
    var TranType = Type
    $.ajax({
        type: "POST",
        url: "WebService.asmx/GetPatientForDispensing",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: '{"TranType":"' + TranType + '"}',
        error: function (ex) {
            console.log(ex);
        },
        success: function (data) {

            var data = jQuery.parseJSON(data.d);
            if (data != null) {
                $("#dddlPatient").jqxComboBox({ source: data, selectedIndex: 0, width: '200', height: '20px', theme: theme, displayMember: "vPatientName", valueMember: "nPatientId" });
                // if (Reports.value == "2")
                BindDispensingReportProductDropDownList("-1", "-1", "'D'");
                // else

            }
            else
                $("#dddlPatient").jqxComboBox('clear');
        }
    });

    //            //bind payment status dropdown list
    //            var source = new Array();
    //            var content = new Object();
    //            content.id = "'Y'" + ',' + "'N'";
    //            content.status = 'both';
    //            source.push(content);

    //            var content = new Object();
    //            content.id = "'Y'";
    //            content.status = 'Paid';
    //            source.push(content);

    //            var content = new Object();
    //            content.id = "'N'";
    //            content.status = 'Pending';
    //            source.push(content);

    //            $("#dddlStatus").jqxComboBox({ source: source, selectedIndex: 0, width: '200', height: '20px', theme: theme, displayMember: "status", valueMember: "id" });


}

function BindCollectedTypeDropDownList() {
    var source = new Array();
    var content = new Object();
    content.id = "'A'" + ',' + "'D'";
    content.status = 'both';
    source.push(content);

    var content = new Object();
    content.id = "'A'";
    content.status = 'Appointment';
    source.push(content);

    var content = new Object();
    content.id = "'D'";
    content.status = 'Dispensing';
    source.push(content);

    $("#dddlCollectedType").jqxComboBox({ source: source, selectedIndex: 0, width: '200', height: '20px', theme: theme, displayMember: "status", valueMember: "id" });
}

function BindDispensingReportComapnydropdownList(PatientId) {
    PatientId = (PatientId == undefined) ? "-1" : PatientId;
    $.ajax({
        type: "POST",
        url: "WebService.asmx/GetCompanyForStock",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: '{"ReportsId":"3","PatientId":' + PatientId + '}',
        error: function (ex) {
            console.log(ex);
        },
        success: function (data) {

            var data = jQuery.parseJSON(data.d);
            if (data != null)
                $("#dddlCompany").jqxComboBox({ source: data, selectedIndex: 0, width: '200', height: '20px', theme: theme, displayMember: "vCompanyName", valueMember: "nCompanyId" });
            else {
                $("#dddlCompany").jqxComboBox('clear');
                onUpdated();
            }
        }
    });
}
function BindCollectionReportdropdownList() {
    var Reports = $("#ddlReports").jqxComboBox('getSelectedItem');
    var TranType = "'A'" + ',' + "'D'";
    $.ajax({
        type: "POST",
        url: "WebService.asmx/GetPatientForDispensing",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: '{"TranType":"' + TranType + '"}',
        error: function (ex) {
            console.log(ex);
        },
        success: function (data) {

            var data = jQuery.parseJSON(data.d);
            if (data != null) {
                $("#cddlPatient").jqxComboBox({ source: data, selectedIndex: 0, width: '200', height: '20px', theme: theme, displayMember: "vPatientName", valueMember: "nPatientId" });
            }
            else {
                $("#cddlPatient").jqxComboBox('clear');
                onUpdated();
            }
        }
    });
}
function BindDispensingReportProductDropDownList(PatientId, CompanyId, TranType) {
    var content = new Object();
    content.PatientId = PatientId;
    content.CompanyId = CompanyId;
    content.TranType = TranType;

    var jsontext = JSON.stringify(content);

    $.ajax({
        type: "POST",
        url: "WebService.asmx/GetProductCompanyWiseForDispense",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: jsontext,
        error: function (ex) {
            console.log(ex);
        },
        success: function (data) {

            var data = jQuery.parseJSON(data.d);
            if (data != null) {
                $("#dddlProduct").jqxComboBox({ source: data, selectedIndex: 0, width: '200', height: '20px', theme: theme, displayMember: "vProductName", valueMember: "nProductId" });
                BindDispensingReportProcedureDropDownList("-1");
            }
            else
                $("#dddlProduct").jqxComboBox('clear');
        }
    });
}
function BindDispensingReportProcedureDropDownList(PatientId) {
    $.ajax({
        type: "POST",
        url: "WebService.asmx/GetProcedurePatientWiseForDispense",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: "{'PatientId':" + PatientId + "}",
        error: function (ex) {
            console.log(ex);
        },
        success: function (data) {

            var data = jQuery.parseJSON(data.d);
            if (data != null) {
                $("#pddlProcedure").jqxComboBox({ source: data, selectedIndex: 0, width: '200', height: '20px', theme: theme, displayMember: "vPurposeDesc", valueMember: "iPurposeId" });
            }
            else
                $("#pddlProcedure").jqxComboBox('clear');
        }
    });
}

function DisplayDispensingReportGrid() {
    
    var Reports = $("#ddlReports").jqxComboBox('getSelectedItem');
    var PatientId = (Reports.value != "3" && Reports.value != "9") ? $("#dddlPatient").jqxComboBox('getSelectedItem') : $("#cddlPatient").jqxComboBox('getSelectedItem');
    var CompanyId = ($("#dddlCompany").jqxComboBox('getSelectedItem') != null && $("#dddlCompany").jqxComboBox('getSelectedItem') != undefined) ? $("#dddlCompany").jqxComboBox('getSelectedItem').value : "-1";
    var ProductId = (Reports.value == '5') ? null : $("#dddlProduct").jqxComboBox('getSelectedItem');
    var PurposeId = $("#pddlProcedure").jqxComboBox('getSelectedItem');
    var ClinicId = ($("#cddlClinic").jqxComboBox('getSelectedItem') != null && $("#cddlClinic").jqxComboBox('getSelectedItem') != undefined) ? $("#cddlClinic").jqxComboBox('getSelectedItem').value : "-1";
    // var Status = $("#dddlStatus").jqxComboBox('getSelectedItem');
    var Status = "'Y'" + ',' + "'N'";
    var CollectedType = $("#dddlCollectedType").jqxComboBox('getSelectedItem').value;
    var TranType = (Reports.value == "2") ? "'D'" : ((Reports.value == '6') ? "'A'" : CollectedType);

    PatientId = (PatientId == null && PatientId == undefined) ? "-1" : PatientId.value;
    ProductId = (ProductId == null && ProductId == undefined) ? "-1" : ProductId.value;
    PurposeId = (PurposeId == null && PurposeId == undefined) ? "-1" : PurposeId.value;
    // Status = (Status == null) ? "-1" : Status.value;

    GetView_PendingPaymentsForDispensing(PatientId, ProductId, Status, TranType, PurposeId, Reports.value, CompanyId, ClinicId);
}

function GetView_PendingPaymentsForDispensing(PatientId, ProductId, Status, TranType, PurposeId, Reports, CompanyId, ClinicId) {
    var fromDate = $('#txtFromDate').datepicker('getDate');
    var toDate = $('#txtToDate').datepicker('getDate');
    var method;
    var content = new Object();
    content.PatientId = PatientId;
    content.CompanyId = CompanyId;
    content.ProductId = ProductId;
    content.Status = Status;
    content.TranType = TranType;

    if (fromDate != null && fromDate != undefined) {
        var startMonth = (fromDate.getMonth().toString().length < 2) ? '0' + (fromDate.getMonth() + 1).toString() : fromDate.getMonth().toString();
        var startDay = (fromDate.getDate().toString().length < 2) ? '0' + fromDate.getDate().toString() : fromDate.getDate().toString();
        var startYear = fromDate.getFullYear().toString();
    }
    else
        fromDate = '';
    if (toDate != null && toDate != undefined) {
        var endMonth = (toDate.getMonth().toString().length < 2) ? '0' + (toDate.getMonth() + 1).toString() : toDate.getMonth().toString();
        var endDay = (toDate.getDate().toString().length < 2) ? '0' + toDate.getDate().toString() : toDate.getDate().toString();
        var endYear = toDate.getFullYear().toString();
    }
    else
        toDate = '';

    content.FromDate = parseInt(startYear + startMonth + startDay + "000000");
    content.ToDate = parseInt(endYear + endMonth + endDay + "999999");
    content.Reports = Reports;
    content.PurposeId = PurposeId;
    content.ClinicId = ClinicId;
    
    var jsontext = JSON.stringify(content);
    method = (Reports == 3 || Reports == 9 ? "WebService.asmx/GetView_PaymentCollection" : "WebService.asmx/GetView_PendingPaymentsForDispensing")
    $.ajax({
        type: "POST",
        url: method,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: jsontext,
        error: function (ex) {
            console.log(ex);
        },
        success: function (data) {
            
            var data = jQuery.parseJSON(data.d);
            console.log(data);
            if (data != null) {
                if (TranType == "'D'" && $("#ddlReports").jqxComboBox('getSelectedItem').value == "2") {
                    BindDispensingReportGrid(data);
                }
                else
                    BindCollectionReportGrid(data);
                if (Reports == "5") {
                    Load3cReport(TranType);
                }
            }
            else
                $("#grid").html("<div style='width:100%;text-align:center;font-weight:bold;font-size:14px'>Data not found</div>");
        },
        failure: function (errMsg) {
            $('#errorMessage').text(errMsg);  //errorMessage is id of the div
        }
    });
}

function BindDispensingReportGrid(result) {
    var dtData = new Array();
    var TotQty = 0;
    var TotAmt = 0;
    var AvgDispensingRate = 0;

    $.each(result, function (index) {
        dtData.push([
                              this.vTranDate,
                              this.vPatientName,
                              this.vProductCompany,
                              this.vProductName,
                              this.vBatch,
                              intToFormat(this.iQuantity),
                              intToFormat(parseFloat(this.nSellRate).toFixed(2)),
                              intToFormat(parseFloat(this.iQuantity * this.nSellRate).toFixed(2))

        //(this.cIsPaymentDone == "Y") ? "Paid" : "Pending"
                        ]);
        TotQty += parseInt(this.iQuantity);
        TotAmt += parseInt(this.iQuantity * this.nSellRate);
        AvgDispensingRate += this.nSellRate;

    });

    AvgDispensingRate = ($("#dddlProduct").jqxComboBox('getSelectedItem').value != -1) ? intToFormat(parseFloat(AvgDispensingRate / dtData.length).toFixed(2)) : '';

    $('#grid').html('<table cellpadding="0" cellspacing="0" border="0" id="table"></table>');
    $('#table').append('<tfoot id="tFoot"><tr><th></th><th></th><th></th><th></th><th></th><th>' + intToFormat(TotQty) + '</th><th>' + AvgDispensingRate + '</th><th>' + intToFormat(parseFloat(TotAmt).toFixed(2)) + '</th></tr>');
    var dTable = $('#table').dataTable({

        "sPaginationType": "scrolling",
        "bPaginate": true,                   // To disable pagination.
        "bJQueryUI": false,
        "sScrollX": "100%",                     // For applying Horizontal scroll in grid.
        "sScrollXInner": "100%",                // Setting size of scroll that how much it is bigger.
        //"iDisplayLength": 25,                   // Setting values of dropdown for pagination
        //"sScrollY": "300px",                    // For Vertical Scroll in grid.
        "bFilter": true,
        "sPaginationType": "full_numbers",      // For displaying next, prev, first, last in pagination.
        "bSort": true,                         // For enable sorting.
        "bSortClasses": false,
        "iDisplayLength": 10,
        "aLengthMenu": [[10, 15, 20, 25, -1], [10, 15, 20, 25, "All"]],
        //                "sScrollY": "380px",
        //                "bScrollCollapse": true,
        //                "bPaginate": true,
        //                "bJQueryUI": false,
        // "sDom": '<"H"lTfr>t<"F"ip>',
        "sDom": 'T<"clear">lfrtip',
        "oTableTools": {
            "aButtons": [
			                {
			                    "sExtends": "copy",
			                    "sButtonText": "Copy to all",
			                    "bFooter": true
			                },
                            {
                                "sExtends": "csv",
                                "sButtonText": "CSV",
                                "bFooter": true
                            },
                            {
                                "sExtends": "xls",
                                "sButtonText": "Excel",
                                "bFooter": true
                            },
			                {
			                    "sExtends": "pdf",
			                    "sPdfOrientation": "landscape",
			                    "bFooter": true,
			                    "sPdfMessage": "Title: Stock Report"
			                }

            //"print"
		                ],
            "sSwfPath": "Scripts/swf/copy_cvs_xls_pdf.swf"
        },
        'aaData': dtData,
        "aoColumns": [
                                { "sTitle": "Dispensing Date", "sWidth": "10%" },
                                { "sTitle": "Patient Name", "sClass": "sleft", "sWidth": "15%" },
                                { "sTitle": "Company Name", "sWidth": "15%", "sClass": "sleft" },
                                { "sTitle": "Product Name", "sWidth": "15%", "sClass": "sleft" },
                                { "sTitle": "Batch", "sWidth": "10%", "sClass": "sleft" },
                                { "sTitle": "Quantity", "sClass": "sright", "sWidth": "10%" },
                                { "sTitle": "Dispensing Rate", "sClass": "sright", "sWidth": "10%" },
                                { "sTitle": "Dispensing Amount", "sClass": "sright", "sWidth": "10%" }

        // { "sTitle": "Status", "sClass": "sright", "sWidth": "5%" }
                            ]
    });
    dTable.fnSort([[0, 'desc'], [1, 'asc']]);
}

function BindCollectionReportGrid(result) {
    debugger;
    var dtData = new Array();
    var columns = new Array();
    $('#grid').html('<table cellpadding="0" cellspacing="0" border="0" id="table"></table>');
    var Reports = $("#ddlReports").jqxComboBox('getSelectedItem');
    var ReportsId = Reports.value;
    var ReportsText = Reports.text;

    if (ReportsId == "3" || ReportsId == "9") {
        var TotAmount = 0;
        var TotPaidAmt = 0;
        var TotDiscount = 0;
        var TotPendingAmt = 0;
        var TotCashAmount = 0;
        var TotChequeAmount = 0;

        $.each(result, function (index) {
            if (ReportsId == "3") {
                if (this.cTranType == 'D' || (this.cTranType == 'A' && this.cStatusIndi == 'C')) {

                    dtData.push([
                              this.vPayCollectedOn,
                              this.vPatientName,
                              this.vPurposeDesc,
                              intToFormat(parseFloat(this.nTotalAmount).toFixed(2)),
                              intToFormat(parseFloat(this.nDiscountAmount).toFixed(2)),
                              intToFormat(parseFloat(this.nPaidAmount).toFixed(2)),
                              intToFormat(parseFloat(this.nPendingAmount).toFixed(2))
                        ]);
                    TotPendingAmt += this.nPendingAmount;
                }
            }
            else {
                dtData.push([
                              this.vPayCollectedOn,
                              this.vPatientName,
                              (this.cTranType == 'D') ? 'Dispensing' : this.vPurposeDesc,
                              intToFormat(parseFloat(this.nTotalAmount).toFixed(2)),
                              intToFormat(parseFloat(this.nDiscountAmount).toFixed(2)),
                              intToFormat(parseFloat(this.nPaidAmount).toFixed(2)),
                              intToFormat(parseFloat(this.nCashAmount).toFixed(2)),
                              intToFormat(parseFloat(this.nChequeAmount).toFixed(2)),
                              this.vChequeNo,
                              this.vBankName
                        ]);
                TotCashAmount += this.nCashAmount;
                TotChequeAmount += this.nChequeAmount;

            }
            TotAmount += this.nTotalAmount;
            TotPaidAmt += this.nPaidAmount;
            TotDiscount += this.nDiscountAmount;

        });

        var obj = new Object();
        obj.sTitle = "Collected Date";
        obj.sWidth = "10%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Patient Name";
        obj.sClass = "sleft";
        obj.sWidth = (ReportsId == "3") ? "25%" : "15%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Purpose";
        obj.sClass = "sleft";
        obj.sWidth = "15%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Total Amount";
        obj.sClass = "sright";
        obj.sWidth = (ReportsId == "3") ? "15%" : "10%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Discount Amount";
        obj.sClass = "sright";
        obj.sWidth = "5%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Collected Amount";
        obj.sClass = "sright";
        obj.sWidth = (ReportsId == "3") ? "15%" : "10%";
        columns.push(obj);

        if (ReportsId == "3") {
            var obj = new Object();
            obj.sTitle = "Pending Amount";
            obj.sClass = "sright";
            obj.sWidth = "15%";
            columns.push(obj);

            $('#table').append('<tfoot id="tFoot"><tr><th></th><th></th><th></th><th>' + intToFormat(parseFloat(TotAmount).toFixed(2)) + '</th><th>' + intToFormat(parseFloat(TotDiscount).toFixed(2)) + '</th><th>' + intToFormat(parseFloat(TotPaidAmt).toFixed(2)) + '</th><th>' + intToFormat(parseFloat(TotPendingAmt).toFixed(2)) + '</th></tr>');

        }
        else {
            var obj = new Object();
            obj.sTitle = "Cash Amount";
            obj.sClass = "sright";
            obj.sWidth = "10%";
            columns.push(obj);

            var obj = new Object();
            obj.sTitle = "Cheque Amount";
            obj.sClass = "sright";
            obj.sWidth = "10%";
            columns.push(obj);

            var obj = new Object();
            obj.sTitle = "Cheque No.";
            obj.sClass = "sleft";
            obj.sWidth = "10%";
            columns.push(obj);

            var obj = new Object();
            obj.sTitle = "Bank Name";
            obj.sClass = "sleft";
            obj.sWidth = "10%";
            columns.push(obj);

            $('#table').append('<tfoot id="tFoot"><tr><th></th><th></th><th></th><th>' + intToFormat(parseFloat(TotAmount).toFixed(2)) + '</th><th>' + intToFormat(parseFloat(TotDiscount).toFixed(2)) + '</th><th>' + intToFormat(parseFloat(TotPaidAmt).toFixed(2)) + '</th><th>' + intToFormat(parseFloat(TotCashAmount).toFixed(2)) + '</th><th>' + intToFormat(parseFloat(TotChequeAmount).toFixed(2)) + '</th><th></th><th></th></tr>');
        }
    }
    else if (ReportsId == "5") {

        var TotPaidAmount = 0;
        var date = new Date();
        date = date.format('dd-MMM-yyyy');
        $.each(result, function (index) {
            if (this.nPaidAmount != "0") {
                dtData.push([
                              date,
                              this.vPatientName,
                              (this.vPurposeDesc == '') ? 'Dispensing' : this.vPurposeDesc,
                              intToFormat(parseFloat(this.nPaidAmount).toFixed(2)),
                              this.vTranDate
                             ]);
                TotPaidAmount += parseFloat(this.nPaidAmount);
            }
        });
        $('#table').append('<tfoot id="tFoot"><tr><th></th><th></th><th></th><th>' + intToFormat(parseFloat(TotPaidAmount).toFixed(2)) + '</th><th></th></tr>');

        var obj = new Object();
        obj.sTitle = "Date";
        obj.sWidth = "20%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Patient Name";
        obj.sClass = "sleft";
        obj.sWidth = "20%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Procedure Description";
        obj.sClass = "sleft";
        obj.sWidth = "20%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Fees Received";
        obj.sClass = "sright";
        obj.sWidth = "20%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Date of receipt";
        obj.sWidth = "20%";
        columns.push(obj);

    }

    else if (ReportsId == "6") {

        var TotAmount = 0;

        $.each(result, function (index) {
            dtData.push([
                              this.vTranDate,
                              this.vPurposeDesc,
                              this.vPatientName,
                              this.vAuthorityName,
                              intToFormat(parseFloat(this.nTotalAmount).toFixed(2))

                        ]);
            TotAmount += this.nTotalAmount;
        });
        $('#table').append('<tfoot id="tFoot"><tr><th></th><th></th><th></th><th></th><th>' + intToFormat(parseFloat(TotAmount).toFixed(2)) + '</th></tr>');

        var obj = new Object();
        obj.sTitle = "Procedure Date";
        obj.sWidth = "20%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Procedure Description";
        obj.sClass = "sleft";
        obj.sWidth = "20%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Patient Name";
        obj.sClass = "sleft";
        obj.sWidth = "20%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Authority Name";
        obj.sClass = "sleft";
        obj.sWidth = "20%";
        columns.push(obj);

        var obj = new Object();
        obj.sTitle = "Charged Amount";
        obj.sClass = "sright";
        obj.sWidth = "20%";
        columns.push(obj);
    }

    var cTable = $('#table').dataTable({

        "sPaginationType": "scrolling",
        "bPaginate": true,                   // To disable pagination.
        "bJQueryUI": false,
        "sScrollX": "100%",                     // For applying Horizontal scroll in grid.
        "sScrollXInner": "100%",                // Setting size of scroll that how much it is bigger.
        //"iDisplayLength": 25,                   // Setting values of dropdown for pagination
        //"sScrollY": "300px",                    // For Vertical Scroll in grid.
        "bFilter": true,
        "sPaginationType": "full_numbers",      // For displaying next, prev, first, last in pagination.
        "bSort": true,                         // For enable sorting.
        "bSortClasses": false,
        "iDisplayLength": 10,
        "aLengthMenu": [[10, 15, 20, 25, -1], [10, 15, 20, 25, "All"]],
        //                "sScrollY": "380px",
        //                "bScrollCollapse": true,
        //                "bPaginate": true,
        //                "bJQueryUI": false,
        // "sDom": '<"H"lTfr>t<"F"ip>',
        "sDom": 'T<"clear">lfrtip',
        "oTableTools": {
            "aButtons": [
			                {
			                    "sExtends": "copy",
			                    "sButtonText": "Copy to all",
			                    "bFooter": true
			                },
                            {
                                "sExtends": "csv",
                                "sButtonText": "CSV",
                                "bFooter": true
                            },
                            {
                                "sExtends": "xls",
                                "sButtonText": "Excel",
                                "bFooter": true
                            },
			                {
			                    "sExtends": "pdf",
			                    "sPdfOrientation": "landscape",
			                    "bFooter": true,
			                    "sPdfMessage": (ReportsId == "5") ? "<div>Bharat Patel</div>FORM NO. 3C<br/>[See rule 6F(3)]/nForm of daily case register/n[TO BE MAINTAINED BY PRACTITIONERS OF ANY SYSTEM OF MEDICINE, I.E., PHYSICIANS, SURGEONS,DENTISTS, PATHOLOGISTS, RADIOLOGISTS, VAIDS, HAKIMS, ETC.]" : "Title: " + ReportsText + ""
			                },
                            {
                                "sExtends": "text",
                                "sButtonText": "Load 3cReport",
                                "bFooter": true
                            }
            //"print"
		                ],
            "sSwfPath": "Scripts/swf/copy_cvs_xls_pdf.swf"
        },
        'aaData': dtData,
        "aoColumns": columns
    });

    //cTable.fnSort([[0, 'desc'], [1, 'asc']]);
    if (ReportsId != "5") {
        cTable.fnSort([[0, 'desc'], [1, 'asc']]);
        $('#ToolTables_table_4').css('display', 'none');
    }
    else {
        //cTable.fnSort([[0, 'desc'], [4, 'asc'], [1, 'asc']]);
        $('#ToolTables_table_4').click(function () {
            var index = $("#dddlCollectedType").jqxComboBox('selectedIndex');
            var trantype = (index == 0 ? "'A','D'" : (index == 1 ? "'A'" : "'D'"));
            Load3cReport(trantype);
        });
    }
}

function BindStockLedgerReportdropdownList(data) {
    var source = new Array();

    $.ajax({
        type: "POST",
        url: "WebService.asmx/GetCompanyForStockLedger",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: '{}',
        error: function (ex) {
            console.log(ex);
        },
        success: function (data) {
            var data = jQuery.parseJSON(data.d);
            if (data != null) {
                $("#lddlCompany").jqxComboBox({ source: data, selectedIndex: 0, width: '200', height: '20px', theme: theme, displayMember: "vCompanyName", valueMember: "nCompanyId" });
                BindStockLedgerComapnyDropDownList("-1");
            }
            else {
                $("#lddlCompany").jqxComboBox('clear');
                onUpdated();
            }
        }
    });

}
function BindStockLedgerComapnyDropDownList(CompanyId) {
    $.ajax({
        type: "POST",
        url: "WebService.asmx/GetProductCompanyWiseForStockLedger",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: '{"CompanyId":' + CompanyId + '}',
        error: function (ex) {
            console.log(ex);
        },
        success: function (data) {

            var data = jQuery.parseJSON(data.d);
            if (data != null) {
                $("#lddlProduct").jqxComboBox({ source: data, selectedIndex: 0, width: '200', height: '20px', theme: theme, displayMember: "vProductName", valueMember: "nProductId" });
                onUpdated();
            }
            else
                $("#lddlProduct").jqxComboBox('clear');
        }
    });
}
function DisplayStockLedgerReportGrid() {

    var CompanyId = $("#lddlCompany").jqxComboBox('getSelectedItem');
    var ProductId = $("#lddlProduct").jqxComboBox('getSelectedItem');

    CompanyId = (CompanyId == null && CompanyId == undefined) ? "-1" : CompanyId.value;
    ProductId = (ProductId == null && prompt == undefined) ? "-1" : ProductId.value;

    GetView_StockLedger(CompanyId, ProductId);
}

function GetView_StockLedger(CompanyId, ProductId) {
    var fromDate = $('#txtFromDate').datepicker('getDate');
    var toDate = $('#txtToDate').datepicker('getDate');
    var content = new Object();
    content.CompanyId = CompanyId;
    content.ProductId = ProductId;

    if (fromDate != null && fromDate != undefined) {
        var startMonth = (fromDate.getMonth().toString().length < 2) ? '0' + (fromDate.getMonth() + 1).toString() : fromDate.getMonth().toString();
        var startDay = (fromDate.getDate().toString().length < 2) ? '0' + fromDate.getDate().toString() : fromDate.getDate().toString();
        var startYear = fromDate.getFullYear().toString();
    }
    else
        fromDate = '';

    if (toDate != null && toDate != undefined) {
        var endMonth = (toDate.getMonth().toString().length < 2) ? '0' + (toDate.getMonth() + 1).toString() : toDate.getMonth().toString();
        var endDay = (toDate.getDate().toString().length < 2) ? '0' + toDate.getDate().toString() : toDate.getDate().toString();
        var endYear = toDate.getFullYear().toString();
    }
    else
        toDate = '';

    content.FromDate = parseInt(startYear + startMonth + startDay + "000000");
    content.ToDate = parseInt(endYear + endMonth + endDay + "999999");

    var jsontext = JSON.stringify(content);

    $.ajax({
        type: "POST",
        url: "WebService.asmx/GetView_StockLedger",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: jsontext,
        error: function (ex) {
            console.log(ex);
        },
        success: function (data) {

            var data = jQuery.parseJSON(data.d);
            console.log(data);

            if (data != null)
                BindStockLedgerReportGrid(data);
            else
                $("#grid").html("<div style='width:100%;text-align:center;font-weight:bold;font-size:14px'>Data not found</div>");
        },
        failure: function (errMsg) {
            $('#errorMessage').text(errMsg);  //errorMessage is id of the div
        }
    });
}

function BindStockLedgerReportGrid(result) {
    var dtData = new Array();

    $.each(result, function (index) {
        dtData.push([
                              (this.vCompanyName == "") ? "-" : this.vCompanyName,
                              (this.vProductName == "") ? "-" : this.vProductName,
                              (this.vBatch == "") ? "-" : this.vBatch,
                              this.vModifyOn.split(" ")[0] + "-" + this.vModifyOn.split(" ")[1] + "-" + this.vModifyOn.split(" ")[2],
                              (this.cTranType == "P") ? "Purchase" : "Dispensing",
                              this.vUserName,
                              intToFormat(this.iQuantity),
                              parseFloat((this.cTranType == "P") ? this.nPurchaseSellRate : this.nSellRate).toFixed(2),
                              intToFormat(parseFloat(((this.cTranType == "P") ? this.nPurchaseSellRate : this.nSellRate) * this.iQuantity).toFixed(2)),
                              intToFormat(this.iBatchPendingQuantity),
                        ]);

    });
    console.log(dtData);
    $('#grid').html('<table cellpadding="0" cellspacing="0" border="0" id="table"></table>');
    $('#table').dataTable({

        "sPaginationType": "scrolling",
        "bPaginate": true,                   // To disable pagination.
        "bJQueryUI": false,
        "sScrollX": "100%",                     // For applying Horizontal scroll in grid.
        "sScrollXInner": "100%",                // Setting size of scroll that how much it is bigger.
        //"iDisplayLength": 25,                   // Setting values of dropdown for pagination
        //"sScrollY": "300px",                    // For Vertical Scroll in grid.
        "bFilter": true,
        "sPaginationType": "full_numbers",      // For displaying next, prev, first, last in pagination.
        "bSort": false,                         // For enable sorting.
        "bSortClasses": false,
        "iDisplayLength": 10,
        "aLengthMenu": [[10, 15, 20, 25, -1], [10, 15, 20, 25, "All"]],
        //                "sScrollY": "380px",
        //                "bScrollCollapse": true,
        //                "bPaginate": true,
        //                "bJQueryUI": false,
        // "sDom": '<"H"lTfr>t<"F"ip>',
        "sDom": 'T<"clear">lfrtip',
        "oTableTools": {
            "aButtons": [
			                {
			                    "sExtends": "copy",
			                    "sButtonText": "Copy to all",
			                    "bFooter": true
			                },
                            {
                                "sExtends": "csv",
                                "sButtonText": "CSV",
                                "bFooter": true
                            },
                            {
                                "sExtends": "xls",
                                "sButtonText": "Excel",
                                "bFooter": true
                            },
			                {
			                    "sExtends": "pdf",
			                    "sPdfOrientation": "landscape",
			                    "bFooter": true,
			                    "sPdfMessage": "Title: Stock Report"
			                }

            //"print"
		                ],
            "sSwfPath": "Scripts/swf/copy_cvs_xls_pdf.swf"
        },
        'aaData': dtData,
        "aoColumns": [

                                { "sTitle": "Company Name", "sClass": "sleft", "sWidth": "15%" },
                                { "sTitle": "Product Name", "sWidth": "15%", "sClass": "sleft" },
                                { "sTitle": "Batch", "sWidth": "10%", "sClass": "sleft" },
                                { "sTitle": "Date", "sWidth": "10%" },
                                { "sTitle": "Type", "sWidth": "10%" },
                                { "sTitle": "Dispensing Authority", "sWidth": "10%" },
                                { "sTitle": "Quantity", "sClass": "sright", "sWidth": "5%" },
                                { "sTitle": "Rate", "sClass": "sright", "sWidth": "5%" },
                                { "sTitle": "Total Amount", "sClass": "sright", "sWidth": "10%" },
                                { "sTitle": "Closing Stock", "sClass": "sright", "sWidth": "10%" }
        // { "sTitle": "Status", "sClass": "sright", "sWidth": "5%" }
                            ]
    });
}
function Load3cReport(TranType) {
    var PatientId = ($("#dddlPatient").jqxComboBox('getSelectedItem') != undefined && $("#dddlPatient").jqxComboBox('getSelectedItem') != null) ? $("#dddlPatient").jqxComboBox('getSelectedItem').value : "-1";
    var ProcedureId = ($("#pddlProcedure").jqxComboBox('getSelectedItem') != undefined && $("#pddlProcedure").jqxComboBox('getSelectedItem') != null) ? $("#pddlProcedure").jqxComboBox('getSelectedItem').value : "-1";
    var FromDate = document.getElementById('txtFromDate').value;
    var ToDate = document.getElementById('txtToDate').value;
    window.open("frmGenerateReports.aspx?PatientId=" + PatientId + "&cTranType=" + TranType + "&ProcedureId=" + ProcedureId + "&FromDate=" + FromDate + "&ToDate=" + ToDate, "Generate 3c Report");
}

function BindLocationDropDownList() {
    $.ajax({
        type: "POST",
        url: "WebService.asmx/GetMstClinic",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: '{}',
        error: function (ex) {
            console.log(ex);
        },
        success: function (data) {

            var data = jQuery.parseJSON(data.d);
            if (data != null) {
                var obj = new Object();
                obj.nClinicId = "-1";
                obj.vClinicName = "Select Clinic"
                data.push(obj);
                data.reverse();

                $("#cddlClinic").jqxComboBox({ source: data, selectedIndex: 0, width: '200', height: '20px', theme: theme, displayMember: "vClinicName", valueMember: "nClinicId" });
            }
            else {
                $("#cddlClinic").jqxComboBox('clear');
            }
        }
    });
}