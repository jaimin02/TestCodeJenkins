<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmDocumentDetail_New.aspx.vb" Inherits="frmDocumentDetail_New"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/popcalendar.js"></script>

    <script type="text/javascript" src="Script/General.js"></script>

    <script type="text/javascript" src="Script/Validation.js"></script>

    <script type="text/javascript" src="Script/AutoComplete.js" language="javascript"></script>

    <script type="text/javascript">

        function DateConvert_Age(ParamDate, txtdate, CheckMore) {



            if (!DateConvert(ParamDate, txtdate)) {
                return false;
            }
            //commented by vishal 
            //            if (CheckMore = 'Y' && !CheckDateMoreThenToday(txtdate.value)) {
            //                txtdate.value = "";
            //                txtdate.focus();
            //                alert('Date should be More than Current Date');
            //                return false;
            //            }
        }
        function DateConvert_Age1(ParamDate, txtdate, CheckMore) {//added by vishal for date validation

            if (!DateConvert(ParamDate, txtdate)) {
                return false;
            }
            //            if (CheckMore = 'Y' && !CheckDateMoreThenToday(txtdate.value)) {
            //                txtdate.value = "";
            //                txtdate.focus();
            //                alert('Date should be More than Current Date');

            //                return false;
            //            }
        }


        function ValidationForSave() {
            if (document.getElementById('<%=FlUploadCom.ClientID%>').value == null) {
                alert("upload a File  Which you Reviewed")
                document.getElementById('<%=FlUploadCom.ClientID%>').focus();

            }


            if ((document.getElementById('<%=TdQC.ClientID%>') == null || document.getElementById('<%=TdQC.ClientID%>') == 'undefined')) {
                if (document.getElementById('<%=DDLStatus.ClientID%>').selectedIndex == 0) {
                    alert("Please Select Status");
                    document.getElementById('<%=DDLStatus.ClientID%>').focus();
                    return false;
                }
            }
            return true;
        }



        function Numeric() {
            var ValidChars = ".0123456789"; // edited by vishal
            var Numeric = true;
            var Char;

            sText = document.getElementById('<%=txtVersionNo.ClientID%>').value;
            for (i = 0; i < sText.length && Numeric == true; i++) {
                Char = sText.charAt(i);
                if (ValidChars.indexOf(Char) == -1) {
                    alert('Please Enter Numeric value');
                    document.getElementById('<%=txtVersionNo.ClientID%>').focus();
                    document.getElementById('<%=txtVersionNo.ClientID%>').value = "";
                    document.getElementById('<%=txtVersionNo.ClientID%>').focus();
                    Numeric = false;
                }
            }

            return Numeric;
        }

    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table id="tdGeneral" runat="server">
                <tbody>
                    <tr>
                        <td align="left">
                            &nbsp;
                            <table style="width: 100%" align="center">
                                <tbody>
                                    <tr>
                                        <td align="left" colspan="5" rowspan="1">
                                            Project No :&nbsp;
                                            <asp:TextBox ID="txtProjNo" runat="server" CssClass="textBox" Width="86px" __designer:wfdid="w91"></asp:TextBox>
                                            &nbsp; &nbsp;&nbsp; Activity Name : &nbsp;<asp:TextBox ID="txtActivityName" runat="server"
                                                CssClass="textBox" Width="246px" __designer:wfdid="w92"></asp:TextBox>
                                            &nbsp; &nbsp;&nbsp; DocStatus :&nbsp;
                                            <asp:TextBox ID="txtDocStatus" runat="server" CssClass="textBox" Width="124px" __designer:wfdid="w93"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="TrReference" runat="server">
                                        <td align="left" colspan="5" rowspan="1">
                                            <strong>
                                                <br />
                                                Reference Documents </strong>&nbsp;
                                            <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                                            <asp:GridView ID="Gv_Reference" runat="server" SkinID="grdViewSml" __designer:wfdid="w94"
                                                OnRowCreated="Gv_Reference_RowCreated" OnRowDataBound="Gv_Reference_RowDataBound"
                                                AutoGenerateColumns="False">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sr No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LblrefSrNo" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Document">
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="hlnkrefFile" runat="server" Target="_blank" Text='<%# Eval("vNodeDisplayName") %>'></asp:HyperLink>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="vUserVersion" HeaderText="Version No" />
                                                    <asp:BoundField DataField="vDocTypeName" HeaderText="Doc.Type" HtmlEncode="False" />
                                                    <asp:BoundField DataField="EffectiveDate" DataFormatString="{0:dd-MMM-yy}" HeaderText="Effective Date"
                                                        HtmlEncode="False" />
                                                    <asp:BoundField DataField="vDocPath" HeaderText="vDocPath" />
                                                    <asp:BoundField DataField="ValidDays" HeaderText="Valid Days" />
                                                    <%--<asp:BoundField DataField="vNodeDisplayName" HeaderText="NodeDisplayName"></asp:BoundField>--%>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="5" rowspan="1">
                                            <strong>
                                                <br />
                                                Upload Documents&nbsp;&nbsp;</strong>
                                            <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-top: 15px" valign="bottom" nowrap align="left" colspan="2">
                                            Current Valid Document
                                        </td>
                                        <td style="width: 120px; padding-top: 15px" valign="bottom" align="right">
                                        </td>
                                        <td style="padding-top: 15px" valign="bottom" align="left" colspan="2" visible="false">
                                            New Document
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-top: 15px" valign="bottom" nowrap align="left">
                                            Doc Link :
                                        </td>
                                        <td style="width: 265px; padding-top: 15px" valign="bottom" align="left">
                                            <asp:LinkButton ID="LnkDocApproved" runat="server" __designer:wfdid="w95"></asp:LinkButton><asp:Label
                                                ID="lblDocApprove" runat="server" __designer:wfdid="w96"></asp:Label>
                                        </td>
                                        <td style="width: 120px; padding-top: 15px" valign="bottom" align="right">
                                        </td>
                                        <td style="padding-top: 15px" valign="bottom" align="left">
                                            Doc Link:
                                        </td>
                                        <td style="padding-top: 20px" valign="bottom" align="left">
                                            &nbsp;<asp:LinkButton ID="LnkDocCreated" OnClick="LnkDoc_Click" runat="server" __designer:wfdid="w97"></asp:LinkButton><asp:Label
                                                ID="lblDocCreated" runat="server" __designer:wfdid="w98"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 134px" align="left">
                                            Version No:
                                        </td>
                                        <td style="width: 265px" align="left">
                                            <asp:Label ID="lblUserVersion" runat="server" __designer:wfdid="w99"></asp:Label>
                                        </td>
                                        <td style="width: 120px" align="left">
                                        </td>
                                        <td style="width: 120px" align="left">
                                            Change Status:
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="DDLStatus" runat="server" CssClass="dropDownList" Width="213px"
                                                __designer:wfdid="w100" AutoPostBack="True" OnSelectedIndexChanged="DDLStatus_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 134px" align="left">
                                            Effective Date:
                                        </td>
                                        <td style="width: 265px" align="left">
                                            <asp:Label ID="lblEffectiveDate" runat="server" __designer:wfdid="w101"></asp:Label>
                                        </td>
                                        <td style="width: 120px" align="left">
                                        </td>
                                        <td style="width: 120px" align="left">
                                        </td>
                                        <td align="left">
                                            <asp:Button ID="BtnBack" OnClick="BtnBack_Click" runat="server" Text="Back" CssClass="button"
                                                __designer:wfdid="w102"></asp:Button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 134px" align="left">
                                        </td>
                                        <td style="width: 265px" align="left">
                                            <asp:Label ID="lblValidDays" runat="server" Visible="False" __designer:wfdid="w103"></asp:Label>
                                        </td>
                                        <td style="width: 120px" align="left">
                                        </td>
                                        <td style="width: 120px" align="left">
                                        </td>
                                        <td align="left">
                                            <input id="HdfFolder" type="hidden" name="HdfFolder" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 134px" align="left">
                                        </td>
                                        <td style="width: 265px" align="left">
                                        </td>
                                        <td style="width: 120px" align="left">
                                        </td>
                                        <td style="width: 120px" align="left">
                                        </td>
                                        <td align="left">
                                            <input id="HdfFileName" type="hidden" name="HdfFileName" runat="server" />
                                            <input id="HdfTranNo" type="hidden" name="HdfTranNo" runat="server" />
                                            <input id="HdfBaseFolder" type="hidden" name="HdfBaseFolder" runat="server" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <%-- <tr>
                                        <td style="width: 100%" align="right">
                                            <strong>Created Documents History</strong><br />
                                            <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                                            &nbsp;&nbsp;<br />
                                            <asp:UpdatePanel ID="Up_History" runat="server" __designer:wfdid="w113">
                                                <ContentTemplate>
                                                    <asp:GridView ID="Gv_DocHistory" runat="server" SkinID="grdViewSml" __designer:wfdid="w114"
                                                        OnRowCreated="Gv_DocHistory_RowCreated" OnRowDataBound="Gv_DocHistory_RowDataBound"
                                                        AutoGenerateColumns="False">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Sr No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LblDocSrNo" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Document">
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="hlnkDocFile" runat="server" Target="_blank" Text='<%# Eval("vFileName") %>'></asp:HyperLink>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="vUserF" HeaderText="Version No"></asp:BoundField>
                                                            <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd-MMM-yy}" DataField="dModiFyOn"
                                                                HeaderText="ModifyOn"></asp:BoundField>
                                                            <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd-MMM-yy}" DataField="EffectiveDate"
                                                                HeaderText="Effective Date"></asp:BoundField>
                                                            <asp:BoundField DataField="vRemark" HeaderText="Remarks"></asp:BoundField>
                                                            <asp:BoundField DataField="vUserName" HeaderText="Created By"></asp:BoundField>
                                                            <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd-MMM-yy}" DataField="dExpiryDate"
                                                                HeaderText="Valid Upto"></asp:BoundField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>--%>
                </tbody>
                <tr>
                    <td style="white-space: nowrap" id="tdCommEntry" align="left" runat="server">
                        <strong>
                            <br />
                            Comment
                            <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                            <input style="width: 306px" id="FlUploadCom" class="textBox" type="file" name="FlUploadCom"
                                runat="server" />
                            <input id="btnUpload2" class="button" type="button" value="Upload" runat="server"
                                visible="false" />&nbsp; </strong>Comment text:&nbsp;
                        <asp:TextBox ID="txtComments" runat="server" CssClass="textBox" Width="34%" __designer:wfdid="w115"
                            TextMode="MultiLine" MaxLength="1023" EnableViewState="false"></asp:TextBox><br />
                        <br />
                        <%--<FTB:FreeTextBox ID="FtbDocsReader" runat="Server" />--%>
                        <asp:RadioButtonList ID="RBLQCAuth" runat="server" __designer:wfdid="w116" RepeatDirection="Horizontal">
                            <asp:ListItem Value="Y">Quality Approved</asp:ListItem>
                            <asp:ListItem Value="N">Quality Rejected</asp:ListItem>
                            <asp:ListItem Selected="True" Value="F">Quality Feedback Given</asp:ListItem>
                        </asp:RadioButtonList>
                        <br />
                        <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="button" __designer:wfdid="w117"
                            OnClientClick="return ValidationForSave();"></asp:Button>
                        <asp:Button ID="BtnExit" runat="server" Text="Back" CssClass="button" __designer:wfdid="w118">
                        </asp:Button>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        &nbsp;
                    </td>
                </tr>
            </table>
            <%--added by vishal for Proposed and effective--%>
            <table align="left" width="100%">
                <table width="50%" align="left">
                    <tr id="TrEffectiveDocumemt" runat="server">
                        <td style="width: 50px" align="left">
                        </td>
                        <td id="TD_Effective" align="left" style="width: 225px">
                            <asp:Panel runat="server" ID="PnlEffectiveDoc" Width="350" BorderStyle="Solid" BorderWidth="1">
                                <img src="images/collapse.jpg" id="ImgColl1" />
                                <strong>Effective Document </strong>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="PnlGrdviewEffective">
                                <asp:GridView ID="GV_EffectiveDocs" HeaderStyle-Width="50px" runat="server" AutoGenerateColumns="false"
                                    SkinID="grdViewDocs" Width="350px">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No">
                                            <ItemStyle HorizontalAlign="Right" />
                                            <ItemTemplate>
                                                <asp:Label ID="LblGv_EffectiveDocumentSrNo" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Document">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:HyperLink ID="hlnk_EffectiveDocument" runat="server" Target="_blank" Text='<%# Eval("vFileName") %>'></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="vUserVersion" HeaderText="Version No" />
                                        <asp:BoundField DataField="EffectiveDate" DataFormatString="{0:dd-MMM-yy}" HeaderText="Effective Date"
                                            HtmlEncode="False" />
                                        <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd-MMM-yy}" DataField="dExpiryDate"
                                            HeaderText="Valid Upto"></asp:BoundField>
                                        <asp:BoundField DataField="vFolderName" HeaderText="vFolderName" />
                                        <asp:BoundField DataField="iStageId" />
                                        <asp:BoundField HeaderText="Status" />
                                        <asp:BoundField DataField="dModiFyOn" DataFormatString="{0:dd-MMM-yy}" HeaderText="ModifyOn" />
                                        <%-- <asp:TemplateField HeaderText="Details">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBtn" runat="server" Text="Details"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                            <cc1:CollapsiblePanelExtender ID="CPEEffective" runat="server" TargetControlID="PnlGrdviewEffective"
                                CollapseControlID="PnlEffectiveDoc" ExpandControlID="PnlEffectiveDoc" Collapsed="true"
                                ExpandedImage="~/images/collapse.jpg" CollapsedImage="~/images/expand.jpg" ImageControlID="ImgColl1">
                            </cc1:CollapsiblePanelExtender>
                            <%--<hr style="background-image: none; color: #ffcc66; margin-right: auto; max-width: 450px;
                                background-color: #ffcc66">--%>
                        </td>
                    </tr>
                </table>
                <table align="left" width="50%">
                    <tr id="Tr1" runat="server">
                        <td align="left" style="width: 225px">
                            <asp:Panel runat="server" ID="PnlProposedDocs" Width="450" BorderStyle="Solid" BorderWidth="1">
                                <img src="images/collapse.jpg" id="Img1" />
                                <strong>Proposed Document</strong></asp:Panel>
                            <asp:Panel runat="server" ID="PnlGvProposed">
                                <asp:Label ID="lblProposedDocs" runat="server" Text="There is no any data Related to Proposed Doc"
                                    Visible="false"></asp:Label>
                                <asp:Panel ID="PnlProposed" runat="server" Width="449px" ScrollBars="auto">
                                    <asp:GridView ID="Gv_ProposedDocument" HeaderStyle-Width="50px" runat="server" __designer:wfdid="w94"
                                        AutoGenerateColumns="false" SkinID="grdViewDocs" Width="350px">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr No">
                                                <ItemStyle HorizontalAlign="Right" />
                                                <ItemTemplate>
                                                    <asp:Label ID="LblGv_ProposedDocumentSrNo" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Document">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="hlnk_ProposedDocument" runat="server" Target="_blank" Text='<%# Eval("vFileName") %>'></asp:HyperLink>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="vUserVersion" HeaderText="Version No" />
                                            <asp:BoundField DataField="EffectiveDate" DataFormatString="{0:dd-MMM-yy}" HeaderText="Effective Date"
                                                HtmlEncode="False" />
                                            <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd-MMM-yy}" DataField="dExpiryDate"
                                                HeaderText="Valid Upto"></asp:BoundField>
                                            <asp:BoundField DataField="vFolderName" HeaderText="vFolderName" />
                                            <asp:BoundField DataField="iStageId" />
                                            <asp:BoundField HeaderText="Status" />
                                            <asp:BoundField DataField="dModiFyOn" DataFormatString="{0:dd-MMM-yy}" HeaderText="ModifyOn" />
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </asp:Panel>
                            <cc1:CollapsiblePanelExtender ID="CPEProposed" runat="server" TargetControlID="PnlGvProposed"
                                CollapseControlID="PnlProposedDocs" ExpandControlID="PnlProposedDocs" Collapsed="true"
                                ExpandedImage="~/images/collapse.jpg" CollapsedImage="~/images/expand.jpg" ImageControlID="Img1">
                            </cc1:CollapsiblePanelExtender>
                        </td>
                    </tr>
                </table>
            </table>
            <table align="center">
                <tbody>
                    <tr>
                        <td id="tdComment" align="Center" runat="server">
                            <asp:Panel runat="server" ID="PnlCommentHistory" Width="450" BorderStyle="Solid"
                                BorderWidth="1">
                                <img src="images/collapse.jpg" id="Img4" />
                                <strong class="Label">
                                    <br />
                                    Comment Document History</strong></asp:Panel>
                            <asp:Panel runat="server" ID="PnlGV_CommentHistory">
                                <asp:GridView ID="GV_Comment" runat="server" SkinID="grdViewSml" __designer:wfdid="w119"
                                    OnRowCreated="GV_Comment_RowCreated" OnRowDataBound="GV_Comment_RowDataBound"
                                    AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No">
                                            <ItemStyle HorizontalAlign="Right" />
                                            <ItemTemplate>
                                                <asp:Label ID="LblSrNo" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Document">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:HyperLink ID="hlnkFile" runat="server" Target="_blank" Text='<%# Eval("vFileName") %>'></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="vUserVersion" HeaderText="Version No" />
                                        <asp:BoundField DataField="dModiFyOn" DataFormatString="{0:dd-MMM-yy}" HeaderText="ModifyOn"
                                            HtmlEncode="False" />
                                        <asp:BoundField DataField="EffectiveDate" DataFormatString="{0:dd-MMM-yy}" HeaderText="Effective Date"
                                            HtmlEncode="False" />
                                        <asp:BoundField DataField="vUserName" HeaderText="Created By" />
                                        <asp:BoundField DataField="vRemark" HeaderText="Comment" />
                                        <asp:BoundField DataField="vFolderName" HeaderText="vFolderName" />
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                            <cc1:CollapsiblePanelExtender ID="CPECommentHistory" runat="server" TargetControlID="PnlGV_CommentHistory"
                                CollapseControlID="PnlCommentHistory" ExpandControlID="PnlCommentHistory" Collapsed="true"
                                ExpandedImage="~/images/collapse.jpg" CollapsedImage="~/images/expand.jpg" ImageControlID="Img4">
                            </cc1:CollapsiblePanelExtender>
                            <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                        </td>
                    </tr>
            </table>
            <table>
                <tr>
                    <td id="TdQC" align="left" runat="server">
                        <strong>
                            <br />
                            QC Comment Document History</strong>
                        <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                        <asp:GridView ID="GV_QCComment" runat="server" SkinID="grdViewSml" __designer:wfdid="w120"
                            OnRowCreated="GV_QCComment_RowCreated" OnRowDataBound="GV_QCComment_RowDataBound"
                            AutoGenerateColumns="False">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr No">
                                    <ItemTemplate>
                                        <asp:Label ID="LblQCSrNo" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Document">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlnkQCFile" runat="server" Target="_blank" Text='<%# Eval("vFileName") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="vUserVersion" HeaderText="Version No" />
                                <asp:BoundField DataField="dModifyOn" DataFormatString="{0:dd-MMM-yy}" HeaderText="ModiFyOn"
                                    HtmlEncode="False" />
                                <asp:BoundField DataField="EffectiveDate" DataFormatString="{0:dd-MMM-yy}" HeaderText="Effective Date"
                                    HtmlEncode="False" />
                                <asp:BoundField DataField="vUserName" HeaderText="QC Done by" />
                                <asp:BoundField DataField="vRemark" HeaderText="Comment" />
                                <asp:BoundField DataField="vFolderName" HeaderText="vFolderName" />
                                <asp:BoundField DataField="cQCAuthorization" HeaderText="QC" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnUpLoad1"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="BtnUnLock"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="BtnSave"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="BtnLockNode"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="DDLStatus"></asp:PostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
    <%--FOR POPUP OF RELEASE--%>
    <%--added by vishal--%>
    <button id="Btn3" runat="server" style="display: none;" />
    <cc1:ModalPopupExtender ID="MdlCreate" runat="server" PopupControlID="DivPopUpCreateDocs"
        PopupDragHandleControlID="LblPopUpTitleWorkSummary" BackgroundCssClass="modalBackground"
        TargetControlID="Btn3" CancelControlID="ImgPopUpCloseWorkSummary">
    </cc1:ModalPopupExtender>
    <div id="DivPopUpCreateDocs" runat="server" style="position: relative; display: none;
        background-color: #cee3ed; padding: 5px; width: 600px; height: inherit; border: dotted 1px gray;">
        <div>
            <div>
                <img id="ImgPopUpCloseWorkSummary" alt="Close" src="images/Sqclose.gif" style="position: relative;
                    float: right; right: 5px;" />
                <asp:Label ID="LblPopUpTitleWorkSummary" runat="server" class="Label" Visible="true"
                    Text="Create Document"></asp:Label>
            </div>
        </div>
        <table style="width: 500px;">
            <tbody>
                <tr>
                    <td class="Label" valign="top" align="center">
                        <asp:Button ID="BtnLockNode" OnClick="BtnLockNode_Click" runat="server" Text="Lock Node"
                            CssClass="button"></asp:Button>
                        <asp:Button ID="BtnUnLock" Height="20px" OnClick="BtnUnLock_Click" runat="server"
                            Text="UnLock" CssClass="button"></asp:Button>&nbsp;
                        <asp:Button ID="BtnUnlockWS" Height="20px" OnClick="BtnUnlockWS_Click" runat="server"
                            Text="UnLock Without Save" Width="166px" CssClass="button"></asp:Button>
                        <asp:Button ID="BtnClose" Height="20px" OnClick="BtnClose_Click" runat="server" Text="Cancel"
                            CssClass="button" Width="66px"></asp:Button>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblUploadedLink" runat="server"></asp:Label><br />
                        <table style="width: 100%">
                            <tr>
                                <td align="right" style="width: 30%" class="Label">
                                    File Path:
                                </td>
                                <td align="left">
                                    <input style="width: 306px" id="FlUpload" class="textBox" type="file" name="FlUpload"
                                        runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="Label">
                                    Version No:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtVersionNo" runat="server" MaxLength="4" CssClass="textBox" __designer:wfdid="w109"
                                        ReadOnly="true" BackColor="AntiqueWhite" ForeColor="Gray" onblur="Numeric()"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%" align="right" class="Label">
                                    Effective Date:
                                </td>
                                <td align="left">
                                    <asp:TextBox onblur="DateConvert_Age(this.value,this,'Y')" ID="txtdoer" runat="server"
                                        CssClass="textBox" Width="140px" __designer:wfdid="w110" Height="14px"></asp:TextBox>(ddmmyyyy
                                    / dd-MMM-yyyy)
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%" align="right" class="Label">
                                    Valid Upto:
                                </td>
                                <td align="left">
                                    <asp:TextBox onblur="DateConvert_Age1(this.value,this,'Y')" ID="TextValidDate" runat="server"
                                        CssClass="textBox" Width="140px" __designer:wfdid="w110" Height="14px"></asp:TextBox>(ddmmyyyy
                                    / dd-MMM-yyyy)
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%" align="right">
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtValidDays" onblur="return Numeric();" runat="server" CssClass="textBox"
                                        Visible="false" Width="62px" __designer:wfdid="w111" Height="14px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%" align="right" class="Label">
                                    Remark:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="textBox" Width="244px" __designer:wfdid="w112"
                                        TextMode="MultiLine" MaxLength="1023"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td class="Label" valign="top" align="center">
                                    <input id="BtnUpLoad1" class="button" type="button" value="Upload" runat="server"
                                        onserverclick="BtnUpLoad1_ServerClick" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <%--FOR POPUP OF Audit Trail For Effective--%>
    <%--added by vishal--%>
    <button id="btnEffectiveAt" runat="server" style="display: none;" />
    <cc1:ModalPopupExtender ID="MdlEffectiveAt" runat="server" PopupControlID="DivPopUpEffectiveAtDocs"
        PopupDragHandleControlID="LblPopUpEffectiveAt" BackgroundCssClass="modalBackground"
        TargetControlID="btnEffectiveAt" CancelControlID="ImgPopUpClose">
    </cc1:ModalPopupExtender>
    <div id="DivPopUpEffectiveAtDocs" runat="server" style="position: relative; display: none;
        background-color: #cee3ed; padding: 5px; width: 600px; height: inherit; border: dotted 1px gray;">
        <div>
            <div>
                <img id="ImgPopUpClose" alt="Close" src="images/Sqclose.gif" style="position: relative;
                    float: right; right: 5px;" />
                <asp:Label ID="LblPopUpEffectiveAt" runat="server" class="LabelBold" Visible="true"
                    Text=" Effective Document Details"></asp:Label>
            </div>
        </div>
        <table>
            <tr>
                <td>
                    <asp:Panel ID="P_ATbefore" runat="server" Height="500px" Width="449px" ScrollBars="auto">
                        <asp:GridView ID="Gv_ATbeforePublished" runat="server" AutoGenerateColumns="False"
                            SkinID="grdViewDocs" Width="450px">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr No">
                                    <ItemTemplate>
                                        <asp:Label ID="LblATbeforeSrNo" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Document">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlnkATbefore" runat="server" Text='<%# Eval("vFileName") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="vUserVersion" HeaderText="Version No" />
                                <asp:BoundField DataField="dModiFyOn" DataFormatString="{0:dd-MMM-yy}" HeaderText="ModifyOn" />
                                <asp:BoundField DataField="EffectiveDate" DataFormatString="{0:dd-MMM-yy}" HeaderText="Effective Date"
                                    HtmlEncode="False" />
                                <asp:BoundField DataFormatString="{0:dd-MMM-yy}" DataField="dExpiryDate" HeaderText="Valid Upto">
                                </asp:BoundField>
                                <asp:BoundField DataField="vFolderName" HeaderText="vFolderName" />
                                <asp:BoundField DataField="iStageId" />
                                <asp:BoundField HeaderText="Status" />
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
