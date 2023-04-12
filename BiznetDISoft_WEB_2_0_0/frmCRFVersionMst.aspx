<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmCRFVersionMst.aspx.vb" Inherits="frmCRFVersionMst"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <asp:UpdatePanel ID="upPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%" cellpadding="4px">
                <tr>
                    <td style="width: 30%; text-align: right;" class="Label ">
                        Project Name* :
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtProject" runat="server" CssClass="textBox" Width="70%"></asp:TextBox>
                        <asp:HiddenField ID="HProjectId" runat="server" />
                        <asp:HiddenField ID="HVersionNo" runat="server" />
                        <asp:HiddenField ID="HFreezeStatus" runat="server" />
                        <asp:HiddenField ID="HFSponsor" runat="server" />
                         <asp:HiddenField ID="hndLockStatus" runat="server" />
                        
                        <asp:Button Style="display: none" ID="btnSetProject" OnClientClick="getData();" runat="server" Text=" Project">
                        </asp:Button>
                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                            CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                            CompletionListElementID="pnlProjectList" CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                            OnClientShowing="ClientPopulated" ServiceMethod="GetCrfVersionProjectList" ServicePath="AutoComplete.asmx"
                            TargetControlID="txtProject" UseContextKey="True">
                        </cc1:AutoCompleteExtender>
                        <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 300px; overflow: auto;
                            overflow-x: hidden" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td style="text-align: left;">
                        <asp:RadioButtonList ID="RblFreeze" runat="server" RepeatDirection="Horizontal" CellPadding="5">
                            <asp:ListItem Text="Freeze" Value="F" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="UnFreeze" Value="U"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td class="Label" style="white-space: nowrap; text-align: right;">
                        Remarks :
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtRemark" Width="40%" runat="server" TextMode="MultiLine" CssClass="textBox"
                            MaxLength="500"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <%-- <td class="Label" style="width: 35%; white-space: nowrap;" align="right">
                    </td>--%>
                    <td class="Label" style="text-align: center;" colspan="2">
                        <asp:Button ID="btnSave" Text="Save" ToolTip="Save" runat="server" CssClass="btn btnsave"
                            OnClientClick="return funChk_Remark(this);" />
                        <asp:Button ID="btnCancel" Text="Cancel" runat="server" ToolTip="Cancel" CssClass="btn btncancel" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Panel ID="pnlVersion" runat="server" Style="width: 100%; max-height: 500px;"
                            ScrollBars="Auto">
                            <asp:GridView runat="server" ID="GV_CRFVersion" AutoGenerateColumns="false" Width="800px"
                                BorderStyle="Solid" BorderColor="#1560a1" BorderWidth="1" EmptyDataText="No Data Found"
                                Style="margin: auto;">
                                <RowStyle BackColor="#cee3ed" Font-Names="Verdana" VerticalAlign="Top" HorizontalAlign="left"
                                    Font-Size="Small" ForeColor="navy" />
                                <EditRowStyle BackColor="#cee3ed" Font-Names="Verdana" Font-Size="Small" />
                                <HeaderStyle Font-Underline="False" BackColor="#1560a1" Font-Names="Verdana" VerticalAlign="Top"
                                    Font-Size="Small" HorizontalAlign="Center" ForeColor="white" Font-Bold="True" />
                                <FooterStyle BackColor="#1560a1" Font-Names="Verdana" Font-Size="Small" HorizontalAlign="Center"
                                    ForeColor="white" Font-Bold="True" />
                                <Columns>
                                    <asp:BoundField DataField="nVersionNo" HeaderText="Version No">
                                        <ItemStyle HorizontalAlign="Center" Width="10" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="cFreezeStatus" HeaderText="Status">
                                        <ItemStyle HorizontalAlign="Center" Width="10" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vRemark" HeaderText="Remarks">
                                        <ItemStyle HorizontalAlign="Center" Width="10" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="dVersiondate" DataFormatString="{0:dd'-'MMM'-'yyyy HH:mm}"
                                        HeaderText="Version Date">
                                        <ItemStyle HorizontalAlign="Center" Width="90" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vFirstName" HeaderText="Modify By">
                                        <ItemStyle HorizontalAlign="Center" Width="10" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="dModifyOn" DataFormatString="{0:dd'-'MMM'-'yyyy HH:mm}"
                                        HeaderText="Modify On">
                                        <ItemStyle HorizontalAlign="Center" Width="90" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Audit Trail">
                                        <ItemStyle HorizontalAlign="Center" Width="90" />
                                        <ItemTemplate>
                                            <asp:ImageButton ID="AuditTrail" ImageUrl="~/Images/audit.png" alt="AuditTrail"
                                                runat="server" ToolTip="Audit Trail" onmouseover="this.style.cursor='pointer';" Height="20px" Width="20px"
                                                />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnShow" runat="server" Text="show Dialog" Style="display: none" />
            <cc1:ModalPopupExtender ID="MPEID" runat="server" TargetControlID="btnShow" PopupControlID="DivAUditGrid"
                BackgroundCssClass="modalBackground" BehaviorID="MPEId">
            </cc1:ModalPopupExtender>
            <div id="DivAUditGrid" runat="server" class="centerModalPopup" style="display: none;
                width: 80%; max-height: 250px;">
                <div style="width: 100%">
                    <h1 class="header">
                        <label id="lblDocAction" class="LabelBold">
                            Audit Trail
                        </label>
                        <img id="ImgPopUpClose" alt="Close" title="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';"
                            onclick="funHideMPE();" />
                    </h1>
                </div>
                <div style="width: 100%; max-height: 200px; overflow: auto;">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:GridView runat="server" ID="GV_Audit" AutoGenerateColumns="false" Width="100%"
                                    BorderStyle="Solid" BorderColor="#1560a1" BorderWidth="1" EmptyDataText="No Audit Trail">
                                    <RowStyle BackColor="#cee3ed" Font-Names="Verdana" VerticalAlign="Top" HorizontalAlign="left"
                                        Font-Size="Small" ForeColor="navy" />
                                    <EditRowStyle BackColor="#cee3ed" Font-Names="Verdana" Font-Size="Small" />
                                    <HeaderStyle Font-Underline="False" BackColor="#1560a1" Font-Names="Verdana" VerticalAlign="Top"
                                        Font-Size="Small" HorizontalAlign="Center" ForeColor="white" Font-Bold="True" />
                                    <FooterStyle BackColor="#1560a1" Font-Names="Verdana" Font-Size="Small" HorizontalAlign="Center"
                                        ForeColor="white" Font-Bold="True" />
                                    <Columns>
                                        <asp:BoundField DataField="nVersionNo" HeaderText="Version No">
                                            <ItemStyle HorizontalAlign="Center" Width="10" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="cFreezeStatus" HeaderText="Status">
                                            <ItemStyle HorizontalAlign="Center" Width="10" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="vRemark" HeaderText="Remarks">
                                            <ItemStyle HorizontalAlign="Center" Width="10" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="dVersiondate_IST" DataFormatString="{0:dd'-'MMM'-'yyyy }"
                                            HeaderText="Version Date">
                                            <ItemStyle HorizontalAlign="Center" Width="150" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="vFirstName" HeaderText="Modify By">
                                            <ItemStyle HorizontalAlign="Center" Width="10" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="dModifyOn_IST" DataFormatString="{0:dd'-'MMM'-'yyyy   HH:mm}"
                                            HeaderText="Modify On">
                                            <ItemStyle HorizontalAlign="Center" Width="150" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnsave" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" src="Script/General.js"></script>

    <script type="text/javascript" language="javascript">
    
        function ClientPopulated(sender, e) {
            ProjectClientShowingCRFVersion('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }
        function OnSelected(sender, e) {

            ProjectOnItemSelectedCRFVersion(e.get_value(), $get('<%= txtProject.clientid %>'),
       $get('<%= HProjectId.clientid %>'), $get('<%= HFreezeStatus.clientid %>'),
       $get('<%= HVersionNo.clientid %>'), "", $get('<%=HFSponsor.clientid%>'), $get('<%= btnSetProject.Clientid%>'));
           
        }
        //    function funRadioList()
        //    {
        //        var status=document.getElementById('<%= HFreezeStatus.clientid %>').value;
        //        if(status=='F')   
        //        {
        //           document.getElementById('ctl00_CPHLAMBDA_RblFreeze_0').checked=true; 
        //           document.getElementById('ctl00_CPHLAMBDA_RblFreeze_1').checked=false;
        //            return true ;
        //        }
        //        document.getElementById('ctl00_CPHLAMBDA_RblFreeze_0').checked=false; 
        //        document.getElementById('ctl00_CPHLAMBDA_RblFreeze_1').checked=true; 
        //        return true ;      
        //    }
        
      
        function funChk_Remark(e) {
            if (document.getElementById('<%= txtProject.ClientId %>').value.toString().trim() == "") {
                msgalert("Please Enter Project !");
                return false;
            }
            else {
                if (document.getElementById('<%= txtRemark.ClientId %>').value.toString().trim() == "") {
                    msgalert("Please Enter Remarks !");
                    return false;
                }
            }
            if (document.getElementById('ctl00_CPHLAMBDA_RblFreeze_0').checked == true) {
                msgConfirmDeleteAlert(null, "Are You Sure You Want To Freeze this Version?", function (isConfirmed) {
                    if (isConfirmed) {
                        __doPostBack(e.name, '');
                        return true;
                    } else {
                        return false;
                    }
                });
            }

            if (document.getElementById('ctl00_CPHLAMBDA_RblFreeze_1').checked == true) {
                msgConfirmDeleteAlert(null, "Are You Sure You Want To Change Version?", function (isConfirmed) {
                    if (isConfirmed) {
                        __doPostBack(e.name, '');
                        return isConfirmed;
                    } else {
                        return isConfirmed;
                    }
                });
            }
            return false;

        }
        function funHideMPE() {
            $find('MPEId').hide();           
        }
        function funAudit() {
            $find('MPEId').show();
        }
        //Add by shivani pandya for project lock
        function getData() {
            var WorkspaceID = $('input[id$=HProjectId]').val();
            $.ajax({
                type: "post",
                url: "frmCRFVersionMst.aspx/LockImpact",
                data: '{"WorkspaceID":"' + WorkspaceID + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                dataType: "json",
                success: function (data) {
                    if (data.d == "L"){
                        msgalert("Project is locked !");
                        $("#<%=hndLockStatus.ClientID%>").val("Lock");
                    }
                    if (data.d == "U") {
                        $("#<%=hndLockStatus.ClientID%>").val("UnLock");
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

</asp:Content>
