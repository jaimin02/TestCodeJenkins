if (typeof MIStatisticsReport == "undefined") {
    MIStatisticsReport = {};
}
if (typeof General == "undefined") {
    General = {};
}

$(function () {

    $(".select2").select2();

    $("#btnGo").on('click', function () {
        if ($("#rbt_AdjudicatorOverAll").prop("checked")) {
            MIStatisticsReport.MIStatisticsReportDetail1();
        }
        else if ($("#rbt_AdjudicatorResult").prop("checked")) {
            MIStatisticsReport.MIStatisticsReportDetail2();
        }
        else {
            MIStatisticsReport.MIStatisticsReportDetail();
        }
        
    });

    $("#ddlstudy").on('change', function () {
        $("#ddlProject").empty();
        $("#select2-ddlProject-container").empty()
            $("#spanSkip").empty();
            var contextKeyVal = ' iUserid =' + $("#hdnuserid").val() + 'AND ParentWorkspaceId=' + $("#ddlstudy").val();
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
                url: WebURL + "MIStatisticsReport/MyProjectCompletionList",
                success: MIStatisticsReport.successUserWiseProjectDetail,
                error: MIStatisticsReport.errorUserWiseProjectDetail
            }
            MIStatisticsReport.fnUserWiseProjectDetail(userWiseProjectDetailAjaxData.async, userWiseProjectDetailAjaxData.data, userWiseProjectDetailAjaxData.type, userWiseProjectDetailAjaxData.url, userWiseProjectDetailAjaxData.success, userWiseProjectDetailAjaxData.success, userWiseProjectDetailAjaxData.error)
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
            if (jsonData.responseText != "") {
                jsonData = JSON.parse(jsonData.responseText)
                $("#ddlProject").append($("<option></option>").html("Select Project").val(""));
                for (var V = 0 ; V < jsonData.length ; V++) {
                    $("#ddlProject").append($("<option></option>").html(jsonData[V].WorkspaceMerge).val(jsonData[V].vWorkspaceId));
                }
            }
            else {
                AlertBox("Information", "Dicom Study!", "No Data Available For Project!")
                if ($(tblStatisticsDetail) != 'undefined') {
                    if ($.fn.DataTable.isDataTable('#tblStatisticsDetail')) {
                        var table = $('#tblStatisticsDetail').DataTable();
                        table.clear();
                        table.destroy();
                        $("#tblStatisticsDetail").find("thead").html("");
                    }
                }
            }
        }
        function errorUserWiseProjectDetail() {
            AlertBox("error", "Dicom Study!", "Error While Retriving Project Details!");
        }

       
        MIStatisticsReport.successUserWiseProjectDetail = successUserWiseProjectDetail;
        MIStatisticsReport.errorUserWiseProjectDetail = errorUserWiseProjectDetail;
        MIStatisticsReport.fnUserWiseProjectDetail = fnUserWiseProjectDetail;
    });

    $('input[type=radio][name=statisticsReport]').change(function () {
        if ($(tblStatisticsDetail) != 'undefined') {
            if ($.fn.DataTable.isDataTable('#tblStatisticsDetail')) {
                var table = $('#tblStatisticsDetail').DataTable();
                table.clear();
                table.destroy();
                $("#tblStatisticsDetail").find("thead").html("");
            }
        }
    });

    (function ($, MIStatisticsReport, General) {
        "use strict";
        function validate() {
            if (ddlBlankCheck('ddlstudy')) {
                AlertBox("warning", "Statistics Report!", "Please Select Study.!");
                return false
            }
            if (ddlBlankCheck('ddlProject')) {
                AlertBox("warning", "Statistics Report!", "Please Select Project.!");
                return false
            }
        }
        MIStatisticsReport.validate = validate;
    })($, MIStatisticsReport, General);

    (function ($, MIStatisticsReport, General) {
        "use strict";

        function getUserWiseProjectDetail() {
            $("#spanSkip").empty();
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
                url: WebURL + "MIStatisticsReport/MyProjectCompletionList",
                success: MIStatisticsReport.successUserWiseProjectDetail,
                error: MIStatisticsReport.errorUserWiseProjectDetail
            }
            MIStatisticsReport.fnUserWiseProjectDetail(userWiseProjectDetailAjaxData.async, userWiseProjectDetailAjaxData.data, userWiseProjectDetailAjaxData.type, userWiseProjectDetailAjaxData.url, userWiseProjectDetailAjaxData.success, userWiseProjectDetailAjaxData.success, userWiseProjectDetailAjaxData.error)
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
            if (jsonData.responseText != "") {
                jsonData = JSON.parse(jsonData.responseText)
                $("#ddlProject").append($("<option></option>").html("Select Project").val(""));
                for (var V = 0 ; V < jsonData.length ; V++) {
                    $("#ddlProject").append($("<option></option>").html(jsonData[V].WorkspaceMerge).val(jsonData[V].vWorkspaceId));
                }
            }
            else {
                AlertBox("Information", "Dicom Study!", "No Data Available For Project!")
                if ($(tblStatisticsDetail) != 'undefined') {
                    if ($.fn.DataTable.isDataTable('#tblStatisticsDetail')) {
                        var table = $('#tblStatisticsDetail').DataTable();
                        table.clear();
                        table.destroy();
                        $("#tblStatisticsDetail").find("thead").html("");
                    }
                }
            }
        }
        function errorUserWiseProjectDetail() {
            AlertBox("error", "Dicom Study!", "Error While Retriving Project Details!");
        }

        MIStatisticsReport.getUserWiseProjectDetail = getUserWiseProjectDetail;
        MIStatisticsReport.successUserWiseProjectDetail = successUserWiseProjectDetail;
        MIStatisticsReport.errorUserWiseProjectDetail = errorUserWiseProjectDetail;
        MIStatisticsReport.fnUserWiseProjectDetail = fnUserWiseProjectDetail;

    }($, MIStatisticsReport, General));

    (function ($, MIStatisticsReport, General) {
        "use strict";

        function MIStatisticsReportDetail() {

            if (MIStatisticsReport.validate() == false) {
                return false;
            }

            var vWorkspaceId = $("#ddlProject").val();
            var vParentWorkspaceId = $("#ddlstudy").val();
            var MIStatisticsReportData = {
                vWorkspaceId: vWorkspaceId,
                vParentWorkspaceId:vParentWorkspaceId,
                vBtnRadioName: $('input[name=statisticsReport]:checked').val()
            }

            var MIStatisticsReportAjaxData = {
                //url: ApiURL + "GetData/MIStatisticReport",
                url: WebURL + "MIStatisticsReport/MIStatisticReport",
                type: "POST",
                data: MIStatisticsReportData,
                success: MIStatisticsReport.successMIStatisticsReport,
                error: MIStatisticsReport.errorMIStatisticsReport
            }

            MIStatisticsReport.fnMIStatisticsReport(MIStatisticsReportAjaxData.url, MIStatisticsReportAjaxData.type, MIStatisticsReportAjaxData.data, MIStatisticsReportAjaxData.success, MIStatisticsReportAjaxData.error)

        }
        var fnMIStatisticsReport = function (url, type, data, success, error) {
            setTimeout(function () {
                $.ajax({
                    url: url,
                    type: type,
                    data: data,
                    complete: success,
                    error: error
                });
            }, 0);
            return this;
        }
        function successMIStatisticsReport(jsonMIStatisticsReport) {
            if (jsonMIStatisticsReport.responseText != "") {
                var activityDataSet = [];
                var randomizationNo = "";
                jsonMIStatisticsReport = JSON.parse(jsonMIStatisticsReport.responseText)
                for (var i = 0; i < jsonMIStatisticsReport.length; i++) {
                    var inDataSet = [];
                    if (i == 0) {
                        //inDataSet.push("Site Identification", "Screening No", "Date of Birth", "Subject Initial", "Visit Number", "Sub Visit Number", "Study Identification", "Date of Examination", "Modality", "Image Quality", "Image Quality Remarks", "Reviewer Name", "Review Date", "Lesion Name", "Organ", "Location", "Description", "Size", "Sum of Diameter", "% Change from Baseline", "% Change from NADIR", "Target Response", "Non Target Response", "New Lesion", "Overall Response", "Global Review", "Global Review Comments", "Comments");
                        //activityDataSet.push(inDataSet);
                    }
                    if (jsonMIStatisticsReport[i].vRandomizationNo == "0") {
                        randomizationNo = "";
                    }
                    else {
                        randomizationNo = jsonMIStatisticsReport[i].vRandomizationNo.toString();
                    }
                    
                     inDataSet.push(jsonMIStatisticsReport[i].ChildProject, jsonMIStatisticsReport[i].ScreenNo, randomizationNo, jsonMIStatisticsReport[i].dBirthDate,
                        jsonMIStatisticsReport[i].FullName, jsonMIStatisticsReport[i].vActivityName, jsonMIStatisticsReport[i].vSubActivityName,
                        jsonMIStatisticsReport[i].ParentProject, jsonMIStatisticsReport[i].dExaminationDate, jsonMIStatisticsReport[i].vModalityDesc,
                        jsonMIStatisticsReport[i].vImageQualityAdequate, jsonMIStatisticsReport[i].vRemarkQuality,
                        jsonMIStatisticsReport[i].ReviewerName, jsonMIStatisticsReport[i].ReviewerDate,
                        jsonMIStatisticsReport[i].vLession, jsonMIStatisticsReport[i].vOrgen, jsonMIStatisticsReport[i].vLocation,
                        jsonMIStatisticsReport[i].vDescription, jsonMIStatisticsReport[i].vSize, jsonMIStatisticsReport[i].vSumOfDiameter,
                        jsonMIStatisticsReport[i].vChangeFromBaseline, jsonMIStatisticsReport[i].vChangeFromNADIR,
                        jsonMIStatisticsReport[i].vTargetResponse, jsonMIStatisticsReport[i].vNonTargetResponse,
                        jsonMIStatisticsReport[i].vNewLesion, jsonMIStatisticsReport[i].vOverallResponse,
                        jsonMIStatisticsReport[i].vGlobalReview, jsonMIStatisticsReport[i].vGlobalReviewComments, jsonMIStatisticsReport[i].vComments,
                        jsonMIStatisticsReport[i].vBOR, jsonMIStatisticsReport[i].vBORRemarks);
                    activityDataSet.push(inDataSet);
                }
                
                //var otable = $("#tblStatisticsDetail").prepend($('<thead>').append($("#tblStatisticsDetail").find('tr:first'))).dataTable({
                var otable = $("#tblStatisticsDetail").dataTable({                
                    "bJQueryUI": true,
                    "sPaginationType": "full_numbers",
                    "bLengthChange": true,
                    "iDisplayLength": 5,
                    "bProcessing": true,
                    "bSort": true,
                    "autoWidth": false,
                    "aaData": activityDataSet,
                    "bInfo": true,
                    "bDestroy": true,
                    "sScrollX": "100%",
                    "scrollY": "600px",
                    "scrollCollapse": true,
                    aaSorting: [[0, 'desc']],
                    dom: 'Bfrtip',
                    buttons: [
                        //'copy', 'csv', 'excel', 'pdf', 'print',
                        'copy', 'csv',
                         {
                             extend: 'excelHtml5',
                             title: 'STATISTICS DETAIL REPORT',
                             //extend: 'excelFlash',
                             filename: 'STATISTICS DETAIL REPORT',
                             customize: function (xlsx) {
                                 //var sheet = xlsx.xl.worksheets['sheet1.xml'];
                                 //$('c[r=A1] t', sheet).text('Custom text');
                             }
                         },
                        {
                            extend: 'pdfHtml5',                            
                            orientation: 'landscape',
                            pageSize: 'A1',
                            text: 'PDF',
                            title: 'STATISTICS DETAIL REPORT',                            
                            styles: {
                                tableHeader: {
                                    bold: true,
                                    fontSize: 9,
                                    color: 'white',
                                    fillColor: '#2d4154',
                                    alignment: 'center'
                                },
                                tableBodyEven: {
                                    alignment: 'center',
                                },
                                tableBodyOdd: {
                                    alignment: 'center',
                                    fillColor: '#f3f3f3'
                                },
                                tableFooter: {
                                    bold: true,
                                    fontSize: 11,
                                    color: 'white',
                                    fillColor: '#2d4154'
                                },
                                title: {
                                    alignment: 'center',
                                    fontSize: 15
                                },
                                message: {}
                            },
                        },
                          {
                              extend: 'print',
                              orientation: 'landscape',
                              pageSize: 'Tabloid',
                              title: 'STATISTICS DETAIL REPORT',
                              customize: function (win) {
                                  $(win.document.body)
                                      .css('font-size', '6.5pt')
                                      .prepend(
                                          //'<img src="http://datatables.net/media/images/logo-fade.png" style="position:absolute; top:0; left:0;" />'
                                      );

                                  $(win.document.body).find('table')
                                      .addClass('compact')
                                      .css('font-size', 'inherit');
                              }
                          },
                    ],
                    //columnDefs: [{
                    //    //targets: -1,
                    //    //visible: false
                    //},
                    //{ "width": "40%", "targets":  1},
                    //],

                    //aoColumnDefs: [{ "sWidth": "20%", "aTargets": [ 1,2,3,4,5,6 ] }] ,

                    "aoColumns": [
                       //{ "sTitle": "Child Project" },
                       { "sTitle": "SITE" },
                       //{ "sTitle": "Screening No" },
                       { "sTitle": "SCREENID" },
                       //{ "sTitle": "Randomization No" },
                       { "sTitle": "RNDNO" },
                       //{ "sTitle": "Birth Date" },
                       { "sTitle": "DOB" },
                       //{ "sTitle": "Full Name" },
                       { "sTitle": "SUBJINIT" },
                       //{ "sTitle": "Activity" },
                       { "sTitle": "VISITNO" },
                       //{ "sTitle": "Sub Activity" },
                       { "sTitle": "SUBVISITNO" },
                       //{ "sTitle": "Parent Project" },
                       { "sTitle": "PROTOCOL" },
                       //{ "sTitle": "Examination Date" },
                       { "sTitle": "EXAMDT" },
                       //{ "sTitle": "Modality" },
                       { "sTitle": "INVESTYP" },
                        //{ "sTitle": "Image Quality" },
                       { "sTitle": "IMAGEQTY" },
                       //{ "sTitle": "Quality Remark" },
                       { "sTitle": "IMAGEQTYREMARK" },
                       //{ "sTitle": "Reviewer Name" },
                       { "sTitle": "REVIEWNM" },                       
                       //{ "sTitle": "Review Date" },
                       { "sTitle": "REVIEWDT" },
                       //{ "sTitle": "Lesion" },
                       { "sTitle": "LESONNM" },
                       //{ "sTitle": "Organ" },
                       { "sTitle": "ORGAN" },
                       //{ "sTitle": "Location" },
                       { "sTitle": "TULOC" },
                        //{ "sTitle": "Description" },
                       { "sTitle": "TUDESC" },
                       //{ "sTitle": "Size" },
                       { "sTitle": "TUSIZE" },                      
                       //{ "sTitle": "Sum Of Diameter" },
                       { "sTitle": "SOD" },
                       //{ "sTitle": "% Change From BaseLine" },
                       { "sTitle": "BASELINE" },
                       //{ "sTitle": "% Change From NADIR" },
                       { "sTitle": "NADIR" },
                       //{ "sTitle": "Target Lesion Response" },
                       { "sTitle": "TLRES" },
                       //{ "sTitle": "Non LesionTarget Response" },
                       { "sTitle": "NTLRES" },
                       //{ "sTitle": "New Lesion Response" },
                       { "sTitle": "NEWLESON" },
                       //{ "sTitle": "Overall Response" },
                       { "sTitle": "OVRLRES" },
                       //{ "sTitle": "Global Review" },
                       { "sTitle": "GLOBREV" },
                       //{ "sTitle": "Global Review Comments" },
                       { "sTitle": "GLOBREVCOMMENT" },
                       //{ "sTitle": "Comments" },
                       { "sTitle": "COMMENT" },

                        { "sTitle": "BOR Reponse" },

                         { "sTitle": "BOR Remarks" },
                    ],
                    
                    "oLanguage": {
                        "sEmptyTable": "No Record Found",
                    },                    
                });
                //setTimeout(function () { $("#tblStatisticsDetail").dataTable().fnAdjustColumnSizing(); }, 10);
                //$("#tblStatisticsDetail").find('tbody tr').length < 3 ? scroll = "25%" : scroll = "285px";

            }
            else {
                AlertBox("warning", "Statistics Report!", "No Data Found For Statistics Report.!");
                if ($(tblStatisticsDetail) != 'undefined') {
                    if ($.fn.DataTable.isDataTable('#tblStatisticsDetail')) {
                        var table = $('#tblStatisticsDetail').DataTable();
                        table.clear();
                        table.destroy();
                        $("#tblStatisticsDetail").find("thead").html("");
                    }
                }
            }
        }
        function errorMIStatisticsReport(e) {
            AlertBox("error", "Statistics Report!", "Error While Retriving Statistics Report Detail!");
        }

        MIStatisticsReport.MIStatisticsReportDetail = MIStatisticsReportDetail;
        MIStatisticsReport.successMIStatisticsReport = successMIStatisticsReport;
        MIStatisticsReport.errorMIStatisticsReport = errorMIStatisticsReport;
        MIStatisticsReport.fnMIStatisticsReport = fnMIStatisticsReport;

    })($, MIStatisticsReport, General);

    (function ($, MIStatisticsReport, General) {
        "use strict";

        function getUserWiseStudyDetail() {
            $("#spanSkip").empty();
            var contextKeyVal = ' iUserid =' + $("#hdnuserid").val();
            var vProjectTypeCodeVal = $("#hdnscopevalues").val()
            //var vProjectTypeCodeVal =  "0001,0002,0003,0004,0005,0006,0007,0008,0009,0010,0014,0015,0016,0017,0018,0019,0020,0021,,,,,,,,,,,,,"
            vProjectTypeCodeVal = "'" + vProjectTypeCodeVal.replace(/,/g, "','") + "'"
            var userWiseStudyDetailData = {
                contextKey: contextKeyVal,
                vProjectTypeCode: vProjectTypeCodeVal,
                prefixText: ''
            }
            var userWiseStudyDetailAjaxData = {
                async: false,
                data: userWiseStudyDetailData,
                type: "POST",
                //url: ApiURL + "GetData/MyProjectCompletionList",
                url: WebURL + "MIStatisticsReport/MyStudyCompletionList",
                success: MIStatisticsReport.successUserWiseStudyDetail,
                error: MIStatisticsReport.errorUserWiseStudyDetail
            }
            MIStatisticsReport.fnUserWiseStudyDetail(userWiseStudyDetailAjaxData.async, userWiseStudyDetailAjaxData.data, userWiseStudyDetailAjaxData.type, userWiseStudyDetailAjaxData.url, userWiseStudyDetailAjaxData.success, userWiseStudyDetailAjaxData.success, userWiseStudyDetailAjaxData.error)
        }
        var fnUserWiseStudyDetail = function (async, data, type, url, success, error) {
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
        function successUserWiseStudyDetail(jsonData) {
            if (jsonData.responseText != "") {
                jsonData = JSON.parse(jsonData.responseText)
                $("#ddlstudy").append($("<option></option>").html("Select Study").val(""));
                for (var V = 0 ; V < jsonData.length ; V++) {
                    $("#ddlstudy").append($("<option></option>").html(jsonData[V].vProjectNo).val(jsonData[V].vWorkspaceId));
                }
            }
            else {
                AlertBox("Information", "Dicom Study!", "No Data Available For Study!")
                if ($(tblStatisticsDetail) != 'undefined') {
                    if ($.fn.DataTable.isDataTable('#tblStatisticsDetail')) {
                        var table = $('#tblStatisticsDetail').DataTable();
                        table.clear();
                        table.destroy();
                        $("#tblStatisticsDetail").find("thead").html("");
                    }
                }
            }
        }
        function errorUserWiseStudyDetail() {
            AlertBox("error", "Dicom Study!", "Error While Retriving Study Details!");
        }

        MIStatisticsReport.getUserWiseStudyDetail = getUserWiseStudyDetail;
        MIStatisticsReport.successUserWiseStudyDetail = successUserWiseStudyDetail;
        MIStatisticsReport.errorUserWiseStudyDetail = errorUserWiseStudyDetail;
        MIStatisticsReport.fnUserWiseStudyDetail = fnUserWiseStudyDetail;

    }($, MIStatisticsReport, General));


    (function ($, MIStatisticsReport, General) {
        "use strict";

        function MIStatisticsReportDetail1() {

            if (MIStatisticsReport.validate() == false) {
                return false;
            }

            var vWorkspaceId = $("#ddlProject").val();
            var iMode = parseInt("1");

            var MIStatisticsReportData = {
                vWorkspaceId : vWorkspaceId,
                imode: iMode
            }

            var MIStatisticsReportAjaxData = {
                //url: ApiURL + "GetData/MIStatisticReport",
                url: WebURL + "MIStatisticsReport/MIStatisticReport1",
                type: "POST",
                data: MIStatisticsReportData,
                success: MIStatisticsReport.successMIStatisticsReport1,
                error: MIStatisticsReport.errorMIStatisticsReport1
            }

            MIStatisticsReport.fnMIStatisticsReport(MIStatisticsReportAjaxData.url, MIStatisticsReportAjaxData.type, MIStatisticsReportAjaxData.data, MIStatisticsReportAjaxData.success, MIStatisticsReportAjaxData.error)

        }
        var fnMIStatisticsReport = function (url, type, data, success, error) {
            setTimeout(function () {
                $.ajax({
                    url: url,
                    type: type,
                    data: data,
                    complete: success,
                    error: error
                });
            }, 0);
            return this;
        }
        function successMIStatisticsReport1(jsonMIStatisticsReport) {
            debugger;
            if (jsonMIStatisticsReport.responseText != "") {
                var activityDataSet = [];
                var randomizationNo = "";
                jsonMIStatisticsReport = JSON.parse(jsonMIStatisticsReport.responseText)
                for (var i = 0; i < jsonMIStatisticsReport.length; i++) {
                    var inDataSet = [];

                    inDataSet.push(jsonMIStatisticsReport[i].ScreenNo, jsonMIStatisticsReport[i].dExaminationDate,
                                   jsonMIStatisticsReport[i].TP2Response, jsonMIStatisticsReport[i].DateOfImage2,
                                   jsonMIStatisticsReport[i].TP3Response, jsonMIStatisticsReport[i].DateOfImage3,
                                   jsonMIStatisticsReport[i].TP4Response, jsonMIStatisticsReport[i].DateOfImage4,
                                   jsonMIStatisticsReport[i].TP5Response, jsonMIStatisticsReport[i].DateOfImage5,
                                   jsonMIStatisticsReport[i].TP6Response, jsonMIStatisticsReport[i].DateOfImage6,
                                   jsonMIStatisticsReport[i].TP7Response, jsonMIStatisticsReport[i].DateOfImage7,
                                   jsonMIStatisticsReport[i].TP8Response, jsonMIStatisticsReport[i].DateOfImage8,
                                   jsonMIStatisticsReport[i].TP9Response, jsonMIStatisticsReport[i].DateOfImage9,
                                   jsonMIStatisticsReport[i].TP10Response, jsonMIStatisticsReport[i].DateOfImage10,
                                   jsonMIStatisticsReport[i].TP11Response, jsonMIStatisticsReport[i].DateOfImage11,
                                   jsonMIStatisticsReport[i].TP12Response, jsonMIStatisticsReport[i].DateOfImage12,
                                   jsonMIStatisticsReport[i].TP13Response, jsonMIStatisticsReport[i].DateOfImage13,
                                   jsonMIStatisticsReport[i].TP14Response, jsonMIStatisticsReport[i].DateOfImage14,
                                   jsonMIStatisticsReport[i].TP15Response, jsonMIStatisticsReport[i].DateOfImage15);
                    activityDataSet.push(inDataSet);
                }

                var otable = $("#tblStatisticsDetail").dataTable({
                    "bJQueryUI": true,
                    "sPaginationType": "full_numbers",
                    "bLengthChange": true,
                    "iDisplayLength": 5,
                    "bProcessing": true,
                    "bSort": true,
                    "autoWidth": false,
                    "aaData": activityDataSet,
                    "bInfo": true,
                    "bDestroy": true,
                    "sScrollX": "100%",
                    "scrollY": "600px",
                    "scrollCollapse": true,
                    aaSorting: [[0, 'desc']],
                    dom: 'Bfrtip',
                    buttons: [
                        'copy', //'csv', //'excel',
                         {
                             extend: 'excelHtml5',
                             //title: 'Adjudicated Final Results',
                             filename: 'Adjudicated Final Results',
                             messageTop: 'Adjudicated Final Results'
                             //customize: function (xlsx) {
                                 
                             //}
                         },
                         //'csv', //'excel',
                         //{
                         //    extend: 'excelHtml5',
                         //    //title: 'Adjudicated Final Results',
                         //    filename: 'Adjudicated Final Results',
                         //    messageTop: 'Adjudicated Final Results'
                         //    //customize: function (xlsx) {

                         //    //}
                         //},
                        {
                            extend: 'pdfHtml5',
                            orientation: 'landscape',
                            pageSize: 'A1',
                            text: 'PDF',
                            title: 'Adjudicated Final Results',
                            styles: {
                                tableHeader: {
                                    bold: true,
                                    fontSize: 9,
                                    color: 'white',
                                    fillColor: '#2d4154',
                                    alignment: 'center'
                                },
                                tableBodyEven: {
                                    alignment: 'center',
                                },
                                tableBodyOdd: {
                                    alignment: 'center',
                                    fillColor: '#f3f3f3'
                                },
                                tableFooter: {
                                    bold: true,
                                    fontSize: 11,
                                    color: 'white',
                                    fillColor: '#2d4154'
                                },
                                title: {
                                    alignment: 'center',
                                    fontSize: 15
                                },
                                message: {}
                            },
                        },
                          {
                              extend: 'print',
                              orientation: 'landscape',
                              pageSize: 'Tabloid',
                              title: 'Adjudicated Final Results',
                              customize: function (win) {
                                  $(win.document.body)
                                      .css('font-size', '6.5pt')
                                      .prepend(
                                          //'<img src="http://datatables.net/media/images/logo-fade.png" style="position:absolute; top:0; left:0;" />'
                                      );

                                  $(win.document.body).find('table')
                                      .addClass('compact')
                                      .css('font-size', 'inherit');
                              }
                          },
                    ],

                    "aoColumns": [
                       { "sTitle": "Screen No" },
                       { "sTitle": "Examination Date" },
                       { "sTitle": "TP2 Response" },
                       { "sTitle": "Date of Image 2" },
                       { "sTitle": "TP3 Response" },
                       { "sTitle": "Date of Image 3" },
                       { "sTitle": "TP4 Response" },
                       { "sTitle": "Date of Image 4" },
                       { "sTitle": "TP5 Response" },
                       { "sTitle": "Date of Image 5" },
                       { "sTitle": "TP6 Response" },
                       { "sTitle": "Date of Image 6" },
                       { "sTitle": "TP7 Response" },
                       { "sTitle": "Date of Image 7" },
                       { "sTitle": "TP8 Response" },
                       { "sTitle": "Date of Image 8" },
                       { "sTitle": "TP9 Response" },
                       { "sTitle": "Date of Image 9" },
                       { "sTitle": "TP10 Response" },
                       { "sTitle": "Date of Image 10" },
                       { "sTitle": "TP11 Response" },
                       { "sTitle": "Date of Image 11" },
                       { "sTitle": "TP12 Response" },
                       { "sTitle": "Date of Image 12" },
                       { "sTitle": "TP13 Response" },
                       { "sTitle": "Date of Image 13" },
                       { "sTitle": "TP14 Response" },
                       { "sTitle": "Date of Image 14" },
                       { "sTitle": "TP15 Response" },
                       { "sTitle": "Date of Image 15" },
                       
                    ],

                    "oLanguage": {
                        "sEmptyTable": "No Record Found",
                    },
                });

            }
            else {
                AlertBox("warning", "Adjudicated Final Results!", "No Data Found For Statistics Report.!");
                if ($(tblStatisticsDetail) != 'undefined') {
                    if ($.fn.DataTable.isDataTable('#tblStatisticsDetail')) {
                        var table = $('#tblStatisticsDetail').DataTable();
                        table.clear();
                        table.destroy();
                        $("#tblStatisticsDetail").find("thead").html("");
                    }
                }
            }
        }
        function errorMIStatisticsReport1(e) {
            AlertBox("error", "Adjudicated Final Results!", "Error While Retriving Statistics Report Detail!");
        }

        MIStatisticsReport.MIStatisticsReportDetail1 = MIStatisticsReportDetail1;
        MIStatisticsReport.successMIStatisticsReport1 = successMIStatisticsReport1;
        MIStatisticsReport.errorMIStatisticsReport1 = errorMIStatisticsReport1;
        MIStatisticsReport.fnMIStatisticsReport = fnMIStatisticsReport;

    })($, MIStatisticsReport, General);

    (function ($, MIStatisticsReport, General) {
        "use strict";

        function MIStatisticsReportDetail2() {

            if (MIStatisticsReport.validate() == false) {
                return false;
            }

            var vWorkspaceId = $("#ddlProject").val();
            var iMode = parseInt("2");

            var MIStatisticsReportData = {
                vWorkspaceId: vWorkspaceId,
                imode: iMode
            }

            var MIStatisticsReportAjaxData = {
                //url: ApiURL + "GetData/MIStatisticReport",
                url: WebURL + "MIStatisticsReport/MIStatisticReport1",
                type: "POST",
                data: MIStatisticsReportData,
                success: MIStatisticsReport.successMIStatisticsReport2,
                error: MIStatisticsReport.errorMIStatisticsReport
            }

            MIStatisticsReport.fnMIStatisticsReport(MIStatisticsReportAjaxData.url, MIStatisticsReportAjaxData.type, MIStatisticsReportAjaxData.data, MIStatisticsReportAjaxData.success, MIStatisticsReportAjaxData.error)

        }
        var fnMIStatisticsReport = function (url, type, data, success, error) {
            setTimeout(function () {
                $.ajax({
                    url: url,
                    type: type,
                    data: data,
                    complete: success,
                    error: error
                });
            }, 0);
            return this;
        }
        function successMIStatisticsReport2(jsonMIStatisticsReport) {
            debugger;
            if (jsonMIStatisticsReport.responseText != "") {
                var activityDataSet = [];
                var randomizationNo = "";
                jsonMIStatisticsReport = JSON.parse(jsonMIStatisticsReport.responseText)
                for (var i = 0; i < jsonMIStatisticsReport.length; i++) {
                    var inDataSet = [];

                    inDataSet.push(jsonMIStatisticsReport[i].ScreenNo, 
                                   jsonMIStatisticsReport[i].TimePoint, 
                                   jsonMIStatisticsReport[i].AdjudicatorResult, 
                                   jsonMIStatisticsReport[i].Remark,
                                   jsonMIStatisticsReport[i].ChangeOn);
                    activityDataSet.push(inDataSet);
                }

                var otable = $("#tblStatisticsDetail").dataTable({
                    "bJQueryUI": true,
                    "sPaginationType": "full_numbers",
                    "bLengthChange": true,
                    "iDisplayLength": 5,
                    "bProcessing": true,
                    "bSort": true,
                    "autoWidth": false,
                    "aaData": activityDataSet,
                    "bInfo": true,
                    "bDestroy": true,
                    "sScrollX": "100%",
                    "scrollY": "600px",
                    "scrollCollapse": true,
                    aaSorting: [[0, 'desc']],
                    dom: 'Bfrtip',
                    buttons: [
                        'copy', 'csv',
                         {
                             extend: 'excelHtml5',
                             title: 'Adjudicator Agreement Report',
                             filename: 'Adjudicator Agreement Report',
                             customize: function (xlsx) {

                             }
                         },
                        {
                            extend: 'pdfHtml5',
                            orientation: 'landscape',
                            pageSize: 'A1',
                            text: 'PDF',
                            title: 'Adjudicator Agreement Report',
                            styles: {
                                tableHeader: {
                                    bold: true,
                                    fontSize: 9,
                                    color: 'white',
                                    fillColor: '#2d4154',
                                    alignment: 'center'
                                },
                                tableBodyEven: {
                                    alignment: 'center',
                                },
                                tableBodyOdd: {
                                    alignment: 'center',
                                    fillColor: '#f3f3f3'
                                },
                                tableFooter: {
                                    bold: true,
                                    fontSize: 11,
                                    color: 'white',
                                    fillColor: '#2d4154'
                                },
                                title: {
                                    alignment: 'center',
                                    fontSize: 15
                                },
                                message: {}
                            },
                        },
                          {
                              extend: 'print',
                              orientation: 'landscape',
                              pageSize: 'Tabloid',
                              title: 'Adjudicator Agreement Report',
                              customize: function (win) {
                                  $(win.document.body)
                                      .css('font-size', '6.5pt')
                                      .prepend(
                                          //'<img src="http://datatables.net/media/images/logo-fade.png" style="position:absolute; top:0; left:0;" />'
                                      );

                                  $(win.document.body).find('table')
                                      .addClass('compact')
                                      .css('font-size', 'inherit');
                              }
                          },
                    ],

                    "aoColumns": [
                       { "sTitle": "Screen No" },
                       { "sTitle": "Time Point" },
                       { "sTitle": "Adjudicator Result" },
                       { "sTitle": "Remark" },
                       { "sTitle": "ChangeOn" },

                    ],

                    "oLanguage": {
                        "sEmptyTable": "No Record Found",
                    },
                });

            }
            else {
                AlertBox("warning", "Adjudicator Agreement Report!", "No Data Found For Statistics Report.!");
                if ($(tblStatisticsDetail) != 'undefined') {
                    if ($.fn.DataTable.isDataTable('#tblStatisticsDetail')) {
                        var table = $('#tblStatisticsDetail').DataTable();
                        table.clear();
                        table.destroy();
                        $("#tblStatisticsDetail").find("thead").html("");
                    }
                }
            }
        }
        function errorMIStatisticsReport2(e) {
            AlertBox("error", "Adjudicator Agreement Report!", "Error While Retriving Statistics Report Detail!");
        }

        MIStatisticsReport.MIStatisticsReportDetail2 = MIStatisticsReportDetail2;
        MIStatisticsReport.successMIStatisticsReport2 = successMIStatisticsReport2;
        MIStatisticsReport.errorMIStatisticsReport2 = errorMIStatisticsReport2;
        MIStatisticsReport.fnMIStatisticsReport = fnMIStatisticsReport;

    })($, MIStatisticsReport, General);



    MIStatisticsReport.getUserWiseProjectDetail();
    MIStatisticsReport.getUserWiseStudyDetail();

   
   
});