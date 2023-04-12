<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmCTMMeddraBrowse.aspx.vb" Inherits="frmCTMMeddraBrowse" %>


<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <style type="text/css">
        /*Added by ketan for (Resolve issue oveRlap button in datatable)*/
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }
        /*Ended by ketan*/
    </style>

    <%--<script type="text/javascript">
 var Grid = null; for expandable and collapsible grid 
 var UpperBound = 0;
 var LowerBound = 1;
 var CollapseImage = 'images/collapse.PNG';
 var ExpandImage = 'images/expand.PNG';
 var IsExpanded = true;   
 var Rows = null;
 var n = 1;
 var TimeSpan = 25;
        
 window.onload = function()
 {
    Grid = document.getElementById('<%=GVWSearchBuilder.ClientId %>');
    Rows = Grid.getElementsByTagName('tr');
    UpperBound = $('#ctl00_CPHLAMBDA_GVWSearchBuilder ').children('tbody').children('tr').length - 1;
 }
        
 function Toggle(Image)
 {
 
    ToggleImage(Image);
    ToggleRows();  
 }    
        
 function ToggleImage(Image)
 {
 
    if(IsExpanded)
    {
       Image.src = ExpandImage;
       Image.title = 'Expand';
       Grid.rules = 'none';
       n = LowerBound;
             
       IsExpanded = false;
    }
    else
    {
       Image.src = CollapseImage;
       Image.title = 'Collapse';
       Grid.rules = 'cols';
       n = UpperBound;
                
       IsExpanded = true;
    }
 }
        
 function ToggleRows()
 {
 
    if (n < LowerBound || n > UpperBound)  return;
                            
    Rows[n].style.display = Rows[n].style.display == '' ? 'none' : '';
    if(IsExpanded) n--; else n++;
    setTimeout("ToggleRows()",TimeSpan);
 }
</script>--%>

    
        
            <table style="width: 80%; margin: auto;">
                <tr>
                    <td>
                        <div>
                            <strong style="font-weight: bold; font-size: 20px">Global Dictionary</strong>
                            <asp:HiddenField ID="Medracode" runat="server" />
                        </div>
                        <div>
                            <hr style="width: 80%; background-image: none; color: #ffcc66; background-color: #ffcc66"
                                class="hr" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <asp:DropDownList ID="ddlMedDRA" runat="Server" CssClass="dropDownList " AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <fieldset id="fProjectInfo" runat="server" style="width: 70%; margin:0px auto;" >
                            <legend id="lProjectInfo" runat="server">
                                <img id="imgExpand" alt="SubjectSpecific" src="images/expand.jpg" onclick="displayProjectInfo(this);" runat="server" />
                                <asp:Label ID="lblProjectInfo" runat="server" Text="Search/Display Selection" CssClass="Label" />
                            </legend>
                            <div id="Gridshow" runat="server" style="display: none; margin: auto;">
                                <asp:GridView ID="GVWSearchBuilder" runat="server" Font-Size="Small" SkinID="grdViewAutoSizeMax" Style="width: auto; margin: auto;"
                                    BorderWidth="1px" BorderColor="Green" AutoGenerateColumns="False">
                                    <Columns>
                                        <%-- <asp:TemplateField>
                            <HeaderStyle Width="5px" />
                            <ItemStyle Width="5px" BackColor="White" />
                            <HeaderTemplate>
                                <asp:Image ID="imgTab" runat="server" ImageUrl="~/images/collapse.PNG" ToolTip="Collapse"
                                    onclick="javascript:Toggle(this);" />
                            </HeaderTemplate>
                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Search Fields">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkMedraName" runat="server" class="chkSearch"></asp:CheckBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Display Fields">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkDisplayField" runat="server" class="chkDisplay"></asp:CheckBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Field Name">
                                            <%--MedDRA Term--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSearchField" runat="server" Text='<% #DataBinder.Eval(Container.DataItem, "AliasName")%>'></asp:Label>
                                                <asp:HiddenField ID="hdnvField" runat="server" Value='<% #DataBinder.Eval(Container.DataItem, "FieldName")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Condition">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlSearchCondition" runat="server" Width="55px" CssClass="dropDownList">
                                                    <asp:ListItem Text="Like"></asp:ListItem>
                                                    <asp:ListItem Text="="></asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Search Value">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtSearchValueText" CssClass="textBox" Width="80%" runat="server"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <SelectedRowStyle BackColor="Moccasin"></SelectedRowStyle>
                                </asp:GridView>
                            </div>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td class="Label" style="text-align: center;">
                        <asp:Button ID="BtnGo" runat="server" Text="" ToolTip="Go" OnClientClick="return validation();"
                            CssClass="btn btngo" />
                        <asp:Button ID="Btnback" runat="server" ToolTip="Back To Meddra Code" Text="" Visible="false" CssClass="btn btnback" OnClientClick="return back();" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                </tr>
            </table>
            
            <table width="80%">
                <tr>
                    <td>
                        <div class="tabgrd" style="width: 900px; margin-left: 230px;">
                            <asp:GridView ID="GVWMeddra" runat="server" AutoGenerateColumns="true" BorderColor="Peru"
                                Font-Size="Small">
                                <Columns>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnSelect" runat="Server" Text="Select"  OnClick="BtnGo_Click"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>

    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script src="Script/jquery.dataTables.min.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">

        function SetMeddraValues(Meddravalue) {
            if (document.getElementById('<%=Medracode.ClientID%>').value == 1) {
                var popup = confirm("Are You Sure You Have Selected Correct Code For Crf Term");
                //var popup = msgconfirmalert('Are You Sure You Have Selected Correct Code For Crf Term ?', this);
                if (popup == true) {
                    var obj = { Meddra: Meddravalue }
                    var parWin = window.opener;
                    if (parWin != null && typeof (parWin) != 'undefined') {
                        parWin.SetMeddra(obj);
                        self.close();
                    }
                }
                else {
                    return false;
                }

                //msgConfirmDeleteAlert(null, "Are you sure want to Exit ?", function (isConfirmed) {
                //    if (isConfirmed) {

                //        var obj = { Meddra: Meddravalue }
                //        var parWin = window.opener;
                //        if (parWin != null && typeof (parWin) != 'undefined') {
                //            parWin.SetMeddra(obj);
                //            self.close();
                //        }
                //        return true;
                //    }
                //    else {
                //        return false;
                //    }
                //});
                //return false;

            }
            var obj = { Meddra: Meddravalue }
            var parWin = window.opener;
            if (parWin != null && typeof (parWin) != 'undefined') {
                parWin.SetMeddra(obj);
                self.close();
            }
        }
        function validation() {
            var res = "";
            if (document.getElementById('<%=ddlMedDRA.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Dictionary !');
                return false;
            }
            $('.chkSearch input[type="checkbox"]').each(function () {
                if (this.checked == true) {
                    res = true;
                }

            });
            if (res != true) {
                msgalert("Plese select any Serch field !");
                return false
            }
            res = "";
            $('.chkDisplay input[type="checkbox"]').each(function () {
                if (this.checked == true) {
                    res = true;
                }

            });
            if (res != true) {
                msgalert("Plese select any Display field !");
                return false
            }

            return true;
        }
        function displayProjectInfo(ele) {

            if (ele.src.toString().toUpperCase().search("EXPAND") != -1) {
                $('#ctl00_CPHLAMBDA_Gridshow').slideToggle(500);
                ele.src = "images/collapse_blue.jpg";

            }
            else {
                $('#ctl00_CPHLAMBDA_Gridshow').slideToggle(500);
                ele.src = "images/expand.jpg";
            }
        }
        function back() {

            //var backpopup = confirm("Are You Sure You Want To Go Back");
            var backpopup = msgconfirmalert_medicalcoding('Are You Sure You Want To Go Back ?', this);
            if (backpopup == true) {

                self.close();
            }

            else {
                return false;
            }
        }

        function bindgGVWMeddra() {
            displayProjectInfo(ctl00_CPHLAMBDA_imgExpand);
            oTable = jQuery('#<%= GVWMeddra.ClientID%>').prepend(jQuery('<thead>').append(jQuery('#<%= GVWMeddra.ClientID%> tr:first'))).dataTable({
                "sScrollY": "100%",
                "sScrollX": "100%",
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": false,
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": false,
                "autoWidth": true,
                "bInfo": true,
                "oLanguage": {
                    "sEmptyTable": "No Record Found",
                },

            });

        }

    </script>

</asp:Content>
