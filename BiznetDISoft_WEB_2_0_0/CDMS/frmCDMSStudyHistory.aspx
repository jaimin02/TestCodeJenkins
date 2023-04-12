<%@ Page Language="VB" MasterPageFile="~/ECTDMasterPage.master" AutoEventWireup="false"
    CodeFile="frmCDMSStudyHistory.aspx.vb" Inherits="frmCDMSStudyHistory" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <link rel="stylesheet" href="../App_Themes/CDMS.css" />

    <script src="../Script/General.js" language="javascript" type="text/javascript"></script>

    <script src="../Script/Validation.js" language="javascript" type="text/javascript"></script>

    <script src="../Script/jquery-1.9.1.js" type="text/javascript"></script>

    <script src="../Script/jquery-ui.js" type="text/javascript"></script>

    <script src="../Script/jquery.dataTables.min.js" type="text/javascript"></script>

    <script src="../Script/AutoComplete.js" type="text/javascript"></script>

    <div id="tabs" style="text-align: left; width: 99%">
        <ul>
            <li onclick="fnRedirect();"><a href="#">
                <img alt="Subject Information" src="images/Subject.png" style="padding-right: 8px;" />Subject
                Information</a></li>
            <li><a href="frmCDMSMedicalCondition.aspx?Mode=<%= CType(Me.ViewState(VS_Choice),Integer) %>&SubjectID=<%= Me.ViewState(VS_SubjectID) %>">
                <img alt="Medical Condition" src="images/Medical Condition.png" style="padding-right: 8px;" />Medical
                Condition</a></li>
            <li><a href="frmCDMSConcoMedication.aspx?Mode=<%= CType(Me.ViewState(VS_Choice),Integer) %>&SubjectID=<%= Me.ViewState(VS_SubjectID) %>">
                <img alt="Conco. Medication" src="images/Medication.png" style="padding-right: 8px;" />
                Conco. Medication</a></li>
            <li><a href="#tabs-3">
                <img alt="Study History" src="images/Studyhistory.png" style="padding-right: 8px;" />Study
                History</a></li>
        </ul>
        <div id="tabs-3" align="left">
            <asp:UpdatePanel ID="upStudyHistory" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td colspan="2">
                                <fieldset class="FieldSetBox" style="padding-top: 7px; width: 95%;">
                                    <table cellpadding="2" cellspacing="2" width="100%">
                                        <tr>
                                            <td class="LabelText" style="text-align: left; font-size: 12px !important; width: 12%;">
                                                Subject Id :
                                            </td>
                                            <td class="LabelText" style="width: 64%; text-align: left; font-weight: normal; font-size: 12px;">
                                                <asp:Label ID="lblSubject" runat="server" />
                                            </td>
                                            <td style="width: 23%;">
                                                <asp:Button ID="btnAddMore" runat="server" Text="Add" CssClass="btn btnadd"  OnClientClick="return ValidateAddMore();" />
                                                <asp:Button ID="btnHistory" runat="server" Text="History" CssClass="btn btnadd" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr style="height: 8px;">
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset class="FieldSetBox" style="padding-top: 7px; width: 95%;">
                                    <legend class="LegendText" style="color: Black">Study History</legend>
                                    <table cellpadding="2" cellspacing="2" width="100%">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="grdStudyHistory" runat="server" AutoGenerateColumns="false">
                                                    <Columns>
                                                        <asp:BoundField DataFormatString="number" HeaderText="Sr No">
                                                            <ItemStyle HorizontalAlign="Center" Width="7%" Wrap="true" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" Wrap="true" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vProjectNo" HeaderText="Project No">
                                                            <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vDrugName" HeaderText="Drug Name">
                                                            <ItemStyle HorizontalAlign="Left" Width="20%"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="dStudyStartDate" HeaderText="Start Date" DataFormatString="{0:dd-MMM-yyyy}"
                                                            HtmlEncode="false">
                                                            <ItemStyle HorizontalAlign="Left" Width="12%"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="dStudyEndDate" HeaderText="End Date" DataFormatString="{0:dd-MMM-yyyy}"
                                                            HtmlEncode="false">
                                                            <ItemStyle HorizontalAlign="Left" Width="12%"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vComments" HeaderText="Comments">
                                                            <ItemStyle HorizontalAlign="Left" Width="30%"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Action">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemTemplate>
                                                                <asp:Image ID="imgEdit" runat="server" AlternateText='<% #databinder.eval(container.dataitem,"nSubjectDtlCDMSStudyHistoryNo") %>'
                                                                    ImageUrl="~/CDMS/images/Edit_Small.png" ToolTip="Edit Study History" Style="cursor: pointer;"
                                                                    onclick="fnEditCondition(this.alt);" />
                                                                <asp:Image ID="imgDelete" runat="server" AlternateText='<% #databinder.eval(container.dataitem,"nSubjectDtlCDMSStudyHistoryNo") %>'
                                                                    ImageUrl="~/CDMS/images/delete_small.png" ToolTip="Delete Study History" Style="padding-left: 3px;
                                                                    cursor: pointer;" onclick="fnDeleteCondition(this.alt);" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                                <asp:Button ID="btnFillGrid" runat="server" Style="display: none;" />
                            </td>
                        </tr>
                        <tr style="height: 8px;">
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset class="FieldSetBox" style="padding-top: 7px; width: 95%;">
                                    <table cellpadding="2" cellspacing="2" width="100%">
                                        <tr>
                                            <td class="LabelText" style="text-align: left; font-size: 12px !important;">
                                                <asp:Button ID="btnPrevious" runat="server" Text="Previous" CssClass="btn btnpre" />
                                            </td>
                                            <td class="LabelText" style="text-align: right; font-size: 12px !important;">
                                                <asp:Button ID="btnNext" runat="server" Text="Next" CssClass="btn btnnext" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr style="height: 8px;">
                            <td colspan="2">
                                <asp:Button ID="btnEdit" runat="server" Style="display: none;" />
                                <cc1:ModalPopupExtender ID="mpEditStudyHistory" runat="server" PopupControlID="divEditStudyHistory"
                                    BackgroundCssClass="modalBackground" BehaviorID="mpEditStudyHistory" CancelControlID="imgEditClosePopup"
                                    TargetControlID="btnEdit">
                                </cc1:ModalPopupExtender>
                                <div id="divEditStudyHistory" runat="server" class="centerModalPopup" style="display: none;
                                    overflow: auto; width: 70%; max-height: 250px; min-height: 230px">
                                    <table cellpadding="2" cellspacing="2" width="100%">
                                        <tr>
                                            <td class="LabelText" style="text-align: center !important; font-size: 12px !important;
                                                width: 97%;">
                                                Edit Study History
                                            </td>
                                            <td style="width: 3%">
                                                <img id="imgEditClosePopup" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <hr />
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding="2" cellspacing="2" width="100%">
                                        <tr>
                                            <td class="LabelText" style="text-align: right; width: 20%;">
                                                Study Name* :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtEditproject" runat="server" CssClass="TextBox" Width="85%"></asp:TextBox>
                                            </td>
                                            <td class="LabelText" style="text-align: right;">
                                                Drug Name :
                                            </td>
                                            <td class="LabelText" style="text-align: left; text-align: left;">
                                                <asp:TextBox ID="txtEditdrug" runat="server" CssClass="TextBox" Width="75%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelText" style="text-align: right;">
                                                Start Date :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtEditstartdate" runat="server" CssClass="TextBox" ToolTip="Please enter date in dd-mm-yyyy format (e.g. '01012013'/'01-Jan-2013')"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CalEditstartdate" runat="server" TargetControlID="txtEditstartdate"
                                                    Format="dd-MMM-yyyy">
                                                </cc1:CalendarExtender>
                                            </td>
                                            <td class="LabelText" style="text-align: right;">
                                                End Date :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtEditenddate" runat="server" CssClass="TextBox" ToolTip="Please enter date in dd-mm-yyyy format (e.g. '01012013'/'01-Jan-2013')"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CalEditenddate" runat="server" TargetControlID="txtEditenddate"
                                                    Format="dd-MMM-yyyy">
                                                </cc1:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;" class="LabelText">
                                                Comments :
                                            </td>
                                            <td colspan="3" class="LabelText" style="text-align: left;">
                                                <asp:TextBox ID="txtEditcomments" runat="server" TextMode="MultiLine" CssClass="TextBox"
                                                    Height="60px" Width="79.5%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" style="text-align: center;">
                                                <asp:Button ID="btnUpdateRecord" runat="server" Text="Update" CssClass="btn btnupdate" />
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btncancel" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <asp:Button ID="mdlbtnAddMore" runat="server" Style="display: none;" />
                                <cc1:ModalPopupExtender ID="mpAddStudyHistory" runat="server" PopupControlID="divAddStudyHistory"
                                    BackgroundCssClass="modalBackground" BehaviorID="mpAddStudyHistory" CancelControlID="imgClosePopup"
                                    TargetControlID="mdlbtnAddMore">
                                </cc1:ModalPopupExtender>
                                <div id="divAddStudyHistory" runat="server" class="centerModalPopup" style="display: none;
                                    width: 70%; height: auto;">
                                    <table cellpadding="2" cellspacing="2" width="100%">
                                        <tr>
                                            <td class="LabelText" style="text-align: center !important; font-size: 12px !important;
                                                width: 97%;">
                                                Add New Study History
                                            </td>
                                            <td style="width: 3%">
                                                <img id="imgClosePopup" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <hr />
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding="2" cellspacing="2" width="100%">
                                        <tr>
                                            <td class="LabelText" style="text-align: right; width: 20%;">
                                                Study Name* :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtproject" runat="server" CssClass="TextBox" Width="85%"></asp:TextBox>
                                                <input type="button" id="btnSetProject" style="display: none;" onclick="return ShowStudyDetails();" />
                                                <asp:HiddenField ID="HProjectId" runat="server" />
                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                                                    TargetControlID="txtProject" ServicePath="~/AutoComplete.asmx" OnClientShowing="ClientPopulated"
                                                    OnClientItemSelected="OnSelected" MinimumPrefixLength="1" ServiceMethod="GetMyProjectCompletionList"
                                                    CompletionListElementID="pnlSelectedDate" CompletionListItemCssClass="autocomplete_listitem"
                                                    CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListCssClass="autocomplete_list"
                                                    BehaviorID="AutoCompleteExtender1">
                                                </cc1:AutoCompleteExtender>
                                                <asp:Panel ID="pnlSelectedDate" runat="server" Style="max-height: 150px; overflow: auto;
                                                    overflow-x: hidden;" />
                                            </td>
                                            <td class="LabelText" style="text-align: right;">
                                                Drug Name :
                                            </td>
                                            <td class="LabelText" style="text-align: left; text-align: left;">
                                                <asp:TextBox ID="txtDrug" runat="server" CssClass="TextBox" Width="75%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LabelText" style="text-align: right;">
                                                Start Date :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtStartDate" runat="server" CssClass="TextBox" ToolTip="Please enter date in dd-mm-yyyy format (e.g. '01012013'/'01-Jan-2013')"></asp:TextBox>
                                                <cc1:CalendarExtender ID="calStartDate" runat="server" TargetControlID="txtStartDate"
                                                    Format="dd-MMM-yyyy">
                                                </cc1:CalendarExtender>
                                            </td>
                                            <td class="LabelText" style="text-align: right;">
                                                End Date :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="TextBox" ToolTip="Please enter date in dd-mm-yyyy format (e.g. '01012013'/'01-Jan-2013')"></asp:TextBox>
                                                <cc1:CalendarExtender ID="calEndDate" runat="server" TargetControlID="txtEndDate"
                                                    Format="dd-MMM-yyyy">
                                                </cc1:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;" class="LabelText">
                                                Comments :
                                            </td>
                                            <td colspan="3" class="labeltext" style="text-align: left;">
                                                <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" CssClass="TextBox"
                                                    Height="60px" Width="77%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" style="text-align: center;">
                                                <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btnadd" OnClientClick="return ValidateAdd();" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <asp:Button ID="btnAudit" runat="server" Style="display: none;" />
                                <cc1:ModalPopupExtender ID="mdlAudit" runat="server" PopupControlID="divAudit" BackgroundCssClass="modalBackground"
                                    BehaviorID="mdlAudit" CancelControlID="imgAuditClosePopup" TargetControlID="btnAudit">
                                </cc1:ModalPopupExtender>
                                <div id="divAudit" runat="server" class="centerModalPopup" style="display: none;
                                    overflow: auto; width: 80%; height: auto; max-height: 85%; min-height: auto">
                                    <table cellpadding="2" cellspacing="2" width="100%">
                                        <tr>
                                            <td class="LabelText" style="text-align: center !important; font-size: 12px !important;
                                                width: 97%;">
                                                <asp:Label ID="lblAudit" runat="server" Text="Audit Trail - Study History" />
                                            </td>
                                            <td style="width: 3%">
                                                <img id="imgAuditClosePopup" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <table cellpadding="2" cellspacing="2" width="100%" style="padding: 5px;">
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="grdAudit" runat="server" AutoGenerateColumns="false">
                                                                <Columns>
                                                                    <asp:BoundField DataField="vProjectNo" HeaderText="Project No">
                                                                        <ItemStyle HorizontalAlign="Left" Width="14%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vDrugName" HeaderText="Drug Name">
                                                                        <ItemStyle HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="dStudyStartDate" HeaderText="Start Date" DataFormatString="{0:dd-MMM-yyyy}"
                                                                        HtmlEncode="false">
                                                                        <ItemStyle HorizontalAlign="Center" Width="12%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="dStudyEndDate" HeaderText="End Date" DataFormatString="{0:dd-MMM-yyyy}"
                                                                        HtmlEncode="false">
                                                                        <ItemStyle HorizontalAlign="Center" Width="12%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vComments" HeaderText="Comments" >
                                                                        <ItemStyle HorizontalAlign="Left" Width="25%" Wrap ="true" ></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap ="true" ></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Status">
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></ItemStyle>
                                                                        <ItemTemplate>
                                                                            <asp:HiddenField ID="hdnStatus" runat="server" Value='<% #databinder.eval(container.dataitem,"cStatusIndi") %>' />
                                                                            <asp:HiddenField ID="hdnIdCode" runat="server" Value='<% #databinder.eval(container.dataitem,"vWorkSpaceID") %>' />
                                                                            <asp:Label ID="lblStatus" runat="server" Style="font-family: Calibri, sans-serif !important;
                                                                                font-size: 12px !important;" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Audit">
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%"></ItemStyle>
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="imgAudit" runat="server" ImageUrl="~/CDMS/images/Audit_Small.png"
                                                                                ToolTip="Audit Trail" Style="cursor: pointer;" />
                                                                            <asp:Label ID="lblAuditMark" runat="server" Text="*" CssClass="LabelText" Style="color: Red !important;
                                                                                font-size: 16px;" Visible="false" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
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
                                        <tr>
                                            <td style="height: 10px;">
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <asp:Button ID="btnRowAudit" runat="server" Style="display: none;" />
                                <cc1:ModalPopupExtender ID="mdlRowAudit" runat="server" PopupControlID="divRowAudit"
                                    BackgroundCssClass="modalBackground" BehaviorID="mdlRowAudit" CancelControlID="imgRowAuditClose"
                                    TargetControlID="btnRowAudit">
                                </cc1:ModalPopupExtender>
                                <div id="divRowAudit" runat="server" class="centerModalPopup" style="display: none;
                                    overflow: auto; width: 95%; height: auto; max-height: 80%; min-height: auto">
                                    <table cellpadding="2" cellspacing="2" width="100%">
                                        <tr>
                                            <td class="LabelText" style="text-align: center !important; font-size: 12px !important;
                                                width: 97%;">
                                                <asp:Label ID="lblRowAudit" runat="server" Text="Record Audit Trail - Study History" />
                                            </td>
                                            <td style="width: 3%">
                                                <img id="imgRowAuditClose" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <table cellpadding="2" cellspacing="2" width="100%" style="padding: 5px;">
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="grdRowAudit" runat="server" AutoGenerateColumns="false">
                                                                <Columns>
                                                                    <asp:BoundField DataField="vProjectNo" HeaderText="Project No">
                                                                        <ItemStyle HorizontalAlign="Left" Width="8%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vDrugName" HeaderText="Drug Name">
                                                                        <ItemStyle HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="dStudyStartDate" HeaderText="Start Date" DataFormatString="{0:dd-MMM-yyyy}"
                                                                        HtmlEncode="false">
                                                                        <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="dStudyEndDate" HeaderText="End Date" DataFormatString="{0:dd-MMM-yyyy}"
                                                                        HtmlEncode="false">
                                                                        <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vComments" HeaderText="Comments">
                                                                        <ItemStyle HorizontalAlign="Left" Width="15%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vRemarks" HeaderText="Reason">
                                                                        <ItemStyle HorizontalAlign="Left" Width="15%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="UserName" HeaderText="Modify By">
                                                                        <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="dModifyOffSet" HeaderText="Modify On">
                                                                        <ItemStyle HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                </Columns>
                                                            </asp:GridView>
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
                                        <tr>
                                            <td style="height: 10px;">
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnUpdateRecord" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <asp:Button ID="btnmdlSave" runat="server" Style="display: none;" />
        <cc1:ModalPopupExtender ID="mdlSaveAlert" runat="server" PopupControlID="divSaveAlert"
            BackgroundCssClass="modalBackground" BehaviorID="mdlSaveAlert" TargetControlID="btnmdlSave">
        </cc1:ModalPopupExtender>
        <div id="divSaveAlert" runat="server" class="centerModalPopup" style="display: none;
            overflow: auto; width: 35%; height: auto; max-height: 25%; min-height: auto;">
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td id="SaveAlertHeader" class="LabelText" style="text-align: left !important; font-size: 12px !important;
                        width: 97%;">
                        Save Warning
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td id="SaveAlertMessage" class="LabelText" style="text-align: center !important;">
                        Are you sure you want to add study details for this subject ?
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btnSaveAdd" runat="server" Text="Add" CssClass="ButtonText" Width="57px"
                            Style="font-size: 12px !important;" ToolTip="Add Conco. Medication" />
                        <asp:Button ID="btnSaveCancel" runat="server" Text="Cancel" CssClass="ButtonText"
                            Width="66px" Style="font-size: 12px !important;" ToolTip="Cancel Conco. Medication" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btnDelete" runat="server" Style="display: none;" />
        <cc1:ModalPopupExtender ID="mdlDelete" runat="server" PopupControlID="divDelete"
            BackgroundCssClass="modalBackground" BehaviorID="mdlDelete" CancelControlID="btnNo"
            TargetControlID="btnDelete">
        </cc1:ModalPopupExtender>
        <div id="divDelete" runat="server" class="centerModalPopup" style="display: none;
            overflow: auto; width: 35%; height: auto; max-height: 25%; min-height: auto;">
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td id="AlertHeader" class="LabelText" style="text-align: left !important; font-size: 12px !important;
                        width: 97%;">
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td id="AlertMessage" class="LabelText" style="text-align: center !important;">
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btnYes" runat="server" Text="Yes" CssClass="btn btnyes" 
                            Style=" display: inline;" />
                        <asp:Button ID="btnNo" runat="server" Text="No" CssClass="btn btnno" 
                            Style=" display: inline;" />
                        <asp:Button ID="btnOk" runat="server" Text="Ok" CssClass="btn btnyes" 
                            Style=" display: none;" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btnRemarks" runat="server" Style="display: none;" />
        <cc1:ModalPopupExtender ID="mdlRemarks" runat="server" PopupControlID="divRemarks"
            BackgroundCssClass="modalBackground" BehaviorID="mdlRemarks" CancelControlID="btnRemarksCancel"
            TargetControlID="btnRemarks">
        </cc1:ModalPopupExtender>
        <div id="divRemarks" runat="server" class="centerModalPopup" style="display: none;
            overflow: auto; width: 32%; height: auto; max-height: 45%; min-height: auto;">
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td class="LabelText" style="text-align: left !important; font-size: 12px !important;
                        width: 97%;">
                        Remarks
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td class="LabelText" style="text-align: left !important;">
                        Enter Remarks:
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left !important;">
                        <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="5" Height="60px"
                            CssClass="TextBox" Width="300px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btnRemarksDelete" runat="server" Text="Delete" CssClass="btn btndelete" Style="display: inline;" />
                        <asp:Button ID="btnRemarksUpdate" runat="server" Text="Update" CssClass="btn btnupdate" Style="display: none;" />
                        <asp:Button ID="btnRemarksCancel" runat="server" Text="Cancel" CssClass="btn btncancel" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btnWarning" runat="server" Style="display: none;" />
        <cc1:ModalPopupExtender ID="mdlWarning" runat="server" PopupControlID="divWarning"
            BackgroundCssClass="modalBackground" BehaviorID="mdlWarning" TargetControlID="btnWarning">
        </cc1:ModalPopupExtender>
        <div id="divWarning" runat="server" class="centerModalPopup" style="display: none;
            overflow: auto; width: 35%; height: auto; max-height: 25%; min-height: auto;">
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td id="WarningHeader" class="LabelText" style="text-align: left !important; font-size: 12px !important;
                        width: 97%;">
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td id="WarningMessage" class="LabelText" style="text-align: center !important;">
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btnWarningOk" runat="server" Text="Ok" CssClass="btn btnadd" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="hdnStudyHistoryNo" runat="server" />
        <asp:HiddenField ID="hdnDrugName" runat="server" />
    </div>

    <script type="text/javascript">
    
        $(function() {
            $('.InnerTable').parent().attr('align','left')
            $('.InnerTable').parent().css('padding-left','4px');
            $( "#tabs" ).tabs().addClass( "ui-tabs-vertical ui-helper-clearfix" );
            $( "#tabs li" ).removeClass( "ui-corner-top" ).addClass( "ui-corner-left" );
            $( "#tabs" ).tabs({active:3});
            $('#tabs ul li a').click(function () {
                    location.href = this.href;
            });
            
            fnApplyDataTable();
            
      
            $('#<%= btnWarningOk.ClientID %>').unbind('click').click(function(){$find('mdlWarning').hide();  return false;});
            
            $('#<%= btnRemarksUpdate.ClientID %>').unbind('click').click(function(){
                         if($("[id$='_txtRemarks']").val().trim() == "")
                         {
                                $find('mdlDelete').show();
                                $find('mdlRemarks').hide();
                                $('#AlertHeader').text('Remarks Warning');
                                $('#AlertMessage').text('Please enter remarks.');
                                $('#<%= btnYes.ClientID %>').css('display','none');
                                $('#<%= btnNo.ClientID %>').css('display','none');
                                $('#<%= btnOk.ClientID %>').css('display','inline');
                                return false;
                         }
                         else
                         {
                                return true;
                         }
                });
                
                    
                $('#<%= btnOk.ClientID %>').unbind('click').click(function(event){ 
                                                        //event.preventDefault();
                                                        $find('mdlDelete').hide();
                                                        $find('mdlRemarks').show();
                                                        $('#AlertHeader').text('');
                                                        $('#AlertMessage').text('');
                                                        $('#<%= btnYes.ClientID %>').css('display','inline');
                                                        $('#<%= btnNo.ClientID %>').css('display','inline');
                                                        $('#<%= btnOk.ClientID %>').css('display','none');
                                                        return false;
                                                  });
                                                  
                                                          
            
        });
        
        function ClientPopulated(sender, e) {

            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), $("#btnSetProject"));
        }

        
        function fnApplyDataTable()
            {
            
                 $('#<%= grdStudyHistory.ClientID %>').prepend($('<thead>').append($('#<%= grdStudyHistory.ClientID %> tr:first'))).dataTable({
                           "bStateSave": false,
                           "bPaginate": true,
                           "sPaginationType": "full_numbers",
                           "bSort": true,
                           "bDestory": true,
                           "bRetrieve": true
                });
                     $('#<%= grdStudyHistory.ClientID %> tr:first').css('background-color','#3A87AD');  
            
                 $('#<%= grdAudit.ClientID %>').prepend($('<thead>').append($('#<%= grdAudit.ClientID %> tr:first'))).dataTable({
                    "bPaginate": true,
                    "sPaginationType": "full_numbers",
                    "bSort": true,
                    "bDestory": true,
                    "bRetrieve": true
                  
                 });
                
                 $('#<%= grdAudit.ClientID %> tr:first').css('background-color','#3A87AD');  
                 
                 $('#<%= grdRowAudit.ClientID %>').prepend($('<thead>').append($('#<%= grdRowAudit.ClientID %> tr:first'))).dataTable({
                    "bPaginate": true,
                    "sPaginationType": "full_numbers",
                    "bSort": true,
                    "bDestory": true,
                    "bRetrieve": true
                });
                
                 $('#<%= grdRowAudit.ClientID %> tr:first').css('background-color','#3A87AD');             
                 
                $('#<%= btnNext.ClientID %>').unbind('click').click(function(){ 
                
                                    if(fnGetQueryString('SubjectID') == "")
                                    {
                                        location.href="frmCDMSSubjectInformation.aspx?Mode=1";
                                    }
                                    else
                                    {
                                        location.href="frmCDMSSubjectInformation.aspx?Mode=" + fnGetQueryString("Mode") + "&SubjectID=" + fnGetQueryString("SubjectID");
                                    }
                                
                                
                 });
                $('#<%= btnPrevious.ClientID %>').unbind('click').click(function(){ 
                                    if(fnGetQueryString('SubjectID') == "")
                                    {
                                        location.href="frmCDMSConcoMedication.aspx?Mode=2&SubjectID=";
                                    }
                                    else
                                    {
                                        location.href="frmCDMSConcoMedication.aspx?Mode=" + fnGetQueryString("Mode") + "&SubjectID=" + fnGetQueryString("SubjectID");
                                    }
                                    
                });
                
            }
            
             function fnRedirect()
            {
                    if(fnGetQueryString('SubjectID') == undefined || fnGetQueryString('SubjectID') == "")
                    {
                        window.location.href = "frmCDMSSubjectInformation.aspx?Mode=1";
                    }
                    else{
                        window.location.href = "frmCDMSSubjectInformation.aspx?Mode=2&SubjectID=" + fnGetQueryString('SubjectID') ;
                    }
                     
            }
            
            function fnGetQueryString(key, default_)
            {
                if (default_ == null) default_ = "";
                key = key.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
                var regex = new RegExp("[\\?&]" + key + "=([^&#]*)");
                var qs = regex.exec(window.location.href);
                if (qs == null)
                    return default_;
                else
                    return qs[1];
            }
            
            
            function fnDeleteCondition(StudyHistoryNo)
            {
                $("[id$='_txtRemarks']").val('');
                $('#AlertHeader').text('Delete Confirmation');
                $('#AlertMessage').text('Are you sure you want to delete this Study History ?');
                $('#<%= btnYes.ClientID %>').css('display','inline');
                $('#<%= btnNo.ClientID %>').css('display','inline');
                $('#<%= btnOk.ClientID %>').css('display','none');
                $find('mdlDelete').show();
                $('#<%= btnYes.ClientID %>').unbind('click').click(function(){ 
                                                        //event.preventDefault();
                                                        $('#AlertHeader').text('');
                                                        $('#AlertMessage').text('');
                                                        $('#<%= btnYes.ClientID %>').css('display','inline');
                                                        $('#<%= btnNo.ClientID %>').css('display','inline');
                                                        $('#<%= btnOk.ClientID %>').css('display','none');
                                                        $find('mdlDelete').hide();
                                                        $('#<%= btnRemarksDelete.ClientID %>').css('display','inline');
                                                        $('#<%= btnRemarksUpdate.ClientID %>').css('display','none')
                                                        $find('mdlRemarks').show();
                                                        return false;
                                                  });
                                                  
                $('#<%= btnRemarksDelete.ClientID %>').unbind('click').click(function(){ 
                                                        //event.preventDefault();
                                                        var content = {};
                                                        content.SubjectId = fnGetQueryString('SubjectID');
                                                        content.StudyHistoryNo = StudyHistoryNo;
                                                        content.Remarks =  $("[id$='_txtRemarks']").val().trim();
                                                         if($("[id$='_txtRemarks']").val().trim() == "")
                                                        {
                                                                $find('mdlDelete').show();
                                                                $find('mdlRemarks').hide();
                                                                $('#AlertHeader').text('Remarks Warning');
                                                                $('#AlertMessage').text('Please enter remarks.');
                                                                $('#<%= btnYes.ClientID %>').css('display','none');
                                                                $('#<%= btnNo.ClientID %>').css('display','none');
                                                                $('#<%= btnOk.ClientID %>').css('display','inline');
                                                        }
                                                        else
                                                        {
                                                                $.ajax({
                                                                          type: "POST",
                                                                          url: "frmCDMSStudyHistory.aspx/DeleteStudyHistory",
                                                                          data: JSON.stringify(content),          
                                                                          contentType: "application/json; charset=utf-8",
                                                                          dataType: "json",
                                                                          success: function(data) {
                                                                                if(data.d == "Success")
                                                                                {
                                                                                    $find('mdlRemarks').hide();
                                                                                    $('#<%= btnFillGrid.ClientID %>').click();
                                                                                }
                                                                                else
                                                                                {
                                                                                    msgalert(data.d);
                                                                                }
                                                                          },
                                                                           failure: function(error) {
                                                                                    msgalert(error);
                                                                                }
                                                                      });
                                                        }
                                                        return false;
                                                  });                                                 
                
                                                 
            }
            
            function fnEditCondition(StudyHistoryNo)
            {
            
                $('#<%= hdnStudyHistoryNo.ClientID %>').val(StudyHistoryNo);
                $('#<%= txtEditproject.ClientID %>').val("");
                $('#<%= HProjectId.ClientID %>').val("");
                $('#<%= txtEditdrug.ClientID %>').val("");
                $('#<%= txtEditstartdate.ClientID %>').val("");
                $('#<%= txtEditenddate.ClientID %>').val("");
                $('#<%= txtEditcomments.ClientID %>').val("");
                $("[id$='_txtRemarks']").val('');
                var content = {};
                content.SubjectId = fnGetQueryString('SubjectID');
                content.StudyHistoryNo = StudyHistoryNo;
                $.ajax({
                           type: "POST",
                           url: "frmCDMSStudyHistory.aspx/EditStudyHistory",
                           data: JSON.stringify(content),          
                           contentType: "application/json; charset=utf-8",
                           dataType: "json",
                           success: function(data) {
                                        if(data.d != undefined || data.d != null)
                                        {
                                        
                                            var dataStudy = $.parseJSON(data.d);
                                             $('#<%= txtEditproject.ClientID %>').val(dataStudy[0].vProjectNo);
                                             $('#<%= HProjectId.ClientID %>').val(dataStudy[0].vWorkSpaceId);
                                             $('#<%= txtEditdrug.ClientID %>').val(dataStudy[0].vDrugName);
                                             $('#<%= txtEditstartdate.ClientID %>').val(dataStudy[0].dStudyStartDate);
                                             $('#<%= txtEditenddate.ClientID %>').val(dataStudy[0].dStudyEndDate);
                                             $('#<%= txtEditcomments.ClientID %>').val(dataStudy[0].vComments);
                                             $('#<%= txtEditDrug.ClientID %>').attr("disabled", "disabled");
                                             $('#<%= txtEditproject.ClientID %>').attr("disabled", "disabled");
                                           
                                            $find('mpEditStudyHistory').show();
                                        }
                                    },
                           failure: function(error) {
                                       msgalert(error);
                                    }
                });
                
                 $('#<%= btnUpdateRecord.ClientID %>').unbind('click').click(function(){ 
                                                        $('#<%= btnRemarksDelete.ClientID %>').css('display','none');
                                                        $('#<%= btnRemarksUpdate.ClientID %>').css('display','inline');                                       
                                                        $find('mdlRemarks').show();
                                                        return false;
                                                  });
                
                $('#<%= btnCancel.ClientID %>').unbind('click').click(function(){ 
                                                        //event.preventDefault(); 
                                                        $find('mpEditStudyHistory').hide();
                                                        return false;
                                                  });
                
                
            }
            
    function ValidateAdd()
      {   
           if ($('#<%= HProjectId.ClientID %>').val() == 0) {
                $find('mdlWarning').show();
                $('#WarningHeader').text('Warning');
                $('#WarningMessage').text('Please Enter Project.');
                $('#ctl00_CPHLAMBDA_txtproject').val() == "";
                $('#ctl00_CPHLAMBDA_txtproject').focus()
                return false;
            }
            
            else if ($('#<%= txtDrug.ClientID %>').val().length <= 0) {
                $find('mdlWarning').show();
                $('#WarningHeader').text('Warning');
                $('#WarningMessage').text('Please Enter Drug Name.');
                $('#ctl00_CPHLAMBDA_txtDrug').val() == " ";
                $('#ctl00_CPHLAMBDA_txtDrug').focus();
                return false;
            }
            else if ($('#<%= txtStartDate.ClientID %>').val().length <= 0) {
                $find('mdlWarning').show();
                $('#WarningHeader').text('Warning');
                $('#WarningMessage').text('Please Enter Start Date.');
                $('#ctl00_CPHLAMBDA_txtStartDate').val() == " ";
                $('#ctl00_CPHLAMBDA_txtStartDate').focus();
                return false;
            }
            else if ($('#<%= txtEndDate.ClientID %>').val().length <= 0) {
                $find('mdlWarning').show();
                $('#WarningHeader').text('Warning');
                $('#WarningMessage').text('Please Enter End Date.');
                $('#ctl00_CPHLAMBDA_txtEndDate').val() == " ";
                $('#ctl00_CPHLAMBDA_txtEndDate').focus();
                return false;
            }
            else if ($('#<%= txtComments.ClientID %>').val().length <= 0) {
                $find('mdlWarning').show();
                $('#WarningHeader').text('Warning');
                $('#WarningMessage').text('Please Enter Comments.');
                $('#ctl00_CPHLAMBDA_txtComments').val() == " ";
                $('#ctl00_CPHLAMBDA_txtComments').focus();
                return false;
            }
         $('#<%= hdnDrugName.ClientID %>').val($('#<%= txtDrug.ClientID %>').val());   
         $find('mdlSaveAlert').show();
         return false;
      }
      
      function ValidateAddMore()
        {
        
           if ($('#<%= lblSubject.ClientID %>').html() == "")
           {
              $find('mdlWarning').show();
              $('#WarningHeader').text('Warning');
              $('#WarningMessage').text('No Subject Found.');
              return false ;
           }
            $('#<%= txtproject.ClientID %>').val("");
            $('#<%= HProjectId.ClientID %>').val("");
            $('#<%= txtDrug.ClientID %>').val("")
            $('#<%= txtStartDate.ClientID %>').val("")
            $('#<%= txtEndDate.ClientID %>').val("")
            $('#<%= txtComments.ClientID %>').val("")
           $find('mpAddStudyHistory').show();
           return false ;
        }

    function ShowStudyDetails()
     {
            $('#<%= txtDrug.ClientID %>').val("")
            $('#<%= txtStartDate.ClientID %>').val("")
            $('#<%= txtEndDate.ClientID %>').val("")
            $('#<%= txtComments.ClientID %>').val("")
            
            var content = {};
            content.wstr = "SELECT * FROM View_StudyInformationDtl WHERE vWorkSpaceId='" + $('#<%= HProjectId.ClientID %>').val() + "' And cStatusIndi <> 'D'";

            $.ajax(
                    {
                        type: "POST",
                        url: "frmCDMSStudyHistory.aspx/ShowStudyHistory",
                        data: JSON.stringify(content),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            var dataStudy = $.parseJSON(data.d);
                            $('#<%= txtDrug.ClientID %>').val(dataStudy[0].vDrugName);
                            if (dataStudy[0].dStartDate != null && dataStudy[0].dStartDate != null)
                            {
                               $('#<%= txtStartDate.ClientID %>').val(dataStudy[0].dStartDate.toString("dd-MMM-yyyy"));
                               $('#<%= txtEndDate.ClientID %>').val(dataStudy[0].dEndDate.toString("dd-MMM-yyyy"));
                            }
                            $('#<%= txtDrug.ClientID %>').attr("disabled", "disabled");
                        },
                        failure: function (error) {
                            msgalert(error);
                        }
                    });
     }       
        
               
            //Added For UK-UNK-UKUK IN Calendar
  var inyear;
  function DateConvertForScreening(ParamDate,txtdate)
        {
        
         if (ParamDate.length == 0)
           {
               return true;
           }
           
         if (ParamDate.trim() != '') {
        
              var dt = ParamDate.trim().toUpperCase();
              var tempdt;
              if (dt.indexOf('UK') >= 0 || dt.indexOf('UNK') >= 0 || dt.indexOf('UKUK') >= 0) {

               if (dt.length < 8) {
                msgalert('Please Enter Date In DDMMYYYY Or dd-Mon-YYYY Format Only.'); 
                txtdate.value = "";
                txtdate.focus();
                return false;
                }
                var day;
                var month;
                var year;
                if (dt.indexOf('-') >= 0) {
                var arrDate = dt.split('-');
                day = arrDate[0];
                month = arrDate[1];
                year = arrDate[2];
                }
                else {
                day = dt.substr(0, 2);
                month = dt.substr(2, 2);
                year = dt.substr(4, 4);
                if (dt.indexOf('UNK') >= 0) {
                    month = dt.substr(2, 3);
                    year = dt.substr(5, 4);
                }
                if (dt.indexOf('UNK') == -1) {
                    month = dt.substr(2, 2);
                    year = dt.substr(4, 5);
                }
            }
               inyear = parseInt(year, 10);
               
            if (day.length > 2 && day.length != 0) {
                msgalert('Please Enter Date In DDMMYYYY Or dd-Mon-YYYY Format Only.');
                txtdate.value = "";
                txtdate.focus();
                return false;
            }
            if (month.length > 3 && month.length != 3) {
                msgalert('Please Enter Date In DDMMYYYY Or dd-Mon-YYYY Format Only.');
                txtdate.value = "";
                txtdate.focus();
                return false;
            }
            if (year.length > 4 && month.length != 4) {
                msgalert('Please Enter Date In DDMMYYYY Or dd-Mon-YYYY Format Only.');
                txtdate.value = "";
                txtdate.focus();
                return false;
            }
            if (day == 'UK') {
                tempdt = '01';
            }
            else {
                tempdt = day;
            }
            if (dt.indexOf('-') >= 0) {
                tempdt += '-';
            }
            if (month == 'UNK') {
                tempdt += '01';
            }
            else {
                tempdt += month;
            }
            if (dt.indexOf('-') >= 0) {
                tempdt += '-';
            }
            if (year == 'UKUK') {
                tempdt += '1800';
            }
            else {
                tempdt += year;
            }
            var chk = false;
            chk = DateConvert(tempdt, txtdate);
            if (chk == true) {
                if (isNaN(month)) {
                    txtdate.value = day + '-' + month + '-' + year;
                }
                else {
                    txtdate.value = day + '-' + cMONTHNAMES[month - 1] + '-' + year;
                }
                if (inyear < 1900)
               {
                    msgalert('You Can Not Add Date Which Is Less Than "01-Jan-1900"  ');
                    txtdate.value = "";
                    txtdate.focus();
                    return false ;
               }
                return true;
            }
            txtdate.value = "";
            txtdate.focus();
            return false;
        }
     }
            DateConvert(txtdate.value, txtdate);
             dt = txtdate.value;
             var Year = dt.substring(dt.lastIndexOf('-') + 1);
             inyear = parseInt(Year, 10);
             if (inyear < 1900)
              {
                    msgalert('You Can Not Add Date Which Is Less Than "01-Jan-1900"  ');
                    txtdate.value = "";
                    txtdate.focus();
                    return false ;
              }
            if ($('#ctl00_CPHLAMBDA_txtStartDate').is(":visible") == true )  
            {
                  var fromdate=document.getElementById('ctl00_CPHLAMBDA_txtStartDate');
                  var todate = document.getElementById('ctl00_CPHLAMBDA_txtEndDate');    
                  if (fromdate.value != "" && todate.value != "" ){
                  var gap=GetDateDifference(fromdate.value, todate.value);
                  if (gap.Days  == 0 || gap.Days  < 0){
                     msgalert('End Date Must Not Be Less Than From Date');    
                     todate.value = "";
                 }
                } 
            }
            else {
            var fromdate=document.getElementById('ctl00_CPHLAMBDA_txtEditstartdate');
            var todate = document.getElementById('ctl00_CPHLAMBDA_txtEditenddate');    
            if (fromdate.value != "" && todate.value != "" ){
            var gap=GetDateDifference(fromdate.value, todate.value);
              if (gap.Days  == 0 || gap.Days  < 0){
                 msgalert('End Date Must Not Be Less Than From Date');    
                 todate.value = "";
               }
           }
          }   
     return true ;  
 }

    </script>

</asp:Content>
