<%@ page language="VB" autoeventwireup="false" inherits="frmMedExInfoHdrDtl, App_Web_xbhimv2u" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <%--<title>Subject Medical Examination </title>--%>
    <link href="App_Themes/StyleBlue/StyleBlue.css" rel="stylesheet" type="text/css" />

    <script src="Script/Validation.js" language="javascript" type="text/javascript"></script>

    <script src="Script/General.js" language="javascript" type="text/javascript"></script>

    <script src="Script/popcalendar.js" language="javascript" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">

        var currTab;

        function closewindow(e) {
           msgConfirmDeleteAlert(null, "Are you sure want to Exit ?", function (isConfirmed) {
                if (isConfirmed) {
                    var parWin = window.opener;
                    if (parWin != null && typeof (parWin) != 'undefined') {
                        parWin.RefreshPage();
                        self.close();
                    }
                    __doPostBack(e.name, '');
                    return true;
                } else {

                    return false;
                }
            });
        }


        function ValidateTextbox(checktype, txt, msg, HighRange, LowRange) {
            var result;
            //alert(checktype);
            if (checktype != 0) {
                switch (checktype) {
                    case 1:
                        result = CheckInteger(txt.value);
                        break;
                    case 2:
                        result = CheckDecimal(txt.value);
                        break;
                    case 3:
                        result = CheckIntegerOrBlank(txt.value);//CheckIntegerOrBlank
                        break;
                    case 4:
                        result = CheckDecimalOrBlank(txt.value);
                        break;
                    case 5:
                        result = CheckAlphabet(txt.value);
                        break;
                    case 6:
                        result = CheckAlphaNumeric(txt.value);
                        break;
                    default: break;          //alert("oh u have all rights ");
                }

                if (result == false) {
                    txt.value = '';
                    txt.focus();
                    msgalert(msg);
                }
            }

            if (HighRange != 0 || LowRange != 0) {
                if (txt.value > HighRange || txt.value < LowRange) {
                    txt.value = '';
                    txt.focus();
                    msgalert('Out Of Range! Range Must be Between ' + LowRange + ' to ' + HighRange);
                }
            }

        }

        function Next(NoneDivId) {
            var arrDiv = NoneDivId.split(',');
            var isShow = false;
            for (i = 0; i < arrDiv.length; i++) {
                document.getElementById(arrDiv[i]).style.display = 'none';
                var disBtn = arrDiv[i].replace('Div', 'BtnDiv');

                document.getElementById(disBtn).style.color = 'Brown';
            }

            for (i = 0; i < arrDiv.length; i++) {
                if (isShow) {
                    currTab = arrDiv[i];
                    isShow = false;
                    break;
                }

                if (arrDiv[i].toLowerCase() == currTab.toLowerCase()) {
                    isShow = true;
                }
            }

            var currBtn = currTab.replace('Div', 'BtnDiv');
            document.getElementById(currTab).style.display = 'block';
            document.getElementById(currBtn).style.color = 'navy';
            return false;
        }

        function Previous(NoneDivId) {
            var arrDiv = NoneDivId.split(',');
            for (i = 0; i < arrDiv.length; i++) {
                document.getElementById(arrDiv[i]).style.display = 'none';
                var disBtn = arrDiv[i].replace('Div', 'BtnDiv');

                document.getElementById(disBtn).style.color = 'Brown';
            }

            for (i = 0; i < arrDiv.length; i++) {
                if (arrDiv[i].toLowerCase() == currTab.toLowerCase()) {
                    if (i > 0) {
                        currTab = arrDiv[i - 1];
                        break;
                    }
                }
            }

            var currBtn = currTab.replace('Div', 'BtnDiv');
            document.getElementById(currTab).style.display = 'block';
            document.getElementById(currBtn).style.color = 'navy';
            return false;
        }



        function DisplayDiv(BlockDivId, NoneDivId) {
            //var disBtn=NoneDivId.replace('Div','Btn');
            //var arrBtn=disBtn.split(',');
            var selBtn = BlockDivId.replace('Div', 'BtnDiv');
            var arrDiv = NoneDivId.split(',');
            //alert(selBtn);
            //alert(BlockDivId);
            for (i = 0; i < arrDiv.length; i++) {
                document.getElementById(arrDiv[i]).style.display = 'none';
                var disBtn = arrDiv[i].replace('Div', 'BtnDiv');
                //alert(disBtn);
                //alert(arrDiv[i]);
                //document.getElementById(disBtn).style.backgroundColor='#336699';
                //alert(document.getElementById(disBtn));
                document.getElementById(disBtn).style.color = 'Brown';
                //alert(document.getElementById(disBtn));
            }
            document.getElementById(BlockDivId).style.display = 'block';
            //document.getElementById(selBtn).style.backgroundColor='white';
            document.getElementById(selBtn).style.color = 'navy';
            return false;
        }

        function ddlAlerton(objId, alerton, alertmsg) {
            if (document.getElementById(objId).value == alerton) {
                msgalert(alertmsg);
            }
        }

        function Alerton(tblName, alertOn, alertMsg) {
            var tblRdo = $get(tblName);
            var name = tblRdo.id;
            name = name.replace(/_/g, '$');
            var rdos = document.getElementsByName(name);

            var i;

            for (i = 0; i < rdos.length; i++) {
                if (rdos[i].checked && rdos[i].value == alertOn) {
                    msgalert(alertMsg);
                }
            }
        }

        function C2F(txtCelsiusID, txtFahrenheitID) {
            var txtCelsius = document.getElementById(txtCelsiusID);
            var txtFahrenheit = document.getElementById(txtFahrenheitID);
            var result;

            if (!(CheckDecimalOrBlank(txtCelsius.value))) {
                msgalert('Please Enter Valid Temperature in Celsius !');
                txtCelsius.focus();
                return false;
            }

            txtFahrenheit.value = c2f(txtCelsius.value);
            return true;
        }

        function F2C(txtFahrenheitID, txtCelsiusID) {
            var txtCelsius = document.getElementById(txtCelsiusID);
            var txtFahrenheit = document.getElementById(txtFahrenheitID);
            var result;

            if (!(CheckDecimalOrBlank(txtFahrenheit.value))) {
                msgalert('Please Enter Valid Temperature in Fahrenheit !');
                txtFahrenheit.focus();
                return false;
            }

            txtCelsius.value = f2c(txtFahrenheit.value);
            return true;
        }



        function QCDivShowHide(Type) {
            //alert(document.getElementById('divQCDtl'));
            if (Type == 'S') {
                document.getElementById('divQCDtl').style.display = 'block';
                SetCenter('divQCDtl');
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
            return false;
        }

        function ValidationQC() {
            //alert(document.getElementById('divQCDtl'));
            if (document.getElementById('txtQCRemarks').value.toString().trim().length <= 0) {
                msgalert('Please Enter Remarks/Response !');
                document.getElementById('txtQCRemarks').focus();
                return false;
            }
            return true;
        }


        function OpenPage() {
            var a = document.getElementById('HFWorkspaceId').value;
            var b = document.getElementById('HType').value;
            var c = document.getElementById('HFPeriodId').value;

            location.href = 'frmWorkspaceSubjectMst.aspx?mode=1&workspaceid=' + a.toString() + '&Page2=frmMedExInfoHdrDtl&Type=' + b.toString() + '&PeriodId=' + c.toString();
        }


        function HistoryDivShowHide(Type, MedexCode, BlockDivId, NoneDivId) {

            //alert(document.getElementById('divHistoryDtl'));

            var btn = document.getElementById('btnHistory');
            document.getElementById('hfMedexCode').value = MedexCode;

            if (Type == 'S') {
                //document.getElementById('divHistoryDtl').style.display = 'block';
                //SetCenter('divHistoryDtl');
                btn.click();
                return false;
            }
            else if (Type == 'H') {
                document.getElementById('divHistoryDtl').style.display = 'none';
                return false;
            }
            else if (Type == 'SN') {
                document.getElementById('divHistoryDtl').style.display = 'block';
                SetCenter('divHistoryDtl');
                return DisplayDiv(BlockDivId, NoneDivId);
            }
            return true;

        }
        function Validation() {

            document.getElementById('BtnSave').style.display = 'none';

            return true;
        }


    </script>

</head>
<body>
    <form id="form1" runat="server" method="post">
    <input style="display: none" type="text" name="fakeusernameremembered" />
    <input style="display: none" type="password" name="fakepasswordremembered" />
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="1000"
            EnablePageMethods="True">
            <Services>
                <asp:ServiceReference Path="AutoComplete.asmx" />
            </Services>
        </asp:ScriptManager>

        <script type="text/javascript">
        
       
        
        </script>

        <table border="0" cellspacing="0" style="border-collapse: collapse" bordercolor="#111111"
            width="998" id="AutoNumber1" cellpadding="0">
            <tr>
                <td style="width: 95%">
                    <img border="0" src="images/topheader.jpg" width="1004">
                </td>
            </tr>
            <tr>
                <td background="images/bluebg.gif" align="left" style="width: 95%">
                    <asp:Label ID="lblWelcome" runat="server" CssClass="Label" Text="Welcome, "></asp:Label><asp:Label
                        ID="lblUserName" runat="server" CssClass="Label" ForeColor="#FF9B2A" Width="618px"></asp:Label>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                <asp:Label ID="lblLoginTime1" runat="server" CssClass="Label" Text="Login Time: "></asp:Label><asp:Label
                    ID="lblTime" runat="server" CssClass="Label" ForeColor="#FF9B2A"></asp:Label>
                    <asp:Label runat="server" ID="lblSessionTimeCap" class="Label" Style="color: #FFFFFF"
                        Text="Session Expires:"></asp:Label>
                    <b><span class="Label" style="color: #FFFFFF" id="timerText"></span></b>

                </td>
            </tr>
            <tr>
                <td background="images/whitebg.gif" align="left" style="width: 95%">&nbsp; &nbsp;
                <%--<asp:LinkButton ID="LinkButton1" runat="server" Font-Bold="True" ForeColor="Navy">Back</asp:LinkButton>&nbsp; --%>
                    <%--<asp:LinkButton ID="lnkAttendance" runat="server" Font-Bold="True" ForeColor="Navy">Attendance</asp:LinkButton>--%>
                    <a onclick="closewindow(this)" style="font-weight: bold; color: Navy; font-family: Verdana; font-size: medium;"><u>Back</u></a>&nbsp; <a onclick="OpenPage()" style="font-weight: bold; color: Navy; font-family: Verdana; font-size: medium; cursor: inherit"><u>Attendance</u></a>&nbsp;
                &nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;<div align="center">
                    <table border="1" cellspacing="1" bordercolor="#F2A041" width="99%" cellpadding="0">
                        <asp:HiddenField ID="CompanyName" runat="server" />
                        <tr>
                            <td align="center" style="width: 98%">
                                <asp:Panel ID="Pan_Hdr" runat="server" Width="100%" CssClass="InnerTable" BackColor="White">
                                    <asp:Panel ID="Pan_Child" runat="server" Width="100%" BackColor="Window">
                                        <div id="Header Label" style="text-align: center;" align="center" class="Div">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td align="left"></td>
                                                    <td align="right" style="font-weight: bold; font-size: x-small; width: 187px; color: Navy; font-family: Verdana, Sans-Serif; font-variant: normal"></td>
                                                </tr>
                                            </table>
                                            <table align="center" style="width: 100%">
                                                <tr>
                                                    <%--<td align="center" width="100%">
                                                            <asp:Label ID="lblHeading" runat="server" SkinID="lblHeading" Font-Bold="True" Font-Size="Large"
                                                                ForeColor="Navy">Screening</asp:Label>
                                                            <br />
                                                        </td>--%>
                                                </tr>
                                                <tr align="center">
                                                    <td align="center" width="890px">
                                                        <strong style="font-weight: bold; font-size: 20px">
                                                            <table style="width: 40%">
                                                                <tr>
                                                                    <td align="left" style="width: 100%">
                                                                        <asp:Label ID="lblHeader" runat="server" SkinID="lblHeading"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            &nbsp;</strong>
                                                        <hr style="width: 982px; background-image: none; color: #ffcc66; background-color: #ffcc66" />
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr align="center">
                                                    <td align="left" width="890">
                                                        <table>
                                                            <tr>
                                                                <td align="left" class="Label">Project: &nbsp;<asp:DropDownList ID="DDLWorkspace" runat="server" CssClass="dropDownList"
                                                                    Width="492px" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                                </td>
                                                                <td align="left" style="vertical-align: middle; background-repeat: no-repeat; white-space: nowrap; background-color: transparent; width: 3px;"></td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="Label">Subject: &nbsp;<asp:DropDownList ID="ddlSubject" runat="server" CssClass="dropDownList"
                                                                    Width="334px">
                                                                </asp:DropDownList>
                                                                    <asp:Button ID="BtnQC" runat="server" CssClass="btn btnnew" OnClientClick="return QCDivShowHide('S');"
                                                                        Text="QA" />
                                                                </td>
                                                                <td style="white-space: nowrap; vertical-align: middle; background-repeat: no-repeat; background-color: transparent; width: 3px;"
                                                                    align="left"></td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="Label">
                                                                    <asp:Label ID="lblMySubjectNo" runat="server" Text="My Subject No.: "></asp:Label><br />
                                                                    <asp:Label ID="lblSubjectID" runat="server" Text="Subject Id: "></asp:Label>
                                                                </td>
                                                                <td align="left" style="vertical-align: middle; width: 3px; background-repeat: no-repeat; white-space: nowrap; background-color: transparent"></td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:HiddenField ID="HFWorkspaceId" runat="server" />
                                                                    <asp:HiddenField ID="HFActivityId" runat="server" />
                                                                    <asp:HiddenField ID="HFNodeId" runat="server" />
                                                                    <asp:HiddenField ID="HFPeriodId" runat="server" />
                                                                    <asp:HiddenField ID="HFSubjectId" runat="server" />
                                                                    <asp:HiddenField ID="HFMySubjectNo" runat="server" />
                                                                    <asp:HiddenField ID="HType" runat="server" />
                                                                    <asp:HiddenField ID="HFMedexInfoDtlTranNo" runat="server" />
                                                                    <asp:HiddenField ID="HFMedexHdrNo" runat="server" />
                                                                    <div id="divQCDtl" runat="server" class="DIVSTYLE2" style="display: none; left: 391px; width: 800px; top: 367px; text-align: left">
                                                                        <asp:Panel ID="Panel1" runat="server" Width="800px" ScrollBars="Auto" Height="268px">
                                                                            <table>
                                                                                <tr class="TR">
                                                                                    <td align="left" class="Label" style="width: 14%"></td>
                                                                                    <td align="left" style="width: 80%">
                                                                                        <asp:RadioButtonList ID="RBLQCFlag" runat="server" RepeatDirection="Horizontal" Visible="False">
                                                                                            <asp:ListItem Value="Y">Approve</asp:ListItem>
                                                                                            <asp:ListItem Value="N">Reject</asp:ListItem>
                                                                                            <asp:ListItem Selected="True" Value="F">Feedback</asp:ListItem>
                                                                                        </asp:RadioButtonList>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="TR">
                                                                                    <td align="left" class="Label" colspan="2">
                                                                                        <asp:Label ID="lblResponse" runat="server"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="TR">
                                                                                    <td align="left" class="Label" style="width: 14%">Remarks/Response :
                                                                                    </td>
                                                                                    <td align="left" colspan="3" style="width: 80%">
                                                                                        <textarea id="txtQCRemarks" class="textBox" runat="server" rows="2" style="width: 277px"></textarea>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="TR">
                                                                                    <td align="left" class="Label" style="width: 14%">To :
                                                                                    </td>
                                                                                    <td align="left" colspan="3" style="width: 80%">
                                                                                        <asp:TextBox ID="txtToEmailId" runat="server" CssClass="textBox" Width="276px"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="TR">
                                                                                    <td align="left" class="Label" style="width: 14%">CC :
                                                                                    </td>
                                                                                    <td align="left" colspan="3" style="width: 80%">
                                                                                        <asp:TextBox ID="txtCCEmailId" runat="server" CssClass="textBox" Height="37px" TextMode="MultiLine"
                                                                                            Width="278px"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="TR">
                                                                                    <td align="left" style="width: 14%">&nbsp;
                                                                                    </td>
                                                                                    <td align="left" colspan="3" style="width: 80%">
                                                                                        <%--<input id="BtnQCSave" runat="server" class="button" style="width: 91px" type="button" value="Save & Send" />--%>
                                                                                        <asp:Button ID="BtnQCSave" runat="server" CssClass="btn btnsave" Text="Save"
                                                                                            OnClientClick="return ValidationQC();" />&nbsp;<asp:Button ID="BtnQCSaveSend" runat="server"
                                                                                                CssClass="btn btnsave" OnClientClick="return ValidationQC();" 
                                                                                                Text="Save & Send" />
                                                                                        <input id="btnExitQC" runat="server" class="btn btnexit" onclick="QCDivShowHide('H');"
                                                                                            type="button" value="Close" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <strong style="text-align: left">
                                                                                <br />
                                                                                QA Comments History </strong>
                                                                            <br />
                                                                            <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                                                                            <br />
                                                                            <asp:GridView ID="GVQCDtl" runat="server" AutoGenerateColumns="False" BorderColor="Peru"
                                                                                Font-Size="Small" SkinID="grdViewSmlSize">
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="nMedexInfoHdrQcNo" HeaderText="nMedexInfoHdrQcNo" />
                                                                                    <%--<asp:BoundField DataField="nMedExInfoHdrNo" HeaderText="nMedExInfoHdrNo" />
                                                                    <asp:BoundField DataField="iNodeId" HeaderText="iNodeId" />
                                                                    <asp:BoundField DataField="iNodeId" HeaderText="iNodeId" />--%>
                                                                                    <asp:BoundField DataField="vWorkSpaceDesc" HeaderText="Project" />
                                                                                    <asp:BoundField DataField="vNodeDisplayName" HeaderText="Activity Name" />
                                                                                    <asp:BoundField DataField="vSubjectId" HeaderText="vSubjectId" />
                                                                                    <asp:BoundField DataField="iTranNo" HeaderText="Sr. No."></asp:BoundField>
                                                                                    <asp:BoundField DataField="FullName" HeaderText="Subject">
                                                                                        <ItemStyle Wrap="false" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="vQCComment" HeaderText="QA Comments" />
                                                                                    <asp:BoundField DataField="cQCFlag" HeaderText="QA" />
                                                                                    <asp:BoundField DataField="vQCGivenBy" HeaderText="QA BY">
                                                                                        <ItemStyle Wrap="false" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="dQCGivenOn" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="QA Date"
                                                                                        HtmlEncode="False">
                                                                                        <ItemStyle Wrap="false" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="vResponse" HeaderText="Response" />
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
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </asp:Panel>
                                                                    </div>
                                                                    <asp:HiddenField ID="hfMedexCode" runat="server" />
                                                                </td>
                                                                <td align="left" style="vertical-align: middle; background-repeat: no-repeat; white-space: nowrap; background-color: transparent; width: 3px;"></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" align="left">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td align="left" valign="top">
                                                                                <br />
                                                                                <asp:UpdatePanel ID="UpPlaceHolder" runat="server" ChildrenAsTriggers="true" EnableViewState="true"
                                                                                    RenderMode="Inline" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:Panel ID="PnlPlaceMedex" runat="server" Width="100%">
                                                                                            <asp:PlaceHolder ID="PlaceMedEx" runat="server" EnableViewState="true"></asp:PlaceHolder>
                                                                                        </asp:Panel>
                                                                                        <div id="divHistoryDtl" runat="server" class="DIVSTYLE2" style="display: none; left: 391px; width: 650px; top: 569px; height: 250px; text-align: left">
                                                                                            <table style="width: 650px">
                                                                                                <tr>
                                                                                                    <td style="width: 100px">
                                                                                                        <strong style="white-space: nowrap">Attribute History Of
                                                                                                            <asp:Label ID="lblMedexDescription" runat="server"></asp:Label></strong>
                                                                                                    </td>
                                                                                                    <td align="right">
                                                                                                        <img onclick="HistoryDivShowHide('H','','','');" src="images/close.gif" style="width: 21px; height: 15px" />
                                                                                                        &nbsp;
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td colspan="2">
                                                                                                        <asp:Panel ID="pnlHistoryDtl" runat="server" Height="250px" ScrollBars="Auto" Width="650px">
                                                                                                            <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                                                                                                            <br />
                                                                                                            <asp:GridView ID="GVHistoryDtl" runat="server" AutoGenerateColumns="False" BorderColor="Peru"
                                                                                                                Font-Size="Small" SkinID="grdViewSmlSize">
                                                                                                                <Columns>
                                                                                                                    <asp:BoundField DataField="vSubjectId" HeaderText="Subject Id">
                                                                                                                        <ItemStyle Wrap="False" />
                                                                                                                    </asp:BoundField>
                                                                                                                    <asp:BoundField DataField="iMySubjectNo" HeaderText="Subject No.">
                                                                                                                        <ItemStyle Wrap="False" />
                                                                                                                    </asp:BoundField>
                                                                                                                    <asp:BoundField DataField="vMedExDesc" HeaderText="Attribute">
                                                                                                                        <ItemStyle Wrap="False" />
                                                                                                                    </asp:BoundField>
                                                                                                                    <asp:BoundField DataField="iTranNo" HeaderText="Sr. No." />
                                                                                                                    <asp:BoundField DataField="vDefaultValue" HeaderText="Value" />
                                                                                                                    <asp:BoundField DataField="vModifyBy" HeaderText="Modify By">
                                                                                                                        <ItemStyle Wrap="False" />
                                                                                                                    </asp:BoundField>
                                                                                                                    <asp:BoundField DataField="dModifyOn" HeaderText="Modify On" HtmlEncode="False">
                                                                                                                        <ItemStyle Wrap="False" />
                                                                                                                    </asp:BoundField>
                                                                                                                </Columns>
                                                                                                            </asp:GridView>
                                                                                                        </asp:Panel>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                        <asp:Button ID="btnHistory" runat="server" CssClass="btn btnnew" OnClick="btnHistory_Click"
                                                                                            Style="display: none" Text="History" />
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="Label" style="width: 100%">
                                                                    <table style="width: 890px">
                                                                        <tr>
                                                                            <td align="left" style="width: 50%">
                                                                                <asp:Button ID="BtnPrevious" runat="server" CssClass="btn btnnew" Text="<< Previous"/>
                                                                            </td>
                                                                            <td align="right" style="width: 50%">
                                                                                <asp:Button ID="BtnNext" runat="server" CssClass="btn btnnew" Text="Next >>"/>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" align="left" class="Label">
                                                                    <asp:Button ID="BtnSave" runat="server" CssClass="btn btnsave" Text="Save" />
                                                                    <asp:Button ID="btnPrint" runat="server" CssClass="btn btnnew" Text="Print" Visible="False" />&nbsp;<input
                                                                        id="BtnExit" class="btn btnexit" type="button" value="Exit" onclick="closewindow(this);" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <br />
                                    </asp:Panel>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </div>
                </td>
            </tr>
            <tr>
                <td style="width: 95%">&nbsp;
                </td>
            </tr>
            <tr>
                <td background="images/orangebg.gif" style="width: 95%">
                    <p align="center">

                        <script type="text/javascript">
                            var copyright;
                            var update;
                            copyright = new Date();
                            update = copyright.getFullYear();
                            document.write("<font face=\"verdana\" size=\"1\" color=\"black\">© Copyright " + update + "," + $get('<%= CompanyName.clientid %>').value + "</font>");
                        </script>
                </td>
            </tr>
        </table>

        <%--<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False">
    </asp:GridView>--%>

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
        </script>
    </form>
</body>
</html>
