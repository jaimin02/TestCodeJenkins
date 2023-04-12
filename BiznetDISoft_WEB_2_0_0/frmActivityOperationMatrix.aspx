<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmActivityOperationMatrix.aspx.vb" Inherits="frmActivityOperationMatrix" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

        /*#ctl00_CPHLAMBDA_GV_ActOperation_wrapper {
            margin: 0px 235px;
        }*/

        .dataTables_filter input {
            width: 100px;
        }

        .dataTables_filter {
            float: right;
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
                font-name: Verdana;
                font-size: smaller;
                color: Black;
                height: 37px;
                width: 100%;
            }

        table.dataTable td {
            padding: 1px 3px;
        }
        .ui-state-default a, .ui-state-default a:link, .ui-state-default a:visited {
            color: white !important;
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
        #ctl00_CPHLAMBDA_GV_ActOperation_info {
            display:none;
        }

        #ctl00_CPHLAMBDA_GV_ActOperation_paginate {
            visibility:hidden;
        }

    </style>
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="Script/jquery-1.4.3.min.js"></script>
    <script src="Script/jquery.dataTables.min.js" type="text/javascript"></script>
    <script src="Script/jquery.dataTables.plugins.js" type="text/javascript"></script>
    <script src="Script/FixedHeader.min.js" type="text/javascript"></script>
    <%--<script src="Script/jquery.searchabledropdown-1.0.7.min.js" type="text/javascript"></script>--%>
   
    <div class="FormTable" style="margin: auto; width: 94%;">
        <table width="100%">
            <tr>
                <td>
                    <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                        <legend class="LegendText" style="color: Black; font-size: 12px">
                            <img id="img2" alt="Activity Operation Details" src="images/panelcollapse.png"
                                onclick="Display(this,'divActivityOperationDetail');" runat="server" style="margin-right: 2px;" />Activity Operation Details</legend>
                        <div id="divActivityOperationDetail" style="border: 1px solid blue; overflow: scroll; width: 100%">
                            <table style="width: 100%; text-align: center;">
                                <tr>
                                    <td class="Label" style="width: 20%">Activity
                                    </td>
                                    <td style="width: 20%" class="Label">User Type
                                    </td>
                                    <td style="width: 20%" class="Label">Department
                                    </td>
                                    <td style="width: 20%" class="Label">Location
                                    </td>
                                    <td style="width: 20%" class="Label">Operation
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
                                            <asp:CheckBoxList ID="chklstUserType" runat="server" CssClass="checkboxlist" />
                                        </div>
                                    </td>
                                    <td style="width: 20%; min-width: 20%; max-width: 20%;">
                                        <div id="Div2" class="checkDiv">
                                            <asp:CheckBoxList ID="chklstDept" runat="server" CssClass="checkboxlist" />
                                        </div>
                                    </td>
                                    <td style="width: 20%; min-width: 20%; max-width: 20%;">
                                        <div id="Div3" class="checkDiv">
                                            <asp:CheckBoxList ID="chklstLocation" runat="server" CssClass="checkboxlist" />
                                        </div>
                                    </td>
                                    <td style="width: 20%; min-width: 20%; max-width: 20%;">
                                        <div id="Div4" class="checkDiv">
                                            <asp:CheckBoxList ID="chklstOperation" runat="server" CssClass="checkboxlist" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5" style="height: 18px; text-align: center;">
                                        <asp:Button ID="BtnSave" OnClientClick="return ValidationForSave();" runat="server"
                                            CssClass="btn btnsave" Text="Save" ToolTip="Save" />
                                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" Text="Cancel" ToolTip="Cancel" />
                                        <asp:Button ID="BtnExit" runat="server" CssClass="btn btnexit" Text="Exit" ToolTip="Exit"
                                            OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </fieldset>
                </td>
            </tr>
        </table>       

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table style="width: 100%;" cellpadding="5px">
                    <tbody>
                        <tr>
                            <td>
                                <fieldset class="FieldSetBox" style="display: block; width: 97%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                                    <legend class="LegendText" style="color: Black; font-size: 12px">
                                        <img id="img1" alt="Activity Operation Data" src="images/panelcollapse.png"
                                            onclick="Display(this,'divActivityOperationData');" runat="server" style="margin-right: 2px;" />Activity Operation Data</legend>
                                    <div id="divActivityOperationData">
                                        <table style="margin: auto; width: 90%;">
                                            <tr>
                                               
                                                     <td style="text-align: center;">
                                                    <span id="LActivity">ActivityName:</span>
                                                    <asp:DropDownList ID="DdlActivityName" runat="server" Width="200px" CssClass="dropDownList"
                                                         />&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span id="LUserName">UserTypeName:</span>
                                                    <asp:DropDownList runat="server" ID="DdlUserType" Width="200px" CssClass="dropDownList"  />
                                                    &nbsp;&nbsp;&nbsp;&nbsp;                              
                                            <asp:Button runat="server" ID="btnGo" CssClass="btn btngo" Text="" OnClientClick="return Alert();" ToolTip="Go" />
                                                </td>
                                              
                                               
                                            </tr>
                                            <tr>
                                                <td align="center" style="padding-bottom: 2px;">
                                                    <asp:PlaceHolder ID="phTopPager" runat="server"></asp:PlaceHolder>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <div style="max-height: 100%; overflow: auto; width: 100%">
                                                        <asp:GridView ID="GV_ForExportToExcel" runat="server" AutoGenerateColumns="false"
                                                            Style="display: none">
                                                            <Columns>
                                                                <asp:BoundField DataField="vActivityName" HeaderText="Activity Name" SortExpression="vActivityName" />
                                                                <asp:BoundField DataField="vUserTypeName" HeaderText="User Type Name" SortExpression="vUserTypeName" />
                                                                <asp:BoundField DataField="vDeptName" HeaderText="Dept. Name" SortExpression="vDeptName" />
                                                                <asp:BoundField DataField="vLocationName" HeaderText="Location Name" SortExpression="vLocationName" />
                                                                <asp:BoundField DataField="vOperationName" HeaderText="Operation Name" SortExpression="vOperationName" />
                                                            </Columns>
                                                        </asp:GridView>
                                                        <asp:GridView ID="GV_ActOperation" runat="server" Style="width: 100%; margin: auto; display: none;"
                                                            OnPageIndexChanging="GV_ActOperation_PageIndexChanging" OnRowDeleting="GV_ActOperation_RowDeleting"
                                                            AutoGenerateColumns="False" AllowSorting="True" EmptyDataText="No records Found">

                                                            <Columns>
                                                                <asp:BoundField HeaderText=" # "/>
                                                                <asp:BoundField DataField="vActivityRoleId" HeaderText="ActivityRoleId" SortExpression="vActivityRoleId" />
                                                                <asp:BoundField DataField="vActivityId" HeaderText="Activity Id" SortExpression="vActivityId" />
                                                                <asp:BoundField DataField="vActivityName" HeaderText="Activity Name" SortExpression="vActivityName" />
                                                                <asp:BoundField DataField="vUserTypeCode" HeaderText="User Type Code" SortExpression="vUserTypeCode" />
                                                                <asp:BoundField DataField="vUserTypeName" HeaderText="User Type Name" SortExpression="vUserTypeName" />
                                                                <asp:BoundField DataField="vDeptCode" HeaderText="Dept Code" SortExpression="vDeptCode" />
                                                                <asp:BoundField DataField="vDeptName" HeaderText="Dept. Name" SortExpression="vDeptName" />
                                                                <asp:BoundField DataField="vLocationCode" HeaderText="Location Code" SortExpression="vLocationCode" />
                                                                <asp:BoundField DataField="vLocationName" HeaderText="Location Name" SortExpression="vLocationName" />
                                                                <asp:BoundField DataField="vOperationCode" HeaderText="Operation Code" SortExpression="vOperationCode" />
                                                                <asp:BoundField DataField="vOperationName" HeaderText="Operation Name" SortExpression="vOperationName" />
                                                                <asp:TemplateField HeaderText="Delete">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ImgDelete" runat="server" ImageUrl="~/Images/i_delete.gif"
                                                                            ToolTip="Delete" OnClientClick="return msgconfirmalert('Are You Sure You Want To Delete?',this)" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                        <div id="createtable"></div>
                                                    </div>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td align="center" style="padding-top: 2px;">
                                                    <asp:PlaceHolder ID="phBottomPager" runat="server"></asp:PlaceHolder>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: center;">
                                                    <asp:Button ID="btnImportToExcel" runat="server" Text="" CssClass="btn btnexcel"
                                                        ToolTip="Export To Excel" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </fieldset>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </ContentTemplate>
            <Triggers>
                <%--<asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />--%>
                <asp:PostBackTrigger ControlID="btnImportToExcel" />
                <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
     <div id='loadingmessage' style='display:none'>
                
                </div>
    <script type="text/javascript" language="javascript">

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

        function ValidationForSave() {
            var chklstActivity = document.getElementById('<%=ChklstActivity.clientid%>');
            var chklstUserType = document.getElementById('<%=chklstUserType.clientid%>');
            var chklstDept = document.getElementById('<%=chklstDept.clientid%>');
            var chklstLocation = document.getElementById('<%=chklstLocation.clientid%>');
            var chklstOperation = document.getElementById('<%=chklstOperation.clientid%>');
            var chks;
            var result = false;
            var i;

            if (chklstActivity != null && typeof (chklstActivity) != 'undefined') {
                chks = chklstActivity.getElementsByTagName('input');
                for (i = 0; i < chks.length; i++) {
                    if (chks[i].type.toUpperCase() == 'CHECKBOX' && chks[i].checked) {
                        result = true;
                        break;
                    }
                }
                if (!result) {
                    msgalert('Please Select Atleast One Activity !');
                    return false;
                }
            }

            if (chklstUserType != null && typeof (chklstUserType) != 'undefined') {
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

            if (chklstDept != null && typeof (chklstDept) != 'undefined') {
                result = false;
                chks = chklstDept.getElementsByTagName('input');
                for (i = 0; i < chks.length; i++) {
                    if (chks[i].type.toUpperCase() == 'CHECKBOX' && chks[i].checked) {
                        result = true;
                        break;
                    }
                }
                if (!result) {
                    msgalert('Please Select Atleast One Department !');
                    return false;
                }
            }

            if (chklstLocation != null && typeof (chklstLocation) != 'undefined') {
                result = false;
                chks = chklstLocation.getElementsByTagName('input');
                for (i = 0; i < chks.length; i++) {
                    if (chks[i].type.toUpperCase() == 'CHECKBOX' && chks[i].checked) {
                        result = true;
                        break;
                    }
                }
                if (!result) {
                    msgalert('Please Select Atleast One Location !');
                    return false;
                }
            }

            if (chklstOperation != null && typeof (chklstOperation) != 'undefined') {
                result = false;
                chks = chklstOperation.getElementsByTagName('input');
                for (i = 0; i < chks.length; i++) {
                    if (chks[i].type.toUpperCase() == 'CHECKBOX' && chks[i].checked) {
                        result = true;
                        break;
                    }
                }
                if (!result) {
                    msgalert('Please Select Atleast One Operation !');
                    return false;
                }
            }
            return true;
        }

        function Alert() {
            if (($('#ctl00_CPHLAMBDA_DdlActivityName').val() == "Select Activity") && ($('#ctl00_CPHLAMBDA_DdlUserType').val() == "Select UserType")) {
                msgalert("Please Select Activity or UserType !");
                return false;
            }
            else {
                return true;
            }
        }

        function CheckGridData() {
            if (($('#ctl00_CPHLAMBDA_DdlActivityName').val() == "Select Activity") && ($('#ctl00_CPHLAMBDA_DdlUserType').val() == "Select UserType")) {
                msgalert("This Much Of Large Data Cannot Be Export..Please Filter Records And Try !");
                return false;
            }
            else {
                return true;
            }
        }
        // for fix gridview header aded on 22-nov-2011
        function pageLoad() {
            //fnGetData();

            //$("#ctl00_CPHLAMBDA_DdlActivityName").searchable({
            //    width: '260'
            //});

            //$("#ctl00_CPHLAMBDA_DdlUserType").searchable({
            //    width: '260'
            //});

            

            var $ahead = $('<thead>').append('<tr>');
            $('#ctl00_CPHLAMBDA_ChklstActivity tr:first td').each(function () {
                $('tr', $ahead).append($('<th>'));
            });

            aTable = $('#<%=ChklstActivity.ClientID%>').prepend($ahead).dataTable({
                "bStateSave": true,
                //"sPaginationType": "scrolling",
                "bPaginate": false,
                "bRetrieve": true,
                "bDestroy": true,
                "bSort": false
            });
            //aTable.fnFilter('');


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
                "bInfo": false
            });
            //bTable.fnFilter('');


            var $ahead = $('<thead>').append('<tr>')
            $('#ctl00_CPHLAMBDA_chklstDept tr:first td').each(function () {
                $('tr', $ahead).append($('<th>'));
            });

            var cTable = $('#<%= chklstDept.ClientID %>').prepend($ahead).dataTable({
                "bStateSave": true,
                "bPaginate": false,
                "bSort": false,
                "bDestory": true,
                "bRetrieve": true,
                "bInfo": false
            });

            //cTable.fnFilter('');


            var $ahead = $('<thead>').append('<tr>')
            $('#ctl00_CPHLAMBDA_chklstLocation tr:first td').each(function () {
                $('tr', $ahead).append($('<th>'));
            });

            var dTable = $('#<%= chklstLocation.ClientID %>').prepend($ahead).dataTable({
                "bStateSave": true,
                "bPaginate": false,
                "bSort": false,
                "bDestory": true,
                "bRetrieve": true,
                "bInfo": false
            });
            //dTable.fnFilter('');



            var $ahead = $('<thead>').append('<tr>')
            $('#ctl00_CPHLAMBDA_chklstOperation tr:first td').each(function () {
                $('tr', $ahead).append($('<th>'));
            });


            var eTable = $('#<%= chklstOperation.ClientID %>').prepend($ahead).dataTable({
                "bStateSave": true,
                "bPaginate": false,
                "bSort": false,
                "bDestory": true,
                "bRetrieve": true,
                "bInfo": false
            });
            //eTable.fnFilter('');
        }
        function UIGV_ActOperation() {
            $('#<%= GV_ActOperation.ClientID%>').removeAttr('style', 'display:block');
            oTab = $('#<%= GV_ActOperation.ClientID%>').prepend($('<thead>').append($('#<%= GV_ActOperation.ClientID%> tr:first'))).dataTable({
                "bJQueryUI": true,
                //"sPaginationType": "full_numbers",
                "bLengthChange": false,
                "iDisplayLength": 1000,
                "bProcessing": true,
                "bSort": false,
                "sScrollY": "250px",
                "sScrollX": "100%",
                "bScrollCollapse": true,
                "bDestroy": true,
            });
            return false;
        }
        var summarydata = '';
        var Table = '';
        var valueid = '';
        var valueidarr = [];
        function fnGetData() {
            debugger;
            $('#loadingmessage').show();
            $.ajax({
                type: "post",
                serverSide: true,
                url: "frmActivityOperationMatrix.aspx/View_ActivityOperationMatrix",

                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: true,
                dataType: "json",
                success: function (data) {
                    var data = data.d;
                    var msgs = JSON.parse(data);
                    summarydata = msgs;
                    if (summarydata == "") return false;
                    // Table = Object(keys(summarydata))[0];
                    CreateSummaryTable(summarydata);
                    $('#loadingmessage').hide();
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
            var jsondata = summarydata.VIEW_ACTIVITYOPERATIONMATRIX;
            for (var Row = 0; Row < jsondata.length; Row++) {
                var InDataset = [];
                InDataset.push(Row + 1, jsondata[Row]['vActivityName'], jsondata[Row]['vUserTypeName'], jsondata[Row]['vDeptName'], jsondata[Row]['vLocationName'], jsondata[Row]['vOperationName'], "", jsondata[Row]['vActivityRoleId']);
                ActivityDataset.push(InDataset);

            }

            $ = jQuery;
            var createtable1 = $("<table id='Activityrecord'  border='1' class='display' cellspacing='0' width='100%'> </table>");
            $("#createtable").empty().append(createtable1);

            $('#Activityrecord').DataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": false,
                "sScrollY": "250px",
                "sScrollX": "100%",
                "bScrollCollapse": true,
                "aaData": ActivityDataset,
                "fnCreatedRow": function (nRow, aData, iDataIndex) {
                    $('td:eq(6)', nRow).append("<input type='image' id='imgdel" + iDataIndex + "' name='imgdel$" + iDataIndex + "' src='images/i_delete.gif'; tempval='" + aData[7] + "';   OnClick='DeleteCellRow(this); return false;' style='border-width:0px;' >");
                },
                //"fnRowCallback": function (nRow, aData, iDisplayIndex) {
                //    $("td:first", nRow).html(iDisplayIndex + 1);
                //    return nRow;
                //},
                "aoColumns": [
                                                  { "sTitle": "#" },
                                              { "sTitle": "Activity Name" },
                                              { "sTitle": "User Type Name" },
                                              { "sTitle": "Dept. Name" },
                                              { "sTitle": "Location Name" },
                                              { "sTitle": "Operation Name" },
                                               { "sTitle": "Delete" }


                ],

                "columns": [
                    null, null, null, null, null, null, null, null, null, null, null, null, null
                ],
                "oLanguage": {
                    "sEmptyTable": "No Record Found"
                },
            });

            $('#Activityrecord').show();


        }
        var r = '';
        function ConfirmDelete() {

            r = confirm("Are You Sure You Want To Delete?");

            r = msgConfirmDeleteAlert("", "Are You Sure You Want To Delete?", "")

        }

        function DeleteCellRow(e) {
            debugger;
            var id = e.attributes.tempval.value;
            // $('#loadingmessage').show();

            msgConfirmDeleteAlert(null, "Are You Sure You Want To Delete?", function (isConfirmed) {
                if (isConfirmed) {
                    $('#loadingmessage').show();
                    $.ajax({
                        type: "post",
                        url: "frmActivityOperationMatrix.aspx/Delete_ActivityOperationMatrix",
                        data: "{'id':'" + id + "'}",
                        contentType: "application/json; charset=utf-8",
                        datatype: JSON,
                        async: true,
                        dataType: "json",
                        success: function (data) {
                            var data = data.d;
                            var msgs = JSON.parse(data);
                            summarydata = msgs;
                            if (summarydata == "") return false;
                            // Table = Object(keys(summarydata))[0];
                            CreateSummaryTable(summarydata);
                            $('#loadingmessage').hide();
                            msgalert("Record Deleted Sucessfully!!")
                        },
                        failure: function (response) {
                            alert("failure");
                            $('#loadingmessage').hide();
                            alert(data.d);
                        },
                        error: function (response) {
                            alert("error");
                            $('#loadingmessage').hide();
                        }
                    });
                    return true;
                } else {
                    return false;
                }
            });


            //msgalert("Record Deleted Sucessfully!!");

            //ConfirmDelete();

            //if (r == true) {
            //    $.ajax({
            //        type: "post",
            //        url: "frmActivityOperationMatrix.aspx/Delete_ActivityOperationMatrix",
            //        data: "{'id':'" + id + "'}",
            //        contentType: "application/json; charset=utf-8",
            //        datatype: JSON,
            //        async: false,
            //        dataType: "json",
            //        success: function (data) {
            //            var data = data.d;
            //            var msgs = JSON.parse(data);
            //            summarydata = msgs;
            //            if (summarydata == "") return false;
            //            // Table = Object(keys(summarydata))[0];
            //            CreateSummaryTable(summarydata);
            //              $('#loadingmessage').hide();
            //            alert("Record Deleted Sucessfully!!")
            //        },
            //        failure: function (response) {
            //            alert("failure");
            //            alert(data.d);
            //        },
            //        error: function (response) {
            //            alert("error");
            //        }
            //    });
            //}

        }
      
    </script>

</asp:Content>
