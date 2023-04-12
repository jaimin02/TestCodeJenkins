<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmInprocQC, App_Web_qa4vhgvp" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" Runat="Server">

 <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }        
  </style>

    <table width ="100%">
        <tr>
            <td>
                <asp:Label ID="lblHeader" runat="server" SkinID="lblHeading"></asp:Label><br />
                <br />
                <asp:HiddenField ID="HFWorkspaceId" runat="server" />
                <asp:HiddenField ID="HFActivityId" runat="server" />
                <asp:HiddenField ID="HFNodeId" runat="server" />
                <asp:HiddenField ID="HFPeriodId" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <div style="width:80%; padding-left: 10%;">
                <asp:GridView ID="gvSubjectQC" runat="server" AllowPaging="True" PageSize="25" AutoGenerateColumns="False"
                    style="width:65%; margin:auto;" ShowHeaderWhenEmpty="true">
                    <Columns>
                        <asp:BoundField DataField="vMySubjectNo" HeaderText="Subject No.">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="vSubjectId" HeaderText="Subject Id">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FullName" HeaderText="Subject Name">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                       <%-- <asp:BoundField DataField="vModifyBy" HeaderText="User" />--%>
                        <%--<asp:BoundField DataField="dStartDate" HeaderText="Date" />--%>
                        <asp:BoundField HeaderText="No Of Comments">
                        <ItemStyle HorizontalAlign="Center" Font-Bold="True" Font-Size="Medium" />
                        </asp:BoundField>
                         <asp:BoundField HeaderText="No Of Response">
                        <ItemStyle HorizontalAlign="Center" Font-Bold="True" Font-Size="Medium" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Q.C." SortExpression="status">
                            <ItemTemplate>
                               <asp:LinkButton ID="LnkQC" runat="server">Q.C.</asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign ="Center" VerticalAlign ="Middle" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="iMySubjectNo" HeaderText="MySubject No" />
                    </Columns>
                </asp:GridView>
                    </div>
                </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="BtnBack" runat="server" CssClass="btn btnback" Text="" ToolTip ="Back" /></td>
        </tr>
    </table>
    <script type="text/javascript" src="Script/jquery-1.4.3.min.js"></script>
    <script src="Script/jquery.dataTables.min.js" type="text/javascript"></script>
    <script src="Script/jquery.dataTables.plugins.js" type="text/javascript"></script>
    <script>
        function RefreshPage() {
            window.location.href = window.location.href;
        }
      function pageLoad() {
                $('[id$="' + '<%= gvSubjectQC.ClientID%>' + '"] tbody tr').length < 8 ? scroll = "25%" : scroll = "300px";
                $('#<%= gvSubjectQC.ClientID%>').prepend($('<thead>').append($('#<%= gvSubjectQC.ClientID%> tr:first'))).dataTable({
                    "bJQueryUI": true,
                    "sScrollX": "100%",
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
                $('#<%= gvSubjectQC.ClientID%> tr:first').css('background-color', '#3A87AD');
                $('tr', $('.dataTables_scrollHeadInner')).css("background-color", "rgb(58, 135, 173)");                
      }
      </script>
</asp:Content>

