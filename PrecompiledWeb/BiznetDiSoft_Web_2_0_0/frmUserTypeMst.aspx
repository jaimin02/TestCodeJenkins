<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmUserTypeMst, App_Web_mlepfeoz" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer; 
            * cursor: hand;
        }    
        .auto-style1 {
            /*color: #ADADAD;*/
	        font-weight: bold;
            font-family: Verdana;
            font-size: 11px;
            font-variant: small-caps;
            height: 29px;
        }
        .auto-style2 {
            height: 29px;
        }
    </style>

    <script type="text/javascript" src="Script/scrollablegrid.js"></script>
    <%--<script type="text/javascript" src="script/jquery-1.7.min.js"></script>--%>

    <script src="Script/jquery.timepicker.js" type="text/javascript"></script>

    <table style="width: 100%; margin-bottom: 2%;" cellpadding="5px">
        <tr>
            <td>
                <fieldset class="FieldSetBox" style="display: block; width: 98%; text-align: left; margin:auto; border: #aaaaaa 1px solid;">
                    <legend class="LegendText" style="color: Black; font-size: 12px">
                        <img id="img2" alt="User Type Detail" src="images/panelcollapse.png"
                            onclick="Display(this,'divUserType');" runat="server" style="margin-right: 2px;" />User Type Detail</legend>
                    <div id="divUserType">
                        <table width="98%">
                            <tr>
                                <td class="Label" style="width: 26%; text-align: right">User Type Name* :
                                </td>
                                <td style="width: 25%; text-align: left">
                                    <asp:TextBox ID="txtUserTypeName" runat="server" CssClass="textBox" Width="100%" MaxLength="50" />
                                </td>
                                <td class="Label" style="width: 16%; text-align: right">Work Stage* :
                                </td>
                                <td style="width: 33%; text-align: left">
                                    <asp:DropDownList runat="server" CssClass="dropDownList" ID="DdlWorkFlow" Width="50%">
                                        <asp:ListItem Text="Select Work Stage" Value="-1" Selected="True" />
                                        <asp:ListItem Text="Data Entry" Value="0" />
                                        <asp:ListItem Text="Medical Coding" Value="1" />
                                        <asp:ListItem Text="Data Validator" Value="2" />
                                        <asp:ListItem Text="Delete Data Entry" Value="3" />
                                        <asp:ListItem Text="View Only" Value="5" />
                                        <asp:ListItem Text="First Review" Value="10" />
                                        <asp:ListItem Text="Second Review" Value="20" />
                                        <asp:ListItem Text="Final Review" Value="30" />
                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <tr>
                                <td class="Label" style="width: 26%; text-align: right">Is EDC User?* :
                                </td>
                                <td style="width: 25%; text-align: left">
                                    <asp:RadioButtonList runat="server" ID="rBtnEdcUser" CssClass="RadioButton" RepeatColumns="2"
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Yes" Value="Y" />
                                        <asp:ListItem Text="No" Value="N" />
                                    </asp:RadioButtonList>
                                </td>
                                <td class="Label" style="width: 16%; text-align: right">Remarks*  :
                                </td>
                                <td style="width: 33%; text-align: left">
                                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="textBox" Width="49%" Height="100%" MaxLength="50" TextMode="MultiLine" />
                                </td>
                            </tr>

                            <tr>
                                <td align="center" class="Label" colspan="4">
                                    <asp:Button ID="BtnSave" runat="server" CssClass="btn btnsave" Text="Save" ToolTip="Save"
                                        OnClientClick=" return Validation();" />
                                    <asp:Button ID="btnExportToExcelGrid" runat="Server" Font-Size="Smaller" CssClass="btn btnexcel"  ToolTip="Export To Excel" />
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
    <asp:UpdatePanel ID="Up_View" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table align="center" width="100%">
                <tbody>
                    <tr>
                        <td style="text-align: center;" colspan="2">
                            <fieldset class="FieldSetBox" style="display: block; width: 97.5%; text-align: left; margin:auto; border: #aaaaaa 1px solid;">
                    <legend class="LegendText" style="color: Black; font-size: 12px">
                        <img id="img1" alt="Profiles" src="images/panelcollapse.png"
                            onclick="Display(this,'divProfiles');" runat="server" style="margin-right: 2px;" />Profiles</legend>
                    <div id="divProfiles">
                        <table style="width:80%; margin:auto;">
                            <tr>
                                 <td style="text-align: center;" colspan="2">
                                <asp:GridView ID="GV_UserType" runat="server" style="margin:auto; font-size:10Px !Important; " AutoGenerateColumns="False"
                                    OnRowCommand="GV_UserType_RowCommand" OnRowCreated="GV_UserType_RowCreated" OnRowDataBound="GV_UserType_RowDataBound"
                                    OnPageIndexChanging="GV_UserType_PageIndexChanging">

                                    <Columns>
                                        <asp:BoundField DataFormatString="number" HeaderText="Sr. No">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"  />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="vUserTypeCode" HeaderText="User Type Code">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="vUserTypeName" HeaderText="User Type Name">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="iWorkflowStageId" HeaderText="Work Stage">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="cIsEDCUser" HeaderText="Is EDC User?">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>                       
                                        <asp:TemplateField HeaderText="Edit" SortExpression="status">
                                            <ItemTemplate>
                                                <%--<asp:LinkButton ID="lnkEdit" Text="Edit" runat="server"></asp:LinkButton>--%>
                                                <asp:ImageButton ID="lnkEdit" runat="server" ImageUrl="~/images/Edit2.gif" ToolTip="Edit" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Audit Trail">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="lnkAudit" runat="server" ImageUrl="~/images/audit.png" ToolTip="Audit Trial" OnClientClick="AudtiTrail(this); return false;" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Export">
                                            <ItemTemplate>
                                                <center>
                                                    <img src="images/Export.gif" onclick="ExportToExcel(this.id);" id='<%#  Eval("vUserTypeCode") %>' />                                                        
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
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
                            <asp:UpdatePanel ID="UpUserTypeMstAuditTrail" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <div id="dvUserTypeMstAudiTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
                                        <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                            <tr>
                                                <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 97%;">
                                                    <asp:Label ID="lblRamge" runat="server" Text="Audit Trail Information" ></asp:Label>
                                                </td>
                                                <td style="width: 3%">
                                                    <img id="imgUserTypeMstAuditTrail" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
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
                                                                <table id="tblUserTypeMstAudit" class="tblAudit" width="100%" style="background-color: aliceblue;"></table>
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
            <cc1:ModalPopupExtender ID="MPE_UserTypeMstHistory" runat="server" PopupControlID="dvUserTypeMstAudiTrail" BehaviorID="MPE_UserTypeMstHistory"
                PopupDragHandleControlID="LbldRandomizationDtl" BackgroundCssClass="modalBackground" CancelControlID="imgUserTypeMstAuditTrail"
                TargetControlID="btn3">
            </cc1:ModalPopupExtender>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <div id="DivExports" runat="server">
        <asp:GridView runat="server" ID="gvExport" AutoGenerateColumns="true" Style="display: none"
            HeaderStyle-BackColor="#1560a1" HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" Header-style-font-size="10px !importnat" HeaderStyle-Font-Names="Verdana, Arial, Helvetica, sans-serif"
            HeaderStyle-Font-Size=" 0.9em" 
            HeaderStyle-HorizontalAlign="Center" CellPadding="3" CellSpacing="0"
            RowStyle-Font-Names=" Verdana, Arial, Helvetica, sans-serif" RowStyle-Font-Size="10pt"
            RowStyle-BackColor="#84c8e6" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#191970" RowStyle-ForeColor="#191970">
        </asp:GridView>
    </div>
     <div id="Div1" runat="server">
        <asp:GridView runat="server" ID="gvExportToExcel" AutoGenerateColumns="true" Style="display: none"
            HeaderStyle-BackColor="#1560a1" HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" Header-style-font-size="10px !importnat" HeaderStyle-Font-Names="Verdana, Arial, Helvetica, sans-serif"
            HeaderStyle-Font-Size=" 0.9em" 
            HeaderStyle-HorizontalAlign="Center" CellPadding="3" CellSpacing="0"
            RowStyle-Font-Names=" Verdana, Arial, Helvetica, sans-serif" RowStyle-Font-Size="10pt"
            RowStyle-BackColor="#84c8e6" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#191970" RowStyle-ForeColor="#191970">
        </asp:GridView>
    </div>
    <asp:HiddenField runat="server" ID="hdnUserTypeCode" />
    <asp:Button ID="btnExportToExcel" runat="server" Style="display: none;" />
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    
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

        function AudtiTrail(e) {
            var vUserTypeCode = $("#" + e.id).attr("vUserTypeCode");

            if (vUserTypeCode != "") {
                $.ajax({
                    type: "post",
                    url: "frmUserTypeMst.aspx/AuditTrail",
                    data: '{"vUserTypeCode":"' + vUserTypeCode + '"}',
                    contentType: "application/json; charset=utf-8",
                    datatype: JSON,
                    async: false,
                    success: function (data) {
                        $('#tblUserTypeMstAudit').attr("IsTable", "has");
                        var aaDataSet = [];
                        var range = null;

                        if (data.d != "" && data.d != null) {
                            data = JSON.parse(data.d);
                            for (var Row = 0; Row < data.length; Row++) {
                                var InDataSet = [];
                                InDataSet.push(data[Row].SrNo, data[Row].UserTypeName, data[Row].WorkflowStageId, data[Row].IsEDCUser, data[Row].Remarks, data[Row].ModifyBy, data[Row].ModifyOn);
                                aaDataSet.push(InDataSet);
                            }

                        }
                        if ($("#tblUserTypeMstAudit").children().length > 0) {
                            $("#tblUserTypeMstAudit").dataTable().fnDestroy();
                        }
                        oTable = $('#tblUserTypeMstAudit').prepend($('<thead>').append($('#tblUserTypeMstAudit tr:first'))).dataTable({

                            "bJQueryUI": true,
                            "sPaginationType": "full_numbers",
                            "bLengthChange": true,
                            "iDisplayLength": 10,
                            "bProcessing": true,
                            "bSort": false,
                            "aaData": aaDataSet,
                            "autowidth": false,                         
                            aLengthMenu: [
                                [10, 25, 50, 100, -1],
                                [10, 25, 50, 100, "All"]
                            ],
                            "aoColumns": [
                                {
                                    "sTitle": "Sr. No",
                                },
                                { "sTitle": "User Type Name" },
                                { "sTitle": "Work Stage" },
                                { "sTitle": "Is EDC User?" },
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
                        $find('MPE_UserTypeMstHistory').show();
                        $('.dataTables_filter input').addClass('textBox');
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
        

        function Validation() {
            var Rbtn = document.getElementById('<%=rBtnEdcUser.ClientID%>');
            var Cnt = 0;
            if (document.getElementById('<%=txtUserTypeName.ClientID%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=txtUserTypeName.ClientID%>').value = '';
                msgalert('Please Enter User Type Name !');
                document.getElementById('<%=txtUserTypeName.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=DdlWorkFlow.ClientID%>').selectedIndex.toString().trim() == '0') {
                msgalert('Please Select User Work Stage !');
                document.getElementById('<%=DdlWorkFlow.ClientID%>').focus();
                return false;
            }
            for (var i = 0; i < Rbtn.getElementsByTagName('input').length; i++) {
                if (Rbtn.getElementsByTagName('input')[i].checked) {
                    Cnt += 1;
                }
            }
            if (Cnt == 0) {
                msgalert('Please Select "Is EDC User?"');
                return false;
            }
            if (document.getElementById("<%=btnSave.ClientID%>").value.trim() == "Update") {

                if (document.getElementById("<%=txtRemarks.ClientID%>").value.trim() == "") {
                    msgalert("Please Enter Remarks !");
                    return false;
                }
            }
            return true;
        }
        function UIGV_UserType() {
            $('#<%= GV_UserType.ClientID%>').removeAttr('style', 'display:block');
             oTab = $('#<%= GV_UserType.ClientID%>').prepend($('<thead>').append($('#<%= GV_UserType.ClientID%> tr:first'))).dataTable({
                 "bJQueryUI": true,
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
        function ShowAlert(msg) {
            alertdooperation(msg, 1, "frmUserTypeMst.aspx?mode=1");

            //msgalert(msg);
            //window.location.href = "frmUserTypeMst.aspx?mode=1";
        }
        function ExportToExcel(id) {
            $("#<%= hdnUserTypeCode.ClientID%>").val(id);
            var btn = document.getElementById('<%= btnExportToExcel.ClientId()%>')
            btn.click();
        }

        // for fix gridview header aded on 22-nov-2011
        //        function pageLoad() {
        //            FreezeTableHeader($('#<%= GV_UserType.ClientID %>'), { height: 250, width: 900 });
        //        }
        
    </script>

</asp:Content>
