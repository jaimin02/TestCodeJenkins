<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmDrugMst, App_Web_mlepfeoz" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
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
        #tblDrugMstAudit_wrapper {
          width: 1100px;
          overflow:auto;
        }
        #tblDrugMstAudit {
            width:100% !important;
        } 

    </style>

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <table cellpadding="5px" style="width: 100%;">
        <tr>
            <td>
                <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                    <legend class="LegendText" style="color: Black; font-size: 12px">
                        <img id="img2" alt="Drug Details" src="images/panelcollapse.png"
                            onclick="Display(this,'divDrugDetail');" runat="server" style="margin-right: 2px;" />Drug Details</legend>
                    <div id="divDrugDetail">
                        <table border="0" align="center" style="width: 80%; margin-bottom: 2%; margin-left: 1%;" cellpadding="5px">
                            <tr>
                                <td class="Label" style="width: 20%; text-align: right;">Drug Name* :
                                </td>
                                <td style="text-align: left; width: 25%">
                                    <asp:TextBox ID="txtDrugName" runat="server" CssClass="textBox" Width="100%" MaxLength="250" />
                                </td>

                                <td style="text-align: right; width: 10%;" class="Label">Food Effect :
                                </td>
                                <td style="text-align: left; width: 25%;">
                                    <asp:TextBox ID="txtFood" runat="server" CssClass="textBox" TextMode="MultiLine"
                                        Width="100%" MaxLength="100" />
                                </td>

                            </tr>
                            <tr>
                                <td style="text-align: right;" class="Label">WashOut Period :
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="TxtWashOutPeriod" onblur="Numeric('Washout');" runat="server" CssClass="textBox"
                                        Width="80%" MaxLength="50" />
                                    days
                                </td>

                                <td style="text-align: right; width: 10%;" class="Label">Strength :
                                </td>
                                <td style="text-align: left; width: 25%;">
                                    <asp:TextBox ID="txtstrength" runat="server" CssClass="textBox"
                                        Width="100%" MaxLength="100" />
                                </td>

                            </tr>
                            <tr>
                                <td style="text-align: right;" class="Label">Housing :
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="TxtHousing" onblur="Numeric('Housing');" runat="server" CssClass="textBox"
                                        Width="30%" MaxLength="50" />
                                    Hrs.
                                </td>

                                <td style="text-align: right; width: 10%;" class="Label">Formulation :
                                </td>
                                <td style="text-align: left; width: 25%;">
                                    <asp:TextBox ID="txtformulation" runat="server" CssClass="textBox"
                                        Width="100%" MaxLength="100" />
                                </td>

                            </tr>
                            <tr>
                                <td style="text-align: right;" class="Label">Drug Synonyms :
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="txtSynonyms" runat="server" CssClass="textBox" TextMode="MultiLine"
                                        Width="100%" MaxLength="250" />
                                </td>

                                <td style="text-align: right; width: 10%;" class="Label">Release :
                                </td>
                                <td style="text-align: left; width: 25%;">
                                    <asp:TextBox ID="txtrelease" runat="server" CssClass="textBox"
                                        Width="100%" MaxLength="100" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;" class="Label">Remark* :
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="txtRemark" runat="server" Rows="3" Columns="20" CssClass="textBox" Style="width: 100%; height: auto;" TextMode="MultiLine" MaxLength="500" />
                                </td>

                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td class="Label" nowrap="nowrap" colspan="8" style="text-align: center; vertical-align: top;">
                                    <asp:Button ID="BtnSave" runat="server" CssClass="btn btnsave" Text="Save" ToolTip="Save"
                                        OnClientClick=" return Validation()" />
                                    <asp:Button ID="btnExportToExcelGrid" runat="Server" Font-Size="Smaller" CssClass="btn btnexcel" ToolTip="Export To Excel" />
                                    <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" Text="Cancel" ToolTip="Cancel" />
                                    <asp:Button ID="BtnExit" runat="server" CssClass="btn btnexit" Text="Exit" ToolTip="Exit"
                                        OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
                                </td>
                            </tr>
                            <tr>
                                <td style="display: none; text-align: left;">Active :
                                </td>
                                <td style="display: none; text-align: left;">
                                    <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </fieldset>
            </td>
        </tr>
    </table>

    <asp:UpdatePanel ID="Up_View" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table border="0" align="center" style="width: 98%; margin-bottom: 2%;" cellpadding="5px">
                <tr>
                    <td>
                        <fieldset class="FieldSetBox" style="display: block; width:98%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                            <legend class="LegendText" style="color: Black; font-size: 12px">
                                <img id="img1" alt="Drug Data" src="images/panelcollapse.png"
                                    onclick="Display(this,'divDrugData');" runat="server" style="margin-right: 2px;" />Drug Data</legend>
                            <div id="divDrugData">
                                <table style="width: 100%; text-align: center;" cellpadding="5px">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="GV_Drug" runat="server" Style="display: none; width:60%; margin:auto;" AutoGenerateColumns="False"
                                                        OnRowCommand="GV_Drug_RowCommand" OnRowCreated="GV_Drug_RowCreated" OnRowDataBound="GV_Drug_RowDataBound"
                                                        ShowFooter="false">
                                                        <Columns>
                                                            <asp:BoundField HeaderText="Sr. No">
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="vDrugCode" HeaderText="Drug Code"></asp:BoundField>
                                                            <asp:BoundField DataField="vDrugName" HeaderText="Drug Name"></asp:BoundField>
                                                            <asp:BoundField DataField="vWashOutPeriod" HeaderText="WashOut Period"></asp:BoundField>
                                                            <asp:BoundField DataField="vHousing" HeaderText="Housing"></asp:BoundField>
                                                            <asp:BoundField DataField="vDrugSynonyms" HeaderText="Drug Synonyms"></asp:BoundField>
                                                            <asp:BoundField DataField="vFoodEffect" HeaderText="Food Effect"></asp:BoundField>
                                                            <asp:BoundField DataField="cActiveFlag" HeaderText="Active"></asp:BoundField>

                                                            <asp:BoundField DataField="vstrength" HeaderText="Strength"></asp:BoundField>
                                                            <asp:BoundField DataField="vformulation" HeaderText="Formulation"></asp:BoundField>
                                                            <asp:BoundField DataField="vrelease" HeaderText="Release"></asp:BoundField>
                                                            <asp:TemplateField HeaderText="Edit" SortExpression="status">
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="lnkEdit" runat="server" ImageUrl="~/images/Edit2.gif" ToolTip="Edit" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Audit Trail">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="lnkAudit" runat="server" ImageUrl="~/Images/audit.png" ToolTip="Audit Trial" OnClientClick="AudtiTrail(this); return false;" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Export">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <img src="images/Export.gif" onclick="ExportToExcel(this.id);" id='<%# Eval("vDrugCode")%>' />
                                                                    </center>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                </asp:GridView>
                                                  <div id ="createtable">
                                                    </div>
                                                  <asp:Button ID="btnEdit" runat="server" Text="Edit" ToolTip="Edit"   style="display:none" />
                                                 <asp:HiddenField ID="hdnEditedId" runat="server" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </fieldset>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <div>
        <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpDrugMstAuditTrail" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div id="dvDrugMstAuditTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 83%; height: auto; max-height: 75%; min-height: auto;">
                                <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                    <tr>
                                        <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 97%;">
                                            <asp:Label ID="lblRamge" runat="server" Text="Audit Trail Information"></asp:Label>
                                        </td>
                                        <td style="width: 3%">
                                            <img id="imgDrugMstAuditTrail" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
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
                                                        <table id="tblDrugMstAudit" class="tblAudit" border='1' style="background-color: aliceblue; width: 100% !important; overflow:auto;display: block; height: 200px;"></table>
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
    <div id='loadingmessage' style='display:none'>
                   
                </div>

    <button id="btn3" runat="server" style="display: none;" />

    <cc1:ModalPopupExtender ID="MPE_DrugMstHistory" runat="server" PopupControlID="dvDrugMstAuditTrail" BehaviorID="MPE_DrugMstHistory"
        PopupDragHandleControlID="LbldRandomizationDtl" BackgroundCssClass="modalBackground" CancelControlID="imgDrugMstAuditTrail"
        TargetControlID="btn3">
    </cc1:ModalPopupExtender>

    <div id="DivExports" runat="server">

        <asp:GridView runat="server" ID="gvExport" AutoGenerateColumns="true" Style="display: none"
            HeaderStyle-BackColor="#1560a1" HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" Header-style-font-size="10px !importnat" HeaderStyle-Font-Names="Verdana, Arial, Helvetica, sans-serif"
            HeaderStyle-Font-Size=" 0.9em"
            HeaderStyle-HorizontalAlign="Center" CellPadding="3" CellSpacing="0"
            RowStyle-Font-Names=" Verdana, Arial, Helvetica, sans-serif" RowStyle-Font-Size="10pt"
            RowStyle-BackColor="#84c8e6" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#191970" RowStyle-ForeColor="#191970">
        </asp:GridView>
    </div>
    <div id="DivExportToExcel" runat="server">
        <asp:GridView runat="server" ID="gvExportToExcel" AutoGenerateColumns="true" Style="display: none"
            HeaderStyle-BackColor="#1560a1" HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" Header-style-font-size="10px !importnat" HeaderStyle-Font-Names="Verdana, Arial, Helvetica, sans-serif"
            HeaderStyle-Font-Size=" 0.9em"
            HeaderStyle-HorizontalAlign="Center" CellPadding="3" CellSpacing="0"
            RowStyle-Font-Names=" Verdana, Arial, Helvetica, sans-serif" RowStyle-Font-Size="10pt"
            RowStyle-BackColor="#84c8e6" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#191970" RowStyle-ForeColor="#191970">
        </asp:GridView>
    </div>
    <asp:HiddenField runat="server" ID="hdnDrugCode" />
    <asp:Button ID="btnExportToExcel" runat="server" Style="display: none;" />


    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <%--added by ketan--%>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            debugger;
            fnGetDataforDrugMaster();
            return false;
        });
        function HideDrugDetails() {
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

        function AudtiTrail(e) {
            //var vDrugCode = $("#" + e.id).attr("vDrugCode");
            var vDrugCode = e.attributes.tempval1.value;

            if (vDrugCode != "") {
                $.ajax({
                    type: "post",
                    url: "frmDrugMst.aspx/AuditTrail",
                    data: '{"vDrugCode":"' + vDrugCode + '"}',
                    contentType: "application/json; charset=utf-8",
                    datatype: JSON,
                    async: false,
                    success: function (data) {
                        $('#tblDrugMstAudit').attr("IsTable", "has");
                        var aaDataSet = [];
                        var range = null;

                        if (data.d != "" && data.d != null) {
                            data = JSON.parse(data.d);
                            for (var Row = 0; Row < data.length; Row++) {
                                var InDataSet = [];
                                InDataSet.push(data[Row].SrNo, data[Row].DrugName, data[Row].WashOutPeriod, data[Row].Housing, data[Row].DrugSynonyms, data[Row].FoodEffect, data[Row].strength, data[Row].formulation, data[Row].release, data[Row].Remarks, data[Row].ModifyBy, data[Row].ModifyOn);
                                aaDataSet.push(InDataSet);
                            }
                        }
                        if ($("#tblDrugMstAudit").children().length > 0) {
                            $("#tblDrugMstAudit").dataTable().fnDestroy();
                        }
                        oTable = $('#tblDrugMstAudit').prepend($('<thead>').append($('#tblDrugMstAudit tr:first'))).dataTable({

                            "bJQueryUI": true,
                            "sPaginationType": "full_numbers",
                            "bLengthChange": true,
                            "iDisplayLength": 10,
                            "bProcessing": true,
                            "bSort": false,
                            "aaData": aaDataSet,
                            "autowidth": false,
                            aLengthMenu: [
                                [10, 25, 50, 100, -1],
                                [10, 25, 50, 100, "All"]
                            ],
                            "aoColumns": [
                                {
                                    "sTitle": "Sr. No",
                                },
                                { "sTitle": "Drug Name" },
                                   { "sTitle": "Wash Out Period" },
                                   { "sTitle": "Housing" },
                                   { "sTitle": "Drug Synonyms" },
                                   { "sTitle": "Food Effect" },
                                   { "sTitle": "Strength" },
                                   { "sTitle": "Formulation" },
                                   { "sTitle": "Release" },
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
                        $find('MPE_DrugMstHistory').show();
                        $('.dataTables_filter input').addClass('textBox');
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

        function ExportToExcel(e) {
            // $("#<%= hdnDrugCode.ClientID()%>").val(id);
            var id = e.attributes.tempval2.value;

            $('#ctl00_CPHLAMBDA_hdnDrugCode').val(id);
            var btn = document.getElementById('<%= btnExportToExcel.ClientId()%>')
            btn.click();
        }

        function Validation() {
            if (document.getElementById('<%=txtDrugName.ClientID%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=txtDrugName.ClientID%>').value = '';
                msgalert('Please Enter Drug Name !');
                document.getElementById('<%=txtDrugName.ClientID%>').focus();
                return false;
            }
            if (document.getElementById("<%=btnSave.ClientID%>").value.trim() == "Update") {

                if (document.getElementById("<%=txtRemark.ClientID%>").value.trim() == "") {
                    msgalert("Please Enter Remarks !");
                    return false;
                }
            }
            return true;
        }

        function Numeric(type) {

            var ValidChars = "0123456789-";
            var Numeric = true;
            var Char;
            if (type == 'Housing') {


                sText = document.getElementById('<%=TxtHousing.ClientID%>').value;

                for (i = 0; i < sText.length && Numeric == true; i++) {
                    Char = sText.charAt(i);
                    if (ValidChars.indexOf(Char) == -1) {
                        msgalert('Please Enter Numeric Value And You Can Add "-"');
                        document.getElementById('<%=TxtHousing.ClientID%>').value = "";
                        document.getElementById('<%=TxtHousing.ClientID%>').focus();
                        Numeric = false;
                    }

                }
            }
            else if (type == 'Washout') {
                sText = document.getElementById('<%=TxtWashOutPeriod.ClientID%>').value;
                for (i = 0; i < sText.length && Numeric == true; i++) {
                    Char = sText.charAt(i);
                    if (ValidChars.indexOf(Char) == -1) {
                        msgalert('Please Enter Numeric Value And You Can Add "-"');
                        document.getElementById('<%=TxtWashOutPeriod.ClientID%>').value = "";
                        document.getElementById('<%=TxtWashOutPeriod.ClientID%>').focus();
                        Numeric = false;
                    }
                }
            }
    }

    function OnSubSelected(sender, e) {
        var strvalue = e.get_value();
        strvalue = strvalue.replace('\'', '');

        var arrstrvalue = strvalue.split('#');


    }

    function SubClientPopulated(sender, e) {


    }
    function ShowAlert(msg) {
       // alert(msg);
        //window.location.href = "frmDrugMst.aspx?mode=1";

        alertdooperation(msg, 1, "frmDrugMst.aspx?mode=1");
    }



    function gvDrugMst() {

        $('#<%= GV_Drug.ClientID%>').removeAttr('style', 'display:block');
        //$('#<%= GV_Drug.ClientID%>').find('tbody tr').length < 3 ? scroll = "25%" : scroll = "285px";
        oTab = $('#<%= GV_Drug.ClientID%>').prepend($('<thead>').append($('#<%= GV_Drug.ClientID%> tr:first'))).dataTable({
            "bJQueryUI": true,
            "bAutoWidth": false,
            "sPaginationType": "full_numbers",
            "bLengthChange": true,
            "iDisplayLength": 10,
            "bProcessing": true,
            "bSort": false,
            "sScrollY": "250px",
            "sScrollX": "true",
            aLengthMenu: [
                [10, 25, 50, 100, -1],
                [10, 25, 50, 100, "All"]
            ]
        });
        setTimeout(function () { oTab.fnAdjustColumnSizing(); }, 12);
        return false;
    }

        var summarydata = '';
        function fnGetDataforDrugMaster() {

            debugger;
            $('#loadingmessage').show();

            $.ajax({
                type: "post",

                url: "frmDrugMst.aspx/View_Drug",
                //data: '{"vWorkSpaceId":"' + vWorkSpaceId + '","vType":"' + vType + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                // async: false,
                dataType: "json",
                success: function (data) {
                    var data = data.d;
                    var msgs = JSON.parse(data);
                    summarydata = msgs;
                    if (summarydata == "") return false;
                    // Table = Object(keys(summarydata))[0];
                    CreateSummaryTable(summarydata);
                    $('#loadingmessage').hide();
                    return false;
                },
                failure: function (response) {
                    alert("failure");
                    alert(data.d);
                },
                error: function (response) {
                    alert("error");
                }
            });
            return false;

        }
        function CreateSummaryTable(summarydata) {

            var ActivityDataset = [];
            var jsondata = summarydata.DRUGMST;
            for (var Row = 0; Row < jsondata.length; Row++) {
                var InDataset = [];
                InDataset.push(Row + 1, jsondata[Row]['vDrugName'], jsondata[Row]['vWashOutPeriod'], jsondata[Row]['vHousing'], jsondata[Row]['vDrugSynonyms'],
                    jsondata[Row]['vFoodEffect'], jsondata[Row]['vStrength'], jsondata[Row]['vFormulation'], jsondata[Row]['vRelease'], "", "", "", jsondata[Row]['vDrugCode']);
                ActivityDataset.push(InDataset);
            }

            $ = jQuery;
            var createtable1 = $("<table id='Activityrecord'  border='1'  class='display'  cellspacing='0' width='100%'> </table>");
            $("#createtable").empty();
            $("#createtable").append(createtable1);

            $('#Activityrecord').DataTable({
                "bJQueryUI": true,
                "sScrollY": "285px",
                "sScrollX": "100%",
                "bScrollCollapse": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": false,
                "aaData": ActivityDataset,
                "fnCreatedRow": function (nRow, aData, iDataIndex) {
                    $('td:eq(9)', nRow).append("<input type='image' id='imgedit" + iDataIndex + "' name='imgedit$" + iDataIndex + "' src='images/Edit2.gif'; tempval='" + aData[12] + "';   OnClick='Editcellrow(this); return false;' style='border-width:0px;'>");
                    $('td:eq(10)', nRow).append("<input type='image' id='imgaudit" + iDataIndex + "' name='imgaudit$" + iDataIndex + "' src='images/audit.png'; tempval1='" + aData[12] + "';   OnClick='AudtiTrail(this); return false;' style='border-width:0px;' >");
                    $('td:eq(11)', nRow).append("<input type='image' id='imgexel" + iDataIndex + "' name='imgexel$" + iDataIndex + "' src='images/Export.gif';  tempval2='" + aData[12] + "';   OnClick='ExportToExcel(this); return false;' style='border-width:0px;' >");
                },
                
                "aoColumns": [

                                  { "sTitle": "#" },
                                  { "sTitle": "Drug Name" },
                                  { "sTitle": "WashOut Period" },
                                  { "sTitle": "Housing" },
                                  { "sTitle": "Drug Synonyms" },
                                  { "sTitle": "Food Effect" },
                                  { "sTitle": "Strength" },
                                  { "sTitle": "Formulation" },
                                  { "sTitle": "Release" },
                                  { "sTitle": "Edit" },
                                  { "sTitle": "Audit Trail" },
                                  { "sTitle": "Export to Excel" },
                                   //{ "sTitle": "vDrugcode" },
                ],

                "columns": [
                    null, null, null, null, null, null, null, null, null, null, null, null
                ],
                "oLanguage": {
                    "sEmptyTable": "No Record Found"
                },
            });

            $('#Activityrecord').show();

        }
        function Editcellrow(e) {

            var id = e.attributes.tempval.value;

            $('#ctl00_CPHLAMBDA_hdnEditedId').val(id);


            $('#ctl00_CPHLAMBDA_btnEdit').click();

        }




    </script>

</asp:Content>
