<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmScreeningStudyUpdateRecord.aspx.vb" Inherits="frmScreeningStudyUpdateRecord" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <style type="text/css">
        #ctl00_CPHLAMBDA_pnlScreeningStudyRecords {
            overflow-x: inherit !important;
        }

        /*.dataTables_scrollBody {
            height: 40px !important;
        }*/
    </style>

    <script type="text/javascript" src="Script/AutoComplete.js" language="javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="Script/General.js"></script>

    <script type="text/javascript">

        function ClientPopulated(sender, e) {
            SubjectClientShowing('AutoCompleteExtender1', $get('<%= txtSubject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            SubjectOnItemSelected(e.get_value(), $get('<%= txtSubject.clientid %>'),
                $get('<%= HSubjectId.clientid %>'), document.getElementById('<%= btnsetSubject.ClientId %>'));
        }
        function ValidateDateFormat(ParamDate, txtdate) {

            try {
                if (txtdate.value != "") {
                    if (!DateConvert(ParamDate, txtdate)) {

                    }

                }
                return true;
            }
            catch (err) {
                txtdate.value = '';
                msgalert('Please Enter Proper Date!');
            }
        }
        //Pass the Date and no. of day which you want add in a date
        function getDateWithAddDay(date, day) {
            date = date.replace(/-/g, ',');
            var myDate = new Date(date);
            var month = new Array(12);
            month[0] = "Jan";
            month[1] = "Feb";
            month[2] = "Mar";
            month[3] = "Apr";
            month[4] = "May";
            month[5] = "Jun";
            month[6] = "Jul";
            month[7] = "Aug";
            month[8] = "Sep";
            month[9] = "Oct";
            month[10] = "Nov";
            month[11] = "Dec";
            myDate.setDate(myDate.getDate() + day);
            return myDate.getDate() + "-" + month[myDate.getMonth()] + "-" + myDate.getFullYear();

        }

        function UIGVScreeningRecords() {
            oTab = $('#<%= GV_ScreeningStudyRecords.ClientID%>').prepend($('<thead>').append($('#<%= GV_ScreeningStudyRecords.ClientID%> tr:first'))).dataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": false,
                "sScrollY": "300px",
                "sScrollX": "100%",
                "bScrollCollapse": true,
                aLengthMenu: [
                    [10, 25, 50, 100, -1],
                    [10, 25, 50, 100, "All"]
                ],
            });
            return false;
        }

        $(document).ready(function() {

            $('#ctl00_CPHLAMBDA_GV_ScreeningStudyRecords input[type="text"][title="Screening Date"]').live("change", function() {
                DateConvert(this.value.trim(), this);
            });

            $('#ctl00_CPHLAMBDA_GV_ScreeningStudyRecords input[type="text"][title="Next XRay Date"]').live("change", function() {
                DateConvert(this.value, this);
            });

            $('#ctl00_CPHLAMBDA_GV_ScreeningStudyRecords input[type="text"][title="XRay Date"]').live("change", function() {
                DateConvert(this.value, this);
            });

            $('#ctl00_CPHLAMBDA_GV_ScreeningStudyRecords input[type="text"][title="Screening FU Date,If Any"]').live("change", function() {
                DateConvert(this.value, this);
            });


            $('#ctl00_CPHLAMBDA_GV_ScreeningStudyRecords input[type="text"][title="Screening FU Date2,If Any"]').live("change", function() {
                ValidateDateFormat(this.value, this);
            });
            $('#ctl00_CPHLAMBDA_GV_ScreeningStudyRecords input[type="text"][title="Last Sample Date"]').live("change", function() {

                var flag = ValidateDateFormat(this.value, this);
                if (flag == true) {
                    var ele = $('input:text', $(this).parent().next().next().next());
                    if (ele.val() != '' && this.value != '') {
                        var date = getDateWithAddDay(this.value, parseInt(ele.val()));
                        $('input:text', $(ele).parent().next()).val(date);
                    }
                    else
                        $('input:text', $(ele).parent().next()).val(this.value);
                }
            });

            $('#ctl00_CPHLAMBDA_GV_ScreeningStudyRecords input[type="text"][title="Eligible date"]').live("change", function() {
                ValidateDateFormat(this.value, this);
            });
            $('#ctl00_CPHLAMBDA_GV_ScreeningStudyRecords input[type="text"][title="No.Of Days"]').live("change", function() {

                var ele = $('input:text', $(this).parent().prev().prev().prev());
                if (this.value != undefined && this.value.trim() != '' && ele.val() != undefined && ele.val().trim != '') {
                    var date = getDateWithAddDay(ele.val(), parseInt(this.value.trim()));
                    $('input:text', $(this).parent().next()).val(date);
                }
                else
                    $('input:text', $(this).parent().next()).val(ele.val());

            });
            $('#ctl00_CPHLAMBDA_GV_ScreeningStudyRecords input[type="text"][title="No.Of Days"]').live("keyup", function() {

                var no = Number(this.value);
                if (no.toString().toUpperCase() == 'NAN') {
                    this.value = '';
                    return false;
                }
                this.value = this.value.trim();

            });


        });

        function DateConvert(ParamDate, txtdate) {

            if (ParamDate.length == 0) {

                return true;
            }
            if (ParamDate.length < 8) {
                txtdate.value = '';
                txtdate.focus();
                msgalert("Please Enter Date In DDMMYYYY Or dd-Mon-YYYY Format Only !");
                return false;
            }

            if (ParamDate.length > 8) {

                var Ret_Date = GetDateFromString(ParamDate);
                if (Ret_Date == '' || isNaN(Ret_Date)) {
                    Ret_Date = new Date();
                    txtdate.value = (Ret_Date.getDate().toString().length < 2 ? '0' + Ret_Date.getDate().toString() : Ret_Date.getDate().toString())
                                + '-' + cMONTHNAMES[Ret_Date.getMonth()] + '-' + Ret_Date.getFullYear().toString();


                    return false;
                }
                return true;
            }

            var PDay = ParamDate.substr(0, 2);
            var PMonNo = ParamDate.substr(2, 2);
            var PYear = ParamDate.substr(4, 4);

            //alert(PDay);
            //alert(PMonNo);
            //alert(PYear);

            if (PDay > 31 || PMonNo > 12 || PDay < 1 || PMonNo < 1) {
                txtdate.value = '';
                txtdate.focus();
                msgalert("Please Enter Date In DDMMMYYYY Or dd-Mon-YYYY Format Only !");
                return false;
            }

            if (PYear < 1800) {
                txtdate.value = '';
                txtdate.focus();
                msgalert("Please Enter Valid Year !");
                return false;
            }

            var DMon = cMONTHNAMES[PMonNo - 1];

            ParamDate = PDay.toString() + '-' + PMonNo + '-' + PYear.toString();

            //alert(ParamDate);

            //alert(isDate(ParamDate));
            if (!isDate(ParamDate)) {
                txtdate.value = '';
                txtdate.focus();
                //alert("Please Enter Proper Date in DDMMYYYY or dd-Mon-YYYY format only.");
                return false;
            }
            txtdate.value = PDay.toString() + '-' + DMon + '-' + PYear.toString();

            return true;

        }
        function isDate(dateStr) {

            var datePat = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
            var matchArray = dateStr.match(datePat); // is the format ok?

            if (matchArray == null) {
                msgalert("Please Enter Date As Either mmm/dd/yyyy Or mmm-dd-yyyy !");
                return false;
            }

            day = matchArray[1]; // p@rse date into variables
            month = matchArray[3];
            year = matchArray[5];

            if (month < 1 || month > 12) { // check month range
                msgalert("Month Must Be Between 1 And 12 !");
                return false;
            }

            if (day < 1 || day > 31) {
                msgalert("Day must Be Between 1 And 31 !");
                return false;
            }

            if ((month == 4 || month == 6 || month == 9 || month == 11) && day == 31) {
                msgalert("Month " + month + " Doesn`t Have 31 Days!")
                return false;
            }

            if (month == 2) { // check for february 29th
                var isleap = (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0));
                if (day > 29 || (day == 29 && !isleap)) {
                    msgalert("February " + year + " doesn`t have " + day + " days!");
                    return false;
                }
            }
            return true; // date is valid
        }


    </script>

    <table width="100%" cellpadding="5px">
        <tr>
            <td class="Label" style="white-space: nowrap; height: 20px; text-align: right; width: 25%;">
                Search Subject:
            </td>
            <td style="height: 20px; text-align: left;">
                <asp:TextBox ID="txtSubject" CssClass="textBox" runat="server" Width="60%" TabIndex="1"></asp:TextBox><asp:Button
                    ID="btnSetSubject" runat="server" OnClick="btnSetProject_Click" Style="display: none"
                    Text="Subject" /><cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server"
                        TargetControlID="txtSubject" BehaviorID="AutoCompleteExtender1" CompletionListCssClass="autocomplete_list"
                        CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListItemCssClass="autocomplete_listitem"
                        MinimumPrefixLength="1" ServiceMethod="GetSubjectCompletionList_NotRejectedDataMerg"
                        UseContextKey="True" OnClientItemSelected="OnSelected" CompletionSetCount="10"
                        OnClientShowing="ClientPopulated" ServicePath="AutoComplete.asmx" CompletionListElementID="pnlSubjectList">
                    </cc1:AutoCompleteExtender>
                <asp:HiddenField ID="HSubjectId" runat="server" />
                <asp:Button ID="btnExit" runat="server" CssClass="btn btnexit" Text="Exit" ToolTip="Exit"
                    OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" TabIndex="2" />
                <asp:Panel ID="pnlSubjectList" runat="server" Style="max-height: 200px; overflow: auto;
                    overflow-x: hidden" />
            </td>
        </tr>
        <%--  <tr>
            <td align="left" class="Label" style="WHITE-SPACE: nowrap; HEIGHT: 20px">
            </td>
            <td align="left" colspan="2" style="HEIGHT: 20px">
            </td>
        </tr>--%>
        <tr>
            <td colspan="2">
                <asp:UpdatePanel ID="Up_View" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table width="100%">
                            <tbody>
                                <tr>
                                    <td style="text-align: center;">
                                        <asp:Button class="btn btnnew" Visible="false" Text="Add New Record" ID="btnAddNewRecord"
                                            runat="server" ToolTip="Add New Record " />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlScreeningStudyRecords" runat="server" ScrollBars="Horizontal" Width="1000px"
                                            Style="margin: auto;">
                                            <asp:GridView ID="GV_ScreeningStudyRecords" runat="server" style="width:80%; margin:auto;" OnPageIndexChanging="GV_ScreeningStudyRecords_PageIndexChanging"
                                                PageSize="25" AllowPaging="True" OnRowDataBound="GV_ScreeningStudyRecords_RowDataBound"
                                                OnRowCreated="GV_ScreeningStudyRecords_RowCreated" AutoGenerateColumns="False"
                                                AutoGenerateEditButton="true">
                                                <%--SkinID="grdViewAutoSizeMax"--%>
                                                <Columns>
                                                    <asp:BoundField HeaderText=" # " ReadOnly="true"></asp:BoundField>
                                                    <asp:BoundField DataField="nSubjectScreeningRecordNo" HeaderText="SubjectScreeningRecordNo">
                                                    </asp:BoundField>
                                                    <%--<asp:BoundField DataField="vSubjectId" HeaderText="SubjectId"></asp:BoundField>--%>
                                                    <asp:BoundField DataField="iTranNo" HeaderText="Tran No"></asp:BoundField>
                                                    <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd-MMM-yyyy}" DataField="dScreeningDate"
                                                        HeaderText="Screening Date">
                                                        <ItemStyle Wrap="false" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vScreeningRecordedBy" HeaderText="Screening Recorded By">
                                                    </asp:BoundField>
                                                    <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd-MMM-yyyy}" DataField="dXRayDate"
                                                        HeaderText="XRay Date">
                                                        <ItemStyle Wrap="false" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd-MMM-yyyy}" DataField="dNextXRayDate"
                                                        HeaderText="Next XRay Date">
                                                        <ItemStyle Wrap="false" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd-MMM-yyyy}" DataField="dScreeningFUDate"
                                                        HeaderText="Screening FU Date,If Any">
                                                        <ItemStyle Wrap="false" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd-MMM-yyyy}" DataField="dScreeningFUDate2"
                                                        HeaderText="Screening FU Date2,If Any">
                                                        <ItemStyle Wrap="false" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vRemark" HeaderText="Remarks"></asp:BoundField>
                                                    <asp:TemplateField HeaderText="IsSelected">
                                                        <ItemTemplate>
                                                            <asp:DropDownList Width="99%" class="dropDownList" ID="ddlIsSelected" DataValueField='<%# Bind("cSelectedFlag") %>'
                                                                runat="server">
                                                                <asp:ListItem Text="Select" Value="S"></asp:ListItem>
                                                                <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="vReasonForRejection" HeaderText="Reason For Rejection">
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Is Paricipated">
                                                        <ItemTemplate>
                                                            <asp:DropDownList Width="99%" class="dropDownList" ID="ddlIsParticipated" DataValueField='<%# Bind("cEnrolledFlag") %>'
                                                                runat="server">
                                                                <asp:ListItem Text="Select" Value="S"></asp:ListItem>
                                                                <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="vReasonForNotParticipated" HeaderText="Reason For Not Participated">
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vProjectNo" HeaderText="ProjectNo" />
                                                    <asp:BoundField DataField="vSubjectNo" HeaderText="SubjectNo" />
                                                    <asp:BoundField ReadOnly="true" DataField="vRecordedBy" HeaderText="Recorded By" />
                                                    <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd-MMM-yyyy}" DataField="dLastSampleDate"
                                                        HeaderText="Last Sample Date">
                                                        <ItemStyle Wrap="false" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Study Completed">
                                                        <ItemTemplate>
                                                            <asp:DropDownList Width="99%" class="dropDownList" ID="ddlStudyCompleted" DataValueField='<%# Bind("cStudyCompleted") %>'
                                                                runat="server">
                                                                <asp:ListItem Text="Select" Value="S"></asp:ListItem>
                                                                <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="vReason" HeaderText="Reason For Non Completion" />
                                                    <asp:BoundField DataField="nNoOfDays" HeaderText="No.Of Days" />
                                                    <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd-MMM-yyyy}" DataField="dEligibledate"
                                                        HeaderText="Eligible date">
                                                        <ItemStyle Wrap="false" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vSampleRecordedBy" HeaderText="Recorded By" />
                                                    <asp:BoundField DataField="vVerifiedBy" HeaderText="Verified By" Visible ="false"/>
                                                    <asp:BoundField DataField="vCheckedBy" HeaderText="Checked By" Visible ="false" />
                                                    </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <table>
                            <tr>
                                <td style="text-align: center;">
                                    <asp:Button Width="15%" class="button" Visible="false" Text="Save Changes" ToolTip="Save Changes"
                                        ID="btnSave" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
