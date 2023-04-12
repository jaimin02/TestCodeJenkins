<%@ page title="" language="VB" masterpagefile="~/ECTDMasterPage.master" autoeventwireup="false" inherits="frmSubjectPIFAuditTrial, App_Web_w1bzwbih" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <style type="text/css">
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
            color:white !important;
        }
        .ajax__calendar_container {
            z-index:1;
        }
    </style>
    <script type="text/javascript" src="Script/jquery-ui.js"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <script type="text/javascript" src="Script/General.js"></script>
    <div id="Div1" runat="server">
        <asp:GridView runat="server" ID="gvExport" AutoGenerateColumns="true" Style="display: none"
            HeaderStyle-BackColor="#1560a1" HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" Header-style-font-size="10px !importnat" HeaderStyle-Font-Names="Verdana, Arial, Helvetica, sans-serif"
            HeaderStyle-Font-Size=" 0.9em"
            HeaderStyle-HorizontalAlign="Center" CellPadding="3" CellSpacing="0"
            RowStyle-Font-Names=" Verdana, Arial, Helvetica, sans-serif" RowStyle-Font-Size="10pt"
            RowStyle-BackColor="#84c8e6" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#191970" RowStyle-ForeColor="#191970">
        </asp:GridView>
        <asp:GridView runat="server" ID="GridView1" AutoGenerateColumns="true" Style="display: none"
            HeaderStyle-BackColor="#1560a1" HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" Header-style-font-size="10px !importnat" HeaderStyle-Font-Names="Verdana, Arial, Helvetica, sans-serif"
            HeaderStyle-Font-Size=" 0.9em"
            HeaderStyle-HorizontalAlign="Center" CellPadding="3" CellSpacing="0"
            RowStyle-Font-Names=" Verdana, Arial, Helvetica, sans-serif" RowStyle-Font-Size="10pt"
            RowStyle-BackColor="#84c8e6" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#191970" RowStyle-ForeColor="#191970">
        </asp:GridView>
        <asp:GridView runat="server" ID="GridView2" AutoGenerateColumns="true" Style="display: none"
            HeaderStyle-BackColor="#1560a1" HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" Header-style-font-size="10px !importnat" HeaderStyle-Font-Names="Verdana, Arial, Helvetica, sans-serif"
            HeaderStyle-Font-Size=" 0.9em"
            HeaderStyle-HorizontalAlign="Center" CellPadding="3" CellSpacing="0"
            RowStyle-Font-Names=" Verdana, Arial, Helvetica, sans-serif" RowStyle-Font-Size="10pt"
            RowStyle-BackColor="#84c8e6" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#191970" RowStyle-ForeColor="#191970">
        </asp:GridView>
        <asp:GridView runat="server" ID="GridView3" AutoGenerateColumns="true" Style="display: none"
            HeaderStyle-BackColor="#1560a1" HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" Header-style-font-size="10px !importnat" HeaderStyle-Font-Names="Verdana, Arial, Helvetica, sans-serif"
            HeaderStyle-Font-Size=" 0.9em"
            HeaderStyle-HorizontalAlign="Center" CellPadding="3" CellSpacing="0"
            RowStyle-Font-Names=" Verdana, Arial, Helvetica, sans-serif" RowStyle-Font-Size="10pt"
            RowStyle-BackColor="#84c8e6" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#191970" RowStyle-ForeColor="#191970">
        </asp:GridView>
        <asp:GridView runat="server" ID="GridView4" AutoGenerateColumns="true" Style="display: none"
            HeaderStyle-BackColor="#1560a1" HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" Header-style-font-size="10px !importnat" HeaderStyle-Font-Names="Verdana, Arial, Helvetica, sans-serif"
            HeaderStyle-Font-Size=" 0.9em"
            HeaderStyle-HorizontalAlign="Center" CellPadding="3" CellSpacing="0"
            RowStyle-Font-Names=" Verdana, Arial, Helvetica, sans-serif" RowStyle-Font-Size="10pt"
            RowStyle-BackColor="#84c8e6" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#191970" RowStyle-ForeColor="#191970">
        </asp:GridView>
    </div>
    <asp:UpdatePanel ID="upPnlWorkspaceSubjectMst" runat="server">
        <ContentTemplate>
            <table style="width: 60% !important;">
                <tr>
                    <td style="width: 100%;">
                        <fieldset class="FieldSetBox" style="display: block; width: 96%; text-align: left; border: #aaaaaa 1px solid;">
                            <legend class="LegendText" style="color: Black; font-size: 12px">
                                <img id="img1" alt="Project Details" src="images/panelcollapse.png" class="HideShowFilter"
                                    onclick="Display(this,'divFilter');" runat="server" style="margin-right: 2px;" />Project Search</legend>
                            <div id="divFilter">
                                <table style="width: 100%;">
                                    <tr>
                                        <td colspan="4" style="text-align: -webkit-center;padding:2% 2%;">
                                            <asp:RadioButtonList runat="server" ID="rbFilterType" RepeatColumns="3" CssClass="RBList">
                                                <asp:ListItem Value="0" Text="Project wise" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Subject wise"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Date wise"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr class="clsProjectFilter">
                                        <td style="text-align: right; width: 20%;" class="LabelText SearchLabel">Project Name* :</td>
                                        <td style="text-align: left; width: 50%">
                                            <asp:TextBox ID="txtproject" TabIndex="2" runat="server" CssClass="textBox" Style="width: 65%;"/>
                                            <asp:Button ID="btnSetProject" runat="server" Style="display: none"
                                                Text=" Project" />
                                            <asp:HiddenField ID="HProjectId" runat="server" />
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtenderProject" runat="server" BehaviorID="AutoCompleteExtenderProject"
                                                CompletionListCssClass="autocomplete_list" CompletionSetCount="10" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelectedProject"
                                                OnClientShowing="ClientPopulatedProject" ServiceMethod="GetMyProjectCompletionList"
                                                ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True"
                                                CompletionListElementID="pnlProjectList">
                                            </cc1:AutoCompleteExtender>
                                            <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" />
                                        </td>
                                    </tr>
                                    <tr class="clsSubjectFilter HideConroll">
                                        <td style="text-align: right; width: 20%;" class="LabelText SearchLabel">Subject Name* :</td>
                                        <td style="text-align: left; width: 50%">
                                            <asp:TextBox ID="txtSubject" TabIndex="2" runat="server" CssClass="textBox" Style="width: 65%;" onkeydown="return (event.keyCode!=13)"/>
                                            <asp:Button Style="display: none" TabIndex="1" ID="btnEdit" runat="server" Text=" Edit "
                                                ToolTip="Edit" CssClass="btn btnsave" />
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtenderSubject" runat="server" ServicePath="AutoComplete.asmx"
                                                OnClientShowing="ClientPopulated" CompletionSetCount="10" OnClientItemSelected="OnSelected"
                                                UseContextKey="True" MinimumPrefixLength="1" ServiceMethod="GetAllSubjectCompletionList"
                                                CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtenderSubject"
                                                TargetControlID="txtSubject">
                                            </cc1:AutoCompleteExtender>
                                            <asp:HiddenField ID="HSubjectId" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="HSelectedSubjectId" runat="server"></asp:HiddenField>

                                        </td>
                                    </tr>
                                    <tr class="clsDateFilter HideConroll">
                                        <td style="text-align: right; width: 20%;" class="LabelText SearchLabel">From Date* :</td>
                                        <td style="text-align: left; width: 50%">
                                            <asp:TextBox ID="txtFromDate" TabIndex="2" runat="server" CssClass="textBox" Style="width: 30%;margin-right:2%;" ReadOnly="true"/>
                                            To Date* :
                                            <asp:TextBox ID="txtToDate" TabIndex="2" runat="server" CssClass="textBox" Style="width: 30%;" ReadOnly="true"/>
                                            <asp:HiddenField ID="hdnFromDate" runat="server" />
                                            <asp:HiddenField ID="hdnToDate" runat="server" />
                                            <cc1:CalendarExtender ID="ceFromDate" runat="server" TargetControlID="txtFromDate"
                                                Format="dd-MMM-yyyy">
                                            </cc1:CalendarExtender>
                                            <cc1:CalendarExtender ID="ceToDate" runat="server" TargetControlID="txtToDate"
                                                Format="dd-MMM-yyyy">
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center;padding:2% 2%;" colspan="4" class="Label">
                                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btnnew"
                                                OnClientClick="return SubjectList();" TabIndex="4" Text="Search" ToolTip="Search" />
                                            <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel"
                                                Text="Cancel" ToolTip="Cancel" OnClientClick="return Clear();" />
                                            <asp:Button ID="btnClose" runat="server" CssClass="btn btnexit"
                                                OnClientClick="return msgconfirmalert('Are You Sure You Want To Exit?',this);" TabIndex="5"
                                                Text="Exit" ToolTip="Exit" />
                                                <input type="button" id="btnExportSubjectList" class="btn btnexcel" onclick="return CheckFilter('SubjectList');" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                        <fieldset class="FieldSetBox" style="display: block; width: 96%; text-align: left; border: #aaaaaa 1px solid;">
                            <legend class="LegendText" style="color: Black; font-size: 12px">
                                <img id="img2" alt="Project Details" src="images/panelcollapse.png" class="HideShowSubject"
                                    onclick="Display(this,'divSubjectDetail');" runat="server" style="margin-right: 2px;" />Subject Details</legend>
                            <div id="divSubjectDetail">
                                <%--<table cellpadding="0" cellspacing="0" border="0" class="display" id="example"></table>--%>
                                <table id="tblSubjectDetail" width="100%"></table>
                                
                                <!-- Start -->
                                <div id="dvUserGroupMstAudiTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 90%; height: 100%; max-height: 75%; min-height: 200px;">
                                    <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                        <tr>
                                            <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 97%;">
                                                <asp:Label ID="lblRamge" runat="server" Text="Audit Trail Information"></asp:Label>
                                            </td>
                                            <td style="width: 3%">
                                                <img id="imgClientAuditTrail" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <hr />
                                            </td>
                                        </tr>
                                    </table>
                                    <div id="tabs" style="width: 99%;">
                                        <ul>
                                            <li><b><a href="#PersionalInfo">Personal Information Audit Trial</a></b></li>
                                            <li><b><a href="#PersionalProofInfo">Proof Audit Trial</a></b></li>
                                            <li><b><a href="#PersionalHabitInfo">Habit Detail Audit Trial</a></b></li>
                                            <li><b><a href="#FemaleInfo">Female Detail Audit Trial</a></b></li>
                                            <li><b><a href="#ContactInfo">Contact Detail Audit Trial</a></b></li>
                                        </ul>
                                        <div id="PersionalInfo" style="width: 98%;">
                                            <table id="tblPersonalDetailAuditTrial" width="100%"></table>
                                        </div>
                                        <div id="PersionalProofInfo" style="width: 98%;">
                                            <table id="tblPersonalProofDetailAuditTrial" width="100%"></table>
                                        </div>
                                        <div id="PersionalHabitInfo" style="width: 98%;">
                                            <table id="tblPersonalHabitDetailAuditTrial" width="100%"></table>
                                        </div>
                                        <div id="FemaleInfo" style="width: 98%;">
                                            <table id="tblFemaleDetailAuditTrial" width="100%"></table>
                                        </div>
                                        <div id="ContactInfo" style="width: 98%;">
                                            <table id="tblContactDetailAuditTrial" width="100%"></table>
                                        </div>
                                    </div>
                                    <input type="button" id="btnExportSubjectAuditTrial" class="btn btnexcel" onclick="return CheckFilter('AuditTrial');" />
                                </div>
                                <button id="btn3" runat="server" style="display: none;" />
                                <cc1:ModalPopupExtender ID="MPE_UserGroupMstHistory" runat="server" PopupControlID="dvUserGroupMstAudiTrail" BehaviorID="MPE_UserGroupMstHistory"
                                    PopupDragHandleControlID="LbldRandomizationDtl" BackgroundCssClass="modalBackground" CancelControlID="imgClientAuditTrail"
                                    TargetControlID="btn3">
                                </cc1:ModalPopupExtender>
                                <!-- End -->
                            </div>
                        </fieldset>
                    </td>
                </tr>
            </table>
            
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportToExcelSubject" />
            <asp:PostBackTrigger ControlID="btnExportToExcelAuditTrial" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Button ID="btnExportToExcelSubject" runat="server" Style="display: none;" />
    <asp:Button ID="btnExportToExcelAuditTrial" runat="server" Style="display: none;" />
<!--Modal: Login / Register Form-->

    <script type="text/javascript" src="Script/scrollablegrid.js"></script>
    <script type="text/javascript">
        jQuery.browser = {};
        (function () {
            jQuery.browser.msie = false;
            jQuery.browser.version = 0;
            if (navigator.userAgent.match(/MSIE ([0-9]+)\./)) {
                jQuery.browser.msie = true;
                jQuery.browser.version = RegExp.$1;
            }
        })();
        $(window).resize(function () {
            if ($('#tblSubjectDetail thead').length) {
                if ($('#tblSubjectDetail').DataTable().length) {
                    $('#tblSubjectDetail').DataTable().fnAdjustColumnSizing();
                }
            }
            if ($("#" + $(".ui-tabs-active").attr("aria-controls")).find("table")[1] != undefined) {
                $($("#" + $(".ui-tabs-active").attr("aria-controls")).find("table")[1]).DataTable().fnAdjustColumnSizing();
            }
        });
        function Clear() {
            if ($("#tblSubjectDetail").children().length > 0) {
                $("#tblSubjectDetail").dataTable().fnDestroy();
                $("#tblSubjectDetail").html("");
            }
            $(<%=HProjectId.ClientID%>).val("");
            $(<%=HSubjectId.ClientID%>).val("");
            $(<%=txtproject.ClientID%>).val("");
            $(<%=txtSubject.ClientID%>).val("");
            $(<%=txtFromDate.ClientID%>).val("");
            $(<%=txtToDate.ClientID%>).val("");
            return false;
        }
        function pageLoad() {
            $("#tabs").tabs({
                activate: function (event, ui) {
                    $(ui.newPanel.find("table")[1]).DataTable().fnAdjustColumnSizing();
                }
            });
        }
        $(document).ready(function () {
            $("#ctl00_CPHLAMBDA_rbFilterType_0").click();
            Display($(".HideShowSubject")[0], 'divSubjectDetail');
            //$(".btnnew").click();
        });
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
        function ClientPopulatedProject(sender, e) {
            ProjectClientShowing('AutoCompleteExtenderProject', $get('<%= txtproject.ClientID%>'));
        }
        function OnSelectedProject(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtproject.ClientID%>'),
            $get('<%= HProjectId.ClientID%>'));
        }

        function ClientPopulated(sender, e) {
            SubjectClientShowing('AutoCompleteExtenderSubject', $get('<%= txtSubject.ClientId %>'));
        }
        function OnSelected(sender, e) {
            SubjectOnItemSelected(e.get_value(), $get('<%= txtSubject.clientid %>'),
            $get('<%= HSubjectId.clientid %>'));
        }
        function ChangeFilterType(FilterType) {
            if (FilterType.indexOf("subject") != -1) {
                $(".clsProjectFilter").addClass("HideConroll");
                $(".clsDateFilter").addClass("HideConroll");
                $(".clsSubjectFilter").removeClass("HideConroll");
            }
            else if (FilterType.indexOf("project") != -1) {
                $(".clsSubjectFilter").addClass("HideConroll");
                $(".clsDateFilter").addClass("HideConroll");
                $(".clsProjectFilter").removeClass("HideConroll");
            }
            else if (FilterType.indexOf("date") != -1) {
                $(".clsProjectFilter").addClass("HideConroll");
                $(".clsSubjectFilter").addClass("HideConroll");
                $(".clsDateFilter").removeClass("HideConroll");
            }
        }
        function createColumn(HeaderText) {
            var Column = new Array();
            var obj = new Object();
            obj.sTitle = "Sr. No.";
            Column.push(obj);
            for (var i = 0; i < HeaderText.length; i++) {
                var obj = new Object();
                obj.sTitle = HeaderText[i].toString();
                if (HeaderText[i].toString().toUpperCase().indexOf("DATE") != -1) {
                    debugger;
                    obj.type = 'date-dd-mmm-yyyy'
                }
                Column.push(obj);
            }
            return Column;
        }
        function CheckFilter(flag) {
            var valueObject = {};
            valueObject['FilterFlag'] = $(<%=rbFilterType.ClientID%>).find("input:checked").siblings().text();
            valueObject['ProjectID'] = $(<%=HProjectId.ClientID%>).val();
            valueObject['SubjectID'] = $(<%=HSubjectId.ClientID%>).val();
            valueObject['FromDate'] = $(<%=txtFromDate.ClientID%>).val();
            valueObject['ToDate'] = $(<%=txtToDate.ClientID%>).val();

            $(<%=hdnFromDate.ClientID%>).val($(<%=txtFromDate.ClientID%>).val());
            $(<%=hdnToDate.ClientID%>).val($(<%=txtToDate.ClientID%>).val());

            if (valueObject['FilterFlag'].toUpperCase().indexOf("PROJECT") != -1 && valueObject['ProjectID'] == "")
            { msgalert('Please Select Project.'); return false; }
            if (valueObject['FilterFlag'].toUpperCase().indexOf("SUBJECT") != -1 && valueObject['SubjectID'] == "")
            { msgalert('Please Select Subject.'); return false; }
            if (valueObject['FilterFlag'].toUpperCase().indexOf("DATE") != -1 && (valueObject['FromDate'] == "" || valueObject['ToDate'] == ""))
            { msgalert('Please Select From Date and To Date.'); return false; }

            var btn = document.getElementById('<%= btnExportToExcelSubject.ClientID()%>')
            var btn2 = document.getElementById('<%= btnExportToExcelAuditTrial.ClientID()%>')
            if (flag == 'SubjectList') { btn.click(); }
            else if (flag == 'AuditTrial') { btn2.click(); }
        }
        function SubjectList() {
            var valueObject = {};
            valueObject['FilterFlag'] = $(<%=rbFilterType.ClientID%>).find("input:checked").siblings().text();
            valueObject['ProjectID'] = $(<%=HProjectId.ClientID%>).val();
            valueObject['SubjectID'] = $(<%=HSubjectId.ClientID%>).val();
            valueObject['FromDate'] = $(<%=txtFromDate.ClientID%>).val();
            valueObject['ToDate'] = $(<%=txtToDate.ClientID%>).val();

            if (valueObject['FilterFlag'].toUpperCase().indexOf("PROJECT") != -1 && valueObject['ProjectID'] == "")
            { msgalert('Please Select Project.'); return false; }
            if (valueObject['FilterFlag'].toUpperCase().indexOf("SUBJECT") != -1 && valueObject['SubjectID'] == "")
            { msgalert('Please Select Subject.'); return false; }
            if (valueObject['FilterFlag'].toUpperCase().indexOf("DATE") != -1 && (valueObject['FromDate'] == "" || valueObject['ToDate'] == ""))
            { msgalert('Please Select From Date and To Date.'); return false; }

            valueObject = { values: valueObject };
            onUpdating('', '');
            $.ajax({
                type: "POST",
                url: "frmSubjectPIFAuditTrial.aspx/GetSubjectList",
                dataType: "json",
                data: JSON.stringify(valueObject),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    onUpdated('', '');
                    var aaDataSet = [];
                    if (data.d != "" && data.d != null) {
                        data = JSON.parse(data.d);

                        for (var Row = 0; Row < data.length; Row++) {
                            var InDataSet = [];
                            InDataSet.push(Row + 1, data[Row].vSubjectID, data[Row].FullName, data[Row].vInitials, data[Row].AuditTrial);
                            aaDataSet.push(InDataSet);
                        }
                        if ($("#tblSubjectDetail").children().length > 0) {
                            $("#tblSubjectDetail").dataTable().fnDestroy();
                            $("#tblSubjectDetail").html("");
                        }
                        Column = createColumn("Subject ID#Subject Name#Initial#Audit Trial".split("#"))
                        oTable = $('#tblSubjectDetail').prepend($('<thead>').append($('#tblSubjectDetail tr:first'))).dataTable({
                            "sScrollY": "200px",
                            "scrollCollapse": true,
                            "bJQueryUI": true,
                            "sPaginationType": "full_numbers",
                            "iDisplayLength": 10,
                            "bSort": false,
                            "bDestroy": true,
                            aLengthMenu: [
                                [10, 25, 50, 100, -1],
                                [10, 25, 50, 100, "All"]
                            ],
                            "aaData": aaDataSet,
                            "aoColumns": Column,
                            "aoColumnDefs": [
                                    { 'bSortable': false, 'aTargets': [0] }
                            ],
                            "oLanguage": {
                                "sEmptyTable": "No Record Found",
                            },
                            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                                $('td:eq(4)', nRow).html("<img src='Images/audit.png' onclick='SubjectAuditTrial(\"" + aData[1] + "\");' />");
                            }
                        });
                        
                        if ($(".HideShowSubject")[0].src.toString().toUpperCase().search("EXPAND") != -1) {
                            $(".HideShowSubject").click();
                            if (oTable.length) {
                                oTable.fnAdjustColumnSizing();
                            }
                        }
                        setTimeout(function () {
                            $(".HideShowFilter").click();
                        },500);
                        
                    }
                },
                failure: function (error) {
                    alert(error);
                }
            });
            return false;
        }
        function SubjectAuditTrial(SubjectID) {
            var valueObject = {};
            valueObject['SubjectID'] = SubjectID;
            $(<%=HSelectedSubjectId.ClientID%>).val(SubjectID);
            
            valueObject = { values: valueObject };
            onUpdating('', '');
            $.ajax({
                type: "POST",
                url: "frmSubjectPIFAuditTrial.aspx/GetSubjectAuditTrial",
                dataType: "json",
                data: JSON.stringify(valueObject),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    onUpdated('', '');
                    var aaDataSet = [];
                    var BlankTableHeader = ["Subject ID", "Modified Field", "Old Value", "New Value", "Operation", "Modified By", "Modified Date", "Remarks"];
                    if (data.d != "" && data.d != null) {
                        data = JSON.parse(data.d);
                        if (data == "LoggedOut") {
                            alert("User logged out.");
                            return;
                        }
                        
                        aaDataSet = [];
                        if (!data["tblPersonalDetailAuditTrial"].length)
                            Column = createColumn(BlankTableHeader)
                        else 
                            Column = createColumn(Object.keys(data["tblPersonalDetailAuditTrial"][0]))
                        
                        for (var Row = 0; Row < data["tblPersonalDetailAuditTrial"].length; Row++) {
                            var InDataSet = [];
                            InDataSet.push(Row + 1,
                                data["tblPersonalDetailAuditTrial"][Row][Column[1]["sTitle"]],
                                data["tblPersonalDetailAuditTrial"][Row][Column[2]["sTitle"]],
                                data["tblPersonalDetailAuditTrial"][Row][Column[3]["sTitle"]],
                                data["tblPersonalDetailAuditTrial"][Row][Column[4]["sTitle"]],
                                data["tblPersonalDetailAuditTrial"][Row][Column[5]["sTitle"]],
                                data["tblPersonalDetailAuditTrial"][Row][Column[6]["sTitle"]],
                                data["tblPersonalDetailAuditTrial"][Row][Column[7]["sTitle"]],
                                data["tblPersonalDetailAuditTrial"][Row][Column[8]["sTitle"]]);
                            aaDataSet.push(InDataSet);
                        }
                        if ($("#tblPersonalDetailAuditTrial").children().length > 0) {
                            $("#tblPersonalDetailAuditTrial").dataTable().fnDestroy();
                            $("#tblPersonalDetailAuditTrial").html("");
                        }
                        oTable = $('#tblPersonalDetailAuditTrial').prepend($('<thead>').append($('#tblPersonalDetailAuditTrial tr:first'))).dataTable({
                            type: "POST",
                            url: "frmSubjectPIFAuditTrial.aspx/GetSubjectAuditTrial",
                            dataType: "json",
                            data: JSON.stringify(valueObject),
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {alert('ok');},
                            "sScrollY": "200px",
                            "scrollCollapse": true,
                            "bJQueryUI": true,
                            "sPaginationType": "full_numbers",
                            "iDisplayLength": 10,
                            "bSort": false,
                            "bDestroy": true,
                            aLengthMenu: [
                                [10, 25, 50, 100, -1],
                                [10, 25, 50, 100, "All"]
                            ],
                            "aaData": aaDataSet,
                            "aoColumns": Column,
                            "aoColumnDefs": [
                                    { 'bSortable': false, 'aTargets': [0] }
                            ],
                            "oLanguage": {
                                "sEmptyTable": "No Audit Trial Found For Personal Detail",
                            }
                        });
                        
                        aaDataSet = [];
                        if (!data["tblPersonalProofDetailAuditTrial"].length)
                            Column = createColumn(BlankTableHeader)
                        else
                            Column = createColumn(Object.keys(data["tblPersonalProofDetailAuditTrial"][0]))

                        for (var Row = 0; Row < data["tblPersonalProofDetailAuditTrial"].length; Row++) {
                            var InDataSet = [];
                            InDataSet.push(Row + 1,
                                data["tblPersonalProofDetailAuditTrial"][Row][Column[1]["sTitle"]],
                                data["tblPersonalProofDetailAuditTrial"][Row][Column[2]["sTitle"]],
                                data["tblPersonalProofDetailAuditTrial"][Row][Column[3]["sTitle"]],
                                data["tblPersonalProofDetailAuditTrial"][Row][Column[4]["sTitle"]],
                                data["tblPersonalProofDetailAuditTrial"][Row][Column[5]["sTitle"]],
                                data["tblPersonalProofDetailAuditTrial"][Row][Column[6]["sTitle"]],
                                data["tblPersonalProofDetailAuditTrial"][Row][Column[7]["sTitle"]],
                                data["tblPersonalProofDetailAuditTrial"][Row][Column[8]["sTitle"]]);
                            aaDataSet.push(InDataSet);
                        }
                        if ($("#tblPersonalProofDetailAuditTrial").children().length > 0) {
                            $("#tblPersonalProofDetailAuditTrial").dataTable().fnDestroy();
                            $("#tblPersonalProofDetailAuditTrial").html("");
                        }
                        oTable = $('#tblPersonalProofDetailAuditTrial').prepend($('<thead>').append($('#tblPersonalProofDetailAuditTrial tr:first'))).dataTable({
                            "sScrollY": "200px",
                            "scrollCollapse": true,
                            "bJQueryUI": true,
                            "sPaginationType": "full_numbers",
                            "iDisplayLength": 10,
                            "bSort": false,
                            "bDestroy": true,
                            aLengthMenu: [
                                [10, 25, 50, 100, -1],
                                [10, 25, 50, 100, "All"]
                            ],
                            "aaData": aaDataSet,
                            "aoColumns": Column,
                            "aoColumnDefs": [
                                    { 'bSortable': false, 'aTargets': [0] }
                            ],
                            "oLanguage": {
                                "sEmptyTable": "No Audit Trial Found For Proof Detail",
                            }
                        });

                        aaDataSet = [];
                        if (!data["tblPersonalHabitDetailAuditTrial"].length)
                            Column = createColumn(BlankTableHeader)
                        else
                            Column = createColumn(Object.keys(data["tblPersonalHabitDetailAuditTrial"][0]))

                        for (var Row = 0; Row < data["tblPersonalHabitDetailAuditTrial"].length; Row++) {
                            var InDataSet = [];
                            InDataSet.push(Row + 1,
                                data["tblPersonalHabitDetailAuditTrial"][Row][Column[1]["sTitle"]],
                                data["tblPersonalHabitDetailAuditTrial"][Row][Column[2]["sTitle"]],
                                data["tblPersonalHabitDetailAuditTrial"][Row][Column[3]["sTitle"]],
                                data["tblPersonalHabitDetailAuditTrial"][Row][Column[4]["sTitle"]],
                                data["tblPersonalHabitDetailAuditTrial"][Row][Column[5]["sTitle"]],
                                data["tblPersonalHabitDetailAuditTrial"][Row][Column[6]["sTitle"]],
                                data["tblPersonalHabitDetailAuditTrial"][Row][Column[7]["sTitle"]],
                                data["tblPersonalHabitDetailAuditTrial"][Row][Column[8]["sTitle"]],
                                data["tblPersonalHabitDetailAuditTrial"][Row][Column[9]["sTitle"]],
                                data["tblPersonalHabitDetailAuditTrial"][Row][Column[10]["sTitle"]],
                                data["tblPersonalHabitDetailAuditTrial"][Row][Column[11]["sTitle"]],
                                data["tblPersonalHabitDetailAuditTrial"][Row][Column[12]["sTitle"]]);
                            aaDataSet.push(InDataSet);
                        }
                        if ($("#tblPersonalHabitDetailAuditTrial").children().length > 0) {
                            $("#tblPersonalHabitDetailAuditTrial").dataTable().fnDestroy();
                            $("#tblPersonalHabitDetailAuditTrial").html("");
                        }
                        oTable = $('#tblPersonalHabitDetailAuditTrial').prepend($('<thead>').append($('#tblPersonalHabitDetailAuditTrial tr:first'))).dataTable({
                            "sScrollY": "200px",
                            "scrollCollapse": true,
                            "bJQueryUI": true,
                            "sPaginationType": "full_numbers",
                            "iDisplayLength": 10,
                            "bSort": false,
                            "bDestroy": true,
                            aLengthMenu: [
                                [10, 25, 50, 100, -1],
                                [10, 25, 50, 100, "All"]
                            ],
                            "aaData": aaDataSet,
                            "aoColumns": Column,
                            "aoColumnDefs": [
                                    { 'bSortable': false, 'aTargets': [0] }
                            ],
                            "oLanguage": {
                                "sEmptyTable": "No Audit Trial Found For Habit Detail",
                            }
                        });

                        aaDataSet = [];
                        if (!data["tblFemaleDetailAuditTrial"].length)
                            Column = createColumn(BlankTableHeader)
                        else
                            Column = createColumn(Object.keys(data["tblFemaleDetailAuditTrial"][0]))

                        for (var Row = 0; Row < data["tblFemaleDetailAuditTrial"].length; Row++) {
                            var InDataSet = [];
                            InDataSet.push(Row + 1,
                                data["tblFemaleDetailAuditTrial"][Row][Column[1]["sTitle"]],
                                data["tblFemaleDetailAuditTrial"][Row][Column[2]["sTitle"]],
                                data["tblFemaleDetailAuditTrial"][Row][Column[3]["sTitle"]],
                                data["tblFemaleDetailAuditTrial"][Row][Column[4]["sTitle"]],
                                data["tblFemaleDetailAuditTrial"][Row][Column[5]["sTitle"]],
                                data["tblFemaleDetailAuditTrial"][Row][Column[6]["sTitle"]],
                                data["tblFemaleDetailAuditTrial"][Row][Column[7]["sTitle"]],
                                data["tblFemaleDetailAuditTrial"][Row][Column[8]["sTitle"]]);
                            aaDataSet.push(InDataSet);
                        }
                        if ($("#tblFemaleDetailAuditTrial").children().length > 0) {
                            $("#tblFemaleDetailAuditTrial").dataTable().fnDestroy();
                            $("#tblFemaleDetailAuditTrial").html("");
                        }
                        oTable = $('#tblFemaleDetailAuditTrial').prepend($('<thead>').append($('#tblFemaleDetailAuditTrial tr:first'))).dataTable({
                            "sScrollY": "200px",
                            "scrollCollapse": true,
                            "bJQueryUI": true,
                            "sPaginationType": "full_numbers",
                            "iDisplayLength": 10,
                            "bSort": false,
                            "bDestroy": true,
                            aLengthMenu: [
                                [10, 25, 50, 100, -1],
                                [10, 25, 50, 100, "All"]
                            ],
                            "aaData": aaDataSet,
                            "aoColumns": Column,
                            "aoColumnDefs": [
                                    { 'bSortable': false, 'aTargets': [0] }
                            ],
                            "oLanguage": {
                                "sEmptyTable": "No Audit Trial Found For Female Detail",
                            }
                        });
                        
                        aaDataSet = [];
                        if (!data["tblContactDetailAuditTrial"].length)
                            Column = createColumn(BlankTableHeader)
                        else
                            Column = createColumn(Object.keys(data["tblContactDetailAuditTrial"][0]))

                        for (var Row = 0; Row < data["tblContactDetailAuditTrial"].length; Row++) {
                            var InDataSet = [];
                            InDataSet.push(Row + 1,
                                data["tblContactDetailAuditTrial"][Row][Column[1]["sTitle"]],
                                data["tblContactDetailAuditTrial"][Row][Column[2]["sTitle"]],
                                data["tblContactDetailAuditTrial"][Row][Column[3]["sTitle"]],
                                data["tblContactDetailAuditTrial"][Row][Column[4]["sTitle"]],
                                data["tblContactDetailAuditTrial"][Row][Column[5]["sTitle"]],
                                data["tblContactDetailAuditTrial"][Row][Column[6]["sTitle"]],
                                data["tblContactDetailAuditTrial"][Row][Column[7]["sTitle"]],
                                data["tblContactDetailAuditTrial"][Row][Column[8]["sTitle"]]);
                            aaDataSet.push(InDataSet);
                        }
                        if ($("#tblContactDetailAuditTrial").children().length > 0) {
                            $("#tblContactDetailAuditTrial").dataTable().fnDestroy();
                            $("#tblContactDetailAuditTrial").html("");
                        }
                        oTable = $('#tblContactDetailAuditTrial').prepend($('<thead>').append($('#tblContactDetailAuditTrial tr:first'))).dataTable({
                            "sScrollY": "200px",
                            "scrollCollapse": true,
                            "bJQueryUI": true,
                            "sPaginationType": "full_numbers",
                            "iDisplayLength": 10,
                            "bSort": false,
                            "bDestroy": true,
                            aLengthMenu: [
                                [10, 25, 50, 100, -1],
                                [10, 25, 50, 100, "All"]
                            ],
                            "aaData": aaDataSet,
                            "aoColumns": Column,
                            "aoColumnDefs": [
                                    { 'bSortable': false, 'aTargets': [0] }
                            ],
                            "oLanguage": {
                                "sEmptyTable": "No Audit Trial Found For Contact Detail",
                            }
                        });
                        $find('MPE_UserGroupMstHistory').show();
                        if ($("#" + $(".ui-tabs-active").attr("aria-controls")).find("table")[1] != undefined) {
                            $($("#" + $(".ui-tabs-active").attr("aria-controls")).find("table")[1]).DataTable().fnAdjustColumnSizing();
                        }
                    }
                },
                failure: function (error) {
                    alert(error);
                }
            });
            return false;

        }
    </script>
</asp:Content>