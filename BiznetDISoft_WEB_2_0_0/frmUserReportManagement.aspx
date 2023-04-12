<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmUserReportManagement.aspx.vb" Inherits="frmUserReportManagement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <style type="text/css">
        #ctl00_CPHLAMBDA_GV_User_wrapper {
            width:60%
        }
        #ctl00_CPHLAMBDA_GVuserhistory_wrapper {
            width:100%;
        }
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
        }
        .ajax__calendar_container {
            z-index:999 !important;
        }
    </style>
    <asp:UpdatePanel ID="Up_General" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%" cellpadding="5px">
                <tbody>
                    <tr>
                        <td style="width: 40%; text-align: right;" class="Label">
                            User Status :
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList ID="ddlUserStatus" runat="server" CssClass="dropDownList" Width="30%"
                                AutoPostBack="true" TabIndex="1">
                                <asp:ListItem Selected="True">Active Sessions</asp:ListItem>
                                <asp:ListItem>User Login History</asp:ListItem>
                                <asp:ListItem>Active User</asp:ListItem>
                                <asp:ListItem>Inactive User</asp:ListItem>
                                <asp:ListItem>All User</asp:ListItem>
                                <asp:ListItem>Blocked User History</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="Label" style="text-align: right;">
                            <asp:Label ID="lblprofile" runat="server"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList ID="DDLUserType" runat="server" CssClass="dropDownList" Width="30%"
                                AutoPostBack="true" TabIndex="1">
                            </asp:DropDownList>
                        </td>
                    </tr>
                     <tr id="trUserName" runat="server">
                        <td style="width: 40%; text-align: right;" class="Label">
                            <asp:Label ID="LblUserName" Text="User Name : " runat="server"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList ID="DDlUserName" runat="server" CssClass="dropDownList" Width="30%"
                                AutoPostBack="true" TabIndex="1">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trDepartment" runat="server">
                        <td style="width: 40%; text-align: right;" class="Label">
                             <asp:Label ID="LblDepartment" Text="Department : " runat="server"></asp:Label> 
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList ID="DDLDepartment" runat="server" CssClass="dropDownList" Width="30%"
                                AutoPostBack="true" TabIndex="1">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trScope" runat="server">
                        <td style="width: 40%; text-align: right;" class="Label">
                           <asp:Label ID="LblScope" Text="Scope : " runat="server"/>
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList ID="DDLScope" runat="server" CssClass="dropDownList" Width="30%"
                                AutoPostBack="true" TabIndex="1">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trDate" runat="server">
                            <td colspan="2" style="text-align: center;">
                                <asp:Label runat="server" Text="From Date :*" ID="LblFromDateForuserhistory" Enabled="True"  class="Label"></asp:Label>
                                <asp:TextBox runat="server" ID="TxtFromDateOfuserhistory" Text="" CssClass="textBox"
                                    Enabled="True"> </asp:TextBox>
                                <cc1:CalendarExtender ID="CalExtFromDateForuserhistory" runat="server" TargetControlID="TxtFromDateOfuserhistory"
                                    Format="dd-MMM-yyyy" OnClientDateSelectionChanged="checkFromDate">
                                </cc1:CalendarExtender>
                                <asp:Label runat="server" Text="To Date :*" ID="LblToDateForuserhistory" Enabled="True"  class="Label"></asp:Label>
                                <asp:TextBox runat="server" ID="TxtToDateOfuserhistory" Text="" CssClass="textBox"
                                    Enabled="True"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalExtToDateForuserhistory" runat="server" TargetControlID="TxtToDateOfuserhistory"
                                    Format="dd-MMM-yyyy" OnClientDateSelectionChanged="checkToDate">
                                </cc1:CalendarExtender>
                                
                            </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: center;">
                            <asp:Button ID="BtnGO" runat="server" CssClass="btn btngo" Text="" Enabled="True" OnClientClick="return checkStatus();" Tooltip="Go"/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: center;">
                            <b>
                                <asp:Label ID="lbltitle" runat="server" Text="lbltitle" Visible="true"></asp:Label></b>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="Up_View" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="margin-left: 18%;width:100%">
                <asp:Panel runat="server" ID="pnlactivesession" ScrollBars="None" Visible="false" Width="80%">
                    <table style="width: 80%">
                        <tbody>
                            <tr>
                                <td>
                                        <asp:GridView ID="GVcntuser" runat="server" AutoGenerateColumns="False"
                                            PageSize="10" OnPageIndexChanging="GVcntuser_PageIndexChanging" BorderColor="Peru"
                                            Font-Size="Small" SkinID="grdViewAutoSizeMax" style="width:60%; margin:auto;">
                                            <Columns>
                                                <asp:BoundField DataField="vUserName" HeaderText="User Name" />
                                                <asp:BoundField DataField="vUsertypeName" HeaderText="User Profile" />
                                                <asp:BoundField DataField="tmp_dLoginDateTime" HeaderText="Login Time" DataFormatString="{0:dd-MMM-yyyy hh:mm tt}" />
                                                <asp:BoundField DataField="vIPAddress" HeaderText="IP Address" />
                                            </Columns>
                                        </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-top: 10px">
                                    <asp:Button ID="btnexclcntuser" runat="server" ToolTip="Export To Excel"
                                        CssClass="btn btnexcel" ></asp:Button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </asp:Panel>
            </div>

            <div style="margin-left: -10%;width:50px">
                <asp:Panel runat="server" ID="Pnlalluser" HorizontalAlign="Center" ScrollBars="None"
                    Visible="false" Width="50%">
                    <table align="center" width="50%">
                        <tbody>
                            <tr>
                                <td align="center" style="overflow: auto;">
                                    <asp:GridView ID="GV_User" runat="server" Width="100%" SkinID="grdViewAutoSizeMax" style="width:65%;" OnPageIndexChanging="GV_User_PageIndexChanging"
                                        PageSize="10" AutoGenerateColumns="False" OnRowCreated="GV_User_RowCreated">
                                         <%--AllowPaging="True"--%>
                                        <Columns>
                                            <asp:BoundField HeaderText="#" />
                                            <asp:BoundField DataField="iUserId" HeaderText="User Id" />
                                            <asp:BoundField DataField="vUserName" HeaderText="User Name" />
                                            <asp:BoundField DataField="vScopeName" HeaderText="Scope" />
                                            <asp:BoundField DataField="iUserGroupCode" HeaderText="User Group Code" />
                                            <asp:BoundField DataField="vUserGroupName" HeaderText="User Group Name" />
                                            <asp:BoundField DataField="vFirstName" HeaderText="First Name" />
                                            <asp:BoundField DataField="vLastName" HeaderText="Last Name" />
                                            <asp:BoundField DataField="vLoginName" HeaderText="Login Name" />
                                            <asp:BoundField DataField="vLoginPass" HeaderText="Login PassWord" />
                                            <asp:BoundField DataField="vUserTypeCode" HeaderText="User Type Code" />
                                            <asp:BoundField DataField="vUserTypeName" HeaderText="User Profile" />
                                            <asp:BoundField DataField="vDeptCode" HeaderText="Department Code" />
                                            <asp:BoundField DataField="vDeptName" HeaderText="Department Name" />
                                            <asp:BoundField DataField="vLocationCode" HeaderText="Location Code" />
                                            <asp:BoundField DataField="vLocationName" HeaderText="Location Name" />
                                            <asp:BoundField DataField="vEmailId" HeaderText="Email Id" />
                                            <asp:BoundField DataField="vPhoneNo" HeaderText="Phone No." />
                                            <asp:BoundField DataField="vExtNo" HeaderText="Ext No." />
                                            <asp:BoundField DataField="vRemark" HeaderText="Remark" />
                                            <asp:BoundField DataField="tmp_dModifyOn" HeaderText="Modify on" DataFormatString="{0:dd-MMM-yyyy hh:mm tt}" >
                                                    <ItemStyle Wrap="false" />
                                            </asp:BOundField>
                                            <asp:BoundField DataField="ModifierName" HeaderText="Modify By" />
                                            <%-- <asp:TemplateField HeaderText="Edit" SortExpression="status">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" Text="Edit" runat="server"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnexportexcel" runat="server" CssClass="btn btnexcel" Tooltip="Export To Excel"/>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </asp:Panel>
            </div>

            <div style="margin-left: 30%">
                <asp:Panel runat="server" ID="Pnluserhistory" HorizontalAlign="Center" ScrollBars="None"
                    Visible="false" Width="100%" Style="margin-left:-22%;">
                    <table width="100%">
                        <tbody>
                            <tr>
                                <td>
                                    <asp:GridView ID="GVuserhistory" runat="server" SkinID="grdViewAutoSizeMax" style="width:60%; margin:auto;" AutoGenerateColumns="False"
                                        OnPageIndexChanging="GVuserhistory_PageIndexChanging" BorderColor="Peru" Font-Size="Small"
                                        AllowPaging="false" PageSize="10">
                                        <Columns>
                                            <asp:BoundField DataField="vUserName" HeaderText="User Name" />
                                            <asp:BoundField DataField="vUsertypeName" HeaderText="User Profile" />
                                            <asp:BoundField DataField="cLOFlag" HeaderText="Status" />
                                            <asp:BoundField DataField="tmp_dInOutDateTime" HeaderText="Login/Logout Time" DataFormatString="{0:dd-MMM-yyyy hh:mm tt}" />
                                            
                                            <asp:BoundField DataField="vIPAddress" HeaderText="IP Address" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnexptuserhistory" runat="server"  ToolTip ="Export To Excel" CssClass="btn btnexcel" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </asp:Panel>
            </div>

            <div style="width: 100%;">
                <asp:Panel runat="server" ID="Pnluserfailure" HorizontalAlign="Center" ScrollBars="None"
                    Visible="false" Width="100%">
                    <table align="center">
                        <tbody>
                            <tr>
                                <td style="overflow: auto;">
                                    <asp:GridView ID="GVuserfailure" runat="server" AutoGenerateColumns="False" OnPageIndexChanging="GVuserfailure_PageIndexChanging"
                                        BorderColor="Peru" Font-Size="Small" SkinID="grdViewAutoSizeMax" style="width:650px; margin:auto;" AllowPaging="false" PageSize="10"
                                        ShowHeader="true">
                                        <Columns>
                                            <asp:BoundField DataField="vUserLoginName" HeaderText="User Name" ItemStyle-Width="150px" />
                                            <asp:BoundField DataField="dLastFailedLogin" HeaderText="Login Failed Time" ItemStyle-Width="226px"
                                                DataFormatString="{0:dd-MMM-yyyy hh:mm tt}" />
                                            <asp:BoundField DataField="vIPAddress" HeaderText="IP Address" ItemStyle-Width="115px" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnexptuserfailure" runat="server"  ToolTip="Export To Excel"
                                        CssClass="btn btnexcel"  />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </asp:Panel>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlUserStatus" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="DDLUserType" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="DDLUserName" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="DDLDepartment" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="DDLScope" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="btnGO"  EventName="Click"/>
            <asp:PostBackTrigger ControlID="btnexclcntuser" />
            <asp:PostBackTrigger ControlID="btnexportexcel" />
            <asp:PostBackTrigger ControlID="btnexptuserhistory" />
            <asp:PostBackTrigger ControlID="btnexptuserfailure" />
        </Triggers>
    </asp:UpdatePanel>

    <script type="text/javascript">
        Sys.Browser.WebKit = {};
        if (navigator.userAgent.indexOf('WebKit/') > -1) {
            Sys.Browser.agent = Sys.Browser.WebKit;
            Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
            Sys.Browser.name = 'WebKit';
        }
    </script>

    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>

    <script type="text/javascript">
        function checkFromDate(sender,args) {
            var startdate = document.getElementById('<%=TxtFromDateOfuserhistory.ClientID%>').value
            var enddate = document.getElementById('<%=TxtToDateOfuserhistory.ClientID%>').value
            
            if (Date.parse(startdate) > Date.parse(enddate)) {
                document.getElementById('<%=TxtFromDateOfuserhistory.ClientID%>').value = "";
                msgalert("From date must be less than To Date !");
            }
        }

        function checkToDate(sender, args) {
            var startdate = document.getElementById('<%=TxtFromDateOfuserhistory.ClientID%>').value
            var enddate = document.getElementById('<%=TxtToDateOfuserhistory.ClientID%>').value

            if (Date.parse(startdate) > Date.parse(enddate)) {
                document.getElementById('<%=TxtToDateOfuserhistory.ClientID%>').value = "";
                msgalert("To date must be greater than From Date !");
            }
        }

        function checkStatus() {
            var e = document.getElementById('<%=ddlUserStatus.ClientID%>');
            var status = e.options[e.selectedIndex].text;
           
            if (status == "User Login History") {

                if (document.getElementById('<%=TxtFromDateOfuserhistory.ClientID%>').value == "") {
                    msgalert("Please Select From Date !");
                    return false;
                }

                if (document.getElementById('<%=TxtToDateOfuserhistory.ClientID%>').value == "") {
                    msgalert("Please Select To Date !");
                    return false;
                }
            }
        }

        function UIgvmedexRefresh() {
            $('#<%= GVcntuser.ClientID%>').removeAttr('style', 'display:block');
            oTab = $('#<%= GVcntuser.ClientID%>').prepend($('<thead>').append($('#<%= GVcntuser.ClientID%> tr:first'))).dataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": false,
                "sScrollY": "250px",
                "sScrollX": "100%",
                "bScrollCollapse": true,
                aLengthMenu: [
                    [10, 25, 50, 100, -1],
                    [10, 25, 50, 100, "All"]
                ],
            });
            return false;
        }

        function UIGVUser() {
            $('#<%= GV_User.ClientID%>').removeAttr('style', 'display:block');
            oTab = $('#<%= GV_User.ClientID%>').prepend($('<thead>').append($('#<%= GV_User.ClientID%> tr:first'))).dataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": false,
                "sScrollY": "250px",
                "sScrollX": "100%",
                "bScrollCollapse": true,
                aLengthMenu: [
                    [10, 25, 50, 100, -1],
                    [10, 25, 50, 100, "All"]
                ],
            });
            return false;
        }

        function GVuserhistory() {
            $('#<%= GVuserhistory.ClientID%>').removeAttr('style', 'display:block');
            oTab = $('#<%= GVuserhistory.ClientID%>').prepend($('<thead>').append($('#<%= GVuserhistory.ClientID%> tr:first'))).dataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": false,
                "sScrollY": "250px",
                "sScrollX": "100%",
                "bScrollCollapse": true,
                aLengthMenu: [
                    [10, 25, 50, 100, -1],
                    [10, 25, 50, 100, "All"]
                ],
            });
            return false;
        }

        function GVuserfailure() {
            $('#<%= GVuserfailure.ClientID%>').removeAttr('style', 'display:block');
            oTab = $('#<%= GVuserfailure.ClientID%>').prepend($('<thead>').append($('#<%= GVuserfailure.ClientID%> tr:first'))).dataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": false,
                "sScrollY": "250px",
                "sScrollX": "100%",
                "bScrollCollapse": true,
                aLengthMenu: [
                    [10, 25, 50, 100, -1],
                    [10, 25, 50, 100, "All"]
                ],
            });
            return false;
        }
        

    </script>

</asp:Content>
