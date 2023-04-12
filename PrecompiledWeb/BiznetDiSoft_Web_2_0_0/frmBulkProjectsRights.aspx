<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmBulkProjectsRights, App_Web_2mzu20n4" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <script type="text/javascript" src="Script/jquery-1.4.3.min.js"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.plugins.js"></script>
    <script type="text/javascript" language="javascript">
        function SelectAllFields() {
            var chkSelectAll = document.getElementById('<%=ChkBoxAllProject.clientid%>').checked;
            var chklst = document.getElementById('<%=ChkBoxLstProjectNo.clientid%>');
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

        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        } 

         function AuditTrail() {
            var txtProject = document.getElementById('<%=txtproject.ClientID%>');
            if (txtProject.value == "") {
                msgalert('Please Select Project No!');
                return false;
            }

            var vWorkSpaceId = $("#ctl00_CPHLAMBDA_HProjectId").val()
         ajaxData = {
                type: "POST",
                url: "frmBulkProjectsRights.aspx/AuditTrail",
                data: '{"vWorkSpaceId":"' + vWorkSpaceId + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                success: successBulkProjectRightsAuditTrailDetails,
                error: errorBulkProjectRightsAuditTrailDetails
            }
         getBulkProjectRightsAuditTrailDetails(ajaxData.type, ajaxData.url, ajaxData.data, ajaxData.contentType, ajaxData.datatype, ajaxData.async, ajaxData.success, ajaxData.error)
        };

        var getBulkProjectRightsAuditTrailDetails = function (type, url, data, contentType, datatype, async, success, error) {
            $.ajax({
                type: type,
                url: url,
                data: data,
                contentType: contentType,
                datatype: datatype,
                async: async,
                success: success,
                error: error
            });
        }

        function successBulkProjectRightsAuditTrailDetails(jsonAuditTrailData) {
            var data = JSON.parse(jsonAuditTrailData.d);
            debugger;
            if (data.Table == "") {
                msgalert("No Audit Trail Data Available for Bulk Project Rights")
            }
            else {
                var activityDataSet = [];
                for (var v = 0, l = data.VIEW_WORKSPACEDEFAULTWORKFLOWUSERDTL.length; v < l; v++) {
                    var inDataSet = [];
                    inDataSet.push(data.VIEW_WORKSPACEDEFAULTWORKFLOWUSERDTL[v].ProjectNo, data.VIEW_WORKSPACEDEFAULTWORKFLOWUSERDTL[v].vUserName,
                        data.VIEW_WORKSPACEDEFAULTWORKFLOWUSERDTL[v].vUserTypeName, data.VIEW_WORKSPACEDEFAULTWORKFLOWUSERDTL[v].ModifierName,
                        data.VIEW_WORKSPACEDEFAULTWORKFLOWUSERDTL[v].dModifyOn);
                    activityDataSet.push(inDataSet);
                }
                var oTable = $('#tblBulkProjectRightsAuditTrail').dataTable({
                    "bJQueryUI": true,
                    "sPaginationType": "full_numbers",
                    "bLengthChange": true,
                    "iDisplayLength": 10,
                    "bProcessing": true,
                    "bSort": true,
                    "autoWidth": false,
                    "aaData": activityDataSet,
                    "bInfo": true,
                    "bDestroy": true,
                    "sScrollX": "100%",
                    "sScrollXInner": "1250",
                    "fnCreatedRow": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                        if (aData[6] == 1) {
                            $(nRow).addClass('Parent');
                        }
                        else {
                            $(nRow).addClass('Child');
                        }
                    },
                    "aoColumns": [
                       { "sTitle": "Project No" },
                        { "sTitle": "User Name" },
                        { "sTitle": "User Profiles" },
                        { "sTitle": "Modify By" },
                        { "sTitle": "Modify On" },

                    ],
                    //"aoColumnDefs": [
                    //     { "bVisible": false, "aTargets": [6] },
                    //],

                    "oLanguage": {
                        "sEmptyTable": "No Record Found",
                    },
                });
                $find('mpeBulkProjectRightsAuditTrail').show();
                oTable.fnAdjustColumnSizing();
            }

        }

        function errorBulkProjectRightsAuditTrailDetails(e) {
            msgalert("ERROR : " + e)
        }

    </script>

    <div>
        <asp:UpdatePanel ID="UpdatePanel" runat="server">
            <ContentTemplate>
                <div id="DivRadioButton" style="width: 100%;">
                    <asp:Panel ID="PnlRadioButton" runat="server">
                        <table width="100%">
                            <tbody>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:RadioButtonList ID="Rdbtnlst" runat="server" AutoPostBack="True" RepeatDirection="Horizontal"
                                            OnSelectedIndexChanged="Rdbtnlst_SelectedIndexChanged">
                                            <asp:ListItem Value="requestid">Request Id</asp:ListItem>
                                            <asp:ListItem Value="projectno">Project No</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </asp:Panel>
                </div>
                <div id="DivFroMAndToTime" style="width: 100%;">
                    <table style="margin: auto;" cellpadding="5px">
                        <tbody>
                            <tr>
                                <td>
                                    <asp:Panel ID="PnlFromTime" runat="server" BorderColor="navy" BorderWidth="1px" Width="100%">
                                        <table>
                                            <tbody>
                                                <tr>
                                                    <td style="text-align: right" class="Label">
                                                        From :
                                                    </td>
                                                    <td style="text-align: left" align="left">
                                                        <asp:TextBox ID="TxtFromDate" CssClass="textBox" runat="server"></asp:TextBox><cc1:CalendarExtender
                                                            ID="CalExtFromDate" runat="server" Format="dd-MMM-yyyy" TargetControlID="TxtFromDate">
                                                        </cc1:CalendarExtender>
                                                    </td>
                                                    <td class="Label" style="text-align: right;">
                                                        To :
                                                    </td>
                                                    <td style="text-align: left" align="left">
                                                        <asp:TextBox ID="TxtToDate" runat="server" CssClass="textBox">
                                                        </asp:TextBox>
                                                        <cc1:CalendarExtender ID="CalExtToDate" runat="server" Format="dd-MMM-yyyy" TargetControlID="TxtToDate">
                                                        </cc1:CalendarExtender>
                                                    </td>
                                                    <td class="Label " style="text-align: left;">
                                                        <asp:CheckBox ID="ChkBoxAll" runat="server" Text="All" Checked="true"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="Label" style="text-align: left; white-space: nowrap;">
                                                        Project No /Request Id :
                                                    </td>
                                                    <td class="Label" colspan="4" style="text-align: left; white-space: nowrap;">
                                                        <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="42%">
                                                        </asp:TextBox><asp:Button Style="display: none" ID="btnSetProject" OnClick="btnSetProject_Click"
                                                            runat="server" Text=" Project"></asp:Button><asp:HiddenField ID="HProjectId" runat="server">
                                                            </asp:HiddenField>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtProject"
                                                            UseContextKey="True" ServicePath="AutoComplete.asmx" ServiceMethod="GetMyProjectCompletionList"
                                                            OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1"
                                                            CompletionListElementID="pnlProjectList" CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                            CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1">
                                                        </cc1:AutoCompleteExtender>
                                                        <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 300px; overflow: auto;
                                                            overflow-x: hidden" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <%--<td align="left" class="label" style="white-space: nowrap; text-align: left">
                                                    </td>--%>
                                                    <td align="left" class="Label " colspan="5" style="white-space: nowrap; text-align: center">
                                                        <asp:Button ID="BtnGo" runat="server" Text="" ToolTip="Go" CssClass="btn btngo" />
                                                        <asp:Button ID="BtnCancel" OnClick="BtnCancel_Click" runat="server" Text="Cancel"
                                                            ToolTip="Cancel" CssClass="btn btncancel" />
                                                        <asp:Button ID="btnAuditTrail" runat="server" CssClass="btn btnaudit" ToolTip="Audit Trail"
                                                            OnClientClick="return AuditTrail();" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <br />
                <div id="DivProjectRights">
                    <table style="margin: auto; width: 50%">
                        <tbody>
                            <tr>
                                <td style="text-align: left;">
                                    <asp:Label ID="LblProjectNo" runat="server" Text=""></asp:Label>
                                    <asp:Panel ID="PnlProjectNo" runat="server" Width="100%" Visible="false" BorderWidth="1px"
                                        BorderColor="navy" Height="230px" ScrollBars="Auto">
                                        <asp:CheckBox ID="ChkBoxAllProject" onclick="SelectAllFields()" runat="server" Font-Bold="True"
                                            Text="Select All"></asp:CheckBox>
                                        <asp:CheckBoxList ID="ChkBoxLstProjectNo" runat="server" Visible="False">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                </td>
                                <%-- <td style="width: 5px">
                                </td>--%>
                                <td style="text-align: left; width: 60%;">
                                    <asp:Label ID="LblUserProfile" runat="server" Text=""></asp:Label>
                                    <asp:Panel ID="PnlUserLst" runat="server" Width="100%" Visible="false" BorderWidth="1px"
                                        BorderColor="navy" Height="230px" ScrollBars="Auto">
                                        <asp:TreeView ID="TV_UserLst" runat="server" Visible="False" ShowCheckBoxes="Leaf"
                                            ShowLines="True" Width="100%" CssClass="TreeView" ExpandDepth="0" ForeColor="Black">
                                        </asp:TreeView>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="3">
                                    <asp:Button ID="BtnSetRights" runat="server" Text="Set Rights" ToolTip="Set Rights"
                                        CssClass="btn btnsave" Visible="false"/>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

   <div>
        <table>
            <tr>
                <td>
                    <asp:UpdatePanel runat="server" ID="upAuditTrail" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="dvBulkProjectRightsAuditTrail" class="centerModalPopup" runat="server" style="display: none; overflow: auto; left: 3.5% !important; width: 93% !important;">
                                <table>
                                    <tr>
                                        <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 97%;">
                                            <asp:Label ID="lblRamge" runat="server" Text="Audit Trail"></asp:Label>
                                        </td>
                                        <td style="width: 3%">
                                            <img id="imgBulkProjectRightsAuditTrail" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" title="Close" />
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
                                                        <div id="divBulkProjectRightsAuditAuditTrailDetails" style="overflow-y: auto; max-height: 390px;">
                                                            <table id="tblBulkProjectRightsAuditTrail" class="tblAudit" width="100%" style="background-color: aliceblue;"></table>
                                                        </div>
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
    <div id="ModalBackGround" runat="server" class="divModalBackGround" style="display: none;">
    </div>

    <button id="btnAudit" runat="server" style="display: none;" />
    <cc1:ModalPopupExtender ID="mpeBulkProjectRightsAuditTrail" runat="server" PopupControlID="dvBulkProjectRightsAuditTrail" BehaviorID="mpeBulkProjectRightsAuditTrail"
        BackgroundCssClass="modalBackground" CancelControlID="imgBulkProjectRightsAuditTrail" TargetControlID="btnAudit">
    </cc1:ModalPopupExtender>
</asp:Content>
