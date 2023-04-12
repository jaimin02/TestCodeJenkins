<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmUserGroupMst.aspx.vb" Inherits="frmUserGroupMst" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

        .ui-dialog .ui-dialog-content {
            width: auto;
            min-height: 50px;
            max-height: 155px;
            overflow: scroll;
            text-align: left;
        }

        /*#ctl00_CPHLAMBDA_GV_UserGroupMst_wrapper {
            margin: 0px 235px;
        }*/
    </style>    

    <table style="width: 100%; margin-bottom: 2%;" cellpadding="5px">
        <tr>
            <td>
                <fieldset class="FieldSetBox" style="display: block; width: 98%; text-align: left; border: #aaaaaa 1px solid;">
                    <legend class="LegendText" style="color: Black; font-size: 12px">
                        <img id="img2" alt="User Group Detail" src="images/panelcollapse.png"
                            onclick="Display(this,'divUserGroup');" runat="server" style="margin-right: 2px;" />User Group Detail</legend>
                    <div id="divUserGroup">
                        <table width="100%">
                            <tr>
                                <td class="Label" style="width: 25%; text-align: right;">User Group Name* :
                                </td>
                                <td style="text-align: left; width: 25%;">
                                    <asp:TextBox ID="txtUsrGroupName" runat="server" CssClass="textBox" Width="99%" MaxLength="50" />
                                </td>
                                <td style="text-align: right; width: 10%;" class="Label">Location* :
                                </td>
                                <td style="text-align: left; width: 35%;">
                                    <asp:DropDownList ID="DDLLocation" runat="server" CssClass="dropDownList" Width="48%" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 25%;" class="Label">Project Type Code* :
                                </td>
                                <td style="text-align: left; width: 25%;">
                                    <asp:DropDownList ID="DDLProjType" runat="server" CssClass="dropDownList" Width="100%" />
                                </td>
                                <td style="text-align: right; width: 10%;" class="Label">Remarks :
                                </td>
                                <td style="text-align: left; width: 35%;">
                                    <input id="TxtRemark" runat="server" class="textBox" style="width: 47%" type="text"
                                        maxlength="250" />
                                </td>
                            </tr>
                            <tr>

                                <td align="center" class="Label" colspan="4" style="padding-top: 1%">
                                    <asp:Button ID="BtnSave" runat="server" CssClass="btn btnsave" Text="Save" ToolTip="Save"
                                        OnClientClick=" return Validation();" />
                                    <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" Text="Cancel" ToolTip="Cancel" />
                                    <asp:Button ID="BtnExit" runat="server" CssClass="btn btnexit" Text="Exit" ToolTip="Exit"
                                        OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </fieldset>
            </td>
        </tr>
    </table>


    <div id="Div1" runat="server">

        <asp:GridView runat="server" ID="gvExport" AutoGenerateColumns="true" Style="display: none"
            HeaderStyle-BackColor="#1560a1" HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" HeaderStyle-Font-Names="Verdana, Arial, Helvetica, sans-serif"
            HeaderStyle-HorizontalAlign="Center" CellPadding="3" CellSpacing="0"
            RowStyle-Font-Names=" Verdana, Arial, Helvetica, sans-serif" RowStyle-Font-Size="10pt"
            RowStyle-BackColor="#84c8e6" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#191970" RowStyle-ForeColor="#191970">
        </asp:GridView>
    </div>

    <asp:UpdatePanel ID="Up_Grid" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table align="center" width="100%">
                <tbody style="width:100%">
                    <tr>
                        <td style="width:100%">
                            <fieldset class="FieldSetBox" style="display: block; width: 98%; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img1" alt="User Group Data" src="images/panelcollapse.png"
                                        onclick="Display(this,'divUserData');" runat="server" style="margin-right: 2px;" />User Group Data</legend>
                                <div id="divUserData" style="width:100%">
                                    <table width="100%">
                                        <tr>
                                            <td align="center" colspan="2">
                                                <div style="height: 100%;margin:auto; width: 85%; overflow: 0 auto;" class="grid">
                                                <asp:GridView ID="GV_UserGroupMst" runat="server" Style="width: 50%; margin: auto;" OnRowCreated="GV_UserGroupMst_RowCreated"
                                                    AutoGenerateColumns="False" OnRowCommand="GV_UserGroupMst_RowCommand" OnRowDataBound="GV_UserGroupMst_RowDataBound"
                                                    OnPageIndexChanging="GV_UserGroupMst_PageIndexChanging">
                                                    <Columns>
                                                        <asp:BoundField HeaderText=" # ">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="iUserGroupCode" HeaderText="User Group Code" />
                                                        <asp:BoundField DataField="vUserGroupName" HeaderText="User Group Name" />
                                                        <asp:BoundField DataField="vLocationCode" HeaderText="Location Code" />
                                                        <asp:BoundField DataField="vProjectTypeCode" HeaderText="ProjectType Code" />
                                                        <asp:BoundField DataField="vRemark" HeaderText="Remarks">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField SortExpression="status" HeaderText="Edit">
                                                            <ItemTemplate>
                                                                <%--<asp:LinkButton ID="lnkEdit" Text="Edit" runat="server"></asp:LinkButton></ItemTemplate>--%>
                                                                <asp:ImageButton ID="lnkEdit" runat="server" ImageUrl="~/images/Edit2.gif" ToolTip="Edit" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Audit Trail">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="lnkAudit" runat="server" ImageUrl="~/Images/audit.png" ToolTip="Audit Trial" OnClientClick="AudtiTrail(this); return false;" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Export">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <img src="images/Export.gif" onclick="ExportToExcel(this.id);" id='<%#  Eval("iUserGroupCode") %>' />
                                                                    <%--<asp:ImageButton ID="imgExport"  runat="server" ImageUrl="images/Export.gif" ToolTip="Export To Excel " />--%>
                                                                </center>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                                    </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                </tbody>
            </table>


            <div>
                <table>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpUserGroupAuditTrail" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <div id="dvUserGroupMstAudiTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
                                        <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                            <tr>
                                                <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 97%;">
                                                    <asp:Label ID="lblRamge" runat="server" Text="Audit Trail Information"></asp:Label>
                                                </td>
                                                <td style="width: 3%">
                                                    <img id="imgClientAuditTrail" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
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
                                                                <table id="tblUserGroupMstAudit" class="tblAudit" width="100%" style="background-color: aliceblue;"></table>
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
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </div>
            <button id="btn3" runat="server" style="display: none;" />
            <cc1:ModalPopupExtender ID="MPE_UserGroupMstHistory" runat="server" PopupControlID="dvUserGroupMstAudiTrail" BehaviorID="MPE_UserGroupMstHistory"
                PopupDragHandleControlID="LbldRandomizationDtl" BackgroundCssClass="modalBackground" CancelControlID="imgClientAuditTrail"
                TargetControlID="btn3">
            </cc1:ModalPopupExtender>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />

        </Triggers>
    </asp:UpdatePanel>
    <asp:HiddenField runat="server" ID="hdnUsergroupCode" />

    <asp:Button ID="btnExportToExcel" runat="server" Style="display: none;" />
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/scrollablegrid.js"></script>

    <script type="text/javascript" language="javascript">

        function HideSponsorDetails() {
            $('#<%= img2.ClientID%>').click();
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

        function Validation() {
            if (document.getElementById('<%=txtUsrGroupName.ClientID%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=txtUsrGroupName.ClientID%>').value = '';
                 msgalert('Please Enter User Group Name !');
                document.getElementById('<%=txtUsrGroupName.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=DDLLocation.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Location !');
                document.getElementById('<%=DDLLocation.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=DDLProjType.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Project Type !');
                document.getElementById('<%=DDLProjType.ClientID%>').focus();
                return false;
            }
    if (document.getElementById("<%=btnSave.ClientID%>").value.trim() == "Update") {
                if (document.getElementById("<%=txtremark.ClientID%>").value.trim() == "") {
            msgalert("Please Enter Remarks !");
            return false;
        }
    }
    return true;
    return true;
}

function UIGV_UserGroupMst() {
    $('#<%= GV_UserGroupMst.ClientID%>').removeAttr('style', 'display:block');
    oTab = $('#<%= GV_UserGroupMst.ClientID%>').prepend($('<thead>').append($('#<%= GV_UserGroupMst.ClientID%> tr:first'))).dataTable({
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
    return false;
}

        function ShowAlert(msg) {
            alertdooperation(msg, 1, "frmUserGroupMst.aspx?mode=1");
   // msgalert(msg);
    //window.location.href = "frmUserGroupMst.aspx?mode=1";
}
function AudtiTrail(e) {
    debugger;
    var VUserGroupCode = $("#" + e.id).attr("iusergroupcode");

    if (VUserGroupCode != "") {
        $.ajax({
            type: "post",
            url: "frmUserGroupMst.aspx/AuditTrail",
            data: '{"VUserGroupCode":"' + VUserGroupCode + '"}',
            contentType: "application/json; charset=utf-8",
            datatype: JSON,
            async: false,
            success: function (data) {
                $('#tblUserGroupMstAudit').attr("IsTable", "has");
                var aaDataSet = [];
                var range = null;
                //if (data.d != "" && data.d != null) {
                data = JSON.parse(data.d);
                for (var Row = 0; Row < data.length; Row++) {
                    var InDataSet = [];
                    InDataSet.push(data[Row].SrNo, data[Row].UserGroupName, data[Row].Remarks, data[Row].ModifyBy, data[Row].ModifyOn);
                    aaDataSet.push(InDataSet);
                }

                // }
                if ($("#tblUserGroupMstAudit").children().length > 0) {
                    $("#tblUserGroupMstAudit").dataTable().fnDestroy();
                }

                oTable = $('#tblUserGroupMstAudit').prepend($('<thead>').append($('#tblUserGroupMstAudit tr:first'))).dataTable({

                    "bJQueryUI": true,
                    "sPaginationType": "full_numbers",
                    "bLengthChange": true,
                    "iDisplayLength": 10,
                    "bProcessing": true,
                    "bSort": false,
                    "aaData": aaDataSet,
                    aLengthMenu: [
                            [10, 25, 50, 100, -1],
                            [10, 25, 50, 100, "All"]
                    ],
                    "aoColumns": [
                        {
                            "sTitle": "#",
                        },

                         { "sTitle": "User Group Name" },
                        { "sTitle": "Remarks" },
                        { "sTitle": "Modify By" },
                        { "sTitle": "Modify On" },

                    ],
                    "aoColumnDefs": [
                                { 'bSortable': false, 'aTargets': [0] }
                    ],
                    "oLanguage": {
                        "sEmptyTable": "No Record Found",
                    }

                });
                oTable.fnAdjustColumnSizing();
                $('.DataTables_sort_wrapper').click;
                $find('MPE_UserGroupMstHistory').show();

            },
            failure: function (response) {
                msgalert(response.d);
            },
            error: function (response) {
                msgalert(response.d);
            }
        });
    }
    return false;

}

function ExportToExcel(id) {
    $("#<%= hdnUsergroupCode.ClientID %>").val(id);
    var btn = document.getElementById('<%= btnExportToExcel.ClientId()%>')
    btn.click();
}

    </script>

</asp:Content>
