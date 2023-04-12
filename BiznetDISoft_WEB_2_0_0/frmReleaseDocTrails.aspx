<%@ Page Language="VB" MasterPageFile="~/ECTDMasterPage.master" AutoEventWireup="false"
    CodeFile="frmReleaseDocTrails.aspx.vb" Inherits="frmReleaseDocTrails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <style type="text/css">
        #ctl00_CPHLAMBDA_pnlReleasedetail tr:nth-child(odd) textarea { background: #CEE3ED; border: 0; }
        #ctl00_CPHLAMBDA_pnlReleasedetail tr:nth-child(even) textarea { background: white; border: 0; }
        fieldset { width: 90%; }
    </style>
    <div id="divMain" style="width: 100%">
        <asp:UpdatePanel ID="upMain" runat="server">
            <ContentTemplate>
                <table style="width: 98%">
                    <tr style="border-top-style: solid; border-top-width: thin">
                        <td id="tdContent" style="top: 5px; vertical-align: top; overflow: auto; display: block;"
                            align="center">
                            <fieldset id="fldReleaseDocTrail" class="detailsbox" runat="server" style="padding: 10px;
                                text-align: left;">
                                <legend class="LabelBold" style="color: Black">Released Document Trail </legend>
                                
                                <table style="width: 100%">
                                    <tr>
                                        <td style="text-align: right" class="labelbold">
                                            Select Department :
                                        </td>
                                        <td style="text-align: left; padding-left: 10px">
                                            <asp:DropDownList ID="ddlProject" runat="server" CssClass="dropDownList" AutoPostBack="true"
                                                TabIndex="1">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align: right" class="labelbold">
                                            Select User Department :
                                        </td>
                                        <td style="text-align: left; padding-left: 10px">
                                            <asp:DropDownList ID="ddlUserDept" runat="server" CssClass="dropDownList" TabIndex="2">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%; text-align: right" class="labelbold">
                                            Select SOP :
                                        </td>
                                        <td style="text-align: left; padding-left: 10px">
                                            <asp:DropDownList ID="ddlSOPNo" runat="server" CssClass="dropDownList" AutoPostBack="true"
                                                TabIndex="3">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 20%; text-align: right" class="labelbold">
                                            Select SOP Type :
                                        </td>
                                        <td style="text-align: left; padding-left: 10px">
                                            <asp:DropDownList ID="ddlSOPType" runat="server" CssClass="dropDownList" AutoPostBack="true"
                                                TabIndex="4">
                                                <asp:ListItem Selected="True">Select SOP Type</asp:ListItem>
                                                <asp:ListItem>Normal SOP</asp:ListItem>
                                                <asp:ListItem>Method SOP</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%; text-align: right" class="labelbold">
                                            Select Form :
                                        </td>
                                        <td style="text-align: left; padding-left: 10px">
                                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="dropDownList" AutoPostBack="true"
                                                TabIndex="5">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 20%; text-align: right" class="labelbold">
                                            Release Date From :
                                        </td>
                                        <td style="text-align: left; padding-left: 10px">
                                            <asp:TextBox ID="txtDateFrom" runat="server" Style="width: 75px" TabIndex="6"></asp:TextBox>
                                            <cc1:CalendarExtender ID="calFromDate" runat="server" TargetControlID="txtDateFrom"
                                                Format="dd-MMM-yyyy">
                                            </cc1:CalendarExtender>
                                            To :
                                            <asp:TextBox ID="txtDateTo" runat="server" Style="width: 75px" TabIndex="7"></asp:TextBox>
                                            <cc1:CalendarExtender ID="calToDate" runat="server" TargetControlID="txtDateTo" Format="dd-MMM-yyyy">
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%; text-align: right" class="labelbold">
                                            Select Project No :
                                        </td>
                                        <td style="text-align: left; padding-left: 10px">
                                            <asp:TextBox ID="txtProjectNo" runat="server" CssClass="textBox" MaxLength="50" Width="250px"
                                                TabIndex="8"></asp:TextBox>
                                            <asp:HiddenField ID="hdnProjectNo" runat="server" />
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                                                TargetControlID="txtProjectNo" ServicePath="AutoComplete.asmx" OnClientShowing="ClientPopulated"
                                                OnClientItemSelected="OnSelected" MinimumPrefixLength="1" CompletionListItemCssClass="autocomplete_listitem"
                                                CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListCssClass="autocomplete_list"
                                                BehaviorID="AutoCompleteExtender1" CompletionListElementID="pnlProjectList" ServiceMethod="GetProjectCompletionListForDMS">
                                            </cc1:AutoCompleteExtender>
                                            <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 300px; overflow: auto;" />
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                        <td colspan="100%" style="text-align: center;">
                                            <asp:Button ID="btnSearch" runat="server" Text="Search" class="btn btnnew" TabIndex="10"
                                                OnClientClick="return fnValidate();" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr id="trReleaseDetail" runat="server">
                        <td style="top: 5px; vertical-align: top; overflow: auto; display: block; padding-top: 10px"
                            align="center">
                            <fieldset id="fldReleaseDetail" class="detailsbox" runat="server" style="display: none;
                                text-align: left;">
                                <legend class="LabelBold" style="color: Black">Release Details</legend>
                                <div>
                                    <table style="width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btnExportgvReleaseDetail" runat="server" class="btn btnexcel" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <asp:Panel ID="pnlReleasedetail" runat="server" Style="max-height: 360px; width: 100%"
                                    ScrollBars="Auto">
                                    <asp:GridView ID="gvReleaseDetail" runat="server" SkinID="grdViewDocs" AutoGenerateColumns="false"
                                        Style="width: 100%; margin: 5px" AllowPaging="true" PageSize="50" TabIndex="8">
                                        <Columns>
                                            <asp:BoundField DataField="vWorkspaceId">
                                                <ItemStyle HorizontalAlign="Left" Width="5"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="iParentNodeId">
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="iReleasedBy">
                                                <ItemStyle HorizontalAlign="Left" Width="250"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="vFileName">
                                                <ItemStyle HorizontalAlign="Left" Width="150"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="iQty">
                                                <ItemStyle HorizontalAlign="Center" Width="30"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Form">
                                                <ItemStyle HorizontalAlign="Left" Width="300"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtFormName" runat="server" Enabled="false" Style="width: 98%;"
                                                        TextMode="MultiLine" Text='<%#eval("vNodeDisplayName") %>' Wrap="true"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="vStartId">
                                                <ItemStyle HorizontalAlign="Left" Width="170" Wrap="false"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="vEndId">
                                                <ItemStyle HorizontalAlign="Left" Width="170" Wrap="false"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            </asp:BoundField>
                                            
                                            <asp:BoundField DataField="dModifyOn_IST" DataFormatString="{0:dd'-'MMM'-'yyyy H:mm tt}">
                                                <ItemStyle HorizontalAlign="Left" Width="100" Wrap="false"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="vUserName">
                                                <ItemStyle HorizontalAlign="Left" Width="120" Wrap="false"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            </asp:BoundField>
                                            
                                            <asp:BoundField DataField="ProjectNumber">
                                                <ItemStyle HorizontalAlign="Left" Width="120" Wrap="false"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Comments">
                                                <ItemStyle HorizontalAlign="Left" Width="150" Wrap="true"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtReleaseDetailComment" runat="server" Enabled="false" Style="width: 98%;"
                                                        TextMode="MultiLine" Text='<%#eval("vComments") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </fieldset>
                            <fieldset id="fldReleaseMsg" class="detailsbox" runat="server" style="display: none">
                                <legend class="LabelBold" style="color: Black">&nbsp;&nbsp;Release Details&nbsp;&nbsp;</legend>
                                <asp:Panel ID="pnlReleaseMsg" runat="server" Style="width: 850px; max-height: 30px"
                                    ScrollBars="Auto">
                                    <asp:Label ID="lblMsg" Text="No Data Found" CssClass="labelbold" Style="background-color: Silver;
                                        font-weight: bold; color: Navy">&nbsp;&nbsp;NO DATA FOUND&nbsp;&nbsp;</asp:Label>
                                </asp:Panel>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; text-align: center">
                            <%--<input type="button" onclick="onUpdating();document.location.href='frmMainPage.aspx?mode=1';" 
                                value="Exit" class="btn btnexit" tabindex="9" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this); " />--%>
                            <asp:Button ID="btnExit" OnClick="btnExit_Click" runat="server" Text="Exit" ToolTip="Exit"
                                CssClass="btn btnexit" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this); "
                                TabIndex="9"></asp:Button>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExportgvReleaseDetail" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <script src="Script/jquery-1.7.min.js" type="text/javascript"></script>
    <script src="Script/AutoComplete.js" type="text/javascript"></script>

    <script type="text/javascript">
        function fnValidate() {
            var flag = 0;
            if (document.getElementById("<%= ddlProject.ClientId %>").selectedIndex != 0) {
                flag = 1;
            }
            else if (document.getElementById("<%= ddlCategory.ClientId %>").selectedIndex > 0) {
                flag = 1;
            }
            else if (document.getElementById("<%= ddlUserDept.ClientId %>").selectedIndex > 0) {
                flag = 1;
            }
            else if (document.getElementById("<%= txtDateFrom.ClientId %>").value != "" && document.getElementById("<%= txtDateTo.ClientId %>").value != "") {
                flag = 1;
            }
            if ( Date.parse(document.getElementById("<%=txtDateFrom.ClientId%>").value) > Date.parse(document.getElementById("<%=txtDateTo.ClientId%>").value)){
                msgalert('TO date Must Be Greater Than/Equal To FROM Date !');
                document.getElementById("<%=txtDateTo.ClientId%>").value = "";
                document.getElementById("<%=txtDateTo.ClientId%>").focus();
                flag = 1;
                return false;
            }
            else if(document.getElementById("<%= ddlSOPType.ClientId %>").selectedIndex >0){
                flag = 1;
            }
            else if(document.getElementById("<%= txtProjectNo.ClientId %>" ).value != ""){
                flag = 1;
            }
            

            if (flag == 0) {
                msgalert('Please Select Atleast One Criteria !');
                return false;
            }
            
        }
        
        function ClientPopulated(sender, e) {

            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProjectNo.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProjectNo.clientid %>'),
            $get('<%= hdnProjectNo.Clientid %>'));
        }
    </script>

</asp:Content>
