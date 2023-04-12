var vWorkspaceId, vProjectNo, vSubjectId, iNodeId, iModalityNo, iAnatomyNo, iuserid, iImgTransmittalHdrId, iImgTransmittalDtlId, iImageStatus, ActivityID, NodeID, ActivityDef, iMySubjectNo, ScreenNo, ParentWorkSpaceId, PeriodId, ActivityName, SubActivityName, subinodeID, parentActivityID, AdjUserId, UserTypeCode, StoredUserTypeCode;
var RDicom;
var WorkFlowStageId = ''
if (typeof MIBizNETAdjudicatorResponse == "undefined") {
    MIBizNETAdjudicatorResponse = {};
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
    debugger;
    //StoredUserTypeCode = document.getElementById("getUserTypeCode").value;
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
    debugger;
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
            //window.open(WebURL + 'MIDicomViewer/MIDicomViewer?WId=' + vWorkspaceId + '&SId=' + vSubjectId + '&PId=' + vProjectNo + '&Uid=' + iuserid + '&MId=' + iModalityNo + '&AId=' + iAnatomyNo + '&VId=' + iNodeId + '&HdrId=' + iImgTransmittalHdrId + '&DtlId=' + iImgTransmittalDtlId + '&iIS=' + iImageStatus + '&ActivityID=' + ActivityID + '&NodeID=' + NodeID + '&ActivityDef=' + ActivityDef + '&iMySubjectNo=' + iMySubjectNo + '&ScreenNo=' + ScreenNo + '&ParentWorkSpaceId=' + ParentWorkSpaceId + '&PeriodId=' + PeriodId, '_blank');
            //window.open(DISoftURL + 'MIBizNETAdjudicatorResponse/MIBizNETAdjudicatorResponse?WId=' + WId + '&SId=' + SId + '&PId=' + PId + '&Uid=' + Uid + '&MId=' + MId + '&AId=' + AId + '&VId=' + VId + '&HdrId=' + HdrId + '&DtlId=' + DtlId + '&ActivityID=' + ActivityID + '&NodeID=' + NodeId + '&ActivityDef=' + ActivityDef + '&iMySubjectNo=' + iMySubjectNo + '&ScreenNo=' + ScreenNo + '&ParentWorkSpaceId=' + ParentWorkSpaceId + '&PeriodId=' + PeriodId + '&ActivityName=' + ActivityName + '&SubActivityName=' + SubActivityName + '&subinodeID=' + subinodeID + '&parentActivityID=' + parentActivityID, '_blank');
            //window.location.replace(window.location.href);
            //window.location.replace(WebURL + 'MIDicomStudy/MIDicomStudy?WId=' + vWorkspaceId + '&SId=' + vSubjectId + '&PId=' + vProjectNo + '&Uid=' + $("#hdnAdjUserId").val() + '&MId=' + iModalityNo + '&AId=' + iAnatomyNo + '&VId=' + iNodeId + '&HdrId=' + iImgTransmittalHdrId + '&DtlId=' + iImgTransmittalDtlId + '&iIS=' + iImageStatus + '&ActivityID=' + ActivityID + '&NodeID=' + NodeID + '&ActivityDef=' + ActivityDef + '&iMySubjectNo=' + iMySubjectNo + '&ScreenNo=' + ScreenNo + '&ParentWorkSpaceId=' + ParentWorkSpaceId + '&PeriodId=' + PeriodId, '_blank');
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
                        //ActivityName = val;
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
                    else if (key == 'RDicom') {
                        RDicom = val;
                    } 
                }
            }
            debugger;
            var userLoginDetails = {
                vUserName: jsonData[0].vUserName,//$("#hdnUserName").val(),
                vLoginPass: '',
                vUserTypeCode: UserTypeCode,
                vIpAddress: $("#hdnIpAddress").val(),
                vUserAgent: $("#hdnUserAgent").val()
            };

            $.ajax({
                url: ApiURL + "GetData/UserAuthenticationDetails",
                type: 'POST',
                data: userLoginDetails,
                dataType: 'json',
                success: onSuccess,
                async: false,
                error: onError

            });

            function onSuccess(jsonData) {
                debugger;
                
                console.log(jsonData);
                //const data = JSON.parse(jsonData).filter(element => element.vUserTypeCode == UserTypeCode);
                var user = jsonData.Data[0].vUserName;
                //var currentUserAgent = $('#hdnUserAgent').val();
                var resultLogin = true;

                if (jsonData.Data.length != 0) {
                    if (jsonData.Data.length >= 0) {
                        //runEffect("loginFormModal");
                        resultLogin = false;

                        var SaveUserLoginData = {
                            iUserId: jsonData.Data[0].iUserId,
                            vIPAddress: $('#hdnIpAddress').val(),
                            vUTCHourDiff: jsonData.Data[0].vUTCHourDiff,
                            vUserAgent: jsonData.Data[0].vUserAgent,
                            DataopMode: 4
                        }
                        $.ajax({
                            type: 'POST',
                            data: SaveUserLoginData,
                            async: false,
                            url: ApiURL + "SetData/save_UserLoginDetails",
                            success: function () {
                                resultLogin = true;

                                window.sessionStorage.setItem("UserNameWithProfile", jsonData.Data[0].UserNameWithProfile);
                                var UserDetails = {};
                                UserDetails.iUserId = jsonData.Data[0].iUserId;
                                UserDetails.iUserGroupCode = jsonData.Data[0].iUserGroupCode;
                                UserDetails.vUserGroupName = jsonData.Data[0].vUserGroupName;
                                UserDetails.vUserName = jsonData.Data[0].vUserName;
                                UserDetails.vLoginName = jsonData.Data[0].vLoginName;
                                UserDetails.vLoginPass = jsonData.Data[0].vLoginPass;
                                UserDetails.vUserTypeCode = jsonData.Data[0].vUserTypeCode;
                                UserDetails.vUserTypeName = jsonData.Data[0].vUserTypeName;
                                UserDetails.vDeptCode = jsonData.Data[0].vDeptCode;
                                UserDetails.vDeptName = jsonData.Data[0].vDeptName;
                                UserDetails.vLocationCode = jsonData.Data[0].vLocationCode;
                                UserDetails.vLocationName = jsonData.Data[0].vLocationName;
                                UserDetails.vTimeZoneName = jsonData.Data[0].vTimeZoneName;
                                UserDetails.vLocationInitiate = jsonData.Data[0].vLocationInitiate;
                                UserDetails.vEmailId = jsonData.Data[0].vEmailId;
                                UserDetails.vPhoneNo = jsonData.Data[0].vPhoneNo;
                                UserDetails.vExtNo = jsonData.Data[0].vExtNo;
                                UserDetails.vRemark = jsonData.Data[0].vRemark;
                                UserDetails.iModifyBy = jsonData.Data[0].iModifyBy;
                                UserDetails.dModifyOn = jsonData.Data[0].dModifyOn;
                                UserDetails.cStatusIndi = jsonData.Data[0].cStatusIndi;
                                UserDetails.vFirstName = jsonData.Data[0].vFirstName;
                                UserDetails.vLastName = jsonData.Data[0].vLastName;
                                UserDetails.nScopeNo = jsonData.Data[0].nScopeNo;
                                UserDetails.vScopeName = jsonData.Data[0].vScopeName;
                                UserDetails.vScopeValues = jsonData.Data[0].vScopeValues;
                                UserDetails.iWorkflowStageId = jsonData.Data[0].iWorkflowStageId;
                                UserDetails.cIsEDCUser = jsonData.Data[0].cIsEDCUser;
                                UserDetails.dFromDate = jsonData.Data[0].dFromDate;
                                UserDetails.dToDate = jsonData.Data[0].dToDate;
                                UserDetails.ModifierName = jsonData.Data[0].ModifierName;
                                UserDetails.UserNameWithProfile = jsonData.Data[0].UserNameWithProfile;
                                UserDetails.tmp_dModifyOn = jsonData.Data[0].tmp_dModifyOn;
                                UserDetails.OperationCode = "MI";
                                UserDetails.tmp_dModifyOn = jsonData.Data[0].tmp_dModifyOn;
                                UserDetails.UserWise_CurrDateTime = jsonData.Data[0].UserWise_CurrDateTime;


                                $.ajax({
                                    type: "POST",
                                    url: WebURL + "MILogin/UserDetails",
                                    data: JSON.stringify(UserDetails),
                                    contentType: "application/json; charset=utf-8",
                                    //dataType: "json",
                                    dataType: "text",
                                    async: false,
                                    success: OnSuccess1,
                                    error: OnErrorCall
                                });
                                function OnSuccess1(jsonData) {
                                    //window.open(WebURL + 'MIDicomViewer/MIDicomViewer');
                                }
                                function OnErrorCall(ex) {
                                    //alert(ex);
                                }
                                result = true;
                                var redirectUrl = $('#redirectTo').val();
                                location.href = redirectUrl;
                                return false;
                                //return result;
                            },
                            error: function (ex) {
                                throw ex;
                                AlertBox("warning", " MI Login ", "Error while saving details of User Login details !")
                                //$("#txtUserName").focus();
                            }
                        });
                    }
                    else {
                        resultLogin = true;
                    }
                }
                if (resultLogin != false) {

                    window.sessionStorage.setItem("UserNameWithProfile", jsonData.Data[0].UserNameWithProfile);
                    var UserDetails = {};
                    UserDetails.iUserId = jsonData.Data[0].iUserId;
                    UserDetails.iUserGroupCode = jsonData.Data[0].iUserGroupCode;
                    UserDetails.vUserGroupName = jsonData.Data[0].vUserGroupName;
                    UserDetails.vUserName = jsonData.Data[0].vUserName;
                    UserDetails.vLoginName = jsonData.Data[0].vLoginName;
                    UserDetails.vLoginPass = jsonData.Data[0].vLoginPass;
                    UserDetails.vUserTypeCode = jsonData.Data[0].vUserTypeCode;
                    UserDetails.vUserTypeName = jsonData.Data[0].vUserTypeName;
                    UserDetails.vDeptCode = jsonData.Data[0].vDeptCode;
                    UserDetails.vDeptName = jsonData.Data[0].vDeptName;
                    UserDetails.vLocationCode = jsonData.Data[0].vLocationCode;
                    UserDetails.vLocationName = jsonData.Data[0].vLocationName;
                    UserDetails.vTimeZoneName = jsonData.Data[0].vTimeZoneName;
                    UserDetails.vLocationInitiate = jsonData.Data[0].vLocationInitiate;
                    UserDetails.vEmailId = jsonData.Data[0].vEmailId;
                    UserDetails.vPhoneNo = jsonData.Data[0].vPhoneNo;
                    UserDetails.vExtNo = jsonData.Data[0].vExtNo;
                    UserDetails.vRemark = jsonData.Data[0].vRemark;
                    UserDetails.iModifyBy = jsonData.Data[0].iModifyBy;
                    UserDetails.dModifyOn = jsonData.Data[0].dModifyOn;
                    UserDetails.cStatusIndi = jsonData.Data[0].cStatusIndi;
                    UserDetails.vFirstName = jsonData.Data[0].vFirstName;
                    UserDetails.vLastName = jsonData.Data[0].vLastName;
                    UserDetails.nScopeNo = jsonData.Data[0].nScopeNo;
                    UserDetails.vScopeName = jsonData.Data[0].vScopeName;
                    UserDetails.vScopeValues = jsonData.Data[0].vScopeValues;
                    UserDetails.iWorkflowStageId = jsonData.Data[0].iWorkflowStageId;
                    UserDetails.cIsEDCUser = jsonData.Data[0].cIsEDCUser;
                    UserDetails.dFromDate = jsonData.Data[0].dFromDate;
                    UserDetails.dToDate = jsonData.Data[0].dToDate;
                    UserDetails.ModifierName = jsonData.Data[0].ModifierName;
                    UserDetails.UserNameWithProfile = jsonData.Data[0].UserNameWithProfile;
                    UserDetails.tmp_dModifyOn = jsonData.Data[0].tmp_dModifyOn;
                    UserDetails.OperationCode = "MI";
                    UserDetails.tmp_dModifyOn = jsonData.Data[0].tmp_dModifyOn;
                    UserDetails.UserWise_CurrDateTime = jsonData.Data[0].UserWise_CurrDateTime;
                    var StoredActivityName = jsonData.Data[0].vUserTypeName;
                    
                    UserTypeCode = $("#hdnUserTypeCode").val();
                    $.ajax({
                        type: "POST",
                        url: WebURL + "MILogin/UserDetails",
                        data: JSON.stringify(UserDetails),
                        contentType: "application/json; charset=utf-8",
                        //dataType: "json",
                        dataType: "text",
                        async: false,
                        success: OnSuccess1,
                        error: OnErrorCall
                    });
                    function OnSuccess1(jsonData) {
                        
                        if (WorkFlowStageId != '') {
                            ActivityName = $("#hdnActivityName").val();
                            SubActivityName = $("#hdnSubActivityName").val();
                            window.location.replace(WebURL + 'MIDicomStudy/MIDicomStudy?WId=' + vWorkspaceId + '&SId=' + vSubjectId + '&PId=' + vProjectNo + '&Uid=' + $("#hdnAdjUserId").val() + '&MId=' + iModalityNo + '&AId=' + iAnatomyNo + '&VId=' + iNodeId + '&HdrId=' + iImgTransmittalHdrId + '&DtlId=' + iImgTransmittalDtlId + '&iIS=' + iImageStatus + '&iMySubjectNo=' + iMySubjectNo + '&ScreenNo=' + ScreenNo + '&PeriodId=' + PeriodId + '&UserProfile=' + ActivityName + '&SubActivityName=' + SubActivityName + '&WorkFlowStageId=' + WorkFlowStageId + '&RDicom=' + RDicom, '_blank');
                        }
                        else {
                            ActivityName = StoredActivityName;
                            SubActivityName = $("#hdnActivityName").val();
                            window.location.replace(WebURL + 'MIDicomStudy/MIDicomStudy?WId=' + vWorkspaceId + '&SId=' + vSubjectId + '&PId=' + vProjectNo + '&Uid=' + $("#hdnAdjUserId").val() + '&MId=' + iModalityNo + '&AId=' + iAnatomyNo + '&VId=' + iNodeId + '&HdrId=' + iImgTransmittalHdrId + '&DtlId=' + iImgTransmittalDtlId + '&iIS=' + iImageStatus + '&ActivityID=' + ActivityID + '&NodeID=' + NodeID + '&ActivityDef=' + ActivityDef + '&iMySubjectNo=' + iMySubjectNo + '&ScreenNo=' + ScreenNo + '&ParentWorkSpaceId=' + ParentWorkSpaceId + '&PeriodId=' + PeriodId + '&UserProfile=' + ActivityName + '&SubActivityName=' + SubActivityName + '&RDicom=' + RDicom, '_blank');
                        }
                    }
                    function OnErrorCall(ex) {
                        //alert(ex);
                    }
                    result = true;
                }
                else {
                    result = false;
                }
                return result;
            }

            function onError(jsonData) {
            }
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
            else if (key == 'iIS') {
                iImageStatus = val;
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
                //ActivityName = val;
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
        }
    }
    var userLoginDetails = {
        vUserName: $("#hdnUserName").val(),
        vLoginPass: '',
        vUserTypeCode: $("#hdnUserTypeCode").val(),
        vIpAddress: $("#hdnIpAddress").val(),
        vUserAgent: $("#hdnUserAgent").val()
    };

    $.ajax({
        url: ApiURL + "GetData/UserAuthenticationDetails",
        type: 'POST',
        data: userLoginDetails,
        dataType: 'json',
        success: onSuccess,
        async: false,
        error: onError

    });

    function onSuccess(jsonData) {
        debugger;
        console.log(jsonData);
        //const data = JSON.parse(jsonData).filter(element => element.vUserTypeCode == $("#vUserTypeCode").val());
        var user = jsonData.Data[0].vUserName;
        //var currentUserAgent = $('#hdnUserAgent').val();
        var resultLogin = true;

        if (jsonData.Data.length != 0) {
            if (jsonData.Data[0].MaxLoginMins >= 0) {
                runEffect("loginFormModal");
                resultLogin = false;

                var SaveUserLoginData = {
                    iUserId: jsonData.Data[0].iUserId,
                    vIPAddress: $('#hdnIpAddress').val(),
                    vUTCHourDiff: jsonData.Data[0].vUTCHourDiff,
                    vUserAgent: jsonData.Data[0].vUserAgent,
                    DataopMode: 1
                }
                $.ajax({
                    type: 'POST',
                    data: SaveUserLoginData,
                    async: false,
                    url: ApiURL + "SetData/save_UserLoginDetails",
                    success: function () {
                        resultLogin = true;

                        window.sessionStorage.setItem("UserNameWithProfile", jsonData.Data[0].UserNameWithProfile);
                        var UserDetails = {};
                        UserDetails.iUserId = jsonData.Data[0].iUserId;
                        UserDetails.iUserGroupCode = jsonData.Data[0].iUserGroupCode;
                        UserDetails.vUserGroupName = jsonData.Data[0].vUserGroupName;
                        UserDetails.vUserName = jsonData.Data[0].vUserName;
                        UserDetails.vLoginName = jsonData.Data[0].vLoginName;
                        UserDetails.vLoginPass = jsonData.Data[0].vLoginPass;
                        UserDetails.vUserTypeCode = jsonData.Data[0].vUserTypeCode;
                        UserDetails.vUserTypeName = jsonData.Data[0].vUserTypeName;
                        UserDetails.vDeptCode = jsonData.Data[0].vDeptCode;
                        UserDetails.vDeptName = jsonData.Data[0].vDeptName;
                        UserDetails.vLocationCode = jsonData.Data[0].vLocationCode;
                        UserDetails.vLocationName = jsonData.Data[0].vLocationName;
                        UserDetails.vTimeZoneName = jsonData.Data[0].vTimeZoneName;
                        UserDetails.vLocationInitiate = jsonData.Data[0].vLocationInitiate;
                        UserDetails.vEmailId = jsonData.Data[0].vEmailId;
                        UserDetails.vPhoneNo = jsonData.Data[0].vPhoneNo;
                        UserDetails.vExtNo = jsonData.Data[0].vExtNo;
                        UserDetails.vRemark = jsonData.Data[0].vRemark;
                        UserDetails.iModifyBy = jsonData.Data[0].iModifyBy;
                        UserDetails.dModifyOn = jsonData.Data[0].dModifyOn;
                        UserDetails.cStatusIndi = jsonData.Data[0].cStatusIndi;
                        UserDetails.vFirstName = jsonData.Data[0].vFirstName;
                        UserDetails.vLastName = jsonData.Data[0].vLastName;
                        UserDetails.nScopeNo = jsonData.Data[0].nScopeNo;
                        UserDetails.vScopeName = jsonData.Data[0].vScopeName;
                        UserDetails.vScopeValues = jsonData.Data[0].vScopeValues;
                        UserDetails.iWorkflowStageId = jsonData.Data[0].iWorkflowStageId;
                        UserDetails.cIsEDCUser = jsonData.Data[0].cIsEDCUser;
                        UserDetails.dFromDate = jsonData.Data[0].dFromDate;
                        UserDetails.dToDate = jsonData.Data[0].dToDate;
                        UserDetails.ModifierName = jsonData.Data[0].ModifierName;
                        UserDetails.UserNameWithProfile = jsonData.Data[0].UserNameWithProfile;
                        UserDetails.tmp_dModifyOn = jsonData.Data[0].tmp_dModifyOn;
                        UserDetails.OperationCode = "MI";
                        UserDetails.tmp_dModifyOn = jsonData.Data[0].tmp_dModifyOn;
                        UserDetails.UserWise_CurrDateTime = jsonData.Data[0].UserWise_CurrDateTime;


                        $.ajax({
                            type: "POST",
                            url: WebURL + "MILogin/UserDetails",
                            data: JSON.stringify(UserDetails),
                            contentType: "application/json; charset=utf-8",
                            //dataType: "json",
                            dataType: "text",
                            async: false,
                            success: OnSuccess1,
                            error: OnErrorCall
                        });
                        function OnSuccess1(jsonData) {
                            window.open(WebURL + 'MIDicomViewer/MIDicomViewer');
                        }
                        function OnErrorCall(ex) {
                            //alert(ex);
                        }
                        result = true;
                        var redirectUrl = $('#redirectTo').val();
                        location.href = redirectUrl;
                        return false;
                        //return result;
                    },
                    error: function (ex) {
                        throw ex;
                        AlertBox("warning", " MI Login ", "Error while saving details of User Login details !")
                        $("#txtUserName").focus();
                    }
                });
            }
            else {
                resultLogin = true;
            }
        }
        if (resultLogin != false) {

            window.sessionStorage.setItem("UserNameWithProfile", jsonData.Data[0].UserNameWithProfile);
            var UserDetails = {};
            UserDetails.iUserId = jsonData.Data[0].iUserId;
            UserDetails.iUserGroupCode = jsonData.Data[0].iUserGroupCode;
            UserDetails.vUserGroupName = jsonData.Data[0].vUserGroupName;
            UserDetails.vUserName = jsonData.Data[0].vUserName;
            UserDetails.vLoginName = jsonData.Data[0].vLoginName;
            UserDetails.vLoginPass = jsonData.Data[0].vLoginPass;
            UserDetails.vUserTypeCode = jsonData.Data[0].vUserTypeCode;
            UserDetails.vUserTypeName = jsonData.Data[0].vUserTypeName;
            UserDetails.vDeptCode = jsonData.Data[0].vDeptCode;
            UserDetails.vDeptName = jsonData.Data[0].vDeptName;
            UserDetails.vLocationCode = jsonData.Data[0].vLocationCode;
            UserDetails.vLocationName = jsonData.Data[0].vLocationName;
            UserDetails.vTimeZoneName = jsonData.Data[0].vTimeZoneName;
            UserDetails.vLocationInitiate = jsonData.Data[0].vLocationInitiate;
            UserDetails.vEmailId = jsonData.Data[0].vEmailId;
            UserDetails.vPhoneNo = jsonData.Data[0].vPhoneNo;
            UserDetails.vExtNo = jsonData.Data[0].vExtNo;
            UserDetails.vRemark = jsonData.Data[0].vRemark;
            UserDetails.iModifyBy = jsonData.Data[0].iModifyBy;
            UserDetails.dModifyOn = jsonData.Data[0].dModifyOn;
            UserDetails.cStatusIndi = jsonData.Data[0].cStatusIndi;
            UserDetails.vFirstName = jsonData.Data[0].vFirstName;
            UserDetails.vLastName = jsonData.Data[0].vLastName;
            UserDetails.nScopeNo = jsonData.Data[0].nScopeNo;
            UserDetails.vScopeName = jsonData.Data[0].vScopeName;
            UserDetails.vScopeValues = jsonData.Data[0].vScopeValues;
            UserDetails.iWorkflowStageId = jsonData.Data[0].iWorkflowStageId;
            UserDetails.cIsEDCUser = jsonData.Data[0].cIsEDCUser;
            UserDetails.dFromDate = jsonData.Data[0].dFromDate;
            UserDetails.dToDate = jsonData.Data[0].dToDate;
            UserDetails.ModifierName = jsonData.Data[0].ModifierName;
            UserDetails.UserNameWithProfile = jsonData.Data[0].UserNameWithProfile;
            UserDetails.tmp_dModifyOn = jsonData.Data[0].tmp_dModifyOn;
            UserDetails.OperationCode = "MI";
            UserDetails.tmp_dModifyOn = jsonData.Data[0].tmp_dModifyOn;
            UserDetails.UserWise_CurrDateTime = jsonData.Data[0].UserWise_CurrDateTime;


            $.ajax({
                type: "POST",
                url: WebURL + "MILogin/UserDetails",
                data: JSON.stringify(UserDetails),
                contentType: "application/json; charset=utf-8",
                //dataType: "json",
                dataType: "text",
                async: false,
                success: OnSuccess1,
                error: OnErrorCall
            });
            function OnSuccess1(jsonData) {

            }
            function OnErrorCall(ex) {
                //alert(ex);
            }
            result = true;
        }
        else {
            result = false;
        }
        return result;
    }

    function onError(jsonData) {
    }

    //window.open(WebURL + 'MIDicomViewer/MIDicomViewer?WId=' + vWorkspaceId + '&SId=' + vSubjectId + '&PId=' + vProjectNo + '&Uid=' + iuserid + '&MId=' + iModalityNo + '&AId=' + iAnatomyNo + '&VId=' + iNodeId + '&HdrId=' + iImgTransmittalHdrId + '&DtlId=' + iImgTransmittalDtlId + '&iIS=' + iImageStatus + '&ActivityID=' + ActivityID + '&NodeID=' + NodeID + '&ActivityDef=' + ActivityDef + '&iMySubjectNo=' + iMySubjectNo + '&ScreenNo=' + ScreenNo + '&ParentWorkSpaceId=' + ParentWorkSpaceId + '&PeriodId=' + PeriodId, '');
   // window.open(WebURL + 'MIDicomViewer/MIDicomViewer');

});
