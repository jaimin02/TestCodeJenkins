<%@ page title="" language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="FrmForceCtmMedexInfoHdrDtlunlockForm, App_Web_qa4vhgvp" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <style type="text/css">
        .LabelText {
            text-align: right;
            color: Black;
            font-weight: bold;
            font-family: Delicious, sans-serif !important;
            font-size: 11px;
        }

        .Label1 {
            color: Navy;
            font-weight: bold;
            font-family:Verdana;
            font-size: 11px;
        }
    </style>
    <asp:UpdatePanel runat="server" ID="upSelection" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset class="FieldSetBox" style="width: 69%; margin-left: 14%; margin-top: 2%; padding-right: 7%">
                <table width="90%" style="margin-top: 1%; margin-bottom: 1%; margin-left: 5%">
                    <tr>

                        <td class="Label1">Project\Request ID:
                        </td>
                        <td style="width: 25%; text-align: left">
                            <asp:DropDownList ID="ddlProject" AutoPostBack="true" runat="server" CssClass="dropDownList" ToolTip="Select Department."></asp:DropDownList>
                        </td>

                        <td class="Label1">Period:
                        </td>

                        <td style="width: 25%; text-align: left">
                            <asp:DropDownList ID="ddlPeriod" AutoPostBack="true" Style="text-align: left" runat="server" CssClass="dropDownList" ToolTip="Select Period."></asp:DropDownList>
                        </td>

                    </tr>
                    <tr>
                        <td class="Label1">Subject\ScreenNo:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSubjects" AutoPostBack="true" runat="server" CssClass="dropDownList" ToolTip="Select Forms."></asp:DropDownList>

                        </td>

                        <td class="Label1">Activity: 
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList ID="ddlActivities" Style="width: 141%" AutoPostBack="true" runat="server" CssClass="dropDownList" ToolTip="Select Forms."></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <asp:Button ID="btnUnlock" runat="server" Text="Unlock" CssClass="btn btnnew" Style="margin-top: 2%;" OnClientClick=" return unLock();" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btncancel" Style="margin-top: 2%; margin-right: 5%;" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnUnlock" EventName="click" />
        </Triggers>
    </asp:UpdatePanel>
    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <script language="javascript" type="text/javascript">

        function unLock() {

            if ($('#<%=ddlProject.ClientId%>').val() == 0 || $('#<%=ddlProject.ClientID%>').val()==null) {
                msgalert("Please Select Project !");
                return false;
            }

            if ($('#<%=ddlPeriod.ClientId%>').val() == 0) {
                msgalert("Please Select Period !");
                return false;
            }

            if ($('#<%=ddlSubjects.ClientID%>').val() == 0) {
                msgalert("Please Select Subject !");
                return false;
            }

            if ($('#<%=ddlActivities.ClientID%>').val() == 0) {
                msgalert("Please Select Activity !")
                return false;
            }
        }
    </script>
</asp:Content>

