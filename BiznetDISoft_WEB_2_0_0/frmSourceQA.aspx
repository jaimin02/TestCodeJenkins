<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmSourceQA.aspx.vb" Inherits="frmSourceQA"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <link href="App_Themes/StyleBlue/StyleBlue.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Script/jquery-1.9.1.js"></script>

    <script type="text/javascript" src="Script/jquery-ui-1.10.2.custom.min.js"></script>

    <script type="text/javascript" src="Script/jquery-ui.js"></script>
    
    <script type="text/javascript" src="Script/slimScroll.min.js"></script>

    <script type="text/javascript" src="Script/General.js"></script>

    <script type="text/javascript" language="javascript">

//function RefreshPage()
//{
//window.location.href = window.location.href;
//}

   
function ShowColor(gvr)
 {
  var lastColor='';
  var lastId='';
   if (lastColor != '')
     {
       document.getElementById(lastId).style.backgroundColor = lastColor;
     }
     lastColor = gvr.style.backgroundColor;
     lastId = gvr.id
     gvr.style.backgroundColor = '#ffdead';
 }

function QCDivShowHide(Type)
{
 debugger     
          //alert(document.getElementById('divQCDtl'));

          if (Type =='S')
          {
              //$find('MPESourceQA').show();
              document.getElementById('divQCDtl').style.display = 'block';
              document.getElementById('mdlDivQc_backgroundElement').style.display = 'block';
              //alert('Hi');
                SetCenter('divQCDtl');
                    //alert('Hello');
                    //alert('Now You Can Add QC Comments.');
                return false;
            }
          else if (Type =='H')
            {
              //$find('MPESourceQA').hide()
              document.getElementById('divQCDtl').style.display = 'none';
                return false;
            }
          else if (Type =='ST')
          {
              //$find('MPESourceQA').show()
                document.getElementById('divQCDtl').style.display = 'block';
                document.getElementById('mdlDivQc_backgroundElement').style.display = 'block';
              //SetCenter('divQCDtl');
                return true;
            }
            return true;
        }

//function openpopup() {
//    $find('divQCDtl').show();
//}
function closesequencediv() {
    $('#<%=txtQCRemarks.ClientID%>').val('');
    $('#divQCDtl').hide();
    document.getElementById('mdlDivQc_backgroundElement').style.display = 'none';
}

      function closewindow()
       { 
//            var conf = confirm('Are You Sure You Want To Exit?');
         
//            if (conf)
//        {
            var parWin = window.opener;
            if (parWin != null && typeof (parWin) != 'undefined') 
            {
                parWin.RefreshPage();
                self.close();
            }
          //         }
            document.getElementById('mdlDivQc_backgroundElement').style.display = 'none';
       }
       
              function ValidateRemarks(txt, cntField, maxSize) 
{
         //cntField = document.getElementById(cntField);
         cntField = document.getElementById('<%=lblcnt.ClientID%>');
         if (txt.value.length > maxSize) 
         {
                txt.value = txt.value.substring(0, maxSize);
         }
            // otherwise, update 'characters left' counter
         else 
         {
                cntField.innerHTML = maxSize - txt.value.length;
          }
}

        function ValidationQC()
        {

          if (document.getElementById('<%=txtQCRemarks.ClientID%>').value.toString().trim().length <= 0)
            {
              msgalert('Please Enter Remarks/Response !');
                document.getElementById('<%=txtQCRemarks.ClientID%>').focus();
                return false;
            }
          return true;
        }



    </script>
<%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>--%>

    <table style="margin:0 auto; width:80%;">
        <tr>
            <td>
                <asp:Label ID="lblHeader" runat="server" SkinID="lblHeading"></asp:Label><br />
                <br />
                <asp:HiddenField ID="HFWorkspaceId" runat="server" />
                <asp:HiddenField ID="HFActivityId" runat="server" />
                <asp:HiddenField ID="HFPath" runat="server" />
                <asp:HiddenField ID="HFParameter" runat="server" />
                <asp:HiddenField ID="HFNodeId" runat="server" />
                <asp:HiddenField ID="HFProjectNo" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <%--<asp:Button ID="btnModalpopup" runat="server" Style="display: none;" />
                <cc1:ModalPopupExtender ID="MPESourceQA" runat="server" PopupControlID="divQCDtl"
                    BehaviorID="MPESourceQA" TargetControlID="btnModalpopup">
                </cc1:ModalPopupExtender>
            <asp:UpdatePanel ID="up1" runat="server">
            <ContentTemplate>--%>
                <div id="mdlDivQc_backgroundElement" class="modalBackground" style="display:none; position: fixed; left: 0px; top: 0px; z-index: 10000; width: 100%; height: 100%;"></div>
                <div style="display: none; left: 10%; width: 82%; position: fixed; top: 15%; height:540px; text-align: left;
                 background-color: Gray; z-index:  10001 !Important; filter: alpha(opacity=70);" 
                    id="divQCDtl" class="centerModalPopup">
                    <table width="100%">
                    <tr>
                        <td>
                            <img id="ImgSeqCancel" alt="Close" src="images/Sqclose.gif" style="position: relative;
                                float: right; right: 5px;" title="Close" onclick="return closesequencediv();" />
                            <asp:Label ID="lblhdr" runat="server" Text="QA Comment" Style="font-weight: bold;
                                color: Black; font-size: 14px; margin-left: 3%; float: left;" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <hr />
                        </td>
                    </tr>
                </table>
                    <asp:Panel ID="pnlQCDtl" runat="server" Width="100%" ScrollBars="Auto">
                        <center>
                        <table>
                            <tbody>
                                <tr class="TR">
                                    <td class="Label" align="left">
                                    </td>
                                    <td align="left">
                                        <asp:RadioButtonList ID="RBLQCFlag" runat="server" Visible="False" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="Y">Approve</asp:ListItem>
                                            <asp:ListItem Value="N">Reject</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="F">Feedback</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr class="TR">
                                    <td class="Label" align="left" colspan="2">
                                        <asp:Label ID="lblResponse" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="TR">
                                    <td style="width: 14%" class="Label" align="left">
                                        Remarks :
                                    </td>
                                    <td style="width: 80%" align="left" colspan="3">
                                        <textarea style="width: 277px" onkeydown="ValidateRemarks(this,'lblcnt',1000);" id="txtQCRemarks"
                                            class="textBox" runat="server"></textarea>
                                        <asp:Label ID="lblcnt" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="TR" style="display: none;">
                                    <td style="width: 14%" class="Label" align="left">
                                        To :
                                    </td>
                                    <td style="width: 80%" align="left" colspan="3">
                                        <asp:TextBox ID="txtToEmailId" runat="server" CssClass="textBox" Width="277px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="TR" style="display: none;">
                                    <td style="width: 14%" class="Label" align="left">
                                        CC :
                                    </td>
                                    <td style="width: 80%" align="left" colspan="3">
                                        <asp:TextBox ID="txtCCEmailId" runat="server" CssClass="textBox" Width="278px" Height="37px"
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="TR">
                                    <td align="left" style="height: 21px">
                                        &nbsp;
                                    </td>
                                    <td class="Label" align="left" colspan="3" style="height: 21px">
                                        <asp:Button ID="BtnQCSave" runat="server" Text="Save" CssClass="btn btnsave"
                                            OnClientClick="return ValidationQC();"></asp:Button>&nbsp;                                        
                                        <input id="Button1" class="btn btnclose" onclick="closesequencediv();" type="button" value="Close"
                                            runat="server" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        </center>
                        <strong style="text-align: left">
                            <br />
                            QA Comments History </strong>
                        <br />
                        <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                        <br /> 
                        <div id="divQCCommentDtl" class ="divQCCommentDtl" style="width: 82%; margin: auto;">
                       

                        <asp:GridView ID="GVQCDtl" runat="server" Font-Size="Small" SkinID="grdViewAutoSizeMax" style="width:auto; margin:auto;"
                            AutoGenerateColumns="False" BorderColor="Peru">
                            <Columns>
                                <asp:BoundField HeaderText="Sr. No."></asp:BoundField>
                                <asp:BoundField DataField="nMedexScreeningHdrQcNo" HeaderText="nMedexScreeningHdrQcNo">
                                </asp:BoundField>
                                <asp:BoundField DataField="vSubjectId" HeaderText="vSubjectId"></asp:BoundField>
                                <asp:BoundField DataField="FullName" HeaderText="Subject">
                                    <ItemStyle Wrap="false" />
                                </asp:BoundField>
                                <asp:BoundField DataField="vQCComment" HeaderText="QA Comments" ></asp:BoundField>
                                <asp:BoundField DataField="cQCFlag" HeaderText="QA"></asp:BoundField>
                                <asp:BoundField DataField="vQCGivenBy" HeaderText="QA BY"></asp:BoundField>
                                <asp:BoundField DataField="ActualTIME" DataFormatString="{0:dd-MMM-yyyy HH:mm}" HeaderText="QA Date" 
                                    HtmlEncode="False">
                                    
                                    <ItemStyle Wrap="true" Width="40px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="vResponse" HeaderText="Response"></asp:BoundField>
                                <asp:BoundField DataField="vResponseGivenBy" HeaderText="Response BY">
                                    <ItemStyle Wrap="false" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ActualTIME2"  DataFormatString="{0:dd-MMM-yyyy HH:mm}" HeaderText="Response Date"
                                    HtmlEncode="False">
                                    <ItemStyle Wrap="true" Width="40px"/>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Response">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkResponse" runat="server">Response</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImgDelete" runat="server" ImageUrl="~/Images/i_delete.gif" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                           
                      </div>
                    </asp:Panel>
                </div>
                
<%--                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="GVQCDtl" EventName="RowCommand" />
                    <asp:AsyncPostBackTrigger ControlID="gvSubjectQC" EventName="RowCommand" />
                </Triggers>
            </asp:UpdatePanel>--%>
            </td>
        </tr>
        <tr>
            <td align="char" style="width: 100%">
                <asp:Label ID="lblActivityName" runat="server" Font-Bold="True" Font-Size="Larger"></asp:Label>
            </td>
        </tr>
        <tr align="left">
            <td>
                <asp:ImageButton ID="Imglockall" runat="server" ImageUrl="~/Images/lockall.jpg" Width="27px" />
                <asp:Label ID="lblLockall" runat="server" Text="Lock All" Font-Bold="True" Width="63px"></asp:Label>
                &nbsp;&nbsp; &nbsp;<asp:ImageButton ID="ImgUnlockall" runat="server" ImageUrl="~/Images/Unlockall.jpg"
                    Width="27px" />
                <asp:Label ID="lblUnlockall" runat="server" Text="Unlock All" Font-Bold="True" Width="91px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="height: 188px">
                <asp:GridView ID="gvSubjectQC" runat="server" PageSize="25" AutoGenerateColumns="False"
                    SkinID="grdViewAutoSizeMax" style="width:70%; margin:auto;">
                    <Columns>
                        <asp:BoundField HeaderText="ASN No">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="vMySubjectNo" HeaderText="Subject No.">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="vSubjectId" HeaderText="Subject Id">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="vInitials" HeaderText="Subject Initials">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="SourceDoc" SortExpression="status">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgSourceDoc" runat="server" ImageUrl="~/Images/item-open.png" ToolTip="SourceDoc"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="LabReport" SortExpression="status">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgLabRpt" runat="server" ImageUrl="~/Images/Labrpt.gif" ToolTip="LabReport"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Lock/Unlock" SortExpression="status">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgStatus" runat="server" ToolTip="Lock/Unlock"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Comments" SortExpression="status">
                            <ItemTemplate>
                                &nbsp;<asp:LinkButton ID="LnkComments" runat="server" ToolTip="Comments"  OnClientClick="$find('MPESourceQA').show();">Comments</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField HeaderText="LOCK">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        </asp:BoundField>

                          
                        <asp:BoundField DataField="nMedExScreeningHdrNo" HeaderText="MedExScreeningHdrNo">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="cRejectionflag" HeaderText="Rejectionflag">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                       
                        <asp:BoundField DataField="FieldToDisplay" HeaderText="Name"  >
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        
                      
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:Button ID="BtnBack" runat="server" CssClass="btn btnback" OnClientClick="closewindow();"
                    Text="" />
            </td>
        </tr>
    </table>

    <script type="text/javascript">
        $(".divQCCommentDtl").mouseover(function () {
            $(this).slimScroll({

                height: '360px',
                size: '8px',
                color: '#0f70bb',
                alwaysVisible: false
            });
        });
    </script>
</asp:Content>

