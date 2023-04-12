<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmMonitorReport.aspx.vb" Inherits="frmReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <style type="text/css">
        #ctl00_CPHLAMBDA_menu
        {
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#1e5799' , endColorstr= '#7db9e8' ,GradientType=1 ); /* IE6-9 fallback on horizontal gradient */
            background-color: #1e5799;
            list-style: none;
            cursor: pointer;
        }
        #ctl00_CPHLAMBDA_menu ul
        {
            left: 0;
            right: 0;
        }
        #ctl00_CPHLAMBDA_menu li
        {
            float: none;
            padding-bottom: 15px;
            caption-side: top;
            display: block;
            text-decoration: none;
            color: white;
            font-size: 14px;
            font-weight: bold;
            cursor: pointer;
        }
        #ctl00_CPHLAMBDA_Report
        {
            width: 15px;
            height: 100px;
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr=  '#7ebae8' , endColorstr= '#9fc5dd' ,GradientType=1 ); /* IE6-9 fallback on horizontal gradient */
            background-color: #7ebae8;
            color: White;
            margin-top: 100px;
            padding-left: 5px;
            z-index: 2000;
            position: absolute;
            font-weight: bold;
        }
        #accordion
        {
            /*background-color: #eee;*/
            border: 1px solid #ccc;
           <%-- width: 700px;--%>
            padding: 5px 10px 5px 10px;
            margin: 5px auto;
            -moz-border-radius: 3px;
            -webkit-border-radius: 3px;
            border-radius: 3px;
            box-shadow: 0 1px 0 #999;
            list-style: none;
        }
        #accordion div
        {
            float: none;
            display: block;
            background-color: #1560a1;
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr=                                 '#1560a1' , endColorstr= '#2372b2' ,GradientType=1 ); /*IE6-9 */
            font-family: Verdana;
            font-size: "11px";
            margin: 1px;
            cursor: pointer;
            padding: 5px 10px 5px 10px;
            list-style: circle;
            color: White;
            font-weight: bold;
        }
        #accordion ul
        {
            list-style: none;
            display: none;
            padding: 0 5 0 5;
        }
        #accordion ul ul li div
        {
            cursor: pointer;
            caption-side: top;
            display: block; /*#A4E4FF*/
            background-color: #A4E4FF;
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr= '#A4E4FF' , endColorstr= '#CEE3ED' ,GradientType=1 ); /* IE6-9 fallback on horizontal gradient */
            padding: 5px 10px 5px 10px;
            text-decoration: none;
            color: White;
            font-weight: bold;
        }
        #accordion div:hover
        {
            font-weight: bolder;
            font-style: italic;
            cursor: pointer;
        }
        #accordion ul li div
        {
            caption-side: top;
            display: block;
            background: #3e88c4;
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr=                                           '#3e88c4' , endColorstr= '#60a0d1' ,GradientType=1 ); /* IE6-9 fallback on horizontal gradient */
            padding: 5px 10px 5px 10px;
            text-decoration: none;
            color: White;
            cursor: auto;
        }
        #ctl00_CPHLAMBDA_DvChilds
        {
            list-style: none;
            margin: 3px 0;
            padding: 0;
            height: 200px;
            overflow: auto;
            height: 500px;
            scrollbar-arrow-color: white;
            scrollbar-face-color: #7db9e8;
        }
        #ctl00_CPHLAMBDA_DivMPECTM
        {
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr=                                  '#1560a1' , endColorstr= '#2372b2' ,GradientType=1 ); /* IE6-9 */
            background-color: #1560a1;
            color: White;
            text-align: left;
        }
        #rblCTMWorkSpaceId td
        {
            padding: 5px;
        }
    </style>
    <asp:ScriptManagerProxy runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="ProjectData" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table width="100%" cellpadding="5">
                                    <tr>
                                        <td class="Label" nowrap="nowrap" style="text-align: right; width: 30%;">
                                            Project* :
                                        </td>
                                        <td class="Label" style="text-align: left;" colspan="2">
                                            <asp:TextBox ID="txtProject" runat="server" CssClass="textBox" Width="65%" MaxLength="50"></asp:TextBox>
                                            <asp:HiddenField ID="HProjectId" runat="server" />
                                            <asp:HiddenField ID="HIsRefresh" runat="server" />
                                            <asp:HiddenField ID="HParentNode" runat="server" />
                                            <asp:Button Style="display: none" ID="btnSetProject" runat="server" Text=" Project">
                                            </asp:Button>
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                                CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                                OnClientShowing="ClientPopulated" ServiceMethod="GetMyProjectCompletionList"
                                                ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True"
                                                CompletionListElementID="pnlProjectList">
                                            </cc1:AutoCompleteExtender>
                                            <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto;
                                                overflow-x: hidden" />
                                            <asp:CheckBox runat="server" ID="ChkBClient" Style="display: none;" Text="All Sites" />
                                        </td>
                                    </tr>
                                    <tr id="Tr1" runat="server">
                                        <td colspan="3" style="white-space: nowrap;" class="Label" valign="middle" align="right">
                                            <div style="border-right: black 2px outset; border-top: black 2px outset; display: none;
                                                font-size: 8pt; border-left: black 2px outset; border-bottom: black 2px outset;
                                                height: auto; background-color: white; text-align: left; width: 660px;" id="divActivityLegends"
                                                runat="server">
                                                <asp:Label ID="lblGray" runat="Server" BackColor="Black">&nbsp;&nbsp;&nbsp;</asp:Label>-Data
                                                Entry Pending,
                                                <asp:Label ID="lblOrange" runat="Server" BackColor="orange">&nbsp;&nbsp;&nbsp;</asp:Label>-Data
                                                Entry Continue,
                                                <asp:Label ID="lblBlue" runat="Server" BackColor="blue">&nbsp;&nbsp;&nbsp;</asp:Label>-Ready
                                                For Review,
                                                <asp:Label ID="lblGreen" runat="Server" BackColor="#50C000">&nbsp;&nbsp;&nbsp;</asp:Label>-Review
                                                <asp:Label ID="lblRed" runat="Server" BackColor="red">&nbsp;&nbsp;&nbsp;</asp:Label>-Discontinued
                                                Subjects
                                            </div>
                                            &nbsp;Legends&nbsp;<img alt="" id="imgActivityLegends" src="images/question.gif"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <table id="MonitorInfo" runat="server" style="display: none;">
                                    <tr>
                                        <td style="vertical-align: top;" id="tdReport" runat="server">
                                            <div runat="server" id="DvSideMenu">
                                                <div id="menu" style="height: 300px; border: solid 3px silver; width: 22%; color: White;
                                                    text-align: left; position: absolute; display: none;" runat="server">
                                                    <%-- <ul>
                                                        <li style="font-weight: bolder;"><u>REPORTS</u>
                                                            <img id="Img2" alt="CLoseImage" src="images/close.gif" runat="server" align="right"
                                                                style="padding-right: 5px" onclick="funCloseReport();" />
                                                        </li>
                                                        <li onclick="funReport('frmReportReview.aspx?mode=4&Type=RPT&vWorkSpaceId=','0')">Lab
                                                            Report </li>
                                                        <li onclick="funReport('frmSourceQA.aspx?Mode=4&Type=RPT&Act=QA on PIF&vActivityId= 1186&WorkSpaceId=','2')">
                                                            PIF</li>
                                                        <li onclick="funReport('frmSourceQA.aspx?mode=4&Type=RPT&Act=QA on MSR&vActivityId= 1185&WorkSpaceId=','1')">
                                                            Screening</li>
                                                        <li onclick="funReport('frmWorkspaceSubjectMst.aspx?mode=4&Type=RPT&WorkSpaceId=','0')">
                                                            Attendance</li>
                                                        <li onclick="funReport('frmCTMCRFReport.aspx?mode=4&Type=RPT&vWorkSpaceId=','0')">Listing
                                                            Report</li>
                                                        <li onclick="funReport('frmActivityDeviationReport.aspx?mode=4&vWorkSpaceId=','0')">
                                                            Activity Deviation Report</li>
                                                        <li onclick="funReport('frmEditChecksReport.aspx?mode=4&vWorkSpaceId=','0')">Edit Check
                                                            Execution Report</li>
                                                        <li onclick="funReport('frmCTMDiscrepancyStatusReport.aspx?mode=4&vWorkSpaceId=','0')">
                                                            Discrepancy Report</li>
                                                    </ul>--%>
                                                </div>
                                                <div id="Report" runat="server" onmouseover="funOnMouseOver(this);" onclick="funOnMouseclick(this);">
                                                    R E P O R T
                                                </div>
                                            </div>
                                        </td>
                                        <td style="vertical-align: top;">
                                            <table align="left" cellpadding="5" style="margin-left: 9%; vertical-align: top;"
                                                id="tblProjectInfo" runat="server" width="910px">
                                                <tr>
                                                    <td style="padding-left: 82px;" id="tdProjectInfo" runat="server">
                                                        <fieldset id="fProjectInfo" runat="server" style="width: 80%; height: auto; text-align: left;">
                                                            <legend id="lProjectInfo" runat="server">
                                                                <img id="imgExpand" alt="SubjectSpecific" src="images/expand.jpg" onclick="displayProjectInfo(this);" />
                                                                <asp:Label ID="lblProjectInfo" runat="server" Text="Project Information" CssClass="Label">
                                                                </asp:Label>
                                                            </legend>
                                                            <div id="ProjectInfo" style="display: none; margin: 5px;">
                                                                <table width="100%">
                                                                    <%--<tr>
                                                                        <td style="width: 19%">
                                                                            <asp:Label runat="server" ID="lblmgr" Text="Project Manager : " CssClass="Label" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblManager" class="Label" runat="server" CssClass="labeldisplay" />
                                                                        </td>
                                                                        <td align="right">
                                                                            <asp:Label runat="server" ID="lblPIMonitor" CssClass="Label" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblPI" Style="width: 50%" class="Label" runat="server" CssClass="labeldisplay" />
                                                                        </td>
                                                                    </tr>--%>
                                                                    <%--<tr>
                                                                        <td align="center" colspan="4">
                                                                            <hr style="color: Navy; width: 100%;" />
                                                                        </td>
                                                                    </tr>--%>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="LblSubs" Text="Subjects : " CssClass="Label" />
                                                                        </td>
                                                                        <td colspan="3">
                                                                            <asp:PlaceHolder ID="PlaceSubsInfo" runat="server" EnableViewState="true"></asp:PlaceHolder>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </fieldset>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-left: 82px;" id="tdProjectActivity" runat="server">
                                                        <fieldset id="fProjectActivity" runat="server" style="width: 80%; text-align: left;">
                                                            <legend id="lProjectActivity" runat="server">
                                                                <img id="img1" alt="SubjectSpecific" src="images/expand.jpg" onclick="displayProjectActivity(this),funParentActivity(this);" />
                                                                <asp:Label ID="lblProjectActivity" runat="server" Text="Project Activity" CssClass="Label">
                                                                </asp:Label>
                                                            </legend>
                                                            <div id="ProjectActivity" style="display: none; overflow: auto; height: 450px; scrollbar-arrow-color: white;
                                                                scrollbar-face-color: #7db9e8;">
                                                            </div>
                                                        </fieldset>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-left: 82px;">
                                                        <div runat="server" id="DvChilds" style="display: none;">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div>
                                                <asp:Button ID="btnShow" runat="server" Text="Show Dialog" Style="display: none" />
                                                <cc1:ModalPopupExtender ID="MPEActivitySequence" runat="server" TargetControlID="btnShow"
                                                    PopupControlID="DivMPECTM" BackgroundCssClass="modalBackground" PopupDragHandleControlID="DivMPECTM"
                                                    BehaviorID="MPEId">
                                                </cc1:ModalPopupExtender>
                                                <div id="DivMPECTM" runat="server" class="centerModalPopup" style="display: none;
                                                    width: 800px; max-height: 600px">
                                                    <div style="width: 100%">
                                                        <h1 class="header">
                                                            <label id="lblDocAction" class="LabelBold ">
                                                                For Which Site Do You Want To See Report?
                                                            </label>
                                                            <img id="ImgPopUpClose" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';"
                                                                onclick="funHideMPE();" />
                                                        </h1>
                                                    </div>
                                                    <div>
                                                        <table cellpadding="5" width="100%">
                                                            <tr>
                                                                <td>
                                                                    <div id="DivCTM" runat="server" style="font-weight: bold;">
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                        </td>
                                    </tr>
                                </table>
                                <asp:HiddenField ID="HQAONMSR" runat="server" />
                                <asp:HiddenField ID="HQAONPIF" runat="server" />
                                <asp:HiddenField ID="HCTMStatus" runat="server" />
                                <asp:HiddenField ID="HOpenString" runat="server" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
                                <%--<asp:AsyncPostBackTrigger ControlID="Report" EventName="Click" />--%>
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript" src="Script/General.js"></script>

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" src="Script/jquery.min.js"></script>

    <script type="text/javascript" language="javascript">

        $(document).ready(function() {
            $('#divActivityLegends').css('display', 'none');
        })
        function funHideMPE() {
            $find('MPEId').hide();
            var radio = document.getElementById('<%=DivCTM.clientid %>').getElementsByTagName('input');
            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked) {
                    radio[i].checked = false;
                }
            }
        }
        setInterval(RefreshData, 30000)
        function RefreshData() {
            // console.log('starting refresh data');

            if (document.getElementById('<%= HCTMStatus.ClientId %>').value != "Y") {
                if (document.getElementById('<%=HProjectId.ClientID %>').value != "") {
                    if (document.getElementById('accordion') != null) {
                        var DivParentLen = document.getElementById('accordion').childNodes.length
                        for (iDivParentLen = 0; iDivParentLen < DivParentLen; iDivParentLen++) {
                            if ($(document.getElementById('accordion').childNodes[iDivParentLen]).is(':visible') == true) {
                                var DivActLen = document.getElementById('accordion').childNodes[iDivParentLen].childNodes.length;
                                DivActLen = DivActLen - 1;
                                if (DivActLen > 0) {
                                    if ($(document.getElementById('accordion').childNodes[iDivParentLen].childNodes[1]).is(':visible') == true) {
                                        //                                    document.getElementById('ctl00_CPHLAMBDA_HIsRefresh').value = 1;
                                        //                                    document.getElementById('accordion').childNodes[iDivParentLen].childNodes[0].click();
                                        ////                                   // $('#accordion ul').slideDown();
                                        //                                    document.getElementById('<%=HIsRefresh.ClientID %>').value = '';
                                        var DivSubLen = document.getElementById('accordion').childNodes[iDivParentLen].childNodes[1].childNodes.length;
                                        for (iSub = 0; iSub < DivSubLen; iSub++) {
                                            if ($(document.getElementById('accordion').childNodes[iDivParentLen].childNodes[1].childNodes[iSub].childNodes[0]).next().is(':visible') == true) {
                                                document.getElementById('<%=HIsRefresh.ClientID %>').value = 1;
                                                document.getElementById('accordion').childNodes[iDivParentLen].childNodes[1].childNodes[iSub].childNodes[0].click();
                                                document.getElementById('<%=HIsRefresh.ClientID %>').value = '';
                                                var str = document.getElementById('accordion').childNodes[iDivParentLen].childNodes[1].children[iSub].innerHTML.split(">")[1].split("<")[0];
                                                document.getElementById('accordion').childNodes[iDivParentLen].childNodes[1].children[iSub].children[0].innerText = str;

                                            }
                                        }
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }
        function SetValue(vProjNo, vWorkSpaceId) {
            $find('MPEId').hide();
            var radio = document.getElementById('<%=DivCTM.clientid %>').getElementsByTagName('input');
            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked) {
                    radio[i].checked = false;
                }
            }

            var str = document.getElementById('<%=HOpenString.ClientId %>').value;
            window.open(str.toString() + vWorkSpaceId.toString() + "&ProjectNo=" + vProjNo.toString());


        }

        function accordionlidiv(ele, vWorkSpaceId, iParentId, iPeriod, vActivityId, HavingTrmplate, cSubjectWiseFlag, vNodeDisplayName) {

            if ($(ele).next().is(':visible') && document.getElementById('<%=HIsRefresh.ClientID %>').value == '') {
                $('#accordion ul').slideUp(300);
                return;
            }
     
            PageMethods.getChildActivity
            (
                 vWorkSpaceId.toString(),
                 document.getElementById('<%= HCTMStatus.ClientId %>').value,
                  iParentId.toString(),
                  iPeriod.toString(),
                  vActivityId.toString(),
                  HavingTrmplate.toString(),
                  cSubjectWiseFlag,
                  vNodeDisplayName,
                function(Result) {

                    //$('#accordion ul').remove();
                    // $(ele).parent().children('#accordion ul').remove();
                    //$(ele).parent. = '';
                    var parent = ele.parentNode;
                    if (ele.nextSibling) {
                        parent.removeChild(ele.nextSibling);
                    }
                    $(ele).parent().append(Result);
                    $('#accordion ul').slideUp();
                    if (false == $(ele).next().is(':visible') && document.getElementById('ctl00_CPHLAMBDA_HIsRefresh').value == '') {
                        $(ele).next().slideDown(300);
                    }

                },
                function(eerror) {
                    msgalert(eerror);
                }
            );
            ////            }
            ////            else {
            ////                if (false == $(ele).next().is(':visible')) {
            ////                    $('#accordion ul').slideUp(300);
            ////                }
            ////                $(ele).next().slideToggle(300);
            ////            }
        }

        function accordionlidivSubject(ele, vWorkSapceId, iNodeId, iPeriod, vActivityId, cSubjectWiseFlag) {

            if ($(ele).next().is(':visible') && document.getElementById('<%=HIsRefresh.ClientID %>').value == '') {
                $('#accordion ul ul').slideUp(300);
                return;
            }

            PageMethods.getSubject(
                vWorkSapceId.toString(),
                document.getElementById('<%= HCTMStatus.ClientId %>').value,
                iNodeId.toString(),
                iPeriod,
                vActivityId,
                cSubjectWiseFlag,
                function(Result) {
                    var parent = ele.parentNode;
                    if (ele.nextSibling) {
                        parent.removeChild(ele.nextSibling);
                    }
                    $(parent).append(Result);
                    $('#accordion ul ul').slideUp(300);
                    if (false == $(ele).next().is(':visible') || document.getElementById('ctl00_CPHLAMBDA_HIsRefresh').value == '') {
                        $(ele).next().slideDown(300);
                    }
                },
                function(eerror) {
                    msgalert(eerror);
                }
            );
            //            }
            //            else {
            //                if (false == $(ele).next().is(':visible')) {
            //                    $('#accordion ul ul').slideUp(300);
            //                }
            //                $(ele).next().slideToggle(300);
            //            }
        }
        function funParentActivity(ele) {
     

            if (ele.src.toString().toUpperCase().search("EXPAND") == -1) {
                if (document.getElementById('ProjectActivity').innerHTML.trim() == "") {
                    PageMethods.GetProc_TreeViewOfNodes
                (
                    document.getElementById('<%= HProjectId.ClientId%>').value,


                    function(Result) {

                        $("#ProjectActivity").append(Result);
                    },
                    function(eerror) {
                        msgalert(eerror);
                    }
                );
                }

            }

        }

        function funParentActivityCTM(ele, ProjectActivity, ProjectId) {

            if (ele.src.toString().toUpperCase().search("EXPAND") == -1) {
                if (document.getElementById(ProjectActivity).innerHTML == '') {
                    PageMethods.GetProc_TreeViewOfNodes
                (
                    ProjectId,
                    function(Result) {
                        $("#" + ProjectActivity).append(Result);
                    },
                    function(eerror) {
                        msgalert(eerror);
                    }
                );
                }

            }

        }

        function funReport(str, Flag) {
            if (document.getElementById('<%=ChkBClient.ClientId%>').checked == true && document.getElementById('<%= HCTMStatus.ClientId %>').value == 'Y') {
                $find('MPEId').show();
                document.getElementById('<%=HOpenString.ClientId %>').value = str.toString();

            }
            else {
                if (Flag == 0) {
                    window.open(str.toString() + document.getElementById('<%= HProjectId.ClientId%>').value + "&ProjectNo=" + document.getElementById('<%=txtProject.ClientId %>').value);
                }
                else if (Flag == 1) {
                    window.open(str.toString() + document.getElementById('<%= HProjectId.ClientId%>').value + "&NodeId=" + document.getElementById('<%= HQAONMSR.ClientId%>').value);
                }
                else {
                    window.open(str.toString() + document.getElementById('<%= HProjectId.ClientId%>').value + "&NodeId=" + document.getElementById('<%= HQAONPIF.ClientId%>').value);
                }

            }
        }

        function funOpen(str) {
            window.open(str);
        }

        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
       $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        function funOnMouseOver(id) {
            id.style.cursor = 'pointer';
        }


        function funOnMouseclick(id) {

            $('#ctl00_CPHLAMBDA_fProjectInfo').animate({ "margin-left": "180px" }, 100);
            $('#ctl00_CPHLAMBDA_fProjectActivity').animate({ "margin-left": "180px" }, 100);
            var Status = document.getElementById('<%= HCTMStatus.clientid %>').value;
            if (Status == "Y") {
                $('#ctl00_CPHLAMBDA_tblProjectInfo').animate({ "margin-left": "180px" }, 100);
            }
            $("#ctl00_CPHLAMBDA_menu").slideDown(700);
            id.style.display = 'none';
        }
        function funCloseReport() {
            document.getElementById('ctl00_CPHLAMBDA_Report').style.display = '';
            $("#ctl00_CPHLAMBDA_menu").slideUp(500);
            var Status = document.getElementById('<%= HCTMStatus.clientid %>').value;
            if (Status == "Y") {
                $('#ctl00_CPHLAMBDA_tblProjectInfo').animate({ "margin-left": "0px" }, 100);
            }
            $('#ctl00_CPHLAMBDA_fProjectInfo').animate({ "margin-left": "0px" }, 500);
            $('#ctl00_CPHLAMBDA_fProjectActivity').animate({ "margin-left": "0px" }, 500);




        }

        function displayProjectInfo(ele) {
            if (ele.src.toString().toUpperCase().search("EXPAND") != -1) {
                $("#ProjectInfo").slideToggle(500);
                ele.src = "images/collapse_blue.jpg";

            }
            else {
                $("#ProjectInfo").slideToggle(500);
                ele.src = "images/expand.jpg";
            }
        }

        function displayProjectActivity(ele) {
            if (ele.src.toString().toUpperCase().search("EXPAND") != -1) {

                $("#ProjectActivity").slideToggle(500);
                ele.src = "images/collapse_blue.jpg";
            }
            else {

                $("#ProjectActivity").slideToggle(500);
                ele.src = "images/expand.jpg";
            }
        }

        function displayProjectActivityCTM(ele, ProjectActivityId, childId) {
            if (ele.src.toString().toUpperCase().search("EXPAND") != -1) {

                $("#" + ProjectActivityId).slideToggle(500);
                ele.src = "images/collapse_blue.jpg";
                $("#" + childId).css('height', function(index) {
                    return $("#" + childId).height() + 400;
                });
            }
            else {
                $("#" + childId).css('height', function(index) {
                    return $("#" + childId).height() - 400;
                });

                $("#" + ProjectActivityId).slideToggle(500);
                ele.src = "images/expand.jpg";

            }
        }


        function displayChildCTM(ele, ProjectActivityId, ChildEle, ChildInfo) {
            if (ele.src.toString().toUpperCase().search("EXPAND") != -1) {

                $("#" + ProjectActivityId).slideToggle(500);
                ele.src = "images/collapse_blue.jpg";

                //                 if (document.getElementById(ChildEle).src.toString().toUpperCase().search("EXPAND")!=-1)
                //                 {
                //                     $("#" + ChildInfo).slideToggle(500);   
                //                     document.getElementById(ChildEle).src="images/collapse_blue.jpg";
                //                 }
            }
            else {

                $("#" + ProjectActivityId).slideToggle(500);
                ele.src = "images/expand.jpg";

                //                if (document.getElementById(ChildEle).src.toString().toUpperCase().search("EXPAND")==-1)
                //                {
                //                    $("#" + ChildInfo).slideToggle(500);
                //                    document.getElementById(ChildEle).src = "images/expand.jpg";
                //                }
            }
        }


        function displayChildInfoCTM(ele, ProjectActivityId, childId) {
            if (ele.src.toString().toUpperCase().search("EXPAND") != -1) {

                $("#" + ProjectActivityId).slideToggle(500);
                ele.src = "images/collapse_blue.jpg";

                $("#" + childId).css('height', function(index) {
                    return $("#" + childId).height() + 100;
                });
            }
            else {
                $("#" + childId).css('height', function(index) {
                    return $("#" + childId).height() - 100;
                });

                $("#" + ProjectActivityId).slideToggle(500);
                ele.src = "images/expand.jpg";


            }
        }
    </script>

</asp:Content>
