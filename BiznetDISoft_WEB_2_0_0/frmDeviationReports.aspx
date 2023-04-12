<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmDeviationReports.aspx.vb" Inherits="frmDeviationReports" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="DataTable/css/demo_table_jui.css"></script>

    <script type="text/javascript" src="DataTable/css/demo_table.css"></script>

    <script type="text/javascript" src="DataTable/css/demo_page.css"></script>

    <script type="text/javascript" src="Script/jquery-1.4.3.min.js"></script>

    <script type="text/javascript" src="DataTable/js/jquery.dataTables.js"></script>

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" language="javascript">

        $(document).ready(function() {
            $('#test').dataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers"

            });
        });

        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%=txtProject.ClientId%>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%=txtProject.clientid%>'),
            $get('<%=HProjectId.clientid%>'), document.getElementById('<%=btnSetProject.ClientId%>'));
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
        function ReportUncheck() {
            $('#ctl00_CPHLAMBDA_chklstColumns input[type=checkbox]').each(function() {
                this.checked = false;
            });

            $('#ctl00_CPHLAMBDA_chkSelectAllFields')[0].checked = false;
        }
        function ReportUncheckForDeviation() {
            $('#ctl00_CPHLAMBDA_ChklistForDeviation input[type=checkbox]').each(function() {
                this.checked = false;
            });
            function pageLoad() {
                CreateGridHeader();
            }
            function Onscrollfnction() {
                var div = document.getElementById('<%=div_Data.ClientID %>');
                var div2 = document.getElementById('<%=div_Header.ClientID%>');
                //****** Scrolling div_Header along with div_Data ******
                div2.scrollLeft = div.scrollLeft;
                return false;
            }
            function CreateGridHeader() {
                var div_DataObj = document.getElementById('<%=div_Data.ClientID %>');
                var DataGridObj = document.getElementById('<%=gvwActivityGrid.ClientID %>');
                var div_HeaderObj = document.getElementById('<%=div_Header.ClientID %>');

                //********* Creating new table which contains the header row ***********
                var HeadertableObj = div_HeaderObj.appendChild(document.createElement('table'));

                div_DataObj.style.paddingTop = '0px';
                var div_DataWidth = div_DataObj.clientWidth;
                div_DataObj.style.width = '955px';
                div_DataObj.style.height = '400px';

                //********** Setting the style of Header Div as per the Data Div ************
                div_HeaderObj.className = div_DataObj.className;
                div_HeaderObj.style.cssText = div_DataObj.style.cssText;
                //**** Making the Header Div scrollable. *****
                div_HeaderObj.style.overflow = 'auto';
                //*** Hiding the horizontal scroll bar of Header Div ****
                div_HeaderObj.style.overflowX = 'hidden';
                //**** Hiding the vertical scroll bar of Header Div **** 
                div_HeaderObj.style.overflowY = 'hidden';
                div_HeaderObj.style.height = DataGridObj.rows[0].clientHeight + 'px';
                //**** Removing any border between Header Div and Data Div ****
                div_HeaderObj.style.borderBottomWidth = '0px';

                //********** Setting the style of Header Table as per the GridView ************
                HeadertableObj.className = DataGridObj.className;
                //**** Setting the Headertable css text as per the GridView css text 
                HeadertableObj.style.cssText = DataGridObj.style.cssText;
                HeadertableObj.border = '1px';
                HeadertableObj.rules = 'all';
                HeadertableObj.cellPadding = DataGridObj.cellPadding;
                HeadertableObj.cellSpacing = DataGridObj.cellSpacing;

                //********** Creating the new header row **********
                var Row = HeadertableObj.insertRow(0);
                Row.className = DataGridObj.rows[0].className;
                Row.style.cssText = DataGridObj.rows[0].style.cssText;
                Row.style.fontWeight = 'bold';

                //******** This loop will create each header cell *********
                for (var iCntr = 0; iCntr < DataGridObj.rows[0].cells.length; iCntr++) {
                    var spanTag = Row.appendChild(document.createElement('td'));
                    spanTag.innerHTML = DataGridObj.rows[0].cells[iCntr].innerHTML;
                    var width = 0;
                    //****** Setting the width of Header Cell **********
                    //            if(spanTag.clientWidth > DataGridObj.rows[1].cells[iCntr].clientWidth)
                    //            {
                    //                width = spanTag.clientWidth;
                    //            }
                    //            else
                    //            {
                    width = DataGridObj.rows[1].cells[iCntr].clientWidth;
                    // }
                    if (iCntr <= DataGridObj.rows[0].cells.length - 2) {
                        spanTag.style.width = width + 'px';
                    }
                    else {
                        spanTag.style.width = width + 'px';
                    }
                    DataGridObj.rows[1].cells[iCntr].style.width = width + 'px';
                }
                var tableWidth = DataGridObj.clientWidth;
                //********* Hidding the original header of GridView *******
                DataGridObj.rows[0].style.display = 'none';
                //********* Setting the same width of all the componets **********
                div_HeaderObj.style.width = div_DataWidth + 'px';
                div_DataObj.style.width = div_DataWidth + 'px';
                DataGridObj.style.width = tableWidth + 'px';
                HeadertableObj.style.width = tableWidth + 'px';

                if (tableWidth > '850') {
                    document.getElementById('<%=pnlActivityGrid.ClientID %>').style.width = '750px';
                }

                return false;

            }
        }
        function uncheckCheckbox() {
            $('input:checkbox').each(function() {
                this.checked = false;
            });
        }

    </script>

    <div style="width: 980px">
        <table style="width: 100%; text-align: center">
            <tr>
                <td style="width: 100%">
                    <table style="width: 100%">
                        <tr>
                            <td class="Label" style="width: 20%; text-align: right">
                                Template Name :-*
                                <br />
                            </td>
                            <td class="Label" style="width: 80%; text-align: left; white-space: nowrap;">
                                <asp:TextBox ID="TxtTemplateName" runat="server" CssClass="textBox" Width="622px"></asp:TextBox><asp:Button
                                    ID="Button1" runat="server" Style="display: none" Text=" Project" /><asp:HiddenField
                                        ID="HiddenField1" runat="server" />
                            </td>
                        </tr>
                        <tr runat="server" id="trActivityGroup">
                            <td class="Label" nowrap="nowrap" style="text-align: right">
                                Select Activity Group: &nbsp;
                            </td>
                            <td class="Label" nowrap="nowrap" style="text-align: left">
                                <asp:DropDownList ID="ddlActivityGroup" TabIndex="3" runat="server" CssClass="dropDownList"
                                    Width="580px" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr runat="server" id="trActivity">
                            <td class="Label" nowrap="nowrap" style="text-align: right">
                                Select Activity: &nbsp;
                            </td>
                            <td class="Label" nowrap="nowrap" style="text-align: left">
                                <asp:DropDownList ID="ddlActivity" TabIndex="4" runat="server" CssClass="dropDownList"
                                    Width="580px" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="Label" style="width: 20%; text-align: right">
                                Project :-*
                                <br />
                            </td>
                            <td class="Label" style="width: 80%; text-align: left; white-space: nowrap;">
                                <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="622px"></asp:TextBox><asp:Button
                                    ID="btnSetProject" runat="server" OnClientClick="ReportUncheck();" OnClick="btnSetProject_Click"
                                    Style="display: none" Text=" Project" /><asp:HiddenField ID="HProjectId" runat="server" />
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                    CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                    CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                    OnClientShowing="ClientPopulated" ServiceMethod="GetProjectCompletionListWithOutSponser"
                                    ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True">
                                </cc1:AutoCompleteExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="Label" style="width: 20%; text-align: right">
                                Deviation Fields :-*
                                <br />
                            </td>
                            <td class="Label" style="width: 80%; text-align: left; white-space: nowrap;">
                                <asp:DropDownList ID="DdlDeviationField1" Width="300px" runat="server" CssClass="textBox">
                                </asp:DropDownList>
                                &lt;&gt;
                                <asp:DropDownList ID="DdlDeviationField2" runat="server" Width="300px" CssClass="textBox">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="Label" style="width: 30%; text-align: left">
                                Select Fields:*
                                <asp:CheckBox ID="chkSelectAllFields" runat="server" Text="Select All" onClick="SelectAllFields()" /><asp:Panel
                                    ID="pnlFields" runat="server" Height="400px" ScrollBars="Auto" BorderColor="#184E8A"
                                    BorderWidth="2px" Width="175px">
                                    <asp:CheckBoxList ID="chklstColumns" CssClass="abc" runat="server">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Button ID="Btnswap"  runat="server" CssClass="btn btnnew" OnClientClick="ReportUncheckForDeviation();"
                                    Text=">>" />
                            </td>
                            <td class="Label" style="width: 30%; text-align: left">
                                <asp:Panel ID="Panel1" runat="server" Height="400px" ScrollBars="Auto" BorderColor="#184E8A"
                                    BorderWidth="2px" Width="175px">
                                    <asp:CheckBoxList ID="ChklistForDeviation" runat="server" checked="unChecked" value="off">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                            </td>
                            <td class="Label" style="width: 40%; text-align: left; white-space: nowrap;">
                                <table style="width: 100%">
                                    <tbody>
                                        <tr>
                                            <td style="width: 100%">
                                                <asp:Panel ID="pnlActivityGrid" runat="server" Width="700px" Height="400px" ScrollBars="none">
                                                    <div runat="server" id="div_Header">
                                                    </div>
                                                    <div style="overflow: auto; max-width: 700px; height: 400px; overflow: auto; padding-left: 5px"
                                                        id="div_Data" onscroll="Onscrollfnction();" runat="server">
                                                        <asp:GridView ID="gvwActivityGrid" runat="server" Width="390px" SkinID="grdViewSmlAutoSize"
                                                            AutoGenerateColumns="true" ShowFooter="True" AllowPaging="True" PageSize="50"
                                                            CellPadding="3" OnPageIndexChanging="gvwActivityGrid_PageIndexChanging">
                                                        </asp:GridView>
                                                    </div>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="Label" style="width: 20%; text-align: right">
                            </td>
                            <td colspan="2" class="Label" style="width: 80%; text-align: left">
                                <br />
                                <asp:Button ID="btnGenerate" runat="server" CssClass="btn btngo" Text="" OnClientClick="return Validation();" />
                                <asp:Button ID="btnExportGrid" OnClick="btnExportGrid_Click" Visible="false" runat="server"
                                    CssClass="btn btnexcel" Width="103px"></asp:Button>
                                <asp:Label ID="LblgridActivity" Visible="false" runat="server" Text="No Records Found "></asp:Label>
                                <asp:Button ID="btnSearch" Visible="false" runat="server" CssClass="btn btnnew" OnClick="btnSearch_Click"
                                    Text="Search" OnClientClick="return Validation();" />
                                <asp:Button ID="btnExit" runat="server" Text="Exit" CssClass="btn btnexit" CausesValidation="False"
                                    OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);"></asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td cssclass="textBox" colspan="2" style="width: 100%">
                                <asp:Panel ID="pnlPlaceSearchOptions" runat="server" Width="100%" ScrollBars="Auto"
                                    CssClass="Label">
                                    <asp:PlaceHolder ID="PlaceSearchOptions" runat="server" EnableViewState="true"></asp:PlaceHolder>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
