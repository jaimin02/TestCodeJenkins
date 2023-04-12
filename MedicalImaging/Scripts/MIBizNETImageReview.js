var vWorkspaceId, vProjectNo, vSubjectId, iNodeId, iModalityNo, iAnatomyNo, iuserid, iImgTransmittalHdrId, iImgTransmittalDtlId, ActivityID, NodeID, ActivityDef, iMySubjectNo, ScreenNo, ParentWorkSpaceId, PeriodId, ActivityName, SubActivityName, subinodeID, parentActivityID;

if (typeof MIBizNETImageReview == "undefined") {
    MIBizNETImageReview = {};
}

if (typeof General == "undefined") {
    General = {};
}

$(document).bind("ajaxSend", function () {
    createDiv();
    $(".spinner").show();
}).bind("ajaxComplete", function () {
    removeDiv();
    $(".spinner").hide();
}).bind("ajaxError", function () {
    removeDiv();
    $(".spinner").hide();
});

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
            }
        }
    }

    var userLoginDetails = {
        iUserId: $("#hdnuserid").val(),
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
        if (jsonData.length == 0) {
            logOut();
            var url = $("#RedirectToLogin").val();
            location.href = url;
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

$(function () {
    debugger;
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
            else if (key == 'ActivityID') {
                ActivityID = val;
                $("#hdnActivityID").val(val);
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
                SubActivityName = val;
                $("#hdnSubActivityName").val(val);
            }
            else if (key == 'subinodeID') {
                subinodeID = val;
                $("#hdnsubinodeID").val(val);
            }
            else if (key == 'parentActivityID') {
                parentActivityID = val;
                $("#hdnparentActivityID").val(val);
            }
        }
    }

    $('.btn').hover(function () {
        $(this).stop().animate({ 'padding-left': '15px', 'padding-right': '45px' }, 'fast');
        $(this).find('.num').stop().animate({ 'left': '-50px' }, 'fast');
        $(this).find('.fa').stop().animate({ 'right': '0px' }, 'fast');
    }, function () {
        $(this).stop().animate({ 'padding-left': '45px', 'padding-right': '15px' }, 'fast');
        $(this).find('.num').stop().animate({ 'left': '0px' }, 'fast');
        $(this).find('.fa').stop().animate({ 'right': '-50px' }, 'fast');
    });

    subjectStudyDetail();
});

function subjectStudyDetail() {

    var subjectStudyDetailData = {
        vWorkspaceId: vWorkspaceId,
        vProjectNo: vProjectNo,
        vSubjectId: vSubjectId,
        iNodeId: iNodeId
    }

    var subjectStudyDetailAjaxData = {
        url: ApiURL + "GetData/BizNetSubjectStudyDetail",
        type: "POST",
        data: subjectStudyDetailData,
        async: false,
        success: successSubjectStudyDetail,
        error: errorSubjectStudyDetail
    }

    fnSubjectStudyDetail(subjectStudyDetailAjaxData.url, subjectStudyDetailAjaxData.type, subjectStudyDetailAjaxData.data, subjectStudyDetailAjaxData.async, subjectStudyDetailAjaxData.success, subjectStudyDetailAjaxData.error)
}

var fnSubjectStudyDetail = function (url, type, data, async, success, error) {
    $.ajax({
        url: url,
        type: type,
        data: data,
        //async: false,
        success: success,
        error: error
    });
};

function successSubjectStudyDetail(jsonData) {
    if (jsonData.length > 0) {
        var activityDataSet = []
        //Changes reversed Bhargav Thaker 03Mar2023
        for (var i = 0; i < jsonData.length; i++) {
            var inDataSet = [];
            inDataSet.push(jsonData[i].iImgTransmittalHdrId, jsonData[i].iImgTransmittalDtlId, jsonData[i].vWorkspaceId, jsonData[i].vProjectNo, jsonData[i].vMySubjectNo, jsonData[i].vSubjectId, jsonData[i].vRandomizationNo, jsonData[i].cDeviation, jsonData[i].nvDeviationReason, jsonData[i].nvInstructions, jsonData[i].iModalityNo, jsonData[i].vModalityDesc, jsonData[i].iAnatomyNo, jsonData[i].vAnatomyDesc, jsonData[i].cIVContrast, jsonData[i].cOralContrast, jsonData[i].dExaminationDate, jsonData[i].iModifyBy, jsonData[i].dModifyOn, jsonData[i].cStatusIndi, jsonData[i].changeOn, '', jsonData[i].iNodeId, jsonData[i].vNodeDisplayName, jsonData[i].iImageStatus);
            activityDataSet.push(inDataSet);
        }

        otable = $("#tblSubjectReviewList").dataTable({
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
                $('td:eq(11)', nRow).append('<a href="" class="myIcon icon-success icon-ef-3 icon-ef-3a hover-color icon-center" data-tooltip="tooltip" title="View" attrid="' + aData[0] + '" Onclick=subjectImageStudyDetail(this) iImgTransmittalHdrId="' + aData[0] + '" iImgTransmittalDtlId ="' + aData[1] + '" vWorkspaceId ="' + aData[2] + '" vProjectNo ="' + aData[3] + '" vSubjectId ="' + aData[5] + '" iModalityNo="' + aData[10] + '" iAnatomyNo="' + aData[12] + '" iNodeId="' + aData[22] + '" iImageStatus="' + aData[24] + '" vActivityName="' + aData[23] + '" style="cursor:pointer;"><i class="fa  fa-info-circle"></i></a>');
            },
            //vActivityId
            "aoColumns": [
              { "sTitle": "iImgTransmittalHdrId" },//0
              { "sTitle": "iImgTransmittalDtlId" },//1
              { "sTitle": "vWorkspaceId" },//2
              { "sTitle": "Project No" },//3
              { "sTitle": "Screening No" },//4
              { "sTitle": "Subject ID" },//5
              { "sTitle": "vRandomizationNo" },//6
              { "sTitle": "Deviation" },//7
              { "sTitle": "Deviation Reason" },//8
              { "sTitle": "Instructions" },//9
              { "sTitle": "iModalityNo" },//10
              { "sTitle": "Modality" },//11
              { "sTitle": "iAnatomyNo" },//12
              { "sTitle": "Anatomy" },//13
              { "sTitle": "IV Contrast" },//14
              { "sTitle": "Oral Contrast" },//15 //change from here
              { "sTitle": "Examination Date" },//16
              { "sTitle": "iModifyBy" },//17
              { "sTitle": "dModifyOn" },//18
              { "sTitle": "cStatusIndi" },//19
              { "sTitle": "Created By" },//20
              { "sTitle": "VIEW" },//21
              { "sTitle": "iNodeId" },//22
              { "sTitle": "Visit" },//23
              { "sTitle": "Image Status" },//24
            ],
            "columnDefs": [
              {
                  //"targets": [0, 1, 2, 5, 9, 11, 15, 16, 17, 20, 22],
                    "targets": [0, 1, 2, 5, 6, 10, 12, 17, 18, 19, 22, 24],
                  "visible": false,
                  "searchable": false
              },
               { "width": "10%", "targets": 3 },
               { "width": "10%", "targets": 4 },
               //{ "width": "5%", "targets": 6 },
                { "width": "18%", "targets": 7 },
               { "width": "18%", "targets": 8 },
               //{ "width": "20%", "targets": 10 },
               //{ "width": "20%", "targets": 12 },
               //{ "width": "5%", "targets": 13 },
               { "width": "15%", "targets": 14 },
               { "width": "100%", "targets": 18 },
              { "bSortable": false, "targets": [0, 1, 2, 5, 9, 11, 15, 16, 17, 19, 20, 22] },
            ],
            "oLanguage": {
                "sEmptyTable": "No Record Found",
            }
        });
        $("#tblSubjectReviewList tbody tr:first").addClass('original');
        $('table#tblSubjectReviewList > tbody > tr').not(':first').addClass('reviewed');
    }
    else {
        AlertBox("information", "Image Review!", "No Subject Study Detail Found.!")
    }
}

function errorSubjectStudyDetail() {
    AlertBox("error", "Image Review!", "Error While Retriving Subject Study Detail.!")
}

function subjectImageStudyDetail(e) {
    var iImgTransmittalHdrId = $(e).attr("iImgTransmittalHdrId");
    var iImgTransmittalDtlId = $(e).attr("iImgTransmittalDtlId");
    var iImageStatus = $(e).attr("iImageStatus");
    var vWorkspaceId = $(e).attr("vWorkspaceId");
    var vProjectNo = $(e).attr("vProjectNo");
    var vSubjectId = $(e).attr("vSubjectId");
    var iModalityNo = $(e).attr("iModalityNo");
    var iAnatomyNo = $(e).attr("iAnatomyNo");
    var iNodeId = $(e).attr("iNodeId");
    var iUserId = $("#hdniUserId").val();
    var ActivityID = $("#hdnActivityID").val();
    var NodeID = $("#hdnNodeID").val();
    var ActivityDef = $("#hdnActivityDef").val();
    var iMySubjectNo = $("#hdniMySubjectNo").val();
    var ScreenNo = $("#hdnScreenNo").val();
    var ParentWorkSpaceId = $("#hdnParentWorkSpaceId").val();
    var PeriodId = $("#hdnPeriodId").val();

    var subjectImageStudyDetailData;
    var subjectImageStudyDetailAjaxData;

    subjectImageStudyDetailData = {
        iImgTransmittalHdrId: iImgTransmittalHdrId,
        iImgTransmittalDtlId: iImgTransmittalDtlId,
        iImageStatus: iImageStatus,
        vWorkspaceId: vWorkspaceId,
        vProjectNo: vProjectNo,
        vSubjectId: vSubjectId,
        iNodeId: iNodeId,
        iModalityNo: iModalityNo,
        iAnatomyNo: iAnatomyNo
    }
    //data: JSON.stringify(subjectImageStudyDetailData),
    $.ajax({
        url: WebURL + "MIBizNETImageReview/DicomDetails",
        type: "POST",
        data: subjectImageStudyDetailData,
        async: false,
        success: OnSuccess1,
        error: OnErrorCall
    });
    function OnSuccess1(jsonData) {
        var url = $("#DicomViewer").val();
        if (ActivityID == "" || ActivityID == '' || ActivityID == null) {
            //window.open('http://localhost/MI/MIDicomViewer/MIDicomViewer?WId=' + vWorkspaceId + '&SId=' + vSubjectId + '&PId=' + vProjectNo + '&Uid=' + iuserid + '&MId=' + iModalityNo + '&AId=' + iAnatomyNo + '&VId=' + iNodeId + '&HdrId=' + iImgTransmittalHdrId + '&DtlId=' + iImgTransmittalDtlId + '&iIS=' + iImageStatus, '_blank');
        }
        else if (iNodeId == "" || iNodeId == null || iNodeId == "undefined") {
            //window.open('http://localhost/MI/MIDicomViewer/MIDicomViewer?WId=' + vWorkspaceId + '&SId=' + vSubjectId + '&PId=' + vProjectNo + '&Uid=' + iuserid + '&MId=' + iModalityNo + '&AId=' + iAnatomyNo + '&VId=' + iNodeId + '&HdrId=' + iImgTransmittalHdrId + '&DtlId=' + iImgTransmittalDtlId + '&iIS=' + iImageStatus, '_blank');
        }
        else {
            //window.open('http://localhost/MI/MIDicomViewer/MIDicomViewer?WId=' + vWorkspaceId + '&SId=' + vSubjectId + '&PId=' + vProjectNo + '&Uid=' + iuserid + '&MId=' + iModalityNo + '&AId=' + iAnatomyNo + '&VId=' + iNodeId + '&HdrId=' + iImgTransmittalHdrId + '&DtlId=' + iImgTransmittalDtlId + '&iIS=' + iImageStatus + '&ActivityID=' + ActivityID + '&NodeID=' + NodeID + '&ActivityDef=' + ActivityDef + '&iMySubjectNo=' + iMySubjectNo + '&ScreenNo=' + ScreenNo + '&ParentWorkSpaceId=' + ParentWorkSpaceId + '&PeriodId=' + PeriodId, '_blank');
            //window.open('http://90.0.0.68/MI/MIDicomViewer/MIDicomViewer?WId=' + vWorkspaceId + '&SId=' + vSubjectId + '&PId=' + vProjectNo + '&Uid=' + iuserid + '&MId=' + iModalityNo + '&AId=' + iAnatomyNo + '&VId=' + iNodeId + '&HdrId=' + iImgTransmittalHdrId + '&DtlId=' + iImgTransmittalDtlId + '&iIS=' + iImageStatus + '&ActivityID=' + ActivityID + '&NodeID=' + NodeID + '&ActivityDef=' + ActivityDef + '&iMySubjectNo=' + iMySubjectNo + '&ScreenNo=' + ScreenNo + '&ParentWorkSpaceId=' + ParentWorkSpaceId + '&PeriodId=' + PeriodId, '_blank');
            //window.open('http://10.1.10.112/DI_SoftValid/MIDicomViewer/MIDicomViewer?WId=' + vWorkspaceId + '&SId=' + vSubjectId + '&PId=' + vProjectNo + '&Uid=' + iuserid + '&MId=' + iModalityNo + '&AId=' + iAnatomyNo + '&VId=' + iNodeId + '&HdrId=' + iImgTransmittalHdrId + '&DtlId=' + iImgTransmittalDtlId + '&iIS=' + iImageStatus + '&ActivityID=' + ActivityID + '&NodeID=' + NodeID + '&ActivityDef=' + ActivityDef + '&iMySubjectNo=' + iMySubjectNo + '&ScreenNo=' + ScreenNo + '&ParentWorkSpaceId=' + ParentWorkSpaceId + '&PeriodId=' + PeriodId, '_blank');
            //window.open('http://10.1.10.112/DISoft_valid/MIDicomViewer/MIDicomViewer?WId=' + vWorkspaceId + '&SId=' + vSubjectId + '&PId=' + vProjectNo + '&Uid=' + iuserid + '&MId=' + iModalityNo + '&AId=' + iAnatomyNo + '&VId=' + iNodeId + '&HdrId=' + iImgTransmittalHdrId + '&DtlId=' + iImgTransmittalDtlId + '&iIS=' + iImageStatus + '&ActivityID=' + ActivityID + '&NodeID=' + NodeID + '&ActivityDef=' + ActivityDef + '&iMySubjectNo=' + iMySubjectNo + '&ScreenNo=' + ScreenNo + '&ParentWorkSpaceId=' + ParentWorkSpaceId + '&PeriodId=' + PeriodId, '_blank');
            //window.open('http://104.211.181.105/DISoft/MIDicomViewer/MIDicomViewer?WId=' + vWorkspaceId + '&SId=' + vSubjectId + '&PId=' + vProjectNo + '&Uid=' + iuserid + '&MId=' + iModalityNo + '&AId=' + iAnatomyNo + '&VId=' + iNodeId + '&HdrId=' + iImgTransmittalHdrId + '&DtlId=' + iImgTransmittalDtlId + '&iIS=' + iImageStatus + '&ActivityID=' + ActivityID + '&NodeID=' + NodeID + '&ActivityDef=' + ActivityDef + '&iMySubjectNo=' + iMySubjectNo + '&ScreenNo=' + ScreenNo + '&ParentWorkSpaceId=' + ParentWorkSpaceId + '&PeriodId=' + PeriodId, '_blank');
            //window.open('http://localhost:51577/MIDicomViewer/MIDicomViewer?WId=' + vWorkspaceId + '&SId=' + vSubjectId + '&PId=' + vProjectNo + '&Uid=' + iuserid + '&MId=' + iModalityNo + '&AId=' + iAnatomyNo + '&VId=' + iNodeId + '&HdrId=' + iImgTransmittalHdrId + '&DtlId=' + iImgTransmittalDtlId + '&iIS=' + iImageStatus + '&ActivityID=' + ActivityID + '&NodeID=' + NodeID + '&ActivityDef=' + ActivityDef + '&iMySubjectNo=' + iMySubjectNo + '&ScreenNo=' + ScreenNo + '&ParentWorkSpaceId=' + ParentWorkSpaceId + '&PeriodId=' + PeriodId, '_blank');
            
            if ($("#hdnUserNameWithProfile").val().search("Adjudicator") != -1)
            {
                (function ($, MIBizNETImageReview, General) {
                    "use strict";
                    function subjectImageStudyDetail(e) {
                        var vWorkSpaceId = $(e).attr("vWorkspaceId");
                        var subjectVal = $("#ddlSubject").val().split('#');
                        var vSubjectId = $(e).attr("vSubjectId");
                        var vMySubjectNo = $("#hdnScreenNo").val();
                        var iMySubjectNo = $("#hdniMySubjectNo").val();;
                        var iPeriod = $("#hdnPeriodId").val();
                        var vParentWorkspaceId = $("#hdnParentWorkSpaceId").val();
                        var ddlActivityVal = $("#ddlActivity").val();
                        var val = ddlActivityVal.split('#');
                        var vActivityId = $("#hdnparentActivityID").val();/////////1
                        var iNodeId = $("#hdnNodeID").val();
                        var ddlSubActivityVal = $("#ddlSubActivity").val().split('#');
                        var vSubActivityId = $("#hdnActivityName").val();///////
                        var iSubNodeId = $("#hdnsubinodeID").val();/////
                        var vActivityName = $("#hdnActivityName").val();//////2
                        var vSubActivityName = $("#hdnSubActivityName").val();//////
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
                        //if (!($("#select2-ddlActivity-container")[0].innerHTML == $(e).attr("Activity"))) {
                        //    AlertBox("warning", "Dicom Study!", "Please Select Proper Matching Activity!");
                        //    return false;
                        //}
                        //else if (!($("#select2-ddlSubActivity-container")[0].innerHTML == $(e).attr("SubActivity"))) {
                        //    AlertBox("warning", "Dicom Study!", "Please Select Proper Matching Sub Activity!");
                        //    return false;
                        //}
                        
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
                        strFormElement += '</form>';

                        var form = $(strFormElement);
                        $(document.body).append(form[0]); //Resolve Chrome Version Change Issue 
                        form.submit();

                        $(".custom-form").remove();

                        localStorage.setItem("IsReviewDone", "false");
                        localStorage.setItem("IsReviewContinue", "false");
                        var MyInterval = setInterval(function (ele) {
                            if (localStorage.getItem("IsReviewDone") == "true") {
                                // debugger;
                                clearInterval(MyInterval);
                                //Glo_Screenno = $("#ddlSubject").val();
                                //Glo_Activity = $("#ddlActivity").val();
                                //Glo_SubActivity = $("#ddlSubActivity").val();
                                //if ($("#select2-ddlProject-container")[0].innerHTML != "") {
                                //    MIBizNETImageReview.getSubjectDetail();
                                //}
                            }
                        }, 1000);

                        var MyInterval2 = setInterval(function (ele) {
                            if (localStorage.getItem("IsReviewContinue") == "true") {
                                // debugger;
                                clearInterval(MyInterval2);
                                //Glo_Screenno = $("#ddlSubject").val();
                                //Glo_Activity = $("#ddlActivity").val();
                                //Glo_SubActivity = $("#ddlSubActivity").val();
                                //if ($("#select2-ddlProject-container")[0].innerHTML != "") {
                                //    MIBizNETImageReview.getSubjectDetail();
                                //}
                            }
                        }, 1000);
                        return false;
                    }
                    MIBizNETImageReview.subjectImageStudyDetail = subjectImageStudyDetail;

                })($, MIBizNETImageReview, General);
            }
            else {
                window.open(WebURL + 'MIDicomViewer/MIDicomViewer?WId=' + vWorkspaceId + '&SId=' + vSubjectId + '&PId=' + vProjectNo + '&Uid=' + iuserid + '&MId=' + iModalityNo + '&AId=' + iAnatomyNo + '&VId=' + iNodeId + '&HdrId=' + iImgTransmittalHdrId + '&DtlId=' + iImgTransmittalDtlId + '&iIS=' + iImageStatus + '&ActivityID=' + ActivityID + '&NodeID=' + NodeID + '&ActivityDef=' + ActivityDef + '&iMySubjectNo=' + iMySubjectNo + '&ScreenNo=' + ScreenNo + '&ParentWorkSpaceId=' + ParentWorkSpaceId + '&PeriodId=' + PeriodId, '_blank');
            }
        }
    }
    function OnErrorCall(ex) {
        alert(ex);
    }
}