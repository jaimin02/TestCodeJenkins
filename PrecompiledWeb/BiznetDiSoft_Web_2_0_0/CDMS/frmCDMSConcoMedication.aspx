<%@ page language="VB" masterpagefile="~/ECTDMasterPage.master" autoeventwireup="false" inherits="CDMS_frmCDMSConcoMedication, App_Web_4wz2dz2v" title=":: CDMS - Conco. Medication ::" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <link rel="stylesheet" href="../App_Themes/CDMS.css" />

    <script src="../Script/General.js" language="javascript" type="text/javascript"></script>

    <script src="../Script/Validation.js" language="javascript" type="text/javascript"></script>

    <script src="../Script/jquery-1.9.1.js" type="text/javascript"></script>

    <script src="../Script/jquery-ui.js" type="text/javascript"></script>

    <script src="../Script/jquery.dataTables.min.js" type="text/javascript"></script>

    <div id="tabs" style="text-align: left; width: 99%">
        <ul>
            <li><li onclick="fnRedirect();"><a href="#">
                <img alt="Subject Information" src="images/Subject.png" style="padding-right: 8px;" />Subject
                Information</a></li>
            <li><a href="frmCDMSMedicalCondition.aspx?Mode=<%= CType(Me.ViewState(VS_Choice),Integer) %>&SubjectID=<%= Me.ViewState(VS_SubjectID) %>">
                <img alt="Medical Condition" src="images/Medical Condition.png" style="padding-right: 8px;" />Medical
                Condition</a></li>
            <li><a href="#tabs-2">
                <img alt="Conco. Medication" src="images/Medication.png" style="padding-right: 8px;" />
                Conco. Medication</a></li>
            <li><a href="frmCDMSStudyHistory.aspx?Mode=<%= CType(Me.ViewState(VS_Choice),Integer) %>&SubjectID=<%= Me.ViewState(VS_SubjectID) %>">
                <img alt="Study History" src="images/Studyhistory.png" style="padding-right: 8px;" />Study
                History</a></li>    
        </ul>
        <div id="tabs-2" align="left">
            <asp:UpdatePanel ID="upConcoMedi" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td colspan="2">
                                <fieldset class="FieldSetBox" style="padding-top: 7px; width: 95%;">
                                    <table cellpadding="2" cellspacing="2" width="100%">
                                        <tr>
                                            <td class="LabelText" style="text-align: left; font-size: 12px !important; width :12%;">
                                                Subject Id :
                                            </td>
                                            <td class="LabelText" style="width: 66%; text-align: left; font-weight: normal; font-size: 12px;">
                                                <asp:Label ID="lblSubjectConcoMedi" runat="server" />
                                            </td>
                                            <td style="width :23%;">
                                                <asp:Button ID="btnAddMore" runat="server" Text="Add" CssClass="btn btnadd"  />
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
                                    <legend class="LegendText" style="color: Black">Conco. Medication</legend>
                                    <table cellpadding="2" cellspacing="2" width="100%">
                                        <tr>
                                            <td>
                                                <asp:HiddenField ID="hdnConcoMedicationNo" runat="server" />
                                                <asp:GridView ID="grdConcoMedi" runat="server" AutoGenerateColumns="false">
                                                    <Columns>
                                                        <asp:BoundField DataField="vIdCode" HeaderText="Code">
                                                            <ItemStyle HorizontalAlign="Left" Width="16%"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vClass" HeaderText="Class">
                                                            <ItemStyle HorizontalAlign="Left" Width="17%"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vDescription" HeaderText="Description">
                                                            <ItemStyle HorizontalAlign="Left" Width="14%"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vDosage" HeaderText="Dosage">
                                                            <ItemStyle HorizontalAlign="Left" Width="17%"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="dStartDate" HeaderText="Start Date" DataFormatString="{0:dd-MMM-yyyy}"
                                                            HtmlEncode="false">
                                                            <ItemStyle HorizontalAlign="Left" Width="12%"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="dEndDate" HeaderText="End Date" DataFormatString="{0:dd-MMM-yyyy}"
                                                            HtmlEncode="false">
                                                            <ItemStyle HorizontalAlign="Left" Width="12%"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Action">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemTemplate>
                                                                <asp:Image ID="imgEdit" runat="server" AlternateText='<% #databinder.eval(container.dataitem,"nSubjectDtlCDMSConcoMedicationNo") %>'
                                                                    ImageUrl="~/CDMS/images/Edit_Small.png" ToolTip="Edit Medication" Style="cursor: pointer;"
                                                                    onclick="fnEditCondition(this.alt);" />
                                                                <asp:Image ID="imgDelete" runat="server" AlternateText='<% #databinder.eval(container.dataitem,"nSubjectDtlCDMSConcoMedicationNo") %>'
                                                                    ImageUrl="~/CDMS/images/delete_small.png" ToolTip="Delete Medication" Style="padding-left: 3px;
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
                                <cc1:ModalPopupExtender ID="mpEditConcoMedi" runat="server" PopupControlID="divEditConcoMedi"
                                    BackgroundCssClass="modalBackground" BehaviorID="mpEditConcoMedi" CancelControlID="imgEditClosePopup"
                                    TargetControlID="btnEdit">
                                </cc1:ModalPopupExtender>
                                <div id="divEditConcoMedi" runat="server" class="centerModalPopup" style="display: none;
                                    overflow: auto; width: 680px; ">
                                    <table cellpadding="2" cellspacing="2" width="100%">
                                        <tr>
                                            <td class="LabelText" style="text-align: center !important; font-size: 12px !important;
                                                width: 97%;">
                                                Edit Conco. Medication
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
                                        <tr>
                                            <td colspan="2">
                                                <table cellpadding="2" cellspacing="2" width="100%" style="padding: 5px;">
                                                    <tr>
                                                        <td class="LabelText">
                                                            Code :
                                                        </td>
                                                        <td style="text-align: left">
                                                            <asp:TextBox ID="txtCode" runat="server" CssClass="TextBox" Width="210px" TabIndex="1"/>
                                                        </td>
                                                        <td class="LabelText">
                                                            Class :
                                                        </td>
                                                        <td style="text-align: left">
                                                            <asp:TextBox ID="txtClass" runat="server" CssClass="TextBox" Width="219px" TabIndex="2"/>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="LabelText">
                                                            Start Date :
                                                        </td>
                                                        <td style="text-align: left">
                                                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="TextBox" ToolTip="Please enter date in dd-mm-yyyy format (e.g. '01012013'/'01-Jan-2013')" TabIndex="3"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="calStartDate" runat="server" TargetControlID="txtStartDate"
                                                                Format="dd-MMM-yyyy">
                                                            </cc1:CalendarExtender>
                                                        </td>
                                                        <td class="LabelText">
                                                            End Date :
                                                        </td>
                                                        <td style="text-align: left">
                                                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="TextBox" ToolTip="Please enter date in dd-mm-yyyy format (e.g. '01012013'/'01-Jan-2013')" TabIndex="4"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="calEndDate" runat="server" TargetControlID="txtEndDate"
                                                                Format="dd-MMM-yyyy">
                                                            </cc1:CalendarExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="LabelText">
                                                            Dosage :
                                                        </td>
                                                        <td class="LabelText" style="text-align: left;">
                                                            <asp:TextBox ID="txtDosage" runat="server" CssClass="TextBox" Width="210px" TabIndex="5"/>
                                                        </td>
                                                        <td class="LabelText">
                                                            Confimed :
                                                        </td>
                                                        <td class="LabelText" style="text-align: left;">
                                                            <asp:DropDownList ID="ddlConfirmed" runat="server" Width="110px" TabIndex="6">
                                                                <asp:ListItem Text="Yes" Value="Y" />
                                                                <asp:ListItem Text="No" Value="N" />
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="LabelText" style="vertical-align: top;">
                                                            Comments :
                                                        </td>
                                                        <td colspan="3" style="text-align: left;">
                                                            <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Rows="5" Height="60px"
                                                                CssClass="TextBox" Width="541px" TabIndex="7"/>
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
                                        <tr style="height: 8px;">
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: center;">
                                                <asp:Button ID="btnUpdateRecord" runat="server" Text="Update" CssClass="btn btnupdate" TabIndex="8"/>
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btncancel" TabIndex="9"/>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <cc1:ModalPopupExtender ID="mpConcoMedi" runat="server" PopupControlID="divConcoMedi"
                                    BackgroundCssClass="modalBackground" BehaviorID="mpConcoMedi" CancelControlID="imgClosePopup"
                                    TargetControlID="btnAddMore">
                                </cc1:ModalPopupExtender>
                                <div id="divConcoMedi" runat="server" class="centerModalPopup" style="display: none;
                                    overflow: auto; width: 80%; height: auto; max-height: 85%; min-height: auto">
                                    <table cellpadding="2" cellspacing="2" width="100%">
                                        <tr>
                                            <td class="LabelText" style="text-align: center !important; font-size: 12px !important;
                                                width: 97%;">
                                                Add New Conco. Medication
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
                                        <tr>
                                            <td colspan="2">
                                                <table cellpadding="2" cellspacing="2" width="100%" style="padding: 5px;">
                                                    <tr>
                                                        <td>
                                                            <asp:HiddenField ID="hdnSelectedConco" runat="server" />
                                                            <asp:GridView ID="grdAddConcoMedi" runat="server" AutoGenerateColumns="false">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Select">
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%"></ItemStyle>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkCodeList" runat="server" onclick="fnCheckBokSelection(this);" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="CodeClassOfMedication" HeaderText="Code">
                                                                        <ItemStyle HorizontalAlign="Center" Width="12%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Class" HeaderText="Class">
                                                                        <ItemStyle HorizontalAlign="Left" Width="14%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Description" HeaderText="Description">
                                                                        <ItemStyle HorizontalAlign="Left" Width="14%"></ItemStyle>
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
                                            <td colspan="2" style="text-align: center;">
                                                <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btnadd"  />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <asp:Button ID="btnAudit" runat="server" Style="display: none;" />
                                <cc1:ModalPopupExtender ID="mdlAudit" runat="server" PopupControlID="divAudit" BackgroundCssClass="modalBackground"
                                    BehaviorID="mdlAudit" CancelControlID="imgAuditClosePopup" TargetControlID="btnAudit">
                                </cc1:ModalPopupExtender>
                                <div id="divAudit" runat="server" class="centerModalPopup" style="display: none;
                                    overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto">
                                    <table cellpadding="2" cellspacing="2" width="100%">
                                        <tr>
                                            <td class="LabelText" style="text-align: center !important; font-size: 12px !important;
                                                width: 97%;">
                                                <asp:Label ID="lblAudit" runat="server" Text="Audit Trail - Conco. Medication" />
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
                                                                    <asp:BoundField DataField="vIdCode" HeaderText="Code">
                                                                        <ItemStyle HorizontalAlign="Left" Width="14%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vClass" HeaderText="Class">
                                                                        <ItemStyle HorizontalAlign="Left" Width="14%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vDescription" HeaderText="Description">
                                                                        <ItemStyle HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Status">
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></ItemStyle>
                                                                        <ItemTemplate>
                                                                            <asp:HiddenField ID="hdnStatus" runat="server" Value='<% #databinder.eval(container.dataitem,"cStatusIndi") %>' />
                                                                            <asp:HiddenField ID="hdnIdCode" runat="server" Value='<% #databinder.eval(container.dataitem,"vIdCode") %>' />
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
                                                <asp:Label ID="lblRowAudit" runat="server" Text="Record Audit Trail - Conco. Medication" />
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
                                                                    <asp:BoundField DataField="dStartDate" HeaderText="Start Date" DataFormatString="{0:dd-MMM-yyyy}"
                                                                        HtmlEncode="false">
                                                                        <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="dEndDate" HeaderText="End Date" DataFormatString="{0:dd-MMM-yyyy}"
                                                                        HtmlEncode="false">
                                                                        <ItemStyle HorizontalAlign="Left" Width="11%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vDosage" HeaderText="Dosage">
                                                                        <ItemStyle HorizontalAlign="Left" Width="15%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="cConfirmed" HeaderText="Confirmed">
                                                                        <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
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
                                                                        <ItemStyle HorizontalAlign="Left" Width="15%"></ItemStyle>
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
                    <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnUpdateRecord" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnFillGrid" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnHistory" EventName="Click" />
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
                        Are you sure you want to add selected Conco. Medication?
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btnSaveAdd" runat="server" Text="Add" CssClass="btn btnadd"  ToolTip="Add Conco. Medication" />
                        <asp:Button ID="btnSaveCancel" runat="server" Text="Cancel" CssClass="btn btncancel" OnClick="btnSaveCancel_Click"
                            ToolTip="Cancel Conco. Medication" />
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
                            Style="display: inline;" />
                        <asp:Button ID="btnOk" runat="server" Text="Ok" CssClass="btn btnadd"
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
                        <asp:Button ID="btnRemarksCancel" runat="server" Text="Cancel" CssClass="btn bntcancel" />
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
                        <asp:Button ID="btnWarningOk" runat="server" Text="Ok" CssClass="ButtonText" Width="57px"
                            Style="font-size: 12px !important;" />
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <script type="text/javascript">
    
        $(function() {
        
            $('.InnerTable').parent().attr('align','left')
            $('.InnerTable').parent().css('padding-left','4px');
            $( "#tabs" ).tabs().addClass( "ui-tabs-vertical ui-helper-clearfix" );
            $( "#tabs li" ).removeClass( "ui-corner-top" ).addClass( "ui-corner-left" );
            $( "#tabs" ).tabs({active:2});
            $('#tabs ul li a').click(function () {
            
                    location.href = this.href;
            });
            
            fnApplyDataTable();
            
            $('#<%= btnAdd.ClientID %>').unbind('click').click(function(){ $find('mdlSaveAlert').show(); return false; });
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
                                                  
                $("[id$='_txtStartDate']").blur(function(){
                                                        if ($(this).val() != "" && !CheckDateLessThenToday($(this).val())) {
                                                            $(this).val('');
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Start date should be less than current date.');
                                                            return false;
                                                        }
                                                        });
                                                        
                  
                  $("[id$='_txtEndDate']").blur(function(){
                                                        if ($(this).val() != "" && !CheckDateLessThenToday($(this).val())) {
                                                            $(this).val('');
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('End date should be less than current date.');
                                                            return false;
                                                        }
                                                        });                                                        
            
        });
        
        function fnApplyDataTable()
            {
                
                $('#<%= grdAddConcoMedi.ClientID %>').prepend($('<thead>').append($('#<%= grdAddConcoMedi.ClientID %> tr:first'))).dataTable({
                    "bStateSave": false,
                    "bPaginate": true,
                    "sPaginationType": "full_numbers",
                    "bSort": true,
                    "bDestory": true,
                    "bRetrieve": true,
                    "aoColumns": [
                                    { "bSortable": false },
                                    null,
                                    null,
                                    null
                                 ]
                });
                
                 $('#<%= grdAddConcoMedi.ClientID %> tr:first').css('background-color','#3A87AD');
                                
                 $('#<%= grdConcoMedi.ClientID %>').prepend($('<thead>').append($('#<%= grdConcoMedi.ClientID %> tr:first'))).dataTable({
                    "bPaginate": true,
                    "sPaginationType": "full_numbers",
                    "bSort": true,
                    "bDestory": true,
                    "bRetrieve": true,
                     "aoColumns": [
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    { "bSortable": false }
                                 ]
                });
                
                 $('#<%= grdConcoMedi.ClientID %> tr:first').css('background-color','#3A87AD');
                 
                 $('#<%= grdAudit.ClientID %>').prepend($('<thead>').append($('#<%= grdAudit.ClientID %> tr:first'))).dataTable({
                    "bPaginate": true,
                    "sPaginationType": "full_numbers",
                    "bSort": true,
                    "bDestory": true,
                    "bRetrieve": true,
                     "aoColumns": [
                                    null,
                                    null,
                                    null,
                                    null,
                                    { "bSortable": false }
                                 ]
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
                                        location.href="frmCDMSStudyHistory.aspx?Mode=2&SubjectID=";
                                    }
                                    else
                                    {
                                        location.href="frmCDMSStudyHistory.aspx?Mode=" + fnGetQueryString("Mode") + "&SubjectID=" + fnGetQueryString("SubjectID");
                                    }
                     });
                $('#<%= btnPrevious.ClientID %>').unbind('click').click(function(){ 
                                    if(fnGetQueryString('SubjectID') == "")
                                    {
                                        location.href="frmCDMSMedicalCondition.aspx?Mode=2&SubjectID=";
                                    }
                                    else
                                    {
                                        location.href="frmCDMSMedicalCondition.aspx?Mode=" + fnGetQueryString("Mode") + "&SubjectID=" + fnGetQueryString("SubjectID");
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
            
            function fnCheckBokSelection(ctrl)
            {
               if($(ctrl).is(':checked'))
                    {
                       if($('#<%= hdnSelectedConco.ClientId %>').val() == "")
                       {
                          $('#<%= hdnSelectedConco.ClientId %>').val($($(ctrl).closest('tr').children('td')[1]).text() + ",");
                       }
                       else
                       {
                           var Data = $('#<%= hdnSelectedConco.ClientId %>').val().split(',');
                           var found = jQuery.inArray($($(ctrl).closest('tr').children('td')[1]).text(), Data);
                           if (found >= 0) {
                                Data.splice(found, 1);
                            } else {
                                Data.push($($(ctrl).closest('tr').children('td')[1]).text());
                            }
                           var blkstr = $.map(Data, function(val,index) {
                                     return val;
                                }).join(",");
                           $('#<%= hdnSelectedConco.ClientId %>').val(blkstr.replace(',,',','));
                       }
                 }
                 else
                 {
                     var Data = $('#<%= hdnSelectedConco.ClientId %>').val().split(',');
                     var found = jQuery.inArray($($(ctrl).closest('tr').children('td')[1]).text(), Data);
                     if (found >= 0) {
                         Data.splice(found, 1);
                     } 
                     var blkstr = $.map(Data, function(val,index) {
                                     return val;
                                }).join(",");
                     $('#<%= hdnSelectedConco.ClientId %>').val(blkstr.replace(',,',','));
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
            
            
            function fnDeleteCondition(ConcoMedicationNo)
            {
                $("[id$='_txtRemarks']").val('');
                $('#AlertHeader').text('Delete Confirmation');
                $('#AlertMessage').text('Are you sure you want to delete this Conco. Medication ?');
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
                                                        content.ConcoMedicationNo = ConcoMedicationNo;
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
                                                                          url: "frmCDMSConcoMedication.aspx/DeleteMedication",
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
            
            function fnEditCondition(ConcoMedicationNo)
            {
                $("[id$='_txtRemarks']").val('');
                var content = {};
                content.SubjectId = fnGetQueryString('SubjectID');
                content.ConcoMedicationNo = ConcoMedicationNo;
                $.ajax({
                           type: "POST",
                           url: "frmCDMSConcoMedication.aspx/EditMedication",
                           data: JSON.stringify(content),          
                           contentType: "application/json; charset=utf-8",
                           dataType: "json",
                           success: function(data) {
                                        if(data.d != undefined || data.d != null)
                                        {
                                            var Data = JSON.parse(data.d);
                                            var cMONTHNAMES = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug','Sep', 'Oct', 'Nov', 'Dec']; 
                                            var txtStartDate = Data.SUBJECTDTLCDMSCONCOMEDICATION[0].dStartDate;
                                            var txtEndDate = Data.SUBJECTDTLCDMSCONCOMEDICATION[0].dEndDate;
//                                            if(txtStartDate != null)
//                                            {
//                                                var StartDateArry;
//                                                StartDateArry = txtStartDate.replace('T00:00:00','').split("-");
//                                                txtStartDate  = StartDateArry[2] + "-" +  cMONTHNAMES[StartDateArry[1] - 1] + "-" + StartDateArry[0];
//                                            }
//                                            if(txtEndDate != null)
//                                            {
//                                                var EndDateArry;
//                                                EndDateArry = txtEndDate.replace('T00:00:00','').split("-");
//                                                txtEndDate  = EndDateArry[2] + "-" +  cMONTHNAMES[EndDateArry[1] - 1] + "-" + EndDateArry[0];
//                                            }
                                            
                                            $('#<%= hdnConcoMedicationNo.ClientID %>').val(Data.SUBJECTDTLCDMSCONCOMEDICATION[0].nSubjectDtlCDMSConcoMedicationNo);
                                            $('#<%= txtCode.ClientID %>').val(Data.SUBJECTDTLCDMSCONCOMEDICATION[0].vIdCode);
                                            $('#<%= txtClass.ClientID %>').val(Data.SUBJECTDTLCDMSCONCOMEDICATION[0].vClass);
                                            $('#<%= txtStartDate.ClientID %>').val(txtStartDate);
                                            $('#<%= txtEndDate.ClientID %>').val(txtEndDate);
                                            $('#<%= txtDosage.ClientID %>').val(Data.SUBJECTDTLCDMSCONCOMEDICATION[0].vDosage);
                                            $('#<%= ddlConfirmed.ClientID %>').val(Data.SUBJECTDTLCDMSCONCOMEDICATION[0].cConfirmed);
                                            $('#<%= txtComments.ClientID %>').val(Data.SUBJECTDTLCDMSCONCOMEDICATION[0].vComments);
                                            $find('mpEditConcoMedi').show();
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
                                                        $find('mpEditConcoMedi').hide();
                                                        return false;
                                                  });
            }
            
            $('#imgRowAuditClose').unbind('click').click(function(event){
                //event.preventDefault();
                $('#<%= btnAudit.ClientID %>').click();
                $find('mdlRowAudit').hide();
                return false;
           });
            
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
          var fromdate=document.getElementById('ctl00_CPHLAMBDA_txtStartDate');
          var todate = document.getElementById('ctl00_CPHLAMBDA_txtEndDate');    
          if (fromdate.value != "" && todate.value != "" ){
          var gap=GetDateDifference(fromdate.value, todate.value);
              if (gap.Days  == 0 || gap.Days  < 0){
                 msgalert ('End Date Must Not Be Less Than From Date');    
                 todate.value = "";
               }
          }
     return true ;  
 }

    </script>

</asp:Content>
