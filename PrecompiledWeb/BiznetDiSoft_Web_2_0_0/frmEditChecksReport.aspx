<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmEditChecksReport, App_Web_22suyskz" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/DD_roundies_0.0.2a.js"></script>

    <asp:UpdatePanel runat="server" ID="PnlControlls" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <table>
                <tr>
                    <td align="left">
                        <asp:Label CssClass="Label" ID="LblProject" runat="server" Text="Project No/Request Id : "
                            AssociatedControlID="txtProject"></asp:Label>
                    </td>
                    <td align="left">
                        &nbsp;<asp:TextBox runat="server" CssClass="textBox" Width="622px" ID="txtProject"></asp:TextBox>
                        <asp:Button runat="server" ID="BtnSetProject" Style="display: none" CssClass="button " />
                        <asp:HiddenField runat="server" ID="HProjectId" />
                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="true"
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
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnSetProject" EventName="click" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UPnl_Data" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="HFPanelValues" />
            <asp:Panel ID="fsSearch" runat="server" Style="width: 840px; padding: 15px; display: none">
                <div class="roundedHeader" style="background-color: #1560a1; height: 13px; padding-right: 3px;
                    padding-left: 3px; padding-bottom: 1px; vertical-align: middle; width: 840px;
                    cursor: pointer; padding-top: 1px; background-color: #1560a1; min-width: 100%"
                    id="div12" onclick="ShowDetail();">
                    <div style="font-weight: bold; font-size: 13px; float: none; vertical-align: middle;
                        color: white;">
                        &nbsp;Search
                    </div>
                </div>
                <asp:Panel ID="pnlSearch" CssClass="pnlSearch" runat="server" Style="display: none"
                    Width="940px">
                    <table style="width: 932px" cellpadding="3" cellspacing="3">
                        <tr>
                            <td align="left" style="width: 68px">
                                <asp:Label ID="lblPeriod" runat="server" Text="Periods : " AssociatedControlID="DdlPeriod"
                                    CssClass="Label"></asp:Label>
                            </td>
                            <td align="left" style="width: 147px">
                                &nbsp;<asp:DropDownList runat="server" ID="DdlPeriod" AutoPostBack="true" CssClass="dropDownList"
                                    Width="120px">
                                </asp:DropDownList>
                            </td>
                            <td align="left" style="width: 129px">
                                <asp:Label ID="lblSubSpecific" runat="server" Text="Activity Type : " AssociatedControlID="RblSubSpecific"
                                    CssClass="Label"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:RadioButtonList ID="RblSubSpecific" runat="server" RepeatDirection="Horizontal"
                                    CssClass="radiobutton " AutoPostBack="true">
                                    <asp:ListItem Text="All" Value="A" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Subject Specific" Value="Y"></asp:ListItem>
                                    <asp:ListItem Text="General Activities" Value="N"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 68px">
                                <asp:Label ID="LblEnteredBy" runat="server" Text="Entered By : " AssociatedControlID="DdlEnteredBy"
                                    CssClass="Label"></asp:Label>
                            </td>
                            <td align="left" style="width: 147px">
                                &nbsp;<asp:DropDownList runat="server" ID="DdlEnteredBy" AutoPostBack="true" CssClass="dropDownList"
                                    Width="120px">
                                </asp:DropDownList>
                            </td>
                            <td align="left" style="width: 129px">
                                <asp:Label ID="LblParentActivity" runat="server" Text="Parent Activity/Visit : "
                                    AssociatedControlID="DdlParentActivity" CssClass="Label"></asp:Label>
                            </td>
                            <td align="left">
                                &nbsp;<asp:DropDownList runat="server" ID="DdlParentActivity" AutoPostBack="true"
                                    CssClass="dropDownList" Width="480px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 68px">
                                <asp:Label ID="LblQryStatus" runat="server" Text="Query Status : " AssociatedControlID="DdlQryStatus"
                                    CssClass="Label"></asp:Label>
                            </td>
                            <td align="left" style="width: 147px">
                                &nbsp;<asp:DropDownList runat="server" ID="DdlQryStatus" AutoPostBack="true" CssClass="dropDownList"
                                    Width="120px">
                                </asp:DropDownList>
                            </td>
                            <td align="left" style="width: 129px">
                                <asp:Label ID="LblEnteredDtRange" runat="server" Text="EditChecks Date : " AssociatedControlID="RblSubSpecific"
                                    CssClass="Label"></asp:Label>
                            </td>
                            <td align="left">
                                From :
                                <asp:TextBox ID="txtStartDt" runat="server" CssClass="textBox" TabIndex="11" Width="80px"
                                    onblur="return DateValidation(this.value,this);"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="txtStartDt">
                                </cc1:CalendarExtender>
                                To :
                                <asp:TextBox ID="txtEndDt" runat="server" CssClass="textBox" TabIndex="11" Width="80px"
                                    onblur="return DateValidation(this.value,this);"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="txtEndDt">
                                </cc1:CalendarExtender>
                                <asp:Button runat="server" ID="BtnSearch" Text="Search" CssClass="btn btnnew" />
                                <asp:Button runat="server" ID="BtnRefresh" Text="Reset" CssClass="btn btnnew" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
            <br />
            <asp:Panel runat="server" ID="pnlEditChecksGrid" Width="980px" Height="400px" ScrollBars="Auto">
                <div style="height: 350px; width: 850px; overflow: auto;">
                    <asp:GridView runat="server" ID="GvEditChecksReport" SkinID="grdViewSmlAutoSize"
                        AutoGenerateColumns="false" EmptyDataText="No Edit Checks Entered">
                        <Columns>
                            <asp:BoundField HeaderText="#" />
                            <asp:BoundField HeaderText="Remark" DataField="vRemarks" />
                            <asp:BoundField HeaderText="Query" DataField="vCondition" />
                            <asp:BoundField HeaderText="Parent Activity" DataField="ParentActivity" />
                            <asp:BoundField HeaderText="Activity" DataField="Source_If_Activity" />
                            <asp:BoundField HeaderText="Entered By" DataField="vUserName" />
                            <asp:BoundField HeaderText="Entered On" DataField="dModifyOn" />
                            <asp:BoundField HeaderText="Query Status" DataField="QueryStatus" />
                            <asp:TemplateField HeaderText="Query Status">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lBnQueryStatus" runat="server"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="nMedExEditCheckNo" DataField="nMedExEditCheckNo" />
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>
            <br />
            <asp:Button ID="btnExportEditChecks" runat="server" CssClass="btn btnexcel"  Style="display: none" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnSetProject" EventName="click" />
            <asp:AsyncPostBackTrigger ControlID="RblSubSpecific" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="DdlEnteredBy" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="DdlParentActivity" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="DdlPeriod" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="BtnSearch" EventName="click" />
            <asp:AsyncPostBackTrigger ControlID="BtnRefresh" EventName="click" />
            <asp:AsyncPostBackTrigger ControlID="DdlQryStatus" EventName="SelectedIndexChanged" />
            <asp:PostBackTrigger ControlID="btnExportEditChecks" />
        </Triggers>
    </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="UPnlExecutedEditchecks" runat="server" ChildrenAsTriggers="false"
        UpdateMode="Conditional">
        <ContentTemplate>
            <button id="ExecutedEditchecks" runat="server" style="display: none;" />
            <cc1:ModalPopupExtender ID="MPExecutedEditChecks" runat="server" PopupControlID="DivExecutedEditChecks"
                BackgroundCssClass="modalBackground" TargetControlID="ExecutedEditchecks" CancelControlID="ImgPopUp">
            </cc1:ModalPopupExtender>
            <div style="left: 438px; width: 900px; position: absolute; top: 888px; height: 520px;
                display: none;" id="DivExecutedEditChecks" class="popUpDivNoTop" runat="server">
                <div style="border-bottom-width: 3">
                    <img id="ImgPopUp" alt="Close" src="images/Sqclose.gif" style="position: relative;
                        float: right; right: 5px;" />
                </div>
                <asp:Panel ID="PnlExecutedEditChecks" runat="server" Width="885px">
                    <br />
                    <asp:Label runat="server" ID="lblRemark" SkinID="lblHeading" Text="Rule : " CssClass="Label"></asp:Label>
                    <asp:Label runat="server" ID="LblQuery" SkinID="lblHeading" BorderStyle="Outset"
                        BorderWidth="1" CssClass="Label"></asp:Label>
                    <br />
                    <br />
                    <asp:Panel runat="server" ID="SearchSubjects">
                        <table cellpadding="2" cellspacing="3" width="678px">
                            <tr>
                                <td align="left">
                                    <asp:Label runat="server" ID="LblFiredBy" AssociatedControlID="DdlFiredBy" Text="EditChecks Executed By :"
                                        CssClass="Label"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:DropDownList runat="server" ID="DdlFiredBy" AutoPostBack="true" CssClass="dropDownList"
                                        Width="90px">
                                    </asp:DropDownList>
                                </td>
                                <td align="left">
                                    <asp:Label ID="LblSubjects" runat="server" Text="Subjects : " AssociatedControlID="DdlSubjects"
                                        CssClass="Label"></asp:Label>
                                </td>
                                <td align="left">
                                    &nbsp;<asp:DropDownList runat="server" ID="DdlSubjects" AutoPostBack="true" CssClass="dropDownList"
                                        Width="150px">
                                        <asp:ListItem Text="All subjects" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="1" align="left">
                                    <asp:Label ID="Label1" runat="server" Text="EditChecks Executed On :" AssociatedControlID="RblSubSpecific"
                                        CssClass="Label"></asp:Label>
                                </td>
                                <td colspan="3" align="left">
                                    From :
                                    <asp:TextBox ID="txtFrmDate" runat="server" CssClass="textBox" TabIndex="11" Width="80px"
                                        onblur="return DateValidation(this.value,this);"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CalExtICFDate" runat="server" Format="dd-MMM-yyyy" TargetControlID="txtFrmDate">
                                    </cc1:CalendarExtender>
                                    To :
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="textBox" TabIndex="11" Width="80px"
                                        onblur="return DateValidation(this.value,this);"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                        TargetControlID="txtToDate">
                                    </cc1:CalendarExtender>
                                    <asp:Button runat="server" ID="BtnSearchSubjects" Text="Search" CssClass="btn btnnew" />
                                    <asp:Button runat="server" ID="BtnReset" Text="Reset" CssClass="btn btnnew" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <br />
                    <span style="display: inline; width: 20px; height: 10px" backcolor="Red"></span>
                    <asp:Label runat="server" ID="LblLegends" Height="20"></asp:Label>
                    <br />
                    <asp:Panel runat="server" ID="PnlEditChecksReport" Width="880px" Height="300px">
                        <div style="height: 280px; width: 850px; overflow: auto;">
                            <asp:GridView runat="server" ID="GvExecutedReport" SkinID="grdViewSmlAutoSize" AutoGenerateColumns="false"
                                EmptyDataText="No subjects">
                                <Columns>
                                    <asp:BoundField HeaderText="Sr.No" />
                                    <asp:BoundField DataField="iSourceNodeId_If" HeaderText="iSourceNodeId_If" />
                                    <asp:BoundField DataField="bIsQuery" HeaderText="bIsQuery" />
                                    <asp:BoundField DataField="Source_If_Period" HeaderText="Period" />
                                    <asp:BoundField DataField="CrossActivityPeriod" HeaderText="CrossActivityPeriod" />
                                    <asp:BoundField DataField="CrossActivityNode" HeaderText="CrossActivityNode" />
                                    <asp:BoundField DataField="vSubjectId" HeaderText="Subject" />
                                    <asp:BoundField DataField="vMySubjectNo" HeaderText="Screen No" />
                                    <asp:BoundField DataField="vInitials" HeaderText="Initials" />
                                    <asp:BoundField DataField="dFiredDate" HeaderText="Executed Date" DataFormatString="{0:dd-MMM-yyyy HH:mm tt}" />
                                    <asp:BoundField DataField="vUserName" HeaderText="Executed By" />
                                    <asp:TemplateField HeaderText="Activity">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lBtn_Activity"><%#Eval("Source_If_Activity")%></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cross Activity">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lBtn_CrossActivity"><%#Eval("CrossActivity")%></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="QueryStatus" HeaderText="Query Status" />
                                    <asp:BoundField DataField="CrossActivityId" HeaderText="CrossActivityId" />
                                    <asp:BoundField DataField="Source_If_ActivityId" HeaderText="Source_If_ActivityId" />
                                    <asp:BoundField DataField="iMySubjectNo" HeaderText="SubjectNo" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                    <br />
                    <asp:Button runat="server" CssClass="button " Text="Export To Excel" ID="btnExportSubjects"
                        Width="120px" />
                </asp:Panel>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="GvExecutedReport" EventName="RowDataBound" />
            <asp:AsyncPostBackTrigger ControlID="BtnSearchSubjects" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="BtnReset" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="DdlFiredBy" EventName="SelectedIndexChanged" />
            <asp:PostBackTrigger ControlID="btnExportSubjects" />
        </Triggers>
    </asp:UpdatePanel>

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" src="Script/popcalendar.js"></script>

    <script type="text/javascript" src="Script/General.js"></script>

    <script type="text/javascript" src="Script/jquery-1.4.3.min.js"></script>

    <script type="text/javascript" src="Script/jquery.min.js"></script>

    <script type="text/javascript" src="Script/scrollablegrid.js"></script>

    <script type="text/javascript">

    DD_roundies.addRule('.roundedHeader', '5px');

    function ClientPopulated(sender, e)
    {
        ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
    }

    function OnSelected(sender, e)
    {
        ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
        $get('<%= HProjectId.clientid %>'), document.getElementById('<%= BtnSetProject.ClientId %>'));
    } 

    function ShowHideSubject()
    {
        msgalert("Selected !");
    }
    //        function ViewDetail(cmd)
    //        {
    //            alert(cmd);
    //            return false;
    //        }
    function OpenWindow(Path)
    {
        window.open(Path);
        return false;
    }
    function DateValidation(ParamDate, txtdate)
    {
        txtdate.style.borderColor = "";
        if (ParamDate.trim() != '')
        {
            var flg = false;
            flg = DateConvert(ParamDate, txtdate);
            if (flg == true && !CheckDateLessThenToday(txtdate.value))
            {
                txtdate.value = "";
                txtdate.focus();
                msgalert('Date should be less than Current Date !');
                return false;
            }
            else if (flg == false)
            {
                txtdate.value = "";
                txtdate.focus();
                msgalert('Date should be proper format!');
                return false;
            }
        }

    }
    function ShowDetail(ele)
    {
        $(".pnlSearch").slideToggle(500);
        if (document.getElementById('<%= pnlSearch.clientid %>').style.display == "none")
        {
            document.getElementById('<%= HFPanelValues.clientid %>').value = "";
        }
        else
        {
            document.getElementById('<%= HFPanelValues.clientid %>').value = "YES";
        }


    }

    // for fix gridview header aded on 22-nov-2011
    function FixHeader(id)
    {
        FreezeTableHeader($('#'+id.id), { height: 290, width: 980 });
    }
    </script>

</asp:Content>
