<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmReportReview.aspx.vb" Inherits="frmReportReview" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:content id="Content1" contentplaceholderid="CPHLAMBDA" runat="Server">
    <%--CHEMISTRY,IMMUNOLOGY,HEMATOLOGY,URIANALYSIS,COAGULATION--%>
    <asp:HiddenField ID="hdnCheckBox" runat="server"></asp:HiddenField>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table style="width: 80%; margin: auto;">
                <tbody>
                    <tr style="width: 100%">
                        <td style="text-align: center;" class="Label">
                            <asp:RadioButtonList ID="RBLProjecttype" runat="server" AutoPostBack="True" Width="50%"
                                RepeatDirection="Horizontal" OnSelectedIndexChanged="RBLProjecttype_SelectedIndexChanged"
                                Style="margin: auto;">
                                <asp:ListItem Selected="true" Value="0000000000">Generic Screening</asp:ListItem>
                                <asp:ListItem Value="1">Project Specific</asp:ListItem>
                                <asp:ListItem Value="2">Project Specific Screening</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="font-weight: bold; font-size: 13px; float: none; color: white; background-color: #1560a1;
                                width: 100%; margin: auto;" id="div1" onclick="ShowHideDiv('<%=divSubjectSelection.ClientID %>','imgSubjectSelection')">
                                <div style="float: right;">
                                    <img id="imgSubjectSelection" alt="Image" src="images/expand.jpg" />
                                </div>
                                <div style="text-align: center;">
                                    Select Subject
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="display: none; width: 100%;" id="divSubjectSelection" class="FieldSetBox"
                                align="center" runat="server">
                                <asp:UpdatePanel ID="upSubjectSelection" runat="server" UpdateMode="Conditional"
                                    ChildrenAsTriggers="true">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tbody>
                                                <tr style="display: none; width: 100%" id="trSearchSub" runat="Server">
                                                    <td style="width: 20%; white-space: nowrap" align="right">
                                                        <strong class="Label">Subject:</strong>
                                                    </td>
                                                    <td style="white-space: nowrap" align="left">
                                                        <asp:TextBox ID="txtSubject" TabIndex="2" runat="server" CssClass="textBox" Width="61%"></asp:TextBox>
                                                        <asp:Button Style="display: none" ID="btnSubject" runat="server" Text="Subject" CssClass="btn btnnew">
                                                        </asp:Button>
                                                        <asp:HiddenField ID="HSubjectId" runat="server"></asp:HiddenField>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                                            MinimumPrefixLength="1" OnClientItemSelected="OnSelected1" OnClientShowing="ClientPopulated1"
                                                            ServiceMethod="GetSubjectCompletionList_NotRejected" ServicePath="AutoComplete.asmx"
                                                            TargetControlID="txtSubject" UseContextKey="True" CompletionListElementID="pnlSubjectList"
                                                            CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                            CompletionListCssClass="autocomplete_list">
                                                        </cc1:AutoCompleteExtender>
                                                        <asp:Panel ID="pnlSubjectList" runat="server" Style="max-height: 200px; overflow: auto;
                                                            overflow-x: hidden" />
                                                    </td>
                                                </tr>
                                                <tr style="display: none" id="trProject" runat="server">
                                                    <td style="width: 20%" align="right">
                                                        <strong class="Label">Project :</strong>
                                                    </td>
                                                    <td style="white-space: nowrap" align="left">
                                                        <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="71%"></asp:TextBox>
                                                        <asp:CheckBox ID="chkInProject" runat="server" AutoPostBack="True" OnCheckedChanged="chkInProject_CheckedChanged"
                                                            Text="InProject"></asp:CheckBox>
                                                        <asp:Button Style="display: none" ID="btnSetProject" runat="server" Text=" Project" CssClass="btn btnnew">
                                                        </asp:Button>
                                                        <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" BehaviorID="AutoCompleteExtender2"
                                                            MinimumPrefixLength="1" OnClientItemSelected="OnSelected" OnClientShowing="ClientPopulated"
                                                            ServiceMethod="GetMyProjectCompletionList" ServicePath="AutoComplete.asmx" TargetControlID="txtProject"
                                                            UseContextKey="True" CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                            CompletionListItemCssClass="autocomplete_listitem" CompletionListElementID="pnlProjectList">
                                                        </cc1:AutoCompleteExtender>
                                                        <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto;
                                                            overflow-x: hidden" />
                                                    </td>
                                                </tr>
                                                <tr style="display: none; width: 100%" id="trActivity" runat="server">
                                                    <td style="width: 20%; white-space: nowrap;" align="right">
                                                        <strong class="Label">Activity :</strong>
                                                    </td>
                                                    <td style="white-space: nowrap;" align="left">
                                                        <asp:DropDownList ID="ddlActivity"  runat="server" CssClass="dropDownList"
                                                            Width="35%" >
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="trperiod" runat="Server" visible="false">
                                                    <td style="width: 20%; white-space: nowrap;" align="right">
                                                        <strong class="Label">Periods :</strong>
                                                    </td>
                                                    <td style="white-space: nowrap;" align="left">
                                                        <asp:DropDownList ID="ddlPeriods" AutoPostBack="true" TabIndex="4" runat="server"
                                                            CssClass="dropDownList" Width="35%">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr style="display: none" id="trSubList" runat="Server">
                                                    <td style="width: 20%;" align="right">
                                                        <strong class="Label">Subjects :</strong>
                                                    </td>
                                                    <td align="left">
                                                        <div style="max-width: 90%; height: 100px; overflow: auto; border: solid 1px navy;"
                                                            id="pnlSubject" runat="server">
                                                            <asp:RadioButtonList ID="RBLSubject" runat="server" AutoPostBack="True" OnSelectedIndexChanged="RBLSubject_SelectedIndexChanged"
                                                                RepeatColumns="2">
                                                            </asp:RadioButtonList>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlperiods" />
                                        <asp:AsyncPostBackTrigger ControlID="chkInProject" EventName="CheckedChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="upscreeningDtl" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tbody>
                                                <tr align="left" id="trScreeningDate" runat="server">
                                                    <td style="width: 20%;" align="right">
                                                        <strong class="Label">Screening Dates :</strong>
                                                    </td>
                                                    <td align="left">
                                                        <asp:RadioButtonList ID="rblScreeningDate" runat="server" CssClass="Label" AutoPostBack="True"
                                                            RepeatDirection="Horizontal" RepeatColumns="3">
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;" align="right">
                                                        <strong class="Label">Samples :</strong>
                                                    </td>
                                                    <td align="left">

                                                        <asp:RadioButtonList ID="rblSampleid" runat="server" CssClass="Label" AutoPostBack="True"
                                                            RepeatDirection="Horizontal" RepeatColumns="3">
                                                        </asp:RadioButtonList>
                                                    </td>
                                                    
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;" align="right">
                                                        <strong class="Label">Previous  Report:</strong>
                                                    </td>
                                                    <td align="left">
                                                         <%--<asp:DropDownList ID="ddlRevisedSampleid"  AutoPostBack="True"  runat="server" CssClass="dropDownList"
                                                            Width="35%">
                                                        </asp:DropDownList>--%>
                                                        <asp:RadioButtonList ID="rblRevisedSampleid"   runat="server" CssClass="Label" AutoPostBack="True"
                                                            RepeatDirection="Horizontal" RepeatColumns="3">
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr style="display: none" id="trbtnTRF" align="left" runat="server">
                                                    <%-- <td style="width: 25%;" align="right" colspan ="2">
                                                    </td>--%>
                                                    <td style="vertical-align: top; text-align: center;" colspan="2">
                                                        <asp:Button Style="display: ; margin: auto;" ID="btnTRF" OnClick="btnTRF_Click" runat="Server"
                                                            CssClass="btn btnnew" Text="TRF" ToolTip="Test Requistion Form" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="RBLSubject" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="rblScreeningDate" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSubject" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnTRF" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="rblSampleid" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="rblRevisedSampleid" EventName="SelectedIndexChanged" />
                                        <%--<asp:PostBackTrigger ControlID="ddlRevisedSampleid" />  --%>
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table style="width: 80%; margin: auto;">
                        <tr>
                            <td>
                                <%--<asp:UpdatePanel id="UpdatePanel2" runat="server" UpdateMode="Conditional"><ContentTemplate>--%>
                                <table width="100%" class="FieldSetBox" style="display: none;" id="tabInfo" runat="server">
                                    <tbody>
                                        <tr runat="server" id="TrScreening" align="left" visible="false">
                                            <td>
                                                <span>Screening No :</span>
                                                <asp:Label ID="LblScrNo" runat="server" Text="" />
                                            </td>
                                            <td>
                                                <span>Lab Id :</span>
                                                <asp:Label ID="LblLabID" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td>
                                                <span>Sample Collected On :</span>
                                                <asp:Label ID="LblSampleClDt" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="Tr1" align="left" visible="false">
                                            <td>
                                                <span>Subject Initials:</span>
                                                <asp:Label ID="LblSub" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td>
                                                <span>Sex :</span>
                                                <asp:Label ID="LblSex" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td>
                                                <span>Sample Collected At :</span>
                                                <asp:Label ID="LblSampleClAt" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="Tr2" align="left" visible="false">
                                            <td>
                                                <span>Sample Received On :</span>
                                                <asp:Label ID="LblSampleRcvDt" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td>
                                                <span>Visit :</span>
                                                <asp:Label ID="LblVisit" runat="server"  Text=""></asp:Label>
                                            </td>
                                            <td>
                                                <span>Report Date :</span>
                                                <asp:Label ID="LblRptDt" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="Tr3" visible="false">
                                            <td align="left">
                                                <span>Birth Date :</span>
                                                <asp:Label ID="LblBDate" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td align="left">
                                                <span>Subject No:</span>
                                                <asp:Label ID="lblSubNo" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td align="left">
                                                <span>Study/Project :</span>
                                                <asp:Label ID="LblPrj" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="Tr4" visible="false">
                                            <td align="left">
                                                <span>Reffered By :</span>
                                                <asp:Label ID="LblRfrdBy" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100%">
                                <table class="FieldSetBox" width="100%" style="display: none;" id="tabData" runat="server">
                                    <tbody>
                                        <tr>
                                            <td style="white-space: nowrap;">
                                                <div style="width: 80%; margin: auto;">
                                                    <asp:LinkButton Style="display: none" ID="lnkChemistry" OnClick="lnkChemistry_Click"
                                                        runat="server" ForeColor="White" Text="CHEMISTRY" CssClass="TABButton" Width="105px"
                                                        Height="25px" OnClientClick="return confirmation(this);"></asp:LinkButton>
                                                    <asp:LinkButton Style="display: none" ID="LnkIMMUNOLOGY" OnClick="LnkIMMUNOLOGY_Click"
                                                        runat="server" ForeColor="White" Text="IMMUNOLOGY" CssClass="TABButton" Width="105px"
                                                        Height="25px" OnClientClick="return confirmation(this);"></asp:LinkButton>
                                                    <asp:LinkButton Style="display: none" ID="LinkButton1" OnClick="lnkHEMATOLOGY_Click"
                                                        runat="server" ForeColor="White" Text="HEMATOLOGY" CssClass="TABButton" Width="105px"
                                                        Height="25px" OnClientClick="return confirmation(this);"></asp:LinkButton>
                                                    <asp:LinkButton Style="display: none" ID="lnkHEMATOLOGY" OnClick="lnkHEMATOLOGY_Click"
                                                        runat="server" ForeColor="White" Text="HEMATOLOGY" CssClass="TABButton" Width="105px"
                                                        Height="25px" OnClientClick="return confirmation(this);"></asp:LinkButton>
                                                    <asp:LinkButton Style="display: none" ID="lnkURIANALYSIS" OnClick="lnkURIANALYSIS_Click"
                                                        runat="server" ForeColor="White" Text="URIANALYSIS" CssClass="TABButton" Width="105px"
                                                        Height="25px" OnClientClick="return confirmation(this);"></asp:LinkButton>
                                                    <asp:LinkButton Style="display: none" ID="lnkCOAGULATION" OnClick="lnkCOAGULATION_Click"
                                                        runat="server" ForeColor="White" Text="COAGULATION" CssClass="TABButton" Width="105px"
                                                        Height="25px" OnClientClick="return confirmation(this);"></asp:LinkButton>
                                                    <asp:LinkButton Style="display: none" ID="lnkSTOOL" OnClick="lnkSTOOL_Click" runat="server"
                                                        ForeColor="White" Text="STOOL EXAMINATION" CssClass="TABButton" Width="180px"
                                                        Height="25px" OnClientClick="return confirmation(this);"></asp:LinkButton>
                                                    <asp:LinkButton Style="display: none" ID="lnkHIV1" OnClick="lnkHIV1_Click" runat="server"
                                                        ForeColor="White" Text="WESTERN BLOT for HIV -I" CssClass="TABButton" Width="220px"
                                                        Height="25px" OnClientClick="return confirmation(this);"></asp:LinkButton>
                                                    <asp:LinkButton Style="display: none" ID="lnkPAP" OnClick="lnkPAP_Click" runat="server"
                                                        ForeColor="White" Text="PAP Smear Report" CssClass="TABButton" Width="220px"
                                                        Height="25px" OnClientClick="return confirmation(this);"></asp:LinkButton>
                                                    <asp:LinkButton Style="display: none" ID="lnkCytology" OnClick="lnkCytology_Click"
                                                        runat="server" ForeColor="White" Text="VAGINAL CYTOLOGY" CssClass="TABButton"
                                                        Width="220px" Height="25px" OnClientClick="return confirmation(this);"></asp:LinkButton>
                                                     <asp:LinkButton Style="display: none" ID="lnkUrineCytology" OnClick="lnkUrineCytology_Click"
                                                        runat="server" ForeColor="White" Text="URINE CYTOLOGY" CssClass="TABButton"
                                                        Width="145px" Height="25px" OnClientClick="return confirmation(this);"></asp:LinkButton>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:GridView ID="gvwtstChemistry" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                                    SkinID="grdViewSml">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="#">
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemStyle HorizontalAlign="center" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Update">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="ChkEdit" runat="server" onclick="ChangedDisabled();" type="checkbox" />
                                                                <%--<input ID="ChkEdit" Name="ChkEdit"  runat="server" onclick="ChangedDisabled();" type="checkbox" />--%>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="vMedexDesc" HeaderText="Test">
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vMedexResult" HeaderText="Test Result" />
                                                        <asp:BoundField DataField="vLowRange" HeaderText="Low Range" />
                                                        <asp:BoundField DataField="vHighRange" HeaderText="High Range" />
                                                        <asp:BoundField DataField="vUOM" HeaderText="UOM" />
                                                        <asp:TemplateField HeaderText="Normal">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="ChkNormal" runat="server" BackColor="White" Font-Bold="True" Font-Overline="True"
                                                                    ForeColor="Black" onclick="return false" type="checkbox" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Abnormal &amp; notCS">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="ChkAbnormal" runat="server" BackColor="Transparent" Font-Bold="True"
                                                                    ForeColor="Blue" onclick="ValidateCheck1(this);" type="checkbox" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Abormal &amp; CS">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="ChkClSignificant" runat="server" onclick="ValidateCheck2(this);" type="checkbox" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Remarks">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtRemark" runat="server" Font-Bold="True" ForeColor="Black" Style="width: 160px"
                                                                    type="text"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Audit Trail">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgAudit" runat="server" ToolTip="Audit Trail" ImageUrl="~/Images/audit.png"
                                                                    Visible="false" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="nSampleId" HeaderText="SampleId" />
                                                        <asp:BoundField DataField="tranno" HeaderText="tranno" />
                                                        <asp:BoundField DataField="cNormalflag" HeaderText="Normalflag" />
                                                        <asp:BoundField DataField="cAbnormalflag" HeaderText="Abnormalflag" />
                                                        <asp:BoundField DataField="cClinicallySignflag" HeaderText="ClinicallySignflag" />
                                                        <asp:BoundField DataField="vGeneralRemark" HeaderText="GeneralRemark" />
                                                        <asp:BoundField DataField="vmedexcode" HeaderText="medexcode" />
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr id="trRptRemarks" runat="server" style="display: none; width: 100%">
                                            <td style="text-align: center; vertical-align: top;" class="Label">
                                                TestGroup Remark(from Lab):
                                                <textarea id="txtRptRemarks" runat="server" rows="2" cols="0" style="font-size: small;
                                                    width: 50%; color: black; vertical-align: bottom;" disabled="disabled"></textarea>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td style="text-align: center;">
                                                <asp:Label ID="lblReviedby" runat="server" SkinID="lblHeading"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="trReviewed" runat="server" style="display: none">
                                            <td>
                                                <asp:CheckBox ID="chkReviewed" runat="server" onclick="ShowHideDivRmk();" Text="I agree that I have reviewed the data" />
                                            </td>
                                        </tr>
                                        <tr runat="server" id="TrBotomTopLine" visible="false">
                                            <td>
                                                <hr class="hr" style="width: 100%;" />
                                            </td>
                                        </tr>
                                        <tr runat="server" id="TrFooter" visible="false">
                                            <td align="center">
                                                <asp:Label runat="server" ID="LblRptAuthOn" Text="Report Authenticated On :"></asp:Label>
                                                <asp:Label runat="server" ID="LblRtpAutDate" Text=""></asp:Label>
                                                <asp:Label runat="server" ID="LblAuthBy" Text="Authenticated By :"></asp:Label>
                                                <asp:Label runat="server" ID="LblAuthdBy" Text=""></asp:Label>
                                                <asp:Label runat="server" ID="LblAuthByAutoRelease" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="TrFooterLine" visible="false">
                                            <td>
                                                <hr class="hr " style="width: 100%;" />
                                            </td>
                                        </tr>
                                        </tbody>
                                    </table>
                                </td>
                        </tr>
                        <tr>
                            <td style="text-align: center;">
                                <asp:Button ID="btnFnlRmkAudit" runat="server" CssClass="btn btnaudit" OnClick="btnFnlRmkAudit_Click"
                                   ToolTip="Audit Trail(Final Remarks)" Visible="False" />
                                <asp:Button ID="btnSave" runat="server" CssClass="btn btnsave" 
                                    OnClientClick="return ValidateTest(this);" Text="Save and Continue" ToolTip="Save and Continue" />
                                <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" OnClick="btnCancel_Click"
                                    OnClientClick="dfd();" Text="Cancel" ToolTip="Cancel" />
                                <asp:Button ID="btnExit" runat="server" CssClass="btn btnexit" OnClick="btnExit_Click" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this); "
                                    Text="Exit" ToolTip="Exit" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <button id="btnSearch" runat="server" style="display: none;" />
                                <cc1:ModalPopupExtender ID="MpeSearch" runat="server" BackgroundCssClass="modalBackground"
                                    CancelControlID="imgSearch" PopupControlID="divSearch" TargetControlID="btnSearch"
                                    BehaviorID="MpeSearch">
                                </cc1:ModalPopupExtender>
                                <div class="modal-content modal-sm" id="divSearch" style="display:none;" runat="server">
                                    <div class="modal-header">
                                        <img id="imgSearch" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px;" title="Close" />
                                        <h2>Review Remarks</h2>
                                    </div>
                                    <div class="modal-body">
                                        <table width="100%">
                                            <tbody>
                                                <tr style="width: 100%">
                                                    <td style="width: 35%;">
                                                        <strong style="white-space: nowrap">Remarks</strong>
                                                    </td>
                                                    <td style="width: 65%;">
                                                        <asp:TextBox ID="txtLockRemark" runat="server" CssClass="textBox" Height="38px" TabIndex="2"
                                                            Width="176px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2">
                                                        
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>

                                    <div class="modal-header">
                                        <asp:Button ID="btnDivOK" runat="server" CssClass="btn btnnew" OnClick="btnDivOK_click" OnClientClick="return Validation();" Text="OK" />
                                        &nbsp;<asp:Button ID="btnDivCancel" runat="server" CssClass="btn btncancel" OnClick="btnDivCancel_Click" Text="Cancel" />
                                    </div>
                                </div>
                                <table>
                                    <tr>
                                        <td>
                                            <button id="btn2" runat="server" style="display: none;" />
                                            <cc1:ModalPopupExtender ID="MpeAudit" runat="server" BackgroundCssClass="modalBackground"
                                                CancelControlID="Img1" PopupControlID="divHistoryDtl" TargetControlID="btn2"
                                                BehaviorID="MpeAudit">
                                            </cc1:ModalPopupExtender>
                                            <div id="divHistoryDtl" runat="server" class="ModalPopup" style="display: none; width: 60%;
                                                margin: auto; position: absolute; top: 525px; max-height: 404px; background-color: white;">
                                                <div>
                                                    <div style="text-align: center;">
                                                        <h1 class="header">
<%--                                                            <img id="Img1" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right;
                                                                right: 5px;" title="Close" />--%>
                                                            <asp:ImageButton runat="server" id="Img1" src="images/Sqclose.gif" style="position: relative; float: right;
                                                                right: 5px;" title="Close" OnClientClick="return $('<%= divHistoryDtl.clientID %>').hide();"/>   

                                                            <asp:Label ID="lblTitle" Text="Audit Trail" runat="server" class="LabelBold"></asp:Label>
                                                        </h1>
                                                    </div>
                                                </div>
                                                <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                                    <tbody>
                                                        <tr>
                                                            <td>
                                                                <strong style="white-space: nowrap">Test History Of
                                                                    <asp:Label ID="lblMedexDescription" runat="server"></asp:Label></strong>
                                                            </td>
                                                            <%-- <td style="text-align: right; padding-right:15Px">
                                                                        <img onclick="DivShowHide('H');" src="images/close.gif" />
                                                                    </td>--%>
                                                        </tr>
                                                        <%-- 
                                                                <tr>--%>
                                                        <td style="text-align: center;" colspan="2">
                                                            <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto">
                                                                <%--nSubjectLabReportNo,nSampleId,vMedexCode,itranNo,Abnormal,ClinicallySig,vGeneralRemark,iModifyBy,dModifyOn,cStatusIndi,vMedExDesc,vUserName--%>
                                                                <asp:GridView ID="GVHistoryDtl" runat="server" AutoGenerateColumns="False" BorderColor="Peru"
                                                                    Font-Size="Small" SkinID="grdViewSmlAutoSize" Width="100%">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="vMedExDesc" HeaderText="Test">
                                                                            <ItemStyle Wrap="false" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Abnormal" HeaderText="Abnormal and NotCS">
                                                                            <ItemStyle Wrap="false" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="ClinicallySig" HeaderText="Abnormal and CS">
                                                                            <ItemStyle Wrap="false" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="vGeneralRemark" HeaderText="Remarks">
                                                                            <ItemStyle Wrap="false" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="dModifyOn_IST" DataFormatString="{0:f}" HeaderText="Modified On"
                                                                            HtmlEncode="False">
                                                                            <ItemStyle Wrap="false" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="vUserName" HeaderText="Modified By">
                                                                            <ItemStyle Width="20%" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="nSampleId" HeaderText="SampleId" />
                                                                        <asp:BoundField DataField="vMedexCode" HeaderText="MedexCode" />
                                                                        <asp:BoundField DataField="itranNo" HeaderText="TranNo" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </td>
                                                        <%--</tr>--%>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <button id="btn3" runat="server" style="display: none;" />
                                            <cc1:ModalPopupExtender ID="MPEfinalAudit" runat="server" BackgroundCssClass="modalBackground"
                                                CancelControlID="Img2" PopupControlID="divAudit" TargetControlID="btn3" BehaviorID="MPEfinalAudit">
                                            </cc1:ModalPopupExtender>
                                            <div id="divAudit" runat="server" class="ModalPopup" style="display: none; margin: auto;
                                                width: 65%; position: absolute; top: 525px; max-height: 404px; background-color: White;">
                                                <table style="width: 100%" cellpadding="5px">
                                                    <tbody>
                                                        <tr>
                                                            <td style="text-align: center;" class="LabelBold ;" colspan="2">
                                                                <h1 class="header">
                                                                    Remarks for SampleID
                                                                    <asp:Label ID="lblSampleID" runat="server"></asp:Label></strong>
                                                                    <img id="Img2" onclick="DivAuditShowHide('H');" alt="Close" src="images/Sqclose.gif"
                                                                        title="Close" style="float: right;" />
                                                                </h1>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: center" colspan="2">
                                                                <asp:GridView ID="GVAuditFnlRmk" runat="server" AutoGenerateColumns="False" BorderColor="Peru"
                                                                    Font-Size="Small" SkinID="grdViewSmlAutoSize" Width="100%">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="vUserName" HeaderText="Reviewed By">
                                                                            <ItemStyle Width="30%" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="dReviewedOn_IST" DataFormatString="{0:f}" HeaderText="Reviewed On"
                                                                            HtmlEncode="False">
                                                                            <ItemStyle Wrap="false" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="vRemarks" HeaderText="Remarks">
                                                                            <ItemStyle Wrap="false" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="nSampleId" HeaderText="SampleId" />
                                                                        <asp:BoundField DataField="iReviewedBy" HeaderText="iReviewedBy" />
                                                                        <asp:BoundField DataField="iTranNo" HeaderText="TranNo" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <button id="btn4" runat="server" style="display: none;" />
                                            <cc1:ModalPopupExtender ID="Mpetrf" runat="server" BackgroundCssClass="modalBackground"
                                                CancelControlID="Img3" PopupControlID="divTRF" TargetControlID="btn4" BehaviorID="Mpetrf">
                                            </cc1:ModalPopupExtender>
                                            <div id="divTRF" runat="server" class="ModalPopup" style="display: none; margin: auto;
                                                width: 60%; position: absolute; top: 525px; max-height: 484px; background-color: White;">
                                                <table style="width: 100%" cellpadding="5px">
                                                    <tbody>
                                                        <tr>
                                                            <td style="text-align: center;" colspan="2">
                                                                <h1 class="header ">
                                                                    TRF of SampleID
                                                                    <asp:Label ID="lblTRF" runat="server" Font-Bold="True"></asp:Label></strong>
                                                                    <img id="Img3" onclick="DivShowHideTRF('H');" src="images/Sqclose.gif" alt="Close"
                                                                        title="Close" style="float: right;" />
                                                                </h1>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="height: 20px">
                                                                <strong style="white-space: nowrap">Sample Collected on:
                                                                    <asp:Label ID="lblSampleCollectedOn" runat="server"></asp:Label></strong>
                                                            </td>
                                                            <td>
                                                                <strong style="white-space: nowrap">Sample Collected by:
                                                                    <asp:Label ID="lblCollectedby" runat="server"></asp:Label></strong>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="height: 20px">
                                                                <strong style="white-space: nowrap">Sample Received on:
                                                                    <asp:Label ID="lblSampleReceivedOn" runat="server"></asp:Label></strong>
                                                            </td>
                                                            <td>
                                                                <strong style="white-space: nowrap">Sample Received by:
                                                                    <asp:Label ID="lblReceivedby" runat="server"></asp:Label></strong>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" colspan="2">
                                                                <asp:Panel ID="pnlMedExGrid" runat="server" Height="140px" ScrollBars="Auto" Width="100%">
                                                                    <asp:GridView ID="gvwTRF" runat="server" AutoGenerateColumns="False" PageSize="25"
                                                                        SkinID="grdViewSmlAutoSize" Width="100%">
                                                                        <Columns>
                                                                            <asp:BoundField HeaderText="#">
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="vMedexDesc" HeaderText="TestName" />
                                                                            <asp:BoundField DataField="vTemplateName" HeaderText="TestTemplate/Profile Name" />
                                                                            <%--<asp:BoundField DataField="nSampleId" HeaderText="nSampleId" />
                                                                            <asp:BoundField DataField="vSampleTypeDesc" HeaderText="SampleType Desc" />--%>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" colspan="2">
                                                                <strong style="white-space: nowrap">SampleType Information</strong>
                                                                <asp:GridView ID="gvwSampletype" runat="server" AutoGenerateColumns="False" PageSize="25"
                                                                    SkinID="grdViewSmlAutoSize" Width="100%">
                                                                    <Columns>
                                                                        <asp:BoundField HeaderText="#">
                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="vSampleTypeDesc" HeaderText="Sample Type" />
                                                                        <asp:BoundField DataField="fVolume" HeaderText="Volume" />
                                                                        <%-- <asp:BoundField DataField="nSampleId" HeaderText="nSampleId" />--%>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>

                </ContentTemplate>
                <Triggers>
                    
                    <asp:AsyncPostBackTrigger ControlID="gvwtstChemistry" EventName="RowCommand" />
                    <asp:AsyncPostBackTrigger ControlID="gvwtstChemistry" EventName="DataBinding" />
                    <asp:AsyncPostBackTrigger ControlID="btnFnlRmkAudit" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnDivOK" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="rblSampleid" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="chkReviewed" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="chkInProject" EventName="CheckedChanged" />
                    
                </Triggers>
            </asp:UpdatePanel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnTRF" EventName="Click"></asp:AsyncPostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
    
    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" src="Script/Gridview.js"></script>

    <script src="Script/Validation.js" type="text/javascript"></script>

    <script type="text/javascript" src="Script/General.js"></script>


    <script type="text/javascript" language="javascript">

        function ClientPopulated1(sender, e) {
            SubjectClientShowing('AutoCompleteExtender1', $get('<%= txtSubject.ClientId %>'));
        }
        function ClientPopulated2(sender, e) {
            //SubjectClientShowing_OnlyID('AutoCompleteExtender1', $get('<%= txtSubject.ClientId %>'));
            SubjectClientShowing('AutoCompleteExtender1', $get('<%= txtSubject.ClientId %>'));
        }
        function OnSelected1(sender, e) {
            SubjectOnItemSelected(e.get_value(), $get('<%= txtSubject.clientid %>'),
         $get('<%= HSubjectId.clientid %>'), document.getElementById('<%= btnSubject.ClientId %>'));
        }

        function confirmation(e) {
            var field = 'Type';
            var url = window.location.href;
            if (url.indexOf('?' + field + '=') != -1)
                return true;
            else if (url.indexOf('&' + field + '=') != -1)
                return true;
            else {
                msgConfirmDeleteAlert(null, "Have You Saved Your Changes In Test Group,You Just Reviewed ?", function (isConfirmed) {
                    if (isConfirmed) {
                        var name = e.name;
                        name = e.href.replace("javascript:__doPostBack('", "").replace("','')", "");
                        __doPostBack(name, '');
                        return true;
                    } else {
                        return false;
                    }
                });
                return false;
            }

            

        }
        function DivShowHide(Type) {
            if (Type == 'S') {
                document.getElementById('divHistoryDtl').style.display = '';
                SetCenter('divHistoryDtl');
                ChangedDisabled();
            }
            else if (Type == 'H') {
                document.getElementById('divHistoryDtl').style.display = 'none';
            }
        }

        function DivAuditShowHide(Type) {
            if (Type == 'S') {
                document.getElementById('divAudit').style.display = '';
                SetCenter('divAudit');
                ChangedDisabled();
            }
            else if (Type == 'H') {
                document.getElementById('divAudit').style.display = 'none';
            }
        }


        function DivShowHideTRF(Type) {
            if (Type == 'S') {
                document.getElementById('divTRF').style.display = '';
                SetCenter('divTRF');
                ChangedDisabled();
            }
            else if (Type == 'H') {
                document.getElementById('divTRF').style.display = 'none';
            }
        }




        function ChangedDisabled() {


            var TargetBaseControl = document.getElementById('<%=gvwtstChemistry.ClientID%>');
            var TargetChildControl = "ChkEdit";
            var TargetChildControl1 = "ChkAbnormal";
            var TargetChildControl2 = "ChkClSignificant";
            var TargetChildControl3 = "txtRemark";

            var Inputs = TargetBaseControl.getElementsByTagName("input");

            for (var n = 0; n < Inputs.length; ++n) {
                if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0) {

                    if (Inputs[n].checked) {

                        if (Inputs[n + 2].type == 'checkbox' && Inputs[n + 2].id.indexOf(TargetChildControl1, 1) >= 0) {

                            if (Inputs[n + 2].disabled != false) {

                                Inputs[n + 2].disabled = false;
                                Inputs[n + 2].checked = false;

                            }
                        }
                        if (Inputs[n + 3].type == 'checkbox' && Inputs[n + 3].id.indexOf(TargetChildControl2, 2) >= 0) {

                            if (Inputs[n + 3].disabled != false) {
                                Inputs[n + 3].disabled = false;
                                Inputs[n + 3].checked = false;
                            }
                        }
                        if (Inputs[n + 4].type == 'text' && Inputs[n + 4].id.indexOf(TargetChildControl3, 3) >= 0) {
                            if (Inputs[n + 4].disabled != false) {
                                Inputs[n + 4].disabled = false;
                                Inputs[n + 4].value = "";
                            }
                        }

                    }
                    else {

                        // drpdown.disabled = false;


                        if (Inputs[n + 2].type == 'checkbox' && Inputs[n + 2].id.indexOf(TargetChildControl1, 1) >= 0) {

                            Inputs[n + 2].disabled = true;



                        }
                        if (Inputs[n + 3].type == 'checkbox' && Inputs[n + 3].id.indexOf(TargetChildControl2, 2) >= 0) {
                            Inputs[n + 3].disabled = true;
                            //Inputs[n+2].value="";        
                        }
                        if (Inputs[n + 4].type == 'text' && Inputs[n + 4].id.indexOf(TargetChildControl3, 3) >= 0) {
                            Inputs[n + 4].disabled = true;
                            //Inputs[n+3].value="";        
                        }

                    }
                }
            }
        }


        function ValidateCheck1(id) {

            //  $find('mpeAbnormalRemark').hide();
            //  var chk = document.getElementById(id.id);
            //  
            //  
            //  if (chk.checked) {
            //  
            //      document.getElementById('<%=hdnCheckBox.ClientID%>').value = chk.id
            //      $find('mpeAbnormalRemark').show();
            //  }
            
           
            
            //jQuery.find('mpeAbnormalRemark').Show()


           var TargetBaseControl = document.getElementById('<%=gvwtstChemistry.ClientID%>');
           var TargetChildControl = "ChkAbnormal";
           var TargetChildControl1 = "ChkClSignificant";
           var Inputs = TargetBaseControl.getElementsByTagName("input");
          
           for (var n = 0; n < Inputs.length - 2; ++n) {
               if (Inputs[n + 2].id.indexOf(TargetChildControl, 0) >= 0) {
                   if (Inputs[n + 2].checked) {
                       if (Inputs[n + 3].id.indexOf(TargetChildControl1, 1) >= 0) {
                           Inputs[n + 3].checked = false;
                       }
                   }
               }
           }
            
        }



        function ValidateCheck2(id) {

           //  $find('mpeAbnormalRemark').hide();
           //  var chk = document.getElementById(id.id);
           // 
           // 
           //  if (chk.checked) {
           //      document.getElementById('<%=hdnCheckBox.ClientID%>').value = chk.id
           //      $find('mpeAbnormalRemark').show();
           //  }


            var TargetBaseControl = document.getElementById('<%=gvwtstChemistry.ClientID%>');
            var TargetChildControl = "ChkAbnormal";
            var TargetChildControl1 = "ChkClSignificant";
            var Inputs = TargetBaseControl.getElementsByTagName("input");

            for (var n = 0; n < Inputs.length - 2; ++n) {
                if (Inputs[n + 3].id.indexOf(TargetChildControl1, 1) >= 0) {
                    if (Inputs[n + 3].checked) {
                        if (Inputs[n + 2].id.indexOf(TargetChildControl, 0) >= 0) {
                            Inputs[n + 2].checked = false;
                        }
                    }
                }
            }
        }

        function ValidateTest(e) {

            var TargetBaseControl = document.getElementById('<%=gvwtstChemistry.ClientID%>');
            var TargetChildControl = "ChkNormal";

            var Inputs = TargetBaseControl.getElementsByTagName("input");
            var tests = TargetBaseControl.getElementsByTagName("asp:BoundField");


            for (var n = 0; n < Inputs.length; ++n) {
                if (n != Inputs.length - 1) {
                    if (Inputs[n + 1].type == 'checkbox' && Inputs[n + 1].id.indexOf(TargetChildControl, 0) >= 0) {
                        if (Inputs[n + 1].checked == false) {

                            if (Inputs[n + 2].checked == false) {

                                if (Inputs[n + 3].checked == false) {

                                    if (Inputs[n + 4].value.trim() == '') {
                                        Inputs[n + 4].parentElement.parentElement.style.backgroundColor = 'red';
                                        msgConfirmDeleteAlert(null, "Test Shown In Red Color Is Not Reviewed.Do You Still Want To Continue ?", function (isConfirmed) {
                                            if (isConfirmed) {
                                                __doPostBack(e.name, '');
                                                return true;
                                            } else {

                                                return false;
                                            }
                                        });
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        function Highlight(row) {
            row.style.backgroundColor = '#e7f2fd';
        }
        function UnHighlight(row) {
            row.style.backgroundColor = '#FFF';
        }



        function $(id) {
            return document.getElementById(id);
        }


        function DivHideTemp() {
            document.getElementById('div1').style.display = "none";
            document.getElementById('div2').style.display = "none";
        }


        function ShowHideDiv(Divid, ImageId) {
            var e = $(Divid);
            var f = $(ImageId);

            if (e.style.display == 'none') {
                e.style.display = '';
                f.src = "images/collapse.jpg";
            }
            else {
                e.style.display = 'none';
                f.src = "images/expand.jpg";
            }
        }


        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender2', $get('<%=txtProject.ClientId%>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%=txtProject.clientid%>'),
         $get('<%=HProjectId.clientid%>'), document.getElementById('<%=btnSetProject.ClientId%>'));
        }

        function ShowHideDivRmk() {
            var chk = document.getElementById('<%= chkReviewed.clientId%>');
            $find('MpeSearch').hide();
            if (chk.checked) {
                document.getElementById('<%= txtLockRemark.ClientId %>').value = '';
                $find('MpeSearch').show();
            }
            ChangedDisabled();
        }

        function Validation() {
            if (document.getElementById('<%=txtLockRemark.clientid%>').value.toString().trim() == '') {
                msgalert('Please Enter Remarks !');
                document.getElementById('<%= txtLockRemark.ClientId %>').focus();
                document.getElementById('<%= txtLockRemark.ClientId %>').value = '';
                return false;
            }
            return true;
        }

        function closewindow() {
            var parWin = window.opener;
            if (parWin != null && typeof (parWin) != 'undefined') {
                if (parWin && parWin.open && !parWin.closed) {
                    window.parent.document.location.reload();
                }
            }
            self.close();
        }

       // function Close() {
          //  var chk = document.getElementById('<%=hdnCheckBox.ClientID%>').value;
            // if (chk.checked == true) {
         //   var chk1 = document.getElementById(chk)
           // chk1.checked = ''
            //}
           // $find('mpeAbnormalRemark').hide();
      //  }

        function RemarksSave() {
            var btn = document.getElementById('<%= btnSave.ClientID %>')
            btn.click();
        }
        
        function pageLoad() {
            jQuery("#ctl00_CPHLAMBDA_RBLSubject").find("td [checked=checked]").focus()

        }

       
    </script>

</asp:content>
