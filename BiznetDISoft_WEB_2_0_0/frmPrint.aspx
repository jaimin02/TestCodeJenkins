<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmPrint.aspx.vb" Inherits="frmPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <%--<title>Print Page</title>--%>
    <style type="text/css">
        #lblHeading
        {
            text-shadow: 0 1px 0 #CCC, 0 2px 0 #C9C9C9, 0 3px 0 #BBB, 0 4px 0 #B9B9B9, 0 5px 0 #AAA, 0 6px 1px rgba(0, 0, 0, .1), 0 0 5px rgba(0, 0, 0, .1), 0 1px 3px rgba(0, 0, 0, .3), 0 3px 5px rgba(0, 0, 0, .2), 0 5px 10px rgba(0, 0, 0, .25), 0 10px 10px rgba(0, 0, 0, .2), 0 20px 20px rgba(0, 0, 0, .15);
            font-size: 30px !important;
            color: #105992 !important;
            text-align: center;
        }
        body, div, span, table, tr, td, th, label, li, ul, a, textarea, select, option
        {
            font-family: Delicious, sans-serif !important;
        }
        button:hover, input[type="button"]:hover, input[type="submit"]:hover
        {
            -webkit-box-shadow: 0px 1px 3px rgba(0, 0, 0, 0.2);
            -moz-box-shadow: 0px 1px 3px rgba(0, 0, 0, 0.2);
            box-shadow: 0px 1px 3px rgba(0, 0, 0, 0.2);
            background: #EBEBEB -webkit-linear-gradient(#FEFEFE, #F8F8F8 40%, #E9E9E9);
            border-color: #999;
            color: #222;
        }
        button, input[type="button"], input[type="submit"]
        {
            -webkit-border-radius: 2px;
            -webkit-box-shadow: 0px 1px 3px rgba(0, 0, 0, 0.1);
            -moz-border-radius: 2px;
            -moz-box-shadow: 0px 1px 3px rgba(0, 0, 0, 0.1);
            border-radius: 2px;
            box-shadow: 0px 1px 3px rgba(0, 0, 0, 0.1);
            background: -webkit-linear-gradient(#FAFAFA, #F4F4F4 40%, #E5E5E5);
            border: 1px solid #AAA;
            color: #444;
            font-size: inherit;
            margin-bottom: 0px;
            min-width: 4em;
            padding: 3px 12px 3px 12px;
            font-weight: bold;
        }
    </style>
</head>
<body onload="Print()">
    <object id="myctr" height="0" width="0" classid="clsid:CA8A9780-280D-11CF-A24D-444553540000"
        viewastext>
    </object>
    <form id="form1" runat="server">
    <table align="center" style="width: 90%; padding: 20px;">
        <tr>
            <td style="padding: 10px;">
                <asp:Label ID="lblHeading" runat="server" Text="Printing in progress..."></asp:Label>
                <br />
                <img src="images/print_loader.gif" alt="Print Loader" width="200px" style="margin-top: 15px;" />
                <hr />
            </td>
        </tr>
        <tr id="trBack">
            <td style="padding: 10px;">
                <asp:Button ID="btnBack" runat="server" Text="" Style="display: none;" CssClass="btn btnback" />
            </td>
        </tr>
    </table>
    </form>

    <script type="text/vbscript">
        Sub Print()	        
            dim url            
            url = window.location.href
            'msgbox url
            a=Split(url,"=")
            'msgbox a(1)
            With myctr 
                .LoadFile(a(1))
                .PrintAll() 
            End With 
            'msgbox "Document sent successfully to the default printer."
            back()
        End Sub        
        
    </script>

    <script type="text/javascript">
        function back()
        {
            setTimeout("recall()",3000);   
        }
        
        function recall()
        {            
            msgalert('Document sent successfully to the default printer.');
            document.getElementById('<%=btnBack.ClientID%>').click();
        }
    </script>

</body>
</html>
