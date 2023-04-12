<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmDWR, App_Web_2mzu20n4" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script language="javascript" src="Script/Validation.js">
    </script>

    <script language="javascript" src="Script/popcalendar.js">
    </script>

    <script type="text/javascript" language="javascript">
function showdiv(vType)
{
 if (vType=='Field')
                    {
                                               
                            divField.style.display = 'block';
                            divOther.style.display = 'none';
                            var btn1 = document.getElementById('Button2');
                            btn1.style.backgroundColor='white';
                            btn1.style.color='#336699';
                              var btn1 = document.getElementById('Button1');
                            btn1.style.backgroundColor='#336699';
                            btn1.style.color='white';
                    }
 if (vType=='Other')
                    {
                                                  
                                   
                            divOther.style.display = 'block';
                            divField.style.display = 'none';
                            var btn1 = document.getElementById('Button1');
                            btn1.style.backgroundColor='white';
                            btn1.style.color='#336699';
                            var btn1 = document.getElementById('Button2');
                            btn1.style.backgroundColor='#336699';
                            btn1.style.color='white';
                                                       
                    }
}

    function ValidationForProjectWork()
    {
        if (document.getElementById('<%=ddlSTP.clientid %>').selectedIndex == 0)
        {
            msgalert('Select Site !');
            return false;            
        }
        else if (document.getElementById('<%=ddlActivityGroup.clientid %>').selectedIndex == 0)
        {
            msgalert('Select Activity Group !');
            return false;            
        }
        else if (document.getElementById('<%=ddlActivity.clientid %>').selectedIndex == 0)
        {
            msgalert('Select Activity !');
            return false;            
        }
        else if (document.getElementById('<%=txtfromTime_Un.clientid %>').value == '')
        {
            msgalert('Enter From Time !');
            return false;            
        }
        else if (document.getElementById('<%=txtToTime_Un.clientid %>').value == '')
        {
            msgalert('Enter To Time !');
            return false;            
        }
        return true;
    }
    function ValidationForOtherWork()
    {
        if (document.getElementById('<%=ddlSTP.clientid %>').selectedIndex == 0)
        {
            msgalert('Select Site !');
            return false;            
        }
        else if (document.getElementById('<%=ddlReason.clientid %>').selectedIndex == 0)
        {
            msgalert('Select Reason !');
            return false;            
        }
        else if (document.getElementById('<%=txtfromTime_OW.clientid %>').value == '')
        {
            msgalert('Enter From Time !');
            return false;            
        }
        else if (document.getElementById('<%=txtToTime_OW.clientid %>').value == '')
        {
            msgalert('Enter To Time !');
            return false;            
        }
        return true;    
    }

    </script>

    <table>
        <tr>
            <td style="width: 100%" align="center">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table style="vertical-align: top" class="TDMandatory">
                            <tbody>
                                <tr>
                                    <td class="Label">
                                        Report Date :
                                    </td>
                                    <td style="vertical-align: top">
                                        <%--    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>--%><asp:TextBox ID="txtRepDate" runat="server" CssClass="textBox"
                                Width="171px" __designer:wfdid="w97" Enabled="False"></asp:TextBox>
                                        <%--<img id="imgToDate" alt="Select  Date" src="images/Calendar_scheduleHS.png" />--%><img
                                            id="imgToDate" onclick="popUpCalendar(this,ctl00_CPHLAMBDA_txtRepDate,'dd-mmm-yyyy');"
                                            alt="Select  Date" src="images/Calendar_scheduleHS.png" />
                                        <%--  <cc1:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="MyCalendar"
                                    Format="dd-MMM-yyyy" PopupButtonID="imgToDate" TargetControlID="txtRepdate">
                                </cc1:CalendarExtender>--%><%--</ContentTemplate>
                        </asp:UpdatePanel>--%>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnGO" runat="server" Text="" CssClass="btn btngo"  __designer:wfdid="w98">
                                        </asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline"
                                            __designer:wfdid="w99">
                                            <ContentTemplate>
                                                <asp:Calendar ID="EventCalendar" runat="server" Font-Size="Small" Font-Names="Verdana"
                                                    Width="79%" SelectMonthText="" PrevMonthText="<<" OnVisibleMonthChanged="EventCalendar_VisibleMonthChanged"
                                                    NextMonthText=">>" CellSpacing="1" __designer:wfdid="w100">
                                                    <WeekendDayStyle ForeColor="Black" />
                                                    <OtherMonthDayStyle ForeColor="LightGray" />
                                                    <DayStyle BorderColor="SteelBlue" BorderStyle="Solid" BorderWidth="1px" Font-Size="XX-Small" />
                                                    <DayHeaderStyle Font-Bold="False" Font-Names="Verdana" Font-Size="X-Small" />
                                                    <TitleStyle BackColor="#F0F0F0" Font-Bold="True" Font-Size="Smaller" />
                                                </asp:Calendar>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnGO" EventName="Click"></asp:AsyncPostBackTrigger>
                                                <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click"></asp:AsyncPostBackTrigger>
                                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click"></asp:AsyncPostBackTrigger>
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGO" EventName="Click"></asp:AsyncPostBackTrigger>
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td style="width: 100%" align="left">
                <asp:UpdatePanel ID="up_PlannedDtl" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table style="vertical-align: top" class="InnerTable">
                            <tbody>
                                <tr>
                                    <td valign="top" align="left" width="100%">
                                        <strong>Planed Work</strong>
                                        <hr style="background-image: none; width: 100%; color: #ffcc66; background-color: #ffcc66" />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="center" width="100%">
                                        <asp:GridView ID="GV_Planned" runat="server" Width="100%" SkinID="grdViewAutoSizeMax" AllowPaging="True"
                                            OnRowCreated="GV_Planned_RowCreated" AutoGenerateColumns="False" OnPageIndexChanging="GV_Planned_PageIndexChanging">
                                            <Columns>
                                                <asp:BoundField DataField="nMTPNo" HeaderText="MTP No"></asp:BoundField>
                                                <asp:BoundField DataField="nSTPNo" HeaderText="STP No"></asp:BoundField>
                                                <asp:BoundField DataField="nCityNo" HeaderText="CityNo"></asp:BoundField>
                                                <asp:BoundField DataField="vActivityId" HeaderText="Activity Id"></asp:BoundField>
                                                <asp:BoundField DataField="vWorkspaceDesc" HeaderText="Project"></asp:BoundField>
                                                <asp:BoundField DataField="vSitename" HeaderText="Site Name"></asp:BoundField>
                                                <asp:BoundField DataField="vCityName" HeaderText="City Name"></asp:BoundField>
                                                <asp:BoundField DataField="vActivityName" HeaderText="Activity"></asp:BoundField>
                                                <asp:BoundField DataField="vRemark" HeaderText="Remarks"></asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="btnGO" EventName="Click"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="GV_DWRDetail" EventName="RowDeleting"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="EventCalendar" EventName="SelectionChanged">
                        </asp:AsyncPostBackTrigger>
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td style="width: 100%" align="left">
                <asp:UpdatePanel ID="up_PnlUnplaned" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="PanelUnMslDtl" runat="server" Visible="false">
                            <%--Naimesh Dave --%><div style="display: block" id="divComman">
                                <table style="vertical-align: top" class="InnerTable">
                                    <tbody>
                                        <tr align="left">
                                            <td align="left" colspan="2">
                                                <strong>Actual Work
                                                    <hr style="background-image: none; width: 100%; color: #ffcc66; background-color: #ffcc66" />
                                                </strong>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td align="left" colspan="2">
                                                <table style="vertical-align: top" class="InnerTable" width="100%">
                                                    <tbody>
                                                        <tr>
                                                            <td style="width: 30%" class="Label" align="left">
                                                                Site Name:
                                                            </td>
                                                            <td style="width: 70%" align="left">
                                                                <asp:DropDownList Style="width: 445px" ID="ddlSTP" runat="server" CssClass="dropDownList"
                                                                    __designer:wfdid="w85">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <table id="tblUnplanned" class="InnerClass">
                                <tbody>
                                    <tr>
                                        <td>
                                            <input id="Button1" class="TABButton" onclick="showdiv('Field')" type="button" value="PROJECT WORK" />
                                        </td>
                                        <td>
                                            <input id="Button2" class="ShowButton" onclick="showdiv('Other')" type="button" value="OTHER WORK" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <div style="display: block" id="divField" class="divDesign">
                                <asp:UpdatePanel ID="up_ProjectWork" runat="server" __designer:dtid="281474976710667"
                                    UpdateMode="Conditional" RenderMode="Inline" __designer:wfdid="w51">
                                    <ContentTemplate __designer:dtid="281474976710668">
                                        <table style="vertical-align: top" class="InnerTable" width="100%">
                                            <tbody>
                                                <tr>
                                                    <td align="left" colspan="2">
                                                        <asp:RadioButtonList ID="rblWorktype" runat="server" Font-Size="X-Small" Font-Bold="True"
                                                            __designer:wfdid="w83" RepeatDirection="Horizontal">
                                                            <asp:ListItem Selected="True" Value="1">OnSite</asp:ListItem>
                                                            <asp:ListItem Value="3">Office</asp:ListItem>
                                                            <asp:ListItem Value="4">Travel</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%" class="Label" align="left">
                                                        Time:
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtfromTime_Un" runat="server" Width="80px" __designer:wfdid="w56"></asp:TextBox><cc1:MaskedEditValidator
                                                            ID="MaskedEditValidator1" runat="server" __designer:wfdid="w57" ToolTip="Enter valid Time"
                                                            SetFocusOnError="True" InvalidValueBlurredMessage="Invalid Time" EmptyValueMessage="Time is required"
                                                            Display="Dynamic" ControlToValidate="txtfromTime_Un" ControlExtender="MaskedEditExtender1">*</cc1:MaskedEditValidator>
                                                        To
                                                        <asp:TextBox ID="txtToTime_Un" runat="server" Width="80px" __designer:wfdid="w58"></asp:TextBox><cc1:MaskedEditValidator
                                                            ID="MaskedEditValidator2" runat="server" __designer:wfdid="w59" ToolTip="Enter valid Time (HH:MM)"
                                                            SetFocusOnError="True" InvalidValueBlurredMessage="Invalid Time" EmptyValueMessage="Time is required"
                                                            Display="Dynamic" ControlToValidate="txtToTime_Un" ControlExtender="MaskedEditExtender2">*</cc1:MaskedEditValidator>
                                                        <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" __designer:wfdid="w60"
                                                            TargetControlID="txtfromTime_Un" MaskType="Time" Mask="99:99" ErrorTooltipEnabled="True"
                                                            ClearTextOnInvalid="True" Century="2000" AutoComplete="False">
                                                        </cc1:MaskedEditExtender>
                                                        <cc1:MaskedEditExtender ID="MaskedEditExtender2" runat="server" __designer:wfdid="w61"
                                                            TargetControlID="txtToTime_Un" MaskType="Time" Mask="99:99" ErrorTooltipEnabled="True"
                                                            ClearTextOnInvalid="True" Century="2000" AutoComplete="False">
                                                        </cc1:MaskedEditExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%" class="Label" align="left">
                                                        Activity Group:
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList Style="width: 445px" ID="ddlActivityGroup" runat="server" CssClass="dropDownList"
                                                            __designer:wfdid="w53" OnSelectedIndexChanged="ddlActivityGroup_SelectedIndexChanged"
                                                            AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%" class="Label" align="left">
                                                        Activity:
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList Style="width: 445px" ID="ddlActivity" runat="server" CssClass="dropDownList"
                                                            __designer:wfdid="w54">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%" class="Label" align="left">
                                                        Objective:
                                                    </td>
                                                    <td align="left">
                                                        <input style="width: 440px" id="txtRemark" class="TextBox" type="text" name="txtObjective"
                                                            runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%" align="left">
                                                    </td>
                                                    <td align="left">
                                                        <asp:Button ID="BtnProjectSave" OnClick="BtnProjectSave_Click" runat="server" Text="Add"
                                                            CssClass="btn btnsave" __designer:wfdid="w55" CausesValidation="False" OnClientClick="return ValidationForProjectWork();">
                                                        </asp:Button>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <br />
                                &nbsp;</div>
                            <div style="display: none" id="divOther" class="divDesign">
                                <asp:UpdatePanel ID="up_OtherWork" runat="server" __designer:dtid="281474976710667"
                                    UpdateMode="Conditional" RenderMode="Inline" __designer:wfdid="w63">
                                    <ContentTemplate __designer:dtid="281474976710668">
                                        <table style="vertical-align: top" class="InnerTable" width="100%">
                                            <tbody>
                                                <tr align="left">
                                                    <td style="width: 30%" class="Label" align="left">
                                                        Reason:
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList ID="ddlReason" runat="server" CssClass="dropDownList" __designer:wfdid="w74">
                                                            <asp:ListItem>Select Reason</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%" class="Label" align="left">
                                                        Time:
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtfromTime_OW" runat="server" Width="80px" __designer:wfdid="w75"></asp:TextBox><cc1:MaskedEditValidator
                                                            ID="MaskedEditValidator1_OW" runat="server" __designer:wfdid="w76" ToolTip="Enter valid Time"
                                                            SetFocusOnError="True" InvalidValueBlurredMessage="Invalid Time" EmptyValueMessage="Time is required"
                                                            Display="Dynamic" ControlToValidate="txtfromTime_OW" ControlExtender="MaskedEditExtender1_OW"
                                                            ErrorMessage="MaskedEditValidator1_OW">*</cc1:MaskedEditValidator>
                                                        To
                                                        <asp:TextBox ID="txtToTime_OW" runat="server" Width="80px" __designer:wfdid="w77"></asp:TextBox><cc1:MaskedEditValidator
                                                            ID="MaskedEditValidator2_OW" runat="server" __designer:wfdid="w78" ToolTip="Enter valid Time (HH:MM)"
                                                            SetFocusOnError="True" InvalidValueBlurredMessage="Invalid Time" EmptyValueMessage="Time is required"
                                                            Display="Dynamic" ControlToValidate="txtToTime_OW" ControlExtender="MaskedEditExtender2_OW"
                                                            ErrorMessage="MaskedEditValidator2_OW">*</cc1:MaskedEditValidator><cc1:MaskedEditExtender
                                                                ID="MaskedEditExtender1_OW" runat="server" __designer:wfdid="w79" TargetControlID="txtfromTime_OW"
                                                                MaskType="Time" Mask="99:99" ErrorTooltipEnabled="True" ClearTextOnInvalid="True"
                                                                Century="2000" AutoComplete="False">
                                                            </cc1:MaskedEditExtender>
                                                        <cc1:MaskedEditExtender ID="MaskedEditExtender2_OW" runat="server" __designer:wfdid="w80"
                                                            TargetControlID="txtToTime_OW" MaskType="Time" Mask="99:99" ErrorTooltipEnabled="True"
                                                            ClearTextOnInvalid="True" Century="2000" AutoComplete="False">
                                                        </cc1:MaskedEditExtender>
                                                    </td>
                                                </tr>
                                                <tr align="left">
                                                    <td style="width: 30%" class="Label" align="left">
                                                        Remarks:
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="TxtOtherRemarks" runat="server" CssClass="textBox" Width="194px"
                                                            __designer:wfdid="w81" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr align="left">
                                                    <td style="width: 30%" align="left">
                                                    </td>
                                                    <td align="left">
                                                        <asp:Button ID="btnOtherSave" runat="server" Text="Add" CssClass="btn btnsave" __designer:wfdid="w82"
                                                            CausesValidation="False" OnClientClick="return ValidationForOtherWork();"></asp:Button>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers __designer:dtid="281474976710669">
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click"></asp:AsyncPostBackTrigger>
                                        <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click"></asp:AsyncPostBackTrigger>
                                        <asp:AsyncPostBackTrigger ControlID="btnGO" EventName="Click"></asp:AsyncPostBackTrigger>
                                        <asp:AsyncPostBackTrigger ControlID="GV_DWRDetail" EventName="RowDeleting"></asp:AsyncPostBackTrigger>
                                        <asp:AsyncPostBackTrigger ControlID="EventCalendar" EventName="SelectionChanged">
                                        </asp:AsyncPostBackTrigger>
                                    </Triggers>
                                </asp:UpdatePanel>
                                <br />
                                &nbsp;</div>
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGO" EventName="Click"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="EventCalendar" EventName="SelectionChanged">
                        </asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click"></asp:AsyncPostBackTrigger>
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td style="width: 100%" align="left">
                <br />
                <asp:UpdatePanel ID="Up_DWRDETail" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label Style="min-height: 12px; align: center" ID="LblFinal" runat="server" ForeColor="Navy"
                            Font-Bold="True" Text="F I N AL V I E W" Width="100%" BackColor="White" Visible="False"
                            __designer:wfdid="w103"></asp:Label><br />
                        <hr style="background-image: none; width: 100%; color: #ffcc66; background-color: #ffcc66" />
                        <asp:GridView ID="GV_DWRDetail" runat="server" CssClass="grdView" Width="100%" SkinID="grdView"
                            __designer:wfdid="w104" AutoGenerateColumns="False" OnRowCreated="GV_DWRDetail_RowCreated">
                            <Columns>
                                <asp:BoundField DataField="nDWRHdrNo" HeaderText="DWRHdrNo"></asp:BoundField>
                                <asp:BoundField DataField="nDWRDtlNo" HeaderText="DWRDtlNo"></asp:BoundField>
                                <asp:BoundField DataField="vActivityId" HeaderText="ActivityId"></asp:BoundField>
                                <asp:BoundField DataField="vWorkspaceDesc" HeaderText="Project"></asp:BoundField>
                                <asp:BoundField DataField="vSiteName" HeaderText="Site name"></asp:BoundField>
                                <asp:BoundField DataField="vCityName" HeaderText="Visited City"></asp:BoundField>
                                <asp:BoundField DataField="vWorkType" HeaderText="Work Type"></asp:BoundField>
                                <asp:BoundField DataField="vActivityDesc" HeaderText="Activity"></asp:BoundField>
                                <asp:BoundField DataField="dFromTime" HeaderText="From Time"></asp:BoundField>
                                <asp:BoundField DataField="dToTime" HeaderText="To Time"></asp:BoundField>
                                <asp:BoundField DataField="vReasonDesc" HeaderText="Reason"></asp:BoundField>
                                <asp:BoundField DataField="vRemark" HeaderText="Remark"></asp:BoundField>
                                <asp:CommandField ShowDeleteButton="True"></asp:CommandField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="GV_Planned" EventName="RowCommand"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="btnOtherSave" EventName="Click"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="BtnProjectSave" EventName="Click"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="EventCalendar" EventName="SelectionChanged">
                        </asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="btnGO" EventName="Click"></asp:AsyncPostBackTrigger>
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td style="width: 100%; white-space: nowrap;" align="center">
                <asp:UpdatePanel ID="UPClear" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Save" CssClass="btn btnsave"
                            __designer:wfdid="w10" CausesValidation="False"></asp:Button>
                        <asp:Button ID="btnClear" OnClick="btnClear_Click" runat="server" Text="Cancel" CssClass="btn btnnew"
                            __designer:wfdid="w11" CausesValidation="False"></asp:Button>
                        <asp:Button ID="btnExit" OnClick="btnExit_Click" runat="server" Text="Exit" CssClass="btn btnexit"
                            __designer:wfdid="w12" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);"
                            CausesValidation="False"></asp:Button>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGO" EventName="Click"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="BtnProjectSave" EventName="Click"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="btnOtherSave" EventName="Click"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="GV_DWRDetail" EventName="RowDeleting"></asp:AsyncPostBackTrigger>
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    &nbsp; &nbsp; &nbsp;
    <%--</asp:MultiView> 
</ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
