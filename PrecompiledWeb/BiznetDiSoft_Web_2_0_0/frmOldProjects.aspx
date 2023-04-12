<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmOldProjects, App_Web_vq2225em" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <div>
        <table width ="100%" cellpadding ="5PX">
            <tr>
                <td style="width: 40%; white-space: nowrap; text-align :right ;" class="Label">
                    Search Project :
                </td>
                <td  style ="text-align :left ;">
                    <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="45%"></asp:TextBox>
                    <asp:Button ID="btnSetProject" runat="server" OnClick="btnSetProject_Click" Style="display: none"
                        Text=" Project" />
                    <asp:HiddenField ID="HProjectId" runat="server" />
                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                        TargetControlID="txtProject" ServicePath="AutoComplete.asmx" ServiceMethod="GetOldProjectsList"
                        OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1"
                        CompletionListElementID="pnlProjectList" CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                        CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1">
                    </cc1:AutoCompleteExtender>
                    <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 300px; overflow: auto;
                            overflow-x: hidden" />
                </td>
            </tr>
            <tr>
                <td style ="text-align :center ;" colspan="2">
                  <asp:Button ID="BtnExit" runat="server" Text="Exit"  ToolTip =" Exit" CssClass="btn btnexit" OnClientClick ="return confirm ('Are You Sure You Want To Exit')" />
                </td>
            </tr>
        </table>
    </div>
  
    <asp:UpdatePanel ID="UpPane_Edit" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <div style ="width :100%; margin :auto;">
                <asp:Panel ID="PnlOldProjects" runat="server" Visible="false" BorderColor="blue"
                    BorderWidth="2" BorderStyle="Double" Width ="80%" style ="margin :auto;">
                    <table width ="100%">
                        <tr>
                            <td style ="text-align :right ; width :20%;" class ="Label">
                                Project No:
                            </td>
                           <td style ="text-align :left ; ">
                                <asp:TextBox ID="TxtProjectNo" runat="server" Enabled="false" Width ="70%" />
                            </td>
                            <td  style =" text-align :right ;" class ="Label">
                                Period 1 Check In Date :
                            </td>
                            <td style ="text-align :left ; width :30%;">
                                <asp:TextBox ID="TxtPeriod1CheckInDate" runat="server" Width ="70%" />
                                <cc1:CalendarExtender ID="CalExtPeriod1CheckInDate" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="TxtPeriod1CheckInDate">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style ="text-align :right ; " class ="Label ">
                                Number Of Subjects :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtNoOfSubjects" runat="server" Width ="70%" />
                            </td>
                            <td style ="text-align :right ; " class ="Label ">
                                Period 2 Check In Date :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtPeriod2CheckInDate" runat="server" Width ="70%" />
                                <cc1:CalendarExtender ID="CalExPeriod2CheckInDate" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="TxtPeriod2CheckInDate">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style ="text-align :right ; " class ="Label ">
                                Submission:
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtSubmission" runat="server" Width ="70%" />
                            </td>
                            <td style ="text-align :right ; " class ="Label ">
                                Period 3 Check In Date :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtPeriod3CheckInDate" runat="server" Width ="70%" />
                                <cc1:CalendarExtender ID="CalExPeriod3CheckInDate" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="TxtPeriod3CheckInDate">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style ="text-align :right ; " class ="Label ">
                                Study Description :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtStudyDescription" runat="server" Width ="70%" />
                            </td>
                            <td style ="text-align :right ; " class ="Label ">
                                Period 4 Check In Date :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtPeriod4CheckInDate" runat="server" Width ="70%" />
                                <cc1:CalendarExtender ID="CalExPeriod4CheckInDate" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="TxtPeriod4CheckInDate">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style ="text-align :right ; " class ="Label ">
                                Product Name :
                            </td>
                            <td style ="text-align :left ;" colspan ="3">
                                <asp:TextBox ID="TxtProductName" runat="server" Width ="27%" />
                            </td>
                           
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td style ="text-align :left ;">
                                <asp:DropDownList ID="DdllstForDrugName" runat="server" AutoPostBack="true" Width ="70%" />
                              
                            </td>
                            <td style ="text-align :right ; " class ="Label ">
                                Period 1 Dosing Date :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtPeriod1DosingDate" runat="server" Width ="70%" />
                                <cc1:CalendarExtender ID="CalExPeriod1DosingDate" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="TxtPeriod1DosingDate">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style ="text-align :right ; " class ="Label ">
                                Status :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtStatus" runat="server" Width ="70%" />
                            </td>
                            <td style ="text-align :right ; " class ="Label ">
                                Period 2 Dosing Date :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtPeriod2DosingDate" runat="server" Width ="70%" />
                                <cc1:CalendarExtender ID="CalExPeriod2DosingDate" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="TxtPeriod2DosingDate">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style ="text-align :right ; " class ="Label ">
                                Project Co-ordinator :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtProjectCoordinator" runat="server" Width ="70%" />
                            </td>
                            <td style ="text-align :right ; " class ="Label ">
                                Period 3 Dosing Date :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtPeriod3DosingDate" runat="server" Width ="70%" />
                                <cc1:CalendarExtender ID="CalExPeriod3DosingDate" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="TxtPeriod3DosingDate">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style ="text-align :right ; " class ="Label ">
                                Sponsor Name :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtSponsorName" runat="server" Width ="70%" />
                            </td>
                            <td style ="text-align :right ; " class ="Label ">
                                Period 4 Dosing Date :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtPeriod4DosingDate" runat="server" Width ="70%" />
                                <cc1:CalendarExtender ID="CalExPeriod4DosingDate" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="TxtPeriod4DosingDate">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td >
                            </td>
                            <td  style ="text-align :left ;">
                                <asp:DropDownList ID="DrpLstSponsorName" runat="server" AutoPostBack="true" Width ="70%">
                                </asp:DropDownList>
                            </td>
                            <td align="left">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style ="text-align :right ; " class ="Label ">
                                Report Month :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtReportMonth" runat="server" Width ="70%" />
                            </td>
                            <td style ="text-align :right ; " class ="Label ">
                                Period 1 Check Out Date :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtPeriod1CheckOutDate" runat="server" Width ="70%" />
                                <cc1:CalendarExtender ID="CalExPeriod1CheckOutDate" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="TxtPeriod1CheckOutDate">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style ="text-align :right ; " class ="Label ">
                                Project Code :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtProjectCode" runat="server"  Width ="70%" />
                            </td>
                            <td style ="text-align :right ; " class ="Label ">
                                Period 2 Check Out Date :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtPeriod2CheckOutDate" runat="server" Width ="70%" />
                                <cc1:CalendarExtender ID="CalExPeriod2CheckOutDate" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="TxtPeriod2CheckOutDate">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style ="text-align :right ; " class ="Label ">
                                Year :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtYear" runat="server" Width ="70%" Text="0"  />
                            </td>
                            <td style ="text-align :right ; " class ="Label ">
                                Period 3 Check Out Date :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtPeriod3CheckOutDate" runat="server" Width ="70%" />
                                <cc1:CalendarExtender ID="CalExPeriod3CheckOutDate" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="TxtPeriod3CheckOutDate">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style ="text-align :right ; " class ="Label ">
                                Month :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtMonth" runat="server" Width ="70%" />
                            </td>
                            <td style ="text-align :right ; " class ="Label ">
                                Period 4 Check Out Date :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtPeriod4CheckOutDate" runat="server" Width ="70%" />
                                <cc1:CalendarExtender ID="CalExPeriod4CheckOutDate" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="TxtPeriod4CheckOutDate">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style ="text-align :right ; " class ="Label ">
                                Design :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtDesign" runat="server" Width ="70%" />
                            </td>
                            <td style ="text-align :right ; " class ="Label ">
                                Letter Issued :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtLetterIssueds" runat="server" Width ="70%" />
                            </td>
                        </tr>
                        <tr>
                            <td style ="text-align :right ; " class ="Label ">
                                Treatment 1 :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtTreatment1" runat="server" Width ="70%" />
                            </td>
                            <td style ="text-align :right ; " class ="Label ">
                                Date Of Letter Issued :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtDateOfLetterIssued" runat="server" Width ="70%" />
                                <cc1:CalendarExtender ID="CalExDateOfLetterIssued" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="TxtDateOfLetterIssued">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style ="text-align :right ; " class ="Label ">
                                Treatment 2 :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtTreatment2" runat="server" Width ="70%" />
                            </td>
                            <td style ="text-align :right ; " class ="Label ">
                                Remarks :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtRemarks" runat="server" Width ="70%" />
                            </td>
                        </tr>
                        <tr>
                            <td style ="text-align :right ; " class ="Label ">
                                Reference Drug Product (A) :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtReferenceDrugProductA" runat="server" Width ="70%" />
                            </td>
                            <td style ="text-align :right ; " class ="Label ">
                                Sample Expected Date :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtSampleExpectedDate" runat="server" Width ="70%"  />
                                <cc1:CalendarExtender ID="CalExSampleExpectedDate" runat="server" TargetControlID="TxtSampleExpectedDate"
                                    Format="dd-MMM-yyyy">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style ="text-align :right ; " class ="Label ">
                                Reference Drug Product (B) :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtReferenceDrugProductB" runat="server" Width ="70%" />
                            </td>
                            <td style ="text-align :right ; " class ="Label ">
                                Analysis Start Date :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtAnalysisStartDate" runat="server" Width ="70%" />
                                <cc1:CalendarExtender ID="CalExAnalysisStartDate" runat="server" TargetControlID="TxtAnalysisStartDate"
                                    Format="dd-MMM-yyyy">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style ="text-align :right ; " class ="Label ">
                                Test Drug Product :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtTestDrugProduct" runat="server" Width ="70%" />
                            </td>
                            <td style ="text-align :right ; " class ="Label ">
                                Analysis End Date :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtAnalysisEndDate" runat="server" Width ="70%" />
                                <cc1:CalendarExtender ID="CalExAnalysisEndDate" runat="server" TargetControlID="TxtAnalysisEndDate"
                                    Format="dd-MMM-yyyy">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style ="text-align :right ; " class ="Label ">
                                Test Drug Product (C) :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtTestDrugProductC" runat="server" Width ="70%" />
                            </td>
                            <td style ="text-align :right ; " class ="Label ">
                                Report Sent Date :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtReportSentDate" runat="server" Width ="70%" />
                                <cc1:CalendarExtender ID="CalExReportSentDate" runat="server" TargetControlID="TxtReportSentDate"
                                    Format="dd-MMM-yyyy">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style ="text-align :right ; " class ="Label ">
                                Test Drug Product (B) :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtTestDrugProductB" runat="server" Width ="70%" />
                            </td>
                            <td style ="text-align :right ; " class ="Label ">
                                Report Year :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtReportYear" runat="server" Width ="70%" />
                            </td>
                        </tr>
                        <tr>
                            <td style ="text-align :right ; " class ="Label ">
                                Sponsor Address :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtSponsorAddress" runat="server" Width ="70%" />
                            </td>
                            <td style ="text-align :right ; " class ="Label ">
                                ReturnTo Sponsor :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtReturnToSponsor" runat="server" Width ="70%" />
                            </td>
                        </tr>
                        <tr>
                            <td style ="text-align :right ; " class ="Label ">
                                Sponsor Telephone :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtSponsorTelephone" runat="server" Width ="70%" />
                            </td>
                            <td style ="text-align :right ; " class ="Label ">
                                Number Of Standby Subjects :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtNumberOfStandbySubjects" runat="server" Width ="70%" />
                            </td>
                        </tr>
                        <tr>
                            <td style ="text-align :right ; " class ="Label ">
                                Sponsor Fax No :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtSponsorFax" runat="server" Width ="70%" />
                            </td>
                            <td style ="text-align :right ; " class ="Label ">
                                Number Of Samples :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtNumberOfSamples" runat="server" Width ="70%" />
                            </td>
                        </tr>
                        <tr>
                            <td style ="text-align :right ; " class ="Label ">
                                Sponsor Contact Person :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtSponsorContactPerson" runat="server" Width ="70%" />
                            </td>
                            <td style ="text-align :right ; " class ="Label ">
                                Report Status :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtReportStatuss" runat="server" Width ="70%" />
                            </td>
                        </tr>
                        <tr>
                            <td style ="text-align :right ; " class ="Label ">
                                Group :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtGroup" runat="server" Width ="70%" />
                            </td>
                            <td style ="text-align :right ; " class ="Label ">
                                Report Status 1 :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtReportStatus1" runat="server" Width ="70%" />
                            </td>
                        </tr>
                        <tr>
                            <td style ="text-align :right ; " class ="Label ">
                                Test Drug Product (D) :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtTestDrugProductD" runat="server" Width ="70%" />
                            </td>
                            <td style ="text-align :right ; " class ="Label ">
                                Last Ambulatory (End Study) :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtLastAmbulatory" runat="server" Width ="70%" />
                                <cc1:CalendarExtender ID="CalExLastAmbulatory" runat="server" TargetControlID="TxtLastAmbulatory"
                                    Format="dd-MMM-yyyy">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style ="text-align :right ; " class ="Label ">
                                Study Results :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtStudyResults" runat="server" Width ="70%" />
                            </td>
                            <td style ="text-align :right ; " class ="Label ">
                                Condition:
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtCondition" runat="server"  Width ="70%"  />
                            </td>
                        </tr>
                        <tr>
                            <td style ="text-align :right ; " class ="Label ">
                                E-Archiving :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtEArchiving" runat="server"  Width ="70%" />
                            </td>
                            <td style ="text-align :right ; " class ="Label ">
                                Location:
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtLocation" runat="server"  Width ="70%" />
                            </td>
                        </tr>
                        <tr>
                            <td style ="text-align :right ; " class ="Label ">
                                Retention Period :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtRetentionPeriod" runat="server"  Width ="70%" />
                            </td>
                            <td style ="text-align :right ; " class ="Label ">
                                RP ID:
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtRpId" runat="server"  Width ="70%" />
                            </td>
                        </tr>
                        <tr>
                            <td style ="text-align :right ; " class ="Label ">
                                Status Name :
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtStatusName" runat="server" Width ="70%"></asp:TextBox>
                            </td>
                            <td style ="text-align :right ; " class ="Label ">
                                Treatment 3:
                            </td>
                            <td style ="text-align :left ;">
                                <asp:TextBox ID="TxtTreatment3" runat="server" Width ="70%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style ="text-align :center ;">
                                <asp:Button runat="server" ID="BtnSave" Text="Save" ToolTip ="Save" CssClass="btn btnsave" />
                                 <asp:Button runat="server" ID="BtnCancel" Text="Cancel" ToolTip ="Cancel" CssClass="btn btncancel" />
                            </td>
                            
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <br />
            <div>
                <asp:Panel ID="PnlGridView" runat="server"  Width="1075px" ScrollBars="auto" style="margin :auto;">
                    <asp:GridView ID="GVWOldProjects" runat="server" SkinID="grdViewForOldProjects" AllowPaging="True"
                        PageSize="5">
                        <Columns>
                            <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>                                    
                                    <asp:ImageButton ID="LnkBtnEdit" runat="server" ToolTip="Edit" ImageUrl="~/images/Edit2.gif" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Delete">
                                <ItemTemplate>
                                    
                                   <asp:ImageButton ID="lnkbtndelete" runat="server" ToolTip="Delete" CommandArgument='<%# Eval("nSerialNo") %>'
                                        CommandName="Delete" ImageUrl="~/Images/i_delete.gif" OnClientClick="return confirm('Are You Sure You Want To Delete?')" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="BtnCancel" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

    <script type="text/javascript">

        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
         $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        Sys.Browser.WebKit = {};
        if (navigator.userAgent.indexOf('WebKit/') > -1) {
            Sys.Browser.agent = Sys.Browser.WebKit;
            Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
            Sys.Browser.name = 'WebKit';
        }          

    </script>

</asp:Content>
