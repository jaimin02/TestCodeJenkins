<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmMedExMst.aspx.vb" Inherits="frmMedExMst" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/General.js"></script>

    <script type="text/javascript" src="Script/Validation.js"></script>

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript">
function ClientPopulated(sender, e)
            {
                MedexShowing('AutoCompleteExtender1',$get('<%= txtMedex.ClientId %>'));
            }
            
function OnSelected(sender,e)
            {
                OnMedexSelected(e.get_value(), $get('<%= txtMedex.clientid %>'),
                    $get('<%= hMedexCode.clientid %>'),document.getElementById('<%= btnSetMedex.ClientId %>') );
              
            }                 
    
function  textonblur()
{

    var txtSrc = document.getElementById ('<%=txtMedExValue.ClientId %>');
    var txtDst = document.getElementById ('<%=txtDefaultValue.ClientId %>');
    var index;
    
    if (txtSrc.value.indexOf(",") > -1)
    {
        index = txtSrc.value.indexOf(",");
        txtDst.value = txtSrc.value.substr(0,index);
    }
    else
    {
        txtDst.value = txtSrc.value;
    }
 }
 
  function ValidateNumeric(txtBox, msg)
    {
        var result = CheckDecimalOrBlank(txtBox.value);
        if (result == false)
        {
            window.alert(msg);
            txtBox.value = "";
            txtBox.focus();
        }
    }
    
  function Validation()
    {
     
        if (document.getElementById('<%=txtmedexdesc.ClientID%>').value.toString().replace(/^\s*/,'').replace(/\s*$/,'').length <= 0)
        {
            document.getElementById('<%=txtmedexdesc.ClientID%>').value='';
            document.getElementById('<%=txtmedexdesc.ClientID%>').focus();
            msgalert('Please Enter Attribute Description !');
            return false;
        }
   
        var txtMedExValue = document.getElementById('<%=txtMedExValue.ClientId %>').value;
        var txtAlert = document.getElementById('<%=txtAlerton.ClientID%>').value;
        var result = txtMedExValue.split(",");
        
        if (document.getElementById('<%=txtAlerton.ClientID%>').value.toString().replace(/^\s*/,'').replace(/\s*$/,'').length > 0)
        {
            
            for(var i in result)
            {
              if (txtAlert == result[i])
              {
                return true;
              }
            }
               
            msgalert('Please Enter Correct alert on value !');
        
         return false;
        }
       return true;        
    }

    function FormulaDivShowHide(Type)
        {
            if (Type =='S')
            {
                document.getElementById('<%=divFormula.ClientID%>').style.display = 'block';
                SetCenter('<%=divFormula.ClientID%>');
                return false;
            }
            else if (Type =='H')
            {
                document.getElementById('<%=divFormula.ClientID%>').style.display = 'none';
                return false;
            }
            return true;
        }
    function ClearFormulaText()
        {
            var txt = document.getElementById('<%=txtFormula.ClientID%>').value;
            var formula = document.getElementById('<%=HFFormula.ClientID%>').value;
            txt = txt.substring(0,txt.length-1)
            formula = formula.substring(0,formula.length-2)
            document.getElementById('<%=txtFormula.ClientID%>').value = txt
            document.getElementById('<%=HFFormula.ClientID%>').value = formula;
//            document.getElementById('<%=txtFormula.ClientID%>').value = '';
//            document.getElementById('<%=HFFormula.ClientID%>').value = '';
        }
    function ClearAllFormulaText()
        {         
            document.getElementById('<%=txtFormula.ClientID%>').value = '';
            document.getElementById('<%=HFFormula.ClientID%>').value = '';
        }
    function SetOperator()
        {
            var op = document.getElementById('<%=ddlOperator.ClientID%>').value;
            if(op != '')
            {
                document.getElementById('<%=txtFormula.ClientID%>').value += op;
                document.getElementById('<%=HFFormula.ClientID%>').value += op + '?';
            }
        }  
    function SetNumber()
        {
            var op = document.getElementById('<%=ddlNumbers.ClientID%>').value;
            if(op != '')
            {
                document.getElementById('<%=txtFormula.ClientID%>').value += op;
                document.getElementById('<%=HFFormula.ClientID%>').value += op + '?';
            }
        }  
    function CopyMedEx()
        {
            var lst = document.getElementById('<%=lstMedEx.ClientID%>');
            var items = lst.options.length;
            if ( lst != null && typeof ( lst ) != 'undefined')
            {
                for ( i=0; i< items; i++)
                {
                    if (lst.options[i].selected)
                    {
                        document.getElementById('<%=txtFormula.ClientID%>').value += lst.options[i].text;
                        document.getElementById('<%=HFFormula.ClientID%>').value += lst.options[i].value + '?';
                        break;
                    }
                }
            }
        }
    </script>

    <script src="Script/Validation.js" type="text/javascript"></script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table align="center">
                <tbody>
                    <tr>
                        <td style="width: 175px" class="Label" align="left">
                            AttributeDesc* :
                        </td>
                        <td style="width: 159px" align="left">
                            <asp:TextBox ID="txtmedexdesc" TabIndex="1" runat="server" CssClass="textBox" __designer:wfdid="w35"></asp:TextBox>
                        </td>
                        <td style="width: 71px" class="Label" align="left">
                            LowRange :
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtLowRange" TabIndex="9" runat="server" CssClass="textBox" __designer:wfdid="w38"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 175px" class="Label" align="left">
                            AttributeGroupDesc* :
                        </td>
                        <td style="width: 159px" align="left">
                            <asp:DropDownList ID="ddlmedexgroup" TabIndex="2" runat="server" CssClass="dropDownList"
                                Width="148px" __designer:wfdid="w37">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 71px" class="Label" align="left">
                            HighRange :
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtHighRange" TabIndex="10" runat="server" CssClass="textBox" __designer:wfdid="w40"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 175px" class="Label" align="left">
                            AttributeSubGroupDesc* :
                        </td>
                        <td style="width: 159px" align="left">
                            <asp:DropDownList ID="ddlMedExSubGroupDesc" TabIndex="3" runat="server" CssClass="dropDownList"
                                Width="150px" __designer:wfdid="w39">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 71px" class="Label" align="left">
                            Length :
                        </td>
                        <td align="left">
                            <asp:TextBox onblur="ValidateNumeric(this,'Please Enter Numeric Values Only !');"
                                ID="txtlength" TabIndex="11" runat="server" CssClass="textBox" __designer:wfdid="w42">0</asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 175px" class="Label" align="left">
                            attributeType* :
                        </td>
                        <td style="width: 159px" align="left">
                            <asp:DropDownList ID="ddlMedExType" TabIndex="4" runat="server" CssClass="dropDownList"
                                Width="150px" __designer:wfdid="w41" AutoPostBack="True" OnSelectedIndexChanged="ddlMedExType_SelectedIndexChanged">
                                <asp:ListItem Value="1">TextBox</asp:ListItem>
                                <asp:ListItem Value="2">TextArea</asp:ListItem>
                                <asp:ListItem Value="3">ComboBox</asp:ListItem>
                                <asp:ListItem Value="4">Radio</asp:ListItem>
                                <asp:ListItem Value="5">CheckBox</asp:ListItem>
                                <asp:ListItem Value="6">File</asp:ListItem>
                                <asp:ListItem Value="7">DateTime</asp:ListItem>
                                <asp:ListItem Value="8">Time</asp:ListItem>
                                <asp:ListItem Value="9">Import</asp:ListItem>
                                <asp:ListItem Value="10">ComboGlobalDictionary</asp:ListItem>
                                <asp:ListItem Value="11">Formula</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 71px" class="Label" align="left">
                            Alert On Value :
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtAlerton" TabIndex="10" runat="server" CssClass="textBox" __designer:wfdid="w20"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 175px" class="Label" align="left">
                            AttributeValues :
                        </td>
                        <td style="width: 159px" align="left">
                            <asp:TextBox onblur="textonblur();" ID="txtMedExValue" TabIndex="5" runat="server"
                                CssClass="textBox" Visible="true" __designer:wfdid="w43"></asp:TextBox>
                        </td>
                        <td style="width: 71px" class="Label" align="left">
                            Alert Message :
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtAlertMsg" TabIndex="10" runat="server" CssClass="textBox" __designer:wfdid="w21"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 175px" class="Label" align="left">
                            ValidationType :
                        </td>
                        <td style="width: 159px" align="left">
                            <asp:DropDownList ID="ddlValidation" TabIndex="6" runat="server" CssClass="dropDownList"
                                Width="122px" __designer:wfdid="w46">
                                <asp:ListItem>NA</asp:ListItem>
                                <asp:ListItem>AN</asp:ListItem>
                                <asp:ListItem>NU</asp:ListItem>
                                <asp:ListItem>IN</asp:ListItem>
                                <asp:ListItem>AL</asp:ListItem>
                                <asp:ListItem>NNI</asp:ListItem>
                                <asp:ListItem>NNU</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 71px" class="Label" align="left">
                            Ref. Table :
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlRefTable" TabIndex="12" runat="server" CssClass="dropDownList"
                                Width="160px" __designer:wfdid="w1" AutoPostBack="True" OnSelectedIndexChanged="ddlRefTable_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="med1" runat="server">
                        <td style="width: 175px; height: 19px" id="td1" class="Label" align="left" visible="True">
                            DefaultValue :
                        </td>
                        <td style="width: 159px; height: 19px" id="td2" align="left" visible="true">
                            <asp:TextBox ID="txtDefaultValue" TabIndex="7" runat="server" CssClass="textBox"
                                __designer:wfdid="w49"></asp:TextBox>
                        </td>
                        <td style="width: 71px; height: 19px" class="Label" align="left" visible="true">
                            Ref. Column :
                        </td>
                        <td style="width: 159px; height: 19px" align="left" visible="true">
                            <asp:DropDownList ID="ddlRefColumn" TabIndex="13" runat="server" CssClass="dropDownList"
                                Width="160px" __designer:wfdid="w2">
                            </asp:DropDownList>
                            <br />
                            <asp:Panel ID="pnlCheckBoxListRefColumn" runat="server" Visible="false" Width="250px"
                                Height="70px" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Auto">
                                <asp:CheckBoxList ID="chkRefColumn" TabIndex="13" runat="server" CssClass="CheckBoxList"
                                    RepeatDirection="Horizontal" RepeatColumns="2">
                                </asp:CheckBoxList>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr id="Tr2" runat="server">
                        <td style="width: 175px; height: 19px" class="Label" align="left" visible="True">
                            UOM :
                        </td>
                        <td style="width: 159px; height: 19px" align="left" visible="true">
                            <asp:DropDownList ID="ddlUOMDesc" runat="server" CssClass="dropDownList" Width="150px"
                                __designer:wfdid="w1">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 71px; height: 19px" class="Label" align="left" visible="true">
                        </td>
                        <td style="width: 159px; height: 19px" align="left" visible="true">
                            <asp:CheckBox ID="chkActive" runat="server" Text="Active" __designer:wfdid="w22"
                                Checked="True"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr runat="server">
                        <td style="width: 175px; height: 19px" class="Label" align="left" visible="True">
                            CDISCValues :
                        </td>
                        <td style="width: 159px; height: 19px" align="left" visible="true">
                            <asp:TextBox ID="txtCDISCValues" TabIndex="10" runat="server" CssClass="textBox"
                                __designer:wfdid="w1"></asp:TextBox>
                        </td>
                        <td style="width: 71px; height: 19px" class="Label" align="left" visible="true">
                            OtherValues :
                        </td>
                        <td style="width: 159px; height: 19px" align="left" visible="true">
                            <asp:TextBox ID="txtOtherValues" TabIndex="10" runat="server" CssClass="textBox"
                                __designer:wfdid="w2"></asp:TextBox>
                        </td>
                    </tr>
                    <tr style="display: none" id="trFormula" runat="server">
                        <td class="Label" align="left">
                            Formula :
                        </td>
                        <td class="Label" align="left">
                            <input style="width: auto" class="button" onclick="FormulaDivShowHide('S');" type="button"
                                value="Create Formula" />
                        </td>
                    </tr>
                    <tr runat="server">
                        <td style="height: 19px; text-align: center" class="Label" align="left" colspan="4"
                            visible="True">
                            &nbsp;
                            <asp:Button ID="BtnSave" TabIndex="12" OnClick="BtnSave_Click" runat="server" Text="Save"
                                CssClass="btn btnsave" __designer:wfdid="w1" OnClientClick="return Validation();">
                            </asp:Button>
                            <asp:Button ID="btnupdate" TabIndex="14" OnClick="btnupdate_Click" runat="server"
                                Text="Update" CssClass="btn btnsave" Visible="False" __designer:wfdid="w2" OnClientClick="return Validation();">
                            </asp:Button><asp:Button ID="btncancel" TabIndex="15" OnClick="btncancel_Click" runat="server"
                                Text="Cancel" CssClass="btn btncancel" __designer:wfdid="w3"></asp:Button><asp:Button ID="BtnExit"
                                    TabIndex="13" OnClick="BtnExit_Click" runat="server" Text="Exit" CssClass="btn btnexit"
                                    __designer:wfdid="w4" OnClientClick="return msgconfirmalert('Are You sure You want to EXIT ?',this)">
                                </asp:Button>
                        </td>
                    </tr>
                    <tr runat="server">
                        <td style="width: 175px; height: 19px" class="Label" align="left" visible="True">
                            Attribute/AttributeGroup :
                        </td>
                        <td style="height: 19px" align="left" colspan="3" visible="true">
                            <asp:TextBox ID="txtMedex" TabIndex="1" runat="server" CssClass="textBox" Width="580px"
                                __designer:dtid="562949953421319" __designer:wfdid="w4"></asp:TextBox>
                            <asp:HiddenField ID="hMedexCode" runat="server" __designer:dtid="562949953421320"
                                __designer:wfdid="w5"></asp:HiddenField>
                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" __designer:dtid="562949953421321"
                                __designer:wfdid="w6" ServiceMethod="GetMedexList" UseContextKey="True" TargetControlID="txtMedex"
                                ServicePath="AutoComplete.asmx" OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected"
                                MinimumPrefixLength="1" CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1">
                            </cc1:AutoCompleteExtender>
                            <asp:Button Style="display: none" ID="btnSetMedex" OnClick="btnSetMedex_Click" runat="server"
                                Text="Medex" __designer:dtid="3659174697238540" __designer:wfdid="w7"></asp:Button>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <div style="display: none; left: 391px; width: 650px; top: 528px; height: 349px;
                                text-align: left" id="divFormula" class="DIVSTYLE2" runat="server">
                                <table style="width: 650px">
                                    <tbody>
                                        <tr align="center">
                                            <td style="text-align: center" align="center" colspan="3">
                                                <strong style="white-space: nowrap">Attribute Formula Master</strong>
                                            </td>
                                        </tr>
                                        <tr align="center">
                                            <td style="text-align: center" align="center" colspan="3">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Label" align="right">
                                                <asp:ListBox ID="lstMedEx" runat="server" CssClass="listbox" Width="254px" __designer:wfdid="w1"
                                                    Height="255px"></asp:ListBox>
                                            </td>
                                            <td class="label" align="center">
                                                <input style="font-weight: bold; width: 59px; height: 47px" class="button" onclick="CopyMedEx();"
                                                    type="button" size="" value=">" />
                                            </td>
                                            <td class="label" align="left">
                                                &nbsp;&nbsp;
                                                <br />
                                                <table style="width: 259px">
                                                    <tbody>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="labelOperator" runat="server" Text="Operator" CssClass="Label"></asp:Label>
                                                            </td>
                                                            <td style="width: 122px">
                                                                <asp:Label ID="lblDigits" runat="server" __designer:wfdid="w1" Text="Digits" CssClass="Label"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="ddlOperator" runat="server" CssClass="label" Width="88px">
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
                                                            <td style="width: 122px">
                                                                <asp:DropDownList ID="ddlNumbers" runat="server" CssClass="label" Width="88px">
                                                                    <asp:ListItem></asp:ListItem>
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
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td style="width: 122px">
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                                <asp:TextBox ID="txtFormula" runat="server" Width="255px" __designer:wfdid="w2" Height="104px"
                                                    TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                                <br />
                                                <input class="button" onclick="ClearFormulaText();" type="button" value="Clear" />
                                                <input class="button" onclick="ClearAllFormulaText();" type="button" value="Clear All" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center" colspan="4">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center" nowrap colspan="4">
                                                <asp:Button ID="btnSaveFormula" OnClick="btnSaveFormula_Click" runat="server" Text="Save"
                                                    CssClass="btn btnsave" __designer:wfdid="w3"></asp:Button>&nbsp;<input class="button"
                                                        onclick="FormulaDivShowHide('H');" type="button" value="Close" /><asp:HiddenField
                                                            ID="HFFormula" runat="server"></asp:HiddenField>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
            <asp:GridView ID="gvmedex" runat="server" SkinID="grdViewAutoSizeMax" __designer:wfdid="w51"
                OnRowDeleting="gvmedex_RowDeleting" OnPageIndexChanging="gvmedex_PageIndexChanging"
                OnRowCommand="gvmedex_RowCommand" OnRowDataBound="gvmedex_RowDataBound" AutoGenerateColumns="False"
                PageSize="25" AllowPaging="True">
                <Columns>
                    <asp:BoundField DataFormatString="number" HeaderText="#">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="vMedExCode" HeaderText="AttributeCode"></asp:BoundField>
                    <asp:BoundField DataField="vMedExDesc" HeaderText="AttributeDesc">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="vMedExGroupDesc" HeaderText="AttributeGroupDesc">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="vMedExSubGroupDesc" HeaderText="AttributeSubGroupDesc">
                    </asp:BoundField>
                    <asp:BoundField DataField="vMedExType" HeaderText="AttributeType"></asp:BoundField>
                    <asp:BoundField DataField="vMedExValues" HeaderText="AttributeValues"></asp:BoundField>
                    <asp:BoundField DataField="vDefaultValue" HeaderText="DefaultValue"></asp:BoundField>
                    <asp:BoundField DataField="vUOM" HeaderText="UOM"></asp:BoundField>
                    <asp:BoundField DataField="vLowRange" HeaderText="LowRange"></asp:BoundField>
                    <asp:BoundField DataField="vHighRange" HeaderText="HighRange"></asp:BoundField>
                    <asp:BoundField DataField="vValidationType" HeaderText="ValidationType"></asp:BoundField>
                    <asp:BoundField DataField="cActiveFlag" HeaderText="ActiveFlag"></asp:BoundField>
                    <asp:BoundField DataField="vRefTable" HeaderText="Ref. Table"></asp:BoundField>
                    <asp:BoundField DataField="vRefColumn" HeaderText="Ref. Column"></asp:BoundField>
                    <asp:TemplateField HeaderText="Edit">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImbEdit" runat="server" ImageUrl="~/images/Edit2.gif" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delete">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImbDelete" runat="server" ImageUrl="~/Images/i_delete.gif" OnClientClick="return msgconfirmalert('Are You sure You want to DELETE?',this)" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            &nbsp;
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
