<%@ Page Title="Information Management" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmNoticeMst.aspx.vb" Inherits="frmNoticeMst" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="FreeTextBox" Namespace="FreeTextBoxControls" TagPrefix="FTB" %>



<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server"> 
    <link rel="stylesheet" type="text/css" href="App_Themes/jquery.multiselect.css" />
    <link rel="stylesheet" type="text/css" href="App_Themes/multiple-select.css" />
    <link rel="stylesheet" type="text/css" href="App_Themes/jquery.multiselect.css" />
    <link href="App_Themes/StyleBlue/UI_Theme/jquery-ui.css" rel="stylesheet" type="text/css" />


    <style type="text/css">
        .ajax__calendar_container {
            z-index:1;
        }
</style>

    <asp:UpdatePanel ID="Up_View" runat="server">
        <ContentTemplate>
            <table style="width: 100%;" cellpadding="5px">
                <tbody>
                     <tr>
                        <td>
                            <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="ImgPanelNoticeDetails" alt="Operation Details" src="images/panelcollapse.png"
                                        onclick="Display(this,'divOperationDetail');" runat="server" style="margin-right: 2px;" />Information Management Details</legend>
                                <div id="divOperationDetail">
                                    <table width="98%">
                                         <tr>
                                            <td class="Label" style="text-align: right; width:27%;">
                                                Project Name :
                                            </td>
                                         <td style="text-align: left; width: 20%;">
                                                    <asp:TextBox ID="txtProject" runat="server" CssClass="textBox" TabIndex="1" Style="width: 50%;" />
                                                    <asp:Button Style="display: none" ID="btnSetProject" runat="server" Text=" Project" CssClass="btn btnnew" />
                                                    <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                                                        TargetControlID="txtProject" ServicePath="AutoComplete.asmx" ServiceMethod="GetMyProjectCompletionList"
                                                        OnClientShowing="ClientFromPopulated" OnClientItemSelected="OnFromSelected" MinimumPrefixLength="1"
                                                        CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                        CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1" CompletionListElementID="pnlfromProject">
                                                    </cc1:AutoCompleteExtender>
                                                    <asp:Panel ID="pnlfromProject" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden;" />
                                                </td>
                                        </tr>
                                        <tr>
                                            <td class="Label" style="text-align: right; width:27%;">
                                                User Profile * :
                                            </td>
                                            <td style="text-align: left; width:25%;">
                                                <asp:DropDownList ID="ddlProfile" runat="server" Style="width: 20%;background:none !important;border:1px black solid !important"
                                                   CssClass="dropDownList" AutoPostBack="false" onchange=" fnProfileName();"></asp:DropDownList>
                                            </td>
                                            
                                        </tr>
                                        <tr>
                                            <td class="Label" style="text-align: right; width:13%;">
                                                Subject * :
                                            </td>
                                            <td style="text-align: left; width:35%;">
                                                <asp:TextBox ID="txtSub" runat="server" CssClass="textBox" Width="250px" autocomplete="off"></asp:TextBox>
                                            </td>

                                            
                                        </tr>
                                        <tr>
                                            <td class="Label" style="text-align: right;">
                                                Description * :
                                            </td>
                                            <td style="text-align: left;">
                                                <FTB:FreeTextBox ID="Editor1" runat="server" Height="150px" Width="500px">
                                                </FTB:FreeTextBox>
                                            </td>
                                            
                                        </tr>
                                        
                                        <tr>
                                            <td class="Label" style="text-align: right;">
                                                From Date * :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="textBox" Enabled="True"></asp:TextBox>&nbsp;
                                                <%--<img style="cursor: hand; position: static" id="img2" onclick="popUpCalendar(this,document.getElementById('<%= txtFromDate.ClientID%>'),'dd-mmm-yyyy');"
                                                    alt="Select Date" src="images/Calendar_scheduleHS.png" />--%>

                                                <cc1:CalendarExtender ID="CalExtFromDateForNoticeMst" runat="server" TargetControlID="txtFromDate"  
                                                    Format="dd-MMM-yyyy" OnClientDateSelectionChanged="verifyDate">
                                                </cc1:CalendarExtender>
                                            </td>
                                            
                                        </tr>
                                        <tr>
                                            <td class="Label" style="text-align: right;">
                                                To Date *  :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtToDate" runat="server" CssClass="textBox" Enabled="True"></asp:TextBox>&nbsp;
                                                        <%--<img style="cursor: hand; position: static" id="img1" onclick="popUpCalendar(this,document.getElementById('<%= txtToDate.ClientID%>'),'dd-mmm-yyyy');"
                                                            alt="Select Date" src="images/Calendar_scheduleHS.png" />--%>

                                                <cc1:CalendarExtender ID="CalExtToDateForNoticeMst" runat="server" TargetControlID="txtToDate" 
                                                    Format="dd-MMM-yyyy" OnClientDateSelectionChanged="verifyDate">
                                                </cc1:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Label" style="text-align: right;">
                                                Attachment  :
                                            </td>
                                            <td style="text-align: left;">
                                                        <asp:FileUpload ID="FileUpload1" runat="server" CssClass="button" Width="225px" onchange="ListofDocumentSelectedSize(this)" Font-Strikeout="False"></asp:FileUpload>
                                           </td>
                                            
                                        </tr>
                                        <tr>
                                            <td style="white-space: nowrap; text-align: center;" colspan="4">
                                                <asp:HiddenField ID="hdnUserTypeCode" runat="server" Value="" />
                                                <asp:Button ID="btnSave" runat="server" Text="Save" ToolTip="Save" CssClass="btn btnsave"
                                                    OnClientClick="return Validation();" />
                                                <asp:Button ID="btnClear" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="btn btncancel" />
                                                <%--<input id="BtnExit" class="btn btnexit" onclick="window.close();" type="button" value="Close" />--%>
                                                 <asp:Button ID="BtnExit" runat="server" ToolTip="Exit" CausesValidation="False" CssClass="btn btnclose"
                                                            OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this); " Text="Exit" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                </tbody>
            </table>
            <table style="text-align: center; width: 100%; margin-top: 2%;">
            <tbody>
                    <tr>
                        <td>
                            <fieldset class="FieldSetBox" style="display: block; width: 95.5%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="ImgPanelNoticeData" alt="Operation Data" src="images/panelcollapse.png"
                                        onclick="Display(this,'divOperationData');" runat="server" style="margin-right: 2px;" />Information Management Data</legend>
                                <div id="divOperationData">
                                    <table style="margin: auto; width: 85%;">
                                        <tr>
                                            <td>
                                                
                                                <asp:GridView ID="gvnotice" runat="server" Style="width: auto; margin: auto; display: none;"
                                                    AutoGenerateColumns="False" OnRowCommand="gvnotice_RowCommand" OnRowDataBound="gvnotice_RowDataBound"
                                                    ShowFooter="false">
                                                    <Columns>
                                                        
                                                        <asp:BoundField DataFormatString="number" HeaderText="Sr. No">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="nNoticeNo" HeaderText="Notice No">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vUserTypeName" HeaderText="UserProfile">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vSubject" HeaderText="Subject">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <%--<asp:BoundField DataField="vDescription" HeaderText="Description">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>--%>
                                                        <asp:BoundField DataField="dStartDate" HeaderText="From Date">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="dEndDate" HeaderText="To Date">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>

                                                        <asp:TemplateField HeaderText="Edit">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="lnkEdit" runat="server" ImageUrl="~/images/Edit2.gif" ToolTip="Edit" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Delete">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="lnkDelete" runat="server" ImageUrl="~/Images/i_delete.gif" OnClientClick="return ShowModalPopup();"/>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
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
        </ContentTemplate>
        <Triggers>
            <%--<asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />--%>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>

    <div id="myModalRemarks" class="modal" runat="server">
        <!-- Modal content -->
        <div class="modal-content">
            <div class="modal-header" style="text-align: left">
                <img id="Img3" alt="Close1" src="images/close_pop.png" class="close modalCloseImage" title="Close" onclick="ModalPopupClose()" />
                <h3 style="text-align: center">
                    <asp:Label runat="server" ID="modalHeading">Remarks</asp:Label></h3>
            </div>
            <div class="modal-body">
                <table width="100%">
                    <tr>
                        <td align="left" style="font-weight: bold; font-size: small;" colspan="2">
                            <asp:Label runat="server" ID="lblHeading"></asp:Label>
                        </td>
                    </tr>
                    <tr><td></td></tr>
                    <tr>
                        <td align="left" style="font-weight: bold; font-size: small;">Remarks *
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" CssClass="textBox" Height="65px" Width="80%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center" colspan="2">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnRemarkSave" runat="server" CssClass="btn btnnew" Text="OK" Width="105px" />
                                    <input type="button" class="button" title="Close" style="width:105px" onclick="ModalPopupClose();" id="btnRemarksCancel" value="Close" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnRemarkSave" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>


    

    <script src="Script/Validation.js" language="javascript" type="text/javascript"></script>
    <script src="Script/jquery-ui-1.10.4.custom.min.js" type="text/javascript"></script>
    <script src="Script/jquery.multiselect.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript" src="Script/popcalendar.js"></script>
    <script type="text/javascript" language="javascript" src="Script/General.js"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>

    <script type="text/javascript" language="javascript">

        function pageLoad() {
            MultiselectRequired();
            fnApplyProfileName();
        }

        function ClientFromPopulated(sender, e) {
            debugger;
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientID%>'));
           
        }

        function OnFromSelected(sender, e) {
            debugger;
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.ClientID%>'),
            $get('<%= HProjectId.ClientID%>'), document.getElementById('<%= btnSetProject.ClientId %>'));
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

        function MultiselectRequired() {
            $('#ctl00_CPHLAMBDA_ddlPanelName').multiselect({
                includeSelectAllOption: true
            });
            $('#ctl00_CPHLAMBDA_ddlProfile').multiselect({
                includeSelectAllOption: true
            });
        }

        function SelectALL(chk) {
            if (document.getElementById(chk) == null) {
                return false;
            }
            ochk = document.getElementById(chk);
            var n = ochk.rows.length;
            var i = 0;
            for (k = 0; k <= n - 1; k++) {
                ConCatStr = chk + "_" + i;
                document.getElementById(ConCatStr).checked = true;
                i = i + 1;
            }
        }

        function ClearALL(chk) {
            if (document.getElementById(chk) == null) {
                return false;
            }
            ochk = document.getElementById(chk);
            var n = ochk.rows.length;
            var i = 0;
            for (k = 0; k <= n - 1; k++) {
                ConCatStr = chk + "_" + i;
                document.getElementById(ConCatStr).checked = false;
                i = i + 1;
            }
        }

        //For Profile
        function fnProfileName() {
            var ProfileName = [];
            document.getElementById('<%= hdnUserTypeCode.ClientID%>').value = "";
            for (i = 0; i < $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlProfile']:checked").length ; i++) {
                ProfileName.push("" + $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlProfile']:checked").eq(i).attr("value") + "");
            }
            document.getElementById('<%= hdnUserTypeCode.ClientID%>').value = ProfileName;
            return true;
        }

        function UIgvNotice() {
            $('#<%= ImgPanelNoticeDetails.ClientID%>').click();
            $('#<%= gvnotice.ClientID%>').removeAttr('style', 'display:block');
            oTab = $('#<%= gvnotice.ClientID%>').prepend($('<thead>').append($('#<%= gvnotice.ClientID%> tr:first'))).dataTable({
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


        var ProfileName = [];
        function fnApplyProfileName() {
            $("#<%= ddlProfile.ClientID%>").multiselect({
                noneSelectedText: "--Select Profile Name--",
                click: function (event, ui) {
                    if (ui.checked == true)
                        ProfileName.push("'" + ui.value + "'");
                    else if (ui.checked == false) {
                        if ($.inArray("'" + ui.value + "'", ProfileName) >= 0)
                            ProfileName.splice(ProfileName.indexOf("'" + ui.value + "'"), 1)
                    }
                    if ($("input[name$='ddlProfile']").length > 0) {
                        //clearControls();
                    }
                },
                checkAll: function (event, ui) {
                    ProfileName = [];
                    for (var i = 0; i < event.target.options.length; i++) {
                        ProfileName.push("'" + $(event.target.options[i]).val() + "'")
                    }

                },
                uncheckAll: function (event, ui) {
                    ProfileName = [];

                }
            });

            $("#<%= ddlProfile.ClientID%>").multiselect("refresh");
            var CheckedCheckBox = document.getElementById('<%= hdnUserTypeCode.ClientID%>').value
            if (CheckedCheckBox != "") {

                CheckedCheckBox = CheckedCheckBox.split(',');
                for (i = 0; i <= CheckedCheckBox.length - 1; i++) {
                    $("#<%= ddlProfile.ClientID%>").multiselect("widget").find(".ui-multiselect-checkboxes.ui-helper-reset").find("input[value=" + CheckedCheckBox[i] + "]").attr("checked", "checked");

                }
                $('#<%= ddlProfile.ClientID%>').multiselect("update");
            }
        }

        function ShowModalPopup() {

            if (confirm("Are You Sure You Want To Delete Reason ?")) {
                $("#ctl00$CPHLAMBDA$txtRemarks").val("")
                document.getElementById('<%=myModalRemarks.ClientID()%>').style.display = 'inline';
                return true;
            }
            return false;
        }
        function ModalPopupClose() {
            document.getElementById('<%=myModalRemarks.ClientID()%>').style.display = 'none';
            $("#ctl00$CPHLAMBDA$txtRemarks").val("")
        }

        function Validation() {
            if ($("#<%= hdnUserTypeCode.ClientID%>").val() == "") {
                msgalert('Please Select Profile !', document.getElementById('<%= txtSub.ClientId %>'));
                return false;
            }


            if (document.getElementById('<%= txtSub.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Subject !', document.getElementById('<%= txtSub.ClientId %>'));
                return false;
            }

            if (document.getElementById('<%= Editor1.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Description !', document.getElementById('<%= Editor1.ClientId %>'));
                return false;
            }

            if (document.getElementById('<%= txtFromDate.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter From Date !', document.getElementById('<%= txtFromDate.ClientId %>'));
                return false;
            }

            if (document.getElementById('<%= txtToDate.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter To Date !', document.getElementById('<%= txtToDate.ClientID%>'));
                return false;
            }

            var fromdate = $get('<%= txtFromDate.ClientId %>').value;
            var todate = $get('<%= txtToDate.ClientID%>').value;

            if (Date.parse(fromdate) > Date.parse(todate)) {
                msgalert("From Date Should Not be Greater Then End Date !",null);
                return false;
            }
         


        }

        function ListofDocumentSelectedSize(fileList) {
            var Fsize = fileList.files[0].size;
            Fsize = Fsize / 1024 / 1024;
            var Magabyte = 1024;
            var ConvertGBsize;

            ConvertGBsize = (Fsize / Magabyte);
            if (ConvertGBsize >= 1) {
                msgalert("File having more than 1 GB size is not allowed!!!",null);
                fileList.value = "";
            }
        }

        function ShowAlert(msg) {
            msgalert(msg);
            window.location.href = "frmNoticeMst.aspx?mode=1";
        }
        function HideNoticeDetail() {
            $('#<%= ImgPanelNoticeData.ClientID%>').click();
        }
        function verifyDate(sender, args) {
            var d = new Date();
            d.setDate(d.getDate() - 1);
            if (sender._selectedDate < d) {
                msgalert("Date should be Today or Grater than Today!");
                sender._textbox.set_Value('')
            }
        }
     

    </script>
</asp:Content>
