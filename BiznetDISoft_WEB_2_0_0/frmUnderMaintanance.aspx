<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmUnderMaintanance.aspx.vb"
    Inherits="frmUnderMaintanance" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style>
        body {
            margin:0px auto;
        }
         .content img {
            width:100%;
            height:100%;
        }
        .content {
            /*background-image: url("images/UnderMaintanance.png");*/
            width: 100%;
            background-size: cover;
            background-repeat: no-repeat;
            overflow: hidden;
            height:622px;
            margin:0px auto;
            position: absolute;
            top: 22px;
        }
        .marquee {
            background: #00cfc7;
            opacity: 0.9;
            margin: 0px auto;
        }
        .footer p {
            position: fixed;
            bottom: 0px;
            text-align: center;
            margin: 0px auto;
            left: 0px;
            right: 0px;
            background: #000;
        }
    </style>
 <script type="text/javascript">
     $('.marquee').marquee({
         duration: 5000,
         gap: 50,
         delayBeforeStart: 0,
         direction: 'left',
         duplicated: true
     });
    </script>
</head>
<body>
   <marquee class="marquee">
             <asp:Label ID="lblText" runat="server" Font-Bold="True" Font-Names="Arial Black" ForeColor="Red" ></asp:Label>
         </marquee>
    <div class="content">
        <img src="images/UnderMaintanance.png" alt="img"/>
    </div>
    <div class="footer">
    <p align="center">
        <script type="text/javascript">
            var copyright;
            var update;
            copyright = new Date();
            update = copyright.getFullYear();
            document.write("<font face=\"verdana\" size=\"1\" color=\"white\">© Copyright " + update + ", Sarjen Systems Pvt LTD. </font>");
        </script>
    </p>
    </div>
</body>
</html>
