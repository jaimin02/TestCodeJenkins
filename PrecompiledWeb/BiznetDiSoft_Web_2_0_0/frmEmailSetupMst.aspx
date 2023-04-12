<%@ page title="" language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmEmailSetupMst, App_Web_qa4vhgvp" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <style type="text/css">
        .ui-timepicker-wrapper {
            overflow-y: auto;
            height: 150px;
            width: 6.5em;
            background: #fff;
            border: 1px solid #ddd;
            -webkit-box-shadow: 0 5px 10px rgba(0,0,0,0.2);
            -moz-box-shadow: 0 5px 10px rgba(0,0,0,0.2);
            box-shadow: 0 5px 10px rgba(0,0,0,0.2);
            outline: none;
            z-index: 10001;
            margin: 0;
        }

        .ui-dialog .ui-dialog-content {
            width: auto;
            min-height: 50px;
            max-height: 155px;
            overflow: scroll;
            text-align: left;
        }

        .ui-timepicker-wrapper.ui-timepicker-with-duration {
            width: 11em;
        }

        .ui-timepicker-list {
            margin: 0;
            padding: 0;
            list-style: none;
        }

        .ui-timepicker-duration {
            margin-left: 5px;
            color: #888;
        }

        .ui-timepicker-list:hover .ui-timepicker-duration {
            color: #888;
        }

        .ui-timepicker-list li {
            padding: 3px 0 3px 5px;
            cursor: pointer;
            white-space: nowrap;
            color: #000;
            list-style: none;
            margin: 0;
            font-size: 15px;
        }

        .ui-timepicker-list:hover .ui-timepicker-selected {
            background: #fff;
            color: #000;
        }

        li.ui-timepicker-selected, .ui-timepicker-list li:hover, .ui-timepicker-list .ui-timepicker-selected:hover {
            background: #1980EC;
            color: #fff;
        }

            li.ui-timepicker-selected .ui-timepicker-duration, .ui-timepicker-list li:hover .ui-timepicker-duration {
                color: #ccc;
            }

        .ui-timepicker-list li.ui-timepicker-disabled, .ui-timepicker-list li.ui-timepicker-disabled:hover, .ui-timepicker-list li.ui-timepicker-selected.ui-timepicker-disabled {
            color: #888;
            cursor: default;
        }

            .ui-timepicker-list li.ui-timepicker-disabled:hover, .ui-timepicker-list li.ui-timepicker-selected.ui-timepicker-disabled {
                background: #f2f2f2;
            }

        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

        /*#ctl00_CPHLAMBDA_GV_UserType_wrapper {
            margin: 0px 235px;
        }*/

        .ajax__calendar_container {
            z-index: 1;
        }
    </style>

    <script type="text/javascript" src="script/jquery-1.7.min.js"></script>
    <script src="Script/jquery.timepicker.js" type="text/javascript"></script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellpadding="5px" style="width: 100%;">
                <tr>
                    <td>
                        <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                            <legend class="LegendText" style="color: Black; font-size: 12px">
                                <img id="img2" alt="Email Setup Details" src="images/panelcollapse.png"
                                    onclick="Display(this,'divEmailSetupDetail');" runat="server" style="margin-right: 2px;" />Email Setup Details</legend>
                            <div id="divEmailSetupDetail">
                                <table border="0" align="center" style="width: 80%; margin-bottom: 2%; margin-left: 1%;" cellpadding="5px">
                                    <%--<tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblmfa" owrap="nowrap" style="text-align: right; width: 30%; padding-left: 30%;"  runat="server" Text="Study Specific :">
                                                <asp:CheckBox ID="chkStudy" Visible="True" ToolTip="IsContractual" runat="server" 
                                                     OnCheckedChanged="chkStudy_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
                                            </asp:Label>
                                        </td>
                                    </tr>--%>

                                    <tr>
                                        <td style="text-align: right;" class="Label">Project*:</td>
                                        <td class="Label" style="text-align: left; width: 25%">
                                            <asp:TextBox ID="txtstudy" runat="server" CssClass="textBox" Width="305px" TabIndex="1"></asp:TextBox>
                                            <asp:Button Style="display: none" ID="btnstudy" runat="server" Text=" Project"></asp:Button>
                                            <asp:HiddenField ID="HParentProjectId" runat="server"></asp:HiddenField>
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" BehaviorID="AutoCompleteExtender2"
                                                CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelectedstudy"
                                                OnClientShowing="ClientPopulatedstudy" ServiceMethod="GetParentProjectCompletionList"
                                                ServicePath="AutoComplete.asmx" CompletionListElementID="pnlProjectList" TargetControlID="txtstudy"
                                                UseContextKey="True">
                                            </cc1:AutoCompleteExtender>
                                            <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden;" />
                                        </td>
                                        <td style="text-align: right; width: 10%;" class="Label" rowspan="3">Site*:</td>
                                        <td style="text-align: left; width: 25%;" rowspan="3">
                                            <asp:CheckBox ID="chkSelectAllStudy" runat="server" Text="Study Specific" __designer:wfdid="w4" OnCheckedChanged="chkStudy_CheckedChanged" AutoPostBack="true" ToolTip='If Study Specific Than Email Set Required For New Site'></asp:CheckBox>
                                            <div style="border-right: gray thin solid; border-top: gray thin solid; overflow-y: scroll; border-left: gray thin solid; width: 305px; border-bottom: gray thin solid; height: 100px; text-align: left"
                                                id="dvChkListstudy" runat="server">
                                                <asp:CheckBoxList ID="chklststudy" runat="server" ForeColor="Black" Font-Size="XX-Small"
                                                    Font-Names="Verdana" CssClass="checkboxlist" Width="260px" __designer:wfdid="w5"
                                                    Height="37px" TabIndex="4">
                                                </asp:CheckBoxList>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;" class="Label">Operation*:</td>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="ddlOperation" runat="server" CssClass="dropDownList" Width="304px"
                                                AutoPostBack="True" TabIndex="2" EnableViewState="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;" class="Label">Profile*:</td>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="ddlProfile" TabIndex="3" runat="server" CssClass="dropDownList"
                                                Width="305px" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;" class="Label">User*:</td>
                                        <td style="text-align: left;">
                                            <br />
                                            <asp:CheckBox ID="chkSelectAllUser" runat="server" Text="User All" __designer:wfdid="w4"></asp:CheckBox>
                                            <div style="border-right: gray thin solid; border-top: gray thin solid; overflow-y: scroll; border-left: gray thin solid; width: 305px; border-bottom: gray thin solid; height: 100px; text-align: left"
                                                id="dvChkListLocation" runat="server">
                                                <asp:CheckBoxList ID="chklstUser" runat="server" ForeColor="Black" Font-Size="XX-Small"
                                                    Font-Names="Verdana" CssClass="checkboxlist" OnSelectedIndexChanged="chklstUser_SelectedIndexChanged" Width="260px" __designer:wfdid="w5"
                                                    Height="37px" TabIndex="4" AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </div>
                                        </td>
                                        <td style="text-align: right; width: 10%;" class="Label">EmailId*:</td>
                                        <td style="text-align: left; width: 25%;">
                                            <asp:TextBox ID="txtMailId" runat="server" Rows="5" TextMode="MultiLine" Width="100%" Height="100%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <div runat="server" id="trRemarks" visible="false">
                                            <td style="text-align: right;" class="Label">Remark*:</td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtRemark" runat="server" Rows="2" Columns="20" CssClass="textBox" Style="width: 100%; height: auto;" TextMode="MultiLine" MaxLength="500" />
                                            </td>
                                        </div>
                                    </tr>
                                    <tr>
                                        <td class="Label" nowrap="nowrap" colspan="8" style="text-align: center; vertical-align: top;">
                                            <asp:Button ID="btnSave" OnClientClick="return Validation(this.id);" runat="server" CssClass="btn btnsave"
                                                Text="Save" ToolTip="Save" TabIndex="5" />
                                            <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="btn btncancel"
                                                Text="Cancel" ToolTip="Cancel" TabIndex="6" />
                                            <asp:Button ID="btnclose" runat="server" CausesValidation="False" CssClass="btn btnclose"
                                                Text="Exit" OnClientClick="return msgconfirmalert('Are You Sure You Want To Exit?',this);"
                                                ToolTip="Exit" TabIndex="7" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </td>
                </tr>
            </table>
            <table style="width: 100%;" cellpadding="5px">
                <tbody>
                    <tr>
                        <td>
                            <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img1" alt="Email Setup Data" src="images/panelcollapse.png"
                                        onclick="Display(this,'divTimeZoneData');" runat="server" style="margin-right: 2px;" />Email Setup Data</legend>
                                <div id="divTimeZoneData">
                                    <table style="margin: auto; width: 94%;">
                                        <tbody>
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <asp:GridView ID="gvEmailSetup" TabIndex="6" runat="server" OnRowCommand="gvEmailSetup_RowCommand" AutoGenerateColumns="False" Style="width: 50%; display: none" ShowFooter="false">
                                                        <Columns>
                                                            <asp:BoundField HeaderText="#" Visible="false" />
                                                            <asp:TemplateField SortExpression="status" HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="lnkEdit" runat="server" ImageUrl="~/images/Edit2.gif" ToolTip="Edit" Width="15PX"></asp:ImageButton>&nbsp;
                                                                <asp:ImageButton ID="lnkAudit" runat="server" ImageUrl="~/images/audit.png" ToolTip="Audit" Width="15PX" OnClientClick="AudtiTrail(this); return false;"></asp:ImageButton>&nbsp;
                                                                <asp:ImageButton ID="lnkDelete" runat="server" ImageUrl="~/images/i_delete.gif" ToolTip="InActive"
                                                                    OnClientClick='<%# Eval("iEmailSetupId", "return ShowModalPopup({0})")%>' Width="15PX"></asp:ImageButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="SiteID" HeaderText="Site Id" />
                                                            <asp:BoundField DataField="vOperation" HeaderText="Operation" />
                                                            <asp:BoundField DataField="vUserTypeName" HeaderText="Profile" />
                                                            <%--<asp:BoundField DataField="vUserName" HeaderText="UserName" />--%>
                                                            <asp:BoundField DataField="vEmailId" HeaderText="Email Id" />
                                                            <asp:BoundField DataField="ChangeOn" HeaderText="Modify By" />
                                                            <%--<asp:BoundField DataField="dModifyOn" HeaderText="Modify By"/>                     --%>
                                                            <asp:BoundField DataField="iEmailSetupId" HeaderText="iEmailSetupId" />
                                                            <asp:BoundField DataField="cStatusIndi" HeaderText="cStatusIndi">
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                </tbody>
            </table>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnclose" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <button id="btnRemarks" runat="server" style="display: none;" />
    <cc1:ModalPopupExtender ID="mdlRemarks" runat="server" PopupControlID="divRemarks"
        BackgroundCssClass="modalBackground" BehaviorID="mdlRemarks" CancelControlID="btnRemarksCancel"
        TargetControlID="btnRemarks">
    </cc1:ModalPopupExtender>

    <div id="divRemarks" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 32%; height: auto; max-height: 45%; min-height: auto;">
        <table cellpadding="2" cellspacing="2" width="100%">
            <tr>
                <td class="LabelText" style="text-align: left !important; font-size: 12px !important; width: 97%;"><b>EmailSetup Remark for Delete </b>
                </td>
            </tr>
            <tr>
                <td>
                    <hr />
                </td>
            </tr>
            <tr>
                <td class="LabelText" style="text-align: left !important;">Enter Remarks:*
                </td>
            </tr>
            <tr>
                <td style="text-align: left !important;">
                    <asp:TextBox ID="txtRemarks_delete" runat="server" TextMode="MultiLine" Rows="5" Height="60px"
                        Width="470px" TabIndex="55"></asp:TextBox>
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
                    <asp:Button ID="btnRemarksUpdate" runat="server" Text="Save" CssClass="btn btnsave" Style="font-size: 12px !important;" OnClick="btnRemarksUpdate_Click" OnClientClick="return ValidateInactiveRemarks();" />
                    <asp:Button ID="btnRemarksCancel" runat="server" Text="Cancel" CssClass="btn btncancel"
                        Style="font-size: 12px !important;" />
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnEmailSetupNO" />
    <div>
        <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpEmailsetupAuditTrail" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div id="dvEmailSetUpAudiTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
                                <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                    <tr>
                                        <td id="Td2" class="LabelText" colspan="6" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 97%;">
                                            <asp:Label ID="lblRamge" runat="server" Text="Audit Trail Information"></asp:Label>
                                        </td>
                                        <td style="text-align: right; width: 3%" nowrap="nowrap" colspan="5">
                                            <img id="imgEmailsetupAuditTrail" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Label" nowrap="nowrap" style="text-align: right;">Site Id :
                                        </td>
                                        <td class="Label" nowrap="nowrap" style="text-align: left;">
                                            <asp:TextBox ID="txtSiteID" runat="server"></asp:TextBox>
                                        </td>

                                        <td class="Label" nowrap="nowrap" style="text-align: right;">&nbsp; &nbsp; Opretion :
                                        </td>
                                        <td class="Label" nowrap="nowrap" style="text-align: left;">
                                            <asp:TextBox ID="txtOpretion" runat="server"></asp:TextBox>
                                        </td>

                                        <td class="Label" nowrap="nowrap" style="text-align: right;">&nbsp; &nbsp; Profile :
                                        </td>
                                        <td class="Label" nowrap="nowrap" style="text-align: left;">
                                            <asp:TextBox ID="txtprofile" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <table align="center" border="0" cellpadding="2" cellspacing="2" width="100%">
                                                <tr>
                                                    <td>
                                                        <table id="tblEmailSetUpAudit" class="tblAudit" width="100%" style="background-color: aliceblue;"></table>
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
    <cc1:ModalPopupExtender ID="MPE_EmailSetupHistory" runat="server" PopupControlID="dvEmailSetUpAudiTrail" BehaviorID="MPE_EmailSetupHistory"
        PopupDragHandleControlID="LbldRandomizationDtl" BackgroundCssClass="modalBackground" CancelControlID="imgEmailsetupAuditTrail"
        TargetControlID="btn3">
    </cc1:ModalPopupExtender>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/scrollablegrid.js"></script>

    <script type="text/javascript" language="javascript">

       <%-- function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }--%>

        function ClientPopulatedstudy(sender, e) {
            ProjectClientShowing('AutoCompleteExtender2', $get('<%= txtstudy.ClientId %>'));
        }

      <%--  function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
          iFlag = 0;
      }--%>

        function OnSelectedstudy(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtstudy.Clientid %>'),
             $get('<%= HParentProjectId.Clientid %>'), document.getElementById('<%= btnstudy.ClientId %>'));
            iFlag = 0;
        }
        
        function CheckBoxListSelection(tableName, chk) {
            var checkBoxes = tableName.getElementsByTagName('input');
            for (i = 0; i < checkBoxes.length; i++) {
                if (checkBoxes[i].type == 'checkbox' || checkBoxes[i].type == 'CHECKBOX') {
                    checkBoxes[i].checked = chk.checked;
                    //if(checkBoxes[i].checked == true){
                    //    $('input[type=checkbox]').attr('disabled', true);
                    //}
                }
            }
        }

        function SetCheckUncheckAll(objTable, chkAll) {
            var i = 0;
            var checkBoxes = objTable.getElementsByTagName('input');
            for (i = 0; i < checkBoxes.length; i++) {
                if (checkBoxes[i].type == 'checkbox' || checkBoxes[i].type == 'CHECKBOX') {
                    if (checkBoxes[i].checked == false) {
                        chkAll.checked = false;
                        return;
                    }
                }
            }
            if (i == checkBoxes.length) {
                chkAll.checked = true;
            }
        }

        function HideEmailSetupDetails() {
            $('#<%= img1.ClientID%>').click();
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

      function UIgvEmailSetup() {
          $('#<%= gvEmailSetup.ClientID%>').removeAttr('style', 'display:block');
          $('#tdFieldSetBox').removeAttr('style', 'display:block');
          setTimeout(function () {
              oTab = $('#<%= gvEmailSetup.ClientID%>').prepend($('<thead>').append($('#<%= gvEmailSetup.ClientID%> tr:first'))).dataTable({
                  "bJQueryUI": true,
                  "sPaginationType": "full_numbers",
                  "bLengthChange": true,
                  "iDisplayLength": 10,
                  "bProcessing": true,
                  //"aaSorting": [[5, "desc"]],
                  "sScrollY": "250px",
                  "sScrollX": "100%",
                  "bScrollCollapse": true,
                  aLengthMenu: [
                      [5, 10, 25, 50, 100, -1],
                      [5, 10, 25, 50, 100, "All"]
                  ],
              });
          }, 100);
          return false;
      }
      function pageLoad() {
          $('.timepicker').timepicker({ 'scrollDefaultNow': true });
      }

      //function studyspacific() {
      //    if (document.getElementById("chkSelectAllStudy").checked) {
      //        document.getElementById("chklststudy").disabled = true;
      //    }
      //}

      function Validation(id) {
          var CheckBoxList = document.getElementById('<%=chklstUser.ClientID%>');
          var chkStudy = document.getElementById('<%=chkSelectAllStudy.Clientid%>');
          if (CheckBoxList != null && typeof (CheckBoxList) != 'undefined') {
              var CheckBoxes = CheckBoxList.getElementsByTagName('input');
              var CountUser = 0;
              for (i = 0; i < CheckBoxes.length; i++) {
                  if (CheckBoxes[i].type == 'checkbox' || checkBoxes[i].type == 'CHECKBOX') {
                      if (CheckBoxes[i].checked == true) {
                          CountUser += 1;
                      }
                  }
              }
          }

          if (document.getElementById("<%=txtstudy.ClientID%>").value.trim() == "") {
                msgalert("Please Enter Study Id !");
                document.getElementById('<%=txtstudy.ClientID%>').focus();
                    return false;
                }
                else if (document.getElementById("<%=HParentProjectId.ClientID%>").value.trim() == "") {
                     document.getElementById('<%=txtstudy.ClientID%>').value = '';
                    msgalert("Please Select Study Id from the list Only !");
                    document.getElementById('<%=txtstudy.ClientID%>').focus();
                    return false;
                }

          //Added by Bhargav Thaker Started 28Feb2023
          if($('#' + id).val().toUpperCase() != "UPDATE")
          {
              let uncheckcnt = 0;
            var tblLenth = $("#ctl00_CPHLAMBDA_chklststudy tr").length;
            $("#ctl00_CPHLAMBDA_chklststudy tr td input").each(function () {
                var sThisVal = (this.checked ? "1" : "0");
                if (sThisVal == "0") {
                    uncheckcnt++;
                }
            });

            if (uncheckcnt == tblLenth) {
                msgalert("Please Select Atleast One Site !");
                document.getElementById('<%=txtstudy.ClientID%>').focus();
                return false;
            }
          }
          //Added by Bhargav Thaker Ended 28Feb2023

            if (document.getElementById('<%=ddlOperation.ClientID%>').selectedIndex == 0) {
              msgalert('Please Select Operation !');
              document.getElementById('<%=ddlOperation.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=ddlProfile.clientid()%>').selectedIndex == 0) {
                 msgalert('Please Select Profile !');
                 document.getElementById('<%=ddlProfile.ClientID%>').focus();
                return false;
            }
            else if (CountUser <= 0) {
                msgalert("Please select user !");
                document.getElementById('<%=chklstUser.ClientID%>').focus();
                return false;
            }

            if (document.getElementById("<%=txtMailId.ClientID%>").value.trim() == "") {
              msgalert("Please Enter Email Id !");
              document.getElementById('<%=txtMailId.ClientID%>').focus();
                    return false;
                }

                if ($('#' + id).val().toUpperCase() == "UPDATE") {
                    if (document.getElementById("<%=txtRemark.ClientID%>").value.trim() == "") {
                  msgalert('Please Enter Remarks !')
                  document.getElementById('<%=txtRemark.ClientID%>').focus();
                  return false;
              }
          }
          return true;
      }

      function Validate(evt) {
          var browser = navigator.appName;
          var charCode = 0;
          if (browser == 'Microsoft Internet Explorer')
              charCode = evt.keyCode;
          else
              charCode = evt.which;

          if (evt.shiftKey == true)
          { return false; }

          if ((charCode == 190) || (charCode == 110) || (charCode == 20) || (charCode == 9) || (charCode == 46) || (charCode == 8))
          { return false; }

          if ((charCode < 48 || charCode > 57) && (charCode < 96 || charCode > 105))
          { return false; }

          return false;
      }


      //function getData() {
      //    var WorkspaceID = $('input[id$=HProjectId]').val();
      //    $.ajax({
      //        type: "post",
      //        url: "frmchildWorkspaceSite.aspx/LockImpact",
      //        data: '{"WorkspaceID":"' + WorkspaceID + '"}',
      //        contentType: "application/json; charset=utf-8",
      //        datatype: JSON,
      //        async: false,
      //        dataType: "json",
      //        success: function (data) {
      //            if (data.d == "L") {
      //                msgalert("Project is locked !");
      //                $("#ctl00_CPHLAMBDA_txtProject").attr("value", "");
      //            } else {
      //                FlagSetProject = "True";
      //                return true;
      //            }
      //        },
      //        failure: function (response) {
      //            msgalert(response.d);
      //        },
      //        error: function (response) {
      //            msgalert(response.d);
      //        }
      //    });
      //    return true;
      //}

      function ValidateInactiveRemarks() {
          if (document.getElementById("<%=txtRemarks_delete.ClientID%>").value.trim() == "") {
        msgalert("Please Enter Remarks !");
        return false;
    }
    return true;
}
function ShowModalPopup(id) {
    $("#<%= hdnEmailSetupNO.ClientID %>").val(id);
    msgConfirmDeleteAlert(null, "Are You Sure You Want To Delete EmailSetup?", function (isConfirmed) {
        if (isConfirmed) {
            $('#<%= txtRemarks_delete.ClientID%>').val('');
            $find('mdlRemarks').show();
            return true;
        } else {
            return false;
        }
    });
    return false;
}

function SelectAllFields() {
    var chkSelectAll = false //true Modify by Bhargav Thaker 28Feb2023

    //if condition added by Bhargav Thaker 03Mar2023
    if ($("#ctl00_CPHLAMBDA_btnSave").val().toUpperCase() == "UPDATE") {
        chkSelectAll = true;
    }
    var chklst = document.getElementById('<%=chklststudy.Clientid%>');
    var chklstAllSelect = document.getElementById('<%=chkSelectAllStudy.Clientid%>');
            var chks;
            var result = false;
            var i;
            if (chklst != null && typeof (chklst) != 'undefined') {
                chks = chklst.getElementsByTagName('input');
                if (chkSelectAll == true) {
                    for (i = 0; i < chks.length; i++) {
                        chks[i].checked = true;
                        chklstAllSelect.checked = true;
                    }
                }
                else {
                    for (i = 0; i < chks.length; i++) {
                        chks[i].checked = false;
                        chklstAllSelect.checked = false;
                    }
                }
            }
            return false;
        }

        function AudtiTrail(e) {
            var iEmailSetupId = $("#" + e.id).attr("iEmailSetupId");

            if (iEmailSetupId != "") {
                $.ajax({
                    type: "post",
                    url: "frmEmailSetupMst.aspx/AuditTrail",
                    data: '{"iEmailSetupId":"' + iEmailSetupId + '"}',
                    contentType: "application/json; charset=utf-8",
                    datatype: JSON,
                    async: false,
                    success: function (data) {
                        $('#tblEmailSetUpAudit').attr("IsTable", "has");
                        var aaDataSet = [];
                        if (data.d == "" && data.d == null) {
                            alert("No Data Available")
                        }
                        else {
                            data = JSON.parse(data.d);
                            for (var Row = 0; Row < data.length; Row++) {
                                var InDataSet = [];
                                InDataSet.push(data[Row].SrNo, data[Row].EmailId, data[Row].Remarks, data[Row].ModifyBy, data[Row].ModifyOn);
                                aaDataSet.push(InDataSet);
                                document.getElementById("ctl00_CPHLAMBDA_txtSiteID").value = data[Row].SiteId;
                                document.getElementById("ctl00_CPHLAMBDA_txtOpretion").value = data[Row].Operation;
                                document.getElementById("ctl00_CPHLAMBDA_txtprofile").value = data[Row].Profile;
                            }
                            $("#ctl00_CPHLAMBDA_txtSiteID").attr("disabled", "disabled");
                            $("#ctl00_CPHLAMBDA_txtOpretion").attr("disabled", "disabled");
                            $("#ctl00_CPHLAMBDA_txtprofile").attr("disabled", "disabled");

                            if ($("#tblEmailSetUpAudit").children().length > 0) {
                                $("#tblEmailSetUpAudit").dataTable().fnDestroy();
                            }

                            oTable = $('#tblEmailSetUpAudit').prepend($('<thead>').append($('#tblEmailSetUpAudit tr:first'))).dataTable({
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
                                        "sTitle": "Sr. No",
                                    },
                                    //{ "sTitle": "Site Id" },
                                    //{ "sTitle": "Operation" },
                                    //{ "sTitle": "Profile" },
                                    { "sTitle": "EmailId" },
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
                            $find('MPE_EmailSetupHistory').show();
                            $('.dataTables_filter input').addClass('textBox');
                        }
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
