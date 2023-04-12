<%@ page language="VB" autoeventwireup="false" inherits="frmrptScreeningAuditTrail, App_Web_eoahe1pj" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Subject Medical Examination </title>
    <link href="App_Themes/StyleBlue/StyleBlue.css" rel="stylesheet" type="text/css" />

    <script src="Script/General.js" language="javascript" type="text/javascript"></script>

    <script src="Script/Validation.js" language="javascript" type="text/javascript"></script>

    <script src="Script/popcalendar.js" language="javascript" type="text/javascript"></script>

    <script src="Script/AutoComplete.js" language="javascript" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
    
    var currTab;
    
function closewindow()
       { 
           self.close();
       }
       
function Next(NoneDivId)
    {
     var arrDiv=NoneDivId.split(',');
     var isShow=false;
       for (i=0;i<arrDiv.length;i++)
       {    
           document.getElementById(arrDiv[i]).style.display='none';
            var disBtn=arrDiv[i].replace('Div','BtnDiv');
           
            document.getElementById(disBtn).style.color='Brown';
       }
       
       for (i=0;i<arrDiv.length;i++)
       {    
            if (isShow)
            {
                currTab=arrDiv[i];
                isShow=false;
                break;
            }
           
         if (arrDiv[i].toLowerCase() == currTab.toLowerCase())
            {
                isShow=true;
            }
       }
       
       var currBtn=currTab.replace('Div','BtnDiv');
       document.getElementById(currTab).style.display='block';
       document.getElementById(currBtn).style.color='navy';
       return false;
    }

function Previous(NoneDivId)
    {
     var arrDiv=NoneDivId.split(',');
     for (i=0;i<arrDiv.length;i++)
       {    
           document.getElementById(arrDiv[i]).style.display='none';
            var disBtn=arrDiv[i].replace('Div','BtnDiv');
           
            document.getElementById(disBtn).style.color='Brown';
       }
       
       for (i=0;i<arrDiv.length;i++)
       {    
           if (arrDiv[i].toLowerCase() == currTab.toLowerCase())
            {
                if (i>0)
                {
                    currTab=arrDiv[i-1];
                    break;
                }
            }
       }
       
       var currBtn=currTab.replace('Div','BtnDiv');
       document.getElementById(currTab).style.display='block';
       document.getElementById(currBtn).style.color='navy';
       return false;
    }
    
function DisplayDiv(BlockDivId,NoneDivId)
       {
       
       var selBtn=BlockDivId.replace('Div','BtnDiv');
       var arrDiv=NoneDivId.split(',');
       
       for (i=0;i<arrDiv.length;i++)
       {    
            document.getElementById(arrDiv[i]).style.display='none';
            var disBtn=arrDiv[i].replace('Div','BtnDiv');
           
            document.getElementById(disBtn).style.color='Brown';
           
       }
       document.getElementById(BlockDivId).style.display='block';
       document.getElementById(selBtn).style.color='navy';
       return false;
       }
       

    </script>

</head>
<body>
    <form id="form1" runat="server" method="post">
    <asp:ScriptManager ID="ScriptManager2" runat="server" AsyncPostBackTimeout="1000"
        EnablePageMethods="True">
        <Services>
            <asp:ServiceReference Path="AutoComplete.asmx" />
        </Services>
    </asp:ScriptManager>

    <script type="text/javascript">
        
        
        function HistoryDivShowHide(Type,MedexCode,BlockDivId,NoneDivId)
        {
        
        //alert(document.getElementById('divHistoryDtl'));
        var btn=document.getElementById('<%= btnHistory.ClientId %>');
        document.getElementById('<%= hfMedexCode.ClientId %>').value = MedexCode;
        
          if (document.getElementById('HSubjectId').value.toString().trim().length <= 0)
            {
              msgalert('Please Enter Subject !');
                document.getElementById('txtSubject').focus();
                document.getElementById('txtSubject').value ='';
                return false;
            }
          else if (Type =='S')
            {
                //document.getElementById('divHistoryDtl').style.display = 'block';
                //SetCenter('divHistoryDtl');
                btn.click();
                return false;
            }
          else if (Type =='H')
            {
                document.getElementById('divHistoryDtl').style.display = 'none';
                return false;
            }
          else if (Type =='SN')
            {
                document.getElementById('divHistoryDtl').style.display = 'block';
                SetCenter('divHistoryDtl');
                return DisplayDiv(BlockDivId,NoneDivId);                
            }
          return true;
          
        }
        
        
          //For Validation
         
        function Validation()
        {
            
            if (document.getElementById('<%= HSubjectId.ClientId %>').value.toString().trim().length <= 0)
            {
                msgalert('Please Enter Subject !');
                document.getElementById('<%= txtSubject.ClientId %>').focus();
                document.getElementById('<%= txtSubject.ClientId %>').value ='';
                return false;
            }
            return true;
        
        }               


        function ClientPopulated(sender, e)
        {
            SubjectClientShowing('AutoCompleteExtender1',$get('<%= txtSubject.ClientId %>'));
        }
            
        function OnSelected(sender,e)
        {
            SubjectOnItemSelected(e.get_value(), $get('<%= txtSubject.clientid %>'),
                $get('<%= HSubjectId.clientid %>'),document.getElementById('<%= btnSubject.ClientId %>'));
        }
        
    </script>

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
                &nbsp; &nbsp;
                <asp:LinkButton ID="LinkButton1" runat="server" Font-Bold="True" ForeColor="Navy"
                    PostBackUrl="~/frmMainPage.aspx">Home</asp:LinkButton>
                &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;<div align="center">
                    <table border="1" cellspacing="1" bordercolor="#F2A041" width="99%" cellpadding="0">
                        <tr>
                            <td align="center" style="width: 98%">
                                <asp:Panel ID="Pan_Hdr" runat="server" Width="100%" CssClass="InnerTable" BackColor="White">
                                    <asp:Panel ID="Pan_Child" runat="server" Width="100%" BackColor="Window">
                                        <div id="Header Label" style="text-align: center;" align="center" class="Div">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td align="left">
                                                    </td>
                                                    <td align="right" style="font-weight: bold; font-size: x-small; width: 187px; color: Navy;
                                                        font-family: Verdana, Sans-Serif; font-variant: normal">
                                                    </td>
                                                </tr>
                                            </table>
                                            <table align="center" style="width: 100%">
                                                <tr>
                                                </tr>
                                                <tr align="center">
                                                    <td align="center" width="890px">
                                                        <strong style="font-weight: bold; font-size: 20px">Subject Screening&nbsp;Audit Trail
                                                            Report</strong>
                                                        <hr style="width: 982px; background-image: none; color: #ffcc66; background-color: #ffcc66" />
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr align="center">
                                                    <td align="left" width="890">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td align="left" style="width: 100%;" class="Label">
                                                                    <asp:Label ID="lblMedExGroup" runat="server" SkinID="lblDisplay" Text="Medical Eaxmination Group:"
                                                                        Visible="False"></asp:Label>
                                                                    <asp:DropDownList ID="DDLMedexGroup" runat="server" AutoPostBack="True" CssClass="dropDownList"
                                                                        Visible="False">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="white-space: nowrap; vertical-align: middle; width: 3px;" align="left">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="Label" style="width: 100%">
                                                                    <asp:Label ID="Label1" runat="server" SkinID="lblDisplay" Text="Project: " Visible="False"></asp:Label>
                                                                    <asp:DropDownList ID="DDLWorkspace" runat="server" CssClass="dropDownList" Width="492px"
                                                                        Style="display: none" AutoPostBack="True">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td align="left" style="vertical-align: middle; background-repeat: no-repeat; white-space: nowrap;
                                                                    background-color: transparent; width: 3px;">
                                                                </td>
                                                            </tr>
                                                            <%--<tr>
                                                                   <td align="left" class="Label" style="width: 100%; white-space: nowrap; height: 110px">
                                                                        Project Name/Request Id:
                                                                        <asp:TextBox ID="txtproject" runat="server" CssClass="textBox"
                                                                            Width="362px"></asp:TextBox>
                                                                            <asp:Button ID="btnSetProject" runat="server" Style="display: none" Text=" Project" />
                                                                        <asp:HiddenField ID="HProjectId" runat="server" />
                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtenderWorkspace" runat="server" BehaviorID="AutoCompleteExtenderWorkspace"
                                                                            CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                                            CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelectedWorkspace"
                                                                            OnClientShowing="ClientPopulatedWorkspace" ServiceMethod="GetProjectCompletionListWithOutSponser" ServicePath="AutoComplete.asmx"
                                                                            TargetControlID="txtProject" UseContextKey="True">
                                                                        </cc1:AutoCompleteExtender>
                                                                    </td>
                                                                    <td align="left" style="vertical-align: middle; width: 3px; background-repeat: no-repeat;
                                                                        white-space: nowrap; height: 110px; background-color: transparent">
                                                                    </td>
                                                                </tr>--%>
                                                            <tr>
                                                                <td align="left" style="white-space: nowrap; width: 100%; height: 110px;">
                                                                    <asp:Label ID="lblSuject" runat="server" SkinID="lblDisplay" Text="Subject: " CssClass="Label"></asp:Label>
                                                                    <asp:TextBox ID="txtSubject" runat="server" CssClass="textBox" TabIndex="2" Width="480px"></asp:TextBox>
                                                                    <asp:Button ID="btnSubject" runat="server" Style="display: none" Text="Subject" />
                                                                    &nbsp;
                                                                    <asp:HiddenField ID="HSubjectId" runat="server" />
                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                                                        MinimumPrefixLength="1" OnClientItemSelected="OnSelected" OnClientShowing="ClientPopulated"
                                                                        ServiceMethod="GetSubjectCompletionList_NotRejected" ServicePath="AutoComplete.asmx"
                                                                        TargetControlID="txtSubject" UseContextKey="True">
                                                                    </cc1:AutoCompleteExtender>
                                                                    <div id="divHistoryDtl" runat="server" class="DIVSTYLE2" style="display: none; left: 391px;
                                                                        width: 650px; top: 367px; text-align: left; height: 250px;">
                                                                        <table style="width: 650px;">
                                                                            <tr>
                                                                                <td style="width: 100px">
                                                                                    <strong style="white-space: nowrap">Attribute History Of
                                                                                        <asp:Label ID="lblMedexDescription" runat="server"></asp:Label></strong>
                                                                                </td>
                                                                                <td align="right">
                                                                                    <img src="images/close.gif" onclick="HistoryDivShowHide('H','','','');" style="width: 21px;
                                                                                        height: 15px" />&nbsp;&nbsp;
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2">
                                                                                    <asp:Panel ID="pnlHistoryDtl" runat="server" Width="650px" ScrollBars="Auto" Height="250px">
                                                                                        <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                                                                                        <br />
                                                                                        <asp:GridView ID="GVHistoryDtl" runat="server" AutoGenerateColumns="False" BorderColor="Peru"
                                                                                            Font-Size="Small" SkinID="grdViewSmlSize">
                                                                                            <Columns>
                                                                                                <asp:BoundField DataField="vSubjectId" HeaderText="Subject Id">
                                                                                                    <ItemStyle Wrap="false" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="dScreenDate" HeaderText="Screening Date">
                                                                                                    <ItemStyle Wrap="false" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="vMedExDesc" HeaderText="Attribute">
                                                                                                    <ItemStyle Wrap="false" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="iTranNo" HeaderText="Sr. No."></asp:BoundField>
                                                                                                <asp:BoundField DataField="vDefaultValue" HeaderText="Value" />
                                                                                                <asp:BoundField DataField="vModifyBy" HeaderText="Modify By">
                                                                                                    <ItemStyle Wrap="False" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="dModifyOn" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Modify On"
                                                                                                    HtmlEncode="False">
                                                                                                    <ItemStyle Wrap="False" />
                                                                                                </asp:BoundField>
                                                                                            </Columns>
                                                                                        </asp:GridView>
                                                                                    </asp:Panel>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </td>
                                                                <td align="left" style="vertical-align: middle; background-repeat: no-repeat; white-space: nowrap;
                                                                    background-color: transparent; width: 3px; height: 110px;">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" align="left" style="width: 100%; height: 169px;">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td align="left" valign="top">
                                                                                <asp:UpdatePanel ID="UpPlaceHolder" runat="server">
                                                                                    <ContentTemplate>
                                                                                        <asp:Panel ID="PnlPlaceMedex" runat="server" Width="100%">
                                                                                            <asp:PlaceHolder ID="PlaceMedEx" runat="server" EnableViewState="true"></asp:PlaceHolder>
                                                                                        </asp:Panel>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                                <asp:HiddenField ID="hfMedexCode" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" style="width: 100%">
                                                                    <table style="width: 890px">
                                                                        <tr>
                                                                            <td align="left" style="width: 50%">
                                                                                <asp:Button ID="BtnPrevious" runat="server" CssClass="btn btnnew" Text="<< Previous"/>
                                                                            </td>
                                                                            <td align="right" style="width: 50%">
                                                                                <asp:Button ID="BtnNext" runat="server" CssClass="btn btnnew" Text="Next >>"/>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td style="width: 3px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" align="left" style="width: 890px">
                                                                    <asp:Button ID="btnHistory" runat="server" CssClass="btn btnnew" Text="History" Style="display: none" /><asp:Button
                                                                        ID="BtnExit" runat="server" CssClass="btn btnexit" Text="Exit" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
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
            </td>
        </tr>
        <tr>
            <td background="images/orangebg.gif" style="width: 95%; height: 30px;">
                <p align="center">

                    <script type="text/javascript">
			        var copyright;
			        var update;
			        copyright=new Date();
			        update=copyright.getFullYear();
			        document.write("<font face=\"verdana\" size=\"1\" color=\"black\">© Copyright "+ update + ", Lambda Therapeutic Research. </font>");

                    </script>

                </p>
            </td>
        </tr>
    </table>
  
 

    </form>
</body>
</html>
