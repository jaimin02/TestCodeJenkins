<%@ page title="" language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmVisitScheduler, App_Web_ybumpksz" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <link rel="stylesheet" type="text/css" href="App_Themes/jquery.multiselect.css" />
    <link rel="Stylesheet" type="text/css" href="App_Themes/StyleBlue/UI_Theme/jquery-ui.css" />
    <asp:Button ID="btnRefreshGrid" runat="server" CssClass="button" Text="Save" Style="display: none;" />
    <asp:Label runat="server" ID="txtVisit" Text="" Style="display: none" />
    <asp:Label runat="server" ID="txtRandomisation" Text="" Style="display: none" />
    <asp:Label runat="server" ID="lbliNodeId" Text="" Style="display: none" />
    <asp:Label runat="server" ID="txtActualVisit" Text="" Style="display: none" />
    <asp:Label runat="server" ID="txtActualRandomisation" Text="" Style="display: none" />
    <asp:Label runat="server" ID="lblModifyby" Text="" Style="display: none" />
    <asp:Label runat="server" ID="lblSubjectId" Text="" Style="display: none" />
    <div>
        <fieldset class="FieldSetBox" style="display: block; width: 98%; text-align: left; border: #aaaaaa 1px solid;">
                    <legend class="LegendText" style="color: Black; font-size: 12px">
                        <img id="img2" alt="Activity Group Details" src="images/panelcollapse.png"
                            onclick="Display(this,'divActivityDetail');" runat="server" style="margin-right: 2px;" />Project Details</legend>
        <center>
            <table cellpadding="5" style="width: 90%;">
                <tbody>

                    <tr>
                        
                        <td style="text-align: right; vertical-align: top; white-space: nowrap; width: 20%;"
                            class="Label">Project Name/Project No* :
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="70%" TabIndex="1">
                            </asp:TextBox>
                            <asp:Button Style="display: none" ID="btnSetProject" runat="server" Text=" Project"></asp:Button>
                            <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                                TargetControlID="txtProject" ServicePath="AutoComplete.asmx" OnClientShowing="ClientPopulated"
                                OnClientItemSelected="OnSelected" MinimumPrefixLength="1" CompletionListItemCssClass="autocomplete_listitem"
                                CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListCssClass="autocomplete_list"
                                BehaviorID="AutoCompleteExtender1" CompletionListElementID="pnlProjectList" ServiceMethod="GetMyProjectCompletionList">
                            </cc1:AutoCompleteExtender>
                            <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" />
                        </td>
                        
                    </tr>
                    <tr>
                        <td colspan="2">
                            <center>
                                
                            </center>
                        </td>
                    </tr>
                </tbody>
            </table>
        </center>
            </fieldset>
        <br />
        <br />
        <br />
        <fieldset class="FieldSetBox" style="display: block; width: 98%; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img1" alt="Activity Group Data" src="images/panelcollapse.png"
                                        onclick="Display(this,'divActivityData');" runat="server" style="margin-right: 2px;" />Visited Schedule Detail</legend>
        <center>
            <asp:UpdatePanel ID="upScheduler" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button runat="server" ID="btnExporttoExcel" OnClick="btnExporttoExcel_Click"  CssClass="btn btnexcel" Visible="false"  Style="" ></asp:Button>
                    <table runat="server" style="width: 100%">
                        <tr runat="server" align="center">
                            <td>
                                
                                <div runat="server" id="dvScheduler" class="dvScheduler" align="center">
                                    <asp:GridView ID="gvScheduler" runat="server" class="dvScheduler" AutoGenerateColumns="true" Width="99%" >
                                        <RowStyle HorizontalAlign="Center" />
                                        <AlternatingRowStyle HorizontalAlign="Center" />
                                    </asp:GridView>

                                    <asp:GridView ID="gvSchedulerExport" style="display:none;" runat="server" class="dvScheduler" AutoGenerateColumns="true" Width="99%" SkinID="grdViewSmlAutoSize">
                                        <RowStyle HorizontalAlign="Center" />
                                        <AlternatingRowStyle HorizontalAlign="Center" />
                                    </asp:GridView>

                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnSchedule" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnVisit" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="gvScheduler" EventName="RowCommand" />
                    <asp:PostBackTrigger ControlID="btnExporttoExcel" />
                </Triggers>
            </asp:UpdatePanel>
        </center>

            </fieldset>

        <asp:UpdatePanel ID="upModel" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="ModalBackGround" runat="server" class="divModalBackGround" style="display: none; width: 100% !Important;"></div>
                <button id="btnModel" runat="server" style="display: none;" class="btn btnnew" />

                <div id="divScheduledDate" runat="server" class="centerModalPopup  divScheduledDate" style="display: none; left: 30%; width: 28%; position: absolute; top: 525px; border: 1px solid; height: 300px;">
                    <div style="background-color: #1560A1;">
                        <table style="width: 90%; margin: auto;">
                            <tr>
                                <td colspan="2" class="LabelText" style="text-align: center !important; color: white; font-size: 14px !important; width: 97%;">
                                    <b>
                                        <asp:Label runat="server" ID="lblHeader" Text="Visit Information"></asp:Label>
                                    </b>

                                </td>

                                <td style="text-align: center; height: 22px;" valign="top">
                                    <img id="CancelRemarks" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px; cursor: pointer;"
                                        title="Close" />
                                </td>
                            </tr>
                        </table>
                        <hr />
                    </div>
                    <table style="margin: 10px 10px 10px 10px;">


                        <tr style="margin: 10px 10px 10px 10px;">

                            <td class="Label" align="right">Site No :
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblSiteNo"></asp:Label>
                            </td>
                        </tr>
                        <tr style="margin: 10px 10px 10px 10px;">

                            <td class="Label" align="right">Randomization no:
                            </td>

                            <td>
                                <asp:Label runat="server" ID="lblRandomisationno"></asp:Label>
                            </td>
                        </tr>
                        <tr style="margin: 10px 10px 10px 10px;">

                            <td class="Label" align="right">Visit :
                            </td>
                            <td>
                                <asp:Label runat="server" for="myRan" ID="lblVisit"></asp:Label>
                            </td>
                        </tr>
                        <%--   <tr style="margin: 10px 10px 10px 10px;" id="trScreenFailure" runat="server">

                            <td class="Label" align="right">ScreenFailure :
                            </td>
                            <td align="center">
                                <asp:RadioButtonList runat="server" ID="rbtScreen" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr style="margin: 10px 10px 10px 10px;" id="trDiscountinut" runat="server">

                            <td class="Label" align="right">DisCountinue :
                            </td>
                            <td align="center">
                                <asp:RadioButtonList runat="server" ID="rbtDiscountinue" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>--%>


                        <tr style="margin: 10px 10px 10px 10px;">

                            <td class="Label" align="right">
                                <asp:Label runat="server" ID="lblSche" Text="Scheduled Date:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtScheduledDate" CssClass="textBox"></asp:TextBox>
                                <cc1:CalendarExtender runat="server" TargetControlID="txtScheduledDate" Format="dd-MMM-yyyy">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnSave" runat="server" CssClass="btn btnsave" Text="Save" OnClientClick="return SaveScheduler();"  />
                                        <asp:Button ID="btnActualSave" runat="server" CssClass="btn btnsave" OnClientClick="SaveActualDate();" Text="Save"  />
                                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" Text="Cancel" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>

                <cc1:ModalPopupExtender ID="ModalScheduler" runat="server" PopupControlID="divScheduledDate"
                    BackgroundCssClass="modalBackground" TargetControlID="btnModel" BehaviorID="ModalScheduler"
                    CancelControlID="CancelRemarks">
                </cc1:ModalPopupExtender>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnActualSave" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>

        <asp:Button runat="server" Style="display: none;" ID="btnSchedule" CssClass="btn btnnew" />
        <asp:Button runat="server" Style="display: none;" ID="btnVisit" CssClass="btn btnnew"/>

        <asp:Button ID="btnAuditActivity" runat="server" Style="display: none" CssClass="btn btnnew"></asp:Button>

        <cc1:ModalPopupExtender ID="mdlAuditActivity" runat="server" BackgroundCssClass="modalBackground" BehaviorID="mdlAuditActivity"
            CancelControlID="imgActivityAuditTrail" PopupControlID="dvActivityAudiTrail"
            TargetControlID="btnAuditActivity">
        </cc1:ModalPopupExtender>

        <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpActivityAuditTrail" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div id="dvActivityAudiTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
                                <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                    <tr>
                                        <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 97%;">Audit Trail Information</td>
                                        <td style="width: 3%">
                                            <img id="imgActivityAuditTrail" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <table align="center" border="0" cellpadding="2" cellspacing="2">
                                                <tr>
                                                    <td>
                                                        <table id="tblActivityAudit" class="tblAudit" style="background-color: aliceblue;"></table>
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
                </td>
            </tr>
        </table>


    </div>

    <link href="App_Themes/StyleBlue/StyleBlue.css" rel="stylesheet" type="text/css" />
    <%--<script src="Script/jquery.dataTables.min.js" type="text/javascript"></script>--%>
    <script src="Script/DatatableScheduler.js" type="text/javascript"></script>
    <script src="Script/jquery.dataTables.js" type="text/javascript"></script>
    <script src="Script/jquery.dataTables.plugins.js" type="text/javascript"></script>
    <script src="Script/TableTools.min.js" type="text/javascript"></script>


    <style type="text/css">
        .image1 {
            position: relative;
            left: 14px;
            top: 6px;
        }

        .dataTables_scrollBody {
            min-height: 70px;
            max-height: 300px;
            overflow: scroll;
        }
        .dataTables_info, .dataTables_length, .dataTables_filter {
            color : white;
        }
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

    </style>
    <script type="text/javascript" language="javascript">

        $(document).ready(function () {
            $(window).keydown(function (event) {
                if (event.keyCode == 13) {
                    event.preventDefault();
                    return false;
                }
            });

        });
        function HideActivityGroupDetails() {
            $('#<%= img2.ClientID%>').click();
        }
        function Display(control, target) {
            if (control.src.toString().toUpperCase().search("EXPAND") != -1) {
                $("#" + target).slideToggle(600);
                control.src = "images/panelcollapse.png";
            }
            else {
                $("#" + target).slideToggle(600);
                control.src = "images/panelexpand.png";
            }
        }

        function pageLoad() {
            if ($get('<%= gvScheduler.ClientID()%>') != null && $get('<%= gvScheduler.ClientID%>_wrapper') == null) {

                if (jQuery('#<%= gvScheduler.ClientID%>')) {
                    jQuery('#<%= gvScheduler.ClientID%>').prepend($('<thead>').append($('#<%= gvScheduler.ClientID%> tr:first'))).DataTable({
                        "bJQueryUI": true,
                        "sScrollY": '200px;',
                        "sScrollY": true,
                        "scrollCollapse": true,
                        "sScrollX": '400px;',
                        "scrollX": true,
                        "sPaginationType": "full_numbers",
                        "bFooter": false,
                        "bHeader": false,
                        "AutoWidth": true,
                        "bSort": false,
                        "fixedHeader": true,
                        "oLanguage": { "sSearch": "Search" },
                        "oTableTools": {
                            "aButtons": [
                                "xls"
                            ],
                            "sSwfPath": "../Script/swf/copy_cvs_xls_pdf.swf"
                        }

                    });
                }
                $(".dataTables_wrapper").css("width", ($(window).width() * 0.85 | 0) + "px");
            }
        }
        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        function OpenMOdelPopup(id) {
            var Ran = id.split("_")[6];
            var SubjectId = id.split("_")[7];
            var type = id.split("_")[4];
            var iNodeId = id.split("_")[8];
            id = id.split("_")[5];

            $("#ctl00_CPHLAMBDA_txtVisit").html(id)
            $("#ctl00_CPHLAMBDA_txtRandomisation").html(Ran)
            $("#ctl00_CPHLAMBDA_lblVisit").html(id)
            $("#ctl00_CPHLAMBDA_lblRandomisationno").html(Ran)
            $("#ctl00_CPHLAMBDA_lblSubjectId").html(SubjectId)
            $("#ctl00_CPHLAMBDA_lbliNodeId").html(iNodeId)

            if (type == "Scheduled") {
                var btn = document.getElementById('<%= btnSchedule.ClientId %>');
                btn.click();
            }
            else {
                var btn = document.getElementById('<%= btnVisit.ClientId %>');
                btn.click();
            }
            return false;
        }

        function OpenActualMOdelPopup(id) {
            var Ran = id.split("_")[6];
            var SubjectId = id.split("_")[7];
            var type = id.split("_")[4];
            id = id.split("_")[5];
            $("#ctl00_CPHLAMBDA_txtActualVisit").html(id)
            $("#ctl00_CPHLAMBDA_txtActualRandomisation").html(Ran)
            $("#ctl00_CPHLAMBDA_lblActualVisit").html(id)
            $("#ctl00_CPHLAMBDA_lblActualRandomisationNo").html(Ran)
            $("#ctl00_CPHLAMBDA_lblSubjectId").html(SubjectId)

            if (type == "Scheduled") {
                var btn = document.getElementById('<%= btnSchedule.ClientId %>');
                btn.click();
            }
            else {
                var btn = document.getElementById('<%= btnVisit.ClientId %>');
                btn.click();
            }
            var btn = document.getElementById('<%= btnVisit.ClientId %>');
            btn.click();
            return false;
        }



        function AssignData() {
            $("#ctl00_CPHLAMBDA_lblVisit").html($("#ctl00_CPHLAMBDA_txtVisit").html())
            $("#ctl00_CPHLAMBDA_lblRandomisationno").html($("#ctl00_CPHLAMBDA_txtRandomisation").html())
        }
        function AssignDataForActual() {
            $("#ctl00_CPHLAMBDA_lblActualVisit").html($("#ctl00_CPHLAMBDA_txtActualVisit").html())
            $("#ctl00_CPHLAMBDA_lblActualRandomisationNo").html($("#ctl00_CPHLAMBDA_txtActualRandomisation").html())
        }



        function SaveScheduler() {
            var WorkSpaceId
            WorkSpaceId = document.getElementById('<%= HProjectId.ClientID%>').value;
            var randomizationno = $("#ctl00_CPHLAMBDA_txtRandomisation").text()
            var visitname = $("#ctl00_CPHLAMBDA_lblVisit").text()
            var scheduleddate = document.getElementById('<%= txtScheduledDate.ClientID%>').value;
            var modifyby = $("#ctl00_CPHLAMBDA_lblModifyby").text()
            var SubjectId = $("#ctl00_CPHLAMBDA_lblSubjectId").text()
            var iNodeid = $("#ctl00_CPHLAMBDA_lbliNodeId").text()

            if (scheduleddate == '') {
                msgalert("Please Enter Schedule Date. !");
                return false;
            }


            $.ajax({
                type: "post",
                url: "frmVisitScheduler.aspx/SaveVisitScheduler",
                data: '{"WorkSpaceId":"' + WorkSpaceId + '","randimisationno":"' + randomizationno + '","VisitName":"' + visitname + '","ScheduledDate":"' + scheduleddate + '","ModifyBy":"' + modifyby + '","SubjectId":"' + SubjectId + '","iNodeId":"' + iNodeid + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                success: function (data) {
                    if (data.d == "Success") {
                        msgalert('Data Saved Successfully')
                        var btn = document.getElementById('<%= btnRefreshGrid.ClientId %>');
                        btn.click();
                    }
                    else {
                        msgalert('There is a error while Saving')
                    }

                },
                failure: function (data) {
                    msgalert(data.d);
                },
                error: function (data) {
                    msgalert(data.d);
                }
            });
        }

        function SaveActualDate() {

            WorkSpaceId = document.getElementById('<%= HProjectId.ClientID%>').value;
            var randomizationno = $("#ctl00_CPHLAMBDA_txtRandomisation").text()
            var visitname = $("#ctl00_CPHLAMBDA_lblVisit").text()
            var ActualDate = document.getElementById('<%= txtScheduledDate.ClientID%>').value;
            var modifyby = $("#ctl00_CPHLAMBDA_lblModifyby").text()
            var SubjectId = $("#ctl00_CPHLAMBDA_lblSubjectId").text()
            var iNodeid = $("#ctl00_CPHLAMBDA_lbliNodeId").text()
            var Screenfailure = ''
            var discountinue = ''
            if (ActualDate == '') {
                msgalert("Please Enter Schedule/Actual Date.");
                return false;
            }

            var lblHeader = $("#ctl00_CPHLAMBDA_lblHeader").text()
            var now = new Date();
            if (lblHeader.indexOf("Actual") != -1) {
                if (new Date(ActualDate) > now) {
                    msgalert("Date should be less than or equal to current date.");
                    return false;
                }
            }

            $.ajax({
                type: "post",
                url: "frmVisitScheduler.aspx/SaveVisitActual",
                data: '{"WorkSpaceId":"' + WorkSpaceId + '","dActualDate":"' + ActualDate + '","ModifyBy":"' + modifyby + '","SubjectId":"' + SubjectId + '","VisitName":"' + visitname + '","iNodeId":"' + iNodeid + '","ScreenFailure":"' + Screenfailure + '","Discountinue":"' + discountinue + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                success: function (data) {
                    if (data.d == "Success") {
                        msgalert('Data Saved Successfully')
                        var btn = document.getElementById('<%= btnRefreshGrid.ClientId %>');
                        btn.click();
                    }
                    else {
                        msgalert('There is a error while Saving')
                    }

                },
                failure: function (data) {
                    msgalert(data.d);
                },
                error: function (data) {
                    msgalert(data.d);
                }
            });
        }

        function AuditTrail(id) {

            WorkSpaceId = document.getElementById('<%= HProjectId.ClientID%>').value;
            var SubjectId = id.split("_")[7];
            var iNodeid = id.split("_")[8];

            $.ajax({
                type: "post",
                url: "frmVisitScheduler.aspx/ActualVisitAuditTrail",
                data: '{"WorkSpaceId":"' + WorkSpaceId + '","SubjectId":"' + SubjectId + '","iNodeId":"' + iNodeid + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                success: function (data) {
                    var aaDataSet = [];
                    if (data.d != "" && data.d != null) {
                        data = JSON.parse(data.d);
                        for (var Row = 0; Row < data.length; Row++) {
                            var InDataSet = [];
                            InDataSet.push(data[Row].SrNo, data[Row].ProjectNo, data[Row].Actitivity, data[Row].ActualDate, data[Row].ModifyBy, data[Row].ModifyOn);
                            aaDataSet.push(InDataSet);
                        }
                    }

                    if ($("#tblActivityAudit").children().length > 0) {
                        $("#tblActivityAudit").dataTable().fnDestroy();
                    }
                    $('#tblActivityAudit').prepend($('<thead>').append($('#tblActivityAudit tr:first'))).dataTable({
                        "bJQueryUI": true,
                        "sPaginationType": "full_numbers",
                        "bLengthChange": false,
                        "iDisplayLength": 10,
                        "bProcessing": true,
                        "bSort": false,
                        "bDestroy": true,
                        "aaData": aaDataSet,
                        "aoColumns": [
                            { "sTitle": "Sr. No." },
                            { "sTitle": "Project No" },
                             { "sTitle": "Activity" },
                            { "sTitle": "Actual Date" },
                            { "sTitle": "Modify By" },
                            { "sTitle": "Modify On" },
                        ],
                        "oLanguage": {
                            "sEmptyTable": "No Record Found",
                        }
                    });
                    $find('mdlAuditActivity').show();
                },
                failure: function (data) {
                    msgalert(data.d);
                },
                error: function (data) {
                    msgalert(data.d);
                }
            });
        }

    </script>
</asp:Content>



