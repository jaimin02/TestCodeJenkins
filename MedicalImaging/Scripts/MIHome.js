$body = $("body");

$(document).on({
    ajaxStart: function () { },
    ajaxStop: function () { }
});

var ActivityData = [];
var SubActivityData = [];
var SubActivityNameData = [];
$(function () {
    onload = function () {
        onfocus = function () {
            if ($("#select2-ddlProject-container")[0].innerHTML != "") { }
        }
    }

    $("#legend").hide();
    $("#spanSkip").empty();
    $(".select2").select2();
    $("#btnSkip").hide();

    document.getElementById("ddlProject").tabIndex = 1;
    document.getElementById("ddlProject").focus();
    //document.getElementById("ddlStatus").tabIndex = 2; //Commented by BHargav Thaker
    //document.getElementById("ddlStatus").focus(); //Commented by BHargav Thaker

    $("#ddlStudy").on('change', function () {
        if ($("#select2-ddlStudy-container")[0].innerHTML != "") {
            if ($.fn.DataTable.isDataTable('#tblSubjectReviewList')) {
                $('#tblSubjectReviewList').DataTable().destroy();
            }
            $('#tblSubjectReviewList').empty();
            $('#tblSubjectReviewList thead').empty();
            $("#ddlProject").select2('val', '');
            StudySubjectDetail();
        }
    });

    $("#ddlProject").on('change', function () {
        if ($("#select2-ddlProject-container")[0].innerHTML != "") {
            if ($.fn.DataTable.isDataTable('#tblSubjectReviewList')) {
                $('#tblSubjectReviewList').DataTable().destroy();
            }
            $('#tblSubjectReviewList').empty();
            $('#tblSubjectReviewList thead').empty();
            $("#ddlStudy").select2('val', '');
            ProjectSubjectDetail();
        }
    });

    //$("#btnGo").on('click', function () {
    //    if ($("#select2-ddlProject-container")[0].innerHTML != "") {
    //        if ($.fn.DataTable.isDataTable('#tblSubjectReviewList')) {
    //            $('#tblSubjectReviewList').DataTable().destroy();
    //        }
    //        $('#tblSubjectReviewList').empty();
    //        $('#tblSubjectReviewList thead').empty();
    //        ProjectSubjectDetail();
    //    }
    //});

    getUserWiseStudyDetail();
    getUserWiseProjectDetail();
   // CheckSetProject();

    $("#btnClear").on('click', function () {
        if ($(tblSubjectReviewList) != 'undefined') {
            if ($.fn.DataTable.isDataTable('#tblSubjectReviewList')) {
                var table = $('#tblSubjectReviewList').DataTable();
                table.clear();
                table.destroy();
                $("#tblSubjectReviewList").find("thead").html("");
            }
        }
        $("#ddlStudy").select2('val', '');
        $("#ddlProject").select2('val', '');
    });

    $("#btnrefresh").on('click', function () {
        if ($("#select2-ddlStudy-container")[0].innerHTML != "") {
            if ($.fn.DataTable.isDataTable('#tblSubjectReviewList')) {
                $('#tblSubjectReviewList').DataTable().destroy();
            }
            $('#tblSubjectReviewList').empty();
            $('#tblSubjectReviewList thead').empty();
            $("#ddlProject").select2('val', '');
            StudySubjectDetail();
        }

        if ($("#select2-ddlProject-container")[0].innerHTML != "") {
            if ($.fn.DataTable.isDataTable('#tblSubjectReviewList')) {
                $('#tblSubjectReviewList').DataTable().destroy();
            }
            $('#tblSubjectReviewList').empty();
            $('#tblSubjectReviewList thead').empty();
            $("#ddlStudy").select2('val', '');
            ProjectSubjectDetail();
        }
    });
})

// Get Study Details by Bhargav Thaker
function getUserWiseStudyDetail() {
    $("#spanSkip").empty();
    $("#btnSkip").hide();
    var contextKeyVal = "cWorkspaceType = 'P' AND cIsTestSite <> 'Y' AND iUserid =" + $("#hdnuserid").val();
    var vProjectTypeCodeVal = $("#hdnscopevalues").val()
    vProjectTypeCodeVal = "'" + vProjectTypeCodeVal.replace(/,/g, "','") + "'"
    var userWiseProjectDetailData = {
        contextKey: contextKeyVal,
        vProjectTypeCode: vProjectTypeCodeVal,
        prefixText: ''
    }
    var userWiseProjectDetailAjaxData = {
        async: false,
        data: userWiseProjectDetailData,
        type: "POST",
        url: WebURL + "MIDicomStudy/MyProjectCompletionList",
        success: successUserWiseStudyDetail,
        error: errorUserWiseStudyDetail
    }
    fnUserWiseProjectDetail(userWiseProjectDetailAjaxData.async, userWiseProjectDetailAjaxData.data, userWiseProjectDetailAjaxData.type, userWiseProjectDetailAjaxData.url, userWiseProjectDetailAjaxData.success, userWiseProjectDetailAjaxData.success, userWiseProjectDetailAjaxData.error)
}
function successUserWiseStudyDetail(jsonData) {
    if (jsonData.responseText != "") {
        jsonData = JSON.parse(jsonData.responseText);
        for (var V = 0 ; V < jsonData.length ; V++) {
            $("#ddlStudy").append($("<option title='PROJECTCODE'></option>").html(jsonData[V].WorkspaceMerge).val(jsonData[V].vWorkspaceId));
            if (jsonData.length == 1) {
                $("#select2-ddlStudy-container")[0].innerHTML = $("#ddlStudy").val(jsonData[0].vWorkspaceId);
                $("#ddlStudy").trigger("change");
            }
        }
    }
    else {
        AlertBox("Information", "Dicom Study!", "No Data Available For Study!")
    }
}
function errorUserWiseStudyDetail() {
    AlertBox("error", "Dicom Study!", "Error While Retriving Study Details!");
}

// Get Project details
function getUserWiseProjectDetail() {
    $("#spanSkip").empty();
    $("#btnSkip").hide();
    var contextKeyVal = "cWorkspaceType = 'C' AND cIsTestSite <> 'Y' AND iUserid =" + $("#hdnuserid").val(); //cApprovedSite='Y' AND
    var vProjectTypeCodeVal = $("#hdnscopevalues").val()
    vProjectTypeCodeVal = "'" + vProjectTypeCodeVal.replace(/,/g, "','") + "'"
    var userWiseProjectDetailData = {
        contextKey: contextKeyVal,
        vProjectTypeCode: vProjectTypeCodeVal,
        prefixText: ''
    }
    var userWiseProjectDetailAjaxData = {
        async: false,
        data: userWiseProjectDetailData,
        type: "POST",
        url: WebURL + "MIDicomStudy/MyProjectCompletionList",
        //url: WebURL + "MIHome/ProjectSubjectDetail",

        success: successUserWiseProjectDetail,
        error: errorUserWiseProjectDetail
    }
    fnUserWiseProjectDetail(userWiseProjectDetailAjaxData.async, userWiseProjectDetailAjaxData.data, userWiseProjectDetailAjaxData.type, userWiseProjectDetailAjaxData.url, userWiseProjectDetailAjaxData.success, userWiseProjectDetailAjaxData.success, userWiseProjectDetailAjaxData.error)
}
var fnUserWiseProjectDetail = function (async, data, type, url, success, error) {
    //setTimeout(function () {
    $.ajax({
        //async: async,
        data: data,
        type: type,
        url: url,
        complete: success,
        error: error
    });
    //}, 0);
    return this;
}
function successUserWiseProjectDetail(jsonData) {
    if (jsonData.responseText != "") {
        jsonData = JSON.parse(jsonData.responseText);
        //$("#ddlProject").append($("<option></option>").html("Select Project").val(""));
        for (var V = 0 ; V < jsonData.length ; V++) {
            $("#ddlProject").append($("<option title='PROJECTCODE'></option>").html(jsonData[V].WorkspaceMerge).val(jsonData[V].vWorkspaceId));
            if (jsonData.length == 1) {
                $("#select2-ddlProject-container")[0].innerHTML = $("#ddlProject").val(jsonData[0].vWorkspaceId);
                $("#ddlProject").trigger("change");
            }
        }
    }
    else {
        AlertBox("Information", "Dicom Study!", "No Data Available For Project!")
    }
}
function errorUserWiseProjectDetail() {
    AlertBox("error", "Dicom Study!", "Error While Retriving Project Details!");
}

function StudySubjectDetail() {
    $("#divTblSubjectReviewList").hide();

    var vWorkspaceId = $("#ddlStudy").val();
    var iPeriod = 1;
    var vUserTypeCode = $("#hdnUserTypeCode").val();
    var iUserId = $("#hdnuserid").val();

    var ProjectLockDetailData = {
        vWorkspaceId: vWorkspaceId,
        iPeriod: iPeriod,
        vUserTypeCode: vUserTypeCode,
        iUserId: iUserId
    }

    var ProjectFreezerDetailAjaxData = {
        async: false,
        data: ProjectLockDetailData,
        type: "POST",
        beforeSend: function () {
            $('.modal').show();
        },
        complete: function () {
            $('.modal').hide();
        },
        url: WebURL + "MIHome/DashBoardDetail",
        success: successProjectSubjectDetail,
        error: errorProjectSubjectDetail
    }
    fnProjectSubjectDetail(ProjectFreezerDetailAjaxData.async, ProjectFreezerDetailAjaxData.data, ProjectFreezerDetailAjaxData.type, ProjectFreezerDetailAjaxData.url, ProjectFreezerDetailAjaxData.success, ProjectFreezerDetailAjaxData.success, ProjectFreezerDetailAjaxData.error)
}

//Get Project Subject fDetails

function ProjectSubjectDetail() {
    $("#divTblSubjectReviewList").hide();

    var vWorkspaceId = $("#ddlProject").val();
    var iPeriod = 1;
    var vUserTypeCode = $("#hdnUserTypeCode").val();
    var iUserId = $("#hdnuserid").val();
    
    var ProjectLockDetailData = {
        vWorkspaceId: vWorkspaceId,
        iPeriod: iPeriod,
        vUserTypeCode: vUserTypeCode,
        iUserId: iUserId
    }

    var ProjectFreezerDetailAjaxData = {
        async: false,
        data: ProjectLockDetailData,
        type: "POST",
        beforeSend: function () {
            $('.modal').show();
        },
        complete: function () {
            $('.modal').hide();
        },
        url: WebURL + "MIHome/DashBoardDetail",
        success: successProjectSubjectDetail,
        error: errorProjectSubjectDetail
    }
    fnProjectSubjectDetail(ProjectFreezerDetailAjaxData.async, ProjectFreezerDetailAjaxData.data, ProjectFreezerDetailAjaxData.type, ProjectFreezerDetailAjaxData.url, ProjectFreezerDetailAjaxData.success, ProjectFreezerDetailAjaxData.success, ProjectFreezerDetailAjaxData.error)
}
var fnProjectSubjectDetail = function (async, data, type, url, success, error) {
    $.ajax({
        data: data,
        type: type,
        url: url,
        complete: success,
        error: error
    });
    return this;
}
function successProjectSubjectDetail(jsonData) {
    if (jsonData.responseText != "") {
        jsonData = JSON.parse(jsonData.responseText);

        if (jsonData.length > 0) {
            $("#divTblSubjectReviewList").show();
            var activityDataSet = []

            for (var i = 0; i < jsonData.length; i++) {
                var inDataSet = [];
                var columnsIn = jsonData[0];

                for (var key in columnsIn) {
                    inDataSet.push(jsonData[i][key]);
                }
                activityDataSet.push(inDataSet);
            }

            var columnsIn = jsonData[0];
            var i = 0;
            var columns = [];

            for (var key in columnsIn) {
                var objectDat = new Object();
                objectDat.sTitle = key;
                columns.push(objectDat);
            }

            if ($.fn.DataTable.isDataTable('#tblSubjectReviewList')) {
                $('#tblSubjectReviewList').DataTable().destroy();
            }
            $('#tblSubjectReviewList').empty();
            $('#tblSubjectReviewList thead').empty();

            var otable = $("#tblSubjectReviewList").dataTable({
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
                "scrollX": true,
                "fnCreatedRow": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    console.log(aData);
                    for (var V = 3 ; V < aData.length ; V++) {
                        var x = V - 1;
                        if (V == 4) {
                            //$('td:eq(' + x + ')', nRow).html(aData[V]);
                            $('td:eq(2)', nRow).html("<a data-toggle='modal' onClick =" + "\" onViewDicom(this," + i + "," + V + ")" + "\"  data-tooltip='tooltip' title='" + aData[V] + "' vSubjectId='" + aData[1] + "'  style='cursor:pointer;'><span><u>" + aData[V] + "</u></span></a>");
                        }
                    }
                    i = i + 1;
                },
                "aoColumns": columns,
                "columnDefs": [
                     { "targets": [0, 1, 6, 7, 9, 10], "visible": false, "searchable": false } //6,7 Added by Bhargav Thaker
                ],
                "oLanguage": {
                    "sEmptyTable": "No Record Found",
                }
            });
        }
    }
    else {
        AlertBox("Information", "Dicom Study!", "No Data Available For Project!")
    }
}

function errorProjectSubjectDetail() {
    $("#divTblSubjectReviewList").hide();
    AlertBox("error", "Dicom Study!", "Error While Retriving Project Subject Details!");
}

var isSubjectReject = "N";
var bValidation = false;

function Validation(vWorkSpaceId, vSubjectId, vMySubjectNo, iMySubjectNo, iPeriod, vParentWorkspaceId, vActivityId, iNodeId, vSubActivityId, iSubNodeId, vActivityName, vSubActivityName) {

    if (vWorkSpaceId == "") {
        AlertBox("warning", "Image Review!", "Project not found.");
        DataSaveStatus = false;
        removeDiv();
        $(".spinner").hide();
    }
    var cRadiologist;
    cRadiologist = vSubActivityName.split('-')[0]

    bValidation = false;
    var DataSaveStatus = false;

    if ((!vActivityName.toUpperCase().match("ADJUDICATOR")) && (!vActivityName.toUpperCase().match("GLOBAL"))) {

        var MI_DataSaveStatus = {
            vParentWorkspaceId: vParentWorkspaceId,
            vWorkSPaceId: vWorkSpaceId,
            vActivityId: vActivityId,
            iNodeId: iNodeId,
            vSubActivityId: vSubActivityId,
            iSubNodeId: iSubNodeId,
            vsubjectid: vSubjectId,
            cRadiologist: cRadiologist,
        }

        $.ajax({
            url: WebURL + "MIDicomStudy/MI_DataSaveStatus",
            type: "POST",
            data: MI_DataSaveStatus,
            async: false,
            success: function (jsonDataSaveStatus) {
                var splitVal = jsonDataSaveStatus.split("@")[0];
                var data = splitVal.split("#")[0];
                if (data == "NOTALLOW" && jsonDataSaveStatus.split("@")[1].split("#")[0].toUpperCase() == "BLOCK") {
                    AlertBox("warning", "Image Review!", "Subject is Rejected And Dicom Study For " + vActivityName + " Is Not Assigned Yet.! ");
                }
                else if (data == "NOTALLOW") {
                    AlertBox("warning", "Image Review!", "Dicom Study For " + vActivityName + " Is Not Assigned Yet.!");
                    DataSaveStatus = false;
                    removeDiv();
                    $(".spinner").hide();

                }
                else if (data == "ERROR") {
                    AlertBox("error", "Image Review!", "Error While Retriving Image Assignment Status.!");
                    DataSaveStatus = false;
                    removeDiv();
                    $(".spinner").hide();
                }
                else if (data == "ALLOW") {
                    if (splitVal.split("#")[1].toUpperCase() == "YES") {
                        DataSaveStatus = true;

                        for (var arryIndex = 0; arryIndex <= arryActivity.length - 1; arryIndex++) {

                            if (jsonDataSaveStatus.split("@")[1].split("#")[1].toUpperCase() == "BL") {
                                arryActivity[arryIndex][1] = "true";
                                if (arryActivity[arryIndex2][0].toUpperCase() == "GLOBAL RESPONSE" || arryActivity[arryIndex2][0].toUpperCase() == "ADJUDICATOR" || arryActivity[arryIndex2][0].toUpperCase() == "IOV ASSESSMENT") {
                                    arryActivity[arryIndex2][1] = "false";
                                }
                                else {
                                    arryActivity[arryIndex2][1] = "true";
                                }
                                break;
                            }

                            if (jsonDataSaveStatus.split("@")[1].split("#")[1].toUpperCase() == arryActivity[arryIndex][0].toUpperCase()) {
                                arryActivity[arryIndex][1] = "true";
                                for (var arryIndex2 = arryIndex + 1; arryIndex2 <= arryActivity.length - 1; arryIndex2++) {
                                    if (arryActivity[arryIndex2][0].toUpperCase() == "GLOBAL RESPONSE" || arryActivity[arryIndex2][0].toUpperCase() == "ADJUDICATOR" || arryActivity[arryIndex2][0].toUpperCase() == "IOV ASSESSMENT") {
                                        arryActivity[arryIndex2][1] = "false";
                                    }
                                    else {
                                        arryActivity[arryIndex2][1] = "true";
                                    }
                                }
                                break;
                            }
                        }

                        if (jsonDataSaveStatus.split("@")[1].split("#")[0].toUpperCase() == "BLOCK") {
                            isSubjectReject = "Y"
                        }
                        else if (jsonDataSaveStatus.split("@")[1].split("#")[0].toUpperCase() == "UNBLOCK") {
                            isSubjectReject = "N"
                        }
                    }
                    else {
                        DataSaveStatus = false;
                        AlertBox("error", "Image Review!!", "No Lesion Detail Found.Subject is not Eligible For Study.!");
                    }

                }
                else {
                    AlertBox("error", "Image Review!", "Error While Retriving Image Assignment Status.!");
                    DataSaveStatus = false;
                    removeDiv();
                    $(".spinner").hide();

                }
            },
            error: function (e) {
                var error = e;
                AlertBox("error", "Image Review!", "Error While Retriving Image Assignment Status.!");
                DataSaveStatus = false;
                removeDiv();
                $(".spinner").hide();
            }
        });
    }
    else {
        DataSaveStatus = true;
        isSubjectReject = "N";
    }


    if (DataSaveStatus == true) {
        var CRFDataEntryStatus = {
            MODE: 1,
            vParentWorkSpaceId: vParentWorkspaceId,
            vWorkspaceId: vWorkSpaceId,
            vSubjectId: vSubjectId,
            iMySubjectNo: iMySubjectNo,
            ScreenNo: vMySubjectNo,
            Radiologist: cRadiologist,
            Activity: vActivityName

        }

        $.ajax({
            url: WebURL + "MIDicomStudy/CRFDataEntryStatus",
            type: "POST",
            data: CRFDataEntryStatus,
            async: false,
            success: function (jsonData) {
                if (jsonData.includes("#")) {
                    if (jsonData.split("#")[1].split("@")[0].toUpperCase() == "BLOCK") {
                        isSubjectReject = "Y";
                    }

                    for (var arryIndex = 0; arryIndex <= arryActivity.length - 1; arryIndex++) {
                        if (jsonData.split("#")[1].split("@")[1] != "" || jsonData.split("#")[1].split("@")[1] != null) {
                            if (jsonData.split("#")[1].split("@")[1].toUpperCase() == "BL") {
                                arryActivity[arryIndex][1] = "true";
                                if (arryActivity[arryIndex2][0].toUpperCase() == "GLOBAL RESPONSE" || arryActivity[arryIndex2][0].toUpperCase() == "ADJUDICATOR" || arryActivity[arryIndex2][0].toUpperCase() == "IOV ASSESSMENT") {
                                    arryActivity[arryIndex2][1] = "false";
                                }
                                else {
                                    arryActivity[arryIndex2][1] = "true";
                                }
                                break;
                            }

                            if (jsonData.split("#")[1].split("@")[1].toUpperCase() == arryActivity[arryIndex][0].toUpperCase()) {
                                arryActivity[arryIndex][1] = "true";
                                for (var arryIndex2 = arryIndex + 1; arryIndex2 <= arryActivity.length - 1; arryIndex2++) {
                                    if (arryActivity[arryIndex2][0].toUpperCase() == "GLOBAL RESPONSE" || arryActivity[arryIndex2][0].toUpperCase() == "ADJUDICATOR" || arryActivity[arryIndex2][0].toUpperCase() == "IOV ASSESSMENT") {
                                        arryActivity[arryIndex2][1] = "false";
                                    }
                                    else {
                                        arryActivity[arryIndex2][1] = "true";
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
                if ((isSubjectReject == "Y") || vActivityName.toUpperCase().match("GLOBAL RESPONSE")) {
                    jsonData = "success";
                }
                var data = jsonData.split("#")[0];
                if (data == "error") {
                    $("#legend").hide();
                    AlertBox("error", "Image Review!", "Error While Retriving CRF Data Entry Control Details.!");
                    return false;
                }
                else if (data == "NO-DATA") {
                    $("#legend").hide();
                    AlertBox("error", "Image Review!", "No Data For CRF Data Entry Control Details.!");
                    return false;
                }
                else if (data == "success") {
                    DataSaveStatus = true
                    bValidation = true
                }
                else {
                    if (data == "") {
                        AlertBox("warning", "Image Review!", "No Detail Found.!")
                    }
                    else {
                        AlertBox("warning", "Image Review!", data)
                    }
                    removeDiv();
                    $(".spinner").hide();
                    return false;
                }
            },
            error: function (e) {
                alert(e);
            }
        });
    }

}

var ImgTransmittalVisit = [];

function GetImgTransmittalVisit(vWorkspaceId, vSubjectID, vVisit) {

    ImgTransmittalVisit = [];

    var ProjectLockDetailData = {
        vWorkspaceId: vWorkspaceId + '#' + vSubjectID + '#' + vVisit,
        SPName: 'Pro_GetImgTransmittalVisit'
    }

    var ProjectFreezerDetailAjaxData = {
        async: false,
        data: ProjectLockDetailData,
        type: "POST",
        url: WebURL + "MIHome/ProjectSubjectDetail",
        success: successImgTransmittalVisit,
        error: errorImgTransmittalVisit
    }

    $.ajax({
        data: ProjectFreezerDetailAjaxData.data,
        async: ProjectFreezerDetailAjaxData.async,
        type: ProjectFreezerDetailAjaxData.type,
        url: ProjectFreezerDetailAjaxData.url,
        complete: ProjectFreezerDetailAjaxData.success,
        error: ProjectFreezerDetailAjaxData.error
    });


    function successImgTransmittalVisit(jsonData) {
        if (jsonData.responseText != "") {
            jsonData = JSON.parse(jsonData.responseText);

            if (jsonData.length > 0) {
                ImgTransmittalVisit = jsonData;

            }
        }

    }

    function errorImgTransmittalVisit() {

        AlertBox("error", "Dicom Study!", "Error While Retriving Project Subject Details!");

    }
}

var arrySubActivityList = [];
var arrySubActivityNameList = [];
var strSingle = "";

function getSubActivityDetail(vWorkSpaceID, iParentNodeId, vSubjectId, vActivityName, radiologist, UserCode) {
    var iPeriod = 1;
    if ($("#hdnUserNameWithProfile").val().toUpperCase().lastIndexOf("ADJUDICATOR") > 0) {
        var vUserTypeCode = UserCode;
        var iUserId = parseInt(radiologist);
    } else {
        var vUserTypeCode = $("#hdnUserTypeCode").val();
        var iUserId = $("#hdnuserid").val();
    }
    var MODE = '2';

    var subActivityDetailData = {
        vWorkSpaceID: vWorkSpaceID,
        iPeriod: iPeriod,
        iParentNodeId: iParentNodeId,
        vUserTypeCode: vUserTypeCode,
        vSubjectId: vSubjectId,
        iUserId: iUserId,
        MODE: MODE,
        vActivityName: vActivityName
    }

    var subActivityDetailAjaxData = {
        url: WebURL + "MIDicomStudy/ProjectActivityDetails",
        type: "POST",
        data: subActivityDetailData,
        async: false,
        success: successSubActivityDetail,
        error: errorSubActivityDetail
    }

    $.ajax({
        url: subActivityDetailAjaxData.url,
        type: subActivityDetailAjaxData.type,
        data: subActivityDetailAjaxData.data,
        async: subActivityDetailAjaxData.async,
        success: subActivityDetailAjaxData.success,
        error: subActivityDetailAjaxData.error
    });

    function successSubActivityDetail(jsonData) {
        if (jsonData == "error") {
            AlertBox("error", "Dicom Study", "Error While Getting Subject Detail!")
        }
        else if (jsonData.length > 0) {
            jsonData = $.parseJSON(jsonData);
            for (var V = 0 ; V < jsonData.length; V++) {
                arrySubActivityList.push(jsonData[V].vActivityId + "#" + jsonData[V].iNodeId);
                arrySubActivityNameList.push(jsonData[V].vNodeDisplayName);
                if (V == 0) {
                    strSingle = jsonData[V].vActivityId + "#" + jsonData[V].iNodeId;
                }
            }
            SubActivityData = JSON.stringify(arrySubActivityList);
            SubActivityNameData = JSON.stringify(arrySubActivityNameList);
           // localStorage.setItem("SubActivityList", JSON.stringify(arrySubActivityList));
           // localStorage.setItem("SubActivityNameList", JSON.stringify(arrySubActivityNameList));
            //if (localStorage.getItem("IsReviewDone") == "true" || localStorage.getItem("IsReviewContinue") == "true") {
            //    $("#ddlSubActivity").val(Glo_SubActivity).change();
            //    localStorage.setItem("IsReviewDone", "false");
            //    localStorage.setItem("IsReviewContinue", "false");
            //    MIDicomStudy.getSubjectStudyDetail()
            //}

            //var strActivityName = $("#select2-ddlActivity-container")[0].innerHTML;
            //if (strActivityName.toUpperCase().match("ELIGIBILITY-REVIEW") || strActivityName.toUpperCase().match("MARK") || strActivityName.toUpperCase().match("BL") || (strActivityName.toUpperCase().match("GLOBAL") || strActivityName.toUpperCase().match("RESPONSE")) || strActivityName.toUpperCase().match("ADJUDICATOR") || strActivityName.toUpperCase().match("IOV ASSESSMENT")) {
            //    $(".SubActivity").show();
            //}
            //else {
            //    $("#ddlSubActivity").val(strSingle).change();
            //    $(".SubActivity").hide();
            //}

        }
        else {
            AlertBox("Information", "Dicom Study!", "No Data Available For Sub Activity!")
        }
    }
    function errorSubActivityDetail() {
        AlertBox("error", "Dicom Study!", "Error While Retriving  Sub Activity Details!");

    }

}

var arryActivity = [];
var arryActivityList = [];

function getActivityDetail(vWorkSpaceID, vSubjectId, radiologist, UserCode) {
    var iPeriod = 1;
    var iParentNodeId = 1;
    if ($("#hdnUserNameWithProfile").val().toUpperCase().lastIndexOf("ADJUDICATOR") > 0) {
        var vUserTypeCode = UserCode;
        var iUserId = parseInt(radiologist);
    } else {
        var vUserTypeCode = $("#hdnUserTypeCode").val();
        var iUserId = $("#hdnuserid").val();
    }
    
    var MODE = '1';
    var activityDetailData = {
        vWorkSpaceID: vWorkSpaceID,
        iPeriod: iPeriod,
        iParentNodeId: iParentNodeId,
        vUserTypeCode: vUserTypeCode,
        vSubjectId: vSubjectId,
        iUserId: iUserId,
        MODE: MODE
    }

    var activityDetailAjaxData = {
        url: WebURL + "MIDicomStudy/ProjectActivityDetails",
        type: "POST",
        data: activityDetailData,
        async: false,
        success: successActivityDetail,
        error: errorActivityDetail
    }

    $.ajax({
        url: activityDetailAjaxData.url,
        type: activityDetailAjaxData.type,
        data: activityDetailAjaxData.data,
        async: activityDetailAjaxData.async,
        success: activityDetailAjaxData.success,
        error: activityDetailAjaxData.error
    });

    function successActivityDetail(jsonData) {
        var arryActivityListMeasurement = [];
        if (jsonData == "error") {
            AlertBox("error", "Dicom Study", "Error While Getting Activity Detail!")
            getActivityDetail = false;
        }

        else if (jsonData.length > 0) {
            jsonData = $.parseJSON(jsonData);
            for (var V = 0 ; V < jsonData.length; V++) {
                arryActivity.push([jsonData[V].vNodeDisplayName, "False"]);
                arryActivityList.push([jsonData[V].vNodeDisplayName]);
                if (jsonData[V].cDataStatusColorCode == "GREEN") {
                    arryActivityListMeasurement.push([jsonData[V].vNodeDisplayName + "#" + jsonData[V].iNodeId]);
                }
            }
            ActivityData = JSON.stringify(arryActivityList);
            //localStorage.setItem("ActivityList", JSON.stringify(arryActivityList));
            localStorage.setItem("ActivityListForMeasurement", JSON.stringify(arryActivityListMeasurement));
            if (localStorage.getItem("IsReviewDone") == "true" || localStorage.getItem("IsReviewContinue") == "true") {
                //  $("#ddlActivity").val(Glo_Activity).change();
            }
        }
        else {
            AlertBox("Information", "Dicom Study!", "No Data Available For Activity!")
        }
    }
    function errorActivityDetail() {
        AlertBox("error", "Dicom Study!", "Error While Retriving Project Activity Details!");
    }
}

function onViewDicom(e, row, col) {
    debugger;
    var url = $("#DicomViewer").val();

    var objtable = $("#tblSubjectReviewList").DataTable()
    
    var rowsfnGetData = $("#tblSubjectReviewList").dataTable().fnGetData();
    var vWorkSpaceId = rowsfnGetData[row][0];
    var vSubjectId = rowsfnGetData[row][1];
    var vMySubjectNo = rowsfnGetData[row][3];

    var status = rowsfnGetData[row][7];
    var struser = rowsfnGetData[row][9].split(',');
    var strUserCode = rowsfnGetData[row][10].split(',');
    console.log(struser, strUserCode);

    var title = objtable.column(col).header();
    var vVisit = rowsfnGetData[row][4];

    var iMySubjectNo = '';
    var iPeriod = '1';
    var vParentWorkspaceId = '';
    var ddlActivityVal = 0;
    var val = '';
    var vActivityId = '';
    var iNodeId = '';
    var ddlSubActivityVal = '';
    var vSubActivityId = '';
    var iSubNodeId = '';
    var vActivityName = '';
    var vSubActivityName = '';
    var iImgTransmittalHdrId = 0
    var iImgTransmittalDtlId = 0
    var iImageStatus = 0
    var vProjectNo = ''
    var ImgTransmittalDtl_iImageTranNo = 1;
    var ImageTransmittalImgDtl_iImageTranNo = 1;
    var isSubjectReject = 'N'
    var iModalityNo = ""
    var iAnatomyNo = "";
    var iImageCount = '1';

    debugger;
    ImgTransmittalVisit = [];

    GetImgTransmittalVisit(vWorkSpaceId, vSubjectId, vVisit)

    if (ImgTransmittalVisit.length > 0) {
        iImgTransmittalHdrId = ImgTransmittalVisit[0].iImgTransmittalHdrId
        vParentWorkspaceId = ImgTransmittalVisit[0].vParentWorkspaceId
        iNodeId = ImgTransmittalVisit[0].iNodeId
        iImgTransmittalDtlId = ImgTransmittalVisit[0].iImgTransmittalDtlId
        vActivityId = ImgTransmittalVisit[0].vActivityId
        vActivityName = ImgTransmittalVisit[0].vNodeDisplayName
        vProjectNo = ImgTransmittalVisit[0].vProjectNo
        iModalityNo = ImgTransmittalVisit[0].iModalityNo
        iImageStatus = ImgTransmittalVisit[0].iImageStatus
        ImgTransmittalDtl_iImageTranNo = ImgTransmittalVisit[0].ImgTransmittalDtl_iImageTranNo
        ImageTransmittalImgDtl_iImageTranNo = ImgTransmittalVisit[0].ImageTransmittalImgDtl_iImageTranNo
        iImageCount = ImgTransmittalVisit[0].iImageCount
        iMySubjectNo = ImgTransmittalVisit[0].iMySubjectNo
    }

    arryActivity = [];
    arryActivityList = [];

    if (status == 'R2 Review Complete') {
        getActivityDetail(vWorkSpaceId, vSubjectId, parseInt(struser[1]), strUserCode[1])
    }
    else {
        getActivityDetail(vWorkSpaceId, vSubjectId, parseInt(struser[0]), strUserCode[0])
    }
    //getActivityDetail(vWorkSpaceId, vSubjectId, struser[0][0])

    if (arryActivityList.length <= 0) {
        AlertBox("warning", "Dicom Study", "User does not have Activity Right.")
        return;
    }

    var bActivity = false;

    for (var i = 0; i < arryActivityList.length; i++) {
        if (arryActivityList[i].toString().toUpperCase() == vActivityName.toUpperCase()) {
            bActivity = true
        }
    }

    if (bActivity == false) {
        AlertBox("warning", "Dicom Study", "User does not have Activity Right.")
        return;
    }

    arrySubActivityList = [];
    arrySubActivityNameList = [];
    strSingle = "";

    //getSubActivityDetail(vWorkSpaceId, iNodeId, vSubjectId, vActivityName)
    if (status == 'R2 Review Complete') {
        getSubActivityDetail(vWorkSpaceId, iNodeId, vSubjectId, vActivityName, parseInt(struser[1]), strUserCode[1])
    }
    else {
        getSubActivityDetail(vWorkSpaceId, iNodeId, vSubjectId, vActivityName, parseInt(struser[0]), strUserCode[0])
    }

    if (arrySubActivityList.length <= 0) {
        AlertBox("warning", "Dicom Study", "User does not have Sub-activity Right.")
        return;
    }

    if (arrySubActivityList.length > 0) {
        if (strSingle != "") {
            vSubActivityId = strSingle.split('#')[0];
            iSubNodeId = strSingle.split('#')[1];
        }
        vSubActivityName = arrySubActivityNameList[0]
    }

    if (vActivityName.toUpperCase().match("ELIGIBILITY-REVIEW") || vActivityName.toUpperCase().match("MARK") || vActivityName.toUpperCase().match("BL")
        || vActivityName.toUpperCase().match("GLOBAL") || vActivityName.toUpperCase().match("RESPONSE") || vActivityName.toUpperCase().match("ADJUDICATOR")
        || vActivityName.toUpperCase().match("IOV ASSESSMENT")) {

        if (arrySubActivityList.length <= 1) {
            AlertBox("warning", "Dicom Study", "User does not have Sub-activity Right.")
            return;
        }
    }
    else {
        if (arrySubActivityList.length < 3) {
            AlertBox("warning", "Dicom Study", "User does not have poper Sub-activity Right.")
            return;
        }
    }

    bValidation = false;

    Validation(vWorkSpaceId, vSubjectId, vMySubjectNo, iMySubjectNo, iPeriod, vParentWorkspaceId, vActivityId, iNodeId, vSubActivityId, iSubNodeId, vActivityName, vSubActivityName)

    if (bValidation == false) {
        return;
    }

    var strFormElement = '<form class="custom-form" method="post" action="' + url + '" target="_blank">';
    strFormElement += '<input type="hidden" name="vParentWorkspaceId" value="' + vParentWorkspaceId + '">';
    strFormElement += '<input type="hidden" name="vWorkSpaceId" value="' + vWorkSpaceId + '">';
    strFormElement += '<input type="hidden" name="vSubjectId" value="' + vSubjectId + '">';
    strFormElement += '<input type="hidden" name="vMySubjectNo" value="' + vMySubjectNo + '">';
    strFormElement += '<input type="hidden" name="iMySubjectNo" value="' + iMySubjectNo + '">';
    strFormElement += '<input type="hidden" name="iPeriod" value="' + iPeriod + '">';
    strFormElement += '<input type="hidden" name="vActivityId" value="' + vActivityId + '">';
    strFormElement += '<input type="hidden" name="iNodeId" value="' + iNodeId + '">';
    strFormElement += '<input type="hidden" name="vSubActivityId" value="' + vSubActivityId + '">';
    strFormElement += '<input type="hidden" name="iSubNodeId" value="' + iSubNodeId + '">';
    strFormElement += '<input type="hidden" name="vActivityName" value="' + vActivityName + '">';
    strFormElement += '<input type="hidden" name="vSubActivityName" value="' + vSubActivityName + '">';

    strFormElement += '<input type="hidden" name="vSkipVisit" value="N">';

    strFormElement += '<input type="hidden" name="iImgTransmittalHdrId" value="' + iImgTransmittalHdrId + '">';
    strFormElement += '<input type="hidden" name="iImgTransmittalDtlId" value="' + iImgTransmittalDtlId + '">';
    strFormElement += '<input type="hidden" name="iImageStatus" value="' + iImageStatus + '">';
    strFormElement += '<input type="hidden" name="vProjectNo" value="' + vProjectNo + '">';
    strFormElement += '<input type="hidden" name="iModalityNo" value="' + iModalityNo + '">';
    strFormElement += '<input type="hidden" name="iAnatomyNo" value="' + iAnatomyNo + '">';
    strFormElement += '<input type="hidden" name="iImageCount" value="' + iImageCount + '">';
    strFormElement += '<input type="hidden" name="ImgTransmittalDtl_iImageTranNo" value="' + ImgTransmittalDtl_iImageTranNo + '">';
    strFormElement += '<input type="hidden" name="ImageTransmittalImgDtl_iImageTranNo" value="' + ImageTransmittalImgDtl_iImageTranNo + '">';
    strFormElement += '<input type="hidden" name="subjectRejectionDtl" value="' + isSubjectReject + '">';
    strFormElement += '<input type="hidden" name="activityArray" value="' + arryActivity.join() + '">';
    strFormElement += '<input type="hidden" name="activityData" value="' + encodeURI(ActivityData) + '">';
    strFormElement += '<input type="hidden" name="subActivityData" value="' + encodeURI(SubActivityData) + '">';
    strFormElement += '<input type="hidden" name="subActivityNameData" value="' + encodeURI(SubActivityNameData) + '">';

    strFormElement += '</form>';


    var form = $(strFormElement);
    $(document.body).append(form[0]); //Resolve Chrome Version Change Issue 
    form.submit();

    $(".custom-form").remove();

    localStorage.setItem("IsReviewDone", "false");
    localStorage.setItem("IsReviewContinue", "false");
}

function CheckSetProject() {
    var UserId = $("#hdnuserid").val();
    data = {
        iUserId: UserId
    }
    $.ajax({
        //url: ApiURL + "GetData/ProjectDetails",
        url: WebURL + "MIDicomStudy/ProjectDetails",
        type: "POST",
        data: data,
        dataType: 'json',
        success: SuccessMethod,
        async: false,
        error: function (ex) {
            //AlertBox("warning", " Set Project ", "Failed to retrieve project details : " + ex);
            //getUserWiseProjectDetail();
        }
    });

    function SuccessMethod(jsonData) {
        var jsonObj = jsonData;
        var sourceArr = [];
        if (jsonData.length > 0) {
            vWorkSpaceId = jsonData[0].vWorkSpaceId;
            nSetProjectNo = jsonData[0].nSetProjectNo;
            WorkspaceMerge = jsonData[0].WorkspaceMerge;

            var $option = $("<option selected></option>").val(vWorkSpaceId).text(WorkspaceMerge);

            $('#ddlProject').append($option).trigger('change');

            //$("#ddlProject option[value=" + vWorkSpaceId + "]").attr('selected', 'selected');
            //$("#ddlProject").val(vWorkSpaceId);
            //$("#ddlProject").val(vWorkSpaceId).trigger('change');
        }
        else {
            vWorkSpaceId = "";
            $('#ddlProject').val('').trigger('change');
            $('#ddlProject').prop('disabled', false);
            getUserWiseProjectDetail();
        }
    }
}