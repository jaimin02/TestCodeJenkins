<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmAttributeMst, App_Web_5xoe1jh1" validaterequest="false" enableeventvalidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <link rel="stylesheet" type="text/css" href="App_Themes/jquery.multiselect.css" />
    <link rel="stylesheet" type="text/css" href="App_Themes/multiple-select.css" />
    <link rel="stylesheet" type="text/css" href="App_Themes/jquery.multiselect.css" />


    <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

        .dataTable {
            width: 1300px !important;
        }

        #ctl00_CPHLAMBDA_gvmedex_wrapper {
        }


        .ui-multiselect {
            border: 1px solid navy;
            /*width: 50% ;*/
            max-width: 40% !important;
            max-height: 35px;
            overflow: auto;
            overflow-x: hidden;
            white-space: nowrap;
        }

        .ui-multiselect-menu {
            max-width: 40% !important;
        }

        table.dataTable td {
            PADDING: 4px 0px;
        }

        table.dataTable thead th {
            PADDING: 3px 4px 3px 10px;
        }

        #ctl00_CPHLAMBDA_gvmedex_info {
            display:block;
        }

    </style>



    <script type="text/javascript" src="Script/General.js"></script>
    <script type="text/javascript" src="Script/Validation.js"></script>
    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <script type="text/javascript" src="Script/scrollablegrid.js"></script>
    <script src="Script/Validation.js" type="text/javascript"></script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%" cellpadding="3px">
                <tbody>
                    <tr>
                        <td>
                            <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img2" alt="Attribute Details" src="images/panelcollapse.png"
                                        onclick="Display(this,'divAttributeDetail');" runat="server" style="margin-right: 2px;" />Attribute Details</legend>
                                <div id="divAttributeDetail">
                                    <table width="100%" cellpadding="3px">
                                        <tbody>
                                            <tr>
                                                <td class="Label" style="text-align: right; width: 20%;">Attribute Desc* :
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                    <asp:TextBox onblur="setNext();" ID="txtmedexdesc" TabIndex="0" runat="server" CssClass="textBox"
                                                        Width="70%" TextMode="MultiLine" MaxLength="1000"></asp:TextBox>
                                                </td>
                                                <td class="Label" style="width: 18%; text-align: right;">Low Range :
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox ID="txtLowRange" TabIndex="11" runat="server" MaxLength="100" CssClass="textBox"
                                                        Width="55%" onchange="ValidateTextBoxInteger(this,'Please Enter Integer Values Only !');"
                                                        onkeypress="return isKeyPress(this);">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            
                                            <tr>
                                                <td class="Label" style="text-align: right;">Attribute Group Desc* :
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:DropDownList ID="ddlmedexgroup" TabIndex="1" runat="server" Class="dropDownList ddl"
                                                        Width="71%">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="Label" style="text-align: right;">High Range :
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox ID="txtHighRange" Width="55%" MaxLength="100" TabIndex="12" runat="server"
                                                        CssClass="textBox" onchange="ValidateTextBoxInteger(this,'Please Enter Integer Values Only !');"
                                                        onkeypress="return isKeyPress(this);"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Label" style="text-align: right;">Attribute SubGroup Desc* :
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:DropDownList ID="ddlMedExSubGroupDesc" TabIndex="2" runat="server" CssClass="dropDownList"
                                                        Width="71%">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="Label" style="text-align: right;">Length :
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox onchange="ValidateNumeric(this,'Please Enter Numeric Values Only !');"
                                                        ID="txtlength" TabIndex="13" runat="server" CssClass="textBox" Width="55%">0</asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" FilterType="Numbers,Custom"
                                                        ValidChars="," TargetControlID="txtlength" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Label" style="text-align: right;">Attribute Type* :
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:DropDownList ID="ddlMedExType" TabIndex="3" runat="server" CssClass="dropDownList"
                                                        Width="71%" OnSelectedIndexChanged="ddlMedExType_SelectedIndexChanged" AutoPostBack="True">
                                                        <asp:ListItem Value="n" Text="Select Attribute Type"></asp:ListItem>
                                                        <asp:ListItem Value="TextBox" Text="TextBox"></asp:ListItem>
                                                        <asp:ListItem Value="TextArea" Text="TextArea"></asp:ListItem>
                                                        <asp:ListItem Value="ComboBox" Text="ComboBox"></asp:ListItem>
                                                        <asp:ListItem Value="Radio" Text="Radio"></asp:ListItem>
                                                        <asp:ListItem Value="CheckBox" Text="CheckBox"></asp:ListItem>
                                                        <asp:ListItem Value="File" Text="File"></asp:ListItem>
                                                        <asp:ListItem Value="DateTime" Text="DateTime"></asp:ListItem>
                                                        <asp:ListItem Value="Time" Text="Time"></asp:ListItem>
                                                        <asp:ListItem Value="AsyncDateTime" Text="AsyncDateTime"></asp:ListItem>
                                                        <asp:ListItem Value="AsyncTime" Text="AsyncTime"></asp:ListItem>
                                                        <asp:ListItem Value="Import" Text="Import"></asp:ListItem>
                                                        <asp:ListItem Value="ComboGlobalDictionary" Text="ComboGlobalDictionary"></asp:ListItem>
                                                        <asp:ListItem Value="Formula" Text="Formula"></asp:ListItem>
                                                        <asp:ListItem Value="Label" Text="Label"></asp:ListItem>
                                                        <asp:ListItem Value="CrfTerm" Text="CrfTerm"></asp:ListItem>
                                                        <asp:ListItem Value="STANDARDDATE" Text="StandardDate"></asp:ListItem>
                                                        <asp:ListItem Value="StandardDateTime" Text="StandardDateTime"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="Label" style="text-align: right;">Alert On Value :
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox ID="txtAlerton" TabIndex="14" runat="server" CssClass="textBox" Width="55%"
                                                        onblur="ValidateTextBox(this,'Please Enter Value in correct Format!');"></asp:TextBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="Label" style="text-align: right;">Attribute Values :
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox ID="txtMedExValue" TabIndex="4" runat="server" CssClass="textBox" Width="70%"
                                                        Visible="true" onblur="ValidateTextBox(this,'Please Enter Value in correct Format !');"></asp:TextBox>
                                                </td>
                                                <td class="Label" style="text-align: right;">Alert Message :
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox ID="txtAlertMsg" TabIndex="15" runat="server" CssClass="textBox" Width="55%"></asp:TextBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="Label" style="text-align: right;">Data Type :
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:DropDownList ID="ddlValidation" TabIndex="5" runat="server" CssClass="dropDownList"
                                                        Width="71%" onchange="OnChangeDropDown()">
                                                        <asp:ListItem Value="0">Select Validation Type</asp:ListItem>
                                                        <asp:ListItem Value="NA">Not Applicable</asp:ListItem>
                                                        <asp:ListItem Value="AN">Alpha Numeric</asp:ListItem>
                                                        <asp:ListItem Value="NU">Numeric</asp:ListItem>
                                                        <asp:ListItem Value="IN">Integer</asp:ListItem>
                                                        <asp:ListItem Value="AL">Alphabet</asp:ListItem>
                                                        <%--  <asp:ListItem Value="NNI">Not Null Integer</asp:ListItem>
                                                <asp:ListItem>Not Null Numeric</asp:ListItem>--%>
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="Label" style="text-align: right;">Ref. Table :
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:DropDownList ID="ddlRefTable" TabIndex="16" runat="server" CssClass="dropDownList"
                                                        Width="56%" OnSelectedIndexChanged="ddlRefTable_SelectedIndexChanged" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="Label" style="text-align: right;" id="td1" visible="true">Default Value :
                                                </td>
                                                <td id="td2" style="text-align: left;" visible="true">
                                                    <asp:TextBox ID="txtDefaultValue" TabIndex="6" runat="server" CssClass="textBox"
                                                        Width="70%" onblur="ValidateTextBox(this,'Please Enter Numeric Values Only !');"></asp:TextBox>
                                                </td>
                                                <td class="Label" style="text-align: right;">Ref. Column :
                                                </td>
                                                <td style="text-align: left;" visible="true">
                                                    <asp:DropDownList ID="ddlRefColumn" TabIndex="17" runat="server" CssClass="dropDownList"
                                                        Width="56%">
                                                    </asp:DropDownList>
                                                    <br />
                                                    <asp:Panel ID="pnlCheckBoxListRefColumn" runat="server" Visible="false" ScrollBars="Auto"
                                                        BorderWidth="1px" BorderStyle="Solid" Height="70px" Style="max-width: 75%;" BorderColor="Navy">
                                                        <asp:CheckBoxList ID="chkRefColumn" TabIndex="16" runat="server" CssClass="checkboxlist checkBox"
                                                            RepeatDirection="Horizontal" RepeatColumns="2" />
                                                    </asp:Panel>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="Label" style="text-align: right;">UOM :
                                                </td>
                                                <td style="text-align: left;" visible="true">
                                                    <asp:DropDownList ID="ddlUOMDesc" TabIndex="7" runat="server" CssClass="dropDownList"
                                                        Width="71%">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="Label" style="text-align: right;"></td>
                                                <td style="text-align: left;" visible="true">
                                                    <asp:CheckBox ID="chkActive" TabIndex="18" runat="server" Text="Active" Checked="True"></asp:CheckBox>
                                                    <asp:CheckBox ID="chkNotNull" TabIndex="18" runat="server" Text="Not Null" Checked="True"></asp:CheckBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Label" style="text-align: right;" visible="true">Variable Name :
                                                </td>
                                                <td style="text-align: left;" visible="true">
                                                    <asp:TextBox ID="txtCDISCValues" TabIndex="8" Width="70%" runat="server" CssClass="textBox">
                                                    </asp:TextBox>
                                                </td>
                                                <td class="Label" style="text-align: right;" visible="true">Other Values :
                                                </td>
                                                <td style="text-align: left;" visible="true">
                                                    <asp:TextBox ID="txtOtherValues" TabIndex="19" Width="55%" runat="server" CssClass="textBox">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr style="display: none" id="trFormula" runat="server">
                                                <td class="Label" style="text-align: right;">Formula :
                                                </td>
                                                <td class="Label" style="text-align: left;">
                                                    <input style="width: auto" class="button" onclick="FormulaDivShowHide('S');" type="button"
                                                        value="Create Formula" title="Create Formula" />
                                                </td>
                                                <td class="Label" style="text-align: right;" visible="true"></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td class="Label" style="text-align: right;">Role of Variable* :
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:DropDownList ID="ddlRoleofVariable" TabIndex="9" runat="server" Class="dropDownList ddl" Width="71%">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="Label" style="text-align: right;">Core of Variable* :
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:DropDownList ID="ddlCoreofVariable" TabIndex="20" runat="server" Class="dropDownList ddl" Width="56%">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td class="Label" style="text-align: right;">Attribute Category *:
                                                </td>
                                                <td class="Label" style="text-align: left;">
                                                    <asp:DropDownList ID="ddlAttributeCategory" TabIndex="10" runat="server" CssClass="dropDownList"
                                                        Width="71%" AutoPostBack="false">
                                                        <asp:ListItem Value="n" Text="Select Attribute Category"></asp:ListItem>
                                                        <asp:ListItem Value="S" Text="SDTM"></asp:ListItem>
                                                        <asp:ListItem Value="C" Text="CDASH"></asp:ListItem>
                                                        <asp:ListItem Value="O" Text="OTHER"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td style="text-align: center" class="Label" align="left" colspan="4" visible="True">
                                                    <asp:Button ID="BtnSave" ToolTip="Save" TabIndex="21" OnClick="BtnSave_Click" runat="server"
                                                        Text="Save" CssClass="btn btnsave" OnClientClick="return Validation();"></asp:Button>
                                                    <asp:Button ID="btncancel" TabIndex="22" OnClick="btncancel_Click" runat="server"
                                                        Text="Cancel" ToolTip="Cancel" CssClass="btn btncancel"></asp:Button>
                                                    <asp:Button ID="BtnExit" TabIndex="23" OnClick="BtnExit_Click" runat="server" Text="Exit"
                                                        CssClass="btn btnexit" ToolTip="Exit" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);"></asp:Button>
                                                </td>
                                            </tr>

                                            <tr style="display: none">
                                                <%--added by kt for multi check box--%>
                                                <td style="text-align: left;" colspan="4">
                                                    <asp:DropDownList ID="ddlRefColumnNew" runat="server" Style="width: 40%" CssClass="dropDownList" AutoPostBack="false" >
                                                    </asp:DropDownList>

                                                    <select id="ddltemp" data-placeholder="Choose a Country..." class="chosen-select" multiple style="width: 350px;" tabindex="4" runat="server">
                                                        <option value=""></option>
                                                        <option value="Zambia">Zambia</option>
                                                        <option value="Zimbabwe">Zimbabwe</option>
                                                        <option value="Zimbabwe">ketan</option>
                                                        <option value="Zimbabwe" selected="selected">arpti</option>
                                                        <option value="Zimbabwe" selected="selected">pus</option>

                                                        <option value="Zimbabwe">pus</option>
                                                    </select>

                                                </td>
                                            </tr>


                                            

                                        </tbody>
                                    </table>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                </tbody>
            </table>
            <table width="100%" cellpadding="3px">
                <tbody>
                    <tr>
                        <td>
                            <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img1" alt="Attribute Data" src="images/panelcollapse.png"
                                        onclick="Display(this,'divAttributeData');" runat="server" style="margin-right: 2px;" />Attribute Data</legend>
                                <div id="divAttributeData">
                                    <table width="85%" align="center">
                                        <tbody>
                                            <tr style="display: block;">
                                                <td class="Label" style="text-align: center;" visible="True" colspan="4">Attribute/AttributeGroup :
                                            <asp:TextBox ID="txtMedex" TabIndex="24" runat="server" CssClass="textBox" Width="40%"></asp:TextBox>
                                                    <asp:HiddenField ID="hMedexCode" runat="server"></asp:HiddenField>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                                        CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                        CompletionListElementID="pnlAttributeList" CompletionListItemCssClass="autocomplete_listitem"
                                                        MinimumPrefixLength="1" OnClientItemSelected="OnSelected" OnClientShowing="ClientPopulated"
                                                        ServicePath="AutoComplete.asmx" TargetControlID="txtMedex" UseContextKey="True"
                                                        ServiceMethod="GetMedexList">
                                                    </cc1:AutoCompleteExtender>
                                                    <asp:Panel ID="pnlAttributeList" runat="server" Style="max-height: 300px; overflow: auto; overflow-x: hidden" />
                                                    <asp:Button Style="display: none" ID="btnSetMedex" OnClick="btnSetMedex_Click" runat="server"
                                                        Text="Medex"></asp:Button>
                                                    <asp:Button ID="BtnViewAll" TabIndex="22" runat="server" Text="View All" ToolTip="View All"
                                                        CssClass="btn btnnew" />
                                                </td>
                                            </tr>
                                            <tr>
                                            <td align="center" style="padding-bottom: 2px;">
                                                <asp:PlaceHolder ID="phTopPager" runat="server"></asp:PlaceHolder>
                                            </td>
                                            </tr>
                                            <tr>
                                                <td colspan="11">
                                                    <div id="div_gvmedex" style="width: 1234px;display: none;" runat="server">
                                                        <asp:GridView ID="gvmedex" runat="server" AutoGenerateColumns="False" Style="margin: auto; display: none; width: 100%;">
                                                            <Columns>
                                                                <%--<asp:BoundField DataFormatString="number" HeaderText="#">
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:BoundField>--%>
                                                                <asp:BoundField DataField="RowNo" HeaderText="#" />
                                                                <asp:BoundField DataField="vMedExCode" HeaderText="AttributeCode" />
                                                                <asp:BoundField DataField="vMedExDesc" HeaderText="Attribute Desc">
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="vMedExGroupDesc" HeaderText="Attribute Group Desc">
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="vMedExSubGroupDesc" HeaderText="Attribute SubGroup Desc" />
                                                                <asp:BoundField DataField="vMedExType" HeaderText="Attribute Type" />
                                                                <asp:BoundField DataField="vMedExValues" HeaderText="Attribute Values">
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="vDefaultValue" HeaderText="Default Value" />
                                                                <asp:BoundField DataField="vUOM" HeaderText="UOM" />
                                                                <asp:BoundField DataField="vLowRange" HeaderText="Low Range" />
                                                                <asp:BoundField DataField="vHighRange" HeaderText="High Range" />
                                                                <asp:BoundField DataField="vValidationType" HeaderText="Validation Type" />
                                                                <asp:BoundField DataField="cActiveFlag" HeaderText="ActiveFlag" />
                                                                <asp:BoundField DataField="vRefTable" HeaderText="Ref. Table" />
                                                                <asp:BoundField DataField="vRefColumn" HeaderText="Ref. Column" />
                                                                <asp:TemplateField HeaderText="Edit">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ImbEdit" runat="server" ToolTip="Edit" ImageUrl="~/images/Edit2.gif" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Delete">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ImbDelete" runat="server" ToolTip="Delete" ImageUrl="~/images/i_delete.gif"
                                                                            OnClientClick="return msgconfirmalert('Are You Sure You Want To Delete?',this)" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>


                                                    <div id="divMedExMstData">
                                                        <table id="tblMEdExMSt" class="" width="100%" style="background-color: #B0AFAF;"></table>
                                                    </div>

                                                    <asp:Button ID="BtnUpdate" Text="Update" ToolTip="Update" Style="display: none;"
                                                        runat="server" CssClass="btn btnsave" />

                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" style="padding-top: 2px;">
                                                    <asp:PlaceHolder ID="phBottomPager" runat="server"></asp:PlaceHolder>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" cellpadding="5px">
                                <tbody>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnShow" runat="server" Text="Show Dialog" Style="display: none" />
                                            <cc1:ModalPopupExtender ID="MPEdivFormula" runat="server" TargetControlID="btnShow"
                                                PopupControlID="divFormula" BackgroundCssClass="modalBackground" BehaviorID="MPEId">
                                            </cc1:ModalPopupExtender>
                                            <asp:UpdatePanel runat="server">
                                                <ContentTemplate>
                                                    <div style="display: none; left: 391px; width: 95%; top: 528px; text-align: left; height: 500px;"
                                                        id="divFormula" runat="server" class="centerModalPopup">
                                                        <table style="width: 100%;">
                                                            <tbody>
                                                                <tr>
                                                                    <td align="center" width="100%">
                                                                        <strong style="white-space: nowrap">Attribute Formula Master</strong>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center" width="100%">
                                                                        <div id="pnlFormulaMedEx" runat="server" style="width: 80%; overflow: auto; height: 300px;">
                                                                            <input type="text" id="searchlist" placeholder="Search Attribute..." style="float: left; width: 98%;"
                                                                                onkeyup="return DoListBoxFilter($('#ctl00_CPHLAMBDA_lstMedEx'), this.value, keys, values);" />
                                                                            <asp:ListBox ID="lstMedEx" runat="server" Height="249px" TabIndex="25" Style="width: 80% !important; overflow: auto;"
                                                                                CssClass="listbox"></asp:ListBox>
                                                                            <%--<cc1:ListSearchExtender  ID ="MEDEXlist"  runat ="server" TargetControlID ="lstMedEx" IsSorted =" true" PromptCssClass ="csextender" PromptPosition= "Bottom " PromptText ="Select Attribute" ></cc1:ListSearchExtender>--%>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center" width="100%">
                                                                        <table width="60%" style="margin: auto;">
                                                                            <tr>
                                                                                <td colspan="4">
                                                                                    <asp:Label ID="lblShowInEdit" Text="In Edit Mode, Clear All Formula and then Make it New one"
                                                                                        runat="server" Style="display: none"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <input class="button" tabindex="26" style="width: 100%;" onclick="CopyMedEx();" type="button"
                                                                                        title="Copy Attribute" value="Copy Attribute" />
                                                                                </td>
                                                                                <td>
                                                                                    <input class="button" tabindex="30" onclick="ClearAllFormulaText();" type="button"
                                                                                        value="Clear All" title="Clear All" />
                                                                                    <input class="button" tabindex="29" onclick="ClearFormulaText();" type="button" value="Clear"
                                                                                        title="Clear" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="labelOperator" runat="server" CssClass="Label" Text="Operator"></asp:Label>
                                                                                    <asp:DropDownList ID="ddlOperator" TabIndex="27" runat="server" CssClass="Label"
                                                                                        Width="88px">
                                                                                        <asp:ListItem></asp:ListItem>
                                                                                        <asp:ListItem Text="+"></asp:ListItem>
                                                                                        <asp:ListItem Text="-"></asp:ListItem>
                                                                                        <asp:ListItem Text="*"></asp:ListItem>
                                                                                        <asp:ListItem Text="/"></asp:ListItem>
                                                                                        <asp:ListItem Text="("></asp:ListItem>
                                                                                        <asp:ListItem Text=")"></asp:ListItem>
                                                                                        <asp:ListItem Text="^"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblDigits" runat="server" CssClass="Label" Text="Digits"></asp:Label>
                                                                                    <asp:DropDownList ID="ddlNumbers" TabIndex="28" runat="server" CssClass="Label" Width="88px">
                                                                                        <asp:ListItem></asp:ListItem>
                                                                                        <asp:ListItem Text="."></asp:ListItem>
                                                                                        <asp:ListItem Text="0"></asp:ListItem>
                                                                                        <asp:ListItem Text="1"></asp:ListItem>
                                                                                        <asp:ListItem Text="2"></asp:ListItem>
                                                                                        <asp:ListItem Text="3"></asp:ListItem>
                                                                                        <asp:ListItem Text="4"></asp:ListItem>
                                                                                        <asp:ListItem Text="5"></asp:ListItem>
                                                                                        <asp:ListItem Text="6"></asp:ListItem>
                                                                                        <asp:ListItem Text="7"></asp:ListItem>
                                                                                        <asp:ListItem Text="8"></asp:ListItem>
                                                                                        <asp:ListItem Text="9"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    &nbsp;&nbsp;&nbsp;
                                                                                    <asp:Label ID="lblDecimal" runat="server" CssClass="Label" Text="DecimalNo"></asp:Label>
                                                                                    <asp:TextBox ID="txtDecimal" runat="server" CssClass="Label " ToolTip="Enter the No for Decimal Place"
                                                                                        Width="70"></asp:TextBox>
                                                                                    <%--<asp:RequiredFieldValidator ID ="r1" ControlToValidate="txtDecimal" runat="server" ErrorMessage="Enter the decimal point" Font-Size="7px" ></asp:RequiredFieldValidator>--%>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align: center" class="Label" align="left" colspan="4">
                                                                                    <asp:TextBox ID="txtFormula" TabIndex="28" runat="server" Width="800px" TextMode="MultiLine"
                                                                                        Height="95px" Enabled="false"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align: center" class="Label" align="right" colspan="4">
                                                                                    <asp:Button ID="btnSaveFormula" TabIndex="31" OnClick="btnSaveFormula_Click" runat="server"
                                                                                        OnClientClick="return Validation1();" Text="Save" ToolTip="Save" CssClass="btn btnsave"></asp:Button>
                                                                                    <input class="btn btnclose" tabindex="32" onclick="FormulaDivShowHide('H');" type="button"
                                                                                        value="Close" title="Close" />
                                                                                    <asp:HiddenField ID="HFFormula" runat="server"></asp:HiddenField>
                                                                                    <asp:HiddenField ID="HFMedexCodeUsedForFormula" runat="server"></asp:HiddenField>
                                                                                    <asp:HiddenField ID="HFdecimalText" runat="server" />
                                                                                    <asp:HiddenField ID="hdnRefColName" runat="server" Value="" />

                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSaveFormula" EventName="Click" />
            <%--<asp:AsyncPostBackTrigger ControlID="BtnUpdate" EventName="Click" />--%>
            <asp:PostBackTrigger ControlID="BtnUpdate" />
        </Triggers>
    </asp:UpdatePanel>


    <asp:HiddenField ID="hdnMedexCode" runat="server" Value="" />
    <asp:HiddenField ID="hdnStatus" runat="server" Value="" />

    <script src="Script/jquery-ui-1.10.4.custom.min.js" type="text/javascript"></script>
    <script src="Script/jquery.multiselect.min.js" type="text/javascript"></script>
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>


    <script type="text/javascript">       

        function HideAttributeDetails() {
            $('#<%= img2.ClientID%>').click();
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

        function UIgvmedex() {
            //bindDropDrownMulitiselect();

            //GetMedExMstData();

            //return true;

            $('#<%= gvmedex.ClientID%>').removeAttr('style', 'display:block');
            var scrolly = "250px";
            if ($('#<%= gvmedex.ClientID%> tr').length < 10) {
                scrolly = "25%";
            }
            oTab = $('#<%= gvmedex.ClientID%>').prepend($('<thead>').append($('#<%= gvmedex.ClientID%> tr:first'))).dataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": false,
                //"sScrollY": "250px",
                "sScrollY": scrolly,
                //"bScrollCollapse": true,
                aLengthMenu: [
                    [10, 25, 50, 100, -1],
                    [10, 25, 50, 100, "All"]
                ],
            });
            HideAttributeDetails();
            return false;
        }


        function UIgvmedexRefresh() {
            $('#<%= gvmedex.ClientID%>').attr('style', 'display:block');
            document.getElementById('<%=div_gvmedex.ClientID%>').style.display = 'block';

            oTab = $('#<%= gvmedex.ClientID%>').prepend($('<thead>').append($('#<%= gvmedex.ClientID%> tr:first'))).dataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": 1000,
                "bProcessing": true,
                "bSort": false,
                "sScrollY": "250px",
                "sScrollX": "100%",
                "bScrollCollapse": true,
                "bDestroy": true,
                aLengthMenu: [
                    [-1],
                    ["All"]
                ],
            });
            $('#ctl00_CPHLAMBDA_gvmedex_info').empty()
            return false;
        }

        function ClientPopulated(sender, e) {
            MedexShowing('AutoCompleteExtender1', $get('<%= txtMedex.ClientId %>'));
        }

        function OnSelected(sender, e) {
            OnMedexSelected(e.get_value(), $get('<%= txtMedex.clientid %>'),
            $get('<%= hMedexCode.clientid %>'), document.getElementById('<%= btnSetMedex.ClientId %>'));
        }

        function textonblur() {
            var txtSrc = document.getElementById('<%=txtMedExValue.ClientId %>');
            var txtDst = document.getElementById('<%=txtDefaultValue.ClientId %>');
            var index;

            if (txtSrc.value.indexOf(",") > -1) {
                index = txtSrc.value.indexOf(",");
                txtDst.value = txtSrc.value.substr(0, index);
            }
            else {
                txtDst.value = txtSrc.value;
            }
        }

        function ValidateNumeric(txtBox, msg) {
            var e = document.getElementById("ctl00_CPHLAMBDA_ddlValidation");
            var strUser = e.options[e.selectedIndex].text;
            // var CommaSepratedNumbers=/[0-9]+(,[0-9]+)*,?/ ;
            var CommaSepratedNumbers = /([1-9][0-9]*,)*[0-9][0-9]*/;
            value3 = document.getElementById("ctl00_CPHLAMBDA_txtlength").value;
            var arrayList = value3.match(CommaSepratedNumbers);
            if (e.options[e.selectedIndex].text == 'Numeric') {
                if (parseInt(value3.split(",")[0]) < parseInt(value3.split(",")[1])) {
                    msgalert('Please Enter correct Scale !');
                    txtBox.value = "";
                    txtBox.focus();
                    return false;
                }
                if (e.options[e.selectedIndex].text == 'Numeric' && (arrayList[1] == '' || typeof (arrayList[1]) == 'undefined')) {
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
                var result = CheckInteger(txtBox.value);
            //                var reg=/^(([0-9a-zA-Z](,)?)*)+$/;
            //var reg=/(?!,)(?:(?:,|^)([-+]?\d+))*$/;

            // var Code=/^(([0-9a-zA-Z](,)?)*)+$/ 
            if (result == false) {
                window.alert(msg);
                txtBox.value = "";
                txtBox.focus();
            }

        }

        function Validation1() {
            if (document.getElementById('<%=txtDecimal.ClientID%>').value == '') {
                msgalert('Please enter Decimal No !');
                document.getElementById('<%=txtDecimal.ClientID%>').focus();
                return false;
            }
        }

        function Validation() {
            if (document.getElementById('<%=txtmedexdesc.ClientID%>').value.toString().replace(/^\s*/, '').replace(/\s*$/, '').length <= 0) {
                document.getElementById('<%=txtmedexdesc.ClientID%>').value = '';
                document.getElementById('<%=txtmedexdesc.ClientID%>').focus();
                msgalert('Please Enter Attribute Description !');
                return false;
            }
            else if (document.getElementById('<%=ddlmedexgroup.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Attribute Group !');
                return false;
            }
            else if (document.getElementById('<%=ddlMedExSubGroupDesc.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Attribute SubGroup !');
                return false;
            }
            else if (document.getElementById('<%=ddlMedExType.ClientID%>').selectedIndex == "n") {
                msgalert('Please Select Attribute Type !');
                return false;
            }
            else if (document.getElementById('<%=txtDecimal.ClientID%>').value = '') {
                msgalert('PLease enter Decimal No !');
                return false;
            }
            else if (document.getElementById('<%=ddlRoleofVariable.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Role of Variable !');
                return false;
            }
            else if (document.getElementById('<%=ddlAttributeCategory.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Atleast One Attribute Category !');
                return false;
            }
            else if (document.getElementById('<%=ddlCoreofVariable.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Core of Variable !');
                return false;
            }
            

            var e = document.getElementById("ctl00_CPHLAMBDA_ddlMedExType");
            var strUser = e.options[e.selectedIndex].text;
            var countchecked = 0;

            if (e.options[e.selectedIndex].text == 'ComboGlobalDictionary') {
                if (document.getElementById('<%=ddlRefTable.ClientID%>').selectedIndex == 0) {
                    msgalert('Please Select Ref. Table !');
                    return false;
                }
                $('.checkBox input[type="checkBox"]').each(function () {
                    if (this.checked == true) {
                        countchecked = countchecked + 1;
                    }
                });
                if (countchecked == 0) {
                    msgalert('Please Select atleast one Ref Column For Dictionary !');
                    return false;
                }
            }

            var txtMedExValue = document.getElementById('<%=txtMedExValue.ClientId %>').value;
            var txtAlert = document.getElementById('<%=txtAlerton.ClientID%>').value;
            var result = txtMedExValue.split(",");

            if (document.getElementById('<%=txtAlerton.ClientID%>').value.toString().replace(/^\s*/, '').replace(/\s*$/, '').length > 0) {
                for (var i in result) {
                    if (txtAlert == result[i]) {
                        return true;
                    }
                }
                msgalert('Please Enter Correct Alert On Value !');
                return false;
            }
            return true;
        }

        function FormulaDivShowHide(Type) {
            if (Type == 'S') {
                $find('MPEId').show();
                document.getElementById('<%=div_gvmedex.ClientID%>').style.display = 'none';
                return false;
            }
            else if (Type == 'H') {
                $find('MPEId').hide();
                document.getElementById('<%=div_gvmedex.ClientID%>').style.display = 'block';
                return false;
            }
            return true;
        }
        function ClearFormulaText() {
            var txt = document.getElementById('<%=txtFormula.ClientID%>').value;
            var formula = document.getElementById('<%=HFFormula.ClientID%>').value;
            txt = txt.substring(0, txt.length - 1)
            formula = formula.substring(0, formula.length - 2)
            document.getElementById('<%=txtFormula.ClientID%>').value = txt
            document.getElementById('<%=HFFormula.ClientID%>').value = formula;
        }
        function ClearAllFormulaText() {
            document.getElementById('<%=txtFormula.ClientID%>').value = '';
            document.getElementById('<%=HFFormula.ClientID%>').value = '';
            document.getElementById('<%=HFMedexCodeUsedForFormula.ClientID%>').value = '';
        }

        function SetOperator() {
            var op = document.getElementById('<%=ddlOperator.ClientID%>').value;
            if (op != '') {
                document.getElementById('<%=txtFormula.ClientID%>').value += op;
                document.getElementById('<%=HFFormula.ClientID%>').value += op + '?';
            }
            document.getElementById('<%=ddlOperator.ClientID%>').selectedIndex = 0;
        }

        function SetNumber() {
            var op = document.getElementById('<%=ddlNumbers.ClientID%>').value;
            if (op != '') {
                document.getElementById('<%=txtFormula.ClientID%>').value += op;
                document.getElementById('<%=HFFormula.ClientID%>').value += op + '?';
            }
            document.getElementById('<%=ddlNumbers.ClientID%>').selectedIndex = 0;
        }

        function CopyMedEx() {
            var vMedexCode = new Array
            var aLength = vMedexCode.length
            var lst = document.getElementById('<%=lstMedEx.ClientID%>');
            var items = lst.options.length;
            if (lst != null && typeof (lst) != 'undefined') {
                for (i = 0; i < items; i++) {
                    if (lst.options[i].selected) {
                        document.getElementById('<%=txtFormula.ClientID%>').value += lst.options[i].text;
                        document.getElementById('<%=HFFormula.ClientID%>').value += lst.options[i].value + '?';
                        document.getElementById('<%=HFMedexCodeUsedForFormula.ClientID%>').value += lst.options[i].value + ',';
                        break;
                    }
                }
            }
        }

        function setNext() {
            document.getElementById('<%=ddlmedexgroup.ClientID%>').focus();
        }

        function OnChangeDropDown() {
            var e = document.getElementById("ctl00_CPHLAMBDA_ddlValidation");
            if (e.options[e.selectedIndex].text == 'Numeric') {
                document.getElementById('<%=txtlength.ClientID%>').value = '0,0';
            }
            else {
                document.getElementById('<%=txtlength.ClientID%>').value = 0;
            }
            return false;
        }

        function ValidateTextBox(txtBox, msg) {
            var e = document.getElementById("ctl00_CPHLAMBDA_ddlValidation");
            var strUser = e.options[e.selectedIndex].text;

            if (e.options[e.selectedIndex].text == 'Numeric') {
                var result = CheckDecimalOrBlank(txtBox.value);
                if (result == false) {
                    window.msgalert(msg);
                    txtBox.value = "";
                    txtBox.focus();
                }
            }
            else if (e.options[e.selectedIndex].text == 'Integer') {
                var result = CheckIntegerOrBlank(txtBox.value);
                if (result == false) {
                    window.msgalert(msg);
                    txtBox.value = "";
                    txtBox.focus();
                }
            }
        }

        function ValidateTextBoxInteger(txtBox, msg) {
            var result = CheckInteger(txtBox.value);
            if (result == false) {
                window.msgalert(msg);
                txtBox.value = "";
                txtBox.focus();
            }
        }

        function isKeyPress(txt) {
            var txtlength = txt.value.length;
            if (txtlength > 50) {
                msgalert("Length Should not be greater than 50 !");
                return false;
            }
        }

        var win;
        var keys = [];
        var values = [];

        function DoListBoxFilter(listBoxSelector, filter, keys, values) {
            var list = $(listBoxSelector);
            var selectBase = '<option value="{0}">{1}</option>';
            list.html("");
            for (i = 0; i < values.length; ++i) {
                var value = values[i];

                if (value == "" || value.toLowerCase().indexOf(filter.toLowerCase()) >= 0) {
                    var temp = '<option value="' + keys[i] + '">' + value + '</option>';
                    list.append(temp);
                }
            }
        }

        var options = $('#ctl00_CPHLAMBDA_lstMedEx option');
        $.each(options, function (index, item) {
            keys = keys.concat(item.value);
            values = values.concat(item.text);
        });

        $(document).ready(function () {
            var watermark = 'Enter text to Search Attribute...';
            $('#searchlist').blur(function () {
                if ($(this).val().length == 0)
                    $(this).val(watermark).addClass('watermark');
            }).focus(function () {
                if ($(this).val() == watermark)
                    $(this).val('').removeClass('watermark');
            }).val(watermark).addClass('watermark');


            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            function EndRequestHandler(sender, args) {
                var watermark = 'Enter text to Search Attribute...';
                $('#searchlist').blur(function () {

                    if ($(this).val().length == 0)
                        $(this).val(watermark).addClass('watermark');
                }).focus(function () {
                    if ($(this).val() == watermark)
                        $(this).val('').removeClass('watermark');
                }).val(watermark).addClass('watermark');
            }

        });




function GetMedExMstData() {
    var TotalActivity;
    var id = '';
    try {
        $.ajax({
            type: "post",
            url: "frmAttributeMst.aspx/GetMedExMstData",
            data: '{"vMedexCode":"' + id + '"}',
            contentType: "application/json; charset=utf-8",
            datatype: JSON,
            async: false,
            dataType: "json",
            success: function (dataset) {


                var data;
                data = eval('(' + dataset.d + ')');
                $('#tblMEdExMSt').attr("IsTable", "has");

                var ActivityDataset = [];
                if (data != "" && data != null) {
                    TotalActivity = data.length;
                    for (var Row = 0; Row < data.length; Row++) {
                        var InDataset = [];
                        InDataset.push(
                            Row + 1,
                            data[Row].vMedExDesc,
                            data[Row].vMedExGroupDesc,
                            data[Row].vMedExSubGroupDesc,
                            " ",
                            data[Row].vDefaultValue,
                            data[Row].vUOM,
                            data[Row].vLowRange,
                            data[Row].vHighRange,
                            data[Row].vRoleDescription,
                            data[Row].vCoreDescription,
                            " ",
                            " ",
                            data[Row].vMedExCode,
                            data[Row].vMedExValues
                            );
                        ActivityDataset.push(InDataset);
                    }

                    otable = $('#tblMEdExMSt').dataTable({

                        "bJQueryUI": true,
                        "sPaginationType": "full_numbers",
                        "bLengthChange": true,
                        "iDisplayLength": 10,
                        "bProcessing": true,
                        "bSort": false,
                        "autoWidth": true,
                        "aaData": ActivityDataset,
                        "bInfo": true,
                        "bDestroy": true,
                        "bScrollCollapse": true,
                        "sScrollY": "285px",
                        aLengthMenu: [
                           [10, 25, 50, -1],
                           [10, 25, 50, "All"]
                        ],

                        "fnCreatedRow": function (nRow, aData, iDataIndex) {
                            if (aData[14] != "") {
                                $('td:eq(4)', nRow).append("<p title='"+ aData[14] +"'>" + aData[14].substring(0, 20) + "..</p>");
                            }
                            else {
                                $('td:eq(4)', nRow).append("<p></p>");
                            }

                            $('td:eq(11)', nRow).append("<input type='image' id='imgEdit_" + iDataIndex + "' name='imgAudit$" + iDataIndex + "' src='images/Edit2.gif' OnClick='EditData(this); return false;' style='border-width:0px;'   vmedexcode='" + aData[13] + "' >");
                            $('td:eq(12)', nRow).append("<input type='image' id='imgDelete_" + iDataIndex + "' name='imgExport$" + iDataIndex + "' src='images/i_delete.gif' OnClick='DeleteData(this); return false;' style='border-width:0px;'   vmedexcode='" + aData[13] + "' >");
                        },

                        "aoColumns": [
                                    { "sTitle": "#" },
                                    { "sTitle": "Attribute Desc" },
                                    { "sTitle": "Attribute Group Desc" },
                                    { "sTitle": "Attribute Sub Group Desc" },
                                    { "sTitle": "Attribute Value" },
                                    { "sTitle": "Default Value" },
                                    { "sTitle": "UOM" },
                                    { "sTitle": "Low Range" },
                                    { "sTitle": "High Range" },
                                    { "sTitle": "Role of Variable" },
                                    { "sTitle": "Core of Variable" },
                                    { "sTitle": "Edit" },
                                    { "sTitle": "Delete" },

                        ],

                        "oLanguage": {
                            "sEmptyTable": "No Record Found",
                        },

                    });
                }
                return false;
            },
            failure: function (response) {
                 msgalert(response.d);
            },
            error: function (response) {
                msgalert(response.d);
            }
        });
        otable.fnAdjustColumnSizing();
        return false;
    }
    catch (err) {
        msgalert('GetPanelDisplayData function : ' + err.message);
        return false;

    }
}

var vMedExCode = 0;
function EditData(e) {
    var TotalActivity;
    var id = e.attributes.vmedexcode.value;
    $('#ctl00_CPHLAMBDA_hdnMedexCode').val(id);
    $('#ctl00_CPHLAMBDA_hdnStatus').val("MYEDIT");
    //$('#ctl00_CPHLAMBDA_BtnUpdate').click();
    __doPostBack('ctl00$CPHLAMBDA$BtnUpdate', '');
    return false;
}
function DeleteData(e) {
    var TotalActivity;
    var id = e.attributes.vmedexcode.value;
    $('#ctl00_CPHLAMBDA_hdnMedexCode').val(id);
    $('#ctl00_CPHLAMBDA_hdnStatus').val("MYDELETE");

    msgConfirmDeleteAlert(null, "Are You Sure You Want To Delete?", function (isConfirmed) {
        if (isConfirmed) {
            __doPostBack('ctl00$CPHLAMBDA$BtnUpdate', '');
            return true;
        } else {
            return false;
        }
    });


    return false;
}

//For Display REf Columns name
function fnRefColumnNew() {
    var RefColName = [];

    document.getElementById('<%= hdnRefColName.ClientID%>').value = "";
        for (i = 0; i < $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlRefColumnNew']:checked").length ; i++) {
            RefColName.push("" + $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_ddlRefColumnNew']:checked").eq(i).attr("value") + "");
        }
        document.getElementById('<%= hdnRefColName.ClientID%>').value = RefColName;
        return true;
    }

    var RefColName = [];
    function fnApplyRefColName() {
        $("#<%= ddlRefColumnNew.ClientID%>").multiselect({
            noneSelectedText: "--Select Refence column Name--",
            click: function (event, ui) {
                if (ui.checked == true)
                    RefColName.push("'" + ui.value + "'");
                else if (ui.checked == false) {
                    if ($.inArray("'" + ui.value + "'", RefColName) >= 0)
                        RefColName.splice(RefColName.indexOf("'" + ui.value + "'"), 1)
                }
                if ($("input[name$='ddlRefColumnNew']").length > 0) {
                    //clearControls();
                }
            },
            checkAll: function (event, ui) {
                RefColName = [];
                for (var i = 0; i < event.target.options.length; i++) {
                    RefColName.push("'" + $(event.target.options[i]).val() + "'")
                }

            },
            uncheckAll: function (event, ui) {
                RefColName = [];

            }
        });

        $("#<%= ddlRefColumnNew.ClientID%>").multiselect("refresh");
        var CheckedCheckBox = document.getElementById('<%= hdnRefColName.ClientID%>').value
        if (CheckedCheckBox != "") {

            CheckedCheckBox = CheckedCheckBox.split(',');
            for (i = 0; i <= CheckedCheckBox.length - 1; i++) {
                $("#<%= ddlRefColumnNew.ClientID%>").multiselect("widget").find(".ui-multiselect-checkboxes.ui-helper-reset").find("input[value=" + CheckedCheckBox[i] + "]").attr("checked", "checked");

            }
            $('#<%= ddlRefColumnNew.ClientID%>').multiselect("update");
        }
    }

    function MultiselectRequired() {
        $('#<%= ddlRefColumnNew.ClientID%>').multiselect({
            includeSelectAllOption: true
        });
    }

    </script>

</asp:Content>
