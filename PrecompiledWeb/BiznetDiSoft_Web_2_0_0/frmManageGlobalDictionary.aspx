<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmManageGlobalDictionary, App_Web_mlepfeoz" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <style type="text/css">
        /*Added by ketan for (Resolve issue oveRlap button in datatable)*/
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

        #tblClientMstAudit tbody {
            text-align: left;
        }
        /*Ended by ketan*/
    </style>
    <%--<script type="text/javascript" src="Script/scrollablegrid.js"></script>--%>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td colspan="2">
                        <div style="min-width: 100%; width: auto !important; width: 100%;" align="center"
                            id="divAddNewDictionary" class="collapsePanel" runat="server">
                            <table>
                                <tr>
                                    <td style="text-align: right;" class="Label">Dictionary Name* :
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox runat="server" ID="txtDictionaryName" class="textBox" MaxLength="50"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;" class="Label">MedDRA version* :
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox runat="server" ID="txtmedDraVersion" class="textBox" MaxLength="100"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;" class="Label">Please Select MedDRA_1_mdhierarchy ASCII File*  :
                                    </td>
                                    <td style="text-align: left;">
                                        <%--<asp:FileUpload runat="server" ID="FUploadXLS"></asp:FileUpload>--%>

                                        <input id="AdFlUpload" runat="server" class="textBox" name="FlUpload" style="width: 250px;"
                                            type="file" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;" class="Label">Please Select MedDRA_1_low_level_term ASCII File* :
                                    </td>
                                    <td style="text-align: left;">
                                        <%--<asp:FileUpload runat="server" ID="FUploadXLS"></asp:FileUpload>--%>

                                        <input id="AdFlUpload_LLT" runat="server" class="textBox" name="FlUpload" style="width: 250px;"
                                            type="file" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center;" colspan="2">
                                        <asp:Button runat="server" ID="btnUpload" class="btn btnnew" Text="Upload" ToolTip="Upload"
                                            OnClientClick="return validate();" />
                                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" Text="Cancel" ToolTip="Cancel" />
                                        <asp:Button ID="btnExit" OnClick="btnExit_Click" runat="server" CssClass="btn btnexit"
                                           Text="Exit" ToolTip="Exit" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this); "
                                            TabIndex="5" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>

                <tr>
                    <td colspan="2">
                        <div style="margin: 0 20%;" align="center"
                            id="divManageGlobalDictionary" class="collapsePanel" runat="server">

                            <asp:UpdatePanel ID="upMngGlobalDictionary" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>

                                    <table>
                                        <tr>
                                            <td>
                                                <fieldset class="FieldSetBox" style="width: 100%;   margin-left: 83%">
                                                    <legend class="LegendText" style="color: Black">Legend</legend>
                                                    <table class="LabelText">
                                                        <tbody>
                                                            <tr>
                                                                <td>
                                                                    <div style="width: 20px; height: 20px; float: left; background-color: rgb(255, 189, 121); font-weight: bold;">
                                                                    </div>
                                                                </td>
                                                                <td>InActive Global Dictionary
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>

                                        <tr>
                                            <asp:GridView ID="gvwMngGlDictionary" runat="server" AutoGenerateColumns="False" Style="display:none">
                                                <Columns>
                                                    <asp:BoundField HeaderText="#">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vRefTableName" HeaderText="Global Dictionary" />
                                                    <asp:TemplateField HeaderText="Browse">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkBrowse" runat="server" CommandName="BROWSE" CommandArgument='<%# Bind("nRefMasterNo") %>'
                                                                Enabled="true" ToolTip="Browse">
                                                    <img src="Images/browse.png" alt ="Browse" style ="margin:none;"/>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Current Status">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblCurrentStatus"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Change Status To">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="LnkGrdstatus" runat="server" Enabled="true">Active</asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="nRefMasterNo" />
                                                    <asp:TemplateField HeaderText="Delete">
                                                        <ItemTemplate>
                                                            <%-- <asp:LinkButton ID="lnkDelete"   runat="server" Enabled="true">Delete</asp:LinkButton>--%>
                                                            <asp:ImageButton ID="lnkDelete" runat="server" ImageUrl="~/Images/i_delete.gif"
                                                                ToolTip="Delete" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Audit Trail">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="lnkAudit" runat="server" ImageUrl="~/Images/audit.png" ToolTip="Audit Trial" OnClientClick="AudtiTrail(this); return false;" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateField>

                                                </Columns>
                                            </asp:GridView>
                                        </tr>
                                    </table>

                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnRemarksUpdate" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <%--<asp:AsyncPostBackTrigger ControlID="btnRemarksUpdate" EventName="Click" />--%>
            <%--<asp:PostBackTrigger ControlID="btnRemarksUpdate"></asp:PostBackTrigger>--%>
            <asp:PostBackTrigger ControlID="btnUpload"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="btnCancel"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="btnExit"></asp:PostBackTrigger>
            <%--<asp:PostBackTrigger ControlID="btnSetProject" />--%>
        </Triggers>

    </asp:UpdatePanel>

    <%--Added by ketan for Remarks--%>
    <asp:Button ID="btnRemarks" runat="server" Style="display: none;" TabIndex="55" CssClass="btn btnnew" />
    <asp:Button ID="Button1" runat="server" Text="Cancel" CssClass="btn btnnew"  Style="font-size: 12px !important; display: none;" TabIndex="56" />
    <cc1:ModalPopupExtender ID="mdlRemarks" runat="server" PopupControlID="divRemarks"
        BackgroundCssClass="modalBackground" BehaviorID="mdlRemarks" CancelControlID="btnRemarksCancel"
        TargetControlID="btnRemarks">
    </cc1:ModalPopupExtender>
    <div id="divRemarks" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 32%; height: auto; max-height: 45%; min-height: auto;">
        <table cellpadding="2" cellspacing="2" width="100%">
            <tr>
                <td class="LabelText" style="text-align: left !important; font-size: 12px !important; width: 97%;">Remarks
                </td>
            </tr>
            <tr>
                <td>
                    <hr />
                </td>
            </tr>
            <tr>
                <td class="LabelText" style="text-align: left !important;">Enter Remarks:
                </td>
            </tr>
            <tr>
                <td style="text-align: left !important;">
                    <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="5" Height="60px"
                        Width="300px" TabIndex="55" />
                </td>
            </tr>
            <tr>
                <td>
                    <hr />
                </td>
            </tr>
            <tr>
                <td style="text-align: center;">
                    <%--<asp:Button ID="btnRemarksUpdate" runat="server" Text="Update" CssClass="ButtonText"
                        Width="64px" Style="font-size: 12px !important;" TabIndex="56" OnClientClick="return UpdateData();" />--%>
                    <asp:Button ID="btnRemarksUpdate" runat="server" Text="Save" CssClass="btn btnsave"
                        Style="font-size: 12px !important;" TabIndex="56" OnClientClick="return UpdateData();" />
                    <asp:Button ID="btnRemarksCancel" runat="server" Text="Cancel" CssClass="btn btncancel"
                        Style="font-size: 12px !important;" TabIndex="56" />

                </td>
            </tr>
        </table>
    </div>

    <button id="btn3" runat="server" style="display: none;" />
    <cc1:ModalPopupExtender ID="MPE_CleintMstHistory" runat="server" PopupControlID="dvClientMstAudiTrail" BehaviorID="MPE_CleintMstHistory"
        PopupDragHandleControlID="LbldRandomizationDtl" BackgroundCssClass="modalBackground" CancelControlID="imgClientAuditTrail"
        TargetControlID="btn3">
    </cc1:ModalPopupExtender>

    <div id="dvClientMstAudiTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
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
                                <table id="tblClientMstAudit" class="tblAudit" width="100%" style="background-color: aliceblue;"></table>
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


    <%--Ended By ketan--%>
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="Script/jquery.min.js"></script>


    <script type="text/javascript">
        jQuery.noConflict();
        function $(id) {
            return document.getElementById(id);
        }
        function validate() {
            var medDraversionFlag;
            var e = document.getElementById('<%=divAddNewDictionary.clientid%>');
            e.style.display = 'block';

            if (document.getElementById('<%=txtDictionaryName.clientid %>').value.trim().length <= 0) {
                msgalert('Please Enter Dictionary Name !');
                return false;
            }
            else {
                var str = document.getElementById('<%=txtDictionaryName.clientid %>').value.trim();
                if (/^[a-zA-Z0-9_ ]*$/.test(str) == false) {
                    msgalert('Your dictionary name is invalid character. Please enter another dictionary name !');
                    return false;
                }

            }
            var medDraversion = document.getElementById('<%=txtmedDraVersion.clientid %>').value;
            if (medDraversion == '') {
                msgalert('Please Enter MedDRA Version !')
                return false;
            }
            if (document.getElementById('<%=AdFlUpload.clientid %>').value.trim().length <= 0) {
                msgalert('Please Select MedDRA_1_mdhierarchy ASCII File !');
                return false;
            }
            if (document.getElementById('<%=AdFlUpload_LLT.clientid %>').value.trim().length <= 0) {
                msgalert('Please Select MedDRA_1_low_level_term ASCII File !');
                return false;
            }

            if (medDraversion != '') {
                jQuery.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "frmManageGlobalDictionary.aspx/VersionExist",
                    data: "{'Version':'" + medDraversion + "'}",
                    dataType: "json",
                    success: function (data) {
                        var obj = data.d;
                        document.getElementById('<%= btnUpload.ClientID%>').disabled = false;
                        if (obj != '') {
                            msgalert('MedDRa Version ' + medDraversion + ' Allready Exist. Please enter another MedDRa version !');
                        }
                        else {
                            <%=Page.ClientScript.GetPostBackEventReference(btnUpload, "").ToString()%>
                        }
                    }
                });
                document.getElementById('<%= btnUpload.ClientID%>').disabled = true;
            }

        }

        function OpenWindow(Path) {
            window.open(Path);
            return false;
        }

        var mode = "";
        var nRefMasterNo = "";
        var RefTableName = "";
        function show_confirm(e) {
            $find('mdlRemarks').show();
            document.getElementById('<%=txtRemarks.ClientID%>').value = "";
            //var r = confirm("Are You Sure You Want To Delete This Record?");
            mode = e.attributes.mode.value;
            nRefMasterNo = e.attributes.nRefMasterNo.value;
            if (mode == "DELETE") {
                RefTableName = e.attributes.RefTableName.value;
            }
            else {
                RefTableName = "";
            }
            return true;
        }
        function UpdateData() {
            var txtRemarks = document.getElementById('<%=txtRemarks.ClientID%>').value;
            if (txtRemarks.trim() == '') {
                msgalert('Please Enter Remarks !')
                return false;
            }
            return true;
        }

        function bindgvwMngGlDictionary() {
            $('#<%= gvwMngGlDictionary.ClientID%>').removeAttr("style", "display:none");
            oTable = $('#<%= gvwMngGlDictionary.ClientID%>').prepend($('<thead>').append($('#<%= gvwMngGlDictionary.ClientID%> tr:first'))).dataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": false,
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": false,
                "autoWidth": true,
                "bInfo": true,
                "oLanguage": {
                    "sEmptyTable": "No Record Found",
                },
                "aoColumnDefs": [{ "aTargets": [0, 2, 3, 4, 5], "bSearchable": false },
                    { "bVisible": false, "aTargets": [6] }
                ],

            });

        }

        function AudtiTrail(e) {
            var nRefMasterNo = e.attributes.nRefMasterNo.value;

            if (nRefMasterNo != "") {
                jQuery.ajax({
                    type: "post",
                    url: "frmManageGlobalDictionary.aspx/AuditTrail",
                    data: '{"nRefMasterNo":"' + nRefMasterNo + '"}',
                    contentType: "application/json; charset=utf-8",
                    datatype: JSON,
                    async: false,
                    success: function (data) {
                        $('#tblClientMstAudit').attr("IsTable", "has");
                        var aaDataSet = [];
                        var range = null;

                        if (data.d != "" && data.d != null) {
                            data = JSON.parse(data.d);
                            for (var Row = 0; Row < data.length; Row++) {
                                var InDataSet = [];
                                InDataSet.push(data[Row].SrNo, data[Row].vRefTableName, data[Row].cActiveFlag, data[Row].vRemark, data[Row].vModifyBy, data[Row].dModifyOn);
                                aaDataSet.push(InDataSet);
                            }

                        }
                        if ($("#tblClientMstAudit").children().length > 0) {
                            $("#tblClientMstAudit").dataTable().fnDestroy();
                        }

                        oTable = $('#tblClientMstAudit').prepend($('<thead>').append($('#tblClientMstAudit tr:first'))).dataTable({

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
                                { "sTitle": "Global Dictionary" },
                                 { "sTitle": "Status" },
                                { "sTitle": "Remarks" },
                                { "sTitle": "Modify By" },
                                { "sTitle": "Modify On" },

                            ],
                            "aoColumnDefs": [
                                        { 'bSortable': false, 'aTargets': [0], }
                            ],

                            "oLanguage": {
                                "sEmptyTable": "No Record Found",
                            }

                        });
                        oTable.fnAdjustColumnSizing();
                        $('.DataTables_sort_wrapper').click;
                        $find('MPE_CleintMstHistory').show();

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


    </script>


</asp:Content>
