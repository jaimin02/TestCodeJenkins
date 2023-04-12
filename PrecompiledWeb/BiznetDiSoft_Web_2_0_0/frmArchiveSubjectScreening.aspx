<%@ page language="VB" autoeventwireup="false" inherits="frmArchiveSubjectScreening, App_Web_xbhimv2u" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="App_Themes/StyleBlue/StyleBlue.css" rel="stylesheet" type="text/css" />

    <script src="Script/General.js" language="javascript" type="text/javascript"></script>

    <script src="Script/Validation.js" language="javascript" type="text/javascript"></script>

    <script src="Script/popcalendar.js" language="javascript" type="text/javascript"></script>

    <script src="Script/AutoComplete.js" language="javascript" type="text/javascript"></script>

    <script src="Script/jquery.min.js" type="text/javascript"></script>

    <script src="Script/jquery-1.4.3.min.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">

        //        function PageLoad() {
        //            history.go(1);
        //        }
        //        //Abandon Session
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
            PageMethods.DeleteScreeningtmpTable(hmed, hSubID, function () { document.getElementById('HSubjectId').value = '', function (err) { msgalert(err); } })

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
                        result = CheckIntegerOrBlank(txt.value);//CheckIntegerOrBlank
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
        function Next(NoneDivId) {
            var MedExGroupCode = document.getElementById('hfMedExGroupCode').value;
            MedExGroupCode = MedExGroupCode.replace('BtnDiv', 'Div');
            var tblName = document.getElementById('hfMedExCodeForSex').value;
            var tblRdo = $get(tblName);
            var name = tblRdo.id;
            name = name.replace(/_/g, '$');
            var rdos = document.getElementsByName(name);
            var i;
            var SexValue;
            for (i = 0; i < rdos.length; i++) {
                if (rdos[i].checked && rdos[i].value == 'Male') {
                    SexValue = 'Male';
                }
                else if (rdos[i].checked && rdos[i].value == 'Female') {
                    SexValue = 'Female';
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
            MedExGroupCode = MedExGroupCode.replace('BtnDiv', 'Div');
            var tblName = document.getElementById('hfMedExCodeForSex').value;
            var tblRdo = $get(tblName);
            var name = tblRdo.id;
            name = name.replace(/_/g, '$');
            var rdos = document.getElementsByName(name);
            var i;
            var SexValue = '';
            for (i = 0; i < rdos.length; i++) {
                if (rdos[i].checked && rdos[i].value == 'Male') {
                    SexValue = 'Male';
                    continue;
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
            msgalert('Have You Checked "Past History" , "General Exam." And "Systemic Exam." ?');
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
                msgalert('Please Enter Valid Weight in Kilogram');
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

        function C2F(txtCelsiusID, txtFahrenheitID) {
            //  var txtCelsius = document.getElementById(txtCelsiusID);
            //  var txtFahrenheit = document.getElementById(txtFahrenheitID);
            //  var result;
            //  
            //  if (!(CheckDecimalOrBlank(txtCelsius.value)))
            //  {
            //    alert('Please Enter Valid Temperature in Celsius.');
            //    txtCelsius.focus();
            //    return false;
            //  }
            //  
            //  txtFahrenheit.value = c2f(txtCelsius.value);
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

            function HistoryDivShowHide(Type, MedexCode, BlockDivId, NoneDivId) {

                var btn = document.getElementById('<%= btnHistory.ClientId %>');
            document.getElementById('<%= hfMedexCode.ClientId %>').value = MedexCode;

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


        //        function Validation(type) {
        //        
        //            //added by vishal for validation
        //              
        //            count = 0;
        //            //var maleCount =21;
        //            var maleCount = document.getElementById('HfMaleCount').value; ;
        //            jQuery('.Required').each(validateControls);
        //            var MedExGroupCode = document.getElementById('hfMedExGroupCode').value;
        //            MedExGroupCode = MedExGroupCode.replace('BtnDiv', 'Div');
        //            var tblName = document.getElementById('hfMedExCodeForSex').value;
        //            var tblRdo = $get(tblName);
        //            var name = tblRdo.id;
        //            name = name.replace(/_/g, '$');
        //            var rdos = document.getElementsByName(name);
        //            var i, cnt = 0;
        //            var SexValue;
        //            for (i = 0; i < rdos.length; i++) {
        //                if (rdos[i].checked && rdos[i].value == 'Male') 
        //                     {
        //                        
        //                        count = count - maleCount;
        //                    }
        //            }
        //            if (count > 0) {
        //                var conf = confirm('' + count + ' Field(s) Are Blank, Do You Still Want To Save?');
        //                if (conf) {
        //               

        //                }
        //                else {
        //                    return false;
        //                }
        //            }

        //            if (document.getElementById('<%= HSubjectId.ClientId %>').value.toString().trim().length <= 0) {
            //                alert('Please Enter Subject');
            //                document.getElementById('<%= txtSubject.ClientId %>').focus();
            //                document.getElementById('<%= txtSubject.ClientId %>').value = '';
            //                return false;
            //            }
            //            //Added for compulsory add remark while Edit else not compulsory on 15-Sep-2009
            ////        
            ////         

            //        }

            //        //added by vishal for validation



            //        function validateControls(index) {
            //     
            //            //debugger added by vishal for validation;
            //            element = jQuery('.Required')[index];




            //            if (element.nodeName == 'TABLE') {
            //                
            //                if ($('input:checked', element).length == 0) {
            //                    element.style.border = '1px solid Red';
            //                    count = count + 1;
            //                    return;
            //                }
            //            }
            //            else {
            //                switch (element.type) {
            //                    case 'text':
            //                    case 'textarea':
            //                        document.getElementById(element.id).style.borderColor = '';
            //                        if (element.value.trim().length <= 0) {
            //                            document.getElementById(element.id).style.borderColor = 'Red';
            //                            count = count + 1;
            //                            return;
            //                        }
            //                        break;

            //                    case 'select-one':
            //                        document.getElementById(element.id).style.backgroundColor = '';
            //                        if (element.value.trim().length <= 0) {
            //                            document.getElementById(element.id).style.backgroundColor = 'Red';
            //                            count = count + 1;
            //                            return;
            //                        }
            //                        break;
            //                }
            //            }
            //        }

            //        
            function ClientPopulated(sender, e) {
                SubjectClientShowing('AutoCompleteExtender1', $get('<%= txtSubject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            SubjectOnItemSelected(e.get_value(), $get('<%= txtSubject.clientid %>'),
                $get('<%= HSubjectId.clientid %>'), document.getElementById('<%= btnSubject.ClientId %>'));
        }
        //        
        //        function CheckOnlyForFemale(tblName)
        //        {
        //            var MedExGroupCode = document.getElementById('<%= hfMedExGroupCode.ClientId %>').value;
            //            var tblRdo = $get(tblName);
            //            var name = tblRdo.id;
            //            name = name.replace(/_/g,'$');
            //            var rdos = document.getElementsByName(name);
            //            var i;
            //            for (i=0; i < rdos.length; i++)
            //            {
            //                if (rdos[i].checked && rdos[i].value == 'Male')
            //                {
            //                    document.getElementById(MedExGroupCode).value = 'Not Applicable For Male';
            //                    document.getElementById(MedExGroupCode).disabled = true;
            //                }
            //                else if (rdos[i].checked && rdos[i].value == 'Female')
            //                {
            //                    document.getElementById(MedExGroupCode).disabled = false;
            //                    document.getElementById(MedExGroupCode).value = 'For Female Only';
            //                }
            //            }
            //        }

            function Authentication() {
                var Rbl_Yes = document.getElementById('00476_0');
                var Rbl_No = document.getElementById('00476_1');
                if (Rbl_Yes.checked.toString().toUpperCase() == "TRUE" || Rbl_No.checked.toString().toUpperCase() == "TRUE") {
                    $find('MPEId').show();
                }
            }
            function Pwd_AuthenticationFail() {
                document.getElementById('00476_0').checked = false;
                document.getElementById('00476_1').checked = false;
                document.getElementById('11171').value = "";
                document.getElementById('11172').value = "";
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
            function HomeClick() {
                var ans = confirm('Are You Sure You Want To Go To Home Page?');
                if (ans == true) {
                    document.getElementById('LinkButton1').click();
                    var btn = document.getElementById('btnDeleteScreenNo');
                    btn.click();
                    return true;
                }
            }
        </script>

        <table border="0" cellspacing="0" style="border-collapse: collapse" bordercolor="#111111"
            width="998" id="AutoNumber1" cellpadding="0">
            <asp:HiddenField ID="CompanyName" runat="server" />
            <tr>
                <td style="width: 95%">
                    <img border="0" src="images/topheader.jpg" width="1004" id="IMG1">
                </td>
            </tr>
            <tr>
                <td background="images/bluebg.gif" align="left" style="width: 95%">
                    <asp:Label ID="lblWelcome" runat="server" CssClass="Label" Text="Welcome, "></asp:Label><asp:Label
                        ID="lblUserName" runat="server" CssClass="Label" ForeColor="#FF9B2A" Width="618px"></asp:Label>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                <asp:Label ID="lblLoginTime1" runat="server" CssClass="Label" Text="Login Time: "></asp:Label><asp:Label
                    ID="lblTime" runat="server" CssClass="Label" ForeColor="#FF9B2A"></asp:Label>
                    <asp:Label runat="server" ID="lblSessionTimeCap" class="Label" Style="color: #FFFFFF"
                        Text="Session Expires:"></asp:Label>
                    <b><span class="Label" style="color: #FFFFFF" id="timerText"></span></b>
                </td>
            </tr>
            <tr>
                <td background="images/whitebg.gif" align="left" style="width: 95%">&nbsp; &nbsp;
                <asp:LinkButton ID="LinkButton1" runat="server" Font-Bold="True" ForeColor="Navy"
                    OnClientClick="HomeClick();">Home</asp:LinkButton>
                    &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;<div align="center">
                        <table border="1" cellspacing="1" bordercolor="#F2A041" width="99%" cellpadding="0">
                            <tr>
                                <td align="center" style="width: 98%">
                                    <asp:Panel ID="Pan_Hdr" runat="server" Width="100%" CssClass="InnerTable" BackColor="White">
                                        <asp:Panel ID="Pan_Child" runat="server" Width="100%" BackColor="Window">
                                            <div id="Header Label" style="text-align: center;" align="center" class="Div">
                                                <table align="center" style="width: 100%">
                                                    <tr align="center">
                                                        <td align="center" width="890px">
                                                            <asp:Label ID="lblHeader" runat="server" SkinID="lblHeading" Text="Screening"></asp:Label>
                                                            <hr style="width: 982px; background-image: none; color: #ffcc66; background-color: #ffcc66" />
                                                        </td>
                                                    </tr>
                                                    <tr align="center">
                                                        <td align="left" width="890">
                                                            <table style="width: 100%">
                                                                <%--<tr>
                                                                    <td align="left" style="width: 100%;" class="Label">
                                                                        <asp:Label ID="lblMedExGroup" runat="server" SkinID="lblDisplay" Text="Medical Eaxmination Group:"
                                                                            Visible="False"></asp:Label>
                                                                        <asp:DropDownList ID="DDLMedexGroup" runat="server" AutoPostBack="True" CssClass="dropDownList"
                                                                            Visible="False">
                                                                        </asp:DropDownList></td>
                                                                    <td style="white-space: nowrap; vertical-align: middle; width: 3px;" align="left">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="Label" style="width: 100%;">
                                                                        <asp:Label ID="Label1" runat="server" SkinID="lblDisplay" Text="Project: " Visible="False"></asp:Label>
                                                                        <asp:DropDownList ID="DDLWorkspace" runat="server" CssClass="dropDownList" Width="492px"
                                                                            Style="display: none" AutoPostBack="True">
                                                                        </asp:DropDownList></td>
                                                                    <td align="left" style="vertical-align: middle; background-repeat: no-repeat; white-space: nowrap;
                                                                        background-color: transparent; width: 3px;">
                                                                    </td>
                                                                </tr>--%>
                                                                <%--<tr>
                                                                   <td align="left" class="Label" style="width: 100%; white-space: nowrap; height: 110px">
                                                                        Project Name/Request Id:
                                                                        <asp:TextBox ID="txtproject" runat="server" CssClass="textBox"
                                                                            Width="362px"></asp:TextBox>
                                                                            <asp:Button ID="btnSetProject" runat="server" Style="display: none" Text=" Project" />
                                                                        <asp:HiddenField ID="HProjectId" runat="server" />
                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtenderWorkspace" runat="server" BehaviorID="AutoCompleteExtenderWorkspace"
                                                                            CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                                            CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelectedWorkspace"
                                                                            OnClientShowing="ClientPopulatedWorkspace" ServiceMethod="GetProjectCompletionListWithOutSponser" ServicePath="AutoComplete.asmx"
                                                                            TargetControlID="txtProject" UseContextKey="True">
                                                                        </cc1:AutoCompleteExtender>
                                                                    </td>
                                                                    <td align="left" style="vertical-align: middle; width: 3px; background-repeat: no-repeat;
                                                                        white-space: nowrap; height: 110px; background-color: transparent">
                                                                    </td>
                                                                </tr>--%>
                                                                <tr>
                                                                    <td align="left" style="white-space: nowrap; width: 100%;">
                                                                        <asp:Label ID="lblSuject" runat="server" SkinID="lblDisplay" Text="Subject: " CssClass="Label"></asp:Label>
                                                                        <asp:TextBox ID="txtSubject" runat="server" CssClass="textBox" TabIndex="2" Width="480px"></asp:TextBox>
                                                                        <asp:Button ID="btnSubject" runat="server" Style="display: none" Text="Subject" />
                                                                        <asp:Button ID="BtnQC" OnClientClick="return QCDivShowHide('S');" runat="server"
                                                                            CssClass="btn btnnew" Text="QC" />&nbsp;
                                                                    <asp:HiddenField ID="HSubjectId" runat="server" />
                                                                        <asp:HiddenField ID="HScrNo" runat="server" />
                                                                        <asp:HiddenField ID="HSchema" runat="server" />
                                                                        <asp:HiddenField ID="HScreenDate" runat="server" />
                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                                                            MinimumPrefixLength="1" OnClientItemSelected="OnSelected" OnClientShowing="ClientPopulated"
                                                                            ServiceMethod="GetSubjectCompletionList_NotRejected" ServicePath="AutoComplete.asmx"
                                                                            TargetControlID="txtSubject" UseContextKey="True">
                                                                        </cc1:AutoCompleteExtender>
                                                                        <cc1:ModalPopupExtender ID="MPEAction" runat="server" PopupControlID="divDocAction"
                                                                            PopupDragHandleControlID="LblPopUpSubMgmt" BackgroundCssClass="modalBackground"
                                                                            BehaviorID="MPEAction" CancelControlID="ImgPopUpClose" TargetControlID="btnShow">
                                                                        </cc1:ModalPopupExtender>
                                                                        <table>
                                                                            <tr style="display: none">
                                                                                <td>
                                                                                    <asp:Button ID="Button2" runat="server" Text="Show" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <div id="divDocAction" runat="server" class="centerModalPopup" style="display: none; max-height: 500px">
                                                                            <div style="width: 100%">
                                                                                <h1 class="header">
                                                                                    <label id="lblDocAction" class="labelbold">
                                                                                        AuditTrail
                                                                                    </label>
                                                                                    <img id="ImgPopUpClose" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
                                                                                </h1>
                                                                            </div>
                                                                            <asp:Panel ID="pnlDocAction" runat="server" Style="max-height: 500px; width: 100%; overflow: auto">
                                                                                <table style="width: 100%" cellpadding="3">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:GridView ID="GVAuditFnlRmk" runat="server" Font-Size="Small" SkinID="grdViewSmlSize"
                                                                                                AutoGenerateColumns="False" BorderColor="Peru">
                                                                                                <Columns>
                                                                                                    <asp:BoundField HeaderText="SrNo">
                                                                                                        <ItemStyle Wrap="false" HorizontalAlign="Center" Width="10" />
                                                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="dScreenDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Screen Date">
                                                                                                        <ItemStyle Wrap="false" HorizontalAlign="Center" Width="10" />
                                                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="vRemark" HeaderText="Remarks">
                                                                                                        <ItemStyle Wrap="false" HorizontalAlign="Center" Width="10" />
                                                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="ModifyBy" HeaderText="Modify By">
                                                                                                        <ItemStyle Wrap="false" HorizontalAlign="Center" Width="10" />
                                                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="dModifyOn" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Modified On"
                                                                                                        HtmlEncode="False">
                                                                                                        <ItemStyle Wrap="false" HorizontalAlign="Center" Width="10" />
                                                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="nMedExScreeningHdrNo" HeaderText="MedExScreeningHdrNo" />
                                                                                                </Columns>
                                                                                            </asp:GridView>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </asp:Panel>
                                                                        </div>
                                                                        <div id="divQCDtl" runat="server" class="DIVSTYLE2" style="display: none; left: 389px; width: 800px; top: 316px; text-align: left; height: 336px;">
                                                                            <asp:Panel ID="pnlQCDtl" runat="server" Width="800px" ScrollBars="Auto" Height="336px">
                                                                                <table>
                                                                                    <tr class="TR">
                                                                                        <td align="left" class="Label" style="width: 14%"></td>
                                                                                        <td align="left" style="width: 80%">
                                                                                            <asp:RadioButtonList ID="RBLQCFlag" runat="server" RepeatDirection="Horizontal" Visible="False">
                                                                                                <asp:ListItem Value="Y">Approve</asp:ListItem>
                                                                                                <asp:ListItem Value="N">Reject</asp:ListItem>
                                                                                                <asp:ListItem Selected="True" Value="F">Feedback</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr class="TR">
                                                                                        <td align="left" class="Label" colspan="2">
                                                                                            <asp:Label ID="lblResponse" runat="server"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr class="TR">
                                                                                        <td align="left" class="Label" style="width: 14%">Remarks :
                                                                                        </td>
                                                                                        <td align="left" colspan="3" style="width: 80%">
                                                                                            <textarea id="txtQCRemarks" onkeydown="ValidateRemarks(this,'lblcnt',1000);" runat="server"
                                                                                                class="textBox" style="width: 277px"></textarea>
                                                                                            <asp:Label ID="lblcnt" runat="server"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr class="TR">
                                                                                        <td align="left" class="Label" style="width: 14%">To :
                                                                                        </td>
                                                                                        <td align="left" colspan="3" style="width: 80%">
                                                                                            <asp:TextBox ID="txtToEmailId" runat="server" CssClass="textBox" Width="277px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr class="TR">
                                                                                        <td align="left" class="Label" style="width: 14%">CC :
                                                                                        </td>
                                                                                        <td align="left" colspan="3" style="width: 80%">
                                                                                            <asp:TextBox ID="txtCCEmailId" runat="server" CssClass="textBox" Height="37px" TextMode="MultiLine"
                                                                                                Width="278px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr class="TR">
                                                                                        <td align="left" style="height: 32px">&nbsp;
                                                                                        </td>
                                                                                        <td align="left" colspan="3" class="Label" style="height: 32px">
                                                                                            <%--<input id="BtnQCSave" runat="server" class="button" style="width: 91px" type="button" value="Save & Send" />--%>
                                                                                            <asp:Button ID="BtnQCSave" runat="server" CssClass="button" Style="width: 91px" Text="Save"
                                                                                                OnClientClick="return ValidationQC();" />&nbsp;<asp:Button ID="BtnQCSaveSend" runat="server"
                                                                                                    CssClass="btn btnsave" OnClientClick="return ValidationQC();" Style="width: 91px"
                                                                                                    Text="Save & Send" />
                                                                                            <input id="Button1" runat="server" class="btn btnnew" onclick="QCDivShowHide('H');" type="button"
                                                                                                value="Close" />
                                                                                            <asp:Button ID="btnDeleteScreenNo" runat="server" />
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
                                                                        <asp:RadioButtonList ID="rblScreeningDate" runat="server" AutoPostBack="True" RepeatColumns="3"
                                                                            RepeatDirection="Horizontal" CssClass="Label">
                                                                        </asp:RadioButtonList>
                                                                    </td>
                                                                    <td align="left" style="vertical-align: middle; background-repeat: no-repeat; white-space: nowrap; background-color: transparent; width: 3px;"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" align="left" style="width: 100%; height: 265px;">
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td align="left" valign="top" style="height: 241px">
                                                                                    <%--<asp:Label ID="lblXRayMsg" runat="server" SkinID="lblError" CssClass="Label"></asp:Label><br />--%>
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
                                                                                    <div id="divHistoryDtl" runat="server" class="DIVSTYLE2" style="display: none; left: 391px; width: 650px; top: 595px; height: 300px; text-align: left; overflow: auto;">
                                                                                        <table style="width: 100%">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <strong style="white-space: nowrap; padding-left: 10px">Attribute History Of
                                                                                                    <asp:Label ID="lblMedexDescription" runat="server"></asp:Label></strong>
                                                                                                </td>
                                                                                                <td align="right" style="width: 56px">
                                                                                                    <img onclick="HistoryDivShowHide('H','','','');" src="images/close.gif" style="width: 21px; height: 15px" />
                                                                                                    &nbsp;
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2" style="height: 260px">
                                                                                                    <asp:Panel ID="pnlHistoryDtl" runat="server" Height="250px" ScrollBars="Auto" Width="625px"
                                                                                                        Style="padding-left: 10px; overflow: auto; overflow: scroll">
                                                                                                        <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                                                                                                        <br />
                                                                                                        <asp:GridView ID="GVHistoryDtl" runat="server" AutoGenerateColumns="False" BorderColor="Peru"
                                                                                                            Font-Size="Small" SkinID="grdViewSmlSize">
                                                                                                            <Columns>
                                                                                                                <asp:BoundField DataField="vSubjectId" HeaderText="Subject Id">
                                                                                                                    <ItemStyle Wrap="false" HorizontalAlign="Center" Width="10" />
                                                                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                                                </asp:BoundField>
                                                                                                                <asp:BoundField DataField="dScreenDate_IST" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Screening Date"
                                                                                                                    HtmlEncode="False">
                                                                                                                    <ItemStyle Wrap="false" HorizontalAlign="Center" Width="10" />
                                                                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                                                </asp:BoundField>
                                                                                                                <asp:BoundField DataField="vMedExDesc" HeaderText="Attribute">
                                                                                                                    <ItemStyle Wrap="false" HorizontalAlign="Center" Width="20" />
                                                                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                                                </asp:BoundField>
                                                                                                                <asp:BoundField DataField="iTranNo" HeaderText="Sr. No." />
                                                                                                                <asp:BoundField DataField="vDefaultValue" HeaderText="Value" />
                                                                                                                <%--<asp:BoundField DataField="vRemark" HeaderText="Remarks" >
                                                                                                                    <ItemStyle Width="20%" />
                                                                                                                </asp:BoundField>--%>
                                                                                                                <asp:BoundField DataField="vModifyBy" HeaderText="Modify By">
                                                                                                                    <ItemStyle Wrap="false" HorizontalAlign="Center" Width="10" />
                                                                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                                                </asp:BoundField>
                                                                                                                <asp:BoundField DataField="dModifyOn_IST" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Modify On"
                                                                                                                    HtmlEncode="False">
                                                                                                                    <ItemStyle Wrap="false" HorizontalAlign="Center" Width="10" />
                                                                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                                                </asp:BoundField>
                                                                                                            </Columns>
                                                                                                        </asp:GridView>
                                                                                                    </asp:Panel>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div style="display: none; width: 451px; left: 168px; top: 460px; text-align: center"
                                                                                        id="divAudit" class="DIVSTYLE2" runat="server">
                                                                                        <table>
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td style="height: 20px" align="right">
                                                                                                        <strong style="white-space: nowrap">Remarks History </strong>
                                                                                                    </td>
                                                                                                    <td align="right">
                                                                                                        <img style="width: 21px; height: 15px" onclick="DivAuditShowHide('H');" src="images/close.gif"
                                                                                                            id="IMG2" />
                                                                                                        &nbsp;
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td align="center" colspan="2"></td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>
                                                                                    </div>
                                                                                    <asp:Button ID="btnSetControlValue" runat="server" CssClass="btn btnnew" Style="display: none"
                                                                                        Text="Set Control Value" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" style="width: 100%">
                                                                        <table style="width: 890px">
                                                                            <tr>
                                                                                <td align="left" style="width: 50%; height: 21px;">
                                                                                    <asp:Button ID="BtnPrevious" runat="server" CssClass="btn btnnew" Text="<< Previous"
                                                                                        />
                                                                                </td>
                                                                                <td align="right" style="width: 50%; height: 21px;">
                                                                                    <asp:Button ID="BtnNext" runat="server" CssClass="btn btnnew" Text="Next >>"  />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center" colspan="2" style="width: 890px; white-space: nowrap">
                                                                        <%-- <asp:Label runat="server" ID="lblRemarks" Text="Remarks:" CssClass="Label"></asp:Label><asp:TextBox
                                                                        ID="txtremark" runat="server" Height="55px" TextMode="MultiLine" Width="363px"
                                                                        onkeydown="ValidateRemarks(this,'lblRcnt',1000);"></asp:TextBox><asp:Label ID="lblRcnt"
                                                                            runat="server"></asp:Label>--%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" align="center" style="width: 890px; height: 21px; white-space: nowrap">
                                                                        <asp:Button ID="btnHistory" runat="server" CssClass="btn btnnew" Style="display: none"
                                                                            Text="History" />&nbsp;
                                                                    <%-- btnRmkHistory--%>
                                                                        <asp:Button ID="btnRmkHistory" runat="server" CssClass="btn btnnew" Text="Remarks History"
                                                                           />
                                                                        <%--<asp:Button ID="BtnSave" runat="server" CssClass="button" Text="Save" />--%>
                                                                        <asp:Button ID="BtnExit" runat="server" CssClass="btn btnexit" Text="Exit" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
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
                    <p align="center">

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
            <table style="width: 400px" align="center" cellspacing="10">
                <tr align="center">
                    <td align="center" colspan="2" class="Label ">
                        <strong>User Authentication</strong>
                    </td>
                </tr>
                <tr>
                    <td class="Label" align="Right">Name :
                    </td>
                    <td class="Label" align="left">
                        <asp:Label runat="server" ID="lblSignername"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="Label" align="Right">Designation :
                    </td>
                    <td class="Label" align="left">
                        <asp:Label runat="server" ID="lblSignerDesignation"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="Label" align="Right">Remarks :
                    </td>
                    <td align="left">
                        <label class="Label">
                            I attest to the accuracy and integrity of the data being reviewed.</label>
                    </td>
                </tr>
                <tr>
                    <td class="Label" align="Right">Password :
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtPassword" runat="Server" Text="" CssClass="textbox" TextMode="Password"> </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="btnAuthenticate" runat="server" Text="Authenticate" CssClass="btn btnnew"
                            OnClientClick="return ValidationForAuthentication();"></asp:Button>
                        <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btnclose" OnClientClick="return DivAuthenticationHideShow();"></asp:Button>
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
                                <img id="Img3" src="~/Images/showQuery.png" runat="server" alt="Confirmation" />
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
        <asp:HiddenField ID="HMedex_Eligibilitydeclaredby" runat="server" />
        <asp:HiddenField ID="HMedex_Eligibilitydeclaredon" runat="server" />
    </form>

    <script type="text/javascript">

        var ACTUAL_SESSIONTIME = "<%= Session.Timeout %>", timerId, sessionFlag = true;
        SessionTimeSet();

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
            document.getElementById('00476_0').checked = false;
            document.getElementById('00476_1').checked = false;
            document.getElementById('11171').value = "";
            document.getElementById('11172').value = "";
        }


    </script>

</body>
</html>
