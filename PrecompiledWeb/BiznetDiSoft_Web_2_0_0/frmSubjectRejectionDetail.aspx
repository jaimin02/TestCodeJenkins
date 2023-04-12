<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmSubjectRejectionDetail, App_Web_ybumpksz" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

        .visibletable {
            display: none;
        }

        .WrapStyle {
            word-break: break-all;
        }
    </style>
    <input type="hidden" id="hdDtValue" />
    <table width="100%" cellpadding="5px">
        <tr>
            <td style="text-align: right; width: 30%;" class="Label">Subject* :
            </td>
            <td align="left">
                <asp:TextBox ID="txtSubject" runat="server" CssClass="textBox" TabIndex="1" Width="50%"></asp:TextBox>
                <asp:Button ID="btnSubject" runat="server" Style="display: none" Text="Subject" />
                <asp:HiddenField ID="HSubjectId" runat="server" />
                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                    MinimumPrefixLength="1" OnClientItemSelected="OnSelected" OnClientShowing="ClientPopulated"
                    ServiceMethod="GetSubjectCompletionList_NotRejected_BlockPeriodDataMerg" ServicePath="AutoComplete.asmx" TargetControlID="txtSubject"
                    UseContextKey="True" CompletionListElementID="pnlSubjectList" CompletionListItemCssClass="autocomplete_listitem"
                    CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem">
                </cc1:AutoCompleteExtender>
                <asp:Panel ID="pnlSubjectList" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" />
            </td>
        </tr>


        <tr>
            <td style="text-align: right" class="Label">From Date :
            </td>
            <td align="left">
                <asp:TextBox ID="txtfromDate" runat="server" CssClass="textBox clsClear" Width="10%"></asp:TextBox>

                <asp:Label ID="Label1" runat="server" CssClass="Label">To Date : </asp:Label>
                <asp:TextBox ID="txttoDate" runat="server" CssClass="textBox clsClear" Width="10%"></asp:TextBox>

            </td>
        </tr>


        <tr>
            <td style="text-align: right;" class="Label">Remarks* :
            </td>
            <td style="text-align: left;">
                <asp:TextBox ID="txtRemark" runat="server" CssClass="textBox" Height="40px" TextMode="MultiLine"
                    Width="30%" TabIndex="2"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="text-align: right;" class="Label">Permanent Block:
            </td>
            <td style="text-align: left;">
                <asp:RadioButtonList ID="rblReject" runat="server" RepeatDirection="Horizontal" TabIndex="3" Visible="false">
                    <asp:ListItem Selected="True" Value="Y">Yes</asp:ListItem>
                    <asp:ListItem Value="N">No</asp:ListItem>
                </asp:RadioButtonList>
                <asp:CheckBox ID="chkreject" runat="server" />

            </td>
        </tr>
        <tr>
            <td style="text-align: right;" class="Label">
                <asp:Label ID="lblRejectedBY" runat="server" Text="Rejected By:"></asp:Label>
            </td>
            <td class="Label " style="text-align: left;">
                <asp:Label ID="lblRejectedByUser" runat="server" TabIndex="4"></asp:Label>
            </td>
        </tr>
        <tr>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td style="text-align: center;" colspan="2">
                <input id="btnHistory" type="button" value="History" onclick="ShowAudit(); return" class="btn btnnew" style="Width: 60px" />

                <asp:Button ID="btnSave" runat="server" CssClass="btn btnsave" Text="Save" ToolTip="Save"
                    TabIndex="5" OnClientClick="return Validation(); " style="Width: 60px" />

                <asp:Button ID="btnExport" OnClientClick="return CheckData();" runat="server" 
                    CssClass="btn btnexcel" ToolTip="Export" />

                <asp:Button ID="BtnCancel" runat="server" CssClass="btn btncancel" Text="Cancel" ToolTip="Cancel"
                    TabIndex="6" style="Width: 60px"/>                
                

                <asp:Button ID="BtnExit" runat="server" CssClass="btn btnexit" Text="Exit" ToolTip="Exit"
                   OnClientClick="return exit(this)" TabIndex="7" style="Width: 60px"/>
            </td>
        </tr>
    </table>

    <input id="btnchkaudit" runat="server" style="width: 180px; display: none;" class="button" />

    <cc1:ModalPopupExtender ID="mpeAuditTrail" runat="server" PopupControlID="divAuditTrail"
        BackgroundCssClass="modalBackground" BehaviorID="mpeAuditTrail" TargetControlID="btnchkaudit"
        CancelControlID="imgAuditClose">
    </cc1:ModalPopupExtender>


    <div id="divAuditTrail" runat="server" class="centerModalPopup" style="display: none; width: 96%; position: absolute; top: 400px; max-height: 100%;">
        <table width="96%">
            <tr>
                <td class="LabelText" style="text-align: center !important; font-size: 12px !important; width: 100%;">
                    <asp:Label ID="lblPopUp" runat="server" Text="Audit Trail"></asp:Label>
                </td>
                <td style="width: 3%; text-align: Right !important;">
                    <img id="imgAuditClose" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';"
                        title="Close" onclick="return DeleteHistory();" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>

            <tr>
                <td>Subject* :  
                            <asp:TextBox ID="txtSubjectaudit" runat="server" CssClass="textBox" TabIndex="1" Width="50%"></asp:TextBox>

                    <asp:HiddenField ID="HSubjectIdAudit" runat="server" />
                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" BehaviorID="AutoCompleteExtender2"
                        MinimumPrefixLength="1" OnClientItemSelected="OnSelectedAudit" OnClientShowing="ClientPopulatedAudit"
                        ServiceMethod="GetSubjectCompletionList_NotRejected_BlockPeriodDataMerg" ServicePath="AutoComplete.asmx" TargetControlID="txtSubjectaudit"
                        UseContextKey="True" CompletionListElementID="pnlSubjectListAudit" CompletionListItemCssClass="autocomplete_listitem"
                        CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem">
                    </cc1:AutoCompleteExtender>
                    <asp:Panel ID="pnlSubjectListAudit" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" />

                </td>
            </tr>

            <tr>
                <td colspan="2" id="tdgvaudit" runat="server">                   
                    <div id="divgrdAudit" style="width: 101%;">
                    <asp:HiddenField ID="hdnExportAuditdata" runat="server" />
                    <table id="tblAudit" class="tblAudit" style="background-color: #d0e4f7 !important; color: white"></table>

                        
                     </div>
                      <asp:Button ID="btnexportAudit" runat="server" CssClass="btn btnexcel"
                        OnClientClick="return CheckAuditData();" Style="font-size: 12px !important;margin-top: 10px;" TabIndex="56" />
                </td>

            </tr>
            
        </table>
    </div>

    <table width="100%">
        <tr>
            <td>
                <asp:UpdatePanel ID="P_SubjectRejection" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div style="width: 93%; padding-left: 3.7%;">
                            <asp:GridView ID="Gv_SubjectRejection" runat="server" Width="100%" AutoGenerateColumns="False"
                                PageSize="25" CellPadding="3" ShowFooter="false" Style="margin: auto;" CssClass="visibletable">
                                <RowStyle BackColor="#cee3ed" Font-Names="Verdana" VerticalAlign="Middle" HorizontalAlign="left"
                                    Font-Size="9pt" ForeColor="navy" />
                                <EditRowStyle BackColor="#cee3ed" Font-Names="Verdana" Font-Size="9pt" VerticalAlign="Middle" />
                                <HeaderStyle Font-Underline="False" BackColor="#1560a1" Font-Names="Verdana" VerticalAlign="Top"
                                    Font-Size="10pt" HorizontalAlign="Center" ForeColor="white" Font-Bold="True" />
                                <FooterStyle BackColor="#1560a1" Font-Names="Verdana" Font-Size="X-Small" HorizontalAlign="Center"
                                    ForeColor="white" Font-Bold="True" />
                                <AlternatingRowStyle BackColor="white" Font-Names="Verdana" HorizontalAlign="left"
                                    Font-Size="9pt" ForeColor="navy" />
                                <PagerStyle ForeColor="#ffa24a" Font-Underline="False" BackColor="white" Font-Bold="True"
                                    Font-Names="Verdana" HorizontalAlign="Center" Font-Size="X-Small" />
                                <Columns>
                                    <asp:BoundField DataField="vSubjectId" HeaderText="Subject ID" />
                                    <asp:BoundField DataField="FullName" HeaderText="Subject Name" />
                                    <asp:BoundField DataField="dBirthDate" DataFormatString="{0:dd-MMM-yy}" HeaderText="D.O.B"
                                        HtmlEncode="False" />
                                    <asp:BoundField DataField="dModiFyOn" DataFormatString="{0:dd-MMM-yyy}" HeaderText="Rejection On" />
                                    <asp:BoundField DataField="vModifyBy" HeaderText="Rejection By" />
                                    <asp:BoundField DataField="vRemark" HeaderText="Remarks" />
                                    <asp:TemplateField HeaderText="LabReport" SortExpression="status">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgLabRpt" runat="server" ToolTip="Lab Report" ImageUrl="~/Images/Labrpt.gif"
                                                CommandName="Lab Report" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText=" View PIF" SortExpression="status">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="LnkPIF" runat="server" ToolTip="PIF Details" ImageUrl="~/Images/view.gif"
                                                CommandName="Details PIF" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="View MSR" SortExpression="status">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="LnkMSR" runat="server" ToolTip="Details MSR" ImageUrl="~/Images/view.gif"
                                                CommandName="DETAILS MSR" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Audit Trail">
                                        <ItemTemplate>                                           
                                            <img src="Images/audit.png"  onmouseover="this.style.cursor='pointer';" onclick="GetAudit(this.id);" id='<%# Eval("vSubjectID")%>' />
                                            
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>                    
                </asp:UpdatePanel>              
            </td>
        </tr>
    </table>

    <script src="Script/scrollablegrid.js" type="text/javascript"></script>

    <script src="Script/General.js" language="javascript" type="text/javascript"></script>


    <script src="Script/jquery-1.11.3.min.js" language="javascript" type="text/javascript"></script>

    <script src="Script/jquery-1.4.3.min.js" type="text/javascript"></script>

    <script src="Script/jquery.dataTables.min.js" type="text/javascript"></script>

    <script src="Script/FixedHeader.min.js" type="text/javascript"></script>

    <script src="Script/Validation.js" language="javascript" type="text/javascript"></script>

    <script src="Script/popcalendar.js" language="javascript" type="text/javascript"></script>

    <script src="Script/AutoComplete.js" language="javascript" type="text/javascript"></script>

    <script src="Script/jquery-ui.js" language="javascript" type="text/javascript"></script>

    <script src="Script/jquery.dataTables.plugins.js" type="text/javascript"></script>



    <script type="text/javascript">

        function pageLoad() {

            oTab = $('#<%= Gv_SubjectRejection.ClientID%>').prepend($('<thead>').append($('#<%= Gv_SubjectRejection.ClientID%> tr:first'))).dataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": false,
                aLengthMenu: [
                    [10, 25, 50, 100, -1],
                    [10, 25, 50, 100, "All"]
                ],
            });
            $('#<%= Gv_SubjectRejection.ClientID%>').removeClass("visibletable");
            $('#<%= Gv_SubjectRejection.ClientID%>').removeAttr('style', 'display:block;');
            return false;
        }

        function Validation() {

            if (document.getElementById('<%= HSubjectId.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Subject');
                document.getElementById('<%= txtSubject.ClientId %>').focus();
                document.getElementById('<%= txtSubject.ClientId %>').value = '';
                return false;
            } // added by vishal for validation in remarks 
            if (document.getElementById('<%= txtRemark.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Remarks');
                document.getElementById('<%= txtRemark.ClientId %>').focus();
                document.getElementById('<%= txtRemark.ClientId %>').value = '';
                return false;
            }

            if (document.getElementById('<%= chkreject.ClientID%>').checked == true) {
                if (document.getElementById('<%= txtfromDate.ClientID%>').value.toString().trim() != '' || document.getElementById('<%= txttoDate.ClientID%>').value.toString().trim() != '') {
                    msgalert("Please Select Either Date Range Or Permanent Blocking !");
                    return false;
                }
            }
            else {
                if (document.getElementById('<%= txtfromDate.ClientID%>').value.toString().trim() == '' || document.getElementById('<%= txttoDate.ClientID%>').value.toString().trim() == '') {
                    msgalert("Please Select Either Date Range Or Permanent Blocking!");
                    return false;
                }

            }
            if (document.getElementById('<%= txtfromDate.ClientID%>').value.toString().trim() != '' && document.getElementById('<%= txttoDate.ClientID%>').value.toString().trim() != '') {
                var dtValuefrom = document.getElementById('<%= txtfromDate.ClientID%>').value.toString().trim();
                var dtValueto = document.getElementById('<%= txttoDate.ClientID%>').value.toString().trim();

                if (ValidateDateformat(dtValuefrom) == false) {
                    msgalert("Please Enter Valid From Date!");
                    return false;
                }
                if (ValidateDateformat(dtValueto) == false) {
                    msgalert("Please Enter Valid To Date!");
                    return false;
                }
                if (validate() == false) {
                    return false;
                }
            }

            return true;

        }


        function ClientPopulated(sender, e) {
            SubjectClientShowing('AutoCompleteExtender1', $get('<%= txtSubject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            SubjectOnItemSelected(e.get_value(), $get('<%= txtSubject.clientid %>'),
                $get('<%= HSubjectId.clientid %>'), document.getElementById('<%= btnSubject.ClientId %>'));
        }
        function ClientPopulatedAudit(sender, e) {
            SubjectClientShowing('AutoCompleteExtender2', $get('<%= txtSubjectaudit.ClientID%>'));
        }

        function OnSelectedAudit(sender, e) {
            SubjectOnItemSelected(e.get_value(), $get('<%= txtSubjectaudit.clientid %>'),
             $get('<%= HSubjectIdAudit.clientid %>'));
            GetSubjectAudit();
        }


        function ValidateDateformat(dtValue) {
            var dtRegex = new RegExp("^([0]?[1-9]|[1-2]\\d|3[0-1])-(JAN|FEB|MAR|APR|MAY|JUN|JULY|AUG|SEP|OCT|NOV|DEC)-[1-2]\\d{3}$", 'i');
            return dtRegex.test(dtValue);
        }
        function ShowAudit() {
            if ($("#tblAudit").children().length > 0) {
                $("#tblAudit").dataTable().fnDestroy();
            }
            $(tblAudit).remove();
            $(divgrdAudit).append('<table id="tblAudit" class="tblAudit" style="background-color: #d0e4f7 !important; color: white"></table>')

            document.getElementById("ctl00_CPHLAMBDA_txtSubjectaudit").value = ''
            document.getElementById("ctl00_CPHLAMBDA_HSubjectIdAudit").value = ''

            document.getElementById('<%=btnexportAudit.ClientID%>').style.display = 'none';

            $find('mpeAuditTrail').show();
            return true
        }

        function CheckData() {
            if ($('[id$="' + '<%= Gv_SubjectRejection.ClientID%>' + '"] tbody tr').length > 0) {
                return true
            }
            else {
                msgalert("No Record Found For Export !")
                return false
            }
        }
        function CheckAuditData() {
            if ($('#<%= hdnExportAuditdata.ClientID%>').val() == '[]') {
                msgalert("No Record Found For Export !")
                return false
            }
        }
        function DeleteHistory() {
            $('#<%= hdnExportAuditdata.ClientID%>').val("");
            return false
        }
        $(function () {
            $('#<%= txtfromDate.ClientId %>').datepicker({
                dateFormat: "dd-M-yy",
                onSelect: function (dateText, inst) {
                    validate()
                }

            });


        });

            $(function () {

                $('#<%= txttoDate.ClientID%>').datepicker({
                    dateFormat: "dd-M-yy",
                    onSelect: function (dateText, inst) {
                        $('#<%= chkreject.ClientID%>').attr("disabled", true);
                        $('#<%= chkreject.ClientID%>').attr("checked", false);
                        validate()
                    }
                });
            });


            $('.clsClear').keyup(function () {

                if (!$('.clsClear').val()) {
                    $('#<%= chkreject.ClientID%>').attr("disabled", false);
                    $('#<%= chkreject.ClientID%>').attr("checked", true);

                }

            });

            function validate() {
                var startDate = document.getElementById("ctl00_CPHLAMBDA_txtfromDate").value
                var endDate = document.getElementById("ctl00_CPHLAMBDA_txttoDate").value

                currdate = new Date()
                currdate.setHours(0, 0, 0, 0, 0);
                startDate = new Date(startDate)
                endDate = new Date(endDate)



                if (startDate != "" && endDate != "") {
                    if (startDate > endDate) {
                        document.getElementById('<%= txtfromDate.ClientId %>').value = '';
                        document.getElementById('<%= txttoDate.ClientId %>').value = '';
                        $('#<%= chkreject.ClientID%>').attr("disabled", false);
                        $('#<%= chkreject.ClientID%>').attr("checked", true);
                        msgalert("Invalid Date Range !")
                        return false
                    }
                }
                if (startDate != "") {
                    if (startDate < currdate) {
                        document.getElementById('<%= txtfromDate.ClientID%>').value = '';
                        document.getElementById('<%= txttoDate.ClientId %>').value = '';
                        $('#<%= chkreject.ClientID%>').attr("disabled", false);
                        $('#<%= chkreject.ClientID%>').attr("checked", true);
                        msgalert("From Date should be greater than or equal to current date !")
                        return false
                    }
                }
                if (endDate != "") {
                    if (endDate < currdate) {
                        document.getElementById('<%= txtfromDate.ClientID%>').value = '';
                        document.getElementById('<%= txttoDate.ClientID%>').value = '';
                        $('#<%= chkreject.ClientID%>').attr("disabled", false);
                        $('#<%= chkreject.ClientID%>').attr("checked", true);
                        msgalert("To Date should be greater than or equal to current date !")
                        return false
                    }
                }

            }

            function GetAudit(SubjectId) {
                document.getElementById('<%= HSubjectIdAudit.ClientID%>').value = SubjectId;
            document.getElementById('<%= txtSubjectaudit.clientid %>').value = SubjectId;
            GetSubjectAudit()
        }
        function exit(e) {

            msgConfirmDeleteAlert(null, "Are You Sure You Want To Exit?", function (isConfirmed) {
                if (isConfirmed) {
                    __doPostBack(e.name, '');
                    window.location = "frmMainPage.aspx";
                    return false;
                };
            });
            return false;

            //if (confirm("Are You Sure You Want To Exit?")) {
            //    window.location = "frmMainPage.aspx";
            //    return false
            //}
            //else {
            //    return false
            //}
        }

        function GetSubjectAudit() {

            var SubjectId = $('#<%= HSubjectIdAudit.ClientID%>').val();
                $.ajax({
                    type: "post",
                    url: "frmSubjectRejectionDetail.aspx/GetSubjectAudit",
                    data: '{"SubjectId":"' + SubjectId + '"}',
                    contentType: "application/json; charset=utf-8",
                    datatype: JSON,
                    async: false,
                    dataType: "json",
                    success: function (data) {
                        $('#tblAudit').attr("IsTable", "has");
                        var aaDataSet = [];
                        var RowId

                        $('#<%= hdnExportAuditdata.ClientID%>').val("");
                        $('#<%= hdnExportAuditdata.ClientID%>').val(data.d);
                        document.getElementById('<%=btnexportAudit.ClientID%>').style.display = '';

                        if (data.d != "" && data.d != null) {
                            data = JSON.parse(data.d);
                            for (var Row = 0; Row < data.length; Row++) {
                                var InDataSet = [];
                                RowId = Row + 1
                                InDataSet.push(data[Row].SrNo, data[Row].vSubjectId, data[Row].FullName, data[Row].cRejectionFlag, data[Row].vRemark, data[Row].dBlockfrom, data[Row].dBlockto, data[Row].ModifyBy, data[Row].dModifyOn);
                                aaDataSet.push(InDataSet);
                            }


                            if ($("#tblAudit").html() != "") {
                                $("#tblAudit").dataTable().fnDestroy();
                                $("#tblAudit").empty();
                            }
                            $(tblAudit).remove();
                            $(divgrdAudit).append('<table id="tblAudit" class="tblAudit" style="background-color: #d0e4f7 !important; color: white"></table>')

                            $('#tblAudit').css('background-color', '#73B1E7');
                            oTable = $('#tblAudit').prepend($('<thead>').append($('#tblAudit tr:first'))).dataTable({
                                "bJQueryUI": true,
                                "sPaginationType": "full_numbers",
                                "bLengthChange": true,
                                "aLengthMenu": [[3, 5, 10, -1], [3, 5, 10, "All"]],
                                "stateSave": true,
                                "bProcessing": true,
                                "bSort": true,
                                "autoWidth": false,
                                "iDisplayLength": 5,
                                "aaData": aaDataSet,
                                "bInfo": true,
                                "aoColumns": [
                                    { "sTitle": "SrNo" },
                                    { "sTitle": "SubjectId" },
                                    { "sTitle": "SubjectName" },
                                    { "sTitle": "Permanent Block" },
                                    { "sTitle": "Remarks" },
                                    { "sTitle": "FromDate" },
                                    { "sTitle": "ToDate" },
                                    { "sTitle": "Modify By" },
                                    { "sTitle": "Modify On" },
                                ],
                                "oLanguage": {
                                    "sEmptyTable": "No Record Found",
                                },
                                "fnCreatedRow": function (nRow, aData, iDataIndex) {    // added tooltip
                                    $.each(aData, function (index) {
                                        if (aData[index] != null) {
                                            if (aData[index].length > 35) {
                                                var abc = aData[index];
                                                var lmn = abc.substring(1, 35);
                                                $('td:eq(' + index + ')', nRow).html("");
                                                $('td:eq(' + index + ')', nRow).append("<input type='image' id='imgExpand_" + iDataIndex + "' name='imgExpand$" + iDataIndex + "' title ='" + aData[index] + "' src='images/question.gif' disabled style='border-width:0px; padding-right:5px; '>" + lmn);
                                                abc = "";
                                                lmn = "";
                                            }
                                        }
                                    });
                                },
                            });

                            $find('mpeAuditTrail').show();
                        }
                    },
                    failure: function (response) {
                        //alert(response.d);
                    },
                    error: function (response) {
                        //alert(response.d);
                    }

                    /* Apply the tooltips */

                });
            }

    </script>

</asp:Content>
