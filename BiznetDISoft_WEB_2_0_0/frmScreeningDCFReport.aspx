<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="frmScreeningDCFReport.aspx.vb" Inherits="frmScreeningDCFReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <style type="text/css">
        .DataTables_sort_wrapper {
            width: 100px;
            text-align: center;
        }

        #ctl00_CPHLAMBDA_gvScreeningSubjectRepprt {
            overflow: auto !important;
            display: block !important;
            max-height: 500px;
            overflow-y: auto;
            overflow-x: auto;
        }

        #ctl00_CPHLAMBDA_gvScreeningProjectSpecific {
            overflow: auto !important;
            display: block !important;
            max-height: 250px;
            overflow-y: auto;
            overflow-x: auto;
        }
    </style>

    <%--<link rel="stylesheet" type="text/css" href="App_Themes/smoothnessjquery-ui.css" />--%>
    <script src="Script/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery-ui.js"></script>
    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <script type="text/javascript" src="Script/General.js"></script>
    <script type="text/javascript" src="Script/Validation.js"></script>
    <script src="Script/jquery.dataTables.plugins.js" type="text/javascript"></script>
    <script src="Script/jquery.dataTables.min.js" type="text/javascript"></script>



    <asp:UpdatePanel ID="upScreeningDCF" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset class="FieldSetBox" style="display: inline-block; width: 94%; text-align: left; border: #aaaaaa 1px solid;">
                <legend class="LegendText" style="color: Black; font-size: 12px">
                    <img id="imgfldgen" alt="Screening DCF Report" src="images/panelcollapse.png"
                        onclick="DispalyDCFReport(this,'tblEntryData');" runat="server" style="margin-right: 2px;" />Screening DCF Detail</legend>
                <div id="tblEntryData" style="height: 400px">
                    <table width="95%" cellpadding="5px" style="margin-left: 5%; width: 90%;margin-top: 25px;">
                        <tr>
                            <td align="center" colspan="2">
                                <asp:RadioButtonList class="Rb1" OnSelectedIndexChanged="rblScreeningType_SelectedIndexChanged" ID="rblScreeningType" runat="server" Style="margin: -23px; color: black; padding: 10px; width: 45%;"
                                    AutoPostBack="True" RepeatDirection="Horizontal" TabIndex="1">
                                    <asp:ListItem Selected="True" Value="1">Generic Screening</asp:ListItem>
                                    <asp:ListItem Value="2">Project Specific Screening</asp:ListItem>
                                    <asp:ListItem Value="3">Project Specific </asp:ListItem>
                                </asp:RadioButtonList></td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <table border="0" align="center" style="width: 80%; margin-bottom: 2%; margin-left: 13%;" cellpadding="5px">
                                    <tr id="rwProject" runat="server" visible="false">
                                        <td style="width: 5%; text-align: right;">
                                            <asp:Label ID="lblProject" runat="server" CssClass="Label">Project Name*:</asp:Label>
                                        </td>
                                        <td colspan="3" style="text-align: left; width: 75%;">
                                            <asp:TextBox ID="txtProject" runat="server" CssClass="textBox" Width="75%" />
                                            <asp:Button Style="display: none" ID="btnSetProject" runat="server" Text=" Project"></asp:Button>
                                            <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" BehaviorID="AutoCompleteExtender2"
                                                CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected_Project"
                                                OnClientShowing="ClientPopulated_Project" ServiceMethod="GetMyProjectCompletionList"
                                                ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True"
                                                CompletionListElementID="pnlProjectList">
                                            </cc1:AutoCompleteExtender>
                                            <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 300px; overflow: auto; overflow-x: hidden" />
                                        </td>
                                    </tr>
                                    <tr id="rwSubject" runat="server">
                                        <td style="width: 5%; text-align: right;">
                                            <asp:Label ID="lblSubject" runat="server" CssClass="Label">Subject Name *:</asp:Label>
                                            
                                        </td>
                                        <td colspan="3" style="text-align: left; width: 90%;">
                                            <asp:CheckBox  ID="chkSelectAllSubjects" Visible="false" Style="margin: 0px auto; display: block;" runat="server"  Text="Select All" onClick="SelectAllSubjects()" />
                                                <asp:Panel 
                                                ID="pnlSubjects" Visible="false"   BorderWidth="1px"  runat="server" Style="max-height: 100px !important; border-radius:5px; min-height:20px;    
                                                    width:75%; float:left;"
                                                ScrollBars="Auto" >
                                                <asp:CheckBoxList ID="chklstSubjects" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:TextBox ID="txtSubject" runat="server" CssClass="textBox" TabIndex="2" Width="75%" />
                                            <asp:CheckBox ID="chkRereview" style=""  runat="server" Text="Re-review" AutoPostBack="True" />
                                            <asp:Button ID="btnSubject" runat="server" Style="display: none" Text="Subject" />
                                            <asp:HiddenField ID="HSubjectId" runat="server" />
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                                MinimumPrefixLength="1" OnClientItemSelected="OnSelected" OnClientShowing="ClientPopulated"
                                                ServiceMethod="GetSubjectCompletionList_NotRejected" ServicePath="AutoComplete.asmx"
                                                TargetControlID="txtSubject" UseContextKey="True" CompletionListElementID="pnlSubjectList"
                                                CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                CompletionListCssClass="autocomplete_list">
                                            </cc1:AutoCompleteExtender>

                                            <asp:Panel ID="plnSubjectList" runat="server" Style="max-height: 300px; overflow: auto; overflow-x: hidden" />

                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="3"></td>
                                    </tr>
                                    <tr>
                                        <td class="Label" style="width: 10%; text-align: right;">From Date: </td>
                                        <td style="text-align: left; width: 17%;">
                                            <asp:TextBox ID="txtFromDate" TabIndex="8" onchange="DateConvert(this.value,this,'Y')" runat="server" CssClass="textBox"
                                                Style="width: 180px;" onkeydown="return (event.keyCode!=13)" />
                                            <asp:Image ID="imgEditdFromDate" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit2.png" Style="display: none;" />
                                            <asp:Image ID="imgAuditFromDate" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display: none;"
                                                ImageUrl="images/audit.png" />
                                            <asp:Button ID="Button1" Style="display: none;" runat="server" Text="Check Duplicate Subject"
                                                CssClass="button" />
                                            <cc1:CalendarExtender ID="caltxtFromDate" runat="server" TargetControlID="txtFromDate"
                                                Format="dd-MMM-yyyy">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td class="Label" style="width: 10%; text-align: right;">To Date: </td>
                                        <td style="text-align: left; width: 30%;">
                                            <asp:TextBox ID="txtToDate" TabIndex="8" onchange="DateConvert(this.value,this,'Y')" runat="server" CssClass="textBox"
                                                Style="width: 180px;" onkeydown="return (event.keyCode!=13)" />
                                            <asp:Image ID="imgEditdToDate" ToolTip="Edit" CssClass="EditControl" runat="server" ImageUrl="images/Edit2.png" Style="display: none;" />
                                            <asp:Image ID="imgAuditdToDate" ToolTip="Audit Trail" CssClass="AuditControl" runat="server" Style="display: none;"
                                                ImageUrl="images/audit.png" />
                                           <asp:Button ID="btnCheckDuplicateSubject" Style="display: none;" runat="server" Text="Check Duplicate Subject"
                                                CssClass="button" />
                                            <cc1:CalendarExtender ID="caltxtToDate" runat="server" TargetControlID="txtToDate"
                                                Format="dd-MMM-yyyy">
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Label" style="width: 10%; text-align: right;">Group : </td>
                                        <td style="text-align: left; width: 17%;">
                                            <asp:DropDownList ID="ddlGroup" runat="server" AutoPostBack="true" onchange="validationNew()"
                                                CssClass="dropDownList" Width="75%" TabIndex="3">
                                                <asp:ListItem Value="0">All Group</asp:ListItem>
                                            </asp:DropDownList></td>
                                        <td class="Label" style="width: 10%; text-align: right;">DCF Type : </td>
                                        <td style="text-align: Left; width: 20%;">
                                            <asp:DropDownList ID="ddlDCFType" runat="server" CssClass="dropDownList" Width="50%">
                                                <asp:ListItem Selected="True" Text="All" Value="0"></asp:ListItem>
                                                <asp:ListItem Value="S">System</asp:ListItem>
                                                <asp:ListItem Value="M">Manual</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Label" style="width: 10%; text-align: right;">Generated By : </td>
                                        <td style="text-align: left; width: 17%;">
                                            <asp:DropDownList ID="ddlGeneratedBy" runat="server" CssClass="dropDownList" Width="75%">
                                            </asp:DropDownList></td>
                                        <td class="Label" style="width: 10%; text-align: right;">Resolved By : </td>
                                        <td style="text-align: left; width: 17%;">
                                            <asp:DropDownList ID="ddlResolvedBy" runat="server" CssClass="dropDownList" Width="50%">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Label" colspan="4" align="center">
                                            <asp:Button runat="server" ID="btnGo" ToolTip="Go" CssClass="btn btngo" 
                                                OnClientClick="return ValidationForGo(); "/>
                                            <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" ToolTip="Cancel" Text="Cancel"  />
                                            <asp:Button ID="btnExit" runat="server" CssClass="btn btnclose" Text="Exit"  OnClientClick="return closewindow(this);" />
                                            <asp:Button ID="btnExportToExcel" runat="server" CssClass="btn btnexcel" Visible="false" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <asp:Label runat="server" ID="lblTest" Text=""></asp:Label>
                </div>
            </fieldset>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSubject" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnExportToExcel" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:UpdatePanel runat="server" ID="upScreeningSubjectReport" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset class="FieldSetBox" style="display: inline-block; width: 94%; text-align: left; border: #aaaaaa 1px solid;">
                <legend class="LegendText" style="color: Black; font-size: 12px">
                    <img id="img1" alt="Screening DCF Report" src="images/panelcollapse.png"
                        onclick="DispalyDCFReportDetail(this,'tblEntryReportDate');" runat="server" style="margin-right: 2px;" />Screening DCF Data</legend>

                <div id="tblEntryReportDate">
                    <table style="width: 90%; margin: auto;">
                        <tr>
                            <td style="width: 100%">
                                <asp:Panel ID="pnlGrid" runat="server" Style="margin: auto; overflow: auto;">
                                    <asp:GridView ID="gvScreeningSubjectRepprt" runat="server" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:BoundField HeaderText="Sr. No">
                                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="nScreeningDCFNo" HeaderText="Screening DCF No" />
                                            <asp:BoundField DataField="vProjectNo" HeaderText="Project No" />
                                            <asp:BoundField DataField="GroupName" HeaderText="Group Name" />
                                            <asp:BoundField DataField="vSubjectId" HeaderText="Subject Id" />
                                            <asp:BoundField DataField="ScreenDate" HeaderText="Screening Date" />
                                            <asp:BoundField DataField="Attribute" HeaderText="Attribute" />
                                            <asp:BoundField DataField="vDiscrepancy" HeaderText="Discrepancy on Value" />
                                            <asp:BoundField DataField="cDcfType" HeaderText="DCF Type" />
                                            <asp:BoundField DataField="Query" HeaderText="Query" />
                                            <asp:BoundField DataField="ModifyValue" HeaderText="Modified Value" />
                                            <asp:BoundField DataField="ModifyRemarks" HeaderText="Modification Remarks" />
                                            <asp:BoundField DataField="ModifyDate" HeaderText="Modified Date" />
                                            <asp:BoundField DataField="ModifyBy" HeaderText="Modified by" />
                                            <asp:BoundField DataField="DCFRemarks" HeaderText="DCF Remarks" />
                                            <asp:BoundField DataField="DCFCreatedby" HeaderText="Created By" />
                                            <asp:BoundField DataField="DCFCreatedDate" HeaderText="Created Date" DataFormatString="{0:dd-MMM-yyyy HH:mm }" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Status" HeaderText="Status" />
                                            <asp:BoundField DataField="DataEntryBy" HeaderText="Data Entry By" />
                                            <asp:BoundField DataField="DCFUpdatedOn" HeaderText="Data Entry On" />
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Edit
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="lnkEdit" runat="server" ImageUrl="~/images/Edit2.gif" ToolTip="Edit" />
                                                 </ItemTemplate>
                                                 <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                           </asp:TemplateField>
                                            <asp:BoundField DataField="vMedExGroupCode" HeaderText="GroupCode" />
                                             <asp:templatefield>
                                                    <headertemplate>
                                                        Resolve
                                                    </headertemplate>
                                                    <itemtemplate>
                                                        <asp:linkbutton runat="server" id="lnkResolve" text="resolve" commandargument='<%# eval("nscreeningdcfno") %>'
                                                            commandname="resolve"></asp:linkbutton>

                                                    </itemtemplate>
                                              </asp:templatefield>
                                            <asp:BoundField DataField="iDCFBY" HeaderText="DCF BY" />
                                            <asp:BoundField DataField="DCFWORKFLOW" HeaderText="DCF WorkFlow" />
                                            <asp:BoundField DataField="DCFUserType" HeaderText="DCF UserType" />
                                            <asp:BoundField DataField="dScreenDate" HeaderText="dScreenDate" />
                                        </Columns>
                                        <SelectedRowStyle BackColor="Moccasin"></SelectedRowStyle>
                                    </asp:GridView>

                                    <asp:GridView ID="gvScreeningProjectSpecific" runat="server" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:BoundField HeaderText="Sr. No">
                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="nScreeningDCFNo" HeaderText="Screening DCF No" />
                                            <asp:BoundField DataField="vProjectNo" HeaderText="Project No" ItemStyle-Width="100" />
                                            <asp:BoundField DataField="GroupName" HeaderText="Group Name" />
                                            <asp:BoundField DataField="vSubjectId" HeaderText="Subject Id" />
                                            <asp:BoundField DataField="ScreenDate" HeaderText="Screening Date" />
                                            <asp:BoundField DataField="Attribute" HeaderText="Attribute" />
                                            <asp:BoundField DataField="vDiscrepancy" HeaderText="Discrepancy on Value" />
                                            <asp:BoundField DataField="cDcfType" HeaderText="DCF Type" />
                                            <asp:BoundField DataField="Query" HeaderText="Query" />
                                            <asp:BoundField DataField="ModifyValue" HeaderText="Modified Value" />
                                            <asp:BoundField DataField="ModifyRemarks" HeaderText="Modification Remarks" />
                                            <asp:BoundField DataField="ModifyDate" HeaderText="Modified Date" />
                                            <asp:BoundField DataField="ModifyBy" HeaderText="Modified by" />
                                            <asp:BoundField DataField="DCFRemarks" HeaderText="DCF Remarks" />
                                            <asp:BoundField DataField="DCFCreatedby" HeaderText="Created By" />
                                            <asp:BoundField DataField="DCFCreatedDate" HeaderText="Created Date" DataFormatString="{0:dd-MMM-yyyy HH:mm }" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Status" HeaderText="Status" />
                                            <asp:BoundField DataField="DataEntryBy" HeaderText="Data Entry By" />
                                            <asp:BoundField DataField="DCFUpdatedOn" HeaderText="Data Entry On" />
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Edit
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="lnkEditForProjectSpecific" runat="server" ImageUrl="~/images/Edit2.gif" ToolTip="Edit" />
                                                 </ItemTemplate>
                                                 <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                           </asp:TemplateField>
                                            <asp:BoundField DataField="vMedExGroupCode" HeaderText="GroupCode" />
                                              <asp:templatefield>
                                                    <headertemplate>
                                                        Resolve
                                                    </headertemplate>
                                                    <itemtemplate>
                                                        <asp:linkbutton runat="server" id="lnkResolve" text="resolve" commandargument='<%# eval("nscreeningdcfno") %>'
                                                            commandname="resolve"></asp:linkbutton>

                                                    </itemtemplate>
                                              </asp:templatefield>
                                            <asp:BoundField DataField="iDCFBY" HeaderText="DCF BY" />
                                            <asp:BoundField DataField="DCFWORKFLOW" HeaderText="DCF WorkFlow" />
                                            <asp:BoundField DataField="DCFUserType" HeaderText="DCF UserType" />
                                            <asp:BoundField DataField="dScreenDate" HeaderText="dScreenDate " />
                                        </Columns>
                                        <SelectedRowStyle BackColor="Moccasin"></SelectedRowStyle>
                                    </asp:GridView>

                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </div>
            </fieldset>
        </ContentTemplate>
    
    </asp:UpdatePanel>
    <asp:HiddenField runat="server" ID="hdnSelectedIndex" />

    <asp:GridView runat="server" ID="gvExport" AutoGenerateColumns="false" Style="display: none"
        HeaderStyle-BackColor="#1560a1" HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" Header-style-font-size="10px !importnat" HeaderStyle-Font-Names="Verdana, Arial, Helvetica, sans-serif"
        HeaderStyle-Font-Size=" 0.9em"
        HeaderStyle-HorizontalAlign="Center" CellPadding="3" CellSpacing="0"
        RowStyle-Font-Names=" Verdana, Arial, Helvetica, sans-serif" RowStyle-Font-Size="10pt"
        RowStyle-BackColor="#84c8e6" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#191970" RowStyle-ForeColor="#191970">
        <Columns>
            <asp:BoundField HeaderText="Sr. No" Visible="false">
                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="nScreeningDCFNo" HeaderText="Screening DCF No" />
            <asp:BoundField DataField="vProjectNo" HeaderText="Project No" ItemStyle-Width="100" />
            <asp:BoundField DataField="GroupName" HeaderText="Group Name" />
            <asp:BoundField DataField="vSubjectId" HeaderText="Subject Id" />
            <asp:BoundField DataField="ScreenDate" HeaderText="Screening Date" />
            <asp:BoundField DataField="Attribute" HeaderText="Attribute" />
            <asp:BoundField DataField="vDiscrepancy" HeaderText="Discrepancy on Value" />
            <asp:BoundField DataField="cDcfType" HeaderText="DCF Type" />
            <asp:BoundField DataField="Query" HeaderText="Query" />
            <asp:BoundField DataField="ModifyValue" HeaderText="Modified Value" />
            <asp:BoundField DataField="ModifyRemarks" HeaderText="Modification Remarks" />
            <asp:BoundField DataField="ModifyDate" HeaderText="Modified Date" />
            <asp:BoundField DataField="ModifyBy" HeaderText="Modified by" />
            <asp:BoundField DataField="DCFRemarks" HeaderText="DCF Remarks" />
            <asp:BoundField DataField="DCFCreatedby" HeaderText="Created By" />
            <asp:BoundField DataField="DCFCreatedDate" HeaderText="Created Date" DataFormatString="{0:dd-MMM-yyyy HH:mm }" ItemStyle-Wrap="false" />
            <asp:BoundField DataField="Status" HeaderText="Status" />
            <asp:BoundField DataField="DataEntryBy" HeaderText="Data Entry By" />
            <asp:BoundField DataField="DCFUpdatedOn" HeaderText="Data Entry On" />
        </Columns>
    </asp:GridView>
    <asp:Button ID="Btn_Resolve" runat="server" Style="display: none" />
    <asp:HiddenField ID="HFnDCFNo" runat="server" />

    <script language="javascript" type="text/javascript">


        function pageLoad() {
            var pnlWidth = screen.width - 115 + 'px';
            $("#ctl00_CPHLAMBDA_pnlGrid").width(pnlWidth);
            $("#ctl00_CPHLAMBDA_gvScreeningSubjectRepprt").width(pnlWidth)
            $("#ctl00_CPHLAMBDA_gvScreeningProjectSpecific").width(pnlWidth)
            var index = $("#ctl00_ddlProfile option:selected").index()
            $("#hdnSelectedIndex").val(index)


            $(window).width() > 1180 ? wid = ($(window).width() - 94) + 'px' : wid = $(window).width() - 100 + 'px';
            $('#<%= pnlGrid.ClientID%>').attr("style", "width:" + wid + ";")

            if ($get('<%= gvScreeningSubjectRepprt.ClientID()%>') != null && $get('<%= gvScreeningSubjectRepprt.ClientID%>_wrapper') == null) {
                if ($('#<%= gvScreeningSubjectRepprt.ClientID%>')) {
                    jQuery('#<%= gvScreeningSubjectRepprt.ClientID%>').prepend($('<thead>').append($('#<%= gvScreeningSubjectRepprt.ClientID%> tr:first'))).DataTable({
                        "bJQueryUI": true,
                        "bPaginate": true,
                        "bFooter": false,
                        "bHeader": false,
                        "AutoWidth": true,
                        "bSort": false,
                        "fixedHeader": true,
                        "oLanguage": { "sSearch": "Search" },
                        "aLengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
                        "iDisplayLength": 10,
                        "sPaginationType": "full_numbers",
                        "oLanguage": {
                            "sEmptyTable": "No Record Found",
                        }

                    });

                }
                //$(".dataTables_wrapper").css("width", ($(window).width() * 0.99 | 0) + "px");
            }


            if ($get('<%= gvScreeningProjectSpecific.ClientID()%>') != null && $get('<%= gvScreeningProjectSpecific.ClientID%>_wrapper') == null) {
                if ($('#<%= gvScreeningProjectSpecific.ClientID%>')) {
                    jQuery('#<%= gvScreeningProjectSpecific.ClientID%>').prepend($('<thead>').append($('#<%= gvScreeningProjectSpecific.ClientID%> tr:first'))).DataTable({
                        "bJQueryUI": true,
                        "bPaginate": true,
                        "bFooter": false,
                        "bHeader": false,
                        "AutoWidth": true,
                        "bSort": false,
                        "fixedHeader": true,
                        "oLanguage": { "sSearch": "Search" },
                        "aLengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
                        "iDisplayLength": 10,
                        "sPaginationType": "full_numbers",
                        "oLanguage": {
                            "sEmptyTable": "No Record Found",
                        }
                    });

                }
                //$(".dataTables_wrapper").css("width", ($(window).width() * 0.99 | 0) + "px");
            }

        }
        function DispalyDCFReport(control, target) {
            if (control.src.toString().toUpperCase().search("EXPAND") != -1) {
                $("#" + target).slideToggle(600);
                control.src = "images/panelcollapse.png";
            }
            else {
                $("#" + target).slideToggle(600);
                control.src = "images/panelexpand.png";
            }
        }

        function DispalyDCFReportDetail(control, target) {
            if (control.src.toString().toUpperCase().search("EXPAND") != -1) {
                $("#" + target).slideToggle(600);
                control.src = "images/panelcollapse.png";
            }
            else {
                $("#" + target).slideToggle(600);
                control.src = "images/panelexpand.png";
            }
        }
        function closewindow(e) {
            msgConfirmDeleteAlert(null, "Are You Sure You Want To Exit?", function (isConfirmed) {
                if (isConfirmed) {
                    var parWin = window.opener;
                    if (parWin != null && typeof (parWin) != 'undefined') {
                        if (parWin && parWin.open && !parWin.closed) {
                            window.parent.document.location.reload();
                        }
                    }
                    self.close();
                    return true;
                }
                else {
                    return false;
                }
            });
            return false;
        }

        function DateConvert(ParamDate, txtdate, CheckLess) {
            if (txtdate.value != "") {
                if (!DateConvert(ParamDate, txtdate)) {
                    return false;
                }
                if (CheckLess = 'Y' && !CheckDateLessThenToday(txtdate.value)) {
                    msgalert('Date should be previous or equal to current date.');
                    return false;
                }
                var dob = document.getElementById('<%=txtToDate.ClientID%>');
                var btn = document.getElementById('<%=btnCheckDuplicateSubject.ClientID %>');
                if (initial.value != "" && dob.value != "") {
                }
            }
            getAge();
            validateHabit();
            return true;
        }

        function ClientPopulated_Project


            (sender, e) {
            ProjectClientShowing('AutoCompleteExtender2', $get('<%= txtProject.ClientId %>'));
            }

            function OnSelected_Project(sender, e) {
                ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
          $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
            }
            function ClientPopulated(sender, e) {
                SubjectClientShowing('AutoCompleteExtender1', $get('<%= txtSubject.ClientId %>'));
            }

            function OnSelected(sender, e) {
                SubjectOnItemSelected(e.get_value(), $get('<%= txtSubject.clientid %>'),
          $get('<%= HSubjectId.clientid %>'), document.getElementById('<%= btnSubject.ClientId %>'));
        }

        function ValidationForGo() {

            if ($("#ctl00_CPHLAMBDA_rwProject").is(":visible") == true) {
                if ($("#ctl00_CPHLAMBDA_HProjectId").val() == "" && $("#ctl00_CPHLAMBDA_HSubjectId").val() == "") {
                    document.getElementById('<%=txtProject.ClientID%>').value = '';
                  msgalert('Please Enter Project Name');
                  document.getElementById('<%=txtProject.ClientID%>').focus();
                  return false;
              }
          }
          else {
              if ($("#ctl00_CPHLAMBDA_HSubjectId").val() == "") {
                  document.getElementById('<%=txtSubject.ClientID%>').value = '';
                  msgalert('Please Enter Subject Name');
                  document.getElementById('<%=txtSubject.ClientID%>').focus();
                  return false;
              }
          }

          return true;
      }

      function OpenWindow(Path) {
          window.open(Path);
          return false;
      }

      function ShowConfirmation(nDCFNo) {
          var Istrue = confirm("Are you sure you want to resolve this Discrepancy?");
          if (Istrue == true) {
              document.getElementById('<%=HFnDCFNo.clientid %>').value = nDCFNo;
            document.getElementById('<%=Btn_Resolve.clientid %>').click();
            return true;
        }
        return false;
      }

        function SelectAllSubjects() {
            var chkSelectAll = document.getElementById('<%=chkSelectAllSubjects.clientid%>').checked;
            var chklst = document.getElementById('<%=chklstSubjects.clientid%>');
            var chks;
            var result = false;
            var i;
            if (chklst != null && typeof (chklst) != 'undefined') {
                chks = chklst.getElementsByTagName('input');
                if (chkSelectAll == true) {
                    for (i = 0; i < chks.length; i++) {
                        chks[i].checked = true;
                    }
                }
                else {
                    for (i = 0; i < chks.length; i++) {
                        chks[i].checked = false;
                    }
                }
            }
            return false;
        }

    </script>
</asp:Content>



