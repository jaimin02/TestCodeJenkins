<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmMyProject, App_Web_5xoe1jh1" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="conMyProject" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    
    <table cellpadding="0px" width="100%">
        <tr>
            <td style="width: 100%; text-align: center">
                <asp:UpdatePanel ID="upnlProjectsGridView" runat="server">
                    <ContentTemplate>
                        <table width="100%">
                            <tbody>
                                <tr>
                                    <td style="width: 13%; white-space: nowrap; text-align: right; float:left; margin:4px 0px; display:inline-block;" class="Label">
                                        Search Project No:
                                    </td>
                                    <td style="text-align: left; float:left; width:80%;">
                                        <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" MaxLength="50" Width="88%"></asp:TextBox>
                                        <asp:Button ID="btnAllProject" runat="server" CssClass="btn btnnew" Text="My Projects"
                                            ToolTip="My Project" Width="11%"  Style="float:right; margin-top:-2px;"  />
                                        <asp:Button ID="btnSetProject" runat="server"  Style="display: none" Text=" Project"
                                            ToolTip="Project" />
                                        <asp:HiddenField ID="HProjectId" runat="server" />
                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                                            TargetControlID="txtProject" ServicePath="AutoComplete.asmx" OnClientShowing="ClientPopulated"
                                            OnClientItemSelected="OnSelected" MinimumPrefixLength="1" CompletionListItemCssClass="autocomplete_listitem"
                                            CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListCssClass="autocomplete_list"
                                            BehaviorID="AutoCompleteExtender1" CompletionListElementID="pnlProjectList">
                                        </cc1:AutoCompleteExtender>
                                        <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 300px; overflow: auto;overflow:hidden;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="padding-bottom: 2px; text-align: center;">
                                        <asp:PlaceHolder ID="phTopPager" runat="server"></asp:PlaceHolder>
                                    </td>
                                </tr>
                                <tr></tr>
                                <tr></tr>
                                <tr></tr>
                                <tr></tr>
                                <tr>
                                    <td colspan="2" style="text-align: center;">
                                        <div style= "width:84%; padding-left: 6%;">
                                        <asp:GridView ID="gvwProjects" runat="server" CssClass="gvwProjects"  style= "margin:auto; width:90%;"
                                            PageSize="20" AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="true" >
                                            <Columns>
                                                <asp:BoundField DataField="vWorkspaceID" SortExpression="vWorkspaceID" HeaderText="WorkspaceID">
                                                    <HeaderStyle Font-Underline="False" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Project">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProjectName" runat="server" Text='<%# Eval("vWorkspaceDesc") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="vRequestId" SortExpression="vRequestId" HeaderText="Request Id">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vProjectNo" SortExpression="vProjectNo" HeaderText="Project No.">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vClientName" SortExpression="vClientName" HeaderText="Sponsor">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vDrugName" SortExpression="vDrugName" HeaderText="Drug">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="iNoOfSubjects" SortExpression="iNoOfSubjects" HeaderText="# Subjects">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vProjectManager" SortExpression="vBrandName" HeaderText="Project Manager">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vProjectCoordinator" SortExpression="vProjectCoordinator"
                                                    HeaderText="Co-Ordinator">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vProjectTypeName" SortExpression="vProjectTypeName" HeaderText="Project Type">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vRegionName" SortExpression="vRegionName" HeaderText="Submission">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="cProjectStatusDesc" SortExpression="cProjectStatusDesc"
                                                    HeaderText="Status">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <FooterStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Change Status">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkBtn" OnClick="lnkBtn_Click" runat="server" Text="Change Status"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="False" HeaderText="Subject Detail">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkBtnSubDet" OnClick="lnkBtnSubDet_Click" runat="server" Text="View Details"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Details">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkBtnProDet" class="LicnkButton" OnClick="lnkBtnProDet_Click" runat="server" Text="Project Details"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Default Document UserRights">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemTemplate>
                                                        <a runat="server" id="lbldocright" href='frmWorkspaceDefaultWorkflowUserDtl.aspx?mode=1&page=frmMyProject&Type=ALL&WorkspaceId=<%# Eval("vWorkspaceID") %>&WorkspaceName=<%# Eval("vWorkspaceDesc") %>'>
                                                            <img src="Images/userrights.png" title="Default Document UserRights" alt="Default Document UserRights"  class="UserImage"
                                                                style="margin: none;" /></a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="cWorkspaceType" HeaderText="cWorkspaceType" />
                                            </Columns>
                                        </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="padding-top: 2px; text-align: center;">
                                        <asp:PlaceHolder ID="phBottomPager" runat="server"></asp:PlaceHolder>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <asp:HiddenField ID="HFProjectStatus" runat="server" />
                        <asp:HiddenField ID="hndLockStatus" runat="server" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnAllProject" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="upnlDiv" runat="server">
                    <ContentTemplate>
                        <div style="padding-right: 10px; display: none; padding-left: 10px; left: 324px;
                            padding-bottom: 10px; padding-top: 10px; top: 483px; text-align: left" id="divMsg"
                            class="DIVSTYLE2" runat="server">
                            <table cellpadding="3" width="100%">
                                <tbody>
                                    <tr>
                                        <td class="Label" style="text-align: right; width: 40%;">
                                            Select Status :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="dropDownList" AutoPostBack="True"
                                                OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" Width="50%">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Label" style="text-align: right;">
                                            Select Template :
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlTemplate" runat="server" CssClass="dropDownList" Width="50%" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlStatus" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Label" style="text-align: right;">
                                            No. of Periods :
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtPeriod" runat="server" Text="2" CssClass="textBox" Width="50%" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlStatus" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Label" style="text-align: right;">
                                            Remarks/Scope of Service:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtReason" CssClass="textBox" Text="" TextMode="MultiLine"
                                                Rows="2" Width="50%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <%-- <td class="Label">
                                        </td>--%>
                                        <td colspan="2" style="text-align: center;">
                                            <asp:Button ID="btnSave" OnClick="btnSave_Click" OnClientClick="return Validation();"
                                                runat="server" Text=" Save " ToolTip="Save" CssClass="btn btnsave" />
                                            <asp:Button ID="btnClose" OnClick="btnClose_Click"  runat="server" Text=" Close " ToolTip="Close"  CssClass="btn btnclose" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>

     <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

         .ui-state-default a, .ui-state-default a:link, .ui-state-default a:visited {
             color: white !important;
         }
        
    </style>

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" src="Script/General.js"></script>

    <script type="text/javascript" src="Script/jquery-1.4.3.min.js"></script>

     <script src="Script/jquery.dataTables.min.js" type="text/javascript"></script>

    <script src="Script/jquery.dataTables.plugins.js" type="text/javascript"></script>

    <script type="text/javascript">

        jQuery(window).focus(function () {
            ThemeSelection();
            return false;
        });
        function pageload() {
            gvwProjectsUI();
        }

        function ShowHideDiv(Show) {
            var Showdv = document.getElementById('<%= divMsg.clientId %>');
            if (Show == 'Y') {
                document.getElementById('<%= txtReason.ClientId %>').value = '';
                Showdv.style.display = 'block';
                SetCenter(Showdv);
            }
            else {
                document.getElementById('<%= txtReason.ClientId %>').value = '';
                Showdv.style.display = 'none';
                
            }
            gvwProjectsUI()
        }

        function SetCenter(dv) {
            var winScroll = BodyScrollHeight();
            var updateProgressDivBounds = Sys.UI.DomElement.getBounds(dv);
            var winBounds = GetWindowBounds();

            var x = Math.round(winBounds.Width / 2) - Math.round(updateProgressDivBounds.width / 2);
            var y = Math.round(winBounds.Height / 2) - Math.round(updateProgressDivBounds.height / 2);

            x += winScroll.xScr;
            y += winScroll.yScr;

            Sys.UI.DomElement.setLocation(dv, parseInt(x), parseInt(y));
        }

        function Validation() {
            var ddlStatus = document.getElementById('<%= ddlStatus.ClientId%>');
            if (document.getElementById('<%=txtReason.ClientID%>').value.trim() == '') {
                msgalert('Please Enter Remarks !');
                document.getElementById('<%=txtReason.ClientID%>').value = ''
                document.getElementById('<%=txtReason.ClientID%>').focus();
                return false;
            }
            if (ddlStatus.value.trim() == 'S') {
                if (!confirm('Are You Sure Entered Number Of Periods Are Correct?')) {
                    return false;
                }
            }
            return true;
        }
        function Numeric() {
            var ValidChars = "0123456789";
            var Numeric = true;
            var Char;
            sText = document.getElementById('<%=txtPeriod.ClientID%>').value;

            if (sText == '') {
                document.getElementById('<%=txtPeriod.ClientID%>').value = "1";
                document.getElementById('<%=txtPeriod.ClientID%>').focus();
            }
            for (i = 0; i < sText.length && Numeric == true; i++) {
                Char = sText.charAt(i);
                if (ValidChars.indexOf(Char) == -1) {
                    msgalert('Please Enter Numeric Value');
                    document.getElementById('<%=txtPeriod.ClientID%>').value = "1";
                    document.getElementById('<%=txtPeriod.ClientID%>').focus();
                    Numeric = false;
                }
            }
            return Numeric;
        }

        function ClientPopulated(sender, e) {

            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
         $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        // for fix gridview header aded on 22-nov-2011

        //        function fixheader() {
        //            FreezeTableHeader($('#<%= gvwProjects.ClientID %>'), { height: 250, width: 900 });
        //        }       
           
        //Add by shivani pandya for project lock
        function getData(e) {
            var WorkspaceID = $('input[id$=HProjectId]').val();
            $.ajax({
                type: "post",
                url: "frmMyProject.aspx/LockImpact",
                data: '{"WorkspaceID":"' + WorkspaceID + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                dataType: "json",
                success: function (data) {
                    if (data.d == "L") {
                        msgalert("Project Is Locked.");
                        $("#<%=hndLockStatus.ClientID%>").val("Lock");
                 }
                   if (data.d == "U") {
                       $("#<%=hndLockStatus.ClientID%>").val("UnLock");
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
        function EnabledControlForProjectLock() {
           // $(".UserImage").attr("style", "Display:none");
           // $(".LicnkButton").attr("style", "Display:none");
          //  $("#<%=hndLockStatus.ClientID%>").val("NoLock");            
          //  return true;
        }
        function ThemeSelection() {
        //    if (document.cookie.split(";")[0] == "Theme=Orange") {
        //        $("#ctl00_CPHLAMBDA_gvwProjects tr").last().css({ 'background-color': '#CF8E4C' });
        //    } else if (document.cookie.split(";")[0] == "Theme=Green") {
        //        $("#ctl00_CPHLAMBDA_gvwProjects tr").last().css({ 'background-color': '#33a047' });
        //    } else if (document.cookie.split(";")[0] == "Theme=Demo") {
        //        $("#ctl00_CPHLAMBDA_gvwProjects tr").last().css({ 'background-color': '#999966' });
        //    } else if (document.cookie.split(";")[0] == "Theme=Blue") {
        //        $("#ctl00_CPHLAMBDA_gvwProjects tr").last().css({ 'background-color': '#1560a1' });
        //    }
        }

       //Added By Vivek Patel
        function gvwProjectsUI() {
            if ($('#<%= gvwProjects.ClientID%>')) {
                $('#<%= gvwProjects.ClientID%>').prepend($('<thead>').append($('#<%= gvwProjects.ClientID%> tr:first')))
                    .dataTable({
                    "bDestroy": true,
                    //"bRetrieve": true,
                    "bJQueryUI": true,
                    "sScrollX": "100%",
                    "sPaginationType": "full_numbers",
                    "bLengthChange": true,
                    "iDisplayLength": 10,
                    "bProcessing": true,
                    "bSort": false,
                    aLengthMenu: [
                        [10, 25, 50, 100, -1],
                        [10, 25, 50, 100, "All"]
                    ],
                });
                $('#<%= gvwProjects.ClientID%> tr:first').css('background-color', '#3A87AD');
                $('tr', $('.dataTables_scrollHeadInner')).css("background-color", "rgb(58, 135, 173)");
            }
        }
        //Completed By Vivek Patel
    </script>

    <script type="text/javascript" src="Script/scrollablegrid.js"></script>

</asp:Content>
