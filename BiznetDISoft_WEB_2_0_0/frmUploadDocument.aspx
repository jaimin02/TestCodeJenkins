<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmUploadDocument.aspx.vb" Inherits="frmUploadDocument"
    ValidateRequest="false"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <link rel="shortcut icon" href="favicon.ico">
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <script type="text/javascript" src="Script/jquery-ui.js"></script>
    <style type="text/css">
        .HeadButton {
            line-height: 1.8em !important;
            border-bottom: 1px solid #ccc !important;
            float: left !important;
            display: inline !important;
            border: 1px solid #d3d3d3 !important;
            background: rgb(30,87,153) !important; /* Old browsers */
            background: -moz-linear-gradient(top, rgba(30,87,153,1) 0%, rgba(41,137,216,1) 50%, rgba(32,124,202,1) 100%, rgba(125,185,232,1) 100%) !important; /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,rgba(30,87,153,1)), color-stop(50%,rgba(41,137,216,1)), color-stop(100%,rgba(32,124,202,1)), color-stop(100%,rgba(125,185,232,1))) !important; /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top, rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 100%,rgba(125,185,232,1) 100%) !important; /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top, rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 100%,rgba(125,185,232,1) 100%) !important; /* Opera 11.10+ */
            background: -ms-linear-gradient(top, rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 100%,rgba(125,185,232,1) 100%) !important; /* IE10+ */
            background: linear-gradient(to bottom, rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 100%,rgba(125,185,232,1) 100%) !important; /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr= '#1e5799', endColorstr= '#7db9e8',GradientType=0 ) !important; /* IE6-9 */
            border-radius: 30px 5px 5px 5px !important;
            color: white !important;
            box-shadow: 5px 5px 5px #888888 !important;
            border: 1px solid aqua !important;
            font-weight: bold !important;
        }

        #tblAuditField {
            margin: 0 auto;
            width: 100%;
            clear: both;
            border-collapse: collapse;
            table-layout: fixed;
            word-wrap: break-word;
        }

        #tblAudit {
            margin: 0 auto;
            width: 100%;
            clear: both;
            border-collapse: collapse;
            table-layout: fixed;
            word-wrap: break-word;
        }

        .hide_column {
            display: none;
        }

        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

        .datatableStyle {
            width: 100%;
        }
    </style>
    <asp:UpdatePanel ID="upPannelUploadDocument" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellpadding="5px" style="width: 100%;">
                <tr>
                    <td>
                        <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                            <legend class="LegendText" style="color: Black; font-size: 12px">
                                <img id="img2" alt="Project Details" src="images/panelcollapse.png"
                                    onclick="Display(this,'divUploadDetail');" runat="server" style="margin-right: 2px;" />Project Details</legend>
                            <div id="divUploadDetail">
                                <table cellpadding="5px" width="100%" id="Edit1">
                                    <tbody>
                                        <tr id="trRbtnlst" runat="server">
                                            <td colspan="4" nowrap="nowrap" class="SpecificType">
                                                <asp:RadioButtonList ID="rbtSpecific" runat="server" RepeatDirection="Horizontal" Style="margin: auto;">
                                                    <asp:ListItem Selected="True" Value="Project" onClick="ResetPage(this);">Project Specific</asp:ListItem>
                                                    <asp:ListItem Value="Activity" onClick="ResetPage(this);">Activity Specific</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Label" nowrap="nowrap" style="text-align: right; width: 30%;">Select Project :
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="txtProject" runat="server" CssClass="textBox" Width="99%"></asp:TextBox>
                                                <asp:Button Style="display: none" ID="btnSetProject" runat="server" Text="Project"></asp:Button>
                                                <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                                                    TargetControlID="txtProject" ServicePath="AutoComplete.asmx" ServiceMethod="GetMyProjectCompletionListParentOnly"
                                                    OnClientShowing="ClientFromPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1"
                                                    CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                    CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1" CompletionListElementID="pnlProjectList">
                                                </cc1:AutoCompleteExtender>
                                                <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" />
                                            </td>
                                            <td id="tdPeriod" runat="server" class="Label" nowrap="nowrap" style="text-align: right; width: 7%;">Period :
                                            </td>
                                            <td id="tdddlPeriod" runat="server" style="text-align: left; width: 33%;">
                                                <asp:DropDownList ID="ddlPeriod" onChange="populateGrid()" CssClass="dropDownList" Width="30%" runat="server" AutoPostBack="True"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="documentUpload">
                                            <td class="Label" nowrap="nowrap" style="text-align: right;">Document :                        
                                            </td>
                                            <td colspan="3" style="text-align: left; color: red">
                                                <asp:FileUpload ID="flCRFUploadDoc" runat="server" CssClass="textboxFileUpload" onchange="validate_fileupload(this);" ClientIDMode="Static" Width="24%" />

                                                Only pdf , jpeg , jpg , png format allow.
                                            </td>

                                        </tr>
                                        <tr>
                                            <td colspan="4" align="center">
                                                <asp:Button ID="btnUpload" runat="server" CssClass="btn btnsave" OnClientClick="return ExistingFileValidation(this);" OnClick="btnUpload_Click" Text="Upload" />
                                                <asp:Button ID="btnCancle" OnClientClick="return ResetPage(this);" runat="server" CssClass="btn btncancel" Text="Cancel" />
                                                <asp:Button ID="btnExit" runat="server" CssClass="btn btnexit" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" Text="Exit" />
                                                <asp:Button ID="btnAudit" runat="server" CssClass="btn btnaudit" OnClientClick="return AudtiTrail(this);" class="AuditTrail" Style="display: none" />
                                                <cc1:ModalPopupExtender ID="mdlAudit" runat="server" BackgroundCssClass="modalBackground" BehaviorID="mdlAudit" CancelControlID="imgClosePopup" PopupControlID="dvAuditTrail" TargetControlID="btnAudit">
                                                </cc1:ModalPopupExtender>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </fieldset>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
            <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table style="width: 100%;" cellpadding="5px">
                <tbody>
                    <tr>
                        <td>
                            <fieldset id="fsetUpload" runat="server" class="FieldSetBox" style="visibility:hidden; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img1" alt="File Details" src="images/panelcollapse.png"
                                        onclick="Display(this,'divGridData');" runat="server" style="margin-right: 2px;" />File Details</legend>
                                <div id="divGridData">
                                    <table align="center" id="Edit3">
                                        <tr>
                                            <td colspan="4" style="text-align: center; width: 10%; padding: 0px 64px 0px 64px;">
                                                <div id="gvGrid">
                                                    <asp:Label ID="lblProjecStatus" runat="server" Style="display: none;"></asp:Label>
                                                    <asp:GridView ID="gvActivityGrid" runat="server" Style="visibility: hidden; width: 100%;" AutoGenerateColumns="false">
                                                        <Columns>
                                                            <asp:BoundField HeaderText="Sr.No" />
                                                            <asp:BoundField HeaderText="Activite" />
                                                            <asp:BoundField HeaderText="Period" />
                                                            <asp:BoundField HeaderText="UserName" />
                                                            <asp:BoundField HeaderText="Upload" />
                                                            <asp:BoundField HeaderText="Audit Trail" />
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Panel ID="pnlOrders" runat="server" Style="display: none">
                                                                        <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="false" CssClass="ChildGrid">
                                                                            <Columns>
                                                                                <asp:BoundField ItemStyle-Width="150px" DataField="OrderId" HeaderText="Order Id" />
                                                                                <asp:BoundField ItemStyle-Width="150px" DataField="OrderDate" HeaderText="Date" />
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
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
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:Button ID="btnFileUploadModalPopup" runat="server" CssClass="button" OnClientClick="return getClickEvent();" Style="display: none;" />
    <cc1:ModalPopupExtender ID="mpActivityGrid" runat="server" BackgroundCssClass="modalBackground" BehaviorID="mpActivityGrid" CancelControlID="imgActivityClose"
        PopupControlID="divActivityGridpopup" TargetControlID="btnFileUploadModalPopup">
    </cc1:ModalPopupExtender>
    <table>
        <tr>
            <td>
                <asp:UpdatePanel ID="upActivityGridModelPopup" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="divActivityGridpopup" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 54%; height: auto; max-height: 75%; min-height: auto;">
                            <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                <tr>
                                    <td id="Td1" class="LabelText" style="font-weight: bold; text-align: center !important; font-size: 15px !important; width: 97%;">File Upload</td>
                                    <td style="width: 3%">
                                        <img id="imgActivityClose" alt="Close" src="images/Sqclose.gif" onclick="return ClearText();" onmouseover="this.style.cursor='pointer';" />
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
                                                <td style="text-align: right; width: 63%">
                                                    <asp:FileUpload text="abc" ID="fuActivityFile" onchange="validate_fileupload(this);" CssClass="textboxFileUpload" runat="server" />
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:Button ID="btnActivityFileUpload" OnClientClick="return ExistingFileValidation(this);" Text="Upload" runat="server" CssClass="btn btnnew" />
                                                    <asp:Button ID="btnClear" OnClientClick="return ClearFilePath();" runat="server" Text="Cancel" CssClass="btn btncancel" />
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
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnActivityFileUpload" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>

        <asp:Label ID="lblActivityName" runat="server" Text=""></asp:Label>
    </table>
    <asp:Button ID="btnAuditActivity" runat="server" OnClientClick="return AudtiTrail(this);" Style="display: none"></asp:Button>
    <cc1:ModalPopupExtender ID="mdlAuditActivity" runat="server" BackgroundCssClass="modalBackground" BehaviorID="mdlAuditActivity" CancelControlID="imgActivityAuditTrail" PopupControlID="dvActivityAudiTrail" TargetControlID="btnAuditActivity"></cc1:ModalPopupExtender>
    <table>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpActivityAuditTrail" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <div id="dvActivityAudiTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 80%; height: auto; max-height: 75%; min-height: auto;">
                            <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                <tr>
                                    <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 97%;">Audit Trail Information</td>
                                    <td style="width: 3%">
                                        <img id="imgActivityAuditTrail" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
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
                                                    <table id="tblActivityAudit" class="tblAudit" width="100%" style="background-color: aliceblue;"></table>
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
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div id="dvAuditTrail" runat="server" class="centerModalPopup" style="height: 100px; overflow-y: scroll; background-color: aliceblue; display: none; width: 75%;">
                                        <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                            <tr>
                                                <td id="HistoryHeader" class="LabelText" style="font-weight: bold; background-color: aliceblue; height: 31px; text-align: center !important; font-size: 15px !important; width: 97%;">Audit Trail Information</td>
                                                <td style="width: 3%">
                                                    <img id="imgClosePopup" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
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
                                                                <table id="tblAudit" class="tblAudit" width="100%">
                                                                </table>
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
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:Button ID="btnRemarks" runat="server" Style="display: none;" TabIndex="55" />
    <cc1:ModalPopupExtender ID="mdlRemarks" runat="server" PopupControlID="divRemarks"
        BackgroundCssClass="modalBackground" BehaviorID="mdlRemarks" CancelControlID="btnRemarksCancel"
        TargetControlID="btnRemarks">
    </cc1:ModalPopupExtender>
    <div id="divRemarks" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 32%; height: auto; max-height: 45%; min-height: auto;">
        <table cellpadding="2" cellspacing="2" width="100%">
            <tr>
                <td class="LabelText" style="text-align: left !important; font-size: 12px !important; width: 97%;">Remarks
                </td>
            </tr>
            <tr>
                <td>
                    <hr />
                </td>
            </tr>
            <tr>
                <td class="LabelText" style="text-align: left !important;">Enter Remarks:
                </td>
            </tr>
            <tr>
                <td style="text-align: left !important;">
                    <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="5" Height="60px"
                        Width="300px" TabIndex="55" />
                </td>
            </tr>
            <tr>
                <td>
                    <hr />
                </td>
            </tr>
            <tr>
                <td style="text-align: center;">
                    <%--<asp:Button ID="btnRemarksUpdate" runat="server" Text="Update" CssClass="ButtonText"
                        Width="64px" Style="font-size: 12px !important;" TabIndex="56" OnClientClick="return UpdateData();" />--%>
                    <asp:Button ID="btnRemarksUpdate" runat="server" Text="Save" CssClass="btn btnsave"
                         Style="font-size: 12px !important;" TabIndex="56" OnClientClick="return CheckValidation();" />
                    <asp:Button ID="btnRemarksCancel" runat="server" Text="Cancel" CssClass="btn btncancel"
                        Style="font-size: 12px !important;" TabIndex="56" />
                </td>
            </tr>
        </table>
    </div>

    <asp:HiddenField ID="hdnMode" runat="server" Value="0"> </asp:HiddenField>

    <asp:Button ID="btnDeleteFile" runat="server" Style="display: none;" />
    <asp:HiddenField runat="server" ID="hdnActivityName" />
    <asp:HiddenField runat="server" ID="hndNodeID" />
    <asp:HiddenField runat="server" ID="hndProjectStatus" />
    <script type="text/javascript" language="javascript">

        function fsetUpload_show() {
            $('#<%=fsetUpload.ClientID%>').attr('style', $('#<%=fsetUpload.ClientID%>').attr('style') + ';display:block');
     }

        function HideSponsorDetails() {
            //$('#<%= img2.ClientID%>').click();
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

        //function PageLoad() {

        //    
        //    Page.Response.Redirect(Page.Request.Url.ToString(), true);
        //}
        var editor;
        var EventId;
        function pageLoad() {
            CheckProjectStatus();
        }

        function getSpecification() {
            if (document.getElementById("ctl00_CPHLAMBDA_rbtSpecific_0").checked == true) {
                document.getElementById("documentUpload").removeAttribute("style")
                //document.getElementById("ctl00_CPHLAMBDA_btnUpload").style.visibility = 'visible';
                document.getElementById("ctl00_CPHLAMBDA_btnUpload").removeAttribute("style");
                //document.getElementById("ctl00_CPHLAMBDA_btnUpload").style.marginLeft = '531px';
                document.getElementById("ctl00_CPHLAMBDA_btnCancle").removeAttribute("style");
                document.getElementById("Edit3").style.visibility = 'hidden';
                document.getElementById("ctl00_CPHLAMBDA_gvActivityGrid").style.visibility = 'hidden';
                document.getElementById("ctl00_CPHLAMBDA_fsetUpload").style.visibility = 'hidden';
                document.getElementById("ctl00_CPHLAMBDA_btnAudit").style.visibility = 'visible';
                document.getElementById("ctl00_CPHLAMBDA_dvAuditTrail").style.visibility = 'visible';
                //    CheckProjectStatus();
                if (document.getElementById("ctl00_CPHLAMBDA_txtProject").value != "") {
                    document.getElementById("Edit3").style.visibility = 'visible';
                    document.getElementById("ctl00_CPHLAMBDA_gvActivityGrid").style.visibility = 'visible';
                    document.getElementById("ctl00_CPHLAMBDA_fsetUpload").style.visibility = 'visible';
                    if ($('#ctl00_CPHLAMBDA_gvActivityGrid').attr("istable") == "has") {
                        ClearGrid();
                        RemoveCss();
                    }
                    else {
                        projectGrid();
                    }

                }
                //  document.getElementById("ctl00_CPHLAMBDA_btnCancle").style.visibility = 'visible';                           
            }
            else {

                document.getElementById("documentUpload").style.display = 'none';
                document.getElementById("ctl00_CPHLAMBDA_btnUpload").style.display = 'none';
                //document.getElementById("ctl00_CPHLAMBDA_btnCancle").style.marginLeft = '525px';
                document.getElementById("ctl00_CPHLAMBDA_btnCancle").style.marginTop = '0px';
                document.getElementById("ctl00_CPHLAMBDA_gvActivityGrid").style.visibility = 'visible';
                document.getElementById("ctl00_CPHLAMBDA_fsetUpload").style.visibility = 'visible';
                document.getElementById("ctl00_CPHLAMBDA_btnAudit").style.visibility = 'hidden';
                document.getElementById("ctl00_CPHLAMBDA_dvAuditTrail").style.visibility = 'hidden';
                //  document.getElementById("ctl00_CPHLAMBDA_btnCancle").style.visibility = 'hidden';   

                if (document.getElementById("ctl00_CPHLAMBDA_txtProject").value != "") {
                    document.getElementById("Edit3").style.visibility = 'visible';
                    document.getElementById("ctl00_CPHLAMBDA_gvActivityGrid").style.visibility = 'visible';
                    document.getElementById("ctl00_CPHLAMBDA_fsetUpload").style.visibility = 'visible';
                    if ($('#ctl00_CPHLAMBDA_gvActivityGrid').attr("istable") == "has") {
                        ClearGrid();
                        RemoveCss();
                    } else {
                        populateGrid();
                    }

                }
                else {
                    document.getElementById("Edit3").style.visibility = 'hidden';
                    document.getElementById("ctl00_CPHLAMBDA_gvActivityGrid").style.visibility = 'hidden';
                    document.getElementById("ctl00_CPHLAMBDA_fsetUpload").style.visibility = 'hidden';
                }
            }
            return true;
        }

        function getSpecification_new() {
            //===========================
            if (window.location.search.substring(1).split("=")[1] == "1") {
                document.getElementById("documentUpload").removeAttribute("style")
                document.getElementById("ctl00_CPHLAMBDA_btnUpload").removeAttribute("style");
                document.getElementById("ctl00_CPHLAMBDA_btnCancle").removeAttribute("style");
                document.getElementById("Edit3").style.visibility = 'hidden';
                document.getElementById("ctl00_CPHLAMBDA_gvActivityGrid").style.visibility = 'hidden';
                document.getElementById("ctl00_CPHLAMBDA_fsetUpload").style.visibility = 'hidden';
                document.getElementById("ctl00_CPHLAMBDA_btnAudit").style.visibility = 'visible';
                document.getElementById("ctl00_CPHLAMBDA_dvAuditTrail").style.visibility = 'visible';
                if (document.getElementById("ctl00_CPHLAMBDA_txtProject").value != "") {
                    document.getElementById("Edit3").style.visibility = 'visible';
                    document.getElementById("ctl00_CPHLAMBDA_gvActivityGrid").style.visibility = 'visible';
                    document.getElementById("ctl00_CPHLAMBDA_fsetUpload").style.visibility = 'visible';
                    if ($('#ctl00_CPHLAMBDA_gvActivityGrid').attr("istable") == "has") {
                        ClearGrid();
                        RemoveCss();
                    }
                    else {
                        projectGrid();
                    }

                }
            }

            else if (window.location.search.substring(1).split("=")[1] != "1") {

                document.getElementById("documentUpload").style.display = 'none';
                document.getElementById("ctl00_CPHLAMBDA_btnUpload").style.display = 'none';
                document.getElementById("ctl00_CPHLAMBDA_btnCancle").style.marginTop = '0px';
                document.getElementById("ctl00_CPHLAMBDA_gvActivityGrid").style.visibility = 'visible';
                document.getElementById("ctl00_CPHLAMBDA_fsetUpload").style.visibility = 'visible';
                document.getElementById("ctl00_CPHLAMBDA_btnAudit").style.visibility = 'hidden';
                document.getElementById("ctl00_CPHLAMBDA_dvAuditTrail").style.visibility = 'hidden';
                if (document.getElementById("ctl00_CPHLAMBDA_txtProject").value != "") {
                    document.getElementById("Edit3").style.visibility = 'visible';
                    document.getElementById("ctl00_CPHLAMBDA_gvActivityGrid").style.visibility = 'visible';
                    document.getElementById("ctl00_CPHLAMBDA_fsetUpload").style.visibility = 'visible';
                    if ($('#ctl00_CPHLAMBDA_gvActivityGrid').attr("istable") == "has") {
                        ClearGrid();
                        RemoveCss();
                    } else {
                        populateGrid();
                    }

                }
                else {
                    document.getElementById("Edit3").style.visibility = 'hidden';
                    document.getElementById("ctl00_CPHLAMBDA_gvActivityGrid").style.visibility = 'hidden';
                    document.getElementById("ctl00_CPHLAMBDA_fsetUpload").style.visibility = 'hidden';
                }
            }
            //===========================


            return true;
        }

        function ClientFromPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientID%>'));
        }
        function CheckProjectStatus() {
            var WorkspaceID = document.getElementById('<%= HProjectId.ClientID%>').value;
            $.ajax({
                type: "post",
                url: "frmUploadDocument.aspx/CheckProjectScope",
                data: '{"WorkspaceID":"' + WorkspaceID + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                dataType: "json",
                success: function (data) {
                    if (data.d == "CT") {
                        //document.getElementById("ddlPeriod").style.visibility = 'hidden';
                        //document.getElementById("ddlPeriod").style.position = 'absolute';
                    }
                },
                failure: function (response) {
                    msgalert(response.d);
                },
                error: function (response) {
                    msgalert(response.d);
                }
            });
        }

        function OnSelected(sender, e) {

            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.ClientID%>'),
            $get('<%= HProjectId.ClientID%>'), document.getElementById('<%= btnSetProject.ClientID%>'));
        }

        function getCounter(i) {
            var cnt = 0;

            if (i > 0 && i < 10)
                cnt = "0" + i;
            else
                cnt = i;
            return cnt;
        }
        function projectGrid() {
            var TotalActivity;
            var FileName = "";
            var WorkspaceID = document.getElementById('<%= HProjectId.ClientID%>').value;
            var LockProjectStatus = false;
            var cDocType;
            if (window.location.search.substring(1).split("=")[1] == "1") {
                cDocType = "T"
            }
            else {
                cDocType = "G"
            }
            document.getElementById("ctl00_CPHLAMBDA_gvActivityGrid").style.visibility = 'visible';
            document.getElementById("ctl00_CPHLAMBDA_fsetUpload").style.visibility = 'visible';
            $.ajax({
                type: "post",
                url: "frmUploadDocument.aspx/FillProjectGrid",
                data: '{"Period" :"' + $get('ctl00_CPHLAMBDA_ddlPeriod').options[$get('ctl00_CPHLAMBDA_ddlPeriod').selectedIndex].text + '","WorkspaceID":"' + WorkspaceID + '" ,"cDocType":"' + cDocType + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                dataType: "json",
                success: function (data) {
                    $('#ctl00_CPHLAMBDA_gvActivityGrid').attr("IsTable", "has");
                    var ActivityDataset = [];
                    if (data.d != "" && data.d != null) {
                        if (data.d.length > 2) {
                            var abc = data.d.split(",");
                            var pqr = abc[2].split(":");
                            var lmn = pqr[1].split("/");
                            var abc = lmn[4];
                            if (abc != undefined) {
                                var FileName = abc.replace('"', ' ');
                                var n = FileName.length;
                                if (n > 80) {
                                    var a = FileName.substring(1, 80);
                                    var b = FileName.substring(81, FileName.length);
                                    var FileName = a + " " + b;
                                }
                            }
                        }
                        data = JSON.parse(data.d)
                        TotalActivity = data.length;
                        for (var Row = 0; Row < data.length; Row++) {
                            var InDataset = [];
                            if (data[Row].LockStatus == "L") {
                                if (data[Row].StatusIndi != "D") {
                                    document.getElementById('<%=btnDeleteFile.ClientID%>').click();
                                }
                                if (data[Row].StatusIndi == "D") {
                                    LockProjectStatus = true;
                                }
                            }
                            InDataset.push(data[Row].SrNo, data[Row].ProjectNo, FileName, data[Row].UpdatedBy, data[Row].UploadedOn, data[Row].AuditTrail, data[Row].LockStatus, data[Row].StatusIndi);
                            ActivityDataset.push(InDataset);
                        }
                        $('#ctl00_CPHLAMBDA_gvActivityGrid').dataTable({
                            "bJQueryUI": true,
                            "processing": true,
                            "bDestory": true,
                            "bSort": false,
                            "scrollX": 10,
                            "scrollCollapse": true,
                            "jQueryUI": true,
                            "bStateSave": true,
                            "bPaginate": false,
                            "sPaginationType": "full_numbers",
                            "bPaginate": true,
                            "bFooter": true,
                            "bHeader": true,
                            "bAutoWidth": true,
                            "bFilter": true,
                            "select": true,
                            "bRetrieve": true,
                            "aaData": ActivityDataset,
                            "bInfo": false,
                            "fnCreatedRow": function (nRow, aData, iDataIndex) {
                                $('td:eq(5)', nRow).append("<input type='image' id='imgAudit_" + iDataIndex + "' name='imgEdit$" + iDataIndex + "' src='Images/audit.png' OnClick='return AudtiTrail(this);' style='border-width:0px;'>");
                            },
                            "aoColumns": [
                                        { "sTitle": "Sr.No." },
                                        { "sTitle": "Project No" },
                                        { "sTitle": "File Name" },
                                        { "sTitle": "Uploaded By" },
                                        { "sTitle": "Uploaded On" },
                                        { "sTitle": "Audit Trail" },
                                        {
                                            "sTitle": "Lock",
                                            "sClass": "hide_column"
                                        },
                                        {
                                            "sTitle": "StatusIndi",
                                            "sClass": "hide_column"
                                        }
                            ],
                            "oLanguage": {
                                "sEmptyTable": "No Record Found",
                            },
                            "aoColumnDefs": [
                                        { "bSortable": false, "aTargets": [0] },
                                        { "bSortable": false, "aTargets": [1] },
                                        { "bSortable": false, "aTargets": [2] },
                                        { "bSortable": false, "aTargets": [3] },
                                        { "bSortable": false, "aTargets": [4] },
                                        { "bSortable": false, "aTargets": [5] },
                            ],
                        });
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
            if (LockProjectStatus == true) {
                msgalert("Project is Locked. You Can not upload documents !");
                document.getElementById("ctl00_CPHLAMBDA_btnUpload").style.display = 'none';
                document.getElementById("ctl00_CPHLAMBDA_btnCancle").style.marginLeft = '525px';
                document.getElementById("ctl00_CPHLAMBDA_btnCancle").style.marginTop = '0px';
                return false;
            }
            return false;
        }
        function populateGrid() {
            var TotalActivity;
            var WorkspaceID = document.getElementById('<%= HProjectId.ClientID%>').value;
            var LockS = false;
            document.getElementById("ctl00_CPHLAMBDA_gvActivityGrid").style.visibility = 'visible';
            document.getElementById("ctl00_CPHLAMBDA_fsetUpload").style.visibility = 'visible';

            $.ajax({
                type: "post",
                url: "frmUploadDocument.aspx/FillActivityGrid",
                data: '{"Period" :"' + $get('ctl00_CPHLAMBDA_ddlPeriod').options[$get('ctl00_CPHLAMBDA_ddlPeriod').selectedIndex].text + '","WorkspaceID":"' + WorkspaceID + '"  }',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                dataType: "json",
                success: function (data) {
                    $('#ctl00_CPHLAMBDA_gvActivityGrid').attr("IsTable", "has");
                    var ActivityDataset = [];
                    if (data.d != "" && data.d != null) {
                        data = JSON.parse(data.d);
                        TotalActivity = data.length;
                        for (var Row = 0; Row < data.length; Row++) {
                            var InDataset = [];
                            if (data[Row].LockStatus == "L") {
                                LockS = true;
                            }
                            InDataset.push(data[Row].Add, data[Row].blank, data[Row].SrNo, data[Row].Activites, data[Row].Period, data[Row].LockStatus, data[Row].vActivityId, data[Row].iNodeId);
                            ActivityDataset.push(InDataset);
                        }
                        $('#ctl00_CPHLAMBDA_gvActivityGrid').dataTable({
                            "bJQueryUI": true,
                            "processing": true,
                            "bDestory": true,
                            "bSort": false,
                            "scrollX": 10,
                            "scrollCollapse": true,
                            "jQueryUI": true,
                            "bStateSave": true,
                            "bPaginate": false,
                            "pagingType": "full_numbers",
                            "bPaginate": true,
                            "bFooter": true,
                            "bHeader": true,
                            "bAutoWidth": true,
                            "bFilter": true,
                            "select": true,
                            "bRetrieve": true,
                            "aaData": ActivityDataset,
                            "bInfo": false,
                            "fnCreatedRow": function (nRow, aData, iDataIndex) {
                                $('td:eq(0)', nRow).append("<input type='image' id='imgAdd_" + iDataIndex + "' name='imgAdd$" + iDataIndex + "' src='images/expand.PNG' class='Expandtable' OnClick='return rowExpand(this);' style='border-width:0px;' vActivityId='" + aData[6] + "' ActivityName='" + aData[3] + "' NodeId= '" + aData[7] + "' >");
                                $('td:eq(0)', nRow).append("<input type='image' id='imgMinus_" + iDataIndex + "' name='imgAdd$" + iDataIndex + "' src='Images/minus.png' class='Expandtable' OnClick='return rowHide(this);' style='border-width:0px;visibility:hidden'>");

                            },
                            "aoColumns": [
                                        {
                                            "sTitle": "",
                                            "className": 'details-control',
                                        },
                                        {
                                            "sTitle": "",
                                            "className": "toggelClass",
                                            "sClass": "hide_column"
                                        },
                                        { "sTitle": "Sr.No." },
                                        { "sTitle": "Activites" },
                                        {
                                            "sTitle": "Period",
                                            "sClass": "hide_column"
                                        },
                                        {
                                            "sTitle": "LockStatus",
                                            "sClass": "hide_column"
                                        },
                                         {
                                             "sTitle": "Activity Id",
                                             "sClass": "hide_column"
                                         },
                                         {
                                             "sTitle": "iNodeId",
                                             "sClass": "hide_column"
                                         }

                            ],
                            "aoColumnDefs": [
                                        { "bSortable": false, "aTargets": [0] },
                                        { "bSortable": false, "aTargets": [1] },
                                        { "bSortable": false, "aTargets": [2] },
                                        { "bSortable": false, "aTargets": [3] },
                                        { "bSortable": false, "aTargets": [4] },
                                         { "bSortable": false, "aTargets": [5] },
                            ],
                            "oLanguage": {
                                "sEmptyTable": "No Record Found",
                            },
                            "oPaginate": {
                                "sPrevious": "Prev",
                                "sNext": "Next"
                            }
                        });
                    }
                },
                failure: function (response) {
                    msgalert(response.d);
                },
                error: function (response) {
                    msgalert(response.d);
                }
            });
            //if (LockS == true) {
            //    alert("Project is Locked. You Can not upload documents.");
            //    return false;
            //}
        }
        function getCounter(i) {
            var cnt = 0;

            if (i > 0 && i < 10)
                cnt = "0" + i;
            else
                cnt = i;
            return cnt;
        }
        function rowExpand(e) {
            var vActiviyId = "";
            var ActivityName = "";
            var Period = "";
            var number = "";
            var NodeId = "";
            var LockStatus = false;
            var WorkspaceID = document.getElementById('<%= HProjectId.ClientID%>').value;
            if (document.getElementById("tblExpand") != null) {
                rowHide(EventId);
            }
            vActiviyId = $("#" + e.id).attr("vActivityId");
            ActivityName = $("#" + e.id).attr("ActivityName");
            NodeId = $("#" + e.id).attr("NodeId");
            Period = $get('ctl00_CPHLAMBDA_ddlPeriod').options[$get('ctl00_CPHLAMBDA_ddlPeriod').selectedIndex].text;
            $(e).closest('tr').after('<tr class="expand"><td></td><td colspan="2"><table id="tblExpand"></table></tr></tr>');
            $.ajax({
                type: "post",
                url: "frmUploadDocument.aspx/FillChildGrid",
                data: '{"Period":"' + Period + '","vActiviyId":"' + vActiviyId + '","ActivityName":"' + ActivityName + '","WorkspaceID":"' + WorkspaceID + '","NodeId":"' + NodeId + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                dataType: "json",
                success: function (data) {
                    var ActivityDataset = [];
                    if (data.d != "" && data.d != null) {
                        data = JSON.parse(data.d)
                        TotalActivity = data.length;
                        for (var Row = 0; Row < data.length; Row++) {
                            var InDataset = [];
                            if (data[Row].FileName != "No File Uploaded") {
                                var splitFilename = data[Row].FileName.split("/");
                                data[Row].FileName = splitFilename[6];
                                var n = splitFilename[6].length;
                                if (n > 25) {
                                    var a = splitFilename[6].substring(1, 25);
                                    var b = splitFilename[6].substring(26, splitFilename[6].length);
                                    data[Row].FileName = a + " " + b;
                                }
                            }
                            if (data[Row].LockStatus == "L") {
                                LockStatus = true;
                            }
                            InDataset.push(data[Row].blank, data[Row].SrNo, data[Row].Activites, data[Row].Period, data[Row].UserName, data[Row].FileName, data[Row].Upload, data[Row].AuditTrail, data[Row].vActivityId, data[Row].LockStatus, data[Row].NodeId);
                            ActivityDataset.push(InDataset);
                        }
                        $('#tblExpand').dataTable({
                            "bJQueryUI": true,
                            "sPaginationType": "full_numbers",
                            "bLengthChange": true,
                            "bProcessing": true,

                            "scrollY": "200px",
                            "scrollX": "200px",
                            "aaData": ActivityDataset,


                            "fnCreatedRow": function (nRow, aData, iDataIndex) {
                                if (LockStatus == true) {
                                    $('td:eq(6)', nRow).append("<input type='image' id='imgEdit_" + iDataIndex + "' name='imgEdit$" + iDataIndex + "' src='images/Upload.jpg' OnClick='return UploadActivityDocument(this);' style='border-width:0px;' disabled vActivityId='" + aData[8] + "' ActivityName='" + aData[2] + "' NodeId = '" + aData[10] + "' >");
                                }
                                else {
                                    $('td:eq(6)', nRow).append("<input type='image' id='imgEdit_" + iDataIndex + "' name='imgEdit$" + iDataIndex + "' src='images/Upload.jpg' OnClick='return UploadActivityDocument(this);' style='border-width:0px;' vActivityId='" + aData[8] + "' ActivityName='" + aData[2] + "' NodeId = '" + aData[10] + "'>");
                                }
                                $('td:eq(7)', nRow).append("<input type='image' id='imgAudit_" + iDataIndex + "' name='imgEdit$" + iDataIndex + "' src='Images/audit.png' OnClick='return AudtiTrail(this);' style='border-width:0px;' vActivityId='" + aData[8] + "' ActivityName='" + aData[2] + "' NodeId = '" + aData[10] + "'>");
                            },
                            "aoColumns": [
                                        {
                                            "sTitle": "",
                                            "className": "toggelClass",
                                            "sClass": "hide_column"
                                        },
                                        { "sTitle": "Sr.No." },
                                        { "sTitle": "Activites" },
                                        {
                                            "sTitle": "Period",
                                            "sClass": "hide_column"
                                        },
                                        { "sTitle": "Uploaded By" },
                                        { "sTitle": "FileName" },
                                        { "sTitle": "Upload", },
                                        { "sTitle": "AuditTrail", },
                                        {
                                            "sTitle": "Activity Id",
                                            "sClass": "hide_column"
                                        },
                                        {
                                            "sTitle": "LockStatus",
                                            "sClass": "hide_column"
                                        },
                                        {
                                            "sTitle": "NodeId",
                                            "sClass": "hide_column"
                                        }

                            ],
                            "oLanguage": {
                                "sEmptyTable": "No Record Found",
                            }
                        });

                    }
                    document.getElementById(e.id).style.visibility = 'hidden';
                    number = e.id.split("_");
                    number = "imgMinus_" + number[1]
                    document.getElementById(number).style.visibility = 'visible';
                    EventId = number;
                },
                failure: function (response) {
                    msgalert(response.d);
                },
                error: function (response) {
                    msgalert(response.d);
                }
            });
            return false
        }
        function rowHide(e) {
            var number = "";
            var Minus = "";
            var Add = "";
            $('tr .expand').hide();
            if (e.id == undefined) {
                number = e.split("_");
                Minus = "imgMinus_" + number[1];
                Add = "imgAdd_" + number[1];
                document.getElementById(Minus).style.visibility = 'hidden';
                document.getElementById(Add).style.visibility = 'visible';
            } else {
                document.getElementById(e.id).style.visibility = 'hidden';
                number = e.id.split("_");
                number = "imgAdd_" + number[1]
                document.getElementById(number).style.visibility = 'visible';
            }
            $('tr .expand').remove();
            populateGrid()
            return false;

        }
        function UploadActivityDocument(e) {
            var RowId = e.id;
            var vActivityId;
            var NodeId;
            //  $("#" + RowId).click(function () {
            ActivityName = $("#" + RowId).attr("vActivityId");
            NodeId = $("#" + RowId).attr("NodeId");
            $("#ctl00_CPHLAMBDA_btnActivityFileUpload").attr("ActivityID", ActivityName);
            $("#ctl00_CPHLAMBDA_btnActivityFileUpload").attr("NodeId", NodeId)
            $("#<%=hdnActivityName.ClientID%>").val(ActivityName);
            $("#<%=hndNodeID.ClientID%>").val(NodeId);
            $find('mpActivityGrid').show();
            $("#ctl00_CPHLAMBDA_btnClear").attr("RowId", e.id);
            return false;
            //   });
            return false;
        }

        var checkfile = "";
        function ExistingFileValidation(e) {

            var strconfirm;
            var SpecificType;
            var vActiviyId = "";
            var Period = "";
            var NodeId = "";
            var WorkspaceID = document.getElementById('<%= HProjectId.ClientID%>').value;
            var cDocType = "G";
            //var Mode = window.location.search.substring(1).split("=")[1];

            //if (window.location.search.substring(1).split("=")[1] == "1") {
            //    cDocType = "T";
            //}
            //else {
            //    cDocType = "G";
            //}
            if (document.getElementById("ctl00_CPHLAMBDA_txtProject").value == "") {
                msgalert("Please Select Project !");
                return false;
            }
            if (e.id == "ctl00_CPHLAMBDA_btnActivityFileUpload") {
                if (document.getElementById("ctl00_CPHLAMBDA_fuActivityFile").value == "") {
                    msgalert("Please Select File !");
                    return false;
                }
            }
            if (e.id == "ctl00_CPHLAMBDA_btnUpload") {
                if (document.getElementById("flCRFUploadDoc").value == "") {
                    msgalert("Please Select File !");
                    return false;
                }
            }
            if (document.getElementById("ctl00_CPHLAMBDA_rbtSpecific_1").checked == true) {
                vActiviyId = $("#ctl00_CPHLAMBDA_btnActivityFileUpload").attr("ActivityID");
                NodeId = $("#ctl00_CPHLAMBDA_btnActivityFileUpload").attr("NodeId");
            }

            Period = $get('ctl00_CPHLAMBDA_ddlPeriod').options[$get('ctl00_CPHLAMBDA_ddlPeriod').selectedIndex].text;

            $.ajax({
                type: "post",
                url: "frmUploadDocument.aspx/CheckUploadFile",
                data: '{"vActiviyId":"' + vActiviyId + '","Period":"' + Period + '","WorkspaceID":"' + WorkspaceID + '","NodeId":"' + NodeId + '","cDocType":"' + cDocType + '" }',
                contentType: "application/json; charset=utf-8",
                async: false,
                datatype: JSON,
                success: function (data) {
                    if (data.d == "True") {
                        //strconfirm = confirm("");
                        //if (strconfirm == true) {
                        //    document.getElementById("ctl00_CPHLAMBDA_txtRemarks").value = ""
                        //    $find('mdlRemarks').show();
                        //    return false;
                        //}
                        //else {
                        //    return false;
                        //}
                        msgConfirmDeleteAlert(null, "Are you sure you want to replace file with same project ?", function (isConfirmed) {
                            strconfirm = isConfirmed;
                            if (isConfirmed) {
                                document.getElementById("ctl00_CPHLAMBDA_txtRemarks").value = ""
                                $find('mdlRemarks').show();
                                return true;
                            } else {
                                return false;
                            }
                        });
                        return false;
                    }
                    else {
                        strconfirm = true;
                        document.getElementById("ctl00_CPHLAMBDA_txtRemarks").value = ""
                        checkfile = "new";
                        checkvalidatefile(checkfile);
                        return true;
                    }
                    return false;
                }
            });
            if (strconfirm == true) {
                return false;
            }
            if (strconfirm == false) {
                if (document.getElementById("ctl00_CPHLAMBDA_rbtSpecific_0").checked == true) {
                    document.getElementById("flCRFUploadDoc").value = "";
                }
                if (document.getElementById("ctl00_CPHLAMBDA_rbtSpecific_1").checked == true) {
                    document.getElementById("ctl00_CPHLAMBDA_fuActivityFile").value = "";
                }
                getSpecification();
                return false;
            }
            return false;
        }
        function SuccessAlert() {
            msgalert("File Uploaded Successfully !");
            if (window.location.search.substring(1).split("=")[1] != "1") {
                if (document.getElementById("ctl00_CPHLAMBDA_rbtSpecific_0").checked == true) {
                    projectGrid();
                }
            }
            else if (window.location.search.substring(1).split("=")[1] == "1") {
                projectGrid();
            }
            return false;
        }
        function AudtiTrail(e) {
            var vActiviyId = "";
            var ActivityName = "";
            var iPeriod = "";
            var NodeId = ""
            var WorkspaceID = document.getElementById('<%= HProjectId.ClientID%>').value;

            /*Audit trail popup on click of "Audit Trial image" */

            vPeriod = $get('ctl00_CPHLAMBDA_ddlPeriod').options[$get('ctl00_CPHLAMBDA_ddlPeriod').selectedIndex].text;
            if (document.getElementById("ctl00_CPHLAMBDA_rbtSpecific_0").checked == true) {
                ActivityName = "";
            }
            if (document.getElementById("ctl00_CPHLAMBDA_rbtSpecific_1").checked == true) {
                populateGrid();
                vActiviyId = $("#" + e.id).attr("vActivityId");
                ActivityName = $("#" + e.id).attr("ActivityName");
                NodeId = $("#" + e.id).attr("NodeID");
            }
            if (document.getElementById("ctl00_CPHLAMBDA_txtProject").value != "") {
                $.ajax({
                    type: "post",
                    url: "frmUploadDocument.aspx/AuditTrail",
                    data: '{"vActiviyId":"' + vActiviyId + '","ActivityName":"' + ActivityName + '","vPeriod":"' + vPeriod + '","WorkspaceID":"' + WorkspaceID + '","NodeId":"' + NodeId + '"}',
                    contentType: "application/json; charset=utf-8",
                    datatype: JSON,
                    async: false,
                    success: function (data) {
                        $('#tblActivityAudit').attr("IsTable", "has");
                        var aaDataSet = [];
                        if (document.getElementById("ctl00_CPHLAMBDA_rbtSpecific_0").checked == true) {
                            if (data.d != "" && data.d != null) {
                                data = JSON.parse(data.d);
                                for (var Row = 0; Row < data.length; Row++) {
                                    var InDataSet = [];
                                    var FilePath = data[Row].vFilePath.split("/");
                                    var FileName = FilePath[4]
                                    InDataSet.push(data[Row].SrNo, data[Row].ProjectNo, FileName, data[Row].vRemark, data[Row].ModifyBy, data[Row].UploadedOn);
                                    aaDataSet.push(InDataSet);
                                }
                            }

                            if ($("#tblActivityAudit").children().length > 0) {
                                $("#tblActivityAudit").dataTable().fnDestroy();
                            }
                            $('#tblActivityAudit').prepend($('<thead>').append($('#tblActivityAudit tr:first'))).dataTable({
                                "bJQueryUI": true,
                                "sPaginationType": "full_numbers",
                                "bLengthChange": false,
                                "iDisplayLength": 10,
                                "bProcessing": true,
                                "bSort": false,

                                "aaData": aaDataSet,
                                "aoColumns": [
                                    { "sTitle": "Sr. No." },
                                    { "sTitle": "Project No" },
                                     { "sTitle": "File Name" },
                                     { "sTitle": "Remark" },
                                    { "sTitle": "Uploaded By" },
                                    { "sTitle": "Uploaded On" },
                                ],
                                "oLanguage": {
                                    "sEmptyTable": "No Record Found",
                                }
                            });
                            $find('mdlAuditActivity').show();
                        }
                        if (document.getElementById("ctl00_CPHLAMBDA_rbtSpecific_1").checked == true) {
                            if (data.d != "" && data.d != null) {
                                data = JSON.parse(data.d);
                                for (var Row = 0; Row < data.length; Row++) {
                                    var InDataSet = [];
                                    var FilePath = data[Row].vFilePath.split("/")
                                    var FileName = FilePath[6];
                                    InDataSet.push(data[Row].SrNo, data[Row].ProjectNo, data[Row].ActivityName, FileName, data[Row].vRemark, data[Row].ModifyBy, data[Row].UploadedOn);
                                    aaDataSet.push(InDataSet);
                                }
                            }

                            if ($("#tblActivityAudit").children().length > 0) {
                                $("#tblActivityAudit").dataTable().fnDestroy();
                            }
                            $('#tblActivityAudit').prepend($('<thead>').append($('#tblActivityAudit tr:first'))).dataTable({
                                "bJQueryUI": true,
                                "sPaginationType": "full_numbers",
                                "bLengthChange": false,
                                "iDisplayLength": 10,
                                "bProcessing": true,
                                "bSort": false,

                                "aaData": aaDataSet,
                                "aoColumns": [
                                    { "sTitle": "Sr. No." },
                                    { "sTitle": "Project No" },
                                    { "sTitle": "Activiy Name" },
                                     { "sTitle": "File Name" },
                                     { "sTitle": "Remark" },
                                    { "sTitle": "Uploaded By" },
                                    { "sTitle": "Uploaded On" },
                                ],
                                "oLanguage": {
                                    "sEmptyTable": "No Record Found",
                                }

                            });
                            $find('mdlAuditActivity').show();
                        }
                    },
                    failure: function (response) {
                        msgalert(response.d);
                    },
                    error: function (response) {
                        msgalert(response.d);
                    }
                });
            }
            //}
            return false;
        }
        function CheckFile(Cntrl) {
            var file = document.getElementById(Cntrl.name);
            var len = file.value.length;
            var ext = file.value;
            if (ext.substr(len - 3, len) != "pdf") {
                msgalert("Please select a doc or pdf file !");
                return false;
            }
        }
        function ResetPage(e) {
            if (document.getElementById("ctl00_CPHLAMBDA_txtProject").value != "") {
                document.getElementById("ctl00_CPHLAMBDA_gvActivityGrid").style.removeProperty("width");
                if ($('#ctl00_CPHLAMBDA_gvActivityGrid').attr("IsTable") == "has") {
                    $('#ctl00_CPHLAMBDA_gvActivityGrid').DataTable().fnDestroy();
                    $('#ctl00_CPHLAMBDA_gvActivityGrid').empty();
                    $('#ctl00_CPHLAMBDA_gvActivityGrid').removeAttr("IsTable");
                }
                if ($('#tblActivityAudit').attr("IsTable") == "has") {
                    $('#tblActivityAudit').DataTable().fnDestroy();
                    $('#tblActivityAudit').empty();
                    $('#tblActivityAudit').removeAttr("IsTable");
                }
            }
            document.getElementById("ctl00_CPHLAMBDA_txtProject").value = "";
            $('#ctl00_CPHLAMBDA_ddlPeriod').empty();
            document.getElementById("flCRFUploadDoc").value = "";
            getSpecification();
            if (document.getElementById("ctl00_CPHLAMBDA_txtProject") != "") {
                document.getElementById("ctl00_CPHLAMBDA_btnAudit").style.visibility = 'hidden';
            }
            if (e.id == "ctl00_CPHLAMBDA_btnCancle") {
                $('#ctl00_CPHLAMBDA_rbtSpecific_0').attr("checked", "checked");
            }

        }

        function CheckFileForUpload() {
            var file = document.getElementById("ctl00_CPHLAMBDA_flCRFUploadDoc").value;

            var len = file.value.length;
            var ext = file.value;
            if (file.value.length <= 0) {
                msgalert('Please select a file to import !');
                document.getElementById('FileUpload1').focus();
                return false;
            }
            else if (ext.substr(len - 3, len) != "pdf" || ext.substr(len - 3, len) != "jpg" || ext.substr(len - 3, len) != "jpeg" || ext.substr(len - 3, len) != "png") {
                msgalert("Please select [pdf-jpeg-jpg-png] formate only file !");
                return false;
            }
        }
        function ClearGrid() {
            $('#ctl00_CPHLAMBDA_gvActivityGrid').DataTable().fnDestroy();
            $('#ctl00_CPHLAMBDA_gvActivityGrid').empty();
            if (document.getElementById("ctl00_CPHLAMBDA_rbtSpecific_0").checked == true) {
                projectGrid();
            }
            if (document.getElementById("ctl00_CPHLAMBDA_rbtSpecific_1").checked == true) {
                populateGrid();
            }
        }
        function validate_fileupload(e) {
            var status = "";
            var allowed_extensions = new Array("jpg", "png", "gif", "pdf", "jpeg");
            var file_extension = e.value.split('.').pop();
            var FileName, FN, RemoveExt, FileLength;
            if (document.getElementById("ctl00_CPHLAMBDA_rbtSpecific_0").checked == true) {

                //FileName = document.getElementById("ctl00_CPHLAMBDA_flCRFUploadDoc").value;
                FileName = e.value;
                var FileN = FileName.substring(FileName.lastIndexOf('\\') + 1);
                RemoveExt = FileN.split(".");
                FileLength = RemoveExt[0].length;
                if (FileLength > 70) {
                    msgalert("File name must be in 70 character !");
                    //document.getElementById("ctl00_CPHLAMBDA_flCRFUploadDoc").value = "";
                    e.value = "";
                    return false;
                }
            }
            if (document.getElementById("ctl00_CPHLAMBDA_rbtSpecific_1").checked == true) {

                //FileName = document.getElementById("ctl00_CPHLAMBDA_fuActivityFile").value;
                FileName = e.value;
                var FileN = FileName.substring(FileName.lastIndexOf('\\') + 1)
                RemoveExt = FileN.split(".");
                FileLength = RemoveExt[0].length;

                if (FileLength > 50) {
                    msgalert("File name must be in 50 character !");
                    //document.getElementById("ctl00_CPHLAMBDA_fuActivityFile").value = "";
                    e.value = "";
                    return false;
                }
            }
            for (var i = 0; i <= allowed_extensions.length; i++) {
                if (allowed_extensions[i] == file_extension) {
                    status = "Allow";
                    return true; // valid file extension
                }
            }
            if (status != "Allow") {
                msgalert("You can only select file like [ jpg - pgn - gif - pdf - jpeg ] [ " + file_extension + " ] file is Not allow to upload");

                //document.getElementById("ctl00_CPHLAMBDA_flCRFUploadDoc").value = "";
                //document.getElementById("ctl00_CPHLAMBDA_fuActivityFile").value = "";
                e.value = "";
                return false
            }
            return false;
        }

        function AlertLocActivity() {
            msgalert("Project is lock.You can not upload Document !");
        }
        function RemoveCss() {
            document.getElementById("ctl00_CPHLAMBDA_gvActivityGrid").style.removeProperty("width");
        }
        function ClearFilePath() {
            var rowId = "";
            document.getElementById("ctl00_CPHLAMBDA_fuActivityFile").value = "";
            rowId = $("#ctl00_CPHLAMBDA_btnClear").attr("RowId");
            UploadActivityDocument(rowId);
            return false;
        }
        function ClearText() {
            document.getElementById("ctl00_CPHLAMBDA_fuActivityFile").value = "";
            return true;
        }
        function LockStatusforProject() {
            msgalert("Project is lock.You can not upload Document !");
            document.getElementById("ctl00_CPHLAMBDA_btnCancle").style.marginLeft = '525px';
            document.getElementById("ctl00_CPHLAMBDA_btnCancle").style.marginTop = '0px';
            document.getElementById("ctl00_CPHLAMBDA_btnUpload").style.display = 'none';
            return false;
        }

        function CheckValidation() {

            if (checkfile == "new") {
                document.getElementById('<%= btnRemarksUpdate.ClientID %>').click();
            }
            else {
                if (document.getElementById("ctl00_CPHLAMBDA_txtRemarks").value.trim() == "") {
                    msgalert("Please Enter Remark !");
                    return false;
                }

            }
          
            return true;
        }
        
    

    </script>

</asp:Content>

