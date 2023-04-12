var iUserId, activityDataSet, inDataSet, nTemplateHdrNo, b_View, type;
var CheckListTemplateHdrData, CheckListTemplateHdrAjaxData, CheckListTemplateDtlData, CheckListTemplateDtlAjaxData;
var TemplateData, TemplateAjaxData, AddTemplateAjaxData;
var CheckListQuestionTemplate, CheckListQuestionTemplateData, CheckListQuestionTemplateAjaxData;

$(function () {

    //Initialize Select2 Elements for AutoComplete
    $(".select2").select2();

    iUserId = $("#hdnuserid").val();
   
    CheckListTemplateHdrDetail();

    $("#btnAddTemplate").on('click', function (e) {
        if (ddlBlankCheck('ddlCheckListTemplate'))
        {
            AlertBox("warning", "CheckList Template!", "Please Select Template !");
            return false
        }

        type = 2,
        nTemplateHdrNo = $("#ddlCheckListTemplate").val();
        TemplateData = {
            nTemplateHdrNo: nTemplateHdrNo,
            type: type
        }       
        AddTemplateAjaxData = {
            async: false,
            data: TemplateData,
            type: 'POST',
            url: ApiURL + "GetData/CheckListTemplateDetail",
            success: successAddTemplate,
            error: errorAddTemplate
        }

        fnAddTemplate(AddTemplateAjaxData.url, AddTemplateAjaxData.type, AddTemplateAjaxData.data, AddTemplateAjaxData.async, AddTemplateAjaxData.success, AddTemplateAjaxData.error);

    });

    $("#btnAddQuestion").on('click', function (e) {
        if (blankCheck('txtCheckListQuestion')) {
            AlertBox("warning", "CheckList Template!", "Please Enter Question !");
            return false;
        }
        var tblCheckListTemplateQuestion = $("#tblCheckListTemplateQuestion").DataTable()
        var question = $("#txtCheckListQuestion").val();
        tblCheckListTemplateQuestion.row.add( [
            '','','',question,''
        ]).draw(false);
        $("#txtCheckListQuestion").val("");
    });

    $("#tbnSaveCheckListQuestionTemplate").on('click', function (e) {
        SaveCheckListQuestionTemplate();
    });

    $("#btnAddCheckListTemplate").on('click', function (e) {

    })

    $('#CheckListTemplateModel').on('shown.bs.modal', function () {
        $('#txtUserName').focus();
    })

    //$(".rGroup").change(function () {
    //    alert("show");
    //    if ($("#rdbNewTemplate").attr("checked")) {
    //        alert("rdbNewTemplate");
    //    }
    //});
    $("#rdbNewTemplate").prop("checked", true)
    //$("#rowNewTemplate").hide();

    $("#rdbNewTemplate").change(function () {
        $("#rowNewTemplate").hide();
        //$("#ddlCheckListTemplate").hide();
    });

    $("#rdbExistingTemplate").change(function () {
        $("#rowNewTemplate").show();
    });

    //$('#ddlCheckListTemplate').on('change keyup', function () {
    //    if ($('#ddlCheckListTemplate').val().length == 2) {
    //        var CheckListTemplateData = {
    //            vTemplateDesc: $('#ddlCheckListTemplate').val(),              
    //        }
    //        //GetPmsProjectNoProductReceipt(GetProjectNo.Url, GetProjectNo.SuccessMethod, ProjectNoDataTemp);
    //    }
    //    else if ($('#ddlToProjectNo').val().length < 2) {
    //        $("#ddlToProjectNo").autocomplete({
    //            source: "",
    //            change: function (event, ui) { }
    //        });
    //    }
    //});

 

});

function CheckListTemplateHdrDetail() {

    CheckListTemplateHdrAjaxData = {
        type: 1,
        nTemplateHdrNo: -999
    }
    CheckListTemplateHdrData = {
        async: false,
        type: 'POST',
        url: ApiURL + "GetData/CheckListTemplateDetail",
        data: CheckListTemplateHdrAjaxData,
        success: successCheckListTemplateHdrDetail,
        error: errorCheckListTemplateHdrDetail
    }
    GetCheckListTemplateHdrDetail(CheckListTemplateHdrData.url, CheckListTemplateHdrData.type, CheckListTemplateHdrData.data, CheckListTemplateHdrData.async, CheckListTemplateHdrData.success, CheckListTemplateHdrData.error);
}

var GetCheckListTemplateHdrDetail = function(url,type,data,async,success,error) {
    $.ajax({
        url: url,
        type: type,
        data: data,
        async: async,
        success: success,
        error: error
    });  
}

function successCheckListTemplateHdrDetail(jsonData) {
    activityDataSet = [];
    var sourceArr = [];
    if (jsonData.length > 0) {
        var ddlCheckListTemplate = document.getElementById('ddlCheckListTemplate')
        while (ddlCheckListTemplate.hasChildNodes()) {
            ddlCheckListTemplate.removeChild(ddlCheckListTemplate.lastChild);
        }
        $("#ddlCheckListTemplate").append($("<option></option>").html("select Template").val(""));
        for (var i = 0; i < jsonData.length; i++) {
            $("#ddlCheckListTemplate").append($("<option></option>").html(jsonData[i].vTemplateDesc).val(jsonData[i].nTemplateHdrNo));         
            //sourceArr.push("[ " + jsonData[i].vTemplateDesc + " ]" + " " + jsonData[i].nTemplateHdrNo);
            inDataSet = [];
            b_View = "";
            inDataSet.push(jsonData[i].nTemplateHdrNo, jsonData[i].vTemplateDesc, '', jsonData[i].changeOn,jsonData[i].iModifyBy, jsonData[i].dModifyOn);
            activityDataSet.push(inDataSet);
        }
        //$("#ddlCheckListTemplate").autocomplete({
        //    source: sourceArr,
        //    change: function (event, ui) { }
        //});
        otable = $("#tblCheckListTemplateHdrDetail").dataTable({
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
            "sScrollXInner": "1090",
            "fnCreatedRow": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                //$('td:eq(1)', nRow).append('<a data-toggle="modal" data-tooltip="tooltip" title="View" data-target="#CheckListTemplateDtlModel" attrid="' + aData[0] + '" class="" Onclick=GetCheckListTemplateDtlDetail(this) nTemplateHdrNo="' + aData[0] + '" style="cursor:pointer;"><i class="btn btn-primary btn-rounded btn-ef btn-ef-5 btn-ef-5a"><i class="fa fa-fw fa-file-text-o"></i><span>View</span></a>');
                $('td:eq(1)', nRow).append('<a href="" class="myIcon icon-success icon-ef-3 icon-ef-3a hover-color icon-center" data-toggle="modal" data-target="#CheckListTemplateDtlModel" data-tooltip="tooltip" title="View" attrid="' + aData[0] + '" Onclick=GetCheckListTemplateDtlDetail(this) nTemplateHdrNo="' + aData[0] + '" style="cursor:pointer;"><i class="fa  fa-info-circle"></i></a>');
            },
            "aoColumns": [
              { "sTitle": "Template No" },
              { "sTitle": "Template Detail" },
              { "sTitle": "View" },              
              { "sTitle": "Created By" },
              { "sTitle": "Created On" },
              { "sTitle": "Change On" },
              
            ],
            "columnDefs": [
              {
                  "targets": [0,4,5],
                  "visible": false,
                  "searchable": false
              },            
              { "bSortable": false, "targets": [2] },
            ],
            "oLanguage": {
                "sEmptyTable": "No Record Found",
            },

        });
    }
    else {
        AlertBox("information", "CheckList Template!", "No Data Available in CheckList Template!");
    }

}

function errorCheckListTemplateHdrDetail() {
    AlertBox("error", "CheckList Template!", "Error While Retriving CheckList Template Details!");
}

function GetCheckListTemplateDtlDetail(e) {
    nTemplateHdrNo = $(e).attr("nTemplateHdrNo");
    type = -999;

    CheckListTemplateDtlAjaxData = {
        type: type,
        nTemplateHdrNo: nTemplateHdrNo
    }

    CheckListTemplateDtlData = {
        async: false,
        type: 'POST',
        url: ApiURL + "GetData/CheckListTemplateDetail",
        data: CheckListTemplateDtlAjaxData,
        success: successCheckListTemplateDtlDetail,
        error: errorCheckListTemplateDtlDetail
    }

    fnGetCheckListTemplateDtlDetail(CheckListTemplateDtlData.url, CheckListTemplateDtlData.type, CheckListTemplateDtlData.data, CheckListTemplateDtlData.async, CheckListTemplateDtlData.success, CheckListTemplateDtlData.error)
}

var fnGetCheckListTemplateDtlDetail = function (url, type, data, async, success, error) {
    $.ajax({
        url: url,
        type: type,
        data: data,
        async: async,
        success: success,
        error: error
    });
}

function successCheckListTemplateDtlDetail(jsonData) {
    activityDataSet = [];
    if (jsonData.length > 0) {
        for (var i = 0; i < jsonData.length; i++) {
            inDataSet = [];
            inDataSet.push(jsonData[i].nTemplateHdrNo, jsonData[i].nTemplateDtlNo, jsonData[i].vTemplateDesc, jsonData[i].vQuestion);
            activityDataSet.push(inDataSet);
        }
        otable = $("#tblCheckListTemplateDtlDetail").dataTable({
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
            "sScrollXInner": "809",           
            "aoColumns": [
              { "sTitle": "Template Hdr No" },
              { "sTitle": "Template Dtl No" },
              { "sTitle": "Template Description" },
              { "sTitle": "Questions" }             
            ],
            "columnDefs": [
              {
                  "targets": [0,1,2],
                  "visible": false,
                  "searchable": false
              },
              { "bSortable": false, "targets": [0,1,2] },
            ],
            "oLanguage": {
                "sEmptyTable": "No Record Found",
            }
        });
    }
    else {
        AlertBox("information", "CheckList Template!", "No Data Available in CheckList Template!");
    }
}

function errorCheckListTemplateDtlDetail() {
    AlertBox("error", "CheckList Template!", "Error While Retriving CheckList Template Details!");
}

var fnAddTemplate = function (url, type, data, async, success, error) {
    $.ajax({
        url: url,
        type: type,
        data: data,
        async: async,
        success: success,
        error: error
    });
}

function successAddTemplate(jsonData) {
    activityDataSet = []
    if (jsonData.length > 0) {
        for (var i = 0; i < jsonData.length; i++) {
            inDataSet = [];
            inDataSet.push(jsonData[i].nTemplateHdrNo, jsonData[i].nTemplateDtlNo, jsonData[i].vTemplateDesc, jsonData[i].vQuestion, '');
            activityDataSet.push(inDataSet);
        }

        otable = $("#tblCheckListTemplateQuestion").dataTable({
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
            "sScrollXInner": "809",
            "fnCreatedRow": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                $('td:eq(1)', nRow).append('<a data-tooltip="tooltip" title="Delete" attrid="' + aData[0] + '" class="" Onclick=deleteCheckListQuestionTemplate(this) nTemplateHdrNo="' + aData[0] + '" style="cursor:pointer;"><i class="btn btn-primary btn-rounded btn-ef btn-ef-5 btn-ef-5a"><i class="fa fa-fw fa-file-text-o"></i><span>Delete</span></a>');
            },
            "aoColumns": [
              { "sTitle": "Template Hdr No" },
              { "sTitle": "Template Dtl No" },
              { "sTitle": "Template Description" },
              { "sTitle": "Questions" },
              { "sTitle": "Delete" }
            ],
            "columnDefs": [
              {
                  "targets": [0, 1, 2],
                  "visible": false,
                  "searchable": false
              },
              { "bSortable": false, "targets": [0, 1, 2] },
            ],
            "oLanguage": {
                "sEmptyTable": "No Record Found",
            }
        });
    }
    else {
        AlertBox("information", "CheckList Template!", "No Data Available in CheckList Template!");
    }
}

function errorAddTemplate(ex) {
    throw ex;
    AlertBox("error", "CheckList Template!", "Error While Retriving CheckList Template Details!");
}

function deleteCheckListQuestionTemplate(e) {    
    $('#tblCheckListTemplateQuestion').DataTable().row($(e).parents('tr')).remove().draw();
}

function SaveCheckListQuestionTemplate() {
    CheckListQuestionTemplate = [];
    CheckListQuestionTemplate = $('#tblCheckListTemplateQuestion').dataTable().fnGetData();

    CheckListQuestionTemplateData = {
        vQuestion: CheckListQuestionTemplate,
        iUserId: iUserId
    }

    CheckListQuestionTemplateAjaxData = {
        async: false,
        data: CheckListQuestionTemplateData,
        type: 'POST',
        url: ApiURL + "SetData/SaveCheckListQuestionTemplateDetails",
        success: successCheckListQuestionTemplateDetails,
        error: errorCheckListQuestionTemplateDetails
    }

    fnSaveCheckListQuestionTemplate(CheckListQuestionTemplateAjaxData.url,CheckListQuestionTemplateAjaxData.type,CheckListQuestionTemplateAjaxData.data,CheckListQuestionTemplateAjaxData.async,CheckListQuestionTemplateAjaxData.success,CheckListQuestionTemplateAjaxData.error)

}

var fnSaveCheckListQuestionTemplate = function (url, type, data, async, success, error) {
    $.ajax({
        url: url,
        type: type,
        data: data,
        async: async,
        success: success,
        error: error
    });
}

function successCheckListQuestionTemplateDetails(jsonData) {
    var result = jsonData
    if (result == 'success') {
        CheckListTemplateHdrDetail();
        $("#tblCheckListTemplateQuestion").DataTable().clear().draw();
        AlertBox("success", "CheckList Template!", "CheckList Question Template Details Saved Successfully!");

    }
    else {
        AlertBox("error", "CheckList Template!", "Error Occured While Saving CheckList Question Template Details!");
    }    
}

function errorCheckListQuestionTemplateDetails() {
    AlertBox("error", "CheckList Template!", "Error While Saving CheckList Question Template Details!");
}



