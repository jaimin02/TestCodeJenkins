<%@ page language="VB" masterpagefile="~/ECTDMasterPage.master" autoeventwireup="false" inherits="frmMedExWorkspaceDtl, App_Web_mlepfeoz" enableeventvalidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <link rel="stylesheet" type="text/css" href="App_Themes/smoothnessjquery-ui.css" />



    <script src="Script/jquery-1.10.13.js" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery-ui.js"></script>

    <script src="Script/jquery.dataTables.js" type="text/javascript"></script>
    <script type="text/javascript" src="Script/slimScroll.min.js"></script>
    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <script type="text/javascript" src="Script/General.js"></script>
    <script type="text/javascript" src="Script/Validation.js"></script>
    <script src="Script/googleapis.js" type="text/javascript"></script>


    <style type="text/css">
        #ctl00_CPHLAMBDA_gvmedexworkspadce tr td {
            font-variant: normal !important;
            font-weight: normal !important;
        }

        #ctl00_CPHLAMBDA_Gv_GeneralScr tr td {
            font-variant: normal !important;
            font-weight: normal !important;
        }

        #ctl00_CPHLAMBDA_GVProjectSpcScreening tr td {
            font-variant: normal !important;
            font-weight: normal !important;
        }

        table.dataTable tr.even td.sorting_1 {
            background-color: White !important;
        }

        table.dataTable tr.odd td.sorting_1 {
            background-color: #CEE3ED !important;
        }

        #SeqMedex {
            list-style-type: none;
            margin: 0;
            padding: 0;
            width: 60%;
        }
        /*#SeqMedex li
        {
            margin: 0 3px 3px 3px;
            padding: 0.4em;
            padding-left: 1.5em;
            font-size: 1.4em;
            height: 18px;
        }
        #SeqMedex li span
        {
            position: absolute;
            margin-left: -1.3em;
        }*/ /*.ui-state-default
        {
            
        }*/

        #SeqMedex {
            /*width: 760px;*/
            margin-bottom: 20px;
            overflow: hidden;
        }

        .allmed {
            line-height: 1.5em;
            border-bottom: 1px solid #ccc;
            float: left;
            display: inline;
            border: 1px solid #d3d3d3;
            background: -moz-linear-gradient(top, rgba(247,247,247,0.73) 0%, rgba(206,227,237,1) 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,rgba(247,247,247,0.73)), color-stop(100%,rgba(206,227,237,1))); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top, rgba(247,247,247,0.73) 0%,rgba(206,227,237,1) 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top, rgba(247,247,247,0.73) 0%,rgba(206,227,237,1) 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top, rgba(247,247,247,0.73) 0%,rgba(206,227,237,1) 100%); /* IE10+ */
            background: linear-gradient(to bottom, rgba(247,247,247,0.73) 0%,rgba(206,227,237,1) 100%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr= '#baf7f7f7', endColorstr= '#cee3ed',GradientType=0 ); /* IE6-9 */
            font-weight: normal;
            color: #555555;
        }

            .allmed:hover {
                background: rgb(30,87,153); /* Old browsers */
                background: -moz-linear-gradient(top, rgba(30,87,153,1) 0%, rgba(41,137,216,1) 50%, rgba(32,124,202,1) 100%, rgba(125,185,232,1) 100%); /* FF3.6+ */
                background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,rgba(30,87,153,1)), color-stop(50%,rgba(41,137,216,1)), color-stop(100%,rgba(32,124,202,1)), color-stop(100%,rgba(125,185,232,1))); /* Chrome,Safari4+ */
                background: -webkit-linear-gradient(top, rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 100%,rgba(125,185,232,1) 100%); /* Chrome10+,Safari5.1+ */
                background: -o-linear-gradient(top, rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 100%,rgba(125,185,232,1) 100%); /* Opera 11.10+ */
                background: -ms-linear-gradient(top, rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 100%,rgba(125,185,232,1) 100%); /* IE10+ */
                background: linear-gradient(to bottom, rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 100%,rgba(125,185,232,1) 100%); /* W3C */
                filter: progid:DXImageTransform.Microsoft.gradient( startColorstr= '#1e5799', endColorstr= '#7db9e8',GradientType=0 ); /* IE6-9 */
                color: White !important;
            }

        .six li {
            width: 16.666%;
        }

        #ctl00_CPHLAMBDA_PanelMedex {
            height: auto !important;
        }

        #ctl00_CPHLAMBDA_tr_AttributeGroup1 {
            margin-left: 18%;
            font-weight: bold;
        }

        #ctl00_CPHLAMBDA_tr_Attribute1 {
            margin-left: 15%;
            font-weight: bold;
        }

        .DecimalNo {
            display: none;
        }
    </style>

    <asp:UpdatePanel runat="server" ID="upModalRemarks" UpdateMode="Conditional">
        <ContentTemplate>

            <div id="ModalBackGround" runat="server" class="divModalBackGround" style="display: none; width: 100% !Important;"></div>
            <button id="btnRemarls" runat="server" style="display: none;" />
            <div id="divRemarks" runat="server" class="centerModalPopup" style="display: none; left: 30%; width: 28%; position: absolute; top: 525px; border: 1px solid; height: 200px;">
                <div style="background-color: #1560A1;">
                    <table style="width: 90%; margin: auto;">
                        <tr>
                            <td colspan="2" class="LabelText" style="text-align: center !important; color: white; font-size: 14px !important; width: 97%;"><b>Enter Remarks</b>

                            </td>

                            <td style="text-align: center; height: 22px;" valign="top">
                                <img id="CancelRemarks" alt="Close" src="images/Close.gif" style="position: relative; float: right; right: 5px; cursor: pointer;"
                                    title="Close" />
                            </td>
                        </tr>
                    </table>
                    <hr />
                </div>
                <table style="margin: 10px 10px 10px 10px;">

                    <tr style="margin: 10px 10px 10px 10px;">

                        <td style="white-space: nowrap; text-align: right; width: 120px;" class="Label">Version No *:
                        </td>
                        <td class="Label" align="right">
                            <asp:TextBox ID="txtVersion" ReadOnly="true" runat="Server" CssClass="textbox"> </asp:TextBox>
                            <asp:HiddenField ID="hdnVersion" runat="Server"></asp:HiddenField>
                        </td>
                    </tr>
                    <tr style="margin: 10px 10px 10px 10px;">

                        <td style="white-space: nowrap; text-align: right; width: 120px;" class="Label">Remark *:
                        </td>
                        <td class="Label" align="right">
                            <asp:TextBox ID="txtRemarks" runat="Server" CssClass="textbox"> </asp:TextBox>
                        </td>
                    </tr>

                    <tr style="margin: 10px 10px 10px 10px; display: none;">

                        <td style="white-space: nowrap; text-align: right; width: 120px;" class="Label">Created Date *:

                        </td>
                        <td class="Label" align="right">
                            <asp:TextBox ID="txtCreatedDate" ReadOnly="true" runat="Server" CssClass="textbox"> </asp:TextBox>
                            <%--<cc1:CalendarExtender ID="calCreatedDate" runat="server" Format="dd-MMM-yyyy" TargetControlID="txtCreatedDate">
                    </cc1:CalendarExtender>--%>
                        </td>
                    </tr>


                </table>
                <br />
                <center>
                    <table>
                        <tr>
                            <td>
                                <center>
                                    <asp:Button ID="btnSaveCopyScreening" runat="server" CssClass="btn btnsave" Text="Copy" OnClientClick="return ScreeningValidation()" Width="105px" />
                                    <asp:Button ID="btnCancel1" runat="server" CssClass="btn btncancel" Text="Cancel" Width="105px" />
                                </center>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
            <cc1:ModalPopupExtender ID="ModalRemarks" runat="server" PopupControlID="divRemarks"
                BackgroundCssClass="modalBackground" TargetControlID="btnRemarls" BehaviorID="ModalRemarks"
                CancelControlID="CancelRemarks">
            </cc1:ModalPopupExtender>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel runat="server" ID="upFreeModal" UpdateMode="Conditional">
        <ContentTemplate>

            <div id="ModalBackGroundForFreeae" runat="server" class="divModalBackGround" style="display: none; width: 100% !Important;"></div>
            <button id="btnFreeze" runat="server" style="display: none;" />
            <div id="divFreeze" runat="server" class="centerModalPopup" style="display: none; left: 30%; width: 28%; position: absolute; top: 525px; border: 1px solid; height: 200px;">
                <div style="background-color: #1560A1;">
                    <table style="width: 90%; margin: auto;">
                        <tr>
                            <td colspan="2" class="LabelText" style="text-align: center !important; color: white; font-size: 14px !important; width: 97%;"><b>Enter Remarks</b>

                            </td>

                            <td style="text-align: center; height: 22px;" valign="top">
                                <img id="imgFreezeClose" alt="Close" src="images/Close.gif" style="position: relative; float: right; right: 5px; cursor: pointer;"
                                    title="Close" />
                            </td>
                        </tr>
                    </table>
                    <hr />
                </div>
                <table style="margin: 10px 10px 10px 10px;">
                    <tr style="margin: 10px 10px 10px 10px;">
                        <td style="white-space: nowrap; text-align: right; width: 120px;" class="Label"></td>
                        <td class="Label" align="right">
                            <asp:TextBox ID="txtTemplateheaderno" Visible="false" runat="Server" CssClass="textbox"> </asp:TextBox>
                        </td>
                    </tr>

                    <tr style="margin: 10px 10px 10px 10px;">
                        <td style="white-space: nowrap; text-align: right; width: 120px;" class="Label">Remarks *:
                        </td>
                        <td class="Label" align="right">
                            <asp:TextBox ID="txtRemarksFreeze" runat="Server" CssClass="textbox"> </asp:TextBox>
                        </td>
                    </tr>

                    <tr style="margin: 10px 10px 10px 10px;">

                        <td style="white-space: nowrap; text-align: right; width: 120px;" class="Label">Effective Date *:

                        </td>
                        <td class="Label" align="right">
                            <asp:TextBox ID="txtEffectiveDate" runat="Server" CssClass="textbox"> </asp:TextBox>
                            <cc1:CalendarExtender ID="clEffectiveDate" runat="server" Format="dd-MMM-yyyy" TargetControlID="txtEffectiveDate">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>


                </table>
                <br />
                <center>
                    <table>
                        <tr>
                            <td>
                                <center>
                                    <asp:Button ID="btnFreezeSave" OnClientClick="return FreezeValidation()" runat="server" CssClass="btn btnsave" Text="Save" Width="105px" />
                                    <asp:Button ID="btnFreezeCancel" runat="server" CssClass="btn btncancel" Text="Cancel" Width="105px" />
                                </center>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
            <cc1:ModalPopupExtender ID="mpfreeze" runat="server" PopupControlID="divFreeze"
                BackgroundCssClass="modalBackground" TargetControlID="btnFreeze" BehaviorID="mpfreeze"
                CancelControlID="imgFreezeClose">
            </cc1:ModalPopupExtender>
        </ContentTemplate>
    </asp:UpdatePanel>



    <button id="btnAuditTrail" runat="server" style="display: none;" />
    <cc1:ModalPopupExtender ID="modalpopupaudittrail" runat="server" PopupControlID="dvAudiTrail"
        BackgroundCssClass="modalBackground" TargetControlID="btnAuditTrail" BehaviorID="modalpopupaudittrail"
        CancelControlID="imgAuditTrail">
    </cc1:ModalPopupExtender>

    <div id="dvAudiTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
        <table border="0" cellpadding="2" cellspacing="2" width="100%">
            <tr>
                <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 80%;">Audit Trail Information</td>
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
                                <table id="tblAudit" class="tblAudit" width="100%"></table>
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

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="Hdn_ValidationType" runat="server" />
            <asp:HiddenField ID="Hdn_LengthNumeric" runat="server" />
            <asp:HiddenField ID="Hdn_FreezeStatus" runat="server" />
            <asp:HiddenField ID="Hdn_ProjectLock" runat="server" />
            <asp:HiddenField ID="hdndictionaryindic" runat="Server" />
            <asp:HiddenField ID="hdnMedexList" runat="server" />
            <asp:HiddenField ID="hdnGridname" runat="server" />
            <asp:HiddenField ID="hdnSelectedCheckBox" runat="server" />
            <input type="hidden" id="hd_TxtLength" value="0" />
            <asp:TextBox ID="TxtUpdateMedexDesc" runat="server" Style="display: none;" />
            <asp:TextBox ID="TxtUpdateMedexId" runat="server" Style="display: none;" />
            <asp:Button ID="BtnUpdateMedexDesc" runat="server" Style="display: none;" />

            <asp:Panel ID="pheader" runat="server">
                <div id="divExpandable" runat="server" style="font-weight: bold; font-size: 13px; float: none; color: white; background-color: #1560a1; width: 90%; margin: auto;">
                    <div>
                        <asp:Image ID="imgArrows" runat="server" Style="float: left;" />
                    </div>
                    <div style="text-align: left; margin-left: 3%; height: 20px;">
                        <%-- Changes done by Vikram--%>
                        <asp:Label ID="Label1" runat="server" Text="Project Criteria">
                        </asp:Label>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnltable" runat="server">
                <table width="95%" cellpadding="5px" style="margin-left: 0%; border: 1px solid; border-color: Black; width: 90%;">
                    <tbody>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:RadioButtonList class="Rb1" ID="RBLProjecttype" runat="server" Style="margin: auto; color: Black; padding: 10px; width: 45%;"
                                    AutoPostBack="True" RepeatDirection="Horizontal"
                                    TabIndex="1">
                                    <asp:ListItem Value="2">Project Specific Screening</asp:ListItem>
                                    <asp:ListItem Value="0000000000">Generic Screening</asp:ListItem>
                                    <asp:ListItem Selected="True" Value="1">Project Specific</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 35%; text-align: right; color: Black;" class="LabelDisplay">Project Name* :
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtproject" TabIndex="2" runat="server" CssClass="textBox" Width="45%" />
                                <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                                    TargetControlID="txtProject" ServicePath="AutoComplete.asmx" ServiceMethod="GetMyProjectCompletionList"
                                    OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1"
                                    CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                    CompletionListCssClass="autocomplete_list" CompletionListElementID="pnlProject"
                                    BehaviorID="AutoCompleteExtender1">
                                </cc1:AutoCompleteExtender>
                                <asp:Panel ID="pnlProject" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden;" />
                                <asp:Button Style="display: none" ID="btnSetProject" OnClick="btnSetProject_Click"
                                    runat="server" Text=" Project" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; color: Black;" id="tdActivity" runat="server" class="LabelDisplay">Activity* :
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlActivity" TabIndex="3" runat="server" Width="45%" />
                            </td>
                        </tr>
                        <tr runat="server" id="trScreeningGroup">
                            <td style="text-align: right; color: Black;" class="LabelDisplay">Attribute Group* :
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlScreeningGroup" runat="server" CssClass="dropDownList" AutoPostBack="True" TabIndex="4" Width="45%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; color: Black;" class="LabelDisplay">Template Name* :
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlTemplate" TabIndex="5" runat="server" Width="45%">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="Label" style="text-align: right; margin-left: 100px;">
                                <asp:UpdatePanel runat="server" ID="upCopyScreening" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnCopyScreening" runat="server" Text="Copy" Visible="false" ToolTip="Copy Screening" CssClass="btn btnnew" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnCopyScreening" EventName="click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Button ID="BtnSave" runat="server" Text="Attach" ToolTip="Attach" CssClass="btn btnsave"
                                    OnClientClick="return Validation();" TabIndex="5" />
                                <asp:Button ID="BtnDetach" runat="server" Text="Detatch" ToolTip="Detach" CssClass="btn btnnew"
                                    Visible="False" TabIndex="6" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="btn btncancel"
                                    TabIndex="7" />
                                <asp:Button ID="btnExit" runat="server" Text="Exit" ToolTip="Exit" CssClass="btn btnexit"
                                    TabIndex="8" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />



                            </td>
                        </tr>
                        <tr id="VersionDtl" runat="server" style="display: none;">
                            <td colspan="2" style="text-align: center; color: Black;">Version :<asp:Label runat="server" ID="VersionNo" Style="padding-right: 10px"></asp:Label>Version
                                Date :<asp:Label ID="VersionDate" Style="padding-right: 10px;" runat="server"></asp:Label>
                                Status:<img src="images/Freeze.jpg" id="ImageLockUnlock" runat="server" alt="Lock" />
                            </td>
                        </tr>
                        <%-- <tr>
                            <td>
                            
                            </tr>
                        </td>--%>
                    </tbody>

                </table>
            </asp:Panel>

            <cc1:CollapsiblePanelExtender ID="Collpase1" runat="server" TargetControlID="pnltable"
                ExpandControlID="pheader" CollapseControlID="pheader" ExpandedImage="images/panelcollapse.png"
                CollapsedImage="images/panelexpand.png" ImageControlID="imgArrows" AutoCollapse="false"
                AutoExpand="true">
            </cc1:CollapsiblePanelExtender>


            <asp:Panel ID="PanelProjectSpecific" runat="server" Style="display: none;">
                <%--margin-top: 2%;--%>
                <div id="div1" runat="server" style="font-weight: bold; font-size: 13px; float: none; color: white; background-color: #1560a1; width: 90%; margin: auto; margin-top: 2%;">
                    <div>
                        <asp:Image ID="ImgProjectSpecific" runat="server" Style="float: left;" />
                    </div>
                    <div style="text-align: left; margin-left: 3%;">
                        <asp:Label ID="lblprojecthdr" runat="server">
                        </asp:Label>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlgvmedexworkspadce" runat="server" Style="display: none;">
                <%--<div style="max-height:250px; height:200px; overflow:auto">--%>

                <table id="tblmedexworkspadce" class="table" width="95%" runat="server" style="margin-left: 0%; border: 1px solid; border-color: Black; padding: 3%; width: 90%;">
                    <%--<tbody>--%>
                    <tr>
                        <td class="Label" align="center">
                            <asp:GridView ID="gvmedexworkspadce" SkinID="grdViewAutoSizeMax" runat="server" Width="100%"
                                OnRowCreated="gvmedexworkspadce_RowCreated" OnRowEditing="gvmedexworkspadce_RowEditing"
                                AutoGenerateColumns="False" CellPadding="5" OnRowCommand="gvmedexworkspadce_RowCommand" TabIndex="9"
                                AllowPaging="true" PageSize="25" OnPageIndexChanging="gvmedexworkspadce_PageIndexChanging1">

                                <HeaderStyle Font-Underline="False" BackColor="#1560a1" Font-Names="Verdana" VerticalAlign="Top"
                                    HorizontalAlign="Center" ForeColor="white" Font-Bold="True" />
                                <Columns>
                                    <asp:TemplateField HeaderText="#">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSrNo" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="nMedExWorkSpaceHdrNo" HeaderText="MedExWorkSpaceHdrNo" />
                                    <asp:BoundField DataField="vWorkspaceId" HeaderText="WorkspaceId" />
                                    <asp:BoundField DataField="vProjectNo" HeaderText="ProjectNo" />
                                    <asp:BoundField DataField="vWorkspaceDesc" HeaderText="ProjectName">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Left" Wrap="True" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vActivityId" HeaderText="ActivityID" />
                                    <asp:BoundField DataField="ActivityDisplayName" HeaderText="Activity Name" ReadOnly="true">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Left" Wrap="True" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImbEdit" runat="server" ToolTip="Edit" ImageUrl="~/images/Edit2.gif"
                                                OnClientClick=" return SetAllowToEdit();" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Detach">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImbDelete" ToolTip="Detach" OnClientClick="return BtnDetachClick(this);"
                                                runat="server" ImageUrl="~/Images/dettach.png" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Preview">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImbPreview" ToolTip="Preview" runat="server" ImageUrl="~/images/view.gif" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="iNodeId" />
                                    <asp:TemplateField HeaderText="Order">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImbReorder" ToolTip="Order The Attributes" runat="server" ImageUrl="~/images/sorting.png" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <%--</tbody>--%>
                </table>
                <table width="95%" id="tblGeneralScr" runat="server" style="margin-left: 0%; border: 1px solid; border-color: Black; padding: 3%; width: 90%;">
                    <tbody>
                        <tr>
                            <td class="Label" align="center" colspan="2">
                                <asp:GridView ID="Gv_GeneralScr" runat="server" Width="100%" AutoGenerateColumns="False"
                                    TabIndex="9" SkinID="grdViewAutoSizeMax" CellPadding="5">
                                    <HeaderStyle Font-Underline="False" BackColor="#1560a1" Font-Names="Verdana" VerticalAlign="Top"
                                        HorizontalAlign="Center" ForeColor="white" Font-Bold="True" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="#">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSrNo" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="nScreeningTemplateHdrNo" HeaderText="nScreeningTemplateHdrNo" />
                                        <asp:BoundField DataField="vWorkspaceId" HeaderText="WorkspaceId" />
                                        <asp:BoundField DataField="vProjectNo" HeaderText="ProjectNo" />
                                        <asp:BoundField DataField="vActivityName" HeaderText="Activity Name">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="vScreeningTemplateVersionName" HeaderText="Version No">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImbEdit" runat="server" ToolTip="Edit" ImageUrl="~/images/Edit2.gif"
                                                    OnClientClick=" return SetAllowToEdit();" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Detach">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImbDelete" ToolTip="Detach" OnClientClick="return BtnDetachClick(this);"
                                                    runat="server" ImageUrl="~/Images/dettach.png" AlternateText="Dettach" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Preview">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImbPreview" ToolTip="Preview" runat="server" ImageUrl="~/images/view.gif" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Order">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImbReorder" ToolTip="Order The Attributes" runat="server" ImageUrl="~/images/sorting.png" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="imgFreezerStatus" Text='<%#  Eval("cFreezeStatus")%>' ToolTip="Status" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="cFreezeStatus" HeaderText="Status">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Audit Trail">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgAuditTrail" ToolTip="Audit Trail" runat="server" ImageUrl="Images/audit.png" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <table width="95%" id="tblProjectSpcScreening" runat="server" style="margin-left: 0%; border: 1px solid; border-color: Black; padding: 3%; width: 90%;">
                    <tbody>
                        <tr>
                            <td class="Label" align="center" colspan="2">
                                <asp:GridView ID="GVProjectSpcScreening" runat="server" Width="100%" AutoGenerateColumns="False"
                                    TabIndex="9" SkinID="grdViewAutoSizeMax" CellPadding="5">
                                    <HeaderStyle Font-Underline="False" BackColor="#1560a1" Font-Names="Verdana" VerticalAlign="Top"
                                        HorizontalAlign="Center" ForeColor="white" Font-Bold="True" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="#">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSrNo" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="nWorkSpaceScreeningHdrNo" HeaderText="nWorkSpaceScreeningHdrNo" />
                                        <asp:BoundField DataField="vWorkspaceId" HeaderText="WorkspaceId" />
                                        <asp:BoundField DataField="vProjectNo" HeaderText="ProjectNo" />
                                        <asp:BoundField DataField="vActivityName" HeaderText="Activity Name">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImbEdit" runat="server" ToolTip="Edit" ImageUrl="~/images/Edit2.gif"
                                                    OnClientClick=" return SetAllowToEdit();" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Detach">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImbDelete" ToolTip="Detach" OnClientClick="return BtnDetachClick(this);"
                                                    runat="server" ImageUrl="~/Images/dettach.png" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Preview">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImbPreview" ToolTip="Preview" runat="server" ImageUrl="~/images/view.gif" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Order">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImbReorder" ToolTip="Order The Attributes" runat="server" ImageUrl="~/images/sorting.png" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </tbody>
                </table>

                <%--</div>--%>
            </asp:Panel>

            <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="pnlgvmedexworkspadce"
                ExpandControlID="PanelProjectSpecific" CollapseControlID="PanelProjectSpecific"
                ExpandedImage="images/panelcollapse.png" CollapsedImage="images/panelexpand.png"
                ImageControlID="ImgProjectSpecific" AutoCollapse="false" AutoExpand="false">
            </cc1:CollapsiblePanelExtender>

            <br />
            <br />
            <asp:Panel ID="Panel1" runat="server" BorderStyle="None">
                <table id="Table1" class="table" width="95%" runat="server" style="margin-left: 0%; padding: 3%; width: 90%; border: none;">
                    <tbody>
                        <tr>
                            <td class="Label" align="center">
                                <asp:GridView ID="gvSubGroupWise" SkinID="grdViewAutoSizeMax" runat="server" max-Height="500px" Width="100%"
                                    AutoGenerateColumns="False" CellPadding="5" TabIndex="9" OnRowCommand="gvSubGroupWise_RowCommand" OnRowCreated="gvSubGroupWise_RowCreated" OnRowDataBound="gvSubGroupWise_RowDataBound">

                                    <HeaderStyle Font-Underline="False" BackColor="#1560a1" Font-Names="Verdana" VerticalAlign="Top"
                                        HorizontalAlign="Center" ForeColor="white" Font-Bold="True" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="#">
                                            <ItemTemplate>
                                                <asp:LinkButton Width="100%" ID="imgbtnvMedExSubGroupDesc" runat="server" Text='<%# Eval("vMedExSubGroupDesc")%>' />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Left" Width="100%" />
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="vMedexSubGroupCode" HeaderText="vMedexSubGroupCode" />
                                        <asp:BoundField DataField="vMedExSubGroupDesc" HeaderText="vMedExSubGroupDesc" />
                                        <asp:BoundField DataField="vWorkspaceId" HeaderText="vWorkspaceId" />
                                        <asp:BoundField DataField="iNodeId" HeaderText="iNodeId" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>

            <asp:Panel ID="PanelGridHeader" runat="server" Style="display: none;">
                <div id="divGrid" runat="server" style="font-weight: bold; font-size: 13px; float: none; color: white; background-color: #1560a1; width: 90%; margin: auto; margin-top: 2%;">
                    <div>
                        <asp:Image ID="ImgHeader" runat="server" Style="float: left;" />
                    </div>
                    <div style="text-align: left; margin-left: 3%;">
                        <asp:Label ID="lblProject" runat="server">
                        </asp:Label>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlMedEx" runat="server" Style="display: none;">
                <table style="width: 95%; border: 1px Solid Black; margin-left: 0%; padding: 1%; width: 90%;">
                    <tr>
                        <td>
                            <span id="Span1" runat="server"> &nbsp; </span>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left;">
                            <span id="tr_AttributeGroup1" runat="server">Attribute Group :
                                <asp:DropDownList ID="ddlMedexGroup" TabIndex="10" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlMedexGroup_SelectedIndexChanged" Width="15%" />
                            </span><span id="tr_Attribute1" runat="server">Attribute :
                                <asp:DropDownList ID="ddlMedex" TabIndex="11" runat="server" Width="15%" />
                            </span><span>
                                <asp:Button ID="btnAddMedEx" TabIndex="12" runat="server" Font-Bold="False"
                                    Text="Add" ToolTip="Add Selected Attributes To Grid" CssClass="btn btnadd" Style="font-weight: bold;" OnClick="btnAddMedEx_Click"
                                    OnClientClick="return ValidationToAdd();" />
                                <asp:Button ID="btndeleteMedex" TabIndex="13" runat="server" Text="Delete" 
                                    ToolTip="Remove Selected Attributes From Grid" CssClass="btn btnnew" /></span>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <asp:Button ID="btnSaveMedEx" TabIndex="14" runat="server" Text="Save" Width="60px"
                                ToolTip="Save Attributes To Database" CssClass="btn btnsave" OnClientClick="return SetAllowToEdit();" />
                            <asp:Button ID="btnCancelMedEx" TabIndex="15" runat="server" Text="Cancel"
                                ToolTip="Cancel" CssClass="btn btncancel" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlMedExGrid" runat="server">
                                <asp:GridView ID="gvwMedEx" runat="server" TabIndex="20" AutoGenerateColumns="False"
                                    Width="100%" SkinID="grdViewSmlAutoSize" DataKeyNames="vMedExType">
                                    <HeaderStyle Font-Underline="False" BackColor="#1560a1" Font-Names="Verdana" VerticalAlign="Top"
                                        HorizontalAlign="Center" ForeColor="white" Font-Bold="True" />
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="3%" ItemStyle-Width="3%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkMove" runat="server" />
                                                <%--<asp:CheckBox ID="ChkMove" runat="server" onclick="fnCheckBokSelection(this);" />--%>
                                                <asp:TextBox ID="txtMedexcodegvwMedEx" runat="server" Text='<%# eval("vMedExCode") %>'
                                                    Style="display: none;" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Details" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="2%"
                                            ItemStyle-Width="2%">
                                            <ItemTemplate>
                                                <asp:Image ID="ImbDetails" runat="server" AlternateText="Details" ImageUrl="~/images/attributedetails.png" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="nMedExWorkSpaceDtlNo" HeaderText="MedExWorkSpaceDtlNo" />
                                        <asp:BoundField DataField="nMedExWorkSpaceHdrNo" HeaderText="MedExWorkSpaceHdrNo" />
                                        <asp:BoundField DataField="vMedExCode" HeaderText="Medex Id" />

                                        <asp:TemplateField HeaderText="Attribute Description" HeaderStyle-Width="11%" ItemStyle-Width="11%">
                                            <ItemTemplate>
                                                <asp:TextBox TextMode="MultiLine" ID="txtMedexDesc" Text='<%# eval("vMedExDesc") %>'
                                                    runat="server" CssClass="textBox" Width="90%" Rows="3" Columns="9" ToolTip='<%# eval("vMedExDesc") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%-- <asp:BoundField DataField="vMedExType" HeaderText="Attribute Type" HeaderStyle-Width="5%"
                                                ItemStyle-Width="5%" />--%>
                                        <asp:TemplateField HeaderText="Attribute Type" HeaderStyle-Width="8%" ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlAttributeType" runat="server" Width="100%" onchange="OnChangeddlAttributeType(this)" OnDataBound="ddlAttributeType_DataBound">
                                                    <asp:ListItem Value="AsyncDateTime" Selected="True">AsyncDateTime</asp:ListItem>
                                                    <asp:ListItem Value="AsyncTime">AsyncTime</asp:ListItem>
                                                    <asp:ListItem Value="CheckBox">CheckBox</asp:ListItem>
                                                    <asp:ListItem Value="ComboBox">ComboBox</asp:ListItem>
                                                    <asp:ListItem Value="ComboGlobalDictionary">ComboGlobalDictionary</asp:ListItem>
                                                    <asp:ListItem Value="CrfTerm">CrfTerm</asp:ListItem>
                                                    <asp:ListItem Value="DateTime">DateTime</asp:ListItem>
                                                    <asp:ListItem Value="File">File</asp:ListItem>
                                                    <asp:ListItem Value="Formula">Formula</asp:ListItem>
                                                    <asp:ListItem Value="Import">Import</asp:ListItem>
                                                    <asp:ListItem Value="Label">Label</asp:ListItem>
                                                    <asp:ListItem Value="Radio">Radio</asp:ListItem>
                                                    <asp:ListItem Value="STANDARDDATE">STANDARDDATE</asp:ListItem>
                                                    <asp:ListItem Value="TextArea">TextArea</asp:ListItem>
                                                    <asp:ListItem Value="TextBox">TextBox</asp:ListItem>
                                                    <asp:ListItem Value="Time">Time</asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Attribute Value" HeaderStyle-Width="6%" ItemStyle-Width="6%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtValue" runat="server" CssClass="textBox " Text='<%# eval("vMedExValues") %>'
                                                    ToolTip='<%# eval("vMedExValues") %>' Width="85%"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Default Value" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDefaultValue" runat="server" CssClass="textBox " Text='<%# eval("vDefaultValue") %>'
                                                    Width="85%" ToolTip='<%# eval("vDefaultValue") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Alert On" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAlertOn" runat="server" CssClass="textBox" Text='<%# eval("vAlertonvalue") %>'
                                                    Width="85%" ToolTip='<%# eval("vAlertonvalue") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Alert Message" HeaderStyle-Width="8%" ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtalertMsg" runat="server" CssClass="textBox" Text='<%# eval("vAlertMessage") %>'
                                                    TextMode="MultiLine" Rows="3" Width="90%" Columns="9" ToolTip='<%# eval("vAlertMessage") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Low Range" HeaderStyle-Width="4%" ItemStyle-Width="4%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLowRange" runat="server" CssClass="ValidNum" Text='<%# eval("vLowRange") %>'
                                                    Width="85%" ToolTip='<%# eval("vLowRange") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="High Range" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtHighRange" runat="server" CssClass="ValidNum" Text='<%# eval("vHighRange") %>'
                                                    Width="85%" ToolTip='<%# eval("vHighRange") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="cActiveFlag" HeaderText="ActiveFlag" />
                                        <asp:BoundField DataField="iSeqNo" HeaderText="SeqNo" />
                                        <asp:BoundField DataField="vMedExGroupCode" HeaderText="MedExGroupCode" />
                                        <%--<asp:TemplateField HeaderText="Group">
                                    <ItemTemplate>
                                        <asp:TextBox TextMode="MultiLine" Width="200px" ID="txtGroupDesc" Text='<%# eval("vmedexgroupDesc") %>'
                                            runat="server" CssClass="textBox" cols="10" Rows="1"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                        <asp:BoundField DataField="vmedexgroupDesc" HeaderText="Attribute Group" />
                                        <asp:BoundField DataField="vMedexGroupCDISCValue" HeaderText="Group CDISC" />
                                        <asp:BoundField DataField="vmedexGroupOtherValue" HeaderText="Group Other Value" />
                                        <asp:BoundField DataField="vMedExSubGroupCode" HeaderText="MedExSubGroupCode" />
                                        <asp:BoundField DataField="vmedexsubGroupDesc" HeaderText=" Attribute SubGroup" />
                                        <asp:BoundField DataField="vMedexSubGroupCDISCValue" HeaderText="SubGroupCDISC" />
                                        <asp:BoundField DataField="vmedexsubGroupOtherValue" HeaderText="SubGroupOther" />
                                        <asp:TemplateField HeaderText="UOM" HeaderStyle-Width="8%" ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlUOMDesc" runat="server" ToolTip="Select UOM" Width="100%" onChange="return fnAssigntitle(this);" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Validation Type" HeaderStyle-Width="8%" ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlValidation" runat="server" Width="100%"
                                                    onchange="OnChangeDropDown(this)">
                                                    <asp:ListItem Value="NA">Not Applicable</asp:ListItem>
                                                    <asp:ListItem Value="AN">Alpha Numeric</asp:ListItem>
                                                    <asp:ListItem Value="NU">Numeric</asp:ListItem>
                                                    <asp:ListItem Value="IN">Integer</asp:ListItem>
                                                    <asp:ListItem Value="AL">Alphabate</asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Length" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLength" runat="server" CssClass="textBox " Width="90%" onkeypress="return ValidateNumericCode(event);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Not Null" HeaderStyle-Width="2%"
                                            ItemStyle-Width="2%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkAlertType" runat="server" Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="cRefType" HeaderText="RefType" />
                                        <asp:BoundField DataField="vRefTable" HeaderText="RefTable" />
                                        <asp:BoundField DataField="vRefColumn" HeaderText="RefColumn" />
                                        <asp:BoundField DataField="vRefFilePath" HeaderText="RefFilePath" />
                                        <asp:TemplateField HeaderText="Variable Name" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCDISCValues" runat="server" CssClass="textBox " Text='<%# eval("vCDISCValues") %>'
                                                    Width="90%" ToolTip='<%# eval("vCDISCValues") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Other" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtotherValues" runat="server" CssClass="textBox " Text='<%# eval("vOtherValues") %>'
                                                    Width="90%" ToolTip='<%# eval("vOtherValues") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Dictionary" HeaderStyle-Width="8%" ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlmeddra" runat="Server" Width="100%" Enabled="false">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="vUOM" HtmlEncode="false" />
                                        <asp:BoundField DataField="vValidationType" />
                                        <asp:BoundField DataField="cAlertType" />
                                        <asp:BoundField DataField="vMedexFormula" />
                                        <asp:BoundField DataField="iDecimalNo" HeaderStyle-CssClass="DecimalNo" ItemStyle-CssClass="DecimalNo" />
                                        <asp:TemplateField HeaderText="Formula" HeaderStyle-CssClass="EditFormula">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Imb_medex_Formula" ToolTip="Change formula" runat="server" ImageUrl="~/images/Edit_Formula.png" Style="height: 25%;" CssClass="EditFormula" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlProjectspScr" runat="server">
                                <asp:GridView ID="GV_ProjectSpScr" TabIndex="16" runat="server" AutoGenerateColumns="False"
                                    Width="100%" SkinID="grdViewSmlAutoSize">
                                    <HeaderStyle Font-Underline="False" BackColor="#1560a1" Font-Names="Verdana" VerticalAlign="Top"
                                        HorizontalAlign="Center" ForeColor="white" Font-Bold="True" />
                                    <Columns>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%" HeaderStyle-Width="3%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkMove" runat="server"></asp:CheckBox>
                                                <%--<asp:CheckBox ID="ChkMove" runat="server" onclick="fnCheckBokSelection(this);"></asp:CheckBox>--%>
                                                <asp:TextBox ID="txtMedexcodeGV_ProjectSpScr" runat="server" Text='<%# eval("vMedExCode") %>'
                                                    Style="display: none;" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Details" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="2%"
                                            ItemStyle-Width="2%">
                                            <ItemTemplate>
                                                <asp:Image ID="ImbDetails" runat="server" AlternateText="Details" ImageUrl="~/images/attributedetails.png" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="nWorkSpaceScreeningDtlNo" HeaderText="WorkSpaceScreeningDtlNo" />
                                        <asp:BoundField DataField="nWorkSpaceScreeningHdrNo" HeaderText="WorkSpaceScreeningHdrNo" />
                                        <asp:BoundField DataField="vMedExCode" HeaderText="Medex Id" />
                                        <asp:TemplateField HeaderText="Attribute Description" ItemStyle-Width="15%" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:TextBox TextMode="MultiLine" Width="90%" ID="txtMedexDesc" Text='<%# eval("vMedExDesc") %>'
                                                    runat="server" CssClass="textBox" cols="9" Rows="3" ToolTip='<%# eval("vMedExDesc") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="vMedExType" HeaderText="Attribute Type" />
                                        <asp:TemplateField HeaderText="Attribute Value" ItemStyle-Width="5%" HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtValue" runat="server" CssClass="textBox " Text='<%# eval("vMedExValues") %>'
                                                    ToolTip='<%# eval("vMedExValues") %>' Width="85%"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Default Value" ItemStyle-Width="6%" HeaderStyle-Width="6%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDefaultValue" runat="server" CssClass="textBox " Text='<%# eval("vDefaultValue") %>'
                                                    Width="85%" ToolTip='<%# eval("vDefaultValue") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Alert On" ItemStyle-Width="5%" HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAlertOn" runat="server" CssClass="textBox" Text='<%# eval("vAlertonvalue") %>'
                                                    Width="85%" ToolTip='<%# eval("vAlertonvalue") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Alert Message" ItemStyle-Width="14%" HeaderStyle-Width="14%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtalertMsg" runat="server" CssClass="textBox" Text='<%# eval("vAlertMessage") %>'
                                                    TextMode="MultiLine" Rows="3" Columns="9" Width="90%" ToolTip='<%# eval("vAlertMessage") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Low Range" ItemStyle-Width="5%" HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLowRange" runat="server" CssClass="ValidNum" Text='<%# eval("vLowRange") %>'
                                                    Width="85%" ToolTip='<%# eval("vLowRange") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="High Range" ItemStyle-Width="4%" HeaderStyle-Width="4%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtHighRange" runat="server" CssClass="ValidNum" Text='<%# eval("vHighRange") %>'
                                                    Width="85%" ToolTip='<%# eval("vHighRange") %>'> </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="cActiveFlag" HeaderText="ActiveFlag" />
                                        <asp:BoundField DataField="iSeqNo" HeaderText="SeqNo" />
                                        <asp:BoundField DataField="vMedExGroupCode" HeaderText="MedExGroupCode" />
                                        <asp:BoundField DataField="vmedexgroupDesc" HeaderText="Group" />
                                        <asp:BoundField DataField="vMedexGroupCDISCValue" HeaderText="Group CDISC" />
                                        <asp:BoundField DataField="vmedexGroupOtherValue" HeaderText="Group Other Value" />
                                        <asp:BoundField DataField="vMedExSubGroupCode" HeaderText="MedExSubGroupCode" />
                                        <asp:BoundField DataField="vmedexsubGroupDesc" HeaderText="SubGroup" />
                                        <asp:BoundField DataField="vMedexSubGroupCDISCValue" HeaderText="SubGroupCDISC" />
                                        <asp:BoundField DataField="vmedexsubGroupOtherValue" HeaderText="SubGroupOther" />
                                        <asp:TemplateField HeaderText="UOM" ItemStyle-Width="9%" HeaderStyle-Width="9%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlUOMDesc" ToolTip="Select UOM" runat="server" Width="90%" onChange="return fnAssigntitle(this);" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Validation Type" ItemStyle-Width="9%" HeaderStyle-Width="9%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlValidation" runat="server" Width="80%" onchange="OnChangeDropDown(this)">
                                                    <asp:ListItem Value="NA">Not Applicable</asp:ListItem>
                                                    <asp:ListItem Value="AN">Alpha Numeric</asp:ListItem>
                                                    <asp:ListItem Value="NU">Numeric</asp:ListItem>
                                                    <asp:ListItem Value="IN">Integer</asp:ListItem>
                                                    <asp:ListItem Value="AL">Alphabate</asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Length" ItemStyle-Width="5%" HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLength" runat="server" CssClass="textBox " Width="85%" onkeypress="return ValidateNumericCode(event);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Not Null" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%"
                                            HeaderStyle-Width="2%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkAlertType" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="cRefType" HeaderText="RefType" />
                                        <asp:BoundField DataField="vRefTable" HeaderText="RefTable" />
                                        <asp:BoundField DataField="vRefColumn" HeaderText="RefColumn" />
                                        <asp:BoundField DataField="vRefFilePath" HeaderText="RefFilePath" />
                                        <asp:TemplateField HeaderText="Variable Name" ItemStyle-Width="5%" HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCDISCValues" runat="server" CssClass="textBox " Text='<%# eval("vCDISCValues") %>'
                                                    Width="85%" ToolTip='<%# eval("vCDISCValues") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Other" ItemStyle-Width="5%" HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtotherValues" runat="server" CssClass="textBox " Text='<%# eval("vOtherValues") %>'
                                                    Width="85%" ToolTip='<%# eval("vOtherValues") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="vUOM" HtmlEncode="false" />
                                        <asp:BoundField DataField="vValidationType" />
                                        <asp:BoundField DataField="cAlertType" />
                                        <asp:BoundField DataField="vMedexFormula" />
                                        <asp:BoundField DataField="iDecimalNo" HeaderStyle-CssClass="DecimalNo" ItemStyle-CssClass="DecimalNo" />
                                        <asp:TemplateField HeaderText="Formula" HeaderStyle-CssClass="EditFormula">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Imb_project_spsc_Formula" ToolTip="Change formula" runat="server" ImageUrl="~/images/Edit_Formula.png" Style="height: 25%;" CssClass="EditFormula" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlGenScr" runat="server" Width="100%">
                                <asp:GridView ID="GvGenScr_Medex" TabIndex="16" SkinID="grdViewSmlAutoSize" runat="server"
                                    AutoGenerateColumns="False" Width="100%">
                                    <HeaderStyle Font-Underline="False" BackColor="#1560a1" Font-Names="Verdana" VerticalAlign="Top"
                                        HorizontalAlign="Center" ForeColor="white" Font-Bold="True" />
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="3%" ItemStyle-Width="3%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkMove" runat="server"></asp:CheckBox>
                                                <%--<asp:CheckBox ID="ChkMove" runat="server" CssClass="listcheckBox" onclick="fnCheckBokSelection(this);">
                                                    </asp:CheckBox>--%>
                                                <asp:TextBox ID="txtMedexcodeGvGenScr_Medex" runat="server" Text='<%# eval("vMedExCode") %>'
                                                    Style="display: none;" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Details" HeaderStyle-Width="2%" ItemStyle-Width="2%">
                                            <ItemTemplate>
                                                <asp:Image ID="ImbDetails" runat="server" AlternateText="Details" ImageUrl="~/images/attributedetails.png" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="nScreeningTemplateDtlNo" HeaderText="ScreeningTemplateDtlNo"></asp:BoundField>
                                        <asp:BoundField DataField="nScreeningTemplateHdrNo" HeaderText="ScreeningTemplateHdrNo"></asp:BoundField>
                                        <asp:BoundField DataField="vMedExCode" HeaderText="Medex Id" />
                                        <asp:TemplateField HeaderStyle-Width="15%" ItemStyle-Width="15%" HeaderText="Attribute Description">
                                            <ItemTemplate>
                                                <asp:TextBox TextMode="MultiLine" Width="90%" ID="txtMedexDesc" Text='<%# eval("vMedExDesc") %>'
                                                    runat="server" CssClass="textBox" cols="15" Rows="3" ToolTip='<%# eval("vMedExDesc") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="vMedExType" HeaderStyle-Width="5%" ItemStyle-Width="5%"
                                            HeaderText="Attribute Type" />
                                        <asp:TemplateField HeaderText="Attribute Value" HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtValue" runat="server" CssClass="textBox " Text='<%# eval("vMedExValues") %>'
                                                    ToolTip='<%# eval("vMedExValues") %>' Width="90%"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Default Value" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDefaultValue" runat="server" CssClass="textBox " Text='<%# eval("vDefaultValue") %>'
                                                    Width="90%" ToolTip='<%# eval("vDefaultValue") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Alert On" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAlertOn" runat="server" CssClass="textBox" Text='<%# eval("vAlertonvalue") %>'
                                                    Width="80%" ToolTip='<%# eval("vAlertonvalue") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Alert Message" HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtalertMsg" runat="server" CssClass="textBox" Text='<%# eval("vAlertMessage") %>'
                                                    TextMode="MultiLine" Width="90%" Rows="3" Columns="12" ToolTip='<%# eval("vAlertMessage") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Low Range" HeaderStyle-Width="4%" ItemStyle-Width="4%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLowRange" runat="server" CssClass="ValidNum" Text='<%# eval("vLowRange") %>'
                                                    Width="80%" ToolTip='<%# eval("vLowRange") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="High Range" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtHighRange" runat="server" CssClass="ValidNum" Text='<%# eval("vHighRange") %>'
                                                    Width="80%" ToolTip='<%# eval("vHighRange") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="cActiveFlag" HeaderText="ActiveFlag" />
                                        <asp:BoundField DataField="iSeqNo" HeaderText="SeqNo" />
                                        <asp:BoundField DataField="vMedExGroupCode" HeaderText="MedExGroupCode" />
                                        <asp:BoundField DataField="vmedexgroupDesc" HeaderText="Group" />
                                        <asp:BoundField DataField="vMedexGroupCDISCValue" HeaderText="Group CDISC" />
                                        <asp:BoundField DataField="vmedexGroupOtherValue" HeaderText="Group Other Value" />
                                        <asp:BoundField DataField="vMedExSubGroupCode" HeaderText="MedExSubGroupCode" />
                                        <asp:BoundField DataField="vmedexsubGroupDesc" HeaderText="SubGroup" />
                                        <asp:BoundField DataField="vMedexSubGroupCDISCValue" HeaderText="SubGroupCDISC" />
                                        <asp:BoundField DataField="vmedexsubGroupOtherValue" HeaderText="SubGroupOther" />
                                        <asp:TemplateField HeaderText="UOM" HeaderStyle-Width="8%" ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlUOMDesc" ToolTip="Select UOM" runat="server" CssClass="dropDownList" Width="100%"
                                                    onChange="return fnAssigntitle(this);" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Validation Type" HeaderStyle-Width="8%" ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlValidation" runat="server" CssClass="dropDownList" Width="100%"
                                                    onchange="OnChangeDropDown(this)">
                                                    <asp:ListItem Value="NA">Not Applicable</asp:ListItem>
                                                    <asp:ListItem Value="AN">Alpha Numeric</asp:ListItem>
                                                    <asp:ListItem Value="NU">Numeric</asp:ListItem>
                                                    <asp:ListItem Value="IN">Integer</asp:ListItem>
                                                    <asp:ListItem Value="AL">Alphabate</asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Length" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLength" runat="server" CssClass="textBox " Width="80%" onkeypress="return ValidateNumericCode(event);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Not Null" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="2%"
                                            ItemStyle-Width="2%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkAlertType" runat="server" Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="cRefType" HeaderText="RefType" />
                                        <asp:BoundField DataField="vRefTable" HeaderText="RefTable" />
                                        <asp:BoundField DataField="vRefColumn" HeaderText="RefColumn" />
                                        <asp:BoundField DataField="vRefFilePath" HeaderText="RefFilePath" />
                                        <asp:TemplateField HeaderText="Variable Name" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCDISCValues" runat="server" CssClass="textBox " Text='<%# eval("vCDISCValues") %>'
                                                    Width="90%" ToolTip='<%# eval("vCDISCValues") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Other" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtotherValues" runat="server" CssClass="textBox " Text='<%# eval("vOtherValues") %>'
                                                    Width="80%" ToolTip='<%# eval("vOtherValues") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="vUOM" HtmlEncode="false" />
                                        <asp:BoundField DataField="vValidationType" />
                                        <asp:BoundField DataField="cAlertType" />
                                        <asp:BoundField DataField="vMedexFormula" />
                                        <asp:BoundField DataField="iDecimalNo" HeaderStyle-CssClass="DecimalNo" ItemStyle-CssClass="DecimalNo" />
                                        <asp:TemplateField HeaderText="Formula" HeaderStyle-CssClass="EditFormula">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Imb_gen_scr_Formula" ToolTip="Change formula" runat="server" ImageUrl="~/images/Edit_Formula.png" Style="height: 25%;" CssClass="EditFormula" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <cc1:CollapsiblePanelExtender ID="Collpase2" runat="server" TargetControlID="pnlMedEx"
                ExpandControlID="PanelGridHeader" CollapseControlID="PanelGridHeader" ExpandedImage="images/panelcollapse.png"
                CollapsedImage="images/panelexpand.png" ImageControlID="ImgHeader" AutoCollapse="false"
                AutoExpand="false">
            </cc1:CollapsiblePanelExtender>


            <asp:Button ID="btnSequence" runat="server" Style="display: none;" />
            <cc1:ModalPopupExtender ID="MPEMedexSequence" runat="server" PopupControlID="divSequence"
                BackgroundCssClass="modalBackground" BehaviorID="MPEMedexSequence" TargetControlID="btnSequence">
            </cc1:ModalPopupExtender>
            <div id="divSequence" runat="server" class="centerModalPopup" style="width: 80%; position: absolute; max-height: 600px; display: none; top: 15%;">
                <table width="100%">
                    <tr>
                        <td>
                            <img id="ImgSeqCancel" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px;"
                                title="Close" onclick="return closesequencediv();" />
                            <asp:Label ID="lblhdr" runat="server" Text="Order Attributes" Style="font-weight: bold; color: Black; font-size: 14px; margin-left: 3%; float: left;" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <hr />
                        </td>
                    </tr>
                </table>
                <div id="divtips" style="width: 100%; margin: auto;">
                    <asp:Label ID="lblTips" runat="server" Text="Follow the attribute sequence in 
                    "
                        Style="color: Black; font-weight: normal; float: left; margin-left: 3%;" />
                    <img src="images/rightarrow.png" alt="Right Arrow" style="margin-right: 73%;" />
                    <br />
                    <asp:Label ID="lbltips2" runat="server" Text="To rearrange drag & drop the attribute to the required position"
                        Style="color: Black; font-weight: normal; float: left; margin-left: 3%;"></asp:Label>
                </div>
                <div id="divMedexSequence" style="width: 100%; margin: auto; margin-bottom: 2%;">
                    <%--<fieldset class="FieldSetBox" style="max-width: 90%; min-width: 70%; max-height: 560px;
                        margin: auto;">
                        <legend class="LegendText" style="color: Black">Change Sequence</legend>--%>
                    <ul id="SeqMedex" runat="server" style="list-style-type: none !important; padding: 10px; width: 100%;">
                    </ul>
                    <%--</fieldset>--%>
                </div>
                <table width="100%">
                    <tr>
                        <td>
                            <hr />
                        </td>
                    </tr>
                </table>
                <asp:Label ID="lblMedexCount" runat="server" Style="float: left; color: Black; margin-left: 3%;"></asp:Label>
                <input type="button" id="btnMedexseq" value="Save" title="Save The Sequence" style="float: left; margin-left: 30%;"
                    onclick="return savesequence();" />
                <input type="button" id="btnCloseModal" value="Cancel" title="Cancel" style="float: left;"
                    onclick="return closesequencediv();" />
                <asp:Button ID="btnSaveSequence" Text="Save" ToolTip="Save The Sequence" runat="server"
                    Style="margin: auto; display: none;" />
            </div>
            <asp:Button ID="btnEditFormula" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="MPEEditFormula" runat="server" PopupControlID="divEditFormula"
                BackgroundCssClass="modalBackground" BehaviorID="MPEEditFormula" TargetControlID="btnEditFormula">
            </cc1:ModalPopupExtender>
            <div id="divEditFormula" runat="server" class="centerModalPopup" style="width: 95%; top: -75px; top: 528px; text-align: left; height: 500px; left: 391px; display: none;">
                <table style="width: 100%;">
                    <tbody>
                        <tr>
                            <td align="center" width="100%">
                                <strong style="white-space: nowrap">Attribute Formula Master</strong>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" width="100%">
                                <div id="pnlFormulaMedEx" runat="server" style="width: 80%; overflow: auto; height: 300px;">
                                    <input type="text" id="searchlist" placeholder="Search Attribute..." style="float: left; width: 98%;"
                                        onkeyup="return DoListBoxFilter($('#ctl00_CPHLAMBDA_lstMedEx'), this.value, keys, values);" />
                                    <asp:ListBox ID="lstMedEx" runat="server" Height="249px" TabIndex="25" Style="width: 80% !important; overflow: auto;"
                                        CssClass="listbox"></asp:ListBox>
                                    <%--<cc1:ListSearchExtender  ID ="MEDEXlist"  runat ="server" TargetControlID ="lstMedEx" IsSorted =" true" PromptCssClass ="csextender" PromptPosition= "Bottom " PromptText ="Select Attribute" ></cc1:ListSearchExtender>--%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" width="100%">
                                <table width="60%" style="margin: auto;">
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="lblShowInEdit" Text="In Edit Mode, Clear All Formula and then Make it New one"
                                                runat="server" Style="display: none"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <input class="btn btnnew" tabindex="26" style="width: 100%;" onclick="CopyMedEx();" type="button"
                                                title="Copy Attribute" value="Copy Attribute" />
                                        </td>
                                        <td>
                                            <input class="btn btncancel" tabindex="30" onclick="ClearAllFormulaText();" type="button"
                                                value="Clear All" title="Clear All" />
                                            <input class="btn btncancel" tabindex="29" onclick="ClearFormulaText();" type="button" value="Clear"
                                                title="Clear" />
                                        </td>
                                        <td>
                                            <asp:Label ID="labelOperator" runat="server" CssClass="Label" Text="Operator"></asp:Label>
                                            <asp:DropDownList ID="ddlOperator" TabIndex="27" runat="server" CssClass="Label" onchange="return SetOperator();"
                                                Width="88px">
                                                <asp:ListItem></asp:ListItem>
                                                <asp:ListItem Text="+"></asp:ListItem>
                                                <asp:ListItem Text="-"></asp:ListItem>
                                                <asp:ListItem Text="*"></asp:ListItem>
                                                <asp:ListItem Text="/"></asp:ListItem>
                                                <asp:ListItem Text="("></asp:ListItem>
                                                <asp:ListItem Text=")"></asp:ListItem>
                                                <asp:ListItem Text="^"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDigits" runat="server" CssClass="Label" Text="Digits"></asp:Label>
                                            <asp:DropDownList ID="ddlNumbers" TabIndex="28" runat="server" CssClass="Label" Width="88px" onchange="return SetNumber();">
                                                <asp:ListItem></asp:ListItem>
                                                <asp:ListItem Text="."></asp:ListItem>
                                                <asp:ListItem Text="0"></asp:ListItem>
                                                <asp:ListItem Text="1"></asp:ListItem>
                                                <asp:ListItem Text="2"></asp:ListItem>
                                                <asp:ListItem Text="3"></asp:ListItem>
                                                <asp:ListItem Text="4"></asp:ListItem>
                                                <asp:ListItem Text="5"></asp:ListItem>
                                                <asp:ListItem Text="6"></asp:ListItem>
                                                <asp:ListItem Text="7"></asp:ListItem>
                                                <asp:ListItem Text="8"></asp:ListItem>
                                                <asp:ListItem Text="9"></asp:ListItem>
                                            </asp:DropDownList>
                                            &nbsp;&nbsp;&nbsp;
                                                                                    <asp:Label ID="lblDecimal" runat="server" CssClass="Label" Text="DecimalNo"></asp:Label>
                                            <asp:TextBox ID="txtDecimal" runat="server" CssClass="Label " ToolTip="Enter the No for Decimal Place"
                                                Width="70" onblur="return chkNumeric(this);"></asp:TextBox>
                                            <%--<asp:RequiredFieldValidator ID ="r1" ControlToValidate="txtDecimal" runat="server" ErrorMessage="Enter the decimal point" Font-Size="7px" ></asp:RequiredFieldValidator>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center" class="Label" align="left" colspan="4">
                                            <asp:TextBox ID="txtFormula" TabIndex="28" runat="server" Width="800px" TextMode="MultiLine"
                                                Height="95px" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center" class="Label" align="right" colspan="4">
                                            <asp:Button ID="btnSaveFormula" TabIndex="31" OnClick="btnSaveFormula_Click" runat="server"
                                                OnClientClick="return Validation1();" Text="Save" ToolTip="Save" CssClass="btn btnsave"></asp:Button>
                                            <input class="btn btncancel" tabindex="32" onclick="CloseFormula();" type="button"
                                                value="Close" title="Close" />
                                            <asp:HiddenField ID="HFFormula" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="HFFormula_Final" runat="server" />
                                            <asp:HiddenField ID="HFMedexCodeUsedForFormula" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="HFdecimalText" runat="server" />
                                            <asp:HiddenField ID="HFFormulaNo" runat="server" />
                                        </td>
                                    </tr>
                    </tbody>
                </table>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnAddMedEx" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="RBLProjecttype" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="btnSaveFormula" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="ddlMedexGroup" EventName="SelectedIndexChanged" />

        </Triggers>
    </asp:UpdatePanel>



    <script type="text/javascript">
        var jq = $.noConflict(true);
        jq(function () {
            jq("#my_tabs").sortable({
                event: "click" //click
            });
        });

        //$.noConflict();
        //$(document).ready(function () {



        //})

        function pageLoad() {
            //UIGV_gvmedexworkspadce()

            //    $('#<%= gvmedexworkspadce.ClientID%>').removeAttr('style', 'display:block');
            //    $('#<%= gvmedexworkspadce.ClientID%>').find('tbody tr').length < 3 ? scroll = "25%" : scroll = "100px";
            //    oTab = $('#<%= gvmedexworkspadce.ClientID%>').prepend($('<thead>').append($('#<%= gvmedexworkspadce.ClientID%> tr:first'))).dataTable({
            //        "bJQueryUI": true,
            //        "sPaginationType": "full_numbers",
            //        "bLengthChange": true,
            //        "iDisplayLength": 10,
            //        "bProcessing": true,
            //        "bSort": false,
            //        aLengthMenu: [
            //            [10, 25, 50, 100, -1],
            //            [10, 25, 50, 100, "All"]
            //        ],
            //    });
            //    $($('.ui-dialog-buttonset .ui-button-text')[1]).text("OK");
            // 


            var pnlWidth = screen.width - 100 + 'px';
            //$("#ctl00_CPHLAMBDA_pnlgvmedexworkspadce").width(pnlWidth);
            $("#ctl00_CPHLAMBDA_gvmedexworkspadce").width("100%;")


            $(window).width() > 1180 ? wid = ($(window).width() - 94) + 'px' : wid = $(window).width() - 100 + 'px';
            $('#<%= pnlgvmedexworkspadce.ClientID%>').attr("style", "width:" + wid + ";")

            //  if ($get('<%=gvmedexworkspadce.ClientID()%>') != null && $get('<%= gvmedexworkspadce.ClientID%>_wrapper') == null) {
            //      if (jQuery('#<%=gvmedexworkspadce.ClientID%>')) {
            //          jQuery('#<%=gvmedexworkspadce.ClientID%>').prepend($('<thead>').append($('#<%=gvmedexworkspadce.ClientID%> tr:first'))).DataTable({
            //  
            //              "bJQueryUI": true,
            //              "bPaginate": true,
            //              "bFooter": false,
            //              "bHeader": false,
            //              "AutoWidth": true,
            //              "bSort": false,
            //              "fixedHeader": true,
            //              "oLanguage": { "sSearch": "Search" },
            //              "aLengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
            //              "iDisplayLength": 10,
            //              "pagingType": "full_numbers",
            //              "oLanguage": {
            //                  "sEmptyTable": "No Record Found",
            //              }
            //          });
            //      }
            //      $(".dataTables_wrapper").css("width", ($(window).width() * 0.90 | 0) + "px");
            //  }



            function allowDrop(ev) {
                ev.preventDefault();
            }

            function drag(ev) {
                ev.dataTransfer.setData("text", ev.target.id);
            }

            function drop(ev) {
                ev.preventDefault();
                var data = ev.dataTransfer.getData("text");
                ev.target.appendChild(document.getElementById(data));
            }


            var ul_Sqlmedex = document.getElementById("ctl00_CPHLAMBDA_SeqMedex");
            if (ul_Sqlmedex.children.length > 0) {
                $('#ctl00_CPHLAMBDA_SeqMedex').sortable()
                $('#ctl00_CPHLAMBDA_SeqMedex').disableSelection();
            }

            if ($("#ctl00_CPHLAMBDA_ImbDetails").length > 0) {
                if ($.contains(document.body, $("#ctl00_CPHLAMBDA_ImbDetails")) == true) {
                    $("#ctl00_CPHLAMBDA_ImbDetails").tooltip();
                }
            }

            if ($("#ctl00_CPHLAMBDA_ddlMedex").length > 0) {
                if ($.contains(document.body, $("#ctl00_CPHLAMBDA_ddlMedex")) == true) {
                    $("#ctl00_CPHLAMBDA_ddlMedex").tooltip();
                }
            }

            $('#divMedexSequence').mouseover(function () {
                $(this).slimScroll({
                    height: '480px',
                    width: '100%',
                    size: '6px',
                    color: 'blue',
                    railVisible: true,
                    railColor: 'gray',
                    railOpacity: 0.3,
                    alwaysVisible: false
                });
            });

            //             $('.innerdiv').mouseover(function() {
            //                 //alert($(this).text());
            //           });

            if (document.getElementsByClassName('ValidNum') > 0) {
                $('.ValidNum').ForceNumericOnly();
            }
            FormulaColumnhide('ctl00_CPHLAMBDA_gvwMedEx');
            FormulaColumnhide('ctl00_CPHLAMBDA_GvGenScr_Medex');
            FormulaColumnhide('ctl00_CPHLAMBDA_GV_ProjectSpScr');
        }

        function FormulaColumnhide(GridviewName) {

            if (document.getElementById(GridviewName) != null) {
                var rowscount = document.getElementById(GridviewName).rows.length;
                if (rowscount > 0) {
                    if (document.getElementById(GridviewName).getElementsByClassName('EditFormula').length <= 1) {
                        for (var i = 0; i < rowscount; i++) {
                            var iformula = $($(document.getElementById(GridviewName)).find('tr')[i]).find('th').length;
                            if (iformula == 0) {
                                iformula = $($(document.getElementById(GridviewName)).find('tr')[i]).find('td').length;
                                $($(document.getElementById(GridviewName)).find('tr')[i]).find('td')[iformula - 1].style.display = "none";

                            }
                            else {
                                $($(document.getElementById(GridviewName)).find('tr')[i]).find('th')[iformula - 1].style.display = "none";
                            }
                            if (i == rowscount - 1 && $("#ctl00_CPHLAMBDA_GV_ProjectSpScr tr").length > 1) {
                                $($(document.getElementById(GridviewName)).find('tr')[i]).find('td')[iformula - 2].style.display = "none";
                            }
                        }
                    }
                    if ($("#ctl00_CPHLAMBDA_GV_ProjectSpScr tr").length > 1) {
                        $($(document.getElementById(GridviewName)).find('tr')[rowscount - 1]).find('td')[$($(document.getElementById(GridviewName)).find('tr')[rowscount - 1]).find('td').length - 2].style.display = "none";
                    }
                }
            }
        }

        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
                    $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));

        }

        function ValidationToAdd() {
            if (document.getElementById('<%=ddlMedExGroup.clientid %>').selectedIndex == 0) {
                msgalert('Select Attribute Group !');
                return false;
            }
            if (document.getElementById('<%=ddlMedEx.clientid %>').selectedIndex == 0) {
                msgalert('Select Attribute !');
                return false;
            }
            return true;
        }
        function Validation() {
            if ($('.Rb1 input:radio:checked').val() != "0000000000") {
                if (document.getElementById('<%= HProjectId.ClientId %>').value.toString().trim().length <= 0) {
                    msgalert('Please Enter Project !');
                    document.getElementById('<%= txtProject.ClientId %>').focus();
                    document.getElementById('<%= txtProject.ClientId %>').value = '';
                    return false;
                }
            }
            if ($('.Rb1 input:radio:checked').val() == "1") {
                if (document.getElementById('<%= ddlActivity.ClientId %>').selectedIndex == 0) {
                    msgalert('Please Select Activity !');
                    document.getElementById('<%= ddlActivity.ClientId %>').focus();
                    return false;
                }
            }

            if (document.getElementById('<%= ddlTemplate.ClientId %>').selectedIndex == 0) {
                msgalert('Please Select Template To Attach !');
                document.getElementById('<%= ddlTemplate.ClientId %>').focus();
                return false;
            }

            if (document.getElementById('<%= Hdn_FreezeStatus.clientid %>').value == "F") {
                msgalert("This Project Is Freezed.Kindly UnFreeze This To Change Project Structure !");
                return false;
            }
            return true;
        }

        function PreviewTemplate(path) {
            window.open(path);
            return false;
        }

        AllowToEdit = 0;
        function SetAllowToEdit() {
            if (document.getElementById('<%= Hdn_FreezeStatus.clientid %>').value == "F") {

            }
            else {
                AllowToEdit = 0;
            }
        }

        function ValidateNumeric(txtBox, ddlSelected, msg) {

            var strUser = ddlSelected.value;
            // var CommaSepratedNumbers=/[0-9]+(,[0-9]+)*,?/ ;
            var CommaSepratedNumbers = /([1-9][0-9]*,)*[0-9][0-9]*/;
            //            value3=document.getElementById("ctl00_CPHLAMBDA_txtlength").value;
            value3 = txtBox.value;
            var arrayList = value3.match(CommaSepratedNumbers);
            if (strUser == 'NU') {
                if (parseInt(value3.split(",")[0]) < parseInt(value3.split(",")[1])) {
                    msgalert('Please Enter Correct Scale !');
                    txtBox.value = "";
                    txtBox.focus();
                    return false;
                }
                if (strUser == 'NU' && (arrayList[1] == '' || typeof (arrayList[1]) == 'undefined')) {
                    msgalert('Please Provide Scale !');
                    txtBox.value = "";
                    txtBox.focus();
                    return false;
                }


                if (arrayList[1] == '') {
                    msgalert('Enter Data Not Correct Format !');
                    txtBox.value = "";
                    txtBox.focus();
                    return false;
                }
                else if (value3.split(",").length > 2) {
                    msgalert('Enter Data Not Correct Format !');
                    txtBox.value = "";
                    txtBox.focus();
                    return false;
                }
            }

            else

                var result = CheckDecimalOrBlank(txtBox.value);

            if (result == false) {
                window.alert(msg);
                txtBox.value = "";
                txtBox.focus();
            }
        }

        function ValidateMedexDesc(txtbox) {

            if (txtbox.value.toString().replace(/^\s*/, '').replace(/\s*$/, '').length <= 0) {
                txtbox.value = '';
                txtbox.focus();
                msgalert('Please Enter Attribute Description !');
                return false;
            }
            return true;
        }

        function ValidateAlertOn(txtboxMedexValue, txtboxAlertOn) {

            var txtMedExValue = txtboxMedexValue.value;
            var txtAlert = txtboxAlertOn.value;
            var result = txtMedExValue.split(",");

            if (txtboxAlertOn.value.toString().replace(/^\s*/, '').replace(/\s*$/, '').length > 0) {

                for (var i = 0 ; i < result.length; i++) {
                    var val = result[i].toString().replace(/^\s\s*/, '').replace(/\s\s*$/, '');
                    if (txtAlert == val) {
                        return true;
                    }
                }
                txtboxAlertOn.focus();
                msgalert('Please Enter Correct Alert On Value !');

                return false;
            }
            return true;
        }

        function BtnDetachClick(BtnId) {
            if (document.getElementById('<%= Hdn_FreezeStatus.clientid %>').value == "F") {
                msgalert("This Project Is Freezed.Kindly UnFreeze This To Change Project Structure");
                return false;
            }
            else {
                return confirm('Are You Sure You Want To Detach This Activity?');
            }
        }


        function ValidateNumericCode(evt) {
            var charCode = evt.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 44)
                return false;
            else
                if (charCode == 44) {
                    return true;
                }

            return true;
        }

        function OnChangeDropDown(e) {

            var const_txtLength = 14;
            var Const_txtLength_child = 0;
            $(e).attr('title', '');
            $(e).attr('title', e.options[e.selectedIndex].text);
            EditObject = e;
            $row = $(e).parents('tr:first');
            $get('hd_TxtLength').value = $row[0].cells[const_txtLength].children[Const_txtLength_child].value; //$row[0].cells[PatientId].firstChild.data;
            if (e.options[e.selectedIndex].text == 'Numeric') {
                $row[0].cells[const_txtLength].children[Const_txtLength_child].value = '0,0';
                return false;
            }
            else
                $row[0].cells[const_txtLength].children[Const_txtLength_child].value = 0;
            return false;
        }

        function OnChangeddlAttributeType(e) {
            $(e).attr('title', '');
            $(e).attr('title', e.options[e.selectedIndex].text);
        }

        function SetDropdownindex(Controlid, selectval) {

            var oldindex = selectval;
            var newindex = document.getElementById(Controlid).selectedIndex;
            if (oldindex != newindex) {
                msgConfirmDeleteAlert(null, "Change In Dictionary Will Require Recode For All Coded Data For This Attribute. Are You Sure You Want To Change The Dictionary?", function (isConfirmed) {
                    if (isConfirmed) {
                        document.getElementById(Controlid).selectedIndex = newindex;
                        $('#<%= hdndictionaryindic.clientid %>').val(1);
                        $("#" + Controlid).attr('title', '');
                        $("#" + Controlid).attr('title', $("#" + Controlid + " option:selected").text());
                        return true;
                    }
                    else {
                        $("#" + Controlid).val(oldindex)
                        //document.getElementById(Controlid).selectedIndex = oldindex; 
                        $('#<%= hdndictionaryindic.clientid %>').val("");
                        return true;
                    }
                });
                return false;

            }
        }

        function closesequencediv() {

            $find('MPEMedexSequence').hide();
        }

        function savesequence() {

            var jsondata = $('#<%= hdnMedexList.clientid %>').val();
            var i = 0;
            if (jsondata.d != "") {
                var data = JSON.parse(jsondata);
                var cpydata = JSON.parse(jsondata);
                $('.allmed').each(function () {

                    var medexcode = $(this).attr('id').replace("ctl00_CPHLAMBDA_", "");
                    for (var a = 0; a < cpydata.length; a++) {
                        if (cpydata[a].vMedExCode == medexcode) {
                            cpydata[a].iSeqNo = data[i].iSeqNo;
                            i = parseInt(i) + 1;
                        }
                    }
                });
                var btn = $('#<%= btnSaveSequence.clientid %>');
                document.getElementById('ctl00_CPHLAMBDA_hdnMedexList').value = JSON.stringify(cpydata);
                btn.click();
                return false;
            }
        }

        jQuery.fn.ForceNumericOnly =
       function () {
           return this.each(function () {
               $(this).keydown(function (e) {
                   var key = e.charCode || e.keyCode || 0;
                   // allow backspace, tab, delete, arrows, numbers and keypad numbers ONLY
                   // home, end, period, and numpad decimal
                   return (
                        key == 8 ||
                        key == 9 ||
                        key == 46 ||
                        key == 110 ||
                        key == 190 ||
                        (key >= 35 && key <= 40) ||
                        (key >= 48 && key <= 57) ||
                        (key >= 96 && key <= 105));
               });
           });
       };

        function fnAssigntitle(ControlID) {

            $(ControlID).attr('title', '');
            $(ControlID).attr('title', ControlID.options[ControlID.selectedIndex].text);
        }

        function CloseFormula() {
            document.getElementById('<%=HFFormula.ClientID%>').value = document.getElementById('<%=HFFormula_Final.ClientID%>').value;
            $find('MPEEditFormula').hide();
            //document.getElementById("ctl00_CPHLAMBDA_divEditFormula").style.display = "none";
            return false;
        }

        function ClearAllFormulaText() {
            document.getElementById('<%=txtFormula.ClientID%>').value = '';
            document.getElementById('<%=HFFormula.ClientID%>').value = '';
            document.getElementById('<%=HFMedexCodeUsedForFormula.ClientID%>').value = '';
        }

        function ClearFormulaText() {
            var cursorPosition = $('#ctl00_CPHLAMBDA_txtFormula').prop("selectionStart");
            var txt = document.getElementById('<%=txtFormula.ClientID%>').value;
            var formula = document.getElementById('<%=HFFormula.ClientID%>').value;
            txt = txt.substring(0, txt.length - 1)
            formula = formula.substring(0, formula.length - 2)
            document.getElementById('<%=txtFormula.ClientID%>').value = txt
            document.getElementById('<%=HFFormula.ClientID%>').value = formula;
        }

        function CopyMedEx() {

            var vMedexCode = new Array
            var aLength = vMedexCode.length
            var lst = document.getElementById('<%=lstMedEx.ClientID%>');
            var items = lst.options.length;
            if (lst != null && typeof (lst) != 'undefined') {
                for (i = 0; i < items; i++) {
                    if (lst.options[i].selected) {
                        document.getElementById('<%=txtFormula.ClientID%>').value += lst.options[i].text;
                        document.getElementById('<%=HFFormula.ClientID%>').value += lst.options[i].value + '?';
                        document.getElementById('<%=HFMedexCodeUsedForFormula.ClientID%>').value += lst.options[i].value + ',';
                        break;
                    }
                }
            }
        }

        function SetNumber() {
            var op = document.getElementById('<%=ddlNumbers.ClientID%>').value;
            if (op != '') {
                document.getElementById('<%=txtFormula.ClientID%>').value += op;
                document.getElementById('<%=HFFormula.ClientID%>').value += op + '?';
            }
            document.getElementById('<%=ddlNumbers.ClientID%>').selectedIndex = 0;
        }

        function SetOperator() {
            var op = document.getElementById('<%=ddlOperator.ClientID%>').value;
            if (op != '') {
                document.getElementById('<%=txtFormula.ClientID%>').value += op;
                document.getElementById('<%=HFFormula.ClientID%>').value += op + '?';
            }
            document.getElementById('<%=ddlOperator.ClientID%>').selectedIndex = 0;
        }

        function Validation1() {


            if (document.getElementById('<%=txtDecimal.ClientID%>').value == '') {
                msgalert('Please Enter Decimal No !');
                document.getElementById('<%=txtDecimal.ClientID%>').focus();
                return false;
            }
        }

        var keys = [];
        var values = [];

        function DoListBoxFilter(listBoxSelector, filter, keys, values) {

            var list = $(listBoxSelector);
            var selectBase = '<option value="{0}">{1}</option>';

            list.html("");
            for (i = 0; i < values.length; ++i) {
                var value = values[i];

                if (value == "" || value.toLowerCase().indexOf(filter.toLowerCase()) >= 0) {
                    var temp = '<option value="' + keys[i] + '">' + value + '</option>';
                    list.append(temp);
                }
            }
        }

        function FillMedexAttribute() {
            keys = [];
            values = [];
            var options = $('#ctl00_CPHLAMBDA_lstMedEx option');
            $.each(options, function (index, item) {

                keys = keys.concat(item.value);
                values = values.concat(item.text);

            });
        }

        $(document).ready(function () {

            var watermark = 'Enter text to Search Attribute...';
            $('#searchlist').blur(function () {
                if ($(this).val().length == 0)
                    $(this).val(watermark).addClass('watermark');
            }).focus(function () {
                if ($(this).val() == watermark)
                    $(this).val('').removeClass('watermark');
            }).val(watermark).addClass('watermark');


            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            function EndRequestHandler(sender, args) {
                var watermark = 'Enter text to Search Attribute...';
                $('#searchlist').blur(function () {

                    if ($(this).val().length == 0)
                        $(this).val(watermark).addClass('watermark');
                }).focus(function () {
                    if ($(this).val() == watermark)
                        $(this).val('').removeClass('watermark');
                }).val(watermark).addClass('watermark');
            }


        });

        function chkNumeric(ele) {
            var val;
            val = document.getElementById(ele.id).value.toString().trim();
            if (val == "") {
                msgalert('Please Enter Decimal No !');
                return false;
            }
            var ex = /^[0-9]$/;
            if (ex.test(document.getElementById(ele.id).value) == false) {
                msgalert("Please Enter Decimal No In Numeric !");
                document.getElementById(ele.id).focus();
                document.getElementById(ele.id).value = "";
                return false;
            }
            return true;
        }

        function ScreeningValidation() {
            if ((ctl00_CPHLAMBDA_txtVersion).value == '') {
                msgalert('Please Enter Version Number !')
                return false;
            }
            if ((ctl00_CPHLAMBDA_txtRemarks).value == '') {
                msgalert('Please Enter Remark !')
                return false;
            }
            if ((ctl00_CPHLAMBDA_txtCreatedDate).value == '') {
                alert('Please Enter Created Date !')
                return false;
            }
        }

        function Audittrail(TemplateId) {
            $.ajax({
                type: "post",
                url: "frmMedExWorkspaceDtl.aspx/AuditTrail",
                data: '{"TemplateTd":"' + TemplateId + '"}',
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
                            InDataSet.push(RowId, data[Row].CreatedDate, data[Row].VersionName, data[Row].vRemarks, data[Row].Status, data[Row].Modifyby, data[Row].dModifyOn);
                            aaDataSet.push(InDataSet);

                        }

                        if ($("#tblAudit").children().length > 0) {
                            $("#tblAudit").dataTable().fnDestroy();
                        }
                        oTable = $('#tblAudit').prepend($('<thead>').append($('#tblAudit tr:first'))).dataTable({
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
                                { "sTitle": "Created Date" },
                                { "sTitle": "Version Name" },
                                { "sTitle": "Remarks" },
                                { "sTitle": "Status" },
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


        function FreezeValidation() {


            if ((ctl00_CPHLAMBDA_txtRemarksFreeze).value == '') {
                msgalert('Please Enter Remarks !')
                return false;
            }
            if ((ctl00_CPHLAMBDA_txtEffectiveDate).value == '') {
                msgalert('Please Enter Effective Date !')
                return false;
            }

            var today = new Date(new Date().format("dd-MMM-yyyy"));
            var EnterDate = new Date((ctl00_CPHLAMBDA_txtEffectiveDate).value);
            if (EnterDate < today) {
                msgalert("Please Enter Date Grater than or Equal To Today's Date !")
                return false;
            }
        }

        function UIGV_gvmedexworkspadce() {
            $('#<%= gvmedexworkspadce.ClientID%>').removeAttr('style', 'display:block');
            oTab = $('#<%= gvmedexworkspadce.ClientID%>').prepend($('<thead>').append($('#<%= gvmedexworkspadce.ClientID%> tr:first'))).dataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                //"iDisplayLength": 25,
                "bProcessing": true,
                "bSort": false,

            });
            return false;
        }

        function fnExpandTrueFalse() {
            $("#ctl00_CPHLAMBDA_pnlgvmedexworkspadce").slideUp(400)
            //$("#ctl00_CPHLAMBDA_pnlgvmedexworkspadce").click();
        }
    </script>

</asp:Content>
