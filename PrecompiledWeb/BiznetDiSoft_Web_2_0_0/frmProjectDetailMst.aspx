<%@ page language="VB" autoeventwireup="false" inherits="frmProjectDetailMst, App_Web_eoahe1pj" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

    .Center{
        text-align: center;
       }
        
    </style>

    <link rel="shortcut icon" type="image/x-icon" href="images/biznet.ico" />
    <link href="App_Themes/StyleBlue/StyleBlue.css" rel="stylesheet" type="text/css" />

   
    <link href="App_Themes/StyleCommon/CommonStyle.css" rel="stylesheet" />
    <script type="text/javascript" src="Script/General.js"></script>

    <script type="text/javascript" src="Script/jquery-1.4.3.min.js"></script>

    <script src="Script/Gridview.js" type="text/javascript"></script>

    <script src="Script/Validation.js" type="text/javascript"></script>

 

     <script src="Script/jquery.dataTables.min.js" type="text/javascript"></script>

    <script src="Script/jquery.dataTables.plugins.js" type="text/javascript"></script>

    <script type="text/javascript">
       
        function ShowColor(gvr) {
            var lastColor = '';
            var lastId = '';
            if (lastColor != '') {
                document.getElementById(lastId).style.backgroundColor = lastColor;
            }
            lastColor = gvr.style.backgroundColor;
            lastId = gvr.id
            gvr.style.backgroundColor = '#ffdead';
        }
        //this is common in this page and frmWorkspaceSubjectMedExInfo.aspx because it is called from frmMedexInfoHdrDtl to refresh page.
        function RefreshPage() {
            window.location.href = window.location.href;
        }
        function disableMenuBarAndToolBar(str) {
            window.open(str);

        }

        function closechildwindow(str) {
            //            var conf = confirm('Are You Sure You Want To Exit?');

            //            if (conf)
            //        {
            var parWin = window.opener;
            if (parWin != null && typeof (parWin) != 'undefined') {
                self.close();
            }
            //         }
        }
        function ValidationBeforeSave() {
            var dt = document.getElementById('txtdob').value;
            if (dt == '') {
                msgalert('Please Enter Date !');
                return false;
            }
            showProgress = false;
            return true;
        }

        function DateValidation(ParamDate, txtdate) {
            txtdate.style.borderColor = "";
            if (ParamDate.trim() != '') {
                var flg = false;
                flg = DateConvert(ParamDate, txtdate);
                if (flg == true && !CheckDateLessThenToday(txtdate.value)) {
                    txtdate.value = "";
                    txtdate.focus();
                    msgalert('Date Should Be Less Than Current Date !');
                    return false;
                }
                else if (flg == false) {
                    txtdate.value = "";
                    txtdate.focus();
                    msgalert('Date Should Be Proper Format !');
                    return false;
                }
            }

        }

        function ShowConfirmation() {
            msgConfirmDeleteAlert(null, "Only Clinical Phase Is Completed. Do You Want To Skip Analysis Phase And Start Document Phase?", function (isConfirmed) {
                if (isConfirmed) {
                    return true;
                } else {
                    document.getElementById('Btnclose').click();
                    return false;
                }
            });
            return false
        }
    </script>

    <style type="text/css">
        </style>
</head>
<body style="margin-left: 0; margin-top: 0;">
    <form id="form1" runat="server">        
        <div align="center" style="width: 100%">
            <center>
                <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="1000">
                </asp:ScriptManager>
                <table style="width:80%">
                    <tr>
                        <td style="width: 100%; text-align: right;">
                            <asp:Label runat="server" ID="lblSessionTimeCap" class="Label" Style="color: blueviolet; text-align: right;"
                                Text="Session Expires:"></asp:Label>
                            <b><span class="Label" style="color: blueviolet" id="timerText"></span></b>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%">
                            <asp:Label ID="lblerrormsg" runat="server" SkinID="lblError"></asp:Label><br />
                            <asp:Label ID="lblProjectSummary" CssClass="Label" runat="server"></asp:Label>

                        </td>
                    </tr>
                </table>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnDivQc" runat="server" Style="display: none;" />
                        <cc1:ModalPopupExtender ID="mdlDivQc" runat="server" PopupControlID="divQC" BackgroundCssClass="modalBackground"
                            BehaviorID="mdlDivQc" CancelControlID="BtnCloseDiv" TargetControlID="btnDivQc">
                        </cc1:ModalPopupExtender>
                        <div style="display: none;" id="divQC" class="centerModalPopup" runat="server">
                            <table style="width: 90%; margin: auto;">
                                <tbody>
                                    <tr>
                                        <td style="width: 100%; white-space: nowrap; vertical-align: top; text-align: center;">
                                            <asp:GridView ID="gvwProjectsdetail0" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                CellPadding="3" OnRowCommand="gvwProjectsdetail_RowCommand" OnRowCreated="gvwProjectsdetail_RowCreated"
                                                OnRowDataBound="gvwProjectsdetail_RowDataBound" SkinID="grdViewSmlAutoSize" Width="100%">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Activity\Node">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblActivity0" runat="server" CssClass="Label" Text='<%# Bind("NodeDisName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Wrap="False" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="iPeriod" HeaderText="Period" />
                                                    <asp:BoundField DataField="iNodeId" HeaderText="Node Id" />
                                                    <asp:BoundField DataField="vDeptName" HeaderText="Dept">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vAttr1Value" HeaderText="Sch. Start" DataFormatString="{0:dd-MMM-yyyy}">
                                                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vAttr2Value" HeaderText="Sch. End" DataFormatString="{0:dd-MMM-yyyy}">
                                                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vAttr6Value" HeaderText="Start Date" DataFormatString="{0:dd-MMM-yyyy}">
                                                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vAttr4Value" HeaderText="End Date" DataFormatString="{0:dd-MMM-yyyy}">
                                                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vLocationResourceName" HeaderText="Location">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vAttr3Value" HeaderText="Status">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vDocTypeName" HeaderText="Doc Type">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vAttr99StageDesc" HeaderText="Doc. Stage">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnSubjectMedEx0" runat="server" CommandName="SubjectMedEx" ToolTip="Subs Details"
                                                                Enabled='<%# Bind("SubjectMedEx") %>' Text="Subs Detail"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="bntQC0" runat="server" CommandName="QC" Enabled='<%# Bind("QC") %>'
                                                                Style="white-space: nowrap" Text="I-QA/R-QA" ToolTip="I-QA/R-QA"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnSlot0" runat="server" CommandName="Slot" ToolTip="Slot" Enabled='<%# Bind("Slot") %>'
                                                                Text="Slot"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnActStart0" runat="server" CommandName="Activity Start" ToolTip="Start"
                                                                Enabled='<%# Bind("ActivityStarted") %>' Text="Start"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnActComp0" runat="server" CommandName="Activity Complete" Enabled='<%# Bind("ActivityCompleted") %>'
                                                                Text="End" ToolTip="End"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnTalk0" runat="server" CommandName="Talk" Enabled='<%# Bind("Talk") %>'
                                                                Text="Talk" ToolTip="Talk"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnDocs0" runat="server" CommandName="Docs" Enabled='<%# Bind("DocDetails") %>'
                                                                Text="Docs"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnSubject0" runat="server" CommandName="Subject" Enabled='<%# Bind("SubjectWise") %>'
                                                                Text="Subs Doc" ToolTip="Sub Docs"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnRights0" runat="server" CommandName="Rights" Enabled='<%# Bind("UserRights") %>'
                                                                Text="Rights" ToolTip="Rights"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnAuditTrail0" runat="server" CommandName="Audit" Enabled='<%# Bind("AuditTrail") %>'
                                                                Text="Audit" ToolTip="Audit"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnView0" runat="server" CommandName="View" Text="View" ToolTip="View"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="vActivityID" HeaderText="Activity Id" />
                                                    <asp:BoundField DataField="vDocTypeCode" HeaderText="Doctype Id" />
                                                    <asp:BoundField DataField="cSubjectWiseFlag" HeaderText="SubjectWise" />
                                                    <asp:BoundField DataField="vAttr5Value" HeaderText="ResourceCode" />
                                                    <asp:BoundField DataField="vLocationCode" HeaderText="vLocationCode" />
                                                </Columns>
                                                <RowStyle BackColor="White" />
                                            </asp:GridView>
                                            <asp:Label ID="LblActivity" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="Navy"
                                                SkinID="lblHeading"></asp:Label>
                                            <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                                            <asp:RadioButtonList ID="RBLQC" runat="server" Font-Bold="True" Font-Size="Small"
                                                OnSelectedIndexChanged="RBLQC_SelectedIndexChanged" RepeatDirection="Horizontal"
                                                AutoPostBack="True" CssClass="RadioButton">
                                                <asp:ListItem Value="DOC">Doc QA</asp:ListItem>
                                                <asp:ListItem Value="INPROC">InProc QA</asp:ListItem>
                                                <asp:ListItem Value="SOURCE">Source Review</asp:ListItem>
                                            </asp:RadioButtonList>
                                            <br />
                                            <asp:Button ID="BtnCloseDiv" runat="server" CssClass="btn btnclose" OnClick="BtnCloseDiv_Click"
                                                Text="Close" />
                                            <asp:HiddenField ID="HFQC" runat="server" />
                                    &nbsp;
                                </tbody>
                            </table>
                        </div>
                        <asp:Button ID="btnSetDate" runat="server" Style="display: none;" />
                        <cc1:ModalPopupExtender ID="mdlSetDate" runat="server" PopupControlID="div2" BackgroundCssClass="modalBackground"
                            BehaviorID="mdlSetDate" CancelControlID="Btnclose" TargetControlID="btnSetDate">
                        </cc1:ModalPopupExtender>
                        <div style="display: none" id="div2" class="centerModalPopup" runat="server">
                            <table style="width: 90%; margin: auto;">
                                <tbody>
                                    <tr>
                                        <td style="width: 100%; white-space: nowrap;" valign="top" align="center">
                                            <asp:Label ID="LblDiv2Heading" runat="server" Font-Bold="True" Font-Size="Larger"
                                                ForeColor="Navy" SkinID="lblHeading"></asp:Label>
                                            <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                                            <br />
                                            <asp:Label runat="server" ID="LblDate" ForeColor="Navy" Text="" Font-Size="Small"></asp:Label>
                                            <asp:TextBox ID="txtdob" runat="server" CssClass="textBox" TabIndex="11" Width="140px"
                                                onblur="return DateValidation(this.value,this);"></asp:TextBox>
                                            <asp:Label ID="LblEnterDateInFormat" Text="Date Fromat : DDMMYYYY or dd-Mon-YYYY"
                                                ForeColor="Red" Visible="true" runat="server" Font-Size="Small"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; white-space: nowrap;" valign="top" align="center">
                                            <asp:Button ID="BtnSaveDiv" runat="server" CssClass="btn btnsave" OnClientClick="return ValidationBeforeSave();"
                                                Text="Save" OnClick="BtnSaveDiv_Click" />
                                            <asp:Button ID="Btnclose" runat="server" CssClass="btn btnclose" Text="Close" />
                                            <asp:HiddenField ID="HF" runat="server" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <table cellpadding="0" cellspacing="0" style="width: 90%; margin: auto;">
                    <tr>
                        <td style="text-align: center;padding:1%">
                            <asp:Button ID="BtnBack" runat="server" CssClass="btn btnclose" Text="Back" ToolTip="Back" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="width: 1139px; margin: auto; overflow: auto">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvwProjectsdetail" runat="server" SkinID="grdViewSmlAutoSizeNofooter" OnRowDataBound="gvwProjectsdetail_RowDataBound"
                                            AutoGenerateColumns="False" AllowSorting="True" CellPadding="3" OnRowCommand="gvwProjectsdetail_RowCommand"
                                            Style="width: 100%" OnRowCreated="gvwProjectsdetail_RowCreated">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Activity\Node">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblActivity" runat="server" CssClass="Label" Text='<%# Bind("NodeDisName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Wrap="False" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="iPeriod" HeaderText="Period" />
                                                <asp:BoundField DataField="iNodeId" HeaderText="Node Id" />
                                                <asp:BoundField DataField="vDeptName" HeaderText="Dept">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vAttr1Value" HeaderText="Sch. Start" DataFormatString="{0:dd-MMM-yyyy}">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vAttr2Value" HeaderText="Sch. End" DataFormatString="{0:dd-MMM-yyyy}">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vAttr6Value" HeaderText="Start Date" DataFormatString="{0:dd-MMM-yyyy}">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vAttr4Value" HeaderText="End Date" DataFormatString="{0:dd-MMM-yyyy}">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vLocationResourceName" HeaderText="Location">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vAttr3Value" HeaderText="Status">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vDocTypeName" HeaderText="Doc Type">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vAttr99StageDesc" HeaderText="Doc. Stage">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnTalk" runat="server" Text="Talk" CommandName="Talk" Enabled='<%# Bind("Talk") %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnSubjectMedEx" runat="server" Text="Subs Detail" CommandName="SubjectMedEx"
                                                            Enabled='<%# Bind("SubjectMedEx") %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="bntQC" runat="server" Text="I-QA/R-QA" CommandName="QC" Enabled='<%# Bind("QC") %>'
                                                            Style="white-space: nowrap"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnSlot" runat="server" Text="Slot" Enabled='<%# Bind("Slot") %>'
                                                            CommandName="Slot"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnActStart" runat="server" Text="Start" CommandName="Activity Start"
                                                            Enabled='<%# Bind("ActivityStarted") %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnActComp" runat="server" Text="End" CommandName="Activity Complete"
                                                            Enabled='<%# Bind("ActivityCompleted") %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnView" runat="server" Text="View" CommandName="View"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnDocs" runat="server" Text="Docs" CommandName="Docs" Enabled='<%# Bind("DocDetails") %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnSubject" runat="server" Text="Subs Doc" CommandName="Subject"
                                                            Enabled='<%# Bind("SubjectWise") %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnRights" runat="server" Text="Rights" CommandName="Rights"
                                                            Enabled='<%# Bind("UserRights") %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnAuditTrail" runat="server" Text="Audit" CommandName="Audit"
                                                            Enabled='<%# Bind("AuditTrail") %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="vActivityID" HeaderText="Activity Id" />
                                                <asp:BoundField DataField="vDocTypeCode" HeaderText="Doctype Id" />
                                                <asp:BoundField DataField="cSubjectWiseFlag" HeaderText="SubjectWise" />
                                                <asp:BoundField DataField="vAttr5Value" HeaderText="ResourceCode" />
                                                <asp:BoundField DataField="vLocationCode" HeaderText="vLocationCode" />

                                                
                                                <asp:TemplateField HeaderText="Actual Start ModifiedBy & On" Visible="false" >
                                                    <ItemTemplate >
                                                        <%#Eval("vActivityStartedby")%> <br /> <%#Eval("dActivityStartedOn")%> 
                                                    </ItemTemplate>
                                                      <ItemStyle CssClass="Center"/>
                                                </asp:TemplateField>

                                                 <asp:TemplateField HeaderText="Actual End ModifiedBy & On" Visible="false" >
                                                    <ItemTemplate>
                                                        <%#Eval("vActivityEndedby")%> <br /> <%#Eval("dActivityEndedOn")%> 
                                                    </ItemTemplate>
                                                      <ItemStyle CssClass="Center"/>
                                                </asp:TemplateField>
                                                   
                                            </Columns>
                                            <RowStyle BackColor="White" />
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;padding:1%">
                            <asp:Button ID="BtnBack2" runat="server" CssClass="btn btnback" Text="" ToolTip="Back" style="visibility: hidden;" />
                        </td>
                    </tr>
                </table>
            </center>
        </div>
         
        <asp:Button ID="btnmdl" runat="server" Style="display: none;" />
        <cc1:ModalPopupExtender ID="mdlSessionTimeoutWarning" runat="server" PopupControlID="divSessionTimeoutWarning"
            BackgroundCssClass="modalBackground" BehaviorID="mdlSessionTimeoutWarning" TargetControlID="btnmdl">
        </cc1:ModalPopupExtender>
        <div id="divSessionTimeoutWarning" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 35%; height: auto; max-height: 25%; min-height: auto;">
            <asp:UpdatePanel ID="HM_Home_upnlSession" runat="server" UpdateMode="Conditional"
                RenderMode="Inline">
                <ContentTemplate>
                    <table width="350px" align="center">
                    <tr>
                        <td>
                            <img id="Img1" src="~/Images/showQuery.png" runat="server" alt="Confirmation" />
                        </td>
                        <td class="Label" style="text-align: left;">
                            Your session will expire within 5 mins.
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button ID="btnContinueWorking" runat="server" Text="Extend" CssClass="btn btnnew" Style="display: none;" />
                            <asp:Button ID="BtnSessionDivClose" runat="server" Text="Close" CssClass="btn btnnew"
                                OnClientClick="closeSessionDiv();" />
                            <asp:Button ID="btnLogout" runat="server" Text="" CssClass="btn btngo" Style="display: none;" />
                        </td>
                    </tr>
                </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        
        <script type="text/javascript">
            var ACTUAL_SESSIONTIME = "<%= Session.Timeout %>", timerId, sessionFlag = true;
            SessionTimeSet();
                
            //$(window).unload(function(){

            //    alert("Byeeeeeeeee");
             

            //});

            function pageLoad() {


                $('[id$="' + '<%= gvwProjectsdetail.ClientID%>' + '"] tbody tr').length < 8 ? scroll = "25%" : scroll = "300px";
                $('#<%= gvwProjectsdetail.ClientID%>').prepend($('<thead>').append($('#<%= gvwProjectsdetail.ClientID%> tr:first'))).dataTable({
                    "bJQueryUI": true,
                    "sScrollX": "100%",
                    "sPaginationType": "full_numbers",
                    "bLengthChange": true,
                    "iDisplayLength": -1,
                    "bProcessing": true,
                    "bSort": false,
                    aLengthMenu: [
                        [10, 25, 50, 100, -1],
                        [10, 25, 50, 100, "All"]
                    ],
                });
                $('#<%= gvwProjectsdetail.ClientID%> tr:first').css('background-color', '#3A87AD');
                $('tr', $('.dataTables_scrollHeadInner')).css("background-color", "rgb(58, 135, 173)");
                setTimeout(function () { $('#<%= gvwProjectsdetail.ClientID%>').dataTable().fnAdjustColumnSizing(); }, 10);
            }


               

        </script>
    </form>
</body>
</html>
