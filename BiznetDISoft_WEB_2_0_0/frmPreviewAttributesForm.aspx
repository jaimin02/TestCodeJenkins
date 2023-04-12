<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmPreviewAttributesForm.aspx.vb"
    Inherits="frmPreviewAttributesForm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <%--<title>Subject Medical Examination </title>--%>
    <link href="App_Themes/StyleCommon/CommonStyle.css" rel="Stylesheet" type="text/css" />
    <link href="App_Themes/sweetalert.css" rel="stylesheet" type="text/css" />

    <script src="Script/Validation.js" language="javascript" type="text/javascript"></script>
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script src="Script/jquery.min.js" language="javascript" type="text/javascript"></script>
    <script src="Script/sweetalert.js" language="javascript" type="text/javascript"></script>
    <link rel="shortcut icon" type="image/x-icon" href="images/biznet.ico" />

    <style type="text/css">
        canvas {
            height: 65px !important;
        }
    </style>

    <script language="javascript" type="text/javascript">
        var currTab;
        function closewindow() {
            msgConfirmDeleteAlert(null, "Are you sure want to Exit ?", function (isConfirmed) {
                if (isConfirmed) {
                    self.close();
                    return true;
                } else {

                    return false;
                }
            });
        }

        $(document).ready(function () {
            //added by shivani pandya for apply css
            $('select').removeAttr("class");
            $('select').attr("class", "dropDownList");

            //for datatable dropdown
            $(".dataTables_wrapper").removeAttr("class");
            $(".dataTables_wrapper").find('select').attr("class", "dropDownList");

            //for file upload
            $('[type=file]').removeAttr("class");
            $('[type=file]').attr("class", "textboxFileUpload");

            //for textbox
            $('[type=text]').removeAttr("class");
            $('[type=text]').attr("class", "textBox");

            //for datatable search textbox
            $(".dataTables_filter").find('[type=text]').removeAttr("class");
            $(".dataTables_filter").find('[type=text]').attr("class", "textBox");

            //button
            $('[type=submit]').removeAttr("class");
            $('[type=submit]').attr("class", "button");

            $('[type=reset]').removeAttr("class");
            $('[type=reset]').attr("class", "button");

            //multiline textbox
            $("textarea").removeAttr("class");
            $("textarea").attr("class", "textBox");

            selectTheme();
        });

        jQuery(window).focus(function () {
            selectTheme();
            return false;
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
                    txt.focus();
                    msgalert(msg);
                }
            }

            if (HighRange != 0 || LowRange != 0) {
                if (txt.value > HighRange || txt.value < LowRange) {
                    txt.value = '';
                    txt.focus();
                    msgalert('Out Of Range! Range Must Be Between ' + LowRange + ' to ' + HighRange);
                }
            }

        }

        function Next(NoneDivId) {
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
                    isShow = false;
                    break;
                }
                if (arrDiv[i].toLowerCase() == currTab.toLowerCase()) {
                    isShow = true;
                }
            }
            var currBtn = currTab.replace('Div', 'BtnDiv');
            document.getElementById(currTab).style.display = 'block';
            //document.getElementById(currBtn).style.color='navy';
            document.getElementById(currBtn).style.color = '#FFC300';
            return false;
        }

        function Previous(NoneDivId) {
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
                        break;
                    }
                }
            }
            var currBtn = currTab.replace('Div', 'BtnDiv');
            document.getElementById(currTab).style.display = 'block';
            //document.getElementById(currBtn).style.color='navy';
            document.getElementById(currBtn).style.color = '#FFC300';
            return false;
        }

        function DisplayDiv(BlockDivId, NoneDivId) {
            var selBtn = BlockDivId.replace('Div', 'BtnDiv');
            var arrDiv = NoneDivId.split(',');
            for (i = 0; i < arrDiv.length; i++) {
                document.getElementById(arrDiv[i]).style.display = 'none';
                var disBtn = arrDiv[i].replace('Div', 'BtnDiv');
                //document.getElementById(disBtn).style.color='Brown';
                document.getElementById(disBtn).style.color = '#284E98';
                if (selBtn.toLowerCase() == disBtn.toLowerCase()) {
                    currTab = arrDiv[i];
                }
            }
            document.getElementById(BlockDivId).style.display = 'block';
            //document.getElementById(selBtn).style.color='navy';
            document.getElementById(selBtn).style.color = '#FFC300';
            return false;
        }

        function DisplayDivFromDDL(BlockDivId, NoneDivId) {
            var selBtn = $("#ddlGroup option:selected").val().replace('Div', 'BtnDiv');
            var arrDiv = NoneDivId.split(',');
            for (i = 0; i < arrDiv.length; i++) {
                document.getElementById(arrDiv[i]).style.display = 'none';
                var disBtn = arrDiv[i].replace('Div', 'BtnDiv');
                //document.getElementById(disBtn).style.color = '#FFffff';
                if (selBtn.toLowerCase() == disBtn.toLowerCase()) {
                    currTab = arrDiv[i];
                }
            }
            //document.getElementById(BlockDivId).style.display = 'block';
            document.getElementById(selBtn.replace("Btn", "")).style.display = 'block';
            document.getElementById(selBtn.replace("Btn", "")).style.color = '#FFC300';
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

        function C2F(txtCelsiusID, txtFahrenheitID) {
            var txtCelsius = document.getElementById(txtCelsiusID);
            var txtFahrenheit = document.getElementById(txtFahrenheitID);
            var result;

            if (!(CheckDecimalOrBlank(txtCelsius.value))) {
                msgalert('Please Enter Valid Temperature In Celsius !');
                txtCelsius.focus();
                return false;
            }
            txtFahrenheit.value = c2f(txtCelsius.value);
            return true;
        }

        function F2C(txtFahrenheitID, txtCelsiusID) {
            var txtCelsius = document.getElementById(txtCelsiusID);
            var txtFahrenheit = document.getElementById(txtFahrenheitID);
            var result;

            if (!(CheckDecimalOrBlank(txtFahrenheit.value))) {
                msgalert('Please Enter Valid Temperature In Fahrenheit !');
                txtFahrenheit.focus();
                return false;
            }
            txtCelsius.value = f2c(txtFahrenheit.value);
            return true;
        }

        function DateValidationForCTM(ParamDate, txtdate, IsNotNull) {
            txtdate.style.borderColor = "";
            if (IsNotNull == 'Y') {
                if (ParamDate.trim() == '') {
                    msgalert('Field can not be left blank !');
                    txtdate.style.borderColor = "Red";
                    return;
                }
            }
            if (ParamDate.trim() != '') {
                //Format Change Start
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
                        return true;
                    }
                    //alert('Please Enter Date in DDMMYYYY or dd-Mon-YYYY format only.');
                    txtdate.value = "";
                    txtdate.focus();
                    return false;
                }
                //End Format change
                DateConvert(txtdate.value, txtdate);
                dt = txtdate.value;
                var Year = dt.substring(dt.lastIndexOf('-') + 1);
                var inyear;
                inyear = parseInt(Year, 10);
                if (inyear < 1900) {
                    msgalert('You Can Not Add Date Which Is Less Than "01-01-1900" !');
                    txtdate.value = "";
                    txtdate.focus();
                }

                var flg = false;
                flg = DateConvert(ParamDate, txtdate);
                if (flg == true && !CheckDateLessThenToday(txtdate.value)) {
                    msgalert('Date Should Be Less Than Current Date !');
                    txtdate.value = "";
                    txtdate.focus();
                    return false;
                }
            }
            return true;
        }

        // add by shivani pandya For theme selection
        function selectTheme() {
            var theme;
            jQuery.each(document.cookie.split(";"), function (i, para) {
                if (para == " Theme=Orange" || para == "Theme=Orange") {
                    theme = para;
                }
                if (para == " Theme=Green" || para == "Theme=Green") {
                    theme = para;
                }
                if (para == " Theme=Demo" || para == "Theme=Demo") {
                    theme = para;
                }
                if (para == " Theme=Blue" || para == "Theme=Blue") {
                    theme = para;
                }
            });
            if (theme != "") {
                jQuery.each(jQuery("link"), function () {
                    if (jQuery(this).attr("href") == "App_Themes/StyleBlue/StyleBlue.css" || jQuery(this).attr("href") == "App_Themes/StyleGreen/GreenStyle.css" || jQuery(this).attr("href") == "App_Themes/StyleDemo/DemoStyle.css" || jQuery(this).attr("href") == "App_Themes/StyleOrange/orange.css") {
                        if (theme == " Theme=Orange" || theme == "Theme=Orange") {
                            jQuery(this).attr("href", "App_Themes/StyleOrange/orange.css");
                        }
                        if (theme == " Theme=Green" || theme == "Theme=Green") {
                            jQuery(this).attr("href", "App_Themes/StyleGreen/GreenStyle.css");
                        }
                        if (theme == " Theme=Demo" || theme == "Theme=Demo") {
                            jQuery(this).attr("href", "App_Themes/StyleDemo/DemoStyle.css");
                        }
                        if (theme == " Theme=Blue" || theme == "Theme=Blue") {
                            jQuery(this).attr("href", "App_Themes/StyleBlue/StyleBlue.css");
                        }
                    }
                });
            }
            assignCSS();
            return true;
        }

        // add by shivani pandya For theme selection
        function assignCSS() {
            var theme;
            var footer = "";
            jQuery.each(document.cookie.split(";"), function (i, para) {
                if (para == " Theme=Orange" || para == "Theme=Orange") {
                    theme = para;
                }
                if (para == " Theme=Green" || para == "Theme=Green") {
                    theme = para;
                }
                if (para == " Theme=Demo" || para == "Theme=Demo") {
                    theme = para;
                }
                if (para == " Theme=Blue" || para == "Theme=Blue") {
                    theme = para;
                }
            });

            if (theme == " Theme=Orange" || theme == "Theme=Orange") {
                jQuery("#ctl00_lblMandatory").css({ 'border-color': '#CF8E4C' });

                jQuery("table[rules] tr[valign=top]").removeAttr("style");
                jQuery("table[rules] tr[valign=top]").removeAttr("class");
                jQuery("table[rules] tr[valign=top]").attr("class", "trHeader");

                footer = jQuery("table[rules] tr[align=center]")[1];
                jQuery(footer).removeAttr("style");
                jQuery(footer).removeAttr("class");
                jQuery(footer).attr("class", "trFooter");

                jQuery("#ctl00_lblHeading").removeAttr("style");

                jQuery("#ctl00_lblHeading").attr("class", "Labelheading");

                jQuery("#ctl00_CPHLAMBDA_divExpandable").css({ 'background-color': '#CF8E4C' });
                jQuery("#ctl00_CPHLAMBDA_LblProjectPreClinical").css({ 'color': 'darkred' });
                jQuery("#ctl00_CPHLAMBDA_LblProjectsClinincalPhase").css({ 'color': 'darkred' });
                jQuery("#ctl00_CPHLAMBDA_LblAnalyticalPhase").css({ 'color': 'darkred' });
                jQuery("#ctl00_CPHLAMBDA_LblDocumentPhase").css({ 'color': 'darkred' });

                $("#ctl00_CPHLAMBDA_pnlMedExGrid table tr").last().css({ "background-color": "#CF8E4C" });
                $("#LabUOM00731").css({ 'color': '#CF8E4C' });

            }
            if (theme == " Theme=Green" || theme == "Theme=Green") {
                jQuery("#ctl00_lblHeading").removeAttr("style");
                jQuery("#ctl00_lblHeading").attr("class", "Labelheading");

                jQuery("#ctl00_CPHLAMBDA_divExpandable").css({ 'background-color': '#33a047' });
                jQuery("#ctl00_lblMandatory").css({ 'border-color': '#33a047' });

                jQuery("table[rules] tr[valign=top]").removeAttr("style");
                jQuery("table[rules] tr[valign=top]").removeAttr("class");
                jQuery("table[rules] tr[valign=top]").attr("class", "trHeader");

                footer = jQuery("table[rules] tr[align=center]")[1];
                jQuery(footer).removeAttr("style");
                jQuery(footer).removeAttr("class");
                jQuery(footer).attr("class", "trFooter");

                jQuery("#ctl00_CPHLAMBDA_divExpandable").css({ 'background-color': ' #FF8000' });
                jQuery("#ctl00_CPHLAMBDA_LblProjectPreClinical").css({ 'color': ' #FF8000' });
                jQuery("#ctl00_CPHLAMBDA_LblProjectsClinincalPhase").css({ 'color': ' #FF8000' });
                jQuery("#ctl00_CPHLAMBDA_LblAnalyticalPhase").css({ 'color': '#FF8000' });
                jQuery("#ctl00_CPHLAMBDA_LblDocumentPhase").css({ 'color': '#FF8000' });

                $("#ctl00_CPHLAMBDA_pnlMedExGrid table tr").last().css({ "background-color": "#33a047" });
                $("#LabUOM00731").css({ 'color': '#33a047' });

            }
            if (theme == " Theme=Demo" || theme == "Theme=Demo") {
                jQuery("#ctl00_lblHeading").removeAttr("style");
                jQuery("#ctl00_lblHeading").attr("class", "Labelheading");

                jQuery("#ctl00_CPHLAMBDA_divExpandable").css('background-color', '#999966');
                jQuery("#ctl00_lblMandatory").css({ 'border-color': '#CF8E4C' });

                jQuery("table[rules] tr[valign=top]").removeAttr("style");
                jQuery("table[rules] tr[valign=top]").removeAttr("class");
                jQuery("table[rules] tr[valign=top]").attr("class", "trHeader");

                footer = jQuery("table[rules] tr[align=center]")[1];
                jQuery(footer).removeAttr("style");
                jQuery(footer).removeAttr("class");
                jQuery(footer).attr("class", "trFooter");

                jQuery("#ctl00_CPHLAMBDA_divExpandable").css({ 'background-color': '#FF8000' });
                jQuery("#ctl00_CPHLAMBDA_LblProjectPreClinical").css({ 'color': '#FF8000' });
                jQuery("#ctl00_CPHLAMBDA_LblProjectsClinincalPhase").css({ 'color': '#FF8000' });
                jQuery("#ctl00_CPHLAMBDA_LblAnalyticalPhase").css({ 'color': '#FF8000' });
                jQuery("#ctl00_CPHLAMBDA_LblDocumentPhase").css({ 'color': '#FF8000' });

                $("#ctl00_CPHLAMBDA_pnlMedExGrid table tr").last().css({ "background-color": "#999966" });
                $("#LabUOM00731").css({ 'color': '#999966' });

            }
            if (theme == " Theme=Blue" || theme == "Theme=Blue") {
                jQuery("#ctl00_lblHeading").removeAttr("style");
                jQuery("#ctl00_lblHeading").attr({ "class": "Labelheading" });

                jQuery("#ctl00_CPHLAMBDA_divExpandable").css({ 'background-color': 'Navy' });
                jQuery("#ctl00_lblMandatory").css({ 'border-color': '#1560a1' });

                jQuery("table[rules] tr[valign=top]").removeAttr("style");
                jQuery("table[rules] tr[valign=top]").removeAttr("class");
                jQuery("table[rules] tr[valign=top]").attr("class", "trHeader");

                footer = jQuery("table[rules] tr[align=center]")[1];
                jQuery(footer).removeAttr("style");
                jQuery(footer).removeAttr("class");
                jQuery(footer).attr("class", "trFooter");

                jQuery("#ctl00_CPHLAMBDA_divExpandable").css({ 'background-color': '#FF8000' });
                jQuery("#ctl00_CPHLAMBDA_LblProjectPreClinical").css({ 'color': '#FF8000' });
                jQuery("#ctl00_CPHLAMBDA_LblProjectsClinincalPhase").css({ 'color': '#FF8000' });
                jQuery("#ctl00_CPHLAMBDA_LblAnalyticalPhase").css({ 'color': '#FF8000' });
                jQuery("#ctl00_CPHLAMBDA_LblDocumentPhase").css({ 'color': '#FF8000' });

                $("#ctl00_CPHLAMBDA_pnlMedExGrid table tr").last().css({ "background-color": "#1560a1" });
                $("#LabUOM00731").css({ 'color': '#1560a1' });
            }
            return false;
        }

    </script>

</head>
<body>
    <form id="form1" runat="server" method="post">
        <input style="display: none" type="text" name="fakeusernameremembered" />
        <input style="display: none" type="password" name="fakepasswordremembered" />
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="1000"
            EnablePageMethods="True">
            <Services>
                <asp:ServiceReference Path="AutoComplete.asmx" />
            </Services>
        </asp:ScriptManager>
        <table style="margin: auto;">
            <tr>
                <td>
                    <asp:Label ID="lblErrorMsg" runat="server" SkinID="lblError"></asp:Label>
                </td>
            </tr>
        </table>
        <table style="border-collapse: collapse; margin: auto;" class="table " width="60%"
            id="AutoNumber1">
            <asp:HiddenField ID="CompanyName" runat="server" />
            <tr style="height: 7px;">
                <td style="vertical-align: bottom; width: 100%; background-repeat: no-repeat; background-size: 100%; padding-bottom: 42px; border-spacing: 0px" align="left">
                    <div style="text-align: right;">
                        <table width="100%">
                            <tr style="height: 65px">
                                <td style="vertical-align: bottom; width: 100%; height: 65px; text-align: left;">
                                    <%--<asp:Label ID="lblWelcome" runat="server" CssClass="Label" Text="Welcome, " Style="color: #000; font-size: 15px;" />
                                    <asp:Label ID="lblUserName" runat="server" CssClass="Label" Style="color: blanchedalmond; text-shadow: 0px 0px 20px #000; font-size: 14px;" />
                                    <asp:Label ID="lblLoginTime1" runat="server" CssClass="Label" Text="Login Time: "
                                        Style="color: #000; font-size: 15px;"></asp:Label><asp:Label ID="lblTime" runat="server" CssClass="Label"
                                            Style="color: blue; text-shadow: 0px 0px 20px #000; opacity: 2.22; font-size: 12px;" />
                                    <asp:Label runat="server" ID="lblSessionTimeCap" class="Label" Style="color: #000; width: 14px; font-weight: bold;"
                                        Text="Session Expires:"></asp:Label>
                                    <b><span class="Label" style="color: blue; text-shadow: 0px 0px 20px #000; font-size: 12px; opacity: 2.22;" id="timerText"></span></b>--%>

                                    <div style="background-image: url(images/left1.jpg); background-repeat: repeat-x; width: 100%; height: 65px;">

                                        <div style="padding: 5px; position: absolute; z-index: 999;">
                                            <img src="Images/biznet-logo.png" alt="biznet logo left" width="60" id="LogoImg" />
                                        </div>
                                        <div style="float: right; width: 100%;">
                                            <div id="qodef-particles" class="fixed" style="height: 65px; background-color: #e4e4e4;" data-particles-density="high"
                                                data-particles-color="#f9a54b" data-particles-opacity="0.8" data-particles-size="3" data-speed="3" data-show-lines="yes"
                                                data-line-length="100" data-hover="yes" data-click="yes">
                                                <div id="qodef-p-particles-container" style="height: 65px;">
                                                    <canvas class="particles-js-canvas-el" width="1349" height="65" style="width: 100%; height: 10%;"></canvas>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div style="clear: both; position: relative; margin-top: -65px; float: right; width: 50%;">
                                        <table style="width: 118%; border: 0 solid #111111; text-align: right; float: right;">
                                            <tr style="height: 35px">
                                                <td style="white-space: nowrap; vertical-align: top; height: 35px;">
                                                    <i class="fa fa-clock-o SessionImageClock" aria-hidden="true"></i>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="white-space: nowrap; float: left">
                                                    <div id="DivSessionTimingWatch" class="SessionTiming">
                                                        <asp:Label ID="lblTime" runat="server" CssClass="Label" Style="color: blue; text-shadow: 0px 0px 20px #000; opacity: 2.22; font-size: 12px;" Visible="false" />
                                                        <asp:Label runat="server" ID="lblSessionTimeCap" CssClass="Label" Style="color: Black; font-size: 15px; width: 14px; font-weight: bold;"
                                                            Text="Session Expires In: "></asp:Label>
                                                        <b><span class="Label headerusername" id="timerText"></span></b>
                                                        <asp:HiddenField ID="HDSessionValue" runat="server" Value='<%=Session("UserId")%>' />
                                                    </div>
                                                </td>
                                            </tr>
                                            <div class="Manage">
                                                <span id="lblWelcome" class="Label" style="color: #000; font-size: 15px;">Welcome :</span>
                                                <asp:Label ID="lblUserName" runat="server" CssClass="Label headerusername" />
                                            </div>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="text-align: center;">
                        <table>
                            <tr>
                                <td>
                                    <asp:Panel ID="Pan_Hdr" runat="server" Width="100%" CssClass="InnerTable" BackColor="White">
                                        <asp:Panel ID="Pan_Child" runat="server" Width="100%" BackColor="Window">
                                            <div id="Header Label" style="text-align: center;" align="center" class="Div">
                                                <table align="center" style="width: 100%">
                                                    <tr align="center">
                                                        <td align="center">
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td align="center" style="width: 100%; font-size: 20px; font-weight: bold;">Preview Of
                                                                    <asp:Label ID="lblProject" runat="server" CssClass="Label " Font-Bold="true" Font-Size="20px" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center" style="width: 100%">
                                                                        <asp:Label ID="lblTemplateName" runat="server" CssClass="Label " Font-Bold="true"
                                                                            Font-Size="Large" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <table style="width: 100%; margin: auto;">
                                                                <tr>
                                                                    <td style="text-align: left;">
                                                                        <asp:HiddenField ID="HFWorkspaceId" runat="server" />
                                                                        <asp:HiddenField ID="HFActivityId" runat="server" />
                                                                        <asp:HiddenField ID="HFTemplateId" runat="server" />
                                                                        <asp:HiddenField ID="HFNodeId" runat="server" />
                                                                        <asp:HiddenField ID="HFWorkspaceHdrScreeningNo" runat="server" />
                                                                        <asp:HiddenField ID="HFScreeningTemplateHdrNo" runat="server" />
                                                                    </td>
                                                                    <td align="left" style="vertical-align: middle; background-repeat: no-repeat; white-space: nowrap; background-color: transparent; width: 5%;"></td>
                                                                </tr>
                                                                <%-- <tr >
                                                                    <td style="text-align: left; ">
                                                                            <asp:DropDownList runat="server" ID="ddlGroup" Style="display: none; margin-bottom:15px !important;">
                                                                            <asp:ListItem Value="0">--Select Group--</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                     <td></td>
                                                                    </tr>--%>
                                                                <tr>
                                                                    <td>
                                                                        <table style="margin: auto; text-align: left; width: 100%;">
                                                                            <tr>
                                                                                <td colspan="2">
                                                                                    <asp:DropDownList runat="server" ID="ddlGroup" Style="display: none; margin-bottom: 15px !important;">
                                                                                        <asp:ListItem Value="0">--Select Group--</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" valign="top">
                                                                                    <asp:UpdatePanel ID="UpPlaceHolder" runat="server" ChildrenAsTriggers="true" EnableViewState="true"
                                                                                        RenderMode="Inline" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:Panel ID="PnlPlaceMedex" runat="server" Width="100%">
                                                                                                <asp:PlaceHolder ID="PlaceMedEx" runat="server" EnableViewState="true" />
                                                                                            </asp:Panel>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="Label" style="width: 100%; text-align: left;">
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td style="text-align: center;">
                                                                                    <asp:Button ID="BtnPrevious" runat="server" CssClass="btn btnnew" Text="<< Previous"
                                                                                        ToolTip="Previous" Visible="false" />
                                                                                    <asp:Button ID="BtnNext" runat="server" CssClass="btn btnnew" Text="Next >>" ToolTip="Next"
                                                                                        Visible="false" />
                                                                                    <input id="BtnExit" class="btn btnexit" type="button" value="Exit" title="Exit"
                                                                                        onclick="closewindow();" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </asp:Panel>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td height="8"></td>
            </tr>
            <tr>
                <td valign="middle" height="18" class="footer_Master">
                    <p align="center">

                        <script type="text/javascript">
                            var copyright;
                            var update;
                            copyright = new Date();
                            update = copyright.getFullYear();
                            document.write("<font face=\"verdana\" size=\"1\" color=\"white\">© Copyright " + update + "," + $get('<%= CompanyName.clientid %>').value + "</font>");
                        </script>
                </td>
            </tr>
        </table>
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

        <script type="text/javascript">
            var ACTUAL_SESSIONTIME = "<%= Session.Timeout %>", timerId, sessionFlag = true;
            SessionTimeSet();
        </script>

    </form>
</body>
<script type="text/javascript" src="Script/Login/third-party.min.js"></script>
<script type="text/javascript" src="Script/Login/modules.min.js"></script>
</html>
