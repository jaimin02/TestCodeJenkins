<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmCTMCRFReport.aspx.vb" Inherits="frmCTMCRFReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <%--<link  href="DataTable/css/demo_table_jui.css"/>

    <link  href="DataTable/css/demo_table.css" />

    <link  href="DataTable/css/demo_page.css"/>--%>

     <style type="text/css">
        
        /*Added by ketan for (Resolve issue oveRlap button in datatable)*/
        .paging_full_numbers {
            padding: 2px 6px;
            margin: 0;
        }

        .fg-buttonset {
            padding: 2px 6px;
            margin: 0;
        }

        .fg-button {
            padding: 2px 6px;
            margin: 0;
        }
        /*Ended by ketan*/
    </style>


    <%--<asp:UpdatePanel ID="upControls" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
    <table cellpadding="5px" width="100%">
        <tbody>
            <tr>
                <td colspan="2" nowrap="nowrap">
                    <asp:RadioButtonList ID="rbtnlstReportTypes" runat="server" RepeatDirection="Horizontal"
                        AutoPostBack="True" Style="margin: auto;">
                        <asp:ListItem Selected="True" Value="CRF">CRF</asp:ListItem>
                        <asp:ListItem Value="MSR">MSR</asp:ListItem>
                        <asp:ListItem>PIF</asp:ListItem>
                        <asp:ListItem>Lab</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td class="Label" nowrap="nowrap" style="text-align: right; width: 25%">Project Name/Request Id* :
                </td>
                <td class="Label" style="text-align: left">
                    <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="70%"></asp:TextBox>
                    <asp:Button Style="display: none" ID="btnSetProject" runat="server" Text=" Project"></asp:Button>
                    <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                        CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                        CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                        OnClientShowing="ClientPopulated" ServiceMethod="GetMyProjectCompletionList"
                        ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True"
                        CompletionListElementID="pnlProjectList">
                    </cc1:AutoCompleteExtender>
                    <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" />
                </td>
            </tr>
            <tr runat="server" id="trPeriodAndGenericActivities">
                <td class="Label" nowrap="nowrap" style="text-align: right">Period :
                </td>
                <td style="text-align: left">
                    <asp:DropDownList runat="server" ID="ddlPeriod" CssClass="dropDownList" AutoPostBack="true"
                        Width="40%">
                    </asp:DropDownList>
                    <asp:CheckBox ID="chkGenericActivities" runat="server" Text="Generic Activities" />
                </td>
            </tr>
            <tr>
                <td class="Label" nowrap="nowrap" style="text-align: right">Select Fields* :
                </td>
                <td style="text-align: left;">
                    <asp:CheckBox ID="chkSelectAllFields" runat="server" Text="Select All" onClick="SelectAllFields()" /><asp:Panel
                        ID="pnlFields" runat="server" Style="max-height: 100px" ScrollBars="Auto" BorderColor="#184E8A"
                        BorderWidth="2px" Width="80%">
                        <asp:CheckBoxList ID="chklstColumns" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                        </asp:CheckBoxList>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;" class="Label">Subject Screen No-Initial* :
                </td>
                <td style="text-align: left;">
                    <asp:CheckBox ID="chkSelectAllSubjects" runat="server" Text="Select All" onClick="SelectAllSubjects()" /><asp:Panel
                        ID="pnlSubjects" runat="server" Style="max-height: 100px; min-height: 15px; max-width: 80%; min-width: 45%;"
                        ScrollBars="Auto" BorderColor="#184E8A" BorderWidth="2px">
                        <asp:CheckBoxList ID="chklstSubjects" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                        </asp:CheckBoxList>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td class="Label" nowrap="nowrap" style="text-align: right">Select Option :
                </td>
                <td class="Label" nowrap="nowrap">
                    <asp:RadioButtonList ID="rbtnlstOptions" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True" Value="0">Latest Records</asp:ListItem>
                        <asp:ListItem Value="1">With Audit Trail</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td class="Label" nowrap="nowrap" style="text-align: right">Select Search Option :
                </td>
                <td class="Label" nowrap="nowrap">
                    <asp:RadioButtonList ID="rbtnlstGridOptions" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True" Value="0">Grid Searching</asp:ListItem>
                        <asp:ListItem Value="1">Column Wise Searching</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td style="text-align: center;" colspan="2">
                    <asp:Button ID="btnGo" runat="server" CssClass="btn btngo" Text="" ToolTip="Go" OnClientClick="return Validation();" />
                    <asp:Button ID="btnExportToExcel" Visible="false" runat="server"
                        ToolTip="Export To Excel" CssClass="btn btnexcel" />
                    <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" ToolTip="Cancel" Text="Cancel" />
                    <asp:Button ID="btnExit" runat="server" Text="Exit" CssClass="btn btnexit" CausesValidation="False"
                        OnClientClick="return closewindow(this);" ToolTip="Exit" />
                    <asp:Button ID="btnSearch" runat="server" ToolTip="Search" CssClass="btn btnnew" OnClick="btnSearch_Click"
                        Text="Search"/>

                    <asp:HiddenField ID="hdf_AllDDLControlVal" runat="server" Value=""></asp:HiddenField>

                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center;">
                    <asp:Panel ID="pnlPlaceSearchOptions" runat="server" Width="1010px" Style="margin: auto;"
                        ScrollBars="Auto">
                        <asp:PlaceHolder ID="PlaceSearchOptions" runat="server" EnableViewState="true"></asp:PlaceHolder>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center;">
                   <%-- <asp:UpdatePanel ID="up_gvwCRFDtl" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>--%>
                            <asp:Panel ID="pnlGrid" runat="server" Width="1010px" Style="margin: auto;" ScrollBars="Auto">
                                <asp:GridView ID="gvwCRFDtl" runat="server" SkinID="grdViewSmlAutoSize" AutoGenerateColumns="True"
                                    ShowFooter="True" AllowPaging="false" PageSize="10" OnPageIndexChanging="gvwCRFDtl_PageIndexChanging"
                                    Width="950px">
                                </asp:GridView>
                            </asp:Panel>
                        <%--</ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnSearch" />
                            <asp:PostBackTrigger ControlID="btnGo" />
                        </Triggers>
                    </asp:UpdatePanel>--%>
                </td>
            </tr>
        </tbody>
    </table>

    <script type="text/javascript" src="Script/jquery-1.4.3.min.js"></script>

    <%--<script type="text/javascript" src="DataTable/js/jquery.dataTables.js"></script>--%>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" language="javascript">

        $(document).ready(function () {
            $('#test').dataTable({
                "bJQueryUI": true,
                "bLengthChange": false,
                "sPaginationType": "full_numbers",
                "sScrollY": "350px",
                "sScrollX": "100%",
                "bScrollCollapse": true,
                "bDestroy": true,
                "iDisplayLength": 25,
                "bSort": false,
            });
        });

        function UIgvwCRFDtl() {
            $('#<%= gvwCRFDtl.ClientID%>').attr('style', 'display:block');
            oTab = $('#<%= gvwCRFDtl.ClientID%>').prepend($('<thead>').append($('#<%= gvwCRFDtl.ClientID%> tr:first'))).dataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": 25,
                "bProcessing": true,
                "bSort": false,
                "sScrollY": "350px",
                "sScrollX": "100%",
                "bScrollCollapse": true,
                aLengthMenu: [
                    [10, 25, 50, 100, -1],
                    [10, 25, 50, 100, "All"]
                ],
            });
        }
      
        $("#ctl00_CPHLAMBDA_btnSearch").click(function () {
            var str = "";
            debugger;
            $('.dropDownList').each(function (index, item) {
                if ($('option:selected', $("#" + this.id)).index() != 0) {
                    str += (this.id).replace("ctl00_CPHLAMBDA_", "") + "@@" + $("#" + this.id).val() + "##";
                    // alert($('option:selected', $("#" + this.id)).index())
                    $("#ctl00_CPHLAMBDA_hdf_AllDDLControlVal").val(str);

                }
            });
            //if (str != "") {
            //    $("#ctl00_CPHLAMBDA_hdf_AllDDLControlVal").val(str);
            //}
        });

        $("#ctl00_CPHLAMBDA_btnExportToExcel").click(function () {
            var str = "";
            $('.dropDownList').each(function (index, item) {
                if ($('option:selected', $("#" + this.id)).index() != 0) {
                    str += (this.id).replace("ctl00_CPHLAMBDA_", "") + "@@" + $("#" + this.id).val() + "##";
                }
            });
            if (str != "") {
                $("#ctl00_CPHLAMBDA_hdf_AllDDLControlVal").val(str);
            }
        });

        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
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

        function Validation() {
            if (document.getElementById('<%= HProjectId.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Project !');
                return false;
            }

            var chklst = document.getElementById('<%=chklstColumns.clientid%>');
            var chks;
            var result = false;
            var i;
            if (chklst != null && typeof (chklst) != 'undefined') {
                chks = chklst.getElementsByTagName('input');
                for (i = 0; i < chks.length; i++) {
                    if (chks[i].type.toUpperCase() == 'CHECKBOX' && chks[i].checked) {
                        result = true;
                        break;
                    }
                }
            }
            if (!result) {
                msgalert('Please Select Atleast One Field !');
                return false;
            }

            var chklstsubject = document.getElementById('<%=chklstSubjects.clientid%>');
            var chkssubject;
            var resultsubject = false;
            var index;
            if (chklstsubject != null && typeof (chklstsubject) != 'undefined') {
                chkssubject = chklstsubject.getElementsByTagName('input');
                for (index = 0; index < chkssubject.length; index++) {
                    if (chkssubject[index].type.toUpperCase() == 'CHECKBOX' && chkssubject[index].checked) {
                        resultsubject = true;
                        break;
                    }
                }
            }

            if (!resultsubject) {
                var chkGenericActivities = document.getElementById('<%=chkGenericActivities.clientid%>').checked;
                if (chkGenericActivities == false) {
                    msgalert('Please Select Atleast One Subject Or Select Generic Activities Option !');
                    return false;
                }
            }

            return true;
        }

        function SelectAllFields() {
            var chkSelectAll = document.getElementById('<%=chkSelectAllFields.clientid%>').checked;
            var chklst = document.getElementById('<%=chklstColumns.clientid%>');
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

        //function Assignvalue()
        //{

        //    var str = "";
        //    debugger;
        //    $('.dropDownList').each(function (index, item) {
        //        if ($('option:selected', $("#" + this.id)).index() != 0) {
        //            str += (this.id).replace("ctl00_CPHLAMBDA_", "") + "@@" + $("#" + this.id).val() + "##";
        //            $("#ctl00_CPHLAMBDA_hdf_AllDDLControlVal").val(str);

        //        }
        //    });
           

           


        //}

      
    </script>

</asp:Content>
