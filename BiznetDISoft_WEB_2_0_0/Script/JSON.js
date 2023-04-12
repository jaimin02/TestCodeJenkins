
$(document).ready(function () {


    $('#ctl00_cpMRPortal_ddlCountry option').each(function () {
        this.text = this.text.substr(0, 1).toUpperCase() + this.text.substr(1).toLowerCase();
    });

    if (navigator.userAgent.toUpperCase().indexOf("IPAD") != -1) {
        $("iframe", parent.document).each(function () {
            if (this.src == window.location.href.toString().substr(window.location.href.toString().lastIndexOf("/") + 1)) {
                $("#divMain").css('width', $(this).parent().width());
            }
        });
    }
    if (navigator.appName.toUpperCase() == "OPERA") {
        $("html").each(function () {
            $(this).css('overflow', 'auto');
        });
    }
    //    if (navigator.userAgent.toUpperCase().indexOf("OPERA") != -1 || navigator.userAgent.toUpperCase().indexOf("TRIDENT") != -1) {
    //        $("html").each(function(){
    //            $(this).css('overflow', 'auto');
    //        });
    //    }

    if ($("#ctl00_cpMRPortal_lblError"))
        $("#ctl00_cpMRPortal_lblError").hide();

});

function getCookie(c_name) {
    var i, x, y, ARRcookies = document.cookie.split(";");
    for (i = 0; i < ARRcookies.length; i++) {
        x = ARRcookies[i].substr(0, ARRcookies[i].indexOf("="));
        y = ARRcookies[i].substr(ARRcookies[i].indexOf("=") + 1);
        x = x.replace(/^\s+|\s+$/g, "");
        if (x == c_name) {
            return unescape(y);
        }
    }
}

function setCookie(c_name, value, exdays) {
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + exdays);
    var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
    document.cookie = c_name + "=" + c_value;
}

// This function is used when click on the audit trail from any form.
// This function display the audit trail as a popup.
function OpenModalPopup(Mode) {

    // $("#div_ModalPopup").dialog("destroy");
    var height = (Mode == undefined) ? 300 : $(document).height();
    var title = (Mode == undefined) ? 'Audit Trail' : 'Dashboard Proto (default color scheme)';
    var color = (Mode == undefined) ? getCookie('WidgetTheme') : 'color21';

    $("#div_ModalPopup").dialog({
        autoOpen: true,
        show: "blind",
        hide: "explode",
        title: title,
        height: height - 2,
        width: $(document).width() - 20,
        minheight: 200,
        minwidth: 300,
        position: 'center',
        draggable: false,
        modal: true,
        resizable: false,
        zIndex: 11111112,
        create: function (event, ui) {
            $(".ui-widget-header").attr('class', $(".ui-widget-header").attr('class').replace(/color/g, '') + ' ' + color);
        }
        //        open: function (event, ui) {
        //            //$(".ui-widget-header").attr('class', $(".ui-widget-header").attr('class').replace(/color/g, '') + ' ' + 'color21');
        //            $(".selector").dialog("option", "dialogClass", getCookie('WidgetTheme'));
        //            //    $('#ui-dialog-title-div_ModalPopup').parent().attr('class', getCookie('WidgetTheme')); 
        //        }

    });

    // console.log($('#ui-dialog-title-div_ModalPopup').parent().attr('class'));
}



//set the src property of iframe after page load.its increse the performance of loading.
//$("iframe").attr('src', 'https://webmail.invida.com/owa/?ae=Folder&t=IPF.Note&id=LgAAAAB85PYel8PRR4XiIxeiPn88AQAnp%2f5ko%2baYSK2MVuwZbos5AFzzTAa6AAAB&slUsng=0');



//Check box validation

// Selecting all checkbox of Widgets to Forward.
$("#ctl00_cpMRPortal_chkAllWidgetSelection").live("click", function () {
    if (this.checked == true) {
        $('#ctl00_cpMRPortal_chkSelectWidget input[type="checkbox"]').each(function () {
            this.checked = true;
        });
    }
    else {
        $('#ctl00_cpMRPortal_chkSelectWidget input[type="checkbox"]').each(function () {
            this.checked = false;
        });
    }
});

// Selecting all checkbox of Widgets to Backword.
$("#ctl00_cpMRPortal_chkAllSelectedWidget").live("click", function () {
    if (this.checked == true) {
        $('#ctl00_cpMRPortal_chkbxDiselectWidgts input[type="checkbox"]').each(function () {
            this.checked = true;
        });
    }
    else {
        $('#ctl00_cpMRPortal_chkbxDiselectWidgts input[type="checkbox"]').each(function () {
            this.checked = false;
        });
    }
});

// For Deselecting chkAllWidgetSelection while none of the checkbox is selected.
$('#ctl00_cpMRPortal_chkSelectWidget input[type="checkbox"]').live("click", function () {
    if (document.getElementById('ctl00_cpMRPortal_chkAllWidgetSelection').checked == true) {
        document.getElementById('ctl00_cpMRPortal_chkAllWidgetSelection').checked = false;
    }
});

// For Deselecting ctl00_cpMRPortal_chkAllSelectedWidget while none of the checkbox is selected.
$('#ctl00_cpMRPortal_chkbxDiselectWidgts input[type="checkbox"]').live("click", function () {
    if (document.getElementById('ctl00_cpMRPortal_chkAllSelectedWidget').checked == true) {
        document.getElementById('ctl00_cpMRPortal_chkAllSelectedWidget').checked = false;
    }
});

// Selecting all checkbox of User.
$("#ctl00_cpMRPortal_chkSelectAllUser").live("click", function () {
    if (this.checked == true) {
        $('#ctl00_cpMRPortal_chkUser input[type="checkbox"]').each(function () {
            this.checked = true;
        });
    }
    else {
        $('#ctl00_cpMRPortal_chkUser input[type="checkbox"]').each(function () {
            this.checked = false;
        });
    }
});


// For Deselecting chkSelectAllUser while none of the User is selected.
$('#ctl00_cpMRPortal_chkUser input[type="checkbox"]').live("click", function () {
    if (document.getElementById('ctl00_cpMRPortal_chkSelectAllUser').checked == true) {
        document.getElementById('ctl00_cpMRPortal_chkSelectAllUser').checked = false;
    }
});

// Selecting all checkbox of Widget Group.
$("#ctl00_cpMRPortal_chkSelectAllGroup").live("click", function () {
    if (this.checked == true) {
        $('#ctl00_cpMRPortal_chkGroup input[type="checkbox"]').each(function () {
            this.checked = true;
        });
    }
    else {
        $('#ctl00_cpMRPortal_chkGroup input[type="checkbox"]').each(function () {
            this.checked = false;
        });
    }
});


// For Deselecting chkSelectAllUser while none of the User is selected.
$('#ctl00_cpMRPortal_chkGroup input[type="checkbox"]').live("click", function () {
    if (document.getElementById('ctl00_cpMRPortal_chkSelectAllGroup').checked == true) {
        document.getElementById('ctl00_cpMRPortal_chkSelectAllGroup').checked = false;
    }
});

// Selecting all checkbox of Division to Forward.
$("#ctl00_cpMRPortal_chkAllDivisions").live("click", function () {
    if (this.checked == true) {
        $('#ctl00_cpMRPortal_chkDivision input[type="checkbox"]').each(function () {
            this.checked = true;
        });
    }
    else {
        $('#ctl00_cpMRPortal_chkDivision input[type="checkbox"]').each(function () {
            this.checked = false;
        });
    }
});

// For Deselecting chkAllDivisions while none of the checkbox is selected.
$('#ctl00_cpMRPortal_chkDivision input[type="checkbox"]').live("click", function () {
    if (document.getElementById('ctl00_cpMRPortal_chkAllDivisions').checked == true) {
        document.getElementById('ctl00_cpMRPortal_chkAllDivisions').checked = false;
    }
});


function ValidateAssignWidgetTemplate() {

    var chkUser = $('#ctl00_cpMRPortal_chkUser input[type=checkbox]:checked').length;
    if (chkUser == 0) {
        alert('Please select at least one user.');
        return false;
    }

    //    if (document.getElementById('ctl00_cpMRPortal_ddlGroup').selectedIndex <= 0) {
    //        alert('Please select widget group.');
    //        document.getElementById('ctl00_cpMRPortal_ddlGroup').focus();
    //        document.getElementById('ctl00_cpMRPortal_ddlGroup').style.backgroundColor = "#FFE6F7";
    //        return false;
    //    }
}

function TemplateResetControls() {
    document.getElementById('ctl00_cpMRPortal_chkSelectAllUser').checked = false;
    $('#ctl00_cpMRPortal_chkUser input[type="checkbox"]').each(function () {
        this.checked = false;
    });
    document.getElementById('ctl00_cpMRPortal_ddlGroup').selectedIndex = 0;
    ClearValidateNameEditTime();
    return false;
}

function ValidateWidget() {

    if (document.getElementById('ctl00_cpMRPortal_txtWidgetName').value.trim() == '') {
        alert("Please enter widget name.");
        document.getElementById('ctl00_cpMRPortal_txtWidgetName').value = '';
        document.getElementById('ctl00_cpMRPortal_txtWidgetName').focus();
        document.getElementById('ctl00_cpMRPortal_txtWidgetName').style.backgroundColor = "#FFE6F7";
        return false;
    }
    else if (document.getElementById('ctl00_cpMRPortal_txtWidgetUrl').value.trim() == '') {
        alert("Please enter widget URL.");
        document.getElementById('ctl00_cpMRPortal_txtWidgetUrl').value = '';
        document.getElementById('ctl00_cpMRPortal_txtWidgetUrl').focus();
        document.getElementById('ctl00_cpMRPortal_txtWidgetUrl').style.backgroundColor = "#FFE6F7";
        return false;
    }
    else if (document.getElementById('ctl00_cpMRPortal_txtWidgetUrlMax').value.trim() == '') {
        alert("Please enter widget URL (Maximized).");
        document.getElementById('ctl00_cpMRPortal_txtWidgetUrlMax').value = '';
        document.getElementById('ctl00_cpMRPortal_txtWidgetUrlMax').focus();
        document.getElementById('ctl00_cpMRPortal_txtWidgetUrlMax').style.backgroundColor = "#FFE6F7";
        return false;
    }
    else if (document.getElementById('ctl00_cpMRPortal_txtWidth').value.trim() == '') {
        alert('Please enter width of widget.');
        document.getElementById('ctl00_cpMRPortal_txtWidth').value = '';
        document.getElementById('ctl00_cpMRPortal_txtWidth').focus();
        document.getElementById('ctl00_cpMRPortal_txtWidth').style.backgroundColor = "#FFE6F7";
        return false;
    }

    var no;
    no = parseInt(document.getElementById('ctl00_cpMRPortal_txtWidth').value.trim());
    if (no < 25 || no > 100) {
        alert('Width should be between 25% and 100%.');
        document.getElementById('ctl00_cpMRPortal_txtWidth').focus();
        document.getElementById('ctl00_cpMRPortal_txtWidth').value = '';
        document.getElementById('ctl00_cpMRPortal_txtWidth').style.backgroundColor = "#FFE6F7";
        return false;
    }
    else if (document.getElementById('ctl00_cpMRPortal_txtHeight').value.trim() == '') {
        alert('Please enter height of widget.');
        document.getElementById('ctl00_cpMRPortal_txtHeight').value = '';
        document.getElementById('ctl00_cpMRPortal_txtHeight').focus();
        document.getElementById('ctl00_cpMRPortal_txtHeight').style.backgroundColor = "#FFE6F7";
        return false;
    }

    //var no;
    no = parseInt(document.getElementById('ctl00_cpMRPortal_txtHeight').value.trim());
    if (no < 25 || no > 100) {
        alert('Height should be between 25% and 100%.');
        document.getElementById('ctl00_cpMRPortal_txtHeight').value = '';
        document.getElementById('ctl00_cpMRPortal_txtHeight').focus();
        document.getElementById('ctl00_cpMRPortal_txtHeight').style.backgroundColor = "#FFE6F7";
        return false;
    }


    document.getElementById('ctl00_cpMRPortal_hdnTemplateCount').value = $('#ctl00_cpMRPortal_chkGroup input[type=checkbox]:checked').length;

}

//function ResetControls() {
//    document.getElementById('ctl00_cpMRPortal_txtWidgetName').value = '';
//    document.getElementById('ctl00_cpMRPortal_txtWidgetUrl').value = '';
//    document.getElementById('ctl00_cpMRPortal_txtWidgetUrlMax').value = '';
//    document.getElementById('ctl00_cpMRPortal_txtWidth').value = '25';
//    document.getElementById('ctl00_cpMRPortal_txtHeight').value = '25';
//    document.getElementById('ctl00_cpMRPortal_hdnMode').value = 'A';
//    document.getElementById('ctl00_cpMRPortal_btnSave').value = 'Save';
//    document.getElementById('ctl00_cpMRPortal_trGroup').style.display = '';
//    return false;
//}

function setUrl(ele) {
    $(ele).attr('href', $(ele).prev().prev().val() + '' + $(ele).prev().val());

}



//Created By : Nidhi Patel
//Created Date: 6-Mar-2012
//Reason : For Brand Master

function BrandMstValidation(btn) {
    var IsDivisionSelected = false;
    $('#ctl00_cpMRPortal_chkDivision input[type="checkbox"]').each(function () {
        if (this.checked == true) { IsDivisionSelected = true; }
    });
    if (IsDivisionSelected == false) {
        alert('Please select at least one division for adding brand.');
        return false;
    }

    if (document.getElementById('ctl00_cpMRPortal_txtBrandName').value.trim() == '') {
        alert('Please enter brand name.');
        document.getElementById('ctl00_cpMRPortal_txtBrandName').value = '';
        document.getElementById('ctl00_cpMRPortal_txtBrandName').focus();
        document.getElementById('ctl00_cpMRPortal_txtBrandName').style.backgroundColor = "#FFE6F7";
        return false;
    }
    if (document.getElementById('ctl00_cpMRPortal_fuBrandLogo').value.trim() == '' && document.getElementById('ctl00_cpMRPortal_ImgEditBrand').style.display == 'none') {
        alert('Please select file to upload.');
        document.getElementById('ctl00_cpMRPortal_fuBrandLogo').focus();
        document.getElementById('ctl00_cpMRPortal_fuBrandLogo').style.backgroundColor = "#FFE6F7";
        return false;
    }

    //  if (btn.value.trim().toUpperCase() == 'SAVE') {
    if (document.getElementById('ctl00_cpMRPortal_fuBrandLogo').value.trim() != '') {
        if (document.getElementById('ctl00_cpMRPortal_fuBrandLogo').value.split('.')[1].toString().toUpperCase() != 'JPG' && document.getElementById('ctl00_cpMRPortal_fuBrandLogo').value.split('.')[1].toString().toUpperCase() != 'GIF' && document.getElementById('ctl00_cpMRPortal_fuBrandLogo').value.split('.')[1].toString().toUpperCase() != 'PNG' && document.getElementById('ctl00_cpMRPortal_fuBrandLogo').value.split('.')[1].toString().toUpperCase() != 'JPEG') {
            alert('Please select .jpg / .jpeg / .gif / .png  file.');
            document.getElementById('ctl00_cpMRPortal_fuBrandLogo').focus();
            document.getElementById('ctl00_cpMRPortal_fuBrandLogo').style.backgroundColor = "#FFE6F7";
            return false;
        }

    }


    document.getElementById('ctl00_cpMRPortal_ddlCountry').disabled = false;
    return validateName('ctl00_cpMRPortal_gvBrandMst', 'ctl00_cpMRPortal_txtBrandName', 'Brand', '3', 'ctl00_cpMRPortal_ddlCountry', 'W');

}
function SetControlValue(e) {

    document.getElementById('ctF)l00_cpMRPortal_ddlCountry').disabled = true;
    var index = e.id.split('_')[3].replace('ctl', '') - 1;
    var grid = document.getElementById('ctl00_cpMRPortal_gvBrandMst');
    document.getElementById('ctl00_cpMRPortal_hdnBrandId').value = grid.rows[index].cells[1].innerHTML;
    document.getElementById('ctl00_cpMRPortal_ddlCountry').value = grid.rows[index].cells[2].innerHTML;
    document.getElementById('ctl00_cpMRPortal_txtBrandName').value = grid.rows[index].cells[4].innerHTML;
    document.getElementById('ctl00_cpMRPortal_btnSaveBrand').value = 'Update';
    return false;
}

function ValidateDocumentMgmt(btn) {
    var IsTypeSelected = false;
    $('#ctl00_cpMRPortal_rdDocType input[type="radio"]').each(function () {
        if (this.checked == true) { IsTypeSelected = true; }
    });
    if (IsTypeSelected == false) {
        alert('Please select at least one document type.');
        document.getElementById('ctl00_cpMRPortal_rdDocType').focus();
        return false;
    }

    if ($('#ctl00_cpMRPortal_ddlCountry option').length <= 0) {
        alert('Country not available. Please contact Administrator.');
        document.getElementById('ctl00_cpMRPortal_ddlCountry').focus();
        document.getElementById('ctl00_cpMRPortal_ddlCountry').style.backgroundColor = "#FFE6F7";
        return false;
    }

    if ($('#ctl00_cpMRPortal_ddlDivision option').length <= 0) {
        alert('No division available for this country. Please contact Administrator.');
        document.getElementById('ctl00_cpMRPortal_ddlDivision').focus();
        document.getElementById('ctl00_cpMRPortal_ddlDivision').style.backgroundColor = "#FFE6F7";
        return false;
    }

    if (document.getElementById('ctl00_cpMRPortal_txtDocTitle').value.trim() == '') {
        alert('Please enter document title.');
        document.getElementById('ctl00_cpMRPortal_txtDocTitle').focus();
        document.getElementById('ctl00_cpMRPortal_txtDocTitle').style.backgroundColor = "#FFE6F7";
        return false;
    }
    else if (document.getElementById('ctl00_cpMRPortal_tr_BrandName').style.display == '' && $('#ctl00_cpMRPortal_ddlBrand option').length <= 0) {
        alert('No brand available for this country. Please contact Administrator.');
        document.getElementById('ctl00_cpMRPortal_ddlBrand').focus();
        document.getElementById('ctl00_cpMRPortal_ddlBrand').style.backgroundColor = "#FFE6F7";
        return false;
    }

    if (btn.value.trim().toUpperCase() == 'SAVE') {
        if (document.getElementById('ctl00_cpMRPortal_fuUploadPDF').value.trim() == '') {
            alert('Please select document to upload.');
            document.getElementById('ctl00_cpMRPortal_fuUploadPDF').focus();
            document.getElementById('ctl00_cpMRPortal_fuUploadPDF').style.backgroundColor = "#FFE6F7";
            return false;
        }
        else if (document.getElementById('ctl00_cpMRPortal_fuUploadPDF').value.split('.')[1] != 'pdf') {
            alert('Please select .pdf file.');
            document.getElementById('ctl00_cpMRPortal_fuUploadPDF').focus();
            document.getElementById('ctl00_cpMRPortal_fuUploadPDF').style.backgroundColor = "#FFE6F7";
            document.getElementById('ctl00_cpMRPortal_fuUploadPDF').value = '';
            return false;
        }
    }

    return validateName('ctl00_cpMRPortal_gvDocMgmt', 'ctl00_cpMRPortal_txtDocTitle', 'Document Title', '1', 'N', 'Y', 'W');

}

function openWindow(url) {
    var w = window.open(url, '', 'width=1000,height=600,toolbar=0,status=0,location=0,menubar=0,directories=0,resizable=1,scrollbars=1');
    w.focus();
    return false;
}

function DocumentResetControls() {
    document.getElementById('ctl00_cpMRPortal_rdDocType_0').checked = true
    /* document.getElementById('ctl00_cpMRPortal_ddlCountry').selectedIndex = 0;*/
    document.getElementById('ctl00_cpMRPortal_tr_BrandName').style.display = 'none';
    document.getElementById('ctl00_cpMRPortal_lnkFile').style.display = 'none';

    document.getElementById('ctl00_cpMRPortal_txtDocTitle').value = '';
    document.getElementById('ctl00_cpMRPortal_fuUploadPDF').value = '';
    document.getElementById('ctl00_cpMRPortal_btnSave').value = 'Save';
    document.getElementById('ctl00_cpMRPortal_lbl_editmsg').style.display = 'none';
    if (document.getElementById('ctl00_cpMRPortal_ddlBrand')) { document.getElementById('ctl00_cpMRPortal_ddlBrand').selectedIndex = 0; }
    document.getElementById('ctl00_cpMRPortal_hd_EditDocMgmt').value = '';

    return false;
}



//Created By : Bharat Patel
//Created On : 19-Mar-2012
//Reason: Check the name(textbox) in the grid that is exists or not.

function validateName(grid, txt, msg, cellNo, cIsCountry, cIsdivision, wait) {

    var Grid_Table = document.getElementById(grid);
    var str;
    var row;
    var country = '';
    var ddlCountry = '';
    var title = $("#spanValidate").text().trim().toUpperCase();
    var documentType = '';
    var division = '';
    var ddlDivision = '';
    var rblDocumentType = '';
    var docid = '';
    var grdDocId = '';
    var brandId = '';
    var gridBrandId = '';

    if (checkSpecialChar(document.getElementById(txt), msg) == false)
        return false;

    if (cIsCountry == undefined) {
        for (row = 1; row < Grid_Table.rows.length; row++) {
            if (Grid_Table.rows[row].cells[cellNo] != undefined) {
                str = Grid_Table.rows[row].cells[cellNo].innerText || Grid_Table.rows[row].cells[cellNo].textContent;
                if ((title == '' || title != document.getElementById(txt).value.trim().toUpperCase()) && str.trim().toUpperCase() == document.getElementById(txt).value.trim().toUpperCase()) {
                    alert(msg + ' with this name already exists.\n Please save with another name.');
                    document.getElementById(txt).value = '';
                    document.getElementById(txt).focus();
                    return false;
                }
            }
        }
    }
    else if (cIsdivision == 'Y') {
        if (title != '')
            docid = title;

        rblDocumentType = $('#ctl00_cpMRPortal_rdDocType input[type="radio"][checked="checked"]').next().text().trim().toUpperCase();
        ddlCountry = $('#ctl00_cpMRPortal_ddlCountry option:selected').text();
        ddlDivision = $('#ctl00_cpMRPortal_ddlDivision option:selected').text();

        for (row = 1; row < Grid_Table.rows.length; row++) {
            str = Grid_Table.rows[row].cells[cellNo].innerText || Grid_Table.rows[row].cells[cellNo].textContent;
            documentType = Grid_Table.rows[row].cells[2].innerText || Grid_Table.rows[row].cells[2].textContent;
            country = Grid_Table.rows[row].cells[3].innerText || Grid_Table.rows[row].cells[3].textContent;
            division = Grid_Table.rows[row].cells[4].innerText || Grid_Table.rows[row].cells[4].textContent;
            grdDocId = Grid_Table.rows[row].cells[11].innerText || Grid_Table.rows[row].cells[11].textContent;

            if (documentType.trim().toUpperCase() == rblDocumentType && country.trim().toUpperCase() == ddlCountry.trim().toUpperCase() && division.trim().toUpperCase() == ddlDivision.trim().toUpperCase() && str.trim().toUpperCase() == document.getElementById(txt).value.trim().toUpperCase() && (docid != grdDocId || docid == '')) {

                //if ((title == '' || title != document.getElementById(txt).value.trim().toUpperCase()) && ) {

                alert(msg + ' with this name already exists for this division.\n Please save with another name.');
                document.getElementById(txt).value = '';
                document.getElementById(txt).focus();
                return false;
                //}
            }
        }
        // onUpdating();
    }

    else {
        brandId = document.getElementById('ctl00_cpMRPortal_hdnBrandId').value.trim().toUpperCase();
        for (row = 1; row < Grid_Table.rows.length; row++) {
            str = Grid_Table.rows[row].cells[cellNo].innerText || Grid_Table.rows[row].cells[cellNo].textContent;
            country = Grid_Table.rows[row].cells[2].innerText || Grid_Table.rows[row].cells[2].textContent;
            ddlCountry = $('#ctl00_cpMRPortal_ddlCountry option:selected').text();
            gridBrandId = Grid_Table.rows[row].cells[10].innerText || Grid_Table.rows[row].cells[10].textContent;

            if ((brandId == '' || brandId != gridBrandId) && str.trim().toUpperCase() == document.getElementById(txt).value.trim().toUpperCase() && country.trim().toUpperCase() == ddlCountry.trim().toUpperCase()) {

                //if (country.trim().toUpperCase() == ddlCountry.trim().toUpperCase()) {

                alert(msg + ' with this name already exists for this country.\n Please save with another name.');
                document.getElementById(txt).value = '';
                document.getElementById(txt).focus();
                return false;
                //}
            }
        }
        // onUpdating();
    }

    if (wait != undefined && wait.toUpperCase() == 'W')
        onUpdating();

    return true;
}

function ConfirmDelete(msg) {
    //    var elem = $("<div>");
    //    elem.appendTo('body');
    //    return $.confirm({
    //        'title': 'Delete Confirmation',
    //        'message': msg,
    //        'buttons': {
    //            'Yes': {
    //                'class': 'blue',
    //                'action': function () {
    //                    elem.remove();
    //                    return true;
    //                }
    //            },
    //            'No': {
    //                'class': 'gray',
    //                'action': function () {
    //                    elem.remove();
    //                    return false;
    //                } // Nothing to do in this case. You can as well omit the action property.
    //            }
    //        }
    //    });

    if (confirm(msg)) {
        return true;
    }
    return false;
}
function validateNumber(e) {
    var value = 0;
    if (e.shiftKey == true)
        return false;
    value = getAsciival(e);
    if ((value > 47 && value < 58) || (value == 8) || (value == 190) || (value > 95 && value < 106) || (value == 110) || (value == 9) || (value == 37) || (value == 39) || (value == 46))
        return true;
    else
        return false;
}


function getAsciival(e) {
    var browser = navigator.appName;
    var value = 0;
    if (browser == 'Microsoft Internet Explorer')
        value = e.keyCode;
    else
        value = e.which;
    return value;
}


//Created By : Bharat Patel
//Created On : 19-Mar-2012
//Reason: Login in owa.

function GetOwaCredential() {

    if ($(parent.document).find("#owaLogin").attr("data-login").toUpperCase() == 'A') {
        $.ajax({
            type: "POST",
            url: "MRPortal.asmx/GetOwaUserMst",
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: '{}',
            error: function (ex) {
                //console.log(ex);

            },
            success: function (data) {

                data = jQuery.parseJSON(data.d);
                if (data != null) {
                    //                document.getElementById('username').value = data[0].vUserName;
                    //                document.getElementById('password').value = data[0].vPassword;                   
                    RedirectToOwa("https://webmail.invida.com", data[0].vDomainName, data[0].vUserName, data[0].vPassword);
                }
                return true;
            }
        });
    }

}

function SaveOwaCredential() {

    var content = new Object();
    content.owaUserName = document.getElementById('username').value;
    content.password = document.getElementById('password').value;
    content.DomainName = 'Invida';

    var memberfilter = new Array();
    memberfilter[0] = "owaUserName";
    memberfilter[1] = "password";
    memberfilter[2] = "DomainName";
    var jsonText = JSON.stringify(content, memberfilter, "\t");

    $.ajax({
        type: "POST",
        url: "MRPortal.asmx/SaveOwaUserMst",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: jsonText,
        error: function (ex) {
            console.log(ex);

        },
        success: function (data) {

            data = jQuery.parseJSON(data.d);
            if (data != null) {
                //                document.getElementById('username').value = data[0].vUserName;
                //                document.getElementById('password').value = data[0].vPassword;
                RedirectToOwa("https://webmail.invida.com", data[0].vDomainName, data[0].vUserName, data[0].vPassword);
            }
            return true;
        }
    });

    return true;
}
function replaceToUpper(key, value) {
    return value.toString().toUpperCase();
}
function RedirectToOwa(server, domain, username, password) {


    var url = server + "/owa/auth/owaauth.dll";
    var p = { destination: server + "/owa", flags: '0', forcedownlevel: '0', trusted: '0', isutf8: '1', username: domain + '\\' + username, password: password };


    var myForm = document.createElement("form");
    myForm.method = "post";
    myForm.action = url;

    for (var k in p) {

        var myInput = document.createElement("input");
        myInput.setAttribute("name", k);
        myInput.setAttribute("value", p[k]);
        myForm.appendChild(myInput);
    }

    document.body.appendChild(myForm);
    myForm.submit();
    document.body.removeChild(myForm);
}

//Added by nidhi for delete Brand functionality
function ConfirmDeleteBrand(e, NoofRows, index) {
    //var ImgViewchild = '#ctl00_cpMRPortal_gvBrandMst_ctl0' + (index + 2) + '_ImgViewChild'
    // $(ImgViewchild).attr('src', '../images/SalesPortal/expand.png');
    if (document.getElementById(e).disabled === false) {
        if (NoofRows === 1) {
            //if 1 row exists
            return confirm('This brand is attached to one division. Are you sure you want to delete?');
        }
        else if (NoofRows > 1) {
            //if more than 1 row exists
            if (confirm('This brand is attached to more than one division. Please select division for which to delete this brand.')) {
                //alert($(ImgViewchild).attr('src').toUpperCase().search('EXPAND'));
                // if ($(ImgViewchild).attr('src').toUpperCase().search('EXPAND') != -1) {
                document.getElementById('ctl00_cpMRPortal_gvBrandMst_ctl0' + (index + 2) + '_ImgViewChild').click();
                // }
                $('#ctl00_cpMRPortal_gvBrandMst_ctl0' + (index + 2) + '_grd_ChdWidget tr').each(function () {
                    $('th:first, td:first', this).show();
                });
                document.getElementById('ctl00_cpMRPortal_gvBrandMst_ctl0' + (index + 2) + '_btnDelete').style.display = '';
                document.getElementById(e).disabled = true;
                return false;
            }
            else { return false; }
        }
    }
}
function ValidateDivCheck(e) {
    var index = e.replace('ctl00_cpMRPortal_gvBrandMst_ctl0', '').replace('_btnDelete', '');
    if ($('#ctl00_cpMRPortal_gvBrandMst_ctl0' + index + '_grd_ChdWidget input[type=checkbox]:checked').length <= 0) {
        alert('Please select at least one division.');
        return false;
    }
    BrandResetControl();
    return true;
}

function SelectAll(e, Chk) {
    var index;
    if (Chk == 'H') {
        //If Header checkbox
        index = e.replace('ctl00_cpMRPortal_gvBrandMst_ctl0', '').replace('_grd_ChdWidget_ctl01_chkAllchilddivision', '')
        if ($('#ctl00_cpMRPortal_gvBrandMst_ctl0' + index + '_grd_ChdWidget_ctl01_chkAllchilddivision').attr('checked') == "checked") {
            $('#ctl00_cpMRPortal_gvBrandMst_ctl0' + index + '_grd_ChdWidget input[type="checkbox"]').each(function () {
                this.checked = true;
            });
        }
        else {
            $('#ctl00_cpMRPortal_gvBrandMst_ctl0' + index + '_grd_ChdWidget input[type="checkbox"]').each(function () {
                this.checked = false;
            });
        }
    }
    else {
        index = e.replace('ctl00_cpMRPortal_gvBrandMst_ctl0', '').replace('', '').substr(0, 1);
        var IsAllChecked = true;
        $('#ctl00_cpMRPortal_gvBrandMst_ctl0' + index + '_grd_ChdWidget input[type="checkbox"]').each(function () {
            if (this.id != 'ctl00_cpMRPortal_gvBrandMst_ctl0' + index + '_grd_ChdWidget_ctl01_chkAllchilddivision') {
                if (this.checked == false) {
                    IsAllChecked = false;
                }
            }
        });

        if (IsAllChecked == true) {
            document.getElementById('ctl00_cpMRPortal_gvBrandMst_ctl0' + index + '_grd_ChdWidget_ctl01_chkAllchilddivision').checked = true;
        }
    }
    return true;
}



function saveDocStatus(dataBankId) {

    var content = new Object();
    content.dataBankId = dataBankId;

    var jsonText = JSON.stringify(content);

    $.ajax({
        type: "POST",
        url: "MRPortal.asmx/saveDocStatus",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: jsonText,
        error: function (ex) {
            //console.log(ex);
            return false;
        },
        success: function (data) {
            return true;
        }
    });
}

function BrandResetControl() {
    document.getElementById('ctl00_cpMRPortal_txtBrandName').value = '';
    document.getElementById('ctl00_cpMRPortal_ddlCountry').disabled = false;
    //sdocument.getElementyId('ctl00_cpMRPortal_ddlCountry').selectedIndex = 0;
    document.getElementById('ctl00_cpMRPortal_btnSaveBrand').value = 'Save';
    document.getElementById('ctl00_cpMRPortal_chkAllDivisions').checked = false;
    document.getElementById('ctl00_cpMRPortal_fuBrandLogo').value = '';
    document.getElementById('ctl00_cpMRPortal_ImgEditBrand').src = '';
    document.getElementById('ctl00_cpMRPortal_ImgEditBrand').style.display = 'none';
    document.getElementById('ctl00_cpMRPortal_hdnBrandId').value = '0';
    document.getElementById('ctl00_cpMRPortal_hdnBrandName').value = '';

    $('#ctl00_cpMRPortal_chkDivision input[type="checkbox"]').each(function () {
        this.checked = false;
    });

    ClearValidateNameEditTime();
    return false;
}

function openNewWindows(img) {
    window.open(img.src);
    return false;
}
function validateNameEditTime(txtName) {    
    $('#spanValidate').text(txtName);
    if ($('#ctl00_cpMRPortal_rblDefault_1:checked').length > 0)
        $get('trCountry').style.display = 'none';
    else if ($('#ctl00_cpMRPortal_rblDefault_0:checked').length > 0)
        $get('trCountry').style.display = '';
}
function ClearValidateNameEditTime() {

    $('#spanValidate').text("");
}

// Check the textbox does not contain the following characters \ / : * ? < > " | ;
// <param type='element' code='document.getElementById('<%= txtReportSetPDFName.ClientID %>')' />
// Example: checkFileName(document.getElementById('<%= txtReportSetPDFName.ClientID %>'));
function checkSpecialChar(ele, txtName) {
    var fileNameRegEx = /^[^\\\/\:\*\?\""\''\<\>\|\;]+$/;
    if (ele.value != '' && !fileNameRegEx.test(ele.value)) {
        alert('A ' + txtName + ' Name cannot contain any of the following characters:\n\t \\ / : * ? < > " | ; \'');
        ele.focus();
        return false;
    }
    return true;
}
