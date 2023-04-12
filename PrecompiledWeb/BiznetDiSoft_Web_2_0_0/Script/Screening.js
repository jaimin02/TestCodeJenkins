
var inyear;
window.onbeforeunload = function() {

    if (window.event.clientX < 0 || window.event.clientY < 0 || (event.altKey == true && event.keyCode == 0)) {


        parwin = window.opener;
        if (parwin == null && typeof (parwin) == 'undefined') {
            HandleLogOutEvent();
            return false;
        }
    }
}

var currTab;
function HandleLogOutEvent() {

    var nMedexScreenNo = '1';
    var hmed = document.getElementById('hMedExNo').value.toString();
    var hSubID = document.getElementById('HSubjectId').value.toString();
    if (!hmed == '') {
        nMedexScreenNo = hmed;
    }
    //    var btn=document.getElementById('btnDeleteScreenNo');
    //    btn.click();
    PageMethods.DeleteScreeningtmpTable(hmed, hSubID, function() { document.getElementById('HSubjectId').value = '', function(err) { msgalert(err); } })

}
function closewindow() {
    self.close();
}

//        var rotation = function (){
//        $("#ImgbtnShowHome").rotate({
//          angle:0, 
//          duration:8000,
//          animateTo:360, 
//          callback: rotation,
//          easing: function (x,t,b,c,d){        // t: current time, b: begInnIng value, c: change In value, d: duration
//              return c*(t/d)+b;
//          }
//        });
//       }

$(document).ready(function() {
    $("#HFWidth").val($(document).width());
    $("#HFHeight").val($(document).height());
    updateCountdown('load');
    $('.crfentrycontrol').change(updateCountdown);
    $('.crfentrycontrol').keyup(updateCountdown);
    $('.crfentrycontrol').keypress(textareakeypress);
    $('#txtPassword').bind('paste', function(e) {
        e.preventDefault();
    });
    // $('ImgbtnShowHome').fadeToggle('slow').fadeToggle('slow').fadeToggle('slow').fadeToggle('slow');
    ShowHideproject();
    //rotation();            
});




function ValidateTextbox(checktype, txt, msg, HighRange, LowRange) {
    var result;
    //alert(checktype);
    if (checktype != 0) {
        switch (checktype) {
            case 1:
                result = CheckInteger(txt.value);
                break;
            case 2:
                result = CheckDecimal(txt.value);
                break;
            case 3:
                result = CheckIntegerOrBlank(txt.value); //CheckIntegerOrBlank
                break;
            case 4:
                result = CheckDecimalOrBlank(txt.value);
                break;
            case 5:
                result = CheckAlphabet(txt.value);
                break;
            case 6:
                result = CheckAlphaNumeric(txt.value);
                break;
            default: break;          //alert("oh u have all rights ");
        }

        if (result == false) {
            txt.value = '';
            msgalert(msg);
        }
    }

    if (HighRange != 0 || LowRange != 0) {
        if (txt.value.toString().trim() != "") {
            if (txt.value > HighRange || txt.value < LowRange) {
                msgalert('Out Of Range! Range Must be Between ' + LowRange + ' to ' + HighRange);
            }
        }
    }

}

function ValidateTextboxNumeric(checktype, txt, msg, HighRange, LowRange, validation, length, NumericScale) {
    var value = txt.value.trim();
    var scaleForNumeric = value.toString().split(".");
    if (validation == 'NU') {
        if (scaleForNumeric.length > 2) {
            msgalert('Enter Data Not In Correct Format');
            return false;
        }
        if (scaleForNumeric[0].length > parseInt(length)) {
            msgalert('Out Of Maximum Length! Value length Must Less or Equal to  ' + parseInt(length) + '. Please Enter the value in Proper Range')
            return false;
        }
        if (typeof (scaleForNumeric[1]) != 'undefined') {
            if (scaleForNumeric[1].length > NumericScale) {
                msgalert('scale Not be greate than ' + NumericScale + '. Please Enter the value in Proper Range');
                return false;
            }
        }

        var decimalRegEx = /^[+|-]?\d*(\.\d+)?$/;
        if (!decimalRegEx.test(value)) {
            msgalert('Enter Value Not in Correct Format');
            return false;
        }
    }
    var result;
    //alert(checktype);
    if (checktype != 0) {
        switch (checktype) {
            case 1:
                result = CheckInteger(txt.value);
                break;
            case 2:
                result = CheckDecimal(txt.value);
                break;
            case 3:
                result = CheckIntegerOrBlank(txt.value); //CheckIntegerOrBlank
                break;
            case 4:
                result = CheckDecimalOrBlank(txt.value);
                break;
            case 5:
                result = CheckAlphabet(txt.value);
                break;
            case 6:
                result = CheckAlphaNumeric(txt.value);
                break;
            default: break;          //alert("oh u have all rights ");
        }

        if (result == false) {
            txt.value = '';
            msgalert(msg);
        }
    }

    if (HighRange != 0 || LowRange != 0) {
        if (txt.value.toString().trim() != "") {
            if (parseFloat(txt.value) > HighRange || parseFloat(txt.value) < LowRange) {
                msgalert('Out Of Range! Range Must be Between ' + LowRange + ' to ' + HighRange);
            }
        }
    }

}


function Next(NoneDivId) {

    var MedExGroupCode = document.getElementById('hfMedExGroupCode').value;
    var SexValue;
    var tblName = document.getElementById('hfMedExCodeForSex').value;
    if (MedExGroupCode != "" && tblName != "") {
        MedExGroupCode = MedExGroupCode.replace('BtnDiv', 'Div');
        var tblRdo = $get(tblName);
        var name = tblRdo.id;
        name = name.replace(/_/g, '$');
        var rdos = document.getElementsByName(name);
        var i;
        for (i = 0; i < rdos.length; i++) {
            if (rdos[i].checked && rdos[i].value == 'Male') {
                SexValue = 'Male';
            }
            else if (rdos[i].checked && rdos[i].value == 'Female') {
                SexValue = 'Female';
            }
        }

    }

    var arrDiv = NoneDivId.split(',');
    var isShow = false;
    for (i = 0; i < arrDiv.length; i++) {
        document.getElementById(arrDiv[i]).style.display = 'none';
        var disBtn = arrDiv[i].replace('Div', 'BtnDiv');

        //document.getElementById(disBtn).style.color='Brown';
        document.getElementById(disBtn).style.color = '#FFffff';
    }

    for (i = 0; i < arrDiv.length; i++) {
        if (isShow) {
            currTab = arrDiv[i];
            //alert(currTab + ' - ' + arrDiv[i+1] + ' - ' + MedExGroupCode + ' - ' + SexValue);
            if (currTab == MedExGroupCode && SexValue == 'Male') {
                currTab = arrDiv[i + 1];
            }
            isShow = false;
            break;
        }

        if (arrDiv[i].toLowerCase() == currTab.toLowerCase()) {
            isShow = true;
        }
    }
    var currBtn = currTab.replace('Div', 'BtnDiv');
    document.getElementById(currTab).style.display = 'block';
    document.getElementById(currBtn).style.color = '#FFC300';
    //document.getElementById(currBtn).style.color='navy';
    return false;
}

function Previous(NoneDivId) {

    var MedExGroupCode = document.getElementById('hfMedExGroupCode').value;
    var tblName = document.getElementById('hfMedExCodeForSex').value;
    if (MedExGroupCode != "" && tblName != "") {
        MedExGroupCode = MedExGroupCode.replace('BtnDiv', 'Div');
        var tblRdo = $get(tblName);
        var name = tblRdo.id;
        name = name.replace(/_/g, '$');
        var rdos = document.getElementsByName(name);
        var i;

        for (i = 0; i < rdos.length; i++) {
            if (rdos[i].checked && rdos[i].value == 'Male') {
                SexValue = 'Male';
                continue;
            }
        }
    }

    var arrDiv = NoneDivId.split(',');
    for (i = 0; i < arrDiv.length; i++) {
        document.getElementById(arrDiv[i]).style.display = 'none';
        var disBtn = arrDiv[i].replace('Div', 'BtnDiv');

        //document.getElementById(disBtn).style.color='Brown';
        document.getElementById(disBtn).style.color = '#FFffff';
    }

    for (i = 0; i < arrDiv.length; i++) {
        if (arrDiv[i].toLowerCase() == currTab.toLowerCase()) {
            if (i > 0) {
                currTab = arrDiv[i - 1];
                if (currTab == MedExGroupCode && SexValue == 'Male') {
                    currTab = arrDiv[i - 2];
                }
                break;
            }
        }
    }

    var currBtn = currTab.replace('Div', 'BtnDiv');
    document.getElementById(currTab).style.display = 'block';
    document.getElementById(currBtn).style.color = '#FFC300';
    //document.getElementById(currBtn).style.color='navy';
    return false;
}

function DisplayDiv(BlockDivId, NoneDivId) {

    var selBtn = BlockDivId.replace('Div', 'BtnDiv');
    var arrDiv = NoneDivId.split(',');

    for (i = 0; i < arrDiv.length; i++) {
        document.getElementById(arrDiv[i]).style.display = 'none';
        var disBtn = arrDiv[i].replace('Div', 'BtnDiv');

        //document.getElementById(disBtn).style.color='Brown';
        document.getElementById(disBtn).style.color = '#FFffff';
        if (selBtn.toLowerCase() == disBtn.toLowerCase()) {
            currTab = arrDiv[i];
        }
    }
    document.getElementById(BlockDivId).style.display = 'block';
    //document.getElementById(selBtn).style.color='navy';
    document.getElementById(selBtn).style.color = '#FFC300';
    return false;
}

function ddlAlerton(objId, alerton, alertmsg) {
    if (document.getElementById(objId).value == alerton) {
        msgalert(alertmsg);
    }
}

function Alerton(tblName, alertOn, alertMsg) {
    var tblRdo = $get(tblName);
    var name = tblRdo.id;
    name = name.replace(/_/g, '$');
    var rdos = document.getElementsByName(name);

    var i;

    for (i = 0; i < rdos.length; i++) {
        if (rdos[i].checked && rdos[i].value == alertOn) {
            msgalert(alertMsg);
        }
    }
}

function JustAlert(UserName, MedExCode, tblID) {
    if ($('#' + tblID).children().children().children().children().children("input:radio").attr("disabled") != true) {
        msgalert('Have You Checked "Past History" , "General Exam." And "Systemic Exam." ?');
        document.getElementById(MedExCode).value = UserName;
    }
    else {
        return false
    }
}

function SetUserName(UserName, MedExCode, tblID) {

    if ($('#' + tblID).children().children().children().children().children("input:radio").attr("disabled") != true) {
        document.getElementById(MedExCode).value = UserName;
        document.getElementById(MedExCode).readonly = true;
    }
    else {
        return false
    }
}

function FillBMIValue(txtHeightID, txtWeightID, txtBMIID) {
    var arrHeight = txtHeightID.split(",")
    var arrWeight = txtWeightID.split(",")
    var arrBMI = txtBMIID.split(",")

    var txtHeight = document.getElementById(txtHeightID);
    var txtWeight = document.getElementById(txtWeightID);
    var txtBMI = document.getElementById(txtBMIID);


    for (i = 0; i < arrHeight.length; i++) {
        if (document.getElementById(arrHeight[i]) != undefined) {
            txtHeight = document.getElementById(arrHeight[i]);
        }
    }

    for (i = 0; i < arrWeight.length; i++) {
        if (document.getElementById(arrWeight[i]) != undefined) {
            txtWeight = document.getElementById(arrWeight[i]);
        }
    }
    for (i = 0; i < arrBMI.length; i++) {
        if (document.getElementById(arrBMI[i]) != undefined) {
            txtBMI = document.getElementById(arrBMI[i]);
        }
    }

   

    //Again validate Height TextBox
    var result = CheckDecimal(txtHeight.value);
    if (result == false) {
        txtHeight.focus();
        return;
    }

    //Now Check Weight TextBox
    result = CheckDecimal(txtWeight.value);
    if (result == false) {
        msgalert('Please Enter Valid Weight in Kilogram');
        txtWeight.value = '';
        txtWeight.focus();
        return;
    }

    var weight = parseFloat(document.getElementById(txtWeight.id).value)
    var height = parseFloat(document.getElementById(txtHeight.id).value)
    var Wei = weight.toFixed(1)
    var Hei = height.toFixed(1)

    document.getElementById(txtHeight.id).value = Hei
    document.getElementById(txtWeight.id).value = Wei

    var bmi = GetBMI(txtHeight.value, txtWeight.value);
    try {
        if ((bmi != null) && !isNaN(bmi)) {
            bmi = parseFloat(bmi);
            txtBMI.value = bmi;

            if (bmi < 18 || bmi > 32) {
                msgalert('Bmi value is not suitable');
            }

        }
        else {
            lblBMI.value = '0';
            txtBMI.value = '0';
        }
    }
    catch (err) {
        msgalert(err.description);
    }
}
var prevtxt = "";
var prevtxt1 = "";
function C2F(txtCelsiusID, txtFahrenheitID, HighRange, LowRange) {

    var aarCel = txtCelsiusID.split(",")
    var aarFar = txtFahrenheitID.split(",")

    var txtCelsius = document.getElementById(txtCelsiusID);
    var txtFahrenheit = document.getElementById(txtFahrenheitID);

    for (i = 0; i < aarCel.length; i++) {
        if (document.getElementById(aarCel[i]) != undefined) {
            txtCelsius = document.getElementById(aarCel[i]);
        }
    }

    for (i = 0; i < aarFar.length; i++) {
        if (document.getElementById(aarFar[i]) != undefined) {
            txtFahrenheit = document.getElementById(aarFar[i]);
        }
    }

 


    var result;

    if (HighRange != 0 || LowRange != 0) {
        if (txtCelsius.value > HighRange || txtCelsius.value < LowRange) {
            if (prevtxt != txtFahrenheit.id) {
                msgalert('Out Of Range! Range Must be Between ' + LowRange + ' to ' + HighRange);
                prevtxt = txtCelsius.id;
            }

        }

    }

    if (!(CheckDecimalOrBlank(txtCelsius.value))) {
        msgalert('Please Enter Valid Temperature in Celsius.');
        return false;
    }



    txtFahrenheit.value = c2f(txtCelsius.value);
    var FarenhitHighLowRange = "";
    FarenhitHighLowRange = document.getElementById('HFFerenhitToCelcius').value;

    if (txtFahrenheit.value != 0) {
        if (txtFahrenheit.value > FarenhitHighLowRange.toString().split("##")[0] || txtFahrenheit.value < FarenhitHighLowRange.toString().split("##")[1]) {
            if (prevtxt1 != txtFahrenheit.id) {
                msgalert('Fahrenheit value Out Of Range! Range Must be Between ' + FarenhitHighLowRange.toString().split("##")[1] + ' to ' + FarenhitHighLowRange.toString().split("##")[0]);
                prevtxt1 = txtFahrenheit.id;
            }
        }
    }
    return true;
}

function F2C(txtFahrenheitID, txtCelsiusID, HighRange, LowRange) {


    var aarCel = txtCelsiusID.split(",")
    var aarFar = txtFahrenheitID.split(",")

    var txtCelsius = document.getElementById(txtCelsiusID);
    var txtFahrenheit = document.getElementById(txtFahrenheitID);
    var result;

    for (i = 0; i < aarCel.length; i++) {
        if (document.getElementById(aarCel[i]) != undefined) {
            txtCelsius = document.getElementById(aarCel[i]);
        }
    }

    for (i = 0; i < aarFar.length; i++) {
        if (document.getElementById(aarFar[i]) != undefined) {
            txtFahrenheit = document.getElementById(aarFar[i]);
        }
    }





    if (HighRange != 0 || LowRange != 0) {
        if (txtFahrenheit.value > HighRange || txtFahrenheit.value < LowRange) {
                msgalert('Out Of Range! Range Must be Between ' + LowRange + ' to ' + HighRange);
        }

    }

    if (!(CheckDecimalOrBlank(txtFahrenheit.value))) {
        msgalert('Please Enter Valid Temperature in Fahrenheit.');
        return false;
    }

    txtCelsius.value = f2c(txtFahrenheit.value);


    return true;
}

//        function ValidateRemarks(txt, cntField, maxSize) {
//            cntField = document.getElementById(cntField);
//            if (txt.value.length > maxSize) {
//                txt.value = txt.value.substring(0, maxSize);
//            }
//            // otherwise, update 'characters left' counter
//            else {
//                cntField.innerHTML = maxSize - txt.value.length;
//            }
//        }

function SetAge(txtDobId, txtAgeId, dtToday) {
    var arrDobId = txtDobId.split(",")
    var arrAge = txtAgeId.split(",")

    var txtDob = document.getElementById(txtDobId);
    var txtAge = document.getElementById(txtAgeId);

    for (i = 0; i < arrDobId.length; i++) {
        if (document.getElementById(arrDobId[i]) != undefined) {
            txtDob = document.getElementById(arrDobId[i]);
        }
    }

    for (i = 0; i < arrAge.length; i++) {
        if (document.getElementById(arrAge[i]) != undefined) {
            txtAge = document.getElementById(arrAge[i]);
        }
    }

    DateConvert(txtDob.value, txtDob);

    if (txtDob.value.length > 10) {

        var dt = txtDob.value;
        var day;
        var month;
        var year;
        var inyear;
        if (dt.indexOf('-') >= 0) {
            var arrDate = dt.split('-');
            day = arrDate[0];
            month = arrDate[1];
            year = arrDate[2];
        }
        inyear = parseInt(year, 10);

        if (inyear < 1900) {
            msgalert('You Can Not Add Date Which Is Less Than "01-Jan-1900" ');
            txtDob.value = "";
            txtAge.value = "";
            txtDob.focus();
        }
        var age = GetDateDifference(txtDob.value, dtToday);
        //age = getAge(txtDob.value);

        if ((age.Days / 365) == 1) {
            age.Years = age.Years + 1;
        }
        else {
            age.Years = age.Years;
        }
        txtAge.value = age.Years.toString();

        if (parseInt(age) < parseInt(18)) {
            msgalert('Age of Subject is not suitable');
        }
        //txtAge.value = age.toString();
    }
}

function getAge(dateString) {
    var today = new Date();
    var birthDate = new Date(dateString);
    var Years = today.getFullYear() - birthDate.getFullYear();
    var m = today.getMonth() - birthDate.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
        Years--;
    }
    return Years;
}

function ValidationQC() {
    if (document.getElementById('txtQCRemarks').value.toString().trim().length <= 0) {
        msgalert('Please Enter Remarks/Response');
        document.getElementById('txtQCRemarks').focus();
        return false;
    }
    return true;
}


function QCDivShowHide(Type) {

    var Valid = false;

    if (document.getElementById('HSubjectId').value.toString().trim().length <= 0) {
        msgalert('Please Enter Subject');
        document.getElementById('txtSubject').focus();
        document.getElementById('txtSubject').value = '';
        return false;
    }
    else if (!(document.getElementById('rblScreeningDate') == null || document.getElementById('rblScreeningDate') == 'undefined')) {
        var tableName = document.getElementById('rblScreeningDate');
        var radio = tableName.getElementsByTagName('input');


        for (i = 0; i < radio.length; i++) {

            if (radio[i].type == 'radio' || radio[i].type == 'RADIO') {

                if (radio[i].checked == true) {
                    Valid = true;
                    break;
                }
            }
        }
    }

    if (Valid == false) {
        msgalert('please select screening date');
        return false;
    }
    else if (Type == 'S') {

        //                document.getElementById('divQCDtl').style.display = 'block';
        //                SetCenter('divQCDtl');
        $find('MpeQC').show();
        return false;
    }
    else if (Type == 'H') {
        $find('MpeQC').hide();
        //document.getElementById('divQCDtl').style.display = 'none';
        return false;
    }
    else if (Type == 'ST') {
        $find('MpeQC').show();
        //                document.getElementById('divQCDtl').style.display = 'block';
        //                SetCenter('divQCDtl');
        return true;
    }
    return true;
}

function DivAuditShowHide(type) {
    if (type == 'H') {
        document.getElementById('divAudit').style.display = 'none';
        return false;
    }
}
function HistoryDivShowHide(Type, MedexCode, BlockDivId, NoneDivId, ScreeningType,LocationCode) {


    //document.getElementById('hfMedexCode').value = MedexCode + '###' + ScreeningType;

    if (document.getElementById('HSubjectId').value.toString().trim().length <= 0) {
        msgalert('Please Enter Subject');
        document.getElementById('txtSubject').focus();
        document.getElementById('txtSubject').value = '';
        return false;
    }
    else if (Type == 'S') {
        var radiolst = document.getElementById("rblScreeningDate");
        var radios;
        var i;
        var SelctedVal = "";
        if (radiolst != null && typeof (radiolst) != 'undefined') {
            radios = radiolst.getElementsByTagName('input');
            for (i = 0; i < radios.length; i++) {
                if (radios[i].checked == true) {
                    SelctedVal = radios[i].value;
                    break;
                }
            }
        }
        var Subject = document.getElementById('HSubjectId').value.toString();
        fnGetAuditTrail(MedexCode, ScreeningType, Subject, SelctedVal,LocationCode);
        return false;
    }
    else if (Type == 'H') {
        $find('MPEDeviation').hide();
        // document.getElementById('divHistoryDtl').style.display = 'none';
        return false;
    }
    //            else if (Type == 'SN') {
    //                document.getElementById('divHistoryDtl').style.display = 'block';
    //                SetCenter('divHistoryDtl');
    //                return DisplayDiv(BlockDivId, NoneDivId);
    //            }
    return true;

}

//For Validation
var count = 0;
var element;
var prev;
var result;


function Validation(type) {
    count = 0;
    var maleCount = document.getElementById('HfMaleCount').value; ;
    var MedExGroupCode = document.getElementById('hfMedExGroupCode').value;
    var tblName = document.getElementById('hfMedExCodeForSex').value;
    jQuery('.Required').each(validateControls);
    if (maleCount != "" && MedExGroupCode != "" && tblName != "") {
        MedExGroupCode = MedExGroupCode.replace('BtnDiv', 'Div');
        var tblRdo = $get(tblName);
        var name = tblRdo.id;
        name = name.replace(/_/g, '$');
        var rdos = document.getElementsByName(name);
        var i, cnt = 0;
        var SexValue;
        for (i = 0; i < rdos.length; i++) {
            if (rdos[i].checked && rdos[i].value == 'Male') {

                count = count - maleCount;
            }
        }
    }

    if (count > 0) {
        var conf = confirm('' + count + ' Field(s) Are Blank, Do You Still Want To Save?');
        if (conf) {
            // document.getElementById('BtnSave').style.display = 'none';

        }
        else {
            return false;
        }
    }

    if (document.getElementById('HSubjectId').value.toString().trim().length <= 0) {
        msgalert('Please Enter Subject');
        document.getElementById('txtSubject').focus();
        document.getElementById('txtSubject').value = '';
        return false;
    }
    //Added for compulsory add remark while Edit else not compulsory on 15-Sep-2009
    else if (type == 'EDIT') {
        //                if (document.getElementById('txtremark').value.toString().trim().length <= 0) {
        //                    alert('Please Enter Remarks');
        //                    return false;
        //                }
        document.getElementById('btnSave').style.display = 'none';
        return true;

    }
    else if (type == 'ADD') {
        document.getElementById('btnSave').style.display = 'none';
        return true;
    }
    return true;

}

function validateControls(index) {
    element = jQuery('.Required')[index];

    if (element.nodeName == 'TABLE') {

        if ($('input:checked', element).length == 0) {
            element.style.border = '1px solid Red';
            count = count + 1;
            return;
        }
    }
    else {
        switch (element.type) {
            case 'text':
            case 'textarea':
                document.getElementById(element.id).style.borderColor = '';
                if (element.value.trim().length <= 0) {
                    document.getElementById(element.id).style.borderColor = 'Red';
                    count = count + 1;
                    return;
                }
                break;

            case 'select-one':
                document.getElementById(element.id).style.backgroundColor = '';
                if (element.value.trim().length <= 0) {
                    document.getElementById(element.id).style.backgroundColor = 'Red';
                    count = count + 1;
                    return;
                }
                break;
        }
    }
}


function CheckOnlyForFemale(tblName) {
    var MedExGroupCode = document.getElementById('hfMedExGroupCode').value;
    var tblRdo = $get(tblName);
    var name = tblRdo.id;
    name = name.replace(/_/g, '$');
    var rdos = document.getElementsByName(name);
    var i;
    for (i = 0; i < rdos.length; i++) {
        if (rdos[i].checked && rdos[i].value == 'Male') {
            document.getElementById(MedExGroupCode).value = 'Not Applicable For Male';
            document.getElementById(MedExGroupCode).disabled = true;
        }
        else if (rdos[i].checked && rdos[i].value == 'Female') {
            document.getElementById(MedExGroupCode).disabled = false;
            document.getElementById(MedExGroupCode).value = 'For Female Only';
        }
    }
}

function Authentication() {

    $find('MPEId').show();

}
//        function Pwd_AuthenticationFail() {
//            document.getElementById('11166').value = "";
//            document.getElementById('11173').value = "";
//            document.getElementById('11184').value = "";
//        }
function SetEligibilitydeclaredon(Eligibilitydeclaredby, Eligibilitydeclaredon, Username) {
    document.getElementById(Eligibilitydeclaredby).value = Username;
    var EligibilityDeclared = document.getElementById(Eligibilitydeclaredby).value.trim();
    document.getElementById(Eligibilitydeclaredon).value = '';
    if (EligibilityDeclared != '') {
        var d = new Date();
        var curr_date = d.getDate();
        var curr_month = d.getMonth() + 1;
        var curr_year = d.getFullYear();
        var curr_hour = d.getHours();
        var curr_min = d.getMinutes();
        var curr_sec = d.getSeconds();

        if (curr_date < 10) {
            var curr_date = '0' + curr_date;
        }
        if (curr_month <= 9) {
            var curr_month = '0' + curr_month;
        }
        if (curr_hour <= 9) {
            var curr_hour = '0' + curr_hour;
        }
        if (curr_min <= 9) {
            var curr_min = '0' + curr_min;
        }
        if (curr_sec <= 9) {
            var curr_sec = '0' + curr_sec;
        }
        document.getElementById(Eligibilitydeclaredon).value = curr_date + "-" + curr_month + "-" + curr_year + " " + curr_hour + ":" + curr_min + ":" + curr_sec;

    }

}

function SetPICommentsgivenon(PICommentsgivenon, Username, PICommentsgivenBy) {

    var d = new Date();
    var curr_date = d.getDate();
    var curr_month = d.getMonth() + 1;
    var curr_year = d.getFullYear();
    var curr_hour = d.getHours();
    var curr_min = d.getMinutes();
    var curr_sec = d.getSeconds();


    if (curr_date < 10) {
        var curr_date = '0' + curr_date;
    }
    if (curr_month <= 9) {
        var curr_month = '0' + curr_month;
    }
    if (curr_hour <= 9) {
        var curr_hour = '0' + curr_hour;
    }
    if (curr_min <= 9) {
        var curr_min = '0' + curr_min;
    }
    if (curr_sec <= 9) {
        var curr_sec = '0' + curr_sec;
    }
    document.getElementById(PICommentsgivenon).value = curr_date + "-" + curr_month + "-" + curr_year + " " + curr_hour + ":" + curr_min + ":" + curr_sec;
    document.getElementById(PICommentsgivenBy).value = Username;
}
function HomeClick(e) {
    msgConfirmDeleteAlert(null, "Are You Sure You Want To Go To Home Page?", function (isConfirmed) {
        if (isConfirmed) {
            __doPostBack(e.name, '');
            return true;
        }
        else {
            return false;
        }
    });
    return false;
}

function MedExFormula(MedExCode, formula,iDecimalNo) {
    document.getElementById('hfMedexCode').value = MedExCode;
//    alert('hfMedexCode');
    document.getElementById('HFMedExFormula').value = formula;
//    alert('HFMedexFormula');
//    document.getElementById('HFDecimalNo').value = iDecimalNo;
//    alert('HfDecimalNo');

    MedExes = formula.split("?");
    var getformula = formula.replace(/\?/g,'');
    var controltype = "text";
    var decimalNo=parseInt(iDecimalNo) ;
    document.getElementById('HFDecimalNo').value=decimalNo
    
    for ( var i = 0; i < MedExes.length; i ++)
     {
         if(MedExes[i].length == 5)
          {
            MedexValue = $('#' + MedExes[i]).val();
            getformula = getformula.replace(MedExes[i], MedexValue)
             if ($('#' + MedExes[i]).attr('type') != "text")
                {
                    controltype ="DateTime"
                }
             if( $('#' + MedExes[i]).attr('onchange') != null)   
                {
                   if ($('#' + MedExes[i]).attr('onchange').toString().indexOf('DateConvertForScreening') != -1)
                    {
                        controltype ="DateTime"                  
                    }
 
                }
          }
     }
    
    var content = {};
    content.formula = getformula;
    content.controltype = controltype ;
    content.DecimalNo = decimalNo;
    $.ajax({
        type: "POST",
        url: "frmSubjectScreening_New.aspx/GetFormulaeVal",
        data: JSON.stringify(content),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(data) {
             if (data.d != "" && data.d != null) {
//                 data = JSON.parse(data.d); // Commented By Dipen Shah at 25-aug-2014 To fic Decimal Number.
                  
                  
                   data=data.d; //Added By Dipen Shah on 25-Aug-2014 to display Decimal Number.
                   if (data.split('.')[1]){ 
                    if (data.split('.')[1].length!=decimalNo){
                        for(i=data.split('.')[1].length ; i<=decimalNo-1 ;i++){
                            data=data + "0";
                        }
                        }
                      }
               else {
                    if(decimalNo!=0)
                    {
                        data = data + ".";
                    }
                for (var i = 0 ; i <= decimalNo-1; i++) {
                    data = data + "0";
                }
            }
                 $('#' + $('#hfMedexCode').val()).val(data)  
                 $('#' + $('#HFDecimalNo').val()).val(data)             
            }
            else {
                msgalert("Error While Getting value");
                return false;
            }
        },
        failure: function(error) {
            msgalert(error);
        }
    });
     
//    var btn = document.getElementById('btnAutoCalculate');
//    btn.click();
    return false;
}

function SetFormulaResult(result) {
    var MedExCode = document.getElementById('hfMedexCode').value;
    var control = document.getElementById(MedExCode);
    if (control != null && typeof (control) != 'undefined') {
        control.value = result;
        control.focus();
    }
}

function DateConvertForScreening(ParamDate, txtdate) {

    if (ParamDate.length == 0) {
        return true;
    }

    if (ParamDate.trim() != '') {

        var dt = ParamDate.trim().toUpperCase();
        var tempdt;
        if (dt.indexOf('UK') >= 0 || dt.indexOf('UNK') >= 0 || dt.indexOf('UKUK') >= 0) {

            if (dt.length < 8) {
                msgalert('Please Enter Date In DDMMYYYY Or dd-Mon-YYYY Format Only.');
                txtdate.value = "";
                txtdate.focus();
                return false;
            }
            var day;
            var month;
            var year;
            if (dt.indexOf('-') >= 0) {
                var arrDate = dt.split('-');
                day = arrDate[0];
                month = arrDate[1];
                year = arrDate[2];
            }
            else {
                day = dt.substr(0, 2);
                month = dt.substr(2, 2);
                year = dt.substr(4, 4);
                if (dt.indexOf('UNK') >= 0) {
                    month = dt.substr(2, 3);
                    year = dt.substr(5, 4);
                }
                if (dt.indexOf('UNK') == -1) {
                    month = dt.substr(2, 2);
                    year = dt.substr(4, 5);
                }
            }
            inyear = parseInt(year, 10);

            if (day.length > 2 && day.length != 0) {
                txtdate.focus();
                msgalert('Please Enter Date In DDMMYYYY Or dd-Mon-YYYY Format Only.');
                txtdate.value = "";
                return false;
            }
            if (month.length > 3 && month.length != 3) {
                txtdate.focus();
                msgalert('Please Enter Date In DDMMYYYY Or dd-Mon-YYYY Format Only.');
                txtdate.value = "";
                return false;
            }
            if (year.length > 4 && month.length != 4) {
                txtdate.focus();
                msgalert('Please Enter Date In DDMMYYYY Or dd-Mon-YYYY Format Only.');
                txtdate.value = "";
                return false;
            }
            if (day == 'UK') {
                tempdt = '01';
            }
            else {
                tempdt = day;
            }
            if (dt.indexOf('-') >= 0) {
                tempdt += '-';
            }
            if (month == 'UNK') {
                tempdt += '01';
            }
            else {
                tempdt += month;
            }
            if (dt.indexOf('-') >= 0) {
                tempdt += '-';
            }
            if (year == 'UKUK') {
                tempdt += '1800';
            }
            else {
                tempdt += year;
            }
            var chk = false;
            chk = DateConvert(tempdt, txtdate);
            if (chk == true) {
                if (isNaN(month)) {
                    txtdate.value = day + '-' + month + '-' + year;
                }
                else {
                    txtdate.value = day + '-' + cMONTHNAMES[month - 1] + '-' + year;
                }
                if (inyear < 1900) {
                    txtdate.focus();
                    msgalert('You Can Not Add Date Which Is Less Than "01-Jan-1900"  ');
                    txtdate.value = "";
                    return false;
                }
                return true;
            }
            txtdate.value = "";
            txtdate.focus();
            return false;
        }
    }
    DateConvert(txtdate.value, txtdate);
    dt = txtdate.value;
    var Year = dt.substring(dt.lastIndexOf('-') + 1);
    inyear = parseInt(Year, 10);
    if (inyear < 1900) {
        txtdate.focus();
        msgalert('You Can Not Add Date Which Is Less Than "01-Jan-1900"  ');
        txtdate.value = "";
        
        return false;
    }
    return true;
}

function ValidationForAuthentication() {

    if ($("input:radio[name='rblSaveEle']:checked").val() == undefined) {
        msgalert('Please Select Eligibility For Authentication.');
        return false;
    }
    if (document.getElementById('txtPassword').value.trim() == '') {
        document.getElementById('txtPassword').value = '';
        msgalert('Please Enter Password For Authentication.');
        document.getElementById('txtPassword').focus();
        return false;
    }
    if (document.getElementById('HdnWorkflow').value == 20) {
        if (document.getElementById('txtPiRemarks').value == '') {
            msgalert('Please Enter Remarks For Authentication.');
            document.getElementById('txtPiRemarks').focus();
            return false;
        }
    }
    var content = {};
    content.SubjectId = $("#HSubjectId").val()
    content.ScreeningDate = $('#ddlScreeningDate option:selected').val();
    content.PWDEnter = $('#txtPassword').val()
    content.PWDOld = $('#hdnPassWord').val()


    $.ajax({
        type: "POST",
        url: "frmSubjectScreening_New.aspx/Proc_ValidationForEligibility",
        data: JSON.stringify(content),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.d != "" && data.d != null) {
                data = JSON.parse(data.d);
                if (data == "PassWord Incorrect") {
                    msgalert("Password Authentication Fails!")
                    return false;
                }

                else if ($('#rblSaveEle_0').is(':checked') == true && data.toUpperCase() == 'NO')
                {
                    msgalert("Clinical Eligibility Criteria Is Selected as No. Please Check again !!")
                    return false;
                }
                else if (data == "Y" && $('input[name=rblSaveEle]:checked').val() == "N") {
                    msgalert("You Can Not Change Eligibility Please First  Rejected a Subject From Project!")
                    return false;
                }
                else {
                    btnAuthenticateHide.click()
                    //var btn = document.getElementById('<%= btnAuthenticateHide.ClientId%>');
                    //btn.click();

                   
                }
            }
            else {
                return false;
            }
        },
        failure: function (error) {
            msgalert(error);
        }
    });
    return true;
}

function ValidationForAuthenticationSave() {
    var chk = document.getElementById('rblSaveEle');
    var radios;
    var i;
    var flag = false;
    if ($('#rblSaveEle').is(":visible") == true) {
        if (chk != null && typeof (chk) != 'undefined') {
            radios = chk.getElementsByTagName('input');
            for (i = 0; i < radios.length; i++) {
                if (radios[i].type.toUpperCase() == 'RADIO') {
                    if (radios[i].checked == true) {
                        flag = true
                    }
                }
            }
        }
    }
    if (flag == false) {
        msgalert('Please Select Eligibility criteria');
        return false;
    }
    if (document.getElementById('txtPassword').value.trim() == '') {
        document.getElementById('txtPassword').value = '';
        msgalert('Please Enter Password For Authentication.');
        document.getElementById('txtPassword').focus();
        return false;
    }
    //            if (document.getElementById('HdnWorkflow').value == 20)
    //            {
    if (document.getElementById('txtPiRemarks').value == '') {
        msgalert('Please Enter Remarks For Authentication.');
        document.getElementById('txtPiRemarks').focus();
        return false;
    }
    //            }


    $find('MPEId').hide();
    return true;
}

function DivAuthenticationHideShow() {
    $find('MPEId').hide();
    document.getElementById('txtPassword').value = "";
    //            document.getElementById('11166').value = "";
    //            document.getElementById('11173').value = "";
    //            document.getElementById('11184').value = "";
}

function ShowHideproject() {
    var indices = '';
    $('#chkScreeningType input:checkbox').each(function(index) { if ($(this).filter('input:checkbox:checked').length == 1) { indices += (index + ' '); } });
    if (indices.split('1').length == 2) {
        $('#tr_WorkSpace')[0].attributes["style"].nodeValue = "display:'';";
    }
    else {
        $('#tr_WorkSpace')[0].attributes["style"].nodeValue = "display:none";

    }
    return true;


}

function c2f(celsius2) {

    var fahrenheit2;
    if (document.getElementById('Celcius').value != "") {
        var HFCelsius = document.getElementById('Celcius').value;
        if ((celsius2.substring(0, 4)) == (HFCelsius.substring(0, 4))) {
            celsius2 = document.getElementById('Celcius').value;
            document.getElementById('Celcius').value = "";
        }

    }
    fahrenheit2 = 9 / 5 * celsius2 + 32;

    document.getElementById('Ferenhit').value = fahrenheit2;

    if (fahrenheit2.toString().indexOf('.') > 0) {
        fahrenheit2 = fahrenheit2.toString().substring(0, fahrenheit2.toString().indexOf('.') + 2)
    }
    return fahrenheit2;
}
function f2c(fahrenheit1) {

    var celsius1;

    if (document.getElementById('Ferenhit').value != "") {
        var HFFarenheit = document.getElementById('Ferenhit').value;

        if ((fahrenheit1.substring(0, 4)) == (HFFarenheit.substring(0, 4))) {
            fahrenheit1 = document.getElementById('Ferenhit').value;
            document.getElementById('Ferenhit').value = "";
        }
    }
    celsius1 = 5 / 9 * (fahrenheit1 - 32)
    document.getElementById('Celcius').value = celsius1;
    if (celsius1.toString().indexOf('.') > 0) {
        celsius1 = celsius1.toString().substring(0, celsius1.toString().indexOf('.') + 2)
    }


    return celsius1;
}


function DiffAge(string) {

    var StartDate = string.substring(0, 7)
    var EndDate = string.substring(8, 15)
    var age = GetDateDifference(StartDate, EndDate);
}

function AuditDivShowHide(Type, MedexCode, buttonId, ScreeningDtlNo, Workspaceid, ScreeninghdrNo,MedexType) {
    var isDisabled
    if (MedexType.toUpperCase() == 'RADIO') {
        isDisabled = $("input[name^=" + MedexCode + "]").attr('disabled')
    }
    else {
         isDisabled = $("#"+MedexCode ).attr('disabled')
    }
    
    if (Type == "U") {
        if (isDisabled == true) {
            return false;
        }
    }

    document.getElementById('HFMedexType').value = MedexType;
   
    if (Type == "U") {
        if ($('#btnUpdate' + buttonId).attr("disabled") == true) {
            return false;
        }


    }
    if (Type == "E") {
  
        if ($('#btnEdit' + buttonId).attr("disabled") == true) {
            return false;
        }
        else {
            $('#hdnCurBtnIDAudit').val('#btnAudittrail' + buttonId);
        }
    }
    var btnE = document.getElementById('btnEdit');
    //var btnU = document.getElementById('btnUpdate');
    document.getElementById('hfMedexCode').value = MedexCode;
    document.getElementById('HFScreeningDtlNo').value = ScreeningDtlNo;
    document.getElementById('HFScreeningWorkSpaceID').value = Workspaceid;
    document.getElementById('HFScreeningHdrlNo').value = ScreeninghdrNo;
    document.getElementById('HdnCurBtnId').value = buttonId;
    if (Type == 'E') {
    
         if(MedexType == "File")
        {
            MedexCode = "FU" + MedexCode;
            document.getElementById("hfMedexCode").value=MedexCode;
        }
         if (MedexType.toUpperCase() == "STANDARDDATE") {
             $('select[id*="' + MedexCode + '"]').attr('disabled', false);
             $('select[id*="' + MedexCode + '"]').attr('readOnly', false);
         }
         else {
             document.getElementById(MedexCode).disabled = false;
             document.getElementById(MedexCode).removeAttribute('readOnly'); //Enhancement in EDC
         }
        fnGetElement(MedexCode, "E")
        $('#btnUpdate' + buttonId).attr("disabled", false)
//        if ($('#' + MedexCode).next().attr('value') == "Calc")
//        {
//           $('#' + MedexCode).next().attr("disabled", false)
//        }
        //document.getElementById('btnUpdate' + buttonId).disabled = false;
        //document.getElementById('btnEdit' + buttonId).disabled = true;
        document.getElementById(MedexCode).focus();
        return false;
    }
    if (Type == 'U') {
    
         if(MedexType == "File")
        {
            MedexCode = "FU" + MedexCode; // Added by Dipen Shah On 28 Aug 2014 For file attribute Edit update And Audittrail.
            document.getElementById("hfMedexCode").value=MedexCode;
        }
    
        fnGetElement(MedexCode, "U")
        $('#txtRemarkForAttributeEdit').val("");
        document.getElementById('divForEditAttribute').style.display = 'block'
        displayBackGround();
        document.getElementById('btnUpdate' + buttonId).disabled = true;
        document.getElementById('btnEdit' + buttonId).disabled = false;
        
         if(MedexType == "File")
           {
            document.getElementById("hfMedexCode").value=MedexCode.split("FU")[1]
           }
      
        return false;
    }
    //            if (Type == 'D') {
    //                btnD.click();
    //                return false;
    //            }
    //            if (Type == 'S') {
    //                var btnS = document.getElementById('btnSaveRunTime');
    //                return false;
    //            }
    return true;
}

function ValidationForEditOrDelete() {
    var medexType = document.getElementById("HFMedexType").value;
    //if (document.getElementById('txtRemarkForAttributeEdit').value.trim() == '') {
    //    alert('Select Either Reason Or Specify Remarks.');
    //    document.getElementById('txtRemarkForAttributeEdit').value = '';
    //    return false;
    //}

    if ($('#ddlRemarksForEdit option:selected').val() == "0" && $("#txtRemarkForAttributeEdit").val() == "") {
        msgalert('Select Either Reason Or Specify Remarks.');
        return false;
    }
    else if ($('#ddlRemarksForEdit option:selected').val() != "0" && $("#txtRemarkForAttributeEdit").val() != "") {
        msgalert('Select Either Reason Or Specify Remarks.');
        return false;
    }

    else {
        try {

            if (document.getElementById('txtRemarkForAttributeEdit').value != "") {
                document.getElementById('HdReasonDesc').value = document.getElementById('txtRemarkForAttributeEdit').value;
            }
            else {
                document.getElementById('HdReasonDesc').value = $('#ddlRemarksForEdit option:selected').text()
            }

            
            funSaveEditVal(medexType);

            var btn = $('#hdnCurBtnIDAudit').val();
            $(btn).attr("src", "Images/AuditMark_Small.png")
            if (btn == "#btnAudittrail00011001500443" || btn == "#btnAudittrail00011001500321") {
                $('#btnAudittrail00011001500323').attr("src", "Images/AuditMark_Small.png")
            }
            if (document.getElementById('HdReasonDesc').value == 'undefined')
            { throw "Undefined value" }
            return true;
        }
        catch (err) {
            msgalert('An error has occurred: ' + err.message);
            return false;
        }

    }
}


function funSaveEditVal(MedexType) {

debugger;
    var Editdata = []
    var Assigncontentdtl = new Object();
    Assigncontentdtl.nMedExScreeningHdrNo = document.getElementById('HFScreeningHdrlNo').value;
    Assigncontentdtl.vMedExCode = document.getElementById('hfMedexCode').value;
    var Medexcode = document.getElementById('hfMedexCode').value
     if (MedexType == "File") // Added by Dipen Shah on 28 Aug 2014 for file type attribute .
    {
       Assigncontentdtl.vMedExResult = document.getElementById('HdnMedexVal').value.split("\\")[2];
    }
    else
    {
        Assigncontentdtl.vMedExResult = document.getElementById('HdnMedexVal').value;
    }
    Assigncontentdtl.vWorkSpaceID = document.getElementById('HFScreeningWorkSpaceID').value;
    Assigncontentdtl.vRemarks = document.getElementById('HdReasonDesc').value;
    Assigncontentdtl.iModifyBy = document.getElementById('HdnSUserid').value;
    Assigncontentdtl.cStatusIndi = "N";
    Editdata.push(Assigncontentdtl)
    if (Medexcode == "00321" || Medexcode == "00443") {
        var AssignBMIdtl = new Object();
        AssignBMIdtl.nMedExScreeningHdrNo = document.getElementById('HFScreeningHdrlNo').value;
        AssignBMIdtl.vMedExCode = "00323"
        AssignBMIdtl.vMedExResult = document.getElementById('00323').value;
        AssignBMIdtl.vWorkSpaceID = document.getElementById('HFScreeningWorkSpaceID').value;
        AssignBMIdtl.vRemarks = document.getElementById('HdReasonDesc').value;
        AssignBMIdtl.iModifyBy = document.getElementById('HdnSUserid').value;
        AssignBMIdtl.cStatusIndi = "N";
        Editdata.push(AssignBMIdtl)
    }
    if (Medexcode == "28071" || Medexcode == "28072") {
        var AssignBMIdtl = new Object();
        AssignBMIdtl.nMedExScreeningHdrNo = document.getElementById('HFScreeningHdrlNo').value;
        AssignBMIdtl.vMedExCode = "28073"
        AssignBMIdtl.vMedExResult = document.getElementById('28073').value;
        AssignBMIdtl.vWorkSpaceID = document.getElementById('HFScreeningWorkSpaceID').value;
        AssignBMIdtl.vRemarks = document.getElementById('HdReasonDesc').value;
        AssignBMIdtl.iModifyBy = document.getElementById('HdnSUserid').value;
        AssignBMIdtl.cStatusIndi = "N";
        Editdata.push(AssignBMIdtl)
    }

    
    if (Medexcode == "00608" || Medexcode == "28065") {
        var AssignAgedtl = new Object();
        AssignAgedtl.nMedExScreeningHdrNo = document.getElementById('HFScreeningHdrlNo').value;
        if (Medexcode == "00608") {
            AssignAgedtl.vMedExCode = "00609"
            AssignAgedtl.vMedExResult = document.getElementById('00609').value;
        }
        else {
            AssignAgedtl.vMedExCode = "28066"
            AssignAgedtl.vMedExResult = document.getElementById('28066').value;
        }
        
        AssignAgedtl.vWorkSpaceID = document.getElementById('HFScreeningWorkSpaceID').value;
        AssignAgedtl.vRemarks = document.getElementById('HdReasonDesc').value;
        AssignAgedtl.iModifyBy = document.getElementById('HdnSUserid').value;
        AssignAgedtl.cStatusIndi = "N";
        Editdata.push(AssignAgedtl)
    }
    if (Medexcode == "00362") {
        var AssignFCdtl = new Object();
        AssignFCdtl.nMedExScreeningHdrNo = document.getElementById('HFScreeningHdrlNo').value;
        AssignFCdtl.vMedExCode = "00610"
        AssignFCdtl.vMedExResult = document.getElementById('00610').value;
        AssignFCdtl.vWorkSpaceID = document.getElementById('HFScreeningWorkSpaceID').value;
        AssignFCdtl.vRemarks = document.getElementById('HdReasonDesc').value;
        AssignFCdtl.iModifyBy = document.getElementById('HdnSUserid').value;
        AssignFCdtl.cStatusIndi = "N";
        Editdata.push(AssignFCdtl)
    }
    if (Medexcode == "00610") {
        var AssignCFdtl = new Object();
        AssignCFdtl.nMedExScreeningHdrNo = document.getElementById('HFScreeningHdrlNo').value;
        AssignCFdtl.vMedExCode = "00362"
        AssignCFdtl.vMedExResult = document.getElementById('00362').value;
        AssignCFdtl.vWorkSpaceID = document.getElementById('HFScreeningWorkSpaceID').value;
        AssignCFdtl.vRemarks = document.getElementById('HdReasonDesc').value;
        AssignCFdtl.iModifyBy = document.getElementById('HdnSUserid').value;
        AssignCFdtl.cStatusIndi = "N";
        Editdata.push(AssignCFdtl)
    }

    if (Medexcode == "28076") {
        var AssignFCdtl = new Object();
        AssignFCdtl.nMedExScreeningHdrNo = document.getElementById('HFScreeningHdrlNo').value;
        AssignFCdtl.vMedExCode = "28077"
        AssignFCdtl.vMedExResult = document.getElementById('00610').value;
        AssignFCdtl.vWorkSpaceID = document.getElementById('HFScreeningWorkSpaceID').value;
        AssignFCdtl.vRemarks = document.getElementById('HdReasonDesc').value;
        AssignFCdtl.iModifyBy = document.getElementById('HdnSUserid').value;
        AssignFCdtl.cStatusIndi = "N";
        Editdata.push(AssignFCdtl)
    }
    if (Medexcode == "28077") {
        var AssignCFdtl = new Object();
        AssignCFdtl.nMedExScreeningHdrNo = document.getElementById('HFScreeningHdrlNo').value;
        AssignCFdtl.vMedExCode = "28076"
        AssignCFdtl.vMedExResult = document.getElementById('00362').value;
        AssignCFdtl.vWorkSpaceID = document.getElementById('HFScreeningWorkSpaceID').value;
        AssignCFdtl.vRemarks = document.getElementById('HdReasonDesc').value;
        AssignCFdtl.iModifyBy = document.getElementById('HdnSUserid').value;
        AssignCFdtl.cStatusIndi = "N";
        Editdata.push(AssignCFdtl)
    }


    

    var jsonText = JSON.stringify({ Editdata: Editdata });
    $.ajax({
        type: "POST",
        url: "Ws_Lambda_JSON.asmx/Save_MedexScreeningDtl",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: jsonText,
        success: function(data) {
            if (data.d != "error") {
                var MedexCode = document.getElementById('hfMedexCode').value;
                fnGetElement(MedexCode, "D")
                AnyDivShowHide("SH");
                //funCloseDiv('divForEditAttribute',1);
                var buttonId = document.getElementById('HdnCurBtnId').value;
                var btn = document.getElementById ('btnRedirect')
                btn.click();
                //$('#btnUpdate' + buttonId).attr("disabled", "true");
            }
            else {
                msgalert('Error While Assigning Data');
                fnGetElement(document.getElementById('hfMedexCode').value, "AD")
                funCloseDiv('divForEditAttribute', 1);
            }
        },
        error: function(ex) {
            msgalert("Error Occured In Assigning Val In Database ")
            console.log(ex);
        }
    });




}

function funCloseDiv(div, mode) {
    if (mode == 2) {
        BtnID = document.getElementById('HdnCurBtnId').value;
        //document.getElementById(AttrID).value = document.getElementById('HdnOldAttrVal').value; 
        var MedexCode = document.getElementById('hfMedexCode').value;
        fnGetElement(MedexCode, "AD");
        var buttonId = document.getElementById('HdnCurBtnId').value;
        $('#btnUpdate' + buttonId).attr("disabled", "true");
        //document.getElementById('btnUpdate' + BtnID).disabled = true;
        //document.getElementById('btnEdit' + BtnID).disabled = false;
    }
    document.getElementById(div).style.display = 'none';
    document.getElementById('ModalBackGround').style.display = 'none';
}

function displayBackGround() {
    document.getElementById('ModalBackGround').style.display = '';
    document.getElementById('ModalBackGround').style.height = $('#HFHeight').val() + "px";
    document.getElementById('ModalBackGround').style.width = $('#HFWidth').val() + "px";
}

function AnyDivShowHide(Type) {
    if (Type == 'S') {
        document.getElementById('divForEditAttribute').style.display = '';
        displayBackGround();
        return false;
    }
    else if (Type == 'H') {
        funCloseDiv('divForEditAttribute', 2);
        return false;
    }
    else if (Type == 'SH') {
        funCloseDiv('divForEditAttribute', 1);
        return false;
    }
    //            else if (Type == 'DCFSHOW') {
    //                document.getElementById('divDCF').style.display = '';
    //                displayBackGround();
    //                return false;
    //            }
    //            else if (Type == 'DCFHIDE') {
    //                //document.getElementById('divDCF').style.display = 'none';
    //                funCloseDiv('divDCF');
    //                return false;
    //            }
    //            else if (Type == 'DIVQUERIESSHOW') {
    //                document.getElementById('divQueries').style.display = '';
    //                SetCenter('divQueries');
    //                return false;
    //            }
    //            else if (Type == 'DIVQUERIESHIDE') {
    //                document.getElementById('divQueries').style.display = 'none';
    //                return false;
    //            }
    return true;
}

function fnGetElement(MedexCode, type) {
    
    var MedexVal = "";
    var eledisable = false;
    var chklst = document.getElementById(MedexCode);
    var chks;
    var i;
    if (chklst != null && typeof (chklst) != 'undefined') {
        chks = chklst.getElementsByTagName('input');
        for (i = 0; i < chks.length; i++) {
            if (chks[i].type.toUpperCase() == 'CHECKBOX') {
                if (type == "E") {
                    if (chks[i].checked) {
                        MedexVal = $(chks[i]).next().text();
                    }
                    chks[i].disabled = false;
                    $(chks[i]).parents('span').removeAttr('disabled');
                }
                else if (type == "U") {
                    if (chks[i].checked == true) {
                       if(MedexVal == "")
                       {
                          MedexVal = $(chks[i]).next().text();
                       }else 
                       {
                          MedexVal = MedexVal + "," + $(chks[i]).next().text();
                       }
                        
                    }
                }

                else if (type == "D") {
                    eledisable = true;
                    chks[i].disabled = true;
                    $(chks[i]).parents('span').attr('disabled', 'disabled');
                }
                else if (type == "AD") {
                    eledisable = true;
                    chks[i].checked = false;
                    if (chks[i].value == document.getElementById('HdnOldAttrVal').value) {
                        chks[i].checked = true;
                    }
                    chks[i].disabled = true;
                    $(chks[i]).parents('span').attr('disabled', 'disabled');
                }
            }
        }
    }


    var radiolst = document.getElementById(MedexCode);
    var radios;
    var i;
    if (radiolst != null && typeof (radiolst) != 'undefined') {
        radios = radiolst.getElementsByTagName('input');
        for (i = 0; i < radios.length; i++) {
            if (radios[i].type.toUpperCase() == 'RADIO') {
                if (type == "E") {
                    if (radios[i].checked) {
                        MedexVal = radios[i].value;
                    }
                    radios[i].disabled = false;
                    $(radios[i]).parents('span').removeAttr('disabled');
                }
                else if (type == "U") {
                    if (radios[i].checked == true) {
                        MedexVal = radios[i].value;
                    }
                }
                else if (type == "D") {
                    eledisable = true;
                    radios[i].disabled = true;
                    $(radios[i]).parents('span').attr('disabled', 'disabled');
                }
                else if (type == "AD") {
                    eledisable = true;
                    radios[i].checked = false;
                    if (radios[i].value == document.getElementById('HdnOldAttrVal').value) {
                        radios[i].checked = true;
                    }
                    radios[i].disabled = true;
                    $(radios[i]).parents('span').attr('disabled', 'disabled');
                }
            }
        }
    }
    if (type == "E") {
        if (MedexVal == "") {
            if (document.getElementById('HFMedexType').value.toString().toUpperCase() == "STANDARDDATE") {
                MedexVal = $('select[id*="' + MedexCode + '"]')[0].value + $('select[id*="' + MedexCode + '"]')[1].value + $('select[id*="' + MedexCode + '"]')[2].value
            }
            else MedexVal = chklst.value;
        }
        document.getElementById('HdnOldAttrVal').value = MedexVal;
        document.getElementById('HdnCurAttrId').value = MedexCode;
        return true;
    }
    if (type == "U") {
        if (MedexVal == "") {
            if (document.getElementById('HFMedexType').value.toString().toUpperCase() == "STANDARDDATE") {
                MedexVal = $('select[id*="' + MedexCode + '"]')[0].value + $('select[id*="' + MedexCode + '"]')[1].value + $('select[id*="' + MedexCode + '"]')[2].value
            }
            else MedexVal = chklst.value;
        }
        document.getElementById('HdnMedexVal').value = MedexVal;
        return true;
    }
    if (type == "D") {
        if (eledisable == false) {
        
         if(document.getElementById('HFMedexType').value=="File")
           {
                 MedexCode="FU" + MedexCode;
         }
         if (document.getElementById('HFMedexType').value.toString().toUpperCase() == "STANDARDDATE") {
             $('select[id*="' + MedexCode + '"]').attr('disabled', true);
         }
          else  document.getElementById(MedexCode).disabled = true;
        }
    }
    if (type == "AD") {
        if (eledisable == false) {
        
        if(document.getElementById('HFMedexType').value == "File")
            {
                MedexCode="FU" + MedexCode;       
            }
       
        if (document.getElementById('HFMedexType').value.toString().toUpperCase() == "STANDARDDATE") {
            $('select[id*="' + MedexCode + '"]')[0].value = document.getElementById('HdnOldAttrVal').value.toString().substring(0, 2)
            $('select[id*="' + MedexCode + '"]')[1].value = document.getElementById('HdnOldAttrVal').value.toString().substring(2, 4)
            $('select[id*="' + MedexCode + '"]')[2].value = document.getElementById('HdnOldAttrVal').value.toString().substring(4, 8)
            $('select[id*="' + MedexCode + '"]').attr('disabled', true);
        }
        else {
            document.getElementById(MedexCode).value = document.getElementById('HdnOldAttrVal').value
            document.getElementById(MedexCode).disabled = true;
        }
        }
    }
    return true;
}


function fnGetAuditTrail(MedexCode, ScreeningType, SubjectID, SelctedScreening,LocationCode) {
    var content = {};
    content.SubjectId = SubjectID;
    content.ScreeningDate = SelctedScreening;
    content.ScreeningType = ScreeningType;
    content.MedexCode = MedexCode;
    content.locationcode=LocationCode;
    $.ajax({
        type: "POST",
        url: "frmSubjectScreening_New.aspx/GetAuditTrailField",
        data: JSON.stringify(content),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(data) {
            var aaDataSet = [];

            if (data.d != "" && data.d != null) {

                data = JSON.parse(data.d);
                $('#lblfld').text("Attribute History Of :" + data[0].vMedExDesc);
                for (var Row = 0; Row < data.length; Row++) {
                    var InDataSet = [];
                    var Location=(data[Row].ActualTIME.split(" ")[2].split("(")[1] == "+05:30") ?"India standard time ":"Eastern standard time";  // Added By Dipen Shah to display actualTIME TimeZone Wise.
                    InDataSet.push(Row + 1, data[Row].vSubjectId, data[Row].dModifyScreeningDt, data[Row].vDefaultValue, data[Row].vRemarks, data[Row].vModifyBy, data[Row].dModifyOffSet,data[Row].ActualTIME);
                    aaDataSet.push(InDataSet);
                }
                if ($("#tblAuditField").children().length > 0) {
                    $("#tblAuditField").dataTable().fnDestroy();
                }
                $('#tblAuditField').prepend($('<thead>').append($('#tblAuditField tr:first'))).dataTable({
                    
                                "bJQueryUI": true,
                                "sPaginationType": "full_numbers",
                                "bLengthChange": true,
                                "iDisplayLength": 10,
                                "bProcessing": true,
                                "bSort": false,
                                "autoWidth": true,
                                "aaData": aaDataSet,
                                "bInfo": true,
                                "bDestroy": true,
                                "bScrollCollapse": true,
                                aLengthMenu: [
                                   [10, 25, 50, -1],
                                   [10, 25, 50, "All"]
                                ],
                    "sDom": "<Hr>t<Fip>",
                    "aoColumns": [
						                                        { "sTitle": "Sr. No." },
						                                        { "sTitle": "Subject ID" },
						                                        { "sTitle": "Screening Date" },
						                                        { "sTitle": "Value" },
						                                        { "sTitle": "Remarks" },
						                                        { "sTitle": "Modify By" },
						                                        { "sTitle": "Modify On" },
						                                        { "sTitle": Location} 
						                                        
					                                        ]
                });
                $('#tblAuditField tr:first').css('background-color', '#3A87AD');
                $find('MpeDIVFLD').show();

            }
            else {
                msgalert("No Audit Trail found");
                return false;
            }
        },
        failure: function(error) {
            msgalert(error);
        }
    });

}

//function ValidateReview(uName, uDesc, mod) {
//    uName = $("#lblUserName").text()
//    uDesc = $("#ddlProfile option:selected").text()
//    var content = {};
//    content.ScreeningHdrNo = $("HFScreeningHdrlNo").val()

//    $.ajax({
//        type: "post",
//        url: "frmSubjectScreening_New.aspx/ScreeningDataEntryCompleted",
//        data: JSON.stringify(content),
//        contentType: "application/json; charset=utf-8",
//        datatype: JSON,
//        async: false,
//        dataType: "json",
//        success: function (data) {
//            if (data.d.toUpperCase() == "COMPLETED") {
                
//            }
//            if (data.d == "NOT COMPLETED") {
//                alert("Data Entry of all the Screening group not Completed. Please Complete it First.")
//                return false;
//            }
//        }

//    });

//    if (mod != 2) {
//        if ($('#btnReviewEdit').is(":visible") != true) {
//            //var chk = document.getElementById('rblReviewCompleted');
//            var chkEle = document.getElementById('rblEligible');
//            var chkComplete = document.getElementById('rblEligibleReview');
//            var radios;
//            var i;
//            var flag = true;
//            if ($('#chkReviewCompleted').is(":visible") == true) {
//                flag = true
//            }
//            if ($('#rblEligible').is(":visible") == true) {
//                if (chkEle != null && typeof (chkEle) != 'undefined') {
//                    radios = chkEle.getElementsByTagName('input');
//                    for (i = 0; i < radios.length; i++) {
//                        if (radios[i].type.toUpperCase() == 'RADIO') {
//                            if (radios[i].checked == true) {
//                                flag = true
//                            }
//                        }
//                    }
//                }
//            } 
//            if ($('#rblEligibleReview').is(":visible") == true) {
//                if (chkComplete != null && typeof (chkComplete) != 'undefined') {
//                    radios = chkComplete.getElementsByTagName('input');
//                    for (i = 0; i < radios.length; i++) {
//                        if (radios[i].type.toUpperCase() == 'RADIO') {
//                            if (radios[i].checked == true) {
//                                flag = true
//                            }
//                        }
//                    }
//                }

//            }
//            if (flag == false) {
//                alert('Select Criteria And Then Click On OK.');
//                return false;
//            }
//        }
//    }
//    $('#lblSignername').text(uName);
//    $('#lblSignerDesignation').text(uDesc);
//    if (mod == 1) {
//        document.getElementById('btnAuthenticate').style.display = "";
//        document.getElementById('btnSaveuthenticate').style.display = "none"
//        //document.getElementById('trEle').style.display = "";
//    }
//    else if (mod == 2) {
//        document.getElementById('btnSaveuthenticate').style.display = "";
//        document.getElementById('btnAuthenticate').style.display = "none"
//        //document.getElementById('trEle').style.display = "";
//        var chk = document.getElementById('rblSaveEle');
//        var radios;
//        var i;
//        var flag = false;
//        if (chk != null && typeof (chk) != 'undefined') {
//            radios = chk.getElementsByTagName('input');
//            for (i = 0; i < radios.length; i++) {
//                if (radios[i].type.toUpperCase() == 'RADIO') {
//                    radios[i].checked = false
//                }
//            }
//        }
//    }
//    document.getElementById('txtPiRemarks').value = "";
//    $find('MPEId').show();

//    document.getElementById('txtPassword').focus();
//    return false;
//}

function ValidateReview(uName, uDesc, mod) {
    $('#rblSaveEle_1').attr('checked', false)
    $('#rblSaveEle_0').attr('checked', false)
        uName = $("#lblUserName").text()
        uDesc = $("#ddlProfile option:selected").text()
    var content = {};
    content.ScreeningHdrNo = $("#HFScreeningHdrlNo").val()

    $.ajax({
        type: "post",
        url: "frmSubjectScreening_New.aspx/ScreeningDataEntryCompleted",
        data: JSON.stringify(content),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            data = JSON.parse(data.d);
            if (data.toUpperCase() == "FALSE") {
                msgalert("Error While Validating ")
            }
            if (data.toUpperCase() == "COMPLETED") {
                if (mod != 2) {
                    if ($('#btnReviewEdit').is(":visible") != true) {
                        //var chk = document.getElementById('rblReviewCompleted');
                        var chkEle = document.getElementById('rblEligible');
                        var chkComplete = document.getElementById('rblEligibleReview');
                        var radios;
                        var i;
                        var flag = true;
                        if ($('#chkReviewCompleted').is(":visible") == true) {
                            flag = true
                        }
                        if ($('#rblEligible').is(":visible") == true) {
                            if (chkEle != null && typeof (chkEle) != 'undefined') {
                                radios = chkEle.getElementsByTagName('input');
                                for (i = 0; i < radios.length; i++) {
                                    if (radios[i].type.toUpperCase() == 'RADIO') {
                                        if (radios[i].checked == true) {
                                            flag = true
                                        }
                                    }
                                }
                            }
                        }
                        if ($('#rblEligibleReview').is(":visible") == true) {
                            if (chkComplete != null && typeof (chkComplete) != 'undefined') {
                                radios = chkComplete.getElementsByTagName('input');
                                for (i = 0; i < radios.length; i++) {
                                    if (radios[i].type.toUpperCase() == 'RADIO') {
                                        if (radios[i].checked == true) {
                                            flag = true
                                        }
                                    }
                                }
                            }

                        }
                        if (flag == false) {
                            msgalert('Select Criteria And Then Click On OK.');
                            return false;
                        }
                    }
                }
                $('#lblSignername').text(uName);
                $('#lblSignerDesignation').text(uDesc);
                if (mod == 1) {
                    document.getElementById('btnAuthenticate').style.display = "";
                    document.getElementById('btnSaveuthenticate').style.display = "none"
                    //document.getElementById('trEle').style.display = "";
                }
                else if (mod == 2) {
                    document.getElementById('btnSaveuthenticate').style.display = "";
                    document.getElementById('btnAuthenticate').style.display = "none"
                    //document.getElementById('trEle').style.display = "";
                    var chk = document.getElementById('rblSaveEle');
                    var radios;
                    var i;
                    var flag = false;
                    if (chk != null && typeof (chk) != 'undefined') {
                        radios = chk.getElementsByTagName('input');
                        for (i = 0; i < radios.length; i++) {
                            if (radios[i].type.toUpperCase() == 'RADIO') {
                                radios[i].checked = false
                            }
                        }
                    }
                }
                document.getElementById('txtPiRemarks').value = "";
                $find('MPEId').show();

                document.getElementById('txtPassword').focus();
                return false;
            }
            if (data.toUpperCase() == "NOT COMPLETED") {
                msgalert("Data Entry of All the Groups Is Not Completed Or DCF Is Pending!")
                return false;
            }
        }

    });


}


function fnCheckChangeEvent(obj) {

    var radiolst = obj;
    var radios;
    var i;
    if (radiolst != null && typeof (radiolst) != 'undefined') {
        radios = radiolst.getElementsByTagName('input');
        for (i = 0; i < radios.length; i++) {
            if (radios[i].type.toUpperCase() == 'RADIO') {
                if (radios[i].checked == true) {
                    $('#hdnchkval').val(radios[i].value);
                }
            }
        }
    }
}

function fnEligible(obj) {
    var radiolst = obj;
    var radios;
    var i;
    if (radiolst != null && typeof (radiolst) != 'undefined') {
        radios = radiolst.getElementsByTagName('input');
        for (i = 0; i < radios.length; i++) {
            if (radios[i].type.toUpperCase() == 'RADIO') {
                if (radios[i].checked == true) {
                    $('#hdnchkval').val(radios[i].value);
                }
            }
        }
    }
}


function fnReviewEdit(type) {

    if (type == 'R') {
        $('#btnOk').removeAttr("disabled");
        //            $('#rblReviewCompleted').removeAttr("disabled");
        //            var radiolst = document.getElementById('rblReviewCompleted');
        //            var radios;
        //            var i;
        //            if (radiolst != null && typeof (radiolst) != 'undefined') {
        //                radios = radiolst.getElementsByTagName('input');
        //                for (i = 0; i < radios.length; i++) {
        //                    if (radios[i].type.toUpperCase() == 'RADIO') {
        //                           radios[i].disabled = false;
        //                            if (radios[i].checked == true)
        //                             {
        //                               $('#hdnchkval').val(radios[i].value);
        //                              } 
        //                        }
        //                   }
        //             }        
    }
    if (type == 'D') {
        //$('#rblEligible').removeAttr("disabled");
        var radiolst = document.getElementById('rblEligible');
        var radios;
        var i;
        if (radiolst != null && typeof (radiolst) != 'undefined') {
            radios = radiolst.getElementsByTagName('input');
            for (i = 0; i < radios.length; i++) {
                if (radios[i].type.toUpperCase() == 'RADIO') {
                    radios[i].disabled = false;
                    $(radios[i]).parents('span').removeAttr('disabled')
                }
            }
        }
    }
    if (type == 'C') {
        //$('#rblEligibleReview').removeAttr("disabled");
        var radiolst = document.getElementById('rblEligibleReview');
        var radios;
        var i;
        if (radiolst != null && typeof (radiolst) != 'undefined') {
            radios = radiolst.getElementsByTagName('input');
            for (i = 0; i < radios.length; i++) {
                if (radios[i].type.toUpperCase() == 'RADIO') {

                    radios[i].disabled = false;
                    $(radios[i]).parents('span').removeAttr('disabled')
                }
            }
        }
    }
    document.getElementById('btnOk').style.display = "";
}

function fnReviewAudit(ScreeningHdrNo,LocationCode, Workflow) {
    var content = {};
    content.ScreeningHdrNo = ScreeningHdrNo;
    content.LocationCode=LocationCode;

    $.ajax({
        type: "POST",
        url: "frmSubjectScreening_New.aspx/GetAuditTrailReview",
        data: JSON.stringify(content),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(data) {
            var aaDataSet = [];
            if (data.d != "" && data.d != null) {
                data = JSON.parse(data.d);
                $('#lblMedexDescription').text("Review History of " + data[0].vSubjectId + " On Screening Date " + data[0].dScreenDate);
                for (var Row = 0; Row < data.length; Row++) {
                    var InDataSet = [];
                    var Location=(data[Row].ActualTime.split(" ")[2].split("(")[1] == "+05:30") ?"India standard time ":"Eastern standard time";  // Added By Dipen Shah to display actualTIME TimeZone Wise.
                    InDataSet.push(Row + 1, data[Row].vRemark, data[Row].iModifyBy, data[Row].dModifyOffSet, data[Row].cIsEligible,data[Row].ActualTime);
                    aaDataSet.push(InDataSet);
                }
                if ($("#tblAudit").children().length > 0) {
                    $("#tblAudit").dataTable().fnDestroy();
                }
                $('#tblAudit').prepend($('<thead>').append($('#tblAudit tr:first'))).dataTable({
                    "bPaginate": true,
                    "sPaginationType": "full_numbers",
                    "bDestory": true,
                    "bRetrieve": true,
                    "aaData": aaDataSet,
                    "sDom": "<Hr>t<Fip>",
                    "aoColumns": [
						                                        { "sTitle": "Sr. No." },
						                                        { "sTitle": "Review Comments" },
						                                        { "sTitle": "Modify By" },
						                                        { "sTitle": "Modify On" },
						                                        { "sTitle": "Is Eligible"},
						                                        {"sTitle" :  Location}
					                                        ]
                });
                $('#tblAudit tr:first').css('background-color', '#3A87AD');
                $find('MPEDeviation').show();

            }
            else {
                msgalert("No Review History found");
                return false;
            }
        },
        failure: function(error) {
            msgalert(error);
        }
    });

}

function displayProjectInfo(ele, parent) {

    if (ele.src.toString().toUpperCase().search("EXPAND") != -1) {
        $("#" + parent).slideToggle(400);
        ele.src = "images/collapse_blue.jpg";

    }
    else {
        $("#" + parent).slideToggle(400);
        ele.src = "images/expand.jpg";
    }
}

function createDivforPrintForData(Template)  // for MSR Structure Print Or Project Specific Structure
{
    var j = 0, i = 0, k = 0, t = 0;
    var str = "";
    var res = "";
    var arrMedexGroup = new Array();
    var arrMedexSubGroup = new Array();

    //for check MedexGroupcode 
    for (i = 0; i < parseInt(Template.length); i++) {
        if (i == 0) {
            arrMedexGroup[j] = Template[i].vMedExGroupCode + "##" + Template[i].vMedExGroupDesc;
            j++;
        }
        else {
            for (k = 0; k < arrMedexGroup.length; k++) {
                if (arrMedexGroup[k].split("##")[0] != Template[i].vMedExGroupCode) {
                    res = true;

                }
                else {
                    res = false
                    break;
                }

            }
            if (res == true) {
                arrMedexGroup[j] = Template[i].vMedExGroupCode + "##" + Template[i].vMedExGroupDesc;
                j++;
                res = "";
            }
        }
    }
    //for MedexSubgroupcode
    j = 0;
    for (i = 0; i < parseInt(Template.length); i++) {
        if (i == 0) {
            arrMedexSubGroup[j] = Template[i].vMedExGroupCode + "++" + Template[i].vMedExSubGroupCode + "##" + Template[i].vMedExSubGroupDesc;
            j++;
        }
        else {
            for (t = 0; t < arrMedexGroup.length; t++) {
                if (arrMedexGroup[t].split("##")[0] == Template[i].vMedExGroupCode) {
                    for (k = 0; k < arrMedexSubGroup.length; k++) {
                        if (arrMedexSubGroup[k].split("++")[1].split("##")[0] != Template[i].vMedExSubGroupCode || arrMedexSubGroup[k].split("++")[0] != Template[i].vMedExGroupCode) {
                            res = true;

                        }
                        else {
                            res = false
                            break;
                        }

                    }
                }
            }
            if (res == true) {
                arrMedexSubGroup[j] = Template[i].vMedExGroupCode + "++" + Template[i].vMedExSubGroupCode + "##" + Template[i].vMedExSubGroupDesc;
                j++;
                res = "";
            }
        }
    }
    //For Display Medex
    var tempStr = "";
    tempStr += '<table width="100%" style=" border-style:solid;border-color:black;border-width:1px;" cellspacing="0"><tr><td>'
    for (i = 0; i < arrMedexGroup.length; i++) {
        tempStr += '<table width="100%" cellspacing="0"><tr><th style="BACKGROUND-COLOR: #008ecd; font-weight: bold; color:white;  page-break-inside:avoid;" colspan="2">' + arrMedexGroup[i].split("##")[1].toString() + '</th></tr>';

        for (j = 0; j < arrMedexSubGroup.length; j++) {
            if (arrMedexGroup[i].split("##")[0] == arrMedexSubGroup[j].split("++")[0]) {
                if (arrMedexSubGroup[j].split("##")[1].toString() == 'Default') { }
                else {
                    tempStr += '<tr><td colspan="2"style="BACKGROUND-COLOR: #ffcc66; font-weight: bold;">' + arrMedexSubGroup[j].split("##")[1].toString() + '</td></tr>';
                }
                for (k = 0; k < Template.length; k++) {
                    if (arrMedexGroup[i].split("##")[0] == Template[k].vMedExGroupCode) {
                        if (arrMedexSubGroup[j].split("++")[1].split("##")[0] == Template[k].vMedExSubGroupCode) {
                            if (Template[k].vMedExType == "Label") {
                                tempStr += '<tr><td style="text-align: left;width:30%; border-bottom: 1px solid black;" colspan="2">' + createObjectForData(Template[k].vMedExType, Template[k].vDefaultValue, Template[k].vMedExValues, Template[k].vMedExCode) + " " + Template[k].vUOM + '</td></tr>';
                            }
                            else {
                                tempStr += '<tr><td class="Label " style="text-align: left;width:30%; border-bottom: 1px solid black;">' + Template[k].vMedExDesc + ' :</td><td style="text-align: left;width:30%; border-bottom: 1px solid black;">' + createObjectForData(Template[k].vMedExType, Template[k].vDefaultValue, Template[k].vMedExValues, Template[k].vMedExCode) + " " + Template[k].vUOM + '</td></tr>';
                            }
                        }
                    }
                }
            }

        }
    }
    tempStr += '</table></td></tr></table>';
    document.getElementById('HdnPrintString').value = tempStr;
    setTimeout(function () {
        $("#ddlGroup").removeAttr("disabled")
    }, 10);
}


function createDivforPrint(Template)  // for MSR Structure Print Or Project Specific Structure
{
    var j = 0, i = 0, k = 0, t = 0;
    var str = "";
    var res = "";
    var arrMedexGroup = new Array();
    var arrMedexSubGroup = new Array();

    //for check MedexGroupcode 
    for (i = 0; i < parseInt(Template.length); i++) {
        if (i == 0) {
            arrMedexGroup[j] = Template[i].vMedExGroupCode + "##" + Template[i].vMedExGroupDesc;
            j++;
        }
        else {
            for (k = 0; k < arrMedexGroup.length; k++) {
                if (arrMedexGroup[k].split("##")[0] != Template[i].vMedExGroupCode) {
                    res = true;

                }
                else {
                    res = false
                    break;
                }

            }
            if (res == true) {
                arrMedexGroup[j] = Template[i].vMedExGroupCode + "##" + Template[i].vMedExGroupDesc;
                j++;
                res = "";
            }
        }
    }
    //for MedexSubgroupcode
    j = 0;
    for (i = 0; i < parseInt(Template.length); i++) {
        if (i == 0) {
            arrMedexSubGroup[j] = Template[i].vMedExGroupCode + "++" + Template[i].vMedExSubGroupCode + "##" + Template[i].vMedExSubGroupDesc;
            j++;
        }
        else {
            for (t = 0; t < arrMedexGroup.length; t++) {
                if (arrMedexGroup[t].split("##")[0] == Template[i].vMedExGroupCode) {
                    for (k = 0; k < arrMedexSubGroup.length; k++) {

                        if (arrMedexSubGroup[k].split("++")[1].split("##")[0] != Template[i].vMedExSubGroupCode || arrMedexSubGroup[k].split("++")[0] != Template[i].vMedExGroupCode) {
                            res = true;

                        }
                        else {
                            res = false
                            break;
                        }

                    }
                }
            }
            if (res == true) {
                arrMedexSubGroup[j] = Template[i].vMedExGroupCode + "++" + Template[i].vMedExSubGroupCode + "##" + Template[i].vMedExSubGroupDesc;
                j++;
                res = "";
            }
        }
    }
    //For Display Medex
    var tempStr = "";
    tempStr += '<table width="100%" style=" border-style:solid;border-color:black;border-width:1px;" cellspacing="0"><tr><td>'
    for (i = 0; i < arrMedexGroup.length; i++) {
        tempStr += '<table width="100%" style=" border-collapse:collapse;" cellspacing="0"><tr><th style="BACKGROUND-COLOR: #008ecd; font-weight: bold; color:white;page-break-inside:avoid;" colspan="2" text-align: center;" >' + arrMedexGroup[i].split("##")[1].toString() + '</th></tr>';

        for (j = 0; j < arrMedexSubGroup.length; j++) {
            if (arrMedexGroup[i].split("##")[0] == arrMedexSubGroup[j].split("++")[0]) {
                if (arrMedexSubGroup[j].split("##")[1].toString() == 'Default') { }
                else {
                    tempStr += '<tr><td colspan="2"style="BACKGROUND-COLOR: #ffcc66; font-weight: bold;">' + arrMedexSubGroup[j].split("##")[1].toString() + '</td></tr>';
                }
                for (k = 0; k < Template.length; k++) {
                    if (arrMedexGroup[i].split("##")[0] == Template[k].vMedExGroupCode) {
                        if (arrMedexSubGroup[j].split("++")[1].split("##")[0] == Template[k].vMedExSubGroupCode) {
                            if (Template[k].vMedExType == "Label") {
                                tempStr += '<tr><td style="text-align: left;width:30%; border-bottom: 1px solid black;" colspan="2">' + createObject(Template[k].vMedExType, Template[k].vMedExValues, Template[k].vDefaultValue, Template[k].vMedExCode) + " " + Template[k].vUOM + '</td></tr>';
                            }
                            else {
                                tempStr += '<tr><td style="text-align: left;width:70%; border-bottom: 1px solid black;" class="Label ">' + Template[k].vMedExDesc + ' :</td><td style="text-align: left;width:30%; border-bottom: 1px solid black;">' + createObject(Template[k].vMedExType, Template[k].vMedExValues, Template[k].vDefaultValue, Template[k].vMedExCode) + " " + Template[k].vUOM + '</td></tr>';
                            }
                        }
                    }
                }
            }

        }
    }
    tempStr += '</table></td></tr></table>';
    document.getElementById('HdnPrintString').value = tempStr;
    setTimeout(function () {
        $("#ddlGroup").removeAttr("disabled")
    }, 10);
}

function fnGetPrintString(mode) {

    $("#ddlGroup").attr("disabled", "disabled")
    var temp = $.parseJSON(document.getElementById('HdnPrint').value)
    if ($("#ddlScreeningDate option:selected").val() == 'N') {
        mode = 'N'
    }
    else {
        mode = 'S'
    }
    if (mode == "S") {
        createDivforPrintForData(temp);
        copyData(mode);
        return true;
    }
    else if (mode == "N") {
        createDivforPrint(temp);
        copyData(mode);
        return true;
    }

    
    return false;
}


function createObjectForData(type, MedExResult, MedexValues, Medexcode) {
    var str = "";
    var temp_MedexValues = MedexValues;
    var i = 0;
    if (type.toUpperCase() == "TEXTBOX" || type.toUpperCase() == "STANDARDDATE") {
        str = "<input type='TEXTBOX'  id='txt_" + Medexcode + "' value='" + MedExResult + "' class='TextBox ' disabled='true'/>"
        return str;
    }
    if (type.toUpperCase() == "CHECKBOX") {
        for (i = 0; i < temp_MedexValues.split(",").length; i++) {
            str += "<input type='CHECKBOX'  id='chk_" + Medexcode + "_" + i + "'"

            for (j = 0; j < MedExResult.split(",").length; j++) {
                if (temp_MedexValues.split(",")[i].toString() == MedExResult.split(",")[j].toString()) {
                    str += " checked='checked'"
                }
            }
            str += " class='checkBoxlist ' disabled='true'/>"
            str += "<label for='chk_" + Medexcode + "_" + i + "'>" + temp_MedexValues.split(",")[i].toString() + "</label>"
        }
        return str;
    }
    if (type.toUpperCase() == "RADIO") {
        for (i = 0; i < temp_MedexValues.split(",").length; i++) {
            str += "<input type='RADIO'  id='rad_" + Medexcode + "_" + i.toString() + "' name='rad_" + Medexcode + "'"
            if (temp_MedexValues.split(",")[i].toString() == MedExResult) {
                str += " checked=checked"
            }
            str += " class='RadioButton ' disabled='true'/>"
            str += "<label for='rad_" + Medexcode + "_" + i.toString() + "' class='Label '>" + temp_MedexValues.split(",")[i].toString() + "</label>"
        }
        return str;
    }
    if (type.toUpperCase() == "COMBOBOX") {
        str = "<select class ='dropDownList' id='cmb_" + Medexcode + "' disabled='true'>";
        for (i = 0; i < temp_MedexValues.split(",").length; i++) {
            str += "<option value='" + i + "_" + Medexcode + "'"
            if (temp_MedexValues.split(",")[i].toString() == MedExResult) {
                str += " selected=selected"
            }
            str += ">" + temp_MedexValues.split(",")[i].toString()
            str += " </option>"
        }
        str += "</select>";
        return str;
    }
    if (type.toUpperCase() == "FILE") {
        str = "<input type='FILE'  id='file_" + Medexcode + "' disabled='true'/>"
        return str;
    }
    if (type.toUpperCase() == "DATETIME" || type.toUpperCase() == "TIME") {
        str = "<input type='TEXTBOX'  id='txt_" + Medexcode + "' value='" + MedExResult + "'/>"
        return str;
    }
    if (type.toUpperCase() == "TEXTAREA") {
        if (MedExResult.length >= 90) {
            str = "<textarea id='txt_" + Medexcode + "' class='TextArea ' disabled='true' style='width:98% ; overflow:hidden; height:120px;'>" + MedExResult + "</textarea>"
            return str;
        }
        else {
            str = "<textarea id='txt_" + Medexcode + "' class='TextArea ' disabled='true' style='width:98% ; overflow:hidden;  height:50px; '>" + MedExResult + "</textarea>"
            return str;
        }
    }
    if (type.toUpperCase() == 'LABEL') {
        str = "<label id='lbl_" + Medexcode + "' class='Label '>" + MedExResult + "</label>"
        return str;
    }
    if (type.toUpperCase() == "FORMULA") {
        str = "<input type='TEXTBOX'  id='txt_" + Medexcode + "' value='" + MedExResult + "' class='TextBox ' disabled='true'/>"
        return str;
    }
}

function copyData(mode) {
    var str = "";

    //            var doc = new jsPDF();
    //            doc.text(20, 20, 'Hello world!');
    //            doc.text(20, 30,document.getElementById('<%=mainDiv.ClientId %>').innerHTML);
    //            doc.addPage();
    //            doc.text(20, 20, 'Do you like that?');
    //            doc.output('datauri');
    if (mode == "N") {

        if ($('#tr_WorkSpace').is(":visible") != true) {

            str += "<table style='border: solid 5 #008ecd; width: 100%;'>";
            str += "<tr>";
            str += "<td align='center' style='font-size: larger; text-align: center; font-weight: bolder; width: 80%;'>";
            str += "Medical Screening Record Form";
            str += "</td>";
            str += "<td style='width: 20%;'>";
            str += "<img id='ImgLogo1' src='~/images/lambda_logo.jpg' alt='Lambda Logo' />";
            str += "</td>";
            str += "</tr>";
            str += "</table>";
            document.getElementById('hfHeaderText').value = str.toString().trim();
        }
        else {

            str += "<table style='border: solid 5 #008ecd; width: 100%;'>";
            str += "<tr>";
            str += "<td style='width: 70%;'>";
            str += "<table style='width: 100%;'>";
            str += "<tr>";
            str += "<td>";
            str += "<td colspan='2' align='center' style='font-size: larger; text-align: center; font-weight: bolder;'>";
            str += "Medical Screening Record Form";
            str += "</td>";
            str += "</tr>";
            str += "<tr>";
            str += "<td style='text-align: right; width: 8%;' align='left'>";
            str += "Sponsor Name:";
            str += "</td>";
            str += "<td style='border: thin solid #000000; text-align: left; width: 50%;'>";
            str += "<label id='lblProjectNo'>";
            str += "Lambda" + "</label>";
            str += "</td>";
            str += "</tr>";
            str += "<tr>";
            str += "<td style='text-align: right; width: 8%;' align='left'>";
            str += "Project No:";
            str += "</td>";
            str += "<td style='border: thin solid #000000; text-align: left; width: 50%;'>";
            str += " <label id='lblProjectNo'>";
            str += "047" + "</label>";
            str += "</td>";
            str += "</tr>";
            str += "</table>";
            str += "</td>";
            str += "<td style='width: 20%;'>";
            str += "<img id='ImgLogo1' src='~/images/lambda_logo.jpg' alt='Lambda Logo' />";
            str += "</td>";
            str += "</tr>";
            str += "</table>";

            document.getElementById('hfHeaderText').value = str.toString().trim();
        }
    }
    else {
        var dt = $('#rblScreeningDate :checked').val().substr(0, 10).split("/");
        var ScreeningDate = dt[1] + "-" + cMONTHNAMES[dt[0] - 1] + "-" + dt[2].toString().substr(0, 4)

        if ($('#tr_WorkSpace').is(":visible") == true) {
            var Project = $('#txtproject').val().substr(1, $('#txtproject').val().indexOf("]") - 1);

            str += "<table style='border: solid 5 #008ecd; width: 100%;'>";
            str += "<tr>";
            str += "<td  style='width: 80%;'>";
            str += "<table>";
            str += "<tr>";
            str += "<td colspan='2' align='center' style='font-size: larger; text-align: center; font-weight: bolder;'>";
            str += "Medical Screening Record Form";
            str += "</td>";
            str += "</tr>";
            str += "<tr>";
            str += "<td style='text-align: right; width: 8%;' align='left'>";
            str += "Subject ID:";
            str += "</td>";
            str += "<td style='border: thin solid #000000; text-align: left; width: 50%;'>";
            str += "<label id='lblSubjectId'>";
            str += $('#HSubjectId').val() + "</label>";
            str += "</td>";
            str += "</tr>";
            str += "<tr>";
            str += "<td style='text-align: right; width: 8%;' align='left'>";
            str += "Screening Date";
            str += "</td>";
            str += "<td style='border: thin solid #000000; text-align: left; width: 50%;'>";
            str += "<label id='lblScreeningDate'>";
            str += ScreeningDate + "</label>";
            str += "</td>";
            str += "</tr>";
            str += "<tr>";
            str += "<td style='text-align: right; width: 8%;' align='left'>";
            str += "Project No";
            str += "</td>";
            str += "<td style='border: thin solid #000000; text-align: left; width: 50%;'>";
            str += "<label id='lblProject'>";
            str += Project + "</label>";
            str += "</td>";
            str += "</tr>";
            str += "</table>";
            str += "</td>";
            str += "<td style='width: 20%;'>";
            str += "<img id='ImgLogo1' src='~/images/lambda_logo.jpg' alt='Lambda Logo' />";
            str += "</td>";
            str += "</tr>";
            str += "</table>";
            document.getElementById('hfHeaderText').value = str.toString().trim();
        }
        else {
            str += "<table style='border: solid 5 #008ecd; width: 100%;'>";
            str += "<tr>";
            str += "<td  style='width: 80%;'>";
            str += "<table>";
            str += "<tr>";
            str += "<td colspan='2' align='center' style='font-size: larger; text-align: center; font-weight: bolder;'>";
            str += "Medical Screening Record Form";
            str += "</td>";
            str += "</tr>";
            str += "<tr>";
            str += "<td style='text-align: right; width: 8%;' align='left'>";
            str += "Subject ID:";
            str += "</td>";
            str += "<td style='border: thin solid #000000; text-align: left; width: 50%;'>";
            str += "<label id='lblSubjectId'>";
            str += $('#HSubjectId').val() + "</label>";
            str += "</td>";
            str += "</tr>";
            str += "<tr>";
            str += "<td style='text-align: right; width: 8%;' align='left'>";
            str += "Screening Date";
            str += "</td>";
            str += "<td style='border: thin solid #000000; text-align: left; width: 50%;'>";
            str += "<label id='lblScreeningDate'>";
            str += ScreeningDate + "</label>";
            str += "</td>";
            str += "</tr>";
            str += "</table>";
            str += "</td>";
            str += "<td style='width: 20%;'>";
            str += "<img id='ImgLogo1' src='~/images/lambda_logo.jpg' alt='Lambda Logo' />";
            str += "</td>";
            str += "</tr>";
            str += "</table>";
            document.getElementById('hfHeaderText').value = str.toString().trim();
        }

    }
    //btnPdfHide.click()
    //document.getElementById('btnPdfHide').click()
    
}

function createObject(type, MedexValues, MedexDefaultValues, Medexcode) {
    var str = "";
    var temp_MedexValues = MedexValues;
    var i = 0;
    if (type.toUpperCase() == "TEXTBOX" || type.toUpperCase() == "STANDARDDATE") {
        str = "<input type='TEXTBOX'  id='txt_" + Medexcode + "' value='" + MedexDefaultValues + "' class='TextBox ' disabled='true'/>"
        return str;
    }
    if (type.toUpperCase() == "CHECKBOX") {
        for (i = 0; i < temp_MedexValues.split(",").length; i++) {
            str += "<input type='CHECKBOX'  id='chk_" + Medexcode + "_" + i + "'"
            for (j = 0; j < MedexDefaultValues.split(",").length; j++) {
                if (temp_MedexValues.split(",")[i].toString() == MedexDefaultValues.split(",")[j].toString()) {
                    str += " checked='checked'"
                }
            }
            str += " class='checkBoxlist ' disabled='true'/>"
            str += "<label for='chk_" + Medexcode + "_" + i + "'>" + temp_MedexValues.split(",")[i].toString() + "</label>"
        }
        return str;
    }
    if (type.toUpperCase() == "RADIO") {
        for (i = 0; i < temp_MedexValues.split(",").length; i++) {
            str += "<input type='RADIO'  id='rad_" + Medexcode + "_" + i.toString() + "' name='rad_" + Medexcode + "'"
            if (temp_MedexValues.split(",")[i].toString() == MedexDefaultValues) {
                str += " checked=checked"
            }
            str += " class='RadioButton ' disabled='true'/>"
            str += "<label for='rad_" + Medexcode + "_" + i.toString() + "' class='Label '>" + temp_MedexValues.split(",")[i].toString() + "</label>"
        }
        return str;
    }
    if (type.toUpperCase() == "COMBOBOX") {

        str = "<select class ='dropDownList' id='cmb_" + Medexcode + "' disabled='true'>";
        for (i = 0; i < temp_MedexValues.split(",").length; i++) {
            str += "<option value='" + i + "_" + Medexcode + "'"
            if (temp_MedexValues.split(",")[i].toString() == MedexDefaultValues) {
                str += " selected=selected"
            }
            str += ">" + temp_MedexValues.split(",")[i].toString()
            str += " </option>"
        }
        str += "</select>";
        return str;
    }
    if (type.toUpperCase() == "FILE") {
        str = "<input type='FILE'  id='file_" + Medexcode + "' disabled='true'/>"
        return str;
    }
    if (type.toUpperCase() == "DATETIME" || type.toUpperCase() == "TIME") {
        str = "<input type='TEXTBOX'  id='txt_" + Medexcode + "'   class='TextBox ' disabled='true'/>"
        return str;
    }
    if (type.toUpperCase() == "TEXTAREA") {
        //if (MedExResult.length >= 90) {
        //    str = "<textarea id='txt_" + Medexcode + "' class='TextArea ' disabled='true' style='width:98% ; height:120px;'> " + MedexDefaultValues + "</textarea>"
        //    return str;
        //}
        //else {
        //    str = "<textarea id='txt_" + Medexcode + "' class='TextArea ' disabled='true' style='width:98% ; height:50px;'> " + MedexDefaultValues + "</textarea>"
        //    return str;
        //}

    }
    if (type.toUpperCase() == 'LABEL') {
        str = "<label id='lbl_" + Medexcode + "' class='Label '  Width='98%'>" + MedexValues + "</label>"
        return str;
    }
    if (type.toUpperCase() == "FORMULA") {
        str = "<input type='TEXTBOX'  id='txt_" + Medexcode + "' value='" + MedexDefaultValues + "' class='TextBox ' disabled='true'/>"
        return str;
    }
}


function updateCountdown(strFire) {
    var remaining, ControlCollection = {};
    if (strFire === "load") {
        ControlCollection = $('.crfentrycontrol');
        for (var cnt = 0; cnt < ControlCollection.length; cnt++) {
            remaining = 2000 - ($(ControlCollection[cnt]).val().length);
            $($(ControlCollection[cnt]).closest('td')).find('.CntTextArea').text(remaining + ' characters remaining.');
        }
    }
    else if (strFire.type === "keyup" || strFire.type === "change") {
        remaining = 2000 - $(this).val().length;
        $($(this).closest('td')).find('.CntTextArea').text(remaining + ' characters remaining.');
        if (remaining == 0) {
            return false;
        }
    }

}

function textareakeypress() {
    var remaining = 3000 - $(this).val().length;
    if (remaining == 0) {
        return false;
    }
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode == 13) {
        return false;
    }
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        if (charCode != 46) {
            return false;
        }

    return true;
}

function fnReviewECG() {  

    var vWorkSpaceId, dScreeningDate, vSubjectId;
    if ($("#txtSubject").val() == "" && $("#HSubjectId").val() == "") {
        msgalert("Please Select Subject to review ECG!");
        return false;
    }
    if ($("#chkScreeningType_0").is(':checked') == true) { // Generic Screening
        if ($("#ddlScreeningDate").val() == "N") { //New Screening
            msgalert("No ECG detail can be available for New Screening!");
            return false;
        }
        else if ($("#ddlScreeningDate").val() == "M") { //Select Screening
            msgalert("Please Select Screening Date!");
            return false;
        }
        else {
            dScreeningDate = $("#ddlScreeningDate").val();
        }
        vWorkSpaceId = "Default";
        vSubjectId = $("#HSubjectId").val();
        //return true;
    }
    else { // Project Specific Screening
        if ($("#txtproject").val() == "") {
            msgalert("Please Select Project for Project Specific Screening to review ECG!");
            return false;
        }        
        if ($("#ddlScreeningDate").val() == "N") { //New Screening
            msgalert("No ECG can be available for New Screening!");
            return false;
        }
        else if ($("#ddlScreeningDate").val() == "M") { //Select Screening
            msgalert("Please Select Screening Date!");
            return false;
        }
        else {
            dScreeningDate = $("#ddlScreeningDate").val();
        }
        vWorkSpaceId = $("#HProjectId").val();
        vSubjectId = $("#HSubjectId").val();
        //return true;
    }

    var ECGAjaxData = {
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: JSON,
        type: "POST",
        data: '{"vWorkSpaceId":"' + vWorkSpaceId + '","vSubjectId":"' + vSubjectId + '","dScreeningDate":"' + dScreeningDate + '"}',
        url: "frmSubjectScreening_New.aspx/ReviewECG",
        success: successECGReviewDetail,
        error: errorECGReviewDetail
    }
    getECGReviewDetail(ECGAjaxData.async, ECGAjaxData.contentType, ECGAjaxData.dataType, ECGAjaxData.type, ECGAjaxData.data, ECGAjaxData.url, ECGAjaxData.success, ECGAjaxData.error)
}

var getECGReviewDetail = function (async, contentType, dataType, type, data, url, success, error) {
    $.ajax({
        async: async,
        contentType: contentType,
        dataType: dataType,
        type: type,
        data: data,
        url: url,
        success: success,
        error: error
    });
}

function successECGReviewDetail(jsonData) {
    //var data = JSON.parse(jsonData.d);
    $("#ifviewDocument").attr("src", "ECG/AH16-03578__VBR_131128.pdf");
    $find('mdDocument').show();   
    
}

function errorECGReviewDetail(e) {
    msgalert("ERROR : " + JSON.parse(e.response).Message)
}

$("#btnExitECG").bind("click", function () {
    $("#ifviewDocument").attr("src", "");
    $find('mdDocument').hide()
});