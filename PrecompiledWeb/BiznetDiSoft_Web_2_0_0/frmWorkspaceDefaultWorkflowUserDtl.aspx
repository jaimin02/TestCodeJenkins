<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmWorkspaceDefaultWorkflowUserDtl, App_Web_w1bzwbih" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <link rel="stylesheet" type="text/css" href="App_Themes/jquery.multiselect.css" />
    <link rel="stylesheet" type="text/css" href="App_Themes/multiple-select.css" />
    <link href="App_Themes/StyleBlue/UI_Theme/jquery-ui.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        /*.hide_column {
            display: none;
        }*/

        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

        /*.table th {
            border: 1px solid black;
            border-collapse: collapse;
        }*/

        .ui-multiselect {
            border: 1px solid navy;
            max-height: 35px;
            overflow: auto;
            overflow-x: hidden;
            white-space: nowrap;
            max-width: 84% !important;
        }

        .ui-multiselect-menu {
            /*max-width: 43% !important;*/
        }

        .ui-corner-all {
            width: 384px !important;
        }

        .MasterLoader {
            display: none;
            position: fixed;
            z-index: 1000;
            top: 0;
            left: 0;
            height: 100%;
            width: 100%;
            background: rgba( 255, 255, 255, .5 ) url('images/AjaxLoader.gif') 50% 50% no-repeat;
        }

        .dataTables_length, .dataTables_info, .dataTables_filter {
            color: white;
        }
    </style>

    <asp:UpdatePanel ID="Up_General" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <table width="100%">
                <tbody>
                    <tr>
                        <td align="center">
                            <table align="center" width="100%">
                                <tbody>
                                    <tr>
                                        <td>
                                            <fieldset class="FieldSetBox" style="display: block; width: 95.4%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                                    <img id="img2" alt="Projectwise User Details" src="images/panelcollapse.png"
                                                        onclick="Display(this,'divProjectwiseUserDetail');" runat="server" style="margin-right: 2px;" />Projectwise User Details</legend>
                                                <div id="divProjectwiseUserDetail">
                                                    <table style="width: 98%;">
                                                        <tr>
                                                            <td style="text-align: center;">
                                                                <asp:Label ID="lblInformation" runat="server" CssClass="Label"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">
                                                                <table style="width: 100%; margin-top: 1%;">
                                                                    <tr>
                                                                        <td>
                                                                            <tr>
                                                                                <%-- <td class="Label" style="text-align: right; width: 25%;">User Group :
                                                                                </td>
                                                                                <td style="text-align: left; width: 30%;">
                                                                                    <asp:DropDownList ID="DDLUserGroup" runat="server" CssClass="dropDownList" Width="90%"
                                                                                        AutoPostBack="True">
                                                                                    </asp:DropDownList>
                                                                                </td>--%>
                                                                                <td class="Label" style="text-align: right; width: 20%;">User Type :
                                                                                </td>
                                                                                <td style="text-align: left; width: 30%;">
                                                                                    <asp:DropDownList ID="DdlUserType" runat="server" CssClass="dropDownList" Width="87%"
                                                                                        AutoPostBack="false" onChange="GetUsers();">
                                                                                    </asp:DropDownList>
                                                                                    <%----%>

                                                                                </td>
                                                                                <td class="Label" style="text-align: right; width: 6%;">Location :
                                                                                </td>
                                                                                <td style="text-align: left; width: 54%;">
                                                                                    <asp:DropDownList ID="DdlLocation" runat="server" CssClass="dropDownList" Width="60%"
                                                                                        AutoPostBack="false" onChange="GetUsers();">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">
                                                                <asp:UpdatePanel ID="Up_UserStages" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <table width="100%">
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td style="text-align: right; width: 20%" class="Label">Users :
                                                                                    </td>
                                                                                    <td align="left" style="width: 28%;">
                                                                                        <asp:DropDownList ID="ddlUserName" runat="server" Style="width: 32%" CssClass="dropDownList" AutoPostBack="false" onchange=" fnUserName();"></asp:DropDownList>
                                                                                    </td>
                                                                                    <td class="Label" style="width: 8%;display:none">Training Assign : </td>
                                                                                    <td>
                                                                                        <asp:CheckBox ID="chkTrainingAssign" runat="server" style="display:none;"/>
                                                                                    </td>
                                                                                </tr>
                                                                                <%--<tr>
                                                                                    <td style="text-align: left" class="Label">Stages
                                                                                    </td>
                                                                                    <td>
                                                                                        <div style="border-right: gray thin solid; border-top: gray thin solid; overflow-y: scroll; border-left: gray thin solid; width: 35%; border-bottom: gray thin solid; height: 80px; text-align: left"
                                                                                            id="Div2">
                                                                                            <asp:CheckBoxList ID="chklstStages" runat="server" ForeColor="Black" Font-Size="Small"
                                                                                                Font-Names="Verdana" CssClass="checkboxlist" Font-Name="Verdana">
                                                                                            </asp:CheckBoxList>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>--%>
                                                                            </tbody>
                                                                        </table>
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click"></asp:AsyncPostBackTrigger>
                                                                    </Triggers>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: center;">
                                                                <asp:Button ID="btnSave" runat="server" Text="Save" ToolTip="Save" CssClass="btn btnsave"
                                                                    OnClientClick="return Validation();" />
                                                                <asp:Button ID="BtnExit" runat="server" Text="" ToolTip="Back" CssClass="btn btnback" />
                                                                <asp:Button ID="Btnhome" runat="server" Text="Exit" ToolTip="Exit" CssClass="btn btnexit"
                                                                    OnClientClick="return msgconfirmalert('Are You sure You want to EXIT?',this)" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <table style="width: 100%;" cellpadding="5px">
                                <tbody>
                                    <tr>
                                        <td>
                                            <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                                    <img id="img1" alt="Projectwise User Data" src="images/panelcollapse.png"
                                                        onclick="Display(this,'divProjectwiseUserData');" runat="server" style="margin-right: 2px;" />Projectwise User Data</legend>
                                                <div id="divProjectwiseUserData">
                                                    <table style="margin: auto; width: 90%;">
                                                        <tr>
                                                            <td>
                                                                <table width="100%" style="padding-bottom: 1%;">
                                                                    <tr>
                                                                        <td class="Label" style="width: 20%; text-align: right;">User Profile :</td>
                                                                        <td style="text-align: left; width: 30%;">
                                                                            <asp:DropDownList ID="DDLUserprofile" runat="server" CssClass="dropDownList" Width="90%" AutoPostBack="true">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td class="Label" style="text-align: right; width: 10%;">Study Personal :</td>
                                                                        <td style="text-align: left; width: 40%;">
                                                                            <asp:DropDownList ID="DDLUserstatus" runat="server" CssClass="dropDownList" Width="67%"
                                                                                AutoPostBack="true">
                                                                                <asp:ListItem Selected="True">All User</asp:ListItem>
                                                                                <asp:ListItem Value="Y">Involved</asp:ListItem>
                                                                                <asp:ListItem Value="N">Not Involved </asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:GridView ID="GV_UserStage_Edit" runat="server" style="display:none;" OnRowCreated="GV_UserStage_Edit_RowCreated"
                                                                            OnRowUpdating="GV_UserStage_Edit_RowUpdating" OnRowEditing="GV_UserStage_Edit_RowEditing"
                                                                            OnRowCommand="GV_UserStage_Edit_RowCommand" OnRowDataBound="GV_UserStage_Edit_RowDataBound"
                                                                            AutoGenerateColumns="False" OnRowCancelingEdit="GV_UserStage_Edit_RowCancelingEdit"
                                                                            OnRowDeleting="GV_UserStage_Edit_RowDeleting" EmptyDataText="No Data Found !">
                                                                            <Columns>
                                                                                 <asp:TemplateField HeaderText="Delete">
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="CHKDelete" runat="Server"></asp:CheckBox>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField DataField="nWorkspaceDefaultWorkflowUserId" HeaderText="WorkspaceDefaultWorkflowUserId">
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="vWorkspaceId" HeaderText="WorkspaceId" />
                                                                                <asp:BoundField DataField="DisplayProject" HeaderText="Project Name" />
                                                                                <asp:BoundField DataField="iUserId" HeaderText="UserId" />
                                                                                <asp:BoundField DataField="vUserName" HeaderText="UserName" />
                                                                                <asp:BoundField DataField="vUserTypeName" HeaderText="Profile" />
                                                                                <asp:BoundField DataField="iStageId" HeaderText="StageId" />
                                                                                <asp:BoundField DataField="vStageDesc" HeaderText="StageName" />
                                                                                <asp:BoundField DataField="dModifyOn" HeaderText="Modify On" DataFormatString="{0:dd-MMM-yyyy hh:mm tt}" />
                                                                                <asp:BoundField DataField="ModifierName" HeaderText="Modify By"/>
                                                                                <asp:TemplateField HeaderText="Delete" SortExpression="status">
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton ID="ImgDelete" runat="server" ImageUrl="~/images/i_delete.gif"  OnClientClick="return ActivityDelete(this);">
                                                                                        </asp:ImageButton>
                                                                                        <%--OnClientClick="return confirm('Are You sure You want to DELETE?')"--%>
                                                                                         
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField DataField="isactiveUser" HeaderText="Is Active" />
                                                                                <asp:BoundField DataField="tableName" HeaderText="Table Name" />
                                                                                <asp:BoundField DataField="iNodeId" HeaderText="Table Name" />
                                                                            </Columns>
                                                                        </asp:GridView>

                                                                        <div id ="createtable"> 
                                                                        </div>
                                                                        <asp:Button ID="BtnDelete" OnClick="BtnDelete_Click" runat="server" Text="Delete"
                                                                            CssClass="btn btncancel" ToolTip="Delete" OnClientClick="return ActivityDelete(this);" Style="margin-left: 48%; margin-top: 1%;"></asp:Button>
                                                                        <asp:Button ID="Btnexptexcl" OnClick="Btnexptexcl_Click" runat="server"
                                                                            ToolTip="Export To Excel" CssClass="btn btnexcel" Style="margin-top: 1%;" />
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click"></asp:AsyncPostBackTrigger>
                                                                        <asp:PostBackTrigger ControlID="Btnexptexcl" />
                                                                        <asp:PostBackTrigger ControlID="DDLUserstatus" />
                                                                        <asp:PostBackTrigger ControlID="DDLUserprofile" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:HiddenField ID="hndLockStatus" runat="server" />
    <asp:HiddenField ID="hdnUserId" runat="server" Value="" />
        <asp:HiddenField ID="hdnSelectedUser" runat="server" Value="" />

    <div id="MasterLoader" class="modal MasterLoader"></div>

    <script src="Script/jquery-1.4.3.min.js" type="text/javascript"></script>
    <script src="Script/jquery-ui-1.10.4.custom.min.js" type="text/javascript"></script>
    <script src="Script/jquery.multiselect.min.js" type="text/javascript"></script>
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>

    <script language="javascript">

        function HidedivProjectwiseUserDetail() {
            $('#<%= img2.ClientID%>').click();
        }

        function MultiselectRequired() {
            $('#ctl00_CPHLAMBDA_ddlUserName').multiselect({
                includeSelectAllOption: true
            });
        }

        function ActivityDelete(e) {
            swal({
                title: "",
                text: "Are You Sure You Want To Delete Selected Items ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: '#EB7140',
                confirmButtonText: '',
                closeOnConfirm: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    __doPostBack(e.name, '');
                    swal.close();
                    return false;
                } else {
                    swal.close();
                    return true;
                }
            });

            //if (confirm("")) {

            //    return true;
            //}
            return false;
        }

        function ClearValue() {
            document.getElementById('<%= hdnUserId.ClientID%>').value = "";
        }


        function fnUserName() {
            var UserName = [];

            document.getElementById('<%= hdnUserId.ClientID%>').value = "";
            for (i = 0; i < $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlUserName']:checked").length ; i++) {
                UserName.push("" + $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlUserName']:checked").eq(i).attr("value") + "");
            }
            document.getElementById('<%= hdnUserId.ClientID%>').value = UserName;
            return true;
        }

        var UserName = [];
        function fnApplyUserName() {
            $("#<%= ddlUserName.ClientID%>").multiselect({
                noneSelectedText: "--Select User Name--",
                click: function (event, ui) {
                    if (ui.checked == true)
                        UserName.push("'" + ui.value + "'");
                    else if (ui.checked == false) {
                        if ($.inArray("'" + ui.value + "'", UserName) >= 0)
                            UserName.splice(UserName.indexOf("'" + ui.value + "'"), 1)
                    }
                    if ($("input[name$='ddlUserName']").length > 0) {
                    }
                },
                checkAll: function (event, ui) {
                    UserName = [];
                    for (var i = 0; i < event.target.options.length; i++) {
                        UserName.push("'" + $(event.target.options[i]).val() + "'")
                    }

                },
                uncheckAll: function (event, ui) {
                    UserName = [];

                }
            });

            $("#<%= ddlUserName.ClientID%>").multiselect("refresh");
            var CheckedCheckBox = document.getElementById('<%= hdnUserId.ClientID%>').value
            if (CheckedCheckBox != "") {

                CheckedCheckBox = CheckedCheckBox.split(',');
                for (i = 0; i <= CheckedCheckBox.length - 1; i++) {
                    $("#<%= ddlUserName.ClientID%>").multiselect("widget").find(".ui-multiselect-checkboxes.ui-helper-reset").find("input[value=" + CheckedCheckBox[i] + "]").attr("checked", "checked");

                }
                $('#<%= ddlUserName.ClientID%>').multiselect("update");
            }
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

        function UIGV_UserStage_Edit() {
            $('#<%= GV_UserStage_Edit.ClientID%>').removeAttr('style', 'display:block');
            oTab = $('#<%= GV_UserStage_Edit.ClientID%>').prepend($('<thead>').append($('#<%= GV_UserStage_Edit.ClientID%> tr:first'))).dataTable({
                "bJQueryUI": true,
                "bDestroy": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": false,
                "sScrollY": "250px",
                "sScrollX": "100%",
                "bScrollCollapse": true,
                aLengthMenu: [
                    [10, 25, 50, 100, -1],
                    [10, 25, 50, 100, "All"]
                ],
            });
            return false;
        }

        function pageLoad() {
            getData();
            MultiselectRequired();
            fnApplyUserName();
        }

        function SelectAll(CheckBoxControl) {
            if (CheckBoxControl.checked == true) {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {

                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf('GV_UserStage') > -1)) {
                        if (document.forms[0].elements[i].disabled == false) {
                            document.forms[0].elements[i].checked = true;
                        }


                    }


                }

            }

            else {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf('GV_UserStage') > -1)) {
                        document.forms[0].elements[i].checked = false;
                    }

                }
            }
        }

        function Validation() {
            var returnval  = true 
            var WorkspaceID = getParameterByName('WorkspaceId');
            var ddlUserName = document.getElementById('<%=ddlUserName.ClientID%>');
            var result = false;
            if ($("#<%= hdnUserId.ClientID%>").val() == "") {
                msgalert('Please Select Atleast One User !');
                return false;
            }
            if ($('#ctl00_CPHLAMBDA_chkTrainingAssign').is(":checked") == true) {
                $.ajax({
                    type: "post",
                    url: "frmWorkspaceDefaultWorkflowUserDtl.aspx/Proc_GetWorkSpaceProjectTrainingGuidline",
                    data: '{"vWorkSpaceId":"' + WorkspaceID + '"}',
                    contentType: "application/json; charset=utf-8",
                    datatype: JSON,
                    async: false,
                    dataType: "json",
                    success: function (data) {
                        if (data.d == "false") {
                            msgalert("Please Upload Document or Uncheck Training Assign !");
                            returnval = false;
                        }
                        else {
                            returnval = true;
                        }
                    },

                });
            }
            if (returnval == false) {
                return false;
            }
            else {
                return true;
            }            

        }

        //Add by shivani pandya for project lock

        function getParameterByName(name) {
            name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                results = regex.exec(location.search);
            return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        }

        function getData() {
            var WorkspaceID = getParameterByName('WorkspaceId');
            $.ajax({
                type: "post",
                url: "frmWorkspaceDefaultWorkflowUserDtl.aspx/LockImpact",
                data: '{"WorkspaceID":"' + WorkspaceID + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                dataType: "json",
                success: function (data) {

                    if (data.d == "L") {
                        $("#<%=hndLockStatus.ClientID%>").val("Lock");
                        $("#ctl00_CPHLAMBDA_BtnDelete").attr("Disabled", "Disabled");
                        $("#ctl00_CPHLAMBDA_GV_UserStage_Edit [type=image]").attr("style", "Display:none");
                        $("#ctl00_CPHLAMBDA_btnSave").attr("Disabled", "Disabled");
                        $("#ctl00_CPHLAMBDA_GV_UserStage_Edit [type=checkbox]").attr("Disabled", "Disabled");
                    }
                    if (data.d == "U") {
                        $("#<%=hndLockStatus.ClientID%>").val("UnLock");
                    }
                },
                failure: function (response) {
                    msgalert(response.d);
                },
                error: function (response) {
                    msgalert(response.d);
                }
            });
            return true;
        }


        function fnGetData() {
            var condition1 = "";
            $('.MasterLoader').show();
            var wstr = "cStatusIndi <> 'D'";

            var qs = getQueryStrings();
            var Type = qs["Type"];
            var WorkSpaceID = qs["WorkspaceId"];

            if ($("#ctl00_CPHLAMBDA_DDLUserprofile").val() != 0) {
                wstr = wstr + " and vUserTypeName = '" + $("#ctl00_CPHLAMBDA_DDLUserprofile :selected").text() + "'"
            }


            if ($("#ctl00_CPHLAMBDA_DDLUserstatus").val() != "All User") {
                wstr = wstr + " and IsactiveUser = '" + $("#ctl00_CPHLAMBDA_DDLUserstatus :selected").val() + "'"
            }

            $.ajax({
                type: "post",
                url: "frmWorkspaceDefaultWorkflowUserDtl.aspx/View_WorkSpaceDefaultUserDtl",
                data: '{"wstr":"' + wstr + '",Type: "'+ Type +'",WorkSpaceID: "'+ WorkSpaceID +'"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                 async: false,
                dataType: "json",
                success: function (data) {
                    var data = data.d;
                    var msgs = JSON.parse(data);
                    summarydata = msgs;
                    if (summarydata == "") return false;
                    CreateSummaryTable(summarydata);
                    $('.MasterLoader').hide();
                    return false;
                },
                failure: function (response) {
                    msgalert("failure");
                    msgalert(data.d);
                },
                error: function (response) {
                    msgalert("error");
                }
            });
            return false;
        }

        function CreateSummaryTable(summarydata) {
            var ActivityDataset = [];
            var jsondata = summarydata.VIEW_WORKSPACEDEFAULTWORKFLOWUSERDTL_NEW;
            
            if (jsondata.length == 0) {
                $("#ctl00_CPHLAMBDA_BtnDelete").attr("style", "display:none");
                $("#ctl00_CPHLAMBDA_Btnexptexcl").attr("style", "display:none");
            }
            else {
                $("#ctl00_CPHLAMBDA_BtnDelete").attr("style", "display:inline;margin-left: 48%;margin-top: 1%;");
                $("#ctl00_CPHLAMBDA_Btnexptexcl").attr("style", "display:inline;margin-top: 1%;");
            }



            for (var Row = 0; Row < jsondata.length; Row++) {
                var abc = '<asp:CheckBox ID="CHKDelete" runat="Server"></asp:CheckBox>'
                var InDataset = [];
                InDataset.push(abc, jsondata[Row]['nWorkspaceDefaultWorkflowUserId'], jsondata[Row]['vWorkspaceId'], jsondata[Row]['vWorkspaceDesc'],
                                   jsondata[Row]['iUserId'], jsondata[Row]['vUserName'], jsondata[Row]['vUserTypeName'], jsondata[Row]['iStageId'],
                                   jsondata[Row]['vStageDesc'], jsondata[Row]['dModifyOn'], jsondata[Row]['ModifierName'], '',
                                   jsondata[Row]['IsactiveUser'], jsondata[Row]['tableName'], jsondata[Row]['iNodeId'], '',
                                   '','');
                ActivityDataset.push(InDataset);

            }

            $ = jQuery;
            var createtable1 = $("<table id='Activityrecord'  border='1'  class='display'  cellspacing='0' width='100%'> </table>");
            $("#createtable").empty().append(createtable1);

            $('#Activityrecord').DataTable({
                "bJQueryUI": true,
                "sScrollY": "285px",
                "sScrollX": "100%",
                "bScrollCollapse": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": false,
                "aaData": ActivityDataset,
                "fnCreatedRow": function (nRow, aData, iDataIndex) {
                    $('td:eq(10)', nRow).append("<input type='image' id='ImgDelete" + iDataIndex + "' name='ImgDelete$" + iDataIndex
                                                + "' src='Images/i_delete.gif'; OnClick='DeleteCellRow(this); return false;'  style='border-width:0px;' nWorkspaceDefaultWorkflowUserId = " + aData[1]
                                                + " iNodeId = " + aData[14] + " vWorkSpaceID = " + aData[2] + " iUserID = " + aData[4] + " iStageID = "
                                                + aData[7] + " vtablename = "+ aData[13] +">");
                },
                "aoColumns": [
                    { "sTitle": "Delete"},
                    { "sTitle": "WorkspaceDefaultWorkflowUserId", "bVisible": false },
                    { "sTitle": "WorkspaceId", "bVisible": false },
                    { "sTitle": "Project Name" },
                    { "sTitle": "UserId", "bVisible": false },
                    { "sTitle": "UserName" },
                    { "sTitle": "Profile" },
                    { "sTitle": "StageId", "bVisible": false },
                    { "sTitle": "StageName" },
                    { "sTitle": "Training Assigned On", "bVisible": false },
                    { "sTitle": "Training Assigned By", "bVisible": false },
                    { "sTitle": "Training Started On", "bVisible": false },
                    { "sTitle": "Is Active", "bVisible": false },
                    { "sTitle": "Table Name", "bVisible": false },
                    { "sTitle": "Table Name", "bVisible": false },
                    { "sTitle": "Training Completed On", "bVisible": false },
                    { "sTitle": "Duration(min)", "bVisible": false },
                    { "sTitle": "Delete" },
                ],

                
                "oLanguage": {
                    "sEmptyTable": "No Record Found"
                },
            });

            $('#Activityrecord').show();
            $('#loadingmessage').hide();
        }

        function getQueryStrings() {
            var assoc = {};
            var decode = function (s) { return decodeURIComponent(s.replace(/\+/g, " ")); };
            var queryString = location.search.substring(1);
            var keyValues = queryString.split('&');

            for (var i in keyValues) {
                var key = keyValues[i].split('=');
                if (key.length > 1) {
                    assoc[decode(key[0])] = decode(key[1]);
                }
            }

            return assoc;
        }


        function DeleteCellRow(e) {
            var workspaceflow = e.attributes.nWorkspaceDefaultWorkflowUserId.value;
            var vWorkspaceId = e.attributes.vWorkspaceId.value;
            var iUserId = e.attributes.iUserId.value;
            var iStageId = e.attributes.iStageId.value;
            var vtablename = e.attributes.vtablename.value;
            var iNodeId = e.attributes.iNodeId.value;

            $.ajax({
                type: "post",
                url: "frmWorkspaceDefaultWorkflowUserDtl.aspx/Delete_WorkSpaceFlow",
                data: "{'workspaceflowid':'" + workspaceflow + "','vWorkspaceId':'" + vWorkspaceId + "','vtablename':'" + vtablename + "','iUserId':'" + iUserId + "','iStageId':'" + iStageId + "'}",
                //,,,iNodeId':'" + iNodeId + "'
                contentType: "application/json; charset=utf-8",
                async: false,
                dataType: "json",
                success: function (data) {
                    fnGetData();
                    msgalert("Attribute deleted sucessfully !!");

                },
                failure: function (response) {
                    msgalert("failure");
                    msgalert(data.d);
                },
                error: function (response) {
                    msgalert("error");
                }
            });
        }

        function filluser() {

            $('.MasterLoader').show();
            var wstr = "cStatusIndi <> 'D'";

            var qs = getQueryStrings();
            var Type = qs["Type"];
            var WorkSpaceID = qs["WorkspaceId"];


            if ($("#ctl00_CPHLAMBDA_DdlUserType").val() != 0) {
                wstr = wstr + " and vUserTypeCode = '" + $("#ctl00_CPHLAMBDA_DdlUserType").val() + "'"
            }


            if ($("#ctl00_CPHLAMBDA_DdlLocation").val() != "0") {
                wstr = wstr + " and vLocationCode = '" + $("#ctl00_CPHLAMBDA_DdlLocation :selected").val() + "'"
            }


            $('#ctl00_CPHLAMBDA_ddlUserName option').each(function () {
                $(this).remove();
            });



            $.ajax({
                type: "post",
                url: "frmWorkspaceDefaultWorkflowUserDtl.aspx/FilluserWeb",
                data: '{"wstr":"' + wstr + '",Type: "' + Type + '",WorkSpaceID: "' + WorkSpaceID + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                dataType: "json",
                success: function (data) {
                    var data = data.d;
                    var msgs = JSON.parse(data);
                    var jsondata = msgs.VIEW_USERMST;

                    for (i = 0; i < jsondata.length; i++)
                    {
                        $("#ctl00_CPHLAMBDA_ddlUserName").append($("<option></option>").val(jsondata[i].iUserId).html(jsondata[i].vUserName));
                    }
                    $('.MasterLoader').hide();
                    fnApplyUserName();
                    return false;
                },
                failure: function (response) {
                    msgalert("failure");
                    msgalert(data.d);
                },
                error: function (response) {
                    msgalert("error");
                }
            });
        }

        function FillDropDownUserProfile() {
            
            var wstr = "cStatusIndi <> 'D'";

            var qs = getQueryStrings();
            var Type = qs["Type"];
            var WorkSpaceID = qs["WorkspaceId"];

            $.ajax({
                type: "post",
                url: "frmWorkspaceDefaultWorkflowUserDtl.aspx/FillDropDownUserProfileWEB",
                data: '{"wstr":"' + wstr + '",Type: "' + Type + '",WorkSpaceID: "' + WorkSpaceID + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                dataType: "json",
                success: function (data) {
                    var data = data.d;
                    var msgs = JSON.parse(data);
                    var jsondata = msgs.VIEW_WORKSPACEDEFAULTWORKFLOWUSERDTL_NEW;


                    $("#ctl00_CPHLAMBDA_DDLUserprofile").empty().append('<option selected="selected" value="0">All Profile</option>');
                    for (i = 0; i < jsondata.length; i++)
                    {
                        $("#ctl00_CPHLAMBDA_DDLUserprofile").append($("<option></option>").val(jsondata[i].vUserTypeName).html(jsondata[i].vUserTypeName));
                    }
                    fnApplyUserName();
                    return false;
                },
                failure: function (response) {
                    msgalert("failure");
                    msgalert(data.d);
                },
                error: function (response) {
                    msgalert("error");
                }
            });
        }

        function GetUsers() {
            $('.MasterLoader').show();
            filluser();
            FillDropDownUserProfile();
            $('.MasterLoader').hide();
        }
    </script>

</asp:Content>
