<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmSubjectAssignment.aspx.vb" Inherits="frmSubjectAssignment" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
<style type="text/css">
        #ctl00_CPHLAMBDA_gvwWorkspaceSubjectMst {
            overflow:auto !important;
            display:block !important;
            max-height:500px;
            overflow-y :auto ;
            overflow-x :auto ;
        }

          #ctl00_CPHLAMBDA_GVAudit {
            overflow:auto !important;
            display:block !important;
            max-height:500px;
            overflow-y :auto ;
            overflow-x :auto ;
        }

          .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

    </style>
    <script src="Script/popcalendar.js" type="text/javascript"></script>
    <script src="Script/AutoComplete.js" type="text/javascript"></script>
    <script src="Script/General.js" type="text/javascript"></script>
    <script src="Script/Validation.js" type="text/javascript"></script>
    <script src="Script/jquery.dataTables.min.js" type="text/javascript"></script>
    <script src="Script/jquery.dataTables.plugins.js" type="text/javascript"></script>
    

    <script language="javascript" type="text/javascript">

        function pageLoad() {

            if ($get('<%= gvwWorkspaceSubjectMst.ClientID()%>') != null && $get('<%= gvwWorkspaceSubjectMst.ClientID%>_wrapper') == null) {
                if ($('#<%= gvwWorkspaceSubjectMst.ClientID%>')) {
                    jQuery('#<%= gvwWorkspaceSubjectMst.ClientID%>').prepend($('<thead>').append($('#<%= gvwWorkspaceSubjectMst.ClientID%> tr:first'))).DataTable({
                        "bJQueryUI": true,
                        "bPaginate": true,
                        "bFooter": false,
                        "bHeader": false,
                        "AutoWidth": true,
                        "bSort": false,
                        "fixedHeader": true,
                        "oLanguage": { "sSearch": "Search" },
                        "aLengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
                        "iDisplayLength": 10,
                        "sPaginationType": "full_numbers",
                        "oLanguage": {
                            "sEmptyTable": "No Record Found",
                        }
                    });

                }
                //$(".dataTables_wrapper").css("width", ($(window).width() * 0.99 | 0) + "px");
            }



            if ($get('<%= GVAudit.ClientID()%>') != null && $get('<%= GVAudit.ClientID%>_wrapper') == null) {
                if ($('#<%= GVAudit.ClientID%>')) {
                    jQuery('#<%= GVAudit.ClientID%>').prepend($('<thead>').append($('#<%= GVAudit.ClientID%> tr:first'))).DataTable({
                        "bJQueryUI": true,
                        "bPaginate": true,
                        "bFooter": false,
                        "bHeader": false,
                        "AutoWidth": true,
                        "bSort": false,
                        "fixedHeader": true,
                        "oLanguage": { "sSearch": "Search" },
                        "aLengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
                        "iDisplayLength": 10,
                        "sPaginationType": "full_numbers",
                        "oLanguage": {
                            "sEmptyTable": "No Record Found",
                        }
                    });

                }
                //$(".dataTables_wrapper").css("width", ($(window).width() * 0.99 | 0) + "px");
            }

            $('input[name*=rblScreeningDate]:enabled').bind("click", function (e) {
                CheckScreenDate();
            });



        }
        //======================================

        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        //For rejection Validation       
        function rejectValidation(e) {

            var Length = document.getElementById('ctl00_CPHLAMBDA_ddlSubject').length;
            if (document.getElementById('<%= txtMySubNo.ClientId %>').value.toString().trim() == '') {
                msgalert('Please enter Replaced MySubject No !');
                document.getElementById('<%= txtMySubNo.ClientId %>').focus();
                return false;
            }
            else if (document.getElementById('<%= ddlReject.ClientId %>').selectedIndex == 0) {
                msgalert('Please select Reason To Replace !');
                document.getElementById('<%= ddlReject.ClientId %>').focus();
                return false;
            }
            else if (document.getElementById('<%= ddlSubject.ClientId %>').selectedIndex == 0) {
                msgConfirmDeleteAlert(null, "You Have Not Selected Any Subject To Replace. Do you want To Continue?", function (isConfirmed) {
                    if (isConfirmed) {
                        __doPostBack(e.name, '');
                        return true;
                    } else {
                        document.getElementById('<%= ddlSubject.ClientId %>').focus();
                        return false;
                    }
                });
            }
        }

        function DivSearchShowHide(Type) {
            if (Type == 'S') {
                document.getElementById('<%=divSearch.ClientId %>').style.display = 'block';
                SetCenter('<%=divSearch.ClientId %>');
                return false;
            }
            else if (Type == 'H') {
                document.getElementById('<%=divSearch.ClientId %>').style.display = 'none';
                    return false;
                }
            return true;
        }

        function CheckScreenDate() {          
            var i = 0, j = 0;
            $('#ctl00_CPHLAMBDA_rblScreeningDate input').each(function () {
                if ($(this).attr('checked') == "checked" || $(this).attr('checked') == true) {
                    j = i;
                }
                i = i + 1;
            }
            );
            if (j != 0) {
                msgalert('You have not selected latest Screening. Make sure that selected Screening date is correct !');
            }
        }

        function DivTextValidation() {
            if (document.getElementById('<%= txtRemark.ClientId %>').value.toString().trim() == '') {
                msgalert('Please enter Remarks !');
                document.getElementById('<%= txtRemark.ClientId %>').focus();
                return false;
            }
        }

        function CheckDays(obj) {
            if (obj.value == '') {
                msgalert("Screening Validation Days cannot be Null or blank !");
                obj.value = 28;
            }
            else if (!checkVal(obj.value, obj.id, '2')) {
                msgalert("Please enter Days in Numeric !");
                obj.value = 28;
            }
        }

        function fnValidateReject(subjectNo) {
            window.MySubjectNo = subjectNo;            
        }

        function validation() {
            var content = {};
            content.MedExScreeningHdrNo = $("input:radio[name=ctl00$CPHLAMBDA$rblScreeningDate]:checked").val()

            $.ajax({
                type: "POST",
                url: "frmsubjectAssignment.aspx/SubjectScreeningDCFValidation",
                data: JSON.stringify(content),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var aaDataSet = [];

                    if (data.d != "" && data.d != null) {
                        data = JSON.parse(data.d);
                        if (data == "Validated") {
                            var btn = document.getElementById('<%= btnOk2.ClientID%>');
                            btn.click();
                            return true;
                        }
                        else if (data == "Re-Review Not Done") {
                            msgalert("Re-Review Not Completed.Please Re-reviewed Screening!")
                            return false;
                        }
                        else if (data == "Not Validated") {
                            msgalert("You Can Not Assign Subject DCF is Pending Or Screening Not Freezed!")
                            return false;
                        }
                    }
                    else {
                        return false;
                    }
                },
                failure: function (error) {
                    msgalert(error);
                }
            });


        }
    </script>

    <asp:UpdatePanel ID="upPnlWorkspaceSubjectMst" runat="server">
        <ContentTemplate>
            <table style="width: 100%">
                <tbody>
                    <tr style="vertical-align: top;">
                        <td style="vertical-align: top;">
                            <button id="btnRejsctSubject" runat="server" style="display: none;" />
                            <cc1:ModalPopupExtender ID="modalrejectsubject" runat="server" BackgroundCssClass="modalBackground"
                                CancelControlID="ImgCloseRejactPopup" PopupControlID="divRejectSubject" PopupDragHandleControlID="lblrejectsubject"
                                TargetControlID="btnRejsctSubject">
                            </cc1:ModalPopupExtender>
                            <div id="divRejectSubject" runat="server" class="centerModalPopup" style="display: none;
                                left: 521px; width: 55%; position: absolute; top: 525px; max-height: 404px;">
                                <table style="width: 100%;">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <h1 class="header">
                                                    <strong class="LabelPeru">
                                                        <asp:Label ID="lblrejectsubject" Text="Replace  Subject" runat="server" class="LabelBold" /></strong>
                                                    <img id="ImgCloseRejactPopup" alt="Close" src="images/Sqclose.gif" style="position: relative;
                                                        float: right; right: 5px;" title="Close" />
                                                </h1>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table style="width: 90%; margin: 0 auto;">
                                                    <tbody>
                                                        <tr id="trAddRejectSubject" runat="server" visible="false">
                                                            <td>
                                                                <table cellpadding="5" style="text-align: left;">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td class="Label" style="text-align: right; width: 40%;">
                                                                                Subject Name:
                                                                            </td>
                                                                            <td style="text-align: left; width: 60%;">
                                                                                <asp:Label ID="lblSubjectName" runat="server" />
                                                                                <asp:HiddenField ID="hdnMySubjectNo" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="Label" style="text-align: right;">
                                                                                Replace With:
                                                                            </td>
                                                                            <td style="text-align: left;">
                                                                                <asp:DropDownList ID="ddlSubject" runat="server" CssClass="dropDownList" Width="387px" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="Label" style="text-align: right;">
                                                                                Replaced MySubject No:
                                                                            </td>
                                                                            <td style="text-align: left;">
                                                                                <asp:TextBox ID="txtMySubNo" TabIndex="11" runat="server" CssClass="textBox" Width="140px" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="Label" style="text-align: right;">
                                                                                Replace Reason*:
                                                                            </td>
                                                                            <td style="text-align: left;">
                                                                                <asp:DropDownList ID="ddlReject" runat="server" CssClass="dropDownList" Width="389px" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr style="display: none" id="divRejectText">
                                                                            <td class="Label" style="text-align: right;">
                                                                                Enter Reason*:
                                                                            </td>
                                                                            <td style="text-align: left;">
                                                                                <asp:TextBox ID="txtSubjectRemark" runat="server" Width="394px" TextMode="MultiLine"
                                                                                    Height="93px" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Replace" CssClass="btn btnsave"
                                                                                    Width="175px" OnClientClick="return rejectValidation(this);" ToolTip="Replace" />
                                                                                <asp:Button ID="btnClose" OnClick="btnClose_Click" runat="server" Text=" Close "
                                                                                    CssClass="btn btnexit" ToolTip="Close" />
                                                                            </td>
                                                                        </tr>                                                                       
                                                                    </tbody>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top">
                            <button id="btnScreeningdate" runat="server" style="display: none;" />
                            <cc1:ModalPopupExtender ID="MPEdate" runat="server" PopupControlID="divSearch" PopupDragHandleControlID="LabelDate"
                                BackgroundCssClass="modalBackground" TargetControlID="btnScreeningdate" CancelControlID="ImgPopUpCloseDate"
                                BehaviorID="MPEDeviation">
                            </cc1:ModalPopupExtender>
                            <div id="divSearch" runat="server" class="centerModalPopup" style="display: none;
                                left: 521px; width: 55%; position: absolute; top: 525px; max-height: 404px;">
                                <table style="width: 100%">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <h1 class="header">
                                                    <strong class="LabelPeru ">
                                                        <asp:Label ID="LabelDate" runat="server" Text="Screening Date" />
                                                    </strong>
                                                    <img id="ImgPopUpCloseDate" src="images/Sqclose.gif" style="width: 24px; height: 15px"
                                                        alt="close" title="Close" />
                                                </h1>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblNote" runat="server" Text="NOTE:To Use Disabled Screenings,Please Declare Eligibility Along With PI review In Respective Screening Forms"
                                                    class="LabelBold" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table style="width: 100%" cellpadding="5px">
                                                    <tbody>
                                                        <tr style="width: 100%">
                                                            <td style="width: 30%; text-align: right;" class="Label">
                                                                Select Screening:
                                                            </td>
                                                            <td style="width: 70%" align="left">
                                                                <asp:Panel ID="pnlSubject" runat="server" ScrollBars="Auto" BorderStyle="Solid" BorderWidth="1px">
                                                                    <asp:RadioButtonList ID="rblScreeningDate" AutoPostBack="false" 
                                                                        runat="server" CssClass="Label" RepeatDirection="Horizontal" />
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" style="width: 100%; text-align: center;">
                                                                <asp:Button ID="btnOK" runat="server" CssClass="btn btnnew" Text="OK" ToolTip="OK" onClientClick="return validation();" />
                                                                <asp:Button ID="btnOk2" style="display:none" runat="server" CssClass="btn btnnew" Text="OK" ToolTip="OK" />
                                                                <asp:Button ID="btnScreenClose" runat="server" CssClass="btn btnexit" Text="Close" ToolTip="Close" />
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <button id="btn1" runat="server" style="display: none;" />
                            <cc1:ModalPopupExtender ID="mpeDialog" runat="server" BackgroundCssClass="modalBackground"
                                CancelControlID="dialogModalClose" PopupControlID="dialogModal" PopupDragHandleControlID="dialogModalTitle"
                                TargetControlID="btn1">
                            </cc1:ModalPopupExtender>
                            <div id="dialogModal" runat="server" class="centerModalPopup" style="display: none;
                                left: 521px; width: 25%; position: absolute; top: 525px; max-height: 404px;">
                                <div>
                                    <img id="dialogModalClose" alt="Close" src="images/Sqclose.gif" style="position: relative;
                                        float: right; right: 5px;" title="Close" />
                                    <asp:Label ID="dialogModalTitle" runat="server" class="LabelBold" />
                                </div>
                                <table style="width: 40%">
                                    <tbody>
                                        <tr style="width: 100%">
                                            <td style="width: 35%; height: 48px">
                                                <strong style="white-space: nowrap">Remarks</strong>
                                            </td>
                                            <td style="width: 65%; height: 48px">
                                                <asp:TextBox ID="txtRemark" runat="server" CssClass="textBox" Height="38px" TabIndex="2"
                                                    Width="176px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center" colspan="2">
                                                <asp:Button ID="btnDivOK" runat="server" CssClass="btn btnnew" Text="OK" wfdid="w31"
                                                    OnClientClick="return DivTextValidation();" ToolTip="OK" />
                                                &nbsp;<asp:Button ID="btnDivCancel" runat="server" CssClass="btn btncancel" Text="Cancel"
                                                    wfdid="w31" ToolTip="Text" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <button id="btn2" runat="server" style="display: none;" />
                            <cc1:ModalPopupExtender ID="MpeAudit" runat="server" BackgroundCssClass="modalBackground"
                                CancelControlID="imgbtnMpeAuditClose" PopupControlID="DivAudit" TargetControlID="btn2"
                                BehaviorID="MpeAudit">
                            </cc1:ModalPopupExtender>
                            <div id="DivAudit" runat="server" class="centerModalPopup" style="display: none;
                                width: 75%; position: absolute; top: 525px; max-height: 404px;">
                                <div style="width: 90%; margin: auto">
                                    <asp:Label ID="lblTitle" Text="Audit Trail" runat="server" class="Label" />
                                    <asp:ImageButton ID="imgbtnMpeAuditClose" runat="server" ImageUrl="images/Sqclose.gif"
                                        Style="position: relative; float: right; right: 5px;" AlternateText="Close" />
                                    <hr />
                                </div>
                                <div style="width: 90%; margin: auto; overflow: auto; margin-bottom: 2%;">
                                    <asp:GridView ID="GVAudit" runat="server" AutoGenerateColumns="False" BorderColor="Peru"
                                        Font-Size="Small" SkinID="grdViewSmlAutoSize">
                                        <Columns>
                                            <asp:BoundField DataField="iAsnNo" HeaderText="AsnNo" />
                                            <asp:BoundField DataField="vInitials" HeaderText="Initials">
                                                <ItemStyle Width="20%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="vSubjectID" HeaderText="Subject ID">
                                                <ItemStyle Width="20%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="vMySubjectNo" HeaderText="MySubject No">
                                                <ItemStyle Width="20%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="dReportingDate" HeaderText="Reporting Date" HtmlEncode="False">
                                                <ItemStyle Wrap="false" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="vModifyBy" HeaderText="Modify By">
                                                <ItemStyle Width="20%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="dModifyOn" HeaderText="Modify On" HtmlEncode="False">
                                                <ItemStyle Wrap="false" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="vRemarks" HeaderText="Remarks">
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="nWorkspaceSubjectHistoryId" HeaderText="Sample Id" />
                                            <asp:BoundField DataField="vWorkspaceSubjectId" HeaderText="iReviewedBy" />
                                            <asp:BoundField DataField="iTranNo" HeaderText="TranNo" />
                                            
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left;">
                        <fieldset class="FieldSetBox" style="display: inline-block; margin-left:50px; width: 90%; text-align: left; border: #aaaaaa 1px solid;">
                            <legend class="LegendText" style="color: Black; font-size: 12px">
                                <img id="imgfldgen" alt="Screening DCF Report" src="images/panelcollapse.png"
                                    onclick="DispalyDCFReport(this,'tblEntryData');" runat="server" style="margin-right: 2px;" />Project Detail</legend>
                                <div id="tblEntryData">
                                    <table cellpadding="3px" style="width: 100%">
                                <tbody>
                                    <tr>
                                        <td style="text-align: right; width: 30%">
                                            <asp:Label ID="lblProject" runat="server" Text="Project Name/Project No :"></asp:Label>
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="50%" />
                                            <asp:Button Style="display: none" ID="btnSetProject" OnClick="btnSetProject_Click"
                                                runat="server" Text=" Project" ToolTip="Project" /><asp:HiddenField ID="HProjectId"
                                                    runat="server" />
                                            <asp:HiddenField ID="HParentWorkSpaceId" runat="server" />
                                            <asp:HiddenField ID="HIsTestSite" runat="server" />
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                                                TargetControlID="txtProject" ServicePath="AutoComplete.asmx" ServiceMethod="GetMyProjectCompletionList"
                                                OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1"
                                                CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                CompletionListElementID="pnlProjectList" CompletionListCssClass="autocomplete_list"
                                                BehaviorID="AutoCompleteExtender1" />
                                            <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 300px; overflow: auto;
                                                overflow-x: hidden" />
                                        </td>
                                    </tr>
                                    <tr style="display: none;">
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblScreeningDys" runat="server" Text="Screening Validation Days*:"></asp:Label>
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtScreenDays" runat="server" Enabled="false" onblur="CheckDays(this);"
                                                CssClass="textBox" Width="36px">28</asp:TextBox>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                                </div>
                           </fieldset>
                        </td>
                    </tr>
                    
                    <tr id="Tr1" runat="server" style="margin-top :20px !important;">
                        
                       <td style="text-align: left;">
                           <fieldset class="FieldSetBox" style="display: inline-block; margin-top:10px; margin-left:50px; width: 90%; text-align: left; border: #aaaaaa 1px solid;">
                             <legend class="LegendText" style="color: Black; font-size: 12px">
                                <img id="img1" alt="Screening DCF Report" src="images/panelcollapse.png"
                                    onclick="DispalyDCFReport(this,'tblEntryDataRecord');" runat="server" style="margin-right: 2px;" />Assignment Detail</legend>
                                <div id="tblEntryDataRecord">
                                <div style="width: 98%; margin: auto; padding-top: 5px;">
                                 <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvwWorkspaceSubjectMst" runat="server" AutoGenerateColumns="False"
                                            OnRowCommand="gvwWorkspaceSubjectMst_RowCommand"
                                            OnRowCreated="gvwWorkspaceSubjectMst_RowCreated" OnRowDataBound="gvwWorkspaceSubjectMst_RowDataBound" style="width:100%;">
                                            <Columns>
                                                <asp:BoundField HeaderText="#">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vSubjectId" HeaderText="Subject ID">
                                                    <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="iMySubjectNo" HeaderText="SysSubect No." />
                                                <asp:BoundField DataField="Subject" HeaderText="Subject">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="My Subject No">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtMySUbjectNo" runat="server" MaxLength="4" Style="width: 60px" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="vInitials" HeaderText="Initials">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="dReportingDate" HeaderText="Reporting Date"  HtmlEncode="False">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="iScrDays" HeaderText="Screening Validation Days" />
                                                <asp:BoundField DataField="dScreenDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Screen Date"
                                                    HtmlEncode="False">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Save" SortExpression="status">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgSave1" runat="server" ImageUrl="~/images/save.gif" AlternateText="Save"
                                                            ToolTip="Save" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Extra" SortExpression="status">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgExtra" runat="server" ImageUrl="~/images/extra.png" AlternateText="Extra"
                                                            ToolTip="Extra" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Replace">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgBtnReject" runat="server" ImageUrl="~/images/rejectreplace.png"
                                                            AlternateText="Replace" ToolTip="Replace" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="vMySubjectNo" HeaderText="vMySubjectNo" />
                                                <asp:BoundField DataField="vWorkspaceSubjectId" HeaderText="WorkspaceSubjectId">
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Assign Screen Date" SortExpression="status">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgScrDate" runat="server" ImageUrl="~/images/screendate.png"
                                                            AlternateText="Assign Screen Date" ToolTip="Assign Screen Date" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="nMedExScreeningHdrNo" HeaderText="MedExScreeningHdrNo">
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Edit" SortExpression="status">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgEdit" runat="server" ImageUrl="~/images/Edit2.gif" AlternateText="Edit"
                                                            ToolTip="Edit" />
                                                        <asp:ImageButton ID="ImgSave" runat="server" ImageUrl="~/images/save.gif" AlternateText="Save"
                                                            ToolTip="Save" />
                                                        <asp:ImageButton ID="ImgCancel" runat="server" ImageUrl="~/images/Cancel.gif" AlternateText="Cancel"
                                                            ToolTip="Cancel" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Audit Trail">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgAudit" runat="server" ImageUrl="~/Images/audit.png" Visible="true"
                                                            AlternateText="Audit Trail" ToolTip="Audit Trail" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:BoundField DataField="dReportingDate"  HtmlEncode="False">
                                                <ItemStyle Wrap="false" />
                                            </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnOK" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                                    </div>
                          </fieldset>
                        </td>
                                
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <asp:Button ID="btnCancel" OnClick="btnCancel_Click1" runat="server" Text="Cancel"
                                CssClass="btn btncancel" ToolTip="Cancel" />
                            <asp:Button ID="btnExit" OnClick="btnExit_Click" runat="server" Text="Exit" CssClass="btn btnexit"
                                OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" ToolTip="Exit" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
