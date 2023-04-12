<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmHL7UploadDoc.aspx.vb" Inherits="frmHL7UploadDoc" Title=" :: Upload HL7 Documnet ::" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table id="divUpload" style="width: 95%; margin: auto; margin-top: 2%; text-align: center;">
                <tr>
                    <td style="width: 40%; text-align: right;">
                        <span class="Label">Document URL * </span>
                    </td>
                    <td style="width: 60%; text-align: left;">
                        <asp:FileUpload ID="fuDocument" runat="server" Style="width: 100%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 40%; text-align: right;"></td>
                    <td style="width: 60%; text-align: left;">
                        <asp:Button ID="btnAttach" Text="Attach" runat="server" CssClass="btn btnnew" />
                        <span style="color: gray; font-size: 11px;">File should be .hl7 Or .Bio</span>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td style="width: 40%; text-align: right;">
                        <span class="Label">Select Project * </span>
                    </td>
                    <td style="width: 60%; text-align: left;">
                        <asp:DropDownList ID="ddlProject" AutoPostBack="true" runat="server" CssClass="dropDownList">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 40%; text-align: right;">
                        <span class="Label" style="margin-right: 1.5%;">Select Activity </span>
                    </td>
                    <td style="width: 60%; text-align: left;">
                        <asp:DropDownList ID="ddlActivty" AutoPostBack="true" runat="server" CssClass="dropDownList">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 40%; text-align: right;">
                        <span class="Label" style="margin-right: 1.5%;">Select Subject </span>
                    </td>
                    <td style="width: 60%; text-align: left;">
                        <asp:DropDownList ID="ddlSubjectList" AutoPostBack="true" runat="server" CssClass="dropDownList"
                            Style="width: 35%;">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 40%; text-align: right;"></td>
                    <td style="width: 60%; text-align: left;">
                        <asp:Button ID="btnGo" runat="server" Text="" CssClass="btn btngo" />
                    </td>
                </tr>
                <tr id="trData" runat="server" style="display: none;">
                    <td style="width: 100%;" align="center" colspan="2">
                        <div style="width: 95% !Important; max-height: 500px; overflow: auto;">
                            <asp:GridView ID="gdvData" SkinID="grdViewSmlAutoSize" AutoGenerateColumns="False"
                                runat="server">
                                <Columns>
                                    <asp:BoundField DataField="vProjectNo" HeaderText="ProjectNo" />
                                    <asp:BoundField DataField="vActivityName" HeaderText="Visit/Timepoint" />
                                    <asp:BoundField DataField="vReferredBy" HeaderText="Investigator" />
                                    <asp:BoundField DataField="vLabID" HeaderText="LabId" />
                                    <asp:BoundField DataField="dDateofCollection" HeaderText="DateOfCollection" />
                                    <asp:BoundField DataField="vSubjectId" HeaderText="SubjectId" />
                                    <asp:BoundField DataField ="vSubjectNo" HeaderText="Subject No" />
                                    <asp:BoundField DataField="cSex" HeaderText="Gender" />
                                    <asp:BoundField DataField="dDateOfBirth" HeaderText="DateOfBirth" />
                                    <asp:BoundField DataField="Age" HeaderText="Age in Year" />
                                    <asp:BoundField DataField="vTestGroup" HeaderText="Panel" />
                                    <asp:BoundField DataField="vTest" HeaderText="Test Name" />
                                    <asp:BoundField DataField="vResult" HeaderText="Result" />
                                    <asp:BoundField DataField="cFlag" HeaderText="Flag" />
                                    <asp:BoundField DataField="vUOM" HeaderText="UOM" />
                                    <asp:BoundField DataField="vLowRange" HeaderText="LowRange" />
                                    <asp:BoundField DataField="vHighRange" HeaderText="HighRange" />
                                    <asp:BoundField DataField="vLowHighUOM" HeaderText="Low-High Range / UOM" />
                                    <asp:BoundField DataField="vUniqueNo" HeaderText="UniqueNo" />
                                    <asp:BoundField DataField="vUniqueNo_OBR" HeaderText="UniqueNo_OBR" />
                                    <asp:BoundField DataField="vResultFlag" HeaderText="ResultFlag" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 40%; text-align: right;"></td>
                    <td style="width: 60%; text-align: left;">
                        <asp:Button ID="btnExport"  runat="server" Style="display: none;"
                            CssClass="btn btnexcel" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAttach" />
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
