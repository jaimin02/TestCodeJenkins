<%@ Page Language="VB" MasterPageFile="~/ECTDMasterPageForSDTM.master" CodeFile="~/frmMedExWorkspaceDtlNew.aspx.vb"
    AutoEventWireup="false" Inherits="frmMedExWorkspaceDtlNew" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <link rel="stylesheet" type="text/css" href="App_Themes/smoothnessjquery-ui.css" />


    <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

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

        #tblMedExMst_wrapper.dataTables_wrapper {
            overflow: auto;
            display: block;
            width: 1300px;
            /*max-height: 400px;*/
        }

        #ctl00_CPHLAMBDA_dvMedExSubGroupMst {
            height:100% !important;
            overflow:auto;
            display:block;
        }

        #tblMedExMst.tblAudit.dataTable {
            overflow: auto;
            display: block;
        }
         
        #ctl00_CPHLAMBDA_dvMedExSubGroupMst.centerModalPopup {
            max-height: 100% !important;
            top: 0px !important;
        }

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

        .loaderClientSide {
            display: none;
            position: fixed;
            z-index: 1000;
            top: 0;
            left: 0;
            height: 100%;
            width: 100%;
            background: rgba( 255, 255, 255, .5 ) url('images/AjaxLoader.gif') 50% 50% no-repeat;
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
                                    <asp:Button ID="btnSaveCopyScreening" runat="server" CssClass="btn btnnew" Text="Copy" OnClientClick="return ScreeningValidation()" Width="105px" />
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
                <td id="Td2" class="LabelText" style="font-weight: bold;  text-align: center !important; font-size: 15px !important; width: 80%;">Audit Trail Information</td>
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
                            <td style="text-align: right; color: Black;" class="LabelDisplay">Screening Group* :
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
                        <tr>
                            <td>
                        </tr>
                        </td>
                    </tbody>

                </table>


            </asp:Panel>
            <cc1:CollapsiblePanelExtender ID="Collpase1" runat="server" TargetControlID="pnltable"
                ExpandControlID="pheader" CollapseControlID="pheader" ExpandedImage="images/panelcollapse.png"
                CollapsedImage="images/panelexpand.png" ImageControlID="imgArrows" AutoCollapse="false"
                AutoExpand="false">
            </cc1:CollapsiblePanelExtender>
            <asp:Panel ID="PanelProjectSpecific" runat="server" Style="display: none; margin-top: 2%;">
                <div id="div1" runat="server" style="font-weight: bold; font-size: 13px; float: none; color: white; background-color: #1560a1; width: 90%; margin: Auto;">
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
                <table id="tblmedexworkspadce" class="table" width="95%" runat="server" style="margin-left: 0%; border: 1px solid; border-color: Black; padding: 3%; width: 90%;">
                    <tbody>
                        <tr>
                            <td class="Label" align="center">
                                <asp:GridView ID="gvmedexworkspadce" SkinID="grdViewAutoSizeMax" runat="server" max-Height="500px" Width="100%"
                                    OnRowEditing="gvmedexworkspadce_RowEditing" OnRowCreated="gvmedexworkspadce_RowCreated"
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
                                                <asp:ImageButton ID="ImbEdit" runat="server" ToolTip="Edit" ImageUrl="~/images/Edit2.gif" />
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
                    </tbody>
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
                                                <asp:ImageButton ID="ImbEdit" runat="server" ToolTip="Edit" ImageUrl="~/images/Edit2.gif" />
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
                                                <asp:ImageButton ID="ImbEdit" runat="server" ToolTip="Edit" ImageUrl="~/images/Edit2.gif" />
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
            </asp:Panel>
            <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="pnlgvmedexworkspadce"
                ExpandControlID="PanelProjectSpecific" CollapseControlID="PanelProjectSpecific"
                ExpandedImage="images/panelcollapse.png" CollapsedImage="images/panelexpand.png"
                ImageControlID="ImgProjectSpecific" AutoCollapse="false" AutoExpand="True">
            </cc1:CollapsiblePanelExtender>
            <asp:Panel ID="PanelGridHeader" runat="server">
            </asp:Panel>

            <div id="divSubGroupPanel" style="margin: 10px 100px 10px 107px;">
            </div>
            
            <div id='loaderouter' class="loaderClientSide" style='display: none'>    </div>

            <div id="dvMedExSubGroupMst" runat="server" class="centerModalPopup" style="display: none; width: 99%; max-height: 75%">
                <div id='loadingmessage' class="loaderClientSide" style='display: none'>    </div>

                <table border="0" cellpadding="2" cellspacing="2" width="100%">
                    <tr style="font-weight: bold;  text-align: center !important; font-size: 15px !important; width: 97%;">
                        <td>
                            <h4 class="modal-title">
                                <label id="lblHeader">Activity Name :</label>
                                <label id="lblActivityName"></label>
                            </h4>
                        </td>
                        <td style="width: 3%">
                            <img id="imgMedExSubGroupMs" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr id="tblAddAttributeGroup">
                        <td>
                            <table width="100%">
                                <tr>
                                    <td style="text-align: right">Attribute Group :
                                    </td>
                                    <td style="text-align: left; width: 200px;">
                                        <select id="ddlAttributeGroup" style="text-align: left; width: 200px;" onchange="GetAttributeName();">
                                        </select>
                                    </td>
                                    <td style="text-align: right">Attribute :
                                    </td>
                                    <td style="text-align: left; width: 200px;">
                                        <select id="ddlAttribute" style="text-align: left; width: 200px;">
                                        </select>
                                    </td>
                                    <td></td>
                                    <td>
                                        <input type="button" value="Add" id="btnAddAttribute" onclick='AddAttributeData()' class="btn btnadd" />
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
                    <tr class="SaveDeleteHide">
                        <td colspan="2">
                            <div class="table-responsive " style="max-height: 200px !important; overflow: auto;">
                                <table class="table" id="tblMedExMstTemp">
                                    <thead>
                                        <tr>
                                            <th>AttributeGroupCode</th>
                                            <th>Attribute Group</th>
                                            <th>AttributeCode</th>
                                            <th>Attribute Description</th>
                                            <th>Delete</th>
                                        </tr>
                                    </thead>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr class="SaveDeleteHide">
                        <td colspan="2">
                            <input type="button" value="Save" id="btnSaveAttribute" onclick='SaveAttributeData()' class="btn btnsave" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table align="center" border="0" cellpadding="2" cellspacing="2" width="100%">
                                <tr>
                                    <td>
                                        <div class="table-responsive " style="max-height: 300px !important; overflow: auto;">
                                            <table id="tblMedExMst" class="tblAudit" width="100%" style="background-color: aliceblue;"></table>
                                        </div>
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
                    <tr>
                        <td>
                            <button type="button" id="btnSaveEditedData" class="btn btnsave SaveDeleteHide" onclick="SaveEditedData()">Update</button>
                            <button type="button" id="btnDeleteAttribute" onclick='DeleteAttributeData()'  class="btn btncancel SaveDeleteHide" data-dismiss="modal">Delete</button>
                            <button type="button" class="btn btnclose" id="btnCloseForModal" onclick="return CloseModalPopup()">Close</button>
                        </td>
                    </tr>
                </table>
            </div>

            <button id="btn3" runat="server" style="display: none;" />

            <cc1:ModalPopupExtender ID="MPE_MedExSubGroupMst" runat="server" PopupControlID="dvMedExSubGroupMst" BehaviorID="MPE_MedExSubGroupMst"
                PopupDragHandleControlID="LbldRandomizationDtl" BackgroundCssClass="modalBackground" CancelControlID="imgMedExSubGroupMs"
                TargetControlID="btn3">
            </cc1:ModalPopupExtender>

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

            <asp:AsyncPostBackTrigger ControlID="RBLProjecttype" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="btnSaveFormula" EventName="Click" />


        </Triggers>
    </asp:UpdatePanel>
     
    <script type="text/javascript" src="Script/General.js"></script>
    <script type="text/javascript" src="Script/Validation.js"></script>
    <script type="text/javascript" src="Script/slimScroll.min.js"></script>
    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <%--<script type="text/javascript" src="Script/googleapis.js" ></script>--%>
    <%--<script type="text/javascript" src="Script/Multiselect/jquery.dataTables.js"></script>--%>
    <script type="text/javascript" src="Script/jquery-ui.js"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.js" ></script>
    <%--<script type="text/javascript" src="Script/jquery-1.10.13.js" ></script>--%>
    <script type="text/javascript" src="Script/jquery-1.12.4.min.js" ></script>

    <script type="text/javascript">
        var jq = $.noConflict(true);
        $(function () {
            $("#my_tabs").sortable({
                event: "click" //click
            });
        });
        function pageLoad() {
            
            var pnlWidth = screen.width - 100 + 'px';
            $("#ctl00_CPHLAMBDA_gvmedexworkspadce").width("100%;")


            $(window).width() > 1180 ? wid = ($(window).width() - 94) + 'px' : wid = $(window).width() - 100 + 'px';
            $('#<%= pnlgvmedexworkspadce.ClientID%>').attr("style", "width:" + wid + ";")

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
            var CommaSepratedNumbers = /([1-9][0-9]*,)*[0-9][0-9]*/;
            value3 = txtBox.value;
            var arrayList = value3.match(CommaSepratedNumbers);
            if (strUser == 'NU') {
                if (parseInt(value3.split(",")[0]) < parseInt(value3.split(",")[1])) {
                    msgalert('Please Enter correct Scale !');
                    txtBox.value = "";
                    txtBox.focus();
                    return false;
                }
                if (strUser == 'NU' && (arrayList[1] == '' || typeof (arrayList[1]) == 'undefined')) {
                    msgalert('Please provide scale !');
                    txtBox.value = "";
                    txtBox.focus();
                    return false;
                }


                if (arrayList[1] == '') {
                    msgalert('Enter data not correct format !');
                    txtBox.value = "";
                    txtBox.focus();
                    return false;
                }
                else if (value3.split(",").length > 2) {
                    msgalert('Enter data not correct format !');
                    txtBox.value = "";
                    txtBox.focus();
                    return false;
                }
            }

            else

                var result = CheckDecimalOrBlank(txtBox.value);

            if (result == false) {
                window.msgalert(msg);
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
                msgalert('Please Enter Correct alert on value !');

                return false;
            }
            return true;
        }

        function BtnDetachClick(e) {
            if (document.getElementById('<%= Hdn_FreezeStatus.clientid %>').value == "F") {
                msgalert("This Project Is Freezed.Kindly UnFreeze This To Change Project Structure !");
                return false;
            }
            else {
                msgConfirmDeleteAlert(null, "Are You Sure You Want To Detach This Activity ?", function (isConfirmed) {
                    if (isConfirmed) {
                        __doPostBack(e.name, '');
                        return true;
                    } else {
                        return false;
                    }
                });
                return false;
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

        function SetDropdownindex(Controlid, selectval) {

            var oldindex = selectval;
            var newindex = document.getElementById(Controlid).selectedIndex;
            if (oldindex != newindex) {
                msgConfirmDeleteAlert(null, "Change In Dictionary Will Require Recode For All Coded Data For This Attribute. Are You Sure You Want To Change The Dictionary ?", function (isConfirmed) {
                    if (isConfirmed) {
                        document.getElementById(Controlid).selectedIndex = newindex;
                        $('#<%= hdndictionaryindic.clientid %>').val(1);
                        $("#" + Controlid).attr('title', '');
                        $("#" + Controlid).attr('title', $("#" + Controlid + " option:selected").text());
                    }
                    else {
                        $("#" + Controlid).val(oldindex)
                        $('#<%= hdndictionaryindic.clientid %>').val("");
                    }
                });
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
                msgalert('Please enter Decimal No !');
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
                msgalert('Please enter Decimal No !');
                return false;
            }
            var ex = /^[0-9]$/;
            if (ex.test(document.getElementById(ele.id).value) == false) {
                msgalert("Please Enter Decimal No in numeric !");
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
                msgalert('Please Enter Created Date !')
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
                msgalert("Please Enter Date Grater Than or Equal To Today's Date !")
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

        ////Change By ketan for edit data
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        function GetSubGroupName(vWorkspaceId, Scope, iNodeId, ActivityName) {
            ActivityName_ForEdit = ActivityName;
            onUpdating();

            debugger;
            $("#ctl00_CPHLAMBDA_div1").click();

            if ($("#ctl00_CPHLAMBDA_Hdn_FreezeStatus").val() == "F") {
                $("#tblAddAttributeGroup").attr("style", "display:none");
                $(".SaveDeleteHide").attr("style", "display:none");

            }
           else if ($("#ctl00_CPHLAMBDA_Hdn_ProjectLock").val() == "L") {
                $("#tblAddAttributeGroup").attr("style", "display:none");
                $(".SaveDeleteHide").attr("style", "display:none");

            }
            else {
                $("#tblAddAttributeGroup").attr("style", "display:");
                $(".SaveDeleteHide").attr("style", "display:");
            }

            $.ajax({
                type: "POST",
                url: "frmMedExWorkspaceDtlNew.aspx/GetSubGroupName",
                data: '{"vWorkspaceId":"' + vWorkspaceId + '","Scope":"' + Scope + '","iNodeId":"' + iNodeId + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    var aaDataSet = [];
                    data = JSON.parse(data.d);
                    $("#divSubGroupPanel").html("")
                    for (var Row = 0; Row < data.length; Row++) {
                        var InDataSet = [];
                        var $input = $('<input type="button"  class="btn btn-info" data-target="#myModal"  style="width:100%; color:white; font-weight: bold; text-align:left; background-color:#4974c3;" Class="Button" onClick=  DisplayAttribute("' + data[Row].vWorkSpaceId + '","' + data[Row].vMedExSubGroupCode + '","' + Scope + '","' + iNodeId + '")  value="' + data[Row].vMedExSubGroupDesc + '"  id=btn' + data[Row].vMedExSubGroupCode + '"/>');
                        $input.appendTo($("#divSubGroupPanel"));
                        $("#divSubGroupPanel").append("</BR>")
                    }

                },
                failure: function (error) {
                    alert(error);
                }
            });
            onUpdated();
            return false;
        }

        function GetSubGroupName_SpecificScreening(nWorkSpaceScreeningHdrNo, vWorkspaceId, Scope, ActivityName) {
            onUpdating();
            ActivityName_ForEdit = ActivityName;
            if (document.getElementById('<%= Hdn_FreezeStatus.clientid %>').value == "F") {

            }
            else {
                AllowToEdit = 0;
            }
            $("#ctl00_CPHLAMBDA_div1").click();

            if ($("#ctl00_CPHLAMBDA_Hdn_FreezeStatus").val() == "F") {
                $("#tblAddAttributeGroup").attr("style", "display:none");
                $(".SaveDeleteHide").attr("style", "display:none");
            }
            else if ($("#ctl00_CPHLAMBDA_Hdn_ProjectLock").val() == "L") {
                $("#tblAddAttributeGroup").attr("style", "display:none");
                $(".SaveDeleteHide").attr("style", "display:none");
            }
            else {
                $("#tblAddAttributeGroup").attr("style", "display:");
                $(".SaveDeleteHide").attr("style", "display:");
            }


            $.ajax({
                type: "POST",
                url: "frmMedExWorkspaceDtlNew.aspx/GetSubGroupName_SpecificScreening",
                data: '{"nWorkSpaceScreeningHdrNo":"' + nWorkSpaceScreeningHdrNo + '","vMedExGroupCode":"' + $("#ctl00_CPHLAMBDA_ddlScreeningGroup").val() + '","Scope":"' + Scope + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    var aaDataSet = [];
                    data = JSON.parse(data.d);
                    $("#divSubGroupPanel").html("")
                    for (var Row = 0; Row < data.length; Row++) {
                        var InDataSet = [];
                        var $input = $('<input type="button"  class="btn btn-info" data-target="#myModal"  style="width:100%; color:white; font-weight: bold; text-align:left; background-color:#4974c3;" Class="Button" onClick=DisplayAttribute_SpecificScreening("' + data[Row].nWorkSpaceScreeningHdrNo + '","' + data[Row].vMedExGroupCode + '","' + Scope + '","' + data[Row].vMedExSubGroupCode + '")  value="' + data[Row].vMedExSubGroupDesc + '"  id=btn' + data[Row].vMedExSubGroupCode + '"/>');
                        $input.appendTo($("#divSubGroupPanel"));
                        $("#divSubGroupPanel").append("</BR>")
                    }

                },
                failure: function (error) {
                    alert(error);
                    return false;

                }
            });
            onUpdated();
            return false;
        }

        function GetSubGroupName_GenericScreening(nScreeningTemplateHdrNo, Scope, StatusForFreeze) {
            ActivityName_ForEdit = "Generic Screening";
            onUpdating();
            if (document.getElementById('<%= Hdn_FreezeStatus.clientid %>').value == "F") {

             }
             else {
                 AllowToEdit = 0;
             }
             $("#ctl00_CPHLAMBDA_div1").click();
             if ($("#ctl00_CPHLAMBDA_Hdn_FreezeStatus").val() == "F") {
                 $("#tblAddAttributeGroup").attr("style", "display:none");
                 $(".SaveDeleteHide").attr("style", "display:none");
             }
             else if ($("#ctl00_CPHLAMBDA_Hdn_ProjectLock").val() == "L") {
                 $("#tblAddAttributeGroup").attr("style", "display:none");
                 $(".SaveDeleteHide").attr("style", "display:none");
             }
             else {
                 $("#tblAddAttributeGroup").attr("style", "display:");
                 $(".SaveDeleteHide").attr("style", "display:");
             }

             if (StatusForFreeze.toLowerCase() == ("Freeze").toLowerCase()) {
                 $("#tblAddAttributeGroup").attr("style", "display:none");
                 $(".SaveDeleteHide").attr("style", "display:none");
             }
             else {
                 $("#tblAddAttributeGroup").attr("style", "display:");
                 $(".SaveDeleteHide").attr("style", "display:");
             }



             $.ajax({
                 type: "POST",
                 url: "frmMedExWorkspaceDtlNew.aspx/GetSubGroupName_GenericScreening",
                 data: '{"nScreeningTemplateHdrNo":"' + nScreeningTemplateHdrNo + '","vMedExGroupCode":"' + $("#ctl00_CPHLAMBDA_ddlScreeningGroup").val() + '","Scope":"' + Scope + '"}',
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 async: false,
                 success: function (data) {
                     var aaDataSet = [];
                     data = JSON.parse(data.d);
                     $("#divSubGroupPanel").html("")
                     for (var Row = 0; Row < data.length; Row++) {
                         var InDataSet = [];
                         var $input = $('<input type="button"  class="btn btn-info" data-target="#myModal"  style="width:100%; color:white; font-weight: bold; text-align:left; background-color:#4974c3;" Class="Button" onClick=  DisplayAttribute_GenericScreening("' + data[Row].nScreeningTemplateHdrNo + '","' + $("#ctl00_CPHLAMBDA_ddlScreeningGroup").val() + '","' + Scope + '","' + data[Row].vMedExSubGroupCode + '",)  value="' + data[Row].vMedExSubGroupDesc + '"  id=btn' + data[Row].vMedExSubGroupCode + '"/>');
                         $input.appendTo($("#divSubGroupPanel"));
                         $("#divSubGroupPanel").append("</BR>")
                     }
                     onUpdated();

                 },
                 failure: function (error) {
                     msgalert(error);
                     onUpdated();
                     return false;

                 }
             });
             return false;
         }


        var temp_vMedExSubGroupCode = undefined;
        function DisplayAttribute(vWorkspaceId, vMedExSubGroupCode, Scope, iNodeID) {
            temp_vMedExSubGroupCode = vMedExSubGroupCode;
            onUpdating();
            $find('MPE_MedExSubGroupMst').show();
            AddMedExMstTemp(true);
            $("#lblActivityName").html(ActivityName_ForEdit);
            FillddlMedexGroup(Scope);
            vWorkspaceId_ForEdit = vWorkspaceId;
            Scope_ForEdit = Scope;
            iNodeID_ForEdit = iNodeID;
            vMedExSubGroupCode_ForEdit = vMedExSubGroupCode;
            FillControlData(vWorkspaceId, vMedExSubGroupCode, Scope, iNodeID);
            onUpdated();
         }

         function DisplayAttribute_SpecificScreening(nWorkSpaceScreeningHdrNo, vMedExGroupCode, Scope, vMedExSubGroupCode) {
             onUpdating();
             temp_vMedExSubGroupCode = vMedExSubGroupCode;
             $find('MPE_MedExSubGroupMst').show();
             AddMedExMstTemp(true);
             $("#lblActivityName").html(ActivityName_ForEdit);
             FillddlMedexGroup(Scope);
             Scope_ForEdit = Scope;
             vMedExSubGroupCode_ForEdit = vMedExSubGroupCode;
             nWorkspaceScreeningHdrNo_ForEdit = nWorkSpaceScreeningHdrNo;
             FillControlData_SpecificScreening(nWorkSpaceScreeningHdrNo, vMedExGroupCode, Scope, vMedExSubGroupCode);
             onUpdated();
         }
         function DisplayAttribute_GenericScreening(nScreeningTemplateHdrNo, vMedExGroupCode, Scope, vMedExSubGroupCode) {
             temp_vMedExSubGroupCode = vMedExSubGroupCode;
             onUpdating();
             $find('MPE_MedExSubGroupMst').show();
             AddMedExMstTemp(true);
             $("#lblActivityName").html(ActivityName_ForEdit);
             FillddlMedexGroup(Scope);
             Scope_ForEdit = Scope;
             vMedExSubGroupCode_ForEdit = vMedExSubGroupCode;
             nScreeningTemplateHdrNo_ForEdit = nScreeningTemplateHdrNo;
             FillControlData_GenericScreening(nScreeningTemplateHdrNo, vMedExGroupCode, Scope, vMedExSubGroupCode);
             onUpdated();
         }


         var vWorkspaceId_ForEdit = undefined;
         var Scope_ForEdit = undefined;
         var iNodeID_ForEdit = undefined;
         var nMedExWorkSpaceHdrNo_ForEdit = undefined;
         var vMedExSubGroupCode_ForEdit = undefined;
         var ActivityName_ForEdit = undefined;
         var nWorkspaceScreeningHdrNo_ForEdit = undefined;
         var nScreeningTemplateHdrNo_ForEdit = undefined;

         function FillddlMedexGroup(Scope) {
             $.ajax({
                 type: "POST",
                 url: "frmMedExWorkspaceDtlNew.aspx/GetMedExGroupData",
                 data: '{"Scope":"' + Scope + '"}',
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 async: false,
                 success: function (data) {
                     var aaDataSet = [];
                     data = JSON.parse(data.d);
                     var select = $('#ddlAttributeGroup')
                     select.html("")
                     select.append('<option value="0">' + "Select Attribute Group " + '</option>')
                     for (var Row = 0; Row < data.length; Row++) {
                         var InDataSet = [];
                         select.append('<option value="' + data[Row].vMedExGroupCode + '">' + data[Row].vMedExGroupDesc + '</option>')
                     }

                 },
                 failure: function (error) {
                     msgalert(error);
                 }
             });
         }

         function GetAttributeName() {
             var selectedVal = $('#ddlAttributeGroup option:selected').attr('value');

             $.ajax({
                 type: "POST",
                 url: "frmMedExWorkspaceDtlNew.aspx/GetAttributeGroupWise",
                 data: '{"GroupId":"' + selectedVal + '" , "vMedExSubGroupCode":"' + temp_vMedExSubGroupCode + '"}',
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 async: false,
                 success: function (data) {
                     var aaDataSet = [];
                     data = JSON.parse(data.d);
                     var select = $('#ddlAttribute')
                     select.html("")
                     select.append('<option value="' + 0 + '">' + "Select Attribute " + '</option>')
                     for (var Row = 0; Row < data.length; Row++) {
                         var InDataSet = [];
                         var tooltipvalue = " Attribute Sub Group :" + data[Row].vMedExSubGroupDesc + " , Attribute Code :" + data[Row].vMedExCode + " , Attribute Type :" + data[Row].vMedExType;
                         select.append('<option value="' + data[Row].vMedExCode + '"  title="'+tooltipvalue+'" >' + data[Row].vMedExDesc + '</option>')
                     }
                 },
                 failure: function (error) {
                     msgalert(error);
                 }
             });
         }

         var UOMData = undefined;
         function getUOM(UOM) {
             var select = $('<select class="UOM" style="Width:70px;"></select>')
             var Scope = ""
             select.html = ""

             if (UOMData != undefined) {
                 select.append('<option value="">' + "Select UOM " + '</option>')
                 for (var Row1 = 0; Row1 < UOMData.length; Row1++) {
                     if (UOMData[Row1].vUOMDesc.toString() == UOM) {
                         select.append('<option Selected="Selected" value="' + UOMData[Row1].vUOMDesc + '">' + UOMData[Row1].vUOMDesc + '</option>')
                     }
                     else {
                         select.append('<option value="' + UOMData[Row1].vUOMDesc + '">' + UOMData[Row1].vUOMDesc + '</option>')
                     }
                 }
                 return select;
             }

             $.ajax({
                 type: "POST",
                 url: "frmMedExWorkspaceDtlNew.aspx/GetUOM  ",
                 data: '{"Scope":"' + Scope + '"}',
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 async: false,
                 success: function (data123) {
                     var aaDataSet = [];
                     data123 = JSON.parse(data123.d);
                     UOMData = data123;
                     select.append('<option value="">' + "Select UOM " + '</option>')

                     for (var Row1 = 0; Row1 < data123.length; Row1++) {
                         if (data123[Row1].vUOMDesc.toString() == UOM) {
                             select.append('<option Selected="Selected" value="' + data123[Row1].vUOMDesc + '">' + data123[Row1].vUOMDesc + '</option>')
                         }
                         else {
                             select.append('<option value="' + data123[Row1].vUOMDesc + '">' + data123[Row1].vUOMDesc + '</option>')
                         }
                     }
                 },
                 failure: function (error) {
                     msgalert(error);
                 }
             });
             return select;
         }

         function getMeddraTable(nRefMasterNo) {
             var select = $('<select class="vMedDraTable" style="Width:70px;"></select>')
             var Scope = ""
             select.html = ""
             $.ajax({
                 type: "POST",
                 url: "frmMedExWorkspaceDtlNew.aspx/GetMedDraTable  ",
                 data: '{"Scope":"' + Scope + '"}',
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 async: false,
                 success: function (data123) {
                     var aaDataSet = [];
                     data123 = JSON.parse(data123.d);
                     select.append('<option value="">' + "Select UOM " + '</option>')

                     for (var Row1 = 0; Row1 < data123.length; Row1++) {
                         if (data123[Row1].nRefMasterNo.toString() == nRefMasterNo) {
                             select.append('<option Selected="Selected" value="' + data123[Row1].nRefMasterNo + '">' + data123[Row1].vRefTableName + '</option>')
                         }
                         else {
                             select.append('<option value="' + data123[Row1].nRefMasterNo + '">' + data123[Row1].vRefTableName + '</option>')
                         }
                     }
                 },
                 failure: function (error) {
                     msgalert(error);
                 }
             });
             return select;
         }

         function FillControlData(vWorkspaceId, vMedExSubGroupCode, Scope, iNodeId) {
             $.ajax({
                 type: "POST",
                 url: "frmMedExWorkspaceDtlNew.aspx/GetViewMedExWorkSpaceDtl ",
                 data: '{"vWorkspaceId":"' + vWorkspaceId + '","iNodeId":"' + iNodeId + '","Scope":"' + Scope + '","vMedExSubGroupCode":"' + vMedExSubGroupCode + '"}',
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 async: false,
                 success: function (data) {
                     var aaDataSet = [];
                     data = JSON.parse(data.d);

                     nMedExWorkSpaceHdrNo_ForEdit = data[0].nMedExWorkSpaceHdrNo;
                     tblMedExMstArray = {};
                     for (var Row = 0; Row < data.length; Row++) {
                         var InDataSet = [];
                         var iLength = data[Row].vValidationType.toString().split(",")[1];
                         if (iLength == undefined) {
                             iLength = "";
                         }
                         debugger;
                         tblMedExMstArray[data[Row].vMedExCode] = data[Row].vMedExGroupCode;
                         InDataSet.push("", "", data[Row].vMedExDesc, data[Row].vMedExType, data[Row].vMedExValues, data[Row].vDefaultValue, data[Row].vAlertonvalue,
                             data[Row].vAlertMessage, data[Row].vLowRange, data[Row].vHighRange, data[Row].vUOM, data[Row].vValidationType,
                             iLength, data[Row].cAlertType, data[Row].vCDISCValues, data[Row].vOtherValues, data[Row].vRefTable,
                             data[Row].vMedExCode,
                             data[Row].nMedExWorkSpaceDtlNo, data[Row].vMedExGroupDesc, data[Row].vMedExSubGroupDesc
                             );
                         aaDataSet.push(InDataSet);
                     }

                     if ($("#tblMedExMst").children().length > 0) {
                         $("#tblMedExMst").dataTable().fnDestroy();
                     }
                     BindtblMedExMst(aaDataSet);
                 },
                 failure: function (error) {
                     msgalert(error);
                 }
             });
         }

         function FillControlData_SpecificScreening(nWorkspaceScreeningHdrNo, vMedExGroupCode, Scope, vMedExSubGroupCode) {

             $.ajax({
                 type: "POST",
                 url: "frmMedExWorkspaceDtlNew.aspx/GetView_WorkspaceScreeningHdrDtl ",
                 data: '{"nWorkspaceScreeningHdrNo":"' + nWorkspaceScreeningHdrNo + '","vMedExGroupCode":"' + vMedExGroupCode + '","Scope":"' + Scope + '" ,"vMedExSubGroupCode":"' + vMedExSubGroupCode + '" }',
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 async: false,
                 success: function (data) {
                     var aaDataSet = [];
                     data = JSON.parse(data.d);

                     nMedExWorkSpaceHdrNo_ForEdit = data[0].nMedExWorkSpaceHdrNo;
                     tblMedExMstArray = {};
                     for (var Row = 0; Row < data.length; Row++) {
                         var InDataSet = [];
                         var iLength = data[Row].vValidationType.toString().split(",")[1];
                         if (iLength == undefined) {
                             iLength = "";
                         }
                         debugger;
                         tblMedExMstArray[data[Row].vMedExCode] = data[Row].vMedExGroupCode;
                         InDataSet.push("", "", data[Row].vMedExDesc, data[Row].vMedExType, data[Row].vMedExValues, data[Row].vDefaultValue, data[Row].vAlertonvalue,
                             data[Row].vAlertMessage, data[Row].vLowRange, data[Row].vHighRange, data[Row].vUOM, data[Row].vValidationType,
                             iLength, data[Row].cAlertType, data[Row].vCDISCValues, data[Row].vOtherValues, data[Row].vRefTable,
                             data[Row].vMedExCode,
                             data[Row].nWorkspaceScreeningDtlNo, data[Row].vMedExGroupDesc, data[Row].vMedExSubGroupDesc
                             );
                         aaDataSet.push(InDataSet);
                     }

                     if ($("#tblMedExMst").children().length > 0) {
                         $("#tblMedExMst").dataTable().fnDestroy();
                     }
                     BindtblMedExMst(aaDataSet);
                 },
                 failure: function (error) {
                     msgalert(error);
                 }
             });
         }

         function FillControlData_GenericScreening(nScreeningTemplateHdrNo, vMedExGroupCode, Scope, vMedExSubGroupCode) {

             $.ajax({
                 type: "POST",
                 url: "frmMedExWorkspaceDtlNew.aspx/GetView_GeneralScreeningHdrDtl",
                 data: '{"nScreeningTemplateHdrNo":"' + nScreeningTemplateHdrNo + '","vMedExGroupCode":"' + vMedExGroupCode + '","Scope":"' + Scope + '","vMedExSubGroupCode":"' + vMedExSubGroupCode + '"}',
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 async: false,
                 success: function (data) {
                     var aaDataSet = [];
                     data = JSON.parse(data.d);

                     nMedExWorkSpaceHdrNo_ForEdit = data[0].nMedExWorkSpaceHdrNo;
                     tblMedExMstArray = {};
                     for (var Row = 0; Row < data.length; Row++) {
                         var InDataSet = [];
                         var iLength = data[Row].vValidationType.toString().split(",")[1];
                         if (iLength == undefined) {
                             iLength = "";
                         }
                         debugger;
                         tblMedExMstArray[data[Row].vMedExCode] = data[Row].vMedExGroupCode;
                         InDataSet.push("", "", data[Row].vMedExDesc, data[Row].vMedExType, data[Row].vMedExValues, data[Row].vDefaultValue, data[Row].vAlertonvalue,
                             data[Row].vAlertMessage, data[Row].vLowRange, data[Row].vHighRange, data[Row].vUOM, data[Row].vValidationType,
                             iLength, data[Row].cAlertType, data[Row].vCDISCValues, data[Row].vOtherValues, data[Row].vRefTable,
                             data[Row].vMedExCode,
                             data[Row].nScreeningTemplateDtlNo, data[Row].vMedExGroupDesc, data[Row].vMedExSubGroupDesc
                             );
                         aaDataSet.push(InDataSet);
                     }

                     if ($("#tblMedExMst").children().length > 0) {
                         $("#tblMedExMst").dataTable().fnDestroy();
                     }
                     BindtblMedExMst(aaDataSet);
                 },
                 failure: function (error) {
                     msgalert(error);
                 }
             });
         }


         function BindtblMedExMst(aaDataSet) {
             var select_Index = 0;
             var Detail_Index = 1;
             var vMedExDesc_Index = 2;
             var vMedExType_Index = 3;
             var vMedExValues_Index = 4;
             var vDefaultValue_Index = 5;
             var vAlertonvalue_Index = 6;
             var vAlertMessage_Index = 7;
             var vLowRange_Index = 8;
             var vHighRange_Index = 9;
             var vUOM_Index = 10;
             var vValidationType_Index = 11;
             var iLength_Index = 12;
             var cAlertType_Index = 13;
             var vCDISCValues_Index = 14;
             var vOtherValues_Index = 15;
             var vRefTable_Index = 16;
             var vMedExCode_Index = 17;
             var nMedExWorkSpaceDtlNo_Index = 18;
             var vMedExGroupDesc_Index = 19;
             var vMedExSubGroupDesc_Index = 20;

             var int = 0;

             $('#tblMedExMst').prepend($('<thead>').append($('#tblMedExMst tr:first'))).dataTable({
                 "bJQueryUI": true,
                 "sPaginationType": "full_numbers",
                 "bLengthChange": true,
                 "iDisplayLength": -1,
                 "bProcessing": true,
                 "bSort": false,
                 "aaData": aaDataSet,
                 aLengthMenu: [
                    [5, 25, 50, -1],
                    [5, 25, 50, "All"]
                 ],
                 "fnCreatedRow": function (nRow, aData, iDataIndex) {
                     int += 1;
                     var ControlId = aData[nMedExWorkSpaceDtlNo_Index];

                     $('td:eq(' + select_Index + ')', nRow).html("<input type='CheckBox' style='Width:40px;' id=chkSelect_" + ControlId + " />");
                     $('td:eq(' + Detail_Index + ')', nRow).html('<img id=imgdetail_' + ControlId + ' title="Attribute Group : ' + aData[vMedExGroupDesc_Index] + ',Attribute SubGroup : ' + aData[vMedExSubGroupDesc_Index] + ',Attribute Code : ' + aData[vMedExCode_Index] + '" src="images/attributedetails.png" alt="Details" style="border-width:0px;">');
                     $('td:eq(' + vMedExDesc_Index + ')', nRow).html("<TextArea class='textBox' onchange='SetDefault($(this),$(this).val());' style='Width:170px;' id=txtAttributeDesc_" + ControlId + " >" + aData[vMedExDesc_Index].trimStart().trimEnd().trim() + "</TextArea>");

                     if ($("[id*=RBLProjecttype] input:checked").val() == 1) {
                         var select = $('<select style="Width:70px;"><option value="" ></option></select>');
                         select.append('<option value="' + "AsyncDateTime" + '">' + 'AsyncDateTime' + '</option>');
                         select.append('<option value="' + "AsyncTime" + '">' + 'AsyncTime' + '</option>');
                         select.append('<option value="' + "CheckBox" + '">' + 'CheckBox' + '</option>');
                         select.append('<option value="' + "ComboBox" + '">' + 'ComboBox' + '</option>');
                         select.append('<option value="' + "ComboGlobalDictionary" + '">' + 'ComboGlobalDictionary' + '</option>');
                         select.append('<option value="' + "CrfTerm" + '">' + 'CrfTerm' + '</option>');
                         select.append('<option value="' + "DateTime" + '">' + 'DateTime' + '</option>');
                         select.append('<option value="' + "File" + '">' + 'File' + '</option>');
                         select.append('<option value="' + "Formula" + '">' + 'Formula' + '</option>');
                         select.append('<option value="' + "Import" + '">' + 'Import' + '</option>');
                         select.append('<option value="' + "Label" + '">' + 'Label' + '</option>');
                         select.append('<option value="' + "Radio" + '">' + 'Radio' + '</option>');
                         select.append('<option value="' + "STANDARDDATE" + '">' + 'STANDARDDATE' + '</option>');
                         select.append('<option value="' + "TextArea" + '">' + 'TextArea' + '</option>');
                         select.append('<option value="' + "TextBox" + '">' + 'TextBox' + '</option>');
                         select.append('<option value="' + "Time" + '">' + 'Time' + '</option>');
                         select.append('<option value="' + aData[vMedExType_Index] + '" selected="selected">' + aData[vMedExType_Index] + '</option>');
                         $('td:eq(' + vMedExType_Index + ')', nRow).html(select);
                     }
                     else {
                         $('td:eq(' + vMedExValues_Index + ')', nRow).html("<label   id=lblAttributetype_" + ControlId + "  >" + aData[vMedExValues_Index].toString().trimEnd().trimStart() + " </label>");
                     }

                     $('td:eq(' + vMedExValues_Index + ')', nRow).html("<TextArea class='textBox' onchange='SetDefault($(this),$(this).val());'  style='Width:70px;' id=txtAttributeValue_" + ControlId + "  >" + aData[vMedExValues_Index].toString().trimEnd().trimStart() + " </TextArea>");
                     $('td:eq(' + vDefaultValue_Index + ')', nRow).html("<TextArea class='textBox' onchange='SetDefault($(this),$(this).val());' style='Width:70px;' id=txtDefaultValue_" + ControlId + "  >" + aData[vDefaultValue_Index].toString().trimEnd().trimStart() + " </TextArea>");
                     $('td:eq(' + vAlertonvalue_Index + ')', nRow).html("<TextArea class='textBox' onchange='SetDefault($(this),$(this).val());' style='Width:70px;' id=txtAlertOn_" + ControlId + "" + "  >" + aData[vAlertonvalue_Index].toString().trimEnd().trimStart() + " </TextArea>");
                     $('td:eq(' + vAlertMessage_Index + ')', nRow).html("<TextArea class='textBox' onchange='SetDefault($(this),$(this).val());' style='Width:70px;' id=txtAlertMessage_" + ControlId + "  >" + aData[vAlertMessage_Index].toString().trimEnd().trimStart() + " </TextArea>");
                     $('td:eq(' + vLowRange_Index + ')', nRow).html("<TextArea class='textBox' onchange='SetDefault($(this),$(this).val());' style='Width:30px;' id=txtLowRange_" + ControlId + "  >" + aData[vLowRange_Index].toString().trimEnd().trimStart() + " </TextArea>");
                     $('td:eq(' + vHighRange_Index + ')', nRow).html("<TextArea class='textBox' onchange='SetDefault($(this),$(this).val());'  style='Width:30px;' id=txtHighRange_" + ControlId + "  >" + aData[vHighRange_Index].toString().trimEnd().trimStart() + " </TextArea>");

                     $('td:eq(' + vUOM_Index + ')', nRow).html(getUOM(aData[vUOM_Index]));

                     var select = $('<select style="Width:70px;"  id=ddlValidationType_' + ControlId + ' ><option value=""  ></option></select>')
                     if (aData[vValidationType_Index].split(",")[0] == 'NA' || aData[vValidationType_Index].split(",") == "") {
                         select.append('<option  selected="selected" value="' + "NA" + '">' + 'Not Applicable' + '</option>')
                     }
                     else {
                         select.append('<option  " value="' + "NA" + '">' + 'Not Applicable' + '</option>')
                     }
                     if (aData[vValidationType_Index].split(",")[0] == 'AN') {
                         select.append('<option selected="selected"  value="' + "AN" + '">' + 'Alpha Numeric' + '</option>')
                     }
                     else {
                         select.append('<option   value="' + "AN" + '">' + 'Alpha Numeric' + '</option>')
                     }

                     if (aData[vValidationType_Index].split(",")[0] == 'NU') {
                         select.append('<option selected="selected"  value="' + "NU" + '">' + 'Numeric' + '</option>')
                     }
                     else {
                         select.append('<option   value="' + "NU" + '">' + 'Numeric' + '</option>')
                     }
                     if (aData[vValidationType_Index].split(",")[0] == 'IN') {
                         select.append('<option selected="selected"  value="' + "IN" + '">' + 'Integer' + '</option>')
                     }
                     else {
                         select.append('<option  value="' + "IN" + '">' + 'Integer' + '</option>')
                     }
                     if (aData[vValidationType_Index].split(",")[0] == 'AL') {
                         select.append('<option selected="selected"  value="' + "AL" + '">' + 'Alphabate' + '</option>')
                     }
                     else {
                         select.append('<option  value="' + "AL" + '">' + 'Alphabate' + '</option>')
                     }

                     $('td:eq(' + vValidationType_Index + ')', nRow).html(select)

                     $('td:eq(' + iLength_Index + ')', nRow).html("<TextArea class='textBox' style='Width:70px;' onchange='SetDefault($(this),$(this).val());' id=txtLength_" + ControlId + " onkeypress='return ValidateNumericCode(event);'  >" + aData[iLength_Index] + " </TextArea>");

                     if (aData[cAlertType_Index] == 'N') {
                         $('td:eq(' + cAlertType_Index + ')', nRow).html("<input  type='CheckBox'  id=chkIsNull_" + ControlId + " style='Width:40px;'  />");
                     }
                     else {
                         $('td:eq(' + cAlertType_Index + ')', nRow).html("<input type='CheckBox' id=chkIsNull_" + ControlId + " checked='checked' style='Width:40px;'/>");
                     }

                     $('td:eq(' + vCDISCValues_Index + ')', nRow).html("<TextArea class='textBox' onchange='SetDefault($(this),$(this).val());' style='Width:70px;' id=txtVarialbleName_" + ControlId + "  >" + aData[vCDISCValues_Index].toString().trimEnd().trimStart() + " </TextArea>");
                     $('td:eq(' + vOtherValues_Index + ')', nRow).html("<TextArea class='textBox' onchange='SetDefault($(this),$(this).val());' style='Width:70px;' id=txtOther_" + ControlId + "  >" + aData[vOtherValues_Index].toString().trimEnd().trimStart() + " </TextArea>");

                     if (aData[vMedExType_Index].toString().toLowerCase() == ("ComboGlobalDictionary").toLowerCase()) {
                         $('td:eq(' + vRefTable_Index + ')', nRow).html(getMeddraTable(aData[vRefTable_Index]));
                     }
                 },

                 "aoColumns": [
                                   { "sTitle": "Select" },
                                   { "sTitle": "Details" },
                                   { "sTitle": "Attribute Description" },
                                   { "sTitle": "Attribute Type" },
                                   { "sTitle": "Attribute Value" },
                                   { "sTitle": "Default Value" },
                                   { "sTitle": "Alert On" },
                                   { "sTitle": "Alert Message" },
                                   { "sTitle": "Low Range" },
                                   { "sTitle": "High Range" },
                                   { "sTitle": "UOM" },
                                   { "sTitle": "Validation Type" },
                                   { "sTitle": "Length" },
                                   { "sTitle": "Not Null" },
                                   { "sTitle": "Variable Name" },
                                   { "sTitle": "Other" },
                                   { "sTitle": "Dictionary" },

                 ],

             });
         }

         function SetDefault(id, value) {
             var ID1 = id.context.id
             $("textarea#" + ID1).html(value)
         }

         function SaveEditedData() {
             if ($("[id*=RBLProjecttype] input:checked").val() == 1) {
                 Save_MedExWorkSpaceDtlForEdited();
             }
             else if ($("[id*=RBLProjecttype] input:checked").val() == 2) {
                 Save_WorkspaceScreeningDtlForEdited();
             }
             else if ($("[id*=RBLProjecttype] input:checked").val() == "0000000000") {
                 Save_ScreeningTemplateDtlForEdited();
             }
         }

         function Save_MedExWorkSpaceDtlForEdited() {
             var select_Index = 0;
             var Detail_Index = 1;
             var vMedExDesc_Index = 2;
             var vMedExType_Index = 3;
             var vMedExValues_Index = 4;
             var vDefaultValue_Index = 5;
             var vAlertonvalue_Index = 6;
             var vAlertMessage_Index = 7;
             var vLowRange_Index = 8;
             var vHighRange_Index = 9;
             var vUOM_Index = 10;
             var vValidationType_Index = 11;
             var iLength_Index = 12;
             var cAlertType_Index = 13;
             var vCDISCValues_Index = 14;
             var vOtherValues_Index = 15;
             var vRefTable_Index = 16;
             var vMedExCode_Index = 17;
             var nMedExWorkSpaceDtlNo_Index = 18;
             var vMedExGroupDesc_Index = 19;
             var vMedExSubGroupDesc_Index = 20;

             var Quantity
             var objSaveData = [];
             var table = $("#tblMedExMst  tbody");
             var dataTable = $('#tblMedExMst').dataTable()
             var int = 1
             var vRefTableid = "";
             $(dataTable.fnGetNodes()).each(function () {
                 var chkcAlertType = ""
                 var $tds = $(this).find('td')
                 if ($tds.eq(cAlertType_Index).find("input[type='checkbox']:checked").length > 0) {
                     chkcAlertType = 'Y'
                 }
                 else {
                     chkcAlertType = 'N'
                 }

                 if ($tds.eq(vMedExType_Index).find("select").val().trim().toLowerCase() == ("ComboGlobalDictionary").toLowerCase()) {
                     vRefTableid = $tds.eq(vRefTable_Index).find("select").val().trim();
                 }
                 else {
                     vRefTableid = "";
                 }

                 objSaveData.push({
                     vMedExDesc: $tds.eq(vMedExDesc_Index).text().trim(),
                     vMedExType: $tds.eq(vMedExType_Index).find("select").val().trim(),
                     vMedExValues: $tds.eq(vMedExValues_Index).text().trim(),
                     vDefaultValue: $tds.eq(vDefaultValue_Index).text().trim(),
                     vAlertonvalue: $tds.eq(vAlertonvalue_Index).text().trim(),
                     vAlertMessage: $tds.eq(vAlertMessage_Index).text().trim(),
                     vLowRange: $tds.eq(vLowRange_Index).text().trim(),
                     vHighRange: $tds.eq(vHighRange_Index).text().trim(),
                     vUOM: $tds.eq(vUOM_Index).find("select").val().trim(),
                     vValidationType: $tds.eq(vValidationType_Index).find("select").val().trim(),
                     cAlertType: chkcAlertType,
                     iLength: $tds.eq(iLength_Index).text().trim(),
                     vCDISCValues: $tds.eq(vCDISCValues_Index).text().trim(),
                     vOtherValues: $tds.eq(vOtherValues_Index).text().trim(),
                     vMedExCode: $tds.eq(vMedExCode_Index).text().trim(),
                     nMedExWorkSpaceDtlNo: $tds.eq(vMedExDesc_Index).find("textarea")[0].id.split("_")[1],
                     vWorkspaceId: vWorkspaceId_ForEdit,
                     Scope: Scope_ForEdit,
                     iNodeId: iNodeID_ForEdit,
                     iModifyBy: '<%= Me.Session(S_UserID)%>',
                    nMedExWorkSpaceHdrNo: nMedExWorkSpaceHdrNo_ForEdit,
                    vMedExSubGroupCode: vMedExSubGroupCode_ForEdit,
                    vRefTable: vRefTableid,


                })
            })
            var JSONObject = JSON.stringify(objSaveData);


            $.ajax({
                type: "POST",
                url: "frmMedExWorkspaceDtlNew.aspx/Save_MedExWorkSpaceDtl",
                dataType: "json",
                data: "{objMedExWorkSpaceDtl :" + JSONObject + "}",
                contentType: 'application/json; charset=utf-8',
                async: false,
                success: function (data123) {
                    data123 = data123.d
                    if (data123 == "True") {
                        msgalert("Data Saved Successfully");
                        $find('MPE_MedExSubGroupMst').hide();

                    }
                    else {
                        msgalert("Error While Saving Attribute Details In MedexTemplateDtl");
                    }

                },
                failure: function (error) {
                    msgalert(error);
                }
            });
        }

        function Save_WorkspaceScreeningDtlForEdited() {
            var select_Index = 0;
            var Detail_Index = 1;
            var vMedExDesc_Index = 2;
            var vMedExType_Index = 3;
            var vMedExValues_Index = 4;
            var vDefaultValue_Index = 5;
            var vAlertonvalue_Index = 6;
            var vAlertMessage_Index = 7;
            var vLowRange_Index = 8;
            var vHighRange_Index = 9;
            var vUOM_Index = 10;
            var vValidationType_Index = 11;
            var iLength_Index = 12;
            var cAlertType_Index = 13;
            var vCDISCValues_Index = 14;
            var vOtherValues_Index = 15;
            var vRefTable_Index = 16;
            var vMedExCode_Index = 17;
            var nMedExWorkSpaceDtlNo_Index = 18;
            var vMedExGroupDesc_Index = 19;
            var vMedExSubGroupDesc_Index = 20;

            var Quantity
            var objSaveData = [];
            var table = $("#tblMedExMst  tbody");
            var dataTable = $('#tblMedExMst').dataTable()
            var int = 1
            var vRefTableid = "";
            $(dataTable.fnGetNodes()).each(function () {
                var chkcAlertType = ""
                var $tds = $(this).find('td')
                if ($tds.eq(cAlertType_Index).find("input[type='checkbox']:checked").length > 0) {
                    chkcAlertType = 'Y'
                }
                else {
                    chkcAlertType = 'N'
                }

                if ($tds.eq(vMedExType_Index).text().toLowerCase() == ("ComboGlobalDictionary").toLowerCase()) {
                    vRefTableid = $tds.eq(vRefTable_Index).find("select").val().trim();
                }
                else {
                    vRefTableid = "";
                }

                objSaveData.push({
                    vMedExDesc: $tds.eq(vMedExDesc_Index).text().trim(),
                    //vMedExType: $tds.eq(vMedExType_Index).find("select").val().trim(),
                    vMedExValues: $tds.eq(vMedExValues_Index).text().trim(),
                    vDefaultValue: $tds.eq(vDefaultValue_Index).text().trim(),
                    vAlertonvalue: $tds.eq(vAlertonvalue_Index).text().trim(),
                    vAlertMessage: $tds.eq(vAlertMessage_Index).text().trim(),
                    vLowRange: $tds.eq(vLowRange_Index).text().trim(),
                    vHighRange: $tds.eq(vHighRange_Index).text().trim(),
                    vUOM: $tds.eq(vUOM_Index).find("select").val().trim(),
                    vValidationType: $tds.eq(vValidationType_Index).find("select").val().trim(),
                    cAlertType: chkcAlertType,
                    iLength: $tds.eq(iLength_Index).text().trim(),
                    vCDISCValues: $tds.eq(vCDISCValues_Index).text().trim(),
                    vOtherValues: $tds.eq(vOtherValues_Index).text().trim(),
                    //vMedExCode: $tds.eq(vMedExCode_Index).text().trim(),
                    nWorkspaceScreeningDtlNo: $tds.eq(vMedExDesc_Index).find("textarea")[0].id.split("_")[1],
                    vWorkspaceId: vWorkspaceId_ForEdit,
                    Scope: Scope_ForEdit,
                    iNodeId: iNodeID_ForEdit,
                    iModifyBy: '<%= Me.Session(S_UserID)%>',
                    nWorkspaceScreeningHdrNo: nWorkspaceScreeningHdrNo_ForEdit,
                    vMedExGroupCode: $("#ctl00_CPHLAMBDA_ddlScreeningGroup").val(),
                    vMedExSubGroupCode: vMedExSubGroupCode_ForEdit,
                    vRefTable: vRefTableid,



                })
            })
            var JSONObject = JSON.stringify(objSaveData);

            $.ajax({
                type: "POST",
                url: "frmMedExWorkspaceDtlNew.aspx/Save_WorkspaceScreeningDtl",
                dataType: "json",
                data: "{objWorkspaceScreeningDtl :" + JSONObject + "}",
                contentType: 'application/json; charset=utf-8',
                async: false,
                success: function (data123) {
                    data123 = data123.d
                    if (data123 == "True") {
                        msgalert("Data Saved Successfully");
                        $find('MPE_MedExSubGroupMst').hide();

                    }
                    else {
                        msgalert("Error While Saving Attribute Details In MedexTemplateDtl");
                    }

                },
                failure: function (error) {
                    msgalert(error);
                }
            });
        }

        function Save_ScreeningTemplateDtlForEdited() {
            var select_Index = 0;
            var Detail_Index = 1;
            var vMedExDesc_Index = 2;
            var vMedExType_Index = 3;
            var vMedExValues_Index = 4;
            var vDefaultValue_Index = 5;
            var vAlertonvalue_Index = 6;
            var vAlertMessage_Index = 7;
            var vLowRange_Index = 8;
            var vHighRange_Index = 9;
            var vUOM_Index = 10;
            var vValidationType_Index = 11;
            var iLength_Index = 12;
            var cAlertType_Index = 13;
            var vCDISCValues_Index = 14;
            var vOtherValues_Index = 15;
            var vRefTable_Index = 16;
            var vMedExCode_Index = 17;
            var nMedExWorkSpaceDtlNo_Index = 18;
            var vMedExGroupDesc_Index = 19;
            var vMedExSubGroupDesc_Index = 20;

            var Quantity
            var objSaveData = [];
            var table = $("#tblMedExMst  tbody");
            var dataTable = $('#tblMedExMst').dataTable()
            var int = 1
            var vRefTableid = "";
            $(dataTable.fnGetNodes()).each(function () {
                var chkcAlertType = ""
                var $tds = $(this).find('td')
                if ($tds.eq(cAlertType_Index).find("input[type='checkbox']:checked").length > 0) {
                    chkcAlertType = 'Y'
                }
                else {
                    chkcAlertType = 'N'
                }

                if ($tds.eq(vMedExType_Index).text().toLowerCase() == ("ComboGlobalDictionary").toLowerCase()) {
                    vRefTableid = $tds.eq(vRefTable_Index).find("select").val().trim();
                }
                else {
                    vRefTableid = "";
                }

                objSaveData.push({
                    vMedExDesc: $tds.eq(vMedExDesc_Index).text().trim(),
                    vMedExValues: $tds.eq(vMedExValues_Index).text().trim(),
                    vDefaultValue: $tds.eq(vDefaultValue_Index).text().trim(),
                    vAlertonvalue: $tds.eq(vAlertonvalue_Index).text().trim(),
                    vAlertMessage: $tds.eq(vAlertMessage_Index).text().trim(),
                    vLowRange: $tds.eq(vLowRange_Index).text().trim(),
                    vHighRange: $tds.eq(vHighRange_Index).text().trim(),
                    vUOM: $tds.eq(vUOM_Index).find("select").val().trim(),
                    vValidationType: $tds.eq(vValidationType_Index).find("select").val().trim(),
                    cAlertType: chkcAlertType,
                    iLength: $tds.eq(iLength_Index).text().trim(),
                    vCDISCValues: $tds.eq(vCDISCValues_Index).text().trim(),
                    vOtherValues: $tds.eq(vOtherValues_Index).text().trim(),
                    nScreeningTemplateDtlNo: $tds.eq(vMedExDesc_Index).find("textarea")[0].id.split("_")[1],
                    vWorkspaceId: vWorkspaceId_ForEdit,
                    Scope: Scope_ForEdit,
                    iNodeId: iNodeID_ForEdit,
                    iModifyBy: '<%= Me.Session(S_UserID)%>',
                    nScreeningTemplateHdrNo: nScreeningTemplateHdrNo_ForEdit,
                    vMedExGroupCode: $("#ctl00_CPHLAMBDA_ddlScreeningGroup").val(),
                    vMedExSubGroupCode: vMedExSubGroupCode_ForEdit,
                    vRefTable: vRefTableid,


                })
            })
            var JSONObject = JSON.stringify(objSaveData);

            $.ajax({
                type: "POST",
                url: "frmMedExWorkspaceDtlNew.aspx/Save_ScreeningTemplateDtl",
                dataType: "json",
                data: "{objScreeningTemplateDtl :" + JSONObject + "}",
                contentType: 'application/json; charset=utf-8',
                async: false,
                success: function (data123) {
                    data123 = data123.d
                    if (data123 == "True") {
                        msgalert("Data Saved Successfully");
                        $find('MPE_MedExSubGroupMst').hide();

                    }
                    else {
                        msgalert("Error While Saving Attribute Details In MedexTemplateDtl");
                    }

                },
                failure: function (error) {
                    msgalert(error);
                }
            });
        }
        var tblMedExMstArray = {};
        var tblMedExMstTempArray = {};
        function AddAttributeData() {
            if ($("#ddlAttributeGroup option:selected").index() <= 0) {
                msgalert("Please Select Attribute Group");
                return false;
            }
            if ($("#ddlAttribute option:selected").index() <= 0) {
                msgalert("Please Select Attribute");
                return false;
            }
            else {
                var element = $("#ddlAttribute").val() in tblMedExMstArray;
                    if (element) {
                        msgalert("Selected Attribute Is Already Added  !");
                        return false;
                    }
            }
            AddMedExMstTemp(false);
        }
        function AddMedExMstTemp(flag) {
            if (flag) {
                if (Object.keys(tblMedExMstTempArray).length > 0) {
                    $("#tblMedExMstTemp").DataTable().clear().draw();
                    tblMedExMstTempArray = {};
                }
                $("#tblMedExMstTemp").dataTable().fnDestroy(); 
                $('#tblMedExMstTemp').DataTable({
                    "bJQueryUI": false, "bFilter": false,
                    "bInfo": false, "bPaginate": false,
                    "fnCreatedRow": function (nRow, aData, iDataIndex) {
                        $('td:eq(0)', nRow).eq(0).hide();
                        $('td:eq(2)', nRow).eq(0).hide();
                        $('td:eq(4)', nRow).html("<input type='image' src='Images/Delete.png' id=" + aData[2] + " />").click(function () {
                            $('#tblMedExMstTemp').DataTable().row($(this).parents('tr')).remove().draw();
                            delete tblMedExMstTempArray[$(this).find("input")[0].id];
                            return false;
                        });
                    }
                });
                $('#tblMedExMstTemp tr').each(function () {
                    $(this).find('th').eq(0).hide();
                    $(this).find('th').eq(2).hide();
                });
            }
            else {
                if (!($("#ddlAttribute").val() in tblMedExMstTempArray)) {
                    tblMedExMstTempArray[$("#ddlAttribute").val()] = $("#ddlAttributeGroup").val();
                    $('#tblMedExMstTemp').DataTable().row.add([
                        $("#ddlAttributeGroup").val(),
                        $("#ddlAttributeGroup :selected").text(),
                        $("#ddlAttribute").val(),
                        $("#ddlAttribute :selected").text(),
                        'delete'
                    ]).draw(false);
                }
                else { msgalert("Selected Attribute Is Already Added  !"); }
            }
        }
        function SaveAttributeData() {
            if (Object.keys(tblMedExMstTempArray).length > 0) {
                if ($("[id*=RBLProjecttype] input:checked").val() == 1) {
                    SaveAttributeDataForProjectSpecific();
                }
                else if ($("[id*=RBLProjecttype] input:checked").val() == 2) {
                    SaveAttributeDataForSpecificScreening();
                }
                else if ($("[id*=RBLProjecttype] input:checked").val() == "0000000000") {
                    SaveAttributeDataForGenericScreening();
                }
            }
            else {
                msgalert('Please Add Attribute!');
            }
        }

        function SaveAttributeDataForProjectSpecific() {
            $.ajax({
                type: "POST",
                url: "frmMedExWorkspaceDtlNew.aspx/SaveAttributeInMedExWorkSpaceDtl",
                data: '{"MedExCode":"' + $('#ddlAttribute :selected').val() + '","nMedExWorkSpaceHdrNo":"' + nMedExWorkSpaceHdrNo_ForEdit + '","UserId":"' + '<%= Me.Session(S_UserID)%>' + '","nMedExWorkSpaceDtlNo":"", vMedExGroupCode : ' + $("#ddlAttributeGroup").val() + ',"BulkAttribute":"' + Object.keys(tblMedExMstTempArray) + '"  }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    data = data.d
                    if (data == "True") {
                        msgalert("Data Saved Successfully")
                        FillControlData(vWorkspaceId_ForEdit, vMedExSubGroupCode_ForEdit, Scope_ForEdit, iNodeID_ForEdit);
                        AddMedExMstTemp(true);
                    }
                    else if (data == "False") {
                        msgalert("Error While Save Data")
                    }
                    else {
                        msgalert(data);
                    }

                },
                failure: function (error) {
                    msgalert(error);
                }
            });
        }

        function SaveAttributeDataForSpecificScreening() {
            $.ajax({
                type: "POST",
                url: "frmMedExWorkspaceDtlNew.aspx/SaveAttributeInWorkspaceScreeningDtl",
                data: '{"MedExCode":"' + $('#ddlAttribute :selected').val() + '","nWorkSpaceScreeningHdrNo":"' + nWorkspaceScreeningHdrNo_ForEdit + '","UserId":"' + '<%= Me.Session(S_UserID)%>' + '","nWorkSpaceScreeningDtlNo":"" ,"vMedExGroupCode":"' + $("#ddlAttributeGroup").val() + '","BulkAttribute":"' + Object.keys(tblMedExMstTempArray) + '"  }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    data = data.d
                    if (data == "True") {
                        msgalert("Data Saved Successfully !")
                        FillControlData_SpecificScreening(nWorkspaceScreeningHdrNo_ForEdit, $("#ctl00_CPHLAMBDA_ddlScreeningGroup").val(), Scope_ForEdit, vMedExSubGroupCode_ForEdit);
                        AddMedExMstTemp(true);
                    }
                    else if (data == "False") {
                        msgalert("Error While Save Data !")
                    }
                    else {
                        msgalert(data);
                    }

                },
                failure: function (error) {
                    msgalert(error);
                }
            });
        }

        function SaveAttributeDataForGenericScreening() {
            $.ajax({
                type: "POST",
                url: "frmMedExWorkspaceDtlNew.aspx/SaveAttributeInScreeningTemplateDtl",
                data: '{"MedExCode":"' + $('#ddlAttribute :selected').val() + '","nScreeningTemplateHdrNo":"' + nScreeningTemplateHdrNo_ForEdit + '","UserId":"' + '<%= Me.Session(S_UserID)%>' + '" , "vMedExGroupCode":"' + $("#ddlAttributeGroup").val() + '","BulkAttribute":"' + Object.keys(tblMedExMstTempArray) + '"    }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    data = data.d
                    if (data == "True") {
                        msgalert("Data Saved Successfully !")
                        FillControlData_GenericScreening(nScreeningTemplateHdrNo_ForEdit, $("#ctl00_CPHLAMBDA_ddlScreeningGroup").val(), Scope_ForEdit, vMedExSubGroupCode_ForEdit);
                        AddMedExMstTemp(true);
                    }
                    else if (data == "False") {
                        msgalert("Error While Save Data")
                    }
                    else {
                        msgalert(data);
                    }

                },
                failure: function (error) {
                    msgalert(error);
                }
            });

        }


        function DeleteAttributeData() {
            if ($("[id*=RBLProjecttype] input:checked").val() == 1) {
                DeleteAttributeDataForProjectSpecific();
            }
            else if ($("[id*=RBLProjecttype] input:checked").val() == 2) {
                DeleteAttributeDataForSpecificScreening();
            }
            else if ($("[id*=RBLProjecttype] input:checked").val() == "0000000000") {
                DeleteAttributeDataForGenericScreening();
            }

        }
        function CloseModalPopup() {
            $find('MPE_MedExSubGroupMst').hide();
            return true;
        }

        function DeleteAttributeDataForProjectSpecific() {
            var dataTable = $('#tblMedExMst').dataTable()
            var MedExWorkSpaceDtlNo = ""
            $(dataTable.fnGetNodes()).each(function () {
                var chkcAlertType = ""
                var $tds = $(this).find('td')
                if ($tds.eq(0).find("input[type='checkbox']:checked").length > 0) {
                    MedExWorkSpaceDtlNo += $tds.eq(2).find("textarea")[0].id.split("_")[1] + ","
                }
            });
            MedExWorkSpaceDtlNo = MedExWorkSpaceDtlNo.substr(0, MedExWorkSpaceDtlNo.length - 1)
            $.ajax({
                type: "POST",
                url: "frmMedExWorkspaceDtlNew.aspx/DeleteAttributeInMedExWorkSpaceDtl ",
                data: '{"nMedExWorkSpaceHdrNo":"' + nMedExWorkSpaceHdrNo_ForEdit + '","UserId":"' + '<%= Me.Session(S_UserID)%>' + '","nMedExWorkSpaceDtlNo":"' + MedExWorkSpaceDtlNo + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    data = data.d
                    if (data == "True") {
                        msgalert("Data Deleted Successfully !")
                        FillControlData(vWorkspaceId_ForEdit, vMedExSubGroupCode_ForEdit, Scope_ForEdit, iNodeID_ForEdit);
                    }
                    else {
                        msgalert("Error While Delete Data")
                    }

                },
                failure: function (error) {
                    msgalert(error);
                }
            });

        }

        function DeleteAttributeDataForSpecificScreening() {
            var dataTable = $('#tblMedExMst').dataTable()
            var nWorkSpaceScreeningDtlNo = ""
            $(dataTable.fnGetNodes()).each(function () {
                var chkcAlertType = ""
                var $tds = $(this).find('td')
                if ($tds.eq(0).find("input[type='checkbox']:checked").length > 0) {
                    nWorkSpaceScreeningDtlNo += $tds.eq(2).find("textarea")[0].id.split("_")[1] + ","
                }
            });
            nWorkSpaceScreeningDtlNo = nWorkSpaceScreeningDtlNo.substr(0, nWorkSpaceScreeningDtlNo.length - 1)
            $.ajax({
                type: "POST",
                url: "frmMedExWorkspaceDtlNew.aspx/DeleteAttributeInWorkspaceScreeningDtl",
                data: '{"nWorkSpaceScreeningHdrNo":"' + nWorkspaceScreeningHdrNo_ForEdit + '","UserId":"' + '<%= Me.Session(S_UserID)%>' + '","nWorkSpaceScreeningDtlNo":"' + nWorkSpaceScreeningDtlNo + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    data = data.d
                    if (data == "True") {
                        msgalert("Data Deleted Successfully !")
                        FillControlData_SpecificScreening(nWorkspaceScreeningHdrNo_ForEdit, $("#ctl00_CPHLAMBDA_ddlScreeningGroup").val(), Scope_ForEdit, vMedExSubGroupCode_ForEdit);
                    }
                    else {
                        msgalert("Error While Delete Data")
                    }

                },
                failure: function (error) {
                    msgalert(error);
                }
            });

        }

        function DeleteAttributeDataForGenericScreening() {
            var dataTable = $('#tblMedExMst').dataTable()
            var nScreeningTemplateDtlNo = ""
            $(dataTable.fnGetNodes()).each(function () {
                var chkcAlertType = ""
                var $tds = $(this).find('td')
                if ($tds.eq(0).find("input[type='checkbox']:checked").length > 0) {
                    nScreeningTemplateDtlNo += $tds.eq(2).find("textarea")[0].id.split("_")[1] + ","
                }
            });
            nScreeningTemplateDtlNo = nScreeningTemplateDtlNo.substr(0, nScreeningTemplateDtlNo.length - 1)
            $.ajax({
                type: "POST",
                url: "frmMedExWorkspaceDtlNew.aspx/DeleteAttributeInScreeningTemplateDtl",
                data: '{"nScreeningTemplateHdrNo":"' + nScreeningTemplateHdrNo_ForEdit + '","UserId":"' + '<%= Me.Session(S_UserID)%>' + '","nScreeningTemplateDtlNo":"' + nScreeningTemplateDtlNo + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    data = data.d
                    if (data == "True") {
                        msgalert("Data Deleted Successfully !")
                        FillControlData_GenericScreening(nScreeningTemplateHdrNo_ForEdit, $("#ctl00_CPHLAMBDA_ddlScreeningGroup").val(), Scope_ForEdit, vMedExSubGroupCode_ForEdit);
                    }
                    else {
                        msgalert("Error While Delete Data !")
                    }

                },
                failure: function (error) {
                    msgalert(error);
                }
            });

        }


    </script>

</asp:Content>
