<%@ page title="" language="VB" masterpagefile="~/ECTDMasterPage.master" autoeventwireup="false" inherits="frmScheduledActivityDeviationReport, App_Web_22suyskz" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <%--<link rel="Stylesheet" href="App_Themes/jqueryui.css" type="text/css" />--%>
    <script src="Script/jquery-ui.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="App_Themes/jquery.multiselect.css" />
    <%--<script src="Script/jquery.multiselect.js" type="text/javascript"></script>--%>
    <style type="text/css">
       table.dataTable tr.odd {
            height: 25px;
        }

        th, td {
            white-space: nowrap;
        }

        .dataTables_filter {
            float: right;
        }

        .dataTables_wrapper {
            position: inherit;
        }
    
    </style>
    <asp:UpdatePanel runat="server" ID="upScheduled" RenderMode="Inline">
        <ContentTemplate>
            <table align="center" style="margin-top: 1%;" cellpadding="0" cellspacing="0" width="70%">
                <tr>
                    <td>
                        <fieldset class="FieldSetBox">
                            <table border="0" width="100%">
                                <tr>
                                    <td class="LabelText" style="white-space: nowrap; text-align: right; width: 5%">Project:
                                    </td>
                                    <td style="text-align: left; width: 99%" colspan="5">
                                        <asp:Button ID="btnSetProject" Style="display: none;" runat="server" />
                                        <asp:TextBox ID="txtProjectNo" runat="server" CssClass="textBox" MaxLength="50" Width="99%" placeholder="Enter Project Name..."
                                            TabIndex="9" onchange="clearControls();"></asp:TextBox>
                                        <asp:HiddenField ID="HProjectId" runat="server" />
                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                                            TargetControlID="txtProjectNo" ServicePath="AutoComplete.asmx" OnClientShowing="ClientPopulated"
                                            OnClientItemSelected="OnSelected" MinimumPrefixLength="1" CompletionListItemCssClass="autocomplete_listitem"
                                            CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListCssClass="autocomplete_list"
                                            BehaviorID="AutoCompleteExtender1" CompletionListElementID="pnlProjectList" ServiceMethod="GetMyProjectCompletionList">
                                        </cc1:AutoCompleteExtender>
                                        <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 100px; overflow: auto;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LabelText" style="white-space: nowrap; text-align: right; width: 5%">Period:
                                    </td>
                                    <td class="LabelText" style="text-align: left; width: 15%">
                                        <asp:DropDownList ID="ddlPeriod" runat="server" Width="100%" TabIndex="1"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="LabelText" style="white-space: nowrap; text-align: right; width: 5%">Activity:
                                    </td>
                                    <td class="LabelText" style="text-align: left; width: 35%">
                                        <asp:DropDownList ID="ddlActivity" runat="server" Width="100%" TabIndex="1"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="LabelText" style="white-space: nowrap; text-align: right; width: 5%">Subject:
                                    </td>
                                    <td class="LabelText" style="text-align: left; width: 15%">
                                        <asp:DropDownList ID="ddlSubject" runat="server" Width="100%" TabIndex="1"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" style="text-align: center">
                                        <asp:Button ID="btnGo" runat="server" CssClass="btn btngo" Text="" Tooltip ="Go"
                                                OnClientClick="return Validate();"/>
                                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" Text="Cancel" Tooltip ="Cancel"/>
                                        <asp:Button ID="btnExporttoExcel" runat="server" CssClass="btn btnexcel"  ToolTip="Export To Excel"
                                            Style="display: none;" TabIndex="5" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
            </table>
            <table align="center"  width="90%">
                <tr>
                    <td>
                        <fieldset id="fldgvDeviation" class="FieldSetBox" runat="server" style="display: none;">
                            <div id="divDeviation" runat="server">
                                <asp:GridView ID="gvDeviation" runat="server" AutoGenerateColumns="True" >
                                    <RowStyle HorizontalAlign="Center" />
                                </asp:GridView>
                            </div>

                        </fieldset>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExporttoExcel" />
        </Triggers>
    </asp:UpdatePanel>
    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <script type="text/javascript" src="Script/Gridview.js"></script>
    <script type="text/javascript" src="Script/General.js"></script>
    <script src="Script/jquery.dataTables.min.js" type="text/javascript"></script>
    <script src="Script/jquery.dataTables.plugins.js" type="text/javascript"></script>
    <!-- Add by Anand -->
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
    </script>
    <!-- END -->

    <script type="text/javascript" language="javascript">
        
        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProjectNo.ClientID%>'));
        }
        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProjectNo.ClientID%>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }
        function pageLoad() {
            
            $(window).width() > 1180 ? wid = ($(window).width() - 94) + 'px' : wid = $(window).width() - 100 + 'px';
            $('#<%= divDeviation.ClientID%>').attr("style", "width:" + wid + ";")

        

            if ($('#<%= gvDeviation.ClientID%>')) {
                $('#<%= gvDeviation.ClientID%>').prepend($('<thead>').append($('#<%= gvDeviation.ClientID%> tr:first'))).dataTable({
                    "sScrollX": '100%',
                    "bJQueryUI": true,
                    "sPaginationType": "full_numbers",
                    "bFooter": true,
                    "bHeader": true,
                    "bAutoWidth": true,
                    "bSort": false
                });
              

            }
        }

        function Validate() {
            if (document.getElementById('ctl00_CPHLAMBDA_txtProjectNo').value.trim() == '') {
                msgalert('Please Enter Project !');
                return false;
            }
            else if (document.getElementById('ctl00_CPHLAMBDA_ddlPeriod').selectedIndex == 0) {
                msgalert('Please Select Period !');
                return false;
            }
            return true;
        }
        function fnApplySelectSubject() {
            var Subject = [];
            $("#<%= ddlSubject.ClientID%>").multiselect({
                noneSelectedText: "--Select Subject--",
                click: function (event, ui) {
                    if (ui.checked == true)
                        Subject.push("'" + ui.value + "'");
                    else if (ui.checked == false) {
                        if ($.inArray("'" + ui.value + "'", Subject) >= 0)
                            Subject.splice(Subject.indexOf("'" + ui.value + "'"), 1)
                    }

                    if ($("input[name$='ddlSubject']").length > 0) {
                        clearControls();
                    }



                },
                checkAll: function (event, ui) {
                    Subject = [];
                    for (var i = 0; i < event.target.options.length; i++) {
                        Subject.push("'" + $(event.target.options[i]).val() + "'")
                    }
                    if ($("input[name$='ddlSubject']").length > 0) {
                        clearControls();
                    }
                    $("#<%= ddlSubject.ClientID%>").multiselect("refresh");
                    $("#<%= ddlSubject.ClientID%>").multiselect("widget").find(':checkbox').click();


                },
                uncheckAll: function (event, ui) {
                    Subject = [];
                    $("#<%= ddlSubject.ClientID%>").multiselect("refresh");
                    if ($("input[name$='ddlSubject':checked]").length > 0) {
                        clearControls();
                    }
                }
            });
        }

        function clearControls() {

        }
    </script>
</asp:Content>

