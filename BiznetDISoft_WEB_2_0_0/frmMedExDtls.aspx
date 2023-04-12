<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmMedExDtls.aspx.vb" Inherits="frmMedExDtls" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Medical Examination for WorkSpace</title>
    <link href="App_Themes/StyleBlue/StyleBlue.css" rel="stylesheet" type="text/css" />

    <script src="Script/Validation.js" language="javascript" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
       function closewindow()
       { 
           //self.close();
       }
       
       function ValidateTextbox(checktype,txt,msg)
       {
       var result;
       
            switch (checktype)
            {
                case 1:
                        result=CheckInteger(txt.value);
                        break;
                case 2:
                        result=CheckDecimal(txt.value);
                        break;           
                case 3:
                        result=CheckIntegerOrBlank(txt.value);//CheckIntegerOrBlank
                        break;   
                case 4:
                        result=CheckDecimalOrBlank(txt.value);
                        break;    
                case 5:
                        result=CheckAlphabet(txt.value);
                        break;                            
                case 6:
                        result=CheckAlphaNumeric(txt.value);
                        break;                                                                                   
                default: msgalert("oh u have all rights !");
            }
        
       if (result==false)
       {
       txt.value='';
       txt.focus();
       msgalert(msg);
       }
       }
       
       function DisplayDiv(BlockDivId,NoneDivId)
       {
       var arrDiv=NoneDivId.split(',');
       //alert(NoneDivId);
       //alert(BlockDivId);
       for (i=0;i<arrDiv.length;i++)
       {    
            document.getElementById(arrDiv[i]).style.display='none';
       }
       document.getElementById(BlockDivId).style.display='block';
       return false;
       }
       
    function DateConvert_Age(ParamDate,txtdate)
     {
            if (! DateConvert(ParamDate,txtdate))
            {
               return false;
            }
            return true;
     }
    </script>

</head>
<body>
    <form id="form1" runat="server" method="post">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="1000"
        EnablePageMethods="True">
        <Services>
            <asp:ServiceReference Path="AutoComplete.asmx" />
        </Services>
    </asp:ScriptManager>
    <table border="0" cellspacing="0" style="border-collapse: collapse" bordercolor="#111111"
        width="998" id="AutoNumber1" cellpadding="0">
        <tr>
            <td style="width: 95%">
                <img border="0" src="images/topheader.jpg" width="1004">
            </td>
        </tr>
        <tr>
            <td background="images/bluebg.gif" align="left" style="width: 95%">
            </td>
        </tr>
        <tr>
            <td background="images/whitebg.gif" align="left" style="width: 95%">
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<div align="center">
                    <table border="1" cellspacing="1" bordercolor="#F2A041" width="99%" cellpadding="0">
                        <tr>
                            <td align="center" style="width: 98%">
                                <asp:Panel ID="Pan_Hdr" runat="server" Width="100%" CssClass="InnerTable" BackColor="#F9FFFC">
                                    <asp:Panel ID="Pan_Child" runat="server" Width="100%" BackColor="Window">
                                        <div id="Header Label" style="text-align: center;" align="center" class="Div">
                                            <table style="width: 90%">
                                                <tr>
                                                    <td align="left">
                                                        &nbsp;
                                                    </td>
                                                    <td align="right" style="font-weight: bold; font-size: x-small; width: 187px; color: Navy;
                                                        font-family: Verdana, Sans-Serif; font-variant: normal">
                                                        <asp:Label ID="lblMandatory" runat="server" Text="Fields with * are Mandatory "></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table align="center" style="width: 100%">
                                                <tr>
                                                    <td align="center" width="100%">
                                                        <asp:Label ID="lblHeading" runat="server" SkinID="lblHeading" Font-Bold="True" Font-Size="Large"
                                                            ForeColor="Navy">Medical Examination For Woerkspace</asp:Label>
                                                        <br />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <table>
                                            <tr>
                                                <td align="left" style="vertical-align: middle; white-space: nowrap" class="Label">
                                                    Medical Eaxmination Group :
                                                </td>
                                                <td style="white-space: nowrap; vertical-align: middle" align="left">
                                                    <asp:DropDownList ID="DDLMedexGroup" runat="server" AutoPostBack="True" CssClass="dropDownList"
                                                        Width="334px" Visible="False">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="Label">
                                                    Subject :
                                                </td>
                                                <td style="white-space: nowrap; vertical-align: middle" align="left">
                                                    <asp:DropDownList ID="ddlSubject" runat="server" CssClass="dropDownList" Width="334px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="left">
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="left">
                                                                <asp:PlaceHolder ID="PlaceMedEx" runat="server" EnableViewState="true"></asp:PlaceHolder>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="white-space: nowrap; vertical-align: middle" align="left">
                                                                <asp:Button ID="BtnAdd" Visible="false" runat="server" CssClass="btn btnnew" Text="Add" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="GridView1" runat="server">
                                                            </asp:GridView>
                                                            <br />
                                                            <asp:GridView ID="GV_MedEx" runat="server" AutoGenerateColumns="False">
                                                                <Columns>
                                                                    <asp:BoundField DataField="vSubjectId" HeaderText="Subject Id" />
                                                                    <asp:BoundField DataField="vMedExCode" HeaderText="MedEx Code" />
                                                                    <asp:BoundField DataField="vSubjectName" HeaderText="Subject Name" />
                                                                    <asp:BoundField DataField="vMedExDesc" HeaderText="MedEx Name" />
                                                                    <asp:BoundField DataField="vMedExValue" HeaderText="MedEx Value" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="left">
                                                    <asp:Button ID="BtnSave" runat="server" CssClass="btn btnsave" Text="Save" />
                                                    <asp:Button ID="BtnExit" runat="server" CssClass="btn btnexit" Text="Exit" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this); " />
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                    </asp:Panel>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td style="width: 95%">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td background="images/orangebg.gif" style="width: 95%">
                <p align="center">
                    copyright statement
            </td>
        </tr>
    </table>
    <%--<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False">
    </asp:GridView>--%>
    </form>
</body>
</html>
