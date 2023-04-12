<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmSubjectDetail.aspx.vb" Inherits="frmSubjectDetail"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script src="Script/General.js" language="javascript" type="text/javascript"></script>

    <script src="Script/Validation.js" language="javascript" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function Validation() {
            if (document.getElementById('<%=DDLSubId.ClientID%>').selectedIndex == '') {
                msgalert('Please Select Subject Id !');
                return false;
            }
            return true;
        }

        function QCDivShowHide(Type) {

            //alert(document.getElementById('divQCDtl'));

            if (Type == 'S') {
                document.getElementById('divQCDtl').style.display = 'block';
                //alert('Hi');
                SetCenter('divQCDtl');
                //alert('Hello');
                //alert('Now You Can Add QC Comments.');
                return false;
            }
            else if (Type == 'H') {
                document.getElementById('divQCDtl').style.display = 'none';
                return false;
            }
            else if (Type == 'ST') {
                document.getElementById('divQCDtl').style.display = 'block';
                SetCenter('divQCDtl');
                return true;
            }
            return true;
        }

        //Added by Deepak Singh to show,hide Proof Div  on 5-Apr-10
        function ProofDivShowHide(Type) {
            if (Type == 'S') {
                document.getElementById('div1').style.display = 'block';
                SetCenter('div1');
                return false;
            }
            else if (Type == 'H') {
                document.getElementById('div1').style.display = 'none';
                return false;
            }
            return true;
        }



        function ValidateRemarks(txt, cntField, maxSize) {
            //cntField = document.getElementById(cntField);
            cntField = document.getElementById('<%=lblcnt.ClientID%>');
            if (txt.value.length > maxSize) {
                txt.value = txt.value.substring(0, maxSize);
            }
            // otherwise, update 'characters left' counter
            else {
                cntField.innerHTML = maxSize - txt.value.length;
            }
        }

        function ValidationQC() {

            if (document.getElementById('<%=txtQCRemarks.ClientID%>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Remarks/Response !');
                document.getElementById('<%=txtQCRemarks.ClientID%>').focus();
                return false;
            }
            return true;
        }
                
    </script>

    <%--<asp:UpdatePanel id="UP_Upload" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <contenttemplate>
<DIV style="LEFT: 215px; TOP: 438px" id="divUPLOAD" class="DIVSTYLE2" runat="server" visible="false">
<TABLE><TBODY><TR><TD style="WIDTH: 122px" align=left>Doc :</TD><TD align=left>
<INPUT style="WIDTH: 306px" id="FlUpload" class="textBox" type=file name="FlUpload" runat="server" /></TD><TD><INPUT id="btnUpload" class="button" type=button value="Upload" runat="server" /></TD><TD><INPUT id="BtnCancel" class="button" type=button value="Cancel" runat="server" /></TD></TR></TBODY></TABLE></DIV>
</contenttemplate>
    <triggers>
<asp:AsyncPostBackTrigger ControlID="BtnAttech" EventName="Click"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="btnUpload" EventName="ServerClick"></asp:AsyncPostBackTrigger>
</triggers>
 </asp:UpdatePanel>--%>
    <asp:UpdatePanel ID="UP_General" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table style="width: 80%; margin: auto;">
                <tbody>
                    <b>
                        <tr>
                            <td style="text-align: center; margin: auto;" colspan="2">
                                <b>No. of Subjects :</b>&nbsp;
                                <asp:TextBox ID="txtNoSub" runat="server" CssClass="textBox" Width="5%"></asp:TextBox>&nbsp;&nbsp;&nbsp;<b>Doc
                                    Type :</b>&nbsp;
                                <asp:TextBox ID="TxtDocType" runat="server" CssClass="textBox" Width="20%"></asp:TextBox>&nbsp;&nbsp;&nbsp;<b>Activity
                                    :</b>&nbsp;
                                <asp:TextBox ID="txtActivity" runat="server" CssClass="textBox" Width="20%"></asp:TextBox><br />
                                <hr style="background-image: none; width: 70%; color: #ffcc66; background-color: #ffcc66"
                                    class="hr" />
                                &nbsp;
                            </td>
                        </tr>
                    </b>
                    <tr>
                        <td style="text-align: right; width: 30%;">
                            Subject:
                        </td>
                        <td style="text-align: left">
                            <asp:DropDownList ID="DDLSubId" runat="server" CssClass="dropDownList" Width="60%">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            Doc :
                        </td>
                        <td style="white-space: nowrap; text-align: left;">
                            <input style="width: 40%" id="FlUpload" class="textBox" type="file" name="FlUpload"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;" colspan="2">
                            <asp:Button ID="BtnSave" runat="server" Text="Save" ToolTip="Save" CssClass="btn btnsave">
                            </asp:Button>&nbsp;<asp:Button ID="BtnExit" runat="server" Text="" CssClass="btn btnback"
                                ToolTip="Back"></asp:Button>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnSave" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="left: 50px; top: 315px; height: 54px" id="divChangeStatus" class="DIVSTYLE2"
                runat="server" visible="false">
                <br />
                Status:
                <asp:DropDownList ID="DDLStatus" runat="server" CssClass="dropDownList" Width="213px">
                </asp:DropDownList>
                &nbsp;<asp:Button ID="BtnChangeStatus" OnClick="BtnChangeStatus_Click" runat="server"
                    Text="Change" CssClass="btn btnnew" ></asp:Button>
                <asp:Button ID="btnCloseStatus" OnClick="btnCloseStatus_Click" runat="server" Text="Close"
                    CssClass="btn btnclose" ></asp:Button><br />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvwViewSubjectDetail" EventName="RowCommand">
            </asp:AsyncPostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table style="width: 100%">
                <tbody>
                    <tr>
                        <td>
                            <asp:GridView ID="gvwViewSubjectDetail" runat="server" SkinID="grdView" OnRowCommand="gvwViewSubjectDetail_RowCommand"
                                OnRowCreated="gvwViewSubjectDetail_RowCreated" CellPadding="3" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSrNo" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="vWorkspaceSubjectDocDetailId" HeaderText="Workspace Subject Doc. Detail Id">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vWorkspaceSubjectId" HeaderText="Workspace Subject Id">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vSubjectId" HeaderText=" Subject Id">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FullName" HeaderText="Subject Name">
                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vDocTypeName" HeaderText="Doc. Type">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Doc">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlnkDoc" runat="server" NavigateUrl='<%# Eval("vDocLink") %>'
                                                Target="_blank" Text='<%# Eval("vDocLink") %>'></asp:HyperLink>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Proof">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LnkProof" runat="server" Text="Proof"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="vUsername" HeaderText="Created By">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="dUploadedOn" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Created On"
                                        HtmlEncode="False">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vStageDesc" HeaderText="Current Stage" />
                                    <asp:TemplateField HeaderText="Change Status">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LnkStatus" runat="server">Change Status</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="iNoOfComments" HeaderText="No Of Comments">
                                        <ItemStyle HorizontalAlign="Center" Font-Bold="True" Font-Size="Medium" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="QA">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LnkQC" runat="server" Text="QA"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="vProjectNo" HeaderText="vProjectNo">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                            <asp:HiddenField ID="HfProjectNo" runat="server"></asp:HiddenField>
                        </td>
                    </tr>
                </tbody>
            </table>
            <input id="HFSubject" type="hidden" runat="server" />
            <div style="display: none; left: 391px; width: 800px; top: 367px; height: 336px;
                text-align: left" id="divQCDtl" class="DIVSTYLE2">
                <asp:Panel ID="pnlQCDtl" runat="server" Width="800px" Height="336px" ScrollBars="Auto">
                    <table>
                        <tbody>
                            <tr>
                                <td class="Label" align="left">
                                </td>
                                <td align="left">
                                    <asp:RadioButtonList ID="RBLQCFlag" runat="server" Visible="False" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="Y">Approve</asp:ListItem>
                                        <asp:ListItem Value="N">Reject</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="F">Feedback</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td class="Label" align="left" colspan="2">
                                    <asp:Label ID="lblResponse" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 14%" class="Label" align="left">
                                    Remarks :
                                </td>
                                <td style="width: 80%" align="left" colspan="3">
                                    <textarea style="width: 277px" onkeydown="ValidateRemarks(this,'lblcnt',1000);" id="txtQCRemarks"
                                        class="textBox" runat="server"></textarea>
                                    <asp:Label ID="lblcnt" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 14%" class="Label" align="left">
                                    To :
                                </td>
                                <td style="width: 80%" align="left" colspan="3">
                                    <asp:TextBox ID="txtToEmailId" runat="server" CssClass="textBox" Width="277px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 14%" class="Label" align="left">
                                    CC :
                                </td>
                                <td style="width: 80%" align="left" colspan="3" __designer:dtid="5066566760661055">
                                    <asp:TextBox ID="txtCCEmailId" runat="server" CssClass="textBox" Width="278px" Height="37px"
                                        TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    &nbsp;
                                </td>
                                <td class="Label" align="left" colspan="3">
                                    <%--<input id="BtnQCSave" runat="server" class="button" style="width: 91px" type="button" value="Save & Send" />--%><asp:Button
                                        Style="width: 91px" ID="BtnQCSave" OnClick="BtnQCSave_Click" runat="server" Text="Save"
                                        CssClass="btn btnsave" OnClientClick="return ValidationQC();"></asp:Button>&nbsp;<asp:Button
                                            ID="BtnQCSaveSend" OnClick="BtnQCSaveSend_Click" runat="server"
                                            Text="Save & Send" CssClass="btn btnsave" OnClientClick="return ValidationQC();">
                                    </asp:Button>
                                    <input id="Button1" class="btn btnclose" onclick="QCDivShowHide('H');" type="button" value="Close"
                                        runat="server" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <strong style="text-align: left">
                        <br />
                        QA Comments History </strong>
                    <br />
                    <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                    <br />
                    <asp:GridView ID="GVQCDtl" runat="server" Font-Size="Small" SkinID="grdViewSmlSize"
                        AutoGenerateColumns="False" BorderColor="Peru">
                        <Columns>
                            <asp:BoundField DataField="nWorkSpaceSubjectCommentNo" HeaderText="nWorkSpaceSubjectCommentNo">
                            </asp:BoundField>
                            <asp:BoundField DataField="vWorkSpaceSubjectDocDetailId" HeaderText="vWorkSpaceSubjectDocDetailId">
                            </asp:BoundField>
                            <asp:BoundField DataField="iTranNo" HeaderText="Sr. No."></asp:BoundField>
                            <asp:BoundField DataField="FullName" HeaderText="Subject">
                                <ItemStyle Wrap="false" />
                            </asp:BoundField>
                            <asp:BoundField DataField="vQCComment" HeaderText="QA Comments"></asp:BoundField>
                            <asp:BoundField DataField="cQCFlag" HeaderText="QA"></asp:BoundField>
                            <asp:BoundField DataField="vQCGivenBy" HeaderText="QA BY"></asp:BoundField>
                            <asp:BoundField DataField="dQCGivenOn" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="QA Date"
                                HtmlEncode="False">
                                <ItemStyle Wrap="false" />
                            </asp:BoundField>
                            <asp:BoundField DataField="vResponse" HeaderText="Response"></asp:BoundField>
                            <asp:BoundField DataField="vResponseGivenBy" HeaderText="Response BY">
                                <ItemStyle Wrap="false" />
                            </asp:BoundField>
                            <asp:BoundField DataField="dResponseGivenOn" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Response Date"
                                HtmlEncode="False">
                                <ItemStyle Wrap="false" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Response">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkResponse" runat="server">Response</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Delete">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgDelete" runat="server" ImageUrl="~/Images/i_delete.gif"
                                        OnClientClick="return msgconfirmalert('Are You Sure You Want To Delete The Comment?',this)" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </div>
            <div style="display: none; width: 350px; top: 667px; text-align: center" id="div1"
                class="DIVSTYLE2">
                <table width="100%">
                    <tbody>
                        <tr style="width: 100%; height: 100%">
                            <td style="height: 105%">
                                <asp:GridView ID="GVSubjectProof" runat="server" SkinID="grdViewSmlSize" OnRowCreated="GVSubjectProof_RowCreated"
                                    AutoGenerateColumns="False" OnRowDataBound="GVSubjectProof_RowDataBound" Width="100%">
                                    <Columns>
                                        <asp:BoundField DataField="nSubjectProofNo" HeaderText="SubjectProofNo" />
                                        <asp:BoundField DataField="vSubjectId" HeaderText="SubjectId" />
                                        <asp:BoundField DataField="iTranNo" HeaderText="TranNo" />
                                        <asp:BoundField DataField="vProofType" HeaderText="ProofType" />
                                        <asp:BoundField DataField="vProofPath" HeaderText="ProofPath" />
                                        <asp:TemplateField HeaderText="Attachment">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="hlnkFile" runat="server" Target="_blank" NavigateUrl='<%# Eval("vProofPath") %>'
                                                    Text='<%# Eval("vProofPath") %>'>
                                                </asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <input id="BtnProofClose" class="btn btnclose" onclick="ProofDivShowHide('H');" type="button"
                                    title="Close" value="Close" runat="server" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnChangeStatus" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="BtnQCSave" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="BtnQCSaveSend" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
