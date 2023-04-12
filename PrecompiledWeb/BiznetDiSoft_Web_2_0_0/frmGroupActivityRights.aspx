      <%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmGroupActivityRights, App_Web_pna05jsx" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%--<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <style type="text/css">
        .dataTables_filter input {
            width: 100px;
            border-radius: 7px;
            padding: 1px;
            height: 18px;
            border-style: solid;
            border-width: 1PX;
            border-color: navy;
        }

        .checkDiv .dataTables_filter {
            float: left;
            text-align: center;
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

            .checkDiv .checkboxlist {
                font: Verdana;
                font-size: smaller;
                color: Black;
                height: 37px;
                width: 100%;
                font-size: 12px;
            }

        table.dataTable td {
            padding: 1px 3px;
        }

        .FieldSetBox {
            border: #aaaaaa 1px solid;
            z-index: 0px;
            border-radius: 4px;
            text-align: left;
        }

        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }
    </style>

    <div class="FormTable">
        <asp:UpdatePanel ID="upgrid" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div>
                    <table width="100%">
                        <tr>
                            <td>
                                <div style="width: 96%; margin-left: 2%;">
                                    <fieldset class="FieldSetBox">
                                        <legend class="LegendText" style="color: Black">
                                            <img id="imgfldgen" alt="Group Activity Report" src="images/panelcollapse.png"
                                                onclick="displayCRFInfo(this,'tblEntryData');" runat="server" style="margin-right: 2px;" />
                                            Screening Group Detail </legend>


                                        <fieldset class="FieldSetBox">

                                            <legend class="LegendText" style="color: Black">
                                                <%--    <img id="img1" alt="Group Activity Report" src="images/panelcollapse.png"
                                                    onclick="displayCRFInfo(this,'tblScreeningType');" runat="server" style="margin-right: 2px;" />--%>
                                                Screening Type  </legend>
                                            <div id="tblScreeningType">

                                                <table style="width: 100%; text-align: center;">
                                                    <tr>
                                                        <td colspan="2">
                                                            <center>
                                                                <asp:RadioButtonList runat="server" ID="rbtnDefault" ValidationGroup="G1"  AutoPostBack ="true"
                                                                    RepeatDirection="Horizontal" OnSelectedIndexChanged="rbtnDefault_SelectedIndexChanged" >
                                                                    <asp:ListItem Text="Generic Screening" Selected="True" Value="0000000000"></asp:ListItem>
                                                                    <asp:ListItem Text="Project Specific" Value="P"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </center>
                                                        </td>

                                                    </tr>
                                                    
                                                    <tr id="trProjectSpecific" runat="server" visible="false"  >
                                                        
                                                        <td style="text-align: right;"  >
                                                            <asp:Label runat="server" ID="lblProject" Text="Project No :"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; width:882px;">
                                                            <asp:TextBox runat="server" ID="txtProject" Width="60%" CssClass="textBox" TabIndex="1"></asp:TextBox>

                                                            <asp:Button Style="display: none" ID="btnSetProject" runat="server" txt="data" OnClick="btnSetProject_Click"/>
                                                            <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                                                CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                                CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                                                OnClientShowing="ClientPopulated" ServiceMethod="GetProjectCompletionListWithOutSponser"
                                                                ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True"
                                                                CompletionListElementID="pnlProjectList">
                                                            </cc1:AutoCompleteExtender>

                                                          <%--  <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                                                                TargetControlID="txtProject" ServicePath="AutoComplete.asmx" ServiceMethod="GetProjectCompletionListWithOutSponser"
                                                                OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1"
                                                                CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                                CompletionListCssClass="autocomplete_list" CompletionListElementID="pnlProjectList"
                                                                BehaviorID="AutoCompleteExtender1">
                                                            </cc1:AutoCompleteExtender>--%>

                                                         <%--   <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>--%>

                                                          <%--  <asp:Button Style="display: none" ID="btnSetProject" OnClick="btnSetProject_Click"
                                                                runat="server" Text=" Project" />--%>

                                                            <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" />


                                                        </td>
                                                       
                                                    </tr>
                                                </table>
                                            </div>
                                        </fieldset>


                                        <div id="tblEntryData">
                                            <table style="width: 100%; text-align: center;">
                                                <tr>
                                                    <td class="Label" style="width: 20%">Screening Group
                                                    </td>
                                                    <td style="width: 20%" class="Label">User Type
                                                    </td>

                                                </tr>
                                                <tr style="height: 180px; text-align: left; width: 100%; max-width: 100%">
                                                    <td style="width: 20%; min-width: 20%; max-width: 20%;">
                                                        <div id="Div5" class="checkDiv">
                                                            <asp:CheckBoxList ID="ChklstActivity" runat="server" CssClass="checkboxlist" />
                                                        </div>
                                                    </td>
                                                    <td style="width: 20%; min-width: 20%; max-width: 20%;">
                                                        <div id="Div1" class="checkDiv">
                                                            <asp:CheckBoxList ID="chklstUserType"  readonly="true" runat="server" CssClass="checkboxlist" />
                                                        </div>
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td colspan="5" style="height: 18px; text-align: center;">
                                                        <asp:Button ID="BtnSave" OnClientClick="return ValidationForSave();" runat="server"
                                                            CssClass="btn btnsave" Text="Save" ToolTip="Save" Width="10%" />
                                                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" Text="Cancel" ToolTip="Cancel" Width="10%" />
                                                        <asp:Button ID="BtnExit" runat="server" CssClass="btn btnexit" Text="Exit" ToolTip="Exit"
                                                            OnClientClick="return exit();" Width="10%" />
                                                        <asp:Button ID="Btninactivated" runat="server" CssClass="btn btnnew" Text="InActivated Report" ToolTip="InActivated Report"
                                                            OnClientClick="return AudtiTrail();" Width="15%" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </fieldset>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div id="Div2" style="width: 96%; margin-left: 2%;" runat="server">
                        <fieldset class="FieldSetBox" id="fldgrdParent" runat="server" style="margin-top: 26px">
                            <legend class="LegendText" style="color: Black">
                                <img id="imgfldGrid" alt="Group Activity Report" src="images/panelcollapse.png"
                                    onclick="displayCRFInfo(this,'tblGrid');" runat="server" style="margin-right: 2px;" />
                                Screening Group Report</legend>
                            <div id="tblGrid">

                                <asp:HiddenField ID="HfGridIndex" runat="server" />
                                <asp:GridView ID="GV_ActOperation" runat="server" Style="width: 100%; margin: auto;"
                                    AutoGenerateColumns="False" AllowSorting="True"
                                    EmptyDataRowStyle-HorizontalAlign="Center">
                                    <Columns>
                                        <asp:BoundField DataField="nScreeningGroupMatrixNo" HeaderText="ActivityRoleId" />
                                        <asp:BoundField DataField="vMedExGroupCode" HeaderText="Activity Id" />
                                        <asp:BoundField DataField="vmedexgroupDesc" HeaderText="Screening Group Name" />
                                        <asp:BoundField DataField="vUserTypeCode" HeaderText="User Type Code" />
                                        <asp:BoundField DataField="vUserTypeName" HeaderText="User Type Name" />
                                        <asp:BoundField DataField="vWorkSpaceId" HeaderText="Project Name" />
                                        <asp:BoundField DataField="vUserName" HeaderText="Modify By" />
                                        <asp:BoundField DataField="dModifyOn" HeaderText="Modify On " />

                                        <asp:TemplateField HeaderText="InActive">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgDelete" runat="server" ImageUrl="~/Images/i_delete.gif"
                                                    ToolTip="InActive" OnClientClick="return validatecancel();" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="vWorkSpaceIdNew" HeaderText="WorkSpace Id" />
                                    </Columns>
                                </asp:GridView>
                                <div style="width: 100%; text-align: center;">
                                    <asp:Button ID="btnExport" runat="server" CssClass="btn btnexcel" Text="" ToolTip="Export" />
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>

                <asp:PostBackTrigger ControlID="btnExport" />


            </Triggers>
        </asp:UpdatePanel>
        <asp:Button ID="btnRemarks" runat="server" Style="display: none;" TabIndex="55" />
        <cc1:ModalPopupExtender ID="mdlRemarks" runat="server" PopupControlID="divRemarks"
            BackgroundCssClass="modalBackground" BehaviorID="mdlRemarks" CancelControlID="btnRemarksCancel"
            TargetControlID="btnRemarks">
        </cc1:ModalPopupExtender>
        <div id="divRemarks" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 32%; height: auto; max-height: 45%; min-height: auto;">
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td class="LabelText" style="text-align: left !important; font-size: 12px !important; width: 97%;">Remarks
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td class="LabelText" style="text-align: left !important;">Enter Remarks:
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left !important;">
                        <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="5" Height="60px" onkeyup="characterlimit(this)"
                            Width="300px" TabIndex="55" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btnSaveRemarks" runat="server" Text="Save" CssClass="btn btnsave"
                            Width="64px" Style="font-size: 12px !important;" TabIndex="56" OnClientClick="return validationremarks();" />
                        <asp:Button ID="btnRemarksCancel" runat="server" Text="Cancel" CssClass="btn btncancel" OnClientClick="return validatecancel();"
                            Width="64px" Style="font-size: 12px !important;" TabIndex="56" />

                    </td>
                </tr>
            </table>
        </div>
        <button id="btn3" runat="server" style="display: none;" />
        <cc1:ModalPopupExtender ID="MPE_CleintMstHistory" runat="server" PopupControlID="dvClientMstAudiTrail" BehaviorID="MPE_CleintMstHistory"
            PopupDragHandleControlID="LbldRandomizationDtl" BackgroundCssClass="modalBackground" CancelControlID="imgClientAuditTrail"
            TargetControlID="btn3">
        </cc1:ModalPopupExtender>
        <div>
            <table>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpClientAuditTrail" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <asp:HiddenField ID="hdnExportAuditdata" runat="server" />
                                <div id="dvClientMstAudiTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
                                    <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                        <tr>
                                            <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 97%;">
                                                <asp:Label ID="lblRamge" runat="server" Text="InActivated Information"></asp:Label>
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
                                        <tr>
                                            <td colspan="2">
                                                <table align="center" border="0" cellpadding="2" cellspacing="2" width="100%">
                                                    <tr>

                                                        <td>
                                                            <table id="tblClientMstAudit" class="tblAudit" width="100%" style="background-color: aliceblue;"></table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <hr />
                                                <asp:Button ID="btnexportAudit" runat="server" CssClass="btn btnexcel"
                                                    OnClientClick="return CheckAuditData();" Width="12%" Style="font-size: 12px !important;" TabIndex="56" />
                                            </td>
                                        </tr>

                                    </table>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnexportAudit" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <%--<script type="text/javascript" src="Script/jquery-1.4.3.min.js"></script>--%>
    <script src="Script/jquery.dataTables.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <script type="text/javascript" language="javascript">

        function pageLoad() {

            UIgvClient()

            var $ahead = $('<thead>').append('<tr>');
            $('#ctl00_CPHLAMBDA_ChklstActivity tr:first td').each(function () {
                $('tr', $ahead).append($('<th>'));
            });

            aTable = $('#<%=ChklstActivity.ClientID%>').prepend($ahead).dataTable({
                "bStateSave": true,
                "bPaginate": false,
                "bRetrieve": true,
                "bDestroy": true,                   // To disable pagination.         
                "bSort": false,
                "bFilter":false
            });
            aTable.fnFilter('');


            var $ahead = $('<thead>').append('<tr>')
            $('#ctl00_CPHLAMBDA_chklstUserType tr:first td').each(function () {
                $('tr', $ahead).append($('<th>'));
            });

            var bTable = $('#<%= chklstUserType.ClientID %>').prepend($ahead).dataTable({
                "bStateSave": true,
                "bPaginate": false,
                "bSort": false,
                "bDestory": true,
                "bRetrieve": true,
                "bInfo": false,
                "bFilter":false
            });
            bTable.fnFilter('');

        }

        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
                    $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));

   }


   function UIgvClient() {
       $('#<%= GV_ActOperation.ClientID%>').removeAttr('style', 'display:block');
            oTab = $('#<%= GV_ActOperation.ClientID%>').prepend($('<thead>').append($('#<%= GV_ActOperation.ClientID%> tr:first'))).dataTable({
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

            if ($('[id$="' + '<%= GV_ActOperation.ClientID%>' + '"] tbody tr').length > 0) {
                $('#<%= fldgrdParent.ClientID%>').show();
            }
            else {

                $('#<%= fldgrdParent.ClientID%>').hide();
            }


            return false;

        }

        function ValidationForSave() {
            var chklstActivity = document.getElementById('<%=ChklstActivity.clientid%>');
            var chklstUserType = document.getElementById('<%=chklstUserType.clientid%>');
            var chks;
            var result = false;
            var i;

            if ($('#ctl00_CPHLAMBDA_rbtnDefault :checked').val() == 'P') {
                if ($('#ctl00_CPHLAMBDA_HProjectId').val() == "") {
                    msgalert("Please Select Project !")
                    return false;
                }
            }
            if (chklstActivity != null && typeof (chklstActivity) != 'undefined') {
                chks = chklstActivity.getElementsByTagName('input');
                for (i = 0; i < chks.length; i++) {
                    if (chks[i].type.toUpperCase() == 'CHECKBOX' && chks[i].checked) {
                        if (chks[i].value   != "No Data Found") {
                        result = true;
                        break;
                        }
                    }
                }
                if (!result) {
                    msgalert('Please Select Atleast One Screening Group !');
                    return false;
                }
            }

            if (chklstUserType != null && typeof (chklstUserType) != 'undefined') {
                result = false;
                chks = chklstUserType.getElementsByTagName('input');
                for (i = 0; i < chks.length; i++) {
                    if (chks[i].type.toUpperCase() == 'CHECKBOX' && chks[i].checked) {
                        result = true;
                        break;
                    }
                }
                if (!result) {
                    msgalert('Please Select Atleast One User Type !');
                    return false;
                }
            }


            return true;
        }
        function validationremarks() {
            if (document.getElementById('<%= txtRemarks.ClientID%>').value.trim() == "") {
                msgalert("Please Enter Remarks !");
                return false
            }
            return true
        }
        function validatecancel() {
            document.getElementById('<%= txtRemarks.ClientId %>').value = '';
            return true

        }
        function characterlimit(id) {

            var text = id.value
            var textLength = text.length;
            if (textLength > 100) {
                $(id).val(text.substring(0, (100)));
                msgalert("Only 100 characters are allowed !");
            }
            else {
                return true;
            }

        }
        function displayCRFInfo(control, target) {

            if (control.src.toString().toUpperCase().search("EXPAND") != -1) {
                $("#" + target).slideToggle(400);
                control.src = "images/panelcollapse.png";
            }
            else {
                $("#" + target).slideToggle(400);
                control.src = "images/panelexpand.png";
            }
        }
        function exit() {
            msgConfirmDeleteAlert(null, "Are you sure want to Exit ?", function (isConfirmed) {
                if (isConfirmed) {
                    window.location = "frmMainPage.aspx";
                    return true;
                } else {
                    return false;
                }
            });



        }
        function CheckAuditData() {
            if ($('#<%= hdnExportAuditdata.ClientID%>').val() == '[]') {
                msgalert("No Record Found For Export !")
                return false
            }
        }
        function AudtiTrail() {
            var iUserid = '<%= Session(S_UserID)%>';

            $.ajax({
                type: "post",
                url: "frmGroupActivityRights.aspx/AuditTrail",
                data: '{"iUserid":"' + iUserid + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                success: function (data) {
                    $('#tblClientMstAudit').attr("IsTable", "has");
                    var aaDataSet = [];
                    var range = null;

                    $('#<%= hdnExportAuditdata.ClientID%>').val("");
                    $('#<%= hdnExportAuditdata.ClientID%>').val(data.d);

                    if (data.d != "" && data.d != null) {
                        data = JSON.parse(data.d);
                        for (var Row = 0; Row < data.length; Row++) {
                            var InDataSet = [];
                            InDataSet.push(data[Row].vmedexgroupDesc, data[Row].vUserTypeName, data[Row].vRemarks, data[Row].vUserName, data[Row].dModifyOn);
                            aaDataSet.push(InDataSet);
                        }

                    }
                    if ($("#tblClientMstAudit").children().length > 0) {
                        $("#tblClientMstAudit").dataTable().fnDestroy();
                    }

                    oTable = $('#tblClientMstAudit').prepend($('<thead>').append($('#tblClientMstAudit tr:first'))).dataTable({

                        "bJQueryUI": true,
                        "sPaginationType": "full_numbers",
                        "bLengthChange": true,
                        "iDisplayLength": 10,
                        "bProcessing": true,
                        "bSort": false,
                        "aaData": aaDataSet,
                        aLengthMenu: [
                                [10, 25, 50, 100, -1],
                                [10, 25, 50, 100, "All"]
                        ],
                        "aoColumns": [
                            { "sTitle": "Screening Group Name" },
                             { "sTitle": "User Type Name" },
                            { "sTitle": "Remarks" },
                            { "sTitle": "ModifiedBy" },
                            { "sTitle": "ModifiedOn" },

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
                    $find('MPE_CleintMstHistory').show();

                },
                failure: function (response) {
                    msgalert(response.d);
                },
                error: function (response) {
                    msg
                    msgalert(response.d);
                }
            });

            return false;

        }
    </script>

</asp:Content>

