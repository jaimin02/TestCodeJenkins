<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmParameterList.aspx.vb" Inherits="frmParameterList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="javascript" src="Script/Validation.js">
    </script>
    <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

        #ctl00_CPHLAMBDA_dgDept_wrapper {
            margin: 0px 235px;
        }
    </style>
    <script type="text/javascript">
        Sys.Browser.WebKit = {};
        if (navigator.userAgent.indexOf('WebKit/') > -1) {
            Sys.Browser.agent = Sys.Browser.WebKit;

            Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
            Sys.Browser.name = 'WebKit';
        }

    </script>
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <script type="text/javascript" language="javascript">


        function HideTempTypeDetails() {
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

        //function Display(control, target) {
        //    if (control.src.toString().toUpperCase().search("EXPAND") != -1) {
        //        $("#" + target).slideToggle(600);
        //        control.src = "images/panelcollapse.png";
        //    }
        //    else {
        //        $("#" + target).slideToggle(600);
        //        control.src = "images/panelexpand.png";
        //    }
        //}

        function Validation() {
            if (document.getElementById('<%=txtName.ClientID%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=txtName.ClientID%>').value = ''
                msgalert('Please Enter  Name !');
                document.getElementById('<%=txtName.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtType.ClientID%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=txtType.ClientID%>').value = ''
                msgalert('Please Enter Type !');
                document.getElementById('<%=txtType.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtValue.ClientID%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=txtValue.ClientID%>').value = ''
                msgalert('Please Enter Value !');
                document.getElementById('<%=txtValue.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtRelatedProcess.ClientID%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=txtRelatedProcess.ClientID%>').value = ''
                msgalert('Please Enter Related Process !');
                document.getElementById('<%=txtRelatedProcess.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtRemark.ClientID%>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Remarks !');
                document.getElementById('<%=txtRemark.ClientID%>').focus();
                return false;
            }
    if (document.getElementById("<%=btnSave.ClientID%>").value.trim() == "Update") {

                if (document.getElementById("<%=txtRemark.ClientID%>").value.trim() == "") {
            msgalert("Please Enter Remarks !");
            return false;
        }
    }
    return true;
}
function Validationadd() {
    if (document.getElementById('<%=txtDeptValue.ClientID%>').value.toString().trim().length <= 0) {
        document.getElementById('<%=txtDeptValue.ClientID%>').value = ''
        msgalert('Please Enter Dept Value !');
        document.getElementById('<%=txtDeptValue.ClientID%>').focus();
        return false;
    }
    else if (document.getElementById('<%=ddlDept.ClientID%>').selectedIndex == 0) {
        msgalert('Please Select Department !');
        return false;
    }
    return true;
}
function UIdgParameter() {
    $('#<%= dgParameter.ClientID%>').removeAttr('style', 'display:block');
    oTab = $('#<%= dgParameter.ClientID%>').prepend($('<thead>').append($('#<%= dgParameter.ClientID%> tr:first'))).dataTable({
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
function UIdgDept() {
    $('#<%= dgDept.ClientID%>').removeAttr('style', 'display:block');
    oTab = $('#<%= dgDept.ClientID%>').prepend($('<thead>').append($('#<%= dgDept.ClientID%> tr:first'))).dataTable({
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
    </script>

    <asp:UpdatePanel ID="UP_PARMA_GRID" runat="server" ChildrenAsTriggers="False" RenderMode="Inline"
        UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlParamerter" runat="server">
                <table style="width: 100%;" cellpadding="5px">
                    <tbody>
                        <tr>
                            <td>
                                <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                                    <legend class="LegendText" style="color: Black; font-size: 12px">
                                        <img id="img1" alt="Parameter Data" src="images/panelcollapse.png"
                                            onclick="Display(this,'divParameterData');" runat="server" style="margin-right: 2px;" />Parameter Data</legend>
                                    <div id="divParameterData">
                                        <table style="margin: auto; width: 80%;">
                                            <tr>
                                                <td align="center">
                                                    <asp:GridView ID="dgParameter" Style="vertical-align: middle; cursor: pointer" runat="server"
                                                        OnRowEditing="dgParameter_RowEditing" AutoGenerateColumns="False">
                                                        <Columns>
                                                            <asp:BoundField DataField="ParameterNo" HeaderText="Parameter No.">
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ParameterName" HeaderText="Parameter Name" />
                                                            <asp:BoundField DataField="ParameterDesc" HeaderText="Parameter Description" />
                                                            <asp:BoundField DataField="ParameterDbType" HeaderText="Parameter Type" />
                                                            <asp:BoundField DataField="IsCorporateLevelParameter" HeaderText="IsCorporateLevel Parameter" />
                                                            <asp:TemplateField HeaderText="Edit">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="~/images/Edit2.gif" ToolTip="Edit"
                                                                        CommandName="Edit" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>

                                                    <asp:Button ID="brnExitMain" OnClick="brnExitMain_Click" runat="server" Text="Exit"
                                                        ToolTip="Exit" CssClass="btn btnexit" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </fieldset>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="dgParameter" EventName="RowCommand" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UP_PARMA_DATA" runat="server" ChildrenAsTriggers="False" RenderMode="Inline"
        UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlParamerterDATA" runat="server">
                <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                    <legend class="LegendText" style="color: Black; font-size: 12px">
                        <img id="img2" alt="Template Type Details" src="images/panelcollapse.png"
                            onclick="Display(this,'divParameterMaster');" runat="server" style="margin-right: 2px;" />Parameter Master</legend>
                    <div id="divParameterMaster">
                <table style="width: 80%; margin: auto" cellpadding="5px">
                    <tbody>
                        <tr>
                            <td class="Label" style="width: 25%; text-align: right;">Name* :
                            </td>
                            <td style="width: 20%; text-align: left;">
                                <asp:TextBox ID="txtName" runat="server" CssClass="textBox" ReadOnly="True" Width="100%" />
                            </td>
                            <td class="Label" style="width: 20%; text-align: right;">Type* :
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtType" runat="server" CssClass="textBox" ReadOnly="True" Width="55%" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="Label">Value* :
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtValue" runat="server" CssClass="textBox" Width="100%" />
                                <asp:DropDownList ID="ddlvaluemain" runat="server" CssClass="dropDownList" Width="100%">
                                    <asp:ListItem Value="1">Y</asp:ListItem>
                                    <asp:ListItem Value="0">N</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: right;" class="Label">Related Process* :
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtRelatedProcess" runat="server" CssClass="textBox" Width="55%" />
                            </td>

                        </tr>
                        <tr>
                            <td style="text-align: right;" class="Label">Description :
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="textBox" ReadOnly="True"
                                    TextMode="MultiLine" Width="100%" Height="50px" />
                            </td>
                            <td style="text-align: right;" class="Label">Remarks* :
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtRemark" runat="server" Rows="3" Columns="20" CssClass="textBox" Style="width: 55%; height: auto;" TextMode="MultiLine" MaxLength="500" />
                            </td>
                        </tr>
                        <tr>
                            <td class="Label" align="center" colspan="2">
                                <asp:CheckBox ID="chkFlag" runat="server" Text="Active" SkinID="chkDisplay" />
                            </td>
                        </tr>
                    </tbody>
                </table>
                        </div>
                    </fieldset>

                
                    <asp:Panel ID="PanelPlanned1" runat="server">
                        <%--<div style="vertical-align: middle; width: 100%; cursor: pointer; margin: auto;">
                            <span style="font-size: 12px; float: inherit; vertical-align: middle">
                                <asp:Label ID="Label5" runat="server" ForeColor="White" Font-Bold="True" Text="D E P A R T M E N T -  D E T A I L S" />
                            </span><span>
                                <asp:Image ID="Image2" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/panelexpand.png" />
                            </span>
                        </div>--%>
                    </asp:Panel>
                    <%--<cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" CollapseControlID="PanelPlanned1"
                        Collapsed="false" CollapsedImage="~/images/panelexpand.png" ExpandControlID="PanelPlanned1"
                        ExpandedImage="~/images/panelcollapse.png" ImageControlID="Image2" SuppressPostBack="true"
                        TargetControlID="PanDept">
                    </cc1:CollapsiblePanelExtender>--%>
                                <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                                <legend class="LegendText" style="color: Black; font-size: 12px">
                                    <img id="img3" alt="Template Type Data" src="images/panelcollapse.png"
                                        onclick="Display(this,'divDEPARTMENTDETAILS');" runat="server" style="margin-right: 2px;" />DEPARTMENTDETAILS</legend>
                                <div id="divDEPARTMENTDETAILS">
                <div style="width: 100%; margin: 0 auto">
                    <asp:Panel ID="PanDept" runat="server">
                        <table style="width: 80%; margin: auto;" id="TABLE1" cellpadding="5px">
                            <tbody>
                                <tr>
                                    <td style="text-align: right; width: 25%" class="Label">Dept Name* :
                                    </td>
                                    <td style="text-align: left; width: 20%">
                                        <asp:DropDownList ID="ddlDept" runat="server" CssClass="dropDownList" Width="100%" />
                                    </td>
                                    <td class="Label" style="width: 20%; text-align: right;">Value* :
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtDeptValue" runat="server" CssClass="textBox" Width="55%" />
                                                <asp:DropDownList ID="ddlboolean" runat="server" CssClass="dropDownList" Width="55%">
                                                    <asp:ListItem Value="1">Y</asp:ListItem>
                                                    <asp:ListItem Value="0">N</asp:ListItem>
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        <asp:Button ID="btnAdd" OnClick="btnAdd_Click" runat="server" Text="ADD" ToolTip="Add"
                                            CssClass="btn btnnew" OnClientClick="return Validationadd();" />
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <div style="width: 80%; margin: 0 auto">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" RenderMode="Inline"
                                            ChildrenAsTriggers="False">
                                            <ContentTemplate>
                                                <asp:GridView Style="cursor: pointer" ID="dgDept" runat="server"
                                                    AutoGenerateColumns="false" OnRowEditing="dgDept_RowEditing"
                                                    OnRowUpdating="dgDept_RowUpdating" OnRowDeleting="dgDept_RowDeleting1">
                                                    <Columns>
                                                        <asp:BoundField DataField="nDeptNo" Visible="False" HeaderText="Dept No" />
                                                        <asp:BoundField DataField="DeptName" HeaderText="Dept Name" />
                                                        <asp:BoundField DataField="vParameterValue" HeaderText="Parameter Value" />
                                                        <asp:TemplateField HeaderText="Delete">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/images/i_delete.gif" CommandName="Delete"
                                                                    Headertext="Action" OnClientClick="return msgconfirmalert('Are you sure you want to Delete?',this);"
                                                                    ToolTip="Delete" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="dgDept" EventName="RowDeleting" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </tr>
                            </tbody>
                        </table>
                    </asp:Panel>
                </div>
                    </div>
                    </fieldset>
                <table style="width: 80%; margin: auto;">
                    <tbody>
                        <tr>
                            <td style="text-align: center" colspan="4">
                                <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Save" ToolTip="Save"
                                    CssClass="btn btnsave" OnClientClick="return  Validation();" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="btn btncancel" />
                                <asp:Button ID="btnExit" runat="server" Text="Exit" ToolTip="Exit" CssClass="btn btnexit"
                                    OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="dgParameter" EventName="RowCommand" />
            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
