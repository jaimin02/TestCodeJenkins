var vWorkspaceId, vProjectNo, vSubjectId, iNodeId, iModalityNo, iAnatomyNo, iuserid, iImgTransmittalHdrId, iImgTransmittalDtlId, iImageStatus, ActivityID, NodeID, ActivityDef, iMySubjectNo, ScreenNo, ParentWorkSpaceId, PeriodId, ActivityName, SubActName, subinodeID, parentActivityID, AdjUserId, UserTypeCode;
var RDicom;
var SubActivityAdjName = '';
var WorkFlowStageId = '';
$body = $("body");
var Glo_Screenno;
var Glo_Activity;
var Glo_SubActivity;
var R1UserId = "";
var R2UserId = "";
var R1UserType = "";
var R2userType = "";
var IsReviewDone;
var IsReviewContinue;
var arrStorage = [];
var ActivityData = [];
var SubActivityData = [];
var SubActivityNameData = [];

$(document).on({
    ajaxStart: function () { },
    ajaxStop: function () { }
});

if (typeof MIDicomStudy == "undefined") {
    MIDicomStudy = {};
}


if (typeof General == "undefined") {
    General = {};
}

$(document).ajaxStart(function () {
    var query = window.location.search.substring(1);
    if (query == '' || query == "" || query == null) {
    }
    else {
        var ActivityID, NodeID, ActivityDef;
        var parms = query.split('&');
        for (var i = 0; i < parms.length; i++) {
            var pos = parms[i].indexOf('=');
            if (pos > 0) {
                var key = parms[i].substring(0, pos);
                var val = parms[i].substring(pos + 1);
                //alert(val);
                if (key == 'Uid') {
                    $("#hdnuserid").val(val);
                }
                else if (key == 'AdjUserId') {
                    $("#hdnAdjUserId").val(val);
                }
            }
        }
    }

    var userLoginDetails = {
        iUserId: $("#hdnAdjUserId").val(),
        vIPAddress: $("#hdnIpAddress").val(),
        DATAOPMODE: 4
    }

    var ajaxData = {
        url: ApiURL + "SetData/save_UserLoginDetails",
        type: 'POST',
        async: false,
        data: userLoginDetails,
        success: successuserLoginDetails,
        error: erroruserLoginDetails
    }

    $.ajax({
        url: ajaxData.url,
        type: ajaxData.type,
        data: ajaxData.data,
        //async: ajaxData.async,
        success: ajaxData.success,
        error: ajaxData.error
    });

    function successuserLoginDetails(jsonData) {
        debugger;
        console.log(jsonData);
        if (jsonData.length == 0) {
            logOut();
            var url = $("#RedirectToLogin").val();
            location.href = url;
        }
        else {
            window.location.replace(WebURL + 'MIDicomViewer/MIDicomViewer?WId=' + vWorkspaceId + '&SId=' + vSubjectId + '&PId=' + vProjectNo + '&Uid=' + $("#hdnAdjUserId").val() + '&MId=' + iModalityNo + '&AId=' + iAnatomyNo + '&VId=' + iNodeId + '&HdrId=' + iImgTransmittalHdrId + '&DtlId=' + iImgTransmittalDtlId + '&iIS=' + iImageStatus + '&ActivityID=' + ActivityID + '&NodeID=' + NodeID + '&ActivityDef=' + ActivityDef + '&iMySubjectNo=' + iMySubjectNo + '&ScreenNo=' + ScreenNo + '&ParentWorkSpaceId=' + ParentWorkSpaceId + '&PeriodId=' + PeriodId, '_blank');
        }
    }

    function erroruserLoginDetails() {
        AlertBox('error', 'MI', 'Error While Saving User Activity!')
    }

    function logOut() {
        var userLoginDetails = {
            iUserId: $("#hdnuserid").val(),
            vIPAddress: $("#hdnIpAddress").val(),
            DATAOPMODE: 3
        }

        var ajaxData = {
            url: ApiURL + "SetData/save_UserLoginDetails",
            type: 'POST',
            async: false,
            data: userLoginDetails,
            success: successlogOutUserDetails,
            error: errorlogOutUserDetails
        }

        $.ajax({
            url: ajaxData.url,
            type: ajaxData.type,
            data: ajaxData.data,
            //async: ajaxData.async,
            success: ajaxData.success,
            error: ajaxData.error
        });

        function successlogOutUserDetails(jsonData) {
            var url = $("#RedirectToLogin").val();
            location.href = url;
        }

        function errorlogOutUserDetails() {
            AlertBox('error', 'MI', 'Error While LogOut User!')
        }

    }
});

$(document).ajaxComplete(function () {
});

var arryActivity = [];
var isSubjectReject = "N";
var GetRadioLogistData = [];
$(function () {
    //var isSubjectReject = "N";
    /////////khan
    var query = window.location.search.substring(1);
    var parms = query.split('&');
    for (var i = 0; i < parms.length; i++) {
        var pos = parms[i].indexOf('=');
        if (pos > 0) {
            var key = parms[i].substring(0, pos);
            var val = parms[i].substring(pos + 1);
            //alert(val);
            if (key == 'WId') {
                vWorkspaceId = val;
            }
            else if (key == 'SId') {
                vSubjectId = val;
            }
            else if (key == 'PId') {
                vProjectNo = val;
            }
            else if (key == 'Uid') {
                iuserid = val;
                $("#hdniUserId").val(iuserid);
            }
            else if (key == 'MId') {
                iModalityNo = val;
            }
            else if (key == 'AId') {
                iAnatomyNo = val;
            }
            else if (key == 'VId') {
                iNodeId = val;
            }
            else if (key == 'HdrId') {
                iImgTransmittalHdrId = val;
            }
            else if (key == 'DtlId') {
                iImgTransmittalDtlId = val;
            }
            else if (key == 'iIS') {
                iImageStatus = val;
            }
            else if (key == 'ActivityID') {
                ActivityID = val;
                //$("#hdnActivityID").val(val);
            }
            else if (key == 'NodeID') {
                NodeID = val;
                $("#hdnNodeID").val(val);
            }
            else if (key == 'ActivityDef') {
                ActivityDef = val;
                $("#hdnActivityDef").val(val);
            }
            else if (key == 'iMySubjectNo') {
                iMySubjectNo = val;
                $("#hdniMySubjectNo").val(val);
            }
            else if (key == 'ScreenNo') {
                ScreenNo = val;
                $("#hdnScreenNo").val(val);
            }
            else if (key == 'ParentWorkSpaceId') {
                ParentWorkSpaceId = val;
                $("#hdnParentWorkSpaceId").val(val);
            }
            else if (key == 'PeriodId') {
                PeriodId = val;
                $("#hdnPeriodId").val(val);
            }
            else if (key == 'ActivityName') {
                ActivityName = val;
                $("#hdnActivityName").val(val);
            }
            else if (key == 'SubActivityName') {
                if (val != undefined) {
                    SubActName = val;
                    SubActivityAdjName = val
                    $("#hdnSubActivityName").val(val);
                }
            }
            else if (key == 'subinodeID') {
                subinodeID = val;
                $("#hdnsubinodeID").val(val);
            }
            else if (key == 'parentActivityID') {
                parentActivityID = val;
                $("#hdnparentActivityID").val(val);
            }
            else if (key == 'parentActivityID') {
                parentActivityID = val;
                $("#hdnparentActivityID").val(val);
            }
            else if (key == 'AdjUserId') {
                AdjUserId = val;
                $("#hdnAdjUserId").val(val);
            }
            else if (key == 'UserTypeCode') {
                UserTypeCode = val;
                $("#hdnUserTypeCode").val(val);
            }
            else if (key == 'WorkFlowStageId') {
                WorkFlowStageId = val;
            }
        }
    }

    ////////
    onload = function () {
        onfocus = function () {
            if ($("#select2-ddlProject-container")[0].innerHTML != "") {
                if ($("#select2-ddlSubject-container")[0].innerHTML != "") {
                    if ($("#select2-ddlActivity-container")[0].innerHTML != "") {
                        if ($("#select2-ddlSubActivity-container")[0].innerHTML != "") {
                            //$.ajax({
                            //    url: WebURL + "MIDicomStudy/updatedSessionValue",
                            //    type: "POST",
                            //    async: false,
                            //    success: function (updateJson) {
                            //        if (updateJson == "TRUE") {
                            //            MIDicomStudy.getSubjectStudyDetail()
                            //        }
                            //    },
                            //    error: function (e) {
                            //        AlertBox("ERROR", "Dicom Study!", "Error While Checking Update Session!");
                            //    }
                            //});
                        }
                    }
                }
            }
        }
    }

    $("#legend").hide();
    $("#spanSkip").empty();
    $(".select2").select2();
    $("#btnSkip").hide();
    $("#btnSkip2").hide();

    document.getElementById("ddlProject").tabIndex = 1;
    document.getElementById("ddlSubject").tabIndex = 2;
    document.getElementById("ddlActivity").tabIndex = 3;
    document.getElementById("ddlSubActivity").tabIndex = 4;
    document.getElementById("ddlProject").focus();

    $("#ddlProject").on('change', function () {
        if ($("#select2-ddlProject-container")[0].innerHTML != "") {
            MIDicomStudy.ProjectFreezerDetail();
            Glo_Activity = '';
        }
    });

    $("#ddlSubject").on('change', function () {
        if ($("#select2-ddlSubject-container")[0].innerHTML != "") {
            arryActivity = [];
            if (Glo_Activity == '' || Glo_Activity == null) {
                MIDicomStudy.getActivityDetail();
                $("#ddlActivity").select2('val', '');
                $("#ddlSubActivity").select2('val', '');
            }
            else {
                MIDicomStudy.getSubjectStudyDetail();
            }

            if ($(tblSubjectReviewList) != 'undefined') {
                if ($.fn.DataTable.isDataTable('#tblSubjectReviewList')) {
                    var table = $('#tblSubjectReviewList').DataTable();
                    table.clear();
                    table.destroy();
                    $("#tblSubjectReviewList").find("thead").html("");
                }
            }
        }

    });

    $("#ddlActivity").on('change', function () {
        if ($("#select2-ddlActivity-container")[0].innerHTML != "") {
            MIDicomStudy.getSubActivityDetail();
        }
    });

    $("#btnGo").on('click', function () {
        debugger;
        MIDicomStudy.getSubjectStudyDetail()
    });

    $("#btnClear").on('click', function () {

        $("#legend").hide();

        if ($(tblSubjectReviewList) != 'undefined') {
            if ($.fn.DataTable.isDataTable('#tblSubjectReviewList')) {
                var table = $('#tblSubjectReviewList').DataTable();
                table.clear();
                table.destroy();
                $("#tblSubjectReviewList").find("thead").html("");
            }
        }

        $("#ddlSubActivity").select2('val', '');
        $("#ddlActivity").select2('val', '');
        $("#ddlSubject").select2('val', '');
        $("#ddlProject").select2('val', '');



    });

    $("#btnSkip").on('click', function () {
        var profile = "R1";
        MIDicomStudy.skipVisit(profile)
    });

    $("#btnSkip2").on('click', function () {
        var profile = "R2";
        MIDicomStudy.skipVisit(profile)
    });

    (function ($, MIDicomStudy, General) {
        'use strict';

        function getUserWiseProjectDetail() {
            $("#spanSkip").empty();
            $("#btnSkip").hide();
            $("#btnSkip2").hide();
            var contextKeyVal = ' iUserid =' + $("#hdnuserid").val();
            var vProjectTypeCodeVal = $("#hdnscopevalues").val()
            //var vProjectTypeCodeVal =  "0001,0002,0003,0004,0005,0006,0007,0008,0009,0010,0014,0015,0016,0017,0018,0019,0020,0021,,,,,,,,,,,,,"
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
                //url: ApiURL + "GetData/MyProjectCompletionList",
                url: WebURL + "MIDicomStudy/MyProjectCompletionList",
                success: MIDicomStudy.successUserWiseProjectDetail,
                error: MIDicomStudy.errorUserWiseProjectDetail
            }
            MIDicomStudy.fnUserWiseProjectDetail(userWiseProjectDetailAjaxData.async, userWiseProjectDetailAjaxData.data, userWiseProjectDetailAjaxData.type, userWiseProjectDetailAjaxData.url, userWiseProjectDetailAjaxData.success, userWiseProjectDetailAjaxData.success, userWiseProjectDetailAjaxData.error)
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
                var query = window.location.search.substring(1);
                if (query == '' || query == "" || query == null) {
                }
                else {
                    var ActivityID, NodeID, ActivityDef;
                    var parms = query.split('&');
                    for (var i = 0; i < parms.length; i++) {
                        var pos = parms[i].indexOf('=');
                        if (pos > 0) {
                            var key = parms[i].substring(0, pos);
                            var val = parms[i].substring(pos + 1);
                            //alert(val);
                            if (key == 'WId') {
                                vWorkspaceId = val;
                            }
                            else if (key == 'SId') {
                                vSubjectId = val;
                            }
                            else if (key == 'UserProfile') {
                                ActivityName = val;
                            }
                            else if (key == 'SubActivityName') {
                                SubActName = val;
                            }
                            else if (key == 'RDicom') {
                                RDicom = val;
                            }
                        }
                    }
                }
                jsonData = JSON.parse(jsonData.responseText);
                $("#ddlProject").append($("<option></option>").html("Select Project").val(""));
                for (var V = 0 ; V < jsonData.length ; V++) {
                    if (query == '' || query == "" || query == null) {
                        $("#ddlProject").append($("<option title='PROJECTCODE'></option>").html(jsonData[V].WorkspaceMerge).val(jsonData[V].vWorkspaceId));
                    }
                    else {
                        if (jsonData[V].vWorkspaceId == vWorkspaceId) {
                            $("#ddlProject").append($("<option title='PROJECTCODE'></option>").html(jsonData[V].WorkspaceMerge).val(jsonData[V].vWorkspaceId));
                            $("#select2-ddlProject-container")[0].innerHTML = $("#ddlProject").val(vWorkspaceId);
                            $("#ddlProject").trigger("change");
                        }
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

        MIDicomStudy.getUserWiseProjectDetail = getUserWiseProjectDetail;
        MIDicomStudy.successUserWiseProjectDetail = successUserWiseProjectDetail;
        MIDicomStudy.errorUserWiseProjectDetail = errorUserWiseProjectDetail;
        MIDicomStudy.fnUserWiseProjectDetail = fnUserWiseProjectDetail;

    }($, MIDicomStudy, General));

    (function ($, MIDicomStudy, General) {
        'USE STRICT'

        function ProjectFreezerDetail() {
            var vWorkspaceId = $("#ddlProject").val();
            var ProjectLockDetailData = {
                vWorkspaceId: vWorkspaceId
            }

            if (vWorkspaceId != "") {
                $(".SubActivity").show();
            }

            var ProjectFreezerDetailAjaxData = {
                async: false,
                data: ProjectLockDetailData,
                type: "POST",
                //url: ApiURL + "GetData/ProjectLockDetail",
                url: WebURL + "MIDicomStudy/ProjectFreezerDetail",
                success: MIDicomStudy.successProjectFreezerDetail,
                error: MIDicomStudy.errorProjectFreezerDetail
            }
            MIDicomStudy.fnProjectFreezerDetail(ProjectFreezerDetailAjaxData.async, ProjectFreezerDetailAjaxData.data, ProjectFreezerDetailAjaxData.type, ProjectFreezerDetailAjaxData.url, ProjectFreezerDetailAjaxData.success, ProjectFreezerDetailAjaxData.success, ProjectFreezerDetailAjaxData.error)
        }
        var fnProjectFreezerDetail = function (async, data, type, url, success, error) {
            $.ajax({
                //async: async,
                data: data,
                type: type,
                url: url,
                complete: success,
                error: error
            });
            return this;
        }
        function successProjectFreezerDetail(jsonData) {
            if (jsonData.responseText != "") {
                jsonData = JSON.parse(jsonData.responseText);
                //UnFreeze
                if (jsonData[0].cFreezeStatus == "U") {
                    getCRFVersion = "U" //UnFreeze
                    AlertBox("warning", "Dicom Study!", "Project Structure Is Not Freeze,To Do Data Entry Freeze It !");
                    //$("#ddlProject").select2('val', '');
                    $("#ddlProject").blur();
                }
                    //Freeze
                else if (jsonData[0].cFreezeStatus == "F") {
                    getCRFVersion = "F" ////Freeze
                    MIDicomStudy.ProjectLockDetail()
                    //if ($("#select2-ddlProject-container")[0].innerHTML != "") {
                    //    $('#ddlSubActivity').find('option').remove().end().append('<option value="select">Select Activity</option>').val('select');
                    //    $("#select2-ddlSubActivity-container")[0].innerHTML = ""
                    //    $('#ddlActivity').find('option').remove().end().append('<option value="select">Select Activity</option>').val('select');
                    //    $("#select2-ddlActivity-container")[0].innerHTML = "";
                    //    MIDicomStudy.getSubjectDetail();
                    //    //MIDicomStudy.getActivityDetail();
                    //}
                }
            }
            //else {
            //    if ($("#select2-ddlProject-container")[0].innerHTML != "") {
            //        $('#ddlSubActivity').find('option').remove().end().append('<option value="select">Select Activity</option>').val('select');
            //        $("#select2-ddlSubActivity-container")[0].innerHTML = ""
            //        $('#ddlActivity').find('option').remove().end().append('<option value="select">Select Activity</option>').val('select');
            //        $("#select2-ddlActivity-container")[0].innerHTML = "";
            //        MIDicomStudy.getSubjectDetail();
            //        //MIDicomStudy.getActivityDetail();
            //    }
            //}
        }
        function errorProjectFreezerDetail() {
            AlertBox("error", "Dicom Study!", "Error While Retriving Project status!");
        }

        MIDicomStudy.ProjectFreezerDetail = ProjectFreezerDetail;
        MIDicomStudy.successProjectFreezerDetail = successProjectFreezerDetail;
        MIDicomStudy.errorProjectFreezerDetail = errorProjectFreezerDetail;
        MIDicomStudy.fnProjectFreezerDetail = fnProjectFreezerDetail;

    }($, MIDicomStudy, General));


    (function ($, MIDicomStudy, General) {
        'USE STRICT'

        function ProjectLockDetail() {
            var vWorkspaceId = $("#ddlProject").val();
            var ProjectLockDetailData = {
                vWorkspaceId: vWorkspaceId
            }

            var ProjectLockDetailAjaxData = {
                async: false,
                data: ProjectLockDetailData,
                type: "POST",
                //url: ApiURL + "GetData/ProjectLockDetail",
                url: WebURL + "MIDicomStudy/ProjectLockDetail",
                success: MIDicomStudy.successProjectLockDetail,
                error: MIDicomStudy.errorProjectLockDetail
            }
            MIDicomStudy.fnProjectLockDetail(ProjectLockDetailAjaxData.async, ProjectLockDetailAjaxData.data, ProjectLockDetailAjaxData.type, ProjectLockDetailAjaxData.url, ProjectLockDetailAjaxData.success, ProjectLockDetailAjaxData.success, ProjectLockDetailAjaxData.error)
        }
        var fnProjectLockDetail = function (async, data, type, url, success, error) {
            $.ajax({
                //async: async,
                data: data,
                type: type,
                url: url,
                complete: success,
                error: error
            });
            return this;
        }
        function successProjectLockDetail(jsonData) {
            if (jsonData.responseText != "") {
                //jsonData = $.parseJSON(jsonData);
                if (JSON.parse(jsonData.responseText)[0].cLockFlag == "L") {
                    ProjectLockStatus = "L"
                    AlertBox("warning", "Dicom Study!", "Project is Locked!");
                    //$("#ddlProject").select2('val', '');
                    $("#ddlProject").blur();
                }
                else {
                    if ($("#select2-ddlProject-container")[0].innerHTML != "") {
                        $('#ddlSubActivity').find('option').remove().end().append('<option value="select">Select Activity</option>').val('select');
                        $("#select2-ddlSubActivity-container")[0].innerHTML = ""
                        $('#ddlActivity').find('option').remove().end().append('<option value="select">Select Activity</option>').val('select');
                        $("#select2-ddlActivity-container")[0].innerHTML = "";
                        MIDicomStudy.getSubjectDetail();
                        //MIDicomStudy.getActivityDetail();
                    }
                }
            }
            else {
                if ($("#select2-ddlProject-container")[0].innerHTML != "") {
                    $('#ddlSubActivity').find('option').remove().end().append('<option value="select">Select Activity</option>').val('select');
                    $("#select2-ddlSubActivity-container")[0].innerHTML = ""
                    $('#ddlActivity').find('option').remove().end().append('<option value="select">Select Activity</option>').val('select');
                    $("#select2-ddlActivity-container")[0].innerHTML = "";
                    MIDicomStudy.getSubjectDetail();
                    //MIDicomStudy.getActivityDetail();
                }
            }
        }
        function errorProjectLockDetail() {
            AlertBox("error", "Dicom Study!", "Error While Retriving Project Lock status!");
        }

        MIDicomStudy.ProjectLockDetail = ProjectLockDetail;
        MIDicomStudy.successProjectLockDetail = successProjectLockDetail;
        MIDicomStudy.errorProjectLockDetail = errorProjectLockDetail;
        MIDicomStudy.fnProjectLockDetail = fnProjectLockDetail;

    }($, MIDicomStudy, General));

    (function ($, MIDicomStudy, General) {
        'use strict';
        function getSubjectDetail() {
            $("#spanSkip").empty();
            $("#btnSkip").hide();
            $("#btnSkip2").hide();
            var vSubjectNo = "";
            var vWorkSpaceID = $("#ddlProject").val();
            var cSubjectFilter = "N";

            var subjectDetailData = {
                vSubjectNo: vSubjectNo,
                vWorkSpaceID: vWorkSpaceID,
                cSubjectFilter: cSubjectFilter,
                iUserId: $("#hdnuserid").val()
            }

            var subjectDetailAjaxData = {
                //async: false,
                data: subjectDetailData,
                type: "POST",

                //Added by rinkal
                //url: WebURL + "MIDicomStudy/SubjectDetailsForDISOFT",
                url: WebURL + "MIDicomStudy/SubjectDetailsForDISOFTWithDataEntryStatus",
                success: MIDicomStudy.successSubjectDetailAjaxData,
                error: MIDicomStudy.errorSubjectDetailAjaxData
            }

            MIDicomStudy.fnSubjectDetail(subjectDetailAjaxData.async, subjectDetailAjaxData.data, subjectDetailAjaxData.type, subjectDetailAjaxData.url, subjectDetailAjaxData.success, subjectDetailAjaxData.error)
        }
        var fnSubjectDetail = function (async, data, type, url, success, error) {
            setTimeout(function () {
                $.ajax({
                    async: false,
                    data: data,
                    type: type,
                    url: url,
                    success: success,
                    error: error
                });
            }, 0);
            return this;
        }
        function successSubjectDetailAjaxData(jsonData) {
            if (jsonData == "error") {
                AlertBox("error", "Dicom Study", "Error While Getting Subject Detail!")
            }
            else if (jsonData.length > 0) {
                jsonData = $.parseJSON(jsonData);
                $('#ddlSubject').find('option').remove().end().append('<option value="select">Select Subject</option>').val('select');
                $("#select2-ddlSubject-container")[0].innerHTML = ""
                for (var V = 0 ; V < jsonData.length ; V++) {
                    //$("#ddlSubject").append($("<option></option>").html(jsonData[V].DisplayName2).val(jsonData[V].vSubjectId + "#" + jsonData[V].vMySubjectNo + "#" + jsonData[V].iMySubjectNo + "#" + jsonData[V].iPeriod + "#" + jsonData[V].vParentWorkspaceId));
                    //$("#ddlSubject").append($("<option title='SUBJECTCODE'></option>").html(jsonData[V].vMySubjectNo).val(jsonData[V].vSubjectId + "#" + jsonData[V].vMySubjectNo + "#" + jsonData[V].iMySubjectNo + "#" + jsonData[V].iPeriod + "#" + jsonData[V].vParentWorkspaceId));
                    if (query == '' || query == "" || query == null) {
                        $("#ddlSubject").append($("<option title='" + jsonData[V].cDataStatusColorCode + "'></option>").html(jsonData[V].vMySubjectNo).val(jsonData[V].vSubjectId + "#" + jsonData[V].vMySubjectNo + "#" + jsonData[V].iMySubjectNo + "#" + jsonData[V].iPeriod + "#" + jsonData[V].vParentWorkspaceId));
                    }
                    else {
                        if (jsonData[V].vSubjectId == vSubjectId) {
                            $("#ddlSubject").append($("<option title='" + jsonData[V].cDataStatusColorCode + "'></option>").html(jsonData[V].vMySubjectNo).val(jsonData[V].vSubjectId + "#" + jsonData[V].vMySubjectNo + "#" + jsonData[V].iMySubjectNo + "#" + jsonData[V].iPeriod + "#" + jsonData[V].vParentWorkspaceId));
                            $("#select2-ddlSubject-container")[0].innerHTML = $("#ddlSubject").val(jsonData[V].vSubjectId + "#" + jsonData[V].vMySubjectNo + "#" + jsonData[V].iMySubjectNo + "#" + jsonData[V].iPeriod + "#" + jsonData[V].vParentWorkspaceId);
                            //$("#select2-ddlProject-container")[0].innerHTML = jsonData[V].WorkspaceMerge;
                            $("#ddlSubject").trigger("change");
                        }
                    }
                }
                if (localStorage.getItem("IsReviewDone") == "true" || localStorage.getItem("IsReviewContinue") == "true") {
                    $("#ddlSubject").val(Glo_Screenno).change();
                    if ($("#select2-ddlActivity-container")[0].innerHTML != "") {
                        MIDicomStudy.getActivityDetail();
                    }
                    //$("#ddlActivity").val(Glo_Activity).change();
                    //$("#ddlSubActivity").val(Glo_SubActivity).change();
                }
            }
            else {
                $('#ddlSubject').find('option').remove().end().append('<option value="select">Select Subject</option>').val('select');
                $("#select2-ddlSubject-container")[0].innerHTML = ""
                AlertBox("Information", "Dicom Study!", "No Data Available For Subject!")
            }
        }
        function errorSubjectDetailAjaxData() {

            AlertBox("error", "Dicom Study!", "Error While Retriving Project Subject Details!");
        }

        MIDicomStudy.getSubjectDetail = getSubjectDetail;
        MIDicomStudy.fnSubjectDetail = fnSubjectDetail;
        MIDicomStudy.successSubjectDetailAjaxData = successSubjectDetailAjaxData;
        MIDicomStudy.errorSubjectDetailAjaxData = errorSubjectDetailAjaxData;

    }($, MIDicomStudy, General));

    (function ($, MIDicomStudy, General) {
        "use strict";

        function getActivityDetail() {
            $("#spanSkip").empty();
            $("#btnSkip").hide();
            $("#btnSkip2").hide();
            var vWorkSpaceID = $("#ddlProject").val();
            var iPeriod = 1;
            var iParentNodeId = 1;
            var vUserTypeCode = $("#hdnUserTypeCode").val();
            var vSubjectId = $("#ddlSubject").val().split('#')[0];
            var iUserId = $("#hdnuserid").val();
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
                //url: ApiURL + "GetData/ProjectActivityDetails",
                url: WebURL + "MIDicomStudy/ProjectActivityDetails",
                type: "POST",
                data: activityDetailData,
                async: false,
                success: MIDicomStudy.successActivityDetail,
                error: MIDicomStudy.errorActivityDetail
            }

            MIDicomStudy.fnActivityDetail(activityDetailAjaxData.url, activityDetailAjaxData.type, activityDetailAjaxData.data, activityDetailAjaxData.async, activityDetailAjaxData.success, activityDetailAjaxData.error)
        }
        var fnActivityDetail = function (url, type, data, async, success, error) {
            setTimeout(function () {
                $.ajax({
                    url: url,
                    type: type,
                    data: data,
                    //async: async,
                    success: success,
                    error: error
                });
            }, 0);
        }
        function successActivityDetail(jsonData) {
            if (jsonData == "error") {
                AlertBox("error", "Dicom Study", "Error While Getting Activity Detail!")
            }

            else if (jsonData.length > 0) {
                var arryActivityList = [];
                jsonData = $.parseJSON(jsonData);
                $('#ddlActivity').find('option').remove().end().append('<option value="select">Select Activity</option>').val('select');
                $("#select2-ddlActivity-container")[0].innerHTML = ""
                for (var V = 0 ; V < jsonData.length; V++) {
                    if (query == '' || query == "" || query == null) {
                        $("#ddlActivity").append($('<option title="' + jsonData[V].cDataStatusColorCode + '"></option>').html(jsonData[V].vNodeDisplayName).val(jsonData[V].vActivityId + "#" + jsonData[V].iNodeId));
                        arryActivity.push([jsonData[V].vNodeDisplayName, "False"]);
                        arryActivityList.push([jsonData[V].vNodeDisplayName]);
                    }
                    else {
                        if (jsonData[V].vNodeDisplayName.toUpperCase() == ActivityName.toUpperCase()) {
                            $("#ddlActivity").append($("<option title='" + jsonData[V].cDataStatusColorCode + "'></option>").html(jsonData[V].vNodeDisplayName).val(jsonData[V].vActivityId + "#" + jsonData[V].iNodeId));
                            $("#select2-ddlActivity-container")[0].innerHTML = $("#ddlActivity").val(jsonData[V].vActivityId + "#" + jsonData[V].iNodeId);
                            arryActivity.push([jsonData[V].vNodeDisplayName, "False"]);
                            arryActivityList.push([jsonData[V].vNodeDisplayName]);
                            $("#ddlActivity").trigger("change");
                        }
                    }
                }
                ActivityData = JSON.stringify(arryActivityList);
                if (localStorage.getItem("IsReviewDone") == "true" || localStorage.getItem("IsReviewContinue") == "true") {
                    $("#ddlActivity").val(Glo_Activity).change();
                }
            }
            else {
                $('#ddlActivity').find('option').remove().end().append('<option value="select">Select Activity</option>').val('select');
                $("#select2-ddlActivity-container")[0].innerHTML = ""
                AlertBox("Information", "Dicom Study!", "No Data Available For Activity!")
            }
            
        }
        function errorActivityDetail() {
            AlertBox("error", "Dicom Study!", "Error While Retriving Project Activity Details!");
        }

        MIDicomStudy.getActivityDetail = getActivityDetail;
        MIDicomStudy.fnActivityDetail = fnActivityDetail;
        MIDicomStudy.successActivityDetail = successActivityDetail;
        MIDicomStudy.errorActivityDetail = errorActivityDetail;

    })($, MIDicomStudy, General);

    (function ($, MIDicomStudy, General) {

        "use strict";

        function getSubActivityDetail() {
            $("#spanSkip").empty();
            $("#btnSkip").hide();
            $("#btnSkip2").hide();
            var vWorkSpaceID = $("#ddlProject").val();
            var iPeriod = 1;
            var val = $("#ddlActivity").val().split('#')
            var iParentNodeId = val[1];
            var vUserTypeCode = $("#hdnUserTypeCode").val();
            var vSubjectId = $("#ddlSubject").val().split('#')[0];
            var iUserId = $("#hdnuserid").val();
            var MODE = '2';
            var vActivityName = $("#ddlActivity option:selected").text();
            //var AppSetting_UserTypeCodedocument = document.getElementById("getUserTypeCode").value;
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

            if (WorkFlowStageId == '') {
                var subActivityDetailAjaxData = {
                    //url: ApiURL + "GetData/ProjectActivityDetails",
                    url: WebURL + "MIDicomStudy/ProjectActivityDetails",
                    type: "POST",
                    data: subActivityDetailData,
                    async: false,
                    success: successSubActivityDetail,
                    error: errorSubActivityDetail
                }
            }
            else {
                var subActivityDetailAjaxData = {
                    //url: ApiURL + "GetData/ProjectActivityDetails",
                    url: WebURL + "MIDicomStudy/ProjectActivityDetails",
                    type: "POST",
                    data: subActivityDetailData,
                    async: false,
                    success: successSubActivityDetailForQA,
                    error: errorSubActivityDetailForQA
                }
            }

            MIDicomStudy.fnSubActivityDetail(subActivityDetailAjaxData.url, subActivityDetailAjaxData.type, subActivityDetailAjaxData.data, subActivityDetailAjaxData.async, subActivityDetailAjaxData.success, subActivityDetailAjaxData.error)
        }
        var fnSubActivityDetail = function (url, type, data, async, success, error) {
            setTimeout(function () {
                $.ajax({
                    url: url,
                    type: type,
                    data: data,
                    //async: async,
                    success: success,
                    error: error
                });
            }, 0);
        }
        function successSubActivityDetail(jsonData) {
            debugger;
            if (jsonData == "error") {
                AlertBox("error", "Dicom Study", "Error While Getting Subject Detail!")
            }
            else if (jsonData.length > 0) {
                var arrySubActivityList = [];
                var arrySubActivityNameList = [];
                jsonData = $.parseJSON(jsonData);
                $('#ddlSubActivity').find('option').remove().end().append('<option value="select">Select Activity</option>').val('select');
                $("#select2-ddlSubActivity-container")[0].innerHTML = ""
                var strSingle = "";
                for (var V = 0 ; V < jsonData.length; V++) {
                    if (query == '' || query == "" || query == null) {
                        $("#ddlSubActivity").append($('<option title="' + jsonData[V].cDataStatusColorCode + '"></option>').html(jsonData[V].vNodeDisplayName).val(jsonData[V].vActivityId + "#" + jsonData[V].iNodeId));
                        arrySubActivityList.push(jsonData[V].vActivityId + "#" + jsonData[V].iNodeId);
                        arrySubActivityNameList.push(jsonData[V].vNodeDisplayName);
                        if (V == 0) {
                            strSingle = jsonData[V].vActivityId + "#" + jsonData[V].iNodeId;
                        }
                    }
                    else {
                        if (jsonData[V].vNodeDisplayName.split('-')[1].toUpperCase() == SubActName.toUpperCase()) {
                            $("#ddlSubActivity").append($('<option title="' + jsonData[V].cDataStatusColorCode + '"></option>').html(jsonData[V].vNodeDisplayName).val(jsonData[V].vActivityId + "#" + jsonData[V].iNodeId));
                            $("#select2-ddlSubActivity-container")[0].innerHTML = $("#ddlSubActivity").val(jsonData[V].vActivityId + "#" + jsonData[V].iNodeId);
                            arrySubActivityList.push(jsonData[V].vActivityId + "#" + jsonData[V].iNodeId);
                            arrySubActivityNameList.push(jsonData[V].vNodeDisplayName);
                            var select = $("#select2-ddlSubActivity-container")[0];
                            select.innerHTML = $("#ddlSubActivity option:selected").text();
                            if (V == 0) {
                                strSingle = jsonData[V].vActivityId + "#" + jsonData[V].iNodeId;
                            }
                            $("#ddlSubActivity option:selected").text()
                        }

                    }
                }
                SubActivityData = JSON.stringify(arrySubActivityList);
                SubActivityNameData = JSON.stringify(arrySubActivityNameList);
                //arrStorage.push("SubActivityList", JSON.stringify(arrySubActivityList));
                //arrStorage.push("SubActivityNameList", JSON.stringify(arrySubActivityNameList));
                //if (arrStorage.IsReviewDone == "true" || arrStorage.IsReviewContinue == "true")
                if (localStorage.getItem("IsReviewDone") == "true" || localStorage.getItem("IsReviewContinue") == "true") {
                    $("#ddlSubActivity").val(Glo_SubActivity).change();
                    localStorage.setItem("IsReviewDone", "false");
                    localStorage.setItem("IsReviewContinue", "false");
                    MIDicomStudy.getSubjectStudyDetail()
                }

                var strActivityName = $("#select2-ddlActivity-container")[0].innerHTML;
                //if (strActivityName.toUpperCase().match("ELIGIBILITY-REVIEW") || strActivityName.toUpperCase().match("MARK") || strActivityName.toUpperCase().match("BL") || (strActivityName.toUpperCase().match("GLOBAL") || strActivityName.toUpperCase().match("RESPONSE")) || strActivityName.toUpperCase().match("ADJUDICATOR") || strActivityName.toUpperCase().match("IOV ASSESSMENT")) {
                //    $(".SubActivity").show();
                //}
                //else {
                //    $("#ddlSubActivity").val(strSingle).change();
                //    $(".SubActivity").hide();
                //}
                // 08-Dec-2022 remove strActivityName.toUpperCase().match("BL") condition
                if (strActivityName.toUpperCase().match("ELIGIBILITY-REVIEW") || strActivityName.toUpperCase().match("MARK") || (strActivityName.toUpperCase().match("GLOBAL") || strActivityName.toUpperCase().match("RESPONSE")) || strActivityName.toUpperCase().match("ADJUDICATOR") || strActivityName.toUpperCase().match("IOV ASSESSMENT")) {
                    $(".SubActivity").show();
                }
                else {
                    $("#ddlSubActivity").val(strSingle).change();
                    $(".SubActivity").hide();
                }
                if (query != '' || query != "") {
                    if ($("#select2-ddlActivity-container")[0].innerHTML != "") {
                        if (RDicom.toUpperCase() == "R1") {
                            $("#btnGo").trigger("click");
                            $("#btnSkip").trigger("click");
                        }
                        else if (RDicom.toUpperCase() == "R2") {
                            $("#btnGo").trigger("click");
                            $("#btnSkip2").trigger("click");
                        }
                    }
                }
            }

            else {
                $('#mySelect').find('option').remove().end().append('<option value="select">Select Sub Activity</option>').val('select');
                $("#select2-ddlSubActivity-container")[0].innerHTML = ""
                AlertBox("Information", "Dicom Study!", "No Data Available For Sub Activity!")
            }
            }
            function errorSubActivityDetail() {
                AlertBox("error", "Dicom Study!", "Error While Retriving  Sub Activity Details!");
            }


        //-------------For QA Profile---------------------------
            function successSubActivityDetailForQA(jsonData) {
                debugger;
                if (jsonData == "error") {
                    AlertBox("error", "Dicom Study", "Error While Getting Subject Detail!")
                }
                else if (jsonData.length > 0) {
                    var arrySubActivityList = [];
                    var arrySubActivityNameList = [];
                    var indexArr = [];
                    var TempjsonData = $.parseJSON(jsonData);
                                        
                    for (var q = 0 ; q < TempjsonData.length; q++) {
                        if (TempjsonData[q].vNodeDisplayName.toUpperCase().includes("QC1 REVIEW") || TempjsonData[q].vNodeDisplayName.toUpperCase().includes("QC2 REVIEW") || TempjsonData[q].vNodeDisplayName.toUpperCase().includes("CASE ASSIGNMENT")) {
                            indexArr.push(q);
                        }
                    }
                    const indexSet = new Set(indexArr);
                    jsonData = TempjsonData.filter((value, i) => !indexSet.has(i));
                    console.log(jsonData);
                    var ArrCount = 0;
                    $('#ddlSubActivity').find('option').remove().end().append('<option value="select">Select Activity</option>').val('select');
                    $("#select2-ddlSubActivity-container")[0].innerHTML = ""
                    var strSingle = "";
                    for (var V = 0 ; V < jsonData.length; V++) {
                        if (SubActivityAdjName == '') {
                            if (jsonData[V].vNodeDisplayName.toUpperCase().split('-')[0] == RDicom.toUpperCase()) {
                                $("#ddlSubActivity").append($('<option title="' + jsonData[V].cDataStatusColorCode + '"></option>').html(jsonData[V].vNodeDisplayName).val(jsonData[V].vActivityId + "#" + jsonData[V].iNodeId));
                                arrySubActivityList.push(jsonData[V].vActivityId + "#" + jsonData[V].iNodeId);
                                arrySubActivityNameList.push(jsonData[V].vNodeDisplayName);
                                if (ArrCount == 0) {
                                    strSingle = jsonData[V].vActivityId + "#" + jsonData[V].iNodeId;
                                    ArrCount++;
                                }
                            }
                        }
                        else {
                            if (jsonData[V].vNodeDisplayName.toUpperCase().split('-')[1] == SubActivityAdjName) {
                                $("#ddlSubActivity").append($('<option title="' + jsonData[V].cDataStatusColorCode + '"></option>').html(jsonData[V].vNodeDisplayName).val(jsonData[V].vActivityId + "#" + jsonData[V].iNodeId));
                                $("#select2-ddlSubActivity-container")[0].innerHTML = $("#ddlSubActivity").val(jsonData[V].vActivityId + "#" + jsonData[V].iNodeId);
                                arrySubActivityList.push(jsonData[V].vActivityId + "#" + jsonData[V].iNodeId);
                                arrySubActivityNameList.push(jsonData[V].vNodeDisplayName);
                                var select = $("#select2-ddlSubActivity-container")[0];
                                select.innerHTML = $("#ddlSubActivity option:selected").text();
                                if (V == 0) {
                                    strSingle = jsonData[V].vActivityId + "#" + jsonData[V].iNodeId;
                                }
                                $("#ddlSubActivity option:selected").text()
                            }
                        }
                        
                        //else {
                        //    if (jsonData[V].vNodeDisplayName.toUpperCase().split('-')[0] == SubActName.toUpperCase().split("-")[0]) {
                        //        $("#ddlSubActivity").append($('<option title="' + jsonData[V].cDataStatusColorCode + '"></option>').html(jsonData[V].vNodeDisplayName).val(jsonData[V].vActivityId + "#" + jsonData[V].iNodeId));
                        //        $("#select2-ddlSubActivity-container")[0].innerHTML = $("#ddlSubActivity").val(jsonData[V].vActivityId + "#" + jsonData[V].iNodeId);
                        //        arrySubActivityList.push(jsonData[V].vActivityId + "#" + jsonData[V].iNodeId);
                        //        arrySubActivityNameList.push(jsonData[V].vNodeDisplayName);
                        //        var select = $("#select2-ddlSubActivity-container")[0];
                        //        select.innerHTML = $("#ddlSubActivity option:selected").text();
                        //        if (V == 0) {
                        //            strSingle = jsonData[V].vActivityId + "#" + jsonData[V].iNodeId;
                        //        }
                        //        $("#ddlSubActivity option:selected").text()
                        //    }
                        //}
                    }
                    SubActivityData = JSON.stringify(arrySubActivityList);
                    SubActivityNameData = JSON.stringify(arrySubActivityNameList);
                    //arrStorage.push("SubActivityList", JSON.stringify(arrySubActivityList));
                    //arrStorage.push("SubActivityNameList", JSON.stringify(arrySubActivityNameList));
                    //if (arrStorage.IsReviewDone == "true" || arrStorage.IsReviewContinue == "true")
                    if (localStorage.getItem("IsReviewDone") == "true" || localStorage.getItem("IsReviewContinue") == "true") {
                        $("#ddlSubActivity").val(Glo_SubActivity).change();
                        localStorage.setItem("IsReviewDone", "false");
                        localStorage.setItem("IsReviewContinue", "false");
                        MIDicomStudy.getSubjectStudyDetail()
                    }

                    var strActivityName = $("#select2-ddlActivity-container")[0].innerHTML;
                    //if (strActivityName.toUpperCase().match("ELIGIBILITY-REVIEW") || strActivityName.toUpperCase().match("MARK") || strActivityName.toUpperCase().match("BL") || (strActivityName.toUpperCase().match("GLOBAL") || strActivityName.toUpperCase().match("RESPONSE")) || strActivityName.toUpperCase().match("ADJUDICATOR") || strActivityName.toUpperCase().match("IOV ASSESSMENT")) {
                    //    $(".SubActivity").show();
                    //}
                    //else {
                    //    $("#ddlSubActivity").val(strSingle).change();
                    //    $(".SubActivity").hide();
                    //}
                    // 08-Dec-2022 remove strActivityName.toUpperCase().match("BL") condition
                    if (strActivityName.toUpperCase().match("ELIGIBILITY-REVIEW") || strActivityName.toUpperCase().match("MARK") || (strActivityName.toUpperCase().match("GLOBAL") || strActivityName.toUpperCase().match("RESPONSE")) || strActivityName.toUpperCase().match("ADJUDICATOR") || strActivityName.toUpperCase().match("IOV ASSESSMENT")) {
                        $(".SubActivity").show();
                    }
                    else {
                        $("#ddlSubActivity").val(strSingle).change();
                        $(".SubActivity").hide();
                    }
                    if (query != '' || query != "") {
                        if ($("#select2-ddlActivity-container")[0].innerHTML != "") {
                            if (RDicom.toUpperCase() == "R1") {
                                $("#btnGo").trigger("click");
                                $("#btnSkip").trigger("click");
                                setTimeout(function () {
                                    $("#tblSubjectReviewList tr td:eq(0) a").trigger("click");
                                }, 500);
                            }
                            else if (RDicom.toUpperCase() == "R2") {
                                $("#btnGo").trigger("click");
                                $("#btnSkip2").trigger("click");
                                setTimeout(function () {
                                    $("#tblSubjectReviewList tr td:eq(0) a").trigger("click");
                                }, 500);
                            }
                        }
                    }
                }

                else {
                    $('#mySelect').find('option').remove().end().append('<option value="select">Select Sub Activity</option>').val('select');
                    $("#select2-ddlSubActivity-container")[0].innerHTML = ""
                    AlertBox("Information", "Dicom Study!", "No Data Available For Sub Activity!")
                }
            }
            function errorSubActivityDetailForQA() {
                AlertBox("error", "Dicom Study!", "Error While Retriving  Sub Activity Details!");
            }
        //-----------------------------------------------------


            MIDicomStudy.getSubActivityDetail = getSubActivityDetail;
            MIDicomStudy.fnSubActivityDetail = fnSubActivityDetail;
            MIDicomStudy.successSubActivityDetail = successSubActivityDetail;
            MIDicomStudy.errorSubActivityDetail = errorSubActivityDetail;

        })($, MIDicomStudy, General);

        (function ($, MIDicomStudy, General) {
            "use strict"
            
            function skipVisit(profile) {
                if (profile != undefined) {
                    debugger;
                    var vWorkSpaceId = $("#ddlProject").val();
                    var subjectVal = $("#ddlSubject").val().split('#');
                    var vSubjectId = subjectVal[0];
                    var vMySubjectNo = subjectVal[1];
                    var iMySubjectNo = subjectVal[2];
                    var iPeriod = subjectVal[3];
                    var vParentWorkspaceId = subjectVal[4];
                    var ddlActivityVal = $("#ddlActivity").val();
                    var val = ddlActivityVal.split('#');
                    var vActivityId = val[0];
                    var iNodeId = val[1];
                    var ddlSubActivityVal = $("#ddlSubActivity").val().split('#');
                    var vSubActivityId = ddlSubActivityVal[0];
                    var iSubNodeId = ddlSubActivityVal[1];
                    var vActivityName = $("#select2-ddlActivity-container")[0].innerHTML;
                    var vSubActivityName = $("#select2-ddlSubActivity-container")[0].innerHTML;
                    var vUserType = profile;
                    var SubActiviName = $("#ddlSubActivity option:selected").text().split("-")[1];
                    var url = $("#DicomViewer").val();
                    //location.href = url;

                    var subjectStudyDetailDataRadiologist = {
                        vWorkspaceId: vWorkSpaceId,
                        vSubjectId: vSubjectId,
                        iNodeId: SubActiviName,
                        cRadiologist: vUserType,
                        MODE: "1",
                    }
                    $.ajax({
                        //url: ApiURL + "GetData/SubjectStudyDetail",
                        url: WebURL + "MIDicomStudy/SubjectStudyDetail",
                        type: "POST",
                        data: subjectStudyDetailDataRadiologist,
                        //async: false,
                        success: successSubjectStudyDetailRadiologist,
                        error: errorSubjectStudyDetailRadiologist
                    });
                    function successSubjectStudyDetailRadiologist(jsonData) {
                        debugger;
                        
                        //var RadioLogistData = MIDicomStudy.GetRadiologistData(vWorkSpaceId);
                        MIDicomStudy.GetRadiologistData(vWorkSpaceId);
                        if (jsonData.length > 0) {
                            jsonData = $.parseJSON(jsonData)[0]
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
                            strFormElement += '<input type="hidden" name="vSkipVisit" value="Y">';
                            strFormElement += '<input type="hidden" name="subjectRejectionDtl" value="' + isSubjectReject + '">';
                            strFormElement += '<input type="hidden" name="activityArray" value="' + arryActivity.join() + '">';
                            strFormElement += '<input type="hidden" name="vUserType" value="R1">';
                            strFormElement += '<input type="hidden" name="vUserTypeR2" value="R2">';

                            strFormElement += '<input type="hidden" name="iUserIdR1" value="' + R1UserId + '">';
                            strFormElement += '<input type="hidden" name="iUserIdR2" value="' + R2UserId + '">';
                            strFormElement += '<input type="hidden" name="vUserTypeCodeR1" value="' + R1UserType + '">';
                            strFormElement += '<input type="hidden" name="vUserTypeCodeR2" value="' + R2userType + '">';

                            strFormElement += '<input type="hidden" name="iImgTransmittalHdrId" value="' + jsonData.iImgTransmittalHdrId + '">';
                            strFormElement += '<input type="hidden" name="iImgTransmittalDtlId" value="' + jsonData.iImgTransmittalDtlId + '">';
                            strFormElement += '<input type="hidden" name="iImageStatus" value="' + jsonData.iImageStatus + '">';
                            strFormElement += '<input type="hidden" name="vProjectNo" value="' + jsonData.vProjectNo + '">';
                            strFormElement += '<input type="hidden" name="iModalityNo" value="' + jsonData.iModalityNo + '">';
                            strFormElement += '<input type="hidden" name="iAnatomyNo" value="' + jsonData.iAnatomyNo + '">';
                            strFormElement += '<input type="hidden" name="iImageCount" value="' + jsonData.iImageCount + '">';
                            strFormElement += '<input type="hidden" name="ImgTransmittalDtl_iImageTranNo" value="' + jsonData.ImgTransmittalDtl_iImageTranNo + '">';
                            strFormElement += '<input type="hidden" name="ImageTransmittalImgDtl_iImageTranNo" value="' + jsonData.ImageTransmittalImgDtl_iImageTranNo + '">';
                            strFormElement += '<input type="hidden" name="arrStorage" value="' + encodeURI(JSON.stringify(arrStorage)) + '">';
                            strFormElement += '<input type="hidden" name="activityData" value="' + encodeURI(ActivityData) + '">';
                            strFormElement += '<input type="hidden" name="subActivityData" value="' + encodeURI(SubActivityData) + '">';
                            strFormElement += '<input type="hidden" name="subActivityNameData" value="' + encodeURI(SubActivityNameData) + '">';
                            strFormElement += '<input type="hidden" name="WorkFlowStageId" value="' + WorkFlowStageId + '">';
                            strFormElement += '</form>';

                            var form = $(strFormElement);
                            $(document.body).append(form[0]); //Resolve Chrome Version Change Issue 
                            form.submit();
                        }
                    }
                    function errorSubjectStudyDetailRadiologist(){
                        return false;
                    }
                    //$(".custom-form").remove();
                    return false;
                }
            }
            MIDicomStudy.skipVisit = skipVisit;

        })($, MIDicomStudy, General);
        
        (function ($, MIDicomStudy, General) {
            "use strict";
            function GetRadiologistData(WorkSpaceId) {
                var RadiologistData = {
                    vWorkspaceId: WorkSpaceId
                }
                $.ajax({
                    url: ApiURL + "GetData/GetRadioLogistData",
                    type: "GET",
                    data: RadiologistData,
                    async: false,
                    success: successGetRadiologistData, 
                    error: errorGetRadiologistData
                });
                function successGetRadiologistData(data) {
                    for (var d = 0; d < data.length; d++) {
                        if (data[d].vUserTypeName == "Radiologist1") {
                            R1UserId = data[d].iUserId;
                            R1UserType = data[d].vUsertypecode;
                        }
                        if (data[d].vUserTypeName == "Radiologist2") {
                            R2UserId = data[d].iUserId;
                            R2userType = data[d].vUsertypecode;
                        }
                    }
                }
                function errorGetRadiologistData() {
                    return false;
                }
                return false;
            }
            MIDicomStudy.GetRadiologistData = GetRadiologistData;
        })($, MIDicomStudy, General);
        
        (function ($, MIDicomStudy, General) {
            "use strict";
            function subjectImageStudyDetail(e) {
                var vWorkSpaceId = $("#ddlProject").val();
                var subjectVal = $("#ddlSubject").val().split('#');
                var vSubjectId = subjectVal[0];
                var vMySubjectNo = subjectVal[1];
                var iMySubjectNo = subjectVal[2];
                var iPeriod = subjectVal[3];
                var vParentWorkspaceId = subjectVal[4];
                var ddlActivityVal = $("#ddlActivity").val();
                var val = ddlActivityVal.split('#');
                var vActivityId = val[0];
                var iNodeId = val[1];
                var ddlSubActivityVal = $("#ddlSubActivity").val().split('#');
                var vSubActivityId = ddlSubActivityVal[0];
                var iSubNodeId = ddlSubActivityVal[1];
                var vActivityName = $("#select2-ddlActivity-container")[0].innerHTML;
                var vSubActivityName = $("#select2-ddlSubActivity-container")[0].innerHTML;
                ///////////////////////////////////////////////////////////////////////////////////
                var iImgTransmittalHdrId = $(e).attr("iImgTransmittalHdrId");
                var iImgTransmittalDtlId = $(e).attr("iImgTransmittalDtlId");
                var iImageStatus = $(e).attr("iImageStatus");
                var vWorkspaceId = $(e).attr("vWorkspaceId");
                var vProjectNo = $(e).attr("vProjectNo");
                var vSubjectId = $(e).attr("vSubjectId");
                var iModalityNo = $(e).attr("iModalityNo");
                //var iAnatomyNo = $(e).attr("iAnatomyNo"); // Changed Because Not Required Further Anatomy Wise Description
                var iAnatomyNo = "";
                var iNodeId = $(e).attr("iNodeId");
                var ImgTransmittalDtl_iImageTranNo = $(e).attr("ImgTransmittalDtl_iImageTranNo");
                var ImageTransmittalImgDtl_iImageTranNo = $(e).attr("ImageTransmittalImgDtl_iImageTranNo");

                var iImageCount = $('#tblSubjectReviewList').DataTable().rows()[0].length;

                var url = $("#DicomViewer").val();
                //location.href = url;

                if (!($("#select2-ddlActivity-container")[0].innerHTML == $(e).attr("Activity"))) {
                    AlertBox("warning", "Dicom Study!", "Please Select Proper Matching Activity!");
                    return false;
                }


                else if (!($("#select2-ddlSubActivity-container")[0].innerHTML == $(e).attr("SubActivity"))) {
                    AlertBox("warning", "Dicom Study!", "Please Select Proper Matching Sub Activity!");
                    return false;
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
                strFormElement += '<input type="hidden" name="iImageCount" value="' + "1" + '">';
                strFormElement += '<input type="hidden" name="ImgTransmittalDtl_iImageTranNo" value="' + ImgTransmittalDtl_iImageTranNo + '">';
                strFormElement += '<input type="hidden" name="ImageTransmittalImgDtl_iImageTranNo" value="' + ImageTransmittalImgDtl_iImageTranNo + '">';
                strFormElement += '<input type="hidden" name="subjectRejectionDtl" value="' + isSubjectReject + '">';
                strFormElement += '<input type="hidden" name="activityArray" value="' + arryActivity.join() + '">';
                strFormElement += '<input type="hidden" name="arrStorage" value="' + encodeURI(JSON.stringify(arrStorage)) + '">';
                strFormElement += '<input type="hidden" name="activityData" value="' + encodeURI(ActivityData) + '">';
                strFormElement += '<input type="hidden" name="subActivityData" value="' + encodeURI(SubActivityData) + '">';
                strFormElement += '<input type="hidden" name="subActivityNameData" value="' + encodeURI(SubActivityNameData) + '">';
                strFormElement += '<input type="hidden" name="WorkFlowStageId" value="' + WorkFlowStageId + '">';
                strFormElement += '</form>';

                var form = $(strFormElement);
                $(document.body).append(form[0]); //Resolve Chrome Version Change Issue 
                form.submit();

                $(".custom-form").remove();

                localStorage.setItem("IsReviewDone", "false");
                localStorage.setItem("IsReviewContinue", "false");

                var MyInterval = setInterval(function (ele) {
                    if (localStorage.getItem("IsReviewDone") == "true") {
                        clearInterval(MyInterval);
                        Glo_Screenno = $("#ddlSubject").val();
                        Glo_Activity = $("#ddlActivity").val();
                        Glo_SubActivity = $("#ddlSubActivity").val();
                        if ($("#select2-ddlProject-container")[0].innerHTML != "") {
                            MIDicomStudy.getSubjectDetail();
                        }
                    }
                }, 1000);

                var MyInterval2 = setInterval(function (ele) {
                    if (localStorage.getItem("IsReviewContinue") == "true") {
                        clearInterval(MyInterval2);
                        Glo_Screenno = $("#ddlSubject").val();
                        Glo_Activity = $("#ddlActivity").val();
                        Glo_SubActivity = $("#ddlSubActivity").val();
                        if ($("#select2-ddlProject-container")[0].innerHTML != "") {
                            MIDicomStudy.getSubjectDetail();
                        }
                    }
                }, 1000);

                return false;

            }
            MIDicomStudy.subjectImageStudyDetail = subjectImageStudyDetail;

        })($, MIDicomStudy, General);

        (function ($, MIDicomStudy, General) {
            "use strict";

            function getSubjectStudyDetail() {
                debugger;
                $("#spanSkip").empty();
                $("#btnSkip").hide();
                $("#btnSkip2").hide();
                if (MIDicomStudy.validate() == false) {
                    if ($(tblSubjectReviewList) != 'undefined') {
                        if ($.fn.DataTable.isDataTable('#tblSubjectReviewList')) {
                            var table = $('#tblSubjectReviewList').DataTable();
                            table.clear();
                            table.destroy();
                            $("#tblSubjectReviewList").find("thead").html("");
                        }
                    }
                    $("#legend").hide();
                    $("#spanSkip").empty();
                    $("#btnSkip").hide();
                    $("#btnSkip2").hide();
                    return false;
                }
                else {
                    if ($("#ddlProject").val() != "" && $("#ddlProject").val() != "select" && $("#ddlProject").val() != null) {
                        if ($("#ddlSubject").val() != "" && $("#ddlSubject").val() != "select" && $("#ddlSubject").val() != null) {
                            if ($("#ddlActivity").val() != "" && $("#ddlActivity").val() != "select" && $("#ddlActivity").val() != null) {
                                if ($("#ddlSubActivity").val() != "" && $("#ddlSubActivity").val() != "select" && $("#ddlSubActivity").val() != null) {
                                    var vWorkSpaceId = $("#ddlProject").val();
                                    var subjectVal = $("#ddlSubject").val().split('#');
                                    var vSubjectId = subjectVal[0];
                                    var vMySubjectNo = subjectVal[1];
                                    var iMySubjectNo = subjectVal[2];
                                    var iPeriod = subjectVal[3];
                                    var vParentWorkspaceId = subjectVal[4];
                                    var ddlActivityVal = $("#ddlActivity").val();
                                    var val = ddlActivityVal.split('#');
                                    var vActivityId = val[0];
                                    var iNodeId = val[1];
                                    var ddlSubActivityVal = $("#ddlSubActivity").val().split('#');
                                    var vSubActivityId = ddlSubActivityVal[0];
                                    var iSubNodeId = ddlSubActivityVal[1];
                                    var vActivityName = $("#select2-ddlActivity-container")[0].innerHTML;
                                    var vSubActivityName = $("#select2-ddlSubActivity-container")[0].innerHTML;
                                    var cRadiologist;
                                    cRadiologist = vSubActivityName.split('-')[0]

                                    var DataSaveStatus = false;

                                    //if ((!vActivityName.toUpperCase().match("MARK")) && (!vActivityName.toUpperCase().match("ADJUDICATOR")) && (!vActivityName.toUpperCase().match("GLOBAL"))) {
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
                                            //url: ApiURL + "GetData/MI_DataSaveStatus",
                                            url: WebURL + "MIDicomStudy/MI_DataSaveStatus",
                                            type: "POST",
                                            data: MI_DataSaveStatus,
                                            async: false,
                                            success: function (jsonDataSaveStatus) {
                                                //var data = jsonDataSaveStatus;
                                                //var data = jsonDataSaveStatus.split("#")[0];
                                                var splitVal = jsonDataSaveStatus.split("@")[0];
                                                var data = splitVal.split("#")[0];
                                                if (data == "NOTALLOW" && jsonDataSaveStatus.split("@")[1].split("#")[0].toUpperCase() == "BLOCK") {
                                                    AlertBox("warning", "Image Review!", "Subject is Rejected And Dicom Study For " + vActivityName + " Is Not Assigned Yet.! ");
                                                }


                                                else if (data == "NOTALLOW") {
                                                    AlertBox("warning", "Image Review!", "Dicom Study For " + vActivityName + " Is Not Assigned Yet.!");
                                                    if ($(tblSubjectReviewList) != 'undefined') {
                                                        if ($.fn.DataTable.isDataTable('#tblSubjectReviewList')) {
                                                            var table = $('#tblSubjectReviewList').DataTable();
                                                            table.clear();
                                                            table.destroy();
                                                            $("#tblSubjectReviewList").find("thead").html("");
                                                        }
                                                    }
                                                    DataSaveStatus = false;
                                                    removeDiv();
                                                    $(".spinner").hide();
                                                    //return false;
                                                }
                                                else if (data == "ERROR") {
                                                    AlertBox("error", "Image Review!", "Error While Retriving Image Assignment Status.!");
                                                    if ($(tblSubjectReviewList) != 'undefined') {
                                                        if ($.fn.DataTable.isDataTable('#tblSubjectReviewList')) {
                                                            var table = $('#tblSubjectReviewList').DataTable();
                                                            table.clear();
                                                            table.destroy();
                                                            $("#tblSubjectReviewList").find("thead").html("");
                                                        }
                                                    }
                                                    DataSaveStatus = false;
                                                    removeDiv();
                                                    $(".spinner").hide();
                                                    //return false;
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
                                                        if ($(tblSubjectReviewList) != 'undefined') {
                                                            if ($.fn.DataTable.isDataTable('#tblSubjectReviewList')) {
                                                                var table = $('#tblSubjectReviewList').DataTable();
                                                                table.clear();
                                                                table.destroy();
                                                                $("#tblSubjectReviewList").find("thead").html("");
                                                            }
                                                        }
                                                    }

                                                }
                                                else {
                                                    AlertBox("error", "Image Review!", "Error While Retriving Image Assignment Status.!");
                                                    if ($(tblSubjectReviewList) != 'undefined') {
                                                        if ($.fn.DataTable.isDataTable('#tblSubjectReviewList')) {
                                                            var table = $('#tblSubjectReviewList').DataTable();
                                                            table.clear();
                                                            table.destroy();
                                                            $("#tblSubjectReviewList").find("thead").html("");
                                                        }
                                                    }
                                                    DataSaveStatus = false;
                                                    removeDiv();
                                                    $(".spinner").hide();
                                                    //return false;                        
                                                }
                                            },
                                            error: function (e) {
                                                var error = e;
                                                AlertBox("error", "Image Review!", "Error While Retriving Image Assignment Status.!");
                                                if ($(tblSubjectReviewList) != 'undefined') {
                                                    if ($.fn.DataTable.isDataTable('#tblSubjectReviewList')) {
                                                        var table = $('#tblSubjectReviewList').DataTable();
                                                        table.clear();
                                                        table.destroy();
                                                        $("#tblSubjectReviewList").find("thead").html("");
                                                    }
                                                }
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
                                            //url: ApiURL + "GetData/CRFDataEntryStatus",
                                            url: WebURL + "MIDicomStudy/CRFDataEntryStatus",
                                            type: "POST",
                                            data: CRFDataEntryStatus,
                                            //async: false,
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
                                                if ((isSubjectReject == "Y") || $("#select2-ddlActivity-container")[0].innerHTML.toUpperCase().match("GLOBAL RESPONSE")) {
                                                    jsonData = "success";
                                                }
                                                var data = jsonData.split("#")[0];
                                                if (data == "error") {
                                                    if ($(tblSubjectReviewList) != 'undefined') {
                                                        if ($.fn.DataTable.isDataTable('#tblSubjectReviewList')) {
                                                            var table = $('#tblSubjectReviewList').DataTable();
                                                            table.clear();
                                                            table.destroy();
                                                            $("#tblSubjectReviewList").find("thead").html("");
                                                        }
                                                    }
                                                    $("#legend").hide();
                                                    AlertBox("error", "Image Review!", "Error While Retriving CRF Data Entry Control Details.!");
                                                    return false;
                                                }
                                                else if (data == "NO-DATA") {
                                                    if ($(tblSubjectReviewList) != 'undefined') {
                                                        if ($.fn.DataTable.isDataTable('#tblSubjectReviewList')) {
                                                            var table = $('#tblSubjectReviewList').DataTable();
                                                            table.clear();
                                                            table.destroy();
                                                            $("#tblSubjectReviewList").find("thead").html("");
                                                        }
                                                    }
                                                    $("#legend").hide();
                                                    AlertBox("error", "Image Review!", "No Data For CRF Data Entry Control Details.!");
                                                    return false;
                                                }
                                                else if (data == "success") {
                                                    var subjectStudyDetailData = {
                                                        vWorkspaceId: vWorkSpaceId,
                                                        vSubjectId: vSubjectId,
                                                        iNodeId: iNodeId,
                                                        cRadiologist: cRadiologist,
                                                        MODE: "1",
                                                    }
                                                    $.ajax({
                                                        //url: ApiURL + "GetData/SubjectStudyDetail",
                                                        url: WebURL + "MIDicomStudy/SubjectStudyDetail",
                                                        type: "POST",
                                                        data: subjectStudyDetailData,
                                                        //async: false,
                                                        success: MIDicomStudy.successSubjectStudyDetail,
                                                        error: MIDicomStudy.errorSubjectStudyDetail
                                                    });
                                                }
                                                else {
                                                    if ($(tblSubjectReviewList) != 'undefined') {
                                                        if ($.fn.DataTable.isDataTable('#tblSubjectReviewList')) {
                                                            var table = $('#tblSubjectReviewList').DataTable();
                                                            table.clear();
                                                            table.destroy();
                                                            $("#tblSubjectReviewList").find("thead").html("");
                                                        }
                                                    }
                                                    $("#legend").hide();
                                                    if (data == "") {
                                                        AlertBox("warning", "Image Review!", "No Detail Found.!")
                                                    }
                                                    else {
                                                        AlertBox("warning", "Image Review!", data)
                                                    }
                                                    removeDiv();
                                                    $(".spinner").hide();
                                                    //subjectStudyDetailData = {
                                                    //    vWorkspaceId: vWorkSpaceId,
                                                    //    vSubjectId: vSubjectId,
                                                    //    iNodeId: iNodeId,
                                                    //    cRadiologist: cRadiologist,
                                                    //    MODE: "1"
                                                    //}
                                                    //$.ajax({
                                                    //    url: ApiURL + "GetData/SubjectStudyDetail",
                                                    //    type: "POST",
                                                    //    data: subjectStudyDetailData,
                                                    //    //async: false,
                                                    //    success: successSubjectStudyDetail,
                                                    //    error: errorSubjectStudyDetail
                                                    //});
                                                    return false;
                                                }
                                            },
                                            error: function (e) {
                                                alert(e);
                                            }
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }
            function successSubjectStudyDetail(jsonData) {
                debugger;
                if (jsonData == "error") {
                    AlertBox("error", "Dicom Study", "Error While Getting Subject Study Detail!")
                    if ($.fn.DataTable.isDataTable('#tblSubjectReviewList')) {
                        var table = $('#tblSubjectReviewList').DataTable();
                        table.clear();
                        table.destroy();
                        $("#tblSubjectReviewList").find("thead").html("");
                    }
                    $("#legend").hide();
                    $("#spanSkip").empty();
                    $("#btnSkip").show();
                    $("#btnSkip2").show();
                }
                else if (jsonData.length > 0) {
                    jsonData = $.parseJSON(jsonData);
                    if (!(jsonData[0].Status == 'R1 Review Complete' || jsonData[0].Status == 'R2 Review Complete' || jsonData[0].Status == 'CA1 Review Complete')) {
                        AlertBox("warning", "Dicom Study", "Case Assignment is Pending!")
                        if ($.fn.DataTable.isDataTable('#tblSubjectReviewList')) {
                            var table = $('#tblSubjectReviewList').DataTable();
                            table.clear();
                            table.destroy();
                            $("#tblSubjectReviewList").find("thead").html("");
                        }
                        $("#legend").hide();
                        $("#spanSkip").empty();
                        $("#btnSkip").show();
                        $("#btnSkip2").show();
                    }
                    else if (jsonData.length > 0) {
                        //jsonData = $.parseJSON(jsonData);
                        $("#divTblSubjectReviewList").show();
                        $("#legend").show();
                        $("#spanSkip").empty();
                        $("#btnSkip").hide();
                        $("#btnSkip2").hide();
                        var activityDataSet = []
                        console.log(jsonData);
                        for (var i = 0; i < jsonData.length; i++) {
                            var inDataSet = [];
                            // inDataSet.push(jsonData[i].iImgTransmittalHdrId, jsonData[i].iImgTransmittalDtlId, jsonData[i].vWorkspaceId, jsonData[i].vProjectNo, jsonData[i].vMySubjectNo, jsonData[i].vSubjectId, jsonData[i].vRandomizationNo, jsonData[i].cDeviation, jsonData[i].nvDeviationReason, jsonData[i].nvInstructions, jsonData[i].iModalityNo, jsonData[i].vModalityDesc, jsonData[i].iAnatomyNo, jsonData[i].vAnatomyDesc, jsonData[i].cIVContrast, jsonData[i].dExaminationDate, jsonData[i].iModifyBy, jsonData[i].dModifyOn, jsonData[i].cStatusIndi, jsonData[i].changeOn, '', jsonData[i].iNodeId, jsonData[i].vNodeDisplayName, jsonData[i].iImageStatus, $("#select2-ddlActivity-container")[0].innerHTML, $("#select2-ddlSubActivity-container")[0].innerHTML, jsonData[i].ImgTransmittalDtl_iImageTranNo, jsonData[i].ImageTransmittalImgDtl_iImageTranNo);

                            inDataSet.push(jsonData[i].iImgTransmittalHdrId, jsonData[i].iImgTransmittalDtlId, jsonData[i].vWorkspaceId, '', jsonData[i].vProjectNo, jsonData[i].vMySubjectNo, jsonData[i].vSubjectId, jsonData[i].vRandomizationNo, jsonData[i].cDeviation, jsonData[i].nvDeviationReason, jsonData[i].nvInstructions, jsonData[i].iModalityNo, jsonData[i].vModalityDesc, jsonData[i].iAnatomyNo, jsonData[i].vAnatomyDesc, jsonData[i].cIVContrast, jsonData[i].cOralContrast, jsonData[i].dExaminationDate, jsonData[i].iModifyBy, jsonData[i].dModifyOn, jsonData[i].cStatusIndi, jsonData[i].changeOn, jsonData[i].iNodeId, jsonData[i].vNodeDisplayName, jsonData[i].iImageStatus, $("#select2-ddlActivity-container")[0].innerHTML, $("#select2-ddlSubActivity-container")[0].innerHTML, jsonData[i].ImgTransmittalDtl_iImageTranNo, jsonData[i].ImageTransmittalImgDtl_iImageTranNo);
                            activityDataSet.push(inDataSet);
                        }
                        console.log(activityDataSet);

                        if (isSubjectReject == "Y") {
                            AlertBox("warning", "Image Review!", "Subject is Rejected!");
                        }
                        // isSubjectReject = "N"
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
                            "sScrollX": "100%",
                            "sScrollXInner": "1400",
                            "fnCreatedRow": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {

                                $('td:eq(0)', nRow).append('<a  class="myIcon icon-success icon-ef-3 icon-ef-3a hover-color icon-center" data-tooltip="tooltip" title="View" attrid="' + aData[0] + '" Onclick=MIDicomStudy.subjectImageStudyDetail(this) iImgTransmittalHdrId="' + aData[0] + '" iImgTransmittalDtlId ="' + aData[1] + '" vWorkspaceId ="' + aData[2] + '" vProjectNo ="' + aData[4] + '" vSubjectId ="' + aData[6] + '" iModalityNo="' + aData[11] + '" iAnatomyNo="' + aData[13] + '" iNodeId="' + aData[22] + '" iImageStatus="' + aData[24] + '" Activity="' + aData[25] + '" SubActivity="' + aData[26] + '"  ImgTransmittalDtl_iImageTranNo="' + aData[27] + '" ImageTransmittalImgDtl_iImageTranNo="' + aData[28] + '"  style="cursor:pointer;"><i class="fa  fa-info-circle"></i></a>');

                            },
                            "aoColumns": [


                               { "sTitle": "iImgTransmittalHdrId" },//0
                              { "sTitle": "iImgTransmittalDtlId" },//1
                              { "sTitle": "vWorkspaceId" },//2

                              { "sTitle": "VIEW" },//3
                              { "sTitle": "Project No" },//4
                              { "sTitle": "Screening No" },//5
                              { "sTitle": "Subject ID" },//6
                              { "sTitle": "vRandomizationNo" },//7
                              { "sTitle": "Deviation" },//8
                              { "sTitle": "Deviation Reason" },//9
                              { "sTitle": "Instructions" },//10
                              { "sTitle": "iModalityNo" },//11
                              { "sTitle": "Modality" },//12
                              { "sTitle": "iAnatomyNo" },//13
                              { "sTitle": "Anatomy" },//14
                              { "sTitle": "IV Contrast" },//15
                              { "sTitle": "Oral Contrast" },//16 //change from here
                              { "sTitle": "Examination Date" },//17
                              { "sTitle": "iModifyBy" },//18
                              { "sTitle": "dModifyOn" },//19
                              { "sTitle": "cStatusIndi" },//20
                              { "sTitle": "Created By" },//21

                             // { "sTitle": "VIEW" },//20

                              { "sTitle": "iNodeId" },//22
                              { "sTitle": "Activity" },//23
                              { "sTitle": "Image Status" },//24
                              { "sTitle": "Activity" },//25
                              { "sTitle": "Sub Activity" },//26


                            ],
                            "columnDefs": [
                              {
                                  //"targets": [0, 1, 2, 5, 9, 11, 15, 16, 17, 18, 20, 22, 23],

                                  "targets": [0, 1, 2, 6, 7, 8, 11, 13, 18, 19, 20, 21, 22, 24, 25],
                                  "visible": false,
                                  "searchable": false
                              },

                                 { "width": "10%", "targets": 4 },
                               { "width": "10%", "targets": 5 },
                               { "width": "18%", "targets": 8 },
                               { "width": "18%", "targets": 9 },
                               { "width": "15%", "targets": 15 },
                               { "width": "100%", "targets": 19 },
                              { "bSortable": false, "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25] },
                            ],
                            "oLanguage": {
                                "sEmptyTable": "No Record Found",
                            }
                        });

                        $("#tblSubjectReviewList tbody tr:first").addClass('original');
                        $('table#tblSubjectReviewList > tbody > tr').not(':first').addClass('reviewed');

                        setTimeout(function () { $("#tblSubjectReviewList").dataTable().fnAdjustColumnSizing(); }, 10);
                    }
                }
                else {
                    if ($(tblSubjectReviewList) != 'undefined') {
                        if ($.fn.DataTable.isDataTable('#tblSubjectReviewList')) {
                            var table = $('#tblSubjectReviewList').DataTable();
                            table.clear();
                            table.destroy();
                            $("#tblSubjectReviewList").find("thead").html("");
                        }
                    }

                    if (($("#select2-ddlActivity-container")[0].innerHTML.toUpperCase().match("ADJUDICATOR") || $("#select2-ddlSubActivity-container")[0].innerHTML.toUpperCase().match("ADJUDICATOR")) || ($("#select2-ddlActivity-container")[0].innerHTML.toUpperCase().match("GLOBAL")) || $("#select2-ddlSubActivity-container")[0].innerHTML.toUpperCase().match("GLOBAL")) {
                        $("#divTblSubjectReviewList").hide();
                        $("#legend").hide();
                        if (isSubjectReject == "Y") {
                            AlertBox("warning", "Image Review!", "Subject is Rejected!");
                        }
                        $("#spanSkip").empty();
                        $("#btnSkip").show();
                        $("#btnSkip2").show();
                    }
                    else {
                        $("#legend").hide();
                        if (isSubjectReject == "Y") {
                            AlertBox("warning", "Image Review!", "Subject is Rejected and No Subject Study Detail Found.!");
                        }
                        //AlertBox("information", "Image Review!", "No Subject Study Detail Found.!")
                        $("#spanSkip").empty();
                        $("#btnSkip").show();
                        $("#btnSkip2").show();
                    }
                }
            }
            function errorSubjectStudyDetail() {
                AlertBox("error", "Image Review!", "Error While Retriving Subject Study Detail.!")
                $("#loader").css("display", "none")
            }

            MIDicomStudy.getSubjectStudyDetail = getSubjectStudyDetail;
            MIDicomStudy.successSubjectStudyDetail = successSubjectStudyDetail;
            MIDicomStudy.errorSubjectStudyDetail = errorSubjectStudyDetail;

        })($, MIDicomStudy, General);

        (function ($, MIDicomStudy, General) {
            "use strict";
            function validate() {
                if (ddlBlankCheck('ddlProject')) {
                    AlertBox("warning", "Dicom Study!", "Please Select Project!");
                    return false
                }
                if (ddlBlankCheck('ddlSubject')) {
                    AlertBox("warning", "Dicom Study!", "Please Select Screening No!");
                    return false
                }
                if (ddlBlankCheck('ddlActivity')) {
                    AlertBox("warning", "Dicom Study!", "Please Select Activity!");
                    return false
                }
                if (ddlBlankCheck('ddlSubActivity')) {
                    AlertBox("warning", "Dicom Study!", "Please Select Sub Activity!");
                    return false
                }
            }
            MIDicomStudy.validate = validate;
        })($, MIDicomStudy, General);


        MIDicomStudy.getUserWiseProjectDetail();

    })
