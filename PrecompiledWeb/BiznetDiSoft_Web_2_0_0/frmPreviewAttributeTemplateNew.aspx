<%@ page language="VB" masterpagefile="~/ECTDMasterPageForSDTM.master" autoeventwireup="false" inherits="frmPreviewAttributeTemplateNew, App_Web_xjkmyygy" enableeventvalidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <link rel="stylesheet" type="text/css" href="App_Themes/smoothnessjquery-ui.css" />
    <link href="App_Themes/bootstrap.min.css" type="text/css"  rel="stylesheet" />
    <link rel="shortcut icon" type="image/x-icon" href="images/biznet.ico" />

    <script type="text/javascript" src="Script/slimScroll.min.js"></script>
    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <script type="text/javascript" src="Script/Validation.js"></script>
    <%--<script type="text/javascript"  src="Script/json_parse.js"></script>--%>
    <script type="text/javascript" src="Script/bootstrap3.3.5.min.js"></script>

    <style type="text/css">
        .dataTables_wrapper {
            margin-top: 20px !important;
        }
        .plus{
        background:url("images/plus1.png") no-repeat 100% 6px;
		background-repeat:no-repeat;
        width:90%; 
        color:white; 
        font-weight: bold; 
        text-align:left; 
        background-color:#4974c3;
        margin-top:10px;
        text-align-last:left !important;
        }

            .modal-dialog{
                    overflow-y: initial !important;
                }

        #tblMedExMst {
            overflow:auto;
            display:block;
            width:100% !important;
            /*height:500px !important ;*/
        }
        select {
            color:#000 !important;
        }
        #SeqMedex {
            width: 760px;
            margin-bottom: 20px;
            overflow: hidden;
        }

        .dataTables_filter input {
            color : black !important ;
            font-style:normal !important;
}
        /*.dataTables_length select {
   background-color: red !important;
}*/

        .allmed {
            line-height: 1.5em;
            border-bottom: 1px solid #ccc;
            float: left;
            display: inline;
            border: 1px solid #d3d3d3;
            background: -moz-linear-gradient(top, rgba(247,247,247,0.73) 0%, rgba(206,227,237,1) 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,rgba(247,247,247,0.73)), color-stop(100%,rgba(206,227,237,1))); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top, rgba(247,247,247,0.73) 0%,rgba(206,227,237,1) 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top, rgba(247,247,247,0.73) 0%,rgba(206,227,237,1) 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top, rgba(247,247,247,0.73) 0%,rgba(206,227,237,1) 100%); /* IE10+ */
            background: linear-gradient(to bottom, rgba(247,247,247,0.73) 0%,rgba(206,227,237,1) 100%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr= '#baf7f7f7', endColorstr= '#cee3ed',GradientType=0 ); /* IE6-9 */
            font-weight: normal;
            color: #555555;
        }

            .allmed:hover {
                background: rgb(30,87,153); /* Old browsers */
                background: -moz-linear-gradient(top, rgba(30,87,153,1) 0%, rgba(41,137,216,1) 50%, rgba(32,124,202,1) 100%, rgba(125,185,232,1) 100%); /* FF3.6+ */
                background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,rgba(30,87,153,1)), color-stop(50%,rgba(41,137,216,1)), color-stop(100%,rgba(32,124,202,1)), color-stop(100%,rgba(125,185,232,1))); /* Chrome,Safari4+ */
                background: -webkit-linear-gradient(top, rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 100%,rgba(125,185,232,1) 100%); /* Chrome10+,Safari5.1+ */
                background: -o-linear-gradient(top, rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 100%,rgba(125,185,232,1) 100%); /* Opera 11.10+ */
                background: -ms-linear-gradient(top, rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 100%,rgba(125,185,232,1) 100%); /* IE10+ */
                background: linear-gradient(to bottom, rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 100%,rgba(125,185,232,1) 100%); /* W3C */
                filter: progid:DXImageTransform.Microsoft.gradient( startColorstr= '#1e5799', endColorstr= '#7db9e8',GradientType=0 ); /* IE6-9 */
                color: White !important;
            }

        #tr_AttributeGroup1 {
            margin-left: 18%;
            font-weight: bold;
        }

        #tr_Attribute1 {
            margin-left: 15%;
            font-weight: bold;
        }

        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }
        #ctl00_CPHLAMBDA_GV_PreviewAtrributeTemplate_wrapper {
            margin: 0px 90px;
        }

        .modal-backdrop.fade {
            display:none;
        }
        .modal-backdrop.in {
            display:none;
        }
        /*.modal-backdrop.in{
            opacity: 1;
            -webkit-transition: opacity 1000ms linear;
    transition: opacity 1000ms linear; 

        }*/
    </style>
    <asp:UpdatePanel ID="UpPnlTemplate" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="HTemplateId" runat="server" />
            <asp:HiddenField ID="Hdn_ValidationType" runat="server" />
            <input type="hidden" id="hd_TxtLength" value="0" />
            <asp:HiddenField ID="Hdn_LengthNumeric" runat="server" />
            <asp:HiddenField ID="hdnMedexList" runat="server" />
            <asp:HiddenField ID="hdnUserId" runat="server" />
            <asp:HiddenField ID="hdnUserScope" runat="server" />
            <asp:TextBox ID="txtUserId" runat="server"  style="display:none"/>


            <table style="width: 100%; margin-bottom: 2%;" cellpadding="5px">
                <tr>
                    <td>
                        <fieldset id="Fieldset1" class="FieldSetBox" style="display: block; width: 98%; text-align: left; margin:auto; border: #aaaaaa 1px solid;" runat="server">
                            <legend class="LegendText" style="color: Black; font-size: 12px" title="test" >
                                <img id="img2" alt="Search Template" src="images/panelcollapse.png" onclick="Display(this,'divUserType');" runat="server" style="margin-right: 2px;" />Search Template</legend>
                            <div id="divUserType">
                                <table width="90%" style="margin: auto; padding: 1%;">
                                    <tr>
                                        <td style="width: 30%; text-align: right;">Search Template :
                                        </td>

                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtTemplate" TabIndex="1" runat="server" CssClass="textBox" Width="70%" />
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                                CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                                OnClientShowing="ClientPopulated" ServiceMethod="GetTemplateCompletionListPreviewAttribute" ServicePath="AutoComplete.asmx"
                                                TargetControlID="txtTemplate" UseContextKey="True" CompletionListElementID="pnlTemplate">
                                            </cc1:AutoCompleteExtender>
                                            <asp:Panel ID="pnlTemplate" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden;" />
                                            <asp:Button ID="BtnViewAll" runat="server" Text="View All" ToolTip="View All" Visible="false"
                                                CssClass="btn btnnew"  />
                                            <asp:Button Style="display: none" ID="btnSetTemplate" OnClick="btnSetTemplate_click"
                                                runat="server" Text="Template" />
                                        </td>
                                    </tr>
                                    <tr style="margin-top:10px !important;" >
                                        <td colspan="2">
                            <asp:GridView ID="GV_PreviewAtrributeTemplate" runat="server" OnPageIndexChanging="GV_PreviewAtrributeTemplate_PageIndexChanging"
                                AutoGenerateColumns="False"  Style="width:100%; margin: auto; margin-top: 2%;" TabIndex="2">
                                <HeaderStyle Font-Underline="False" BackColor="#1560a1" Font-Names="Verdana" VerticalAlign="Top"
                                    HorizontalAlign="Center" ForeColor="white" Font-Bold="True" />
                                <Columns>
                                    <asp:BoundField HeaderText="#" ItemStyle-HorizontalAlign="Right">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vTemplateName" HeaderText="Attribute Template" />
                                    <asp:BoundField DataField="vMedExTemplateId" HeaderText="Template ID" />
                                    <asp:TemplateField HeaderText="Edit" SortExpression="status">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImbEdit" runat="server" ToolTip="Edit" ImageUrl="~/images/Edit2.gif" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Preview" SortExpression="status">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImbMPreview" runat="server" ToolTip="Preview" ImageUrl="~/Images/view.gif" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="vProjectTypeCode" HeaderText="ProjectTypeCode" />
                                    <asp:TemplateField HeaderText="Order">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImbReorder" ToolTip="Reorder The Attributes" runat="server"
                                                ImageUrl="~/images/sorting.png" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
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
            </table>

            <asp:Button ID="btnSequence" runat="server" Style="display: none;" />
            <cc1:ModalPopupExtender ID="MPEMedexSequence" runat="server" PopupControlID="divSequence"
                BackgroundCssClass="modalBackground" BehaviorID="MPEMedexSequence" TargetControlID="btnSequence">
            </cc1:ModalPopupExtender>
            <div id="divSequence" runat="server" class="centerModalPopup" style="width: 80%; position: absolute; max-height: 500px; display: none;">
                <table width="100%">
                    <tr>
                        <td>
                            <img id="ImgSeqCancel" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px;"
                                title="Close" onclick="return closesequencediv();" />
                            <asp:Label ID="lblhdr" runat="server" Text="Order Attributes" Style="font-weight: bold; color: Black; font-size: 14px; margin-left: 3%; float: left;" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <hr />
                        </td>
                    </tr>
                </table>
                <div id="divtips" style="width: 100%; margin: auto;">
                    <asp:Label ID="lblTips" runat="server" Text="Follow the attribute sequence in 
                    "
                        Style="color: Black; font-weight: normal; float: left; margin-left: 3%;" />
                    <img src="images/rightarrow.png" alt="Right Arrow" style="margin-right: 73%;" />
                    <br />
                    <asp:Label ID="lbltips2" runat="server" Text="To rearrange drag & drop the attribute to the required position"
                        Style="color: Black; font-weight: normal; float: left; margin-left: 3%;"></asp:Label>
                </div>
                <div id="divMedexSequence" style="width: 100%; margin: auto; margin-bottom: 2%;height:300px; overflow:auto !important;">
                    <ul id="SeqMedex" runat="server" style="list-style-type: none !important; padding: 10px; width: 100%;">
                    </ul>
                </div>
                <table width="100%">
                    <tr>
                        <td>
                            <hr />
                        </td>
                    </tr>
                </table>
                <asp:Label ID="lblMedexCount" runat="server" Style="float: left; color: Black; margin-left: 3%;"></asp:Label>
                <input type="button" id="btnMedexseq" value="Save" title="Save The Sequence" style="float: left; margin-left: 25%;"
                    onclick="return savesequence();" class="btn btnsave" />                

                <input type="button" id="btnCloseModal" value="Cancel" title="Cancel" style="float: left;margin-left:1%"
                    onclick="return closesequencediv();" class="btn btncancel"/>

                <asp:Button ID="btnSaveSequence" Text="Save" ToolTip="Save The Sequence" runat="server"
                    Style="margin: auto; display: none;" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
     <fieldset id="fldSetForTemplate"  style="display: none; width: 90% !important; text-align: left !important; margin:20px !important; border: #aaaaaa 1px solid;" runat="server" >
            <legend class="LegendText" style="color: Black; text-align:left; font-size: 12px"  >
            <img id="img3" alt="Search Template" src="images/panelcollapse.png" onclick="Display(this,'divSubGroupPanel');" runat="server" style="margin-right: 2px;" />
                    <label id="lblTemplateName"></label> 
            </legend>
                <div id="divSubGroupPanel" style="margin:0px 0px 0px 175px;" >
                 </div>
         </fieldset>
  <div id="myModal" class="modal fade" role="dialog">
  <div class="modal-dialog modal-lg" style="">
    <!-- Modal content-->
      <div class="modal-content" style="width: 152% !Important; margin-left: -26%;">
          <div class="modal-header">
              <button type="button" class="close" data-dismiss="modal">&times;</button>
              <h4 class="modal-title">
                  <label id="lblHeader"></label>
                  <label id="lblTemplateId" style="display: none"></label>
                  <label id="lblGroup" style="display: none"></label>

              </h4>
          </div>
          <div class="table-responsive">
              <table class="table" style="margin-bottom: 0px;">
                  <tr>
                      <td style="text-align: right">Attribute Group :
                      </td>
                      <td style="text-align: left; width: 200px;">
                          <select id="ddlAttributeGroup" style="text-align: left; width: 200px;">
                          </select>
                      </td>
                      <td style="text-align: right">Attribute :
                      </td>
                      <td style="text-align: left; width: 200px;">
                          <select id="ddlAttribute" style="text-align: left; width: 200px;">
                          </select>
                      </td>
                      <td></td>
                      <td>
                          <input type="button" value="Add" id="btnAdd" onclick='return AddAttributeData()' class="button" />
                      </td>
                  </tr>
              </table>
          </div>
          <div class="modal-body" style="padding-top: 0px">
              <div class="table-responsive " style="max-height: 200px !important; overflow: auto;">
                  <table class="table" id="tblMedExMstTemp">
                      <thead>
                          <tr>
                              <th>AttributeGroupCode</th>
                              <th>Attribute Group</th>
                              <th>AttributeCode</th>
                              <th>Attribute Description</th>
                              <th>Delete</th>
                          </tr>
                      </thead>
                  </table>
              </div>
              <button type="button" class="button" onclick="SaveAttributeData();" data-dismiss="modal">Save</button>
              <div class="table-responsive " style="max-height: 300px !important; overflow: auto;">
                  <table class="table" id="tblMedExMst">
                  </table>
              </div>
          </div>
          <div class="modal-footer">
              <button type="button" class="btn btn-success" onclick="SaveData()" data-dismiss="modal">Update</button>
              <button type="button" id="btnDelete" onclick='DeleteAttributeData()' class="btn btncancel" data-dismiss="modal">Delete</button>
              <button type="button" id="btnCloseForModal" class="btn btnclose" data-dismiss="modal">Close</button>
          </div>
      </div>

  </div>
</div>

    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery-1.10.9.dataTables.min.js"></script>
    <script type="text/javascript" src="Script/jquery-ui_New.js"></script>
    <script type="text/javascript">
        function UIGV_PreviewAtrributeTemplate() {
            $('#<%= GV_PreviewAtrributeTemplate.ClientID%>').removeAttr('style', 'display:block');
            oTab = $('#<%= GV_PreviewAtrributeTemplate.ClientID%>').prepend($('<thead>').append($('#<%= GV_PreviewAtrributeTemplate.ClientID%> tr:first'))).dataTable({
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

        function OpenAttributeTemplate() {
            $('.tblmedex').css('display', '')
            $('#<%= img2.ClientID%>').click();
        }

        function CloseAttributeTemplate() {
            $('.tblmedex').css('display', 'none')
        }


        function pageLoad() {
            $('#ctl00_CPHLAMBDA_SeqMedex').sortable()
            $('#ctl00_CPHLAMBDA_SeqMedex').disableSelection();
            $('.ValidNum').ForceNumericOnly();
        }



        function ClientPopulated(sender, e) {
            MedexTemplateClientShowing('AutoCompleteExtender1', $get('<%= txtTemplate.ClientId %>'));
        }

        function OnSelected(sender, e) {
            MedexTemplateOnItemSelected(e.get_value(), $get('<%= txtTemplate.clientid %>'),
                    $get('<%= HTemplateId.clientid %>'), $get('<%=btnSetTemplate.clientid %>'));


        }

        function PreviewTemplate(Path) {
            window.open(Path);
            return false;
        }

        function closesequencediv() {
            $find('MPEMedexSequence').hide();
        }

        function ValidationToAdd() {
                     return true;
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


        function ValidateNumeric(txtBox, ddlSelected, msg) {
            var strUser = ddlSelected.value;
            // var CommaSepratedNumbers=/[0-9]+(,[0-9]+)*,?/ ;
            var CommaSepratedNumbers = /([1-9][0-9]*,)*[0-9][0-9]*/;
            //            value3=document.getElementById("ctl00_CPHLAMBDA_txtlength").value;
            value3 = txtBox.value;
            var arrayList = value3.match(CommaSepratedNumbers);
            if (strUser == 'NU') {
                if (value3.split(",")[0] < value3.split(",")[1]) {
                    msgalert('Please Enter Correct Scale !');
                    txtBox.value = "";
                    txtBox.focus();
                    return false;
                }
                if (strUser == 'NU' && (arrayList[1] == '' || typeof (arrayList[1]) == 'undefined')) {
                    msgalert('Please provide scale !');
                    txtBox.value = "";
                    txtBox.focus();
                    return false;
                }

                if (arrayList[1] == '') {
                    msgalert('Enter data not correct format !');
                    txtBox.value = "";
                    txtBox.focus();
                    return false;
                }
                else if (value3.split(",").length > 2) {
                    msgalert('Enter data not correct format !');
                    txtBox.value = "";
                    txtBox.focus();
                    return false;
                }

            }

            else

                var result = CheckDecimalOrBlank(txtBox.value);
            if (result == false) {
                window.msgalert(msg);
                txtBox.value = "";
                txtBox.focus();
            }
        }

        function ValidateMedexDesc(txtbox) {

            if (txtbox.value.toString().replace(/^\s*/, '').replace(/\s*$/, '').length <= 0) {
                txtbox.value = '';
                txtbox.focus();
                msgalert('Please Enter Attribute Description !');
                return false;
            }
            return true;
        }

        function ValidateAlertOn(txtboxMedexValue, txtboxAlertOn) {
            var txtMedExValue = txtboxMedexValue.value;
            var txtAlert = txtboxAlertOn.value;
            var result = txtMedExValue.split(",");

            if (txtboxAlertOn.value.toString().replace(/^\s*/, '').replace(/\s*$/, '').length > 0) {

                for (var i in result) {
                    if (txtAlert == result[i]) {
                        return true;
                    }
                }
                txtboxAlertOn.focus();
                msgalert('Please Enter Correct Alert On Value !');

                return false;
            }
            return true;
        }

        function ValidateNumericCode(evt) {
            var charCode = evt.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 44)
                return false;
            else
                if (charCode == 44) {
                    return true;
                }

            return true;
        }

        function OnChangeDropDown(e) {
            var const_txtLength = 14;
            var Const_txtLength_child = 0;
            $(e).attr('title', '');
            $(e).attr('title', e.options[e.selectedIndex].text);
            EditObject = e;
            $row = $(e).parents('tr:first');
            $get('hd_TxtLength').value = $row[0].cells[const_txtLength].children[Const_txtLength_child].value; //$row[0].cells[PatientId].firstChild.data;
            if (e.options[e.selectedIndex].text == 'Numeric') {
                $row[0].cells[const_txtLength].children[Const_txtLength_child].value = '0,0';
                return false;
            }
            else
                $row[0].cells[const_txtLength].children[Const_txtLength_child].value = 0;
            return false;
        }

        function savesequence() {
            var jsondata = $('#<%= hdnMedexList.clientid %>').val();
            var i = 0;
            if (jsondata.d != "") {
                var data = JSON.parse(jsondata);
                var cpydata = JSON.parse(jsondata);
                $('.allmed').each(function () {

                    var medexcode = $(this).attr('id').replace("ctl00_CPHLAMBDA_", "");
                    for (var a = 0; a < cpydata.length; a++) {
                        if (cpydata[a].vMedExCode == medexcode) {
                            cpydata[a].iSeqNo = data[i].iSeqNo;
                            i = parseInt(i) + 1;
                        }
                    }
                });
                var btn = $('#<%= btnSaveSequence.clientid %>');
                document.getElementById('ctl00_CPHLAMBDA_hdnMedexList').value = JSON.stringify(cpydata);
                btn.click();
                return false;
            }
        }
        jQuery.fn.ForceNumericOnly =
              function () {
                  return this.each(function () {
                      $(this).keydown(function (e) {
                          var key = e.charCode || e.keyCode || 0;
                          return (
                               key == 8 ||
                               key == 9 ||
                               key == 46 ||
                               key == 110 ||
                               key == 190 ||
                               (key >= 35 && key <= 40) ||
                               (key >= 48 && key <= 57) ||
                               (key >= 96 && key <= 105));
                      });
                  });
              };

        function fnAssigntitle(ControlID) {
            $(ControlID).attr('title', '');
            $(ControlID).attr('title', ControlID.options[ControlID.selectedIndex].text);
        }

        function EditTemplate(TempalteId, Scope) {

            
            var Scope = Scope
            $("#ctl00_CPHLAMBDA_fldSetForTemplate").attr("style", "display:block; width: 98% !important; text-align: left !important; margin:0px !important; border: #aaaaaa 1px solid;")

            $.ajax({
                type: "POST",
                url: "frmPreviewAttributeTemplateNew.aspx/GetTemplateDate  ",
                data: '{"TemplateId":"' + TempalteId + '","Scope":"' + Scope + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var aaDataSet = [];
                    data = JSON.parse(data.d);
                    $("#divSubGroupPanel").html("")
                    for (var Row = 0; Row < data.length; Row++) {
                        var InDataSet = [];
                        //var $input = $('<input type="button"  class="btn btn-info btn-lg" data-toggle="modal" data-target="#myModal"  onClick=  DisplayAttribute("' + data[Row].vMedExSubGroupCode + '","' + data[Row].vMedExTemplateId + '") style="width:90%; color:white; font-weight: bold; text-align:left; background-color:#4974c3;" Class="Button" value="' + data[Row].vMedExSubGroupdesc + '"  id=btn' + data[Row].vMedExSubGroupCode + "-" + data[Row].vMedExTemplateId + '"/>');
                        var $input = $('<input type="button"  class="plus btn btn-info plus-text" data-toggle="modal" data-target="#myModal"  style="width:88%; color:white; font-weight: bold; text-align:left; background-color:#4974c3;" Class="Button" onClick=  DisplayAttribute("' + data[Row].vMedExSubGroupCode + '","' + data[Row].vMedExTemplateId + '","' + Scope + '")  value="' + data[Row].vMedExSubGroupDesc + '"  id=btn' + data[Row].vMedexSubGroupCode + "-" + data[Row].vMedExTemplateId + '"/>');

                        //var $input = $('<input type="button"  class="plus btn btn-info" data-toggle="modal" data-target="#myModal"   Class="Button" onClick=  DisplayAttribute("' + data[Row].vMedExSubGroupCode + '","' + data[Row].vMedExTemplateId + '","' + Scope + '")  value="' + data[Row].vMedExSubGroupdesc + '"  id=btn' + data[Row].vMedExSubGroupCode + "-" + data[Row].vMedExTemplateId + '"><i class="icon icon-plus-sign"></i></input>');
                        $input.appendTo($("#divSubGroupPanel"));
                        $("#divSubGroupPanel").append("</BR>")
                        $("#lblTemplateName").text(data[Row].vTemplateName)
                    }
                    //$("#lblTemplateName").text(TempalteId)
                },
                failure: function (error) {
                    msgalert(error);
                }
            });

        }
        var tblMedExMstArray = {};
        var tblMedExMstTempArray = {};
        function AddMedExMstTemp(flag) {
            if (flag) {
                if (Object.keys(tblMedExMstTempArray).length > 0) {
                    $("#tblMedExMstTemp").DataTable().clear().draw();
                    tblMedExMstTempArray = {};
                }
                $("#tblMedExMstTemp").dataTable().fnDestroy();
                $('#tblMedExMstTemp').DataTable({
                    "bJQueryUI": false, "bFilter": false,
                    "bInfo": false, "bPaginate": false,
                    "fnCreatedRow": function (nRow, aData, iDataIndex) {
                        $('td:eq(0)', nRow).eq(0).hide();
                        $('td:eq(2)', nRow).eq(0).hide();
                        $('td:eq(4)', nRow).html("<input type='image' src='Images/Delete.png' id=" + aData[2] + " />").click(function () {
                            $('#tblMedExMstTemp').DataTable().row($(this).parents('tr')).remove().draw();
                            delete tblMedExMstTempArray[$(this).find("input")[0].id];
                            return false;
                        });
                    }
                });
                $('#tblMedExMstTemp tr').each(function () {
                    $(this).find('th').eq(0).hide();
                    $(this).find('th').eq(2).hide();
                });
            }
            else {
                if (!($("#ddlAttribute").val() in tblMedExMstTempArray)) {
                    tblMedExMstTempArray[$("#ddlAttribute").val()] = $("#ddlAttributeGroup").val();
                    $('#tblMedExMstTemp').DataTable().row.add([
                        $("#ddlAttributeGroup").val(),
                        $("#ddlAttributeGroup :selected").text(),
                        $("#ddlAttribute").val(),
                        $("#ddlAttribute :selected").text(),
                        'delete'
                    ]).draw(false);
                }
                else { msgalert('Attribute Already Added!');}
            }
        }
        function DisplayAttribute(SubGroupCode, TemplateId, Scope) {
            var Scope = Scope
            $('#tblMedExMst tr').not(function () { return !!$(this).has('th').length; }).remove()
            var select = $('#ddlAttribute')
            select.html("")
            $("#lblGroup").text(SubGroupCode)
            var select = ddlAttributeGroup
            var UOM 
            AddMedExMstTemp(true);
            $.ajax({
                type: "POST",
                url: "frmPreviewAttributeTemplateNew.aspx/GetGroupData  ",
                data: '{"TemplateId":"' + TemplateId + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var aaDataSet = [];
                    data = JSON.parse(data.d);
                    var select = $('#ddlAttributeGroup')
                    select.html("")
                    select.append('<option value="0">' + "Select Attribute Group " + '</option>')
                    for (var Row = 0; Row < data.length; Row++) {
                        var InDataSet = [];
                        select.append('<option value="' + data[Row].vMedExGroupCode + '">' + data[Row].vMedExGroupDesc + '</option>')
                    }

                },
                failure: function (error) {
                    msgalert(error);
                }
            });

            



            $.ajax({
                type: "POST",
                url: "frmPreviewAttributeTemplateNew.aspx/GetTemplateDateToDisplay  ",
                data: '{"TemplateId":"' + TemplateId + '","Scope":"' + Scope + '","SubGroupId":"' + SubGroupCode + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var aaDataSet = [];


                    data = JSON.parse(data.d);
                    tblMedExMstArray = {};
                    for (var Row = 0; Row < data.length; Row++) {
                        var InDataSet = [];
                        tblMedExMstArray[data[Row].vMedExCode] = data[Row].vMedExGroupCode;
                        InDataSet.push("", data[Row].vMedExDesc, data[Row].vMedExType, data[Row].vMedExValues, data[Row].vDefaultValue, data[Row].vAlertonvalue, data[Row].vAlertMessage,
                            data[Row].vLowRange, data[Row].vHighRange, data[Row].vUOM, data[Row].vValidationType, "", data[Row].cAlertType, data[Row].vCDISCValues, data[Row].vOtherValues, data[Row].vMedExCode,
                            data[Row].nMedExTemplateDtlNo, data[Row].vMedExTemplateId, data[Row].vTemplateName, data[Row].iSeqNo, data[Row].cActiveFlag, data[Row].vProjectTypeCode);
                        aaDataSet.push(InDataSet);
                        $("#lblHeader").text(data[Row].vTemplateName)
                        $("#lblTemplateId").text(data[Row].vMedExTemplateId)
                    }
                    console.log('1 @' + data.d)
                    if ($("#tblMedExMst").children().length > 0) {
                        $("#tblMedExMst").dataTable().fnDestroy();
                    }

                    var int = 0
                    $('#tblMedExMst').prepend($('<thead>').append($('#tblMedExMst tr:first'))).dataTable({
                        "bJQueryUI": true,
                        "sPaginationType": "full_numbers",
                        "bLengthChange": true,
                        "iDisplayLength": -1,
                        "bProcessing": true,
                        "bSort": false,
                        "aaData": aaDataSet,
                        aLengthMenu: [
                           [5, 25, 50, -1],
                           [5, 25, 50, "All"]
                        ],
                        "fnCreatedRow": function (nRow, aData, iDataIndex) {
                            int += 1
                            $('td:eq(0)', nRow).html("<input type='CheckBox' style='Width:40px;' id=" + aData[16] +" />");
                            $('td:eq(1)', nRow).html("<TextArea class='textBox' onchange='SetDefault($(this),$(this).val());' style='Width:170px;' id=" + aData[15] + "" + int + "2" + " >" + aData[1].trimStart().trimEnd().trim() + "</TextArea>");


                            var select = $('<select style="Width:70px;"><option value="" ></option></select>')
                            select.append('<option value="' + "AsyncDateTime" + '">' + 'AsyncDateTime' + '</option>')
                            select.append('<option value="' + "AsyncTime" + '">' + 'AsyncTime' + '</option>')
                            select.append('<option value="' + "CheckBox" + '">' + 'CheckBox' + '</option>')
                            select.append('<option value="' + "ComboBox" + '">' + 'ComboBox' + '</option>')
                            select.append('<option value="' + "ComboGlobalDictionary" + '">' + 'ComboGlobalDictionary' + '</option>')
                            select.append('<option value="' + "CrfTerm" + '">' + 'CrfTerm' + '</option>')
                            select.append('<option value="' + "DateTime" + '">' + 'DateTime' + '</option>')
                            select.append('<option value="' + "File" + '">' + 'File' + '</option>')
                            select.append('<option value="' + "Formula" + '">' + 'Formula' + '</option>')
                            select.append('<option value="' + "Import" + '">' + 'Import' + '</option>')
                            select.append('<option value="' + "Label" + '">' + 'Label' + '</option>')
                            select.append('<option value="' + "Radio" + '">' + 'Radio' + '</option>')
                            select.append('<option value="' + "STANDARDDATE" + '">' + 'STANDARDDATE' + '</option>')
                            select.append('<option value="' + "TextArea" + '">' + 'TextArea' + '</option>')
                            select.append('<option value="' + "TextBox" + '">' + 'TextBox' + '</option>')
                            select.append('<option value="' + "Time" + '">' + 'Time' + '</option>')
                            select.append('<option value="' + aData[2] + '" selected="selected">' + aData[2] + '</option>')
                            $('td:eq(2)', nRow).html(select)

                            $('td:eq(3)', nRow).html("<TextArea class='textBox' onchange='SetDefault($(this),$(this).val());'  style='Width:70px;' id=" + aData[15] + "" + int + "3" + "  >" + aData[3].toString().trimEnd().trimStart() + " </TextArea>");
                            $('td:eq(4)', nRow).html("<TextArea class='textBox' onchange='SetDefault($(this),$(this).val());' style='Width:70px;' id=" + aData[15] + "" + int + "4" + "  >" + aData[4].toString().trimEnd().trimStart() + " </TextArea>");
                            $('td:eq(5)', nRow).html("<TextArea class='textBox' onchange='SetDefault($(this),$(this).val());' style='Width:70px;' id=" + aData[15] + "" + int + "5" + "  >" + aData[5].toString().trimEnd().trimStart() + " </TextArea>");
                            $('td:eq(6)', nRow).html("<TextArea class='textBox' onchange='SetDefault($(this),$(this).val());' style='Width:70px;' id=" + aData[15] + "" + int + "6" + "  >" + aData[6].toString().trimEnd().trimStart() + " </TextArea>");
                            $('td:eq(7)', nRow).html("<TextArea class='textBox' onchange='SetDefault($(this),$(this).val());' style='Width:30px;' id=" + aData[15] + "" + int + "7" + "  >" + aData[7].toString().trimEnd().trimStart() + " </TextArea>");
                            $('td:eq(8)', nRow).html("<TextArea class='textBox' onchange='SetDefault($(this),$(this).val());'  style='Width:30px;' id=" + aData[15] + "" + int + "8" + "  >" + aData[8].toString().trimEnd().trimStart() + " </TextArea>");

                            $('td:eq(9)', nRow).html(getUOM(aData[9]))

                            var select = $('<select style="Width:70px;"><option value="" ></option></select>')
                            if (aData[10].split(",")[0] == 'NA' || aData[10].split(",") == "") {
                                select.append('<option  selected="selected" value="' + "NA" + '">' + 'Not Applicable' + '</option>')
                            }
                            else{
                                select.append('<option  " value="' + "NA" + '">' + 'Not Applicable' + '</option>')
                            }
                            if (aData[10].split(",")[0] == 'AN') {
                                select.append('<option selected="selected"  value="' + "AN" + '">' + 'Alpha Numeric' + '</option>')
                            }
                            else{
                                select.append('<option   value="' + "AN" + '">' + 'Alpha Numeric' + '</option>')
                            }

                            if (aData[10].split(",")[0] == 'NU') {
                                select.append('<option selected="selected"  value="' + "NU" + '">' + 'Numeric' + '</option>')
                            }
                            else{
                                select.append('<option   value="' + "NU" + '">' + 'Numeric' + '</option>')
                            }
                            if (aData[10].split(",")[0] == 'IN') {
                                select.append('<option selected="selected"  value="' + "IN" + '">' + 'Integer' + '</option>')
                            }
                            else{
                                select.append('<option  value="' + "IN" + '">' + 'Integer' + '</option>')
                            }
                            if (aData[10].split(",")[0] == 'AL') {
                                select.append('<option selected="selected"  value="' + "AL" + '">' + 'Alphabate' + '</option>')
                            }
                            else {
                                select.append('<option  value="' + "AL" + '">' + 'Alphabate' + '</option>')
                            }
                            $('td:eq(10)', nRow).html(select)

                            $('td:eq(11)', nRow).html("<TextArea class='textBox' style='Width:70px;' id=" + aData[15] + "" + int + "11" + " onkeypress='return ValidateNumericCode(event);'  >" + aData[11] + " </TextArea>");

                            if (aData[12] == 'N') {
                                $('td:eq(12)', nRow).html("<input  type='CheckBox' style='Width:40px;' id=" + aData[12]  +" />");
                            }
                            else {
                                $('td:eq(12)', nRow).html("<input type='CheckBox' checked='checked' style='Width:40px;' id=" + aData[12] + " />");
                            }

                            $('td:eq(13)', nRow).html("<TextArea class='textBox' onchange='SetDefault($(this),$(this).val());' style='Width:70px;' id=" + aData[15] + "" + int + "13" + "  >" + aData[13].toString().trimEnd().trimStart() + " </TextArea>");
                            $('td:eq(14)', nRow).html("<TextArea class='textBox' onchange='SetDefault($(this),$(this).val());' style='Width:70px;' id=" + aData[15] + "" + int + "14" + "  >" + aData[14].toString().trimEnd().trimStart() + " </TextArea>");
                            $('td:eq(15)', nRow).html("<TextArea class='textBox' style='Width:70px;' id=" + aData[15] + "" + int + "15" + "  >" + aData[15] + " </TextArea>");

                            $('td:eq(16)', nRow).html("<TextArea class='textBox' style='Width:70px;' id=" + aData[15] + "" + int + "16" + "  >" + aData[16].toString().trimEnd().trimStart() + " </TextArea>");
                            $('td:eq(17)', nRow).html("<TextArea class='textBox' style='Width:70px;' id=" + aData[15] + "" + int + "17" + "  >" + aData[17].toString().trimEnd().trimStart() + " </TextArea>");
                            $('td:eq(18)', nRow).html("<TextArea class='textBox' style='Width:70px;' id=" + aData[15] + "" + int + "18" + "  >" + aData[18].toString().trimEnd().trimStart() + " </TextArea>");
                            $('td:eq(19)', nRow).html("<TextArea class='textBox' style='Width:70px;' id=" + aData[15] + "" + int + "19" + "  >" + aData[19].toString().trimEnd().trimStart() + " </TextArea>");
                            $('td:eq(20)', nRow).html("<TextArea class='textBox' style='Width:70px;' id=" + aData[15] + "" + int + "20" + "  >" + aData[20].toString().trimEnd().trimStart() + " </TextArea>");
                            $('td:eq(21)', nRow).html("<TextArea class='textBox' style='Width:70px;' id=" + aData[15] + "" + int + "21" + "  >" + aData[21].toString().trimEnd().trimStart() + " </TextArea>");
                            $('td:eq(22)', nRow).html("<TextArea class='textBox' style='Width:70px;'  id=" + aData[15] + "" + int + "22" + "  >" + aData[22] + " </TextArea>");

                        },
                        "aoColumns": [
                                          { "sTitle": "Select" },
                                          { "sTitle": "Attribute Description" },
                                          { "sTitle": "Attribute Type" },
                                          { "sTitle": "Attribute Value" },
                                          { "sTitle": "Default Value" },
                                          { "sTitle": "Alert On" },
                                          { "sTitle": "Alert Message" },
                                          { "sTitle": "Low Range" },
                                          { "sTitle": "High Range" },
                                          { "sTitle": "UOM" },
                                          { "sTitle": "Validation Type" },
                                          { "sTitle": "Length" },
                                          { "sTitle": "Not Null" },
                                          { "sTitle": "CDISC" },
                                          { "sTitle": "Other" },
                                          { "sTitle": "vMedExCode" },
                                          { "sTitle": "nMedExTemplateDtlNo" },
                                          { "sTitle": "vMedExTemplateId" },
                                          { "sTitle": "vTemplateName" },
                                          { "sTitle": "iSeqNo" },
                                          { "sTitle": "cActiveFlag" },
                                          { "sTitle": "vProjectTypeCode " },


                        ],
                        //"columnDefs": [
                        //    {
                        //        "targets": [20],
                        //        "visible": false,
                        //    },
                        //    {
                        //        "targets": [21],
                        //        "visible": false
                        //      }
                        //],


                    });

                    //if (data.length == 0) {
                    //    $('#tblMedExMst_wrapper').css("display", "none")
                    //}
                    $('#tblMedExMst tr').each(function () {
                        $(this).find('th').eq(15).hide();
                        $(this).find('td').eq(15).hide();
                        $(this).find('th').eq(16).hide();
                        $(this).find('td').eq(16).hide();
                        $(this).find('th').eq(17).hide();
                        $(this).find('td').eq(17).hide();
                        $(this).find('th').eq(18).hide();
                        $(this).find('td').eq(18).hide();
                        $(this).find('th').eq(19).hide();
                        $(this).find('td').eq(19).hide();
                        $(this).find('th').eq(20).hide();
                        $(this).find('td').eq(20).hide();
                        $(this).find('th').eq(21).hide();
                        $(this).find('td').eq(21).hide();
                    });

                    //$('#tblMedExMst tr:first').css('background-color', '#3A87AD');
                },
                failure: function (error) {
                    msgalert(error);
                }
            });

        }

        var UOMData = undefined;
        function getUOM(UOM) {

            var select = $('<select class="UOM" style="Width:70px;"></select>')
            var Scope = ""
            select.html = ""

            if (UOMData != undefined) {
                select.append('<option value="">' + "Select UOM " + '</option>')
                for (var Row1 = 0; Row1 < UOMData.length; Row1++) {
                    if (UOMData[Row1].vUOMDesc.toString() == UOM) {
                        select.append('<option Selected="Selected" value="' + UOMData[Row1].vUOMDesc + '">' + UOMData[Row1].vUOMDesc + '</option>')
                    }
                    else {
                        select.append('<option value="' + UOMData[Row1].vUOMDesc + '">' + UOMData[Row1].vUOMDesc + '</option>')
                    }
                }
                return select;
            }
            $.ajax({
                type: "POST",
                url: "frmPreviewAttributeTemplateNew.aspx/GetUOM  ",
                data: '{"Scope":"' + Scope + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async:false,
                success: function (data123) {
                    var aaDataSet = [];
                    data123 = JSON.parse(data123.d);
                    UOMData = data123;
                    select.append('<option value="">' + "Select UOM " + '</option>')
                    for (var Row1 = 0; Row1 < data123.length; Row1++) {
                        if (data123[Row1].vUOMDesc.toString() == UOM) {
                            select.append('<option Selected="Selected" value="' + data123[Row1].vUOMDesc + '">' + data123[Row1].vUOMDesc + '</option>')
                        }
                        else {
                            select.append('<option value="' + data123[Row1].vUOMDesc + '">' + data123[Row1].vUOMDesc  + '</option>')
                        }   
                    }
                },
                failure: function (error) {
                    msgalert(error);
                }
            });
            return select;
        }

        function SaveData() {
            var Quantity
            var objSaveData = [];
            var table = $("#tblMedExMst  tbody");
            var dataTable = $('#tblMedExMst').dataTable()
            var int = 1 
            $(dataTable.fnGetNodes()).each(function () {
                var chkcAlertType = ""
                var $tds = $(this).find('td')
                if ($tds.eq(12).find("input[type='checkbox']:checked").length > 0) {
                    chkcAlertType = 'Y'
                }
                else {
                    chkcAlertType = 'N'
                }
                //Quantity = 
              objSaveData.push({
                  nMedExTemplateDtlNo: $tds.eq(16).text().trim(),
                  vMedExTemplateId: $tds.eq(17).text().trim(),
                  vTemplateName: $tds.eq(18).text().trim(),
                  iSeqNo: $tds.eq(19).text().trim(),
                  vMedExCode: $tds.eq(15).text().trim(),
                  vDefaultValue: $tds.eq(4).text().trim(),
                  cActiveFlag: $tds.eq(20).text().trim(),
                  iModifyBy: $("#hdnUserId").val(),
                  dModifyOn: "",
                  cStatusIndi: "E",
                  vProjectTypeCode: $tds.eq(21).text().trim(),
                  vMedExDesc: $tds.eq(1).text().trim(),
                  vMedExGroupCode: "",
                  vmedexgroupDesc: "",
                  vMedexGroupCDISCValue: "",
                  vmedexGroupOtherValue: "",
                  vMedExSubGroupCode: "",
                  vmedexsubGroupDesc: "",
                  vMedexSubGroupCDISCValue: "",
                  vmedexsubGroupOtherValue: "",
                  vMedExType: $tds.eq(2).find("select").val().trim(),
                  vMedExValues : $tds.eq(3).text().trim(),
                  vUOM: $tds.eq(9).find("select").val().trim(),
                  vLowRange: $tds.eq(7).text().trim(),
                  vHighRange: $tds.eq(8).text().trim(),
                  vValidationType: $tds.eq(10).find("select").val(),
                  vAlertonvalue: $tds.eq(5).text().trim(),
                  vAlertMessage: $tds.eq(6).text().trim(),
                  cAlertType: chkcAlertType,
                  vCDISCValues : $tds.eq(13).text().trim(),
                  vOtherValues : $tds.eq(14).text().trim(),
                  vMedexFormula: "",
                  MedExTemplateId: $tds.eq(17).text().trim(),
                  DATAOPMODE: 2,
              })
            })
            var JSONObject = JSON.stringify(objSaveData);


            $.ajax({
                type: "POST",
                url: "frmPreviewAttributeTemplateNew.aspx/SaveTemplateData",
                //data: objSaveData,
                //contentType: "application/json; charset=utf-8",
                dataType: "json",
                //data: "{ MedExTemplateDtl :" + JSONObject + "}",
                data: "{MedExTemplateDtl:" + JSONObject + "," + "iModifyBy:'" + $("#ctl00_CPHLAMBDA_txtUserId").val() + "'}",
                //data: '{"TemplateId":"' + TemplateId + '","Scope":"' + Scope + '","SubGroupId":"' + SubGroupCode + '"}',
                contentType: 'application/json; charset=utf-8',

                async: false,
                success: function (data123) {
                    data123 = data123.d
                    if (data123 == "True") {
                        msgalert("Data Saved Successfully")
                    }
                    else {
                        msgalert("Error While Saving Attribute Details In MedexTemplateDtl")
                    }

                },
                failure: function (error) {
                    msgalert(error);
                }
            });


            //table.find('tr').each(function (i, el) {
            //    var $tds = $(this).find('td'),
            //        Quantity = 
            //    objSaveData.push({
            //        vMedExDesc: $tds.eq(1).text(),
            //        vMedExType : $(this).find("select").val(),
            //        vMedExValues : $tds.eq(3).text(),
            //        vDefaultValue : $tds.eq(4).text(),
            //        vAlertonvalue: $tds.eq(5).text(),
            //        vAlertMessag : $tds.eq(6).text(),
            //        vLowRange : $tds.eq(7).text(),
            //        vHighRange : $tds.eq(8).text(),
            //        cAlertType : $tds.eq(12).text(),
            //        vCDISCValues : $tds.eq(13).text(),
            //        vOtherValues : $tds.eq(14).text(),
            //        vMedExCode: $tds.eq(15).text(),
            //        nMedExTemplateDtlNo: $tds.eq(16).text(),
            //        vMedExTemplateId: $tds.eq(17).text(),
            //        vTemplateName: $tds.eq(18).text(),
            //        iSeqNo: $tds.eq(19).text(),
            //        cActiveFlag: $tds.eq(20).text(),
            //        vProjectTypeCode: $tds.eq(20).text(),
            //        DATAOPMODE: 2,
            //    });
            //    alert(objSaveData)
            //});
        }
       
        $('#ddlAttributeGroup').change(function () {
            var selectedVal = $('#ddlAttributeGroup option:selected').attr('value');
            var SelectSubGroupValue = $("#lblGroup").text();
            $.ajax({
                type: "POST",
                url: "frmPreviewAttributeTemplateNew.aspx/GetGroupAttributeData  ",
                <%--data: '{"GroupId":"' + selectedVal + '" }',--%>
                //data: '{"GroupId":"' + selectedVal + '"  }',
                data: "{GroupId:'" + selectedVal + "'," + "SubGroupId:'" + SelectSubGroupValue + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var aaDataSet = [];
                    data = JSON.parse(data.d);
                    var select = $('#ddlAttribute')
                    select.html("")
                    select.append('<option value="' + 0 +'">' + "Select Attribute " + '</option>')
                    for (var Row = 0; Row < data.length; Row++) {
                        var InDataSet = [];
                        select.append('<option value="' + data[Row].vMedExCode + '">' + data[Row].vMedExDesc + '</option>')
                    }
                },
                failure: function (error) {
                    msgalert(error);
                }
            });


        });

        function SetDefault(id, value) {
            var ID1 = id.context.id
            $("textarea#" + ID1).html(value)
        }

        $("#btnCloseForModal").click(function () {
           
            if ($("#tblMedExMst").children().length > 0) {
            }
        });
        function SaveAttributeData() {
            if (Object.keys(tblMedExMstTempArray).length > 0) {
                $.ajax({
                    type: "POST",
                    url: "frmPreviewAttributeTemplateNew.aspx/SaveAttributeInTemplate  ",
                    data: '{"MedExCode":"' + $('#ddlAttribute :selected').val() + '","TemplateId":"' + $("#lblTemplateId").text() + '","UserId":"' + $("#ctl00_CPHLAMBDA_txtUserId").val() + '","BulkAttribute":"' + Object.keys(tblMedExMstTempArray) + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        data = data.d
                        if (data == "True") {
                            msgalert("Data Saved Successfully");
                            EditTemplate($("#lblTemplateId").text(), $("#ctl00_CPHLAMBDA_hdnUserScope").val())
                            $('#myModal').modal('show');
                            DisplayAttribute($("#lblGroup").text(), $("#lblTemplateId").text(), $("#ctl00_CPHLAMBDA_hdnUserScope").val())
                        }
                        else {
                            msgalert("Error While Save Data")
                        }
                    },
                    failure: function (error) {
                        msgalert(error);
                    }
                });
            }
            else {
                msgalert('Please Add Attribute!');
            }
        }
        function AddAttributeData() {
            if ($("#ddlAttributeGroup option:selected").index() <= 0) {
                msgalert("Please Select Attribute Group")
                return false;
            }
            if ($("#ddlAttribute option:selected").index() <= 0) {
                msgalert("Please Select Attribute")
                return false;
            }
            else {
                var element = $("#ddlAttribute").val() in tblMedExMstArray;
                if (element) {
                    msgalert("Selected Attribute is Already Added  !");
                    return false;
                }
            }
            AddMedExMstTemp(false);
        }

        function DeleteAttributeData() {
            var dataTable = $('#tblMedExMst').dataTable()
            var MedExTemplateDtl = ","
            $(dataTable.fnGetNodes()).each(function () {
                var chkcAlertType = ""
                var $tds = $(this).find('td')
                if ($tds.eq(0).find("input[type='checkbox']:checked").length > 0) {
                    MedExTemplateDtl +=  $tds.eq(0).find("input[type='checkbox']:checked")[0].id + ","
                }
            });
            MedExTemplateDtl = MedExTemplateDtl.substr(0, MedExTemplateDtl.length - 1)
            $.ajax({
                type: "POST",
                url: "frmPreviewAttributeTemplateNew.aspx/DeleteAttributeInTemplate  ",
                data: '{"UserId":"' + $("#ctl00_CPHLAMBDA_txtUserId").val() + '","vmedExTemplateDtlNo":"' + MedExTemplateDtl + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    data = data.d
                    if (data == "True") {
                        msgalert("Data Deleted Successfully !")
                        EditTemplate($("#lblTemplateId").text(), $("#ctl00_CPHLAMBDA_hdnUserScope").val())
                        $('#myModal').modal('show');
                        DisplayAttribute($("#lblGroup").text(), $("#lblTemplateId").text(), $("#ctl00_CPHLAMBDA_hdnUserScope").val())
                        
                    }
                    else {
                        msgalert("Error While Delete Data")
                    }

                },
                failure: function (error) {
                    msgalert(error);
                }
            });

        }

        $('#tblMedExMst').on('page.dt', function () {
            $('#tblMedExMst tr').each(function () {
                $(this).find('th').eq(15).hide();
                $(this).find('td').eq(15).hide();
                $(this).find('th').eq(16).hide();
                $(this).find('td').eq(16).hide();
                $(this).find('th').eq(17).hide();
                $(this).find('td').eq(17).hide();
                $(this).find('th').eq(18).hide();
                $(this).find('td').eq(18).hide();
                $(this).find('th').eq(19).hide();
                $(this).find('td').eq(19).hide();
                $(this).find('th').eq(20).hide();
                $(this).find('td').eq(20).hide();
                $(this).find('th').eq(21).hide();
                $(this).find('td').eq(21).hide();
            });
        });

        function CloseDetails()
        {
           
            $("#ctl00_CPHLAMBDA_fldSetForTemplate").attr("style", "display:none;")
            $("#divSubGroupPanel").html("");
        }

    </script>

</asp:Content>
