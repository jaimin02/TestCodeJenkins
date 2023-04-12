
<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmCTMMedExInfoHdrDtl.aspx.vb"
    Inherits="frmCTMMedExInfoHdrDtl" ValidateRequest="false" EnableEventValidation="false" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     <style type="text/css">
         [title~=Edit] {
             margin: 0px 2.5px;
         }

         [title~=Update] {
             margin: 0px 2.5px;
         }

         [title~=DCF] {
             margin: 0px 2.5px;
         }

         [title~=AuditTrail] {
             margin: 0px 2.5px;
         }

         AutoTimeConvert .fg-toolbar ui-toolbar ui-widget-header ui-helper-clearfix ui-corner-tl ui-corner-tr {
             width: 97%;
         }

         .dataTables_scrollBody {
             width: 97%;
         }

         .dataTables_scrollHead ui-state-default {
             width: 97%;
         }

         .odd {
             padding-right: 63px;
         }

         .even {
             padding-right: 63px;
         }

         .ui-state-default sorting_disabled {
             padding-right: 63px;
         }

         .ms-choice {
             width: 449px !important;
             height: 23px !important;
         }

         /* Addby Anand Patel */
         .updateProgress {
             position: relative;
             left: 40% !important;
         }
         /*commented by shivani pandya*/
         /*#shadow { height:1375px; }*/

         /*Added for ToolTip*/
         .DataTables_sort_wrapper {
             width: 120px;
             overflow: hidden;
         }


             .DataTables_sort_wrapper:hover {
                 text-overflow: ellipsis;
             }
         /*Added By Vivek Patel */
         .paging_full_numbers .ui-button {
             padding: 2px 6px;
             margin: 0;
             cursor: pointer;
             * cursor: hand;
         }

         canvas {
             height: 65px !important;
         }

         #GVHistoryDtl_wrapper {
                 width: 1200px !important;
         }
         /*Completyed By Vivek Patel*/
     </style>


    <link rel="Stylesheet" href="App_Themes/StyleCommon/CommonStyle.css" />
    <link rel="stylesheet" href="App_Themes/multiple-select.css"/>
    <link href="App_Themes/sweetalert.css" rel="Stylesheet" type="text/css" />
    <link rel="shortcut icon" href="images/biznet.ico" type="image/x-icon"/>
    <link href="App_Themes/font-awesome.min.css" rel="stylesheet"  type="text/css"/>

    <script src="Script/jquery-1.11.3.min.js" type="text/javascript"></script>  
    <script src="Script/jquery-migrate-1.2.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.multiple.select.js"></script>
        
    <script  src="Script/jquery-1.10.9.dataTables.min.js"  type="text/javascript"></script>
    <script src="Script/paginationextjs.js" type="text/javascript"></script>
    
    <script src="Script/Validation.js" language="javascript" type="text/javascript"></script>
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script src="Script/popcalendar.js" language="javascript" type="text/javascript"></script>    
    <script src="Script/jquery.cookie.js" type="text/javascript"></script>
           
    <script type="text/javascript" src="Script/jquery-ui.js"></script>
    <script src="Script/General.js" language="javascript" type="text/javascript"></script> 
    <script type="text/javascript" src="Script/sweetalert.js"></script>    
 

    <asp:PlaceHolder ID="plhScript" runat="server">
        <script language="javascript" type="text/javascript">
            var vHash = '';
            var bajb_backdetect = {
                Version: '1.0.0',
                Description: 'Back Button Detection',
                Browser: {
                    IE: !!(window.attachEvent && !window.opera),
                    Safari: navigator.userAgent.indexOf('Apple') > -1,
                    Opera: !!window.opera
                },
                FrameLoaded: 0,
                FrameTry: 0,
                FrameTimeout: null,
                OnBack: function () {
                    msgalert('Back Button Clicked')
                },
                BAJBFrame: function () {
                    var BAJBOnBack = document.getElementById('BAJBOnBack');
                    if (bajb_backdetect.FrameLoaded > 1) {
                        if (bajb_backdetect.FrameLoaded == 2) {

                            bajb_backdetect.OnBack();
                            //history.back()
                        }
                    }
                    bajb_backdetect.FrameLoaded++;
                    if (bajb_backdetect.FrameLoaded == 1) {
                        if (bajb_backdetect.Browser.IE) {
                            bajb_backdetect.SetupFrames()
                        } else {
                            bajb_backdetect.FrameTimeout = setTimeout("bajb_backdetect.SetupFrames();", 700)
                        }
                    }
                },
                SetupFrames: function () {
                    clearTimeout(bajb_backdetect.FrameTimeout);
                    var BBiFrame = document.getElementById('BAJBOnBack');
                    var checkVar = BBiFrame.src.substr(-11, 11);
                    if (bajb_backdetect.FrameLoaded == 1 && checkVar != "HistoryLoad") {
                        BBiFrame.src = "blank.html?HistoryLoad"
                    } else {
                        if (bajb_backdetect.FrameTry < 2 && checkVar != "HistoryLoad") {
                            bajb_backdetect.FrameTry++;
                            bajb_backdetect.FrameTimeout = setTimeout("bajb_backdetect.SetupFrames();", 700)
                        }
                    }
                },
                SafariHash: 'false',
                Safari: function () {
                    if (bajb_backdetect.SafariHash == 'false') {
                        if (window.location.hash == '#b') {
                            bajb_backdetect.SafariHash = 'true'
                        } else {
                            window.location.hash = '#b'
                        }
                        setTimeout("window.location.hash = '#b';bajb_backdetect.Safari();", 100)
                    } else if (bajb_backdetect.SafariHash == 'true') {
                        if (window.location.hash == '') {
                            bajb_backdetect.SafariHash = 'back';
                            bajb_backdetect.OnBack();
                            //history.back()
                        } else {
                            setTimeout("window.location.hash = '#b';bajb_backdetect.Safari();", 100)
                        }
                    }
                },
                Initialise: function () {

                    if (bajb_backdetect.Browser.Safari) {
                        window.location.hash = '#b';
                        setTimeout("window.location.hash = '#b';bajb_backdetect.Safari();", 600)
                    } else {
                        document.write('<iframe src="blank.html" style="display:none;" id="BAJBOnBack" onunload="alert(\'de\')" onload="bajb_backdetect.BAJBFrame();"></iframe>')
                    }
                }
            };
            bajb_backdetect.Initialise();


            bajb_backdetect.OnBack = function () {



                var obj = new Object();
                var ds_DataentryControl = new Array();

                obj.vWorkspaceId = document.getElementById('HFWorkspaceId').value;
                obj.iNodeId = document.getElementById('ddlActivities').value.split("#")[1];
                obj.vSubjectId = document.getElementById('HFSubjectId').value;
                obj.iModifyBy = '<%= Session(S_UserID)%>';
                obj.iWorkflowStageId = document.getElementById('HFWorkFlowStageId').value;
                ds_DataentryControl.push(obj);
                var content = {};
                content.Choice_1 = "3";
                content.ds_DataentryControl = ds_DataentryControl;
                var JsonText = JSON.stringify(content);

                $.ajax(
                      {
                          type: "POST",
                          url: "Ws_Lambda_JSON.asmx/insert_DataEntryControl",
                          async: false,
                          data: JsonText,
                          contentType: "application/json; charset=utf-8",
                          dataType: "json",
                          success: function (response) {
                              self.close();
                          },
                          failure: function (error) {
                              msgalert(error);
                          }

                      });

            }
            var workSpaceId;
            var nodeId;
            var periodId;
            var subjectId;
            //var mySubjectNo;
            function IsNewTab() {
                return $.cookie(workSpaceId + ":" + nodeId + ":" + periodId + ":" + subjectId);
            }

            //$(document).ready(function () {
            function Open(oArg) {
                debugger;
                workSpaceId = oArg.wId;
                nodeId = oArg.nId;
                periodId = oArg.pId;
                subjectId = oArg.sId;
                //mySubjectNo = oArg.mNo;
                var strrepeatNo = $('#ddlRepeatNo [selected=selected]').val();
                if (strrepeatNo == "N" && ('<%= Session(S_UserType)%>' == "0120" || '<%= Session(S_UserType)%>' == "0121")) {
                    var StrWorkSpaceId = document.getElementById('HFWorkspaceId').value;
                    var StrScreenNo = document.getElementById('HFScreenNo').value;
                    var SubjectId = document.getElementById('HFSubjectId').value;
                    var ImageType = $("#hdnImageType").val();
                    var ActivityId = $("#HFParentActivityId").val();
                    var NodeId = $("#HFParentNodeId").val();
                    var StrUid = document.getElementById('hdniUserId').value;
                    var StrUserTypeCode = '<%= Session(S_UserType)%>';

                    //$.ajax({
                    //    url: "frmCTMMedExInfoHdrDtl.aspx/CheckVisitStatus",
                    //    type: "POST",
                    //    data: '{"WorkSpaceId":"' + StrWorkSpaceId + '","SubjectId":"' + SubjectId + '",' +
                    //            ' "ActivityId":"' + ActivityId + '","NodeId":"' + NodeId + '","UserTypeCode":"' + StrUserTypeCode + '"}',
                    //    async: false,
                    //    contentType: "application/json; charset=utf-8",
                    //    datatype: "json",
                    //    success: successVisitStatus,
                    //    error: errorVisitStatus
                    //});

                    function successVisitStatus(data) {
                        debugger;

                        var data
                        data = data.d
                        if (data.split("#")[0] == "0") {
                            msgalert(data.split("#")[1].toString());
                            document.getElementById('hdnLocked').value = "1";
                            $('#PnlPlaceMedex')[0].style.display = "none";
                            $('#tblButtons')[0].style.display = "none";
                            return false;
                        }
                        else if (data.split("#")[0] == "2") {
                            var strval = data.split("#")[1];
                            $("#hdnImageType").val(strval);
                            document.getElementById("legAuthentication").innerHTML = strval;
                            //$("#tblDigitalFP").find("input,button,textarea,select").attr("disabled", "disabled");
                        }
                    }

                    function errorVisitStatus(e) {
                        throw e;
                    }
                }

                //alert("Hello " + fixedName + "!");
                if (!IsNewTab()) {
                    $.cookie(workSpaceId + ":" + nodeId + ":" + periodId + ":" + subjectId, "YES", {
                        path: '/'
                    });

                    var obj = new Object();
                    var ds_DataentryControl = new Array();
                    var Choice_1, eStr_Retu;
                    obj.vWorkspaceId = document.getElementById('HFWorkspaceId').value;
                    obj.iNodeId = document.getElementById('ddlActivities').value.split("#")[1];
                    obj.vSubjectId = document.getElementById('HFSubjectId').value;
                    obj.iModifyBy = '<%= Session(S_UserID)%>';
                    obj.iWorkflowStageId = document.getElementById('HFWorkFlowStageId').value;
                    ds_DataentryControl.push(obj);
                    var content = {};
                    content.Choice_1 = "1";
                    content.ds_DataentryControl = ds_DataentryControl;
                    var JsonText = JSON.stringify(content);


                    $.ajax(
                          {
                              type: "POST",
                              url: "Ws_Lambda_JSON.asmx/insert_DataEntryControl",
                              data: JsonText,
                              async: false,
                              contentType: "application/json; charset=utf-8",
                              dataType: "json",
                              success: function (response) {
                                  //alert(response.d);
                                  //self.close();
                                  if (response.d == "") {
                                      msgalert('Data Entry on this Activity already Going on.');
                                      document.getElementById('hdnLocked').value = "1";
                                      $('#PnlPlaceMedex')[0].style.display = "none";
                                      $('#tblButtons')[0].style.display = "none";
                                  }

                              },
                              failure: function (error) {
                                  msgalert(error);
                              }

                          });
                    $(window).unload(function () {


                        $.removeCookie(workSpaceId + ":" + nodeId + ":" + periodId + ":" + subjectId, {
                            path: '/'
                        });

                        if (document.getElementById('ddlActivities').value.split("#")[1] == document.getElementById('HFNodeId').value) {

                            var obj = new Object();
                            var ds_DataentryControl = new Array();

                            obj.vWorkspaceId = document.getElementById('HFWorkspaceId').value;
                            obj.iNodeId = document.getElementById('ddlActivities').value.split("#")[1];
                            obj.vSubjectId = document.getElementById('HFSubjectId').value;
                            obj.iModifyBy = '<%= Session(S_UserID)%>';
                            obj.iWorkflowStageId = document.getElementById('HFWorkFlowStageId').value;
                            ds_DataentryControl.push(obj);
                            var content = {};
                            content.Choice_1 = "3";
                            content.ds_DataentryControl = ds_DataentryControl;
                            var JsonText = JSON.stringify(content);

                            $.ajax(
                                  {
                                      type: "POST",
                                      url: "Ws_Lambda_JSON.asmx/insert_DataEntryControl",
                                      data: JsonText,
                                      async: false,
                                      contentType: "application/json; charset=utf-8",
                                      dataType: "json",
                                      success: function (response) {
                                          //alert(response.d);
                                          //self.close();
                                      },
                                      failure: function (error) {
                                          msgalert(error);
                                      }

                                  });
                            var parWin = window.opener;
                            if (parWin != null && typeof (parWin) != 'undefined') {
                                if (parWin && parWin.open && !parWin.closed) {
                                    if (typeof (window.opener.RefreshPage) != 'undefined') {
                                        window.opener.RefreshPage();
                                    }

                                }
                            }
                        }


                        //document.getElementById('hdnIsPopup').value == "false";

                    });

                } else {
                    msgalert('Data Entry on this Activity already Going on.');
                    document.getElementById('hdnLocked').value = "1";
                    $('#PnlPlaceMedex')[0].style.display = "none";
                    $('#tblButtons')[0].style.display = "none";
                    //self.close();
                    //$(function () {
                    //    $('body').html('<div class="error">' +
                    //         '<h1>Sorry!</h1>' +
                    //         '<p>You can only have one instance of this web page open at a time.</p>' +
                    //        '</div>');
                    // });
                    //OR
                    //window.close()
                }
            }

            function Open1(oArg) {

                workSpaceId = oArg.wId;
                nodeId = oArg.nId;
                periodId = oArg.pId;
                subjectId = oArg.sId;
                //mySubjectNo = oArg.mNo;

                //alert("Hello " + fixedName + "!");
                if (!IsNewTab()) {
                    $.cookie(workSpaceId + ":" + nodeId + ":" + periodId + ":" + subjectId, "YES", {
                        path: '/'
                    });
                    $(window).unload(function () {


                        $.removeCookie(workSpaceId + ":" + nodeId + ":" + periodId + ":" + subjectId, {
                            path: '/'
                        });


                    });
                } else {
                    msgalert('Data Entry on this Activity already Going on.');
                    $('#PnlPlaceMedex')[0].style.display = "none";
                    $('#tblButtons')[0].style.display = "none";
                    //$(function () {
                    //    $('body').html('<div class="error">' +
                    //        '<h1>Sorry!</h1>' +
                    //        '<p>You can only have one instance of this web page open at a time.</p>' +
                    //        '</div>');
                    //});
                    ////OR
                    //window.close()
                }
                //});
            }

            function RemoveCookies(oArg) {
                $.removeCookie(oArg.wId + ":" + oArg.nId + ":" + oArg.pId + ":" + oArg.sId, {
                    path: '/'
                });
            }
            function closewindow(Type, ele) {

                if (Type == 'D') {
                    var str = GetQueryStringParams('From');
                    if (str != undefined && str.toUpperCase() == "SCH" && ele.id == $('#btnSaveAndContinue').attr('id'))
                        var conf = false
                    else
                        //var conf = confirm('');
                        msgConfirmDeleteAlert(null, "Are You Sure You Want To Exit ?", function (isConfirmed) {
                            if (isConfirmed) {
                                var parWin = window.opener;
                                if (parWin != null && typeof (parWin) != 'undefined') {
                                    if (parWin && parWin.open && !parWin.closed) {
                                        if (typeof (window.opener.RefreshPage) != 'undefined') {
                                            window.opener.RefreshPage();
                                        }

                                    }
                                }
                                var obj = new Object();
                                var ds_DataentryControl = new Array();

                                obj.vWorkspaceId = document.getElementById('HFWorkspaceId').value;
                                obj.iNodeId = document.getElementById('ddlActivities').value.split("#")[1];
                                obj.vSubjectId = document.getElementById('HFSubjectId').value;
                                obj.iModifyBy = '<%= Session(S_UserID)%>';
                                obj.iWorkflowStageId = document.getElementById('HFWorkFlowStageId').value;
                                ds_DataentryControl.push(obj);
                                var content = {};
                                content.Choice_1 = "3";
                                content.ds_DataentryControl = ds_DataentryControl;
                                var JsonText = JSON.stringify(content);

                                $.ajax(
                                      {
                                          type: "POST",
                                          url: "Ws_Lambda_JSON.asmx/insert_DataEntryControl",
                                          async: false,
                                          data: JsonText,
                                          contentType: "application/json; charset=utf-8",
                                          dataType: "json",
                                          success: function (response) {
                                              self.close();
                                          },
                                          failure: function (error) {
                                              msgalert(error);
                                          }

                                      });
                            }
                            else {
                                return false;
                            }
                        });


                    //if (conf) {

                    //}
                }
                else if (Type == 'COMBO') {

                    if (document.getElementById('hdnLocked').value != "1") {

                        var obj = new Object();
                        var ds_DataentryControl = new Array();

                        obj.vWorkspaceId = document.getElementById('HFWorkspaceId').value;
                        obj.iNodeId = document.getElementById('HFNodeId').value;
                        obj.vSubjectId = document.getElementById('HFSubjectId').value;
                        obj.iModifyBy = '<%= Session(S_UserID)%>';
                        obj.iWorkflowStageId = document.getElementById('HFWorkFlowStageId').value;
                        ds_DataentryControl.push(obj);

                        var content = {};
                        content.Choice_1 = "3";
                        content.ds_DataentryControl = ds_DataentryControl;
                        var JsonText = JSON.stringify(content);

                        $.ajax(
                              {
                                  type: "POST",
                                  url: "Ws_Lambda_JSON.asmx/insert_DataEntryControl",
                                  data: JsonText,
                                  async: false,
                                  contentType: "application/json; charset=utf-8",
                                  dataType: "json",
                                  success: function (response) {
                                      return true;
                                  },
                                  failure: function (error) {
                                      msgalert(error);
                                  }

                              });
                    }
                }
                else {

                    $.removeCookie(document.getElementById('HFWorkspaceId').value + ":" + document.getElementById('ddlActivities').value.split("#")[1] + ":" + document.getElementById('HFSubjectId').value + ":" + document.getElementById('HFWorkFlowStageId').value, {
                        path: '/'
                    });
                    self.close();
                }
        }
        </script>
     
    </asp:PlaceHolder>
    <script language="javascript" type="text/javascript">


        var ControlId, TriggerFlag = false;  //Modify by ketan
        var PreviousValue;
        var inyear;
        var cMONTHNAMES = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug',
                   'Sep', 'Oct', 'Nov', 'Dec'];
        var isOpen


        $(document).ready(function () {

            $('#canal').css('display', 'none');
            $("#HFWidth").val($(document).width());
            $("#HFHeight").val($(document).height());
            updateCountdown('load');
            $('.crfentrycontrol').change(updateCountdown);
            $('.crfentrycontrol').keyup(updateCountdown);
            $('.crfentrycontrol').keypress(textareakeypress);
        });

        $(document).ready(function () {
            $('#divActivityLegends').css('display', 'none');

            $('.password').bind('paste', function (e) {
                e.preventDefault();
                if (e.originalEvent.clipboardData) {
                    msgalert("You can not paste in password field");
                    $('.password').val("");
                }
            });

        });


        var currTab;
        var prevtxt = "";
        function ValidateTextbox(checktype, txt, msg, HighRange, LowRange, AlertOn,
                                 AlertMsg, length, IsNotNull, BtnUpdateId, Scale,
                                 ValidationType, TargetMedExCode) {
            if (TriggerFlag == false) {
                var ShowUpdate = true;
                txt.style.borderColor = '';

                if (IsNotNull == 'Y' && txt.value.trim() == '') {

                    if (prevtxt != txt.id) {
                        $("#" + txt.id).focus();
                        msgalert('Field Can Not Be Left Blank');
                        prevtxt = txt.id;
                    }

                    txt.style.borderColor = "Red";

                    if (txt.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                        return;
                    }

                    if (txt.value.toUpperCase() != PreviousValue.toUpperCase()) {
                        txt.style.borderColor = "navy";
                        CheckToUpdateValue(BtnUpdateId);
                        return;
                    }
                    ShowUpdate = false;
                    //CheckToUpdateValue(BtnUpdateId);
                }
                var CommaSepratedNumbers = /([1-9][0-9]*,)*[0-9][0-9]*/;

                var value = txt.value.trim();
                var scaleForNumeric = value.toString().split(".");
                if (ValidationType == 'NU') {
                    if (scaleForNumeric.length > 2) {
                        msgalert('Enter Data Not In Correct Format');
                        txt.style.borderColor = "Red";
                        if (txt.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                            return;
                        }
                        ShowUpdate = false;
                    }
                    if (scaleForNumeric[0].length > parseInt(length)) {
                        msgalert('Out Of Maximum Length! Value length Must Less or Equal to  ' + parseInt(length) + '. Please Enter The Value in Range')
                        txt.style.borderColor = "Red";
                        //                 txt.value=txt.value.substring(0,parseInt(length));
                        if (txt.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                            return;
                        }
                        ShowUpdate = false;
                    }
                    if (typeof (scaleForNumeric[1]) != 'undefined') {
                        length = parseFloat(length) + 1;
                        if (scaleForNumeric[1].length > Scale) {
                            msgalert('Scale should not be greater than ' + Scale + '.Please enter the value in range');
                            txt.style.borderColor = "Red";
                            if (txt.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                                return;
                            }
                            ShowUpdate = false;
                        }
                    }

                    var decimalRegEx = /^[+|-]?\d*(\.\d+)?$/;
                    if (!decimalRegEx.test(value)) {
                        msgalert('Enter Value Not in Correct Format');
                        $("#" + txt.id).val('')
                        txt.style.borderColor = "Red";
                        if (txt.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                            return;
                        }
                        ShowUpdate = false;
                    }

                    if (txt.value.length <= length) {
                        if (Scale == 0) {
                            if (scaleForNumeric[0].length > (length - Scale)) {
                                msgalert("Please enter data in correct format")
                                txt.value = ''
                                return false;
                            }
                        }
                        else {
                            if (scaleForNumeric[0].length > (length - Scale) - 1) {
                                msgalert("Please enter data in correct format")
                                txt.value = ''
                                return false;
                            }
                        }
                        if (scaleForNumeric.length == 1 && txt.value.trim().length > 1) {
                            if (Scale > 1) {
                                txt.value = txt.value + '.0'
                            }
                            for (i = 0; i < Scale - 1; i++) {
                                txt.value = txt.value + '0'
                            }

                        }
                        if (scaleForNumeric.length > 1) {
                            if (scaleForNumeric[1].length > Scale || scaleForNumeric[0].length > (length - Scale)) {
                                msgalert("Please enter data in correct format")
                                txt.value = ''
                                return false;
                            }
                        }

                    }
                    else {
                        msgalert("Please enter data in correct format")
                        txt.value = ''
                        return false;

                    }
                }
                var result;
                if (txt.value.trim() != '') {

                    if (ValidationType != 'NU') {

                        if (checktype != 0) {
                            switch (parseInt(checktype)) {
                                case 1:
                                    result = CheckInteger(txt.value);
                                    break;
                                case 2:
                                    result = CheckDecimal(txt.value);
                                    break;
                                case 3:
                                    result = CheckIntegerOrBlank(txt.value);
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
                        }

                        if (result == false) {
                            txt.value = '';
                            msgalert(msg);
                            txt.style.borderColor = "Red";
                            if (txt.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                                return;
                            }
                            ShowUpdate = false;

                            //CheckToUpdateValue(BtnUpdateId);
                        }

                    }
                }
                if ((HighRange != 0) && (LowRange != 0)) {
                    if ((txt.value != '')) {
                        if ((txt.value > HighRange) || (txt.value < LowRange)) {
                            msgalert('Value should be Less than ' + HighRange + ' And Greater than ' + LowRange + '.');
                            txt.style.borderColor = "Red";
                            if (txt.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                                return;
                            }
                            ShowUpdate = false;
                        }
                    }
                }

                else if (HighRange != 0) {
                    if (txt.value > HighRange) {
                        msgalert('Value should be Less than ' + HighRange + '.');
                        txt.style.borderColor = "Red";
                        if (txt.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                            return;
                        }
                        ShowUpdate = false;
                        //CheckToUpdateValue(BtnUpdateId);
                    }
                }

                else if (LowRange != 0) {
                    if (txt.value < LowRange) {
                        msgalert('Value should be Greater than ' + LowRange + '.');
                        txt.style.borderColor = "Red";
                        if (txt.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                            return;
                        }
                        ShowUpdate = false;
                        //CheckToUpdateValue(BtnUpdateId);
                    }
                }


                if (txt.value.trim() != '') {
                    if (txt.value.toUpperCase() == AlertOn.toUpperCase()) {
                        msgalert(AlertMsg);
                        //txt.style.borderColor = "Red";
                        if (txt.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                            return;
                        }
                        ShowUpdate = false;
                        //CheckToUpdateValue(BtnUpdateId);
                    }
                }
                if (length != 0) {
                    if (ValidationType == "NU" || scaleForNumeric.length > 1) {
                        length = parseInt(length) + parseInt(1)
                    }


                    if (txt.value.length > parseInt(length)) {
                        msgalert('Length Exceeded! Lengh Should Be Less Then Or Equal To ' + length);
                        txt.style.borderColor = "Red";
                        txt.value = (txt.value.substring(0, parseInt(length)));
                        if (txt.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                            return;
                        }
                        //CheckToUpdateValue(BtnUpdateId);
                    }
                }
            }
            TriggerFlag = false;

            //Added by Vimal Ghoniya
            var TargetMedExCodeArray = TargetMedExCode.split(','), MedExCode = "";
            for (var count = 0; count < TargetMedExCodeArray.length; count++) {
                MedExCode = TargetMedExCodeArray[count];
                if (MedExCode != undefined && MedExCode != "") {
                    if (MedExCode.split("#")[2].toUpperCase().indexOf('[' + txt.value.toUpperCase() + ']') != -1) {
                        if (MedExCode.split("#")[1].toLowerCase() === "radio" ||
                            MedExCode.split("#")[1].toLowerCase() === "checkbox") {
                            $("#" + MedExCode.split("#")[0] + ' *').removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0] + ' *').val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "file") {
                            $("#FU" + MedExCode.split("#")[0]).removeAttr("disabled");
                            $("#FU" + MedExCode.split("#")[0]).val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "comboglobaldictionary") {
                            $("#" + MedExCode.split("#")[0]).siblings().removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }
                        else {
                            $("#" + MedExCode.split("#")[0]).removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).siblings().removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }

                    }
                    else {
                        if (MedExCode.split("#")[1].toLowerCase() === "radio" ||
                            MedExCode.split("#")[1].toLowerCase() === "checkbox") {
                            $("#" + MedExCode.split("#")[0] + ' *').attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0] + ' *').removeAttr('checked');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "file") {
                            $("#FU" + MedExCode.split("#")[0]).attr("disabled", "disabled");
                            $("#FU" + MedExCode.split("#")[0]).val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "comboglobaldictionary") {
                            $("#" + MedExCode.split("#")[0]).siblings().attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }
                        else {
                            $("#" + MedExCode.split("#")[0]).attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).siblings().attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).val('');

                        }
                    }
                }
            }
            //End

            if (txt.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                return;
            }
            if (ShowUpdate == true) {
                CheckToUpdateValue(BtnUpdateId);
            }

            return;
        }

        function C2F(txtCelsiusID, txtFahrenheitID) {
            var txtCelsius = document.getElementById(txtCelsiusID);
            var txtFahrenheit = document.getElementById(txtFahrenheitID);
            var result = true;

            if (!(CheckDecimalOrBlank(txtCelsius.value))) {
                msgalert('Please Enter Valid Temperature In Celsius.');
                txtCelsius.focus();
                if (txtCelsius.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                    return false;
                }
                result = false;
                //CheckToUpdateValue(BtnUpdateId);
                return false;
            }
            txtFahrenheit.value = c2f(txtCelsius.value);
            if (txtCelsius.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                return true;
            }
            if (result == false) {
                CheckToUpdateValue(BtnUpdateId);
            }
            return true;
        }

        function F2C(txtFahrenheitID, txtCelsiusID, BtnUpdateId, TargetMedExCode) {
            var txtCelsius = document.getElementById(txtCelsiusID);
            var txtFahrenheit = document.getElementById(txtFahrenheitID);
            var result = true;

            if (TriggerFlag == false) {
                if (!(CheckDecimalOrBlank(txtFahrenheit.value))) {
                    msgalert('Please Enter Valid Temperature In Fahrenheit.');
                    txtFahrenheit.focus();
                    if (txtFahrenheit.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                        return false;
                    }
                    //CheckToUpdateValue(BtnUpdateId);
                    result = false;

                    return false;
                }
                txtCelsius.value = f2c(txtFahrenheit.value);
            }
            TriggerFlag = false;
            //Added by Vimal Ghoniya
            var TargetMedExCodeArray = TargetMedExCode.split(','), MedExCode = "";
            for (var count = 0; count < TargetMedExCodeArray.length; count++) {
                MedExCode = TargetMedExCodeArray[count];
                if (MedExCode != undefined && MedExCode != "") {
                    if (MedExCode.split("#")[2].toUpperCase().indexOf('[' + txtCelsius.value.toUpperCase() + ']') != -1) {
                        if (MedExCode.split("#")[1].toLowerCase() === "radio" ||
                            MedExCode.split("#")[1].toLowerCase() === "checkbox") {
                            $("#" + MedExCode.split("#")[0] + ' *').removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0] + ' *').val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "file") {
                            $("#FU" + MedExCode.split("#")[0]).removeAttr("disabled");
                            $("#FU" + MedExCode.split("#")[0]).val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "comboglobaldictionary") {
                            $("#" + MedExCode.split("#")[0]).siblings().removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }
                        else {
                            $("#" + MedExCode.split("#")[0]).removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).siblings().removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }

                    }
                    else {
                        if (MedExCode.split("#")[1].toLowerCase() === "radio" ||
                            MedExCode.split("#")[1].toLowerCase() === "checkbox") {
                            $("#" + MedExCode.split("#")[0] + ' *').attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0] + ' *').removeAttr('checked');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "file") {
                            $("#FU" + MedExCode.split("#")[0]).attr("disabled", "disabled");
                            $("#FU" + MedExCode.split("#")[0]).val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "comboglobaldictionary") {
                            $("#" + MedExCode.split("#")[0]).siblings().attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }
                        else {
                            $("#" + MedExCode.split("#")[0]).attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).siblings().attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).val('');

                        }
                    }
                }
            }
            //End
            if (txtFahrenheit.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                return true;
            }
            if (result == true) {
                CheckToUpdateValue(BtnUpdateId);
            }
            return true;
        }
        function ddlAlerton(objId, alerton, alertmsg, BtnUpdateId, TargetMedExCode) {

            if (TriggerFlag == false) {
                document.getElementById(objId).style.color = "Navy";
                if (alerton != '') {
                    if (document.getElementById(objId).value.toUpperCase() == alerton.toUpperCase()) {
                        //document.getElementById(objId).style.color = "Red";
                        msgalert(alertmsg);
                    }
                }
            }
            TriggerFlag = false;

            //Added By Vimal Ghoniya For Dependency
            var TargetMedExCodeArray = TargetMedExCode.split(','), MedExCode = "", boolFlag = false;
            for (var count = 0; count < TargetMedExCodeArray.length; count++) {
                MedExCode = TargetMedExCodeArray[count];
                if (MedExCode != undefined && MedExCode != "") {
                    for (var i = 0; i < $('#' + objId).find('option').length; i++) {
                        if (MedExCode.split("#")[2].toUpperCase().indexOf($($('#' + objId).find('option')[i]).val().toUpperCase()) != -1) {
                            boolFlag = true;
                        }
                    }
                    if (MedExCode.split("#")[2].toUpperCase().indexOf($('#' + objId).val().toUpperCase()) != -1 &&
                        boolFlag) {
                        if (MedExCode.split("#")[1].toLowerCase() === "radio" ||
                            MedExCode.split("#")[1].toLowerCase() === "checkbox") {
                            $("#" + MedExCode.split("#")[0] + ' *').removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0] + ' *').val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "file") {
                            $("#FU" + MedExCode.split("#")[0]).removeAttr("disabled");
                            $("#FU" + MedExCode.split("#")[0]).val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "comboglobaldictionary") {
                            $("#" + MedExCode.split("#")[0]).siblings().removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }
                        else {
                            $("#" + MedExCode.split("#")[0]).removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).siblings().removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }

                    }
                    else {
                        if (MedExCode.split("#")[1].toLowerCase() === "radio" ||
                            MedExCode.split("#")[1].toLowerCase() === "checkbox") {
                            $("#" + MedExCode.split("#")[0] + ' *').attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0] + ' *').removeAttr('checked');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "file") {
                            $("#FU" + MedExCode.split("#")[0]).attr("disabled", "disabled");
                            $("#FU" + MedExCode.split("#")[0]).val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "comboglobaldictionary") {
                            $("#" + MedExCode.split("#")[0]).siblings().attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }
                        else {
                            $("#" + MedExCode.split("#")[0]).attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).siblings().attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).val('');

                        }
                    }
                }
            }
            //End
            if (PreviousValue != undefined && PreviousValue != "") {
                if (document.getElementById(objId).value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                    return;
                }
                CheckToUpdateValue(BtnUpdateId);
            }

        }
        function checkddlNotNull(objId, IsNotNull, BtnUpdateId) {
            var result = true;
            document.getElementById(objId).style.backgroundColor = '';
            if (IsNotNull == 'Y' && document.getElementById(objId).value.trim() == '') {
                document.getElementById(objId).style.backgroundColor = "Red";
                msgalert('Field Can Not Be Left Blank');
                result = false;
            }
            if (PreviousValue != undefined && PreviousValue != "") {
                if (document.getElementById(objId).value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                    return;
                }
            }
            if (result == true) {
                CheckToUpdateValue(BtnUpdateId);
            }
        }
        function checkRBLNotNull(tblName, IsNotNull, BtnUpdateId) {
            var result = true;
            if (IsNotNull == 'Y') {
                var tblRdo = $get(tblName);
                var name = tblRdo.id;
                name = name.replace(/_/g, '$');
                var rdos = document.getElementsByName(name);
                var i;
                tblRdo.style.color = "Navy";
                for (i = 0; i < rdos.length; i++) {
                    if (rdos[i].checked) {
                        break;
                    }
                }
                result = false;
                tblRdo.style.color = "Red";
                msgalert('Field Can Not Be Left Blank');
            }
            if (result == true) {
                CheckToUpdateValue(BtnUpdateId);
            }
        }
        function Alerton(tblName, alertOn, alertMsg, BtnUpdateId, Value, TargetMedExCode) {
            ControlId = tblName;
            PreviousValue = Value;
            var tblRdo = $get(tblName);
            var name = tblRdo.id;
            name = name.replace(/_/g, '$');
            var rdos = document.getElementsByName(name);
            var i;

            //Added By Vimal Ghoniya For Depedency
            var TargetMedExCodeArray = TargetMedExCode.split(','), MedExCode = "";
            for (var count = 0; count < TargetMedExCodeArray.length; count++) {
                MedExCode = TargetMedExCodeArray[count];
                if (MedExCode != undefined && MedExCode != "") {
                    if (MedExCode.split("#")[2].toUpperCase().indexOf('[' + $($('#' + tblName)).find('input:radio:checked').val().toUpperCase() + ']') != -1) {
                        if (MedExCode.split("#")[1].toLowerCase() === "radio") {
                            $("#" + MedExCode.split("#")[0] + ' *').removeAttr("disabled");
                            //$("#" + MedExCode.split("#")[0] + ' *').val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "standarddate") {
                            $("#" + MedExCode.split("#")[0] + "_1").removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0] + "_1").val('');

                            $("#" + MedExCode.split("#")[0] + "_2").removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0] + "_2").val('');

                            $("#" + MedExCode.split("#")[0] + "_3").removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0] + "_3").val('');

                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "standarddatetime") {
                            $("#" + MedExCode.split("#")[0] + "_1").removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0] + "_1").val('');

                            $("#" + MedExCode.split("#")[0] + "_2").removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0] + "_2").val('');

                            $("#" + MedExCode.split("#")[0] + "_3").removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0] + "_3").val('');

                            $("#" + MedExCode.split("#")[0] + "_4").removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0] + "_4").val('');

                            $("#" + MedExCode.split("#")[0] + "_5").removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0] + "_5").val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "checkbox") {
                            $("#" + MedExCode.split("#")[0] + ' *').removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0] + ' *').val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "file") {
                            $("#FU" + MedExCode.split("#")[0]).removeAttr("disabled");
                            $("#FU" + MedExCode.split("#")[0]).val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "comboglobaldictionary") {
                            $("#" + MedExCode.split("#")[0]).siblings().removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }
                        else {
                            $("#" + MedExCode.split("#")[0]).removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).siblings().removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }

                    }
                    else {
                        if (MedExCode.split("#")[1].toLowerCase() === "radio" ||
                                        MedExCode.split("#")[1].toLowerCase() === "checkbox") {
                            $("#" + MedExCode.split("#")[0] + ' *').attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0] + ' *').removeAttr('checked');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "standarddate") {
                            $("#" + MedExCode.split("#")[0] + "_1").attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0] + "_1").val('');

                            $("#" + MedExCode.split("#")[0] + "_2").attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0] + "_2").val('');

                            $("#" + MedExCode.split("#")[0] + "_3").attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0] + "_3").val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "standarddatetime") {
                            $("#" + MedExCode.split("#")[0] + "_1").attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0] + "_1").val('');

                            $("#" + MedExCode.split("#")[0] + "_2").attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0] + "_2").val('');

                            $("#" + MedExCode.split("#")[0] + "_3").attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0] + "_3").val('');

                            $("#" + MedExCode.split("#")[0] + "_4").attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0] + "_4").val('');

                            $("#" + MedExCode.split("#")[0] + "_5").attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0] + "_5").val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "file") {
                            $("#FU" + MedExCode.split("#")[0]).attr("disabled", "disabled");
                            $("#FU" + MedExCode.split("#")[0]).val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "comboglobaldictionary") {
                            $("#" + MedExCode.split("#")[0]).siblings().attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }
                        else {
                            $("#" + MedExCode.split("#")[0]).attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).siblings().attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).val('');

                        }
                    }

                }
            }
            //End
            if (TriggerFlag == false) {
                for (i = 0; i < rdos.length; i++) {
                    tblRdo.style.color = "Navy";
                    if (rdos[i].checked && rdos[i].value.toUpperCase() == alertOn.toUpperCase()) {
                        //tblRdo.style.color = "Red";
                        msgalert(alertMsg);
                        break;
                    }
                    if (rdos[i].checked && rdos[i].value.toUpperCase() == Value && BtnUpdateId != null) {
                        return;
                    }
                }
            }
            TriggerFlag = false;
            CheckToUpdateValue(BtnUpdateId);
        }

        function RemoveSelection(tblName, BtnUpdateId) {
            var tblRdo = $get(tblName);
            var name = tblRdo.id;
            name = name.replace(/_/g, '$');
            var rdos = document.getElementsByName(name);
            var i;

            if (rdos[0].disabled != true) {
                for (i = 0; i < rdos.length; i++) {

                    rdos[i].checked = false;
                }
                document.getElementById('HFRadioButtonValue').value = 'NULL';
                CheckToUpdateValue(BtnUpdateId);
            }
        }

        function checkCBLNotNull(chklst, IsNotNull, BtnUpdateId) {
            var result = true;
            if (IsNotNull == 'Y') {
                chklst.style.color = "Navy";
                var chks;
                var result = false;
                var i;
                if (chklst != null && typeof (chklst) != 'undefined') {
                    chks = chklst.getElementsByTagName('input');
                    for (i = 0; i < chks.length; i++) {
                        var val = chks[i].nextSibling.innerText;
                        if (chks[i].type.toUpperCase() == 'CHECKBOX' && chks[i].checked) {
                            break;
                        }
                    }
                    chklst.style.color = "Red";
                    msgalert('Field Can Not Be Left Blank');
                    result = false;
                }
            }
            if (result == true) {
                CheckToUpdateValue(BtnUpdateId);
            }
        }
        function AlertonCheckBox(chklst, alertOn, alertMsg, BtnUpdateId, Value, TargetMedExCode, id) {
            document.getElementById('hdnOldMedExValue').value = Value
            ControlId = chklst.id;
            PreviousValue = Value;
            //            chklst.style.color = "Navy";
            var chks;
            if (PreviousValue != "") {
                var result1 = $('#' + chklst).is(':checked')
                var valuewithDefault = $('#' + chklst).first().next("label").text()
            }
            var valuewithoutDefault = $(chklst).first().next("label").text()
            var result2 = $(chklst).is(':checked')
            document.getElementById('HFChkSelectedList').value = alertOn;
            var i;

            //Added By Vimal Ghoniya for Dependency
            var TargetMedExCodeArray = TargetMedExCode.split(','), MedExCode = "", boolFlag = false;
            for (var count = 0; count < TargetMedExCodeArray.length; count++) {
                MedExCode = TargetMedExCodeArray[count];
                if (MedExCode != undefined && MedExCode != "") {
                    for (var i = 0; i < $('#' + id).find('input:checkbox:checked').length; i++) {
                        if (MedExCode.split("#")[2].toUpperCase().indexOf($($('#' + id).find('input:checkbox:checked').next('label')[i]).text().toUpperCase()) != -1) {
                            boolFlag = true;
                        }
                    }
                    if (MedExCode.toUpperCase().indexOf($(chklst).next("label").text().toUpperCase()) != -1 &&
                        boolFlag) {
                        if (MedExCode.split("#")[1].toLowerCase() === "radio" ||
                            MedExCode.split("#")[1].toLowerCase() === "checkbox") {
                            $("#" + MedExCode.split("#")[0] + ' *').removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0] + ' *').val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "file") {
                            $("#FU" + MedExCode.split("#")[0]).removeAttr("disabled");
                            $("#FU" + MedExCode.split("#")[0]).val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "comboglobaldictionary") {
                            $("#" + MedExCode.split("#")[0]).siblings().removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }
                        else {
                            $("#" + MedExCode.split("#")[0]).removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).siblings().removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }

                    }
                    else {
                        if (MedExCode.split("#")[1].toLowerCase() === "radio" ||
                            MedExCode.split("#")[1].toLowerCase() === "checkbox") {
                            $("#" + MedExCode.split("#")[0] + ' *').attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0] + ' *').removeAttr('checked');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "file") {
                            $("#FU" + MedExCode.split("#")[0]).attr("disabled", "disabled");
                            $("#FU" + MedExCode.split("#")[0]).val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "comboglobaldictionary") {
                            $("#" + MedExCode.split("#")[0]).siblings().attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }
                        else {
                            $("#" + MedExCode.split("#")[0]).attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).siblings().attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).val('');

                        }
                    }
                }
            }

            //End


            if (TriggerFlag == false) {
                if (valuewithDefault == alertOn && result1 == true) {
                    msgalert(alertMsg);
                    CheckToUpdateValue(BtnUpdateId);
                }
                if (valuewithoutDefault == alertOn && result2 == true) {
                    msgalert(alertMsg);
                    CheckToUpdateValue(BtnUpdateId);

                }
            }

            TriggerFlag = false;
            //            if (chklst != null && typeof (chklst) != 'undefined') {
            //                chks = chklst.getElementsByTagName('input');
            //                for (i = 0; i < chks.length; i++) {
            //                    var val = chks[i].nextSibling.innerText;
            //                    if (chks[i].type.toUpperCase() == 'CHECKBOX' && chks[i].checked && value == alertOn) {
            //                        msgalert(alertMsg);
            //                        //chklst.style.color = "Red";
            //                        break;

            //                    }
            //                }
            //            }
            //            CheckToUpdateValue(BtnUpdateId);


        }

        function DateValidation(ParamDate, txtdate, IsNotNull, BtnUpdateId, TargetMedExCode) {

            var result = true;
            if (TriggerFlag == false) {
                txtdate.style.borderColor = "";
                if (IsNotNull == 'Y') {
                    if (ParamDate.trim() == '') {
                        result = false;
                        msgalert('Field Can Not Be Left Blank');
                        txtdate.style.borderColor = "Red";
                        if (txtdate.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                            return;
                        }
                        //CheckToUpdateValue(BtnUpdateId);
                        return;
                    }
                }
                if (ParamDate.trim() != '') {
                    var flg = false;
                    flg = DateConvert(ParamDate, txtdate);
                    if (flg == true && !CheckDateLessThenToday(txtdate.value) && (((ParamDate.indexOf('UK')) == -1 || (ParamDate.indexOf('UNK')) == -1 || (ParamDate.indexOf('UKUK')) == -1))) {
                        txtdate.value = "";
                        txtdate.focus();
                        msgalert('Date Should Be Less Than Current Date');
                        result = false;
                        if (txtdate.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                            return false;
                        }
                        //CheckToUpdateValue(BtnUpdateId);
                        return false;
                    }
                }
            }
            TriggerFlag = false;
            //Added By Vimal Ghoniya
            var TargetMedExCodeArray = TargetMedExCode.split(','), MedExCode = "";
            for (var count = 0; count < TargetMedExCodeArray.length; count++) {
                MedExCode = TargetMedExCodeArray[count];
                if (MedExCode != undefined && MedExCode != "") {
                    if (MedExCode.split("#")[2].toUpperCase().indexOf('[' + txtdate.value.toUpperCase() + ']') != -1) {
                        if (MedExCode.split("#")[1].toLowerCase() === "radio" ||
                            MedExCode.split("#")[1].toLowerCase() === "checkbox") {
                            $("#" + MedExCode.split("#")[0] + ' *').removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0] + ' *').val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "file") {
                            $("#FU" + MedExCode.split("#")[0]).removeAttr("disabled");
                            $("#FU" + MedExCode.split("#")[0]).val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "comboglobaldictionary") {
                            $("#" + MedExCode.split("#")[0]).siblings().removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }
                        else {
                            $("#" + MedExCode.split("#")[0]).removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).siblings().removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }

                    }
                    else {
                        if (MedExCode.split("#")[1].toLowerCase() === "radio" ||
                            MedExCode.split("#")[1].toLowerCase() === "checkbox") {
                            $("#" + MedExCode.split("#")[0] + ' *').attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0] + ' *').removeAttr('checked');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "file") {
                            $("#FU" + MedExCode.split("#")[0]).attr("disabled", "disabled");
                            $("#FU" + MedExCode.split("#")[0]).val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "comboglobaldictionary") {
                            $("#" + MedExCode.split("#")[0]).siblings().attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }
                        else {
                            $("#" + MedExCode.split("#")[0]).attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).siblings().attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).val('');

                        }
                    }
                }
            }
            //End
            if (txtdate.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                return true;
            }
            if (result == true) {
                CheckToUpdateValue(BtnUpdateId);
            }

            return true;
        }

        function DateValidationForCTM(ParamDate, txtdate, IsNotNull, BtnUpdateId, TargetMedExCode) {
            var result = true;
            if (TriggerFlag == false) {
                txtdate.style.borderColor = "";
                if (IsNotNull == 'Y') {
                    if (ParamDate.trim() == '') {
                        msgalert('Field can Not Be Left Blank');
                        result = false;
                        txtdate.style.borderColor = "Red";
                        if (txtdate.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                            return;
                        }
                        if (txtdate.value.toUpperCase() != PreviousValue.toUpperCase()) {
                            txtdate.style.borderColor = "navy";
                            CheckToUpdateValue(BtnUpdateId);
                            return;
                        }
                        //CheckToUpdateValue(BtnUpdateId);
                        return;
                    }
                }
                if (ParamDate.trim() != '') {

                    //Format Change Start
                    var dt = ParamDate.trim().toUpperCase();
                    var tempdt;
                    if (dt.indexOf('UK') >= 0 || dt.indexOf('UNK') >= 0 || dt.indexOf('UKUK') >= 0) {

                        if (dt.length < 8) {
                            msgalert('Please Enter Date in DDMMYYYY or dd-Mon-YYYY format only.');
                            txtdate.value = "";
                            txtdate.focus();
                            if (txtdate.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                                return false;
                            }
                            result = false;
                            //CheckToUpdateValue(BtnUpdateId);
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
                            msgalert('Please Enter Date In DDMMYYYY Or dd-Mon-YYYY Format Only.');
                            result = false;
                            txtdate.value = "";
                            txtdate.focus();
                            if (txtdate.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                                return false;
                            }
                            //CheckToUpdateValue(BtnUpdateId);
                            return false;
                        }
                        if (month.length > 3 && month.length != 3) {
                            msgalert('Please Enter Date In DDMMYYYY Or dd-Mon-YYYY Format Only.');
                            txtdate.value = "";
                            txtdate.focus();
                            if (txtdate.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                                return false;
                            }
                            result = false;
                            //CheckToUpdateValue(BtnUpdateId);
                            return false;
                        }
                        if (year.length > 4 && year.length != 4) {
                            msgalert('Please Enter Date In DDMMYYYY Or dd-Mon-YYYY Format Only.');
                            txtdate.value = "";
                            txtdate.focus();
                            if (txtdate.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                                return false;
                            }
                            result = false;
                            //CheckToUpdateValue(BtnUpdateId);
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
                                msgalert('You Can Not Add Date Which Is Less Than "01-Jan-1900" ');
                                txtdate.value = "";
                                txtdate.focus();
                            }
                            if (txtdate.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                                return true;
                            }
                            // CheckToUpdateValue(BtnUpdateId);
                            return true;
                        }
                        //msgalert('Please Enter Date in DDMMYYYY or dd-Mon-YYYY format only.');
                        txtdate.value = "";
                        txtdate.focus();
                        if (txtdate.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                            return false;
                        }
                        //CheckToUpdateValue(BtnUpdateId);
                        return false;
                    }
                    //End Format change
                    var flg = false;
                    flg = DateConvert(ParamDate, txtdate);
                    if (flg == true && !CheckDateLessThenToday(txtdate.value)) {
                        msgalert('Date Should Be Less Than Current Date');
                        txtdate.value = "";
                        txtdate.focus();
                        if (txtdate.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                            return false;
                        }
                        result = false;
                        //CheckToUpdateValue(BtnUpdateId);
                        return false;
                    }
                    else if (flg == false) {
                        msgalert('Please Enter Date In DDMMYYYY Or dd-Mon-YYYY Format Only.');
                        txtdate.value = PreviousValue
                        txtdate.focus();
                        if (txtdate.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                            return false;
                        }
                        result = false;
                        //CheckToUpdateValue(BtnUpdateId);
                        return false;
                    }
                    else if (flg == true) {
                        dt = txtdate.value.toUpperCase();
                        var Year = dt.substring(dt.lastIndexOf('-') + 1);
                        inyear = parseInt(Year, 10);
                        if (Year.length == 2) {
                            if (parseInt(Year) <= cCUTOFFYEAR) {
                                Year = "20" + Year.toString();
                            }
                            else {
                                Year = "19" + Year.toString();
                            }
                        }
                        var Day = dt.substring(0, dt.indexOf('-'));
                        var Month = dt.substring(dt.indexOf('-') + 1, dt.lastIndexOf('-'));
                        Month = ConvertMonthToInt(Month);
                        Month = parseFloat(Month);
                        Month = Month - 1;
                        var startDate = new Date();
                        startDate.setFullYear(Year, Month, Day);
                        txtdate.value = startDate.format('dd-MMM-yyyy');
                    }
                }
                if (inyear < 1900) {
                    msgalert('You Can Not Add Date Which Is Less Than "01-Jan-1900" ');
                    txtdate.value = "";
                    txtdate.focus();

                }
            }
            TriggerFlag = false;

            //Added By Vimal Ghoniya
            var TargetMedExCodeArray = TargetMedExCode.split(','), MedExCode = "";
            for (var count = 0; count < TargetMedExCodeArray.length; count++) {
                MedExCode = TargetMedExCodeArray[count];
                if (MedExCode != undefined && MedExCode != "") {
                    if (MedExCode.split("#")[2].toUpperCase().indexOf('[' + txtdate.value.toUpperCase() + ']') != -1) {
                        if (MedExCode.split("#")[1].toLowerCase() === "radio" ||
                            MedExCode.split("#")[1].toLowerCase() === "checkbox") {
                            $("#" + MedExCode.split("#")[0] + ' *').removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0] + ' *').val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "file") {
                            $("#FU" + MedExCode.split("#")[0]).removeAttr("disabled");
                            $("#FU" + MedExCode.split("#")[0]).val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "comboglobaldictionary") {
                            $("#" + MedExCode.split("#")[0]).siblings().removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "standarddate") {
                            $('select[id*="' + MedExCode.split("#")[0] + '"]').removeAttr("disabled");
                            $('select[id*="' + MedExCode.split("#")[0] + '"]').val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "standarddatetime") {
                            $('select[id*="' + MedExCode.split("#")[0] + '"]').removeAttr("disabled");
                            $('select[id*="' + MedExCode.split("#")[0] + '"]').val('');
                        }
                        else {
                            $("#" + MedExCode.split("#")[0]).removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).siblings().removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }

                    }
                    else {
                        if (MedExCode.split("#")[1].toLowerCase() === "radio" ||
                            MedExCode.split("#")[1].toLowerCase() === "checkbox") {
                            $("#" + MedExCode.split("#")[0] + ' *').attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0] + ' *').removeAttr('checked');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "file") {
                            $("#FU" + MedExCode.split("#")[0]).attr("disabled", "disabled");
                            $("#FU" + MedExCode.split("#")[0]).val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "comboglobaldictionary") {
                            $("#" + MedExCode.split("#")[0]).siblings().attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "standarddate") {
                            $('select[id*="' + MedExCode.split("#")[0] + '"]').attr("disabled", "disabled");
                            $('select[id*="' + MedExCode.split("#")[0] + '"]').val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "standarddatetime") {
                            $('select[id*="' + MedExCode.split("#")[0] + '"]').attr("disabled", "disabled");
                            $('select[id*="' + MedExCode.split("#")[0] + '"]').val('');
                        }
                        else {
                            $("#" + MedExCode.split("#")[0]).attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).siblings().attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).val('');

                        }
                    }
                }
            }
            //End

            if (txtdate.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                return true;
            }
            if (result == true) {


                CheckToUpdateValue(BtnUpdateId);
            }
            // CheckToUpdateValue(BtnUpdateId);

            return true;
        }

        function AutoTimeConvert(ParamTime, txtTime, IsNotNull, BtnUpdateId, TargetMedExCode) {
            if (ParamTime.trim().length > 0) {
                if (/^[0-9:]*$/.test(ParamTime) == false) {
                    msgalert("Please enter time in correct formate as HH:MM")
                    txtTime.value = ""
                    return;
                }
                if (ParamTime.match(/[^a-zA-Z~!@#$&%*()^]/g) != null) {
                    if (ParamTime.match(/[^a-zA-Z]/g).length != ParamTime.length) {
                        msgalert("Please enter time in correct formate as HH:MM")
                        txtTime.value = ""
                        return;
                    }
                }
                else {
                    msgalert("Please enter time in correct formate as HH:MM")
                    txtTime.value = ""
                    return;
                }
            }

            var result = true;
            if (TriggerFlag == false) {
                txtTime.style.borderColor = "";
                if (IsNotNull == 'Y') {
                    if (ParamTime.trim() == '') {
                        msgalert('Field Can Not Be Left Blank');
                        txtTime.style.borderColor = "Red";
                        if (txtTime.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                            return;
                        }
                        if (txtTime.value.toUpperCase() != PreviousValue.toUpperCase()) {
                            txtTime.style.borderColor = "navy";
                            CheckToUpdateValue(BtnUpdateId);
                            return;
                        }
                        result = false;
                        if (result == true) {
                            CheckToUpdateValue(BtnUpdateId);
                        }
                        //CheckToUpdateValue(BtnUpdateId);
                        return;
                    }
                }
                var bolresilt = TimeConvert(ParamTime, txtTime);
                if (bolresilt == false) {
                    txtTime.value = ""
                    return;
                }
                if (txtTime.value.toUpperCase() != PreviousValue.toUpperCase()) {
                    if (IsNotNull.toUpperCase() == "N") {
                        CheckToUpdateValue(BtnUpdateId);
                        return;
                    }
                    else {
                        return;
                    }
                }
            }
            TriggerFlag = false;

            //Added By Vimal Ghoniya
            var TargetMedExCodeArray = TargetMedExCode.split(','), MedExCode = "";
            for (var count = 0; count < TargetMedExCodeArray.length; count++) {
                MedExCode = TargetMedExCodeArray[count];
                if (MedExCode != undefined && MedExCode != "") {
                    if (MedExCode.split("#")[2].toUpperCase().indexOf('[' + txtTime.value.toUpperCase() + ']') != -1) {
                        if (MedExCode.split("#")[1].toLowerCase() === "radio" ||
                            MedExCode.split("#")[1].toLowerCase() === "checkbox") {
                            $("#" + MedExCode.split("#")[0] + ' *').removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0] + ' *').val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "file") {
                            $("#FU" + MedExCode.split("#")[0]).removeAttr("disabled");
                            $("#FU" + MedExCode.split("#")[0]).val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "comboglobaldictionary") {
                            $("#" + MedExCode.split("#")[0]).siblings().removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }
                        else {
                            $("#" + MedExCode.split("#")[0]).removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).siblings().removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }

                    }
                    else {
                        if (MedExCode.split("#")[1].toLowerCase() === "radio" ||
                            MedExCode.split("#")[1].toLowerCase() === "checkbox") {
                            $("#" + MedExCode.split("#")[0] + ' *').attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0] + ' *').removeAttr('checked');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "file") {
                            $("#FU" + MedExCode.split("#")[0]).attr("disabled", "disabled");
                            $("#FU" + MedExCode.split("#")[0]).val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "comboglobaldictionary") {
                            $("#" + MedExCode.split("#")[0]).siblings().attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }
                        else {
                            $("#" + MedExCode.split("#")[0]).attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).siblings().attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }
                    }
                }
            }
            //End
            if (txtTime.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                return;
            }
            CheckToUpdateValue(BtnUpdateId);

        }
        function Next(NoneDivId) {
            var arrDiv = NoneDivId.split(',');
            var isShow = false;
            for (i = 0; i < arrDiv.length; i++) {
                document.getElementById(arrDiv[i]).style.display = 'none';
                var disBtn = arrDiv[i].replace('Div', 'BtnDiv');
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
            document.getElementById(currTab).style.display = '';
            document.getElementById(currBtn).style.color = '#FFC300';
            return false;
        }

        function Previous(NoneDivId) {
            var arrDiv = NoneDivId.split(',');
            for (i = 0; i < arrDiv.length; i++) {
                document.getElementById(arrDiv[i]).style.display = 'none';
                var disBtn = arrDiv[i].replace('Div', 'BtnDiv');
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
            document.getElementById(currTab).style.display = '';
            document.getElementById(currBtn).style.color = '#FFC300';
            return false;
        }

        function DisplayDiv(BlockDivId, NoneDivId) {
            var selBtn = BlockDivId.replace('Div', 'BtnDiv');
            var arrDiv = NoneDivId.split(',');
            for (i = 0; i < arrDiv.length; i++) {
                document.getElementById(arrDiv[i]).style.display = 'none';
                var disBtn = arrDiv[i].replace('Div', 'BtnDiv');
                document.getElementById(disBtn).style.color = '#FFffff';
                if (selBtn.toLowerCase() == disBtn.toLowerCase()) {
                    currTab = arrDiv[i];
                }
            }
            document.getElementById(BlockDivId).style.display = '';
            document.getElementById('HFActivateTab').value = BlockDivId;
            document.getElementById(selBtn).style.color = '#FFC300';

            return false;
        }


        //Edit Check Query Added by Vivek Patel 
        function EditCheckQuery(Type, MedexCode, iRepeatNo) {
            debugger;
            //$(GDVEditcheckQuery).DataTable({ "bPaginate": false, "bSort": false, "bJQueryUI": true, "sDom": 'T<"clear">lrtip' });
            if (Type == 'A') {
                document.getElementById('hdnMedExCode').value = MedexCode;
                document.getElementById('hdniRepeatNo').value = iRepeatNo;

                btnEditCheckQuery.click();
                return false;
            }
        }
        //Added by Vivek Patel 
        function fnEditCheckQueryUI() {
            if (document.getElementById('GDVEditcheckQuery') != null) {

                $(GDVEditcheckQuery).DataTable({
                    "bJQueryUI": true,
                    "sPaginationType": "full_numbers",
                    "bLengthChange": true,
                    "iDisplayLength": 10,
                    "bProcessing": true,
                    "bSort": false,
                    aLengthMenu: [
                        [10, 25, 50, 100, -1],
                        [10, 25, 50, 100, "All"]
                    ],
                });
            }
        }
        //Added by Vivek Patel 
        function fnGDVEditcheckUI() {

            if (document.getElementById('GDVEditcheck') != null) {
                $(GDVEditcheck).find('tbody tr').length < 3 ? scroll = "25%" : scroll = "275px";
                //$(GDVEditcheck).prepend($('<thead>').append($(GDVEditcheck).find('tr:first')))
                $(GDVEditcheck).dataTable({
                    "bDestroy": true,
                    "bJQueryUI": true,
                    "sScrollY": "275px",
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
                setTimeout(function () { $(GDVEditcheck).dataTable().fnAdjustColumnSizing(); }, 10);
            }
        }

        function fnAditTrail() {
            if (document.getElementById('GVHistoryDtl') != null) {

                $(GVHistoryDtl).DataTable({
                    "scrollX": '50vh',
                    "scrollY": '50vh',
                    "scrollCollapse": true,
                    "bJQueryUI": true,
                    "sPaginationType": "full_numbers",
                    "bLengthChange": true,
                    "iDisplayLength": 10,
                    "bProcessing": true,
                    "bSort": false,
                    aLengthMenu: [
                        [10, 25, 50, 100, -1],
                        [10, 25, 50, 100, "All"]
                    ],
                });
            }
        }

        function fnDCF() {
            if (document.getElementById('GVWDCF') != null) {
                $(GVWDCF).DataTable({
                    "scrollX": '50vh',
                    "scrollY": '50vh',
                    "scrollCollapse": true,
                    "bJQueryUI": true,
                    "sPaginationType": "full_numbers",
                    "bLengthChange": true,
                    "iDisplayLength": 10,
                    "bProcessing": true,
                    "bSort": false,
                    "oLanguage": {
                        "sEmptyTable": "Record not found"
                    },
                    aLengthMenu: [
                        [10, 25, 50, 100, -1],
                        [10, 25, 50, 100, "All"]
                    ],
                });
            }
        }


        function HistoryDivShowHide(Type, MedexCode, BlockDivId, NoneDivId, CRFDtlNo) {

            document.getElementById('hfMedexCode').value = MedexCode;
            document.getElementById('HFCRFDtlNo').value = CRFDtlNo;


            if (document.getElementById(MedexCode) != null) {
                document.getElementById('hdnOldMedExValue').value = document.getElementById(MedexCode).value
            }

            var tagName
            try {
                tagName = document.getElementById(MedexCode + "_1").tagName
            }
            catch (err) {
                tagName = ""
            }

            if (tagName == "SELECT") {
                if (document.getElementById(MedexCode + "_4") == null) {
                    document.getElementById('HFMedexType').value = "STANDARDDATE"
                    MedexVal = $('select[id*="' + MedexCode + '"]')[0].value + $('select[id*="' + MedexCode + '"]')[1].value + $('select[id*="' + MedexCode + '"]')[2].value
                }
                else {
                    document.getElementById('HFMedexType').value = "STANDARDDATETIME"
                    MedexVal = $('select[id*="' + MedexCode + '"]')[0].value + $('select[id*="' + MedexCode + '"]')[1].value + $('select[id*="' + MedexCode + '"]')[2].value + $('select[id*="' + MedexCode + '"]')[3].value + $('select[id*="' + MedexCode + '"]')[4].value
                }
                document.getElementById('hdnOldMedExValue').value = MedexVal
            }
            if (tagName == "INPUT") {
                var radiolst = document.getElementById(MedexCode);
                var radios;
                var i;
                if (radiolst != null && typeof (radiolst) != 'undefined') {
                    radios = radiolst.getElementsByTagName('input');
                    for (i = 0; i < radios.length; i++) {
                        if (radios[i].checked) {
                            document.getElementById('hdnOldMedExValue').value = radios[i].value
                        }
                    }
                }

            }

            document.getElementById('hndLetestData').value = "GetAuditTrail";

            var btnAudit = document.getElementById('btnAudittrail');
            debugger;
            if (Type == 'S') {
                return false;
            }
            else if (Type == 'H') {
                // document.getElementById('divHistoryDtl').style.display = 'none';
                funCloseDiv('divHistoryDtl');
                return false;
            }
            else if (Type == 'A') {
                btnAudit.click();
                return false;
            }
            else if (Type == 'SN') {
                document.getElementById('divHistoryDtl').style.display = '';
                displayBackGround();
                //SetCenter('divHistoryDtl');
                return DisplayDiv(BlockDivId, NoneDivId);
            }
            return true;
        }

        function AuditDivShowHide(Type, MedexCode, buttonId, CRFDtlNo, MedexType) {
            document.getElementById('hdnOldMedExType').value = Type
            document.getElementById('HFMedexType').value = MedexType

            var oldvalue = document.getElementById('hdnOldMedExValue').value

            var type = Type
            var MedexVal = "";
            var eledisable = false;
            var chklst = document.getElementById(MedexCode);
            var chks;
            var i;


            //if (MedexType != undefined || MedexType != null) {
            //    document.getElementById('hdnOldMedExValue').value = document.getElementById(MedexCode).value
            //}

            if (MedexType != undefined || MedexType != null) {

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
                                    if (MedexVal == "") {
                                        MedexVal = $(chks[i]).next().text();
                                    } else {
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
                                if (chks[i].value == document.getElementById('hdnOldMedExValue').value) {
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
                                if (radios[i].value == document.getElementById('hdnOldMedExValue').value) {
                                    radios[i].checked = true;
                                }
                                radios[i].disabled = true;
                                $(radios[i]).parents('span').attr('disabled', 'disabled');
                            }
                        }
                    }
                }
                // }
                if (MedexVal == "" && (MedexType != "STANDARDDATE" && MedexType != "STANDARDDATETIME") && document.getElementById(MedexCode) != null) {
                    // var chklst = document.getElementById(MedexCode)
                    document.getElementById('hdnOldMedExValue').value = document.getElementById(MedexCode).value
                }

                    //if (document.getElementById('hdnOldMedExValue').value != "" && MedexType.toUpperCase() == "STANDARDDATE") {
                    //    // var chklst = document.getElementById(MedexCode)
                    //    document.getElementById('hdnOldMedExValue').value = document.getElementById(MedexCode).value
                    //}
                    //else if (document.getElementById('hdnOldMedExValue').value != "" && MedexType.toUpperCase() == "STANDARDDATE") {
                else if (MedexType.toUpperCase() == "STANDARDDATE") {
                    document.getElementById('hdnOldMedExValue').value = $('select[id*="' + MedexCode + '"]')[0].value + $('select[id*="' + MedexCode + '"]')[1].value + $('select[id*="' + MedexCode + '"]')[2].value
                }
                else if (MedexType.toUpperCase() == "STANDARDDATETIME") {
                    document.getElementById('hdnOldMedExValue').value = $('select[id*="' + MedexCode + '"]')[0].value + $('select[id*="' + MedexCode + '"]')[1].value + $('select[id*="' + MedexCode + '"]')[2].value + $('select[id*="' + MedexCode + '"]')[3].value + $('select[id*="' + MedexCode + '"]')[4].value
                }
                else {
                    document.getElementById('hdnOldMedExValue').value = MedexVal
                }
            }
            var btnE = document.getElementById('btnEdit');
            var btnD = document.getElementById('btnDCF');
            document.getElementById('hfMedexCode').value = MedexCode;
            document.getElementById('HFCRFDtlNo').value = CRFDtlNo;

            if (Type == 'E') {

                if (MedexType == "File") {
                    MedexCode = "FU" + MedexCode
                }
                if (MedexType.toUpperCase() == "STANDARDDATE") {
                    $('select[id*="' + MedexCode + '"]').attr('disabled', false);
                    $('select[id*="' + MedexCode + '"]').attr('readOnly', false);

                    if (document.getElementById('hdnOldMedExValue').value == "") {

                        MedexVal = $('select[id*="' + MedexCode + '"]')[0].value + $('select[id*="' + MedexCode + '"]')[1].value + $('select[id*="' + MedexCode + '"]')[2].value
                        document.getElementById('hdnOldMedExValue').value = MedexVal
                    }
                }
                else if (MedexType.toUpperCase() == "STANDARDDATETIME") {
                    $('select[id*="' + MedexCode + '"]').attr('disabled', false);
                    $('select[id*="' + MedexCode + '"]').attr('readOnly', false);

                    if (document.getElementById('hdnOldMedExValue').value == "") {

                        MedexVal = $('select[id*="' + MedexCode + '"]')[0].value + $('select[id*="' + MedexCode + '"]')[1].value + $('select[id*="' + MedexCode + '"]')[2].value + $('select[id*="' + MedexCode + '"]')[3].value + $('select[id*="' + MedexCode + '"]')[4].value
                        document.getElementById('hdnOldMedExValue').value = MedexVal
                    }
                }
                else {
                    document.getElementById(MedexCode).disabled = false;
                    document.getElementById(MedexCode).removeAttribute('readOnly'); //Enhancement in EDC
                }
                var chklst = document.getElementById(MedexCode);
                var chks;
                var i;
                if (chklst != null && typeof (chklst) != 'undefined') {
                    chks = chklst.getElementsByTagName('input');
                    for (i = 0; i < chks.length; i++) {
                        if (chks[i].type.toUpperCase() == 'CHECKBOX') {
                            chks[i].disabled = false;
                            $(chks[i]).parents('span').removeAttr('disabled');
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
                            radios[i].disabled = false;
                            $(radios[i]).parents('span').removeAttr('disabled');
                        }
                    }
                }

                document.getElementById('btnUpdate' + buttonId).disabled = false;
                if (document.getElementById('btnBrowse' + buttonId) != null) {
                    document.getElementById('btnBrowse' + buttonId).disabled = false;
                }
                //document.getElementById('btnEdit' + buttonId).disabled = true;
                TriggerFlag = false;
                if (MedexType.toUpperCase() == "STANDARDDATE") $('select[id*="' + MedexCode + '"]')[0].focus();
                else if (MedexType.toUpperCase() == "STANDARDDATETIME") $('select[id*="' + MedexCode + '"]')[0].focus();
                else document.getElementById(MedexCode).focus();

                return false;
            }
            if (Type == 'U') {

                if (MedexType == "File") {
                    MedexCode = "FU" + MedexCode
                }

                //                var prm = Sys.WebForms.PageRequestManager.getInstance();
                //                var prm = Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginRequestHandler)
                //msgalert(prm);


                document.getElementById('btnUpdate' + buttonId).disabled = true;
                document.getElementById('btnEdit' + buttonId).disabled = false;
                document.getElementById('HFChkSelectedList').value = '';

                if (MedexType.toUpperCase() != "STANDARDDATE" && MedexType.toUpperCase() != "STANDARDDATETIME") {
                    //var chklst = $('select[id*="' + MedexCode + '"]');
                    //var chkValue = $(chklst[0]).val() + $(chklst[1]).find('option:selected').text() + $(chklst[2]).val();
                    var chklst = document.getElementById(MedexCode);
                    var chkValue = chklst.value;


                    var chks;
                    var i;
                    var files;

                    //                sessionStorage.setItem("Content", chkValue);
                    //                document.getElementById('HFFileTypeValue').value=chklst.value;
                    //              // Added By Dipen Shah On  1 Sept 2014

                    //   var xhr = new XMLHttpRequest();
                    //   xhr.open("POST", "frmCTMMedExInfoHdrDtl.aspx.vb/FileValue", true);
                    //   xhr.setRequestHeader("FileValue", chklst.value + "###" + MedexCode + "###" + MedexType);
                    //   xhr.send(files)



                    if (chklst != null && typeof (chklst) != 'undefined' && MedexType.toUpperCase() != "STANDARDDATE" && MedexType.toUpperCase() != "STANDARDDATETIME") {
                        chks = chklst.getElementsByTagName('input');
                        for (i = 0; i < chks.length; i++) {
                            if (chks[i].type.toUpperCase() == 'CHECKBOX') {
                                if (chks[i].checked) {
                                    if (document.getElementById('HFChkSelectedList').value == '') {
                                        document.getElementById('HFChkSelectedList').value = $(chks[i]).parents('span')[0].children[1].innerHTML;
                                    }
                                    else {
                                        document.getElementById('HFChkSelectedList').value += ',' + $(chks[i]).parents('span')[0].children[1].innerHTML;
                                    }
                                }

                            }
                        }
                    }
                    if (Type == 'U') {
                        if (oldvalue == document.getElementById('hdnOldMedExValue').value) {
                            msgalert("you have not change any value.")
                            document.getElementById('btnUpdate' + buttonId).disabled = false;
                            document.getElementById('btnEdit' + buttonId).disabled = false;
                            document.getElementById(MedexCode).disabled = true;
                            if (type = 'checkbox') {
                                $("#" + MedexCode + " td  span [type= checkbox]").attr('disabled', 'disabled')
                            }
                            funCloseDiv('divForEditAttribute');
                            return false;
                        }
                        document.getElementById('hdnOldMedExValue').value = oldvalue
                        btnE.click();
                        return true;

                    }
                    else {
                        document.getElementById('hdnOldMedExValue').value = oldvalue
                        btnE.click();
                        return true;
                    }
                }
                else {

                    count = 0;
                    $('select[id*="' + MedexCode + '"]').each(function () {
                        this.style.backgroundColor = '';
                        if (this.value.trim().length <= 0) {
                            this.style.backgroundColor = 'Red';
                            count = count + 1;
                        }
                    });
                    if (count > 0) {
                        var conf = confirm('' + count + ' Field(s) Are Blank, Do You Still Want To Save?');
                        if (conf) {
                            document.getElementById('hdnOldMedExValue').value = oldvalue
                            btnE.click();
                        }
                    }
                    else {
                        if (Type == 'U') {
                            if (oldvalue == document.getElementById('hdnOldMedExValue').value) {
                                msgalert("you have not change any value.")
                                document.getElementById('btnUpdate' + buttonId).disabled = false;
                                document.getElementById('btnEdit' + buttonId).disabled = false;
                                document.getElementById(MedexCode).disabled = true;
                                if (type = 'checkbox') {
                                    $("#" + MedexCode + " td  span [type= checkbox]").attr('disabled', 'disabled')
                                }

                                funCloseDiv('divForEditAttribute');
                                return false;
                            }
                            document.getElementById('hdnOldMedExValue').value = oldvalue
                            btnE.click();
                            return true;

                        }
                        else {
                            document.getElementById('hdnOldMedExValue').value = oldvalue
                            btnE.click();
                            return true;
                        }
                    }
                }

                return false;
            }
            if (Type == 'D') {
                if (document.getElementById(MedexCode) != null) {
                    document.getElementById('hdnOldMedExValue').value = document.getElementById(MedexCode).value
                }
                btnD.click();
                return false;
            }
            if (Type == 'S') {
                var btnS = document.getElementById('btnSaveRunTime');
                btnS.click();
                return false;
            }
            return true;
        }


        function AnyDivShowHide(Type) {
            if (Type == 'S') {
                document.getElementById('divForEditAttribute').style.display = '';
                displayBackGround();
                //SetCenter('divForEditAttribute');
                return false;
            }
            else if (Type == 'H') {
                //document.getElementById('divForEditAttribute').style.display = 'none';
                funCloseDiv('divForEditAttribute');
                return false;
            }
            else if (Type == 'DCFSHOW') {
                document.getElementById('divDCF').style.display = '';
                displayBackGround();
                //SetCenter('divDCF');
                return false;
            }
            else if (Type == 'DCFHIDE') {
                //document.getElementById('divDCF').style.display = 'none';
                funCloseDiv('divDCF');
                return false;
            }
            else if (Type == 'DIVQUERIESSHOW') {
                document.getElementById('divQueries').style.display = '';
                SetCenter('divQueries');
                return false;
            }
            else if (Type == 'DIVQUERIESHIDE') {
                document.getElementById('divQueries').style.display = 'none';
                return false;
            }
                //added by Vivek Patel
            else if (Type == 'divEditChecksQuery') {
                document.getElementById('divEditChecksQuery').style.display = '';
                displayBackGround();
                return false;
            }
            return true;
        }
        function DivAuthenticationHideShow(Type) {
            if (Type == 'S') {
                document.getElementById('divAuthentication').style.display = '';
                displayBackGround();
                //SetCenter('divAuthentication');
                document.getElementById('txtPassword').value = '';
                document.getElementById('txtPassword').focus();
                return false;
            }
            else if (Type == 'H') {
                //document.getElementById('divAuthentication').style.display = 'none';
                funCloseDiv('divAuthentication');
                return false;
            }
            return true;
        }

        function ValidationForAuthentication() {
            if (document.getElementById('txtPassword').value.trim() == '') {
                document.getElementById('txtPassword').value = '';
                msgalert('Please Enter Password For Authentication');
                document.getElementById('txtPassword').focus();
                return false;
            }
            return true;
        }
        function MeddraBrowser(MedExCode, nMedExWorkSpaceDtlNo) {

            document.getElementById('hfMedexCode').value = MedExCode;
            MedExCode = MedExCode.substring(0, MedExCode.indexOf('R'));
            window.open('frmCTMMeddraBrowse.aspx?MedExCode=' + MedExCode + '&MedExWorkSpaceDtlNo=' + nMedExWorkSpaceDtlNo);
            return false;
        }

        function SetMeddra(objLLT) {
            var MedExCode = document.getElementById('hfMedexCode').value;
            var control = document.getElementById(MedExCode);
            if (control != null && typeof (control) != 'undefined') {
                control.value = objLLT.Meddra;
                control.focus();
            }
        }

        function ValidateReview() {
            var chk = document.getElementById('chkReviewCompleted');
            var chkAll = document.getElementById('chkReviewAll');
            if (!chk.checked && !chkAll.checked) {
                msgalert('Select The Check Box And Then Click On OK.');
                return false;
            }
            /*-----Added By Dipen Shah On 7-Feb=2015 to prevent Double checkbox checked.-----*/
            if (chk.checked && chkAll.checked) {
                msgalert("Please select only one check box and then click on OK.");
                return false;
            }
            /*--------*/

            if (chk.checked) {
                document.getElementById('HReviewFlag').value = 1;
            }
            else {
                document.getElementById('HReviewFlag').value = 0;
            }
            //            if (DCFCount > 0) {
            //                var Result = confirm(DCFCount + ' Discrepancy Pending, You Can Not Review.');
            //                return false;
            //            }
            //            //DivAuthenticationHideShow('S');
            //            //document.getElementById('BtnAuthentication').click();
            //            document.getElementById('divAuthentication').style.display = '';
            //            displayBackGround();
            //            document.getElementById('txtPassword').focus();
            //            //            PageMethods.GetServerDateTimeFor(function(Result)
            //            //            {
            //            //                document.getElementById('lblSignDateTime').innerHTML = Result;
            //            //            }, function(eerror) { alert(eerror); });

            //return false;
            return true;
        }

        function CheckTheEnterKey(e) {
            var unicode = e.charCode ? e.charCode : e.keyCode
            if (unicode == 13) {
                document.getElementById('btnAuthenticate').click();
                return false;
            }
            return true;
        }
        function MedExFormula(MedExCode, formula, Decimalno) {
            document.getElementById('hfMedexCode').value = MedExCode;
            document.getElementById('HFMedExFormula').value = formula;
            document.getElementById('hfDecimalNo').value = Decimalno; //Added By dipen Shah
            //alert("-----" + document.getElementById('hfDecimalNo').value);
            var btn = document.getElementById('btnAutoCalculate');
            btn.click("hfDecimalNo:" + document.getElementById('hfDecimalNo').value);
            return false;
        }
        function SetFormulaResult(result, deci) {
            var MedExCode = document.getElementById('hfMedexCode').value;
            var control = document.getElementById(MedExCode);
            if (control != null && typeof (control) != 'undefined') {
                control.value = DecimalPoint(result, deci);
                control.focus();
            }
            if ($(control).parents('tr').find('[id^=btnUpdate]')) {
                $(control).parents('tr').find('[id^=btnUpdate]').attr('disabled', '');
            }
        }

        function DecimalPoint(result, Decimal) {
            if (result.split('.')[1]) {
                if (result.split('.')[1].length != Decimal) {
                    for (var i = result.split('.')[1].length ; i <= Decimal - 1 ; i++) {
                        result = result + "0";
                    }
                }
            }
            else {
                if (Decimal != 0) {
                    result = result + ".";
                }
                for (var i = 0 ; i <= Decimal - 1 ; i++) {
                    result = result + "0";
                }
            }
            return result;
        }
        function SetDosingTime(medexcode, BtnUpdateId) {
            var txtdosingtime = document.getElementById(medexcode);
            var d = new Date();
            var curr_hour = d.getHours();
            var curr_min = d.getMinutes();
            //            var curr_sec = d.getSeconds();
            txtdosingtime.value = curr_hour + ":" + curr_min;
            //             + ":" + curr_sec;
            if (txtdosingtime.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null) {
                return;
            }
            CheckToUpdateValue(BtnUpdateId);
            return true;
        }

        function ValidationForUpdate() {
            if (document.getElementById('txtRemarkForAttributeEdit').value.trim() == '') {
                msgalert('Please enter remarks.');
                document.getElementById('txtRemarkForAttributeEdit').value = '';
                document.getElementById('txtRemarkForAttributeEdit').focus();
                return false;
            }
            return true;
        }

        function ValidationForDiscrepancy() {
            if (document.getElementById('txtDiscrepancyRemarks').value.trim() == '') {
                msgalert('Please enter remarks.');
                document.getElementById('txtDiscrepancyRemarks').value = '';
                document.getElementById('txtDiscrepancyRemarks').focus();
                return false;
            }
            //$find('MpeDCF').hide();
            funCloseDiv('divForEditAttribute');
            return true;
        }

        var count = 0;
        var element;
        var prev;
        var result;

        function ValidationsForSave() {

            var flg = true;
            $('select[standarddate="Y"]', $('.FieldSetBox')[1]).each(function () {
                if (this.id.indexOf("_1") > 0) {
                    if ($('input[id*="btnEdit"]', $(this).parent().next()).length == 1 && this.disabled != true) {
                        msgalert("Please Updae values.");
                        this.focus();
                        flg = false;
                    }
                }
            });
            if (flg == true) {
                document.getElementById('BtnSave').style.display = 'none';
                document.getElementById('btnSaveAndContinue').style.display = 'none';
                try {
                    count = 0;
                    jQuery('.Required').each(validateControls);
                    if (count > 0) {
                        var conf = confirm('' + count + ' Field(s) Are Blank, Do You Still Want To Save?');
                        if (conf) {
                            document.getElementById('BtnSave').style.display = 'none';
                            document.getElementById('btnSaveAndContinue').style.display = 'none';
                        }
                        else {
                            document.getElementById('BtnSave').style.display = '';
                            document.getElementById('btnSaveAndContinue').style.display = '';
                            return false;
                        }
                    }

                    jQuery('input[type=checkbox]', $get('UpPlaceHolder')).attr('disabled', false).parents('table').attr('disabled', false);
                    //            if (jQuery('input[type=checkbox]', $get('UpPlaceHolder')).parents('table').length > 0)
                    //                jQuery('input[type=checkbox]', $get('UpPlaceHolder')).parents('table')[0].disabled = false;
                    onUpdating();
                    return true;
                }
                catch (errr) {
                    msgalert('An error has occurred: ' + err.message);
                    return true;
                }
            }
            else return false;
        }

        function validateControls(index) {
            element = jQuery('.Required')[index];
            /*Added By dipen Shah on 4-feb-2015*/
            /*-----*/
            if (element.type == undefined) {
                element = $("#" + $('.Required')[index].id).closest('tr').find('input[type]')

                if (element.length > 0) {
                    if ($("#" + $(".Required")[index].id).find('input[type=radio]').length > 0) {
                        var rdos = $("#" + $(".Required")[index].id).find('input[type=radio]');
                        var j;
                        result = false;
                        for (j = 0; j < rdos.length; j++) {
                            rdos[j].style.color = 'Navy';
                            if (rdos[j].checked) {
                                result = true;
                                $("#" + rdos[0].id).closest('table').css('background', '')
                                return;
                            }
                        }
                        if (result == false) {
                            $("#" + rdos[0].id).closest('table').css('background', 'red')
                            count = count + 1;
                            return;
                        }
                    }
                    if ($("#" + $(".Required")[index].id).find('input[type=checkbox]').length > 0) {
                        var chks;
                        var j;
                        result = false
                        chks = $("#" + $(".Required")[index].id).find('input[type=checkbox]');
                        for (j = 0; j < chks.length; j++) {
                            chks[j].style.color = 'Navy'
                            if (chks[j].checked) {
                                result = true;
                                $("#" + $(".Required")[index].id).css('background', '')
                                return;
                            }
                        }
                        if (result == false) {
                            $("#" + $(".Required")[index].id).css('background', 'red');
                            count = count + 1;
                            return;
                        }
                    }
                }
                /*---------*/
            }
            switch (element.type) {
                case 'text':
                    document.getElementById(element.id).style.borderColor = '';
                    if (element.value.trim().length <= 0) {
                        document.getElementById(element.id).style.borderColor = 'Red';
                        count = count + 1;
                        return;
                    }
                    break;

                case 'textarea': // Added by Dipen Shah on 9-March-2015
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
                        //if ($(element).attr('standarddate') != "Y" || (($(element).attr('standarddate') == "Y") && element.id.indexOf("_2") < 0 && element.id.indexOf("_3") < 0))
                        count = count + 1;
                        return;
                    }
                    break;

                case 'radio':
                    if (prev != element.name) {
                        prev = element.name;
                        var rdos = document.getElementsByName(element.name);
                        var j;
                        result = false;
                        for (j = 0; j < rdos.length; j++) {
                            rdos[j].style.color = 'Navy';
                            if (rdos[j].checked) {
                                result = true;
                                return;
                            }
                        }
                        if (result == false) {
                            for (j = 0; j < rdos.length; j++) {
                                rdos[j].style.color = 'Red';
                            }
                            count = count + 1;
                            return;
                        }
                    }
                    break;

                case 'checkbox':
                    if (prev != element.name) {
                        prev = element.name;
                        var chks;
                        var j;
                        result = false;
                        if (element != null && typeof (element) != 'undefined') {
                            if (document.getElementById(element.id).canHaveChildren == true) {
                                chks = element.getElementsByTagName('input');
                                for (j = 0; j < chks.length; j++) {
                                    chks[j].style.backgroundColor = '';
                                    if (chks[j].type.toUpperCase() == 'CHECKBOX' && chks[j].checked) {
                                        result = true;
                                        return;
                                    }
                                }
                            }
                            else {
                                element.style.backgroundColor = '';
                                if (element.checked) {
                                    result = true;
                                    return;
                                }
                            }
                        }
                        if (result == false) {
                            if (document.getElementById(element.id).canHaveChildren == true) {
                                chks = element.getElementsByTagName('input');
                                for (j = 0; j < chks.length; j++) {
                                    chks[j].style.backgroundColor = 'Red';
                                }
                            }
                            else {
                                element.style.backgroundColor = 'Red';
                            }
                            count = count + 1;
                            return;
                        }
                    }
                    break;
            }
        }

        function CheckToUpdateValue(BtnUpdateId) {
            var btnUpdate = document.getElementById(BtnUpdateId);
            if (btnUpdate != null) {
                //document.getElementById('BtnForEditAttribute').click();
                document.getElementById('divForEditAttribute').style.display = ''
                displayBackGround();
            }
        }
        function SetValue(Id, Value) {

            if (ControlId != Id && PreviousValue == Value) {
                ControlId = Id;
                PreviousValue = Value;
            }
            else if (ControlId != Id) {
                ControlId = Id;
                PreviousValue = Value;
            }
        }

        function ResetValue() {
            //document.getElementById(ControlId).value = PreviousValue;
            element = document.getElementById(ControlId);
            if (element.type != 'undefined') {
                if (element.type == 'text') {
                    document.getElementById(ControlId).value = PreviousValue;
                }

                else if (element.type == 'select-one') {
                    document.getElementById(ControlId).value = PreviousValue;
                }

                else {
                    var tblRdo = $get(ControlId);
                    if (tblRdo != null) {
                        var name = tblRdo.id;
                        name = name.replace(/_/g, '$');
                        var rdos = document.getElementsByName(name);
                        var i;
                        for (i = 0; i < rdos.length; i++) {
                            if (rdos[i].nextSibling != null) {
                                rdos[i].checked = false;
                                var values = PreviousValue.split('##');
                                var j;
                                for (j = 0; j < values.length; j++) {
                                    if (rdos[i].nextSibling.innerText.toUpperCase() == values[j].toUpperCase()) {
                                        rdos[i].checked = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        function GetCurrentDate(CtrlId, TargetMedExCode) {
            //Commented and added by Aaditya on 16-Nov-2015 for auto date capture Zone wise
            //document.getElementById(CtrlId).value = document.getElementById('hdnServerDateTime').value
            var curr_Date;
            var content = {};

            content.timeZone = document.getElementById('hdnServerDateTime').value;
            $.ajax({
                type: "POST",
                url: "frmCTMMedExInfoHdrDtl.aspx/GetServerTime",
                data: JSON.stringify(content),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    curr_Date = msg.d.split(" ")[0].trim();
                    document.getElementById(CtrlId).value = curr_Date;
                }
            });
            //Ended by Aaditya
            //Added By Vimal Ghoniya
            var TargetMedExCodeArray = TargetMedExCode.split(','), MedExCode = "";
            for (var count = 0; count < TargetMedExCodeArray.length; count++) {
                MedExCode = TargetMedExCodeArray[count];
                if (MedExCode != undefined && MedExCode != "") {
                    if (MedExCode.split("#")[2].toUpperCase().indexOf('[' + document.getElementById(CtrlId).value.toUpperCase() + ']') != -1) {
                        if (MedExCode.split("#")[1].toLowerCase() === "radio" ||
                            MedExCode.split("#")[1].toLowerCase() === "checkbox") {
                            $("#" + MedExCode.split("#")[0] + ' *').removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0] + ' *').val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "file") {
                            $("#FU" + MedExCode.split("#")[0]).removeAttr("disabled");
                            $("#FU" + MedExCode.split("#")[0]).val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "comboglobaldictionary") {
                            $("#" + MedExCode.split("#")[0]).siblings().removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }
                        else {
                            $("#" + MedExCode.split("#")[0]).removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).siblings().removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }

                    }
                    else {
                        if (MedExCode.split("#")[1].toLowerCase() === "radio" ||
                            MedExCode.split("#")[1].toLowerCase() === "checkbox") {
                            $("#" + MedExCode.split("#")[0] + ' *').attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0] + ' *').removeAttr('checked');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "file") {
                            $("#FU" + MedExCode.split("#")[0]).attr("disabled", "disabled");
                            $("#FU" + MedExCode.split("#")[0]).val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "comboglobaldictionary") {
                            $("#" + MedExCode.split("#")[0]).siblings().attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }
                        else {
                            $("#" + MedExCode.split("#")[0]).attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).siblings().attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).val('');

                        }
                    }
                }
            }
            //End

            return false;
        }

        function GetCurrentTime(CtrlId, TargetMedExCode) {
            var curr_Time;
            var curr_hour;
            var curr_min;
            var TimeFormat;
            var content = {};

            //Added By Dipen Shah on 2-Dec-2014 for get a server time on auto time .
            content.timeZone = document.getElementById('HFServerTime').value;
            $.ajax({
                type: "POST",
                url: "frmCTMMedExInfoHdrDtl.aspx/GetServerTime",
                data: JSON.stringify(content),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    //                        $("#Result").text(msg.d);
                    //                          alert(msg.d);

                    curr_hour = msg.d.split(" ")[1].split(":")[0];
                    curr_min = msg.d.split(" ")[1].split(":")[1];
                    //if (msg.d.split(" ")[2] == "PM") {
                    //    curr_hour = Number(curr_hour) + 12;
                    //}

                    if (curr_hour.toString().length < 2) {
                        curr_hour = '0' + curr_hour;
                    }
                    if (curr_min.toString().length < 2) {
                        curr_min = '0' + curr_min;
                    }
                    curr_Time = curr_hour + ":" + curr_min;
                    document.getElementById(CtrlId).value = curr_Time
                }
            });

            //Added By Vimal Ghoniya
            var TargetMedExCodeArray = TargetMedExCode.split(','), MedExCode = "";
            for (var count = 0; count < TargetMedExCodeArray.length; count++) {
                MedExCode = TargetMedExCodeArray[count];
                if (MedExCode != undefined && MedExCode != "") {
                    if (MedExCode.split("#")[2].toUpperCase().indexOf('[' + document.getElementById(CtrlId).value.toUpperCase() + ']') != -1) {
                        if (MedExCode.split("#")[1].toLowerCase() === "radio" ||
                            MedExCode.split("#")[1].toLowerCase() === "checkbox") {
                            $("#" + MedExCode.split("#")[0] + ' *').removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0] + ' *').val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "file") {
                            $("#FU" + MedExCode.split("#")[0]).removeAttr("disabled");
                            $("#FU" + MedExCode.split("#")[0]).val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "comboglobaldictionary") {
                            $("#" + MedExCode.split("#")[0]).siblings().removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }
                        else {
                            $("#" + MedExCode.split("#")[0]).removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).siblings().removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }

                    }
                    else {
                        if (MedExCode.split("#")[1].toLowerCase() === "radio" ||
                            MedExCode.split("#")[1].toLowerCase() === "checkbox") {
                            $("#" + MedExCode.split("#")[0] + ' *').attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0] + ' *').removeAttr('checked');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "file") {
                            $("#FU" + MedExCode.split("#")[0]).attr("disabled", "disabled");
                            $("#FU" + MedExCode.split("#")[0]).val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "comboglobaldictionary") {
                            $("#" + MedExCode.split("#")[0]).siblings().attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }
                        else {
                            $("#" + MedExCode.split("#")[0]).attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).siblings().attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).val('');

                        }
                    }
                }
            }
            //End

            return false;
        }
        function pad(str, max) {
            str = str.toString();
            return str.length < max ? pad("0" + str, max) : str;
        }

        function GetCurrentStandardDate(CtrlId, TargetMedExCode) {
            var curr_Date;
            var content = {};
            var curr_Time;
            var curr_hour;
            var curr_min;
            var TimeFormat;
            var content = {};

            content.timeZone = document.getElementById('hdnServerDateTime').value;
            $.ajax({
                type: "POST",
                url: "frmCTMMedExInfoHdrDtl.aspx/GetServerTime",
                data: JSON.stringify(content),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async:false,
                success: function (msg) {
                    curr_Date = msg.d.split(" ")[0].trim();
                    var month = pad(ConvertMonthToInt(curr_Date.split("-")[1].toUpperCase()), 2);
                    
                    $("#" + CtrlId + "_1").val(curr_Date.split("-")[0].toString());
                    $("#" + CtrlId + "_2").val(month.toString());
                    $("#" + CtrlId + "_3").val(curr_Date.split("-")[2].toString());

                    curr_hour = msg.d.split(" ")[1].split(":")[0];
                    curr_min = msg.d.split(" ")[1].split(":")[1];
                    if (curr_hour.toString().length < 2) {
                        curr_hour = '0' + curr_hour;
                    }
                    if (curr_min.toString().length < 2) {
                        curr_min = '0' + curr_min;
                    }
                    curr_Time = curr_hour + ":" + curr_min;
                    $("#" + CtrlId + "_4").val(curr_hour.toString());
                    $("#" + CtrlId + "_5").val(curr_min.toString());
                    return false;
                }
            });
           
            
            //Ended by Aaditya
            //Added By Vimal Ghoniya
            var TargetMedExCodeArray = TargetMedExCode.split(','), MedExCode = "";
            for (var count = 0; count < TargetMedExCodeArray.length; count++) {
                MedExCode = TargetMedExCodeArray[count];
                if (MedExCode != undefined && MedExCode != "") {
                    if (MedExCode.split("#")[2].toUpperCase().indexOf('[' + document.getElementById(CtrlId).value.toUpperCase() + ']') != -1) {
                        if (MedExCode.split("#")[1].toLowerCase() === "radio" ||
                            MedExCode.split("#")[1].toLowerCase() === "checkbox") {
                            $("#" + MedExCode.split("#")[0] + ' *').removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0] + ' *').val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "file") {
                            $("#FU" + MedExCode.split("#")[0]).removeAttr("disabled");
                            $("#FU" + MedExCode.split("#")[0]).val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "comboglobaldictionary") {
                            $("#" + MedExCode.split("#")[0]).siblings().removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }
                        else {
                            $("#" + MedExCode.split("#")[0]).removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).siblings().removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }

                    }
                    else {
                        if (MedExCode.split("#")[1].toLowerCase() === "radio" ||
                            MedExCode.split("#")[1].toLowerCase() === "checkbox") {
                            $("#" + MedExCode.split("#")[0] + ' *').attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0] + ' *').removeAttr('checked');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "file") {
                            $("#FU" + MedExCode.split("#")[0]).attr("disabled", "disabled");
                            $("#FU" + MedExCode.split("#")[0]).val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "comboglobaldictionary") {
                            $("#" + MedExCode.split("#")[0]).siblings().attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }
                        else {
                            $("#" + MedExCode.split("#")[0]).attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).siblings().attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).val('');

                        }
                    }
                }
            }
            //End

            return false;
        }

        function ClearVal(CtrlId, TargetMedExCode) {
            document.getElementById(CtrlId).value = '';

            //Added By Vimal Ghoniya
            var TargetMedExCodeArray = TargetMedExCode.split(','), MedExCode = "";
            for (var count = 0; count < TargetMedExCodeArray.length; count++) {
                MedExCode = TargetMedExCodeArray[count];
                if (MedExCode != undefined && MedExCode != "") {
                    if (MedExCode.split("#")[2].toUpperCase().indexOf('[' + document.getElementById(CtrlId).value.toUpperCase() + ']') != -1) {
                        if (MedExCode.split("#")[1].toLowerCase() === "radio" ||
                            MedExCode.split("#")[1].toLowerCase() === "checkbox") {
                            $("#" + MedExCode.split("#")[0] + ' *').removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0] + ' *').val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "file") {
                            $("#FU" + MedExCode.split("#")[0]).removeAttr("disabled");
                            $("#FU" + MedExCode.split("#")[0]).val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "comboglobaldictionary") {
                            $("#" + MedExCode.split("#")[0]).siblings().removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }
                        else {
                            $("#" + MedExCode.split("#")[0]).removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).siblings().removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }

                    }
                    else {
                        if (MedExCode.split("#")[1].toLowerCase() === "radio" ||
                            MedExCode.split("#")[1].toLowerCase() === "checkbox") {
                            $("#" + MedExCode.split("#")[0] + ' *').attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0] + ' *').removeAttr('checked');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "file") {
                            $("#FU" + MedExCode.split("#")[0]).attr("disabled", "disabled");
                            $("#FU" + MedExCode.split("#")[0]).val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "comboglobaldictionary") {
                            $("#" + MedExCode.split("#")[0]).siblings().attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }
                        else {
                            $("#" + MedExCode.split("#")[0]).attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).siblings().attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).val('');

                        }
                    }
                }
            }
            //End

            return false;
        }

        function ClearStandardDateVal(CtrlId, TargetMedExCode) {
            $("#" + CtrlId + "_1").val("DD");
            $("#" + CtrlId + "_2").val("MMM");
            $("#" + CtrlId + "_3").val("YYYY");
            $("#" + CtrlId + "_4").val("HH");
            $("#" + CtrlId + "_5").val("MM");

            
            //Added By Vimal Ghoniya
            var TargetMedExCodeArray = TargetMedExCode.split(','), MedExCode = "";
            for (var count = 0; count < TargetMedExCodeArray.length; count++) {
                MedExCode = TargetMedExCodeArray[count];
                if (MedExCode != undefined && MedExCode != "") {
                    if (MedExCode.split("#")[2].toUpperCase().indexOf('[' + document.getElementById(CtrlId).value.toUpperCase() + ']') != -1) {
                        if (MedExCode.split("#")[1].toLowerCase() === "radio" ||
                            MedExCode.split("#")[1].toLowerCase() === "checkbox") {
                            $("#" + MedExCode.split("#")[0] + ' *').removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0] + ' *').val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "file") {
                            $("#FU" + MedExCode.split("#")[0]).removeAttr("disabled");
                            $("#FU" + MedExCode.split("#")[0]).val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "comboglobaldictionary") {
                            $("#" + MedExCode.split("#")[0]).siblings().removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }
                        else {
                            $("#" + MedExCode.split("#")[0]).removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).siblings().removeAttr("disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }

                    }
                    else {
                        if (MedExCode.split("#")[1].toLowerCase() === "radio" ||
                            MedExCode.split("#")[1].toLowerCase() === "checkbox") {
                            $("#" + MedExCode.split("#")[0] + ' *').attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0] + ' *').removeAttr('checked');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "file") {
                            $("#FU" + MedExCode.split("#")[0]).attr("disabled", "disabled");
                            $("#FU" + MedExCode.split("#")[0]).val('');
                        }
                        else if (MedExCode.split("#")[1].toLowerCase() === "comboglobaldictionary") {
                            $("#" + MedExCode.split("#")[0]).siblings().attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).val('');
                        }
                        else {
                            $("#" + MedExCode.split("#")[0]).attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).siblings().attr("disabled", "disabled");
                            $("#" + MedExCode.split("#")[0]).val('');

                        }
                    }
                }
            }
            //End

            return false;
        }

        function BindTrigger(ctrlId, eventName) {
            TriggerFlag = false;  //Modify by ketan true
            $('#' + ctrlId).trigger(eventName);
        }

        function Cblenable(ctrlId) {
            $("#" + ctrlId + ' *').attr("disabled", "disabled");
        }

        function updateCountdown(strFire) {
            var remaining, ControlCollection = {};
            if (strFire === "load") {
                ControlCollection = $('.crfentrycontrol');
                for (var cnt = 0; cnt < ControlCollection.length; cnt++) {
                    remaining = 3000 - ($(ControlCollection[cnt]).val().length);
                    $($(ControlCollection[cnt]).closest('td')).find('.CntTextArea').text(remaining + ' characters remaining.');
                }
            }
            else if (strFire.type === "keyup" || strFire.type === "change") {
                remaining = 3000 - $(this).val().length;
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

        function GetQueryStringParams(sParam) {
            var sURLVariables = window.location.href.split('&');
            for (var i = 0; i < sURLVariables.length; i++) {
                var sParameterName = sURLVariables[i].split('=');
                if (sParameterName[0] == sParam)
                    return sParameterName[1];
            }
        }

    </script>

</head>
<body style="margin-left: 0; margin-top: 0;">
    <form id="form1" runat="server" method="post">
        <asp:hiddenfield id="hdnCookiesCurrentProfile" runat="server" value="0" />
        <input style="display: none" type="text" name="fakeusernameremembered" />
        <input style="display: none" type="password" name="fakepasswordremembered" />
        <div id="divThemeSelection"  onChange="hideDiv()" class="themeSelection" style="display:none;" >
                          <table>
                              <tr>
                                  <td>
                                      <h4 style="text-align:center">Select Theme</h4>
                                       <hr style="width:133%;float:none;"/>
                                  </td>
                              </tr>
                              <tr>
                                  <td>
                            <table style="margin-left:82px;">                            
                             <tr>
                                 <td style="width:20%;text-align:right;">
                                     <label id="lblornage" class="ThemeLable" style="background-color:#CF8E4C;" ></label>
                                 </td>
                                 <td style="text-align:left;">
                                     <label id="Label1" class="ThemeLable" style="background-color:#1560a1;" ></label>
                                 </td>
                             </tr>
                             <tr>
                                 <td style="width:20%;text-align:right;">
                                     <label id="Label2" class="ThemeLable" style="background-color:#33a047;"></label>
                                 </td>
                                 <td style="text-align:left;">
                                     <label id="lblDemo" class="ThemeLable" style="background-color:#999966;"></label>
                                 </td>
                             </tr>
                                   </table>
                                  </td>
                              </tr>
                          </table>                      
        </div>
        <div align="center">
            <center>
                <asp:scriptmanager id="ScriptManager1" runat="server" asyncpostbacktimeout="1000"
                    enablepagemethods="True" ScriptMode="Release">
                <Services>
                    <asp:ServiceReference Path="AutoComplete.asmx" />
                </Services>
            </asp:scriptmanager>
                <asp:hiddenfield id="hdnuserprofile" runat="server" value="0" />
                <asp:hiddenfield id="hdnLocked" runat="server" value="0" />
                <asp:hiddenfield id="hdnOldMedExType" runat="server" />
                <asp:hiddenfield id="HFMedexType" runat="server" />
                <asp:hiddenfield id="hdnOldMedExValue" runat="server" />
                <table border="0" cellspacing="0" style="border-collapse: collapse; border-color: #111111;"
                    width="100%" id="AutoNumber1" cellpadding="0">
                    <tr style="height: 65px">
                        <td style="vertical-align: bottom; width: 100%; height: 65px; text-align: left;">
                           <div style="background-image: url(images/left1.jpg); background-repeat: repeat-x; width: 100%; height: 65px;">
                        <div style=" padding:5px; position:absolute;z-index:999;">
                            <img src="Images/biznet-logo.png" alt="biznet logo left" width="60" />
                        </div>
                        <div style="float:right; width:100%;">
						    <div id="qodef-particles" class="fixed" style="height: 65px; background-color: #d0e2ea;" data-particles-density="high" 
                                    data-particles-color="#ff3d6d" data-particles-opacity="0.8" data-particles-size="3" data-speed="3" data-show-lines="yes" 
                                    data-line-length="100" data-hover="yes" data-click="yes">
                                <div id="qodef-p-particles-container" style="height:65px;">
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
                                <td style="white-space: nowrap;float:left">
                                    <div id="DivSessionTimingWatch" class="SessionTiming">
                                        <asp:Label ID="lblTime" runat="server" CssClass="Label headerusername" Visible="false"/>
                                    <asp:Label runat="server" ID="lblSessionTimeCap" CssClass="Label" Style="color: Black; font-size:15px; width:14px;font-weight:bold;"
                                        Text="Session Expires In: "></asp:Label>
                                    <b><span class="Label headerusername" id="timerText"></span></b>                                            
                                    <asp:HiddenField ID="HDSessionValue" runat="server" Value='<%=Session("UserId")%>' />
                                    </div>
                                </td>
                            </tr>
                            <div class="Manage">
                                <span id="lblWelcome" class="Label" style="color: #000;font-size:15px;">Welcome :</span>
                                <asp:Label ID="lblUserName" runat="server" CssClass="Label headerusername"/>
                            </div> 
                        </table>
                    </div>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td align="center" style="width: 95%; padding: 2%;">
                            <div align="center">
                                <table style="" width="100%" cellpadding="0">
                                    <tr>
                                        <td align="center" style="width: 100%">
                                            <asp:panel id="Pan_Hdr" runat="server" width="100%" cssclass="InnerTable" backcolor="White">
                                            <asp:Panel ID="Pan_Child" runat="server" BackColor="Window" Width="100%">
                                                <div style="text-align: center" id="Header Label" class="Div" align="center">
                                                    <table style="width: 100%" cellpadding="0px" cellspacing="0px">
                                                        <tr>
                                                            <td>
                                                                <fieldset class="FieldSetBox" id="ProjectFieldSet" style="width: 90%; border-radius: 5Px; padding-bottom: 0%;
                                                                    float: left; padding-left: 4%; padding-right: 3%; margin-left: 0.2%;">
                                                                    <legend class="LegendText" style="color: Black; text-align: left;">Project Information</legend>
                                                                    <table style="width: 100%; padding-left: 0%; padding-right: 0%; padding-bottom: 1%;">
                                                                        <tr align="center">
                                                                            <td align="center" width="100%">
                                                                                <strong style="font-weight: bold; font-size: 20px"></strong>
                                                                                <table style="width: 90%">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td style="width: 100%" align="center">
                                                                                                <asp:Label ID="lblHeader" runat="server" SkinID="lblHeading"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                                <hr style="background-image: none; width: 100%; color: #ffcc66; background-color: #ffcc66" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="VersionDtl" class="Label" runat="server" style="display: none; font-size: small;
                                                                            font-weight: bold;">
                                                                            <td align="center">
                                                                                version :<asp:Label runat="server" ID="VersionNo" Style="padding-right: 10px"></asp:Label>Version
                                                                                Date :<asp:Label ID="VersionDate" Style="padding-right: 10px;" runat="server"></asp:Label>
                                                                                Status:<img src="images/Freeze.jpg" id="ImageLockUnlock" runat="server" alt="Lock" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr align="center">
                                                                            <td align="left" width="100%">
                                                                                <table width="100%" cellpadding="0px" cellspacing="0px">
                                                                                    <tbody>
                                                                                        <tr id="Tr1" runat="server">
                                                                                            <td style="text-align: left; white-space: nowrap; width: 40%;">
                                                                                                <asp:DropDownList ID="ddlActivities" runat="server" Font-Bold="true" Font-Size="Small"
                                                                                                    CssClass="dropDownList " AutoPostBack="True" Width="87%" onchange="closewindow('COMBO',this);">
                                                                                                </asp:DropDownList>
                                                                                                <asp:Label ID="lblLastReviewedBy" runat="server"></asp:Label>
                                                                                            </td>
                                                                                            <td style="text-align: right; width: 30%; padding-right: 0%;">
                                                                                                <table width="100%">
                                                                                                    <tr>
                                                                                                        <td style="text-align: right;">
                                                                                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                                                    <ContentTemplate>
                                                                                                            <asp:ImageButton ID="imgbtnShowQuery" AlternateText="Show Query" ImageUrl="~/Images/showQuery.png"
                                                                                                                runat="server" Style="display: none;" ToolTip="Show Query" />
                                                                                                            <asp:Button Visible="false" ID="btnShowquery" runat="server" Text="Show Queries"
                                                                                                                CssClass="button" Width="100px" OnClientClick="return AnyDivShowHide('DIVQUERIESSHOW');">
                                                                                                            </asp:Button>
                                                                                                         <asp:ImageButton Visible="false" ID="btnReviewHistory" ImageUrl="~/Images/Review_Histroy.png"
                                                                                                                runat="server" ToolTip="Review history" />
                                                                                                        </ContentTemplate>
                                                                                                                </asp:UpdatePanel>
                                                                                                           
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td style="text-align: right;">
                                                                                                            <asp:Label ID="lblNoOfQuery" runat="server" Style="display: none; color: Red;" Text="Query:(0)"
                                                                                                                CssClass="LegendText"></asp:Label>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td style="text-align: right;">
                                                                                                            <img id="imgViewDocument" src="Images/ViewDocumnent.png" alt="View Activity Document" onclick="viewProjectDocument(this)" runat="server" title="View Activity Document" />
                                                                                                            <img id="imgDicomViewer" src="Images/ViewDicom.png" alt="View Dicom" onclick="viewDicom()" runat="server" title="View Dicom" />
                                                                                                             <a id="btnQuery" title="Query Details" onclick="QueryDetailsFn();"><i class="fa fa-comments-o" style="font-size:30px; color:slateblue""></i></a>
                                                                                                            <asp:HiddenField ID="hdnActivityName" runat="server" Value="" />
                                                                                                            <asp:HiddenField ID="hdniUserId" runat="server" Value="" />
                                                                                                            <asp:HiddenField ID="hdniUserIP" runat="server" Value="" />
                                                                                                            <asp:HiddenField ID="DISoftURL" runat="server" Value="" />
                                                                                                            <img id="imgActivityLegends" src="images/question.gif" alt="Activity Legends" runat="server"
                                                                                                                title="Activity Legends" />
                                                                                                            <img id="imgShow" src="images/question.gif" alt="Question" runat="server" title="Image Legends" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="width: 57%" class="Label" align="left">
                                                                                                <asp:DropDownList ID="ddlRepeatNo" multiple="multiple" style="width:72%;" runat="server" CssClass="Label" onchange="closewindow('COMBO',this);"
                                                                                                    AutoPostBack="true">
                                                                                                </asp:DropDownList>
                                                                                                <asp:button ID="btnRepeatGo" runat="server" Text="" CssClass="btn btngo" OnClientClick="return getSelectedItem();" style="height:23px" />
                                                                                                <asp:ImageButton ID="imgViewAll" ImageUrl="~/images/ViewAll.png" runat="server" Style="margin-top: 0.2em; display: none" title="All Repetition" />                                                                                                
                                                                                                <asp:Button ID="btnGridViewDisplay" visible="false" runat="server" Text="Grid View" onClientClick="return IdentifyGridViewButton();" CssClass="btn btnnew" style="height:22px;"/>
                                                                                            </td>
                                                                                            <td id="Tr2" runat="server" style="width: 60%; white-space: nowrap;" class="Label"
                                                                                                valign="middle" align="right">
                                                                                                <fieldset style="display: none; font-size: 7pt; height: auto; text-align: left" id="divActivityLegends"
                                                                                                    runat="server">
                                                                                                    <div>
                                                                                                        <asp:PlaceHolder ID="PhlReviewer" runat="server" EnableViewState="true"></asp:PlaceHolder>
                                                                                                        <%--<asp:Label ID="lblRed" runat="Server" BackColor="red">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:Label>-Data
                                                                                                        Entry Pending,
                                                                                                        <asp:Label ID="lblOrange" runat="Server" BackColor="orange">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:Label>-Data
                                                                                                        Entry Continue,
                                                                                                        <asp:Label ID="lblBlue" runat="Server" BackColor="blue">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:Label>-Ready
                                                                                                        For Review,
                                                                                                        <br />
                                                                                                        <br />
                                                                                                        <asp:Label ID="lblYellow" runat="Server" BackColor="#50C000">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:Label>-First
                                                                                                        Review Done,
                                                                                                        <asp:Label ID="lblGreen" runat="Server" BackColor="#006000">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:Label>-Second
                                                                                                        Review Done,
                                                                                                        <asp:Label ID="lblGray" runat="Server" BackColor="gray">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:Label>-Reviewed
                                                                                                        & Freeze--%>
                                                                                                    </div>
                                                                                                </fieldset>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="width: 40%;">
                                                                                            </td>
                                                                                            <td style="width: 60%; text-align: right; white-space: nowrap" class="Label" id="trimageinformation"
                                                                                                runat="server">
                                                                                                <fieldset if="fImageLegends" style="display: none; height: 20px; font-size: 7pt;
                                                                                                    vertical-align: top" id="canal" runat="server">
                                                                                                    <div style="float: right">
                                                                                                        <img id="imgedit" src="~/images/Update_Small.png" alt="Edit" runat="server" />&nbsp;Edit
                            
                                                                                                                                                                                    Field Value &nbsp;&nbsp;&nbsp;
                                                                                                        <img id="imgupdate" src="~/Images/Edit_Small.png" alt="Update" runat="server" />&nbsp;Update
                                                                                                        &nbsp;&nbsp;&nbsp;
                                                                                                        <img id="imgdiscrepancy" src="images/Discrepancy_Small.png" alt="dicrepency" runat="server" />&nbsp;Discrepancy&nbsp;&nbsp;&nbsp;
                                                                                                        <img id="imgaudittrail" src="Images/Audit_Small.png" alt="AuditTrail" runat="server" />&nbsp;Audit
                                                                                                        trail
                                                                                                         <img id="imgEditCheckQuery" src="Images/EditCheckQuery_Small.png" alt="AuditTrail" runat="server" />&nbsp;Edit Check Query
                                                                                                    </div>
                                                                                                </fieldset>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="width: 40%; text-align: left; white-space: nowrap;" class="Label" id="trReviewCompleted"
                                                                                                runat="server">
                                                                                                <fieldset style="width: 84%; margin-top: 4%; height: 26px; text-align: left;">
                                                                                                    <asp:CheckBox ID="chkReviewCompleted" runat="server" Text="Review Completed" ToolTip="Review Completed">
                                                                                                    </asp:CheckBox>
                                                                                                    <asp:Button ID="btnOk" runat="server" Text="Ok" ToolTip="Ok" CssClass="btn btnsave" OnClientClick="return ValidateReview();" />
                                                                                                    <asp:CheckBox ID="chkReviewAll" runat="server" Text="Review All" />
                                                                                                </fieldset>
                                                                                            </td>
                                                                                            <td style="width: 60%">
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </fieldset>
                                                            </td>
                                                        </tr>
                                                        <tr align="center">
                                                            <td align="left" width="100%">
                                                                <table width="100%" cellpadding="0px" cellspacing="0px">
                                                                    <tbody>
                                                                        <tr id="secound">
                                                                            <td align="left" colspan="2">
                                                                                <table width="100%" cellpadding="1px" cellspacing="0px"">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td valign="top" align="left" style="width: 100%">
                                                                                                <asp:UpdatePanel ID="UpPlaceHolder" runat="server" ChildrenAsTriggers="true">
                                                                                                    <ContentTemplate>
                                                                                                        <asp:Panel ID="PnlPlaceMedex" runat="server" Width="100%">
                                                                                                            <asp:PlaceHolder ID="PlaceMedEx" runat="server" EnableViewState="False"></asp:PlaceHolder>
                                                                                                        </asp:Panel>
                                                                                                        <%--<button id="BtnForAuditTrail" runat="server" style="display: none;" />
                                                                                                        <cc1:ModalPopupExtender ID="MpeAuditTrail" runat="server" PopupControlID="divHistoryDtl"
                                                                                                            PopupDragHandleControlID="lblMedexDescription" BackgroundCssClass="modalBackground"
                                                                                                            TargetControlID="BtnForAuditTrail" CancelControlID="ImgPopUpCloseAuditTrail">
                                                                                                        </cc1:ModalPopupExtender>--%>
                                                                                                        <%--<div style="display: none; width: 90%; margin: auto; max-height: 250px; text-align: left"
                                                                                                            class="divModalPopup" runat="server" id="divHistoryDtl">--%>
                                                                                                        <div style="display: none; text-align: left; width:1200px;max-height:400px; overflow:auto;"
                                                                                                            class="divModalPopup" runat="server" id="divHistoryDtl">
                                                                                                            <table style="width: 100%">
                                                                                                                <tbody>
                                                                                                                    <tr>
                                                                                                                        <td style="font-size: 14px;" class="Label">
                                                                                                                            <img id="ImgPopUpCloseAuditTrail" alt="Close" src="images/Sqclose.gif" style="position: relative;
                                                                                                                                float: right; right: 27px;" onclick="funCloseDiv('divHistoryDtl');" />
                                                                                                                            History Of Attribute :
                                                                                                                            <asp:Label ID="lblMedexDescription" runat="server"></asp:Label>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                            <asp:Panel ID="pnlHistoryDtl" runat="server" Width="90%" ScrollBars="Auto" Style="max-height: 300px;
                                                                                                                                margin: auto;">
                                                                                                                                <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66;" />
                                                                                                                                <asp:GridView ID="GVHistoryDtl" runat="server" AutoGenerateColumns="False" Width="100%" >
                                                                                                                                    <Columns>
                                                                                                                                        <asp:BoundField DataField="iTranNo" HeaderText="Sr. No." />
                                                                                                                                        <asp:BoundField DataField="vMedExResult" HeaderText="Value" />
                                                                                                                                        <asp:BoundField DataField="vModificationRemark" HeaderText="Reason" />
                                                                                                                                        <asp:BoundField DataField="CRFSubDtlChangedBy" HeaderText="Modify By">
                                                                                                                                            <ItemStyle Wrap="False" />
                                                                                                                                        </asp:BoundField>
                                                                                                                                        <asp:BoundField DataField="dModifyOnSubDtl_IST" HeaderText="Modify On" DataFormatString="{0:dd-MMM-yyyy HH:mm:ss tt}">
                                                                                                                                            <ItemStyle Wrap="False" />
                                                                                                                                        </asp:BoundField>
                                                                                                                                    </Columns>
                                                                                                                                </asp:GridView>
                                                                                                                            </asp:Panel>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </tbody>
                                                                                                            </table>
                                                                                                        </div>
                                                                                                        <%--<button id="BtnForEditAttribute" runat="server" style="display: none;" />
                                                                                                        <cc1:ModalPopupExtender ID="MpeEditAttribute" runat="server" PopupControlID="divForEditAttribute"
                                                                                                            PopupDragHandleControlID="lblReasonForEdit" BackgroundCssClass="modalBackground"
                                                                                                            TargetControlID="BtnForEditAttribute" CancelControlID="ImgPopUpCloseEditAttribute"
                                                                                                            BehaviorID="MpeEditAttribute">
                                                                                                        </cc1:ModalPopupExtender>--%>
                                                                                                        <div style="display: none; width: 40%; max-height: 200px; text-align: left; margin: auto;
                                                                                                            left: 30% !important;" class="divModalPopup" id="divForEditAttribute" runat="server">
                                                                                                            <table style="width: 100%; margin-bottom: 2%">
                                                                                                                <tr>
                                                                                                                    <td colspan="2" style="font-size: 14px;" class="Label">
                                                                                                                        <img id="ImgPopUpCloseEditAttribute" alt="Close" src="images/Sqclose.gif" style="position: relative;
                                                                                                                            float: right; right: 5px;" onclick="funCloseDiv('divForEditAttribute');" />
                                                                                                                        Reason For Edit
                                                                                                                        <hr />
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td class="Label" style="text-align: right; width: 30%">
                                                                                                                        <span class="Label">Reason* :</span>
                                                                                                                    </td>
                                                                                                                    <td style="text-align: left; width: 70%">
                                                                                                                        <asp:DropDownList ID="DdlEditRemarks" runat="server" CssClass="dropDownList" Width="70%">
                                                                                                                        </asp:DropDownList>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td colspan="2" style="text-align: center; width: 100%">
                                                                                                                        <span class="Label">OR </span>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td class="Label" style="text-align: right; width: 30%">
                                                                                                                        <span class="Label" style="vertical-align: top;">Remark* : </span>
                                                                                                                    </td>
                                                                                                                    <td style="text-align: left; width: 70%">
                                                                                                                        <asp:TextBox ID="txtRemarkForAttributeEdit" runat="Server" Text="" TextMode="MultiLine"
                                                                                                                            CssClass="textbox" Width="70%"> </asp:TextBox>
                                                                                                                        <asp:HiddenField ID="HdReasonDesc" runat="server" Value="" />
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td colspan="2">
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td class="Label" colspan="2" style="text-align: center; width: 100%">
                                                                                                                        <asp:Button ID="btnSaveRemarksForAttribute" runat="server" CssClass="btn btnsave" OnClientClick="return ValidationForEditOrDelete();"
                                                                                                                            Text="Save" />
                                                                                                                        <input class="button" onclick="$get('ImgPopUpCloseEditAttribute').click();" type="button"
                                                                                                                            value="Close" />
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </div>
                                                                                                        <div id="ModalBackGround" runat="server" class="divModalBackGround" style="display: none;
                                                                                                            width: 100% !Important;">
                                                                                                        </div>
                                                                                                        <%--<button id="BtnForDCF" runat="server" style="display: none;" />
                                                                                                            <cc1:ModalPopupExtender ID="MpeDCF" runat="server" PopupControlID="divDCF" BehaviorID="MpeDCF"
                                                                                                                PopupDragHandleControlID="lblAttributeDCF" BackgroundCssClass="modalBackground"
                                                                                                                TargetControlID="BtnForDCF" CancelControlID="ImgPopUpCloseDCF">
                                                                                                            </cc1:ModalPopupExtender>--%>
                                                                                                        <div style="display: none; width: 90%; margin: auto; max-height: 450px; text-align: left"
                                                                                                            id="divDCF" class="divModalPopup" runat="server">
                                                                                                            <table style="width: 100%">
                                                                                                                <tbody>
                                                                                                                    <tr align="center">
                                                                                                                        <td colspan="2" style="font-size: 14px !important; text-align: center" class="Label">
                                                                                                                            <img id="ImgPopUpCloseDCF" alt="Close" src="images/Sqclose.gif" style="position: relative;
                                                                                                                                float: right; right: 5px;" onclick="funCloseDiv('divDCF');" />
                                                                                                                            Discrepancy Of Attribute :
                                                                                                                            <asp:Label ID="lblAttributeDCF" runat="server"></asp:Label>
                                                                                                                            <hr style="margin-top: 25px;"/>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>

                                                                                                                        <td align="right" style="text-align: right">
                                                                                                                            Remarks For Discrepancy :
                                                                                                                        </td>
                                                                                                                        <td style="text-align: left">
                                                                                                                            <asp:TextBox ID="txtDiscrepancyRemarks" runat="Server" Text="" CssClass="textBox"  onKeyPress ="Value()"
                                                                                                                                Width="226px" TextMode="MultiLine" Height="59px"></asp:TextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td align="right" style="text-align: right">
                                                                                                                            Status :
                                                                                                                        </td>
                                                                                                                        <td class="Label" align="left" style="text-align: left">
                                                                                                                            <asp:DropDownList ID="ddlDiscrepancyStatus" runat="server" CssClass="Label">
                                                                                                                                <asp:ListItem Text="Generated" Value="N" Selected="True"></asp:ListItem>
                                                                                                                                <asp:ListItem Text="Answered" Value="O"></asp:ListItem>
                                                                                                                                <asp:ListItem Text="Resolved" Value="R"></asp:ListItem>
                                                                                                                                <asp:ListItem Text="Internally Resolved" Value="I"></asp:ListItem>
                                                                                                                            </asp:DropDownList>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                        </td>
                                                                                                                        <td style="text-align: left" align="right">
                                                                                                                            <asp:Button ID="btnSaveDiscrepancy" runat="server" Text="Add" CssClass="btn btnnew" OnClientClick="return ValidationForDiscrepancy();">
                                                                                                                            </asp:Button>
                                                                                                                            <asp:Button ID="btnUpdateDiscrepancy" runat="server" Text="Update" CssClass="button"
                                                                                                                                OnClientClick="return ValidationForDiscrepancy();" Width="133px"></asp:Button>
                                                                                                                            <input type="button" value="Close" class="btn btnclose" onclick="$get('ImgPopUpCloseDCF').click();" />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td style="max-height: 260px" colspan="2">
                                                                                                                            <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                                                                                                                            <asp:Panel ID="pnlDCFGrid" runat="server" Width="95%" ScrollBars="Auto" Style="max-height: 250px; max-width:1100px; 
                                                                                                                                margin: auto">
                                                                                                                                <asp:GridView ID="GVWDCF" runat="server" AutoGenerateColumns="False"  Width="100%" >
                                                                                                                                    <Columns>
                                                                                                                                        <asp:BoundField DataField="nDCFNo" HeaderText="DCFNo"></asp:BoundField>
                                                                                                                                        <asp:BoundField DataField="nCRFDtlNo" HeaderText="CRFDtlNo"></asp:BoundField>
                                                                                                                                        <asp:BoundField DataField="iSrNo" HeaderText="SrNo"></asp:BoundField>
                                                                                                                                        <asp:BoundField DataField="vMedExCode" HeaderText="MedExCode"></asp:BoundField>
                                                                                                                                        <asp:BoundField DataField="cDCFType" HeaderText="DCF Type">
                                                                                                                                            <ItemStyle Wrap="False" />
                                                                                                                                        </asp:BoundField>
                                                                                                                                        <asp:BoundField DataField="iDCFBy" HeaderText="iDCFBy" />
                                                                                                                                        <asp:BoundField DataField="vCreatedBy" HeaderText="CreatedBy" />
                                                                                                                                        <asp:BoundField DataField="dDCFDate_IST" HeaderText="DCF Date" HtmlEncode="False">
                                                                                                                                            <ItemStyle Wrap="False" />
                                                                                                                                        </asp:BoundField>
                                                                                                                                        <asp:BoundField DataField="vDiscrepancy" HeaderText="Discrepancy" />
                                                                                                                                       <%-- <asp:BoundField DataField="vSourceResponse" HeaderText="Remarks" >
                                                                                                                                        </asp:BoundField>--%>
                                                                                                                                        <asp:TemplateField HeaderText="DCF Query">
                                                                                                                                            <ItemTemplate>
                                                                                                                                                <asp:TextBox ID="txtRemarks" TextMode="MultiLine" Readonly="true"  Style="width:300px; height:30px; "   runat="server" Text='<%# Bind("vSourceResponse")%>'>                                                                                                                                               
                                                                                                                                                </asp:TextBox>
                                                                                                                                            </ItemTemplate>
                                                                                                                                        </asp:TemplateField>          
                                                                                                                                        <asp:boundField DataField="vUpdateRemarks" HeaderText="Remarks"></asp:boundField>                                                                                                                            
                                                                                                                                        <asp:BoundField DataField="cDCFStatus" HeaderText="Status"></asp:BoundField>
                                                                                                                                        <asp:BoundField DataField="vUpdatedBy" HeaderText="Updated By"></asp:BoundField>
                                                                                                                                        <asp:BoundField DataField="dStatusChangedOn_IST" HeaderText="Updated On" HtmlEncode="False">
                                                                                                                                            <ItemStyle Wrap="False" />
                                                                                                                                        </asp:BoundField>                                                                                                                                       
                                                                                                                                        <asp:TemplateField HeaderText="Update">
                                                                                                                                            <ItemTemplate>
                                                                                                                                                <asp:LinkButton ID="lnkbtnUpdate" runat="server" CssClass="LinkButton" Text="Update"
                                                                                                                                                   Style="white-space: nowrap" ></asp:LinkButton>
                                                                                                                                            </ItemTemplate>
                                                                                                                                        </asp:TemplateField>                                                                                                                                      
                                                                                                                                        <asp:BoundField DataField="vUserTypeCode" HeaderText="UserTypeCode" />
                                                                                                                                        <%-- <asp:TemplateField HeaderText="Internally Resolved">
                                                                                                                                                <ItemTemplate>
                                                                                                                                                    <asp:LinkButton ID="LnkBtnInternallyResolved" runat="server" CssClass="LinkButton" Text="Internally Resolved"
                                                                                                                                                        CommandName="IR" ></asp:LinkButton>
                                                                                                                                                </ItemTemplate>
                                                                                                                                            </asp:TemplateField>--%>
                                                                                                                                    </Columns>
                                                                                                                                </asp:GridView>
                                                                                                                            </asp:Panel>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </tbody>
                                                                                                            </table>
                                                                                                        </div>
                                                                                                        <%--  <div style="display: none; width: 50%; margin-left: 22%; max-height: 450px; text-align: left"
                                                                                                            id="divDeleteRepetition" class="divModalPopup" runat="server">--%>
                                                                                                        <div style="display: none; width: 90%; margin: auto; max-height: 450px; text-align: left"
                                                                                                            id="divDeleteRepetition" class="divModalPopup" runat="server">
                                                                                                            <table style="width: 100%">
                                                                                                                <tr>
                                                                                                                    <td colspan="2" style="font-size: 14px !important; text-align: center" class="Label">
                                                                                                                        <img id="imgClose" alt="Close" src="images/Sqclose.gif" style="position: relative;
                                                                                                                            float: right; right: 5px;" tooltip="Close" onclick="funCloseDiv('divDeleteRepetition');" />
                                                                                                                        Reason For Deletion
                                                                                                                        <hr />
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td align="right" style="text-align: right">
                                                                                                                        Remarks For Deletion:
                                                                                                                    </td>
                                                                                                                    <td style="text-align: left">
                                                                                                                        <asp:TextBox ID="txtDeleteRepetition" runat="Server" Text="" CssClass="textBox" Width="226px"
                                                                                                                            TextMode="MultiLine" Height="59px"></asp:TextBox>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td colspan="2" style="height: 5px;">
                                                                                                                        <hr />
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td colspan="2" align="center">
                                                                                                                        <asp:Button ID="btnSaveDeleteRepetition" runat="server" Text="Save" CssClass="btn btnsave"
                                                                                                                            OnClientClick="return ValidationForDeleteRepetition();"></asp:Button>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td colspan="2" style="height: 5px;">
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </div>
                                                                                                        <div style="display: none; width: 90%; max-height: 450px; margin: auto; text-align: left;"
                                                                                                            id="divEditChecks" class="divModalPopup" runat="server">
                                                                                                            <table style="width: 100%; margin: auto;">
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                        <div style="font-size: 14px; width: 90%; margin: auto;" class="Label">
                                                                                                                            <img id="Img1" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right;
                                                                                                                                right: -60px;" onclick="funCloseDiv('divEditChecks');" />
                                                                                                                            Show Query
                                                                                                                            <hr />
                                                                                                                        </div>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                        <div style="max-height: 294px; width: 90%; margin: auto; overflow: auto;">
                                                                                                                            <asp:GridView ID="GDVEditcheck" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true">
                                                                                                                                <Columns>
                                                                                                                                    <asp:BoundField DataField="vQueryMessage" HeaderText="Edit Check Formula" ControlStyle-Width="100%">
                                                                                                                                        <ItemStyle  HorizontalAlign="Center" VerticalAlign="Middle" Width="100%"/>
                                                                                                                                    </asp:BoundField>
                                                                                                                                    <asp:BoundField DataField="vErrorMessage" HeaderText="Discrepancy Message"></asp:BoundField>
                                                                                                                                    <asp:BoundField DataField="RepetitionNo" HeaderText="Repeatation No"></asp:BoundField>
                                                                                                                                    <asp:BoundField DataField="cEditCheckTypeDisplay" HeaderText="Type"></asp:BoundField>
                                                                                                                                     <asp:BoundField DataField="FiredBy" HeaderText="Executed By"></asp:BoundField>
                                                                                                                                    <asp:BoundField DataField="vFiredDate" HeaderText="Executed On"></asp:BoundField>                                                                                                                                   
                                                                                                                                </Columns>
                                                                                                                            </asp:GridView>
                                                                                                                        </div>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </div>


                                                                                                        <%--Added By Vivek Patel--%>

                                                                                                         <div style="display: none; width: 90%; max-height: 450px; margin: auto; text-align: left;"
                                                                                                            id="divEditChecksQuery" class="divModalPopup" runat="server">
                                                                                                            <table style="width: 100%; margin: auto;">
                                                                                                                 <tbody>

                                                                                                                       <tr>
                                                                                                                        <td style="font-size: 14px;" class="Label">
                                                                                                                            <img id="Img5" alt="Close" src="images/Sqclose.gif" style="position: relative;
                                                                                                                                float: right; right: 5px;" onclick="funCloseDiv('divEditChecksQuery');" />
                                                                                                                                    Edit Check Query
                                                                                                                            <asp:Label ID="Label3" runat="server"></asp:Label>
                                                                                                                        </td>
                                                                                                                    </tr>                                                                                                                                                                                                                              
                                                                                                                <tr>
                                                                                                                    <td>                                                                                                                      
                                                                                                                         <asp:Panel ID="Panel1" runat="server" Width="90%" ScrollBars="Auto" Style="max-height: 220px;
                                                                                                                                margin: auto;">                                                                                                                              
                                                                                                                                    <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66;" />
                                                                                                                                    <asp:GridView ID="GDVEditcheckQuery" runat="server" AutoGenerateColumns="False" BorderColor="Peru" Font-Size="Small" OnPreRender="GDVEditcheckQuery_PreRender" ShowHeaderWhenEmpty="true" SkinID="grdViewSmlAutoSize" Width="100%">
                                                                                                                                        <Columns>
                                                                                                                                            <asp:BoundField DataField="vQueryMessage" HeaderText="Edit Check Formula">
                                                                                                                                                <ItemStyle Width="40%" />
                                                                                                                                            </asp:BoundField>
                                                                                                                                            <asp:BoundField DataField="vErrorMessage" HeaderText="Discrepancy Message"></asp:BoundField>
                                                                                                                                            <asp:BoundField DataField="RepetitionNo" HeaderText="Repeatation No"></asp:BoundField>
                                                                                                                                            <asp:BoundField DataField="cEditCheckTypeDisplay" HeaderText="Type"></asp:BoundField>
                                                                                                                                            <asp:BoundField DataField="FiredBy" HeaderText="Executed By"></asp:BoundField>
                                                                                                                                            <asp:BoundField DataField="vFiredDate" HeaderText="Executed On"></asp:BoundField>                                                                                                                                          
                                                                                                                                        </Columns>
                                                                                                                                    </asp:GridView>
                                                                                                                           </asp:Panel>
                                                                                                                    </td>
                                                                                                                </tr>                                                                                                                       
                                                                                                             </tbody>
                                                                                                            </table>
                                                                                                        </div>
                                                                                                        <%--Completed by Vivek Patel--%>
                                                                                                        
                                                                                                        <asp:HiddenField ID="HFWidth" runat="server" />
                                                                                                        <asp:HiddenField ID="HFServerTime" runat="server" />
                                                                                                        <asp:HiddenField ID="HFHeight" runat="server" />
                                                                                                        <asp:Button Style="display: none" ID="btnEdit" runat="server" Text="Edit" ToolTip="Edit"
                                                                                                            CssClass="btn btnedit" />
                                                                                                        <asp:Button Style="display: none" ID="btnDCF" runat="server" Text="DCF" ToolTip="DCF"
                                                                                                            CssClass="btn btnnew" OnClientClick="displayBackGround();" />
                                                                                                        <asp:Button Style="display: none" ID="btnAudittrail" runat="server" Text=""
                                                                                                            ToolTip="AuditTrail" CssClass="btn btnaudit" onClic="displayBackGround();" />
                                                                                                        <%--Added By Vivek Patel--%>
                                                                                                        <asp:Button Style="display: none" ID="btnEditCheckQuery" runat="server" Text="Query"
                                                                                                            ToolTip="Query" CssClass="btn btnnew"/>
                                                                                                         <%--Completed by Vivek Patel--%>
                                                                                                        <asp:Button Style="display: none" ID="btnSaveRunTime" runat="server" Text="SaveRunTime"
                                                                                                            ToolTip="SaveRunTime" CssClass="btn btnew" />
                                                                                                        <asp:Button Style="display: none" ID="btnMeddraBrowse" runat="server" Text="MedDRA Browser"
                                                                                                            ToolTip="MedDRA Browser" CssClass="btn btnnew" />
                                                                                                        <asp:Button Style="display: none" ID="btnAutoCalculate" runat="server" Text="Auto Calculate"
                                                                                                            ToolTip="Auto Calculate" CssClass="btn btnnew" OnClientClick="displayBackGround();" />
                                                                                                        <asp:Button Style="display: none" ID="BtnDosingTime" runat="server" Text="DosingTime"
                                                                                                            ToolTip="DosingTime" CssClass="btn btnnew" />
                                                                                                        <asp:Button Style="display: none" ID="btnDeleteRepetition" runat="server" Text="Delete Repetition"
                                                                                                            ToolTip="Delete" CssClass="btn btncancel" />
                                                                                                        <div style="display: none; left: 391px; width: 650px; top: 460px; height: 450px;
                                                                                                            text-align: left" id="divQueries" class="DIVSTYLE2" runat="server">
                                                                                                            <table style="width: 650px">
                                                                                                                <tbody>
                                                                                                                    <tr>
                                                                                                                        <td style="text-align: center" align="center">
                                                                                                                            <strong style="white-space: nowrap">Queries Of Activity</strong>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td style="text-align: center" align="center">
                                                                                                                            <asp:Button ID="btnCloseQuery" runat="server" Text="Close" CssClass="btn btnclose" OnClientClick="return AnyDivShowHide('DIVQUERIESHIDE');">
                                                                                                                            </asp:Button>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td style="height: 270px">
                                                                                                                            <asp:Panel ID="pnlQuery" runat="server" Width="640px" ScrollBars="Auto" Height="250px">
                                                                                                                                <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                                                                                                                                <br />
                                                                                                                                <asp:GridView ID="gvwQueries" runat="server" AutoGenerateColumns="False" BorderColor="Peru"
                                                                                                                                    Font-Size="Small" SkinID="grdViewSmlAutoSize">
                                                                                                                                    <Columns>
                                                                                                                                        <%--<asp:BoundField DataField="vQueryValue" HeaderText="Query Value" />--%>
                                                                                                                                        <asp:BoundField DataField="RepetitionNo" HeaderText="Repetition No." />
                                                                                                                                        <asp:BoundField DataField="vRemarks" HeaderText="Query Remarks" />
                                                                                                                                    </Columns>
                                                                                                                                </asp:GridView>
                                                                                                                            </asp:Panel>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </tbody>
                                                                                                            </table>
                                                                                                        </div>
                                                                                                        <asp:HiddenField ID="HFWorkspaceId" runat="server" />
                                                                                                        <asp:HiddenField ID="HFParentWorkspaceId" runat="server" />
                                                                                                        <asp:HiddenField ID="HFProjectLockStatus" runat="server" />
                                                                                                        <asp:HiddenField ID="HFType" runat="server" />
                                                                                                        <asp:HiddenField ID="HFActivityId" runat="server" />
                                                                                                        <asp:HiddenField ID="HFParentActivityId" runat="server" />
                                                                                                        <asp:HiddenField ID="HFNodeId" runat="server" />
                                                                                                        <asp:HiddenField ID="HFParentNodeId" runat="server" />
                                                                                                        <asp:HiddenField ID="HFPeriodId" runat="server" />
                                                                                                        <asp:HiddenField ID="HFSubjectId" runat="server" />
                                                                                                        <asp:HiddenField ID="HFMySubjectNo" runat="server" />
                                                                                                        <asp:HiddenField ID="HFScreenNo" runat="server" />
                                                                                                        <asp:HiddenField ID="HFMedexInfoDtlTranNo" runat="server" />
                                                                                                        <asp:HiddenField ID="HFCRFHdrNo" runat="server" />
                                                                                                        <asp:HiddenField ID="HFCRFDtlNo" runat="server" />
                                                                                                        <asp:HiddenField ID="hfMedexCode" runat="server" />
                                                                                                        <asp:HiddenField ID="hfDecimalNo" runat="server" />
                                                                                                        <asp:HiddenField ID="HFCRFDtlLockStatus" runat="server" />
                                                                                                        <asp:HiddenField ID="HFMedExFormula" runat="server" />
                                                                                                        <asp:HiddenField ID="HFRadioButtonValue" runat="server" />
                                                                                                        <asp:HiddenField ID="HFReviewedWorkFlowId" runat="server" />
                                                                                                        <asp:HiddenField ID="HReviewFlag" runat="server" />
                                                                                                        <asp:HiddenField ID="HFActivateTab" runat="server" />
                                                                                                        <asp:HiddenField ID="HFSessionFlg" runat="server" />
                                                                                                        <asp:HiddenField ID="HFImportedDataWorkFlowId" runat="server" />
                                                                                                        <asp:HiddenField ID="HFIsRepeatNo" runat="server" />
                                                                                                        <asp:HiddenField ID="HFWorkFlowStageId" runat="server" />
                                                                                                        <asp:HiddenField ID="HFChkSelectedList" runat="server" />
                                                                                                        <asp:HiddenField ID="hdnServerDateTime" runat="server" />
                                                                                                        <asp:HiddenField ID="hdnDeleteRepetitionNo" runat="server" />
                                                                                                    </ContentTemplate>
                                                                                                </asp:UpdatePanel>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </td>
                                                                        </tr>                                                                                                                                                                                                    
                                                                            <tr id="Third" style="display:none">  
                                                                            <td>                                                                                                                                                                                                                                                                                                              
                                                                             <fieldset id="TablulerRepetiaton" class="FieldSetBox" style="min-width:98%;max-width:98%;width:98%;">                                                                                
                                                                                        <table style="width: 100%; padding-left: 0%; padding-right: 0%; padding-bottom: 1%;">
                                                                                        <tr>                                                                                           
                                                                                            <td style="width:1%;" nowrap="nowrap">
                                                                                                <span class="Label">Repetition :</span>                                                                                                                                                                                                                                                                      
                                                                                               <asp:DropDownList ID="ddlGridRepetation" multiple="multiple" runat="server" style="margin-left:37%;border-radius:7px !important;border:1px solid Navy;" CssClass="dropDownList"></asp:DropDownList>                                                                                               
                                                                                            </td>                                                                                                 
                                                                                         </tr>                                                                                         
                                                                                         <tr>
                                                                                             <td nowrap="nowrap">                                                                                                 
                                                                                                  <span class="Label">Attributes :</span>                                                                                         
                                                                                                    <asp:DropDownList ID="ddlGridActivity" multiple="multiple" style="border-radius:7px !important;border:1px solid Navy;" CssClass="dropDownList" runat="server"></asp:DropDownList>
                                                                                                    <asp:Button ID="btnGo" onClientClick="return bindTable();" Text="GO" CssClass="btn btngo" runat="server" width="7%" height="22px"/>
                                                                                             </td>
                                                                                         </tr>                                                                                                                                                       
                                                                                        <tr>                                                                                            
                                                                                            <td id="tablulerRepetation" colspan="2">
                                                                                                <br />  
                                                                                                <div id="dvTabluer" style="white-space: nowrap;">                                                                                            
                                                                                                  <table id="gvActivityGrid" runat="server"  class="dataTable"></table>   
                                                                                                 </div>                                                                                            
                                                                                           </td>                                                                                                                                                                                                            
                                                                                        </tr>
                                                                                        <tr> 
                                                                                            <td colspan="2" align="center">
                                                                                                <br />
                                                                                                 <asp:Button ID="btnExitGrid" runat="server" Text="Exit" CssClass="btn btncancel"></asp:Button>                                                                                                                            
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>                                                                                                                                                                                                     
                                                                            </fieldset>                                                                                                                
                                                                         </td>
                                                                        </tr>
                                                                     <tr>
                                                                            <td style="width: 100%; height: 40px" class="Label" align="center">
                                                                                <asp:HiddenField ID="CompanyName" runat="server" />
                                                                                <table id="tblButtons" style="width: 90%; margin: auto;">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td style="width: 50%; height: 13px" align="left">
                                                                                                <asp:Button ID="BtnPrevious" runat="server" Text="<< Previous" CssClass="btn btnnew"></asp:Button>
                                                                                            </td>
                                                                                            <td style="white-space: nowrap; height: 13px" class="Label" align="center">
                                                                                                <asp:Button ID="btnSaveAndContinue" runat="server" Text="Save & Continue" CssClass="btn btnsave"
                                                                                                     OnClientClick="return Validate();"></asp:Button>&nbsp;
                                                                                                <asp:Button ID="BtnSave" runat="server" Text="Submit" CssClass="btn btnsave" OnClientClick="return ValidationsForSave();">
                                                                                                </asp:Button>
                                                                                                <input id="BtnExit" class="btn btnexit" runat="server" onclick="closewindow('D', this);" type="button"
                                                                                                    value="Exit" title="Exit" />
                                                                                            </td>
                                                                                            <td style="width: 50%; height: 13px" align="right">
                                                                                                <asp:Button ID="BtnNext" runat="server" Text="Next >>" CssClass="btn btnnew" Width="97px">
                                                                                                </asp:Button>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <button id="BtnDataentryControl" runat="server" style="display: none;" />
                                                                                                <cc1:ModalPopupExtender ID="MpeDataentryControl" runat="server" PopupControlID="divDataentryControl"
                                                                                                    BackgroundCssClass="modalBackground" TargetControlID="BtnDataentryControl"></cc1:ModalPopupExtender>
                                                                                                <div style="display: none; width: 40%; height: 100px; text-align: left; left: 35% !important;
                                                                                                    background-color: White;" id="divDataentryControl" class="centerModalPopup">
                                                                                                    <table style="width: 100%; margin: auto">
                                                                                                        <tr align="center">
                                                                                                            <td style="font-size: 14px !important; text-align: center" class="Label">
                                                                                                                Data-Entry Control
                                                                                                                <hr />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                               
                                                                                                                  <asp:UpdatePanel ID="upDataEntryControl" runat="server" >
                                                                                                    <ContentTemplate>
                                                                                                                    <asp:Label ID="lblDataEntrycontroller" runat="server"></asp:Label>    
                                                                                                        </ContentTemplate>
                                                                                                                      <Triggers>
                                                                                                                         
                                                                                                                                              <asp:AsyncPostBackTrigger ControlID="ddlActivities" EventName="SelectedIndexChanged" >
                                                                                                                                                    </asp:AsyncPostBackTrigger>
                                                                                                                      </Triggers>
                                                                                                                      </asp:UpdatePanel>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <hr />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr id="trExit">
                                                                                                            <td align="center">
                                                                                                                <input type="submit" id="btnExitDataEntryControl" class="btn btnexit" onclick="closewindow('E', this);"
                                                                                                                    value="Exit" title="Exit" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <%--<button id="BtnAuthentication" runat="server" style="display: none;" />
                                                                                                <cc1:ModalPopupExtender ID="MpeAuthentication" runat="server" PopupControlID="divAuthentication"
                                                                                                    PopupDragHandleControlID="lblAttributeDCF" BackgroundCssClass="modalBackground"
                                                                                                    TargetControlID="BtnAuthentication" CancelControlID="ImgPopUpCloseAuthentication">
                                                                                                </cc1:ModalPopupExtender>--%>
                                                                                                <div style="display: none; width: 40%; max-height: 200px; text-align: left; margin: auto;"
                                                                                                    class="divModalPopup" runat="server" id="divAuthentication">
                                                                                                    <table style="width: 90%; margin: auto;">
                                                                                                        <tbody>
                                                                                                            <tr align="center">
                                                                                                                <td style="font-size: 14px !important; text-align: center" class="Label" colspan="2">
                                                                                                                    <img id="ImgPopUpCloseAuthentication" alt="Close" src="images/Sqclose.gif" style="position: relative;
                                                                                                                        float: right; right: 5px;" onclick="funCloseDiv('divAuthentication');" />
                                                                                                                    User Authentication
                                                                                                                    <hr />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr runat="server" id="trName">
                                                                                                                <td style="white-space: nowrap; text-align: right;" class="Label">
                                                                                                                    Name :
                                                                                                                </td>
                                                                                                                <td class="Label" align="left">
                                                                                                                    <asp:Label runat="server" ID="lblSignername" Text=""></asp:Label>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr runat="server" id="trDesignation">
                                                                                                                <td style="white-space: nowrap; text-align: right;" class="Label">
                                                                                                                    Designation :
                                                                                                                </td>
                                                                                                                <td class="Label" align="left">
                                                                                                                    <asp:Label runat="server" ID="lblSignerDesignation" Text=""></asp:Label>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr runat="server" id="trRemarks">
                                                                                                                <td style="white-space: nowrap; text-align: right;" class="Label">
                                                                                                                    Remarks :
                                                                                                                </td>
                                                                                                                <td class="Label" align="left">
                                                                                                                    <asp:Label runat="server" ID="lblSignRemarks" Text=""></asp:Label>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr runat="server" id="trpassword">
                                                                                                                <td style="white-space: nowrap; text-align: right;" class="Label">
                                                                                                                    Password :
                                                                                                                </td>
                                                                                                                <td class="Label" align="left">
                                                                                                                    <asp:TextBox ID="txtPassword" TabIndex="21" onkeydown="return CheckTheEnterKey(event);"
                                                                                                                        runat="Server" Text="" CssClass="textbox password" TextMode="Password"> </asp:TextBox>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr align="center">
                                                                                                                <td align="right" class="Label" style="white-space: nowrap">
                                                                                                                </td>
                                                                                                                <td style="text-align: left;" class="Label">
                                                                                                                    <asp:Button ID="btnAuthenticate" runat="server" CssClass="btn btnnew" OnClientClick="return ValidationForAuthentication();"
                                                                                                                        TabIndex="22" Text="Authenticate" Width="110px" />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </tbody>
                                                                                                    </table>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </asp:Panel>
                                        </asp:panel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="footer_Master">
                            <p align="center">

                                <script type="text/javascript">
                                    var copyright;
                                    var update;
                                    copyright = new Date();
                                    update = copyright.getFullYear();
                                    //document.write("<font face=\"verdana\" size=\"1\" color=\"black\">© Copyright " + update + "," + $get('<%= CompanyName.clientid %>').value + "</font>");
                                    document.write("<font face=\"verdana\" size=\"1\" >© Copyright " + update + ", Sarjen Systems Pvt LTD. </font>");
                                </script>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="1" align="center">
                            <center>
                            <div id="updateProgress" class="updateProgress" style="display: none;position: absolute;vertical-align:middle !important;">
                                <div align="center">
                                    <table>
                                         <tr>
                                            <td style="height: 120px; ">
                                                <font class="updateText" style="margin-right:22px">Please Wait...</font>
                                            </td>
                                            <td style="height: 120px">
                                                <div title="Wait" class="updateImage" style="height:120px !important">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>  
                           </center>                                                                                                     
                        </td>
                    </tr>
                </table>
                <asp:updatepanel id="upTest" runat="server" updatemode="Conditional">
                <ContentTemplate>
                    <table>
                        <tbody>
                            <tr>
                                <td>
                                    <asp:Button ID="btnShow" runat="server" Style="display: none;" />
                                    <cc1:ModalPopupExtender ID="MPEActivitySequence" runat="server" TargetControlID="btnShow"
                                        PopupControlID="DivPopSequence" BackgroundCssClass="modalBackground" BehaviorID="MPEId">
                                    </cc1:ModalPopupExtender>
                                    <div id="DivPopSequence" style="display: none; width: 50%; max-height: 400px; text-align: left;
                                        background-color: White; left: 30% !important" runat="server">
                                        <div>
                                            <table width="100%">
                                                <tr>
                                                    <td style="font-size: 14px !important; text-align: center" class="Label">
                                                        <asp:Label runat="server" ID="lblHeading" Text="Activity Deviation" />
                                                        <hr />
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="100%" style="padding: 2%">
                                                <tr>
                                                    <td align="left" colspan="2">
                                                        <asp:Label ID="lblContent" Width="100%" runat="server" Style="overflow: auto; max-height: 200px;
                                                            font-weight: 12px;">
                                                        </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="text-align: left;">
                                                        <asp:Label ID="lbl" runat="server" Text="Do You Want To Continue? "></asp:Label>
                                                        <asp:LinkButton ID="lbtnForSub" runat="server" Text="Sequence" Style="display: none;"></asp:LinkButton>
                                                        <label id="btnPforStruct" onclick="open_ProjStruct();" onmouseover="funOnMouseOver(this);">
                                                            <u>Structure Management</u></label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; width: 10%">
                                                        <asp:Label ID="lblRemark" runat="server" Text="Remarks:  " Style="color: Navy; font-weight: bold;"></asp:Label>
                                                    </td>
                                                    <td style="text-align: left; width: 90%">
                                                        <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" Width="90%"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="text-align: left;">
                                                        <asp:Label ID="lblError" runat="server" Style="color: Red;"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2">
                                                        <asp:Button runat="server" ID="btnOk_MPEID" Text="OK" OnClientClick="return checkRemark_Sequence();"
                                                            class="btn btnsave " />
                                                        <input type="button" id="btnCancel_MPEID" value="Cancel" onclick="Cancel_MPEID();"
                                                            class="btn btncancel " />
                                                        <%--<asp:Button ID="" Text="" CssClass="button" runat="server" />--%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    <asp:HiddenField ID="HFPendingNode" runat="server" />
                                    <asp:HiddenField ID="HFRemarks" runat="server" />
                                    <asp:HiddenField ID="HFselectedActivity" runat="server" />
                                    <asp:HiddenField ID="HFNumericScale" runat="server" />
                                    <asp:HiddenField ID="hdnMedExCode" runat="server" />  <%--Added By Vivek Patel--%>
                                     <asp:HiddenField ID="hdniRepeatNo" runat="server" /> <%--Added By Vivek Patel--%>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlActivities" EventName="SelectedIndexChanged">
                    </asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="BtnOk" EventName="Click"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="BtnAuthenticate" EventName="Click"></asp:AsyncPostBackTrigger>
                    <asp:PostBackTrigger ControlID="btnOk_MPEID" />   
                    <asp:AsyncPostBackTrigger ControlID="btnDataEntryModalPopup" EventName="click" />                 
                    <%--<asp:AsyncPostBackTrigger ControlID="btnSaveRemarksForAttribute" EventName="Click"></asp:AsyncPostBackTrigger>--%>
                    <%--<asp:AsyncPostBackTrigger ControlID="btnAudittrail" EventName="Click"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="btnAuthenticate" EventName="Click"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="btnAutoCalculate" EventName="Click"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="btnDCF" EventName="Click"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="btnMeddraBrowse" EventName="Click"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="btnSaveAndContinue" EventName="Click"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="btnSaveDiscrepancy" EventName="Click"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="btnSaveRemarksForAttribute" EventName="Click"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="btnUpdateDiscrepancy" EventName="Click"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="rblRepeatNo" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>--%>
                </Triggers>
            </asp:updatepanel>
            </center>
        </div>
        <asp:button id="btnmdl" runat="server" style="display: none;" />
        <cc1:modalpopupextender id="mdlSessionTimeoutWarning" runat="server" popupcontrolid="divSessionTimeoutWarning"
            backgroundcssclass="modalBackground" behaviorid="mdlSessionTimeoutWarning" targetcontrolid="btnmdl">
    </cc1:modalpopupextender>
        <div id="divSessionTimeoutWarning" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 35%; height: auto; max-height: 25%; min-height: auto;">
            <asp:updatepanel id="HM_Home_upnlSession" runat="server" updatemode="Conditional"
                rendermode="Inline">
            <ContentTemplate>
                <table width="350px" align="center">
                    <tr>
                        <td>
                            <img id="Img2" src="~/Images/showQuery.png" runat="server" alt="Confirmation" />
                        </td>
                        <td class="Label" style="text-align: left;">
                            Your session will expire within 5 mins.
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button ID="btnContinueWorking" runat="server" Text="Extend" CssClass="btn btnsave"
                                Style="display: none;" />
                            <asp:Button ID="BtnSessionDivClose" runat="server" Text="Close" CssClass="btn btnclose"
                                OnClientClick="closeSessionDiv();" />
                            <asp:Button ID="btnLogout" runat="server" Text="GO" CssClass="btn btnclose" Style="display: none;" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:updatepanel>
        </div>
        
         <asp:button id="btnViewDocument" runat="server" OnClientClick="return viewProjectDocument(this);" style="display:none"></asp:button>
         <cc1:ModalPopupExtender ID="mdViewDocument" runat="server" BackgroundCssClass="modalBackground" BehaviorID="mdViewDocument"
         CancelControlID="imgActivityAuditTrail" PopupControlID="dvdialog" TargetControlID="btnViewDocument"></cc1:ModalPopupExtender>
         
         <div id="dvdialog" style="display: none" class="centerModalPopup" style="display: none; width:75%;">
          <table border="0" cellpadding="2" cellspacing="2" width="100%">
                         <tr>
                               <%--<td id="Td2" class="LabelText" style="text-align: center !important; font-size: 15px !important; width: 97%;">Guideline Instructionstant; width: 97%;">Guideline Instructionstant; width: 97%;">Guideline Instructions</td>--%>
                               <td id="Td2" class="LabelText" style="text-align: center !important; font-size: 15px !important; width: 97%;">Guideline Instructions</td>

                               <td style="width: 3%">
                                    <img id="imgActivityAuditTrail" alt="Close" src="images/close.gif" onmouseover="this.style.cursor='pointer';" />
                               </td>
                         </tr>
                          <tr>
                             <td colspan="2">
                                 <hr />
                             </td>
                         </tr>
                         <tr>
                            <td colspan="2">
                                <table style="width:100%;">
                                    <tr>
                                        <td>
                                            <iframe id="ifviewDocument" style="height:391px; width:100%;" runat="server"></iframe>
                                        </td>
                                    </tr>
                                </table>
                                                                                                                 
                            </td>
                        </tr>
                         <tr>
                              <td colspan="2">
                          <hr />
                         </td>
                  </tr>
            </table>
          </div>
         <asp:Button ID="btnOpenModelPopup" runat="server" style="display:none"/>    
      <asp:Button ID="btnDataEntryModalPopup" runat="server" CssClass="btn btnnew" style="display:none;"/>
      <cc1:ModalPopupExtender ID="mpDataEntry" runat="server" BackgroundCssClass="modalBackground" BehaviorID="mpDataEntry" CancelControlID="imgDataEntryClose" 
         PopupControlID ="divActivityGridpopup" TargetControlID="btnDataEntryModalPopup"></cc1:ModalPopupExtender>        
      <table>
        <tr>
            <td>              
                      <div id="divActivityGridpopup" runat="server" class="centerModalPopup" style="background-color: aliceblue; display: none; overflow: auto; width: 76%; height: auto; max-height: 84%; min-height: auto;">
                          <table border="0" cellpadding="2" cellspacing="2" width="100%">
                              <tr>
                                   <td class="LabelText" style="color:Navy;font-family:Verdana;font-size:Small;background-color:aliceblue; text-align: center !important; font-size: 15px !important; width: 97%;"> View Repetition</td>
                                   <td style="width: 3%">
                                    <img id="imgDataEntryClose" alt="Close" src="images/Sqclose.gif" onclick="return ClearData();" onmouseover="this.style.cursor='pointer';" />
                                       <br />
                               </td>
                              </tr>
                            <tr>
                               <td id="Td1" class="LabelText" style="font-weight:bold; background-color:aliceblue; text-align: center !important; font-size: 15px !important; width: 97%;"><br /></td>                              
                               </tr>
                                <tr>
                                    <td colspan="2">
                                    <hr />
                                    </td>
                                </tr>
                                <tr>
                                 <td colspan="2">
                                 <table align="center" id="tblModalDataEntry" border="0" cellpadding="2" cellspacing="2" width="100%">                                    
                                 <tr>
                                     <td>
                                        <div id="dvModelDatEntry" width="100%"></div>                                                                                                                                    
                                     </td>                                    
                                 </tr>                                                           
                                 </table>
                                 </td>
                                  </tr>
                                  <tr>
                                      <td colspan="2">
                                       <hr />
                                      </td>
                                  </tr>
                         </table>                  
                      </div>                   
            </td>        
        </tr>                 
    </table> 
         <asp:Button ID="btnUpdateDCFRemarks" runat="server" CssClass="btn btnupdate" style="display:none;"/>
      <cc1:ModalPopupExtender ID="mdpDCFUpdateRemarks" runat="server" BackgroundCssClass="modalBackground" BehaviorID="mdpDCFUpdateRemarks" CancelControlID="imgDataEntryClose" 
         PopupControlID ="divUpdateDCFRemarks" TargetControlID="btnUpdateDCFRemarks"></cc1:ModalPopupExtender>        
      <table>
        <tr>
           <td>              
                      <div id="divUpdateDCFRemarks" runat="server" class="centerModalPopup" style="background-color: aliceblue; display: none; overflow: auto; width: 30%; height: auto; max-height: 84%; min-height: auto;">
                            <table style="width: 100%; margin-bottom: 2%">
                            <tr>
                                 <td colspan="2" style="font-size: 14px;" class="Label">
                                         <img id="Img3" alt="Close" src="images/Sqclose.gif" style="position: relative;
                                         float: right; right: 5px;" onclick="return ClearDCFUpdateRemarks();" onmouseover="this.style.cursor='pointer';"  />
                                Reason For Update
                                <hr />
                                </td>
                                </tr>
                            <tr>
                                    <td class="Label" style="text-align: right; width: 30%">
                                        <span class="Label">Reason* :</span>
                                    </td>
                                     <td style="text-align: left; width: 70%">
                                         <asp:DropDownList ID="ddlDirectUpdateRemarks" runat="server" CssClass="dropDownList" Width="70%">
                                         </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: center; width: 100%">
                                        <span class="Label">OR </span>
                                    </td>
                               </tr>
                               <tr>
                                   <td class="Label" style="text-align: right; width: 30%">
                                       <span class="Label" style="vertical-align: top;">Remark* : </span>
                                   </td>
                                   <td style="text-align: left; width: 70%">
                                       <asp:TextBox ID="txtDCFUpdateRemarks" runat="Server" Text="" TextMode="MultiLine"
                                       CssClass="textbox" Width="70%"> </asp:TextBox>
                                       <asp:HiddenField ID="hdnDCFUpdateRemarks" runat="server" Value="" />
                                  </td>
                              </tr>
                              <tr>
                                 <td colspan="2">
                                 </td>
                              </tr>
                              <tr>
                                  <td class="Label" colspan="2" style="text-align: center; width: 100%">
                                      <asp:Button ID="btnUpdateRemarks" runat="server" CssClass="btn btnupdate" OnClientClick="return CheckDCFUpdateRemarksValidation();"
                                          Text="Save" />
                                         <input class="button" onclick="return ClearDCFUpdateRemarks();" type="button" value="Close"/>
                                 </td>
                             </tr>
                    </table>              
                   </div>                   
            </td>        
        </tr>                 
    </table>      
        <asp:Button ID="btnClearDynamicPage" runat="server" style="display:none;"/>
        <div id="hidDV" style="display:none"></div>         
        <asp:HiddenField ID="hndtabluler" runat="server"/>
        <asp:HiddenField ID="hndDiscrpancyStatus" runat="server"/>
         <asp:HiddenField ID="hndGridViewStatus" runat="server"/>
        <asp:hiddenField ID="hndLatestRepeatition" runat="server" />
        <asp:hiddenField ID="hndRepetitionNo" runat="server" />
        <asp:hiddenField ID="hndLetestData" runat="server" />

        <div id="myModalSignAuth" class="modal" runat="server" display="none">
            <div class="modal-content" style="top:6%; width: 38%">
                <div class="modal-header" style="text-align: left">
                    <img id="Img4" alt="Close1" src="images/Sqclose.gif" class="close modalCloseImage" title="Close" runat="server" onclick="CloseAuthentication();" />
                    <h3 style="text-align: center">
                        <asp:label runat="server" id="Label4">Signature Authentication</asp:label>
                    </h3>
                    <table width="100%">
                        <tr>
                            <td align="right" style="width: 16%; font-weight: bold; font-size: smaller;">User Name :
                            </td>
                            <td align="left" style="width: 15%; font-size: small">
                                <asp:label runat="server" id="lblSignAuthUserName"></asp:label>
                            </td>
                            <td align="right" style="width: 16%; font-weight: bold; font-size: 10px;">Date & Time :
                            </td>
                            <td align="left" style="width: 26%;">
                                <asp:label runat="server" id="lblSignAuthDateTime"></asp:label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="modal-body">
                    <table width="100%">
                        <tr>
                            <td colspan="2">
                                <fieldset>
                                    <legend style="text-align:left;" id="legAuthentication">Authentication</legend>
                                    <table id="tblDigitalFP" runat="server">
                                        <tr id="FollowVisitHidden" >
                                            <td align="right">
                                                <asp:label id="Label6" runat="server" text="Image having personal information?* :"></asp:label>
                                         </td>
                                            <td align="left">
                                                <asp:radiobuttonlist id="rblPersonalInfo" runat="server" repeatdirection="Horizontal" readonly="true" onclick="return fnCheckPersonalInformation(this);">
                                                <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                            </asp:radiobuttonlist>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td colspan="2"></td>
                                        </tr>

                                        <tr>
                                            <td align="right">
                                                <asp:label id="lbleligible" runat="server" text="Approval Status* :"></asp:label>
                                            </td>
                                            <td align="left">
                                                <asp:radiobuttonlist id="rblApprovalStatus" runat="server" repeatdirection="Horizontal" readonly="true" onclick="return fnCheckChangeEvent(this);">
                                                <asp:ListItem Text="Approve" Value="A"></asp:ListItem>
                                                <asp:ListItem Text="Reject" Value="R"></asp:ListItem>
                                            </asp:radiobuttonlist>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2"></td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:label id="lblRemarks" runat="server" text="Remarks* :"></asp:label>
                                            </td>
                                            <td align="left">
                                                <textarea id="txtQCRemarks" runat="server" class="textBox" style="width: 100%;" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <%--<td align="left" style="font-weight: bold; font-size: small;">Password :--%>
                            <td align="right">
                                <asp:label id="Label5" runat="server" text="Password* :"></asp:label>
                            </td>
                            <td align="left">
                                <asp:textbox id="txtPasswords" runat="Server" text="" width="59%" cssclass="textbox" textmode="Password" oncopy="return false" onpaste="return false"
                                    oncut="return false" placeholder="Password" class="td-ie8" autocomplete="off"></asp:textbox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                               <%-- <br />--%>
                            </td>
                        </tr>
                        <tr>
                            <%--<td align="center" style="font-weight: bold; font-size: small;" colspan="2">I hereby confirm signing of this record electronically. --%>
                            <td align="center" colspan="2">
                                <asp:label id="Label7" runat="server" text="I hereby confirm signing of this record electronically."></asp:label>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                        <tr>
                            <td style="text-align: center" colspan="2">
                                <asp:updatepanel id="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnSignAuthOK" runat="server" CssClass="btn btnsave" Text="OK" Width="105px" OnClientClick="return ValidationForQCAuthentication();"/>
                                    <asp:Button ID="btnSignAuthCancel" runat="server" CssClass="btn btncancel" Text="Cancel" Width="105px" OnClientClick="return SignAuthModalClose();" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSignAuthOK" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSignAuthCancel" EventName="Click" />
                                </Triggers>
                            </asp:updatepanel>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>

         <div id='loadingmessage' style='display:none'></div>
     <div id="QueryDiv" class="modal">
      <div class="modal-content" style="top:20%;left:4%;width:90%"}}>
            <div class="modal-header">
                <img alt="Close1" src="images/Sqclose.gif" class="close modalCloseImage"  onclick="QueryDetailsClose(); return false;" />
                <h3 style="text-align:center">
                <label id="Label31">Query Details</label>
                </h3>
            </div>
            <div class='modal-body' style="overflow-y:auto; overflow-x:auto;">
                 <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                    <ContentTemplate>
                 <table class="FormTable" id="tblQuerydetalis">
                    <tbody>
                        <tr>
                            
                            <td class="Label" style="text-align: right; width: 30%">
                               <span class="Label" style="vertical-align: top;">Reupload Required: </span>
                            </td>
                            <td style="text-align: left" ><input type="checkbox" id="CheckReUpload" value="R" runat="server" /></td>

                        </tr>
                        <tr>
                               <td class="Label" style="text-align: right; width: 30%">
                               <span class="Label" style="vertical-align: top;">Approve With Warning: </span>
                            </td>
                            <td style="text-align: left" ><input type="checkbox" id="CheckQueryApprove" value="A" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="Label" style="text-align: right; width: 30%">
                               <span class="Label" style="vertical-align: top;">Remark* : </span>
                            </td>
                                <td style="text-align: left; width: 70%">
                                    <asp:TextBox ID="txtRemarkQuery" runat="Server" Text="" TextMode="MultiLine" rows="6"
                                        CssClass="textbox" Width="70%"> </asp:TextBox>
                                    <asp:HiddenField ID="HiddenField1" runat="server" Value="" />
                                </td>
                         </tr>
                        <tr>
                                        <td nowrap="nowrap" colspan="2" style="text-align: center; vertical-align: top;">
                                            <asp:Button ID="btnQuerySave" OnClientClick="return QueryValidation();" runat="server" CssClass="btn btnsave"
                                                Text="Save" ToolTip="Save" TabIndex="5" />
                                            <asp:Button ID="btnQurtyAudit" runat="server" CausesValidation="False" CssClass="btn btncancel"
                                                Text="Cancel" ToolTip="Cancel" TabIndex="6" />
                                        </td>
                                    </tr>
                        </tbody>
                     <//table>
                    </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnQuerySave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnQurtyAudit" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
            </div>
        </div>
         </div>                   
        <script type="text/javascript">

            var RepetationList = "";
            var AttributeList = "";
            var EnableScrollY;
            var cachedModalBehavior;

            //Add by shivani pandya for theme selection
            jQuery(document).ready(function () {
                assignCSS();
                selectTheme();
            });

            $(function () {
                $("#tblQuerydetalis input[type=checkbox]").click(function () {
                    if ($(this).is(":checked")) {
                        $("#tblQuerydetalis input[type=checkbox]").removeAttr("checked");
                        $(this).attr("checked", "checked");
                    }
                });
            });


            
            $('#ddlGridRepetation').change(function () {
                var DataRepetation = [];
                console.log($(this).val());
                if ($(this).val() == null) {
                    $('.placeholder').html("Select value");
                }
            }).multipleSelect({
                width: '100%',
                filter: true
            });
            $('#ddlGridActivity').change(function () {
                var DataAttribute = [];
                console.log($(this).val());
                if ($(this).val() == null) {
                    $('.placeholder').html("Select value");
                }
            }).multipleSelect({
                width: '100%',
                filter: true
            });

            $('#btnSaveAndContinue').one("click", function (event) {
                $(this).off(event);
            });

            $('#ddlRepeatNo').change(function () {
                var DataAttribute = [];
                console.log($(this).val());
                if ($(this).val() == null) {
                    $('.placeholder').html("Select value");
                }
            }).multipleSelect({
                width: '100%',
                includeSelectAllOption: true,
                emptyText: "Please select ..."
            });


            $("#ddlRepeatNo").next().first().find(".ms-drop li").first().attr("style", "Display:none");
            $("#ddlRepeatNo").next().first().find(".ms-drop li").first().attr("checked", "checked");

            $(document).ready(function () {
                var abc = ""
                if ($("#hndRepetitionNo").val() != "") {
                    $("#ddlRepeatNo").next().first().find(".ms-drop li").next().find("input").each(function () {
                        abc = $("#hndRepetitionNo").val().split(",")
                        for (i = 0; i <= abc.length; i++) {
                            if ($(this).attr("value") == abc[i]) {
                                $(this).attr("Checked", "Checked");
                                $("#ddlRepeatNo [value=" + $(this).attr("value") + " ]").attr("selected", "selected");
                            }
                        }
                        //$(this).attr("value")
                    });
                }
            });


            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_initializeRequest(onUpdating);
            prm.add_endRequest(onUpdated);

            var ACTUAL_SESSIONTIME = "<%= Session.Timeout %>", timerId, sessionFlag = true;
            SessionTimeSet();

            function onUpdating(sender, args) {
                var updateProgressDiv = $get('updateProgress');

                createDiv();
                updateProgressDiv.style.display = '';
                setLayerPosition();
            }

            function Validate() // Added by dipen shah on 3-dec-2014 tpo persist more than one click on save and continue. 
            {

                btnSaveAndContinue.style.display = 'none';
                BtnSave.style.display = 'none';
                onUpdating();
                return true;
            }
            function createDiv() {
                var divTag = document.createElement('div');

                divTag.id = 'shadow';
                divTag.setAttribute('align', 'center');
                divTag.style.position = 'absolute';
                divTag.style.top = '0px';
                divTag.style.left = '0px';
                divTag.style.opacity = '0.6';
                divTag.style.filter = 'alpha(opacity=30)';
                divTag.style.backgroundColor = '#000000';
                divTag.style.zIndex = '100000';
                //Add by shivani pandya
                divTag.style.height = $(document).height() + "px";

                document.body.appendChild(divTag);
            }

            function onUpdated(sender, args) {
                // get the update progress div
                var updateProgressDiv = $get('updateProgress');
                // make it invisible
                updateProgressDiv.style.display = 'none';
                document.body.removeChild(document.getElementById('shadow'));

                //clearTimeout(tld);
            }

            function setLayerPosition() {

                var winScroll = BodyScrollHeight();
                var updateProgressDivBounds = Sys.UI.DomElement.getBounds($get('updateProgress'));
                var shadow = document.getElementById('shadow');
                var bws = GetWindowBounds();

                if (!shadow) {
                    return;
                }
                shadow.style.width = bws.Width + "px";
                //Add by shivani pandya
                shadow.style.height = $(document).height(); +"px"
                // shadow.style.height = bws.Height + "px";
                shadow.style.top = winScroll.yScr;
                shadow.style.left = winScroll.xScr;
            }
            window.onresize = setLayerPosition;
            window.onscroll = setLayerPosition;

            function pageLoad() {
                //   $(window).width() > 1180 ? wid = ($(window).width() - 94) + 'px' : wid = $(window).width() - 100 + 'px';
                //   $('#<%= gvActivityGrid.ClientID%>').attr("style", "width:" + wid + ";")

                jQuery('#<%= btnShowQuery.ClientID %>').fadeToggle('slow').fadeToggle('slow').fadeToggle('slow').fadeToggle('slow');

                cachedModalBehavior = $find('mpDataEntry');

            }
            //jQuery('#<%= imgbtnShowQuery.ClientID %>').fadeToggle('slow').fadeToggle('slow').fadeToggle('slow').fadeToggle('slow').fadeToggle('slow').fadeToggle('slow').fadeToggle('slow').fadeToggle('slow');

            function CloseWindow() {
                msgalert("Please Enter Actual date.")
                window.close();
            }

            function show_popup(str) {
                var arg = 'resizable=no, toolbar=no,location=no,directories=no,addressbar=no,scrollbars=no,status=no,menubar=no,top=100,left=250';
                window.open(str, "_blank", arg);
                return false;
            }
            function checkRemark_Sequence() {
                var txtContent = document.getElementById('<%= txtContent.ClientID %>').value.toString().trim();
                if (txtContent == "") {
                    document.getElementById('<%=lblError.ClientID %>').innerHTML = "Please Enter Remark";
                    return false;
                }
                //closewindow('COMBO', this); 
                return true;
            }
            function Cancel_MPEID() {
                $find('MPEId').hide();
            }
            function funOnMouseOver(id) {

                id.style.cursor = 'pointer';
            }
            function open_ProjStruct() {
                window.open("frmEditWorkspaceNodeDetail.aspx?WorkSpaceId=" + document.getElementById('<%= HFWorkspaceId.ClientId%>').value);
            }

            function funCloseDiv(div) {

                if ($('#gvActivityGrid tr td').length != 0) {
                    if (div != "divAuthentication") {
                        $find('mpDataEntry').show();
                        $("#secound").css({ display: "none" });
                    }
                }

                var val = document.getElementById('hdnOldMedExValue').value
                var val1 = document.getElementById('hfMedexCode').value

                if (val != "" || val != "undefined") {
                    // document.getElementById(val1).value = val

                    var type = "AD"
                    var MedexVal = document.getElementById('hdnOldMedExValue').value
                    var eledisable = false;
                    var chklst = document.getElementById(val1);
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
                                        if (MedexVal == "") {
                                            MedexVal = $(chks[i]).next().text();
                                        } else {
                                            MedexVal = MedexVal + "," + $(chks[i]).next().text();
                                        }

                                    }
                                }

                                else if (type == "D") {
                                    eledisable = true;
                                    chks[i].disabled = true;
                                    $(chks[i]).parents('span').attr('disabled', 'disabled');
                                }
                                //else if (type == "AD") {
                                //    eledisable = true;
                                //    // chks[i].checked = false;
                                //    if (chks[i].value == document.getElementById('hdnOldMedExValue').value) {
                                //        chks[i].checked = true;
                                //    }
                                //    chks[i].disabled = true;
                                //    $(chks[i]).parents('span').attr('disabled', 'disabled');
                                //}
                            }
                        }
                    }


                    var radiolst = document.getElementById(val1);
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
                                    //radios[i].checked = false;
                                    if (radios[i].value == document.getElementById('hdnOldMedExValue').value) {
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
                            else if (document.getElementById('HFMedexType').value.toString().toUpperCase() == "STANDARDDATETIME") {
                                MedexVal = $('select[id*="' + MedexCode + '"]')[0].value + $('select[id*="' + MedexCode + '"]')[1].value + $('select[id*="' + MedexCode + '"]')[2].value + $('select[id*="' + MedexCode + '"]')[3].value + $('select[id*="' + MedexCode + '"]')[4].value
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
                            else if (document.getElementById('HFMedexType').value.toString().toUpperCase() == "STANDARDDATETIME") {
                                MedexVal = $('select[id*="' + MedexCode + '"]')[0].value + $('select[id*="' + MedexCode + '"]')[1].value + $('select[id*="' + MedexCode + '"]')[2].value + $('select[id*="' + MedexCode + '"]')[3].value + $('select[id*="' + MedexCode + '"]')[4].value
                            }
                            else MedexVal = chklst.value;
                        }
                        document.getElementById('HdnMedexVal').value = MedexVal;
                        return true;
                    }
                    if (type == "D") {
                        if (eledisable == false) {

                            if (document.getElementById('HFMedexType').value == "File") {
                                MedexCode = "FU" + MedexCode;
                            }
                            if (document.getElementById('HFMedexType').value.toString().toUpperCase() == "STANDARDDATE") {
                                $('select[id*="' + MedexCode + '"]').attr('disabled', true);
                            }
                            else if (document.getElementById('HFMedexType').value.toString().toUpperCase() == "STANDARDDATETIME") {
                                $('select[id*="' + MedexCode + '"]').attr('disabled', true);
                            }
                            else document.getElementById(MedexCode).disabled = true;
                        }
                    }
                    if (type == "AD") {
                        if (eledisable == false) {

                            if (document.getElementById('HFMedexType').value == "File") {
                                MedexCode = "FU" + MedexCode;
                            }

                            if (document.getElementById('HFMedexType').value == "STANDARDDATE") {
                                $('select[id*="' + val1 + '"]')[0].value = document.getElementById('hdnOldMedExValue').value.toString().substring(0, 2)
                                $('select[id*="' + val1 + '"]')[1].value = document.getElementById('hdnOldMedExValue').value.toString().substring(2, 4)
                                $('select[id*="' + val1 + '"]')[2].value = document.getElementById('hdnOldMedExValue').value.toString().substring(4, 8)

                                $('select[id*="' + val1 + '"]').attr('disabled', true);
                                document.getElementById('HFMedexType').value = ""; // Added By Jeet Patel on 15-Jun-2015
                            }
                            else if (document.getElementById('HFMedexType').value == "STANDARDDATETIME") {
                                $('select[id*="' + val1 + '"]')[0].value = document.getElementById('hdnOldMedExValue').value.toString().substring(0, 2)
                                $('select[id*="' + val1 + '"]')[1].value = document.getElementById('hdnOldMedExValue').value.toString().substring(2, 4)
                                $('select[id*="' + val1 + '"]')[2].value = document.getElementById('hdnOldMedExValue').value.toString().substring(4, 8)
                                //Added BY Rahul For SDTM Changes
                                $('select[id*="' + val1 + '"]')[3].value = document.getElementById('hdnOldMedExValue').value.toString().substring(8, 10)
                                $('select[id*="' + val1 + '"]')[4].value = document.getElementById('hdnOldMedExValue').value.toString().substring(10, 12)

                                $('select[id*="' + val1 + '"]').attr('disabled', true);
                                document.getElementById('HFMedexType').value = ""; // Added By Jeet Patel on 15-Jun-2015
                            }
                            else {
                                if (val1 != "") {
                                    document.getElementById(div).disabled = true;  // Change Val1 to Div for closing DCF and Audit Trail Window  By Jeet Patel on 15-Jun-2015
                                    document.getElementById(div).value = val;
                                    if (document.getElementById(val1) != null) {  //Added by Shivani
                                        document.getElementById(val1).disabled = true; // Added by Aaditya on 06-Oct-2015 for Issue in Save and continue without remarks and reason
                                        document.getElementById(val1).value = val;
                                    }
                                }
                            }
                        }
                    }
                }
                document.getElementById(div).style.display = 'none';
                document.getElementById('<%=ModalBackGround.ClientId %>').style.display = 'none';
                // document.getElementById(val1).disabled = true
            }

            function displayBackGround() {
                document.getElementById('<%=ModalBackGround.ClientId %>').style.display = '';
                document.getElementById('<%=ModalBackGround.ClientId %>').style.height = $('#HFHeight').val() + "px";
                document.getElementById('<%=ModalBackGround.ClientID%>').style.width = $('#HFWidth').val() + "px";
            }
            function displaydivAuthentication() {
                DivAuthenticationHideShow('S');
            }

            function DeleteRepetition(RepetitionValue) {
                $get('<%= hdnDeleteRepetitionNo.ClientID%>').value = ''
                if (confirm(ddlRepeatNo.options.length == 2 ? 'Are you sure to delete? ' : 'Are you sure to delete repetition ' + RepetitionValue.split("_")[1] + "?") == true) {
                    $get('<%= divDeleteRepetition.ClientId%>').style.display = '';
                    $get('<%= hdnDeleteRepetitionNo.ClientID%>').value = RepetitionValue;
                    displayBackGround();
                    if ($('#gvActivityGrid tr td').length != 0) {
                        $("#secound").css({ display: "block" });
                        $find('mpDataEntry').hide();
                        $("#tblMain").css({ display: "none" });
                    }
                }
                return false;
            }

            function ValidationForDeleteRepetition() {
                if ($get('<%= txtDeleteRepetition.ClientID%>').value == '') {
                    msgalert('Please enter remarks for deletion.');
                    $get('<%= txtDeleteRepetition.ClientID%>').focus();
                    return false;
                }
            }
            function CheckStandardDateAttr(ele, DateId, MonthId, YearId, TargetMedExCode) {
                //var Flg;
                //flg = CheckStandardDate(ele, DateId, MonthId, YearId)
                CheckStandardDate(ele, DateId, MonthId, YearId)
                if (TargetMedExCode != "") {
                    var TargetMedExCodeArray = TargetMedExCode.split(','), MedExCode = "", boolFlag = false;
                    for (var count = 0; count < TargetMedExCodeArray.length; count++) {
                        MedExCode = TargetMedExCodeArray[count];
                        if (MedExCode != undefined && MedExCode != "") {

                            if (MedExCode.split("#")[2].toUpperCase().indexOf('[' + $($('select[id*="' + ele.id.split('_')[0] + '"]')[0]).val() + $($('select[id*="' + ele.id.split('_')[0] + '"]')[1]).val() + $($('select[id*="' + ele.id.split('_')[0] + '"]')[2]).val() + ']') != -1) {
                                if (MedExCode.split("#")[1].toLowerCase() === "radio" ||
                                    MedExCode.split("#")[1].toLowerCase() === "checkbox") {
                                    $("#" + MedExCode.split("#")[0] + ' *').removeAttr("disabled");
                                    $("#" + MedExCode.split("#")[0] + ' *').val('');
                                }
                                else if (MedExCode.split("#")[1].toLowerCase() === "file") {
                                    $("#FU" + MedExCode.split("#")[0]).removeAttr("disabled");
                                    $("#FU" + MedExCode.split("#")[0]).val('');
                                }
                                else if (MedExCode.split("#")[1].toLowerCase() === "comboglobaldictionary") {
                                    $("#" + MedExCode.split("#")[0]).siblings().removeAttr("disabled");
                                    $("#" + MedExCode.split("#")[0]).val('');
                                }
                                else {
                                    $("#" + MedExCode.split("#")[0]).removeAttr("disabled");
                                    $("#" + MedExCode.split("#")[0]).siblings().removeAttr("disabled");
                                    $("#" + MedExCode.split("#")[0]).val('');
                                }

                            }
                            else {
                                if (MedExCode.split("#")[1].toLowerCase() === "radio" ||
                                    MedExCode.split("#")[1].toLowerCase() === "checkbox") {
                                    $("#" + MedExCode.split("#")[0] + ' *').attr("disabled", "disabled");
                                    $("#" + MedExCode.split("#")[0] + ' *').removeAttr('checked');
                                }
                                else if (MedExCode.split("#")[1].toLowerCase() === "file") {
                                    $("#FU" + MedExCode.split("#")[0]).attr("disabled", "disabled");
                                    $("#FU" + MedExCode.split("#")[0]).val('');
                                }
                                else if (MedExCode.split("#")[1].toLowerCase() === "comboglobaldictionary") {
                                    $("#" + MedExCode.split("#")[0]).siblings().attr("disabled", "disabled");
                                    $("#" + MedExCode.split("#")[0]).val('');
                                }
                                else {
                                    $("#" + MedExCode.split("#")[0]).attr("disabled", "disabled");
                                    $("#" + MedExCode.split("#")[0]).siblings().attr("disabled", "disabled");
                                    $("#" + MedExCode.split("#")[0]).val('');

                                }
                            }
                        }
                    }
                }
                //if(flg != false)
                //    CheckToUpdateValue(BtnUpdateId);

            }

            function ParentButton() {

                window.parent.document.getElementById('ctl00_CPHLAMBDA_btnSaveandredirect').click();
            }

            $("input:checkbox, label").dblclick(function (event) {

                return false;
                //event.preventDefault();
                // Your code goes here
            });


            function Value() {
                var len = document.getElementById("txtDiscrepancyRemarks").value
                if (len.length > 500) {
                    event.returnValue = false;
                    // alert("more than "  + " chars");
                    return false;
                }

            }

            function viewProjectDocument(e) {
                var SID = window.location.href.split('&');
                var ActivityId = $get('ddlActivities').options[$get('ddlActivities').selectedIndex].value;
                var WorkSpaceId = document.getElementById('HFWorkspaceId').value;
                var NodeId = document.getElementById('ddlActivities').value.split("#")[1];
                //For Activity
                var abc = ActivityId.split("#");
                var ActivityId = abc[0];

                //For Period
                var pqr = SID[3].split("=");
                var Period = pqr[1];

                $.ajax({
                    type: "post",
                    url: "frmCTMMedExInfoHdrDtl.aspx/ViewDocument",
                    data: '{"ActivityId":"' + ActivityId + '","Period":"' + Period + '","WorkSpaceId":"' + WorkSpaceId + '","NodeId":"' + NodeId + '"}',
                    contentType: "application/json; charset=utf-8",
                    datatype: JSON,
                    async: false,
                    success: function (data) {
                        if (data.d == "") {
                            msgalert("No Uploaded Documents found for selected Activity");
                        }
                        else {
                            var FileName = data.d;
                            $("#ifviewDocument").attr("src", FileName);
                            $find('mdViewDocument').show();
                            return false;
                        }
                    }
                });

            }

            function viewDicom() {
                var ParentWorkSpaceId = document.getElementById('HFParentWorkspaceId').value;
                var WorkSpaceId = document.getElementById('HFWorkspaceId').value;
                var SubjectId = document.getElementById('HFSubjectId').value;
                //var MySubjectNo = document.getElementById('HFMySubjectNo').value;
                var iMySubjectNo = document.getElementById('HFMySubjectNo').value;
                var ActivityName = document.getElementById('hdnActivityName').value;
                var NodeId = document.getElementById('HFParentNodeId').value;
                var PeriodId = document.getElementById('HFPeriodId').value;
                var ScreenNo = document.getElementById('HFScreenNo').value;
                var Uid = document.getElementById('hdniUserId').value;
                var DISoftURL = document.getElementById('DISoftURL').value;
                var URL = DISoftURL + 'MIBizNETImageReview/MIBizNETImageReview?WId=';
                //var ip = document.getElementById('hdniUserIp').value;

                $.ajax({
                    url: "frmCTMMedExInfoHdrDtl.aspx/checkDicom",
                    type: "POST",
                    data: '{"WorkSpaceId":"' + WorkSpaceId + '","NodeId":"' + NodeId + '","SubjectId":"' + SubjectId + '","iMySubjectNo":"' + iMySubjectNo + '"}',
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    datatype: "json",
                    success: successviewDicom,
                    error: errorviewDicom
                });

                function successviewDicom(data) {
                    var data
                    data = data.d
                    var msgs = [];
                    msgs = JSON.parse(data);
                    if (msgs.Table[0] != null) {
                        var WId = msgs.Table[0].vWorkspaceId;
                        var SId = msgs.Table[0].vSubjectId;
                        var PId = msgs.Table[0].vProjectNo;
                        var Uid = document.getElementById('hdniUserId').value;
                        var MId = msgs.Table[0].iModalityNo;
                        var AId = msgs.Table[0].iAnatomyNo;
                        var VId = msgs.Table[0].iNodeId;
                        var HdrId = msgs.Table[0].iImgTransmittalHdrId;
                        var DtlId = msgs.Table[0].iImgTransmittalDtlId;

                        var ActivityID, NodeId, ActivityDef; HFParentActivityId
                        //if (ActivityName.indexOf("Target Lesions") >= 0) {
                        if (ActivityName.match("Target")) {
                            ActivityID = document.getElementById('HFActivityId').value;
                            NodeId = document.getElementById('HFNodeId').value;
                            ActivityDef = 'TL';
                        }
                        if (ActivityName.indexOf("Non Target") >= 0) {
                            ActivityID = document.getElementById('HFActivityId').value;
                            NodeId = document.getElementById('HFNodeId').value;
                            ActivityDef = 'NTL';
                        }
                        //window.open('http://90.0.0.68/MI/MIBizNETImageReview/MIBizNETImageReview?WId=' + WId + '&SId=' + SId + '&PId=' + PId + '&Uid=' + Uid + '&MId=' + MId + '&AId=' + AId + '&VId=' + VId + '&HdrId=' + HdrId + '&DtlId=' + DtlId + '&ActivityID=' + ActivityID + '&NodeID=' + NodeId + '&ActivityDef=' + ActivityDef + '&iMySubjectNo=' + iMySubjectNo + '&ScreenNo=' + ScreenNo + '&ParentWorkSpaceId=' + ParentWorkSpaceId + '&PeriodId=' + PeriodId, '_blank');
                        window.open(DISoftURL + 'MIBizNETImageReview/MIBizNETImageReview?WId=' + WId + '&SId=' + SId + '&PId=' + PId + '&Uid=' + Uid + '&MId=' + MId + '&AId=' + AId + '&VId=' + VId + '&HdrId=' + HdrId + '&DtlId=' + DtlId + '&ActivityID=' + ActivityID + '&NodeID=' + NodeId + '&ActivityDef=' + ActivityDef + '&iMySubjectNo=' + iMySubjectNo + '&ScreenNo=' + ScreenNo + '&ParentWorkSpaceId=' + ParentWorkSpaceId + '&PeriodId=' + PeriodId, '_blank');

                    }
                    else {
                        msgalert("No Dicom Image Found For Given Activity Detail.!");
                        return false;
                    }
                }

                function errorviewDicom(e) {
                    throw e;
                }
            }

            function TablulerRepetationSetting() {
                document.getElementById("ProjectFieldSet").removeAttribute("style");
                document.getElementById("ProjectFieldSet").style.maxWidth = '98%';
                document.getElementById("ProjectFieldSet").style.minWidth = '98%';
                document.getElementById("ProjectFieldSet").style.width = '98%';
                $("#Third").show();
                //   $("#secound").hide();                  
                TablulerFormate();
                $("#gvActivityGrid_filter input").val("");
                $(".pageinate_input_box").attr("disabled", "disabled");
                // $(".ms-choice").attr("style", "width:449px;height:23px;");
                return false;
            }
            function TablulerFormate(DataRepetation, DataAttribute) {
                var TotalActivity;
                var ShowButton;
                var k = [];
                var p;
                var WorkspaceID = document.getElementById('HFWorkspaceId').value;
                var ActivityId = document.getElementById('HFActivityId').value;
                var SubjectId = document.getElementById('HFSubjectId').value;
                var Period = document.getElementById('HFPeriodId').value;
                var NodeId = document.getElementById('HFNodeId').value;
                var StatusTableBind = "true";

                $("#dvTabluer").css({ "max-width": $(window).width() * 0.95 });

                if (DataRepetation != undefined || DataAttribute != undefined) {
                    if ($("#gvActivityGrid").html() != "") {
                        $("#gvActivityGrid").dataTable().fnDestroy();
                        $("#gvActivityGrid").empty();
                    }
                    if (DataRepetation == "") {
                        RepetationList = "";
                    } else {
                        RepetationList = DataRepetation;
                    }
                    if (DataAttribute == "") {
                        AttributeList = "";
                    }
                    else {
                        AttributeList = DataAttribute;
                    }
                    StatusTableBind = "false";
                }
                else {
                    $("#ddlGridRepetation option").attr('selected', 'selected')
                    $("#ddlGridActivity option").attr('selected', 'selected')
                    $('.ms-search').parent().find('input:checkbox').attr('checked', 'checked');
                    $('.placeholder').html("All Selected");
                    RepetationList = "First";
                    AttributeList = "Secound";
                    $("#tblButtons").attr("style", "display:none");
                    EnableScrollY = "265px";
                }
                $.ajax({
                    type: "post",
                    url: "frmCTMMedExInfoHdrDtl.aspx/TablulerRepetationGrid",
                    data: '{"WorkspaceID" :"' + WorkspaceID + '","ActivityId":"' + ActivityId + '","SubjectId":"' + SubjectId + '","Period":"' + Period + '","NodeId":"' + NodeId + '"}',
                    contentType: "application/json; charset=utf-8",
                    datatype: JSON,
                    async: false,
                    dataType: "json",
                    success: function (data) {
                        if (RepetationList != "" && AttributeList != "") {
                            var aaDataSet = [];
                            var ActivityDataset = [];
                            var RowData = [];
                            var Rep = [];
                            var Attr = [];
                            var status = "";
                            var colData = "";
                            var addCol = "";
                            if (data.d != "" && data.d != null) {
                                data = JSON.parse(data.d);

                                //   get dynmmic column.
                                var headerCol = [];
                                var i = 0;
                                $.each(data[0], function (key, value) {
                                    if (DataRepetation != undefined || DataAttribute != undefined) {
                                        Attr = AttributeList.split("#");
                                        $.each(Attr, function (id, RAttr) {
                                            if (RAttr == key) {
                                                status = "true";
                                            }
                                        });
                                        if (status == "true" || key == "Repetition") {
                                            var obj = { sTitle: key };
                                            headerCol.push(obj);
                                            status = " ";
                                        }
                                    } else {
                                        var obj = { sTitle: key };
                                        headerCol.push(obj);
                                    }
                                });
                                // fetch all records from json result and make row data set.
                                var rowdataset = [];
                                var i = 0;
                                $.each(data, function (key, value) {
                                    var rowdata = [];
                                    var j = 0;
                                    status = " ";
                                    if (DataRepetation != undefined || DataAttribute != undefined) {
                                        Rep = RepetationList.split("#");
                                        $.each(Rep, function (index, Rdata) {
                                            $.each(value, function (k, par) {
                                                if (Rdata != "") {
                                                    if (Rdata == par) {
                                                        status = "true";
                                                    }
                                                }
                                            });
                                        });
                                        if (status == "true") {
                                            $.each(data[i], function (key, value) {
                                                addCol = "";
                                                $.each(Attr, function (h, ColName) {
                                                    if (ColName != "") {
                                                        if (key == ColName) {
                                                            addCol = "add";

                                                        }
                                                    }
                                                    else {
                                                        colData = "";
                                                    }
                                                });
                                                if (addCol == "add" || key == "Repetition") {
                                                    var Splitdata = value.split(",")
                                                    var n = Splitdata.length;
                                                    if (n > 1) {
                                                        var i = 0;
                                                        var valueCon = " ";
                                                        $.each(Splitdata, function (key, val) {
                                                            var strValue = Splitdata[i];
                                                            valueCon = valueCon + "  " + strValue + "  "
                                                            i++;
                                                        });
                                                        rowdata[j] = valueCon;
                                                        addCol = "";
                                                        if (n > 3) {
                                                            ShowButton = true;
                                                            k = j
                                                        }
                                                    }
                                                    else {
                                                        rowdata[j] = value;
                                                        status == " ";
                                                        addCol = "";
                                                    }
                                                    j++;
                                                }
                                            });
                                            if (rowdata != "") {
                                                rowdataset[i] = rowdata;
                                            }
                                            status == " ";
                                        }
                                    }
                                    else {
                                        $.each(data[i], function (key, value) {
                                            if (value != null) {
                                                var Splitdata = value.split(",");
                                                var n = Splitdata.length;
                                            } else {
                                                var n = value;
                                            }
                                            if (n > 1) {
                                                var i = 0;
                                                var valueCon = " ";
                                                $.each(Splitdata, function (key, val) {
                                                    var strValue = Splitdata[i];
                                                    valueCon = valueCon + "  " + strValue + "  "
                                                    i++;
                                                });
                                                rowdata[j] = valueCon;
                                                if (n > 3) {
                                                    ShowButton = true;
                                                    k[j] = j
                                                }
                                            }
                                            else {
                                                rowdata[j] = value;
                                            }
                                            j++;

                                        })
                                        rowdataset[i] = rowdata;
                                    }
                                    i++;
                                });
                                if (DataRepetation != undefined || DataAttribute != undefined) {
                                    var j = 0;
                                    $.each(rowdataset, function (i) {
                                        if (rowdataset[i] != undefined) {
                                            RowData[j] = rowdataset[i];
                                            j = j + 1;
                                        }
                                    });
                                    rowdataset = [];
                                    $.each(RowData, function (i) {
                                        rowdataset[i] = RowData[i];
                                    });
                                }

                                oTable = $('#gvActivityGrid').dataTable({
                                    "bSort": false,
                                    "jQueryUI": true,
                                    "bHeader": true,
                                    "bFilter": true,
                                    "bPaginate": true,
                                    "sPaginationType": "extStyle",
                                    "aaData": rowdataset,
                                    "scrollX": true,
                                    "fnCreatedRow": function (nRow, aData, iDataIndex) {
                                        $.each(aData, function (index) {
                                            if (aData[index] != null) {
                                                if (aData[index].length > 40) {
                                                    var abc = aData[index];
                                                    var lmn = abc.substring(1, 40);
                                                    $('td:eq(' + index + ')', nRow).html(""); // Modify by ketan 
                                                    $('td:eq(' + index + ')', nRow).append("<input type='image' id='imgExpand_" + iDataIndex + "' name='imgExpand$" + iDataIndex + "' title ='" + aData[index] + "' src='images/question.gif' disabled style='border-width:0px; padding-right:5px; '>" + lmn);   // Modify by ketan 
                                                    abc = "";
                                                    lmn = "";
                                                }
                                            }
                                        });
                                        var data = $('td:eq(0)', nRow).html();

                                        if (data.indexOf('title') > -1) {  // added by prayag grid not bind when Activity length>40
                                            data = $(data).attr('title');
                                        }

                                        var rep = data.split("_");
                                        $('td:eq(0)', nRow).html("");
                                        $('td:eq(0)', nRow).append("<a href='#' id='Repetition_" + rep[1] + "' onClick='return GenerateData(this);'>" + data + "</a>");
                                        if (StatusTableBind == "true") {
                                            bindPlaceMedEx(data);
                                        }
                                    },
                                    'aoColumns': headerCol,
                                    "oPaginate": {
                                        "sPrevious": "Prev",
                                        "sNext": "Next"
                                    },
                                    "scrollY": EnableScrollY,
                                    aLengthMenu: [
                                        [10, 50, 100, 200, 300, 400, 500, -1],
                                        [10, 50, 100, 200, 300, 400, 500, "All"]
                                    ],
                                    iDisplayLength: 10
                                });
                            }
                            return false;
                        }
                        else {
                            if (RepetationList == "" && AttributeList == "") {
                                msgalert("Please select at least one repeatation and Attribute");
                            }
                            else {
                                if (RepetationList == "") {
                                    msgalert("Please select at least one Repetition");
                                }
                                if (AttributeList == "") {
                                    msgalert("Please select at least one Attribute");
                                }
                            }
                        }
                        if ($("#ddlGridActivity option:selected").length < 11) {
                            $("#TablulerRepetiaton").removeAttr("style");
                            $("#ProjectFieldSet").removeAttr("style");
                            $("#TablulerRepetiaton").css({ "min-width": "98%" });
                            $("#ProjectFieldSet").css({ "min-width": "98%" });
                        }
                        if ($("#ddlGridActivity option:selected").length > 11) {
                            $("#TablulerRepetiaton").removeAttr("style");
                            $("#ProjectFieldSet").removeAttr("style");
                            $("#TablulerRepetiaton").css({ "max-width": "98%" });
                            $("#ProjectFieldSet").css({ "max-width": "98%" });
                        }
                    },
                    failure: function (response) {
                        msgalert(response.d);
                    },
                    error: function (response) {
                        msgalert(response.d);
                    }
                });

                /* Apply the tooltips */
                oTable.$('tr').tooltip({
                    "delay": 0,
                    "track": true,
                    "fade": 250
                });

                var abc = $("#gvActivityGrid").height();
                abc = abc + 28 + "px";
                $(".dataTables_scrollBody").removeAttr("style");
                var para = "position: relative; overflow: auto; width: 100%;height:" + abc;
                $(".dataTables_scrollBody").attr("style", para);

                return false;
            }

            $(document).ready(function () {
                $('.DataTables_sort_wrapper').each(function () {
                    var nTds = $('th', this);
                    var $td = $(this);
                    if ($td.text().length > 10) {
                        this.setAttribute('title', $td.text());
                    }

                });

            });

            function tablulerRepetaitonAlter() {
                msgalert("No Data found");
                return false;
            }
            function GenerateData(e) {
                var i = 1;
                var status = false;
                var Repeatation;
                var ProjectNo;
                var SubjectID;
                var Visit;
                var status;

                Repeatation = e.id.split("_");

                if ($("#<%=hndLetestData.ClientID%>").val() == "") {
                    Repeatation = e.id.split("_");
                } else {
                    $("#<%=hndLetestData.ClientID%>").val("");
                    if (e.id == undefined) {
                        Repeatation = e.split("_");
                    } else {
                        Repeatation = e.id.split("_");
                    }
                }

                $.each($("#lblHeader").html().split(","), function (i, para) {

                    if (para.split(":")[0].trim() == "Site No") {
                        ProjectNo = para.split(":")[1];
                    }
                    if (para.split(":")[0].trim() == "Screen No") {
                        SubjectID = para.split(":")[1];
                    }
                    if (para.split(":")[0].trim() == "Visit") {
                        Visit = para.split(":")[1];
                    }
                });

                $("#tblRepetition_" + Repeatation[1]).attr("class", "Selected");
                if ($("#tblRepetition_" + Repeatation[1]).html() == "") {
                    $("#tblRepetition_" + Repeatation[1]).html("Repetition is not submitted.");
                }
                $(".Repetition").css("display", "none");
                var ActivityName = $get('ddlActivities').options[$get('ddlActivities').selectedIndex].text;

                var header = "ProjectNo/Site No: " + ProjectNo + " , SubjectNo: " + SubjectID + " , Visit: " + Visit + " , Activity: " + ActivityName;
                $("#Td1").html(header);
                $("#Td1").attr("style", "color:Navy;font-family:Verdana;font-size:Small;font-weight:bold;");

                $find('mpDataEntry').show();

                $("#<%=hndtabluler.ClientID%>").val("Tabuler");
                $("#<%=hndLetestData.ClientID%>").val("Repeatation_" + Repeatation[1]);
                return false;
            }
            function ClearData() {
                var abc = $(".Selected").attr("id");
                var IdName = abc + " tr td";
                if ($(IdName).length == 1) {
                    $(IdName).html("");
                }
                $(".Repetition").removeAttr("style");
                $(".Selected").attr("class", "Repetition");
            }
            function bindTable() {
                var DataRepetation = [];
                var DataAttribute = [];
                var i = 1;
                var j = 1;
                IdentifyGridViewButton();
                $("#ddlGridRepetation option:selected").each(function () {

                    if ($("#ddlGridRepetation option:selected").length == i) {
                        DataRepetation += $(this).html();
                    } else {
                        DataRepetation += $(this).html() + "#";
                    }
                    i = i + 1;
                });
                $("#ddlGridActivity option:selected").each(function () {
                    if ($("#ddlGridActivity option:selected").length == j) {
                        DataAttribute += $(this).html();
                    } else {
                        DataAttribute += $(this).html() + "#";
                    }
                    j = j + 1;
                });

                TablulerFormate(DataRepetation, DataAttribute);
                $("#gvActivityGrid_filter input").val("");
                $(".pageinate_input_box").attr("disabled", "disabled");
                return false;
            }
            function bindPlaceMedEx(e) {
                if ($("#hidDV").html().length == 0) {
                    $('#tblMain div').each(function () {
                        DivIdl = $(this).attr("id");
                    });
                    $("#PnlPlaceMedex").css({ "display": "none" });
                }
                if ($("#dvModelDatEntry").html().length == 0) {
                    $("#dvModelDatEntry").append("<div id='" + DivIdl + "' width='100%'></div>");
                }
                abc = e.split("_");
                $("#dvModelDatEntry div").append("<table id='tblRepetition_" + abc[1] + "' width='100%' class='Repetition'></table>")

                $('#tblRepetition_' + abc[1]).append($("tr[id=Repetition_" + abc[1] + "]"));

                $('#tblRepetition_' + abc[1]).find("[title=Edit]").attr("disabled", "true");
                $('#tblRepetition_' + abc[1]).find("[title=Update]").attr("disabled", "true");
            }
            function getContentData() {
                if ($('#gvActivityGrid tr td').length != 0) {
                    $("#secound").css({ display: "block" });
                    $find('mpDataEntry').hide();
                    $("#tblMain").css({ display: "none" });
                }
            }
            function setData() {
                document.getElementById('<%=btnGridViewDisplay.ClientID%>').click();
            }
            function IdentifyGridViewButton() {
                $("#<%=hndGridViewStatus.ClientID%>").val("Grid");
            }
            function buttonSettings() {
                $("#btnSaveAndContinue").css({ "display": "none" });
                $("#BtnSave").css({ "display": "none" });
            }
            jQuery(document).ready(function () {
                jQuery('#gvActivityGrid').
                      on('click', 'tr', function () {
                          if ($("[hasColorRow]").length > 0) {
                              $("[hasColorRow]").removeAttr("style");
                              $("[hasColorRow]").find("a").removeAttr("style")
                          }
                          $(this).css({ "background-color": "#FFA55F", "color": "white" });
                          $(this).find("a").css({ "background-color": "#FFA55F", "color": "white" });
                          $(this).attr("hasColorRow", "yes");
                      });
            });
            // add by shivani pandya For theme selection          
            jQuery(window).focus(function () {
                selectTheme();
                return false;
            });

            // add by shivani pandya For theme selection
            jQuery("#imgTheme").click(function () {
                jQuery("#divThemeSelection").removeAttr("style");
                jQuery("#divThemeSelection").show();
                return false;
            });

            // add by shivani pandya For theme selection
            function hideDiv() {
                jQuery("#divThemeSelection").hide();
                jQuery("#divThemeSelection").attr("style", "display:none");
            }

            jQuery(document).click(function () {
                jQuery("#divThemeSelection").hide();
                jQuery("#divThemeSelection").attr("style", "display:none");
            })

            jQuery("#lblornage").click(function () {
                document.cookie = "Theme=Orange";
                selectTheme();
                return false;
            });
            jQuery("#lblGreen").click(function () {
                document.cookie = "Theme=Green";
                selectTheme();
                return false;
            });
            jQuery("#lblDemo").click(function () {
                document.cookie = "Theme=Demo";
                selectTheme();
                return false;
            });
            jQuery("#lblBlue").click(function () {
                document.cookie = "Theme=Blue";
                selectTheme();
                return false;
            });
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
                    if ($("#ctl00_CPHLAMBDA_pnlMedExGrid") != null) {
                        $("#ctl00_CPHLAMBDA_pnlMedExGrid table tr").last().css({ "background-color": "#CF8E4C" });
                    }
                    if ($("#ctl00_CPHLAMBDA_GVHabits") != null) {
                        $("#ctl00_CPHLAMBDA_GVHabits tr").last().css({ "background-color": "#CF8E4C" });
                    }
                    if ($("#ctl00_CPHLAMBDA_GV_PreviewAtrributeTemplate") != null) {
                        $(".trFooter").prev().css({ "background-color": "#CF8E4C" });
                        $(".trFooter").prev().css({ "display": "none" });
                    }
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

                    if ($("#ctl00_CPHLAMBDA_pnlMedExGrid") != null) {
                        $("#ctl00_CPHLAMBDA_pnlMedExGrid table tr").last().css({ "background-color": "#33a047" });
                    }
                    if ($("#ctl00_CPHLAMBDA_GV_PreviewAtrributeTemplate") != null) {
                        $(".trFooter").prev().css({ "background-color": "#33a047" });
                        $(".trFooter").prev().css({ "display": "none" });
                    }
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

                    if ($("#ctl00_CPHLAMBDA_pnlMedExGrid") != null) {
                        $("#ctl00_CPHLAMBDA_pnlMedExGrid table tr").last().css({ "background-color": "#999966" });
                    }
                    if ($("#ctl00_CPHLAMBDA_GV_PreviewAtrributeTemplate") != null) {
                        $(".trFooter").prev().css({ "background-color": "#999966" });
                        $(".trFooter").prev().css({ "display": "none" });
                    }
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

                    if ($("#ctl00_CPHLAMBDA_GVHabits") != null) {
                        $("#ctl00_CPHLAMBDA_GVHabits tr").last().css({ "background-color": "Navy" });
                    }
                    if ($("#ctl00_CPHLAMBDA_GV_PreviewAtrributeTemplate") != null) {
                        $(".trFooter").prev().css({ "background-color": "Navy" });
                        $(".trFooter").prev().css({ "display": "none" });
                    }
                }
                return false;
            }
            $("#ddlRepeatNo").next().first().find(".ms-drop li").next().find("input").click(function () {
                var FlagData = false;
                if ($(this).attr("checked") == undefined) {
                    $(this).removeAttr("Checked");
                    $("#ddlRepeatNo [value=" + $(this).attr("value") + " ]").removeAttr("selected");
                } else {
                    $(this).attr("Checked", "checked");
                    $("#ddlRepeatNo [value=" + $(this).attr("value") + " ]").attr("selected", "selected");
                    var abc = $(this.parentNode).attr("style");
                    if ($(this).val() != "N") {
                        var hhh = abc.split(":")[1];
                        if (hhh == "Orange") {
                            $("#ddlRepeatNo").next().find("button").css({ 'color': 'Orange', 'font-weight': 'bold', 'font-family': 'Verdana, Arial, Helvetica, sans-serif' });
                        }
                        if (hhh == "Red") {
                            $("#ddlRepeatNo").next().find("button").css({ 'color': 'Red', 'font-weight': 'bold', 'font-family': 'Verdana, Arial, Helvetica, sans-serif' });
                        }
                        if (hhh == "Blue") {
                            $("#ddlRepeatNo").next().find("button").css({ 'color': 'Blue', 'font-weight': 'bold', 'font-family': 'Verdana, Arial, Helvetica, sans-serif' });
                        }
                        if (hhh == "#800080") {
                            $("#ddlRepeatNo").next().find("button").css({ 'color': '#800080', 'font-weight': 'bold', 'font-family': 'Verdana, Arial, Helvetica, sans-serif' });
                        }
                        if (hhh == "#006000") {
                            $("#ddlRepeatNo").next().find("button").css({ 'color': '#006000', 'font-weight': 'bold', 'font-family': 'Verdana, Arial, Helvetica, sans-serif' });
                        }
                    }
                }
                $("#ddlRepeatNo").next().first().find(".ms-drop li").next().find("input").each(function () {
                    if ($(this).parent().html().split(">")[1] != " New Repeatation") {
                        if ($(this).attr("checked") == "checked") {
                            FlagData = true;
                        }
                    }
                });
                if (FlagData == true && $("#ddlRepeatNo").next().first().find(".ms-drop li").next().first().find("input").attr("checked") == "checked" && $("#ddlRepeatNo").next().first().find(".ms-drop li").next().first().find("label").html().split(">")[1] == " New Repeatation") {
                    msgalert("You can not select New Repeatation with existing selected Repeatation");
                    $(this).removeAttr("checked");
                    $("#ddlRepeatNo [value=" + $(this).attr("value") + " ]").removeAttr("selected");
                    return false;
                }
            });

            function getSelectedItem() {
                var abc = "";
                var FlagChk = false;
                $("#ddlRepeatNo").next().first().find(".ms-drop li").first().removeAttr("checked");
                $("#ddlRepeatNo").next().find(".ms-drop li").each(function () {
                    if ($(this).find("input").attr("checked") == "checked") {
                        FlagChk = true;
                    }
                });
                if (FlagChk == false) {
                    msgalert("Please select atleast one Repeatation");
                    return false;
                    $('#ddlRepeatNo').children('option').each(function () {
                        if ($(this).attr("selected") = "selected") {
                            $(this).removeAttr("selected");
                        }
                    });
                    return false;
                }
                if (FlagChk == true) {
                    $('#ddlRepeatNo [selected=selected]').each(function (index) {
                        if (index != $('#ddlRepeatNo [selected=selected]').length - 1) {
                            abc = abc + $(this).attr("value") + ",";
                        } else {
                            abc = abc + $(this).attr("value");
                        }
                    });
                    $("#<%=hndRepetitionNo.ClientID%>").val(abc);
                }
                return true;
            }
            function getRemarks() {
                // $("#divDCF").css({ 'display': 'none' })
                //$("#txtDCFUpdateRemarks").val("")  //remove by prayag
                $find('mdpDCFUpdateRemarks').show();
                $("#txtDiscrepancyRemarks").val("");
            }
            function CheckDCFUpdateRemarksValidation() {
                if (document.getElementById('ddlDirectUpdateRemarks').selectedIndex == '0') {
                    if (document.getElementById('txtDCFUpdateRemarks').value.trim() == '') {
                        msgalert('Select Either Reason Or Specify Remarks.');
                        document.getElementById('ddlDirectUpdateRemarks').selectedIndex = 0;
                        document.getElementById('txtDCFUpdateRemarks').value = '';
                        return false;
                    }
                    else {
                        try {
                            if (document.getElementById('ddlDirectUpdateRemarks').selectedIndex != '0' && document.getElementById('txtDCFUpdateRemarks').value.trim() != '') {
                                msgalert('Select Either Reason Or Specify Remarks.');
                                document.getElementById('ddlDirectUpdateRemarks').selectedIndex = 0;
                                document.getElementById('txtRemarkForAttributeEdit').value = '';
                                return false;

                            }
                            else {
                                //document.getElementById('HdReasonDesc').value = document.getElementById('txtDCFUpdateRemarks').value;
                                //document.getElementById('divForEditAttribute').style.display = 'none';
                                //document.getElementById('ModalBackGround').style.display = 'none';
                                //if (document.getElementById('HdReasonDesc').value == 'undefined')
                                //{ throw "Undefined value" }
                                //return true;

                                document.getElementById('HdReasonDesc').value = document.getElementById('ddlDirectUpdateRemarks').options[document.getElementById('ddlDirectUpdateRemarks').selectedIndex].innerHTML;
                            }
                        }
                        catch (err) {
                            msgalert('An error has occurred: ' + err.message);
                            return false;
                        }

                    }

                }
                else if (document.getElementById('txtRemarkForAttributeEdit').value.trim() != '') {
                    msgalert('Select Either Reason Or Specify Remarks.');
                    document.getElementById('ddlDirectUpdateRemarks').selectedIndex = 0;
                    document.getElementById('txtRemarkForAttributeEdit').value = '';
                    return false;
                }
                else {
                    try {

                        if (document.getElementById('ddlDirectUpdateRemarks').selectedIndex != '0' && document.getElementById('txtDCFUpdateRemarks').value.trim() != '') {
                            msgalert('Select Either Reason Or Specify Remarks.');
                            document.getElementById('ddlDirectUpdateRemarks').selectedIndex = 0;
                            document.getElementById('txtDCFUpdateRemarks').value = '';
                            return false;

                        }
                        else {

                            //document.getElementById('HdReasonDesc').value = document.getElementById('DdlEditRemarks').options[document.getElementById('DdlEditRemarks').selectedIndex].innerHTML;
                            //document.getElementById('ddlDirectUpdateRemarks').style.display = 'none';
                            //document.getElementById('ModalBackGround').style.display = 'none';
                            document.getElementById('HdReasonDesc').value = document.getElementById('ddlDirectUpdateRemarks').options[document.getElementById('ddlDirectUpdateRemarks').selectedIndex].innerHTML;
                            if (document.getElementById('HdReasonDesc').value == 'undefined')
                            { throw "Undefined value" }
                            return true;
                        }

                    }
                    catch (err) {
                        msgalert('An error has occurred: ' + err.message);
                        return false;
                    }

                }

                //if ($("#txtDCFUpdateRemarks").val() == "") {
                //    msgalert("Please enter update remarks");
                //    return false;
                //}
                return true;
            }

            function ClearDCFUpdateRemarks() {
                $("#txtDCFUpdateRemarks").val("");
                $find('mdpDCFUpdateRemarks').hide();
                document.getElementById('ddlDiscrepancyStatus').selectedIndex = 0;
            }
            function DisplayData() {
                $("#divDCF").css({ 'display': 'none' });
                var styleEle = $(".divModalBackGround").attr("style");
                $(".divModalBackGround").removeAttr("style")
                $(".divModalBackGround").attr("style", styleEle);
                return true;
            }

            function ValidationForEditOrDelete() {

                // $("#<%=hndGridViewStatus.ClientID%>").val("Grid");


                if (document.getElementById('DdlEditRemarks').selectedIndex == '0') {
                    if (document.getElementById('txtRemarkForAttributeEdit').value.trim() == '') {
                        msgalert('Select Either Reason Or Specify Remarks.');
                        document.getElementById('DdlEditRemarks').selectedIndex = 0;
                        document.getElementById('txtRemarkForAttributeEdit').value = '';
                        return false;
                    }
                    else {
                        try {
                            document.getElementById('HdReasonDesc').value = document.getElementById('txtRemarkForAttributeEdit').value;
                            document.getElementById('divForEditAttribute').style.display = 'none';
                            document.getElementById('ModalBackGround').style.display = 'none';
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
                else if (document.getElementById('txtRemarkForAttributeEdit').value.trim() != '') {
                    msgalert('Select Either Reason Or Specify Remarks.');
                    document.getElementById('DdlEditRemarks').selectedIndex = 0;
                    document.getElementById('txtRemarkForAttributeEdit').value = '';
                    return false;
                }
                else {
                    try {
                        document.getElementById('HdReasonDesc').value = document.getElementById('DdlEditRemarks').options[document.getElementById('DdlEditRemarks').selectedIndex].innerHTML;
                        document.getElementById('divForEditAttribute').style.display = 'none';
                        document.getElementById('ModalBackGround').style.display = 'none';
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
            //Add by shivani pandya for Repeatiton dropdown color
            function getRepeatitionDropDownColor() {
                $.ajax({
                    type: "post",
                    url: "frmCTMMedExInfoHdrDtl.aspx/getRepeatitionColor",
                    data: '{}',
                    contentType: "application/json; charset=utf-8",
                    datatype: JSON,
                    async: false,
                    success: function (data) {
                        if (data.d != "") {
                            var valueRep = data.d.split(",");
                            for (var index = 0; index <= valueRep.length; index++) {
                                if (valueRep[index] != undefined) {
                                    if (valueRep[index].length != 1) {
                                        var colrRep = valueRep[index].split(":");
                                        if (valueRep.length != index + 1) {
                                            if (index != 0) {
                                                $("#ddlRepeatNo").next().find("div").last().find("label [value=" + colrRep[0] + "]").parent().attr("style", "color:" + colrRep[1] + "");
                                                $("#ddlRepeatNo").next().find("div").last().find("label [value=" + colrRep[0] + "]").parent().css({ 'font-weight': 'bold' });
                                            }
                                            $("#ddlRepeatNo").next().find("div").last().find("[checked=checked]").each(function () {
                                                if ($(this).attr("value") == colrRep[0]) {
                                                    if (colrRep[1] == "Orange") {
                                                        $("#ddlRepeatNo").next().find("button").css({ 'color': 'Orange', 'font-weight': 'bold', 'font-family': 'Verdana, Arial, Helvetica, sans-serif' });
                                                    }
                                                    if (colrRep[1] == "Red") {
                                                        $("#ddlRepeatNo").next().find("button").css({ 'color': 'Red', 'font-weight': 'bold', 'font-family': 'Verdana, Arial, Helvetica, sans-serif' });
                                                    }
                                                    if (colrRep[1] == "Blue") {
                                                        $("#ddlRepeatNo").next().find("button").css({ 'color': 'Blue', 'font-weight': 'bold', 'font-family': 'Verdana, Arial, Helvetica, sans-serif' });
                                                    }
                                                    if (colrRep[1] == "#800080") {
                                                        $("#ddlRepeatNo").next().find("button").css({ 'color': '#800080', 'font-weight': 'bold', 'font-family': 'Verdana, Arial, Helvetica, sans-serif' });
                                                    }
                                                    if (colrRep[1] == "#006000") {
                                                        $("#ddlRepeatNo").next().find("button").css({ 'color': '#006000', 'font-weight': 'bold', 'font-family': 'Verdana, Arial, Helvetica, sans-serif' });
                                                    }
                                                }
                                            });
                                        }
                                    }
                                }
                            }

                        }
                    }
                });
            }
            function LengthValidation(str1, str, txt) {
                var length12 = document.getElementById(txt.id).value.length
                if (parseInt(str1) + parseInt(1) <= length12) {
                    return false
                }
                return true
            }
            function RepeatitionShowHide() {
                $("#ddlRepeatNo").next().first().find(".ms-drop li").first().attr("style", "display:none");
                $("#ddlRepeatNo").next().next().attr("style", "display:none ! Important;");
                $("#ddlRepeatNo").next().first().find(".ms-drop li").first().attr("Visible", "false");
                $(".ms-choice").attr("style", "display:none ! Important;");
                $(".ms-choice").next().next().attr("style", "display:none ! Important;");


            }

            setInterval(function () {
                var cookiCurrentPro;
                Profile = $("#hdnCookiesCurrentProfile").val();
                if (IsExisitingCookie("currentProfile") > 1)
                    setCookie("currentProfile", "", -1);

                if (!IsExisitingCookie("currentProfile")) {
                    setCookie("currentProfile", Profile, 30);
                }
                cookiCurrentPro = getCookie("currentProfile");

                if (cookiCurrentPro != undefined && cookiCurrentPro != Profile) {
                    //Profile.selectedIndex = cookiCurrentPro;
                    if (document.location.pathname.indexOf("Interface") > 0) {
                        window.location.reload();
                        //document.location.href = "../Default.aspx"
                    }
                    else {
                        window.location.reload();
                        //document.location.href = "Default.aspx";
                    }
                }
            }, 1000)

            function IsExisitingCookie(cname) {
                var allcookies = document.cookie;
                var count = 0;
                cookiearray = allcookies.split(';');
                for (var i = 0; i < cookiearray.length; i++) {
                    name = cookiearray[i].split('=')[0];
                    value = cookiearray[i].split('=')[1];
                    if (name.trimLeft().toUpperCase() == cname.trim().toUpperCase()) {
                        count = count + 1;
                    }
                }
                return count;
            }

            function setCookie(cname, cvalue, exdays) {
                var d = new Date();
                d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
                var expires = "expires=" + d.toGMTString();
                document.cookie = cname + "=" + cvalue + "; " + expires;
            }

            function getCookie(cname) {
                var name = cname + "=";
                var ca = document.cookie.split(';');
                for (var i = 0; i < ca.length; i++) {
                    var c = ca[i];
                    while (c.charAt(0) == ' ') c = c.substring(1);
                    if (c.indexOf(name) == 0) {
                        return c.substring(name.length, c.length);
                    }
                }
                return "";
            }
            function getCookieDynamicPage(cname) {
                var name = cname + "=";
                var ca = document.cookie.split(';');
                for (var i = 0; i < ca.length; i++) {
                    var c = ca[i];
                    while (c.charAt(0) == ' ') c = c.substring(1);
                    if (c.indexOf(name) == 0) {
                        $("#hdnCookiesCurrentProfile").val(c.substring(name.length, c.length));
                        msgalert(c.substring(name.length, c.length));
                        //$("#hdnCookiesCurrentProfile").value(c.substring(name.length, c.length));
                    }
                }
                return "";
            }

            //Added by Shyam Kamdar for Signature Autentication popup
            function CloseAuthentication() {
                document.getElementById('<%=myModalSignAuth.ClientID()%>').style.display = 'none';
            }

             function SignAuthModalClose() {
                document.getElementById('<%=myModalSignAuth.ClientID()%>').style.display = 'none';
            }

            function fnCheckPersonalInformation(obj) {
                var radiolst = obj;
                var radios;
                var i;
                if (radiolst != null && typeof (radiolst) != 'undefined') {
                    radios = radiolst.getElementsByTagName('input');
                    for (i = 0; i < radios.length; i++) {
                        if (radios[i].type.toUpperCase() == 'RADIO') {
                            if (radios[i].checked == true) {
                                //alert(radios[i].value);
                                if (radios[i].value == "Y") {
                                    $("input[name=rblApprovalStatus]").val(['R']);
                                    $("input[name=rblApprovalStatus]").attr('disabled', true);
                                    $("#lblRemarks").text("Remarks* :");
                                }
                                else {
                                    $("input[name=rblApprovalStatus]").val(['']);
                                    $("input[name=rblApprovalStatus]").removeAttr('disabled', false);
                                    $("#lblRemarks").text("Remarks :");
                                }
                            }
                        }
                    }
                }
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
                                //alert(radios[i].value);
                                if (radios[i].value == "R") {
                                    $("#lblRemarks").text("Remarks* :");
                                }
                                else {
                                    $("#lblRemarks").text("Remarks :");
                                }
                            }
                        }
                    }
                }
            }

            function ValidationForQCAuthentication() {
                //if ($("input:radio[name='rblPersonalInfo']:checked").val() == undefined && document.getElementById('FollowVisitHidden').style.display !== 'none') {
                //    msgalert('Please confirm Image having personal information?');
                //    return false; 
                //}
                if ($("input:radio[name='rblPersonalInfo']:checked").val() == undefined) {
                    msgalert('Please confirm Image having personal information?');
                    return false;
                }
                if ($("input:radio[name='rblApprovalStatus']:checked").val() == undefined) {
                    msgalert('Please Select Approval Status.');
                    return false;
                }
                if ($("input:radio[name='rblApprovalStatus']:checked").val() == "R" && document.getElementById('txtQCRemarks').value.trim() == '') {
                    document.getElementById('txtQCRemarks').value = '';
                    msgalert('Please Enter Remarks.');
                    document.getElementById('txtQCRemarks').focus();
                    return false;
                }

                if (document.getElementById('txtPasswords').value.trim() == '') {
                    document.getElementById('txtPasswords').value = '';
                    msgalert('Please Enter Password For Authentication.');
                    document.getElementById('txtPasswords').focus();
                    return false;
                }
                return true;
            }

            function QueryDetailsClose() {
                document.getElementById("QueryDiv").style.display = 'none';
            }
            function QueryDetailsFn() {
                document.getElementById("QueryDiv").style.display = 'block';
            }
            function QueryValidation() {
                debugger;
                //if (document.getElementById('CheckReUpload').checked) {
                //    document.getElementById('CheckReUpload') == 'R'
                //}
                //else {
                //    document.getElementById('CheckReUpload') == 'A'
                //}
                

              if(document.getElementById("<%=txtRemarkQuery.ClientID%>").value.trim() == "") {
                    msgalert("Please Enter Remark.!");
                    document.getElementById('<%=txtRemarkQuery.ClientID%>').focus();
                    return false;
            }
    return true;
            }

        </script>
    </form>
</body>
    <script type="text/javascript" src="Script/Login/third-party.min.js"></script>
    <script type="text/javascript" src="Script/Login/modules.min.js"></script>
</html>

