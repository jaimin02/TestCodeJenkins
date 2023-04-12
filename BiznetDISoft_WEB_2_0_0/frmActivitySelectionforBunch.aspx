<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="frmActivitySelectionforBunch.aspx.vb" Inherits="frmActivitySelectionforBunch" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <style type="text/css">
        div.dataTables_wrapper {
            width: 100%;
            margin: 0 auto;
        }

        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

        th, td {
            white-space: nowrap;
        }

        .HeaderNoWrap {
            white-space: nowrap !important;
        }

        .Batchtxt {
            width: 10%;
        }

        .Activitytxt {
            width: 60%;
        }
    </style>

    <asp:UpdatePanel ID="Up_ChildProtocol" runat="server">
        <ContentTemplate>
            <table style="width: 100%; margin: auto;" cellpadding="3px">
                <tr>
                    <td>
                        <fieldset class="FieldSetBox" style="display: block; width: 98%; text-align: left; border: #aaaaaa 1px solid;">
                            <legend class="LegendText" style="color: Black; font-size: 12px">Project Details</legend>
                            <div id="divPatientDetail">
                                <table width="100%">
                                    <tr>
                                        <td class="Label" nowrap="nowrap" style="text-align: left; width: 5%;">Project Name/Request Id* : </td>
                                        <td class="Label" nowrap="nowrap" style="text-align: left; width: 30%;">
                                            <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" MaxLength="50" Width="99%"></asp:TextBox>
                                            <asp:Button Style="display: none" ID="btnSetProject" runat="server" Text=" Project"></asp:Button>
                                            <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="HParentWorkSpaceId" runat="server" />
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                                CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelectedProject"
                                                OnClientShowing="ClientPopulatedProject" ServiceMethod="GetMyProjectCompletionList"
                                                ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True"
                                                CompletionListElementID="pnlProjectList">
                                            </cc1:AutoCompleteExtender>
                                            <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" />
                                        </td>

                                        <td class="Label" nowrap="nowrap" style="text-align: left; width: 5%; padding-left: 5px;">Subject Initial/SubjectNo* : </td>
                                        <td class="Label" nowrap="nowrap" style="text-align: left; width: 20%;">
                                            <asp:TextBox ID="txtSubject" runat="server" CssClass="textBox" Width="99%" MaxLength="50"></asp:TextBox>
                                            <asp:Button Style="display: none" ID="btnSetSubject" runat="server" Text="Subject"></asp:Button>
                                            <asp:HiddenField ID="HSubject" runat="server"></asp:HiddenField>
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" BehaviorID="AutoCompleteExtender2"
                                                CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelectedSubject"
                                                OnClientShowing="ClientPopulatedSubject" ServiceMethod="GetSubjectCompletionList_Assigned_NotRejected"
                                                ServicePath="AutoComplete.asmx" TargetControlID="txtSubject" UseContextKey="True"
                                                CompletionListElementID="pnlSubjectList">
                                            </cc1:AutoCompleteExtender>
                                            <asp:Panel ID="pnlSubjectList" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </td>
                </tr>
            </table>
            <table style="width: 100%; margin: auto;" cellpadding="3" id="tblDataEntry" runat="server">
                <tr>
                    <td>
                        <fieldset id="fContactDetail" class="FieldSetBox" style="width: 98%;" runat="server">
                            <table style="width: 40%; margin: auto; float: left">
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left">
                                                    <asp:Label ID="LblActivity" runat="server" Text=""></asp:Label>
                                                    <asp:Panel ID="PnlActivity" runat="server" Width="45%" Visible="false" BorderWidth="1px"
                                                        BorderColor="navy" Height="150px" ScrollBars="Auto">
                                                        <asp:CheckBox ID="ChkBoxAllActivity" onclick="SelectAllFields()" runat="server" Font-Bold="True"
                                                            Text="Select All"></asp:CheckBox>
                                                        <asp:CheckBoxList ID="ChkBoxLstActivity" runat="server" Visible="False"></asp:CheckBoxList>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table style="width: 50%">
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="Up_TempateDtl" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                            <ContentTemplate>
                                                <asp:Panel ID="PnlPlaceMedex" runat="server" Width="100%">
                                                    <asp:PlaceHolder ID="PlaceMedEx" runat="server" EnableViewState="False"></asp:PlaceHolder>
                                                    <asp:HiddenField ID="hdnMedExCode" runat="server"></asp:HiddenField>
                                                    <asp:HiddenField ID="hdnMedExDesc" runat="server"></asp:HiddenField>
                                                    <asp:HiddenField ID="hdnAns1" runat="server"></asp:HiddenField>
                                                    <asp:HiddenField ID="hdnAns2" runat="server"></asp:HiddenField>
                                                    <asp:HiddenField ID="hdnAns3" runat="server"></asp:HiddenField>
                                                    <asp:HiddenField ID="hdnAns4" runat="server"></asp:HiddenField>
                                                    <asp:HiddenField ID="hdnAns5" runat="server"></asp:HiddenField>
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;" colspan="2" id="tdSave" runat="server">
                        <asp:Button ID="btnSave" runat="server" Text="Save" ToolTip="Save"
                            CssClass="btn btnsave" OnClientClick="return Validation(this);"></asp:Button>
                        <asp:Button ID="btnExit" runat="server" Text="Exit" ToolTip="Exit"
                            CssClass="btn btnexit" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);"></asp:Button>
                        <input id="btnExitView" class="btn btnexit" runat="server" onclick="closewindow(this);" type="button" value="Exit" title="Exit" visible="false" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%;">
                        <asp:UpdatePanel ID="Up_Dtl" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                            <ContentTemplate>
                                <div style="width: 80%; overflow: auto;">
                                    <asp:GridView ID="GvwDtl" runat="server" SkinID="grdViewSmlAutoSize" OnRowCommand="GvwDtl_RowCommand"
                                        OnRowDeleting="GvwDtl_RowDeleting" OnRowDataBound="GvwDtl_RowDataBound"
                                        AutoGenerateColumns="False" Style="width: 100%;">
                                        <Columns>
                                            <asp:BoundField DataField="iBunchId" HeaderText="Batch No." ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="Batchtxt" />
                                            <asp:BoundField DataField="vActivity" HeaderText="Activity" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="Activitytxt" />
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImbADelete" runat="server" ToolTip="Delete" ImageUrl="~/Images/i_delete.gif"></asp:ImageButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Bottom" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="txtProject" EventName="TextChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            <div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnExit" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="GvwDtl" EventName="RowCommand" />
        </Triggers>
    </asp:UpdatePanel>

    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.min.js"></script>
    <script src="Script/jquery.min.js?t=633677264283125000" type="text/javascript"></script>
    <script src="Script/jquery-2.1.0.min.js" type="text/javascript"></script>
    <script src="Script/jquery-ui-1.10.4.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <script src="Script/jquery-1.11.dataTables.min.js" type="text/javascript"></script>
    <script src="Script/jquery.dataTables.fixedColumns.js" type="text/javascript"></script>
    <script src="Script/TableTools.min.js" type="text/javascript"></script>
    <script src="Script/Validation.js" language="javascript" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        function ClientPopulatedProject(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtproject.ClientId %>'));
        }

        function OnSelectedProject(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
            //$("#tblDataEntry").attr("style", "Display:none");
        }

        function ClientPopulatedSubject(sender, e) {
            SubjectClientShowing('AutoCompleteExtender2', $get('<%= txtSubject.ClientId %>'));
        }

        function OnSelectedSubject(sender, e) {
            SubjectOnItemSelected(e.get_value(), $get('<%= txtSubject.clientid %>'),
            $get('<%= HSubject.clientid %>'), document.getElementById('<%= btnSetSubject.ClientId %>'));
            $("#ctl00_CPHLAMBDA_btnSetSubject").click();
        }

        function Validation(e) {
            var vProjectId = $("#ctl00_CPHLAMBDA_HProjectId").val();
            if (vProjectId == "") {
                msgalert("Please Select Project")
                return false;
            }

            var vSubjectId = $("#ctl00_CPHLAMBDA_HSubject").val();
            if (vSubjectId == "") {
                msgalert("Please Select Subject")
                return false;
            }

            var IsRecord = $("#ctl00_CPHLAMBDA_LblActivity").text();
            if (IsRecord == "") {
                msgalert("No Pending Visit Found!")
                return false;
            }

            var Isanychecked = $('#ctl00_CPHLAMBDA_PnlActivity').find('input[type="checkbox"]:checked').length;
            if (Isanychecked == 0) {
                msgalert("Please Select Atleast one Activity")
                return false;
            }

            //var UncheckedRdo = 0;
            //var tblLen = $("#tblMain tr").length;
            //for (var i = 1; i < tblLen+1; i++) {
            //    var isChecked = $('input[name=Template_' + i + ']:checked').val();
            //    if (isChecked == undefined) { isChecked = "";}
            //    if (isChecked == "") {
            //        UncheckedRdo += 1;
            //    }
            //}

            //if (UncheckedRdo != 0) {
            //    msgalert("Please tick Answer")
            //    return false;
            //}
            return true;
        }

        function show_confirm() {
            var r = confirm("Are You Sure You Want To Delete This Record?");
            return r
        }

        function SelectAllFields() {
            var chkSelectAll = document.getElementById('<%=ChkBoxAllActivity.Clientid%>').checked;
            var chklst = document.getElementById('<%=ChkBoxLstActivity.Clientid%>');
            var chks;
            var result = false;
            var i;
            if (chklst != null && typeof (chklst) != 'undefined') {
                chks = chklst.getElementsByTagName('input');
                if (chkSelectAll == true) {
                    for (i = 0; i < chks.length; i++) {
                        chks[i].checked = true;
                    }
                }
                else {
                    for (i = 0; i < chks.length; i++) {
                        chks[i].checked = false;
                    }
                }
            }
            return false;
        }

        $(document).on('dblclick', '.RemoveSelection', function () {
            if (this.checked) {
                $(this).prop('checked', false);
            }
        });

        function SetValue(id) {
            var Curval = $('input[name="Template_' + id + '"]:checked').val();
            $("#ctl00_CPHLAMBDA_hdnAns" + id).val(Curval);
        }

        function closewindow(ele) {
            msgConfirmDeleteAlert(null, "Are You Sure You Want To Exit ?", function (isConfirmed) {
                if (isConfirmed) {
                    var parWin = window.opener;
                    if (parWin != null && typeof (parWin) != 'undefined') {
                        if (parWin && parWin.open && !parWin.closed) {
                            if (typeof (window.opener.RefreshPage) != 'undefined') {
                                window.opener.RefreshPage();
                            }
                        }
                    }
                    self.close();
                }
                else {
                    return false;
                }
            });
        }
    </script>

</asp:Content>
