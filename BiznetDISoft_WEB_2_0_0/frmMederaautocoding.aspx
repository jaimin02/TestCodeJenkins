<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmMederaautocoding.aspx.vb" Inherits="frmMederaautocoding" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <link rel="stylesheet" type="text/css" href="App_Themes/jqueryui.css" />
    <link rel="stylesheet" href="App_Themes/CDMS.css" />

    <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

        .dataTables_scroll {
            width: 100%;
            overflow: auto;
            display: block;
            margin: 0px auto;
        }

        ul {
            list-style: none;
        }

        #ctl00_CPHLAMBDA_tvSubject ul {
            padding-left: 0Px !important;
        }

        #ctl00_CPHLAMBDA_tvActivity ul {
            padding-left: 0Px !important;
        }

        .FieldSetBox {
            border: #aaaaaa 1px solid;
            z-index: 0px;
            border-radius: 4px;
        }
    </style>

    <script type="text/javascript" src="script/autocomplete.js"></script>
    <script type="text/javascript" src="Script/scrollablegrid.js"></script>
    <script type="text/javascript" language="javascript"></script>
    <asp:UpdatePanel ID="pnlSubjectWiseEntry" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <table width="100%" cellpadding="5">
                <tr>
                    <td>
                        <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                            <legend class="LegendText" style="color: Black; font-size: 12px">
                                <img id="imgMedCodeDetail" alt="Medical Coding Details" src="images/panelcollapse.png"
                                    onclick="Display(this,'divMedCodeDetail');" runat="server" style="margin-right: 2px;" />Medical Coding Details</legend>
                            <div id="divMedCodeDetail">
                                <table width="98%" cellpadding="5px">
                                    <tr>
                                        <td colspan="4" nowrap="nowrap" class="CodingType">
                                            <asp:RadioButtonList ID="rbtCoding" runat="server" RepeatDirection="Horizontal" Style="margin: auto;">
                                                <asp:ListItem Selected="True" Value="General" onClick="ResetAll();">General Coding</asp:ListItem>
                                                <asp:ListItem Value="SiteWise" onClick="ResetAll();">Coding Site Wise</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Label" width="25%" style="text-align: right;">Project Name* :
                                        </td>
                                        <td colspan="3" style="text-align: left;">
                                            <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" MaxLength="50" Width="79.8%"></asp:TextBox>
                                            <asp:Button ID="btnSetProject" runat="server" Style="display: none" OnClientClick="return getData();" />
                                            <asp:HiddenField ID="HProjectId" runat="server" />
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                                                TargetControlID="txtProject" ServicePath="AutoComplete.asmx" OnClientShowing="ClientPopulated"
                                                OnClientItemSelected="OnSelected" MinimumPrefixLength="1" ServiceMethod="GetMyProjectCompletionList"
                                                CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1">
                                            </cc1:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                    <tr id="trForSiteWise" style="display: none">
                                        <td class="Label" style="text-align: right; width: 25%;">Period :
                                        </td>
                                        <td style="text-align: left; width: 25%;">
                                            <asp:DropDownList ID="ddlPeriods" CssClass="dropDownList" runat="server" AutoPostBack="True"
                                                Style="width: 100%" TabIndex="2">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="Label" style="text-align: right; width: 10%;">Activity Type*:
                                        </td>
                                        <td style="text-align: left; width: 40%;">
                                            <asp:DropDownList ID="ddlActType" runat="server" AutoPostBack="true" CssClass="dropDownList" Width="62%" TabIndex="3">
                                                <asp:ListItem Value="0">Please Select Activity Type</asp:ListItem>
                                                <asp:ListItem Value="1">Subject Specific Activity</asp:ListItem>
                                                <asp:ListItem Value="2">Generic Activity</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trDictionary" runat="server">
                                        <td id="lblCodeStatus" runat="server" class="Label" style="text-align: right; width: 25%;">Coding Status :
                                        </td>
                                        <td id="tdCodeStatus" runat="server" style="text-align: left; width: 25%;">
                                            <asp:DropDownList ID="ddlCodingStatus" runat="server" AutoPostBack="true" CssClass="dropDownList" Width="100%" TabIndex="3">
                                                <asp:ListItem Value="0">All</asp:ListItem>
                                                <asp:ListItem Value="C">Coded Term</asp:ListItem>
                                                <asp:ListItem Value="U">UnCoded Term</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>

                                        <td class="Label" style="text-align: right; width: 10%;">Dictionary :
                                        </td>
                                        <td style="text-align: left; width: 40%;">
                                            <asp:DropDownList ID="ddlDictionary" runat="Server" CssClass="dropDownList" AutoPostBack="true"
                                                Width="62%" OnSelectedIndexChanged="ddlDictionary_SelectedIndexChanged" />
                                        </td>
                                    </tr>

                                    <tr id="trCRFTerm">
                                        <td class="Label" style="text-align: right; width: 25%;">CRF Term :
                                        </td>
                                        <td style="text-align: left; width: 25%;">
                                            <asp:DropDownList ID="MedExDropDown" runat="server" CssClass="dropDownList" Width="100%"
                                                OnSelectedIndexChanged="MedExDropDown_SelectedIndexChanged" AutoPostBack="True" />
                                            <asp:HiddenField runat="server" ID="hfMedexCode" />
                                        </td>
                                        <td class="Label" align="right" id="tdActStatus" style="width: 10%; display: none;">Activity Status :
                                        </td>
                                        <td align="left" id="tdddlStatus" style="width: 40%; display: none;">
                                            <asp:DropDownList ID="ddlActStatus" AutoPostBack="true" runat="server" Style="vertical-align: top; width: 62%"
                                                TabIndex="6" CssClass="dropDownList">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="tdHRUpper" runat="server" colspan="4" style="display: none">
                                            <hr style="width: 80%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <table style="width: 81%; margin: 0px auto;">
                                                <tr>
                                                    <td valign="top" style="align: right;">
                                                        <fieldset id="fsetSubject" runat="server" class="FieldSetBox" style="overflow: auto; width: 100%; display: none; max-height: 230px;"
                                                            tabindex="5">
                                                            <asp:TreeView ID="tvSubject" Width="10px" Height="15px" ShowCheckBoxes="All" BorderColor="DarkGreen"
                                                                Font-Bold="True" Font-Size="X-Small" runat="server">
                                                            </asp:TreeView>
                                                            <asp:HiddenField ID="SubjectCount" runat="server" />
                                                        </fieldset>
                                                    </td>
                                                    <td valign="top">
                                                        <fieldset id="fsetActivity" runat="server" class="FieldSetBox" style="margin-left: 5%; overflow: auto; display: none; max-height: 230px;"
                                                            tabindex="6">
                                                            <asp:TreeView ID="tvActivity" runat="server" BorderColor="DarkGreen" Font-Bold="True"
                                                                Font-Size="X-Small" Height="250px" ShowCheckBoxes="All" Width="100px">
                                                            </asp:TreeView>
                                                            <asp:HiddenField ID="ActivityCount" runat="server" />
                                                        </fieldset>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="tdHRLower" runat="server" colspan="4" style="display: none">
                                            <hr style="width: 80%" />
                                        </td>
                                    </tr>

                                    <tr id="trButton" style="display: none;">
                                        <td class="Label" nowrap="nowrap" colspan="4" style="text-align: center; vertical-align: top;">
                                            <asp:Button ID="btnGo" runat="server" OnClientClick="return Validation();" CssClass="btn btngo"
                                                Text="" ToolTip="Go" Style="font-size: 12px !important; width: 56px !important;" TabIndex="7" />
                                            <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="btn btncancel"
                                                OnClick="btnCancel_Click" Text="Cancel" TabIndex="4" ToolTip="Cancel" />
                                            <asp:Button ID="btnclose" runat="server" CausesValidation="False" CssClass="btn btnclose"
                                                Text="Exit" OnClientClick="return msgconfirmalert('Are You Sure You Want To Exit?',this);"
                                                TabIndex="5" ToolTip="Exit" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </td>
                </tr>
            </table>

            <table width="100%" align="center" style="margin-top: 2%;" id="tblCRFGrid">
                <tr>
                    <td>
                        <fieldset id="fsetCodingData" runat="server" class="FieldSetBox" style="display: none; width: 95.5%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                            <legend class="LegendText" style="color: Black; font-size: 12px">
                                <img id="imgCodingData" alt="Medical Coding Data" src="images/panelcollapse.png"
                                    onclick="Display(this,'divCodingData');" runat="server" style="margin-right: 2px;" />Medical Coding Data</legend>
                            <div id="divCodingData">
                                <table style="margin: auto; width: 90%;">
                                    <tr>
                                        <td style="text-align: center;">
                                            <asp:Button ID="btnBrowse" ToolTip="Perform Coding" Text="Perform Coding" runat="server"
                                                CssClass="btn btnnew" Style="display: none;" />
                                            <asp:Button ID="btnExport" runat="server" CssClass="btn btnexcel"
                                                ToolTip="Export To Excel" Visible="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 1200px; display: block;">
                                            <asp:GridView ID="gvwCRF" runat="server" AutoGenerateColumns="False" Style="margin: 0px auto;" ShowHeaderWhenEmpty="true" ShowHeader="true">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <input id="chkSelectAll" onclick="SelectAll(this, 'gvwCRF')" type="checkbox" />
                                                            <asp:Label ID="Label1" runat="server" Text="Select All"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <%--<input id="ChkMove" type="checkbox"  />--%>
                                                            <asp:CheckBox ID="ChkMove" runat="server" onclick="Eachcheckbox('gvwCRF')" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="vProjectNo" HeaderText="Project No">
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vSubjectId" HeaderText="Screening No">
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vMySubjectNo" HeaderText="Subject ID">
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vRandomizationNo" HeaderText="Randomization No">
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="RepetitionNo" HeaderText="Repeat No">
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CRFTerm" HeaderText="CRF Term">
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vMedExResult" HeaderText="Dictionary Code Term">
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="true" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vRefTableRemark" HeaderText="Dictionary">
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vModifyBy" HeaderText="Modify By">
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="dModifyOn" HeaderText="Modify On">
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="nCRFSubDtlNo" HeaderText="CrfSubDtl NO">
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vProjectTypeCOde" HeaderText="Project Type Code">
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
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

            <div id="autocode" style="width: 100%; display: none;">
                <table width="100%" cellpadding="5">
                    <tr>
                        <td class="Label" style="text-align: left" colspan="2">
                            <asp:TextBox ID="txtcode" runat="server" CssClass="textBox" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: center;" class="Label">
                            <asp:Button ID="btnSave" runat="server" CssClass="btn btnsave" Text="Auto Code" ToolTip="Auto code"
                                OnClientClick=" return Validation();" />
                        </td>
                    </tr>
                </table>
            </div>

            <div id="DivExportToExcel" runat="server">
                <asp:GridView runat="server" ID="gvExportToExcel" AutoGenerateColumns="true" Style="display: none"
                    HeaderStyle-BackColor="#1560a1" HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" Header-style-font-size="10px !importnat" HeaderStyle-Font-Names="Verdana, Arial, Helvetica, sans-serif"
                    HeaderStyle-Font-Size=" 0.9em"
                    HeaderStyle-HorizontalAlign="Center" CellPadding="3" CellSpacing="0"
                    RowStyle-Font-Names=" Verdana, Arial, Helvetica, sans-serif" RowStyle-Font-Size="10pt"
                    RowStyle-BackColor="#84c8e6" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#191970" RowStyle-ForeColor="#191970">
                </asp:GridView>
            </div>

            <asp:HiddenField ID="hndLockStatus" runat="server" />
        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>

    </asp:UpdatePanel>

    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>

    <script type="text/javascript" language="javascript">

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

        function ChangeColor() {
        }

        $("[id*=tvSubject] input[type=checkbox]").live("click", function () {
            var table = $(this).closest("table");
            var Flag = false;
            var Index = 0;
            var IndexNot = 0;
            if (table.next().length > 0 && table.next()[0].tagName == "DIV") {
                var childDiv = table.next();
                var isChecked = $(this).is(":checked");
                $("input[type=checkbox]", childDiv).each(function () {
                    if (isChecked) {
                        $(this).attr("checked", "checked");
                    } else {
                        $(this).removeAttr("checked");
                    }
                });
            } else {
                var parentDIV = $(this).closest("DIV");
                if ($("input[type=checkbox]", parentDIV).length == $("input[type=checkbox]:checked", parentDIV).length) {
                    $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");
                }
                else {
                    $("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
                }
            }
        });

        $("[id*=tvActivity] input[type=checkbox]").live("click", function () {
            var table = $(this).closest("table");
            var Flag = false;
            var Index = 0;
            var IndexNot = 0;
            var IndexAll = 0;
            if (table.next().length > 0 && table.next()[0].tagName == "DIV") {
                var childDiv = table.next();
                var isChecked = $(this).is(":checked");
                $("input[type=checkbox]", childDiv).each(function () {
                    if (isChecked) {
                        $(this).attr("checked", "checked");
                    } else {
                        $(this).removeAttr("checked");
                    }
                });
            } else {
                var parentDIV = $(this).closest("DIV");
                $(this).closest("DIV").find("table [type=checkbox]").each(function () {
                    if ($(this).attr("checked") == true) {
                        Flag = true;
                        Index = Index + 1;
                    }
                    if ($(this).attr("checked") == false) {
                        IndexNot = IndexNot + 1;
                    }
                });
                if (Flag == true) {
                    parentDIV.prev().find("[type=checkbox]").attr("checked", "checked");
                }
                if ($(this).closest("DIV").find("table [type=checkbox]").length == Index) {
                    parentDIV.prev().find("[type=checkbox]").removeAttr("checked");
                    parentDIV.prev().find("[type=checkbox]").attr("checked", "checked");

                }
                if ($(this).closest("DIV").find("table [type=checkbox]").length == IndexNot) {
                    parentDIV.prev().find("[type=checkbox]").removeAttr("checked");
                    parentDIV.prev().find("[type=checkbox]").removeAttr("checked");
                }
                $("#ctl00_CPHLAMBDA_tvActivity").find("table [type=checkbox]").each(function () {
                    if ($(this).find("table [type=checkbox]").attr("checked") == true) {
                        IndexAll = IndexAll + 1;
                    }
                });
                if ($("#ctl00_CPHLAMBDA_tvActivity").find("table [type=checkbox]").length == IndexAll) {
                    $("#ctl00_CPHLAMBDA_tvActivity").find("table [type=checkbox]").first().attr("checked", "checked");
                } else {
                    $("#ctl00_CPHLAMBDA_tvActivity").find("table [type=checkbox]").first().removeAttr("checked")
                }
            }
        });

        function pageLoad() {
            Reset();
        }

        function Reset() {
            if (document.getElementById("ctl00_CPHLAMBDA_rbtCoding_0").checked == true) {
                document.getElementById("trForSiteWise").style.display = 'none';
                document.getElementById("tdActStatus").style.display = 'none';
                document.getElementById("tdddlStatus").style.display = 'none';
                document.getElementById("trButton").style.display = 'none';
                $("#tblCRFGrid").show();
                $("#ctl00_CPHLAMBDA_tdHRUpper").hide();
                $("#ctl00_CPHLAMBDA_tdHRLower").hide();
                $("#ctl00_CPHLAMBDA_fsetSubject").hide();
                $("#ctl00_CPHLAMBDA_fsetActivity").hide();
            }
            else {
                document.getElementById("tdActStatus").removeAttribute("style");
                document.getElementById("tdddlStatus").removeAttribute("style");
                document.getElementById("trForSiteWise").removeAttribute("style");
                document.getElementById("trButton").removeAttribute("style");
            }
        }

        function ResetAll() {
            if (document.getElementById("ctl00_CPHLAMBDA_rbtCoding_0").checked == true) {
                document.getElementById("trForSiteWise").style.display = 'none';
                document.getElementById("tdActStatus").style.display = 'none';
                document.getElementById("tdddlStatus").style.display = 'none';
                document.getElementById("trButton").style.display = 'none';
                $('#ctl00_CPHLAMBDA_txtproject').val('');
                //$("#ctl00_CPHLAMBDA_trDictionary").hide();
                //$("#ctl00_CPHLAMBDA_lblCodeStatus").hide();
                //$("#ctl00_CPHLAMBDA_tdCodeStatus").hide();
                $("#tblCRFGrid").hide();
                $("#ctl00_CPHLAMBDA_tdHRUpper").hide();
                $("#ctl00_CPHLAMBDA_tdHRLower").hide();
                $("#ctl00_CPHLAMBDA_fsetSubject").hide();
                $("#ctl00_CPHLAMBDA_fsetActivity").hide();
                document.getElementById("ctl00_CPHLAMBDA_fsetCodingData").style.display = 'none';
            }
            else {
                document.getElementById("tdActStatus").removeAttribute("style");
                document.getElementById("tdddlStatus").removeAttribute("style");
                document.getElementById("trForSiteWise").removeAttribute("style");
                document.getElementById("trButton").removeAttribute("style");
                $('#ctl00_CPHLAMBDA_txtproject').val('');
                //$("#ctl00_CPHLAMBDA_tdCodeStatus").hide();
                //$("#ctl00_CPHLAMBDA_lblCodeStatus").hide();
                $("select[id$=ddlCodingStatus] > option").remove();
                $("select[id$=ddlDictionary] > option").remove();
                $("select[id$=MedExDropDown] > option").remove();
                $("select[id$=ddlPeriods] > option").remove();
                $("select[id$=ddlActStatus] > option").remove();
                //$("#ctl00_CPHLAMBDA_trDictionary").hide();
                $("#tblCRFGrid").hide();
                $("#ctl00_CPHLAMBDA_tdHRUpper").hide();
                $("#ctl00_CPHLAMBDA_tdHRLower").hide();
                $("#ctl00_CPHLAMBDA_fsetSubject").hide();
                $("#ctl00_CPHLAMBDA_fsetActivity").hide();
                //document.getElementById("ctl00_CPHLAMBDA_ddlActStatus").style.width = '100%';
                document.getElementById("ctl00_CPHLAMBDA_fsetCodingData").style.display = 'none';
            }
        }

        function ClientPopulated(sender, e) {

            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
         $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        function SelectAll(CheckBoxControl, Grid) {
            var str = "";
            document.getElementById("ctl00_CPHLAMBDA_btnBrowse").style.display = "";
            var Gvd = document.getElementById('<%=gvwCRF.ClientId %>');
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

                // document.getElementById('autocode').style.display = '';                   
            }
            else {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf(Grid) > -1)) {
                        document.forms[0].elements[i].checked = false;
                    }
                }
                document.getElementById('ctl00_CPHLAMBDA_btnBrowse').style.display = 'none';
                // document.getElementById('autocode').style.display = 'none';
            }
        }

        function Validation() {
            var dataSubject = parseInt($("#<%=tvSubject.ClientID %> .AspNet-TreeView-Leaf").length) + 1;
            var dataActivity = parseInt($("#<%=tvActivity.ClientID %> .AspNet-TreeView-Leaf").length) + parseInt($("#<%=tvActivity.ClientID %> .AspNet-TreeView-Parent").length) + 1;

            if (document.getElementById('<%=txtproject.ClientID%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=txtproject.clientid %>').value == '';
                msgalert('Please Select Project !');
                document.getElementById('<%=txtproject.ClientID%>').focus();
                return false;
            }
            //else if (document.getElementById('<%=MedExDropDown.clientid %>').selectedIndex == 0) {
            //    msgalert('Please Select CRF Term !');
            //    return false;
            //}

            if (document.getElementById("ctl00_CPHLAMBDA_rbtCoding_0").checked == false) {
                if (document.getElementById('<%=ddlActType.ClientID%>').selectedIndex == 0) {
                    msgalert('Please Select Activity Type !');
                    return false;
                }
                //else if (document.getElementById('<%=ddlActStatus.ClientID%>').selectedIndex == 0) {
                //    msgalert('Please Select Activity Status !');
                //    return false;
                //}
                else if ($("#ctl00_CPHLAMBDA_ddlActType [selected=selected]").val().trim() == "1") {
                    if ($("#ctl00_CPHLAMBDA_tvSubject [type=checkbox]:checked").length == 0) {
                        msgalert("Please select Subject");
                        return false;
                    }
                }

                if ($("#ctl00_CPHLAMBDA_tvActivity [type=checkbox]:checked").length == 0) {
                    msgalert("Please select Activity");
                    return false;
                }
            }

            
        }

        function MeddraBrowser(medexcode, medexdtlno) {

            var crfterm = $("#ctl00_CPHLAMBDA_MedExDropDown option:selected").text();
            window.open('frmCTMMeddraBrowse.aspx?MedExCode=' + medexcode + '&MedExWorkSpaceDtlNo=' + medexdtlno + '&CRFTERM=' + crfterm, '_blank', 'fullscreen=yes,scrollbars=yes');
            return false;
        }

        function SetMeddra(objLLT) {

            document.getElementById('ctl00_CPHLAMBDA_btnBrowse').style.display = '';
            //document.getElementById('autocode').style.display = '';    
            document.getElementById('<%=txtcode.clientid %>').value = objLLT.Meddra;
            var btn = document.getElementById('<%= btnSave.ClientId%>');
            btn.click();
            //document.getElementById('<%=txtcode.ClientID%>').focus();
        }

        function Eachcheckbox(Grid) {


            var Checkall = document.getElementById('chkSelectAll');
            var Gvd = document.getElementById('<%=gvwCRF.ClientId %>');
            var flag = false;
            var sys = false;
            j = 0;
            for (i = 0; i < document.forms[0].elements.length; i++) {
                if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf(Grid) > -1)) {
                    j = j + 1;
                    if (document.forms[0].elements[i].checked == false) {
                        Checkall.checked = false;
                        var sys = true;
                    }
                    if (document.forms[0].elements[i].checked == true) {
                        var flag = true;
                        if (j == Gvd.rows.length - 2) {
                            Checkall.checked = true;
                        }
                    }
                }
            }
            if (flag == true) {
                document.getElementById('ctl00_CPHLAMBDA_btnBrowse').style.display = '';
                //document.getElementById('autocode').style.display = '';    
            }
            else {
                document.getElementById('ctl00_CPHLAMBDA_btnBrowse').style.display = 'none';
                // document.getElementById('autocode').style.display = 'none'; 
            }
            if (sys == true) {
                Checkall.checked = false;
            }
        }

        //Add by shivani pandya for project lock
        function getData(e) {
            var WorkspaceID = $('input[id$=HProjectId]').val();
            $.ajax({
                type: "post",
                url: "frmMederaautocoding.aspx/LockImpact",
                data: '{"WorkspaceID":"' + WorkspaceID + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                dataType: "json",
                success: function (data) {
                    if (data.d == "L") {
                        // alert("Project is locked !");
                        // $("#<%=hndLockStatus.ClientID%>").val("Lock");
                        // $("#ctl00_CPHLAMBDA_gvwCRF [type=checkbox]").attr("Disabled", "Disabled")
                        // return true;

                        msgCallbackAlert(null, "Project is locked ?", function (isConfirmed) {
                            //alert(isConfirmed);
                            if (isConfirmed) {
                                $("#<%=hndLockStatus.ClientID%>").val("Lock");
                                $("#ctl00_CPHLAMBDA_gvwCRF [type=checkbox]").attr("Disabled", "Disabled")
                                return true;
                            } else {
                                return false;
                            }
                        });
                        return false;
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
        oTab = "";
        function UIgvwCRF() {
            document.getElementById("ctl00_CPHLAMBDA_fsetCodingData").style.display = 'block';
            oTab = $('#<%= gvwCRF.ClientID%>').prepend($('<thead>').append($('#<%= gvwCRF.ClientID%> tr:first'))).dataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": false,
                "sScrollY": "250px",
                "sScrollX": "100%",
                "bScrollCollapse": true,
                aLengthMenu: [
                    [10, 25, 50, 100, -1],
                    [10, 25, 50, 100, "All"]
                ],
                "oLanguage": {
                    "sEmptyTable": "No Record Found",
                },
            });
            setTimeout(function () {
                oTab.fnAdjustColumnSizing();
            }, 100);
            return false;
        }
    </script>

</asp:Content>
