<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmWorkspaceSubjectMst, App_Web_22suyskz" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }
       #ctl00_CPHLAMBDA_GVAudit_wrapper
    {
        width:1500px !important;
}
    </style>
    <script type="text/javascript" src="Script/popcalendar.js"></script>
    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <script type="text/javascript" src="Script/General.js"></script>
    <script type="text/javascript" src="Script/Validation.js"></script>
    <script type="text/javascript" src="Script/jquery.min.js"></script>

    <asp:UpdatePanel ID="upPnlWorkspaceSubjectMst" runat="server">
        <ContentTemplate>
            <table style="width: 90%; margin: auto;">
                <tbody>
                    <tr>
                        <td style="vertical-align: top;">
                            <button id="btn2" runat="server" style="display: none;" />
                            <cc1:ModalPopupExtender ID="MpeAudit" runat="server" BackgroundCssClass="modalBackground"
                                CancelControlID="ImgCloseDeletedAudit" PopupControlID="DivAudit" PopupDragHandleControlID="lblTitle"
                                TargetControlID="btn2">
                            </cc1:ModalPopupExtender>
                            <button id="btn3" runat="server" style="display: none;" />
                            <cc1:ModalPopupExtender ID="MPEAuditTrail" runat="server" BackgroundCssClass="modalBackground"
                                CancelControlID="ImgAuditTrail" PopupControlID="DivAuditTrail" PopupDragHandleControlID="lblAuditTrailTitle"
                                TargetControlID="btn3" />
                            <button id="btn4" runat="server" style="display: none;" />
                            <cc1:ModalPopupExtender ID="MPEReject" runat="server" BackgroundCssClass="modalBackground"
                                CancelControlID="ImgCloseRejection" PopupControlID="divRejectSubject" PopupDragHandleControlID="lblRejectSubject"
                                TargetControlID="btn4">
                            </cc1:ModalPopupExtender>

                            <div id="DivAudit" runat="server" class="centerModalPopup" style="display: none; width: 90%; position: absolute; max-height: 404px; margin: auto">
                                <div style="margin: auto">
                                    <img id="ImgCloseDeletedAudit" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px;"
                                        title="Close" />
                                    <asp:Label ID="lblTitle" Text="Details of Deleted Attendance" runat="server" class="Label" />
                                    <hr />
                                </div>
                                <div style="max-height: 300px; margin: auto; overflow-x: auto; overflow-y: auto">
                                    <asp:GridView ID="GVAudit" runat="server" AutoGenerateColumns="False" BorderColor="Peru"
                                        Font-Size="Small" SkinID="grdViewSmlAutoSize" ForeColor="Red">
                                        <Columns>
                                            <asp:BoundField DataField="iAsnNo" HeaderText="Asn No" />
                                            <asp:BoundField DataField="vInitials" HeaderText="Initials">
                                                <ItemStyle Width="20%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="vSubjectID" HeaderText="Subject ID">
                                                <ItemStyle Width="20%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="vMySubjectNo" HeaderText="MySubject No">
                                                <ItemStyle Width="20%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="dReportingDate" HeaderText="Reporting Date" HtmlEncode="False">
                                                <ItemStyle Wrap="false" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="vModifyBy" HeaderText="Modify By">
                                                <ItemStyle Width="20%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="dModifyOn" HeaderText="Modify On" HtmlEncode="False">
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
                                </div>
                            </div>

                            <div id="DivAuditTrail" runat="server" class="centerModalPopup" style="display: none; width: 90%; position: absolute; top: 25%; max-height: 404px;">
                                <div style="width: 100%; margin: auto; overflow: auto;">
                                    <img id="ImgAuditTrail" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px;"
                                        title="Close" />
                                    <asp:Label ID="lblAuditTrailTitle" Text="Audit Trail" runat="server" class="Label" />
                                    <hr />
                                </div>
                                <div style="margin: auto; overflow: auto;">
                                    <asp:GridView ID="GVAuditTrail" runat="server" AutoGenerateColumns="False" BorderColor="Peru"
                                        Font-Size="Small">
                                        <Columns>
                                            <asp:BoundField DataField="vInitials" HeaderText="Initials"></asp:BoundField>
                                            <asp:BoundField DataField="vSubjectID" HeaderText="Subject ID">
                                                <ItemStyle Width="20%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="vMySubjectNo" HeaderText="MySubject No"></asp:BoundField>
                                            <asp:BoundField DataField="dReportingDate" HeaderText="Reporting Date" HtmlEncode="False">
                                                <ItemStyle Wrap="false" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="vModifyBy" HeaderText="Modify By">
                                                <ItemStyle Width="20%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="dModifyOn" HeaderText="Modify On" HtmlEncode="False">
                                                <ItemStyle Wrap="false" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ReasonDesc" HeaderText="Rejected Remarks">
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>

                            <button id="btn1" runat="server" style="display: none;" />
                            <cc1:ModalPopupExtender ID="mpeDialog" runat="server" BackgroundCssClass="modalBackground"
                                CancelControlID="dialogModalClose" PopupControlID="dialogModal" PopupDragHandleControlID="dialogModalTitle"
                                TargetControlID="btn1">
                            </cc1:ModalPopupExtender>


                            <div class="modal-content modal-sm" id="dialogModal" style="display:none;" runat="server">
                                <div class="modal-header">
                                    <img id="dialogModalClose" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px;"
                                        title="Close" />
                                    <h2 style="font-size:24px;">Subject Delete</h2>
                                </div>
                                <div class="modal-body">
                                    <table cellpadding="2" cellspacing="2" style="width: 30%;margin-left:19%;">
                                        <tbody>
                                            <tr style="width: 100%">
                                                <td style="width: 35%; height: 48px; " align="center">
                                                    <strong style="white-space: nowrap">Remarks</strong>
                                                </td>
                                                <td style="width: 65%; height: 48px" align="center">
                                                    <asp:TextBox ID="txtLockRemark" runat="server" CssClass="textBox" Height="38px" TabIndex="2"
                                                        Width="176px" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>

                                <div class="modal-header">
                                    <asp:Button ID="btnDivOK" runat="server" CssClass="btn btnsave" Text="OK" OnClientClick="return remarkValidaton();" ToolTip="OK" />
                                        <asp:Button ID="btnDivCancel" runat="server" CssClass="btn btncancel" Text="Cancel" wfdid="w31" ToolTip="Cancel" />
                                </div>
                            </div>

                            <div class="modal-content modal-sm" id="divRejectSubject" style="display:none;" runat="server">
                                <div class="modal-header">
                                    <img id="ImgCloseRejection" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px;"
                                        title="Close" />
                                    <h2 style="font-size:24px;">Subject Rejection</h2>
                                </div>
                                <div class="modal-body">
                                    <table style="width: 100%; margin: auto;" cellpadding="5px">
                                        <tbody>
                                            <tr>
                                                <td class="Label" style="text-align: right; vertical-align: bottom;">
                                                    Subject Name:
                                                </td>
                                                <td style="text-align: left; vertical-align: bottom;">
                                                    <asp:Label ID="lblSubjectName" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Label" style="text-align: right; vertical-align: bottom;">Reject Reason:
                                                </td>
                                                <td style="text-align: left; vertical-align: bottom;">
                                                    <asp:DropDownList ID="ddlReject" runat="server" CssClass="dropDownList" />
                                                </td>
                                            </tr>
                                            <tr id="divRejectText" style="display: none">
                                                <td class="Label" style="text-align: right; vertical-align: bottom;">Enter Reason:
                                                </td>
                                                <td style="vertical-align: bottom;">
                                                    <asp:TextBox ID="txtSubjectRemark" runat="server" Height="93px" TextMode="MultiLine"
                                                        Width="394px" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <div class="modal-header">
                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btnsave" OnClientClick="return rejectValidation();" Text="Reject" ToolTip="Reject" />
                                    <asp:Button ID="btnClose" runat="server" CssClass="btn btnclose" OnClick="btnClose_Click" Text="Cancel" ToolTip="Cancel" />
                                </div>
                            </div>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <fieldset class="FieldSetBox" style="display: block; width: 98%; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img2" alt="Attendance Parameter" src="images/panelcollapse.png"
                                        onclick="Display(this,'divAttendanceParameter');" runat="server" style="margin-right: 2px;" />Attendance Parameter</legend>
                                <div id="divAttendanceParameter">
                                    <table cellpadding="5px" style="margin-left: 5%; width: 90%;">
                                        <tr>
                                            <td style="text-align: left; vertical-align: top; width: 75%">
                                                <fieldset class="FieldSetBox" style="display: block; width: 94%; text-align: left; border: #aaaaaa 1px solid;">
                                                    <legend class="LegendText" style="color: Black; font-size: 12px">
                                                        <img id="img1" alt="Attendance" src="images/panelcollapse.png"
                                                            onclick="Display(this,'divAttendance');" runat="server" style="margin-right: 2px;" />Attendance</legend>
                                                    <div id="divAttendance">
                                                        <table cellpadding="5" style="width: 90%;">
                                                            <tbody>
                                                                <tr>
                                                                    <td style="text-align: right; vertical-align: top; white-space: nowrap; width: 20%;"
                                                                        class="Label">Project Name/Project No* :
                                                                    </td>
                                                                    <td style="text-align: left; vertical-align: top; width: 80%;" colspan="2">
                                                                        <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Style="width: 90%;"
                                                                            TabIndex="1" />
                                                                        <br />
                                                                        <asp:Button ID="btnSetProject" runat="server" OnClick="btnSetProject_Click" Style="display: none"
                                                                            Text=" Project" ToolTip="Project" />
                                                                        <asp:HiddenField ID="HProjectId" runat="server" />
                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                                                            CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                                            CompletionListElementID="pnlProjectList" CompletionListItemCssClass="autocomplete_listitem"
                                                                            MinimumPrefixLength="1" OnClientItemSelected="OnSelected" OnClientShowing="ClientPopulated"
                                                                            ServiceMethod="GetMyProjectCompletionList" ServicePath="AutoComplete.asmx" TargetControlID="txtProject"
                                                                            UseContextKey="True" />
                                                                        <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 300px; overflow: auto; overflow-x: hidden" />
                                                                        <asp:CheckBox ID="chkProject" runat="server" AutoPostBack="True" CssClass="Label"
                                                                            OnCheckedChanged="chkProject_CheckedChanged" Text="Project Specific Subjects Only"
                                                                            TabIndex="2" />
                                                                    </td>
                                                                    <td></td>
                                                                </tr>

                                                                <tr id="tdScreeningValidDays" runat="Server">
                                                                    <td class="Label" style="width: 237px; white-space: nowrap; text-align: right; vertical-align: top;">Screening Validation Days* :<br />
                                                                        (-Pre-Check-in days)
                                                                    </td>
                                                                    <td style="text-align: left; vertical-align: top">
                                                                        <asp:TextBox ID="txtScreenDays" runat="server" CssClass="textBox" onblur="CheckDays(this);"
                                                                            Width="36px" TabIndex="3">28</asp:TextBox>
                                                                    </td>
                                                                    <td></td>
                                                                </tr>

                                                                <tr>
                                                                    <td class="Label" style="width: 237px; white-space: nowrap; vertical-align: top; text-align: right;">Period* :
                                                                    </td>
                                                                    <td style="white-space: nowrap; text-align: left; vertical-align: top">
                                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:DropDownList ID="ddlPeriod" runat="server" AutoPostBack="True" CssClass="dropDownList"
                                                                                    OnSelectedIndexChanged="ddlPeriod_SelectedIndexChanged" Width="203px" TabIndex="4" />
                                                                            </ContentTemplate>
                                                                            <Triggers>
                                                                                <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
                                                                            </Triggers>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                    <td></td>
                                                                </tr>

                                                                <tr id="tdsubject" runat="server">
                                                                    <td class="Label" style="width: 237px; white-space: nowrap; text-align: right; vertical-align: top;">Subject* :
                                                                    </td>
                                                                    <td style="text-align: left; vertical-align: top;">
                                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:TextBox ID="txtSubject" runat="server" CssClass="textBox" TabIndex="5" Width="94%" />
                                                                                <asp:Button ID="btnSubject" runat="server" OnClick="btnSubject_Click" Style="display: none"
                                                                                    Text="Subject" ToolTip="Subject" />
                                                                                <asp:HiddenField ID="HSubjectId" runat="server" />
                                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" BehaviorID="AutoCompleteExtender2"
                                                                                    CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                                                    CompletionListElementID="pnlSubjectList" CompletionListItemCssClass="autocomplete_listitem"
                                                                                    MinimumPrefixLength="1" OnClientItemSelected="OnSelectedSubject" OnClientShowing="ClientPopulatedSubject"
                                                                                    ServiceMethod="GetSubjectCompletionList_Dynamic" ServicePath="AutoComplete.asmx"
                                                                                    TargetControlID="txtSubject" UseContextKey="True">
                                                                                </cc1:AutoCompleteExtender>
                                                                                <asp:Panel ID="pnlSubjectList" runat="server" Style="max-height: 300px; overflow: auto; overflow-x: hidden" />
                                                                            </ContentTemplate>
                                                                            <Triggers>
                                                                                <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
                                                                            </Triggers>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                    <td></td>
                                                                </tr>

                                                                <tr>
                                                                    <td class="TDMandatory" style="width: 237px; white-space: nowrap; text-align: left; vertical-align: top;" />
                                                                    <td class="Label" colspan="1" style="white-space: nowrap; text-align: left; white-space: nowrap; vertical-align: top">
                                                                        <asp:Button ID="btnAdd" runat="server" CausesValidation="True" CssClass="btn btnsave"
                                                                            OnClick="btnAdd_Click" OnClientClick="return Validation();" TabIndex="7" Text=" Save"
                                                                            ToolTip="Save" />
                                                                        <asp:Button ID="btnCancel" runat="server" CausesValidation="True" CssClass="btn btncancel"
                                                                            OnClick="btnCancel_Click" TabIndex="8" Text="Cancel" ToolTip="Cancel" />
                                                                        <asp:Button ID="btnCloseNew" runat="server" CausesValidation="True" CssClass="btn btnexit"
                                                                            OnClick="btnCloseNew_Click" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);"
                                                                            TabIndex="10" Text="Exit" ToolTip="Exit" />
                                                                        <asp:Button ID="btnAudit" runat="server" Visible="false" CausesValidation="True"
                                                                            CssClass="btn btnaudit" TabIndex="6" ToolTip="Audit Trail"
                                                                             />
                                                                        <asp:Button ID="btnchkScrDate" Style="display: none;" runat="server" CssClass="btn btnnew" />
                                                                    </td>
                                                                    <td class="Label" colspan="1" style="white-space: nowrap; text-align: left; vertical-align: top; white-space: nowrap;" />
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </fieldset>
                                            </td>
                                            <td style="text-align: left; vertical-align: top; width: 25%">
                                                <fieldset class="FieldSetBox" style="display: block; width: 94%; text-align: left; border: #aaaaaa 1px solid;">
                                                    <legend class="LegendText" style="color: Black; font-size: 12px">
                                                        <img id="imgfldgen" alt="Photograph" src="images/panelcollapse.png"
                                                            onclick="Display(this,'divPhotograph');" runat="server" style="margin-right: 2px;" />Photograph</legend>
                                                    <div id="divPhotograph" style="height: 187px;">
                                                        <table style="height: 100%">
                                                            <tr>
                                                                <td class="Label" style="text-align: center; vertical-align: middle;">
                                                                    <br />
                                                                    <asp:Image Style="margin-left: 41%" ID="Image1" runat="server" Height="56%" ImageUrl="~/images/NotFound.gif"
                                                                        Width="130px" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </fieldset>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </fieldset>
                        </td>
                    </tr>

                    <tr>
                        <td style="text-align: left; vertical-align: top">
                            <fieldset id="fsetAttendanceData" runat="server" class="FieldSetBox" style="display: none; width: 98%; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img3" alt="Attendance Data" src="images/panelcollapse.png"
                                        onclick="Display(this,'divAttendanceData');" runat="server" style="margin-right: 1px;" />Attendance Data</legend>
                                <div id="divAttendanceData" style="width: 1120px; margin:auto; padding-top: 5px;">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblTotalSub" runat="server" Width="298px" />
                                            <div id="Exportdiv" runat="server" style="display: none;">
                                                <asp:Button ID="btnExportToExcel" runat="server"  Style="float: right;"
                                                    CssClass="btn btnexcel" ToolTip="Export To Excel"/>
                                                <asp:Button ID="btnExporttoPdf" runat="server" Style="float: right;margin-right:10px;"
                                                    CssClass="btn btnpdf" ToolTip="Export To PDF"/>
                                            </div>
                                            <asp:GridView ID="gvwWorkspaceSubjectMst" runat="server" AutoGenerateColumns="False"
                                                OnPageIndexChanging="gvwWorkspaceSubjectMst_PageIndexChanging" OnRowCommand="gvwWorkspaceSubjectMst_RowCommand"
                                                OnRowCreated="gvwWorkspaceSubjectMst_RowCreated" OnRowDataBound="gvwWorkspaceSubjectMst_RowDataBound"
                                                Style="width: 100%;">
                                                <Columns>
                                                    <asp:BoundField HeaderText="Sr No">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vSubjectId" HeaderText="Subject ID">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vMySubjectNo" HeaderText="MySubject No">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
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
                                                    <asp:TemplateField HeaderText="Reason">
                                                        <ItemTemplate>
                                                            <img id="imgShow" runat="server" alt='<%#Eval("vReasonDesc") %>' src="images/collapse.jpg"
                                                                title="Reason" />
                                                            <div id="canal" runat="server" style="display: none; border: outset 2px black; background: white; font: verdana; font-size: 8pt; height: auto;">
                                                                <asp:Label ID="Reason" runat="server" Text='<%# Eval("vReasonDesc") %>' />
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="vUserName" HeaderText="Last Modified BY" />
                                                    <asp:TemplateField HeaderText="Edit" SortExpression="status">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Reject">
                                                        <ItemTemplate>
                                                            <%--<asp:LinkButton ID="lnkReject" runat="server" CommandArgument='<%# Bind("vWorkspaceSubjectId") %>'>Reject</asp:LinkButton>--%>
                                                            <asp:ImageButton ID="lnkReject" runat="server" CommandArgument='<%# Bind("vWorkspaceSubjectId") %>'
                                                                ImageUrl="~/images/reject.png" AlternateText="Reject" ToolTip="Reject" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="vWorkspaceSubjectId" HeaderText="WorkspaceSubjectId" />
                                                    <asp:TemplateField HeaderText="View MSR">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="lnkView" runat="server" ImageUrl="~/images/view.gif" AlternateText="View MSR"
                                                                ToolTip="View MSR" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="cRejectionFlag" HeaderText="cRejectionFlag" />
                                                    <asp:BoundField DataField="iScrDays" HeaderText="Screening Validation Days" />
                                                    <%--'01-Feb-11--%>
                                                    <asp:BoundField DataField="iMySubjectNo" HeaderText="iMySubNo" />
                                                    <asp:TemplateField HeaderText="Delete" SortExpression="status">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgCancel" runat="server" OnClientClick="return msgconfirmalert('Are You Sure You want to Delete attendance of this Subject?',this);"
                                                                ImageUrl="~/images/i_delete.gif" AlternateText="Delete" ToolTip="Delete" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Audit Trail">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgAudit" runat="server" ImageUrl="~/images/audit.png" Visible="true"
                                                                AlternateText="Audit Trail" ToolTip="Audit Trail" /><%--height="20px" Width="20px"/>--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="nMedExScreeningHdrNo" HeaderText="ScrHdr" />
                                                </Columns>
                                            </asp:GridView>
                                            <asp:GridView ID="gvForExcel" runat="server" AutoGenerateColumns="False" SkinID="grdViewAutoSizeMax">
                                                <Columns>
                                                    <asp:BoundField HeaderText="ASN">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vSubjectId" HeaderText="Subject ID">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vMySubjectNo" HeaderText="MySubject No">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
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
                                                    <asp:BoundField DataField="vReasonDesc" HeaderText="Reason"></asp:BoundField>
                                                    <asp:BoundField DataField="vUserName" HeaderText="Last Modified BY" />
                                                    <asp:BoundField DataField="cRejectionFlag" HeaderText="RejectionFlag" />
                                                    <asp:BoundField DataField="iScrDays" HeaderText="Screening Validation Days">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlPeriod" EventName="SelectedIndexChanged" />
                                            <asp:PostBackTrigger ControlID="btnExportToExcel" />
                                            <asp:PostBackTrigger ControlID="btnExporttoPdf" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <script type="text/javascript" language="javascript">

        function fsetAttendanceData_Show() {
            $('#<%=fsetAttendanceData.ClientID%>').attr('style', $('#<%=fsetAttendanceData.ClientID%>').attr('style') + ';display:block');
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
                "sScrollX": "100%",
                "bScrollCollapse": true,
                aLengthMenu: [
                    [10, 25, 50, 100, -1],
                    [10, 25, 50, 100, "All"]
                ],
            });
            return false;
        }

        function UIgvAuditTrail() {
            oTab = $('#<%= GVAuditTrail.ClientID%>').prepend($('<thead>').append($('#<%= GVAuditTrail.ClientID%> tr:first'))).dataTable({
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
            $('#<%= GVAuditTrail.ClientID%>').removeClass("visibletable");
            $('#<%= GVAuditTrail.ClientID%>').removeAttr('style', 'display:block;');
            return true;
        }

        function GVAudit() {
            oTab = $('#<%= GVAudit.ClientID%>').prepend($('<thead>').append($('#<%= GVAudit.ClientID%> tr:first'))).dataTable({
                 "bJQueryUI": true,
                 "sPaginationType": "full_numbers",
                 "bLengthChange": true,
                 "iDisplayLength": 10,
                 "bProcessing": true,
                 "bSort": false,
                 "scrollY": 400,
                 "scrollX": true,
                 aLengthMenu: [
                     [10, 25, 50, 100, -1],
                     [10, 25, 50, 100, "All"]
                 ],
             });
             $('#<%= GVAudit.ClientID%>').removeClass("visibletable");
             $('#<%= GVAudit.ClientID%>').removeAttr('style', 'display:block;');
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

        $(document).ready(function () {
            $('#canal').css('display', 'none');
        })
        // For Project    
        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        //For Subject  
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
                msgalert('Please Select Period !');
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
            if (document.getElementById('<%= ddlReject.ClientId %>').selectedIndex == 0) {
                msgalert('Please Select Reason To Reject !');
                document.getElementById('<%= ddlReject.ClientId %>').focus();
                return false;
            }
            //document.getElementById('<%= btnSave.ClientId %>').click();
            return true;
        }

        function CheckToReject() {
            var Grid = document.getElementById('<%=gvwWorkspaceSubjectMst.clientid %>');
            if (confirm('Are You Sure You want to Reject This Subject')) {
            }
        }

        function ShowAttConfirm(altStr,e) {
            msgConfirmDeleteAlert(null, altStr, function (isConfirmed) {
                if (isConfirmed) {
                    document.getElementById('<%= btnchkScrDate.ClientId %>').click();
                    //__doPostBack(e.name, '');
                    return true;
                } else {
                    return false;
                }
            });
            return false;
        }

        function remarkValidaton() {
            if (document.getElementById('<%= txtLockRemark.ClientId %>').value.trim() == '') {
                msgalert('Please Enter A Valid Remark To Delete !');
                return false;
            }
            else
                return true;
        }

    </script>

</asp:Content>
