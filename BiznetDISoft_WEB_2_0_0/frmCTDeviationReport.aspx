<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="frmCTDeviationReport.aspx.vb" Inherits="frmCTDeviationReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <style type="text/css">
        table {
            border-collapse: initial;
        }

        #ctl00_CPHLAMBDA_gvIndependentVisit_wrapper {
            width: 100% !important;
        }
    </style>
    <asp:HiddenField ID="hdnSubSelection" runat="server" />

    <div id="divMain" style="width: 100% ! important;">
        <div id="divradiocontent" runat="server">
            <br />
            <asp:RadioButtonList ID="rbtSelectioncontent" runat="server" RepeatDirection="Horizontal"
                Style="margin: auto" AutoPostBack="true">
                <asp:ListItem Selected="True" Value="0">Define Window/Deviation</asp:ListItem>
                <asp:ListItem Value="1">Generate Report</asp:ListItem>

            </asp:RadioButtonList>
        </div>
        <hr />

        <div id="divradioreportmain" runat="server">
            <asp:RadioButtonList ID="rbtreporttype" runat="server" OnSelectedIndexChanged="rbtreporttype_SelectedIndexChanged" RepeatDirection="Horizontal"
                Style="margin: auto" AutoPostBack="true">
                <asp:ListItem Value="0" Selected="True">Based On Parent Visit</asp:ListItem>
                <asp:ListItem Value="1">Independent Visit</asp:ListItem>
            </asp:RadioButtonList>
            <div id="divradioreport" runat="server">
                <table style="width: 100% ! important;">
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="upreport" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table style="width: 100% !important; border: 1px solid darkgray; border-radius: 5px;" align="left" border="0" cellspacing="10px">
                                        <tr>
                                            <td class="Label" style="white-space: nowrap; text-align: right; width: 10%">Project No /Site No:
                                            
                                            </td>

                                            <td style="text-align: left; width: 90%;">
                                                <span style="font-weight: normal">
                                                    <asp:TextBox ID="txtProjectReport" runat="server" CssClass="textBox" Width="50%" onkeydown="return (event.keyCode!=13)" />
                                                </span>
                                                <asp:Button ID="btnSetProjectReport" runat="server" Style="display: none" Text=" Project" />
                                                <asp:HiddenField ID="HReportProjectId" runat="server" />
                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" BehaviorID="AutoCompleteExtender2"
                                                    CompletionListCssClass="pnlprojectlist1" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                    CompletionListElementID="pnlprojectlist1" CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelectedReport"
                                                    OnClientShowing="ClientPopulatedReport" ServiceMethod="GetMyProjectCompletionList"
                                                    ServicePath="AutoComplete.asmx" TargetControlID="txtProjectReport" UseContextKey="True">
                                                </cc1:AutoCompleteExtender>
                                                <asp:Panel ID="pnlprojectlist1" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" BackColor="White" BorderWidth="1" />
                                            </td>

                                        </tr>
                                        <tr>
                                            <td class="Label" style="white-space: nowrap; text-align: right; width: 10%">Subject No:
                                            </td>
                                            <td style="text-align: left; width: 90%;">
                                                <asp:DropDownList ID="ddlSUbject" multiple="multiple" onchange=" fnSubjectSelection();" Style="width: 251px;" runat="server" CssClass="dropDownList">
                                                </asp:DropDownList>
                                            </td>

                                        </tr>
                                        <%--  <tr runat="server" id="trindependantvisit" visible="false" >
                                            
                                            <td  >
                                                
                                                    <asp:DropDownList runat="server" ID="ddlFromVisit" Width="200px" CssClass="dropDownList">
                                                    <asp:ListItem>--From--</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                           
                                            <td >
                                                <asp:DropDownList runat="server" ID="ddlToVisit" Width="200px" CssClass="dropDownList">
                                                    <asp:ListItem>--To--</asp:ListItem>
                                                </asp:DropDownList>

                                            </td>

                                        </tr>--%>
                                        <%-- <tr>
                                            <td>
                                                TO Visit :
                                            </td>
                                            <td style="margin-left: 10px; text-align: left;">
                                               <asp:DropDownList runat="server" ID="ddlFromVisit" Width="200px" CssClass="dropDownList">
                                                    <asp:ListItem>--To--</asp:ListItem>
                                                </asp:DropDownList>

                                            </td>
                                        </tr>--%>

                                        <tr runat="server" id="trindependantvisit" visible="false">
                                            <td></td>
                                            <td style="margin-left: 100px; text-align: left;">
                                                <asp:DropDownList runat="server" ID="ddlFromVisit" Width="200px" CssClass="dropDownList">
                                                    <asp:ListItem>--From--</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:DropDownList runat="server" ID="ddlToVisit" Width="200px" CssClass="dropDownList">
                                                    <asp:ListItem>--To--</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>

                                        </tr>

                                        <tr>
                                            <td colspan="2">
                                                <center>
                                                    <asp:Button runat="server" CssClass="btn btngo" ID="btnGo" OnClientClick="return Validation();" Text="" Title="Go" />
                                                    <asp:Button runat="server" CssClass="btn btnexcel" ID="btnExcel" Title="Export To Excel" />
                                                    <asp:Button runat="server" CssClass="btn btnpdf" ID="btnPDF" Title="Export To PDF" />
                                                    <asp:Button runat="server" CssClass="btn btnexit" ID="btnExitBasedOnParent"
                                                        OnClientClick="return msgconfirmalert('Are You Sure You Want To Exit?',this); " OnClick="btnExitBasedOnParent_Click" Text="Exit" Title="Exit" />
                                                    <asp:HiddenField ID="HFHeaderLabel" runat="server" />
                                                    <asp:HiddenField ID="HFHeaderPDF" runat="server" />
                                                </center>
                                            </td>
                                        </tr>

                                        <tr runat="server" id="TrLegends" visible="false">
                                            <td style="text-align: left;"></td>
                                            <td style="text-align: right; padding-right: 1%;">Legends
                                            <img id="imgShow" runat="server" src="images/question.gif" enableviewstate="false"
                                                onmouseover="$('#ctl00_CPHLAMBDA_canal').toggle('medium');" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td colspan="2">
                                                <fieldset style="display: none; width: 85%; margin-left: 95px; font-size: 7pt; height: auto; text-align: left"
                                                    id="canal" runat="server" class="FieldSetBox">
                                                    <div>
                                                        <asp:Label runat="server" ID="Label1" Width="20" Height="10" BackColor="Red"
                                                            EnableViewState="false">  </asp:Label>
                                                        <b>Out of window Period </b>

                                                        <asp:Label runat="server" ID="Label2" Width="20" Height="10" BackColor="Green"
                                                            EnableViewState="false">  </asp:Label>
                                                        <b>Within window Period</b>

                                                        <asp:Label runat="server" ID="Label3" Width="20" Height="10" BackColor="Gray"
                                                            EnableViewState="false">  </asp:Label>
                                                        <b>Data not entered</b>

                                                    </div>
                                                </fieldset>
                                            </td>
                                        </tr>

                                        <%--</table>
                                    <table style="width: 100% !important; max-width:800px !important;" runat="server">--%>
                                        <tr runat="server" class="dvParentVisit" id="BasedOnParentVisit" visible="true">
                                            <td colspan="2">
                                                <div runat="server" id="dvParentVisit" class="dvParentVisit" align="center">
                                                    <asp:GridView ID="gvBasedOnParentVisit" runat="server" AutoGenerateColumns="true">
                                                        <RowStyle HorizontalAlign="Center" />
                                                        <AlternatingRowStyle HorizontalAlign="Center" />
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="IndependentVisit">
                                            <td colspan="2">
                                                <div runat="server" id="divIndependentVisit" class="divIndependentVisit" align="center">
                                                    <asp:GridView runat="server" ID="gvIndependentVisit" AutoGenerateColumns="true">
                                                        <RowStyle HorizontalAlign="Center" />
                                                        <AlternatingRowStyle HorizontalAlign="Center" />
                                                        <%-- <Columns>
                                                        <asp:BoundField DataField="vProjectNo" HeaderText="Project No"   />
                                                        <asp:BoundField DataField="vSubjectId" HeaderText="Subjects"   />
                                                        <asp:BoundField DataField="vRandomizationNo" HeaderText="Randomization no" />
                                                        <asp:BoundField DataField="vChildActivity" HeaderText="Child Activity" />
                                                        <asp:BoundField DataField="vRefActivity" HeaderText="Ref Activity" />
                                                    </Columns>--%>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
                                    <asp:PostBackTrigger ControlID="btnExcel" />
                                    <asp:PostBackTrigger ControlID="btnPDF" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="divMainContent" runat="server" style="width: 100%">
            <div id="divLeft" runat="server" style="width: 30%; float: left; padding-top: 3%; height: 560px;">
                <div id="divEditAdd" runat="server">
                    <asp:RadioButtonList ID="rbtAddEdit" runat="server" OnSelectedIndexChanged="rbtAddEdit_SelectedIndexChanged" RepeatDirection="Horizontal"
                        Style="margin: auto" AutoPostBack="true">
                        <asp:ListItem Selected="True" Value="0">Add</asp:ListItem>
                        <asp:ListItem Value="1">Edit</asp:ListItem>

                    </asp:RadioButtonList>
                </div>

                <table style="width: 100%;">
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="upPeriod" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table style="width: 100%; border: 1px solid darkgray; border-radius: 5px;" align="left" border="0" cellspacing="10px">
                                        <tr>
                                            <td class="Label" style="white-space: nowrap; text-align: right; width: 10%">Project No /Site *:
                                            
                                            </td>

                                            <td style="text-align: left; width: 90%;">
                                                <span style="font-weight: normal">
                                                    <asp:TextBox ID="txtProject" runat="server" CssClass="textBox" Width="100%" onkeydown="return (event.keyCode!=13)" />
                                                </span>
                                                <asp:Button ID="btnSetProject" runat="server" Style="display: none" Text=" Project" />
                                                <asp:HiddenField ID="HProjectId" runat="server" />
                                                <asp:HiddenField ID="HClientName" runat="server" />
                                                <asp:HiddenField ID="HProjectName" runat="server" />
                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                                    CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                    CompletionListElementID="pnlProjectlist" CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                                    OnClientShowing="ClientPopulated" ServiceMethod="GetMyProjectCompletionListParentOnly"
                                                    ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True">
                                                </cc1:AutoCompleteExtender>
                                                <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" />

                                            </td>

                                        </tr>
                                        <div runat="server" id="divAdd">
                                            <tr>

                                                <td class="Label" style="white-space: nowrap; text-align: right; width: 10%">Parent Activity *: </td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlParentActivity" name="ParentActivity" AutoPostBack="true" CssClass="dropDownList" Width="250px">
                                                        <asp:ListItem Value="0">Select Visit/Parent Activity</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="Label" style="white-space: nowrap; text-align: right; width: 10%">Child Activity *: </td>
                                                <td>
                                                    <asp:DropDownList runat="server" name="ChildActivity" ID="ddlChildActivity" CssClass="dropDownList" Width="250px">
                                                        <asp:ListItem Value="0">Select Child Activity</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="Label" style="white-space: nowrap; text-align: right; width: 10%">Reference Activity *: </td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlReferanceActivity" onchange="return validationForActivity(this)" name="RefActivity" Width="250px" CssClass="dropDownList">
                                                        <asp:ListItem Value="0">Select Reference Activity</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>


                                            <tr>
                                                <td class="Label" style="white-space: nowrap; text-align: right; width: 10%">Actual *: </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtActual" Width="250px" CssClass="textBox"></asp:TextBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="Label" style="white-space: nowrap; text-align: right; width: 10%">Definition Negative(-) *: </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtPositive" Width="250px" CssClass="textBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Label" style="white-space: nowrap; text-align: right; width: 10%">Definition Positive(+) *: </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtNegative" Width="250px" CssClass="textBox"></asp:TextBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="Label" style="white-space: nowrap; text-align: right; width: 10%"></td>
                                                <td>
                                                    <asp:Button runat="server" CssClass="btn btnnew" ID="btnAdd" OnClientClick="return AddValidation();" Text="Add" />
                                                </td>
                                            </tr>
                                        </div>
                                    </table>

                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </div>

            <div id="divRight" style="width: 60%; margin-left: 10%; float: left; padding-top: 3%; height: auto;">
                <asp:UpdatePanel ID="upGvDeviation" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div style="margin: 20px 20px 20px 20px">
                            <asp:Button runat="server" ID="btnSave" Text="Save" Style="display: none" CssClass="btn btnsave" title="Save" />
                            <asp:Button runat="server" ID="btnDelete" OnClick="btnDelete_Click" OnClientClick=" return ValidationForDelete();" Text="Delete"
                                CssClass="btn btncancel" Style="display: none" title="Delete" />
                            <asp:Button runat="server" ID="btnCancel" OnClick="btnCancel_Click" Text="Cancel" CssClass="btn btncancel" Style="display: none" title="Cancel" />
                            <asp:Button runat="server" ID="btnExit" OnClientClick="return msgconfirmalert('Are You Sure You Want To Exit?',this);"
                                OnClick="btnExit_Click" Text="Exit" CssClass="btn btnexit" Style="display: none" title="Exit" />
                        </div>
                        <asp:GridView ID="gvDeviation" runat="server" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField HeaderText="Edit / Delete">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" AutoPostBack="true" ID="chkEdit" ToolTip='<%# Eval("nWorkSpaceDeviationId")%>' OnCheckedChanged="chkEdit_CheckedChanged" onchange="return ValidationCheckBox();" />

                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Project Name" HeaderStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txt" CssClass="textBox" Width="50px" Text='<%# Eval("vProjectName")%>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Parent Activity">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlgvParentActivity" Width="100px" CssClass="dropDownList" runat="server" Orientation="Horizontal">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Child Activity">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlgvChildActivity" Width="100px" runat="server" CssClass="dropDownList" Orientation="Horizontal">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Ref Activity">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlgvRefeActivity" Width="100px" runat="server" CssClass="dropDownList" Orientation="Horizontal">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Deviation Negative" HeaderStyle-Width="30px">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtDeviationNegative" CssClass="textBox" Width="30px" Text='<%# Eval("iDeviationNegative") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Deviation Positive" HeaderStyle-Width="30px">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtDeviationPositive" CssClass="textBox" Width="30px" Text='<%# Eval("iDeviationPositive")%>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Window Period" HeaderStyle-Width="30px">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtWindowPeriod" CssClass="textBox" Width="30px" Text='<%# Eval("iWindowPeriod") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Update Field" HeaderStyle-Width="30px">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnk" OnClientClick="return ShowPopupBtnTest();" CommandName="MyValue" ToolTip="Update" Text="Update"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Audit Trail" HeaderStyle-Width="30px">
                                    <ItemTemplate>
                                        <img src="Images/audit.png" onmouseover="this.style.cursor='pointer';" onclick="AuditTrail(this.id);" id='<%# Eval("nWorkSpaceDeviationId")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="nWorkSpaceDeviationId" HeaderText="WorkSpaceId" />
                                <asp:BoundField DataField="cStatusIndi" HeaderText="StatusIndi" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>

        </div>
    </div>
    <button id="btnRemarls" runat="server" style="display: none;" />

    <cc1:ModalPopupExtender ID="ModalRemarks" runat="server" PopupControlID="divRemarks"
        BackgroundCssClass="modalBackground" TargetControlID="btnRemarls" BehaviorID="ModalRemarks"
        CancelControlID="CancelRemarks">
    </cc1:ModalPopupExtender>


    <div id="ModalBackGround" runat="server" class="divModalBackGround" style="display: none; width: 100% !Important;"></div>

    <div class="modal-content modal-sm" id="divRemarks" style="display: none;" runat="server">
        <div class="modal-header">
            <img id="CancelRemarks" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px; cursor: pointer;" title="Close" />
            <h2>Enter Remarks</h2>
        </div>
        <div class="modal-body">
            <table style="margin: 10px 10px 10px 10px;">
                <tr>
                    <td colspan="2">
                        <center>
                            <asp:Label runat="server" ID="lblChangeStatus"></asp:Label>
                        </center>
                    </td>
                </tr>

                <tr style="margin: 10px 10px 10px 10px;">
                    <td style="white-space: nowrap; text-align: right; width: 120px;" class="Label">Remarks*:
                    </td>
                    <td class="Label" align="right">
                        <asp:TextBox ID="txtRemarks" runat="Server" TextMode="MultiLine" onkeyup="characterlimit(this)" Text="" CssClass="textbox" Style="width: 225px; height: 69px;"> </asp:TextBox>
                        <asp:Label runat="server" Text="*" ForeColor="Red" ID="lblError" Style="display: none;"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="modal-footer">
            <asp:Button ID="btnSaveRemarks" runat="server" CssClass="btn btnsave" OnClientClick="return validation21();" ValidationGroup="validate" Text="Update" title="Update" />
            <asp:Button ID="btnCancel1" OnClick="btnCancel1_Click" runat="server" CssClass="btn btncancel" Text="Cancel" title="Cancel" />
        </div>
    </div>

    <button id="btnAuditTrail" runat="server" style="display: none;" />

    <img alt="" runat="server" id="ImgLogo" style="display: none;" src="images/lambda_logo.jpg" enableviewstate="false" />

    <cc1:ModalPopupExtender ID="modalpopupaudittrail" runat="server" PopupControlID="dvAudiTrail"
        BackgroundCssClass="modalBackground" TargetControlID="btnAuditTrail" BehaviorID="modalpopupaudittrail"
        CancelControlID="imgAuditTrail">
    </cc1:ModalPopupExtender>

    <div id="dvAudiTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
        <table border="0" cellpadding="2" cellspacing="2" width="100%">
            <tr>
                <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 80%;">Audit Trail</td>
                <td style="width: 3%">
                    <img id="imgAuditTrail" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table align="center" border="0" cellpadding="2" cellspacing="2" width="100%">
                        <tr>
                            <td>
                                <table id="tblAudit" class="tblAudit" width="100%" style="background-color: aliceblue;"></table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
        </table>
    </div>
    <div style="text-align: center; overflow: auto; height: 600px; display: none;" id="HeaderLabel"
        runat="server" align="center">
        <table style="width: 100%" align="center" id="MainContentTable" runat="server" class="table1">
            <tbody>
                <tr align="center">
                    <td align="center" width="100%" colspan="3">
                        <asp:UpdatePanel ID="UpPlaceHolder" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                            <ContentTemplate>
                                <asp:Panel ID="PnlPlaceMedex" runat="server" Width="100%" ScrollBars="Auto">
                                    <asp:PlaceHolder ID="PlaceMedEx" runat="server" EnableViewState="true"></asp:PlaceHolder>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>

    <%--<div id="divHeaderPDF" style="display: none;" runat="server" align="left" enableviewstate="false">
        <table runat="server" id="tbldivheader" style="padding: 2px; margin: auto; border: solid 1px black; width: 100%; font-family: 'Times New Roman' !Important; font-size: 12px;"
            align="left">
            <tr>
                <td style="vertical-align: top; font-size: 12px; font-family: 'Times New Roman' !Important;">Lambda Therapeutic Research
                </td>
                <td>
                    <label for="reportname" title="lblreportname" />
                </td>
                <td align="right" style="font-family: 'Times New Roman' !Important; font-size: 12px;">Project No:
                </td>
                <td valign="top">
                    <img alt="" runat="server" id="Img2" src="images/lambda_logo.jpg" enableviewstate="false" />
                </td>
                <table>
                        <tr>
                            <td colspan="4" style="vertical-align: top;">
                                <table>
                                    <tr>
                                        <td style="vertical-align: top; font-size: 12px; font-family: 'Times New Roman' !Important;">Lambda Therapeutic Research
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; font-size: 12px; font-family: 'Times New Roman' !Important;">
                                            <span id="spnBaBe" runat="server"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: bottom; font-size: 12px; font-weight: bold !important; font-family: 'Times New Roman' !Important; text-align: right">Deviation Report :
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="font-family: 'Times New Roman' !Important; font-size: 12px;">Project No:
                            </td>
                        </tr>

                    </table>
                </td>
                <td valign="top">
                    <img alt="" runat="server" id="Img1" src="images/lambda_logo.jpg" enableviewstate="false" />
                </td>
            </tr>
        </table>
    </div>--%>
    <div style="text-align: center; overflow: auto; height: 500px; display: none;" id="Div1"
        runat="server" align="center">
        <table style="width: 100%" align="center" id="Table1" runat="server">
            <tbody>
                <tr align="center">
                    <td align="center" width="100%" colspan="3">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                            <ContentTemplate>
                                <asp:Panel ID="Panel1" runat="server" Width="100%" ScrollBars="Auto">
                                    <asp:PlaceHolder ID="MedExPlace" runat="server" EnableViewState="true"></asp:PlaceHolder>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>




    <script src="Script/jquery-ui-1.10.4.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="Script/General.js"></script>
    <script type="text/javascript" src="Script/Validation.js"></script>
    <script type="text/javascript" src="Script/popcalendar.js"></script>
    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <script src="Script/jquery-ui.js" type="text/javascript"></script>


    <script src="Script/jquery.multiselect.js" type="text/javascript"></script>
    <script src="Script/FixedHeader.min.js" type="text/javascript"></script>

    <link rel="stylesheet" type="text/css" href="App_Themes/jquery.multiselect.css" />
    <link rel="Stylesheet" type="text/css" href="App_Themes/StyleBlue/UI_Theme/jquery-ui.css" />
    <style type="text/css">
        .FieldSetBox {
            border: #aaaaaa 1px solid;
            z-index: 0px;
            border-radius: 4px;
        }

        .textBox {
            color: Black !important;
            font-family: Delicious, sans-serif !important;
            font-size: 12px !important;
            width: 100%;
        }

        .Multiselect {
            color: #555;
            line-height: 165%;
        }

        .ui-multiselect {
            max-height: 35px;
            /*overflow: auto;*/
            overflow-x: hidden;
            white-space: nowrap;
        }

        .ui-multiselect-menu {
            width: 300px !important;
            max-height: 100px!important;
            font-size: 0.8em !important;
            overflow: auto;
        }

            .ui-multiselect-menu span {
                vertical-align: top;
            }

        .ui-menu .ui-menu-item a {
            font-size: 11px !important;
            text-align: left !important;
        }

        .ui-multiselect-checkboxes {
            overflow-y: visible !important;
        }

            .ui-multiselect-checkboxes li ul li {
                list-style: none !important;
                clear: both;
                font-size: 2.0em;
                padding-right: 3px;
            }

            .ui-multiselect-checkboxes ui-helper-reset {
                width: 500px;
            }

        .ui-widget {
            font-size: 1.0em;
        }

        .table1 {
            font-family: 'Times New Roman' !important;
        }

        /*.dataTables_wrapper {
            min-width: 70% !important;
            max-width: 100% !important;
            height: auto !important;
        }*/

        /*.dataTables_scrollHeadInner {
            min-width: 70% !important;
            max-width: 1317px !important;
        }*/

        /*.dataTables_scrollBody {
            height: auto !important;
        }*/

        /*.divIndependentVisit {
            width: 77% !important;
            height: auto !important;
        }*/

        /*.dvParentVisit {
            min-width: 500px;
            max-width: 1266px !important;
            height: auto !important;
        }*/
    </style>
    <script type="text/javascript">

        function pageLoad() {

            if ($("#<%= ddlSUbject.ClientID%>")) {
                fnApplySelectSubject();
            }


            var pnlWidth = screen.width - 100 + 'px';
            $("#divradioreport").width(pnlWidth);
            //alert(pnlWidth)

            if ($get('<%= gvBasedOnParentVisit.ClientID%>') != null && $get('<%= gvBasedOnParentVisit.ClientID %>_wrapper') == null) {

                if ($('#<%= gvBasedOnParentVisit.ClientID%>')) {
                    $('#<%= gvBasedOnParentVisit.ClientID%>').prepend($('<thead>').append($('#<%= gvBasedOnParentVisit.ClientID%> tr:first')))
                 .dataTable({
                     "bJQueryUI": true,
                     "sScrollY": 'auto',
                     "sScrollX": '100%',
                     "scrollX": true,
                     "bPaginate": false,
                     "bFooter": false,
                     "bHeader": false,
                     "AutoWidth": true,
                     "bSort": false,
                     "dom": 'T<"clear">lfrtip',
                     //"sDom": '<"H"frT>t<"F"i>',
                     "oLanguage": { "sSearch": "Search" },
                 });
                }
                $(".dataTables_wrapper").css("width", ($(window).width() * 0.85 | 0) + "px");
            }


            if ($get('<%= gvDeviation.ClientID%>') != null && $get('<%= gvDeviation.ClientID %>_wrapper') == null) {

                if ($('#<%= gvDeviation.ClientID%>')) {
                    $('#<%= gvDeviation.ClientID%>').prepend($('<thead>').append($('#<%= gvDeviation.ClientID%> tr:first')))
                            .dataTable({
                                "sScrollY": '250px',
                                "sScrollX": '70%',
                                "bJQueryUI": true,
                                "bPaginate": false,
                                "bFooter": false,
                                "bHeader": false,
                                "bAutoWidth": false,
                                "Width": '65%',
                                "bSort": false,
                                // "dom": 'T<"clear">lfrtip',
                                //"oLanguage": { "sSearch": "Search" },
                            });
                }

                $(".dataTables_scrollBody").css("height", "80%");
                //$(".dataTables_wrapper").css("width", ($(window).width() * 0.95 | 0) + "px");

            }

            //if ($('#<%= gvDeviation.ClientID%>')) {
            //    $('#<%= gvDeviation.ClientID%>').prepend($('<thead>').append($('#<%= gvDeviation.ClientID%> tr:first')))
            //   .dataTable({
            //       "sScrollY": 'auto',
            //       "sScrollX": '50%',
            //       "bJQueryUI": true,
            //       "bPaginate": false,
            //       "bFooter": false,
            //       "bHeader": false,
            //       "bAutoWidth": false,
            //       "bSort": false,
            //       "sDom": '<"H"frT>t<"F"i>',
            //       "oLanguage": { "sSearch": "Search" },
            //       "oTableTools": {
            //           "aButtons": [
            //              "copy",
            //                     "csv",
            //                      "xls",
            //                      {
            //                          "sExtends": "pdf",
            //                          "sPdfOrientation": 'portrait'
            //                      },
            //                     "print"],
            //           "sSwfPath": "Script/swf/copy_csv_xls_pdf.swf"
            //       }
            //   });
            //
            //}

            // $(window).width() > 1180 ? wid = ($(window).width() - 94) + 'px' : wid = $(window).width() - 100 + 'px';
            //  $('#<%= gvBasedOnParentVisit.ClientID%>').attr("style", "width:" + wid + ";")
            // $("#gvBasedOnParentVisit").css({ "max-width": $(window).width() * 0.75 });

            //if ($("#gvIndependentVisit").children().length > 0) {
            //    $("#gvIndependentVisit").dataTable().fnDestroy();
            //}

            //if ($("#gvIndependentVisit").html() != "") {
            //    //  $("#gvIndependentVisit").dataTable().fnDestroy();
            //    $("#gvIndependentVisit").empty();
            //}

            //$('#gvIndependentVisit').DataTable({
            //    destroy: true,
            //    searching: false
            //});

            if ($get('<%= gvIndependentVisit.ClientID%>') != null && $get('<%= gvIndependentVisit.ClientID %>_wrapper') == null) {

                if ($('#<%= gvIndependentVisit.ClientID%>')) {
                    $('#<%= gvIndependentVisit.ClientID%>').prepend($('<thead>').append($('#<%= gvIndependentVisit.ClientID%> tr:first')))
                          .dataTable({
                              "bJQueryUI": true,
                              "scrollY": true,
                              "sScrollX": '100%',
                              "bPaginate": false,
                              "bFooter": false,
                              "bHeader": false,
                              "bAutoWidth": false,
                              //"Width": '65%',
                              "bSort": false,
                              "dom": 'T<"clear">lfrtip',
                              //"oLanguage": { "sSearch": "Search" },
                          });
                }
                //$(".dataTables_wrapper").css("max-width", ($(window).width() * 0.6 | 0) + "px");
                $(".dataTables_wrapper").css("width", ($(window).width() * 0.58 | 0) + "px");


                //$(".dataTables_scroll").css("Weight", "auto");
                //$(".dataTables_wrapper").css("height", "auto");
            }
        }

        function ClientPopulated(sender, e) {
            //ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientID%>'));
            ProjectClientShowingSchema('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));

        }

        function OnSelected(sender, e) {

            ProjectOnItemSelectedForMsrLog(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'), $get('<%=HClientName.clientid %>'), $get('<%=HProjectName.clientid %>'));


            //ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            // $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }
        function ClientPopulatedReport(sender, e) {
            ProjectClientShowing('AutoCompleteExtender2', $get('<%= txtProject.ClientID%>'));
        }

        function OnSelectedReport(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProjectReport.ClientID%>'),
             $get('<%= HReportProjectId.clientid %>'), document.getElementById('<%= btnSetProjectReport.ClientID%>'));
            document.getElementById('<%= hdnSubSelection.ClientID%>').value = "";
            //Subject = [];
            //for (var i = 0; i < event.target.options.length; i++) {
            //    Subject.push("'" + $(event.target.options[i]).val() + "'")
            //}
        }

        var Subject = [];
        function fnApplySelectSubject() {
            fnDeletePreviousMultiselect();

            $("#<%= ddlSUbject.ClientID%>").multiselect({
                noneSelectedText: "--Select Subject--",
                click: function (event, ui) {
                    if (ui.checked == true)
                        Subject.push("'" + ui.value + "'");
                    else if (ui.checked == false) {
                        if ($.inArray("'" + ui.value + "'", Subject) >= 0)
                            Subject.splice(Subject.indexOf("'" + ui.value + "'"), 1)
                    }
                    if ($("input[name$='ddlSUbject']").length > 0) {
                        //clearControls();
                    }
                },
                checkAll: function (event, ui) {
                    Subject = [];
                    for (var i = 0; i < event.target.options.length; i++) {
                        Subject.push("'" + $(event.target.options[i]).val() + "'")
                    }
                    //if ($("input[name$='ddlSUbject']").length > 0) {
                    //    //clearControls();
                    //}
                    //$("#<%= ddlSUbject.ClientID%>").multiselect("refresh");
                    //$("#<%= ddlSUbject.ClientID%>").multiselect("widget").find(':checkbox').click();
                    //document.getElementById('<%= hdnSubSelection.clientid %>').valueOf = Subject;

                },
                uncheckAll: function (event, ui) {
                    Subject = [];
                    //$("#<%= ddlSUbject.ClientID%>").multiselect("refresh");
                    //if ($("input[name$='ddlSUbject']:checked").length > 0) {
                    //    //clearControls();
                    //}
                }
            });

            $("#<%= ddlSUbject.ClientID%>").multiselect("refresh");
            var CheckedCheckBox = document.getElementById('<%= hdnSubSelection.ClientID %>').value
            if (CheckedCheckBox != "") {

                CheckedCheckBox = CheckedCheckBox.split(',');
                for (i = 0; i <= CheckedCheckBox.length - 1; i++) {
                    $("#<%= ddlSUbject.ClientID%>").multiselect("widget").find(".ui-multiselect-checkboxes.ui-helper-reset").find("input[value=" + CheckedCheckBox[i] + "]").attr("checked", "checked");

                }
                $('#<%= ddlSUbject.ClientID%>').multiselect("update");
            }
        }

        function fnSubjectSelection() {
            var subtypes = [];
            //Subject = [];

            document.getElementById('<%= hdnSubSelection.ClientID%>').value = "";
            for (i = 0; i < $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlSUbject']:checked").length ; i++) {
                subtypes.push("'" + $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlSUbject']:checked").eq(i).attr("value") + "'");
            }
            document.getElementById('<%= hdnSubSelection.clientid %>').value = subtypes;
            return true;
        }

        function fnDeletePreviousMultiselect() {
            // if ($("ul[class*='ui-multiselect']").length > 1) {
            $('#aspnetForm').nextUntil('ul[id*="ui-id"]').not('#shadow').remove();
            $('#aspnetForm').next().not('#shadow').remove();
            // } 
        }

        function Validation() {
            if ((document.getElementById('<%= txtProjectReport .ClientID%>').value == '')) {
                msgalert('Please Enter Project No !')
                return false;
            }

            if ($("input[name$='ddlSUbject']:checked").length <= 0) {
                msgalert('Please Select Aleast One Subject !')
                return false
            }

            if (document.getElementById('ctl00_CPHLAMBDA_ddlFromVisit').selectedIndex == 0) {
                msgalert('Please Select From Activity First !')
                return false
            }
            if (document.getElementById('ctl00_CPHLAMBDA_ddlToVisit').selectedIndex == 0) {
                msgalert('Please Select To Activity First !')
                return false
            }

            if (document.getElementById('ctl00_CPHLAMBDA_ddlToVisit').selectedIndex == document.getElementById('ctl00_CPHLAMBDA_ddlFromVisit').selectedIndex) {
                msgalert('Both Activity Are Same Please Change IT !')
                return false
            }

        }

        function validation21() {
            if (document.getElementById('<%= txtRemarks.ClientID%>').value == '') {
                msgalert("Please Enter Remarks !");
                return false
            }
            return true
        }
        function validateForm() {

            if ($("input[name$='ddlSUbject']:checked").length <= 0) {
                msgalert('Please Select Atleast One Subject !')
                return false;
            }
            else {
                document.getElementById('<%= HFHeaderLabel.ClientID%>').value = document.getElementById('<%=HeaderLabel.ClientId %>').innerHTML;
                return true;
            }
        }

        function ShowPopupBtnTest(id) {
            $("#<%= btnSaveRemarks.ClientID%>").val("Update");
            return true;
        }

        function EditRow(row) {
            var rowid = row.id
            msgalert(rowid)
        }


        function ValidationCheckBox() {
            var count = 0;
            for (i = 0; i < document.forms[0].elements.length; i++) {
                if ((document.forms[0].elements[i].type == 'checkbox') &&
                    (document.forms[0].elements[i].name.indexOf('chkEdit') > -1)) {
                    if (document.forms[0].elements[i].checked == true) {
                        count++;
                        if (count > 1) {
                            document.forms[0].elements[i].checked = false;
                            break;
                        }
                    }
                }
            }
            if (count > 1) {
                msgalert('Please select only one checkbox !');
                return false;
            }
            else { return true; }
        }



        function ValidationForDelete() {
            $("#<%= btnSaveRemarks.ClientID%>").val('Delete');
            var count = 0;
            for (i = 0; i < document.forms[0].elements.length; i++) {
                if ((document.forms[0].elements[i].type == 'checkbox') &&
                    (document.forms[0].elements[i].name.indexOf('chkEdit') > -1)) {
                    if (document.forms[0].elements[i].checked == true) {
                        count++;
                        if (count > 1) {
                            document.forms[0].elements[i].checked = false;
                            break;
                        }
                    }
                }
            }
            if (count == 0) {
                msgalert('Please select atleast one Deviation Activity !');
                return false;
            }
            else { return true; }
        }





        function AuditTrail(id) {
            var WorkSpaceDeviationId = id
            var WorkSpaceId
            WorkSpaceId = document.getElementById('<%= HProjectId.ClientID%>').value;

            $.ajax({
                type: "post",
                url: "frmCTDeviationReport.aspx/AuditTrailForActiveInActiveUser",
                data: '{"WorkSpaceDeviationId":"' + WorkSpaceDeviationId + '","WorkSpaceId":"' + WorkSpaceId + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                success: function (data) {
                    $('#tblAudit').attr("IsTable", "has");
                    var aaDataSet = [];
                    var RowId
                    if (data.d != "" && data.d != null) {
                        data = JSON.parse(data.d);
                        for (var Row = 0; Row < data.length; Row++) {
                            var InDataSet = [];
                            RowId = Row + 1
                            InDataSet.push(RowId, data[Row].ProjectNo, data[Row].ParentActivity, data[Row].ChildActivity, data[Row].RefActivity, data[Row].WindowPeriod, data[Row].DeviationNegative, data[Row].DeviationPositive, data[Row].Remarks, data[Row].ModifyBy, data[Row].Modifyon);
                            aaDataSet.push(InDataSet);
                        }

                        if ($("#tblAudit").children().length > 0) {
                            $("#tblAudit").dataTable().fnDestroy();
                        }
                        $('#tblAudit').prepend($('<thead>').append($('#tblAudit tr:first'))).dataTable({
                            "bStateSave": false,
                            "bPaginate": false,
                            "sPaginationType": "full_numbers",
                            "sDom": '<fr>t<p>',
                            "iDisplayLength": 10,
                            "bSort": false,
                            "bFilter": false,
                            "bDestory": true,
                            "bRetrieve": true,
                            "aaData": aaDataSet,
                            "aoColumns": [
                                { "sTitle": "Sr.No." },
                                { "sTitle": "ProjectNo" },
                                { "sTitle": "ParentActivity" },
                                { "sTitle": "ChildActivity" },
                                { "sTitle": "RefActivity" },
                                { "sTitle": "Window Period" },
                                { "sTitle": "Deviation Negative" },
                                { "sTitle": "Deviation Positive" },
                                 { "sTitle": "Remarks" },
                                { "sTitle": "ModifyBy" },
                                { "sTitle": "ModifyOn" },
                            ],
                            "oLanguage": {
                                "sEmptyTable": "No Record Found",
                            }
                        });
                        $find('modalpopupaudittrail').show();
                    }
                },
                failure: function (response) {
                    //alert(response.d);
                },
                error: function (response) {
                    //alert(response.d);
                }
            });

        }

        function destroyGrid() {


            if ($("#gvIndependentVisit").html() != "") {
                // $("#gvIndependentVisit").dataTable().fnDestroy();
                $("#gvIndependentVisit").empty();
            }

            if ($("#gvBasedOnParentVisit").html() != "") {
                // $("#gvBasedOnParentVisit").dataTable().fnDestroy();
                $("#gvBasedOnParentVisit").empty();
            }

            if ($("#gvDeviation").html() != "") {
                // $("#gvDeviation").dataTable().fnDestroy();
                $("#gvDeviation").empty();
            }



            if ($("#gvIndependentVisit").children().length > 0) {
                $("#gvIndependentVisit").dataTable().fnDestroy();
            }

            if ($("#gvBasedOnParentVisit").children().length > 0) {
                $("#gvBasedOnParentVisit").dataTable().fnDestroy();
            }
            if ($("#gvDeviation").children().length > 0) {
                $("#gvDeviation").dataTable().fnDestroy();
            }
        }

        function AddValidation() {
            //var e = document.getElementById("ddlParentActivity");
            //var strUser = e.options[e.selectedIndex].value;
            if (document.getElementById('<%= txtProject.ClientID%>').value == '' || document.getElementById('<%= txtProject.ClientID%>').value == 'undefine') {
                msgalert('Please Select Project First !');
                return false
            }
            if (document.getElementById('ctl00_CPHLAMBDA_ddlParentActivity').selectedIndex == 0) {
                msgalert('Please Select Parent Activity First !');
                return false
            }
            if (document.getElementById('ctl00_CPHLAMBDA_ddlChildActivity').selectedIndex == 0) {
                msgalert('Please Select Child Activity First !');
                return false
            }
            if (document.getElementById('ctl00_CPHLAMBDA_ddlReferanceActivity').selectedIndex == 0) {
                msgalert('Please Select Reference Activity First !');
                return false
            }
            if (document.getElementById('<%= txtActual.ClientID%>').value == '' || document.getElementById('<%= txtActual.ClientID%>').value == 'undefine') {
                msgalert('Please Enter Actual/Window Time !');
                return false
            }
            if (document.getElementById('<%= txtPositive.ClientID%>').value == '' || document.getElementById('<%= txtPositive.ClientID%>').value == 'undefine') {
                msgalert('Please Enter Positive Deviation !');
                return false
            }
            if (document.getElementById('<%= txtNegative.ClientID%>').value == '' || document.getElementById('<%= txtNegative.ClientID%>').value == 'undefine') {
                msgalert('Please Enter Negative Deviation !');
                return false
            }

            var selectedText = jQuery('#ctl00_CPHLAMBDA_ddlChildActivity').find("option:selected").text();
            var selectedValue = jQuery('#ctl00_CPHLAMBDA_ddlChildActivity').val();

            var selectedText1 = jQuery('#ctl00_CPHLAMBDA_ddlReferanceActivity').find("option:selected").text();
            var selectedValue1 = jQuery('#ctl00_CPHLAMBDA_ddlReferanceActivity').val();

            if (selectedValue == selectedValue1) {
                ddl.selectedIndex = 0
                msgalert('Child and Reference Activity Should Not Be Same !');
                return false;
            }

        }

        function PdfExcelValidation() {
            var table = $('#example').DataTable();
            msgalert(table);

            var ButtonList = document.getElementById('<%= rbtreporttype.ClientID%>')

            if (ButtonList != null) {
                // get the buttons (inputs) in the table
                var Buttons = ButtonList.getElementsByTagName("input");
                for (var x = 0; x < Buttons.length; x++) {
                    if (Buttons[x].checked) {
                        if (Buttons[x].value == 0) {
                            var rowscount = document.getElementByID('<%=gvBasedOnParentVisit.ClientID%>').rows.length;
                            if (rowscount == 0) {
                                msgalert('Please Click on Go Button First or There is No Data !');
                                return false
                            }
                        }
                        if (Buttons[x].value == 1) {
                            var rowscount = document.getElementByID('<%=gvIndependentVisit.ClientID()%>').rows.length;
                            if (rowscount == 0) {
                                msgalert('Please Click on Go button First or There is no Data !');
                                return false
                            }
                        }
                        //alert("You checked " + Buttons[x].id + " which has the value " + Buttons[x].value);
                    }
                }
            }
        }


        function validationForActivity(ddl) {

            var selectedText = jQuery('#ctl00_CPHLAMBDA_ddlChildActivity').find("option:selected").text();
            var selectedValue = jQuery('#ctl00_CPHLAMBDA_ddlChildActivity').val();

            var selectedText1 = jQuery(ddl).find("option:selected").text();
            var selectedValue1 = jQuery(ddl).val();

            if (selectedValue == selectedValue1) {
                ddl.selectedIndex = 0
                msgalert('Child and Reference Activity Should Not Be Same !');
                return false;
            }
            return true;


        }

        function ShowAlert(msg) {
            debugger;
            alertdooperation(msg, 4, "");
        }

        //$("#ddlFruits").change(function () {
        //    var selectedText = $(this).find("option:selected").text();
        //    var selectedValue = $(this).val();
        //    alert("Selected Text: " + selectedText + " Value: " + selectedValue);
        //});


    </script>

    <link href="App_Themes/StyleBlue/StyleBlue.css" rel="stylesheet" type="text/css" />
    <%--<script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>--%>
    <script src="Script/jquery.dataTables.min.js" type="text/javascript"></script>
    <script src="Script/jquery.dataTables.plugins.js" type="text/javascript"></script>
    <script src="Script/TableTools.min.js" type="text/javascript"></script>


</asp:Content>


