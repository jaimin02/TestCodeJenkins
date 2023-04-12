<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmUploadRandomizationDetail.aspx.vb" Inherits="frmUploadRandomizationDetail" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <script type="text/javascript" src="script/autocomplete.js"></script>
    <link rel="shortcut icon" href="favicon.ico">
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <style type="text/css">
        
        .hide_column {
            display: none;
        }
        /*Added by ketan for (Resolve issue oveRlap button in datatable)*/
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }
        /*Ended by ketan*/
    </style>

    <script>

        ////// For Project    
        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'), $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        function Validation() {
            if (document.getElementById('<%=HProjectId.clientid%>').value.toString().trim().length <= 0 || document.getElementById('<%=txtProject.clientid%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=txtProject.clientid%>').value = '';
                document.getElementById('<%=HProjectId.clientid%>').value = '';
                msgalert('Please Select Project !');
                document.getElementById('<%=txtProject.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=FlAttachment.clientid%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=FlAttachment.clientid%>').value = '';
                msgalert('Please Select File to Upload!');
                document.getElementById('<%=FlAttachment.ClientID%>').focus();
                return false;
            }
        document.getElementById('<%= buttonSave.ClientId %>').style.display = 'none';
            return true;
        }

        function ShowConfirmation(strsub) {
            msgConfirmDeleteAlert(null, "Randomization File Is Already Uploaded For This Project And Subject ( " + strsub + " ). This Upload Will Delete Previous Data. Do You Still Want To Continue ?", function (isConfirmed) {
                if (isConfirmed) {
                    document.getElementById('<%=BtnDeleteOld.clientid %>').click();
                    return true;
                } else {
                    return false;
                }
            });
            return false;
        }
        function ValidationForRemark() {
            if ($get('<%= txtRemarks.ClientID%>').value.trim() == '') {
                msgalert('Please Enter Remarks!.');
                $get('<%= txtRemarks.ClientID%>').value = '';
                $get('<%= txtRemarks.ClientID%>').focus();
                return false;
            }
        }
        function funCloseDiv(div) {
            document.getElementById(div).style.display = 'none';
            document.getElementById('<%=ModalBackGround.ClientId %>').style.display = 'none';
        }
        function displayBackGround() {

            document.getElementById('<%=ModalBackGround.ClientId %>').style.display = '';
            document.getElementById('<%=ModalBackGround.ClientId %>').style.height = $('#HFHeight').val() + "px";
            document.getElementById('<%=ModalBackGround.ClientID%>').style.width = $('#HFWidth').val() + "px";
        }
        function CheckTextLength(text, long) {
            var maxlength = new Number(long); // Change number to your max length.
            if (text.value.length > maxlength) {
                text.value = text.value.substring(0, maxlength);
                msgalert(" Only " + long + " characters allowed");
            }
        }
    </script>

    <asp:UpdatePanel ID="upPnlWorkspaceSubjectMst" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellpadding="5" style="width:90%; margin:auto">
                <tbody>
                    <tr>
                        <td>
                            <fieldset class="FieldSetBox" style="display: block; width: 98%; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img2" alt="File Details" src="images/panelcollapse.png"
                                        onclick="Display(this,'divFileDetail');" runat="server" style="margin-right: 2px;" />File Details</legend>
                                <div id="divFileDetail">
                                    <table width="100%">
                                        <tr>
                                            <td style="white-space: nowrap; width: 30%; vertical-align: top; text-align: right;"
                                                class="Label">Project Name/Project No* :
                                            </td>
                                            <td style="white-space: nowrap; text-align: left;">
                                                <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="70%" TabIndex="1"></asp:TextBox>
                                                <asp:Button Style="display: none" ID="btnSetProject" runat="server" Text=" Project"></asp:Button>
                                                &nbsp;
                            <asp:CheckBox ID="chkdoubleblinded" runat="server" Text="Double Blinded Study" TabIndex="2"></asp:CheckBox><asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                                                <asp:HiddenField ID="HParentWorkSpaceId" runat="server"></asp:HiddenField>
                                                <asp:HiddenField ID="HIsTestSite" runat="server"></asp:HiddenField>
                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                                                    TargetControlID="txtProject" ServicePath="AutoComplete.asmx" ServiceMethod="GetMyProjectCompletionList"
                                                    OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1"
                                                    CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                    CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1"
                                                    CompletionListElementID="pnlProjectList">
                                                </cc1:AutoCompleteExtender>
                                                <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td style="white-space: nowrap; text-align: right;" class="Label">File Upload* :
                                            </td>
                                            <td style="white-space: nowrap" align="left" width="80%">
                                                <asp:FileUpload ID="FlAttachment" TabIndex="3" runat="server"></asp:FileUpload>
                                                <asp:LinkButton runat="server" ID="LnkBtnCsvFileFormat" Text=" View CSV File Format"
                                                    ToolTip="Csv File"></asp:LinkButton>
                                            </td>
                                        </tr>

                                        <tr>
                                            <%-- <td style="white-space: nowrap">
                        </td>--%>
                                            <td style="text-align: center;" colspan="2">
                                                <asp:Button ID="buttonSave" runat="server" CssClass="btn btnsave" Text="Save"
                                                    ToolTip="Save" OnClientClick="return Validation();" TabIndex="4" />
                                                <asp:Button ID="buttonExit" OnClick="buttonExit_Click" runat="server" CssClass="btn btnexit"
                                                    Text="Exit" ToolTip="Exit" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);"
                                                    TabIndex="5" />
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
            <asp:PostBackTrigger ControlID="buttonSave" />
            <asp:PostBackTrigger ControlID="btnSetProject" />
            <asp:PostBackTrigger ControlID="btnSaveRemarks" />
        </Triggers>
    </asp:UpdatePanel>

    <table align="center" id="Edit3" style="width:89.5%; margin:auto">
        <tr>
            <td>
                <fieldset id="fsRandomization" runat="server" class="FieldSetBox" style="visibility: hidden; width: 98%; text-align: left; border: #aaaaaa 1px solid;">
                    <legend class="LegendText" style="color: Black; font-size: 12px">
                        <img id="img1" alt="Randomization Data" src="images/panelcollapse.png"
                            onclick="Display(this,'divRandomization');" runat="server" style="margin-right: 2px;" />Randomization Data</legend>
                    <div id="divRandomization">
                        <table width="100%">
                            <tr>
                                <td colspan="4" style="text-align: center; width: 10%; padding: 0px 64px 0px 64px;">
                                    <div id="gvGrid">
                                        <asp:Label ID="lblProjecStatus" runat="server" Style="display: none;"></asp:Label>
                                        <asp:GridView ID="gvRandomizationHdr" runat="server" Style="visibility: hidden; width: 100%;" AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:BoundField HeaderText="Sr.No" />
                                                <asp:BoundField HeaderText="ProjectNo" />
                                                <asp:BoundField HeaderText="Subject No Range" />
                                                <asp:BoundField HeaderText="Uploaded By" />
                                                <asp:BoundField HeaderText="Upload On" />
                                                <asp:BoundField HeaderText="Remarks" />
                                                <asp:BoundField HeaderText="Audit Trail" />
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
    </table>

    <table width="100%">
        <tbody>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="GV_RandomizationHdr" runat="server" SkinID="grdViewAutoSizeMax" Style="width: auto; margin: auto;"
                        PageSize="5" AutoGenerateColumns="False" AllowPaging="True">
                        <Columns>
                            <asp:BoundField HeaderText="#" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField DataField="vProjectNo" HeaderText="ProjectNo">
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Range" HeaderText="Subject No Range">
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField DataField="vUploadedBy" HeaderText="Uploaded By">
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField DataField="dModifyOffSet" HeaderText="Uploaded On">
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField DataField="vRemarks" HeaderText="Remarks" />
                            <asp:TemplateField HeaderText="View">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgView" runat="server" ToolTip="View" ImageUrl="~/images/view.gif"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="FileNo" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblnFileNo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "nFileNo")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </tbody>
    </table>

    <button id="btn3" runat="server" style="display: none;" />
    <cc1:ModalPopupExtender ID="mpeRandomizationDtl" runat="server" PopupControlID="dvActivityAudiTrail" BehaviorID="mpeRandomizationDtl"
        PopupDragHandleControlID="LbldRandomizationDtl" BackgroundCssClass="modalBackground" CancelControlID="imgActivityAuditTrail"
        TargetControlID="btn3">
    </cc1:ModalPopupExtender>

    <%--added by ketan--%>

    <table>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpActivityAuditTrail" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <div id="dvActivityAudiTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
                            <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                <tr>
                                    <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 97%;">
                                             <asp:Label ID="lblRamge" runat="server" class="LabelBold" Text="RANDOMIZATION DETAILS FOR"></asp:Label>
                                    </td>
                                    <td style="width: 3%">
                                        <img id="imgActivityAuditTrail" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
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
                                                    <table id="tblRandomizationView" class="tblAudit" width="100%" style="background-color: aliceblue;"></table>
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


    <div id="dialogRandomizationDtl" runat="server" style="position: relative; display: none; background-color: #FFFFFF; padding: 5px; width: 600; height: inherit; border: dotted 1px gray;">
        <div>
            <div>
                <img id="dialogRandomizationClose" alt="Close" title="Close" src="images/Sqclose.gif"
                    style="position: relative; float: right; right: 5px;" />
                <asp:Label ID="lblRandomizationDtl" runat="server" class="LabelBold" Text=""></asp:Label>
            </div>
        </div>


        <table width="100%">
            <tbody>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="GV_Randomization" runat="server" SkinID="grdViewSml" AllowPaging="True"
                            PageSize="10" AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField HeaderText="#" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField DataField="vProjectNo" HeaderText="ProjectNo">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField DataField="iMySubjectNo" HeaderText="Subject No">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField DataField="iPeriod" HeaderText="Period">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField DataField="vProductType" HeaderText="Product Type">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField DataField="dModifyOn" Visible="false" HeaderText="ModifyOn" />
                                <asp:BoundField DataField="vRandomizationcode" HeaderText="Randomization Code" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </tbody>
        </table>

    </div>

    <asp:Button ID="BtnDeleteOld" Text="Del" ToolTip="Delete" Style="display: none;"
        runat="server" CssClass="btn btncancel" />
    <asp:Button ID="BtnUpdate" Text="Update" ToolTip="Update" Style="display: none;"
        runat="server" CssClass="btn btnsave" />
    <button id="btn1" runat="server" style="display: none;" />
    <cc1:ModalPopupExtender ID="mpeDialog" runat="server" PopupControlID="dialogModal"
        PopupDragHandleControlID="dialogModalTitle" BackgroundCssClass="modalBackground"
        TargetControlID="btn1" CancelControlID="dialogModalClose">
    </cc1:ModalPopupExtender>

    <div class="modal-content modal-lg" id="dialogModal" style="display:none;width:52% !important" runat="server">
        <div class="modal-header">
            <h2>CSV File Format</h2>
            <img id="dialogModalClose" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px;bottom:48px;" />
        </div>
        <div class="modal-body">
            <table border="0" cellpadding="2" cellspacing="2" width="98%">
                <tr>
                    <td>
                        <img src="images/Csv_File_Format.bmp" runat="server" id="Img_Csv_File_Format" style="border: solid 1px blue" alt="CSV FILE" title="Close"/>
                    </td>
                </tr>
            </table>                                    
        </div>
    </div>

    <button id="btn2" runat="server" style="display: none;" />
    <cc1:ModalPopupExtender ID="mpeDialogdoubleblind" runat="server" PopupControlID="dialogModaldoubleblind"
        PopupDragHandleControlID="Lbldoubleblind" BackgroundCssClass="modalBackground"
        TargetControlID="btn2" CancelControlID="dialogModalClose1">
    </cc1:ModalPopupExtender>
    <div id="dialogModaldoubleblind" runat="server" style="position: relative; display: none; background-color: #cee3ed; padding: 5px; width: 600; height: inherit; border: dotted 1px gray;">
        <div>
            <div>
                <img id="dialogModalClose1" alt="Close" title="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px;" />
                <asp:Label ID="Lbldoubleblind" runat="server" class="LabelBold" Text=" CSV File Format"></asp:Label>
            </div>
        </div>
        <table border="0" cellpadding="2" cellspacing="2" width="100%">
            <tr>
                <td>
                    <img src="~/images/doubleblind.jpg" runat="server" id="Img_Csv_File_Format_DB" style="border: solid 1px blue"
                        alt="" />
                </td>
            </tr>
        </table>
    </div>
    <button id="btnRemarks" runat="server" style="display: none;" />
    <cc1:ModalPopupExtender ID="MPERemarks" runat="server" PopupControlID="divRemarks"
        BackgroundCssClass="modalBackground" TargetControlID="btnRemarks" CancelControlID="imgReplacement">
    </cc1:ModalPopupExtender>
    
    <div class="modal-content modal-sm" id="divRemarks" style="display:none;" runat="server">
        <div class="modal-header">
            <h2>Remarks</h2>
            <img id="imgReplacement" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px;bottom: 45px;" title="Close" />
        </div>
        <div class="modal-body">
            <table cellpadding="5" style="width: 100%">
                <tr>
                    <td colspan="2">
                        <table style="width: 100%; text-align: left;">
                            <tr>
                                <td style="text-align: right;" nowrap="noWrap">Remarks* :
                                </td>
                                <td style="text-align: left;" nowrap="noWrap">
                                    <asp:TextBox ID="txtRemarks" runat="Server" Text="" CssClass="textBox" Width="226px"
                                        TextMode="MultiLine" onKeyUp="CheckTextLength(this,500)" onChange="CheckTextLength(this,500)"
                                        Height="50px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left" colspan="2">
                                    <asp:Label ID="lblNote" runat="server" class="LabelBold" ForeColor="Red" Text=" Note:- Maximum 500 characters are allowed "></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>                                     
        </div>
        <div class="modal-header">
            <asp:Button ID="btnSaveRemarks" runat="server" Text="Confirm" CssClass="btn btnsave" 
                OnClientClick="return ValidationForRemark();" ToolTip="Confirm"></asp:Button>                                          
        </div>
    </div>


    <div id="ModalBackGround" runat="server" class="divModalBackGround">
    </div>

    <script type="text/javascript">
        function HideSponsorDetails() {
            //$('#<%= img2.ClientID%>').click();
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

        function projectGrid() {
            var TotalActivity;
            var FileName = "";
            var WorkspaceID = document.getElementById('<%= HProjectId.ClientID%>').value;

            $.ajax({
                type: "post",
                url: "frmUploadRandomizationDetail.aspx/FillProjectGrid",
                data: '{"WorkspaceID":"' + WorkspaceID + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                dataType: "json",
                success: function (data) {
                    $('#ctl00_CPHLAMBDA_fsRandomization').attr("IsTable", "has");
                    $('#ctl00_CPHLAMBDA_gvRandomizationHdr').attr("IsTable", "has");
                    var ActivityDataset = [];
                    if (data.d != "" && data.d != null) {
                        if (data.d.length > 2) {

                        }
                        data = JSON.parse(data.d)
                        TotalActivity = data.length;
                        if (TotalActivity > 0) {
                            document.getElementById("ctl00_CPHLAMBDA_fsRandomization").style.visibility = 'visible';
                            document.getElementById("ctl00_CPHLAMBDA_gvRandomizationHdr").style.visibility = 'visible';
                        }
                        else {
                            document.getElementById("ctl00_CPHLAMBDA_fsRandomization").style.visibility = 'hidden';
                            document.getElementById("ctl00_CPHLAMBDA_gvRandomizationHdr").style.visibility = 'hidden';
                        }
                        for (var Row = 0; Row < data.length; Row++) {
                            var InDataset = [];
                            InDataset.push(data[Row].SrNo, data[Row].ProjectNo, data[Row].FileName, data[Row].UploadedBy, data[Row].UploadedOn, data[Row].Remarks, data[Row].AuditTrail, data[Row].nFileNo);
                            ActivityDataset.push(InDataset);
                        }

                        oTable = $('#ctl00_CPHLAMBDA_gvRandomizationHdr').dataTable({

                            "bJQueryUI": true,
                            "sPaginationType": "full_numbers",
                            "bLengthChange": false,
                            "iDisplayLength": 10,
                            "bProcessing": true,
                            "bSort": false,
                            "autoWidth": true,
                            "aaData": ActivityDataset,
                            "bInfo": false,
                            
                            "fnCreatedRow": function (nRow, aData, iDataIndex) {
                                $('td:eq(6)', nRow).append("<input type='image' id='imgAudit_" + iDataIndex + "' name='imgEdit$" + iDataIndex + "' src='Images/view.gif' OnClick='return AudtiTrail(this);' style='border-width:0px;'  nFileNo='" + aData[7] + "' >");
                            },
                            "aoColumns": [
                                        { "sTitle": "#" },
                                        { "sTitle": "Project No" },
                                        { "sTitle": "Subject No Range" },
                                        { "sTitle": "Uploaded By" },
                                        { "sTitle": "Uploaded On" },
                                        { "sTitle": "Remarks" },
                                        { "sTitle": "View" },
                                        {
                                            "sTitle": "nFileNo",
                                            "sClass": "hide_column"
                                        },
                                         
                            ],
                            "oLanguage": {
                                "sEmptyTable": "No Record Found",
                            },
                            "aoColumnDefs": [
                                        { "bSortable": false, "aTargets": [0] },
                                        { "bSortable": false, "aTargets": [1] },
                                        { "bSortable": false, "aTargets": [2] },
                                        { "bSortable": false, "aTargets": [3] },

                            ],
                            "columnDefs": [
                                       { "width": "1%", "targets": 1 },
                                        { "width": "99%", "targets": 2 }
                            ]

                        });
                    }
                    return false;
                },
                failure: function (response) {
                    msgalert(response.d);
                },
                error: function (response) {
                    msgalert(response.d);
                }
            });
            oTable.fnAdjustColumnSizing();
            return false;
        }

        function AudtiTrail(e) {
            var vActiviyId = "";
            var ActivityName = "";
            var iPeriod = "";
            var nFileNo = ""
            var WorkspaceID = document.getElementById('<%= HProjectId.ClientID%>').value;

            /*Audit trail popup on click of "Audit Trial image" */
            nFileNo = $("#" + e.id).attr("nFileNo");

            if (document.getElementById("ctl00_CPHLAMBDA_txtproject").value != "") {
                $.ajax({
                    type: "post",
                    url: "frmUploadRandomizationDetail.aspx/AuditTrail",
                    data: '{"WorkspaceID":"' + WorkspaceID + '", "nFileNo":"' + nFileNo + '"}',
                    contentType: "application/json; charset=utf-8",
                    datatype: JSON,
                    async: false,
                    success: function (data) {
                        $('#tblRandomizationView').attr("IsTable", "has");
                        var aaDataSet = [];
                        var range = null;

                        if (data.d != "" && data.d != null) {
                            data = JSON.parse(data.d);
                            for (var Row = 0; Row < data.length; Row++) {
                                var InDataSet = [];
                                InDataSet.push(data[Row].SrNo, data[Row].ProjectNo, data[Row].SubjectNo, data[Row].Period, data[Row].ProductType, data[Row].RandomizationCode);
                                range= " [" + data[0].SubjectNo + " - " + data[Row].SubjectNo + "]";
                                aaDataSet.push(InDataSet);
                            }
                            $('#ctl00_CPHLAMBDA_lblRamge').text("RANDOMIZATION DETAILS FOR"+ range );
                        }
                        if ($("#tblRandomizationView").children().length > 0) {
                            $("#tblRandomizationView").dataTable().fnDestroy();
                        }
                        oTable = $('#tblRandomizationView').prepend($('<thead>').append($('#tblRandomizationView tr:first'))).dataTable({

                            "bJQueryUI": true,
                            "sPaginationType": "full_numbers",
                            "bLengthChange": false,
                            "iDisplayLength": 10,
                            "bProcessing": true,
                            "bSort": false,

                            "aaData": aaDataSet,
                            "aoColumns": [
                                {
                                    "sTitle": "#",
                                },
                                { "sTitle": "Project No" },
                                 { "sTitle": "Subject No" },
                                { "sTitle": "Period" },
                                { "sTitle": "Product Type" },
                                //{ "sTitle": "RandomizationCode" },

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
                        $find('mpeRandomizationDtl').show();

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
