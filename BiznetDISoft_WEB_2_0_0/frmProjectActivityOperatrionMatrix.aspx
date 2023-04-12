<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="frmProjectActivityOperatrionMatrix.aspx.vb" Inherits="frmProjectActivityOperatrionMatrix" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <style>
        .checkDiv .checkboxlist {
            font-name: Verdana;
            font-size: smaller;
            color: Black;
            height: 37px;
            width: 100%;
        }

        .checkDiv {
            border-right: gray thin solid;
            border-top: gray thin solid;
            overflow-y: scroll;
            border-left: gray thin solid;
            border-bottom: gray thin solid;
            height: 180px;
            width: 100%;
            min-width: 100%;
            max-width: 100%;
        }

        ul {
            list-style: none;
        }

        #ctl00_CPHLAMBDA_tvUserType ul {
            padding-left: 0Px !important;
        }

        #ctl00_CPHLAMBDA_tvActivity ul {
            padding-left: 0Px !important;
        }

        .FieldSetBox {
            border: #aaaaaa 1px solid;
            z-index: 0px;
            border-radius: 4px;
        }

        .Parent {
            font-weight: bold;
            color: white;
            background-color: rgba(160, 160, 6, 0.44) !important;
        }

        .Child {
            font-weight: bold;
            color: white;
            background-color: #CCC !important;
        }

        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
        }
    </style>

    <script type="text/javascript" src="Script/General.js" language="javascript"></script>
    <script type="text/javascript" src="script/AutoComplete.js"></script>
    <script type="text/javascript" src="Script/jquery-1.4.3.min.js"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.plugins.js"></script>

    <asp:HiddenField ID="hdnsession" runat="server"></asp:HiddenField>
    <table style="width: 95%; margin-left: 3%;">
        <tr>
            <td>
                <asp:UpdatePanel ID="upOperationDetail" runat="server">
                    <ContentTemplate>
                        <fieldset class="FieldSetBox">
                            <legend class="LegendText" style="color: Black; font-size: 12px; text-align: left;">
                                <img id="img2" alt="Project Activity Operation Matrix" src="images/panelcollapse.png" onclick="Display(this,'divProjectActivityOperationMatrix');" runat="server" style="margin-right: 2px;" />
                                Project Activity Operation Matrix
                            </legend>
                            <div id="divProjectActivityOperationMatrix" style="/*border: 1px solid green; overflow: scroll; width: 100% */">
                                <table style="width: 100%" cellpadding="5px">
                                    <tr>
                                        <td class="LabelText" nowrap="nowrap" style="text-align: right; width: 25%">Select Project* :
                                        </td>
                                        <td class="Label" colspan="4" style="text-align: left">
                                            <asp:Label ID="lblProject" runat="server" Text=""></asp:Label>
                                            <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="89%" TabIndex="1"></asp:TextBox>
                                            <asp:Button Style="display: none" ID="btnSetProject" runat="server" txt="data" />
                                            <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                                CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                                OnClientShowing="ClientPopulated" ServiceMethod="GetProjectCompletionListWithOutSponser"
                                                ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True"
                                                CompletionListElementID="pnlProjectList">
                                            </cc1:AutoCompleteExtender>

                                            <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 300px; overflow: auto; overflow-x: hidden;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="LabelText" nowrap="nowrap" style="text-align: right;">Period*:
                                        </td>
                                        <td class="Label" nowrap="nowrap" style="text-align: left">
                                            <asp:DropDownList ID="ddlPeriods" runat="server" CssClass="dropDownList" Width="20%" Style="width: 200px" TabIndex="2" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                    </tr>

                                    <tr>
                                        <td id="tdHRUpper" runat="server" colspan="4" style="display: none">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td valign="top" style="align: right">
                                            <fieldset id="divUserType" runat="server" class="FieldSetBox" style="overflow: auto; display: none; max-height: 230px; height: 230px;"
                                                tabindex="5">
                                                <asp:TreeView ID="tvUserType" Width="100px" Height="250px" ShowCheckBoxes="All" BorderColor="DarkGreen"
                                                    Font-Bold="True" Font-Size="X-Small" runat="server">
                                                </asp:TreeView>
                                                <asp:HiddenField ID="UserTypeCount" runat="server" />
                                            </fieldset>
                                        </td>
                                        <td colspan="3" valign="top">
                                            <fieldset id="divActivity" runat="server" class="FieldSetBox" style="overflow: auto; display: none; max-height: 230px;"
                                                tabindex="6">
                                                <asp:TreeView ID="tvActivity" runat="server" BorderColor="DarkGreen" Font-Bold="True"
                                                    Font-Size="X-Small" Height="250px" ShowCheckBoxes="All" Width="100px">
                                                </asp:TreeView>
                                                <asp:HiddenField ID="ActivityCount" runat="server" />
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="tdHRLower" runat="server" colspan="4" style="display: none">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center;" class="Label" colspan="100%">
                                            <asp:Button ID="BtnSave" OnClientClick="return ValidationForSave();" runat="server"
                                                CssClass="btn btnsave" Text="Save" ToolTip="Save" />
                                            <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" Text="Cancel" ToolTip="Cancel" OnClientClick="return clear();"/>
                                            <asp:Button ID="BtnExit" runat="server" CssClass="btn btnexit" Text="Exit" ToolTip="Exit"
                                                OnClientClick="return msgconfirmalert('Are You Sure You Want To Exit?',this);" />
                                            <asp:Button ID="btnAuditTrail" runat="server" CssClass="btn btnaudit" ToolTip="Audit Trail" 
                                                    OnClientClick="return AuditTrail();" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>

    <table style="width: 95%; margin-left: 3%;">
        <tr>
            <td>
                <asp:UpdatePanel ID="upMatrixDetail" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <fieldset class="FieldSetBox">
                            <legend class="LegendText" style="color: Black; font-size: 12px; text-align: left;">
                                <img id="img1" alt="Project Activity Operation Matrix" src="images/panelcollapse.png" onclick="Display(this,'divProjectActivityOperationMatrixDetails');" runat="server" style="margin-right: 2px;" />
                                Project Activity Operation Matrix Details
                            </legend>
                            <div id="legend" style="float: right; border: inset; width: 13%;">
                                Legends
                                <img id="imgShow" runat="server" src="images/question.gif" enableviewstate="false" onmouseover="$('#ctl00_CPHLAMBDA_canal').toggle('medium');" onmouseout="$('#ctl00_CPHLAMBDA_canal').toggle('medium');" />
                                <div>
                                    <fieldset style="display: none; width: 85%; font-size: 7pt; height: auto; text-align: left; margin-left: 3%;"
                                        id="canal" runat="server" class="FieldSetBox">
                                        <div>
                                            <span>
                                                <canvas id="Canvas1" width="12" style="height: 12px !important; width:12px !important;" class="Parent"></canvas>
                                            </span>
                                            <span class="">Activity</span>
                                            <span>
                                                <canvas id="Canvas2" width="12" style="height: 12px !important; width:12px !important;" class="Child"></canvas>
                                            </span>
                                            <span class="">Sub Activity</span>
                                        </div>
                                    </fieldset>
                                </div>
                            </div>
                            <div id="legendLine">
                                <%--<br />
                                <br />
                                <br />
                                <hr />--%>
                            </div>

                            <div id="divProjectActivityOperationMatrixDetails">
                                <table id="tblProjectActivityOperationMatrix">
                                </table>
                            </div>
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>

    <div>
        <table>
            <tr>
                <td>
                    <asp:UpdatePanel runat="server" ID="upAuditTrail" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="dvProjectActivityOperationMatrixAuditTrail" class="centerModalPopup" runat="server" style="display: none; overflow: auto; left: 3.5% !important; width: 93% !important;">
                                <table>
                                    <tr>
                                        <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 97%;">
                                            <asp:Label ID="lblRamge" runat="server" Text="Audit Trail"></asp:Label>
                                        </td>
                                        <td style="width: 3%">
                                            <img id="imgProjectActivityOperationMatrixAuditTrail" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" title="Close" />
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
                                                         <div id="divProjectActivityOperationMatrixAuditTrailDetails" style=" overflow-y: auto; max-height: 390px;">
                                                             <table id="tblProjectActivityOperationMatrixAuditTrail" class="tblAudit" width="100%" style="background-color: aliceblue;"></table>
                                                         </div>                                                        
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

    <div>
        <table>
            <tr>
                <td>

                    <div id="dvRemarks" class="divModalPopup" runat="server" style="background-color: #FFFFFF !important; display: none; overflow: auto; left: 35% !important; width: 31% !important; top: 35% !important;">
                        <table>
                            <tr>
                                <td id="Td1" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 97%;">
                                    <asp:Label ID="Label1" runat="server" Text="Remark"></asp:Label>
                                </td>
                                <td style="width: 3%">
                                    <img id="imgRemark" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" title="Close" onclick="funCloseDiv();" />
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
                                            <td class="Label" style="text-align: right; width: 5%;">Remark* :                                                        
                                            </td>
                                            <td nowrap="nowrap" style="text-align: left; width: 25%;">
                                                <asp:TextBox ID="txtRemark" runat="server" CssClass="textBox" TextMode="MultiLine" Height="106%" Width="60%" MaxLength="500" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Button ID="btnSaveRemark" OnClientClick="ValidationForRemark(this); return false;" runat="server"
                                                    CssClass="btn btnsave" Text="Save" ToolTip="Save" />
                                                <button id="btnClearRemark" onclick="return ClearRemark();" class="btn btncancel" title="Clear">Clear</button>
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
    </div>

    <div id="ModalBackGround" runat="server" class="divModalBackGround" style="display: none;">
    </div>

    <button id="btnAudit" runat="server" style="display: none;" />
    <cc1:ModalPopupExtender ID="mpeProjectActivityOperationMatrixAuditTrail" runat="server" PopupControlID="dvProjectActivityOperationMatrixAuditTrail" BehaviorID="mpeProjectActivityOperationMatrixAuditTrail"
        BackgroundCssClass="modalBackground" CancelControlID="imgProjectActivityOperationMatrixAuditTrail" TargetControlID="btnAudit">
    </cc1:ModalPopupExtender>

    <script type="text/javascript" language="javascript">

        var delRecordVal;

        $(function () {
            $("#legend").hide()
            $("#legendLine").hide()
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

        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        function legendUI() {
            $("#legend").hide()
            $("#legendLine").hide()
        }

        function projectActivityOperationDetails() {
            var vWorkSpaceId = $("#ctl00_CPHLAMBDA_HProjectId").val()
            var iPeriod
            if ($("#ctl00_CPHLAMBDA_ddlPeriods").val() == "ALL") {
                iPeriod = 1;
            }
            else {
                iPeriod = $("#ctl00_CPHLAMBDA_ddlPeriods").val()
            }
            var ajaxData = {
                type: "POST",
                url: "frmProjectActivityOperatrionMatrix.aspx/getprojectActivityOperationDetails",
                data: '{"vWorkSpaceId":"' + vWorkSpaceId + '","iPeriod":"' + iPeriod + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                success: successProjectActivityOperationDetails,
                error: errorProjectActivityOperationDetails
            }
            getProjectActivityOperationDetails(ajaxData.type, ajaxData.url, ajaxData.data, ajaxData.contentType, ajaxData.datatype, ajaxData.async, ajaxData.success, ajaxData.error)
        }

        var getProjectActivityOperationDetails = function (type, url, data, contentType, datatype, async, success, error) {
            $.ajax({
                type: type,
                url: url,
                data: data,
                contentType: contentType,
                datatype: datatype,
                async: async,
                success: success,
                error: error
            });
        }

        function successProjectActivityOperationDetails(jsonData) {
            var data = JSON.parse(jsonData.d);
            debugger;
            if (data.Table == "") {
                $("#legend").hide()
                $("#legendLine").hide()
                $("#divProjectActivityOperationMatrixDetails").hide();
                msgalert("No Data Available for Project Activity Operation Matrix")
            }
            else {
                $("#legend").show()
                $("#legendLine").show()
                $("#divProjectActivityOperationMatrixDetails").show();
                var activityDataSet = [];
                for (var v = 0, l = data.Table.length; v < l; v++) {
                    var inDataSet = [];
                    inDataSet.push(data.Table[v].vProjectActivityOperationId, data.Table[v].vUserTypeName, data.Table[v].vParentNodeDisplayName, data.Table[v].vNodeDisplayName, data.Table[v].iModifyBy, data.Table[v].dModifyOn, data.Table[v].iParentNodeId, '');
                    activityDataSet.push(inDataSet);
                }
                otable = $('#tblProjectActivityOperationMatrix').dataTable({
                    "bJQueryUI": true,
                    "sPaginationType": "full_numbers",
                    "bLengthChange": true,
                    "iDisplayLength": 10,
                    "bProcessing": true,
                    "bSort": true,
                    "autoWidth": false,
                    "aaData": activityDataSet,
                    "bInfo": true,
                    "bDestroy": true,
                    "sScrollX": "100%",
                    //"sScrollY": "200px",
                    //"scrollCollapse": true,
                    //"paging":         false,
                    "sScrollXInner": "1250", /* It varies dynamically if number of columns increases */
                    "fnCreatedRow": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                        $('td:eq(5)', nRow).append('<a title="Delete" attrid="' + aData[0] + '" " Onclick=DeleteProjectActivityOperationMatrix(this) id="' + aData[0] + '" style="cursor:pointer;"><img src="Images/i_delete.gif" alt="Delete"></a>');
                        if (aData[6] == 1) {
                            $(nRow).addClass('Parent');
                        }
                        else {
                            $(nRow).addClass('Child');
                        }
                    },
                    "aoColumns": [
                        { "sTitle": "ProjectActivityOperationId" },
                        { "sTitle": "User Profiles" },
                        { "sTitle": "Parent Activity" },
                        { "sTitle": "Activity" },
                        { "sTitle": "Modify By" },
                        { "sTitle": "Modify On" },
                        { "sTitle": "iParentNodeId" },
                        { "sTitle": "Delete" },

                    ],
                    "aoColumnDefs": [
                         { "bVisible": false, "aTargets": [0, 6] },
                    ],
                    "oLanguage": {
                        "sEmptyTable": "No Record Found",
                    },
                });
            }
        }

        function errorProjectActivityOperationDetails(e) {
            msgalert("ERROR : " + e)
        }

        function AuditTrailProjectActivityOperationMatrix(e) {
            id = $(e).attr("id");
            msgalert(id)
        }

        function DeleteProjectActivityOperationMatrix(e) {
            $("#ctl00_CPHLAMBDA_txtRemark").val("");
            delRecordVal = $(e).attr("id");
            displayBackGround();
        }

        $("[id*=tvUserType] input[type=checkbox]").live("click", function () {
            var table = $(this).closest("table");
            var Flag = false;
            var Index = 0;
            var IndexNot = 0;
            if (table.next().length > 0 && table.next()[0].tagName == "DIV") {
                //Is Parent CheckBox
                var childDiv = table.next();
                var isChecked = $(this).is(":checked");
                $("input[type=checkbox]", childDiv).each(function () {
                    if (isChecked) {
                        $(this).attr("checked", "checked");
                    } else {
                        $(this).removeAttr("checked");
                    }
                });
            }
            else {
                //Is Child CheckBox
                var parentDIV = $(this).closest("DIV");
                if ($("input[type=checkbox]", parentDIV).length == $("input[type=checkbox]:checked", parentDIV).length) {
                    $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");
                }
                else {
                    $("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
                }
            }
        });

        $("[id*=tvActivity] input[type=checkbox]").live("click", function () {
            var table = $(this).closest("table");
            var Flag = false;
            var Index = 0;
            var IndexNot = 0;
            if (table.next().length > 0 && table.next()[0].tagName == "DIV") {
                //Is Parent CheckBox
                var childDiv = table.next();
                var isChecked = $(this).is(":checked");
                $("input[type=checkbox]", childDiv).each(function () {
                    if (isChecked) {
                        $(this).attr("checked", "checked");
                    } else {
                        //$(this).removeAttr("checked");
                    }
                });
            } else {
                //Is Child CheckBox
                var parentDIV = $(this).closest("DIV");
                if ($("input[type=checkbox]", parentDIV).length == $("input[type=checkbox]:checked", parentDIV).length) {
                    $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");
                }
                else {
                    // $("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
                }
            }
        })

        function ValidationForSave() {
            var txtProject = document.getElementById('<%=txtProject.ClientID%>');
            var ddlPeriods = document.getElementById('<%=ddlPeriods.ClientID%>');
            var chks;
            var i;
            var result = false;
            if (txtProject.value == "") {
                msgalert('Please Select Project No!');
                return false;
            }
            if (ddlPeriods.value == "" || ddlPeriods.value == null || ddlPeriods.value.match("SELECT")) {
                msgalert('Please Select Period!');
                return false;
            }
            if ($("#ctl00_CPHLAMBDA_tvUserType [type=checkbox]:checked").length == 0) {
                msgalert("Please select User Type!");
                return false;
            }
            if ($("#ctl00_CPHLAMBDA_tvActivity [type=checkbox]:checked").length == 0) {
                msgalert("Please select Activity!");
                return false;
            }
            return true;
        }

        function ValidationForRemark(e) {
            var txtRemark = document.getElementById('<%=txtRemark.ClientID%>');
            if (txtRemark.value == "") {
                msgalert('Please Enter Remark!');
                return false;
            }

            msgConfirmDeleteAlert(null, "Do You Really Want to Delete This Record ?", function (isConfirmed) {
                if (isConfirmed) {
                    //__doPostBack(e.name, '');
                    id = delRecordVal;
                    $.ajax({
                        type: "POST",
                        url: "frmProjectActivityOperatrionMatrix.aspx/manageProjectActivityOperationDetails",
                        data: '{"mode":2,"vProjectActivityOperationId":"' + id + '","session":"' + $("#ctl00_CPHLAMBDA_hdnsession").val() + '","vRemark":"' + $("#ctl00_CPHLAMBDA_txtRemark").val() + '"}',
                        contentType: "application/json; charset=utf-8",
                        datatype: JSON,
                        async: false,
                        success: function (data) {
                            if (data.d.match("SUCCESS")) {
                                msgalert("Record Deleted Successfully !")
                                funCloseDiv();
                            }
                            else {
                                msgalert("Error While Deleting Record !")
                            }
                        },
                        error: function (e) {
                            msgalert("Error While Deleting Record !")
                        },
                    });
                    projectActivityOperationDetails();
                };
            });
        }

        function ClearRemark() {
            document.getElementById('<%=txtRemark.ClientID%>').value = "";
            return false;
        }

        function AuditTrail() {
            var txtProject = document.getElementById('<%=txtproject.ClientID%>');
            if (txtProject.value == "") {
                msgalert('Please Select Project No!');
                return false;
            }

            var vWorkSpaceId = $("#ctl00_CPHLAMBDA_HProjectId").val()
            var iPeriod
            if ($("#ctl00_CPHLAMBDA_ddlPeriods").val() == "ALL") {
                iPeriod = 1;
            }
            else {
                iPeriod = $("#ctl00_CPHLAMBDA_ddlPeriods").val()
            }

            var ajaxData = {
                type: "POST",
                url: "frmProjectActivityOperatrionMatrix.aspx/AuditTrail",
                data: '{"vWorkSpaceId":"' + vWorkSpaceId + '","iPeriod":"' + iPeriod + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                success: successProjectActivityOperationMatrixAuditTrailDetails,
                error: errorProjectActivityOperationMatrixAuditTrailDetails
            }
            getProjectActivityOperationMatrixAuditTrailDetails(ajaxData.type, ajaxData.url, ajaxData.data, ajaxData.contentType, ajaxData.datatype, ajaxData.async, ajaxData.success, ajaxData.error)
        };

        var getProjectActivityOperationMatrixAuditTrailDetails = function (type, url, data, contentType, datatype, async, success, error) {
            $.ajax({
                type: type,
                url: url,
                data: data,
                contentType: contentType,
                datatype: datatype,
                async: async,
                success: success,
                error: error
            });
        }

        function successProjectActivityOperationMatrixAuditTrailDetails(jsonAuditTrailData) {
            var data = JSON.parse(jsonAuditTrailData.d);
            debugger;
            if (data.Table == "") {
                msgalert("No Audit Trail Data Available for Project Activity Operation Matrix")
            }
            else {
                var activityDataSet = [];
                for (var v = 0, l = data.Table.length; v < l; v++) {
                    var inDataSet = [];
                    inDataSet.push(data.Table[v].vProjectActivityOperationId, data.Table[v].vUserTypeName, data.Table[v].vParentNodeDisplayName, data.Table[v].vNodeDisplayName, data.Table[v].iModifyBy, data.Table[v].dModifyOn, data.Table[v].iParentNodeId, data.Table[v].vRemark);
                    activityDataSet.push(inDataSet);
                }
                var oTable = $('#tblProjectActivityOperationMatrixAuditTrail').dataTable({
                    "bJQueryUI": true,
                    "sPaginationType": "full_numbers",
                    "bLengthChange": true,
                    "iDisplayLength": 10,
                    "bProcessing": true,
                    "bSort": true,
                    "autoWidth": false,
                    "aaData": activityDataSet,
                    "bInfo": true,
                    "bDestroy": true,
                    "sScrollX": "100%",
                    "sScrollXInner": "1250",
                    "fnCreatedRow": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                        if (aData[6] == 1) {
                            $(nRow).addClass('Parent');
                        }
                        else {
                            $(nRow).addClass('Child');
                        }
                    },
                    "aoColumns": [
                        { "sTitle": "ProjectActivityOperationId" },
                        { "sTitle": "User Profiles" },
                        { "sTitle": "Parent Activity" },
                        { "sTitle": "Activity" },
                        { "sTitle": "Modify By" },
                        { "sTitle": "Modify On" },
                        { "sTitle": "iParentNodeId" },
                        { "sTitle": "Remark" },

                    ],
                    "aoColumnDefs": [
                         { "bVisible": false, "aTargets": [0, 6] },
                    ],
                    "oLanguage": {
                        "sEmptyTable": "No Record Found",
                    },
                });
                $find('mpeProjectActivityOperationMatrixAuditTrail').show();
                oTable.fnAdjustColumnSizing();
            }

        }

        function errorProjectActivityOperationMatrixAuditTrailDetails(e) {
            msgalert("ERROR : " + e)
        }

        function clear() {
            $("#legend").hide()
            $("#legendLine").hide()
            $("#ctl00_CPHLAMBDA_tvUserType").find("input[type=checkbox]").removeAttr("checked")
            $("#ctl00_CPHLAMBDA_tvActivity").find("input[type=checkbox]").removeAttr("checked")
            __doPostBack('', '');
        }

        function displayBackGround() {
            $("#ctl00_CPHLAMBDA_txtRemark").value = "";
            document.getElementById('<%=dvRemarks.ClientID%>').style.display = '';
            document.getElementById('<%=ModalBackGround.ClientId %>').style.display = '';
            document.getElementById('<%=ModalBackGround.ClientId %>').style.height = ($(document).height()) + "px";
            document.getElementById('<%=ModalBackGround.ClientId %>').style.width = ($(document).width()) + "px";
        }
        function funCloseDiv(div) {
            document.getElementById('<%=dvRemarks.ClientID%>').style.display = 'none';
            document.getElementById('<%=ModalBackGround.ClientId %>').style.display = 'none';
        }

    </script>

</asp:Content>

