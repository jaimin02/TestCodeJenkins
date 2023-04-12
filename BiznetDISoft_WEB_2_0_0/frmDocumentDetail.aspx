<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmDocumentDetail.aspx.vb" Inherits="frmDocumentDetail"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <asp:UpdatePanel id="UpdatePanel3" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <contenttemplate>
<DIV style="LEFT: 55px; TOP: 332px" id="Div_Lock" class="DIVSTYLE2" runat="server" visible="false"><TABLE cellPadding=3><TBODY><TR><TD style="WIDTH: 400px"><asp:Button id="BtnLockNode" onclick="BtnLockNode_Click" runat="server" Text="Lock Node" CssClass="btn btnnew" __designer:wfdid="w13"></asp:Button> <asp:Button id="BtnUnLock" onclick="BtnUnLock_Click" runat="server" Text="UnLock" CssClass="btn btnnew" __designer:wfdid="w14"></asp:Button>&nbsp;<asp:Button id="BtnUnlockWS" onclick="BtnUnlockWS_Click" runat="server" Text="UnLock Without Save" CssClass="btn btnnew" Width="155px" __designer:wfdid="w15"></asp:Button> <asp:Button id="BtnClose" onclick="BtnClose_Click" runat="server" Text="Back" CssClass="btn btnclose" Width="66px" __designer:wfdid="w16"></asp:Button></TD></TR><TR><TD style="WIDTH: 400px"><asp:Panel id="Panel1" runat="server" Visible="False" HorizontalAlign="Left" __designer:wfdid="w17"><INPUT style="WIDTH: 306px" id="FlUpload" class="textBox" type=file name="FlUpload" runat="server" /> <INPUT id="BtnUpLoad1" class="btn btnnew" type=button value="Upload" runat="server" OnServerClick="BtnUpLoad1_ServerClick" /> Remark:<asp:TextBox id="txtRemarks" runat="server" Width="244px" __designer:dtid="844424930131995" TextMode="MultiLine" MaxLength="1023" __designer:wfdid="w18"></asp:TextBox></asp:Panel>&nbsp; </TD></TR><TR><TD style="WIDTH: 400px"><STRONG>Created &nbsp;Documents History</STRONG><BR />
<HR style="BACKGROUND-IMAGE: none; WIDTH: 100%; COLOR: #ffa469; BACKGROUND-COLOR: #ffcc66" />
&nbsp;<BR /><asp:UpdatePanel id="Up_History" runat="server" __designer:wfdid="w19"><ContentTemplate>
<asp:GridView id="Gv_DocHistory" runat="server" SkinID="grdViewSml" __designer:wfdid="w20" AutoGenerateColumns="False" OnRowDataBound="Gv_DocHistory_RowDataBound" OnRowCreated="Gv_DocHistory_RowCreated"><Columns>
<asp:TemplateField HeaderText="Sr No"><ItemTemplate>
<asp:Label id="LblDocSrNo" runat="server"></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="vFileType" HeaderText="Doc Type"></asp:BoundField>
<asp:TemplateField HeaderText="File Name"><ItemTemplate>
<asp:HyperLink ID="hlnkDocFile" runat="server" Target="_blank" Text='<%# Eval("vFileName") %>'></asp:HyperLink>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="vUserName" HeaderText="Created By"></asp:BoundField>
<asp:BoundField DataField="iModifyBy" HeaderText="Created by Id"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd-MMM-yyyy}" DataField="dModifyOn" HeaderText="Created On"></asp:BoundField>
<asp:BoundField DataField="vFolderName" HeaderText="FolderName"></asp:BoundField>
<asp:BoundField DataField="vRemark" HeaderText="Remarks"></asp:BoundField>
</Columns>
</asp:GridView> 
</ContentTemplate>
</asp:UpdatePanel></TD></TR></TBODY></TABLE></DIV>
</contenttemplate>
        <triggers>
<asp:AsyncPostBackTrigger ControlID="DDLStatus" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
<asp:PostBackTrigger ControlID="BtnUpload1"></asp:PostBackTrigger>
</triggers>
    </asp:UpdatePanel>
    <table id="tdGeneral" runat="server">
        <tr>
            <td align="left" style="height: 56px">
                <strong>Upload Documents </strong>
                <br />
                <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                &nbsp;</td>
        </tr>
        <tr>
            <td align="left">
                <asp:UpdatePanel id="UP_General" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <contenttemplate>
<TABLE style="WIDTH: 100%" align=center><TBODY><TR><TD align=left colSpan=4 rowSpan=1>Project No :&nbsp; <asp:TextBox id="txtProjNo" runat="server" CssClass="textBox" Width="86px" __designer:wfdid="w27"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp; Activity Name : &nbsp;<asp:TextBox id="txtActivityName" runat="server" CssClass="textBox" Width="246px" __designer:wfdid="w28"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp; Doc Status :&nbsp; <asp:TextBox id="txtDocStatus" runat="server" CssClass="textBox" Width="124px" __designer:wfdid="w29"></asp:TextBox></TD></TR><TR><TD style="PADDING-TOP: 15px" vAlign=bottom noWrap align=left>Doc Link :</TD><TD style="PADDING-TOP: 15px" vAlign=bottom align=left><asp:LinkButton id="LnkDoc" onclick="LnkDoc_Click" runat="server" __designer:wfdid="w30"></asp:LinkButton><asp:Label id="Label1" runat="server" Text="Label" __designer:wfdid="w31"></asp:Label></TD><TD style="PADDING-TOP: 15px" vAlign=bottom align=right>Change Status:</TD><TD style="PADDING-TOP: 20px" vAlign=bottom align=left><asp:DropDownList id="DDLStatus" runat="server" CssClass="dropDownList" Width="216px" __designer:wfdid="w32" OnSelectedIndexChanged="DDLStatus_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList></TD></TR><TR><TD style="WIDTH: 134px" align=left></TD><TD style="WIDTH: 253px" align=left></TD><TD style="WIDTH: 120px" align=left><INPUT id="HdfFolder" type=hidden name="HdfFolder" runat="server" /></TD><TD align=left><INPUT id="HdfFileName" type=hidden name="HdfFileName" runat="server" /> <INPUT id="HdfTranNo" type=hidden name="HdfTranNo" runat="server" /> <INPUT id="HdfBaseFolder" type=hidden name="HdfBaseFolder" runat="server" /></TD></TR></TBODY></TABLE>
</contenttemplate>
                    <triggers>
<asp:AsyncPostBackTrigger ControlID="BtnExit" EventName="Click"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="BtnUnLock" EventName="Click"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="BtnClose" EventName="Click"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="BtnUnlockWS" EventName="Click"></asp:AsyncPostBackTrigger>
</triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="left">
                </td>
        </tr>
        <tr>
            <td align="left">
                <strong>Upload Document for Comment</strong><br />
                <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                &nbsp;</td>
        </tr>
        <tr>
         
            <td align="centre" class="Label">
                <asp:UpdatePanel id="UpdatePanel2" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <contenttemplate>
<INPUT style="WIDTH: 306px" id="FlUploadCom" class="textBox" type=file name="FlUploadCom" runat="server" /><INPUT id="btnUpload2" class="btn btnnew" type=button value="Upload" runat="server" Visible="false" /> 
</contenttemplate>
                    <triggers>
<asp:PostBackTrigger ControlID="btnUpload2"></asp:PostBackTrigger>
<asp:PostBackTrigger ControlID="BtnSave"></asp:PostBackTrigger>
</triggers>
                </asp:UpdatePanel>Comment text:
                <asp:TextBox ID="txtComments" runat="server" MaxLength="1023" TextMode="MultiLine"
                    Width="34%"></asp:TextBox></td>
        </tr>
        <tr>
            <td align="left">
                <strong>
                <table style="width: 895px">
                    <tr>
                        <td align="center">
                            <asp:Button ID="BtnSave" runat="server" CssClass="btn btnsave" Text="Save" />&nbsp;<asp:Button
                                ID="BtnExit" runat="server" CssClass="btn btnexit" Text="Exit" onclientclick="return confirm('Are You sure You want to EXIT?')" /></td>
                    </tr>
                </table>
                    <br />
                    Comment Document History<br />
                    <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                </strong>&nbsp;</td>
        </tr>
        <tr>
            <td align="left">
                <asp:UpdatePanel id="UpdatePanel1" runat="server">
                    <contenttemplate>
<asp:GridView id="GV_Comment" runat="server" SkinID="grdViewSml" __designer:wfdid="w107" AutoGenerateColumns="False" OnRowDataBound="GV_Comment_RowDataBound" OnRowCreated="GV_Comment_RowCreated"><Columns>
<asp:TemplateField HeaderText="Sr No"><ItemTemplate>
<asp:Label id="LblSrNo" runat="server"></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="File Name"><ItemTemplate>
<asp:HyperLink ID="hlnkFile" runat="server" Target="_blank" Text='<%# Eval("vFileName") %>'></asp:HyperLink>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="vUserName" HeaderText="Created By"></asp:BoundField>
<asp:BoundField DataField="vRemark" HeaderText="Comment"></asp:BoundField>
<asp:BoundField DataField="iModifyBy" HeaderText="Created by Id"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd-MMM-yyyy}" DataField="dModifyOn" HeaderText="Created On"></asp:BoundField>
<asp:BoundField DataField="vFolderName" HeaderText="Folder Name"></asp:BoundField>
</Columns>
</asp:GridView> 
</contenttemplate>
                </asp:UpdatePanel></td>
        </tr>
        <tr>
            <td align="left">
                </td>
        </tr>
    </table>
</asp:Content>
