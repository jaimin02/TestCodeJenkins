<%@ Page Language="VB" MasterPageFile="~/ECTDMasterPage.master" AutoEventWireup="false"
    CodeFile="frmCRFPrint.aspx.vb" Inherits="frmCRFPrint" ValidateRequest="false"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <link href="App_Themes/StyleBlue/StyleBlue.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="App_Themes/jqueryui.css" />
    <link rel="stylesheet" href="App_Themes/CDMS.css" />
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    
    <style type="text/css">
        .FieldSetBox { border: #aaaaaa 1px solid; z-index: 0px; border-radius: 4px; }
        .textBox {color: Black !important;font-family: Delicious, sans-serif !important;font-size: 12px !important;}
    </style>

    <asp:UpdatePanel runat="server" ID="UpPnlForCRFPrint">
        <ContentTemplate>
              <table width="100%" cellpadding="0" cellspaceing="0" style="margin-top: 20px;">
                <tr>
                    <td style="width:30%; vertical-align:top;">
                            <table width="100%" cellpadding="0" cellspaceing="0">
                            <tr>
                                <td>
                                       <fieldset class="FieldSetBox" style="text-align: left; width:99%; max-width: 99%">
                                        <legend class="LegendText" style="color: Black;">Project Details </legend>
                                        <table width="100%" cellpadding="3">
                                            <tr>
                                               <td style="text-align: right;width:23%;" class="LabelText">Project Name/Request Id :
                                                </td>
                                                 <td style="text-align: left;" colspan="3">
                                                 <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="350px" style="margin-top:14px"></asp:TextBox>&nbsp;<asp:Button
                                                                Style="display: none" ID="btnSetProject" OnClick="btnSetProject_Click" runat="server"
                                                                Text=" Project"></asp:Button>
                                                            <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
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
                                                <td style="text-align: right;" class="LabelText">Activity Type :
                                                </td>
                                                  <td style="text-align: left;width:165px;" class="LabelText">
                                                     <asp:DropDownList ID="ddlActivtyType" runat="server"  AutoPostBack="true" Width="100%">
                                                                <asp:ListItem Selected="True">Select Activity </asp:ListItem>
                                                                <asp:ListItem>Generic Activities</asp:ListItem>
                                                                <asp:ListItem>Subject Specific Activities</asp:ListItem>
                                                                <asp:ListItem>All Activities</asp:ListItem>

                                                    </asp:DropDownList>
                                                </td>
                                                <td style="text-align: left;" class="LabelText">Subject :
                                                </td>
                                                <td style="text-align: left; width:125px;" class="LabelText" >
                                                         <asp:DropDownList runat="server" ID="ddlSubject" Width="100%" onchange="DisablePdfGeneration();"></asp:DropDownList>
                                                </td>
                                                            
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <table id="tblactivity" runat="server">
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                      <td style="text-align: left;" class="LabelText">Activity
                                                </td>
                                                <td style="text-align: right;" class="LabelText" colspan="3">
                                                <asp:CheckBox ID="ChkBoxSelectAll" runat="server"  Text="Select All" onclick="SelectAllFields();" />
                                                </td>
                                            </tr>
                                            <tr>
                                                        <td align="left" valign="top" class="LabelText" style="vertical-align: top;" colspan="4" >
                                                            <asp:Panel runat="server" ID="PnlForChkListBox" CssClass="FieldSetBox" Height="200px" ScrollBars="Auto" Width="100%" Style="text-align: left">
                                                                <asp:CheckBoxList ID="ChkListBoxActivity" runat="server" onchange="DisablePdfGeneration();">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                        </td>
                                            </tr>
                                            <tr>
                                                        <td class="LabelText" nowrap="nowrap" style="text-align: left" colspan="4">
                                                            <asp:RadioButtonList ID="RBLProjecttype" runat="server" AutoPostBack="True" Width="289px"
                                                                RepeatDirection="Horizontal">
                                                                <asp:ListItem Selected="true" Value="000000">Normal CRF Print</asp:ListItem>
                                                                <asp:ListItem Value="1">Annotated CRF Print</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                            </tr>
                                             <tr>
                                                <td style="height:10px;" colspan="4">
                                                </td>
                                             </tr>
                                            <tr>
                                                <td colspan="4" style="text-align:center;">
                                                      <asp:Button ID="btnGo" runat="server" CssClass="btn btngo" 
                                                           Style="font-size: 12px !important;" Text="" 
                                                           OnClientClick="return validationBlankProjectAndActivity();" />
                                                       
                                                       <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" 
                                                            Style="font-size: 12px !important;" Text="Cancel" />
                                                       
                                                       <asp:Button ID="btnExit" runat="server" Text="Exit" CssClass="btn btnexit"
                                                            Style="font-size: 12px !important;" CausesValidation="False"
                                                            OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this); " />
                                                </td>
                                            </tr>
                                            </table>
                                            </fieldset>
                                </td>
                               </tr>
                              </table>                 
                    </td>
                    <td style="width:70%;padding-left:1.5%;vertical-align:top;">
                     <table width="100%" cellpadding="0" cellspaceing="0">
                            <tr>
                                <td>
                                       <fieldset class="FieldSetBox" style="text-align: left; width:97%; max-width: 97%">
                                        <legend class="LegendText" style="color: Black;">CRF Structure</legend>
                                            <table width="100%" cellpadding="3">
                                                <tr>
                                                    <td style="float:right;">
                                                          <asp:Button ID="BtnGeneratePdf" runat="server"
                                                            Style="font-size: 12px !important; display: block;"
                                                            OnClientClick="return validateForm();" CssClass="btn btnpdf" />
                                                          <asp:HiddenField ID="HFHeaderLabel" runat="server" />
                                                          <asp:HiddenField ID="HFHeaderPDF" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                   <td>
                                                       <div style="text-align: center; overflow: auto; height: 500px; display: none;" id="HeaderLabel"
                                                    runat="server" align="center">
                                                    <table style="width: 100%" align="center" id="MainContentTable" runat="server">
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
                       <div id="divHeaderPDF" style="display: none;" runat="server" align="left" enableviewstate="false">
                                    <table style="padding: 2px; margin: auto; border: solid 1px black; width: 91%; font-family: Verdana;"
                                        align="left">
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td colspan="4" style="vertical-align: top;">
                                                            <table>
                                                                <tr>
                                                                    <td style="vertical-align: top; font-size: larger; font-weight: 900">
                                                                        <span id="spnhdr" runat="server"></span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="vertical-align: top; font-size: larger; font-weight: 700">
                                                                        <span id="spnBaBe" runat="server"></span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                </tr>
                                                                <tr>
                                                                    <td style="vertical-align: bottom; font-size: medium; font-weight: 600; text-align: right">
                                                                        CASE REPORT FORM
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            Project No:
                                                        </td>
                                                        <td style="border: thin solid #000000;">
                                                            <asp:Label ID="lblProjectNo" runat="server" Text="_" EnableViewState="false"></asp:Label>
                                                        </td>
                                                        <td id="tdSiteName" align="right" runat="server" enableviewstate="false">
                                                            <span id="SpnSite" runat="server" enableviewstate="false">Site Id: </span>
                                                        </td>
                                                        <td id="tdSiteId" runat="server" style="border: thin solid #000000;" enableviewstate="false">
                                                            <asp:Label ID="lblSiteNo" runat="server" Text="_" EnableViewState="false"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr id="trProtocol" runat="server" enableviewstate="false">
                                                        <td align="right">
                                                            <span id="Span1" runat="server" enableviewstate="false">Protocol No: </span>
                                                        </td>
                                                        <td style="border: thin solid #000000;">
                                                            <asp:Label ID="LblProtocol" runat="server" Text="_" EnableViewState="false"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <span id="SpnSubject" runat="server" enableviewstate="false">Subject No: </span>
                                                        </td>
                                                        <td style="border: thin solid #000000;">
                                                            <asp:Label ID="lblSubjectNo" runat="server" Text="_" EnableViewState="false"></asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <span id="SpnSubjectInit" runat="server" enableviewstate="false">Subject Initials:
                                                            </span>
                                                        </td>
                                                        <td style="border: thin solid #000000;">
                                                            <asp:Label ID="lblSubjectName" runat="server" Text="_" EnableViewState="false"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td valign="top">
                                                <img alt="" runat="server" id="ImgLogo" src="images/client_logo.png" enableviewstate="false" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                                   </td>
                                                </tr>
                                            </table>
                                       </fieldset>
                                </td>
                            </tr>
                     </table>
                      
                      
                    </td>
                </tr>
               </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnGo" />
            <asp:PostBackTrigger ControlID="BtnGeneratePdf" />
        </Triggers>
    </asp:UpdatePanel>

    <script type="text/javascript">

        $(document).ready(function() {
            $('#canal').css('display', 'none');
            $('#divActivityLegends').css('display', 'none');
        })
        function validateForm() {
            document.getElementById('<%= HFHeaderLabel.ClientId %>').value = document.getElementById('<%=HeaderLabel.ClientId %>').innerHTML;
            document.getElementById('<%= HFHeaderPDF.ClientId %>').value = document.getElementById('<%=divHeaderPDF.ClientId %>').innerHTML;
            return true;
        }

        function SelectAllFields() {
            document.getElementById('<%= BtnGeneratePdf.ClientId%>').style.display = 'none';

            var chkSelectAll = document.getElementById('<%= ChkBoxSelectAll.ClientId%>').checked;
            var chklst = document.getElementById('<%= ChkListBoxActivity.ClientId%>');
            var chks;
            var result = false;
            var i;
            if (chklst != null && typeof (chklst) != 'undefined') {
                chks = chklst.getElementsByTagName('input');
                if (chkSelectAll == true) {
                    for (i = 0; i < chks.length; i++) {
                        chks[i].checked = true;
                    }
                }
                else {
                    for (i = 0; i < chks.length; i++) {
                        chks[i].checked = false;
                    }
                }
            }
            return false;
        }

        function ClientPopulated(sender, e) {

            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));

            document.getElementById('<%= ChkBoxSelectAll.ClientId%>').checked = false;

        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));

        }
        function DisablePdfGeneration() {
            document.getElementById('<%= BtnGeneratePdf.ClientId%>').style.display = 'none';
        }

        function validationBlankProjectAndActivity() {
            if (document.getElementById('<%= txtProject.ClientId%>').value == "" || document.getElementById('<%= HProjectId.ClientId%>').value == "") {
                msgalert("Please Select Project Name/Request Id !");
                return false;
                document.getElementById('<%= ChkBoxSelectAll.ClientId%>').checked = false;
            }
            else {
                
                if(document.getElementById('<%= ddlActivtyType.ClientId %>').selectedIndex === 0)
                {
                    msgalert('Please Select Activity Type !');
                    return false;
                }
                
                var chklst = document.getElementById('<%= ChkListBoxActivity.ClientId%>');
                var chks = chklst.getElementsByTagName('input');
                
                for (i = 0; i < chks.length; i++) {
                    if (chks[i].checked == true) {

                        document.getElementById('<%= BtnGeneratePdf.ClientId%>').style.display = 'block';

                        return true;
                    }
                }
                document.getElementById('<%= ChkBoxSelectAll.ClientId%>').checked = false;
                msgalert("Please Select Atleast One Activity !");
                return false;

            }
        }
        function resetAllCheckBox() {
            document.getElementById('<%= ChkBoxSelectAll.ClientId%>').checked = false;
            var chklst = document.getElementById('<%= ChkListBoxActivity.ClientId%>');

            var chks = chklst.getElementsByTagName('input');

            for (i = 0; i < chks.length; i++) {
                chks[i].checked = false;
            }
        }
    </script>

</asp:Content>
