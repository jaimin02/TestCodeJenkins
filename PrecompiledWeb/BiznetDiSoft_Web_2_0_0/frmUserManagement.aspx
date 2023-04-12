<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmUserManagement, App_Web_pna05jsx" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <link rel="stylesheet" type="text/css" href="App_Themes/smoothnessjquery-ui.css" />
    <link href="App_Themes/StyleBlue/StyleBlue.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Script/General.js"></script>

    <script src="Script/Validation.js" language="javascript" type="text/javascript"></script>

    <script type="text/javascript" src="Script/Gridview.js"></script>

    <script src="Script/popcalendar.js" language="javascript" type="text/javascript"></script>

    <script src="Script/jquery.min.js" type="text/javascript"></script>

    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>

    <script src="Script/jquery.searchabledropdown-1.0.7.min.js" type="text/javascript"></script>

    <script src="Script/jquery.dataTables.plugins.js" type="text/javascript"></script>


    <script type="text/javascript" language="javascript">
        function pageLoad() {
            $("#ctl00_CPHLAMBDA_ddlUserEdit").searchable({
                width: '260'
            });

            //$(window).width() > 1180 ? wid = ($(window).width() - 94) + 'px' : wid = $(window).width() - 100 + 'px';
            //$('#<%= gvwUserType.ClientID%>').attr("style", "width:" + '500px' + " !important;")

            if ($('#<%= gvwUserType.ClientID%>')) {
                $('#<%= gvwUserType.ClientID%>').prepend($('<thead>').append($('#<%= gvwUserType.ClientID%> tr:first')))
                    .dataTable({
                        "sScrollY": '250px',
                        "sScrollX": '70%',
                        "bJQueryUI": true,
                        "bPaginate": false,
                        "bFooter": false,
                        "bHeader": false,
                        "bAutoWidth": false,
                        "Width": '65%',
                        "bSort": false,
                        "sDom": '<"H"frT>t<"F"i>',
                        //"oLanguage": { "sSearch": "Search" },
                        "oTableTools": {
                            "aButtons": [
                                "xls"
                            ],
                            "sSwfPath": "Script/swf/copy_cvs_xls_pdf.swf"
                        }
                    });

            }
            //$('[id$="gvwUserType_wrapper"]').find(".dataTables_filter").find(":text").unbind().bind("input", function (e) {
            //    var searchval = $(this).val();
            //    searchval = searchval.replace(/,/gi, "|")
            //    //$('[id$="gvwUserType"]').dataTable().fnFilter(searchval, 7, true, true)
            //})

            $('#ctl00_CPHLAMBDA_chkEmail').change(function () {
                var chkmfa = $('#ctl00_CPHLAMBDA_chkmfa');
                var chkEmail = $('#ctl00_CPHLAMBDA_chkEmail');
                var chksms = $('#ctl00_CPHLAMBDA_chksms');
                if (chkEmail.attr('checked') == false && chksms.attr('checked') == false) {
                    chkmfa.attr('checked', false);
                } else {
                    chkmfa.attr('checked', true);
                }
            });

            $('#ctl00_CPHLAMBDA_chksms').change(function () {
                var chkmfa = $('#ctl00_CPHLAMBDA_chkmfa');
                var chkEmail = $('#ctl00_CPHLAMBDA_chkEmail');
                var chksms = $('#ctl00_CPHLAMBDA_chksms');
                if (chkEmail.attr('checked') == false && chksms.attr('checked') == false) {
                    chkmfa.attr('checked', false);
                } else {
                    chkmfa.attr('checked', true);
                }
            });
        }

        function gridviewclick() {
            $find('ModalRemarks').show();
        }

        function Validation() {

            if (document.getElementById('<%=txtUserAdd.ClientID%>').value == '') {
                msgalert('Please Enter User Name !');
                return false;
            }

            else if (document.getElementById('<%=DDLScopeMst.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Scope Name !');
                return false;

            }

            else if (document.getElementById('<%=TxtFirstName.ClientID%>').value == '') {
                msgalert('Please Enter First Name !');
                return false;

            }

            else if (document.getElementById('<%=TxtLastName.ClientID%>').value == '') {
                msgalert('Please Enter Last Name !');
                return false;

            }

            else if (document.getElementById('<%=DDLDept.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Department !');
                return false;

            }
            else if (document.getElementById('<%=DDLLocation.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Location !');
                return false;

            }
            else if (document.getElementById('<%=txtloginfrom.ClientID%>').value.toString().trim().length <= 0 && document.getElementById('<%=txtLoginTo.ClientID%>').value.toString().trim().length != 0) {
                msgalert('Please Enter Valid From Date !');
                document.getElementById('<%=txtloginfrom.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtLoginTo.ClientID%>').value.toString().trim().length <= 0 && document.getElementById('<%=txtloginfrom.ClientID%>').value.toString().trim().length != 0) {
                msgalert('Please Enter Valid To Date !');
                document.getElementById('<%=txtLoginTo.ClientID%>').focus();
                return false;
            }


            else {
                return true;
            }
}

function EditValidation() {

    if (document.getElementById('<%=ddlUserEdit.ClientID%>').selectedIndex == 0) {
        msgalert('Please Select UserName !');
        return false;

    }
    else if (document.getElementById('<%=TxtFirstName.ClientID%>').value == '') {
        msgalert('Please Enter First Name !');
        return false;

    }

    else if (document.getElementById('<%=TxtLastName.ClientID%>').value == '') {
        msgalert('Please Enter Last Name !');
        return false;

    }
    else if (document.getElementById('<%=DDLDept.ClientID%>').selectedIndex == 0) {
        msgalert('Please Select Department !');
        return false;

    }
    else if (document.getElementById('<%=DDLLocation.ClientID%>').selectedIndex == 0) {
        msgalert('Please Select Location !');
        return false;

    }
    else if (document.getElementById('<%=DDLScopeMst.ClientID%>').selectedIndex == 0) {
        msgalert('Please Select Scope Name !');
        return false;

    }
    else if (document.getElementById('<%=txtloginfrom.ClientID%>').value.toString().trim().length <= 0 && document.getElementById('<%=txtLoginTo.ClientID%>').value.toString().trim().length != 0) {
        msgalert('Please Enter Valid From Date !');
        document.getElementById('<%=txtloginfrom.ClientID%>').focus();
        return false;
    }
    else if (document.getElementById('<%=txtLoginTo.ClientID%>').value.toString().trim().length <= 0 && document.getElementById('<%=txtloginfrom.ClientID%>').value.toString().trim().length != 0) {
        msgalert('Please Enter Valid To Date !');
        document.getElementById('<%=txtLoginTo.ClientID%>').focus();
        return false;
    }


    else {
        return true;
    }
}

function ValidateUserName(uName) {
    var regExp = /\s/;


    if (uName.match(regExp)) {
        document.getElementById('<%=txtUserAdd.ClientID%>').value = '';
        msgalert('User Name Cannot Contain White Spaces !');
        document.getElementById('<%=txtUserAdd.ClientID%>').focus();
        return false;
    }
    else {
        return validateSpecialCharacter();
    }

}
function validateSpecialCharacter() {
    var i;
    var chr;
    var iChars = "!@#$%^&*()+=-[]\\\';,./{}|\":<>?_";
    for (var i = 0; i < document.getElementById('<%=txtUserAdd.ClientID%>').value.length; i++) {
        chr = document.getElementById('<%=txtUserAdd.ClientID%>').value.charAt(i);

        for (var j = 0; j < iChars.length; j++) {
            if (iChars.charAt(j) == chr) {
                msgalert("The Box Has Special Characters. \nThese Are Not Allowed.\n");
                return false;
            }

        }
    }

    return true;


}
function dateformatvalidator(mode) {

    if (mode == 1) {
        var str = 'ctl00_CPHLAMBDA_txtloginfrom';
    }
    else {
        var str = 'ctl00_CPHLAMBDA_txtLoginTo';
    }
    if (document.getElementById(str).value != "") {
        var textbox = document.getElementById(str);
        var currentDate = new Date();
        var date = currentDate.getDate() + "-" + cMONTHNAMES[currentDate.getMonth()] + "-" + currentDate.getFullYear();
        DateConvert(textbox.value, textbox);
        var difference = GetDateDifference(textbox.value, date);
        if (difference.Days > 0) {
            msgalert('Valid From/Valid To Should Not Be less Than Current Date !');
            document.getElementById(str).value = '';
            document.getElementById(str).focus();
            return false;
        }
    }
    if (document.getElementById('ctl00_CPHLAMBDA_txtloginfrom').value != "" && document.getElementById('ctl00_CPHLAMBDA_txtLoginTo').value != "") {
        var fromdate = document.getElementById('ctl00_CPHLAMBDA_txtloginfrom');
        var todate = document.getElementById('ctl00_CPHLAMBDA_txtLoginTo');
        var gap = GetDateDifference(fromdate.value, todate.value);
        if (gap.Days < 0) {
            msgalert('Valid To Must Not Be Less Than Valid From Date !');
            document.getElementById(str).value = '';
            document.getElementById(str).focus();
            return false;
        }
    }
}
function Updating() {
    onUpdating();
}

function validation21() {
    if (document.getElementById('<%= txtRemarks.ClientId %>').value.toString().trim().length <= 0) {
        document.getElementById('<%= lblError.ClientID%>').style.display = '';
        msgalert('Please Enter Remarks !')
        $find('ModalRemarks').show()
        return false
    }
    var btn = document.getElementById('<%= btnSave.ClientId()%>')
    btn.click();
    return true
}

function AuditTrail(id) {
    //var UserName = document.getElementById('<%= ddlUserEdit.ClientID()%>').value
    var UserName = ""
    var ProfileName = id


    $.ajax({
        type: "post",
        url: "frmUserManagement.aspx/AuditTrailForActiveInActiveUser",
        data: '{"ProfileName":"' + ProfileName + '","UserName":"' + UserName + '"}',
        contentType: "application/json; charset=utf-8",
        datatype: JSON,
        async: false,
        success: function (data) {
            $('#tblAudit').attr("IsTable", "has");
            var aaDataSet = [];
            var RowId
            if (data.d != "" && data.d != null) {
                data = JSON.parse(data.d);
                for (var Row = 0; Row < data.length; Row++) {
                    var InDataSet = [];
                    RowId = Row + 1
                    //if (data[Row].vUserTypeCode == '') {
                    //    data[Row].vUserTypeCode = data[0].vUserTypeCode
                    //}
                    var Location = (data[Row].IST.split(" ")[2].split("(")[1] == "+05:30") ? "India standard time " : "Eastern standard time";  

                    InDataSet.push(RowId, data[Row].vUserTypeCode, data[Row].vRemark, data[Row].cBlockedFlag, data[Row].vUserName, data[Row].ModifyBy, data[Row].dModifyOn, data[Row].IST);
                    aaDataSet.push(InDataSet);
                }

                if ($("#tblAudit").children().length > 0) {
                    $("#tblAudit").dataTable().fnDestroy();
                }
                $('#tblAudit').prepend($('<thead>').append($('#tblAudit tr:first'))).dataTable({
                    "bStateSave": false,
                    "bPaginate": false,
                    "sPaginationType": "full_numbers",
                    "sDom": '<fr>t<p>',
                    "iDisplayLength": 10,
                    "bSort": false,
                    "bFilter": false,
                    "bDestory": true,
                    "bRetrieve": true,
                    "aaData": aaDataSet,
                    "aoColumns": [
                        { "sTitle": "Sr.No." },
                        { "sTitle": "UserType" },
                        { "sTitle": "Remarks" },
                        { "sTitle": "Status" },
                        { "sTitle": "LoginName" },
                        { "sTitle": "ModifyBy" },
                        { "sTitle": "ModifyOn" },
                        { "sTitle": Location },
                    ],
                    "oLanguage": {
                        "sEmptyTable": "No Record Found",
                    }
                });
                $find('modalpopupaudittrail').show();
            }
        },
        failure: function (response) {
            //alert(response.d);
        },
        error: function (response) {
            //alert(response.d);
        }
    });

}

function ExportToExcel(UserProfile) {
    $("#<%= HFProfileName.ClientID %>").val(UserProfile);
    var btn = document.getElementById('<%= btnExportToExcel.ClientId()%>')
    btn.click();
}
function Updated() {
    //onUpdated();
}

function characterlimit(id) {

    var text = id.value
    var textLength = text.length;
    if (textLength > 500) {
        $(id).val(text.substring(0, (500)));
        msgalert("Only 500 characters are allowed !");
        return false;
    }
    else {
        return true;
    }

}

    </script>
    <style type="text/css">
        .dataTables_wrapper {
            width: 100% !important;
        }

        .dataTables_scrollHeadInner {
            width: auto !important;
        }
    </style>

    <script type="text/javascript">
        Sys.Browser.WebKit = {};
        if (navigator.userAgent.indexOf('WebKit/') > -1) {
            Sys.Browser.agent = Sys.Browser.WebKit;

            Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
            Sys.Browser.name = 'WebKit';
        }

    </script>

    <div id="ModalBackGround" runat="server" class="divModalBackGround" style="display: none; width: 100% !Important;"></div>
    <button id="btnRemarls" runat="server" style="display: none;" />
    <asp:HiddenField runat="server" ID="HFCurrentIndex" />
    <asp:HiddenField runat="server" ID="HFUserName" />
    <div id="divRemarks" runat="server" class="centerModalPopup" style="display: none; left: 30%; width: 28%; position: absolute; top: 525px; border: 1px solid; height: 200px;">
        <div style="background-color: #1560A1;">
            <table style="width: 90%; margin: auto;">
                <tr>
                    <td colspan="2" class="LabelText" style="text-align: center !important; color: white; font-size: 14px !important; width: 97%;"><b>Enter Remarks</b>

                    </td>

                    <td style="text-align: center; height: 22px;" valign="top">
                        <img id="CancelRemarks" alt="Close" src="images/Close.gif" style="position: relative; float: right; right: 5px; cursor: pointer;"
                            title="Close" />
                    </td>
                </tr>
            </table>
            <hr />
        </div>
        <table style="margin: 10px 10px 10px 10px;">

            <tr>

                <td colspan="2">
                    <center>
                        <asp:Label runat="server" ID="lblChangeStatus"></asp:Label>
                    </center>
                </td>

            </tr>

            <tr style="margin: 10px 10px 10px 10px;">

                <td style="white-space: nowrap; text-align: right; width: 120px;" class="Label">Remarks*:
                </td>
                <td class="Label" align="right">
                    <asp:TextBox ID="txtRemarks" runat="Server" TextMode="MultiLine" onkeyup="characterlimit(this)" Text="" CssClass="textbox"> </asp:TextBox>
                    <asp:Label runat="server" Text="*" ForeColor="Red" ID="lblError" Style="display: none;"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <center>
            <table>
                <tr>
                    <td>
                        <center>
                            <asp:Button ID="btnSave" runat="server" CssClass="btn btnsave" OnClientClick="return validation21();" ValidationGroup="validate" Text="Save" />
                            <asp:Button ID="btnCancel1" runat="server" CssClass="btn btncancel" Text="Cancel"  />
                        </center>
                    </td>
                </tr>
            </table>
        </center>
    </div>

    <cc1:ModalPopupExtender ID="ModalRemarks" runat="server" PopupControlID="divRemarks"
        BackgroundCssClass="modalBackground" TargetControlID="btnRemarls" BehaviorID="ModalRemarks"
        CancelControlID="CancelRemarks">
    </cc1:ModalPopupExtender>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellpadding="0" width="100%">
                <tbody>
                    <tr>
                        <td>
                            <table cellpadding="5" width="100%">
                                <tbody>
                                    <tr>
                                        <td colspan="2" class="Label">
                                            <asp:RadioButtonList ID="rblmode" runat="server" AutoPostBack="True" RepeatDirection="Horizontal"
                                                Style="margin: auto; width: 10%" TabIndex="1">
                                                <asp:ListItem Value="A">Add</asp:ListItem>
                                                <asp:ListItem Value="E">Edit</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 40%; text-align: right;" class="Label">
                                            <asp:Label ID="lblMode" runat="server"></asp:Label>
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox onblur="ValidateUserName(this.value);" ID="txtUserAdd" runat="server"
                                                CssClass="textBox" Visible="False" Width="32%" TabIndex="2"></asp:TextBox>
                                            <asp:DropDownList ID="ddlUserEdit" runat="server" CssClass="dropDownList" TabIndex="2"
                                                Visible="False" AutoPostBack="True" Width="35%">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr id="TblAdd" runat="server" visible="false">
                        <td>
                            <table width="100%" cellpadding="5px">
                                <tbody>
                                    <tr>
                                        <td style="text-align: right; width: 40%;" class="Label">Scope Name*:
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="DDLScopeMst" runat="server" CssClass="dropDownList" Width="33%"
                                                TabIndex="3">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;" class="Label">First Name*:
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="TxtFirstName" runat="server" CssClass="textBox" Width="32%" TabIndex="4"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;" class="Label">Last Name*:
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="TxtLastName" runat="server" CssClass="textBox" Width="32%" TabIndex="5"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;" class="Label">Department*:
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="DDLDept" runat="server" CssClass="dropDownList" Width="33%"
                                                TabIndex="6">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;" class="Label">Location*:
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="DDLLocation" runat="server" CssClass="dropDownList" Width="33%"
                                                TabIndex="7">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;" class="Label">Email Id :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="textBox" Width="32%" CausesValidation="True"
                                                ValidationGroup="B" TabIndex="8"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Width="360px"
                                                ValidationGroup="B" ControlToValidate="txtEmail" Display="Dynamic" ErrorMessage="Please Enter Email Id in Correct Format."
                                                SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;" class="Label">PhoneNo :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtPhNo" runat="server" TabIndex="9" CssClass="textBox" Width="32%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;" class="Label">ExtNo :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtExtNo" runat="server" TabIndex="10" CssClass="textBox" Width="32%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Label" style="text-align: right;">Valid From :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtloginfrom" runat="server" Width="32%" CssClass="textBox" TabIndex="11" />
                                            <cc1:CalendarExtender ID="CalExPeriod2CheckInDate" runat="server" Format="dd-MMM-yyyy"
                                                TargetControlID="txtloginfrom">
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Label" style="text-align: right;">Valid To :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtLoginTo" runat="server" Width="32%" CssClass="textBox" TabIndex="12" />
                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                                TargetControlID="txtLoginTo">
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;" class="Label">Remark :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtRemark" runat="server" CssClass="textBox" Width="32%" TabIndex="13"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 40%; text-align: right;" class="Label">
                                            <asp:Label ID="lblmfa" runat="server" Text="MFA :" Visible="True"></asp:Label>
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:CheckBox ID="chkmfa" Visible="True" ToolTip="IsContractual" runat="server" OnCheckedChanged="chkmfa_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr id="trmfssend" runat="server" visible="false">
                                        <td style="width: 40%; text-align: right;" class="Label"></td>
                                        <td style="text-align: left;">
                                            <asp:CheckBox ID="chkEmail" Visible="True" Text="Email" ToolTip="Email" runat="server"></asp:CheckBox>
                                            <asp:CheckBox ID="chksms" Visible="True" Text="Sms" ToolTip="Sms" runat="server"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align: center;">
                                            <asp:Button ID="btnadd" runat="server" Text="" ToolTip="Go" CssClass="btn btngo" Visible="False"
                                                TabIndex="14"></asp:Button>
                                            <asp:Button ID="btnedit" runat="server" Text="Update" ToolTip="Update" CssClass="btn btnnew"
                                                Visible="False" TabIndex="15"></asp:Button>
                                            <asp:Button ID="btncancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="btn btncancel"
                                                Visible="False" TabIndex="16"></asp:Button>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <center>
                                <div style="width: 75% !important; overflow: auto; margin-top:100px;">

                                    <asp:GridView ID="gvwUserType" runat="server" SkinID="grdViewAutoSizeMax" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:BoundField DataField="vusertypeCode" HeaderText="UserTypeNo" />
                                            <asp:BoundField DataField="vUserTypeName" HeaderText="UserType" />
                                            <asp:BoundField DataField="LoginName" HeaderText="LoginName" />
                                            <asp:BoundField DataField="LoginPass" HeaderText="Password" />
                                            <asp:TemplateField HeaderText="Add" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LnkGrdAdd" CommandName="MYADD" runat="server" OnClientClick="return Updating();">Add</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ResetPassword">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LnkGrdRstPwd" CommandName="RESETPWD" runat="server" Enabled="false"
                                                        OnClick="LnkGrdRstPwd_Click">ResetPassword</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CurrentStatus">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblCurrentStatus"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ChangeStatusTo">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LnkGrdstatus" CommandName="STATUS" runat="server" Enabled="false">Active</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unlock">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LnkGrdUnLock" CommandName="UNLOCK" runat="server">Unlock</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="AuditTrail">
                                                <ItemTemplate>
                                                    <center>
                                                        <img src="Images/audit.png" onclick="AuditTrail(this.id);" id='<%#  Eval("LoginName") + "+" + Eval("vUserTypeCode") %>' />
                                                    </center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Export">
                                                <ItemTemplate>
                                                    <center>
                                                        <img src="images/Export.gif" onclick="ExportToExcel(this.id);" id='<%# "img" + Eval("LoginName") + "+" + Eval("vUserTypeCode")%> ' />
                                                    </center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </center>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnadd" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnedit" EventName="Click" />
            <asp:PostBackTrigger ControlID="gvwusertype" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:Button ID="btnExportToExcel" runat="server" Style="display: none;" class="btn btnexcel"/>
    <asp:HiddenField runat="server" ID="HFProfileName" />
    <button id="btnAuditTrail" runat="server" style="display: none;" class="btn btnaudit"/>

    <cc1:ModalPopupExtender ID="modalpopupaudittrail" runat="server" PopupControlID="dvAudiTrail"
        BackgroundCssClass="modalBackground" TargetControlID="btnAuditTrail" BehaviorID="modalpopupaudittrail"
        CancelControlID="imgAuditTrail">
    </cc1:ModalPopupExtender>

    <div id="dvAudiTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
        <table border="0" cellpadding="2" cellspacing="2" width="100%">
            <tr>
                <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 80%;">Audit Trail Information</td>
                <td style="width: 3%">
                    <img id="imgAuditTrail" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table align="center" border="0" cellpadding="2" cellspacing="2" width="100%">
                        <tr>
                            <td>
                                <table id="tblAudit" class="tblAudit" width="100%" style="background-color: aliceblue;"></table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
        </table>
    </div>

    <asp:GridView runat="server" ID="gvExport" AutoGenerateColumns="true" Style="display: none">
    </asp:GridView>

</asp:Content>
