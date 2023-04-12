<%@ page title="" language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmDataExtractForSDTM, App_Web_ybumpksz" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<script runat="server">
</script>


<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <style type="text/css">
        #ctl00_CPHLAMBDA_gvwSDTMData {
            overflow: auto !important;
            display: block !important;
            max-height: 500px;
            overflow-y: auto;
            overflow-x: auto;
        }

        .ui-multiselect-menu, ui-widget, ui-widget-content, ui-corner-all {
            z-index: 1000000000000 !important;
        }
        .ui-multiselect ui-widget ui-state-default ui-corner-all {
            width:250px;
        }
    </style>


    <link rel="stylesheet" type="text/css" href="App_Themes/jquery.multiselect.css" />
    <link rel="stylesheet" type="text/css" href="App_Themes/multiple-select.css" />
    <link rel="stylesheet" type="text/css" href="App_Themes/jquery.multiselect.css" />
    <link href="App_Themes/StyleBlue/UI_Theme/jquery-ui.css" rel="stylesheet" />


    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script src="Script/jquery-ui-1.10.4.custom.min.js" type="text/javascript"></script>
    <script src="Script/jquery.multiselect.min.js" type="text/javascript"></script>


    <script src="Script/jquery-1.11.dataTables.min.js" type="text/javascript"></script>
    <script src="Script/jquery.dataTables.plugins.js" type="text/javascript"></script>

    <asp:HiddenField runat="server" ID="hdnRequiredColumn" />

    <table style="width: 100%; margin-bottom: 2%;" cellpadding="5px">
        <tr>
            <td>

                <fieldset id="fldgen" class="FieldSetBox" style="width: 85%; margin: auto; margin-top: 20px;">
                    <legend id="lblProject" runat="server" class="LegendText" style="font-weight: bold; font-size: 12px; text-align: left;">
                        <img id="imgfldgen" alt="SubjectSpecific" src="images/expand.jpg" onclick="displayProjectInfo(this,'tblgen');"
                            runat="server" />
                        Project Details</legend>

                    <div id="tblgen">
                        <table cellpadding="3" align="center" width="85%">
                            <tr>
                                <td class="Label" nowrap="nowrap" style="text-align: right; width: 30%;">Project Name/Request Id* : &nbsp;
                                </td>
                                <td class="Label" style="text-align: left; width: 70%;">
                                    <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="400px" TabIndex="1"></asp:TextBox><asp:Button
                                        Style="display: none" ID="btnSetProject" runat="server" Text=" Project"></asp:Button>
                                    <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                        CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                        CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                        OnClientShowing="ClientPopulated" ServiceMethod="GetMyProjectCompletionList"
                                        ServicePath="AutoComplete.asmx" CompletionListElementID="pnlTemplate" TargetControlID="txtProject"
                                        UseContextKey="True">
                                    </cc1:AutoCompleteExtender>
                                    <asp:Panel ID="pnlTemplate" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden;" />
                                </td>
                            </tr>
                            <tr>
                                <td class="Label" nowrap="nowrap" style="text-align: right; width: 30%;">Domain Name* : &nbsp;
                                </td>
                                <td class="Label" style="text-align: left; width: 70%;">
                                    <asp:DropDownList runat="server" ID="ddlDomain"></asp:DropDownList>

                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button runat="server" ID="btnView" OnClientClick="return  Validation()" Text="View" CssClass="btn btnnew" />
                                    <asp:Button runat="server" ID="btnExport" OnClientClick="return ValidationForExcel()" CssClass=" btn btnexcel" />
                                    <asp:Button runat="server" ID="btnClear" Text="Clear" CssClass="btn btncancel" />
                                    <asp:Button runat="server" ID="btnClose" Text="Exit" OnClick="btnClose_click" CssClass="btn btnexit" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />

                                </td>
                            </tr>
                        </table>
                    </div>

                </fieldset>

                <fieldset id="fldSDTM" runat="server" class="FieldSetBox" style="width: 85%; margin: auto; margin-top: 20px; display: none;">
                    <legend id="Legend1" runat="server" class="LegendText" style="font-weight: bold; font-size: 12px; text-align: left;">
                        <img id="img1" alt="SubjectSpecific" src="images/expand.jpg" onclick="displayProjectInfo(this,'tblSDTM');"
                            runat="server" />
                        SDTM Data </legend>

                    <div id="tblSDTM" style="max-width: 1000px !important; margin-left: 80px;">
                        <asp:GridView ID="gvwSDTMData" runat="server">
                            <Columns>
                            </Columns>
                        </asp:GridView>
                    </div>

                </fieldset>

                <fieldset id="fldcDesc" runat="server" class="FieldSetBox" style="width: 85%; margin: auto; margin-top: 20px; display: none;">
                    <legend id="Legend2" runat="server" class="LegendText" style="font-weight: bold; font-size: 12px; text-align: left;">
                        <img id="img2" alt="SubjectSpecific" src="images/expand.jpg" onclick="displayProjectInfo(this,'tblcDisc');"
                            runat="server" />
                        CDASH Data </legend>
                    <div id="tblcDisc" style="max-width: 1000px !important; margin-left: 80px;">
                        <asp:GridView ID="gvwCDASH" runat="server">
                            <Columns>
                            </Columns>
                        </asp:GridView>

                    </div>
                </fieldset>


                <fieldset id="fldOther" runat="server" class="FieldSetBox" style="width: 85%; margin: auto; margin-top: 20px; display: none;">
                    <legend id="Legend3" runat="server" class="LegendText" style="font-weight: bold; font-size: 12px; text-align: left;">
                        <img id="img3" alt="SubjectSpecific" src="images/expand.jpg" onclick="displayProjectInfo(this,'tabOther');"
                            runat="server" />
                        Other Data </legend>
                    <div id="tabOther" style="max-width: 1000px !important; margin-left: 80px;">
                        <asp:GridView ID="gvwOther" runat="server">
                            <Columns>
                            </Columns>
                        </asp:GridView>

                    </div>
                </fieldset>
            </td>
        </tr>
        <tr>
            <td>
                <button id="btnExportForSDTM" runat="server" style="display: none;" />

                <cc1:ModalPopupExtender ID="MPExport" runat="server" PopupControlID="divExport"
                    BackgroundCssClass="modalBackground" TargetControlID="btnExportForSDTM" CancelControlID="ImgPopUpClose"
                    BehaviorID="MPExport">
                </cc1:ModalPopupExtender>

                <div id="divExport" runat="server" class="centerModalPopup" style="display: none; width: 50%; position: absolute; max-height: 80%; overflow: auto;">
                    <table style="width: 100%" cellpadding="5px">
                        <tr>
                            <td colspan="2">
                                <div style="width: 100%">
                                    <h1 class="header">
                                        <label id="lblHeader" class="LabelBold" style="color: white;">
                                            Download CDISC/SDTM Data
                                        </label>
                                        <img id="ImgPopUpClose" alt="Close" title="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
                                    </h1>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">Export Type *:
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList runat="server" ID="ddlExportType" CssClass="dropDownList" Style="width: 200px;">
                                    <asp:ListItem Value="0">Select Export Type</asp:ListItem>
                                    <asp:ListItem Value="S">SDTM</asp:ListItem>
                                    <asp:ListItem Value="C">CDASH</asp:ListItem>
                                    <asp:ListItem Value="O">OTHER</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">Required Column *:
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlRequireField" runat="server" Style="width: 200px;" CssClass="dropDownList" AutoPostBack="false" onchange=" fnColumnName();">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr runat="server" id="trQORIG" style="display:none">
                            <td style="text-align: right;">QORIG :
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox runat="server" ID="txtQRIG"></asp:TextBox>
                            </td>
                        </tr>
                        <tr runat="server" id="trQEVAL" style="display:none" >
                            <td style="text-align: right;">QEVAL :
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox runat="server" ID="txtQEVAL"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td></td>
                        </tr>

                        <tr>
                            <td colspan="2">
                                <asp:Label runat="server" ID="lblNotice" Style="font-weight: bold; color: red;"></asp:Label>
                                <%--<label runat="server" id="lblNotice" title= "Note : Selected Column will be included in Supplementary Class">
                                 </label>--%>
                            </td>
                        </tr>
                        

                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnDownLoad" OnClientClick="return ValidationForDownLoad()" runat="server" CssClass="btn btnexcel" />
                                <asp:Button ID="btnDownloadClear" runat="server" Text="Clear" CssClass="btn btncancel" />
                                <asp:Button ID="btnDownLoadClose" runat="server" Text="Exit" CssClass="btn btnclose" />
                            </td>
                        </tr>
                    </table>
                </div>

            </td>

        </tr>
        <tr>
            <td>
                <asp:GridView runat="server" ID="gvwExport" AutoGenerateColumns="true" Style="display: none"
                    HeaderStyle-BackColor="#1560a1" HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" HeaderStyle-Font-Names="Verdana, Arial, Helvetica, sans-serif"
                    HeaderStyle-HorizontalAlign="Center" CellPadding="3" CellSpacing="0"
                    RowStyle-Font-Names=" Verdana, Arial, Helvetica, sans-serif" RowStyle-Font-Size="10pt"
                    RowStyle-BackColor="#84c8e6" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#191970" RowStyle-ForeColor="#191970">
                </asp:GridView>



            </td>
        </tr>
    </table>




    <script type="text/javascript">

        function pageLoad() {

            MultiselectRequired();
            fnApplyColunmName1()
            if ($get('<%= gvwSDTMData.ClientID()%>') != null && $get('<%= gvwSDTMData.ClientID%>_wrapper') == null) {
                if (jQuery('#<%= gvwSDTMData.ClientID%>')) {
                    jQuery('#<%= gvwSDTMData.ClientID%>').prepend($('<thead>').append($('#<%= gvwSDTMData.ClientID%> tr:first'))).DataTable({
                        "bJQueryUI": true,
                        "bLengthChange": true,
                        "scrollCollapse": true,
                        "pageLength": 10,
                        "bProcessing": true,
                        "bPaginate": true,
                        "bFooter": false,
                        "bDestory": true,
                        "bHeader": false,
                        "AutoWidth": true,
                        "bSort": false,
                        "fixedHeader": true,
                        "oLanguage": { "sSearch": "Search" },
                        "aLengthMenu": [[10, 20, 50, -1], [10, 20, 50, "All"]],
                        "iDisplayLength": 10,
                    });
                }
                $(".dataTables_wrapper").css("width", ($(window).width() * 0.75 | 0) + "px");
            }


            if ($get('<%= gvwCDASH.ClientID()%>') != null && $get('<%= gvwCDASH.ClientID%>_wrapper') == null) {
                if (jQuery('#<%= gvwCDASH.ClientID%>')) {
                    jQuery('#<%= gvwCDASH.ClientID%>').prepend($('<thead>').append($('#<%= gvwCDASH.ClientID%> tr:first'))).DataTable({
                        "bJQueryUI": true,
                        "bLengthChange": true,
                        "scrollCollapse": true,
                        "pageLength": 10,
                        "bProcessing": true,
                        "bPaginate": true,
                        "bFooter": false,
                        "bDestory": true,
                        "bHeader": false,
                        "AutoWidth": true,
                        "bSort": false,
                        "fixedHeader": true,
                        "oLanguage": { "sSearch": "Search" },
                        "aLengthMenu": [[10, 20, 50, -1], [10, 20, 50, "All"]],
                        "iDisplayLength": 10,
                        "columnDefs": [
                                { "width": "10px", "targets": 0 },
                                { "width": "40px", "targets": 1 },
                                { "width": "100px", "targets": 2 },
                                { "width": "70px", "targets": 3 },
                                { "width": "70px", "targets": 4 },
                                { "width": "70px", "targets": 5 },
                                { "width": "70px", "targets": 6 },
                                { "width": "70px", "targets": 7 },
                                { "width": "70px", "targets": 8 },
                                { "width": "70px", "targets": 9 },
                        ],
                    });
                }
                $(".dataTables_wrapper").css("width", ($(window).width() * 0.75 | 0) + "px");
            }

            if ($get('<%= gvwOther.ClientID()%>') != null && $get('<%= gvwOther.ClientID%>_wrapper') == null) {
                if (jQuery('#<%= gvwOther.ClientID%>')) {
                    jQuery('#<%= gvwOther.ClientID%>').prepend($('<thead>').append($('#<%= gvwOther.ClientID%> tr:first'))).DataTable({
                        "bJQueryUI": true,
                        "bLengthChange": true,
                        "scrollCollapse": true,
                        "pageLength": 10,
                        "bProcessing": true,
                        "bPaginate": true,
                        "bFooter": false,
                        "bDestory": true,
                        "bHeader": false,
                        "AutoWidth": true,
                        "bSort": false,
                        "fixedHeader": true,
                        "oLanguage": { "sSearch": "Search" },
                        "aLengthMenu": [[10, 20, 50, -1], [10, 20, 50, "All"]],
                        "iDisplayLength": 10,
                        "columnDefs": [
                                { "width": "10px", "targets": 0 },
                                    { "width": "40px", "targets": 1 },
                                    { "width": "100px", "targets": 2 },
                                    { "width": "70px", "targets": 3 },
                                    { "width": "70px", "targets": 4 },
                                    { "width": "70px", "targets": 5 },
                                    { "width": "70px", "targets": 6 },
                                    { "width": "70px", "targets": 7 },
                                    { "width": "70px", "targets": 8 },
                                    { "width": "70px", "targets": 9 },
                                    ],
                    });
                }
                $(".dataTables_wrapper").css("width", ($(window).width() * 0.75 | 0) + "px");
            }


        }

        function displayProjectInfo(ele, parent) {
            if (ele.src.toString().toUpperCase().search("EXPAND") != -1) {
                $("#" + parent).slideToggle(400);
                ele.src = "images/collapse_blue.jpg";

            }
            else {
                $("#" + parent).slideToggle(400);
                ele.src = "images/expand.jpg";
            }
        }

        function ClosedisplayProjectInfo() {
            $("#" + "tblcDisc").slideToggle(400);
            $("#" + "tabOther").slideToggle(400);
        }

        function closewindow(e) {
            msgConfirmDeleteAlert(null, "Are you sure want to Exit ?", function (isConfirmed) {
                if (isConfirmed) {
                    var parWin = window.opener;
                    if (parWin != null && typeof (parWin) != 'undefined') {
                        if (parWin && parWin.open && !parWin.closed) {
                            window.parent.document.location.reload();
                        }
                    }
                    self.close();
                    __doPostBack(e.name, '');
                    return true;
                } else {

                    return false;
                }
            });
            return false;
        }


        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
                $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
            iFlag = 0;
        }

        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function Validation() {
            if ($('#<%= HProjectId.ClientId %>').val() === "") {
                msgalert("Please Enter Project Name !");
                return false;
            }

            if ($('#ctl00_CPHLAMBDA_ddlDomain :selected').text() === "Select Domain") {
                msgalert("Please Select Domain Name !");
                return false;
            }


        }
        function ValidationForExcel() {
            if ($('#<%= HProjectId.ClientId %>').val() === "") {
                msgalert("Please Enter Project Name !");
                return false;
            }

            if ($('#ctl00_CPHLAMBDA_ddlDomain :selected').text() === "Select Domain") {
                msgalert("Please Select Domain Name !");
                return false;
            }


        }

        function ValidationForDownLoad() {
            if ($('#ctl00_CPHLAMBDA_ddlExportType :selected').text() === "Select Export Type") {
                msgalert("Please Select Export Type!");
                return false;
            }

            if ($('#<%= hdnRequiredColumn.ClientId %>').val() === "") {
                msgalert("Please Select Column !");
                return false;
            }
        }


        function fnColumnName() {

            var ColunmName = [];
            document.getElementById('<%= hdnRequiredColumn.ClientID%>').value = "";
            for (i = 0; i < $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlRequireField']:checked").length ; i++) {
                ColunmName.push("" + $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlRequireField']:checked").eq(i).attr("value") + "");
            }
            document.getElementById('<%= hdnRequiredColumn.ClientID%>').value = ColunmName;
            return true;
        }

        function MultiselectRequired() {
            $('#ctl00_CPHLAMBDA_ddlRequireField').multiselect({
                includeSelectAllOption: true
            });
        }

        var ColunmName1 = [];
        function fnApplyColunmName1() {
            $("#<%= ddlRequireField.ClientID%>").multiselect({
                noneSelectedText: "--Select Column Name--",
                click: function (event, ui) {
                    if (ui.checked == true)
                        ColunmName1.push("'" + ui.value + "'");
                    else if (ui.checked == false) {
                        if ($.inArray("'" + ui.value + "'", ColunmName1) >= 0)
                            ColunmName1.splice(ColunmName1.indexOf("'" + ui.value + "'"), 1)
                    }
                    if ($("input[name$='ddlRequireField']").length > 0) {
                        //clearControls();
                    }
                },
                checkAll: function (event, ui) {
                    ColunmName1 = [];
                    for (var i = 0; i < event.target.options.length; i++) {
                        ColunmName1.push("'" + $(event.target.options[i]).val() + "'")
                    }

                },
                uncheckAll: function (event, ui) {
                    ColunmName1 = [];

                }
            });

            $("#<%= ddlRequireField.ClientID%>").multiselect("refresh");
            var CheckedCheckBox = document.getElementById('<%= hdnRequiredColumn.ClientID%>').value
            if (CheckedCheckBox != "") {

                CheckedCheckBox = CheckedCheckBox.split(',');
                for (i = 0; i <= CheckedCheckBox.length - 1; i++) {
                    $("#<%= ddlRequireField.ClientID%>").multiselect("widget").find(".ui-multiselect-checkboxes.ui-helper-reset").find("input[value=" + CheckedCheckBox[i] + "]").attr("checked", "checked");

                }
                $('#<%= ddlRequireField.ClientID%>').multiselect("update");
            }
        }

        $("#ctl00_CPHLAMBDA_ddlExportType").change(function () {
            var vWorkSpaceId = $("#ctl00_CPHLAMBDA_HProjectId").val()
            var GroupCode = $("#ctl00_CPHLAMBDA_ddlDomain :selected").val();
            var attributetypr = $("#ctl00_CPHLAMBDA_ddlExportType :selected").val();

            var content = {};
            content.vWorkSpaceId = vWorkSpaceId
            content.vMedExGroupCode = GroupCode
            content.cAttributeType = attributetypr

            $.ajax({
                type: "POST",
                url: "frmDataExtractForSDTM.aspx/GETAttributeData",
                data: JSON.stringify(content),
                contentType: "application/json; charset=utf-8",
                dataType: "json",

                success: function (data) {
                    var aaDataSet = [];
                    var mySelect = $('#ctl00_CPHLAMBDA_ddlRequireField');
                    mySelect.empty();

                    if (data.d != "" || data.d != null) {
                        data = JSON.parse(data.d)
                        var i = 0;

                        var mySelect = $('#ctl00_CPHLAMBDA_ddlRequireField');
                        mySelect.empty();

                        for (i = 0; i < data.length; i++) {
                            mySelect.append('<option value' + data[i].vMedExCode + '>' + data[i].vCDISCValues + '</option>');
                        }

                        MultiselectRequired();
                        fnApplyColunmName1()
                    }
                    else {

                        return false;
                    }
                },
                failure: function (error) {
                    alert(error);
                }
            });


        });


        function UISDTMDATA() {
            // if ($get('<%= gvwSDTMData.ClientID()%>') != null && $get('<%= gvwSDTMData.ClientID%>_wrapper') == null) {
            //     if (jQuery('#<%= gvwSDTMData.ClientID%>')) {
            //         jQuery('#<%= gvwSDTMData.ClientID%>').prepend($('<thead>').append($('#<%= gvwSDTMData.ClientID%> tr:first'))).DataTable({
            //             "bJQueryUI": true,
            //             "bLengthChange": true,
            //             "scrollCollapse": true,
            //             "pageLength": 5,
            //             "bDestory": true,
            //             "bProcessing": true,
            //             "bPaginate": true,
            //             "bFooter": false,
            //             "bHeader": false,
            //             "AutoWidth": true,
            //             "bSort": false,
            //             "fixedHeader": true,
            //             "oLanguage": { "sSearch": "Search" },
            //             "aLengthMenu": [[10, 20, 50, -1], [10, 20, 50, "All"]],
            //             "iDisplayLength": 10,
            //         });
            //     }
            //     $(".dataTables_wrapper").css("width", ($(window).width() * 0.85 | 0) + "px");
            // }


            // if ($get('<%= gvwSDTMData.ClientID%>') != null) {
            //     if ($('#<%= gvwSDTMData.ClientID%>' + ' tr').length > 0) {
            // 
            //         oTab = $('#<%= gvwSDTMData.ClientID%>').prepend($('<thead>').append($('#<%= gvwSDTMData.ClientID%> tr:first'))).dataTable({
            //             "sScrollY": (0.5 * $(window).height()),
            //             "sScrollX": "100%",
            //             scrollCollapse: true,
            //             //"sPaginationType": "full_numbers",
            //             "bJQueryUI": true,
            //             "bLengthChange": false,
            //             "bSort": true,
            //             "bPaginate": true,
            //             "bDestory": true,
            //             "bRetrieve": true,
            //             "bStateSave": false,
            //             "aaSorting": [],
            //             "bFilter": false,
            //             "sDom": '<"H"flr>t<"F"p>'
            //             
            //         });
            //     }
            // }
        }

        $('#ctl00_CPHLAMBDA_ddlExportType').change(function () {
            var val = $('#ctl00_CPHLAMBDA_ddlExportType option:selected').text();
            if (val == "CDASH" || val == "OTHER") {
                $('#<%=lblNotice.ClientID%>').html("Note : Selected column will be included in supplementary class");
                $('#ctl00_CPHLAMBDA_trQEVAL').css("display", "");
                $('#ctl00_CPHLAMBDA_trQORIG').css("display", "");
                $('#ctl00_CPHLAMBDA_txtQRIG').val('')
                $('#ctl00_CPHLAMBDA_txtQEVAL').val('')
                

            }
            else {
                $('#<%=lblNotice.ClientID%>').html("");
                $('#ctl00_CPHLAMBDA_trQEVAL').css("display", "none");
                $('#ctl00_CPHLAMBDA_trQORIG').css("display", "none");
            }

        });
    </script>

</asp:Content>

