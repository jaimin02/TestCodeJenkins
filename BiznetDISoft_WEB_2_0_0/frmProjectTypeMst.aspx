<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmProjectTypeMst.aspx.vb" Inherits="frmProjectTypeMst" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/scrollablegrid.js"></script>
    <style>
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }
    </style>
    <asp:HiddenField runat="server" ID="hdnClientCode" />
    <div id="Div1" runat="server">
      <asp:GridView runat="server" ID="gvExport" AutoGenerateColumns="true" Style="display: none"
            HeaderStyle-BackColor="#1560a1" HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" Header-style-font-size="10px !importnat" HeaderStyle-Font-Names="Verdana, Arial, Helvetica, sans-serif"
            HeaderStyle-Font-Size=" 0.9em"
            HeaderStyle-HorizontalAlign="Center" CellPadding="3" CellSpacing="0"
            RowStyle-Font-Names=" Verdana, Arial, Helvetica, sans-serif" RowStyle-Font-Size="10pt"
            RowStyle-BackColor="#84c8e6" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#191970" RowStyle-ForeColor="#191970">
        </asp:GridView>
    </div>
    <asp:Button ID="btnExportToExcel" runat="server" Style="display: none;" />
    <table style="width: 100%; padding-top: 2%;" cellpadding="5px">
        <tr>
            <td>
                <fieldset class="FieldSetBox" style="display: block; width: 98%; text-align: left; border: #aaaaaa 1px solid;">
                    <legend class="LegendText" style="color: Black; font-size: 12px">
                        <img id="img2" alt="Project Type Details" src="images/panelcollapse.png"
                            onclick="Display(this,'divProjectType');" runat="server" style="margin-right: 2px;" />Project Type Details</legend>
                    <div id="divProjectType">
                        <table width="100%">
                            <tr>
                                <td class="Label" style="width: 32%; text-align: right;">Project Type Name* :
                                </td>
                                <td style="text-align: left; width: 19%;">
                                    <asp:DropDownList ID="ddlProjectType" runat="server" CssClass="dropDownList" Width="83%"></asp:DropDownList>
                                    <%--<asp:TextBox ID="txtProjectTypeName" runat="server" CssClass="textBox" Width="30%"
                    MaxLength="50" />--%>
                                </td>
                                <td style="text-align: right; width: 11%;" class="Label">Project Sub Type :
                                </td>
                                <td style="text-align: left; width: 38%;">
                                    <asp:TextBox ID="txtProjectSubType" runat="server" CssClass="textBox" Width="42%" MaxLength="250" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 32%;" class="Label">Project Type Suffix :
                                </td>
                                <td style="text-align: left; width: 19%;">
                                    <asp:TextBox ID="txtPSuffix" runat="server" CssClass="textBox" Width="81%" MaxLength="250" />
                                </td>
                                <div runat="server" id="trRemarks" visible="false">
                                    <td style="text-align: right; width: 11%;" class="Label">Remark  *:
                                    </td>
                                    <td style="text-align: left; width: 38%;">
                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="textBox" Width="42%" MaxLength="250" />
                                    </td>
                                </div>
                            </tr>
                            <tr>
                                <td style="text-align: center; padding-top: 1%;" class="Label" colspan="4">
                                    <asp:Button ID="BtnSave" runat="server" CssClass="btn btnsave" Text="Save" ToolTip="Save"
                                        OnClientClick="return  Validation(this.id);" />
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
    <asp:HiddenField ID="hdnProjectSubTypeCode" runat="server" Value=""></asp:HiddenField>
    <asp:UpdatePanel ID="Up_View" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <%--<div style="margin: 0 18%;">--%>
                <table width="100%" style="text-align: center;">
                    <tbody>
                        <tr>
                            <td>
                                <fieldset class="FieldSetBox" style="display: block; width: 98%; text-align: left; border: #aaaaaa 1px solid;">
                                    <legend class="LegendText" style="color: Black; font-size: 12px">
                                        <img id="img1" alt="Project Type Data" src="images/panelcollapse.png"
                                            onclick="Display(this,'divProjectData');" runat="server" style="margin-right: 2px;" />Project Type Data</legend>
                                    <div id="divProjectData">
                                        <table style="margin: auto; width: 80%;">
                                            <tr>
                                                <td colspan="2" style="width: 100%; text-align: center;">
                                                    <div style="width: 100%; overflow: auto;" class=" grid">
                                                        <asp:GridView ID="GV_ProjectType" runat="server" Style="width: 100%; margin: auto; display: none;"
                                                            OnRowDataBound="GV_ProjectType_RowDataBound"
                                                            OnRowCommand="GV_ProjectType_RowCommand"
                                                            AutoGenerateColumns="False">
                                                            <Columns>
                                                                <asp:BoundField HeaderText=" # ">
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="vProjectTypeCode" HeaderText="Project Type Code" />
                                                                <asp:BoundField DataField="vProjectTypeName" HeaderText="Project Type Name" />
                                                                <asp:BoundField DataField="vProjectSubTypeName" HeaderText="Project Sub Type" />
                                                                <asp:BoundField DataField="vProjectTypeSuffix" HeaderText="Project Type Suffix" />
                                                                <asp:BoundField DataField="vProjectSubTypeCode" HeaderText="Project Sub Type Code" />
                                                                <asp:TemplateField SortExpression="status" HeaderText="Edit">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="lnkEdit" runat="server" ToolTip="Edit" ImageUrl="~/images/Edit2.gif" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Audit Trail">
                                                                    <ItemTemplate>
                                                                        <img src="Images/audit.png" onclick="AuditTrail(this.id);" id='<%# Session(S_UserID) + "+" + Eval("vProjectTypeCode") + "+" + Eval("vProjectSubTypeCode") %>' />
                                                                        <%--<asp:ImageButton ID='<%#  Eval("LoginName") + "+" + Eval("vUserTypeCode") %>' runat="server" ImageUrl="~/Images/audit.png" ToolTip="Audit Trial" />--%>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Export">
                                                                    <ItemTemplate>
                                                                        <center>
                                                                            <img src="images/Export.gif" onclick="ExportToExcel(this.id);" id='<%# Session(S_UserID) + "+" + Eval("vProjectTypeCode") + "+" + Eval("vProjectSubTypeCode") %>' />
                                                                            <%--<img src="images/Export.gif" onclick="ExportToExcel();" runat="server" id="img" />--%>
                                                                            <%--<asp:ImageButton ID="imgExport" runat="server" ImageUrl="images/Export.gif" ToolTip="Export To Excel " />--%>
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
            <%--</div>--%>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnSave" />
        </Triggers>
    </asp:UpdatePanel>

    <button id="btnAuditTrail" runat="server" style="display: none;" />

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
    

    <script language="javascript" type="text/javascript">

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

        function Validation(id) {
            if ($("#ctl00_CPHLAMBDA_ddlProjectType")[0].selectedIndex == 0) {
                msgalert('Please Select Project Type Name !');
                document.getElementById('ctl00_CPHLAMBDA_ddlProjectType').focus();
                return false;
            }
            if ($('#' + id).val().toUpperCase() == "UPDATE") {
                if ($("#ctl00_CPHLAMBDA_txtRemark").val() == '') {
                    msgalert('Please Enter Remarks !')
                    return false;
                }
            }
            return true;
        }

        function ShowAlert(msg) {
            //msgalert(msg);
            // window.location.href = "frmProjectTypeMst.aspx?mode=1";
            alertdooperation(msg, 1, "frmProjectTypeMst.aspx?mode=1");
        }

        function FillProjectType() {
            $('#<%= GV_ProjectType.ClientID%>').removeAttr('style', 'display:block');

            oTab = $('#<%= GV_ProjectType.ClientID%>').prepend($('<thead>').append($('#<%= GV_ProjectType.ClientID%> tr:first'))).dataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": false,
                "sScrollY": "250px",
                "sScrollX": "100%",
                "bScrollCollapse": true,
            });

            //alert(oTab.fnSettings());
            return false;
        }


        function AuditTrail(id) {
            var ProjectTypeCode = id.split('+')[1]
            var ProjectSubTypeCode = id.split('+')[2]
            var UserId = id.split('+')[0]
            $.ajax({
                type: "post",
                url: "frmProjectTypeMst.aspx/AuditTrailForProjectSubType",
                data: '{"ProjectTypeCode":"' + ProjectTypeCode + '","ProjectSubTypeCode":"' + ProjectSubTypeCode + '","iUserId":"' + UserId + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                success: function (data) {
                    $('#tblActivityAudit').attr("IsTable", "has");
                    var aaDataSet = [];
                    var RowId
                    if (data != "" && data != null) {
                        data = JSON.parse(data.d);
                        for (var Row = 0; Row < data.length; Row++) {
                            var InDataSet = [];
                            InDataSet.push(data[Row].SrNo, data[Row].vProjectTypeName, data[Row].vProjectSubTypeName, data[Row].vProjectTypeSuffix, data[Row].vRemark, data[Row].ModifyBy, data[Row].Modifyon);
                            aaDataSet.push(InDataSet);
                        }

                        if ($("#tblAudit").children().length > 0) {
                            $("#tblAudit").dataTable().fnDestroy();
                        }
                        $('#tblAudit').prepend($('<thead>').append($('#tblAudit tr:first'))).dataTable({
                            "bJQueryUI": true,
                            "sPaginationType": "full_numbers",
                            "bLengthChange": true,
                            "iDisplayLength": 10,
                            "bProcessing": true,
                            "bSort": false,
                            "bDestory": true,
                            "bRetrieve": true,
                            "aaData": aaDataSet,
                            aLengthMenu: [
                                [10, 25, 50, 100, -1],
                                [10, 25, 50, 100, "All"]
                            ],
                            "aoColumns": [
                                { "sTitle": "Sr.No." },
                                { "sTitle": "Project TypeName" },
                                { "sTitle": "Project SubTypeName" },
                                { "sTitle": "Project TypeSuffix" },

                                { "sTitle": "Remark" },
                                { "sTitle": "Modify By" },
                                { "sTitle": "Modify On" },

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
        function ExportToExcel(ID) {
            $("#<%= hdnClientCode.ClientID %>").val(ID);
            var btn = document.getElementById('<%= btnExportToExcel.ClientId()%>')
            btn.click();
        }
    </script>

</asp:Content>
