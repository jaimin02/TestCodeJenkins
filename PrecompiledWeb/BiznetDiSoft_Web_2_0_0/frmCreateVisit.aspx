<%@ page title="" language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmCreateVisit, App_Web_l40sj1d0" enableeventvalidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>


<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="Script/Gridview.js"></script>
    <script type="text/javascript" src="Script/scrollablegrid.js"></script>
    <script type="text/javascript" src="Script/popcalendar.js"></script>
    <script type="text/javascript" src="Script/Validation.js"></script>
    <script type="text/javascript" src="script/AutoComplete.js"></script>
    <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }
    </style>
    <asp:UpdatePanel ID="updUCMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellpadding="5px" style="width: 90%; margin: auto;">
                <tbody>
                    <tr>
                        <td>
                            <fieldset class="FieldSetBox" style="display: block; width: 98%; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img2" alt="Subject Details" src="images/panelcollapse.png"
                                        onclick="Display(this,'divPatientDetail');" runat="server" style="margin-right: 2px;" />Visit Details</legend>
                                <div id="divPatientDetail">
                                    <table width="100%">
                                        <tr>
                                            <td>Site No* :
                                                            <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="30%" MaxLength="50"></asp:TextBox>
                                                <asp:Button Style="display: none" ID="btnSetProject" OnClick="btnSetProject_Click"
                                                    runat="server" Text=" Project"></asp:Button>
                                                <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                                                <asp:HiddenField ID="HExternalProjectNo" runat="server"></asp:HiddenField>
                                                <asp:HiddenField ID="HProjeName" runat="server"></asp:HiddenField>
                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                                    CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                    CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                                    OnClientShowing="ClientPopulated" ServiceMethod="GetMyProjectCompletionList"
                                                    ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True"
                                                    CompletionListElementID="pnlProjectList">
                                                </cc1:AutoCompleteExtender>
                                                <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 300px; overflow: auto; overflow-x: hidden" />
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="btnImport" runat="server" Text="Create Visit" ToolTip="Create Visit"
                                                    CssClass="btn btnnew" OnClientClick="return CheckProjectName();"></asp:Button>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Label" nowrap="nowrap" style="text-align: left;" colspan="2">
                                                <asp:GridView ID="gvVisitList" runat="server" AutoGenerateColumns="false" DataKeyNames="vWorkspaceId,iNodeId,vSubjectId">
                                                    <Columns>

                                                        <asp:BoundField DataField="vProjectNo" HeaderText="Site Id" />
                                                        <asp:BoundField DataField="vInitials" HeaderText="PationtInitial" />
                                                        <asp:BoundField DataField="vMySubjectNo" HeaderText="Screening No" />
                                                        <asp:BoundField DataField="vVisitNo" HeaderText="Visit" />
                                                        <asp:BoundField DataField="vDOB" HeaderText="Visit Date" />
                                                        <asp:BoundField DataField="vRandomizationNo" HeaderText="RandomizationNo" />
                                                        <asp:BoundField DataField="vRandomizationNo" HeaderText="Randomization Date" Visible="false" />
                                                        <asp:TemplateField HeaderText="Upload">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload" ToolTip="Upload" CommandName="Upload" CommandArgument="<%# Container.DataItemIndex %>"></asp:Button>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Status" HeaderText="Status" />
                                                        <asp:BoundField DataField="vWorkspaceId" HeaderText="Project Id" Visible="false" />
                                                        <asp:BoundField DataField="iNodeId" HeaderText="Visit Id" Visible="false" />
                                                        <asp:BoundField DataField="vSubjectId" HeaderText="SubjectId" Visible="false" />
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>

                                        <tr style="text-align: center; text-align: center;">
                                            <td align="center" style="text-align: center; text-align: center;">

                                                <button id="btnMpeSubjectMst" runat="server" style="display: none;" />
                                                <cc1:ModalPopupExtender ID="MpeSubjectMst" runat="server" PopupControlID="divSubjectMst" BehaviorID="MpeSubjectMst"
                                                    BackgroundCssClass="modalBackground" TargetControlID="btnMpeSubjectMst" CancelControlID="ImgSubjectMstClose">
                                                </cc1:ModalPopupExtender>
                                                <div style="display: none; left: 330px; width: 28%; top: 367px; text-align: center; border-radius: 15px; background-color: white"
                                                    id="divSubjectMst1" class="DIVSTYLE2" runat="server">
                                                </div>

                                                <div class="modal-content modal-sm" id="divSubjectMst" style="display: none; width: 90%; height: 80%;" runat="server">
                                                    <div class="modal-header">
                                                        <img src="images/Sqclose.gif" class="ModalImage" onclick="ModalClose();" alt="" id="ImgSubjectMstClose" />
                                                        <h2>Visit Enrollment</h2>
                                                    </div>
                                                    <div class="modal-body">
                                                        <table style="width: 100%;">

                                                            <tr>
                                                                <td colspan="2">

                                                                    <asp:HiddenField ID="HReplaceImySubjectNo" runat="server"></asp:HiddenField>
                                                                    <asp:GridView ID="gvCreateVisit" runat="server" AutoGenerateColumns="false" DataKeyNames="vSubjectId">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="">
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chkVisit" runat="server" class="chkDisplay"></asp:CheckBox>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="vSubjectId" HeaderText="Subject Id" Visible="false" />
                                                                            <asp:BoundField DataField="vMySubjectNo" HeaderText="Screening No" />
                                                                            <asp:BoundField DataField="vInitials" HeaderText="Initials" />
                                                                            <asp:BoundField DataField="dReportingDate" HeaderText="Reporting Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                                                            <asp:BoundField DataField="vFirstName" HeaderText="vFirstName" Visible="false" />
                                                                            <asp:BoundField DataField="vMiddleName" HeaderText="vMiddleName" Visible="false" />
                                                                            <asp:BoundField DataField="vSurName" HeaderText="vSurName" Visible="false" />
                                                                            <asp:BoundField DataField="vRandomizationNo" HeaderText="Randomization No" />
                                                                            <%--<asp:BoundField DataField="vRandomizationDate" HeaderText="Randomization Date" />--%>
                                                                            <asp:TemplateField HeaderText="Randomization Date">
                                                                                <ItemTemplate>
                                                                                    <%--<asp:Label ID="lblRandomizationDate" Text='<%#Convert.ToDateTime(Eval("vRandomizationDate")).ToString("dd-MMM-yyyy")%>' runat="server" />--%>
                                                                                <asp:Label ID="lblRandomizationDate" Text='<%# If(Eval("vRandomizationDate").ToString() = "", String.Empty, Convert.ToDateTime(Eval("vRandomizationDate")).ToString("dd-MMM-yyyy"))%>' runat="server"/>
                                                                                    
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="vVisitNo" HeaderText="Visit" />
                                                                            <asp:BoundField DataField="dVisitDate" HeaderText="Visit Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                                                        </Columns>
                                                                    </asp:GridView>

                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>

                                                    <div class="modal-header">
                                                        <asp:Button ID="BtnSaveVisit" runat="server" Text="Save" ToolTip="Save" CssClass="btn btnsave" />
                                                        <asp:Button ID="Btnhidden" runat="server" Text="Hidden" CssClass="button" Style="display: none" />
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </fieldset>
                        </td>
                    </tr>


                </tbody>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnImport" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="BtnSaveVisit" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="txtProject" EventName="TextChanged" />
        </Triggers>
    </asp:UpdatePanel>
    <script type="text/javascript">

        function pageLoad() {
            if ($get('<%= gvVisitList.ClientID()%>') != null && $get('<%= gvVisitList.ClientID%>_wrapper') == null) {
                if (jQuery('#<%= gvVisitList.ClientID%>')) {
                    jQuery('#<%= gvVisitList.ClientID%>').prepend($('<thead>').append($('#<%= gvVisitList.ClientID%> tr:first'))).DataTable({
                        "bJQueryUI": true,
                        "sPaginationType": "full_numbers",
                        "scrollCollapse": true,
                        "pageLength": 5,
                        "bProcessing": true,
                        "bPaginate": true,
                        "bFooter": false,
                        "bHeader": false,
                        "AutoWidth": true,
                        "bSort": false,
                        "fixedHeader": true,
                        //"dom": 'T<"clear">lfrtip',
                        //"sDom": '<"H"frT>t<"F"i>',
                        "oLanguage": { "sSearch": "Search" },
                        "aLengthMenu": [[10, 20, 50, -1], [10, 20, 50, "All"]],
                        "iDisplayLength": 10,
                    });
                }
                $(".dataTables_wrapper").css("width", ($(window).width() * 0.85 | 0) + "px");
            }

            if ($get('<%= gvCreateVisit.ClientID()%>') != null && $get('<%= gvCreateVisit.ClientID%>_wrapper') == null) {
                if (jQuery('#<%= gvCreateVisit.ClientID%>')) {
                     jQuery('#<%= gvCreateVisit.ClientID%>').prepend($('<thead>').append($('#<%= gvCreateVisit.ClientID%> tr:first'))).DataTable({
                        "bJQueryUI": true,
                        "sPaginationType": "full_numbers",
                        "scrollCollapse": true,
                        "pageLength": 5,
                        "bProcessing": true,
                        "bPaginate": true,
                        "bFooter": false,
                        "bHeader": false,
                        "AutoWidth": true,
                        "bSort": false,
                        "fixedHeader": true,
                        //"sScrollY": "250px",
                        //"dom": 'T<"clear">lfrtip',
                        //"sDom": '<"H"frT>t<"F"i>',
                        "oLanguage": { "sSearch": "Search" },
                        "aLengthMenu": [[10, 20, 50, -1], [10, 20, 50, "All"]],
                        "iDisplayLength": 10,
                    });
                }
                $(".dataTables_wrapper").css("width", ($(window).width() * 0.85 | 0) + "px");
            }
        }

        function CheckProjectName() {
            if (document.getElementById('<%=txtProject.ClientID%>').value.toString().trim().length <= 0) {
                msgalert("Please Select The Project !")
                return false;
            }
            return true;
        }

       <%-- function UIgvCreateVisit() {

            $('#<%=gvCreateVisit.ClientID%>').removeAttr('style', 'display:block');
            oTab = $('#<%=gvCreateVisit.ClientID%>').prepend($('<thead>').append($('#<%=gvCreateVisit.ClientID%> tr:first'))).dataTable({
                "bDestroy": true,
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": false,
                "sScrollY": "250px",
                "sScrollXInner": "100%",
                "sScrollX": "100%",
                "bScrollCollapse": true,
                aLengthMenu: [
                    [10, 25, 50, 100, -1],
                    [10, 25, 50, 100, "All"]
                ],
            });
            return false;
        }--%>
        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }
        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

    </script>
</asp:Content>

