<%@ Page Language="VB" MasterPageFile="~/ECTDMasterPage.master" AutoEventWireup="false"
    CodeFile="frmMSRPrint.aspx.vb" Inherits="frmMSRPrint" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <style type="text/css">
        .div { border-top: 6px solid #2372b2; border-left: 6px solid #2372b2; border-bottom: 6px solid #2372b2; border-right: 6px solid #2372b2; -webkit-border-radius: 15px; padding: 4px; overflow: hidden; border-width: 2px; }
        #divTable td {
            text-align:center;
        }

         .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }
        { </style>
    <table id="Headtable" style="width: 100%;" cellpadding="5px" cellspacing="5px">
        <tr>
            <td colspan="2" class="Label">
                <asp:RadioButtonList ID="rblLog" runat="server" Style="margin: auto;" class="RadioButton "
                    RepeatDirection="Horizontal">
                    <asp:ListItem Text="Screening Template Structure" onclick="getViewScreeningLog();"
                        Value="Structure"></asp:ListItem>
                    <asp:ListItem Text="Project-specific Screening Log" Value="Data" onclick="getViewScreeningLog();"></asp:ListItem>
                    <asp:ListItem Text="Generic MSR Log" Value="GenericData" onclick="getViewScreeningLog();"></asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr id="trScreening" style="display: none;">
            <td colspan="2" style="margin: auto; text-align: center">
                <input type="radio" id="rblGenericScr" name="ScreeningType" value="0" class="RadioButton "
                    onclick="GenCallData();" />
                <label for="rblGenericScr" class="Label ">
                    Generic Screening</label>
                <input type="radio" id="rblProjectspScr" name="ScreeningType" value="1" class="RadioButton "
                    onclick="Cleardiv();" />
                <label for="rblProjectspScr" class="Label ">
                    Project-Specific Screening</label>
            </td>
        </tr>
        <tr id="trProject" style="display: none;">
            <td style="text-align: right;">
                <label class="Label ">
                    Project Name* :
                </label>
            </td>
            <td style="text-align: left;">
                <asp:TextBox ID="txtProject" runat="server" CssClass="textBox " Width="65%" ToolTip="Enter Project Number"></asp:TextBox>
                <input type="button" style="display: none" id="btnSetProject" value=" Project" onclick="GenCallData();" />
                <asp:HiddenField ID="HProjectId" runat="server" />
                <asp:HiddenField ID="HClientName" runat="server" />
                <asp:HiddenField ID="HProjectNo" runat="server" />
                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" ServicePath="AutoComplete.asmx"
                    ServiceMethod="GetMyProjectCompletionListForProjectSpScr" TargetControlID="txtProject"
                    OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1"
                    CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                    CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1">
                </cc1:AutoCompleteExtender>
            </td>
        </tr>
        <tr id="trExportToExcel" style="display: none;">
            <td>
                <asp:Button ID="btnExporttoExcel" runat="server"  CssClass="btn btnexcel" ToolTip="Export To Excel"
                    OnClick="btnExportSubjectInfo_Click"></asp:Button>
            </td>
        </tr>
        <tr id="trTable" style="display: none;">
            <td id="tdTable" colspan="2">
                <div id="divTable" style="width: 90%; margin: auto;">
                </div>
            </td>
        </tr>
        <table width="100%">
            <tr id="trpdf" style="display: none;">
                <td colspan="2" style="margin: auto; text-align: center">
                    <asp:Button ID="btnGeneratePDF" ToolTip="Generate Pdf" OnClientClick="copyData();"
                        runat="server" CssClass="btn btnpdf"/>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <img id="ImgLogo" runat="server" src="~/images/lambda_logo.jpg" alt="Lambda Logo"
                        style="display: none;" />
                </td>
            </tr>
        </table>
        <tr id="trMaindiv" style="display: none;">
            <td style="text-align: left;" colspan="2">
                <div style="width: 88%; margin: auto;">
                    <div id="mainDiv" class="div" runat="server" style="max-height: 600px; margin-bottom: 17px;">
                    </div>
                </div>
            </td>
        </tr>
    </table>
    
    <asp:HiddenField ID="hdn" runat="server" />
    <asp:HiddenField ID="hfHeaderText" runat="server" />
    <asp:HiddenField ID="hfWaterMark" runat="server" />

    <script src="Script/jquery-1.7.2.min.js" type="text/javascript"></script>
    <%--<script src="Script/jquery-1.4.3.min.js" type="text/javascript"></script>--%>
    <script src="Script/slimScroll.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <%--<script type="text/javascript" src="Script/jquery-ui-1.8.20.custom.min.js"></script>--%>
    <script src="Script/jquery.dataTables.min.js" type="text/javascript"></script>
    <script src="Script/TableTools.min.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        var ProjectNo = '';
        var SubjectIntials = '';
        var SubjectId = '';
        var ClientName = '';
        var MySubjectNo = '';
        var gender = '';
        var screendate='';
        $(document).ready(function () {
        });
        //For Select/Deselect All
        $('#chk_header').live("click", function () {
            if (this.checked == true) { $('#table input[type="checkbox"]').each(function () { this.checked = true; }); }
            else { $('#table input[type="checkbox"]').each(function () { this.checked = false; }); }
        });

        $('.checkbox').live("click", function () { if (this.checked == false) { $('#chk_header').checked = false; } });

        function pageLoad(sender, e) { $find('AutoCompleteExtender1').set_contextKey('iUserId = <%= Session(S_UserID).ToString() %>'); }
        function ClientPopulated(sender, e) {
            ProjectClientShowingSchema('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }
        function GenCallData() {

            ProjectNo = document.getElementById('<%=HProjectNo.clientid %>').value;
            ClientName = document.getElementById('<%=HClientName.clientid %>').value;
            if ($('.RadioButton input:radio:checked').val() == "Structure") {
                document.getElementById('<%=hfWaterMark.ClientId %>').value = "0";
                GenCallShowUI();
            }
            else {

                document.getElementById('<%=hfWaterMark.ClientId %>').value = "1";
                document.getElementById('trpdf').style.display = 'none';
                document.getElementById('trTable').style.display = ''
                getScreeningLog();
            }
        }

        function OnSelected(sender, e) {

            document.getElementById('trMaindiv').style.display = '';
            document.getElementById('trpdf').style.display = '';
           document.getElementById('trExportToExcel').style.display='';
            document.getElementById('trProject').style.display = '';
            document.getElementById('trScreening').style.display = '';
            if (document.getElementById('rblGenericScr').checked == false && document.getElementById('rblProjectspScr').checked == false) {
                document.getElementById('trScreening').style.display = 'none';
                document.getElementById('trMaindiv').style.display = 'none';
                
                
            }

            ProjectOnItemSelectedForMsrLog(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), $("#btnSetProject"), $get('<%=HClientName.clientid %>'), $get('<%=HProjectNo.clientid %>'));



            return false;
        }
        $(".div").mouseover(function () {
            $(this).slimScroll({

                height: '660px',
                size: '10px',
                color: '#0f70bb',
                alwaysVisible: false
            });
        });
        function GenCallShowUI() {
            if ($('.RadioButton input:radio:checked').val() == "Structure") {
                var JsonText = "";
                document.getElementById('<%=mainDiv.ClientId %>').innerHTML = "";
                if (document.getElementById("rblGenericScr").checked == true) {
                    var obj = new Object();
                    obj.query = " select vMedExCode,vMedExDesc,vMedExGroupCode,vMedExGroupDesc,vMedExSubGroupCode,vMedExSubGroupDesc,vMedExType,vUOM,vMedExValues,vDefaultValue " +
                    " from View_GeneralScreeningHdrDtl where vWorkspaceid='0000000000' And cActiveFlag <> 'N' And cStatusindi <> 'D' And vLocationCode like'%<%= Session(S_LocationCode).ToString() %>%'" +
                    " And vMedexType<>'Import' AND GETDATE() BETWEEN dFromDate AND ISNULL(dToDate,GETDATE()) " +
                    " order by iSeqno,vMedExGroupCode"
                    JsonText = JSON.stringify(obj);
                    document.getElementById('trMaindiv').style.display = '';
                    document.getElementById('trpdf').style.display = '';
                    document.getElementById('trProject').style.display = 'none';
                }
                else if (document.getElementById("rblProjectspScr").checked == true) {
                    document.getElementById('trProject').style.display = '';
                   
                    document.getElementById('trMaindiv').style.display = 'none';
                    if (document.getElementById('<%= txtProject.clientid %>').value == "") {
                        msgalert("Please Enter Project !");
                        return;
                    }
                    document.getElementById('trMaindiv').style.display = '';
                    
                    var obj = new Object();
                    obj.query = " select vMedExCode,vMedExDesc,vMedExGroupCode,vMedExGroupDesc,vMedExSubGroupCode,vMedExSubGroupDesc,vMedExType,vUOM,vMedExValues,vDefaultValue " +
                    " from View_WorkspaceScreeningHdrDtl where vWorkspaceid='" + $get('<%= HProjectId.clientid %>').value + "' And cActiveFlag <> 'N' And cStatusindi <> 'D'" +
                    " And vMedexType<>'Import'" +
                    " order by iSeqno,vMedExGroupCode"
                    JsonText = JSON.stringify(obj);

                }

            $.ajax(
                    {
                        type: "POST",
                        url: "WS_HelpDB_JSON.asmx/GetDataTableObjectBySqlQuery",
                        data: JsonText,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            var Template = $.parseJSON(response.d);
                            createDivforPrint(Template, 'div');

                        },
                        failure: function (error) {
                            msgalert(error);
                        }
                    });

        }


    }
    function getScreeningLog() {



        //            Added by dipen Shah On 15-May-2014 for Use one datatable for two function
        if ($('.RadioButton input:radio:checked').val() == "Data") {
            $('#table').remove();
            $('#divTable').html('<table id="table" class="table"></table>');

            var oTable = "";
            var obj = new Object();

            obj.query = " select nMedExScreeningHdrNo,vSubjectId,dScreenDate,vWorkSpaceId,cIsEligible,vInitials,cSex,TemplateID from View_ProjectSpecificScreeningLog " +
                " where vWorkspaceid='" + $get('<%= HProjectId.clientid %>').value + "'"
            var JsonText = JSON.stringify(obj);
            $.ajax(
                    {
                        type: "POST",
                        url: "WS_HelpDB_JSON.asmx/GetDataTableObjectBySqlQuery",
                        data: JsonText,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            var Grid = $.parseJSON(response.d);
                            var dtData = new Array();
                            $.each(Grid, function (index) {
                                dtData.push(
                                [
                              this.nMedExScreeningHdrNo,
                              this.vSubjectId,
                              this.vInitials,
                              this.dScreenDate,
                              this.cIsEligible,
                              this.cSex


                                ]);
                            });

                            //For deciding Columns
                            var HeaderText = "Select,SubjectId,Initials,ScreenDate,IsEligible";
                            var DataField = "nMedExScreeningHdrNo,vSubjectId,dScreenDate,vWorkSpaceId,cIsEligible,vInitials,cSex";
                            var Width = "5,10,20,5,7,10";
                            var TemplateField = "checkbox";
                            var PrimaryKey = "TemplateID";
                            var NotDisplayColumn = "0";
                            var Column = new Array();
                            Column = createColumn(HeaderText, DataField, Width);
                            //CreateGrid(Grid,Column,'table',NotDisplayColumn,TemplateField,PrimaryKey) ;
                            oTable = $('#table').dataTable
                            ({

                                "sPaginationType": "scrolling",
                                "bStateSave": false,
                                "bPaginate": true,                   // To disable pagination.
                                "bJQueryUI": false,
                                "sScrollX": "100%",
                                "bDestroy": true,
                                "bRetrive": true,                       // For applying Horizontal scroll in grid.
                                "sScrollXInner": "100%",                // Setting size of scroll that how much it is bigger.
                                "bFilter": true,
                                "sPaginationType": "full_numbers",      // For displaying next, prev, first, last in pagination.
                                "bSort": true,
                                "iDisplayLength": 10,

                                'aaData': dtData,
                                "aoColumns": Column,
                                "aoColumnDefs":
                                [
                                {
                                    "fnRender":
                                      function (oObj) {
                                          
                                          return '<input type="radio" onclick="SubjectData(this)" name="radioButton"/><input type="hidden"ID="hfSubjectID" Value="' + oObj.aData[0] + "#" + oObj.aData[1] + "#" + oObj.aData[2] + "#" + oObj.aData[5] + '" />';

                                      },
                                    "aTargets": [0]
                                }
                                ],
                                "bSortClasses": false


                            });



                        },
                        failure: function (error) {
                            msgalert(error);
                        }
                    });

        }

        //            Added by Dipen Shah On 14-May-2014 For GENERIC SCREENING MSR LOG 

        if ($('.RadioButton input:radio:checked').val() == "GenericData") {

            $('#table').remove();
            $('#divTable').html('<table id="table" class="table"></table>');

            var oTable = "";
            var obj = new Object();
            obj.query = " select nMedExScreeningHdrNo,vSubjectId,vMySubjectNo,dScreenDate,vWorkSpaceId,cIsEligible,vInitials,TemplateID,cSex from  View_GenericScreeningMSRLog " +
                " where vWorkspaceid='" + $get('<%= HProjectId.clientid %>').value + "'"
            
              
                        var JsonText = JSON.stringify(obj);
                        $.ajax(
                               {
                                   type: "POST",
                                   url: "WS_HelpDB_JSON.asmx/GetDataTableObjectBySqlQuery",
                                   data: JsonText,
                                   contentType: "application/json; charset=utf-8",
                                   dataType: "json",
                                   success: function (response) {
                                     var Grid = $.parseJSON(response.d);
                                       var dtData = new Array();

                                       $.each(Grid, function (index) {
                                           dtData.push(
                                           [
                                         this.nMedExScreeningHdrNo,
                                         this.vSubjectId,
                                         this.vMySubjectNo,
                                         this.vInitials,
                                         this.dScreenDate,
                                         this.cIsEligible,
                                         this.cSex,


                                           ]);
                                       });

                                       //For deciding Columns
                                       var HeaderText = "Select,SubjectId,MySubjectNo,Initials,ScreenDate,IsEligible";
                                       var DataField = "nMedExScreeningHdrNo,vSubjectId,vMySubjectNo,dScreenDate,vWorkSpaceId,cIsEligible,vInitials,cSex";
                                       var Width = "5,10,5,20,5,7,10";
                                       var TemplateField = "checkbox";
                                       var PrimaryKey = "TemplateID";
                                       var NotDisplayColumn = "0";
                                       var Column = new Array();
                                       Column = createColumn(HeaderText, DataField, Width);
                                       //CreateGrid(Grid,Column,'table',NotDisplayColumn,TemplateField,PrimaryKey) ;
                                       oTable = $('#table').dataTable
                                       ({

                                           "sPaginationType": "scrolling",
                                           "bStateSave": false,
                                           "bPaginate": true,                   // To disable pagination.
                                           "bJQueryUI": false,
                                           "sScrollX": "100%",
                                           "bDestroy": true,
                                           "bRetrive": true,                       // For applying Horizontal scroll in grid.
                                           "sScrollXInner": "100%",                // Setting size of scroll that how much it is bigger.
                                           "bFilter": true,
                                           "sPaginationType": "full_numbers",      // For displaying next, prev, first, last in pagination.
                                           "bSort": true,
                                           "iDisplayLength": 10,
//                                           "sDom": 'T<"clear">lfrtip',
//                                           "oTableTools": {
//                                               "aButtons": [

//                                                           {
//                                                               "sExtends": "xls",
//                                                               "sButtonText": "Export To Excel",
//                                                               "bFooter": true
//                                                           }
//                                             ],
//                                       "sSwfPath": "Script/swf/copy_cvs_xls_pdf.swf"
//                                           },
                                           'aaData': dtData,
                                           "aoColumns": Column,
                                           "aoColumnDefs":
                                           [
                                           {
                                               "fnRender":
                                                 function (oObj) {
                                                    
                                                     return '<input type="radio" onclick="SubjectData(this)" name="radioButton"/><input type="hidden"ID="hfSubjectID" Value="' + oObj.aData[0] + "#" + oObj.aData[1] + "#" + oObj.aData[2] + "#" + oObj.aData[3] + "#" + oObj.aData[6] + "#" + oObj.aData[4] + "#" + oObj.aData[5] +'" />';
                                                  
                                                 },
                                               "aTargets": [0]
                                           }
                                           ],
                                           "bSortClasses": false
                                       });
                                   },
                                   failure: function (error) {
                                       msgalert(error);
                                   }
                               });
                    }
                }
                function createColumn(HeaderText, DataField, Width) {

                    var Column = new Array();
                    for (var i = 0; i < HeaderText.split(",").length; i++) {
                        //                var obj=new Object();
                        //                obj.HeaderText=HeaderText.split(",")[i].toString(); 
                        //                obj.DataField=DataField.split(",")[i].toString(); 
                        //                Column.push(obj);  
                        var obj = new Object();
                        obj.sTitle = HeaderText.split(",")[i].toString();
                        obj.sWidth = Width.split(",")[i].toString() + "%";
                        Column.push(obj);
                    }
                    return Column;
                }
                function CreateGrid(Grid, Column, className, NotDisplayColumn, TemplateField, PrimaryKey) {


                    var i = 0, j = 0, k = 0;
                    var str = "", ID = "";
                    str = '<table><tr><th class="FixedHeader_Header">#</th><th class="FixedHeader_Header"><input type=' + TemplateField + ' id="chk_header" class="checkbox"/>Select ALL</th>';
                    for (i = 0; i < Column.length; i++) {
                        str += '<th class="FixedHeader_Header"';
                        for (k = 0; k < NotDisplayColumn.split(",").length; k++) {
                            if (i == NotDisplayColumn.split(",")[k].toString()) {
                                str += ' style="display: none;"';
                            }
                        }
                        str += ' >' + Column[i].HeaderText + '</th>'
                    }
                    str += '</tr>';
                    $("." + className).append(str.trim());
                    for (i = 0; i < Grid.length; i++) {
                        str = '<tr><td>' + (i + 1) + '</td><td><input type=' + TemplateField + ' id="chk_' + Grid[i][PrimaryKey] + '" class="checkbox"/> </td>';
                        for (j = 0; j < Column.length; j++) {
                            str += '<td';
                            for (k = 0; k < NotDisplayColumn.split(",").length; k++) {
                                if (j == NotDisplayColumn.split(",")[k].toString()) {
                                    str += ' style="display: none;"';
                                }
                            }
                            str += ' >' + Grid[i][Column[j].DataField] + '</td>';

                        }
                        str += '</tr>';
                        $("." + className).append(str.trim());
                    }
                    $("." + className).append('</table>');

                }
                function createDivforPrint(Template, div)  // for MSR Structure Print Or Project Specific Structure
                {


                    var j = 0, i = 0, k = 0, t = 0;
                    var str = "";
                    var res = "";
                    var arrMedexGroup = new Array();
                    var arrMedexSubGroup = new Array();

                    //for check MedexGroupcode 
                    for (i = 0; i < parseInt(Template.length) ; i++) {
                        if (i == 0) {
                            arrMedexGroup[j] = Template[i].vMedExGroupCode + "##" + Template[i].vMedExGroupDesc;
                            j++;
                        }
                        else {
                            for (k = 0; k < arrMedexGroup.length; k++) {
                                if (arrMedexGroup[k].split("##")[0] != Template[i].vMedExGroupCode) {
                                    res = true;

                                }
                                else {
                                    res = false
                                    break;
                                }

                            }
                            if (res == true) {
                                arrMedexGroup[j] = Template[i].vMedExGroupCode + "##" + Template[i].vMedExGroupDesc;
                                j++;
                                res = "";
                            }
                        }
                    }
                    //for MedexSubgroupcode
                    j = 0;
                    for (i = 0; i < parseInt(Template.length) ; i++) {
                        if (i == 0) {
                            arrMedexSubGroup[j] = Template[i].vMedExGroupCode + "++" + Template[i].vMedExSubGroupCode + "##" + Template[i].vMedExSubGroupDesc;
                            j++;
                        }
                        else {
                            for (t = 0; t < arrMedexGroup.length; t++) {
                                if (arrMedexGroup[t].split("##")[0] == Template[i].vMedExGroupCode) {
                                    for (k = 0; k < arrMedexSubGroup.length; k++) {

                                        if (arrMedexSubGroup[k].split("++")[1].split("##")[0] != Template[i].vMedExSubGroupCode || arrMedexSubGroup[k].split("++")[0] != Template[i].vMedExGroupCode) {
                                            res = true;

                                        }
                                        else {
                                            res = false
                                            break;
                                        }

                                    }
                                }
                            }
                            if (res == true) {
                                arrMedexSubGroup[j] = Template[i].vMedExGroupCode + "++" + Template[i].vMedExSubGroupCode + "##" + Template[i].vMedExSubGroupDesc;
                                j++;
                                res = "";
                            }
                        }
                    }
                    //For Display Medex
                    var tempStr = "";


                    tempStr += '<table width="100%" style=" border-style:solid;border-color:black;border-width:1px;" cellspacing="0"><tr><td>'
                    for (i = 0; i < arrMedexGroup.length; i++) {
                        tempStr += '<table width="100%" style=" border-collapse:collapse;" cellspacing="0"><tr><th style="BACKGROUND-COLOR: #008ecd; font-weight: bold; color:white;page-break-inside:avoid;" colspan="2" text-align: center;" >' + arrMedexGroup[i].split("##")[1].toString() + '</th></tr>';

                        for (j = 0; j < arrMedexSubGroup.length; j++) {
                            if (arrMedexGroup[i].split("##")[0] == arrMedexSubGroup[j].split("++")[0]) {
                                if (arrMedexSubGroup[j].split("##")[1].toString() == 'Default') { }
                                else {
                                    tempStr += '<tr><td colspan="2"style="BACKGROUND-COLOR: #ffcc66; font-weight: bold;">' + arrMedexSubGroup[j].split("##")[1].toString() + '</td></tr>';
                                }
                                for (k = 0; k < Template.length; k++) {
                                    if (arrMedexGroup[i].split("##")[0] == Template[k].vMedExGroupCode) {
                                        if (arrMedexSubGroup[j].split("++")[1].split("##")[0] == Template[k].vMedExSubGroupCode) {
                                            if (Template[k].vMedExType == "Label") {
                                                tempStr += '<tr><td style="text-align: left;width:30%; border-bottom: 1px solid black;" colspan="2">' + createObject(Template[k].vMedExType, Template[k].vMedExValues, Template[k].vDefaultValue, Template[k].vMedExCode) + " " + Template[k].vUOM + '</td></tr>';
                                            }
                                            else {
                                                tempStr += '<tr><td style="text-align: left;width:70%; border-bottom: 1px solid black;" class="Label ">' + Template[k].vMedExDesc + ' :</td><td style="text-align: left;width:30%; border-bottom: 1px solid black;">' + createObject(Template[k].vMedExType, Template[k].vMedExValues, Template[k].vDefaultValue, Template[k].vMedExCode) + " " + Template[k].vUOM + '</td></tr>';
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                    tempStr += '</table></td></tr></table>';
                    $("." + div).append(tempStr.toString().trim());
                }

                function createObject(type, MedexValues, MedexDefaultValues, Medexcode) {


                    var str = "";
                    var temp_MedexValues = MedexValues;
                    var i = 0;
                    if (type.toUpperCase() == "TEXTBOX" || type.toUpperCase() == "STANDARDDATE") {
                        str = "<input type='TEXTBOX'  id='txt_" + Medexcode + "' value='" + MedexDefaultValues + "' class='TextBox ' disabled='true'/>"
                        return str;
                    }
                    if (type.toUpperCase() == "CHECKBOX") {
                        for (i = 0; i < temp_MedexValues.split(",").length; i++) {
                            str += "<input type='CHECKBOX'  id='chk_" + Medexcode + "_" + i + "'"
                            for (j = 0; j < MedexDefaultValues.split(",").length; j++) {
                                if (temp_MedexValues.split(",")[i].toString() == MedexDefaultValues.split(",")[j].toString()) {
                                    str += " checked='checked'"
                                }
                            }
                            str += " class='checkBoxlist ' disabled='true'/>"
                            str += "<label for='chk_" + Medexcode + "_" + i + "'>" + temp_MedexValues.split(",")[i].toString() + "</label>"
                        }
                        return str;
                    }
                    if (type.toUpperCase() == "RADIO") {
                        for (i = 0; i < temp_MedexValues.split(",").length; i++) {
                            str += "<input type='RADIO'  id='rad_" + Medexcode + "_" + i.toString() + "' name='rad_" + Medexcode + "'"
                            if (temp_MedexValues.split(",")[i].toString() == MedexDefaultValues) {
                                str += " checked=checked"
                            }
                            str += " class='RadioButton ' disabled='true'/>"
                            str += "<label for='rad_" + Medexcode + "_" + i.toString() + "' class='Label '>" + temp_MedexValues.split(",")[i].toString() + "</label>"
                        }
                        return str;
                    }
                    if (type.toUpperCase() == "COMBOBOX") {

                        str = "<select class ='dropDownList' id='cmb_" + Medexcode + "' disabled='true'>";
                        for (i = 0; i < temp_MedexValues.split(",").length; i++) {
                            str += "<option value='" + i + "_" + Medexcode + "'"
                            if (temp_MedexValues.split(",")[i].toString() == MedexDefaultValues) {
                                str += " selected=selected"
                            }
                            str += ">" + temp_MedexValues.split(",")[i].toString()
                            str += " </option>"
                        }
                        str += "</select>";
                        return str;
                    }
                    if (type.toUpperCase() == "FILE") {
                        str = "<input type='FILE'  id='file_" + Medexcode + "' disabled='true'/>"
                        return str;
                    }
                    if (type.toUpperCase() == "DATETIME" || type.toUpperCase() == "TIME") {
                        str = "<input type='TEXTBOX'  id='txt_" + Medexcode + "'   class='TextBox ' disabled='true'/>"
                        return str;
                    }
                    if (type.toUpperCase() == "TEXTAREA") {
                        str = "<textarea id='txt_" + Medexcode + "' class='TextArea ' disabled='true' style='width:98% ; height:50px;'> " + MedexDefaultValues + "</textarea>"
                        return str;
                    }
                    if (type.toUpperCase() == 'LABEL') {
                        str = "<label id='lbl_" + Medexcode + "' class='Label '  Width='98%'>" + MedexValues + "</label>"
                        return str;
                    }
                    if (type.toUpperCase() == "FORMULA") {
                        str = "<input type='TEXTBOX'  id='txt_" + Medexcode + "' value='" + MedexDefaultValues + "' class='TextBox ' disabled='true'/>"
                        return str;
                    }
                }
                function copyData() {



                    var str = "";

                    if ($('.RadioButton input:radio:checked').val() == "Structure") {

                        if (document.getElementById('rblGenericScr').checked == true) {

                            str += "<table style='border: solid 5 #008ecd; width: 100%;'>";
                            str += "<tr>";
                            str += "<td align='center' style='font-size: larger; text-align: center; font-weight: bolder; width: 80%;'>";
                            str += "Medical Screening Record Form";
                            str += "</td>";
                            str += "<td style='width: 20%;'>";
                            str += "<img id='ImgLogo1' src='~/images/lambda_logo.jpg' alt='Lambda Logo' />";
                            str += "</td>";
                            str += "</tr>";
                            str += "</table>";
                            document.getElementById('<%=hfHeaderText.ClientID %>').value = str.toString().trim();
                        }
                        else {

                            str += "<table style='border: solid 5 #008ecd; width: 100%;'>";
                            str += "<tr>";
                            str += "<td style='width: 70%;'>";
                            str += "<table style='width: 100%;'>";
                            str += "<tr>";
                            str += "<td>";
                            str += "<td colspan='2' align='center' style='font-size: larger; text-align: center; font-weight: bolder;'>";
                            str += "Medical Screening Record Form";
                            str += "</td>";
                            str += "</tr>";
                            str += "<tr>";
                            str += "<td style='text-align: right; width: 8%;' align='left'>";
                            str += "Sponsor Name:";
                            str += "</td>";
                            str += "<td style='border: thin solid #000000; text-align: left; width: 50%;'>";
                            str += "<label id='lblProjectNo'>";
                            str += ClientName.toString() + "</label>";
                            str += "</td>";
                            str += "</tr>";
                            str += "<tr>";
                            str += "<td style='text-align: right; width: 8%;' align='left'>";
                            str += "Project No:";
                            str += "</td>";
                            str += "<td style='border: thin solid #000000; text-align: left; width: 50%;'>";
                            str += " <label id='lblProjectNo'>";
                            str += ProjectNo.toString() + "</label>";
                            str += "</td>";
                            str += "</tr>";
                            str += "<tr>";
                            str += "<td style='text-align: right; width: 8%;' align='left'>";
                            str += "ScreenDate:";
                            str += "</td>";
                            str += "<td style='border: thin solid #000000; text-align: left; width: 50%;'>";
                            str += " <label id='lblScreenDate'>";
                            str += screendate.toString() + "</label>";
                            str += "</td>";
                            str += "</tr>";
                            str += "</table>";
                            str += "</td>";
                            str += "<td style='width: 20%;'>";
                            str += "<img id='ImgLogo1' src='~/images/lambda_logo.jpg' alt='Lambda Logo' />";
                            str += "</td>";
                            str += "</tr>";
                            str += "</table>";

                            document.getElementById('<%=hfHeaderText.ClientID %>').value = str.toString().trim();
                            }
                        }
                        else {

                            str += "<table style='border: solid 5 #008ecd; width: 100%;'>";
                            str += "<tr>";
                            str += "<td  style='width: 80%;'>";
                            str += "<table>";
                            str += "<tr>";
                            str += "<td colspan='2' align='center' style='font-size: larger; text-align: center; font-weight: bolder;'>";
                            str += "Medical Screening Record Form";
                            str += "</td>";
                            str += "</tr>";
                            str += "<tr>";
                            str += "<td style='text-align: right; width: 8%;' align='left'>";
                            str += "Sponsor Name:";
                            str += "</td>";
                            str += "<td style='border: thin solid #000000; text-align: left; width: 50%;'>";
                            str += "<label id='lblProjectNo'>";
                            str += ClientName.toString() + "</label>";
                            str += "</td>";
                            str += "</tr>";
                            str += "<tr>";
                            str += "<td style='text-align: right; width: 8%;' align='left'>";
                            str += "Project No:";
                            str += "</td>";
                            str += "<td style='border: thin solid #000000; text-align: left; width: 50%;'>";
                            str += "<label id='lblProjectNo'>";
                            str += ProjectNo.toString() + "</label>";
                            str += "</td>";
                            str += "</tr>";

                            str += "<tr>";
                            str += "<td style='text-align: right; width: 8%;' align='left'>";
                            str += "Subject Id:";
                            str += "</td>";
                            str += "<td style='border: thin solid #000000; text-align: left; width: 50%;'>";
                            str += "<label id='subjectId'>";
                            str += SubjectId.toString() + "</label>";
                            str += "</td>";
                            str += "</tr>";
                            str += "<tr>";
                            str += "<td style='text-align: right; width: 8%;' align='left'>";
                            str += "Initials :";
                            str += "</td>";
                            str += "<td style='border: thin solid #000000; text-align: left; width: 50%;'>";
                            str += "<label id='lblProjectNo'>";
                            str += SubjectIntials.toString() + "</label>";
                            str += "</td>";
                            str += "</tr>";
                            str += "<tr>";
                            str += "<td style='text-align: right; width: 8%;' align='left'>";
                            str += "ScreenDate:";
                            str += "</td>";
                            str += "<td style='border: thin solid #000000; text-align: left; width: 50%;'>";
                            str += " <label id='lblScreenDate'>";
                            str += screendate.toString() + "</label>";
                            str += "</td>";
                            str += "</tr>";
                            str += "</table>";
                            str += "</td>";
                            str += "<td style='width: 20%;'>";
                            str += "<img id='ImgLogo1' src='~/images/lambda_logo.jpg' alt='Lambda Logo' />";
                            str += "</td>";
                            str += "</tr>";
                            str += "</table>";
                            document.getElementById('<%=hfHeaderText.ClientID %>').value = str.toString().trim();
            }
            document.getElementById('<%=hdn.ClientId %>').value = document.getElementById('<%=mainDiv.ClientId %>').innerHTML;
                }
                function getViewScreeningLog() {
                    document.getElementById('<%=mainDiv.ClientId %>').innerHTML = '';
                        document.getElementById('rblGenericScr').checked = false;
                        document.getElementById('rblProjectspScr').checked = false;
                        document.getElementById('<%=txtProject.ClientId %>').value = '';

                        document.getElementById('trMaindiv').style.display = 'none';
                        document.getElementById('trProject').style.display = 'none';
                        document.getElementById('trTable').style.display = 'none';
                        document.getElementById('trExportToExcel').style.display='none';

                        document.getElementById('trpdf').style.display = 'none';
                        document.getElementById('trScreening').style.display = 'none';

                        if ($('.RadioButton input:radio:checked').val() == "Structure") {
                            document.getElementById('trScreening').style.display = '';
                        }
                        if ($('.RadioButton input:radio:checked').val() == "Data") {

                            document.getElementById('trProject').style.display = '';
                            document.getElementById('trScreening').style.display = 'none';
                        }
                        //            Added By : Dipen Shah for Generic Screening MSR LOG ON 14-MAY-2014 
                        if ($('.RadioButton input:radio:checked').val() == "GenericData") {
                            document.getElementById('trProject').style.display = '';
                            document.getElementById('trScreening').style.display = 'none';
                        }
                    }
                    function Cleardiv() {
                        document.getElementById('trProject').style.display = '';
                        document.getElementById('<%=mainDiv.ClientID %>').innerHTML = "";
            document.getElementById('<%=txtProject.ClientID%>').value = '';
            document.getElementById('trMaindiv').style.display = 'none';
            document.getElementById('trTable').style.display = 'none';
             document.getElementById('trExportToExcel').style.display='none';

        }
        function SubjectData(e) {
            if ($('.RadioButton input:radio:checked').val() == "Data") {
                $row = $(e).parents('tr:first');
                var ScrHdrId = $row[0].cells[0].children[1].value.split("#")[0];
                var str = $row[0].cells[0].children[1].value.split("#")[1];
                SubjectId = $row[0].cells[0].children[1].value.split("#")[1];
                SubjectIntials = $row[0].cells[0].children[1].value.split("#")[2];
                gender = $row[0].cells[0].children[1].value.split("#")[3];//added By dipen Shah

                document.getElementById('<%=mainDiv.ClientId %>').innerHTML = "";
                 var obj = new Object();
                 if (gender == 'M') {
                    obj.query = " SELECT vMedExCode,vMedExDesc,vMedExGroupCode,vMedExGroupDesc,vMedExSubGroupCode,vMedExSubGroupDesc,vMedExType,vUOM,vMedExValues,vDefaultValue,vMedExResult " +
                    " FROM  View_ProjectSpecificScreeningLog_Data  WHERE vWorkspaceid='" + $get('<%= HProjectId.clientid %>').value + "' And vSubjectId='" + str.toString() + "'AND nMedExScreeningHdrNo = '" + ScrHdrId + "' And cStatusindi <> 'D'" +
                   " AND vMedexType<>'Import'" +
                   " ORDER BY ScreeningType,iSeqno,vMedExGroupCode"
                }
                else {

                    obj.query = " SELECT vMedExCode,vMedExDesc,vMedExGroupCode,vMedExGroupDesc,vMedExSubGroupCode,vMedExSubGroupDesc,vMedExType,vUOM,vMedExValues,vDefaultValue,vMedExResult " +
                  " FROM  View_ProjectSpecificScreeningLog_Data  WHERE vWorkspaceid='" + $get('<%= HProjectId.clientid %>').value + "' And vSubjectId='" + str.toString() + "'AND nMedExScreeningHdrNo = '" + ScrHdrId + "' And cStatusindi <> 'D'" +
                   " AND vMedexType<>'Import'" +
                   " ORDER BY ScreeningType,iSeqno,vMedExGroupCode"
                }
                JsonText = JSON.stringify(obj);
                $.ajax(
                    {
                        type: "POST",
                        url: "WS_HelpDB_JSON.asmx/GetDataTableObjectBySqlQuery",
                        data: JsonText,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            var Template = $.parseJSON(response.d);
                            createDivforPrintForData(Template, 'div')
                        },
                        failure: function (error) {
                            msgalert(error);
                        }
                    });

                document.getElementById('trMaindiv').style.display = '';
                document.getElementById('trpdf').style.display = '';
            }
            if ($('.RadioButton input:radio:checked').val() == "GenericData") {

                $row = $(e).parents('tr:first');
                var ScrHdrId = $row[0].cells[0].children[1].value.split("#")[0];
                var str = $row[0].cells[0].children[1].value.split("#")[1];
                SubjectId = $row[0].cells[0].children[1].value.split("#")[1];
                MySubjectNo = $row[0].cells[0].children[1].value.split("#")[2];
                SubjectIntials = $row[0].cells[0].children[1].value.split("#")[3];
                gender = $row[0].cells[0].children[1].value.split("#")[4];
                screendate=$row[0].cells[0].children[1].value.split("#")[5];
                document.getElementById('<%=mainDiv.ClientId %>').innerHTML = "";
            var obj = new Object();
            if (gender == 'M') {
                var no = '00037'
                obj.query = " SELECT vMedExCode,vMedExDesc,vMedExGroupCode,vMedExGroupDesc,vMedExSubGroupCode,vMedExSubGroupDesc,vMedExType,vUOM,vMedExValues,vDefaultValue,vMedExResult " +
                " FROM  View_GenericScreeningMSRLog_Data  WHERE vWorkspaceid='" + $get('<%= HProjectId.clientid %>').value + "'And  vMedExGroupCode <>'" + no + "' And vSubjectId='" + str.toString() + "'AND nMedExScreeningHdrNo = '" + ScrHdrId + "' And cStatusindi <> 'D'" +
                     " AND vMedexType<>'Import'" +
                     " ORDER BY ScreeningType,iSeqno,vMedExGroupCode"
               }
               else {
                   obj.query = " SELECT vMedExCode,vMedExDesc,vMedExGroupCode,vMedExGroupDesc,vMedExSubGroupCode,vMedExSubGroupDesc,vMedExType,vUOM,vMedExValues,vDefaultValue,vMedExResult " +
                  " FROM  View_GenericScreeningMSRLog_Data  WHERE vWorkspaceid='" + $get('<%= HProjectId.clientid %>').value + "' And vSubjectId='" + str.toString() + "'AND nMedExScreeningHdrNo = '" + ScrHdrId + "' And cStatusindi <> 'D'" +
                     " AND vMedexType<>'Import'" +
                     " ORDER BY ScreeningType,iSeqno,vMedExGroupCode"
               }
               JsonText = JSON.stringify(obj);
               $.ajax(
                   {
                       type: "POST",
                       url: "WS_HelpDB_JSON.asmx/GetDataTableObjectBySqlQuery",
                       data: JsonText,
                       contentType: "application/json; charset=utf-8",
                       dataType: "json",
                       success: function (response) {
                           var Template = $.parseJSON(response.d);
                           createDivforPrintForData(Template, 'div')
                       },
                       failure: function (error) {
                           msgalert(error);
                       }
                   });
               document.getElementById('trMaindiv').style.display = '';
               document.getElementById('trpdf').style.display = '';
           }
       }
       function createDivforPrintForData(Template, div)  // for MSR Structure Print Or Project Specific Structure
       {
           var j = 0, i = 0, k = 0, t = 0;
           var str = "";
           var res = "";
           var arrMedexGroup = new Array();
           var arrMedexSubGroup = new Array();

           //for check MedexGroupcode 
           for (i = 0; i < parseInt(Template.length) ; i++) {


               if (i == 0) {
                   arrMedexGroup[j] = Template[i].vMedExGroupCode + "##" + Template[i].vMedExGroupDesc;

                   j++;
               }
               else {

                   for (k = 0; k < arrMedexGroup.length; k++) {
                       if (arrMedexGroup[k].split("##")[0] != Template[i].vMedExGroupCode) {
                           res = true;

                       }
                       else {
                           res = false
                           break;
                       }

                   }
                   if (res == true) {
                       arrMedexGroup[j] = Template[i].vMedExGroupCode + "##" + Template[i].vMedExGroupDesc;
                       j++;
                       res = "";
                   }
               }
           }
           //for MedexSubgroupcode
           j = 0;
           for (i = 0; i < parseInt(Template.length) ; i++) {


               if (i == 0) {
                   arrMedexSubGroup[j] = Template[i].vMedExGroupCode + "++" + Template[i].vMedExSubGroupCode + "##" + Template[i].vMedExSubGroupDesc;

                   j++;
               }
               else {

                   for (t = 0; t < arrMedexGroup.length; t++) {
                       if (arrMedexGroup[t].split("##")[0] == Template[i].vMedExGroupCode) {
                           for (k = 0; k < arrMedexSubGroup.length; k++) {
                               if (arrMedexSubGroup[k].split("++")[1].split("##")[0] != Template[i].vMedExSubGroupCode || arrMedexSubGroup[k].split("++")[0] != Template[i].vMedExGroupCode) {
                                   res = true;
                               }
                               else {
                                   res = false
                                   break;
                               }

                           }
                       }
                   }
                   if (res == true) {
                       arrMedexSubGroup[j] = Template[i].vMedExGroupCode + "++" + Template[i].vMedExSubGroupCode + "##" + Template[i].vMedExSubGroupDesc;
                       j++;
                       res = "";
                   }
               }
           }
           //For Display Medex
           var tempStr = "";
           tempStr += '<table width="100%" style=" border-style:solid;border-color:black;border-width:1px;" cellspacing="0"><tr><td>'
           for (i = 0; i < arrMedexGroup.length; i++) {
               tempStr += '<table width="100%" cellspacing="0"><tr><th style="BACKGROUND-COLOR: #008ecd; font-weight: bold; color:white;  page-break-inside:avoid;" colspan="2">' + arrMedexGroup[i].split("##")[1].toString() + '</th></tr>';

               for (j = 0; j < arrMedexSubGroup.length; j++) {
                   if (arrMedexGroup[i].split("##")[0] == arrMedexSubGroup[j].split("++")[0]) {
                       if (arrMedexSubGroup[j].split("##")[1].toString() == 'Default') { }
                       else {
                           tempStr += '<tr><td colspan="2"style="BACKGROUND-COLOR: #ffcc66; font-weight: bold;">' + arrMedexSubGroup[j].split("##")[1].toString() + '</td></tr>';
                       }
                       for (k = 0; k < Template.length; k++) {
                           if (arrMedexGroup[i].split("##")[0] == Template[k].vMedExGroupCode) {
                               if (arrMedexSubGroup[j].split("++")[1].split("##")[0] == Template[k].vMedExSubGroupCode) {
                                   if (Template[k].vMedExType == "Label") {
                                       tempStr += '<tr><td style="text-align: left;width:30%; border-bottom: 1px solid black;" colspan="2">' + createObjectForData(Template[k].vMedExType, Template[k].vMedExResult, Template[k].vMedExValues, Template[k].vMedExCode) + " " + Template[k].vUOM + '</td></tr>';
                                   }
                                   else {
                                       tempStr += '<tr><td class="Label " style="text-align: left;width:30%; border-bottom: 1px solid black;">' + Template[k].vMedExDesc + ' :</td><td style="text-align: left;width:30%; border-bottom: 1px solid black;">' + createObjectForData(Template[k].vMedExType, Template[k].vMedExResult, Template[k].vMedExValues, Template[k].vMedExCode) + " " + Template[k].vUOM + '</td></tr>';
                                   }
                               }
                           }
                       }
                   }

               }
           }
           tempStr += '</table></td></tr></table>';
           $("." + div).append(tempStr.toString().trim());
           $('textarea').each(function () {
               this.style.height = "1px";
               this.style.height = (15 + this.scrollHeight) + "px";
           });
       }
       function createObjectForData(type, MedExResult, MedexValues, Medexcode) {
           var str = "";
           var temp_MedexValues = MedexValues;
           var i = 0;
           if (type.toUpperCase() == "TEXTBOX" || type.toUpperCase() == "STANDARDDATE") {
               str = "<input type='TEXTBOX'  id='txt_" + Medexcode + "' value='" + MedExResult + "' class='TextBox ' disabled='true'/>"
               return str;
           }
           if (type.toUpperCase() == "CHECKBOX") {
               for (i = 0; i < temp_MedexValues.split(",").length; i++) {
                   str += "<input type='CHECKBOX'  id='chk_" + Medexcode + "_" + i + "'"

                   for (j = 0; j < MedExResult.split(",").length; j++) {
                       if (temp_MedexValues.split(",")[i].toString().trim() == MedExResult.split(",")[j].toString().trim()) {
                           str += " checked='checked'"
                       }
                   }
                   str += " class='checkBoxlist ' disabled='true'/>"
                   str += "<label for='chk_" + Medexcode + "_" + i + "'>" + temp_MedexValues.split(",")[i].toString() + "</label>"
               }
               return str;
           }
           if (type.toUpperCase() == "RADIO") {
               for (i = 0; i < temp_MedexValues.split(",").length; i++) {
                   str += "<input type='RADIO'  id='rad_" + Medexcode + "_" + i.toString() + "' name='rad_" + Medexcode + "'"
                   if (temp_MedexValues.split(",")[i].toString() == MedExResult) {
                       str += " checked=checked"
                   }
                   str += " class='RadioButton ' disabled='true'/>"
                   str += "<label for='rad_" + Medexcode + "_" + i.toString() + "' class='Label '>" + temp_MedexValues.split(",")[i].toString() + "</label>"
               }
               return str;
           }
           if (type.toUpperCase() == "COMBOBOX") {
               str = "<select class ='dropDownList' id='cmb_" + Medexcode + "' disabled='true'>";
               for (i = 0; i < temp_MedexValues.split(",").length; i++) {
                   str += "<option value='" + i + "_" + Medexcode + "'"
                   if (temp_MedexValues.split(",")[i].toString() == MedExResult) {
                       str += " selected=selected"
                   }
                   str += ">" + temp_MedexValues.split(",")[i].toString()
                   str += " </option>"
               }
               str += "</select>";
               return str;
           }
           if (type.toUpperCase() == "FILE") {
               str = "<input type='FILE'  id='file_" + Medexcode + "' disabled='true'/>"
               return str;
           }
           if (type.toUpperCase() == "DATETIME" || type.toUpperCase() == "TIME") {
               str = "<input type='TEXTBOX'  id='txt_" + Medexcode + "' value='" + MedExResult + "'/>"
               return str;
           }
           if (type.toUpperCase() == "TEXTAREA") {
               str = "<textarea id='txt_" + Medexcode + "' class='TextArea ' disabled='true' style='width:98% ; overflow:hidden;'>" + MedExResult + "</textarea>"
               return str;
           }
           if (type.toUpperCase() == 'LABEL') {
               str = "<label id='lbl_" + Medexcode + "' class='Label '>" + MedExResult + "</label>"
               return str;
           }
           if (type.toUpperCase() == "FORMULA") {
               str = "<input type='TEXTBOX'  id='txt_" + Medexcode + "' value='" + MedExResult + "' class='TextBox ' disabled='true'/>"
               return str;
           }
       }
       function textAreaAdjust(o) {
           o.style.height = "1px";
           o.style.height = (25 + o.scrollHeight) + "px";
       }

    </script>

</asp:Content>
