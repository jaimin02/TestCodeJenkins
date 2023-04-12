<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmCRFReviewHistory.aspx.vb"
    Inherits="frmCRFReviewHistory"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>

<script type="text/javascript" src="Script/jquery.min.js"></script>

<body>
    <form id="form1" runat="server">
    <div id="dv" align="center" style="overflow: auto; width: 550px; text-align: center;">
        <asp:Label runat="server" ID="lblActivity" CssClass="Label"></asp:Label>
        <br /><br />
        <asp:GridView ID="GVWReviewHistory" runat="server" AutoGenerateColumns="False" BorderColor="Peru"
            Font-Size="Small" SkinID="grdViewSmlAutoSize" Width="549px">
            <Columns>
                <asp:BoundField DataField="iTranNo" HeaderText="Sr. No." />
                <%--<asp:BoundField DataField="iWorkFlowStageId" HeaderText="Status" />--%>
                <asp:BoundField DataField="Reviewer" HeaderText="Status" />
                <asp:BoundField DataField="StatusChangedBy" HeaderText="Status Changed By" />
                <asp:BoundField DataField="dModifyOn" HeaderText="Status Changed On" DataFormatString="{0:dd-MMM-yyyy HH:mm}" />
            </Columns>
        </asp:GridView>
        <br />
        <asp:Label runat="server" ID="lblCount" CssClass="Label"></asp:Label>
        <input style="vertical-align:middle;" type="button" value="Close" class="button" onclick="window.close();" />
    </div>
    </form>

    <script type="text/javascript">

        $(document).ready(function()
        {
            var table = $('#dv');
            window.resizeTo(table.width() + 30, table[0].offsetHeight + 70);
        });
    
    </script>

</body>
</html>
