<%@ page language="VB" masterpagefile="~/ECTDMasterPage.master" autoeventwireup="false" inherits="frmCRFActivityStatusReport, App_Web_qa4vhgvp" validaterequest="false" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <link rel="stylesheet" type="text/css" href="App_Themes/jqueryui.css" />
    <link rel="stylesheet" href="App_Themes/CDMS.css" />
    
    <style type="text/css">
        ul { list-style: none; }
        #ctl00_CPHLAMBDA_tvSubject ul { padding-left: 0Px !important; }
        #ctl00_CPHLAMBDA_tvActivity ul { padding-left: 0Px !important; }
        .FieldSetBox { border: #aaaaaa 1px solid; z-index: 0px; border-radius: 4px; }

        .GridFooterColor {
            color: white !important;
            font-weight:bold !important;
        }

    </style>
 
    <table style="width: 80%">
        <tr>
            <td>
                <asp:UpdatePanel ID="upcontrols" runat="server">
                    <ContentTemplate>
                        <fieldset class="FieldSetBox">
                            <legend class="LegendText" style="color: Black">
                                <img id="imgfldgen" alt="CRF Activity Status Report" src="images/panelcollapse.png"
                                    onclick="displayCRFInfo(this,'tblEntryData');" runat="server" style="margin-right: 2px;" />CRF
                                Activity Detail </legend>
                            <div id="tblEntryData">
                                <table style="width: 100%" cellpadding="5px">
                                    <tr>
                                        <td class="LabelText" nowrap="nowrap" style="text-align: right; width: 25%">
                                            Project Name/Request Id*:
                                        </td>
                                        <td class="Label" colspan="4" style="text-align: left">
                                            <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="89%" TabIndex="1"></asp:TextBox>
                                            <asp:CheckBox ID="chkAll" runat="server" Text="All" Style="display: none" />
                                            <asp:Button Style="display: none" ID="btnSetProject" runat="server" Text=" Project">
                                            </asp:Button>
                                            <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="HProjectNo" runat="server" />
                                            <asp:HiddenField ID="HallChildVWorkSpaceId" runat="server" />
                                            <asp:HiddenField ID="HCworkSpaceType" runat="server" />
                                            <asp:HiddenField ID="hType" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="hIsCtmParentProject" runat="server" />
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                                CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                                OnClientShowing="ClientPopulated" ServiceMethod="GetMyProjectCompletionList"
                                                ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True"
                                                CompletionListElementID="pnlProjectList">
                                            </cc1:AutoCompleteExtender>
                                            <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto;
                                                overflow-x: hidden" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="LabelText" nowrap="nowrap" style="text-align: right;">
                                             Period:
                                        </td>
                                        <td class="Label" nowrap="nowrap" style="text-align: left">
                                            <asp:DropDownList ID="ddlPeriods" CssClass="EntryControl" runat="server" AutoPostBack="True"
                                                Style="width: 200px" TabIndex="2">
                                            </asp:DropDownList>
                                        </td>
                                        <td id="tdtypeLabel" runat="server" class="LabelText" nowrap="nowrap" style="text-align: right;">
                                            Activity Type*:
                                        </td>
                                        <td id="tdtypeddl" runat="server" class="Label" nowrap="nowrap" style="text-align: left">
                                            <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="true" CssClass="EntryControl"
                                                onchange="validationNew()" Width="250px" TabIndex="3">
                                                <asp:ListItem Value="0">Please Select Activity Type</asp:ListItem>
                                                <asp:ListItem Value="1">Subject Specific Activity</asp:ListItem>
                                                <asp:ListItem Value="2">Generic Activity</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="LabelText" nowrap="nowrap" style="text-align: right;">
                                            <asp:Label ID="lblFilter" class="LabelText" runat="server" Text="Activity Status" Width="100px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlFilter" AutoPostBack="true" runat="server" Style="vertical-align: top; width: 200px;"
                                                TabIndex="6" CssClass="EntryControl">
                                                <%--<asp:ListItem Value="0" Text="All"></asp:ListItem>
                                                <asp:ListItem Value="1">Data Entry Pending</asp:ListItem>
                                                <asp:ListItem Value="2">Data Entry Continue</asp:ListItem>
                                                <asp:ListItem Value="3">Ready For Review</asp:ListItem>
                                                <asp:ListItem Value="4">First Review Done</asp:ListItem>
                                                <asp:ListItem Value="5">Second Review Done</asp:ListItem>
                                                <asp:ListItem Value="6">Final Reviewed & Freeze</asp:ListItem>--%>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="tdHRUpper" runat="server" colspan="4" style="display: none">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td valign="top" style="align: right">
                                            <fieldset id="divSubject" runat="server" class="FieldSetBox" style="overflow: auto;
                                                display: none; max-height: 230px;" tabindex="5">
                                                <asp:TreeView ID="tvSubject" Width="10px" Height="15px" ShowCheckBoxes="All" BorderColor="DarkGreen"
                                                    Font-Bold="True" Font-Size="X-Small" runat="server">
                                                </asp:TreeView>
                                                <asp:HiddenField ID="SubjectCount" runat="server" />
                                            </fieldset>
                                        </td>
                                        <td colspan="3" valign="top">
                                            <fieldset id="divActivity" runat="server" class="FieldSetBox" style="overflow: auto;
                                                display: none; max-height: 230px;" tabindex="6">
                                                <asp:TreeView ID="tvActivity" runat="server" BorderColor="DarkGreen" Font-Bold="True"
                                                    Font-Size="X-Small" Height="250px" ShowCheckBoxes="All" Width="100px">
                                                </asp:TreeView>
                                                <asp:HiddenField ID="ActivityCount" runat="server" />
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="tdHRLower" runat="server" colspan="4" style="display: none">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center;" class="Label" colspan="100%">
                                            

                                            <asp:Button ID="btnGo" runat="server" OnClientClick="return Validation();" CssClass="btn btngo"
                                                Text="" ToolTip="Go" Style="font-size: 12px !important;" TabIndex="7"/>
                                            <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" Text="Cancel" ToolTip="Cancel" TabIndex="8" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                        <fieldset class="FieldSetBox" id="fldgrdParent" runat="server" style="display: none;
                            margin-top: 20px">
                            <legend class="LegendText" style="color: Black">
                                <img id="imgfldGrid" alt="CRF Activity Status Report" src="images/panelcollapse.png"
                                    onclick="displayCRFInfo(this,'tblGrid');" runat="server" style="margin-right: 2px;" />CRF
                                Activity Status Report</legend>
                            <div id="tblGrid">
                                <table style="width: 100%" cellpadding="5px">
                                    <tr>
                                        <td valign="top" colspan="2">
                                            <asp:UpdatePanel ID="updgrd" runat="server">
                                                <ContentTemplate>
                                                    <div style="float: right; width: 100%; text-align: right; margin-bottom: 10px; margin-top: -10px;">
                                                        <%--<asp:Button ID="btnExportAll" runat="server" ToolTip="Export to Excel"  CssClass="btn btnexcel"
                                                              style="float:left; font-size: 12px !important;"/>--%>
                                                        <button type="button" id="btnselectoption" onclick="Openoption(); return false;" style="float:left; font-size: 12px !important; " class="btn btnexcel" ></button>
                                                        <asp:Label ID="lblimg" runat="server" Text="Legends  ">
                                                            <img id="imgActivityLegends" src="images/question.gif" alt="Legend" runat="server" /></asp:Label>
                                                    </div>
                                                    <div style="font-size: 8pt; display: none; width: 80%; text-align: left; float: right;
                                                        margin-bottom: 10px;" class="FieldSetBox" id="divActivityLegends" runat="server">
                                                        <asp:PlaceHolder ID="PhlReviewer" runat="server" EnableViewState="true"></asp:PlaceHolder>
                                                        <%--<asp:Label ID="lblRed" runat="Server" BackColor="red">&nbsp;&nbsp;&nbsp;</asp:Label>-Data
                                                        Entry Pending,
                                                        <asp:Label ID="lblOrange" runat="Server" BackColor="orange">&nbsp;&nbsp;&nbsp;</asp:Label>-Data
                                                        Entry Continue,
                                                        <asp:Label ID="lblBlue" runat="Server" BackColor="blue">&nbsp;&nbsp;&nbsp;</asp:Label>-
                                                        Ready For Review,
                                                        <asp:Label ID="lblYellow" runat="Server" BackColor="#50C000">&nbsp;&nbsp;&nbsp;</asp:Label>-
                                                        First Review Done,
                                                        <asp:Label ID="lblGreen" runat="Server" BackColor="#006000">&nbsp;&nbsp;&nbsp;</asp:Label>-
                                                        Second Review Done,
                                                        <asp:Label ID="lblGray" runat="Server" BackColor="gray">&nbsp;&nbsp;&nbsp;</asp:Label>-Final Reviewed & Freeze--%>
                                                    </div>
                                                    <%--<div style="margin-top:5px;margin-bottom:5px;"><hr /></div>--%>
                                                    <div style="overflow: auto; height: 380px; margin: auto; width: 100%;">
                                                        <asp:GridView ID="grdParent" Width="100%" runat="server" AutoGenerateColumns="false"
                                                            CellPadding="4" ShowFooter="true" ForeColor="#333333" GridLines="Vertical">
                                                            <FooterStyle BackColor="#1560A1" ForeColor="White" Font-Bold="True" HorizontalAlign="Center" />
                                                            <RowStyle ForeColor="#333333" BackColor="#F7F6F3" />
                                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="white" />
                                                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Height="20px" ForeColor="White"  />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="SubjectNo\<br>ScreenNo">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSubjectId" runat="server" Text='<%# Eval("vMySubjectNo") %>'>
                                                                        </asp:Label>
                                                                        <asp:HiddenField ID="hvSubjectId" runat="server" Value='<%# Eval("vSubjectId") %>' />
                                                                        <asp:HiddenField ID="hcRejectionFlag" runat="server" Value='<%# Eval("cRejectionFlag") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="100px" />
                                                                    <HeaderStyle BackColor="#1560A1" />
                                                                    <FooterStyle BackColor="#1560A1" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Activity Status">
                                                                    <ItemStyle Width="500px" />
                                                                    <HeaderStyle BackColor="#1560A1" />
                                                                    <FooterStyle BackColor="#1560A1" />
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btnShow" runat="server" ImageUrl="~/images/expand.PNG" ToolTip="Show Details" />
                                                                        <asp:ImageButton ID="btnHide" runat="server" ImageUrl="~/images/collapse.PNG" ToolTip="Hide Details" />
                                                                        <asp:Label ID="lblActivity" runat="server" Text=""></asp:Label>
                                                                        <div id="divGrid" style="overflow: auto; max-height: 200px;" runat="server">
                                                                            <asp:Panel ID="pnlGrid" runat="server" BorderColor="Black">
                                                                                <asp:GridView ID="grdChild" OnRowDataBound="grdChild_RowDataBound" runat="server"
                                                                                    AutoGenerateColumns="False" ShowFooter="True" DataKeyNames="ChildId" AllowSorting="True"
                                                                                    CellPadding="3" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                                                                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="Parent" HeaderText="Parent Activity\Visit">
                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                        </asp:BoundField>
                                                                                        <asp:TemplateField ItemStyle-Font-Bold="true" HeaderText="Activity">
                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                            <ItemTemplate>
                                                                                                <asp:LinkButton ID="lnkActivity" OnClientClick="return Redirect(this);" CausesValidation="false"
                                                                                                    runat="server" Text='<%#Eval("Child") %>'></asp:LinkButton>
                                                                                                <asp:HiddenField ID="hId" runat="server" Value='<%#Eval("vWorkSpaceid").ToString()+";"+Eval("vActivityId").ToString()+";"+Eval("parentid").ToString()+";"+Eval("iPeriod").ToString()+";"+Eval("vSubjectId").ToString()+";"+Eval("vMySubjectNo").ToString()+";"+Eval("childid").ToString()+";"+Eval("iMySubjectNo").ToString() %>' />
                                                                                                <asp:HiddenField ID="hdnParent" runat="server" Value='<%#Eval("Parent").ToString()%>' />
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle Font-Bold="True" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:BoundField HeaderText="DCF" DataField="DCF" />
                                                                                        <asp:TemplateField Visible="false">
                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label runat="server" ID="lblStatus" Text='<%#Eval("Status") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                    <FooterStyle BackColor="#CCCC99" />
                                                                                    <RowStyle ForeColor="#000066" />
                                                                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                                                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                                                                                    <HeaderStyle BackColor="#1560A1" Font-Bold="True" ForeColor="White" />
                                                                                </asp:GridView>
                                                                            </asp:Panel>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField HeaderText="Data Entry Pending" HeaderStyle-BackColor="Red" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="Red"
                                                                    ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center" DataField="DEP">
                                                                    <HeaderStyle BackColor="Red" />
                                                                    <FooterStyle BackColor="Red" />
                                                                    <ItemStyle ForeColor="Red" HorizontalAlign="Center" Width="60px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField HeaderText="Data Entry Continue" HeaderStyle-BackColor="Orange" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="Orange"
                                                                    ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center" DataField="DEC">
                                                                    <HeaderStyle BackColor="Orange" />
                                                                    <FooterStyle BackColor="Orange" />
                                                                    <ItemStyle ForeColor="Orange" HorizontalAlign="Center" Width="60px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField HeaderText="Ready For Review" HeaderStyle-BackColor="Blue" ItemStyle-ForeColor="Blue" ItemStyle-Width="60px"
                                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" DataField="FRP">
                                                                    <HeaderStyle BackColor="Blue" />
                                                                    <FooterStyle BackColor="Blue" />
                                                                    <ItemStyle ForeColor="Blue" HorizontalAlign="Center" Width="60px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField HeaderText="First Review Done" HeaderStyle-BackColor="#50C000" ItemStyle-ForeColor="#50C000" ItemStyle-Width="60px"
                                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" DataField="SRP">
                                                                    <HeaderStyle BackColor="#50C000" />
                                                                    <FooterStyle BackColor="#50C000" />
                                                                    <ItemStyle ForeColor="#50C000" HorizontalAlign="Center" Width="60px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField HeaderText="Second Review Done" HeaderStyle-BackColor="#006000" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="#006000"
                                                                    ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center" DataField="FnlRP">
                                                                    <HeaderStyle BackColor="#006000" />
                                                                    <FooterStyle BackColor="#006000" />
                                                                    <ItemStyle ForeColor="#006000" HorizontalAlign="Center" Width="60px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField HeaderText="Final Reviewed & Freeze" HeaderStyle-BackColor="Gray" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="Gray"
                                                                    ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center" DataField="Locked">
                                                                    <HeaderStyle BackColor="Gray" />
                                                                    <FooterStyle BackColor="Gray" />
                                                                    <ItemStyle ForeColor="Gray" HorizontalAlign="Center" Width="60px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField HeaderText="DCF" ItemStyle-Font-Bold="true" DataField="DCF" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemStyle Font-Bold="True" ForeColor="Red" />
                                                                    <HeaderStyle BackColor="#1560A1" />
                                                                    <FooterStyle BackColor="#1560A1" />
                                                                </asp:BoundField>
                                                            </Columns>
                                                            <FooterStyle ForeColor="White" />
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                        </asp:GridView>
                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
                                                  <%--  <asp:PostBackTrigger ControlID="btnExportAll" />--%>
                                                </Triggers>
                                            </asp:UpdatePanel>
                                            <asp:HiddenField ID="hdnCheckedSubject" runat="server" />
                                             <asp:HiddenField runat="server" ID="hdnSubjectNo"/>
                                             <asp:HiddenField runat="server" ID="hdnDEP"/>
                                             <asp:HiddenField runat="server" ID="hdnDEC"/>
                                             <asp:HiddenField runat="server" ID="hdnFRP"/>
                                             <asp:HiddenField runat="server" ID="hdnSRP"/>
                                             <asp:HiddenField runat="server" ID="hdnFnlRP"/>
                                             <asp:HiddenField runat="server" ID="hdnLocked"/>
                                             <asp:HiddenField runat="server" ID="hdnDCF"/>
                                             <asp:HiddenField runat="server" ID="hdnexport"/>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
      <div  runat="server">
        <asp:GridView  ID="gvExport" runat="server" AutoGenerateColumns="true" Style="display: none"
            HeaderStyle-BackColor="#1560a1" HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" Header-style-font-size="10px !importnat" HeaderStyle-Font-Names="Verdana, Arial, Helvetica, sans-serif"
            HeaderStyle-Font-Size=" 0.9em"
            HeaderStyle-HorizontalAlign="Center" CellPadding="3" CellSpacing="0"
            RowStyle-Font-Names=" Verdana, Arial, Helvetica, sans-serif" RowStyle-Font-Size="10pt"
            RowStyle-BackColor="#84c8e6" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#191970" RowStyle-ForeColor="#191970">
        </asp:GridView>
         </div>
    <button id="btn" runat="server" style="display: none;" />
    <cc1:ModalPopupExtender ID="MPExport" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="Img2" PopupControlID="Divexport" BehaviorID="MPExport" 
        TargetControlID="btn">
    </cc1:ModalPopupExtender>
    <div id="Divexport" runat="server" style="position: relative; display: none; background-color: white; padding: 5px; left: 75px; width: 400px; height: 200px; border: dotted 1px gray;"
        class="centerModalPopup">
        <div>
            <img id="Img2" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px;"
                title="Close" />

            <asp:Label ID="Label1" runat="server" Text="Export To Excel" class="LabelText"
                Style="font-size: 12px !important;" ForeColor="Black" Font-Bold="true" align="center" />
            <br />
            <hr />
        </div>
        <div>
            <table style="width: 100%;">
                <tbody>
                    <tr>
                        <td align="center" colspan="5" style="width: 500px;">
                            <div style="padding-top: 1%">
                                <table style="width: 100%;">
                                   <tr>
                                       <td>
                                           <asp:RadioButtonList ID="rbtTipi" runat="server" onclick="resetFun();" Style="margin-top: 4px; margin-bottom: 8px;">
                                               <asp:ListItem Value="0">&nbsp; CRF Activity Report (Summary)</asp:ListItem>
                                               <asp:ListItem Value="1">&nbsp; CRF Activity Report (Detail)</asp:ListItem>
                                           </asp:RadioButtonList>
                                       </td>
                                      </tr>
                                   <tr>
                                       <td style="text-align:center">
                                           <asp:Button ID="btnExportAll" runat="server" ToolTip="Export to Excel"  CssClass="btn btnexcel"
                                                              style="font-size: 12px !important;" OnClientClick="return checkoption();"/>
                                       </td>
                                   </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.min.js"></script>
    
    <script type="text/javascript" language="javascript">

        jQuery(window).focus(function () {
            ChangeColor();
        });

        function ChangeColor() {
            if ($("#ctl00_CPHLAMBDA_grdParent").length > 0) {
                $("#ctl00_CPHLAMBDA_grdParent tr").last().find("td").css({ 'color': 'white' });
            }
        }

        $(document).ready(function () {
            $('#divActivityLegends').css('display', 'none');
        })
                 
        function OpenWindow(Path) {
            window.open(Path);
            return false;
        }


        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {

            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        function validationNew() {

            if (document.getElementById('<%=ddlType.ClientID %>').selectedIndex > 0) {
                if (document.getElementById('<%=HProjectId.ClientId %>').value.toString().trim().length <= 0) {
                    msgalert('Enter Project First !');
                    return false;
                }

                return true;
            }
        }

        function Validation() {
            var dataSubject = parseInt($("#<%=tvSubject.ClientID %> .AspNet-TreeView-Leaf").length) + 1;
            var dataActivity = parseInt($("#<%=tvActivity.ClientID %> .AspNet-TreeView-Leaf").length) + parseInt($("#<%=tvActivity.ClientID %> .AspNet-TreeView-Parent").length) + 1;

            var i = 0;
            var checked = false;
            var checkedAct = false;

            if (document.getElementById('<%= HProjectId.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Project !');
                return false;
            }

            if (document.getElementById('<%=hType.ClientID %>').value == "") {

            } else {
                if (document.getElementById('<%= ddlType.ClientId %>').selectedIndex == 0) {
                    msgalert('Please Select Activity Type !');
                    return false;
                }
            }

            if ($("#ctl00_CPHLAMBDA_ddlType [selected=selected]").val().trim() == "1") {
                if ($("#ctl00_CPHLAMBDA_tvSubject [type=checkbox]:checked").length == 0) {
                    msgalert("Please Select Subject !");
                    return false;
                }
            }

            if ($("#ctl00_CPHLAMBDA_tvActivity [type=checkbox]:checked").length == 0) {
                msgalert("Please Select Activity !");
                return false;
            }                             
            return true;
        }

        function Show(ctrl) {

            var id = ctrl.id;
            var i = 0;
            id = id.replace('_chkSelect', '');


            if (ctrl.checked == true) {
                if (document.getElementById(id + '_grdChild') == null) {
                    msgalert('No Data Found !');
                    ctrl.checked = false;
                    return;
                }
                i = document.getElementById(id + '_grdChild').rows.length;
                document.getElementById(id + '_grdChild').style.display = 'block';

                if ((i * 25) < 250) {
                    document.getElementById(id + '_pnlGrid').style.height = 25 * i + 'px';
                }
                else {
                    document.getElementById(id + '_pnlGrid').style.height = '250px';
                    //return true;
                }
            }
            else {
                document.getElementById(id + '_grdChild').style.display = 'none';

                document.getElementById(id + '_pnlGrid').style.height = '0px';
            }

        }


        function Redirect(ctrl) {
            var id = ctrl.id;
            var workspaceId = document.getElementById('ctl00_CPHLAMBDA_HProjectId').value;
            id = id.replace('_lnkActivity', '');
            var str = document.getElementById(id + '_hId').value.split(";");
            // Added by Rahul Rupareliya
            var str1 = document.getElementById(id + '_hdnParent').value
            // Edned
            var strRedirect;
            if (document.getElementById('<%=hType.ClientID %>').value == "") {
                strRedirect = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + str[0] +
                                     "&ActivityId=" + str[1] + "&NodeId=" + str[6] +
                                     "&PeriodId=" + str[3] + "&SubjectId=" + str[4] +
                                     "&ScreenNo=" + str[5] + "&MySubjectNo=" + str[7] + "&SubNo=" +
                                     "&Activityname=" + str1;

            }
            else {
                strRedirect = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + str[0] +
                                     "&ActivityId=" + str[1] + "&NodeId=" + str[6] +
                                     "&PeriodId=" + str[3] + "&SubjectId=" + str[4] +
                                     "&Type=" + document.getElementById('<%=hType.ClientID %>').value +
                                     "&ScreenNo=" + str[5] + "&MySubjectNo=" + str[7] + "&SubNo=" +
                                     "&Activityname=" + str1;
            }




            window.open(strRedirect);
            return false;
        }


        function RefreshPage() {
            var btn = document.getElementById('<%= btnGo.ClientId %>');
            btn.click();
        }

        function displayCRFInfo(control, target) {

            if (control.src.toString().toUpperCase().search("EXPAND") != -1) {
                $("#" + target).slideToggle(400);
                control.src = "images/panelcollapse.png";
            }
            else {
                $("#" + target).slideToggle(400);
                control.src = "images/panelexpand.png";
            }
        }

        function ChangeUrl(page, url) {
            if (typeof (history.pushState) != "undefined") {
                var obj = { Page: page, Url: url };
                history.pushState(obj, obj.Page, obj.Url);
            } else {
                msgalert("Browser does not support HTML5 !");
            }
        }

        //$('[type=submit]').attr("class", "button");

        //Add by shivani for check all checkbox
        $("[id*=tvSubject] input[type=checkbox]").live("click", function () {
            var table = $(this).closest("table");
            var Flag = false;
            var Index = 0;
            var IndexNot = 0;
            if (table.next().length > 0 && table.next()[0].tagName == "DIV") {
                //Is Parent CheckBox
                var childDiv = table.next();
                var isChecked = $(this).is(":checked");
                $("input[type=checkbox]", childDiv).each(function () {
                    if (isChecked) {
                        $(this).attr("checked", "checked");
                    } else {
                        $(this).removeAttr("checked");
                    }
                });
            } else {
                //Is Child CheckBox
                var parentDIV = $(this).closest("DIV");
                if ($("input[type=checkbox]", parentDIV).length == $("input[type=checkbox]:checked", parentDIV).length) {
                    $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");
                }
                else {
                    $("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
                }
            }
        });

        //Add by shivani for check all checkbox
        $("[id*=tvActivity] input[type=checkbox]").live("click", function () {
            var table = $(this).closest("table");
            var Flag = false;
            var Index = 0;
            var IndexNot = 0;
            var IndexAll = 0;
            if (table.next().length > 0 && table.next()[0].tagName == "DIV") {
                //Is Parent CheckBox
                var childDiv = table.next();
                var isChecked = $(this).is(":checked");
                $("input[type=checkbox]", childDiv).each(function () {
                    if (isChecked) {
                        $(this).attr("checked", "checked");
                    } else {
                        $(this).removeAttr("checked");
                    }
                });
            } else {
                //Is Child CheckBox
                var parentDIV = $(this).closest("DIV");
                $(this).closest("DIV").find("table [type=checkbox]").each(function () {
                    if ($(this).attr("checked") == true) {
                        Flag = true;
                        Index = Index + 1;
                    }
                    if ($(this).attr("checked") == false) {
                        IndexNot = IndexNot + 1;
                    }
                });
                if (Flag == true) {
                    parentDIV.prev().find("[type=checkbox]").attr("checked", "checked");
                }
                if ($(this).closest("DIV").find("table [type=checkbox]").length == Index) {
                    parentDIV.prev().find("[type=checkbox]").removeAttr("checked");
                    parentDIV.prev().find("[type=checkbox]").attr("checked", "checked");

                }
                if ($(this).closest("DIV").find("table [type=checkbox]").length == IndexNot) {
                    parentDIV.prev().find("[type=checkbox]").removeAttr("checked");
                    parentDIV.prev().find("[type=checkbox]").removeAttr("checked");
                }
                $("#ctl00_CPHLAMBDA_tvActivity").find("table [type=checkbox]").each(function () {
                    if ($(this).find("table [type=checkbox]").attr("checked") == true) {
                        IndexAll = IndexAll + 1;
                    }
                });
                if ($("#ctl00_CPHLAMBDA_tvActivity").find("table [type=checkbox]").length == IndexAll) {
                    $("#ctl00_CPHLAMBDA_tvActivity").find("table [type=checkbox]").first().attr("checked", "checked");
                } else {
                    $("#ctl00_CPHLAMBDA_tvActivity").find("table [type=checkbox]").first().removeAttr("checked")
                }
            }
        });

        function Openoption()
        {
            $('#ctl00_CPHLAMBDA_hdnexport').val("");
            var elementRef = document.getElementById('<%= rbtTipi.ClientID%>');
            var inputElementArray = elementRef.getElementsByTagName('input');

            for (var i = 0; i < inputElementArray.length; i++) {
                var inputElement = inputElementArray[i];

                inputElement.checked = false;
            }
            var modal = $find("MPExport");
            modal.show();
        }
        function resetFun()
        {
            var radioButtons = $('#<%=rbtTipi.ClientID%>');
            var id = radioButtons.find('input:checked').val();
           
            $('#ctl00_CPHLAMBDA_hdnexport').val(id);
        }
        function checkoption()
        {
            var id = $('#ctl00_CPHLAMBDA_hdnexport').val();
            if (id == "") {
                msgalert('Please,select report type !');
                return false;
            }
            var modal = $find("MPExport");
            modal.hide();

        }

    </script>      
</asp:Content>
