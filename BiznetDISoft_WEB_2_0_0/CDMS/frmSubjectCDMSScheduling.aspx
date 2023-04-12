<%@ Page Language="VB" MasterPageFile="~/ECTDMasterPage.master" AutoEventWireup="false"
    CodeFile="frmSubjectCDMSScheduling.aspx.vb" Inherits="CDMS_frmSubjectCDMSScheduling" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <link rel="stylesheet" href="../App_Themes/CDMS.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/fullcalendar.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/fullcalendar.print.css"
        media="print" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/smoothnessjquery-ui.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/anytime.css" />

    <script type="text/javascript" src="../Script/General.js"></script>

    <script type="text/javascript" src="../Script/Jquery.js"></script>

    <script type="text/javascript" src="../Script/jquery-1.9.1.js"></script>

    <script type="text/javascript" src="../Script/jquery-migrate.js"></script>

    <script type="text/javascript" src="../Script/jquery-ui-1.10.2.custom.min.js"></script>

    <script type="text/javascript" src="../Script/anytime.js"></script>

    <script type="text/javascript" src="../Script/jquery-ui.js"></script>

    <script type="text/javascript" src="../Script/fullcalendar.min.js"></script>

    <script src="../Script/jquery.dataTables.min.js" type="text/javascript"></script>

    <script type="text/javascript" src="../Script/AutoComplete.js"></script>

    <style type="text/css">
        #wrap {
            width: 100%;
            height: auto;
        }

        .external-events {
            cursor: move;
        }

        #external {
            float: left;
            width: 27%;
            padding: 0 10px;
            border: 1px solid #ccc;
            text-align: left;
            margin-left: 2px;
            height: auto !important;
        }

        .fc-day-header {
            background: #7db9e8; /* Old browsers */
            background: -moz-linear-gradient(top, #7db9e8 1%, #7db9e8 46%, #2989d8 100%, #2989d8 100%, #2989d8 100%, #207cca 100%, #1e5799 100%) !important; /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(1%,#7db9e8), color-stop(46%,#7db9e8), color-stop(100%,#2989d8), color-stop(100%,#2989d8), color-stop(100%,#2989d8), color-stop(100%,#207cca), color-stop(100%,#1e5799)) !important; /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top, #7db9e8 1%,#7db9e8 46%,#2989d8 100%,#2989d8 100%,#2989d8 100%,#207cca 100%,#1e5799 100%) !important; /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top, #7db9e8 1%,#7db9e8 46%,#2989d8 100%,#2989d8 100%,#2989d8 100%,#207cca 100%,#1e5799 100%) !important; /* Opera 11.10+ */
            background: -ms-linear-gradient(top, #7db9e8 1%,#7db9e8 46%,#2989d8 100%,#2989d8 100%,#2989d8 100%,#207cca 100%,#1e5799 100%) !important; /* IE10+ */
            background: linear-gradient(to bottom, #7db9e8 1%,#7db9e8 46%,#2989d8 100%,#2989d8 100%,#2989d8 100%,#207cca 100%,#1e5799 100%) !important; /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr= '#7db9e8', endColorstr= '#1e5799',GradientType=0 ) !important; /* IE6-9 */
            color: #FFFFFF !important;
        }

        .fc-border-separate tr.fc-last th, .fc-border-separate tr.fc-last td {
            border-bottom-width: 1px !important;
        }

        .fc-button {
            background: #7db9e8; /* Old browsers */
            background: -moz-linear-gradient(top, #7db9e8 1%, #7db9e8 46%, #2989d8 100%, #2989d8 100%, #2989d8 100%, #207cca 100%, #1e5799 100%) !important; /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(1%,#7db9e8), color-stop(46%,#7db9e8), color-stop(100%,#2989d8), color-stop(100%,#2989d8), color-stop(100%,#2989d8), color-stop(100%,#207cca), color-stop(100%,#1e5799)) !important; /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top, #7db9e8 1%,#7db9e8 46%,#2989d8 100%,#2989d8 100%,#2989d8 100%,#207cca 100%,#1e5799 100%) !important; /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top, #7db9e8 1%,#7db9e8 46%,#2989d8 100%,#2989d8 100%,#2989d8 100%,#207cca 100%,#1e5799 100%) !important; /* Opera 11.10+ */
            background: -ms-linear-gradient(top, #7db9e8 1%,#7db9e8 46%,#2989d8 100%,#2989d8 100%,#2989d8 100%,#207cca 100%,#1e5799 100%) !important; /* IE10+ */
            background: linear-gradient(to bottom, #7db9e8 1%,#7db9e8 46%,#2989d8 100%,#2989d8 100%,#2989d8 100%,#207cca 100%,#1e5799 100%) !important; /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr= '#7db9e8', endColorstr= '#1e5799',GradientType=0 ) !important; /* IE6-9 */
            color: #FFFFFF !important;
        }

        .fc-widget-content:hover {
            /* background: linear-gradient(to bottom, rgba(247, 247, 247, 0.73) 0%,rgba(206, 227, 237, 1) 100%) !important;*/
            background: -moz-linear-gradient(top, rgba(247,247,247,0.73) 0%, rgba(206,227,237,1) 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,rgba(247,247,247,0.73)), color-stop(100%,rgba(206,227,237,1))); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top, rgba(247,247,247,0.73) 0%,rgba(206,227,237,1) 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top, rgba(247,247,247,0.73) 0%,rgba(206,227,237,1) 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top, rgba(247,247,247,0.73) 0%,rgba(206,227,237,1) 100%); /* IE10+ */
            background: linear-gradient(to bottom, rgba(247,247,247,0.73) 0%,rgba(206,227,237,1) 100%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr= '#baf7f7f7', endColorstr= '#cee3ed',GradientType=0 ); /* IE6-9 */
        }

        .fc-event-inner:hover {
            background: #1e5799; /* Old browsers */
            background: -moz-linear-gradient(top, #1e5799 0%, #2989d8 100%, #7db9e8 100%, #207cca 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#1e5799), color-stop(100%,#2989d8), color-stop(100%,#7db9e8), color-stop(100%,#207cca)); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top, #1e5799 0%,#2989d8 100%,#7db9e8 100%,#207cca 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top, #1e5799 0%,#2989d8 100%,#7db9e8 100%,#207cca 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top, #1e5799 0%,#2989d8 100%,#7db9e8 100%,#207cca 100%); /* IE10+ */
            background: linear-gradient(to bottom, #1e5799 0%,#2989d8 100%,#7db9e8 100%,#207cca 100%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr= '#1e5799', endColorstr= '#207cca',GradientType=0 ); /* IE6-9 */
            color: White !important;
            font-weight: bold;
        }

        .header {
            background: linear-gradient(to bottom, rgba(14, 59, 238, 0.73) 0%,rgba(157, 238, 238, 1) 100%) !important;
        }
        /*.fc-other-month
        {
            background: linear-gradient(to bottom, rgba(250, 250, 250, 0.73) 0%,rgba(218, 220, 240, 1) 100%) !important;
        }*/ #ctl00_CPHLAMBDA_divRemarks_DropShadow {
            background-color: Gray !important;
            opacity: 0.1 !important;
            position: relative !important;
            width: 10000px !important;
            height: 10000px !important;
        }

        #loading {
            background-color: Gray !important;
            opacity: 0.1 !important;
            position: relative !important;
            width: 10000px !important;
            height: 10000px !important;
        }

        .fc-event-inner {
            min-height: 25px !important;
            box-shadow: 6px 10px 10px #888888;
            border: 1px;
            font-size: 0.9em !important;
        }

        .fc-day {
            max-height: 150px !important;
            overflow: auto !important;
        }

        .subclr {
            font-weight: bold !important;
        }

        .fc-view-agendaDay {
            height: 10000px !important;
        }

        .fc-view-agendaWeek {
            height: 10000px !important;
        }

        #ui-datepicker-div {
            z-index: 99999999 !important;
        }

        .ui-datepicker-trigger {
            cursor: pointer !important;
        }

        .sorting {
            background-color: #3A87AD !important;
            color: #FFFFFF !important;
        }

        .sorting_asc {
            background-color: #3A87AD !important;
            color: #FFFFFF !important;
        }

        .divModalBackGround {
            top: 0px;
            left: 0px;
            background-color: gray;
            z-index: 999999999999 !important;
            opacity: 0.8;
            position: absolute;
            display: none;
            width: 100%;
            height: 1000px;
        }

        .updateImage {
            vertical-align: middle;
            height: 100px;
            width: 100px;
            background-repeat: no-repeat;
            border-radius: 6px;
            left: 50%;
            top: 15%;
            z-index: 99999999999999 !important;
        }

        .updateText {
            font-weight: bold;
            color: #227199;
            font-family: Verdana;
            font-variant: small-caps;
            font-size: 12pt;
        }

        .fc-day-number {
            cursor: pointer;
        }

        .fc-header-title {
            font-size: 9px !important;
        }

        .fc-header-right {
            text-transform: capitalize;
        }

        .titleproject {
            font-size: 14px !important;
            color: #000080;
        }

        #calendar {
            float: right;
            max-height: 480px !important;
            overflow: auto;
        }

        div#ctl00_CPHLAMBDA_CurrentDateEvent {
            z-index: 10001!important;
        }
    </style>

    <script type="text/javascript" language="javascript">
        var cMONTHNAMES = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug',
                   'Sep', 'Oct', 'Nov', 'Dec'];
        var rendermode = "";
        var events = "";

        //Global variables for function fnSubjectDtlCDMSStatus ---- Added by Pratik Soni

        var gSubjectId = "";
        var gWorkSpaceId = "";
        var flag_fnStatusDtlCDMSStatus = false;

        var globalOriginalEventObject = "";
        var globalSubjectId = "";
        //--------------------------------------


        function daysInMonth(month, year) {
            return new Date(year, month, 0).getDate();
        }
        function pageLoad() {
            //$(document).ready(function () { 



            //$('.fc-today').css('background-color','Turquoise');


            var SWorkFlowStageId = '<%= Session(S_WorkFlowStageId).ToString() %>';
            if (SWorkFlowStageId != "0") {
                document.getElementById('external').style.display = 'none';
                //document.getElementById('calendarcontainer').style.width = '100%';
                document.getElementById('calendar').style.width = '100%';
                $('.ChngStatus').css('display', 'none');
                $('.RmvSubject').css('display', 'none');

            }
            else {
                document.getElementById('DivViewProjectWise').style.display = 'none';
                //document.getElementById('calendarcontainer').style.width = parseInt($(window).width()) - 420 + 'px';
                //                     document.getElementById('calendar').style.width = parseInt($(window).width()) - 440 + 'px';
                //                     Slimwidth = parseInt($(window).width()) - 400 + 'px';
                //                      $("#calendarcontainer").slimScroll({
                //                         position: 'left',                           
                //                         height:'500px',
                //                         width: Slimwidth,
                //                         size: '10px',
                //                         color: '#FFFFFF',
                //                      // railVisible: true,
                //                         railColor: 'gray',
                //                         railOpacity: 0.3
                //                      // alwaysVisible: true 
                //                       });
            }

            var date = new Date();
            var d = date.getDate();
            var m = date.getMonth();
            var y = date.getFullYear();
            //mockjax.init();  //mock events (for test purposes)
            /* initialize the external events
            -----------------------------------------------------------------*/

            $('.external-events ').each(function () {

                // create an Event Object (http://arshaw.com/fullcalendar/docs/event_data/Event_Object/)
                // it doesn't need to have a start or end
                var eventObject = {
                    title: $(this).attr('id').replace("ctl00_CPHLAMBDA_GrdSubject_ctl02_", "") // use the element's id as the event title
                };

                // store the Event Object in the DOM element so we can get to it later
                $(this).data('eventObject', eventObject);

                // make the event draggable using jQuery UI
                $(this).draggable({
                    zIndex: 999,
                    revert: true,      // will cause the event to go back to its
                    revertDuration: 0  //  original position after the drag
                });

            });



            /* initialize the calendar
            -----------------------------------------------------------------*/
            $('#calendar').fullCalendar({

                header: {
                    left: 'prev,next ',
                    center: 'title',
                    right: 'today,month,agendaWeek,agendaDay'
                },
                firstDay: 1,
                selectable: true,
                selectHelper: true,
                editable: true,
                disableResizing: true,
                disableDragging: true,
                droppable: true,
                dragOpacity: .5,
                width: '100%',
                drop: function (date, allDay) {
                    debugger;
                    if (rendermode == "Save") {
                        rendermode = "";
                        var originalEventObject = $(this).attr('id').split('_')[4];
                        var copiedEventObject = $.extend({}, originalEventObject);
                        $(this).css('display', 'none');

                        document.getElementById('ctl00_CPHLAMBDA_hdnDraggedSubjectID').value = originalEventObject;

                        if (document.getElementById('ctl00_CPHLAMBDA_hdnElementData').value.split('##')[13] == "U") {
                            var DropeventData = document.getElementById('ctl00_CPHLAMBDA_hdnElementData').value;
                            Updateevents(originalEventObject, DropeventData);
                            document.getElementById('ctl00_CPHLAMBDA_hdnElementData').value = "";
                        }
                        else if (document.getElementById('ctl00_CPHLAMBDA_hdnElementData').value.split('##')[13] == "B") {
                            msgConfirmDeleteAlert(null, "Are You Sure You Want To Remove The Booked Subject From This Slot And Assign A New Subject?", function (isConfirmed) {
                                if (isConfirmed) {
                                    $find('BookSubjectRemark').show();
                                    $('#txtRemarkForBooking').val("");
                                    return true;
                                }
                            });
                            return false;
                        }

                    }
                    else if (rendermode == "NewSave") {
                        globalOriginalEventObject = $(this).attr('id').split('_')[4];
                        globalSubjectId = $(this).attr('id')
                        //$(this).css('display', 'none');
                    }
                    else if (rendermode == "") {

                        var originalEventObject = $(this).data('eventObject');
                        var copiedEventObject = $.extend({}, originalEventObject);
                        copiedEventObject.start = date;
                        copiedEventObject.allDay = allDay;
                        msgalert('Please Drop The Subject Over The Slots');
                        return false;
                        // $('#calendar').fullCalendar('renderEvent', copiedEventObject, true);     
                    }
                    else if (rendermode == "Cancel") {
                        rendermode = "";
                        return false;
                    }
                    else if (rendermode == "NotAccepted") {
                        msgalert('This Subject does not match for this project slot');
                        rendermode = "";
                        return false;
                    }

                },


                loading: function (bool) {
                    if (bool) {
                        $('.divModalBackGround').css('display', 'block');
                        return false;
                    }
                    else {
                        $('.divModalBackGround').css('display', 'none');
                    }
                },
                dayClick: function (start, allDay, jsEvent, view) {
                    // debugger ;
                    var startMonth = start.getMonth() + 1;
                    if (startMonth.toString().length == 1) {
                        var ScheduleMonth = "0" + startMonth;
                    }
                    else {
                        var ScheduleMonth = startMonth;
                    }
                    var startDay = start.getDate();
                    if (startDay.toString().length == 1) {
                        var ScheduleDay = "0" + startDay;
                    }
                    else {
                        var ScheduleDay = startDay;
                    }
                    var startYear = start.getFullYear().toString();
                    var currentDate = new Date();
                    var date = startDay + "-" + cMONTHNAMES[startMonth - 1] + "-" + startYear;
                    // var ScheduleDate =  ScheduleMonth + "/" + ScheduleDay + "/" + startYear;
                    var view = $('#calendar').fullCalendar('getView');
                    if (view.name == "month") {

                        //                     var wstr ="SELECT DISTINCT vProjectNo FROM VIEW_WorkspaceScreeningScheduleHdrDtl WHERE dScheduledate = '" + ScheduleDate +"'";
                        //                     var obj = new Object();
                        //                     obj.query = wstr.toString();
                        //                     var JsonText = JSON.stringify(obj);

                        //               $.ajax({
                        //                      type: "POST",
                        //                      url: "../WS_HelpDB_JSON.asmx/GetDataTableObjectBySqlQuery",
                        //                      data: JsonText,                      
                        //                      contentType: "application/json; charset=utf-8",
                        //                      dataType: "json",
                        //                      success: function(data) {
                        //                      //debugger ;
                        //                      var dataProject= $.parseJSON(data.d);
                        //                           if (dataProject.length != 0)
                        //                           {
                        //                             for (var a = 0; a < dataProject.length ; a++ )
                        //                             {
                        //                                $('#ctl00_CPHLAMBDA_lblSlotSubject').append(dataProject[a].vProjectNo + " ");
                        //                             }
                        //                           } 
                        //                           else 
                        //                           {
                        //                                $('#ctl00_CPHLAMBDA_lblSlotSubject').append("No Project Exists For Today");    
                        //                           }
                        //                           //$('#ctl00_CPHLAMBDA_lblSlotSubject').html("Subject Scheduling -" + " " + date);  
                        //                        },
                        //                       failure: function(error) 
                        //                       {
                        //                                msgalert(error);
                        //                       }
                        //                  });
                        $find('SelectedDate').show();
                        $('#ctl00_CPHLAMBDA_lblCurrentDate').html("Subject Scheduling -" + " " + date);
                        CurrrentDatePopupload();
                    }
                },
                //	         eventClick: function(calEvent, jsEvent, view, a, b, c, d) {
                //	         debugger ;
                //                 alert (calEvent ,jsEvent ,view, a, b, c, d);
                //                 $find('CALENDAR1').show();
                //             },
                //                eventDrop: function (event, dayDelta, minuteDelta, allDay, revertFunc) {

                //                    msgalert(event.title + " was moved " + dayDelta + " days and " + minuteDelta + " minutes.");

                //                    if (allDay) {
                //                        msgalert("Event is now all-day");
                //                    }
                //                    else {
                //                        msgalert("Event has a time-of-day");
                //                    }

                //                    if (!confirm("Are you sure about this change?")) {
                //                        revertFunc();
                //                    }
                //                },
                //                eventMouseover: function (event, jsEvent, view) {
                //                   var evntheight = $(jsEvent.currentTarget).height() + 30
                //                    
                //                    $(jsEvent.currentTarget).animate
                //                           ({ 
                //                               height: evntheight + "px",
                //                               zIndex: '999999' 
                //                            }, 'fast'
                //                            );
                //                },
                //                eventMouseout: function (event, jsEvent, view) {
                //                     
                //                    if ($(jsEvent.currentTarget).height() > 55)
                //                       {
                //                         var evntheight = $(jsEvent.currentTarget).height() - 30
                //                       }
                //                    else 
                //                       {
                //                         var evntheight = $(jsEvent.currentTarget).height()
                //                       }   
                ////                    alert ($(jsEvent.currentTarget).height());
                ////                    alert (evntheight);
                //                    $(jsEvent.currentTarget).animate
                //                           ({ 
                //                               height: evntheight + "px",
                //                               zIndex: '8' 
                //                            }, 'fast'
                //                            );
                //                },
                //                eventMouseover: function (event, jsEvent, view) {
                //                debugger ;
                //                     
                //                    $(jsEvent.currentTarget).animate({ cursor: 'cell', borderradius: '30px 30px 30px 30px',border: '1px solid cyan' }, 'fast'
                //                    );
                //                  },

                //                eventMouseout: function (event, jsEvent, view) {
                //debugger ;
                //                    $(jsEvent.currentTarget).animate({ cursor: 'cell', borderradius: '3',border: ''  }, 'fast'
                //                    );
                //                  },
                eventRender: function (event, element, view) {
                    var evnt = event.description
                    if (event.description.split('##')[13] == "B") {
                        var SWorkFlowStageId = '<%= Session(S_WorkFlowStageId).ToString() %>';
                        if (SWorkFlowStageId == "0") {
                            evnt = evnt.replace(/\'/g, '\\\'');
                            element.find(".fc-event-title").after($("<span class=\"fc-event-icons\"></span>").html('<img src="images/patientstatus.png" style="float:right;cursor:pointer;"  title ="Change Status Of The Subject" onclick ="return ChangeStatus(' + "'" + evnt + "'" + ');"/>'));
                            element.find(".fc-event-title").after($("<span class=\"fc-event-icons\"></span>").html('<img src="images/remove.png" style="float:right;cursor:pointer;"  title ="Remove Subject From This Slot" onclick ="return ValidateRemoveSubjectOnly(' + "'" + evnt + "'" + ');"/>'));
                        }
                    }

                    if (view.name == "agendaDay") {
                        $('.fc-day-content').css('minHeight', '500px');

                        // $('.fc-agenda-allday').css('maxHeight','5000px');
                    }
                    $('.fc-day-content').css('minHeight', '');
                    $('.fc-agenda-slots').css('display', 'none');
                    $('.fc-agenda-divider-inner').css('display', 'none');
                    $('.fc-agenda-divider').css('display', 'none');
                    //$('.fc-event').css('minHeight','30px');
                    element.droppable({
                        drop: function (date, allDay) {
                            if ($('#ctl00_CPHLAMBDA_hdnProjectForsubject').val() != "") {
                                if ($('#ctl00_CPHLAMBDA_hdnProjectForsubject').val().toString() != event.description.split("##")[10].toString()) {
                                    rendermode = "NotAccepted"
                                    return false;
                                }
                            }
                            rendermode = "NewSave";
                            msgConfirmDeleteAlert(null, "Are You Sure You Want To Book The Subject For This Slot?", function (isConfirmed) {
                                if (isConfirmed) {
                                    document.getElementById('ctl00_CPHLAMBDA_hdnElementData').value = event.description;

                                    rendermode = "";
                                    var originalEventObject = globalOriginalEventObject;

                                    var copiedEventObject = $.extend({}, originalEventObject);
                                    globalSubjectId = "#" + globalSubjectId;

                                    document.getElementById('ctl00_CPHLAMBDA_hdnDraggedSubjectID').value = originalEventObject;

                                    if (document.getElementById('ctl00_CPHLAMBDA_hdnElementData').value.split('##')[13] == "U") {
                                        $(globalSubjectId).css('display', 'none');
                                        var DropeventData = document.getElementById('ctl00_CPHLAMBDA_hdnElementData').value;
                                        Updateevents(originalEventObject, DropeventData);
                                        document.getElementById('ctl00_CPHLAMBDA_hdnElementData').value = "";
                                    }
                                    else if (document.getElementById('ctl00_CPHLAMBDA_hdnElementData').value.split('##')[13] == "B") {
                                        //msgConfirmDeleteAlert(null, "Are You Sure You Want To Remove The Booked Subject From This Slot And Assign A New Subject?", function (isConfirmed) {
                                        //    if (isConfirmed) {
                                        //        $find('BookSubjectRemark').show();
                                        //        $('#txtRemarkForBooking').val("");
                                        //        return true;
                                        //    }
                                        //});
                                        //return false;
                                        if (confirm("Are You Sure You Want To Remove The Booked Subject From This Slot And Assign A New Subject?")) {
                                            $(globalSubjectId).css('display', 'none');
                                            $find('BookSubjectRemark').show();
                                            $('#txtRemarkForBooking').val("");
                                            return true;
                                        }
                                    }
                                }
                                else {
                                    document.getElementById('ctl00_CPHLAMBDA_hdnElementData').value = "";
                                    rendermode = "Cancel";
                                }
                            });
                            //return false;

                            //if (confirm("Are You Sure You Want To Book The Subject For This Slot?")) {
                            //    document.getElementById('ctl00_CPHLAMBDA_hdnElementData').value = event.description;
                            //}
                            //else {
                            //    document.getElementById('ctl00_CPHLAMBDA_hdnElementData').value = "";
                            //    rendermode = "Cancel";
                            //}
                        }
                    });
                },
                //                 eventAfterRender: function(event, element, view) {
                //                 debugger ;
                //                  
                //                  },
                events: function (start, end, callBack) {
                    debugger;
                    $("#calendar").fullCalendar('removeEvents');
                    var start = new Date($('#calendar').fullCalendar('getDate'));
                    var startMonth = ((start.getMonth() + 1).toString().length > 1) ? (start.getMonth() + 1) : '0' + (start.getMonth() + 1).toString();
                    var startDay = ((start.getDate().toString() + '').length > 1) ? start.getDate().toString() : '0' + start.getDate().toString();
                    var startYear = start.getFullYear().toString();
                    var TotalDay = daysInMonth(startMonth, startYear);
                    var events = new Array();
                    var endDate = (((startMonth.toString().length > 1) ? startMonth : '0' + startMonth) + '/' + ((startDay.toString().length > 1) ? startDay : '0' + startDay) + '/' + startYear);
                    if ($('#ctl00_CPHLAMBDA_hdnProjectwiseSubject').val() != "Active") {
                        //var wstr = "SELECT * FROM View_WorkSpaceScreeningScheduleHdrDtl WHERE dScheduledate BETWEEN '" + startMonth + "/01/2013' AND '" + startMonth + "/31/2013' order by vProjectNO,dScheduleDate,dStartTime"
                        var wstr = "SELECT * FROM View_WorkSpaceScreeningScheduleHdrDtl WHERE dScheduleDateWithoutFormat >= '" + startMonth + "/01/" + startYear + "' and  dScheduleDateWithoutFormat <= '" + startMonth + "/" + TotalDay + "/" + startYear + "' order by vProjectNO,dScheduleDate,dStartTime"
                        //var wstr = "SELECT * FROM View_WorkSpaceScreeningScheduleHdrDtl WHERE dScheduleDateWithoutFormat BETWEEN '" + startMonth + "/01/" + startYear + "' AND '" + startMonth + "/" + TotalDay + "/" + startYear + "' order by vProjectNO,dScheduleDate,dStartTime"
                    }
                    else {
                        //var wstr = "SELECT * FROM View_WorkSpaceScreeningScheduleHdrDtl WHERE vWorkspaceId = '" + $('#ctl00_CPHLAMBDA_hdnProjectForsubject').val() + "' AND dScheduledate BETWEEN '" + startMonth + "/01/2013' AND '" + startMonth + "/31/2013' order by vProjectNO,dScheduleDate,dStartTime"
                        var wstr = "SELECT * FROM View_WorkSpaceScreeningScheduleHdrDtl WHERE vWorkspaceId = '" + $('#ctl00_CPHLAMBDA_hdnProjectForsubject').val() + "' AND dScheduleDateWithoutFormat >= '" + startMonth + "/01/" + startYear + "' and  dScheduleDateWithoutFormat <= '" + startMonth + "/" + TotalDay + "/" + startYear + "' order by vProjectNO,dScheduleDate,dStartTime"
                    }
                    var obj = new Object();
                    obj.query = wstr.toString();
                    var JsonText = JSON.stringify(obj);

                    $.ajax(
                                {
                                    type: "POST",
                                    url: "../WS_HelpDB_JSON.asmx/GetDataTableObjectBySqlQuery",
                                    data: JsonText,
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (data) {
                                        debugger;
                                        var dataSheduled = $.parseJSON(data.d);
                                        for (var i = 0; i < dataSheduled.length; i++) {
                                            if (dataSheduled[i].cStatusIndi != "D") {
                                                var event = new Object();
                                                event.id = dataSheduled[i].nWorkspaceScreeningScheduleNo + dataSheduled[i].nWorkSpaceScreeningScheduleHdrId;
                                                event.title = "(" + dataSheduled[i].dStartTime + ") " + " " + dataSheduled[i].vSubjectId + " " + "[" + dataSheduled[i].vProjectNo + "]" + " " + dataSheduled[i].vFirstName + " " + dataSheduled[i].vMiddleName + " " + dataSheduled[i].vSurName;
                                                event.start = dataSheduled[i].dScheduledate;
                                                event.borderColor = '#999';
                                                if (dataSheduled[i].cStatus == "B") {
                                                    event.color = '#E48330'; //#FF9429,#E78730,#EE8133
                                                }
                                                event.description = dataSheduled[i].nWorkspaceScreeningScheduleNo +
                                                                     "##" + dataSheduled[i].nWorkSpaceScreeningScheduleHdrId + "##" + dataSheduled[i].dScheduledate +
                                                                     "##" + dataSheduled[i].dStartTime + "##" + dataSheduled[i].vSubjectId +
                                                                     "##" + dataSheduled[i].iTranNo + "##" + dataSheduled[i].vRemarks +
                                                                     "##" + dataSheduled[i].cStatusIndi + "##" + dataSheduled[i].iModifyBy +
                                                                     "##" + dataSheduled[i].dModifyOn + "##" + dataSheduled[i].vWorkSpaceId +
                                                                     "##" + dataSheduled[i].vProjectNo + "##" + dataSheduled[i].nWorkSpaceScreeningScheduleDtlId +
                                                                     "##" + dataSheduled[i].cStatus + "##" + dataSheduled[i].vFirstName +
                                                                     "##" + dataSheduled[i].vMiddleName + "##" + dataSheduled[i].vSurName;
                                                events.push(event);
                                            }
                                        }
                                        callBack(events);
                                    },
                                    failure: function (error) {
                                        msgalert(error);
                                    }
                                });
                    weekMode: 'liquid';

                }


            });
            $('.fc-header-space').after($("<span class=\"fc-event-customcalendar\"  style ='margin-left:15px;'></span>").html('<input type="hidden" id="dp"/>')).after($("<span class=\"fc-event-refcal\"></span>").html('<img src="images/refreshCal.png" style="cursor:pointer;" title ="Refresh Calendar" onclick ="return RefreshCalendar();"/>'));

            $("#dp").datepicker({
                buttonImage: 'images/Calendar.png',
                buttonImageOnly: true,
                buttonText: 'Select Any Date To View The Scheduling',
                changeMonth: true,
                changeYear: true,
                showOn: 'both',
                onSelect: function (dateText) {
                    var selecteddate = dateText.split('/');
                    var currentmonth = parseInt(selecteddate[0], 10) - 1;
                    var currentdate = selecteddate[1];
                    var currentyear = selecteddate[2];
                    $("#calendar").fullCalendar('gotoDate', currentyear, currentmonth, currentdate);

                }
            });

            // });

            var bTable = $('#<%= GrdSubject.ClientID %>').prepend($('<thead>').append($('#<%= GrdSubject.ClientID %> tr:first'))).dataTable({
                "bStateSave": false,
                "bPaginate": true,
                "sPaginationType": "full_numbers",
                "bSort": true,
                "bDestory": true,
                "bRetrieve": true
            });
            $('#<%= GrdSubject.ClientID %> tr:first').css('background-color', '#3A87AD');




            $('.divModalBackGround').css('width', $(window).width())
        }



        function RefreshCalendar() {
            $('.titleproject').html("");
            document.getElementById('ctl00_CPHLAMBDA_hdnProjectwiseSubject').value = "";
            $('#calendar').fullCalendar('refetchEvents');
            $('#ctl00_CPHLAMBDA_txtprojectForsubject').val("");
            $('#ctl00_CPHLAMBDA_hdnProjectForsubject').val("");
            $('.titleproject').remove();
        }

        function RefreshPanel() {

            $('#ctl00_CPHLAMBDA_txtprojectForsubject').val("");
            $('#ctl00_CPHLAMBDA_HSubjectId').val("");
            $('#ctl00_CPHLAMBDA_txtSearchSubject').val("");
            $('#ctl00_CPHLAMBDA_hdnProjectForsubject').val("");
            $('#ctl00_CPHLAMBDA_GrdSubject').dataTable().fnDestroy();
            $('#ctl00_CPHLAMBDA_GrdSubject thead').remove();
            $('#ctl00_CPHLAMBDA_GrdSubject tbody').remove()
        }

        function ValidateRemoveSubjectOnly(data) {
            msgConfirmDeleteAlert(null, "Are You Sure You Want To Remove The Booked Subject From This Slot?", function (isConfirmed) {
                if (isConfirmed) {
                    document.getElementById('btnSubDelOk').style.display = '';
                    $find('Search1').show();
                    $('#txtDelSubjectRemark').val("");
                    document.getElementById('ctl00_CPHLAMBDA_hdnRemoveSubjectOnly').value = data;
                    gSubjectId = data.split('##')[4];
                    gWorkSpaceId = data.split('##')[10]; //Global variable for vWorkSpaceId
                }
                return false;
            });
        }

        function ChangeStatus(data) {
            msgConfirmDeleteAlert(null, "Are You Sure You Want To Change The Status Of Booked Subject?", function (isConfirmed) {
                if (isConfirmed) {
                    $('.Rbl').attr('checked', false)
                    $('#TRStatusHold').css('display', 'none');
                    $('#<%= StatusStartDate.ClientId %>').val("");
                    $('#<%= StatusEndDate.ClientId %>').val("");
                    document.getElementById('ctl00_CPHLAMBDA_hdnEventData').value = data;
                    $find('ModalChangeStatus').show();
                    //Added by Pratik Soni
                    gSubjectId = data.split('##')[4];
                    gWorkSpaceId = data.split('##')[10];
                    return true;
                };
            });
            return false;

            //if (confirm("")) {
            //    $('.Rbl').attr('checked', false)
            //    $('#TRStatusHold').css('display', 'none');
            //    $('#<%= StatusStartDate.ClientId %>').val("");
            //    $('#<%= StatusEndDate.ClientId %>').val("");
            //    document.getElementById('ctl00_CPHLAMBDA_hdnEventData').value = data;
            //    $find('ModalChangeStatus').show();
            //    //Added by Pratik Soni
            //    gSubjectId = data.split('##')[4];
            //    gWorkSpaceId = data.split('##')[10];
            //    //----------------------------------
            //}
            //else {
            //    return false;
            //}
        }

        function DeleteEvt() {
            var DeletedataDtl = [];
            var details = $('#ctl00_CPHLAMBDA_hdnRemoveSubjectOnly').val().split('##');


            $("#calendar").fullCalendar('removeEvents', details[0] + details[1]);
            id: details[0];
            title: "(" + details[3] + ") " + details[4] + " " + "[" + details[11] + "]" + " " + details[14] + " " + details[15] + " " + details[16];
            start: details[2];
            description: details[0] + '##' + details[1] + "##" + details[2] +
                                                      "##" + details[3] + "##" + details[4] +
                                                      "##" + details[5] + "##" + "" +
                                                      "##" + details[7] + "##" + details[8] +
                                                      "##" + details[9] + "##" + details[10] +
                                                      "##" + details[11] + "##" + details[12] +
                                                      "##" + details[13] + "##" + details[14] +
                                                      "##" + details[15] + "##" + details[16];

            var Assigncontentdtl = new Object();
            Assigncontentdtl.nWorkSpaceScreeningScheduleDtlId = details[12];
            Assigncontentdtl.nWorkSpaceScreeningScheduleHdrId = details[1];
            document.getElementById('ctl00_CPHLAMBDA_hdnEachhdrID').value = details[1];
            Assigncontentdtl.dStartTime = details[3];
            document.getElementById('ctl00_CPHLAMBDA_hdnEachStartime').value = details[3];
            Assigncontentdtl.vSubjectId = details[4];
            document.getElementById('ctl00_CPHLAMBDA_hdnEachSbjtID').value = "";
            Assigncontentdtl.iTranNo = "";
            Assigncontentdtl.vRemarks = $('#txtDelSubjectRemark').val();
            Assigncontentdtl.cStatusIndi = "E";
            Assigncontentdtl.iModifyBy = $('#ctl00_CPHLAMBDA_hdnSessionuserid').val();
            Assigncontentdtl.dModifyOn = "";
            Assigncontentdtl.nWorkspaceScreeningScheduleNo = details[0];
            document.getElementById('ctl00_CPHLAMBDA_hdnEachScheduleNo').value = details[0];
            Assigncontentdtl.cStatus = "U";
            Assigncontentdtl.mode = "DELETE";
            DeletedataDtl.push(Assigncontentdtl)
            var jsonText = JSON.stringify({ DeletedataDtl: DeletedataDtl });
            $.ajax({
                type: "POST",
                url: "../Ws_Lambda_JSON.asmx/Save_WorkSpaceScreeningScheduleDtl",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: jsonText,
                success: function (data) {
                    if (data.d == "Success") {
                        // $("#calendar").fullCalendar('refetchEvents');

                        UpdateDeletedEvent();
                        msgalert("Subject Unbooked Sucessfully ");
                        $('.divModalBackGround').css('display', 'none');
                        fnSubjectDtlCDMSStatus('', '', "AC");
                    }
                    else if (data.d == "NotSave") {         // Added By Jeet Patel
                        msgalert('This subject is already removed from this slot');
                        window.location.reload();
                    }                                       // ===================
                    else {
                        msgalert('Error While Removing Subject');
                    }
                },
                error: function (ex) {
                    msgalert("Error Occured In Removing Subject In Database ")
                    console.log(ex);
                }
            });

        }

        function Updateevents(dragobject, eventobject) {
            if (flag_fnStatusDtlCDMSStatus == true) {
                fnSubjectDtlCDMSStatus('', '', "AC");
            }
            $('.divModalBackGround').css('display', 'block');
            var DeletedataDtl = [];
            var details = eventobject.split('##');
            $("#calendar").fullCalendar('removeEvents', details[0] + details[1]);
            id: details[0];
            title: "(" + details[3] + ") " + details[4] + " " + "[" + details[11] + "]" + " " + details[14] + " " + details[15] + " " + details[16];
            start: details[2];
            description: details[0] + '##' + details[1] + "##" + details[2] +
                                                      "##" + details[3] + "##" + details[4] +
                                                      "##" + details[5] + "##" + "" +
                                                      "##" + details[7] + "##" + details[8] +
                                                      "##" + details[9] + "##" + details[10] +
                                                      "##" + details[11] + "##" + details[12] +
                                                      "##" + details[13] + "##" + details[14] +
                                                      "##" + details[15] + "##" + details[16];

            var Assigncontentdtl = new Object();
            Assigncontentdtl.nWorkSpaceScreeningScheduleDtlId = details[12];
            Assigncontentdtl.nWorkSpaceScreeningScheduleHdrId = details[1];
            document.getElementById('ctl00_CPHLAMBDA_hdnEachhdrID').value = details[1];
            Assigncontentdtl.dStartTime = details[3];
            document.getElementById('ctl00_CPHLAMBDA_hdnEachStartime').value = details[3];
            Assigncontentdtl.vSubjectId = dragobject;
            document.getElementById('ctl00_CPHLAMBDA_hdnEachSbjtID').value = dragobject;
            Assigncontentdtl.iTranNo = "";
            Assigncontentdtl.vRemarks = $('#txtRemarkForBooking').val();
            Assigncontentdtl.cStatusIndi = "E"
            Assigncontentdtl.iModifyBy = $('#ctl00_CPHLAMBDA_hdnSessionuserid').val();
            Assigncontentdtl.dModifyOn = "";
            Assigncontentdtl.nWorkspaceScreeningScheduleNo = details[0];
            document.getElementById('ctl00_CPHLAMBDA_hdnEachScheduleNo').value = details[0];
            Assigncontentdtl.cStatus = "B";
            Assigncontentdtl.mode = "EDIT";
            DeletedataDtl.push(Assigncontentdtl)
            var jsonText = JSON.stringify({ DeletedataDtl: DeletedataDtl });
            $.ajax({
                type: "POST",
                url: "../Ws_Lambda_JSON.asmx/Save_WorkSpaceScreeningScheduleDtl",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: jsonText,
                success: function (data) {
                    if (data.d == "Success") {
                        // $("#calendar").fullCalendar('refetchEvents');

                        UpdateRenderEvent();

                        msgalert("Subject Booked successfully ");

                        fnSubjectDtlCDMSStatus(dragobject, eventobject, "BO"); //Added By Pratik Soni

                        rendermode = false;

                    }
                    else if (data.d == "NotSave") {         // Added By Jeet Patel
                        msgalert("This Slot is already Booked");
                        window.location.reload();
                    }
                    else if (data.d == "NotBook") {
                        msgalert("This Subject is Already Booked.");
                        window.location.reload();
                    }                                   //=============================
                    else {
                        msgalert('Error While Assigning');
                    }
                },
                error: function (ex) {
                    msgalert("Error Occured In Saving Subject In Database!")
                    console.log(ex);
                }
            });
        }

        //Added by Pratik Soni

        function fnSubjectDtlCDMSStatus(dragobject, eventobject, status) {
            //debugger ;
            var content = new Object();
            var changeStatusDtl = new Array();

            if (flag_fnStatusDtlCDMSStatus == true) {

                content.vSubjectId = dragobject;
                content.vWorkSpaceId = eventobject;
            }
            else {
                if (status == "BO") {
                    content.vSubjectId = dragobject;
                    gWorkSpaceId = eventobject.split('##')[10]; //Global variable for vWorkSpaceId
                    content.vWorkSpaceId = gWorkSpaceId;
                }
                else {
                    content.vSubjectId = gSubjectId;
                    content.vWorkSpaceId = gWorkSpaceId;
                }
            }

            content.iTranNo = "";
            content.cStatus = status;
            content.iModifyBy = $('#ctl00_CPHLAMBDA_hdnSessionuserid').val();
            content.dModifyOn = "";
            content.cStatusIndi = "E";
            changeStatusDtl.push(content)

            var jsonText = JSON.stringify({ changeStatusDtl: changeStatusDtl });

            $.ajax({
                type: "POST",
                url: "../Ws_Lambda_JSON.asmx/Save_SubjectDtlCDMSStatus",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                //async: false,
                data: jsonText,
                success: function (data) {
                    if (data.d != "error") {
                        //UpdateRenderEvent();
                    }
                        //else if (data.d == "success") {
                        //    msgalert("Subject Booked successfully ");
                        //}
                    else {
                        msgalert('Error While Assigning');
                    }
                },
                error: function (ex) {
                    msgalert("ERROR Occured In changing status In Database!" + ex.toString());
                    console.log(ex);
                }
            });
            flag_fnStatusDtlCDMSStatus = false;
        }

        function fnChangeFlag() {
            //debugger ;
            flag_fnStatusDtlCDMSStatus = false;
        }

        //-----------------------------------------------------

        function UpdateRenderEvent(mod) {
            var wstr = "SELECT * FROM View_WorkSpaceScreeningScheduleHdrDtl Where nWorkSpaceScreeningScheduleHdrId = '" + $('#ctl00_CPHLAMBDA_hdnEachhdrID').val() + "' AND vSubjectId = '" +
                       $('#ctl00_CPHLAMBDA_hdnEachSbjtID').val() + "' AND dStartTime = '" + $('#ctl00_CPHLAMBDA_hdnEachStartime').val() + "'" +
                       " AND nWorkspaceScreeningScheduleNo = '" + $('#ctl00_CPHLAMBDA_hdnEachScheduleNo').val() + " ' order by vProjectNO,dScheduleDate,dStartTime"
            var obj = new Object();
            obj.query = wstr.toString();
            var JsonText = JSON.stringify(obj);

            $.ajax(
                    {
                        type: "POST",
                        url: "../WS_HelpDB_JSON.asmx/GetDataTableObjectBySqlQuery",
                        data: JsonText,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            var DeletedataDtl = $.parseJSON(data.d);

                            $('#calendar').fullCalendar('renderEvent', {
                                id: DeletedataDtl[0].nWorkspaceScreeningScheduleNo + DeletedataDtl[0].nWorkSpaceScreeningScheduleHdrId,
                                title: "(" + DeletedataDtl[0].dStartTime + ")" + " " + DeletedataDtl[0].vSubjectId + "[" + DeletedataDtl[0].vProjectNo + "]" + " " + DeletedataDtl[0].vFirstName + " " + DeletedataDtl[0].vMiddleName + " " + DeletedataDtl[0].vSurName,
                                start: DeletedataDtl[0].dScheduledate,
                                color: '#E48330',
                                description: DeletedataDtl[0].nWorkspaceScreeningScheduleNo + "##" + DeletedataDtl[0].nWorkSpaceScreeningScheduleHdrId +
                                                      "##" + DeletedataDtl[0].dScheduledate +
                                                      "##" + DeletedataDtl[0].dStartTime + "##" + DeletedataDtl[0].vSubjectId +
                                                      "##" + DeletedataDtl[0].iTranNo + "##" + DeletedataDtl[0].vRemarks +
                                                      "##" + DeletedataDtl[0].cStatusIndi + "##" + DeletedataDtl[0].iModifyBy +
                                                      "##" + DeletedataDtl[0].dModifyOn + "##" + DeletedataDtl[0].vWorkSpaceId +
                                                      "##" + DeletedataDtl[0].vProjectNo + "##" + DeletedataDtl[0].nWorkSpaceScreeningScheduleDtlId +
                                                      "##" + DeletedataDtl[0].cStatus + "##" + DeletedataDtl[0].vFirstName +
                                                      "##" + DeletedataDtl[0].vMiddleName + "##" + DeletedataDtl[0].vSurName
                            }, true);
                            $find('BookSubjectRemark').hide();
                            $('#txtRemarkForBooking').val("");
                            DeletedataDtl = [];
                            $('.divModalBackGround').css('display', 'none');
                        },
                        failure: function (error) {
                            msgalert(error);
                        }
                    });

        }
        function UpdateDeletedEvent() {

            var wstr = "SELECT * FROM View_WorkSpaceScreeningScheduleHdrDtl Where nWorkSpaceScreeningScheduleHdrId = '" + $('#ctl00_CPHLAMBDA_hdnEachhdrID').val() +
                       " ' AND dStartTime = '" + $('#ctl00_CPHLAMBDA_hdnEachStartime').val() + "'" +
                       " AND nWorkspaceScreeningScheduleNo = '" + $('#ctl00_CPHLAMBDA_hdnEachScheduleNo').val() + " ' order by vProjectNO,dScheduleDate,dStartTime"
            var obj = new Object();
            obj.query = wstr.toString();
            var JsonText = JSON.stringify(obj);

            $.ajax(
                    {
                        type: "POST",
                        url: "../WS_HelpDB_JSON.asmx/GetDataTableObjectBySqlQuery",
                        data: JsonText,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            var DeletedataDtl = $.parseJSON(data.d);

                            $('#calendar').fullCalendar('renderEvent', {
                                id: DeletedataDtl[0].nWorkspaceScreeningScheduleNo + DeletedataDtl[0].nWorkSpaceScreeningScheduleHdrId,
                                title: "(" + DeletedataDtl[0].dStartTime + ")" + " " + DeletedataDtl[0].vSubjectId + "[" + DeletedataDtl[0].vProjectNo + "]" + " " + DeletedataDtl[0].vFirstName + " " + DeletedataDtl[0].vMiddleName + " " + DeletedataDtl[0].vSurName,
                                start: DeletedataDtl[0].dScheduledate,
                                description: DeletedataDtl[0].nWorkspaceScreeningScheduleNo + "##" + DeletedataDtl[0].nWorkSpaceScreeningScheduleHdrId +
                                                      "##" + DeletedataDtl[0].dScheduledate +
                                                      "##" + DeletedataDtl[0].dStartTime + "##" + DeletedataDtl[0].vSubjectId +
                                                      "##" + DeletedataDtl[0].iTranNo + "##" + DeletedataDtl[0].vRemarks +
                                                      "##" + DeletedataDtl[0].cStatusIndi + "##" + DeletedataDtl[0].iModifyBy +
                                                      "##" + DeletedataDtl[0].dModifyOn + "##" + DeletedataDtl[0].vWorkSpaceId +
                                                      "##" + DeletedataDtl[0].vProjectNo + "##" + DeletedataDtl[0].nWorkSpaceScreeningScheduleDtlId +
                                                      "##" + DeletedataDtl[0].cStatus + "##" + DeletedataDtl[0].vFirstName +
                                                      "##" + DeletedataDtl[0].vMiddleName + "##" + DeletedataDtl[0].vSurName
                            }, true);

                            DeletedataDtl = [];
                            $find('Search1').hide();
                            $('#txtDelSubjectRemark').val("");
                        },
                        failure: function (error) {
                            msgalert(error);
                        }
                    });
        }


        function CurrrentDatePopupload() {
            // for Reset All Html Controls Of Popup
            $('#ctl00_CPHLAMBDA_txtproject').val("");
            $('#txtstartTime').val("");
            $('#txtendTime').val("");
            $('#txtsubjects').val("");
            $('#txttimelength').val("");
            $('#DTimeLength').val("");
            //$('#ctl00_CPHLAMBDA_lblSlotSubject').html("");
            $('.DivRadio').html("");
            document.getElementById('SheduleSave').style.display = "none";
            document.getElementById('tbldate').style.display = "none";
            document.getElementById('tdDDlLocation').style.display = "none";
            document.getElementById('tdlocation').style.display = "none";
            document.getElementById('btndeleteall').style.display = "none";
            Sheduledtl = [];
            if ($("#Tableshedule tr")[0] != undefined) {
                $("#Tableshedule").dataTable().fnDestroy();
                $('#Tableshedule').html("");
            }
            if ($('#AnyTime--txtstartTime').html() != undefined) {

                AnyTime.noPicker("txtstartTime");
                //              $("#txtstartTime").remove();
                //              $("#txtstartTime").AnyTime_noPicker().remove();
            }
            if ($('#AnyTime--txtendTime').html() != "") {
                AnyTime.noPicker("txtendTime");
            }

            //**************************************
            fillddlLocation();
        }

        function fillddlLocation() {
            var wstr = "select vLocationName,vLocationCode  from LocationMst"
            $("#ctl00_CPHLAMBDA_ddlLoction").html("");
            var obj = new Object();
            obj.query = wstr.toString();
            var JsonText = JSON.stringify(obj);

            $.ajax(
                    {
                        type: "POST",
                        url: "../WS_HelpDB_JSON.asmx/GetDataTableObjectBySqlQuery",
                        data: JsonText,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            var datalocation = $.parseJSON(data.d);
                            $("#ctl00_CPHLAMBDA_ddlLoction").append($("<option>Select Location</option>").attr("value", 0));
                            for (var i = 0; i < datalocation.length; i++) {
                                $("#ctl00_CPHLAMBDA_ddlLoction").append($("<option></option>").attr("value", datalocation[i].vLocationCode).text(datalocation[i].vLocationName));
                            }
                            //for opening Timer pickup div
                            $("#txtstartTime").AnyTime_picker(
                           {
                               format: "%H:%i", labelTitle: "Start Time",
                               labelHour: "Hours", labelMinute: "Minutes"
                           });

                            $("#txtendTime").AnyTime_picker(
                           {
                               format: "%H:%i", labelTitle: "End Time",
                               labelHour: "Hours", labelMinute: "Minutes"
                           });
                            //*****************************************
                        },
                        failure: function (error) {
                            msgalert(error);
                        }
                    });
        }

        function ValidationRemarkForRemove() {
            if ($('#txtDelSubjectRemark').val().length <= 0) {
                msgalert('Please Enter Remarks');
                return false;
            }
            else {
                $('.divModalBackGround').css('display', 'block');
                DeleteEvt();
                return true;
            }
        }
        function ValidateSubjectBooking() {

            if ($('#txtRemarkForBooking').val().length <= 0) {
                msgalert('Please Enter Remarks');
                return false;
            }
            else {
                $('.divModalBackGround').css('display', 'block');
                flag_fnStatusDtlCDMSStatus = true;
                fnSubjectDtlCDMSStatus(document.getElementById('ctl00_CPHLAMBDA_hdnElementData').value.split("##")[4], document.getElementById('ctl00_CPHLAMBDA_hdnElementData').value.split("##")[10], "AC");
                fnUpdateStatus();
                return true;
            }
        }

        function fnUpdateStatus() {
            var content = {};
            content.SubjectId = document.getElementById('ctl00_CPHLAMBDA_hdnElementData').value.split("##")[4];
            content.ColumnName = "cStatus";
            content.TableName = "SUBJECTDTLCDMS";
            content.ChangedValue = "AC";
            content.Remarks = "";
            content.StartDate = "";
            content.EndDate = "";
            $.ajax({
                type: "POST",
                url: "frmSubjectCDMSScheduling.aspx/UpdateFieldValues",
                data: JSON.stringify(content),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d == true) {
                        var originalEventObject = document.getElementById('ctl00_CPHLAMBDA_hdnDraggedSubjectID').value;
                        var DropeventData = document.getElementById('ctl00_CPHLAMBDA_hdnElementData').value;
                        Updateevents(originalEventObject, DropeventData);
                    }
                    else {
                        msgalert(data.d);
                    }
                },
                failure: function (error) {
                    msgalert(error);
                }
            });
        }


        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

    </script>

    <asp:UpdatePanel ID="upScheduling" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <table width="100%">
                <tr style="height: 20px;">
                    <td></td>
                </tr>
            </table>
            <div id='Div1' class="divModalBackGround">
                <div style="width: 20%; margin-top: 20%; border-width: 1px; border-style: solid; border-color: #227199; background-color: #BFE2F5; box-shadow: 0 0 10px 2px #000000;">
                    <table width="100%" style="height: 130px; text-align: right;">
                        <tr>
                            <td>
                                <font class="updateText">Please Wait...</font>
                            </td>
                            <td>
                                <div title="Wait" class="updateImage">
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div style="text-align: left;" id="DivViewProjectWise">
                <table width="40%" style="margin-bottom: 2%;">
                    <tr>
                        <td style="width: 20%; text-align: right;">
                            <asp:Label Text="Enter Project :" ID="lblProjectForview" runat="server" />
                        </td>
                        <td style="text-align: left;">
                            <%-- <img id="ImgProjectForview" alt="ProjectSchedule" title="Get Project Wise Scheduling"
                                src="images/projectscheduling.png" style="cursor: pointer; float: right;" onclick="return Validateprojectwise();" />--%>
                            <asp:TextBox ID="txtProjectForview" runat="server" CssClass="TextBox" placeholder="Please Enter Project No..."
                                Width="90%" />
                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" UseContextKey="True"
                                TargetControlID="txtProjectForview" ServicePath="~/AutoComplete.asmx" OnClientShowing="ClientPopulatedprojectForsubjectView"
                                OnClientItemSelected="OnSelectedprojectForsubjectView" MinimumPrefixLength="1"
                                ServiceMethod="GetMyProjectCompletionList" CompletionListElementID="pnlSubjectForProjectview"
                                CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender4">
                            </cc1:AutoCompleteExtender>
                            <asp:Panel ID="pnlSubjectForProjectview" runat="server" Style="max-height: 150px; overflow: auto; overflow-x: hidden;" />
                            <input type="button" id="BtnProjectForview" style="display: none;" onclick="return Validateprojectwise();" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id='wrap'>
                <div id='external' class="FieldSetBox">
                    <%--<div id="Buttons" style="margin: auto; height: auto; margin-bottom: 2%;">
                        <input type="button" id="btnSearch" name="Search" value="Search" class="ButtonText"
                            style="font-size: 12px !important;" title="Search" />
                        <input type="button" id="btnAdd" name="Add" value="Add" class="ButtonText" style="font-size: 12px !important;
                            float: right; width: 70px;" title="Add" />
                    </div>--%>
                    <div style="margin-bottom: 4%; margin-top: 2%;">
                        <table width="100%">
                            <tr>
                                <td style="width: 65%;">
                                    <asp:TextBox ID="txtprojectForsubject" runat="server" CssClass="TextBox" placeholder="Please Enter Project No..."
                                        Width="100%" onblur="return ResetProject();" />
                                    <input type="button" id="btnsetsubjectForProject" style="display: none;" onclick="return ResetGrid();" />
                                    <%--<asp:Button ID="btnMatch" runat="server" CssClass="ButtonText" Style="font-size: 12px !important;
                            float: right; width: 70px;" Text="Match" ToolTip="Match Subject"  />--%>
                                    <asp:HiddenField ID="hdnProjectForsubject" runat="server" />
                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" UseContextKey="True"
                                        TargetControlID="txtprojectForsubject" ServicePath="~/AutoComplete.asmx" OnClientShowing="ClientPopulatedprojectForsubject"
                                        OnClientItemSelected="OnSelectedprojectForsubject" MinimumPrefixLength="1" ServiceMethod="GetMyProjectCompletionList"
                                        CompletionListElementID="pnlSubjectForProject" CompletionListItemCssClass="autocomplete_listitem"
                                        CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListCssClass="autocomplete_list"
                                        BehaviorID="AutoCompleteExtender2">
                                    </cc1:AutoCompleteExtender>
                                    <asp:Panel ID="pnlSubjectForProject" runat="server" Style="max-height: 150px; overflow: auto; overflow-x: hidden;" />
                                </td>
                                <td style="padding-left: 4%">
                                    <img id="btnprojectwisescheduling" alt="ProjectSchedule" title="Get Project Wise Scheduling"
                                        src="images/projectscheduling.png" style="cursor: pointer;" onclick="return Validateprojectwise();" />
                                    <asp:ImageButton ID="btnMatch" runat="server" ToolTip="Match Subjects According To Criteria"
                                        Style="cursor: pointer;" ImageUrl="~/CDMS/images/patientmatch.png" OnClientClick="return ValidateProjectMatch();" />
                                    <img id="btnSearch" alt="Search" title="Advance Search For Subjects" style="cursor: pointer;"
                                        src="images/advancesearch.png" onclick="return ValidateSearch();" />
                                    <img id="btnAdd" alt="Add" title="Add Subject Information" src="images/addsubject.png"
                                        style="cursor: pointer;" onclick="return ValidateAdd();" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="margin-top: 2%; margin-bottom: 2%;">
                        <img src="images/refreshCal.png" style="cursor: pointer; float: right;" title="Refresh Panel"
                            onclick="return RefreshPanel();" />
                        <asp:TextBox ID="txtSearchSubject" runat="server" Width="90%" CssClass="TextBox"
                            placeholder="Please Enter Subject ID Here..." Style="margin-left: 1%" onblur="return ResetSubject();" />
                        <asp:Button Style="display: none;" ID="btnSearchSubject" runat="server" />
                        <%--<input type="button" id="btnSearchSubject" style="display: none;" />--%>
                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" ServicePath="~/AutoComplete.asmx"
                            OnClientShowing="ClientPopulatedForSubjectSearch" CompletionSetCount="10" OnClientItemSelected="OnSelectedForSubjectSearch"
                            UseContextKey="True" MinimumPrefixLength="1" ServiceMethod="GetCDMSSubjectCompletionListActive"
                            CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                            CompletionListElementID="PanelSubjectSearch" CompletionListCssClass="autocomplete_list"
                            BehaviorID="AutoCompleteExtender3" TargetControlID="txtSearchSubject">
                        </cc1:AutoCompleteExtender>
                        <asp:HiddenField ID="HSubjectId" runat="server" />
                        <asp:Panel ID="PanelSubjectSearch" runat="server" Style="max-height: 150px; overflow: auto; overflow-x: hidden;" />
                    </div>
                    <div id="SubjectContainer" style="width: 100%; max-height: 600px;">
                        <asp:UpdatePanel ID="Updatepanel1" runat="server" RenderMode="Inline" UpdateMode="Always">
                            <ContentTemplate>
                                <div id="GridViewContainer" style="width: 100%; min-height: 100px;">
                                    <%--<asp:GridView ID="GrdSubject" Width="100%" runat="server" AutoGenerateColumns="false"
                                        AlternatingRowStyle-BackColor="#CEE3ED" OnRowDataBound="GrdSubject_RowDataBound"
                                        AllowPaging="True" PageSize="12" ShowFooter="True" FooterStyle-BackColor="#CEE3ED"
                                        FooterStyle-ForeColor="White" FooterStyle-HorizontalAlign="Center">--%>
                                    <asp:GridView ID="GrdSubject" Width="100%" runat="server" AutoGenerateColumns="false"
                                        OnRowDataBound="GrdSubject_RowDataBound">
                                        <HeaderStyle BackColor="#3A87AD" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="41%" HeaderText="Subject Details" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-ForeColor="White" ItemStyle-Width="41%">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="lblSubjectCode" runat="server" Target="_blank">
                                                <%--<asp:Label ID="lblSubjectCode" runat="server" CssClass="subclr" />--%>
                                                    </asp:HyperLink>
                                                    <br />
                                                    <asp:Label ID="lblSubject" runat="server" CssClass="labeltext" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="vSubjectAge" HeaderText="Subject Age" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-ForeColor="White" />
                                            <asp:BoundField DataField="vSubjectID" HeaderText="Subject Code" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-ForeColor="White" />
                                            <asp:TemplateField HeaderText="Drag" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-ForeColor="White" HeaderStyle-Width="30%">
                                                <ItemTemplate>
                                                    <asp:Image ID="imgDrag" runat="server" CssClass="external-events" AlternateText="Drag" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="vFirstName" HeaderText="First Name" />
                                            <asp:BoundField DataField="vSurName" HeaderText="Sur Name" />
                                            <asp:BoundField DataField="cSex" HeaderText="Sex" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div id='calendar' style="width: 70%;">
                </div>
                <asp:Button ID="btnAddToGrid" runat="server" Style="display: none;" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnMatch" EventName="Click" />
            <%-- <asp:PostBackTrigger ControlID="btnAdvanceSearchExport" />--%>
        </Triggers>
    </asp:UpdatePanel>
    <%--This Modal pop-up is for Removing Subject From Slot Remarks--%>
    <button id="btnSearchpopup" runat="server" style="display: none;" />
    <cc1:ModalPopupExtender ID="Search" runat="server" PopupControlID="divSearch" BackgroundCssClass="modalBackground"
        TargetControlID="btnSearchpopup" BehaviorID="Search1" CancelControlID="Closesearch">
    </cc1:ModalPopupExtender>
    <div id="divSearch" runat="server" class="centerModalPopup" style="display: none; left: 521px; width: 25%; position: absolute; top: 525px; max-height: 404px;">
        <table cellpadding="5" style="width: 100%">
            <tr>
                <td class="LabelText" style="text-align: left !important; font-size: 12px !important; width: 97%;">Remarks
                </td>
                <td style="text-align: center; height: 22px;" valign="top">
                    <img id="Closesearch" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px; cursor: pointer;"
                        title="Close" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlMedEx" runat="server" Visible="true">
            <table width="100%">
                <tr>
                    <td class="LabelText" colspan="2" style="text-align: left;">Enter Remark* :
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <textarea id="txtDelSubjectRemark" rows="3" cols="5" style="width: 97%; height: 63px;"
                            class="TextBox"></textarea>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <input type="button" value="OK" title="Enter Remark For Removing The Subject" id="btnSubDelOk"
                            onclick="return ValidationRemarkForRemove(); " class="btn btnsave" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <button id="btnSubjectbook" runat="server" style="display: none;" />
    <cc1:ModalPopupExtender ID="BookSubjectRemark" runat="server" PopupControlID="divRemarkForBookingSubject"
        BackgroundCssClass="modalBackground" TargetControlID="btnSubjectbook" BehaviorID="BookSubjectRemark"
        CancelControlID="CloseSubjectRemark">
    </cc1:ModalPopupExtender>
    <div id="divRemarkForBookingSubject" runat="server" class="centerModalPopup" style="display: none; left: 521px; width: 25%; position: absolute; top: 525px; max-height: 404px;">
        <table cellpadding="5" style="width: 100%">
            <tr>
                <td class="LabelText" style="text-align: left !important; font-size: 12px !important; width: 97%;">Remarks
                </td>
                <td style="text-align: center; height: 22px;" valign="top">
                    <img id="CloseSubjectRemark" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px; cursor: pointer;"
                        title="Close" onclick="fnChangeFlag();" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
        </table>
        <asp:Panel ID="Panel1" runat="server" Visible="true">
            <table width="100%">
                <tr>
                    <td class="LabelText" colspan="2" style="text-align: left;">Enter Remark* :
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <textarea id="txtRemarkForBooking" rows="3" cols="5" style="width: 97%; height: 63px;"
                            class="TextBox"></textarea>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <input type="button" value="OK" title="Enter Remark For Asssigning A New Subject"
                            id="btnBookSubjectOK" onclick="return ValidateSubjectBooking(); " />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <%--This Pop-up is For Advance Search--%>
    <button id="btnAdvanceSearchpopup" runat="server" style="display: none;" />
    <cc1:ModalPopupExtender ID="AdvanceSearchpopup" runat="server" PopupControlID="divAdvanceSearch"
        BackgroundCssClass="modalBackground" TargetControlID="btnAdvanceSearchpopup"
        BehaviorID="AdvanceSearchpopup" CancelControlID="CancelAdvanceSearch">
    </cc1:ModalPopupExtender>
    <div id="divAdvanceSearch" runat="server" class="centerModalPopup" style="display: none; left: 15%; width: 80%; position: absolute; top: 10% !important; max-height: 85%;">
        <table style="width: 100%">
            <tr>
                <td class="LabelText" style="text-align: center; font-size: 12px !important;">Advance Search
                </td>
                <td class="Label" style="text-align: right; width: 4%;" valign="top">
                    <img id="CancelAdvanceSearch" alt="Close" src="images/Close.png" style="position: relative; float: right; right: 5px; cursor: pointer;"
                        title="Close" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
        </table>
        <asp:Panel ID="Panel2" runat="server" Visible="true" Style="overflow: auto;">
            <div id="divtblSearch" style="width: 100%; max-height: 110px; overflow: auto;">
                <table id="tblSearch" width="100%">
                </table>
            </div>
            <hr />
            <hr />
            <div id="divAdvanceSearchResult" style="width: 100%; max-height: 310px; overflow: auto;">
                <table id="AdvanceSearchResult" width="100%">
                </table>
            </div>
        </asp:Panel>
    </div>
    <%--this modal pop-up for Changing Status OF Subject--%>
    <button id="BtnChange" runat="server" style="display: none;" />
    <cc1:ModalPopupExtender ID="ModalChangeStatus" runat="server" PopupControlID="divChangeStatus"
        BackgroundCssClass="modalBackground" TargetControlID="BtnChange" BehaviorID="ModalChangeStatus"
        CancelControlID="CancelChangeStaus">
    </cc1:ModalPopupExtender>
    <div id="divChangeStatus" runat="server" class="centerModalPopup" style="display: none; left: 30%; width: 40%; position: absolute; top: 525px; max-height: 404px;">
        <table style="width: 100%">
            <tr>
                <td class="LabelText" style="text-align: center !important; font-size: 12px !important; width: 97%;">Change Status
                </td>
                <td style="text-align: center; height: 22px;" valign="top">
                    <img id="CancelChangeStaus" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px; cursor: pointer;"
                        title="Close" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
        </table>
        <asp:Panel ID="Panel3" runat="server" Visible="true" Width="100%">
            <table width="100%">
                <tr>
                    <td class="LabelText" style="text-align: center;">
                        <input type="radio" id="RblScreening" title="Screening" class="Rbl" onclick="return ValidateChangeStatus('RblScreening');">Screening</input>
                        <input type="radio" id="RblHold" title="On Hold" class="Rbl" onclick="return ValidateChangeStatus('RblHold');">On
                            Hold</input>
                    </td>
                </tr>
                <tr style="height: 10px;">
                    <td></td>
                </tr>
                <tr id="TRStatusHold" style="display: none;">
                    <td>
                        <table width="100%" cellspacing="2%">
                            <tr>
                                <td class="LabelText">Start Date :
                                </td>
                                <td>
                                    <asp:TextBox ID="StatusStartDate" CssClass="TextBox" runat="server" Width="100px"
                                        ToolTip="Please enter date in dd-mm-yyyy format (e.g. '01012013'/'01-Jan-2013')"
                                        onChange="DateConvertForScreening(this.value,this,1);" />
                                    <cc1:CalendarExtender ID="calStartDate" runat="server" TargetControlID="StatusStartDate"
                                        Format="dd-MMM-yyyy">
                                    </cc1:CalendarExtender>
                                </td>
                                <td class="LabelText">End Date :
                                </td>
                                <td>
                                    <asp:TextBox ID="StatusEndDate" CssClass="TextBox" runat="server" Width="100px" ToolTip="Please enter date in dd-mm-yyyy format (e.g. '01012013'/'01-Jan-2013')"
                                        onChange="DateConvertForScreening(this.value,this,2);" />
                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="StatusEndDate"
                                        Format="dd-MMM-yyyy">
                                    </cc1:CalendarExtender>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type="button" value="Save" title="Change Status Of The Booked Subject" id="btnChangeStatus"
                            onclick="return ValidateStatus(); " class="btn btnsave" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <button id="btnSelectedDate" runat="server" style="display: none;" />
    <cc1:ModalPopupExtender ID="CurrrentDatePopup" runat="server" PopupControlID="CurrentDateEvent"
        BackgroundCssClass="modalBackground" TargetControlID="btnSelectedDate" BehaviorID="SelectedDate"
        CancelControlID="CancelCurrDate">
    </cc1:ModalPopupExtender>
    <div id="CurrentDateEvent" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; position: absolute; height: 80%; z-index: 999999! important;">
        <table cellpadding="2" cellpadding="2" style="width: 100%">
            <tr>
                <td class="LabelText" style="text-align: center !important; font-size: 12px !important; width: 97%;">
                    <asp:Label ID="lblCurrentDate" runat="server" Font-Bold="true" />
                </td>
                <td style="width: 3%">
                    <img id="CancelCurrDate" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlProjectDetails" runat="server" Visible="true">
            <%--<asp:Label ID="lblProjectlist" runat="server" Font-Bold="true" Text="Projects List :"
                CssClass="LabelText" />--%>
            <%--<asp:Label ID="lblSlotSubject" runat="server" Font-Bold="true" CssClass="LabelText" />--%>
            <div style="margin: auto; text-align: center;" class="DivRadio LabelText">
            </div>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr style="height: 10px;">
                    <td colspan="4"></td>
                </tr>
                <tr>
                    <td class="LabelText" style="text-align: right; width: 20%;">Project Name* :
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtproject" runat="server" CssClass="TextBox" Width="80%"></asp:TextBox>
                        <input type="button" id="btnSetProject" style="display: none;" onclick="return ShowsheduleSlotting();" />
                        <asp:HiddenField ID="HProjectId" runat="server" />
                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                            TargetControlID="txtProject" ServicePath="~/AutoComplete.asmx" OnClientShowing="ClientPopulated"
                            OnClientItemSelected="OnSelected" MinimumPrefixLength="1" ServiceMethod="GetMyProjectCompletionList"
                            CompletionListElementID="pnlSelectedDate" CompletionListItemCssClass="autocomplete_listitem"
                            CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListCssClass="autocomplete_list"
                            BehaviorID="AutoCompleteExtender1">
                        </cc1:AutoCompleteExtender>
                        <asp:Panel ID="pnlSelectedDate" runat="server" Style="max-height: 150px; overflow: auto; overflow-x: hidden;" />
                    </td>
                    <td class="LabelText" id="tdlocation" style="text-align: right; width: 17%; display: none;">Location* :
                    </td>
                    <td class="LabelText" id="tdDDlLocation" style="text-align: left; display: none;">
                        <asp:DropDownList ID="ddlLoction" runat="server" Width="75%" />
                    </td>
                </tr>
            </table>
            <table id="tbldate" width="100%" style="display: none;">
                <tr>
                    <td class="LabelText" style="text-align: right; width: 20%">Shedule Start Time* :
                    </td>
                    <td style="text-align: left;">
                        <input id="txtstartTime" type="text" class="time TextBox" style="width: 25%" /><span
                            class="LabelText" style="font-size: 10px !important;">&nbsp;Hrs</span>
                    </td>
                    <td class="LabelText" style="text-align: right;">Shedule End Time* :
                    </td>
                    <td style="text-align: left;">
                        <input id="txtendTime" type="text" class="time TextBox" style="width: 25%" /><span
                            class="LabelText" style="font-size: 10px !important;">&nbsp;Hrs</span>
                    </td>
                </tr>
                <tr>
                    <td class="LabelText" style="text-align: right;">No.of Subjects* :
                    </td>
                    <td style="text-align: left;">
                        <input id="txtsubjects" type="text" style="width: 25%" class="TextBox" />
                    </td>
                    <td class="LabelText" style="text-align: right;">Duration* :
                    </td>
                    <td style="text-align: left;">
                        <input id="txttimelength" type="text" style="width: 25%" class="TextBox" /><span
                            class="LabelText" style="font-size: 10px !important;">&nbsp;Mins</span>
                    </td>
                </tr>
                <tr>
                    <td style="height: 7px;" colspan="4"></td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: center;">
                        <input type="button" id="btnSheduleSubject" class="btn btnnew" value="Shedule" title="Shedule Subjects"
                            onclick="return validationForSheduleSubject();" style="font-size: 12px !important;" />
                        <input type="button" id="SheduleSave" class="btn btnsave" onclick="return SaveTimeSlots();"
                            value="Save Shedule" title="Save Slots For Subjects" style="margin: auto; display: none; font-size: 12px !important;" />
                        <input type="button" id="btndeleteall" class="btn btnsave" onclick="return DeleteAllSheduleSubject();"
                            value="Delete All" title="Delete All Shedule Subjects" style="margin: auto; display: none; font-size: 12px !important;" />
                        <input type="button" id="btnCancel" class="btn btncancel" onclick="return CancelSlotting();"
                            value="Cancel" title="Cancel Slotting To Add New Slots" style="margin: auto; display: none; font-size: 12px !important;" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
            </table>
            <div id="divShedule" style="width: 100%; margin-top: 5%; margin: auto; max-height: 300px; overflow-y: auto; display: none;">
                <table id="Tableshedule" width="100%" cellpadding="0" cellspacing="0">
                </table>
            </div>
        </asp:Panel>
        <asp:HiddenField ID="hdnSessionuserid" runat="server" />
        <asp:HiddenField ID="HdnDelete" runat="server" />
        <asp:HiddenField ID="HdnForEachDeleteNode" runat="server" />
        <asp:HiddenField ID="HdnRenderRemove" runat="server" />
        <asp:HiddenField ID="hdnElementData" runat="server" />
        <asp:HiddenField ID="hdnEachSbjtID" runat="server" />
        <asp:HiddenField ID="hdnEachStartime" runat="server" />
        <asp:HiddenField ID="hdnEachhdrID" runat="server" />
        <asp:HiddenField ID="hdnEachScheduleNo" runat="server" />
        <asp:HiddenField ID="hdnRemoveSubjectOnly" runat="server" />
        <asp:HiddenField ID="hdnDraggedSubjectID" runat="server" />
        <asp:HiddenField ID="hdnAdvanceSearchExport" runat="server" />
        <asp:HiddenField ID="hdnEventData" runat="server" />
        <asp:HiddenField ID="hdnProjectwiseSubject" runat="server" />
    </div>
    <button id="Buttonremark" runat="server" style="display: none;" />
    <cc1:ModalPopupExtender ID="ModalPopupForRemark" runat="server" PopupControlID="divRemarks"
        BackgroundCssClass="modalBackground" BehaviorID="ModalPopupForRemark" CancelControlID="CancelRemark"
        TargetControlID="Buttonremark">
    </cc1:ModalPopupExtender>
    <div class="modal-content modal-sm" id="divRemarks" style="display: none;" runat="server">
        <div class="modal-header">
            <%--<img id="CancelRemark" alt="Close" src="images/Close.png" onmouseover="this.style.cursor='pointer';" />--%>
            <img id="CancelRemark" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px;" />
            <h2 style="font-size: x-large;">Remarks</h2>
        </div>
        <div class="modal-body">
            <asp:Panel ID="Panel4" runat="server" Visible="true">
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td colspan="2" class="LabelText" style="text-align: left !important;">Enter Remarks:
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: left !important;">
                            <textarea id="txtremark" rows="3" cols="5" style="width: 97%; height: 63px;" class="TextBox"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: center;"></td>
                    </tr>
                </table>
            </asp:Panel>
            <input type="text" id="DTimeLength" style="display: none;" />
        </div>
        <div class="modal-footer">
            <input type="button" id="btnRemark" class="btn btnsave" title="Delete" value="Delete" onclick="return ValidateDelete();" />
        </div>
    </div>

    <script type="text/javascript" language="javascript">
        var cMONTHNAMES = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug',
                   'Sep', 'Oct', 'Nov', 'Dec'];

        var dataHdr = [];
        var dataDtl = [];
        var dataFinal = [];
        var DeletedataDtl = [];
        var Sheduledtl = [];
        var SheduleHdr = [];
        var choice = [];
        var olddatahdr = [];
        var olddatadtl = [];
        var mode = "";
        var flag = "";
        var SheduleDate = "";
        var deleteall = false;
        var EditedDatadtl = [];
        //function pageLoad() {


        //                var bTable = $('#<%= GrdSubject.ClientID %>').prepend($('<thead>').append($('#<%= GrdSubject.ClientID %> tr:first'))).dataTable({
        //                "bStateSave": false,
        //                "bPaginate": true,
        //                "sPaginationType": "full_numbers",
        //                "bSort": true,
        //                "bDestory": true,
        //                "bRetrieve": true
        //                "aLengthMenu": [[10, 25, 50, 100, 200, 500, -1], [10, 25, 50, 100, 200, 500, "All"]],
        //                "aoColumns": [
        //	                            null, {"sSortDataType":"dom-checkbox"},
        //	                            null,
        //	                            { "sSortDataType": "dom-text" },
        //	                            null, null, null, null, null, null, null
        //	                         ]
        //                });
        //                $('#<%= GrdSubject.ClientID %> tr:first').css('background-color','#3A87AD');  
        //}


        function ClientPopulated(sender, e) {

            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), $("#btnSetProject"));
        }


        function ClientPopulatedprojectForsubject(sender, e) {
            ProjectClientShowing('AutoCompleteExtender2', $get('<%= txtprojectForsubject.ClientId %>'));
        }

        function OnSelectedprojectForsubject(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtprojectForsubject.clientid %>'),
            $get('<%= hdnProjectForsubject.clientid %>'), $("#btnsetsubjectForProject"));
        }


        function ClientPopulatedForSubjectSearch(sender, e) {
            SubjectClientShowing('AutoCompleteExtender3', $get('<%= txtSearchSubject.ClientId %>'));
        }

        function OnSelectedForSubjectSearch(sender, e) {
            SubjectOnItemSelected(e.get_value(), $get('<%= txtSearchSubject.ClientId %>'),
            $get('<%= HSubjectId.clientid %>'), document.getElementById('<%= btnSearchSubject.ClientId %>'));
        }

        function ClientPopulatedprojectForsubjectView(sender, e) {

            ProjectClientShowing('AutoCompleteExtender4', $get('<%= txtProjectForview.ClientId %>'));
        }

        function OnSelectedprojectForsubjectView(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProjectForview.clientid %>'),
            $get('<%= hdnProjectForsubject.clientid %>'), $("#BtnProjectForview"));
            Validateprojectwise();
        }

        function ValidateSearch() {


            window.open('frmSubjectCDMSAdvanceSearch.aspx', '_blank', 'fullscreen=yes,scrollbars=yes');
            return false;
            //        $('#btnSearch').click(function () {
            //            if ($("#AdvanceSearchResult tr")[0] != undefined) {
            //                $("#AdvanceSearchResult").dataTable().fnDestroy();
            //                $('#AdvanceSearchResult').html("");

            //            }
            //            $('#tblSearch').html("");
            //            $('#ctl00_CPHLAMBDA_btnAdvanceSearchExport').css('display', 'none');
            //            $('#ctl00_CPHLAMBDA_btnAddToGrid').css('display', 'none');
            //            CreateTableForAdvanceSearch(0);
            //            $find('AdvanceSearchpopup').show();
        }
        //        });

        //        $('#btnprojectwisescheduling').click(function (){
        function Validateprojectwise() {
            //debugger ;
            if ($('#ctl00_CPHLAMBDA_hdnProjectForsubject').val() != "") {
                $('.divModalBackGround').css('display', 'block');
                $('#ctl00_CPHLAMBDA_hdnProjectwiseSubject').val("Active");
                projectwiserenderslots();
                //$('.fc-header-title').after("<span class = 'titleproject' style='font-Weight:bold;'> Slot List For : " + $('#ctl00_CPHLAMBDA_txtprojectForsubject').val().substring(0, $('#ctl00_CPHLAMBDA_txtprojectForsubject').val().indexOf(']') + 1) + "</span>")
            }
            else {
                msgalert('Please Enter Project To View Its Slots')
                return false
            }
        }
        //         })
        function ValidateAdd() {
            //        $('#btnAdd').click(function (event) {
            event.preventDefault();
            window.location.href = "frmCDMSSubjectInformation.aspx?Mode=1";
            //        });
        }
        function ValidateProjectMatch() {
            debugger;
            if ($('#ctl00_CPHLAMBDA_hdnProjectForsubject').val() == "") {
                msgalert('Please Enter Project');
                $('#ctl00_CPHLAMBDA_txtprojectForsubject').val() == " ";
                $('#ctl00_CPHLAMBDA_txtprojectForsubject').focus();
                return false;

            }
            return true;
        }

        function ResetGrid() {

            $('#ctl00_CPHLAMBDA_txtSearchSubject').val("");
            $('#ctl00_CPHLAMBDA_HSubjectId').val("");
            if ($("#ctl00_CPHLAMBDA_GrdSubject tr")[0] != undefined) {
                $("#ctl00_CPHLAMBDA_GrdSubject").dataTable().fnDestroy();
                $("#ctl00_CPHLAMBDA_GrdSubject").html("");
            }

        }

        function ResetSubject() {
            if ($('#ctl00_CPHLAMBDA_txtSearchSubject').val() == "") {
                $('#ctl00_CPHLAMBDA_HSubjectId').val("");
            }
            return;
        }

        function ResetProject() {
            if ($('#ctl00_CPHLAMBDA_txtprojectForsubject').val() == "") {
                $('#ctl00_CPHLAMBDA_hdnProjectForsubject').val("");
            }
            return;
        }


        function validationForSheduleSubject() {

            debugger;
            // alert ('HHIiii I am RaWan')
            //document.getElementById('ctl00_CPHLAMBDA_ddlLoction').selectedIndex
            if ($('#ctl00_CPHLAMBDA_HProjectId').val() == 0) {
                msgalert('Please Enter Project');
                $('#ctl00_CPHLAMBDA_txtproject').val() == "";
                $('#ctl00_CPHLAMBDA_txtproject').focus()

                return false;
            }
            else if (document.getElementById('ctl00_CPHLAMBDA_ddlLoction').selectedIndex == 0) {
                msgalert('Please Select Location !');
                $('#ctl00_CPHLAMBDA_ddlLoction').focus();
                return false;
            }

            else if ($('#txtstartTime').val().length <= 0) {
                msgalert('Please Enter Start Time');
                $('#txtstartTime').val() == " ";
                $('#txtstartTime').focus();
                return false;
            }
            else if ($('#txtendTime').val().length <= 0) {
                msgalert('Please Enter End Time');
                $('#txtendTime').val() == " ";
                $('#txtendTime').focus();
                return false;
            }
            else if ($('#txtsubjects').val().length <= 0) {
                msgalert('Please Enter No. Of Subjects');
                $('#txtsubjects').val() == " ";
                $('#txtsubjects').focus();
                return false;
            }
            else if ($('#txttimelength').val().length <= 0) {
                msgalert('Please Enter Time Length For Each Subject');
                $('#txttimelength').val() == " ";
                $('#txttimelength').focus();
                return false;
            }

            SheduleSubject();
            return true;
        }

        function SheduleSubject() {
            debugger;
            var StartTime = $('#txtstartTime').val();
            var EndTime = $('#txtendTime').val();
            document.getElementById('btnCancel').style.display = "none";
            if (StartTime != null || StartTime != undefined && EndTime != null || EndTime != undefined) {
                //For Getting Start Time And Time Difference
                var ArrStarttime = StartTime.split(':')
                var starthrs = parseInt(ArrStarttime[0] * 60, 10);
                var startminutes = parseInt(ArrStarttime[1], 10);
                var Arrendtime = EndTime.split(':')
                var endhrs = parseInt(Arrendtime[0] * 60, 10);
                var endminutes = parseInt(Arrendtime[1], 10);
                var diffTime = ((endhrs + endminutes) - (starthrs + startminutes));
                var Timelength = parseInt($('#txttimelength').val(), 10);
                var subjects = parseInt($('#txtsubjects').val(), 10);
                if (isNaN(Timelength)) {
                    msgalert('Please Enter Time Length In Numeric Format');
                    return false;
                }
                if (isNaN(subjects)) {
                    msgalert('Please Enter No. Of Subjects In Numeric Format');
                    return false;
                }
                if (diffTime >= Timelength) {
                    var slots = (diffTime / Timelength);

                    if (slots >= subjects || slots == 1) {
                        if (slots.toString().indexOf(".") == -1) {
                            flag = "1";
                            createGrid(slots, ArrStarttime, Timelength, subjects);
                        }
                        else {
                            msgalert('Please Change Given Time Duration And Slots Duration Are Not Matching.');
                            if ($("#Tableshedule tr")[0] != undefined) {
                                $("#Tableshedule").dataTable().fnDestroy();
                                $('#Tableshedule').html("");
                            }
                            return false;
                        }
                    }
                    else {
                        msgalert([subjects] + ' Subjects Cannot Be Screened In ' + [diffTime] + ' mins As One Subject Requires Minimum ' + [Timelength] + ' mins \n Please Change No. Of Subjects Or Duration');
                        if ($("#Tableshedule tr")[0] != undefined) {
                            $("#Tableshedule").dataTable().fnDestroy();
                            $('#Tableshedule').html("");
                        }
                        return false;
                    }
                }
                else {
                    msgalert('Time Duration For Screening Cannot Be Less Than Time Length')
                    if ($("#Tableshedule tr")[0] != undefined) {
                        $("#Tableshedule").dataTable().fnDestroy();
                        $('#Tableshedule').html("");
                    }
                    return false;
                }
            }
            return true;
        }

        function createGrid(noOfrows, ArrStarttime, duration, repeat) {
            document.getElementById('divShedule').style.display = "";
            document.getElementById('SheduleSave').style.display = "";
            var interval = 0;
            var str = '';
            var time = "";
            var subjectid = "";
            dataDtl = [];
            var seq = 0;
            var slotseq = 0;
            var Condition = false;
            var Bookedsubjectcount = 0;
            var UnBookedsubjectcount = 0;
            var ExtraSlotIndic = false;
            if (flag == 2) {
                str = "<tr><th>Seq No.</th><th>Subjects</th><th>Start Time</th><th>Remove</th></tr>";
            }
            else if (flag == 1) {
                str = "<tr><th>Seq No.</th><th>Subjects</th><th>Start Time</th></tr>";
            }

            for (var i = 0; i < noOfrows; i++) {
                mode = "dtl";
                for (var a = 0; a < repeat; a++) {
                    str += "<tr>";
                    seq = parseInt(seq) + 1;
                    str += "<td>" + seq + "</td>"
                    if (flag == 2) {
                        subjectid = Sheduledtl[i].vSubjectId;
                        str += "<td>" + subjectid + "</td>"
                    }
                    else {
                        if (Sheduledtl.length != 0 && $('#txttimelength').val() == $('#DTimeLength').val()) {
                            Condition = true;
                        }
                        else {
                            slotseq = seq;
                            subjectid = "Slot" + slotseq.toString();
                            str += "<td>" + subjectid + "</td>"
                        }
                    }
                    if (flag == 2) {
                        var subjectstr = "'" + subjectid + "'";
                        time = Sheduledtl[i].dStartTime.substring(0, 5);
                        timestr = "'" + time + "'";
                        str += "<td>" + time + "</td>"
                        str += "<td>" + '<img src="images/Delete.png" title ="Delete" style ="cursor : pointer;"' +
                               'onClick="return DeleteEachSheduleSubject(' + Sheduledtl[i].nWorkSpaceScreeningScheduleDtlId + ',' +
                               '' + Sheduledtl[i].nWorkSpaceScreeningScheduleHdrId + ',' + timestr + ',' +
                               '' + subjectstr + ',' + Sheduledtl[i].iTranNo + ',' + Sheduledtl[i].nWorkspaceScreeningScheduleNo + ');"/>' + "</td>"
                    }
                    else {
                        if (a == 0) {
                            var min = parseInt(ArrStarttime[1], 10) + interval;
                        }
                        if (min == 0 || min == 1 || min == 2 || min == 3 || min == 4 || min == 5 || min == 6 || min == 7 || min == 8 || min == 9) {
                            if (min.toString.length == 1) {
                                min = "0" + min.toString()
                            }
                        }
                        if (min >= 60) {
                            min = min - 60
                            if (min.toString().length != 2) {
                                min = "0" + min.toString()
                            }
                            if (a == 0) {
                                ArrStarttime[0] = parseInt(ArrStarttime[0], 10) + 1
                            }
                        }
                        ArrStarttime[1] = min;
                        interval = duration;
                        if (ArrStarttime[0].toString().length == 1) {
                            ArrStarttime[0] = "0" + ArrStarttime[0];
                        }
                        time = ArrStarttime[0] + ":" + min
                        if (Condition == true) {
                            for (a = 0; a < Sheduledtl.length; a++) {
                                if (Sheduledtl[a].dStartTime == time) {
                                    if (Sheduledtl[a].cStatus == "B") {
                                        subjectid = Sheduledtl[a].vSubjectId;
                                        str += "<td>" + subjectid + "</td>";
                                        str += "<td>" + time + "</td>";
                                        Condition = false;
                                        var Indi = 1;
                                        Bookedsubjectcount = parseInt(Bookedsubjectcount, 10) + 1;
                                    }
                                    else {
                                        slotseq = seq;
                                        subjectid = "Slot" + slotseq.toString();
                                        str += "<td>" + subjectid + "</td>";
                                        str += "<td>" + time + "</td>";
                                        Condition = false;
                                        var Indi = 1;
                                        UnBookedsubjectcount = parseInt(UnBookedsubjectcount, 10) + 1;
                                    }
                                }
                            }
                            if (Indi != 1) {
                                slotseq = seq;
                                subjectid = "Slot" + slotseq.toString();
                                str += "<td>" + subjectid + "</td>";
                                str += "<td>" + time + "</td>";
                                Indi = "";
                                ExtraSlotIndic = true;

                            }
                            else if (Indi == 1 && Condition != false) {
                                slotseq = seq;
                                subjectid = "Slot" + slotseq.toString();
                                str += "<td>" + subjectid + "</td>";
                                str += "<td>" + time + "</td>";
                                Indi = "";
                                ExtraSlotIndic = true;
                            }
                        }
                            //                         else if (Condition == true )
                            //                         {
                            //                               slotseq =  seq;
                            //                               subjectid = "Slot" + slotseq.toString() ;
                            //                               str += "<td>" + subjectid + "</td>"  
                            //                               str += "<td>" + time + "</td>"
                            //                         }
                        else {
                            str += "<td>" + time + "</td>"
                        }
                    }
                    str += "</tr>"
                    if (flag == 1 && document.getElementById('ctl00_CPHLAMBDA_HdnDelete').value == 3) {
                        JsonObject(mode, subjectid, time, NaN);
                    }
                    else {
                        JsonObject(mode, subjectid, time, seq);
                    }
                }

            }
            if (Bookedsubjectcount != Sheduledtl.length && flag == 1) {
                if (UnBookedsubjectcount != Sheduledtl.length) {
                    if (ExtraSlotIndic == false) {
                        msgalert('Assigned Subject Slots Is Not Present In This Time Duration,Please Change The Time Duration');
                        return false;
                    }
                }
            }

            if ($("#Tableshedule tr")[0] != undefined) {
                $("#Tableshedule").dataTable().fnDestroy();
                $('#Tableshedule').html("");
            }

            $('#Tableshedule ').append(str);
            $('#Tableshedule').prepend($('<thead>').append($('#Tableshedule tr:first'))).dataTable({
                "bPaginate": true,
                "sPaginationType": "full_numbers",
                "bSort": true,
                "bDestory": true,
                "bRetrieve": true

            });
            $('#Tableshedule tr:first').css('background-color', '#3A87AD')
        }

        function SaveTimeSlots() {
            //
            $('.divModalBackGround').css('display', 'block');
            //$('#SheduleSave').attr('disabled', true);
            mode = "hdr";
            choice = [];
            var jsonText = "";
            var contentchoice = new Object();
            var ExistingSlot = "";
            var tempolddatahdr = [];
            var tempolddatadtl = [];
            var tempdatadtl = [];
            var MaxSheduleNo = "";
            var ScheduleHdrID = "";
            if (document.getElementById('ctl00_CPHLAMBDA_HdnDelete').value == 1) {
                contentchoice.mode = "Edit";
                choice.push(contentchoice);
                dataFinal.push(dataHdr);
                dataFinal.push(dataDtl);
                dataFinal.push(choice);

            }
            else if (document.getElementById('ctl00_CPHLAMBDA_HdnDelete').value == 3) {
                dataHdr = [];
                JsonObject(mode, NaN, NaN, NaN, NaN);
                tempolddatadtl = olddatadtl;
                tempolddatahdr = olddatahdr;
                if (tempolddatahdr[0].dTimelength != dataHdr[0].dTimelength) {
                    for (var indexdtl = 0; indexdtl < tempolddatadtl.length; indexdtl++) {
                        if (tempolddatadtl[indexdtl].cStatus != "B") {
                            tempolddatadtl[indexdtl].iModifyBy = $('#ctl00_CPHLAMBDA_hdnSessionuserid').val();
                            tempolddatadtl[indexdtl].cStatusIndi = "D";
                            tempolddatadtl[indexdtl].vRemarks = "Slots Time Length Changed ";
                        }
                        else {
                            msgalert('You Have Changed The Time Length Of Slots In Which Subject Is Assigned.Please Change The Minutes Or Unbook The Subjects From The Slot');
                            tempolddatadtl = [];
                            return false;
                        }
                    }

                    for (var a = 0; a < tempolddatadtl.length; a++) {
                        $("#calendar").fullCalendar('removeEvents', tempolddatadtl[a].nWorkspaceScreeningScheduleNo + tempolddatadtl[a].nWorkSpaceScreeningScheduleHdrId);
                        id: tempolddatadtl[a].nWorkspaceScreeningScheduleNo;
                        title: "(" + tempolddatadtl[a].dStartTime + ") " + tempolddatadtl[a].vSubjectId + " " + "[" + tempolddatadtl[a].vProjectNo + "]" + " " + tempolddatadtl[a].vFirstName + " " + tempolddatadtl[a].vMiddleName + " " + tempolddatadtl[a].vSurName;
                        start: tempolddatadtl[a].dScheduledate;
                        description: tempolddatadtl[a].nWorkspaceScreeningScheduleNo + "##" + tempolddatadtl[a].nWorkSpaceScreeningScheduleHdrId +
                                          "##" + tempolddatadtl[a].dScheduledate +
                                          "##" + tempolddatadtl[a].dStartTime + "##" + tempolddatadtl[a].vSubjectId +
                                          "##" + tempolddatadtl[a].iTranNo + "##" + tempolddatadtl[a].vRemarks +
                                          "##" + tempolddatadtl[a].cStatusIndi + "##" + tempolddatadtl[a].iModifyBy +
                                          "##" + tempolddatadtl[a].dModifyOn + "##" + tempolddatadtl[a].vWorkSpaceId +
                                          "##" + tempolddatadtl[a].vProjectNo + "##" + tempolddatadtl[a].nWorkSpaceScreeningScheduleDtlId +
                                          "##" + tempolddatadtl[a].cStatus + "##" + tempolddatadtl[a].vFirstName +
                                          "##" + tempolddatadtl[a].vMiddleName + "##" + tempolddatadtl[a].vSurName;

                    }
                    tempolddatahdr[0].iModifyBy = $('#ctl00_CPHLAMBDA_hdnSessionuserid').val();
                    tempolddatahdr[0].cStatusIndi = "D";
                    tempolddatahdr[0].vRemarks = "Slots Time Length Changed";
                    contentchoice.mode = "Edit";
                    choice.push(contentchoice);
                    dataFinal.push(tempolddatahdr);
                    dataFinal.push(tempolddatadtl);
                    dataFinal.push(choice);
                    jsonText = JSON.stringify({ dataFinal: dataFinal });
                }
                else if (tempolddatahdr[0].dTimelength == dataHdr[0].dTimelength) {
                    tempdatadtl = dataDtl;
                    tempolddatadtl = olddatadtl;
                    //tempolddatahdr = olddatahdr;
                    for (var a = 0; a < tempolddatadtl.length; a++) {
                        if (tempolddatadtl[a].cStatus == "U") {
                            tempolddatadtl[a].cStatusIndi = "D";
                        }
                        $("#calendar").fullCalendar('removeEvents', tempolddatadtl[a].nWorkspaceScreeningScheduleNo + tempolddatadtl[a].nWorkSpaceScreeningScheduleHdrId);
                        id: tempolddatadtl[a].nWorkspaceScreeningScheduleNo;
                        title: "(" + tempolddatadtl[a].dStartTime + ") " + tempolddatadtl[a].vSubjectId + " " + "[" + tempolddatadtl[a].vProjectNo + "]" + " " + tempolddatadtl[a].vFirstName + " " + tempolddatadtl[a].vMiddleName + " " + tempolddatadtl[a].vSurName;
                        start: tempolddatadtl[a].dScheduledate;
                        description: tempolddatadtl[a].nWorkspaceScreeningScheduleNo + "##" + tempolddatadtl[a].nWorkSpaceScreeningScheduleHdrId +
                                                      "##" + tempolddatadtl[a].dScheduledate +
                                                      "##" + tempolddatadtl[a].dStartTime + "##" + tempolddatadtl[a].vSubjectId +
                                                      "##" + tempolddatadtl[a].iTranNo + "##" + tempolddatadtl[a].vRemarks +
                                                      "##" + tempolddatadtl[a].cStatusIndi + "##" + tempolddatadtl[a].iModifyBy +
                                                      "##" + tempolddatadtl[a].dModifyOn + "##" + tempolddatadtl[a].vWorkSpaceId +
                                                      "##" + tempolddatadtl[a].vProjectNo + "##" + tempolddatadtl[a].nWorkSpaceScreeningScheduleDtlId +
                                                      "##" + tempolddatadtl[a].cStatus + "##" + tempolddatadtl[a].vFirstName +
                                                      "##" + tempolddatadtl[a].vMiddleName + "##" + tempolddatadtl[a].vSurName;
                    }

                    for (var indexdtl = 0; indexdtl < tempdatadtl.length; indexdtl++) {
                        for (var a = 0; a < tempolddatadtl.length; a++) {
                            if (tempdatadtl[indexdtl].dStartTime == tempolddatadtl[a].dStartTime) {
                                if (tempolddatadtl[a].cStatus == "B") {

                                    tempdatadtl[indexdtl].nWorkSpaceScreeningScheduleDtlId = tempolddatadtl[a].nWorkSpaceScreeningScheduleDtlId;
                                    tempdatadtl[indexdtl].nWorkSpaceScreeningScheduleHdrId = tempolddatadtl[a].nWorkSpaceScreeningScheduleHdrId;
                                    tempdatadtl[indexdtl].nWorkspaceScreeningScheduleNo = tempolddatadtl[a].nWorkspaceScreeningScheduleNo;;
                                    tempdatadtl[indexdtl].dStartTime = tempolddatadtl[a].dStartTime;
                                    //tempdatadtl[indexdtl].vSubjectId = tempolddatadtl[a].vSubjectId;
                                    tempdatadtl[indexdtl].iTranNo = tempolddatadtl[a].iTranNo;
                                    tempdatadtl[indexdtl].vRemarks = tempolddatadtl[a].vRemarks;
                                    tempdatadtl[indexdtl].cStatusIndi = tempolddatadtl[a].cStatusIndi;
                                    tempdatadtl[indexdtl].iModifyBy = $('#ctl00_CPHLAMBDA_hdnSessionuserid').val();
                                    tempdatadtl[indexdtl].dModifyOn = "";
                                    tempdatadtl[indexdtl].cStatus = tempolddatadtl[a].cStatus;

                                    tempolddatadtl[a].cStatus = "NR" // Not Reequired To Save Again
                                }

                                MaxSheduleNo = tempolddatadtl[a].nWorkspaceScreeningScheduleNo;
                                ScheduleHdrID = tempolddatadtl[a].nWorkSpaceScreeningScheduleHdrId;
                                ExistingSlot = true;
                            }

                        }
                    }

                    for (var indexdtl = 0; indexdtl < tempdatadtl.length; indexdtl++) {
                        if (isNaN(tempdatadtl[indexdtl].nWorkspaceScreeningScheduleNo)) {
                            MaxSheduleNo = parseInt(MaxSheduleNo, 10) + 1;
                            tempdatadtl[indexdtl].nWorkspaceScreeningScheduleNo = MaxSheduleNo;
                            tempdatadtl[indexdtl].nWorkSpaceScreeningScheduleHdrId = ScheduleHdrID;
                        }
                    }
                }

                dataHdr[0].nWorkSpaceScreeningScheduleHdrId = ScheduleHdrID;
                contentchoice.mode = "Edit";
                choice.push(contentchoice);
                dataFinal.push(dataHdr);
                dataFinal.push(tempdatadtl);
                dataFinal.push(choice);
                dataFinal.push(tempolddatadtl);
                document.getElementById('ctl00_CPHLAMBDA_HdnDelete').value = "";
            }

            else {
                dataHdr = [];
                JsonObject(mode, NaN, NaN, NaN, NaN);
                contentchoice.mode = "Add";
                choice.push(contentchoice);
                dataFinal.push(dataHdr);
                dataFinal.push(dataDtl);
                dataFinal.push(choice);
            }
            jsonText = JSON.stringify({ dataFinal: dataFinal });

            $.ajax({
                type: "POST",
                url: "../Ws_Lambda_JSON.asmx/Save_WorkSpaceScreeningScheduleHdrDtl",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: jsonText,
                success: function (data) {
                    if (document.getElementById('ctl00_CPHLAMBDA_HdnDelete').value == 1) {
                        //RenderOrRemoveobjects(2);
                        msgalert("Slots Delete Successfully ");
                        document.getElementById('ctl00_CPHLAMBDA_HdnDelete').value == "";
                        $find('SelectedDate').hide();
                        $('#SheduleSave').attr('disabled', false);
                        dataHdr = [];
                        dataDtl = [];
                        window.location.reload()
                    }
                    else if (document.getElementById('ctl00_CPHLAMBDA_HdnDelete').value == 3) {
                        dataFinal = [];
                        SaveNewTimeSlots();
                    }
                    else {
                        RenderOrRemoveobjects(1);
                        msgalert("Slots Saved Successfully ");
                        $find('SelectedDate').hide();
                        $('#SheduleSave').attr('disabled', false);
                        dataHdr = [];
                        dataDtl = [];
                    }
                    tempolddatahdr = [];
                    tempolddatadtl = [];
                    dataFinal = [];
                    document.getElementById('ctl00_CPHLAMBDA_HdnDelete').value = "";
                    //$('.divModalBackGround').css('display', 'none');
                },
                error: function (ex) {
                    msgalert("Error Occured In Saving Slots In Database")
                    console.log(ex);
                }
            });
        }


        function SaveNewTimeSlots() {
            choice = [];
            var jsonText = "";
            var contentchoice = new Object();
            contentchoice.mode = "Add";
            choice.push(contentchoice);
            dataFinal.push(dataHdr);
            dataFinal.push(dataDtl);
            dataFinal.push(choice);
            jsonText = JSON.stringify({ dataFinal: dataFinal });
            $.ajax({
                type: "POST",
                url: "../Ws_Lambda_JSON.asmx/Save_WorkSpaceScreeningScheduleHdrDtl",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: jsonText,
                success: function (data) {
                    RenderOrRemoveobjects(1);
                    $find('SelectedDate').hide();
                    dataHdr = [];
                    dataDtl = [];
                    dataFinal = [];
                },
                error: function (ex) {
                    msgalert("Error Occured In Saving New Slots In Database")
                    console.log(ex);
                }
            });

        }

        function JsonObject(mode, SubjectId, Starttime, RowIndex) {
            if (mode == "hdr") {
                var contenthdr = new Object();
                contenthdr.nWorkSpaceScreeningScheduleHdrId = "";
                contenthdr.vWorkSpaceId = $('#ctl00_CPHLAMBDA_HProjectId').val();
                contenthdr.dScheduledate = $('#ctl00_CPHLAMBDA_lblCurrentDate').text().replace("Subject Scheduling", "").substr(3, 20);
                contenthdr.dFromTime = $('#txtstartTime').val();
                contenthdr.dToTime = $('#txtendTime').val();
                contenthdr.nNoofSubject = $('#txtsubjects').val();
                contenthdr.vLocationCode = $('#ctl00_CPHLAMBDA_ddlLoction').val();
                contenthdr.vRemarks = "";
                contenthdr.cStatusIndi = "N";
                contenthdr.iModifyBy = $('#ctl00_CPHLAMBDA_hdnSessionuserid').val();
                contenthdr.dModifyOn = "";
                contenthdr.dTimelength = $('#txttimelength').val();
                dataHdr.push(contenthdr);
            }
            else if (mode == "dtl") {
                var contentdtl = new Object();
                contentdtl.nWorkSpaceScreeningScheduleDtlId = "";
                contentdtl.nWorkSpaceScreeningScheduleHdrId = "";
                contentdtl.nWorkspaceScreeningScheduleNo = RowIndex;
                contentdtl.dStartTime = Starttime;
                contentdtl.vSubjectId = SubjectId;
                contentdtl.iTranNo = ""
                contentdtl.vRemarks = ""
                contentdtl.cStatusIndi = "N"
                contentdtl.iModifyBy = $('#ctl00_CPHLAMBDA_hdnSessionuserid').val();
                contentdtl.dModifyOn = "";
                contentdtl.cStatus = "U";
                dataDtl.push(contentdtl)
            }
        }

        function ShowsheduleSlotting() {
            var getdate = $('#ctl00_CPHLAMBDA_lblCurrentDate').text().replace("Subject Scheduling", "").substr(3, 20).split("-");
            SheduleDate = getdate[2] + "-" + getdate[1] + "-" + getdate[0];

            var wstr = "SELECT nWorkspacescreeningScheduleHdrid,dFromTime,dToTime FROM WorkSpaceScreeningScheduleHdr WHERE vWorkSpaceId = '" + $('#ctl00_CPHLAMBDA_HProjectId').val() + "' AND dScheduledate = '" +
                        SheduleDate + "'" + "AND cStatusIndi <> 'D' "
            var obj = new Object();
            obj.query = wstr.toString();
            var JsonText = JSON.stringify(obj);

            $.ajax({
                type: "POST",
                url: "../WS_HelpDB_JSON.asmx/GetDataTableObjectBySqlQuery",
                data: JsonText,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var dataHdrList = $.parseJSON(data.d);
                    if (dataHdrList.length != 0) {
                        for (var a = 0; a < dataHdrList.length; a++) {
                            $('.DivRadio').append('<input type = "radio" class="rblProject" id ="' + dataHdrList[a].nWorkspacescreeningScheduleHdrid + '" onclick = "return rblSelectedIndexChange(' + dataHdrList[a].nWorkspacescreeningScheduleHdrid + ');">' + dataHdrList[a].dFromTime.toString().substr(0, dataHdrList[a].dFromTime.toString().length - 3) + "hrs " + "-" + dataHdrList[a].dToTime.toString().substr(0, dataHdrList[a].dToTime.toString().length - 3) + "hrs" + '</input>')
                        }
                    }
                    document.getElementById('tbldate').style.display = "";
                    document.getElementById('tdDDlLocation').style.display = "";
                    document.getElementById('tdlocation').style.display = "";
                },
                failure: function (error) {
                    msgalert(error);
                }
            });
        }


        function Showshedule(hdrid) {
            document.getElementById('tbldate').style.display = "";
            document.getElementById('tdDDlLocation').style.display = "";
            document.getElementById('tdlocation').style.display = "";
            SheduleHdr = [];
            var getdate = $('#ctl00_CPHLAMBDA_lblCurrentDate').text().replace("Subject Scheduling", "").substr(3, 20).split("-");
            SheduleDate = getdate[2] + "-" + getdate[1] + "-" + getdate[0];

            if (hdrid != "") {
                var wstr = "SELECT * FROM WorkSpaceScreeningScheduleHdr WHERE vWorkSpaceId = '" + $('#ctl00_CPHLAMBDA_HProjectId').val() + "' AND dScheduledate = '" +
                        SheduleDate + "'" + "AND cStatusIndi <> 'D' " + "AND nWorkSpaceScreeningScheduleHdrId = '" + hdrid + "'"
            }
            else {
                var wstr = "SELECT * FROM WorkSpaceScreeningScheduleHdr WHERE vWorkSpaceId = '" + $('#ctl00_CPHLAMBDA_HProjectId').val() + "' AND dScheduledate = '" +
                        SheduleDate + "'" + "AND cStatusIndi <> 'D' "
            }

            var obj = new Object();
            obj.query = wstr.toString();
            var JsonText = JSON.stringify(obj);

            $.ajax(
                    {
                        type: "POST",
                        url: "../WS_HelpDB_JSON.asmx/GetDataTableObjectBySqlQuery",
                        data: JsonText,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            var datahdr = $.parseJSON(data.d);
                            if (datahdr != "") {
                                SheduleHdr = datahdr;
                                document.getElementById('txtstartTime').value = datahdr[0].dFromTime.substring(0, 5);
                                document.getElementById('txtendTime').value = datahdr[0].dToTime.substring(0, 5);
                                document.getElementById('txtsubjects').value = datahdr[0].nNoofSubject;
                                document.getElementById('txttimelength').value = datahdr[0].dTimelength;
                                document.getElementById('DTimeLength').value = datahdr[0].dTimelength;
                                document.getElementById('ctl00_CPHLAMBDA_ddlLoction').value = datahdr[0].vLocationCode;
                                document.getElementById('SheduleSave').style.display = "none";
                                document.getElementById('ctl00_CPHLAMBDA_HdnDelete').value = 3;
                                var contenthdr = new Object();
                                contenthdr.nWorkSpaceScreeningScheduleHdrId = datahdr[0].nWorkSpaceScreeningScheduleHdrId;
                                contenthdr.vWorkSpaceId = datahdr[0].vWorkSpaceId;
                                contenthdr.dScheduledate = datahdr[0].dScheduledate;
                                contenthdr.dFromTime = datahdr[0].dFromTime;
                                contenthdr.dToTime = datahdr[0].dToTime;
                                contenthdr.nNoofSubject = datahdr[0].nNoofSubject;
                                contenthdr.vLocationCode = datahdr[0].vLocationCode;
                                contenthdr.vRemarks = datahdr[0].vRemarks;
                                contenthdr.cStatusIndi = datahdr[0].cStatusIndi;
                                contenthdr.iModifyBy = datahdr[0].iModifyBy;
                                contenthdr.dModifyOn = datahdr[0].dModifyOn;
                                contenthdr.dTimelength = datahdr[0].dTimelength;
                                olddatahdr.push(contenthdr);
                                getSubjectDetails(datahdr[0].nWorkSpaceScreeningScheduleHdrId);
                            }
                            else {
                                document.getElementById('ctl00_CPHLAMBDA_HdnDelete').value = "";
                            }
                        },
                        failure: function (error) {
                            msgalert(error);
                        }
                    });
        }


        function getSubjectDetails(hdrid) {
            var getdate = $('#ctl00_CPHLAMBDA_lblCurrentDate').text().replace("Subject Scheduling", "").substr(3, 20).split("-");
            SheduleDate = getdate[2] + "-" + getdate[1] + "-" + getdate[0];
            Sheduledtl = [];
            olddatadtl = [];
            if (!isNaN(hdrid)) {
                var wstr = "SELECT * FROM View_WorkSpaceScreeningScheduleHdrDtl WHERE nWorkSpaceScreeningScheduleHdrid = ' " + hdrid + " ' AND cStatusIndi <> 'D'"
            }
            else {
                var wstr = "SELECT * FROM View_WorkSpaceScreeningScheduleHdrDtl WHERE nWorkSpaceScreeningScheduleHdrid IN ( " +
                              "SELECT nWorkSpaceScreeningScheduleHdrid FROM WorkSpaceScreeningScheduleHdr WHERE vWorkSpaceId = '" + $('#ctl00_CPHLAMBDA_HProjectId').val() +
                              "' AND dScheduledate = '" + SheduleDate + "') AND cStatusIndi <> 'D' order by vProjectNO,dScheduleDate,dStartTime"

            }
            var obj = new Object();
            obj.query = wstr.toString();
            var Indic = false;
            var JsonText = JSON.stringify(obj);

            $.ajax(
                       {
                           type: "POST",
                           url: "../WS_HelpDB_JSON.asmx/GetDataTableObjectBySqlQuery",
                           data: JsonText,
                           contentType: "application/json; charset=utf-8",
                           dataType: "json",
                           success: function (data) {
                               var Sheduledatadtl = $.parseJSON(data.d);

                               if (Sheduledatadtl != "") {
                                   flag = 2;
                                   Sheduledtl = Sheduledatadtl;

                                   for (var i = 0; i < Sheduledatadtl.length; i++) {
                                       var contentdtl = new Object();
                                       contentdtl.nWorkSpaceScreeningScheduleDtlId = Sheduledatadtl[i].nWorkSpaceScreeningScheduleDtlId;
                                       contentdtl.nWorkSpaceScreeningScheduleHdrId = Sheduledatadtl[i].nWorkSpaceScreeningScheduleHdrId;
                                       contentdtl.nWorkspaceScreeningScheduleNo = Sheduledatadtl[i].nWorkspaceScreeningScheduleNo;
                                       contentdtl.dStartTime = Sheduledatadtl[i].dStartTime;
                                       contentdtl.vSubjectId = Sheduledatadtl[i].vSubjectId;
                                       contentdtl.iTranNo = Sheduledatadtl[i].iTranNo;
                                       contentdtl.vRemarks = Sheduledatadtl[i].vRemarks;
                                       contentdtl.cStatusIndi = Sheduledatadtl[i].cStatusIndi;
                                       contentdtl.iModifyBy = Sheduledatadtl[i].iModifyBy;
                                       contentdtl.dModifyOn = Sheduledatadtl[i].dModifyOn;
                                       contentdtl.cStatus = Sheduledatadtl[i].cStatus;
                                       olddatadtl.push(contentdtl)
                                       if (Sheduledatadtl[i].cStatus == "B") {
                                           var Indic = true;
                                       }
                                   }
                                   if (Indic == true) {
                                       document.getElementById('btndeleteall').style.display = "none";
                                   }
                                   else {
                                       document.getElementById('btndeleteall').style.display = "";
                                   }
                                   createGrid(Sheduledtl.length, NaN, NaN, 1);
                                   RenderOrRemoveobjects(2);
                               }
                               else {
                                   if ($("#Tableshedule tr")[0] != undefined) {
                                       $("#Tableshedule").dataTable().fnDestroy();
                                       $('#Tableshedule').html("");
                                   }
                                   document.getElementById('btndeleteall').style.display = "none";
                                   document.getElementById('SheduleSave').style.display = "none";
                                   RenderOrRemoveobjects(2);
                                   //alert ('Error While Getting Sheduled Subject Details')                            
                               }
                           },
                           failure: function (error) {
                               msgalert(error);
                           }
                       });
        }

        function DeleteEachSheduleSubject(DtlId, HdrId, Time, subject, TranNo, WorkspaceScreeningScheduleNo) {
            document.getElementById('ctl00_CPHLAMBDA_HdnForEachDeleteNode').value = DtlId + "," + HdrId + "," + Time + "," + subject + "," + TranNo + "," + WorkspaceScreeningScheduleNo;

            msgConfirmDeleteAlert(null, "Are You Sure You Want To Delete The Scheduled Slot?", function (isConfirmed) {
                if (isConfirmed) {
                    document.getElementById('ctl00_CPHLAMBDA_HdnDelete').value = 2;
                    document.getElementById('txtremark').value = "";
                    $('#btnRemark').attr('disabled', false);
                    $find('ModalPopupForRemark').show();
                    return false;
                }
            });
        }

        function DeleteAllSheduleSubject() {
            msgConfirmDeleteAlert(null, "Are You Sure You Want Delete All Scheduled Subjects?", function (isConfirmed) {
                if (isConfirmed) {
                    document.getElementById('ctl00_CPHLAMBDA_HdnDelete').value = 1;
                    document.getElementById('txtremark').value = "";
                    $find('ModalPopupForRemark').show();
                    //$("#ctl00_CPHLAMBDA_CurrentDateEvent").css("display", 'none')
                    $('#btndeleteall').attr('disabled', true);
                    return false;
                }
            });
            return false;
        }

        function ValidateDelete() {
            //
            $('#btnRemark').attr('disabled', true);
            if ($('#txtremark').val().trim() != "" && $('#txtremark').val().trim() != undefined) {
                $find('ModalPopupForRemark').hide();
                if (document.getElementById('ctl00_CPHLAMBDA_HdnDelete').value == 1) {

                    for (var indexhdr = 0; indexhdr < SheduleHdr.length; indexhdr++) {
                        SheduleHdr[indexhdr].iModifyBy = $('#ctl00_CPHLAMBDA_hdnSessionuserid').val();
                        SheduleHdr[indexhdr].cStatusIndi = "D";
                        SheduleHdr[indexhdr].vRemarks = $('#txtremark').val().trim();
                    }
                    dataHdr = SheduleHdr

                    for (var indexdtl = 0; indexdtl < Sheduledtl.length; indexdtl++) {
                        Sheduledtl[indexdtl].iModifyBy = $('#ctl00_CPHLAMBDA_hdnSessionuserid').val();
                        Sheduledtl[indexdtl].cStatusIndi = "D";
                        Sheduledtl[indexdtl].vRemarks = $('#txtremark').val().trim();
                    }
                    dataDtl = Sheduledtl;
                    SaveTimeSlots();
                    $('#btnRemark').attr('disabled', false);
                    return true
                }
                else if (document.getElementById('ctl00_CPHLAMBDA_HdnDelete').value == 2) {
                    if ($('#Tableshedule tbody tr').length == 1) {

                    }
                    var dtlcontent = document.getElementById('ctl00_CPHLAMBDA_HdnForEachDeleteNode').value.split(",");
                    var Deletecontentdtl = new Object();
                    Deletecontentdtl.nWorkSpaceScreeningScheduleDtlId = dtlcontent[0];
                    Deletecontentdtl.nWorkSpaceScreeningScheduleHdrId = dtlcontent[1];
                    Deletecontentdtl.dStartTime = dtlcontent[2];
                    Deletecontentdtl.vSubjectId = dtlcontent[3];
                    Deletecontentdtl.iTranNo = dtlcontent[4];
                    Deletecontentdtl.vRemarks = $('#txtremark').val().trim();
                    Deletecontentdtl.cStatusIndi = "D"
                    Deletecontentdtl.iModifyBy = $('#ctl00_CPHLAMBDA_hdnSessionuserid').val();
                    Deletecontentdtl.dModifyOn = "";
                    Deletecontentdtl.nWorkspaceScreeningScheduleNo = dtlcontent[5];
                    Deletecontentdtl.cStatus = "U";
                    Deletecontentdtl.mode = "EDIT";
                    DeletedataDtl.push(Deletecontentdtl)
                    var jsonText = JSON.stringify({ DeletedataDtl: DeletedataDtl });

                    $.ajax({
                        type: "POST",
                        url: "../Ws_Lambda_JSON.asmx/Save_WorkSpaceScreeningScheduleDtl",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: jsonText,
                        success: function (data) {
                            if (data.d == "NotBook") {
                                msgalert("Subject is Booked.!")
                            }
                            else {
                                msgalert("Record Deleted Sucessfully ")
                            }

                            //$find('SelectedDate').hide();
                            DeletedataDtl = [];
                            getSubjectDetails(NaN);
                            $('#btnRemark').attr('disabled', false);
                        },
                        error: function (ex) {
                            msgalert("Error Occured In Saving Slots In Database")
                            console.log(ex);
                        }
                    });

                }

            }
            else {
                msgalert('Please Enter Remarks To Delete');
                $('#btnRemark').attr('disabled', false);
                return false;
            }
            return true;
        }

        function RenderOrRemoveobjects(nobj) {
            //
            var getdate = $('#ctl00_CPHLAMBDA_lblCurrentDate').text().replace("Subject Scheduling", "").substr(3, 20).split("-");

            SheduleDate = getdate[2] + "-" + getdate[1] + "-" + getdate[0];
            var wstr = "SELECT * FROM View_WorkSpaceScreeningScheduleHdrDtl WHERE nWorkSpaceScreeningScheduleHdrid IN ( " +
                         "SELECT nWorkSpaceScreeningScheduleHdrid FROM WorkSpaceScreeningScheduleHdr WHERE vWorkSpaceId = '" + $('#ctl00_CPHLAMBDA_HProjectId').val() +
                         "' AND dScheduledate = '" + SheduleDate + "') order by vProjectNO,dScheduleDate,dStartTime"

            var obj = new Object();
            obj.query = wstr.toString();
            var JsonText = JSON.stringify(obj);

            $.ajax(
                       {
                           type: "POST",
                           url: "../WS_HelpDB_JSON.asmx/GetDataTableObjectBySqlQuery",
                           data: JsonText,
                           contentType: "application/json; charset=utf-8",
                           dataType: "json",
                           success: function (data) {
                               //debugger ;
                               var Renderedatadtl = $.parseJSON(data.d);

                               if (Renderedatadtl != "") {
                                   if (nobj == 1) {
                                       for (var a = 0; a < Renderedatadtl.length; a++) {
                                           if (Renderedatadtl[a].cStatusIndi != "D") {
                                               if (Renderedatadtl[a].cStatus == "B") {
                                                   $('#calendar').fullCalendar('renderEvent', {
                                                       id: Renderedatadtl[a].nWorkspaceScreeningScheduleNo + Renderedatadtl[a].nWorkSpaceScreeningScheduleHdrId,
                                                       title: "(" + Renderedatadtl[a].dStartTime + ") " + Renderedatadtl[a].vSubjectId + " " + "[" + Renderedatadtl[a].vProjectNo + "]" + " " + Renderedatadtl[a].vFirstName + " " + Renderedatadtl[a].vMiddleName + " " + Renderedatadtl[a].vSurName,
                                                       start: Renderedatadtl[a].dScheduledate,
                                                       color: '#E48330',
                                                       description: Renderedatadtl[a].nWorkspaceScreeningScheduleNo + "##" + Renderedatadtl[a].nWorkSpaceScreeningScheduleHdrId +
                                                          "##" + Renderedatadtl[a].dScheduledate +
                                                          "##" + Renderedatadtl[a].dStartTime + "##" + Renderedatadtl[a].vSubjectId +
                                                          "##" + Renderedatadtl[a].iTranNo + "##" + Renderedatadtl[a].vRemarks +
                                                          "##" + Renderedatadtl[a].cStatusIndi + "##" + Renderedatadtl[a].iModifyBy +
                                                          "##" + Renderedatadtl[a].dModifyOn + "##" + Renderedatadtl[a].vWorkSpaceId +
                                                          "##" + Renderedatadtl[a].vProjectNo + "##" + Renderedatadtl[a].nWorkSpaceScreeningScheduleDtlId +
                                                          "##" + Renderedatadtl[a].cStatus + "##" + Renderedatadtl[a].vFirstName +
                                                          "##" + Renderedatadtl[a].vMiddleName + "##" + Renderedatadtl[a].vSurName
                                                   }, true);
                                               }
                                               else {
                                                   $('#calendar').fullCalendar('renderEvent', {
                                                       id: Renderedatadtl[a].nWorkspaceScreeningScheduleNo + Renderedatadtl[a].nWorkSpaceScreeningScheduleHdrId,
                                                       title: "(" + Renderedatadtl[a].dStartTime + ") " + Renderedatadtl[a].vSubjectId + " " + "[" + Renderedatadtl[a].vProjectNo + "]" + " " + Renderedatadtl[a].vFirstName + " " + Renderedatadtl[a].vMiddleName + " " + Renderedatadtl[a].vSurName,
                                                       start: Renderedatadtl[a].dScheduledate,
                                                       description: Renderedatadtl[a].nWorkspaceScreeningScheduleNo + "##" + Renderedatadtl[a].nWorkSpaceScreeningScheduleHdrId +
                                                          "##" + Renderedatadtl[a].dScheduledate +
                                                          "##" + Renderedatadtl[a].dStartTime + "##" + Renderedatadtl[a].vSubjectId +
                                                          "##" + Renderedatadtl[a].iTranNo + "##" + Renderedatadtl[a].vRemarks +
                                                          "##" + Renderedatadtl[a].cStatusIndi + "##" + Renderedatadtl[a].iModifyBy +
                                                          "##" + Renderedatadtl[a].dModifyOn + "##" + Renderedatadtl[a].vWorkSpaceId +
                                                          "##" + Renderedatadtl[a].vProjectNo + "##" + Renderedatadtl[a].nWorkSpaceScreeningScheduleDtlId +
                                                          "##" + Renderedatadtl[a].cStatus + "##" + Renderedatadtl[a].vFirstName +
                                                          "##" + Renderedatadtl[a].vMiddleName + "##" + Renderedatadtl[a].vSurName
                                                   }, true);

                                               }

                                           }
                                       }
                                   }
                                   else if (nobj == 2)

                                       for (var a = 0; a < Renderedatadtl.length; a++) {
                                           if (Renderedatadtl[a].cStatusIndi == "D") {
                                               $("#calendar").fullCalendar('removeEvents', Renderedatadtl[a].nWorkspaceScreeningScheduleNo + Renderedatadtl[a].nWorkSpaceScreeningScheduleHdrId);
                                               id: Renderedatadtl[a].nWorkspaceScreeningScheduleNo + Renderedatadtl[a].nWorkSpaceScreeningScheduleHdrId;
                                               title: "(" + Renderedatadtl[a].dStartTime + ") " + Renderedatadtl[a].vSubjectId + " " + "[" + Renderedatadtl[a].vProjectNo + "]" + " " + Renderedatadtl[a].vFirstName + " " + Renderedatadtl[a].vMiddleName + " " + Renderedatadtl[a].vSurName;
                                               start: Renderedatadtl[a].dScheduledate;
                                               description: Renderedatadtl[a].nWorkspaceScreeningScheduleNo + "##" + Renderedatadtl[a].nWorkSpaceScreeningScheduleHdrId +
                                                          "##" + Renderedatadtl[a].dScheduledate +
                                                          "##" + Renderedatadtl[a].dStartTime + "##" + Renderedatadtl[a].vSubjectId +
                                                          "##" + Renderedatadtl[a].iTranNo + "##" + Renderedatadtl[a].vRemarks +
                                                          "##" + Renderedatadtl[a].cStatusIndi + "##" + Renderedatadtl[a].iModifyBy +
                                                          "##" + Renderedatadtl[a].dModifyOn + "##" + Renderedatadtl[a].vWorkSpaceId +
                                                          "##" + Renderedatadtl[a].vProjectNo + "##" + Renderedatadtl[a].nWorkSpaceScreeningScheduleDtlId +
                                                          "##" + Renderedatadtl[a].cStatus + "##" + Renderedatadtl[a].vFirstName +
                                                          "##" + Renderedatadtl[a].vMiddleName + "##" + Renderedatadtl[a].vSurName;
                                           }
                                       }

                               }
                               else {
                                   msgalert('Error While Getting Scheduled Subject Details')

                               }
                               $('.divModalBackGround').css('display', 'none');
                           },
                           failure: function (error) {
                               msgalert(error);

                           }

                       });

        }


        //        function CreateTableForAdvanceSearch(objects) {
        //            var str = "";
        //            if (objects == "0") {
        //                str = "<tr class = 'FirstTr'><th></th><th class='LabelText' style='text-align:center;'>Select Criteria</th><th class='LabelText' style='text-align:center;'>Select Operator</th><th class='LabelText' style='text-align:center;'>Value</th><th class='LabelText' style='text-align:center;'>Operator</th></tr>";
        //                str += "<tr>";
        //                str += "<td style='width:5%;'><img src='images/add.png' class='addCls' title='ADD' style ='cursor : pointer;'/></td>"


        //            }
        //            else {
        //                str += "<tr>";
        //                str += "<td style='width:5%;'><img src='images/add.png' class='addCls' title='ADD' style ='cursor : pointer;'/></td>"
        //            }
        //            str += "<td style='width:25%;text-align: center;' class='LabelText'><select class = 'DDLSubject' style='width:60%;'><option value='vFirstName'>First Name</option><option value='vMiddleName'>Middle Name</option>" +
        //               "<option value='vSurName'>Sur Name</option><option value='vInitials'>Initials</option><option value='dBirthDate'>Birth Date</option><option value='Age'>Age</option><option value='dEnrollmentDate'>Enrollment Date</option>" +
        //               "<option value='vContactNo'>Contact No</option><option value='vEmailAddress'>Email Address</option><option value='vPlace'>Place</option><option value='cSex'>Sex</option>" +
        //               "<option value='vRace'>Race</option><option value='nHeight'>Height</option><option value='vTransportation'>Transportation</option>" +
        //               "<option value='vAvailiability'>Availiability</option><option value='cRegularDiet'>Regular Diet</option><option value='nWeight'>Weight</option><option value='nBMI'>BMI</option>" +
        //               "<option value='nBloodAvailable'>Blood Available</option><option value='dWashOutDate'>WashOut Date</option><option value='nBloodUsed'>Blood Used</option><option value='vSymptom'>Medical Symptom</option><option value='vLastStudy'>Last Study</option>" +
        //               "</select></td>";
        //            str += "<td style='width:25%;text-align: center;' class='LabelText' ><select class='DDLOperator' style='width:60%;'><option value='='>Equal</option><option value='<>'>Not Equal</option><option value ='>'>Greater Than</option>"
        //               + "<option value = '<'>Less Than</option><option value = '>='>Greater Than Equal</option><option value = '<='>Less Than Equal</option></select></td>";
        //            str += "<td style='width:25%;text-align: center;' class='clsvaluefieldTD LabelText'><input type='TEXTBOX' class='clsvaluefield'/></td>";
        //            str += "<td style='width:20%;text-align: center;' class='LabelText'><select class='DDLClause' style='width:60%;'><option value='AND'>And</option><option value='OR'>Or</option></select></td>"
        //            str += "</tr>"

        //            $('#tblSearch').append(str);
        //            initRemoveClick();
        //            initADDClick();
        //            initCriteriaChange();
        //        }

        //        function initRemoveClick() {

        //            $('.btnRemoveTr').unbind('click').click(function () {
        //                $(this).closest('tr').remove();
        //            });
        //        }
        //        function initADDClick() {
        //            $('.addCls').unbind('click').click(function () {
        //                $(this).attr('src', 'images/minus.png');
        //                $(this).attr('class', 'btnRemoveTr');
        //                $(this).attr('title', 'Remove');
        //                CreateTableForAdvanceSearch(1);
        //            });
        //        }
        //        function initCriteriaChange() {
        //            $('.DDLSubject').unbind('change').change(function () {
        //                if ($(this).val() == "cSex") {
        //                    $(this).closest('tr').children('.clsvaluefieldTD').children().remove();
        //                    $(this).closest('tr').children('.clsvaluefieldTD').wrapInner("<select class='clsvaluefield' style='width:68%;'><option value = '0'>Select Gender</option><option value = 'M'>Male</option><option value = 'F'>Female</option></select>");
        //                }
        //                else if ($(this).val() == "vRace") {
        //                    $(this).closest('tr').children('.clsvaluefieldTD').children().remove();
        //                    $(this).closest('tr').children('.clsvaluefieldTD').wrapInner("<select class='clsvaluefield' style='width:68%;'>" +
        //                                     "<option value = '0'>Select Race</option><option value = 'Asian/Oriental'>Asian/Oriental</option><option value = 'Black'>Black</option><option value = 'Caucasian'>Caucasian</option>" +
        //                                     "<option value = 'Hispanic'>Hispanic</option><option value = 'Mulatto'>Mulatto</option><option value = 'Nativ'>Nativ</option>" +
        //                                     "<option value = 'Verify at Screening'>Verify at Screening</option>"
        //                                     + "</select>");
        //                }
        //                else if ($(this).val() == "vAvailiability") {
        //                    $(this).closest('tr').children('.clsvaluefieldTD').children().remove();
        //                    $(this).closest('tr').children('.clsvaluefieldTD').wrapInner("<select class='clsvaluefield' style='width:68%;'>" +
        //                                     "<option value = '0'>Select Availibility</option><option value = 'Includes all availability'>Includes all availability</option><option value = 'Monday to Friday'>Monday to Friday</option><option value = 'Friday to Monday'>Friday to Monday</option>" +
        //                                     "</select>");
        //                }
        //                else if ($(this).val() == "vTransportation") {
        //                    $(this).closest('tr').children('.clsvaluefieldTD').children().remove();
        //                    $(this).closest('tr').children('.clsvaluefieldTD').wrapInner("<select class='clsvaluefield' style='width:68%;'>" +
        //                                     "<option value = '0'>Select Transport</option><option value = 'Public Transportation'>Public Transportation</option><option value = 'Car'>Car</option><option value = 'None'>None</option>" +
        //                                     "</select>");
        //                }
        //                else if ($(this).val() == "cRegularDiet") {
        //                    $(this).closest('tr').children('.clsvaluefieldTD').children().remove();
        //                    $(this).closest('tr').children('.clsvaluefieldTD').wrapInner("<select class='clsvaluefield' style='width:68%;'>" +
        //                                     "<option value = '0'>Select Diet</option><option value = 'Y'>Yes</option><option value = 'N'>No</option>" +
        //                                     "</select>");
        //                }
        //                else if ($(this).val() == "dBirthDate" || $(this).val() == "dEnrollmentDate" || $(this).val() == "dWashOutDate") {
        //                    $(this).closest('tr').children('.clsvaluefieldTD').children().remove();
        //                    $(this).closest('tr').children('.clsvaluefieldTD').wrapInner("<input type='TEXTBOX' class='clsvaluefield' />");
        //                    $('.clsvaluefield').datepicker({});
        //                }
        //                else if ($(this).val() == "nHeight" || $(this).val() == "nWeight" || $(this).val() == "nBMI" || $(this).val() == "nBloodAvailable" || $(this).val() == "nBloodUsed") {
        //                    $(this).closest('tr').children('.clsvaluefieldTD').children().remove();
        //                    $(this).closest('tr').children('.clsvaluefieldTD').wrapInner("<input type='TEXTBOX' class='clsvaluefield' />");
        //                    $(this).closest('tr').find('.clsvaluefield').ForceNumericOnly();

        //                }
        //                else {
        //                    $(this).closest('tr').children('.clsvaluefieldTD').children().remove();
        //                    $(this).closest('tr').children('.clsvaluefieldTD').wrapInner("<input type='TEXTBOX' class='clsvaluefield' />");
        //                }
        //            });
        //        }

        //        //cRegularDiet,vFirstName,vMiddleName,vSurName,cSex,vRace,vAvailiability,vInitials,vTransportation,dBirthDate,dEnrollmentDate,vContactNo,vEmailAddress,vPlace,vLanguage,nHeight,Weight,nBMI,vLastStudy,nBloodUsed,dWashOutDate,nBloodAvailable            
        //        function ValidateAdvanceSearch() {
        //            var NoOfTr = $('#tblSearch tr').size() - 1, criteria = "", operator = "", Fieldvalue = "", clause = "", Wstr = "";
        //            var query = "SELECT TOP (1000) * FROM VIEW_CDMSSubjectDetails WHERE "
        //            for (var i = 1; i <= NoOfTr; i++) {
        //                if ($('#tblSearch tr:eq(' + i + ')').find('.clsvaluefield').val() != "" && $('#tblSearch tr:eq(' + i + ')').find('.clsvaluefield').val() != "0") {
        //                    $('#tblSearch tr:eq(' + i + ')').find('.clsvaluefield').css('border-color', '');
        //                    criteria = $('#tblSearch tr:eq(' + i + ')').find('.DDLSubject').val();
        //                    operator = $('#tblSearch tr:eq(' + i + ')').find('.DDLOperator').val();
        //                    Fieldvalue = $('#tblSearch tr:eq(' + i + ')').find('.clsvaluefield').val();
        //                    clause = $('#tblSearch tr:eq(' + i + ')').find('.DDLClause').val();
        //                    query += " " + criteria + " " + operator + " '" + Fieldvalue + "' " + clause;
        //                }
        //                else {
        //                    msgalert('Please Enter Value Or Select From Dropdown');
        //                    $('#tblSearch tr:eq(' + i + ')').find('.clsvaluefield').css('border-color', 'red');
        //                    $('#tblSearch tr:eq(' + i + ')').find('.clsvaluefield').focus();
        //                    return false;
        //                }
        //            }
        //            Wstr = query.substring(0, query.toString().length - 3);
        //            AdvanceResultSet(Wstr);
        //            return true
        //        }

        //        function AdvanceResultSet(Wstr) {
        //        debugger ;
        //            var obj = new Object();
        //            obj.query = Wstr.toString();
        //            var JsonText = JSON.stringify(obj);

        //            $.ajax(
        //                    {
        //                        type: "POST",
        //                        url: "../WS_HelpDB_JSON.asmx/GetDataTableObjectBySqlQuery",
        //                        data: JsonText,
        //                        contentType: "application/json; charset=utf-8",
        //                        dataType: "json",
        //                        success: function (data) {
        //                        debugger ;
        //                            document.getElementById('ctl00_CPHLAMBDA_hdnAdvanceSearchExport').value = data.d;
        //                            var Resultset = $.parseJSON(data.d);
        //                            if (Resultset != "") {
        //                                var str = "<tr><th>SubjectID</th><th>Full Name</th><th>D.O.B.</th><th>Gender</th><th>Contact No</th><th>Height</th><th>Weight</th><th>BMI</th></tr>"
        //                                for (var i = 0; i < Resultset.length; i++) {
        //                                    str += "<tr>"
        //                                    str += "<td style ='width:10%;text-align:left;'>" + Resultset[i].vSubjectID + "</td>";
        //                                    str += "<td style ='width:30%;text-align:left'>" + Resultset[i].vFirstName + " " + Resultset[i].vSurName + "</td>";
        //                                    str += "<td style ='width:20%;text-align:left'>" + Resultset[i].dBirthDate.substr(0, Resultset[i].dBirthDate.length - 11) + "</td>";
        //                                    str += "<td style ='width:5%;text-align:left'>" + Resultset[i].cSex + "</td>";
        //                                    str += "<td style ='width:20%;text-align:left'>" + Resultset[i].vContactNo1 + "</td>";
        //                                    str += "<td style ='width:5%;text-align:left'>" + Resultset[i].nHeight + "</td>";
        //                                    str += "<td style ='width:5%;text-align:left'>" + Resultset[i].nWeight + "</td>";
        //                                    str += "<td style ='width:5%;text-align:left'>" + Resultset[i].nBMI + "</td>";
        //                                    str += "</tr>"
        //                                }

        //                                if ($("#AdvanceSearchResult tr")[0] != undefined) {
        //                                    $("#AdvanceSearchResult").dataTable().fnDestroy();
        //                                    $('#AdvanceSearchResult').html("");

        //                                }
        //                                $('#AdvanceSearchResult').append(str);
        //                                $('#AdvanceSearchResult').prepend($('<thead>').append($('#AdvanceSearchResult tr:first'))).dataTable({
        //                                    "bPaginate": true,
        //                                    "sPaginationType": "full_numbers",
        //                                    "bSort": true,
        //                                    "bDestory": true,
        //                                    "bRetrieve": true

        //                                });
        //                                $('#AdvanceSearchResult tr:first').css('background-color', '#3A87AD');
        //                                $('#ctl00_CPHLAMBDA_btnAdvanceSearchExport').css('display', '');
        //                                $('#ctl00_CPHLAMBDA_btnAddToGrid').css('display', '');
        //                            }
        //                            else {
        //                                if ($("#AdvanceSearchResult tr")[0] != undefined) {
        //                                    $("#AdvanceSearchResult").dataTable().fnDestroy();
        //                                    $('#AdvanceSearchResult').html("");

        //                                }
        //                            }
        //                        },
        //                        failure: function (error) {
        //                            msgalert(error);
        //                        }
        //                    });

        //        }
        //prevent alphabate from entering textbox   
        jQuery.fn.ForceNumericOnly =
       function () {
           return this.each(function () {
               $(this).keydown(function (e) {
                   var key = e.charCode || e.keyCode || 0;
                   // allow backspace, tab, delete, arrows, numbers and keypad numbers ONLY
                   // home, end, period, and numpad decimal
                   return (
                        key == 8 ||
                        key == 9 ||
                        key == 46 ||
                        key == 110 ||
                        key == 190 ||
                        (key >= 35 && key <= 40) ||
                        (key >= 48 && key <= 57) ||
                        (key >= 96 && key <= 105));
               });
           });
       };

        function rblSelectedIndexChange(hdrid) {
            $('.DivRadio input[type=radio]').attr('checked', false)
            $('#' + hdrid).attr('checked', 'true')
            if ($("#Tableshedule tr")[0] != undefined) {
                $("#Tableshedule").dataTable().fnDestroy();
                $('#Tableshedule').html("");
            }
            document.getElementById('btnCancel').style.display = "";
            Showshedule(hdrid);
        }

        function ValidateChangeStatus(rblid) {
            $('.Rbl').attr('checked', false)
            $('#' + rblid).attr('checked', 'true')
            var Currentdate = new Date();
            var Data = $('#ctl00_CPHLAMBDA_hdnEventData').val();
            if (rblid == "RblHold") {
                $('#<%= StatusStartDate.ClientID %>').val("");
                $('#<%= StatusEndDate.ClientID %>').val("");
                //                var wstr = "DECLARE @day AS NUMERIC(18,0)= (SELECT iWashPostStudy FROM STUDYDTLCDMS WHERE vWorkspaceId = " + Data.split('##')[10] + " )" +
                //                       "DECLARE @date AS DATETIME = (SELECT dEndDate FROM STUDYDTLCDMS WHERE vWorkspaceId = " + Data.split('##')[10] + " )" +
                //                       "DECLARE @RESULT AS DATETIME = (SELECT DATEADD (DAY,@day,@date)) SELECT CONVERT(VARCHAR(10),@RESULT,111) AS FINALRESULT"
                //                var obj = new Object();
                //                obj.query = wstr.toString();
                //                var JsonText = JSON.stringify(obj);

                //                $.ajax(
                //                    {
                //                        type: "POST",
                //                        url: "../WS_HelpDB_JSON.asmx/GetDataTableObjectBySqlQuery",
                //                        data: JsonText,
                //                        contentType: "application/json; charset=utf-8",
                //                        dataType: "json",
                //                        success: function (data) {
                //                            var dataStatus = $.parseJSON(data.d);
                //                            if (dataStatus.length != 0) {
                //                                if (dataStatus[0].FINALRESULT != "") {
                //                                    $('#TRStatusHold').css('display', '');
                //                                    document.getElementById('StatusStartDate').value = Currentdate.getDate() + "-" + cMONTHNAMES[Currentdate.getMonth()] + "-" + Currentdate.getFullYear();
                //                                    var FinalDate = dataStatus[0].FINALRESULT.split("/");
                //                                    document.getElementById('StatusEndDate').value = FinalDate[2] + "-" + cMONTHNAMES[parseInt(FinalDate[1]) - 1] + "-" + FinalDate[0];
                //                                }
                //                                $('#TRStatusHold').css('display', '');
                //                                $('#StatusStartDate').datepicker({
                //                                    onSelect: function (dateText) {
                //                                        var selecteddate = dateText.split('/');
                //                                        var currentmonth = selecteddate[0];
                //                                        var currentdate = selecteddate[1];
                //                                        var currentyear = selecteddate[2];
                //                                        $('#StatusStartDate').val("");
                //                                        document.getElementById('StatusStartDate').value = currentdate + "-" + cMONTHNAMES[currentmonth - 1] + "-" + currentyear;
                //                                    }
                //                                });

                //                                $('#StatusEndDate').datepicker({
                //                                    onSelect: function (dateText) {
                //                                        var selecteddate = dateText.split('/');
                //                                        var currentmonth = selecteddate[0];
                //                                        var currentdate = selecteddate[1];
                //                                        var currentyear = selecteddate[2];
                //                                        $('#StatusEndDate').val("");
                //                                        document.getElementById('StatusEndDate').value = currentdate + "-" + cMONTHNAMES[currentmonth - 1] + "-" + currentyear;
                //                                    }
                //                                });
                //                            }
                //                        },
                //                        failure: function (error) {
                //                            msgalert(error);
                //                        }
                //                    });
                $('#TRStatusHold').css('display', '');
                //                $('#StatusStartDate').datepicker({

                //                });

                //                $('#StatusEndDate').datepicker({
                //                });


            }
            else if (rblid == "RblScreening") {
                $('#TRStatusHold').css('display', 'none');
            }
        }

        function ValidateStatus() {
            if ($('.Rbl:checked').attr("id") == "RblHold") {
                if ($('#<%= StatusStartDate.ClientID %>').val() == "") {
                    msgalert('Please Enter StartDate In Proper Format');
                    $('#<%= StatusStartDate.ClientID %>').focus();
                }
                else if ($('#<%=StatusEndDate.ClientID %>').val() == "") {
                    msgalert('Please Enter EndDate In Proper Format');
                    $('#<%= StatusEndDate.ClientId %>').focus();
                }
                else if ($('#<%= StatusEndDate.ClientID %>').val() != "" && $('#<%= StatusStartDate.ClientId %>').val() != "") {
                    var Data = $('#ctl00_CPHLAMBDA_hdnEventData').val();
                    $('.divModalBackGround').css('display', 'block');
                    UpdateStatusOfSubject(Data.split("##")[4], "HO", $('#<%= StatusStartDate.ClientID %>').val(), $('#<%= StatusEndDate.ClientID %>').val());
                    fnSubjectDtlCDMSStatus('', '', "HO"); //Added by Pratik Soni
                }

    }
    else if ($('.Rbl:checked').attr("id") == "RblScreening") {
        var Data = $('#ctl00_CPHLAMBDA_hdnEventData').val();
        UpdateStatusOfSubject(Data.split("##")[4], "SC", "", "");
        fnSubjectDtlCDMSStatus('', '', "SC"); //Added by Pratik Soni
    }

}

function UpdateStatusOfSubject(Subjectid, Changeval, startdate, enddate) {
    var content = {};
    content.SubjectId = Subjectid;
    content.ColumnName = "cStatus";
    content.TableName = "SUBJECTDTLCDMS";
    content.ChangedValue = Changeval;
    content.Remarks = "";
    content.StartDate = startdate;
    content.EndDate = enddate;
    $.ajax({
        type: "POST",
        url: "frmSubjectCDMSScheduling.aspx/UpdateFieldValues",
        data: JSON.stringify(content),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.d == true) {
                msgalert('Status Changed Sucessfully');
                $find('ModalChangeStatus').hide();
                $('.divModalBackGround').css('display', 'none');
            }
            else {
                msgalert(data.d);
            }
        },
        failure: function (error) {
            msgalert(error);
        }
    });
}

function CancelSlotting() {
    Sheduledtl = [];
    SheduleHdr = [];
    dataDtl = [];
    tempolddatahdr = [];
    tempolddatadtl = [];
    tempdatadtl = [];
    if ($("#Tableshedule tr")[0] != undefined) {
        $("#Tableshedule").dataTable().fnDestroy();
        $('#Tableshedule').html("");
    }
    //$('#ctl00_CPHLAMBDA_txtproject').val("");
    $('#txtstartTime').val("");
    $('#txtendTime').val("");
    $('#txtsubjects').val("");
    $('#txttimelength').val("");
    $('#DTimeLength').val("");
    // $('#ctl00_CPHLAMBDA_lblSlotSubject').html("");
    document.getElementById('btndeleteall').style.display = "none";
    Sheduledtl = [];

    if ($('#AnyTime--txtstartTime').html() != undefined) {
        AnyTime.noPicker("txtstartTime");
    }
    if ($('#AnyTime--txtendTime').html() != "") {
        AnyTime.noPicker("txtendTime");
    }
    $('.rblProject').attr('checked', false);
    document.getElementById('SheduleSave').style.display = "none";
    document.getElementById('btnCancel').style.display = "none";
    document.getElementById('ctl00_CPHLAMBDA_ddlLoction').selectedIndex = 0;
}
function ValidateExportOrRender() {
    if ($('#ctl00_CPHLAMBDA_hdnAdvanceSearchExport').val() == "") {
        msgalert('Data not found.');
        return false;
    }
    return true;
}

var inyear;
function DateConvertForScreening(ParamDate, txtdate, mode) {

    var flag = false

    if (ParamDate.length == 0) {
        return true;
    }

    if (ParamDate.trim() != '') {

        var dt = ParamDate.trim().toUpperCase();
        var tempdt;
        if (dt.indexOf('UK') >= 0 || dt.indexOf('UNK') >= 0 || dt.indexOf('UKUK') >= 0) {

            if (dt.length < 8) {
                msgalert('Please enter date in DDMMYYYY or dd-Mon-YYYY format only.');
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
                msgalert('Please enter date in DDMMYYYY or dd-Mon-YYYY format only.');
                txtdate.value = "";
                txtdate.focus();
                return false;
            }
            if (month.length > 3 && month.length != 3) {
                msgalert('Please enter date in DDMMYYYY or dd-Mon-YYYY format only.');
                txtdate.value = "";
                txtdate.focus();
                return false;
            }
            if (year.length > 4 && month.length != 4) {
                msgalert('Please enter date in DDMMYYYY or dd-Mon-YYYY format only.');
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
                if (flag == true) {
                    var currentDate = new Date();
                    var date = currentDate.getDate() + "-" + cMONTHNAMES[currentDate.getMonth()] + "-" + currentDate.getFullYear();
                    var difference = GetDateDifference(txtdate.value, date);
                    if (difference.Days > 0) {
                        msgalert('You can not add date which is less than current date ');
                        txtdate.value = "";
                        txtdate.focus();
                        return false;
                    }
                }
                else {
                    if (inyear < 1900) {
                        msgalert('You can not add date which is less than "01-Jan-1900" ');
                        txtdate.value = "";
                        txtdate.focus();
                        return false;
                    }
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
    var currentDate = new Date();
    var date = currentDate.getDate() + "-" + cMONTHNAMES[currentDate.getMonth()] + "-" + currentDate.getFullYear();
    var difference = GetDateDifference(txtdate.value, date);
    if (flag == true) {
        if (difference.Days > 0) {
            msgalert('You can not add date which is less than current date ');
            txtdate.value = "";
            txtdate.focus();
            return false;
        }
    }
    else {
        if (inyear < 1900) {
            msgalert('You can not add date which is less than "01-Jan-1900" ');
            txtdate.value = "";
            txtdate.focus();
            return false;
        }
    }
    dateformatvalidator(mode);
    return true;
}

function projectwiserenderslots() {
    //debugger ;
    var wstr = "SELECT * FROM View_WorkSpaceScreeningScheduleHdrDtl WHERE vWorkspaceId = '" + $('#ctl00_CPHLAMBDA_hdnProjectForsubject').val() + "' order by vProjectNO,dScheduleDate,dStartTime"
    var obj = new Object();
    obj.query = wstr.toString();
    var Indic = false;
    var JsonText = JSON.stringify(obj);

    $.ajax(
               {
                   type: "POST",
                   url: "../WS_HelpDB_JSON.asmx/GetDataTableObjectBySqlQuery",
                   data: JsonText,
                   contentType: "application/json; charset=utf-8",
                   dataType: "json",
                   success: function (data) {
                       //debugger ;
                       var Renderedatadtl = $.parseJSON(data.d);

                       if (Renderedatadtl != "") {
                           $("#calendar").fullCalendar('removeEvents');
                           for (var a = 0; a < Renderedatadtl.length; a++) {
                               if (Renderedatadtl[a].cStatusIndi != "D") {
                                   if (Renderedatadtl[a].cStatus == "B") {
                                       $('#calendar').fullCalendar('renderEvent', {
                                           id: Renderedatadtl[a].nWorkspaceScreeningScheduleNo + Renderedatadtl[a].nWorkSpaceScreeningScheduleHdrId,
                                           title: "(" + Renderedatadtl[a].dStartTime + ") " + Renderedatadtl[a].vSubjectId + " " + "[" + Renderedatadtl[a].vProjectNo + "]" + " " + Renderedatadtl[a].vFirstName + " " + Renderedatadtl[a].vMiddleName + " " + Renderedatadtl[a].vSurName,
                                           start: Renderedatadtl[a].dScheduledate,
                                           color: '#E48330',
                                           description: Renderedatadtl[a].nWorkspaceScreeningScheduleNo + "##" + Renderedatadtl[a].nWorkSpaceScreeningScheduleHdrId +
                                              "##" + Renderedatadtl[a].dScheduledate +
                                              "##" + Renderedatadtl[a].dStartTime + "##" + Renderedatadtl[a].vSubjectId +
                                              "##" + Renderedatadtl[a].iTranNo + "##" + Renderedatadtl[a].vRemarks +
                                              "##" + Renderedatadtl[a].cStatusIndi + "##" + Renderedatadtl[a].iModifyBy +
                                              "##" + Renderedatadtl[a].dModifyOn + "##" + Renderedatadtl[a].vWorkSpaceId +
                                              "##" + Renderedatadtl[a].vProjectNo + "##" + Renderedatadtl[a].nWorkSpaceScreeningScheduleDtlId +
                                              "##" + Renderedatadtl[a].cStatus + "##" + Renderedatadtl[a].vFirstName +
                                              "##" + Renderedatadtl[a].vMiddleName + "##" + Renderedatadtl[a].vSurName
                                       }, true);
                                   }
                                   else {
                                       $('#calendar').fullCalendar('renderEvent', {
                                           id: Renderedatadtl[a].nWorkspaceScreeningScheduleNo + Renderedatadtl[a].nWorkSpaceScreeningScheduleHdrId,
                                           title: "(" + Renderedatadtl[a].dStartTime + ") " + Renderedatadtl[a].vSubjectId + " " + "[" + Renderedatadtl[a].vProjectNo + "]" + " " + Renderedatadtl[a].vFirstName + " " + Renderedatadtl[a].vMiddleName + " " + Renderedatadtl[a].vSurName,
                                           start: Renderedatadtl[a].dScheduledate,
                                           description: Renderedatadtl[a].nWorkspaceScreeningScheduleNo + "##" + Renderedatadtl[a].nWorkSpaceScreeningScheduleHdrId +
                                              "##" + Renderedatadtl[a].dScheduledate +
                                              "##" + Renderedatadtl[a].dStartTime + "##" + Renderedatadtl[a].vSubjectId +
                                              "##" + Renderedatadtl[a].iTranNo + "##" + Renderedatadtl[a].vRemarks +
                                              "##" + Renderedatadtl[a].cStatusIndi + "##" + Renderedatadtl[a].iModifyBy +
                                              "##" + Renderedatadtl[a].dModifyOn + "##" + Renderedatadtl[a].vWorkSpaceId +
                                              "##" + Renderedatadtl[a].vProjectNo + "##" + Renderedatadtl[a].nWorkSpaceScreeningScheduleDtlId +
                                              "##" + Renderedatadtl[a].cStatus + "##" + Renderedatadtl[a].vFirstName +
                                              "##" + Renderedatadtl[a].vMiddleName + "##" + Renderedatadtl[a].vSurName
                                       }, true);

                                   }

                               }
                           }
                           $('.divModalBackGround').css('display', 'none');
                       }
                       else {
                           msgalert('No Slots Found For This Project');
                           return false;
                       }
                   },
                   failure: function (error) {
                       msgalert(error);
                   }
               });

}


function SetGrid() {

    var btn = document.getElementById('<%= btnAddToGrid.ClientId%>');
    btn.click();
}

function dateformatvalidator(mode) {
    debugger;
    if (mode == 1) {
        var str = 'ctl00_CPHLAMBDA_StatusStartDate';
    }
    else {
        var str = 'ctl00_CPHLAMBDA_StatusEndDate';
    }
    if (document.getElementById('ctl00_CPHLAMBDA_StatusStartDate').value != "" && document.getElementById('ctl00_CPHLAMBDA_StatusEndDate').value != "") {
        var fromdate = document.getElementById('ctl00_CPHLAMBDA_StatusStartDate');
        var todate = document.getElementById('ctl00_CPHLAMBDA_StatusEndDate');
        var gap = GetDateDifference(fromdate.value, todate.value);
        if (gap.Days <= 0) {
            if (fromdate.value != todate.value) {
                msgalert('End Date Must Not Be Less Than Start Date');
                document.getElementById(str).value = '';
                document.getElementById(str).focus();
                return false;
            }
        }
    }
}

    </script>

</asp:Content>
