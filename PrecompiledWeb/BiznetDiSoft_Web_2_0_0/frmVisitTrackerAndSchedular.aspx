<%@ page title="" language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmVisitTrackerAndSchedular, App_Web_5xoe1jh1" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <div id="divMainContent" runat="server" style="width: 100%">
         
        <div id="divLeft" runat="server" style="width: 30%; float: left; padding-top: 3%; height: 560px;">
  <fieldset class="FieldSetBox" style="display: block; width: 98%; text-align: left; border: #aaaaaa 1px solid;">
                    <legend class="LegendText" style="color: Black; font-size: 12px">
                        <img id="img2" alt="Activity Group Details" src="images/panelcollapse.png"
                            onclick="Display(this,'divActivityDetail');" runat="server" style="margin-right: 2px;" />Project Details</legend>
            <table style="width: 100%;">
               
                <tr>
                    <td>
                        <asp:UpdatePanel ID="upPeriod" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div id="divDate" runat="server">
                                    <asp:RadioButtonList ID="rbtdateselecton" runat="server" RepeatDirection="Horizontal" Style="margin: auto" AutoPostBack="true">
                                        <asp:ListItem Value="S">Screening Date</asp:ListItem>
                                        <asp:ListItem Value="R">Randomization Date</asp:ListItem>
                                        <asp:ListItem Value="N">Not applicable/Same</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div id="divshowhideadd" runat="server" style="width: 30%; float: left; padding-top: 3%; height: 560px;">
                                    <table style="width: 100%; border: 1px solid darkgray; border-radius: 5px;border-collapse:initial" align="left" border="0" cellspacing="10px">
                                        <tr>
                                            <td class="Label" style="white-space: nowrap; text-align: right; width: 10%">Project No *:
                                            
                                            </td>

                                            <td>
                                                <span style="font-weight: normal">
                                                    <asp:TextBox ID="txtProject" runat="server" CssClass="textBox" Width="250px" onkeydown="return (event.keyCode!=13)" />
                                                </span>
                                                <asp:Button ID="btnSetProject" runat="server" Style="display: none" Text=" Project" />
                                                <asp:HiddenField ID="HProjectId" runat="server" />
                                                <asp:HiddenField ID="HClientName" runat="server" />
                                                <asp:HiddenField ID="HProjectName" runat="server" />
                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtenderadd" runat="server" BehaviorID="AutoCompleteExtenderadd"
                                                    CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                    CompletionListElementID="pnlProjectlist" CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                                    OnClientShowing="ClientPopulated" ServiceMethod="GetMyProjectCompletionListParentOnly"
                                                    ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True">
                                                </cc1:AutoCompleteExtender>
                                                <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" />

                                            </td>

                                        </tr>

                                        <div runat="server" id="divAdd">
                                            <tr>

                                                <td class="Label" style="white-space: nowrap; text-align: right; width: 10%">Parent Activity *: </td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlParentActivity" name="ParentActivity" AutoPostBack="true" CssClass="dropDownList" Width="250px">
                                                        <asp:ListItem Value="0">Select visit/Parent activity</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Label" style="white-space: nowrap; text-align: right; width: 10%">Visit Actual Days *: </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtActual" Width="250px" CssClass="textBox"></asp:TextBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="Label" style="white-space: nowrap; text-align: right; width: 10%">Visit Deviation Negative(-) : </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtNegative" Width="250px" CssClass="textBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Label" style="white-space: nowrap; text-align: right; width: 10%">Visit Deviation Positive(+) : </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtPositive" Width="250px" CssClass="textBox"></asp:TextBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="Label" style="white-space: nowrap; text-align: right; width: 10%"></td>
                                                <td>
                                                    <asp:Button runat="server" CssClass="btn btnadd" ID="btnAdd" OnClientClick="return AddValidation();" OnClick="btnAdd_Click" Text="Add" />
                                                    <asp:Button runat="server" CssClass="btn btncancel" ID="btnClear" OnClientClick="return ClearValidation();" OnClick="btnClear_Click" Text="Clear" />
                                                    <asp:Button runat="server" CssClass="btn btnexit" ID="btnExit" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this); " Text="Exit" />
                                                </td>
                                            </tr>
                                        </div>
                                    </table>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            
            </table>
      </fieldset>
        </div>
             
        <div id="divRight" runat="server" style="width: 64%; margin-left: 6%; float: left; padding-top: 4%;">

            

            <asp:UpdatePanel ID="upGvDeviation" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="lblNote" runat="server" Style="margin-right: 61%; color: red;"></asp:Label>
                    <p/>
                    <asp:HiddenField ID="HfVisitNo" runat="server" />   
                    <asp:HiddenField ID="HfInodeid" runat="server" />                    
                    <asp:GridView ID="gvDeviation" runat="server" AutoGenerateColumns="false" >
                        <Columns>
                            <asp:TemplateField HeaderText="Tracker" HeaderStyle-Width="50px" Visible="false">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtTracker" Enabled="false" CssClass="textBox" Width="70px" Text='<%# Eval("nTrackerNo")%>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" AutoPostBack="true" ID="chkEdit" ToolTip='<%# Eval("nTrackerNo")%>' OnCheckedChanged="chkEdit_CheckedChanged" onchange="return ValidationCheckBox();" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Project Name" HeaderStyle-Width="50px">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txt" Enabled="false" CssClass="textBox" Width="70px" Text='<%# Eval("vProjectNo")%>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Visit No" HeaderStyle-Width="30px" Visible="false">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtvisitNo" Enabled="false" CssClass="textBox" Width="50px" Text='<%# "Visit " + Eval("iVisitNo").ToString() %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <%--<asp:TemplateField HeaderText="Activity" HeaderStyle-Width="30px">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtactivity" Enabled="false" ToolTip='<%# Eval("vNodeDisplayName").ToString() %>' CssClass="textBox" Text='<%# Eval("vNodeDisplayName").ToString() %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Activity" HeaderStyle-Width="30px">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlgvChildActivity" Width="200px" runat="server" CssClass="dropDownList"  Orientation="Horizontal">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Actual Days" HeaderStyle-Width="30px">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtActualDays" CssClass="textBox" Width="30px" Text='<%# Eval("iActualDays")%>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Deviation Negative" HeaderStyle-Width="30px">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtDeviationNegative" CssClass="textBox" Width="30px" Text='<%# Eval("iDeviationNegative") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Deviation Positive" HeaderStyle-Width="30px">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtDeviationPositive" CssClass="textBox" Width="30px" Text='<%# Eval("iDeviationPositive")%>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Update Field" HeaderStyle-Width="30px">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lnk" CommandName="MyValue" ToolTip="Update" Text="Update"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Audit Trail" HeaderStyle-Width="30px">
                                <ItemTemplate>
                                    <img src="Images/audit.png" onmouseover="this.style.cursor='pointer';" onclick="AuditTrail(this.id);" id='<%# Eval("nTrackerNo")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <button id="btnRemarls" runat="server" style="display: none;" />
    <cc1:ModalPopupExtender ID="ModalRemarks" runat="server" PopupControlID="divRemarks"
        BackgroundCssClass="modalBackground" TargetControlID="btnRemarls" BehaviorID="ModalRemarks"
        CancelControlID="CancelRemarks">
    </cc1:ModalPopupExtender>

    <asp:UpdatePanel ID="Upmodal" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="ModalBackGround" runat="server" class="divModalBackGround" style="display: none; width: 100% !Important;"></div>

            <div id="divRemarks" runat="server" class="centerModalPopup" style="display: none; left: 30%; width: 32%; position: absolute; top: 525px; border: 1px solid;">
                <div>
                    <table style="width: 90%; margin: auto;">
                        <tr>
                            <td colspan="2" class="LabelText" style="text-align: center !important; color: white; font-size: 16px !important; width: 97%;"><b>Enter Remarks</b>

                            </td>

                            <td style="text-align: center; height: 22px;" valign="top">
                                 <img id="CancelRemarks" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: -17px;" title="Close" onkeypress="return validatecancel();" />
                            </td>
                        </tr>
                    </table>
                    <hr />
                </div>
                <table style="margin: 10px 10px 10px 10px;">
                    <tr>
                        <td colspan="2">
                            <center>
                                <asp:Label runat="server" ID="lblChangeStatus"></asp:Label>
                            </center>
                        </td>
                    </tr>

                    <tr style="margin: 10px 10px 10px 10px;">

                        <td style="white-space: nowrap; text-align: right; width: 120px;" class="Label">Remarks*:
                        </td>
                        <td class="Label" align="right">
                            <asp:TextBox ID="txtRemarks" runat="Server" TextMode="MultiLine" onkeyup="characterlimit(this)" Text="" CssClass="textbox"> </asp:TextBox>
                            <asp:Label runat="server" Text="*" ForeColor="Red" ID="lblError" Style="display: none;"></asp:Label>
                        </td>
                    </tr>
                </table>                
                <center>
                    <table>
                        <hr />
                        <tr>
                            <td>
                                <center>
                                    <asp:Button ID="btnSaveRemarks" runat="server" CssClass="btn btnsave" OnClientClick="return validationremarks();" ValidationGroup="validate" Text="Save"/>
                                    <asp:Button ID="btnCancel" OnClientClick="return validatecancel();" runat="server" CssClass="btn btncancel" Text="Cancel" />
                                </center>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSaveRemarks" />
            <%--<asp:AsyncPostBackTrigger ControlID="btnSaveRemarks" EventName="Click" />--%>
            <asp:PostBackTrigger ControlID="btnCancel"  />
        </Triggers>
    </asp:UpdatePanel>
    <button id="btnAuditTrail" runat="server" style="display: none;" />

    <img alt="" runat="server" id="ImgLogo" style="display: none;" src="images/lambda_logo.jpg" enableviewstate="false" />

    <cc1:ModalPopupExtender ID="modalpopupaudittrail" runat="server" PopupControlID="dvAudiTrail"
        BackgroundCssClass="modalBackground" TargetControlID="btnAuditTrail" BehaviorID="modalpopupaudittrail"
        CancelControlID="imgAuditTrail">
    </cc1:ModalPopupExtender>

    <div id="dvAudiTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
        <table border="0" cellpadding="2" cellspacing="2" width="100%">
            <tr>
                <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 80%;">Audit Trail</td>
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
                                <table id="tblAudit" class="tblAudit" width="100%" style="background-color: aliceblue; word-wrap: break-word;"></table>
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
    <script src="Script/jquery-1.10.9.dataTables.min.js" type="text/javascript"></script>
    <script src="Script/jquery-ui-1.10.4.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="script/jquery.datatables.min.js"></script>
    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript">
        function HideActivityGroupDetails() {
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

        function pageLoad() {

            datatablestru();

        }
        function datatablestru() {
            if ($get('<%= gvDeviation.ClientID%>') != null && $get('<%= gvDeviation.ClientID %>_wrapper') == null) {
                
                if ($('#<%= gvDeviation.ClientID%>')) {
                    $('[id$="' + '<%= gvDeviation.ClientID%>' + '"] tbody tr').length < 7 ? scroll = "25%" : scroll = "193px";
                    $('#<%= gvDeviation.ClientID%>').prepend($('<thead>').append($('#<%= gvDeviation.ClientID%> tr:first')))
                           .dataTable({
                               "sScrollY": scroll,
                               "sScrollX": '70%',
                               "bJQueryUI": true,
                               "bPaginate": false,
                               "bFooter": false,
                               "bHeader": false,
                               "bAutoWidth": false,
                               "Width": '65%',
                               "bSort": false,
                           });
                }
                $("#<%= gvDeviation.ClientID %>_wrapper").css("display", "inline-block");
                //$(".dataTables_scrollBody").css("height", "100px");
            }
        }
        function ValidationCheckBox() {
            var count = 0;
            for (i = 0; i < document.forms[0].elements.length; i++) {
                if ((document.forms[0].elements[i].type == 'checkbox') &&
                    (document.forms[0].elements[i].name.indexOf('chkEdit') > -1)) {
                    if (document.forms[0].elements[i].checked == true) {
                        count++;
                        if (count > 1) {
                            document.forms[0].elements[i].checked = false;
                            break;
                        }
                    }
                }
            }
            if (count > 1) {
                msgalert('Please select only one checkbox !');
                return false;
            }

            else { return true; }
        }
        function validationremarks() {
            if (document.getElementById('<%= txtRemarks.ClientID%>').value.trim() == "") {
                msgalert("Please Enter Remarks !");
                return false
            }
            return true
        }
        function validatecancel() {
            document.getElementById('<%= txtRemarks.ClientId %>').value = '';
            return true

        }
        function characterlimit(id) {

            var text = id.value
            var textLength = text.length;
            if (textLength > 100) {
                $(id).val(text.substring(0, (100)));
                msgalert("Only 100 characters are allowed !");

               // $(id).val(text.substring(0, (100)));
                //alert("Only 100 characters are allowed");
                //document.getElementById('<%= txtRemarks.ClientId %>').value = '';
                //$find('ModalRemarks').hide()
                //return false;
            }
            else {
                return true;
            }

        }
        function OnSelected(sender, e) {

            ProjectOnItemSelectedForMsrLog(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'), $get('<%=HClientName.clientid %>'), $get('<%=HProjectName.clientid %>'));

        }

        function ClientPopulated(sender, e) {
            ProjectClientShowingSchema('AutoCompleteExtenderadd', $get('<%= txtProject.ClientId %>'));
        }
        function AddValidation() {
           
            if (document.getElementById("<%=rbtdateselecton.ClientID%>").style.display != 'none') {
                var RB1 = document.getElementById("<%=rbtdateselecton.ClientID%>");
                var radio = RB1.getElementsByTagName("input");
                var isChecked = false;
                for (var i = 0; i < radio.length; i++) {
                    if (radio[i].checked) {
                        isChecked = true;
                        break;
                    }
                }
                if (!isChecked) {
                    msgalert("Please Select Date type !");
                    return false
                }

            }


            if ((document.getElementById('<%= HProjectId.ClientID%>').value.trim().length <= 9)) {
                msgalert('Please Enter Valid Project No !');
                return false;
            }
            if ((document.getElementById('<%= txtProject.ClientID%>').value.trim() == "")) {
                msgalert('Please Enter Valid Project No !');
                return false;
            }

            if (document.getElementById('ctl00_CPHLAMBDA_ddlParentActivity').selectedIndex == 0) {
                msgalert('Please Select Parent Activity first !');
                return false
            }


            if ((isNaN(document.getElementById('<%= txtActual.ClientID%>').value)) || document.getElementById('<%= txtActual.ClientID%>').value == '') {
                msgalert('Please Enter Valid Actual Days !');
                return false
            }
          

            return true
        }
        function ClearValidation() {
            document.getElementById('<%= txtProject.ClientID%>').value = ''
            document.getElementById('<%= txtActual.ClientID%>').value = ''
            document.getElementById('<%= txtNegative.ClientID%>').value = ''
            document.getElementById('<%= txtPositive.ClientID%>').value = ''
            document.getElementById('ctl00_CPHLAMBDA_ddlParentActivity').selectedIndex = 0
            return true
        }

        function Exitvalidation() {
            window.location = "frmMainPage.aspx";
        }
              
        function AuditTrail(id) {
            var WorkSpaceDeviationId = id

            $.ajax({
                type: "post",
                url: "frmVisitTrackerAndSchedular.aspx/AuditTrailForVisitTracker",
                data: '{"WorkSpaceDeviationId":"' + WorkSpaceDeviationId + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                success: function (data) {
                    $('#tblAudit').attr("IsTable", "has");
                    var aaDataSet = [];
                    var RowId
                    if (data.d != "" && data.d != null) {
                        data = JSON.parse(data.d);
                        for (var Row = 0; Row < data.length; Row++) {
                            var InDataSet = [];
                            RowId = Row + 1
                            InDataSet.push(data[Row].vProjectNo, data[Row].iVisitNo, data[Row].vNodeDisplayName, data[Row].iActualDays, data[Row].iDeviationNegative, data[Row].iDeviationPositive, data[Row].vRemarks, data[Row].iModifyBy, data[Row].dModifyOn);
                            aaDataSet.push(InDataSet);
                        }

                        if ($("#tblAudit").children().length > 0) {
                            $("#tblAudit").dataTable().fnDestroy();
                        }
                        oTable=    $('#tblAudit').prepend($('<thead>').append($('#tblAudit tr:first'))).dataTable({
                            "bStateSave": false,
                            "bPaginate": false,
                            "sPaginationType": "full_numbers",
                            "sDom": '<fr>t<p>',
                            "iDisplayLength": 8,
                            "bSort": false,
                            "bFilter": false,
                            "bDestory": true,
                            "bRetrieve": true,
                            "aaData": aaDataSet,
                            "aoColumns": [
                                { "sTitle": "Project No." },
                                { "sTitle": "Visit No." },
                                { "sTitle": "Activity" },
                                { "sTitle": "Actual Days" },
                                { "sTitle": "Deviation Negative" },
                                { "sTitle": "Deviation Positive" },
                                { "sTitle": "Remarks" },
                                { "sTitle": "Modify By" },
                                { "sTitle": "Modify On" },
                            ],
                            "oLanguage": {
                                "sEmptyTable": "No Record Found",
                            },
                            "fnCreatedRow": function (nRow, aData, iDataIndex) {    // added tooltip
                                $.each(aData, function (index) {
                                    if (aData[index] != null) {
                                        if (aData[index].length > 40) {
                                            var abc = aData[index];
                                            var lmn = abc.substring(0, 40);
                                            $('td:eq(' + index + ')', nRow).html(""); 
                                            $('td:eq(' + index + ')', nRow).append("<input type='image' id='imgExpand_" + iDataIndex + "' name='imgExpand$" + iDataIndex + "' title ='" + aData[index] + "' src='images/question.gif' disabled style='border-width:0px; padding-right:5px; '>" + lmn);
                                            abc = "";
                                            lmn = "";
                                        }
                                    }
                                });
                            },
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

                /* Apply the tooltips */
               
            });
            oTable.$('tr').tooltip({
                "delay": 0,
                "track": true,
                "fade": 250
            });
        }

    </script>
</asp:Content>
