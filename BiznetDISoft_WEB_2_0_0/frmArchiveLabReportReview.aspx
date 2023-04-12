<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmArchiveLabReportReview.aspx.vb" Inherits="frmArchiveLabReportReview" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <%--CHEMISTRY,IMMUNOLOGY,HEMATOLOGY,URIANALYSIS,COAGULATION--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table style="width: 890px" align="center">
                <tbody>
                    <tr style="width: 100%">
                        <td style="width: 100%" align="top" colspan="6">
                        </td>
                    </tr>
                    <tr style="width: 100%">
                        <td style="width: 100%; height: 41px" class="Label" align="center" colspan="6">
                            <asp:RadioButtonList ID="RBLProjecttype" runat="server" AutoPostBack="True" Width="289px"
                                RepeatDirection="Horizontal" OnSelectedIndexChanged="RBLProjecttype_SelectedIndexChanged"
                                __designer:wfdid="w4">
                                <asp:ListItem Selected="true" Value="0000000000">Generic Screening</asp:ListItem>
                                <asp:ListItem Value="1">Project Specific</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr style="width: 50%" align="center">
                        <td style="width: 790px; height: 20px; background-color: #1560a1" align="center"
                            colspan="6">
                            <div style="padding-right: 3px; padding-left: 3px; min-height: 8px; padding-bottom: 0px;
                                vertical-align: middle; width: 100%; cursor: pointer; padding-top: 0px; height: 8px;
                                background-color: #1560a1; min-width: 100%" id="div1" onclick="ShowHideDiv('<%=divSubjectSelection.ClientID %>','imgSubjectSelection')">
                                <div style="font-weight: bold; font-size: 12px; float: none; vertical-align: middle;
                                    color: white">
                                    Select Subject
                                </div>
                            </div>
                        </td>
                        <td style="width: 10px; height: 20px; background-color: #1560a1">
                            <div style="float: right; vertical-align: middle">
                                <img id="imgSubjectSelection" alt="Image" src="images/expand.jpg" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <div style="display: none; width: 100%; min-width: 100%" id="divSubjectSelection"
                                class="collapsePanel" align="center" runat="server">
                                <asp:UpdatePanel ID="upSubjectSelection" runat="server" UpdateMode="Conditional"
                                    __designer:wfdid="w5" ChildrenAsTriggers="False">
                                    <ContentTemplate>
                                        <table>
                                            <tbody>
                                                <tr style="display: none; width: 100%" id="trSearchSub" runat="Server">
                                                    <td style="width: 62px; white-space: nowrap" align="right">
                                                        <strong class="Label">Subject:</strong>
                                                    </td>
                                                    <td style="width: 752px; white-space: nowrap" align="left">
                                                        <asp:TextBox ID="txtSubject" TabIndex="2" runat="server" CssClass="textBox" __designer:wfdid="w6"
                                                            Width="618px"></asp:TextBox>
                                                        <asp:Button Style="display: none" ID="btnSubject" runat="server" Text="Subject" __designer:wfdid="w7">
                                                        </asp:Button>
                                                        <asp:HiddenField ID="HSubjectId" runat="server" __designer:wfdid="w8"></asp:HiddenField>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" __designer:wfdid="w9"
                                                            BehaviorID="AutoCompleteExtender1" MinimumPrefixLength="1" OnClientItemSelected="OnSelected1"
                                                            OnClientShowing="ClientPopulated1" ServiceMethod="GetSubjectCompletionList_NotRejected"
                                                            ServicePath="AutoComplete.asmx" TargetControlID="txtSubject" UseContextKey="True">
                                                        </cc1:AutoCompleteExtender>
                                                    </td>
                                                </tr>
                                                <tr style="display: none" id="trProject" runat="server">
                                                    <td style="width: 62px" align="right">
                                                        <strong class="Label">Project :</strong>
                                                    </td>
                                                    <td style="width: 752px; white-space: nowrap" align="left">
                                                        <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="622px" __designer:wfdid="w10"></asp:TextBox><asp:CheckBox
                                                            ID="chkInProject" runat="server" AutoPostBack="True" Width="88px" __designer:wfdid="w11"
                                                            OnCheckedChanged="chkInProject_CheckedChanged" Text="InProject"></asp:CheckBox><asp:Button
                                                                Style="display: none" ID="btnSetProject" runat="server" Text=" Project" __designer:wfdid="w12">
                                                            </asp:Button><asp:HiddenField ID="HProjectId" runat="server" __designer:wfdid="w13">
                                                            </asp:HiddenField>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" __designer:wfdid="w14"
                                                            BehaviorID="AutoCompleteExtender2" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                                            OnClientShowing="ClientPopulated" ServiceMethod="GetMyProjectCompletionList"
                                                            ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True"
                                                            CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                            CompletionListItemCssClass="autocomplete_listitem">
                                                        </cc1:AutoCompleteExtender>
                                                    </td>
                                                </tr>
                                                <tr style="display: none; width: 100%" id="trActivity" runat="server">
                                                    <td style="width: 62px; white-space: nowrap; height: 21px" align="right">
                                                        <strong class="Label">Activity :</strong>
                                                    </td>
                                                    <td style="width: 752px; white-space: nowrap; height: 21px" align="left">
                                                        <asp:DropDownList ID="ddlActivity" TabIndex="3" runat="server" CssClass="dropDownList"
                                                            __designer:wfdid="w15" Width="580px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="trperiod" runat="Server" visible="false">
                                                    <td style="width: 62px; white-space: nowrap; height: 21px" align="right">
                                                        <strong class="Label">Periods :</strong>
                                                    </td>
                                                    <td style="width: 52px; white-space: nowrap; height: 21px" align="left">
                                                        <asp:DropDownList ID="ddlPeriods" AutoPostBack="true" TabIndex="4" runat="server"
                                                            CssClass="dropDownList" __designer:wfdid="w15" Width="80px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr style="display: none" id="trSubList" runat="Server">
                                                    <td style="width: 62px; height: 120px" align="right">
                                                        <strong class="Label">Subjects :</strong>
                                                    </td>
                                                    <td style="width: 752px; height: 120px" align="left">
                                                        <asp:Panel ID="pnlSubject" runat="server" __designer:wfdid="w16" ScrollBars="Auto"
                                                            BorderStyle="Solid" BorderWidth="1px" Height="100px">
                                                            <asp:RadioButtonList ID="RBLSubject" runat="server" AutoPostBack="True" __designer:wfdid="w17"
                                                                OnSelectedIndexChanged="RBLSubject_SelectedIndexChanged" RepeatColumns="2">
                                                            </asp:RadioButtonList>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <asp:HiddenField ID="HschemaId" runat="server" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click"></asp:AsyncPostBackTrigger>
                                        <asp:AsyncPostBackTrigger ControlID="ddlperiods"></asp:AsyncPostBackTrigger>
                                        <asp:AsyncPostBackTrigger ControlID="chkInProject" EventName="CheckedChanged"></asp:AsyncPostBackTrigger>
                                    </Triggers>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="upscreeningDtl" runat="server" UpdateMode="Conditional" __designer:wfdid="w18"
                                    ChildrenAsTriggers="False">
                                    <ContentTemplate>
                                        <table width="710">
                                            <tbody>
                                                <tr align="left" id="trScreeningDate" runat="server">
                                                    <td style="width: 10%; height: 55px" align="right">
                                                        <strong class="Label">Screening Dates:</strong>
                                                    </td>
                                                    <td style="width: 90%; height: 55px" align="left">
                                                        <asp:RadioButtonList ID="rblScreeningDate" runat="server" CssClass="Label" AutoPostBack="True"
                                                            RepeatDirection="Horizontal" __designer:wfdid="w1" RepeatColumns="3">
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 10%; height: 26px" align="right">
                                                        <strong class="Label">Samples :</strong>
                                                    </td>
                                                    <td style="width: 90%; height: 26px" align="left">
                                                        <asp:RadioButtonList ID="rblSampleid" runat="server" CssClass="Label" AutoPostBack="True"
                                                            RepeatDirection="Horizontal" __designer:wfdid="w2" RepeatColumns="3">
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr style="display: none" id="trbtnTRF" align="left" runat="server">
                                                    <td style="width: 10%; height: 55px" align="right">
                                                    </td>
                                                    <td style="display: block; vertical-align: top; width: 90%; height: 55px" align="left">
                                                        <asp:Button Style="display: block" ID="btnTRF" OnClick="btnTRF_Click" runat="Server"
                                                            CssClass="btn btnnew" __designer:wfdid="w3" Text="TRF"></asp:Button>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="RBLSubject" EventName="SelectedIndexChanged">
                                        </asp:AsyncPostBackTrigger>
                                        <asp:AsyncPostBackTrigger ControlID="rblScreeningDate" EventName="SelectedIndexChanged">
                                        </asp:AsyncPostBackTrigger>
                                        <asp:AsyncPostBackTrigger ControlID="btnSubject" EventName="Click"></asp:AsyncPostBackTrigger>
                                        <asp:AsyncPostBackTrigger ControlID="btnTRF" EventName="Click"></asp:AsyncPostBackTrigger>
                                        <asp:AsyncPostBackTrigger ControlID="rblSampleid" EventName="SelectedIndexChanged">
                                        </asp:AsyncPostBackTrigger>
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 300px">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" __designer:wfdid="w22" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <%--<asp:UpdatePanel id="UpdatePanel2" runat="server" UpdateMode="Conditional"><ContentTemplate>--%><table
                                        width="100%">
                                        <tbody>
                                            <tr runat="server" id="TrTop" align="left" visible="false">
                                                <td colspan="6">
                                                    <hr />
                                                </td>
                                            </tr>
                                            <tr runat="server" id="TrScreening" align="left" visible="false">
                                                <td colspan="3">
                                                    <asp:Label ID="Label1" runat="server" Text="Screening No :"></asp:Label>
                                                    <asp:Label ID="LblScrNo" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:Label ID="Label11" runat="server" Text="Lab Id :"></asp:Label>
                                                    <asp:Label ID="LblLabID" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label17" runat="server" Text="Sample Collected On :"></asp:Label>
                                                    <asp:Label ID="LblSampleClDt" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="Tr1" align="left" visible="false">
                                                <td colspan="3">
                                                    <asp:Label ID="Label3" runat="server" Text="Subject Initials:"></asp:Label>
                                                    <asp:Label ID="LblSub" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:Label ID="Label13" runat="server" Text="Sex :"></asp:Label>
                                                    <asp:Label ID="LblSex" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label19" runat="server" Text="Sample Collected At :"></asp:Label>
                                                    <asp:Label ID="LblSampleClAt" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="Tr2" align="left" visible="false">
                                                <td colspan="3">
                                                    <asp:Label ID="Label5" runat="server" Text="Sample Received On :"></asp:Label>
                                                    <asp:Label ID="LblSampleRcvDt" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:Label ID="Label15" runat="server" Visible="false" Text="Visit :"></asp:Label>
                                                    <asp:Label ID="LblVisit" runat="server" Visible="false" Text=""></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label21" runat="server" Text="Report Date :"></asp:Label>
                                                    <asp:Label ID="LblRptDt" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="Tr3" visible="false">
                                                <td colspan="3" align="left">
                                                    <asp:Label ID="Label7" runat="server" Text="Birth Date :"></asp:Label>
                                                    <asp:Label ID="LblBDate" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td colspan="2" align="left">
                                                    <asp:Label ID="Label2" runat="server" Text="Subject No:"></asp:Label>
                                                    <asp:Label ID="lblSubNo" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="Label23" runat="server" Text="Study/Project :"></asp:Label>
                                                    <asp:Label ID="LblPrj" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="Tr4" visible="false">
                                                <td colspan="3" align="left">
                                                    <asp:Label ID="Label9" runat="server" Text="Reffered By :"></asp:Label>
                                                    <asp:Label ID="LblRfrdBy" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr runat="server" id="TrTopLine" visible="false">
                                                <td colspan="6">
                                                    <hr />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 105px; white-space: nowrap; height: 37px" align="left">
                                                    <asp:LinkButton Style="display: none" ID="lnkChemistry" OnClick="lnkChemistry_Click"
                                                        runat="server" ForeColor="White" Text="CHEMISTRY" CssClass="TABButton" __designer:wfdid="w24"
                                                        Width="105px" Height="25px"></asp:LinkButton>
                                                </td>
                                                <td style="width: 105px; white-space: nowrap; height: 37px" align="left">
                                                    <asp:LinkButton Style="display: none" ID="LnkIMMUNOLOGY" OnClick="LnkIMMUNOLOGY_Click"
                                                        runat="server" ForeColor="White" Text="IMMUNOLOGY" CssClass="TABButton" __designer:wfdid="w25"
                                                        Width="105px" Height="25px"></asp:LinkButton>
                                                </td>
                                                <td style="width: 105px; white-space: nowrap; height: 37px" align="left">
                                                    <asp:LinkButton Style="display: none" ID="lnkHEMATOLOGY" OnClick="lnkHEMATOLOGY_Click"
                                                        runat="server" ForeColor="White" Text="HEMATOLOGY" CssClass="TABButton" __designer:wfdid="w26"
                                                        Width="105px" Height="25px"></asp:LinkButton>
                                                </td>
                                                <td style="width: 105px; white-space: nowrap; height: 37px" align="left">
                                                    <asp:LinkButton Style="display: none" ID="lnkURIANALYSIS" OnClick="lnkURIANALYSIS_Click"
                                                        runat="server" ForeColor="White" Text="URIANALYSIS" CssClass="TABButton" __designer:wfdid="w27"
                                                        Width="105px" Height="25px"></asp:LinkButton>
                                                </td>
                                                <td style="width: 105px; white-space: nowrap; height: 37px" align="left">
                                                    <asp:LinkButton Style="display: none" ID="lnkCOAGULATION" OnClick="lnkCOAGULATION_Click"
                                                        runat="server" ForeColor="White" Text="COAGULATION" CssClass="TABButton" __designer:wfdid="w28"
                                                        Width="105px" Height="25px"></asp:LinkButton>
                                                </td>
                                                <td style="width: 350px; white-space: nowrap; height: 27px" align="left">
                                                    <asp:LinkButton Style="display: none" ID="lnkSTOOL" OnClick="lnkSTOOL_Click" runat="server"
                                                        ForeColor="White" Text="STOOL EXAMINATION" CssClass="TABButton" __designer:wfdid="w29"
                                                        Width="180px" Height="25px"></asp:LinkButton>&nbsp;
                                                    <asp:LinkButton Style="display: none" ID="lnkHIV1" OnClick="lnkHIV1_Click" runat="server"
                                                        ForeColor="White" Text="WESTERN BLOT for HIV -I" CssClass="TABButton" __designer:wfdid="w30"
                                                        Width="220px" Height="25px"></asp:LinkButton>&nbsp;
                                                    <asp:LinkButton Style="display: none" ID="lnkPAP" OnClick="lnkPAP_Click" runat="server"
                                                        ForeColor="White" Text="PAP Smear Report" CssClass="TABButton" __designer:wfdid="w31"
                                                        Width="220px" Height="25px"></asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100%" colspan="6">
                                                    <table cellpadding="0" width="890">
                                                        <tbody>
                                                            <tr>
                                                                <td align="left" colspan="6" style="height: 229px">
                                                                    <asp:GridView ID="gvwtstChemistry" runat="server" __designer:wfdid="w32" AllowPaging="false"
                                                                        AutoGenerateColumns="False" SkinID="grdViewAutoSizeMax" style="width:70%; margin:auto;">
                                                                        <Columns>
                                                                            <asp:BoundField HeaderText="#">
                                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Update">
                                                                                <ItemTemplate>
                                                                                    <input id="ChkEdit" runat="server" onclick="ChangedDisabled();" type="checkbox" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="vMedexDesc" HeaderText="Test">
                                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="vMedexResult" HeaderText="Test Result"></asp:BoundField>
                                                                            <asp:BoundField DataField="vLowRange" HeaderText="Low Range"></asp:BoundField>
                                                                            <asp:BoundField DataField="vHighRange" HeaderText="High Range"></asp:BoundField>
                                                                            <asp:BoundField DataField="vUOM" HeaderText="UOM"></asp:BoundField>
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
                                                                                        ForeColor="Blue" onclick="ValidateCheck1();" type="checkbox" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Abormal &amp; CS">
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="ChkClSignificant" runat="server" onclick="ValidateCheck2();" type="checkbox" />
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
                                                                                    <asp:ImageButton ID="ImgAudit" runat="server" ImageUrl="~/Images/paste.png" Visible="false" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="nSampleId" HeaderText="SampleId"></asp:BoundField>
                                                                            <asp:BoundField DataField="tranno" HeaderText="tranno"></asp:BoundField>
                                                                            <asp:BoundField DataField="cNormalflag" HeaderText="Normalflag"></asp:BoundField>
                                                                            <asp:BoundField DataField="cAbnormalflag" HeaderText="Abnormalflag"></asp:BoundField>
                                                                            <asp:BoundField DataField="cClinicallySignflag" HeaderText="ClinicallySignflag">
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="vGeneralRemark" HeaderText="GeneralRemark"></asp:BoundField>
                                                                            <asp:BoundField DataField="vmedexcode" HeaderText="medexcode"></asp:BoundField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                            <tr id="trRptRemarks" runat="server" style="display: none; width: 100%">
                                                                <td style="height: 44px; width: 15%;" align="left" colspan="1">
                                                                    <strong class="Label">TestGroup Remark(from Lab):</strong>
                                                                </td>
                                                                <td align="left" colspan="5" style="width: 85%">
                                                                    <textarea id="txtRptRemarks" runat="server" disabled style="font-size: small; width: 721px;
                                                                        color: black"></textarea>
                                                                </td>
                                                            </tr>
                                                            <tr align="left">
                                                                <td style="width: 15%; height: 6px" align="right">
                                                                </td>
                                                                <td style="width: 85%; height: 6px;" colspan="5">
                                                                    <asp:Label ID="lblReviedby" runat="server" __designer:wfdid="w33" SkinID="lblHeading"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr id="trReviewed" runat="server" style="display: none">
                                                                <td style="height: 29px" colspan="6">
                                                                    <asp:CheckBox ID="chkReviewed" runat="server" __designer:wfdid="w34" onclick="ShowHideDivRmk();"
                                                                        Text="I Have Reviewed and Data is now ready to Lock" />
                                                                </td>
                                                            </tr>
                                                            <tr runat="server" id="TrBotomTopLine" visible="false">
                                                                <td colspan="6">
                                                                    <hr />
                                                                </td>
                                                            </tr>
                                                            <tr runat="server" id="TrFooter" visible="false">
                                                                <td colspan="6" align="center">
                                                                    <asp:Label runat="server" ID="LblRptAuthOn" Text="Report Authenticated On :"></asp:Label>
                                                                    <asp:Label runat="server" ID="LblRtpAutDate" Text=""></asp:Label>
                                                                    <asp:Label runat="server" ID="LblAuthBy" Text="Authenticated By :"></asp:Label>
                                                                    <asp:Label runat="server" ID="LblAuthdBy" Text=""></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr runat="server" id="TrFooterLine" visible="false">
                                                                <td colspan="6">
                                                                    <hr />
                                                                </td>
                                                            </tr>
                                            </tr>
                                            <tr>
                                                <td style="height: 19px" colspan="6">
                                                    &nbsp;
                                                    <asp:Button ID="btnFnlRmkAudit" runat="server" __designer:wfdid="w35" CssClass="btn btnaudit"
                                                        OnClick="btnFnlRmkAudit_Click" Text="Audit Trail(Final Remarks)" Visible="False"
                                                        wfdid="w31" />
                                                    <%--<asp:Button ID="btnSave" runat="server" __designer:wfdid="w36" CssClass="button"
                                                        OnClick="btnSave_Click" OnClientClick="return ValidateTest();" Text="Save and Continue"
                                                        Width="123px" />
                                                    <asp:Button ID="btnCancel" runat="server" __designer:wfdid="w37" CssClass="button"
                                                        OnClick="btnCancel_Click" OnClientClick="dfd();" Text="Cancel" />--%>
                                                    <asp:Button ID="btnExit" runat="server" __designer:wfdid="w38" CssClass="btn btnexit"
                                                        OnClick="btnExit_Click" Text="Exit" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 300px;">
                                                    <div id="divSearch" runat="server" class="DIVSTYLE2" style="display: none; left: 638px;
                                                        width: 296px; top: 392px; height: 87px">
                                                        <table width="100%">
                                                            <tbody>
                                                                <tr style="width: 100%">
                                                                    <td style="width: 35%; height: 48px">
                                                                        <strong style="white-space: nowrap">Remarks</strong>
                                                                    </td>
                                                                    <td style="width: 65%; height: 48px">
                                                                        <asp:TextBox ID="txtLockRemark" runat="server" __designer:wfdid="w39" CssClass="textBox"
                                                                            Height="38px" TabIndex="2" Width="176px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center" colspan="2">
                                                                        <asp:Button ID="btnDivOK" runat="server" __designer:wfdid="w40" CssClass="btn btnok"
                                                                            OnClick="btnDivOK_click" OnClientClick="return Validation();" Text="OK" wfdid="w31" />
                                                                        &nbsp;<asp:Button ID="btnDivCancel" runat="server" __designer:wfdid="w41" CssClass="btn btncancel"
                                                                            OnClick="btnDivCancel_Click" Text="Cancel" wfdid="w31" />
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                    <div id="divHistoryDtl" class="popUpDivNoTop" style="display: none; left: 230px;
                                                        top: 69px; text-align: left">
                                                        <table style="width: 100%">
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <strong style="white-space: nowrap">Test History Of
                                                                            <asp:Label ID="lblMedexDescription" runat="server" __designer:wfdid="w42"></asp:Label></strong>
                                                                    </td>
                                                                    <td style="text-align: right">
                                                                        <img onclick="DivShowHide('H');" src="images/close.gif" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center" colspan="2" style="height: 168px">
                                                                        <%--nSubjectLabReportNo,nSampleId,vMedexCode,itranNo,Abnormal,ClinicallySig,vGeneralRemark,iModifyBy,dModifyOn,cStatusIndi,vMedExDesc,vUserName--%><asp:GridView
                                                                            ID="GVHistoryDtl" runat="server" __designer:wfdid="w43" AutoGenerateColumns="False"
                                                                            BorderColor="Peru" Font-Size="Small" SkinID="grdViewSmlSize">
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
                                                                                <asp:BoundField DataField="dModifyOn" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Modified On"
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
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                    <div id="divAudit" class="popUpDivNoTop" style="display: none; left: 150px; top: 136px;
                                                        text-align: left">
                                                        <table style="width: 100%">
                                                            <tbody>
                                                                <tr>
                                                                    <td style="height: 20px">
                                                                        <strong style="white-space: nowrap">Final Remarks for SampleID
                                                                            <asp:Label ID="lblSampleID" runat="server" __designer:wfdid="w44"></asp:Label></strong>
                                                                    </td>
                                                                    <td style="text-align: right">
                                                                        <img onclick="DivAuditShowHide('H');" src="images/close.gif" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center" colspan="2" style="height: 168px">
                                                                        <asp:GridView ID="GVAuditFnlRmk" runat="server" __designer:wfdid="w45" AutoGenerateColumns="False"
                                                                            BorderColor="Peru" Font-Size="Small" SkinID="grdViewSmlSize">
                                                                            <Columns>
                                                                                <asp:BoundField DataField="vUserName" HeaderText="Reviewed By">
                                                                                    <ItemStyle Width="20%" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="dReviewedOn" DataFormatString="{0:f}" HeaderText="Reviewed On"
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
                                                    <div id="divTRF" class="popUpDivNoTop" style="display: none; left: 150px; top: 136px">
                                                        <table style="width: 100%">
                                                            <tbody>
                                                                <tr>
                                                                    <td align="left" style="height: 20px">
                                                                        <strong style="white-space: nowrap">TRF of SampleID
                                                                            <asp:Label ID="lblTRF" runat="server" __designer:wfdid="w46" Font-Bold="True"></asp:Label></strong>
                                                                    </td>
                                                                    <td style="text-align: right">
                                                                        &nbsp;<img onclick="DivShowHideTRF('H');" src="images/close.gif" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="height: 20px">
                                                                        <strong style="white-space: nowrap">Sample Collected on:
                                                                            <asp:Label ID="lblSampleCollectedOn" runat="server" __designer:wfdid="w47"></asp:Label></strong>
                                                                    </td>
                                                                    <td>
                                                                        <strong style="white-space: nowrap">Sample Collected by:
                                                                            <asp:Label ID="lblCollectedby" runat="server" __designer:wfdid="w48"></asp:Label></strong>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="height: 20px">
                                                                        <strong style="white-space: nowrap">Sample Received on:
                                                                            <asp:Label ID="lblSampleReceivedOn" runat="server" __designer:wfdid="w49"></asp:Label></strong>
                                                                    </td>
                                                                    <td>
                                                                        <strong style="white-space: nowrap">Sample Received by:
                                                                            <asp:Label ID="lblReceivedby" runat="server" __designer:wfdid="w50"></asp:Label></strong>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center" colspan="2">
                                                                        <asp:Panel ID="pnlMedExGrid" runat="server" __designer:wfdid="w51" Height="140px"
                                                                            ScrollBars="Auto" Width="90%">
                                                                            <asp:GridView ID="gvwTRF" runat="server" __designer:wfdid="w52" AutoGenerateColumns="False"
                                                                                PageSize="25" SkinID="grdViewSmlAutoSize" Width="100%">
                                                                                <Columns>
                                                                                    <asp:BoundField HeaderText="#"></asp:BoundField>
                                                                                    <asp:BoundField DataField="vMedexDesc" HeaderText="TestName"></asp:BoundField>
                                                                                    <asp:BoundField DataField="vTemplateName" HeaderText="TestTemplate/Profile Name">
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="nSampleId" HeaderText="nSampleId"></asp:BoundField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center" colspan="2">
                                                                        <strong style="white-space: nowrap">SampleType Information</strong>
                                                                        <asp:GridView ID="gvwSampletype" runat="server" __designer:wfdid="w53" AutoGenerateColumns="False"
                                                                            PageSize="25" SkinID="grdViewSmlAutoSize" Width="100%">
                                                                            <Columns>
                                                                                <asp:BoundField HeaderText="#"></asp:BoundField>
                                                                                <asp:BoundField DataField="vSampleTypeDesc" HeaderText="Sample Type"></asp:BoundField>
                                                                                <asp:BoundField DataField="fVolume" HeaderText="Volume"></asp:BoundField>
                                                                                <asp:BoundField DataField="nSampleId" HeaderText="nSampleId"></asp:BoundField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    </td> </tr> </tbody> </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="gvwtstChemistry" EventName="RowCommand"></asp:AsyncPostBackTrigger>
                                    <asp:AsyncPostBackTrigger ControlID="gvwtstChemistry" EventName="DataBinding"></asp:AsyncPostBackTrigger>
                                    <asp:AsyncPostBackTrigger ControlID="btnFnlRmkAudit" EventName="Click"></asp:AsyncPostBackTrigger>
                                    <%--<asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click"></asp:AsyncPostBackTrigger>--%>
                                    <%--<asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click"></asp:AsyncPostBackTrigger>--%>
                                    <asp:AsyncPostBackTrigger ControlID="btnDivOK" EventName="Click"></asp:AsyncPostBackTrigger>
                                    <asp:AsyncPostBackTrigger ControlID="rblSampleid" EventName="SelectedIndexChanged">
                                    </asp:AsyncPostBackTrigger>
                                    <asp:AsyncPostBackTrigger ControlID="chkReviewed" EventName="CheckedChanged"></asp:AsyncPostBackTrigger>
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </tbody>
            </table>
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

        function OnSelected1(sender, e) {
            SubjectOnItemSelected(e.get_value(), $get('<%= txtSubject.clientid %>'),
         $get('<%= HSubjectId.clientid %>'), document.getElementById('<%= btnSubject.ClientId %>'));
        }


        function DivShowHide(Type) {
            if (Type == 'S') {
                document.getElementById('divHistoryDtl').style.display = 'block';
                SetCenter('divHistoryDtl');
                ChangedDisabled();
            }
            else if (Type == 'H') {
                document.getElementById('divHistoryDtl').style.display = 'none';
            }
        }

        function DivAuditShowHide(Type) {
            if (Type == 'S') {
                document.getElementById('divAudit').style.display = 'block';
                SetCenter('divAudit');
                ChangedDisabled();
            }
            else if (Type == 'H') {
                document.getElementById('divAudit').style.display = 'none';
            }
        }


        function DivShowHideTRF(Type) {
            if (Type == 'S') {
                document.getElementById('divTRF').style.display = 'block';
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


        function ValidateCheck1() {
            var TargetBaseControl = document.getElementById('<%=gvwtstChemistry.ClientID%>');
            var TargetChildControl = "ChkAbnormal";
            var TargetChildControl1 = "ChkClSignificant";
            var Inputs = TargetBaseControl.getElementsByTagName("input");

            for (var n = 0; n < Inputs.length; ++n) {
                if (Inputs[n + 2].id.indexOf(TargetChildControl, 0) >= 0) {
                    if (Inputs[n + 2].checked) {
                        if (Inputs[n + 3].id.indexOf(TargetChildControl1, 1) >= 0) {
                            Inputs[n + 3].checked = false;
                        }
                    }
                }
            }
        }


        function ValidateCheck2() {
            var TargetBaseControl = document.getElementById('<%=gvwtstChemistry.ClientID%>');
            var TargetChildControl = "ChkAbnormal";
            var TargetChildControl1 = "ChkClSignificant";
            var Inputs = TargetBaseControl.getElementsByTagName("input");

            for (var n = 0; n < Inputs.length; ++n) {
                if (Inputs[n + 3].id.indexOf(TargetChildControl1, 1) >= 0) {
                    if (Inputs[n + 3].checked) {
                        if (Inputs[n + 2].id.indexOf(TargetChildControl, 0) >= 0) {
                            Inputs[n + 2].checked = false;
                        }
                    }
                }
            }
        }

        function ValidateTest() {

            var TargetBaseControl = document.getElementById('<%=gvwtstChemistry.ClientID%>');
            var TargetChildControl = "ChkNormal";

            var Inputs = TargetBaseControl.getElementsByTagName("input");
            var tests = TargetBaseControl.getElementsByTagName("asp:BoundField");


            for (var n = 0; n < Inputs.length; ++n) {
                if (Inputs[n + 1].type == 'checkbox' && Inputs[n + 1].id.indexOf(TargetChildControl, 0) >= 0) {
                    if (Inputs[n + 1].checked == false) {

                        if (Inputs[n + 2].checked == false) {

                            if (Inputs[n + 3].checked == false) {

                                if (Inputs[n + 4].value.trim() == '') {
                                    Inputs[n + 4].parentElement.parentElement.style.backgroundColor = 'red';
                                    var review = confirm('Test shown in Red color is not reviewed.Do you still want to continue?')
                                    {
                                        if (review == true) {
                                            return true;
                                        }
                                        else {
                                            return false;
                                        }
                                    }

                                    return false;
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
                e.style.display = 'block';
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
            document.getElementById('<%=divSearch.clientid%>').style.display = 'none';

            if (chk.checked) {


                document.getElementById('<%=divSearch.clientid%>').style.display = 'block';
                SetCenter('<%=divSearch.clientid%>');
            }

            ChangedDisabled();
        }

        function Validation() {
            if (document.getElementById('<%=txtLockRemark.clientid%>').value.toString().trim() == '') {
                msgalert('Please Enter Remarks');
                document.getElementById('<%= txtLockRemark.ClientId %>').focus();
                document.getElementById('<%= txtLockRemark.ClientId %>').value = '';
                return false;
            }
            return true;
        }


        function closewindow() {
           var parWin = window.opener;
                if (parWin != null && typeof (parWin) != 'undefined')
                {
                    if (parWin && parWin.open && !parWin.closed)
                    {
                        window.parent.document.location.reload();
                    }
                }
                self.close(); 
        }
    
    
       
     
       

    </script>

</asp:Content>
