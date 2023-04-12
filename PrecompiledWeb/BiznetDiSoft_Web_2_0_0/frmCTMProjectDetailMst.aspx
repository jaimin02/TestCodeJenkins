<%@ page language="VB" autoeventwireup="false" inherits="frmCTMProjectDetailMst, App_Web_eoahe1pj" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="App_Themes/StyleBlue/StyleBlue.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="Script/General.js"></script>

    <script type="text/javascript" src="Script/jquery.min.js"></script>

    <script type="text/javascript" language="javascript" src="Script/popcalendarDiv.js"></script>

    <script type="text/javascript">

    
    
    function ShowColor(gvr)
    {
    
    var lastColor='';
    var lastId='';
        if (lastColor != '')
        {
            document.getElementById(lastId).style.backgroundColor = lastColor;
        }
        lastColor = gvr.style.backgroundColor;
        lastId = gvr.id
        gvr.style.backgroundColor = '#ffdead';
    }
//this is common in this page and frmWorkspaceSubjectMedExInfo.aspx because it is called from frmMedexInfoHdrDtl to refresh page.
function RefreshPage()
{
}


          
function DateConvert_Format(ParamDate,txtdate)
 {      
     
    if (! DateConvert(ParamDate,txtdate))
      {
        return false;
      }
 }
   
  
 

    </script>

</head>
<body topmargin="0" leftmargin="0">
    <form id="form1" runat="server">
    <div align="center">
        <center>
            <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="1000">
            </asp:ScriptManager>
            <table>
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblerrormsg" runat="server" SkinID="lblError"></asp:Label><br />
                        <asp:Label ID="lblProjectSummary" CssClass="Label" runat="server"></asp:Label>
                    </td>
                </tr>
                
            </table>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div style="left: 204px; top: 176px" id="divQC" class="DIVSTYLE2" runat="server"
                        visible="false">
                        <table style="width: 100%;">
                            <tbody>
                                <tr>
                                    <td style="width: 100%; white-space: nowrap;" valign="top" align="left">
                                        <asp:Label ID="LblActivity" runat="server" Font-Bold="True" Font-Size="Larger" ForeColor="Navy"
                                            SkinID="lblHeading"></asp:Label>
                                        <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                                        <br />
                                        <asp:RadioButtonList ID="RBLQC" runat="server" Font-Bold="True" Font-Size="Larger"
                                            OnSelectedIndexChanged="RBLQC_SelectedIndexChanged" RepeatDirection="Horizontal"
                                            AutoPostBack="True" CssClass="RadioButton">
                                            <asp:ListItem Value="DOC">Doc QA</asp:ListItem>
                                            <asp:ListItem Value="INPROC">InProc QA</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:Button ID="BtnCloseDiv" runat="server" CssClass="btn btnclose" OnClick="BtnCloseDiv_Click"
                                            Text="Close" />
                                        <asp:HiddenField ID="HFQC" runat="server" />
                                &nbsp;
                            </tbody>
                        </table>
                    </div>
                    <div style="left: 854px; top: 126px" id="div2" class="DIVSTYLE2" runat="server" visible="false">
                        <table style="width: 100%;">
                            <tbody>
                                <tr>
                                    <td style="width: 100%; white-space: nowrap;" valign="top" align="center">
                                        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Larger" ForeColor="Navy"
                                            SkinID="lblHeading"></asp:Label>
                                        <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                                        <br />
                                        <asp:TextBox ID="txtdob" runat="server" onblur="DateConvert_Format(this.value,this)"
                                            CssClass="textBox" TabIndex="11" Width="140px"></asp:TextBox>
                                        <asp:Label ID="lbldbo" runat="server" Font-Size="Small" TabIndex="53" Text='(e.g."01012009"or "01-Jan-2008")'
                                            CssClass="label"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%; white-space: nowrap;" valign="top" align="center">
                                        <asp:Button ID="BtnSaveDiv" runat="server" CssClass="btn btnsave" OnClick="BtnSaveDiv_Click"
                                            Text="Save" />
                                        <asp:Button ID="Btnclose" runat="server" CssClass="btn btnclose" OnClick="Btnclose_Click"
                                            Text="Close" />
                                        <asp:HiddenField ID="HF" runat="server" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <script src="Script/Gridview.js" type="text/javascript"></script>

            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td align="left" style="height: 23px">
                        <asp:Button ID="BtnBack" runat="server" CssClass="btn btnback" Text="" />
                    </td>
                </tr>
                <tr>
                    <td style="height: 333px">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvwProjectsdetail" runat="server" SkinID="grdVForProjectDetail"
                                    OnRowDataBound="gvwProjectsdetail_RowDataBound" AutoGenerateColumns="False" AllowSorting="True"
                                    CellPadding="3" OnRowCommand="gvwProjectsdetail_RowCommand" Width="920px" OnRowCreated="gvwProjectsdetail_RowCreated">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Activity\Node">
                                            <ItemTemplate>
                                                <asp:Label ID="lblActivity" runat="server" CssClass="Label" Text='<%# Bind("NodeDisName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="False" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="iPeriod" HeaderText="Period"></asp:BoundField>
                                        <asp:BoundField DataField="iNodeId" HeaderText="Node Id"></asp:BoundField>
                                        <asp:BoundField DataField="vDeptName" HeaderText="Dept">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="vAttr1Value" HeaderText="Sch. Start">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="vAttr2Value" HeaderText="Sch. End">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="vAttr6Value" HeaderText="Start Date">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="vAttr4Value" HeaderText="End Date">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="vLocationResourceName" HeaderText="Location">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="vAttr3Value" HeaderText="Status">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="vDocTypeName" HeaderText="Doc Type">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="vAttr99StageDesc" HeaderText="Doc. Stage">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnSubjectMedEx" runat="server" CssClass="LinkButton" Text="Subs Detail"
                                                    CommandName="SubjectMedEx" Enabled='<%# Bind("SubjectMedEx") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="bntQC" runat="server" CssClass="LinkButton" Text="I-QA/R-QA"
                                                    CommandName="QC" Enabled='<%# Bind("QC") %>' Style="white-space: nowrap"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnSlot" runat="server" CssClass="LinkButton" Text="Slot" Enabled='<%# Bind("Slot") %>'
                                                    CommandName="Slot"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnActStart" runat="server" CssClass="LinkButton" Text="Start"
                                                    CommandName="Activity Start" Enabled='<%# Bind("ActivityStarted") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnActComp" runat="server" CssClass="LinkButton" Text="End" CommandName="Activity Complete"
                                                    Enabled='<%# Bind("ActivityCompleted") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnTalk" runat="server" CssClass="LinkButton" Text="Talk" CommandName="Talk"
                                                    Enabled='<%# Bind("Talk") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnDocs" runat="server" CssClass="LinkButton" Text="Docs" CommandName="Docs"
                                                    Enabled='<%# Bind("DocDetails") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnSubject" runat="server" CssClass="LinkButton" Text="Subs Doc"
                                                    CommandName="Subject" Enabled='<%# Bind("SubjectWise") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnRights" runat="server" CssClass="LinkButton" Text="Rights"
                                                    CommandName="Rights" Enabled='<%# Bind("UserRights") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnAuditTrail" runat="server" CssClass="LinkButton" Text="Audit"
                                                    CommandName="Audit" Enabled='<%# Bind("AuditTrail") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnView" runat="server" CssClass="LinkButton" Text="View" CommandName="View"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="vActivityID" HeaderText="Activity Id"></asp:BoundField>
                                        <asp:BoundField DataField="vDocTypeCode" HeaderText="Doctype Id"></asp:BoundField>
                                        <asp:BoundField DataField="cSubjectWiseFlag" HeaderText="SubjectWise" />
                                        <asp:BoundField DataField="vAttr5Value" HeaderText="ResourceCode" />
                                        <asp:BoundField DataField="vLocationCode" HeaderText="vLocationCode" />
                                    </Columns>
                                    <RowStyle BackColor="White" />
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="height: 24px">
                        <asp:Button ID="BtnBack2" runat="server" CssClass="btn btnback" Text="" />
                    </td>
                </tr>
            </table>
        </center>
    </div>
  
    </form>
</body>
</html>
