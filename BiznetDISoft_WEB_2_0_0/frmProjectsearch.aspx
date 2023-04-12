<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmProjectsearch.aspx.vb" Inherits="frmProjectsearch"  %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" language="javascript" src="Script/popcalendar.js"></script>

    <script type="text/javascript" language="javascript" src="Script/Validation.js"></script>

    <script type="text/javascript" language="javascript">

        function Validation() {

            if (document.getElementById('<%=DDLProStatus.clientid %>').selectedIndex == 0) {
                msgalert('Please Select Project Status !');
                return false;
            }

            return true;
        }
        function Valid() {
            if (document.getElementById('<%=chkAllDate.clientid %>').checked == false) {

                if (document.getElementById('<%=txtFromDate.ClientID%>').value.toString().trim().length <= 0 || document.getElementById('<%=txtToDate.ClientID%>').value.toString().trim().length <= 0) {
                    msgalert('Please Enter Date Or Select ALL !');
                    document.getElementById('<%=txtFromDate.ClientID%>').focus();
                    return false;
                }
                else if (CompareDate(document.getElementById('<%=txtFromDate.ClientID%>').value, document.getElementById('<%=txtToDate.ClientID%>').value) == false) {
                    msgalert('Fromdate Must Be Smaller Then Todate ! ');
                    return false;
                }
            }
            return true;
        }




        function popcalender(obj) {
            var txt = document.getElementById('<%=txtFromDate.clientid %>');
            popUpCalendar(obj, txt, 'dd-mmm-yy');
            document.getElementById('<%=chkAllDate.clientid %>').checked = false;
        }
    </script>
    <style type="text/css">
        .InnerTable {
            width:1320px !important;
            margin:0px 10px !important;
        }
    </style>

    <%--<meta http-equiv="X-UA-Compatible" content="IE=7">--%>
    <table style="width: 100%;" cellpadding="5px">
        <tr>
            <td style="width: 40%; text-align: right;" class="Label">
                Project Type  :
            </td>
            <td style="text-align: left">
                <asp:DropDownList ID="DDLProType" runat="server" CssClass="dropDownList" Width="30%"
                    AutoPostBack="True" />
            </td>
        </tr>
        <tr>
            <td style="text-align: right;" class="Label">
                Project Status  :
            </td>
            <td style="text-align: left">
                <asp:DropDownList ID="DDLProStatus" runat="server" CssClass="dropDownList" Width="30%"
                    AutoPostBack="True" />
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="BtnGo1" runat="server" Text="" ToolTip="Go" CssClass="btn btngo"
                            OnClientClick="return Validation();" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="DDLProType" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="DDLProStatus" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="BtnGo" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="Up_Go1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="DivSpDrug" runat="server" style="width: 100%;">
                <table style="width: 100%" cellpadding ="5px">
                    <tbody>
                        <tr>
                            <td style="width: 40%; text-align: right;" class="Label">
                                Sponsor :
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="DDLSponsor" runat="server" CssClass="dropDownList" Width="30%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right" class="Label">
                                Drug :
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="DDLDrug" runat="server" CssClass="dropDownList" Width="30%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right" class="Label">
                                Segment :
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="DDLSegment" runat="server" CssClass="dropDownList" Width="15%"
                                    AutoPostBack="True">
                                    <asp:ListItem Value="0">Select Segment</asp:ListItem>
                                    <asp:ListItem Value="1">IND</asp:ListItem>
                                    <asp:ListItem Value="2">POL</asp:ListItem>
                                    <asp:ListItem Value="3">CAN</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right" class="Label">
                                Location :
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="DDLLocation" runat="server" CssClass="dropDownList" Width="30%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right" class="Label">
                                <strong class="Label">Select Date : </strong>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="textBox" Width="15%" />
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtfromDate"  Format="dd-MMM-yyyy"></cc1:CalendarExtender>
                                To :
                                <asp:TextBox ID="txtToDate" TabIndex="1" runat="server" CssClass="textBox" Width="15%" />
                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"  Format="dd-MMM-yyyy"></cc1:CalendarExtender>
                                <asp:CheckBox runat="server" Text="ALL" ID="chkAllDate" Checked="true" />
                            </td>
                        </tr>
                    </tbody>
                </table>
                <table width="100%" style="text-align: center;">
                    <tbody>
                        <tr>
                            <td>
                                <asp:Button ID="BtnExportExcel" runat="server" Text="Export to Excel" CssClass="brn btnexcel"
                                    Width="14%" Visible="False" />
                                <asp:Button ID="BtnGo" runat="server" Text="" ToolTip="Go" CssClass="btn btngo" 
                                    OnClientClick="return Valid();" />
                                <asp:Button ID="BtnBack" OnClick="BtnBack_Click" runat="server" Text="" CssClass="btn btnback"
                                    ToolTip="Back"  />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnGo1" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="DDLProType" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="DDLProStatus" EventName="SelectedIndexChanged" />
            <asp:PostBackTrigger ControlID="BtnExportExcel" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="Up_Go2" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%" style="overflow:auto; display:block;">
                <tbody>
                    <tr>
                        <td colspan="2" style ="width :70%; text-align :center ;">
                            <asp:GridView ID="GV_Project" runat="server" Width="100%" CssClass="gvwProjects" SkinID="grdViewAutoSizeMax"
                                ShowFooter="True" CellPadding="3" OnPageIndexChanging="GV_Project_PageIndexChanging"
                                AllowPaging="True" PageSize="25" AutoGenerateColumns="False" OnRowCommand="GV_Project_RowCommand"
                                OnRowDataBound="GV_Project_RowDataBound" OnRowCreated="GV_Project_RowCreated">
                                <Columns>
                                    <asp:BoundField DataField="vProjectTypeCode" HeaderText="Project Type Code"></asp:BoundField>
                                    <asp:BoundField DataField="vProjectNo" HeaderText="Project No."></asp:BoundField>
                                    <asp:BoundField DataField="vRequestId" HeaderText="Project Request ID"></asp:BoundField>
                                    <asp:TemplateField HeaderText="Project">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProjectName" runat="server" Text='<%# Eval("vWorkspaceDesc") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="vClientName" HeaderText="Sponsor"></asp:BoundField>
                                    <asp:BoundField DataField="vDrugName" HeaderText="Drug"></asp:BoundField>
                                    <asp:BoundField DataField="vBrandName" HeaderText="Brand"></asp:BoundField>
                                    <asp:BoundField DataField="vProjectManager" HeaderText="Project Manager"></asp:BoundField>
                                    <asp:BoundField DataField="vProjectCoordinator" HeaderText="Project Coordinator">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="iNoOfSubjects" HeaderText="No. of Subjects"></asp:BoundField>
                                    <asp:BoundField DataField="nRetaintionPeriod" HeaderText="RetentionPeriod"></asp:BoundField>
                                    <asp:BoundField DataField="cFastingFed" HeaderText="Fastfed"></asp:BoundField>
                                    <asp:BoundField DataField="vRegionName" HeaderText="Submission"></asp:BoundField>
                                    <asp:BoundField DataField="cProjectStatus" HeaderText="Status"></asp:BoundField>
                                    <asp:BoundField DataField="Check-In(1) Start" HeaderText="Checkin(Period1)"></asp:BoundField>
                                    <asp:BoundField DataField="IP Administration(1) Start" HeaderText="Dosing(Period1)">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Check-In(2) Start" HeaderText="CheckIn(Period2)"></asp:BoundField>
                                    <asp:BoundField DataField="IP Administration(2) Start" HeaderText="Dosing(Period2)">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Check-In(3) Start" HeaderText="CheckIn(Period3)"></asp:BoundField>
                                    <asp:BoundField DataField="IP Administration(3) Start" HeaderText="Dosing(Period3)">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Check-In(4) Start" HeaderText="Checkin(Period4)"></asp:BoundField>
                                    <asp:BoundField DataField="IP Administration(4) Start" HeaderText="Dosing(Period4)">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Check-In(5) Start" HeaderText="Checkin(Period5)"></asp:BoundField>
                                    <asp:BoundField DataField="IP Administration(5) Start" HeaderText="Dosing(Period5)">
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Detail">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LkbDetail" runat="server">Detail</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="vWorkspaceId" HeaderText="WorkSpaceId"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnGo" EventName="Click"></asp:AsyncPostBackTrigger>
            <asp:AsyncPostBackTrigger ControlID="DDLProType" EventName="SelectedIndexChanged">
            </asp:AsyncPostBackTrigger>
            <asp:AsyncPostBackTrigger ControlID="DDLProStatus" EventName="SelectedIndexChanged">
            </asp:AsyncPostBackTrigger>
            <asp:PostBackTrigger ControlID="BtnExportExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
