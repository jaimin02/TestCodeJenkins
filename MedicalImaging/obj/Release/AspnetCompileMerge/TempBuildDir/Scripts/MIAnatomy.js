$(function () {
    getAnatomy();
    getUserRights();

    $("#btnAddAnatomy").on('click', function (e) {
        $("#btnSaveAnatomy").text("Save Changes");
        $("#lblAnatomyRemarks").text("Anmatomy Remarks");
        $('.AuditControl').each(function () { this.style.display = "none" });
        $('.form-control').each(function () { $(this).attr('disabled', false) });
        $("#txtAnatomyName").val("")
        $("#txtAnatomyRemarks").val("")
        $("#btnSaveAnatomy").show();
        $("#btnSaveAnatomy").text("Save Changes")
    })

    $('#btnSaveAnatomy').on('click', function (e) {

        var btnval = document.getElementById("btnSaveAnatomy").innerText.toUpperCase().trim();
        if (btnval == "SAVE CHANGES") {
            if (blankCheck('txtAnatomyName')) {
                AlertBox("warning", "Anatomy Master!", "Please Enter Anatomy!");
                return false;
            }
            //if (specCheck('txtAnatomyName')) {
            //    AlertBox("warning", "Anatomy Master!", "Please Enter Valid Anatomy Name!");
            //    return false;
            //}
            //if (charCheck('txtAnatomyName')) {
            //    AlertBox("warning", "Anatomy Master!", "Please Enter Character Only!");
            //    return false;
            //}
            if (!dublicate()) {
            }
            else {
                var anatomyDesc = $('#txtAnatomyName').val();
                var anatomyRemarks = $('#txtAnatomyRemarks').val();

                var Data = {
                    vAnatomyDesc: anatomyDesc,
                    vRemarks: anatomyRemarks,
                    cStatusIndi: 'N',
                    iModifyBy: $("#hdnuserid").val()
                }

                var ajaxData = {
                    async: 'false',
                    data: Data,
                    type: 'POST',
                    //url: ApiURL + "SetData/PostAddAnatomy",
                    url: WebURL + "MIAnatomy/PostAddAnatomy",
                    success: successInsert,
                    error: errorInsert
                }
                InsertAnatomy(ajaxData.async, ajaxData.data, ajaxData.type, ajaxData.url, ajaxData.success, ajaxData.error)
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
                    var anatomyDesc = $('#txtAnatomyName').val();
                    var anatomyRemarks = $('#txtAnatomyRemarks').val();

                    var Data = {
                        nAnatomyNo: nAnatomyNo,
                        vAnatomyDesc: anatomyDesc,
                        vRemarks: anatomyRemarks,
                        cStatusIndi: 'E',
                        iModifyBy: $("#hdnuserid").val()
                    }

                    var ajaxData = {
                        async: 'false',
                        data: Data,
                        type: 'POST',
                        //url: ApiURL + "SetData/PostAddAnatomy",
                        url: WebURL + "MIAnatomy/PostAddAnatomy",
                        success: successInsert,
                        error: errorInsert
                    }
                    InsertAnatomy(ajaxData.async, ajaxData.data, ajaxData.type, ajaxData.url, ajaxData.success, ajaxData.error)
                }
            }
        }

        else if (btnval == "DELETE CHANGES") {
            if (blankCheck('txtAnatomyRemarks')) {
                AlertBox("warning", "Anatomy Master!", "Please Enter Anatomy Remarks to InActive!");
                return false;
            }
            else {
                var anatomyDesc = $('#txtAnatomyName').val();
                var anatomyRemarks = $('#txtAnatomyRemarks').val();

                var Data = {
                    nAnatomyNo: nAnatomyNo,
                    vAnatomyDesc: "",
                    vRemarks: anatomyRemarks,
                    cStatusIndi: 'D',
                    iModifyBy: $("#hdnuserid").val()
                }

                var ajaxData = {
                    async: 'false',
                    data: Data,
                    type: 'POST',
                    //url: ApiURL + "SetData/PostAddAnatomy",
                    url: WebURL + "MIAnatomy/PostAddAnatomy",
                    success: successInsert,
                    error: errorInsert
                }
                InsertAnatomy(ajaxData.async, ajaxData.data, ajaxData.type, ajaxData.url, ajaxData.success, ajaxData.error)
            }
        }
    })
});

function validate() {

    if (blankCheck('txtAnatomyName')) {
        AlertBox("warning", "Anatomy Master!", "Please Enter Anatomy!");
        return false;
    }
    //if (specCheck('txtAnatomyName')) {
    //    AlertBox("warning", "Anatomy Master!", "Please Enter Valid Anatomy Name!");
    //    return false;
    //}
    //if (charCheck('txtAnatomyName')) {
    //    AlertBox("warning", "Anatomy Master!", "Please Enter Character Only!");
    //    return false;
    //}
    if (blankCheck('txtAnatomyRemarks')) {
        AlertBox("warning", "Anatomy Master!", "Please Enter Anatomy Remarks!");
        return false;
    }
}

function dublicate() {
    if ($.fn.DataTable.isDataTable('#tblAnatomyMst')) {
        var AnatomyDesc = $("#txtAnatomyName").val().trim();
        var rows = $("#tblAnatomyMst").dataTable().fnGetNodes();
        for (i = 0; i < rows.length; i++) {
            if ($(rows[i]).find("td:eq(4)").attr("class") != "disabled") {
                if (AnatomyDesc.toUpperCase() == $(rows[i]).find("td:eq(0)").html().trim().toUpperCase()) {
                    AlertBox("warning", "Anatomy Master!", "This Anatomy Name Already Exists!");
                    return false;
                    break;
                }
            }
        }
        return true;
    }
    else {
        return true;
    }
   
}

function ClearData() {
    $('#txtAnatomyName').val("");
    $('#txtAnatomyRemarks').val("");
}

var InsertAnatomy = function (async, data, type, url, success, error) {
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

function successInsert(response) {
    var result = response.responseText;
    if (result != "") {
        if (result == "N") {
            AlertBox("success", "Anatomy Master!", "Anatomy Saved Successfully!");
        }
        else if (result == "E") {
            AlertBox("success", "Anatomy Master!", "Anatomy Edited Successfully!");
        }
        else if (result == "D") {
            AlertBox("success", "Anatomy Master!", "Anatomy InActive Successfully!");
        }
        getAnatomy();
        ClearData();
        $("#AnatomyModel").modal('hide');
    }
    else {
        AlertBox("error", "Anatomy Master!", "Error while Saving Anatomy Details!");
    }
}

function errorInsert(response) {
    AlertBox("error", "Anatomy Master!", "Error while Saving Anatomy Details!");
}

function getAnatomy() {

    var AnatomyAjax = {
        //url: ApiURL + "GetData/GetAnatomy",
        url: WebURL + "MIAnatomy/GetAnatomy",
        type: 'GET',
        async: false,
        data: '',
        success: successGetAnatomy,
        error: errorGetAnatomy
    }

    $.ajax({
        url: AnatomyAjax.url,
        type: AnatomyAjax.type,
        async: AnatomyAjax.async,
        data: AnatomyAjax.data,
        success: AnatomyAjax.success,
        error: AnatomyAjax.error
    });


    function successGetAnatomy(jsonData) {
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

                    //b_Edit = '<a data-tooltip="tooltip" data-toggle="modal" title="Edit" class="" attrid="' + jsonData[i].nAnatomyNo + '" data-target="#AnatomyModel" style="cursor:pointer;" Onclick=editAnatomy(this) id="' + jsonData[i].nAnatomyNo + '" desc="' + jsonData[i].vAnatomyDesc + '" remarks="' + jsonData[i].vRemarks + '"><i class="btn btn-primary btn-rounded btn-ef btn-ef-5 btn-ef-5a"><i class="fa fa-fw fa-edit"></i><span>Edit</span></a>';
                    //b_InActive = '<a data-tooltip="tooltip1" data-toggle="modal" title="In-Active" class="" data-target="#AnatomyModel" onClick=inActiveAnatomy(this) id="' + jsonData[i].nAnatomyNo + '" desc="' + jsonData[i].vAnatomyDesc + '" remarks="' + jsonData[i].vRemarks + '"><i class="btn btn-primary btn-rounded btn-ef btn-ef-5 btn-ef-5a"><i class="glyphicon glyphicon-remove"></i><span>In-Active</span></a>';

                    inDataSet.push(jsonData[i].nAnatomyNo, jsonData[i].vAnatomyDesc, jsonData[i].vRemarks, jsonData[i].iModifyBy, jsonData[i].dModifyOn, jsonData[i].cStatusIndi, '', '', '');
                    activityDataSet.push(inDataSet);
                }

                otable = $("#tblAnatomyMst").dataTable({
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
                    //"sScrollXInner": "1250",
                    "fnCreatedRow": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                        //$('td:eq(2)', nRow).append('<a data-toggle="modal" data-tooltip="tooltip" title="Audit Trail" data-target="#AnatomyModel" attrid="' + aData[0] + '" class="" Onclick=AuditTrailAnatomy(this) id="' + aData[0] + '" Desc="' + aData[1] + '" style="cursor:pointer;"><i class="btn btn-primary btn-rounded btn-ef btn-ef-5 btn-ef-5a"><i class="fa fa-fw fa-file-text-o"></i><span>Audit</span></a>');
                        $('td:eq(2)', nRow).append('<a data-toggle="modal" data-tooltip="tooltip" title="Audit Trail" data-target="#AnatomyModel" attrid="' + aData[0] + '" class="myIcon icon-success icon-ef-3 icon-ef-3a hover-color icon-center" Onclick=AuditTrailAnatomy(this) id="' + aData[0] + '" Desc="' + aData[1] + '" style="cursor:pointer;"><i class="fa fa-file-text-o"></i><span>Audit</span></a>');
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
                            //$('td:eq(3)', nRow).append('<a data-toggle="modal"  title="Edit" data-target="#AnatomyModel" attrid="' + aData[0] + '" class="" Onclick=editAnatomy(this) id="' + aData[0] + '" desc="' + aData[1] + '" remarks="' + aData[2] + '" style="cursor:pointer;"><i class="btn btn-primary btn-rounded btn-ef btn-ef-5 btn-ef-5a"><i class="fa fa-fw fa-file-text-o"></i><span>Edit</span></a>');
                            $('td:eq(3)', nRow).append('<a class="myIcon icon-success icon-ef-3 icon-ef-3a hover-color icon-center" data-toggle="modal"  title="Edit" data-target="#AnatomyModel" attrid="' + aData[0] + '" class="" Onclick=editAnatomy(this) id="' + aData[0] + '" desc="' + aData[1] + '" remarks="' + aData[2] + '" style="cursor:pointer;"><i class="fa fa-edit"></i><span>Edit</span></a>');
                            //$('td:eq(4)', nRow).append('<a data-toggle="modal"  title="InActive" data-target="#AnatomyModel" attrid="' + aData[0] + '" class="" Onclick=inActiveAnatomy(this) id="' + aData[0] + '" desc="' + aData[1] + '" remarks="' + aData[2] + '" style="cursor:pointer;"><i class="btn btn-primary btn-rounded btn-ef btn-ef-5 btn-ef-5a"><i class="fa fa-fw fa-file-text-o"></i><span>InActive</span></a>');
                            $('td:eq(4)', nRow).append('<a class="myIcon icon-success icon-ef-3 icon-ef-3a hover-color icon-center" data-toggle="modal"  title="InActive" data-target="#AnatomyModel" attrid="' + aData[0] + '" class="" Onclick=inActiveAnatomy(this) id="' + aData[0] + '" desc="' + aData[1] + '" remarks="' + aData[2] + '" style="cursor:pointer;"><i class="fa fa-ban"></i><span>InActive</span></a>');
                        }
                    },
                    "aoColumns": [
                       { "sTitle": "Anatomy No" },
                       { "sTitle": "Anatomy Name" },
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
                AlertBox("Information", "Anatomy Master!", "No Data Available in anatomy Master!")
            }
        }
    };

    function errorGetAnatomy() {
        AlertBox("error", "Anatomy Master!", "Error While Retriving Anatomy Master Details!");
    };
};

function editAnatomy(e) {
    nAnatomyNo = $(e).attr("id");
    nAnatomyDesc = $(e).attr("desc");
    vRemarks = $(e).attr("remarks");

    $("#btnSaveAnatomy").show();
    $('.AuditControl').each(function () { this.style.display = "none" });
    $('.form-control').each(function () { $(this).attr('disabled', false) });

    $("#txtAnatomyName").val(nAnatomyDesc);
    $("#txtAnatomyRemarks").val(vRemarks);
    $("#lblAnatomyRemarks").text("Anatomy Remarks*");
    $("#btnSaveAnatomy").text("Update Changes")
}

function inActiveAnatomy(e) {
    nAnatomyNo = $(e).attr("id");
    vAnatomyDesc = $(e).attr("desc");
    vRemarks = $(e).attr("remarks");

    $("#btnSaveAnatomy").show();
    $('.AuditControl').each(function () { this.style.display = "none" });

    $("#txtAnatomyName").val(vAnatomyDesc);
    $("#txtAnatomyName").attr('disabled', true);
    $("#txtAnatomyRemarks").val(vRemarks);
    $("#lblAnatomyRemarks").text("Anatomy Remarks");
    $("#txtAnatomyRemarks").attr('disabled', false);
    $("#btnSaveAnatomy").text("Delete Changes")
}

function AuditTrailAnatomy(e) {
    $('.AuditControl').each(function () { this.style.display = "inline" });
    $('.form-control').each(function () { $(this).attr("disabled", true) });
    $("#btnSaveAnatomy").hide();
    $("#lblAnatomyRemarks").text("Anatomy Remarks");

    nAnatomyNo = $(e).attr("id");

    var anatomyData = {
        nAnatomyNo: nAnatomyNo
    }

    var ajaxData = {
        //url: ApiURL + "GetData/GetAnatomyAuditTrail?nAnatomyNo=" + nAnatomyNo + "",
        //url: WebURL + "MIAnatomy/GetAnatomyAuditTrail?nAnatomyNo=" + nAnatomyNo + "",
        url: WebURL + "MIAnatomy/GetAnatomyAuditTrail",
        type: 'GET',
        data :anatomyData,
        success: successGetAnatomyAuditTrail,
        error: errorGetAnatomyAuditTrail
    }

    $.ajax({
        url: ajaxData.url,
        type: ajaxData.type,
        async: false,
        data : ajaxData.data,
        success: ajaxData.success,
        error: ajaxData.error
    });
}

function AuditTrail(e) {
    var id = e.id;
    var title = $(e).attr("titlename");
    var vFieldName = id.substring(3);
    var nAnatomyNo = $("#hdnnAnatomyNo").val();
    var vTableName = "AnatomyMstHst";
    var vIdName = "nAnatomyNo";
    var vIdValue = nAnatomyNo;
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
        url: WebURL + "MIAnatomy/AuditTrail",
        data: data,
        success: successAuditTrail,
        error: errorAuditTrail
    });

    function successAuditTrail(jsonData) {
        debugger;
        if (jsonData == "error") {
            AlertBox("error", "Anatomy Master!", "Error While Getting Audit Detail!")
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
                otable = $('#tblAnatomyAuditTrail').dataTable({
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
            }
            else {
                AlertBox("information", "Anatomy Master!", "No Field Level Audit Trail Found!");
            }
        }
    }
    function errorAuditTrail() {
        AlertBox("error", "Anatomy Master!", "Error While Retriving Field Level Audit Trail Data!");
    }

}

function successGetAnatomyAuditTrail(jsonData) {
    if (jsonData == "error") {
        AlertBox("error", "Anatomy Master!", "Error While Getting Audit Detail!")
    }
    else {
        jsonData = JSON.parse(jsonData)
        if (jsonData.length > 0) {
            $("#hdnnAnatomyNo").val(jsonData[0].nAnatomyNo)
            $("#txtAnatomyName").val(jsonData[0].vAnatomyDesc)
            $("#txtAnatomyRemarks").val(jsonData[0].vRemarks)
        }
        else {
            AlertBox("information", "Anatomy Master!", "No Data Found For Audit Trail!");
        }
    }
}

function errorGetAnatomyAuditTrail() {
    AlertBox("error", "Anatomy Master!", "Error While Retriving Anatomy Master Audit Trail Details!");
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