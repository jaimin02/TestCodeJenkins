<%@ page title="" language="VB" masterpagefile="~/ECTDMasterPage.master" autoeventwireup="false" inherits="frmSchedulingandGunningActivities, App_Web_2mzu20n4" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
     
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <asp:UpdatePanel runat="server" ID="UpControls" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="hdnDynamicURL" runat="server" />
            <asp:HiddenField ID="HdnCookie" runat="server" Value="" />
            <asp:HiddenField ID="hdnSubjectId" runat="server" Value="" />
            <asp:HiddenField ID="hdnDataEntryPendingCount" runat="server" Value="" />

            <asp:Button runat="server" ID="btndefault" Style="display: none" OnClientClick="return PassTheBarcodeTextBox();" />
            <table style="width: 30%; border: 1px solid darkgray; border-radius: 5px;" align="left" border="0" cellpadding="4px">
                <tr>
                    <td class="Label" style="white-space: nowrap; text-align: right; width: 14%">Project* :
                    </td>
                    <td style="text-align: left; width: 86%">
                        <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="98%" TabIndex="1"></asp:TextBox><asp:Button
                            Style="display: none" ID="btnSetProject" runat="server" Text=" Project"></asp:Button><asp:HiddenField
                                ID="HProjectId" runat="server"></asp:HiddenField>
                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                            TargetControlID="txtProject" ServicePath="AutoComplete.asmx" ServiceMethod="GetMyProjectCompletionList"
                            OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1"
                            CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                            CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1"
                            CompletionListElementID="pnlProjectList">
                        </cc1:AutoCompleteExtender>
                        <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" />
                    </td>
                </tr>

                <tr>
                    <td class="Label" style="white-space: nowrap; text-align: right; width: 14%">Period :
                    </td>
                    <td style="text-align: left; width: 86%">
                        <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="dropDownList" Width="50%"
                            AutoPostBack="True" TabIndex="2">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="Label" style="white-space: nowrap; text-align: right; width: 14%">Activity :
                    </td>
                    <td style="text-align: left; width: 86%">
                        <asp:DropDownList ID="ddlActivity" runat="server" CssClass="dropDownList" Width="100%"
                            TabIndex="3" AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                </tr>

                <tr>
                    <td></td>
                    <td style="text-align: center; width: 100%" class="Label">
                        <asp:HiddenField ID="hfTextChnaged" runat="server" />
                        <asp:Button ID="btnSubjectMgmt" runat="server" CssClass="btn btnnew" Text="Subject Mgmt"
                            ToolTip="Subject Management" TabIndex="6" Enabled="false" />
                        <asp:Button ID="btnSearch" runat="server" CssClass="btn btnnew" Text="Search" ToolTip="Search"
                            TabIndex="7" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td class="Label" style="white-space: nowrap; text-align: right; width: 14%">Barcode :
                    </td>
                    <td style="text-align: left; width: 86%">
                        <asp:Button ID="btnSaveandredirect" runat="server" CssClass="btn btnsave" Style="display: none" />
                        <asp:Button ID="btnForceKillPage" runat="server" CssClass="btn btncancel" Style="display: none" />
                        <asp:TextBox ID="txtScan" runat="server" TabIndex="8" CssClass="textBox"
                            Enabled="False" Width="80%" MaxLength="10"></asp:TextBox>
                        <asp:HiddenField ID="HdFieldValidationTrueOrFalse" runat="server" />

                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="Label" runat="server" id="canal" visible="false" style="border: outset 2px black; font: verdana; font-size: 8pt; height: auto; background-color: White; text-align: left; width: 100%">
                           <asp:Label ID="Label2" BackColor="Red" Width="20" Height="10" runat="server" />
                                -Rejected Subject,&nbsp;&nbsp;
                                <asp:Label ID="Label3" BackColor="Orange" Width="20" Height="10" runat="server" />-Data Entry
                                Continue,&nbsp;&nbsp;
                                <asp:Label ID="Label4" BackColor="Blue" runat="server" Width="20" Height="10" />- Data Entry
                                Completed
                        </div>
                        <asp:UpdatePanel runat="server" ID="UpGridSubjectSample" UpdateMode="Conditional">
                            <ContentTemplate>
                                         <table width="100%" style="margin: auto;">
                                    <tr>
                                        <td>
                                            <div id="divsubjectsample" runat="server" style="overflow-y: auto; height: 280px;">
                                             <asp:GridView ID="gvwSubjectSample" runat="server" AutoGenerateColumns="False" SkinID="grdViewAutoSizeMax"
                                                PageSize="25">
                                                    <Columns>
                                                        <%--<asp:BoundField DataField="nPKSampleId" HeaderText="nPKSampleId" />
                                                    <asp:BoundField DataField="vPKSampleId" HeaderText="Sample Id" />--%>
                                                        <asp:BoundField DataField="iMySubjectNo" HeaderText="Subject No" />
                                                        <asp:BoundField DataField="vMySubjectNo" HeaderText="Subject No" ItemStyle-HorizontalAlign="Center" />
                                                        <%--<asp:BoundField DataField="vSubjectId" HeaderText="Subject Id" />--%>
                                                        <asp:TemplateField HeaderText="Subject Id">
                                                            <HeaderTemplate>
                                                                Subject Id
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkSubjectId" runat="server" Enabled="false" Text='<%# Bind("vSubjectId") %>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <%--<asp:BoundField DataField="vInitials" HeaderText="Subject Initials">
                                                        <ItemStyle Wrap="false" />
                                                    </asp:BoundField>--%>
                                                        <asp:BoundField DataField="iRefNodeId" HeaderText="Ref.NodeId" />
                                                        <asp:BoundField DataField="Reference Time" HeaderText="Scheduled Time">
                                                            <ItemStyle Wrap="false" HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vSubjectId" HeaderText="Subject Id" />
                                                        <asp:BoundField DataField="IsDataEntryDone" HeaderText="IsDataEntryDone" />
                                                          <asp:BoundField DataField="DataStatus" HeaderText="DataStatus" />
                                                          <asp:BoundField DataField="ReplaceSubject" HeaderText="ReplaceSubject" />
                                                        <%--<asp:BoundField DataField="dCollectionDateTime" HeaderText="Collection DateTime">
                                                        <ItemStyle Wrap="false" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="iCollectionBy" HeaderText="Collection By" />
                                                    <asp:BoundField DataField="vCollectionBy" HeaderText="Collection By" />
                                                    <asp:BoundField DataField="nRefTime" HeaderText="Dosing Time" />
                                                    <asp:BoundField DataField="vRemark" HeaderText="Replacement Remark" />--%>
                                                        <%--<asp:BoundField DataField="AttendanceMysubNo" HeaderText="AttendanceMysubNo" />--%>
                                                    </Columns>
                                                </asp:GridView>
                                          </div>
                                        </td>
                                        <asp:HiddenField ID="HdFieldForSelectedGridIndex" runat="server" />
                                    </tr>
                                </table>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="gvwSubjectSample" EventName="RowCommand" />
                                <%--<asp:AsyncPostBackTrigger ControlID="txtreplaceCode" EventName="TextChanged" />--%>
                                <%--<asp:AsyncPostBackTrigger ControlID="btnReplaceOK" EventName="Click" />--%>
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>


        </ContentTemplate>
        <Triggers>
            <%--<asp:AsyncPostBackTrigger ControlID="btnSubjectMgmt" EventName="Click" />--%>
            <asp:AsyncPostBackTrigger ControlID="gvwSubjectSample" EventName="RowCommand" />
            <asp:AsyncPostBackTrigger ControlID="txtScan" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="btnSaveandredirect" EventName="Click" />
            <%--<asp:AsyncPostBackTrigger ControlID="btnReplaceOK" EventName="Click" />--%>
            <asp:PostBackTrigger ControlID="btnSaveSubject" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional" ChildrenAsTriggers="false">

        <ContentTemplate>
            <asp:Panel ID="m_PanelImage" runat="server">
                <table style="width: 70%" border="0">
                    <tr>
                        <td>
                            <iframe id="iFrmDynamicPage" runat="server" width="100%" height="500px" visible="False"></iframe>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td>
                        <button id="btnSubjectManagement" runat="server" style="display: none;" />
                        <cc1:ModalPopupExtender ID="MPESubjectManagement" runat="server" PopupControlID="divSubjectManagement"
                            BackgroundCssClass="modalBackground" TargetControlID="btnSubjectManagement" CancelControlID="imgSubjectManagement"
                            BehaviorID="MPESubjectManagement1">
                        </cc1:ModalPopupExtender>
                        <div id="divSubjectManagement" runat="server" class="centerModalPopup" style="display: none; left: 521px; width: 40%; position: absolute; top: 525px; max-height: 404px;">
                            <asp:Panel ID="pnlMedEx" runat="server" Visible="true">
                                <table cellpadding="5" style="width: 100%">
                                    <tr>
                                        <td class="Label" style="text-align: center; height: 22px;" valign="top" colspan="2">
                                            <h1 class="header">
                                                <asp:Label runat="server" ID="lblSubjectManagement" Text="Subject Management" class="LabelBold" />
                                                <img id="imgSubjectManagement" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px;"
                                                    title="Close" /></h1>
                                            </h1>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" valign="top">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="pnlMedExGrid" runat="server" Height="300px" ScrollBars="Auto" Width="100%"
                                                            Style="margin: auto;">
                                                            <asp:GridView ID="gvwSubjects" runat="server" AutoGenerateColumns="False" PageSize="5"
                                                                SkinID="grdViewSmlAutoSize" TabIndex="10" Width="100%" Visible="true">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <input id="chkSelectAll" onclick="SelectAll(this, 'gvwSubjects')" type="checkbox" />
                                                                            <asp:Label ID="Label1" runat="server" Text="All"></asp:Label>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="ChkMove" runat="server" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="vWorkSpaceId" HeaderText="vWorkSpaceId">
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="iMySubjectNoNew" HeaderText="iMySubjectNo.">
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vMySubjectNo" HeaderText="MySubject No.">
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vSubjectId" HeaderText="Subject Id">
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20%" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vInitials" HeaderText="Subject Initials">
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" class="Label" colspan="2" style="text-align: center; height: 29px;"
                                            valign="top">
                                            <asp:Button ID="btnSaveSubject" OnClientClick="return CheckAtleastOne('<%= gvwSubjects.ClientId %>');"
                                                runat="server" CssClass="btn btnsave" TabIndex="11" Text="Save" />
                                            <asp:Button ID="btnClose" runat="server" CssClass="btn btnclose" Text="Close" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSubjectMgmt" EventName="Click" />
            <%--<asp:AsyncPostBackTrigger ControlID="gvwSubjectSample" EventName="RowCommand" />
            <asp:AsyncPostBackTrigger ControlID="txtScan" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="btnSaveandredirect" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnReplaceOK" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnSaveSubject" />--%>
        </Triggers>
    </asp:UpdatePanel>

    <asp:Button ID="btnShow" runat="server" Text="Show Dialog" Style="display: none" />
    <cc1:ModalPopupExtender ID="MPEActivitySequence" runat="server" TargetControlID="btnShow"
        PopupControlID="DivPopSequence" BackgroundCssClass="modalBackground" BehaviorID="MPEId">
    </cc1:ModalPopupExtender>
    <div id="DivPopSequence" runat="server" class="centerModalPopup" style="display: none; left: 521px; width: 50%; position: absolute; top: 525px; max-height: 404px;">
        <table width="100%" cellpadding="5px">
            <tr>
                <td align="left" class="Label" style="text-align: center; height: 22px;" valign="top">
                    <h1 class="header">
                        <asp:Label runat="server" ID="lblHeading" Text="Activity Deviation" class="LabelBold" /></h1>
                </td>
            </tr>
        </table>
        <div style="width: 100%; left: 204px; top: 77px" id="divTV" runat="server">
            <table width="100%">
                <tbody>
                    <tr>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <div style="overflow: auto; max-height: 100px; margin: auto; width: 100%;">
                                            <asp:Panel runat="server" ID="Deviation">
                                                <asp:PlaceHolder ID="PlaceDeviation" runat="server"></asp:PlaceHolder>
                                            </asp:Panel>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: left;">
                                        <asp:Label ID="lbl" runat="server" Text="Do You Want To Continue? "></asp:Label>
                                        <asp:LinkButton ID="lbtnForSub" runat="server" Text="Sequence" Style="display: none;"></asp:LinkButton>
                                        <label id="btnPforStruct" onclick="open_ProjStruct();" onmouseover="funOnMouseOver(this);">
                                            <u>Structure Management</u></label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            <table width="100%">
                                                <tr>
                                                    <td style="text-align: right; width: 10%;">
                                                        <asp:Label ID="lblRemark" runat="server" Text="Remarks:  " Style="color: Navy; font-weight: bold;"></asp:Label>
                                                    </td>
                                                    <td style="text-align: left;">
                                                        <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" Width="88%"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left;" valign="top">
                                                    <div>
                                                        <asp:Label ID="lblError" runat="server" Style="color: Red;"></asp:Label>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: center;">
                                        <asp:Button ID="btnOk" Text="Ok" ToolTip="Ok" CssClass="btn btnnsave" runat="server" OnClientClick="return chk_Remark();" />
                                        <input type="button" id="btnCancel" title="Cancel" value="Cancel" class="btn btncancel"
                                            onclick="unCheckSelected();" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>


    <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
        <ContentTemplate>
            <button id="BtnDataentryControl" runat="server" style="display: none;" />
            <cc1:ModalPopupExtender ID="MpeDataentryControl" runat="server" PopupControlID="divDataentryControl"
                BackgroundCssClass="modalBackground" TargetControlID="BtnDataentryControl">
            </cc1:ModalPopupExtender>
            <div style="display: none; width: 40%; height: 120px; text-align: left; left: 35% !important; background-color: White;"
                id="divDataentryControl" class="centerModalPopup">
                <table style="width: 100%; margin: auto">
                    <tr align="center">
                        <td style="font-size: 14px !important; text-align: center" class="Label">Data-Entry Control
                                                                                                                <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span style="font-weight: 12px;">For this subject some work is already going on by
                                                                                                                    <asp:Label ID="lblDataEntrycontroller" runat="server"></asp:Label>
                                You Can not continue. </span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <input type="submit" id="btnExitDataEntryControl" class="btn btnexit" onclick="return msgconfirmalert('Are you sure you want to Exit?', this);"
                                value="Exit" title="Exit" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvwSubjectSample" EventName="RowCommand" />
            <asp:AsyncPostBackTrigger ControlID="txtScan" EventName="TextChanged" />
            <%--<asp:AsyncPostBackTrigger ControlID="btnSaveandredirect" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnReplaceOK" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnSaveSubject" />--%>
        </Triggers>
    </asp:UpdatePanel>
    <asp:HiddenField ID="HsubjectId" runat="server" />
    <asp:HiddenField ID="HPendingNode" runat="server" />
    <asp:HiddenField ID="hremark" runat="server" />
    <asp:HiddenField ID="hdnFrom" runat="server" />

    <!-- Add by Anand -->      
    <script src="./Script/googleapis.js" type="text/javascript"></script>
    <!-- END -->
    <script src="Script/jquery.cookie.js" type="text/javascript"></script>
    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <script type="text/javascript" src="Script/Gridview.js"></script>
    <script type="text/javascript" src="Script/General.js"></script>
    
   

    <asp:PlaceHolder ID="plhScript" runat="server">
        <script type="text/javascript" language="javascript">
            var workSpaceId;
            var nodeId;
            var periodId;
            var subjectId;
            //var mySubjectNo;
            function IsNewTab() {
                return $.cookie(workSpaceId + ":" + nodeId + ":" + periodId + ":" + subjectId);
            }
            function Open(oArg) {
                debugger;
                workSpaceId = oArg.wId;
                nodeId = oArg.nId;
                periodId = oArg.pId;
                subjectId = oArg.sId;
                //mySubjectNo = oArg.mNo;

                //alert("Hello " + fixedName + "!");
                if (!IsNewTab()) {
                    $.cookie(workSpaceId + ":" + nodeId + ":" + periodId + ":" + subjectId, "YES", {
                        path: '/'
                    });


                } else {
                    msgalert('Data Entry on this Activity already Going on !');
                    //document.getElementById('<%= HdnCookie.ClientID%>').value = '';
                    document.getElementById('<%= btnForceKillPage.ClientID%>').click();
                    //document.getElementById('ctl00_CPHLAMBDA_iFrmDynamicPage').style.display = 'none';
                    //document.getElementById('ctl00_CPHLAMBDA_txtScan').value = '';
                    //document.getElementById('ctl00_CPHLAMBDA_txtScan').focus();

                    //return false;


                    //OR
                    //window.close()
                }
            }
            function OpenAlready() {
                debugger;
                msgalert('Data Entry on this Activity already Going on !');
                //document.getElementById('<%= HdnCookie.ClientID%>').value = '';
                document.getElementById('<%= btnForceKillPage.ClientID%>').click();
                return false;
            }
            function RemoveCookie(oArg,Iframe) {
                debugger;
                var CookieVal = oArg.hCookie;
                var Iframe1 = oArg.Frame123

                //alert(CookieVal);
                if (CookieVal != '') {
                    var workspaceid = document.getElementById('<%= HProjectId.ClientID%>').value;
                    var nodeid = document.getElementById('<%= ddlActivity.ClientID%>').value.split("#")[1];
                    var period = document.getElementById('<%= ddlPeriod.ClientID%>').value;
                    var subjectid = document.getElementById('<%= txtScan.ClientID%>').value.trim();
                    var cookieValue = CookieVal.split(":");

                    $.removeCookie(CookieVal,null, {
                        path: '/'
                    });

                    var obj = new Object();
                    var ds_DataentryControl = new Array();
                    var cookieValue = CookieVal.split(":");
                    obj.vWorkspaceId = cookieValue[0];
                    obj.iNodeId = cookieValue[1];
                    obj.vSubjectId = cookieValue[3];
                    obj.iModifyBy = '<%= Session(S_UserID)%>';
                    obj.iWorkflowStageId = '<%= Session(S_WorkFlowStageId)%>';
                    ds_DataentryControl.push(obj);

                    var content = {};
                    content.Choice_1 = "3";
                    content.ds_DataentryControl = ds_DataentryControl;
                    var JsonText = JSON.stringify(content);

                    $.ajax(
                          {
                              type: "POST",
                              url: "Ws_Lambda_JSON.asmx/insert_DataEntryControl",
                              data: JsonText,
                              contentType: "application/json; charset=utf-8",
                              dataType: "json",
                              success: function (response) {

                              },
                              failure: function (error) {
                                  msgalert(error);
                              }

                          });
                }
                if (Iframe1 == 1) {
                     iFramebtnClicked1()
                }
                

            }
            function CompareCookie(oArg) {
                var CookieVal = oArg.hCookie;
                if ($.cookie(CookieVal)) {
                    msgalert('Data Entry on this Activity already Going on !');
                    //document.getElementById('<%= HdnCookie.ClientID%>').value = '';
                    document.getElementById('<%= btnForceKillPage.ClientID%>').click();
                    return false;

                }

            }
            $(window).unload(function () {

                $.removeCookie(document.getElementById('<%= HdnCookie.ClientID%>').value, {
                    path: '/'
                });


                var CookieVal = document.getElementById('<%= HdnCookie.ClientID%>').value;
                document.getElementById('<%= HdnCookie.ClientID%>').value = '';
                var obj = new Object();
                var ds_DataentryControl = new Array();
                var cookieValue = CookieVal.split(":");
                obj.vWorkspaceId = cookieValue[0];
                obj.iNodeId = cookieValue[1];
                obj.vSubjectId = cookieValue[3];
                obj.iModifyBy = '<%= Session(S_UserID)%>';
                obj.iWorkflowStageId = '<%= Session(S_WorkFlowStageId)%>';
                ds_DataentryControl.push(obj);

                var content = {};
                content.Choice_1 = "3";
                content.ds_DataentryControl = ds_DataentryControl;
                var JsonText = JSON.stringify(content);
                $.ajax(
                      {
                          type: "POST",
                          url: "Ws_Lambda_JSON.asmx/insert_DataEntryControl",
                          data: JsonText,
                          async: false,
                          contentType: "application/json; charset=utf-8",
                          dataType: "json",
                          success: function (response) {

                          },
                          failure: function (error) {
                              msgalert(error);
                          }

                      });

            });
            function SearchClearCookie(oArg) {
                var CookieVal = oArg.hCookie;
                $.removeCookie(CookieVal,null, {
                    path: '/'
                });

                document.getElementById('<%= HdnCookie.ClientID%>').value = '';
                var obj = new Object();
                var ds_DataentryControl = new Array();
                var cookieValue = CookieVal.split(":");
                obj.vWorkspaceId = cookieValue[0];
                obj.iNodeId = cookieValue[1];
                obj.vSubjectId = cookieValue[3];
                obj.iModifyBy = '<%= Session(S_UserID)%>';
                obj.iWorkflowStageId = '<%= Session(S_WorkFlowStageId)%>';
                ds_DataentryControl.push(obj);

                var content = {};
                content.Choice_1 = "3";
                content.ds_DataentryControl = ds_DataentryControl;
                var JsonText = JSON.stringify(content);

                $.ajax(
                      {
                          type: "POST",
                          url: "Ws_Lambda_JSON.asmx/insert_DataEntryControl",
                          data: JsonText,
                          contentType: "application/json; charset=utf-8",
                          dataType: "json",
                          success: function (response) {

                          },
                          failure: function (error) {
                              msgalert(error);
                          }

                      });
            }
        </script>

    </asp:PlaceHolder>
    <script type="text/javascript" language="javascript">
        function pageLoad() {
            document.getElementById('<%=txtScan.clientid %>').focus();

      
        }
        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }
        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        function CheckSelected() {

            var Gdv = document.getElementById('<%=gvwSubjects.ClientId %>');
            var str = "";
            for (c = 1; c < Gdv.rows.length - 1; c++)// -1 as gdv row count contains both header and footer
            {
                if (Gdv.rows[c].children[0].children[0].type == "checkbox") {
                    if (Gdv.rows[c].children[0].children[0].checked == true) {
                        if (str.toString() == "") {
                            str = str + Gdv.rows[c].children[2].innerText;
                        }
                        else {
                            str = str + "," + Gdv.rows[c].children[2].innerText;
                        }
                    }
                }
            }
            document.getElementById('<%=HsubjectId.ClientID %>').value = str.toString();

        }
        function SelectAll(CheckBoxControl, Grid) {
            var str = "";
            var Gvd = document.getElementById('<%=gvwSubjects.ClientId %>');
            if (CheckBoxControl.checked == true) {
                var i;
                var Cell = 0;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf(Grid) > -1)) {
                        if (document.forms[0].elements[i].disabled == false) {
                            document.forms[0].elements[i].checked = true;
                            Cell += 1;

                            if (str.toString() == "") {
                                str = str + Gvd.lastChild.childNodes[Cell].children[2].innerText;
                            }
                            else {
                                str = str + "," + Gvd.lastChild.childNodes[Cell].children[2].innerText;
                            }

                        }
                    }
                }
                document.getElementById('<%=HsubjectId.ClientID %>').value = str.toString();

            }
            else {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf(Grid) > -1)) {
                        document.forms[0].elements[i].checked = false;
                    }
                }
            }
        }
        function PassTheBarcode() {
            if (document.getElementById('<%=txtScan.ClientID %>').value.length > 0) {
                document.getElementById('<%=txtScan.ClientID%>').click();
                //__doPostBack('ctl00$CPHLAMBDA$txtScan', '');

            }
            return false;
        }
        function PassTheBarcodeTextBox() {
            if (document.getElementById('<%=txtScan.ClientID%>').value.length > 0) {
                //RemoveCookie(document.getElementById('<%=HdnCookie.ClientID%>').value);
                $('#ctl00_CPHLAMBDA_txtScan')[0].disabled = true;
                __doPostBack('ctl00$CPHLAMBDA$txtScan', '');
            }
            return false;
        }
        function chk_Remark() {
            var txtContent = document.getElementById('<%= txtContent.ClientID %>').value.toString().trim();

            if (txtContent == "") {
                document.getElementById('<%=lblError.ClientID %>').innerHTML = "Please Enter Remark";
                return false;
            }
            else {
                document.getElementById('<%=hremark.ClientId %>').value = txtContent.toString();
            }
            return true;
        }
        function CheckAtleastOne(gv) {
            var gvwSubject = document.getElementById('<%= gvwSubjects.ClientID %>');

            if (CheckOne(gvwSubject.id) == false) {
                msgalert('Please Select Atleast One Subject !');
                return false;
            }
            return true;
        }

        function iFramebtnClicked1() {
            $('#ctl00_CPHLAMBDA_txtScan')[0].disabled = true
            var btnSaveandRedirect = document.getElementById('<%=btnSaveandredirect.ClientID%>');
            if ($('#<%=iFrmDynamicPage.ClientID%>').get(0)) {
                var frame = $('#<%=iFrmDynamicPage.ClientID%>').get(0).contentDocument;

                //alert(frame.getElementById("button2"));
                //alert(frame);

                if (frame) {
                    var btnSave = frame.getElementById("btnSaveAndContinue");

                    if (btnSave) {
                        btnSave.click();
                    }
                    else {
                        btnSaveandRedirect.click();
                    }
                }
                else {
                    btnSaveandRedirect.click();
                }
            }
            else {
                btnSaveandRedirect.click();
            }

        }
        function iFramebtnClicked(type) {

            var frame = $('#<%=iFrmDynamicPage.ClientID%>').get(0).contentDocument;
            //alert(frame.getElementById("button2"));
            //alert(frame);

            if (frame) {
                var btnSave = frame.getElementById("btnSaveAndContinue");
                if (btnSave) {
                    btnSave.click();
                }
                else {
                    if (type == 'textbox') {
                        $('#<%=iFrmDynamicPage.ClientID%>').attr('src', document.getElementById('<%=hdnDynamicURL.ClientID %>').value);
                    }
                    else {
                        $('#<%=iFrmDynamicPage.ClientID%>').attr('src', document.getElementById('<%=hdnDynamicURL.ClientID %>').value);
                    }

                }
            }
        }

        function unCheckSelected() {
            var Gdv = document.getElementById('<%=gvwSubjects.ClientId %>');
            $find('MPEId').hide();
            $find('MPESubjectManagement1').show();
            for (c = 1; c < Gdv.rows.length - 1; c++)// -1 as gdv row count contains both header and footer
            {
                if (Gdv.rows[c].childNodes[0].childNodes[0].type == "checkbox") {
                    if (Gdv.rows[c].childNodes[0].childNodes[0].checked == true) {
                        Gdv.rows[c].childNodes[0].childNodes[0].checked = false;
                    }
                }
            }

            document.getElementById('<%=HsubjectId.ClientID %>').value = "";

        }

      
        //$('#ctl00_CPHLAMBDA_txtScan').keypress(function (event) {
        //    if (event.keyCode == 13) {
        //        $('#ctl00_CPHLAMBDA_txtScan')[0].disabled = true
        //    }
        //});

     
    

    </script>
</asp:Content>

