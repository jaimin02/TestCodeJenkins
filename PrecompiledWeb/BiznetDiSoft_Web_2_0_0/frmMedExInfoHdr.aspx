<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmMedExInfoHdr, App_Web_2mzu20n4" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" Runat="Server">

    <script src="Script/popcalendar.js" type="text/javascript"></script>

    <script src="Script/AutoComplete.js" type="text/javascript"></script>

    <script type="text/javascript">
        function ClientPopulated(sender, e)
            {
                ProjectClientShowing('AutoCompleteExtender1',$get('<%= txtProject.ClientId %>'));
            }
            
            function OnSelected(sender,e)
            {
                ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
                    $get('<%= HProjectId.clientid %>'),document.getElementById('<%= btnProject.ClientId %>') );
              
            }
    </script>

    <table cellpadding="0" cellspacing="0" width="95%">
        <tr>
            <td>
                <table cellpadding="5" width="100%">
                    <tr>
                        <td align="left">
                            <strong>Set Project For Clinical Trial</strong>
                            <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="left">
                <table cellpadding="5">
                    <tr>
                        <td class="Label">
                            Select Project :
                        </td>
                        <td align="left">
                            <%--<asp:DropDownList ID="ddlProject" runat="server" CssClass="dropDownList" AutoPostBack="True">
                            </asp:DropDownList><br />--%>
                            <asp:TextBox ID="txtProject" runat="server" CssClass="textBox" TabIndex="1" Width="622px"></asp:TextBox><br />
                            <asp:Button ID="btnProject" runat="server" Style="display: none" Text=" Project " />
                            <asp:HiddenField ID="HProjectId" runat="server" />
                            <cc1:autocompleteextender id="AutoCompleteExtender1" runat="server" behaviorid="AutoCompleteExtender1"
                                completionlistcssclass="autocomplete_list" completionlisthighlighteditemcssclass="autocomplete_highlighted_listitem"
                                completionlistitemcssclass="autocomplete_listitem" minimumprefixlength="1" onclientitemselected="OnSelected"
                                onclientshowing="ClientPopulated" ServiceMethod="GetProjectCompletionListWithOutSponser" servicepath="AutoComplete.asmx"
                                targetcontrolid="txtProject" usecontextkey="True"></cc1:autocompleteextender>
                        </td>
                        <td class="Label" style="padding-left: 15px">
                            Location :
                        </td>
                        <td align="left">
                            <asp:Label ID="lblLocation" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="Label">
                            Period :
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlPeriod" runat="server" AutoPostBack="True" CssClass="dropDownList"
                                TabIndex="2">
                                <asp:ListItem Value="0007">Period - I</asp:ListItem>
                                <asp:ListItem Value="0021">Period - II</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="Label">
                            Start Date :
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtDate" runat="server" CssClass="textBox" TabIndex="3"></asp:TextBox>
                            <img id="img2" runat="server" align="absMiddle" alt="Select  Date" onclick="popUpCalendar(this,ctl00_CPHLAMBDA_txtDate,'dd-mmm-yyyy');"
                                src="images/calendar.gif" />
                        </td>
                    </tr>
                    <tr>
                        <td class="Label" colspan="2">
                            <asp:Button ID="btnOk" runat="server" CssClass="btn btnnew" TabIndex="4" Text=" OK " />
                            <br />
                            <asp:DropDownList ID="ddlActivity" runat="server" CssClass="dropDownList" Width="656px">
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="GV_Subject" runat="server" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField DataField="vSubjectId" HeaderText="SubjectId" />
                                    <asp:BoundField DataField="vSubjectName" HeaderText="Subject Name" />
                                    <asp:BoundField DataField="vMySubjectNo" HeaderText="My Subject No" />
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LnkEdit" runat="server">Edit</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>

