<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmSendSamples.aspx.vb" Inherits="frmSendSamples" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <script type="text/javascript" src="Script/popcalendar.js"></script>

    <asp:UpdatePanel ID="upSendSamples" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                <legend class="LegendText" style="color: Black; font-size: 12px">
                    <img id="img2" alt="Sample Details" src="images/panelcollapse.png"
                        onclick="Display(this,'divSampleDetail');" runat="server" style="margin-right: 2px;" />Sample Details</legend>
                <div id="divSampleDetail">
                    <table style="width: 100%" cellpadding="5px">
                        <tbody>
                            <tr>
                                <td class="Label" style="text-align: center;">
                                    <asp:RadioButtonList ID="rblSelection" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblSelection_SelectedIndexChanged"
                                        RepeatDirection="Horizontal" Style="margin: auto; width: 30%;">
                                        <asp:ListItem Value="00">Screening</asp:ListItem>
                                        <asp:ListItem Value="01">Project Specific</asp:ListItem>
                                        <asp:ListItem Value="02">Track Discard</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="display: none; left: 701px; width: 40%; position: absolute; top: 613px; height: 134px"
                                        id="divSend" class="divStyleNoAbs" runat="server">
                                        <table style="width: 100%;" cellpadding="5">
                                            <tbody>
                                                <tr>
                                                    <td class="Label" colspan="2" style="text-align: center;">
                                                        <asp:Label ID="lblSendTo" runat="server" Text="Send To Division"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right;" class="Label">Division :
                                                    </td>
                                                    <td style="text-align: left;">
                                                        <asp:DropDownList ID="ddlDivDivision" runat="server" CssClass="dropDownList" Width="60%">
                                                            <asp:ListItem Value="1">Select Division</asp:ListItem>
                                                            <asp:ListItem Value="0015">CLLab</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="Label" style="text-align: center;" colspan="2">
                                                        <asp:Button ID="btnDivSend" OnClick="btnDivSend_Click" runat="server" Text="Send"
                                                            ToolTip="Send" CssClass="btn btnsave" OnClientClick="return ValidationForddlDivDivision();" />
                                                        <asp:Button ID="btnDivClose" OnClick="btnDivClose_Click" runat="server" Text="Close"
                                                            ToolTip="Close" CssClass="btn btnclose" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <asp:Panel ID="pnlProjectSpecific" runat="Server">
                                        <table width="100%">
                                            <tbody>
                                                <tr>
                                                    <td style="text-align: right; width: 70%" class="Label">Project Name/Request Id:
                                                <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="60%"></asp:TextBox>
                                                    </td>
                                                    <td style="text-align: left;">
                                                        <asp:CheckBox ID="chkAllProjects" TabIndex="1" runat="server" Text="All" AutoPostBack="True"
                                                            OnCheckedChanged="chkAllProjects_CheckedChanged"></asp:CheckBox>
                                                        <asp:Button Style="display: none" ID="btnSetProject" OnClientClick="getData(this);" OnClick="btnSetProject_Click" runat="server" Text=" Project"></asp:Button>
                                                        <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                                            CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                            CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                                            OnClientShowing="ClientPopulated" ServiceMethod="GetMyProjectCompletionList"
                                                            ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True">
                                                        </cc1:AutoCompleteExtender>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>

                                <td style="text-align: center" class="Label" colspan="3">
                                    From Date :
                                    <asp:TextBox ID="txtfromDate" Enabled="true" runat="server" CssClass="textBox" Width="10%"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtfromDate"  Format="dd-MMM-yyyy"></cc1:CalendarExtender>

                                    To Date : 
                                    <asp:TextBox ID="txttoDate" Enabled="true" runat="server" CssClass="textBox" Width="10%"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txttoDate"  Format="dd-MMM-yyyy"></cc1:CalendarExtender>

                                    <asp:Button ID="btnSearch" TabIndex="4" OnClientClick="return Datevalidate();" OnClick="btnSearch_Click" runat="server"
                                        Text="Search" CssClass="btn btnnew"  ToolTip="Search"></asp:Button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br /><br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset id="fsetSendSample" runat="server" class="FieldSetBox" style="display: none; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                <legend class="LegendText" style="color: Black; font-size: 12px">
                    <img id="img1" alt="Sample Data" src="images/panelcollapse.png"
                        onclick="Display(this,'divSampleData');" runat="server" style="margin-right: 2px;" />Sample Data</legend>
                <div id="divSampleData">
                    <table style="width: 100%" cellpadding="5px">
                        <tbody>
                            <tr>
                                <td style="text-align: left" align="left">
                                    <asp:Panel ID="PannelgvDiscard" runat="server" BorderStyle="None" BorderWidth="1px"
                                        Style="margin: auto; margin-left: 2%; width: 96%;">
                                        <asp:GridView ID="gvDiscardSample" TabIndex="5" runat="server" PageSize="3000" OnRowCreated="gvDiscardSample_RowCreated"
                                            OnRowDataBound="gvDiscardSample_RowDataBound" AutoGenerateColumns="False" Width="100%">
                                            <RowStyle BackColor="#cee3ed" Font-Names="Verdana" VerticalAlign="Middle" HorizontalAlign="left"
                                                Font-Size="9pt" ForeColor="navy" />
                                            <EditRowStyle BackColor="#cee3ed" Font-Names="Verdana" Font-Size="9pt" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Underline="False" BackColor="#1560a1" Font-Names="Verdana" VerticalAlign="Top"
                                                Font-Size="10pt" HorizontalAlign="Center" ForeColor="white" Font-Bold="True" />
                                            <FooterStyle BackColor="#1560a1" Font-Names="Verdana" Font-Size="X-Small" HorizontalAlign="left"
                                                ForeColor="Navy" Font-Bold="True" />
                                            <AlternatingRowStyle BackColor="white" Font-Names="Verdana" HorizontalAlign="left"
                                                Font-Size="9pt" ForeColor="navy" />
                                            <PagerStyle ForeColor="#ffa24a" Font-Underline="False" BackColor="white" Font-Bold="True"
                                                Font-Names="Verdana" HorizontalAlign="Center" Font-Size="X-Small" />
                                            <Columns>
                                                <asp:BoundField DataField="vSampleBarCode" HeaderText="Sample" />
                                                <asp:BoundField DataField="vSampleTypeDesc" HeaderText="Sample Type" />
                                                <asp:BoundField DataField="vWorkspacedesc" HeaderText="Project">
                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vSubjectID" HeaderText="Subject Id">
                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                </asp:BoundField>

                                                <asp:BoundField DataField="FullName" HeaderText="Subject" />
                                                <asp:BoundField DataField="vNodeDisplayName" HeaderText="Activity" />
                                                <asp:BoundField DataField="CollectionUser" HeaderText="CollectedBy" HtmlEncode="False" />
                                                <asp:BoundField DataField="dCollectionDateTime" HeaderText="Collection DateTime" HtmlEncode="False" HeaderStyle-Wrap="true">
                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DiscardUser" HeaderText="DiscardedBy" HtmlEncode="False" />

                                                <asp:BoundField DataField="DiscardDate" HeaderText="Discard Date" HtmlEncode="False" HeaderStyle-Wrap="true">
                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                </asp:BoundField>

                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDisPuteRemark" runat="server" Enabled="false" Text='<%# DataBinder.Eval(Container.Dataitem,"vRemark") %>'
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>

                                    </asp:Panel>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td class="Label" style="text-align: center;">
                                    <asp:Label ID="lblSampleId" runat="server" Text="Sample Id:" Width="5%"></asp:Label>
                                    <asp:TextBox ID="txtSampleId" TabIndex="1" runat="server" Visible="False" OnTextChanged="txtSampleId_TextChanged"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left" align="left">
                                    <asp:Panel ID="pnlgvwSample" runat="server" BorderStyle="None" BorderWidth="1px"
                                        Style="margin: auto; width: 100%;">
                                        <asp:GridView ID="gvwSample" TabIndex="2" runat="server" PageSize="3000" OnRowCommand="gvwSample_RowCommand"
                                            OnRowCreated="gvwSample_RowCreated" OnRowDataBound="gvwSample_RowDataBound" AutoGenerateColumns="False"
                                            Width="100%">
                                            <RowStyle BackColor="#cee3ed" Font-Names="Verdana" VerticalAlign="Middle" HorizontalAlign="left"
                                                Font-Size="9pt" ForeColor="navy" />
                                            <EditRowStyle BackColor="#cee3ed" Font-Names="Verdana" Font-Size="9pt" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Underline="False" BackColor="#1560a1" Font-Names="Verdana" VerticalAlign="Top"
                                                Font-Size="10pt" HorizontalAlign="Center" ForeColor="white" Font-Bold="True" />
                                            <FooterStyle BackColor="#1560a1" Font-Names="Verdana" Font-Size="X-Small" HorizontalAlign="left"
                                                ForeColor="Navy" Font-Bold="True" />
                                            <AlternatingRowStyle BackColor="white" Font-Names="Verdana" HorizontalAlign="left"
                                                Font-Size="9pt" ForeColor="navy" />
                                            <PagerStyle ForeColor="#ffa24a" Font-Underline="False" BackColor="white" Font-Bold="True"
                                                Font-Names="Verdana" HorizontalAlign="Center" Font-Size="X-Small" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="All">
                                                    <HeaderTemplate>
                                                        <input id="chkSelectAll" onclick="SelectAll(this, 'gvwSample')" type="checkbox" />
                                                        <asp:Label ID="chkSelectAll" runat="server" Text="All"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelectSample" onclick="CheckUncheckAll('gvwSample')" runat="server"></asp:CheckBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="nSampleTypeDetailNo" HeaderText="nSampleTypeDetailNo" />
                                                <asp:BoundField DataField="vSampleBarCode" HeaderText="Sample" />
                                                <asp:BoundField DataField="vSampleTypeDesc" HeaderText="Sample Type" />
                                                <asp:BoundField DataField="cSampleStatusFlag" HeaderText="Sample Status" />
                                                <asp:BoundField DataField="vWorkspacedesc" HeaderText="Project">
                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vSubjectID" HeaderText="Subject Id">
                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vMySubjectNo" HeaderText="MySubjectNo" />
                                                <asp:BoundField DataField="FullName" HeaderText="Subject" />
                                                <asp:BoundField DataField="vNodeDisplayName" HeaderText="Activity" />
                                                <asp:BoundField DataField="CollectionUser" HeaderText="Collected By" />
                                                <asp:BoundField DataField="dCollectionDateTime" HeaderText="Collection DateTime" HtmlEncode="False" />
                                                <asp:TemplateField HeaderText="Send">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnSend" OnClick="btnSend_Click" runat="server" Text="Send" ToolTip="Send"
                                                            CssClass="btn btnsave"></asp:Button>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Discard">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDisPuteRemark" runat="server" Text='<%# DataBinder.Eval(Container.Dataitem,"vRemark") %>'
                                                            TextMode="MultiLine"></asp:TextBox><asp:Button ID="btnDisPute" runat="server" Text="Discard"
                                                                ToolTip="Discard" CssClass="btn btnnew" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>

                                    </asp:Panel>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center">
                                    <asp:Panel ID="pnlPagging" runat="server" BorderStyle="None" BorderWidth="1px"
                                        Style="margin: auto; width: 100%;">
                                        <asp:Repeater ID="rptPager" runat="server">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>' Enabled='<%# Eval("Enabled") %>' OnClick="Page_Changed"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center" class="Label">
                                    <asp:Label ID="lblSendToDivision" runat="server" Text="Send To Division:"></asp:Label>
                                    <asp:DropDownList ID="ddlDivision" TabIndex="2" runat="server" CssClass="dropDownList"
                                        Width="15%">
                                        <asp:ListItem Value="1">Select Division</asp:ListItem>
                                        <asp:ListItem Value="0015">CLLAb</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center">
                                    <asp:Button ID="btnSendAll" TabIndex="3" OnClick="btnSendAll_Click" runat="server"
                                        Text="Send All" ToolTip="Send All" CssClass="btn btnsave" OnClientClick="return ValidationToSend('<%= gvwSample.ClientID %>');"></asp:Button>
                                    <asp:Button ID="btnCancel" TabIndex="4" OnClick="btnCancel_Click" runat="server"
                                        Text="Cancel" CssClass="btn btncancel"  ToolTip="Cancel"></asp:Button>
                                    <asp:Button ID="btnExit" TabIndex="5" OnClick="btnExit_Click" runat="server" Text="Exit"
                                        ToolTip="Exit" CssClass="btn btnexit" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this); "></asp:Button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </fieldset>
            <asp:HiddenField ID="hndLockStatus" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript" language="javascript">

        function fsetSendSample_Show() {
            $('#<%=fsetSendSample.ClientID%>').attr('style', $('#<%=fsetSendSample.ClientID%>').attr('style') + ';display:block');
        }

        function Display(control, target) {
            if (control.src.toString().toUpperCase().search("EXPAND") != -1) {
                $("#" + target).slideToggle(600);
                control.src = "images/panelcollapse.png";
            }
            else {
                $("#" + target).slideToggle(600);
                control.src = "images/panelexpand.png";
            }
        }

        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));

        }

        function SelectAll(CheckBoxControl, Grid) {
            if (CheckBoxControl.checked == true) {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf(Grid) > -1)) {
                        if (document.forms[0].elements[i].disabled == false) {
                            document.forms[0].elements[i].checked = true;
                        }
                    }
                }
            }
            else {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf(Grid) > -1)) {
                        document.forms[0].elements[i].checked = false;
                    }
                }
            }
        }

        function ValidationForddlDivDivision() {
            debugger;
            if (document.getElementById('<%=ddlDivDivision.clientid %>').selectedIndex == 0) {
                msgalert('Select Division !');
                return false;
            }
            //return true;
        }

        Sys.Browser.WebKit = {};
        if (navigator.userAgent.indexOf('WebKit/') > -1) {
            Sys.Browser.agent = Sys.Browser.WebKit;
            Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
            Sys.Browser.name = 'WebKit';
        }

        function ValidationToSend(gv) {
            if (document.getElementById('<%=ddlDivision.clientid %>').selectedIndex == 0) {
                msgalert('Select Division !');
                return false;
            }
            var gvwSample = document.getElementById('<%= gvwSample.ClientID %>');
            if (gvwSample == null || typeof (gvwSample) == 'undefined') {
                return false;
            }
                //alert(CheckOne('gvwSample'));
            else if (CheckOne(gvwSample.id) == false) {
                msgalert('Select atleast one Sample !');
                return false;
            }
            return true;
        }

        //------------functions for DIV
        function ShowElement(btnid, elemid) {
            var elem = document.getElementById(elemid);
            var btn = document.getElementById(btnid);
            var tl = GetElementTopLeft(btn);
            elem.style.display = 'block';
            SetCenter(elemid);
        }

        function HideElement(elemId) {
            var elem = document.getElementById(elemId);
            elem.style.display = 'none';
        }
        //-----------------------

        //-------added on 14-Sep-09 by Deepak Singh to uncheck ChekAll if a single checkbox is unchecked or check CheckALL if all checkboxes are checked individually       
        function CheckUncheckAll(Grid) {
            var Checkall = document.getElementById('chkSelectAll');
            var Gvd = document.getElementById('<%=gvwSample.ClientId %>');
            j = 0;
            for (i = 0; i < document.forms[0].elements.length; i++) {
                if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf(Grid) > -1)) {
                    j = j + 1;
                    if (document.forms[0].elements[i].checked == false) {
                        Checkall.checked = false;
                        break;
                    }
                    else if (document.forms[0].elements[i].checked == true) {
                        if (j == Gvd.rows.length - 2) {
                            Checkall.checked = true;
                        }
                    }
                }
            }
        }

        //Add by shivani pandya for project lock
        function getData(e) {
            var WorkspaceID = $('input[id$=HProjectId]').val();
            $.ajax({
                type: "post",
                url: "frmSendSamples.aspx/LockImpact",
                data: '{"WorkspaceID":"' + WorkspaceID + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                dataType: "json",
                success: function (data) {
                    if (data.d == "L") {
                        msgalert("Project is locked !");
                        $("#<%=hndLockStatus.ClientID%>").val("Lock");
                        $("#ctl00_CPHLAMBDA_gvwSample [type=checkbox]").attr("Disabled", "Disabled");
                        $("textarea").attr("Disabled", "Disabled");
                        $("#ctl00_CPHLAMBDA_pgvwSamle [type=submit]").attr("Disabled", "Disabled");
                        $("#ctl00_CPHLAMBDA_txtSampleId").attr("Disabled", "Disabled");
                        $("#ctl00_CPHLAMBDA_ddlDivision").attr("Disabled", "Disabled");
                        $("#ctl00_CPHLAMBDA_btnSendAll").attr("Disabled", "Disabled");
                    }
                    if (data.d == "U") {
                        $("#<%=hndLockStatus.ClientID%>").val("UnLock");
                        $("#ctl00_CPHLAMBDA_btnSendAll").removeAttr("Disabled");
                        $("#ctl00_CPHLAMBDA_ddlDivision").removeAttr("Disabled");
                        $("#ctl00_CPHLAMBDA_txtSampleId").removeAttr("Disabled");
                        $("#ctl00_CPHLAMBDA_gvwSample [type=submit]").removeAttr("Disabled");
                        $("textarea").removeAttr("Disabled");
                        $("#ctl00_CPHLAMBDA_gvwSample [type=checkbox]").removeAttr("Disabled");
                    }
                },
                failure: function (response) {
                    msgalert(response.d);
                },
                error: function (response) {
                    msgalert(response.d);
                }
            });
            return true;
        }

        //function FixHeader(id) {
        //    FreezeTableHeader($('#' + id.id), { height: 300, width: 1314 });
        //}
        function DisabledData() {
            $("#ctl00_CPHLAMBDA_gvwSample [type=checkbox]").attr("Disabled", "Disabled");
            $("textarea").attr("Disabled", "Disabled");
            $("#ctl00_CPHLAMBDA_gvwSample [type=submit]").attr("Disabled", "Disabled");
            $("#ctl00_CPHLAMBDA_txtSampleId").attr("Disabled", "Disabled");
            $("#ctl00_CPHLAMBDA_ddlDivision").attr("Disabled", "Disabled");
            $("#ctl00_CPHLAMBDA_btnSendAll").attr("Disabled", "Disabled");
        }

        function EnabledData() {
            $("#ctl00_CPHLAMBDA_btnSendAll").removeAttr("Disabled");
            $("#ctl00_CPHLAMBDA_ddlDivision").removeAttr("Disabled");
            $("#ctl00_CPHLAMBDA_txtSampleId").removeAttr("Disabled");
            $("#ctl00_CPHLAMBDA_gvwSample [type=submit]").removeAttr("Disabled");
            $("textarea").removeAttr("Disabled");
            $("#ctl00_CPHLAMBDA_gvwSample [type=checkbox]").removeAttr("Disabled");
        }
        function Datevalidate() {

            var objFromDate = document.getElementById("ctl00_CPHLAMBDA_txtfromDate").value;
            var objToDate = document.getElementById("ctl00_CPHLAMBDA_txttoDate").value;

            var date1 = new Date(objFromDate);
            var date2 = new Date(objToDate);

            //var Month = (date2.getMonth() - date1.getMonth()) + ((date2.getFullYear() - date1.getFullYear()) * 12)
            var ONE_DAY = 1000 * 60 * 60 * 24
            var date1_ms = date1.getTime()
            var date2_ms = date2.getTime()
            var difference_ms = Math.abs(date1_ms - date2_ms)
            var day = Math.round(difference_ms / ONE_DAY)



            if (date1 > date2) {
                msgalert("FromDate should be less than ToDate !");
                return false;
            }
            if (!Date.parse(date1) || !Date.parse(date2)) {
                msgalert('Please Select Date !');
                return false;
            }
            if (day > 30) {
                msgalert("FromDate And ToDate Must Be Between 1 Month !");
                return false;
            }

            else {

                return true;
            }

        }
    </script>

</asp:Content>
