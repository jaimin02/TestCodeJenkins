<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmMedExGroupMst, App_Web_2mzu20n4" validaterequest="false" enableeventvalidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:content id="Content1" contentplaceholderid="CPHLAMBDA" runat="Server">

        <style type="text/css">
            .paging_full_numbers .ui-button {
                padding: 2px 6px;
                margin: 0;
                cursor: pointer;
                * cursor: hand;
            }

            /*#ctl00_CPHLAMBDA_gvmedexgrp_wrapper {
                margin: 0px 235px;
            }*/
        </style> 


    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="Script/AutoComplete.js" language="javascript"></script>
    <script type="text/javascript" src="Script/scrollablegrid.js"></script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table style="width: 100%;" cellpadding="5px">
                <tbody>
                     <tr>
     <td>
         <fieldset class="FieldSetBox" style="display: block; width: 96%; margin:auto; text-align: left; border: #aaaaaa 1px solid;">
             <legend class="LegendText" style="color: Black; font-size: 12px">
                 <img id="img2" alt="Attribute Group Details" src="images/panelcollapse.png"
                 onclick="Display(this,'divAttributeGroupDetail');" runat="server" style="margin-right: 2px;" />Attribute Group Details</legend>
                     <div id="divAttributeGroupDetail">
                         <table width="98%">
                    <tr>
                        <td class="Label" style="text-align: right; width: 26%;">
                            Project Type* :
                        </td>
                        <td style="text-align: left; width: 25%;">
                            <asp:DropDownList ID="ddlProjectType" runat="server" TabIndex="1" CssClass="dropDownList"
                                Width="100%" />
                        </td>
                        <td class="Label" style="text-align: right; width: 16%;">
                            Attribute Group* :
                        </td>
                        <td style="text-align: left; width: 33%;">
                            <asp:TextBox ID="txtmedexdesc" TabIndex="2" runat="server" CssClass="textBox" Width="44%"
                                MaxLength="50" />
                        </td>
                    </tr>
                    <tr>
                        <td class="Label" style="text-align: right; width: 26%;">
                            Variable Name :
                        </td>
                        <td style="text-align: left; width: 25%;">
                            <asp:TextBox Style="width: 99%" ID="txtCDISCValue" TabIndex="3" runat="server" CssClass="textBox"
                                MaxLength="10" />
                        </td>
                        <td class="Label" style="text-align: right; width: 16%;">
                            Other Value :
                        </td>
                        <td style="text-align: left; width: 33%;">
                            <asp:TextBox Style="width: 44%" ID="txtOtherValue" TabIndex="3" runat="server" CssClass="textBox"
                                MaxLength="100" />
                        </td>
                    </tr>
                    <tr>
                        <td style =" text-align :right ;" class="Label">
                            Remark* :
                        </td>
                        <td  nowrap="nowrap" style =" text-align :left ;">
                            <asp:TextBox ID="txtRemark" runat="server" Rows="3" Columns="20" CssClass="textBox" style="width:60%; height:auto;" TextMode="MultiLine"  MaxLength ="500" />
                        </td>                    
                        <td style="display: none; text-align: right;" class="Label">
                            ActiveFlag
                        </td>
                        <td style="display: none; width: 15%; text-align: left;">
                            <asp:CheckBox ID="chkactive" TabIndex="4" runat="server" CssClass="checkboxlist "
                                Checked="True" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center" class="Label" colspan="4">
                            <asp:Button ID="BtnSave" TabIndex="5" OnClick="BtnSave_Click" runat="server" Text="Save"
                                ToolTip="Save" CssClass="btn btnsave" OnClientClick="return Validation();" />
                            <asp:Button ID="btnExportToExcelGrid" runat="Server" Font-Size="Smaller" CssClass="btn btnexcel"  ToolTip="Export To Excel" />
                            <asp:Button ID="BtnExit" TabIndex="6" OnClick="BtnExit_Click" runat="server" Text="Exit"
                                ToolTip=" Exit" CssClass="btn btnexit" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);
" />
                            <asp:Button ID="btncancel" OnClick="btncancel_Click" runat="server" Text="Cancel"
                                ToolTip="Cancel" CssClass="btn btncancel" TabIndex="8" Visible="False" />
                        </td>
                    </tr>
                             </table>
                         </div>
             </fieldset>
         </td>
                         </tr>
                </tbody>
            </table>
            <table style="width: 100%; margin-bottom: 1%; display:none;">
                <tr>
                    <td style="text-align: right; width: 30%;">
                        <asp:Label runat="server" ID="lblSearch" Text="Search Attribute Group : " />
                    </td>
                    <td style="text-align: left;">
                    
                        <asp:TextBox ID="txtMedexGroup" runat="server" CssClass="textBox" Width="55%" />
                        <asp:Button ID="btnSetMedexGroup" runat="server" Style="display: none" Text="" />
                        <asp:HiddenField ID="HMedexGroupId" runat="server" />
                        <cc1:AutoCompleteExtender ID="AutoCompleteExtenderMedexGroup" runat="server" UseContextKey="True"
                            TargetControlID="txtMedexGroup" ServicePath="AutoComplete.asmx" ServiceMethod="GetAttributeGroup"
                            OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1"
                            CompletionListElementID="pnlAttributeGrpList" CompletionListItemCssClass="autocomplete_listitem"
                            CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListCssClass="autocomplete_list"
                            BehaviorID="AutoCompleteExtenderMedexGroup">
                        </cc1:AutoCompleteExtender>
                        <asp:Button runat="server" ID="BtnViewAll" CssClass="btn btnnew" Text="View All" ToolTip="View All"/>
                        <asp:Panel ID="pnlAttributeGrpList" runat="server" Style="max-height: 200px; overflow: auto; overflow-x:hidden" />
                        
                    </td>
                </tr>
            </table>
             <table style="width: 100%;" cellpadding="5px">
                <tbody>
                      <tr>
     <td>
                    <fieldset class="FieldSetBox" style="display: block; width: 96%; margin:auto; text-align: left; border: #aaaaaa 1px solid;">
             <legend class="LegendText" style="color: Black; font-size: 12px">
                 <img id="img1" alt="Attribute Group Data" src="images/panelcollapse.png"
                 onclick="Display(this,'divAttributeGroupData');" runat="server" style="margin-right: 2px;" />Attribute Group Data</legend>
                     <div id="divAttributeGroupData">
                          <table width="98%">
                             <tr>
                                 <td>
                    <asp:GridView ID="gvmedexgrp" runat="server" style="display:none; width:auto; margin:auto;"
                        AutoGenerateColumns="False" OnRowDataBound="gvmedexgrp_RowDataBound"
                        OnRowCommand="gvmedexgrp_RowCommand" OnPageIndexChanging="gvmedexgrp_PageIndexChanging"
                        OnRowEditing="gvmedexgrp_RowEditing" OnRowDeleting="gvmedexgrp_RowDeleting">
                        <Columns>
                            <asp:BoundField DataFormatString="number" HeaderText="Sr. No">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="vMedExGroupCode" HeaderText="Attribute Group Code" />
                            <asp:BoundField DataField="vMedExGroupDesc" HeaderText="Attribute Group Desc">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="vCDISCValue" HeaderText="Variable Name">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="vOtherValues" HeaderText="Other Value">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="vProjectTypeName" HeaderText="Project Type" />
                            <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImbEdit" runat="server" TabIndex="9" ToolTip=" Edit" ImageUrl="~/images/Edit2.gif" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Delete">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImbDelete" runat="server" TabIndex="9" ToolTip="Delete" ImageUrl="~/Images/i_delete.gif"
                                        OnClientClick="return msgconfirmalert('Are you sure you want to Delete?',this);
" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" />
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
                                                    <img src="images/Export.gif" onclick="ExportToExcel(this.id);" id='<%# Eval("vMedExGroupCode")%>' />                                                        
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                        </td>
                             </tr>
                             </table>
                        </fieldset>
           </td>
         </tr>
                    </tbody>
                </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnExportToExcelGrid" />
        </Triggers>
    </asp:UpdatePanel>

    <div>
                <table>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpMedExGroupMstAuditTrail" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <div id="dvMedExGroupMstAudiTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
                                        <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                            <tr>
                                                <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 97%;">
                                                    <asp:Label ID="lblRamge" runat="server" Text="Audit Trail Information" ></asp:Label>
                                                </td>
                                                <td style="width: 3%">
                                                    <img id="imgMedExGroupMstAuditTrail" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
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
                                                                <table id="tblMedExGroupMstAudit" class="tblAudit" width="100%" style="background-color: aliceblue;"></table>
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

              <cc1:ModalPopupExtender ID="MPE_MedExGroupMstHistory" runat="server" PopupControlID="dvMedExGroupMstAudiTrail" BehaviorID="MPE_MedExGroupMstHistory"
                PopupDragHandleControlID="LbldRandomizationDtl" BackgroundCssClass="modalBackground" CancelControlID="imgMedExGroupMstAuditTrail"
                TargetControlID="btn3">
            </cc1:ModalPopupExtender>

     <div id="DivExports" runat="server">
        <asp:GridView runat="server" ID="gvExport" AutoGenerateColumns="true" Style="display: none"
            HeaderStyle-BackColor="#1560a1" HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" Header-style-font-size="10px !importnat" HeaderStyle-Font-Names="Verdana, Arial, Helvetica, sans-serif"
            HeaderStyle-Font-Size=" 0.9em" 
            HeaderStyle-HorizontalAlign="Center" CellPadding="3" CellSpacing="0"
            RowStyle-Font-Names=" Verdana, Arial, Helvetica, sans-serif" RowStyle-Font-Size="10pt"
            RowStyle-BackColor="#84c8e6" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#191970" RowStyle-ForeColor="#191970">
        </asp:GridView>
    </div>
    <div id="DivExportToExcel" runat="server">
        <asp:GridView runat="server" ID="gvExportToExcel" AutoGenerateColumns="true" Style="display: none"
            HeaderStyle-BackColor="#1560a1" HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" Header-style-font-size="10px !importnat" HeaderStyle-Font-Names="Verdana, Arial, Helvetica, sans-serif"
            HeaderStyle-Font-Size=" 0.9em"
            HeaderStyle-HorizontalAlign="Center" CellPadding="3" CellSpacing="0"
            RowStyle-Font-Names=" Verdana, Arial, Helvetica, sans-serif" RowStyle-Font-Size="10pt"
            RowStyle-BackColor="#84c8e6" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#191970" RowStyle-ForeColor="#191970">
        </asp:GridView>
     </div> 
    <asp:HiddenField runat="server" ID="hdnMedExGroupCode" />
    <asp:Button ID="btnExportToExcel" runat="server" Style="display: none;"/>



 <script type="text/javascript">

     function HideAttributeGroupDetails() {
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
         var vMedExGroupCode = $("#" + e.id).attr("vMedExGroupCode");

         if (vMedExGroupCode != "") {
             $.ajax({
                 type: "post",
                 url: "frmMedExGroupMst.aspx/AuditTrail",
                 data: '{"vMedExGroupCode":"' + vMedExGroupCode + '"}',
                 contentType: "application/json; charset=utf-8",
                 datatype: JSON,
                 async: false,
                 success: function (data) {
                     $('#tblMedExGroupMstAudit').attr("IsTable", "has");
                     var aaDataSet = [];
                     var range = null;

                     if (data.d != "" && data.d != null) {
                         data = JSON.parse(data.d);
                         for (var Row = 0; Row < data.length; Row++) {
                             var InDataSet = [];
                             InDataSet.push(data[Row].SrNo, data[Row].MedExGroupDesc, data[Row].CDISCValue, data[Row].OtherValues, data[Row].ProjectTypeName, data[Row].Remark, data[Row].ModifyBy, data[Row].ModifyOn);
                             aaDataSet.push(InDataSet);
                         }
                     }
                     if ($("#tblMedExGroupMstAudit").children().length > 0) {
                         $("#tblMedExGroupMstAudit").dataTable().fnDestroy();
                     }
                     oTable = $('#tblMedExGroupMstAudit').prepend($('<thead>').append($('#tblMedExGroupMstAudit tr:first'))).dataTable({

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
                             { "sTitle": "Attribute Group Desc" },
                                { "sTitle": "Variable Name" },
                                { "sTitle": "Other Value" },
                                { "sTitle": "Project Type Name" },
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
                     $find('MPE_MedExGroupMstHistory').show();
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

     function UIgvmedexgrp() {
         $('#<%= gvmedexgrp.ClientID%>').removeAttr('style', 'display:block');
         oTab = $('#<%= gvmedexgrp.ClientID%>').prepend($('<thead>').append($('#<%= gvmedexgrp.ClientID%> tr:first'))).dataTable({
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

     function Validation() {
         if (document.getElementById('<%=ddlProjectType.clientid %>').selectedIndex == 0) {
                msgalert('Please Select Project Type !');
                return false;
            }
            else if (document.getElementById('<%=txtmedexdesc.ClientID%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=txtmedexdesc.ClientID%>').value = '';
                document.getElementById('<%=txtmedexdesc.ClientID%>').focus();
                msgalert('Please Enter Attribute Group !');
                return false;
            }
        if (document.getElementById("<%=btnSave.ClientID%>").value.trim() == "Update") {

                if (document.getElementById("<%=txtRemark.ClientID%>").value.trim() == "") {
                msgalert("Please Enter Remarks !");
                return false;
            }
        }

        return true;
    }
    function ExportToExcel(id) {
        $("#<%= hdnMedExGroupCode.ClientID()%>").val(id);
         var btn = document.getElementById('<%= btnExportToExcel.ClientId()%>')
         btn.click();
     }

     function ClientPopulated(sender, e) {
         SubjectClientShowing('AutoCompleteExtenderMedexGroup', $get('<%= txtMedexGroup.ClientId %>'));
        }

        function OnSelected(sender, e) {
            SubjectOnItemSelected(e.get_value(), $get('<%= txtMedexGroup.clientid %>'),
         $get('<%= HMedexGroupId.clientid %>'), document.getElementById('<%= btnSetMedexGroup.ClientId %>'));
        }
        // for fix gridview header aded on 22-nov-2011
        //         function pageLoad()
        //        {
        //            FreezeTableHeader($('#<%= gvmedexgrp.ClientID %>'), { height: 250, width: 900 });
     //        }

    </script>
</asp:content>
