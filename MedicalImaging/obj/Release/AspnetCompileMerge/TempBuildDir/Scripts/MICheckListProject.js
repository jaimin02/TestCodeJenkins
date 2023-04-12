var vProjectNo, cProjectFilter;
var projectData, projectAjaxData;
var sourceArrayN //sourceArrayName;
var sourceArrayV = new Object(); //sourceArrayValue
var templateData, templateAjaxData;
var CheckListTemplateHdrData, CheckListTemplateHdrAjaxData;
var TemplateData, TemplateAjaxData, AddTemplateAjaxData;
var iUserId, activityDataSet, inDataSet, nTemplateHdrNo, b_View, type;
var vWorkSpaceID;
var CheckListQuestionTemplate, CheckListProjectTemplateData, CheckListProjectTemplateAjaxData;

$(function () {
    //Initialize Select2 Elements for AutoComplete
    $(".select2").select2();

    iUserId = $("#hdnuserid").val();

    projectDetails();

    CheckListTemplateHdrDetail();

    $("#btnAddCheckListProjectTemplate").on('click', function (e) {
        AddCheckListProjectTemplate();
    });

    $("#btnCancelCheckListProjectTemplate").on('click', function (e) {
        CancelCheckListProjectTemplate();
    });

    $("#btnSaveCheckListProjectTemplate").on('click', function (e) {
        SaveCheckListProjectTemplate();
    });

});

function validate() {
    if (ddlBlankCheck('ddlProject')) {
        AlertBox("warning", "CheckList Master!", "Please Select Project.!");
        return false;
    }
    if (ddlBlankCheck('ddlTemplate')) {
        AlertBox("warning", "CheckList Master!", "Please Select Template.!");
        return false;
    }
    

}

function projectDetails() {
    //vProjectNo = $("#ddlProject").val()
    projectData = {
        //  vProjectNo: vProjectNo,
        cProjectFilter: 'N'
    }
    projectAjaxData = {
        url: ApiURL + "CommonData/ProjectDetails",
        type: 'POST',
        data: projectData,
        async: false,
        success: successProjectDetails,
        error: errorProjectDetails
    }
    getProjectDetails(projectAjaxData.url, projectAjaxData.type, projectAjaxData.data, projectAjaxData.async, projectAjaxData.success, projectAjaxData.error);

    $("#btnSaveCheckListProjectTemplate").css('visibility', 'hidden');
}

var getProjectDetails = function (url, type, data, async, success, error) {
    $.ajax({
        url: url,
        type: type,
        data: data,
        async: async,
        success: success,
        error:error
    });
}

function successProjectDetails(jsonData) {
    if (jsonData.length > 0) {
        sourceArrayN = [];        
        //for (var i = 0; i < jsonData.length; i++) {
            //sourceArrayN.push("[" + jsonData[i].vProjectNo + "]" + " " + jsonData[i].vRequestId);
            //sourceArrayV["[" + jsonData[i].vProjectNo + "]" + " " + jsonData[i].vRequestId] = jsonData[i].vWorkspaceId;
            //sourceArrayN.push(jsonData[i].vProjectNo);
            //sourceArrayV[jsonData[i].vProjectNo] = jsonData[i].vWorkspaceId;           
        //}
        //$("#ddlProject").autocomplete({
        //    source: sourceArrayN,
        //    change: function (event, ui) { }
        //});

        $("#ddlProject").append($("<option></option>").html("select Project").val(""));
        for (var i = 0; i < jsonData.length; i++) {
            $("#ddlProject").append($("<option></option>").html(jsonData[i].vProjectNo + " [ " + jsonData[i].vRequestId +" ] ").val(jsonData[i].vWorkspaceId));
        }
        //$.each(jsonData, function (i, item) {
        //    $("#ddlProject").append($("<option></option>").html(this[vProjectNo]).val(this[vWorkspaceId]));
        //});
        
       
    }
    else {
        //AlertBox("information","CheckList Project!","No Project Found.!")
    }
}

function errorProjectDetails() {
    AlertBox("error","CheckList Project!","Error While Retriving Project.!")
}

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
    getCheckListTemplateHdrDetail(CheckListTemplateHdrData.url, CheckListTemplateHdrData.type, CheckListTemplateHdrData.data, CheckListTemplateHdrData.async, CheckListTemplateHdrData.success, CheckListTemplateHdrData.error);
}

var getCheckListTemplateHdrDetail = function(url,type,data,async,success,error) {
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
        $("#ddlTemplate").append($("<option></option>").html("select Template").val(""));
        for (var i = 0; i < jsonData.length; i++) {
            $("#ddlTemplate").append($("<option></option>").html(jsonData[i].vTemplateDesc).val(jsonData[i].nTemplateHdrNo));
        }      
    }
    else {
        AlertBox("information", "CheckList Project!", "No Data Available in CheckList Template!");
    }

}

function errorCheckListTemplateHdrDetail() {
    AlertBox("error", "CheckList Project!", "Error While Retriving CheckList Template Details!");
}

function AddCheckListProjectTemplate() {

    if (validate() == false) {
        return false;
    }

    type = 2,
    nTemplateHdrNo = $("#ddlTemplate").val();
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
            "sScrollXInner": "1040",            
            "aoColumns": [
              { "sTitle": "Template Hdr No" },
              { "sTitle": "Template Dtl No" },
              { "sTitle": "Template Description" },
              { "sTitle": "Questions" }              
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
        $("#btnAddCheckListProjectTemplate").attr('disabled', true);
        $("#ddlProject").attr('disabled', true);
        $("#ddlTemplate").attr('disabled', true);
        $("#btnSaveCheckListProjectTemplate").css('visibility', 'visible');
    }
    else {
        AlertBox("information", "Project Template!", "No Data Available in CheckList Template!");
    }
}

function errorAddTemplate(ex) {
    throw ex;
    AlertBox("error", "CheckList Project!", "Error While Retriving CheckList Template Details!");
}

function CancelCheckListProjectTemplate() {
    $("#btnAddCheckListProjectTemplate").attr('disabled', false);
    $("#ddlProject").attr('disabled', false);
    $("#ddlTemplate").attr('disabled', false);    
    $("#tblCheckListTemplateQuestion").DataTable().clear().draw();
    $("#btnSaveCheckListProjectTemplate").css('visibility', 'hidden');
}

function SaveCheckListProjectTemplate() {
    vWorkSpaceID = $("#ddlProject").val();
    if (vWorkSpaceID == '' || vWorkSpaceID == "" || vWorkSpaceID == null) {
        AlertBox("warning", "CheckList Project!", "Please select project!");
        return false;
    }

    nTemplateHdrNo = $("#ddlTemplate").val();
    if (nTemplateHdrNo == '' || nTemplateHdrNo == "" || nTemplateHdrNo == null) {
        AlertBox("warning", "CheckList Project!", "Please select Template!");
        return false;
    }

    iUserId = $("#hdnuserid").val();
    CheckListQuestionTemplate = [];
    CheckListQuestionTemplate = $("#tblCheckListTemplateQuestion").dataTable().fnGetData();

    CheckListProjectTemplateData = {
        nTemplateHdrNo: nTemplateHdrNo,
        vWorkSpaceID: vWorkSpaceID,
        iUserId: iUserId,
        nTemplateDtlNo: CheckListQuestionTemplate
    }

    CheckListProjectTemplateAjaxData = {
        url: ApiURL + "SetData/SaveCheckListProjectTemplateDetails",
        type: 'POST',
        data: CheckListProjectTemplateData,
        async: false,
        success: successCheckListProjectTemplate,
        error: errorCheckListProjectTemplate
    }

    fnSaveCheckListProjectTemplate(CheckListProjectTemplateAjaxData.url, CheckListProjectTemplateAjaxData.type, CheckListProjectTemplateAjaxData.data, CheckListProjectTemplateAjaxData.async, CheckListProjectTemplateAjaxData.success, CheckListProjectTemplateAjaxData.error)
}

var fnSaveCheckListProjectTemplate = function (url, type, data, async, success, error) {
    $.ajax({
        url: url,
        type: type,
        data: data,
        async: async,
        success: success,
        error: error
    });
}

function successCheckListProjectTemplate(jsonData) {
    var result = jsonData
    if (result == "success") {
        AlertBox("success", "CheckList Project!", "CheckList Project Template Details Saved Successfully!");
        $("#tblCheckListTemplateQuestion").DataTable().clear().draw();
        $("#btnSaveCheckListProjectTemplate").css('visibility', 'hidden');
        $("#btnAddCheckListProjectTemplate").attr('disabled', false);
        $("#btnCancelCheckListProjectTemplate").attr('disabled', true);
    }
    else {
        AlertBox("error", "CheckList Project!", "Error Occured While Saving CheckList Project Template Details!");
    }
    
}

function errorCheckListProjectTemplate() {
    AlertBox("error", "CheckList Project!", "Error While Saving CheckList Project Details!");
}
