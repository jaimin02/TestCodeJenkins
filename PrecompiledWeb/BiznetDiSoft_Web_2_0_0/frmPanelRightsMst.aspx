<%--Develop by Ketan Muliya--%>
<%@ page title="" language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmPanelRightsMst, App_Web_pna05jsx" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <link rel="stylesheet" type="text/css" href="App_Themes/jquery.multiselect.css" />
    <link rel="stylesheet" type="text/css" href="App_Themes/multiple-select.css" />
    <link rel="stylesheet" type="text/css" href="App_Themes/jquery.multiselect.css" />
    <link href="App_Themes/StyleBlue/UI_Theme/jquery-ui.css" rel="stylesheet" />
    <style type="text/css">
        .hide_column {
            display: none;
        }

        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

        .table th {
            border: 1px solid black;
            border-collapse: collapse;
        }

        .ui-multiselect {
            border: 1px solid navy;
            /*width: 50% ;*/
            max-width: 100% !important;
            max-height: 35px;
            overflow: auto;
            overflow-x: hidden;
            white-space: nowrap;
        }

        .ui-multiselect-menu {
            max-width: 40% !important;
        }
    </style>
    <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                    <legend class="LegendText" style="color: Black; font-size: 12px; margin-bottom: 2%;">
                        <img id="img2" alt="Panel Display Details" src="images/panelexpand.png"
                            onclick="Display(this,'divOperationDetail');" runat="server" style="margin-right: 2px;" />Panel Display Details</legend>
   
        <div id="divOperationDetail" style="display: none";>
    <table style="width: 100%;" cellpadding="5px">
        <tr>
            <td class="Label" style="text-align: right; width: 26%;">Display Panel Name* :
            </td>
            <td style="text-align: left; width: 30%; float: left;">
                <asp:DropDownList ID="ddlPanelName" runat="server" CssClass="dropDownList" AutoPostBack="false" onchange=" fnPanelName();"></asp:DropDownList>
            </td>
            <td class="Label" style="text-align: left; width: 6%; float: left;">Profile* :
            </td>
            <td style="    text-align: left; width: 37%; float: left;">
                <asp:DropDownList ID="ddlProfile" runat="server" Style="" CssClass="dropDownList" AutoPostBack="false" onchange=" fnProfileName();"></asp:DropDownList>
            </td>
        </tr>

        <tr style="display: none">
            <td class="Label" style="width: 40%; text-align: right;">Department*:
            </td>
            <td style="text-align: left;">
                <asp:DropDownList ID="ddlDepartment" runat="server" Style="width: 40%" CssClass="dropDownList" AutoPostBack="false"></asp:DropDownList>
            </td>
        </tr>

        <tr style="display: none">
            <td class="Label" style="width: 40%; text-align: right;">Location*:
            </td>
            <td style="text-align: left;">
                <asp:DropDownList ID="ddlLocation" runat="server" Style="width: 40%" CssClass="dropDownList" AutoPostBack="false"></asp:DropDownList>
            </td>
        </tr>

        <tr style="display: none">
            <td style="text-align: right;" class="Label">Remarks :
            </td>
            <td style="text-align: left;">
                <asp:TextBox ID="txtremarks" runat="server" CssClass="textBox" TextMode="MultiLine" Width="40%" Height="50px" />
            </td>
        </tr>
        <tr>
            <td style="text-align: center;" colspan="4">
                <asp:Button ID="BtnSave" runat="server" CssClass="btn btnsave" Text="Save" ToolTip="Save" OnClientClick="InsertData_DisplayPanel('ADD'); return false;" />
                <asp:Button ID="BtnCancel" runat="server" CssClass="btn btncancel" Text="Cancel" ToolTip="Cancel" OnClientClick="ClearField(); return false;" />
                <asp:Button ID="BtnExit" runat="server" CssClass="btn btnexit" Text="Exit" ToolTip="Exit"
                    OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" OnClick="BtnExit_Click" />
                <asp:Button ID="btnExportToExcel" runat="server" CssClass="btn btnexcel"  OnClick="btnExportToExcel_Click" />
            </td>
        </tr>
    </table>
            </div>
        </fieldset>
    <fieldset class="FieldSetBox" style="display: block; width: 95.5%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img1" alt="Panel Display Data" src="images/panelcollapse.png"
                                        onclick="Display(this,'divOperationData');" runat="server" style="margin-right: 2px;" />Panel Display Data</legend>
        <div id="divOperationData">
    <div id="divPanelData" style="margin: 2% 12%">
        <table id="tblPanelDisplayData" class="" width="100%" style="background-color: #B0AFAF;"></table>
    </div>
            </div>
        </fieldset>

    <button id="btnaudit" runat="server" style="display: none;" />
    <cc1:ModalPopupExtender ID="MPE_PanelAuditTrial" runat="server" PopupControlID="dvServiceAudiTrail" BehaviorID="MPE_PanelAuditTrial"
        PopupDragHandleControlID="LbldServiceDtl" BackgroundCssClass="modalBackground" CancelControlID="imgServiceAuditTrail"
        TargetControlID="btnaudit">
    </cc1:ModalPopupExtender>
    <div>
        <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpServiceAuditTrail" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div id="dvServiceAudiTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
                                <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                    <tr>
                                        <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 12px !important; width: 97%;">
                                            <asp:Label ID="lblRamge" runat="server" Text="Audit Trail"></asp:Label>
                                        </td>
                                        <td style="width: 3%">
                                            <img id="imgServiceAuditTrail" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
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
                                                        <table id="tblAuditTrail" class="tblAudit" width="100%" style="background-color: aliceblue;"></table>
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

    <div style="display: none; width: 35%; max-height: 200px; text-align: left; margin: auto; left: 30% !important;"
        class="divModalPopup" runat="server" id="divForRemarks">
        <table style="width: 100%; margin-bottom: 2%">
            <tr>
                <td colspan="2" style="font-size: 13px;" class="Label">
                    <img id="imgClose" title="Close Window" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px;"
                        onclick="funCloseDiv('divForRemarks');" />
                    Reason/Remark for Change Status 
                                            <hr />
                </td>
            </tr>
            <tr>
                <td class="Label" style="text-align: right; width: 20%">
                    <span class="Label" style="vertical-align: top;">Remark* : </span>
                </td>
                <td style="text-align: left; width: 78%">
                    <asp:TextBox TextMode="MultiLine" ID="txtReasonRemark" Text="" class="Textbox" Width="88%"
                        runat="server" Height="60px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 10px;"></td>
            </tr>
            <tr>
                <td colspan="2" class="Label" style="text-align: center; width: 100%">
                    <asp:Button ID="btnSaveRemark" runat="server" class="btn btnsave" Text="Save" OnClientClick="InsertData_DisplayPanel('EDIT'); return false;" />
                </td>
            </tr>
        </table>
    </div>
    <div id="ModalBackGround" runat="server" class="divModalBackGround" style="display: none;">
    </div>

    <asp:Button ID="BtnExportToExcel_AuditTrail" runat="server" Text="Export To Excel" OnClick="BtnExportToExcel_AuditTrail_Click" Style="display: none" />
    <asp:HiddenField ID="hdn_npaneldisplyno" runat="server" Value="0" />
    <asp:HiddenField ID="hdnPanelId" runat="server" Value="" />
    <asp:HiddenField ID="hdnUserTypeCode" runat="server" Value="" />

    <%--<script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>--%>

    <script src="Script/jquery-1.4.3.min.js" type="text/javascript"></script>
    
    <script src="Script/jquery-ui-1.10.4.custom.min.js" type="text/javascript"></script>
    <script src="Script/jquery.multiselect.min.js" type="text/javascript"></script>
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>


    <script type="text/javascript">


        function HideSponsorDetails() {
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



        //Gloabal Variable Declare
        var vPanelId;
        var vUserTypeCode;
        var cActiveFlag;
        var vPanelName = "";
        var vRemark = "";
        var userid;
        var strmessage = "";
        var paneldisplaydata;

        $(document).ajaxStart(function () {
            $("#updateProgress").css("display", "block");
            createDiv();
            setLayerPosition();
        });
        $(document).ajaxComplete(function () {
            $("#updateProgress").css("display", "none");
            document.body.removeChild(document.getElementById('shadow'));
        });

        function pageLoad() {
            GetPanelDisplayData();
            MultiselectRequired();
            fnApplyPanelName();
            fnApplyProfileName();
        }

        function GetPanelDisplayData() {

            var TotalActivity;
            var id = '<%= Me.Session(S_UserID)%>';

            try {
                $.ajax({
                    type: "post",
                    url: "frmpanelrightsmst.aspx/GetPanelRightsData",
                    data: '{"ID":"' + id + '"}',
                    contentType: "application/json; charset=utf-8",
                    datatype: JSON,
                    async: false,
                    dataType: "json",
                    success: function (dataset) {
                        var data;
                        data = eval('(' + dataset.d + ')').dt_PanelDisplay
                        paneldisplaydata = data;
                        var FilterDataPanelName = eval('(' + dataset.d + ')').distinctDT

                        $('#tblPanelDisplayData').attr("IsTable", "has");

                        var ddlPanelName = $("[id*=ddlPanelName]");
                        //ddlPanelName.empty().append('<option selected="selected" value="0">--Select Display Name--</option>');
                        for (var Row = 0; Row < FilterDataPanelName.length; Row++) {
                            ddlPanelName.append($("<option></option>").val(FilterDataPanelName[Row].vPanelId).html(FilterDataPanelName[Row].vPanelName));
                        }

                        var ActivityDataset = [];
                        if (data != "" && data != null) {
                            TotalActivity = data.length;
                            for (var Row = 0; Row < data.length; Row++) {
                                var InDataset = [];
                                InDataset.push(data[Row].SrNo, data[Row].vPanelName, data[Row].vUserTypeName, data[Row].vDeptName, data[Row].vLocationName, data[Row].cActiveFlag, data[Row].vPanelId, data[Row].vUserTypeCode, data[Row].nPanelDisplyNo, data[Row].vRemark, " ", " ", data[Row].Audit, " ");
                                ActivityDataset.push(InDataset);
                            }

                            otable = $('#tblPanelDisplayData').dataTable({

                                "bJQueryUI": true,
                                "sPaginationType": "full_numbers",
                                "bLengthChange": true,
                                "iDisplayLength": 10,
                                "bProcessing": true,
                                "bSort": false,
                                "autoWidth": true,
                                "aaData": ActivityDataset,
                                "bInfo": true,
                                "bDestroy": true,
                                "bScrollCollapse": true,
                                "sScrollY": "285px",
                                aLengthMenu: [
                                   [10, 25, 50, -1],
                                   [10, 25, 50, "All"]
                                ],

                                "fnCreatedRow": function (nRow, aData, iDataIndex) {
                                    if (aData[5] == 'Y') {
                                        $('td:eq(4)', nRow).append("Active");
                                        $('td:eq(5)', nRow).append("<a href='#'  OnClick='ChangeStatus(this); return false;' vpanelid='" + aData[6] + "' , vusertypecode = '" + aData[7] + "' , npaneldisplyno='" + aData[8] + "', cactiveflag ='N'>InActive </a>");
                                    }
                                    else {
                                        $('td:eq(4)', nRow).append("InActive");
                                        $('td:eq(5)', nRow).append("<a href='#'  OnClick='ChangeStatus(this); return false;' vpanelid='" + aData[6] + "' , vusertypecode = '" + aData[7] + "' , npaneldisplyno='" + aData[8] + "' , cactiveflag ='Y'>Active </a>");
                                    }

                                    $('td:eq(6)', nRow).append("<input type='image' id='imgAudit_" + iDataIndex + "' name='imgAudit$" + iDataIndex + "' src='images/audit.png' OnClick='AuditTrial(this); return false;' style='border-width:0px;'   npaneldisplyno='" + aData[8] + "' >");
                                    $('td:eq(7)', nRow).append("<input type='image' id='imgExport_" + iDataIndex + "' name='imgExport$" + iDataIndex + "' src='images/Export.gif' OnClick='Export_AuditTrial(this); return false;' style='border-width:0px;'   npaneldisplyno='" + aData[8] + "' >");
                                },

                                "aoColumns": [
                                            { "sTitle": "#" },
                                            { "sTitle": "Panel Name" },
                                            { "sTitle": "Profile" },
                                            { "sTitle": "Department" },
                                            { "sTitle": "Location" },
                                            { "sTitle": "Status" },
                                            { "sTitle": "panelId" },
                                            { "sTitle": "USerTypeCode" },
                                            { "sTitle": "PanelDisplayNo" },
                                            { "sTitle": "Remark" },

                                            { "sTitle": "Current Status" },
                                            { "sTitle": "Change Status To" },
                                            { "sTitle": "Audit Trail" },
                                            { "sTitle": "Export To Excel" },

                                ],

                                "aoColumnDefs": [{ "bVisible": false, "aTargets": [3, 4, 5, 6, 7, 8] }],
                                "oLanguage": {
                                    "sEmptyTable": "No Record Found",
                                },

                            });
                        }
                        return false;
                    },
                    failure: function (response) {
                        msgalert(response.d);
                    },
                    error: function (response) {
                        msgalert(response.d);
                    }
                });
                otable.fnAdjustColumnSizing();
                return false;
            }
            catch (err) {
                msgalert('GetPanelDisplayData function : ' + err.message);
                return false;

            }
        }

        function ChangeStatus(e) {
            vPanelId = e.attributes.vpanelid.value;
            vUserTypeCode = e.attributes.vusertypecode.value;
            cActiveFlag = e.attributes.cactiveflag.value;
            $('#ctl00_CPHLAMBDA_txtReasonRemark').val('');
            displayBackGround();
        }

        function displayBackGround() {
            document.getElementById('<%=divForRemarks.ClientId %>').style.display = '';
            document.getElementById('<%=ModalBackGround.ClientId %>').style.display = '';
            document.getElementById('<%=ModalBackGround.ClientId %>').style.height = ($(document).height()) + "px";
            document.getElementById('<%=ModalBackGround.ClientId %>').style.width = ($(document).width()) + "px";
        }
        function funCloseDiv(div) {
            document.getElementById('<%=divForRemarks.ClientID %>').style.display = 'none';
            document.getElementById('<%=ModalBackGround.ClientId %>').style.display = 'none';
        }

        function InsertData_DisplayPanel(OpMode) {

            var data = paneldisplaydata;
            var panelid = $("#<%= hdnPanelId.ClientID%>").val();
            var arrayPanelID = panelid.split(',');
            strmessage = "";

            var ProfileId = $("#<%= hdnUserTypeCode.ClientID%>").val();
            var arrayProfileID = ProfileId.split(',');

            if (OpMode == "ADD") {
                if ($("#<%= hdnPanelId.ClientID%>").val() == "") {
                    msgalert('Please Select Display Panel Name !');
                    return false;
                }
                if ($("#<%= hdnUserTypeCode.ClientID%>").val() == "") {
                    msgalert('Please Select Profile !');
                    return false;
                }
                for (var panelval = 0; panelval < arrayPanelID.length; panelval++) {
                    for (var profileval = 0; profileval < arrayProfileID.length; profileval++) {
                        vPanelId = arrayPanelID[panelval].toString();
                        vUserTypeCode = arrayProfileID[profileval].toString();
                        InsertData(OpMode);
                    }
                }
                msgalert('Data Added Successfully !');

            }
            else {
                InsertData(OpMode);
                msgalert('Data Update Successfully !');
            }
            if (strmessage != "") {
                msgalert("Already Exist : " + strmessage.substring(0, strmessage.length - 1));
            }
            ClearField();
            return true;
        }

        function InsertData(OpMode) {
            userid = '<%= Me.Session(S_UserID)%>';
            var data = paneldisplaydata;
            try {

                if (OpMode == "ADD") {
                    vRemark = $('#ctl00_CPHLAMBDA_txtremarks').val();
                    OpMode = 1;
                    cActiveFlag = 'Y';

                    if (vPanelId == "0" || vPanelId.trim() == "") {
                        msgalert("Please Select Display Name !");
                        return false;
                    }
                    if (vUserTypeCode == "0" || vUserTypeCode.trim() == "0") {
                        msgalert("Please Select Profile !");
                        return false;
                    }

                    for (var Row = 0; Row < data.length; Row++) {
                        if (data[Row].vPanelId == vPanelId && data[Row].vUserTypeCode == vUserTypeCode) {
                            strmessage += "\n" + data[Row].vPanelName + " : " + data[Row].vUserTypeName + ","
                            return false;
                        }
                    }

                }
                else if (OpMode == "EDIT") {
                    if ($('#ctl00_CPHLAMBDA_txtReasonRemark').val().trim() == "") {
                        msgalert("Please Enter Remark !");
                        return false;
                    }
                    vRemark = $('#ctl00_CPHLAMBDA_txtReasonRemark').val();
                    OpMode = 2;
                }

                $.ajax({
                    type: "POST",
                    url: "frmpanelrightsmst.aspx/InsertData",
                    data: '{"userid":"' + userid + '", "vPanelId":"' + vPanelId + '", "vPanelName":"' + vPanelName + '" , "vUserTypeCode":"' + vUserTypeCode + '", "vRemark":"' + vRemark + '" , "cActiveFlag": "' + cActiveFlag + '", "OpMode":"' + OpMode + '" }',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        funCloseDiv('divForRemarks');
                        
                        $('#ctl00_CPHLAMBDA_txtReasonRemark').val('');
                        $('#ctl00_CPHLAMBDA_txtremarks').val('');
                        $('#ctl00_CPHLAMBDA_ddlPanelName').val("0");
                        $('#ctl00_CPHLAMBDA_ddlProfile').val('0');
                        GetPanelDisplayData();
                        return true;
                    }
                });
                return false;
            }
            catch (err) {
                msgalert('InsertData_DisplayPanel function : ' + err.message);
                return false;

            }
        }

        function AuditTrial(e) {
            npaneldisplyno = e.attributes.npaneldisplyno.value;
            try {
                if (npaneldisplyno != "") {
                    $.ajax({
                        type: "post",
                        url: "frmpanelrightsmst.aspx/AuditTrail",
                        data: '{"npaneldisplyno":"' + npaneldisplyno + '"}',
                        contentType: "application/json; charset=utf-8",
                        datatype: JSON,
                        async: false,
                        success: function (data) {
                            $('#tblAuditTrail').attr("IsTable", "has");
                            var aaDataSet = [];
                            var range = null;

                            if (data.d != "" && data.d != null) {
                                data = JSON.parse(data.d);
                                for (var Row = 0; Row < data.length; Row++) {
                                    var InDataSet = [];
                                    InDataSet.push(data[Row].SrNo, data[Row].vPanelName, data[Row].vUserTypeName, data[Row].cActiveFlag, data[Row].vRemark, data[Row].vModifyBy, data[Row].dModifyOn);
                                    aaDataSet.push(InDataSet);
                                }

                            }
                            if ($("#tblAuditTrail").children().length > 0) {
                                $("#tblAuditTrail").dataTable().fnDestroy();
                            }
                            oTable = $('#tblAuditTrail').prepend($('<thead>').append($('#tblAuditTrail tr:first'))).dataTable({
                                "bJQueryUI": true,
                                "sPaginationType": "full_numbers",
                                "bLengthChange": true,
                                "iDisplayLength": 10,
                                "bProcessing": true,
                                "bSort": false,

                                "aaData": aaDataSet,
                                "aoColumns": [
                                    {
                                        "sTitle": "#",
                                    },
                                    { "sTitle": "Panel Name" },
                                    { "sTitle": "Profile" },
                                    { "sTitle": "Status" },
                                    { "sTitle": "Remarks" },
                                    { "sTitle": "Modify By" },
                                    { "sTitle": "Modify On" },

                                ],
                                "aoColumnDefs": [
                                            { 'bSortable': false, 'aTargets': [0] }
                                ],
                                "oLanguage": {
                                    "sEmptyTable": "No Record Found",
                                }

                            });
                            oTable.fnAdjustColumnSizing();
                            $('.DataTables_sort_wrapper').click;
                            $find('MPE_PanelAuditTrial').show();

                        },
                        failure: function (response) {
                            msgalert(response.d);
                        },
                        error: function (response) {
                            msgalert(response.d);
                        }
                    });
                }
                return false;
            }

            catch (err) {
                msgalert('AuditTrial function : ' + err.message);
                return false;
            }
        }

        function Export_AuditTrial(e) {
            npaneldisplyno = e.attributes.npaneldisplyno.value;
            $('#ctl00_CPHLAMBDA_hdn_npaneldisplyno').val(npaneldisplyno)
            if ($('#ctl00_CPHLAMBDA_hdn_npaneldisplyno').val() != "0" || $('#ctl00_CPHLAMBDA_hdn_npaneldisplyno').val() != "") {
                $("#ctl00_CPHLAMBDA_BtnExportToExcel_AuditTrail").click();
            }

        }
        function ClearField() {
            $('#ctl00_CPHLAMBDA_txtReasonRemark').val('');
            $('#ctl00_CPHLAMBDA_txtremarks').val('');
            $('#ctl00_CPHLAMBDA_ddlPanelName').val("0");
            $('#ctl00_CPHLAMBDA_ddlProfile').val('0');
            $('#ctl00_CPHLAMBDA_hdnPanelId').val('');
            $('#ctl00_CPHLAMBDA_hdnUserTypeCode').val('');
            $('.ui-multiselect-none').click();
            return false;
        }

        function MultiselectRequired() {
            $('#ctl00_CPHLAMBDA_ddlPanelName').multiselect({
                includeSelectAllOption: true
            });
            $('#ctl00_CPHLAMBDA_ddlProfile').multiselect({
                includeSelectAllOption: true
            });
        }

        //For Display Panel Name
        function fnPanelName() {
            var PanelName = [];

            document.getElementById('<%= hdnPanelId.ClientID%>').value = "";
            for (i = 0; i < $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlPanelName']:checked").length ; i++) {
                PanelName.push("" + $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlPanelName']:checked").eq(i).attr("value") + "");
            }
            document.getElementById('<%= hdnPanelId.ClientID%>').value = PanelName;
            return true;
        }

        var PanelName = [];
        function fnApplyPanelName() {
            $("#<%= ddlPanelName.ClientID%>").multiselect({
                noneSelectedText: "--Select Panel Name--",
                click: function (event, ui) {
                    if (ui.checked == true)
                        PanelName.push("'" + ui.value + "'");
                    else if (ui.checked == false) {
                        if ($.inArray("'" + ui.value + "'", PanelName) >= 0)
                            PanelName.splice(PanelName.indexOf("'" + ui.value + "'"), 1)
                    }
                    if ($("input[name$='ddlPanelName']").length > 0) {
                        //clearControls();
                    }
                },
                checkAll: function (event, ui) {
                    PanelName = [];
                    for (var i = 0; i < event.target.options.length; i++) {
                        PanelName.push("'" + $(event.target.options[i]).val() + "'")
                    }

                },
                uncheckAll: function (event, ui) {
                    PanelName = [];

                }
            });

            $("#<%= ddlPanelName.ClientID%>").multiselect("refresh");
            var CheckedCheckBox = document.getElementById('<%= hdnPanelId.ClientID%>').value
            if (CheckedCheckBox != "") {

                CheckedCheckBox = CheckedCheckBox.split(',');
                for (i = 0; i <= CheckedCheckBox.length - 1; i++) {
                    $("#<%= ddlPanelName.ClientID%>").multiselect("widget").find(".ui-multiselect-checkboxes.ui-helper-reset").find("input[value=" + CheckedCheckBox[i] + "]").attr("checked", "checked");

                }
                $('#<%= ddlPanelName.ClientID%>').multiselect("update");
            }
        }

        //For Profile
        function fnProfileName() {

            var ProfileName = [];
            document.getElementById('<%= hdnUserTypeCode.ClientID%>').value = "";
            for (i = 0; i < $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlProfile']:checked").length ; i++) {
                ProfileName.push("" + $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlProfile']:checked").eq(i).attr("value") + "");
            }
            document.getElementById('<%= hdnUserTypeCode.ClientID%>').value = ProfileName;
            return true;
        }


        var ProfileName = [];
        function fnApplyProfileName() {
            $("#<%= ddlProfile.ClientID%>").multiselect({
                noneSelectedText: "--Select Profile Name--",
                click: function (event, ui) {
                    if (ui.checked == true)
                        ProfileName.push("'" + ui.value + "'");
                    else if (ui.checked == false) {
                        if ($.inArray("'" + ui.value + "'", ProfileName) >= 0)
                            ProfileName.splice(ProfileName.indexOf("'" + ui.value + "'"), 1)
                    }
                    if ($("input[name$='ddlProfile']").length > 0) {
                        //clearControls();
                    }
                },
                checkAll: function (event, ui) {
                    ProfileName = [];
                    for (var i = 0; i < event.target.options.length; i++) {
                        ProfileName.push("'" + $(event.target.options[i]).val() + "'")
                    }

                },
                uncheckAll: function (event, ui) {
                    ProfileName = [];

                }
            });

            $("#<%= ddlProfile.ClientID%>").multiselect("refresh");
            var CheckedCheckBox = document.getElementById('<%= hdnUserTypeCode.ClientID%>').value
            if (CheckedCheckBox != "") {

                CheckedCheckBox = CheckedCheckBox.split(',');
                for (i = 0; i <= CheckedCheckBox.length - 1; i++) {
                    $("#<%= ddlProfile.ClientID%>").multiselect("widget").find(".ui-multiselect-checkboxes.ui-helper-reset").find("input[value=" + CheckedCheckBox[i] + "]").attr("checked", "checked");

                }
                $('#<%= ddlProfile.ClientID%>').multiselect("update");
            }
        }


    </script>

</asp:Content>

