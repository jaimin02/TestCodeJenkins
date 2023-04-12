<%@ Page Title="" Language="VB" MasterPageFile="~/ECTDMasterPage.master" AutoEventWireup="false" CodeFile="frmVisitSchedulerReport.aspx.vb" Inherits="frmVisitSchedulerReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" Runat="Server">
     <link rel="stylesheet" type="text/css" href="App_Themes/jqueryui.css" />
        <link rel="stylesheet" href="App_Themes/CDMS.css" />
    
    <style type="text/css">
        ul { list-style: none; }
        #ctl00_CPHLAMBDA_tvSubject ul { padding-left: 0Px !important; }
        #ctl00_CPHLAMBDA_tvActivity ul { padding-left: 0Px !important; }
        .FieldSetBox { border: #aaaaaa 1px solid; z-index: 0px; border-radius: 4px; }
        .GridForeColor {
            color:white !important;
        }

    </style>
 <div>
     <center>
         <asp:RadioButtonList runat="server" ID="rbtnType"  RepeatDirection="Horizontal" AutoPostBack="true"  OnSelectedIndexChanged="rbtnType_SelectedIndexChanged" >
             <asp:ListItem Value="0"> Detail</asp:ListItem>
             <asp:ListItem Value="1"> Summary</asp:ListItem>
           </asp:RadioButtonList>
         
     </center>
 </div>
    <div runat="server" id="divSchedulerRrepot" visible="false" >
        <table style="width: 80%">
        <tr>
            <td>
                <asp:UpdatePanel ID="upcontrols" UpdateMode="Conditional"  runat="server">
                    <ContentTemplate>
                        <fieldset class="FieldSetBox">
                            <asp:Button runat="server" ID="hdnbutton" Style="display:none"  OnClick="hdnbutton_Click"/>
                            <legend class="LegendText" style="color: Black">
                                <img id="imgfldgen" alt="Visit Scheduler Report" src="images/panelcollapse.png"
                                    onclick="displayCRFInfo(this,'tblEntryData');" runat="server" style="margin-right: 2px;" />VISIT
                                SCHEDULER DETAIL</legend>
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
                                            </asp:DropDownList>
                                        </td>
                                        <td class="LabelText" nowrap="nowrap" style="text-align: right;">
                                            For UnScheduled ?
                                        </td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="chkUnScheduled" />
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
                                                Text="" ToolTip="Go" Style="font-size: 12px !important;" TabIndex="7" />
                                            <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" Text="Cancel" ToolTip="Cancel"
                                                TabIndex="8" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                        <fieldset class="FieldSetBox" id="fldgrdParent" runat="server" style="display: none;
                            margin-top: 20px">
                            <legend class="LegendText" style="color: Black">
                                <img id="imgfldGrid" alt="Visit Scheduler Report" src="images/panelcollapse.png"
                                    onclick="displayCRFInfo(this,'tblGrid');" runat="server" style="margin-right: 2px;" />Visit Schedule Report</legend>
                            <div id="tblGrid">
                                <table style="width: 100%" cellpadding="5px">
                                    <tr>
                                        <td valign="top" colspan="2">
                                            <asp:UpdatePanel ID="updgrd" runat="server">
                                                <ContentTemplate>
                                                    <div style="float: right; width: 100%; text-align: right; margin-bottom: 10px; margin-top: -10px;">
                                                        <asp:Button ID="btnExportAll" runat="server" ToolTip="Export to Excel"  CssClass="btn btnexcel"
                                                              style="float:left; font-size: 12px !important;"/>
                                                        <asp:Label ID="lblimg" runat="server" Text="Legends  ">
                                                            <img id="imgActivityLegends" src="images/question.gif" alt="Legend" runat="server" /></asp:Label>
                                                    </div>
                                                    <div style="font-size: 8pt; display: none; width: 80%; text-align: left; float: right;
                                                        margin-bottom: 10px;" class="FieldSetBox" id="divActivityLegends" runat="server">
                                                        <asp:PlaceHolder ID="PhlReviewer" runat="server" EnableViewState="true"></asp:PlaceHolder>
                                                    </div>
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
                                                    <asp:PostBackTrigger ControlID="btnExportAll" />
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
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <asp:GridView runat="server" ID="grdExportToExcel" AutoGenerateColumns="false" Style="display:none;">
            <Columns>
                <asp:BoundField HeaderText ="Subject No" DataField="vMySubjectNo"  />
                <asp:BoundField HeaderText ="Randomization No" DataField="vRandomizationNo"  />
                <asp:BoundField HeaderText ="Visit" DataField="parent"  />
                <asp:BoundField HeaderText ="Visit Date" DataField="dActualDate"/>
                    <%--<asp:TemplateField HeaderText="Visit Date">
                    <ItemTemplate>
                    <asp:Label ID="lblDate" runat="server" Text='<%#Eval("dActualDate","{0:dd, MMM yyyy}") %>'/>
                    </ItemTemplate>
                    </asp:TemplateField>--%>
                <asp:BoundField HeaderText ="Activity" DataField="Child"  />
                <asp:BoundField HeaderText ="Status" DataField="STATUS"  />
                <asp:BoundField HeaderText ="DCF" DataField="DCF"  />
                <asp:BoundField HeaderText ="No of days Since Actual Visit" DataField="Diff"  />
            </Columns>
        </asp:GridView>
    </table>
    </div>
    <div runat="server" id="divSummeryRepot" visible="false">
    <asp:UpdatePanel ID="upSummeryReport" UpdateMode="Conditional"   runat="server">
        <ContentTemplate>
            <table style="width: 90%">
                <tr>
                    <td colspan="3" style="height: 10px;"></td>
                </tr>
                <tr>
                    <td class="LabelText" nowrap="nowrap" style="text-align: right; width: 35%;">
                       <asp:Label runat="server"  id ="lblProject" Text="Project Name/Request Id*:"></asp:Label>
                    </td>
                    <td class="Label" style="text-align: left; width: 40%">
                        <asp:TextBox ID="txtSummeryProject" runat="server" CssClass="textBox" Width="92%" TabIndex="1" Style="margin-left: 5px;"></asp:TextBox>
                        <asp:Button Style="display: none" ID="btnSummarySetProject" runat="server" Text=" Project"></asp:Button>
                        <asp:HiddenField ID="hdnSummeryProjectId" runat="server"></asp:HiddenField>
                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" BehaviorID="AutoCompleteExtender2"
                            CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                            CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelectedForSummeryReport"
                            OnClientShowing="ClientPopulatedForSummeryReport" ServiceMethod="GetMyProjectCompletionList"
                            ServicePath="AutoComplete.asmx" TargetControlID="txtSummeryProject" UseContextKey="True"
                            CompletionListElementID="pnlSumnmeryProjectList">
                        </cc1:AutoCompleteExtender>

                        <asp:Panel ID="pnlSumnmeryProjectList" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" />
                    </td>
                    <td class="Label" style="text-align: left; width: 25%">
                        <asp:CheckBox ID="chkParent" runat="server" Text="Include Parent" Style="display: none;" onChange="RemoveGrid();" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="height: 3px;"></td>
                </tr>
                <tr>
                    <td class="LabelText" nowrap="nowrap" style="text-align: right; width: 35%;">Period:
                    </td>
                    <td class="Label" nowrap="nowrap" style="text-align: left; width: 65%;" colspan="2">
                        <asp:DropDownList ID="ddlSummeryPeriod" CssClass="EntryControl" runat="server" AutoPostBack="false"
                            Style="width: 230px; margin-left: 5px;" TabIndex="2" onChange="RemoveGrid();">
                        </asp:DropDownList>
                    </td>

                </tr>
                <tr>
                    <td colspan="3" style="height: 10px;"></td>
                </tr>
                <tr>
                    <td style="width: 35%;"></td>
                    <asp:UpdatePanel ID="upExport" runat="server">
                        <ContentTemplate>
                            <td colspan="2" style="text-align: left; width: 65%;">
                                <asp:Button ID="btnSummerygo" runat="server" OnClientClick="return ValidationForSummery();" CssClass="btn btngo"
                                    Text="" ToolTip="Go" Style="font-size: 10px !important; margin-left: 10px; vertical-align: top;" TabIndex="3" />
                                <asp:Button ID="btnSummeryCancel" runat="server" CssClass="btn btncancel" Text="Cancel" ToolTip="Cancel" OnClick="btnSummeryCancel_Click"
                                    TabIndex="4" />
                                <asp:Button ID="btnExport" OnClick="btnExport_Click"  runat="server" CssClass="btn btnexcel"  ToolTip="Export To Excel"
                                     Style="font-size: 10px !important; display: none;" TabIndex="5" /> 
                            </td>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnExport" />
                        </Triggers>
                    </asp:UpdatePanel>
                </tr>
                <tr>
                    <td colspan="3" style="height: 10px;"></td>
                </tr>
                <tr>
                    <td colspan="3" align="center">
                        <div style="overflow: auto; height: 450px; margin: auto; width: 100%;">
                            <asp:GridView ID="gvActivityCount" runat="server" AutoGenerateColumns="true" ShowFooter="True"
                                CellPadding="3" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Height="20px" ForeColor="White" />
                                <RowStyle ForeColor="#333333" BackColor="#F7F6F3" Font-Bold="true" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <FooterStyle BackColor="#1560A1" ForeColor="White" Font-Bold="True" HorizontalAlign="Center" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            </asp:GridView>
                        </div>
                    </td>

                </tr>
                <tr>
                    <td colspan="3" style="height: 10px;"></td>
                </tr>

            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    </div>

    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.min.js"></script>
    
    <script type="text/javascript" language="javascript">

        jQuery(window).focus(function () {
            ChangeColor();
            ChangeColorForSummery();

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
                    msgalert('Enter Project First');
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
                msgalert('Please Enter Project');
                return false;
            }

            if (document.getElementById('<%=hType.ClientID %>').value == "") {

            } else {
                if (document.getElementById('<%= ddlType.ClientId %>').selectedIndex == 0) {
                    msgalert('Please Select Activity Type');
                    return false;
                }
            }

            if ($("#ctl00_CPHLAMBDA_ddlType [selected=selected]").val().trim() == "1") {
                if ($("#ctl00_CPHLAMBDA_tvSubject [type=checkbox]:checked").length == 0) {
                    msgalert("Please Select Subject");
                    return false;
                }
            }

            if ($("#ctl00_CPHLAMBDA_tvActivity [type=checkbox]:checked").length == 0) {
                msgalert("Please Select Activity");
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
                    msgalert('No Data Found');
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
            var str1 = document.getElementById(id + '_hdnParent').value
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
                msgalert("Browser does not support HTML5.");
            }
        }

        //$('[type=submit]').attr("class", "btn btnsave");
        

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
                var parentDIV = $(this).closest("DIV");
                if ($("input[type=checkbox]", parentDIV).length == $("input[type=checkbox]:checked", parentDIV).length) {
                    $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");
                }
                else {
                    $("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
                }
            }
        });

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

      


        //For Summery Report

        function ClientPopulatedForSummeryReport(sender, e) {
            $get('<%= chkParent.ClientID%>').parentElement.style.display = "none";
              RemoveGrid();
              ProjectClientShowing('AutoCompleteExtender2', $get('<%= txtSummeryProject.ClientId %>'));
        }

        function OnSelectedForSummeryReport(sender, e) {

            ProjectOnItemSelected(e.get_value(), $get('<%= txtSummeryProject.clientid %>'),
            $get('<%= hdnSummeryProjectId.clientid %>'), document.getElementById('<%= btnSummarySetProject.ClientId %>'));
        }

        function ValidationForSummery() {

            if (document.getElementById('<%= hdnSummeryProjectId.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Project.');
                return false;
            }

            return true;
        }

        function RemoveGrid() {
            if ($('#<%=gvActivityCount.ClientID%>').length > 0) $('#<%=gvActivityCount.ClientID%>').remove();
            $get('<%= btnExport.ClientID%>').style.display = "none";
        }

        function ChangeColorForSummery() {
            if ($("#ctl00_CPHLAMBDA_gvActivityCount").length > 0) {
                $("#ctl00_CPHLAMBDA_gvActivityCount tr").last().find("td").css({ 'color': 'white' });
            }
        }

    </script>      
</asp:Content>

