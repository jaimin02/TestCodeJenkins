<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmInHouseSubjectAssignment.aspx.vb" Inherits="frmInHouseSubjectAssignment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/popcalendar.js"></script>

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" src="Script/General.js"></script>

    <script type="text/javascript" src="Script/Validation.js"></script>

    <script type="text/javascript" src="Script/jquery.min.js"></script>

    <script type="text/javascript" src="Script/scrollablegrid.js"></script>
    <%--<script src="Script/General.js" language="javascript" type="text/javascript"></script>--%>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>

    <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }
    </style>

    <script type="text/javascript" language="javascript">
        var oTable = "";
        function fsetInHouse_Show() {
            $('#<%=fsetInHouse.ClientID%>').attr('style', $('#<%=fsetInHouse.ClientID%>').attr('style') + ';display:block');
        }
        $(document).ready(function () {
            $('#canal').css('display', 'none');
        })
        ////// For Project    
        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        ///////////For Subject  
        function ClientPopulatedSubject(sender, e) {
            SubjectClientShowing('AutoCompleteExtender2', $get('<%= txtSubject.ClientId %>'));
        }

        function OnSelectedSubject(sender, e) {
            SubjectOnItemSelected(e.get_value(), $get('<%= txtSubject.clientid %>'),
                $get('<%= HSubjectId.clientid %>'), document.getElementById('<%= btnSubject.ClientId %>'));
        }


        function CheckDays(obj) {
            if (obj.value == '') {
                msgalert("Screening Validation Days cannot be Null or blank !");
                obj.value = 28;
            }
            else if (!checkVal(obj.value, obj.id, '2')) {

                msgalert("Please enter Days in Numeric !");
                obj.value = 28;
            }
        }
        //            else {

        //                alert("Please enter Days in Numeric and Not Greater Than 21.");
        //                obj.value = 21;
        //            }
        //        }
        //For Validation
        function Validation() {

            if (document.getElementById('<%= HProjectId.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Project !');
                document.getElementById('<%= txtProject.ClientId %>').focus();
                document.getElementById('<%= txtProject.ClientId %>').value = '';
                return false;
            }
            else if (document.getElementById('<%= ddlPeriod.ClientId %>').selectedIndex == 0) {
                msgalert('Please select Period !');
                document.getElementById('<%= ddlPeriod.ClientId %>').focus();
                return false;
            }
            else if (document.getElementById('<%= HSubjectId.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Subject !');
                document.getElementById('<%= txtSubject.ClientId %>').focus();
                document.getElementById('<%= txtSubject.ClientId %>').value = '';
                return false;
            }
    document.getElementById('<%=btnadd.ClientID %>').style.display = 'none';
            return true;

        }

        //For rejection Validation       
        function rejectValidation() {
            if (document.getElementById('<%= txtRejectReason.ClientId %>').value == '') {
                msgalert('Please select Reason To Reject !');
                document.getElementById('<%= txtRejectReason.ClientId  %>').focus();
                return false;
            }
            return true;
        }

        function ShowAttConfirm(altStr) {
            document.getElementById('<%= btnchkScrDate.ClientId %>').click();
        }

        function UIgvwWorkspaceSubjectMst() {
            $('#<%= gvwWorkspaceSubjectMst.ClientID%>').removeAttr('style', 'display:block');
            oTab = $('#<%= gvwWorkspaceSubjectMst.ClientID%>').prepend($('<thead>').append($('#<%= gvwWorkspaceSubjectMst.ClientID%> tr:first'))).dataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": false,
                "sScrollY": "250px",
                //"sScrollX": "100%",
                
                "bScrollCollapse": true,
                aLengthMenu: [
                    [10, 25, 50, 100, -1],
                    [10, 25, 50, 100, "All"]
                ],
            });

            setTimeout(function () {
                oTab.fnAdjustColumnSizing();

            },100);
           
            return false;
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
        function fsetInHouse_Show() {
            $('#<%=fsetInHouse.ClientID%>').attr('style', $('#<%=fsetInHouse.ClientID%>').attr('style') + ';display:block');
         }

    </script>

    <asp:UpdatePanel ID="upPnlWorkspaceSubjectMst" runat="server">
        <ContentTemplate>
            <table cellpadding="0" style="width: 100%">
                <tbody>
                    <tr style="vertical-align: top;">
                        <td style="vertical-align: top;">
                            <button id="btn2" runat="server" style="display: none;" />
                            <cc1:ModalPopupExtender ID="MpeAudit" runat="server" BackgroundCssClass="modalBackground"
                                CancelControlID="Img1" PopupControlID="DivAudit" PopupDragHandleControlID="lblTitle"
                                TargetControlID="btn2">
                            </cc1:ModalPopupExtender>
                            <div id="DivAudit" runat="server" style="position: relative; display: none; background-color: #cee3ed;
                                padding: 5px; width: 800px; height: inherit; border: dotted 1px gray;">
                                <div>
                                    <img id="Img1" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right;
                                        right: 5px;" />
                                    <asp:Label ID="lblTitle" Text="Details of Deleted Attendance" runat="server" class="LabelBold" />
                                </div>
                                <table border="0" cellpadding="2" cellspacing="2" width="40%">
                                    <tbody>
                                        <tr>
                                            <td align="center" colspan="2" style="height: 168px">
                                                <asp:Panel ID="pnlMedExGrid" runat="server" Height="300px" ScrollBars="Auto" Width="795px">
                                                    <asp:GridView ID="GVAudit" runat="server" __designer:wfdid="w45" AutoGenerateColumns="False"
                                                        BorderColor="Peru" Font-Size="Small" SkinID="grdViewSmlSize">
                                                        <Columns>
                                                            <asp:BoundField DataField="iAsnNo" HeaderText="AsnNo" />
                                                            <asp:BoundField DataField="vInitials" HeaderText="Initials">
                                                                <ItemStyle Width="20%" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="vSubjectID" HeaderText="SubjectID">
                                                                <ItemStyle Width="20%" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="vMySubjectNo" HeaderText="MySubjectNo">
                                                                <ItemStyle Width="20%" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="dReportingDate" HeaderText="Reporting Date" HtmlEncode="False">
                                                                <ItemStyle Wrap="false" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="vModifyBy" HeaderText="Deleted By">
                                                                <ItemStyle Width="20%" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="dModifyOn" HeaderText="Deleted On" HtmlEncode="False">
                                                                <ItemStyle Wrap="false" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="vRemarks" HeaderText="Remarks">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="nWorkspaceSubjectHistoryId" HeaderText="SampleId" />
                                                            <asp:BoundField DataField="vWorkspaceSubjectId" HeaderText="iReviewedBy" />
                                                            <asp:BoundField DataField="iTranNo" HeaderText="TranNo" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <button id="btnmpeReject" runat="server" style="display: none" />
                            <cc1:ModalPopupExtender runat="server" ID="mpeRejectSubject" BackgroundCssClass="modalBackground"
                                TargetControlID="btnmpeReject" PopupControlID="divRejectSubject" CancelControlID="ImgClose">
                            </cc1:ModalPopupExtender>
                            <div style="display: none; left: 230px; top: 69px; text-align: center; width: 300px"
                                id="divRejectSubject" class="popUpDivNoTop" runat="server">
                                <table cellpadding="0" align="center
                                
                                
                                
                                
                                ">
                                    <tr>
                                        <td>
                                            <img runat="server" id="ImgClose" alt="Close" src="images/Sqclose.gif" style="position: relative;
                                                float: right; right: 5px;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <h5>
                                                Subject Rejection</h5>
                                        </td>
                                    </tr>
                                    <tr id="trAddRejectSubject" runat="server">
                                        <td>
                                            <table cellpadding="5">
                                                <tbody>
                                                    <tr>
                                                        <td class="Label" align="left">
                                                            Subject Name:
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="lblSubjectName" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Label" align="left">
                                                            Reject Reason: &nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtRejectReason" runat="server" TextMode="MultiLine" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Button ID="btnSave" runat="server" CssClass="btn btnsave" OnClick="btnSave_Click"
                                                                OnClientClick="return rejectValidation();" Text="Reject" />
                                                            &nbsp;<asp:Button ID="btnClose" runat="server" CssClass="btn btnclose" OnClick="btnClose_Click"
                                                                Text="Cancel" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <button id="btn1" runat="server" style="display: none;" />
                            <cc1:ModalPopupExtender ID="mpeDialog" runat="server" BackgroundCssClass="modalBackground"
                                CancelControlID="dialogModalClose" PopupControlID="dialogModal" PopupDragHandleControlID="dialogModalTitle"
                                TargetControlID="btn1">
                            </cc1:ModalPopupExtender>
                            <div id="dialogModal" runat="server" style="position: relative; display: none; background-color: #cee3ed;
                                padding: 5px; width: 250px; height: inherit; border: dotted 1px gray;">
                                <div>
                                    <h1>
                                        <div>
                                            <img id="dialogModalClose" alt="Close" src="images/Sqclose.gif" style="position: relative;
                                                float: right; right: 5px;" />
                                            <asp:Label ID="dialogModalTitle" runat="server" class="LabelBold" />
                                        </div>
                                    </h1>
                                </div>
                                <table cellpadding="2" cellspacing="2" style="width: 40%; border: 0">
                                    <tbody>
                                        <tr style="width: 100%">
                                            <td style="width: 35%; height: 48px">
                                                <strong style="white-space: nowrap">Remarks</strong>
                                            </td>
                                            <td style="width: 65%; height: 48px">
                                                <asp:TextBox ID="txtLockRemark" runat="server" __designer:wfdid="w39" CssClass="textBox"
                                                    Height="38px" TabIndex="2" Width="176px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:Button ID="btnDivOK" runat="server" __designer:wfdid="w40" CssClass="btn btnnew"
                                                    Text="OK" wfdid="w31" />
                                                &nbsp;<asp:Button ID="btnDivCancel" runat="server" __designer:wfdid="w41" CssClass="btn btncancel"
                                                    Text="Cancel" wfdid="w31" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <fieldset class="FieldSetBox" style="display: block; margin: auto; width: 98%; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img2" alt="In-House Subject Assignment Parameter" src="images/panelcollapse.png"
                                        onclick="Display(this,'divInHouseParameter');" runat="server" style="margin-right: 2px;" />In-House Subject Assignment Parameter</legend>
                                <div id="divInHouseParameter">
                                    <table cellpadding="5px" style="margin-left: 5%; width: 90%;">
                                        <tbody>
                                            <tr>
                                                <td style="text-align: left; vertical-align: top; width: 75%">
                                                    <fieldset class="FieldSetBox" style="display: block; width: 94%; text-align: left; border: #aaaaaa 1px solid;">
                                                        <legend class="LegendText" style="color: Black; font-size: 12px">
                                                            <img id="img3" alt="In-House Subject Assignment" src="images/panelcollapse.png"
                                                                onclick="Display(this,'divSubject');" runat="server" style="margin-right: 2px;" />In-House Subject Assignment</legend>
                                                        <div id="divSubject">
                                                            <table cellpadding="5" style="width: 90%;">
                                                                <tbody>
                                                                    <tr>
                                                                        <td style="text-align: right; vertical-align: top; white-space: nowrap; width: 20%;"
                                                                            class="Label">Project Name/Project No* :
                                                                        </td>
                                                                        <td align="left" colspan="2">
                                                                            <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="92%" />
                                                                            <br />
                                                                            <asp:Button ID="btnSetProject" runat="server" OnClick="btnSetProject_Click" Style="display: none"
                                                                                Text=" Project" />
                                                                            <asp:HiddenField ID="HProjectId" runat="server" />
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                                                                CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                                                CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                                                                OnClientShowing="ClientPopulated" ServiceMethod="GetProjectCompletionListWithOutSponser"
                                                                                ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True"
                                                                                CompletionListElementID="pnlProjectList">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" />
                                                                        </td>
                                                                        <%--<td align="left" class="Label" rowspan="5" style="text-align: center" valign="top">Photo<br />
                                                            <asp:Image ID="Image1" runat="server" Height="125px" ImageUrl="frmPIFImage.aspx"
                                                                Width="125px" />
                                                        </td>--%>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right" class="Label" style="width: 237px; white-space: nowrap" valign="top">Period* :
                                                                        </td>
                                                                        <td align="left" style="white-space: nowrap">
                                                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:DropDownList ID="ddlPeriod" runat="server" AutoPostBack="True" CssClass="dropDownList"
                                                                                        OnSelectedIndexChanged="ddlPeriod_SelectedIndexChanged" Width="203px">
                                                                                    </asp:DropDownList>
                                                                                    &nbsp;
                                                                    <asp:Button runat="server" ID="BtnAddPeriod" Text="Add Period" CssClass="btn btnadd"
                                                                        OnClientClick="return msgconfirmalert('Adding A Period Will Also cause Addition of Activities in Project Structure.\n Are you Sure To add Period.',this);"
                                                                        Visible="false" Width="80" />
                                                                                </ContentTemplate>
                                                                                <Triggers>
                                                                                    <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
                                                                                </Triggers>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="Label" style="width: 237px; white-space: nowrap; text-align: right; vertical-align: top;">Subject* :
                                                                        </td>
                                                                        <td style="text-align: left; vertical-align: top;">
                                                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:TextBox ID="txtSubject" runat="server" CssClass="textBox" TabIndex="2" Width="96%" />
                                                                                    <asp:Button ID="btnSubject" runat="server" OnClick="btnSubject_Click" Style="display: none"
                                                                                        Text="Subject" />
                                                                                    <asp:HiddenField ID="HSubjectId" runat="server" />
                                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" BehaviorID="AutoCompleteExtender2"
                                                                                        CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                                                        CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelectedSubject"
                                                                                        OnClientShowing="ClientPopulatedSubject" ServiceMethod="GetSubjectCompletionList_NotRejected_InHouse"
                                                                                        ServicePath="AutoComplete.asmx" TargetControlID="txtSubject" UseContextKey="True"
                                                                                        CompletionListElementID="pnlSubjectList">
                                                                                    </cc1:AutoCompleteExtender>
                                                                                    <asp:Panel ID="pnlSubjectList" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" />
                                                                                </ContentTemplate>
                                                                                <Triggers>
                                                                                    <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
                                                                                </Triggers>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="Label" style="width: 237px; white-space: nowrap; height: 31px; text-align: left; vertical-align: top;" />
                                                                        <td class="Label" colspan="1" nowrap style="height: 31px; text-align: left;">
                                                                            <asp:TextBox ID="txtDate" runat="server" CssClass="textBox" Style="disabled: false"
                                                                                Visible="False" Width="90px" />
                                                                            <img id="ImgStart" alt="Select  Date" onclick="popUpCalendar(this,ctl00_CPHLAMBDA_txtDate,'dd-mmm-yyyy');"
                                                                                src="images/Calendar_scheduleHS.png" style="display: none; disabled: true" />
                                                                        </td>
                                                                        <td class="Label" colspan="1" nowrap style="height: 31px; text-align: left;" />
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="TDMandatory" style="width: 237px; white-space: nowrap; text-align: left; vertical-align: top" />
                                                                        <td class="Label" colspan="1" nowrap style="white-space: nowrap; text-align: left;">
                                                                            <asp:Button ID="btnAdd" runat="server" CausesValidation="True" CssClass="btn btnsave"
                                                                                OnClick="btnAdd_Click" OnClientClick="return Validation();" TabIndex="6" Text=" Save" />
                                                                            &nbsp;<asp:Button ID="btnCancel" runat="server" CausesValidation="True" CssClass="btn btncancel"
                                                                                OnClick="btnCancel_Click" TabIndex="6" Text="Cancel" />
                                                                            &nbsp;
                                                            <asp:Button ID="btnCloseNew" runat="server" CausesValidation="True" CssClass="btn btnclose"
                                                                OnClick="btnCloseNew_Click" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);"
                                                                TabIndex="6" Text="Exit" />
                                                                            &nbsp;
                                                                        <asp:Button ID="btnAudit" runat="server" Visible="false" CausesValidation="True"
                                                                                CssClass="btn btnaudit" TabIndex="6" Text="Audit Trail" />
                                                                            <asp:Button ID="btnchkScrDate" Style="display: none;" runat="server" CssClass="btn btnnew" />
                                                                        </td>
                                                                        <td class="Label" colspan="1" nowrap style="white-space: nowrap; text-align: left;" />
                                                                    </tr>
                                                            </table>
                                                        </div>
                                                    </fieldset>
                                                </td>
                                                <td style="text-align: left; vertical-align: top; width: 25%">
                                                    <fieldset class="FieldSetBox" style="display: block; width: 94%; text-align: left; border: #aaaaaa 1px solid;">
                                                        <legend class="LegendText" style="color: Black; font-size: 12px">
                                                            <img id="imgfldgen" alt="Photograph" src="images/panelcollapse.png"
                                                                onclick="Display(this,'divPhotograph');" runat="server" style="margin-right: 2px;" />Photograph</legend>
                                                        <div id="divPhotograph" style="height: 173px;">
                                                            <table style="height: 100%">
                                                                <tr>
                                                                    <td class="Label" style="text-align: center; vertical-align: middle;">
                                                                        <br />
                                                                        <asp:Image ID="Image1" Style="margin-left: 41%" runat="server" Height="125px" ImageUrl="frmPIFImage.aspx"
                                                                            Width="125px" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </fieldset>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <fieldset id="fsetInHouse" runat="server" class="FieldSetBox" style="display: none; width: 98%; text-align: left; border: #aaaaaa 1px solid;">
                            <legend class="LegendText" style="color: Black; font-size: 12px">
                                <img id="img4" alt="Patient Details" src="images/panelcollapse.png"
                                    onclick="Display(this,'divPatientData');" runat="server" style="margin-right: 2px;" />Enrolled Patient Data</legend>

                            <div id="divPatientData">
                                            <table cellpadding="5" style="width: 100%;">
                                <tbody>
                                    <tr>
                                        <td>
                                            <%--<div style="width: 100%; margin: 0 auto; padding-top: 5px;">--%>
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <span style="margin: 0 auto;">
                                                            <asp:Label ID="lblTotalSub" runat="server" Width="298px" /></span>
                                                        <asp:GridView ID="gvwWorkspaceSubjectMst" runat="server" AutoGenerateColumns="False"
                                                            OnPageIndexChanging="gvwWorkspaceSubjectMst_PageIndexChanging" OnRowCreated="gvwWorkspaceSubjectMst_RowCreated"
                                                            OnRowDataBound="gvwWorkspaceSubjectMst_RowDataBound">
                                                            <Columns>
                                                                <asp:BoundField HeaderText="ASN">
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="vSubjectId" HeaderText="Subject ID">
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="iMySubjectNo" HeaderText="My Subject No" />
                                                                <asp:BoundField DataField="FullName" HeaderText="Subject">
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="vInitials" HeaderText="Initials">
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="iPeriod" HeaderText="Period" />
                                                                <asp:BoundField DataField="dReportingDate" HeaderText="Reporting Date" HtmlEncode="False">
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="vUserName" HeaderText="Last Modified BY" />
                                                                <asp:BoundField DataField="vWorkspaceSubjectId" HeaderText="WorkspaceSubjectId" />
                                                                <%--'01-Feb-11--%>
                                                                <asp:TemplateField HeaderText="Delete" SortExpression="status">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ImgCancel" runat="server" OnClientClick="return msgconfirmalert('Are You Sure You want to Delete attendance of this Subject?',this);"
                                                                            ImageUrl="~/images/Cancel.gif" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Reason">
                                                                    <ItemTemplate>
                                                                        <img id="imgShow" runat="server" alt='<%#Eval("vRemarks") %>' src="images/collapse.jpg"
                                                                            title="Reason" />
                                                                        <div id="canal" runat="server" style="display: none; border: outset 2px black; background: white;
                                                                            font: verdana; font-size: 8pt; height: auto;">
                                                                            <asp:Label ID="Reason" runat="server" Text='<%# Eval("vRemarks") %>' />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Reject">
                                                                    <ItemTemplate>
                                                                        <%--<asp:LinkButton ID="ImgReject" runat="server" Text="Reject" OnClientClick="return confirm('Are You Sure You want to Reject This Subject?');">Reject</asp:LinkButton>--%>
                                                                        <asp:ImageButton ID="ImgReject" runat="server" OnClientClick="return msgconfirmalert('Are You Sure You want to Reject This Subject ?',this);"
                                                                            ImageUrl="~/images/reject.png" AlternateText="Reject" ToolTip="Reject" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="cRejectionFlag" HeaderText="IsRejected" />
                                                            </Columns>
                                                        </asp:GridView>
                                                        <br />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlPeriod" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnchkScrDate" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
