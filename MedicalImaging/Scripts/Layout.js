var query
//$body = $("body");

//setTimeout(function () {
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
//}, 0);

$(document).ajaxStart(function () {
    var userLoginDetails
    userLoginDetails = {
        iUserId: $("#hdnuserid").val(),
        vIPAddress: $("#hdnIpAddress").val(),
        DATAOPMODE: 2
    }

    var ajaxData = {
        url: ApiURL + "SetData/save_UserLoginDetails",
        type: 'POST',
        async: false,
        data: userLoginDetails,
        success: successuserLoginDetails,
        error: erroruserLoginDetails
    }
    setTimeout(function () {
        $.ajax({
            url: ajaxData.url,
            type: ajaxData.type,
            data: ajaxData.data,
            //async: ajaxData.async,
            success: ajaxData.success,
            error: ajaxData.error,
            beforeSend: function () {
            },
            complete: function () {
            },
        });
    }, 0);

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
});

$(document).ajaxComplete(function () {});

$(document).ajaxError(function () {});

function logOut() {
    var userLoginDetails = {
        iUserId: $("#hdnuserid").val(),
        vIPAddress: $("#hdnIpAddress").val(),
        DATAOPMODE: 3
    }

    var ajaxData = {
        url: ApiURL + "SetData/save_UserLoginDetails",
        type: 'POST',
        //async: false,
        data: userLoginDetails,
        success: successlogOutUserDetails,
        error: errorlogOutUserDetails
    }
    setTimeout(function () {
        $.ajax({
            url: ajaxData.url,
            type: ajaxData.type,
            data: ajaxData.data,
            //async: ajaxData.async,
            success: ajaxData.success,
            error: ajaxData.error
        });
    }, 0);
    function successlogOutUserDetails(jsonData) {
        var url = $("#RedirectToLogin").val();
        location.href = url;
    }

    function errorlogOutUserDetails() {
        AlertBox('error', 'MI', 'Error While LogOut User!')
    }
}

$(function () {
    if (query == '' || query == "" || query == null) {
        getUserProfile();
        getUserMenu();
    }

    $("#signOut").on('click', function (e) {
        logOut();
    });

    $('.btnchange').hover(function () {
        $(this).stop().animate({ 'padding-left': '15px', 'padding-right': '45px' }, 'fast');
        $(this).find('.num').stop().animate({ 'left': '-50px' }, 'fast');
        $(this).find('.fa').stop().animate({ 'right': '0px' }, 'fast');
    }, function () {
        $(this).stop().animate({ 'padding-left': '45px', 'padding-right': '15px' }, 'fast');
        $(this).find('.num').stop().animate({ 'left': '0px' }, 'fast');
        $(this).find('.fa').stop().animate({ 'right': '-50px' }, 'fast');
    });

});

function getUserProfile() {
    var vUserName = $('#hdnUserName').val();
    var data = {
        vUserName: vUserName
    }
    var ajaxData = {
        async: false,
        data: data,
        type: 'POST',
        url: ApiURL + "GetData/UserProfile"
    }
    setTimeout(function () {
        $.ajax({
            url: ajaxData.url,
            type: ajaxData.type,
            data: data,
            //async: ajaxData.async,
            success: successUserProfile,
            error: errorUserProfile
        });
    }, 0);
    function successUserProfile() {
    }

    function errorUserProfile() {
        AlertBox('error', 'MI', 'Error While Retriving User Profile!')
    }
}

function getUserMenu() {
    var vUserTypeCode = $('#hdnUserTypeCode').val();

    data = {
        vUserTypeCode: vUserTypeCode
    }

    var ajaxData = {
        type: 'POST',
        data: data,
        url: ApiURL + "GetData/UserMenu",
        async: false,
        success: successUserMenu,
        error: errorUserMenu
    }

    //$.ajax({
    //    url: ajaxData.url,
    //    type: ajaxData.type,
    //    data: ajaxData.data,
    //    async: ajaxData.async,
    //    success: ajaxData.successUserMenu,
    //    error: ajaxData.errorUserMenu
    //});

    setTimeout(function () {
        $.ajax({
            url: ApiURL + "GetData/UserMenu",
            type: 'POST',
            data: data,
            //async: false,
            success: successUserMenu,
            error: errorUserMenu
        });
    }, 0);

    function successUserMenu(jsonData) {
        document.getElementById('userMenu').innerHTML = "";
        if (!window.location.match("ChangePassword")) {
            var strmenu = ""
            var strhdr = ""
            var childform = jsonData;
            var cnt1 = 1, cnt2 = 1;
            for (var i = 0; i < jsonData.length; i++) {
                if (jsonData[i].ParentID == null) {

                    if (strmenu == "") {
                        strhdr = '<li class="header"> MAIN NAVIGATION </li>';
                        strmenu += '<li class="active treeview">';
                    }
                    else {
                        strmenu += '<li class="treeview">';
                    }
                    strmenu += '<a href=' + jsonData[i].MenuURL + '>';
                    strmenu += '<i class="' + jsonData[i].vOperationClassName + '"></i>';
                    strmenu += '<span>' + jsonData[i].MenuText + '</span>';
                    strmenu += '<i class="fa fa-angle-left pull-right"></i>';
                    strmenu += '</a>';

                    if (jsonData[i].MenuID != '0900') {
                        strmenu += '<ul class="treeview-menu">';

                        if (jsonData[i].MenuURL != '#') {
                            strmenu += '<li ><a href= "' + jsonData[i].MenuURL + '" id="' + jsonData[i].MenuID + '" onmouseup=GetMenuId(this)><i class="fa fa-circle-o"></i>' + jsonData[i].MenuText + '</a></li>'
                        }
                    }

                    //strmenu += '<ul class="treeview-menu">';
                    //if (jsonData[i].MenuURL != '#') {
                    //    strmenu += '<li ><a href= "' + jsonData[i].MenuURL + '" id="' + jsonData[i].MenuID + '" onmouseup=GetMenuId(this)><i class="fa fa-circle-o"></i>' + jsonData[i].MenuText + '</a></li>'
                    //}
                }
                for (var j = i; j < childform.length; j++) {
                    if (jsonData[i].MenuID === childform[j].ParentID) {
                        strmenu += '<li ><a href= "' + childform[j].MenuURL + '" id="' + jsonData[j].MenuID + '" onmouseup=GetMenuId(this) onmousedown=GetMenuId(this) ><i class="fa fa-circle-o"></i>' + childform[j].MenuText + '</a></li>'
                    }
                }
                if (jsonData[i].ParentID == null) {
                    strmenu += '</ul>';
                    strmenu += '</li>';
                }
            }
            var str = strhdr + strmenu;
            $(".sidebar-menu").append(str);
        }
    }

    function successUserMenu(jsonData) {
        debugger;
        document.getElementById('userMenu').innerHTML = "";
        var url = window.location.href;
        if (url.includes("ChangePassword")) {
            document.getElementById('userMenu').innerHTML = ""
        }else{
            var strmenu = ""
            var strhdr = ""
            var childform = jsonData;
            var cnt1 = 1, cnt2 = 1;
            for (var i = 0; i < jsonData.length; i++) {
                if (jsonData[i].ParentID == null) {

                    if (strmenu == "") {
                        strhdr = '<li class="header"> MAIN NAVIGATION </li>';
                        strmenu += '<li class="active treeview">';
                    }
                    else {
                        strmenu += '<li class="treeview">';
                    }
                    strmenu += '<a href=' + jsonData[i].MenuURL + '>';
                    strmenu += '<i class="' + jsonData[i].vOperationClassName + '"></i>';
                    strmenu += '<span>' + jsonData[i].MenuText + '</span>';
                    strmenu += '<i class="fa fa-angle-left pull-right"></i>';
                    strmenu += '</a>';
                    if (jsonData[i].MenuID != '0900') {
                        strmenu += '<ul class="treeview-menu">';

                        //}
                        //if (jsonData[i].MenuID != '0900') {
                        if (jsonData[i].MenuURL != '#') {
                            strmenu += '<li ><a href= "' + jsonData[i].MenuURL + '" id="' + jsonData[i].MenuID + '" onmouseup=GetMenuId(this)><i class="fa fa-circle-o"></i>' + jsonData[i].MenuText + '</a></li>'
                        }
                    }
                }
                for (var j = i; j < childform.length; j++) {
                    if (jsonData[i].MenuID === childform[j].ParentID) {
                        strmenu += '<li ><a href= "' + childform[j].MenuURL + '" id="' + jsonData[j].MenuID + '" onmouseup=GetMenuId(this) onmousedown=GetMenuId(this) ><i class="fa fa-circle-o"></i>' + childform[j].MenuText + '</a></li>'
                    }
                }
                if (jsonData[i].ParentID == null) {
                    strmenu += '</ul>';
                    strmenu += '</li>';
                }
            }
            var str = strhdr + strmenu;
            $(".sidebar-menu").append(str);
        }
    }

    function errorUserMenu() {
        AlertBox('error', 'MI', 'Error While Retriving User Menu Rights!')
    }
}

var sTime = new Date().getTime();
var countDown = 3600 * 3; //Modify by Bhargav Thaker 14Mar2023  if reverse=3600 * 1.5
var secondflag = true;

function UpdateCountDownTime() {
    var cTime = new Date().getTime();
    var diff = cTime - sTime;
    var timeStr = '';
    var seconds = countDown - Math.floor(diff / 1000);

    if (seconds == 300) {
        if (secondflag == true) {
            $.confirm({
                title: 'Confirm!',
                icon: 'fa fa-warning',
                content: 'USER CONFORMATION',
                onContentReady: function () {
                    var self = this;
                    this.setContentPrepend('<div>MI</div>');
                    setTimeout(function () {
                        self.setContentAppend('<div>Your session will expire within 5 Mins. <br />Do You Want To Continue ?</div>');
                    }, 1000);
                },
                columnClass: 'medium',
                //animation: 'zoom',
                closeIcon: true,
                closeIconClass: 'fa fa-close',
                //columnClass: 'small',
                boxWidth: '30%',
                autoClose: 'danger|300000',
                animation: 'news',
                closeAnimation: 'news',
                closeAnimation: 'scale',
                backgroundDismissAnimation: 'random',
                //backgroundDismissAnimation: 'glow',
                type: 'blue',
                theme: 'blue',
                draggable: true,
                buttons: {
                    info: {
                        btnClass: 'btn-blue',
                        text: 'OK (O)',
                        keys: ['O'],
                        action: function () {
                            countDown = 3600 * 2;
                            sTime = new Date().getTime();
                        }
                    },
                    danger: {
                        btnClass: 'btn-red any-other-class',
                        text: 'CANCEL (C)',
                        keys: ['C'],
                        action: function () {
                            return
                        }
                    },

                }
            });
            secondflag = false;
        }
        //if (confirm('Your session will expire within 5 mins.')) {
        //}
        //else {
        //    countDown = 3600;
        //    sTime = new Date().getTime();
        //}
    }
    if (seconds >= 0) {
        var hours = Math.floor(seconds / 3600);
        var minutes = Math.floor((seconds - (hours * 3600)) / 60);
        seconds -= (hours * 3600) + (minutes * 60);
        if (hours < 10) {
            timeStr = "0" + hours;
        } else {
            timeStr = hours;
        }
        if (minutes < 10) {
            timeStr = timeStr + ":0" + minutes;
        } else {
            timeStr = timeStr + ":" + minutes;
        }
        if (seconds < 10) {
            timeStr = timeStr + ":0" + seconds;
        } else {
            timeStr = timeStr + ":" + seconds;
        }
        document.getElementById("sessionExpired").innerHTML = timeStr;
    }
    else {
        //document.getElementById("timerText").style.display = "none";
        var url = $("#RedirectToLogin").val();
        AlertBox("WARNING", "MI", "Your session has expired!");
        logOut();
        location.href = url;
        clearInterval(counter);
    }
}

window.onresize = function (e) {
    //if ((window.outerHeight - window.innerHeight) > 100)
    //    alert('Docked inspector was opened');
    var threshold = 160;
    if ((window.outerHeight - window.innerHeight) > threshold)
        //alert('Docked inspector was opened');        
        if ((window.outerWidth - window.innerWidth) > threshold) {
            //alert('Docked inspector was opened');
            //this.close()
            //var e = new Event("keydown");
            //e.key = "F12";    // just enter the char you want to send 
            //e.keyCode = e.key.charCodeAt(0);
            //e.which = e.keyCode;
            //e.altKey = false;
            //e.ctrlKey = true;
            //e.shiftKey = false;
            //e.metaKey = false;
            //e.bubbles = true;
            //document.dispatchEvent(e);


            //alert('Docked inspector was opened');
            //var code = 123
            //jQuery.Event('keydown', { keyCode: code, which: code });

            //$('input').trigger(e);
            //jQuery.trigger({ type: 'keypress', which: '123' });
            //e = $.Event('keyup');
            //e.keyCode = 123; // enter
            //$('input').trigger(e);
        }
}

UpdateCountDownTime();
var counter = setInterval(UpdateCountDownTime, 30);

document.onkeydown = function (e) {
    if (event.keyCode == 123) {
        return false;
    }
    if (e.ctrlKey && e.shiftKey && e.keyCode == 'I'.charCodeAt(0)) {
        return false;
    }
    if (e.ctrlKey && e.shiftKey && e.keyCode == 'C'.charCodeAt(0)) {
        return false;
    }
    if (e.ctrlKey && e.shiftKey && e.keyCode == 'J'.charCodeAt(0)) {
        return false;
    }
    if (e.ctrlKey && e.keyCode == 'U'.charCodeAt(0)) {
        return false;
    }
}
document.addEventListener('contextmenu', function (e) {
    e.preventDefault();
});