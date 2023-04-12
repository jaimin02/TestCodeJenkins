<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmMainPage, App_Web_vq2225em" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:content id="ConFrmMainPage" contentplaceholderid="CPHLAMBDA" runat="Server"> 
    <link href="App_Themes/NoticeBoard.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/PanelDisplay.css" rel="stylesheet" />

    <style type="text/css">
        #tblSiteWiseSubjectInformation {
            width: 100% !important;
        }

        #tblSiteWiseCAAssignment {
            width: 100% !important;
        }

        #tblSiteWiseGlobalResponse {
            width: 100% !important;
        }

        #tblSiteWiseAdjudicatorResponse {
            width: 100% !important;
        }

         #ctl00_CPHLAMBDA_CalendarExtender1_popupDiv, #ctl00_CPHLAMBDA_CalendarExtender2_popupDiv {
            left: auto !important;
            top: auto !important;
        }

        .hiddenRow {
    padding: 0 !important;
}
        tr.collapse.in {
  display:table-row;
}
        .overlaymonth {
            position: absolute;
            bottom: 20px;
            left: 0px;
            right: 0;
            background-color: #fff;
            overflow: hidden;
            width: 88%;
            height: 0;
            transition: .5s ease;
            margin-left: 75px;
        }

        .overlayCA {
            position: absolute;
            bottom: 20px;
            left: 0px;
            right: 0;
            background-color: #fff;
            overflow: hidden;
            width: 88%;
            height: 0;
            transition: .5s ease;
            margin-left: 75px;
        }

        .overlayGR {
            position: absolute;
            bottom: 20px;
            left: 0px;
            right: 0;
            background-color: #fff;
            overflow: hidden;
            width: 88%;
            height: 0;
            transition: .5s ease;
            margin-left: 75px;
        }

        .overlayAR {
            position: absolute;
            bottom: 20px;
            left: 0px;
            right: 0;
            background-color: #fff;
            overflow: hidden;
            width: 88%;
            height: 0;
            transition: .5s ease;
            margin-left: 75px;
        }

        /*testing by rinkal*/

        /* Style the tab */
        .tab {
            overflow: hidden;
            border: 1px solid #ccc;
            background-color: #f1f1f1;
        }

            /* Style the buttons inside the tab */
            .tab button {
                background-color: inherit;
                float: left;
                border: none;
                outline: none;
                cursor: pointer;
                padding: 9px 16px;
                transition: 0.3s;
                font-size: 17px;
            }

                /* Change background color of buttons on hover */
                .tab button:hover {
                    background-color: #ddd;
                }

                /* Create an active/current tablink class */
                .tab button.active {
                    background-color: #ccc;
                }

        /* Style the tab content */
        .tabcontent {
            display: none;
            padding: 6px 12px;
            border: 1px solid #ccc;
            border-top: none;
        }
        /**/

        .AuditControl, .EditControl {
            cursor: pointer;
        }

        .RBList span, label {
            vertical-align: sub;
        }

        .HideConroll {
            display: none;
        }

        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

        th.ui-state-default {
            color: white !important;
        }

        .ajax__calendar_container {
            z-index: 1;
        }

        .MyLinkButtons {
            text-decoration: underline;
            cursor: pointer;
            display: block;
            margin-right: 20px;
            margin-bottom: 4px;
            background-color: #eaea75;
        }

        .parentdiv span {
            color: white !important;
        }

        .maintr {
            display: none;
            cursor: pointer;
            width: 100%;
            position: inherit;
            top: 20px;
        }

        .maintd {
            vertical-align: middle;
            width: 100%;
            text-align: left;
        }

        .parentdiv {
            background-color: #6894bb;
            color: #FFFFFF;
            height: 20px;
            padding: 5px 3px 0px 3px;
            vertical-align: middle;
            width: 100%;
            -moz-border-bottom-left-radius: 3px;
            -webkit-border-bottom-left-radius: 3px;
            border-top-left-radius: 3px;
            -moz-border-top-left-radius: 3px;
            -webkit-border-top-left-radius: 3px;
            font-weight: bold;
            font-size: 13px;
            float: none;
            text-align: left;
        }

        .buttondiv {
            background-color: #6894bb;
            height: 25px;
            float: right;
            vertical-align: middle;
            margin: 1px;
            border-bottom-right-radius: 3px;
            -moz-border-bottom-right-radius: 3px;
            -webkit-border-bottom-right-radius: 3px;
            border-top-right-radius: 3px;
            -moz-border-top-right-radius: 3px;
            -webkit-border-top-right-radius: 3px;
        }

        .imgExpand {
            margin: 5px;
        }

        .roundedHeader {
            color: #fff !important;
        }

        #loadingmessage {
            display: none;
            position: fixed;
            z-index: 1000;
            top: 0;
            left: 0;
            height: 100%;
            width: 100%;
            background: rgba( 255, 255, 255, .5 ) url('images/AjaxLoader.gif') 50% 50% no-repeat;
        }

        #ctl00_CPHLAMBDA_DivPopUp {
            position: fixed !important;
            z-index: 100001;
            left: 17% !important;
            top: 10% !important;
        }

        .ModalImage {
            cursor: pointer;
        }

        .highcharts-container {
            width: 100% !important;
        }
        /*rect[Attributes Style] {
            fill: rgb(255, 255, 255);
            x: 0;
            y: 0;
            width: 1199;
            height: 350;
            rx: 0;
            ry: 0;
        }*/
        /*#divSiteWiseSubjectInformation .highcharts-background {
            width:100%!important;
        }*/
    </style>

    <style type="text/css">
        .btnAllocation {
            background: url(../../images/export_go.svg);
            background-repeat: no-repeat;
            width: 92px;
            height: 25px;
            background-color: #75acdc;
        }

        /*body
        {
            padding: 0;
            margin: 0;
            height: 100%;
            list-style: none;
            overflow: hidden;
            font-family: 'Lato', Calibri, Arial, sans-serif;
        }*/
        .progressDiv {
            /*width: 84%;
            background: #fcfcfc;
            height: 325px;
            border: 1px solid #ccc;
            position: relative;
            left: 7%;
            top: 100px;
            display: inline-block;
            border-radius: 2px;
            box-shadow: 0px 1px 1px 1px #ccc;*/
        }

        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }
        /* Pie Chart */
        .progress-pie-chart {
            width: 200px;
            height: 200px;
            border-radius: 50%;
            background-color: #E5E5E5;
            position: relative;
        }

            .progress-pie-chart.gt-50 {
                background-color: #81CE97;
            }

        .ppc-progress {
            content: "";
            position: absolute;
            border-radius: 50%;
            left: calc(50% - 100px);
            top: calc(50% - 100px);
            width: 200px;
            height: 200px;
            clip: rect(0, 200px, 200px, 100px);
        }

            .ppc-progress .ppc-progress-fill {
                content: "";
                position: absolute;
                border-radius: 50%;
                left: calc(50% - 100px);
                top: calc(50% - 100px);
                width: 200px;
                height: 200px;
                clip: rect(0, 100px, 200px, 0);
                background: #81CE97;
                transform: rotate(60deg);
            }

        .gt-50 .ppc-progress {
            clip: rect(0, 100px, 200px, 0);
        }

            .gt-50 .ppc-progress .ppc-progress-fill {
                clip: rect(0, 200px, 200px, 100px);
                background: #E5E5E5;
            }

        .ppc-percents {
            content: "";
            position: absolute;
            border-radius: 50%;
            left: calc(50% - 173.91304px/2);
            top: calc(50% - 173.91304px/2);
            width: 173.91304px;
            height: 173.91304px;
            background: #fff;
            text-align: center;
            display: table;
        }

            .ppc-percents span {
                display: block;
                font-size: 2.6em;
                font-weight: bold;
                color: #81CE97;
            }

        .pcc-percents-wrapper {
            display: table-cell;
            vertical-align: middle;
        }

        .progress-pie-chart {
            margin: 50px auto 0;
            margin-left: 5%;
        }

        div.dataTables_wrapper {
            width: 100%; /*850px;*/
            margin: 0 auto;
            /*_height: 12px;
            min-height: 12px;*/
        }
        
        .ajax__calendar_container {
            z-index: 1;
        }

        .LabelColor {
            color: white !important;
        }

        .modal-xl {
            width: 70%;
            left: 15%;
            z-index: 999;
            top: 2%;
        }

        .modalheightxl {
            height: 450px;
            overflow: auto;
        }

        #divMyCalenderHeader {
            cursor: move;
        }

        .modal-header h2 {
            color: white;
        }

        .modal-content {
            resize: both !important;
            overflow: auto;
        }

        .SetHeaderOfMyCalendarAs td {
            color: white !important;
        }

        .modal-header {
            background-color: #56a0da;
        }
    </style>  
     <!--for tabs-->
    <%--<script type="text/javascript" src='https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js'></script>--%>
    <%--<script src="Script/for_tab.js"></script>--%>
    <%--<script type="text/javascript" src="Script/for_tab.js"></script>--%>
    <%--<link href="App_Themes/for_tab.css" rel="stylesheet" />--%>
    <%--<link href="Script/for_tab.css" rel="stylesheet" />--%>  
    <!--for tabs-->

     <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <script type="text/javascript" src="Script/General.js"></script>
    <script type="text/javascript" src="Script/DD_roundies_0.0.2a.js"></script>

 <%-- <script type="text/javascript" src="Script/jquery-1.8.2.js"></script>--%>

    <script type="text/javascript" src="Script/jquery-ui.js"></script>

     <%--<script type="text/javascript" src="Script/highcharts.js"></script>--%>

     <script type="text/javascript" src="Script/highstock.js"></script>
    <script type="text/javascript" src="Script/exporting.js"></script>
    
    <script type="text/javascript" language="javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (args.get_error() != undefined) {
                args.set_errorHandled(true);
            }
        }
    </script>

    <script type="text/javascript">
        function ChartPlus() {
            var str = $("#img2").attr('src');

            if (str.toLowerCase().indexOf('noticeunpin.png') >= 0) {
                $(".overlaymonth").attr("style", "display:block;height:85%;transform: translatey(-0%);width:92.7%;margin-left:50px;margin-bottom:-100px")
                //overlayCA
                $(".highcharts-root").css({ "width": "1197" });
            }
            else {
                $(".overlaymonth").attr("style", "display:block;height:85%;transform: translatey(-0%);width:90%;margin-left:50px;margin-bottom:-100px")
                $(".highcharts-root").css({ "width": "892" });
            }
            return false;
        }

        function TableMinus() {
            var str = $("#img2").attr('src');

            if (str.toLowerCase().indexOf('noticeunpin.png') >= 0) {
                $(".highcharts-root").css({ "width": "1197" });
            }
            else {
                $(".highcharts-root").css({ "width": "892" });
            }
            $(".overlaymonth").attr("style", "display:block;height:0%transform: translateY(-0%);")
        }

        function ChartPlusForCA() {
            var str = $("#img2").attr('src');

            if (str.toLowerCase().indexOf('noticeunpin.png') >= 0) {
                $(".overlayCA").attr("style", "display:block;height:85%;transform: translatey(-0%);width:92.7%;margin-left:50px;margin-bottom:-150px")
                $(".highcharts-root").css({ "width": "1197" });
            }
            else {
                $(".overlayCA").attr("style", "display:block;height:85%;transform: translatey(-0%);width:90%;margin-left:50px;margin-bottom:-150px")
                $(".highcharts-root").css({ "width": "892" });
            }
            return false;
        }

        function TableMinusForCA() {
            var str = $("#img2").attr('src');

            if (str.toLowerCase().indexOf('noticeunpin.png') >= 0) {
                $(".highcharts-root").css({ "width": "1197" });
            }
            else {
                $(".highcharts-root").css({ "width": "892" });
            }
            $(".overlayCA").attr("style", "display:block;height:0%transform: translateY(-0%);")
        }

        function ChartPlusForGR() {
            var str = $("#img2").attr('src');

            if (str.toLowerCase().indexOf('noticeunpin.png') >= 0) {
                $(".overlayGR").attr("style", "display:block;height:85%;transform: translatey(-0%);width:92.7%;margin-left:50px;margin-bottom:-205px")
                $(".highcharts-root").css({ "width": "1197" });
            }
            else {
                $(".overlayGR").attr("style", "display:block;height:85%;transform: translatey(-0%);width:90%;margin-left:50px;margin-bottom:-205px")
                $(".highcharts-root").css({ "width": "892" });
            }
            return false;
        }

        function TableMinusForGR() {
            var str = $("#img2").attr('src');

            if (str.toLowerCase().indexOf('noticeunpin.png') >= 0) {
                $(".highcharts-root").css({ "width": "1197" });
            }
            else {
                $(".highcharts-root").css({ "width": "892" });
            }
            $(".overlayGR").attr("style", "display:block;height:0%transform: translateY(-0%);")
        }

        function ChartPlusForAR() {
            var str = $("#img2").attr('src');

            if (str.toLowerCase().indexOf('noticeunpin.png') >= 0) {
                $(".overlayAR").attr("style", "display:block;height:85%;transform: translatey(-0%);width:92.7%;margin-left:50px;margin-bottom:-253px")
                $(".highcharts-root").css({ "width": "1197" });
            }
            else {
                $(".overlayAR").attr("style", "display:block;height:85%;transform: translatey(-0%);width:90%;margin-left:50px;margin-bottom:-253px")
                $(".highcharts-root").css({ "width": "892" });
            }
            return false;
        }

        function TableMinusForAR() {
            var str = $("#img2").attr('src');

            if (str.toLowerCase().indexOf('noticeunpin.png') >= 0) {
                $(".highcharts-root").css({ "width": "1197" });
            }
            else {
                $(".highcharts-root").css({ "width": "892" });
            }
            $(".overlayAR").attr("style", "display:block;height:0%transform: translateY(-0%);")
        }

        //jQuery.noConflict();
        DD_roundies.addRule('.roundedHeader', '5px');
        var isAdd = "false";
        function openCity(evt, cityName) {
            var i, tabcontent, tablinks;
            tabcontent = document.getElementsByClassName("tabcontent");
            for (i = 0; i < tabcontent.length; i++) {
                tabcontent[i].style.display = "none";
                //tabcontent[0].style.display = "block";
            }
            tablinks = document.getElementsByClassName("tablinks");
            for (i = 0; i < tablinks.length; i++) {
                tablinks[i].className = tablinks[i].className.replace(" active", "");
            }
            document.getElementById(cityName).style.display = "block";
            evt.currentTarget.className += " active";

            if (cityName == "DiSoft") {
                var a = document.getElementsByClassName("tablinks")[0];
                if (!a.className.includes("active")) {
                    a.className += " active";
                }
                var acc = document.getElementsByClassName("panel-heading");
                var i;
                for (i = 0; i < acc.length; i++) {
                    if (isAdd == "false") {
                        acc[i].addEventListener("click", function () {                            
                            var IsOpenPanel = document.getElementsByClassName(this.className);
                            for (var j = 0; j < IsOpenPanel.length; j++) {
                                if (IsOpenPanel[j].className.includes("active")) {
                                    if (IsOpenPanel[j].innerText != this.innerText) {
                                        IsOpenPanel[j].className = IsOpenPanel[j].className.replace(" active", "");
                                        IsOpenPanel[j].nextElementSibling.style.display = "none"
                                    }
                                }
                            }
                            this.classList.toggle("active");
                            $(this).next('div').toggle(1000);
                            //$(this).next('div').css("width","100%")
                            TableMinus();
                            TableMinusForCA();
                            TableMinusForGR();
                            TableMinusForAR();
                            //$(this).parent().siblings().find('.panel-heading').slideUp();
                        });
                    }
                }
                isAdd = "true";
            }
            return false;
        }

        function $(id) {
            return document.getElementById(id);
        }

        function DivHideTemp() {
            document.getElementById('div1').style.display = "none";
            document.getElementById('div2').style.display = "none";
        }

        function DivHide() {
            document.getElementById('<%=Div1.ClientId %>').
            $('<%=Div1.ClientId %>').style.display = "none";
            return true;
        }

        function ValidationForProjectTrack() {
            var txt = document.getElementById('<%= TxtProjectNoPlainForTrackProjectStatus.ClientId %>');
            if (document.getElementById('<%= TxtProjectNoPlainForTrackProjectStatus.ClientId %>').style.display == 'none') {
                var txt = document.getElementById('<%= TxtProjectNoPlainForTrackProjectStatus.ClientId %>');
            }

            if (txt.value.trim() == '') {
                msgalert('Please Enter Project.');
                return false;
            }
            return true;
        }

        function validateall() {
            var dept = document.getElementById('<%=DdllistForDepartment.ClientId%>');
            var month = document.getElementById('<%=DdlListMonthForMyCalendar.ClientId%>');
            var year = document.getElementById('<%=DdlListYearForMyCalendar.ClientId%>');

            if (dept.options[dept.selectedIndex].value == 'Default' && month.options[month.selectedIndex].value != 'Select...' && year.options[year.selectedIndex].value != 'Select...') {
                msgalert("Please Select Department");
                return false;
            }
            else if (dept.options[dept.selectedIndex].value != 'Default' && month.options[month.selectedIndex].value == 'Select...' && year.options[year.selectedIndex].value != 'Select...') {
                msgalert("Please Select Month");
                return false;
            }
            else if (dept.options[dept.selectedIndex].value != 'Default' && month.options[month.selectedIndex].value != 'Select...' && year.options[year.selectedIndex].value == 'Select...') {
                msgalert("Please Select Year");
                return false;
            }
            else if (dept.options[dept.selectedIndex].value == 'Default' && month.options[month.selectedIndex].value == 'Select...' && year.options[year.selectedIndex].value != 'Select...') {
                msgalert("Please Select Department,Month");
                return false;
            }
            else if (dept.options[dept.selectedIndex].value == 'Default' && month.options[month.selectedIndex].value != 'Select...' && year.options[year.selectedIndex].value == 'Select...') {
                msgalert("Please Select Department,Year");
                return false;
            }
            else if (dept.options[dept.selectedIndex].value != 'Default' && month.options[month.selectedIndex].value == 'Select...' && year.options[year.selectedIndex].value == 'Select...') {
                msgalert("Please Select Month,Year");
                return false;
            }
            else if (dept.options[dept.selectedIndex].value == 'Default' && month.options[month.selectedIndex].value == 'Select...' && year.options[year.selectedIndex].value == 'Select...') {
                msgalert("Please Select Department,Month,Year");
                return false;
            }
        }

        function ShowActivity(ele, date, projectno) {
            var id = ele.id;
            var workspaceid = document.getElementById('<%=HdDFieldWorkSapceId.ClientId%>');
            workspaceid.value = id;
            var Date = document.getElementById('<%=HdFieldDate.ClientId%>');
            Date.value = date;
            var ProjectNo = document.getElementById('<%=HdFieldProjectNo.ClientId %>');
            ProjectNo.value = projectno;
            var btn = document.getElementById('<%=BtnGetActivityDetails.ClientId %>');
            btn.click();
        }

        function Call(ele) {
            jQuery(ele).animate({ 'height': '100px' });
        }

        function CallOut(ele) {
            jQuery(ele).animate({ 'height': '50px' });
        }

        function SetColorOfficeDaysHover(ele) {
            //jQuery(ele).css({ 'background': 'url(images/date_bg1.jpg) #dff3fc' });
            jQuery(ele).css({ 'background': 'url(images/date_bg.jpg) #c0e8f8' });
            jQuery(ele).css({ 'background-repeat': 'repeat-x' });
        }
        function SetColorOfficeDaysOut(ele) {
            //jQuery(ele).css({ 'background': 'url(images/date_bg.jpg) #c0e8f8' });
            jQuery(ele).css({ 'background': 'url(images/whitebg.jpg)' });
            jQuery(ele).css({ 'background-repeat': 'repeat-x' });
        }
        function SetColorHoliday(ele) {
            jQuery(ele).css({ 'background': 'url(images/holyday_bg.jpg) #ffda18' });
        }

        function noticeopen() {
            $(".Panelattachment").css("display", "");
            if ($(".Panelattachment ").css("left") == "-450px" || $(".Panelattachment ").css("left") == "-810px") {
                $(".Panelattachment").animate({ "left": "3%" }, "medium");
            }
            else {
                $(".Panelattachment").animate({ "left": "-450px" }, "medium");
            }
        }

        function noticepin() {
            var str = $("#img2").attr('src');
            if (str.toLowerCase().indexOf('noticeunpin.png') >= 0) {
                $("#img2").attr('src', 'images/NoticePin.png');
                $(".Contentth").css({ "left": "25.3%", "width": "72.5%" });
                $(".Content-Head").css({ "width": "100%" });
                $("#divgrid").width($(window).width() - 343);
                widthChart = "900"

                chartselection(<%=ddlChartforSubject.ClientID%>);
                chartselection_CA(<%=ddlChartforCA.ClientID%>);
                chartselection_GR(<%=ddlChartforGR.ClientID%>);
                chartselection_AR(<%=ddlChartforAR.ClientID%>);

                //$(".overlaymonth").attr("style", "display:block;height:85%;transform: translatey(-0%);width:90%;margin-left:50px;margin-bottom:-100px")
                //$(".overlayCA").attr("style", "display:block;height:85%;transform: translatey(-0%);width:90%;margin-left:50px;margin-bottom:-150px")
                //$(".overlayGR").attr("style", "display:block;height:85%;transform: translatey(-0%);width:90%;margin-left:50px;margin-bottom:-205px")
                //$(".overlayAR").attr("style", "display:block;height:85%;transform: translatey(-0%);width:90%;margin-left:50px;margin-bottom:-253px")
            }
            else {
                $(".Panel").css({ "left": "-450px" });
                $(".Contentth").css({ "left": "3%", "width": "95%" });
                $(".Content-Head").css({ "width": "100%" });
                $("#img2").attr('src', 'images/NoticeUnpin.png');
                $("#divgrid").width($(window).width() - 62);
                widthChart = "1200"

                chartselection(<%=ddlChartforSubject.ClientID%>);
                chartselection_CA(<%=ddlChartforCA.ClientID%>);
                chartselection_GR(<%=ddlChartforGR.ClientID%>);
                chartselection_AR(<%=ddlChartforAR.ClientID%>);

                //$(".overlaymonth").attr("style", "display:block;height:85%;transform: translatey(-0%);width:92.7%;margin-left:50px;margin-bottom:-100px")
                //$(".overlayCA").attr("style", "display:block;height:85%;transform: translatey(-0%);width:92.7%;margin-left:50px;margin-bottom:-150px")
                //$(".overlayGR").attr("style", "display:block;height:85%;transform: translatey(-0%);width:92.7%;margin-left:50px;margin-bottom:-205px")
                //$(".overlayAR").attr("style", "display:block;height:85%;transform: translatey(-0%);width:92.7%;margin-left:50px;margin-bottom:-253px")
            }
            TableMinus();
            TableMinusForCA();
            TableMinusForGR();
            TableMinusForAR();
            //$('.pin').stopPropagation();
            //$('.pin').preventDefault();
        }

        function ModalOpen(Divid) {
            if (Divid == "divMyCalendar") {
                document.getElementById('divDashboard').style.display = "block";
                document.getElementById('tdBack').style.display = "none";
            }
            else {
                var ParentDivID = $("#" + Divid + "").parent().attr("id");
                document.getElementById(ParentDivID).style.display = "block";
                document.getElementById('divDashboard').style.display = "none";
                document.getElementById('tdBack').style.display = "block";
            }

            $("#" + Divid + "maxmin").attr("style", "margin-top:2px;cursor:pointer;margin-right:5px;");
            var popupval = document.getElementById('<%= extendedpopup.ClientID %>').value;
            if (popupval == "plusbtn") {
                document.getElementById("" + Divid + "maxmin").src = "images/plus-icon.png";
                document.getElementById("" + Divid + "maxmin").alt = "plusbtn";
                $("#" + Divid + "").attr("style", "height:85%;width:65%;top:10%;left:17%;");
            }
            else {
                document.getElementById("" + Divid + "maxmin").src = "images/minus-icon.png";
                document.getElementById("" + Divid + "maxmin").alt = "minusbtn";
                $("#" + Divid + "").attr("style", "height:85%;width:98%;top:10%;left:0;z-index:10000;");
            }
            
            $("#" + Divid + "Header").attr("style", "cursor:move;");
            dragElement(document.getElementById(Divid));

            function dragElement(elmnt) {
                var pos1 = 0, pos2 = 0, pos3 = 0, pos4 = 0;
                if (document.getElementById(Divid + "Header")) {
                    document.getElementById(Divid + "Header").onmousedown = dragMouseDown;
                } else {
                    elmnt.onmousedown = dragMouseDown;
                }

                function dragMouseDown(e) {
                    e = e || window.event;
                    pos3 = e.clientX;
                    pos4 = e.clientY;
                    document.onmouseup = closeDragElement;
                    document.onmousemove = elementDrag;
                }

                function elementDrag(e) {
                    e = e || window.event;
                    pos1 = pos3 - e.clientX;
                    pos2 = pos4 - e.clientY;
                    pos3 = e.clientX;
                    pos4 = e.clientY;
                    elmnt.style.top = (elmnt.offsetTop - pos2) + "px";
                    elmnt.style.left = (elmnt.offsetLeft - pos1) + "px";
                }

                function closeDragElement() {
                    document.onmouseup = null;
                    document.onmousemove = null;
                }
            }
            
            //fnlDivid = DivID
            SiteWiseSubjectDetail = document.getElementById("divSiteWiseSubjectDetail").id;
            SiteInformation = document.getElementById('divSiteInformation').id;
            CRFStatus = document.getElementById("divCRFStatus").id;
            AESAE = document.getElementById("divAESAE").id;

            OperationKPI = document.getElementById("divOperationalKpi").id;
            //added by DhruviShah
            NewDemo = document.getElementById("divDemo").id;
            NewDemo1 = document.getElementById("divDemo1").id;
            DcfManagement = document.getElementById("divDcfmanage").id;
            //completed 

            if (OperationKPI == Divid) {
                $('<%=TxtFromDateOfOperationalKpi.ClientID%>').focus()
            }

            if (SiteWiseSubjectDetail == Divid || SiteInformation == Divid || CRFStatus == Divid || AESAE == Divid || NewDemo == Divid || NewDemo1 == Divid || DcfManagement == Divid) {
                if (SiteWiseSubjectDetail == Divid) {
                    document.getElementById('ctl00_CPHLAMBDA_dd_chart').disabled = true
                    $($get('<%= dd_chart.ClientID()%>')).val("Select Chart");
                }
                if (NewDemo == Divid) {
                    document.getElementById('ctl00_CPHLAMBDA_dd1_chart').disabled = true
                    $($get('<%= dd1_chart.ClientID()%>')).val("Select Chart");
                }
                if (NewDemo1 == Divid) {
                    document.getElementById('ctl00_CPHLAMBDA_dd2_chart').disabled = true
                    $($get('<%= dd2_chart.ClientID()%>')).val("Select Chart");
                }
                if (DcfManagement == Divid) {
                    document.getElementById('ctl00_CPHLAMBDA_dd3_chart').disabled = true
                    $($get('<%= dd3_chart.ClientID()%>')).val("Select Chart");
                }
                GetProjectNo(Divid);
            }

            var modal = document.getElementById(Divid);
            modal.style.display = "block";
        }

        function ModalClose() {
            $(".modal-content").attr("style", "display:none");
            document.getElementById("<%= extendedpopup.ClientID%>").value = "plusbtn";
        }

        function ShowDivBox(DIVID) {
            document.getElementById(DIVID).style.display = "block";
            document.getElementById('divDashboard').style.display = "none";
            document.getElementById('tdBack').style.display = "block";
        }

        function ShowDiSoftDivBox(DIVID) {
            debugger;
            document.getElementById(DIVID).style.display = "block";
            document.getElementById('divDisoftDashboard').style.display = "none";
            document.getElementById('tdBack').style.display = "block";
        }

        function DashboardMainBox() {
            document.getElementById('divDashboard').style.display = "block";
            document.getElementById('tdBack').style.display = "none";

            if ($('#divTracking').css('display') == 'block') {
                document.getElementById('divTracking').style.display = "none";
            }
            else if ($('#divAnalytics').css('display') == 'block') {
                document.getElementById('divAnalytics').style.display = "none";
            }

            else if ($('#divCTMS').css('display') == 'block') {
                document.getElementById('divCTMS').style.display = "none";
            }
        }

        function ModalExtend(Divid) {
            alt = $("#" + Divid + "maxmin").attr("alt");
            if (alt == "plusbtn") {
                document.getElementById("" + Divid + "maxmin").src = "images/minus-icon.png";
                document.getElementById("" + Divid + "maxmin").alt = "minusbtn";
                document.getElementById("<%= extendedpopup.ClientID %>").value = "minusbtn";
                $("#" + Divid + "").attr("style", "height:85%;width:98%;top:10%;left:0;z-index:10000;");
            }
            else {
                document.getElementById("" + Divid + "maxmin").src = "images/plus-icon.png";
                document.getElementById("" + Divid + "maxmin").alt = "plusbtn";
                document.getElementById("<%= extendedpopup.ClientID %>").value = "plusbtn";
                $("#" + Divid + "").attr("style", "height:85%;width:65%;top:10%;left:17%;");
            }
        }
    </script>

    <div>
        <asp:HiddenField runat="server" ID="extendedpopup" Value="plusbtn" />
        <table class="FormTable">
            <tbody>
                <tr>
                    <td style="width: 100%; text-align: left; vertical-align: top;">
                        <div valign="top" align="center" style="width:90%;">
                            <div style="width:100%">
                                <div>
                                    <div valign="top" id="tdTransit" runat="server" align="center" width="0%" style="border-right: solid 0px Silver;" visible="true">
                                        <div class="SliderMenu SliderMenuattach " style="left: auto; display: block; float: left; height: 16%;" onclick="noticeopen()">
                                            <div class="sideimg">                                                
                                                <span class="messageboard" >Message Board</span>
                                            </div>
                                        </div>
                                        <div class="Panelattachment Panel" style="left: -810px; width: 22%; height: 73.7% !important;overflow:auto;">
                                            <div cellpadding="0" cellspacing="0" style="width:100%";>
                                                <div>
									                <div class="Content-Head" style="width: 100%;">
                                                        <div style="text-align: right; width: 100%;">
                                                            <span class="pin">
                                                                <img id="img2" title="pin" height="20px" alt="Pin" src="Images/NoticePin.png" onclick="noticepin();" />
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
								            </div>
                                            <span>
									            <table id="divUpload">
										            <tbody>
											            <tr>
                                                            <td style="vertical-align: top; text-align: left; width:30%" >
                                                                <div align="right" class="" id="divScrollbar" runat="server">
                                                                    <div class="notice" id="divnotice">
                                                                        <asp:Panel ID="PnlPlaceAttach" runat="server">
                                                                            <asp:PlaceHolder ID="PlaceAttach" runat="server" EnableViewState="true"></asp:PlaceHolder>
                                                                        </asp:Panel>
                                                                    </div>
                                                                </div>
                                                            </td> 
											            </tr>
										            </tbody>
									            </table>
                                            </span>
                                        </div>
                                    </div>
                                    <div>
                                       <div class="Contentth" style="height: 70.5%!important; overflow: auto;">
                                           <div class="tab">
                                              <button class="tablinks" onclick="return openCity(event, 'DiSoft')">DiSoft</button>
                                              <button class="tablinks" onclick="return openCity(event, 'Biznet')" style="display:none;">Biznet</button>
                                            </div>
                                            <div id="DiSoft" class="tabcontent">
                                                <table class="FormTable">
                                                <tbody>
                                                    <tr>
                                                        <td style="text-align:center">
                                                            <label class="LabelText">Select Study* :</label>
                                                            <%--<asp:Label ID="lblProject" runat="server" Text=""></asp:Label>--%>
                                                            <asp:DropDownList ID="txtprojectForDI" runat="server" Style="width: 35%" CssClass="dropDownList" AutoPostBack="true" OnSelectedIndexChanged="txtprojectForDI_SelectedIndexChanged" TabIndex="1">                                                                
                                                            </asp:DropDownList>
                                                            <%--<asp:TextBox ID="txtprojectForDI" runat="server" CssClass="textBox" Width="50%" TabIndex="1"></asp:TextBox>--%>
                                                            <%--<asp:Button Style="display: none" ID="btnSetProject" runat="server" txt="data" />--%>
                                                            <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                                                            
                                                            <%--<cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                                                CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                                CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                                                OnClientShowing="ClientPopulated" ServiceMethod="GetMyProjectCompletionList"
                                                                ServicePath="AutoComplete.asmx" TargetControlID="txtprojectForDI" UseContextKey="True"
                                                                CompletionListElementID="pnlProjectListForDI">
                                                            </cc1:AutoCompleteExtender>--%>

                                                        <%--<asp:Panel ID="pnlProjectListForDI" runat="server" Style="max-height: 300px; overflow: auto; overflow-x: hidden;" />--%>
                                                            <asp:Button ID="btnClear" OnClick="btnClear_Click" TabIndex="15" runat="server" Text=" Clear "
                                                    CssClass="btn btncancel" ToolTip="Cancel" CausesValidation="False" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align:center">
                                                            <label class="LabelText">Select Site* :</label>
                                                            <asp:TextBox ID="txtSite" runat="server" CssClass="textBox" Width="40%"></asp:TextBox>
                                                            <asp:Button Style="display: none" ID="btnSetSite" runat="server" txt="data" />
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                                                CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                                CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelectedSite"
                                                                OnClientShowing="ClientPopulatedSite" ServiceMethod="GetMyProjectCompletionList"
                                                                ServicePath="AutoComplete.asmx" TargetControlID="txtSite" UseContextKey="True">
                                                            </cc1:AutoCompleteExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                   <td>
                                                       	<section>
			                                                <div class="container">
                                                                <div class="panel panel-primary" id="dvVisitReview" runat="server">
					                                                <div class="panel-heading">
						                                                <h3 class="panel-title">My Inbox</h3>
					                                                </div>
					                                                <div class="panel-body">
						                                                 <div class="col-12">  
                                                                              <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                                                  <ContentTemplate>
                                                                             <table id="Table1" cellpadding="0" cellspacing="0" style="width:100%"; >
										                                        <tbody>
                                                                                    <tr>
                                                                                    <td style="width: 20%; text-align: left; white-space: nowrap">
                                                                                        <strong class="Label">Date From *:</strong>
                                                                                        <asp:TextBox ID="txtVisitFromDate" Enabled="true" runat="server" CssClass="textBox" style="margin-left: 17px;" AutoComplete="off"></asp:TextBox>
                                                                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtVisitFromDate" Format="dd-MMM-yyyy" PopupButtonID="ImgFromDate"></cc1:CalendarExtender>
                                                                                    </td>
                                                                                    <td style="width: 20%; text-align: left">
                                                                                        <strong class="Label">Date To *: </strong>
                                                                                        <asp:TextBox ID="txtVisitToDate" Enabled="true" runat="server" CssClass="textBox" style="margin-left: 25px;" AutoComplete="off"></asp:TextBox>
                                                                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtVisitToDate" Format="dd-MMM-yyyy"></cc1:CalendarExtender>
                                                                                    </td> 
                                                                                  </tr>
											                                        <tr>
                                                                                        <td style="text-align: left; height:45px; width: 40%;">
                                                                                        <label class="LabelText">Screening No* :</label>
                                                                                        <asp:TextBox ID="txtScreeningForDI" runat="server" CssClass="textBox" Width="40%" TabIndex="1" AutoPostBack="false"></asp:TextBox>
                                                                                        <asp:Button Style="display: none" ID="btnScreeningNo" runat="server" txt="data" />
                                                                                        <asp:HiddenField ID="HScreeningNo" runat="server"></asp:HiddenField>
                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" BehaviorID="AutoCompleteExtender2"
                                                                                            CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                                                            CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelectedForVisit"
                                                                                            OnClientShowing="ClientPopulatedForVisit" ServiceMethod="GetScreeningNoCompletionlistUserWise"
                                                                                            ServicePath="AutoComplete.asmx" TargetControlID="txtScreeningForDI" UseContextKey="True"
                                                                                            CompletionListElementID="pnlScreeningListForDI">
                                                                                        </cc1:AutoCompleteExtender>
                                                                                            
                                                                                        <asp:Panel ID="pnlScreeningListForDI" runat="server" Style="max-height: 300px; overflow: auto; overflow-x: hidden;" />
                                                                                       </td>
                                                                                            <td id="td1" runat="server" class="Label" nowrap="nowrap" style="text-align: left;  width: 20%;">
                                                                                            <strong class="Label" style="display:inline-block;">Select Status *:</strong>
                                                                                            <asp:DropDownList ID="ddlVisitStatus" runat="server" AutoPostBack="false" Width="170px" >
                                                                                               <%-- <asp:ListItem Value="All">All</asp:ListItem>
                                                                                                <asp:ListItem Value="Pending" Selected="True">Pending</asp:ListItem>
                                                                                                <asp:ListItem Value="ImageUploader">Image Uploader</asp:ListItem>
                                                                                                <asp:ListItem Value="QC1">QC1</asp:ListItem>
                                                                                                <asp:ListItem Value="QC2">QC2</asp:ListItem>
                                                                                                <asp:ListItem Value="CA">CA1</asp:ListItem>--%>
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                         <td style="text-align: left;  width: 20%;" >
                                                                                             <asp:Button runat="server" ID="btnGoVisit" Text="Refresh" ToolTip="Refresh" CssClass="btn btngo" 
                                                                                                  OnClientClick="return ValidationForGo();" style=" margin-left: 164px;padding:0px;" AutoPostBack="false"/>
                                                                                             <asp:HiddenField ID="hdniUserId1" runat="server" Value="" />
                                                                                             <asp:HiddenField ID="hdnAdjUserId1" runat="server" Value="" />
                                                                                             <asp:HiddenField ID="hdnUserTypeCode1" runat="server" Value="" />
                                                                                                            <asp:HiddenField ID="hdnWorkFlowStageId1" runat="server" Value="" />
                                                                                                            <asp:HiddenField ID="DISoftURL1" runat="server" Value="" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    </tbody>
                                                                                 </table>
                                                                                <div class="datatable_filedetailVisit" style="width: 100%; height: auto; overflow: auto;">
                                                                                    <asp:GridView ID="gvVisitReviewStatus" runat="server"  Style="display: none; width: 100%; margin: auto;" AutoGenerateColumns="false"  
                                                                                        DataKeyNames="vWorkspaceId,vActivityId,iNodeId,iPeriod,vSubjectId,iMySubjectNo,ScreeningNo,QCUserCode,GraderUserCode,iImgTransmittalDtlId,CAUserCode,QC2UserCode"
                                                                                        OnRowCommand ="gvVisitReviewStatus_RowCommand" onrowdatabound="gvVisitReviewStatus_RowDataBound">
                                                                                    <%--<RowStyle Wrap="False"/>--%>
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField ="vSubjectId" HeaderText ="Subject Id" Visible="false" />
                                                                                        <asp:TemplateField  HeaderText ="Download Image">
                                                                                            <ItemTemplate >
                                                                                                <asp:ImageButton ID="btnImageType" runat="server" ImageUrl="~/images/imgimport.png" Text='DCM' Font-Underline="true" ToolTip='DCM' CommandName='DCM' 
                                                                                                    CommandArgument="<%# Container.DataItemIndex %>" ></asp:ImageButton> 
                                                                                                <%--<asp:LinkButton ID="btnQC" runat="server"  Text="QC" Font-Underline="true" ToolTip="QC" CommandName="QC" CommandArgument="<%# Container.DataItemIndex %>"></asp:LinkButton>--%>&nbsp;
                                                                                                <asp:ImageButton  ID="btnQC" runat="server" ImageUrl="~/images/QC.png" Text="QC1" Font-Underline="true" ToolTip="QC1" WIDTH="25PX" CommandName="QC1" CommandArgument="<%# Container.DataItemIndex %>"></asp:ImageButton>&nbsp;
                                                                                                <asp:ImageButton  ID="btnQC2" runat="server" ImageUrl="~/images/QC.png" Text="QC2" Font-Underline="true" ToolTip="QC2" WIDTH="25PX" CommandName="QC2" CommandArgument="<%# Container.DataItemIndex %>"></asp:ImageButton>&nbsp;
                                                                                                <asp:ImageButton  ID="btnGrader" runat="server" ImageUrl="~/images/QC.png" Text="CA" Font-Underline="true" ToolTip="CA" WIDTH="25PX" CommandName="CA1" CommandArgument="<%# Container.DataItemIndex %>"></asp:ImageButton>
                                                                                                <%--<asp:LinkButton ID="btnGrader" runat="server" Text="CA" Font-Underline="true" ToolTip="CA" CommandName="CA1" CommandArgument="<%# Container.DataItemIndex %>"></asp:LinkButton>--%>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:BoundField DataField="SiteNo" HeaderText="SiteNo" ></asp:BoundField>
                                                                                        <asp:BoundField DataField="PatientInitial" HeaderText="PatientInitial" />
                                                                                        <asp:BoundField DataField="ScreeningNo" HeaderText="ScreeningNo" />
                                                                                        <asp:BoundField DataField="RandomizationNo" HeaderText="RandomizationNo"></asp:BoundField>
                                                                                        <asp:BoundField DataField="Instruction" HeaderText="Instruction to CRC"></asp:BoundField>
                                                                                        <asp:BoundField DataField="Visit" HeaderText="Visit" />
                                                                                        <asp:BoundField DataField="VisitDate" HeaderText="VisitDate" />
                                                                                        <asp:BoundField DataField="Status" HeaderText="Status" />
                                                                                        <asp:BoundField DataField="ChangeOn" HeaderText="Change On" />
                                                                                        
                                                                                        <asp:TemplateField HeaderText ="View R1">
                                                                                            <ItemTemplate>
                                                                                                <asp:LinkButton ID="btnR1" style="display:block;" runat="server" Text="View R1" Font-Underline="true" ToolTip="View R1" CommandName="ViewR1" CommandArgument="<%# Container.DataItemIndex %>"></asp:LinkButton>
                                                                                            </ItemTemplate>
                                                                                             <ItemStyle HorizontalAlign="Center" />
                                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField  HeaderText ="View R2"  >
                                                                                            <ItemTemplate>
                                                                                                <asp:LinkButton ID="btnR2" style="display:block;" runat="server" Text="View R2" Font-Underline="true" ToolTip="View R2" CommandName="ViewR2"  CommandArgument="<%# Container.DataItemIndex %>"></asp:LinkButton>
                                                                                            </ItemTemplate>
                                                                                             <ItemStyle HorizontalAlign="Center" />
                                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                        </asp:TemplateField>

                                                                                        <asp:BoundField DataField="vWorkspaceId" HeaderText="WorkspaceId" Visible="false" />   
                                                                                        <asp:BoundField DataField="vSubjectId" HeaderText="SubjectId" Visible="false" />   
                                                                                        <asp:BoundField DataField="vActivityId" HeaderText="ActivityId" Visible="false" />   
                                                                                        <asp:BoundField DataField="iNodeId" HeaderText="NodeId" Visible="false" />   
                                                                                        <asp:BoundField DataField="iPeriod" HeaderText="Period" Visible="false" />   
                                                                                        <asp:BoundField DataField="iMySubjectNo" HeaderText="MySubjectNo" Visible="false" /> 
                                                                                        <asp:BoundField DataField="QCUserCode" HeaderText="QC1" Visible="false" />   
                                                                                        <asp:BoundField DataField="GraderUserCode" HeaderText="Grader" Visible="false" /> 
                                                                                        <asp:BoundField DataField="CAUserCode" HeaderText="CA" Visible="false" />
                                                                                        <asp:BoundField DataField="QC2UserCode" HeaderText="QC2" Visible="false" />
                                                                                        <asp:BoundField DataField="iImgTransmittalDtlId" HeaderText="iImgTransmittalDtlId" Visible="false" />
                                                                                        <asp:BoundField DataField="iImgTransmittalHdrId" HeaderText= "ImgTransmittalHdrId" Visible="false" />
                                                                                       <%-- <asp:BoundField DataField="QCUserId" HeaderText="QC" Visible="false" /> 
                                                                                        <asp:BoundField DataField="GraderUserId" HeaderText="Grader" Visible="false" /> --%>
                                                                                    </Columns>
                                                                                <HeaderStyle Wrap="False" />
                                                                                </asp:GridView>
                                                                                    <table id="emptyTable" class="table" runat="server" visible="false" style="width:100%">
                                                                                         <tr>
                                                                                             <td colspan="9" style="padding: 5px 10px;">
                                                                                                 No Records Found!
                                                                                             </td>
                                                                                         </tr>
                                                                                     </table>
                                                                                </div>
                                                                                      </ContentTemplate>
                                                                                  <Triggers>
                                                                                    <asp:AsyncPostBackTrigger ControlID="btnGoVisit" EventName="Click"  />
                                                                                    <%--<asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />--%>
                                                                                </Triggers>
                                                                                  </asp:UpdatePanel>
						                                               </div>
					                                                </div>
				                                                </div>
                                                          
                                                               <!--MIDesiger dashoard -->
                                                                <div class="panel panel-primary"  id="dvAdjucatorReview"  runat="server">
					                                                <div class="panel-heading">
						                                                <h3 class="panel-title">Site Wise Adjudicator Review Status</h3>
					                                                </div>
					                                                <div class="panel-body">
						                                                 <div class="col-12">  
                                                                              <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                                                  <ContentTemplate>
                                                                             <table id="Table1" cellpadding="0" cellspacing="0" style="width:100%"; >
										                                        <tbody>
											                                        <tr>
                                                                                            <td id="td2" runat="server" class="Label" nowrap="nowrap" style="text-align: left;  width: 20%;">
                                                                                            <strong class="Label" style="display:inline-block;">Select Status *:</strong>
                                                                                            <asp:DropDownList ID="ddlAdjudicator" runat="server" AutoPostBack="false" Width="170px" >
                                                                                                <asp:ListItem Value="All">All</asp:ListItem>                                                                                                
                                                                                                <asp:ListItem Value="Pending">Pending</asp:ListItem>
                                                                                                <asp:ListItem Value="Completed">Completed</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                         <td style="text-align: left;  width: 20%;" >
                                                                                             <asp:Button runat="server" ID="btnGoAdjucator" Text="Refresh" ToolTip="Refresh" CssClass="btn btngo" 
                                                                                                  style=" margin-left: 164px;padding:0px;" AutoPostBack="false"/>
                                                                                             <asp:HiddenField ID="hdniUserId" runat="server" Value="" />
                                                                                             <asp:HiddenField ID="hdnAdjUserId" runat="server" Value="" />
                                                                                             <asp:HiddenField ID="hdnUserTypeCode" runat="server" Value="" />
                                                                                                            <asp:HiddenField ID="hdnWorkFlowStageId" runat="server" Value="" />
                                                                                                            <asp:HiddenField ID="DISoftURL" runat="server" Value="" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    </tbody>
                                                                                 </table>
                                                                                <div class="datatable_filedetailAdjucator" style="width: 100%; height: auto; overflow: auto;margin-top:5px;">
                                                                                    <asp:GridView ID="gvAdjucatorReviewStatus" runat="server" Style="display: none; width: 100%; margin: auto;" AutoGenerateColumns="false"  
                                                                                        OnRowCommand ="gvAdjucatorReviewStatus_RowCommand"
                                                                                        DataKeyNames="vWorkspaceId, vParentWorkSpaceId, vSubjectId, iMySubjectNo,iPeriod,R1iImgTransmittalHdrId, R2iImgTransmittalHdrId, R1iImgTransmittalDtlId, R2iImgTransmittalDtlId, 
                                                                                                          R1vParentActivityId, R2vParentActivityId, R1iParentNodeId, R2iParentNodeId, R1vActivityId, R2vActivityId, R1iNodeId, R2iNodeId, R1vPeriodId, R2vPeriodId, 
                                                                                                          R2vActivityName, R1vSubActivityName, R2vSubActivityName, R1cRadiologist, R2cRadiologist, 
                                                                                                          R1vMedExDesc, R2vMedExDesc"
                                                                                        >
                                                                                        <%--DataKeyNames="vWorkspaceId, vParentWorkSpaceId,vSubjectId, R1iImgTransmittalHdrId, R2iImgTransmittalHdrId, R1iImgTransmittalDtlId, R2iImgTransmittalDtlId, 
                                                                                                          R1vParentActivityId, R2vParentActivityId, R1iParentNodeId, R2iParentNodeId, R1vActivityId, R2vActivityId, R1iNodeId, R2iNodeId, R1vPeriodId, R2vPeriodId, 
                                                                                                          R2vActivityName, R1vSubActivityName, R2vSubActivityName, R1cRadiologist, R2cRadiologist, R1vAnnotationType, R2vAnnotationType, 
                                                                                                          R1vMedExDesc, R2vMedExDesc"--%>
                                                                                        
                                                                                    <RowStyle Wrap="False"/>
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="SiteNo" HeaderText="SiteNo" ></asp:BoundField>
                                                                                        <asp:BoundField DataField="ScreeningNo" HeaderText="ScreeningNo" />
                                                                                        <asp:BoundField DataField="ActivityName" HeaderText="Activity"></asp:BoundField>
                                                                                        <asp:BoundField DataField="R1vActivityName" HeaderText="Sub Activity"></asp:BoundField>     
                                                                                        <asp:BoundField DataField="R1vMedExResult" HeaderText="R1-Overall Response" />                                                  
                                                                                        <asp:BoundField DataField="R2vMedExResult" HeaderText="R2-Overall Response" />
                                                                                        <asp:BoundField DataField="Status" HeaderText="Status" />

                                                                                        <asp:TemplateField HeaderText ="">
                                                                                            <ItemTemplate>
                                                                                                <asp:LinkButton ID="btnR1" style="display:block;" runat="server" Text="R1" Font-Underline="true" ToolTip="R1" CommandName="R1" CommandArgument="<%# Container.DataItemIndex %>" ></asp:LinkButton>
                                                                                            </ItemTemplate>
                                                                                             <ItemStyle HorizontalAlign="Center" />
                                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText ="">
                                                                                            <ItemTemplate>
                                                                                                <asp:LinkButton ID="btnR2" style="display:block;" runat="server" Text="R2" Font-Underline="true" ToolTip="R2" CommandName="R2" CommandArgument="<%# Container.DataItemIndex %>" ></asp:LinkButton>
                                                                                            </ItemTemplate>
                                                                                             <ItemStyle HorizontalAlign="Center" />
                                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField  HeaderText =""  >
                                                                                            <ItemTemplate>
                                                                                                <asp:LinkButton ID="btnR1Adj" style="display:block;" runat="server" Text="View" Font-Underline="true" ToolTip="View" CommandName="AdjView" CommandArgument="<%# Container.DataItemIndex %>" ></asp:LinkButton>
                                                                                            </ItemTemplate>
                                                                                             <ItemStyle HorizontalAlign="Center" />
                                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                        </asp:TemplateField>
                                                                                        <%--<asp:BoundField DataField="vWorkspaceId" HeaderText="WorkspaceId" Visible="false" />   
                                                                                        <asp:BoundField DataField="vSubjectId" HeaderText="SubjectId" Visible="false" />   
                                                                                        <asp:BoundField DataField="vActivityId" HeaderText="ActivityId" Visible="false" />   
                                                                                        <asp:BoundField DataField="iNodeId" HeaderText="NodeId" Visible="false" />   
                                                                                        <asp:BoundField DataField="iPeriod" HeaderText="Period" Visible="false" />   
                                                                                        <asp:BoundField DataField="iMySubjectNo" HeaderText="MySubjectNo" Visible="false" /> 
                                                                                        <asp:BoundField DataField="QCUserCode" HeaderText="QC1" Visible="false" />   
                                                                                        <asp:BoundField DataField="GraderUserCode" HeaderText="Grader" Visible="false" /> 
                                                                                        <asp:BoundField DataField="CAUserCode" HeaderText="CA" Visible="false" />
                                                                                        <asp:BoundField DataField="QC2UserCode" HeaderText="QC2" Visible="false" />
                                                                                        <asp:BoundField DataField="iImgTransmittalDtlId" HeaderText="iImgTransmittalDtlId" Visible="false" />
                                                                                        <asp:BoundField DataField="iImgTransmittalHdrId" HeaderText= "ImgTransmittalHdrId" Visible="false" />--%>
                                                                                       <%-- <asp:BoundField DataField="QCUserId" HeaderText="QC" Visible="false" /> 
                                                                                        <asp:BoundField DataField="GraderUserId" HeaderText="Grader" Visible="false" /> --%>
                                                                                    </Columns>
                                                                                <HeaderStyle Wrap="False" />
                                                                                </asp:GridView>
                                                                                    <table id="Table2" class="table" runat="server" visible="false" style="width:100%">
                                                                                         <tr>
                                                                                             <td colspan="9" style="padding: 5px 10px;">
                                                                                                 No Records Found!
                                                                                             </td>
                                                                                         </tr>
                                                                                     </table>
                                                                                    <table id="table11" cellpadding="0" cellspacing="0" style="width:100%;" >
										                                            <tbody>
											                                        <tr>                                                                                            
                                                                                         <td style="text-align: right; padding-right: 20px; width: 20%;" >
                                                                                             <asp:Button runat="server" ID="btnSaveActivity" Text="Alloacation" ToolTip="Go" CssClass="btn btnAllocation" 
                                                                                                  style="padding:0px; display: none;" AutoPostBack="false"/>                                                                                             
                                                                                        </td>
                                                                                    </tr>
                                                                                    </tbody>
                                                                                 </table>
                                                                                </div>
                                                                                      </ContentTemplate>
                                                                                  <Triggers>
                                                                                    <asp:AsyncPostBackTrigger ControlID="btnGoAdjucator" EventName="Click"  />
                                                                                    <asp:AsyncPostBackTrigger ControlID="btnSaveActivity" EventName="Click" />
                                                                                </Triggers>
                                                                                  </asp:UpdatePanel>
						                                               </div>
					                                                </div>
				                                                </div>

                                                                <!--CA dashboard -->
                                                                <%--<div class="panel panel-primary" id="dvCaseAssign" runat="server">
                                                                    <div class="panel-heading">
						                                                <h3 class="panel-title">Site Wise Case Assignment</h3>
					                                                </div>
                                                                    <div class="panel-body">
                                                                        <div class="col-12">
                                                                            <asp:UpdatePanel ID="UpdPnlCA" runat="server">
                                                                                <ContentTemplate>
                                                                                    <table id="tblCA" cellpadding="0" cellspacing="0" style="width:100%"; >
										                                                <tbody>
											                                                <tr>
                                                                                                <td>
                                                                                                    <label class="LabelText">Project Name* :</label>
                                                                                                    <asp:Label ID="Label32" runat="server" Text=""></asp:Label>
                                                                                                    <asp:TextBox ID="txtprojectForCA" runat="server" CssClass="textBox" Width="50%" TabIndex="3" AutoPostBack="false"></asp:TextBox>
                                                                                                    <asp:Button Style="display: none" ID="btnSetProjectForCA" runat="server" txt="data" />
                                                                                                    <asp:HiddenField ID="HdnProjectId" runat="server"></asp:HiddenField>
                                                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" BehaviorID="AutoCompleteExtender3"
                                                                                                        CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                                                                        CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelectedProject"
                                                                                                        OnClientShowing="ClientPopulatedProject" ServiceMethod="GetMyProjectCompletionList"
                                                                                                        ServicePath="AutoComplete.asmx" TargetControlID="txtprojectForCA" UseContextKey="True"
                                                                                                        CompletionListElementID="pnlProjectListForCA">
                                                                                                    </cc1:AutoCompleteExtender>
                                                                                                    <asp:Panel ID="pnlProjectListForCA" runat="server" Style="max-height: 300px; overflow: auto; overflow-x: hidden;" />
                                                                                                </td>
                                                                                                 <td style="text-align: left; width: 10%;">
                                                                                                     <asp:Button runat="server" ID="btnGoCA" Text="Go" ToolTip="Go" CssClass="btn btngo" style="padding:0px;" AutoPostBack="false"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                 </table>
                                                                                    <div class="datatable_filedetailCA" style="width:100%; height:auto; overflow:auto; margin-top:5px;">
                                                                                        <asp:GridView ID="gvCA" runat="server" Style="display: none; width: 100%; margin: auto;" OnRowCommand ="gvCA_RowCommand" AutoGenerateColumns="false" DataKeyNames="vWorkspaceId, vSubjectId">
                                                                                            <RowStyle Wrap="False"/>
                                                                                            <Columns>
                                                                                                <asp:BoundField DataField="SiteNo" HeaderText="SiteNo" ></asp:BoundField>
                                                                                                <asp:BoundField DataField="PatientInitial" HeaderText="PatientInitial" />
                                                                                                <asp:BoundField DataField="ScreeningNo" HeaderText="ScreeningNo" />
                                                                                                <asp:TemplateField HeaderText="Action">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:ImageButton ID="btnCA" runat="server" ImageUrl="~/images/QC.png" Text="CA" Font-Underline="true" ToolTip="CA" WIDTH="25PX" CommandName="CA1" CommandArgument="<%# Container.DataItemIndex %>"></asp:ImageButton>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:BoundField DataField="vWorkspaceId" HeaderText="WorkspaceId" Visible="false" />   
                                                                                                <asp:BoundField DataField="vSubjectId" HeaderText="SubjectId" Visible="false" />
                                                                                            </Columns>
                                                                                            <HeaderStyle Wrap="False" />
                                                                                        </asp:GridView>
                                                                                        <table id="CAEmptytbl" runat="server" visible="false">
                                                                                         <tr>
                                                                                             <td colspan="9">
                                                                                                 No Records Found!
                                                                                             </td>
                                                                                         </tr>
                                                                                     </table>
                                                                                    </div>
                                                                                </ContentTemplate>
                                                                                <Triggers>
                                                                                    <asp:AsyncPostBackTrigger ControlID="btnGoCA" EventName="Click"  />
                                                                                </Triggers>
                                                                            </asp:UpdatePanel>
                                                                        </div>
                                                                    </div>
                                                                </div>--%>
                                                </div>

				                                                <div class="panel panel-primary" style="display:none;">
					                                                <%--<div class="panel-heading"  onclick="chartselection(this)" id="ctl00_CPHLAMBDA_ddlChartforSubject">--%>
                                                                    <div class="panel-heading  active">
						                                                <h3 class="panel-title">Site Wise Subject Information</h3>
					                                                </div>
					                                                <div class="panel-body" style="display:block;">
						                                               <div class="col-12">
                                                                           <div>
                                                                               <button type="button" class="btn btn-success btn-sm pull-right" style="margin-right: 20px;" id="btnSiteWiseChart" onclick="return ChartPlus();">
                                                                               <i class="fa fa-plus"></i>
                                                                            </button>
                                                                           </div>
                                                                           <br />
                                                                           <br />
                                                                           <div>
                                                                               <div class="form-group  floating-labels">
                                                                                          <asp:Label ID="lblChart" runat="server" Text=" Select Chart" Style="color: darkgoldenrod"></asp:Label>
                                                                                           <asp:DropDownList ID="ddlChartforSubject" runat="server" onchange="chartselection(this);" Style="color: purple" CssClass="dropDownList" AutoPostBack="false" Chart="subject">
                                                                                                            <asp:ListItem Value="">Select Chart</asp:ListItem>
                                                                                                            <asp:ListItem Value="Pie" Selected="True">Bar Chart</asp:ListItem>
                                                                                                            <asp:ListItem Value="Line">Line Chart</asp:ListItem>
                                                                                                            <asp:ListItem Value="Doughnut">Doughnut Chart</asp:ListItem>
                                                                                                        </asp:DropDownList>
                                                                                </div>
                                                                           </div>
                                                                           <div id="divSiteWiseSubjectInformation" style="position: relative; height: 350px;"></div>
                   
                                                                                <div class="overlaymonth" style="width:88%">
                                                                                    <div class="pull-right" style="margin-right:25px">
                                                                                        <button type="button" class="btn btn-success btn-sm pull-right" id="btnSiteWisetable" onclick="return TableMinus();">
                                                                                            <i class="fa fa-minus"></i>
                                                                                        </button>
                                                                                    </div>
                                                                                    <div class="box-body" style="margin-top:48px;">
                                                                                        <table id="tblSiteWiseSubjectInformation" class="table table-bordered table-striped">
                                                                                             <thead>
                                                                                                 <tr>
                                                                                                     <th>Site</th>
                                                                                                     <th>Total</th>
                                                                                                     <th>Rejected Subject</th>
                                                                                                     <th>Enrolled Subject</th>
                                                                                                 </tr>
                                                                                             </thead>
                                                                                               <tbody>
                                                                                               </tbody>
                                                                                        </table>
                                                                                    </div>
                                                                                </div>
						                                               </div>
					                                                </div>
				                                                </div>

                                                                <div class="panel panel-primary" style="display:none;">
					                                                <div class="panel-heading">
						                                                <h3 class="panel-title">Site Wise Case Assignment</h3>
					                                                </div>
					                                                <div class="panel-body" style="display:none;">
						                                                 <div class="col-12">
                                                                           <div>
                                                                               <button type="button" class="btn btn-success btn-sm pull-right" style="margin-right: 15px;" id="Button2"  onclick="return ChartPlusForCA();">
                                                                               <i class="fa fa-plus"></i>
                                                                            </button>
                                                                           </div>
                                                                           <br />
                                                                           <br />
                                                                           <div>
                                                                               <div class="form-group  floating-labels">
                                                                                   <asp:Label ID="Label30" runat="server" Text=" Select Activity" Style="color: darkgoldenrod"></asp:Label>
                                                                                    <asp:DropDownList ID="ddlActivityForCA" runat="server" Style="color: purple" class="dropDownList" AutoPostBack="false" onchange="Activityselection_CA(this);">
                                                                                                        </asp:DropDownList>
                                                                                          <asp:Label ID="Label22" runat="server" Text=" Select Chart" Style="color: darkgoldenrod"></asp:Label>
                                                                                           <asp:DropDownList ID="ddlChartforCA" runat="server" onchange="chartselection_CA(this);" Style="color: purple" class="dropDownList" AutoPostBack="false">
                                                                                                            <asp:ListItem Value="">Select Chart</asp:ListItem>
                                                                                                            <asp:ListItem Value="Pie" Selected="True">Bar Chart</asp:ListItem>
                                                                                                            <asp:ListItem Value="Line">Line Chart</asp:ListItem>
                                                                                                            <asp:ListItem Value="Doughnut">Doughnut Chart</asp:ListItem>
                                                                                                        </asp:DropDownList>
                                                                                  
                                                                                </div>
                                                                           </div>
                                                                           <div id="divSiteWiseCAAssignment" style="position: relative; height: 350px;"></div>
                   
                                                                                <div class="overlayCA"  style="width:88%">
                                                                                    <div class="pull-right" style="margin-right:20px">
                                                                                        <button type="button" class="btn btn-success btn-sm pull-right" id="btnSiteWiseCA" onclick="return TableMinusForCA();">
                                                                                            <i class="fa fa-minus"></i>
                                                                                        </button>
                                                                                    </div>
                                                                                    <div class="box-body" style="margin-top:48px;">
                                                                                        <table id="tblSiteWiseCAAssignment" class="table table-bordered table-striped">
                                                                                             <thead>
                                                                                                 <tr>
                                                                                                     <th>Site</th>
                                                                                                    <th>Total Subject</th>
                                                                                                     <th>CA Subject</th>
                                                                                                     <%--<th>Enrolled Subject</th>--%>
                                                                                                 </tr>
                                                                                             </thead>
                                                                                               <tbody>
                                                                                               </tbody>
                                                                                        </table>
                                                                                    </div>
                                                                                </div>
						                                               </div>
					                                                </div>
				                                                </div>

                                                                <div class="panel panel-primary" style="display:none;">
					                                                <div class="panel-heading">
						                                                <h3 class="panel-title">Site Wise Global Response</h3>
					                                                </div>
					                                                <div class="panel-body" style="display:none;">
						                                                 <div class="col-12">
                                                                           <div>
                                                                               <button type="button" class="btn btn-success btn-sm pull-right" style="margin-right: 15px;" id="Button3"  onclick="return ChartPlusForGR();">
                                                                               <i class="fa fa-plus"></i>
                                                                            </button>
                                                                           </div>
                                                                           <br />
                                                                           <br />
                                                                           <div>
                                                                               <div class="form-group  floating-labels">
                                                                                 <%--   <asp:Label ID="Label31" runat="server" Text=" Select Activity" Style="color: darkgoldenrod"></asp:Label>
                                                                                    <asp:DropDownList ID="ddlActivityForGR" runat="server" Style="color: purple" class="dropDownList" AutoPostBack="false" onchange="Activityselection_GR(this);">
                                                                                                        </asp:DropDownList>--%>
                                                                                          <asp:Label ID="Label28" runat="server" Text=" Select Chart" Style="color: darkgoldenrod"></asp:Label>
                                                                                           <asp:DropDownList ID="ddlChartforGR" runat="server" onchange="chartselection_GR(this);" Style="color: purple" class="dropDownList" AutoPostBack="false">
                                                                                                           <asp:ListItem Value="">Select Chart</asp:ListItem>
                                                                                                            <asp:ListItem Value="Pie" Selected="True">Bar Chart</asp:ListItem>
                                                                                                            <asp:ListItem Value="Line">Line Chart</asp:ListItem>
                                                                                                            <asp:ListItem Value="Doughnut">Doughnut Chart</asp:ListItem>
                                                                                                        </asp:DropDownList>
                                                                                </div>
                                                                           </div>
                                                                           <div id="divSiteWiseGlobalResponse" style="position: relative; height: 350px;"></div>
                   
                                                                                <div class="overlayGR"  style="width:88%">
                                                                                    <div class="pull-right" style="margin-right:20px">
                                                                                        <button type="button" class="btn btn-success btn-sm pull-right" id="btnSiteWiseGR" onclick="return TableMinusForGR();">
                                                                                            <i class="fa fa-minus"></i>
                                                                                        </button>
                                                                                    </div>
                                                                                    <div class="box-body" style="margin-top:48px;">
                                                                                        <table id="tblSiteWiseGlobalResponse" class="table table-bordered table-striped">
                                                                                             <thead>
                                                                                                 <tr>
                                                                                                     <th>Site</th>
                                                                                                     <th>Total Subject</th>
                                                                                                     <th>Subject</th>
                                                                                                    <%-- <th>Rejected Subject</th>
                                                                                                     <th>Enrolled Subject</th>--%>
                                                                                                 </tr>
                                                                                             </thead>
                                                                                               <tbody>
                                                                                               </tbody>
                                                                                        </table>
                                                                                    </div>
                                                                                </div>
						                                               </div>
					                                                </div>
				                                                </div>

                                                                <div class="panel panel-primary" style="display:none;">
					                                                <div class="panel-heading">
						                                                <h3 class="panel-title">Site Wise Adjudicator Response</h3>
					                                                </div>
					                                                <div class="panel-body" style="display:none;">
						                                                 <div class="col-12">
                                                                           <div>
                                                                               <button type="button" class="btn btn-success btn-sm pull-right" style="margin-right: 15px;" id="Button5"  onclick="return ChartPlusForAR();">
                                                                               <i class="fa fa-plus"></i>
                                                                            </button>
                                                                           </div>
                                                                           <br />
                                                                           <br />
                                                                           <div>
                                                                               <div class="form-group  floating-labels">
                                                                                          <asp:Label ID="Label29" runat="server" Text=" Select Chart" Style="color: darkgoldenrod"></asp:Label>
                                                                                           <asp:DropDownList ID="ddlChartforAR" runat="server" onchange="chartselection_AR(this);" Style="color: purple" class="dropDownList" AutoPostBack="false">
                                                                                                            <asp:ListItem Value="">Select Chart</asp:ListItem>
                                                                                                            <asp:ListItem Value="Pie" Selected="True">Bar Chart</asp:ListItem>
                                                                                                            <asp:ListItem Value="Line">Line Chart</asp:ListItem>
                                                                                                            <asp:ListItem Value="Doughnut">Doughnut Chart</asp:ListItem>
                                                                                                        </asp:DropDownList>
                                                                                </div>
                                                                           </div>
                                                                           <div id="divSiteWiseAdjudicatorResponse" style="position: relative; height: 350px;"></div>
                   
                                                                                <div class="overlayAR"  style="width:88%">
                                                                                    <div class="pull-right" style="margin-right:20px">
                                                                                        <button type="button" class="btn btn-success btn-sm pull-right" id="Button6" onclick="return TableMinusForAR();">
                                                                                            <i class="fa fa-minus"></i>
                                                                                        </button>
                                                                                    </div>
                                                                                    <div class="box-body" style="margin-top:48px;">
                                                                                        <table id="tblSiteWiseAdjudicatorResponse" class="table table-bordered table-striped">
                                                                                             <thead>
                                                                                                 <tr>
                                                                                                     <th>Site</th>
                                                                                                     <th>Total Subject</th>
                                                                                                     <th>Subject</th>
                                                                                                    <%-- <th>Rejected Subject</th>
                                                                                                     <th>Enrolled Subject</th>--%>
                                                                                                 </tr>
                                                                                             </thead>
                                                                                               <tbody>
                                                                                               </tbody>
                                                                                        </table>
                                                                                    </div>
                                                                                </div>
						                                               </div>
					                                                </div>
				                                                </div>
		                                                </section>
                                                   </td>
                                                        </tr>
                                                </tbody>
                                                </table>
        </div>
                                              <%--  <table class="FormTable">
                                                    <tbody>
                                                        <tr>
                                                            <td>
                                                                <div id="divSiteWiseSubjectInformation">
                                                                 </div>
                                                                <table id="SiteWiseSubjectInformation" class="display" cellspacing="0" width="100%" hidden>
                                                                    <thead>
                                                                        <tr>
                                                                            <td>Site</td>
                                                                            <td>WorkSpaceId</td>
                                                                            <td>Total Subject</td>
                                                                            <td>Rejected Subject</td>
                                                                        </tr>
                                                                    </thead>
                                                                    <tbody></tbody>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                 </table>--%>

                                            <div id="Biznet" class="tabcontent">
                                              <table class="FormTable">
                                                <tbody>
                                                    <tr>
                                                         <td style="width: 80%; vertical-align: top; padding: 0px 10px 0px 10px;" >     
                                                             <asp:UpdatePanel ID="UpdatePanel9" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                        <table style="width: 100%;padding-right:30px">
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td style="display:none" id="tdBack">
                                                                                        <i class="fa fa-arrow-circle-left" aria-hidden="true" style="font-size:35px" onclick="DashboardMainBox();" title="Back To Dashboard"></i>
                                                                                    </td>
                                                                                    <td colspan="2" style="text-align: right;">
                                                                                        <span class="Label">Project Manager:</span>
                                                                                        <asp:DropDownList ID="ddlProjectManager" AutoPostBack="true" runat="server">
                                                                                        </asp:DropDownList>
                                                                                        <span class="Label">Select Location :</span>
                                                                                        <asp:DropDownList ID="ddlLocation" AutoPostBack="true" runat="server">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                        <br />
                                                                        <div style="width: 100%;margin-left:2.5%;">
                                                                            <div id="divDashboard" style="display:block;">
                                                                                <div class="col-lg-6" style="display:block">
                                                                                    <div class="small-box bg-aquaDashboard" style="background-color:#f9a54b !important">
                                                                                        <div class="inner" style="padding:25px !important">
                                                                                            <h3 style="margin-right:100%"> </h3>
                                                                                            <p style="margin-right:10%">TRACKING</p>
                                                                                        </div>
                                                                                        <div class="icon">
                                                                                            <i class="fa fa-list-alt"></i>
                                                                                        </div>
                                                                                        <a href="#" class="small-box-footer" onclick="ShowDivBox('divTracking')">More info <i class="fa fa-arrow-circle-down"></i></a>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="col-lg-6" style="display:block">
                                                                                    <div class="small-box bg-aquaDashboard " style="background-color:#4be58f !important">
                                                                                        <div class="inner" style="padding:25px !important">
                                                                                            <h3 style="margin-right:100%"> </h3>
                                                                                            <p style="margin-right:10%">ANALYTICS</p>
                                                                                        </div>
                                                                                        <div class="icon">
                                                                                            <i class="fa fa-area-chart"></i>
                                                                                        </div>
                                                                                        <a href="#" class="small-box-footer" onclick="ShowDivBox('divAnalytics')">More info <i class="fa fa-arrow-circle-down"></i></a>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="col-lg-6" style="display:block">
                                                                                    <div class="small-box bg-aquaDashboard" style="background-color:#2cc0f9 !important">
                                                                                        <div class="inner" style="padding:25px !important">
                                                                                            <h3 style="margin-right:100%"> </h3>
                                                                                            <p style="margin-right:10%">CTMS</p>
                                                                                        </div>
                                                                                        <div class="icon">
                                                                                            <i class="fa fa-bar-chart"></i>
                                                                                        </div>
                                                                                        <a href="#" class="small-box-footer" onclick="ShowDivBox('divCTMS')">More info <i class="fa fa-arrow-circle-down"></i></a>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="col-lg-6" runat="server" style="display:block" id="TrMyCalendar">
                                                                                    <div class="small-box bg-aquaDashboard" style="background-color:#ff8c60 !important">
                                                                                        <div class="inner" style="padding:25px !important">
                                                                                            <h3 style="margin-right:100%"></h3>
                                                                                            <p style="margin-right:10%">CALENDAR</p>
                                                                                        </div>
                                                                                        <div class="icon">
                                                                                            <i class="fa fa-calendar"></i>
                                                                                        </div>
                                                                                        <a href="#" class="small-box-footer" >More info <i class="fa fa-arrow-circle-down"></i></a>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="modal-content modal-lg" id="divMyCalendar" style="display:none;">
                                                                                    <div class="modal-header" id="divMyCalendarHeader">
                                                                                        <img src="images/Sqclose.gif" class="ModalImage" onclick="ModalClose();" alt="" />
                                                                                        <img src="images/plus-icon.png" class="ModalImage" id="divMyCalendarmaxmin" height="28px" width="28px" alt="plusbtn" onclick="ModalExtend('divMyCalendar');" style="cursor:pointer;margin-right:5px;"/>
                                                                                        <h2 style="text-align:center;">My Calender</h2>
                                                                                    </div>
                                                                                    <div id="Div1" class="modal-body modalheight" runat="server" style="overflow:initial;">
                                                                                        <table style="text-align: left; width: 90%; margin: auto;">
                                                                                            <tr style="text-align: left">
                                                                                                <td>Department :
                                                                                                    <asp:DropDownList ID="DdllistForDepartment" runat="server" Width="100" CssClass="dropDownListSearch" AutoPostBack="true" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    Activity :
                                                                                                    <asp:DropDownList ID="DdllistActivityName" runat="server" Width="100" CssClass="dropDownListSearch" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    Month :
                                                                                                    <asp:DropDownList ID="DdlListMonthForMyCalendar" runat="server" Width="100" TabIndex="1" CssClass="dropDownList" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    Year :
                                                                                                    <asp:DropDownList ID="DdlListYearForMyCalendar" runat="server" Width="100" TabIndex="2" CssClass="dropDownList" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Button ID="BtnGOForCalendar" Text="" runat="server" CssClass="btn btngo" Tooltip ="Go"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="5">
                                                                                                    <hr />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>

                                                                                        <table style="text-align: left; width: 80%; margin: auto;">
                                                                                            <tr>
                                                                                                <td style="text-align: right; vertical-align: top">
                                                                                                    <asp:Panel runat="server" ID="PnlLegendsForMyCalendar" Visible="false">
                                                                                                        <table>
                                                                                                            <tr style="text-align: left">
                                                                                                                <td><b>Legends : </b></td>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="LblRed" Text="" Width="15" Height="8" BackColor="Red" runat="server" />
                                                                                                                </td>
                                                                                                                <td>Delayed</td>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="LblBlue" Text="" Width="15" Height="8" BackColor="blue" runat="server" />
                                                                                                                </td>
                                                                                                                <td>On Schedule</td>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="LblGreen" Text="" Width="15" Height="8" BackColor="Green" runat="server" />
                                                                                                                </td>
                                                                                                                <td>Scheduled</td>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="LblBrown" Text="" Width="15" Height="8" BackColor="Brown" runat="server" />
                                                                                                                </td>
                                                                                                                <td>Actual Not Entered</td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </asp:Panel>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td style="text-align: left; padding-bottom: 10px;">
                                                                                                    <asp:Calendar ID="CldDatePicker" runat="server" NextPrevFormat="FullMonth" ForeColor="Black"
                                                                                                        Visible="false" Font-Size="12pt" Font-Names="Calibri" CellSpacing="1" BackColor="navy" Width="100%"
                                                                                                        TabIndex="3" Height="250" CellPadding="10">
                                                                                                        <SelectedDayStyle BackColor="#FFCC99" />
                                                                                                        <TodayDayStyle BackColor="Gray" />
                                                                                                        <DayStyle BackColor="#CCCCCC" Height="80px" Width="80px" HorizontalAlign="Left" VerticalAlign="Top" CssClass="CalendarDay" />
                                                                                                        <OtherMonthDayStyle ForeColor="#999999" />
                                                                                                        <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="White" />
                                                                                                        <%--<DayHeaderStyle Font-Bold="True" Font-Size="8pt" ForeColor="Navy" Height="8pt" CssClass="SetDayOfMyCalendar" />--%>
                                                                                                        <DayHeaderStyle Font-Bold="True" Font-Size="12pt" Height="15pt" BackColor="White" CssClass=""/>
                                                                                                        <TitleStyle BackColor="#56a0da" Font-Bold="True" Font-Size="10pt" ForeColor="White"
                                                                                                            Height="10pt" CssClass="SetHeaderOfMyCalendarAs" />
                                                                                                    </asp:Calendar>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:HiddenField ID="HdDFieldWorkSapceId" runat="server" />
                                                                                                    <asp:HiddenField ID="HdFieldDate" runat="server" />
                                                                                                    <asp:HiddenField ID="HdFieldProjectNo" runat="server" />
                                                                                                    <asp:DropDownList ID="DdlProjectList" runat="server" Style="display: none;">
                                                                                                    </asp:DropDownList>
                                                                                                    <asp:Button ID="BtnGetActivityDetails" runat="server" Text="btn btnnew" Style="display: none;" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>

                                                                                        <button id="Btn3" runat="server" style="display: none;" />
                                                                                        <cc1:ModalPopupExtender ID="Mpedialog3" runat="server" PopupControlID="DivPopUp"
                                                                                            PopupDragHandleControlID="LblPopUpTitleWorkSummary" BackgroundCssClass=""
                                                                                            TargetControlID="btn3" CancelControlID="ImgPopUp">
                                                                                        </cc1:ModalPopupExtender>                                                                                        
                                                                                    </div>
                                                                                    <%--<div class="modal-footer">
                                                                                        <h3></h3>
                                                                                    </div>--%>
                                                                                </div>

                                                                                <div class="modal-content modal-lg" id="DivPopUp" style="display:none;" runat="server">
                                                                                    <div class="modal-header">
                                                                                        <img id="ImgPopUp" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px;" />
                                                                                        <h2 style="text-align:center;"><asp:Label ID="LblPopUpTitle" runat="server" class="LabelBold" Visible="true" /></h2>
                                                                                    </div>
                                                                                    <div class="modal-body modalheight">
                                                                                        <table style="border: 0; width: 98%">
                                                                                            <tr>
                                                                                                <td class="Label" style="text-align: right; width: 100%; text-align: center; vertical-align: middle;">
                                                                                                    <div id="canal" style="font: verdana; text-align: left;">
                                                                                                        <table>
                                                                                                            <tr>
                                                                                                                <td style="text-align: left" colspan="3">Legends :
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td style="width: 20%; vertical-align: top;">
                                                                                                                    <img src="~/images/themeRed.png" runat="server" id="imgRed" alt="themeRed" />- Delayed
                                                                                                                </td>
                                                                                                                <td style="width: 20%; vertical-align: top;">
                                                                                                                    <img src="images/themeBlue.png" runat="server" id="imgBlue" alt="themeBlue" />-On
                                                                                                                    Schedule
                                                                                                                </td>
                                                                                                                <td style="width: 20%; vertical-align: top;">
                                                                                                                    <img src="images/themeGreen.png" runat="server" id="imgGreen" alt="themeGreen" />-Scheduled
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div runat="server" id="divGvwPnlPnlActivityDetails" style="width: 95%; margin: auto; height: auto; max-height: 356px;">
                                                                                                        <asp:GridView runat="server" ID="GvwPnlPnlActivityDetails" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true">
                                                                                                            <Columns>
                                                                                                                <asp:BoundField HeaderText="Sr. No" />
                                                                                                                <asp:BoundField DataField="vNodeDisplayName" HeaderText="Activity Name" ItemStyle-HorizontalAlign="Left" />
                                                                                                                <asp:BoundField DataField="SchStart" HeaderText="Schedule Start Date" ItemStyle-HorizontalAlign="Center" />
                                                                                                                <asp:BoundField DataField="ActStart" HeaderText="Actual Start Date" ItemStyle-HorizontalAlign="Center" />
                                                                                                                <asp:BoundField DataField="SchEnd" HeaderText="Schedule End Date" ItemStyle-HorizontalAlign="Center" />
                                                                                                                <asp:BoundField DataField="ActEnd" HeaderText="Actual End Date" ItemStyle-HorizontalAlign="Center" />
                                                                                                                <asp:BoundField DataField="vClientName" HeaderText="Sponsor" ItemStyle-HorizontalAlign="Center" />
                                                                                                                <asp:BoundField DataField="vDrugName" HeaderText="Drug" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="true" />
                                                                                                                <asp:TemplateField HeaderText="Indicator">
                                                                                                                    <ItemTemplate>
                                                                                                                        <center>
                                                                                                                            <asp:Image ID="ImgIndicator" runat="server" />
                                                                                                                        </center>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                            </Columns>
                                                                                                        </asp:GridView>
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td style="text-align: center; vertical-align: top;" colspan="2">
                                                                                                    <asp:Button ID="Button4" runat="server" Text="Update" CssClass="btn btnsave" Style="display: none;" Tooltip="Update"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>  
                                                                                    </div>
                                                                                    <div class="modal-footer">
                                                                                        <h3> </h3>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <div id="divTracking" style="display:none">
                                                                                <%--Track Project Status--%>
                                                                                <div class="col-lg-3" id="TrProjectTrack" runat="server" style="display:none">
                                                                                    <div class="small-box bg-aquaDashboard">
                                                                                        <div class="inner">
                                                                                            <h3 style="margin-right:100%">
                                                                                                <asp:Label ID="Label1" runat="server" Style="border-radius: 10%; padding-left: 2px; padding-right: 2px; margin-left: 0.5%;" />
                                                                                            </h3>
                                                                                            <p style="margin-right:10%;">Track project status</p>
                                                                                        </div>
                                                                                        <div class="icon">
                                                                                            <i class="fa fa-shopping-bag"></i>
                                                                                        </div>
                                                                                        <a href="#" class="small-box-footer" id="a1">More info <i class="fa fa-arrow-circle-right"></i></a>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="modal-content modal-lg" id="divProjectTrack" style="display:none;">
                                                                                    <div class="modal-header" id="divProjectTrackHeader">
                                                                                        <img src="images/Sqclose.gif" class="ModalImage" onclick="ModalClose();" alt="" />
                                                                                        <img src="images/plus-icon.png" class="ModalImage" id="divProjectTrackmaxmin" height="28px" width="28px" alt="plusbtn" onclick="ModalExtend('divProjectTrack');" style="cursor:pointer;margin-right:5px;"/>
                                                                                        <h2 style="text-align:center;">Track Project Status</h2>
                                                                                    </div>
                                                                                    <div class="modal-body">
                                                                                        <table style="text-align: left; width: 100%;">
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td style="text-align: right; width: 15%;">Project No :
                                                                                                    </td>
                                                                                                    <td style="text-align: left; width: 70%">
                                                                                                        <asp:TextBox ID="TxtProjectNoPlainForTrackProjectStatus" runat="server" Text="" Style="width: 100%;"
                                                                                                            Visible="true" CssClass="textBox" />
                                                                                                        <asp:Button Style="display: none" ID="btnSetProjectforTracking" runat="server" CssClass="btn btngo" />
                                                                                                        <asp:HiddenField ID="HProjectIdForTracking" runat="server" />
                                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtenderForTracking" runat="server" BehaviorID="AutoCompleteExtenderForTracking"
                                                                                                            CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                                                                            CompletionListElementID="pnlProjectList" CompletionListItemCssClass="autocomplete_listitem"
                                                                                                            MinimumPrefixLength="1" OnClientItemSelected="OnSelectedForTracking" OnClientShowing="ClientPopulatedForTracking"
                                                                                                            ServiceMethod="GetMyProjectCompletionListForDashboard" ServicePath="AutoComplete.asmx"
                                                                                                            TargetControlID="TxtProjectNoPlainForTrackProjectStatus" UseContextKey="true">
                                                                                                        </cc1:AutoCompleteExtender>
                                                                                                        <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 300px; overflow: auto; overflow-x: hidden;" />
                                                                                                    </td>
                                                                                                    <td style="text-align: left; padding-left: 07px;" >
                                                                                                        <asp:Button ID="btnProjectforTrackingGo" OnClientClick="return ValidationForProjectTrack();"
                                                                                                        OnClick="btnProjectforTrackingGo_Click"  runat="server" class="btn btngo" Tooltip="Go" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div class="modal-footer">
                                                                                        <h3> </h3>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="col-lg-3" id="TrClientRequest" runat="server" style="display:none;">
                                                                                    <div class="small-box bg-aquaDashboard">
                                                                                        <div class="inner">
                                                                                            <h3 id="H2" style="margin-right:100%">
                                                                                                <asp:Label ID="LblClientRequestProjects" runat="server" Style="border-radius: 10%; padding-right: 2px; padding-left: 2px; margin-left: 0.5%;color:white" />
                                                                                            </h3>
                                                                                            <p id="p2" style="margin-right:10%">Projects In Client Request</p>
                                                                                        </div>
                                                                                        <div class="icon">
                                                                                            <i class="fa fa-shopping-bag"></i>
                                                                                        </div>
                                                                                        <a href="#" class="small-box-footer" id="a3" >More info <i class="fa fa-arrow-circle-right"></i></a>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="modal-content modal-lg" id="divClientRequest" style="display:none;">
                                                                                    <div class="modal-header" id="divClientRequestHeader">
                                                                                        <img src="images/Sqclose.gif" class="ModalImage" onclick="ModalClose();" alt="" />
                                                                                        <img src="images/plus-icon.png" class="ModalImage" id="divClientRequestmaxmin" height="28px" width="28px" alt="plusbtn" onclick="ModalExtend('divClientRequest');" style="cursor:pointer;margin-right:5px;"/>
                                                                                        <h2 style="text-align:center;">Projects In Client Request</h2>
                                                                                    </div>
                                                                                    <div class="modal-body modalheight">
                                                                                        <table style="text-align: left; width: 100%;">
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td style="width: 15%; text-align: right;">Request Id :
                                                                                                    </td>
                                                                                                    <td style="text-align: left;">
                                                                                                        <asp:TextBox ID="TxtRequestIdClientRequest" runat="server" Style="width: 80%;" CssClass="textBox" />
                                                                                                        <asp:Button Style="display: none" ID="btnSetProjectforclientrequest" OnClick="btnSetProjectforclientrequest_Click"
                                                                                                            runat="server" Text=" Project" />
                                                                                                        <asp:HiddenField ID="HProjectIdForClientRequest" runat="server" />
                                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtenderForClientRequest" runat="server"
                                                                                                            BehaviorID="AutoCompleteExtenderForClientRequest" CompletionListCssClass="autocomplete_list"
                                                                                                            CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListItemCssClass="autocomplete_listitem"
                                                                                                            MinimumPrefixLength="1" OnClientItemSelected="OnSelectedForClientRequest" OnClientShowing="ClientPopulatedForClientRequest"
                                                                                                            ServiceMethod="GetClientRequestProjectCompletionListForDashboard" ServicePath="AutoComplete.asmx"
                                                                                                            TargetControlID="TxtRequestIdClientRequest" UseContextKey="true" CompletionListElementID="pnlClientProject">
                                                                                                        </cc1:AutoCompleteExtender>
                                                                                                        <asp:Panel ID="pnlClientProject" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="text-align: right;">Sponsor Name :
                                                                                                    </td>
                                                                                                    <td style="text-align: left;">
                                                                                                        <asp:DropDownList ID="DdlClientnameforClientRequest" runat="server" AutoPostBack="true" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>
                                                                                        <asp:UpdatePanel ID="upPnl0001" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                                                            <ContentTemplate>
                                                                                                <asp:Panel ID="pnl0001" runat="server" BorderWidth="0px" Style="width: 98%; margin: auto;" ScrollBars="none">
                                                                                                    <asp:GridView ID="gvwpnl0001" runat="server" AutoGenerateColumns="False"
                                                                                                        Style="width: 100%" ShowHeaderWhenEmpty="true" CssClass="ConvertToDataTable" SkinID="grdViewForDashboard">
                                                                                                        <Columns>
                                                                                                            <asp:TemplateField HeaderText="Details">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:LinkButton ID="lnkBtnDetails" runat="server" Text="Details" />
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:BoundField DataField="vWorkSpaceId" HeaderText="WorkSpaceId" />
                                                                                                            <asp:BoundField DataField="vRequestId" HeaderText="Req Id" />
                                                                                                            <asp:BoundField DataField="vDrugName" HeaderText="Drug" />
                                                                                                            <asp:BoundField DataField="vClientName" HeaderText="Sponsor" />
                                                                                                            <asp:BoundField DataField="vRegionName" HeaderText="Submission" />
                                                                                                            <asp:BoundField DataField="dCreatedOn" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Date of Request" HtmlEncode="False" />
                                                                                                            <asp:BoundField DataField="vProjectManager" HeaderText="Project Manager" />
                                                                                                        </Columns>
                                                                                                        <PagerStyle Height="10px" />
                                                                                                    </asp:GridView>
                                                                                                </asp:Panel>    
                                                                                            </ContentTemplate>
                                                                                            <Triggers>
                                                                                                <asp:PostBackTrigger ControlID="BtnExportToExcelForClientRequest" />
                                                                                                <asp:AsyncPostBackTrigger  ControlID="DdlClientnameforClientRequest" EventName="SelectedIndexChanged" />
                                                                                                <asp:AsyncPostBackTrigger  ControlID="btnSetProjectforclientrequest" EventName="Click" />
                                                                                            </Triggers>
                                                                                        </asp:UpdatePanel>
                                                                                    </div>
                                                                                    <div class="modal-footer">
                                                                                        <asp:Button ID="BtnExportToExcelForClientRequest" OnClick="BtnExportToExcelForClientRequest_Click" runat="server" Text="" CssClass="btn btnexcel" />
                                                                                    </div>
                                                                                </div>

                                                                                 <%--'* 2nd pane started [projects in Pre-Clinical phase] *'--%>
                                                                                <div class="col-lg-3" id="TrPreClinical" runat="server" style="display:none">
                                                                                    <div class="small-box bg-aquaDashboard">
                                                                                        <div class="inner">
                                                                                            <h3 style="margin-right:100%;">
                                                                                                <asp:Label ID="LblProjectPreClinical" runat="server" Style="border-radius: 10%; 
                                                                                                        padding-right: 2px; padding-left: 2px; margin-left: 0.5%;" class="LabelColor" />
                                                                                            </h3>
                                                                                            <p id="p3" style="margin-right:10%">Projects In Pre-Study</p>
                                                                                        </div>
                                                                                        <div class="icon">
                                                                                            <i class="fa fa-shopping-bag"></i>
                                                                                        </div>
                                                                                        <a href="#" class="small-box-footer" id="a4">More info <i class="fa fa-arrow-circle-right"></i></a>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="modal-content modal-lg" id="divProjectPreClinical" style="display:none;">
                                                                                    <div class="modal-header" id="divProjectPreClinicalHeader">
                                                                                        <img src="images/Sqclose.gif" class="ModalImage" onclick="ModalClose();" alt="" />
                                                                                        <img src="images/plus-icon.png" class="ModalImage" id="divProjectPreClinicalmaxmin" height="28px" width="28px" alt="plusbtn" onclick="ModalExtend('divProjectPreClinical');" style="cursor:pointer;margin-right:5px;"/>
                                                                                        <h2 style="text-align:center;">Projects In Pre-Study</h2>
                                                                                    </div>
                                                                                    <div class="modal-body modalheight">
                                                                                        <table style="text-align: left; width: 100%;">
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td style="text-align: right; width: 15%;">Request Id :</td>
                                                                                                    <td style="text-align: left;">
                                                                                                        <asp:TextBox ID="TxtProjectRequestIdProjectPreClinical" runat="server" Style="width: 80%;" CssClass="textBox" />
                                                                                                        <asp:Button Style="display: none" ID="btnSetProjectProjectPreClinical" runat="server" Text=" Project" OnClick="btnSetProjectProjectPreClinical_Click" />
                                                                                                        <asp:HiddenField ID="HFRequestIdProjectPreClinical" runat="server" />
                                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtenderForProjectPreClinical" runat="server"
                                                                                                            BehaviorID="AutoCompleteExtenderForProjectPreClinical" CompletionListCssClass="autocomplete_list"
                                                                                                            CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListItemCssClass="autocomplete_listitem"
                                                                                                            MinimumPrefixLength="1" OnClientItemSelected="OnSelectedForProjectPreClinical"
                                                                                                            OnClientShowing="ClientPopulatedForProjectPreClinical" ServiceMethod="GetClientRequestProjectCompletionListForDashboard"
                                                                                                            ServicePath="AutoComplete.asmx" TargetControlID="TxtProjectRequestIdProjectPreClinical"
                                                                                                            UseContextKey="true" CompletionListElementID="pnlRequestList">
                                                                                                        </cc1:AutoCompleteExtender>
                                                                                                        <asp:Panel ID="pnlRequestList" runat="server" Style="max-height: 300px; overflow: auto; overflow-x: hidden;" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="text-align: right; width: 15%;">Sponsor Name :</td>
                                                                                                    <td style="text-align: left;">
                                                                                                        <asp:DropDownList ID="DdlClientnameforProjectPreClinical" runat="server" AutoPostBack="true" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>
                                                                                        <asp:UpdatePanel ID="upPnl0013" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                                                            <ContentTemplate>
                                                                                                <asp:Panel ID="pnl0013" runat="server" Width="650px" BorderWidth="0px" Style="width: 98%; margin: auto;" ScrollBars="none">
                                                                                                    <asp:GridView ID="gvwpnl0013" runat="server" Style="width: 100%"
                                                                                                        AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" SkinID="grdViewForDashboard">
                                                                                                        <Columns>
                                                                                                            <asp:TemplateField HeaderText="Details">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:LinkButton ID="lnkBtnDetailsProjectPreClinical" runat="server" Text="Details" />
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:BoundField DataField="vWorkSpaceId" HeaderText="WorkSpaceId" />
                                                                                                            <asp:BoundField DataField="vRequestId" HeaderText="Req Id" />
                                                                                                            <asp:BoundField DataField="vDrugName" HeaderText="Drug" />
                                                                                                            <asp:BoundField DataField="vClientName" HeaderText="Sponsor" />
                                                                                                            <asp:BoundField DataField="vRegionName" HeaderText="Submission" />
                                                                                                            <asp:BoundField DataField="dCreatedOn" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Date of Request" HtmlEncode="False" />
                                                                                                            <asp:BoundField DataField="vProjectManager" HeaderText="Project Manager" />
                                                                                                        </Columns>
                                                                                                        <PagerStyle Height="10px" />
                                                                                                    </asp:GridView>
                                                                                                </asp:Panel>
                                                                                            </ContentTemplate>
                                                                                            <Triggers>
                                                                                                <asp:PostBackTrigger ControlID="BtnExportToExcelForProjectPreClinical" />
                                                                                                <asp:AsyncPostBackTrigger  ControlID="DdlClientnameforProjectPreClinical" EventName="SelectedIndexChanged" />
                                                                                                <asp:AsyncPostBackTrigger  ControlID="btnSetProjectProjectPreClinical" EventName="Click" />
                                                                                            </Triggers>
                                                                                        </asp:UpdatePanel>
                                                                                    </div>
                                                                                    <div class="modal-footer">
                                                                                        <asp:Button ID="BtnExportToExcelForProjectPreClinical" runat="server" Text="" CssClass="btn btnexcel" />
                                                                                    </div>
                                                                                </div>

                                                                                <%--'* 3rd pane started [Projects in Clinical Phase] *'--%>
                                                                                <div class="col-lg-3" id="TrClinicalPhase" runat="server" style="display:none">
                                                                                    <div class="small-box bg-aquaDashboard">
                                                                                        <div class="inner">
                                                                                            <h3 style="margin-right:100%">
                                                                                                <asp:Label ID="LblProjectsClinincalPhase" runat="server" 
                                                                                                        Style="border-radius: 10%; padding-right: 2px; padding-left: 2px; 
                                                                                                        margin-left: 0.5%;" class="LabelColor"/>
                                                                                            </h3>
                                                                                            <p style="margin-right:10%">Projects In Clinical Phase </p>
                                                                                            <%--<br />(In Progress)--%>
                                                                                        </div>
                                                                                        <div class="icon">
                                                                                            <i class="fa fa-list-alt"></i>
                                                                                        </div>
                                                                                        <a href="#" class="small-box-footer" id="a5">More info <i class="fa fa-arrow-circle-right"></i></a>
                                                                                    </div>
                                                                        </div>

                                                                                <div class="modal-content modal-lg" id="divClinicalPhase" style="display:none;">
                                                                                    <div class="modal-header" id="divClinicalPhaseHeader">
                                                                                        <img src="images/Sqclose.gif" class="ModalImage" onclick="ModalClose();" alt="" />
                                                                                        <img src="images/plus-icon.png" class="ModalImage" id="divClinicalPhasemaxmin" height="28px" width="28px" alt="plusbtn" onclick="ModalExtend('divClinicalPhase');" style="cursor:pointer;margin-right:5px;"/>
                                                                                        <h2 style="text-align:center;">Projects In Clinical Phase(In Progress)</h2>
                                                                                    </div>
                                                                                    <div class="modal-body modalheight">
                                                                                        <table style="text-align: left; width: 100%">
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td style="width: 15%; text-align: right;">Project No :</td>
                                                                                                    <td style="text-align: left;">
                                                                                                        <asp:TextBox ID="TxtProjectForClinicalPhase" runat="server" CssClass="textBox" Style="width: 80%;" />
                                                                                                        <asp:Button Style="display: none" ID="btnSetProjectForClinicalPhase" OnClick="btnSetProjectForClinicalPhase_Click"
                                                                                                            runat="server" Text="Project" />
                                                                                                        <asp:HiddenField ID="HProjectIdForClinicalPhase" runat="server" />
                                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtenderForClinicalPhase" runat="server"
                                                                                                            BehaviorID="AutoCompleteExtenderForClinicalPhase" CompletionListCssClass="autocomplete_list"
                                                                                                            CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListItemCssClass="autocomplete_listitem"
                                                                                                            MinimumPrefixLength="1" OnClientItemSelected="OnSelectedForClinicalPhase" OnClientShowing="ClientPopulatedForClinicalPhase"
                                                                                                            ServiceMethod="GetMyProjectCompletionListForDashboard" ServicePath="AutoComplete.asmx"
                                                                                                            TargetControlID="TxtProjectForClinicalPhase" UseContextKey="True" CompletionListElementID="pnlProjectListClinicalPhs">
                                                                                                        </cc1:AutoCompleteExtender>
                                                                                                        <asp:Panel ID="pnlProjectListClinicalPhs" runat="server" Style="max-height: 300px; overflow: auto; overflow-x: hidden;" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="text-align: right;">Sponsor Name :</td>
                                                                                                    <td style="text-align: left;">
                                                                                                        <asp:DropDownList ID="DdlCLientNameForCliniclaphase" runat="server" AutoPostBack="true"
                                                                                                            OnSelectedIndexChanged="DdlCLientNameForCliniclaphase_SelectedIndexChanged" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>
                                                                                        <asp:UpdatePanel ID="upPnl0003" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                                                            <ContentTemplate>
                                                                                                <asp:Panel ID="pnl0003" runat="server" Style="width: 98%; margin: auto;" BorderWidth="0px" ScrollBars="none">
                                                                                                    <asp:GridView ID="gvwpnl0003" runat="server" AutoGenerateColumns="False"
                                                                                                        Style="width: 100%" ShowHeaderWhenEmpty="true" SkinID="grdViewForDashboard">
                                                                                                        <Columns>
                                                                                                            <asp:TemplateField HeaderText="Details">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:LinkButton ID="lnkBtnDetailsForClinicalPhase" runat="server" Text="Details" />
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:BoundField DataField="vworkSpaceId" HeaderText="WorkSpaceId" />
                                                                                                            <asp:BoundField DataField="vProjectNo" HeaderText="Project No." />
                                                                                                            <asp:BoundField DataField="vDrugName" HeaderText="Drug Name" />
                                                                                                            <asp:BoundField DataField="vClientName" HeaderText="Sponsor" />
                                                                                                            <asp:BoundField DataField="iNoOfPeriods" HeaderText="No Of Periods" />
                                                                                                            <asp:BoundField DataField="iNoOfSubjects" HeaderText="No Of Subjects" />
                                                                                                            <asp:BoundField DataField="cFastingFed" HeaderText="Fast/Fed" />
                                                                                                            <asp:BoundField DataField="iNoOfTimePoints" HeaderText="Sample Time Points" />
                                                                                                        </Columns>
                                                                                                    </asp:GridView>
                                                                                                </asp:Panel>
                                                                                            </ContentTemplate>
                                                                                            <Triggers>
                                                                                                <asp:PostBackTrigger ControlID="BtnExportToExcelForClinicalPhase" />
                                                                                                <asp:AsyncPostBackTrigger  ControlID="DdlCLientNameForCliniclaphase" EventName="SelectedIndexChanged" />
                                                                                                <asp:AsyncPostBackTrigger  ControlID="btnSetProjectForClinicalPhase" EventName="Click" />
                                                                                            </Triggers>
                                                                                        </asp:UpdatePanel>
                                                                                    </div>
                                                                                    <div class="modal-footer">
                                                                                        <asp:Button ID="BtnExportToExcelForClinicalPhase" OnClick="BtnExportToExcelForClinicalPhase_Click" runat="server" Text="" CssClass="btn btnexcel" />
                                                                                    </div>    
                                                                                </div>

                                                                                <%--'* 4th pane started [Projects in Analytical Phase] *'--%>
                                                                                <div class="col-lg-3" id="TrAnalytical" runat="server" style="display:none">
                                                                                    <div class="small-box bg-aquaDashboard">
                                                                                        <div class="inner">
                                                                                            <h3 style="margin-right:100%">
                                                                                                <asp:Label ID="LblAnalyticalPhase" runat="server" Style=" border-radius: 10%; padding-left: 2px; 
                                                                                                    padding-right: 2px; margin-left: 0.5%;" class="LabelColor"/>
                                                                                            </h3>
                                                                                            <p style="margin-right:10%">Projects In Analytical Phase </p>
                                                                                            <%--<br /> (In Progress)--%>
                                                                                        </div>
                                                                                        <div class="icon">
                                                                                            <i class="fa fa-list-alt"></i>
                                                                                        </div>
                                                                                        <a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i></a>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="modal-content modal-lg" id="divAnalyticalphase" style="display:none;">
                                                                                    <div class="modal-header" id="divAnalyticalphaseHeader">
                                                                                        <img src="images/Sqclose.gif" class="ModalImage" onclick="ModalClose();" alt="" />
                                                                                        <img src="images/plus-icon.png" class="ModalImage" id="divAnalyticalphasemaxmin" height="28px" width="28px" alt="plusbtn" onclick="ModalExtend('divAnalyticalphase');" style="cursor:pointer;margin-right:5px;"/>
                                                                                        <h2 style="text-align:center;">Projects In Analytical Phase (In Progress)</h2>
                                                                                    </div>
                                                                                    <div class="modal-body modalheight">
                                                                                        <table style="text-align: left; width: 100%;">
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td style="text-align: right; width: 15%">Project No :</td>
                                                                                                        <td style="text-align: left;">
                                                                                                            <asp:TextBox ID="TxtProjectNoForAnalyticalPhase" runat="server" Text="" Style="width: 80%;"
                                                                                                                CssClass="textBox" />
                                                                                                            <asp:Button Style="display: none" ID="btnSetProjectforAnalyticalPhase"
                                                                                                                    OnClick="btnSetProjectforAnalyticalPhase_Click" runat="server" Text=" Project" />
                                                                                                            <asp:HiddenField ID="HProjectIdforAnalyticalPhase" runat="server" />
                                                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtenderForAnalyticalPhase" runat="server"
                                                                                                                BehaviorID="AutoCompleteExtenderForAnalyticalPhase" CompletionListCssClass="autocomplete_list"
                                                                                                                CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListItemCssClass="autocomplete_listitem"
                                                                                                                MinimumPrefixLength="1" OnClientItemSelected="OnSelectedForAnalyticalPhase" OnClientShowing="ClientPopulatedForAnalyticalPhase"
                                                                                                                ServiceMethod="GetMyProjectCompletionListForDashboard" ServicePath="AutoComplete.asmx"
                                                                                                                TargetControlID="TxtProjectNoForAnalyticalPhase" UseContextKey="True" CompletionListElementID="pnlAnalyticalProjects">
                                                                                                            </cc1:AutoCompleteExtender>
                                                                                                            <asp:Panel ID="pnlAnalyticalProjects" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td style="text-align: right;">Sponsor Name :
                                                                                                        </td>
                                                                                                        <td style="text-align: left">
                                                                                                            <asp:DropDownList ID="DdlCLientNameForAnalyticalPhase" runat="server" AutoPostBack="true"
                                                                                                                OnSelectedIndexChanged="DdlCLientNameForAnalyticalPhase_SelectedIndexChanged" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </tbody>
                                                                                            </table>
                                                                                        <asp:UpdatePanel ID="upPnl_Analysis" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                                                            <ContentTemplate>
                                                                                                <asp:Panel ID="pnlAnalysis" runat="server" Style="width: 98%; margin: auto;" BorderWidth="0px"
                                                                                                    ScrollBars="none">
                                                                                                    <asp:GridView ID="gvwpnl_Analysis" runat="server" Style="width: 100%;"
                                                                                                        AutoGenerateColumns="False" OnPageIndexChanging="gvwpnl_Analysis_PageIndexChanging" ShowHeaderWhenEmpty="true" SkinID="grdViewForDashboard">
                                                                                                        <Columns>
                                                                                                            <asp:TemplateField HeaderText="Details">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:LinkButton ID="lnkBtnDetailsForAnalyticalphase" runat="server" Text="Details"
                                                                                                                        class="" />
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:BoundField DataField="vWorkSpaceId" HeaderText="WorkSpaceId" />
                                                                                                            <asp:BoundField DataField="vProjectNo" HeaderText="Project No." />
                                                                                                            <asp:BoundField DataField="vDrugName" HeaderText="Drug Name" />
                                                                                                            <asp:BoundField DataField="vClientName" HeaderText="Sponsor" />
                                                                                                            <asp:BoundField DataField="iNoOfSubjects" HeaderText="No Of Subjects" />
                                                                                                            <asp:BoundField DataField="NoOfTimePoints" HeaderText="No Of TimePoints" />
                                                                                                            <asp:BoundField DataField="cProjectStatus" HeaderText="Status" />
                                                                                                        </Columns>
                                                                                                    </asp:GridView>
                                                                                                </asp:Panel>
                                                                                                <div>                        
                                                                                                </div>
                                                                                                </ContentTemplate>
                                                                                            <Triggers>
                                                                                                <asp:PostBackTrigger ControlID="BtnExportToExcelForAnalyticalPhase" />
                                                                                                <asp:AsyncPostBackTrigger  ControlID="DdlCLientNameForAnalyticalPhase" EventName="SelectedIndexChanged" />
                                                                                                <asp:AsyncPostBackTrigger  ControlID="btnSetProjectforAnalyticalPhase" EventName="Click" />
                                                                                            </Triggers>
                                                                                        </asp:UpdatePanel>
                                                                                    </div>
                                                                                    <div class="modal-footer">
                                                                                        <asp:Button ID="BtnExportToExcelForAnalyticalPhase" runat="server" Text="" Tooltip="Export To Excel"
                                                                                                        CssClass="btn btnexcel" />
                                                                                    </div>
                                                                                </div>

                                                                                <%--'* 5th pane started [Document Phase] *'--%>
                                                                                <div class="col-lg-3" id="TrDocument" style="display:none" runat="server" onclick="ShowHideDiv('divDocumentPhase','imgDocumentPhase','DocumentProjects');">
                                                                                    <div class="small-box bg-aquaDashboard">
                                                                                        <div class="inner">
                                                                                            <h3 style="margin-right:100%">
                                                                                                <asp:Label ID="LblDocumentPhase" runat="server" Style="border-radius: 10%; padding-left: 2px; 
                                                                                                        padding-right: 2px; margin-left: 0.5%;" class="LabelColor"/>
                                                                                            </h3>
                                                                                            <p style="margin-right:10%">Projects In Document Phase</p>
                                                                                        </div>
                                                                                        <div class="icon">
                                                                                            <i class="fa fa-list-alt"></i>
                                                                                        </div>
                                                                                        <a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i></a>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="modal-content modal-lg" id="divDocumentPhase" style="display:none;">
                                                                                    <div class="modal-header" id="divDocumentPhaseHeader">
                                                                                        <img src="images/Sqclose.gif" class="ModalImage" onclick="ModalClose();" alt="" />
                                                                                        <img src="images/plus-icon.png" class="ModalImage" id="divDocumentPhasemaxmin" height="28px" width="28px" alt="plusbtn" onclick="ModalExtend('divDocumentPhase');" style="cursor:pointer;margin-right:5px;"/>
                                                                                        <h2 style="text-align:center;">Projects In Document Phase</h2>
                                                                                    </div>
                                                                                    <div class="modal-body modalheight">
                                                                                        <table style="text-align: left; width: 100%;">
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td style="text-align: right; width: 15%;">Project No :
                                                                                                    </td>
                                                                                                    <td style="text-align: left;">
                                                                                                        <asp:TextBox ID="TxtProjectNoForDocumentPhase" runat="server" Text="" Style="width: 80%"
                                                                                                            CssClass="textBox" />
                                                                                                        <asp:Button Style="display: none" ID="BtnSetProjectForDocumentPhase"
                                                                                                                runat="server" Text="Project" />
                                                                                                        <asp:HiddenField ID="HProjectIdforDocumentPhase" runat="server" />
                                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtenderForDocumentPhase" runat="server"
                                                                                                            BehaviorID="AutoCompleteExtenderForDocumentPhase" CompletionListCssClass="autocomplete_list"
                                                                                                            CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListItemCssClass="autocomplete_listitem"
                                                                                                            MinimumPrefixLength="1" OnClientItemSelected="OnSelectedForDocumentPhase" OnClientShowing="ClientPopulatedForDocumentPhase"
                                                                                                            ServiceMethod="GetMyProjectCompletionListForDashboard" ServicePath="AutoComplete.asmx"
                                                                                                            TargetControlID="TxtProjectNoForDocumentPhase" UseContextKey="True" CompletionListElementID="pnlProjectListDocPhs">
                                                                                                        </cc1:AutoCompleteExtender>
                                                                                                        <asp:Panel ID="pnlProjectListDocPhs" runat="server" Style="max-height: 300px; overflow: auto; overflow-x: hidden;" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="text-align: right;">Sponsor Name :
                                                                                                    </td>
                                                                                                    <td style="text-align: left;">
                                                                                                        <asp:DropDownList ID="DdllstForDocumentPhase" runat="server" AutoPostBack="true" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>
                                                                                        <asp:UpdatePanel ID="upPnl0004" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                                                        <ContentTemplate>
                                                                                            <asp:Panel ID="pnl0004" runat="server" Style="width: 98%; margin: auto;" BorderWidth="0px"
                                                                                                ScrollBars="Auto">
                                                                                                <asp:GridView ID="gvwpnl0004" runat="server" Style="width: 100%"
                                                                                                    AutoGenerateColumns="False" SkinID="grdViewForDashboard">
                                                                                                    <Columns>
                                                                                                        <asp:TemplateField HeaderText="Details">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:LinkButton ID="lnkBtnDetailsForDocumentphase" runat="server" Text="Details" />
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:BoundField DataField="vWorkSpaceId" HeaderText="WorkSpaceId" />
                                                                                                        <asp:BoundField DataField="vProjectNo" HeaderText="Project No." />
                                                                                                        <asp:BoundField DataField="vDrugName" HeaderText="Drug Name" />
                                                                                                        <asp:BoundField DataField="vClientName" HeaderText="Sponsor" />
                                                                                                        <asp:BoundField DataField="iNoOfSubjects" HeaderText=" No of Subjects" />
                                                                                                    </Columns>
                                                                                                </asp:GridView>
                                                                                            </asp:Panel>
                                                                                        </ContentTemplate>
                                                                                        <Triggers>
                                                                                            <asp:PostBackTrigger ControlID="BtnExportToExcelForDocumentPhase" />
                                                                                            <asp:AsyncPostBackTrigger  ControlID="DdllstForDocumentPhase" EventName="SelectedIndexChanged" />
                                                                                            <asp:AsyncPostBackTrigger  ControlID="BtnSetProjectForDocumentPhase" EventName="Click" />
                                                                                        </Triggers>
                                                                                    </asp:UpdatePanel>
                                                                                    </div>
                                                                                    <div class="modal-footer">
                                                                                        <asp:Button ID="BtnExportToExcelForDocumentPhase" runat="server" Text="" CssClass="btn btnexcel" ToolTip="Export To Excel"/>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <div id="divAnalytics" style="display:none">
                                                                                <%--'* 6th pane started [Screening Analytic] *'--%>
                                                                                <div class="col-lg-3" id="TrScreeningAnalytic" runat="server" style="display:none">
                                                                                    <div class="small-box bg-aquaDashboard">
                                                                                        <div class="inner">
                                                                                            <h3 style="margin-right:100%">
                                                                                                <asp:Label ID="Label2" runat="server" Style="border-radius: 10%; padding-left: 2px; padding-right: 2px; margin-left: 0.5%;" />
                                                                                            </h3>
                                                                                            <p style="margin-right:10%">Screening Analytics</p>
                                                                                        </div>
                                                                                        <div class="icon">
                                                                                            <i class="fa fa-list-alt"></i>
                                                                                        </div>
                                                                                        <a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i></a>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="modal-content modal-lg" id="divScreeningAnalytic" style="display:none;">
                                                                                    <div class="modal-header" id="divScreeningAnalyticHeader">
                                                                                        <img src="images/Sqclose.gif" class="ModalImage" onclick="ModalClose();" alt="" />
                                                                                        <img src="images/plus-icon.png" class="ModalImage" id="divScreeningAnalyticmaxmin" height="28px" width="28px" alt="plusbtn" onclick="ModalExtend('divScreeningAnalytic');" style="cursor:pointer;margin-right:5px;"/>
                                                                                        <h2 style="text-align:center;">Screening Analytics</h2>
                                                                                    </div>
                                                                                    <div class="modal-body modalheight">
                                                                                        <table style="text-align: left; width: 80%; margin: auto;">
                                                                                            <tr>
                                                                                                <td>Month :<asp:DropDownList runat="server" ID="DdllistMonth" />
                                                                                                </td>
                                                                                                <td>Year :<asp:DropDownList runat="server" ID="DdllistYear" />
                                                                                                </td>

                                                                                                <td>
                                                                                                    <asp:Button runat="server" Text="" ID="BtnGoFOrScreeningAnalytic" CssClass="btn btngo"
                                                                                                        OnClick="BtnGoFOrScreeningAnalytic_Click" Tooltip="Go"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                        <asp:UpdatePanel ID="upPnlScreeningAnalytic" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                                                            <ContentTemplate>
                                                                                                <asp:Panel ID="pnl0005" runat="server" Style="width: 100%; margin: auto; margin-bottom: 2%;"
                                                                                                    BorderWidth="0px" ScrollBars="none" Visible="false">

                                                                                                    <asp:GridView ID="gvwpnlScreeningAnalytic" runat="server" Style="width: 80%" AutoGenerateColumns="False" 
                                                                                                            ShowHeaderWhenEmpty="true" SkinID="grdViewForDashboard">
                                                                                                        <Columns>
                                                                                                            <asp:BoundField DataField="vDisplayName" HeaderText="Screening Ratio" />
                                                                                                            <asp:TemplateField HeaderText="Generic Screening Ratio">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:LinkButton ID="lnkGenericScreening" runat="server" Text='<%#Eval("vGenericValue") %>'
                                                                                                                        OnClick="lnkGenericScreening_click">
                                                                                                                    </asp:LinkButton>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Project Specific Screening Ratio">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:LinkButton ID="lnkProjectSpecific" runat="server" Text='<%#Eval("vPscrValue") %>'
                                                                                                                        OnClick="lnkProjectSpecifcScreening_click">
                                                                                                                    </asp:LinkButton>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                        </Columns>
                                                                                                    </asp:GridView>
                                                                                                </asp:Panel>

                                                                                                <button id="btn0" runat="server" style="display: none;" />
                                                                                                <cc1:ModalPopupExtender ID="mdpSubjectInfo" runat="server" PopupControlID="Div17"
                                                                                                    PopupDragHandleControlID="Label1" BackgroundCssClass="modalBackground" TargetControlID="btn0"
                                                                                                    CancelControlID="Img12">
                                                                                                </cc1:ModalPopupExtender>
                                                                                                <div id="Div17" runat="server" style="display: none;" class="modal-content modal-lg">
                                                                                                    <div class="modal-header">
                                                                                                        <img id="Img12" alt="" src="images/Sqclose.gif" style="position: relative; float: right; left: 10px;top:9px"  title="Close"/>
                                                                                                        <h2>Subject Information</h2>
                                                                                                    </div>
                                                                                                    <div class="modal-body modalheight">
                                                                                                        <asp:Label ID="lbltask" runat="server" class="LabelBold" Font-Size="Small" Style="margin-left: 1%"></asp:Label> <br />
                                                                                                        <asp:Panel ID="Panel1" runat="server" Style="width: 100%; margin: auto;" BorderWidth="0px" ScrollBars="Auto">
                                                                                                            <asp:GridView runat="server" ID="grdSubjectInfo" AutoGenerateColumns="False"
                                                                                                                            AllowSorting="true" Style="width: 100%;"
                                                                                                                            OnPageIndexChanging="grdSubjectInfo_PageIndexChanging" ShowHeaderWhenEmpty="true" SkinID="grdViewForDashboard">
                                                                                                                <Columns>
                                                                                                                    <asp:BoundField DataField="vProjectNo" HeaderText="Project No" ItemStyle-HorizontalAlign="Center"
                                                                                                                        HeaderStyle-Width="10%" Visible="false" />
                                                                                                                    <asp:BoundField DataField="vSubjectId" HeaderText="Subject Id" ItemStyle-HorizontalAlign="Center"
                                                                                                                        HeaderStyle-Width="15%" />
                                                                                                                    <asp:BoundField DataField="vInitials" HeaderText="Initials" ItemStyle-HorizontalAlign="Center"
                                                                                                                        HeaderStyle-Width="8%" />
                                                                                                                    <asp:BoundField DataField="vSubjectName" HeaderText="FullName" ItemStyle-HorizontalAlign="left"
                                                                                                                        HeaderStyle-Width="30%" />
                                                                                                                    <asp:BoundField DataField="dScreenDate" HeaderText="Screening-Date" ItemStyle-HorizontalAlign="Center"
                                                                                                                        HeaderStyle-Width="12%" />
                                                                                                                </Columns>
                                                                                                            </asp:GridView>
                                                                                                        </asp:Panel>
                                                                                                    </div>
                                                                                                    <div class="modal-footer">
                                                                                                        <asp:Button ID="btnExportToExcelForScreeningSubjectInfo" runat="server" CssClass="btn btnexcel" Tooltip ="Export To Excel" />
                                                                                                    </div>
                                                                                                </div>
                                                                                            </ContentTemplate>
                                                                                            <Triggers>
                                                                                                <asp:AsyncPostBackTrigger ControlID="BtnGoFOrScreeningAnalytic" />
                                                                                                <asp:PostBackTrigger ControlID="btnExportToExcelForScreeningSubjectInfo" />
                                                                                            </Triggers>
                                                                                        </asp:UpdatePanel>   
                                                                                    </div>

                                                                                    <div class="modal-footer">
                                                                                        <h3></h3>
                                                                                    </div>
                                                                                </div>

                                                                                <%--'* 7th pane started [Project Study Work Summary] *'--%>
                                                                                <div class="col-lg-3" id="TrProjectStudyWorkSummary" runat="server" style="display:none">
                                                                                    <div class="small-box bg-aquaDashboard">
                                                                                        <div class="inner">
                                                                                            <h3 style="margin-right:100%">
                                                                                                <asp:Label ID="Label14" runat="server" Style="border-radius: 10%; padding-left: 2px; padding-right: 2px; margin-left: 0.5%;" />
                                                                                            </h3>
                                                                                            <p style="margin-right:10%">Project/Study Work Summary</p>
                                                                                        </div>
                                                                                        <div class="icon">
                                                                                            <i class="fa fa-list-alt"></i>
                                                                                        </div>
                                                                                        <a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i></a>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="modal-content modal-lg" id="divProjectStudyWorkSummary" style="display:none;">
                                                                                    <div class="modal-header" id="divProjectStudyWorkSummaryHeader">
                                                                                        <img src="images/Sqclose.gif" class="ModalImage" onclick="ModalClose();" alt="" />
                                                                                        <img src="images/plus-icon.png" class="ModalImage" id="divProjectStudyWorkSummarymaxmin" height="28px" width="28px" alt="plusbtn" onclick="ModalExtend('divProjectStudyWorkSummary');" style="cursor:pointer;margin-right:5px;"/>
                                                                                        <h2 style="text-align:center;">Project/Study Work Summary</h2>
                                                                                    </div>
                                                                                    <div class="modal-body modalheight">
                                                                                        <asp:UpdatePanel ID="upPnlProjectStudyWorkSummary" runat="server">
                                                                                            <ContentTemplate>
                                                                                        <table style="width: 100%">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table style="width: 80%; margin: auto;">
                                                                                                        <tr>
                                                                                                            <td style="text-align: left;">Filter On :
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:DropDownList runat="server" ID="DdllistFilterOn" class="dropDownList" />
                                                                                                            </td>
                                                                                                            <td style="text-align: left;">From Date :
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:TextBox runat="server" ID="TxtFromDate" Text="" CssClass="textBox" Style="width: 85px" />
                                                                                                                <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="TxtFromDate"
                                                                                                                    Format="dd-MMM-yyyy">
                                                                                                                </cc1:CalendarExtender>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td style="text-align: left;">Report For :
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:DropDownList runat="server" ID="DdllistReportFor" class="dropDownList" AutoPostBack="true" />
                                                                                                            </td>
                                                                                                            <td style="text-align: left;">To Date :
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:TextBox runat="server" ID="TxtToDate" Text="" CssClass="textBox" Style="width: 85px" />
                                                                                                                <cc1:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="TxtToDate"
                                                                                                                    Format="dd-MMM-yyyy" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:RadioButtonList ID="RdbtnListForProjectStudyWorkSummary" runat="server" RepeatDirection="Horizontal">
                                                                                                                    <asp:ListItem Text="Details" Value="0" Selected="True" />
                                                                                                                    <asp:ListItem Text="Summary" Value="1" />
                                                                                                                </asp:RadioButtonList>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Button ID="BtnGoForStudyWorkSummary" Text="" runat="server" CssClass="btn btngo" tooltip ="Go"/>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>

                                                                                        <table>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:DropDownList runat="server" ID="DdllistAllSponsor" class="dropDownList" AutoPostBack="true"
                                                                                                        Visible="false" Enabled="false" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:DropDownList runat="server" ID="DdllistAllProjectType" class="dropDownList"
                                                                                                        AutoPostBack="true" Enabled="false" Visible="false" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:DropDownList runat="server" ID="DdllistPilotPivotal" class="dropDownList" AutoPostBack="true"
                                                                                                        Enabled="false" Visible="false" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                        
                                                                                                <asp:Panel ID="pnl0006" runat="server" Style="width: 98%; margin: auto;" BorderWidth="0px"
                                                                                                    ScrollBars="none" Visible="False">
                                                                                                    <asp:GridView runat="server" ID="GvwPnlProjectStudyWorkSummary" AutoGenerateColumns="False" Style="width: 100%;" ShowHeaderWhenEmpty="true" SkinID="grdViewForDashboard">
                                                                                                        <Columns>
                                                                                                            <asp:TemplateField HeaderText="Details">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:LinkButton ID="lnkBtnDetailsForWorkSummary" runat="server" Text="Details" />
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:BoundField DataField="vWorkSpaceId" HeaderText="WorkSpaceId" />
                                                                                                            <asp:BoundField DataField="vProjectNo" HeaderText="Project No" />
                                                                                                            <asp:BoundField DataField="vWorkTypeDesc" HeaderText="Pilot/Pivotal" />
                                                                                                            <asp:BoundField DataField="vClientName" HeaderText="Sponsor" />
                                                                                                            <asp:BoundField DataField="vRegionName" HeaderText="Regulatory Submission" />
                                                                                                            <asp:BoundField DataField="vProjectTypeName" HeaderText="Project Type" />
                                                                                                            <asp:BoundField DataField="iNoOfSubjects" HeaderText="Study Subjects" />
                                                                                                            <asp:BoundField DataField="DosedSubjects" HeaderText="Dosed Subjects" />
                                                                                                            <asp:BoundField DataField="cPermissionRequired" HeaderText="DCGI Approval" />
                                                                                                            <asp:BoundField DataField="ClinicEndDate" HeaderText="Clinic End Date" HtmlEncode="false"
                                                                                                                ItemStyle-Wrap="false" DataFormatString="{0:dd-MMM-yy}" />
                                                                                                            <asp:BoundField DataField="SampleAnalysisEndDate" HeaderText="Sample Analysis End Date"
                                                                                                                HtmlEncode="false" ItemStyle-Wrap="false" DataFormatString="{0:dd-MMM-yy}" />
                                                                                                            <asp:BoundField DataField="SponsorReportDate" HeaderText="Report Sent To Sponsor Date"
                                                                                                                HtmlEncode="false" ItemStyle-Wrap="false" DataFormatString="{0:dd-MMM-yy}" />
                                                                                                            <asp:BoundField DataField="ReportDispatchDate" HeaderText="Report Dispatch End Date"
                                                                                                                HtmlEncode="false" ItemStyle-Wrap="false" DataFormatString="{0:dd-MMM-yy}" />
                                                                                                            <asp:BoundField DataField="vDrugName" HeaderText="Drug Name" />
                                                                                                            <asp:BoundField DataField="vProjectManager" HeaderText="Project Manager" />
                                                                                                        </Columns>
                                                                                                    </asp:GridView>
                                                                                                    <asp:Label runat="Server" ID="LblNoRecordFound" Text="No Record Found." Visible="false"
                                                                                                        ForeColor="Red" />
                                                                                                </asp:Panel>
                                                                                                <asp:Panel ID="pnlChartForWorkSummary1" runat="server" Width="650px" BorderWidth="1px"
                                                                                                    ScrollBars="Horizontal" Visible="False">
                                                                                                    <table style="width: 100%">
                                                                                                        <tr>
                                                                                                            <td colspan="2" align="left">
                                                                                                                <asp:Label runat="server" ID="LblForStdTimeOfESHeadingInWorkSummary" Text="" />
                                                                                                                <asp:TextBox ID="TxtStdTimeForES" runat="server" Width="20" Height="15" Visible="false"
                                                                                                                    Text="28" />
                                                                                                                <asp:TextBox ID="TxtStdForESinReportDispatch" runat="server" Width="20" Height="15"
                                                                                                                    Visible="false" Text="42" />
                                                                                                                <asp:Label ID="LblForDaysESinWorkSummary" runat="server" Text="" Visible="false" />
                                                                                                                <asp:Button ID="BtnChangeForESInWorkSummary" runat="server" Text="Change" CssClass="btn btnsave"
                                                                                                                    Visible="false" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td colspan="2" align="left">
                                                                                                                <asp:Label runat="server" ID="LblForStdTimeOfSAHeadingInWorkSummary" Text="" />
                                                                                                                <asp:TextBox ID="TxtStdTimeForSA" runat="server" Width="20" Visible="false" Height="15"
                                                                                                                    Text="14" />
                                                                                                                <asp:Label ID="LblForDaysSAinWorkSummary" runat="server" Text="" Visible="false" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td style="text-align: left;"></td>
                                                                                                            <td rowspan="3" align="right">
                                                                                                                <asp:Chart ID="Chart1" runat="server" Palette="BrightPastel" BackColor="WhiteSmoke"
                                                                                                                    Height="296px" Width="412px" BorderDashStyle="Solid" BackSecondaryColor="White"
                                                                                                                    BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="26, 59, 105" ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)">
                                                                                                                    <Titles>
                                                                                                                        <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 10pt, style=Bold" ShadowOffset="3"
                                                                                                                            Text="Pie Chart" Name="Title1" ForeColor="26, 59, 105">
                                                                                                                        </asp:Title>
                                                                                                                    </Titles>
                                                                                                                    <Legends>
                                                                                                                        <asp:Legend BackColor="Transparent" Alignment="Center" Docking="Bottom" Font="Trebuchet MS, 8.25pt, style=Bold"
                                                                                                                            IsTextAutoFit="False" Name="Default" LegendStyle="Row">
                                                                                                                        </asp:Legend>
                                                                                                                    </Legends>
                                                                                                                    <BorderSkin SkinStyle="Emboss" />
                                                                                                                    <Series>
                                                                                                                        <asp:Series Name="Default" ChartType="Pie" BorderColor="180, 26, 59, 105" Color="220, 65, 140, 240">
                                                                                                                        </asp:Series>
                                                                                                                    </Series>
                                                                                                                    <ChartAreas>
                                                                                                                        <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BackSecondaryColor="Transparent"
                                                                                                                            BackColor="Transparent" ShadowColor="Transparent" BorderWidth="0">
                                                                                                                            <Area3DStyle Rotation="0" />
                                                                                                                            <AxisY LineColor="64, 64, 64, 64">
                                                                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                                                            </AxisY>
                                                                                                                            <AxisX LineColor="64, 64, 64, 64">
                                                                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                                                            </AxisX>
                                                                                                                        </asp:ChartArea>
                                                                                                                    </ChartAreas>
                                                                                                                </asp:Chart>
                                                                                                                <br />
                                                                                                                <asp:Chart ID="Chart2" runat="server" Palette="BrightPastel" BackColor="WhiteSmoke"
                                                                                                                    Height="296px" Width="412px" BorderDashStyle="Solid" BackSecondaryColor="White"
                                                                                                                    BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="26, 59, 105" ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)">
                                                                                                                    <Titles>
                                                                                                                        <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 10pt, style=Bold" ShadowOffset="3"
                                                                                                                            Text="Pie Chart" Name="Title1" ForeColor="26, 59, 105">
                                                                                                                        </asp:Title>
                                                                                                                    </Titles>
                                                                                                                    <Legends>
                                                                                                                        <asp:Legend BackColor="Transparent" Alignment="Center" Docking="Bottom" Font="Trebuchet MS, 8.25pt, style=Bold"
                                                                                                                            IsTextAutoFit="False" Name="Default" LegendStyle="Row">
                                                                                                                        </asp:Legend>
                                                                                                                    </Legends>
                                                                                                                    <BorderSkin SkinStyle="Emboss" />
                                                                                                                    <Series>
                                                                                                                        <asp:Series Name="Default" ChartType="Pie" BorderColor="180, 26, 59, 105" Color="220, 65, 140, 240">
                                                                                                                        </asp:Series>
                                                                                                                    </Series>
                                                                                                                    <ChartAreas>
                                                                                                                        <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BackSecondaryColor="Transparent"
                                                                                                                            BackColor="Transparent" ShadowColor="Transparent" BorderWidth="0">
                                                                                                                            <Area3DStyle Rotation="0" />
                                                                                                                            <AxisY LineColor="64, 64, 64, 64">
                                                                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                                                            </AxisY>
                                                                                                                            <AxisX LineColor="64, 64, 64, 64">
                                                                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                                                            </AxisX>
                                                                                                                        </asp:ChartArea>
                                                                                                                    </ChartAreas>
                                                                                                                </asp:Chart>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td style="text-align: left; vertical-align: top;">
                                                                                                                <table cellspacing="3" class="table">
                                                                                                                    <tr class="SetTableData">
                                                                                                                        <td>
                                                                                                                            <asp:Label ID="LblProjects" runat="server" Text="" Visible="false" />
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <asp:LinkButton ID="LnkBtnProjectsCountsForES" runat="server" Text="" Visible="false"
                                                                                                                                Font-Bold="True" ForeColor="Navy" />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                            <asp:Label ID="LblAverageTimeFromES" runat="server" Text="" Visible="false" />
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <asp:Label ID="LblAverageTimeCountsForES" runat="server" Text="" Visible="false"
                                                                                                                                Font-Bold="true" />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr class="SetTableData">
                                                                                                                        <td>
                                                                                                                            <asp:Label ID="LblOnTimeFromES" runat="server" Text="" Visible="false" />
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <asp:LinkButton ID="LnkBtnOnTimeCountsForES" runat="server" Text="" Visible="false"
                                                                                                                                Font-Bold="True" ForeColor="Navy" />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                            <asp:Label ID="LblDelayedFromES" runat="server" Text="" Visible="false" />
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <asp:LinkButton ID="LnkBtnDelayedCountsForES" runat="server" Text="" Visible="false"
                                                                                                                                Font-Bold="True" ForeColor="Navy" />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr class="SetTableData">
                                                                                                                        <td>
                                                                                                                            <asp:Label ID="LblAverageTimeFromSA" runat="server" Text="" Visible="false" />
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <asp:Label ID="LblAverageTimeCountsForSA" runat="server" Text="" Visible="false"
                                                                                                                                Font-Bold="true" />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                            <asp:Label ID="LblOnTimeFromSA" runat="server" Text="" Visible="false" />
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <asp:LinkButton ID="LnkBtnOnTimeCountsForSA" runat="server" Text="" Visible="false"
                                                                                                                                Font-Bold="True" ForeColor="Navy" />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr class="SetTableData">
                                                                                                                        <td>
                                                                                                                            <asp:Label ID="LblDelayedFromSA" runat="server" Text="" Visible="false" />
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <asp:LinkButton ID="LnkBtnDelayedCountsForSA" runat="server" Text="" Visible="false"
                                                                                                                                Font-Bold="True" ForeColor="Navy" />
                                                                                                                            <asp:Label ID="LblDelyedCountsForSA" runat="server" Text="" Visible="false" Font-Bold="true" />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                            <asp:Image ID="ImageForES" runat="server" ImageUrl="~/images/question.gif" Visible="false" />
                                                                                                                            <asp:Label ID="LblListOfProjectForES" runat="server" Text="" Visible="false" ForeColor="red" />
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <div id="DivForES" style="position: absolute; display: none; border: 2; background-color: #CEE3ED; width: 300px;">
                                                                                                                                <asp:Label ID="LblListOfProjectNoForES" runat="server" Text="" Visible="false" Font-Bold="true" />
                                                                                                                            </div>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                            <asp:Image ID="ImageForSA" runat="server" ImageUrl="~/images/question.gif" Visible="false" />
                                                                                                                            <asp:Label ID="LblListOfProjectForSA" runat="server" Text="" Visible="false" ForeColor="red" />
                                                                                                                            <div id="DivForSA" style="position: absolute; display: none; border: 2; background-color: #CEE3ED; width: 300px;">
                                                                                                                                <asp:Label ID="LblListOfProjectNoForSA" runat="server" Text="" Visible="false" Font-Bold="true" />
                                                                                                                            </div>
                                                                                                                        </td>
                                                                                                                        <td></td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </asp:Panel>

                                                                                                <button id="Btn2" runat="server" style="display: none;" />
                                                                                                <cc1:ModalPopupExtender ID="Mpedialog2" runat="server" PopupControlID="DivPopUpWorkSummary"
                                                                                                    PopupDragHandleControlID="LblPopUpTitleWorkSummary" BackgroundCssClass="modalBackground"
                                                                                                    TargetControlID="btn2" CancelControlID="ImgPopUpCloseWorkSummary">
                                                                                                </cc1:ModalPopupExtender>

                                                                                                <div id="DivPopUpWorkSummary" runat="server" style="position: relative; display: none; background-color: #cee3ed; padding: 5px; width: 750px; height: inherit; border: dotted 1px gray;">
                                                                                                    <div>
                                                                                                        <h1>
                                                                                                            <div>
                                                                                                                <img id="ImgPopUpCloseWorkSummary" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px;" />
                                                                                                                <asp:Label ID="LblPopUpTitleWorkSummary" runat="server" class="LabelBold" Visible="true" />
                                                                                                            </div>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                            <h1></h1>
                                                                                                        </h1>
                                                                                                    </div>
                                                                                                    <table border="0" cellpadding="2" cellspacing="2" width="98%">
                                                                                                        <tr>
                                                                                                            <td align="left">Project No :
                                                                                                            </td>
                                                                                                            <td align="left">
                                                                                                                <asp:Label runat="server" ID="LblProjectNoPopUpWorkSummary" Font-Bold="true" ForeColor="Navy" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="center" valign="top" colspan="2">
                                                                                                                <asp:Button ID="Button1" runat="server" Text="Update" CssClass="btn btnupdate" Visible="false" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </div>
                                                                                            </ContentTemplate>
                                                                                            <Triggers>
                                                                                                <asp:PostBackTrigger ControlID="BtnExportToExcelForProjectStudyWorkSummary" />
                                                                                                <asp:AsyncPostBackTrigger ControlID="BtnGoForStudyWorkSummary" />
                                                                                                <asp:AsyncPostBackTrigger ControlID="BtnChangeForESInWorkSummary" />
                                                                                                <asp:AsyncPostBackTrigger  ControlID="DdllistReportFor" EventName="SelectedIndexChanged" />
                                                                                            </Triggers>
                                                                                        </asp:UpdatePanel>
                                                                                    </div>
                                                                                    <div class="modal-footer">
                                                                                        <asp:Button runat="server" ID="BtnExportToExcelForProjectStudyWorkSummary" Text="" CssClass="btn btnexcel" Tooltip="Export To Excel"/>
                                                                                    </div>
                                                                                </div>

                                                                                <%--'* 8th pane started [Operational KPI] *'--%>
                                                                                <div class="col-lg-3" id="TrOperationalKpi" runat="server" style="display:none">
                                                                                    <div class="small-box bg-aquaDashboard">
                                                                                        <div class="inner">
                                                                                            <h3 style="margin-right:100%">
                                                                                                <asp:Label ID="Label15" runat="server" Style="border-radius: 10%; padding-left: 2px; padding-right: 2px; margin-left: 0.5%;" />
                                                                                            </h3>
                                                                                            <p style="margin-right:10%">Operational KPI</p>
                                                                                        </div>
                                                                                        <div class="icon">
                                                                                            <i class="fa fa-list-alt"></i>
                                                                                        </div>
                                                                                        <a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i></a>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="modal-content modal-lg" id="divOperationalKpi" style="display:none;">
                                                                                    <div class="modal-header" id="divOperationalKpiHeader">
                                                                                        <img src="images/Sqclose.gif" class="ModalImage" onclick="ModalClose();" alt="" />
                                                                                        <img src="images/plus-icon.png" class="ModalImage" id="divOperationalKpimaxmin" height="28px" width="28px" alt="plusbtn" onclick="ModalExtend('divOperationalKpi');" style="cursor:pointer;margin-right:5px;"/>
                                                                                        <h2 style="text-align:center;">Operational KPI</h2>
                                                                                    </div>

                                                                                    <div class="modal-body modalheight">
                                                                                         <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                                        <ContentTemplate>
                                                                                        <table style="width: 90%; margin: auto;">
                                                                                            <tr>
                                                                                                <td align="left">
                                                                                                    <asp:DropDownList ID="DdllistForOperationalKpi" runat="server" AutoPostBack="true">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label runat="server" Text="From Date :" ID="LblFromDateForOperationalKpi" Enabled="false" />
                                                                                                    <asp:TextBox runat="server" ID="TxtFromDateOfOperationalKpi" Text="" CssClass="textBox"
                                                                                                        Enabled="false" Style="width: 80px;" />
                                                                                                    <cc1:CalendarExtender ID="CalExtFromDateForOperationalKpi" runat="server" TargetControlID="TxtFromDateOfOperationalKpi"
                                                                                                        Format="dd-MMM-yyyy">
                                                                                                    </cc1:CalendarExtender>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label runat="server" Text="To Date :" ID="LblToDateForOperationalKpi" Enabled="false" />
                                                                                                    <asp:TextBox runat="server" ID="TxtToDateOfOperationalKpi" Text="" CssClass="textBox"
                                                                                                        Enabled="false" Style="width: 80px;" />
                                                                                                    <cc1:CalendarExtender ID="CalExtToDateForOperationalKpi" runat="server" TargetControlID="TxtToDateOfOperationalKpi"
                                                                                                        Format="dd-MMM-yyyy">
                                                                                                    </cc1:CalendarExtender>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Button ID="BtnGoForOperationalKpi" runat="server" CssClass="btn btngo" Text="" ToolTip="Go"
                                                                                                        Enabled="false" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                        <br />
                                                                                       
                                                                                        <asp:Panel runat="server" ID="PnlTblForOperationalKpi" Visible="false" Style="text-align: left; width: 98%; margin: auto; margin-bottom: 1%;">
                                                                                            <table cellpadding="1" cellspacing="1" style="width: 100%;" class="table" id="tblOperationalKpi">
                                                                                                <tr class="SetTableHeader">
                                                                                                    <td>
                                                                                                        <b>Description</b>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <b>Total </b>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <b>Pilot </b>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <b>Pivotal </b>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="text-align: left;">No Of Bed Nights
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:LinkButton runat="server" ID="LnkBtnBedNightsTotal" Text="dfgdf" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:LinkButton runat="server" ID="LnkBtnBedNightsPilot" Text="dgdg" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:LinkButton runat="server" ID="LnkBtnBedNightsPivotal" Text="dfg" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr class="SetTableData">
                                                                                                    <td style="text-align: left;">No Of Subjects Dosed
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:LinkButton runat="server" ID="LnkBtnSubjectsDosedTotal" Text="" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:LinkButton runat="server" ID="LnkBtnSubjectsDosedPilot" Text="" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:LinkButton runat="server" ID="LnkBtnSubjectsDosedPivotal" Text="" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="text-align: left;">No Of Subjects Total Dosed
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:LinkButton runat="server" ID="LnkBtnSubjectsDosedTtl" Text="" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:LinkButton runat="server" ID="LnkBtnSubjectsDosedttlPilot" Text="" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:LinkButton runat="server" ID="LnkBtnSubjectsDosedTtlPivotal" Text="" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr class="SetTableData">
                                                                                                    <td style="text-align: left;">No Of Projects Clinically Completed
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:LinkButton runat="server" ID="LnkBtnClinicEndTotal" Text="" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:LinkButton runat="server" ID="LnkBtnClinicEndPilot" Text="" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:LinkButton runat="server" ID="LnkBtnClinicEndPivotal" Text="" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="text-align: left;">No Of Projects Analytically completed
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:LinkButton runat="server" ID="LnkBtnSampleEndTotal" Text="" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:LinkButton runat="server" ID="LnkBtnSampleEndPilot" Text="" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:LinkButton runat="server" ID="LnkBtnSampleEndPivotal" Text="" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr class="SetTableData">
                                                                                                    <td style="text-align: left;">No Of Projects Sent for sponsor Review
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:LinkButton runat="server" ID="LnkBtnReportToSponsorReviewEndTotal" Text="" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:LinkButton runat="server" ID="LnkBtnReportToSponsorReviewEndPilot" Text="" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:LinkButton runat="server" ID="LnkBtnReportToSponsorReviewEndPivotal" Text="" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="text-align: left;">No Of Projects Report Dispatched
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:LinkButton runat="server" ID="LnkBtnReportEndTotal" Text="" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:LinkButton runat="server" ID="LnkBtnReportEndPilot" Text="" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:LinkButton runat="server" ID="LnkBtnReportEndPivotal" Text="" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr class="SetTableData">
                                                                                                    <td style="text-align: left;">No Of Sample Analysis
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:LinkButton runat="server" ID="LnkBtnSampleAnalystEndTotal" Text="" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:LinkButton runat="server" ID="LnkBtnSampleAnalystEndPilot" Text="" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:LinkButton runat="server" ID="LnkBtnSampleAnalystEndPivotal" Text="" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="text-align: left;">No Of Projects Released
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:LinkButton runat="server" ID="LnkBtnProjectsReleased" Text="" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:LinkButton runat="server" ID="LnkBtnProjectsReleasedPilot" Text="" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:LinkButton runat="server" ID="LnkBtnProjectsReleasedPivotal" Text="" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </asp:Panel>

                                                                                        <button id="btn1" runat="server" style="display: none;" />
                                                                                        <cc1:ModalPopupExtender ID="mpeDialog" runat="server" PopupControlID="dialogModal"
                                                                                            PopupDragHandleControlID="dialogModalTitle" BackgroundCssClass="modalBackground"
                                                                                            TargetControlID="btn1" CancelControlID="dialogModalClose">
                                                                                        </cc1:ModalPopupExtender>
                                                                                        </ContentTemplate>
                                                                                            <Triggers>
                                                                                                <%--<asp:PostBackTrigger ControlID="BtnExportToExcelForProjectStudyWorkSummary" />--%>
                                                                                                <asp:AsyncPostBackTrigger ControlID="BtnGoForOperationalKpi" />
                                                                                                <asp:AsyncPostBackTrigger  ControlID="DdllistForOperationalKpi" EventName="SelectedIndexChanged" />
                                                                                            </Triggers>
                                                                                        </asp:UpdatePanel>
                                                                                    </div>

                                                                                    <div class="modal-footer">
            
                                                                                    </div>
                                                                                </div>

                                                                                <div class="modal-content modal-lg" id="dialogModal" style="display:none;" runat="server">
                                                                                    <div class="modal-header">
                                                                                        <img id="dialogModalClose" alt="" src="images/Sqclose.gif" class="ModalImage" />
                                                                                        <h2 style="text-align:center;"> <asp:Label ID="dialogModalTitle" runat="server" class="LabelBold" /></h2>
                                                                                    </div>
                                                                                    <div class="modal-body modalheight">
                                                                                        <table style="width: 98%; border: 0" cellpadding="2" cellspacing="2">
                                                                                            <tr>
                                                                                                <td style="text-align: left; vertical-align: top">
                                                                                                    <asp:UpdatePanel ID="upOperationalKPI" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                                                                        <ContentTemplate>
                                                                                                            <div runat="server" id="PanelForOperationalKpi" style="width: 98%; margin: auto; height: auto; max-height: 410px; margin-top: -1%;">
                                                                                                                <asp:GridView ID="GrdvgiewOfOperationalKpi" runat="server" AutoGenerateColumns="false" Visible="false" ShowHeaderWhenEmpty="true">
                                                                                                                    <Columns>
                                                                                                                        <asp:TemplateField HeaderText="Details" ItemStyle-HorizontalAlign="Left">
                                                                                                                            <ItemTemplate>
                                                                                                                                <asp:LinkButton ID="lnkBtnDetailsForOperationalKpi" runat="server" Text="Details" />
                                                                                                                            </ItemTemplate>
                                                                                                                        </asp:TemplateField>
                                                                                                                        <asp:BoundField DataField="vProjectNo" HeaderText="Project No" ItemStyle-HorizontalAlign="Left" />
                                                                                                                        <asp:BoundField DataField="vClientname" HeaderText="Sponsor" ItemStyle-HorizontalAlign="Left" />
                                                                                                                        <asp:BoundField DataField="vWorkTypeDesc" HeaderText="Pilot/Pivtoal" ItemStyle-HorizontalAlign="Left"
                                                                                                                            ItemStyle-Width="30px" />
                                                                                                                        <asp:BoundField DataField="iPeriod" HeaderText="Period" ItemStyle-HorizontalAlign="Left" />
                                                                                                                        <asp:BoundField DataField="iNoOfSubjects" HeaderText="Study Subjects" ItemStyle-HorizontalAlign="Left"
                                                                                                                            ItemStyle-Width="30px" />
                                                                                                                        <asp:BoundField DataField="cPermissionRequired" HeaderText="DCGI Approval" ItemStyle-HorizontalAlign="Left"
                                                                                                                            ItemStyle-Width="10px" />
                                                                                                                        <asp:BoundField DataField="vProjectTypeName" HeaderText="Project Type" ItemStyle-HorizontalAlign="Left"
                                                                                                                            ItemStyle-Width="45px" />
                                                                                                                        <asp:BoundField DataField="vWorkSpaceId" HeaderText="WorkSpaceId" ItemStyle-HorizontalAlign="Left" />
                                                                                                                        <asp:BoundField DataField="Samples" HeaderText="No Of Samples" ItemStyle-HorizontalAlign="Left"
                                                                                                                            ItemStyle-Width="15px" />
                                                                                                                        <asp:BoundField DataField="IxSamples" HeaderText="Incurred Samples" ItemStyle-HorizontalAlign="Left"
                                                                                                                            ItemStyle-Width="15px" />
                                                                                                                        <asp:BoundField DataField="TotalDosed" HeaderText="Total Dosed Subject" ItemStyle-HorizontalAlign="Left"
                                                                                                                            ItemStyle-Width="15px" />
                                                                                                                        <asp:BoundField DataField="BedNightsTtl" HeaderText="No Of Bed Nights" ItemStyle-HorizontalAlign="Left"
                                                                                                                            ItemStyle-Width="15px" />
                                                                                                                    </Columns>
                                                                                                                </asp:GridView>
                                                                                                            </div>
                                                                                                        </ContentTemplate>
                                                                                                        <Triggers>
                                                                                                            <asp:PostBackTrigger ControlID="btnExportToExcelKPI"></asp:PostBackTrigger>
                                                                                                            <asp:PostBackTrigger ControlID="btnExportSiteWiseData"></asp:PostBackTrigger>
                                                                                                            <asp:PostBackTrigger ControlID="btnExportDataStatus"></asp:PostBackTrigger>
                                                                                                            <asp:PostBackTrigger ControlID="btnExportQueryMgt"></asp:PostBackTrigger>
                                                                                                        </Triggers>
                                                                                                    </asp:UpdatePanel>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td style="text-align: center; vertical-align: top">
                                                                                                    <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btnsave" Visible="false"
                                                                                                        OnClientClick="return Validate();" Tootip="Update"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div class="modal-footer">
                                                                                        <asp:Button ID="btnExportToExcelKPI" runat="server" Text="" CssClass="btn btnexcel" Style="margin-top: 1%" Tooltip ="Export To Excel"/>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <%--'* My Calendar Completed--%>
                                                                            <div class="col-lg-3" id="TrMyCalendar1" runat="server" style="display:none">
                                                                                <div class="small-box bg-aquaDashboard">
                                                                                    <div class="inner">
                                                                                        <h3 style="margin-right:100%">0</h3>
                                                                                        <p style="margin-right:10%">My Calender</p>
                                                                                    </div>
                                                                                    <div class="icon">
                                                                                        <i class="fa fa-calendar"></i>
                                                                                    </div>
                                                                                    <a href="#" class="small-box-footer" id="a2">More info <i class="fa fa-arrow-circle-right"></i></a>
                                                                                </div>
                                                                            </div>

                                                                            <div id="divCTMS" style="display:none">
                                                                                <%--'* 9th pane started [Visit Tracker] *'--%>
                                                                                <div class="col-lg-3" id="TrVisitTracker" runat="server" style="display:none">
                                                                                    <div class="small-box bg-aquaDashboard">
                                                                                        <div class="inner">
                                                                                            <h3 style="margin-right:100%">
                                                                                                <asp:Label ID="Label17" runat="server" Style="border-radius: 10%; padding-left: 2px; padding-right: 2px; margin-left: 0.5%;" />
                                                                                            </h3>
                                                                                            <p style="margin-right:10%">
                                                                                                Visit Tracker
                                                                                            </p>
                                                                                        </div>
                                                                                        <div class="icon">
                                                                                            <i class="fa fa-list-alt"></i>
                                                                                        </div>
                                                                                        <a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i></a>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="modal-content modal-lg" id="divVisitTracker" style="display:none;">
                                                                                    <div class="modal-header">
                                                                                        <img src="images/Sqclose.gif" class="ModalImage" onclick="ModalClose();" alt="" />
                                                                                        <h2 style="text-align:center;">Visit Tracker</h2>
                                                                                    </div>
                                                                                    <div class="modal-body modalheight">
                                                                                          <asp:UpdatePanel ID="upGvDeviation" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                                                                            <ContentTemplate>
                                                                                        <table>
                                                                                            <center>
                                                                                                <tr>
                                                                                                    <td class="Label" style="white-space: nowrap; text-align: right; width: 20%">Project Name/Project No* : </td>
                                                                                                    <td>
                                                                                                        <span style="font-weight: normal">
                                                                                                            <asp:TextBox ID="txtProjectForVisitTracker" runat="server" CssClass="textBox" onkeydown="return (event.keyCode!=13)" Width="70%" />
                                                                                                        </span>
                                                                                                        <asp:Button ID="btnSetForVisitTracker" runat="server" Style="display: none" Text=" Project" />
                                                                                                        <asp:HiddenField ID="HProjectIdForVisitTracker" runat="server" />
                                                                                                        <asp:HiddenField ID="HClientName" runat="server" />
                                                                                                        <asp:HiddenField ID="HProjectName" runat="server" />
                                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtenderadd" runat="server" BehaviorID="AutoCompleteExtenderadd" 
                                                                                                            CompletionListCssClass="autocomplete_list" CompletionListElementID="pnlVisitTrackerProjectlist" 
                                                                                                            CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" 
                                                                                                            CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" 
                                                                                                            OnClientItemSelected="OnSelectedForVisitTracker" OnClientShowing="ClientPopulatedForVisitTracker" 
                                                                                                            ServiceMethod="GetMyProjectCompletionListParentOnly" ServicePath="AutoComplete.asmx" 
                                                                                                            TargetControlID="txtProjectForVisitTracker" UseContextKey="True">
                                                                                                        </cc1:AutoCompleteExtender>
                                                                                                        <asp:Panel ID="pnlVisitTrackerProjectlist" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </center>
                                                                                        </table>
                                                                                                <asp:Label ID="lblNote" runat="server" Style="margin-right: 68%; color: red;"></asp:Label>
                                                                                                <p />
                                                                                                <asp:HiddenField ID="HfVisitNo" runat="server" />
                                                                                                <center>
                                                                                                    <asp:GridView ID="gvVisitTracker" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" SkinID="grdViewForDashboard">
                                                                                                        <Columns>
                                                                                                            <asp:TemplateField HeaderText="Project Name" HeaderStyle-Width="50px">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox runat="server" ID="txt" Enabled="false" CssClass="textBox" Width="70px" Text='<%# Eval("vProjectNo")%>'></asp:TextBox>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Visit No" HeaderStyle-Width="30px">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox runat="server" ID="txtvisitNo" Enabled="false" CssClass="textBox" Width="50px" Text='<%# "Visit " + Eval("iVisitNo").ToString() %>'></asp:TextBox>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Activity" HeaderStyle-Width="30px">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox runat="server" ID="txtactivity" Enabled="false" ToolTip='<%# Eval("vNodeDisplayName").ToString() %>' CssClass="textBox" Text='<%# Eval("vNodeDisplayName").ToString() %>'></asp:TextBox>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Actual Days" HeaderStyle-Width="30px">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox runat="server" ID="txtActualDays" CssClass="textBox" Enabled="false" Width="30px" Text='<%# Eval("iActualDays")%>'></asp:TextBox>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Deviation Negative" HeaderStyle-Width="30px">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox runat="server" ID="txtDeviationNegative" CssClass="textBox" Enabled="false" Width="30px" Text='<%# Eval("iDeviationNegative") %>'></asp:TextBox>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Deviation Positive" HeaderStyle-Width="30px">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox runat="server" ID="txtDeviationPositive" CssClass="textBox" Enabled="false" Width="30px" Text='<%# Eval("iDeviationPositive")%>'></asp:TextBox>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                        </Columns>
                                                                                                    </asp:GridView>
                                                                                                </center>
                                                                                            </ContentTemplate>
                                                                                            <Triggers>
                                                                                                <asp:AsyncPostBackTrigger  ControlID="btnSetForVisitTracker" EventName="Click" />
                                                                                            </Triggers>
                                                                                        </asp:UpdatePanel>
                                                                                    </div>
                                                                                    <div class="modal-footer">
                                                                                        <h3></h3>
                                                                                    </div>
                                                                                </div>

                                                                                <%--'* 10th pane started [Visit Scheduler] *'--%>
                                                                                <div class="col-lg-3" id="TrVisitScheduler" runat="server" style="display:none">
                                                                                    <div class="small-box bg-aquaDashboard">
                                                                                        <div class="inner">
                                                                                            <h3 style="margin-right:100%">
                                                                                                <asp:Label ID="Label18" runat="server" Style="border-radius: 10%; padding-left: 2px; padding-right: 2px; margin-left: 0.5%;" />
                                                                                            </h3>
                                                                                            <p style="margin-right:10%">Visit Scheduler</p>
                                                                                        </div>
                                                                                        <div class="icon">
                                                                                            <i class="fa fa-list-alt"></i>
                                                                                        </div>
                                                                                        <a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i></a>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="modal-content modal-lg" id="divVisitScheduler" style="display:none;">
                                                                                    <div class="modal-header">
                                                                                        <img src="images/Sqclose.gif" class="ModalImage" onclick="ModalClose();" alt="" />
                                                                                        <h2 style="text-align:center;">Visit Scheduler</h2>
                                                                                    </div>

                                                                                    <div class="modal-body modalheight" id="div5">
                                                                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                                                                            <ContentTemplate>
                                                                                        <table cellpadding="5" style="width: 90%;">
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td style="text-align: right; vertical-align: top; white-space: nowrap; width: 20%;"
                                                                                                        class="Label">Project Name/Project No* :
                                                                                                    </td>
                                                                                                    <td style="text-align: left;">
                                                                                                        <asp:TextBox ID="txtprojectForVisitScheduler" runat="server" CssClass="textBox" Width="70%" TabIndex="1">
                                                                                                        </asp:TextBox>
                                                                                                        <asp:Button Style="display: none" ID="btnSetProjectforVisitScheduler" runat="server" Text=" Project"></asp:Button>
                                                                                                        <asp:HiddenField ID="HProjectIdForVisitScheduler" runat="server"></asp:HiddenField>
                                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtenderForVisitScheduler" runat="server" UseContextKey="True"
                                                                                                            TargetControlID="txtprojectForVisitScheduler" ServicePath="AutoComplete.asmx" OnClientShowing="ClientPopulatedForVisitScheduler"
                                                                                                            OnClientItemSelected="OnSelectedForVisitScheduler" MinimumPrefixLength="1" CompletionListItemCssClass="autocomplete_listitem"
                                                                                                            CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListCssClass="autocomplete_list"
                                                                                                            BehaviorID="AutoCompleteExtenderForVisitScheduler" CompletionListElementID="pnlProjectListForVisitScheduler" ServiceMethod="GetMyProjectCompletionList">
                                                                                                        </cc1:AutoCompleteExtender>
                                                                                                        <asp:Panel ID="pnlProjectListForVisitScheduler" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>
                                                                                        <center>
                                                                                            <div runat="server" id="divSchdulerGrid" style="height: auto; width: auto; overflow-y: scroll; max-height: 250px;">
                                                                                                <asp:GridView ID="gvScheduler" runat="server" class="dvScheduler" Width="100%" ShowHeaderWhenEmpty="true" SkinID="grdViewForDashboard">
                                                                                                    <RowStyle BackColor="#cee3ed" Font-Names="Verdana" VerticalAlign="Middle" HorizontalAlign="Center" Font-Size="9pt" ForeColor="navy" />
                                                                                                    <EditRowStyle BackColor="#cee3ed" Font-Names="Verdana" Font-Size="9pt" VerticalAlign="Middle" />
                                                                                                    <HeaderStyle Font-Underline="False" BackColor="#1560a1" Font-Names="Verdana" VerticalAlign="Top" Font-Size="10pt" HorizontalAlign="Center" ForeColor="white" Font-Bold="True" />
                                                                                                    <FooterStyle BackColor="#1560a1" Font-Names="Verdana" Font-Size="X-Small" HorizontalAlign="Center" ForeColor="white" Font-Bold="True" />
                                                                                                    <AlternatingRowStyle BackColor="white" Font-Names="Verdana" HorizontalAlign="Center" Font-Size="9pt" ForeColor="navy" />
                                                                                                    <PagerStyle ForeColor="#ffa24a" Font-Underline="False" BackColor="white" Font-Bold="True" Font-Names="Verdana" HorizontalAlign="Center" Font-Size="X-Small" />
                                                                                                </asp:GridView>
                                                                                            </div>
                                                                                        </center>
                                                                                            </ContentTemplate>
                                                                                            <Triggers>
                                                                                                <asp:AsyncPostBackTrigger  ControlID="btnSetProjectforVisitScheduler" EventName="Click" />
                                                                                            </Triggers>
                                                                                        </asp:UpdatePanel>
                                                                                    </div>
                                                                                    <div class="modal-footer">
                                                                                        <h3></h3>
                                                                                    </div>
                                                                                </div>

                                                                                <%--'* 11th pane started [SiteWise Subject Detail] *'--%>
                                                                                <div class="col-lg-3" id="TrSiteWiseSubjectDetail" runat="server" style="display:none">
                                                                                    <div class="small-box bg-aquaDashboard">
                                                                                        <div class="inner">
                                                                                            <h3 style="margin-right:100%">
                                                                                                <asp:Label ID="Label20" runat="server" Style="border-radius: 10%; padding-left: 2px; padding-right: 2px; margin-left: 0.5%;" />
                                                                                            </h3>
                                                                                            <p style="margin-right:10%">
                                                                                                Site Wise Subject Detail
                                                                                            </p>
                                                                                        </div>
                                                                                        <div class="icon">
                                                                                            <i class="fa fa-list-alt"></i>
                                                                                        </div>
                                                                                        <a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i></a>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="modal-content modal-lg" id="divSiteWiseSubjectDetail" style="display:none;">
                                                                                    <div class="modal-header" id="divSiteWiseSubjectDetailHeader">
                                                                                        <img src="images/Sqclose.gif" class="ModalImage" onclick="ModalClose();" alt="" />
                                                                                        
                                                                                        <img src="images/plus-icon.png" class="ModalImage" id="divSiteWiseSubjectDetailmaxmin" height="28px" width="28px" alt="plusbtn" onclick="ModalExtend('divSiteWiseSubjectDetail');" 
                                                                                            style="cursor:pointer;margin-right:5px;"/>
                                                                                        <h2 style="text-align:center;">
                                                                                            Site Wise Subject Detail
                                                                                        </h2>
                                                                                    </div>
                                                                                    <div class="modal-body modalheight">
                                                                                        <table style="text-align: left; width: 100%">
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td style="width: 12%; text-align: left; padding-left: 15px; color: darkgoldenrod !important" class="Label">Project No :</td>
                                                                                                    <td style="text-align: left;">
                                                                                                        <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Style="width: 600px" />
                                                                                                        <asp:Button Style="display: none" ID="BtnSubjectinfo" OnClick="btnSubjectinfo_Click"
                                                                                                            runat="server" Text="Project" />
                                                                                                        <asp:HiddenField ID="ProjectWorkSpaceID" runat="server" />
                                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtenderforsubjectInfo" runat="server"
                                                                                                            BehaviorID="AutoCompleteExtenderforsubjectInfo" CompletionListCssClass="autocomplete_list"
                                                                                                            CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListItemCssClass="autocomplete_listitem"
                                                                                                            MinimumPrefixLength="1" OnClientItemSelected="OnSelectedForsubjectInfo" OnClientShowing="ClientPopulatedForsubjectInfo"
                                                                                                            ServiceMethod="GetMyProjectCompletionList" ServicePath="AutoComplete.asmx"
                                                                                                            TargetControlID="txtproject" UseContextKey="True" CompletionListElementID="pnlProjectListsubjectInfo">
                                                                                                        </cc1:AutoCompleteExtender>
                                                                                                        <asp:Panel ID="pnlProjectListsubjectInfo" runat="server" Style="max-height: 300px; overflow: auto; overflow-x: hidden;" />
                                                                                                    </td>
                                                                                                    <td style="text-align: left; padding-left: 01px;">
                                                                                                        <input type="button" id="btnSiteWiseSubjectDetailProjectDetail" name="Retrive" value="Retrive" onclick="return fnSiteWiseSubjectDetail();" class="btn btnnew" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="text-align: left; white-space: nowrap; width: 12%; padding-left: 20px;" class="Label">
                                                                                                        <asp:Label ID="lblselectchart" runat="server" Text=" Select Chart" Style="color: darkgoldenrod"></asp:Label>
                                                                                                    </td>
                                                                                                    <td style="text-align: left;">
                                                                                                        <asp:DropDownList ID="dd_chart" runat="server" onchange="chart_selection();" Style="color: purple" class="dropDownList">
                                                                                                            <asp:ListItem>Select Chart</asp:ListItem>
                                                                                                            <asp:ListItem Value="Pie">Bar Chart</asp:ListItem>
                                                                                                            <asp:ListItem Value="Line">Line Chart</asp:ListItem>
                                                                                                            <asp:ListItem Value="Doughnut">Doughnut Chart</asp:ListItem>
                                                                                                        </asp:DropDownList>
                                                                                                        <asp:DropDownList ID="dd_project" runat="server" onchange="fnSiteWiseSubjectDetail();" Width="500px" Style="display: none"></asp:DropDownList>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="text-align: left; white-space: nowrap; width: 20%; padding-left: 15px;" class="Label" colspan="2">
                                                                                                        <asp:Label ID="Study_No" runat="server" Text="Study No:" Style="color: darkgoldenrod"></asp:Label>
                                                                                                        <asp:Label ID="Study_No_Result" runat="server" Text="" Style="color: purple"></asp:Label>
                                                                                                        <asp:Label ID="Seperator1" runat="server" Text="||"></asp:Label>
                                                                                                        <asp:Label ID="Study_Type" runat="server" Text="Study Type:" Style="color: darkgoldenrod"></asp:Label>
                                                                                                        <asp:Label ID="Study_Type_Result" runat="server" Text="" Style="color: purple"></asp:Label>
                                                                                                        <asp:Label ID="Seperator2" runat="server" Text="||"></asp:Label>
                                                                                                        <asp:Label ID="Study_Status" runat="server" Text="Study Status:" Style="color: darkgoldenrod"></asp:Label>
                                                                                                        <asp:Label ID="Study_Status_Result" runat="server" Text="" Style="color: purple"></asp:Label>
                                                                                                        <asp:Label ID="Seperator3" runat="server" Text="||"></asp:Label>
                                                                                                        <asp:Label ID="study_completion_phase" runat="server" Text="Study Phase:" Style="color: darkgoldenrod"></asp:Label>
                                                                                                        <asp:Label ID="study_completion_phase_Result" runat="server" Text="" Style="color: purple"></asp:Label>
                                                                                                    </td>
                                                                                                    <td></td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>

                                                                                        <div id="container"></div>
                                                                                        <table id="tbl_projectdetail" class="display" cellspacing="0" width="100%">
                                                                                            <thead>
                                                                                                <tr>
                                                                                                    <th>Project/Stie</th>
                                                                                                    <th>Enrolled Subject</th>
                                                                                                    <th>Rejected Subject</th>
                                                                                                </tr>
                                                                                            </thead>
                                                                                            <tbody>
                                                                                            </tbody>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div class="modal-footer">
                                                                                        <h3></h3>
                                                                                    </div>
                                                                                </div>

                                                                                <%--'* 12th pane started [Site Information] *'--%>
                                                                                <div class="col-lg-3" id="TrSiteInformation" runat="server" style="display:none">
                                                                                    <div class="small-box bg-aquaDashboard">
                                                                                        <div class="inner">
                                                                                            <h3 style="margin-right:100%">
                                                                                                <asp:Label ID="Label21" runat="server" Style="border-radius: 10%; padding-left: 2px; padding-right: 2px; margin-left: 0.5%;" />
                                                                                            </h3>
                                                                                            <p style="margin-right:10%">
                                                                                                Site Information
                                                                                            </p>
                                                                                        </div>
                                                                                        <div class="icon">
                                                                                            <i class="fa fa-list-alt"></i>
                                                                                        </div>
                                                                                        <a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i></a>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="modal-content modal-xl" id="divSiteInformation" style="display:none;" >
                                                                                    <div class="modal-header" id="divSiteInformationHeader">
                                                                                        <img src="images/Sqclose.gif" class="ModalImage" onclick="ModalClose();" alt="" />
                                                                                        <img src="images/plus-icon.png" class="ModalImage" id="divSiteInformationmaxmin" height="28px" width="28px" alt="plusbtn" onclick="ModalExtend('divSiteInformation');" style="cursor:pointer;margin-right:5px;"/>
                                                                                        <h2 style="text-align:center;">Site Information</h2>
                                                                                    </div>
                                                                                    <div class="modal-body modalheightxl">
                                                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                                                        <ContentTemplate>
                                                                                        <table style="text-align: left; width: 100%">
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td style="width: 12%; text-align: left; padding-left: 15px; color: darkgoldenrod !important" class="Label">Project No :</td>
                                                                                                    <td style="text-align: left;">
                                                                                                        <asp:TextBox ID="txtSiteInformationProject" runat="server" CssClass="textBox" Style="width: 600px" />
                                                                                                        <asp:Button Style="display: none" ID="BtnSiteInf1" OnClick="btnSiteInf1_Click"
                                                                                                            runat="server" Text="Project" />
                                                                                                        <asp:HiddenField ID="hdnSiteInformationProjectWorkSpaceID" runat="server" />
                                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtenderforsiteInfo" runat="server"
                                                                                                            BehaviorID="AutoCompleteExtenderforsiteInfo" CompletionListCssClass="autocomplete_list"
                                                                                                            CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListItemCssClass="autocomplete_listitem"
                                                                                                            MinimumPrefixLength="1" OnClientItemSelected="OnSelectedForsiteInfo" OnClientShowing="ClientPopulatedForsiteInfo"
                                                                                                            ServiceMethod="GetMyProjectCompletionList" ServicePath="AutoComplete.asmx"
                                                                                                            TargetControlID="txtSiteInformationProject" UseContextKey="True" CompletionListElementID="pnlProjectListsiteInfo">
                                                                                                        </cc1:AutoCompleteExtender>
                                                                                                        <asp:Panel ID="pnlProjectListsiteInfo" runat="server" Style="max-height: 300px; overflow: auto; overflow-x: hidden;" />
                                                                                                    </td>

                                                                                                    <td style="text-align: left; padding-left: 01px;">
                                                                                                        <input type="button" id="btnSiteInformationProjectDetail" name="Retrive" value="Retrive" onclick="return fnSiteInformation();" class="btn btnnew" />
                                                                                                        <%--<asp:HiddenField ID="ProjectWorkSpaceIdDemo" runat="server" />--%>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="text-align: left; white-space: nowrap; width: 12%; padding-left: 15px;" class="Label"></td>
                                                                                                    <td></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="text-align: left; white-space: nowrap; width: 20%; padding-left: 15px;" class="Label" colspan="2"></td>
                                                                                                    <td></td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>

                                                                                        <div class="progress-pie-chart" id="vp" data-percent="0" style="margin-left: auto !important">
                                                                                            <div class="ppc-progress">
                                                                                                <div class="ppc-progress-fill"></div>
                                                                                            </div>
                                                                                            <div class="ppc-percents">
                                                                                                <div class="pcc-percents-wrapper">
                                                                                                    Project Subject Count
                                                                                                    <span></span>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                        <br />

                                                                                        <div class="box-body">
                                                                                        <div>
                                                                                            <table id="tbl_siteinfo" class="display" cellspacing="0" width="100%">
                                                                                                <thead>
                                                                                                    <tr>
                                                                                                        <th>Project/Site</th>
                                                                                                        <th>Investigator</th>
                                                                                                        <th>Total Subject</th>
                                                                                                        <th>Active Subject</th>
                                                                                                        <th>DeActive Subject</th>
                                                                                                        <th>Total Screening</th>
                                                                                                        <th>Successfull Screening</th>
                                                                                                        <th>Rejected Screening</th>
                                                                                                    </tr>
                                                                                                </thead>
                                                                                                <tbody>
                                                                                                </tbody>
                                                                                            </table>
                                                                                            <br />
                                                                                            <br />
                                                                                            <table id="tbl_subsiteinfo" class="display" cellspacing="0" width="100%">
                                                                                                <thead>
                                                                                                    <tr>
                                                                                                        <th>Project/Site</th>
                                                                                                        <th>Investigator</th>
                                                                                                        <th>Total Subject</th>
                                                                                                        <th>Active Subject</th>
                                                                                                        <th>DeActive Subject</th>
                                                                                                        <th>Total Screening</th>
                                                                                                        <th>Successfull Screening</th>
                                                                                                        <th>Rejected Screening</th>
                                                                                                    </tr>
                                                                                                </thead>
                                                                                                <tbody>
                                                                                                </tbody>
                                                                                            </table>
                                                                                        </div>
                                                                                    </div>
                                                                                            </ContentTemplate>
                                                                                        <Triggers>
                                                                                            <asp:AsyncPostBackTrigger  ControlID="BtnSiteInf1" EventName="Click" />
                                                                                        </Triggers>
                                                                                    </asp:UpdatePanel>

                                                                                    </div>
                                                                                    <%--<div class="modal-footer">
                                                                                        <h3>Modal Footer</h3>
                                                                                    </div>--%>
                                                                                </div>

                                                                                <%--'* 13th pane started [CRF Status] *'--%>
                                                                                <div class="col-lg-3" id="TrCRFStatus" runat="server" style="display:none">
                                                                                    <div class="small-box bg-aquaDashboard">
                                                                                        <div class="inner">
                                                                                            <h3 style="margin-right:100%">
                                                                                                <asp:Label ID="Label23" runat="server" Style="border-radius: 10%; padding-left: 2px; padding-right: 2px; margin-left: 0.5%;" />
                                                                                            </h3>
                                                                                            <p style="margin-right:10%">
                                                                                                CRF Status
                                                                                            </p>
                                                                                        </div>
                                                                                        <div class="icon">
                                                                                            <i class="fa fa-list-alt"></i>
                                                                                        </div>
                                                                                        <a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i></a>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="modal-content modal-lg" id="divCRFStatus" style="display:none;">
                                                                                    <div class="modal-header" id="divCRFStatusHeader">
                                                                                        <img src="images/Sqclose.gif" class="ModalImage" onclick="ModalClose();" alt="" />
                                                                                        <img src="images/plus-icon.png" class="ModalImage" id="divCRFStatusmaxmin" height="28px" width="28px" alt="plusbtn" onclick="ModalExtend('divCRFStatus');" style="cursor:pointer;margin-right:5px;"/>
                                                                                        <h2 style="text-align:center;">CRF Status</h2>
                                                                                    </div>
                                                                                    <div class="modal-body modalheight">
                                                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                                                        <ContentTemplate>

                                                                                        <table style="text-align: left; width: 100%">
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td style="width: 12%; text-align: left; padding-left: 15px; color: darkgoldenrod !important" class="Label">Project No :</td>
                                                                                                    <td style="text-align: left;">
                                                                                                        <asp:TextBox ID="txtCRFStatusProject" runat="server" CssClass="textBox" Style="width: 600px" />
                                                                                                        <asp:Button Style="display: none" ID="Btncrf1" OnClick="btncrf1_Click"
                                                                                                            runat="server" Text="Project" />
                                                                                                        <asp:HiddenField ID="hdnCRFStatusProjectWorkSpaceID" runat="server" />
                                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtenderforcrf1" runat="server"
                                                                                                            BehaviorID="AutoCompleteExtenderforcrf1" CompletionListCssClass="autocomplete_list"
                                                                                                            CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListItemCssClass="autocomplete_listitem"
                                                                                                            MinimumPrefixLength="1" OnClientItemSelected="OnSelectedForcrf1" OnClientShowing="ClientPopulatedForcrf1"
                                                                                                            ServiceMethod="GetMyProjectCompletionList" ServicePath="AutoComplete.asmx"
                                                                                                            TargetControlID="txtCRFStatusProject" UseContextKey="True" CompletionListElementID="pnlProjectListcrf1">
                                                                                                        </cc1:AutoCompleteExtender>
                                                                                                        <asp:Panel ID="pnlProjectListcrf1" runat="server" Style="max-height: 300px; overflow: auto; overflow-x: hidden;" />
                                                                                                    </td>
                                                                                                    <td style="text-align: left; padding-left: 01px;">
                                                                                                        <input type="button" id="btnCRFStatusProjectDetail" name="Retrive" value="Retrive" onclick="return fnCRFStatus();" class="btn btnnew" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="text-align: left; white-space: nowrap; width: 12%; padding-left: 15px;" class="Label"></td>
                                                                                                    <td></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="text-align: left; white-space: nowrap; width: 20%; padding-left: 15px;" class="Label" colspan="2"></td>
                                                                                                    <td></td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>
                                                                                        <br />
                                                                                        <div>
                                                                                            <table id="tbl_CRFStatus" class="display" cellspacing="0" width="100%">
                                                                                                <thead>
                                                                                                    <tr>
                                                                                                        <th>Project/Site Id.</th>
                                                                                                        <th>No. Of Subject</th>
                                                                                                        <th>Data Entry Pending</th>
                                                                                                        <th>Data Entry Continue</th>
                                                                                                        <th>Ready For Review</th>
                                                                                                        <th>First Review Done</th>
                                                                                                        <th>Second Review Done</th>
                                                                                                        <th>Final Reviewed & Freeze</th>
                                                                                                        <th>Generated DCF</th>
                                                                                                        <th>Answered DCF</th>
                                                                                                        <th>Total DCF</th>
                                                                                                    </tr>
                                                                                                </thead>
                                                                                                <tbody>
                                                                                                </tbody>
                                                                                            </table>
                                                                                        </div>
                                                                                        </ContentTemplate>
                                                                                        <Triggers>
                                                                                            <asp:AsyncPostBackTrigger  ControlID="Btncrf1" EventName="Click" />
                                                                                        </Triggers>
                                                                                    </asp:UpdatePanel>
                                                                                    </div>
                                                                                    <div class="modal-footer"><h3></h3></div>
                                                                                </div>

                                                                                <%--'* 14th pane started [AE SAE] *'--%>
                                                                                <div class="col-lg-3" id="TrAESAE" runat="server" style="display:none">
                                                                                    <div class="small-box bg-aquaDashboard">
                                                                                        <div class="inner">
                                                                                            <h3 style="margin-right:100%">
                                                                                                <asp:Label ID="Label24" runat="server" Style="border-radius: 10%; padding-left: 2px; padding-right: 2px; margin-left: 0.5%;" />
                                                                                            </h3>
                                                                                            <p style="margin-right:10%">
                                                                                                AE SAE
                                                                                            </p>
                                                                                        </div>
                                                                                        <div class="icon">
                                                                                            <i class="fa fa-list-alt"></i>
                                                                                        </div>
                                                                                        <a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i></a>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="modal-content modal-lg" id="divAESAE" style="display:none;">
                                                                                    <div class="modal-header" id="divAESAEHeader">
                                                                                        <img src="images/Sqclose.gif" class="ModalImage" onclick="ModalClose();" alt="" />
                                                                                        <img src="images/plus-icon.png" class="ModalImage" id="divAESAEmaxmin" height="28px" width="28px" alt="plusbtn" onclick="ModalExtend('divAESAE');" style="cursor:pointer;margin-right:5px;"/>
                                                                                        <h2 style="text-align:center;">AE SAE</h2>
                                                                                    </div>
                                                                                    <div class="modal-body">
                                                                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                                                        <ContentTemplate>

                                                                                        <table style="text-align: left; width: 100%">
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td style="width: 12%; text-align: left; padding-left:15px; color: darkgoldenrod !important" class="Label">Project No :</td>
                                                                                                    <td style="text-align: left;">
                                                                                                        <asp:TextBox ID="txtAESAEProject" runat="server" CssClass="textBox" Style="width: 600px" />
                                                                                                        <asp:Button Style="display: none" ID="BtnAESE" OnClick="btnAESE_Click" runat="server" Text="Project" />
                                                                                                        <asp:HiddenField ID="hdnAESAEProjectWorkSpaceID" runat="server" />
                                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtenderforaese" runat="server"
                                                                                                            BehaviorID="AutoCompleteExtenderforaese" CompletionListCssClass="autocomplete_list"
                                                                                                            CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListItemCssClass="autocomplete_listitem"
                                                                                                            MinimumPrefixLength="1" OnClientItemSelected="OnSelectedForaese" OnClientShowing="ClientPopulatedForaese"
                                                                                                            ServiceMethod="GetMyProjectCompletionList" ServicePath="AutoComplete.asmx"
                                                                                                            TargetControlID="txtAESAEProject" UseContextKey="True" CompletionListElementID="pnlProjectListaese">
                                                                                                        </cc1:AutoCompleteExtender>
                                                                                                        <asp:Panel ID="pnlProjectListaese" runat="server" Style="max-height: 300px; overflow: auto; overflow-x: hidden;" />
                                                                                                    </td>
                                                                                                    <td style="text-align: left; padding-left: 01px;">
                                                                                                        <input type="button" id="btnAESAEProjectDetail" name="Retrive" value="Retrive"  onclick="return fnAESAE();"  class="btn btnnew" />                                    
                                                                                                    </td>
                                                                                                </tr> 
                                                                                                <tr>
                                                                                                    <td style="text-align: left;  white-space: nowrap; width: 12%; padding-left: 15px;"  class="Label"></td>
                                                                                                    <td></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="text-align: left;  white-space: nowrap; width: 20%; padding-left: 15px;"  class="Label" colspan="2"> </td>
                                                                                                    <td> </td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>
                                                                                        <br />
                                                                                        <div>
                                                                                            <table id="tbl_AESAE" class="display" cellspacing="0" width="100%">
                                                                                                <thead>
                                                                                                    <tr>
                                                                                                        <th>Project/Site</th>
                                                                                                        <th>AE</th>
                                                                                                        <th>SAE</th>
                                                                                                    </tr>
                                                                                                </thead>
                                                                                                <tbody>
                                                                                                </tbody>
                                                                                            </table>
                                                                                        </div>

                                                                                        </ContentTemplate>
                                                                                        <Triggers>
                                                                                            <asp:AsyncPostBackTrigger  ControlID="BtnAESE" EventName="Click" />
                                                                                        </Triggers>
                                                                                    </asp:UpdatePanel>
                                                                                    </div>
                                                                                    <%--<div class="modal-footer">
                                                                                        <h3>Modal Footer</h3>
                                                                                    </div>--%>
                                                                                </div>

                                                                                <%--'* 15th pane started [Demo] *'--%>
                                                                                <div class="col-lg-3" id="TrDemo" runat="server" style="display:none">
                                                                                    <div class="small-box bg-aquaDashboard">
                                                                                        <div class="inner">
                                                                                            <h3 style="margin-right:100%">
                                                                                                <asp:Label ID="Label25" runat="server" Style="border-radius: 10%; padding-left: 2px; padding-right: 2px; margin-left: 0.5%;" />
                                                                                            </h3>
                                                                                            <p style="margin-right:10%">
                                                                                                Data Status
                                                                                            </p>
                                                                                        </div>
                                                                                        <div class="icon">
                                                                                            <i class="fa fa-list-alt"></i>
                                                                                        </div>
                                                                                        <a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i></a>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="modal-content modal-xl" id="divDemo" style="display:none;">
                                                                                    <div class="modal-header" id="divDemoHeader">
                                                                                        <img src="images/Sqclose.gif" class="ModalImage" onclick="ModalClose();" alt="" />
                                                                                        <img src="images/plus-icon.png" class="ModalImage" id="divDemomaxmin" height="28px" width="28px" alt="plusbtn" onclick="ModalExtend('divDemo');" style="cursor:pointer;margin-right:5px;"/>
                                                                                        <h2 style="text-align:center;">Data Status</h2>
                                                                                    </div>
                                                                                    <div class="modal-body modalheightxl">
                                                                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                                                        <ContentTemplate>

                                                                                        <table style="text-align: left; width: 100%">
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td style="width: 12%; text-align: left; padding-left: 15px; color: darkgoldenrod !important" class="Label">Project No :</td>
                                                                                                    <td style="text-align: left;">
                                                                                                        <asp:TextBox ID="txtDemo" runat="server" CssClass="textBox" Style="width: 600px" />
                                                                                                        <asp:Button Style="display: none" ID="BtnCRFDataStatus" OnClick="btnCRFDataStatus_Click" 
                                                                                                            runat="server" Text="Project" />
                                                                                                        <asp:HiddenField ID="ProjectWorkSpaceIdDemo" runat="server" />
                                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtenderForCRFDataStatus" runat="server"
                                                                                                            BehaviorID="AutoCompleteExtenderForCRFDataStatus" CompletionListCssClass="autocomplete_list"
                                                                                                            CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListItemCssClass="autocomplete_listitem"
                                                                                                            MinimumPrefixLength="1" OnClientItemSelected="OnSelectedForCRFDataStatus" OnClientShowing="ClientPopulatedForCRFDataStatus"
                                                                                                            ServiceMethod="GetMyProjectCompletionList" ServicePath="AutoComplete.asmx"
                                                                                                            TargetControlID="txtDemo" UseContextKey="True" CompletionListElementID="pnlProjectListCRFDataDataStatus">
                                                                                                        </cc1:AutoCompleteExtender>
                                                                                                        <asp:Panel ID="pnlProjectListCRFDataDataStatus" runat="server" Style="max-height: 300px; overflow: auto; overflow-x: hidden;" />
                                                                                                    </td>
                                                                                                    <td style="text-align: left; padding-left: 01px; padding-top:14px;">
                                                                                                        <input type="button" id="btnProjectSummeryReport" name="Retrive" value="Retrive" onclick="return fnNewDemo();" class="btn btnnew" />
                                                                                                        <input type="checkbox" id="chkCRFDataStatusParentProject"  title="Include Parent Project" runat="server" />Include Parent 
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="text-align: left; white-space: nowrap; width: 12%; padding-left: 20px;" class="Label">
                                                                                                        <asp:Label ID="Label4" runat="server" Text=" Select Type" Style="color: darkgoldenrod"></asp:Label>
                                                                                                    </td>
                                                                                                    <td style="text-align: left;">
                                                                                                        <asp:DropDownList ID="ddlColumnList" runat="server" onchange="getSelectedColumnList(this);" Style="color: purple" class="dropDownList">
                                                                                                            <asp:ListItem Value="Select Type">Select Type</asp:ListItem>
                                                                                                            <asp:ListItem Value="All">All</asp:ListItem>
                                                                                                            <asp:ListItem Value="Data Entry Pending">Data Entry Pending</asp:ListItem>
                                                                                                            <asp:ListItem Value="SDV Pending">SDV Pending</asp:ListItem>
                                                                                                            <asp:ListItem Value="DM Review Pending">DM Reviewed Pending</asp:ListItem>
                                                                                                            <asp:ListItem Value="DM Review Done">DM Reviewed Done</asp:ListItem>
                                                                                                            <%--<asp:ListItem Value="System Genrated DCF">System Genrated DCF</asp:ListItem>--%>
                                                                                                        </asp:DropDownList>
                                                                                                        <asp:Label ID="Label3" runat="server" Text=" Select Chart" Style="color: darkgoldenrod" class="Label"></asp:Label>
                                                                                                        <asp:DropDownList ID="dd1_chart" runat="server" onchange="chartN_selection(this);" Style="color: purple" class="dropDownList">
                                                                                                            <asp:ListItem Value="-1">Select Chart</asp:ListItem>
                                                                                                            <asp:ListItem Value="Pie">Bar Chart</asp:ListItem>
                                                                                                            <asp:ListItem Value="Line">Line Chart</asp:ListItem>
                                                                                                            <asp:ListItem Value="Doughnut">Doughnut Chart</asp:ListItem>
                                                                                                        </asp:DropDownList>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="text-align: left; white-space: nowrap; width: 20%; padding-left: 15px;" class="Label" colspan="2">
                                                                                                        <asp:Label ID="Studynodemo" runat="server" Text="Study No:" Style="color: darkgoldenrod"></asp:Label>
                                                                                                        <asp:Label ID="Studynodemoresult" runat="server" Text="" Style="color: purple"></asp:Label>
                                                                                                        <asp:Label ID="Label7" runat="server" Text="||"></asp:Label>
                                                                                                        <asp:Label ID="Studytypedemo" runat="server" Text="Study Type:" Style="color: darkgoldenrod"></asp:Label>
                                                                                                        <asp:Label ID="Studytypedemoresult" runat="server" Text="" Style="color: purple"></asp:Label>
                                                                                                        <asp:Label ID="Label10" runat="server" Text="||"></asp:Label>
                                                                                                        <asp:Label ID="Studystatusdemo" runat="server" Text="Study Status:" Style="color: darkgoldenrod"></asp:Label>
                                                                                                        <asp:Label ID="Studystatusdemoresult" runat="server" Text="" Style="color: purple"></asp:Label>
                                                                                                        <asp:Label ID="Label13" runat="server" Text="||"></asp:Label>
                                                                                                        <asp:Label ID="Studyphasedemo" runat="server" Text="Study Phase:" Style="color: darkgoldenrod"></asp:Label>
                                                                                                        <asp:Label ID="Studyphasedemoresult" runat="server" Text="" Style="color: purple"></asp:Label>
                                                                                                    </td>
                                                                                                    <td></td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>

                                                                                        <div id="containermain">
                                                                                            <div id="container1"></div>
                                                                                            <div id="container2"></div>
                                                                                            <div id="container3"></div>
                                                                                            <div id="container4"></div>
                                                                                            <div id="container5"></div>
                                                                                        </div>

                                                                                        <table id="demotab" class="display" cellspacing="0" width="100%">
                                                                                            <thead>
                                                                                                <tr>
                                                                                                    <th>Project/Site Id.</th>
                                                                                                    <th>No. Of Subject</th>
                                                                                                    <th>Data Entry Pending</th>
                                                                                                    <th>Data Entry Continue</th>
                                                                                                    <th>Ready For Review</th>
                                                                                                    <th>First Review Done</th>
                                                                                                    <th>Second Review Done</th>
                                                                                                    <th>Final Reviewed & Freeze</th>
                                                                                                    <th>Generated DCF</th>
                                                                                                    <th>Answered DCF</th>
                                                                                                    <th>Total DCF</th>
                                                                                                </tr>
                                                                                            </thead>
                                                                                            <tbody>
                                                                                            </tbody>
                                                                                        </table>

                                                                                    </ContentTemplate>
                                                                                        <Triggers>
                                                                                            <asp:AsyncPostBackTrigger  ControlID="BtnCRFDataStatus" EventName="Click" />
                                                                                            <%--<asp:AsyncPostBackTrigger  ControlID="btnExportDataStatus" EventName="Click" />--%>
                                                                                        </Triggers>
                                                                                    </asp:UpdatePanel>
                                                                                    </div>
                                                                                    <div class="modal-footer">
                                                                                        <asp:Button ID="btnExportDataStatus" runat="server" CssClass="btn btnexcel" Style="display:none" Text="" Tooltip ="Export To Excel"/>
                                                                                    </div>
                                                                                </div>

                                                                                <%--'* 16th pane started [Demo 1] *'--%>
                                                                                <div class="col-lg-3" id="TrDemo1" runat="server" style="display:none">
                                                                                    <div class="small-box bg-aquaDashboard">
                                                                                        <div class="inner">
                                                                                            <h3 style="margin-right:100%">
                                                                                                <asp:Label ID="Label26" runat="server" Style="border-radius: 10%; padding-left: 2px; padding-right: 2px; margin-left: 0.5%;" />
                                                                                            </h3>
                                                                                            <p style="margin-right:10%">
                                                                                                Sitewise Data Status
                                                                                            </p>
                                                                                        </div>
                                                                                        <div class="icon">
                                                                                            <i class="fa fa-list-alt"></i>
                                                                                        </div>
                                                                                        <a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i></a>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="modal-content modal-xl" id="divDemo1" style="display:none;">
                                                                                    <div class="modal-header" id="divDemo1Header">
                                                                                        <img src="images/Sqclose.gif" class="ModalImage" onclick="ModalClose();" alt="" />
                                                                                        <img src="images/plus-icon.png" class="ModalImage" id="divDemo1maxmin" height="28px" width="28px" alt="plusbtn" onclick="ModalExtend('divDemo1');" style="cursor:pointer;margin-right:5px;"/>
                                                                                        <h2 style="text-align:center;">Sitewise Data Status</h2>
                                                                                    </div>
                                                                                    <div class="modal-body modalheightxl">
                                                                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                                                        <ContentTemplate>

                                                                                        <table style="text-align: left; width: 100%">
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td style="width: 12%; text-align: left; padding-left: 15px; color: darkgoldenrod !important" class="Label">Project No :</td>
                                                                                                    <td style="text-align: left;">
                                                                                                        <asp:TextBox ID="txtDemo1" runat="server" CssClass="textBox" Style="width: 537px" />
                                                                                                        <asp:Button Style="display: none" ID="BtnSiteDataStatus" OnClick="btnSiteDataStatus_Click"
                                                                                                            runat="server" Text="Project" />
                                                                                                        <asp:HiddenField ID="ProjectWorkSpaceIdDemo1" runat="server" />
                                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtenderforsiteData" runat="server"
                                                                                                            BehaviorID="AutoCompleteExtenderforsiteData" CompletionListCssClass="autocomplete_list"
                                                                                                            CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListItemCssClass="autocomplete_listitem"
                                                                                                            MinimumPrefixLength="1" OnClientItemSelected="OnSelectedForsiteData" OnClientShowing="ClientPopulatedForsiteData"
                                                                                                            ServiceMethod="GetMyProjectCompletionList" ServicePath="AutoComplete.asmx"
                                                                                                            TargetControlID="txtDemo1" UseContextKey="True" CompletionListElementID="pnlProjectListsiteData">
                                                                                                        </cc1:AutoCompleteExtender>
                                                                                                        <asp:Panel ID="pnlProjectListsiteData" runat="server" Style="max-height: 300px; overflow: auto; overflow-x: hidden;" />
                                                                                                    </td>
                                                                                                    <td style="text-align: left; padding-left: 01px;">
                                                                                                        <input type="button" id="btnProjectSummeryReport1" name="Retrive" value="Retrive" onclick="return fnNewDemo1();" class="btn btnnew" />
                                                                                                        <input type="checkbox" id="chkSiteWiseParentProject" runat="server"/>Include Parent
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="text-align: left; white-space: nowrap; width: 12%; padding-left: 15px;" class="Label">
                                                                                                        <asp:Label ID="Label6" runat="server" Text=" Select Type" Style="color: darkgoldenrod"></asp:Label>
                                                                                                    </td>
                                                                                                    <td style="text-align: left;">
                                                                                                        <asp:DropDownList ID="ddlColumnList1" runat="server" onchange="getSelectedColumnList1(this);" Style="color: purple" class="dropDownList">
                                                                                                            <asp:ListItem Value="Select Type">Select Type</asp:ListItem>
                                                                                                            <asp:ListItem Value="All">All</asp:ListItem>
                                                                                                            <asp:ListItem Value="Total Expected Data">Total Expected Data</asp:ListItem>
                                                                                                            <asp:ListItem Value="Available Data">Available Data</asp:ListItem>
                                                                                                            <asp:ListItem Value="SDV Data">SDV Data</asp:ListItem>
                                                                                                            <asp:ListItem Value="DM Reviwed">DM Reviewed</asp:ListItem>

                                                                                                        </asp:DropDownList>
                                                                                                        <asp:Label ID="Label8" runat="server" Text=" Select Chart" Style="color: darkgoldenrod" class="Label"></asp:Label>
                                                                                                        <asp:DropDownList ID="dd2_chart" runat="server" onchange="chartN1_selection(this);" Style="color: purple" class="dropDownList">
                                                                                                            <asp:ListItem Value="-1">Select Chart</asp:ListItem>
                                                                                                            <asp:ListItem Value="Pie">Bar Chart</asp:ListItem>
                                                                                                            <asp:ListItem Value="Line">Line Chart</asp:ListItem>
                                                                                                            <asp:ListItem Value="Doughnut">Doughnut Chart</asp:ListItem>
                                                                                                        </asp:DropDownList>

                                                                                                        <asp:Label ID="Label9" runat="server" Text=" Select Site" Style="color: darkgoldenrod" class="Label"></asp:Label>
                                                                                                        <asp:DropDownList ID="ddlsite" runat="server" onchange="getSelectedSite(this);" Style="color: purple" class="dropDownList">
                                                                                                        </asp:DropDownList>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="text-align: left; white-space: nowrap; width: 20%; padding-left: 15px;" class="Label" colspan="2">
                                                                                                        <asp:Label ID="Studynodemo1" runat="server" Text="Study No:" Style="color: darkgoldenrod"></asp:Label>
                                                                                                        <asp:Label ID="Studynodemoresult1" runat="server" Text="" Style="color: purple"></asp:Label>
                                                                                                        <asp:Label ID="Label12" runat="server" Text="||"></asp:Label>
                                                                                                        <asp:Label ID="Studytypedemo1" runat="server" Text="Study Type:" Style="color: darkgoldenrod"></asp:Label>
                                                                                                        <asp:Label ID="Studytypedemoresult1" runat="server" Text="" Style="color: purple"></asp:Label>
                                                                                                        <asp:Label ID="Label16" runat="server" Text="||"></asp:Label>
                                                                                                        <asp:Label ID="Studystatusdemo1" runat="server" Text="Study Status:" Style="color: darkgoldenrod"></asp:Label>
                                                                                                        <asp:Label ID="Studystatusdemoresult1" runat="server" Text="" Style="color: purple"></asp:Label>
                                                                                                        <asp:Label ID="Label19" runat="server" Text="||"></asp:Label>
                                                                                                        <asp:Label ID="Studyphasedemo1" runat="server" Text="Study Phase:" Style="color: darkgoldenrod"></asp:Label>
                                                                                                        <asp:Label ID="Studyphasedemoresult1" runat="server" Text="" Style="color: purple"></asp:Label>

                                                                                                    </td>
                                                                                                    <td></td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>

                                                                                        <div id="containermain1">
                                                                                            <div id="Div40">
                                                                                            </div>
                                                                                            <div id="Div41">
                                                                                            </div>
                                                                                            <div id="Div42">
                                                                                            </div>
                                                                                            <div id="Div43">
                                                                                            </div>
                                                                                            <div id="Div44">
                                                                                            </div>
                                                                                        </div>

                                                                                        <table id="demotab1" class="display" cellspacing="0" width="100%">
                                                                                            <thead>
                                                                                                <tr>
                                                                                                    <th>Project/Site Id.</th>
                                                                                                    <th>Total Expected Data</th>
                                                                                                    <th>Available Data</th>
                                                                                                    <th>SDV Data</th>
                                                                                                    <th>DM Reviwed </th>
                                                                                                </tr>
                                                                                            </thead>
                                                                                            <tbody>
                                                                                            </tbody>
                                                                                        </table>

                                                                                        </ContentTemplate>
                                                                                        <Triggers>
                                                                                            <asp:AsyncPostBackTrigger  ControlID="BtnSiteDataStatus" EventName="Click" />
                                                                                            <asp:PostBackTrigger ControlID="btnExportSiteWiseData" />
                                                                                        </Triggers>
                                                                                    </asp:UpdatePanel>
                                                                                    </div>
                                                                                    <div class="modal-footer">
                                                                                        <asp:Button ID="btnExportSiteWiseData" runat="server" CssClass="btn btnexcel" Style="display:none" Text="" Tooltip="Export To Excel" />
                                                                                    </div>
                                                                                </div>

                                                                                <%--'* 17th pane started [DCF Manage] *'--%>
                                                                                <div class="col-lg-3" id="TrDCFManage" runat="server" style="display:block">
                                                                                    <div class="small-box bg-aquaDashboard">
                                                                                        <div class="inner">
                                                                                            <h3 style="margin-right:100%">
                                                                                                <asp:Label ID="Label27" runat="server" Style="border-radius: 10%; padding-left: 2px; padding-right: 2px; margin-left: 0.5%;" />
                                                                                            </h3>
                                                                                            <p style="margin-right:10%">
                                                                                                Query Management
                                                                                            </p>
                                                                                        </div>
                                                                                        <div class="icon">
                                                                                            <i class="fa fa-bar-chart"></i>
                                                                                        </div>
                                                                                        <a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i></a>
                                                                                    </div>
                                                                                </div>
                                                                                
                                                                                <div class="modal-content modal-xl" id="divDcfmanage" style="display:none;">
                                                                                    <div class="modal-header" id="divDcfmanageHeader">
                                                                                        <img src="images/Sqclose.gif" class="ModalImage" onclick="ModalClose();" alt="" />
                                                                                        <img src="images/plus-icon.png" class="ModalImage" id="divDcfmanagemaxmin" height="28px" width="28px" alt="plusbtn" onclick="ModalExtend('divDcfmanage');" style="cursor:pointer;margin-right:5px;"/>
                                                                                        <h2 style="text-align:center;">Query Management</h2>
                                                                                    </div>
                                                                                    <div class="modal-body modalheightxl">
                                                                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                                                        <ContentTemplate>

                                                                                        <table style="text-align: left; width: 100%">
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td style="width: 12%; text-align: left; padding-left: 15px; color: darkgoldenrod !important" class="Label">Project No :</td>
                                                                                                    <td style="text-align: left;">
                                                                                                        <asp:TextBox ID="txtDcfmanage" runat="server" CssClass="textBox" Style="width: 600px" />
                                                                                                        <asp:Button Style="display: none" ID="BtnDCFManage" OnClick="btndcfmanage_Click"
                                                                                                            runat="server" Text="Project" />
                                                                                                        <asp:HiddenField ID="ProjectWorkSpaceIdDCFManage" runat="server" />
                                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtenderforDCFManage" runat="server"
                                                                                                            BehaviorID="AutoCompleteExtenderforDCFManage" CompletionListCssClass="autocomplete_list"
                                                                                                            CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListItemCssClass="autocomplete_listitem"
                                                                                                            MinimumPrefixLength="1" OnClientItemSelected="OnSelectedForDCFmanage" OnClientShowing="ClientPopulatedForDCFmanage"
                                                                                                            ServiceMethod="GetMyProjectCompletionList" ServicePath="AutoComplete.asmx"
                                                                                                            TargetControlID="txtDcfmanage" UseContextKey="True" CompletionListElementID="pnlProjectListdcfmanage">
                                                                                                        </cc1:AutoCompleteExtender>
                                                                                                        <asp:Panel ID="pnlProjectListdcfmanage" runat="server" Style="max-height: 300px; overflow: auto; overflow-x: hidden;" />
                                                                                                    </td>
                                                                                                    <td style="text-align: left; padding-left: 01px;">
                                                                                                        <input type="button" id="Dcfmanagereport" name="Retrive" value="Retrive" onclick="return fnDCFManage();" class="btn btnnew" />
                                                                                                    </td>
                                                                                                </tr>

                                                                                                <tr>
                                                                                                    <td style="text-align: left; white-space: nowrap; width: 12%; padding-left: 15px;" class="Label">
                                                                                                        <asp:Label ID="Label5" runat="server" Text=" Select Type" Style="color: darkgoldenrod"></asp:Label>
                                                                                                    </td>
                                                                                                    <td style="text-align: left;">
                                                                                                        <asp:DropDownList ID="ddlColumnList3" runat="server" onchange="getSelectedManage(this);" Style="color: purple" class="dropDownList">
                                                                                                            <asp:ListItem Value="Select Type">Select Type</asp:ListItem>
                                                                                                            <asp:ListItem Value="DCF STATUS">Query Status</asp:ListItem>
                                                                                                            <asp:ListItem Value="TYPES OF DCF">Types of Query</asp:ListItem>
                                                                                                            <asp:ListItem Value="ACTIVITY WISE">Activity wise</asp:ListItem>
                                                                                                            <asp:ListItem Value="System Generated">System Generated Query By Site</asp:ListItem>
                                                                                                            <asp:ListItem Value="Manually Generated">Manually Generated Query By Site</asp:ListItem>
                                                                                                            <asp:ListItem Value="FULL CHART">Days Taken To Clear Queries</asp:ListItem>
                                                                                                            <asp:ListItem Value="Query By Site">No. of Queries by Site</asp:ListItem>
                                                                                                            <asp:ListItem Value="Query Per Subject">Average No. of Queries Per Subject</asp:ListItem>
                                                                                                        </asp:DropDownList>
                                                                                                        <asp:Label ID="Label11" runat="server" Text=" Select Chart" Style="color: darkgoldenrod" class="Label"></asp:Label>
                                                                                                        <asp:DropDownList ID="dd3_chart" runat="server" onchange="chartN2_selection();" Style="color: purple" class="dropDownList">
                                                                                                            <asp:ListItem Value="-1">Select Chart</asp:ListItem>
                                                                                                            <asp:ListItem Value="Pie">Bar Chart</asp:ListItem>
                                                                                                            <asp:ListItem Value="Line">Line Chart</asp:ListItem>
                                                                                                            <asp:ListItem Value="Doughnut">Doughnut Chart</asp:ListItem>
                                                                                                        </asp:DropDownList>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="text-align: left; white-space: nowrap; width: 20%; padding-left: 15px;" class="Label" colspan="2"></td>
                                                                                                    <td></td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>

                                                                                        <div id="containermain2">
                                                                                        </div>

                                                                                        <div id="createtable">
                                                                                        </div>

                                                                                         </ContentTemplate>
                                                                                        <Triggers>
                                                                                            <asp:AsyncPostBackTrigger  ControlID="BtnDCFManage" EventName="Click" />
                                                                                            <asp:PostBackTrigger ControlID="btnExportQueryMgt" />
                                                                                        </Triggers>
                                                                                    </asp:UpdatePanel>
                                                                                    </div>
                                                                                    <div class="modal-footer">
                                                                                        <asp:Button ID="btnExportQueryMgt" runat="server" CssClass="btn btnexcel" Style="display:none" Text="" Tooltip="Export To Excel" />
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="btnMediator" />
                                                                        <asp:AsyncPostBackTrigger ControlID="btnSetProjectforTracking" EventName="Click" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                            </table>
                                            </div>

                                                <%--<div class="container">
                                                    <div class="panel panel-primary" id="dvQuerydetailsreview" runat="server">
					                                                <div class="panel-heading">
						                                                <h3 class="panel-title">Query Details</h3>
					                                                </div>
					                                                <div class="panel-body">
						                                                 <div class="col-12">  
                                                                              <asp:UpdatePanel ID="UpdatePanel133" runat="server">
                                                                                  <ContentTemplate>
                                                                             <table id="Table1" cellpadding="0" cellspacing="0" style="width:100%"; >
										                                        <tbody>
											                                        <tr>
                                                                                        <td style="text-align: left; height:45px; width: 40%;">
                                                                                        <label class="LabelText">Screening No* :</label>
                                                                                        <asp:TextBox ID="txtScreeningForDI12" runat="server" CssClass="textBox" Width="40%" TabIndex="1" AutoPostBack="false"></asp:TextBox>
                                                                                        <asp:Button Style="display: none" ID="btnScreeningNo12" runat="server" txt="data" />
                                                                                        <asp:HiddenField ID="HScreeningNo12" runat="server"></asp:HiddenField>
                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender12" runat="server" BehaviorID="AutoCompleteExtender12"
                                                                                            CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                                                            CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelectedForQuery"
                                                                                            OnClientShowing="ClientPopulatedForQuery" ServiceMethod="GetMyScreeingNoCompletionList"
                                                                                            ServicePath="AutoComplete.asmx" TargetControlID="txtScreeningForDI12" UseContextKey="false" 
                                                                                            CompletionListElementID="pnlScreeningListForDI12">
                                                                                        </cc1:AutoCompleteExtender>
                                                                                            
                                                                                        <asp:Panel ID="Panel2" runat="server" Style="max-height: 300px; overflow: auto; overflow-x: hidden;" />
                                                                                       </td>
                                                                                            <td id="td3" runat="server" class="Label" nowrap="nowrap" style="text-align: left;  width: 20%;">
                                                                                            <strong class="Label" style="display:inline-block;">Select Status *:</strong>
                                                                                            <asp:DropDownList ID="ddlQueryStatus" runat="server" AutoPostBack="true"   Width="170px" >
                                                                                                <asp:ListItem Value="All">All</asp:ListItem>                                                                                            
                                                                                                <asp:ListItem Value="ImageUploader">Image Uploader</asp:ListItem>
                                                                                                <asp:ListItem Value="QC1">QC1</asp:ListItem>
                                                                                                <asp:ListItem Value="QC2">QC2</asp:ListItem>
                                                                                                <asp:ListItem Value="CA">CA1</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                         <td style="text-align: left;  width: 20%;" >
                                                                                             <asp:Button runat="server" ID="btnGoQuery" Text="Go" ToolTip="Go" CssClass="btn btngo" 
                                                                                                   style=" margin-left: 164px;padding:0px;" AutoPostBack="false"/>
                                                                                        </td>
                                                                                    </tr>
                                                                                    </tbody>
                                                                                 </table>
                                                                                <div class="datatable_filedetailVisit" style="width: 100%; height: auto; overflow: auto;">
                                                                                    <asp:GridView ID="gvQueryDetails" runat="server"  Style="display: none; width: 100%; margin: auto;" AutoGenerateColumns="false"  
                                                                                        DataKeyNames="vWorkspaceId,vActivityId,iNodeId,vSubjectId,vMySubjectNo,QCUserCode,CAUserCode,QC2UserCode"
                                                                                        OnRowCommand ="gvQueryDetails_RowCommand" onrowdatabound="gvQueryDetails_RowDataBound">
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField ="vSubjectId" HeaderText ="Subject Id" Visible="false"  />
                                                                                        <asp:BoundField DataField="vProjectNo" HeaderText="SiteNo" ></asp:BoundField>
                                                                                        <asp:BoundField DataField="vMySubjectNo" HeaderText="vMySubjectNo" />
                                                                                        <asp:BoundField DataField="vNodeDisplayName" HeaderText="Visit" />
                                                                                        <asp:BoundField DataField="Status" HeaderText="Status" />
                                                                                        <asp:BoundField DataField="vRemarks" HeaderText="Remarks" />
                                                                                        <asp:BoundField DataField="dModifyOn" HeaderText="Change On" />
                                                                                        
                                                                                        <asp:TemplateField  HeaderText ="Download Image">
                                                                                            <ItemTemplate >
                                                                                                <asp:ImageButton ID="btnImageType" runat="server" ImageUrl="~/images/imgimport.png" Text='Download' Font-Underline="true" ToolTip='Download' CommandName='DCM' 
                                                                                                    CommandArgument="<%# Container.DataItemIndex %>" ></asp:ImageButton> 
                                                                                                <asp:ImageButton  ID="btnQC" runat="server" ImageUrl="~/images/QC.png" Text="QC1" Font-Underline="true" ToolTip="QC1" WIDTH="25PX" CommandName="QC1" CommandArgument="<%# Container.DataItemIndex %>"></asp:ImageButton>&nbsp;
                                                                                                <asp:ImageButton  ID="btnQC2" runat="server" ImageUrl="~/images/QC.png" Text="QC2" Font-Underline="true" ToolTip="QC2" WIDTH="25PX" CommandName="QC2" CommandArgument="<%# Container.DataItemIndex %>"></asp:ImageButton>&nbsp;
                                                                                                <asp:ImageButton  ID="btnGrader" runat="server" ImageUrl="~/images/CA.png" Text="CA" Font-Underline="true" ToolTip="CA" WIDTH="25PX" CommandName="CA1" CommandArgument="<%# Container.DataItemIndex %>"></asp:ImageButton>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:BoundField DataField="vWorkspaceId" HeaderText="WorkspaceId" Visible="false" />   
                                                                                        <asp:BoundField DataField="vSubjectId" HeaderText="SubjectId" Visible="false" />   
                                                                                        <asp:BoundField DataField="vActivityId" HeaderText="ActivityId" Visible="false" />   
                                                                                        <asp:BoundField DataField="iNodeId" HeaderText="NodeId" Visible="false" /> 
                                                                                        <asp:BoundField DataField="vMySubjectNo" HeaderText="vMySubjectNo" Visible="false" />
                                                                                        <asp:BoundField DataField="QCUserCode" HeaderText="QC1" Visible="false" />
                                                                                        <asp:BoundField DataField="CAUserCode" HeaderText="CA" Visible="false" />
                                                                                        <asp:BoundField DataField="QC2UserCode" HeaderText="QC2" Visible="false" />
                                                                                    </Columns>
                                                                                <HeaderStyle Wrap="False" />
                                                                                </asp:GridView>
                                                                                    <table id="emptyTable1" runat="server" visible="false">
                                                                                         <tr>
                                                                                             <td colspan="9">
                                                                                                 No Records Found!
                                                                                             </td>
                                                                                         </tr>
                                                                                     </table>
                                                                                </div>

                                                                                      </ContentTemplate>
                                                                                  <Triggers>
                                                                                    <asp:AsyncPostBackTrigger ControlID="btnGoQuery" EventName="Click"  />
                                                                                </Triggers>
                                                                                  </asp:UpdatePanel>
						                                               </div>
					                                                </div>
				                                                </div>
                                                </div>
                                                --%>
                                            <%-- <div class="container">
                                            <div class="panel panel-primary" id="Div2" runat="server">
					                                                <div class="panel-heading">
						                                                <h3 class="panel-title">Query Details</h3>
					                                                </div>
					                                                <div class="panel-body">
						                                                 <div class="col-12">  
                                                                              <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                                                  <ContentTemplate>
                                                                         
                                                                                <div  style="width: 100%; height: auto; overflow: auto;">
                                                                                    <table id="tblQueryMaster" class="tblAudit" width="100%" style="background-color: aliceblue;" border="1">
                                                                                       
                                                                                     </table>
                                                                                </div>
                                                                                      </ContentTemplate>
                                                                                  </asp:UpdatePanel>
						                                               </div>
					                                                </div>
				                                                </div>
                                                 </div>--%>
                                                 </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button runat="server" ID="btnMediator" Text="Mediator" Style="display: none;" />
                            <asp:HiddenField runat="server" ID="HfProjectType" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    <div id='loadingmessage' style='display:none'></div>
    <asp:hiddenfield id="hdiQueryId" runat="server" value="0" />   
     <div id="QueryDiv" class="modal" runat="server" >
      <div class="modal-content" style="top:20%;left:4%;width:90%"}}>
            <div class="modal-header" style="position: sticky;">
                <img alt="Close1" src="images/Sqclose.gif" class="close modalCloseImage"  onclick="SignAuthModalClose(); return false;" />
                <h3 style="text-align:center">
                <label id="Label31">Audit Query Details</label>
                </h3>
            </div>
            <div class='modal-body' style="height:300px; width:100%; overflow:auto;">
                <table id="AuditTrailTable" class="tblAudit" width="100%" style="background-color: aliceblue;" border="1">
                </table>
                <input type="button" id="btnexportPDF" class="btn btnsave" value="Export to PDF"  style="width:105px;" onclick="exportPDF(); return false;" />
            </div>
        </div>
         </div>
    <script language="javascript" type="text/javascript" id=".">
        var GloabalWorkspaceId = "";
        function NoticeOpenByDefault() {
            $(".Panelattachment").animate({ "left": "3%" }, "medium");
            $("#img2").attr('src', 'images/NoticePin.png');
            $(".Contentth").css({ "left": "25.3%", "width": "72.5%" });
            $(".Content-Head").css({ "width": "100%" });
            $("#divgrid").width($(window).width() - 343);
        }

        function pageLoad() {
            //debugger;
            //added by Vivek Patel
            //$("#tabs").tabs();
            //document.getElementsByClassName('tablinks')[0].click()
            //openCity(event, 'DiSoft');
            $('#tbl_projectdetail').hide();
            $('#tbl_siteinfo').hide();
            $('#tbl_subsiteinfo').hide();
            $('#tbl_CRFStatus').hide();
            $('#tbl_AESAE').hide();
            $('#vp').hide();

            //added by Dhruvi Shah
            $('#demotab1').hide();
            $('#demotab').hide();
            $('#dcfmanage').hide();
            //completed

            jQuery('#<%= LblListOfProjectForSA.ClientID %>').mouseover(function () {
                jQuery('#DivForSA').fadeIn('medium');
            });
            jQuery('#<%= LblListOfProjectForSA.ClientID %>').mouseout(function () {
                jQuery('#DivForSA').fadeOut('medium');
            });
            jQuery('#<%= LblListOfProjectForES.ClientID %>').mouseover(function () {
                jQuery('#DivForES').fadeIn('medium');
            });
            jQuery('#<%= LblListOfProjectForES.ClientID %>').mouseout(function () {
                jQuery('#DivForES').fadeOut('medium');
            });
            jQuery('#<%= ImageForES.ClientID %>').mouseover(function () {
                jQuery('#DivForES').fadeIn('medium');
            });
            jQuery('#<%= ImageForES.ClientID %>').mouseout(function () {
                jQuery('#DivForES').fadeOut('medium');
            });
            jQuery('#<%= ImageForSA.ClientID %>').mouseover(function () {
                jQuery('#DivForSA').fadeIn('medium');
            });
            jQuery('#<%= ImageForSA.ClientID %>').mouseout(function () {
                jQuery('#DivForSA').fadeOut('medium');
            });

            jQuery('#canal').hide();
            jQuery('#legend').mouseover(function () {
                $('#canal').show('medium');
            });
            jQuery('#legend').mouseout(function () {
                jQuery('#canal').hide('medium');
            });
        }

        function showmodalpopup() {
            $("#Div1").dialog({
                title: "Subject Information",
                width: 430,
                height: 250,
                modal: true,
                buttons: {
                    Close: function () {
                        $(this).dialog('close');
                    }
                }
            });
        };


        var SiteWiseSubjectDetail, SiteInformation, CRFStatus, AESAE, OperationKPI, NewDemo, NewDemo1, DcfManagement
        var fnlpanelId, fnlDivid

        function togglePanel(ele, panelId, speed) {
            fnlpanelId = panelId
            SiteWiseSubjectDetail = document.getElementById("divSiteWiseSubjectDetail").id;
            SiteInformation = document.getElementById("divSiteInformation").id;
            CRFStatus = document.getElementById("divCRFStatus").id;
            AESAE = document.getElementById("divAESAE").id;
            OperationKPI = document.getElementById("divOperationalKpi").id;
            //Added by Dhruvi Shah
            NewDemo = document.getElementById("divDemo").id;
            NewDemo1 = document.getElementById("divDemo1").id;
            DcfManagement = document.getElementById("divDcfmanage").id;
            //completed

            if (OperationKPI == panelId) {
                $('<%=TxtFromDateOfOperationalKpi.ClientID%>').focus()
            }

            if (SiteWiseSubjectDetail == panelId || SiteInformation == panelId || CRFStatus == panelId || AESAE == panelId || NewDemo == panelId || NewDemo1 == panelId || DcfManagement == panelId) {
                if (SiteWiseSubjectDetail == panelId) {
                    document.getElementById('ctl00_CPHLAMBDA_dd_chart').disabled = true
                    $($get('<%= dd_chart.ClientID()%>')).val("Select Chart");
                }
                if (NewDemo == panelId) {
                    document.getElementById('ctl00_CPHLAMBDA_dd1_chart').disabled = true
                    $($get('<%= dd1_chart.ClientID()%>')).val("Select Chart");
                }
                if (NewDemo1 == panelId) {
                    document.getElementById('ctl00_CPHLAMBDA_dd2_chart').disabled = true
                    $($get('<%= dd2_chart.ClientID()%>')).val("Select Chart");
                }
                if (DcfManagement == panelId) {
                    document.getElementById('ctl00_CPHLAMBDA_dd3_chart').disabled = true
                    $($get('<%= dd3_chart.ClientID()%>')).val("Select Chart");
                }
                GetProjectNo(panelId);
            }

            if (speed == undefined) speed = 'slow';
            //jQuery('#' + panelId).toggle(speed, function () {
            //var src = jQuery(ele).attr("src").replace("collapse", "expand");
            //var alt = jQuery(ele).attr("alt").replace("Collapse", "Expand");
            //if (ele.src.indexOf('panelexpand.png') > 0) {
            //    src = jQuery(ele).attr("src").replace("expand", "collapse");
            //    alt = jQuery(ele).attr("alt").replace("Expand", "Collapse");
            //}
            //jQuery(ele).attr("src", src);
            //jQuery(ele).attr("alt", alt);
            //});
            ModalOpen(panelId);
        }

        function ShowHideDiv(Divid, ImageId, Type) {
            var e = document.getElementById(Divid);
            var f = $("#" + ImageId);
            if (e.style.display == 'none') {
                e.style.display = '';
                $(f).attr("src", "images/panelcollapse.png");
                document.getElementById('<%= HfProjectType.ClientId %>').value = '';
                document.getElementById('<%=txtDemo.ClientID %>').value = '';
                document.getElementById('<%=ddlColumnList.ClientID%>').value = '';
                document.getElementById('<%=ProjectWorkSpaceIdDemo.ClientID%>').value = '';
                document.getElementById('<%=txtDemo1.ClientID%>').value = '';
                document.getElementById('<%=ddlColumnList1.ClientID%>').value = '';
                document.getElementById('<%=ProjectWorkSpaceIdDemo1.ClientID%>').value = '';
                document.getElementById('<%=txtDcfmanage.ClientID%>').value = '';
                document.getElementById('<%=ddlColumnList3.ClientID%>').value = '';
                document.getElementById('<%=ProjectWorkSpaceIdDCFManage.ClientID%>').value = '';
                if (Type != 'TrackProjectStatus') {
                    document.getElementById('<%= HfProjectType.ClientId %>').value = Type;
                    var btn = document.getElementById('<%= btnMediator.ClientId %>');
                    btn.click();
                }
            }
            else {
                e.style.display = 'none';
                $(f).attr("src", "images/panelexpand.png");
            }
        }

        var vWorkSpaceId = document.getElementById('<%= ProjectWorkSpaceID.ClientID%>').value

        function ShowHideDivTemp(Divid, ImageId) {
            fnlDivid = Divid
            SiteWiseSubjectDetail = document.getElementById("divSiteWiseSubjectDetail").id;
            SiteInformation = document.getElementById("divSiteInformation").id;
            CRFStatus = document.getElementById("divCRFStatus").id;
            AESAE = document.getElementById("divAESAE").id;
            OperationKPI = document.getElementById("divOperationalKpi").id;
            //added by DhruviShah
            NewDemo = document.getElementById("divDemo").id;
            NewDemo1 = document.getElementById("divDemo1").id;
            DcfManagement = document.getElementById("divDcfmanage").id;
            //completed 

            if (OperationKPI == Divid) {
                $('<%=TxtFromDateOfOperationalKpi.ClientID%>').focus()
            }

            if (SiteWiseSubjectDetail == Divid || SiteInformation == Divid || CRFStatus == Divid || AESAE == Divid || NewDemo == Divid || NewDemo1 == Divid || DcfManagement == Divid) {
                if (SiteWiseSubjectDetail == Divid) {
                    document.getElementById('ctl00_CPHLAMBDA_dd_chart').disabled = true
                    $($get('<%= dd_chart.ClientID()%>')).val("Select Chart");
                }
                if (NewDemo == Divid) {
                    document.getElementById('ctl00_CPHLAMBDA_dd1_chart').disabled = true
                    $($get('<%= dd1_chart.ClientID()%>')).val("Select Chart");
                }
                if (NewDemo1 == Divid) {
                    document.getElementById('ctl00_CPHLAMBDA_dd2_chart').disabled = true
                    $($get('<%= dd2_chart.ClientID()%>')).val("Select Chart");
                }
                if (DcfManagement == Divid) {
                    document.getElementById('ctl00_CPHLAMBDA_dd3_chart').disabled = true
                    $($get('<%= dd3_chart.ClientID()%>')).val("Select Chart");
                }
                GetProjectNo(Divid);
            }

            var e = document.getElementById(Divid);
            var f = $("#" + ImageId);
            if (e.style.display == 'none') {
                e.style.display = '';
                $(f).attr("src", "images/panelcollapse.png");
            }
            else {
                e.style.display = 'none';
                $(f).attr("src", "images/panelexpand.png");
            }
        }

        function ShowHideDivTemp1(Divid, ImageId, FlagId) {
            var e = document.getElementById(Divid);
            var f = $(ImageId);
            var g = $(FlagId);
            if (FlagId == '1') {
                e.style.display == '';
            }
            else {
                e.style.display == 'none';
            }
        }

        // For Clinical Request //  
        function ClientPopulatedForClientRequest(sender, e) {
            ProjectClientShowingDynamic('AutoCompleteExtenderForClientRequest', $get('<%= TxtRequestIdClientRequest.ClientId %>'));
        }
        function OnSelectedForClientRequest(sender, e) {
            ProjectOnItemSelectedDynamic(e.get_value(), $get('<%= TxtRequestIdClientRequest.ClientId %>'),
        $get('<%= HProjectIdForClientRequest.ClientId %>'), document.getElementById('<%= btnSetProjectforclientrequest.ClientId %>'));
        }

        // For Pre-Clinical //
        function ClientPopulatedForProjectPreClinical(sender, e) {
            ProjectClientShowingDynamic('AutoCompleteExtenderForProjectPreClinical', $get('<%= TxtProjectRequestIdProjectPreClinical.ClientId %>'));
        }
        function OnSelectedForProjectPreClinical(sender, e) {
            ProjectOnItemSelectedDynamic(e.get_value(), $get('<%= TxtProjectRequestIdProjectPreClinical.ClientId %>'),
        $get('<%= HFRequestIdProjectPreClinical.ClientId %>'), document.getElementById('<%= btnSetProjectProjectPreClinical.ClientId %>'));
        }

        function ClientPopulatedForVisit(sender, e) {
            SubjectDashboardClientShowing('AutoCompleteExtender2', $get('<%= txtScreeningForDI.ClientId %>'));
        }
        function OnSelectedForVisit(sender, e) {
            SubjectOnItemSelectedDashboard(e.get_value(), $get('<%= txtScreeningForDI.ClientId %>'), $get('<%= HScreeningNo.ClientId %>'));
        }
        <%--OnSelectedForVisit--%>


        <%-- function ClientPopulatedForQuery(sender, e) {
            SubjectDashboardClientShowing('AutoCompleteExtender12', $get('<%= txtScreeningForDI12.ClientId %>'));
        }
        function OnSelectedForQuery(sender, e) {
            SubjectOnItemSelectedDashboard(e.get_value(), $get('<%= txtScreeningForDI12.ClientId %>'),
        $get('<%= HScreeningNo12.ClientId %>'), document.getElementById('<%= btnScreeningNo12.ClientId %>'));
        }
        OnSelectedForQuery--%>

        // For Clinical Phase//
        function ClientPopulatedForClinicalPhase(sender, e) {
            ProjectClientShowingDynamic('AutoCompleteExtenderForClinicalPhase', $get('<%= TxtProjectForClinicalPhase.ClientId %>'));
        }
        function OnSelectedForClinicalPhase(sender, e) {
            ProjectOnItemSelectedDynamic(e.get_value(), $get('<%= TxtProjectForClinicalPhase.ClientId %>'),
        $get('<%= HProjectIdForClinicalPhase.ClientId %>'), document.getElementById('<%= btnSetProjectForClinicalPhase.ClientId %>'));
        }

        // For AnalyticalPhase  //
        function ClientPopulatedForAnalyticalPhase(sender, e) {
            ProjectClientShowingDynamic('AutoCompleteExtenderForAnalyticalPhase', $get('<%= TxtProjectNoForAnalyticalPhase.ClientId %>'));
        }
        function OnSelectedForAnalyticalPhase(sender, e) {
            ProjectOnItemSelectedDynamic(e.get_value(), $get('<%= TxtProjectNoForAnalyticalPhase.ClientId %>'),
        $get('<%= HProjectIdforAnalyticalPhase.ClientId %>'), document.getElementById('<%= btnSetProjectforAnalyticalPhase.ClientId %>'));
        }

        //Document Phase //
        function ClientPopulatedForDocumentPhase(sender, e) {
            ProjectClientShowingDynamic('AutoCompleteExtenderForDocumentPhase', $get('<%= TxtProjectNoForDocumentPhase.ClientId %>'));
        }
        function OnSelectedForDocumentPhase(sender, e) {
            ProjectOnItemSelectedDynamic(e.get_value(), $get('<%= TxtProjectNoForDocumentPhase.ClientId %>'),
        $get('<%= HProjectIdforDocumentPhase.ClientId %>'), document.getElementById('<%= BtnSetProjectForDocumentPhase.ClientId %>'));
        }

        // For Project Tracking

        function ClientPopulatedForTracking(sender, e) {
            ProjectClientShowingDynamic('AutoCompleteExtenderForTracking', $get('<%= TxtProjectNoPlainForTrackProjectStatus.ClientId %>'));
        }
        function OnSelectedForTracking(sender, e) {
            ProjectOnItemSelectedDynamic(e.get_value(), $get('<%= TxtProjectNoPlainForTrackProjectStatus.ClientId %>'), $get('<%= HProjectIdForTracking.ClientId %>'), document.getElementById('<%= btnSetProjectforTracking.ClientId %>'));
        }

        function chkChange() {
            return true;
        }

        function OnSelectedForVisitTracker(sender, e) {
            ProjectOnItemSelectedForMsrLog(e.get_value(), $get('<%= txtProjectForVisitTracker.Clientid %>'),
             $get('<%= HProjectIdForVisitTracker.Clientid %>'), document.getElementById('<%= btnSetForVisitTracker.ClientId%>'), $get('<%=HClientName.Clientid %>'), $get('<%=HProjectName.Clientid %>'));
        }
        function ClientPopulatedForVisitTracker(sender, e) {
            ProjectClientShowingSchema('AutoCompleteExtenderadd', $get('<%= txtProjectForVisitTracker.ClientId %>'));
        }
        function OnSelectedForVisitScheduler(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtprojectForVisitScheduler.Clientid %>'),
            $get('<%= HProjectIdForVisitScheduler.Clientid %>'), document.getElementById('<%= btnSetProjectforVisitScheduler.ClientId %>'));
        }

        function ClientPopulatedForVisitScheduler(sender, e) {
            ProjectClientShowing('AutoCompleteExtenderForVisitScheduler', $get('<%= txtprojectForVisitScheduler.ClientId %>'));
        }

        //Added by Dhruvi Shah
        function ClientPopulatedForCRFDataStatus(sender, e) {
            ProjectClientShowingDynamic('AutoCompleteExtenderForCRFDataStatus', $get('<%= txtDemo.ClientID%>'));
        }
        function OnSelectedForCRFDataStatus(sender, e) {
            ProjectOnItemSelectedDynamic(e.get_value(), $get('<%= txtDemo.ClientID%>'),
        $get('<%= ProjectWorkSpaceIdDemo.ClientID%>'), document.getElementById('<%= BtnCRFDataStatus.ClientId %>'));
        }
        function ClientPopulatedForsiteData(sender, e) {
            ProjectClientShowingDynamic('AutoCompleteExtenderforsiteData', $get('<%= txtDemo1.ClientID%>'));
        }
        function OnSelectedForsiteData(sender, e) {
            ProjectOnItemSelectedDynamic(e.get_value(), $get('<%= txtDemo1.ClientID%>'),
        $get('<%= ProjectWorkSpaceIdDemo1.ClientID%>'), document.getElementById('<%= BtnSiteDataStatus.ClientID%>'));
        }
        function ClientPopulatedForDCFmanage(sender, e) {
            ProjectClientShowingDynamic('AutoCompleteExtenderforDCFManage', $get('<%= txtDcfmanage.ClientID%>'));
        }
        function OnSelectedForDCFmanage(sender, e) {
            ProjectOnItemSelectedDynamic(e.get_value(), $get('<%= txtDcfmanage.ClientID%>'),
       $get('<%= ProjectWorkSpaceIdDCFManage.ClientID%>'), document.getElementById('<%= BtnDCFManage.ClientID%>'));
        }
        function ClientPopulatedForaese(sender, e) {
            ProjectClientShowingDynamic('AutoCompleteExtenderforaese', $get('<%= txtAESAEProject.ClientID%>'));
        }
        function OnSelectedForaese(sender, e) {
            ProjectOnItemSelectedDynamic(e.get_value(), $get('<%= txtAESAEProject.ClientID%>'),
       $get('<%= hdnAESAEProjectWorkSpaceID.ClientID%>'), document.getElementById('<%= BtnAESE.ClientID%>'));
        }
        function ClientPopulatedForcrf1(sender, e) {
            ProjectClientShowingDynamic('AutoCompleteExtenderforcrf1', $get('<%= txtCRFStatusProject.ClientID%>'));
        }
        function OnSelectedForcrf1(sender, e) {
            ProjectOnItemSelectedDynamic(e.get_value(), $get('<%= txtCRFStatusProject.ClientID%>'),
      $get('<%= hdnCRFStatusProjectWorkSpaceID.ClientID%>'), document.getElementById('<%= Btncrf1.ClientID%>'));
        }
        function ClientPopulatedForsiteInfo(sender, e) {
            ProjectClientShowingDynamic('AutoCompleteExtenderforsiteInfo', $get('<%= txtSiteInformationProject.ClientID%>'));
        }
        function OnSelectedForsiteInfo(sender, e) {
            ProjectOnItemSelectedDynamic(e.get_value(), $get('<%= txtSiteInformationProject.ClientID%>'),
     $get('<%= hdnSiteInformationProjectWorkSpaceID.ClientID%>'), document.getElementById('<%= BtnSiteInf1.ClientID%>'));
        }
        function ClientPopulatedForsubjectInfo(sender, e) {
            ProjectClientShowingDynamic('AutoCompleteExtenderforsubjectInfo', $get('<%= txtproject.ClientID%>'));
        }
        function OnSelectedForsubjectInfo(sender, e) {
            ProjectOnItemSelectedDynamic(e.get_value(), $get('<%= txtproject.ClientID%>'),
     $get('<%= ProjectWorkSpaceID.ClientID%>'), document.getElementById('<%= BtnSubjectinfo.ClientID%>'));
        }


        // Completed 
        //Added by rinkal 
        <%--function ClientPopulated(sender, e) {
            ProjectClientShowingDynamic('AutoCompleteExtender1', $get('<%= txtprojectForDI.ClientID%>'));
        }--%>
        <%--function OnSelected(sender, e) {
            ProjectOnItemSelectedDynamic(e.get_value(), $get('<%= txtprojectForDI.ClientID%>'),
     $get('<%= HProjectId.ClientID%>'), document.getElementById('<%= btnSetProject.ClientID%>'));
        }--%>
        function ClientPopulatedSite(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtSite.ClientID%>'));
        }
        function OnSelectedSite(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtSite.Clientid %>'),
            $get('<%= HProjectId.Clientid %>'), document.getElementById('<%= btnSetSite.ClientId %>'));
      }
        //completed

        <%--function ClientPopulatedProject(sender, e) {
            ProjectClientShowing('AutoCompleteExtender3', $get('<%= txtprojectForCA.ClientId %>'));
        }

        function OnSelectedProject(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProjectForCA.Clientid %>'),
            $get('<%= HdnProjectId.Clientid %>'), document.getElementById('<%= btnSetProjectForCA.ClientId %>'));            
        }--%>
    </script>

     <script src="Script/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery-1.4.3.min.js"></script>
    <script src="Script/jquery.dataTables.min.js" type="text/javascript"> </script>
     <script src="Script/jquery.dataTables.plugins.js" type="text/javascript"></script>
 
      <%--Below Login For dashBoard added By Vivek Patel--%>
    <script type="text/javascript">
        var selectionList = {};
        var selectionList2 = {};
        var finalproject = [];
        var finalsubject = [];
        var donutFinal = [];
        var childproject = [];
        var childsubject = [];
        var Dfinalproject = [];
        var Dfinalsubject = [];
        var DdonutFinal = [];
        var Dchildproject = [];
        var Dchildsubject = [];
        var projectdetail = [];
        var productIds = new Object();
        var vWorkSpaceId;
        var WorkSpaceId;
        var nWorkspaceDefaultWorkflowUserId;
        var Workspace;
        var Training;

        function txtprojectSelectAll(id) {
            document.getElementById(id).focus();
            document.getElementById(id).select();
        }

        $(document).ready(function () {
            //bindQueryFn();
        });

        //Get Project No to Bind AutoComplete DropDown   

        function GetProjectNo(Divid) {
            $.ajax({
                type: "post",
                url: "frmMainPage.aspx/Proc_GetProjectNo",
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                dataType: "json",
                success: function (data) {
                    var data
                    data = data.d
                    var msgs = [];
                    msgs = JSON.parse(data);
                    if (msgs == null) {
                    }
                    else {

                        if (SiteWiseSubjectDetail == Divid || SiteWiseSubjectDetail == fnlpanelId) {
                            $("#dataproject").empty();
                        }
                        else if (SiteInformation == Divid || SiteInformation == fnlpanelId) {
                            $("#SiteInformationdataproject").empty();
                        }
                        else if (CRFStatus == Divid || CRFStatus == fnlpanelId) {
                            $("#CRFStatusdataproject").empty();
                        }
                        else if (AESAE == Divid || AESAE == fnlpanelId) {
                            $("#AESAEdataproject").empty();
                        }
                        else {
                            $("#dataproject").empty();
                            $("#SiteInformationdataproject").empty();
                            $("#CRFStatusdataproject").empty();
                            $("#AESAEdataproject").empty();
                        }
                        //for (var i = 0, l = msgs.Table.length; i < l; i++) {
                        if (SiteWiseSubjectDetail == Divid || SiteWiseSubjectDetail == fnlpanelId) {
                            for (var i = 0, l = msgs.Table.length; i < l; i++) {
                                $("#dataproject").append('<option value="' + msgs.Table[i].DisplayProject + '" id="' + msgs.Table[i].vWorkspaceId + '">' + msgs.Table[i].vProjectNo + '</option>');
                            }
                        }
                        else if (SiteInformation == Divid || SiteInformation == fnlpanelId) {
                            for (var i = 0, l = msgs.Table.length; i < l; i++) {
                                $("#SiteInformationdataproject").append('<option value="' + msgs.Table[i].DisplayProject + '" id="' + msgs.Table[i].vWorkspaceId + '">' + msgs.Table[i].vProjectNo + '</option>');
                            }
                        }
                        else if (CRFStatus == Divid || CRFStatus == fnlpanelId) {
                            for (var i = 0, l = msgs.Table.length; i < l; i++) {
                                $("#CRFStatusdataproject").append('<option value="' + msgs.Table[i].DisplayProject + '" id="' + msgs.Table[i].vWorkspaceId + '">' + msgs.Table[i].vProjectNo + '</option>');
                            }
                        }
                        else if (AESAE == Divid || AESAE == fnlpanelId) {
                            for (var i = 0, l = msgs.Table.length; i < l; i++) {
                                $("#AESAEdataproject").append('<option value="' + msgs.Table[i].DisplayProject + '" id="' + msgs.Table[i].vWorkspaceId + '">' + msgs.Table[i].vProjectNo + '</option>');
                            }
                        }
                        else {
                            for (var i = 0, l = msgs.Table.length; i < l; i++) {
                                $("#dataproject").append('<option value="' + msgs.Table[i].DisplayProject + '" id="' + msgs.Table[i].vWorkspaceId + '">' + msgs.Table[i].vProjectNo + '</option>');
                                $("#SiteInformationdataproject").append('<option value="' + msgs.Table[i].DisplayProject + '" id="' + msgs.Table[i].vWorkspaceId + '">' + msgs.Table[i].vProjectNo + '</option>');
                                $("#CRFStatusdataproject").append('<option value="' + msgs.Table[i].DisplayProject + '" id="' + msgs.Table[i].vWorkspaceId + '">' + msgs.Table[i].vProjectNo + '</option>');
                                $("#AESAEdataproject").append('<option value="' + msgs.Table[i].DisplayProject + '" id="' + msgs.Table[i].vWorkspaceId + '">' + msgs.Table[i].vProjectNo + '</option>');
                            }
                        }
                        //}
                    }
                },
                failure: function (response) {
                    msgalert("failure");
                    msgalert(data.d);
                },
                error: function (response) {
                    msgalert("error");
                }
            });
        }

        function fnPanelRights() {
            $.ajax({
                type: "post",
                url: "frmMainPage.aspx/PanelRights",
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                dataType: "json",
                success: function (data) {
                    // debugger;
                    var data
                    data = data.d
                    var msgs = [];
                    msgs = JSON.parse(data);
                    msgs = msgs.PANELDISPLAY;

                    for (i = 0 ; i < msgs.length; i++) {
                        if (msgs[i].vPanelId == "0001") {
                            document.getElementById('TrClientRequest').style.display = 'block';
                        }
                    }
                }
            });
        }

        function fnSiteWiseSubjectDetail() {
            $('#tbl_projectdetail').show();
            var dd_project = $($get('<%= dd_project.ClientID%>')).val();
            $("#container").empty();
            $($get('<%= dd_chart.ClientID()%>')).val("Select Chart");
            document.getElementById('ctl00_CPHLAMBDA_dd_chart').disabled = false
            var k;
            var projectval = $("#<%=txtproject.ClientID%>").val()
            var vWorkSpaceId = $("#<%=ProjectWorkSpaceID.ClientID%>").val();
            if (WorkSpaceId == undefined) {
                vWorkSpaceId = document.getElementById('<%= ProjectWorkSpaceID.ClientID%>').value
            }
            else {
                document.getElementById('<%= ProjectWorkSpaceID.ClientID%>').value = WorkSpaceId
                vWorkSpaceId = document.getElementById('<%= ProjectWorkSpaceID.ClientID%>').value
            }

            //For Subject Details

            $(function () {
                $(document).ready(function () {
                    $.ajax({
                        type: "post",
                        url: "frmMainPage.aspx/Proc_SiteWiseSubjectInformation",
                        data: '{"vWorkSpaceId":"' + vWorkSpaceId + '"}',
                        contentType: "application/json; charset=utf-8",
                        datatype: JSON,
                        async: false,
                        dataType: "json",
                        success: function (data) {
                            var data
                            data = data.d
                            var msgs = [];
                            msgs = JSON.parse(data);
                            if (msgs == null) {
                                //alert("No Data Available")
                            }
                            else {
                                var parentproject, parentsubject;
                                parentproject = null;
                                parentsubject = null;
                                childproject = [];
                                childsubject = [];
                                finalproject = [];
                                finalsubject = [];
                                donutFinal = [];
                                Dparentproject = null;
                                Dparentsubject = null;
                                Dchildproject = [];
                                Dchildsubject = [];
                                Dfinalproject = [];
                                Dfinalsubject = [];
                                DdonutFinal = [];
                                var j = 1;
                                for (var i = 0, l = msgs.Table.length; i < l; i++) {
                                    var msg = msgs[i];
                                    if (i == 0) {
                                        parentproject = msgs.Table[i].ProjectNo;
                                        parentsubject = msgs.Table[i].subject;
                                        Dparentproject = msgs.Table[i].ProjectNo;
                                        Dparentsubject = msgs.Table[i].DSubject;
                                    }
                                    else {
                                        if (i == msgs.Table.length) {
                                            childproject.push(msgs.Table[i].ProjectNo + ",");
                                            childsubject.push(msgs.Table[i].subject + ",")
                                            Dchildproject.push(msgs.Table[i].ProjectNo + ",");
                                            Dchildsubject.push(msgs.Table[i].DSubject + ",")
                                        }
                                        else {
                                            childproject.push(msgs.Table[i].ProjectNo);
                                            childsubject.push(msgs.Table[i].subject)
                                            Dchildproject.push(msgs.Table[i].ProjectNo);
                                            Dchildsubject.push(msgs.Table[i].DSubject)
                                        }
                                    }
                                    finalproject.push(msgs.Table[i].ProjectNo);
                                    finalsubject.push(msgs.Table[i].subject)
                                    donutFinal.push({ name: msgs.Table[i].ProjectNo, y: msgs.Table[i].subject });
                                    Dfinalproject.push(msgs.Table[i].ProjectNo);
                                    Dfinalsubject.push(msgs.Table[i].DSubject)
                                    DdonutFinal.push({ name: msgs.Table[i].ProjectNo, y: msgs.Table[i].DSubject });
                                    j++
                                    k = j;
                                    $(".content").height($(".wrapper").height());
                                }
                                $(".content").height($(".wrapper").height());
                                pie();
                            }
                        },
                        failure: function (response) {
                            msgalert("failure");
                            msgalert(data.d);
                        },
                        error: function (response) {
                            msgalert("error");
                        }
                    });
                });
            });

            //Chart Table
            jQuery(document).ready(function ($) {
                var ActivityDataset = [];
                for (var Row = 0; Row < finalproject.length; Row++) {
                    var InDataset = [];
                    InDataset.push(finalproject[Row], finalsubject[Row], Dfinalsubject[Row]);
                    ActivityDataset.push(InDataset);
                }
                $ = jQuery;
                $('title').html("");
                $('#tbl_projectdetail').dataTable().fnDestroy();
                otable = $('#tbl_projectdetail').DataTable({
                    "scrollY": "90px", "scrollCollapse": true, "paging": true, "bPaginate": true,
                    "scrollX": true,
                    "bJQueryUI": true,
                    "sPaginationType": "full_numbers",
                    "bLengthChange": true,
                    "aLengthMenu": [[3, 5, 10, -1], [3, 5, 10, "All"]],
                    "pageLength": 5,
                    "stateSave": true,
                    "bProcessing": true,
                    "bSort": true,
                    "autoWidth": true,
                    "aaData": ActivityDataset,
                    "bInfo": true,
                    "iDisplayLength": 5,
                    "fnCreatedRow": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                        $('td:eq(1)', nRow).append("<finalproject='" + aData[0] + "',  finalsubject='" + aData[1] + "', Dfinalsubject='" + aData[2] + "'>");
                    },
                    "aoColumns": [
                                { "sTitle": "Project/Site Name" },
                                { "sTitle": "Enrolled Subjected" },
                                { "sTitle": "Rejected Subjected" },
                    ],
                    "oLanguage": {
                        "sEmptyTable": "No Record Found",
                    },
                });
            });
            //For Study Detail
            fnStudyDetail(vWorkSpaceId)
        }

        function fnStudyDetail(vWorkSpaceId) {
            $.ajax({
                type: "post",
                url: "frmMainPage.aspx/Proc_ProjectStudyDetail",
                data: '{"vWorkspaceId":"' + vWorkSpaceId + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                dataType: "json",
                success: function (data) {
                    var data
                    data = data.d
                    var msgs = [];
                    msgs = JSON.parse(data);
                    if (msgs == null) {
                        //alert("No Data Available")
                    }
                    else {
                        var study_no = null, study_type = null, study_status = null;
                        for (var i = 0, l = msgs.Table.length; i < l; i++) {
                            study_no = msgs.Table[i].Project_Name;
                            study_type = msgs.Table[i].Project_Type;
                            study_status = msgs.Table[i].Project_Status;
                            study_completion_phase = msgs.Table[i].Project_Phase_Completed;
                            $('#<%= Study_No_Result.ClientID%>').text(study_no);
                            $('#<%= Study_Type_Result.ClientID%>').text(study_type);
                            $('#<%= Study_Status_Result.ClientID%>').text(study_status);
                            $('#<%= Study_Completion_Phase_Result.ClientID%>').text(study_completion_phase);
                        }
                        $(".content").height($(".wrapper").height());
                    }
                },
                failure: function (response) {
                    msgalert("failure");
                    msgalert(data.d);
                },
                error: function (response) {
                    msgalert("error");
                }
            });
        }

        function fnSiteInformation() {
            var projectval = $("#<%=txtSiteInformationProject.ClientID%>").val();
            var vWorkSpaceId = $("#<%=hdnSiteInformationProjectWorkSpaceID.ClientID%>").val();
            fnTotalSubjectDonutCount(vWorkSpaceId);
            fnSiteInfo(vWorkSpaceId);
            fnSubSiteInfo(vWorkSpaceId);
        }

        function fnTotalSubjectDonutCount(vWorkSpaceId) {
            $.ajax({
                type: "post",
                url: "frmMainPage.aspx/Proc_WorkSpaceProjectTotalSubjectCount",
                data: '{"vWorkSpaceId":"' + vWorkSpaceId + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                dataType: "json",
                success: function (data) {
                    var data
                    data = data.d
                    var msgs = [];
                    msgs = JSON.parse(data);
                    if (msgs == null) {
                        msgalert("No Data Available")
                    }
                    else {
                        var TotalSubject = 0;
                        TotalSubject = msgs.Table[0].TotalSubject
                        $("#vp").attr("data-percent", TotalSubject);
                        var $ppc = $('.progress-pie-chart'),
                        //percent = parseInt($ppc.data('percent')),
                        percent = $("#vp").attr("data-percent");
                        deg = 360 * percent / 100;
                        if (percent > 50) {
                            $ppc.addClass('gt-50');
                        }
                        $('.ppc-progress-fill').css('transform', 'rotate(' + TotalSubject + 'deg)');
                        $('.ppc-percents span').html(percent + '');
                        $('#vp').show();
                    }
                },
                failure: function (response) {
                    msgalert("failure");
                    msgalert(data.d);
                },
                error: function (response) {
                    msgalert("error");
                }
            });
        }

        function fnSiteInfo(vWorkSpaceId) {
            $.ajax({
                type: "post",
                url: "frmMainPage.aspx/Proc_SiteInformation",
                data: '{"vWorkSpaceId":"' + vWorkSpaceId + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                dataType: "json",
                success: function (data) {
                    var data = JSON.parse(data.d);
                    if (data == null) {
                        //alert("No Data Available")
                    }
                    else {
                        var ActivityDataset = [];
                        for (var Row = 0; Row < data.Table.length; Row++) {
                            var InDataset = [];
                            InDataset.push(data.Table[Row].Project_Detail, data.Table[Row].Investigator_Detail, data.Table[Row].Total_Subject, data.Table[Row].Active_Subject, data.Table[Row].DeActive_Subject, data.Table[Row].TotalScreening, data.Table[Row].SuccessfullScreening, data.Table[Row].RejectedScreening);
                            ActivityDataset.push(InDataset);
                        }
                        $ = jQuery;
                        $('#tbl_siteinfo').dataTable().fnDestroy();
                        otable = $('#tbl_siteinfo').DataTable({
                            "bJQueryUI": true,
                            //"sPaginationType": "full_numbers",
                            "bLengthChange": false,
                            "aLengthMenu": [[3, 5, 10, -1], [3, 5, 10, "All"]],
                            "stateSave": true,
                            "pageLength": 5,
                            "bProcessing": true,
                            "bSort": true,
                            "autoWidth": true,
                            "aaData": ActivityDataset,
                            "bInfo": true,
                            "iDisplayLength": 5,
                            "fnCreatedRow": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                                $('td:eq(1)', nRow).append("<Project_Detail='" + aData[0] + "',  Investigator_Detail='" + aData[1] + "', Total_Subject='" + aData[2] + "', Active_Subject='" + aData[3] + "', DeActive_Subject='" + aData[4] + "', TotalScreening='" + aData[5] + "', SuccessfullScreening='" + aData[6] + "', RejectedScreening='" + aData[7] + "'>");
                            },
                            "aoColumns": [
                                        { "sTitle": "Project/Site" },
                                        { "sTitle": "Investigator" },
                                        { "sTitle": "Total Subject" },
                                        { "sTitle": "Active Subject" },
                                        { "sTitle": "DeActive Subject" },
                                        { "sTitle": "Total Screening" },
                                        { "sTitle": "Successfull Screening" },
                                        { "sTitle": "Rejected Screening" },
                            ],
                            "oLanguage": {
                                "sEmptyTable": "No Record Found",
                            },
                        });
                        $(".content").height($(".wrapper").height());
                        $('#tbl_siteinfo').show();
                    }
                },
                failure: function (response) {
                    msgalert("failure");
                    msgalert(data.d);
                },
                error: function (response) {
                    msgalert("error");
                }
            });
        }

        function fnSubSiteInfo(vWorkSpaceId) {
            $.ajax({
                type: "post",
                url: "frmMainPage.aspx/Proc_SubSiteInformation",
                data: '{"vWorkSpaceId":"' + vWorkSpaceId + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                dataType: "json",
                success: function (data) {
                    var data = JSON.parse(data.d);

                    if (data == null) {
                        //alert("No Data Available")
                    }
                    else {
                        var ActivityDataset = [];
                        for (var Row = 0; Row < data.Table.length; Row++) {
                            var InDataset = [];
                            InDataset.push(data.Table[Row].ProjectNo, data.Table[Row].Investigator, data.Table[Row].Total_Subject, data.Table[Row].Active_Subject, data.Table[Row].Subject_Cancelled, data.Table[Row].Total_Screening, data.Table[Row].Successfull_Screening, data.Table[Row].Rejected_Screening);
                            ActivityDataset.push(InDataset);
                        }
                        $ = jQuery;
                        $('#tbl_subsiteinfo').dataTable().fnDestroy();
                        otable = $('#tbl_subsiteinfo').DataTable({
                            "bJQueryUI": true,
                            "sPaginationType": "full_numbers",
                            "pageLength": 5,
                            "bLengthChange": true,
                            "aLengthMenu": [[3, 5, 10, -1], [3, 5, 10, "All"]],
                            "stateSave": true,
                            "bProcessing": true,
                            "bSort": true,
                            "autoWidth": true,
                            "aaData": ActivityDataset,
                            "bInfo": true,
                            "iDisplayLength": 5,
                            "fnCreatedRow": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                                $('td:eq(1)', nRow).append("<ProjectNo='" + aData[0] + "',  Investigator='" + aData[1] + "', Total_Subject='" + aData[2] + "', Active_Subject='" + aData[3] + "', Subject_Cancelled='" + aData[4] + "', Total_Screening='" + aData[5] + "', Successfull_Screening='" + aData[6] + "', Rejected_Screening='" + aData[7] + "'>");
                            },
                            "aoColumns": [
                                        { "sTitle": "Project/Site" },
                                        { "sTitle": "Investigator" },
                                        { "sTitle": "Total Subject" },
                                        { "sTitle": "Active Subject" },
                                        { "sTitle": "DeActive Subject" },
                                        { "sTitle": "Total Screening" },
                                        { "sTitle": "Successfull Screening" },
                                        { "sTitle": "Rejected Screening" },
                            ],
                            "oLanguage": {
                                "sEmptyTable": "No Record Found",
                            },
                        });
                        $(".content").height($(".wrapper").height());
                        $('#tbl_subsiteinfo').show();
                    }
                },
                failure: function (response) {
                    msgalert("failure");
                    msgalert(data.d);
                },
                error: function (response) {
                    msgalert("error");
                }
            });
        }

        function fnCRFStatus() {
            var projectval = $("#<%=txtCRFStatusProject.ClientID%>").val()
            var isChild = "Y";
            var vBoolId = 0;
            var vWorkSpaceId = $("#<%=hdnCRFStatusProjectWorkSpaceID.ClientID%>").val();
            $.ajax({
                type: "post",
                url: "frmMainPage.aspx/Proc_GetActivityStatusCountRecords",
                data: '{"vWorkSpaceId":"' + vWorkSpaceId + '","vBoolId":"' + vBoolId + '","isChild":"' + isChild + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                dataType: "json",
                success: function (data) {
                    var data = JSON.parse(data.d);
                    if (data == null) {
                        //alert("No Data Available")
                    }
                    else {
                        var ActivityDataset = [];
                        for (var Row = 0; Row < data.Table.length; Row++) {
                            var InDataset = [];
                            //InDataset.push(data.Table[Row]['Answered DCF'], data.Table[Row]['Data Entry Continue'], data.Table[Row]['Data Entry Pending'], data.Table[Row]['Final Reviewed & Freeze'], data.Table[Row]['First Review Done'], data.Table[Row]['Generated DCF'], data.Table[Row]['No. Of Subject'], data.Table[Row]['Project/Site Id.'], data.Table[Row]['Ready For Review'], data.Table[Row]['Second Review Done'], data.Table[Row]['Total DCF']);
                            InDataset.push(data.Table[Row]['Project/Site Id.'], data.Table[Row]['No. Of Subject'], data.Table[Row]['Data Entry Pending'], data.Table[Row]['Data Entry Continue'], data.Table[Row]['Ready For Review'], data.Table[Row]['First Review Done'], data.Table[Row]['Second Review Done'], data.Table[Row]['Final Reviewed & Freeze'], data.Table[Row]['Generated DCF'], data.Table[Row]['Answered DCF'], data.Table[Row]['Total DCF']);
                            ActivityDataset.push(InDataset);
                        }
                        $ = jQuery;
                        $('#tbl_CRFStatus').dataTable().fnDestroy();
                        otable = $('#tbl_CRFStatus').DataTable({
                            "bJQueryUI": true,
                            "scrollX": true,
                            "sPaginationType": "full_numbers",
                            "pageLength": '5',
                            "bLengthChange": true,
                            "aLengthMenu": [[3, 5, 10, -1], [3, 5, 10, "All"]],
                            "stateSave": true,
                            "bProcessing": true,
                            "bSort": true,
                            "autoWidth": false,
                            "aaData": ActivityDataset,
                            "bInfo": true,
                            "iDisplayLength": 5,
                            "fnCreatedRow": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                                $('td:eq(15)', nRow).append("<['Project/Site Id.'] = '" + aData[0] + "', ['No. Of Subject'] = '" + aData[1] + "', ['Data Entry Pending'] = '" + aData[2] + "', ['Data Entry Continue'] = '" + aData[3] + "', ['Ready For Review'] = '" + aData[4] + "', ['First Review Done'] = '" + aData[5] + "', ['Second Review Done'] ='" + aData[6] + "', ['Final Reviewed & Freeze'] = '" + aData[7] + "', ['Generated DCF'] = '" + aData[8] + "', ['Answered DCF'] = '" + aData[9] + "', ['Total DCF'] = '" + aData[10] + "'>");
                            },
                            "aoColumns": [
                                        { "sTitle": "Project" },
                                        { "sTitle": "No.Of Sub" },
                                        { "sTitle": "Data Entry Pend" },
                                        { "sTitle": "Data Entry Cont" },
                                        { "sTitle": "Ready For Review" },
                                        { "sTitle": "First Review Done" },
                                        { "sTitle": "Second Review Done" },
                                        { "sTitle": "Final Review & Freeze" },
                                        { "sTitle": "Generated DCF" },
                                        { "sTitle": "Answered DCF" },
                                        { "sTitle": "Total DCF" },
                            ],
                            "columns": [
                                { "width": "5%", "targets": 0 },
                                null, null, null, null, null, null, null, null, null, null,
                            ],
                            "oLanguage": {
                                "sEmptyTable": "No Record Found",
                            },
                        });
                        $(".content").height($(".wrapper").height());
                        $('#tbl_CRFStatus').show();
                    }
                },
                failure: function (response) {
                    msgalert("failure");
                    msgalert(data.d);
                },
                error: function (response) {
                    msgalert("dfgasdfgasdf");
                }
            });
        }

        function fnAESAE() {
            var projectval = $("#<%=txtAESAEProject.ClientID%>").val()


            var vWorkSpaceId = $("#<%=hdnAESAEProjectWorkSpaceID.ClientID%>").val();

            $.ajax({
                type: "post",
                url: "frmMainPage.aspx/Proc_GetWorkSpaceProjectAESAE",
                data: '{"vWorkSpaceId":"' + vWorkSpaceId + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                dataType: "json",
                success: function (data) {
                    var data = JSON.parse(data.d);
                    if (data == null) {
                        //alert("No Data Available")
                    }
                    else {
                        var ActivityDataset = [];

                        for (var Row = 0; Row < data.Table.length; Row++) {
                            var InDataset = [];
                            InDataset.push(data.Table[Row]['SiteName'], data.Table[Row]['AE'], data.Table[Row]['SAE']);
                            ActivityDataset.push(InDataset);
                        }
                        $ = jQuery;
                        $('#tbl_AESAE').dataTable().fnDestroy();
                        otable = $('#tbl_AESAE').DataTable({
                            "bJQueryUI": true,
                            //"sPaginationType": "full_numbers",
                            "bLengthChange": false,
                            "stateSave": true,
                            "bProcessing": true,
                            "bSort": true,
                            "autoWidth": true,
                            "aaData": ActivityDataset,
                            "bInfo": true,
                            "fnCreatedRow": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                                $('td:eq(15)', nRow).append("<['SiteName'] = '" + aData[0] + "', [AE] = '" + aData[1] + "', ['SAE'] = '" + aData[2] + "'>");
                            },
                            "aoColumns": [
                                        { "sTitle": "Project/Site" },
                                        { "sTitle": "AE" },
                                        { "sTitle": "SAE" },
                            ],
                            "oLanguage": {
                                "sEmptyTable": "No Record Found",
                            },
                        });
                        $(".content").height($(".wrapper").height());
                        $('#tbl_AESAE').show();
                    }
                },
                failure: function (response) {
                    msgalert("failure");
                    msgalert(data.d);
                },
                error: function (response) {
                    msgalert("error");
                }
            });
        }
        function chart_selection() {

            var dd_chart = $($get('<%= dd_chart.ClientID%>')).val();

            if (dd_chart == "Pie") {
                pie();
            }
            else if (dd_chart == "Line") {
                Line();
            }
            else if (dd_chart == "Doughnut") {
                Doughnut();
            }
            else {
                msgalert("Please Select The Chart");
            }
        }

        var chart;

        function pie() {

            if (chart != null) {

                chart = new Highcharts.Chart({
                    chart: {
                        type: 'column',
                        renderTo: 'container'
                    },
                    title: {
                        text: 'Recruitment Status'
                    },
                    subtitle: {
                        text: 'Source: BizNet'
                    },
                    xAxis: {
                        title: {
                            text: 'Project/Site'
                        },
                        categories: [],
                        crosshair: true
                    },
                    yAxis: {
                        min: 0,
                        title: {
                            text: 'Subject'
                        }
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                        pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}:</td>' +
                            '<td style="padding:0"><b>{point.y:.0f}</b></td></tr>',
                        footerFormat: '</table>',
                        shared: true,
                        useHTML: true
                    },
                    plotOptions: {
                        column: {
                            pointPadding: 0.2,
                            borderWidth: 0
                        }
                    },
                    series: [{
                        name: 'Enrolled Subject',
                        data: []
                    },
                    {
                        name: 'Rejected Subject',
                        data: []
                    }
                    ]
                });
            }

            chart = new Highcharts.Chart({
                chart: {
                    type: 'column',
                    renderTo: 'container'
                },
                title: {
                    text: 'Recruitment Status'
                },
                subtitle: {
                    text: 'Source: BizNet'
                },
                xAxis: {
                    title: {
                        text: 'Project/Site'
                    },
                    categories: finalproject,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: 'Subject'
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}:</td>' +
                        '<td style="padding:0"><b>{point.y:.0f}</b></td></tr>',
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    }
                },
                series: [{
                    name: 'Enrolled Subject',
                    data: finalsubject
                },
                {
                    name: 'Rejected Subject',
                    data: Dfinalsubject
                }
                ]
            });

            function clear() {
                var chart = $('#container').highcharts();
                var seriesLength = chart.series.length;
                for (var i = seriesLength - 1; i > -1; i--) {
                    chart.series[i].data([])
                    chart.series[i].remove();
                }
            }

            $('.remove').click(function () {
                var chart = $('#container').highcharts();
                var seriesLength = chart.series.length;
                for (var i = seriesLength - 1; i > -1; i--) {
                    font
                    //chart.series[i].datasetFill();
                    chart.series[i].data([])
                    chart.series[i].remove();
                    //chart.series[i].DataTable([]);                   
                    //showmodalpopup();
                }
                //this.disabled = true;
            });
            $(".content").height($(".wrapper").height());
        }

        function Line() {

            if (chart != null) {
                var chart = new Highcharts.Chart({
                    chart: {
                        renderTo: 'container'
                    },
                    title: {
                        text: 'Recruitment Status',
                        x: -20
                    },
                    subtitle: {
                        text: 'Source: BizNet',
                        x: -20
                    },
                    xAxis: {
                        title: {
                            text: 'Project/Site'
                        },
                        categories: []
                    },
                    yAxis: {
                        title: {
                            text: 'Subject'
                        },
                        plotLines: [{
                            value: 0,
                            width: 1,
                            color: '#808080'
                        }]
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                        pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}:</td>' +
                            '<td style="padding:0"><b>{point.y:.0f}</b></td></tr>',
                        footerFormat: '</table>',
                        shared: true,
                        useHTML: true
                    },
                    legend: {
                        layout: 'vertical',
                        align: 'right',
                        verticalAlign: 'middle',
                        borderWidth: 0
                    },
                    series: [{
                        name: 'Enrolled Subject',
                        data: []
                    }, {
                        name: 'Rejected Subject',
                        data: []
                    }]
                });
            }

            $(function () {

                var chart = new Highcharts.Chart({
                    chart: {
                        renderTo: 'container'
                    },
                    title: {
                        text: 'Recruitment Status',
                        x: -20
                    },
                    subtitle: {
                        text: 'Source: BizNet',
                        x: -20
                    },
                    xAxis: {
                        title: {
                            text: 'Project/Site'
                        },
                        categories: finalproject
                    },
                    yAxis: {
                        title: {
                            text: 'Subject'
                        },
                        plotLines: [{
                            value: 0,
                            width: 1,
                            color: '#808080'
                        }]
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                        pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}:</td>' +
                            '<td style="padding:0"><b>{point.y:.0f}</b></td></tr>',
                        footerFormat: '</table>',
                        shared: true,
                        useHTML: true
                    },
                    legend: {
                        layout: 'vertical',
                        align: 'right',
                        verticalAlign: 'middle',
                        borderWidth: 0
                    },
                    series: [{
                        name: 'Enrolled Subject',
                        data: finalsubject
                    }, {
                        name: 'Rejected Subject',
                        data: Dfinalsubject
                    }]
                });
                $(".content").height($(".wrapper").height());
            });
        }

        function Doughnut() {
            if (chart != null) {
                var chart = new Highcharts.Chart({
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false,
                        type: 'pie',
                        renderTo: 'container'
                    },
                    title: {
                        text: 'Recruitment Status'
                    },
                    subtitle: {
                        text: 'Source: BizNet',
                    },
                    tooltip: {
                        pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: false
                            },
                            showInLegend: true,
                        }
                    },
                    series: [{
                        name: 'Enrolled Subjects',
                        colorByPoint: true,
                        data: []
                    }]
                });
            }
            $(function () {
                $(document).ready(function () {
                    var chart = new Highcharts.Chart({
                        chart: {
                            plotBackgroundColor: null,
                            plotBorderWidth: null,
                            plotShadow: false,
                            type: 'pie',
                            renderTo: 'container'
                        },
                        title: {
                            text: 'Recruitment Status'
                        },
                        subtitle: {
                            text: 'Source: BizNet',
                        },
                        tooltip: {
                            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                        },
                        plotOptions: {
                            pie: {
                                allowPointSelect: true,
                                cursor: 'pointer',
                                dataLabels: {
                                    enabled: false
                                },
                                showInLegend: true,
                            }
                        },
                        series: [{
                            name: 'Enrolled Subjects',
                            colorByPoint: true,
                            data: donutFinal
                        }]
                    });
                    $(".content").height($(".wrapper").height());
                });
            });
        }
        /*Dhruvi CRF DATA STATUS  ............................................................................................................................*/
        var TrialDemo = '';
        var vBoolId = '';
        var summarydata = '';
        var input_chart = [];
        var input_chart1 = [];
        var in_chart = [];
        var donutinput1 = '';
        var in_chartDEP = [];
        var in_chartFinalReview = [];
        var in_chartFinalFreezeDone = [];
        var in_chartSecondReviewDone = [];
        var in_chartDCF = [];
        var donutinputf1 = [];
        var donutinputf2 = [];
        var donutinputf3 = [];
        var donutinputf4 = [];
        var donutinputf5 = [];
        //var dnf1 = [];
        //var dnf2 = [];
        //var dnf3 = [];
        //var dnf4 = [];
        //var dnf5 = [];




        function fnNewDemo() {
            $('#loadingmessage').show();
            GloabalWorkspaceId = $("#<%=ProjectWorkSpaceIdDemo.ClientID%>").val();
            var projectval = $("#<%=txtDemo.ClientID%>").val();
            if (projectval.length == '') {
                msgalert(" Please, Select Project first");
                $('#loadingmessage').hide();
                return false;
            }
            $('#demotab').show();
            document.getElementById('ctl00_CPHLAMBDA_dd1_chart').disabled = false;
            var CRFStatusdata = $("#<%=ProjectWorkSpaceIdDemo.ClientID%>").val();

            var vWorkSpaceId = CRFStatusdata;
            vBoolId = 00;
            fnStudyDetailDemo(vWorkSpaceId);
            fnGetData(vWorkSpaceId);
            $('#loadingmessage').hide();
        }

        function fnStudyDetailDemo(vWorkSpaceId) {
            $.ajax({
                type: "post",
                url: "frmMainPage.aspx/Proc_ProjectStudyDetail",
                data: '{"vWorkspaceId":"' + vWorkSpaceId + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                dataType: "json",
                success: function (data) {
                    var data
                    data = data.d
                    var msgs = [];
                    msgs = JSON.parse(data);
                    if (msgs == null) {
                        //alert("No Data Available")
                    }
                    else {
                        var study_no = null, study_type = null, study_status = null;
                        for (var i = 0, l = msgs.Table.length; i < l; i++) {
                            Studynodemo = msgs.Table[i].Project_Name;
                            Studytypedemo = msgs.Table[i].Project_Type;
                            Studystatusdemo = msgs.Table[i].Project_Status;
                            Studyphasedemo = msgs.Table[i].Project_Phase_Completed;
                            $('#<%= Studynodemoresult.ClientID%>').text(Studynodemo);
                            $('#<%= Studytypedemoresult.ClientID%>').text(Studytypedemo);
                            $('#<%= Studystatusdemoresult.ClientID%>').text(Studystatusdemo);
                            $('#<%= Studyphasedemo.ClientID%>').text(Studyphasedemo);
                        }
                        $(".content").height($(".wrapper").height());
                    }
                },
                failure: function (response) {
                    msgalert("failure");
                    msgalert(data.d);
                },
                error: function (response) {
                    msgalert("error");
                }
            });
        }

        function fnGetData(vWorkSpaceId) {
            var isChild
            if ($('#ctl00_CPHLAMBDA_chkCRFDataStatusParentProject').is(":checked") == true) {
                isChild = "Y"
            }
            else {
                isChild = "N"
            }

            $.ajax({
                type: "post",
                url: "frmMainPage.aspx/Proc_GetActivityStatusCountRecords",
                data: '{"vWorkSpaceId":"' + vWorkSpaceId + '","vBoolId":"' + vBoolId + '","isChild":"' + isChild + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                dataType: "json",
                success: function (data) {
                    var data = data.d;
                    var msgs = JSON.parse(data);
                    summarydata = msgs;
                    if (summarydata == "") {
                        document.getElementById('ctl00_CPHLAMBDA_btnExportDataStatus').style.display = "none";
                        return false;
                    }
                    CreateSummaryTable(summarydata);
                    CreateSummaryChart(summarydata);
                    document.getElementById('ctl00_CPHLAMBDA_btnExportDataStatus').style.display = "Inline";
                },
                failure: function (response) {
                    msgalert("failure");
                    msgalert(data.d);
                },
                error: function (response) {
                    msgalert("error");
                }
            });

        }

        function CreateSummaryTable(summarydata) {

            var ActivityDataset = [];
            for (var Row = 0; Row < summarydata.Table.length; Row++) {
                var InDataset = [];
                InDataset.push(summarydata.Table[Row]['Project/Site Id.'], summarydata.Table[Row]['No. Of Subject'], summarydata.Table[Row]['Data Entry Pending'],
                               summarydata.Table[Row]['Data Entry Continue'], summarydata.Table[Row]['Ready For Review'], summarydata.Table[Row]['First Review Done'],
                               summarydata.Table[Row]['Second Review Done'], summarydata.Table[Row]['Final Reviewed & Freeze'], summarydata.Table[Row]['Generated DCF'],
                               summarydata.Table[Row]['Answered DCF'], summarydata.Table[Row]['Total DCF']);
                ActivityDataset.push(InDataset);
            }
            $ = jQuery;
            $('#demotab').dataTable().fnDestroy();
            otable = $('#demotab').DataTable({

                "bJQueryUI": true,
                "scrollX": true,
                "sPaginationType": "full_numbers",
                "pageLength": '5',
                "bLengthChange": true,
                "aLengthMenu": [[3, 5, 10, -1], [3, 5, 10, "All"]],
                "stateSave": true,
                "bProcessing": true,
                "bSort": false,
                "autoWidth": false,
                "aaData": ActivityDataset,
                "bInfo": true,
                "iDisplayLength": 5,
                "fnCreatedRow": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    $('td:eq(15)', nRow).append("<['Project/Site Id.'] = '" + aData[0] + "', ['No. Of Subject'] = '" + aData[1] + "', ['Data Entry Pending'] = '" + aData[2] + "', ['Data Entry Continue'] = '" + aData[3] + "', ['Ready For Review'] = '" + aData[4] + "', ['First Review Done'] = '" + aData[5] + "', ['Second Review Done'] ='" + aData[6] + "', ['Final Reviewed & Freeze'] = '" + aData[7] + "', ['Generated DCF'] = '" + aData[8] + "', ['Answered DCF'] = '" + aData[9] + "', ['Total DCF'] = '" + aData[10] + "'>");
                },
                "aoColumns": [
                            { "sTitle": "Project" },
                            { "sTitle": "No.Of Sub" },
                            { "sTitle": "Data Entry Pend" },
                            { "sTitle": "Data Entry Cont" },
                            { "sTitle": "Ready For Review" },
                            { "sTitle": "First Review Done" },
                            { "sTitle": "Second Review Done" },
                            { "sTitle": "Final Review & Freeze" },
                            { "sTitle": "Generated DCF" },
                            { "sTitle": "Answered DCF" },
                            { "sTitle": "Total DCF" },
                ],
                "columns": [
                    { "width": "5%", "targets": 0 },
                    null, null, null, null, null, null, null, null, null, null,
                ],
                "oLanguage": {
                    "sEmptyTable": "No Record Found",
                },
            });
            $(".content").height($(".wrapper").height());
            $('#demotab').show();
        }

        function CreateSummaryChart(summarydata) {
            //debugger;
            var ddlColumnList = $($get('<%= ddlColumnList.ClientID()%>'));
            var ddlColumnListVal = $(ddlColumnList).val();
            var AllDataset = [];
            //var in_chart = [];
            // var in_chart1 = [];
            //var Suballdata = ["Data Entry Pending", "SDV Pending", "DM Review Pending", "DM Review Done", "System Genrated DCF"];
            var Suballdata = ["Data Entry Pending", "SDV Pending", "DM Review Pending", "DM Review Done"];
            var Dataview = $("#ctl00_CPHLAMBDA_ddlColumnList").find('option')
            var Trialview = Dataview.length;
            DataNeed();
            for (var i = 0 ; i < Trialview ; i++) {
                AllDataset.push($($("#ctl00_CPHLAMBDA_ddlColumnList").find('option')[i]).text());
            }
            if (ddlColumnListVal == "Data Entry Pending") {
                DPending();
            }
            else if (ddlColumnListVal == "SDV Pending") {

                SDPending();
            }
            else if (ddlColumnListVal == "DM Review Pending") {
                DMPending();
            }
            else if (ddlColumnListVal == "DM Review Done") {
                DMDone();
            }
            else if (ddlColumnListVal == "All") {
                ViewAllChart();
            }
        }

        function DataNeed() {
            ActivityDataset = [];
            finalproject1 = [];
            finaldep = [];
            finaldecon = [];
            finalrrevie = [];
            finalfrevdone = [];
            finalsrevdone = [];
            finalgdcf = [];
            donutinput = [];
            donutinput1 = [];



            for (var Row = 0; Row < summarydata.Table.length; Row++) {
                var InDataset = [];
                InDataset.push(summarydata.Table[Row]['Project/Site Id.'], summarydata.Table[Row]['No. Of Subject'], summarydata.Table[Row]['Data Entry Pending'],
                               summarydata.Table[Row]['Data Entry Continue'], summarydata.Table[Row]['Ready For Review'], summarydata.Table[Row]['First Review Done'],
                               summarydata.Table[Row]['Second Review Done'], summarydata.Table[Row]['Final Reviewed & Freeze'], summarydata.Table[Row]['Generated DCF'],
                               summarydata.Table[Row]['Answered DCF'], summarydata.Table[Row]['Total DCF']);
                ActivityDataset.push(InDataset);
            }

            for (var i = 0, l = summarydata.Table.length; i < l; i++) {
                //var msg = summarydata[i];
                finalproject1.push(summarydata.Table[i]['Project/Site Id.']);
                finaldep.push(summarydata.Table[i]['Data Entry Pending']);
                finaldecon.push(summarydata.Table[i]['Data Entry Continue']);
                finalrrevie.push(summarydata.Table[i]['Ready For Review']);
                finalfrevdone.push(summarydata.Table[i]['First Review Done']);
                finalsrevdone.push(summarydata.Table[i]['Second Review Done']);
                finalgdcf.push(summarydata.Table[i]['Generated DCF']);
            }

            donutinput.push(finaldep);
            donutinput.push(finalrrevie);
            donutinput.push(finalfrevdone);
            donutinput.push(finalsrevdone);
            donutinput.push(finalgdcf);

        }

        function DPending() {
            in_chart.push(finaldep);
            input_chart = in_chart[0];
            for (var i = 0, l = summarydata.Table.length; i < l; i++) {
                donutinput1.push({ name: finalproject1[i], y: input_chart[i] });
            }
            pie1();
            in_chart = [];
        }

        function SDPending() {
            in_chart.push(finalrrevie);
            input_chart = in_chart[0];
            for (var i = 0, l = summarydata.Table.length; i < l; i++) {
                donutinput1.push({ name: finalproject1[i], y: input_chart[i] });
            }

            pie1();
            in_chart = [];
        }

        function DMPending() {
            in_chart.push(finalfrevdone);
            input_chart = in_chart[0];
            for (var i = 0, l = summarydata.Table.length; i < l; i++) {
                donutinput1.push({ name: finalproject1[i], y: input_chart[i] });
            }
            pie1();
            in_chart = [];
        }

        function DMDone() {
            in_chart.push(finalsrevdone);
            input_chart = in_chart[0];
            for (var i = 0, l = summarydata.Table.length; i < l; i++) {
                donutinput1.push({ name: finalproject1[i], y: input_chart[i] });
            }
            pie1();
            in_chart = [];
        }

        function SysDCF() {
            in_chart.push(finalgdcf);
            input_chart = in_chart[0];
            for (var i = 0, l = summarydata.Table.length; i < l; i++) {
                donutinput1.push({ name: finalproject1[i], y: input_chart[i] });
            }
            pie1();
            in_chart = [];
        }

        function ViewAllChart() {
            in_chartDEP = [];
            in_chartFinalReview = [];
            in_chartFinalFreezeDone = [];
            in_chartSecondReviewDone = [];
            in_chartDCF = [];
            in_chartDEP.push(finaldep);
            in_chartDEP[0];
            donutinputf1 = [];
            donutinputf2 = [];
            donutinputf3 = [];
            donutinputf4 = [];
            donutinputf5 = [];

            for (var i = 0, l = summarydata.Table.length; i < l; i++) {
                donutinputf1.push({ name: finalproject1[i], y: in_chartDEP[0][i] });
            }
            // dnf1 = donutinputf1;
            in_chartFinalReview.push(finalrrevie);
            in_chartFinalReview[0];
            for (var i = 0, l = summarydata.Table.length; i < l; i++) {
                donutinputf2.push({ name: finalproject1[i], y: in_chartFinalReview[0][i] });
            }
            // dnf2 = donutinputf2;
            in_chartFinalFreezeDone.push(finalfrevdone);
            in_chartFinalFreezeDone[0];
            for (var i = 0, l = summarydata.Table.length; i < l; i++) {
                donutinputf3.push({ name: finalproject1[i], y: in_chartFinalFreezeDone[0][i] });
            }
            //  dnf3 = donutinputf3;
            in_chartSecondReviewDone.push(finalsrevdone);
            in_chartSecondReviewDone[0];
            for (var i = 0, l = summarydata.Table.length; i < l; i++) {
                donutinputf4.push({ name: finalproject1[i], y: in_chartSecondReviewDone[0][i] });
            }
            //dnf4 = donutinputf4;
            in_chartDCF.push(finalgdcf);
            in_chartDCF[0];
            for (var i = 0, l = summarydata.Table.length; i < l; i++) {
                donutinputf5.push({ name: finalproject1[i], y: in_chartDCF[0][i] });
            }
            // dnf5 = donutinputf5;
            Allpie1();

        }

        function chartN_selection() {


            var ddlColumnList = $($get('<%= ddlColumnList.ClientID%>')).val();
            var dd1_chart = $($get('<%= dd1_chart.ClientID%>')).val();

            TrialDemo = ddlColumnList;
            if (dd1_chart == "Pie" && ddlColumnList == "Data Entry Pending" || dd1_chart == "Pie" && ddlColumnList == "SDV Pending" || dd1_chart == "Pie" && ddlColumnList == "DM Review Pending" || dd1_chart == "Pie" && ddlColumnList == "DM Review Done" || dd1_chart == "Pie" && ddlColumnList == "Data Entry Pending") {
                //|| dd1_chart == "Pie" && ddlColumnList == "System Genrated DCF"
                pie1();
            }
            else if (dd1_chart == "Line" && ddlColumnList == "Data Entry Pending" || dd1_chart == "Line" && ddlColumnList == "SDV Pending" || dd1_chart == "Line" && ddlColumnList == "DM Review Pending" || dd1_chart == "Line" && ddlColumnList == "DM Review Done" || dd1_chart == "Line" && ddlColumnList == "Data Entry Pending") {
                //|| dd1_chart == "Line" && ddlColumnList == "System Genrated DCF"
                Line1();
            }
            else if (dd1_chart == "Doughnut" && ddlColumnList == "Data Entry Pending" || dd1_chart == "Doughnut" && ddlColumnList == "SDV Pending" || dd1_chart == "Doughnut" && ddlColumnList == "DM Review Pending" || dd1_chart == "Doughnut" && ddlColumnList == "DM Review Done" || dd1_chart == "Doughnut" && ddlColumnList == "Data Entry Pending") {
                //|| dd1_chart == "Doughnut" && ddlColumnList == "System Genrated DCF"
                Doughnut1();
            }
            else if (dd1_chart == "Line" && ddlColumnList == "All") {
                AllLine1();
            }
            else if (dd1_chart == "Pie" && ddlColumnList == "All") {
                Allpie1();
            }
            else if (dd1_chart == "Doughnut" && ddlColumnList == "All") {
                Doughnut2();
            }

            else if (ddlColumnList == "Select Type") {
                msgalert("Please Select The Type!");
                $($get('<%= dd1_chart.ClientID%>')).val("Select Type");
            }
}

var chartN;

function pie1() {

    if (chartN != null) {


        var $container = $('#containermain').append('<div>');
        window.chart = new Highcharts.Chart({
            chart: {

                height: 500,
                renderTo: $container[0],
                type: 'column',
                margin: 92,
                options3d: {
                    enabled: true,
                    alpha: 10,
                    beta: 25,
                    depth: 70
                }


            },

            title: {
                text: 'CRF Data View'
            },
            subtitle: {
                text: 'Source: BizNETCTM'
            },
            xAxis: {
                title: {
                    text: 'Project/Site'
                },
                categories: [],
                crosshair: true
            },
            yAxis: {

                title: {
                    text: 'Data'
                }
            },

            tooltip: {
                enabled: true
            },
            plotOptions: {
                //column: {
                //    pointPadding: 0.2,
                //    //borderWidth: 0,
                //    depth: 70
                //},
                //series: {
                //    dataLabels: {
                //        enabled: true
                //    }
                //}
                column: {
                    pointPadding: 0.2,
                    borderWidth: 0
                },
                series: {
                    dataLabels: {
                        enabled: true
                    }
                }
            },
            credits: {
                enabled: false
            },

            series: [{

                name: 'Enrolled Subject',

                data: []

            }
            //  {
                //  name: 'Rejected Subject',
                //data: []
            //}
            ]
        });
    }



    $(function () {
        var $container = $('#containermain').append('<div>');
        window.chart = new Highcharts.Chart({
            chart: {
                height: 500,
                renderTo: $container[0],
                type: 'column',
                margin: 92,
                options3d: {
                    enabled: true,
                    alpha: 10,
                    beta: 25,
                    depth: 70
                }
            },

            title: {
                text: 'CRF Data View'
            },
            subtitle: {
                text: 'Source: BizNETCTM'
            },
            xAxis: {
                title: {
                    text: 'Project/Site'
                },
                categories: finalproject1,
                crosshair: true
            },
            yAxis: {

                title: {
                    text: 'Data'
                }
            },
            tooltip: {
                enabled: true
            },
            plotOptions: {
                column: {
                    //pointPadding: 0.2,
                    //borderWidth: 0,
                    depth: 70
                },
                series: {
                    dataLabels: {
                        enabled: true
                    }
                }
            },
            credits: {
                enabled: false
            },
            series: [{
                name: $($get('<%= ddlColumnList.ClientID%>')).val(),
                data: input_chart

            }
                      //  {
                        //    name: 'Rejected Subject',
                          //  data: Dfinalsubject
                      //  }
            ]
        });
    })
    }
    function Line1() {
        var $container = $('#containermain').append('<div>');
        if (chartN != null) {
            window.chart = new Highcharts.Chart({
                chart: {
                    height: 500,
                    renderTo: $container[0]
                },
                title: {
                    text: 'CRF Data view',
                    x: -20
                },
                subtitle: {
                    text: 'Source: BizNETCTM',
                    x: -20
                },
                xAxis: {
                    title: {
                        text: 'Project/Site'
                    },
                    categories: []
                },
                yAxis: {
                    title: {
                        text: 'Data'
                    },
                    tooltip: {
                        enabled: true
                    },
                    plotLines: [{
                        value: 0,
                        width: 1,
                        color: '#808080'
                    }]
                },
                plotOptions: {

                    series: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle',
                    borderWidth: 0
                },
                credits: {
                    enabled: false
                },
                series: [{
                    name: 'Enrolled Subject',
                    data: []
                },
                //{
                  //  name: 'Rejected Subject',
                    //data: []
                //}
                ]
            });
        }

        $(function () {
            var $container = $('#containermain').append('<div>');
            window.chart = new Highcharts.Chart({
                chart: {
                    height: 500,
                    renderTo: $container[0]
                },
                title: {
                    text: 'CRF Data Status',
                    x: -20
                },
                subtitle: {
                    text: 'Source: BizNETCTM',
                    x: -20
                },
                xAxis: {
                    title: {
                        text: 'Project/Site'
                    },
                    categories: finalproject1
                },
                yAxis: {
                    title: {
                        text: 'Data'
                    },
                    plotLines: [{
                        value: 0,
                        width: 1,
                        color: '#808080'
                    }]
                },
                plotOptions: {

                    series: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle',
                    borderWidth: 0
                },
                credits: {
                    enabled: false
                },
                series: [{
                    name: TrialDemo,
                    data: input_chart
                }
                //{
                  //  name: 'Rejected Subject',
                    //data: Dfinalsubject
                //}
                ]
            });
            $(".content").height($(".wrapper").height());
        });
    }

    function Doughnut1() {
        if (chartN != null) {
            var $container = $('#containermain').append('<div>');
            window.chart = new Highcharts.Chart({
                chart: {
                    height: 500,
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie',
                    renderTo: $container[0]
                },
                title: {
                    text: 'CRF Data view'
                },
                subtitle: {
                    text: 'Source: BizNETCTM',
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            distance: -60,
                            format: '<b>{point.name}</b>: {point.y:.1f}, <b>{point.percentage:.1f}%</b> ',
                            style: {
                                color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                            }
                        },
                        showInLegend: true,
                    },

                },
                labels: {
                    style: {
                        color: '#3E576F',
                        fontsize: '14px'
                    },
                    items: [{
                        html: $($get('<%= ddlColumnList.ClientID%>')).val(),
                        style: {
                            left: '10px',
                            top: '40px'
                        }
                    }]
                },
                credits: {
                    enabled: false
                },
                series: [{
                    name: 'Enrolled Subjects',
                    colorByPoint: true,
                    data: []
                }]
            });
            }
            $(function () {
                var $container = $('#containermain').append('<div>');
                $(document).ready(function () {
                    window.chart = new Highcharts.Chart({
                        chart: {
                            height: 500,
                            plotBackgroundColor: null,
                            plotBorderWidth: null,
                            plotShadow: false,
                            type: 'pie',
                            renderTo: $container[0]
                        },
                        title: {
                            text: 'CRF Data view'
                        },
                        subtitle: {
                            text: 'Source: BizNETCTM',
                        },
                        tooltip: {
                            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                        },
                        plotOptions: {
                            pie: {
                                allowPointSelect: true,
                                cursor: 'pointer',
                                dataLabels: {
                                    enabled: true,
                                    format: '<b>{point.name}</b>: {point.y:.1f}, <b>{point.percentage:.1f}%</b> ',
                                    style: {
                                        color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                                    }
                                },
                                showInLegend: true,
                            },

                        },
                        labels: {
                            style: {
                                color: '#3E576F',
                                "font-size": '12px',
                                "font-weight": 'bold'

                            },
                            items: [{
                                html: $($get('<%= ddlColumnList.ClientID%>')).val(),
                                style: {
                                    left: '10px',
                                    top: '20px'
                                }
                            }]
                        },
                        credits: {
                            enabled: false
                        },
                        series: [{
                            name: $($get('<%= ddlColumnList.ClientID%>')).val(),
                            colorByPoint: true,
                            data: donutinput1
                        }]
                    });
                    $(".content").height($(".wrapper").height());
                });
            });
            }
            function Allpie1() {

                if (chartN != null) {


                    var $container = $('#containermain').append('<div>');
                    window.chart = new Highcharts.Chart({
                        chart: {
                            height: 800,
                            width: 850,
                            type: 'column',
                            renderTo: $container[0],
                            margin: 75,
                            options3d: {
                                enabled: true,
                                alpha: 100,
                                beta: 250,
                                depth: 750
                            }


                        },
                        title: {
                            text: 'CRF Data view'
                        },
                        subtitle: {
                            text: 'Source: BizNETCTM'
                        },
                        xAxis: {
                            title: {
                                text: 'Project/Site'
                            },
                            categories: [],
                            crosshair: true
                        },
                        yAxis: {
                            min: 0,
                            title: {
                                text: 'Data'
                            }
                        },
                        tooltip: {
                            enabled: true
                        },
                        plotOptions: {
                            column: {
                                pointPadding: 0.2,
                                borderWidth: 0,
                                depth: 25
                            },
                            series: {
                                dataLabels: {
                                    enabled: true
                                }
                            }
                        },
                        credits: {
                            enabled: false
                        },
                        series: [{
                            name: 'Enrolled Subject',

                            data: []
                        }
                        //  {
                            //  name: 'Rejected Subject',
                            //data: []
                        //}
                        ]
                    });
                }



                $(function () {
                    var $container = $('#containermain').append('<div>');
                    window.chart = new Highcharts.Chart({
                        chart: {
                            height: 500,
                            width: 850,
                            type: 'column',
                            renderTo: $container[0],
                            margin: 92,
                            options3d: {
                                enabled: true,
                                alpha: 10,
                                beta: 25,
                                depth: 50
                            }
                        },
                        title: {
                            text: 'CRF Data view'
                        },
                        subtitle: {
                            text: 'Source: BizNETCTM'
                        },
                        xAxis: {
                            title: {
                                text: 'Project/Site'
                            },
                            categories: finalproject1,
                            crosshair: true,
                            margin: 100,
                        },
                        yAxis: {
                            min: 0,
                            title: {
                                text: 'Data'
                            }
                        },
                        tooltip: {
                            enabled: true
                        },
                        plotOptions: {
                            column: {
                                pointPadding: 0.2,
                                borderWidth: 0
                            },
                            series: {
                                dataLabels: {
                                    enabled: true
                                }
                            }
                        },
                        credits: {
                            enabled: false
                        },
                        series: [{
                            name: 'Data Entry Pending',
                            data: in_chartDEP[0]
                        },
                            {
                                name: 'SDV Pending',
                                data: in_chartFinalReview[0]
                            },
                            {
                                name: 'DM Review Pending',
                                data: in_chartFinalFreezeDone[0]
                            },
                            {
                                name: 'DM Review Done',
                                data: in_chartSecondReviewDone[0]
                            },
                            //{
                            //    name: 'System Genrated DCF',
                            //    data: in_chartDCF[0]
                            //}

                    //  {
                    //    name: 'Rejected Subject',
                        //  data: Dfinalsubject
                    //  }
                        ]
                    });

                })

            }

            function AllLine1() {
                var $container = $('#containermain').append('<div>');
                if (chartN != null) {
                    window.chart = new Highcharts.Chart({
                        chart: {
                            height: 500,
                            Width: 1000,
                            renderTo: $container[0]
                        },
                        title: {
                            text: 'CRF Data view',
                            x: -20
                        },
                        subtitle: {
                            text: 'Source: BizNETCTM',
                            x: -20
                        },
                        xAxis: {
                            title: {
                                text: 'Project/Site'
                            },
                            categories: []
                        },
                        yAxis: {
                            title: {
                                text: 'Data'
                            },
                            plotLines: [{
                                value: 0,
                                width: 1,
                                color: '#808080'
                            }]
                        },
                        plotOptions: {

                            series: {
                                dataLabels: {
                                    enabled: true
                                }
                            }
                        },
                        legend: {
                            layout: 'vertical',
                            align: 'right',
                            verticalAlign: 'middle',
                            borderWidth: 0
                        },
                        credits: {
                            enabled: false
                        },
                        series: [{
                            name: 'Enrolled Subject',
                            data: []
                        },
                        //{
                            //  name: 'Rejected Subject',
                            //data: []
                        //}
                        ]
                    });
                }

                $(function () {
                    var $container = $('#containermain').append('<div>');
                    window.chart = new Highcharts.Chart({
                        chart: {
                            height: 500,
                            renderTo: $container[0]
                        },
                        title: {
                            text: 'CRF Data view',
                            x: -20
                        },
                        subtitle: {
                            text: 'Source: BizNETCTM',
                            x: -20
                        },
                        xAxis: {
                            title: {
                                text: 'Project/Site'
                            },
                            categories: finalproject1
                        },
                        yAxis: {
                            title: {
                                text: 'Data'
                            },
                            plotLines: [{
                                value: 0,
                                width: 1,
                                color: '#808080'
                            }]
                        },
                        plotOptions: {

                            series: {
                                dataLabels: {
                                    enabled: true
                                }
                            }
                        },
                        legend: {
                            layout: 'vertical',
                            align: 'right',
                            verticalAlign: 'middle',
                            borderWidth: 0
                        },
                        credits: {
                            enabled: false
                        },
                        series: [{
                            name: 'Data Entry Pending',
                            data: in_chartDEP[0]
                        },
                            {
                                name: 'SDV Pending',
                                data: in_chartFinalReview[0]
                            },
                            {
                                name: 'DM Review Pending',
                                data: in_chartFinalFreezeDone[0]
                            },
                            {
                                name: 'DM Review Done',
                                data: in_chartSecondReviewDone[0]
                            },
                            //{
                            //    name: 'System Genrated DCF',
                            //    data: in_chartDCF[0]
                            //}
                        ]
                    });
                    $(".content").height($(".wrapper").height());
                });
            }

            function Doughnut2() {
                if (chartN != null) {
                    var $container = $('#containermain').append('<div>');
                    window.chart = new Highcharts.Chart({
                        chart: {
                            height: 600,
                            width: 1000,
                            plotBackgroundColor: null,
                            plotBorderWidth: 1,//null,
                            plotShadow: false,
                            renderTo: $container[0]
                        },
                        title: {
                            text: 'CRF Data view'
                        },
                        tooltip: {
                            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                        },
                        plotOptions: {
                            pie: {
                                allowPointSelect: true,
                                cursor: 'pointer',
                                dataLabels: {
                                    enabled: true,
                                    distance: -20,
                                    //format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                                    format: '<b>{point.name}</b>: {point.y:.1f}, <b>{point.percentage:.1f}%</b> ',
                                    // format: ' {point.percentage:.1f} %',
                                    style: {
                                        color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                                    }
                                },
                                showInLegend: true,
                            },
                        },
                        labels: {
                            style: {
                                color: '#3E576F',
                                fontsize: '14px'
                            },
                            items: [{
                                html: 'Data Entry Pending',
                                style: {
                                    center: '50px',
                                    top: '40px'
                                }
                            },
                                {
                                    html: 'SDV Pending',
                                    style: {
                                        left: '120px',
                                        top: '60px'
                                    }
                                },
                                {
                                    html: 'DM Review Pending',
                                    style: {
                                        left: '210px',
                                        top: '40px'
                                    }
                                },
                                {
                                    html: 'DM Review Done',
                                    style: {
                                        left: '320px',
                                        top: '60px'
                                    }
                                },
                                //{
                                //    html: 'System Genrated DCF',
                                //    style: {
                                //        left: '410px',
                                //        top: '40px'
                                //    }
                                //}

                            ]
                        },
                        credits: {
                            enabled: false
                        },
                        series: [{
                            type: 'pie',
                            name: 'Data Entry Pending',
                            center: [50, null],
                            size: 100,
                            dataLabels: {
                                enabled: true,
                                distance: -20
                            },
                            showInLegend: false,
                            data: []


                        },
                                {
                                    type: 'pie',
                                    name: 'SDV Pending',
                                    center: [250, 250],
                                    size: 100,
                                    dataLabels: {
                                        enabled: true
                                    },
                                    data: []


                                },
                        {
                            type: 'pie',
                            name: 'DM Review Pending',
                            center: [350, null],
                            size: 100,
                            dataLabels: {
                                enabled: true
                            },

                            data: []


                        },
                        {
                            type: 'pie',
                            name: 'DM Review Done',

                            center: [500, null],
                            size: 100,
                            dataLabels: {
                                enabled: true
                            },

                            data: []


                        },
                            //{
                            //    type: 'pie',
                            //    name: 'System Genrated DCF',
                            //    center: [650, null],
                            //    size: 100,
                            //    dataLabels: {
                            //        enabled: true
                            //    },
                            //    data: []
                            //}
                        ]
                    },
                        function (chart) {

                            $(chart.series[0].data).each(function (i, e) {
                                e.legendItem.on('click', function (event) {
                                    var legendItem = e.name;

                                    event.stopPropagation();

                                    $(chart.series).each(function (j, f) {
                                        $(this.data).each(function (k, z) {
                                            if (z.name == legendItem) {
                                                if (z.visible) {
                                                    z.setVisible(false);
                                                }
                                                else {
                                                    z.setVisible(true);
                                                }
                                            }
                                        });
                                    });

                                });
                            });
                        })
                }

                $(function () {
                    var $container = $('#containermain').append('<div>');
                    window.chart = new Highcharts.Chart({
                        chart: {
                            height: 800,
                            width: 1000,
                            plotBackgroundColor: null,
                            plotBorderWidth: 1,//null,
                            plotShadow: false,
                            renderTo: $container[0]
                        },
                        title: {
                            text: 'CRF Data view'
                        },
                        tooltip: {
                            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                        },
                        plotOptions: {
                            pie: {
                                allowPointSelect: true,
                                cursor: 'pointer',
                                dataLabels: {
                                    enabled: true,
                                    distance: -50,
                                    format: '<b>{point.name}</b>: {point.y:.1f}, <b>{point.percentage:.1f}%</b> ',
                                    style: {
                                        color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                                    }
                                }
                            }
                        },
                        labels: {
                            style: {
                                color: '#3E576F',
                                "font-size": '12px',
                                "font-weight": 'bold',
                            },
                            items:
                            [
                                {
                                    html: 'Data Entry Pending',
                                    style: {
                                        left: '200px',
                                        top: '200px'
                                    }
                                },
                                {
                                    html: 'SDV Pending',
                                    style: {
                                        left: '200px',
                                        top: '420px'
                                    }
                                },
                                {
                                    html: 'DM Review Pending',
                                    style: {
                                        left: '600px',
                                        top: '200px'
                                    }
                                },
                                {
                                    html: 'DM Review Done',
                                    style: {
                                        left: '600px',
                                        top: '420px'
                                    }
                                },
                                //{
                                //    html: 'System Genrated DCF',
                                //    style: {
                                //        left: '200px',
                                //        top: '650px'
                                //    }
                                //}
                            ]
                        },
                        credits: {
                            enabled: false
                        },
                        series: [{
                            type: 'pie',
                            name: 'Data Entry Pending',
                            center: [220, 100],
                            size: 100,
                            dataLabels: {
                                enabled: true,
                                distance: 20
                            },
                            showInLegend: false,
                            data: donutinputf1
                        },
                            {
                                type: 'pie',
                                name: 'SDV Pending',
                                center: [220, 300],
                                size: 100,
                                dataLabels: {
                                    enabled: true,
                                    distance: 20
                                },
                                data: donutinputf2
                            },
                            {
                                type: 'pie',
                                name: 'DM Review Pending',
                                center: [650, 100],
                                size: 100,
                                dataLabels: {
                                    enabled: true,
                                    distance: 20
                                },
                                data: donutinputf3
                            },
                            {
                                type: 'pie',
                                name: 'DM Review Done',
                                center: [650, 300],
                                size: 100,
                                dataLabels: {
                                    enabled: true,
                                    distance: 20
                                },
                                data: donutinputf4
                            },
                            //{
                            //    type: 'pie',
                            //    name: 'System Genrated DCF',
                            //    center: [220, 500],
                            //    size: 100,
                            //    dataLabels: {
                            //        enabled: true,
                            //        distance: 20
                            //    },

                            //    data: donutinputf5
                            //}
                        ]
                    }, function (chart) {

                        $(chart.series[0].data).each(function (i, e) {
                            e.legendItem.on('click', function (event) {
                                var legendItem = e.name;

                                event.stopPropagation();

                                $(chart.series).each(function (j, f) {
                                    $(this.data).each(function (k, z) {
                                        if (z.name == legendItem) {
                                            if (z.visible) {
                                                z.setVisible(false);
                                            }
                                            else {
                                                z.setVisible(true);
                                            }
                                        }
                                    });
                                });

                            });
                        });
                    }
                        );

                });
            }

            function getSelectedColumnList(ddlColumnList) {
                var vWorkSpaceId = $("#<%=ProjectWorkSpaceIdDemo.ClientID%>").val();
                if (vWorkSpaceId != GloabalWorkspaceId) return false;

                $('#loadingmessage').show();

                var ddlColumnList = $($get('<%= ddlColumnList.ClientID%>')).val();
                if (summarydata == "") {
                    $('#loadingmessage').hide();
                    return false;
                }

                if (ddlColumnList == "Data Entry Pending") {
                    var dd1_chart = $($get('<%= dd1_chart.ClientID%>')).val("Pie");
                    fnGetData(vWorkSpaceId);
                    chartN_selection();
                    $('#loadingmessage').hide();
                }
                else if (ddlColumnList == "SDV Pending") {
                    document.getElementById('ctl00_CPHLAMBDA_dd1_chart').disabled = false;
                    var dd1_chart = $($get('<%= dd1_chart.ClientID%>')).val("Pie");
                    fnGetData(vWorkSpaceId);
                    chartN_selection();
                }
                else if (ddlColumnList == "DM Review Pending") {
                    document.getElementById('ctl00_CPHLAMBDA_dd1_chart').disabled = false;
                    var dd1_chart = $($get('<%= dd1_chart.ClientID%>')).val("Pie");
                    fnGetData(vWorkSpaceId);
                    chartN_selection();
                }
                else if (ddlColumnList == "DM Review Done") {
                    document.getElementById('ctl00_CPHLAMBDA_dd1_chart').disabled = false;
                    var dd1_chart = $($get('<%= dd1_chart.ClientID%>')).val("Pie");
                    fnGetData(vWorkSpaceId);
                    chartN_selection();
                }
                else if (ddlColumnList == "System Genrated DCF") {
                    document.getElementById('ctl00_CPHLAMBDA_dd1_chart').disabled = false;
                    var dd1_chart = $($get('<%= dd1_chart.ClientID%>')).val("Pie");
                    //CreateSummaryTable(summarydata);
                    //CreateSummaryChart(summarydata);
                    fnGetData(vWorkSpaceId);
                    chartN_selection();
                }
                else if (ddlColumnList == "All") {
                    document.getElementById('ctl00_CPHLAMBDA_dd1_chart').disabled = false;
                    var dd1_chart = $($get('<%= dd1_chart.ClientID%>')).val("Pie");
                    //CreateSummaryTable(summarydata);
                    //CreateSummaryChart(summarydata);
                    fnGetData(vWorkSpaceId);
                    chartN_selection();
                }
                else {
                    msgalert("Please Select the Chart and type");
                }
    $('#loadingmessage').hide();
}
/*Dhruvi CRF DATA STATUS  Completed ............................................................................................................................*/
/*-------------Dhruvi Sitewise Data Status------------------------------------------------------------------------------------------------------------------*/
var summarydata1 = '';
var vBoolId = '';
var TrialDemo1 = '';
var input_chart1 = [];
var in_chart1 = [];
var totData = [];
var avaData = [];
var sdvData = [];
var dmData = [];
var finalselectedData = [];
var finalcolumns = [];
var finaldonutin = [];

function fnNewDemo1() {

    GloabalWorkspaceId = $("#<%=ProjectWorkSpaceIdDemo1.ClientID%>").val();
    var projectval = $("#<%=txtDemo1.ClientID%>").val();
    if (projectval.length == '') {
        msgalert(" Please, Select Project first");
        return false;
    }
    $('#demotab1').show();
    document.getElementById('ctl00_CPHLAMBDA_dd2_chart').disabled = false;
    var SiteData = $("#<%=ProjectWorkSpaceIdDemo1.ClientID%>").val();

    var vWorkSpaceId = SiteData;
    vBoolId = 999;
    fnStudyDetailDemo1(vWorkSpaceId);
    fnGetData1(vWorkSpaceId);
}
function fnStudyDetailDemo1(vWorkSpaceId) {
    $.ajax({
        type: "post",
        url: "frmMainPage.aspx/Proc_ProjectStudyDetail",
        data: '{"vWorkspaceId":"' + vWorkSpaceId + '"}',
        contentType: "application/json; charset=utf-8",
        datatype: JSON,
        async: false,
        dataType: "json",
        success: function (data) {
            var data
            data = data.d
            var msgs = [];
            msgs = JSON.parse(data);
            if (msgs == null) {
                //alert("No Data Available")
            }
            else {
                var study_no = null, study_type = null, study_status = null;
                for (var i = 0, l = msgs.Table.length; i < l; i++) {
                    Studynodemo1 = msgs.Table[i].Project_Name;
                    Studytypedemo1 = msgs.Table[i].Project_Type;
                    Studystatusdemo1 = msgs.Table[i].Project_Status;
                    Studyphasedemo1 = msgs.Table[i].Project_Phase_Completed;
                    $('#<%= Studynodemoresult1.ClientID%>').text(Studynodemo1);
                    $('#<%= Studytypedemoresult1.ClientID%>').text(Studytypedemo1);
                    $('#<%= Studystatusdemoresult1.ClientID%>').text(Studystatusdemo1);
                    $('#<%= Studyphasedemo1.ClientID%>').text(Studyphasedemo1);
                }
                $(".content").height($(".wrapper").height());
            }
        },
        failure: function (response) {
            msgalert("failure");
            msgalert(data.d);
        },
        error: function (response) {
            msgalert("error");
        }
    });
}
function fnGetData1(vWorkSpaceId) {
    var isChild
    if ($('#ctl00_CPHLAMBDA_chkSiteWiseParentProject').is(":checked") == true) {
        isChild = "Y"
    }
    else {
        isChild = "N"
    }


    $.ajax({
        type: "post",
        url: "frmMainPage.aspx/Proc_GetActivityStatusCountRecords",
        data: '{"vWorkSpaceId":"' + vWorkSpaceId + '","vBoolId":"' + vBoolId + '","isChild":"' + isChild + '"}',
        contentType: "application/json; charset=utf-8",
        datatype: JSON,
        async: false,
        dataType: "json",
        success: function (data) {
            var data = data.d;
            var msgs = JSON.parse(data);
            summarydata1 = msgs;
            if (summarydata1 == "") {
                document.getElementById('ctl00_CPHLAMBDA_btnExportSiteWiseData').style.display = "none";
                return false;
            }
            document.getElementById('ctl00_CPHLAMBDA_btnExportSiteWiseData').style.display = "Inline";
            CreateSummaryTable1(summarydata1);
            CreateSummaryChart1(summarydata1);
        },
        failure: function (response) {
            msgalert("failure");
            msgalert(data.d);
        },
        error: function (response) {
            msgalert("error");
        }
    });

}
function CreateSummaryTable1(summarydata1) {

    var ActivityDataset = [];
    for (var Row = 0; Row < summarydata1.Table.length; Row++) {
        var InDataset = [];
        InDataset.push(summarydata1.Table[Row]['Project/Site Id.'], summarydata1.Table[Row]['Total Expected Data'],
                       summarydata1.Table[Row]['Available Data'], summarydata1.Table[Row]['SDV Data'],
                       summarydata1.Table[Row]['Second Review Done']);
        ActivityDataset.push(InDataset);
    }
    $ = jQuery;
    $('#demotab1').dataTable().fnDestroy();
    $('#demotab1').DataTable({
        "bJQueryUI": true,
        "scrollX": true,
        "sPaginationType": "full_numbers",
        "pageLength": '5',
        "bLengthChange": true,
        "aLengthMenu": [[3, 5, 10, -1], [3, 5, 10, "All"]],
        "stateSave": true,
        "bProcessing": true,
        "bSort": false,
        "autoWidth": false,
        "aaData": ActivityDataset,
        "bInfo": true,
        "iDisplayLength": 5,
        "fnCreatedRow": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $('td:eq(15)', nRow).append("<['Project/Site Id.'] = '" + aData[0] + "', ['Total Expected Data'] = '" + aData[1] + "', ['Available Data'] = '" + aData[2] + "', ['SDV Data'] = '" + aData[3] + "', ['Second Review Done'] = '" + aData[4] + "'>");
        },
        "aoColumns": [
                    { "sTitle": "Project" },
                     { "sTitle": "Total Expected Data" },
                    { "sTitle": "Available Data" },
                    { "sTitle": "SDV Data" },
                    { "sTitle": "DM Reviewed" }
        ],
        "columns": [
            null, null, null, null, null, null, null, null, null, null, null, null, null
        ],
        "oLanguage": {
            "sEmptyTable": "No Record Found"
        },
    });
    $(".content").height($(".wrapper").height());
    $('#demotab1').show();
}
function CreateSummaryChart1(summarydata1) {

    var ddlColumnList1 = $($get('<%= ddlColumnList1.ClientID()%>'));
    var ddlColumnListVal1 = $(ddlColumnList1).val();
    var AllDataset = [];
    //var in_chart = [];
    // var in_chart1 = [];

    DataNeed1();

    if (ddlColumnListVal1 == "Total Expected Data") {
        TotalExpectedData();
    }
    else if (ddlColumnListVal1 == "Available Data") {

        AvailableData();
    }
    else if (ddlColumnListVal1 == "SDV Data") {
        SdvData();
    }
    else if (ddlColumnListVal1 == "DM Reviwed") {
        DMReviwed();
    }

    else if (ddlColumnListVal1 == "All") {
        ViewAllChart1()
    }

}
function DataNeed1() {
    ActivityDataset = [];
    finalproject2 = [];

    finalsrevdone1 = [];

    finalexpdata = [];
    finalreadydata = [];
    finalsdvdata = [];
    finalcolumns = [];


    for (var Row = 0; Row < summarydata1.Table.length; Row++) {
        var InDataset = [];
        InDataset.push(summarydata1.Table[Row]['Project/Site Id.'], summarydata1.Table[Row]['No. Of Subject'], summarydata1.Table[Row]['Data Entry Pending'], summarydata1.Table[Row]['Data Entry Continue'], summarydata1.Table[Row]['Ready For Review'], summarydata1.Table[Row]['First Review Done'], summarydata1.Table[Row]['Second Review Done'], summarydata1.Table[Row]['Final Reviewed & Freeze'], summarydata1.Table[Row]['Generated DCF'], summarydata1.Table[Row]['Answered DCF'], summarydata1.Table[Row]['Total DCF'], summarydata1.Table[Row]['Total Expected Data'], summarydata1.Table[Row]['Available Data'], summarydata1.Table[Row]['SDV Data']);
        ActivityDataset.push(InDataset);
    }
    for (var i = 0, l = summarydata1.Table.length; i < l; i++) {
        //var msg = summarydata[i];
        finalproject2.push(summarydata1.Table[i]['Project/Site Id.']);
        finalexpdata.push(summarydata1.Table[i]['Total Expected Data']);
        finalreadydata.push(summarydata1.Table[i]['Available Data']);
        finalsdvdata.push(summarydata1.Table[i]['SDV Data']);
        finalsrevdone1.push(summarydata1.Table[i]['Second Review Done']);

    }
    $('#ctl00_CPHLAMBDA_ddlsite').empty();
    $.each(finalproject2, function (i, p) {
        $('#ctl00_CPHLAMBDA_ddlsite').append($('<option></option>').val(p).html(p));
    });
    document.getElementById('ctl00_CPHLAMBDA_ddlsite').disabled = true;
    if (summarydata1.Table[0] == undefined) {
        finalcolumns.push(["Total Expected Data"]);
        finalcolumns.push(["Available Data"]);
        finalcolumns.push(["SDV Data"]);
        finalcolumns.push(["Second Review Done"]);

    }
    else {
        finalcolumns.push(Object.keys(summarydata1.Table[0])[11]);

        finalcolumns.push(Object.keys(summarydata1.Table[0])[12]);
        finalcolumns.push(Object.keys(summarydata1.Table[0])[13]);
        finalcolumns.push(Object.keys(summarydata1.Table[0])[06]);
    }
}

function TotalExpectedData() {

    in_chart1.push(finalexpdata);
    input_chart1 = in_chart1[0];

    pie2();
    in_chart1 = [];

}
function AvailableData() {

    in_chart1.push(finalreadydata);
    input_chart1 = in_chart1[0];

    pie2();
    in_chart1 = [];

}
function SdvData() {
    in_chart1.push(finalsdvdata);
    input_chart1 = in_chart1[0];

    pie2();
    in_chart1 = [];

}
function DMReviwed() {
    in_chart1.push(finalsrevdone1);
    input_chart1 = in_chart1[0];

    pie2();
    in_chart1 = [];

}

function ViewAllChart1() {
    totData = [];
    avaData = [];
    sdvData = [];
    dmData = [];

    totData.push(finalexpdata);
    totData[0];

    avaData.push(finalreadydata);
    avaData[0];

    sdvData.push(finalsdvdata);
    sdvData[0];

    dmData.push(finalsrevdone1);
    dmData[0];

    Allpie2();
}


var chartN;

function pie2() {
    if (chartN != null) {
        var $container = $('#containermain1').append('<div>');
        window.chart = new Highcharts.Chart({
            chart: {
                height: 500,
                renderTo: $container[0],
                type: 'column',
                margin: 97,
                options3d: {
                    enabled: true,
                    alpha: 10,
                    beta: 25,
                    depth: 70
                }
            },
            title: {
                text: 'Site Data at Glance'
            },
            subtitle: {
                text: 'Source: BizNETCTM'
            },
            xAxis: {
                title: {
                    text: 'Project/Site'
                },
                categories: [],
                crosshair: true
            },
            yAxis: {
                title: {
                    text: 'Data'
                }
            },
            tooltip: {
                enabled: true
            },
            plotOptions: {
                column: {
                    //pointPadding: 0.2,
                    //borderWidth: 0,
                    depth: 70
                },
                series: {
                    dataLabels: {
                        enabled: true
                    }
                }
            },
            credits: {
                enabled: false
            },
            series: [{
                name: 'Enrolled Subject',
                data: []
            }
            //  {
                //  name: 'Rejected Subject',
                //data: []
            //}
            ]
        });
    }

    $(function () {
        var $container = $('#containermain1').append('<div>');
        window.chart = new Highcharts.Chart({
            chart: {
                height: 500,
                renderTo: $container[0],
                type: 'column',
                margin: 97,
                options3d: {
                    enabled: true,
                    alpha: 10,
                    beta: 25,
                    depth: 70
                }
            },
            title: {
                text: 'Site Data at Glance'
            },
            subtitle: {
                text: 'Source: BizNETCTM'
            },
            xAxis: {
                title: {
                    text: 'Project/Site'
                },
                categories: finalproject2,
                crosshair: true
            },
            yAxis: {

                title: {
                    text: 'Data'
                }
            },
            tooltip: {
                enabled: true
            },
            plotOptions: {
                column: {
                    //pointPadding: 0.2,
                    //borderWidth: 0,
                    depth: 70
                },
                series: {
                    dataLabels: {
                        enabled: true
                    }
                }
            },
            credits: {
                enabled: false
            },
            series: [{
                name: $($get('<%= ddlColumnList1.ClientID%>')).val(),
                data: input_chart1

            }
                      //  {
                        //    name: 'Rejected Subject',
                          //  data: Dfinalsubject
                      //  }
            ]
        });



    })


    }
    function Line2() {
        var $container = $('#containermain1').append('<div>');
        if (chartN != null) {
            window.chart = new Highcharts.Chart({
                chart: {
                    height: 500,
                    renderTo: $container[0]
                },
                title: {
                    text: 'Site data at glance',
                    x: -20
                },
                subtitle: {
                    text: 'Source: BizNETCTM',
                    x: -20
                },
                xAxis: {
                    title: {
                        text: 'Project/Site'
                    },
                    categories: []
                },
                yAxis: {
                    title: {
                        text: 'Data'
                    },
                    tooltip: {
                        enabled: true
                    },
                    plotLines: [{
                        value: 0,
                        width: 1,
                        color: '#808080'
                    }]
                },
                plotOptions: {

                    series: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle',
                    borderWidth: 0
                },
                credits: {
                    enabled: false
                },
                series: [{
                    name: 'Enrolled Subject',
                    data: []
                },
                //{
                  //  name: 'Rejected Subject',
                    //data: []
                //}
                ]
            });
        }

        $(function () {
            var $container = $('#containermain1').append('<div>');
            window.chart = new Highcharts.Chart({
                chart: {
                    height: 500,
                    renderTo: $container[0]
                },
                title: {
                    text: 'Site data at glance',
                    x: -20
                },
                subtitle: {
                    text: 'Source: BizNETCTM',
                    x: -20
                },
                xAxis: {
                    title: {
                        text: 'Project/Site'
                    },
                    categories: finalproject2
                },
                yAxis: {
                    title: {
                        text: 'Data'
                    },
                    plotLines: [{
                        value: 0,
                        width: 1,
                        color: '#808080'
                    }]
                },
                plotOptions: {

                    series: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                },
                credits: {
                    enabled: false
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle',
                    borderWidth: 0
                },
                series: [{
                    name: TrialDemo1,
                    data: input_chart1
                }
                //{
                  //  name: 'Rejected Subject',
                    //data: Dfinalsubject
                //}
                ]
            });
            $(".content").height($(".wrapper").height());
        });
    }

    function Allpie2() {

        if (chartN != null) {


            var $container = $('#containermain1').append('<div>');
            window.chart = new Highcharts.Chart({
                chart: {
                    height: 500,
                    width: 850,
                    type: 'column',
                    renderTo: $container[0],
                    margin: 97,
                    options3d: {
                        enabled: true,
                        alpha: 100,
                        beta: 250,
                        depth: 750
                    }


                },
                title: {
                    text: 'Site Data at Glance'
                },
                subtitle: {
                    text: 'Source: BizNETCTM'
                },
                xAxis: {
                    title: {
                        text: 'Project/Site'
                    },
                    categories: [],
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: 'Data'
                    }
                },
                tooltip: {
                    enabled: true
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0,
                        depth: 25
                    },
                    series: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                },
                credits: {
                    enabled: false
                },

                series: [{
                    name: 'Enrolled Subject',

                    data: []
                }

                //  {
                    //  name: 'Rejected Subject',
                    //data: []
                //}
                ]
            });
        }



        $(function () {
            var $container = $('#containermain1').append('<div>');
            window.chart = new Highcharts.Chart({
                chart: {
                    height: 500,
                    width: 850,
                    type: 'column',
                    renderTo: $container[0],
                    margin: 97,
                    options3d: {
                        enabled: true,
                        alpha: 10,
                        beta: 25,
                        depth: 70
                    }
                },
                title: {
                    text: 'Site Data at Glance'
                },
                subtitle: {
                    text: 'Source: BizNETCTM'
                },
                xAxis: {
                    title: {
                        text: 'Project/Site'
                    },
                    categories: finalproject2,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: 'Data'
                    }
                },
                tooltip: {
                    enabled: true
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    },
                    series: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                },
                credits: {
                    enabled: false
                },
                series: [{
                    name: 'Total Expected Data',
                    data: totData[0]
                },
                 {
                     name: 'Available Data',
                     data: avaData[0]
                 },
                  {
                      name: 'SDV Data',
                      data: sdvData[0]

                  },
                  {
                      name: 'DM Reviewed',
                      data: dmData[0]
                  }

          //  {
            //    name: 'Rejected Subject',
              //  data: Dfinalsubject
          //  }
                ]
            });

        })

    }
    function AllLine2() {
        var $container = $('#containermain1').append('<div>');
        if (chartN != null) {
            window.chart = new Highcharts.Chart({
                chart: {
                    height: 500,
                    renderTo: $container[0]
                },
                title: {
                    text: 'Site Data at Glance',
                    x: -20
                },
                subtitle: {
                    text: 'Source: BizNETCTM',
                    x: -20
                },
                xAxis: {
                    title: {
                        text: 'Project/Site'
                    },
                    categories: []
                },
                yAxis: {
                    title: {
                        text: 'Data'
                    },
                    plotLines: [{
                        value: 0,
                        width: 1,
                        color: '#808080'
                    }]
                },
                plotOptions: {

                    series: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle',
                    borderWidth: 0
                },
                credits: {
                    enabled: false
                },
                series: [{
                    name: 'Enrolled Subject',
                    data: []
                },
                //{
                  //  name: 'Rejected Subject',
                    //data: []
                //}
                ]
            });
        }

        $(function () {
            var $container = $('#containermain1').append('<div>');
            window.chart = new Highcharts.Chart({
                chart: {
                    height: 500,
                    renderTo: $container[0]
                },
                title: {
                    text: 'Site Data at Glance',
                    x: -20
                },
                subtitle: {
                    text: 'Source: BizNETCTN',
                    x: -20
                },
                xAxis: {
                    title: {
                        text: 'Project/Site'
                    },
                    categories: finalproject2
                },
                yAxis: {
                    title: {
                        text: 'Data'
                    },
                    plotLines: [{
                        value: 0,
                        width: 1,
                        color: '#808080'
                    }]
                },
                plotOptions: {

                    series: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle',
                    borderWidth: 0
                },
                credits: {
                    enabled: false
                },
                series: [{
                    name: 'Total Expected Data',
                    data: totData[0]
                },
                 {
                     name: 'Available Data',
                     data: avaData[0]
                 },
                  {
                      name: 'SDV Data',
                      data: sdvData[0]
                  },
                  {
                      name: 'DM Reviewed',
                      data: dmData[0]
                  }

                ]
            });
            $(".content").height($(".wrapper").height());
        });
    }
    function Doughnut3() {
        if (chartN != null) {
            var $container = $('#containermain1').append('<div>');
            window.chart = new Highcharts.Chart({
                chart: {
                    height: 500,
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie',
                    renderTo: $container[0]
                },
                title: {
                    text: 'Site Data at Glance'
                },
                subtitle: {
                    text: 'Source: BizNETCTM',
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            distance: -60,
                            format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                            style: {
                                color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                            }
                        },
                        showInLegend: true,
                    },

                },
                labels: {
                    style: {
                        color: '#3E576F',
                        fontsize: '14px'
                    },
                    items: [{
                        html: $($get('<%= ddlsite.ClientID%>')).val(),
                        style: {
                            left: '10px',
                            top: '40px'
                        }
                    }]
                },
                credits: {
                    enabled: false
                },
                series: [{
                    name: 'Enrolled Subjects',
                    colorByPoint: true,
                    data: []
                }]
            });
            }
            $(function () {
                var $container = $('#containermain1').append('<div>');
                $(document).ready(function () {
                    window.chart = new Highcharts.Chart({
                        chart: {
                            height: 500,
                            plotBackgroundColor: null,
                            plotBorderWidth: null,
                            plotShadow: false,
                            type: 'pie',
                            renderTo: $container[0]
                        },
                        title: {
                            text: 'Site Data at Glance'
                        },
                        subtitle: {
                            text: 'Source: BizNETCTM',
                        },
                        tooltip: {
                            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                        },
                        plotOptions: {
                            pie: {
                                allowPointSelect: true,
                                cursor: 'pointer',
                                dataLabels: {
                                    enabled: true,
                                    format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                                    style: {
                                        color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                                    }
                                },
                                showInLegend: true,
                            },

                        },
                        labels: {
                            style: {
                                color: '#3E576F',
                                "font-size": '12px',
                                "font-weight": 'bold'

                            },
                            items: [{
                                html: $($get('<%= ddlsite.ClientID%>')).val(),
                                style: {
                                    left: '10px',
                                    top: '20px'
                                }
                            }]
                        },
                        credits: {
                            enabled: false
                        },
                        series: [{
                            name: $($get('<%= ddlsite.ClientID%>')).val(),
                            colorByPoint: true,
                            data: finaldonutin
                        }]
                    });
                    $(".content").height($(".wrapper").height());
                });
            });
            }


            function chartN1_selection() {
                finalselectedData = [];
                finaldonutin = [];

                var ddlColumnList1 = $($get('<%= ddlColumnList1.ClientID%>')).val();
                var dd2_chart = $($get('<%= dd2_chart.ClientID%>')).val();
                TrialDemo1 = ddlColumnList1;
                if (dd2_chart == "Pie" && ddlColumnList1 == "Total Expected Data" || dd2_chart == "Pie" && ddlColumnList1 == "Available Data" || dd2_chart == "Pie" && ddlColumnList1 == "SDV Data" || dd2_chart == "Pie" && ddlColumnList1 == "DM Reviwed") {
                    document.getElementById('ctl00_CPHLAMBDA_ddlsite').disabled = true;
                    pie2();
                }
                else if (dd2_chart == "Line" && ddlColumnList1 == "Total Expected Data" || dd2_chart == "Line" && ddlColumnList1 == "Available Data" || dd2_chart == "Line" && ddlColumnList1 == "SDV Data" || dd2_chart == "Line" && ddlColumnList1 == "DM Reviwed") {
                    document.getElementById('ctl00_CPHLAMBDA_ddlsite').disabled = true;
                    Line2();
                }
                else if (dd2_chart == "Doughnut" && ddlColumnList1 == "Total Expected Data" || dd2_chart == "Doughnut" && ddlColumnList1 == "Available Data" || dd2_chart == "Doughnut" && ddlColumnList1 == "SDV Data" || dd2_chart == "Doughnut" && ddlColumnList1 == "DM Reviwed") {
                    document.getElementById('ctl00_CPHLAMBDA_ddlsite').disabled = false;
                    var ddlsite = $($get('<%= ddlsite.ClientID%>')).val();
                    getSelectedSite(ddlsite);


                }
                else if (dd2_chart == "Line" && ddlColumnList1 == "All") {
                    AllLine2();
                }
                else if (dd2_chart == "Pie" && ddlColumnList1 == "All") {
                    Allpie2();
                }
                else if (dd2_chart == "Doughnut" && ddlColumnList1 == "All") {
                    document.getElementById('ctl00_CPHLAMBDA_ddlsite').disabled = false;


                    getSelectedSite(ddlsite);
                }

                else if (ddlColumnList1 == "Select Type") {
                    msgalert("Please Select The Type!");
                    $($get('<%= dd2_chart.ClientID%>')).val("Select Type");
                }
}


function getSelectedColumnList1(ddlColumnList1) {
    var vWorkSpaceId = $("#<%=ProjectWorkSpaceIdDemo1.ClientID%>").val();
    if (vWorkSpaceId != GloabalWorkspaceId) return false;
    var ddlColumnList1 = $($get('<%= ddlColumnList1.ClientID%>')).val();
    if (summarydata1 == "") return false;
    if (ddlColumnList1 == "Total Expected Data") {
        document.getElementById('ctl00_CPHLAMBDA_dd2_chart').disabled = false;
        var dd2_chart = $($get('<%= dd2_chart.ClientID%>')).val("Pie");
        //CreateSummaryTable1(summarydata1);
        //CreateSummaryChart1(summarydata1);
        fnGetData1(vWorkSpaceId);
        chartN1_selection();
    }
    else if (ddlColumnList1 == "Available Data") {
        document.getElementById('ctl00_CPHLAMBDA_dd2_chart').disabled = false;
        var dd2_chart = $($get('<%= dd2_chart.ClientID%>')).val("Pie");
        //CreateSummaryTable1(summarydata1);
        //CreateSummaryChart1(summarydata1);
        fnGetData1(vWorkSpaceId);
        chartN1_selection();
    }
    else if (ddlColumnList1 == "SDV Data") {
        document.getElementById('ctl00_CPHLAMBDA_dd2_chart').disabled = false;
        var dd2_chart = $($get('<%= dd2_chart.ClientID%>')).val("Pie");
        //CreateSummaryTable1(summarydata1);
        //CreateSummaryChart1(summarydata1);
        fnGetData1(vWorkSpaceId);
        chartN1_selection();
    }
    else if (ddlColumnList1 == "DM Reviwed") {
        document.getElementById('ctl00_CPHLAMBDA_dd2_chart').disabled = false;
        var dd2_chart = $($get('<%= dd2_chart.ClientID%>')).val("Pie");
        //CreateSummaryTable1(summarydata1);
        //CreateSummaryChart1(summarydata1);
        fnGetData1(vWorkSpaceId);
        chartN1_selection();
    }

    else if (ddlColumnList1 == "All") {
        document.getElementById('ctl00_CPHLAMBDA_dd2_chart').disabled = false;
        var dd2_chart = $($get('<%= dd2_chart.ClientID%>')).val("Pie");
        //CreateSummaryTable1(summarydata1);
        //CreateSummaryChart1(summarydata1);
        fnGetData1(vWorkSpaceId);
        chartN1_selection();

    }
    else {
        alert("Please Select the Chart and type");
    }
}

function getSelectedSite(ddlsite) {
    var ddlsite = $($get('<%= ddlsite.ClientID%>')).val();
    $($get('<%= dd2_chart.ClientID%>')).val("Doughnut");
    $($get('<%= ddlColumnList1.ClientID%>')).val("Select Type");
    for (var i = 0; i < finalproject2.length; i++) {

        if (finalproject2[i] == ddlsite) {

            finalselectedData.push(finalexpdata[i]);
            finalselectedData.push(finalreadydata[i]);
            finalselectedData.push(finalsdvdata[i]);
            finalselectedData.push(finalsrevdone1[i]);

        }
    }
    for (i = 0; i < finalcolumns.length; i++) {

        finaldonutin.push({ name: finalcolumns[i], y: finalselectedData[i] })
    }
    Doughnut3();
    finalselectedData = [];
    finaldonutin = [];


}
/*-------------Dhruvi Sitewise Data Status Completed------------------------------------------------------------------------------------------------------------------*/

/*--------------------------------Dhruvi DCF Management Started-------------------------------------------------------------------------------------------------*/
var summarydataDCF = '';
var vType = '';
var TrialDemo3 = '';
var ActivityDataset = [];
var finalprojectdcf = [];
var finaldcfcount = [];
var finaldonutinputdcf = [];
var Querytoans = [];
var Queryanstoresol = [];
var Querygentoresol = [];
var aheightbar = '';
var bwidthline = '';
var bminline = '';
var finalcols = [];
function fnDCFManage() {
    GloabalWorkspaceId = $("#<%=ProjectWorkSpaceIdDCFManage.ClientID%>").val();
    var projectval = $("#<%=txtDcfmanage.ClientID%>").val();
    if (projectval.length == '') {
        msgalert(" Please, Select Project first");
        return false;
    }

    document.getElementById('ctl00_CPHLAMBDA_dd3_chart').disabled = false;
    var dcfmanagedata = $("#<%=ProjectWorkSpaceIdDCFManage.ClientID%>").val();
    vType = $("#<%=ddlColumnList3.ClientID%>").val();
    if (vType == "Select Type") {
        msgalert("Please Select Type");
        return false;
    }
    $('#dcfmanage').show();
    var vWorkSpaceId = dcfmanagedata;
    fnGetDataDCF(vWorkSpaceId);

}

///Added by rinkal for DiSoft Dashboard
var summarydataSiteWiseSubjectInformation = ''
var summarydataSiteWiseCAInformation = ''
var summarydataSiteWiseGRInformation = ''
var summarydataSiteWiseAdjuInformation = ''
$(document).ready(function () {
    //debugger;
    openCity(event, 'DiSoft');
    //var objResponce = ""
    //if(<%=ViewState("Panel1")%> == ""){
    var objResponce = '<%=ViewState("Panel1")%>';
    if (objResponce != "") {
        objResponce = JSON.parse(objResponce);
    }


    var objActivity = '<%=ViewState("Activity")%>';
    if (objActivity != "") {
        objActivity = JSON.parse(objActivity);
    }


    summarydataSiteWiseSubjectInformation = objResponce
    //summarydataSiteWiseCAInformation = objResponceCA
    //summarydataSiteWiseGRInformation = objResponceGR
    //summarydataSiteWiseAdjuInformation = objResponceAR

    //$(  <%=ddlActivityForCA.ClientID%>).append($('<option>Select Activity</option>'))
    if (objActivity != "") {
        for (var i = 0; i < objActivity.Table.length; i++) {
            $(  <%=ddlActivityForCA.ClientID%>).append($('<option>', {
                value: objActivity.Table[i].iNodeId,
                text: objActivity.Table[i].vNodeDisplayName
            }));
        }
        Activityselection_CA();
        Activityselection_GR();
        Activityselection_AR();
    }

    if (summarydataSiteWiseSubjectInformation == "" || summarydataSiteWiseSubjectInformation.Table.length == 0) {
        //msgalert("No Data Found");
        return false;
    }
    if (summarydataSiteWiseSubjectInformation.Table.length > 0) {

        for (var i = 0, l = summarydataSiteWiseSubjectInformation.Table.length; i < l; i++) {

            TtlSubject.push(summarydataSiteWiseSubjectInformation.Table[i]['Total']);
            sumofTtlSubject += (summarydataSiteWiseSubjectInformation.Table[i]['Total']);
        }

    }

    chartselection(<%=ddlChartforSubject.ClientID%>);
    chartselection_CA(<%=ddlChartforCA.ClientID%>);
    chartselection_GR(<%=ddlChartforGR.ClientID%>);
    chartselection_AR(<%=ddlChartforAR.ClientID%>);

    //$(".overlaymonth").attr("style", "display:block;height:85%;transform: translatey(-0%);width:92.7%;margin-left:50px;margin-bottom:-100px")


});
var TtlSubject = [];
var sumofTtlSubject = [];
var tblTtlSubject = [];
//function fnGetDataSiteWiseSubject() {

//    $.ajax({
//        type: "post",
//        url: "frmMainPage.aspx/SiteWiseSubjectInformation",
//        //data: '{"vWorkSpaceId":"' + vWorkSpaceId + '"}',
//        contentType: "application/json; charset=utf-8",
//        datatype: JSON,
//        async: false,
//        dataType: "json",
//        success: function (data) {
//            debugger;
//            var data = data.d;
//            var msgs = JSON.parse(data);
//            summarydataSiteWiseSubjectInformation = msgs;
//        },
//        failure: function (response) {
//            msgalert("failure");
//            msgalert(data.d);
//        },
//        error: function (response) {
//            msgalert("error");
//        }
//    });

//}
var widthChart = "900"
function chartselection(e) {
    debugger;
    var chartName = $("#" + e.id).val();
    //if ($("#" + e.id).val() === undefined || $("#" + e.id).val() == "") chartName = "Pie"

    var Totalsubject = [], Rejectedsubject = [], Enrolledsubject = [];
    var xAxis = [], _array = []
    var Total = {}, Rejected = {}, Enrolled = {};
    var divId, chartTitle, yAxisTitle;
    var _Totalsubject, _Rejectedsubject, _Enrolledsubject;
    var sumofTotalsubject = 0, sumofRejectedsubject = 0, sumofEnrolledsubject = 0;
    var ActivityDataset = [];

    //if(summarydataSiteWiseSubjectInformation == "" || summarydataSiteWiseSubjectInformation.Table.length == 0){
    //    //msgalert("No Data Found");
    //    return false;
    //}
    if (summarydataSiteWiseSubjectInformation.Table.length > 0) {

        for (var i = 0, l = summarydataSiteWiseSubjectInformation.Table.length; i < l; i++) {
            xAxis.push(summarydataSiteWiseSubjectInformation.Table[i]['vProjectNo']);

            Totalsubject.push(summarydataSiteWiseSubjectInformation.Table[i]['Total']);
            Rejectedsubject.push(summarydataSiteWiseSubjectInformation.Table[i]['RejectedSubject']);
            Enrolledsubject.push(summarydataSiteWiseSubjectInformation.Table[i]['EnrolledSubject']);

            sumofTotalsubject += (summarydataSiteWiseSubjectInformation.Table[i]['Total']);
            sumofRejectedsubject += (summarydataSiteWiseSubjectInformation.Table[i]['RejectedSubject']);
            sumofEnrolledsubject += (summarydataSiteWiseSubjectInformation.Table[i]['EnrolledSubject']);
        }
        //TtlSubject = Totalsubject
        //sumofTtlSubject = sumofTotalsubject
    }

    _Totalsubject = Totalsubject
    Total = {
        name: "Total Subjects",
        data: _Totalsubject
    }
    _array.push(Total)

    _Rejectedsubject = Rejectedsubject
    Rejected = {
        name: "Rejected Subjects",
        data: _Rejectedsubject
    }
    _array.push(Rejected)

    _Enrolledsubject = Enrolledsubject
    Enrolled = {
        name: "Enrolled Subjects",
        data: _Enrolledsubject
    }

    divId = 'divSiteWiseSubjectInformation';
    chartTitle = 'Site Wise Subject Information';
    yAxisTitle = 'Count'
    _array.push(Enrolled);

    for (var Row = 0; Row < summarydataSiteWiseSubjectInformation.Table.length; Row++) {
        var InDataset = [];
        tblTtlSubject = summarydataSiteWiseSubjectInformation.Table[Row].Total
        InDataset.push(summarydataSiteWiseSubjectInformation.Table[Row].vProjectNo,
                       summarydataSiteWiseSubjectInformation.Table[Row].Total, summarydataSiteWiseSubjectInformation.Table[Row].RejectedSubject,
                       summarydataSiteWiseSubjectInformation.Table[Row].EnrolledSubject
                       );
        ActivityDataset.push(InDataset);
    }


    if (chartName == "Pie") {
        if (e.id = "ctl00_CPHLAMBDA_ddlChartforSubject") {
            getPie4DI(divId, chartTitle, xAxis, yAxisTitle, _array, widthChart)
        }
    }
    else if (chartName == "Line") {
        getLine4DI(divId, chartTitle, xAxis, yAxisTitle, _array, widthChart);
    }
    else if (chartName == "Doughnut") {
        _array = []
        Total = {
            name: "Total Subjects",
            y: sumofTotalsubject,
            sliced: true,
            selected: true
        }
        _array.push(Total)

        Rejected = {
            name: "Rejected Subjects",
            y: sumofRejectedsubject
        }
        _array.push(Rejected)

        Enrolled = {
            name: "Enrolled Subjects",
            y: sumofEnrolledsubject
        }
        _array.push(Enrolled);

        getDoughnut4DI(divId, chartTitle, xAxis, yAxisTitle, _array, widthChart)
    }
    else {
        msgalert("Please Select Chart");
    }

    getDataTable4DI('tblSiteWiseSubjectInformation', ActivityDataset)
}

//function fnGetDataSiteWiseCA() {

//    $.ajax({
//        type: "post",
//        url: "frmMainPage.aspx/SiteWiseCAInformation",
//        //data: '{"vWorkSpaceId":"' + vWorkSpaceId + '"}',
//        contentType: "application/json; charset=utf-8",
//        datatype: JSON,
//        async: false,
//        dataType: "json",
//        success: function (data) {
//            debugger;
//            var data = data.d;
//            var msgs = JSON.parse(data);
//            summarydataSiteWiseCAInformation = msgs;
//        },
//        failure: function (response) {
//            msgalert("failure");
//            msgalert(data.d);
//        },
//        error: function (response) {
//            msgalert("error");
//        }
//    });

//}

function chartselection_CA(e) {
    //debugger;
    var chartName = $("#" + e.id).val();
    //if ($("#" + e.id).val() === undefined || $("#" + e.id).val() == "") chartName = "Pie"

    var Totalsubject = [], SubmittedSubject = [], Enrolledsubject = [];
    var xAxis = [], _array = []
    var Total = {}, Submitted = {}, Enrolled = {};
    var divId, chartTitle, yAxisTitle;
    var _Totalsubject, _SubmittedSubject, _Enrolledsubject;
    var sumofTotalsubject = 0, sumofSubmittedSubject = 0, sumofEnrolledsubject = 0;
    var ActivityDataset = [];

    //if(summarydataSiteWiseCAInformation == "" || summarydataSiteWiseCAInformation.Table.length == 0){
    //    //msgalert("No Data Found");
    //    return false;
    //}
    if (summarydataSiteWiseCAInformation.Table.length > 0) {
        for (var i = 0, l = summarydataSiteWiseCAInformation.Table.length; i < l; i++) {
            xAxis.push(summarydataSiteWiseCAInformation.Table[i]['vProjectNo']);

            //TotalsubjectForCA.push(summarydataSiteWiseCAInformation.Table[i]['Total']);
            SubmittedSubject.push(summarydataSiteWiseCAInformation.Table[i]['SubmittedSubject']);
            //Enrolledsubject.push(summarydataSiteWiseCAInformation.Table[i]['EnrolledSubject']);

            //sumofTotalsubject += (summarydataSiteWiseCAInformation.Table[i]['Total']);
            sumofSubmittedSubject += (summarydataSiteWiseCAInformation.Table[i]['SubmittedSubject']);
            //sumofEnrolledsubject += (summarydataSiteWiseCAInformation.Table[i]['EnrolledSubject']);
        }
        Totalsubject = TtlSubject
        sumofTotalsubject = parseInt(sumofTtlSubject)
    }

    _Totalsubject = Totalsubject
    Total = {
        name: "Total Subjects",
        data: _Totalsubject
    }
    _array.push(Total)

    _SubmittedSubject = SubmittedSubject
    Submitted = {
        name: "CA Subjects",
        data: _SubmittedSubject
    }
    _array.push(Submitted)

    //_Enrolledsubject = Enrolledsubject
    //Enrolled = {
    //    name: "Enrolled Subjects",
    //    data: _Enrolledsubject
    //}

    divId = 'divSiteWiseCAAssignment';
    chartTitle = 'Site Wise Case Assignment Information';
    yAxisTitle = 'Count'
    //_array.push(Enrolled);

    for (var Row = 0; Row < summarydataSiteWiseCAInformation.Table.length; Row++) {
        var InDataset = [];
        InDataset.push(summarydataSiteWiseCAInformation.Table[Row].vProjectNo,
                        tblTtlSubject,
                        summarydataSiteWiseCAInformation.Table[Row].SubmittedSubject
                       );
        ActivityDataset.push(InDataset);
    }


    if (chartName == "Pie") {
        if (e.id = "ctl00_CPHLAMBDA_ddlChartforCA") {
            getPie4DI(divId, chartTitle, xAxis, yAxisTitle, _array, widthChart)
        }
    }
    else if (chartName == "Line") {
        getLine4DI(divId, chartTitle, xAxis, yAxisTitle, _array, widthChart);
    }
    else if (chartName == "Doughnut") {
        _array = []
        Total = {
            name: "Total Subjects",
            y: sumofTotalsubject,
            sliced: true,
            selected: true
        }
        _array.push(Total)

        Submitted = {
            name: "CA Subjects",
            y: sumofSubmittedSubject
        }
        _array.push(Submitted)

        //Enrolled = {
        //    name: "Enrolled Subjects",
        //    y: sumofEnrolledsubject
        //}
        //_array.push(Enrolled);

        getDoughnut4DI(divId, chartTitle, xAxis, yAxisTitle, _array, widthChart)
    }
    else {
        msgalert("Please Select Chart");
    }
    getDataTable4DI('tblSiteWiseCAAssignment', ActivityDataset)
}

function Activityselection_CA() {
    //debugger;
    $.ajax({
        type: "post",
        url: "frmMainPage.aspx/SiteWiseCAInformation",
        data: '{"vWorkSpaceId":"' + document.getElementById('<%= HProjectId.ClientID %>').value + '","iNodeId":"' + document.getElementById('<%= ddlActivityForCA.ClientID %>').value + '","MODE":"2"}',
        contentType: "application/json; charset=utf-8",
        datatype: JSON,
        async: false,
        dataType: "json",
        success: function (data) {
            //debugger;
            var data = data.d;
            var msgs = JSON.parse(data);
            summarydataSiteWiseCAInformation = msgs;
            chartselection_CA(<%=ddlChartforCA.ClientID%>);
            //chartselection_CA(document.getElementById('<%= ddlChartforCA.ClientID%>'))

        },
        failure: function (response) {
            msgalert("failure");
            msgalert(data.d);
        },
        error: function (response) {
            msgalert("error");
        }
    });
}

function Activityselection_GR() {
    //debugger;
    $.ajax({
        type: "post",
        url: "frmMainPage.aspx/SiteWiseCAInformation",
        data: '{"vWorkSpaceId":"' + document.getElementById('<%= HProjectId.ClientID %>').value + '","iNodeId":"","MODE":"3"}',
        contentType: "application/json; charset=utf-8",
        datatype: JSON,
        async: false,
        dataType: "json",
        success: function (data) {
            //debugger;
            var data = data.d;
            var msgs = JSON.parse(data);
            summarydataSiteWiseGRInformation = msgs;
            //chartselection_CA(document.getElementById('<%= ddlChartforCA.ClientID%>'))

        },
        failure: function (response) {
            msgalert("failure");
            msgalert(data.d);
        },
        error: function (response) {
            msgalert("error");
        }
    });

}
function Activityselection_AR() {
    //debugger;
    $.ajax({
        type: "post",
        url: "frmMainPage.aspx/SiteWiseCAInformation",
        data: '{"vWorkSpaceId":"' + document.getElementById('<%= HProjectId.ClientID %>').value + '","iNodeId":"","MODE":"4"}',
        contentType: "application/json; charset=utf-8",
        datatype: JSON,
        async: false,
        dataType: "json",
        success: function (data) {
            //debugger;
            var data = data.d;
            var msgs = JSON.parse(data);
            summarydataSiteWiseAdjuInformation = msgs;
            //chartselection_CA(document.getElementById('<%= ddlChartforCA.ClientID%>'))

        },
        failure: function (response) {
            msgalert("failure");
            msgalert(data.d);
        },
        error: function (response) {
            msgalert("error");
        }
    });

}
function chartselection_GR(e) {

    var chartName = $("#" + e.id).val();
    //if ($("#" + e.id).val() === undefined || $("#" + e.id).val() == "") chartName = "Pie"

    var Totalsubject = [], SubmittedSubjectForR1 = [], SubmittedSubjectForR2 = [];
    var xAxis = [], _array = []
    var Total = {}, SubmmittedForR1 = {}, SubmmittedForR2 = {};
    var divId, chartTitle, yAxisTitle;
    var _Totalsubject, _SubmittedSubjectForR1, _SubmittedSubjectForR2;
    var sumofTotalsubject = 0, sumofSubmittedSubjectForR1 = 0, sumofSubmittedSubjectForR2 = 0;
    var ActivityDataset = [];

    //if(summarydataSiteWiseGRInformation == "" || summarydataSiteWiseGRInformation.Table.length == 0){
    //    //msgalert("No Data Found");
    //    return false;
    //}
    if (summarydataSiteWiseGRInformation.Table.length > 0) {
        for (var i = 0, l = summarydataSiteWiseGRInformation.Table.length; i < l; i++) {
            xAxis.push(summarydataSiteWiseGRInformation.Table[i]['vProjectNo']);

            if (summarydataSiteWiseGRInformation.Table[i].vNodeDisplayName.includes("R1")) {
                SubmittedSubjectForR1.push(summarydataSiteWiseGRInformation.Table[i]['SubmittedSubject']);
                sumofSubmittedSubjectForR1 += (summarydataSiteWiseGRInformation.Table[i]['SubmittedSubject']);

            }
            else {
                SubmittedSubjectForR2.push(summarydataSiteWiseGRInformation.Table[i]['SubmittedSubject']);
                sumofSubmittedSubjectForR2 += (summarydataSiteWiseGRInformation.Table[i]['SubmittedSubject']);
            }

        }
        Totalsubject = TtlSubject
        sumofTotalsubject = parseInt(sumofTtlSubject)
    }

    _Totalsubject = Totalsubject
    Total = {
        name: "Total Subjects",
        data: _Totalsubject
    }
    _array.push(Total)

    _SubmittedSubjectForR1 = SubmittedSubjectForR1
    SubmmittedForR1 = {
        name: "R1 Global Response",
        data: _SubmittedSubjectForR1
    }
    _array.push(SubmmittedForR1)

    _SubmittedSubjectForR2 = SubmittedSubjectForR2
    SubmmittedForR2 = {
        name: "R2 Global Response",
        data: _SubmittedSubjectForR2
    }
    _array.push(SubmmittedForR2);

    divId = 'divSiteWiseGlobalResponse';
    chartTitle = 'Site Wise Global Response';
    yAxisTitle = 'Count'


    for (var Row = 0; Row < summarydataSiteWiseGRInformation.Table.length; Row++) {
        var InDataset = [];
        InDataset.push(summarydataSiteWiseGRInformation.Table[Row].vProjectNo,
                        tblTtlSubject,
                        summarydataSiteWiseGRInformation.Table[Row].SubmittedSubject

                       );
        ActivityDataset.push(InDataset);
    }


    if (chartName == "Pie") {
        if (e.id = "ctl00_CPHLAMBDA_ddlChartforGR") {
            getPie4DI(divId, chartTitle, xAxis, yAxisTitle, _array, widthChart)
        }
    }
    else if (chartName == "Line") {
        getLine4DI(divId, chartTitle, xAxis, yAxisTitle, _array, widthChart);
    }
    else if (chartName == "Doughnut") {
        _array = []
        Total = {
            name: "Total Subjects",
            y: sumofTotalsubject,
            sliced: true,
            selected: true
        }
        _array.push(Total)

        SubmmittedForR1 = {
            name: "R1 Global Response",
            y: sumofSubmittedSubjectForR1
        }
        _array.push(SubmmittedForR1)

        SubmmittedForR2 = {
            name: "R2 Global Response",
            y: sumofSubmittedSubjectForR2
        }
        _array.push(SubmmittedForR2);

        getDoughnut4DI(divId, chartTitle, xAxis, yAxisTitle, _array, widthChart)
    }
    else {
        msgalert("Please Select Chart");
    }
    getDataTable4DI('tblSiteWiseGlobalResponse', ActivityDataset)
}

function chartselection_AR(e) {
    //debugger;
    var chartName = $("#" + e.id).val();
    //if ($("#" + e.id).val() === undefined || $("#" + e.id).val() == "") chartName = "Pie"

    var Totalsubject = [], SubmittedSubject = [], Enrolledsubject = [];
    var xAxis = [], _array = []
    var Total = {}, Submitted = {}, Enrolled = {};
    var divId, chartTitle, yAxisTitle;
    var _Totalsubject, _SubmittedSubject, _Enrolledsubject;
    var sumofTotalsubject = 0, sumofSubmittedSubject = 0, sumofEnrolledsubject = 0;
    var ActivityDataset = [];

    //if(summarydataSiteWiseAdjuInformation == "" || summarydataSiteWiseAdjuInformation.Table.length == 0){
    //    //msgalert("No Data Found");
    //    return false;
    //}
    if (summarydataSiteWiseAdjuInformation.Table.length > 0) {
        for (var i = 0, l = summarydataSiteWiseAdjuInformation.Table.length; i < l; i++) {
            xAxis.push(summarydataSiteWiseAdjuInformation.Table[i]['vProjectNo']);

            //Totalsubject.push(summarydataSiteWiseAdjuInformation.Table[i]['Total']);
            SubmittedSubject.push(summarydataSiteWiseAdjuInformation.Table[i]['SubmitedSubject']);
            //Enrolledsubject.push(summarydataSiteWiseAdjuInformation.Table[i]['EnrolledSubject']);

            //sumofTotalsubject += (summarydataSiteWiseAdjuInformation.Table[i]['Total']);
            sumofSubmittedSubject += (summarydataSiteWiseAdjuInformation.Table[i]['SubmitedSubject']);
            //sumofEnrolledsubject += (summarydataSiteWiseAdjuInformation.Table[i]['EnrolledSubject']);
        }
        Totalsubject = TtlSubject
        sumofTotalsubject = parseInt(sumofTtlSubject)
    }

    _Totalsubject = Totalsubject
    Total = {
        name: "Total Subjects",
        data: _Totalsubject
    }
    _array.push(Total)

    _SubmittedSubject = SubmittedSubject
    Submitted = {
        name: "Subjects",
        data: _SubmittedSubject
    }
    _array.push(Submitted)

    //_Enrolledsubject = Enrolledsubject
    //Enrolled = {
    //    name: "Enrolled Subjects",
    //    data: _Enrolledsubject
    //}

    divId = 'divSiteWiseAdjudicatorResponse';
    chartTitle = 'Site Wise Adjudicator Response';
    yAxisTitle = 'Count'
    //_array.push(Enrolled);

    for (var Row = 0; Row < summarydataSiteWiseAdjuInformation.Table.length; Row++) {
        var InDataset = [];
        InDataset.push(summarydataSiteWiseAdjuInformation.Table[Row].vProjectNo,
                        tblTtlSubject,
                       summarydataSiteWiseAdjuInformation.Table[Row].SubmitedSubject
                       );
        ActivityDataset.push(InDataset);
    }


    if (chartName == "Pie") {
        if (e.id = "ctl00_CPHLAMBDA_ddlChartforAR") {
            getPie4DI(divId, chartTitle, xAxis, yAxisTitle, _array, widthChart)
        }
    }
    else if (chartName == "Line") {
        getLine4DI(divId, chartTitle, xAxis, yAxisTitle, _array, widthChart);
    }
    else if (chartName == "Doughnut") {
        _array = []
        Total = {
            name: "Total Subjects",
            y: sumofTotalsubject,
            sliced: true,
            selected: true
        }
        _array.push(Total)

        Submitted = {
            name: "Subjects",
            y: sumofSubmittedSubject
        }
        _array.push(Submitted)

        //Enrolled = {
        //    name: "Enrolled Subjects",
        //    y: sumofEnrolledsubject
        //}
        //_array.push(Enrolled);

        getDoughnut4DI(divId, chartTitle, xAxis, yAxisTitle, _array, widthChart)
    }
    else {
        msgalert("Please Select Chart");
    }

    getDataTable4DI('tblSiteWiseAdjudicatorResponse', ActivityDataset)
}


function getPie4DI(divId, chartTitle, xAxis, yAxisTitle, _array, widthChart) {
    var chart = Highcharts.chart(divId, {
        chart: {
            type: 'column',
            width: widthChart
        },
        title: {
            text: chartTitle
        },
        xAxis: {
            categories: xAxis,
            //crosshair: true
        },

        yAxis: {
            min: 0,
            title: {
                text: yAxisTitle
            },
            labels: {
                // overflow: 'justify'
            }
        },
        plotOptions: {
            column: {
                dataLabels: {
                    enabled: true,
                    crop: false,
                    overflow: 'none'
                }
            }
        },



        credits: {
            enabled: false
        },

        tooltip: {
            formatter: function () {
                return '' +
                    this.series.name + ': ' + this.y + '';
            }
        },
        series:
            _array


    });
}

function getDoughnut4DI(divId, chartTitle, xAxis, yAxisTitle, _array, widthChart) {
    var chart = Highcharts.chart(divId, {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie',
            width: widthChart
        },
        title: {
            text: chartTitle
        },
        credits: {
            enabled: false
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    distance: -30,
                    format: '{point.y:.1f}',
                    color: 'white'
                },
                showInLegend: true,
            }
        },
        series: [{
            name: 'Count',
            colorByPoint: true,
            data: _array
        }]
    });
}

function getLine4DI(divId, chartTitle, xAxis, yAxisTitle, _array, widthChart) {
    Highcharts.chart(divId, {
        chart: {
            type: 'line',
            width: widthChart
        },
        title: {
            text: chartTitle
        },
        xAxis: {
            categories: xAxis
        },
        yAxis: {
            min: 0,
            title: {
                text: yAxisTitle
            }
        },
        plotOptions: {
            line: {
                dataLabels: {
                    enabled: true
                },
                enableMouseTracking: false
            }
        },
        series: _array
    });
}

function getDataTable4DI(tblId, activeDataSet) {
    $ = jQuery;
    $('#' + tblId).dataTable().fnDestroy();
    otable = $('#' + tblId).DataTable({

        "bJQueryUI": true,
        "sPaginationType": "full_numbers",
        "bLengthChange": false,
        "iDisplayLength": 5,
        "bProcessing": true,
        "bSort": false,
        "autoWidth": false,
        "aaData": activeDataSet,
        "bInfo": true,
        "scrollX": true,
        //"aLengthMenu": [[3, 5, 10, -1], [3, 5, 10, "All"]],       
        "columns": [
            { "width": "5%", "targets": 0 },
            null, null, null
        ],
        "oLanguage": {
            "sEmptyTable": "No Record Found",
        },
    });
}


//

function fnGetDataDCF(vWorkSpaceId) {

    $.ajax({
        type: "post",
        url: "frmMainPage.aspx/Proc_DCFTrackingReport",
        data: '{"vWorkSpaceId":"' + vWorkSpaceId + '","vType":"' + vType + '"}',
        contentType: "application/json; charset=utf-8",
        datatype: JSON,
        async: false,
        dataType: "json",
        success: function (data) {
            var data = data.d;
            var msgs = JSON.parse(data);
            summarydataDCF = msgs;
            if (summarydataDCF == "") {
                document.getElementById('ctl00_CPHLAMBDA_btnExportQueryMgt').style.display = "none";
                return false;
            }
            document.getElementById('ctl00_CPHLAMBDA_btnExportQueryMgt').style.display = "Inline";
            CreateSummaryTableDCF(summarydataDCF);
            CreateSummaryChartDCF(summarydataDCF);
        },
        failure: function (response) {
            msgalert("failure");
            msgalert(data.d);
        },
        error: function (response) {
            msgalert("error");
        }
    });

}
function CreateSummaryTableDCF(summarydataDCF) {
    var vType = $("#<%=ddlColumnList3.ClientID%>").val();
    var ActivityDataset = [];

    if (vType == 'DCF STATUS') {

        for (var Row = 0; Row < summarydataDCF.Table.length; Row++) {
            var InDataset = [];
            InDataset.push(summarydataDCF.Table[Row]['ProjectNo'], summarydataDCF.Table[Row]['DCFCount'], summarydataDCF.Table[Row]['cDCFStatus']);
            ActivityDataset.push(InDataset);
        }
        finalcols = [
             { "sTitle": "Project" },
             { "sTitle": "DCFCount" },
             { "sTitle": vType },
        ]
    }
    else if (vType == 'TYPES OF DCF') {
        for (var Row = 0; Row < summarydataDCF.Table.length; Row++) {
            var InDataset = [];
            InDataset.push(summarydataDCF.Table[Row]['ProjectNo'], summarydataDCF.Table[Row]['DCFCount'], summarydataDCF.Table[Row]['DCFType']);
            ActivityDataset.push(InDataset);
        }
        finalcols = [
             { "sTitle": "Project" },
             { "sTitle": "DCFCount" },
             { "sTitle": vType },
        ]
    }
    else if (vType == 'ACTIVITY WISE') {
        for (var Row = 0; Row < summarydataDCF.Table.length; Row++) {
            var InDataset = [];
            InDataset.push(summarydataDCF.Table[Row]['DCFCount'], summarydataDCF.Table[Row]['vActivityName']);
            ActivityDataset.push(InDataset);
        }
        finalcols = [
             //{ "sTitle": "Project" },
             { "sTitle": "DCFCount" },
             { "sTitle": vType },
        ]
    }
    else if (vType == 'FULL CHART') {
        for (var Row = 0; Row < summarydataDCF.Table.length; Row++) {
            var InDataset = [];

            InDataset.push(summarydataDCF.Table[Row]['Site'], summarydataDCF.Table[Row]['Days'], summarydataDCF.Table[Row]['QUERY GENRATED TO ANSWER'],
                           summarydataDCF.Table[Row]['QUERY ANSWRED TO RESOLVED'], summarydataDCF.Table[Row]['QUERY GENRATED TO RESOLVED']);
            ActivityDataset.push(InDataset);
        }
        finalcols = [
            { "sTitle": "Poject" },
            { "sTitle": "Days" },
            { "sTitle": "Query Generated to answer" },
            { "sTitle": "Query Answered to resolved" },
            { "sTitle": "Query Generated to resolved" },
        ]
    }
    else if (vType == 'Query By Site') {
        for (var Row = 0; Row < summarydataDCF.Table.length; Row++) {
            var InDataset = [];
            InDataset.push(summarydataDCF.Table[Row]['vProjectNo'], summarydataDCF.Table[Row]['QueryCOUNT']);
            ActivityDataset.push(InDataset);
        }
        finalcols = [
             { "sTitle": "Project" },
             { "sTitle": "Query Count" },
             //{ "sTitle": vType },
        ]
    }
    else if (vType == 'Query Per Subject') {
        for (var Row = 0; Row < summarydataDCF.Table.length; Row++) {
            var InDataset = [];
            InDataset.push(summarydataDCF.Table[Row]['vProjectNo'], summarydataDCF.Table[Row]['QueryCOUNT']);
            ActivityDataset.push(InDataset);
        }
        finalcols = [
             { "sTitle": "Project No" },
             { "sTitle": "Avg.No Queries Per Subject" },
        ]
    }
    else if (vType == 'System Generated' || vType == 'Manually Generated') {
        for (var Row = 0; Row < summarydataDCF.Table.length; Row++) {
            var InDataset = [];
            InDataset.push(summarydataDCF.Table[Row]['vProjectNo'], summarydataDCF.Table[Row]['QueryCOUNT'], summarydataDCF.Table[Row]['Type']);
            ActivityDataset.push(InDataset);
        }
        finalcols = [
             { "sTitle": "Project No" },
             { "sTitle": "Query Count" },
             { "sTitle": "Type" },
        ]
    }

    $ = jQuery;
    var createtable1 = $("<table id='dcfmanage' class='display' cellspacing='0' width='100%'> </table>");
    $("#createtable").empty().append(createtable1);
    $('#dcfmanage').DataTable({
        "bJQueryUI": true,
        "sScrollX": "100%",
        "sPaginationType": "full_numbers",
        "pageLength": '5',
        "bSort": false,
        "aaData": ActivityDataset,
        "iDisplayLength": 5,

        "aoColumns": finalcols,

        "columns": [
            null, null, null, null, null, null, null, null, null, null, null, null, null
        ],
        "oLanguage": {
            "sEmptyTable": "No Record Found"
        },
    });
}
function CreateSummaryChartDCF(summarydataDCF) {

    var ddlColumnList3 = $($get('<%= ddlColumnList3.ClientID()%>'));
    var ddlColumnListVal3 = $(ddlColumnList3).val();
    var AllDataset = [];

    if (ddlColumnListVal3 == "TYPES OF DCF") {
        TypeOFDCF();
    }
    else if (ddlColumnListVal3 == "DCF STATUS") {
        StatusofDCF();
    }
    else if (ddlColumnListVal3 == "ACTIVITY WISE") {
        ActivitywiseDCF();
    }
    else if (ddlColumnListVal3 == "FULL CHART") {
        SummaryDCF();
    }
    else if (ddlColumnListVal3 == "Query By Site") {
        SummaryQueryBySite();
    }
    else if (ddlColumnListVal3 == "Query Per Subject") {
        SummaryQueryPerSubject();
    }
    else if (ddlColumnListVal3 == "System Generated" || ddlColumnListVal3 == "Manually Generated") {
        StatusofDCFbySite();
    }
}

function TypeOFDCF() {
    finalprojectdcf = [];
    finaldcfcount = [];
    finaldonutinputdcf = [];

    for (var Row = 0; Row < summarydataDCF.Table.length; Row++) {
        var InDataset = [];
        InDataset.push(summarydataDCF.Table[Row]['ProjectNo'], summarydataDCF.Table[Row]['DCFCount'], summarydataDCF.Table[Row]['DCFType']);
        ActivityDataset.push(InDataset);
    }

    for (var i = 0, l = summarydataDCF.Table.length; i < l; i++) {
        //var msg = summarydata[i];
        finalprojectdcf.push(summarydataDCF.Table[i]['DCFType']);

        finaldcfcount.push(summarydataDCF.Table[i]['DCFCount']);
    }
    for (var i = 0, l = summarydataDCF.Table.length; i < l; i++) {
        finaldonutinputdcf.push({ name: finalprojectdcf[i], y: finaldcfcount[i] });
    }
    aheightbar = 400;

    Pie4();

}

function StatusofDCF() {
    finalprojectdcf = [];
    finaldcfcount = [];
    finaldonutinputdcf = [];
    for (var Row = 0; Row < summarydataDCF.Table.length; Row++) {
        var InDataset = [];
        InDataset.push(summarydataDCF.Table[Row]['ProjectNo'], summarydataDCF.Table[Row]['DCFCount'], summarydataDCF.Table[Row]['cDCFStatus']);
        ActivityDataset.push(InDataset);
    }

    for (var i = 0, l = summarydataDCF.Table.length; i < l; i++) {
        //var msg = summarydata[i];
        finalprojectdcf.push(summarydataDCF.Table[i]['cDCFStatus']);

        finaldcfcount.push(summarydataDCF.Table[i]['DCFCount']);
    }
    for (var i = 0, l = summarydataDCF.Table.length; i < l; i++) {
        finaldonutinputdcf.push({ name: finalprojectdcf[i], y: finaldcfcount[i] });
    }
    aheightbar = 400;

    Pie4();

}

function ActivitywiseDCF() {
    finalprojectdcf = [];
    finaldcfcount = [];
    finaldonutinputdcf = [];
    for (var Row = 0; Row < summarydataDCF.Table.length; Row++) {
        var InDataset = [];
        InDataset.push(summarydataDCF.Table[Row]['DCFCount'], summarydataDCF.Table[Row]['vActivityName']);
        ActivityDataset.push(InDataset);
    }

    for (var i = 0, l = summarydataDCF.Table.length; i < l; i++) {
        //var msg = summarydata[i];
        finalprojectdcf.push(summarydataDCF.Table[i]['vActivityName']);

        finaldcfcount.push(summarydataDCF.Table[i]['DCFCount']);
    }
    for (var i = 0, l = summarydataDCF.Table.length; i < l; i++) {
        finaldonutinputdcf.push({ name: finalprojectdcf[i], y: finaldcfcount[i] });
    }
    aheightbar = 1000;
    Pie4();
}

function SummaryDCF() {
    finalprojectdcf = [];
    Querytoans = [];
    Queryanstoresol = [];
    Querygentoresol = [];
    for (var Row = 0; Row < summarydataDCF.Table.length; Row++) {
        var InDataset = [];
        InDataset.push(summarydataDCF.Table[Row]['Site'], summarydataDCF.Table[Row]['Days'], summarydataDCF.Table[Row]['QUERY GENRATED TO ANSWER'],
                       summarydataDCF.Table[Row]['QUERY ANSWRED TO RESOLVED'], summarydataDCF.Table[Row]['QUERY GENRATED TO RESOLVED']);
        ActivityDataset.push(InDataset);
    }
    for (var i = 0, l = summarydataDCF.Table.length; i < l; i++) {
        //var msg = summarydata[i];
        finalprojectdcf.push(summarydataDCF.Table[i]['Days']);

        Querytoans.push(summarydataDCF.Table[i]['QUERY GENRATED TO ANSWER']);
        Queryanstoresol.push(summarydataDCF.Table[i]['QUERY ANSWRED TO RESOLVED']);
        Querygentoresol.push(summarydataDCF.Table[i]['QUERY ANSWRED TO RESOLVED']);
    }
    Allbar1();

}

function SummaryQueryBySite() {
    finalprojectdcf = [];
    finaldcfcount = [];
    finaldonutinputdcf = [];

    for (var Row = 0; Row < summarydataDCF.Table.length; Row++) {
        var InDataset = [];
        InDataset.push(summarydataDCF.Table[Row]['vProjectNo'], summarydataDCF.Table[Row]['QueryCOUNT']);
        ActivityDataset.push(InDataset);
    }

    for (var i = 0, l = summarydataDCF.Table.length; i < l; i++) {
        finalprojectdcf.push(summarydataDCF.Table[i]['vProjectNo']);
        finaldcfcount.push(summarydataDCF.Table[i]['QueryCOUNT']);
    }
    for (var i = 0, l = summarydataDCF.Table.length; i < l; i++) {
        finaldonutinputdcf.push({ name: finalprojectdcf[i], y: finaldcfcount[i] });
    }
    aheightbar = 400;
    Pie4();
}

function SummaryQueryPerSubject() {
    finalprojectdcf = [];
    finaldcfcount = [];
    finaldonutinputdcf = [];

    for (var Row = 0; Row < summarydataDCF.Table.length; Row++) {
        var InDataset = [];
        InDataset.push(summarydataDCF.Table[Row]['vProjectNo'], summarydataDCF.Table[Row]['QueryCOUNT']);
        ActivityDataset.push(InDataset);
    }

    for (var i = 0, l = summarydataDCF.Table.length; i < l; i++) {
        finalprojectdcf.push(summarydataDCF.Table[i]['vProjectNo']);
        finaldcfcount.push(summarydataDCF.Table[i]['QueryCOUNT']);
    }
    for (var i = 0, l = summarydataDCF.Table.length; i < l; i++) {
        finaldonutinputdcf.push({ name: finalprojectdcf[i], y: finaldcfcount[i] });
    }
    aheightbar = 400;
    Pie4();
}

function StatusofDCFbySite() {
    finalprojectdcf = [];
    finaldcfcount = [];
    finaldonutinputdcf = [];
    for (var Row = 0; Row < summarydataDCF.Table.length; Row++) {
        var InDataset = [];
        InDataset.push(summarydataDCF.Table[Row]['vProjectNo'], summarydataDCF.Table[Row]['QueryCOUNT'], summarydataDCF.Table[Row]['Type']);
        ActivityDataset.push(InDataset);
    }

    for (var i = 0, l = summarydataDCF.Table.length; i < l; i++) {
        finalprojectdcf.push(summarydataDCF.Table[i]['vProjectNo']);

        finaldcfcount.push(summarydataDCF.Table[i]['QueryCOUNT']);
    }
    for (var i = 0, l = summarydataDCF.Table.length; i < l; i++) {
        finaldonutinputdcf.push({ name: finalprojectdcf[i], y: finaldcfcount[i] });
    }
    aheightbar = 400;
    Pie4();
}


function chartN2_selection() {
    var ddlColumnList3 = $($get('<%= ddlColumnList3.ClientID%>')).val();
    var dd3_chart = $($get('<%= dd3_chart.ClientID%>')).val();
    TrialDemo3 = ddlColumnList3;

    if (dd3_chart == "Pie") {
        if (ddlColumnList3 == "TYPES OF DCF" || ddlColumnList3 == "DCF STATUS" || ddlColumnList3 == "ACTIVITY WISE" || ddlColumnList3 == "Query By Site" || ddlColumnList3 == "Query Per Subject" || ddlColumnList3 == "System Generated" || ddlColumnList3 == "Manually Generated") {
            Pie4();
        }
        else {
            Allbar1();
        }
    }
    else if (dd3_chart == "Line") {
        if (ddlColumnList3 == "TYPES OF DCF" || ddlColumnList3 == "DCF STATUS" || ddlColumnList3 == "Query By Site" || ddlColumnList3 == "Query Per Subject" || ddlColumnList3 == "System Generated" || ddlColumnList3 == "Manually Generated") {
            bwidthline = 800;
            bminline = 0;
            Line3();
        }
        else if (ddlColumnList3 == "ACTIVITY WISE") {
            bwidthline = 1048;
            bminline = 10;
            Line3();
        }
        else if (ddlColumnList3 == "FULL CHART") {
            AllLinedcf();
        }
    }
    else if (dd3_chart == "Doughnut") {
        if (ddlColumnList3 == "System Generated" || ddlColumnList3 == "Manually Generated") {
            DoughnutSystemManual();
        }
        else {
            Doughnutdcf();
        }


    }
    else if (ddlColumnList3 == "Select Type") {
        msgalert("Please Select The Type!");
        $($get('<%= dd3_chart.ClientID%>')).val("Select Type");
    }
}

var datain = '';
function getSelectedManage(ddlColumnList3) {
    var dcfmanagedata = $("#<%=ProjectWorkSpaceIdDCFManage.ClientID%>").val();
    if (dcfmanagedata != GloabalWorkspaceId) return false;
    var vWorkSpaceId = dcfmanagedata;





    var ddlColumnList3 = $($get('<%= ddlColumnList3.ClientID%>')).val();
    if (ddlColumnList3 == "ACTIVITY WISE" || ddlColumnList3 == "FULL CHART") {
        datain = $('#<%= dd3_chart.ClientID%>').find("option[value='Doughnut']").attr("disabled", "disabled");
    }
    vType = ddlColumnList3;

    if (summarydataDCF == "") return false;

    $($get('<%= dd3_chart.ClientID%>')).val("Pie");
    if (ddlColumnList3 == "TYPES OF DCF") {
        document.getElementById('ctl00_CPHLAMBDA_dd3_chart').disabled = false;
        datain = $('#<%= dd3_chart.ClientID%>').find("option[value='Doughnut']").removeAttr("disabled");
        fnGetDataDCF(vWorkSpaceId);
        chartN2_selection();
    }
    else if (ddlColumnList3 == "DCF STATUS") {
        document.getElementById('ctl00_CPHLAMBDA_dd3_chart').disabled = false;
        datain = $('#<%= dd3_chart.ClientID%>').find("option[value='Doughnut']").removeAttr("disabled");
        fnGetDataDCF(vWorkSpaceId);
        chartN2_selection();
    }
    else if (ddlColumnList3 == "ACTIVITY WISE") {
        document.getElementById('ctl00_CPHLAMBDA_dd3_chart').disabled = false;
        datain = $('#<%= dd3_chart.ClientID%>').find("option[value='Doughnut']").attr("disabled", "disabled");
        fnGetDataDCF(vWorkSpaceId);
        chartN2_selection();
    }
    else if (ddlColumnList3 == "FULL CHART") {
        document.getElementById('ctl00_CPHLAMBDA_dd3_chart').disabled = false;
        datain = $('#<%= dd3_chart.ClientID%>').find("option[value='Doughnut']").attr("disabled", "disabled");
        fnGetDataDCF(vWorkSpaceId);
        chartN2_selection();
    }
    else if (ddlColumnList3 == "Query By Site") {
        document.getElementById('ctl00_CPHLAMBDA_dd3_chart').disabled = false;
        datain = $('#<%= dd3_chart.ClientID%>').find("option[value='Doughnut']").attr("disabled", "disabled");
        fnGetDataDCF(vWorkSpaceId);
        chartN2_selection();
    }
    else if (ddlColumnList3 == "Query Per Subject") {
        document.getElementById('ctl00_CPHLAMBDA_dd3_chart').disabled = false;
        datain = $('#<%= dd3_chart.ClientID%>').find("option[value='Doughnut']").attr("disabled", "disabled");
        fnGetDataDCF(vWorkSpaceId);
        chartN2_selection();
    }
    else if (ddlColumnList3 == "System Generated" || ddlColumnList3 == "Manually Generated") {
        document.getElementById('ctl00_CPHLAMBDA_dd3_chart').disabled = false;
        fnGetDataDCF(vWorkSpaceId);
        chartN2_selection();
    }
    else {
        msgalert("Please Select The Chart End Type");
    }
}
var chartN;

function DoughnutSystemManual() {
    if (chartN != null) {
        var $container = $('#containermain2').append('<div>');
        window.chart = new Highcharts.Chart({
            chart: {
                height: 500,
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false,
                type: 'pie',
                renderTo: $container[0]
            },
            title: {
                text: 'DCF Managegement'
            },
            subtitle: {
                text: 'Source: BizNETCTM',
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.y}</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        distance: -60,
                        format: '<b>{point.name}</b>: {point.y}',
                        style: {
                            color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                        }
                    },
                    showInLegend: true,
                },

            },
            labels: {
                style: {
                    color: '#3E576F',
                    fontsize: '14px'
                },
                items: [{
                    html: $($get('<%= ddlColumnList3.ClientID%>')).val(),
                    style: {
                        left: '10px',
                        top: '40px'
                    }
                }]
            },
            credits: {
                enabled: false
            },
            series: [{
                name: 'Enrolled Subjects',
                colorByPoint: true,
                data: []
            }]
        });
        }
        $(function () {
            var $container = $('#containermain2').append('<div>');
            $(document).ready(function () {
                window.chart = new Highcharts.Chart({
                    chart: {
                        height: 500,
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false,
                        type: 'pie',
                        renderTo: $container[0]
                    },
                    title: {
                        text: 'DCF Management'
                    },
                    subtitle: {
                        text: 'Source: BizNETCTM',
                    },
                    tooltip: {
                        pointFormat: '{series.name}: <b>{point.y}</b>'
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                format: '<b>{point.name}</b>: {point.y}',
                                style: {
                                    color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                                }
                            },
                            showInLegend: true,
                        },

                    },
                    labels: {
                        style: {
                            color: '#3E576F',
                            "font-size": '12px',
                            "font-weight": 'bold'

                        },
                        items: [{
                            html: $($get('<%= ddlColumnList3.ClientID%>')).val(),
                            style: {
                                left: '10px',
                                top: '20px'
                            }
                        }]
                    },
                    credits: {
                        enabled: false
                    },
                    series: [{
                        name: $($get('<%= ddlColumnList3.ClientID%>')).val(),
                        colorByPoint: true,
                        data: finaldonutinputdcf
                    }]
                });
                $(".content").height($(".wrapper").height());
            });
        });
        }


        function Line3() {
            var $container = $('#containermain2').append('<div>');
            if (chartN != null) {
                window.chart = new Highcharts.Chart({
                    chart: {
                        height: 800,
                        width: bwidthline,
                        renderTo: $container[0]
                    },
                    title: {
                        text: 'DCF Management',
                        x: -20
                    },
                    subtitle: {
                        text: 'Source: BizNETCTM',
                        x: -20
                    },
                    xAxis: {
                        title: {
                            text: 'Project/Site'
                        },
                        categories: [],
                        //min: bminline


                    },
                    yAxis: {
                        title: {
                            text: 'Data'
                        },
                        tooltip: {
                            enabled: true
                        },
                        plotLines: [{
                            value: 0,
                            width: 1,
                            color: '#808080'
                        }]
                    },
                    plotOptions: {

                        series: {
                            dataLabels: {
                                enabled: true
                            }
                        }
                    },
                    legend: {
                        layout: 'vertical',
                        align: 'right',
                        verticalAlign: 'middle',
                        borderWidth: 0
                    },
                    //scrollbar: {
                    //    enabled: true
                    //},

                    credits: {
                        enabled: false
                    },
                    series: [{
                        name: 'Enrolled Subject',
                        data: []
                    },


                    ]
                });
            }

            $(function () {
                var $container = $('#containermain2').append('<div>');
                window.chart = new Highcharts.Chart({
                    chart: {
                        height: 500,
                        width: bwidthline,

                        renderTo: $container[0]
                    },
                    title: {
                        text: 'DCF Management',
                        x: -20
                    },
                    subtitle: {
                        text: 'Source: BizNETCTM',
                        x: -20
                    },
                    xAxis: {
                        title: {
                            text: 'Project/Site'
                        },
                        categories: finalprojectdcf,
                        //min: bminline


                    },
                    yAxis: {
                        title: {
                            text: 'Data'
                        },
                        plotLines: [{
                            value: 0,
                            width: 1,
                            color: '#808080'
                        }]
                    },
                    plotOptions: {

                        series: {
                            dataLabels: {
                                enabled: true
                            }
                        }
                    },
                    legend: {
                        layout: 'vertical',
                        align: 'right',
                        verticalAlign: 'middle',
                        borderWidth: 0
                    },
                    //scrollbar: {
                    //    enabled: true
                    //},
                    credits: {
                        enabled: false
                    },

                    series: [{
                        name: TrialDemo3,
                        data: finaldcfcount
                    }

                    ]
                });
                $(".content").height($(".wrapper").height());
            });
        }

        function Doughnutdcf() {
            if (chartN != null) {
                var $container = $('#containermain2').append('<div>');
                window.chart = new Highcharts.Chart({
                    chart: {
                        height: 500,
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false,
                        type: 'pie',
                        renderTo: $container[0]
                    },
                    title: {
                        text: 'DCF Managegement'
                    },
                    subtitle: {
                        text: 'Source: BizNETCTM',
                    },
                    tooltip: {
                        pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                distance: -60,
                                format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                                style: {
                                    color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                                }
                            },
                            showInLegend: true,
                        },

                    },
                    labels: {
                        style: {
                            color: '#3E576F',
                            fontsize: '14px'
                        },
                        items: [{
                            html: $($get('<%= ddlColumnList3.ClientID%>')).val(),
                            style: {
                                left: '10px',
                                top: '40px'
                            }
                        }]
                    },
                    credits: {
                        enabled: false
                    },
                    series: [{
                        name: 'Enrolled Subjects',
                        colorByPoint: true,
                        data: []
                    }]
                });
                }
                $(function () {
                    var $container = $('#containermain2').append('<div>');
                    $(document).ready(function () {
                        window.chart = new Highcharts.Chart({
                            chart: {
                                height: 500,
                                plotBackgroundColor: null,
                                plotBorderWidth: null,
                                plotShadow: false,
                                type: 'pie',
                                renderTo: $container[0]
                            },
                            title: {
                                text: 'DCF Management'
                            },
                            subtitle: {
                                text: 'Source: BizNETCTM',
                            },
                            tooltip: {
                                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                            },
                            plotOptions: {
                                pie: {
                                    allowPointSelect: true,
                                    cursor: 'pointer',
                                    dataLabels: {
                                        enabled: true,
                                        format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                                        style: {
                                            color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                                        }
                                    },
                                    showInLegend: true,
                                },

                            },
                            labels: {
                                style: {
                                    color: '#3E576F',
                                    "font-size": '12px',
                                    "font-weight": 'bold'

                                },
                                items: [{
                                    html: $($get('<%= ddlColumnList3.ClientID%>')).val(),
                                    style: {
                                        left: '10px',
                                        top: '20px'
                                    }
                                }]
                            },
                            credits: {
                                enabled: false
                            },
                            series: [{
                                name: $($get('<%= ddlColumnList3.ClientID%>')).val(),
                                colorByPoint: true,
                                data: finaldonutinputdcf
                            }]
                        });
                        $(".content").height($(".wrapper").height());
                    });
                });
                }

                function Pie4() {

                    if (chartN != null) {


                        var $container = $('#containermain2').append('<div>');
                        window.chart = new Highcharts.Chart({
                            chart: {
                                height: aheightbar,
                                type: 'bar',
                                renderTo: $container[0]
                            },
                            title: {
                                text: 'DCF Management'
                            },
                            subtitle: {
                                text: 'Source: BizNETCTM'
                            },
                            xAxis: {
                                categories: finalprojectdcf,
                                title: {
                                    text: null
                                }
                            },
                            yAxis: {
                                min: 0,
                                title: {

                                },
                                labels: {
                                    overflow: 'justify'
                                }
                            },

                            plotOptions: {
                                bar: {
                                    dataLabels: {
                                        enabled: true
                                    }
                                }
                            },

                            credits: {
                                enabled: false
                            },

                            series: [{
                                name: $($get('<%= ddlColumnList3.ClientID%>')).val(),
                                data: finaldcfcount
                            }
                            ]



                        }


					);
                        }



                        $(function () {
                            var $container = $('#containermain2').append('<div>');
                            window.chart = new Highcharts.Chart({
                                chart: {
                                    height: aheightbar,
                                    type: 'bar',
                                    renderTo: $container[0]
                                },
                                title: {
                                    text: 'DCF Management'
                                },
                                subtitle: {
                                    text: 'Source:BizNETCTM'
                                },
                                xAxis: {
                                    categories: finalprojectdcf,
                                    title: {
                                        text: null
                                    }
                                },
                                yAxis: {
                                    min: 0,
                                    title: {

                                    },
                                    labels: {
                                        overflow: 'justify'
                                    }
                                },

                                plotOptions: {
                                    bar: {
                                        dataLabels: {
                                            enabled: true
                                        }
                                    }
                                },

                                credits: {
                                    enabled: false
                                },

                                series: [{
                                    name: $($get('<%= ddlColumnList3.ClientID%>')).val(),
                                    data: finaldcfcount
                                }
                                ]



                            }



					);



                        })

                        }
                        function Allbar1() {

                            if (chartN != null) {


                                var $container = $('#containermain2').append('<div>');
                                window.chart = new Highcharts.Chart({
                                    chart: {
                                        type: 'bar',
                                        renderTo: $container[0]
                                    },
                                    title: {
                                        text: 'DCF Management'
                                    },
                                    xAxis: {
                                        categories: finalprojectdcf,
                                    },
                                    yAxis: {
                                        min: 0,
                                        title: {
                                            text: 'Values'
                                        }
                                    },
                                    tooltip: {
                                        pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b> ({point.percentage:.0f}%)<br/>',
                                        shared: true
                                    },
                                    plotOptions: {
                                        series: {
                                            stacking: 'percent',
                                            dataLabels: {
                                                enabled: true
                                            }
                                        }
                                    },
                                    credits: {
                                        enabled: false
                                    },

                                    series: [{
                                        name: 'Query Generated to answer',
                                        data: []
                                    }, {
                                        name: 'Query answered to resloved',
                                        data: []
                                    }, {
                                        name: 'Query Generated to resolved',
                                        data: []
                                    }]
                                });


                            }







                            $(function () {
                                var $container = $('#containermain2').append('<div>');
                                window.chart = new Highcharts.Chart({
                                    chart: {
                                        type: 'bar',
                                        renderTo: $container[0]
                                    },
                                    title: {
                                        text: 'DCF Management'
                                    },
                                    xAxis: {
                                        categories: finalprojectdcf,
                                    },
                                    yAxis: {
                                        min: 0,
                                        title: {
                                            text: 'Values'
                                        }
                                    },
                                    tooltip: {
                                        pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b> ({point.percentage:.0f}%)<br/>',
                                        shared: true
                                    },
                                    plotOptions: {
                                        series: {
                                            stacking: 'percent',
                                            dataLabels: {
                                                enabled: true
                                            }
                                        }
                                    },
                                    credits: {
                                        enabled: false
                                    },

                                    series: [{
                                        name: 'Query Generated to answer',
                                        data: Querytoans
                                    }, {
                                        name: 'Query answered to resloved',
                                        data: Queryanstoresol
                                    }, {
                                        name: 'Query Generated to resolved',
                                        data: Querygentoresol
                                    }]




                                }



                                        );



                            })

                        }

                        function AllLinedcf() {
                            var $container = $('#containermain2').append('<div>');
                            if (chartN != null) {
                                window.chart = new Highcharts.Chart({
                                    chart: {
                                        height: 500,
                                        renderTo: $container[0]
                                    },
                                    title: {
                                        text: 'DCF Management',
                                        x: -20
                                    },
                                    subtitle: {
                                        text: 'Source: BizNETCTM',
                                        x: -20
                                    },
                                    xAxis: {
                                        title: {
                                            text: 'Project/Site'
                                        },
                                        categories: []
                                    },
                                    yAxis: {
                                        title: {
                                            text: 'Data'
                                        },
                                        plotLines: [{
                                            value: 0,
                                            width: 1,
                                            color: '#808080'
                                        }]
                                    },
                                    plotOptions: {

                                        series: {
                                            dataLabels: {
                                                enabled: true
                                            }
                                        }
                                    },
                                    legend: {
                                        layout: 'vertical',
                                        align: 'right',
                                        verticalAlign: 'middle',
                                        borderWidth: 0
                                    },
                                    credits: {
                                        enabled: false
                                    },
                                    series: [{
                                        name: 'Enrolled Subject',
                                        data: []
                                    },
                                    //{
                                      //  name: 'Rejected Subject',
                                        //data: []
                                    //}
                                    ]
                                });
                            }

                            $(function () {
                                var $container = $('#containermain2').append('<div>');
                                window.chart = new Highcharts.Chart({
                                    chart: {
                                        height: 500,
                                        renderTo: $container[0]
                                    },
                                    title: {
                                        text: 'DCF Management',
                                        x: -20
                                    },
                                    subtitle: {
                                        text: 'Source: BizNETCTN',
                                        x: -20
                                    },
                                    xAxis: {
                                        title: {
                                            text: 'Project/Site'
                                        },
                                        categories: finalprojectdcf
                                    },
                                    yAxis: {
                                        title: {
                                            text: 'Data'
                                        },
                                        plotLines: [{
                                            value: 0,
                                            width: 1,
                                            color: '#808080'
                                        }]
                                    },
                                    plotOptions: {

                                        series: {
                                            dataLabels: {
                                                enabled: true
                                            }
                                        }
                                    },
                                    legend: {
                                        layout: 'vertical',
                                        align: 'right',
                                        verticalAlign: 'middle',
                                        borderWidth: 0
                                    },
                                    credits: {
                                        enabled: false
                                    },
                                    series: [{
                                        name: 'Query Generated to answer',
                                        data: Querytoans
                                    },
                                     {
                                         name: 'Query answered to resloved',
                                         data: Queryanstoresol
                                     },
                                      {
                                          name: 'Query generated to resloved',
                                          data: Querygentoresol
                                      }


                                    ]
                                });
                                $(".content").height($(".wrapper").height());
                            });
                        }



                        /*-------------Dhruvi DCF Management Completed------------------------------------------------------------------------------------------------------------------*/
                        function gvtodt(gv) {

                            if ($(gv).length > 0) {
                                $(gv).find('tbody tr').length < 3 ? scroll = "25%" : scroll = "285px";
                                $(gv).prepend($('<thead>').append($(gv).find('tr:first'))).dataTable({
                                    "bDestroy": true,
                                    "bJQueryUI": true,
                                    "sScrollY": "285px",
                                    "sScrollX": "100%",
                                    "sPaginationType": "full_numbers",
                                    "bLengthChange": true,
                                    "iDisplayLength": 5,
                                    "bProcessing": true,
                                    "bSort": false,
                                    "bScrollCollapse": true,
                                    aLengthMenu: [
                                        [5, 10, 20, 50, -1],
                                        [5, 10, 20, 50, "All"]
                                    ],
                                    "oLanguage": {
                                        "sEmptyTable": "No Record Found",
                                    }

                                });
                                $(gv).find('tr:first').css('background-color', '#3A87AD');
                                $('tr', $('.dataTables_scrollHeadInner')).css("background-color", "rgb(58, 135, 173)");
                                setTimeout(function () { $(gv).dataTable().fnAdjustColumnSizing(); }, 10);
                            }
                        }

                        $($('#ctl00_CPHLAMBDA_ifviewTrainingDocument').contents()).scroll(function () {
                            msgalert('Frame Scrolled In Jquery');
                        });
        //Query Table
                        <%--function UIQueryDatatable() {
                            debugger;
                           pageJson = pageJson || undefined;
                              $('#<%=gvQueryDetails.ClientID%>').removeAttr('style', 'display:block');
                            oTabQuery = $('#<%=gvQueryDetails.ClientID%>').prepend($('<thead>').append($('#<%=gvQueryDetails.ClientID%> tr:first'))).dataTable({
                                "bJQueryUI": true,
                                "sPaginationType": "full_numbers",
                                "bLengthChange": true,
                                "iDisplayLength": 10,
                                "bProcessing": false,
                                "bSort": false,
                                "bFilter": true,
                                "bInfo": false,
                                "sScrollY": "250px",
                                "sScrollX": "100%",
                                "bScrollCollapse": false,
                                aLengthMenu: [
                                    [10, 25, 50, 100, -1],
                                    [10, 25, 50, 100, "All"]
                                ],
                            });
                            $(".dataTable thead th").each(function (i) {
                                if (i >= 6) return;
                                this.innerHTML += fnCreateSelect(oTabQuery.fnGetColumnData(i, true, false));
                                var sel = $('select', this).change(function () {
                                    selValue = $(this).val();
                                    selValue = selValue.replace(/&/g, "&amp;");
                                    oTabQuery.fnFilter(selValue, i);
                                }).click(function (event) {
                                    event.stopPropagation();
                                });
                                sel[0].id = this.abbr;
                                selectionList2[this.abbr] = sel;
                            });
                        }--%>
        // End QueryTable
                        //Visit Table
                        function UIVisitDatatable() {
                            pageJson = pageJson || undefined;
                            //console.log(pageJson);
                            // $("#ctl00_CPHLAMBDA_gvwCertificateReviewStatus").empty() // .dataTable().fnDestroy();                            
                            $('#<%=gvVisitReviewStatus.ClientID%>').removeAttr('style', 'display:block');
                            setTimeout(function () {
                                oTab = $('#<%=gvVisitReviewStatus.ClientID%>').prepend($('<thead>').append($('#<%=gvVisitReviewStatus.ClientID%> tr:first'))).dataTable({
                                    "bJQueryUI": true,
                                    "sPaginationType": "full_numbers",
                                    "bLengthChange": true,
                                    "iDisplayLength": 10,
                                    "bProcessing": true,
                                    "bSort": false,
                                    "bFilter": true,
                                    "bInfo": true,
                                    "sScrollY": "250px",
                                    "sScrollX": "100%",
                                    "sScrollXInner": "1300",
                                    //"bScrollCollapse": true,
                                    aLengthMenu: [
                                        [10, 25, 50, 100, -1],
                                        [10, 25, 50, 100, "All"]
                                    ],
                                    "columnDefs": [
                                            {
                                                "targets": [0],
                                                "visible": false,
                                                "searchable": false
                                            }],
                                });
                            
                            $(".dataTable thead th").each(function (i) {
                                if (i > 9) return;
                                if (i == 0) return;                                
                                this.innerHTML += fnCreateSelect(oTab.fnGetColumnData(i, true, false));
                                var sel = $('select', this).change(function () {
                                    selValue = $(this).val();
                                    selValue = selValue.replace(/&/g, "&amp;");
                                    oTab.fnFilter(selValue, i);
                                }).click(function (event) {
                                    event.stopPropagation();
                                });
                                sel[0].id = this.abbr;
                                selectionList[this.abbr] = sel;
                            });

                            function fnCreateSelect(aData) {
                                var r = '<select style="width:50px"><option value="">All</option>', i, iLen = aData.length;
                                //alert(aData.length);
                                for (i = 0; i < iLen; i++) {
                                    r += '<option value="' + aData[i] + '">' + aData[i] + '</option>';
                                }
                                return r + '</select>';
                            }
                            }, 100);
                        }                        

                        function UIAdjucatorDatatable() {
                            pageJson = pageJson || undefined;
                            // $("#ctl00_CPHLAMBDA_gvwCertificateReviewStatus").empty() // .dataTable().fnDestroy();

                            // $('#<%=btnSaveActivity.ClientID%>').removeAttr('style', 'display:none');
                            $('#<%=gvAdjucatorReviewStatus.ClientID%>').removeAttr('style', 'display:block');
                            setTimeout(function () {
                            oTabAdju = $('#<%=gvAdjucatorReviewStatus.ClientID%>').prepend($('<thead>').append($('#<%=gvAdjucatorReviewStatus.ClientID%> tr:first'))).dataTable({
                                "bJQueryUI": true,
                                "sPaginationType": "full_numbers",
                                "bLengthChange": true,
                                "iDisplayLength": 10,
                                "bProcessing": true,
                                "bSort": false,
                                "bFilter": true,
                                "bInfo": true,
                                "sScrollY": "250px",
                                "sScrollX": "100%",
                                "bScrollCollapse": false,
                                aLengthMenu: [
                                    [10, 25, 50, 100, -1],
                                    [10, 25, 50, 100, "All"]
                                ],
                            });
                            
                            $(".dataTable thead th").each(function (i) {
                                if (i >= 7) return;
                                if (i == 0) return;
                                this.innerHTML += fnCreateSelect(oTabAdju.fnGetColumnData(i, true, false));
                                var sel = $('select', this).change(function () {
                                    selValue = $(this).val();
                                    selValue = selValue.replace(/&/g, "&amp;");
                                    oTabAdju.fnFilter(selValue, i);
                                }).click(function (event) {
                                    event.stopPropagation();
                                });
                                sel[0].id = this.abbr;
                                selectionList2[this.abbr] = sel;
                            });
                            }, 100);
                        }

                        function fnCreateSelect(aData) {
                            var r = '<select style="width:50px"><option value="">All</option>', i, iLen = aData.length;
                            //alert(aData.length);
                            for (i = 0; i < iLen; i++) {
                                r += '<option value="' + aData[i] + '">' + aData[i] + '</option>';
                            }
                            return r + '</select>';
                        }

                        function SignAuthModalClose() {
                            document.getElementById('ctl00_CPHLAMBDA_QueryDiv').style.display = 'none';
                        }

                        function QueryDetailsFn() {
                            document.getElementById('ctl00_CPHLAMBDA_QueryDiv').style.display = 'block';
                        }

                       <%-- function UICADatatable() {
                            pageJson = pageJson || undefined;
                            $('#<%=gvCA.ClientID%>').removeAttr('style', 'display:block');
                            oTabCA = $('#<%=gvCA.ClientID%>').prepend($('<thead>').append($('#<%=gvCA.ClientID%> tr:first'))).dataTable({
                                "bJQueryUI": true,
                                "sPaginationType": "full_numbers",
                                "bLengthChange": true,
                                "iDisplayLength": 10,
                                "bProcessing": true,
                                "bSort": false,
                                "bFilter": true,
                                "bInfo": true,
                                "sScrollY": "250px",
                                "sScrollX": "100%",
                                "bScrollCollapse": false,
                                aLengthMenu: [
                                    [10, 25, 50, 100, -1],
                                    [10, 25, 50, 100, "All"]
                                ],
                            });
                        }--%>

<%--        function UIquerydetailstable() {
                            //pageJson = pageJson || undefined;
                            //console.log(pageJson);
                            // $("#ctl00_CPHLAMBDA_gvwCertificateReviewStatus").empty() // .dataTable().fnDestroy();
                            $('#<%=gvQueryMaster.ClientID%>').removeAttr('style', 'display:block');
                            oTab = $('#<%=gvQueryMaster.ClientID%>').prepend($('<thead>').append($('#<%=gvQueryMaster.ClientID%> tr:first'))).dataTable({
                                "bJQueryUI": true,
                                "sPaginationType": "full_numbers",
                                "bLengthChange": true,
                                "iDisplayLength": 10,
                                "bProcessing": true,
                                "bSort": false,
                                "bFilter": true,
                                "bInfo": false,
                                "sScrollY": "250px",
                                "sScrollX": "100%",
                                "sScrollXInner": "1400",
                                //"bScrollCollapse": true,
                                aLengthMenu: [
                                    [10, 25, 50, 100, -1],
                                    [10, 25, 50, 100, "All"]
                                ],
                                "columnDefs": [
                                        {
                                            //"targets": [0, 1, 2, 5, 9, 11, 15, 16, 17, 18, 20, 22, 23],

                                            "targets": [0],
                                            "visible": false,
                                            "searchable": false
                                        }],
                            });
                            //$(".dataTable thead th").each(function (i) {
                            //    if (i >= 9) return;
                            //    this.innerHTML += fnCreateSelect(oTab.fnGetColumnData(i, true, false));
                            //    var sel = $('select', this).change(function () {
                            //        selValue = $(this).val();
                            //        selValue = selValue.replace(/&/g, "&amp;");
                            //        oTab.fnFilter(selValue, i);
                            //    }).click(function (event) {
                            //        event.stopPropagation();
                            //    });
                            //    sel[0].id = this.abbr;
                            //    selectionList[this.abbr] = sel;
                            //});
        }--%>        

        function AuditTrailQueryFn(iQueryId) {
            if (iQueryId != "") {
                document.getElementById('ctl00_CPHLAMBDA_hdiQueryId').value = iQueryId;
                $.ajax({
                    type: "post",
                    url: "frmMainPage.aspx/AuditTrail",
                    data: '{"iQueryId":"' + iQueryId + '"}',
                    contentType: "application/json; charset=utf-8",
                    datatype: JSON,
                    success: function (response) {
                        document.getElementById('<%=QueryDiv.Clientid%>').style.display = 'block';
                        $('#AuditTrailTable').attr("IsTable", "has");
                        var aaDataSet = [];
                        var range = null;
                        response = JSON.parse(response.d);
                        for (var Row = 0; Row < response.length; Row++) {
                            var InDataSet = [];
                            InDataSet.push(response[Row].vProjectNo, response[Row].vMySubjectNo, response[Row].vNodeName,
                                response[Row].vQueryreply, response[Row].ChangeOn);
                            aaDataSet.push(InDataSet);
                        }
                        if ($("#AuditTrailTable").children().length > 0) {
                            $("#AuditTrailTable").dataTable().fnDestroy();
                            $("#AuditTrailTable").empty();
                        }

                        oTable = $('#AuditTrailTable').prepend($('<thead>').append($('#AuditTrailTable tr:first'))).dataTable({
                            paging: true,
                            scrollX: true,
                            lengthChange: true,
                            "bJQueryUI": true,
                            "sPaginationType": "full_numbers",
                            "bLengthChange": true,
                            "iDisplayLength": 10,
                            "bProcessing": true,
                            "bSort": false,
                            "aaData": aaDataSet,
                            "autowidth": false,
                            "aoColumns": [
                                   { "sTitle": "ProjectNo", },
                                   { "sTitle": "MySubjectNo" },
                                   { "sTitle": "Visit" },
                                   { "sTitle": "Remarks" },
                                   { "sTitle": "Change On(Uploader Date)" },
                            ],
                            "oLanguage": {
                                "sEmptyTable": "No Record Found",
                            }
                        });

                        oTable.fnAdjustColumnSizing();
                        $('.DataTables_sort_wrapper').click;
                        $('.dataTables_filter input').addClass('textBox');
                    },
                    failure: function (error) {
                        alert(error._message);
                        return false;
                    }
                });
                }
            }
        function exportPDF(nQueryNo) {
            var iQueryId = document.getElementById('ctl00_CPHLAMBDA_hdiQueryId').value
            var me = this;
            $.ajax({
                type: "POST",
                url: 'frmMainPage.aspx/exportToPDF',
                data: '{"nQueryNo":"' + nQueryNo + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                success: function (result) {
                    var fileName = document.getElementById('tblQueryMaster').rows[parseInt(0) + 2].cells[1].innerText + ".pdf"
                    //Convert Base64 string to Byte Array.
                    var bytes = Base64ToBytes(result.d);

                    //Convert Byte Array to BLOB.
                    var blob = new Blob([bytes], { type: "application/octetstream" });

                    //Check the Browser type and download the File.
                    var isIE = false || !!document.documentMode;
                    if (isIE) {
                        window.navigator.msSaveBlob(blob, fileName);
                    } else {
                        var url = window.URL || window.webkitURL;
                        var link = url.createObjectURL(blob);
                        var a = $("<a />");
                        a.attr("download", fileName);
                        a.attr("href", link);
                        $("body").append(a);
                        a[0].click();
                        $("body").remove(a);
                    }
                },
                failure: function (error) {
                    alert(error._message);
                    window.onUpdated();
                    return false;
                }
            });
        }
        Base64ToBytes = (base64) => {
            var s = window.atob(base64);
            var bytes = new Uint8Array(s.length);
            for (var i = 0; i < s.length; i++) {
                bytes[i] = s.charCodeAt(i);
            }
            return bytes;
        }

function ValidationForGo() {

    <%--if (document.getElementById('<%=txtVisitFromDate.Clientid%>').value == '') {
        msgalert('Please Enter From Date !');
        return false;
    }
    if (document.getElementById('<%=txtVisitToDate.Clientid%>').value == '') {
        msgalert('Please Enter To Date !');
        return false;
    }--%>

    if (document.getElementById('<%=txtVisitFromDate.Clientid%>').value != '') {
        if (document.getElementById('<%=txtVisitToDate.Clientid%>').value == '') {
            msgalert('Please Enter To Date !');
            return false;
        }
    }

    if (document.getElementById('<%=txtVisitToDate.Clientid%>').value != '') {
        if (document.getElementById('<%=txtVisitFromDate.Clientid%>').value == '') {
            msgalert('Please Enter From Date !');
            return false;
        }
    }
}

        //function ValidationForCA() {
        //    var vProjectId = $("#ctl00_CPHLAMBDA_txtprojectForCA").val();
        //    if (vProjectId == "") {
        //        msgalert("Please Select Project")
        //        return false;
        //    }
        //}

        //function bindQueryFn() {
        //    $.ajax({
        //        type: "post",
        //        url: "frmMainPage.aspx/FillQueryDetails",
        //        data: {},
        //        contentType: "application/json; charset=utf-8",
        //        datatype: JSON,
        //        success: function (response) {
        //            var tempTable = "";
        //            var aaDataSet = [];
        //            var range = null;
        //            tempTable = " <tboby> <tr class='trBorder'><thead><th class='trBorder'></th><th class='trBorder'>ProjectNo</th><th class='trBorder'>MySubjectNo</th><th class='trBorder'>Visit</th>" +
        //                         "<th class='trBorder'>Remarks</th><th class='trBorder'>Change On</th><th>Downloads</th</thead></tr> ";
        //            response = JSON.parse(response.d);
        //            if (response != null) {
        //                for (var Row = 0; Row < response.length; Row++) {
        //                    var InDataSet = [];
        //                    tempTable += "<tr data-toggle='collapse' data-target='#demo1' class='accordion-toggle' class='trBorder'><td><a alt='submit' class='btn btn-default btn-xs' style ='cursor: pointer' onclick='handleQueryDetails(" + response[Row].nQueryNo + ")'><i class='fa fa-eye'style='font-size:30px;'></i></a></td><td class='trBorder'>" + response[Row].vProjectNo + "</td><td class='trBorder'>" + response[Row].vMySubjectNo + "</td> " +
        //  " <td class='trBorder'>" + response[Row].vNodeDisplayName + "</td><td class='trBorder'>" + response[Row].vRemarks + "</td> " +
        //  " <td class='trBorder'>" + response[Row].dModifyOn + "</td>"+
        //  "<td olspan='12' class='hiddenRow'><a alt='submit' style ='cursor: pointer' onclick='exportPDF(" + response[Row].nQueryNo + ")'><i class='fa fa-download' style='font-size:30px;'></i></a></td></tr>";
        //                    tempTable += "<tr id='appendRow'></tr>";
        //                }
        //            }
        //            else { tempTable += "<tr class='trBorder'><td colSpan='9'>No Record Found..!</td></tr>" }
        //            tempTable += "</tboby>"
        //            document.getElementById('tblQueryMaster').innerHTML = tempTable;
        //        }
        //    });
        //}
        function handleQueryDetails(nQueryNo)
        {
            debugger;
            $.ajax({
                type: "post",
                url: "frmMainPage.aspx/AuditTrail",
                data:  '{"nQueryNo":"' + nQueryNo + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                success: function (response) {
                    var tempTable = "";
                    var aaDataSet = [];
                    var range = null;
                    tempTable = "<td colspan='12' class='hiddenRow'><div class='accordian-body collapse' id='demo1'> <table><thead> <tr>"+
                                "<th class='trBorder'></th><th class='trBorder'>ProjectNo</th><th class='trBorder'>MySubjectNo</th><th class='trBorder'>Visit</th>" +
                                "<th class='trBorder'>Remarks</th><th class='trBorder'>Change On</th></tr></thead> <tbody>";
                    response = JSON.parse(response.d);
                    if (response != null) {
                        for (var Row = 0; Row < response.length; Row++) {
                            var InDataSet = [];

                            tempTable += "<tr data-toggle='collapse'  class='accordion-toggle collapse' >" +
                                "<td class='trBorder'>" + response[Row].vProjectNo + "</td><td class='trBorder'>" + response[Row].vMySubjectNo + "</td> " +
                                " <td class='trBorder'>" + response[Row].vNodeDisplayName + "</td><td class='trBorder'>" + response[Row].vRemarks + "</td> " +
                                " <td class='trBorder'>" + response[Row].ChangeOn + "</td></tr> "
                        }
                    }
                    else { tempTable += "<tr class='trBorder'><td colSpan='9'>No Record Found..!</td></tr>" }

                    tempTable += "</tbody></table></div></td>"
                    document.getElementById('appendRow').innerHTML = tempTable;
                    //$('#appendRow').parent().append(tempTable);
                }
            });
        }
    </script> 
</asp:content>