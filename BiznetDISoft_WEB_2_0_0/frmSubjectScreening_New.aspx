<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmSubjectScreening_New.aspx.vb"
    Inherits="frmSubjectScreening_New" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <script language="javascript">
        function SelectCurrentTab() {
            //$(".dropDownListForGroup").val($("#hdnSubGroup").val());
        }
    </script>
    
    <link rel="shortcut icon" href="images/biznet.ico" type="image/x-icon"/>

    <link href="App_Themes/StyleCommon/CommonStyle.css" rel="Stylesheet" type="text/css" />
    <link href="App_Themes/font-awesome.min.css" rel="Stylesheet" type="text/css" />
    <link href="App_Themes/sweetalert.css" rel="Stylesheet" type="text/css" />

    <script src="Script/jquery.min.js" type="text/javascript"></script>
    <script src="Script/jquery-1.4.3.min.js" type="text/javascript"></script>
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script src="Script/Validation.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <script src="Script/AutoComplete.js" language="javascript" type="text/javascript"></script>
    <script src="Script/jquery.dataTables.plugins.js" type="text/javascript"></script>
    <script type="text/javascript" src="Script/sweetalert.js"></script>
    <style type="text/css">
        .dropDownListForGroup {
            font-weight: bold;
        }

            .dropDownListForGroup option {
                font-weight: bold;
            }

        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

        .buttonForImage {
            background: url(./Images/TranscribeDate.png) no-repeat;
            cursor: pointer;
            border: none;
            width: 26px;
            height: 26px;
            text-align: right;
            float: right;
        }

        .buttonForImageFreezeAuditTrail {
            background: url(./Images/FreezeAuditTrail.png) no-repeat;
            cursor: pointer;
            border: none;
            width: 26px;
            height: 26px;
            text-align: right;
            float: right;
        }


        .buttonForImageFreeze {
            background: url(./images/Freeze.jpg) no-repeat;
            cursor: pointer;
            border: none;
            width: 26px;
            height: 26px;
            text-align: right;
            float: right;
        }

        .buttonForImageFreezeStatus {
            background: url(./images/Freeze.jpg) no-repeat;
            cursor: pointer;
            border: none;
            width: 26px;
            height: 26px;
            text-align: right;
            /*float: left;*/
            /*margin-left :30px;*/
        }

        .buttonForImageUnFreezeStatus {
            background: url(./images/UnFreeze.jpg) no-repeat;
            cursor: pointer;
            border: none;
            width: 30px;
            height: 30px;
            text-align: right;
            /*float: left;*/
            /*margin-left :30px;*/
        }


        .HeadButton {
            line-height: 1.8em !important;
            border-bottom: 1px solid #ccc !important;
            float: left !important;
            display: inline !important;
            border: 1px solid #d3d3d3 !important;
            background: rgb(30,87,153) !important; /* Old browsers */
            background: -moz-linear-gradient(top, rgba(30,87,153,1) 0%, rgba(41,137,216,1) 50%, rgba(32,124,202,1) 100%, rgba(125,185,232,1) 100%) !important; /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,rgba(30,87,153,1)), color-stop(50%,rgba(41,137,216,1)), color-stop(100%,rgba(32,124,202,1)), color-stop(100%,rgba(125,185,232,1))) !important; /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top, rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 100%,rgba(125,185,232,1) 100%) !important; /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top, rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 100%,rgba(125,185,232,1) 100%) !important; /* Opera 11.10+ */
            background: -ms-linear-gradient(top, rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 100%,rgba(125,185,232,1) 100%) !important; /* IE10+ */
            background: linear-gradient(to bottom, rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 100%,rgba(125,185,232,1) 100%) !important; /*W3C*/
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr= '#1e5799', endColorstr= '#7db9e8',GradientType=0 ) !important; /* IE6-9 */
            border-radius: 30px 5px 5px 5px !important;
            color: white !important;
            box-shadow: 5px 5px 5px #888888 !important;
            border: 1px solid aqua !important;
            font-weight: bold !important;
        }

        #tblAuditField {
            margin: 0 auto;
            width: 100%;
            clear: both;
            border-collapse: collapse;
            table-layout: fixed;
            word-wrap: break-word;
        }

        #tblAudit {
            margin: 0 auto;
            width: 100%;
            clear: both;
            border-collapse: collapse;
            table-layout: fixed;
            word-wrap: break-word;
        }

        #tblFreezeAuditTrail tr {
            width: 100%;
        }

        #tblTransCribeAuditTrail tr {
            width: 100%;
        }

        #tblFreezeAuditTrail {
            margin: 0 auto;
            width: 100% !Important;
            clear: both;
            border-collapse: collapse;
            table-layout: fixed;
            word-wrap: break-word;
        }

        #tblTransCribeAuditTrail {
            margin: 0 auto;
            width: 100% !Important;
            clear: both;
            border-collapse: collapse;
            table-layout: fixed;
            word-wrap: break-word;
        }


        #tblDCF {
            margin: 0 auto;
            width: 100% !Important;
            clear: both;
            border-collapse: collapse;
            table-layout: fixed;
            word-wrap: break-word;
        }


        .divModalBackGroundForEdit {
            position: fixed !important;
            left: 0px !important;
            top: 0px !important;
            z-index: 99999999 !important;
            /*width: 1364px !important;
            height: 647px !important;*/
            opacity: 0.8 !important;
            background-color: gray !Important;
        }

        .centerModalPopupForEdit {
            position: fixed !important;
            padding: 5px !important;
            text-align: center !important;
            border: #84c8e6 1px solid;
            vertical-align: middle;
            text-align: center;
            z-index: 10000000000 !important;
            -moz-box-shadow: 0 0 10px 1px #888;
            -webkit-box-shadow: 0 0 10px 1px #888;
            box-shadow: 0 0 10px 1px #888;
            border-radius: 6px;
            width: 700px;
        }

        .divModalPopupForDCF {
            position: fixed !important;
            padding: 5px !important;
            text-align: center !important;
            border: #84c8e6 1px solid;
            vertical-align: middle;
            text-align: center;
            z-index: 1000 !important;
            -moz-box-shadow: 0 0 10px 1px #888;
            -webkit-box-shadow: 0 0 10px 1px #888;
            box-shadow: 0 0 10px 1px #888;
            border-radius: 6px;
            left: 5%;
            top: 20%;
            background-color: gray !important;
        }

        [title~=Update] {
            margin: 0px 5px;
        }

         [title~=DCF] {
            margin: 0px 5px;
        }
        [title~=AuditTrail] {
            margin: 0px 5px;
        }
        [title~=Edit] {
            margin: 0px 5px;
        }

        .popupcontrol {
            position: fixed !important;
            padding: 5px !important;
            text-align: center !important;
            border: #84c8e6 1px solid;
            vertical-align: middle;
            text-align: center;
            z-index: 1000000 !important;
            -moz-box-shadow: 0 0 10px 1px #888;
            -webkit-box-shadow: 0 0 10px 1px #888;
            box-shadow: 0 0 10px 1px #888;
            border-radius: 6px;
            width: 700px;
            background-color: #FFFFFF !important;
            left: 75px !important;
            width: 1200px !important;
        }
        canvas {
            height:65px !important;
        }
        .sweet-overlay {
            z-index:9999999999!important;
        }
        .sweet-alert {
            z-index:10000000000!important;
        }

        /*.popupcontrol {
            position: fixed !important;
            padding: 5px !important;
            text-align: center !important;
            border: #84c8e6 1px solid;
            vertical-align: middle;
            text-align: center;
            z-index: 1000000 !important;
            -moz-box-shadow: 0 0 10px 1px #888;
            -webkit-box-shadow: 0 0 10px 1px #888;
            box-shadow: 0 0 10px 1px #888;
            border-radius: 6px;
            width: 700px;
            background-color: #FFFFFF !important;
            left: 150px !important;
            width: 1025px !important;
        }*/

        #tblECGReviewList {
            width: 1017px !important;
        }
        #tblECGReviewList_wrapper {
             width: 1017px !important;
        }

    </style>

</head>
<body>
    <form id="form1" runat="server" method="post">
        <asp:HiddenField runat="server" ID="hdnAllGroupId" />
        <asp:HiddenField runat="server" ID="hdnSubGroup" />
        <asp:HiddenField runat="server" ID="hdnAlreadySaved" />
        <asp:HiddenField runat="server" ID="hdnMedExScreenHdrno" />
        <asp:HiddenField runat="server" ID="hdnIsTranscribe" />
        <asp:HiddenField runat="server" ID="hdnIsModelPopupShow" />
        <asp:HiddenField runat="server" ID="hdnScreeningDate" />
        <asp:HiddenField runat="server" ID="hdnTranscribeRemarks" />
        <asp:HiddenField runat="server" ID="hdnUserTypeName" />
        <asp:HiddenField runat="server" ID="hdnWebConfigUserTypeName" />
        <asp:HiddenField runat="server" ID="hdnIsSave" />
        <asp:HiddenField runat="server" ID="hdnDCFGenerated" />
        <asp:HiddenField runat="server" ID="hdnMedExScreeningDtlNo" />
        <asp:HiddenField runat="server" ID="hdnUserId" />
        <asp:HiddenField runat="server" ID="hdnScreeningDCno" />
        <asp:HiddenField runat="server" ID="hdnTranscribeDate" />
        <asp:HiddenField runat="server" ID="hdnIsTranscribeYes" />
        <asp:Button runat="server" ID="btnDCFShowHide" Style='display: none;' />
        <asp:Button runat="server" ID="btnRedirectPage" Style='display: none;' />
        <asp:Button runat="server" ID="btnDropDownSelectedIndexChanged" Style='display: none;' />

        <center>
            <div id="updateProgress" align="center" class="updateProgress" style="display: none; left: 40%; position: absolute; vertical-align: middle !important;">
                <div align="center" style="z-index: 999999999999999 ! important;">
                    <table>
                        <tr>
                            <td style="height: 100px;">
                                <font class="updateText" style="margin-right: 22px">Please Wait.....</font>
                            </td>
                            <td style="height: 100px">
                                <div title="Wait" class="updateImage">
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </center>

        <div id="divThemeSelection" onchange="hideDiv()" class="themeSelection" style="display: none;">
            <table>
                <tr>
                    <td>
                        <h4 style="text-align: center">Select Theme</h4>
                        <hr style="width: 133%; float: none;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <table style="margin-left: 82px;">
                            <tr>
                                <td style="width: 20%; text-align: right;">
                                    <label id="lblornage" class="ThemeLable" style="background-color: #CF8E4C;"></label>
                                </td>
                                <td style="text-align: left;">
                                    <label id="lblBlue" class="ThemeLable" style="background-color: #1560a1;"></label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%; text-align: right;">
                                    <label id="lblGreen" class="ThemeLable" style="background-color: #33a047;"></label>
                                </td>
                                <td style="text-align: left;">
                                    <label id="lblDemo" class="ThemeLable" style="background-color: #999966;"></label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <input style="display: none" type="text" name="fakeusernameremembered" />
        <input style="display: none" type="password" name="fakepasswordremembered" />
        <asp:ScriptManager ID="ScriptManager2" runat="server" AsyncPostBackTimeout="1000"
            EnablePageMethods="True">
            <Services>
                <asp:ServiceReference Path="AutoComplete.asmx" />
            </Services>
        </asp:ScriptManager>
        <table border="0" cellspacing="0" style="border-collapse: collapse; width: 100%;"
            bordercolor="#111111" id="AutoNumber1" cellpadding="0">
            <asp:HiddenField ID="CompanyName" runat="server" />
           <tr style="height: 65px">
                <td style="vertical-align: bottom; width: 100%; height: 65px; text-align: left;">
                    <div style="background-image: url(images/left1.jpg); background-repeat: repeat-x; width: 100%; height: 65px;">                       
                        <div style="padding:5px; position: absolute; z-index: 999;">
                                    <img src="Images/biznet-logo.png" alt="biznet logo left" width="60" id="LogoImg"/>
                                </div>
                        <div style="float:right; width:100%;">
						    <div id="qodef-particles" class="fixed" style="height: 65px; background-color: #e4e4e4;" data-particles-density="high" 
                                    data-particles-color="#f9a54b" data-particles-opacity="0.8" data-particles-size="3" data-speed="3" data-show-lines="yes" 
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
                                        <asp:Label ID="lblTime" runat="server" CssClass="Label" Style="color: blue; text-shadow: 0px 0px 20px #000; opacity: 2.22; font-size: 12px;" Visible="false"/>
                                    <asp:Label runat="server" ID="lblSessionTimeCap" CssClass="Label" Style="color: Black; font-size:15px; width:14px;font-weight:bold;"
                                        Text="Session Expires In: "></asp:Label>
                                    <b><span class="Label headerusername" id="timerText"></span></b>                                            
                                    <asp:HiddenField ID="HDSessionValue" runat="server" Value='<%=Session("UserId")%>' />
                                    </div>
                                </td>
                            </tr>
                            <div class="Manage">
                                <span id="lblWelcome" class="Label" style="color: #000;font-size:15px;">Welcome :</span>
                                <asp:Label ID="lblUserName" runat="server" CssClass="Label headerusername" />
                            </div> 
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 100%; text-align: left;" background="images/bluebg.gif"></td>
            </tr>
            <tr>
                <td style="width: 95%; text-align: left;">
                    <div style="width: 90%; margin: auto;">
                        <table cellspacing="1" width="100%" cellpadding="0">
                            <tr>
                                <td align="center" style="width: 98%">
                                    <asp:Panel ID="Pan_Hdr" runat="server" Width="100%" CssClass="InnerTable" BackColor="White">
                                        <asp:Panel ID="Pan_Child" runat="server" Width="100%" BackColor="Window">
                                            <div id="HeaderbtnReviewEdit Label" style="text-align: center;" align="center" class="Div">
                                                <table style="width: 100%; text-align: center;">
                                                    <tr style="text-align: center;">
                                                        <td align="center" width="100%">
                                                            <asp:Label ID="lblHeader" runat="server" SkinID="lblHeading" Text="Screening" />
                                                        </td>
                                                    </tr>
                                                    <tr style="text-align: center;">
                                                        <td style="text-align: left; width: 890px;">
                                                            <div style="margin: auto; color: Red; text-align: center;">
                                                                <asp:Label ID="lblerrormsg" runat="server" Visible="false" />
                                                            </div>
                                                            <table style="width: 100%" cellpadding="3px">
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <fieldset id="fldSubject" class="FieldSetBox" style="width: 100%; height: auto; float: left;">
                                                                            <legend id="lblDetails" runat="server" class="LegendText" style="color: Black; font-size: 12px;">
                                                                                <img id="imgfldgen" alt="SubjectSpecific" src="images/expand.jpg" onclick="displayProjectInfo(this,'tblgen');"
                                                                                    runat="server" />
                                                                                Subject Details</legend>
                                                                            <div id="tblgen">
                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td style="white-space: nowrap; text-align: right;" colspan="100%">
                                                                                            <asp:Label ID="lblProfile" runat="server" CssClass="Label" Text="Change Profile : " />
                                                                                            <asp:DropDownList runat="server" ID="ddlProfile" AutoPostBack="True" CssClass="Label" Width="160px" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="white-space: nowrap; width: 10%; text-align: right;">
                                                                                            <asp:Label ID="lblWristband" runat="server" SkinID="lblDisplay" Text="Wristband Gunning :" CssClass="Label" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtWristBand" runat="server" CssClass="textBox" TabIndex="2" Width="480px"
                                                                                                Style="vertical-align: initial;" autocomplete="off" />
                                                                                            <%--<asp:CheckBox runat="server" ID="txtIsGunned" Text="Subject" onclick="ShowWristBand()" />--%>
                                                                                        </td>
                                                                                    </tr>

                                                                                    <tr runat="server" id="trSubject">
                                                                                        <td style="white-space: nowrap; width: 10%; text-align: right;">
                                                                                            <asp:Label ID="lblSuject" runat="server" SkinID="lblDisplay" Text="Subject :" CssClass="Label" />
                                                                                        </td>
                                                                                        <td>

                                                                                            <img id="ImgbtnShowECG" alt="Review ECG" src="Images/ECG.png" onclick="fnReviewECG()" title="Review ECG" style="float: right; cursor: pointer;" />
                                                                                            <asp:ImageButton ID="ImgBtnShowECG" AlternateText="ECG" ImageUrl="~/Images/ECG.png"
                                                                                                runat="server" Style="float: right;" ToolTip="Click Me To GO Home" OnClientClick="return fnReviewECG();" Visible="false" />
                                                                                            <asp:ImageButton ID="ImgbtnShowHome" AlternateText="HOME" ImageUrl="~/Images/Home.png"
                                                                                                runat="server" Style="float: right;" ToolTip="Click Me To GO Home" OnClientClick="return HomeClick(this);" />
                                                                                            <asp:ImageButton ID="btnPdfHide" runat="server" Text="Authenticate" causevalidation="false" CssClass="button" Width="110px" Style="display: none;"></asp:ImageButton>

                                                                                            <asp:ImageButton ID="btnPdf" ImageUrl="~/Images/PrintPdf.png" runat="server" ToolTip="Export To PDF"
                                                                                                Style="float: right; cursor: pointer" OnClientClick="fnGetPrintString('S')" />

                                                                                            <%--<input   type="image"  id="btnPdf" src="./Images/PrintPdf.png" onclick="fnGetPrintString('S')" alt="Submit" style="float: right; cursor: pointer" title="Export To PDF">--%>

                                                                                            <img id="ImgLogo" runat="server" src="~/images/lambda_logo.jpg" alt="Lambda Logo"
                                                                                                style="display: none;" />
                                                                                            <asp:Image ID="btnReviewHistory" ToolTip="Review History" runat="server" ImageUrl="~/Images/Review_Histroy.png"
                                                                                                Style="float: right; display: none; cursor: pointer" />


                                                                                            <input type="button" id="imgbtnTranscribe" title="Click Me to Show Transcribe AuditTrail" class="buttonForImage" onclick="return TranscribeAuditTrail()">

                                                                                            <input type="button" id="imgbtnFreeze" title="Click Me to Show Freeze/UnFreeze Audit Trail" class="buttonForImageFreezeAuditTrail" onclick="return FreezeAuditTrail()">


                                                                                            <%--<asp:ImageButton ID="imgbtnTranscribe"  ImageUrl="~/Images/TranscribeDate.png"
                                                                                                runat="server" Style="float: right;" OnClick="imgbtnTranscribe_Click" ToolTip="Click Me To Show Transcribe Date Audit Trail" />--%>

                                                                                            <%--<asp:ImageButton ID="imgbtnFreeze"  runat="server" Style="float: right;" />--%>

                                                                                            <asp:TextBox ID="txtSubject" runat="server" CssClass="textBox" TabIndex="2" Width="480px"
                                                                                                Style="vertical-align: baseline;" />
                                                                                            <asp:Button ID="btnSubject" runat="server" Style="display: none" Text="Subject" />
                                                                                            <asp:Button ID="BtnQC" OnClientClick="return QCDivShowHide('S');" runat="server"
                                                                                                CssClass="button" Text="QC" Visible="false" />
                                                                                            <asp:HiddenField ID="HSubjectId" runat="server" />
                                                                                            <asp:HiddenField ID="HScrNo" runat="server" />
                                                                                            <asp:HiddenField ID="HFNumericScale" runat="server" />
                                                                                            <asp:HiddenField ID="HFMedexType" runat="server" />
                                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                                                                                MinimumPrefixLength="1" OnClientItemSelected="OnSelected" OnClientShowing="ClientPopulated"
                                                                                                ServiceMethod="GetSubjectCompletionList_NotRejected" ServicePath="AutoComplete.asmx"
                                                                                                TargetControlID="txtSubject" UseContextKey="True" CompletionListElementID="pnlSubjectList"
                                                                                                CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                                                                CompletionListCssClass="autocomplete_list">
                                                                                            </cc1:AutoCompleteExtender>
                                                                                            <asp:Panel ID="pnlSubjectList" runat="server" Style="max-height: 300px; overflow: auto; overflow-x: hidden" />
                                                                                        </td>
                                                                                    </tr>

                                                                                    <tr>
                                                                                        <td class="Label" colspan="2">
                                                                                            <asp:CheckBoxList ID="chkScreeningType" runat="server" RepeatDirection="Horizontal"
                                                                                                onclientclick="return ShowHideproject();" CssClass="chkScreenType" AutoPostBack="true"
                                                                                                Style="margin-left: 10%;">
                                                                                                <asp:ListItem Value="DF">Generic Screening</asp:ListItem>
                                                                                                <asp:ListItem Value="PS">Project Specific Screening</asp:ListItem>
                                                                                            </asp:CheckBoxList>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr id="tr_WorkSpace" style="display: none;" runat="server" class="tr_WorkSpace">
                                                                                        <td class="Label" style="text-align: right;">
                                                                                            <asp:Label ID="lblProject" runat="server" Text="Project :"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="475px" TabIndex="1" />
                                                                                            <asp:Button Style="display: none" ID="btnSetProject" runat="server" OnClick="btnSetProject_Click" Text=" Project" />
                                                                                            <asp:HiddenField ID="HProjectId" runat="server" />
                                                                                            <asp:HiddenField ID="HFMedExFormula" runat="server" />
                                                                                            <asp:HiddenField ID="HFDecimalNo" runat="server" />
                                                                                            <asp:HiddenField ID="HClientName" runat="server" />
                                                                                            <asp:HiddenField ID="HProjectNo" runat="server" />
                                                                                            <asp:HiddenField ID="HFLocationCode" runat="server" />
                                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtenderWorkSpace" runat="server" UseContextKey="True"
                                                                                                TargetControlID="txtproject" ServicePath="AutoComplete.asmx" ServiceMethod="GetMyProjectCompletionListForProjectSpScr"
                                                                                                OnClientShowing="ClientPopulated_WorkSpace" OnClientItemSelected="OnSelected_WorkSpace"
                                                                                                CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" MinimumPrefixLength="1"
                                                                                                CompletionListItemCssClass="autocomplete_listitem" CompletionListCssClass="autocomplete_listitem"
                                                                                                BehaviorID="AutoCompleteExtenderWorkSpace" CompletionListElementID="pnlProjectList" />
                                                                                            <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="2">
                                                                                            <asp:RadioButtonList ID="rblScreeningDate" runat="server" AutoPostBack="True" RepeatColumns="4"
                                                                                                RepeatDirection="Horizontal" CssClass="Label" Style="margin-left: 10%; display: none;" />
                                                                                            <asp:DropDownList ID="ddlScreeningDate" runat="server" CssClass="dropDownList" Style="margin-left: 10%; max-width: 250px;" AutoPostBack="true" />

                                                                                            <input type="button" id="btnFreezeUnFreeze" style="display: none" onclick="return FreezeUnFreezeScreening()" />

                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td></td>
                                                                                        <td style="text-align: left; white-space: nowrap;" class="Label" id="trReviewCompleted"
                                                                                            visible="false" runat="server">
                                                                                            <fieldset style="width: 300px; margin-top: 1%; height: 30px; text-align: left;" class="FieldSetBox">
                                                                                                <div style="float: right;">
                                                                                                    <asp:Image ID="btnReviewEdit" runat="server" ImageUrl="images/Edit2.gif" Style="display: none" />
                                                                                                    <input type="button" class="btn btnadd" id="btnOk" style="display: none;" value="Ok" onclick="ValidateReview('', '', 1)" />

                                                                                                </div>
                                                                                                <div runat="server" id="ChkEligible">
                                                                                                    <asp:Panel runat="server" ID="reviewpanel" Enabled="false">
                                                                                                        <table>
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="lbleligible" runat="server" Text="Is Eligible"></asp:Label>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:RadioButtonList ID="rblEligible" runat="server" RepeatDirection="Horizontal" readonly="true"
                                                                                                                        onclick="return fnCheckChangeEvent(this);">
                                                                                                                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                                                                                                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                                                                                                    </asp:RadioButtonList>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </asp:Panel>
                                                                                                </div>
                                                                                                <div runat="server" id="chkReviewCompleted" visible="false">
                                                                                                    <table>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblReviewComplete" runat="server" Text="Review Completed :"></asp:Label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </div>
                                                                                                <div runat="server" id="chkEligibleReview" visible="false">
                                                                                                    <table>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblEligibleReview" runat="server" Text="Is Eligible & Review Completed"></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:RadioButtonList ID="rblEligibleReview" runat="server" RepeatDirection="Horizontal"
                                                                                                                    onclick="return fnCheckChangeEvent(this);">
                                                                                                                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                                                                                                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                                                                                                </asp:RadioButtonList>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </div>
                                                                                            </fieldset>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </fieldset>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 100%; height: auto; float: right;">
                                                                        <%--<img id="imgActivityLegends" src="images/question.gif" alt="Activity Legends" style=" height: auto; float: right;" runat="server"
                                                                                                                title="Activity Legends" />--%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" style="width: 100%; height: 265px; text-align: left;">
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
                                                                                    <asp:HiddenField ID="hfMedExCodeForGender" runat="server" />
                                                                                    <asp:HiddenField ID="HfUserName" runat="server" />
                                                                                    <asp:HiddenField ID="HfMaleCount" runat="server" />
                                                                                    <asp:HiddenField ID="HFFerenhitToCelcius" runat="server" />
                                                                                    <button id="btnDeviation" runat="server" style="display: none;" />
                                                                                    <cc1:ModalPopupExtender ID="MPEDeviation" runat="server" PopupControlID="divHistoryDtl"
                                                                                        BackgroundCssClass="modalBackground" TargetControlID="btnDeviation" CancelControlID="ImgPopUpCloseDeviation"
                                                                                        BehaviorID="MPEDeviation">
                                                                                    </cc1:ModalPopupExtender>
                                                                                    <div id="divHistoryDtl" runat="server" class="centerModalPopup" style="display: none; width: 80%; position: absolute; max-height: 80%; overflow: auto;">
                                                                                        <table style="width: 100%" cellpadding="5px">
                                                                                            <tr>
                                                                                                <td class="Label" style="text-align: center;" colspan="2">
                                                                                                    <img id="ImgPopUpCloseDeviation" onclick="HistoryDivShowHide('H','','','');" src="images/close_pop.png"
                                                                                                        style="width: 24px; height: 25px; float: right;" title="Close" />
                                                                                                    <strong style="color: Black; font-size: 12px; font-weight: bold; float: left; font-variant: normal;">
                                                                                                        <asp:Label ID="lblMedexDescription" runat="server" /></strong>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2" valign="top">
                                                                                                    <hr />
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2" valign="top">
                                                                                                    <table id="tblAudit" width="100%">
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <button id="btnfldAudit" runat="server" style="display: none;" />
                                                                                    <cc1:ModalPopupExtender ID="MpeDIVFLD" runat="server" PopupControlID="divFldHistory"
                                                                                        BackgroundCssClass="modalBackground" TargetControlID="btnfldAudit" CancelControlID="imgClosefld"
                                                                                        BehaviorID="MpeDIVFLD">
                                                                                    </cc1:ModalPopupExtender>
                                                                                    <div id="divFldHistory" runat="server" class="centerModalPopup" style="display: none; width: 80%; position: absolute; max-height: 80%; overflow: auto;">
                                                                                        <table style="width: 100%" cellpadding="5px">
                                                                                            <tr>
                                                                                                <td class="Label" style="text-align: center;" colspan="2">
                                                                                                    <img id="imgClosefld" src="images/close_pop.png" style="float: right;"
                                                                                                        title="Close" />
                                                                                                    <strong style="color: Black; font-size: 12px; font-weight: bold; float: left; font-variant: normal;">
                                                                                                        <asp:Label ID="lblfld" runat="server" /></strong>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2" valign="top">
                                                                                                    <hr />
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2" valign="top">
                                                                                                    <table id="tblAuditField" width="100%">
                                                                                                    </table>
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
                                                                                                        <asp:GridView ID="GVAuditFnlRmk" runat="server" Font-Size="Small" SkinID="grdViewAutoSizeMax"
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
                                                                                    <asp:Button ID="btnSetControlValue" runat="server" CssClass="button" Style="display: none"
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
                                                                                    <asp:Button ID="BtnPrevious" runat="server" CssClass="button" Style="display: none" Text="<< Previous"
                                                                                        Width="97px" />
                                                                                </td>
                                                                                <td style="width: 50%; height: 21px; text-align: right;">
                                                                                    <asp:Button ID="BtnNext" runat="server" CssClass="button" Text="Next >>" Style="display: none" Width="97px" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" style="width: 890px; height: 21px; white-space: nowrap; text-align: center; margin: 0 auto;">
                                                                        <asp:Button ID="btnRmkHistory" Visible="false" runat="server" CssClass="button" Text="Remarks History"
                                                                            Style="margin: 0 auto;" display="none" Width="140px" />
                                                                        <asp:Button ID="btnContinueSave" Style="display: none" runat="server" CssClass="btn btnsave" Text="Save & Continue"
                                                                            Width="140px" ToolTip="Save & Continue" />
                                                                        <asp:Button ID="BtnSave" runat="server" Style="display: none" CssClass="btn btnsave" Text="Submit" ToolTip="Save all data" />
                                                                        <asp:Button ID="BtnExit" runat="server" CssClass="btn btnexit" Text="Exit" ToolTip="Exit To Perform New Screening"
                                                                            OnClientClick="return msgconfirmalert('Are You Sure You Want To Exit?',this); " />
                                                                        <asp:Button Style="display: none" ID="btnSaveRunTime" runat="server" Text="SaveRunTime"
                                                                            ToolTip="SaveRunTime" CssClass="btn btnsave" />
                                                                        <asp:Button Style="display: none" ID="btnEdit" runat="server" Text="Edit" ToolTip="Edit"
                                                                            CssClass="btn btnedit" />
                                                                        <asp:Button Style="display: none" ID="btnRedirect" runat="server" Text="Edit" ToolTip="Edit"
                                                                            CssClass="btn btnnew" />

                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>

                                                            <asp:DropDownList runat="server" ID="ddlGroup" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged" Style="display: none;">
                                                                <asp:ListItem Value="0">--Select Group--</asp:ListItem>
                                                            </asp:DropDownList>



                                                            <%--<fieldset style="display: none; font-size: 7pt; height: auto; text-align: left" id="divActivityLegends"
                                                                runat="server">--%>
                                                            <asp:PlaceHolder ID="PhlReviewer" runat="server" EnableViewState="true"></asp:PlaceHolder>
                                                            <%--   </fieldset>--%>
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
                <td class="footer_Master">
                    <p style="text-align: center">
                        <script type="text/javascript">
                            var copyright;
                            var update;
                            copyright = new Date();
                            update = copyright.getFullYear();
                            document.write("<font face=\"verdana\" size=\"1\" color=\"black\">© Copyright " + update + ", Sarjen Systems Pvt LTD. </font>");
                        </script>

                    </p>
                </td>
            </tr>
        </table>
        <button id="btnQCMPe" runat="server" style="display: none;" />
        <cc1:ModalPopupExtender ID="MpeQC" runat="server" PopupControlID="divQCDtl" BackgroundCssClass="modalBackground"
            TargetControlID="btnQCMPe" CancelControlID="Closesearch" BehaviorID="MpeQC">
        </cc1:ModalPopupExtender>
        <div id="divQCDtl" runat="server" class="centerModalPopup" style="display: none; width: 65%; max-height: 500px;">
            <table cellpadding="5" style="width: 100%">
                <tr>
                    <img id="Closesearch" alt="Close" src="images/close_pop.png" style="position: relative; float: right; right: 5px; cursor: pointer;"
                        title="Close" />
                    <td class="LabelText" style="text-align: left !important; font-size: 15px !important; font-weight: bold; color: Black;">QC Comments
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlQCDtl" runat="server">
                <table width="100%">
                    <tr>
                        <td align="left" colspan="2">
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
                        <td class="Label" style="width: 20%; text-align: right;">Remarks :
                        </td>
                        <td style="text-align: left;">
                            <textarea id="txtQCRemarks" onkeydown="ValidateRemarks(this,'lblcnt',1000);" runat="server"
                                class="textBox" style="width: 277px" />
                            <asp:Label ID="lblcnt" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="Label" style="width: 20%; text-align: right; display: none">To :
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="txtToEmailId" runat="server" CssClass="textBox" Width="277px" Style="display: none" />
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="Label" style="width: 20%; text-align: right;">CC :
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="txtCCEmailId" runat="server" CssClass="textBox" Height="37px" TextMode="MultiLine"
                                Width="278px" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="Label" style="text-align: center;">
                            <asp:Button ID="BtnQCSave" runat="server" CssClass="btn btnsave" Style="width: 91px" Text="Save"
                                OnClientClick="return ValidationQC();" />&nbsp;
                            <asp:Button ID="BtnQCSaveSend" runat="server"
                                CssClass="btn btnsave" OnClientClick="return ValidationQC();" Style="width: 91px; display: none"
                                Text="Save & Send" />
                            <asp:Button Style="display: none" ID="btnAutoCalculate" runat="server" Text="Auto Calculate"
                                ToolTip="Auto Calculate" CssClass="btn btnnew" />
                            <input id="Button1" runat="server" class="btn btnnew" onclick="QCDivShowHide('H');" type="button"
                                value="Close" />
                            <asp:Button ID="btnDeleteScreenNo" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: left">
                            <strong>QC Comments History </strong>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="overflow: auto; max-height: 150px">
                                <asp:GridView ID="GVQCDtl" runat="server" AutoGenerateColumns="False" Font-Size="Small"
                                    SkinID="grdViewSmlAutoSize">
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
                                        <asp:BoundField DataField="dQCGivenOn" HeaderText="QC Date" HtmlEncode="False"></asp:BoundField>
                                        <asp:BoundField DataField="vResponse" HeaderText="Response" />
                                        <asp:BoundField DataField="vResponseGivenBy" HeaderText="Response BY">
                                            <ItemStyle Wrap="false" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="dResponseGivenOn" HeaderText="Response Date" HtmlEncode="False"></asp:BoundField>

                                        <asp:TemplateField HeaderText="Response">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkResponse" runat="server">Response</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
        <asp:HiddenField ID="HFWidth" runat="server" />
        <asp:HiddenField ID="HFHeight" runat="server" />
        <asp:HiddenField ID="HdnOldAttrVal" runat="server" />
        <asp:HiddenField ID="HdnCurBtnId" runat="server" />
        <asp:HiddenField ID="HdnCurAttrId" runat="server" />
        <asp:HiddenField ID="HdnPrint" runat="server" />
        <asp:HiddenField ID="HdnPrintString" runat="server" />
        <asp:HiddenField ID="hfWaterMark" runat="server" />
        <asp:HiddenField ID="hfHeaderText" runat="server" />
        <asp:HiddenField ID="hdnchkval" runat="server" />
        <asp:HiddenField ID="hdnCurBtnIDAudit" runat="server" />

        <asp:Button runat="server" ID="btnTranscribeDate" Style="display: none;" />
        <cc1:ModalPopupExtender ID="MpeTranscribeScreening" runat="server" PopupControlID="divTranscribeDate" BackgroundCssClass="modalBackground"
            TargetControlID="btnTranscribeDate" CancelControlID="Closesearch" BehaviorID="MpeTranscribeScreening">
        </cc1:ModalPopupExtender>

        <div id="divTranscribeDate" runat="server" class="centerModalPopup" style="display: none; width: 25%; max-height: 500px;">
            <table cellpadding="3" style="width: 100%;">

                <tr>
                    <img id="Img4" alt="Close" src="images/close_pop.png" style="float: right; right: 0px; height: 25px;"
                        title="Close" onclick="CloseTranscribePopup()" />
                    <hr />

                    <td colspan="3" class="LabelText" style="text-align: center !important; font-size: 15px !important; font-weight: bold; color: Black;">Transcribe Date Option 
                        
                    </td>
                </tr>
            </table>

            <asp:Panel ID="pnlTranscribe" runat="server">
                <table width="90%">
                    <tr>
                        <td class="Label" style="width: 30%; text-align: right;">Transcribe ? :
                        </td>
                        <td style="text-align: left; width: 70%;">
                            <asp:RadioButton runat="server" ID="rbtnYes" GroupName="TD" Text="Yes" />
                            <asp:RadioButton runat="server" ID="rbtnNo" GroupName="TD" Text="No" Style="display: none;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="Label" style="width: 30%; text-align: right;">Screen Date :
                        </td>
                        <td style="text-align: left; width: 60%;">
                            <asp:TextBox runat="server" ID="txtTranscribeDate" Style="width: 80%;"></asp:TextBox>
                            <cc1:CalendarExtender ID="calTranscribeDate" runat="server" Format="dd-MMM-yyyy" TargetControlID="txtTranscribeDate"></cc1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td class="Label" style="width: 20%; text-align: right;">Remarks :
                        </td>
                        <td style="text-align: left; width: 20%;">
                            <asp:TextBox runat="server" ID="txtTranscribeRemarks" Style="width: 80%;" TextMode="MultiLine"></asp:TextBox>

                        </td>
                    </tr>
                    <tr style="margin-top: 500px ! Important;">
                        <td></td>
                        <td style="text-align: left; margin-left: 20%; margin-top: 50px ! Important;">
                            <input type="button" id="btnContinue" class="button" value="Continue" />
                            <%--<asp:Button runat="server" CssClass="button" ID="btnContinue" Text="Continue" />--%>
                            <input type="button" id="btnTranscribeCancel" onclick="CloseTranscribePopup()" class="button" value="Cancel" />
                            <%--<asp:Button runat="server" CssClass="button" ID="btnTranscribeCancel" Text="Cancel" />--%>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>

        <div style="display: none; width: 40%; max-height: 300px; text-align: left; margin: auto; left: 30% !important; background-color: gray;"
            class="divModalPopup" id="divForEditAttribute" runat="server">
            <table style="width: 100%; margin-bottom: 2%">
                <tr>
                    <td colspan="2" style="font-size: 14px;" class="Label">
                        <img id="ImgPopUpCloseEditAttribute" alt="Close" src="images/close_pop.png" style="position: relative; float: right; right: 5px;"
                            onclick="funCloseDiv('divForEditAttribute',2);" />
                        Reason For Edit
                    <hr />
                    </td>
                </tr>

                <tr>
                    <td class="Label" style="text-align: right; width: 30%">
                        <span class="Label" style="vertical-align: top;">Reason* : </span>
                    </td>
                    <td style="text-align: left; width: 70%">
                        <asp:DropDownList runat="server" ID="ddlRemarksForEdit"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2"><span class="Label" style="vertical-align: top;">OR </span></td>
                </tr>
                <tr>
                    <td class="Label" style="text-align: right; width: 30%">
                        <span class="Label" style="vertical-align: top;">Remark* : </span>
                    </td>
                    <td style="text-align: left; width: 70%">
                        <asp:TextBox ID="txtRemarkForAttributeEdit" runat="Server" Text="" TextMode="MultiLine"
                            CssClass="textbox" Width="60%"> </asp:TextBox>
                        <asp:HiddenField ID="HdReasonDesc" runat="server" Value="" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td class="Label" colspan="2" style="text-align: center; width: 100%">
                        <input type="button" id="btnSaveRemarksForAttribute" onclick="return ValidationForEditOrDelete();"
                            class="btn btnsave" value="Save" />
                        <input class="btn btnclose" onclick="$get('ImgPopUpCloseEditAttribute').click();" type="button"
                            value="Close" />
                    </td>
                </tr>
            </table>
        </div>

        <div id="ModalBackGround" runat="server" class="divModalBackGround" style="display: none; width: 100% !Important;">
        </div>


        <table id="ModalBackGroundForEdit" runat="server" class="divModalBackGroundForEdit" style="display: none; width: 100% !Important;">
        </table>

        <asp:Button ID="btnShow" runat="server" Text="Show Dialog" Style="display: none" />
        <cc1:ModalPopupExtender ID="MPEAunthticate" runat="server" TargetControlID="btnShow"
            PopupControlID="divAuthentication" BackgroundCssClass="modalBackground" BehaviorID="MPEId">
        </cc1:ModalPopupExtender>
        <div style="display: none; left: 391px; width: 50%; top: 528px; min-height: 180px; text-align: left"
            id="divAuthentication" class="centerModalPopup" runat="server">
            <table style="width: 100%;">
                <tr>
                    <td style="text-align: left; font-size: 10pt;" colspan="2" class="Label">
                        <img id="Img1" onclick="return DivAuthenticationHideShow();" src="images/close_pop.png"
                            style="width: 24px; height: 25px; float: right;" title="Close" />
                        <strong>User Authentication</strong>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td class="Label" style="text-align: right; width: 30%;">Name :
                    </td>
                    <td class="Label" style="text-align: left;">
                        <asp:Label runat="server" ID="lblSignername"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="Label" style="text-align: right;">Designation :
                    </td>
                    <td class="Label" style="text-align: left;">
                        <asp:Label runat="server" ID="lblSignerDesignation"></asp:Label>
                    </td>
                </tr>
                <tr id="trEle" runat="server">
                    <td class="Label" style="text-align: right;">Eligibility :
                    </td>
                    <td class="Label" style="text-align: left;">
                        <asp:RadioButtonList ID="rblSaveEle" runat="server" RepeatDirection="Horizontal"
                            onclick="return fnEligible(this);">
                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td class="Label" style="text-align: right;">Remarks :
                    </td>
                    <td style="text-align: left;">
                        <label class="Label">
                            I attest to the accuracy and integrity of the data being reviewed.</label>
                    </td>
                </tr>
                <tr>
                    <td class="Label" style="text-align: right;">Password :
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtPassword" runat="Server" Text="" CssClass="textbox" TextMode="Password"> </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="Label" style="text-align: right;">Review Comments :
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtPiRemarks" Width="80%" runat="Server" TextMode="MultiLine" CssClass="textbox"> </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;" colspan="2">
                        <asp:Button ID="btnAuthenticateHide" runat="server" Text="Authenticate" CssClass="button"
                            Width="110px" Style="display: none;"></asp:Button>

                        <input type="button" id="btnAuthenticate" value="Authenticate" onclick="return ValidationForAuthentication();" class="button" />
                        <%--<asp:Button ID="btnAuthenticate" runat="server" Text="Authenticate" CssClass="button"
                            Width="110px" OnClientClick="return ValidationForAuthentication();" Style="display: none;"></asp:Button>--%>

                        <asp:Button ID="btnSaveuthenticate" runat="server" Text="Authenticate" CssClass="btn btnsave"
                            Width="110px" OnClientClick="return ValidationForAuthenticationSave();" Style="display: none;"></asp:Button>
                        <input type="button" id="btnClose" onclick="return DivAuthenticationHideShow();"
                            value="Close" class="btn btnclose" title="Close Autentication" />
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
                                <asp:Button ID="btnContinueWorking" runat="server" Text="Extend" CssClass="btn btnsave" Style="display: none;" />
                                <asp:Button ID="BtnSessionDivClose" runat="server" Text="Close" CssClass="btn btnclose"
                                    OnClientClick="closeSessionDiv();" />
                                <asp:Button ID="btnLogout" runat="server" Text="Go" CssClass="button" Style="display: none;" />
                            </td>
                        </tr>
                    </table>

                </ContentTemplate>
            </asp:UpdatePanel>

        </div>

        <button id="btnTrancribeAudittrail" runat="server" style="display: none;" />

        <cc1:ModalPopupExtender ID="mpTranscribeAuditTrail" runat="server" PopupControlID="dvTransCribeAuditTrail" BehaviorID="mpTranscribeAuditTrail"
            BackgroundCssClass="modalBackground" CancelControlID="imgAuditTrail" TargetControlID="btnTrancribeAudittrail">
        </cc1:ModalPopupExtender>

        <asp:UpdatePanel ID="TransCribePanel" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <div id="dvTransCribeAuditTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
                    <table border="0" cellpadding="2" cellspacing="2" width="100%">
                        <tr>
                            <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 80%;">Audit Trail Information</td>
                            <td style="width: 3%">
                                <img id="imgAuditTrail" alt="Close" src="images/close_pop.png" onmouseover="this.style.cursor='pointer';" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table align="center" border="0" cellpadding="2" cellspacing="2" width="100%">
                                    <tr>
                                        <td>
                                            <table id="tblTransCribeAuditTrail" runat="server"></table>
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
            </ContentTemplate>
        </asp:UpdatePanel>



        <button id="btnFreezeAuditTrail" runat="server" style="display: none;" />

        <cc1:ModalPopupExtender ID="mpFreezeAuditTrail" runat="server" PopupControlID="dvFreezeAuditTrail" BehaviorID="mpFreezeAuditTrail"
            BackgroundCssClass="modalBackground" CancelControlID="imgFreezeAuditTrail" TargetControlID="btnFreezeAuditTrail">
        </cc1:ModalPopupExtender>

        <asp:UpdatePanel ID="FreezePanel" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <div id="dvFreezeAuditTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
                    <table border="0" cellpadding="2" cellspacing="2" width="100%">
                        <tr>
                            <td id="Td1" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 80%;">Audit Trail Information</td>
                            <td style="width: 3%">
                                <img id="imgFreezeAuditTrail" alt="Close" src="images/close_pop.png" onmouseover="this.style.cursor='pointer';" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table align="center" border="0" cellpadding="2" cellspacing="2" width="100%">
                                    <tr>
                                        <td>
                                            <table id="tblFreezeAuditTrail" runat="server"></table>
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
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="dialog" title="Alert message" style="display: none">
            <div class="ui-dialog-content ui-widget-content">
                <p>
                    <span class="ui-icon ui-icon-alert" style="float: left; margin: 0 7px 20px 0"></span>
                    <label id="lblMessage">
                    </label>
                </p>
            </div>
        </div>

        <button id="btnDCFGenerate" runat="server" style="display: none;" />

        <cc1:ModalPopupExtender ID="mpDCFGenerate" runat="server" PopupControlID="divDCF" BehaviorID="mpDCFGenerate"
            BackgroundCssClass="divModalPopupForDCF" CancelControlID="ImgPopUpCloseDCF" TargetControlID="btnDCFGenerate">
        </cc1:ModalPopupExtender>

        <div id="divDCF" runat="server" class="centerModalPopup" style="background-color: aliceblue; display: none; overflow: auto; width: 94%; height: auto; max-height: 84%; min-height: auto;">
            <table style="width: 100%">
                <tbody>
                    <tr align="center">
                        <td colspan="2" style="font-size: 14px !important; text-align: center" class="Label">

                            <b>Discrepancy For Attribute :
                            </b>
                            <%--<asp:Label runat="server" ID="txtDiscrepancyOn"  Text=""></asp:Label>--%>
                            <label id="txtDiscrepancyOn"></label>
                        </td>
                        <td>
                            <img id="ImgPopUpCloseDCF" alt="Close" src="images/close_pop.png" style="float: right; right: 0px; height: 25px;"
                                onclick="DCFClose()" />
                        </td>
                    </tr>
                    <tr align="center">
                        <td colspan="2" style="font-size: 14px !important; text-align: center" class="Label">
                            <%--<img id="ImgPopUpCloseDCF" alt="Close" src="images/close_pop.png" style="float: right; right: 0px; height: 25px;"
                                onclick="DCFClose()" />--%>
                            <hr style="margin-top: 0px; width: 100%;" />
                        </td>
                        <td>
                            <hr style="margin-top: 0px;" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="text-align: right">Remarks For Discrepancy :
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtDiscrepancyRemarks" runat="Server" Text="" CssClass="textBox"
                                Width="226px" TextMode="MultiLine" Height="59px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="text-align: right">Status :
                        </td>
                        <td class="Label" align="left" style="text-align: left">
                            <asp:DropDownList ID="ddlDiscrepancyStatus" runat="server" CssClass="Label">
                                <asp:ListItem Text="Generated" Value="N" Selected="True"></asp:ListItem>
                                <%--<asp:ListItem Text="Answered" Value="O"></asp:ListItem>
                                <asp:ListItem Text="Resolved" Value="R"></asp:ListItem>--%>
                                <%--<asp:ListItem Text="Internally Resolved" Value="I"></asp:ListItem>--%>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td style="text-align: left" align="right">
                            <input id="btnSaveDiscrepancy" type="button" value="Add" onclick="return ValidationForDiscrepancy();" class="btn btnadd" />
                            <%--<input id="btnUpdateDiscrepancy" type="button" value="Update" onclick="return ValidationForDiscrepancy();" class="button" />--%>
                            <input id="btnDCFClose" type="button" onclick="DCFClose();" value="Close" class="btn btnclose" />

                        </td>
                    </tr>
                    <tr></tr>
                    <tr></tr>
                </tbody>
            </table>
            <table>
                <tbody>

                    <tr>
                        <td colspan="2">
                            <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                            <asp:Panel ID="pnlDCFGrid" runat="server" Width="95%" ScrollBars="Auto" Style="max-height: 50px; max-width: 900px; margin: auto;">

                                <td colspan="2" valign="top">
                                    <table id="tblDCF" width="100%">
                                    </table>
                                </td>

                            </asp:Panel>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

        <button id="btnDCFEdit" runat="server" onclientclick="displayBackGroundForDCFEdit();" style="display: none;" />

        <cc1:ModalPopupExtender ID="mpDCFEdit" runat="server" PopupControlID="divDCFEdit" BehaviorID="mpDCFEdit"
            BackgroundCssClass=" divModalBackGroundForEdit" CancelControlID="ImgPopUpCloseDCFEdit" TargetControlID="btnDCFEdit">
        </cc1:ModalPopupExtender>


        <asp:UpdatePanel ID="upDCFEdit" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <td colspan="2" style="font-size: 14px !important; text-align: center" class="Label">

                    <div id="divDCFEdit" runat="server" class="centerModalPopupForEdit" style="background-color: #fff; display: none; width: 30%;">
                        <table style="width: 100%">
                            <tbody>
                                <tr align="center">
                                    <td colspan="2" style="font-size: 14px !important; text-align: center" class="Label">
                                        <center style="width: 90%; float: left;">Reason For update</center>
                                        <img id="ImgPopUpCloseDCFEdit" alt="Close" src="images/close_pop.png" style="float: right; right: 0px; height: 25px;"
                                            onclick="DCFCloseEdit()" />
                                        <hr style="margin-top: 0px; width: 100%; float: left;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="text-align: right">Reason :
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList runat="server" ID="ddlDCFEditReason">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">OR
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="text-align: right">Remarks:
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox runat="server" ID="txtDCFUpdateRemarks" Width="200px" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td align="right" style="text-align: left">
                                        <input type="button" id="btnDCFUpdate" value="Save" class="btn btnsave" onclick="return DCFUpdate('Not')" />
                                        <input type="button" id="btnDCFCancel" value="Close" class="btn btnclose" onclick="return DCFCloseEdit()" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%--  <center>
            <div id="updateProgress" align="center" class="updateProgress" style="display: none; position: absolute; vertical-align: middle !important;">
                <div align="center" style="z-index: 999999999999999 ! important;">
                    <table>
                        <tr>
                            <td style="height: 120px;">
                                <font class="updateText" style="margin-right: 22px">Please Wait.....</font>
                            </td>
                            <td style="height: 120px">
                                <div title="Wait" class="updateImage">
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </center>--%>


        <button id="btnFreezeScreening" runat="server" style="display: none;" />

        <cc1:ModalPopupExtender ID="mpFreezeScreening" runat="server" PopupControlID="divFreezeScreening" BehaviorID="mpFreezeScreening"
            BackgroundCssClass="divModalBackGroundForEdit" CancelControlID="ImgPopUpFreezeClose" TargetControlID="btnFreezeScreening">
        </cc1:ModalPopupExtender>

        <div id="divFreezeScreening" runat="server" class="centerModalPopupForEdit" style="background-color: aliceblue; display: none; overflow: auto; width: 24%; height: auto; max-height: 84%; min-height: auto;">
            <table style="width: 100%">
                <tbody>
                    <tr align="center">
                        <td colspan="2" style="font-size: 14px !important; text-align: center" class="Label">
                            <img id="ImgPopUpFreezeClose" alt="Close" src="images/close_pop.png" style="float: right; right: 0px; height: 25px;" />
                            <hr style="margin-top: 10px;" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="margin-left: 50px !important;">
                            <input type="radio" disabled="disabled" id="rbtnFreeze" name="Freee" value="Freeze">
                            Freeze
                            <input type="radio" disabled="disabled" id="rbtnUnFreeze" name="Freee" value="UnFreeze">
                            UnFreeze
                        </td>

                    </tr>

                    <tr>
                        <td style="text-align: right">Remarks *:
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox runat="server" ID="txtFreezeRemarks" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="2">
                            <input type="submit" value="Save" id="btnFreezeSave" onclick="return SaveFreezeStatus()" class="btn btnsave" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>



        <asp:UpdatePanel ID="upDataEntryControl" runat="server">

            <ContentTemplate>

                <asp:Label ID="lblDataEntrycontroller" runat="server"></asp:Label>
            </ContentTemplate>
            <Triggers>
                <%--<asp:AsyncPostBackTrigger ControlID="ddlGroup" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>--%>
                <asp:AsyncPostBackTrigger ControlID="ddlScreeningDate" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                <%--<asp:AsyncPostBackTrigger ControlID="btnPdf" EventName="Click"></asp:AsyncPostBackTrigger>--%>
                <%--<asp:PostBackTrigger ControlID="btnPdf" />--%>
                <asp:PostBackTrigger ControlID="btnDCFShowHide"></asp:PostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>


        <table style="width: 100%">
            <tr>
                <td colspan="2" valign="top"></td>
            </tr>
        </table>

        <asp:HiddenField ID="HMedex_Medex_PI_Co_I_Designate" runat="server" />
        <asp:HiddenField ID="HMedex_Medex_PICommentsgivenon" runat="server" />
        <asp:HiddenField ID="Ferenhit" runat="server" />
        <asp:HiddenField ID="Celcius" runat="server" />
        <asp:HiddenField ID="HFScreeningDtlNo" runat="server" />
        <asp:HiddenField ID="HFScreeningHdrlNo" runat="server" />
        <asp:HiddenField ID="HFScreeningWorkSpaceID" runat="server" />
        <asp:HiddenField ID="HdnMedexVal" runat="server" />
        <asp:HiddenField ID="HdnSUserid" runat="server" />
        <asp:HiddenField ID="HdnWorkflow" runat="server" />
        <asp:HiddenField ID="HfFolderPath" runat="server" />
        <asp:HiddenField ID="hdnIsOpenAuditTrail" runat="server" />
        <asp:HiddenField ID="hdnPassWord" runat="server" />

        <button id="btnViewDocument" runat="server" style="display: none;" />
        <cc1:ModalPopupExtender ID="mdDocument" runat="server" BackgroundCssClass="modalBackground" BehaviorID="mdDocument"
            CancelControlID="imgViewDocument" PopupControlID="dvdialog" TargetControlID="btnViewDocument">
        </cc1:ModalPopupExtender>
        <div id="dvdialog" class="popupcontrol" style="display: none; width: 75%;">
            <table border="0" cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td id="Td3" class="LabelText" style="text-align: center !important; font-size: 25px !important; width: 97%; font-weight: bold; text-decoration: underline; text-decoration-color: chocolate;">ECG Review</td>
                    <td style="width: 3%">
                        <img id="imgViewDocument" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" style="visibility:hidden" />
                    </td>
                </tr>

                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>

                <tr>
                    <td colspan="2">
                        <table style="width: 100%;">
                            <tr>
                                <td>
                                    <iframe id="ifviewDocument" style="height: 391px; width: 100%;" runat="server"></iframe>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td colspan="2" align="left"></td>
                </tr>

                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>

                <tr>
                    <td></td>
                    <td>
                        <input type="button" id="btnExitECG" name="Exit" value="Exit" class="btn btnnew" onclick="" />
                    </td>
                </tr>
            </table>
        </div>

        <button id="btnViewDocumentList" runat="server" style="display: none;"></button>
        <cc1:ModalPopupExtender ID="mdDocumentList" runat="server" BackgroundCssClass="modalBackground" BehaviorID="mdDocumentList"
            CancelControlID="imgViewDocumentList" PopupControlID="dvdialogList" TargetControlID="btnViewDocumentList">
        </cc1:ModalPopupExtender>
        <div id="dvdialogList" class="popupcontrol" style="display: none; width: 75%;">

            <table border="0" cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td id="Td4" class="LabelText" style="text-align: center !important; font-size: 25px !important; width: 97%; font-weight: bold; text-decoration: underline; text-decoration-color: chocolate;">ECG Review List</td>
                    <td style="width: 3%">
                        <img id="imgViewDocumentList" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table>
                            <tr>
                                <td>                                    
                                    <table id="tblECGReviewList" />
                                    <iframe id="iFrameECG" style="height: 391px; width: 100%;" runat="server" visible="false"></iframe>
                                    <asp:GridView runat="server" ID="gdvDocumentList" AutoGenerateColumns="false" Visible="false">
                                        <Columns>
                                            <asp:BoundField HeaderText="#">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:BoundField>

                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="imgbtnView" ToolTip="View"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField HeaderText="Workspace ID" DataField="vWorkSpaceId">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:BoundField>

                                            <asp:BoundField HeaderText="Subject ID" DataField="vSubjectId">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:BoundField>

                                            <asp:BoundField HeaderText="Screening Date" DataField="dScreenDate">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:BoundField>

                                            <asp:BoundField HeaderText="vECGFile" DataField="vECGFile">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:BoundField>

                                            <asp:BoundField HeaderText="vECGPath" DataField="vECGPath">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:BoundField>

                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>


        <script type="text/javascript">
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_initializeRequest(onUpdating);
            prm.add_endRequest(onUpdated);


            function onUpdating(sender, args) {
                var updateProgressDiv = $get('updateProgress');

                createDiv();
                updateProgressDiv.style.display = '';
                setLayerPosition();
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
                shadow.style.height = $(document).height(); +"px"
                shadow.style.top = winScroll.yScr;
                shadow.style.left = winScroll.xScr;
            }
            window.onresize = setLayerPosition;
            window.onscroll = setLayerPosition;

            jQuery(document).ready(function () {
                function SelectGroup(Group) {
                    var group1 = "Div" + Group
                    $('.dropDownListForGroup').val(group1);
                }
            })

            jQuery(document).ready(function () {
                assignCSS();
                selectTheme();
                SelectCurrentTab();

                if ($("#HdnWorkflow").val() == 20) {
                    $("#btnOk").css("display", "block");
                }
                else {
                    $("#btnOk").css("display", "none");
                }
                var name = 'rblScreeningDate'
                var SelectdValue = $('#ddlScreeningDate option:selected').val();

                if (SelectdValue == undefined) {
                    document.getElementById("btnFreezeUnFreeze").style.display = "none";
                }
                else if (SelectdValue == "N") {
                    document.getElementById("btnFreezeUnFreeze").style.display = "none";
                }
                else if (SelectdValue == "M") {
                    document.getElementById("btnFreezeUnFreeze").style.display = "none";
                }
                else {
                    document.getElementById("btnFreezeUnFreeze").style.display = "";
                }
                if ($("#HdnWorkflow").val() == 20 && SelectdValue != undefined) {
                    document.getElementById("btnFreezeUnFreeze").style.display = "";
                }

                if ($("#HdnWorkflow").val() == 0 && SelectdValue != undefined) {
                    document.getElementById("btnFreezeUnFreeze").disabled = false
                }
                else {
                    document.getElementById("btnFreezeUnFreeze").disabled = true
                }



                var SubjectId = $("#HSubjectId").val()
                var ScreenDate = $('#ddlScreeningDate option:selected').val();

                if (ScreenDate != undefined) {
                    $.ajax({
                        type: "POST",
                        url: "frmSubjectScreening_New.aspx/Proc_ScreeningVersionStatus",
                        data: '{"SubjectId":"' + SubjectId + '","ScreeningDate":"' + ScreenDate + '"}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            var aaDataSet = [];
                            if (data.d != "" && data.d != null) {
                                data = JSON.parse(data.d);
                                if (data == "UnFreeze") {
                                    $('#btnFreezeUnFreeze').addClass("buttonForImageUnFreezeStatus");
                                    $('#btnFreezeUnFreeze').attr('disabled', true);

                                }
                                else if (data == "Freeze") {
                                    $('#btnFreezeUnFreeze').addClass("buttonForImageFreezeStatus");
                                    $('#btnFreezeUnFreeze').attr('disabled', false);
                                }
                                else {
                                    $('#btnFreezeUnFreeze').addClass("buttonForImageUnFreezeStatus");
                                    $('#btnFreezeUnFreeze').attr('disabled', true);
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

                }

                $('input[name="' + name + '"][value="' + SelectdValue + '"]').attr('checked', 'checked');



                $('#imgbtnTranscribe').hide()
                $('#imgbtnFreeze').hide()
                $("#btnPdf").hide()
                if (SelectdValue == "N") {
                    $('#imgbtnTranscribe').hide()
                    $('#imgbtnFreeze').hide()
                    $("#btnPdf").show()
                }
                else {
                    if ($("#HSubjectId").val() != "") {

                        if (SelectdValue != 'undefined' && SelectdValue != 'M') {
                            $('#imgbtnTranscribe').show()
                            $('#imgbtnFreeze').show()
                            $("#btnPdf").show()
                        }
                    }
                }

                if (SelectdValue == "N") {
                    ShowTranscribePopup()
                }

                //if ($('#hdnIsSave').val() == "True") {
                //    msgalert("Record Saved Successfully !");
                //}

            });

            $("#<%=btnRedirect.ClientID %>").click(function () {
                $(this).attr("CommandName", "1").attr("CommandArgument", "2");
            });
            var ACTUAL_SESSIONTIME = "<%= Session.Timeout %>", timerId, sessionFlag = true;
            SessionTimeSet();

            function ClientPopulated(sender, e) {
                SubjectClientShowing('AutoCompleteExtender1', $get('<%= txtSubject.ClientId %>'));
            }

            function OnSelected(sender, e) {
                SubjectOnItemSelected(e.get_value(), $get('<%= txtSubject.clientid %>'),
                $get('<%= HSubjectId.ClientID%>'), document.getElementById('<%= btnSubject.ClientId %>'));
            }

            function ClientPopulated_WorkSpace(sender, e) {
                ProjectClientShowingSchema('AutoCompleteExtenderWorkSpace', $get('<%= txtproject.ClientId %>'));
            }

            function OnSelected_WorkSpace(sender, e) {

                ProjectOnItemSelectedForMsrLog(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'), $get('<%=HClientName.clientid %>'), $get('<%=HProjectNo.clientid %>'));
            }

            function successMessage() {

                msgalert('Record Saved Successfully !');
                return false;
            }
            function ClientPopulated1(sender, e) {
                SubjectClientShowing('AutoCompleteExtender1', $get('<%= txtSubject.ClientId %>'));
            }

            function CheckStandardDateAttr(ele, DateId, MonthId, YearId) {

                CheckStandardDate(ele, DateId, MonthId, YearId)

            }
            $("input:checkbox, label").dblclick(function (event) {

                return false;
            });

            jQuery(window).focus(function () {
                selectTheme();
                return false;
            });

            jQuery("#imgTheme").click(function () {
                jQuery("#divThemeSelection").removeAttr("style");
                jQuery("#divThemeSelection").show();
                return false;
            });

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

                    jQuery("#ctl00_CPHLAMBDA_divExpandable").css({ 'background-color': '#CF8E4C' });
                    jQuery("#ctl00_CPHLAMBDA_LblProjectPreClinical").css({ 'color': 'darkred' });
                    jQuery("#ctl00_CPHLAMBDA_LblProjectsClinincalPhase").css({ 'color': 'darkred' });
                    jQuery("#ctl00_CPHLAMBDA_LblAnalyticalPhase").css({ 'color': 'darkred' });
                    jQuery("#ctl00_CPHLAMBDA_LblDocumentPhase").css({ 'color': 'darkred' });

                    $.each($("#ctl00_CPHLAMBDA_pnltable div table tr td"), function () {
                        $(this).css({ 'color': '#CF8E4C' });
                    });
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

                    $.each($("#ctl00_CPHLAMBDA_pnltable div table tr td"), function () {
                        $(this).css({ 'color': '#33a047' });
                    });

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

                    $.each($("#ctl00_CPHLAMBDA_pnltable div table tr td"), function () {
                        $(this).css({ 'color': '#999966' });
                    });
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

                    $.each($("#ctl00_CPHLAMBDA_pnltable div table tr td"), function () {
                        $(this).css({ 'color': 'Navy' });
                    });
                }
                return false;
            }

            function GetWeightData(Height, Weight, BMI) {
                var weig
                var heig
                var bm
                var arrHeight = Height.split(",")
                var arrWeight = Weight.split(",")
                var arrBMI = BMI.split(",")

                for (i = 0; i < arrWeight.length; i++) {
                    if (document.getElementById(arrWeight[i]) != undefined) {
                        weig = document.getElementById(arrWeight[i]);
                    }
                }
                for (i = 0; i < arrHeight.length; i++) {
                    if (document.getElementById(arrWeight[i]) != undefined) {
                        heig = document.getElementById(arrWeight[i]);
                    }
                }
                for (i = 0; i < arrBMI.length; i++) {
                    if (document.getElementById(arrWeight[i]) != undefined) {
                        bm = document.getElementById(arrWeight[i]);
                    }
                }


                if (document.getElementById(weig.id).disabled == true) {

                    return false
                }
                var SubjectId = document.getElementById('HSubjectId').value;
                $.ajax({
                    type: "post",
                    url: "frmSubjectScreening_New.aspx/GetSubjectWeightData",
                    data: '{"SubjectId":"' + SubjectId + '"}',
                    contentType: "application/json; charset=utf-8",
                    datatype: JSON,
                    async: false,
                    dataType: "json",
                    success: function (data) {
                        if (data.d == "False") {
                            msgalert("There Is No Weight Captured Today For Selected Subject !")
                            document.getElementById(weig.id).readOnly = true;
                            return false
                        }
                        if (data.d != "") {
                            if (document.getElementById(weig.id) != null || document.getElementById(weig.id) != undefined) {
                                document.getElementById(weig.id).value = data.d;
                                FillBMIValue(Height, Weight, BMI);
                                //document.getElementById(weig.id).readOnly = true;
                                return true
                            }

                        }
                    }

                });
                return false
            }
            $('.dropDownListForGroup').change(function () {
                var AllValue = $("#hdnAllGroupId").val()
                var SelecttedValue = $('.dropDownListForGroup option:selected').val()
                $("#hdnSubGroup").val(SelecttedValue)

                var btn = document.getElementById('<%= btnDropDownSelectedIndexChanged.ClientId%>');
                btn.click();

            });

            $('#ddlScreeningDate').change(function () {
                $("[id$='rblScreeningDate']").val($('#ddlScreeningDate option:selected').val());

                var name = 'rblScreeningDate'
                var SelectdValue = $('#ddlScreeningDate option:selected').val();

                $('input[name="' + name + '"][value="' + SelectdValue + '"]').attr('checked', 'checked');

                var btn = document.getElementById('<%= btnDropDownSelectedIndexChanged.ClientId%>');
                btn.click();

            });

            function ShowTranscribePopup() {

                if ($('#ddlScreeningDate option:selected').val() == "N" && $('#ddlGroup option:selected').val() == "Div00000" && $('#hdnUserTypeName').val() == $('#hdnWebConfigUserTypeName').val()) {
                    var pop2 = $get('MpeTranscribeScreening');
                    if ($('#hdnIsModelPopupShow').val() != 'False') {
                        if (pop2 == null) {
                            $find('MpeTranscribeScreening').show();
                            return false;
                        }
                    }
                }
            }

            $("#btnContinue").click(function () {
                if ($('input[id*=rbtnYes]').is(":checked")) {
                }
                else {
                    msgalert("Please Select Transcribe Type !");
                    return false;
                }
                if (new Date($('#txtTranscribeDate').val()) > new Date()) {
                    msgalert('Screening Date Can Not be Greater Than Current Date !')
                    return false;
                }
                if (!$('input[id*=rbtnNo]').is(":checked")) {
                    if ($('#txtTranscribeDate').val() == '') {
                        msgalert("Please Select Screening Date !");
                        return false;
                    }
                    if ($('#txtTranscribeRemarks').val() == '') {
                        msgalert('Please Enter Remarks !');
                        return false;
                    }
                }

                var content = {};
                content.SubjectId = $("#HSubjectId").val()
                content.TranscribeDate = $("#txtTranscribeDate").val()

                $.ajax({
                    type: "POST",
                    url: "frmSubjectScreening_New.aspx/TranscribeScreeningValidation",
                    data: JSON.stringify(content),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        var aaDataSet = [];
                        if (data.d != "" && data.d != null) {
                            data = JSON.parse(data.d);
                            if (data == "Subject Already Screened") {
                                msgalert("Selected Date Alreay Screened For This Subject !")
                                $find('MpeTranscribeScreening').show()
                            }
                            else if (data == "True") {
                                $("#hdnTranscribeDate").val($("#txtTranscribeDate").val())
                                $("#hdnTranscribeRemarks").val($("#txtTranscribeRemarks").val())
                                if ($('#rbtnNo').is(':checked')) {
                                    $("#hdnIsTranscribeYes").val('N')
                                }
                                else {
                                    $("#hdnIsTranscribeYes").val('Y')
                                }
                                $find('MpeTranscribeScreening').hide()
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

            });
            //jQuery(document).ready(function () {
            //    function SelectGroup(Group) {
            //        var group1 = "Div" + Group
            //        $('.dropDownListForGroup').val(group1);
            //    }
            //})


            function TranscribeAuditTrail() {
                var SubjectId = document.getElementById('HSubjectId').value;
                var ScreenDate = document.getElementById('hdnScreeningDate').value;

                $.ajax({
                    type: "post",
                    url: "frmSubjectScreening_New.aspx/TranscribeAuditTrail",
                    data: '{"SubjectId":"' + SubjectId + '","ScreenDate":"' + ScreenDate + '"}',
                    contentType: "application/json; charset=utf-8",
                    datatype: JSON,
                    async: false,
                    success: function (data) {
                        $('#tblTransCribeAuditTrail').attr("IsTable", "has");
                        var aaDataSet = [];
                        var RowId
                        if (data.d != "" && data.d != null) {
                            data = JSON.parse(data.d);
                            for (var Row = 0; Row < data.length; Row++) {
                                var InDataSet = [];
                                var srNo = Row + 1
                                InDataSet.push(srNo, data[Row].vSubjectId, data[Row].dScreenDate, data[Row].IsTranscribe, data[Row].vTranscribeRemarks, data[Row].Modifyby, data[Row].dModifyOn);
                                aaDataSet.push(InDataSet);

                            }

                            if ($("#tblTransCribeAuditTrail").children().length > 0) {
                                $("#tblTransCribeAuditTrail").dataTable().fnDestroy();
                            }
                            $('#tblTransCribeAuditTrail').dataTable({
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
                                "aoColumns": [
                                      {
                                          "sTitle": "#",
                                      },
                                     { "sTitle": "Subject Id" },
                                     { "sTitle": "Screening Date" },
                                    { "sTitle": "Is Transcribe" },
                                    { "sTitle": "Remarks" },
                                    { "sTitle": "Modify By" },
                                    { "sTitle": "Modify On" },
                                ],
                                "oLanguage": {
                                    "sEmptyTable": "No Record Found",
                                }
                            });
                            //$('#tblTransCribeAuditTrail tr:first').css('background-color', '#3A87AD');
                            $find('mpTranscribeAuditTrail').show();
                            return false;
                        }
                    },
                    failure: function (response) {
                        msgalert(response.d);
                        return false;
                    },
                    error: function (response) {
                        msgalert(response.d);
                        return false;
                    }
                });
            }

            function FreezeAuditTrail() {
                var SubjectId = document.getElementById('HSubjectId').value;
                var ScreenDate = document.getElementById('hdnScreeningDate').value;

                $.ajax({
                    type: "post",
                    url: "frmSubjectScreening_New.aspx/FreezeAuditTrail",
                    data: '{"SubjectId":"' + SubjectId + '","ScreenDate":"' + ScreenDate + '"}',
                    contentType: "application/json; charset=utf-8",
                    datatype: JSON,
                    async: false,
                    success: function (data) {
                        $('#tblFreezeAuditTrail').attr("IsTable", "has");
                        var aaDataSet = [];
                        var RowId
                        if (data.d != "" && data.d != null) {
                            data = JSON.parse(data.d);
                            for (var Row = 0; Row < data.length; Row++) {
                                var InDataSet = [];
                                var srNo = Row + 1
                                InDataSet.push(srNo, data[Row].vSubjectId, data[Row].dScreenDate, data[Row].cFreezeStatus, data[Row].vRemarks, data[Row].Modifyby, data[Row].dModifyOn);
                                aaDataSet.push(InDataSet);

                            }

                            if ($("#tblFreezeAuditTrail").children().length > 0) {
                                $("#tblFreezeAuditTrail").dataTable().fnDestroy();
                            }
                            $('#tblFreezeAuditTrail').prepend($('<thead>').append($('#tblFreezeAuditTrail tr:first'))).dataTable({
                                "bStateSave": false,
                                "bPaginate": false,
                                "sPaginationType": "full_numbers",
                                "sDom": '<fr>t<p>',
                                "iDisplayLength": 10,
                                "bSort": false,
                                "bFilter": false,
                                "bDestory": true,
                                "bRetrieve": true,
                                "aaData": aaDataSet,
                                "aoColumns": [
                                      {
                                          "sTitle": "#",
                                      },
                                     { "sTitle": "Subject Id" },
                                     { "sTitle": "Screening Date" },
                                    { "sTitle": "Status" },
                                    { "sTitle": "Remarks" },
                                    { "sTitle": "Modify By" },
                                    { "sTitle": "Modify On" },
                                ],
                                "oLanguage": {
                                    "sEmptyTable": "No Record Found",
                                }
                            });
                            $('#tblFreezeAuditTrail tr:first').css('background-color', '#3A87AD');
                            $find('mpFreezeAuditTrail').show();
                            return false;
                        }
                    },
                    failure: function (response) {
                        msgalert(response.d);
                        return false;
                    },
                    error: function (response) {
                        msgalert(response.d);
                        return false;
                    }
                });
            }



            function ShowDialogBox(title, content, btn1text, btn2text, functionText, parameterList) {
                var btn1css;
                var btn2css;

                if (btn1text == '') {
                    btn1css = "hidecss";
                } else {
                    btn1css = "showcss";
                }

                if (btn2text == '') {
                    btn2css = "hidecss";
                } else {
                    btn2css = "showcss";
                }
                $("#lblMessage").html(content);

                $("#dialog").dialog({
                    resizable: false,
                    title: title,
                    modal: true,
                    width: '400px',
                    height: 'auto',
                    bgiframe: false,
                    hide: { effect: 'scale', duration: 400 },

                    buttons: [
                                    {
                                        text: btn1text,
                                        "class": btn1css,
                                        click: function () {

                                            $("#dialog").dialog('close');
                                            $("#hdnIsSave").val("False")

                                        }
                                    },

                    ]
                });
            }

            function ValidationForDiscrepancy() {
                if (document.getElementById('txtDiscrepancyRemarks').value.trim() == '') {
                    msgalert('Please enter remarks !');
                    document.getElementById('txtDiscrepancyRemarks').value = '';
                    document.getElementById('txtDiscrepancyRemarks').focus();
                    return false;
                }

                var content = {};
                content.ScreeningDCFNo = $("#hdnScreeningDCno").val()
                content.ScreeningDCFDtlNo = $('#hdnMedExScreeningDtlNo').val();
                content.vMedExCode = $('#hfMedexCode').val();
                content.ModifyBy = $("#hdnUserId").val()
                content.Type = "G"
                content.ReasonForUpdate = $("#txtDiscrepancyRemarks").val()
                content.WorkSpaceID = $("#HProjectId").val()

                $.ajax({
                    type: "POST",
                    url: "frmSubjectScreening_New.aspx/GenerateDCF",
                    data: JSON.stringify(content),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        if (data.d != "" && data.d != null) {
                            data = JSON.parse(data.d);
                            if (data == "DCF Already Generated") {
                                $("#txtDiscrepancyRemarks").val("")
                                msgalert("Discrepancy  is Already  Generated On This Field !");
                            }
                            else if (data = true) {
                                if ($("#HProjectId").val() == "") {
                                    DCFSHOWHIDE("", $('#hfMedexCode').val(), "", $('#hdnMedExScreeningDtlNo').val(), "", $("#HFScreeningHdrlNo").val(), "", "0000000000", "")
                                }
                                else {
                                    if ($("#txtproject").val() == "") {
                                        DCFSHOWHIDE("", $('#hfMedexCode').val(), "", $('#hdnMedExScreeningDtlNo').val(), "", $("#HFScreeningHdrlNo").val(), "", "0000000000", "")
                                    }
                                    else {
                                        DCFSHOWHIDE("", $('#hfMedexCode').val(), "", $('#hdnMedExScreeningDtlNo').val(), "", $("#HFScreeningHdrlNo").val(), "", $("#HProjectId").val(), "")
                                    }
                                }

                                msgalert("Discrepancy Generated Successfully !");
                                $("#txtDiscrepancyRemarks").val("")
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

            function DCFSHOWHIDE(Type, MedexCode, buttonId, ScreeningDtlNo, Workspaceid, ScreeninghdrNo, MedexType, WorkSpaceId, Desc) {

                $("#hdnDCFGenerated").val("True");
                $("#hfMedexCode").val(MedexCode);
                $('#hdnMedExScreeningDtlNo').val(ScreeningDtlNo)
                $('#txtDiscrepancyOn').html(Desc)
                var content = {};
                if (WorkSpaceId == "0000000000") {
                    content.ScreeningDCFDtlNo = ScreeninghdrNo
                }
                else {
                    content.ScreeningDCFDtlNo = ScreeninghdrNo
                }
                content.vMedExCode = MedexCode;
                content.WorkSpaceId = WorkSpaceId;

                $.ajax({
                    type: "POST",
                    url: "frmSubjectScreening_New.aspx/GETDCFGRID",
                    data: JSON.stringify(content),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        var aaDataSet = [];

                        if (data.d != "" && data.d != null) {

                            $('#tblDCF').attr("IsTable", "has");

                            data = JSON.parse(data.d);

                            for (var Row = 0; Row < data.length; Row++) {
                                var InDataSet = [];
                                InDataSet.push(data[Row].nScreeningDCFNo, Row + 1, data[Row].cDCFType, data[Row].DCFGeneratedBy, data[Row].DCFGeneratedOn, data[Row].Discrepancy, data[Row].DCFquery, data[Row].Remarks, data[Row].Status, data[Row].DCFUpdatedBy, data[Row].DCFUpdatedOn, "", data[Row].iDcfBy, data[Row].iWorkflowStageId);
                                aaDataSet.push(InDataSet);
                            }

                            otable = $('#tblDCF').dataTable({

                                //"bStateSave": false,
                                //"bPaginate": false,
                                //"sPaginationType": "full_numbers",
                                //"sDom": '<fr>t<p>',
                                //"iDisplayLength": 10,
                                //"bSort": false,
                                //"bFilter": false,
                                //"bDestory": true,
                                //"bRetrieve": true,
                                //"aaData": aaDataSet,

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

                                "fnCreatedRow": function (nRow, aData, iDataIndex) {
                                    if (aData[8] == "Answered" && $("#HdnWorkflow").val() == 0) {

                                    }
                                    else if (aData[8] != "Resolved") {
                                        if (aData[13] >= $("#HdnWorkflow").val()) {
                                            if (aData[13] > $("#HdnWorkflow").val() && ($("#HdnWorkflow").val() == 0)) {
                                                $('td:eq(11)', nRow).append("<a href='#'  OnClick=ShowEditDCF(" + aData[0] + ",'" + aData[8] + "'); return false;'>Update </a>");
                                            }
                                            else if (aData[13] == $("#HdnWorkflow").val()) {
                                                $('td:eq(11)', nRow).append("<a href='#'  OnClick=ShowEditDCF(" + aData[0] + ",'" + aData[8] + "'); return false;'>Update </a>");
                                            }
                                        }
                                        //else {
                                        //    if (aData[13] == $("#HdnWorkflow").val()) {
                                        //        $('td:eq(11)', nRow).append("<a href='#'  OnClick='ShowEditDCF(" + aData[0] + "); return false;'>Update </a>");
                                        //    }
                                        //}
                                        //else if (aData[13] ='5')
                                        //{
                                        //    $('td:eq(11)', nRow).append("<a href='#'  OnClick='ShowEditDCF(" + aData[0] + "); return false;'>Update </a>");
                                        //}
                                    }
                                },

                                "aoColumns": [
                                                  { "sTitle": "DCF No." },
                                                  { "sTitle": "Sr. No." },
                                                  { "sTitle": "DCF Type" },
                                                  { "sTitle": "Created By" },
                                                  { "sTitle": "DCF Date" },
                                                  { "sTitle": "Discrepancy" },
                                                  { "sTitle": "DCF Query" },
                                                  { "sTitle": "Remarks" },
                                                  { "sTitle": "Status" },
                                                  { "sTitle": "Updated By" },
                                                  { "sTitle": "Updated On" },
                                                  { "sTitle": "Update" },
                                                  { "sTitle": "WorkFlowStageId" },
                                ]
                            });

                            if (data.length == 0) {
                                $('#tblDCF_wrapper').css("display", "none")
                            }

                            $('#tblDCF tr').each(function () {
                                $(this).find('th').eq(0).hide();
                                $(this).find('td').eq(0).hide();
                                $(this).find('th').eq(12).hide();
                                $(this).find('td').eq(12).hide();
                            });

                            $('#tblDCF tr:first').css('background-color', '#3A87AD');
                            $find('mpDCFGenerate').show();
                            $("#txtDiscrepancyRemarks").val("")
                            return false;
                        }
                        else {
                            return false;
                        }

                    },
                    failure: function (error) {
                        msgalert(error);
                    }
                });


            }

            function displayBackGround() {
                document.getElementById('<%=ModalBackGround.ClientId %>').style.display = '';
                document.getElementById('<%=ModalBackGround.ClientId %>').style.height = $('#HFHeight').val() + "px";
                document.getElementById('<%=ModalBackGround.ClientID%>').style.width = $('#HFWidth').val() + "px";
            }

            function DCFClose() {
                $("#hdnDCFGenerated").val("False");
                $get('ImgPopUpCloseDCF').click();
                location.reload();
            }

            function ShowEditDCF(nSceeningDCFNo, Status) {
                $("#hdnScreeningDCno").val(nSceeningDCFNo)
                //$find('mpDCFGenerate').hide();

                //  document.getElementById('<%=ModalBackGroundForEdit.ClientId%>').style.display = '';
                //  document.getElementById('<%=ModalBackGroundForEdit.ClientId%>').style.height = $('#HFHeight').val() + "px";
                //  document.getElementById('<%=ModalBackGroundForEdit.ClientID%>').style.width = $('#HFWidth').val() + "px";

                //displayBackGroundForDCFEdit()
                if (Status != "Answered") {
                    $find("mpDCFEdit").show();
                }
                else {
                    DCFUpdate('Validation')
                }



            }
            function DCFCloseEdit() {

                $get('ImgPopUpCloseDCFEdit').click();
                document.getElementById('<%=ModalBackGroundForEdit.ClientId%>').style.display = 'none';

                if ($("#HProjectId").val() == "") {
                    DCFSHOWHIDE("", $('#hfMedexCode').val(), "", $('#hdnMedExScreeningDtlNo').val(), "", $("#HFScreeningHdrlNo").val(), "", "0000000000", "")
                }
                else {
                    DCFSHOWHIDE("", $('#hfMedexCode').val(), "", $('#hdnMedExScreeningDtlNo').val(), "", $("#HFScreeningHdrlNo").val(), "", $("#HProjectId").val(), "")
                }
                //DCFSHOWHIDE("", $('#hfMedexCode').val(), "", $('#hdnMedExScreeningDtlNo').val(), "", $("#HFScreeningHdrlNo").val(), "", $("#HProjectId").val(), "")
            }


            function displayBackGroundForDCFEdit() {
                // document.getElementById('<%=ModalBackGroundForEdit.ClientId%>').style.display = '';
                //   document.getElementById('<%=ModalBackGroundForEdit.ClientId%>').style.height = $('#HFHeight').val() + "px";
                //   document.getElementById('<%=ModalBackGroundForEdit.ClientID%>').style.width = $('#HFWidth').val() + "px";
            }

            function DCFUpdate(validation) {
                if (validation != "Validation") {
                    if ($('#ddlDCFEditReason option:selected').val() == "0" && $("#txtDCFUpdateRemarks").val() == "") {
                        msgalert('Select Either Reason Or Specify Remarks !');
                        return false;
                    }
                    if ($('#ddlDCFEditReason option:selected').val() != "0" && $("#txtDCFUpdateRemarks").val() != "") {
                        msgalert('Select Either Reason Or Specify Remarks !');
                        return false;
                    }
                }

                var nDCFNo = $("#hdnMedExScreeningDtlNo").val();

                var content = {};
                content.ScreeningDCFNo = $("#hdnScreeningDCno").val()
                content.ScreeningDCFDtlNo = $('#hdnMedExScreeningDtlNo').val();
                content.vMedExCode = $('#hfMedexCode').val();
                content.ModifyBy = $("#hdnUserId").val()
                content.Type = "R"

                if ($('#ddlDCFEditReason option:selected').index() == "0") {
                    content.ReasonForUpdate = $("#txtDCFUpdateRemarks").val()

                }
                else {
                    content.ReasonForUpdate = $('#ddlDCFEditReason option:selected').text()
                }


                content.WorkSpaceID = $("#HProjectId").val()


                $.ajax({
                    type: "POST",
                    url: "frmSubjectScreening_New.aspx/GenerateDCF",
                    data: JSON.stringify(content),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        var aaDataSet = [];
                        if (data.d != "" && data.d != null) {
                            data = JSON.parse(data.d);
                            if (data == "DCF Resolved Successfully") {
                                msgalert("DCF Resolved Successfully !")
                                $get('ImgPopUpCloseDCFEdit').click();

                                if ($("#HProjectId").val() == "") {
                                    DCFSHOWHIDE("", $('#hfMedexCode').val(), "", $('#hdnMedExScreeningDtlNo').val(), "", $("#HFScreeningHdrlNo").val(), "", "0000000000", "")
                                }
                                else {
                                    DCFSHOWHIDE("", $('#hfMedexCode').val(), "", $('#hdnMedExScreeningDtlNo').val(), "", $("#HFScreeningHdrlNo").val(), "", $("#HProjectId").val(), "")
                                }
                            }
                            else if (data == "Error While Saving.") {
                                msgalert("Error While Data Saving !")
                            }
                            else if (data == "False ") {
                                msgalert("Error While Data Saving !")
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




            }
            function SHOEDCFFROMVB(id) {
                if (id = "S") {
                    $find("mpDCFGenerate").show();
                    $("#txtDiscrepancyRemarks").val("")
                }
                else {
                    $find("mpDCFGenerate").hide();
                }
            }

            function FreezeUnFreezeScreening() {
                var content = {};
                content.ScreeningDate = $('#ddlScreeningDate option:selected').val();
                content.SubjectId = $("#HSubjectId").val()
                content.vWorkSpaceId = $("#HProjectId").val()

                if ($("#HdnWorkflow").val() != "0") {
                    msgalert("You do not have a writes to unfreeze");
                    return;
                }

                $.ajax({
                    type: "POST",
                    url: "frmSubjectScreening_New.aspx/ScreeningIsFreeze",
                    data: JSON.stringify(content),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        var aaDataSet = [];
                        if (data.d != "" && data.d != null) {
                            data = JSON.parse(data.d);
                            document.getElementById("rbtnUnFreeze").checked = true
                            if (data == 'NOT ALLOWED') {
                                msgalert("Subject is already assigned in a Project,Please reject first !!")
                            }
                            else {
                                $find('mpFreezeScreening').show();
                                return false;
                            }
                            //if (data == "UnFreeze") {
                            //    document.getElementById("rbtnFreeze").checked = true
                            //}
                            //else {
                            //    document.getElementById("rbtnUnFreeze").checked = true
                            //}

                            return false;
                        }
                        else {
                            return false;
                            $find('mpFreezeScreening').show();
                        }
                    },
                    failure: function (error) {
                        msgalert(error);
                        return false;
                    }
                });


            }

            function SaveFreezeStatus() {

                if ($("#txtFreezeRemarks").val() == "") {
                    msgalert("Please enter Remarks !")
                    return false;
                }

                var content = {};
                content.ScreeningDate = $('#ddlScreeningDate option:selected').val();
                content.SubjectId = $("#HSubjectId").val()
                content.FreezeStatus = $('input[name=Freee]:checked').val();
                content.Remarks = $("#txtFreezeRemarks").val();
                content.ModifyBy = $("#hdnUserId").val()

                $.ajax({
                    type: "POST",
                    url: "frmSubjectScreening_New.aspx/Save_ScreeningVersionMst",
                    data: JSON.stringify(content),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        var aaDataSet = [];
                        if (data.d != "" && data.d != null) {
                            data = JSON.parse(data.d);
                            if (data == "Please Reject Subject") {
                                msgalert("Please Rejected Subject First !")
                                $find('mpFreezeScreening').show();
                            }
                            else if (data == "True") {
                                msgalert("Screening Unfreeze Successfully !")
                            }
                            else {
                                msgalert("Error While Save in Data !")
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


            }
            function CloseTranscribePopup() {
                $("#hdnIsModelPopupShow").val("False")
                $find('MpeTranscribeScreening').hide()
            }

            function msgconfirmalert(msg, e) {
                swal({
                    title: "",
                    text: msg,
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: '#EB7140',
                    confirmButtonText: '',
                    closeOnConfirm: false
                },
                function (isConfirm) {
                    if (isConfirm) {


                        swal.close();
                        __doPostBack(e.name, '');
                    } else {
                        return false;
                    }
                });
                return false;
            }

            //function ShowWristBand() {
            //    if ($('#txtIsGunned').is(':checked')) {
            //        $("#txtWristBand").attr("style", "display:none;");
            //        $("#lblWristband").attr("style", "display:none;");
            //        $("#trSubject").attr("style", "display:''");
            //    }
            //    else {
            //        $("#txtWristBand").attr("style", "display:block");
            //        $("#lblWristband").attr("style", "display:block");
            //        $("#lblWristband").attr("style", "width:480px;");
            //        $("#trSubject").attr("style", "display:none;");
            //    }
            //}

        </script>

        <script type="text/javascript" src="Script/Screening.js"></script>
        <script type="text/javascript" src="Script/Login/third-party.min.js"></script>
        <script type="text/javascript" src="Script/Login/modules.min.js"></script>
    </form>
</body>
</html>


