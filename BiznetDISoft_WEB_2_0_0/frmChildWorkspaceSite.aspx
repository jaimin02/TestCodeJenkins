<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmChildWorkspaceSite.aspx.vb" Inherits="frmChildWorkspaceSite" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/popcalendar.js"></script>

    <script type="text/javascript" src="Script/Validation.js"></script>

    <script type="text/javascript" src="script/AutoComplete.js"></script>

    <asp:UpdatePanel ID="Up_ChildProtocol" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table style="width: 90%; margin: auto; margin-bottom: 0.5%;" cellpadding="1" cellspacing="0">
                <tr>
                    <td style="text-align: center; width: 100%" colspan="2">
                        <asp:RadioButtonList ID="RblListCreateAndEditChildProject" runat="server" RepeatDirection="Horizontal"
                            AutoPostBack="true" Style="margin: auto;">
                            <asp:ListItem Selected="True">Create Child/Site Project </asp:ListItem>
                            <asp:ListItem>Edit Child/Site Project</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%;" class="Label" colspan="2">
                        <asp:Label ID="lblProject" runat="server" Text=""></asp:Label>
                        <asp:TextBox ID="txtProject" TabIndex="2" runat="server" CssClass="textBox" Width="60%"></asp:TextBox>
                        <asp:Button style="display:none" ID="btnSetProject" runat="server" txt="data" />
                        <!-- Add by shivani pandya for project Lock -->
                        <asp:Button Style="display: none" ID="btnData" OnClientClick="getData();" runat="server" Text=" Project" />
                        <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                            CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                            CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                            OnClientShowing="ClientPopulated" ServiceMethod="GetProjectCompletionListWithOutSponser"
                            ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True"
                            CompletionListElementID="pnlProjectList">
                        </cc1:AutoCompleteExtender>
                        <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 300px; overflow: auto; overflow-x: hidden;" />
                    </td>
                </tr>
            </table>
            <table style="width: 90%; margin: auto;" cellpadding="1" cellspacing="0" id="tblDataEntry" runat="server">
                <tr>
                    <td colspan="2" style="width: 100%">
                        <fieldset style="display: none;" id="fldChild" class="FieldSetBox" runat="server">
                            <table style="width: 90%; margin: auto;">
                                <tbody>
                                    <tr>
                                        <td style="text-align: right; width: 20%;" class="Label">Site/Location*:
                                        </td>
                                        <td style="width: 33%; text-align: left">
                                            <asp:DropDownList ID="ddlLocation" TabIndex="2" runat="server" CssClass="dropDownList"
                                                AutoPostBack="True" Width="65%" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align: right;" class="Label ">Child ProjectNo.
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtGetChildVersion" TabIndex="3" runat="server" CssClass="textBox"
                                                onBlur="return CheckDuplicateSiteFun();"></asp:TextBox>
                                            <asp:Label runat="server" ID="LblError" CssClass="ErrorCode" Text="" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;" class="Label">Account Manager*:
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="ddlmanager" TabIndex="4" runat="server" CssClass="dropDownList"
                                                Width="65%">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align: right;" class="Label">Child Request Id:
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtChildReqID" TabIndex="5" runat="server" Width="60%" CssClass="textBox" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;" class="Label">Study Type*:
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="ddlStudytype" TabIndex="4" runat="server" CssClass="dropDownList"
                                                Width="65%">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align: right;" class="Label">Retention Period:
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtRetentionPeriod" TabIndex="5" runat="server" Width="60%" MaxLength="9" onblur="return Numeric(this);"
                                                CssClass="textBox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;" class="Label">FastFed :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:CheckBox ID="chkFastfedYes" onclick="ChkBox('ChkfedYes')" runat="server" Width="48px"
                                                Text="Yes"></asp:CheckBox>
                                            <asp:CheckBox ID="chkFastfedNo" onclick="ChkBox('ChkfedNo')" runat="server" Width="40px"
                                                Text="No"></asp:CheckBox>(Check only if applicable)
                                        </td>
                                        <td style="text-align: right;" class="Label">No. of Subjects :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtnoofprjct" TabIndex="6" MaxLength="4" runat="server" CssClass="textBox" onblur="return Numeric(this);"
                                                Width="20%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;" class="Label">For Test Site :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:CheckBox ID="chkTestSite" runat="server" Text="Yes" />
                                        </td>
                                        <%--<td style="text-align: right;" class="Label"></td>--%>
                                        <%--<td style="text-align: left;"></td>--%>
                                        <td style="text-align: right;" class="Label">BizNet Site No :</td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtBiznetProjectNo" runat="server" CssClass="textBox" onblur="return SiteNo();"></asp:TextBox>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td style="width: 50%;">
                        <fieldset id="fContactDetail" class="FieldSetBox" style="width: 96.5%; display: none;"
                            runat="server">
                            <table style="width: 90%; margin: auto;">
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td colspan="2" align="center" style="text-align: center;" class="Label">Contact Detail
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right; width: 40%;" class="Label ">Name*:
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtContName" TabIndex="7" MaxLength="50" runat="server" CssClass="textBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" class="Label ">Address 1 :
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtContAdds1" TabIndex="8" MaxLength="100" runat="server" CssClass="textBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" class="Label ">Address 2 :
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtContAdds2" TabIndex="9" runat="server" MaxLength="100" CssClass="textBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" class="Label ">Address 3 :
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtContAdds3" TabIndex="10" runat="server" MaxLength="100" CssClass="textBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" class="Label ">Telephone No. :
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtContTelno" TabIndex="11" runat="server" MaxLength="20" CssClass="textBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" class="Label ">Extention No. :
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtContExtNo" TabIndex="12" runat="server" MaxLength="4" CssClass="textBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" class="Label ">Designation :
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtContDesig" TabIndex="13" runat="server" MaxLength="50" CssClass="textBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" class="Label ">Qualification :
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtContQuali" TabIndex="14" runat="server" MaxLength="50" CssClass="textBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: center" colspan="2">
                                                    <asp:Button ID="btnContAdd" TabIndex="15" runat="server" Text="Add" ToolTip="Add"
                                                        CssClass="btn btnnew" OnClientClick="return checkContactName();"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%;">
                                        <asp:UpdatePanel ID="Up_ContactDtl" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                            <ContentTemplate>
                                                <div style="width: 90%; overflow: auto;">
                                                    <asp:GridView ID="GvwContactDtl" runat="server" SkinID="grdViewSmlAutoSize" OnRowCommand="GvwContactDtl_RowCommand"
                                                        OnRowDeleting="GvwContactDtl_RowDeleting" OnRowDataBound="GvwContactDtl_RowDataBound"
                                                        AutoGenerateColumns="False" Style="width: 100%;">
                                                        <Columns>
                                                            <asp:BoundField DataField="vType" HeaderText="Type" />
                                                            <asp:BoundField DataField="vName" HeaderText="Name" />
                                                            <asp:BoundField DataField="vAddress1" HeaderText="Address1" />
                                                            <asp:BoundField DataField="vAddress2" HeaderText="Address2" />
                                                            <asp:BoundField DataField="vAddress3" HeaderText="Address3" />
                                                            <asp:BoundField DataField="vTeleNo" HeaderText="Telephone No." />
                                                            <asp:BoundField DataField="vExtNo" HeaderText="Extention No." />
                                                            <asp:BoundField DataField="vDesignation" HeaderText="Designation" />
                                                            <asp:BoundField DataField="vQualification" HeaderText="Qualification" />
                                                            <asp:TemplateField HeaderText="Delete">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="ImbADelete" runat="server" ToolTip="Delete" ImageUrl="~/Images/i_delete.gif"></asp:ImageButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Bottom" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Edit">
                                                                <ItemTemplate>
                                                                    <%--<asp:LinkButton ID="LnkBtnEditForCD" runat="server" Text="Edit"></asp:LinkButton>--%>
                                                                    <asp:ImageButton ID="LnkBtnEditForCD" runat="server" ToolTip="Edit" ImageUrl="~/images/Edit2.gif" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Bottom" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="nWorkspaceProtocolDetailMatrixNo" HeaderText="MatrixNo"></asp:BoundField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="txtProject" EventName="TextChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                    <td style="width: 50%;">
                        <fieldset id="fInvestigator" class="FieldSetBox" style="width: 96%; display: none;"
                            runat="server">
                            <table style="width: 90%; margin: auto;">
                                <tr>
                                    <td>
                                        <table width="100%;">
                                            <tr>
                                                <td colspan="2" align="center" style="text-align: center;" class="Label">Investigator Detail
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right; width: 40%;" class="Label">Name*:
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtInvsName" TabIndex="16" runat="server" MaxLength="50" CssClass="textBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" class="Label">Address 1 :
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtInvsAdds1" TabIndex="17" runat="server" MaxLength="100" CssClass="textBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" class="Label ">Address 2 :
                                                </td>
                                                <td style="height: 21px; text-align: left">
                                                    <asp:TextBox ID="TxtInvsAdds2" TabIndex="18" runat="server" MaxLength="100" CssClass="textBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" class="Label ">Address 3 :
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtInvsAdds3" TabIndex="19" runat="server" MaxLength="100" CssClass="textBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" class="Label ">Telephone No. :
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtInvstele" TabIndex="20" runat="server" MaxLength="20" CssClass="textBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" class="Label ">Extention No. :
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtInvsExtes" TabIndex="21" runat="server" MaxLength="4" CssClass="textBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" class="Label ">Designation :
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtInvsDesig" TabIndex="22" MaxLength="50" runat="server" CssClass="textBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;" class="Label ">Qualification :
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox ID="TxtInvsqalific" TabIndex="23" runat="server" CssClass="textBox"
                                                        MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: center" colspan="2">
                                                    <asp:Button ID="btnInvesAdd" TabIndex="24" runat="server" Text="Add" ToolTip="Add"
                                                        CssClass="btn btnnew" OnClientClick="return checkInvName();"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center;">
                                        <asp:UpdatePanel ID="Up_InvestDtl" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                            <ContentTemplate>
                                                <div style="width: 90%; overflow: auto;">
                                                    <asp:GridView ID="GvwInvestDtl" runat="server" SkinID="grdViewSmlAutoSize" OnRowCommand="GvwInvestDtl_RowCommand"
                                                        OnRowDeleting="GvwInvestDtl_RowDeleting" OnRowDataBound="GvwInvestDtl_RowDataBound"
                                                        AutoGenerateColumns="False" Style="width: 100%">
                                                        <Columns>
                                                            <asp:BoundField DataField="vType" HeaderText="Type" />
                                                            <asp:BoundField DataField="vName" HeaderText="Name" />
                                                            <asp:BoundField DataField="vAddress1" HeaderText="Address1" />
                                                            <asp:BoundField DataField="vAddress2" HeaderText="Address2" />
                                                            <asp:BoundField DataField="vAddress3" HeaderText="Address3" />
                                                            <asp:BoundField DataField="vTeleNo" HeaderText="Telephone No." />
                                                            <asp:BoundField DataField="vExtNo" HeaderText="Extention No." />
                                                            <asp:BoundField DataField="vDesignation" HeaderText="Designation" />
                                                            <asp:BoundField DataField="vQualification" HeaderText="Qualification" />
                                                            <asp:TemplateField HeaderText="Delete">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="ImbMDelete" runat="server" ToolTip="Delete" ImageUrl="~/Images/i_delete.gif"></asp:ImageButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Bottom" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Edit">
                                                                <ItemTemplate>
                                                                    <%--<asp:LinkButton ID="LnkBtnEditForID" runat="server" Text="Edit"></asp:LinkButton>--%>
                                                                    <asp:ImageButton ID="LnkBtnEditForID" runat="server" ToolTip="Edit" ImageUrl="~/images/Edit2.gif" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Bottom" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="nWorkspaceProtocolDetailMatrixNo" HeaderText="MatrixNo"></asp:BoundField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="txtProject" EventName="TextChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; display: none;" colspan="2" id="tdSave" runat="server">
                        <asp:Button ID="btnSave" TabIndex="25" runat="server" Text="Save" ToolTip="Save"
                            CssClass="btn btnsave" OnClientClick="return Validation(this);"></asp:Button>
                        <asp:Button ID="btnExit" TabIndex="26" runat="server" Text="Exit" ToolTip="Exit"
                            CssClass="btn btnexit"  OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);"></asp:Button>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnExit" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="GvwInvestDtl" EventName="RowCommand" />
            <asp:AsyncPostBackTrigger ControlID="GvwContactDtl" EventName="RowCommand" />
        </Triggers>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">

        var FlagSetProject = "Flase"
      
        $("#ctl00_CPHLAMBDA_RblListCreateAndEditChildProject_0").live("click",function () {
            if ($(this).attr("checked") == true) {
                $("#ctl00_CPHLAMBDA_txtProject").attr("value", "");
                $("#tblDataEntry").attr("style", "Display:none");
            }            
        });

        $("#ctl00_CPHLAMBDA_RblListCreateAndEditChildProject_1").live("click",function () {
            if ($(this).attr("checked") == true) {
                $("#ctl00_CPHLAMBDA_txtProject").attr("value", "");
                $("#tblDataEntry").attr("style", "Display:none");
            }
        });


        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            
            //Change by shivani pandya for project lock

            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),            
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnData.ClientId %>'));

            //Add by shivani pandya for project lock
            if (FlagSetProject == "True") {
                $("#ctl00_CPHLAMBDA_btnSetProject").click();
                FlagSetProject = "Flase";
            }           
        }

        function Validation(e) {

            if (document.getElementById('<%=txtProject.ClientID%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=txtProject.ClientID%>').value = '';
                msgalert('Please Enter Project Name !');
                document.getElementById('<%=txtProject.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=ddlLocation.ClientID%>').disabled == false && document.getElementById('<%=ddlLocation.ClientID%>').selectedIndex == 0) {
                document.getElementById('<%=ddlLocation.ClientID%>').value = '';
                msgalert('Please Select Location !');
                document.getElementById('<%=ddlLocation.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=ddlManager.ClientID%>').disabled == false && document.getElementById('<%=ddlManager.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Manager !');
                document.getElementById('<%=ddlManager.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=ddlStudytype.ClientID%>').disabled == false && document.getElementById('<%=ddlStudytype.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Study Type !');
                document.getElementById('<%=ddlStudytype.ClientID%>').focus();
                return false;
            }
            <%--else if (document.getElementById('<%=ddlLocation.ClientID%>').disabled == false && document.getElementById('<%=ddlLocation.ClientID%>').selectedIndex == 0) {
                //var result = confirm('You Have Not Selected Location, Are You Sure You Want To Create A Test Site?');
                //if (result == false) {
                //    msgalert('Please Select Location !');
                //    document.getElementById('<%=ddlLocation.ClientID%>').focus();
                //    return false;
                //}


                msgConfirmDeleteAlert(null, "You Have Not Selected Location, Are You Sure You Want To Create A Test Site?", function (isConfirmed) {
                    if (isConfirmed)
                    {
                        __doPostBack(e.name, '');
                        return true;
                    } else
                    {
                        
                        document.getElementById('<%=ddlLocation.ClientID%>').focus();
                        alertdooperation('Please Select Location !');
                    }
                });
                return false;
            }--%>
            return true;
        }

         function SiteNo()
        {
            var BizNetSiteNo = document.getElementById('<%=txtBiznetProjectNo.ClientID%>');
            var ChildProjectNo = document.getElementById('<%=txtGetChildVersion.ClientID%>');
            ApprovalSiteNo.value = ChildProjectNo.value + ' (' + BizNetSiteNo.value + ')';
         }

function ChkBox(type) {
    if (type == 'ChkfedYes') {
        document.getElementById('<%=chkFastfedYes.ClientID%>').checked = true;
        document.getElementById('<%=ChkfastfedNo.ClientID%>').checked = false;
        return true;
    }
    else if (type == 'ChkfedNo') {
        document.getElementById('<%=ChkfastfedNo.ClientID%>').checked = true;
        document.getElementById('<%=ChkfastfedYes.ClientID%>').checked = false;
        return true;
    }
}

function Numeric(e) {
    var ValidChars = "0123456789";
    var Numeric = true;
    var Char;

    sText = e.value.trim();
    if (sText.trim() != '') {
        if (sText > 0) {
            for (i = 0; i < sText.length && Numeric == true; i++) {
                Char = sText.charAt(i);
                if (ValidChars.indexOf(Char) == -1) {
                    msgalert('Please Enter Value Greater than Zero !');
                    e.value = '';
                    e.focus();
                    Numeric = false;
                }
            }
        }
        else {
            msgalert('Please Enter Value Greater than Zero !');
            e.value = '';
            e.focus();
            Numeric = false;
        }
    }
    return Numeric;
}

function show_confirm() {
    var r = confirm("Are You Sure You Want To Delete This Record?");
    return r
}
function checkContactName() {
    if (document.getElementById('<%=txtContName.clientId %>').value == "") {
        msgalert("Please Enter Name !")
        return false;
    }
    return true;
}
function checkInvName() {
    if (document.getElementById('<%=txtInvsName.clientId %>').value == "") {
        msgalert("Please Enter Name !")
        return false;
    }
    return true;
}
function CheckDuplicateSiteFun() {
    PageMethods.CheckDuplicateSite
    (document.getElementById('<%= txtGetChildVersion.clientId %>').value,
                document.getElementById('<%= HProjectId.clientId %>').value,
                function (Result) {
                    document.getElementById('<%= LblError.clientId %>').innerHTML = Result;
                    if (Result.length > 0) {
                        document.getElementById('<%= txtGetChildVersion.clientId %>').focus()
                        return false;
                    }
                },
                function (eerror) {
                    msgalert(eerror);
                }
            );
                return true;
}
        //Add by shivani pandya for project lock
        function getData() {          
            var WorkspaceID = $('input[id$=HProjectId]').val();         
            $.ajax({
                type: "post",
                url: "frmchildWorkspaceSite.aspx/LockImpact",
                data: '{"WorkspaceID":"' + WorkspaceID + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                dataType: "json",
                success: function (data) {
                    if (data.d == "L") {
                        msgalert("Project is locked !");
                        $("#ctl00_CPHLAMBDA_txtProject").attr("value", "");                       
                    } else {
                        FlagSetProject = "True";
                        return true;
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
