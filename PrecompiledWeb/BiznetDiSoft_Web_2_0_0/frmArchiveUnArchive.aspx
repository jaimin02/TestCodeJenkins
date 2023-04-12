<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmArchiveUnArchive, App_Web_l40sj1d0" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <asp:UpdatePanel ID="upanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdn_prjectstatus" />
            <asp:HiddenField runat="server" ID="hdn_LabArchiveFlag" />
            <asp:HiddenField runat="server" ID="hdn_LabArchiveStatus" />
            <table width="100%" cellpadding="3px" id="maintable">
                <tbody>
                    <tr>
                        <td style="width: 30%; white-space: nowrap; text-align :right ;">
                            <label id="lblProject" class="Label ">
                                Project* :</label>
                        </td>
                        <td  style ="text-align :left ;">
                            <asp:TextBox ID="txtProject" runat="server" Width="60%" CssClass="textBox " />
                            <asp:Button Style="display: none" ID="btnSetProject" runat="server" />
                            <asp:HiddenField ID="HProjectId" runat="server" />
                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                                TargetControlID="txtProject" ServicePath="AutoComplete.asmx" ServiceMethod="GetMyProjectCompletionListForArchive"
                                OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1"
                                CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1">
                            </cc1:AutoCompleteExtender>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"  style ="text-align :center ;">
                            <asp:RadioButtonList ID="rblArchive" runat="server" RepeatDirection="Horizontal"
                                CssClass="RadioButton " style="margin :auto;">
                                <asp:ListItem Selected="True" Text="Archive"></asp:ListItem>
                                <asp:ListItem Text="UnArchive"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; white-space: nowrap;">
                            <label id="lblRemark" class="Label ">
                                Remark* :</label>
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="txtRemark"  runat="server" TextMode="MultiLine" Width ="40%" CssClass="textBox"
                                Height="35px" MaxLength ="500" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; white-space: nowrap;">
                            <label id="lblForYears" class="Label">
                                For Years* :</label>
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="txtYears" runat="server" CssClass="textBox" Width="6%" MaxLength ="4" onkeypress="return ValidateYear(event);" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;" colspan="2">
                            <asp:Button ID="btnSave" runat="server" Text="Save" ToolTip="Save" CssClass="btn btnsave"
                                OnClientClick="return validation();" />
                            <asp:Button ID="btnExit" runat="server" Text="Exit" ToolTip="Exit" CssClass="btn btnexit " />
                            <asp:Button ID="btnShowAll" runat="server" Text="Show All" ToolTip="Show All" CssClass="btn btnnew" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Panel ID="pnlReleasedetail" runat="server" Style="width: 100%; max-height: 500px"
                                ScrollBars="Auto">
                                <asp:GridView runat="server" ID="GV_ArchieveDetail" AutoGenerateColumns="false" Width="90%"
                                    BorderStyle="Solid" BorderColor="#1560a1" BorderWidth="1" EmptyDataText="No Data Found">
                                    <RowStyle BackColor="#cee3ed" Font-Names="Verdana" VerticalAlign="Top" HorizontalAlign="left"
                                        Font-Size="Small" ForeColor="navy" />
                                    <EditRowStyle BackColor="#cee3ed" Font-Names="Verdana" Font-Size="Small" />
                                    <HeaderStyle Font-Underline="False" BackColor="#1560a1" Font-Names="Verdana" VerticalAlign="Top"
                                        Font-Size="Small" HorizontalAlign="Center" ForeColor="white" Font-Bold="True" />
                                    <FooterStyle BackColor="#1560a1" Font-Names="Verdana" Font-Size="Small" HorizontalAlign="Center"
                                        ForeColor="white" Font-Bold="True" />
                                    <Columns>
                                        <asp:BoundField DataField="vProjectNo" HeaderText="Project NO">
                                            <ItemStyle HorizontalAlign="Center" Width="10" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="iArchiveYear" HeaderText="Archive Year">
                                            <ItemStyle HorizontalAlign="Center" Width="10" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="iArchivedForYrs" HeaderText="Archive ForYears">
                                            <ItemStyle HorizontalAlign="Center" Width="30" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="cArchiveFlag" HeaderText="Status">
                                            <ItemStyle HorizontalAlign="Center" Width="10" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="vRemarks" HeaderText="Remarks">
                                            <ItemStyle HorizontalAlign="Center" Width="10" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="vUserTypeName" HeaderText="Archive/UnArchive By">
                                            <ItemStyle HorizontalAlign="Center" Width="10" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="dModifyOn" DataFormatString="{0:dd'-'MMM'-'yyyy   H:mm tt}"
                                            HeaderText="Archived/UnArchived On">
                                            <ItemStyle HorizontalAlign="Center" Width="90" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="vWorkspaceID" HeaderText="WorkspaceID" />
                                        <asp:TemplateField HeaderText="ArchiveLabReport">
                                            <ItemStyle HorizontalAlign="Center" Width="12" Wrap="false" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemTemplate>
                                                <asp:LinkButton Text="Archive" ID="ArchiveLabrpt" CommandName="ArchiveLabData" runat="server"
                                                    ToolTip="Archive" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Audit Trail" SortExpression="status">
                                            <ItemStyle HorizontalAlign="Center" Width="20" VerticalAlign="Middle" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Audittrail" runat="server" ToolTip="Audit Trail" ImageUrl="~/images/AuditTrailUpdated.png"
                                                    CommandName=" Audittrail" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                            <asp:GridView runat="server" ID="ForExport" Style="display: none" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:BoundField DataField="vProjectNo" HeaderText="Project NO" />
                                    <asp:BoundField DataField="iArchiveYear" HeaderText="Archive Year" />
                                    <asp:BoundField DataField="iArchivedForYrs" HeaderText="ArchiveForYears" />
                                    <asp:BoundField DataField="cArchiveFlag" HeaderText="Project Status" />
                                    <asp:BoundField DataField="cArchiveForLabData" HeaderText="Lab-Report Status" />
                                    <asp:BoundField DataField="vRemarks" HeaderText="Remarks" />
                                    <asp:BoundField DataField="vUserTypeName" HeaderText="Archive By" />
                                    <asp:BoundField DataField="dModifyOn" DataFormatString="{0:dd'-'MMM'-'yyyy H:mm tt}"
                                        HeaderText="Project Archive/UnArchive On" />
                                    <asp:BoundField DataField="dLabDataModifyOn" DataFormatString="{0:dd'-'MMM'-'yyyy H:mm tt}"
                                        HeaderText="Lab-Report Archive/UnArchive On" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: center;">
                            <asp:Button ID="btnImportToExcel" runat="server" ToolTip="Export To Excel"
                                CssClass="btn btnexcel" OnClientClick="return EmptyGrid(this)" />
                            <asp:Button ID="btnExportToPdf" runat="server"  ToolTip="Export To PDF"
                                CssClass="btn btnpdf"  OnClientClick="return EmptyGrid(this)" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <cc1:ModalPopupExtender ID="MPEAction" runat="server" PopupControlID="divDocAction"
                                PopupDragHandleControlID="LblPopUpSubMgmt" BackgroundCssClass="modalBackground"
                                BehaviorID="MPEAction" CancelControlID="ImgPopUpClose" TargetControlID="btnShow">
                            </cc1:ModalPopupExtender>
                            <table>
                                <tbody>
                                    <tr style="display: none">
                                        <td>
                                            <asp:Button ID="btnShow" runat="server" Text="Show" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <div id="divDocAction" runat="server" class="centerModalPopup" style="display: none;
                                width: 100%; max-height: 600px">
                                <div style="width: 100%">
                                    <h1 class="header">
                                        <label id="lblDocAction" class="LabelBold ">
                                            AuditTrail
                                        </label>
                                        <img id="ImgPopUpClose" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
                                    </h1>
                                </div>
                                <asp:Panel ID="pnlDocAction" runat="server" Style="max-height: 200px; width: 100%;
                                    overflow: auto">
                                    <table style="width: 100%" cellpadding="3">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="GV_Audittrail" runat="server" AutoGenerateColumns="false" SkinID="grdViewSmlAutoSize"
                                                    Width="100%">
                                                    <Columns>
                                                        <asp:BoundField DataField="vProjectNo" HeaderText="Project NO">
                                                            <ItemStyle HorizontalAlign="Center" Width="10" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="iArchiveYear" HeaderText="Archive Year">
                                                            <ItemStyle HorizontalAlign="Center" Width="5" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="iArchivedForYrs" HeaderText="Archive For Years">
                                                            <ItemStyle HorizontalAlign="Center" Width="5" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="cArchiveFlag" HeaderText="Project-Status">
                                                            <ItemStyle HorizontalAlign="Center" Width="10" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vRemarks" HeaderText="Remarks">
                                                            <ItemStyle HorizontalAlign="Center" Width="10" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vUserTypeName" HeaderText="Archive/ UnArchive By">
                                                            <ItemStyle HorizontalAlign="Center" Width="10" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="dModifyOn_IST" DataFormatString="{0:dd'-'MMM'-'yyyy H:mm tt}"
                                                            ItemStyle-Width="3000px" HeaderText="ProjectArchivedOn">
                                                            <ItemStyle HorizontalAlign="Center" Width="15" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="cArchiveForLabData" HeaderText="Lab-Report Status">
                                                            <ItemStyle HorizontalAlign="Center" Width="10" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="dLabDataModifyOn_IST" DataFormatString="{0:dd'-'MMM'-'yyyy H:mm tt}"
                                                            HeaderText="Lab-Report Archive/UnArchive On" HtmlEncode="false">
                                                            <ItemStyle HorizontalAlign="Center" Width="10" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="click" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="click" />
            <asp:AsyncPostBackTrigger ControlID="btnShowAll" EventName="click" />
            <asp:PostBackTrigger ControlID="btnImportToExcel" />
            <asp:PostBackTrigger ControlID="btnExportToPdf" />
        </Triggers>
    </asp:UpdatePanel>
    <%-- <script src="Script/jquery-1.7.min.js" type="text/javascript"></script>
--%>

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" src="Script/Gridview.js"></script>

    <script src="Script/jquery.dataTables.min.js" type="text/javascript"></script>

    <script src="Script/jquery.dataTables.plugins.js" type="text/javascript"></script>

    <link href="App_Themes/StyleBlue/StyleBlue.css" rel="stylesheet" type="text/css" />
    <%--<style type="text/css">
        .dataTables_length
        {
            padding-left: 90px;
        }
        .dataTables_filter
        {
            padding-right: 90px;
        }
        .dataTables_info
        {
            padding-left: 90px;
        }
        .dataTables_paginate
        {
            padding-right: 90px;
        }
    </style>--%>

    <script type="text/javascript">

        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }
        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
     $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        //    $('#ctl00_CPHLAMBDA_btnSetProject').click(function()
        //    {
        //        $('#ctl00_CPHLAMBDA_txtRemark').value="";
        //        $('#ctl00_CPHLAMBDA_txtYears').value="";
        //    });


        function ValidateYear(evt) {
            if (document.getElementById('<%=txtYears.ClientID%>').value.trim().length > 1) {
                msgalert("Year Should Not More Than 2 Digits");
                return false;
            }

            var charCode = evt.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            else
                return true;
        }

        function validation() {
            if (document.getElementById('<%=txtProject.ClientID%>').value.trim().length <= 0) {
                msgalert("Please Enter Project No !");
                $('#ctl00_CPHLAMBDA_txtProject').focus();
                return false;
            }

            if (document.getElementById('<%=txtRemark.ClientID%>').value.trim().length <= 0) {
                msgalert("Please Enter Remarks !");
                $('#ctl00_CPHLAMBDA_txtRemark').focus();
                return false;
            }

            if ($('#ctl00_CPHLAMBDA_hdn_prjectstatus').val() == "N") {
                if (document.getElementById('<%=txtYears.ClientID%>').value.trim().length <= 0) {
                    msgalert("Please Enter Years !");
                    $('#ctl00_CPHLAMBDA_txtYears').focus();
                    return false;
                }
            }

            if ($('#ctl00_CPHLAMBDA_hdn_prjectstatus').val() == 0) {
                if (document.getElementById('<%=txtYears.ClientID%>').value.trim().length <= 0) {
                    msgalert("Please Enter Years !");
                    $('#ctl00_CPHLAMBDA_txtYears').focus();
                    return false;
                }
                PromptForLabRptArchive();
            }


            //          if( document .getElementById ('<%=txtYears.ClientID%>').value.trim().length<=0)
            //          {
            //            alert ("Please Enter Years");
            //            $('#ctl00_CPHLAMBDA_txtYears').focus();
            //            return false;
            //          }

            if (isNaN(document.getElementById('<%=txtYears.ClientID%>').value.trim())) {
                msgalert("Please Enter Numeric Value For Years !");
                document.getElementById('<%=txtYears.ClientID%>').value = '';
                document.getElementById('<%=txtYears.ClientID%>').focus();
                return false;
            }

            if (document.getElementById('<%= HProjectId.clientid %>').value == "") {
                msgalert("Please Select Project !");
                $('#ctl00_CPHLAMBDA_txtProject').focus();
                return false;
            }

        }


        //         TableTools.DEFAULTS = {
        //                "sSwfPath": "Script/swf/copy_cvs_xls_pdf.swf",
        //                "sRowSelect": "multi",
        //                "sSelectedClass": "DTTT_selected",
        //                "fnPreRowSelect": null,
        //                "fnRowSelected": null,
        //                "fnRowDeselected": null

        //            };
        //        function pageLoad()
        //        {
        //            

        //              oTable = $('#<%= GV_ArchieveDetail.ClientID %>').prepend($('<thead>').append($('#<%= GV_ArchieveDetail.ClientID %> tr:first'))).dataTable({ //yet to test on multiple dataTable
        //                "sPaginationType": "full_numbers",
        //"bJQueryUI": true,
        //                 "bSort": true
        // "sDom": 'R,C,T<"clear">lfrtip',
        //                "sDom": '<"H"lTfrRC>t<"F"ip>',
        //                "oTableTools": {
        //			    "sSwfPath": "Script/swf/copy_cvs_xls_pdf.swf",
        //			     "sRowSelect": "multi",
        //                    "sSelectedClass": "row_selected",
        //                    "aButtons":
        //								 [
        //								     {
        //								         "sExtends": "xls",
        //								         "sCharSet": "utf16le",
        //								         "bBomInc": true,
        //								         "sFileName": "Line Listing Report.xls",
        //								         "sFieldBoundary": "",
        //								         "sFieldSeperator": "\t",
        //								         "sNewLine": "\n",
        //								         "sTitle": "Line Listing Report",
        //								         "sToolTip": "Export To Excel",
        //								         "sButtonClass": "DTTT_button_xls",
        //								         "sButtonClassHover": "DTTT_button_xls_hover",
        //								         "sButtonText": "Excel",
        //								         "mColumns": "visible",
        //								         "bSelectedOnly": true
        ////								         "fnClick": function(nButton, oConfig, flash) {
        ////								             this.fnSetText(flash, "Line Listing Report [DRAFT REPORT (Covers Cases in all Stages) Not Voided] \n\n" +
        ////			                                            this.fnGetTableData(oConfig));
        //								    
        //            
        //		                            }
        //		                      ]
        //		                      }
        //        });
        //     }

        function pageLoad() {


            var aTable = $('#<%= GV_ArchieveDetail.ClientID %>').prepend($('<thead>').append($('#<%= GV_ArchieveDetail.ClientID %> tbody tr:first '))).dataTable({
                "bStateSave": true,
                //                  "bPaginate":true,
                "bSort": false,
                "bDestory": true,
                "bRetrieve": true,
                "sPaginationType": "full_numbers"
            });
            //aTable.fnFilter('');
        }


        function EmptyGrid(ele) {

            var a = $('#ctl00_CPHLAMBDA_GV_ArchieveDetail').dataTable().fnGetNodes().length;
            if (a == 0) {
                msgalert("Data Not Available !");
                return false;
            }
            return true;
        }

        function PromptForLabRptArchive() {
            var a = confirm("Do You Want To Archive Labreports Of This Project ");
            if (a.toString() == "true") {
                document.getElementById('<%=hdn_LabArchiveFlag.ClientId %>').value = "Y";
            }

        }
            
    </script>

</asp:Content>
