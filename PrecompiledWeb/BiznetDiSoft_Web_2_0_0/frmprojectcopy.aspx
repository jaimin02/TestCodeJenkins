<%@ page language="VB" masterpagefile="~/ECTDMasterPage.master" autoeventwireup="false" inherits="frmprojectcopy, App_Web_qa4vhgvp" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <%-- <link rel="stylesheet" type="text/css" href="App_Themes/smoothnessjquery-ui.css" />
    <script type="text/javascript" src="Script/jquery-1.9.1.js"></script>

    <script type="text/javascript" src="Script/jquery-ui-1.10.2.custom.min.js"></script>

    <script type="text/javascript" src="Script/jquery-ui.js"></script>--%>

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <asp:UpdatePanel runat="server" ID="uppanel" UpdateMode="Conditional">
        <ContentTemplate>
            <div>
                <br />
                <div id="maindiv" runat="server">
                    <table cellpadding="5px" style="margin-right: auto; width: 100%;">
                        <tr>
                            <td>
                                <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                                    <legend class="LegendText" style="color: Black; font-size: 12px">
                                        <img id="img2" alt="Project Structure Details" src="images/panelcollapse.png"
                                            onclick="Display(this,'divProjectDetail');" runat="server" style="margin-right: 2px;" />Project Structure Details</legend>
                                    <div id="divProjectDetail">
                                        <table width="98%">
                                            <tr>
                                                <td style="text-align: right; color: Black; width: 21%;">Project Name/Request Id*(From):
                                                </td>
                                                <td style="text-align: left; width: 20%;">
                                                    <asp:TextBox ID="txtfromProject" runat="server" CssClass="textBox" TabIndex="1" Style="width: 99%;" />
                                                    <asp:Button Style="display: none" ID="btnSetfromProject" runat="server" Text=" Project" CssClass="btn btnnew" />
                                                    <asp:HiddenField ID="HfromProjectId" runat="server"></asp:HiddenField>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                                                        TargetControlID="txtfromProject" ServicePath="AutoComplete.asmx" ServiceMethod="GetMyProjectCompletionListwithoutInitial"
                                                        OnClientShowing="ClientFromPopulated" OnClientItemSelected="OnFromSelected" MinimumPrefixLength="1"
                                                        CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                        CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1" CompletionListElementID="pnlfromProject">
                                                    </cc1:AutoCompleteExtender>
                                                    <asp:Panel ID="pnlfromProject" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden;" />
                                                </td>
                                                <td style="text-align: right; color: Black; width: 19%;">Project Name/Request Id*(To):
                                                </td>
                                                <td style="text-align: left; width: 20%;">
                                                    <asp:TextBox ID="txttoproject" runat="server" CssClass="textBox" TabIndex="1" Style="width: 99%;" />

                                                    <asp:HiddenField ID="HtoProjectId" runat="server"></asp:HiddenField>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" UseContextKey="True"
                                                        TargetControlID="txttoProject" ServicePath="AutoComplete.asmx" ServiceMethod="GetMyProjectCompletionListwithInitial"
                                                        OnClientShowing="ClientToPopulated" OnClientItemSelected="OntoSelected" MinimumPrefixLength="1"
                                                        CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                        CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender2" CompletionListElementID="pnltopProject">
                                                    </cc1:AutoCompleteExtender>
                                                    <asp:Panel ID="pnltopProject" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden;" />
                                                    <asp:Button Style="display: none" ID="btnSettoProject" runat="server" Text=" Project" CssClass="btn btnnew" />
                                                </td>
                                                
                                            </tr>
                                            <tr><td></td></tr>
                                            <tr>
                                                <td style="text-align: right; color: Black; width: 12%;">With Edit Checks*:
                                                </td>
                                                <td style="width: 9%;">
                                                    <asp:RadioButton runat="server" ID="rblCopyEdit" Text="Yes" GroupName="EditCheck" />
                                                    <asp:RadioButton runat="server" ID="rblCopyEditCheck" Text="No" GroupName="EditCheck" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <div style="text-align: center; margin-top: 2%;">
                                                        <asp:Button ID="btncopyproject" runat="server" Text="Copy Project"
                                                            CssClass="btn btnnew"
                                                            Font-Bold="False" Style="font-weight: bold;"
                                                            ToolTip="Copy project structure" OnClientClick="return Validation();" />
                                                        <asp:Button ID="btncancel" runat="server" Text="Cancel" CssClass="btn btncancel"
                                                            Font-Bold="False" Style="font-weight: bold;" ToolTip="Cancel" OnClientClick="ResetPage();" />
                                                        <asp:Button ID="btnexit" runat="server" Text="Exit" CssClass="btn btnexit"
                                                            Font-Bold="False" Style="font-weight: bold;" ToolTip="Exit"
                                                            OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
                                                    </div>

                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Button runat="server" ID="btnGetEditChecks" Style="display: none;" />
    <asp:Button runat="server" ID="btnTemplateValidation" Style="display: none;" />
    <asp:HiddenField runat="server" ID="hdnIsEditCheck" />
    <asp:HiddenField runat="server" ID="hdnValidationCount" />




    <script type="text/javascript" language="javascript">

        function Display(control, target) {
            if (control.src.toString().toUpperCase().search("EXPAND") != -1) {
                $("#" + target).slideToggle(600);
                control.src = "images/panelcollapse.png";
            }
            else {
                $("#" + target).slideToggle(600);
                control.src = "images/panelexpand.png";
            }
        }

        function ClientFromPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtfromProject.ClientID%>'));
        }

        function OnFromSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtfromProject.ClientID%>'),
            $get('<%= HfromProjectId.ClientID%>'), document.getElementById('<%= btnSetfromProject.ClientId %>'));

            var btn = document.getElementById('<%= btnGetEditChecks.ClientId%>');
            btn.click();
        }

        function ClientToPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender2', $get('<%= txttoproject.ClientID%>'));
        }

        function OntoSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txttoproject.ClientID%>'),
            $get('<%= HtoProjectId.ClientID%>'), document.getElementById('<%= btnSettoProject.ClientID%>'));


            var btn = document.getElementById('<%= btnTemplateValidation.ClientID%>');
            btn.click();

        }

        function Validation() {
            var ToProjectId = document.getElementById('<%= HtoProjectId.ClientID%>');
            var FromProjectId = document.getElementById('<%= HfromProjectId.ClientID%>');
            var FromProjectText = document.getElementById('<%= txtfromProject.ClientID%>');
            var ToProjectText = document.getElementById('<%= txttoproject.ClientID%>');


            if (FromProjectId != null) {
                if (FromProjectId.value == '') {
                    msgalert('Please select Project Name/Request Id*(From) !');
                    return false;
                }
                else if (FromProjectText.value == '') {
                    msgalert('Please select Project Name/Request Id*(From) !');
                    return false;
                }
            }
            else {
                msgalert('Please select Project Name/Request Id*(From) !');
                return false;
            }

            if (ToProjectId != null) {
                if (ToProjectId.value == '') {
                    msgalert('Please select Project Name/Request Id*(To) !');
                    return false;
                }
                else if (ToProjectText.value == '') {
                    msgalert('Please select Project Name/Request Id*(To) !');
                    return false;
                }
            }
            else {
                msgalert('Please select Project Name/Request Id*(ToS) !');
                return false;
            }


            if (!($('#ctl00_CPHLAMBDA_rblCopyEdit').is(':checked') || $('#ctl00_CPHLAMBDA_rblCopyEditCheck').is(':checked'))) {
                msgalert('Please Select Copy Edit checks !');
                return false;
            }

            if ($('#ctl00_CPHLAMBDA_rblCopyEdit').is(':checked')) {
                if (document.getElementById('<%= hdnIsEditCheck.ClientID()%>').value == "NO") {
                    msgalert("There are no edit check found to copy from Project !")
                    return false;
                }
            }

            var validation = document.getElementById('<%= hdnValidationCount.ClientID%>');
            if ($('#ctl00_CPHLAMBDA_rblCopyEdit').is(':checked') && validation.value == "FALSE") {
                msgalert("T1 Template of both the projects are diffrent. You can't copy edit checks !")
                return false
            }
        }

        function ResetPage() {

            var i = 0;
            document.getElementById('<%= HtoProjectId.ClientID%>').value = "";
            document.getElementById('<%= HfromProjectId.ClientID%>').value = "";
            i = 1;
            document.getElementById('<%= txtfromProject.ClientID%>').value = "";
            document.getElementById('<%= txttoproject.ClientID%>').value = "";

            return false;
        }

    </script>

</asp:Content>
