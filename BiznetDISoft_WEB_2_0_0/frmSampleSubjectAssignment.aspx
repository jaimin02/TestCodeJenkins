<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmSampleSubjectAssignment.aspx.vb" Inherits="frmSampleSubjectAssignment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <script type="text/javascript" src="Script/General.js"></script>
    <script type="text/javascript" language="javascript" src="Script/popcalendar.js"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>

    <center>
    <table style="width: 80%" cellpadding="5px">
        <tr>
            <td colspan="2" class="Label">
                <fieldset style="width:100%;">
                    <legend>Selection :</legend>
                        <asp:RadioButtonList ID="RBLProjecttype" runat="server" Width="35%" AutoPostBack="True"
                            RepeatDirection="Horizontal" Style="margin: auto;" OnSelectedIndexChanged="RBLProjecttype_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="0000000000">Screening</asp:ListItem>
                            <asp:ListItem Value="1">Project Specific</asp:ListItem>
                        </asp:RadioButtonList>
                </fieldset>
            </td>
        </tr>

        <tr runat="server" id="trProject" style="display: none;">
            <td style="text-align: right;" class="Label">
                ProjectName* :
            </td>
            <td style="text-align: left;">
                <asp:TextBox ID="txtproject" TabIndex="1" runat="server" CssClass="textBox" Width="65%"></asp:TextBox><asp:HiddenField
                    ID="HProjectId" runat="server"></asp:HiddenField>
                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                    TargetControlID="txtProject" ServicePath="AutoComplete.asmx" ServiceMethod="GetProjectCompletionListWithOutSponser"
                    OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1"
                    CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                    CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1"
                    CompletionListElementID="pnlSubjectForProject">
                </cc1:AutoCompleteExtender>
                <asp:Button Style="display: none" ID="btnSetProject" OnClick="btnSetProject_Click"
                    runat="server" Text=" Project"></asp:Button>
                <asp:CheckBox ID="chkProjectScreening" Text="Project Screening" runat="server" />
                <asp:Panel ID="pnlSubjectForProject" runat="server" Style="max-height: 100px; overflow: auto;
                    overflow-x: hidden;" />
            </td>
        </tr>

        <tr runat="server" id="trActivity" style="display: none">
            <td style="text-align: right;" class="Label">
                Activity* :
            </td>
            <td style="text-align: left;">
                <asp:DropDownList ID="ddlActivity" TabIndex="2" runat="server" CssClass="dropDownList"
                    Width="40%" AutoPostBack="True">
                </asp:DropDownList>
            </td>
        </tr>
        
        <tr id="Tr1" runat="Server" >
            <td class="Label" nowrap="noWrap" style="text-align: right; " >
                Collection type:
            </td>
            <td class="DropDownList" nowrap="noWrap" style="text-align: left" >
                <asp:DropDownList class="DropDownList" AutoPostBack="true" Width="165px"   runat="server" id="ddlType" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                    
                    <asp:ListItem Text="With Barcode" Value="1"  ></asp:ListItem>
                    <asp:ListItem Text="WithOut Barcode" Value="2"  ></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>

        <tr runat="server" id="rwBarcodeId">
            <td class="Label" nowrap="noWrap" style="text-align: right; width: 11% !important;">
                Barcode Id:
            </td>
            <td class="Label" style="text-align: left;">
                <asp:TextBox ID="txtScan" runat="server" AutoPostBack="True"></asp:TextBox>
                <%--<asp:CheckBox runat="server" AutoPostBack="true" ID="chkwithoutscanner" Visible="false"  Text="Collect without BarcodeScanner" />--%>
                <asp:CheckBox runat="server" AutoPostBack="true" ID="chkwithoutscanner" Visible="false"  Text="Collect without BarcodeScanner" />
            </td>
        </tr>

        <tr runat="Server" id="trLbl">
            <td colspan="2" class="Label" nowrap="noWrap" style="text-align: center;">
                <asp:Label ID="lblSampleId" runat="server" Text="Sample Id :"></asp:Label><asp:Label
                    ID="lblSample" runat="server" Text=""></asp:Label>
                <asp:Label ID="lblSubjectId" runat="server" Text="Subject Id :"></asp:Label><asp:Label
                    ID="lblSubject" runat="server" Text=""></asp:Label>
                <asp:Label ID="lblMySubjectNo" runat="server" Text="MySubject No :"></asp:Label><asp:Label
                    ID="lblMySubject" runat="server"></asp:Label>
            </td>
        </tr>
        <%--<tr runat="Server" id="trWoutScanner">
            <td colspan="2" class="Label">
                <table width="30%" style="margin: auto;">
                    <tr>
                        <td style="text-align: left;">
                            <asp:Label ID="lblSubID" runat="server" Text="SubjectId(MySubjectNo):" Width="159px"></asp:Label>
                            <asp:DropDownList ID="ddlSubjectID" runat="server" AutoPostBack="true" Width="158px"
                                Visible="False">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left;">
                            <asp:Panel ID="pnlSubject" runat="server" Height="100px" ScrollBars="Auto" BorderStyle="Solid"
                                BorderWidth="1px" Style="display: none; max-width: 20%;">
                                <asp:CheckBoxList ID="ChklSampleId" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                                </asp:CheckBoxList>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>--%>

         <tr runat="Server" id="trWoutScanner">
            <td class="Label" nowrap="noWrap" style="text-align: right; width: 11% !important;">
                <asp:Label ID="lblSubID" runat="server" Text="Subject Id (MySubjectNo) :" Width="159px"></asp:Label>
            </td>
            <td  class="Label" style="text-align: left;" >
                <asp:DropDownList ID="ddlSubjectID" runat="server" AutoPostBack="true" Width="165px"
                    Visible="False">
                </asp:DropDownList>
           </td>
                 
        </tr>
            <tr runat="Server" id="trWoutScanner1">
                <td class="Label" nowrap="noWrap" style="text-align: right; width: 11% !important;">
                  </td>
                <td class="Label" style="text-align: left;" >
                <asp:Panel ID="pnlSubject" runat="server" Height="60px" Width="400px" ScrollBars="Auto" BorderStyle="Solid"
                     Style="display: none;">
                    <asp:CheckBoxList ID="ChklSampleId" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                    </asp:CheckBoxList>
                </asp:Panel>
            </td>                         
            
            </tr>


        <tr>
            <td colspan="2" class="Label" width="35%" align="left" >
                        <asp:Button ID="BtncollectSample" runat="server" OnClientClick="return ValidationForSubject();"
                            CssClass="btn btnnew" Text="Collect Sample" ToolTip="Collect Sample"
                            Visible="False" Enabled="False" style="margin-left: 349px" />
                        <asp:Button ID="btnExit" runat="server" CssClass="btn btnexit" Text="Exit" ToolTip="Exit"
                    OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
            </td>
        </tr>

        <tr>
            <td colspan="2" class="Label" align="left" style="margin-left:115px;">
                <asp:Label ID="lblNote" runat="server" style="margin-left:115px;" Text="NOTE : Sample Details Shown In Red Colour Are Not Created Or Collected In CPMA Dept"
                    class="LabelBold"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Panel ID="pnlGrid" runat="server" Style="margin: auto; width: 82.2%">

                    <asp:GridView ID="gvwSubjectSample" runat="server" AutoGenerateColumns="False" Width="100%"
                        PageSize="25">
                        <RowStyle BackColor="#cee3ed" Font-Names="Verdana" VerticalAlign="Middle" HorizontalAlign="left"
                            Font-Size="9pt" ForeColor="navy" />
                        <EditRowStyle BackColor="#cee3ed" Font-Names="Verdana" Font-Size="9pt" VerticalAlign="Middle" />
                        <HeaderStyle Font-Underline="False" BackColor="#1560a1" Font-Names="Verdana" VerticalAlign="Top"
                            Font-Size="10pt" HorizontalAlign="Center" ForeColor="white" Font-Bold="True" />
                        <FooterStyle BackColor="#1560a1" Font-Names="Verdana" Font-Size="X-Small" HorizontalAlign="left"
                            ForeColor="Navy" Font-Bold="True" />
                        <AlternatingRowStyle BackColor="white" Font-Names="Verdana" HorizontalAlign="left"
                            Font-Size="9pt" ForeColor="navy" />
                        <PagerStyle ForeColor="#ffa24a" Font-Underline="False" BackColor="white" Font-Bold="True"
                            Font-Names="Verdana" HorizontalAlign="Center" Font-Size="X-Small" />
                        <Columns>
                            <asp:BoundField DataField="nSampleTypeDetailNo" HeaderText="nSampleTypeDetailNo" />
                            <asp:BoundField DataField="vSampleBarCode" HeaderText="Sample" />
                            <asp:BoundField DataField="vSubjectId" HeaderText="Subject Id">
                                <ItemStyle Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="vMySubjectNo" HeaderText="Subject No" />
                            <asp:BoundField DataField="iMySubjectNo" HeaderText="iSubjectNo" />
                            <asp:BoundField DataField="FullName" HeaderText="Subject Name" />
                            <asp:BoundField DataField="vWorkSpaceDesc" HeaderText="Project" />
                            <asp:BoundField DataField="vNodeDisplayName" HeaderText="Activity" />
                            <asp:BoundField DataField="iPeriod" HeaderText="Period" />
                            <asp:BoundField DataField="dCollectionDateTime" HeaderText="Sample Collection Time" />
                            <asp:BoundField DataField="dModifyOn" HeaderText="Actual Save Time" />
                            <asp:BoundField DataField="vUserName" HeaderText="Collected By" />
                            <asp:BoundField DataField="vDeptCode" HeaderText="vDeptCode" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                <asp:Label ID="lblCount" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    </center>
     <asp:HiddenField ID="hndLockStatus" runat="server" />
        <script type="text/javascript">

            function pageLoad() {
                $('#<%= gvwSubjectSample.ClientID%>').removeAttr('style', 'display:block');
                var scrolly = "250px";
                if ($('#<%= gvwSubjectSample.ClientID%> tr').length < 10) {
                    scrolly = "25%";
                }
                oTab = $('#<%= gvwSubjectSample.ClientID%>').prepend($('<thead>').append($('#<%= gvwSubjectSample.ClientID%> tr:first'))).dataTable({
                    "bJQueryUI": true,
                    "sPaginationType": "full_numbers",
                    "bLengthChange": true,
                    "iDisplayLength": 10,
                    "bProcessing": true,
                    "bSort": false,
                    "sScrollY": scrolly,
                    aLengthMenu: [
                        [10, 25, 50, 100, -1],
                        [10, 25, 50, 100, "All"]
                    ],
                });
            }

            function ClientPopulated(sender, e) {
                ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
            }

            function OnSelected(sender, e) {
                ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
          $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));

        }

        function AutoTimeConvert(ParamTime, txtTime, IsNotNull) {

            if (isNaN(ParamTime) == true) {
                msgalert("Please Enter In Numeric HH:MM:SS Format !");
                return false;
            }
            TimeConvert(ParamTime, txtTime);
        }


        function ValidationForSubject() {
            var chklst = document.getElementById('<%=ChklSampleId.clientid%>');
            var chks;
            var result = false;
            var i;

            if (chklst != null && typeof (chklst) != 'undefined') {
                chks = chklst.getElementsByTagName('input');
                for (i = 0; i < chks.length; i++) {
                    if (chks[i].type.toUpperCase() == 'CHECKBOX' && chks[i].checked) {
                        result = true;
                        break;
                    }
                }
            }

            if (!result) {
                msgalert('Please Select Atleast One Sample !');
                return false;
            }

            return true;
        }

        //Add by shivani pandya for project lock
        function getData(e) {
            var WorkspaceID = $('input[id$=HProjectId]').val();
            $.ajax({
                type: "post",
                url: "frmSampleSubjectAssignment.aspx/LockImpact",
                data: '{"WorkspaceID":"' + WorkspaceID + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                dataType: "json",
                success: function (data) {
                    if (data.d == "L") {
                        msgalert("Project is locked !");
                        $("#<%=hndLockStatus.ClientID%>").val("Lock");
                        $("#ctl00_CPHLAMBDA_txtScan").attr("Disabled", "Disalbed");
                        //$("[type=checkbox]").attr("Disabled", "Disabled");
                        //$("#ctl00_CPHLAMBDA_ddlActivity").attr("Disabled", "Disabled");
                        //$("#ctl00_CPHLAMBDA_BtncollectSample").attr("Disabled", "Disabled");
                        $("#ctl00_CPHLAMBDA_chkwithoutscanner").attr("Disabled", "Disalbed");
                    }
                    if (data.d == "U") {
                        $("#<%=hndLockStatus.ClientID%>").val("UnLock");
                        //$("[type=checkbox]").removeAttr("Disabled");
                        $("#ctl00_CPHLAMBDA_txtScan").removeAttr("Disabled");
                        //$("#ctl00_CPHLAMBDA_ddlActivity").removeAttr("Disabled");
                        //$("#ctl00_CPHLAMBDA_BtncollectSample").removeAttr("Disabled");
                        $("#ctl00_CPHLAMBDA_chkwithoutscanner").removeAttr("Disabled");
                    }
                },
                failure: function (response) {
                    msgalert(response.d);
                },
                error: function (response) {
                    msgalert(response.d);
                }
            });
            return true;
        }
    </script>
</asp:Content>
