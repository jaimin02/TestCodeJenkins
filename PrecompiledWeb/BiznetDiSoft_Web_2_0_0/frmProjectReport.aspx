<%@ page title="" language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmProjectReport, App_Web_qa4vhgvp" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%--<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <script type="text/javascript" language="javascript" src="Script/popcalendar.js"></script>
    <script type="text/javascript" language="javascript" src="Script/General.js"></script>
    <script type="text/javascript" language="javascript" src="Script/Validation.js"></script>
     <link href="App_Themes/StyleBlue/UI_Theme/jquery-ui.css" rel="stylesheet" type="text/css" /> 

    <link rel="stylesheet" type="text/css" href="App_Themes/jquery.multiselect.css" />
    <link rel="stylesheet" type="text/css" href="App_Themes/multiple-select.css" />
    <link rel="stylesheet" type="text/css" href="App_Themes/jquery.multiselect.css" />
    <%-- <link href="App_Themes/StyleBlue/UI_Theme/jquery-ui.css" rel="stylesheet" type="text/css" />--%>

    <style>
        #dataTables_wrapper {
            min-width: 70% !important;
            /*height: auto !important;*/
            overflow: scroll !important;
        }

        /*.ui-multiselect {
            border: 1px solid navy;
            width: 60% ;
            max-width: 60% !important;
            max-height: 35px;
            overflow: auto;
            overflow-x: hidden;
            white-space: nowrap;           
        }*/
         .ui-multiselect {
            border: 1px solid navy;
            max-height: 35px;
            overflow: auto;
            overflow-x: hidden;
            white-space: nowrap;
            max-width: 84% !important;
        }
        .ui-helper-reset {

        }

        .ui-corner-all {
            width: 245px !important;
        }

        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

          .dataTables_wrapper {
            width:1200px;
        }
        #ctl00_CPHLAMBDA_gvProjectReportDetails_wrapper {
            width: 1270px;
            /*overflow: auto;*/
        }
        .dataTable {
            overflow: auto;
            width: 99.8%;
            display: block;
           max-height: 500px;
        }
        .ui-helper-clearfix {
            width:100%;
        }
        .ajax__calendar_container {
            z-index: 5;
        }
    </style>

    <asp:HiddenField ID="hdnProjectManager" runat="server" Value="" />
    <asp:HiddenField ID="hdnDrug" runat="server" Value="" />
    <asp:HiddenField ID="hdnSubmission" runat="server" Value="" />
    <asp:HiddenField ID="hdnSponsor" runat="server" Value="" />
    <asp:HiddenField ID="hdnProjectType" runat="server" Value="" />
    <asp:HiddenField ID="hdnProjectSubtype" runat="server" Value="" />
  <%--  <div >--%>
   <asp:UpdatePanel ID="upgrid" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>
    <table cellpadding="5px" style="width: 100%;">
        <tr>
            <td>
                <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                    <legend class="LegendText" style="color: Black; font-size: 12px">
                        <img id="img2" alt="Project Details" src="images/panelcollapse.png"
                            onclick="Display(this,'divProjectDetail');" runat="server" style="margin-right: 2px;" />Project Details</legend>
                    <div id="divProjectDetail">
                        <table width="98%" cellpadding="5px">
                            <tr>
                                <td class="Label" style="text-align: right; width:20%; white-space:nowrap;" valign="top" >Project Manager :
                                </td>
                                <td style="text-align: left; width: 9% ">
                                    <asp:DropDownList ID="ddlProjectManager" runat="server" Style="width: 10%" CssClass="dropDownList" AutoPostBack="false" onchange=" fnProjectManager();"></asp:DropDownList>
                                    <%--<asp:ListBox ID="LbProjectManager" runat="server" SelectionMode="Multiple" class="dropDownList" styel="width:50%;" />--%>

                                </td>
                                <td style="text-align: right; width: 1%; white-space:nowrap;" valign="top" class="Label">Drug :
                                </td>
                                <td style="text-align: left;width: 28% ">
                                    <asp:DropDownList ID="ddlDrug" runat="server" Style="width: 10%" CssClass="dropDownList" AutoPostBack="false" onchange=" fnDrug();"></asp:DropDownList>
                                    <%--<asp:ListBox ID="LbDrug" runat="server" SelectionMode="Multiple" class="dropDownList" styel="width:50%;" />--%>

                                </td>
                            </tr>

                            <tr>
                                <td style="text-align: right;" class="Label">Sponsor :
                                </td>
                                <td style="text-align: left;">
                                    <asp:DropDownList ID="ddlSponsor" runat="server" Style="width: 32%" CssClass="dropDownList" AutoPostBack="false" onchange=" fnSponsor();"></asp:DropDownList>
                                    <%--<asp:ListBox ID="LbSponsor" runat="server" SelectionMode="Multiple" class="dropDownList" styel="width:40%;" />--%>
                                </td>
                                <td style="text-align: right; white-space:nowrap;" class="Label">Submission :
                                </td>
                                <td style="text-align: left;">
                                    <asp:DropDownList ID="ddlSubmission" runat="server" Style="width: 32%" CssClass="dropDownList" AutoPostBack="false" onchange=" fnSubmission();"></asp:DropDownList>
                                    <%--<asp:ListBox ID="LbSubmission" runat="server" SelectionMode="Multiple" class="dropDownList" styel="width:40%;" />--%>

                                </td>
                            </tr>


                            <tr>
                                <td style="text-align: right;" class="Label">Project Type :
                                </td>
                                <td style="text-align: left;">
                                    <asp:DropDownList ID="ddlProjectType" runat="server" Style="width: 32%" CssClass="dropDownList" AutoPostBack="false" onchange=" fnProjectType();"></asp:DropDownList>
                                </td>
                                <td style="text-align: right; white-space:nowrap;" class="Label">Project SubType :
                                </td>
                                <td style="text-align: left;">
                                    <asp:DropDownList ID="ddlProjectTypeDetail" runat="server" Style="width: 32%" CssClass="dropDownList" AutoPostBack="false" onchange=" fnProjectSubType();"></asp:DropDownList>
                                </td>
                            </tr>



                            <tr>
                                <td style="text-align: right" class="Label">
                                    From Date : 
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="textBox" Width="50%"></asp:TextBox>
                                    <cc1:CalendarExtender ID="caltxtDBirthDate" runat="server" TargetControlID="txtFromDate"
                                        Format="dd-MMM-yyyy">
                                    </cc1:CalendarExtender>

                                </td>
                                <td style="text-align: right" class="Label">
                                    To Date :  
                                </td>
                                <td>
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="textBox" TabIndex="1" Width="30%" onBlur="return DateValidation()"></asp:TextBox>

                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtToDate"
                                        Format="dd-MMM-yyyy">
                                    </cc1:CalendarExtender>
                                </td>
                            </tr>

                            <tr>
                                <td style="text-align: center;" class="Label" colspan="4 ">
                                    <asp:Button ID="BtnGo" runat="server" CssClass="btn btngo" Text="" ToolTip="Go"
                                        OnClientClick="return Validate();  "  />
                                    <asp:Button ID="btncancel" runat="server" CssClass="btn btncancel" Text="Cancel" ToolTip="Cancel" OnClientClick="return ClearControl();"/>
                                    <asp:Button ID="BtnExit" runat="server" CssClass="btn btnexit" Text="Exit" ToolTip="Exit"
                                        OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />

                                  <%--  <asp:Button ID="btnGenerate" runat="server" CssClass="bnt btnsave" Text="Generate Report" ToolTip="Generate Report" Style="display: none; height:30px;" OnClientClick="return CheckData(); " />--%>

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%--<asp:Button ID="btnGenerate" runat="server" CssClass="button" Text="Generate Report" ToolTip="Generate Report" Style="margin-left: 128.5%; display:none" OnClientClick="return CheckData(); " />--%>
                                </td>
                            </tr>
                        </table>
                    </div>
                </fieldset>
            </td>
        </tr>
    </table>
 
     <table style="width: 100%;" cellpadding="5px">
    <%--<div runat="server" id="divMain" style="margin: 0 5%;; width:1107px!Important; overflow-x :auto; display: inline-block;" class="dataTables_wrapper">--%>
          <tbody>
                  <tr>
                      <td>
                          <asp:UpdatePanel ID="upmain" runat="server" ChildrenAsTriggers="true">
                              <ContentTemplate>
                          <fieldset class="FieldSetBox" id="fsetProjectData" runat="server" style="display: none; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img1" alt="Project Data" src="images/panelcollapse.png"
                                        onclick="Display(this,'divProjectData');" runat="server" style="margin-right: 2px;" />Project Data</legend>
                                <div id="divProjectData">
        <table width="99%">
           
                <tr>  
                    <div id="tempexport" style="text-align:center;">
                      <asp:Button ID="btnGenerate" runat="server" Font-Size="Smaller" CssClass="btn btnexcel" ToolTip="Export to Excel" OnClientClick="return CheckData(); " />
                    </div>
                    <td colspan="2">
                        <asp:GridView ID="gvProjectReportDetails" runat="server" Style="width:500px; margin: auto" CssClass="dataTable" 
                            AutoGenerateColumns="False" OnRowDataBound="gvProjectReportDetails_RowDataBound"
                            ShowFooter="false">
                            <Columns>
                                <asp:BoundField DataFormatString="number" HeaderText="#">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ProjectNo" HeaderText="Project No">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:BoundField DataField="Sponsorname" HeaderText="Sponsor Name">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:BoundField DataField="Drug" HeaderText="Drug Name">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:BoundField DataField="cFastingFed" HeaderText="FastFed" >
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20%" />
                                </asp:BoundField>

                                 <asp:BoundField DataField="vProjectTypeName" HeaderText="ProjectType Name">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20%" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="vProjectSubTypeName" HeaderText="Project SubType Name" >
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20%" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="StudyType" HeaderText="Study Type" >
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20%" />
                                </asp:BoundField>
                               
                                 <asp:BoundField DataField="Submission" HeaderText="Submission">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="ServiceName" HeaderText="Service Name">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="Location" HeaderText="Location">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:BoundField DataField="iNoOfSubjects" HeaderText="No Of Subjects">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:BoundField DataField="ProjectManager" HeaderText="Project Manager"  >
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20%"/>
                                </asp:BoundField>
                                 <asp:BoundField DataField="ClinicalStartDate" HeaderText="Clinical StartDate">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ClinicalEndDate" HeaderText="Clinical EndDate">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AnalysisStartDate" HeaderText="Analysis StartDate">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AnalysisEndDate" HeaderText="Analysis EndDate">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField> 
                                <asp:BoundField DataField="ReportDisPatchDate" HeaderText="Report Dispatch Date">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField DataField="cProjectStatus" HeaderText="Project Status">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>

                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>

            </div>
                </fieldset>
                                    </ContentTemplate>
                              </asp:UpdatePanel>
                </td>
                </tr>
            </tbody>
        </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnGenerate" />
            <asp:AsyncPostBackTrigger ControlID="btncancel" EventName="click"/>
        </Triggers>
        </asp:UpdatePanel>

   <%-- </div>--%>
    <script src="Script/jquery-ui-1.10.4.custom.min.js" type="text/javascript"></script>
    <script src="Script/jquery.multiselect.min.js" type="text/javascript"></script>
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>

    <script type="text/javascript" language="javascript">

        $(document).ready(function () {

        });
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

        function pageLoad() {
            MultiselectRequired();
            fnApplyProjectManager();
            fnApplyDrug();
            fnApplySponsor();
            fnApplySubmission();
            fnApplyProjectTypeMst();
            fnApplyProjectSubTypeMst();
            $('ctl00_CPHLAMBDA_gvProjectReportDetails_wrapper').width('700px');
        }
        function CheckData() {

            //if (document.getElementById("VS_ProjectReportDetails") != null) {
            //    var vCode = document.getElementById("VS_ProjectReportDetails").value;
            //    if (vCode == "0") {
            //        msgalert("Data not found !");
            //        return false;
            //    }
            //    return true;
            //}
            //else {
            //    msgalert("Data not found !");
            //    return false;
            //}

        }
        function Validate() {

            if ($("#<%= hdnProjectManager.ClientID%>").val() == '' && $("#<%= hdnDrug.ClientID%>").val() == '' && $("#<%= hdnSponsor.ClientID%>").val() == '' && $("#<%= hdnSubmission.ClientID%>").val() == '' && $("#<%= hdnProjectType.ClientID%>").val() == '' && $("#<%= hdnProjectSubtype.ClientID%>").val() == '') {
                msgalert('Please Select atleast one option !');
                return false;
            }
            var fdate = $("#ctl00_CPHLAMBDA_txtFromDate").val();
            var tdate = $("#ctl00_CPHLAMBDA_txtFromDate").val();
            var fromDate = new Date($("#ctl00_CPHLAMBDA_txtFromDate").val());
            var toDate = new Date($("#ctl00_CPHLAMBDA_txtToDate").val());
            var days = daydiff(fromDate, toDate);
            if (days > 365) {
                $("#ctl00_CPHLAMBDA_txtFromDate").val(fdate);
                $("#ctl00_CPHLAMBDA_txtFromDate").val(tdate)
                msgalert("you can not select date difference more than one year !")
                return false;
            }

            // if ($("#<%= hdnDrug.ClientID%>").val() == '') {
            //     alert('Please Select Drug Name.');
            //     return false;
            // }
            // if ($("#<%= hdnSponsor.ClientID%>").val() == '') {
            //     alert('Please Select Sponsor Name.');
            //     return false;
            // }
            // if ($("#<%= hdnSubmission.ClientID%>").val() == '') {
            //     alert('Please Select Submission Name.');
            //     return false;
            // }
            //  if ($("#<%= txtFromDate.ClientID%>").val() == '') {
            //      alert('Please Enter From Date.');
            //      return false;
            //  }
            //  if ($("#<%= txtToDate.ClientID%>").val() == '') {
            //      alert('Please Enter To Date.');
            //      return false;
            //  }

        }

        function MultiselectRequired() {

            $('#ctl00_CPHLAMBDA_ddlProjectManager').multiselect({
                includeSelectAllOption: true
            });
            $('#ctl00_CPHLAMBDA_ddlDrug').multiselect({
                includeSelectAllOption: true
            });
            $('#ctl00_CPHLAMBDA_ddlSponsor').multiselect({
                includeSelectAllOption: true
            });
            $('#ctl00_CPHLAMBDA_ddlSubmission').multiselect({
                includeSelectAllOption: true
            });

            $('#ctl00_CPHLAMBDA_ddlProjectType').multiselect({
                includeSelectAllOption: true
            });

        }

        var ProjectManager = [];
        function fnApplyProjectManager() {
            // fnDeletePreviousMultiselect();

            $("#<%= ddlProjectManager.ClientID%>").multiselect({
                noneSelectedText: "--Select Project Manager--",
                click: function (event, ui) {
                    if (ui.checked == true)
                        ProjectManager.push("'" + ui.value + "'");
                    else if (ui.checked == false) {
                        if ($.inArray("'" + ui.value + "'", ProjectManager) >= 0)
                            ProjectManager.splice(ProjectManager.indexOf("'" + ui.value + "'"), 1)
                    }
                    if ($("input[name$='ddlProjectManager']").length > 0) {
                        //clearControls();
                    }
                },
                checkAll: function (event, ui) {
                    ProjectManager = [];
                    for (var i = 0; i < event.target.options.length; i++) {
                        ProjectManager.push("'" + $(event.target.options[i]).val() + "'")
                    }

                },
                uncheckAll: function (event, ui) {
                    ProjectManager = [];

                }
            });

            $("#<%= ddlProjectManager.ClientID%>").multiselect("refresh");
            var CheckedCheckBox = document.getElementById('<%= hdnProjectManager.ClientID%>').value
            if (CheckedCheckBox != "") {

                CheckedCheckBox = CheckedCheckBox.split(',');
                for (i = 0; i <= CheckedCheckBox.length - 1; i++) {
                    $("#<%= ddlProjectManager.ClientID%>").multiselect("widget").find(".ui-multiselect-checkboxes.ui-helper-reset").find("input[value=" + CheckedCheckBox[i] + "]").attr("checked", "checked");

                }
                $('#<%= ddlProjectManager.ClientID%>').multiselect("update");
            }
        }


        var Drug = [];
        function fnApplyDrug() {
            // fnDeletePreviousMultiselect();

            $("#<%= ddlDrug.ClientID%>").multiselect({
                noneSelectedText: "--Select Drug Name--",
                click: function (event, ui) {
                    if (ui.checked == true)
                        Drug.push("'" + ui.value + "'");
                    else if (ui.checked == false) {
                        if ($.inArray("'" + ui.value + "'", Drug) >= 0)
                            Drug.splice(Drug.indexOf("'" + ui.value + "'"), 1)
                    }
                    if ($("input[name$='ddlDrug']").length > 0) {
                        //clearControls();
                    }
                },
                checkAll: function (event, ui) {
                    Drug = [];
                    for (var i = 0; i < event.target.options.length; i++) {
                        Drug.push("'" + $(event.target.options[i]).val() + "'")
                    }

                },
                uncheckAll: function (event, ui) {
                    Drug = [];

                }
            });

            $("#<%= ddlDrug.ClientID%>").multiselect("refresh");
            var CheckedCheckBox = document.getElementById('<%= hdndrug.ClientID%>').value
            if (CheckedCheckBox != "") {

                CheckedCheckBox = CheckedCheckBox.split(',');
                for (i = 0; i <= CheckedCheckBox.length - 1; i++) {
                    $("#<%= ddlDrug.ClientID%>").multiselect("widget").find(".ui-multiselect-checkboxes.ui-helper-reset").find("input[value=" + CheckedCheckBox[i] + "]").attr("checked", "checked");

                }
                $('#<%= ddlDrug.ClientID%>').multiselect("update");
            }
        }

        var Sponsor = [];
        function fnApplySponsor() {
            // fnDeletePreviousMultiselect();

            $("#<%= ddlSponsor.ClientID%>").multiselect({
                noneSelectedText: "--Select Sponsor Name--",
                click: function (event, ui) {
                    if (ui.checked == true)
                        Sponsor.push("'" + ui.value + "'");
                    else if (ui.checked == false) {
                        if ($.inArray("'" + ui.value + "'", Sponsor) >= 0)
                            Sponsor.splice(Sponsor.indexOf("'" + ui.value + "'"), 1)
                    }
                    if ($("input[name$='ddlSponsor']").length > 0) {
                        //clearControls();
                    }
                },
                checkAll: function (event, ui) {
                    Sponsor = [];
                    for (var i = 0; i < event.target.options.length; i++) {
                        Sponsor.push("'" + $(event.target.options[i]).val() + "'")
                    }

                },
                uncheckAll: function (event, ui) {
                    Sponsor = [];

                }
            });

            $("#<%= ddlSponsor.ClientID%>").multiselect("refresh");
            var CheckedCheckBox = document.getElementById('<%= hdnSponsor.ClientID%>').value
            if (CheckedCheckBox != "") {

                CheckedCheckBox = CheckedCheckBox.split(',');
                for (i = 0; i <= CheckedCheckBox.length - 1; i++) {
                    $("#<%= ddlSponsor.ClientID%>").multiselect("widget").find(".ui-multiselect-checkboxes.ui-helper-reset").find("input[value=" + CheckedCheckBox[i] + "]").attr("checked", "checked");

                }
                $('#<%= ddlSponsor.ClientID%>').multiselect("update");
            }
        }


        var ProjectType = [];
        function fnApplyProjectTypeMst() {
            // fnDeletePreviousMultiselect();

            $("#<%= ddlProjectType.ClientID%>").multiselect({
                noneSelectedText: "--Select ProjectType--",
                click: function (event, ui) {
                    if (ui.checked == true)
                        ProjectType.push("'" + ui.value + "'");
                    else if (ui.checked == false) {
                        if ($.inArray("'" + ui.value + "'", ProjectType) >= 0)
                            ProjectType.splice(ProjectType.indexOf("'" + ui.value + "'"), 1)
                    }
                    if ($("input[name$='ddlProjectType']").length > 0) {
                        //clearControls();
                    }
                },
                checkAll: function (event, ui) {
                    ProjectType = [];
                    for (var i = 0; i < event.target.options.length; i++) {
                        ProjectType.push("'" + $(event.target.options[i]).val() + "'")
                    }

                },
                uncheckAll: function (event, ui) {
                    ProjectType = [];

                }
            });

            $("#<%= ddlProjectType.ClientID%>").multiselect("refresh");
            var CheckedCheckBox = document.getElementById('<%= hdnProjectType.ClientID%>').value
            if (CheckedCheckBox != "") {

                CheckedCheckBox = CheckedCheckBox.split(',');
                for (i = 0; i <= CheckedCheckBox.length - 1; i++) {
                    $("#<%= ddlProjectType.ClientID%>").multiselect("widget").find(".ui-multiselect-checkboxes.ui-helper-reset").find("input[value=" + CheckedCheckBox[i] + "]").attr("checked", "checked");

                }
                $('#<%= ddlProjectType.ClientID%>').multiselect("update");
            }
        }




        var ProjectSubType = [];
        function fnApplyProjectSubTypeMst() {
            // fnDeletePreviousMultiselect();

            $("#<%= ddlProjectTypeDetail.ClientID%>").multiselect({
                noneSelectedText: "--Select ProjectSubType--",
                click: function (event, ui) {
                    if (ui.checked == true)
                        ProjectSubType.push("'" + ui.value + "'");
                    else if (ui.checked == false) {
                        if ($.inArray("'" + ui.value + "'", ProjectSubType) >= 0)
                            ProjectSubType.splice(ProjectType.indexOf("'" + ui.value + "'"), 1)
                    }
                    if ($("input[name$='ddlProjectTypeDetail']").length > 0) {
                        //clearControls();
                    }
                },
                checkAll: function (event, ui) {
                    ProjectSubType = [];
                    for (var i = 0; i < event.target.options.length; i++) {
                        ProjectSubType.push("'" + $(event.target.options[i]).val() + "'")
                    }

                },
                uncheckAll: function (event, ui) {
                    ProjectSubType = [];

                }
            });

            $("#<%= ddlProjectTypeDetail.ClientID%>").multiselect("refresh");
            var CheckedCheckBox = document.getElementById('<%= hdnProjectSubtype.ClientID%>').value
            if (CheckedCheckBox != "") {

                CheckedCheckBox = CheckedCheckBox.split(',');
                for (i = 0; i <= CheckedCheckBox.length - 1; i++) {
                    $("#<%= ddlProjectTypeDetail.ClientID%>").multiselect("widget").find(".ui-multiselect-checkboxes.ui-helper-reset").find("input[value=" + CheckedCheckBox[i] + "]").attr("checked", "checked");

                }
                $('#<%= ddlProjectTypeDetail.ClientID%>').multiselect("update");
            }
        }



        var Submission = [];
        function fnApplySubmission() {
            // fnDeletePreviousMultiselect();

            $("#<%= ddlSubmission.ClientID%>").multiselect({
                noneSelectedText: "--Select Submission--",
                click: function (event, ui) {
                    if (ui.checked == true)
                        Submission.push("'" + ui.value + "'");
                    else if (ui.checked == false) {
                        if ($.inArray("'" + ui.value + "'", Submission) >= 0)
                            Submission.splice(Submission.indexOf("'" + ui.value + "'"), 1)
                    }
                    if ($("input[name$='ddlSubmission']").length > 0) {
                        //clearControls();
                    }
                },
                checkAll: function (event, ui) {
                    Submission = [];
                    for (var i = 0; i < event.target.options.length; i++) {
                        Submission.push("'" + $(event.target.options[i]).val() + "'")
                    }

                },
                uncheckAll: function (event, ui) {
                    Submission = [];

                }
            });

            $("#<%= ddlSubmission.ClientID%>").multiselect("refresh");
            var CheckedCheckBox = document.getElementById('<%= hdnSubmission.ClientID%>').value
            if (CheckedCheckBox != "") {

                CheckedCheckBox = CheckedCheckBox.split(',');
                for (i = 0; i <= CheckedCheckBox.length - 1; i++) {
                    $("#<%= ddlSubmission.ClientID%>").multiselect("widget").find(".ui-multiselect-checkboxes.ui-helper-reset").find("input[value=" + CheckedCheckBox[i] + "]").attr("checked", "checked");

                }
                $('#<%= ddlSubmission.ClientID%>').multiselect("update");
            }
        }

        function fnProjectSubType() {
            var fnProjectSubType = [];
            //Subject = [];

            document.getElementById('<%= hdnProjectSubtype.ClientID%>').value = "";
            for (i = 0; i < $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlProjectTypeDetail']:checked").length ; i++) {
                //fnProjectSubType.push("'" + $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlProjectTypeDetail']:checked").eq(i).attr("value") + "'");
                fnProjectSubType.push("" + $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlProjectTypeDetail']:checked").eq(i).attr("value") + "");
            }
            document.getElementById('<%= hdnProjectSubtype.ClientID%>').value = fnProjectSubType;
            return true;
        }

        function fnProjectType() {
            var fnProjectType = [];
            //Subject = [];

            document.getElementById('<%= hdnProjectManager.ClientID%>').value = "";
            for (i = 0; i < $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlProjectType']:checked").length ; i++) {
                //fnProjectType.push("'" + $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlProjectType']:checked").eq(i).attr("value") + "'");
                fnProjectType.push("" + $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlProjectType']:checked").eq(i).attr("value") + "");
            }
            document.getElementById('<%= hdnProjectType.ClientID%>').value = fnProjectType;
            return true;
        }

        function fnProjectManager() {
            var ProjectManager = [];
            //Subject = [];

            document.getElementById('<%= hdnProjectManager.ClientID%>').value = "";
            for (i = 0; i < $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlProjectManager']:checked").length ; i++) {
                //ProjectManager.push("'" + $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlProjectManager']:checked").eq(i).attr("value") + "'");
                ProjectManager.push("" + $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlProjectManager']:checked").eq(i).attr("value") + "");
            }
            document.getElementById('<%= hdnProjectManager.ClientID%>').value = ProjectManager;
            return true;
        }

        function fnDrug() {
            var Drug = [];
            //Subject = [];

            document.getElementById('<%= hdnDrug.ClientID%>').value = "";
            for (i = 0; i < $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlDrug']:checked").length ; i++) {
                //Drug.push("'" + $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlDrug']:checked").eq(i).attr("value") + "'");
                Drug.push("" + $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlDrug']:checked").eq(i).attr("value") + "");
            }
            document.getElementById('<%= hdnDrug.ClientID%>').value = Drug;
            return true;
        }

        function fnSponsor() {
            var Sponsor = [];
            //Subject = [];

            document.getElementById('<%= hdnSponsor.ClientID%>').value = "";
            for (i = 0; i < $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlSponsor']:checked").length ; i++) {
                //Sponsor.push("'" + $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlSponsor']:checked").eq(i).attr("value") + "'");
                Sponsor.push("" + $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlSponsor']:checked").eq(i).attr("value") + "");
            }
            document.getElementById('<%= hdnSponsor.ClientID%>').value = Sponsor;

            return true;
        }

        function fnSubmission() {
            var Submission = [];
            //Subject = [];

            document.getElementById('<%= hdnSubmission.ClientID%>').value = "";
            for (i = 0; i < $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlSubmission']:checked").length ; i++) {
                //Submission.push("'" + $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlSubmission']:checked").eq(i).attr("value") + "'");
                Submission.push("" + $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlSubmission']:checked").eq(i).attr("value") + "");
            }
            document.getElementById('<%= hdnSubmission.ClientID%>').value = Submission;

            return true;
        }

        function BingGvProjectReportDetails() {
            //$('#<%= btnGenerate.ClientID%>')
            document.getElementById('ctl00_CPHLAMBDA_btnGenerate').style.removeProperty('display');
            $('#<%= gvProjectReportDetails.ClientID%>').removeAttr('style', 'display:block');
            document.getElementById("ctl00_CPHLAMBDA_fsetProjectData").style.display = 'block';

            oTab = $('#<%= gvProjectReportDetails.ClientID%>').prepend($('<thead>').append($('#<%= gvProjectReportDetails.ClientID%> tr:first'))).dataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": false,
                //  aLengthMenu: [
                //      [10, 25, 50, 100, -1],
                //      [10, 25, 50, 100, "All"]
                //  ],
            });
            return false;
        }

        function DateValidation() {
            var fdate = $("#ctl00_CPHLAMBDA_txtFromDate").val();
            var tdate = $("#ctl00_CPHLAMBDA_txtFromDate").val();
            var fromDate = new Date($("#ctl00_CPHLAMBDA_txtFromDate").val());
            var toDate = new Date($("#ctl00_CPHLAMBDA_txtToDate").val());
            var days = daydiff(fromDate, toDate);
            if (days > 365) {
                $("#ctl00_CPHLAMBDA_txtFromDate").val(fdate);
                $("#ctl00_CPHLAMBDA_txtFromDate").val(tdate)
                msgalert("you can not select date difference more than one year !")
                return false;
            }

        }
        function parseDate(str) {
            var mdy = str.split('-')
            return new Date(mdy[2], mdy[0] - 1, mdy[1]);
        }

        function daydiff(first, second) {
            return (second - first) / (1000 * 60 * 60 * 24)
        }

        function ClearControl() {
            window.location.href = "frmProjectReport.aspx";
            //$("#ctl00_CPHLAMBDA_ddlProjectManager").multiselect("refresh");
            //$("#ctl00_CPHLAMBDA_ddlDrug").multiselect("refresh");
            //$("#ctl00_CPHLAMBDA_ddlSponsor").multiselect("refresh");
            //$("#ctl00_CPHLAMBDA_ddlSubmission").multiselect("refresh");
            //$("#ctl00_CPHLAMBDA_ddlProjectType").multiselect("refresh");
            //$("#ctl00_CPHLAMBDA_ddlProjectTypeDetail").multiselect("refresh");
            return true;
        }

        $("#ctl00_CPHLAMBDA_txtToDate").on("change", function () {
            var ToDateStr = $("#ctl00_CPHLAMBDA_txtToDate").val();
            var ToDateArr = ToDateStr.split("-");
            var today = new Date();
            var d2_str = today;
            if (ToDateStr != "") {
                d1 = new Date(ToDateArr[2], ConvertMonthInt(ToDateArr[1]) - 1, ToDateArr[0]);
                if (d2_str.getTime() < d1.getTime()) {
                    msgalert('Selected Date Cannot be Greater Than Cuurent Date !');
                    $("#ctl00_CPHLAMBDA_txtToDate").val('')
                    return false;
                }
                else {
                    return true;
                }
            }
        });

        $("#ctl00_CPHLAMBDA_txtFromDate").on("change", function () {
            var ToDateStr = $("#ctl00_CPHLAMBDA_txtFromDate").val();
            var ToDateArr = ToDateStr.split("-");
            var today = new Date();
            var d2_str = today;
            if (ToDateStr != "") {
                d1 = new Date(ToDateArr[2], ConvertMonthInt(ToDateArr[1]) - 1, ToDateArr[0]);
                if (d2_str.getTime() < d1.getTime()) {
                    msgalert('Selected Date Cannot be Greater Than Cuurent Date !');
                    $("#ctl00_CPHLAMBDA_txtFromDate").val('')
                    return false;
                }
                else {
                    return true;
                }
            }
        });


    </script>


</asp:Content>

