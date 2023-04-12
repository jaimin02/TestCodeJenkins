<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmWorkspaceSubjectMedExInfo, App_Web_qa4vhgvp" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <style>
        .clrwhite {
                color: #fff !important;
        }
    </style>

    <script type="text/javascript" language="javascript">
        function SelectAll(CheckBoxControl) {
            if (CheckBoxControl.checked == true) {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {

                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf('gvWorkSpaceSubjectMst') > -1)) {
                        if (document.forms[0].elements[i].disabled == false) {
                            document.forms[0].elements[i].checked = true;
                        }


                    }


                }

            }

            else {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf('gvWorkSpaceSubjectMst') > -1)) {
                        document.forms[0].elements[i].checked = false;
                    }

                }
            }
        }

        function RefreshPage() {
            window.location.href = window.location.href;
        }

        // added by vishal to open new window faster

        function OpenWindow(Path, Param, subjectId) {
            if (Param == 'R') {
                if (confirm('Subject - ' + subjectId + ' Is Already Rejected, Are You Sure You Want To View Or Edit It Again?')) {
                    window.open(Path);
                    return false;
                }
            }
            else if (Param == 'E') {
                window.open(Path);
                return false;
            }
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
                url: "frmWorkspaceSubjectMedExInfo.aspx/LockImpact",
                data: '{"WorkspaceID":"' + WorkspaceID + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                dataType: "json",
                success: function (data) {
                    if (data.d == "L") {
                        $("#ctl00_CPHLAMBDA_gvWorkSpaceSubjectMst [type=checkbox]").attr("Disabled", "Disabled");
                        $("#ctl00_CPHLAMBDA_gvWorkSpaceSubjectMst [type=image]").attr("Disabled", "Disabled");
                        $("#ctl00_CPHLAMBDA_BtnSave").attr("Disabled", "Disabled");
                    }
                },
                failure: function (response) {
                    alert(response.d);
                },
                error: function (response) {
                    alert(response.d);
                }
            });
            return true;
        }

    </script>

    <table width="100%">
        <tr>
            <td>
                <table style="width: 60%; margin: auto;">
                    <tr>
                        <td class="Label" valign="middle" align="center" style="white-space: nowrap; text-align: right;">
                            <div runat="server" id="canal" style="border: outset 2px black; font: verdana; font-size: 8pt;
                                height: auto; background-color: White; text-align: left;">
                                <asp:Label BackColor="Red" Width="20" Height="10" runat="server" />
                                -Rejected Subject,&nbsp;&nbsp;
                                <asp:Label BackColor="Orange" Width="20" Height="10" runat="server" />-Data Entry
                                Continue,&nbsp;&nbsp;
                                <asp:Label BackColor="Blue" runat="server" Width="20" Height="10" />- Data Entry
                                Completed
                            </div>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%">
                    <tr>
                      <%--  <td style="width: 100%" align="left">--%>
                            <asp:GridView ID="gvWorkSpaceSubjectMst" runat="server" AutoGenerateColumns="False"
                                SkinID="grdViewAutoSizeMax"  AllowPaging="false" style="width:60%; margin:auto;">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <input id="Checkbox1" onclick="SelectAll(this)" type="checkbox">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSave" runat="server" Visible="true"></asp:CheckBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="vSubjectId" HeaderText="Subject Id" />
                                    <asp:BoundField DataField="vMySubjectNo" HeaderText="Subject No" />
                                    <asp:BoundField DataField="vInitials" HeaderText="Subject Initial" />
                                    <asp:BoundField DataField="cRejectionFlag" HeaderText="Rejection Flag" />
                                    <asp:BoundField DataField="iMySubjectNo" HeaderText="MySubject No" />
                                    <asp:TemplateField HeaderText="Save">
                                        <ItemTemplate>
                                            <%--<asp:LinkButton ID="lnkSave" runat="server" Enabled="False">Save</asp:LinkButton>--%>
                                            <asp:ImageButton ID="lnkSave" runat="server" ImageUrl="~/images/save1.png" ToolTip="Save"
                                                Enabled="false" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <%--<asp:LinkButton ID="lnkEdit" runat="server">Edit</asp:LinkButton>--%>
                                            <asp:ImageButton ID="lnkEdit" runat="server" ImageUrl="~/images/Edit2.gif" ToolTip="Edit" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:HiddenField ID="HFWorkspaceId" runat="server" />
                            <asp:HiddenField ID="HFActivityId" runat="server" />
                            <asp:HiddenField ID="HFNodeId" runat="server" />
                            <asp:HiddenField ID="HFPeriodId" runat="server" />
                            <asp:HiddenField ID="HFSubjectId" runat="server" />
                       <%-- </td>--%>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <asp:Button ID="BtnSave" runat="server" CssClass="btn btnsave" Text="Save" ToolTip="Save"
                                Enabled="False" />
                            <asp:Button ID="BtnBack" runat="server" CssClass="btn btnback" Text="" ToolTip="Back" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
