<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmSlotDateandLocation.aspx.vb" Inherits="frmSlotDateandLocation"  %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="conSlotDateLocation" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/popcalendar.js"></script>

    <script type="text/javascript" src="Script/General.js"></script>

    <script type="text/javascript">
        function ShowDiv(e, nameDiv) {
            var ev = e || window.event
            var dv = document.getElementById(nameDiv);
            if (dv != null || dv != 'undefined') {
                var posY = e.clientY + document.body.scrollTop
			                    + document.documentElement.scrollTop;

                dv.style.display = 'block';
                dv.style.top = posY + 15;
                dv.focus();
                return false;
            }
        }

        function SwapValues() {
            var txtDestination = document.getElementById('<%= txtSchEndDt.ClientId  %>');
            var txtSource = document.getElementById('<%= txtSchStartDt.ClientId  %>');
            txtDestination.value = txtSource.value;
        }

        //Add by shivani pandya for project lock

        function getParameterByName(name) {
            name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                results = regex.exec(location.search);
            return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        }

        function getData() {
            var WorkspaceID = getParameterByName('workspaceid');
            $.ajax({
                type: "post",
                url: "frmProjectTalk.aspx/LockImpact",
                data: '{"WorkspaceID":"' + WorkspaceID + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                dataType: "json",
                success: function (data) {
                    if (data.d == "L") {
                        $("#ctl00_CPHLAMBDA_btnSave").attr("Disabled", "Disabled");
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

        
    </script>

    <table id="tblTopTable" runat="server" width="100%">
        <tr>
            <td>
                <table cellpadding="4px" width="100%">
                    <tr>
                        <td style="text-align: center" colspan="4">
                            <strong>Activity :</strong>
                            <asp:Label ID="lblActivity" runat="server" CssClass="Label " Font-Bold="True" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 20%;" class="Label">
                            Schedule Start Dt. :
                        </td>
                        <td style="text-align: left; width: 32%;">
                            <asp:TextBox ID="txtSchStartDt" runat="server" CssClass="textBox" Width="55%" />
                            <img id="img1" alt="Select  Date" src="images/calendar.gif" />
                            <cc1:CalendarExtender ID="Calendar1" PopupButtonID="img1" runat="server" TargetControlID="txtSchStartDt"
                                Format="dd-MMM-yyyy">
                            </cc1:CalendarExtender>

                            
                        </td>
                        <td class="Label" style="text-align: right;">
                            Schedule End Dt. :
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtSchEndDt" runat="server" onfocus="SwapValues();" CssClass="textBox" Width ="45%" />
                            <img id="img2" onclick="SwapValues();" alt="Select  Date" src="images/calendar.gif" />
                            <cc1:CalendarExtender ID="CalendarExtender1" PopupButtonID="img2" runat="server" TargetControlID="txtSchEndDt"
                                Format="dd-MMM-yyyy">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>
                    <%--  <tr>
                        <td align="right">
                           
                        </td>
                        <td align="left">
                            
                        </td>
                        <td>
                        </td>
                        <td align="left">
                        </td>
                    </tr>--%>
                    <tr>
                        <td style ="text-align :right ;" class="Label">
                            Select Location :
                        </td>
                        <td  style ="text-align :left ;">
                            <asp:DropDownList ID="ddlLocation" runat="server" AutoPostBack="True" CssClass="dropDownList" Width ="55%" />
                          
                        </td>
                     <td style ="text-align :right ;" class="Label">
                            Select Resource :
                        </td>
                          <td  style ="text-align :left ;">
                            <asp:DropDownList ID="ddlResource" runat="server" CssClass="dropDownList" Width ="45%" />
                         
                            <asp:Button ID="btnViewReport" runat="server" Text=" View Report " ToolTip ="View Report" 
                                         CssClass="btn btnnew" />
                        </td>
                    </tr>
                    <tr>
                        <td  style ="text-align :center ;" colspan ="4">
                            <asp:Button ID="btnSave" runat="server" Text=" Save " ToolTip ="Save" CssClass="btn btnsave" />
                            <asp:Button ID="BtnBack" runat="server" CssClass="btn btnback"  Text="" ToolTip ="Back" />
                        </td>
                    </tr>
                   
                </table>
            </td>
        </tr>
    </table>
    <table cellpadding="2px">
        <tr>
            <td>
                <asp:Button ID="btnClose" CssClass="btn btnclose" Visible="false" runat="server" Text="Close" ToolTip ="Close"
                    OnClientClick="window.close(); return false" />
            </td>
        </tr>
    </table>
    <table width="70%" style ="margin :auto;">
        <tr>
            <td>
                <asp:Calendar ID="cldSlotData" runat="server" Width="100%" NextPrevFormat="ShortMonth"
                    Height="250px" ForeColor="Black" Font-Size="9pt" Font-Names="Verdana" CellSpacing="1"
                    BorderStyle="Solid" BorderColor="Black" BackColor="White">
                    <TodayDayStyle BackColor="#999999" ForeColor="White" />
                    <DayStyle BackColor="#CCCCCC" />
                    <OtherMonthDayStyle ForeColor="#999999" />
                    <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="White" />
                    <DayHeaderStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" Height="8pt" />
                    <TitleStyle BackColor="#333399" Font-Bold="True" BorderStyle="Solid" Font-Size="12pt"
                        ForeColor="White" Height="12pt" />
                </asp:Calendar>
            </td>
        </tr>
    </table>
</asp:Content>
