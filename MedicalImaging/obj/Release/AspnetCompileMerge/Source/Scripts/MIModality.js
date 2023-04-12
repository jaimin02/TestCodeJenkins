var userViewMode = "";
var nModalityNo = ""
var vModalityDesc = ""
var vRemarks = ""
var iUserId;
$(function () {

    //$(document).mousedown(function (e) {
    //    if (e.button == 2) {
    //        $(this).bind("contextmenu", function (e) {
    //            //alert('Not Allowed!');
    //            e.preventDefault();
    //        });
    //        //e.preventDefault();
    //        //var v = true;
    //        //alert('Not Allowed!');
    //        //return false;
    //    }
    //    return true;
    //});
    //$(document).keydown(function (e) {

    //    var map = {};
    //    map[e.keyCode] = e.type == 'keydown';

    //    if ((map[17]) || (map[17] && map[73]) || (map[17] && map[16] && map[73]))
    //    {
    //        alert("Not Allowed");
    //        return false;
    //        //e.keyCode[17].preventDefault();            
    //    }

    //    if (e.which === 123) {
    //        //alert('Not Allowed!');
    //        return false;
    //    }        
    //});

    //function test_key(selkey) {
    //    var alias = {
    //        "ctrl": 17,
    //        "shift": 16,
    //        "A": 65,
    //        /* ... */
    //    };

    //    return key[selkey] || key[alias[selkey]];
    //}

    //function test_keys() {
    //    var keylist = arguments;

    //    for (var i = 0; i < keylist.length; i++)
    //        if (!test_key(keylist[i]))
    //            return false;

    //    return true;
    //}








    iUserId = $("#hdnuserid").val();

    GetModality();

    getUserRights();

    $("#btnAddModality").on('click', function (e) {
        $("#btnSaveModality").show();
        $("#btnSaveModality").text("Save Changes");
        $("#lblModalityInstructions").text("Modality Remarks");
        $('.AuditControl').each(function () { this.style.display = "none" });
        $('.form-control').each(function () { $(this).attr('disabled', false) });
        $("#txtModalityName").val("")
        $("#txtModalityInstructions").val("")
    });

    $('#btnSaveModality').on('click', function (e) {
        debugger;
        var btnval = document.getElementById("btnSaveModality").innerText.toUpperCase().trim();
        if (btnval == "SAVE CHANGES") {
            if (blankCheck('txtModalityName')) {
                AlertBox("warning", "Modality Master!", "Please Enter Modality!");
                return false;
            }
            //if (specCheck('txtModalityName')) {
            //    AlertBox("warning", "Modality Master!", "Please Enter Valid Modality Name!");                
            //    return false;
            //}
            //if (charCheck('txtModalityName')) {
            //    AlertBox("warning", "Modality Master!", "Please Enter Character Only!");             
            //    return false;
            //}
            if (!dublicate()) {
            }
            else {
                var modalityDesc = $('#txtModalityName').val();
                var modalityRemarks = $('#txtModalityInstructions').val();

                var Data = {
                    vModalityDesc: modalityDesc,
                    vRemarks: modalityRemarks,
                    cStatusIndi: 'N',
                    iModifyBy: $("#hdnuserid").val()

                }

                var ajaxData = {
                    async: 'false',
                    data: Data,
                    type: 'POST',
                    //url: ApiURL + "SetData/PostAddModality",
                    url: WebURL + "MIModality/PostAddModality",
                    success: successInsert,
                    error: errorInsert
                }
                InsertModality(ajaxData.async, ajaxData.data, ajaxData.type, ajaxData.url, ajaxData.success, ajaxData.error)
            }
        }

        else if (btnval == "UPDATE CHANGES") {
            if (validate() == false) {
                return false;
            }
            else {
                if (!dublicate()) {
                }
                else {
                    var modalityDesc = $('#txtModalityName').val();
                    var modalityRemarks = $('#txtModalityInstructions').val();

                    var Data = {
                        nModalityNo: nModalityNo,
                        vModalityDesc: modalityDesc,
                        vRemarks: modalityRemarks,
                        cStatusIndi: 'E',
                        iModifyBy: $("#hdnuserid").val()
                    }

                    var ajaxData = {
                        async: 'false',
                        data: Data,
                        type: 'POST',
                        //url: ApiURL + "SetData/PostAddModality",
                        url: WebURL + "MIModality/PostAddModality",
                        success: successInsert,
                        error: errorInsert
                    }
                    InsertModality(ajaxData.async, ajaxData.data, ajaxData.type, ajaxData.url, ajaxData.success, ajaxData.error)
                }
            }
        }

        else if (btnval == "DELETE CHANGES") {
            if (blankCheck('txtModalityInstructions')) {
                AlertBox("warning", "Modality Master!", "Please Enter Modality Remarks to InActive!");
                return false;
            }
            else {
                var modalityDesc = $('#txtModalityName').val();
                var modalityRemarks = $('#txtModalityInstructions').val();

                var Data = {
                    nModalityNo: nModalityNo,
                    vModalityDesc: "",
                    vRemarks: modalityRemarks,
                    cStatusIndi: 'D',
                    iModifyBy: $("#hdnuserid").val()
                }

                var ajaxData = {
                    async: 'false',
                    data: Data,
                    type: 'POST',
                    //url: ApiURL + "SetData/PostAddModality",
                    url: WebURL + "MIModality/PostAddModality",
                    success: successInsert,
                    error: errorInsert
                }
                InsertModality(ajaxData.async, ajaxData.data, ajaxData.type, ajaxData.url, ajaxData.success, ajaxData.error)
            }
        }
    })
});

function validate() {

    if (blankCheck('txtModalityName')) {
        AlertBox("warning", "Modality Master!", "Please Enter Modality!");
        return false;
    }
    //if (specCheck('txtModalityName')) {
    //    AlertBox("warning", "Modality Master!", "Please Enter Valid Modality Name!");     
    //    return false;
    //}
    //if (charCheck('txtModalityName')) {
    //    AlertBox("warning", "Modality Master!", "Please Enter Character Only!");      
    //    return false;
    //}
    if (blankCheck('txtModalityInstructions')) {
        AlertBox("warning", "Modality Master!", "Please Enter Modality Remarks!");
        return false;
    }
}

function dublicate() {
    if ($.fn.DataTable.isDataTable('#tblModalityMst')) {
        if ($("#tblModalityMst").dataTable().fnSettings().aoData.length === 0) {
            $("#tblModalityMst").dataTable({
                "aaSorting": false,
                "order": [[5, "asc"]],
            });
            return false;
        }
        else {
            var ModalityDesc = $("#txtModalityName").val().trim();
            var rows = $("#tblModalityMst").dataTable().fnGetNodes();
            for (i = 0; i < rows.length; i++) {
                if ($(rows[i]).find("td:eq(4)").attr("class") != "disabled") {
                    if (ModalityDesc.toUpperCase() == $(rows[i]).find("td:eq(0)").html().trim().toUpperCase()) {
                        AlertBox("warning", "Modality Master!", "This Modality Name Already Exists!");
                        return false;
                        break;
                    }
                }
            }
            return true;
        }
    }
    else {
        return true;
    }
}

var InsertModality = function (async, data, type, url, success, error) {
    $.ajax({
        async: false,
        data: data,
        type: type,
        url: url,
        complete: success,
        error: function (e) {
            throw e;
        }
    });
}

function ClearData() {
    $('#txtModalityName').val("");
    $('#txtModalityInstructions').val("");
}

function successInsert(response) {
    debugger;
    var result = response.responseText;
    if (result != "") {
        if (result == "N") {
            AlertBox("success", "Modality Master!", "Modality Saved Successfully!");
        }
        else if (result == "E") {
            AlertBox("success", "Modality Master!", "Modality Edited Successfully!");
        }
        else if (result == "D") {
            AlertBox("success", "Modality Master!", "Modality InActive Successfully!");
        }
        GetModality();
        ClearData();
        $("#ModalityModel").modal('hide');
    }
    else {
        AlertBox("error", "Modality Master!", "Error while Saving Modality Details!");
    }
    
}

function errorInsert(response) {
    AlertBox("error", "Modality Master!", "Error while Saving Modality Details!");
}

function GetModality() {

    var ajaxData = {
        //url: ApiURL + "GetData/GetModality",
        url: WebURL + "MIModality/GetModality",
        success: successGetModality,
        type: 'GET'
    }

    $.ajax({
        url: ajaxData.url,
        type: ajaxData.type,
        async: 'false',
        data: '',
        success: ajaxData.success,
        error: errorGetModality
    });

}

function successGetModality(jsonData) {
    var activityDataSet = [];
    if (jsonData == "error") {
        AlertBox("error", "Modality Master!", "Error while getting Data Available From Modality Master!");
    }
    else {
        jsonData = JSON.parse(jsonData);
        if (jsonData.length > 0) {
            for (var i = 0; i < jsonData.length; i++) {
                var inDataSet = [];
                var b_Edit = "";
                var b_InActive = "";

                //if (userViewMode = "viewOnly") {
                //    b_Edit = '<a class="disabled" title="Edit" data-tooltip="tooltip"><i class="btn btn-primary btn-rounded btn-ef btn-ef-5 btn-ef-5a"><i class="fa fa-fw fa-edit"></i><span>Edit</span></a>';
                //    b_InActive = '<a class="disabled" title="In-Active" data-tooltip="tooltip"><i class="btn btn-primary btn-rounded btn-ef btn-ef-5 btn-ef-5a"><i class="glyphicon glyphicon-remove"></i><span>InActive</span></a>';
                //}
                //else {
                //    b_Edit = '<a data-tooltip="tooltip" data-toggle="modal" title="Edit" class="" attrid="'+jsonData[i].nModalityNo+'" data-target="#modalityModel" style="cursor:pointer;" Onclick=editModality(this) id="' + jsonData[i].nModalityNo + '" desc="' + jsonData[i].vModalityDesc + '"><i class="btn btn-primary btn-rounded btn-ef btn-ef-5 btn-ef-5a"><i class="fa fa-fw fa-edit"></i><span>Edit</span></a>';
                //    b_InActive = '<a data-tooltip="tooltip1" data-toggle="modal" title="In-Active" class="clsEdit" onClick=inActiveModality(this) id="'+jsonData[i].nModalityNo+'" desc="'+jsonData.vModalityDesc+'" ><i class="btn btn-primary btn-rounded btn-ef btn-ef-5 btn-ef-5a"><i class="glyphicon glyphicon-remove"></i> <span>In-Active</span></a>'
                //}

                //b_Edit = '<a data-tooltip="tooltip" data-toggle="modal" title="Edit" class="" attrid="' + jsonData[i].nModalityNo + '" data-target="#ModalityModel" style="cursor:pointer;" Onclick=editModality(this) id="' + jsonData[i].nModalityNo + '" desc="' + jsonData[i].vModalityDesc + '" remarks="'+jsonData[i].vRemarks+'"><i class="btn btn-primary btn-rounded btn-ef btn-ef-5 btn-ef-5a"><i class="fa fa-fw fa-edit"></i><span>Edit</span></a>';
                //b_InActive = '<a data-tooltip="tooltip1" data-toggle="modal" title="In-Active" class="clsEdit" data-target="#ModalityModel" onClick=inActiveModality(this) id="' + jsonData[i].nModalityNo + '" desc="' + jsonData[i].vModalityDesc + '" remarks="'+jsonData[i].vRemarks+'"><i class="btn btn-primary btn-rounded btn-ef btn-ef-5 btn-ef-5a"><i class="glyphicon glyphicon-remove"></i> <span>In-Active</span></a>'

                inDataSet.push(jsonData[i].nModalityNo, jsonData[i].vModalityDesc, jsonData[i].vRemarks, jsonData[i].iModifyBy, jsonData[i].dModifyOn, jsonData[i].cStatusIndi, '', '', '')
                activityDataSet.push(inDataSet);
            }

            otable = $('#tblModalityMst').dataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": true,
                "autoWidth": false,
                "aaData": activityDataSet,
                "bInfo": true,
                "bDestroy": true,
                "sScrollX": "100%",
                "scrollY": "300px",
                "scrollCollapse": true,
                //"sScrollXInner": "1250", /* It varies dynamically if number of columns increases */           
                "fnCreatedRow": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    //$('td:eq(2)', nRow).append('<a data-toggle="modal" data-tooltip="tooltip" title="Audit Trail" data-target="#ModalityModel" attrid="' + aData[0] + '" class="" Onclick=AuditTrailModality(this) id="' + aData[0] + '" Desc="' + aData[1] + '" style="cursor:pointer;"><i class="btn btn-primary btn-rounded btn-ef btn-ef-5 btn-ef-5a"><i class="fa fa-fw fa-file-text-o"></i><span>Audit</span></a>');
                    $('td:eq(2)', nRow).append('<a data-toggle="modal" data-tooltip="tooltip" title="Audit Trail" data-target="#ModalityModel" attrid="' + aData[0] + '" class="myIcon icon-success icon-ef-3 icon-ef-3a hover-color icon-center" Onclick=AuditTrailModality(this) id="' + aData[0] + '" Desc="' + aData[1] + '" style="cursor:pointer;"><i class="fa fa-file-text-o"></i><span>Audit</span></a>');
                    //$('td:eq(2)', nRow).append('<a href="" class="myIcon icon-success icon-ef-3 icon-ef-3a hover-color icon-center" data-toggle="modal" data-tooltip="tooltip" title="Audit Trail" data-target="#ModalityModel" attrid="' + aData[0] + '"  Onclick=AuditTrailModality(this) id="' + aData[0] + '" Desc="' + aData[1] + '" style="cursor:pointer;><i class="fa  fa-info-circle"></i></a>');
                    if (aData[5] == 'D') {
                        $(nRow).addClass('reviewed');
                        $('td', nRow).eq(2).attr("disabled", "disabled");
                        $('td', nRow).eq(4).addClass('disabled');
                        //$('td:eq(3)', nRow).append('<a  data-tooltip="tooltip" title="Edit"  style="cursor:pointer;"><i class="btn btn-primary btn-rounded btn-ef btn-ef-5 btn-ef-5a" disabled="disabled"><i class="fa fa-fw fa-file-text-o" ></i><span>Edit</span></a>');
                        $('td:eq(3)', nRow).append('<a class="myIcon icon-success icon-ef-3 icon-ef-3a hover-color icon-center" data-tooltip="tooltip" title="Edit"  style="cursor:pointer;"><div class="" disabled="disabled"></div><i class="fa fa-edit" ></i><span>Edit</span></a>');
                        //$('td:eq(4)', nRow).append('<a  data-tooltip="tooltip" title="InActive"  style="cursor:pointer;"><i class="btn btn-primary btn-rounded btn-ef btn-ef-5 btn-ef-5a" disabled="disabled"><i class="fa fa-fw fa-file-text-o" ></i><span>InActive</span></a>');
                        $('td:eq(4)', nRow).append('<a  class="myIcon icon-success icon-ef-3 icon-ef-3a hover-color icon-center" data-tooltip="tooltip" title="InActive"  style="cursor:pointer;"><div class="" disabled="disabled"></div><i class="fa fa-ban" ></i><span>InActive</span></a>');
                    }
                    else {
                        $(nRow).addClass('original');
                        //$('td:eq(3)', nRow).append('<a data-toggle="modal"  title="Edit" data-target="#ModalityModel" attrid="' + aData[0] + '" class="" Onclick=editModality(this) id="' + aData[0] + '" desc="' + aData[1] + '" remarks="' + aData[2] + '" style="cursor:pointer;"><i class="btn btn-primary btn-rounded btn-ef btn-ef-5 btn-ef-5a"><i class="fa fa-fw fa-file-text-o"></i><span>Edit</span></a>');
                        $('td:eq(3)', nRow).append('<a class="myIcon icon-success icon-ef-3 icon-ef-3a hover-color icon-center" data-toggle="modal"  title="Edit" data-target="#ModalityModel" attrid="' + aData[0] + '" class="" Onclick=editModality(this) id="' + aData[0] + '" desc="' + aData[1] + '" remarks="' + aData[2] + '" style="cursor:pointer;"><i class="fa fa-edit"></i><span>Edit</span></a>');
                        //$('td:eq(4)', nRow).append('<a data-toggle="modal"  title="InActive" data-target="#ModalityModel" attrid="' + aData[0] + '" class="" Onclick=inActiveModality(this) id="' + aData[0] + '" desc="' + aData[1] + '" remarks="' + aData[2] + '" style="cursor:pointer;"><i class="btn btn-primary btn-rounded btn-ef btn-ef-5 btn-ef-5a"><i class="fa fa-fw fa-file-text-o"></i><span>InActive</span></a>');
                        $('td:eq(4)', nRow).append('<a class="myIcon icon-success icon-ef-3 icon-ef-3a hover-color icon-center" data-toggle="modal"  title="InActive" data-target="#ModalityModel" attrid="' + aData[0] + '" class="" Onclick=inActiveModality(this) id="' + aData[0] + '" desc="' + aData[1] + '" remarks="' + aData[2] + '" style="cursor:pointer;"><i class="fa fa-ban"></i><span>InActive</span></a>');
                    }
                },
                "aoColumns": [
                    { "sTitle": "Modality No" },
                    { "sTitle": "Modality Name" },
                    { "sTitle": "Remarks" },
                    { "sTitle": "Modify By" },
                    { "sTitle": "Modify On" },
                    { "sTitle": "cstatusIndi" },
                    { "sTitle": "Audit Trail" },
                    { "sTitle": "Edit" },
                    { "sTitle": "In-Active" },
                ],
                "columnDefs": [
                    {
                        "targets": [0, 3, 4, 5],
                        "visible": false,
                        "searchable": false
                    },

                    { "width": "10%", "targets": 1 },
                    { "width": "20%", "targets": 2 },
                    { "width": "5%", "targets": 6 },
                    { "width": "5%", "targets": 7 },
                    { "width": "5%", "targets": 8 },
                    { "bSortable": false, "targets": [6, 7, 8] },
                ],
                "oLanguage": {
                    "sEmptyTable": "No Record Found",
                },
            });
        }

        else {
            AlertBox("information", "Modality Master!", "No Data Available in Modality Master!");
        }
    }

}

function errorGetModality() {
    AlertBox("error", "Modality Master!", "Error While Retriving Modality Master Details!");
}

function editModality(e) {
    nModalityNo = $(e).attr("id");
    nModalityDesc = $(e).attr("desc");
    vRemarks = $(e).attr("remarks");

    $("#btnSaveModality").show();
    $('.AuditControl').each(function () { this.style.display = "none" });
    $('.form-control').each(function () { $(this).attr('disabled', false) });

    $("#txtModalityName").val(nModalityDesc);
    $("#txtModalityInstructions").val(vRemarks);
    $("#lblModalityInstructions").text("Modality Remarks*");
    $("#btnSaveModality").text("Update Changes")
}

function inActiveModality(e) {
    nModalityNo = $(e).attr("id");
    vModalityDesc = $(e).attr("desc");
    vRemarks = $(e).attr("remarks");

    $("#btnSaveModality").show();
    $('.AuditControl').each(function () { this.style.display = "none" });

    $("#txtModalityName").val(vModalityDesc);
    $("#txtModalityName").attr('disabled', true);
    $("#txtModalityInstructions").val(vRemarks);
    $("#lblModalityInstructions").text("Modality Remarks");
    $("#txtModalityInstructions").attr('disabled', false);
    $("#btnSaveModality").text("Delete Changes")
}

function AuditTrailModality(e) {
    $('.AuditControl').each(function () { this.style.display = "inline" });
    $('.form-control').each(function () { $(this).attr("disabled", true) });
    $("#btnSaveModality").hide();
    $("#lblModalityInstructions").text("Modality Remarks");

    nModalityNo = $(e).attr("id");

    var modalityData = {
        nModalityNo: nModalityNo
    }

    var ajaxData = {
        //url: ApiURL + "GetData/GetModalityAuditTrail?nModalityNo=" + nModalityNo + "",
        url: WebURL + "MIModality/GetModalityAuditTrail",
        type: 'GET',
        data:modalityData,
        success: successGetModalityAuditTrail,
        error: errorGetModalityAuditTrail
    }

    $.ajax({
        url: ajaxData.url,
        type: ajaxData.type,
        async: false,
        data:ajaxData.data,
        success: ajaxData.success,
        error: ajaxData.error
    });
}

function successGetModalityAuditTrail(jsonData) {
    if (jsonData == "error") {
        AlertBox("error", "Modality Master!", "Error While Getting Audit Detail!")
    }
    else {
        jsonData = JSON.parse(jsonData);
        if (jsonData.length > 0) {
            $("#hdnnModalityNo").val(jsonData[0].nModalityNo)
            $("#txtModalityName").val(jsonData[0].vModalityDesc)
            $("#txtModalityInstructions").val(jsonData[0].vRemarks)
        }
        else {
            AlertBox("information", "Modality Master!", "No Data Found For Audit Trail!");
        }
    }
}

function AuditTrail(e) {
    var id = e.id;
    var title = $(e).attr("titlename");
    var vFieldName = id.substring(3);
    var nModalityNo = $("#hdnnModalityNo").val();
    var vTableName = "ModalityMstHst";
    var vIdName = "nModalityNo";
    var vIdValue = nModalityNo;
    var iUserId = iUserId;

    var data = {
        vTableName: vTableName,
        vIdName: vIdName,
        vIdValue: vIdValue,
        vFieldName: vFieldName,
        iUserId: iUserId
    }

    var ajaxData = $.ajax({
        type: 'POST',
        //url: ApiURL + "CommonData/AuditTrail",
        url: WebURL + "MIModality/AuditTrail",
        data: data,
        success: successAuditTrail,
        error: errorAuditTrail
    });

    function successAuditTrail(jsonData) {
        if (jsonData == "error") {
            AlertBox("error", "Modality Master!", "Error While Getting Audit Detail!")
        }
        else {
            jsonData = JSON.parse(jsonData)
            if (jsonData.length > 0) {
                var activityDataSet = [];
                for (var i = 0; i < jsonData.length; i++) {
                    var inDataSet = [];
                    inDataSet.push(jsonData[i].vFieldName, jsonData[i].Operation, title + ' ' + jsonData[i].vDescription, jsonData[i].vRemarks, jsonData[i].vModifyBy, jsonData[i].dModifyOn);
                    activityDataSet.push(inDataSet);
                }
                otable = $('#tblModalityAuditTrail').dataTable({
                    "bJQueryUI": true,
                    "sPaginationType": "full_numbers",
                    "bLengthChange": true,
                    "iDisplayLength": 10,
                    "bProcessing": true,
                    "bSort": false,
                    "autoWidth": false,
                    "aaData": activityDataSet,
                    "bInfo": true,
                    "bDestroy": true,
                    //"scrollY": "30%",
                    //"scrollCollapse": true,
                    "scrollY": "150px",
                    "scrollCollapse": true,
                    "aoColumns": [
                        { "sTitle": "" + title + "" },
                        { "sTitle": "Operations" },
                        { "sTitle": "Description" },
                        { "sTitle": "Remarks" },
                        { "sTitle": "Modify by" },
                        { "sTitle": "Modify On" }
                    ],
                    "columnDefs": [
                        { "width": "20%", "targets": 1 },
                        { "width": "20%", "targets": 2 },
                        { "width": "20%", "targets": 3 },
                        { "width": "20%", "targets": 4 },
                        { "width": "20%", "targets": 5 },
                    ],
                    "oLanguage": {
                        "sEmptyTable": "No Record Found",
                    },
                });
                //setTimeout(function () { $('#tblModalityAuditTrail').dataTable().fnAdjustColumnSizing(); }, 1);
            }
            else {
                AlertBox("information", "Modality Master!", "No Field Level Audit Trail Found!");
            }
        }
    }
    function errorAuditTrail() {
        AlertBox("error", "Modality Master!", "Error While Retriving Field Level Audit Trail Data!");
    }
}

function errorGetModalityAuditTrail() {
    AlertBox("error", "Modality Master!", "Error While Retriving Modality Master Audit Trail Details!");
}

function getUserRights() {
    var userRightsDetail = $("#hdnViewModeID").val().split(",");

    for (var i = 0; i < userRightsDetail.length; i++) {
        if ($("#hdnUserTypeCode").val().trim() == userRightsDetail[i]) {
            userViewMode = "viewOnly";
            break;
        }
        else {
            userViewMode = "All";
        }
    }
}
