$(function () {  
    subjectStudyDetail();
});

function subjectStudyDetail() {
    var subjectStudyDetailData;
    var subjectStudyDetailAjaxData;

    var vWorkspaceId = $("#dicomStudyvWorkSpaceId").val()    
    var vSubjectId = $("#dicomStudyvSubjectId").val()
    var iNodeId = $("#dicomStudyiNodeId").val()   
    
    subjectStudyDetailData = {
        vWorkspaceId: vWorkspaceId,        
        vSubjectId: vSubjectId,
        iNodeId: iNodeId
    }
    $.ajax({
        url: ApiURL + "GetData/SubjectStudyDetail",
        type: "POST",
        data: subjectStudyDetailData,
        async: false,
        success: successSubjectStudyDetail,
        error: errorSubjectStudyDetail
    });

    //subjectStudyDetailAjaxData = {
    //    url: ApiURL + "GetData/SubjectStudyDetail",
    //    type: "POST",
    //    data: subjectStudyDetailData,
    //    async: false,
    //    success: successSubjectStudyDetail,
    //    error: errorSubjectStudyDetail
    //}
    //fnSubjectStudyDetail(subjectStudyDetailAjaxData.url, subjectStudyDetailAjaxData.type, subjectStudyDetailAjaxData.data, subjectStudyDetailAjaxData.async, subjectStudyDetailAjaxData.success, subjectStudyDetailAjaxData.error)
}

var fnSubjectStudyDetail = function (url, type, data, async, success, error) {
    $.ajax({
        url: url,
        type: type,
        async: false,
        success: success,
        error: error
    });
};

function successSubjectStudyDetail(jsonData) {
    if (jsonData.length > 0) {
        var activityDataSet = []
        for (var i = 0; i < jsonData.length; i++) {
            var inDataSet = [];
            inDataSet.push(jsonData[i].iImgTransmittalHdrId, jsonData[i].iImgTransmittalDtlId, jsonData[i].vWorkspaceId, jsonData[i].vProjectNo, jsonData[i].vSubjectId, jsonData[i].vRandomizationNo, jsonData[i].cDeviation, jsonData[i].nvDeviationReason, jsonData[i].nvInstructions, jsonData[i].iModalityNo, jsonData[i].vModalityDesc, jsonData[i].iAnatomyNo, jsonData[i].vAnatomyDesc, jsonData[i].cIVContrast, jsonData[i].dExaminationDate, jsonData[i].iModifyBy, jsonData[i].dModifyOn, jsonData[i].cStatusIndi, jsonData[i].changeOn, '', jsonData[i].iNodeId, jsonData[i].vNodeDisplayName, jsonData[i].iImageStatus);
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
                $('td:eq(9)', nRow).append('<a href="" class="myIcon icon-success icon-ef-3 icon-ef-3a hover-color icon-center" data-tooltip="tooltip" title="View" attrid="' + aData[0] + '" Onclick=subjectImageStudyDetail(this) iImgTransmittalHdrId="' + aData[0] + '" iImgTransmittalDtlId ="' + aData[1] + '" vWorkspaceId ="' + aData[2] + '" vProjectNo ="' + aData[3] + '" vSubjectId ="' + aData[4] + '" iModalityNo="' + aData[9] + '" iAnatomyNo="' + aData[11] + '" iNodeId="' + aData[20] + '" iImageStatus="' + aData[22] + '" style="cursor:pointer;"><i class="fa  fa-info-circle"></i></a>');
                if (aData[22] == '1') {
                    $(nRow).addClass('highlight');
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
              { "sTitle": "Visit" },
              { "sTitle": "Image Status" },
            ],
            "columnDefs": [
              {
                  "targets": [0, 1, 2, 5, 9, 11, 12, 15, 16, 17,20,22],
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
              { "bSortable": false, "targets": [0, 1, 2, 5, 9, 11, 15, 16, 17,19,20,22] },
            ],
            "oLanguage": {
                "sEmptyTable": "No Record Found",
            }
        });

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
    //var iAnatomyNo = $(e).attr("iAnatomyNo"); // Changed Because Not Required Further Anatomy Wise Description
    var iAnatomyNo = "";
    var iNodeId = $(e).attr("iNodeId");

    var subjectImageStudyDetailData;
    var subjectImageStudyDetailAjaxData;

    var dicomStudyvActivityName = $("#dicomStudyvActivityName").val();

    subjectImageStudyDetailData = {
        iImgTransmittalHdrId: iImgTransmittalHdrId,
        iImgTransmittalDtlId: iImgTransmittalDtlId,
        iImageStatus:iImageStatus,
        vWorkspaceId: vWorkspaceId,
        vProjectNo: vProjectNo,
        vSubjectId: vSubjectId,
        iNodeId: iNodeId,
        iModalityNo: iModalityNo,
        iAnatomyNo: iAnatomyNo        
    }
    //data: JSON.stringify(subjectImageStudyDetailData),
    $.ajax({
        url: WebURL + "MIImageReview/DicomDetails",
        type: "POST",    
        data: subjectImageStudyDetailData,
        async: false,
        success: OnSuccess1,
        error: function (e) {
        alert("error")}
    });
    function OnSuccess1(jsonData) {
        var url = $("#DicomViewer").val();
        //location.href = url;
        //var url = $("#Viewer").attr('href');
        //window.open(url, "popupWindow", "width=600,height=800,scrollbars=yes,toolbar=0,location=0, directories=0, status=0, menubar=0");
        //window.open(url, "popupWindow", "directories = no, titlebar = no, toolbar = no, location = no, status = no, menubar = no, scrollbars = no, resizable = no, width = 900, height = 600");
        //window.open(url, "directories = no, titlebar = no, toolbar = no, location = no, status = no, menubar = no, scrollbars = no, resizable = no, width = 900, height = 600");
        window.open(url, "_blank");
        //directories = no, titlebar = no, toolbar = no, location = no, status = no, menubar = no, scrollbars = no, resizable = no, width = 720, height = 800
    }
    function OnErrorCall(ex) {
        //alert(ex);
    }

    //subjectImageStudyDetailAjaxData = {
    //    url: ApiURL + "GetData/SubjectImageStudyDetail",
    //    type: "POST",
    //    async: false,
    //    data: subjectImageStudyDetailData,
    //    success: successSubjectImageStudyDetail,
    //    error: errorSubjectImageStudyDetail
    //}

    //fnSubjectImageStudyDetail(subjectImageStudyDetailAjaxData.url, subjectImageStudyDetailAjaxData.type, subjectImageStudyDetailAjaxData.data, subjectImageStudyDetailAjaxData.async, subjectImageStudyDetailAjaxData.success, subjectImageStudyDetailAjaxData.error)
}

var fnSubjectImageStudyDetail = function (url, type, data, async, success, error) {
    $.ajax({
        url: url,
        type: type,
        data: data,
        async: async,
        success: success,
        error: error
    });
}

function successSubjectImageStudyDetail(jsonData) {
    if (jsonData.length > 0) {       
    }
    else {
        AlertBox("information", "Image Review!", "Dicom Image Found For Subject Not Found.!")
    }
}

function errorSubjectImageStudyDetail() {
    AlertBox("information", "Image Review!", "Error While Retriving Dicom Image Found For Subject.!")
}
