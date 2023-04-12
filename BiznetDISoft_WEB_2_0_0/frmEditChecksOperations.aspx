<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmEditChecksOperations.aspx.vb" Inherits="frmEditChecksOperations" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" src="Script/jquery.min.js"></script>

    <script type="text/javascript" language="javascript">

        //        function ValidateMaxSubjects()
        //        {
        //           var Count = $('#ctl00_CPHLAMBDA_ChklSubjects input[type="checkbox"]:checked').length
        //           if (Count > 5)
        //           {
        //                alert("Maximum 5 Subjects are allowed.");
        //                return false;
        //           }
        //           return true;
        //        }

        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        function Validation() {
            if (document.getElementById('<%= txtProject.clientid%>').value.trim() == '') {
                msgalert('Please Enter Project !');
                document.getElementById('<%= txtProject.clientid%>').value = '';
                document.getElementById('<%= txtProject.clientid%>').focus();
                return false;
            }
            return true;
        }

        function ValidationForGenerateQuery() {
            if (document.getElementById('<%= txtGenerateQueryRemarks.clientid%>').value.trim() == '') {
                msgalert('Please Enter Remarks !');
                document.getElementById('<%= txtGenerateQueryRemarks.clientid%>').value = '';
                document.getElementById('<%= txtGenerateQueryRemarks.clientid%>').focus();
                return false;
            }
            return true;
        }

        function ValidationForResolveQuery() {
            if (document.getElementById('<%= txtResolveQueryRemarks.clientid%>').value.trim() == '') {
                msgalert('Please Enter Remarks !');
                document.getElementById('<%= txtResolveQueryRemarks.clientid%>').value = '';
                document.getElementById('<%= txtResolveQueryRemarks.clientid%>').focus();
                return false;
            }
            return true;
        }

        function GenerateQueryDivShowHide(Type, TargetBaseControl) {
            var TargetBaseControl = document.getElementById('<%=GVEditChecksHdrDtl.ClientID%>');
            var TargetChildControl = "chkGenerateQuery";
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            var result = false;
            for (var n = 0; n < Inputs.length; ++n) {
                if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0) {
                    if (Inputs[n].checked == true) {
                        result = true;
                        break;
                    }
                }
            }
            if (result == false) {
                msgalert('Please Select Any Edit Check To Generate !');
                return false;
            }
            if (Type == 'S') {
                document.getElementById('<%=txtGenerateQueryRemarks.ClientID%>').value = '';
                document.getElementById('<%=divGenerateQuery.ClientID%>').style.display = 'block';
                SetCenter('<%=divGenerateQuery.ClientID%>');
                //createDiv();
                //document.getElementById('<%=divGenerateQuery.ClientID%>').style.display = '';
                //setLayerPosition();
                //toggleDisabled();
                return false;
            }
            else if (Type == 'H') {
                //toggleDisabled();
                document.getElementById('<%=divGenerateQuery.ClientID%>').style.display = 'none';
                return false;
            }
            return true;
        }

        function ResolveQueryDivShowHide(Type, TargetBaseControl) {
            var TargetBaseControl = document.getElementById('<%=GVEditChecksHdrDtl.ClientID%>');
            var TargetChildControl = "chkResolveQuery";
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            var result = false;
            for (var n = 0; n < Inputs.length; ++n) {
                if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0) {
                    if (Inputs[n].checked == true) {
                        result = true;
                        break;
                    }
                }
            }
            if (result == false) {
                msgalert('Please Select Any Edit Check To Resolve !');
                return false;
            }
            if (Type == 'S') {
                document.getElementById('<%=txtResolveQueryRemarks.ClientID%>').value = '';
                document.getElementById('<%=divResolveQuery.ClientID%>').style.display = 'block';
                SetCenter('<%=divResolveQuery.ClientID%>');
                return false;
            }
            else if (Type == 'H') {
                document.getElementById('<%=divResolveQuery.ClientID%>').style.display = 'none';
                return false;
            }
            return true;
        }

        function OnscrollfnctionHdr() {
            var div = document.getElementById('<%=divHdr_Data.ClientID %>');
            var div2 = document.getElementById('<%=divHdr_Header.ClientID%>');
            //****** Scrolling div_Header along with div_Data ******
            div2.scrollLeft = div.scrollLeft;
            return false;
        }
        function OnscrollfnctionDtl() {
            var div = document.getElementById('<%=divDtl_Data.ClientID %>');
            var div2 = document.getElementById('<%=divDtl_Header.ClientID%>');
            //****** Scrolling div_Header along with div_Data ******
            div2.scrollLeft = div.scrollLeft;
            return false;
        }
        var GridId = "<%=GVEditChecksHdrDtl.ClientID %>";
        var ScrollHeight = 50;

        function CreateGridHeaderForDtl() {
            //            var DataDivObj = document.getElementById('<%=divDtl_Data.ClientID %>');
            //            var DataGridObj = document.getElementById('<%=GVEditChecksHdrDtl.ClientID %>');
            //            var HeaderDivObj = document.getElementById('<%=divDtl_Header.ClientID %>');

            var grid = document.getElementById(GridId);
            var gridWidth = grid.offsetWidth;
            var headerCellWidths = new Array();
            for (var i = 0; i < grid.getElementsByTagName("TH").length; i++) {
                headerCellWidths[i] = grid.getElementsByTagName("TH")[i].offsetWidth;
            }
            grid.parentNode.appendChild(document.createElement("div"));
            var parentDiv = grid.parentNode;

            var table = document.createElement("table");
            for (i = 0; i < grid.attributes.length; i++) {
                if (grid.attributes[i].specified && grid.attributes[i].name != "id") {
                    table.setAttribute(grid.attributes[i].name, grid.attributes[i].value);
                }
            }
            table.style.cssText = grid.style.cssText;
            table.style.width = gridWidth + "px";
            table.appendChild(document.createElement("tbody"));
            table.getElementsByTagName("tbody")[0].appendChild(grid.getElementsByTagName("TR")[0]);
            var cells = table.getElementsByTagName("TH");

            var gridRow = grid.getElementsByTagName("TR")[0];
            for (var i = 0; i < cells.length; i++) {
                var width;
                if (headerCellWidths[i] > gridRow.getElementsByTagName("TD")[i].offsetWidth) {
                    width = headerCellWidths[i];
                }
                else {
                    width = gridRow.getElementsByTagName("TD")[i].offsetWidth;
                }
                cells[i].style.width = parseInt(width - 3) + "px";
                gridRow.getElementsByTagName("TD")[i].style.width = parseInt(width - 3) + "px";
            }
            parentDiv.removeChild(grid);

            var dummyHeader = document.createElement("div");
            dummyHeader.appendChild(table);
            parentDiv.appendChild(dummyHeader);
            var scrollableDiv = document.createElement("div");
            gridWidth = parseInt(gridWidth) + 17;
            scrollableDiv.style.cssText = "overflow:auto;height:" + ScrollHeight + "px;width:" + gridWidth + "px";
            scrollableDiv.appendChild(grid);
            parentDiv.appendChild(scrollableDiv);

        }
        function CreateGridHeaderForHdr() {
            var div_DataObj = document.getElementById('<%=divHdr_Data.ClientID %>');
            var DataGridObj = document.getElementById('<%=GVEditChecksHdr.ClientID %>');
            var div_HeaderObj = document.getElementById('<%=divHdr_Header.ClientID %>');

            //********* Creating new table which contains the header row ***********
            var HeadertableObj = div_HeaderObj.appendChild(document.createElement('table'));

            div_DataObj.style.paddingTop = '0px';
            var div_DataWidth = div_DataObj.clientWidth;
            div_DataObj.style.width = '950px';
            div_DataObj.style.height = '125px';

            //********** Setting the style of Header Div as per the Data Div ************
            div_HeaderObj.className = div_DataObj.className;
            div_HeaderObj.style.cssText = div_DataObj.style.cssText;
            //**** Making the Header Div scrollable. *****
            div_HeaderObj.style.overflow = 'auto';

            //*** Hiding the horizontal scroll bar of Header Div ****
            div_HeaderObj.style.overflowX = 'hidden';
            //**** Hiding the vertical scroll bar of Header Div **** 
            div_HeaderObj.style.overflowY = 'hidden';
            div_HeaderObj.style.height = DataGridObj.rows[0].clientHeight + 'px';
            //**** Removing any border between Header Div and Data Div ****
            div_HeaderObj.style.borderBottomWidth = '0px';

            //********** Setting the style of Header Table as per the GridView ************
            HeadertableObj.className = DataGridObj.className;
            //**** Setting the Headertable css text as per the GridView css text 
            HeadertableObj.style.cssText = DataGridObj.style.cssText;
            HeadertableObj.border = '1px';
            HeadertableObj.rules = 'all';
            HeadertableObj.cellPadding = DataGridObj.cellPadding;
            HeadertableObj.cellSpacing = DataGridObj.cellSpacing;

            //********** Creating the new header row **********
            var Row = HeadertableObj.insertRow(0);
            Row.className = DataGridObj.rows[0].className;
            Row.style.cssText = DataGridObj.rows[0].style.cssText;
            Row.style.fontWeight = 'bold';

            //******** This loop will create each header cell *********
            for (var iCntr = 0; iCntr < DataGridObj.rows[0].cells.length; iCntr++) {
                var spanTag = Row.appendChild(document.createElement('td'));
                spanTag.innerHTML = DataGridObj.rows[0].cells[iCntr].innerHTML;

                var width = 0;
                //****** Setting the width of Header Cell **********
                //            if(spanTag.clientWidth > DataGridObj.rows[1].cells[iCntr].clientWidth)
                //            {
                //                width = spanTag.clientWidth;
                //            }
                //            else
                //            {
                width = DataGridObj.rows[1].cells[iCntr].clientWidth;
                // }
                if (iCntr <= DataGridObj.rows[0].cells.length - 2) {
                    spanTag.style.width = width + 'px';
                }
                else {
                    spanTag.style.width = width + 20 + 'px';
                }
                DataGridObj.rows[1].cells[iCntr].style.width = width + 'px';
            }
            var tableWidth = DataGridObj.clientWidth;
            //********* Hidding the original header of GridView *******
            DataGridObj.rows[0].style.display = 'none';
            //********* Setting the same width of all the componets **********
            if (div_DataWidth > tableWidth) {
                div_HeaderObj.style.width = div_DataWidth + 'px';
                div_DataObj.style.width = div_DataWidth + 'px';
            }
            else {
                div_HeaderObj.style.width = tableWidth + 'px';
                div_DataObj.style.width = tableWidth + 'px';
            }
            DataGridObj.style.width = tableWidth + 'px';
            HeadertableObj.style.width = tableWidth + 20 + 'px';

            if (tableWidth > '950') {
                document.getElementById('<%=pnlHdr.ClientID %>').style.width = '950px'
            }

            return false;

        }
        function OpenWindow(Path) {
            window.open(Path);
            return false;
        }

    </script>

    <table id="tblMain" width="100%">
        <tr>
            <td>
                <asp:UpdatePanel runat="server" ID="UpPnlTop" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <table cellpadding="5px" width="100%">
                            <tr>
                                <td class="Label" style="text-align: right; white-space: nowrap; width: 30%">
                                    Project Name/Request Id:
                                </td>
                                <td class="Label" style="text-align: left; white-space: nowrap;">
                                    <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="70%" />
                                    <asp:Button Style="display: none" ID="btnSetProject" runat="server" Text=" Project" />
                                    <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                        CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                        CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                        OnClientShowing="ClientPopulated" ServiceMethod="GetMyProjectCompletionList"
                                        ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True">
                                    </cc1:AutoCompleteExtender>
                                    <asp:HiddenField ID="HSubject" runat="server"></asp:HiddenField>
                                </td>
                            </tr>
                            <tr>
                                <td class="Label" style="text-align: right; white-space: nowrap;">
                                    Visit/Parent Activity:&nbsp;
                                </td>
                                <td class="Label" style="text-align: left; white-space: nowrap;">
                                    <asp:DropDownList ID="ddlVisit" runat="server" CssClass="dropDownList" Width="30%">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <%-- <tr>
                                <td class="Label" style="text-align: right; white-space: nowrap;">
                                    Subjects : &nbsp;
                                </td>
                                <td class="Label" style="text-align: left; white-space: nowrap;">
                                    <asp:CheckBoxList runat="server" ID="ChklSubjects" RepeatColumns="1" RepeatDirection="Vertical"
                                        Height="120px" BorderStyle="Solid" onclick="return ValidateMaxSubjects();">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>--%>
                            <tr style="display: none">
                                <%-- <td class="Label" style="white-space: nowrap; text-align: right">
                                </td>--%>
                                <td class="Label" style="white-space: nowrap; text-align: center" colspan="2">
                                    <asp:RadioButtonList ID="rbtnlstOptions" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="L" Selected="True">Last Executed</asp:ListItem>
                                        <asp:ListItem Value="A">All</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <%-- <tr height="10px">
                                <td class="Label" style="text-align: right; white-space: nowrap;">
                                </td>
                                <td class="Label" style="text-align: left; white-space: nowrap;">
                                </td>
                            </tr>--%>
                            <tr>
                                <%-- <td class="Label" style="text-align: right; white-space: nowrap;">
                                </td>--%>
                                <td class="Label" style="text-align: center; white-space: nowrap;" colspan="2">
                                    <asp:Button ID="btnGo" runat="server" CssClass="btn btnnew" Text="Execute" ToolTip="Execute"
                                        OnClientClick="return Validation();" />
                                    <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" Text="Cancel" ToolTip="Cancel" />
                                    <asp:Button ID="btnExit" runat="server" CssClass="btn btnexit" ToolTip="Exit" Text="Exit" />
                                    <asp:Button ID="btnView" runat="server" CssClass="btn btnnew" ToolTip="View" Text="View"
                                        OnClientClick="return Validation();" />
                                    <asp:Button ID="btnSaveEditchecks" runat="server" Style="display: none" CssClass="btn btnsave"/>
                                </td>
                            </tr>
                            <%-- <tr height="10px">
                                <td class="Label" style="text-align: right; white-space: nowrap;">
                                </td>
                                <td class="Label" style="text-align: left; white-space: nowrap;">
                                </td>
                            </tr>--%>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnSaveEditchecks" />
                        <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
                        <%--<asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />--%>
                        <asp:AsyncPostBackTrigger ControlID="btnExit" EventName="Click" />
                        <%--<asp:AsyncPostBackTrigger ControlID="btnView" EventName="Click" />--%>
                    </Triggers>
                </asp:UpdatePanel>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="upGridHdr" runat="server" RenderMode="Inline" UpdateMode="Conditional"
                                ChildrenAsTriggers="False">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Panel runat="server" ID="pnlHdr" ScrollBars="None">
                                                    <table width="100%">
                                                        <tbody>
                                                            <tr>
                                                                <td style="text-align: left" class="Label" colspan="2">
                                                                    <div runat="server" id="divHdr_Header">
                                                                    </div>
                                                                    <div style="overflow: auto;" id="divHdr_Data" onscroll="OnscrollfnctionHdr();" runat="server">
                                                                        <asp:GridView ID="GVEditChecksHdr" runat="server" SkinID="grdViewSmlAutoSize" AutoGenerateColumns="False"
                                                                            CssClass="gvwProjects" EmptyDataText="No Queries found." Style="margin: auto;">
                                                                            <Columns>
                                                                                <asp:BoundField DataFormatString="number" HeaderText="#">
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="vSubjectId" HeaderText="vSubjectId" />
                                                                                <asp:BoundField DataField="vWorkspaceId" HeaderText="vWorkspaceId" />
                                                                                <asp:BoundField DataField="vInitials" HeaderText="Initials" />
                                                                                <asp:BoundField DataField="ScreenNo" HeaderText="Screen No" />
                                                                                <asp:BoundField DataField="vRandomizationNo" HeaderText="Randomization No" />
                                                                                <%--<asp:BoundField DataField="iTranNo" HeaderText="Tran No" />--%>
                                                                                <%--<asp:BoundField DataField="Activity" HeaderText="Activity" />--%>
                                                                                <asp:BoundField DataField="vLoginName" HeaderText="Executed By" />
                                                                                <asp:BoundField DataField="dFiredDate" HeaderText="Executed Date" />
                                                                                <asp:TemplateField HeaderText="View">
                                                                                    <ItemTemplate>
                                                                                        <%--<asp:LinkButton ID="lnkbtnView" runat="server">View</asp:LinkButton>--%>
                                                                                        <asp:ImageButton ID="lnkbtnView" runat="server" ToolTip="View" ImageUrl="~/Images/view.gif" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField DataField="cRejectionFlag" HeaderText="Rejection Flag" />
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Button runat="server" ID="Btn_viewAll" Text="View All" ToolTip="View All" CssClass="btn btnnew"
                                                    Style="display: none;" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <%--<asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click"></asp:AsyncPostBackTrigger>--%>
                                    <asp:AsyncPostBackTrigger ControlID="btnView" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    <%--<asp:AsyncPostBackTrigger ControlID="btnSaveEditchecks" EventName="Click" />
                                    <asp:PostBackTrigger ControlID="btnSaveEditchecks" />--%>
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="upGridDtl" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Panel runat="server" ID="pnlDtl" Width="100%">
                                                    <table width="100%">
                                                        <tbody>
                                                            <%-- <tr>
                                                                <td style="white-space: nowrap; text-align: center" class="Label" colspan="2">
                                                                    <asp:Label ID="lblInitial" runat="server" Text="" __designer:wfdid="w13"></asp:Label>
                                                                    <asp:Label ID="lblScreenNo" runat="server" Text="" __designer:wfdid="w14"></asp:Label>
                                                                    <asp:Label ID="lblRandomizationNo" runat="server" Text="" __designer:wfdid="w15"></asp:Label>
                                                                </td>
                                                            </tr>--%>
                                                            <tr>
                                                                <td style="white-space: nowrap; text-align: left" class="Label">
                                                                    <asp:Panel runat="server" ID="upDtl">
                                                                        <div runat="server" id="divDtl_Header">
                                                                        </div>
                                                                        <div id="divDtl_Data" onscroll="OnscrollfnctionDtl();" style="overflow: auto; margin: auto;
                                                                            max-width: 900px; max-height: 500px;" runat="server">
                                                                            <asp:GridView ID="GVEditChecksHdrDtl" runat="server" SkinID="grdViewAutoSizeMax" AutoGenerateColumns="False"
                                                                                RowStyle-Wrap="false">
                                                                                <Columns>
                                                                                    <asp:BoundField DataFormatString="number" HeaderText="#">
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="nEditChecksHdrNo" HeaderText="nEditChecksHdrNo" />
                                                                                    <asp:BoundField DataField="nEditChecksDtlNo" HeaderText="nEditChecksDtlNo" />
                                                                                    <asp:BoundField DataField="vMySubjectNo" HeaderText="Screen No" />
                                                                                    <%--<asp:BoundField DataField="ParentActivityName" HeaderText="Parent Activity" />--%>
                                                                                    <asp:TemplateField HeaderText="Activity">
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton runat="server" ID="lBtn_Activity"><%#Eval("vNodeDisplayName")%></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Cross Activity">
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton runat="server" ID="lBtn_CrossActivity"><%#Eval("CrossActivity")%></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <%--<asp:BoundField DataField="vNodeDisplayName" HeaderText="Activity" />--%>
                                                                                    <asp:BoundField DataField="iRepeatNo" HeaderText="Repeatation" />
                                                                                    <asp:BoundField DataField="dFiredDate" HeaderText="Executed Date"/>
                                                                                    <asp:BoundField DataField="vQueryValue" HeaderText="Query" />
                                                                                    <asp:BoundField DataField="vRemarks" HeaderText="Remarks" />
                                                                                    <asp:BoundField DataField="cGenerateFlag" HeaderText="cGenerateFlag" />
                                                                                    <asp:TemplateField HeaderText="Generate Query">
                                                                                        <HeaderTemplate>
                                                                                            <asp:CheckBox ID="chkGenerateQueryAll" runat="server" Text="Generate Query" onclick="SelectAllGenerateQuery(this)" />
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkGenerateQuery" runat="server" />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField DataField="vGenerateRemark" HeaderText="Remarks" />
                                                                                    <asp:BoundField DataField="cResolvedFlag" HeaderText="cResolvedFlag" />
                                                                                    <asp:TemplateField HeaderText="Resolved">
                                                                                        <HeaderTemplate>
                                                                                            <asp:CheckBox ID="chkResolveQueryAll" runat="server" Text="Resolve Query" onclick="SelectAllResolveQuery(this)" />
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkResolveQuery" runat="server" />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField DataField="vResolvedRemark" HeaderText="Remark" />
                                                                                    <asp:BoundField DataField="iMySubjectNo" HeaderText="MySubjectNo" />
                                                                                    <asp:BoundField DataField="iPeriod" HeaderText="Period" />
                                                                                    <asp:BoundField DataField="vActivityId" HeaderText="vActivityId" />
                                                                                    <asp:BoundField DataField="iNodeId" HeaderText="iNodeId" />
                                                                                    <asp:BoundField DataField="vSubjectId" HeaderText="vSubjectId" />
                                                                                    <asp:BoundField DataField="iSourceNodeId_If" HeaderText="iSourceNodeId_If" />
                                                                                    <asp:BoundField DataField="CrossActivity_NodeId" HeaderText="CrossActivity_NodeId" />
                                                                                    <asp:BoundField DataField="CrossActivity_ActivityId" HeaderText="CrossActivity_ActivityId" />
                                                                                    <asp:BoundField DataField="CrossActivity_Period" HeaderText="CrossActivity_Period" />
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                    <table>
                                                        <tbody>
                                                            <tr>
                                                                <td style="white-space: nowrap; text-align: left" class="Label" colspan="2">
                                                                    <input  id="btnGenerateQuery" class="btn btnnew" onclick="return GenerateQueryDivShowHide('S','chkGenerateQueryAll.CleintId');"
                                                                        type="button" value="Generate Query" runat="server" visible="false" />
                                                                    <input  id="btnResolveQuery" class="btn btnnew" onclick="return ResolveQueryDivShowHide('S','chkResolveQueryAll.ClientId');"
                                                                        type="button" value="Resolve Query" runat="server" visible="false" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="white-space: nowrap; text-align: center" class="Label" align="center"
                                                                    colspan="2">
                                                                    <div style="display: none; left: 391px; width: 300px; top: 528px; height: 180px;
                                                                        text-align: left" id="divGenerateQuery" class="DIVSTYLE2" runat="server">
                                                                        <asp:Panel ID="pnlGenerateQuery" runat="server" Width="300px" Height="180px">
                                                                            <table width="100%" style="margin: auto;">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td style="text-align: center" class="Label" colspan="2">
                                                                                            Generate Query
                                                                                        </td>
                                                                                    </tr>
                                                                                    <%--  <tr >
                                                                                        <td style="text-align: center" align="center" colspan="2">
                                                                                        </td>
                                                                                    </tr>--%>
                                                                                    <tr>
                                                                                        <td>
                                                                                            Remarks* :
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtGenerateQueryRemarks" runat="server" Width="208px" Height="70px"
                                                                                                TextMode="MultiLine"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="2">
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="text-align: center" colspan="2">
                                                                                            <asp:Button ID="btndivGenerateQuery" class="btn btnnew" runat="server" Text="Generate"
                                                                                                ToolTip="Generate Query" OnClientClick="return ValidationForGenerateQuery();">
                                                                                            </asp:Button>
                                                                                            <input id="btndivGenerateQueryClose" class="btn btnnew" onclick="return GenerateQueryDivShowHide('H','chkGenerateQueryAll.CleintId');"
                                                                                                type="button" value="Close" title="Close" runat="server" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="white-space: nowrap; text-align: center" class="Label" align="center"
                                                                    colspan="2">
                                                                    <div style="display: none; left: 391px; width: 300px; top: 528px; height: 180px;
                                                                        text-align: left" id="divResolveQuery" class="DIVSTYLE2" runat="server">
                                                                        <asp:Panel ID="pnlResolveQuery" runat="server" Width="300px" Height="180px">
                                                                            <table width="100%" style="margin: auto;">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td style="text-align: center" colspan="2">
                                                                                            Resolve Query
                                                                                        </td>
                                                                                    </tr>
                                                                                    <%--  <tr>
                                                                                        <td style="text-align: center" align="center" colspan="2">
                                                                                        </td>
                                                                                    </tr>--%>
                                                                                    <tr>
                                                                                        <td>
                                                                                            Remarks* :
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtResolveQueryRemarks" runat="server" Width="208px" Height="70px"
                                                                                                TextMode="MultiLine"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <%--   <tr height="6">
                                                                                        <td colspan="2">
                                                                                        </td>
                                                                                    </tr>--%>
                                                                                    <tr>
                                                                                        <td style="text-align: center" colspan="2">
                                                                                            <asp:Button ID="btndivResolveQuery" class="btn btnnew" runat="server" Text="Resolve"
                                                                                                ToolTip="Resolve Query" OnClientClick="return ValidationForResolveQuery();" />
                                                                                            <input id="btndivResolveQueryClose" class="btn btnnew" onclick="return ResolveQueryDivShowHide('H','chkResolveQueryAll.ClientId');"
                                                                                                type="button" value="Close" title="Close" runat="server" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center;">
                                                <asp:Button runat="server" ID="btnExportToExcell" ToolTip="Export To Excel"
                                                    CssClass="btn btnexcel" Style="display: none;" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="GVEditChecksHdr" EventName="RowCommand" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnView" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="Btn_viewAll" EventName="Click" />
                                    <asp:PostBackTrigger ControlID="btnExportToExcell" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">

        function createDiv() {
            var divTag = document.createElement('div');

            divTag.id = 'shadow';
            divTag.setAttribute('align', 'center');
            divTag.style.position = 'absolute';
            divTag.style.top = '0px';
            divTag.style.left = '0px';
            divTag.style.opacity = '0.6';
            divTag.style.filter = 'alpha(opacity=30)';
            divTag.style.backgroundColor = '#000000';
            divTag.style.zIndex = '1000';

            document.body.appendChild(divTag);

        }

        function onUpdated(sender, args) {
            // get the update progress div
            var updateProgressDiv = $get('updateProgress');
            // make it invisible
            updateProgressDiv.style.display = 'none';
            document.body.removeChild(document.getElementById('shadow'));
            //clearTimeout(tld);
        }

        function setLayerPosition() {

            var winScroll = BodyScrollHeight();
            var updateProgressDivBounds = Sys.UI.DomElement.getBounds($get('divGenerateQuery'));
            var shadow = document.getElementById('shadow');
            var bws = GetWindowBounds();

            if (!shadow) {
                return;
            }

            shadow.style.width = bws.Width + "px";
            shadow.style.height = bws.Height + "px";
            shadow.style.top = winScroll.yScr;
            shadow.style.left = winScroll.xScr;

            x = Math.round((bws.Width - updateProgressDivBounds.width) / 2);
            y = Math.round((bws.Height - updateProgressDivBounds.height) / 2);

            x += winScroll.xScr;
            y += winScroll.yScr;

            Sys.UI.DomElement.setLocation($get('divGenerateQuery'), parseInt(x), parseInt(y));
        }

        function toggleDisabled() {
            var el = document.getElementById('tblMain');
            try {
                el.disabled = !el.disabled;
            }
            catch (E)
    { }
            if (el.childNodes && el.childNodes.length > 0) {
                for (var x = 0; x < el.childNodes.length; x++) {
                    toggleDisabled(el.childNodes[x]);
                }
            }
        }

        function SelectAllGenerateQuery(TargetParentControl) {
            var TargetBaseControl = document.getElementById('<%=GVEditChecksHdrDtl.ClientID%>');
            var TargetChildControl = "chkGenerateQuery";
            var Inputs = TargetBaseControl.getElementsByTagName("input");

            for (var n = 0; n < Inputs.length; ++n) {
                if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0) {
                    if (TargetParentControl.checked == true) {
                        Inputs[n].checked = true;
                    }
                    else {
                        Inputs[n].checked = false;
                    }
                }
            }
        }

        function SelectAllResolveQuery(TargetParentControl) {
            var TargetBaseControl = document.getElementById('<%=GVEditChecksHdrDtl.ClientID%>');
            var TargetChildControl = "chkResolveQuery";
            var Inputs = TargetBaseControl.getElementsByTagName("input");

            for (var n = 0; n < Inputs.length; ++n) {
                if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0) {
                    if (TargetParentControl.checked == true) {
                        Inputs[n].checked = true;
                    }
                    else {
                        Inputs[n].checked = false;
                    }
                }
            }
        }

        function ShowConfirmation() {

            var Istrue = msgconfirmalert("Edit checks executed sucessfully.Click on 'View button' to check executed edit checks",this);

            document.getElementById('<%=btnGo.clientid %>').enable = false;
            document.getElementById('<%=btnCancel.clientid %>').enable = false;
            document.getElementById('<%=btnExit.clientid %>').enable = false;
            document.getElementById('<%=btnView.clientid %>').enable = false;
            //           document.getElementById('<%=btnSaveEditchecks.clientid %>').click();
            return true;
        }


    </script>

</asp:Content>
