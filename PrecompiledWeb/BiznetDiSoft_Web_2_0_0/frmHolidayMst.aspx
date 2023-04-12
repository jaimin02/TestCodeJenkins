<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmHolidayMst, App_Web_pna05jsx" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/popcalendar.js"></script>

    <script type="text/javascript">
        function Validation() {
            if (document.getElementById('<%=txtDate.ClientID%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=txtDate.ClientID%>').value = '';
                msgalert('Please Select Date !');
                //            document.getElementById('<%=txtDate.ClientID%>').focus();
                return false;
            }
            if (CheckSelectedLocations() == false) {
                return false;
            }

            return true;
        }
        function CheckSelectedLocations() {
            var CheckBoxList = document.getElementById('<%=chklstLocation.ClientId %>');
            var CheckBoxes = CheckBoxList.getElementsByTagName('input');
            var CountLocations = 0;
            for (i = 0; i < CheckBoxes.length; i++) {
                if (CheckBoxes[i].type == 'checkbox' || checkBoxes[i].type == 'CHECKBOX') {
                    if (CheckBoxes[i].checked == true) {
                        CountLocations += 1;
                    }
                }
            }
            if (CountLocations <= 0) {
                msgalert("Please select Location(s) to add !");
                return false;
            }
            return true;
        }

        function ShowHideHolidayOption() {

            document.getElementById('<%= divHolidayOption.ClientId %>').style.display = 'block';
            document.getElementById('<%= divWeeklyOffOption.ClientId %>').style.display = 'none';
            if (document.getElementById('<%= divHolidayOption.ClientId %>').style.display == 'block') {
                document.getElementById('<%= pnlView.ClientId %>').style.display = 'none';
            }

        }
        function ShowHideWeeklyOffOption() {

            document.getElementById('<%= divHolidayOption.ClientId %>').style.display = 'none';
            document.getElementById('<%= divWeeklyOffOption.ClientId %>').style.display = 'block';
            if (document.getElementById('<%= divHolidayOption.ClientId %>').style.display == 'block') {
                document.getElementById('<%= pnlView.ClientId %>').style.display = 'none';
            }

        }

        function CheckBoxListSelection(tableName, chk) {
            var checkBoxes = tableName.getElementsByTagName('input');
            for (i = 0; i < checkBoxes.length; i++) {
                if (checkBoxes[i].type == 'checkbox' || checkBoxes[i].type == 'CHECKBOX') {
                    checkBoxes[i].checked = chk.checked;
                }
            }
        }

        function SetCheckUncheckAll(objTable, chkAll) {
            var i = 0;
            var checkBoxes = objTable.getElementsByTagName('input');
            for (i = 0; i < checkBoxes.length; i++) {
                if (checkBoxes[i].type == 'checkbox' || checkBoxes[i].type == 'CHECKBOX') {
                    if (checkBoxes[i].checked == false) {
                        chkAll.checked = false;
                        return;
                    }
                }
            }
            if (i == checkBoxes.length) {
                chkAll.checked = true;
            }
        }

        function ClearConfirm(e) {
            msgConfirmDeleteAlert(null, "Are you sure ? It will clear All Holidays.", function (isConfirmed) {
                if (isConfirmed) {
                    __doPostBack(e.name, '');
                    return true;
                } else {
                    return false;
                }
            });
            return false;
        }
       
    </script>

    <table style="width: 100%; margin: auto">
        <tr>
            <td valign="top" align="center" width="100%">
                <asp:UpdatePanel ID="updPnlHOLEntryForm" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table width="100%">
                            <tbody>
                                <tr>
                                    <td class="Label" align="center" colspan="2">
                                        Select Holiday Type: <span style="font-weight: normal" class="Label">
                                            <input id="rdoHoliday" onclick="ShowHideHolidayOption();" type="radio" checked value="H"
                                                name="HolidayType" runat="server" />Holiday
                                            <input id="rdoWeeklyOff" onclick="ShowHideWeeklyOffOption();" type="radio" value="W"
                                                name="HolidayType" runat="server" />Weekly Off </span>
                                    </td>
                                </tr>
                                <tr style="width: 100%;" align="right">
                                    <td valign="top">
                                        <table cellpadding="5">
                                            <tbody>
                                                <tr>
                                                    <td class="Label" valign="top" align="right">
                                                        Select Location:
                                                    </td>
                                                    <td class="Label" align="left">
                                                        <asp:DropDownList ID="ddlLocation" runat="server" CssClass="dropDownList" __designer:wfdid="w1">
                                                        </asp:DropDownList>
                                                        <br />
                                                        <asp:CheckBox ID="chkSelectAllLocation" runat="server" Text="Select / UnSelect" __designer:wfdid="w4">
                                                        </asp:CheckBox>
                                                        <div style="border-right: gray thin solid; border-top: gray thin solid; overflow-y: scroll;
                                                            border-left: gray thin solid; width: 212px; border-bottom: gray thin solid; height: 100px;
                                                            text-align: left" id="dvChkListLocation" runat="server">
                                                            <asp:CheckBoxList ID="chklstLocation" runat="server" ForeColor="Black" Font-Size="XX-Small"
                                                                Font-Names="Verdana" CssClass="checkboxlist" Width="260px" __designer:wfdid="w5"
                                                                Height="37px">
                                                            </asp:CheckBoxList>
                                                        </div>
                                                        <br />
                                                        <asp:Button ID="btnClear" runat="server" Text="Clear All Holidays" CssClass="btn btncancel"
                                                            ToolTip="clear Holidays"  __designer:wfdid="w6" OnClientClick="return ClearConfirm(this); ">
                                                        </asp:Button>
                                                        <asp:Button ID="btnExit" OnClick="btnExit_Click" runat="server" Text="Exit" CssClass="btn btnexit"
                                                            ToolTip="Exit" __designer:wfdid="w8" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);">
                                                        </asp:Button>
                                                        <asp:Button ID="btnView" OnClick="btnView_Click" runat="server" Text="View" CssClass="btn btnnew"
                                                            ToolTip="View" __designer:wfdid="w1"></asp:Button>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                    <td style="padding-left: 15px" valign="top" align="left">
                                        <div id="divHolidayOption" runat="server">
                                            <table style="padding-left: 10px" cellpadding="3">
                                                <tbody>
                                                    <tr>
                                                        <td class="Label">
                                                            Date:
                                                        </td>
                                                        <td class="Label">
                                                            <asp:TextBox ID="txtDate" runat="server" CssClass="textBox" __designer:wfdid="w7" Enabled="true"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDate"  Format="dd-MMM-yyyy"></cc1:CalendarExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Label">
                                                            Description:
                                                        </td>
                                                        <td class="Label">
                                                            <asp:TextBox ID="txtHolidayDescription" runat="server" CssClass="textBox" __designer:wfdid="w8"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Label">
                                                            Active
                                                        </td>
                                                        <td class="Label">
                                                            <asp:CheckBox ID="chkIsActive" runat="server" __designer:wfdid="w9" Checked="True">
                                                            </asp:CheckBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Label">
                                                            <asp:Button ID="btnAddHoliday" runat="server" Text=" Add " CssClass="btn btnnew" ToolTip="Add"
                                                                __designer:wfdid="w10"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <div id="divUpdateCancel" class="Label" runat="server" visible="false">
                                                                <asp:Button ID="btnUpdate" runat="server" Text=" Update " CssClass="btn btnupdate" __designer:wfdid="w11">
                                                                </asp:Button>&nbsp;<asp:Button ID="btnCancel" runat="server" Text=" Cancel " CssClass="btn btncancel"
                                                                    __designer:wfdid="w12" ToolTip="Cancel"></asp:Button>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                        <div id="divWeeklyOffOption" runat="server">
                                            <table cellpadding="3" style =" width :100%">
                                                <tbody>
                                                    <tr>
                                                        <td class="Label">
                                                            Year:
                                                        </td>
                                                        <td class="Label">
                                                            <asp:DropDownList ID="ddlYear" runat="server" CssClass="dropDownList" __designer:wfdid="w13">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Label" valign="top">
                                                            Saturday:
                                                        </td>
                                                        <td class="Label" valign="top">
                                                            <div style="border-right: gray thin solid; border-top: gray thin solid; overflow-y: scroll;
                                                                border-left: gray thin solid; width: 212px; border-bottom: gray thin solid; height: 125px;
                                                                text-align: left" id="divWeekSaturday" runat="server">
                                                                <asp:CheckBoxList ID="chkLstSaturday" runat="server" ForeColor="Black" Font-Size="XX-Small"
                                                                    Font-Names="Verdana" CssClass="checkboxlist" Width="260px" __designer:wfdid="w14"
                                                                    Height="47px">
                                                                    <asp:ListItem Value="1">First</asp:ListItem>
                                                                    <asp:ListItem Value="2">Second</asp:ListItem>
                                                                    <asp:ListItem Value="3">Third</asp:ListItem>
                                                                    <asp:ListItem Value="4">Fourth</asp:ListItem>
                                                                    <asp:ListItem Value="5">Fifth</asp:ListItem>
                                                                </asp:CheckBoxList>
                                                            </div>
                                                            <asp:Button ID="btnGo" runat="server" Text="" CssClass="btn btngo" ToolTip="Go"
                                                                __designer:wfdid="w15" OnClientClick="return CheckSelectedLocations();"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" colspan="2">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;"  valign="top" colspan="2">
                                        <asp:Panel ID="pnlView" runat="server" Visible="False" __designer:dtid="281474976710667"
                                            __designer:wfdid="w23" BorderStyle="Solid" BorderWidth="1px">
                                            <table id="tblView" cellpadding="5" __designer:dtid="281474976710668" visible="false">
                                                <tbody>
                                                    <tr __designer:dtid="281474976710671">
                                                        <td class="Label" valign="top" nowrap align="left" colspan="2" __designer:dtid="281474976710672">
                                                            From :
                                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="textBox" Width="87px" __designer:dtid="281474976710674"
                                                                __designer:wfdid="w33" Enabled="true"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFromDate"  Format="dd-MMM-yyyy"></cc1:CalendarExtender>
                                                            

                                                            &nbsp;&nbsp; To :
                                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="textBox" Width="87px" __designer:dtid="281474976710679"
                                                                __designer:wfdid="w34" Enabled="False"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtToDate"  Format="dd-MMM-yyyy"></cc1:CalendarExtender>

                                                            &nbsp;
                                                            <%--<img id="ImgToDate" onclick="popUpCalendar(this,document.getElementById('<%= txtToDate.ClientId %>'),'dd-mmm-yy');"
                                                                    alt="Select  Date" src="images/Calendar_scheduleHS.png" __designer:dtid="281474976710680"
                                                                    __designer:servercode=" onclick " />--%>
                                                            
                                                            &nbsp; &nbsp;<asp:Button ID="btnDivView" OnClick="btnDivView_Click"
                                                                        runat="server" Text="View" CssClass="btn btnadd" __designer:dtid="281474976710683"
                                                                        __designer:wfdid="w35"></asp:Button>
                                                            <asp:Button ID="btnDivClose" OnClick="btnDivClose_Click" runat="server" Text="Close"
                                                                CssClass="btn btnclose" __designer:dtid="281474976710684" __designer:wfdid="w36"></asp:Button>
                                                        </td>
                                                    </tr>
                                                    <tr __designer:dtid="281474976710681">
                                                        <td class="Label" valign="top" nowrap align="center" colspan="2" __designer:dtid="281474976710682">
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                            <asp:GridView ID="gvwView" runat="server" SkinID="grdViewAutoSizeMax" __designer:wfdid="w37"
                                                OnRowCommand="gvwView_RowCommand" OnPageIndexChanging="gvwView_PageIndexChanging"
                                                AllowPaging="True" AutoGenerateColumns="False" OnRowCreated="gvwView_RowCreated">
                                                <Columns>
                                                    <asp:BoundField DataField="iHolidayNo" HeaderText="HolidayNo"></asp:BoundField>
                                                    <asp:BoundField DataField="vLocationCode" HeaderText="LocationCode"></asp:BoundField>
                                                    <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd-MMM-yy}" DataField="dHolidayDate"
                                                        HeaderText="HolidayDate"></asp:BoundField>
                                                    <asp:BoundField DataField="cHolidayType" HeaderText="HolidayType"></asp:BoundField>
                                                    <asp:BoundField DataField="vHolidayDescription" HeaderText="HolidayDescription">
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="cActiveFlag" HeaderText="ActiveFlag"></asp:BoundField>
                                                    <asp:BoundField DataField="iModifyBy" HeaderText="ModifyBy"></asp:BoundField>
                                                    <asp:BoundField DataField="dModifyOn" HeaderText="ModifyOn"></asp:BoundField>
                                                    <asp:BoundField DataField="cStatusIndi" HeaderText="StatusIndi"></asp:BoundField>
                                                    <asp:BoundField DataField="vLocationName" HeaderText="LocationName"></asp:BoundField>
                                                    <asp:TemplateField HeaderText="Edit" ShowHeader="False">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgBtnEdit_View" runat="server" CausesValidation="False" CommandArgument='<%# Eval("iHolidayNo") %>'
                                                                CommandName="MyEdit" ImageUrl="~/images/Edit2.gif" Text="Button" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Delete" ShowHeader="False">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgBtnDelete_View" runat="server" CausesValidation="False" CommandArgument='<%# Eval("iHolidayNo") %>'
                                                                CommandName="MyDelete" ImageUrl="~/images/i_delete.gif" Text="Button" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvwHolidays" EventName="RowCommand"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="gvwHolidays" EventName="RowCreated"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="gvwHolidays" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <table cellpadding="3px" style =" width:100%;">
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="updPnlGVWHoliday" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="gvwHolidays" runat="server" SkinID="grdViewAutoSizeMax" __designer:wfdid="w38"
                                        OnRowCommand="gvwHolidays_RowCommand" OnPageIndexChanging="gvwHolidays_PageIndexChanging"
                                        AllowPaging="True" PageSize="10" AutoGenerateColumns="False" OnRowDataBound="gvwHolidays_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="iHolidayNo" HeaderText="#">
                                             <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="vLocationCode" HeaderText="Location Code">
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="vLocationName" HeaderText="Location">
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="dHolidayDate" DataFormatString="{0:dd-MMM-yy}" HeaderText="Date"
                                                HtmlEncode="False">
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="vHolidayDescription" HeaderText="Holiday Name">
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Day">
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="cHolidayType" HeaderText="Type"></asp:BoundField>
                                            <asp:TemplateField HeaderText="Edit" ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="False" CommandArgument='<%# Eval("iHolidayNo") %>'
                                                        CommandName="MyEdit" ImageUrl="~/images/Edit2.gif" Text="Button" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:ButtonField CommandName="MyDelete" ImageUrl="~/Images/i_delete.gif" Text="Button"
                                                ButtonType="Image" HeaderText="Delete">
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:ButtonField>
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btnsave" Visible="False"
                                        __designer:wfdid="w39"></asp:Button>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnAddHoliday" EventName="Click"></asp:AsyncPostBackTrigger>
                                    <asp:AsyncPostBackTrigger ControlID="btnUpdate" EventName="Click"></asp:AsyncPostBackTrigger>
                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click"></asp:AsyncPostBackTrigger>
                                    <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click"></asp:AsyncPostBackTrigger>
                                    <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click"></asp:AsyncPostBackTrigger>
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
