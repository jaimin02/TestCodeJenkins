<%@ page language="VB" masterpagefile="~/ECTDMasterPage.master" autoeventwireup="false" inherits="CDMS_frmCDMSStudyStatusLog, App_Web_4wz2dz2v" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <link rel="stylesheet" href="../App_Themes/CDMS.css" />

    <script src="../Script/jquery-1.11.3.min.js" type="text/javascript"></script>

    <script src="../Script/Jquery.js" type="text/javascript"></script>

    <script src="../Script/General.js" type="text/javascript"></script>

    <script src="../Script/Validation.js" language="javascript" type="text/javascript"></script>

    <script src="../Script/AutoComplete.js" type="text/javascript"></script>

    <style type="text/css">
        .Color {
            color: white !important;
        }
    </style>

    <div>




        <br />
        <table style="width: 80%;">
            <tr>
                <td style="width: 35%; text-align: right;" class="LabelText">
                    <asp:Label ID="lblProject" runat="server" Style="color: Black;" Text="Select Project* :"></asp:Label>
                </td>
                <td style="text-align: left;">
                    <asp:TextBox ID="txtsearch" runat="server" CssClass="TextBox" Width="55%" />
                    <asp:Button Style="display: none" ID="BtnEdit" runat="server" ForeColor="Black" Font-Bold="True"
                        Text="Edit" BackColor="White" BorderColor="Black" BorderStyle="Solid" />
                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" UseContextKey="True"
                        TargetControlID="txtsearch" ServicePath="~/AutoComplete.asmx" OnClientShowing="ClientPopulated"
                        OnClientItemSelected="OnSelected" MinimumPrefixLength="1" ServiceMethod="GetMyProjectCompletionList"
                        CompletionListElementID="pnlSubjectForProject" CompletionListItemCssClass="autocomplete_listitem"
                        CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListCssClass="autocomplete_list"
                        BehaviorID="AutoCompleteExtender2">
                    </cc1:AutoCompleteExtender>
                    <asp:Panel ID="pnlSubjectForProject" runat="server" Style="max-height: 100px; overflow: auto; overflow-x: hidden;" />
                    <asp:HiddenField ID="HdnWorkspaceId" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center;" class="LabelText">
                    <asp:Button ID="btnGo" runat="server" Text="" CssClass="btn btngo" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btncancel" />
                    <asp:Button ID="btnExport" runat="server" Text="" Style="display: none;"
                        CssClass="btn btnexcel" ToolTip="Export Grid Data To Excel" />
                </td>
            </tr>
        </table>
        <br />
        <div>
            <asp:Label ID="lbl_Message" runat="server"></asp:Label>
        </div>

        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <fieldset id="fldResult" runat="server" class="FieldSetBox" style="width: 50%; text-align: left"
                    visible="false">
                    <legend class="LegendText" style="color: Black; text-align: left">Result</legend>
                    <table width="100%" align="center">
                        <tr>
                            <td colspan="100%">
                                <div id="divGrid" style="overflow: auto; max-height: 400px;" runat="server">
                                    <asp:GridView ID="gvwSubjectForProject" runat="server" AutoGenerateColumns="false"
                                        SkinID="grdViewSmlAutoSize" Width="100%" ShowFooter="true">
                                        <Columns>
                                            <asp:BoundField HeaderText="Sr. No." DataField="Sr.No.">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Status" HeaderStyle-Width="70%">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnExpand" runat="server" ImageUrl="../images/expand.png"
                                                        alt="Expand" OnClientClick="imgbtnExpand_Click" Height="13px" Width="13px" />
                                                    <asp:HiddenField ID="hdncStatus" runat="server" Value='<%# Eval("cStatus") %>' />
                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                                                    <div id="div_gvChild" runat="server" style="max-height: 300px; width: 100%; position: static; overflow: auto; text-align: right; float: right">
                                                        <asp:Panel ID="pnlgvwSubjectStatus" runat="server" BorderColor="Black">
                                                            <asp:GridView ID="gvChildGrid" runat="server" AutoGenerateColumns="false" SkinID="grdViewSmlAutoSize"
                                                                Width="100%">
                                                                <Columns>
                                                                    <asp:BoundField HeaderText="Sr.No." DataField="Sr.No.">
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField ItemStyle-Width="40%" DataField="vSubjectID" HeaderText="Subject Id">
                                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="true" Width="40%" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="true" Width="40%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField ItemStyle-Width="70%" DataField="FullName" HeaderText="Full Name">
                                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="true" Width="60%" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="true" Width="60%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField HeaderText="Age" DataField="nAge">
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                </Columns>
                                                                <AlternatingRowStyle BackColor="White" ForeColor="White" />
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Subject">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalSubject" runat="server" Text='<%# Bind("TotalSubject") %>' />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblFooterTotalSubject" runat="server" ForeColor="White" Font-Size="9"
                                                        Font-Bold="true"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle Font-Bold="true" ForeColor="White" Font-Size="9" HorizontalAlign="Center" />
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </ContentTemplate>
            <Triggers>
                <%--   <asp:PostBackTrigger ControlID="btnGo" />--%>
                <asp:AsyncPostBackTrigger ControlID="btngo" EventName="click" />
                <%--  <asp:AsyncPostBackTrigger ControlID="btnExport" EventName ="click" />--%>
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript" language="javascript">

        function ClientPopulated(sender, e) {
            var searchText = $get('<%= txtsearch.ClientId %>');
            ProjectClientShowing('AutoCompleteExtender2', searchText);
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtsearch.clientid %>'), $get('<%= HdnWorkspaceId.clientid %>'));
        }

        function Buttonexport() {
            $("#ctl00_CPHLAMBDA_btnExport").css("display", "inline");
        }

        function ButtonexportHide() {
            $("#ctl00_CPHLAMBDA_btnExport").css("display", "none");

        }


    </script>

</asp:Content>
