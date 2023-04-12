<%@ page CodeFile="~/frmOldNewProjectSearch.aspx.vb" language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmOldNewProjectSearch" enableEventValidation="false" theme="StyleBlue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/DD_roundies_0.0.2a.js"></script>

    <script type="text/javascript" language="javascript" src="Script/popcalendar.js"></script>

    <script type="text/javascript" src="Script/General.js"></script>

    <script type="text/javascript" src="Script/jquery.min.js"></script>

    <script type="text/javascript">

        DD_roundies.addRule('.roundedHeader', '5px');

        function $(id) {
            return document.getElementById(id);
        }

        function ShowHideDiv(Divid, ImageId, btn) {
            var dv = $(Divid);
            var img = $(ImageId);
            var btn = $(btn);
            if (dv.style.display == 'none') {
                dv.style.display = 'block';
                img.src = "images/collapse.jpg";
                if(document.getElementById("<%=DDlSponser.ClientID %>").length <= 0 && document.getElementById("<%=DDlDrugs.ClientID %>").length <= 0 && document.getElementById("<%=DdlLocation.ClientID %>").length <= 0 && document.getElementById("<%=DdlStudyType.ClientID %>").length <= 0 && document.getElementById("<%=DdlSubmission.ClientID %>").length <= 0)
                {
                    btn.click();
                }
            }
            else {
                dv.style.display = 'none';
                img.src = "images/expand.jpg";
            }
        }

        function ShowDiv(Divid, Imgid) {
            var dv = $(Divid);
            var img = $(Imgid);
            if (dv.style.display == 'none') {
                dv.style.display = 'block';
                img.src = "images/expand.jpg";
            }
        }

        function popcalender(obj, txtBox) {
            var txt = $(txtBox);
            popUpCalendar(obj, txt, 'dd-mmm-yy');
            document.getElementById('<%=ChkAllDate.clientid %>').checked = false;
        }

        function CheckCheckedFields() {
            var ChkList = document.getElementById('<%=ChkListFields.ClientID %>');
            var ischked = 0
            if (ChkList != null && typeof (ChkList) != 'undefined') {
                var chkbx = ChkList.getElementsByTagName('input')
                var chkbxCount = ChkList.getElementsByTagName('input').length
                for (var count = 0; count <= chkbxCount - 1; count++) {
                    if (chkbx(count).checked == true) {
                        ischked = ischked + 1;
                        return true;
                    }
                }
                if (ischked <= 0) {
                    msgalert("Select atleast one field to display !");
                    return false;
                }
            }
        }

        function ClearDates() {
            if (document.getElementById('<%=ChkAllDate.ClientID %>').checked == true) {
                document.getElementById('<%=txtFrmDate.ClientID %>').value = "";
                document.getElementById('<%=txtToDate.ClientID %>').value = "";
            }
        }
        
        function CheckAllFields(){
            var ChkList = document.getElementById('<%=ChkListFields.ClientID %>');
            var chkAll = document.getElementById('<%=ChkAllFields.ClientID %>');
            var checkBoxes = ChkList.getElementsByTagName('input');
            if (chkAll.checked == false){
                for (i = 0; i < checkBoxes.length; i++) {
                    if (checkBoxes[i].type == 'checkbox' || checkBoxes[i].type == 'CHECKBOX') {
                        checkBoxes[i].checked = false;
                    }
                }
            }
            else if (chkAll.checked == true){
                for (i = 0; i < checkBoxes.length; i++) {
                    if (checkBoxes[i].type == 'checkbox' || checkBoxes[i].type == 'CHECKBOX') {
                        checkBoxes[i].checked = true;
                    }
                }
            }
        }
        
        function DisplayPanels()
        {
            var panelNew = document.getElementById('<%=PnlNewProjectsHeader.ClientID %>')
            var panelOld = document.getElementById('<%=PnlOldProjectsHeader.ClientID %>')
            panelNew.style.display = 'block';
            panelOld.style.display = 'block';
        }
    </script>

    <div runat="server" id="DvMainSearch">
        <table border="0" cellpadding="2" cellspacing="3" style="width: 685px">
            <tr>
                <td style="width: 97px">
                    Project Type :
                </td>
                <td style="width: 79px">
                    <asp:DropDownList runat="server" ID="DdlProjectType" CssClass="dropDownList" AutoPostBack="false">
                    </asp:DropDownList>
                </td>
                <td style="width: 7px">
                    &nbsp;&nbsp;
                </td>
                <td style="width: 55px">
                    Status :
                </td>
                <td align="left" style="width: 252px">
                    <asp:DropDownList runat="server" ID="DdlProjectStatus" CssClass="dropDownList" AutoPostBack="false">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr runat="server" id="trDateRange">
                <td colspan="3">
                    &nbsp;
                </td>
                <td colspan="5" align="right">
                    From :&nbsp;
                    <asp:TextBox ID="txtFrmDate" runat="server" CssClass="textBox" Width="70px"></asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFrmDate"  Format="dd-MMM-yyyy"></cc1:CalendarExtender>
                    
                    <asp:TextBox runat="server" ID="txtToDate" CssClass="textBox" Width="70px"></asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"  Format="dd-MMM-yyyy"></cc1:CalendarExtender>
                    
                    <asp:CheckBox runat="server" Text="All" ID="ChkAllDate" Checked="true" OnClick="ClearDates();" />
                </td>
            </tr>
        </table>
        <br />
        <asp:UpdatePanel runat="server" ID="UpAdvSearch">
            <ContentTemplate>
                <asp:Button runat="server" ID="BtnPnlSearch" Style="display: none;" />
                <table border="0" width="950px">
                    <tr style="cursor: pointer;" onclick="ShowHideDiv('<%=DvAdvSearch.ClientID%>','imgAdvSearch','<%=BtnPnlSearch.ClientID%>');">
                        <td style="vertical-align: middle; width: 100%;" align="left">
                            <div class="roundedHeader" style="background-color: #1560a1; height: 20px; padding-right: 3px;
                                padding-left: 3px; padding-bottom: 0px; vertical-align: middle; width: 100%;
                                cursor: pointer; padding-top: 5px; min-width: 100%" id="div3" runat="server">
                                <div style="font-weight: bold; font-size: 13px; float: none; vertical-align: middle;
                                    color: white">
                                    Advance search
                                </div>
                            </div>
                        </td>
                        <td style="vertical-align: middle; width: 10px;">
                            <div class="roundedHeader" style="background-color: #1560a1; height: 25px; float: right;
                                vertical-align: middle; margin: 1px;">
                                <img style="margin: 3px;" id="imgAdvSearch" alt="Image" src="images/expand.jpg" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div runat="server" id="DvAdvSearch" style="display: none; width: 100%; min-width: 100%"
                                class="collapsePanel" align="center">
                                <asp:Panel ID="ContentPnl" runat="server">
                                    <table width="100%" border="0" cellpadding="5">
                                        <tr align="left">
                                            <td width="72">
                                                Sponser :
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="DDlSponser" CssClass="dropDownList" Width="300px">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                Drug :
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="DDlDrugs" CssClass="dropDownList" Width="420px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="100%" border="0" cellpadding="5">
                                        <tr align="left">
                                            <td>
                                                Location :
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="DdlLocation" CssClass="dropDownList" Width="180px">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                Study Type :
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="DdlStudyType" CssClass="dropDownList" Width="180px">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                Submission :
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="DdlSubmission" CssClass="dropDownList" Width="180px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                        </td>
                    </tr>
                </table>
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="BtnSelectFields" runat="server" Text="Select Fields" CssClass="btn btnnew" />
                        </td>
                        <td>
                            <asp:Button runat="server" ID="BtnSearch" Text="Search" CssClass="btn btnnew" BorderStyle="Ridge"
                                OnClientClick="return CheckCheckedFields();" />
                        </td>
                        <td>
                            <asp:Button ID="BtnReset" runat="server" Text="Reset" CssClass="btn btncancel" />
                        </td>
                    </tr>
                </table>
                <cc1:ModalPopupExtender ID="MPFields" runat="server" PopupControlID="DvFields" PopupDragHandleControlID="LblPopUpTitle"
                    BackgroundCssClass="modalBackground" TargetControlID="BtnSelectFields" CancelControlID="ImgPopUp">
                </cc1:ModalPopupExtender>
                <div id="DvFields" runat="server" style="display: none; border: outset 2px black;
                    font-size: 8pt; height: 290px; width: 442px; background-color: #FFFFFF; text-align: left;
                    margin-top: 0px; margin-left: 0px;">
                    <table runat="server" id="TBFields" border="0">
                        <tr>
                            <td style="height: 19px" align="center">
                                <asp:Label ID="LblPopUpTitle" runat="server" class="LabelBold" Visible="true" Text="Select Fields to display"
                                    ForeColor="#003399"></asp:Label>
                            </td>
                            <td align="right">
                                <img id="ImgPopUp" alt="Close" src="images/Sqclose.gif" style="position: relative;
                                    float: right; right: 8px;" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:CheckBox runat="server" ID="ChkAllFields" Text="All" onClick="CheckAllFields();" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:CheckBoxList runat="server" ID="ChkListFields" BorderColor="#0033CC" BorderStyle="Double"
                                    Width="172px" CellPadding="2" CellSpacing="2" Height="85px" RepeatColumns="5"
                                    RepeatDirection="Horizontal" Style="margin-right: 0px">
                                    <asp:ListItem Text="Drug" Value="vDrugName" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Fastfed" Value="cFastingFed"></asp:ListItem>
                                    <asp:ListItem Text="Study Result" Value="StudyResult"></asp:ListItem>
                                    <asp:ListItem Text="Dosing(Period2)" Value="Dosing2"></asp:ListItem>
                                    <asp:ListItem Text="Checkin(Period2)" Value="CheckIn2"></asp:ListItem>
                                    <asp:ListItem Text="Brand" Value="vBrandName"></asp:ListItem>
                                    <asp:ListItem Text="Location" Value="Location"></asp:ListItem>
                                    <asp:ListItem Text="Project Manager" Value="vProjectManager"></asp:ListItem>
                                    <asp:ListItem Text="Dosing(Period3)" Value="dosing3"></asp:ListItem>
                                    <asp:ListItem Text="Checkin(Period3)" Value="checkin3"></asp:ListItem>
                                    <asp:ListItem Text="Status" Value="cProjectStatus"></asp:ListItem>
                                    <asp:ListItem Text="RequestId" Value="vRequestId"></asp:ListItem>
                                    <asp:ListItem Text="No. of Subjects" Value="iNoOfSubjects" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Dosing(Period4)" Value="dosing4"></asp:ListItem>
                                    <asp:ListItem Text="Checkin(Period4)" Value="checkin4"></asp:ListItem>
                                    <asp:ListItem Text="Details" Value="Details" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Submission" Value="vRegionName" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="RetentionPeriod" Value="nRetaintionPeriod" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Sponser" Value="vClientName" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Project No" Value="vProjectNo" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Dosing(Period1)" Value="dosing1"></asp:ListItem>
                                    <asp:ListItem Text="Checkin(Period1)" Value="CheckIn1"></asp:ListItem>
                                    <asp:ListItem Text="Project Co-Ordinator" Value="coordinator"></asp:ListItem>
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 19px" align="center" colspan="2">
                                <asp:Button runat="server" ID="BtnSearchProject" Text="Search" CssClass="btn btnnew"
                                    BorderStyle="Ridge" OnClientClick="return CheckCheckedFields();" />
                            </td>
                        </tr>
                    </table>
                </div>
                <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="PnlNewProjects"
                    CollapseControlID="PnlNewProjectsHeader" ExpandControlID="PnlNewProjectsHeader"
                    Collapsed="false" ExpandedImage="~/images/collapse.jpg" CollapsedImage="~/images/expand.jpg"
                    ImageControlID="ImgColl1">
                </cc1:CollapsiblePanelExtender>
                <asp:Panel runat="server" ID="PnlNewProjectsHeader" Width="798px" BorderStyle="Solid"
                    BorderWidth="1" Style="display: none;">
                    <table width="100%">
                        <tr>
                            <td align="left">
                                <strong>Projects Enrolled in BizNET</strong>
                            </td>
                            <td align="right">
                                <img src="images/collapse.jpg" id="ImgColl1" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel runat="server" ID="PnlNewProjects" Width="800px" Height="50px" ScrollBars="Auto">
                    <asp:GridView ID="GvNewProjects" runat="server" AllowPaging="True" AllowSorting="True"
                        AutoGenerateColumns="false" CssClass="gvwProjects" EmptyDataText="No Projects Found for this selection"
                        RowStyle-Wrap="false" RowStyle-Height="10px" Width="768px" SkinID="grdView" ShowFooter="True"
                        CellPadding="3">
                        <Columns>
                            <asp:BoundField HeaderText="Project No" DataField="vProjectNo" />
                            <asp:BoundField HeaderText="Request Id" DataField="vRequestId" />
                            <asp:BoundField HeaderText="Sponser" DataField="vClientName" />
                            <asp:BoundField HeaderText="Drug" DataField="vDrugName" />
                            <asp:BoundField HeaderText="Brand" DataField="vBrandName" />
                            <asp:BoundField HeaderText="Project Manager" DataField="vProjectManager" />
                            <asp:BoundField HeaderText="Project Co-Ordinator" DataField="vProjectCoordinator" />
                            <asp:BoundField HeaderText="No. of Subjects" DataField="iNoOfSubjects" />
                            <asp:BoundField HeaderText="RetentionPeriod" DataField="nRetaintionPeriod" />
                            <asp:BoundField HeaderText="Fastfed" DataField="cFastingFed" />
                            <asp:BoundField HeaderText="Location" DataField="vLocationName" />
                            <asp:BoundField HeaderText="Submission" DataField="vRegionName" />
                            <asp:BoundField HeaderText="Status" DataField="cProjectStatusDesc" />
                            <asp:BoundField HeaderText="Checkin(Period1)" DataField="Check-In(1) Start" />
                            <asp:BoundField HeaderText="Dosing(Period1)" DataField="IP Administration(1) Start" />
                            <asp:BoundField HeaderText="Checkin(Period2)" DataField="Check-In(2) Start" />
                            <asp:BoundField HeaderText="Dosing(Period2)" DataField="IP Administration(2) Start" />
                            <asp:BoundField HeaderText="Checkin(Period3)" DataField="Check-In(3) Start" />
                            <asp:BoundField HeaderText="Dosing(Period3)" DataField="IP Administration(3) Start" />
                            <asp:BoundField HeaderText="Checkin(Period4)" DataField="Check-In(4) Start" />
                            <asp:BoundField HeaderText="Dosing(Period4)" DataField="IP Administration(4) Start" />
                            <asp:BoundField HeaderText="Initial Quotation" DataField="InitialQuotationStart" />
                            <asp:BoundField HeaderText="Not Rewarded" DataField="NotRewardedStart" />
                            <asp:BoundField HeaderText="Pre-Clinical" DataField="PreClinicalStart" />
                            <asp:BoundField HeaderText="Study" DataField="StudyStart" />
                            <asp:BoundField HeaderText="Sample Analysis" DataField="AnalyticalStart" />
                            <asp:BoundField HeaderText="Document Preparation" DataField="DocumentStart" />
                            <asp:BoundField HeaderText="Completed" DataField="ComplitedStart" />
                            <asp:BoundField HeaderText="Terminated" DataField="TerminatedDate" />
                            <asp:BoundField HeaderText="Archived" DataField="ArchivedDate" />
                            <asp:TemplateField HeaderText="Detail">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LkbDetail" runat="server" CommandName="DETAIL">Detail</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="vWorkSpaceId" DataField="vWorkSpaceId" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                <br />
                <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="PnlOldProjects"
                    CollapseControlID="PnlOldProjectsHeader" ExpandControlID="PnlOldProjectsHeader"
                    Collapsed="false" ExpandedImage="~/images/collapse.jpg" CollapsedImage="~/images/expand.jpg"
                    ImageControlID="ImgColl2">
                </cc1:CollapsiblePanelExtender>
                <asp:Panel runat="server" ID="PnlOldProjectsHeader" Width="798px" BorderStyle="Solid"
                    BorderWidth="1" Style="display: none; margin-left: 0px;">
                    <table style="width: 100%">
                        <tr>
                            <td align="left">
                                <strong>Projects not Enrolled in BizNET</strong>
                            </td>
                            <td align="right">
                                <img src="images/collapse.jpg" id="ImgColl2" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel runat="server" ID="PnlOldProjects" Width="800px" Height="50px" ScrollBars="Auto">
                    <asp:GridView ID="GvOldProjects" runat="server" AllowPaging="True" AllowSorting="True"
                        AutoGenerateColumns="false" CssClass="gvwProjects" EmptyDataText="No Projects Found for this selection"
                        RowStyle-Wrap="false" RowStyle-Height="10px" Width="768px" SkinID="grdView" ShowFooter="True"
                        CellPadding="3">
                        <Columns>
                            <asp:BoundField HeaderText="Project No" DataField="vProjectNo" />
                            <asp:BoundField HeaderText="Sponser" DataField="vClientName" />
                            <asp:BoundField HeaderText="Drug" DataField="vDrugName" />
                            <asp:BoundField HeaderText="No. of Subjects" DataField="iNoOfSubjects" />
                            <asp:BoundField HeaderText="Project Co-Ordinator" DataField="vProjectCoordinator" />
                            <asp:BoundField HeaderText="RetentionPeriod" DataField="nRetaintionPeriod" />
                            <asp:BoundField HeaderText="Location" DataField="vLocationName" />
                            <asp:BoundField HeaderText="Submission" DataField="vRegionName" />
                            <asp:BoundField HeaderText="Status" DataField="cProjectDesc" />
                            <asp:BoundField HeaderText="Checkin(Period1)" DataField="Check-In(1) Start" />
                            <asp:BoundField HeaderText="Dosing(Period1)" DataField="IP Administration(1) Start" />
                            <asp:BoundField HeaderText="Checkin(Period2)" DataField="Check-In(2) Start" />
                            <asp:BoundField HeaderText="Dosing(Period2)" DataField="IP Administration(2) Start" />
                            <asp:BoundField HeaderText="Checkin(Period3)" DataField="Check-In(3) Start" />
                            <asp:BoundField HeaderText="Dosing(Period3)" DataField="IP Administration(3) Start" />
                            <asp:BoundField HeaderText="Checkin(Period4)" DataField="Check-In(4) Start" />
                            <asp:BoundField HeaderText="Dosing(Period4)" DataField="IP Administration(4) Start" />
                            <asp:BoundField HeaderText="Result" DataField="StudyResults" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="BtnSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="DDlSponser" EventName="DataBinding" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
