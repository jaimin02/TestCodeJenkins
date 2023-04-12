<%@ Page Language="VB" MasterPageFile="~/ECTDMasterPage.master" CodeFile="~/frmPreviewAttributeTemplate.aspx.vb"
    AutoEventWireup="false" Inherits="frmPreviewAttributeTemplate" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <link rel="stylesheet" type="text/css" href="App_Themes/smoothnessjquery-ui.css" />
    <link rel="shortcut icon" type="image/x-icon" href="images/biznet.ico" />
    
    <style type="text/css">
        #SeqMedex {
            width: 760px;
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

        #tr_AttributeGroup1 {
            margin-left: 18%;
            font-weight: bold;
        }

        #tr_Attribute1 {
            margin-left: 15%;
            font-weight: bold;
        }

        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }
        #ctl00_CPHLAMBDA_GV_PreviewAtrributeTemplate_wrapper {
            margin: 0px 90px;
        }
        .slimScrollDiv {
            /*OVERFLOW-Y: AUTO !important;*/
            height:250px !important;
        }

    </style>
    <asp:UpdatePanel ID="UpPnlTemplate" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="HTemplateId" runat="server" />
            <asp:HiddenField ID="Hdn_ValidationType" runat="server" />
            <input type="hidden" id="hd_TxtLength" value="0" />
            <asp:HiddenField ID="Hdn_LengthNumeric" runat="server" />
            <asp:HiddenField ID="hdnMedexList" runat="server" />


            <table style="width: 100%; margin-bottom: 2%;" cellpadding="5px">
                <tr>
                    <td>
                        <fieldset class="FieldSetBox" style="display: block; width: 98%; text-align: left; margin:auto; border: #aaaaaa 1px solid;" runat="server">
                            <legend class="LegendText" style="color: Black; font-size: 12px" title="test" >
                                <img id="img2" alt="Search Template" src="images/panelcollapse.png" onclick="Display(this,'divUserType');" runat="server" style="margin-right: 2px;" />Search Template</legend>
                            <div id="divUserType">
                                <table width="90%" style="margin: auto; padding: 1%;">
                                    <tr>
                                        <td style="width: 30%; text-align: right;">Search Template :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtTemplate" TabIndex="1" runat="server" CssClass="textBox" Width="70%" />
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                                CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                                OnClientShowing="ClientPopulated" ServiceMethod="GetTemplateCompletionListPreviewAttribute" ServicePath="AutoComplete.asmx"
                                                TargetControlID="txtTemplate" UseContextKey="True" CompletionListElementID="pnlTemplate">
                                            </cc1:AutoCompleteExtender>
                                            <asp:Panel ID="pnlTemplate" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden;" />
                                            <asp:Button ID="BtnViewAll" runat="server" Text="View All" ToolTip="View All" Visible="false"
                                                CssClass="btn btnnew" />
                                            <asp:Button Style="display: none" ID="btnSetTemplate" OnClick="btnSetTemplate_click"
                                                runat="server" Text="Template" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                            <asp:GridView ID="GV_PreviewAtrributeTemplate" runat="server" OnPageIndexChanging="GV_PreviewAtrributeTemplate_PageIndexChanging"
                                AutoGenerateColumns="False" Style="width:90%; margin: auto; margin-top: 2%;" TabIndex="2">
                                <HeaderStyle Font-Underline="False" BackColor="#1560a1" Font-Names="Verdana" VerticalAlign="Top"
                                    HorizontalAlign="Center" ForeColor="white" Font-Bold="True" />
                                <Columns>
                                    <asp:BoundField HeaderText="#" ItemStyle-HorizontalAlign="Right">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vTemplateName" HeaderText="Attribute Template" />
                                    <asp:BoundField DataField="vMedExTemplateId" HeaderText="Template ID" />
                                    <asp:TemplateField HeaderText="Edit" SortExpression="status">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImbEdit" runat="server" ToolTip="Edit" ImageUrl="~/images/Edit2.gif" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Preview" SortExpression="status">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImbMPreview" runat="server" ToolTip="Preview" ImageUrl="~/Images/view.gif" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="vProjectTypeCode" HeaderText="ProjectTypeCode" />
                                    <asp:TemplateField HeaderText="Order">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImbReorder" ToolTip="Reorder The Attributes" runat="server"
                                                ImageUrl="~/images/sorting.png" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </td>
                </tr>
            </table>

            <table style="width: 100%; margin-bottom: 2%;display:none" cellpadding="5px" class="tblmedex">
                <tr>
                    <td>
                        <fieldset class="FieldSetBox" style="display: block; width: 97.5%; text-align: left; margin:auto; border: #aaaaaa 1px solid;" id="fieldsetTempalte">
                            <legend class="LegendText" style="color: Black; font-size: 12px" id="legendMedExTemplateName">
                                <img id="img1" alt="" src="images/panelcollapse.png" onclick="Display(this,'divProfiles');" runat="server" style="margin-right: 2px;" />
                                <asp:Label ID="lblProject" runat="server"></asp:Label>
                            </legend>
                            <div id="divProfiles">
                                <table style="width: 90%; margin: auto; padding: 1%;">
                                    <tbody>
                                        <tr>
                                            <td style="text-align: left;">
                                                <span id="tr_AttributeGroup1">Attribute Group :
                                                    <asp:DropDownList ID="ddlMedexGroup" TabIndex="3" runat="server" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddlMedexGroup_SelectedIndexChanged" Width="15%" />
                                                </span><span id="tr_Attribute1">Attribute :
                                                    <asp:DropDownList ID="ddlMedex" TabIndex="4" runat="server" Width="15%" />
                                                </span><span>
                                                    <asp:Button ID="btnAddMedEx" TabIndex="5" runat="server" Text="Add" ToolTip="Add Selected Attributes To Grid"
                                                        CssClass="btn btnnew" Font-Size="8pt" OnClientClick="return ValidationToAdd();" />
                                                    <asp:Button ID="btndeleteMedex" TabIndex="6" runat="server" Font-Size="8pt" Text="Delete"
                                                        ToolTip="Remove Selected Attributes From Grid" CssClass="btn btncancel" />
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center;">
                                                <asp:Button ID="btnSaveMedEx" TabIndex="7" Font-Size="8pt" runat="server" Text="Save"
                                                    ToolTip="Save" CssClass="btn btnsave" />
                                                <asp:Button ID="btnCancelMedEx" TabIndex="8" runat="server" Text="Cancel" Font-Size="8pt"
                                                    ToolTip="Cancel" CssClass="btn btncancel" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlMedExGrid" runat="server" Width="100%">
                                                    <asp:GridView ID="gvwMedEx" TabIndex="9" runat="server" SkinID="grdViewSmlAutoSize"
                                                        PageSize="5" AutoGenerateColumns="False" Width="100%">
                                                        <HeaderStyle Font-Underline="False" BackColor="#1560a1" Font-Names="Verdana" VerticalAlign="Top"
                                                            HorizontalAlign="Center" ForeColor="white" Font-Bold="True" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderStyle-Width="3%" ItemStyle-Width="3%" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="ChkMove" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Details" HeaderStyle-Width="2%" ItemStyle-Width="2%">
                                                                <ItemTemplate>
                                                                    <asp:Image ID="ImbDetails" runat="server" AlternateText="Details" ImageUrl="~/images/attributedetails.png" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="nMedExTemplateDtlNo" HeaderText="MedExTemplateDtlNo" />
                                                            <asp:BoundField DataField="vMedExTemplateId" HeaderText="MedExWorkSpaceHdrNo" />
                                                            <asp:BoundField DataField="vMedExCode" HeaderText="Medex Id" />
                                                            <asp:TemplateField HeaderText="Attribute Description" HeaderStyle-Width="15%" ItemStyle-Width="15%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox TextMode="MultiLine" Width="90%" ID="txtMedexDesc" Text='<%# eval("vMedExDesc") %>'
                                                                        runat="server" CssClass="textBox" cols="9" Rows="3" ToolTip='<%# eval("vMedExDesc") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Attribute Type" HeaderStyle-Width="10%" ItemStyle-Width="18%">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlMedExAttributeType" ToolTip="Select Attribute Type" runat="server" Width="100%"/>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Attribute Value" HeaderStyle-Width="15%" ItemStyle-Width="15%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtValue" runat="server" CssClass="textBox " Text='<%# eval("vMedExValues") %>'
                                                                        ToolTip='<%# eval("vMedExValues") %>' Width="85%"  TextMode="MultiLine" Rows="3" Columns="9" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Default Value" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtDefaultValue" runat="server" CssClass="textBox " Text='<%# eval("vDefaultValue") %>'
                                                                        Width="85%" ToolTip='<%# eval("vDefaultValue") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Alert On" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtAlertOn" runat="server" CssClass="textBox" Text='<%# eval("vAlertonvalue") %>'
                                                                        Width="85%" ToolTip='<%# eval("vAlertonvalue") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Alert Message" HeaderStyle-Width="15%" ItemStyle-Width="15%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtalertMsg" runat="server" Width="90%" CssClass="textBox" Text='<%# eval("vAlertMessage") %>'
                                                                        TextMode="MultiLine" Rows="3" Columns="9" ToolTip='<%# eval("vAlertMessage") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Low Range" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtLowRange" runat="server" CssClass="ValidNum" Text='<%# eval("vLowRange") %>'
                                                                        Width="85%" ToolTip='<%# eval("vLowRange") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="High Range" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtHighRange" runat="server" CssClass="ValidNum" Text='<%# eval("vHighRange") %>'
                                                                        Width="85%" ToolTip='<%# eval("vHighRange") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="cActiveFlag" HeaderText="ActiveFlag" />
                                                            <asp:BoundField DataField="iSeqNo" HeaderText="SeqNo" />
                                                            <asp:BoundField DataField="vMedExGroupCode" HeaderText="MedExGroupCode" />
                                                            <asp:BoundField DataField="vmedexgroupDesc" HeaderText="Group" />
                                                            <asp:BoundField DataField="vMedexGroupCDISCValue" HeaderText="Group CDISC" Visible="false" />
                                                            <asp:BoundField DataField="vmedexGroupOtherValue" HeaderText="Group Other Value"
                                                                Visible="false" />
                                                            <asp:BoundField DataField="vMedExSubGroupCode" HeaderText="MedExSubGroupCode" Visible="false" />
                                                            <asp:BoundField DataField="vmedexsubGroupDesc" HeaderText="SubGroup" />
                                                            <asp:BoundField DataField="vMedexSubGroupCDISCValue" HeaderText="SubGroupCDISC" Visible="false" />
                                                            <asp:BoundField DataField="vmedexsubGroupOtherValue" HeaderText="SubGroupOther" Visible="false" />
                                                            <asp:TemplateField HeaderText="UOM" HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlUOMDesc" ToolTip="Select UOM" runat="server" Width="100%" onChange="return fnAssigntitle(this);" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Validation Type" HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlValidation" runat="server"
                                                                        Width="100%" onchange="OnChangeDropDown(this)" ToolTip="Not Applicable">
                                                                        <asp:ListItem Value="NA">Not Applicable</asp:ListItem>
                                                                        <asp:ListItem Value="AN">Alpha Numeric</asp:ListItem>
                                                                        <asp:ListItem Value="NU">Numeric</asp:ListItem>
                                                                        <asp:ListItem Value="IN">Integer</asp:ListItem>
                                                                        <asp:ListItem Value="AL">Alphabate</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Length" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtLength" runat="server" CssClass="textBox " Width="85%" onkeypress="return ValidateNumericCode(event);" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Not Null" HeaderStyle-Width="2%" ItemStyle-Width="2%"
                                                                ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="ChkAlertType" runat="server" Width="85%" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="cRefType" HeaderText="RefType" />
                                                            <asp:BoundField DataField="vRefTable" HeaderText="RefTable" />
                                                            <asp:BoundField DataField="vRefColumn" HeaderText="RefColumn" />
                                                            <asp:BoundField DataField="vRefFilePath" HeaderText="RefFilePath" />
                                                            <asp:TemplateField HeaderText="CDISC" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtCDISCValues" runat="server" CssClass="textBox " Text='<%# eval("vCDISCValues") %>'
                                                                        ToolTip='<%# eval("vCDISCValues") %>' Width="85%" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Other" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtotherValues" runat="server" CssClass="textBox " Text='<%# eval("vOtherValues") %>'
                                                                        Width="85%" ToolTip='<%# eval("vOtherValues") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <%--<asp:TemplateField HeaderText="Delete">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgDelete" runat="server" align="Center" ImageUrl="~/Images/i_delete.gif"
                                                                        OnClientClick="return confirm('Are You Sure You Want To Delete?')" ToolTip="Delete" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateField>--%>
                                                            <asp:BoundField DataField="vUOM" />
                                                            <asp:BoundField DataField="vValidationType" />
                                                            <asp:BoundField DataField="cAlertType" />
                                                            <asp:BoundField DataField="vMedExType" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </fieldset>
                    </td>
                </tr>
            </table>




            <%--<asp:Panel ID="pheader" runat="server">
                <div id="divExpandable" runat="server" style="font-weight: bold; font-size: 13px; float: none; color: white; background-color: #1560a1; width: 90%; margin: auto;">
                    <div>
                        <asp:Image ID="imgArrows" runat="server" Style="float: right;" />
                    </div>
                    <div style="text-align: left; margin-left: 3%; height:20px;">
                        <asp:Label ID="Label1" runat="server" Text="Search Template">
                        </asp:Label>
                    </div>
                </div>
            </asp:Panel>--%>
            <%-- <asp:Panel ID="pnltable" runat="server">
                <table width="90%" style="margin: auto; padding: 1%; border: 1px solid; border-color: Black;">
                    <tr>
                        <td style="width: 30%; text-align: right;">Search Template :
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="txtTemplate" TabIndex="1" runat="server" CssClass="textBox" Width="70%" />
                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                OnClientShowing="ClientPopulated" ServiceMethod="GetTemplateCompletionList" ServicePath="AutoComplete.asmx"
                                TargetControlID="txtTemplate" UseContextKey="True" CompletionListElementID="pnlTemplate">
                            </cc1:AutoCompleteExtender>
                            <asp:Panel ID="pnlTemplate" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden;" />
                            <asp:Button ID="BtnViewAll" runat="server" Text="View All" ToolTip="View All" Visible="false"
                                CssClass="btn btnnew" />
                            <asp:Button Style="display: none" ID="btnSetTemplate" OnClick="btnSetTemplate_click"
                                runat="server" Text="Template" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="GV_PreviewAtrributeTemplate" runat="server" OnPageIndexChanging="GV_PreviewAtrributeTemplate_PageIndexChanging"
                                AutoGenerateColumns="False" ShowFooter="true" Style="width:90%; margin: auto; margin-top: 2%;" TabIndex="2">
                                <HeaderStyle Font-Underline="False" BackColor="#1560a1" Font-Names="Verdana" VerticalAlign="Top"
                                    HorizontalAlign="Center" ForeColor="white" Font-Bold="True" />
                                <Columns>
                                    <asp:BoundField HeaderText="#" ItemStyle-HorizontalAlign="Right">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vTemplateName" HeaderText="Attribute Template" />
                                    <asp:BoundField DataField="vMedExTemplateId" HeaderText="Template ID" />
                                    <asp:TemplateField HeaderText="Edit" SortExpression="status">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImbEdit" runat="server" ToolTip="Edit" ImageUrl="~/images/Edit2.gif" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Preview" SortExpression="status">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImbMPreview" runat="server" ToolTip="Preview" ImageUrl="~/Images/view.gif" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="vProjectTypeCode" HeaderText="ProjectTypeCode" />
                                    <asp:TemplateField HeaderText="Order">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImbReorder" ToolTip="Reorder The Attributes" runat="server"
                                                ImageUrl="~/images/sorting.png" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </asp:Panel>--%>
            <%--<cc1:CollapsiblePanelExtender ID="Collpase1" runat="server" TargetControlID="pnltable"
                ExpandControlID="pheader" CollapseControlID="pheader" ExpandedImage="images/panelcollapse.png"
                CollapsedImage="images/panelexpand.png" ImageControlID="imgArrows" AutoCollapse="false"
                AutoExpand="false">
            </cc1:CollapsiblePanelExtender>--%>


<%--            <asp:Panel ID="PanelGridHeader" runat="server" Style="margin-top: 2%; display: none;">
                <div id="divGrid" runat="server" style="font-weight: bold; font-size: 13px; float: none; color: white; background-color: #1560a1; width: 90%; margin: auto;">
                    <div>
                        <asp:Image ID="ImgHeader" runat="server" Style="float: right;" />
                    </div>
                    <div style="text-align: left; margin-left: 3%;">
                        <asp:Label ID="lblProject" runat="server">
                        </asp:Label>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlMedEx" runat="server" Style="display: none;">
                
            </asp:Panel>
            <cc1:CollapsiblePanelExtender ID="Collpase2" runat="server" TargetControlID="pnlMedEx"
                ExpandControlID="PanelGridHeader" CollapseControlID="PanelGridHeader" ExpandedImage="images/panelcollapse.png"
                CollapsedImage="images/panelexpand.png" ImageControlID="ImgHeader" AutoCollapse="false"
                AutoExpand="false">
            </cc1:CollapsiblePanelExtender>--%>
            <asp:Button ID="btnSequence" runat="server" Style="display: none;" />
            <cc1:ModalPopupExtender ID="MPEMedexSequence" runat="server" PopupControlID="divSequence"
                BackgroundCssClass="modalBackground" BehaviorID="MPEMedexSequence" TargetControlID="btnSequence">
            </cc1:ModalPopupExtender>
            <div id="divSequence" runat="server" class="centerModalPopup" style="width: 80%; position: absolute; max-height: 500px; display: none;">
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
                <div id="divMedexSequence" style="width: 100%; margin: auto; margin-bottom: 2%;height:300px; overflow:auto !important;">
                    <%--<fieldset class="FieldSetBox" style="max-width: 90%; min-width: 70%; max-height: 560px;
                        margin: auto;">
                        <legend class="LegendText" style="color: Black">Change Sequence</legend>--%>
                    <ul id="SeqMedex" runat="server" style="list-style-type: none !important; padding: 10px; width: 100%;">
                    </ul>
                    <%--</fieldset>--%>
                </div>
                <table width="100%">
                    <tr>
                        <td>
                            <hr />
                        </td>
                    </tr>
                </table>
                <asp:Label ID="lblMedexCount" runat="server" Style="float: left; color: Black; margin-left: 3%;"></asp:Label>
                <input type="button" id="btnMedexseq" value="Save" title="Save The Sequence" style="float: left; margin-left: 25%;"
                    onclick="return savesequence();" class="btn btnsave" />                

                <input type="button" id="btnCloseModal" value="Cancel" title="Cancel" style="float: left;margin-left:1%"
                    onclick="return closesequencediv();" class="btn btncancel"/>

                <asp:Button ID="btnSaveSequence" Text="Save" ToolTip="Save The Sequence" runat="server"
                    Style="margin: auto; display: none;" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
     
    <script type="text/javascript" src="Script/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="Script/jquery-ui-1.10.2.custom.min.js"></script>
    <script type="text/javascript" src="Script/slimScroll.min.js"></script>
    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <script type="text/javascript" src="Script/Validation.js"></script>

    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery-ui_New.js"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    

    <script type="text/javascript">
        function UIGV_PreviewAtrributeTemplate() {
            $('#<%= GV_PreviewAtrributeTemplate.ClientID%>').removeAttr('style', 'display:block');
            oTab = jQuery('#<%= GV_PreviewAtrributeTemplate.ClientID%>').prepend(jQuery('<thead>').append(jQuery('#<%= GV_PreviewAtrributeTemplate.ClientID%> tr:first'))).dataTable({
                "bJQueryUI": true,
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
            return false;
        }

        function OpenAttributeTemplate() {
            $('.tblmedex').css('display', '')
            $('#<%= img2.ClientID%>').click();
        }

        function CloseAttributeTemplate() {
            $('.tblmedex').css('display', 'none')
            $('#<%= img1.ClientID%>').click();
        }
        

        function pageLoad() {
            $('#ctl00_CPHLAMBDA_SeqMedex').sortable()
            $('#ctl00_CPHLAMBDA_SeqMedex').disableSelection();


            //$('#divMedexSequence').mouseover(function () {
            //    $(this).slimScroll({

            //        height: '500px',
            //        width: '100%',
            //        size: '6px',
            //        color: 'blue',
            //        railVisible: true,
            //        railColor: 'gray',
            //        railOpacity: 0.3,
            //        alwaysVisible: false
            //    });
            //});

            $('.ValidNum').ForceNumericOnly();
        }



        function ClientPopulated(sender, e) {
            MedexTemplateClientShowing('AutoCompleteExtender1', $get('<%= txtTemplate.ClientId %>'));
        }

        function OnSelected(sender, e) {
            MedexTemplateOnItemSelected(e.get_value(), $get('<%= txtTemplate.clientid %>'),
                    $get('<%= HTemplateId.clientid %>'), $get('<%=btnSetTemplate.clientid %>'));


        }

        function PreviewTemplate(Path) {
            window.open(Path);
            return false;
        }

        function closesequencediv() {
            $find('MPEMedexSequence').hide();
        }

        function ValidationToAdd() {
            if (document.getElementById('<%=ddlMedExGroup.clientid %>').selectedIndex == 0) {
                msgalert('Select Attribute Group !');
                return false;
            }
            if (document.getElementById('<%=ddlMedEx.clientid %>').selectedIndex == 0) {
                msgalert('Select Attribute !');
                return false;
            }
            return true;
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


        //        function ValidateNumeric(txtBox, msg) {
        //            var result = CheckDecimalOrBlank(txtBox.value);
        //            if (result == false) {
        //                window.alert(msg);
        //                txtBox.value = "";
        //                txtBox.focus();
        //            }
        //        }


        function ValidateNumeric(txtBox, ddlSelected, msg) {
            var strUser = ddlSelected.value;
            // var CommaSepratedNumbers=/[0-9]+(,[0-9]+)*,?/ ;
            var CommaSepratedNumbers = /([1-9][0-9]*,)*[0-9][0-9]*/;
            //            value3=document.getElementById("ctl00_CPHLAMBDA_txtlength").value;
            value3 = txtBox.value;
            var arrayList = value3.match(CommaSepratedNumbers);
            if (strUser == 'NU') {
                if (value3.split(",")[0] < value3.split(",")[1]) {
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

            //                var reg=/^(([0-9a-zA-Z](,)?)*)+$/;
            //var reg=/(?!,)(?:(?:,|^)([-+]?\d+))*$/;

            // var Code=/^(([0-9a-zA-Z](,)?)*)+$/ 
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

                for (var i in result) {
                    if (txtAlert == result[i]) {
                        return true;
                    }
                }
                txtboxAlertOn.focus();
                msgalert('Please Enter Correct Alert On Value !');

                return false;
            }
            return true;
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
        //        {
        //         var e = object;
        //         if (e.options[e.selectedIndex].text=='Numeric')
        //         {
        //        
        //        }
        //        else
        //        {
        //         
        //         }
        //        return false;
        //        }

        // for fix gridview header aded on 22-nov-2011
        //        function pageLoad() {
        //            FreezeTableHeader($('#<%= GV_PreviewAtrributeTemplate.ClientID %>'), { height: 250, width: 900 });
        //        }
        //        


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
                          // allow backspace, tab, delete, arrows, numbers and keypad numbers ONLY
                          // home, end, period, and numpad decimal
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


        //function FillMedExTemplateName(message) {
        //    document.getElementById("legendMedExTemplateName").innerHTML = message;
        //}


    </script>

</asp:Content>
