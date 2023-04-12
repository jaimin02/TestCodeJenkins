<%@ page language="VB" masterpagefile="~/ECTDMasterPage.master" autoeventwireup="false" inherits="CDMS_frmCDMSMedicalCondition, App_Web_4wz2dz2v" title=":: CDMS - Medical Condition ::" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <link rel="stylesheet" href="../App_Themes/CDMS.css" />

    
    <script src="../Script/jquery-1.11.3.min.js"></script>
    <script src="../Script/jquery-ui.js" type="text/javascript"></script>
    <script src="../Script/General.js" language="javascript" type="text/javascript"></script>
    <script src="../Script/Validation.js" language="javascript" type="text/javascript"></script>
    <script src="../Script/jquery.dataTables.min.js" type="text/javascript"></script>

    <div id="tabs" style="text-align: left; width: 99%">
        <ul>
            <li onclick="fnRedirect();"><a href="#">
                <img alt="Subject Information" src="images/Subject.png" style="padding-right: 8px;" />Subject
                Information</a></li>
            <li><a href="#tabs-1">
                <img alt="Medical Condition" src="images/Medical Condition.png" style="padding-right: 8px;" />Medical
                Condition</a></li>
            <li><a href="frmCDMSConcoMedication.aspx?Mode=<%= CType(Me.ViewState(VS_Choice),Integer) %>&SubjectID=<%= Me.ViewState(VS_SubjectID) %>">
                <img alt="Conco. Medication" src="images/Medication.png" style="padding-right: 8px;" />
                Conco. Medication</a></li>
            <li><a href="frmCDMSStudyHistory.aspx?Mode=<%= CType(Me.ViewState(VS_Choice),Integer) %>&SubjectID=<%= Me.ViewState(VS_SubjectID) %>">
                <img alt="Study History" src="images/Studyhistory.png" style="padding-right: 8px;" />Study
                History</a></li>
        </ul>
        <div id="tabs-1" align="left">
            <asp:UpdatePanel ID="upMedicalCond" runat="server" UpdateMode="Conditional" RenderMode="Inline">
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
                                                <asp:Label ID="lblSubjectMedicalCond" runat="server" />
                                            </td>
                                            <td style="width: 23%;">
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
                                    <legend class="LegendText" style="color: Black">Medical Condition</legend>
                                    <table cellpadding="2" cellspacing="2" width="100%">
                                        <tr>
                                            <td>
                                                <asp:HiddenField ID="hdnMedicalConditionNo" runat="server" />
                                                <asp:GridView ID="grdMedicalCondition" runat="server" AutoGenerateColumns="false">
                                                    <Columns>
                                                        <asp:BoundField DataField="vType" HeaderText="Type">
                                                            <ItemStyle HorizontalAlign="Left" Width="16%"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vSubType" HeaderText="Sub Type">
                                                            <ItemStyle HorizontalAlign="Left" Width="17%"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vDescription" HeaderText="Description">
                                                            <ItemStyle HorizontalAlign="Left" Width="17%"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vSymptom" HeaderText="Symptom">
                                                            <ItemStyle HorizontalAlign="Left" Width="17%"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="dOnsetDate" HeaderText="Onset Date" DataFormatString="{0:dd-MMM-yyyy}"
                                                            HtmlEncode="false">
                                                            <ItemStyle HorizontalAlign="Left" Width="15%"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="dResolutionDate" HeaderText="Resolution Date" DataFormatString="{0:dd-MMM-yyyy}"
                                                            HtmlEncode="false">
                                                            <ItemStyle HorizontalAlign="Left" Width="22%"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vComments" HeaderText="Comments">
                                                            <ItemStyle HorizontalAlign="Left" Width="22%" Wrap ="true"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap ="true" ></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Action">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemTemplate>
                                                                <asp:Image ID="imgEdit" runat="server" AlternateText='<% #databinder.eval(container.dataitem,"nSubjectDtlCSMSMedicalConditionNo") %>'
                                                                    ImageUrl="~/CDMS/images/Edit_Small.png" ToolTip="Edit Medical Condition" Style="cursor: pointer;"
                                                                    onclick="fnEditCondition(this.alt);" />
                                                                <asp:Image ID="imgDelete" runat="server" AlternateText='<% #databinder.eval(container.dataitem,"nSubjectDtlCSMSMedicalConditionNo") %>'
                                                                    ImageUrl="~/CDMS/images/delete_small.png" ToolTip="Delete Medical Condition" Style="padding-left: 3px;
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
                                <asp:Button ID="btnEdit" runat="server" Style="display: none;" />
                                <cc1:ModalPopupExtender ID="mpEditMedicalCond" runat="server" PopupControlID="divEditMedicalCond"
                                    BackgroundCssClass="modalBackground" BehaviorID="mpEditMedicalCond" CancelControlID="imgEditClosePopup"
                                    TargetControlID="btnEdit">
                                </cc1:ModalPopupExtender>
                                <div id="divEditMedicalCond" runat="server" class="centerModalPopup" style="display: none;
                                    width: 710px; height: 350px;">
                                    <table cellpadding="2" cellspacing="2" width="100%">
                                        <tr>
                                            <td class="LabelText" style="text-align: center !important; font-size: 12px !important;
                                                width: 97%;">
                                                Edit Medical Condition
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
                                                            Type :
                                                        </td>
                                                        <td style="text-align: left">
                                                            <asp:TextBox ID="txtType" Enabled="false" runat="server" CssClass="TextBox" Width="210px" />
                                                        </td>
                                                        <td class="LabelText">
                                                            Sub Type :
                                                        </td>
                                                        <td style="text-align: left">
                                                            <asp:TextBox ID="txtSubType" Enabled="false" runat="server" CssClass="TextBox" Width="219px" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="LabelText">
                                                            Description :
                                                        </td>
                                                        <td colspan="3" style="text-align: left">
                                                            <asp:TextBox ID="txtDescrption" Enabled="false" runat="server" CssClass="TextBox"
                                                                Width="541px" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="LabelText">
                                                            Onset Date :
                                                        </td>
                                                        <td style="text-align: left">
                                                            <asp:TextBox ID="txtOnsetDate" runat="server" CssClass="TextBox" ToolTip="Please enter date in dd-mm-yyyy format (e.g. '01012013'/'01-Jan-2013')" TabIndex="1"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="calOnsetDate" runat="server" TargetControlID="txtOnsetDate"
                                                                Format="dd-MMM-yyyy">
                                                            </cc1:CalendarExtender>
                                                        </td>
                                                        <td class="LabelText">
                                                            Resolution Date :
                                                        </td>
                                                        <td style="text-align: left">
                                                            <asp:TextBox ID="txtResolutionDate" runat="server" CssClass="TextBox" ToolTip="Please enter date in dd-mm-yyyy format (e.g. '01012013'/'01-Jan-2013')" TabIndex="2"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="calResolutionDate" runat="server" TargetControlID="txtResolutionDate"
                                                                Format="dd-MMM-yyyy">
                                                            </cc1:CalendarExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="LabelText">
                                                            Source :
                                                        </td>
                                                        <td class="LabelText" style="text-align: left;">
                                                            <asp:DropDownList ID="ddlSource" runat="server" Width="156px" TabIndex="3">
                                                                <asp:ListItem Text="Select Source" Value="0" />
                                                                <asp:ListItem Text="Both" Value="Both" />
                                                                <asp:ListItem Text="Medical/Surgical History" Value="Medical/Surgical History" />
                                                                <asp:ListItem Text="Physical Examination" Value="Physical Examination" />
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="LabelText">
                                                            Confimed :
                                                        </td>
                                                        <td class="LabelText" style="text-align: left;">
                                                            <asp:DropDownList ID="ddlConfirmed" runat="server" Width="110px" TabIndex="4">
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
                                                                CssClass="TextBox" Width="541px" TabIndex="5" />
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
                                                <asp:Button ID="btnUpdateRecord" runat="server" Text="Update" CssClass="btn btnupdate" TabIndex="6"/>
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btncancel"  TabIndex="7"/>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <cc1:ModalPopupExtender ID="mpMedicalCond" runat="server" PopupControlID="divMedicalCond"
                                    BackgroundCssClass="modalBackground" BehaviorID="mpMedicalCond" CancelControlID="imgClosePopup"
                                    TargetControlID="btnAddMore">
                                </cc1:ModalPopupExtender>
                                <div id="divMedicalCond" runat="server" class="centerModalPopup" style="display: none;
                                    overflow: auto; width: 80%; height: auto; max-height: 85%; min-height: auto">
                                    <table cellpadding="2" cellspacing="2" width="100%">
                                        <tr>
                                            <td class="LabelText" style="text-align: center !important; font-size: 12px !important;
                                                width: 97%;">
                                                Add New Medical Condition
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
                                                            <asp:HiddenField ID="hdnSelectedCondition" runat="server" />
                                                            <asp:GridView ID="grdAddMedicalCond" runat="server" AutoGenerateColumns="false">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Select">
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%"></ItemStyle>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkCodeList" runat="server" onclick="fnCheckBokSelection(this);" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="CodeMedicalConditions" HeaderText="ID Code">
                                                                        <ItemStyle HorizontalAlign="Center" Width="12%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Type" HeaderText="Type">
                                                                        <ItemStyle HorizontalAlign="Left" Width="14%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="SubType" HeaderText="Sub Type">
                                                                        <ItemStyle HorizontalAlign="Left" Width="14%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Description" HeaderText="Description">
                                                                        <ItemStyle HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Symptom" HeaderText="Symptom">
                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
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
                                                <asp:Button ID="btnAdd" runat="server" Text="Add" OnClientClick ="show()" CssClass="btn btnadd"   />
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
                                                <asp:Label ID="lblAudit" runat="server" Text="Audit Trail - Medical Condition" />
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
                                                            <asp:GridView ID="grdAudit" runat="server" AutoGenerateColumns="false" style="width:100%;">
                                                                <Columns>
                                                                    <asp:BoundField DataField="vType" HeaderText="Type">
                                                                        <ItemStyle HorizontalAlign="Left" Width="14%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vSubType" HeaderText="Sub Type">
                                                                        <ItemStyle HorizontalAlign="Left" Width="14%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vDescription" HeaderText="Description">
                                                                        <ItemStyle HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vSymptom" HeaderText="Symptom">
                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Status">
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%"></ItemStyle>
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
                                                <asp:Label ID="lblRowAudit" runat="server" Text="Record Audit Trail - Medical Condition" />
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
                                                            <asp:GridView ID="grdRowAudit" runat="server" AutoGenerateColumns="false" style="width:100%;">
                                                                <Columns>
                                                                    <asp:BoundField DataField="dOnsetDate" HeaderText="Onset Date" DataFormatString="{0:dd-MMM-yyyy}"
                                                                        HtmlEncode="false">
                                                                        <ItemStyle HorizontalAlign="Left" Width="13%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="dResolutionDate" HeaderText="Resolution Date" DataFormatString="{0:dd-MMM-yyyy}"
                                                                        HtmlEncode="false">
                                                                        <ItemStyle HorizontalAlign="Left" Width="17%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vSource" HeaderText="Source">
                                                                        <ItemStyle HorizontalAlign="Left" Width="15%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="cConfirmed" HeaderText="Confirmed">
                                                                        <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vComments" HeaderText="Comments">
                                                                        <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vRemarks" HeaderText="Reason">
                                                                        <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
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
                    </table>
                </ContentTemplate>
                <Triggers>
                    <%--<asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />--%>
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
                        Are you sure you want to add selected Medical Condition?
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btnSaveAdd" runat="server" Text="Add" OnClick="btnSaveAdd_Click" CssClass="btn btnsave" ToolTip="Add Medical Condition" />
                        <asp:Button ID="btnSaveCancel" runat="server" Text="Cancel" OnClick="btnSaveCancel_Click" CssClass="btn btncancel" ToolTip="Cancel Medical Condition" />
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
                            Style="display: inline;" />
                        <asp:Button ID="btnNo" runat="server" Text="No" CssClass="btn btnno" 
                            Style="display: inline;" />
                        <asp:Button ID="btnOk" runat="server" Text="Ok" CssClass="btn btnyes" 
                            Style="font-size: 12px !important; display: none;" />
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
                        <asp:Button ID="btnWarningOk" runat="server" Text="Ok" CssClass="Button" Width="57px"
                            Style="font-size: 12px !important;" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField ID="reload" runat="server" />

    <script type="text/javascript">
       
            $(function() {
                $('.InnerTable').parent().attr('align','left')
                $('.InnerTable').parent().css('padding-left','4px');
                $( "#tabs" ).tabs().addClass( "ui-tabs-vertical ui-helper-clearfix" );
                $( "#tabs li" ).removeClass( "ui-corner-top" ).addClass( "ui-corner-left" );
                $( "#tabs" ).tabs({active:1});
                $('#tabs ul li a').click(function () {
                        location.href = this.href;
                });
                
                  
                fnApplyDataTable();
                
                // $('#<%= btnAdd.ClientID %>').unbind('click').click(function(){ $find('mdlSaveAlert').show(); return false; });
                 $('#<%= btnWarningOk.ClientID %>').unbind('click').click(function(){$find('mdlWarning').hide();  return false;});
                
                $('#<%= btnRemarksUpdate.ClientID %>').click(function(){
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
                
                    
                $('#<%= btnOk.ClientID %>').click(function(event){ 
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
                                                  
                                                  
                  $("[id$='_txtOnsetDate']").blur(function(){
                                                        if ($(this).val() != "" && !CheckDateLessThenToday($(this).val())) {
                                                            $(this).val('');
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Onset date should be less than current date.');
                                                            return false;
                                                        }
                                                        });
                                                        
                  
                  $("[id$='_txtResolutionDate']").blur(function(){
                                                        if ($(this).val() != "" && !CheckDateLessThenToday($(this).val())) {
                                                            $(this).val('');
                                                            $find('mdlWarning').show();
                                                            $('#WarningHeader').text('Warning');
                                                            $('#WarningMessage').text('Resolution date should be less than current date.');
                                                            return false;
                                                        }
                                                        });                                    
                
            });
            
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
            
            
            function fnApplyDataTable()
            {
                
                $('#<%= grdAddMedicalCond.ClientID %>').prepend($('<thead>').append($('#<%= grdAddMedicalCond.ClientID %> tr:first'))).dataTable({
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
                                    null,
                                    null,
                                    null
                                 ]
                });
                 
                  $('#<%= grdAddMedicalCond.ClientID %> tr:first').css('background-color','#3A87AD');
                
                 $('#<%= grdMedicalCondition.ClientID %>').prepend($('<thead>').append($('#<%= grdMedicalCondition.ClientID %> tr:first'))).dataTable({
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
                                    null,
                                    { "bSortable": false }
                                 ]
                });
                 $('#<%= grdMedicalCondition.ClientID %> tr:first').css('background-color','#3A87AD');
                 
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
                 
                $('#<%= btnNext.ClientID %>').click(function(){ location.href="frmCDMSConcoMedication.aspx?Mode=" + fnGetQueryString("Mode") + "&SubjectID=" + fnGetQueryString("SubjectID"); });
                $('#<%= btnPrevious.ClientID %>').click(function(){ 
                                    if(fnGetQueryString('SubjectID') == "")
                                    {
                                        location.href="frmCDMSSubjectInformation.aspx?Mode=1";
                                    }
                                    else
                                    {
                                        location.href="frmCDMSSubjectInformation.aspx?Mode=" + fnGetQueryString("Mode") + "&SubjectID=" + fnGetQueryString("SubjectID");
                                    }
                                    
                });
                
            }
            
            function fnCheckBokSelection(ctrl)
            {
               if($(ctrl).is(':checked'))
                    {
                       if($('#<%= hdnSelectedCondition.ClientId %>').val() == "")
                       {
                          $('#<%= hdnSelectedCondition.ClientId %>').val($($(ctrl).closest('tr').children('td')[1]).text() + ",");
                       }
                       else
                       {
                           var Data = $('#<%= hdnSelectedCondition.ClientId %>').val().split(',');
                           var found = jQuery.inArray($($(ctrl).closest('tr').children('td')[1]).text(), Data);
                           if (found >= 0) {
                                Data.splice(found, 1);
                            } else {
                                Data.push($($(ctrl).closest('tr').children('td')[1]).text());
                            }
                           var blkstr = $.map(Data, function(val,index) {
                                     return val;
                                }).join(",");
                           $('#<%= hdnSelectedCondition.ClientId %>').val(blkstr.replace(',,',','));
                       }
                 }
                 else
                 {
                     var Data = $('#<%= hdnSelectedCondition.ClientId %>').val().split(',');
                     var found = jQuery.inArray($($(ctrl).closest('tr').children('td')[1]).text(), Data);
                     if (found >= 0) {
                         Data.splice(found, 1);
                     } 
                     var blkstr = $.map(Data, function(val,index) {
                                     return val;
                                }).join(",");
                     $('#<%= hdnSelectedCondition.ClientId %>').val(blkstr.replace(',,',','));
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
            
            function fnDeleteCondition(MedicalConditionNo)
            {
                $("[id$='_txtRemarks']").val('');
                $('#AlertHeader').text('Delete Confirmation');
                $('#AlertMessage').text('Are you sure you want to delete this medical condition ?');
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
                                                        content.MedicalConditionNo = MedicalConditionNo;
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
                                                                      url: "frmCDMSMedicalCondition.aspx/DeleteMedicalCondition",
                                                                      data: JSON.stringify(content),          
                                                                      contentType: "application/json; charset=utf-8",
                                                                      dataType: "json",
                                                                      success: function(data) {
                                                                            if(data.d == "Success")
                                                                            {
                                                                                $find('mdlRemarks').hide();
                                                                                $('#<%= reload.ClientId %>').val("YES")
                                                                                $('#<%= btnFillGrid.ClientID %>').click();
                                                                                //location.reload();
                                                                                
                                                                                
                                                                            }
                                                                            else
                                                                            {
                                                                                msgalert(data.d);
                                                                               // location.reload();
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
            
            function fnEditCondition(MedicalConditionNo)
            {
                $("[id$='_txtRemarks']").val('');
                var content = {};
                content.SubjectId = fnGetQueryString('SubjectID');
                content.MedicalConditionNo = MedicalConditionNo;
                $.ajax({
                           type: "POST",
                           url: "frmCDMSMedicalCondition.aspx/EditMedicalCondition",
                           data: JSON.stringify(content),          
                           contentType: "application/json; charset=utf-8",
                           dataType: "json",
                           success: function(data) {
                                        if(data.d != undefined || data.d != null)
                                        {
                                            var Data = JSON.parse(data.d);
                                            var cMONTHNAMES = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug','Sep', 'Oct', 'Nov', 'Dec']; 
                                            var txtOnsetDate = Data.SUBJECTDTLCDMSMEDICALCONDITION[0].dOnsetDate;
                                            var txtResolutionDate = Data.SUBJECTDTLCDMSMEDICALCONDITION[0].dResolutionDate;
//                                            if(txtOnsetDate != null)
//                                            {
//                                                var OnsetDateArry;
//                                                OnsetDateArry = txtOnsetDate.replace('T00:00:00','').split("-");
//                                                txtOnsetDate  = OnsetDateArry[2] + "-" +  cMONTHNAMES[OnsetDateArry[1] - 1] + "-" + OnsetDateArry[0];
//                                            }
//                                            if(txtResolutionDate != null)
//                                            {
//                                                var ResolutionDateArry;
//                                                ResolutionDateArry = txtResolutionDate.replace('T00:00:00','').split("-");
//                                                txtResolutionDate  = ResolutionDateArry[2] + "-" +  cMONTHNAMES[ResolutionDateArry[1] - 1] + "-" + ResolutionDateArry[0];
//                                            }
                                            
                                            $('#<%= hdnMedicalConditionNo.ClientID %>').val(Data.SUBJECTDTLCDMSMEDICALCONDITION[0].nSubjectDtlCSMSMedicalConditionNo);
                                            $('#<%= txtType.ClientID %>').val(Data.SUBJECTDTLCDMSMEDICALCONDITION[0].vType);
                                            $('#<%= txtSubType.ClientID %>').val(Data.SUBJECTDTLCDMSMEDICALCONDITION[0].vSubType);
                                            $('#<%= txtDescrption.ClientID %>').val(Data.SUBJECTDTLCDMSMEDICALCONDITION[0].vDescription);
                                            $('#<%= txtOnsetDate.ClientID %>').val(txtOnsetDate);
                                            $('#<%= txtResolutionDate.ClientID %>').val(txtResolutionDate);
                                            $('#<%= ddlSource.ClientID %>').val(Data.SUBJECTDTLCDMSMEDICALCONDITION[0].vSource);
                                            $('#<%= ddlConfirmed.ClientID %>').val(Data.SUBJECTDTLCDMSMEDICALCONDITION[0].cConfirmed);
                                            $('#<%= txtComments.ClientID %>').val(Data.SUBJECTDTLCDMSMEDICALCONDITION[0].vComments);
                                            $find('mpEditMedicalCond').show();
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
                                                        $find('mpEditMedicalCond').hide();
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
     return true ;  
 }
   
  function show() {
      $find('mdlSaveAlert').show();
  }
    </script>
    <style>
        .Button{
            border-right: #1560A1 2px solid;
            border-top: #1560A1 2px solid;
            font-size: x-small;
            background: White;
            border-left: #1560A1 2px solid;
            border-bottom: #1560A1 2px solid;
            font-style: normal;
            font-family: Verdana, Arial, Helvetica, sans-serif;
            color: #284E98;
            font-size: smaller;
            /* width: 75px; */
            font-weight: bold;
            border-radius:7px;
   </style>
</asp:Content>
