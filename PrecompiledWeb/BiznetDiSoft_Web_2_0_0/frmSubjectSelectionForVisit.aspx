<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmSubjectSelectionForVisit, App_Web_qa4vhgvp" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:content id="Content1" contentplaceholderid="CPHLAMBDA" runat="Server">

    <style type="text/css">
        .textArea {
            font-family: Verdana, Arial, Helvetica, sans-serif;
            border-radius: 7px;
            padding: 1px;
            height: 30px;
            border-style: solid;
            border-width: 1PX;
        }

        .MyCalendar .ajax__calendar_container {
            border: 2px solid;
            border-color: black;
            background-color: white;
            color: navy;
        }

        .dataTables_scrollBody datateble {
            width: 0% !important;
        }

        #ctl00_CPHLAMBDA_gvwSubjectSelectionForVisit_wrapper {
            width: 100% !important;
            /*height: 300px !important;*/
        }

        #ctl00_CPHLAMBDA_gvwSubjectSelectionForVisit_wrapper table thead {
            max-height: 20% !important;
            overflow: auto !important;
        }

        #ctl00_CPHLAMBDA_gvwSubjectSelectionForVisit_wrapper table tbody {
            width: 100%;
            height: 200px !important;
            overflow: auto !important;
        }

        #ImgSubjectMstClose.Close {
            content: '\00d7';
            color: white;
            font-weight: bold;
            font-size: 18px;
            line-height: 22px;
            font-family: 'Courier New', Courier, monospace;
            position: absolute;
            top: -11px;
            right: -11px;
            width: 25px;
            height: 25px;
            border: 2px solid white;
            background-color: black;
            text-align: center;
            border-radius: 15px;
            cursor: pointer;
            z-index: 12;
            box-shadow: 2px 2px 6px #111;
            padding: 0px;
        }

        #ctl00_CPHLAMBDA_gvScreenFailureAuditTrail_wrapper {
            width: 800px !important;
            height: 200px !important;
            overflow: auto !important;
        }

        #ctl00_CPHLAMBDA_gvScreenFailureAuditTrail {
            width: 100% !important;
        }

        #ctl00_CPHLAMBDA_gvAudittrail {
            width: 100% !important;
            /*height : 600px !Important;*/
        }

        #ctl00_CPHLAMBDA_gvScreenFailureAuditTrail_wrapper table thead {
            max-height: 200px !important;
            min-height: 50px !important;
            overflow: auto !important;
            overflow-x: auto !important;
            overflow-y: auto !important;
        }

        #ctl00_CPHLAMBDA_gvScreenFailureAuditTrail_wrapper table tbody {
            height: 200px !important;
            overflow: auto !important;
        }

        #ctl00_CPHLAMBDA_gvScreenFailureAuditTrail_info tbody {
            width: 100% !important;
            height: 300px !important;
        }

        #ctl00_CPHLAMBDA_gvAudittrail_wrapper {
            width: 100% !important;
            height: 270px !important;
            min-height: 50px !important;
            overflow: auto !important;
        }

        #ctl00_CPHLAMBDA_gvAudittrail_wrapper table thead {
            height: 200px !important;
            /*overflow: auto !important;
            overflow-x: auto !important;
            overflow-y: auto !important;*/
        }

        #ctl00_CPHLAMBDA_gvAudittrail_wrapper table tbody {
            height: 200px !important;
            overflow: auto !important;
        }

        .dataTables_scrollHeadInner {
            width: 100% !Important;
        }

        h2 {
            font-size: 1.1550rem !important;
            margin-top:14px!important;
        }

        #ctl00_CPHLAMBDA_GvwDeletedSubjectAuditTrail_wrapper {
            width:640px!important;
        }

        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }
    </style>

    <script src="Script/jquery-1.9.1.js" type="text/javascript"></script>

    <script src="Script/jquery-ui.js" type="text/javascript"></script>

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" src="Script/General.js"></script>

    <script src="Script/Validation.js" language="javascript" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">

        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        function ClientPopulatedSubject(sender, e) {
            SubjectClientShowing('AutoCompleteExtenderSubject', $get('<%= txtSubject.ClientId %>'));
        }

        function OnSelectedSubject(sender, e) {
            SubjectOnItemSelected(e.get_value(), $get('<%= txtSubject.clientid %>'),
            $get('<%= HSubject.clientid %>'), document.getElementById('<%= btnSetSubject.ClientId %>'));
        }

        function CheckProjectName() {
            if (document.getElementById('<%=txtProject.ClientID%>').value.toString().trim().length <= 0) {
                msgalert("Please Select The Project !")
                return false;
            }
            return true;
        }

        function ClosePopUp() {
            $get('ImgRejectionOrDeletion').click();
            return false;
        }

        function pageLoad() {

            $(document).keydown(function (event) {
                if (event.keyCode == '27') {
                    if ($get('<%= divSubjectRejectorDelete.ClientID %>').style.display != 'none') {
                        $get('ImgRejectionOrDeletion').click();
                    }

                    if ($get('<%= divSubjectMst.ClientID %>').style.display != 'none') {
                        $get('ImgSubjectMstClose').click();
                    }

                }
            });

        }

        function ShowConfirmation() {
            fsetPatient_Show();
            //var result = confirm("Subject With Entered Initials Is Already Enrolled In Project. Do You Still Want To Continue ?");
            //if (result) {
            //    document.getElementById('<%=Btnhidden.Clientid %>').click();
            //    document.getElementById('<%=BtnSaveSubjectMst.Clientid %>').style.display = 'none';
            //    return true;
            //}
            //else {
            //    return false;
            //}

            msgConfirmDeleteAlert(null, "Subject With Entered Initials Is Already Enrolled In Project. Do You Still Want To Continue ?", function (isConfirmed) {
                if (isConfirmed) {
                    document.getElementById('<%=Btnhidden.Clientid %>').click();
                    document.getElementById('<%=BtnSaveSubjectMst.Clientid %>').style.display = 'none';
                    return true;
                } else {
                    return false;
                }
            });

            return false;
        }
    </script>

    <table cellpadding="5px" style="width: 90%; margin: auto;">
        <tbody>
            <tr>
                <td>
                    <fieldset class="FieldSetBox" style="display: block; width: 98%; text-align: left; border: #aaaaaa 1px solid;">
                        <legend class="LegendText" style="color: Black; font-size: 12px">
                            <img id="img2" alt="Subject Details" src="images/panelcollapse.png"
                                onclick="Display(this,'divPatientDetail');" runat="server" style="margin-right: 2px;" />Subject Details</legend>
                        <div id="divPatientDetail">
                            <table width="100%">
                                <tr>
                                    <td class="Label" nowrap="nowrap" style="text-align: left;">Project No/Site Id* :
                                        <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="30%" MaxLength="50"></asp:TextBox>
                                        <asp:Button Style="display: none" ID="btnSetProject" OnClick="btnSetProject_Click"
                                            runat="server" Text=" Project"></asp:Button>
                                        <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                            CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                            CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                            OnClientShowing="ClientPopulated" ServiceMethod="GetMyProjectCompletionList"
                                            ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True"
                                            CompletionListElementID="pnlProjectList">
                                        </cc1:AutoCompleteExtender>
                                        <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 300px; overflow: auto; overflow-x: hidden" />
                                        <asp:HiddenField ID="HLocationCode" runat="server"></asp:HiddenField>
                                        Subject Initial/SubjectNo/RandomizationNo :
                                        <asp:TextBox ID="txtSubject" runat="server" CssClass="textBox" Width="20%" MaxLength="50"></asp:TextBox>
                                        <asp:Button Style="display: none" ID="btnSetSubject" OnClick="btnSetSubject_Click"
                                            runat="server" Text=" Subject"></asp:Button>
                                        <asp:HiddenField ID="HSubject" runat="server"></asp:HiddenField>
                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtenderSubject" runat="server" BehaviorID="AutoCompleteExtenderSubject"
                                            CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                            CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelectedSubject"
                                            OnClientShowing="ClientPopulatedSubject" ServiceMethod="GetSubjectCompletionList_Assigned_NotRejected"
                                            ServicePath="AutoComplete.asmx" TargetControlID="txtSubject" UseContextKey="True"
                                            CompletionListElementID="pnlSubjectList">
                                        </cc1:AutoCompleteExtender>
                                        <asp:Panel ID="pnlSubjectList" runat="server" Style="max-height: 300px; overflow: auto; overflow-x: hidden" />
                                        <asp:Label runat="server" ID="lblPeriod" Text="Period :" />
                                        <asp:DropDownList runat="server" ID="ddlPeriod" class="dropDownList" AutoPostBack="True"
                                            Width="5%">
                                        </asp:DropDownList>

                                    </td>
                                </tr>

                                <tr>
                                    <td class="Label" nowrap="nowrap" style="text-align: center; padding-top: 2%;" colspan="2">
                                        <asp:Button ID="btnAdd" runat="server" Text="Add New Subject" ToolTip="Add New Subject"
                                            CssClass="btn btnnew" Width="12%" OnClientClick="return CheckProjectName();"></asp:Button>

                                        <asp:Button ID="btnExit" runat="server" Text="Exit" ToolTip="Exit" CssClass="btn btnclose"
                                            CausesValidation="False" Width="6%" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);"></asp:Button>

                                        <asp:Button ID="BtnDeletedSubjects" ToolTip="AuditTrail Of Deleted Subjects"
                                            runat="server" CssClass="btn btnaudit" Visible="false" />
                                    </td>
                                </tr>
                                
                                <%--OnClientClick="return confirm('Are You Sure You Want To Exit?')"></asp:Button>--%>
                                <tr style="text-align: center; text-align: center;">
                                    <td align="center" style="text-align: center; text-align: center;">
                                        <table>
                                            <tr>
                                                <td>
                                                    <button id="btnMpeSubjectMst" runat="server" style="display: none;" />
                                                    <cc1:ModalPopupExtender ID="MpeSubjectMst" runat="server" PopupControlID="divSubjectMst" BehaviorID="MpeSubjectMst"
                                                        BackgroundCssClass="modalBackground" TargetControlID="btnMpeSubjectMst" CancelControlID="ImgSubjectMstClose">
                                                    </cc1:ModalPopupExtender>
                                                    <div style="display: none; left: 330px; width: 28%; top: 367px; text-align: center; border-radius: 15px; background-color: white"
                                                        id="divSubjectMst1" class="DIVSTYLE2" runat="server">
                                                        <%--<table width="100%">
                                                            <tr>
                                                                <td align="center" style="font-weight: bold; font-size: small;" class="Label ">Subject Enrollment
                                                                </td>
                                                                <td align="right">
                                                                    <img id="ImgSubjectMstClose" src="images/close_pop.png" style="position: relative; float: right; right: 5px;"
                                                                        title="Close" onclick="SubjectMstDivShowHide('H');" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <hr />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:UpdatePanel ID="upSubjectMst" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:HiddenField ID="HReplaceImySubjectNo" runat="server"></asp:HiddenField>
                                                                            <table cellpadding="3px">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td class="Label" style="text-align: right;">First Name :
                                                                                        </td>
                                                                                        <td style="text-align: initial;">
                                                                                            <asp:TextBox ID="txtFirstName" runat="server" MaxLength="1" onblur="SetInitial();" CssClass="textBox" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="Label" style="text-align: right;">Middle Name :
                                                                                        </td>
                                                                                        <td style="text-align: left;">
                                                                                            <asp:TextBox ID="txtMiddleName" runat="server" MaxLength="1" onblur="SetInitial();" CssClass="textBox">
                                                                                            </asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="Label" style="text-align: right;">Last Name :
                                                                                        </td>
                                                                                        <td style="text-align: left;">
                                                                                            <asp:TextBox ID="txtLastName" runat="server" MaxLength="1" onblur="SetInitial();" CssClass="textBox" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="Label" style="text-align: right;">Initial*
                                                                                        </td>
                                                                                        <td style="text-align: left;">
                                                                                            <asp:TextBox ID="txtInitial" runat="server" MaxLength="3" CssClass="textBox" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr id="Tr3" runat="server" visible="false">
                                                                                        <td class="Label" style="text-align: right;">Date Of ICF Signed* :
                                                                                        </td>
                                                                                        <td style="text-align: left;">
                                                                                            <asp:TextBox ID="txtICFSignedDate" runat="server">
                                                                                            </asp:TextBox><cc1:CalendarExtender ID="CalExtICFDate" runat="server" Format="dd-MMM-yyyy"
                                                                                                TargetControlID="txtICFSignedDate" CssClass="MyCalendar">
                                                                                            </cc1:CalendarExtender>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="Label" style="text-align: right;">Subject  No* :
                                                                                        </td>
                                                                                        <td style="text-align: left;">
                                                                                            <asp:TextBox ID="txtScreenNo" runat="server" CssClass="textBox">
                                                                                            </asp:TextBox>
                                                                                        </td>
                                                                                    </tr>

                                                                                    <tr runat="server" id="trPatientRandomizationNo" style="display: none">
                                                                                        <td class="Label" style="text-align: right;">Randomization No :
                                                                                        </td>
                                                                                        <td style="text-align: left;">
                                                                                            <asp:TextBox ID="txtPatientRandomizationNo" runat="server" Enabled="True" CssClass="textBox" />
                                                                                            <asp:HiddenField ID="hdnRandomizationType" runat="server"></asp:HiddenField>
                                                                                            <asp:HiddenField ID="hdncScreenFailure" runat="server"></asp:HiddenField>
                                                                                            <asp:HiddenField ID="hdncDisContinue" runat="server"></asp:HiddenField>
                                                                                        </td>
                                                                                        <td style="text-align: left;">
                                                                                            <img id="ImgRandomizationNO" src="images/Randomization.png" style="position: relative; float: right; right: 5px; height: 30px; width: 30px"
                                                                                                title="Randomization" onclick="RandomizationNoAutoManual();" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr runat="server" id="trRemarks" style="display: none">
                                                                                        <td class="Label" style="text-align: right;">Remarks* :
                                                                                        </td>
                                                                                        <td style="text-align: left;">
                                                                                            <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" CssClass="textBox" Height="50px" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="Label" colspan="2" style="text-align: center;">
                                                                                            <asp:Button ID="BtnSaveSubjectMst" runat="server" Text="Save" ToolTip="Save" CssClass="btn btnsave" />
                                                                                            <asp:Button ID="Btnhidden" runat="server" Text="Hidden" CssClass="button" Style="display: none" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="gvwSubjectSelectionForVisit" />
                                                                            <asp:AsyncPostBackTrigger ControlID="btnAdd" />
                                                                            <asp:AsyncPostBackTrigger ControlID="BtnSaveSubjectMst" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                        </table>--%>
                                                    </div>

                                                    <div class="modal-content modal-sm" id="divSubjectMst" style="display:none;" runat="server">
                                                        <div class="modal-header">
                                                            <img src="images/Sqclose.gif" class="ModalImage" onclick="ModalClose();" alt="" id="ImgSubjectMstClose"/>
                                                            <h2>Subject Enrollment</h2>
                                                        </div>
                                                        <div class="modal-body">
                                                            <table style="width:100%;margin-left:15%">
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:UpdatePanel ID="upSubjectMst" runat="server">
                                                                            <ContentTemplate>
                                                                                <asp:HiddenField ID="HReplaceImySubjectNo" runat="server"></asp:HiddenField>
                                                                                <table cellpadding="3px">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td class="Label" style="text-align: right;">First Initial :
                                                                                            </td>
                                                                                            <td style="text-align: initial;">
                                                                                                <asp:TextBox ID="txtFirstName" runat="server" MaxLength="1" onblur="SetInitial();" CssClass="textBox" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="Label" style="text-align: right;">Middle Initial :
                                                                                            </td>
                                                                                            <td style="text-align: left;">
                                                                                                <asp:TextBox ID="txtMiddleName" runat="server" MaxLength="1" onblur="SetInitial();" CssClass="textBox">
                                                                                                </asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="Label" style="text-align: right;">Last Initial :
                                                                                            </td>
                                                                                            <td style="text-align: left;">
                                                                                                <asp:TextBox ID="txtLastName" runat="server" MaxLength="1" onblur="SetInitial();" CssClass="textBox" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="Label" style="text-align: right;">Initial*
                                                                                            </td>
                                                                                            <td style="text-align: left;">
                                                                                                <asp:TextBox ID="txtInitial" runat="server" MaxLength="3" CssClass="textBox" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="Tr3" runat="server" visible="false">
                                                                                            <td class="Label" style="text-align: right;">Date Of ICF Signed* :
                                                                                            </td>
                                                                                            <td style="text-align: left;">
                                                                                                <asp:TextBox ID="txtICFSignedDate" runat="server">
                                                                                                </asp:TextBox><cc1:CalendarExtender ID="CalExtICFDate" runat="server" Format="dd-MMM-yyyy"
                                                                                                    TargetControlID="txtICFSignedDate" CssClass="MyCalendar">
                                                                                                </cc1:CalendarExtender>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="Label" style="text-align: right;">Subject  No* :
                                                                                            </td>
                                                                                            <td style="text-align: left;">
                                                                                                <asp:TextBox ID="txtScreenNo" runat="server" CssClass="textBox" onblur="return removeSpecialCharacter(this);">
                                                                                                </asp:TextBox>
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr runat="server" id="trPatientRandomizationNo" style="display: none">
                                                                                            <td class="Label" style="text-align: right;">Randomization No :
                                                                                            </td>
                                                                                            <td style="text-align: left;">
                                                                                                <asp:TextBox ID="txtPatientRandomizationNo" runat="server" Enabled="True" CssClass="textBox" onblur="return removeSpecialCharacter(this);" />
                                                                                                <asp:HiddenField ID="hdnRandomizationType" runat="server"></asp:HiddenField>
                                                                                                <asp:HiddenField ID="hdncScreenFailure" runat="server"></asp:HiddenField>
                                                                                                <asp:HiddenField ID="hdncDisContinue" runat="server"></asp:HiddenField>
                                                                                            </td>
                                                                                            <td style="text-align: left;">
                                                                                                <img id="ImgRandomizationNO" src="images/Randomization.png" style="position: relative; float: right; right: 5px; height: 30px; width: 30px"
                                                                                                    title="Randomization" onclick="RandomizationNoAutoManual();" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server" id="trRemarks" style="display: none">
                                                                                            <td class="Label" style="text-align: right;">Remarks* :
                                                                                            </td>
                                                                                            <td style="text-align: left;">
                                                                                                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" CssClass="textBox" Height="50px" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="Label" colspan="2" style="text-align: center;">
                                                                                                <%--<asp:Button ID="BtnSaveSubjectMst" runat="server" Text="Save" ToolTip="Save" CssClass="btn btnsave" />
                                                                                                <asp:Button ID="Btnhidden" runat="server" Text="Hidden" CssClass="button" Style="display: none" />--%>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </ContentTemplate>
                                                                            <Triggers>
                                                                                <asp:AsyncPostBackTrigger ControlID="gvwSubjectSelectionForVisit" />
                                                                                <asp:AsyncPostBackTrigger ControlID="btnAdd" />
                                                                                <asp:AsyncPostBackTrigger ControlID="BtnSaveSubjectMst" />
                                                                            </Triggers>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>

                                                        <div class="modal-header">
                                                            <asp:Button ID="BtnSaveSubjectMst" runat="server" Text="Save" ToolTip="Save" CssClass="btn btnsave" />
                                                            <asp:Button ID="Btnhidden" runat="server" Text="Hidden" CssClass="button" Style="display: none" />
                                                        </div>
                                                    </div>


                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="upDemographic" runat="server">
                                                        <ContentTemplate>
                                                            <button id="btnDemographic" runat="server" style="display: none">
                                                            </button>
                                                            <cc1:ModalPopupExtender BackgroundCssClass="modalBackground" ID="mpeDemographic"
                                                                runat="server" TargetControlID="btnDemographic" CancelControlID="ImgClose" PopupControlID="divDemographicDetails">
                                                            </cc1:ModalPopupExtender>
                                                            <div id="divDemographicDetails" style="display: none; left: 424px; width: 500px; top: 367px; text-align: center"
                                                                class="DIVSTYLE2" runat="server">
                                                                <table style="width: 100%" cellpadding="3px">
                                                                    <thead>
                                                                        <tr>
                                                                            <td colspan="2" style="font-weight: bold; font-size: small; text-align: center;">Demographic Details
                                                                            </td>
                                                                            <td align="right">
                                                                                <img id="ImgClose" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px;"
                                                                                    title="Close" onclick="SubjectMstDivShowHide('HIDEDEMOGRAPHIC');" />
                                                                            </td>
                                                                        </tr>
                                                                    </thead>
                                                                    <tbody>
                                                                        <tr>
                                                                            <td style="text-align: right;" class="Label">Height*(Cms.) :
                                                                            </td>
                                                                            <td style="text-align: left;">
                                                                                <asp:TextBox runat="server" MaxLength="5" ID="txtHeight" /><%--onblur="CheckHeight(this);"--%>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="text-align: right;" class="Label">Weight*(Kg.) :
                                                                            </td>
                                                                            <td style="text-align: left;">
                                                                                <asp:TextBox runat="server" MaxLength="5" ID="txtWeight" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="text-align: right;" class="Label">BMI* :
                                                                            </td>
                                                                            <td style="text-align: left;">
                                                                                <asp:TextBox runat="server" MaxLength="5" ID="txtBMI" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="text-align: right;" class="Label">Date Of Birth* :
                                                                            </td>
                                                                            <td style="text-align: left;">
                                                                                <asp:TextBox runat="server" ID="txtDOB" onblur="DOBBlur()"></asp:TextBox><cc1:CalendarExtender
                                                                                    ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy" TargetControlID="txtDOB" CssClass="MyCalendar">
                                                                                </cc1:CalendarExtender>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="text-align: right;" class="Label">Age* :
                                                                            </td>
                                                                            <td style="text-align: left;">
                                                                                <asp:TextBox runat="server" MaxLength="4" ID="txtAGE" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="text-align: right;" class="Label">Sex* :
                                                                            </td>
                                                                            <td style="text-align: left;">
                                                                                <asp:RadioButtonList runat="server" ID="rblSex" RepeatDirection="Horizontal">
                                                                                    <asp:ListItem>Male</asp:ListItem>
                                                                                    <asp:ListItem>Female</asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="text-align: center;" colspan="2">
                                                                                <asp:Button runat="server" class="btn btnsave" ID="btnDemographicSubmit" Text="Submit"
                                                                                    ToolTip="Submit" OnClientClick="return chkSubmit();" />
                                                                                <input type="button" runat="server" class="btn btncancel" id="btnCancel" title="Clear All"
                                                                                    value="Clear All" onclick="ClearAll();" />
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnDemographicSubmit" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>

                                <tr align="center" style="text-align: center">
                                    <td align="center" style="text-align: center">
                                        <table>
                                            <tr>
                                                <td>
                                                    <button id="btnMpeRejectionOrDeletion" runat="server" style="display: none;" />
                                                    <cc1:ModalPopupExtender ID="MpeRejectionOrDeletion" runat="server" PopupControlID="divSubjectRejectorDelete"
                                                        BackgroundCssClass="modalBackground" TargetControlID="btnMpeRejectionOrDeletion"
                                                        CancelControlID="ImgRejectionOrDeletion">
                                                    </cc1:ModalPopupExtender>
                                                    <div class="modal-content modal-sm" id ="divSubjectRejectorDelete" style="display:none" runat="server">
                                                        <div class="modal-header">
                                                            <img id="ImgRejectionOrDeletion" src="images/close_pop.png" style="position: relative; float: right; right: 5px;" 
                                                                        title="Close" onclick="SubjectRejectOrDelete('H');" />
                                                            <h2>Subject Rejection Or Deletion</h2>
                                                        </div>
                                                        <div class="modal-body">
                                                            <table cellpadding="5px" style="width: 100%">
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <table width="100%">
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td class="Label" style="white-space: nowrap; text-align: right;">Select Reason* :
                                                                                    </td>
                                                                                    <td style="text-align: left;">
                                                                                        <asp:DropDownList ID="ddlReason" runat="server" Width="70%">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr style="height: 25px">
                                                                                    <td colspan="2" style="text-align: Center;" class="Label">OR
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="Label" style="text-align: right;">Remarks* :
                                                                                    </td>
                                                                                    <td style="text-align: left;">
                                                                                        <asp:TextBox runat="server" ID="TxtReason" MaxLength="250" TextMode="MultiLine" Text="" Width="70%" />
                                                                                        <asp:HiddenField ID="HdReasonDesc" runat="server" Value="" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr style="height: 25px">
                                                                                    <td colspan="2" style="text-align: Center;" class="Label"></td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div class="modal-footer">
                                                            <asp:Button ID="btnSave" runat="server" Text="Save" ToolTip="Save" CssClass="btn btnsave"
                                                                OnClientClick="return ValidationForRejectOrDelete();"></asp:Button>
                                                            <asp:Button ID="BtnClose" runat="server" Text="Close" ToolTip="Close" CssClass="btn btnclose"
                                                                OnClientClick="return ClosePopUp();"></asp:Button>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>

                                <tr align="center" style="text-align: center">
                                    <td align="center" style="text-align: center">
                                        <button id="BtnMpeAudittrail" runat="server" style="display: none;" />
                                        <cc1:ModalPopupExtender ID="MpeAudittrail" runat="server" PopupControlID="divAudittrail"
                                            BackgroundCssClass="modalBackground" TargetControlID="BtnMpeAudittrail" CancelControlID="ImgPopUpClose">
                                        </cc1:ModalPopupExtender>

                                        <div class="modal-content modal-lg" id="divAudittrail" runat="server" style="display:none">
                                            <div class="modal-header">
                                                <h2>
                                                    Audit Trail
                                                </h2>
                                                <img src="images/Sqclose.gif" class="ModalImage" id="ImgPopUpClose" alt="" 
                                                        onmouseover="this.style.cursor='pointer';" style="margin-top:-47px;" onclick="DeletedSubjectAuditTrailShow()"/>
                                            </div>
                                            <div class="modal-body">
                                                <asp:Panel ID="pnlDocAction" runat="server" Style="overflow: auto">
                                                    <table style="width: 100%" cellpadding="3">
                                                        <tr>
                                                            <td>
                                                                <asp:UpdatePanel ID="upAudittrailgrid" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:Panel runat="server" ID="pnlAudittrail" Width="100%" ScrollBars="Auto">
                                                                            <asp:GridView ID="gvAudittrail" runat="server"
                                                                                AutoGenerateColumns="False" Width="100%">
                                                                                <Columns>
                                                                                    <asp:BoundField HeaderText="#" DataField="iTranNo">
                                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="vMySubjectNo" HeaderText="SubjectNo" />
                                                                                    <asp:BoundField DataField="vInitials" HeaderText="Initials" />
                                                                                    <asp:BoundField DataField="vRandomizationNo" HeaderText="RandomizationNo" />
                                                                                    <asp:BoundField DataField="dICFDate" Visible="false" HeaderText="ICFDate" HtmlEncode="false" DataFormatString="{0:dd-MMM-yyyy}"
                                                                                        ItemStyle-Wrap="false" />
                                                                                    <asp:BoundField DataField="vRemarks" HeaderText="Remarks" />
                                                                                    <asp:BoundField DataField="vModifyBy" HeaderText="ModifyBy" />
                                                                                    <asp:BoundField DataField="dModifyOn" HeaderText="ModifyOn" HtmlEncode="false" ItemStyle-Wrap="false" />
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </asp:Panel>
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="gvwSubjectSelectionForVisit" EventName="PageIndexChanging" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td align="right"></td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </fieldset>
                </td>
            </tr>


        </tbody>
    </table>
    <div>
        <table>
            <%--'''For Deleted Subjects Audit Trail--%>
            <tr>
                <td>
                    <div>
                        <table>
                            <tr>
                                <td>
                                    <%--'''' ********************************************************************************* '''--%>
                                    <button id="BtnTempForDeletedSubjectPopUp" runat="server" style="display: none;" />
                                    <cc1:ModalPopupExtender ID="MpeDeletedSubjectPopUp" runat="server" PopupControlID="divDeletedSubjectMst"
                                        BackgroundCssClass="modalBackground" TargetControlID="BtnTempForDeletedSubjectPopUp"
                                        CancelControlID="ImgCloseDeletedSubject">
                                    </cc1:ModalPopupExtender>

                                    <div class="modal-content" id="divDeletedSubjectMst" style="display:none;width:50%;" runat="server">
                                        <div class="modal-header">
                                            <img src="images/Sqclose.gif" class="ModalImage" alt="" id="ImgCloseDeletedSubject" title="Close"/>
                                            <h2>AuditTrail Of Deleted Subjects</h2>
                                        </div>
                                        <div class="modal-body">
                                            <table align="center" width="100%">
                                                <tr>
                                                    <td align="center" colspan="2">
                                                        <asp:UpdatePanel runat="server" ID="UpPnlDeletedSubjects">
                                                            <ContentTemplate>
                                                                <asp:GridView ID="GvwDeletedSubjectAuditTrail" runat="server"
                                                                    ShowFooter="false" PageSize="25" AllowPaging="false" AutoGenerateColumns="False">
                                                                    <Columns>
                                                                        <asp:BoundField DataFormatString="number" HeaderText="#">
                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="vInitials" HeaderText="Initials" />
                                                                        <asp:BoundField DataField="vMySubjectNo" HeaderText="Subject No" />
                                                                        <asp:BoundField DataField="vRandomizationNo" HeaderText="RandomizationNo" />
                                                                        <asp:BoundField DataField="dICFDate" Visible="false" HeaderText="ICF Date" HtmlEncode="false" DataFormatString="{0:dd-MMM-yyyy}"
                                                                            ItemStyle-Wrap="false" />
                                                                        <asp:TemplateField HeaderText="Audit Trail">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="imgbtnAudittrailForDeletedSubjects" runat="server" ToolTip="Audit Trail"
                                                                                    ImageUrl="~/Images/audit.png" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="BtnDeletedSubjects" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>



                                    <div style="display: none; left: 424px; width: 580px; top: 367px; text-align: center;"
                                        id="divDeletedSubjectMst1" class="DIVSTYLE2" runat="server">
                                        
                                    </div>
                                    <%--''' *********************************************************************************** '''--%>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <%--''' *****************************************--%>
        </table>
    </div>
    <asp:UpdatePanel ID="upSubjectSelectionForVisit" runat="server" >
        <ContentTemplate>
            <table style="width: 90%; margin: auto;">
                <tr id="TrLegend" runat="server" visible="false">
                    <td style="text-align: center;">
                        <%-- <asp:Label runat="server" ID="LblRed" Width="20" BackColor="Red" Height="10"></asp:Label>
                        <b>Rejected Subjects</b>--%>
                    </td>
                </tr>
                <%--'' Tr For GridView--%>
                <tr>
                    <td>
                        <fieldset id="fsetPatient" runat="server" class="FieldSetBox" style="display: none; margin-left: 0.5%; width: 97.5%; text-align: left; border: #aaaaaa 1px solid;">
                            <legend class="LegendText" style="color: Black; font-size: 12px">
                                <img id="img3" alt="Patient Details" src="images/panelcollapse.png"
                                    onclick="Display(this,'divPatientData');" runat="server" style="margin-right: 2px;" />Enrolled Subject Data</legend>

                            <div id="divPatientData" style="height: auto">
                                <table width="100%">
                                    <tr>
                                        <td align="right">
                                            <span>
                                                <canvas id="myCanvas" width="12" height="12" style="font-weight: bold; color: white; background-color: #9fdbc6 !important;height:10px !important;"></canvas>
                                            </span>
                                            <span class="InActive">Screen Failure / Discontinue</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="gvwSubjectSelectionForVisit" runat="server"
                                                AutoGenerateColumns="False"
                                                Style="margin: auto;">
                                                <Columns>
                                                    <asp:BoundField DataFormatString="number" HeaderText="#">
                                                        <%--<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" />--%>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vSubjectId" HeaderText="Subject Id" />
                                                    <asp:BoundField DataField="vInitials" HeaderText="Initials" />
                                                    <asp:BoundField DataField="vWorkspaceSubjectId" HeaderText="vWorkspaceSubjectId" />
                                                    <asp:BoundField DataField="iPeriod" HeaderText="Period" />
                                                    <asp:BoundField DataField="vMySubjectNo" HeaderText="Subject No" />
                                                    <asp:BoundField DataField="vRandomizationNo" HeaderText="RandomizationNo" />
                                                    <asp:TemplateField HeaderText="Patient/Randomization No">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgBtnAssignment" runat="server" ImageUrl="~\images\tick.jpg" />
                                                            <asp:TextBox ID="txtRandomizationNo" runat="server"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="dICFDate" HeaderText="ICF Date" HtmlEncode="false" Visible="false" DataFormatString="{0:dd-MMM-yy}"
                                                        ItemStyle-Wrap="false" />
                                                    <asp:BoundField DataField="cRejectionFlag" HeaderText="Rejection Flag" />
                                                    <asp:TemplateField HeaderText="Reject" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="lnkbtnReject" runat="server" ToolTip="Reject" ImageUrl="~/images/reject.png" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Edit">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgbtnEdit" runat="server" ToolTip="Edit" ImageUrl="~/Images/Edit2.gif" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Delete">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgbtnDelete" runat="server" ToolTip="Delete" ImageUrl="~/Images/i_delete.gif" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Screen Failure">
                                                        <ItemTemplate>
                                                            <%--<asp:LinkButton runat="server" ID="lnkScreenFailure" >Update</asp:LinkButton>--%>

                                                            <asp:ImageButton ID="imgScreenFailure" runat="server" ToolTip="Screen Failure" ImageUrl="~/Images/ScreenFailure.png" OnClientClick="OpenModelPopup()"/>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Product/Kit Allocation" visible="false">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgKitAllocation" runat="server" ToolTip="Product/Kit Allocation" ImageUrl="~/Images/KitAllocation.png"
                                                                vSubjectNo='<%# Eval("vMySubjectNo") %>' vRandomizationNo='<%# Eval("vRandomizationNo")%>'
                                                                vWorkSpaceID='<%# Eval("vWorkSpaceID")%>' vSubjectID='<%# Eval("vSubjectId")%>' cScreenFailure='<%# Eval("cScreenFailure")%>'
                                                                cDisContinue='<%# Eval("cDisContinue")%>' OnClientClick="KitAllocation(this);" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Reverse Randomization" visible="false">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgReverseRandomization" runat="server" ToolTip="Reverse Randomization" ImageUrl="~/Images/Randomization.png"
                                                                vRandomizationNo='<%# Eval("vRandomizationNo")%>' vWorkspaceSubjectId='<%# Eval("vWorkspaceSubjectId")%>'
                                                                vSubjectID='<%# Eval("vSubjectId")%>' vWorkSpaceID='<%# Eval("vWorkSpaceID")%>' 
                                                                OnClientClick="ReverseRandomization(this);"  Height="30px" Width="30px"/>


                                                            
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateField>



                                                    <asp:TemplateField HeaderText="Audit Trail">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgbtnAudittrail" runat="server" ToolTip="Audit Trail" ImageUrl="~/Images/audit.png"></asp:ImageButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="cStatusIndi" HeaderText="Deleted"></asp:BoundField>
                                                    <asp:TemplateField HeaderText="Add/Edit</br>Demographic Details" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgbtnAddDemographic" runat="server" ToolTip="Demographic Details"
                                                                ImageUrl="~/images/item-add.png" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="cRandomizationType" HeaderText="cRandomizationType" />
                                                    <asp:BoundField DataField="cScreenFailure" HeaderText="cRandomizationType" />
                                                    <asp:BoundField DataField="cDisContinue" HeaderText="cRandomizationType" />
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                        <%--''Td For GridView--%>
                    </td>
                    <%--''Td Completed For GridView--%>
                </tr>
                <%--'' Tr Completed For GridView--%>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvwSubjectSelectionForVisit" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="upModel" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="ModalBackGround" runat="server" class="divModalBackGround" style="display: none; width: 100% !Important;"></div>
            <button id="btnModel" runat="server" style="display: none;" />

            <div id="divScreenFailuer" class="modal-content modal-lg" style="display:none" runat="server">
                <div class="modal-header">
                    <img id="CancelRemarks" src="images/close_pop.png" style="position: relative; float: right; cursor: pointer;" title="Close" />
                    <h2><asp:Label runat="server" ID="lblHeader" Text="Screen Failure / Discontinue"></asp:Label></h2>
                </div>
                <div class="modal-body">
                    <center>
                        <table>
                            <tr style="margin: 10px 10px 10px 10px ! Important; text-align: center" id="trScreenFailure" runat="server">
                                    <td class="Label" align="right"></td>
                                    <td align="center">
                                        <asp:RadioButtonList runat="server" ID="rbtScreen" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Screen Failure" Value="S"></asp:ListItem>
                                            <asp:ListItem Text="Discontinue" Value="D"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>

                                <tr style="margin: 10px 10px 10px 10px ! Important; text-align: center" id="tr1" runat="server">

                                    <td class="Label" align="right">Date:
                                    </td>
                                    <td align="center">
                                        <asp:TextBox runat="server" ID="txtScreenFailureDate" CssClass="textBox"></asp:TextBox>
                                        <cc1:CalendarExtender runat="server" TargetControlID="txtScreenFailureDate" Format="dd-MMM-yyyy">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>

                                <tr style="margin: 10px 10px 10px 10px ! Important; text-align: center" id="tr2" runat="server">

                                    <td class="Label" align="right">Remarks:
                                    </td>
                                    <td align="center">
                                        <asp:TextBox runat="server" ID="txtScreenFailureRemarks" onblur="characterlimit(this)" TextMode="MultiLine" CssClass="textArea"></asp:TextBox>
                                    </td>
                                </tr>
                                
                                <tr style="margin: 10px 10px 10px 10px ! Important; text-align: center" id="tr4" runat="server">

                                        <td class="Label" align="right">Visit:
                                        </td>
                                        <td align="center">
                                            <asp:DropDownList ID="ddlVisits" runat="server" CssClass="dropDownList" Width="20%" Style="width: 200px" TabIndex="2" AutoPostBack="True">
                                            </asp:DropDownList>
                                            <%--<asp:TextBox runat="server" ID="TextBox1" onblur="characterlimit(this)" TextMode="MultiLine" CssClass="textArea"></asp:TextBox>--%>
                                        </td>
                                    </tr>
                            </table>
                        </center>
                        <br />
                        <center>
                            <table>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btnScreenSave" runat="server" OnClientClick="return validtaion();" CssClass="btn btnsave" 
                                                    Text="Save" Title="Save" />
                                            <asp:Button ID="btnScreenCancel" runat="server" CssClass="btn btncancel" Text="Cancel" Title="Cancel" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>

                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                        <center>
                            <asp:GridView ID="gvScreenFailureAuditTrail" Style="width: 500px !important; min-height: 50px; max-height: 100px !important; overflow: auto;" runat="server" AutoGenerateColumns="False" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="vMySubjectNo" HeaderText="SubjectNo" />
                                    <asp:BoundField DataField="vInitials" HeaderText="Initials" />
                                    <asp:BoundField DataField="cScreenFailure" HeaderText="Screen Failure" />
                                    <asp:BoundField DataField="cDisContinue" HeaderText="Dis Continue" />
                                    <asp:BoundField DataField="vScreenFailureRemaks" HeaderText="Remarks" />
                                    <asp:BoundField DataField="dScreenFailureDate" HeaderText="ScreenFailure Date" />
                                    <asp:BoundField DataField="vModifyBy" HeaderText="Modify By" />
                                    <asp:BoundField DataField="dModifyOn" HeaderText="ModifyOn" />

                                </Columns>
                            </asp:GridView>
                        </center>
                </div>
            </div>


            <cc1:ModalPopupExtender ID="ModalScreenFailure" runat="server" PopupControlID="divScreenFailuer"
                BackgroundCssClass="modalBackground" TargetControlID="btnModel" BehaviorID="ModalScreenFailure"
                CancelControlID="CancelRemarks">
            </cc1:ModalPopupExtender>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnScreenSave" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnScreenCancel" EventName="Click" />
            
              
            <asp:AsyncPostBackTrigger ControlID="ddlVisits" EventName="SelectedIndexChanged" />
             
        </Triggers>
    </asp:UpdatePanel>

    <div id="myModal" class="modal" runat="server">
        <!-- Modal content -->
        <div class="modal-content">
            <div class="modal-header" style="text-align: left">
                <img id="Img1" alt="Close1" src="images/close_pop.png" class="close modalCloseImage" title="Close" />
                <h3 style="text-align: center">
                    <asp:Label runat="server" ID="modalHeading"></asp:Label></h3>
            </div>
            <div class="modal-body">
                <table width="100%">
                    <tr>
                        <td align="left" style="font-weight: bold; font-size: small;" colspan="2">
                            <asp:Label runat="server" ID="lblHeading"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr id="trRandomizationRemarks">
                        <td align="left" style="font-weight: bold; font-size: small;">Remarks *
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtRandomizationRemarks" runat="server" TextMode="MultiLine" CssClass="textBox" Height="65px" Width="80%" />
                        </td>
                    </tr>
                    <tr id="trRandomizationGenerate" style="display: none">
                        <td align="center" style="font-weight: bold; font-size: small;" colspan="2">
                            <asp:Label runat="server" ID="lblRandomizationMsg">You Have Successfully Randomize this Subject. Your Randomization No is : - </asp:Label>
                            <asp:Label runat="server" ID="lblRandomizationno"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td style="text-align: center" colspan="2">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnSaveRandomizationNoSave" runat="server" CssClass="btn btnsave" Text="OK" Width="105px" />
                                    <input type="button" class="btn btnclose" title="Close" style="width: 105px" onclick="ModalPopupClose();" id="btnRandomizationCancel" value="Close" />
                                    <input type="button" class="btn btnclose" title="Close" style="width: 105px; display: none" onclick="CloseAllPopup();" id="btnRandomizationGenerate" value="Close" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSaveRandomizationNoSave" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>



    <div id="myModalSignAuth" class="modal" runat="server" display="none">
        <div class="modal-content">
            <div class="modal-header" style="text-align: left">
                <img id="Img5" alt="Close1" src="images/Sqclose.gif" class="close modalCloseImage" title="Close" runat="server" onclick="CloseAuthentication();" />
                <h3 style="text-align: center">
                    <asp:Label runat="server" ID="Label2">Signature Authentication</asp:Label>
                </h3>
                <table width="100%">
                    <tr>
                        <td align="right" style="width: 16%; font-weight: bold; font-size: smaller;">User Name :
                        </td>
                        <td align="left" style="width: 15%; font-size: small">
                            <asp:Label runat="server" ID="lblSignAuthUserName"></asp:Label>
                        </td>
                        <td align="right" style="width: 16%; font-weight: bold; font-size: 10px;">Date & Time :
                        </td>
                        <td align="left" style="width: 26%;">
                            <asp:Label runat="server" ID="lblSignAuthDateTime"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="modal-body">
                <table width="100%">
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td align="left" style="font-weight: bold; font-size: small;">Password :
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtPassword" runat="Server" Text="" CssClass="textbox" TextMode="Password" oncopy="return false" onpaste="return false"
                                oncut="return false" placeholder="Password" class="td-ie8" autocomplete="off"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="font-weight: bold; font-size: small;" colspan="2">I hereby confirm signing of this record electronically. 
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td style="text-align: center" colspan="2">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnSignAuthOK" runat="server" CssClass="btn btnsave" Text="OK" Width="105px" />
                                    <asp:Button ID="btnSignAuthCancel" runat="server" CssClass="btn btncancel" Text="Cancel" Width="105px" OnClientClick="return SignAuthModalClose();" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSignAuthOK" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSignAuthCancel" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

    <div id="KitAllocationModal" class="modal" runat="server">
        <!-- Modal content -->
        <div class="modal-content">
            <div class="modal-header" style="text-align: left">
                <img id="Img4" alt="Close1" src="images/close_pop.png" class="close modalCloseImage" title="Close" onclick="CloseAllKitPopup()" />
                <h3 style="text-align: center">
                    <asp:Label runat="server" ID="Label1">Product/Kit Allocation</asp:Label></h3>
            </div>
            <div class="modal-body">

                <table width="100%">
                    <tr id="trKitAllocationTabletKit">
                        <td align="center" style="font-weight: bold; font-size: small; width: 30%" colspan="2">
                            <input type="radio" name="KitRadio" value="Tablet" id="rdTablet" />
                            Product
                            <input type="radio" name="KitRadio" value="Kit" id="rdKit" />
                            Kit
                        </td>
                    </tr>

                    <tr></tr>
                    <tr></tr>

                    <tr id="trVisit" style="display: block">
                        <td align="right" style="font-weight: bold; font-size: small; width: 50%">Visit *
                        </td>
                        <td align="left" style="width: 50%">
                            <asp:DropDownList runat="server" ID="ddlVisit" class="dropDownList" AutoPostBack="false" Width="100%" Style="margin-left: 20px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr></tr>

                    <tr id="trLabelQty" style="display: none">
                        <td align="right" style="font-weight: bold; font-size: small; width: 50%">No of Qty *
                        </td>
                        <td align="left" style="width: 50%">
                            <input type="text" id="txtNoofQtylabel" class="textbox" />
                        </td>
                    </tr>
                    <tr></tr>

                    <tr id="trLabelDose" style="display: none">
                        <td align="right" style="font-weight: bold; font-size: small; width: 50%">Dose
                        </td>
                        <td align="left" style="width: 50%">
                            <input type="text" id="txtDoseRemark" class="textbox" />
                        </td>
                    </tr>
                    <tr></tr>

                    <tr></tr>
                    <tr id="trMessage" style="display: none">
                        <td align="left" style="width: 50%" colspan="2">
                            <h3>
                                <asp:Label runat="server" ID="lblKitMessage"></asp:Label></h3>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center" colspan="2">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnOK" runat="server" CssClass="btn btnsave" Text="OK" Width="105px" />
                                    <input type="button" class="btn btnclose" title="Close" style="width: 105px" id="btnCloseKit" value="Close" onclick="CloseAllKitPopup();" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSaveRandomizationNoSave" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

    <div id="KitAllocationSignAuth" class="modal" runat="server" display="none">
        <div class="modal-content">
            <div class="modal-header" style="text-align: left">
                <img id="Img6" alt="Close1" src="images/Sqclose.gif" class="close modalCloseImage" title="Close" runat="server" onclick="CloseAuthentication();" />
                <h3 style="text-align: center">
                    <asp:Label runat="server" ID="Label3">Signature Authentication</asp:Label>
                </h3>
                <table width="100%">
                    <tr>
                        <td align="right" style="width: 16%; font-weight: bold; font-size: smaller;">User Name :
                        </td>
                        <td align="left" style="width: 15%; font-size: small">
                            <asp:Label runat="server" ID="lbluserNameForKit"></asp:Label>
                        </td>
                        <td align="right" style="width: 16%; font-weight: bold; font-size: 10px;">Date & Time :
                        </td>
                        <td align="left" style="width: 26%;">
                            <asp:Label runat="server" ID="lblDateTimeForKit"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="modal-body">
                <table width="100%">
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td align="left" style="font-weight: bold; font-size: small;">Password :
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtPasswordForKit" runat="Server" Text="" CssClass="textbox" TextMode="Password" oncopy="return false" onpaste="return false"
                                oncut="return false" placeholder="Password" class="td-ie8" autocomplete="off"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="font-weight: bold; font-size: small;" colspan="2">I hereby confirm signing of this record electronically. 
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td style="text-align: center" colspan="2">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnOKSignAuth" runat="server" CssClass="btn btnsave" Text="OK" Width="105px" />
                                    <asp:Button ID="btnCloseSignAuth" runat="server" CssClass="btn btnClose" Text="Cancel" Width="105px" OnClientClick="return SignAuthModalCloseKitAllocation();" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnOKSignAuth" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCloseSignAuth" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

    <div id="ModalReverseRandomization" class="modal" runat="server" display="none">
        <div class="modal-content">
            <div class="modal-header" style="text-align: left">
                <img id="Img7" alt="Close1" src="images/Sqclose.gif" class="close modalCloseImage" title="Close" runat="server" onclick="SaveReverseRandomization();" />
                <h3 style="text-align: center">
                    <asp:Label runat="server" ID="Label4">Reverse Randomization</asp:Label>
                </h3>
            </div>
            <div class="modal-body">
                <table width="100%">
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td align="left" style="font-weight: bold; font-size: small;">Remarks :
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtReverseRandomizationRemarks" runat="server" TextMode="MultiLine" CssClass="textBox" Height="65px" Width="80%" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td style="text-align: center" colspan="2">
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnSaveReverseRandomization" runat="server" CssClass="btn btnsave" Text="OK" OnClientClick="return ValidationForReverseRandomization()" />
                                    <%--<asp:Button ID="btnCloseReverseRandomization" runat="server" CssClass="btn btncancel" Text="Cancel" Width="105px" />--%>
                                    <input type="button" id="btnCloseReverseRandomization" class="btn btnclose" value="Close" onclick="SaveReverseRandomization()" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSaveReverseRandomization" EventName="Click" />
                                    <%--<asp:AsyncPostBackTrigger ControlID="btnCloseReverseRandomization" EventName="Click" />--%>
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>



    <link href="App_Themes/StyleBlue/StyleBlue.css" rel="stylesheet" type="text/css" />
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <%--<script src="Script/DatatableScheduler.js" type="text/javascript"></script>--%>
    <script src="Script/jquery.dataTables.js" type="text/javascript"></script>
    <script src="Script/jquery.dataTables.plugins.js" type="text/javascript"></script>
    <script src="Script/TableTools.min.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">

        function fsetPatient_Show() {
            $('#<%=fsetPatient.ClientID%>').attr('style', $('#<%=fsetPatient.ClientID%>').attr('style') + ';display:block');
        }

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

        function AuditTrailSubjectWise() {
            document.getElementById('ctl00_CPHLAMBDA_divDeletedSubjectMst').style.zIndex = "0";
        }

        function DeletedSubjectAuditTrailShow() {
            document.getElementById('ctl00_CPHLAMBDA_divDeletedSubjectMst').style.zIndex = "100001";
        }

        function DeleteSubjectAuditTrail_Datatable() {
            oTab = $('#<%= GvwDeletedSubjectAuditTrail.ClientID%>').prepend($('<thead>').append($('#<%= GvwDeletedSubjectAuditTrail.ClientID%> tr:first'))).dataTable({
                "bJQueryUI": true,
                "bPaginate": true,
                "sPaginationType": "full_numbers",
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": false,
                aLengthMenu: [
                    [10, 15, 20, 50, -1],
                    [10, 15, 20, 50, "All"]
                ],
            });
            return false;
        }


        function pageLoad() {
            if ($get('<%= gvwSubjectSelectionForVisit.ClientID()%>') != null && $get('<%= gvwSubjectSelectionForVisit.ClientID%>_wrapper') == null) {
                if (jQuery('#<%= gvwSubjectSelectionForVisit.ClientID%>')) {
                    jQuery('#<%= gvwSubjectSelectionForVisit.ClientID%>').prepend($('<thead>').append($('#<%= gvwSubjectSelectionForVisit.ClientID%> tr:first'))).DataTable({
                        "bJQueryUI": true,
                        "sPaginationType": "full_numbers",
                        "scrollCollapse": true,
                        "pageLength": 5,
                        "bProcessing": true,
                        "bPaginate": true,
                        "bFooter": false,
                        "bHeader": false,
                        "AutoWidth": true,
                        "bSort": false,
                        "fixedHeader": true,
                        //"dom": 'T<"clear">lfrtip',
                        //"sDom": '<"H"frT>t<"F"i>',
                        "oLanguage": { "sSearch": "Search" },
                        "aLengthMenu": [[10, 20, 50, -1], [10, 20, 50, "All"]],
                        "iDisplayLength": 10,
                    });
                }
                $(".dataTables_wrapper").css("width", ($(window).width() * 0.85 | 0) + "px");
            }
        }
        //$(".dataTables_wrapper").css("width", ($(window).width() * 0.85 | 0) + "px");

        function DOBBlur() {
            if (!DateConvert($("[id$='_txtDOB']").val(), $("[id$='_txtDOB']"))) {
                $("[id$='_txtDOB']").val('');
                return false;
            }
            if ($("[id$='_txtDOB']").val() != "" && !CheckDateLessThenToday($("[id$='_txtDOB']").val())) {
                $("[id$='_txtDOB']").val('');
                $("[id$='_txtAGE']").val('');
                msgalert('Date of Birth should be less than Current Date !');
                $("[id$='_txtDOB']").focus();
                return false;
            }
            SetAge(document.getElementById('<%=txtAge.ClientID %>'));
            return true;
        }

        function Validation() {
            var dt = document.getElementById('<%= txtICFSignedDate.ClientId %>');
            
            if (document.getElementById('<%= txtInitial.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Initial !');
                document.getElementById('<%= txtInitial.ClientId %>').focus();
                return false;
            }
            else if (document.getElementById('<%= txtScreenNo.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Subject No !');
                document.getElementById('<%= txtScreenNo.ClientId %>').focus();
                return false;
            }
            else if (document.getElementById('<%= txtScreenNo.ClientId %>').value == 0) {
                msgalert('Screen No could not be zero !');
                document.getElementById('<%= txtScreenNo.ClientId %>').focus();
                return false;
            }

            var element = document.getElementById("ctl00_CPHLAMBDA_trRemarks");
            if (element.style.display == "") {
                if (document.getElementById('<%= txtRemarks.ClientId %>').value.toString().trim().length <= 0) {
                    msgalert('Please Enter Remarks !');
                    document.getElementById('<%= txtScreenNo.ClientId %>').focus();
                    return false;
                }
                
            }
            return true;

            if (document.getElementById('<%= txtICFSignedDate.ClientId %>').value.toString().trim() != '') {
                var dt = document.getElementById('<%= txtICFSignedDate.ClientId %>');
                if (!CheckDateLessThenToday(dt.value)) {
                    dt.value = "";
                    dt.focus();
                    msgalert('ICF Date should not be greater than Current Date !');
                    return false;
                }
                return true;
            }

           
        }

        function ValidationForEdit() {
            var dt = document.getElementById('<%= txtICFSignedDate.ClientId %>');
    
            if (document.getElementById('<%= txtInitial.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Initial !');
                document.getElementById('<%= txtInitial.ClientId %>').focus();
                return false;
            }
            else if (document.getElementById('<%= txtScreenNo.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Screen No !');
                document.getElementById('<%= txtScreenNo.ClientId %>').focus();
                return false;
            }
            else if (document.getElementById('<%= txtScreenNo.ClientId %>').value == 0) {
                msgalert('Screen No could not be zero !');
                document.getElementById('<%= txtScreenNo.ClientId %>').focus();
                return false;
            }
            else if (document.getElementById('<%= txtRemarks.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Remarks !');
                document.getElementById('<%= txtRemarks.ClientId %>').focus();
                return false;
            }
            else if (document.getElementById('<%= txtICFSignedDate.ClientId %>').value.toString().trim() != '') {
                var dt = document.getElementById('<%= txtICFSignedDate.ClientId %>');
                if (!CheckDateLessThenToday(dt.value)) {
                    dt.value = "";
                    dt.focus();
                    msgalert('ICF Date should not be greater than Current Date !');
                    return false;
                }
                return true;
            }
            return false;
        }

        function SubjectMstDivShowHide(Type) {
            if (document.getElementById('<%= HProjectId.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Project !');
                document.getElementById('<%= txtProject.ClientId %>').focus();
                document.getElementById('<%= txtProject.ClientId %>').value = '';
                return false;
            }
            else if (Type == 'S') {
                document.getElementById('<%= divSubjectMst.ClientId %>').style.display = 'block';
                SetCenter('<%= divSubjectMst.ClientId %>');
                document.getElementById('<%= txtFirstName.ClientId %>').focus();
                return false;
            }
            else if (Type == 'H') {
                document.getElementById('<%= divSubjectMst.ClientId %>').style.display = 'none';
                document.getElementById('<%= txtFirstName.ClientId %>').value = '';
                document.getElementById('<%= txtMiddleName.ClientId %>').value = '';
                document.getElementById('<%= txtLastName.ClientId %>').value = '';
                document.getElementById('<%= txtInitial.ClientId %>').value = '';
                document.getElementById('<%= txtICFSignedDate.ClientId %>').value = '';
                document.getElementById('<%= txtScreenNo.ClientId %>').value = '';
                document.getElementById('<%= trPatientRandomizationNo.ClientId %>').style.display = 'none';
                document.getElementById('<%= trRemarks.ClientId %>').style.display = 'none';
                return false;
            }
            else if (Type == 'SHOWAUDITTRAIL') {
                document.getElementById('<%= divAudittrail.ClientId %>').style.display = 'block';
                SetCenter('<%= divAudittrail.ClientId %>');
                return false;
            }
            else if (Type == 'HIDEAUDITTRAIL') {
                document.getElementById('<%= divAudittrail.ClientId %>').style.display = 'none';
                return false;
            }
            else if (Type == 'HIDEDEMOGRAPHIC') {
                document.getElementById('<%= divDemographicDetails.ClientId %>').style.display = 'none';
                return false;
            }
            else if (Type == 'SHOWDEMOGRAPHIC') {
                document.getElementById('<%= divDemographicDetails.ClientId %>').style.display = 'block';
                    return false;
                }
            return true;
        }

        function SetInitial() {
            var fname = document.getElementById('<%= txtFirstName.ClientId %>').value.toString().trim();
            var mname = document.getElementById('<%= txtMiddleName.ClientId %>').value.toString().trim();
            var lname = document.getElementById('<%= txtLastName.ClientId %>').value.toString().trim();
            var initial = new String();
            var Ffieldname = document.getElementById('<%= txtFirstName.ClientId %>');
            var Mfieldname = document.getElementById('<%= txtMiddleName.ClientId %>');
            var Lfieldname = document.getElementById('<%= txtLastName.ClientId %>');
            var result = false;

            if (fname == '')
                initial = '-';
            else
                initial = fname;
            checkVal(fname, Ffieldname, '13');

            if (mname == '')
                initial = initial + '-';
            else
                initial = initial + mname;
            checkVal(mname, Mfieldname, '13');

            if (lname == '')
                initial = initial + '-';
            else
                result = checkVal(lname, Lfieldname, '13');
            if (result == false)
                lname = '';
            initial = initial + lname;

            document.getElementById('<%= txtInitial.ClientId %>').value = initial;
        }

        function SubjectRejectOrDelete(Type) {
            if (Type == 'S') {
                document.getElementById('<%= divSubjectRejectorDelete.ClientId %>').style.display = 'block';
                SetCenter('<%= divSubjectRejectorDelete.ClientId %>');
                document.getElementById('<%= ddlReason.ClientId %>').focus();
                return false;
            }
            else if (Type == 'H') {
                document.getElementById('<%= divSubjectRejectorDelete.ClientId %>').style.display = 'none';
                    return false;
                }
            return true;
        }

        function ValidationForRejectOrDelete() {
            if (document.getElementById('<%= ddlReason.ClientId %>').selectedIndex == '0') {
                if (document.getElementById('<%=TxtReason.clientid %>').value == '') {
                    msgalert('Select Either Reason Or Give Remarks !');
                    document.getElementById('<%= ddlReason.ClientId %>').selectedIndex = 0;
                    document.getElementById('<%=TxtReason.clientid %>').value = '';
                    return false;
                }
                else {
                    document.getElementById('<%=TxtReason.clientid %>').innerText
                    document.getElementById('<%=HdReasonDesc.clientid %>').value = document.getElementById('<%=TxtReason.clientid %>').innerText;
                    return true;
                }
            }
            else if (document.getElementById('<%=TxtReason.clientid %>').value != '') {
                msgalert('Select only Any One. Either Select Reason Or Select Remarks !');
                document.getElementById('<%= ddlReason.ClientId %>').selectedIndex = 0;
                document.getElementById('<%=TxtReason.clientid %>').value = '';
                return false;
            }
            else {
                document.getElementById('<%=HdReasonDesc.clientid %>').value = document.getElementById('<%= ddlReason.ClientId %>').options[document.getElementById('<%= ddlReason.ClientId %>').selectedIndex].innerText;
                return true;
            }
        }
        function DateValidation(ParamDate, txtdate) {
            if (ParamDate.trim() == '') {
                msgalert('ICF date can not be left blank !');
                txtdate.focus();
                return false;
            }
            if (ParamDate.trim() != '') {
                var flg = false;
                flg = DateConvert(ParamDate, txtdate);
                if (flg == false) {
                    txtdate.value = "";
                    txtdate.focus();
                    return false;
                }
                if (flg == true && !CheckDateLessThenToday(txtdate.value)) {
                    txtdate.value = "";
                    msgalert('ICF date should be less than Current Date !');
                    txtdate.focus();
                    return false;
                }
            }
            return true;
        }
//        for validations on add demographic details
        function CheckHeight(txtHeight) {
            if (txtHeight.value.trim() == '') {
                msgalert('Please Enter Valid Height in Centimeters !');
                txtHeight.value = '';
                txtHeight.focus();
            }

            if (txtHeight.value.trim() != '') {
                var result = CheckDecimal(txtHeight.value);
                if (result == false) {
                    msgalert('Please Enter Valid Height in Centimeters !');
                    txtHeight.value = '';
                    txtHeight.focus();
                }
            }
        }


        function FillBMIValue(txtHeightID, txtWeightID, txtBMIID) {
            var txtHeight = document.getElementById(txtHeightID);
            var txtWeight = document.getElementById(txtWeightID);
            var txtBMI = document.getElementById(txtBMIID);

            if (txtHeight.value.trim() == '' || txtWeight.value.trim() == '') {
                if (txtHeight.value.trim() == '') {
                    msgalert('Please Enter Valid Height in Centimeters !');
                    txtHeight.focus();
                    return false;
                }
                else if (txtWeight.value.trim() == '') {
                    msgalert('Please Enter Valid Weight in Kilogram !');
                    txtWeight.focus();
                    return false;
                }
            }
            //Again validate Height TextBox
            var result = CheckDecimal(txtHeight.value);
            if (result == false) {
                msgalert('Please Enter Valid Height in Centimeters !');
                txtHeight.focus();
                return;
            }

            //Now Check Weight TextBox
            result = CheckDecimal(txtWeight.value);
            if (result == false) {
                msgalert('Please Enter Valid Weight in Kilogram !');
                txtWeight.value = '';
                txtWeight.focus();
                return;
            }

            var bmi = GetBMI(txtHeight.value, txtWeight.value);
            try {
                if ((bmi != null) && !isNaN(bmi)) {
                    bmi = parseFloat(bmi);
                    txtBMI.value = bmi;
                    txtBMI.disabled = true;
                    document.getElementById('<%=txtDOB.ClientID %>').focus();
                }
                else {
                    txtBMI.value = '0';
                }
            }
            catch (err) {
            }
            txtBMI.disabled = true;
        }

        function SetAge(txt) {
            var txtDOB = document.getElementById('<%=txtDOB.ClientID %>');
            if (txtDOB.value.length > 8) {
                var nowDate = new Date();
                dob = GetDateDifference(txtDOB.value, nowDate.format('dd-MMM-yyyy'));
                if ((dob.Days / 365) == 1) {
                    txt.value = (dob.Years + 1).toString();
                    txt.disabled = true;
                }
                else {
                    txt.value = dob.Years.toString();
                    txt.disabled = true;
                }
            }
        }

        function ClearAll() {
            document.getElementById('<%= txtWeight.ClientID %>').value = '';
            document.getElementById('<%= txtHeight.ClientID %>').value = '';
            document.getElementById('<%= txtBMI.ClientID %>').value = '';
            document.getElementById('<%= txtDOB.ClientID %>').value = '';
            document.getElementById('<%= txtAge.ClientID %>').value = '';

            var rbllst = document.getElementById('<%= rblSex.ClientId%>');

            var chks = rbllst.getElementsByTagName('input');

            for (i = 0; i < chks.length; i++) {
                chks[i].checked = false;
            }
        }

        function CheckSelectedIndex() {
            var ddlPeriod = document.getElementById('<%=ddlPeriod.ClientID %>');
        }

        function OnSelectedRadioButtonList() {
        }

        function chkSubmit() {
            var flg = true;
            $('input[type!="button"]', ctl00_CPHLAMBDA_divDemographicDetails).each(function () {
                if ((this.type == "text" && this.value == "") || (this.type == "radio" && this.checked == false && flg == false)) {
                    flg = false;
                    msgalert("Please Enter all mandatory fields !");
                    this.focus();
                    return false;
                }
                if (this.type == "radio" && this.checked == false) flg = false;
                else flg = true;
            });
            if (flg == true) return true;
            else return false;
        }

        function UIgvClient() {
            oTab = $('#<%= gvAudittrail.ClientID%>').prepend($('<thead>').append($('#<%= gvAudittrail.ClientID%> tr:first'))).dataTable({
                "bJQueryUI": true,
                "bPaginate": true,
                "sPaginationType": "full_numbers",
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": false,
                aLengthMenu: [
                    [10, 15, 20, 50, -1],
                    [10, 15, 20, 50, "All"]
                ],
            });
            return false;
        }

        function ScreenFailureAudit() {
            oTab = $('#<%= gvScreenFailureAuditTrail.ClientID%>').prepend($('<thead>').append($('#<%= gvScreenFailureAuditTrail.ClientID%> tr:first'))).dataTable({
                "iDisplayLength": 5,
                "bJQueryUI": true,
                //"sScrollY": '200px;',
                //"sScrollY": true,
                //"scrollCollapse": true,
                //"sScrollX": '400px;',
                //"scrollX": true,
                "bPaginate": true,
                "bFooter": false,
                "bHeader": false,
                "AutoWidth": true,
                "bSort": false,
                "fixedHeader": true,

                //"dom": 'T<"clear">lfrtip',
                //"sDom": '<"H"frT>t<"F"i>',
                "oLanguage": { "sSearch": "Search" },
                //"aLengthMenu": [[25, 50, 75, -1], [25, 50, 75, "All"]],
                //"iDisplayLength": 25,
                aLengthMenu: [
                    [5, 10, 25, 50, -1],
                    [5, 10, 25, 50, "All"]
                ],
            });
            return false;
        }

        function OpenModelPopup() {

        }

        function characterlimit(id) {


            var text = id.value


            var textLength = text.length;
            if (textLength > 200) {
                $(id).val(text.substring(0, (200)));
                msgalert("Only 200 characters are allowed !");
                return false;
            }
            else {
                return true;
            }

        }

        function validtaion() {
            var text = document.getElementById('<%= txtScreenFailureRemarks.ClientId %>').value
            var textDate = document.getElementById('<%= txtScreenFailureDate.ClientId %>').value
            if (textDate == '') {
                msgalert("Please Enter Date !");
                return false;
            }
            if (CheckDateForScreenFailure(textDate)) {
                msgalert("Screen Failure / Discountinue Date Should be Less then Current Date");
                return false;
            }
            if (text == '') {
                msgalert("Please Enter Remarks !");
                return false;
            }
            if (document.getElementById('<%= ddlVisits.ClientId%>').value.toUpperCase() == "SELECT VISIT") {
                msgalert('Please select visit !');
                document.getElementById('<%= ddlVisits.ClientId %>').focus();
                return false;
            }
        }

        function StudySetupCount() {
            var setWorkspaceId = document.getElementById('<%= HProjectId.ClientId %>').value.toString().trim();
            var result = true;
            var PostData = {
                vWorkSpaceID: setWorkspaceId,
            }
            var ApiUrl_PMS = '<%=ConfigurationManager.AppSettings("ApiUrl_PMS").ToString()%>';
            var BaseUrl = ApiUrl_PMS;
            if (setWorkspaceId != "") {
                $.ajax({
                    url: BaseUrl + "PmsRecordFetch/Proc_GetStudyProductDesignMst",
                    type: 'POST',
                    data: PostData,
                    async: false,
                    success: function (jsonData) {
                        if (jsonData.length == 0) {
                            result = false;
                        }
                    },
                });
            }
            return result;
        }

        function removeSpecialCharacter(PatientRandomizationNo) {
            PatientRandomizationNo.value = PatientRandomizationNo.value.replace(/[&\/\\\[\]#,+()$~%.'":*?<>{}]/g, '');
        }
        // Written By Arpit for RandomizationNO Auto and Manual Generate On 28/12/2016
        function RandomizationNoAutoManual() {
            var RandomizationType = document.getElementById('<%= hdnRandomizationType.ClientID%>').value
            var cScreenFailure = document.getElementById('<%= hdncScreenFailure.ClientID%>').value
            var cDisCountinue = document.getElementById('<%= hdncDisContinue.ClientID%>').value
            var RandomizationNo = document.getElementById('<%= txtPatientRandomizationNo.ClientID%>').value
            $("#ctl00_CPHLAMBDA_modalHeading").text('Randomization Subject');
            $("#ctl00_CPHLAMBDA_lblHeading").text("");
            $("#ctl00_CPHLAMBDA_txtRandomizationRemarks").val("")

            if (StudySetupCount() == false) {
                msgalert("First Define Study Setup in IMP Track, After You Can Only Generate Randomization Number !")
                return false;
            }

            if (cDisCountinue == "Y" || cScreenFailure == "Y") {
                document.getElementById('<%=myModal.ClientID()%>').style.display = 'inline';
                document.getElementById('<%=lblHeading.ClientID()%>').style.display = 'inline';
                $("#ctl00_CPHLAMBDA_lblHeading").text("You Can Not Randomize This Subject Because This Subject is Either Screen Failure or Discontinue");
                document.getElementById('<%=btnSaveRandomizationNoSave.Clientid()%>').style.display = 'none';
                document.getElementById('btnRandomizationCancel').style.display = 'inline';
                document.getElementById('btnRandomizationGenerate').style.display = 'none';
                $('#trRandomizationRemarks').hide();
                $('#trRandomizationGenerate').hide();
                $find("MpeSubjectMst").hide();
                return false;
            }

            if (RandomizationNo != "" && RandomizationType == "I") {
                $('#trRandomizationGenerate').hide();
                $('#trRandomizationRemarks').hide();
                $("#ctl00_CPHLAMBDA_modalHeading").text('Randomization Subject');
                $("#ctl00_CPHLAMBDA_lblHeading").text('You Aleardy Generate Randomization No So Can Not Proceed With This Option');
                document.getElementById('<%=btnSaveRandomizationNoSave.Clientid()%>').style.display = 'none';
                document.getElementById('btnRandomizationCancel').style.display = 'inline';
                document.getElementById('btnRandomizationGenerate').style.display = 'none';
                document.getElementById('<%=myModal.ClientID()%>').style.display = 'inline';
                $find("MpeSubjectMst").hide();
                $('#trRandomizationRemarks').hide();
                document.getElementById('<%=lblHeading.ClientID()%>').style.display = 'inline';
                return false;
            }

            if (RandomizationType == "M") {
                document.getElementById('<%=myModal.ClientID()%>').style.display = 'inline';
                document.getElementById('<%=lblHeading.ClientID()%>').style.display = 'inline';
                $("#ctl00_CPHLAMBDA_lblHeading").text("Randomization Type Define As Manual So You Can't Proceed With This Option");
                document.getElementById('<%=btnSaveRandomizationNoSave.Clientid()%>').style.display = 'none';
                document.getElementById('btnRandomizationCancel').style.display = 'inline';
                document.getElementById('btnRandomizationGenerate').style.display = 'none';

                $('#trRandomizationRemarks').hide();
                $('#trRandomizationGenerate').hide();
                $find("MpeSubjectMst").hide();
                return false;
            }

            if (RandomizationType == "I") {
                //var result = confirm("Are You Complete Inclusion / Exclusion?");
                msgConfirmDeleteAlert(null, "Are You Complete Inclusion / Exclusion?", function (isConfirmed) {
                    if (isConfirmed) {
                        var setWorkspaceId = document.getElementById('<%= HProjectId.ClientId %>').value.toString().trim();
                            var PostData = {
                                vWorkSpaceId: setWorkspaceId
                            }

                            var ApiUrl_PMS = '<%=ConfigurationManager.AppSettings("ApiUrl_PMS").ToString()%>';
                            var BaseUrl = ApiUrl_PMS;

                            if (setWorkspaceId != "") {
                                $.ajax({
                                    url: BaseUrl + "PmsRecordFetch/RandomizationDetailData",
                                    type: 'POST',
                                    data: PostData,
                                    async: false,
                                    success: function (data) {
                                        if (data.length == 0) {
                                            msgalert("You Can Not Generate Randomization Number Before File Upload !");
                                        }
                                        else {
                                            document.getElementById('<%=myModal.ClientID()%>').style.display = 'inline';
                                            $('#trRandomizationRemarks').show();
                                            $('#trRandomizationGenerate').hide();
                                            $find("MpeSubjectMst").hide();
                                            document.getElementById('<%=lblHeading.ClientID()%>').style.display = 'inline';
                                            $("#ctl00_CPHLAMBDA_lblHeading").text("Are You Sure Want To Randomize This Subject?");
                                            document.getElementById('btnRandomizationCancel').style.display = 'inline';
                                            document.getElementById('<%=btnSaveRandomizationNoSave.ClientID()%>').style.display = 'inline';
                                            document.getElementById('btnRandomizationGenerate').style.display = 'none';
                                        }
                                    },
                                });
                            }
                            return true;
                        }
                        else {
                            return false;
                        }
                    });
                    return false;
                }
            }

            function ModalPopupClose() {
                var modal = document.getElementById(('<%= myModal.ClientID%>'));
                modal.style.display = "none";
                $find("MpeSubjectMst").show();
                return fsetPatient_Show();
            }

            function RandomizationNumberGeneration(RandomizationNo) {
                fsetPatient_Show();
                $("#ctl00_CPHLAMBDA_lblHeading").text("");
                document.getElementById('<%=lblHeading.ClientID()%>').style.display = 'none';
                $('#trRandomizationGenerate').show();
                $('#trRandomizationRemarks').hide();
                if (RandomizationNo == "") {
                    $("#ctl00_CPHLAMBDA_modalHeading").text('Randomization Subject');
                    $("#ctl00_CPHLAMBDA_lblRandomizationno").text("Current Stock Is Zero");
                    $("#ctl00_CPHLAMBDA_lblRandomizationMsg").attr("style", "display:none")
                }
                else {
                    $("#ctl00_CPHLAMBDA_modalHeading").text('! Congratulation !');
                    $("#ctl00_CPHLAMBDA_lblRandomizationno").text(RandomizationNo);
                    $("#ctl00_CPHLAMBDA_lblRandomizationMsg").attr("style", "display:block")
                }

                $('#trRandomizationGenerate').show();
                document.getElementById('btnRandomizationCancel').style.display = 'none';
                document.getElementById('<%=btnSaveRandomizationNoSave.ClientID()%>').style.display = 'none';
                document.getElementById('btnRandomizationGenerate').style.display = 'inline';
                document.getElementById('<%=myModalSignAuth.ClientID()%>').style.display = 'none';
                document.getElementById('<%=myModal.ClientID()%>').style.display = 'inline';
            }

            function CloseAllPopup() {
                var modal = document.getElementById(('<%= myModal.ClientID%>'));
                modal.style.display = "none";
                $find("MpeSubjectMst").hide();
            }

            function validationforRandomizationNO(UserName) {
                fsetPatient_Show();
                if (document.getElementById('<%= txtRandomizationRemarks.ClientID%>').value.toString().trim().length <= 0) {
                    msgalert('Please Enter Remarks !');
                    document.getElementById('<%= txtRandomizationRemarks.ClientID%>').focus();
                    return false;
                }
                else {
                    document.getElementById('<%=myModalSignAuth.ClientID()%>').style.display = 'inline';
                    document.getElementById('<%=myModal.ClientID()%>').style.display = 'none';
                    $("#ctl00_CPHLAMBDA_lblSignAuthUserName").text(UserName);
                    document.getElementById("<%=txtPassword.ClientID()%>").value = '';

                    var today = new Date();
                    var date = today.getDate();
                    var minutes = today.getMinutes()
                    var month = today.getMonth() //January is 0!
                    var MonthList = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
                    if (date < 10) {
                        date = '0' + date;
                    }

                    if (minutes < 10) {
                        minutes = '0' + minutes;
                    }
                    var currentdate = date + '-' + MonthList[month] + '-' + today.getFullYear() + " " + today.getHours() + ":" + minutes;
                    $("#ctl00_CPHLAMBDA_lblSignAuthDateTime").text(currentdate);

                }
            }


            var KitWorkSpaceID = "";
            var kitRandomizationNo = "";
            var KitSubjectID = "";
            var vKitNo = "";
            var KitSubjectNo = "";
            var KitAllocationType = "";

    function GetVisit(vWorkSpaceID) {
        var ApiUrl_PMS = '<%=ConfigurationManager.AppSettings("ApiUrl_PMS").ToString()%>';
        var BaseUrl = ApiUrl_PMS;

        $.ajax({
            url: BaseUrl + "PmsRecordFetch/GetVisitMappingProjectWise/" + vWorkSpaceID + "",
            type: 'GET',
            success: SuccessMethod,
            async: false,
            datatype: JSON,
            contentType: "application/json; charset=utf-8",
            error: function () {
                msgalert("Visit Not Found !");
            }
        });
        function SuccessMethod(jsonData) {
            jsonData = jsonData.Table;
            if (jsonData.length > 0) {
                $("#ctl00_CPHLAMBDA_ddlVisit").empty().append('<option selected="selected" value="0">Please Select Visit</option>');
                for (var i = 0; i < jsonData.length; i++)
                    $("#ctl00_CPHLAMBDA_ddlVisit").append($("<option></option>").val(jsonData[i].vActivityName).html(jsonData[i].vActivityName));
            }
            else {
                $("#ctl00_CPHLAMBDA_ddlVisit").empty().append('<option selected="selected" value="0">Please Select Visit</option>');
            }

        }
    }

    $('input:radio[name=KitRadio]').change(function () {
        if (this.value == 'Tablet') {
            KitAllocationType = "Tablet"
            document.getElementById('trLabelQty').style.display = 'Block';
            document.getElementById('trLabelDose').style.display = 'Block';
        }
        else if (this.value == 'Kit') {
            KitAllocationType = "Kit"
            document.getElementById('trLabelQty').style.display = 'none';
            document.getElementById('trLabelDose').style.display = 'none';
        }
        $("#ctl00_CPHLAMBDA_ddlVisit").val("0");
        $("#txtNoofQtylabel").val("");
        $("#txtDoseRemark").val("");
    });


    function KitAllocation(e) {
        document.getElementById('trVisit').style.display = 'block';
        document.getElementById('trLabelQty').style.display = 'none';
        document.getElementById('trLabelDose').style.display = 'none';
        KitWorkSpaceID = e.attributes.vworkspaceid.value
        kitRandomizationNo = e.attributes.vrandomizationno.value
        KitSubjectID = e.attributes.vsubjectid.value
        KitSubjectNo = e.attributes.vsubjectno.value
        var cScreenFailure = e.attributes.cscreenfailure.value
        var cDisContinue = e.attributes.cdiscontinue.value

        if (cScreenFailure == "Y" || cDisContinue == "Y") {
            msgalert("Kit Can not Allocate Because Subject is Either ScreenFailure or Discontinue !")
            return false;
        }

        if (kitRandomizationNo == "") {
            msgalert("Randomization Number is Not Generated For Subject No " + KitSubjectNo + " !")
            return false;
        }

        document.getElementById('trMessage').style.display = 'none';
        document.getElementById('trKitAllocationTabletKit').style.display = 'Block';
        $("#rdTablet").removeAttr("checked")
        $("#rdKit").removeAttr("checked");
        $("#ctl00_CPHLAMBDA_lblKitMessage").text('');
        document.getElementById('<%=KitAllocationModal.ClientID()%>').style.display = 'inline';
            GetVisit(KitWorkSpaceID);
        }

        function SignatureAuthforKitAllocation(UserName) {
            if ($("#ctl00_CPHLAMBDA_ddlVisit").val() == 0) {
                msgalert("Please Select Visit !");
                return fsetPatient_Show();
                return false;
            }

            if (KitAllocationType == "Tablet") {
                if ($("#txtNoofQtylabel").val() == "") {
                    msgalert("Please Enter No of Label Quantity !");
                    return fsetPatient_Show();
                    return false;
                }

                if ($("#txtNoofQtylabel").val() == "0") {
                    msgalert("Please Enter Quantity More Than 0 !");
                    return fsetPatient_Show();
                    return false;
                }
            }



            if (KitAllocationType == "Kit" || KitAllocationType == "Tablet") {
                if (CheckKitAllocatedORNOT(KitAllocationType) == false) {
                    document.getElementById('trMessage').style.display = 'block';
                    document.getElementById('trVisit').style.display = 'none';
                    document.getElementById('trLabelQty').style.display = 'none';
                    document.getElementById('trLabelDose').style.display = 'none';
                    document.getElementById('<%=KitAllocationSignAuth.ClientID()%>').style.display = 'none';
                    document.getElementById('trKitAllocationTabletKit').style.display = 'none';

                    if (KitAllocationType == "Kit") {
                        $("#ctl00_CPHLAMBDA_lblKitMessage").text('Kit Number ' + vKitNo + ' Allocated To Subject ID ' + KitSubjectNo + '');
                    }
                    else if (KitAllocationType = "Tablet") {
                        $("#ctl00_CPHLAMBDA_lblKitMessage").text('Label Number ' + vKitNo + ' Allocated To Subject ID ' + KitSubjectNo + '');
                    }
                    document.getElementById('<%=btnOK.ClientID()%>').style.display = 'none';
                    return fsetPatient_Show();
                    return false;
                }
            }
            document.getElementById('<%=KitAllocationSignAuth.ClientID()%>').style.display = 'inline';
            document.getElementById('trMessage').style.display = 'none';
            document.getElementById('trVisit').style.display = 'block';
            document.getElementById("<%=txtPasswordForKit.ClientID()%>").innerHTML = '';
            document.getElementById("<%=lbluserNameForKit.ClientID()%>").innerHTML = UserName;
            document.getElementById('<%=btnOK.ClientID()%>').style.display = 'Inline';

            var today = new Date();
            var date = today.getDate();
            var minutes = today.getMinutes()
            var month = today.getMonth() //January is 0!
            var MonthList = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
            if (date < 10) {
                date = '0' + date;
            }

            if (minutes < 10) {
                minutes = '0' + minutes;
            }
            var currentdate = date + '-' + MonthList[month] + '-' + today.getFullYear() + " " + today.getHours() + ":" + minutes;
            document.getElementById("<%=lblDateTimeForKit.ClientID()%>").innerHTML = currentdate;
            return fsetPatient_Show();
        }

        function CheckKitAllocatedORNOT(cDispenseType) {
            if (cDispenseType == "Kit") {
                cDispenseType = "K";
            }
            else if (cDispenseType = "Tablet") {
                cDispenseType = "L";
            }

            var Result = true;
            var wStr = "";
            wStr = "vWorkspaceId = " + KitWorkSpaceID + " AND cStatusIndi <> 'D'  AND cDispenseType = '" + cDispenseType + "' AND vNodeDisplayName = '" + $("#ctl00_CPHLAMBDA_ddlVisit :selected").text() + "' "
            wStr += "and vMySubjectNo = '" + KitSubjectNo + "' and vRandomizationNo = '" + kitRandomizationNo + "' and isNULL(vKitNo , '') <> ''"

            var WhereData = {
                WhereCondition_1: wStr,
            }
            var ApiUrl_PMS = '<%=ConfigurationManager.AppSettings("ApiUrl_PMS").ToString()%>';
            var BaseUrl = ApiUrl_PMS;

            if (KitWorkSpaceID != "") {
                $.ajax({
                    url: BaseUrl + "PmsRecordFetch/View_StudyProductDispensingDtl",
                    type: 'POST',
                    data: WhereData,
                    async: false,
                    success: SuccessKitLabelData,
                });

                function SuccessKitLabelData(data) {
                    if (data.Table.length == 0) {
                        Result = true;
                    }
                    else {
                        vKitNo = "";
                        for (var i = 0; i < data.Table.length; i++) {
                            vKitNo += data.Table[i].vKitNo + ",";
                        }
                        Result = false;
                    }
                }
                return Result;
            }
        }

        function SignAuthModalCloseKitAllocation() {
            document.getElementById('<%=KitAllocationSignAuth.ClientID()%>').style.display = 'none';
            document.getElementById('<%=KitAllocationModal.ClientID()%>').style.display = 'inline';
            fsetPatient_Show();
        }

        function KitDataSave(ModifyBy) {
            var cDispenseType = "";
            if (KitAllocationType == "Kit") {
                cDispenseType = "K"

                KitAllocateData = [];
                KitAllocateData.push({
                    vWorkspaceId: KitWorkSpaceID,
                    vSubjectID: KitSubjectNo,
                    vRandomizationNo: kitRandomizationNo,
                    iPeriod: 1,
                    vKitNo: "",
                    iModifyBy: ModifyBy,
                    vVisit: $("#ctl00_CPHLAMBDA_ddlVisit :selected").text(),
                    cDispenseType: cDispenseType,
                });
            }
            else if (KitAllocationType == "Tablet") {
                cDispenseType = "L";

                if (LabelQuantityCheck(KitWorkSpaceID, kitRandomizationNo, $("#ctl00_CPHLAMBDA_ddlVisit :selected").text()) == false) {
                    fsetPatient_Show();
                    document.getElementById('<%=KitAllocationSignAuth.ClientID()%>').style.display = 'none';
                    return false;
                }

                var KitAllocateData = [];
                for (var i = 0; i < $("#txtNoofQtylabel").val() ; i++) {
                    var KitData = {
                        vWorkspaceId: KitWorkSpaceID,
                        vSubjectID: KitSubjectNo,
                        vRandomizationNo: kitRandomizationNo,
                        iPeriod: 1,
                        vKitNo: "",
                        iModifyBy: ModifyBy,
                        vVisit: $("#ctl00_CPHLAMBDA_ddlVisit :selected").text(),
                        cDispenseType: cDispenseType,
                        vRemark: $("#txtDoseRemark").val(),
                    }
                    KitAllocateData.push(KitData);
                }
            }

        var ApiUrl_PMS = '<%=ConfigurationManager.AppSettings("ApiUrl_PMS").ToString()%>';
            var BaseUrl = ApiUrl_PMS;

            if (KitWorkSpaceID != "") {
                $.ajax({
                    url: BaseUrl + "PmsRecordSave/Save_StudyProductKitDispensingDtl",
                    type: 'POST',
                    data: { '': KitAllocateData },
                    async: false,
                    success: SuccessTreatTypeMappingData,
                });

                function SuccessTreatTypeMappingData(data) {
                    var JsonData = data.Table;
                    if (JsonData.length != 0) {
                        if (JsonData[0].Column1 != undefined) {
                            document.getElementById('<%=KitAllocationSignAuth.ClientID()%>').style.display = 'none';
                            document.getElementById('<%=KitAllocationModal.ClientID()%>').style.display = 'block';
                            document.getElementById('trMessage').style.display = 'block';
                            document.getElementById('trVisit').style.display = 'none';
                            document.getElementById('trLabelQty').style.display = 'none';
                            document.getElementById('trLabelDose').style.display = 'none';
                            document.getElementById('trKitAllocationTabletKit').style.display = 'none';
                            document.getElementById('<%=KitAllocationSignAuth.ClientID()%>').style.display = 'none';
                            $("#ctl00_CPHLAMBDA_lblKitMessage").text(JsonData[0].Column1);
                            document.getElementById('<%=btnOK.ClientID()%>').style.display = 'none';
                        }
                        else {
                            document.getElementById('<%=KitAllocationSignAuth.ClientID()%>').style.display = 'none';
                            document.getElementById('<%=KitAllocationModal.ClientID()%>').style.display = 'block';
                            document.getElementById('trMessage').style.display = 'block';
                            document.getElementById('trKitAllocationTabletKit').style.display = 'none';
                            document.getElementById('trVisit').style.display = 'none';
                            document.getElementById('trLabelQty').style.display = 'none';
                            document.getElementById('trLabelDose').style.display = 'none';
                            document.getElementById('<%=KitAllocationSignAuth.ClientID()%>').style.display = 'none';

                            if (KitAllocationType == "Kit") {
                                $("#ctl00_CPHLAMBDA_lblKitMessage").text('Kit Number ' + JsonData[0].vKitNo + ' Allocated To Subject ID ' + KitSubjectNo + '');
                            }
                            else if (KitAllocationType = "Tablet") {
                                $("#ctl00_CPHLAMBDA_lblKitMessage").text('' + $("#txtNoofQtylabel").val() + 'Label Allocated To Subject ID ' + KitSubjectNo + '');
                            }
                            document.getElementById('<%=btnOK.ClientID()%>').style.display = 'none';
                        }
                    }
                }
                return fsetPatient_Show();
            }
        }

        function CloseAllKitPopup() {
            document.getElementById('<%=KitAllocationSignAuth.ClientID()%>').style.display = 'none';
            document.getElementById('<%=KitAllocationModal.ClientID()%>').style.display = 'none';
            return fsetPatient_Show();
        }

        function SignAuthModalClose() {
            document.getElementById('btnRandomizationGenerate').style.display = 'none';
            document.getElementById('<%=myModalSignAuth.ClientID()%>').style.display = 'none';
            document.getElementById('<%=myModal.ClientID()%>').style.display = 'inline';
        }

        // Ended By Arpit on 29/12/2016


        function ReverseRandomization(e) {
            var vRandomization = e.attributes.vrandomizationno.value
            var vWorkspaceSubjectId = e.attributes.vworkspacesubjectid.value
            var WorkSpaceID = e.attributes.vworkspaceid.value
            var vSubjectID = e.attributes.vsubjectid.value
            if (vRandomization == "") {
                msgalert("Without Generate Randomization Number, You Can't Reverse Randomize This Subject !")
                return false;
            }

            var setWorkspaceId = document.getElementById('<%= HProjectId.ClientId %>').value.toString().trim();
        var PostData = {
            WhereCondition_1: "vWorkspaceId = '" + setWorkspaceId + "' and ISNULL(vKitNo,'') <> '' and vRandomizationNo = '" + vRandomization + "'",
            columnName_1: "COUNT(vKitNo) as [TotalCount]",
        }

        var ApiUrl_PMS = '<%=ConfigurationManager.AppSettings("ApiUrl_PMS").ToString()%>';
            var BaseUrl = ApiUrl_PMS;
            if (setWorkspaceId != "") {
                $.ajax({
                    url: BaseUrl + "PmsRecordFetch/View_StudyProductDispensingDtl",
                    type: 'POST',
                    data: PostData,
                    async: false,
                    success: function (data) {
                        var jsondata = data.Table;

                        if (jsondata[0].TotalCount == 0) {
                            document.getElementById('<%=ModalReverseRandomization.ClientID()%>').style.display = 'Inline';
                        }
                        else {
                            document.getElementById('<%=ModalReverseRandomization.ClientID()%>').style.display = 'none';
                            msgalert("Label / Kit is Dispense To This Subject, So Reverse Randomization Number is Not Possible !")
                        }
                    },
                });

            }
        }

        function SaveReverseRandomization() {
            document.getElementById('<%=ModalReverseRandomization.ClientID()%>').style.display = 'none';
            document.getElementById('<%= txtReverseRandomizationRemarks.ClientID %>').value = '';
            fsetPatient_Show();
        }


        function CheckDateForScreenFailure(e) {
            var t = new Date;
            if (e.length < 6) return !1;
            if (3 != e.toString().split("-").length) return !1;
            var n = e.substring(e.lastIndexOf("-") + 1);
            2 == n.length && (n = parseInt(n) <= cCUTOFFYEAR ? "20" + n.toString() : "19" + n.toString());
            var i = e.substring(0, e.indexOf("-")),
                r = e.substring(e.indexOf("-") + 1, e.lastIndexOf("-"));
            r = ConvertMonthToInt(r), r = parseFloat(r), r -= 1;
            var o = new Date;
            return o.setFullYear(n, r, i), t = new Date, t >= o ? !1 : !0
        }

        function LabelQuantityCheck(WorkSpaceID, Randomizationno, VisitName) {
            var result = true;
            var KitData = {
                vWorkspaceId: WorkSpaceID,
                vRandomizationNo: Randomizationno,
                vVisit: VisitName,
            }

            var ApiUrl_PMS = '<%=ConfigurationManager.AppSettings("ApiUrl_PMS").ToString()%>';
            var BaseUrl = ApiUrl_PMS;

            if (WorkSpaceID != "") {
                $.ajax({
                    url: BaseUrl + "PmsRecordFetch/Proc_GetProductLabelStock",
                    type: 'POST',
                    data: KitData,
                    async: false,
                    success: function (data) {
                        if (data.length != 0) {
                            if (parseInt(data[0].TotalQty) < parseInt($("#txtNoofQtylabel").val())) {
                                $("#txtNoofQtylabel").val("");
                                msgalert("Current Stock Is " + data[0].TotalQty + " !");
                                result = false;
                            }
                        }
                    },
                });
            }
            return result;
        }

        function ValidationForReverseRandomization() {
            if (document.getElementById('<%= txtReverseRandomizationRemarks.ClientID%>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Remarks !');
                document.getElementById('<%= txtReverseRandomizationRemarks.ClientId %>').focus();
                return false;
            }
        }


    </script>

</asp:content>
