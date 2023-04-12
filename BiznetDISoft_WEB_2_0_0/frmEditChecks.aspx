<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmEditChecks.aspx.vb" Inherits="frmEditChecks" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <asp:UpdatePanel ID="UpTree" runat="server" RenderMode="Inline" UpdateMode="Conditional"
        ChildrenAsTriggers="true">
        <ContentTemplate>
            <table border="0" cellpadding="0" cellspacing="0" width="99%">
                <tr>
                    <td class="Label" nowrap="nowrap" style="text-align: right; width: 30%;">
                        Project Name/Request Id: &nbsp;
                    </td>
                    <td class="Label" style="text-align: left; width: 70%;">
                        <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="550px" TabIndex="1"></asp:TextBox><asp:Button
                            Style="display: none" ID="btnSetProject" runat="server" Text=" Project"></asp:Button>
                        <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                            CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                            CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                            OnClientShowing="ClientPopulated" ServiceMethod="GetMyProjectCompletionList"
                            ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True"
                            CompletionListElementID="pnlProjectList">
                        </cc1:AutoCompleteExtender>
                        <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto;
                            overflow-x: hidden" />
                    </td>
                </tr>
                <tr style="display: none" runat="server" id="trPeriod">
                    <td class="Label" nowrap="nowrap" style="text-align: right; width: 30%;">
                        Period: &nbsp;
                    </td>
                    <td align="left" style="text-align: left; width: 70%;">
                        <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="dropDownList" Width="192px"
                            AutoPostBack="True" TabIndex="2">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr style="display: none" runat="server" id="trActivity">
                    <td class="Label" nowrap="nowrap" style="text-align: right; width: 30%">
                        Select Activity: &nbsp;
                    </td>
                    <td class="Label" nowrap="nowrap" style="text-align: left; width: 70%">
                        <asp:DropDownList ID="ddlActivity" TabIndex="3" runat="server" CssClass="dropDownList"
                            Width="580px" AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr style="padding: 2px">
                    <td align="left" class="Label" valign="top" width="16%">
                        Select Attribute
                    </td>
                    <td align="left" class="Label" valign="top" width="42%">
                        Specify Condition
                    </td>
                </tr>
                <tr style="padding: 2px">
                    <td align="left" class="Label" valign="top" width="30%">
                        <div style="border-right: gray thin solid; border-top: gray thin solid; overflow-y: auto;
                            border-left: gray thin solid; width: auto; border-bottom: gray thin solid; height: 350px"
                            id="Div1" align="left">
                            <asp:TreeView ID="trvwStructure" runat="server" ForeColor="black" CssClass="TreeView"
                                Width="100%" ExpandDepth="0" ShowLines="True" ShowCheckBoxes="Leaf" NodeWrap="True"
                                OnTreeNodeCheckChanged="trvwStructure_TreeNodeCheckChanged" TabIndex="4">
                            </asp:TreeView>
                        </div>
                    </td>
                    <td align="left" class="Label" valign="top" width="70%">
                        <table style="text-align: left; width: 100%;">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpMedEx" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table style="width: 100%">
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkIf" runat="server" Text="If" Enabled="False" Checked="True">
                                                            </asp:CheckBox>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkThen" runat="server" Text="And" TabIndex="11"></asp:CheckBox>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Source Attribute:*
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSourceMedEx" runat="server" TextMode="MultiLine" Enabled="false"
                                                                TabIndex="6"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            Source&nbsp;Attribute:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtThenSourceMedEx" runat="server" TextMode="MultiLine" Enabled="false"
                                                                TabIndex="12"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Operator:*
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlOperator" runat="server" CssClass="dropdownlist" Width="88px">
                                                                <asp:ListItem Text="&gt;"></asp:ListItem>
                                                                <asp:ListItem Text="&lt;"></asp:ListItem>
                                                                <asp:ListItem Text="&gt;="></asp:ListItem>
                                                                <asp:ListItem Text="&lt;="></asp:ListItem>
                                                                <asp:ListItem Text="="></asp:ListItem>
                                                                <asp:ListItem Text="&lt;&gt;"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            Operator:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlThenOperator" runat="server" CssClass="dropdownlist" Width="88px"
                                                                TabIndex="13">
                                                                <asp:ListItem Text="&gt;"></asp:ListItem>
                                                                <asp:ListItem Text="&lt;"></asp:ListItem>
                                                                <asp:ListItem Text="&gt;="></asp:ListItem>
                                                                <asp:ListItem Text="&lt;="></asp:ListItem>
                                                                <asp:ListItem Text="="></asp:ListItem>
                                                                <asp:ListItem Text="&lt;&gt;"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td rowspan="2">
                                                            Target Value:*
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="UPMedExValuesIf" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Panel ID="pnlMedExAttributeValues_If" runat="server" Width="170px" ScrollBars="Auto">
                                                                        <asp:RadioButtonList ID="rbtnlstMedExValues_If" runat="server" RepeatColumns="1"
                                                                            TabIndex="7">
                                                                        </asp:RadioButtonList>
                                                                    </asp:Panel>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td>
                                                            &nbsp;&nbsp;
                                                        </td>
                                                        <td rowspan="2">
                                                            Target Value:
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="UPMedExValuesThen" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Panel ID="pnlMedExAttributeValues_Then" runat="server" Width="170px" ScrollBars="Auto">
                                                                        <asp:RadioButtonList ID="rbtnlstMedExValues_Then" runat="server" RepeatColumns="1"
                                                                            TabIndex="14">
                                                                        </asp:RadioButtonList>
                                                                    </asp:Panel>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtTargetValue" runat="server" TabIndex="8"></asp:TextBox>
                                                            <br />
                                                            <asp:CheckBox ID="chkNull" runat="server" OnClick="return ChkUnchkNull();" />
                                                            NULL
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtThenTargetValue" runat="server" TabIndex="15"></asp:TextBox>
                                                            <br />
                                                            <asp:CheckBox runat="server" ID="chkThenNull" Text="NULL" OnClick="return ChkUnchkThenNull();" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Target Attribute:*
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtTargetMedEx" runat="server" TextMode="MultiLine" Enabled="false"
                                                                TabIndex="9"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            Target Attribute:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtThenTargetMedEx" runat="server" TextMode="MultiLine" Enabled="false"
                                                                TabIndex="16"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Message:*
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:TextBox ID="txtRemarks" runat="server" Width="490px" TextMode="MultiLine" TabIndex="10"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: center" colspan="5">
                                                            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btnnew" OnClientClick="return ValidationToAddEditCheck();"
                                                                TabIndex="17"></asp:Button>
                                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btncancel" __designer:wfdid="w7"
                                                                TabIndex="18"></asp:Button>
                                                            <asp:Button ID="btnexit" runat="server" Text="Exit" CssClass="btn btnexit" __designer:dtid="281474976710686"
                                                                OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" TabIndex="19">
                                                            </asp:Button>
                                                        </td>
                                                    </tr>
                                                    <tr style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 10px">
                                                        <td style="text-align: left" valign="middle" align="center" width="100%" colspan="6">
                                                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btnsave" Visible="False"
                                                                TabIndex="19"></asp:Button>
                                                        </td>
                                                    </tr>
                                                    <tr style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 10px">
                                                        <td valign="middle" align="center" width="100%" colspan="6">
                                                            <asp:HiddenField ID="HFSourceMedExCode" runat="server"></asp:HiddenField>
                                                            <asp:HiddenField ID="HFTargetMedExCode" runat="server"></asp:HiddenField>
                                                            <asp:HiddenField ID="HFThenSourceMedExCode" runat="server"></asp:HiddenField>
                                                            <asp:HiddenField ID="HFThenTargetMedExCode" runat="server"></asp:HiddenField>
                                                            <asp:HiddenField ID="HFSourceNodeId" runat="server"></asp:HiddenField>
                                                            <asp:HiddenField ID="HFTargetNodeId" runat="server"></asp:HiddenField>
                                                            <asp:HiddenField ID="HFThenSourceNodeId" runat="server"></asp:HiddenField>
                                                            <asp:HiddenField ID="HFThenTargetNodeId" runat="server"></asp:HiddenField>
                                                            <asp:HiddenField ID="HFSourceParentNodeId" runat="server"></asp:HiddenField>
                                                            <asp:HiddenField ID="HFTargetParentNodeId" runat="server"></asp:HiddenField>
                                                            <asp:HiddenField ID="HFThenSourceParentNodeId" runat="server"></asp:HiddenField>
                                                            <asp:HiddenField ID="HFThenTargetParentNodeId" runat="server"></asp:HiddenField>
                                                            <asp:HiddenField ID="HFParentNodeId" runat="server"></asp:HiddenField>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click"></asp:AsyncPostBackTrigger>
                                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click"></asp:AsyncPostBackTrigger>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UPGrid" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlGrid" runat="server" Width="100%" Height="25%">
                                                <asp:GridView ID="GVEditChecks" runat="server" SkinID="grdViewAutoSizeMax" AutoGenerateColumns="False"
                                                    OnRowCreated="GVEditChecks_RowCreated" OnRowDataBound="GVEditChecks_RowDataBound"
                                                    OnRowCommand="GVEditChecks_RowCommand" OnRowDeleting="GVEditChecks_RowDeleting">
                                                    <Columns>
                                                        <asp:BoundField DataFormatString="number" HeaderText="#">
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="nMedExEditCheckNo" HeaderText="nMedExEditCheckNo" ReadOnly="True">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vWorkspaceId" HeaderText="vWorkspaceId" ReadOnly="True">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="iParentNodeId" HeaderText="iParentNodeId" ReadOnly="True">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="iSourceNodeId_If" HeaderText="iSourceNodeId_If" ReadOnly="True">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vSourceMedExCode_If" HeaderText="vSourceMedExCode_If"
                                                            ReadOnly="True"></asp:BoundField>
                                                        <asp:BoundField DataField="vOperator_If" HeaderText="vOperator_If" ReadOnly="True">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="iTargetNodeId_If" HeaderText="iTargetNodeId_If" ReadOnly="True">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vTargetMedExCode_If" HeaderText="vTargetMedExCode_If"
                                                            ReadOnly="True"></asp:BoundField>
                                                        <asp:BoundField DataField="vTargetValue_If" HeaderText="vTargetValue_If" ReadOnly="True">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="iSourceNodeId_Then" HeaderText="iSourceNodeId_Then" ReadOnly="True">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vSourceMedExCode_Then" HeaderText="vSourceMedExCode_Then"
                                                            ReadOnly="True"></asp:BoundField>
                                                        <asp:BoundField DataField="vOperator_Then" HeaderText="vOperator_Then" ReadOnly="True">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="iTargetNodeId_Then" HeaderText="iTargetNodeId_Then" ReadOnly="True">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vTargetMedExCode_Then" HeaderText="vTargetMedExCode_Then"
                                                            ReadOnly="True"></asp:BoundField>
                                                        <asp:BoundField DataField="vTargetValue_Then" HeaderText="vTargetValue_Then" ReadOnly="True">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vCondition" HeaderText="Condition" ReadOnly="True"></asp:BoundField>
                                                        <asp:BoundField DataField="vRemarks" HeaderText="Remarks" ReadOnly="True"></asp:BoundField>
                                                        <asp:TemplateField HeaderText="Edit">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="~/images/Edit2.gif"></asp:ImageButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Delete">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgBtnDelete" runat="server" ImageUrl="~/Images/i_delete.gif">
                                                                </asp:ImageButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="cStatusIndi" HeaderText="Status" ReadOnly="True"></asp:BoundField>
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="trvwStructure" EventName="TreeNodeCheckChanged">
                                            </asp:AsyncPostBackTrigger>
                                            <asp:AsyncPostBackTrigger ControlID="trvwStructure" EventName="SelectedNodeChanged">
                                            </asp:AsyncPostBackTrigger>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="ddlActivity" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlActivity" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>

    <script type="text/javascript" language="javascript">

        var count = 0;
        var SelectedAttribute = '';

        function NodeChecked()
        {
            var inputsInTree = document.getElementById('<%= trvwStructure.ClientId %>').getElementsByTagName("input");
            var StrMedExValues = '';
            var MedExCode = '';
            var SourceNodeId = '';
            var NodeId = '';
            var ParentNodeId = '';
            var Value = '';
            var CheckThen = document.getElementById('<%= chkThen.ClientId %>');
            var chk = document.getElementById("<%= chkNull.ClientId %>");

            for (var i = 0; i < inputsInTree.length; i++)
            {
                var currentElement = inputsInTree[i];
                if (currentElement.nextSibling.canHaveChildren)
                {
                    var childs = currentElement.nextSibling.childNodes;
                    for (var j = 0; j < childs.length; j++)
                    {
                        var currentChildElement = childs[j];
                        if (currentChildElement != null && typeof (currentChildElement) != 'undefined')
                        {
                            if (currentElement.type != null && typeof (currentElement.type) != 'undefined')
                            {
                                if (currentElement.type == "checkbox" && currentElement.checked)
                                {

//                                    if (SelectedAttribute == currentElement.parentNode.innerText)
//                                    {
//                                        continue;
//                                    }
//                                    if (count == 1)
//                                    {
//                                        return false;
//                                    }
//                                    if (count > 1)
//                                    {
//                                        alert('Please Select Only One Node');
//                                        currentElement.checked = false;
//                                        count -= 1;
//                                        return false;
//                                    }

                                    if (document.getElementById('<%= txtSourceMedEx.ClientId %>').value.trim() == '')
                                    {
                                        count += 1;
                                        SelectedAttribute = currentElement.parentNode.innerText;
                                        Value = currentChildElement.parentNode.href;
                                        StrMedExValues = Value.substr(Value.indexOf('V-') + 2, (Value.length - (Value.substr(0, Value.indexOf('V-') + 2).length + 2)));
                                        MedExCode = Value.substr(Value.indexOf('M-') + 2, (Value.indexOf('N-') - (Value.indexOf('M-') + 2)));
                                        NodeId = Value.substr(Value.indexOf('N-') + 2, (Value.indexOf('P-') - (Value.indexOf('N-') + 2)));
                                        ParentNodeId = Value.substr(Value.indexOf('P-') + 2, (Value.indexOf('V-') - (Value.indexOf('P-') + 2)));

                                        document.getElementById('<%= HFSourceMedExCode.ClientId %>').value = MedExCode;
                                        document.getElementById('<%= HFSourceNodeId.ClientId %>').value = NodeId;
                                        document.getElementById('<%= HFSourceParentNodeId.ClientId %>').value = ParentNodeId;
                                        document.getElementById('<%= txtSourceMedEx.ClientId %>').value = currentChildElement.data;
                                        if (StrMedExValues != '')
                                        {
                                            CreateRadioButtonList(StrMedExValues, 'SOURCE');
                                        }
                                        currentElement.checked = false;
                                        return true;
                                    }
                                    else if (document.getElementById('<%= txtTargetMedEx.ClientId %>').value.trim() == '' && document.getElementById('<%= txtTargetValue.ClientId %>').value.trim() == '')
                                    {
                                        count += 1;
                                        SelectedAttribute = currentElement.parentNode.innerText;
                                        Value = currentChildElement.parentNode.href;
                                        StrMedExValues = Value.substr(Value.indexOf('V-') + 2, (Value.length - (Value.substr(0, Value.indexOf('V-') + 2).length + 2)));
                                        MedExCode = Value.substr(Value.indexOf('M-') + 2, (Value.indexOf('N-') - (Value.indexOf('M-') + 2)));
                                        NodeId = Value.substr(Value.indexOf('N-') + 2, (Value.indexOf('P-') - (Value.indexOf('N-') + 2)));
                                        ParentNodeId = Value.substr(Value.indexOf('P-') + 2, (Value.indexOf('V-') - (Value.indexOf('P-') + 2)));

                                        document.getElementById('<%= HFTargetMedExCode.ClientId %>').value = MedExCode;
                                        document.getElementById('<%= HFTargetNodeId.ClientId %>').value = NodeId;
                                        document.getElementById('<%= HFTargetParentNodeId.ClientId %>').value = ParentNodeId;
                                        document.getElementById('<%= txtTargetMedEx.ClientId %>').value = currentChildElement.data;
                                        chk.disabled = true;
                                        document.getElementById("<%=txtTargetValue.ClientId %>").disabled = true;
                                        document.getElementById("<%=pnlMedExAttributeValues_If.ClientId %>").disabled = true;
                                        currentElement.checked = false;
                                        return true;
                                    }

//                                    else if (CheckThen.checked == true)
//                                    {
                                    else if (document.getElementById('<%= txtThenSourceMedEx.ClientId %>').value.trim() == '')
                                    {
                                        count += 1;
                                        SelectedAttribute = currentElement.parentNode.innerText;
                                        Value = currentChildElement.parentNode.href;
                                        StrMedExValues = Value.substr(Value.indexOf('V-') + 2, (Value.length - (Value.substr(0, Value.indexOf('V-') + 2).length + 2)));
                                        MedExCode = Value.substr(Value.indexOf('M-') + 2, (Value.indexOf('N-') - (Value.indexOf('M-') + 2)));
                                        NodeId = Value.substr(Value.indexOf('N-') + 2, (Value.indexOf('P-') - (Value.indexOf('N-') + 2)));
                                        ParentNodeId = Value.substr(Value.indexOf('P-') + 2, (Value.indexOf('V-') - (Value.indexOf('P-') + 2)));

                                        document.getElementById('<%= HFThenSourceMedExCode.ClientId %>').value = MedExCode;
                                        document.getElementById('<%= HFThenSourceNodeId.ClientId %>').value = NodeId;
                                        document.getElementById('<%= HFThenSourceParentNodeId.ClientId %>').value = ParentNodeId;
                                        document.getElementById('<%= txtThenSourceMedEx.ClientId %>').value = currentChildElement.data;
                                        if (StrMedExValues != '')
                                        {
                                            CreateRadioButtonList(StrMedExValues, 'TARGET');
                                        }
                                        CheckThen.checked = true;
                                        currentElement.checked = false;
                                        return true;
                                    }
                                    else if (document.getElementById('<%= txtThenTargetMedEx.ClientId %>').value.trim() == '' && document.getElementById('<%= txtThenTargetValue.ClientId %>').value.trim() == '')
                                    {
                                        count += 1;
                                        SelectedAttribute = currentElement.parentNode.innerText;
                                        Value = currentChildElement.parentNode.href;
                                        StrMedExValues = Value.substr(Value.indexOf('V-') + 2, (Value.length - (Value.substr(0, Value.indexOf('V-') + 2).length + 2)));
                                        MedExCode = Value.substr(Value.indexOf('M-') + 2, (Value.indexOf('N-') - (Value.indexOf('M-') + 2)));
                                        NodeId = Value.substr(Value.indexOf('N-') + 2, (Value.indexOf('P-') - (Value.indexOf('N-') + 2)));
                                        ParentNodeId = Value.substr(Value.indexOf('P-') + 2, (Value.indexOf('V-') - (Value.indexOf('P-') + 2)));

                                        document.getElementById('<%= HFThenTargetMedExCode.ClientId %>').value = MedExCode;
                                        document.getElementById('<%= HFThenTargetNodeId.ClientId %>').value = NodeId;
                                        document.getElementById('<%= HFThenTargetParentNodeId.ClientId %>').value = ParentNodeId;
                                        document.getElementById('<%= txtThenTargetMedEx.ClientId %>').value = currentChildElement.data;
                                        currentElement.checked = false;
                                        CheckThen.disabled = true;
                                        document.getElementById("<%=txtThenTargetValue.ClientId %>").disabled = true;
                                        document.getElementById("<%=pnlMedExAttributeValues_Then.ClientId %>").disabled = true;
                                        return true;
                                    }
//                                    }
//                                    else
//                                    {
//                                        alert('Please Select " And " Option');
//                                        return false;
//                                    }
                                }
//                                else if (currentElement.type == "checkbox" && currentElement.checked == false)
//                                {
//                                    if (count > 0 && currentElement.parentNode.innerText == SelectedAttribute)
//                                    {
//                                        count -= 1;
//                                        SelectedAttribute = '';
//                                        return true;
//                                    }
//                                    else if (count > 0)
//                                    {
//                                        currentElement.checked = false;
//                                        //alert('Please Select Only One Node');
//                                        continue;
//                                    }
//                                }
                            }
                        }
                    }
                }
            }
            if (count == 0 || count == 1)
            {
                return true;
            }
//            return false;
        }

        var index = 0;
        function CreateRadioButtonList(Values, Type)
        {
            var container;
            if (Type == 'SOURCE')
                container = document.getElementById('<%= pnlMedExAttributeValues_If.ClientId %>');
            else
                container = document.getElementById('<%= pnlMedExAttributeValues_Then.ClientId %>');

            var words;
            words = Values.split(',');
            for (var i = 0; i < words.length; i++)
            {
                index = index + 1;
                container.appendChild(createRadio(index, words[i], Type));
                var label = document.createElement('<label for="' + index + '">');
                label.innerHTML = words[i];
                container.appendChild(label);
                container.appendChild(document.createElement('<br/>'));
            }
        }

        function createRadio(id, txt, Type)
        {
            var radio;
            if (/MSIE (\d+\.\d+);/.test(navigator.userAgent))
            {
                ieversion = new Number(RegExp.$1);
            }
            if (ieversion == 7 || ieversion == 6)
            {
                radio = document.createElement('<input type="radio" name="' + Type + '" onclick="SetValue(this,\'' + Type + '\')">');
            }
            else
            {
                radio = document.createElement('input');
                radio.name = Type;
                radio.type = 'radio';
                radio.setAttribute('onClick', 'SetValue(this,"' + Type + '");');
            }
            radio.id = id;
            radio.value = txt;

            return radio;
        }

        function SetValue(rd, Type)
        {
            if (Type == 'SOURCE')
            {
                document.getElementById('<%= txtTargetValue.ClientId %>').value = rd.value;
            }
            else
            {
                document.getElementById('<%= txtThenTargetValue.ClientId %>').value = rd.value;
            }
        }

        function ClientPopulated(sender, e)
        {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e)
        {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        function ValidationToAddEditCheck()
        {
            if (document.getElementById('<%= HFSourceMedExCode.ClientId %>').value.trim() == '')
            {
                msgalert('Please Select Source Attribute !');
                document.getElementById('<%= txtSourceMedEx.ClientId %>').value = '';
                return false;
            }
            else if (document.getElementById('<%= HFTargetMedExCode.ClientId %>').value.trim() == '' && document.getElementById('<%= txtTargetValue.ClientId %>').value.trim() == '')
            {
                msgalert('Please Select Target Attribute Or Target Value !');
                document.getElementById('<%= txtTargetMedEx.ClientId %>').value = '';
                document.getElementById('<%= txtTargetValue.ClientId %>').value = '';
                document.getElementById('<%= txtTargetValue.ClientId %>').focus();
                return false;
            }
//            else if (document.getElementById('<%= HFThenSourceMedExCode.ClientId %>').value.trim() == '')
//            {
//                alert('Please Select Source Attribute For And Condition');
//                document.getElementById('<%= txtThenSourceMedEx.ClientId %>').value = '';
//                document.getElementById('<%= txtThenSourceMedEx.ClientId %>').focus();
//                return false;
//            }
//            else if (document.getElementById('<%= HFThenTargetMedExCode.ClientId %>').value.trim() == '' && document.getElementById('<%= txtThenTargetValue.ClientId %>').value.trim() == '')
//            {
//                alert('Please Select Target Attribute Or Taget Value For And Condition');
//                document.getElementById('<%= txtThenTargetMedEx.ClientId %>').value = '';
//                document.getElementById('<%= txtThenTargetValue.ClientId %>').value = '';
//                document.getElementById('<%= txtThenTargetValue.ClientId %>').focus();
//                return false;
//            }
            else if (document.getElementById('<%= txtRemarks.ClientId %>').value.trim() == '')
            {
                msgalert('Please Enter Remarks !');
                document.getElementById('<%= txtRemarks.ClientId %>').focus();
                return false;
            }
            return true;
        }
        function ChkUnchkNull()
        {
            if (document.getElementById("<%= txtTargetMedEx.ClientId %>").value.length <= 0)
            {
                var chk = document.getElementById("<%= chkNull.ClientId %>");
                if(chk.checked == true)
                {
                    document.getElementById("<%= txtTargetValue.ClientId %>").value = "NULL";
                    document.getElementById("<%= txtTargetValue.ClientId %>").disabled = true;
                    document.getElementById("<%= pnlMedExAttributeValues_If.ClientId %>").disabled = true;
                }
                else
                {
                    document.getElementById("<%= txtTargetValue.ClientId %>").value = "";
                    document.getElementById("<%= txtTargetValue.ClientId %>").disabled = false;
                    document.getElementById("<%= pnlMedExAttributeValues_If.ClientId %>").disabled = false;
                }
                return true;
            }
            return false;
        }
        function ChkUnchkThenNull()
        {
            if (document.getElementById("<%= txtThenTargetMedEx.ClientId %>").value.length <= 0)
            {
                var chk = document.getElementById("<%= chkThenNull.ClientId %>");
                if(chk.checked == true)
                {
                    document.getElementById("<%= txtThenTargetValue.ClientId %>").value = "NULL";
                    document.getElementById("<%= txtThenTargetValue.ClientId %>").disabled = true;
                    document.getElementById("<%= pnlMedExAttributeValues_Then.ClientId %>").disabled = true;
                }
                else
                {
                    document.getElementById("<%= txtThenTargetValue.ClientId %>").value = "";
                    document.getElementById("<%= txtThenTargetValue.ClientId %>").disabled = false;
                    document.getElementById("<%= pnlMedExAttributeValues_Then.ClientId %>").disabled = false;
                }
                return true;
            }
            return false;
        }
        

    </script>

</asp:Content>
