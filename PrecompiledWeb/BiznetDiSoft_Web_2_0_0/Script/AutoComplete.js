// JScript File
function ProjectClientShowing(behaviorId, textBox) {
    debugger;
    var txt = $find(behaviorId);

    var target = txt.get_completionList();
    var items = target.childNodes;

    var searchText = textBox.value;

    for (var i = 0; i < items.length; i++) {
        var child = items[i];
        var value = child._value;
        var startIndex;
        var len;
        var strValue;
        var tempValue;

        startIndex = value.toUpperCase().indexOf(searchText.toUpperCase());
        len = searchText.length;
        strValue = value.substring(0, startIndex);

        strValue = strValue + '<span style="color:red"><b>' + value.substring(startIndex, parseInt(strValue.length) + parseInt(len)) + '</b></span>';
        tempValue = strValue;

        tempValue = tempValue.replace('<span style="color:red"><b>', '');
        tempValue = tempValue.replace('</b></span>', '');

        strValue += value.substring(tempValue.length);

        strValue = strValue.replace('\'', '');

        child.style.width = '100%';

        var arrstrvalue = strValue.split('#');
        if (arrstrvalue.length == 2) {
            //child.innerHTML = '<span style="font-family:Calibri;font-size:11pt;white-space:normal;"><hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + '[' + arrstrvalue[1] + ']' + ' ' + arrstrvalue[2] + '</span>';
            child.innerHTML = '<hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + arrstrvalue[1];

        }
        else if (arrstrvalue.length == 3) {
            //child.innerHTML = '<span style="font-family:Calibri;font-size:11pt;white-space:normal;"><hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + '[' + arrstrvalue[1] + ']' + ' ' + arrstrvalue[2] + '</span>';
            child.innerHTML = '<hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + '[' + arrstrvalue[1] + ']' + ' ' + arrstrvalue[2];

        }
        else if (arrstrvalue.length == 4) {
            //child.innerHTML = '<span style="font-family:Calibri;font-size:11pt;white-space:normal;"><hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + '[' + arrstrvalue[1] + ']' + ' ' + arrstrvalue[2] + '</span>';
            child.innerHTML = '<hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + '[' + arrstrvalue[1] + ']' + ' ' + arrstrvalue[2];

        }

        else if (arrstrvalue.length == 5) {
            //child.innerHTML = '<span style="font-family:Calibri;font-size:11pt;white-space:normal;"><hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + '[' + arrstrvalue[1] + ']' + arrstrvalue[2] + '-' 
            //                + arrstrvalue[3] + '</span>' + '-' + arrstrvalue[4] + '</span>';
            child.innerHTML = '<hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + '[' + arrstrvalue[1] + ']' + arrstrvalue[2] + '-'
                + arrstrvalue[3] + '-' + arrstrvalue[4];
        }
        else if (arrstrvalue.length > 5) {
            //child.innerHTML = '<span style="font-family:Calibri;font-size:11pt;white-space:normal;"><hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + '[' + arrstrvalue[1] + ']' + arrstrvalue[2] + '-' 
            //              + arrstrvalue[3] + '-' + arrstrvalue[4] + '</span>' + '[' + arrstrvalue[5] + ']' + '</span>';
            child.innerHTML = '<hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + '[' + arrstrvalue[1] + ']' + arrstrvalue[2] + '-'
                          + arrstrvalue[3] + '-' + arrstrvalue[4] + '[' + arrstrvalue[5] + ']';
        }

    }
}
function ProjectClientShowingSchema(behaviorId, textBox) {
    var txt = $find(behaviorId);

    var target = txt.get_completionList();
    var items = target.childNodes;

    var searchText = textBox.value;

    for (var i = 0; i < items.length; i++) {
        var child = items[i];
        var value = child._value;
        var startIndex;
        var len;
        var strValue;
        var tempValue;

        startIndex = value.toUpperCase().indexOf(searchText.toUpperCase());
        len = searchText.length;
        strValue = value.substring(0, startIndex);

        strValue = strValue + '<span style="color:red"><b>' + value.substring(startIndex, parseInt(strValue.length) + parseInt(len)) + '</b></span>';
        tempValue = strValue;

        tempValue = tempValue.replace('<span style="color:red"><b>', '');
        tempValue = tempValue.replace('</b></span>', '');

        strValue += value.substring(tempValue.length);

        strValue = strValue.replace('\'', '');

        child.style.width = '100%';

        var arrstrvalue = strValue.split('#');

        //child.innerHTML = '<span style="font-family:Calibri;font-size:11pt;white-space:normal;"><hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + '[' + arrstrvalue[1] + ']' + ' ' + arrstrvalue[2] + '</span>';
        child.innerHTML = '<hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + '[' + arrstrvalue[1] + ']' + ' ' + arrstrvalue[2];

    }
}

function ProjectOnItemSelectedSchema(val, txt, hFld, hfSchemaId, hFArchivedFlag, hFArchiveLabData, btn) {
    var strvalue = val;
    strvalue = strvalue.replace('\'', '');
    var arrStrValue = strvalue.split('#');
    txt.value = '[' + arrStrValue[1] + ']' + arrStrValue[2];

    hFld.value = arrStrValue[0];
    hfSchemaId.value = arrStrValue[3];
    hFArchivedFlag.value = arrStrValue[4];
    hFArchiveLabData.value = arrStrValue[5];

    if (!(btn == null || btn == 'undefined')) {
        btn.click();
    }

}
function ProjectClientShowingCRFVersion(behaviorId, textBox) {

    var txt = $find(behaviorId);

    var target = txt.get_completionList();
    var items = target.childNodes;

    var searchText = textBox.value;

    for (var i = 0; i < items.length; i++) {
        var child = items[i];
        var value = child._value;
        var startIndex;
        var len;
        var strValue;
        var tempValue;



        startIndex = value.toUpperCase().indexOf(searchText.toUpperCase());
        len = searchText.length;
        strValue = value.substring(0, startIndex);


        strValue = strValue + '<span style="color:red"><b>' + value.substring(startIndex, parseInt(strValue.length) + parseInt(len)) + '</b></span>';
        tempValue = strValue;

        tempValue = tempValue.replace('<span style="color:red"><b>', '');
        tempValue = tempValue.replace('</b></span>', '');


        strValue += value.substring(tempValue.length);

        strValue = strValue.replace('\'', '');

        child.style.width = '100%';

        var arrstrvalue = strValue.split('#');


        //child.innerHTML = '<span style="font-family:Calibri;font-size:11pt;white-space:normal;"><hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + '[' + arrstrvalue[1] + ']' + ' ' + arrstrvalue[2] + '</span>';
        child.innerHTML = '<hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + '[' + arrstrvalue[1] + ']' + ' ' + arrstrvalue[2];

    }
}

function ProjectOnItemSelectedCRFVersion(val, txt, hFld, hfreezestatus, hnversionNo, hfreezedDate, Sponsor, btn) {
    var strvalue = val;
    strvalue = strvalue.replace('\'', '');
    var arrStrValue = strvalue.split('#');
    txt.value = '[' + arrStrValue[1] + ']' + arrStrValue[2];
    hFld.value = arrStrValue[0];
    hnversionNo.value = arrStrValue[3];
    hfreezestatus.value = arrStrValue[4];
    hfreezedDate.value = arrStrValue[5];
    Sponsor.value = arrStrValue[6];

    if (!(btn == null || btn == 'undefined')) {

        btn.click();
    }

}

function ProjectOnItemSelected(val, txt, hFld, btn) {
    var strvalue = val;
    strvalue = strvalue.replace('\'', '');
    var arrStrValue = strvalue.split('#');

    if (arrStrValue.length == 3) {
        txt.value = '[' + arrStrValue[1] + '] ' + arrStrValue[2];

        hFld.value = arrStrValue[0];
    }
    else if (arrStrValue.length == 4) {
        if (arrStrValue[4] == undefined) {

            txt.value = '[' + arrStrValue[1] + '] ' + arrStrValue[2] + '-'
                                  + arrStrValue[3];

        }
        else {
            txt.value = '[' + arrStrValue[1] + '] ' + arrStrValue[2] + '-'
                          + arrStrValue[3] + '-' + arrStrValue[4];

        }

        hFld.value = arrStrValue[0];

    }
    else {
        txt.value = '[' + arrStrValue[1] + '] ' + arrStrValue[2] + '-'
                              + arrStrValue[3] + '-' + arrStrValue[4] + '-' + arrStrValue[5];

        hFld.value = arrStrValue[0];

    }
    if (!(btn == null || btn == 'undefined')) {
        btn.click();
    }

}

function ProjectOnItemSelectedForMsrLog(val, txt, hFld, btn, clientName, Projectno) {
    var strvalue = val;
    strvalue = strvalue.replace('\'', '');
    var arrStrValue = strvalue.split('#');



    txt.value = '[' + arrStrValue[1] + ']' + arrStrValue[2];
    hFld.value = arrStrValue[0];
    clientName.value = arrStrValue[3];
    Projectno.value = arrStrValue[1];

    if (!(btn == null || btn == 'undefined')) {
        btn.click();
    }

}

function SubjectDashboardClientShowing(behaviorId, textBox) {
    var txt = $find(behaviorId);

    var target = txt.get_completionList();
    var items = target.childNodes;
    var searchText = textBox.value;

    for (var i = 0; i < items.length; i++) {
        var child = items[i];
        var value = child._value;
        var startIndex;
        var len;
        var strValue;
        var tempValue;

        startIndex = value.toUpperCase().indexOf(searchText.toUpperCase());
        len = searchText.length;
        strValue = value.substring(0, startIndex);

        strValue = strValue + '<span style="color:red"><b>' + value.substring(startIndex, parseInt(strValue.length) + parseInt(len)) + '</b></span>';
        tempValue = strValue;


        tempValue = tempValue.replace('<span style="color:red"><b>', '');
        tempValue = tempValue.replace('</b></span>', '');

        strValue += value.substring(tempValue.length);
        strValue = strValue.replace('\'', '');

        var arrstrvalue = strValue.split('#');
        if (strValue.split('#').length == 3) {
            //child.innerHTML = '<span style="font-family:Calibri;font-size:11pt;white-space:normal;"><hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + strValue.split('#')[1] + ' (' + strValue.split('#')[0] + ')' + ' (' + strValue.split('#')[2] + ')' + '</span>';
            child.innerHTML = '<hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + strValue.split('#')[1];
        }
        else {
            //child.innerHTML = '<span style="font-family:Calibri;font-size:11pt;white-space:normal;"><hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + strValue.split('#')[1] + ' (' + strValue.split('#')[0] + ')' + '</span>';
            child.innerHTML = '<hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + strValue.split('#')[1] + '</span>';
        }
    }
}

function SubjectClientShowing(behaviorId, textBox) {
    var txt = $find(behaviorId);

    var target = txt.get_completionList();
    var items = target.childNodes;
    var searchText = textBox.value;

    for (var i = 0; i < items.length; i++) {
        var child = items[i];
        var value = child._value;
        var startIndex;
        var len;
        var strValue;
        var tempValue;

        startIndex = value.toUpperCase().indexOf(searchText.toUpperCase());
        len = searchText.length;
        strValue = value.substring(0, startIndex);

        strValue = strValue + '<span style="color:red"><b>' + value.substring(startIndex, parseInt(strValue.length) + parseInt(len)) + '</b></span>';
        tempValue = strValue;


        tempValue = tempValue.replace('<span style="color:red"><b>', '');
        tempValue = tempValue.replace('</b></span>', '');

        strValue += value.substring(tempValue.length);
        strValue = strValue.replace('\'', '');

        var arrstrvalue = strValue.split('#');
        if (strValue.split('#').length == 3) {
            //child.innerHTML = '<span style="font-family:Calibri;font-size:11pt;white-space:normal;"><hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + strValue.split('#')[1] + ' (' + strValue.split('#')[0] + ')' + ' (' + strValue.split('#')[2] + ')' + '</span>';
            child.innerHTML = '<hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + strValue.split('#')[1] + ' (' + strValue.split('#')[0] + ')' + ' (' + strValue.split('#')[2] + ')';
        }
        else {
            //child.innerHTML = '<span style="font-family:Calibri;font-size:11pt;white-space:normal;"><hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + strValue.split('#')[1] + ' (' + strValue.split('#')[0] + ')' + '</span>';
            child.innerHTML = '<hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + strValue.split('#')[1] + ' (' + strValue.split('#')[0] + ')' + '</span>';
        }
    }
}

function SubjectClientShowing_OnlyID(behaviorId, textBox) {
    var txt = $find(behaviorId);

    var target = txt.get_completionList();
    var items = target.childNodes;
    var searchText = textBox.value;

    for (var i = 0; i < items.length; i++) {
        var child = items[i];
        var value = child._value;
        var startIndex;
        var len;
        var strValue;
        var tempValue;

        startIndex = value.toUpperCase().indexOf(searchText.toUpperCase());
        len = searchText.length;
        strValue = value.substring(0, startIndex);

        //strValue = strValue + '<b>' + value.substring(startIndex,parseInt(strValue.length) + parseInt(len)) + '</b>';
        strValue = strValue + '<span style="color:red"><b>' + value.substring(startIndex, parseInt(strValue.length) + parseInt(len)) + '</b></span>';
        tempValue = strValue;

        tempValue = tempValue.replace('<span style="color:red"><b>', '');
        tempValue = tempValue.replace('</b></span>', '');

        strValue += value.substring(tempValue.length);
        strValue = strValue.replace('\'', '');

        //child.innerHTML = '<span style="font-family:Calibri;font-size:11pt;white-space:normal;"><hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + strValue.split('#')[0] + '</span>';
        child.innerHTML = '<hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + strValue.split('#')[0];

    }
}

function SubjectOnItemSelectedDashboard(val, txt, hFld, btn) {
    var strvalue = val;
    strvalue = strvalue.replace('\'', '');
    var arrStrValue = strvalue.split('#');
    if (arrStrValue.length == 3) {
        txt.value = arrStrValue[1];
    }
    else {
        txt.value = arrStrValue[1];
    }
    hFld.value = arrStrValue[0];

    if (!(btn == null || btn == 'undefined')) {
        btn.click();
    }
}

function SubjectOnItemSelected(val, txt, hFld, btn) {
    var strvalue = val;
    strvalue = strvalue.replace('\'', '');
    var arrStrValue = strvalue.split('#');
    if (arrStrValue.length == 3) {
        txt.value = arrStrValue[1] + ' (' + arrStrValue[0] + ')' + ' (' + arrStrValue[2] + ')';
    }
    else {
        txt.value = arrStrValue[1] + ' (' + arrStrValue[0] + ')';
    }
    hFld.value = arrStrValue[0];

    if (!(btn == null || btn == 'undefined')) {
        btn.click();
    }
}

function ADRClientShowing(behaviorId, textBox) {
    var txt = $find(behaviorId);

    var target = txt.get_completionList();
    var items = target.childNodes;
    var searchText = textBox.value;

    for (var i = 0; i < items.length; i++) {
        var child = items[i];
        var value = child._value;
        var startIndex;
        var len;
        var strValue;
        var tempValue;

        startIndex = value.toUpperCase().indexOf(searchText.toUpperCase());
        len = searchText.length;
        strValue = value.substring(0, startIndex);

        strValue = strValue + '<b>' + value.substring(startIndex, parseInt(strValue.length) + parseInt(len)) + '</b>';
        tempValue = strValue;

        tempValue = tempValue.replace('<b>', '');
        tempValue = tempValue.replace('</b>', '');

        strValue += value.substring(tempValue.length);
        strValue = strValue.replace('\'', '');

        var arrstrvalue = strValue.split('#');
        //child.innerHTML = '<span style="font-family:Calibri;font-size:11pt;white-space:normal;"><hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + '(' + strValue.split('#')[0] + ')' + ' ' + strValue.split('#')[1] + '</span>';
        child.innerHTML = '<hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + '(' + strValue.split('#')[0] + ')' + ' ' + strValue.split('#')[1];
    }
}


function ADROnItemSelected(val, txt, hFld, btn) {
    var strvalue = val;
    strvalue = strvalue.replace('\'', '');
    var arrStrValue = strvalue.split('#');
    txt.value = arrStrValue[1];
    hFld.value = arrStrValue[0];

    if (!(btn == null || btn == 'undefined')) {
        btn.click();
    }
}


function MedexShowing(behaviorId, textBox) {

    var txt = $find(behaviorId);

    var target = txt.get_completionList();
    var items = target.childNodes;
    var searchText = textBox.value;

    for (var i = 0; i < items.length; i++) {
        var child = items[i];
        var value = child._value;
        var startIndex;
        var len;
        var strValue;
        var tempValue;

        startIndex = value.toUpperCase().indexOf(searchText.toUpperCase());
        len = searchText.length;
        strValue = value.substring(0, startIndex);

        strValue = strValue + '<span style="color:red"><b>' + value.substring(startIndex, parseInt(strValue.length) + parseInt(len)) + '</b></span>';
        tempValue = strValue;

        tempValue = tempValue.replace('<span style="color:red"><b>', '');
        tempValue = tempValue.replace('</b></span>', '');

        strValue += value.substring(tempValue.length);
        strValue = strValue.replace('\'', '');

        var arrstrvalue = strValue.split('#');
        //child.innerHTML = '<span style="font-family:Calibri;font-size:11pt;white-space:normal;"><hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + '(' + strValue.split('#')[0] + ')' + ' ' + strValue.split('#')[1] + '</span>';
        child.innerHTML = '<hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + '(' + strValue.split('#')[0] + ')' + ' ' + strValue.split('#')[1];
    }
}


function OnMedexSelected(val, txt, hFld, btn) {
    var strvalue = val;
    strvalue = strvalue.replace('\'', '');
    var arrStrValue = strvalue.split('#');
    txt.value = arrStrValue[1];
    hFld.value = arrStrValue[0];

    if (!(btn == null || btn == 'undefined')) {
        btn.click();
    }
}

function MedexTemplateClientShowing(behaviorId, textBox) {
    var txt = $find(behaviorId);

    var target = txt.get_completionList();
    var items = target.childNodes;
    var searchText = textBox.value;

    for (var i = 0; i < items.length; i++) {
        var child = items[i];
        var value = child._value;
        var startIndex;
        var len;
        var strValue;
        var tempValue;

        startIndex = value.toUpperCase().indexOf(searchText.toUpperCase());
        len = searchText.length;
        strValue = value.substring(0, startIndex);

        strValue = strValue + '<b>' + value.substring(startIndex, parseInt(strValue.length) + parseInt(len)) + '</b>';
        tempValue = strValue;

        tempValue = tempValue.replace('<b>', '');
        tempValue = tempValue.replace('</b>', '');

        strValue += value.substring(tempValue.length);
        strValue = strValue.replace('\'', '');

        var arrstrvalue = strValue.split('#');
        //child.innerHTML = '<span style="font-family:Calibri;font-size:11pt;white-space:normal;"><hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + strValue.split('#')[1] + ' (' + strValue.split('#')[0] + ')' + '</span>';
        child.innerHTML = '<hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + strValue.split('#')[1] + ' (' + strValue.split('#')[0] + ')';
    }
}


function MedexTemplateOnItemSelected(val, txt, hFld, btn) {
    var strvalue = val;
    strvalue = strvalue.replace('\'', '');
    var arrStrValue = strvalue.split('#');
    txt.value = arrStrValue[1];
    hFld.value = arrStrValue[0];

    if (!(btn == null || btn == 'undefined')) {
        btn.click();
    }
}

//Added by Chandresh Vanker for dynamic parameters on 14-Sept-2010

function ProjectClientShowingDynamic(behaviorId, textBox) {
    var txt = $find(behaviorId);

    var target = txt.get_completionList();
    var items = target.childNodes;

    var searchText = textBox.value;

    for (var i = 0; i < items.length; i++) {
        var child = items[i];
        var value = child._value;
        var startIndex;
        var len;
        var strValue;
        var tempValue;


        startIndex = value.toUpperCase().indexOf(searchText.toUpperCase());
        len = searchText.length;
        strValue = value.substring(0, startIndex);


        strValue = strValue + '<span style="color:red"><b>' + value.substring(startIndex, parseInt(strValue.length) + parseInt(len)) + '</b></span>';
        tempValue = strValue;

        tempValue = tempValue.replace('<span style="color:red"><b>', '');
        tempValue = tempValue.replace('</b></span>', '');


        strValue += value.substring(tempValue.length);

        strValue = strValue.replace('\'', '');

        child.style.width = '100%';

        var arrstrvalue = strValue.split('#');

        var TxtToDisplay = '';
        TxtToDisplay = '[' + arrstrvalue[1] + ']' + arrstrvalue[2];
        for (var cnt = 3; cnt < arrstrvalue.length - 1; cnt++) {
            TxtToDisplay += '-' + arrstrvalue[cnt];
        }
        TxtToDisplay += '</span>' + '[' + arrstrvalue[arrstrvalue.length - 1] + ']';
        //child.innerHTML = '<span style="font-family:Calibri;font-size:11pt;white-space:normal;"><hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + TxtToDisplay + '</span>';
        child.innerHTML = '<hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + TxtToDisplay;
    }
}


function ProjectOnItemSelectedDynamic(val, txt, hFld, btn) {
    var strvalue = val;
    strvalue = strvalue.replace('\'', '');
    var arrStrValue = strvalue.split('#');

    var TxtToDisplay = '';
    TxtToDisplay = '[' + arrStrValue[1] + ']' + arrStrValue[2];
    for (var cnt = 3; cnt < arrStrValue.length; cnt++) {
        TxtToDisplay += '-' + arrStrValue[cnt];
    }
    txt.value = TxtToDisplay;

    hFld.value = arrStrValue[0];


    if (!(btn == null || btn == 'undefined')) {
        btn.click();
    }

}
function ProjectOnItemSelectedForPeriod(val, txt, hFld, hFParentId, btn) {
    var strvalue = val;
    strvalue = strvalue.replace('\'', '');
    var arrStrValue = strvalue.split('#');


    txt.value = '[' + arrStrValue[1] + ']' + arrStrValue[2];

    hFld.value = arrStrValue[0];
    hFParentId.value = arrStrValue[3];

    if (!(btn == null || btn == 'undefined')) {
        btn.click();
    }

}


function GetOperationSelected(val, txt, hFld) {
    var strvalue = val;
    strvalue = strvalue.replace('\'', '');
    var arrStrValue = strvalue.split('#');
    txt.value = arrStrValue[1]
    hFld.value = arrStrValue[0];
}

function OperationClientShowing(behaviorId, textBox) {

    var txt = $find(behaviorId);

    var target = txt.get_completionList();
    var items = target.childNodes;

    var searchText = textBox.value;

    for (var i = 0; i < items.length; i++) {
        var child = items[i];
        var value = child._value;
        var startIndex;
        var len;
        var strValue;
        var tempValue;
        startIndex = value.toUpperCase().indexOf(searchText.toUpperCase());
        len = searchText.length;
        strValue = value.substring(0, startIndex);


        //strValue = strValue + '<span style="color:red"><b>' + value.substring(startIndex, parseInt(strValue.length) + parseInt(len)) + '</b></span>';
        //tempValue = strValue;

        //tempValue = tempValue.replace('<span style="color:red"><b>', '');
        //tempValue = tempValue.replace('</b></span>', '');

        strValue = strValue + value.substring(startIndex, parseInt(strValue.length) + parseInt(len));
        tempValue = strValue;

        tempValue = tempValue.replace('<span style="color:red"><b>', '');
        tempValue = tempValue.replace('</b></span>', '');


        strValue += value.substring(tempValue.length);

        strValue = strValue.replace('\'', '');

        child.style.width = '100%';

        var arrstrvalue = strValue.split('#');

        //child.innerHTML = '<span style="font-family:Calibri;font-size:11pt;white-space:normal;"><hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + arrstrvalue[1] + '</span>';
        child.innerHTML = '<hr style="margin:0px 0px 0px 0px !important;padding:0px !important">' + arrstrvalue[1];
    }
}

//****************************************************
