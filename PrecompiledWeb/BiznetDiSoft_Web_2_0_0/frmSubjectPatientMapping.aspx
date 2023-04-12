<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmSubjectPatientMapping, App_Web_l40sj1d0" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <link rel="stylesheet" href="../App_Themes/CDMS.css" />

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" src="Script/General.js"></script>

    <script type="text/javascript" src="Script/Gridview.js"></script>

    <link href="App_Themes/StyleBlue/StyleBlue.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" language="javascript">

        ////// For Project    
        function ClientPopulated(sender, e) {

            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {

            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        $(document).ready(function() {
            $('#canal').css('display', 'none');
        })
        function UncheckOthers(obj) {

            var targetClass = obj.value;

            $('input:radio').each(function() {

                if ($(this).attr('value') == targetClass) {
                    $(this).attr('checked', false);
                }

            })
            document.getElementById(obj.id).checked = true;
            //var allRadios= document.getElementsByClassName(targetClass);

        }
      
    </script>

    <asp:UpdatePanel ID="upSampleSubjectMapping" runat="server">
        <ContentTemplate>
            <table cellpadding="0" width="100%">
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        Project :
                        <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="510px" TabIndex="1"></asp:TextBox>
                        <br />
                        <asp:Button Style="display: none" ID="btnSetProject" runat="server"></asp:Button><asp:HiddenField
                            ID="HProjectId" runat="server"></asp:HiddenField>
                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="False"
                            TargetControlID="txtProject" ServicePath="AutoComplete.asmx" ServiceMethod="GetMyProjectCompletionList"
                            OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1"
                            CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                            CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1"
                            CompletionListElementID="pnlProjectList">
                        </cc1:AutoCompleteExtender>
                        <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto;
                            overflow-x: hidden" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="height: 20px">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <div>
                            <asp:GridView AllowPaging="true" PageSize="10" runat="server" SkinID="grdViewSmlAutoSize"
                                ID="gvSubjectPatient" CssClass="" AutoGenerateColumns="false" Width="85%">
                                <Columns>
                                    <asp:BoundField HeaderText="Sr. No.">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                    </asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:RadioButton runat="server" ID="rbSubjectID" CssClass="rbSubject" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Subject ID (BizNET)" DataField="Subject">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Left" Width="45%" />
                                    </asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:RadioButton ID="rbPatientID" runat="server" CssClass="rbPatient" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Patient ID (Lab)" DataField="Patient">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="Small" />
                                        <ItemStyle HorizontalAlign="Left" Width="45%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vSubjectID" />
                                    <asp:BoundField DataField="vPatientID" />
                                    <asp:BoundField DataField="LabScreeningNo" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="height: 20px">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <div>
                            <asp:Button ID="btnMap" Visible="false" Text="Map IDs" runat="server" CssClass="btn btnnew" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="height: 20px">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <div>
                            <asp:GridView AllowPaging="true" PageSize="10" runat="server" SkinID="grdViewSmlAutoSize"
                                ID="gvMappedResult" AutoGenerateColumns="false" Visible="false" Width="85%">
                                <Columns>
                                    <asp:BoundField HeaderText="Sr. No.">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Subject ID (BizNET)" DataField="Subject">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Left" Width="45%" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Patient ID (Lab)" DataField="Patient">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Left" Width="45%" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>                                           
                                            <asp:ImageButton ImageUrl="Images/i_delete.gif" runat="server" OnClientClick="return confirm('Are You Sure to delete This Mapping!')"
                                                ID="btnDelete" align="center" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Left" Width="5%" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="vSubjectID" />
                                    <asp:BoundField DataField="vPatientID" />
                                    <asp:BoundField DataField="nSubjectPatientMapping" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="height: 20px">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <div>
                            <asp:Button ID="btnSaveChanges" Text="Save" Visible="false" runat="server" CssClass="btn btnnew" /></div>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnMap" />
            <asp:PostBackTrigger ControlID="btnSaveChanges" />
            <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
