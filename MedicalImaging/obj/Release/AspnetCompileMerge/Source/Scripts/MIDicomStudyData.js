$body = $("body");

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
});

$(document).ajaxComplete(function () {
});

$(function () {

    onload = function () {
        onfocus = function () {
            if ($("#select2-ddlProject-container")[0].innerHTML != "") {
                if ($("#select2-ddlSubject-container")[0].innerHTML != "") {
                    if ($("#select2-ddlActivity-container")[0].innerHTML != "") {
                        if ($("#select2-ddlSubActivity-container")[0].innerHTML != "") {
                            $.ajax({
                                url: WebURL + "MIDicomStudy/updatedSessionValue",
                                type: "POST",
                                async: false,
                                success: function (updateJson) {
                                    if (updateJson == "TRUE") {
                                        MIDicomStudy.getSubjectStudyDetail()
                                    }
                                },
                                error: function (e) {
                                    AlertBox("ERROR", "Dicom Study!", "Error While Checking Update Session!");
                                }
                            });
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

    document.getElementById("ddlProject").tabIndex = 1;
    document.getElementById("ddlSubject").tabIndex = 2;
    document.getElementById("ddlActivity").tabIndex = 3;
    document.getElementById("ddlSubActivity").tabIndex = 4;
    document.getElementById("ddlProject").focus();


    $("#ddlProject").on('change', function () {
        if ($("#select2-ddlProject-container")[0].innerHTML != "") {
            $('#ddlSubActivity').find('option').remove().end().append('<option value="select">Select Activity</option>').val('select');
            $("#select2-ddlSubActivity-container")[0].innerHTML = ""
            MIDicomStudy.getSubjectDetail();
            MIDicomStudy.getActivityDetail();
        }
    });

    $("#ddlSubject").on('change', function () {
        $("#ddlActivity").select2('val', '');
        $("#ddlSubActivity").select2('val', '');
    });

    $("#ddlActivity").on('change', function () {
        if ($("#select2-ddlActivity-container")[0].innerHTML != "") {
            MIDicomStudy.getSubActivityDetail();
        }
    });

    $("#btnGo").on('click', function () {
        MIDicomStudy.getSubjectStudyDetail()
    });

    $("#btnClear").on('click', function () {

        $("#ddlProject").select2('val', '');
        $("#ddlSubject").select2('val', '');
        $("#ddlActivity").select2('val', '');
        $("#ddlSubActivity").select2('val', '');

        if ($.fn.DataTable.isDataTable('#tblSubjectReviewList')) {
            var table = $('#tblSubjectReviewList').DataTable();
            var rows = table
                .clear()
                .draw();
        }
        $("#legend").hide();
    });

    $("#btnSkip").on('click', function () {
        MIDicomStudy.skipVisit()
    });

    (function ($, MIDicomStudy, General) {
        'use strict';

        function getUserWiseProjectDetail() {
            $("#spanSkip").empty();
            $("#btnSkip").hide();
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
                url: ApiURL + "GetData/MyProjectCompletionList",
                success: MIDicomStudy.successUserWiseProjectDetail,
                error: MIDicomStudy.errorUserWiseProjectDetail
            }
            MIDicomStudy.fnUserWiseProjectDetail(userWiseProjectDetailAjaxData.async, userWiseProjectDetailAjaxData.data, userWiseProjectDetailAjaxData.type, userWiseProjectDetailAjaxData.url, userWiseProjectDetailAjaxData.success, userWiseProjectDetailAjaxData.success, userWiseProjectDetailAjaxData.error)
        }
        var fnUserWiseProjectDetail = function (async, data, type, url, success, error) {
            setTimeout(function () {
                $.ajax({
                    //async: async,
                    data: data,
                    type: type,
                    url: url,
                    complete: success,
                    error: error
                });
            }, 0);
            return this;
        }
        function successUserWiseProjectDetail(jsonData) {
            if (jsonData.responseJSON.length > 0) {
                $("#ddlProject").append($("<option></option>").html("Select Project").val(""));
                for (var V = 0 ; V < jsonData.responseJSON.length ; V++) {
                    $("#ddlProject").append($("<option></option>").html(jsonData.responseJSON[V].WorkspaceMerge).val(jsonData.responseJSON[V].vWorkspaceId));
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
        'use strict';
        function getSubjectDetail() {
            $("#spanSkip").empty();
            $("#btnSkip").hide();
            var vSubjectNo = "";
            var vWorkSpaceID = $("#ddlProject").val();
            var cSubjectFilter = "N";

            var subjectDetailData = {
                vSubjectNo: vSubjectNo,
                vWorkSpaceID: vWorkSpaceID,
                cSubjectFilter: cSubjectFilter
            }

            var subjectDetailAjaxData = {
                async: false,
                data: subjectDetailData,
                type: "POST",
                url: ApiURL + "CommonData/SubjectDetails",
                success: MIDicomStudy.successSubjectDetailAjaxData,
                error: MIDicomStudy.errorSubjectDetailAjaxData
            }

            MIDicomStudy.fnSubjectDetail(subjectDetailAjaxData.async, subjectDetailAjaxData.data, subjectDetailAjaxData.type, subjectDetailAjaxData.url, subjectDetailAjaxData.success, subjectDetailAjaxData.error)
        }
        var fnSubjectDetail = function (async, data, type, url, success, error) {
            setTimeout(function () {
                $.ajax({
                    //async: false,
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
            if (jsonData.length > 0) {
                $('#ddlSubject').find('option').remove().end().append('<option value="select">Select Subject</option>').val('select');
                $("#select2-ddlSubject-container")[0].innerHTML = ""
                for (var V = 0 ; V < jsonData.length ; V++) {
                    //$("#ddlSubject").append($("<option></option>").html(jsonData[V].DisplayName2).val(jsonData[V].vSubjectId + "#" + jsonData[V].vMySubjectNo + "#" + jsonData[V].iMySubjectNo + "#" + jsonData[V].iPeriod + "#" + jsonData[V].vParentWorkspaceId));
                    $("#ddlSubject").append($("<option></option>").html(jsonData[V].vMySubjectNo).val(jsonData[V].vSubjectId + "#" + jsonData[V].vMySubjectNo + "#" + jsonData[V].iMySubjectNo + "#" + jsonData[V].iPeriod + "#" + jsonData[V].vParentWorkspaceId));
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
            var vWorkSpaceID = $("#ddlProject").val();
            var iPeriod = 1;
            var iParentNodeId = 1;
            var vUserTypeCode = $("#hdnUserTypeCode").val();

            var activityDetailData = {
                vWorkSpaceID: vWorkSpaceID,
                iPeriod: iPeriod,
                iParentNodeId: iParentNodeId,
                vUserTypeCode: vUserTypeCode
            }

            var activityDetailAjaxData = {
                url: ApiURL + "GetData/ProjectActivityDetails",
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
            if (jsonData.length > 0) {
                $('#ddlActivity').find('option').remove().end().append('<option value="select">Select Activity</option>').val('select');
                $("#select2-ddlActivity-container")[0].innerHTML = ""
                for (var V = 0 ; V < jsonData.length; V++) {
                    $("#ddlActivity").append($("<option></option>").html(jsonData[V].vNodeDisplayName).val(jsonData[V].vActivityId + "#" + jsonData[V].iNodeId));
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
            var vWorkSpaceID = $("#ddlProject").val();
            var iPeriod = 1;
            var val = $("#ddlActivity").val().split('#')
            var iParentNodeId = val[1];
            var vUserTypeCode = $("#hdnUserTypeCode").val();

            var subActivityDetailData = {
                vWorkSpaceID: vWorkSpaceID,
                iPeriod: iPeriod,
                iParentNodeId: iParentNodeId,
                vUserTypeCode: vUserTypeCode
            }

            var subActivityDetailAjaxData = {
                url: ApiURL + "GetData/ProjectActivityDetails",
                type: "POST",
                data: subActivityDetailData,
                async: false,
                success: successSubActivityDetail,
                error: errorSubActivityDetail
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
            if (jsonData.length > 0) {
                $('#ddlSubActivity').find('option').remove().end().append('<option value="select">Select Activity</option>').val('select');
                $("#select2-ddlSubActivity-container")[0].innerHTML = ""
                for (var V = 0 ; V < jsonData.length; V++) {
                    $("#ddlSubActivity").append($("<option></option>").html(jsonData[V].vNodeDisplayName).val(jsonData[V].vActivityId + "#" + jsonData[V].iNodeId));
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

        MIDicomStudy.getSubActivityDetail = getSubActivityDetail;
        MIDicomStudy.fnSubActivityDetail = fnSubActivityDetail;
        MIDicomStudy.successSubActivityDetail = successSubActivityDetail;
        MIDicomStudy.errorSubActivityDetail = errorSubActivityDetail;

    })($, MIDicomStudy, General);

    (function ($, MIDicomStudy, General) {
        "use strict"

        function skipVisit() {
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

            var url = $("#DicomViewer").val();
            //location.href = url;

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
            strFormElement += '</form>';

            var form = $(strFormElement);
            $(document.body).append(form[0]); //Resolve Chrome Version Change Issue 
            form.submit();
            $(".custom-form").remove();
            return false;
        }

        MIDicomStudy.skipVisit = skipVisit;

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
            strFormElement += '<input type="hidden" name="iImageCount" value="' + iImageCount + '">';
            strFormElement += '<input type="hidden" name="ImgTransmittalDtl_iImageTranNo" value="' + ImgTransmittalDtl_iImageTranNo + '">';
            strFormElement += '<input type="hidden" name="ImageTransmittalImgDtl_iImageTranNo" value="' + ImageTransmittalImgDtl_iImageTranNo + '">';

            strFormElement += '</form>';

            var form = $(strFormElement);
            $(document.body).append(form[0]); //Resolve Chrome Version Change Issue 
            form.submit();
            $(".custom-form").remove();
            return false;

        }
        MIDicomStudy.subjectImageStudyDetail = subjectImageStudyDetail;

    })($, MIDicomStudy, General);

    (function ($, MIDicomStudy, General) {
        "use strict";

        function getSubjectStudyDetail() {
            $("#spanSkip").empty();
            $("#btnSkip").hide();
            if (MIDicomStudy.validate() == false) {
                if ($.fn.DataTable.isDataTable('#tblSubjectReviewList')) {
                    var table = $('#tblSubjectReviewList').DataTable();
                    var rows = table
                        .clear()
                        .draw();
                }
                $("#legend").hide();
                $("#spanSkip").empty();
                $("#btnSkip").hide();
                return false;
            }
            else {
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

                if ((!vActivityName.toUpperCase().match("MARK")) && (!vActivityName.toUpperCase().match("ADJUDICATOR")) && (!vActivityName.toUpperCase().match("GLOBAL"))) {


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
                        url: ApiURL + "GetData/MI_DataSaveStatus",
                        type: "POST",
                        data: MI_DataSaveStatus,
                        async: false,
                        success: function (jsonDataSaveStatus) {
                            var data = jsonDataSaveStatus;
                            if (data == "NOTALLOW") {
                                AlertBox("warning", "Image Review!", "Dicom Study For " + vActivityName + " Is Not Assigned Yet.!");
                                if ($.fn.DataTable.isDataTable('#tblSubjectReviewList')) {
                                    var table = $('#tblSubjectReviewList').DataTable();
                                    var rows = table
                                        .clear()
                                        .draw();
                                }
                                DataSaveStatus = false;
                                removeDiv();
                                $(".spinner").hide();
                                //return false;
                            }
                            else if (data == "ERROR") {
                                AlertBox("error", "Image Review!", "Error While Retriving Image Assignment Status.!");
                                if ($.fn.DataTable.isDataTable('#tblSubjectReviewList')) {
                                    var table = $('#tblSubjectReviewList').DataTable();
                                    var rows = table
                                        .clear()
                                        .draw();
                                }
                                DataSaveStatus = false;
                                removeDiv();
                                $(".spinner").hide();
                                //return false;
                            }
                            else if (data == "ALLOW") {
                                DataSaveStatus = true;
                            }
                            else {
                                AlertBox("error", "Image Review!", "Error While Retriving Image Assignment Status.!");
                                if ($.fn.DataTable.isDataTable('#tblSubjectReviewList')) {
                                    var table = $('#tblSubjectReviewList').DataTable();
                                    var rows = table
                                        .clear()
                                        .draw();
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
                            if ($.fn.DataTable.isDataTable('#tblSubjectReviewList')) {
                                var table = $('#tblSubjectReviewList').DataTable();
                                var rows = table
                                    .clear()
                                    .draw();
                            }
                            DataSaveStatus = false;
                            removeDiv();
                            $(".spinner").hide();
                        }
                    });
                }
                else {
                    DataSaveStatus = true;
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
                        url: ApiURL + "GetData/CRFDataEntryStatus",
                        type: "POST",
                        data: CRFDataEntryStatus,
                        //async: false,
                        success: function (jsonData) {
                            var data = jsonData;
                            if (data == "error") {

                                if ($.fn.DataTable.isDataTable('#tblSubjectReviewList')) {
                                    var table = $('#tblSubjectReviewList').DataTable();
                                    var rows = table
                                        .clear()
                                        .draw();
                                }
                                $("#legend").hide();
                                AlertBox("error", "Image Review!", "Error While Retriving CRF Data Entry Control Details.!");
                                return false;
                            }
                            else if (data == "NO-DATA") {
                                if ($.fn.DataTable.isDataTable('#tblSubjectReviewList')) {
                                    var table = $('#tblSubjectReviewList').DataTable();
                                    var rows = table
                                        .clear()
                                        .draw();
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
                                    url: ApiURL + "GetData/SubjectStudyDetail",
                                    type: "POST",
                                    data: subjectStudyDetailData,
                                    //async: false,
                                    success: MIDicomStudy.successSubjectStudyDetail,
                                    error: MIDicomStudy.errorSubjectStudyDetail
                                });
                            }
                            else {
                                if ($.fn.DataTable.isDataTable('#tblSubjectReviewList')) {
                                    var table = $('#tblSubjectReviewList').DataTable();
                                    var rows = table
                                        .clear()
                                        .draw();
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
        function successSubjectStudyDetail(jsonData) {
            if (jsonData.length > 0) {
                $("#divTblSubjectReviewList").show();
                $("#legend").show();
                $("#spanSkip").empty();
                $("#btnSkip").hide();
                var activityDataSet = []
                for (var i = 0; i < jsonData.length; i++) {
                    var inDataSet = [];
                    inDataSet.push(jsonData[i].iImgTransmittalHdrId, jsonData[i].iImgTransmittalDtlId, jsonData[i].vWorkspaceId, jsonData[i].vProjectNo, jsonData[i].vSubjectId, jsonData[i].vRandomizationNo, jsonData[i].cDeviation, jsonData[i].nvDeviationReason, jsonData[i].nvInstructions, jsonData[i].iModalityNo, jsonData[i].vModalityDesc, jsonData[i].iAnatomyNo, jsonData[i].vAnatomyDesc, jsonData[i].cIVContrast, jsonData[i].dExaminationDate, jsonData[i].iModifyBy, jsonData[i].dModifyOn, jsonData[i].cStatusIndi, jsonData[i].changeOn, '', jsonData[i].iNodeId, jsonData[i].vNodeDisplayName, jsonData[i].iImageStatus, $("#select2-ddlActivity-container")[0].innerHTML, $("#select2-ddlSubActivity-container")[0].innerHTML, jsonData[i].ImgTransmittalDtl_iImageTranNo, jsonData[i].ImageTransmittalImgDtl_iImageTranNo);
                    activityDataSet.push(inDataSet);
                }

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
                        $('td:eq(9)', nRow).append('<a  class="myIcon icon-success icon-ef-3 icon-ef-3a hover-color icon-center" data-tooltip="tooltip" title="View" attrid="' + aData[0] + '" Onclick=MIDicomStudy.subjectImageStudyDetail(this) iImgTransmittalHdrId="' + aData[0] + '" iImgTransmittalDtlId ="' + aData[1] + '" vWorkspaceId ="' + aData[2] + '" vProjectNo ="' + aData[3] + '" vSubjectId ="' + aData[4] + '" iModalityNo="' + aData[9] + '" iAnatomyNo="' + aData[11] + '" iNodeId="' + aData[20] + '" iImageStatus="' + aData[22] + '" Activity="' + aData[23] + '" SubActivity="' + aData[24] + '"  ImgTransmittalDtl_iImageTranNo="' + aData[25] + '" ImageTransmittalImgDtl_iImageTranNo="' + aData[26] + '"  style="cursor:pointer;"><i class="fa  fa-info-circle"></i></a>');
                        if (aData[22] == '1') {
                            $(nRow).addClass('original');
                        }
                        else {
                            $(nRow).addClass('reviewed');
                        }
                    },
                    "aoColumns": [
                      { "sTitle": "iImgTransmittalHdrId" },
                      { "sTitle": "iImgTransmittalDtlId" },
                      { "sTitle": "vWorkspaceId" },
                      { "sTitle": "Project No" },
                      { "sTitle": "Subject ID" },
                      { "sTitle": "vRandomizationNo" },
                      { "sTitle": "Deviation" },
                      { "sTitle": "Deviation Reason" },
                      { "sTitle": "Instructions" },
                      { "sTitle": "iModalityNo" },
                      { "sTitle": "Modality" },
                      { "sTitle": "iAnatomyNo" },
                      { "sTitle": "Anatomy" },
                      { "sTitle": "IV Contrast" },
                      { "sTitle": "Examination Date" },
                      { "sTitle": "iModifyBy" },
                      { "sTitle": "dModifyOn" },
                      { "sTitle": "cStatusIndi" },
                      { "sTitle": "Created By" },
                      { "sTitle": "VIEW" },
                      { "sTitle": "iNodeId" },
                      { "sTitle": "Activity" },
                      { "sTitle": "Image Status" },
                      { "sTitle": "Activity" },
                      { "sTitle": "Sub Activity" },
                    ],
                    "columnDefs": [
                      {
                          "targets": [0, 1, 2, 5, 9, 11, 15, 16, 17, 18, 20, 22, 23],
                          "visible": false,
                          "searchable": false
                      },
                       { "width": "10%", "targets": 3 },
                       { "width": "10%", "targets": 4 },
                       { "width": "18%", "targets": 7 },
                       { "width": "18%", "targets": 8 },
                       { "width": "15%", "targets": 14 },
                       { "width": "100%", "targets": 18 },
                      { "bSortable": false, "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24] },
                    ],
                    "oLanguage": {
                        "sEmptyTable": "No Record Found",
                    }
                });
                setTimeout(function () { $("#tblSubjectReviewList").dataTable().fnAdjustColumnSizing(); }, 10);
            }
            else {
                if ($.fn.DataTable.isDataTable('#tblSubjectReviewList')) {
                    var table = $('#tblSubjectReviewList').DataTable();
                    var rows = table
                        .clear()
                        .draw();
                }

                if (($("#select2-ddlActivity-container")[0].innerHTML.toUpperCase().match("ADJUDICATOR") || $("#select2-ddlSubActivity-container")[0].innerHTML.toUpperCase().match("ADJUDICATOR")) || ($("#select2-ddlActivity-container")[0].innerHTML.toUpperCase().match("GLOBAL")) || $("#select2-ddlSubActivity-container")[0].innerHTML.toUpperCase().match("GLOBAL")) {
                    $("#divTblSubjectReviewList").hide();
                    $("#legend").hide();
                    $("#spanSkip").empty();
                    $("#btnSkip").show();
                }
                else {
                    $("#legend").hide();
                    AlertBox("information", "Image Review!", "No Subject Study Detail Found.!")
                    $("#spanSkip").empty();
                    $("#btnSkip").show();
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
                AlertBox("warning", "Dicom Study!", "Please Select Subject!");
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



