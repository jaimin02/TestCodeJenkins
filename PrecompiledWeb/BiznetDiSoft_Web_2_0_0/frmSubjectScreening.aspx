<%@ page language="VB" autoeventwireup="false" theme="StyleBlue" inherits="frmSubjectScreening, App_Web_xbhimv2u" enableEventValidation="false" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <%--<title>Subject Medical Examination </title>--%>
    <link href="App_Themes/StyleBlue/StyleBlue.css" rel="stylesheet" type="text/css" />

    <script src="Script/General.js" language="javascript" type="text/javascript"></script>

    <script src="Script/Validation.js" language="javascript" type="text/javascript"></script>

    <script src="Script/popcalendar.js" language="javascript" type="text/javascript"></script>

    <script src="Script/AutoComplete.js" language="javascript" type="text/javascript"></script>

    <script src="Script/jquery.min.js" type="text/javascript"></script>

    <script src="Script/jquery-1.4.3.min.js" type="text/javascript"></script>

    <link href="App_Themes/sweetalert.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Script/sweetalert.js"></script>

    <script language="javascript" type="text/javascript">


        window.onbeforeunload = function () {

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
            PageMethods.DeleteScreeningtmpTable(hmed, hSubID, function () { document.getElementById('HSubjectId').value = '', function (err) { alert(err); } })

        }
        function closewindow() {
            self.close();
        }


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
                    txt.focus();
                    msgalert(msg);
                }
            }

            if (HighRange != 0 || LowRange != 0) {
                if (txt.value > HighRange || txt.value < LowRange) {

                    msgalert('Out Of Range! Range Must be Between ' + LowRange + ' to ' + HighRange);
                }
            }

        }

        function ValidateTextboxNumeric(checktype, txt, msg, HighRange, LowRange, validation, length, NumericScale) {
            var value = txt.value.trim();
            var scaleForNumeric = value.toString().split(".");
            if (validation == 'NU') {
                if (scaleForNumeric.length > 2) {
                    msgalert('Enter Data Not In Correct Format !');
                    return false;
                }
                if (scaleForNumeric[0].length > parseInt(length)) {
                    msgalert('Out Of Maximum Length! Value length Must Less or Equal to  ' + parseInt(length) + '. Please Enter the value in Proper Range !')
                    return false;
                }
                if (typeof (scaleForNumeric[1]) != 'undefined') {
                    if (scaleForNumeric[1].length > NumericScale) {
                        msgalert('scale Not be greate than ' + NumericScale + '. Please Enter the value in Proper Range !');
                        return false;
                    }
                }

                var decimalRegEx = /^[+|-]?\d*(\.\d+)?$/;
                if (!decimalRegEx.test(value)) {
                    msgalert('Enter Value Not in Correct Format !');
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
                    txt.focus();
                    msgalert(msg);
                }
            }

            if (HighRange != 0 || LowRange != 0) {
                if (parseFloat(txt.value) > HighRange || parseFloat(txt.value) < LowRange) {

                    msgalert('Out Of Range! Range Must be Between ' + LowRange + ' to ' + HighRange);
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

        function JustAlert(UserName, MedExCode) {
            msgalert('Have You Checked "Past History" , "General Exam." And "Systemic Exam." !');
            document.getElementById(MedExCode).value = UserName;
        }

        function SetUserName(UserName, MedExCode) {
            document.getElementById(MedExCode).value = UserName;
            document.getElementById(MedExCode).readonly = true;
        }

        function FillBMIValue(txtHeightID, txtWeightID, txtBMIID) {

            var txtHeight = document.getElementById(txtHeightID);
            var txtWeight = document.getElementById(txtWeightID);
            var txtBMI = document.getElementById(txtBMIID);

            //Again validate Height TextBox
            var result = CheckDecimal(txtHeight.value);
            if (result == false) {
                txtHeight.focus();
                return;
            }

            //Now Check Weight TextBox
            result = CheckDecimal(txtWeight.value);
            if (result == false) {
                msgalert('Please Enter Valid Weight in Kilogram !');
                txtWeight.value = '';
                txtWeight.focus();
                return;
            }



            var bmi = GetBMI(txtHeight.value, txtWeight.value);
            try {
                if ((bmi != null) && !isNaN(bmi)) {
                    bmi = parseFloat(bmi);
                    txtBMI.value = bmi;

                    if (bmi < 18 || bmi > 32) {
                        msgalert('Bmi value is not suitable !');
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

        function C2F(txtCelsiusID, txtFahrenheitID, HighRange, LowRange) {
            var txtCelsius = document.getElementById(txtCelsiusID);
            var txtFahrenheit = document.getElementById(txtFahrenheitID);
            var result;

            if (HighRange != 0 || LowRange != 0) {
                if (txtCelsius.value > HighRange || txtCelsius.value < LowRange) {

                    msgalert('Out Of Range! Range Must be Between ' + LowRange + ' to ' + HighRange);

                }

            }

            if (!(CheckDecimalOrBlank(txtCelsius.value))) {
                msgalert('Please Enter Valid Temperature in Celsius !');
                txtCelsius.focus();
                return false;
            }



            txtFahrenheit.value = c2f(txtCelsius.value);
            var FarenhitHighLowRange = "";
            FarenhitHighLowRange = document.getElementById('HFFerenhitToCelcius').value;

            if (txtFahrenheit.value != 0) {
                if (txtFahrenheit.value > FarenhitHighLowRange.toString().split("##")[0] || txtFahrenheit.value < FarenhitHighLowRange.toString().split("##")[1]) {
                    msgalert('Fahrenheit value Out Of Range! Range Must be Between ' + FarenhitHighLowRange.toString().split("##")[1] + ' to ' + FarenhitHighLowRange.toString().split("##")[0]);
                }
            }
            return true;
        }

        function F2C(txtFahrenheitID, txtCelsiusID, HighRange, LowRange) {
            var txtCelsius = document.getElementById(txtCelsiusID);
            var txtFahrenheit = document.getElementById(txtFahrenheitID);
            var result;

            if (HighRange != 0 || LowRange != 0) {
                if (txtFahrenheit.value > HighRange || txtFahrenheit.value < LowRange) {

                    msgalert('Out Of Range! Range Must be Between ' + LowRange + ' to ' + HighRange);

                }

            }

            if (!(CheckDecimalOrBlank(txtFahrenheit.value))) {
                msgalert('Please Enter Valid Temperature in Fahrenheit !');
                txtFahrenheit.focus();
                return false;
            }

            txtCelsius.value = f2c(txtFahrenheit.value);


            return true;
        }

        function ValidateRemarks(txt, cntField, maxSize) {
            cntField = document.getElementById(cntField);
            if (txt.value.length > maxSize) {
                txt.value = txt.value.substring(0, maxSize);
            }
                // otherwise, update 'characters left' counter
            else {
                cntField.innerHTML = maxSize - txt.value.length;
            }
        }

        function SetAge(txtDobId, txtAgeId, dtToday) {


            var txtDob = document.getElementById(txtDobId);
            var txtAge = document.getElementById(txtAgeId);

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
                    msgalert('You Can Not Add Date Which Is Less Than "01-Jan-1900" !');
                    txtDob.value = "";
                    txtAge.value = "";
                    txtDob.focus();
                }
                var age = GetDateDifference(txtDob.value, dtToday);
                txtAge.value = age.Years.toString();

                if (age.Years.toString() < 18) {
                    msgalert('Age of Subject is not suitable !');
                }
                //hf.value = age.Years.toString();
                //txtAge.disabled = true;
            }
        }

        function ValidationQC() {
            if (document.getElementById('txtQCRemarks').value.toString().trim().length <= 0) {
                msgalert('Please Enter Remarks/Response !');
                document.getElementById('txtQCRemarks').focus();
                return false;
            }
            return true;
        }


        function QCDivShowHide(Type) {
            var Valid = false;

            if (document.getElementById('HSubjectId').value.toString().trim().length <= 0) {
                msgalert('Please Enter Subject !');
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
                msgalert('please select screening date !');
                return false;
            }
            else if (Type == 'S') {
                document.getElementById('divQCDtl').style.display = 'block';
                SetCenter('divQCDtl');
                return false;
            }
            else if (Type == 'H') {
                document.getElementById('divQCDtl').style.display = 'none';
                return false;
            }
            else if (Type == 'ST') {
                document.getElementById('divQCDtl').style.display = 'block';
                SetCenter('divQCDtl');
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

    </script>

</head>
<body>
    <form id="form1" runat="server" method="post">
        <asp:ScriptManager ID="ScriptManager2" runat="server" AsyncPostBackTimeout="1000"
            EnablePageMethods="True">
            <Services>
                <asp:ServiceReference Path="AutoComplete.asmx" />
            </Services>
        </asp:ScriptManager>

        <script type="text/javascript">
            var inyear;

            function HistoryDivShowHide(Type, MedexCode, BlockDivId, NoneDivId, ScreeningType) {
                var btn = document.getElementById('<%= btnHistory.ClientId %>');
            document.getElementById('<%= hfMedexCode.ClientId %>').value = MedexCode + '###' + ScreeningType;

            if (document.getElementById('HSubjectId').value.toString().trim().length <= 0) {
                msgalert('Please Enter Subject !');
                document.getElementById('txtSubject').focus();
                document.getElementById('txtSubject').value = '';
                return false;
            }
            else if (Type == 'S') {
                btn.click();
                return false;
            }
            else if (Type == 'H') {
                document.getElementById('divHistoryDtl').style.display = 'none';
                return false;
            }
            else if (Type == 'SN') {
                document.getElementById('divHistoryDtl').style.display = 'block';
                SetCenter('divHistoryDtl');
                return DisplayDiv(BlockDivId, NoneDivId);
            }
            return true;

        }

        //For Validation
        var count = 0;
        var element;
        var prev;
        var result;


        function Validation(type) {
            debugger;
            //added by vishal for validation

            count = 0;
            //var maleCount =21;
            var maleCount = document.getElementById('HfMaleCount').value;;
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
                msgConfirmDeleteAlert(null, count + ' Field(s) Are Blank, Do You Still Want To Save ?', function (isConfirmed) {
                    if (isConfirmed) {
                        if (document.getElementById('<%= HSubjectId.ClientId %>').value.toString().trim().length <= 0) {
                            msgalert('Please Enter Subject !');
                            document.getElementById('<%= txtSubject.ClientId %>').focus();
                            document.getElementById('<%= txtSubject.ClientId %>').value = '';
                            return false;
                        }
                        else if (type == 'EDIT') {
                                            if (document.getElementById('txtremark').value.toString().trim().length <= 0) {
                                                msgalert('Please Enter Remarks !');
                                                return false;
                                            }
                                            else {
                                                document.getElementById("<%=btnSave.ClientId %>").style.display = 'none';
                                    return true;
                                }
                            }
                        else if (type == 'ADD') {
                                document.getElementById("<%=btnSave.ClientId %>").style.display = 'none';
                                return true;
                            }
                        return true;
                    } else
                    {

                        return false;
                    }
                });
                return false;
            }

            if (document.getElementById('<%= HSubjectId.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Subject !');
                document.getElementById('<%= txtSubject.ClientId %>').focus();
                document.getElementById('<%= txtSubject.ClientId %>').value = '';
                return false;
            }
                //Added for compulsory add remark while Edit else not compulsory on 15-Sep-2009
            else if (type == 'EDIT') {
                if (document.getElementById('txtremark').value.toString().trim().length <= 0) {
                    msgalert('Please Enter Remarks !');
                    return false;
                }
                else {
                    document.getElementById("<%=btnSave.ClientId %>").style.display = 'none';
                    return true;
                }
            }
            else if (type == 'ADD') {
                document.getElementById("<%=btnSave.ClientId %>").style.display = 'none';
                return true;
            }
    return true;

}

//added by vishal for validation

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


function ClientPopulated(sender, e) {
    SubjectClientShowing('AutoCompleteExtender1', $get('<%= txtSubject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            SubjectOnItemSelected(e.get_value(), $get('<%= txtSubject.clientid %>'),
                $get('<%= HSubjectId.clientid %>'), document.getElementById('<%= btnSubject.ClientId %>'));
        }

        function ClientPopulated_WorkSpace(sender, e) {
            ProjectClientShowingSchema('AutoCompleteExtenderWorkSpace', $get('<%= txtproject.ClientId %>'));
        }

        function OnSelected_WorkSpace(sender, e) {

            ProjectOnItemSelectedForMsrLog(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'), $get('<%=HClientName.clientid %>'), $get('<%=HProjectNo.clientid %>'));
        }

        function CheckOnlyForFemale(tblName) {
            var MedExGroupCode = document.getElementById('<%= hfMedExGroupCode.ClientId %>').value;
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
        function Pwd_AuthenticationFail() {
            document.getElementById('11166').value = "";
            document.getElementById('11173').value = "";
            document.getElementById('11184').value = "";
        }
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
                //                var curr_date = (curr_date + "-" + curr_month + "-" + curr_year).toDateString();
                //                var curr_time = (curr_hour + ":" + curr_min + ":" + curr_sec).toTimeString();
                //                document.getElementById(PICommentsgivenon).value = curr_date + " " + curr_time;


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
                //                curr_hour + ":" + curr_min + ":" + curr_sec;
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
            //                var curr_date = (curr_date + "-" + curr_month + "-" + curr_year).toDateString();
            //                var curr_time = (curr_hour + ":" + curr_min + ":" + curr_sec).toTimeString();
            //                document.getElementById(PICommentsgivenon).value = curr_date + " " + curr_time; 
            document.getElementById(PICommentsgivenon).value = curr_date + "-" + curr_month + "-" + curr_year + " " + curr_hour + ":" + curr_min + ":" + curr_sec;
            document.getElementById(PICommentsgivenBy).value = Username;
        }
        function HomeClick(e) {
            msgConfirmDeleteAlert(null, "Are You Sure You Want To Go To Home Page ?", function (isConfirmed) {
                if (isConfirmed) {
                    var btn = document.getElementById('btnDeleteScreenNo');
                    btn.click();
                    __doPostBack(e.name, '');
                    return true;
                } else {
                    return false;
                }
            });
            return false;
        }

        function MedExFormula(MedExCode, formula) {
            document.getElementById('hfMedexCode').value = MedExCode;
            document.getElementById('HFMedExFormula').value = formula;
            var btn = document.getElementById('btnAutoCalculate');
            btn.click();
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
                        msgalert('Please Enter Date In DDMMYYYY Or dd-Mon-YYYY Format Only !');
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
                        msgalert('Please Enter Date In DDMMYYYY Or dd-Mon-YYYY Format Only !');
                        txtdate.value = "";
                        txtdate.focus();
                        return false;
                    }
                    if (month.length > 3 && month.length != 3) {
                        msgalert('Please Enter Date In DDMMYYYY Or dd-Mon-YYYY Format Only !');
                        txtdate.value = "";
                        txtdate.focus();
                        return false;
                    }
                    if (year.length > 4 && month.length != 4) {
                        msgalert('Please Enter Date In DDMMYYYY Or dd-Mon-YYYY Format Only !');
                        txtdate.value = "";
                        txtdate.focus();
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
                            msgalert('You Can Not Add Date Which Is Less Than "01-Jan-1900" !');
                            txtdate.value = "";
                            txtdate.focus();
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
                msgalert('You Can Not Add Date Which Is Less Than "01-Jan-1900" !');
                txtdate.value = "";
                txtdate.focus();
                return false;
            }
            return true;
        }


        //   function ValidationForContinue()
        //   {
        //    if (document.getElementById('txtremark').value.toString().trim().length <= 0) 
        //       {
        //                    alert('Please Enter Remarks');
        //                    return false;
        //       }   
        //   }



        </script>

        <table border="0" cellspacing="0" style="border-collapse: collapse; width: 100%;"
            bordercolor="#111111" id="AutoNumber1" cellpadding="0">
            <asp:HiddenField ID="CompanyName" runat="server" />
            <tr>
                <td style="width: 95%">
                    <%--<img border="0" src="images/topheader.jpg" width="1004" id="IMG1">--%>
                    <div style="background-image: url('images/background.jpg'); background-repeat: repeat-x; min-width: 100%; height: 65px;">
                        <div style="float: left;">
                            <img src="images/left.jpg" alt="biznet logo left" />
                        </div>
                        <div style="float: right;">
                            <img src="images/right.jpg" alt="biznet logo right" />
                        </div>
                    </div>
                    <div style="clear: both; position: relative; margin-top: -65px; float: right; width: 50%;">
                        <table style="width: 100%; border: 0 solid #111111; text-align: right; float: right;">
                            <tr style="height: 35px">
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblWelcome" runat="server" CssClass="Label" Text="Welcome, " Style="color: #FFFFFF" /><asp:Label
                                        ID="lblUserName" runat="server" CssClass="Label" ForeColor="#ffc300" />
                                    <asp:Label ID="lblLoginTime1" runat="server" CssClass="Label" Text="Login Time: "
                                        Style="color: #FFFFFF" />
                                    <asp:Label ID="lblTime" runat="server" CssClass="Label" ForeColor="#ffc300" />
                                    <asp:Label runat="server" ID="lblSessionTimeCap" class="Label" Style="color: #FFFFFF"
                                        Text="Session Expires:"></asp:Label>
                                    <b><span class="Label" style="color: #FFFFFF" id="timerText"></span></b>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 100%; text-align: left;" background="images/bluebg.gif">
                    <%--<asp:Label ID="lblWelcome" runat="server" CssClass="Label" Text="Welcome, " /><asp:Label
                    ID="lblUserName" runat="server" CssClass="Label" ForeColor="#FF9B2A" Width="500px" />
                <asp:Label ID="lblLoginTime1" runat="server" CssClass="Label" Text="Login Time: "
                    Style="margin-left: 90px" />
                <asp:Label ID="lblTime" runat="server" CssClass="Label" ForeColor="#FF9B2A" />--%>
                </td>
            </tr>
            <tr>
                <td style="width: 95%; text-align: left;" background="images/whitebg.gif">&nbsp; &nbsp;
                <asp:LinkButton ID="LinkButton1" runat="server" Font-Bold="True" ForeColor="Navy"
                    OnClientClick="return HomeClick(this);">Home</asp:LinkButton>
                    &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;<div align="center">
                        <table border="1" cellspacing="1" bordercolor="#F2A041" width="99%" cellpadding="0">
                            <tr>
                                <td align="center" style="width: 98%">
                                    <asp:Panel ID="Pan_Hdr" runat="server" Width="100%" CssClass="InnerTable" BackColor="White">
                                        <asp:Panel ID="Pan_Child" runat="server" Width="100%" BackColor="Window">
                                            <div id="Header Label" style="text-align: center;" align="center" class="Div">
                                                <table style="width: 100%; text-align: center;">
                                                    <tr style="text-align: center;">
                                                        <td align="center" width="100%">
                                                            <asp:Label ID="lblHeader" runat="server" SkinID="lblHeading" Text="Screening" />
                                                            <hr style="width: 100%; background-image: none; color: #ffcc66; background-color: #ffcc66" />
                                                        </td>
                                                    </tr>
                                                    <tr style="text-align: center;">
                                                        <td style="text-align: left; width: 890px;">
                                                            <div style="margin: auto; color: Red; text-align: center;">
                                                                <asp:Label ID="lblerrormsg" runat="server" />
                                                            </div>
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td style="white-space: nowrap; width: 100%; text-align: left;">
                                                                        <asp:Label ID="lblSuject" runat="server" SkinID="lblDisplay" Text="Subject: " CssClass="Label" />
                                                                        <asp:TextBox ID="txtSubject" runat="server" CssClass="textBox" TabIndex="2" Width="480px" />
                                                                        <asp:Button ID="btnSubject" runat="server" Style="display: none" Text="Subject" />
                                                                        <asp:Button ID="BtnQC" OnClientClick="return QCDivShowHide('S');" runat="server"
                                                                            CssClass="btn btnnew" Text="QC" />&nbsp;
                                                                    <asp:HiddenField ID="HSubjectId" runat="server" />
                                                                        <asp:HiddenField ID="HScrNo" runat="server" />
                                                                        <asp:HiddenField ID="HFNumericScale" runat="server" />
                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                                                            MinimumPrefixLength="1" OnClientItemSelected="OnSelected" OnClientShowing="ClientPopulated"
                                                                            ServiceMethod="GetSubjectCompletionList_NotRejected" ServicePath="AutoComplete.asmx"
                                                                            TargetControlID="txtSubject" UseContextKey="True" CompletionListElementID="pnlSubject">
                                                                        </cc1:AutoCompleteExtender>
                                                                        <asp:Panel ID="pnlSubject" runat="server" />
                                                                        <div id="divQCDtl" runat="server" class="DIVSTYLE2" style="display: none; left: 389px; width: 800px; top: 316px; text-align: left; height: 336px;">
                                                                            <asp:Panel ID="pnlQCDtl" runat="server" Width="800px" ScrollBars="Auto" Height="336px">
                                                                                <table>
                                                                                    <tr>
                                                                                        <td align="left" class="Label" style="width: 14%"></td>
                                                                                        <td align="left" style="width: 80%">
                                                                                            <asp:RadioButtonList ID="RBLQCFlag" runat="server" RepeatDirection="Horizontal" Visible="False">
                                                                                                <asp:ListItem Value="Y">Approve</asp:ListItem>
                                                                                                <asp:ListItem Value="N">Reject</asp:ListItem>
                                                                                                <asp:ListItem Selected="True" Value="F">Feedback</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="Label" colspan="2" style="text-align: left;">
                                                                                            <asp:Label ID="lblResponse" runat="server" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="Label" style="width: 14%; text-align: left;">Remarks :
                                                                                        </td>
                                                                                        <td colspan="3" style="width: 80%; text-align: left;">
                                                                                            <textarea id="txtQCRemarks" onkeydown="ValidateRemarks(this,'lblcnt',1000);" runat="server"
                                                                                                class="textBox" style="width: 277px" />
                                                                                            <asp:Label ID="lblcnt" runat="server" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr class="TR">
                                                                                        <td align="left" class="Label" style="width: 14%">To :
                                                                                        </td>
                                                                                        <td colspan="3" style="width: 80%; text-align: left;">
                                                                                            <asp:TextBox ID="txtToEmailId" runat="server" CssClass="textBox" Width="277px" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr class="TR">
                                                                                        <td class="Label" style="width: 14%; text-align: left;">CC :
                                                                                        </td>
                                                                                        <td colspan="3" style="width: 80%; text-align: left;">
                                                                                            <asp:TextBox ID="txtCCEmailId" runat="server" CssClass="textBox" Height="37px" TextMode="MultiLine"
                                                                                                Width="278px" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr class="TR">
                                                                                        <td style="height: 32px; text-align: left;">&nbsp;
                                                                                        </td>
                                                                                        <td colspan="3" class="Label" style="height: 32px; text-align: left;">
                                                                                            <%--<input id="BtnQCSave" runat="server" class="button" style="width: 91px" type="button" value="Save & Send" />--%>
                                                                                            <asp:Button ID="BtnQCSave" runat="server" CssClass="btn btnsave" Text="Save"
                                                                                                OnClientClick="return ValidationQC();" />&nbsp;<asp:Button ID="BtnQCSaveSend" runat="server"
                                                                                                    CssClass="btn btnsave" OnClientClick="return ValidationQC();"
                                                                                                    Text="Save & Send" />
                                                                                            <asp:Button Style="display: none" ID="btnAutoCalculate" OnClick="btnAutoCalculate_Click"
                                                                                                runat="server" Text="Auto Calculate" ToolTip="Auto Calculate" CssClass="btn btnsave" />
                                                                                            <input id="Button1" runat="server" class="btn btnclose" onclick="QCDivShowHide('H');" type="button"
                                                                                                value="Close" />
                                                                                            <asp:Button ID="btnDeleteScreenNo" runat="server" class="btn btncancel" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                                <strong style="text-align: left">
                                                                                    <br />
                                                                                    QC Comments History </strong>
                                                                                <br />
                                                                                <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                                                                                <br />
                                                                                <asp:GridView ID="GVQCDtl" runat="server" AutoGenerateColumns="False" BorderColor="Peru"
                                                                                    Font-Size="Small" SkinID="grdViewSmlSize">
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="nMedExScreeningHdrQCNo" HeaderText="nMedExScreeningHdrQCNo" />
                                                                                        <asp:BoundField DataField="vSubjectId" HeaderText="vSubjectId" />
                                                                                        <asp:BoundField DataField="iTranNo" HeaderText="Sr. No."></asp:BoundField>
                                                                                        <asp:BoundField DataField="FullName" HeaderText="Subject">
                                                                                            <ItemStyle Wrap="false" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="vQCComment" HeaderText="QC Comments" />
                                                                                        <asp:BoundField DataField="cQCFlag" HeaderText="QC" />
                                                                                        <asp:BoundField DataField="vQCGivenBy" HeaderText="QC BY" />
                                                                                        <asp:BoundField DataField="dQCGivenOn" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="QC Date"
                                                                                            HtmlEncode="False">
                                                                                            <ItemStyle Wrap="false" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="vResponse" HeaderText="Response" />
                                                                                        <asp:BoundField DataField="vResponseGivenBy" HeaderText="Response BY">
                                                                                            <ItemStyle Wrap="false" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="dResponseGivenOn" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Response Date"
                                                                                            HtmlEncode="False">
                                                                                            <ItemStyle Wrap="false" />
                                                                                        </asp:BoundField>
                                                                                        <asp:TemplateField HeaderText="Response">
                                                                                            <ItemTemplate>
                                                                                                <asp:LinkButton ID="lnkResponse" runat="server">Response</asp:LinkButton>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </asp:Panel>
                                                                        </div>
                                                                    </td>
                                                                    <td style="vertical-align: middle; background-repeat: no-repeat; white-space: nowrap; background-color: transparent; width: 3px; text-align: left;"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="Label" colspan="2" style="padding-left: 50px;">
                                                                        <asp:CheckBoxList ID="chkScreeningType" runat="server" RepeatDirection="Horizontal"
                                                                            onClientclick=" return ShowHideproject();" CssClass="chkScreenType" AutoPostBack="true">
                                                                            <asp:ListItem Value="DF">Default Screening</asp:ListItem>
                                                                            <asp:ListItem Value="PS">Project Specific Screening</asp:ListItem>
                                                                        </asp:CheckBoxList>
                                                                    </td>
                                                                </tr>
                                                                <tr id="tr_WorkSpace" style="display: none;" runat="server" class="tr_WorkSpace">
                                                                    <td class="Label" colspan="2">Project:
                                                                    <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="475px" TabIndex="1" />
                                                                        <asp:Button Style="display: none" ID="btnSetProject" runat="server" Text=" Project" />
                                                                        <asp:HiddenField ID="HProjectId" runat="server" />
                                                                        <asp:HiddenField ID="HFMedExFormula" runat="server" />
                                                                        <asp:HiddenField ID="HClientName" runat="server" />
                                                                        <asp:HiddenField ID="HProjectNo" runat="server" />
                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtenderWorkSpace" runat="server" UseContextKey="True"
                                                                            TargetControlID="txtproject" ServicePath="AutoComplete.asmx" ServiceMethod="GetMyProjectCompletionListForProjectSpScr"
                                                                            OnClientShowing="ClientPopulated_WorkSpace" OnClientItemSelected="OnSelected_WorkSpace"
                                                                            MinimumPrefixLength="1" CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                                            CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtenderWorkSpace" />
                                                                        <%--CompletionListElementID="PnlProject" <asp:Panel ID="PnlProject" runat="server" />--%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" style="width: 100%; height: 265px; text-align: left;">
                                                                        <div style="padding: 0px 0px 0px 50px;">
                                                                            <asp:RadioButtonList ID="rblScreeningDate" runat="server" AutoPostBack="True" RepeatColumns="3"
                                                                                RepeatDirection="Horizontal" CssClass="Label" />
                                                                            <asp:DropDownList ID="ddlScreeningDate" runat="server" CssClass="dropDownList" Visible="false" />
                                                                        </div>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td style="height: 241px; text-align: left; vertical-align: top;">
                                                                                    <asp:UpdatePanel ID="UpPlaceHolder" runat="server">
                                                                                        <ContentTemplate>
                                                                                            <asp:Panel ID="PnlPlaceMedex" runat="server" Width="100%">
                                                                                                <asp:PlaceHolder ID="PlaceMedEx" runat="server" EnableViewState="true"></asp:PlaceHolder>
                                                                                            </asp:Panel>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                    <asp:HiddenField ID="hfBMI" runat="server" />
                                                                                    <asp:HiddenField ID="hMedExNo" runat="server" />
                                                                                    <asp:HiddenField ID="hfMedexCode" runat="server" />
                                                                                    <asp:HiddenField ID="hfMedExGroupCode" runat="server" />
                                                                                    <asp:HiddenField ID="hfMedExCodeForSex" runat="server" />
                                                                                    <asp:HiddenField ID="HfUserName" runat="server" />
                                                                                    <asp:HiddenField ID="HfMaleCount" runat="server" />
                                                                                    <asp:HiddenField ID="HFFerenhitToCelcius" runat="server" />
                                                                                    <button id="btnDeviation" runat="server" style="display: none;" />
                                                                                    <cc1:ModalPopupExtender ID="MPEDeviation" runat="server" PopupControlID="divHistoryDtl"
                                                                                        RepositionMode="None" PopupDragHandleControlID="lblMedexDescription" BackgroundCssClass="modalBackground"
                                                                                        TargetControlID="btnDeviation" CancelControlID="ImgPopUpCloseDeviation" BehaviorID="MPEDeviation">
                                                                                    </cc1:ModalPopupExtender>
                                                                                    <div id="divHistoryDtl" runat="server" class="centerModalPopup" style="display: none; width: 80%; position: absolute; max-height: 80%;">
                                                                                        <table style="width: 100%" cellpadding="5px">
                                                                                            <tr>
                                                                                                <td class="Label" style="text-align: center;" colspan="2">
                                                                                                    <h1 class="header" style="background-repeat: repeat-y; height: 25%;">
                                                                                                        <strong>Attribute History Of
                                                                                                        <asp:Label ID="lblMedexDescription" runat="server" /></strong>
                                                                                                        <img id="ImgPopUpCloseDeviation" onclick="HistoryDivShowHide('H','','','');" src="images/Sqclose.gif"
                                                                                                            style="width: 24px; height: 15px" title="Close" />
                                                                                                    </h1>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2" valign="top">
                                                                                                    <asp:Panel ID="pnlHistoryDtl" runat="server" Height="250px" ScrollBars="Auto" Width="100%"
                                                                                                        Style="margin: auto;">
                                                                                                        <asp:GridView ID="GVHistoryDtl" runat="server" AutoGenerateColumns="False" BorderColor="Peru"
                                                                                                            Font-Size="Small" SkinID="grdViewSmlAutoSize" Width="100%" BorderStyle="Solid"
                                                                                                            BorderWidth="1">
                                                                                                            <Columns>
                                                                                                                <asp:BoundField DataField="vSubjectId" HeaderText="Subject Id">
                                                                                                                    <ItemStyle Wrap="false" />
                                                                                                                </asp:BoundField>
                                                                                                                <asp:BoundField DataField="dScreenDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Screening Date"
                                                                                                                    HtmlEncode="False">
                                                                                                                    <ItemStyle Wrap="false" />
                                                                                                                </asp:BoundField>
                                                                                                                <asp:BoundField DataField="vMedExDesc" HeaderText="Attribute">
                                                                                                                    <ItemStyle Width="20%" />
                                                                                                                </asp:BoundField>
                                                                                                                <asp:BoundField DataField="iTranNo" HeaderText="Sr. No." />
                                                                                                                <asp:BoundField DataField="vDefaultValue" HeaderText="Value" />
                                                                                                                <asp:BoundField DataField="vModifyBy" HeaderText="Modify By">
                                                                                                                    <ItemStyle Wrap="False" />
                                                                                                                </asp:BoundField>
                                                                                                                <asp:BoundField DataField="dModifyOn_IST" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Modify On"
                                                                                                                    HtmlEncode="False">
                                                                                                                    <ItemStyle Wrap="False" />
                                                                                                                </asp:BoundField>
                                                                                                            </Columns>
                                                                                                        </asp:GridView>
                                                                                                    </asp:Panel>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div style="display: none; width: 70%; text-align: center; margin: 0 auto;" id="divAudit"
                                                                                        class="DIVSTYLE2" runat="server">
                                                                                        <table style="width: 100%; text-align: center; margin: 0 auto;">
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td style="height: 20px; text-align: center; width: 95%;">
                                                                                                        <strong style="white-space: nowrap; margin: 0 auto;">Remarks History </strong>
                                                                                                    </td>
                                                                                                    <td style="text-align: center; float: right;">
                                                                                                        <img style="width: 21px; height: 15px" onclick="DivAuditShowHide('H');" src="images/close.gif"
                                                                                                            id="IMG2" />
                                                                                                        &nbsp;
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="text-align: center;" colspan="2">
                                                                                                        <asp:GridView ID="GVAuditFnlRmk" runat="server" Font-Size="Small" SkinID="grdViewSml1"
                                                                                                            AutoGenerateColumns="False" BorderColor="Peru">
                                                                                                            <Columns>
                                                                                                                <asp:BoundField HeaderText="SrNo" />
                                                                                                                <asp:BoundField DataField="dScreenDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Screen Date"></asp:BoundField>
                                                                                                                <asp:BoundField DataField="vRemark" HeaderText="Remarks"></asp:BoundField>
                                                                                                                <asp:BoundField DataField="ModifyBy" HeaderText="Modify By"></asp:BoundField>
                                                                                                                <asp:BoundField DataField="dModifyOn" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Modified On"
                                                                                                                    HtmlEncode="False">
                                                                                                                    <ItemStyle Wrap="false" />
                                                                                                                </asp:BoundField>
                                                                                                                <asp:BoundField DataField="nMedExScreeningHdrNo" HeaderText="MedExScreeningHdrNo" />
                                                                                                            </Columns>
                                                                                                        </asp:GridView>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>
                                                                                    </div>
                                                                                    <asp:Button ID="btnSetControlValue" runat="server" CssClass="btn btnnew" Style="display: none"
                                                                                        Text="Set Control Value" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 100%; text-align: left;">
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td style="width: 50%; height: 21px; text-align: left;">
                                                                                    <asp:Button ID="BtnPrevious" runat="server" CssClass="btn btnnew" Text="<< Previous" />
                                                                                </td>
                                                                                <td style="width: 50%; height: 21px; text-align: right;">
                                                                                    <asp:Button ID="BtnNext" runat="server" CssClass="btn btnnew" Text="Next >>" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" style="width: 890px; white-space: nowrap; text-align: center; vertical-align: middle;">
                                                                        <asp:Label runat="server" ID="lblRemarks" Text="Remarks:" CssClass="Label" />
                                                                        <asp:TextBox ID="txtremark" runat="server" Height="55px" TextMode="MultiLine" Width="363px"
                                                                            onkeydown="ValidateRemarks(this,'lblRcnt',1000);" />
                                                                        <asp:Label ID="lblRcnt" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" style="width: 890px; height: 21px; white-space: nowrap; text-align: center; margin: 0 auto;">
                                                                        <asp:Button ID="btnHistory" runat="server" CssClass="btn btnnew" Style="display: none; margin: auto;"
                                                                            Text="History" />&nbsp;
                                                                    <%-- btnRmkHistory--%>
                                                                        <%--<div style="text-align: center; margin:0 auto; width: 100%; vertical-align: middle;">--%>
                                                                        <asp:Button ID="btnRmkHistory" runat="server" CssClass="btn btnnew" Text="Remarks History"
                                                                            Style="margin: 0 auto;" /><%--</div>--%>
                                                                        <asp:Button ID="btnContinueSave" runat="server" CssClass="btn btnsave" Text="Save & Continue" ToolTip="Save & Continue" />
                                                                        <asp:Button ID="BtnSave" runat="server" CssClass="btn btnsave" Text="Save" ToolTip="Save" />
                                                                        <asp:Button ID="BtnExit" runat="server" CssClass="btn btnexit" Text="Exit" ToolTip="Exit" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <br />
                                        </asp:Panel>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 95%"></td>
            </tr>
            <tr>
                <td background="images/orangebg.gif" style="width: 95%">
                    <p style="text-align: center">

                        <script type="text/javascript">
                            var copyright;
                            var update;
                            copyright = new Date();
                            update = copyright.getFullYear();
                            document.write("<font face=\"verdana\" size=\"1\" color=\"black\">© Copyright " + update + "," + $get('<%= CompanyName.clientid %>').value + "</font>");

                        </script>

                    </p>
                </td>
            </tr>
        </table>
        <asp:Button ID="btnShow" runat="server" Text="Show Dialog" Style="display: none" />
        <cc1:ModalPopupExtender ID="MPEAunthticate" runat="server" TargetControlID="btnShow"
            PopupControlID="divAuthentication" BackgroundCssClass="modalBackground" PopupDragHandleControlID="divAuthentication"
            BehaviorID="MPEId">
        </cc1:ModalPopupExtender>
        <div style="display: none; left: 391px; width: 450px; top: 528px; height: 200px; text-align: left"
            id="divAuthentication" class="DIVSTYLE2" runat="server">
            <table style="width: 400px; text-align: center;" cellspacing="10">
                <tr style="text-align: center;">
                    <td style="text-align: center;" colspan="2" class="Label ">
                        <strong>User Authentication</strong>
                    </td>
                </tr>
                <tr>
                    <td class="Label" style="text-align: center;">Name :
                    </td>
                    <td class="Label" style="text-align: left;">
                        <asp:Label runat="server" ID="lblSignername"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="Label" style="text-align: center;">Designation :
                    </td>
                    <td class="Label" style="text-align: left;">
                        <asp:Label runat="server" ID="lblSignerDesignation"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="Label" style="text-align: center;">Remarks :
                    </td>
                    <td style="text-align: left;">
                        <label class="Label">
                            I attest to the accuracy and integrity of the data being reviewed.</label>
                    </td>
                </tr>
                <tr>
                    <td class="Label" style="text-align: center;">Password :
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtPassword" runat="Server" Text="" CssClass="textbox" TextMode="Password"> </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;" colspan="2">
                        <asp:Button ID="btnAuthenticate" runat="server" Text="Authenticate" CssClass="button"
                            Width="110px" OnClientClick="return ValidationForAuthentication();"></asp:Button>
                        <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btnclose" OnClientClick="return DivAuthenticationHideShow();" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btnmdl" runat="server" Style="display: none;" />
        <cc1:ModalPopupExtender ID="mdlSessionTimeoutWarning" runat="server" PopupControlID="divSessionTimeoutWarning"
            BackgroundCssClass="modalBackground" BehaviorID="mdlSessionTimeoutWarning" TargetControlID="btnmdl">
        </cc1:ModalPopupExtender>
        <div id="divSessionTimeoutWarning" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 35%; height: auto; max-height: 25%; min-height: auto;">
            <asp:UpdatePanel ID="HM_Home_upnlSession" runat="server" UpdateMode="Conditional"
                RenderMode="Inline">
                <ContentTemplate>
                    <table width="350px" align="center">
                        <tr>
                            <td>
                                <img id="Img1" src="~/Images/showQuery.png" runat="server" alt="Confirmation" />
                            </td>
                            <td class="Label" style="text-align: left;">Your session will expire within 5 mins.
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Button ID="btnContinueWorking" runat="server" Text="Extend" CssClass="btn btnnew" Style="display: none;" />
                                <asp:Button ID="BtnSessionDivClose" runat="server" Text="Close" CssClass="btn btnnew"
                                    OnClientClick="closeSessionDiv();" />
                                <asp:Button ID="btnLogout" runat="server" Text="" CssClass="btn btngo" Style="display: none;" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <asp:HiddenField ID="HMedex_Medex_PI_Co_I_Designate" runat="server" />
        <asp:HiddenField ID="HMedex_Medex_PICommentsgivenon" runat="server" />
        <asp:HiddenField ID="Ferenhit" runat="server" />
        <asp:HiddenField ID="Celcius" runat="server" />
    </form>

    <script type="text/javascript">

        var ACTUAL_SESSIONTIME = "<%= Session.Timeout %>", timerId, sessionFlag = true;
        SessionTimeSet();

        $(document).ready(function () {
            ShowHideproject();
        });

        function ValidationForAuthentication() {
            if (document.getElementById('<%= txtPassword.ClientId %>').value.trim() == '') {
                document.getElementById('<%= txtPassword.ClientId %>').value = '';
                msgalert('Please Enter Password For Authentication !');
                document.getElementById('<%= txtPassword.ClientId %>').focus();
                return false;
            }
            return true;
        }
        function DivAuthenticationHideShow() {
            $find('MPEId').hide();
            document.getElementById('11166').value = "";
            document.getElementById('11173').value = "";
            document.getElementById('11184').value = "";
        }


        //PSS
        function ShowHideproject() {
            var indices = '';
            $('#<%=chkScreeningType.ClientID %> input:checkbox').each(function (index) { if ($(this).filter('input:checkbox:checked').length == 1) { indices += (index + ' '); } });
            if (indices.split('1').length == 2) {
                $('#tr_WorkSpace')[0].attributes["style"].nodeValue = "display:'';";
            }
            else {
                $('#tr_WorkSpace')[0].attributes["style"].nodeValue = "display:none";
                //$get('<%= HProjectId.clientid %>').value = "";
                //document.getElementById('<%= btnSetProject.ClientId %>').value = "";
            }
            return true;

            //            if ($get('<%= HProjectId.clientid %>').value == "") {
            //                return false;
            //            }
            //            return true;
        }

        function c2f(celsius2) {

            var fahrenheit2;
            if (document.getElementById('<%=Celcius.ClientId %>').value != "") {
                var HFCelsius = document.getElementById('<%=Celcius.ClientId %>').value;
                if ((celsius2.substring(0, 4)) == (HFCelsius.substring(0, 4))) {
                    celsius2 = document.getElementById('<%=Celcius.ClientId %>').value;
                    document.getElementById('<%=Celcius.ClientId %>').value = "";
                }

            }
            fahrenheit2 = 9 / 5 * celsius2 + 32;

            document.getElementById('<%=Ferenhit.ClientId %>').value = fahrenheit2;

            if (fahrenheit2.toString().indexOf('.') > 0) {
                fahrenheit2 = fahrenheit2.toString().substring(0, fahrenheit2.toString().indexOf('.') + 2)
            }
            return fahrenheit2;
        }
        function f2c(fahrenheit1) {

            var celsius1;

            if (document.getElementById('<%=Ferenhit.ClientId %>').value != "") {
                var HFFarenheit = document.getElementById('<%=Ferenhit.ClientId %>').value;

                if ((fahrenheit1.substring(0, 4)) == (HFFarenheit.substring(0, 4))) {
                    fahrenheit1 = document.getElementById('<%=Ferenhit.ClientId %>').value;
                    document.getElementById('<%=Ferenhit.ClientId %>').value = "";
                }
            }
            celsius1 = 5 / 9 * (fahrenheit1 - 32)
            document.getElementById('<%=Celcius.ClientId %>').value = celsius1;
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



    </script>

</body>
</html>
