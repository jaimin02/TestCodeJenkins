<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmCTMDashboard, App_Web_pna05jsx" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <style type="text/css">
        th, td {
            white-space: nowrap;
        }

        div.dataTables_wrapper {
            width: 100%;
            margin: 0 auto;
        }
        /*.DTFC_LeftBodyWrapper { height:auto !important;}*/
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }
    </style>

    <style type="text/css">
        .HeaderNoWrap {
            white-space: nowrap !important;
        }
        /*#ctl00_CPHLAMBDA_ddlPeriods{
            display:inline-block !important;
        }*/
    </style>

    <table cellpadding="0" align="center" style="width: 100%; margin: auto">
        <asp:HiddenField ID="hdnSub" runat="server"></asp:HiddenField>
        <tr>
            <td>
                <asp:UpdatePanel ID="upcontrols" runat="server">
                    <ContentTemplate>
                        <table style="width: 99%; margin: auto;" cellpadding="3px">
                            <tr>
                                <td>
                                    <fieldset class="FieldSetBox" style="display: block; width: 98%; text-align: left; border: #aaaaaa 1px solid;">
                                        <legend class="LegendText" style="color: Black; font-size: 12px">
                                            <img id="img2" alt="Patient Details" src="images/panelcollapse.png"
                                                onclick="Display(this,'divPatientDetail');" runat="server" style="margin-right: 2px;" />Project Details</legend>
                                        <div id="divPatientDetail">
                                            <table width="100%">
                                                <tr>
                                                    <td class="Label" nowrap="nowrap" style="text-align: left; width:10%;">
                                                        Project Name/Request Id* :
                                                        </td>
                                                    <td class="Label" nowrap="nowrap" style="text-align: left; width:30%;">
                                    <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" MaxLength="50" Width="99%"></asp:TextBox>
                                                        <asp:CheckBox ID="chkAll" runat="server" Text="All" Style="display: none" />
                                                        <asp:Button Style="display: none" ID="btnSetProject" runat="server" Text=" Project"></asp:Button>
                                                        <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                                                        <asp:HiddenField ID="hfProjectTypeCode" runat="server"></asp:HiddenField>
                                                        <asp:HiddenField ID="hdnIsReview" runat="server" Value=""></asp:HiddenField>
                                                        <%--Added by ketan--%>
                                                        <asp:HiddenField ID="hdnSubjectID" runat="server" Value=""></asp:HiddenField>
                                                        <asp:HiddenField ID="HLocationCode" runat="server" Value=""></asp:HiddenField>
                                                        <asp:HiddenField ID="hdn_vWorkspaceSubjectId" runat="server" Value=""></asp:HiddenField>
                                                        <%--Ended by ketan--%>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                                            CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                            CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                                            OnClientShowing="ClientPopulated" ServiceMethod="GetMyProjectCompletionList"
                                                            ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True"
                                                            CompletionListElementID="pnlProjectList">
                                                        </cc1:AutoCompleteExtender>
                                                        <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" />
                                                        </td>
                                                    <td class="Label" nowrap="nowrap" style="text-align: left; width:10%;">
                                                        Subject Initial/SubjectNo/RandomizationNo :
                                    </td>
                                                    <td style="width:20%;">
                                     <asp:TextBox ID="txtSubject" runat="server" CssClass="textBox" Width="99%" MaxLength="50"></asp:TextBox>
                                                        <asp:Button Style="display: none" ID="btnSetSubject" runat="server" Text=" Subject"></asp:Button>
                                                        <asp:HiddenField ID="HSubject" runat="server"></asp:HiddenField>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtenderSubject" runat="server" BehaviorID="AutoCompleteExtenderSubject"
                                                            CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                            CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelectedSubject"
                                                            OnClientShowing="ClientPopulatedSubject" ServiceMethod="GetSubjectCompletionList_Assigned_NotRejected"
                                                            ServicePath="AutoComplete.asmx" TargetControlID="txtSubject" UseContextKey="True"
                                                            CompletionListElementID="pnlSubjectList">
                                                        </cc1:AutoCompleteExtender>
                                                        <asp:Panel ID="pnlSubjectList" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" />
                                                        </td>
                                                    <td id="lblPeriods" runat="server" class="Label" nowrap="nowrap" style="text-align: left; width:4%;">
                                                        <asp:Label runat="server" ID="lblPeriod" Text="Period :" /> 
                                                        </td>
                                                    <td id="ddlPeriod" runat="server" class="Label" nowrap="nowrap" style="text-align: left; width:5%;">
                                     <asp:DropDownList ID="ddlPeriods" CssClass="dropDownList" Width="91%" runat="server" AutoPostBack="True">
                                     </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                
                                                <tr>
                                                    <td colspan="6" style="text-align: center; padding-top:2%" colspan="2" class="Label">
                                                        <asp:Button ID="btnGo" runat="server" OnClientClick="return Validation();" CssClass="btn btngo"
                                                            Text="" ToolTip="Go" />
                                                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" Text="Cancel" ToolTip="Cancel" />
                                                        <asp:Button ID="btnExit" runat="server" ToolTip="Exit" CausesValidation="False" CssClass="btn btnclose"
                                                            OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this); " Text="Exit" />
                                                    </td>
                                                </tr>

                                                <tr colspan="6" id="trCRFVersion" runat="server" style="display: none; text-align: left">
                                                    <td class="Label" style="text-align: left; width: 100%" valign="top" colspan="2">
                                                        <span class="Label">CRFVersion :</span><asp:Label ID="lblCRFVersion" runat="server"
                                                            CssClass="Label" EnableViewState="false"></asp:Label>
                                                        <span class="Label" style="margin-left: 0.5%;">CRFDate :</span><asp:Label ID="lblCRFDate"
                                                            runat="server" CssClass="Label" EnableViewState="false"></asp:Label>
                                                        <span class="Label" style="margin-left: 0.5%;">CRFStatus :</span><asp:Image ID="imgCRFStatus"
                                                            runat="server" alt="CRFStatus" ImageUrl="~/images/Freeze.jpg" EnableViewState="false" />
                                                    </td>
                                                </tr>

                                                <tr runat="server" id="TrLegends" visible="false">
                                                    <td style="text-align: left; float: left; width: 0%;" id="TdRejectSubjects" runat="server">
                                                        <asp:Label runat="server" ID="LblLegends" Width="20" Height="10" BackColor="Red"
                                                            EnableViewState="false">  </asp:Label>
                                                        <b>Rejected Subjects</b>
                                                    </td>
                                                    <td colspan="5" style="text-align: right; padding-right: 1%; width: 0%; left: 634px;">
                                                        <a>
                                                            <img src="Images/VISITSCHEDULER.png" onclick="Redirect(this)" runat="server" title="Visit Scheduler" id="imgVisitScheduler" style="margin-left:80%;" /></a>
                                                        <a>
                                                            <img src="Images/ViewDocumnent.png" onclick="viewProjectDocument(this)" runat="server" title="View Document of Project" id="imgViewDocument" /></a>
                                                        <a>
                                                            <img src="~/images/icon_activity.png" onclick="Redirect(this)" runat="server" title="CRF Activity Status Report" visible="false" id="img7" alt="New" enableviewstate="false" /></a>
                                                        <a>
                                                            <img src="~/images/DCFReview.png" runat="server" onclick="Redirect(this)" title="Discrepancy Management Report" id="img8" visible="false" alt="New" enableviewstate="false" /></a>
                                                        Legends
                                    <img id="imgShow" runat="server" src="images/question.gif" enableviewstate="false"
                                        onmouseover="$('#ctl00_CPHLAMBDA_canal').toggle('medium');" />
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td colspan="2">
                                                        <fieldset style="display: none; width: 98%; font-size: 7pt; height: auto; text-align: left"
                                                            id="canal" runat="server" class="FieldSetBox">
                                                            <div>
                                                                <%--Add and commented by shivani for dynamic review--%>
                                                                <asp:PlaceHolder ID="PhlReviewer" runat="server" EnableViewState="true"></asp:PlaceHolder>
                                                                <%--<img src="images/new.png" runat="server" id="img1" alt="New" enableviewstate="false" />-Data
                                            Entry Pending,&nbsp;
                                            <img src="images/continue.png" runat="server" id="img2" alt="Continue" enableviewstate="false" />-Data
                                            Entry Continue,&nbsp;
                                            <img src="images/review.png" runat="server" id="img3" alt="Review" enableviewstate="false" />-Ready
                                            For Review,&nbsp;
                                            <img src="images/complete_yellow.png" runat="server" id="img4" alt="First Review Complete"
                                                enableviewstate="false" />-First Review Done,&nbsp;
                                            <img src="images/complete.png" runat="server" id="img5" alt="Second Review Complete"
                                                enableviewstate="false" />-Second Review Done,&nbsp;
                                            <img src="images/lock.png" runat="server" id="img6" alt="Lock" enableviewstate="false" />-Reviewed
                                            & Freeze--%>
                                                            </div>
                                                        </fieldset>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td colspan="3" id="Icons" runat="server" style="text-align: right; padding-right: 1%;">
                                                        <%-- <a><img src="~/images/icon_activity.png" onclick="Redirect(this)"  runat="server" title="CRF Activity Status Report"  visible="false"   id="img7" alt="New" enableviewstate="false" /></a>
                                            <a ><img src="~/images/DCFReview.png" runat="server" onclick="Redirect(this)" title="Discrepancy And Re-review Status Report"   id="img8" visible="false"   alt="New" enableviewstate="false" /></a> --%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>

                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click"></asp:AsyncPostBackTrigger>
                    </Triggers>

                </asp:UpdatePanel>

            </td>
        </tr>
    </table>

    <asp:UpdatePanel ID="upSubjectSelectionForVisit" runat="server" EnableViewState="true"
        RenderMode="Inline" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <asp:HiddenField ID="hfGo" runat="server" Value="0"></asp:HiddenField>
            <table style="width: 100%; margin: auto;">
                <tbody>
                    <tr>
                        <td>
                            <fieldset id="fsetPatient" runat="server" class="FieldSetBox" style="display: none; width: 96.5%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img3" alt="Patient Details" src="images/panelcollapse.png"
                                        onclick="Display(this,'divPatientData');" runat="server" style="margin-right: 2px;" />Project Data</legend>

                                <div id="divPatientData" style="height: auto">
                                    <table width="100%">
                                        <tr>
                                            <td align="center" style="padding-bottom: 2px;">
                                                <asp:PlaceHolder ID="phTopPager" runat="server"></asp:PlaceHolder>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <div style="width: 100%;">
                                                    <asp:Panel ID="pnlGrid" runat="server" Height="90%">

                                                        <div class="datatable_filedetail" style="width: 100%; overflow: auto;">
                                                            <asp:GridView ID="gvwSubjectSelectionForVisit" runat="server" AutoGenerateColumns="False">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Review All">
                                                                        <HeaderTemplate>
                                                                            Review All<input id="chkReviewAll" onclick="SelectAll(this, 'gvwSubjectSelectionForVisit')"
                                                                                title="Review All" type="checkbox" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkReview" runat="server" Text="Review" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <HeaderStyle Wrap="False" />
                                                            </asp:GridView>
                                                        </div>

                                                    </asp:Panel>
                                                </div>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td align="center" style="padding-top: 2px;">
                                                <asp:PlaceHolder ID="phBottomPager" runat="server"></asp:PlaceHolder>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
      <%--      <asp:AsyncPostBackTrigger ControlID="btnAuthenticate" EventName="Click" />--%>
            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table>
                <tbody>
                    <tr>
                        <td>
                            <div style="display: none; left: 391px; width: 480px; top: 528px; height: 200px; text-align: left"
                                id="divAuthentication" class="divModalPopup" runat="server">
                                <table style="width: 400px" align="center">
                                    <tbody>
                                        <tr align="center">
                                            <td class="Label" align="center" colspan="2">
                                                <strong style="white-space: nowrap">User Authentication</strong>
                                            </td>
                                        </tr>

                                        <tr runat="server" id="trName">
                                            <td style="white-space: nowrap" class="Label" align="left">Name :
                                            </td>
                                            <td class="Label" align="left">
                                                <asp:Label runat="server" ID="lblSignername" Text=""></asp:Label>
                                            </td>
                                        </tr>

                                        <tr runat="server" id="trDesignation">
                                            <td style="white-space: nowrap" class="Label" align="left">Designation :
                                            </td>
                                            <td class="Label" align="left">
                                                <asp:Label runat="server" ID="lblSignerDesignation" Text=""></asp:Label>
                                            </td>
                                        </tr>

                                        <tr runat="server" id="trRemarks">
                                            <td style="white-space: nowrap" class="Label" align="left">Remarks :
                                            </td>
                                            <td class="Label" align="left">
                                                <asp:Label runat="server" ID="lblSignRemarks" Text=""></asp:Label>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td style="white-space: nowrap" class="Label" align="left">Password :
                                            </td>
                                            <td class="Label" align="left">
                                                <asp:TextBox ID="txtPassword" runat="Server" Text="" CssClass="textbox password"
                                                    onkeydown="return CheckTheEnterKey(event);" TextMode="Password"> </asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="Label" align="left" colspan="2">
                                                <asp:Button ID="btnAuthenticate" runat="server" Text="Authenticate" CssClass="btn btnsave"
                                                    Width="100px" ToolTip="Authenticate" OnClientClick="return ValidationForAuthentication();" />
                                                <asp:Button ID="btnClose" runat="server" Text="Close" ToolTip="Close" CssClass="btn btnexit"
                                                    OnClientClick="return DivAuthenticationHideShow('H');" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <div style="display: none; left: 391px; width: 280px; top: 528px; height: 100px; text-align: center"
                                id="divDirectAuthentication" class="DIVSTYLE2" runat="server">
                                <table style="width: 100%; margin-top: 20px; margin-left: 25px;" align="center">
                                    <tbody>
                                        <tr id="Tr1" runat="server">
                                            <td class="Label" align="left">
                                                <asp:Label runat="server" ID="Label3" Text="Are you sure,you want to authenticate?"></asp:Label>
                                            </td>
                                        </tr>

                                        <tr style="height: 10px;">
                                            <td>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="Label" align="left" colspan="2">
                                                <asp:Button ID="btnDirectAuthenticate" runat="server" Text="Authenticate" CssClass="btn btnnew"
                                                    Width="100px" ToolTip="Authenticate" />
                                                <asp:Button ID="btnDirectClose" runat="server" Text="Close" ToolTip="Close" CssClass="btn btnexit"
                                                    OnClientClick="return DivAuthenticationHideShow('H');" />
                                                <asp:HiddenField ID="hdnDirectAuthentication" runat="server" Value="false" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:Button ID="btnViewDocument" runat="server" OnClientClick="return viewProjectDocument(this);" Style="display: none"></asp:Button>
    <cc1:ModalPopupExtender ID="mdViewDocument" runat="server" BackgroundCssClass="modalBackground" BehaviorID="mdViewDocument"
        CancelControlID="imgActivityAuditTrail" PopupControlID="dvdialog" TargetControlID="btnViewDocument">
    </cc1:ModalPopupExtender>

    <div id="dvdialog" style="display: none" class="centerModalPopup" style="display: none; width: 75%;">
        <table border="0" cellpadding="2" cellspacing="2" width="100%">
            <tr>
                <td id="Td2" class="LabelText" style="text-align: center !important; font-size: 15px !important; width: 97%;">Guideline Instructions</td>
                <td style="width: 3%">
                    <img id="imgActivityAuditTrail" alt="Close" src="images/close.gif" onmouseover="this.style.cursor='pointer';" />
                </td>
            </tr>

            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>

            <tr>
                <td colspan="2">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <iframe id="ifviewDocument" style="height: 391px; width: 100%;" runat="server"></iframe>
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

    <%--Added by ketan--%>
    <div>
        <table>
            <tr>
                <td>
                    <button id="btnMpeSubjectMst" runat="server" style="display: none;" />
                    <cc1:ModalPopupExtender ID="MpeSubjectMst" runat="server" PopupControlID="divSubjectMst" BehaviorID="MpeSubjectMst"
                        BackgroundCssClass="modalBackground" TargetControlID="btnMpeSubjectMst" CancelControlID="ImgSubjectMstClose">
                    </cc1:ModalPopupExtender>
                    <div style="display: none; left: 330px; width: 400px; top: 367px; text-align: center; border-radius: 15px; background-color: white"
                        id="divSubjectMst" class="DIVSTYLE2" runat="server">
                        <table width="100%">
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
                                    <asp:UpdatePanel ID="upSubjectMst" runat="server">
                                        <ContentTemplate>
                                            <asp:HiddenField ID="HReplaceImySubjectNo" runat="server"></asp:HiddenField>
                                            <table width="100%" cellpadding="3px">
                                                <tbody>
                                                    <tr>
                                                        <td class="Label" style="text-align: right;">First Name :
                                                        </td>
                                                        <td style="text-align: left;">
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
                                                    <tr id="Tr2" runat="server" visible="false">
                                                        <td class="Label" style="text-align: right;">Date Of ICF Signed* :
                                                        </td>
                                                        <td style="text-align: left;">
                                                            <asp:TextBox ID="txtICFSignedDate" runat="server">
                                                            </asp:TextBox>
                                                            <cc1:CalendarExtender ID="CalExtICFDate" runat="server" Format="dd-MMM-yyyy"
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

                                                    <tr runat="server" id="trPatientRandomizationNo">
                                                        <td class="Label" style="text-align: right;">Randomization No :
                                                        </td>
                                                        <td style="text-align: left;">
                                                            <asp:TextBox ID="txtPatientRandomizationNo" runat="server" Enabled="True" CssClass="textBox" />
                                                            <asp:HiddenField ID="hdnRandomizationType" runat="server"></asp:HiddenField>
                                                        </td>
                                                        <td style="text-align: left;">
                                                            <img id="ImgRandomizationNO" src="images/Randomization.png" style="position: relative; float: right; right: 5px; height: 30px; width: 30px"
                                                                title="Randomization" onclick="RandomizationNoAutoManual();" />
                                                        </td>
                                                    </tr>
                                                    <tr runat="server" id="tr3">
                                                        <td class="Label" style="text-align: right;">Remarks* :
                                                        </td>
                                                        <td style="text-align: left;">
                                                            <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" CssClass="textBox" Height="50px" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Label" colspan="2" style="text-align: center;">
                                                            <asp:Button ID="BtnSaveSubjectMst" runat="server" Text="Save" ToolTip="Save" CssClass="btn btnsave" OnClientClick="return ValidationForEdit();" />
                                                            <asp:Button ID="Btnhidden" runat="server" Text="Hidden" CssClass="btn btnnew" Style="display: none" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="gvwSubjectSelectionForVisit" />

                                            <asp:AsyncPostBackTrigger ControlID="BtnSaveSubjectMst" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <%--Eneded by ketan--%>

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
                        <td align="left" style="font-weight: bold; font-size: small; white-space: normal" colspan="2">
                            <asp:Label runat="server" ID="lblHeading"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td></td>
                    </tr>

                    <tr id="trRandomizationRemarks">
                        <td align="left" style="font-weight: bold; font-size: small;">Remarks
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtRandomizationRemarks" runat="server" TextMode="MultiLine" CssClass="textBox" Height="65px" Width="80%" />
                        </td>
                    </tr>

                    <tr id="trRandomizationGenerate" style="display: none">
                        <td align="center" style="font-weight: bold; font-size: small; white-space: normal" colspan="2">You Have Successfully Randomize this Subject. 
                            Your Randomization No is : - 
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
                                    <%--OnClientClick="return validationforRandomizationNO();"--%>
                                    <asp:Button ID="btnRandomizationCancel" runat="server" CssClass="btn btncancel" Text="Close" Width="105px" OnClientClick="return ModalPopupClose();" />
                                    <asp:Button ID="btnRandomizationGenerate" runat="server" CssClass="btn btnnew" Text="Close" Width="105px" OnClientClick="return CloseAllPopup();" style="display:none"/>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnRandomizationCancel" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSaveRandomizationNoSave" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

    <div id="myModalSignAuth" class="modal" runat="server">
        <!-- Modal content -->
        <div class="modal-content">
            <div class="modal-header" style="text-align: left">
                <img id="Img4" alt="Close1" src="images/close_pop.png" class="close modalCloseImage" title="Close" />
                <h3 style="text-align: center">
                    <asp:Label runat="server" ID="Label1">Signature Authentication</asp:Label></h3>
            </div>
            <div class="modal-body">
                <table width="100%">
                    <tr>
                        <td align="left" style="font-weight: bold; font-size: small;">
                            User Name
                        </td>
                        <td align="left">
                            <asp:Label runat="server" ID="lblSignAuthUserName"></asp:Label>
                        </td>
                    </tr>
                    <tr><td></td></tr>
                    <tr>
                        <td align="left" style="font-weight: bold; font-size: small;">
                            Date & Time
                        </td>
                        <td align="left">
                            <asp:Label runat="server" ID="lblSignAuthDateTime"></asp:Label>
                        </td>
                    </tr>
                    <tr><td></td></tr>
                    <tr>
                        <td align="left" style="font-weight: bold; font-size: small;">
                            Please Enter Password
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtPasswordESign" runat="Server" Text="" CssClass="textbox" TextMode="Password" oncopy="return false" onpaste="return false" 
                                oncut="return false" placeholder="Password" class="td-ie8" autocomplete="off"></asp:TextBox>
                        </td>
                    </tr>
                    <tr><td></td></tr>
                    <tr>
                        <td align="center" style="font-weight: bold; font-size: small;" colspan="2">
                            I hereby confirm signing of this record electronically. 
                        </td>
                    </tr>
                    <tr><td></td></tr><tr><td></td></tr><tr><td></td></tr>
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


    <asp:HiddenField ID="hfgateway" runat="server" />
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.min.js"></script>
    <script src="Script/jquery.min.js?t=633677264283125000" type="text/javascript"></script>
    <script src="Script/jquery-2.1.0.min.js" type="text/javascript"></script>
    <script src="Script/jquery-ui-1.10.4.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <script src="Script/jquery-1.11.dataTables.min.js" type="text/javascript"></script>
    <script src="Script/jquery.dataTables.fixedColumns.js" type="text/javascript"></script>
    <script src="Script/TableTools.min.js" type="text/javascript"></script>
    <script src="Script/Validation.js" language="javascript" type="text/javascript"></script>
    <%-- Added by Ketan--%>

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

        var getTextDataForGrid;

        function OpenWindow(Path) {
            var QueryString = Path.split('?')[1]
            var vWorkSpaceId = QueryString.split('&')[0].split("=")[1]
            var iNodeId = QueryString.split('&')[2].split("=")[1]
            var vSubjectId = QueryString.split('&')[4].split("=")[1]
            $.ajax({
                type: "post",
                url: "frmCTMDashboard.aspx/CheckValidationForDataEntry",
                data: '{"WorkSpaceId":"' + vWorkSpaceId + '","iNodeId":"' + iNodeId + '" , "vSubjectId":"' + vSubjectId + '" }',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                success: function (data) {
                    if (data.d == "Success") {
                        window.open(Path);
                    }
                    else {
                        msgalert('Please enter Actual date in Visit Scheduler !')
                    }

                },
                failure: function (data) {
                    msgalert(data.d);
                },
                error: function (data) {
                    msgalert(data.d);
                }
            });


            return false;
        }

        var DirectAuthentication = $('#<%= hdnDirectAuthentication.ClientID %>').val();

        $(document).ready(function () {
            $('#canal').css('display', 'none');

            $('.password').bind('paste', function (e) {
                e.preventDefault();
                if (e.originalEvent.clipboardData) {
                    msgalert("You can not paste in password field !");
                    $('.password').val("");
                }
            });
        })

        function setLocation() {
            var dv = document.getElementById('<%=canal.ClientID%>');
            var winScroll = BodyScrollHeight();
            var updateProgressDivBounds = Sys.UI.DomElement.getBounds(dv);
            var winBounds = GetWindowBounds();

            x += winScroll.xScr;
            y += winScroll.yScr;

            Sys.UI.DomElement.setLocation(dv, parseInt(x), parseInt(y));
        }
        function DivShowHide(Type) {
            if (Type == 'S') {
                document.getElementById('<%=canal.ClientID%>').style.display = '';
                SetCenter('<%=canal.ClientID%>');
                return false;
            }
            else if (Type == 'H') {
                document.getElementById('<%=canal.ClientID%>').style.display = 'none';
                    return false;
                }
            return true;
        }

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

            function Validation() {
                if (document.getElementById('<%= HProjectId.ClientId %>').value.toString().trim().length <= 0) {
                    msgalert('Please Enter Project !');
                return false;
            }
            return true;
        }

        function CheckTheEnterKey(e) {
            var unicode = e.charCode ? e.charCode : e.keyCode
            if (unicode == 13) {
                document.getElementById('<%= btnAuthenticate.ClientId %>').click();
                return false;
            }
            return true;
        }

        function RefreshPage() {

            var btn = document.getElementById('<%= btnGo.ClientId %>');
            btn.click();
        }

        function DivAuthenticationHideShow(Type) {
            DirectAuthentication = $('#ctl00_CPHLAMBDA_hdnDirectAuthentication').val();
            var div = document.getElementById('<%= divAuthentication.ClientId %>');
            var divDirect = document.getElementById('<%= divDirectAuthentication.ClientId %>');
            if (DirectAuthentication === "false") {
                if (div != null) {
                    if (Type == 'S') {
                        div.style.display = '';
                        SetCenter(document.getElementById('<%= divAuthentication.ClientId %>').id);
                        document.getElementById('<%= txtPassword.ClientId %>').value = '';
                        document.getElementById('<%= txtPassword.ClientId %>').focus();
                        return false;
                    }
                    else if (Type == 'H') {
                        div.style.display = 'none';
                        ClearCheckBoxes();
                        return false;
                    }
                }
            }
            else {
                if (divDirect != null) {
                    if (Type == 'S') {
                        divDirect.style.display = '';
                        SetCenter(document.getElementById('<%= divDirectAuthentication.ClientId %>').id);
                        return false;
                    }
                    else if (Type == 'H') {
                        divDirect.style.display = 'none';
                        ClearCheckBoxes();
                        return false;
                    }
                }
            }
            return false;
        }

        function ValidationForAuthentication() {
            if (document.getElementById('<%= txtPassword.ClientId %>').value.trim() == '') {
                document.getElementById('<%= txtPassword.ClientId %>').value = '';
                msgalert('Please Enter Password For Authentication !');
                document.getElementById('<%= txtPassword.ClientId %>').focus();
                return false;
            }
            return true;
        }

        var CheckBoxReviewAll;
        var GridView;

        function SelectAll(CheckBoxControl, Grid) {
            CheckBoxReviewAll = CheckBoxControl;
            GridView = Grid;

            if (CheckBoxControl.checked == true) {
                document.getElementById('<%=hdnSub.ClientID()%>').value = "1,2,3,4,5,6"
                var i;

                for (i = 0; i < $('#ctl00_CPHLAMBDA_gvwSubjectSelectionForVisit').find('input[type="checkbox"]').length; i++) {
                    var id = $('#ctl00_CPHLAMBDA_gvwSubjectSelectionForVisit').find('input[type="checkbox"]')[i].id
                    document.getElementById(id).checked = true;
                    document.getElementById(id).setAttribute("checked", "checked");
                }
                DivAuthenticationHideShow('S');
            }
            else {
                document.getElementById('<%=hdnSub.ClientID()%>').value = ""
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    //if (document.getElementById(id).checked == false) {
                    document.getElementById(id).checked = false;
                    document.getElementById(id).removeAttr('checked');

                    //}
                }
                DivAuthenticationHideShow('H');
            }
        }

        function CheckUncheckAll(id, Grid) {
            var chk
            if (id.checked == true) {
                id.checked = true
                $(id).prop('checked', true)
                chk = true

            }
            else {
                id.checked = false
                $(id).prop('checked', false);
                chk = false
            }

            var Checkall = document.getElementById('chkReviewAll');

            CheckBoxReviewAll = Checkall;
            GridView = Grid;
            document.getElementById('<%=hdnSub.ClientID()%>').value = ""
            var Gvd = document.getElementById('<%=gvwSubjectSelectionForVisit.ClientId %>');

            for (i = 0; i < $('#ctl00_CPHLAMBDA_gvwSubjectSelectionForVisit').find('input[type="checkbox"]').length; i++) {
                var idGrid = $('#ctl00_CPHLAMBDA_gvwSubjectSelectionForVisit').find('input[type="checkbox"]')[i].id
                if (idGrid == id.id) {
                    if (chk == true) {
                        $("#" + idGrid).prop("checked", true)
                    }
                    else {
                        $("#" + idGrid).prop("checked", false)
                    }
                }
                if ($("#" + idGrid).is(':checked') == false) {
                    Checkall.checked = false;
                    idGrid.checked = false
                    //break;
                }
                else if ($("#" + idGrid).is(':checked') == true) {
                    document.getElementById('<%=hdnSub.ClientID()%>').value += "," + (document.getElementById(idGrid).id).split('_')[3].substring(4, 5)
                        DivAuthenticationHideShow('S');
                    }
            }
        }

        function ClearCheckBoxes() {
            CheckBoxReviewAll.checked == false;
            var i;
            for (i = 0; i < document.forms[0].elements.length; i++) {
                if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf(GridView) > -1)) {
                    document.forms[0].elements[i].checked = false;
                }
            }
        }

        function Redirect(id) {
            debugger;
            var id = id.id
            //alert(id)
            var WorkSpace = document.getElementById('<%= HProjectId.ClientId %>').value.toString().trim()
            var SubjectId = document.getElementById('<%= HSubject.ClientID %>').value.toString().trim()
            var Period;
            if (document.getElementById('<%= ddlPeriods.ClientID %>') == null) {
                Period = 1;
            }
            else {
                Period = document.getElementById('<%= ddlPeriods.ClientID %>').value.toString().trim();
            }
           
            var ProjectName = document.getElementById('<%= txtproject.ClientID  %>').value.toString().trim()
            if (id == 'ctl00_CPHLAMBDA_img7') {
                window.open('frmCRFActivityStatusReport.aspx?WorkSpaceId=' + WorkSpace + '&SubjectId=' + SubjectId + '&Period=' + Period + '&ProjectName=' + ProjectName, '_blank');
            }
            else if (id == 'ctl00_CPHLAMBDA_imgVisitScheduler') {
                window.open('frmVisitScheduler.aspx?WorkSpaceId=' + WorkSpace + '&ProjectName=' + ProjectName, '_blank');
            }
            else {
                window.open('frmCTMDiscrepancyStatusReport.aspx?WorkSpaceId=' + WorkSpace + '&SubjectId=' + SubjectId + '&Period=' + Period + '&ProjectName=' + ProjectName, '_blank');
            }
        }
        function viewProjectDocument(e) {
            var WorkSpaceId = document.getElementById('<%= HProjectId.ClientId %>').value.toString().trim();
            var Period;
            if (document.getElementById('<%= ddlPeriods.ClientID %>') == null) {
                Period = 1;
            }
            else {
                Period = $get('ctl00_CPHLAMBDA_ddlPeriods').options[$get('ctl00_CPHLAMBDA_ddlPeriods').selectedIndex].text;
            }
            $.ajax({
                type: "post",
                url: "frmCTMDashboard.aspx/ViewDocument",
                data: '{"WorkSpaceId":"' + WorkSpaceId + '","Period":"' + Period + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                success: function (data) {
                    if (data.d == "") {
                        msgalert("No Uploaded Documents found for selected Project and Period !");
                    }
                    else {
                        var FileName = data.d;
                        $("#ctl00_CPHLAMBDA_ifviewDocument").attr("src", FileName);
                        $find('mdViewDocument').show();
                        return false;
                    }
                }
            });

        }
        //Add by shivani pandya for column sorting , header wraping , first time grid height
        function getDatatable() {

            $(".datatable_filedetail").css({ "max-width": $(window).width() * 0.95 });

            if ($get('<%= gvwSubjectSelectionForVisit.ClientID%>') != null) {
                if ($('#<%= gvwSubjectSelectionForVisit.ClientID%>' + ' tr').length > 0) {

                    oTab = $('#<%= gvwSubjectSelectionForVisit.ClientID%>').prepend($('<thead>').append($('#<%= gvwSubjectSelectionForVisit.ClientID%> tr:first'))).dataTable({
                        "sScrollY": (0.5 * $(window).height()),
                        "sScrollX": "100%",
                        scrollCollapse: true,
                        //"sPaginationType": "full_numbers",
                        "bJQueryUI": true,
                        "bLengthChange": false,
                        "bSort": true,
                        "bPaginate": true,
                        "bDestory": true,
                        "bRetrieve": true,
                        "bStateSave": false,
                        "aaSorting": [],
                        "bFilter": false,
                        "sDom": '<"H"flr>t<"F"p>'
                    });
                    $(".DTFC_LeftHeadWrapper table th").css({ 'color': 'black' });

                    if ($('div[class="DTFC_ScrollWrapper"]', $get('<%= gvwSubjectSelectionForVisit.ClientID%>' + '_wrapper')).length != 1) {

                        if (document.getElementById('<%= hfProjectTypeCode.ClientID%>').value == '0014') {
                            if (document.getElementById('<%= hdnIsReview.ClientID%>').value == 'true') {
                                new $.fn.dataTable.FixedColumns(oTab, { leftColumns: 5 });
                            }
                            else {
                                new $.fn.dataTable.FixedColumns(oTab, { leftColumns: 4 });
                            }
                        }
                        else {
                            if (document.getElementById('<%= hdnIsReview.ClientID%>').value == 'true') {
                                new $.fn.dataTable.FixedColumns(oTab, { leftColumns: 4 });
                            }
                            else {
                                new $.fn.dataTable.FixedColumns(oTab, { leftColumns: 3 });
                            }
                        }
                    }

                    $(".dataTables_scrollHeadInner table tr th div").each(function (iDataIndex) {
                        $(this).attr("oldname", $(this).text());
                        $(this).text($(this).text().split(".")[0]);
                        $(this).append("<span class='DataTables_sort_icon css_right ui-icon ui-icon-carat-2-n-s'></span>");
                        $(this).attr("newname", $(this).text().split(".")[0]);
                    });

                    $(".dataTables_scrollHeadInner table tr th div").mouseenter(function (iDataIndex) {
                        $(this).text($(this).attr("oldname"));
                        $(this).append("<span class='DataTables_sort_icon css_right ui-icon ui-icon-carat-2-n-s'></span>");
                    });

                    $(".dataTables_scrollHeadInner table tr th div").mouseleave(function (iDataIndex) {
                        $(this).text($(this).attr("newname"));
                        $(this).append("<span class='DataTables_sort_icon css_right ui-icon ui-icon-carat-2-n-s'></span>");
                    });
                }
            }
        }

        //Added by ketan for fill textbox (fnam,lnam,mname etc....)
        function ShowConfirmPopup(subjectId) {
            getDatatable();
            var TotalActivity;
            var WorkspaceID = document.getElementById('<%= HProjectId.ClientID%>').value;
            document.getElementById('<%= hdnSubjectID.ClientID%>').value = subjectId;

            $.ajax({
                type: "post",
                url: "frmCTMDashboard.aspx/FillSubjectDetails",
                data: '{"WorkspaceID":"' + WorkspaceID + '", "vSubjectId":"' + subjectId + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                dataType: "json",
                success: function (data) {
                    $('#ctl00_CPHLAMBDA_gvRandomizationHdr').attr("IsTable", "has");
                    var ActivityDataset = [];
                    if (data.d != "" && data.d != null) {
                        data = JSON.parse(data.d)
                        TotalActivity = data.length;
                        for (var Row = 0; Row < data.length; Row++) {
                            debugger;
                            document.getElementById('<%= txtFirstName.ClientID%>').value = data[Row].vFirstName;
                            document.getElementById('<%= txtMiddleName.ClientID%>').value = data[Row].vMiddleName;
                            document.getElementById('<%= txtLastName.ClientID%>').value = data[Row].vSurName;
                            document.getElementById('<%= txtInitial.ClientID%>').value = data[Row].vInitials;
                            document.getElementById('<%= txtScreenNo.ClientID%>').value = data[Row].vMySubjectNo;
                            document.getElementById('<%= txtPatientRandomizationNo.ClientID%>').value = data[Row].vRandomizationNo;
                            document.getElementById('<%= hdn_vWorkspaceSubjectId.ClientID%>').value = data[Row].vWorkspaceSubjectId;
                            document.getElementById('<%= HReplaceImySubjectNo.ClientID%>').value = data[Row].iMySubjectNo;
                            document.getElementById('<%= txtRemarks.ClientID%>').value = "";
                            document.getElementById('<%= hdnRandomizationType.ClientID()%>').value = data[Row].cRandomizationType;

                            if (data[Row].cRandomizationType == "I") {
                                document.getElementById('<%= txtPatientRandomizationNo.ClientID()%>').disabled = true;
                            }
                            else if (data[Row].cRandomizationType == "M") {
                                document.getElementById('<%= txtPatientRandomizationNo.ClientID()%>').disabled = false;
                            }


                    }
                    if (TotalActivity > 0) {
                        $find('MpeSubjectMst').show();
                    }
                }
                else {
                }
                    return false;
                },
                failure: function (response) {
                    msgalert(response.d);
                },
                error: function (response) {
                    msgalert(response.d);
                }
            });
        return false;

    }

    function ValidationForEdit() {
        var dt = document.getElementById('<%= txtICFSignedDate.ClientId %>');
            if (isBlank(document.getElementById('<%= txtInitial.ClientId %>').value.toString().trim())) {
                msgalert('Please Enter Initial !');
                document.getElementById('<%= txtInitial.ClientId %>').focus();
                return false;
            }
            else if (isBlank(document.getElementById('<%= txtScreenNo.ClientId %>').value.toString().trim())) {
                msgalert('Please Enter Screen No !');
                document.getElementById('<%= txtScreenNo.ClientId %>').focus();
                return false;
            }

            else if (document.getElementById('<%= txtScreenNo.ClientId %>').value == 0) {
                msgalert('Screen No could not be zero !');
                document.getElementById('<%= txtScreenNo.ClientId %>').focus();
                return false;
            }

            else if (isBlank(document.getElementById('<%= txtRemarks.ClientId %>').value.toString().trim())) {
                msgalert('Please Enter Remarks !');
                document.getElementById('<%= txtRemarks.ClientId %>').focus();
                return false;
            }
    return true;
}

function SubjectMstDivShowHide(Type) {

    if (Type == 'H') {
        debugger;
        document.getElementById('<%= divSubjectMst.ClientId %>').style.display = 'none';
        document.getElementById('<%= txtFirstName.ClientId %>').value = '';
        document.getElementById('<%= txtMiddleName.ClientId %>').value = '';
        document.getElementById('<%= txtLastName.ClientId %>').value = '';
        document.getElementById('<%= txtInitial.ClientId %>').value = '';
        document.getElementById('<%= txtScreenNo.ClientId %>').value = '';
        document.getElementById('<%= txtRemarks.ClientID%>').value = "";
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

    function ShowConfirmation() {

        var Istrue = confirm("Subject With Entered Initials Is Already Enrolled In Project. Do You Still Want To Continue ?");

        if (Istrue == true) {
            document.getElementById('<%=Btnhidden.Clientid %>').click();
        document.getElementById('<%=BtnSaveSubjectMst.Clientid %>').style.display = 'none';
        return true;
    }

    return false;
}

}

//Ended by ketan

// Written By Arpit for RandomizationNO Auto and Manual Generate On 28/12/2016
function RandomizationNoAutoManual() {
    var RandomizationType = document.getElementById('<%= hdnRandomizationType.ClientID%>').value
    var RandomizationNo = document.getElementById('<%= txtPatientRandomizationNo.ClientID%>').value
    $("#ctl00_CPHLAMBDA_modalHeading").text('Randomization Subject');
    $("#ctl00_CPHLAMBDA_txtRandomizationRemarks").val("")

    if (RandomizationNo != "" && RandomizationType == "I") {
        $('#trRandomizationGenerate').hide();
        $('#trRandomizationRemarks').hide();
        $("#ctl00_CPHLAMBDA_modalHeading").text('Randomization Subject');
        $("#ctl00_CPHLAMBDA_lblHeading").text('You Aleardy Generate Randomization Number,So Can Not Proceed With This Option');
        document.getElementById('<%=btnSaveRandomizationNoSave.Clientid()%>').style.display = 'none';
            document.getElementById('<%=btnRandomizationCancel.ClientID()%>').style.display = 'inline';
            document.getElementById('<%=btnRandomizationGenerate.ClientID()%>').style.display = 'none';
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
            document.getElementById('<%=btnRandomizationCancel.ClientID()%>').style.display = 'inline';
            document.getElementById('<%=btnRandomizationGenerate.ClientID()%>').style.display = 'none';
            $('#trRandomizationRemarks').hide();
            $('#trRandomizationGenerate').hide();
            $find("MpeSubjectMst").hide();
            return false;
        }

    if (RandomizationType == "I") {
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
                    data: WhereData,
                    async: false,
                    success: SuccessTreatTypeMappingData,
                });

                function SuccessTreatTypeMappingData(data) {
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
                        document.getElementById('<%=btnRandomizationCancel.ClientID()%>').style.display = 'inline';
                        document.getElementById('<%=btnSaveRandomizationNoSave.ClientID()%>').style.display = 'inline';
                        document.getElementById('<%=btnRandomizationGenerate.ClientID()%>').style.display = 'none';
                    }
                }
            }
        }
    }

    function ModalPopupClose() {
        var modal = document.getElementById(('<%= myModal.ClientID%>'));
        modal.style.display = "none";
        $find("MpeSubjectMst").show();

        if (document.getElementById('<%=hdnRandomizationType.ClientID()%>').value == "I") {
            document.getElementById('<%=txtPatientRandomizationNo.ClientID()%>').disabled = true;
        }
        else if (document.getElementById('<%=hdnRandomizationType.ClientID()%>').value == "M") {
            document.getElementById('<%=txtPatientRandomizationNo.ClientID()%>').disabled = false;
        }

}

function RandomizationNumberGeneration(RandomizationNo) {
    $("#ctl00_CPHLAMBDA_lblHeading").text("");
    document.getElementById('<%=lblHeading.ClientID()%>').style.display = 'none';
        $('#trRandomizationGenerate').show();
        $('#trRandomizationRemarks').hide();
        $("#ctl00_CPHLAMBDA_modalHeading").text('! Congratulation !');
        $('#trRandomizationGenerate').show();
        $("#ctl00_CPHLAMBDA_lblRandomizationno").text(RandomizationNo);
        document.getElementById('<%=btnRandomizationCancel.ClientID()%>').style.display = 'none';
        document.getElementById('<%=btnSaveRandomizationNoSave.ClientID()%>').style.display = 'none';
        document.getElementById('<%=btnRandomizationGenerate.ClientID()%>').style.display = 'inline';
        document.getElementById('<%=myModalSignAuth.ClientID()%>').style.display = 'none';
        document.getElementById('<%=myModal.ClientID()%>').style.display = 'inline';
    }

    function CloseAllPopup() {
        var modal = document.getElementById(('<%= myModal.ClientID%>'));
        modal.style.display = "none";
        $find("MpeSubjectMst").hide();
    }

    function validationforRandomizationNO(UserName) {
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


function SignAuthModalClose() {
    document.getElementById('<%=btnRandomizationGenerate.ClientID()%>').style.display = 'none';
        document.getElementById('<%=myModalSignAuth.ClientID()%>').style.display = 'none';
        document.getElementById('<%=myModal.ClientID()%>').style.display = 'inline';

    }
    // Ended By Arpit on 29/12/2016

        function OpenForAuthentication()
        {

            DivAuthenticationHideShow('S');
            msgalert("Password Authentication Fails. ");
            return false;

        }

    </script>

</asp:Content>
