<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmNewRoleOperationMatrix, App_Web_22suyskz" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <hr class="hr" style="background-color: Green;width:100%" />
    <table cellpadding="3px" width="100%">
        <tr>
            <td class="Label" valign="top">
                <asp:RadioButtonList ID="rbtnlselecttype" runat="server" RepeatDirection="Horizontal"
                    AutoPostBack="True" Style="margin: auto;">
                    <asp:ListItem Value="0">Assign/Remove Rights</asp:ListItem>
                    <asp:ListItem Value="1">Report</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td class="Label" valign="top" colspan="2">
                <asp:RadioButtonList ID="rbtnlstApplicationType" runat="server" RepeatDirection="Horizontal"
                    AutoPostBack="True" Style="margin: auto;">
                    <asp:ListItem Value="0">BizNET Web</asp:ListItem>
                    <asp:ListItem Value="1">BizNET Desktop</asp:ListItem>
                    <asp:ListItem Value="2">BizNET LIMS</asp:ListItem>
                    <asp:ListItem Value="3">BioLyte</asp:ListItem>
                    <asp:ListItem Value="4">IMP Track</asp:ListItem>
                    <asp:ListItem Value="5">DI Soft</asp:ListItem>
                    <asp:ListItem Value="6">SDTM</asp:ListItem>
                    <asp:ListItem Value="7">OIMS</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
    </table>

    <div id="divtreeview" runat="server">
        <table style="margin: auto;" width="50%">
            <tr>
                <td style="text-align: left;" class="Label">User Type<span class="TDMandatory">*</span>
                </td>
                <td style="text-align: left;" class="Label">Operation<span class="TDMandatory">*</span>
                </td>
            </tr>
            <tr>
                <td style="text-align: left; vertical-align: top;" class="Label">
                    <asp:UpdatePanel ID="UpRole" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ListBox ID="ddlRole" runat="server" Rows="26" CssClass="dropDownList" AutoPostBack="True"
                                Width="100%" Height="340px" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
                <td style="text-align: left; vertical-align: top; width: 60%;" class="Label">
                    <asp:UpdatePanel ID="UpTree" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div style="border-right: gray thin solid; border-top: gray thin solid; overflow: auto; border-left: gray thin solid; width: 90%; border-bottom: gray thin solid; height: 340px"
                                id="Div1" align="left">
                                <asp:TreeView ID="trVwrpt" runat="server" Width="300px" ShowCheckBoxes="All" ShowLines="True"
                                    ExpandDepth="0" ForeColor="black" />
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlRole" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr style="padding: 10px 0px 0px 0px">
                <td colspan="2" width="100%" align="center" valign="middle">
                    <asp:Button ID="btnSave" runat="server" CssClass="btn btnsave" OnClientClick="javascript:return IsChecked();"
                        Text="Save" ToolTip="Save" />
                    <asp:Button ID="btnexit" runat="server" Text="Exit" ToolTip="Exit" CssClass="btn btnexit"
                        OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
                    <asp:Button ID="btnAudit" runat="server" OnClientClick="return validaudit();" ToolTip="Audit" CssClass="btn btnaudit" />
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hdnForm" runat="server"></asp:HiddenField>

    <div id="divreportview" runat="server" visible="false">
        <table style="margin: auto;" width="50%">
            <tr>
                <td class="Label" align="center" colspan="2">
                    <asp:DropDownList ID="ddlProfile" runat="server" CssClass="Multiselect" Width="200px" border="2px" TabIndex="4">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnReport" runat="server" Text="Report" OnClientClick="return validreport();" ToolTip="Report" CssClass="btn btnnew" />
                </td>
            </tr>
        </table>

    </div>



    <div id="ModalBackGround" runat="server" class="divModalBackGround" style="display: none; width: 100% !Important;"></div>

    <button id="btnRemarls" runat="server" style="display: none;" />
    <cc1:ModalPopupExtender ID="ModalRemarks" runat="server" PopupControlID="divRemarks"
        BackgroundCssClass="modalBackground" TargetControlID="btnRemarls" BehaviorID="ModalRemarks"
        CancelControlID="CancelRemarks">
    </cc1:ModalPopupExtender>

    <div id="divRemarks" runat="server" class="centerModalPopup" style="display: none; left: 30%; width: 32%; position: absolute; top: 525px; border: 1px solid; height: 200px;">
        <div>
            <table style="width: 90%; margin: auto;">
                <tr>
                    <td colspan="2" class="LabelText" style="text-align: center !important; color: white; font-size: 16px !important; width: 97%;"><b style="margin-left: 6%;">Enter Remarks</b>

                    </td>
                    <td style="width: 3%; text-align: Right !important;">
                        <img id="CancelRemarks" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" title="Close" style="margin-right:-64%" />
                    </td>
                    <%--<td style="text-align: center; height: 22px;" valign="top">
                        <img id="CancelRemarks" alt="Close" src="images/Close.gif" style="position: relative; float: right; right: 5px; cursor: pointer;"
                            title="Close" />
                    </td>--%>
                </tr>
            </table>
            <hr />
        </div>
        <table style="margin: 10px 10px 10px 10px;">

            <tr style="margin: 10px 10px 10px 10px;">

                <td style="white-space: nowrap; text-align: right; width: 120px;" class="Label">Remarks*:
                </td>
                <td class="Label" align="right">
                    <asp:TextBox ID="txtRemarks" runat="Server" TextMode="MultiLine" onkeyup="characterlimit(this)" Text="" CssClass="textbox"> </asp:TextBox>
                    <asp:Label runat="server" Text="*" ForeColor="Red" ID="lblError" Style="display: none;"></asp:Label>
                </td>
            </tr>
        </table>        
        <hr />
        <center>
            <table>
                <tr>
                    <td>
                        <center>
                            <asp:Button ID="btnSaveremarks" runat="server" CssClass="btn btnsave" OnClientClick="return validation();" Text="Save" Width="105px" />
                            <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" OnClientClick="return validatecancel();" Text="Cancel" Width="105px" />
                        </center>
                    </td>
                </tr>
            </table>
        </center>
    </div>

    <input id="btnchkaudit" runat="server" style="width: 180px; display: none;" class="button" />
    <cc1:ModalPopupExtender ID="mpeAuditTrail" runat="server" PopupControlID="divAuditTrail"
        BackgroundCssClass="modalBackground" BehaviorID="mpeAuditTrail" TargetControlID="btnchkaudit"
        CancelControlID="imgAuditClose">
    </cc1:ModalPopupExtender>


    <div id="divAuditTrail" runat="server" class="centerModalPopup" style="display: none; width: 82%; position: absolute; top: 400px; max-height: 100%;">
        <table style="overflow-x: auto; max-width: 100%; display: block;">
            <tr>
                <td class="LabelText" style="text-align: center !important; font-size: 16px !important; width: 100%;">
                    <asp:Label ID="lblPopUp" runat="server"><b>Audit Trail</b></asp:Label>
                </td>
                <td style="width: 3%; text-align: Right !important;">
                    <img id="imgAuditClose" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';"
                        title="Close" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:UpdatePanel ID="UppanelgrdAudit" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="divgrdAudit" runat="server" style="width: 99%; margin: 0px auto;">
                                <asp:GridView ID="gvwUserTypeaudit" runat="server" Style="background-color: #d0e4f7 !important; color: white" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:BoundField DataField="cOperationType" HeaderText="App.Type" />
                                        <asp:BoundField DataField="UserType" HeaderText="Profile" />
                                        <asp:BoundField DataField="ChildName" HeaderText="Operation" />
                                        <asp:BoundField DataField="vStatus" HeaderText="Status" />
                                        <asp:BoundField DataField="dModifyOn" HeaderText="FromDate" ItemStyle-Wrap="false" />
                                        <asp:BoundField DataField="dModifyOff" HeaderText="ToDate" ItemStyle-Wrap="false" />
                                        <asp:BoundField DataField="vRemarkActive" HeaderText="RemarkActive" ItemStyle-CssClass="WrapStyle" />
                                        <asp:BoundField DataField="vRemarkInActive" HeaderText="RemarkInActive" ItemStyle-CssClass="WrapStyle" />
                                        <asp:BoundField DataField="ModifyBy" HeaderText="ModifyBy" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                        <Triggers>

                            <asp:AsyncPostBackTrigger ControlID="btnAudit" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>

            </tr>
           
            <tr>

                <td style="text-align: center !important; width: 100%;">
                    <asp:Button ID="btnexcel" runat="server" CssClass="btn btnexcel" />
                </td>
            </tr>
        </table>
    </div>

    <input id="btnreportinput" runat="server" style="width: 180px; display: none;" class="btn btnnew" />
    <cc1:ModalPopupExtender ID="mperolereport" runat="server" PopupControlID="divreport"
        BackgroundCssClass="modalBackground" BehaviorID="mperolereport" TargetControlID="btnreportinput"
        CancelControlID="imgReportClose">
    </cc1:ModalPopupExtender>


    <div id="divreport" runat="server" class="centerModalPopup" style="display: none; width: 82% !important; position: absolute; top: 400px; max-height: 465px;">
        <asp:Label ID="lblPopUpReport" runat="server" Text="Role Report"></asp:Label>
        <img id="imgReportClose" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" style="float: right;"
            title="Close" />

        <hr />

        <asp:UpdatePanel ID="uppanelgvwreport" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:GridView ID="gvwreport" runat="server" AutoGenerateColumns="true" Style="width: 99%; overflow: auto; background-color: #d0e4f7 !important; color: white;">
                </asp:GridView>
            </ContentTemplate>
            <Triggers>

                <asp:AsyncPostBackTrigger ControlID="btnreport" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>

        <asp:Button ID="btnexportreport" runat="server" CssClass="btn btnexcel" />
    </div>

    <link rel="stylesheet" type="text/css" href="App_Themes/smoothnessjquery-ui.css" />
    <link href="App_Themes/StyleBlue/StyleBlue.css" rel="stylesheet" type="text/css" />

    <link rel="Stylesheet" href="App_Themes/StyleBlue/UI_Theme/jquery-ui.css" type="text/css" />
    <link rel="stylesheet" type="text/css" href="App_Themes/jquery.multiselect.css" />

    <script src="Script/jquery-ui.js" type="text/javascript"></script>

    <script src="Script/jquery.multiselect.js" type="text/javascript"></script>

    <script type="text/javascript" src="script/jquery.datatables.min.js"></script>

    <script src="Script/jquery.dataTables.plugins.js" type="text/javascript"></script>

    <script language="javaxscript" type="text/javascript">

        function pageLoad() {

            fnApplySelectForm();
            $("#ctl00_CPHLAMBDA_ddlProfile").next().css({ "width": "218.777777671814px" });
            datatablestru();
        }
        function IsChecked() {
            if (document.getElementById('<%=ddlRole.ClientID%>').selectedIndex < 0) {
                msgalert("No Role Found !");
                return false;
            }
            if (document.getElementById('<%=ddlRole.ClientID%>').selectedIndex == 0) {
                msgalert("Please Select Role !");
                return false;
            }
            var treeView = document.getElementById('<%=trVwrpt.ClientID%>');

            for (var i = 1; i < treeView.childNodes.length - 1; i++) {
                var tbl = treeView.childNodes[i];
                var tbl1 = tbl.getElementsByTagName("input");

                if (tbl1.length > 0) {
                    for (j = 0; j < tbl1.length; j++) {
                        if (tbl1[j].checked == true) {
                            return true;
                        }
                    }
                }
            }
            msgalert("Please Select Atleast One Operation !");
            return false;
        }
        function ClearItem(i) {
            i = parseInt(i) + 1;
            if (document.getElementById('ctl00_CPHLAMBDA_trVwrptn' + i + 'CheckBox') != null) {
                document.getElementById('ctl00_CPHLAMBDA_trVwrptn' + i + 'CheckBox').checked = false;
            }
            else { return; }

            ClearItem(i);

        }
        function checkItem(strNodeToSelect) {


            for (iStr = 0; iStr < strNodeToSelect.split(",").length; iStr++) {
                document.getElementById('ctl00_CPHLAMBDA_trVwrptn' + strNodeToSelect.split(",")[iStr] + 'CheckBox').checked = true;
            }
        }

        function validation() {
            if (document.getElementById('<%= txtRemarks.ClientId %>').value.toString().trim().length <= 0) {
                document.getElementById('<%= lblError.ClientID%>').style.display = '';
                msgalert('Please Enter Remarks !')
                $find('ModalRemarks').show()
                return false
            }
            var btn = document.getElementById('<%= btnSaveremarks.ClientId()%>')
            btn.click();
            return true
        }
        function validatecancel() {
            document.getElementById('<%= txtRemarks.ClientId %>').value = '';
            return true

        }
        function validaudit() {
            if (document.getElementById('<%=ddlRole.ClientID%>').selectedIndex == 0) {
                msgalert("Please Select Role !");
                return false;
            }
        }
        function validreport() {
            var Form = "";
            $('#<%= hdnForm.ClientId %>').val('');
            if ($("input[name$='ddlProfile']:checked").length > 0) {
                $("input[name$='ddlProfile']:checked").each(function () {
                    Form += $(this).val() + ",";
                });
                Form = Form.substring(0, Form.length - 1);
                $('#<%= hdnForm.ClientId %>').val(Form);
            }
            else {
                msgalert("Please Select Role !");
                return false;
            }
        }
        function characterlimit(id) {

            var text = id.value
            var textLength = text.length;
            if (textLength > 100) {
                $(id).val(text.substring(0, (100)));
                msgalert("Only 100 characters are allowed !");
                $find('ModalRemarks').hide()
                return false;
            }
            else {
                return true;
            }

        }

        function fnApplySelectForm() {
            var Form = [];
            $("#<%= ddlProfile.ClientID%>").multiselect({
                noneSelectedText: "--Select Usertype--",
                click: function (event, ui) {
                    if (ui.checked == true)
                        Form.push("'" + ui.value + "'");
                    else if (ui.checked == false) {
                        if ($.inArray("'" + ui.value + "'", Form) >= 0)
                            Form.splice(Form.indexOf("'" + ui.value + "'"), 1)
                    }

                    if ($("input[name$='ddlProfile']").length > 0) {

                    }



                },
                checkAll: function (event, ui) {
                    Form = [];
                    for (var i = 0; i < event.target.options.length; i++) {
                        Form.push("'" + $(event.target.options[i]).val() + "'")
                    }
                    if ($("input[name$='ddlProfile']").length > 0) {

                    }
                    $("#<%= ddlProfile.ClientID%>").multiselect("refresh");
                    $("#<%= ddlProfile.ClientID%>").multiselect("widget").find(':checkbox').click();


                },
                uncheckAll: function (event, ui) {
                    Form = [];
                    $("#<%= ddlProfile.ClientID%>").multiselect("refresh");
                    if ($("input[name$='ddlProfile':checked]").length > 0) {

                    }
                }
            });
        }
        function datatablestru() {
            $('[id$="' + '<%= gvwUserTypeaudit.ClientID%>' + '"] tbody tr').length < 8 ? scroll = "25%" : scroll = "300px";
            $('#<%= gvwUserTypeaudit.ClientID%>').prepend($('<thead>').append($('#<%= gvwUserTypeaudit.ClientID%> tr:first'))).dataTable({
                "sScrollY": "300px",
                "sScrollX": "100%",
                "bStateSave": false,
                "bPaginate": false,
                "bAutoWidth": true,
                "sDom": 'r<"H"lf><"datatable-scroll"t><"F"ip>',
                "bSort": false,
                "bDestory": true,
                "bRetrieve": true,
                "bScrollCollapse": true
            });
            $('#<%= gvwUserTypeaudit.ClientID%> tr:first').css('background-color', '#3A87AD');
            $('tr', $('.dataTables_scrollHeadInner')).css("background-color", "rgb(58, 135, 173)");

            if ($('#<%= gvwUserTypeaudit.ClientID%>').length > 0) {
                setTimeout(function () { $('#<%= gvwUserTypeaudit.ClientID%>').dataTable().fnAdjustColumnSizing(); }, 10);
            }


            $('[id$="' + '<%= gvwreport.ClientID%>' + '"] tbody tr').length < 8 ? scroll = "25%" : scroll = "300px";
            $('#<%= gvwreport.ClientID%>').prepend($('<thead>').append($('#<%= gvwreport.ClientID%> tr:first'))).dataTable({
                "sScrollY": "300px",
                "sScrollX": "100%",
                "bStateSave": false,
                "bPaginate": false,
                "bAutoWidth": true,
                "sDom": 'r<"H"lf><"datatable-scroll"t><"F"ip>',
                "bSort": false,
                "bDestory": true,
                "bRetrieve": true,
                "bScrollCollapse": true
            });
            $('#<%= gvwreport.ClientID%> tr:first').css('background-color', '#3A87AD');
            $('tr', $('.dataTables_scrollHeadInner')).css("background-color", "rgb(58, 135, 173)");
            $("#ctl00_CPHLAMBDA_gvwreport_wrapper").attr("style", "width:99%");


            if ($('#<%= gvwreport.ClientID%>').length > 0) {
                setTimeout(function () { $('#<%= gvwreport.ClientID%>').dataTable().fnAdjustColumnSizing(); }, 10);
            }
        }

        $("[id*=trVwrpt] input[type=checkbox]").live("click", function () {
            var table = $(this).closest("table");
            var Flag = false;
            var Flag1 = false;
            var Index = 0;
            var IndexNot = 0;
            var IndexParent = 0;
            var IndexParentNot = 0;
            var chkAll = 0;
            if (table.next().length > 0 && table.next()[0].tagName == "DIV") {
                //Is Parent CheckBox
                var childDiv = table.next();
                var parentDiv = table.parent().prev();
                var childNode = table.parent();
                var isChecked = $(this).is(":checked");

                $("input[type=checkbox]", childDiv).each(function () {

                    if (isChecked) {
                        $(this).attr("checked", "checked");
                    } else {
                        $(this).removeAttr("checked");
                    }
                });

                $("input[type=checkbox]", parentDiv).each(function () {
                    var childTrue = 0;
                    var childFalse = 0;
                    var IsRemove = false;
                    $("input[type=checkbox]", childNode).each(function () {
                        var select = $(this).attr("checked") ? true : false;
                        if (select == true) {
                            childTrue = childTrue + 1;
                        }
                        if (select == false) {
                            childFalse = childFalse + 1;
                        }
                    });
                    if (childNode.children().find('input[type="checkbox"]').length == childFalse) {
                        $(this).removeAttr("checked");
                    } else {
                        $(this).attr("checked", "checked");
                    }
                });

            } else {
                //Is Child CheckBox
                var parentDIV = $(this).closest("DIV");
                $(this).closest("DIV").find("table [type=checkbox]").each(function () {
                    var check = $(this).attr("checked") ? true : false;
                    if (check == true) {
                        Flag = true;
                        Index = Index + 1;
                    }
                    if (check == false) {
                        IndexNot = IndexNot + 1;
                    }
                });
                if (Flag == true) {
                    parentDIV.prev().find("[type=checkbox]").attr("checked", "checked");
                }
                if ($(this).closest("DIV").find("table [type=checkbox]").length == Index) {
                    parentDIV.prev().find("[type=checkbox]").removeAttr("checked");
                    parentDIV.prev().find("[type=checkbox]").attr("checked", "checked");

                }
                if ($(this).closest("DIV").find("table [type=checkbox]").length == IndexNot) {
                    parentDIV.prev().find("[type=checkbox]").removeAttr("checked");
                }

                $(this).closest("DIV").parent().find("table [type=checkbox]").each(function () {
                    var chk = $(this).attr("checked") ? true : false;
                    if (chk == true) {
                        Flag1 = true;
                        IndexParent = IndexParent + 1;
                    }
                    if (chk == false) {
                        IndexParentNot = IndexParentNot + 1;
                    }
                });
                if (Flag1 == true) {
                    parentDIV.parent().prev().find("[type=checkbox]").attr("checked", "checked");
                }
                if ($(this).closest("DIV").parent().find("table [type=checkbox]").length == IndexParent) {
                    parentDIV.parent().prev().find("[type=checkbox]").removeAttr("checked");
                    parentDIV.parent().prev().find("[type=checkbox]").attr("checked", "checked");

                }
                if ($(this).closest("DIV").parent().find("table [type=checkbox]").length == IndexParentNot) {
                    parentDIV.parent().prev().find("[type=checkbox]").removeAttr("checked");
                }
            }
        });
    </script>
    <style type="text/css">
        table.dataTable tr.odd {
            height: 48px;
        }

        .WrapStyle {
            word-break: break-all;
        }

        /*.dataTables_wrapper .dataTable tbody td {
            padding: 0px 0px !important;
            font-family: Calibri, sans-serif !important;
        }

        .dataTables_wrapper .dataTable th {
            color: White !important;
            font-family: Calibri, sans-serif !important;
            font-size: 14px !important;
        }*/

        .hiddenColumn {
            display: none;
        }

        .left {
            text-align: left;
        }

        .right {
            text-align: right;
        }

        .ui-multiselect {
            max-height: 35px;
            overflow: auto;
            overflow-x: hidden;
            white-space: nowrap;
        }

        .ui-multiselect-menu {
            width: 215px !important;
            font-size: 0.8em !important;
        }

            .ui-multiselect-menu span {
                vertical-align: top;
            }

        .ui-menu .ui-menu-item a {
            font-size: 11px !important;
            text-align: left !important;
        }

        .tablelabel {
            color: black;
            font-family: Calibri, sans-serif !important;
            font-size: 12px;
        }

        .ui-multiselect-checkboxes li ul li {
            list-style: none !important;
            clear: both;
            font-size: 1.0em;
            padding-right: 3px;
        }

        .columnul {
            float: left !important; /*width: 14.28% !important;*/ /*width: 14.28% !important;*/
            padding-left: 10px !important; /*height: 175px !important;*/
        }

        .columnwidth {
            /*width: 50% !important;*/
            width: 75%;
            font-size: 0.8em !important;
        }

        .ui-multiselect-checkboxes ui-helper-reset {
            height: 200px;
            width: 500px;
            overflow: auto;
        }

        .Multiselect {
            color: #555;
            line-height: 165%;
        }
        .dataTables_wrapper {
            max-width:1100px;
        }
    </style>
</asp:Content>
