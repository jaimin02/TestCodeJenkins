<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmUserMaster.aspx.vb" Inherits="frmUserMaster" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:content id="Content1" contentplaceholderid="CPHLAMBDA" runat="Server">
    <style type="text/css">
        
        #Activityrecord_wrapper th:nth-last-child(0n+4) {
            width:90px !important;
        } 

        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }
        .dateWidth {
            width:90px;
        }
        #ctl00_CPHLAMBDA_GV_User_wrapper {
            /*margin: 0px 235px;*/
        }

        #loadingmessage {
            display: none;
            position: fixed;
            z-index: 1000;
            top: 0;
            left: 0;
            height: 100%;
            width: 100%;
            background: rgba( 255, 255, 255, .5 ) url('images/AjaxLoader.gif') 50% 50% no-repeat;
        }
        #tblUserMstAudit_wrapper {
          width: 1106px;
          overflow:auto;
        }
        #tblUserMstAudit {
            width:99% !important;
        }
        /*.ui-widget-header .ui-state-default {
        width:90%;
        }*/

      
    </style>

     <script type="text/javascript" src="Script/General.js"></script>
<script src="Script/Validation.js" language="javascript" type="text/javascript"></script>
<script type="text/javascript" src="Script/Gridview.js"></script>
 <script src="Script/popcalendar.js" language="javascript" type="text/javascript"></script>
   <script type="text/javascript" src="Script/jquery-1.4.3.min.js"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
 <script src="Script/jquery.dataTables.plugins.js" type="text/javascript"></script>
  <script src="Script/FixedHeader.min.js" type="text/javascript"></script>

      <div id="Div1" runat="server">
        <asp:GridView  ID="gvExport" runat="server" AutoGenerateColumns="true" Style="display: none"
            HeaderStyle-BackColor="#1560a1" HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" Header-style-font-size="10px !importnat" HeaderStyle-Font-Names="Verdana, Arial, Helvetica, sans-serif"
            HeaderStyle-Font-Size=" 0.9em"
            HeaderStyle-HorizontalAlign="Center" CellPadding="3" CellSpacing="0"
            RowStyle-Font-Names=" Verdana, Arial, Helvetica, sans-serif" RowStyle-Font-Size="10pt"
            RowStyle-BackColor="#84c8e6" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#191970" RowStyle-ForeColor="#191970">
        </asp:GridView>
         </div>


    <asp:UpdatePanel ID="Up_General" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table align="center" width="100%" cellpadding="5px">
                <tbody>
                    <tr>
                        <td>
                            <fieldset class="FieldSetBox" style="display: block; width: 98%; text-align: left; margin: auto; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img1" alt="User Detail" src="images/panelcollapse.png"
                                        onclick="Display(this,'divUser');" runat="server" style="margin-right: 2px;" />User Detail</legend>
                                <div id="divUser">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 30%; text-align: right;" class="Label">User Group* :
                                            </td>
                                            <td style="width: 23%; text-align: left;">
                                                <asp:DropDownList ID="DDLUserGroup" runat="server" CssClass="dropDownList" Width="71%" />
                                            </td>
                                            <td class="Label" style="width: 11%; text-align: right;">Scope type* :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:DropDownList ID="ddlScope" runat="server" CssClass="dropDownList" Width="40%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Label" style="text-align: right;">First Name :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="TxtFirstName" runat="server" CssClass="textBox" Width="70%" MaxLength="50" />
                                            </td>
                                            <td class="Label" style="text-align: right;">Last Name :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="TxtLastName" runat="server" CssClass="textBox" Width="40%" MaxLength="50" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Label" style="text-align: right;">Department* :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:DropDownList ID="DDLDept" runat="server" CssClass="dropDownList" Width="71%" />
                                            </td>
                                            <td class="Label" style="text-align: right;">Location * :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:DropDownList ID="DDLLocation" runat="server" CssClass="dropDownList" Width="40%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Label" style="text-align: right;">User Name* :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtUName" runat="server" CssClass="textBox" Width="70%" MaxLength="50" />
                                            </td>
                                            <td class="Label" style="text-align: right;">User Type* :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:DropDownList ID="DDLUserType" runat="server" CssClass="dropDownList" Width="40%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Label" style="text-align: right;">Password*:
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox onblur="ValidateNamePassword(this.value);" ID="txtLPass" runat="server"
                                                    CssClass="textBox" Width="70%" ValidationGroup="A" TextMode="Password" MaxLength="50" />
                                            </td>
                                            <td class="Label" style="text-align: right;">Email Id :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtEmail" runat="server" CssClass="textBox" Width="40%" ValidationGroup="B"
                                                    CausesValidation="True" MaxLength="100" />
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Width="360px"
                                                    ValidationGroup="B" ErrorMessage="Please Enter Email Id in Correct Format." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                    SetFocusOnError="True" ControlToValidate="txtEmail" Display="Dynamic" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Label" style="text-align: right;">Phone No :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtPhNo" runat="server" CssClass="textBox" Width="70%" MaxLength="20" />
                                            </td>
                                            <td class="Label" style="text-align: right;">Ext. No :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtExtNo" runat="server" CssClass="textBox" Width="40%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Label" style="text-align: right;">Valid From :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtloginfrom" runat="server" Width="70%" CssClass="textBox" />
                                                <cc1:CalendarExtender ID="CalExPeriod2CheckInDate" runat="server" Format="dd-MMM-yyyy"
                                                    TargetControlID="txtloginfrom">
                                                </cc1:CalendarExtender>
                                            </td>
                                            <td class="Label" style="text-align: right;">Valid To :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtLoginTo" runat="server" Width="40%" CssClass="textBox" />
                                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                                    TargetControlID="txtLoginTo">
                                                </cc1:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Label" style="text-align: right;">Remark :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtRemark" runat="server" CssClass="textBox" Width="69%" TextMode="MultiLine" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Label" style="text-align: center;" colspan="4">
                                                <asp:Button ID="BtnSave" runat="server" Text="Save" ToolTip="Save" CssClass="btn btnsave"
                                                    OnClientClick=" return Validation();" />
                                                <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Text="Cancel"
                                                    ToolTip="Cancel" CssClass="btn btncancel" />
                                                <asp:Button ID="BtnExit" runat="server" Text="Exit" ToolTip="Exit" CssClass="btn btnexit"
                                                    OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />

                                                  <asp:Button ID="btnEdit" runat="server" Text="Edit" ToolTip="Edit"   style="display:none" />
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
            <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="BtnExit" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="Up_View" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table style="width: 100%; text-align: center;">
                <tbody>
                    <tr>
                        <td>
                            <fieldset class="FieldSetBox" style="display: block; width: 97%; text-align: left; margin: auto; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img4" alt="User Data" src="images/panelcollapse.png"
                                        onclick="Display(this,'divUserData');" runat="server" style="margin-right: 2px;" />User Data</legend>
                                <div id="divUserData">
                                    <table width="100%">
                                        <tr>
                                            <td style="white-space: nowrap; padding-top: 1%; text-align: center;" colspan="4">
                                                <asp:Button ID="btnInactUser" runat="server" Text="Inactive User" ToolTip="Inactive User"
                                                    CssClass="btn btnnew" OnClientClick="return Inactiveuser(); "/>
                                                <asp:Button ID="btnactUser" runat="server" Text="Active User" ToolTip="Active User"
                                                    CssClass="btn btnnew"  OnClientClick="return activeuser(); " />
                                                <asp:Button ID="btnalluser" runat="server" Text="All User" ToolTip="All User" CssClass="btn btnnew" OnClientClick="return  fnGetData()" />
                                                <asp:Button ID="btncntuser" runat="server" Text="Active Sessions" ToolTip="Active Session"
                                                    CssClass="btn btnnew" />
                                                <asp:Button ID="btnhistoryuser" runat="server" Text="User History" ToolTip="User History"
                                                    CssClass="btn btnnew" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <!-- -->
                                            <td style="padding-top: 1%; text-align: center;" colspan="4">
                                                <b>
                                                    <asp:Label ID="lblexceltitle" runat="server"  
                                                        Visible="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center; width: 100%;" colspan="4">
                                                <div style="height: 400px; width: 1200px; overflow: auto; margin: auto;">
                                                    <asp:GridView ID="GV_User" runat="server" Style="width: auto; margin: auto; display: none;" OnPageIndexChanging="GV_User_PageIndexChanging"
                                                        AutoGenerateColumns="False" OnRowCreated="GV_User_RowCreated"
                                                        OnRowCommand="GV_User_RowCommand" OnRowDataBound="GV_User_RowDataBound">
                                                        <Columns>
                                                            <asp:BoundField HeaderText="#">
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="iUserId" HeaderText="User Id" />
                                                            <asp:BoundField DataField="vUserName" HeaderText="User Name" />
                                                            <asp:BoundField DataField="vScopeName" HeaderText="Scope" />
                                                            <asp:BoundField DataField="iUserGroupCode" HeaderText="User Group Code" />
                                                            <asp:BoundField DataField="vUserGroupName" HeaderText="User Group Name" />
                                                            <asp:BoundField DataField="vFirstName" HeaderText="First Name" />
                                                            <asp:BoundField DataField="vLastName" HeaderText="Last Name" />
                                                            <asp:BoundField DataField="vLoginName" HeaderText="Login Name" />
                                                            <asp:BoundField DataField="vLoginPass" HeaderText="Login PassWord" />
                                                            <asp:BoundField DataField="vUserTypeCode" HeaderText="User Type Code" />
                                                            <asp:BoundField DataField="vUserTypeName" HeaderText="User Type Name" />
                                                            <asp:BoundField DataField="vDeptCode" HeaderText="Department Code" />
                                                            <asp:BoundField DataField="vDeptName" HeaderText="Department Name" />
                                                            <asp:BoundField DataField="vLocationCode" HeaderText="Location Code" />
                                                            <asp:BoundField DataField="vLocationName" HeaderText="Location Name" />
                                                            <asp:BoundField DataField="vEmailId" HeaderText="Email Id" />
                                                            <asp:BoundField DataField="vPhoneNo" HeaderText="Phone No." />
                                                            <asp:BoundField DataField="vExtNo" HeaderText="Ext No." />
                                                            <asp:BoundField DataField="vRemark" HeaderText="Remark" />
                                                            <asp:BoundField DataField="dModifyOn" HeaderText="ModifyOn" >
                                                                <ItemStyle Wrap="false"/>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ModifierName" HeaderText="Modifier Name" />
                                                            <asp:TemplateField HeaderText="Edit" SortExpression="status">
                                                                <ItemTemplate>
                                                                    <%--<asp:LinkButton ID="lnkEdit" Text="Edit" runat="server"></asp:LinkButton>--%>
                                                                    <asp:ImageButton ID="lnkEdit" runat="server" ImageUrl="~/images/Edit2.gif" ToolTip="Edit" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Audit Trail">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="lnkAudit" runat="server" ImageUrl="~/Images/audit.png" ToolTip="Audit Trial" OnClientClick="AudtiTrail(this); return false;" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Export">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <img src="images/Export.gif" onclick="ExportToExcel(this.id);" id='<%# Eval("vUserName")%>' />
                                                                    </center>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                        </Columns>
                                                    </asp:GridView>
                                                      <div id ="createtable"> 
                   </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="white-space: nowrap; text-align: center;" colspan="4">
                                                <asp:Button ID="btnexportexcel" runat="server" ToolTip="Export To Excel"
                                                    CssClass="btn btnexcel" />

                                                <asp:HiddenField ID="hdnActiveData" runat="server" />
                                                
                                            </td>
                                        </tr>
                                    </table>
                                    <!--  added for current user by Mrunal Parekh   on 16-Oct-2011 -->
                                    <table style="text-align: center; width: 100%;">
                                        <tbody>
                                            <%--  <tr>
                                <td>
                                    <fieldset class="FieldSetBox" style="display: block; width: 98%; text-align: left; border: #aaaaaa 1px solid;">
                                        <legend class="LegendText" style="color: Black; font-size: 12px">
                                            <img id="img3" alt="Patient Details" src="images/panelcollapse.png"
                                                onclick="Display(this,'divPatientDetail');" runat="server" style="margin-right: 2px;" />Project Details</legend>
                                        <div id="divPatientDetail">
                                            <table width="100%">--%>


                                            <tr>
                                                <td>
                                                    <button id="btn2" runat="server" style="display: none;" />
                                                    <cc1:ModalPopupExtender ID="Mpusermst" runat="server" BackgroundCssClass="modalBackground"
                                                        CancelControlID="imgclose" PopupControlID="Divcntuser" PopupDragHandleControlID="lblTitle"
                                                        TargetControlID="btn2">
                                                    </cc1:ModalPopupExtender>
                                                    <div id="Divcntuser" runat="server" style="position: fixed !important; display: none; background-color: white; width: 60%; max-height: 550px; border: dotted 1px gray;"
                                                        class="centerModalPopup">
                                                        <div style="width: 100%; margin: 0 auto">
                                                            <img id="imgclose" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px;"
                                                                title="Close" />
                                                            <asp:Label ID="lblTitle" Text="Active Sessions" runat="server" align="center"
                                                                ForeColor="black" Font-Bold="true" />
                                                            <br />
                                                            <hr />
                                                            <table border="0" cellpadding="2" cellspacing="1" align="center" style="width: 100%">
                                                                <tbody>
                                                                    <tr>
                                                                        <td style="width: 100%;">
                                                                            <div style="max-height: 400px; overflow: auto; x-overflow: hidden; width: auto">
                                                                                <asp:GridView ID="GVcntuser" runat="server" AutoGenerateColumns="False" BorderColor="Peru"
                                                                                    Font-Size="Small" SkinID="grdViewSmlAutoSize" Style="width: 100%;">
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="vUserName" HeaderText="User Name" />
                                                                                        <asp:BoundField DataField="vUsertypeName" HeaderText="User Profile" />
                                                                                        <asp:BoundField DataField="dLoginDateTime" HeaderText="Login Time" DataFormatString="{0:dd-MMM-yyyy HH:mm}" />
                                                                                        <asp:BoundField DataField="vIPAddress" HeaderText="IP Address" />
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <hr />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center">
                                                                            <asp:Button ID="exclcntuser" runat="server" Text="" ToolTip="Export To Excel"
                                                                                CssClass="btn btnexcel"/>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                            
                                        </tbody>
                                    </table>
                                    <!-------------------------------- -->
                                    <!--   for User Login History  -->
                                    <table align="center" width="100%">
                                        <tbody>
                                            
                                            <tr>
                                                <td>
                                                    <button id="btn3" runat="server" style="display: none;" />
                                                    <cc1:ModalPopupExtender ID="MPEUserhistory" runat="server" BackgroundCssClass="modalBackground"
                                                        CancelControlID="Img2" PopupControlID="Divuserhistory" PopupDragHandleControlID="lblTitle"
                                                        TargetControlID="btn3">
                                                    </cc1:ModalPopupExtender>
                                                    <div id="Divuserhistory" runat="server" style="position: relative; display: none; background-color: white; padding: 5px; left: 75px; width: 700px; height: 500px; border: dotted 1px gray;"
                                                        class="centerModalPopup">
                                                        <div>
                                                            <img id="Img2" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px;"
                                                                title="Close" />
                                                           
                                                            <asp:Label ID="Label1" runat="server" Text="User Login History" class="LabelText"
                                                                Style="font-size: 12px !important;" ForeColor="Black" Font-Bold="true" align="center" />
                                                            <br />
                                                            <hr />
                                                        </div>
                                                        <div>
                                                            <table style="width: 100%;">
                                                                <tbody>
                                                                    <tr>
                                                                        <td align="center" colspan="5" style="width: 500px;">
                                                                            <div style="padding-top: 1%">
                                                                                <table style="width: 100%;">
                                                                                    <tr>
                                                                                        <td style="padding-top: 2%;">
                                                                                            <asp:Label runat="server" Text="From Date :" ID="LblFromDateForuserhistory" Enabled="True" />
                                                                                            <asp:TextBox runat="server" ID="TxtFromDateOfuserhistory" Text="" CssClass="textBox"
                                                                                                Enabled="True"></asp:TextBox>
                                                                                            <cc1:CalendarExtender ID="CalExtFromDateForuserhistory" runat="server" TargetControlID="TxtFromDateOfuserhistory"
                                                                                                Format="dd-MMM-yyyy">
                                                                                            </cc1:CalendarExtender>
                                                                                            <asp:Label runat="server" Text="To Date :" ID="LblToDateForuserhistory" Enabled="True" />
                                                                                            <asp:TextBox runat="server" ID="TxtToDateOfuserhistory" Text="" CssClass="textBox"
                                                                                                Enabled="True"></asp:TextBox>
                                                                                            <cc1:CalendarExtender ID="CalExtToDateForuserhistory" runat="server" TargetControlID="TxtToDateOfuserhistory"
                                                                                                Format="dd-MMM-yyyy">
                                                                                            </cc1:CalendarExtender>
                                                                                        </td>
                                                                                        <td style="padding-top: 15px">
                                                                                            <asp:Button ID="BtnGoForuserhistory" runat="server" CssClass="btn btngo" Text="" ToolTip="Go"
                                                                                                Enabled="True" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>

                                                                                        <td style="text-align: center;" colspan="4">
                                                                                            <div style="height: 350px; overflow: auto; width: 100%;">
                                                                                                <asp:GridView ID="GVuserhistory" runat="server" AutoGenerateColumns="False" BorderColor="Peru"
                                                                                                    Font-Size="Small" SkinID="grdViewSmlAutoSize" Width="100%">
                                                                                                    <Columns>
                                                                                                        <asp:BoundField DataField="vUserName" HeaderText="User Name" />
                                                                                                        <asp:BoundField DataField="vUsertypeName" HeaderText="User Profile" />
                                                                                                        <asp:BoundField DataField="cLOFlag" HeaderText="Status" />
                                                                                                        <asp:BoundField DataField="dInOutDateTime" HeaderText="Login/Logout Time" DataFormatString="{0:dd-MMM-yyyy HH:mm}" />
                                                                                                        <asp:BoundField DataField="vIPAddress" HeaderText="IP Address" />
                                                                                                    </Columns>
                                                                                                </asp:GridView>
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                         <td style="padding-top: 10px; text-align: center;" colspan="4">
                                                                                            <asp:Button ID="btnexptuserhistory" runat="server" Text="" ToolTip="Export To Excel"
                                                                                                CssClass="btn btnexcel"/>
                                                                                        </td>
                                                                                    </tr>
                                                                                    
                                                                                </table>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>

                                        </tbody>
                                    </table>
                                    <!--------------------------------------- -->
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnSave" />
            <asp:PostBackTrigger ControlID="btnactUser" />
            <asp:AsyncPostBackTrigger controlid="btninactuser" />
            <asp:PostBackTrigger ControlID="btnalluser" />
            <asp:PostBackTrigger ControlID="btnexportexcel" />
            <asp:PostBackTrigger ControlID="exclcntuser" />
            <asp:PostBackTrigger ControlID="btnexptuserhistory" />
        </Triggers>
    </asp:UpdatePanel>

      <asp:HiddenField ID="hdnEditedId" runat="server" />

    <asp:HiddenField runat="server" ID="hdnUserName" />
    <asp:Button ID="btnExportToExcel" runat="server" Style="display: none;" />

    <div>
        <table width="100%">
        </table>
    </div>
    <div>
                <table>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpUserAuditTrail" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <div id="dvUserMstAudiTrail" runat="server" class="centerModalPopup" style="display: none; overflow: auto; width: 83%; height: auto; max-height: 75%; min-height: auto;">
                                        <table border="0" cellpadding="2" cellspacing="2" width="80%">
                                            <tr>
                                                <td id="Td2" class="LabelText" style="font-weight: bold; background-color: aliceblue; text-align: center !important; font-size: 15px !important; width: 97%;">
                                                    <asp:Label ID="lblRamge" runat="server" Text="Audit Trail Information"></asp:Label>
                                                </td>
                                                <td style="width: 3%">
                                                    <img id="imgUserAuditTrail" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
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
                                                                <table id="tblUserMstAudit" class="tblAudit" border='1' style="background-color: aliceblue; width: 100% !important; overflow:auto;display: block; height: 200px;"></table>
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
            </div>
     <button id="Button1" runat="server" style="display: none;" />
            <cc1:ModalPopupExtender ID="MPE_CleintMstHistory" runat="server" PopupControlID="dvUserMstAudiTrail" BehaviorID="MPE_CleintMstHistory"
                PopupDragHandleControlID="LbldRandomizationDtl" BackgroundCssClass="modalBackground" CancelControlID="imgUserAuditTrail"
                TargetControlID="Button1">
            </cc1:ModalPopupExtender>
            <div id='loadingmessage' style='display:none'>
                   
                </div>
   
    <script type="text/javascript" language="javascript">

        $(document).ready(function () {
            debugger;
            fnGetData();
            return false;
        });

        function HideSponsorDetails() {
            $('#<%= img1.ClientID%>').click();
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

        function UIGV_User() {
            $('#<%= GV_User.ClientID%>').removeAttr('style', 'display:block');
            $('#<%= GV_User.ClientID%>').find('tbody tr').length < 3 ? scroll = "25%" : scroll = "100px";
            oTab = $('#<%= GV_User.ClientID%>').prepend($('<thead>').append($('#<%= GV_User.ClientID%> tr:first'))).dataTable({
                "bJQueryUI": true,
                "sScrollY": "285px",
                "sScrollX": "100%",
                "bScrollCollapse": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": false,
                aLengthMenu: [
                    [10, 25, 50, 100, -1],
                    [10, 25, 50, 100, "All"]
                ],
                //"columnDefs": [
                //    { "width": "90%", "targets": 15 }
                //]
            }); 
            return false;
        }

        var cMONTHNAMES = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug',
                   'Sep', 'Oct', 'Nov', 'Dec'];
        function Validation() {
            if (document.getElementById('<%=DDLUserGroup.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select User Group !');
                document.getElementById('<%=DDLUserGroup.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=ddlScope.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Scope !');
                document.getElementById('<%=ddlScope.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=DDLDept.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Department !');
                document.getElementById('<%=DDLDept.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=DDLLocation.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Location !');
                document.getElementById('<%=DDLLocation.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtUName.ClientID%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=txtUName.ClientID%>').value = '';
                msgalert('Please Enter User Name !');
                document.getElementById('<%=txtUName.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=DDLUserType.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select User Type !');
                document.getElementById('<%=DDLUserType.ClientID%>').focus();
                return false;
            }

            else if (document.getElementById('<%=txtLPass.ClientID%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=txtLPass.ClientID%>').value = '';
                msgalert('Please Enter PassWord !');
                document.getElementById('<%=txtLPass.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtloginfrom.ClientID%>').value.toString().trim().length <= 0 && document.getElementById('<%=txtLoginTo.ClientID%>').value.toString().trim().length != 0) {
                msgalert('Please Enter Valid From Date !');
                document.getElementById('<%=txtloginfrom.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtLoginTo.ClientID%>').value.toString().trim().length <= 0 && document.getElementById('<%=txtloginfrom.ClientID%>').value.toString().trim().length != 0) {
                msgalert('Please Enter Valid To Date !');
                document.getElementById('<%=txtLoginTo.ClientID%>').focus();
                return false;
            }
    if (document.getElementById("<%=BtnSave.ClientID%>").value.trim() == "Update") {
                if (document.getElementById("<%=txtremark.ClientID%>").value.trim() == "") {
            msgalert("Please enter Remarks !");
            return false;
        }
    }
    return ValidatePassword();

}


function ValidateNamePassword(uname) {
    var lname;
    var pwd;
    Uname = document.getElementById('<%=txtUName.ClientID %>').value;
            pwd = document.getElementById('<%=txtLPass.ClientID %>').value;
            if (Uname != '' && pwd != '' && Uname == pwd) {
                msgalert('UserName And Password Should Not Be Same');
                (document.getElementById('<%=txtLPass.ClientID%>').value = '');
        document.getElementById('<%=txtLPass.ClientID%>').focus();
        return false;
    }
    return true;
}

function ValidatePassword() {
    //(?=.*[A-Z])
    var resAlhpa = "true";
    var resChar = "true";
    var alphaExp = /^[0-9]+$/;
    var charExp = /^[a-zA-Z]+$/;
    var pwdRegEx = /^(?=.*\d)(?=.*[a-z])\w{6,}$/;
    var txtPWD = document.getElementById('<%=txtLPass.ClientID%>');

    if (txtPWD.value.length > 5) {
        if (txtPWD.value.match(alphaExp)) {
            resAlhpa = "true";
        }
        else {
            resAlhpa = "false";
        }

        if (txtPWD.value.match(charExp)) {
            resChar = "true";
        } else {
            resChar = "false";
        }
    }

    if (resAlhpa == "false" && resChar == "false") {
        return true;
    } else {
        msgalert('Password Must Be More Than 6 Characters.\nPassword Must Contain Atleast One Alphabet [a-z].\nPassword Must Contain Atleast One Digit [0-9].');
        txtPWD.focus();
        return false;
    }
    return true;
}

function ShowAlert(msg) {
    alertdooperation(msg, 1, "frmUserMaster.aspx?mode=1");
    //alert(msg);
    //window.location.href = "frmUserMaster.aspx?mode=1";
}

function dateformatvalidator(mode) {

    if (mode == 1) {
        var str = 'ctl00_CPHLAMBDA_txtloginfrom';
    }
    else {
        var str = 'ctl00_CPHLAMBDA_txtLoginTo';
    }
    if (document.getElementById(str).value != "") {
        var textbox = document.getElementById(str);
        var currentDate = new Date();
        var date = currentDate.getDate() + "-" + cMONTHNAMES[currentDate.getMonth()] + "-" + currentDate.getFullYear();
        DateConvert(textbox.value, textbox);
        var difference = GetDateDifference(textbox.value, date);
        if (difference.Days > 0) {
            msgalert('Valid From/Valid To Should Not Be less Than Current Date');
            document.getElementById(str).value = '';
            document.getElementById(str).focus();
            return false;
        }
    }
    if (document.getElementById('ctl00_CPHLAMBDA_txtloginfrom').value != "" && document.getElementById('ctl00_CPHLAMBDA_txtLoginTo').value != "") {
        var fromdate = document.getElementById('ctl00_CPHLAMBDA_txtloginfrom');
        var todate = document.getElementById('ctl00_CPHLAMBDA_txtLoginTo');
        var gap = GetDateDifference(fromdate.value, todate.value);
        if (gap.Days < 0) {
            msgalert('Valid To Must Not Be Less Than Valid From Date');
            document.getElementById(str).value = '';
            document.getElementById(str).focus();
            return false;
        }
    }
}
var summarydata = '';
function fnGetData() {
    $('#ctl00_CPHLAMBDA_lblexceltitle').text("All User");
    $('#ctl00_CPHLAMBDA_hdnActiveData')
    $('#ctl00_CPHLAMBDA_hdnActiveData').val("All-User");

    debugger;
    $('#loadingmessage').show();

    $.ajax({
        type: "post",

        url: "frmUserMaster.aspx/View_UserMst",
        //data: '{"vWorkSpaceId":"' + vWorkSpaceId + '","vType":"' + vType + '"}',
        contentType: "application/json; charset=utf-8",
        datatype: JSON,
        async: true,
        dataType: "json",
        success: function (data) {
            var data = data.d;
            var msgs = JSON.parse(data);
            summarydata = msgs;
            if (summarydata == "") return false;
            // Table = Object(keys(summarydata))[0];
            CreateSummaryTable(summarydata);
            $('#loadingmessage').hide();
            return false;
        },
        failure: function (response) {
            msgalert("failure");
            msgalert(data.d);
        },
        error: function (response) {
            msgalert("error");
        }
    });
    return false;
}
function CreateSummaryTable(summarydata) {

    var ActivityDataset = [];
    var jsondata = summarydata.VIEW_USERMST;
    for (var Row = 0; Row < jsondata.length; Row++) {
        var InDataset = [];
        InDataset.push(Row + 1, jsondata[Row]['vUserName'], jsondata[Row]['vScopeName'], jsondata[Row]['vUserGroupName'], jsondata[Row]['vFirstName'], jsondata[Row]['vLastName'], jsondata[Row]['vLoginName'], jsondata[Row]['vUserTypeName'], jsondata[Row]['vDeptName'], jsondata[Row]['vLocationName'], jsondata[Row]['vEmailId'], jsondata[Row]['vPhoneNo'], jsondata[Row]['vExtNo'], jsondata[Row]['vRemark'], jsondata[Row]['tmp_dModifyOn'], "", "", "", jsondata[Row]['iUserId']);
        ActivityDataset.push(InDataset);

    }

    $ = jQuery;
    var createtable1 = $("<table id='Activityrecord'  border='1'  class='display'  cellspacing='0' width='100%'> </table>");
    $("#createtable").empty().append(createtable1);

    $('#Activityrecord').DataTable({
        "bJQueryUI": true,
        "sScrollY": "285px",
        "sScrollX": "100%",
        "bScrollCollapse": true,
        "sPaginationType": "full_numbers",
        "bLengthChange": true,
        "iDisplayLength": 10,
        "bProcessing": true,
        "bSort": false,
        "aaData": ActivityDataset,
        "fnCreatedRow": function (nRow, aData, iDataIndex) {
            $('td:eq(15)', nRow).append("<input type='image' id='imgedit" + iDataIndex + "' name='imgedit$" + iDataIndex + "' src='images/Edit2.gif'; tempval='" + aData[18] + "';   OnClick='Editcellrow(this); return false;' style='border-width:0px;'>");
            $('td:eq(16)', nRow).append("<input type='image' id='imgaudit" + iDataIndex + "' name='imgaudit$" + iDataIndex + "' src='images/audit.png'; tempval1='" + aData[18] + "';   OnClick='AudtiTrail(this); return false;'  style='border-width:0px;' >");
            $('td:eq(17)', nRow).append("<input type='image' id='imgexel" + iDataIndex + "' name='imgexel$" + iDataIndex + "' src='images/Export.gif'; tempval2='" + aData[18] + "';   OnClick='ExportToExcel(this); return false;' style='border-width:0px;' >");
        },
        //"fnRowCallback": function (nRow, aData, iDisplayIndex) {
        //    $("td:first", nRow).html(iDisplayIndex + 1);
        //    return nRow;
        //},
        "aoColumns": [

                                      { "sTitle": "#" },
                                      { "sTitle": "User Name" },
                                      { "sTitle": "Scope" },
                                      { "sTitle": "User Group Name" },
                                      { "sTitle": "First Name" },
                                      { "sTitle": "Last Name" },
                                      { "sTitle": "Login Name" },
                                      { "sTitle": "User Type Name" },
                                      { "sTitle": "Department Name" },
                                      { "sTitle": "Location Name" },
                                      { "sTitle": "Email Id" },
                                      { "sTitle": "Phone No." },
                                      { "sTitle": "Ext No." },
                                      { "sTitle": "Remark" },
                                      { "sTitle": "ModifyOn" },
                                      { "sTitle": "Edit" },
                                      { "sTitle": "Audit trail" },
                                      { "sTitle": "Export to excel" },



        ],

        "columns": [
                null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null
        ],
        "oLanguage": {
            "sEmptyTable": "No Record Found"
        },
    });

    $('#Activityrecord').show();



}

function Inactiveuser() {
    $('#loadingmessage').show();

    $('#ctl00_CPHLAMBDA_lblexceltitle').text("Inactive User");
    $('#ctl00_CPHLAMBDA_hdnActiveData').val("Inactive-User");

    //$('#lblexceltitle').empty();
    //$('#lblexceltitle').append("inactive user");

    debugger;
    $.ajax({
        type: "post",

        url: "frmUserMaster.aspx/View_InactiveUserMst",
        //data: '{"vWorkSpaceId":"' + vWorkSpaceId + '","vType":"' + vType + '"}',
        contentType: "application/json; charset=utf-8",
        datatype: JSON,
        async: true,
        dataType: "json",
        success: function (data) {
            var data = data.d;
            var msgs = JSON.parse(data);
            summarydata = msgs;
            if (summarydata == "") return false;
            // Table = Object(keys(summarydata))[0];
            CreateSummaryTable(summarydata);
            $('#loadingmessage').hide();
            return false;
        },
        failure: function (response) {
            msgalert("failure");
            msgalert(data.d);
        },
        error: function (response) {
            msgalert("error");
        }
    });
    return false;

}

function activeuser() {

    debugger;
   
    $('#ctl00_CPHLAMBDA_lblexceltitle').text("Active User");
    $('#ctl00_CPHLAMBDA_hdnActiveData').val("Active-User");
    $('#loadingmessage').show();

    $.ajax({
        type: "post",
       
        url: "frmUserMaster.aspx/View_activeUserMst",
        //data: '{"vWorkSpaceId":"' + vWorkSpaceId + '","vType":"' + vType + '"}',
        contentType: "application/json; charset=utf-8",
        datatype: JSON,
        async: false,
        dataType: "json",
        success: function (data) {
           
            $('#loadingmessage').show();
            var data = data.d;
            var msgs = JSON.parse(data);
            summarydata = msgs;
            if (summarydata == "") return false;
            // Table = Object(keys(summarydata))[0];
            CreateSummaryTable(summarydata);
            $('#loadingmessage').hide();
            return false;
        },
        failure: function (response) {
            msgalert("failure");
            msgalert(data.d);
        },
        error: function (response) {
            msgalert("error");
        }
    });
    return false;

}
      
function Editcellrow(e) {

    var id = e.attributes.tempval.value;

    $('#ctl00_CPHLAMBDA_hdnEditedId').val(id);


    $('#ctl00_CPHLAMBDA_btnEdit').click();

}

function AudtiTrail(e) {

    var id = e.attributes.tempval1.value;



    $.ajax({
        type: "post",
        url: "frmUserMaster.aspx/AuditTrail",
        data: "{'id':'" + id + "'}",
        contentType: "application/json; charset=utf-8",
        datatype: JSON,
        async: false,

        success: function (data) {
            $('#tblUserMstAudit').attr("IsTable", "has");
            var aaDataSet = [];
            var range = null;

            if (data.d != "" && data.d != null) {
                data = JSON.parse(data.d);
                for (var Row = 0; Row < data.length; Row++) {
                    var InDataSet = [];
                    InDataSet.push(data[Row].SrNo, data[Row].UserName, data[Row].UserGroupName, data[Row].FirstName, data[Row].LastName, data[Row].LoginName, data[Row].UserTypeName, data[Row].DeptName, data[Row].LocationName, data[Row].Emailid, data[Row].Remarks, data[Row].ModifyBy, data[Row].ModifyOn);
                    aaDataSet.push(InDataSet);
                }

            }
            if ($("#tblUserMstAudit").children().length > 0) {
                $("#tblUserMstAudit").dataTable().fnDestroy();
            }

            oTable = $('#tblUserMstAudit').dataTable({
                "fixedHeader": true,
                "bJQueryUI": true,
                //"sScrollY": "285px",
                //"sScrollX": "90%",
                "bScrollCollapse": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": false,
                "aaData": aaDataSet,
               
                "aLengthMenu": [[3, 5, 10, -1], [3, 5, 10, "All"]],
                "aoColumns": [

                                { "sTitle": "Sr. No", },
                             { "sTitle": "UserName" },
                                { "sTitle": "UserGroupName" },
                                { "sTitle": "FirstName" },
                                { "sTitle": "LastName" },
                              { "sTitle": "LoginName" },
                             { "sTitle": "UserTypeName" },
                               { "sTitle": "DeptName" },
                             { "sTitle": "LocationName" },
                              { "sTitle": "EmailId" },
                            { "sTitle": "Remarks" },
                            { "sTitle": "Modify By" },
                            { "sTitle": "ModifyOn" },
                ],
                //"aocolumndefs": [
                //            { 'bsortable': false, 'atargets': [0] }
              //  ],
                "oLanguage": {
                    "sEmptyTable": "No Record Found",
                }

            });
            oTable.fnAdjustColumnSizing();
            $('.DataTables_sort_wrapper').click;
            $find('MPE_CleintMstHistory').show();
          //  $('.dataTables_filter input').addClass('textBox');
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

function ExportToExcel(e) {


    var id = e.attributes.tempval2.value;
    $("#<%= hdnUserName.ClientID%>").val(id);


    var btn = document.getElementById('<%= btnExportToExcel.ClientId()%>')
    btn.click();
}



    </script>

</asp:content>
